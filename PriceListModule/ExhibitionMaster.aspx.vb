Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Partial Class PriceListModule_ExhibitionMaster
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim ObjDate As New clsDateTime
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim dpFDate As New TextBox
    Dim dpTDate As New TextBox
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("Exhibition", Request.QueryString("State"))
                ViewState.Add("ExhibitionRefCode", Request.QueryString("RefCode"))
                'If ViewState("Hotelchain") = "New" Then
                fillDategrd(grdDates, False, 5)
                If ViewState("Exhibition") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Exhibition Master"
                    Page.Title = Page.Title + " " + "New Exhibition Master"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("Exhibition") = "Copy" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Copy New Exhibition Master"
                    Page.Title = Page.Title + " " + "Copy Exhibition Master"
                    btnSave.Text = "Save"
                    ShowRecord(CType(ViewState("ExhibitionRefCode"), String))
                    ShowpromDates(CType(ViewState("ExhibitionRefCode"), String))
                    Me.txtCode.Value = ""
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("Exhibition") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Exhibition Master"
                    Page.Title = Page.Title + " " + "Exhibition Master"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("ExhibitionRefCode"), String))
                    ShowpromDates(CType(ViewState("ExhibitionRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    DisableControl()
                ElseIf ViewState("Exhibition") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Exhibition Master"
                    Page.Title = Page.Title + " " + "View Exhibition Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    ShowRecord(CType(ViewState("ExhibitionRefCode"), String))
                    ShowpromDates(CType(ViewState("ExhibitionRefCode"), String))
                    DisableControl()
                ElseIf ViewState("Exhibition") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Exhibition Master"
                    Page.Title = Page.Title + " " + "Delete Exhibition Master"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("ExhibitionRefCode"), String))
                    ShowpromDates(CType(ViewState("ExhibitionRefCode"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Hotelchain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Page.Title = "Exhibition Entry"

    End Sub
#End Region
#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        Dim chckDeletion As CheckBox
        If ViewState("Exhibition") = "View" Or ViewState("Exhibition") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            btnAddLineDates.Enabled = False
            btnDeleteLineDates.Enabled = False
            chkActive.Disabled = True
            For Each gvRow In Me.grdDates.Rows
                dpFDate = gvRow.FindControl("txtfromDate")
                dpFDate.Enabled = False
                dpTDate = gvRow.FindControl("txtToDate")
                dpTDate.Enabled = False
                chckDeletion = gvRow.FindControl("chckDeletion")
                chckDeletion.Enabled = False
            Next
        ElseIf ViewState("ChildState") = "Copy" Then
            dpFDate.Enabled = False
            dpTDate.Enabled = False
        ElseIf ViewState("Exhibition") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub
#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim lblCLineNo As Label
        Dim strPassQry As String = "false"
        Try
            If Page.IsValid = True Then
                
                If ViewState("Exhibition") = "New" Or ViewState("Exhibition") = "Edit" Or ViewState("Exhibition") = "Copy" Then
                    If ViewState("Exhibition") = "New" Or ViewState("Exhibition") = "Edit" Or ViewState("Exhibition") = "Copy" Then
                        If checkForDuplicate() = False Then
                            Exit Sub
                        End If
                    End If

                    If ValidatePage() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    If ViewState("Exhibition") = "New" Or ViewState("Exhibition") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("EXHIBITION", mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim
                    End If
                    'If ViewState("Hotelchain") = "New" Or ViewState("Hotelchain") = "Edit" Then
                    'SQL  Trans start
                    If ViewState("Exhibition") = "New" Or ViewState("Exhibition") = "Copy" Then
                        mySqlCmd = New SqlCommand("sp_add_exhibition", mySqlConn, sqlTrans)
                    ElseIf ViewState("Exhibition") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_exhibition", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.ExecuteNonQuery()
                    '----------------------------------- Deleting Data From subchilddates_detail_new Table
                    mySqlCmd = New SqlCommand("sp_Del_exhibitiondates_new", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    '----------------------------------- Inserting Data To subchilddates_detail Table
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromDate")
                        dpTDate = gvRow.FindControl("txtToDate")
                        lblCLineNo = gvRow.FindControl("lblCLineNo")
                        If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_exhibition_detail_new", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int, 9)).Value = CType(lblCLineNo.Text, Long)
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)

                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    '-----------------------------------------------------------
                ElseIf ViewState("Exhibition") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_exhibitioncode", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CurrenciesSearch.aspx", False)

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CurrWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExhibitionMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "contracts_exhibition_detail", "exhibitioncode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition   is already used for  contracts_exhibition_detail, cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "contracts_exhibition_header", "exhibitioncode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for contracts_exhibition_header  , cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "contracts_exhibition_agents", "exhibitionid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for contracts_exhibition_agents   cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "edit_contracts_exhibition_detail", "exhibitioncode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for edit_contracts_exhibition_detail, cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "contracts_exhibition_countries", "exhibitionid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for contracts_exhibition_countries , cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "edit_contracts_exhibition_agents", "exhibitionid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for  edit_contracts_exhibition_agents  , cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "edit_contracts_exhibition_countries", "exhibitionid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for edit_contracts_exhibition_countries, cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "edit_contracts_exhibition_header", "exhibitionid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for edit_contracts_exhibition_header , cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " edit_edit_contracts_exhibition_header", "exhibitionid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Exhibition is already used for  edit_edit_contracts_exhibition_header , cannot delete this Exhibition');", True)
            checkForDeletion = False
            Exit Function
        End If



        checkForDeletion = True
    End Function
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("Select * from exhibition_master Where exhibitioncode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("exhibitioncode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("exhibitioncode"), String)

                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("exhibitionname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("exhibitionname"), String)
                    Else
                        Me.txtName.Value = ""
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
            objUtils.WritErrorLog("ExhibitionMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region
#Region "Private Sub ShowpromDates(ByVal RefCode As String)"

    Private Sub ShowpromDates(ByVal RefCode As String)
        Try

            Dim lngCnt As Long
            'lngCnt = objUtils.GetDBFieldFromStringnew("exhibition_detail", "count(exhibitioncode)", "exhibitioncode", RefCode)
            lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "exhibition_detail", "count(exhibitioncode)", "exhibitioncode", CType(ViewState("ExhibitionRefCode"), String))
            If lngCnt = 0 Then lngCnt = 1
            fillDategrd(grdDates, False, lngCnt)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("select * from exhibition_detail  where  exhibitioncode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromDate")
                        dpTDate = gvRow.FindControl("txtToDate")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("fromdate")) = False Then
                                ' dpFDate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("txtfromDate")), String)
                                dpFDate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("toDate")) = False Then
                                'dpTDate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("txttxtToDate")), String)
                                dpTDate.Text = CType(Format(CType(mySqlReader("toDate"), Date), "dd/MM/yyyy"), String)
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExhibitionMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("Exhibition") = "New" Or ViewState("Exhibition") = "Copy" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "exhibition_master", "exhibitioncode", txtCode.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This exhibition code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "exhibition_master", "exhibitionname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This exhibition name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("Exhibition") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "exhibition_master", "exhibitioncode", "exhibitionname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This exhibition name name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ' Response.Write("<script language='javascript'> nw=window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=54,left=760,width=250,height=600'); </script>")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExhibitionEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region






    Protected Sub btnAddLineDates_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLines()
    End Sub

    Protected Sub btnDeleteLineDates_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        DeleteLines()
    End Sub

#Region "Private Sub AddLines()"
    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim n As Integer = 0

        Try
            For Each GVRow In grdDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                n = n + 1
            Next


            Dim gridNewrow As GridViewRow
            gridNewrow = grdDates.Rows(grdDates.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdDates.Rows.Count - 1, String) + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#End Region
#Region "Private Sub DeleteLines()"
    Private Sub DeleteLines()
        Dim count As Integer
        Dim chckDeletion As CheckBox

        Dim GVRow As GridViewRow
        count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim n As Integer = 0
        Try
            For Each GVRow In grdDates.Rows
                chckDeletion = GVRow.FindControl("chckDeletion")
                If chckDeletion.Checked = False Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)
                    n = n + 1
                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(grdDates, False, count)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                n = n + 1
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
            objUtils.WritErrorLog("ExhibitionMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
#End Region
    Protected Sub grdDates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDates.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            e.Row.Attributes("onclick") = String.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex)
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event);"
            e.Row.Attributes("onselectstart") = "javascript:return false;"
        End If
    End Sub

