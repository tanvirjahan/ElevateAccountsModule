
'------------================--------------=======================------------------================
'   Page Name       :   SupplierType.aspx
'   Developer Name  :    Pramod Desai
'   Date            :    14 June 2008
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic


Partial Class ExcursionTypesMaster
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
                    lblHeading.Text = "Add New - Excursion   Types"
                    Page.Title = Page.Title + " " + "New Excursion   Types"
                    btnSave.Text = "Save"
                    txtCode.ReadOnly = True
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    'End If
                ElseIf Session("ExcTypesState") = "Edit" Then
                    SetFocus(txtName)
                    txtCode.ReadOnly = True
                    ' lblHeading.Text = "Edit Supplier Category"
                    Page.Title = Page.Title + " " + "Edit Excursion Types"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf Session("ExcTypesState") = "Copy" Then
                    SetFocus(txtName)
                    txtCode.ReadOnly = True
                    ' lblHeading.Text = "Edit Supplier Category"
                    lblHeading.Text = "Add New - Excursion   Types"
                    btnSave.Text = "Save"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf Session("ExcTypesState") = "View" Then
                    SetFocus(btnCancel)
                    txtCode.ReadOnly = True
                    ' lblHeading.Text = "View Supplier Category"
                    Page.Title = Page.Title + " " + "View Excursion Types"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Supplier Category"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                ElseIf Session("ExcTypesState") = "Delete" Then
                    SetFocus(btnSave)
                    txtCode.ReadOnly = True
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



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionTypesMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
            Page.Title = "Excursion Types Entry"
        End If
    End Sub

    Private Sub BindCombogrid(Optional ByVal strCode As String = "")
        Dim dt As New DataTable
        Dim strQuery As String = ""
        If strCode = "" Then
            ' strQuery = "select ec.exctypcombocode,ec.exctypcode,e.exctypname,e.combo from excursiontypes_combodetails(nolock) ec,excursiontypes(nolock)e where ec.exctypcode='00' and ec.exctypcode=e.exctypcode"
            strQuery = "select ec.exctypcombocode exctypcode,e.exctypname,exctypcombocode from excursiontypes_combodetails(nolock) ec,excursiontypes(nolock)e where ec.exctypcode='00' and  ec.exctypcombocode=e.exctypcode"
        Else
            'strQuery = "select ec.exctypcombocode,ec.exctypcode,e.exctypname,e.combo from excursiontypes_combodetails(nolock) ec,excursiontypes(nolock)e where ec.exctypcode='" & strCode & "' and ec.exctypcode=e.exctypcode "
            strQuery = "select ec.exctypcombocode exctypcode,e.exctypname,exctypcombocode from excursiontypes_combodetails(nolock) ec,excursiontypes(nolock)e where ec.exctypcode='" & strCode & "' and  ec.exctypcombocode=e.exctypcode"
        End If

        Dim MyAdapter As SqlDataAdapter
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strQuery, mySqlConn)
        MyAdapter.Fill(dt)

        If dt.Rows.Count > 0 Then
            gvCombo.DataSource = dt
            gvCombo.DataBind()
        Else
            dt.Rows.Add()
            dt.Rows(0)("exctypcombocode") = "0"
            gvCombo.DataSource = dt
            gvCombo.DataBind()
        End If

    End Sub

    Private Sub DisableControl()
        If Session("ExcTypesState") = "View" Or Session("ExcTypesState") = "Delete" Then
            txtCode.Enabled = False
            txtName.Enabled = False
            ddlautoconf.Enabled = False
            ddlentrytkt.Enabled = False
            ddlratebasis.Enabled = False
            ddlsicpri.Enabled = False
            ddlstarcat.Enabled = False
            ddltktbased.Enabled = False
            ddltransinc.Enabled = False
            ddlCombo.Enabled = False
            ddlMultipleDates.Enabled = False
            txtclassificationname.Enabled = False
            chkactive.Disabled = True
            chkmealinc.Disabled = True
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
                    If Session("ExcTypesState") <> "Copy" Then
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
                 
                    If IsDBNull(mySqlReader("classificationcode")) = False Then

                        txtclassificationcode.Text = CType(mySqlReader("classificationcode"), String)
                        txtclassificationname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excclassification_header", "classificationname", "classificationcode", mySqlReader("classificationcode"))

                    Else
                        'ddlSupplierType.Value = "[Select]"
                        'ddlSupplierTypeName.Value = "[Select]"
                        txtclassificationname.Text = ""
                        txtclassificationcode.Text = ""

                    End If
                    If IsDBNull(mySqlReader("autoconfirm")) = False Then
                        ddlautoconf.SelectedValue = CType(mySqlReader("autoconfirm"), String)
                    Else
                        ddlautoconf.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("ratebasis")) = False Then
                        ddlratebasis.SelectedValue = CType(mySqlReader("ratebasis"), String)
                    Else
                        ddlratebasis.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("entrytktreqd")) = False Then
                        ddlentrytkt.SelectedValue = CType(mySqlReader("entrytktreqd"), String)
                    Else
                        ddlentrytkt.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("tktbasedontime")) = False Then
                        ddltktbased.SelectedValue = CType(mySqlReader("tktbasedontime"), String)
                    Else
                        ddltktbased.SelectedValue = "[Select]"
                    End If


                    If IsDBNull(mySqlReader("transferincl")) = False Then
                        ddltransinc.SelectedValue = CType(mySqlReader("transferincl"), String)
                    Else
                        ddltransinc.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("starcat")) = False Then
                        ddlstarcat.SelectedValue = CType(mySqlReader("starcat"), String)
                    Else
                        ddlstarcat.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("sicpvt")) = False Then
                        ddlsicpri.SelectedValue = CType(mySqlReader("sicpvt"), String)
                    Else
                        ddlsicpri.SelectedValue = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("sectorwiserates")) = False Then
                        ddlsectorwise.SelectedValue = CType(mySqlReader("sectorwiserates"), String)
                    Else
                        ddlsectorwise.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("multicost")) = False Then
                        ddlmultiplecost.SelectedValue = CType(mySqlReader("multicost"), String)
                    Else
                        ddlmultiplecost.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("combo")) = False Then
                        ddlCombo.SelectedValue = CType(mySqlReader("combo"), String)
                    Else
                        ddlCombo.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("multipledatesyesno")) = False Then
                        ddlMultipleDates.SelectedValue = CType(mySqlReader("multipledatesyesno"), String)
                    Else
                        ddlMultipleDates.SelectedValue = "[Select]"
                    End If
                    'If IsDBNull(mySqlReader("rankorder")) = False Then
                    '    Me.txtOrder.Value = CType(mySqlReader("rankorder"), String)
                    'Else
                    '    Me.txtOrder.Value = ""
                    'End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1 " Then
                            chkactive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkactive.Checked = False
                        End If
                    End If


                    If IsDBNull(mySqlReader("withmealop")) = False Then
                        If CType(mySqlReader("withmealop"), String) = "1" Then
                            chkmealinc.Checked = True
                        ElseIf CType(mySqlReader("withmealop"), String) = "0" Then
                            chkmealinc.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("preferred")) = False Then
                        If CType(mySqlReader("preferred"), String) = "1" Then
                            chkprefer.Checked = True
                        ElseIf CType(mySqlReader("preferred"), String) = "0" Then
                            chkprefer.Checked = False
                        End If
                    End If


                    If ddlCombo.SelectedValue = "YES" Then
                        BindCombogrid(txtCode.Text)
                    Else
                        gvCombo.DataBind()

                    End If


                End If
                End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypesMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
    Function CheckExcursion() As Boolean
        CheckExcursion = True
        Dim MyAdapter As SqlDataAdapter
        Dim ds As New DataSet
        Dim strMsg As String = ""
        Try
            If ddlsectorwise.SelectedValue = "NO" Then

                strSqlQry = "select distinct h.eplistcode , 'Selling PriceList' Options from excplist_header h(nolock), excplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(sectoryesno,0)=1 and d.exccode='" & txtCode.Text.Trim & "' union all  " _
                            & " select distinct h.eplistcode,' Cost Price List' Options from exccplist_header h(nolock), exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(sectoryesno,0)=1 and d.exccode='" & txtCode.Text.Trim & "'"




                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(ds, "showsectors")


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then
                            strMsg = "For this Excursion Already Entered Sectorwise Price list " + "\n"

                            For i = 0 To ds.Tables(0).Rows.Count - 1

                                strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                            Next

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            CheckExcursion = False
                            Exit Function
                        End If
                    End If
                End If
            ElseIf ddlsectorwise.SelectedValue = "YES" Then

                strSqlQry = "select distinct h.eplistcode , 'Selling PriceList' Options from excplist_header h(nolock), excplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(sectoryesno,0)=0 and d.exccode='" & txtCode.Text.Trim & "' union all  " _
                            & " select distinct h.eplistcode,' Cost Price List' Options from exccplist_header h(nolock), exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(sectoryesno,0)=0 and d.exccode='" & txtCode.Text.Trim & "'"




                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(ds, "showsectors")


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then
                            strMsg = "For this Excursion Already Entered  Price list " + "\n"

                            For i = 0 To ds.Tables(0).Rows.Count - 1

                                strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                            Next

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            CheckExcursion = False
                            Exit Function
                        End If
                    End If
                End If

            End If


            If ddlmultiplecost.SelectedValue = "NO" Then

                strSqlQry = "select distinct h.eplistcode , 'MultiCostPriceList' Options from excmulticplist_header h(nolock) where h.exctypcode='" & txtCode.Text.Trim & "'"





                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(ds, "showsectors")


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then
                            strMsg = "For this Excursion Already Entered Multicost Price list " + "\n"

                            For i = 0 To ds.Tables(0).Rows.Count - 1

                                strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                            Next

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            CheckExcursion = False
                            Exit Function
                        End If
                    End If
                End If

            ElseIf ddlmultiplecost.SelectedValue = "YES" Then

                strSqlQry = "select distinct h.eplistcode , 'Selling PriceList' Options from excplist_header h(nolock), excplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(sectoryesno,0)=0 and d.exccode='" & txtCode.Text.Trim & "' union all  " _
                            & " select distinct h.eplistcode,' Cost Price List' Options from exccplist_header h(nolock), exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(sectoryesno,0)=0 and d.exccode='" & txtCode.Text.Trim & "'"




                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(ds, "showsectors")


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then
                            strMsg = "For this Excursion Already Entered  Price list " + "\n"

                            For i = 0 To ds.Tables(0).Rows.Count - 1

                                strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                            Next

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            CheckExcursion = False
                            Exit Function
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypesMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strPassQry As String = "false"
        Dim frmmode As String = 0
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        'If Session("ExcTypesState") Is Nothing Then
        '    Session.Add("ExcTypesState", Request.QueryString("State"))
        '    Session.Add("ExcTypesRefCode", Request.QueryString("RefCode"))
        'End If
        Try
            If Page.IsValid = True Then
                If Session("ExcTypesState") = "New" Or Session("ExcTypesState") = "Edit" Or Session("ExcTypesState") = "Copy" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If

                    If CheckExcursion() = False Then
                        Exit Sub
                    End If

                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    If ddlmultiplecost.SelectedValue = "YES" Then
                        If ddlratebasis.SelectedValue <> "UNIT" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('If multicost selected, rate basis should be UNIT.');", True)
                            Exit Sub
                        End If
                    End If
                    'If ddlCombo.SelectedValue = "YES" And ddlMultipleDates.SelectedValue = "YES" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Combo and Multiple dates both cannot be YES, it should be either or.');", True)
                    '    Exit Sub
                    'End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If Session("ExcTypesState") = "New" Or Session("ExcTypesState") = "Copy" Then
                        mySqlCmd = New SqlCommand("sp_add_exctypmaster", mySqlConn, sqlTrans)

                        frmmode = 1

                        lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='EXCTYP'")
                        optionval = objUtils.GetAutoDocNo("EXCTYP", mySqlConn, sqlTrans)
                        txtCode.Text = optionval.Trim

                    ElseIf Session("ExcTypesState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_exctypmaster", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypname", SqlDbType.VarChar, 150)).Value = CType(txtName.Text.Trim, String)   ''' changed shahul 16/07/18
                    mySqlCmd.Parameters.Add(New SqlParameter("@classificationcode", SqlDbType.VarChar, 20)).Value = CType(txtclassificationcode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ratebasis", SqlDbType.VarChar, 20)).Value = CType(ddlratebasis.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@entrytktreqd", SqlDbType.VarChar, 100)).Value = CType(ddlentrytkt.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tktbasedontime", SqlDbType.VarChar, 100)).Value = CType(ddltktbased.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@autoconfirm", SqlDbType.VarChar, 20)).Value = CType(ddlautoconf.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@transferincl", SqlDbType.VarChar, 20)).Value = CType(ddltransinc.SelectedValue.Trim, String)
                    If CType(ddlstarcat.SelectedValue.Trim, String) = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@starcat", SqlDbType.Int)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@starcat", SqlDbType.Int)).Value = CType(ddlstarcat.SelectedValue.Trim, Integer)
                    End If

                    If chkmealinc.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@withmealop", SqlDbType.Int)).Value = 1
                    ElseIf chkmealinc.Checked = False Then

                        mySqlCmd.Parameters.Add(New SqlParameter("@withmealop", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@sicpvt", SqlDbType.VarChar, 25)).Value = CType(ddlsicpri.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorwiserates", SqlDbType.VarChar, 10)).Value = CType(ddlsectorwise.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@multicost", SqlDbType.VarChar, 10)).Value = CType(ddlmultiplecost.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@combo", SqlDbType.VarChar, 10)).Value = CType(ddlCombo.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@multipledatesyesno", SqlDbType.VarChar, 10)).Value = CType(ddlMultipleDates.SelectedValue.Trim, String)
                    If chkactive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkactive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)



                    Dim strExcComboDetails As New StringBuilder
                    strExcComboDetails.Append("<DocumentElement>")

                    If gvCombo.Rows.Count > 0 And ddlCombo.SelectedValue = "YES" Then
                        For Each gvrow As GridViewRow In gvCombo.Rows
                            Dim lblId As Label = CType(gvrow.FindControl("lblId"), Label)
                            Dim chkSelect As CheckBox = CType(gvrow.FindControl("chkSelect"), CheckBox)
                            Dim txtExcursionType As TextBox = CType(gvrow.FindControl("txtExcursionType"), TextBox)
                            Dim txtExcursionTypeCode As TextBox = CType(gvrow.FindControl("txtExcursionTypeCode"), TextBox)
                            If txtExcursionTypeCode.Text <> "" Then
                                strExcComboDetails.Append("<Table>")
                                strExcComboDetails.Append("<exctypcombocode>" & txtExcursionTypeCode.Text.Trim & "</exctypcombocode>")
                                strExcComboDetails.Append("</Table>")
                            End If

                        Next
                    End If
                   
                    strExcComboDetails.Append("</DocumentElement>")
                    mySqlCmd.Parameters.Add(New SqlParameter("@ExcComboDetails", SqlDbType.VarChar, 10000)).Value = strExcComboDetails.ToString
                    If chkprefer.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int)).Value = 1
                    ElseIf chkprefer.Checked = False Then

                        mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int)).Value = 0
                    End If
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

                If Session("ExcTypesState") = "New" Or Session("ExcTypesState") = "Copy" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)

                    Session.Add("ExcTypesState", "Edit")



                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('ExcursionSuppliersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                    'txtCode.Value = txtCode.Value 'added by sribish


                ElseIf Session("ExcTypesState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    Session.Add("ExcTypesState", "Edit")

                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('ExcursionSuppliersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                End If


                If Session("ExcTypesState") = "Delete" Then

                    Dim strscript As String = ""

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Deleted Successfully.');", True)

                    strscript = "window.opener.__doPostBack('ExcursionSuppliersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If

            End If

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypesMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("SupplierCategoriesSearch.aspx", False)

        Dim strscript1 As String = ""
        strscript1 = "window.opener.__doPostBack('ExcursionSuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
    Public Function checkForDuplicate() As Boolean
        If Session("ExcTypesState") = "New" Or Session("ExcTypesState") = "Copy" Then
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
            If txtclassificationname.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Classification Name.');", True)
                SetFocus(txtclassificationname.Text)
                ValidatePage = False
                Exit Function
            End If
            If ddlautoconf.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Auto Confirm.');", True)
                SetFocus(ddlautoconf)
                ValidatePage = False
                Exit Function
            End If

            If ddlentrytkt.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Entry Ticket.');", True)
                SetFocus(ddlentrytkt)
                ValidatePage = False
                Exit Function
            End If
            If ddlratebasis.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Rate Basis.');", True)
                SetFocus(ddlratebasis)
                ValidatePage = False
                Exit Function
            End If
            If ddlsicpri.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select SIC/PRIVATE.');", True)
                SetFocus(ddlsicpri)
                ValidatePage = False
                Exit Function
            End If
            If ddltktbased.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Ticket Based On Time');", True)
                SetFocus(ddltktbased)
                ValidatePage = False
                Exit Function
            End If
            If ddlCombo.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Combo');", True)
                SetFocus(ddltktbased)
                ValidatePage = False
                Exit Function
            End If
            '' Added shahul 26/05/18
            'If ddlCombo.SelectedValue = "YES" And ddlmultiplecost.SelectedValue = "YES" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Can not  Select Combo Tours as Multiple Cost Yes');", True)
            '    SetFocus(ddlmultiplecost)
            '    ValidatePage = False
            '    Exit Function
            'End If

            If ddlMultipleDates.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select multiple date field');", True)
                SetFocus(ddltktbased)
                ValidatePage = False
                Exit Function
            End If
            'If ddltransinc.SelectedValue = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Transfer Included');", True)
            '    SetFocus(ddltransinc)
            '    ValidatePage = False
            '    Exit Function
            'End If

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypesMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=excursionmain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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

    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GeExcursionType(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select exctypcode,exctypname from excursiontypes(nolock) where active=1 and isnull(combo,'')<>'YES' and exctypname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("exctypname").ToString(), myDS.Tables(0).Rows(i)("exctypcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function


    Protected Sub ddlCombo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCombo.SelectedIndexChanged
        If ddlCombo.SelectedValue = "YES" Then
            BindCombogrid(txtCode.Text)
        Else
            gvCombo.DataBind()

        End If
    End Sub

    Protected Sub btnAddrow_Click(sender As Object, e As System.EventArgs) Handles btnAddrow.Click

        Dim dt As DataTable = CreateComboDataSource()


        If gvCombo.Rows.Count > 0 Then

            For Each gvrow As GridViewRow In gvCombo.Rows
                Dim lblId As Label = CType(gvrow.FindControl("lblId"), Label)
                Dim chkSelect As CheckBox = CType(gvrow.FindControl("chkSelect"), CheckBox)
                Dim txtExcursionType As TextBox = CType(gvrow.FindControl("txtExcursionType"), TextBox)
                Dim txtExcursionTypeCode As TextBox = CType(gvrow.FindControl("txtExcursionTypeCode"), TextBox)
                Dim dr As DataRow
                dr = dt.NewRow()
                dr("exctypname") = txtExcursionType.Text
                dr("exctypcode") = txtExcursionTypeCode.Text
                dr("exctypcombocode") = txtExcursionTypeCode.Text
                dt.Rows.Add(dr)
            Next
            Dim dr1 As DataRow
            dr1 = dt.NewRow()
            dr1("exctypname") = ""
            dr1("exctypcode") = ""
            dr1("exctypcombocode") = ""
            dt.Rows.Add(dr1)

            gvCombo.DataSource = dt
            gvCombo.DataBind()

        End If
    End Sub


    Protected Sub btnRowDelete_Click(sender As Object, e As System.EventArgs) Handles btnRowDelete.Click
        Dim dt As DataTable = CreateComboDataSource()


        If gvCombo.Rows.Count > 0 Then
            For Each gvrow As GridViewRow In gvCombo.Rows
                Dim lblId As Label = CType(gvrow.FindControl("lblId"), Label)
                Dim chkSelect As CheckBox = CType(gvrow.FindControl("chkSelect"), CheckBox)
                Dim txtExcursionType As TextBox = CType(gvrow.FindControl("txtExcursionType"), TextBox)
                Dim txtExcursionTypeCode As TextBox = CType(gvrow.FindControl("txtExcursionTypeCode"), TextBox)
                If chkSelect.Checked = False Then
                    Dim dr As DataRow
                    dr = dt.NewRow()
                    dr("exctypname") = txtExcursionType.Text
                    dr("exctypcode") = txtExcursionTypeCode.Text
                    dr("exctypcombocode") = txtExcursionTypeCode.Text
                    dt.Rows.Add(dr)
                End If
        
            Next

            gvCombo.DataSource = dt
            gvCombo.DataBind()

            If gvCombo.Rows.Count = 0 Then
                Dim dt1 As DataTable = CreateComboDataSource()
                Dim dr1 As DataRow
                dr1 = dt1.NewRow()
                dr1("exctypname") = ""
                dr1("exctypcode") = ""
                dr1("exctypcombocode") = ""
                dt1.Rows.Add(dr1)

                gvCombo.DataSource = dt1
                gvCombo.DataBind()
            End If


        End If

       
    End Sub

    Private Function CreateComboDataSource() As DataTable
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("exctypcombocode", GetType(String)))
        dt.Columns.Add(New DataColumn("exctypname", GetType(String)))
        dt.Columns.Add(New DataColumn("exctypcode", GetType(String)))
        'dt.Columns.Add(New DataColumn("adddate", GetType(String)))
        'dt.Columns.Add(New DataColumn("adduser", GetType(String)))
        'dt.Columns.Add(New DataColumn("moddate", GetType(String)))
        'dt.Columns.Add(New DataColumn("moduser", GetType(String)))
        'return a DataView to the DataTable
        CreateComboDataSource = dt
    End Function

End Class


