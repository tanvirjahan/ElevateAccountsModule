#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class PriceListModule_Guides
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As SqlDataAdapter

#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try

                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If
                btnselect.Visible = True

                btnunselect.Visible = True
                gv_SearchResult.Visible = True


                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If


                ' txtconnection.Value = Session("dbconnectionName")
                ViewState.Add("GuideState", Request.QueryString("State"))
                ViewState.Add("GuideRefCode", Request.QueryString("RefCode"))
                FillGrid("nationalitycode")

                '  txtTrns.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =1001")
                If ViewState("GuideState") = "New" Then
                    SetFocus(txtName)

                    lblHeading.Text = "Add New Guide"

                    btnSave.Text = "Save"
                    btnSave.Focus()


                    btnSave.Attributes.Add("onclick", "return Validate('New')")

                ElseIf ViewState("GuideState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Guide"

                    btnSave.Text = "Update"
                    btnSave.Focus()
                    DisableControl()

                    ShowRecord(CType(ViewState("GuideRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return Validate('Edit')")

                ElseIf ViewState("GuideState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Guide"

                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Other Types"
                    DisableControl()

                    ShowRecord(CType(ViewState("GuideRefCode"), String))

                ElseIf ViewState("GuideState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Guide"

                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("GuideRefCode"), String))

                    btnSave.Attributes.Add("onclick", "return Validate('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")



                '   ValidateOnlyNumber()



                'charcters(txtCode)
                'charcters(txtName)
                'Numbers(txtTel)
                'Numbers(txtMobile)
                'Dim Dt As DataSet = New DataSet

                'Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select nationalitycode As Code,nationalityname As [Language] from nationality_master order by nationalitycode")

                'If Dt.Tables.Count > 0 Then
                '    gv_SearchResult.DataSource = Dt.Tables(0)
                '    gv_SearchResult.DataBind()
                '    btnselect.Visible = True
                '    btnunselect.Visible = True
                'Else
                '    gv_SearchResult.DataSource = Nothing
                '    gv_SearchResult.DataBind()
                '    btnselect.Visible = False
                '    btnunselect.Visible = False

                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Records Found');", True)
                'End If
                'charcters(txtRemark)
                'txtRemark.Attributes.Add("onkeypress", "return checkCharacter(event)")


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("GuideSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
    End Sub
#End Region


#Region "charcters1"
    Public Sub charcters1(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
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
        'currently all chk boxes are not necessary-- so disabled and unchkd (for all conditons)


        If ViewState("GuideState") = "View" Or ViewState("GuideState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True

            txtAddress.Enabled = False
            txtTel.Disabled = True
            txtMobile.Disabled = True
            chkActive.Disabled = True

            btnselect.Visible = False

            btnunselect.Visible = False


            gv_SearchResult.Enabled = False

            'ddlType.Disabled = True
        ElseIf ViewState("GuideState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try

            If txtName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                SetFocus(txtName)
                ValidatePage = False
                Exit Function
            End If

            Dim chkSel As New CheckBox
            Dim FlgCk As Boolean = False

            FlgCk = False
            For Each GvRow In gv_SearchResult.Rows
                chkSel = GvRow.FindControl("chkSelect")
                If chkSel.Checked = True Then
                    FlgCk = True
                    Exit For
                End If
            Next
            If FlgCk = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Any One Known Language');", True)
                ValidatePage = False
                Exit Function
            End If


            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ValidatePage = False
        End Try
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            If Page.IsValid = True Then
                If ViewState("GuideState") = "New" Or ViewState("GuideState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("GuideState") = "New" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("GUIDEMAST", mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim
                        mySqlCmd = New SqlCommand("sp_add_guidemaster", mySqlConn, sqlTrans)
                    ElseIf ViewState("GuideState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_guidemaster", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@guidecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@guidename", SqlDbType.VarChar, 200)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@guideaddress", SqlDbType.VarChar, 500)).Value = CType(txtAddress.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel", SqlDbType.VarChar, 100)).Value = CType(txtTel.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@mobile", SqlDbType.VarChar, 100)).Value = CType(txtMobile.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = CType(txtRemarks.Text.Trim, String)


                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    'mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)


                    mySqlCmd.ExecuteNonQuery()

                    '----------------------------------- Inserting Data To guide_detail Table
                    mySqlCmd = New SqlCommand("sp_del_guidedetails ", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@guidecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    Dim chksel As New CheckBox

                    For Each GvRow In gv_SearchResult.Rows

                        chksel = GvRow.FindControl("chkSelect")
                        If chksel.Checked = True Then

                            mySqlCmd = New SqlCommand("sp_add_guidedetails ", mySqlConn, sqlTrans)
                        
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@guidecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)

                            mySqlCmd.Parameters.Add(New SqlParameter("@nationalitycode", SqlDbType.VarChar, 20)).Value = gv_SearchResult.Rows(GvRow.RowIndex).Cells(1).Text

                            mySqlCmd.ExecuteNonQuery()

                        End If
                    Next




                ElseIf ViewState("GuideState") = "Delete" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_guidedetails", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@guidecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_guidemaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@guidecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('GuideWindowPostBack', '');window.close();window.opener.focus();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("Guides.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from guide_master Where guidecode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("guidecode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("guidecode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("guidename")) = False Then
                        Me.txtName.Value = CType(mySqlReader("guidename"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("guideaddress")) = False Then
                        Me.txtAddress.Text = mySqlReader("guideaddress")
                    Else
                        Me.txtAddress.Text = ""
                    End If
                    If IsDBNull(mySqlReader("tel")) = False Then
                        Me.txtTel.Value = mySqlReader("tel")
                    Else
                        Me.txtTel.Value = ""
                    End If
                    If IsDBNull(mySqlReader("mobile")) = False Then
                        Me.txtMobile.Value = mySqlReader("mobile")
                    Else
                        Me.txtMobile.Value = ""
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        Me.txtRemarks.Text = mySqlReader("remarks")
                    Else
                        Me.txtRemarks.Text = ""
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




            Dim chksel As New CheckBox

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from guide_details Where guidecode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)

            If mySqlReader.HasRows Then
                While mySqlReader.Read() = True

                    If gv_SearchResult.Rows.Count = 0 Then
                        Dim Dt As DataSet = New DataSet

                        Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select n.nationalitycode As [ Language Code],n.nationLanguage As [Language] from nationality_master n left join guide_details g on g.nationalitycode=n.nationalitycode order by n.nationalitycode")

                        If Dt.Tables.Count > 0 Then
                            gv_SearchResult.DataSource = Dt.Tables(0)
                            gv_SearchResult.DataBind()
                            If ViewState("GuideState") = "View" Or ViewState("GuideState") = "Delete" Then
                                btnselect.Visible = False
                                btnunselect.Visible = False
                            Else
                                btnselect.Visible = True
                                btnunselect.Visible = True
                            End If


                        Else
                            gv_SearchResult.DataSource = Nothing
                            gv_SearchResult.DataBind()
                            btnselect.Visible = False
                            btnunselect.Visible = False

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Records Found');", True)
                        End If
                    End If
                    Try
                        For Each GvRow In gv_SearchResult.Rows
                            If CType(mySqlReader("nationalitycode"), String) = GvRow.Cells(1).Text.ToString Then
                                chksel = GvRow.FindControl("chkSelect")
                                chksel.Checked = True
                                Exit For
                            End If
                        Next
                    Catch ex As Exception
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
                    End Try

                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Guides.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("RoutesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        'Dim strFilter As String
        'strFilter = "othgrpcode= (select option_selected  from reservation_parameters where param_id =1001)"

        If ViewState("GuideState") = "New" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "guide_master", "guidecode", CType(txtCode.Value.Trim, String)) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This guide code is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "guide_master", "guidename", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This guide name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If

        ElseIf ViewState("GuideState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "guide_master", "guidecode", "guidename", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This guide name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If

        End If

        checkForDuplicate = True
    End Function
#End Region





    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Guides','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselect.Click
        Try
            Dim chksel As New CheckBox
            For Each GvRow In gv_SearchResult.Rows
                chksel = GvRow.FindControl("chkSelect")
                chksel.Checked = True
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub


    Protected Sub btnunselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnunselect.Click
        Try
            Dim chksel As New CheckBox
            For Each GvRow In gv_SearchResult.Rows
                chksel = GvRow.FindControl("chkSelect")
                chksel.Checked = False
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub
    Protected Sub btnshowall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnshowall.Click
        Dim myDS As New DataSet
        Dim rec As New ArrayList
        Dim i As Integer
        gv_SearchResult.Visible = True
        strSqlQry = ""
        Try

            Try
                Dim chksel As New CheckBox
                For Each GvRow In gv_SearchResult.Rows
                    chksel = GvRow.FindControl("chkSelect")
                    If chksel.Checked = True Then
                        rec.Add(GvRow.Cells(1).Text.ToString)
                    End If
                Next
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
            End Try
            strSqlQry = "select nationalitycode, nationLanguage from nationality_master order by nationalitycode ASC"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS
            gv_SearchResult.DataBind()

            Try
                Dim chksel As New CheckBox
                For Each GvRow In gv_SearchResult.Rows

                    For i = 0 To rec.Count - 1
                        If rec.Item(i) = GvRow.Cells(1).Text.ToString Then
                            chksel = GvRow.FindControl("chkSelect")
                            chksel.Checked = True
                        End If
                    Next

                Next
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
            End Try
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
       
     
    End Sub
    Protected Sub btnshowselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnshowselect.Click
        Dim dt As New DataTable
        Dim drow As DataRow
        Dim flag As Integer
        flag = 0
        Try

            dt.Columns.Add("nationalitycode", GetType(String))
            dt.Columns.Add("nationLanguage", GetType(String))
            Dim chksel As New CheckBox

            For Each GvRow In gv_SearchResult.Rows
                chksel = GvRow.FindControl("chkSelect")
                If chksel.Checked = True Then
                    drow = dt.NewRow()
                    drow.Item("nationalitycode") = GvRow.Cells(1).Text.ToString
                    drow.Item("nationLanguage") = GvRow.Cells(2).Text.ToString
                    dt.Rows.Add(drow)
                    flag = flag + 1
               
                End If
            Next

            If flag <> 0 Then

                gv_SearchResult.Visible = True
                gv_SearchResult.DataSource = dt
                gv_SearchResult.DataBind()
                For Each GvRow In gv_SearchResult.Rows
                    chksel = GvRow.FindControl("chkSelect")
                    chksel.Checked = True

                Next
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub


#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        'lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            'strSqlQry = "select nationalitycode, nationalityname from nationality_master "
            strSqlQry = "select nationalitycode, nationLanguage from nationality_master "
            'nationLanguage
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else

                gv_SearchResult.DataBind()
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Guides.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try

    End Sub
#End Region

    'Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
    '    Try
    '        If e.Row.RowType = DataControlRowType.Header Then
    '            e.Row.Cells(1).Visible = False
    '        End If
    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            e.Row.Cells(1).Visible = False
    '            If ViewState("GuideState") = "View" Or ViewState("GuideState") = "Delete" Then
    '                e.Row.Cells(0).Enabled = False
    '            End If
    '        End If

    '    Catch ex As Exception

    '    End Try
    'End Sub


End Class