#Region "Validate Page"
    Public Function ValidatePage() As Boolean
        Dim flgdt As Boolean = False
        'Dim flgChS As Boolean = False
        'Dim flgChEB As Boolean = False
        Dim flgCk As Boolean = False
        Dim ToDt As Date = Nothing
        Try
            If txtName.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter exhibition  Name');", True)
                SetFocus(dpFDate)
                ValidatePage = False
                Exit Function
            End If


            ''''''''''' Dates Overlapping


         


            Dim dtdatesnew As New DataTable
            Dim dsdates As New DataSet
            Dim dr As DataRow
            Dim xmldates As String = ""




            dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
            dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))


            For Each gvRow1 In grdDates.Rows
                dpFDate = gvRow1.FindControl("txtfromDate")
                dpTDate = gvRow1.FindControl("txtToDate")

                If dpFDate.Text <> "" And dpTDate.Text <> "" Then

                    dr = dtdatesnew.NewRow

                    dr("fromdate") = Format(CType(dpFDate.Text, Date), "yyyy/MM/dd")
                    dr("todate") = Format(CType(dpTDate.Text, Date), "yyyy/MM/dd")
                    dtdatesnew.Rows.Add(dr)

                End If

            Next
            dsdates.Clear()
            If dtdatesnew IsNot Nothing Then
                If dtdatesnew.Rows.Count > 0 Then
                    dsdates.Tables.Add(dtdatesnew)
                    xmldates = objUtils.GenerateXML(dsdates)
                End If
            Else
                xmldates = "<NewDataSet />"
            End If

            Dim strMsg As String = ""
            Dim ds As DataSet
            Dim parms As New List(Of SqlParameter)
            Dim parm(1) As SqlParameter

            parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))
            'parm(1) = New SqlParameter("@contractfromdate", DBNull.Value)
            'parm(2) = New SqlParameter("@contracttodate", DBNull.Value)

            parms.Add(parm(0))

            'For i = 0 To 2
            '    parms.Add(parm(i))
            'Next

            ds = New DataSet()
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
                        strMsg = "Dates Are Overlapping Please check " + "\n"
                        For i = 0 To ds.Tables(0).Rows.Count - 1

                            strMsg += "  Date -  " + ds.Tables(0).Rows(i)("fromdateC") + "\n"
                        Next

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            End If


            '''''''''''''''''

            '--------------------------------------------- Validate Date Grid
            For Each gvRow In grdDates.Rows
                dpFDate = gvRow.FindControl("txtfromDate")
                dpTDate = gvRow.FindControl("txtToDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    If ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text) <= ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(dpTDate)
                        ValidatePage = False
                        Exit Function
                    End If
                    'If ToDt <> Nothing Then
                    '    If ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) <= ToDt Then
                    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date Overlapping.');", True)
                    '        SetFocus(dpFDate)
                    '        ValidatePage = False
                    '        Exit Function
                    '    End If
                    'End If
                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)
                    flgdt = True

                ElseIf dpFDate.Text <> "" And dpTDate.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter To Date.');", True)
                    SetFocus(dpTDate)
                    ValidatePage = False
                    Exit Function
                ElseIf dpFDate.Text = "" And dpTDate.Text <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date.');", True)
                    SetFocus(dpFDate)
                    ValidatePage = False
                    Exit Function
                End If
            Next
            If flgdt = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Dates grid should not be blank.');", True)
                SetFocus(grdDates)
                ValidatePage = False

                Exit Function
            End If
            ValidatePage = True
        Catch ex As Exception
        End Try
    End Function
#End Region
End Class
