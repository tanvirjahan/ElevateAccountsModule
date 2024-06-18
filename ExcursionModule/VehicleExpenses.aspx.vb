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
    Dim objectcl As New clsDateTime
    Dim gvRow As GridViewRow
    Dim ObjDate As New clsDateTime
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If txtDate.Text = "" Then
            txtDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        End If
        txtconnection.Value = Session("dbconnectionName")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldcode, "drivercode", "select drivercode,drivername from drivermaster where active=1  order by drivercode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldname, "drivername", "select drivername,drivercode from drivermaster where active=1  order by drivername", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldcode, "drivercode", "drivername", "select drivercode,drivername from drivermaster where active=1 order by drivercode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldname, "drivername", "drivercode", "select drivername,drivercode from drivermaster where active=1 order by drivername", True)

                ViewState.Add("excServiceSelltypeState", Request.QueryString("State"))
                ViewState.Add("excServiceSelltypeRefCode", Request.QueryString("RefCode"))

                txtexpid.Enabled = False
                txttot.Enabled = False


                If ViewState("excServiceSelltypeState") = "New" Then
                    SetFocus(txtexpid)

                    lblHeading.Text = "Add New - Vehicle Expense"
                    Page.Title = Page.Title + " " + "New Vehicle Expense"
                    btnSave.Text = "Save"
                    If ddldcode.Value <> "[Select]" Then
                        fillgridnew(grdRecord, True, 10)

                    End If
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Vehicle Expense?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("excServiceSelltypeState") = "Edit" Then

                    lblHeading.Text = "Edit -Vehicle Expense"
                    Page.Title = Page.Title + " " + "Edit Vehicle Expense"
                    btnSave.Text = "Update"
                    DisableControl()

                    

                    show_record(CType(ViewState("excServiceSelltypeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Vehicle Expense?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("excServiceSelltypeState") = "View" Then
                    SetFocus(btnCancel)



                    lblHeading.Text = "View - Vehicle Expense"
                    Page.Title = Page.Title + " " + "View Vehicle Expense"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    show_record(CType(ViewState("excServiceSelltypeRefCode"), String))

                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete - Vehicle Expense"


                    btnSave.Text = "Delete"
                    DisableControl()

                    show_record(CType(ViewState("excServiceSelltypeRefCode"), String))
                    '   btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Vehicle Expense?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddldcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddldname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnprint.Visible = False
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
            txtexpid.Enabled = False

            ddldcode.Disabled = True
            ddldname.Disabled = True
            grdRecord.Enabled = False
            txtremks.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = False
            txtDate.Enabled = False
            btnloadgrid.Enabled = False
        ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
            txtexpid.Enabled = False
            ddldcode.Disabled = False
            ddldname.Disabled = False
            grdRecord.Enabled = True
            txtremks.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = True
            txtDate.Enabled = True
            btnloadgrid.Enabled = True
        End If
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim gvrow1 As New GridViewRow
        Dim ddlvehcode As New HtmlSelect
        Dim i As Integer = 0
        Try


            For Each gvRow In grdRecord.Rows
                ddlvehcode = gvRow.FindControl("ddlvehcode")
                If ddlvehcode.Items(ddlvehcode.SelectedIndex).Text = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Vehicle code.');", True)
                    SetFocus(ddlvehcode)
                    ValidatePage = False
                    Exit Function
                End If

                i = i + 1
            Next


            If ddldcode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Driver code.');", True)
                SetFocus(ddldcode)
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
        Dim intRow As Integer = 0
        Dim introw1 As Integer = 0
        Dim lblLineNo As New Label
        Dim txtgriddate As New TextBox
        Dim ddlcode As New HtmlSelect
        Dim ddlexpname As New HtmlSelect
        Dim ddlexptype As New HtmlSelect
        Dim ddlreqid As New DropDownList 'HtmlSelect
        Dim txtamount As New HtmlInputText
        'Dim txtremarks As New HtmlInputText
        Dim txtremarks As New HtmlTextArea
        Dim ddlvehcode As New HtmlSelect
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

                    Dim optionval As String
                    If txtexpid.Text = "" Then
                        optionval = objUtils.GetAutoDocNo("VEH.EXP", mySqlConn, sqlTrans)
                        txtexpid.Text = optionval.Trim
                    End If

                    If ViewState("excServiceSelltypeState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_expense_master", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_expense_master", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@exid", SqlDbType.VarChar, 20)).Value = CType(txtexpid.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@exdate", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                    mySqlCmd.Parameters.Add(New SqlParameter("@driver", SqlDbType.VarChar, 20)).Value = CType(ddldcode.Items(ddldcode.SelectedIndex).Text, String) 'CType(ddldcode.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@vehiclecode", SqlDbType.VarChar, 20)).Value = "" 'vehiclecode selection in detail
                    mySqlCmd.Parameters.Add(New SqlParameter("@expensevalue", SqlDbType.VarChar, 20)).Value = txttot.Text
                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 200)).Value = txtremks.Text

                    If ViewState("excServiceSelltypeState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = ""


                    mySqlCmd.ExecuteNonQuery()

                    'deletfrom detail table 
                    mySqlCmd = New SqlCommand("sp_del_expense_detail", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exid", SqlDbType.VarChar, 20)).Value = txtexpid.Text
                    mySqlCmd.ExecuteNonQuery()


                    'Save In Detail Table
                    For Each gvRow In grdRecord.Rows
                        intRow = 1 + intRow



                        If Val(lblLineNo.Text) = 0 Then
                            lblLineNo.Text = intRow
                        End If

                        lblLineNo = gvRow.FindControl("lblLineNo")
                        txtgriddate = gvRow.FindControl("txtgriddate")
                        ddlcode = gvRow.FindControl("ddlcode")

                        ddlexpname = gvRow.FindControl("ddlexpname")
                        ddlexptype = gvRow.FindControl("ddlexptype")
                        ddlreqid = gvRow.FindControl("ddlreqid")
                        txtamount = gvRow.FindControl("txtamount")
                        txtremarks = gvRow.FindControl("txtremarks")
                        ddlvehcode = gvRow.FindControl("ddlvehcode")

                        Dim strreqid As String = ""

                        If ddlreqid.SelectedIndex = "-1" Then
                            strreqid = ""
                        Else
                            strreqid = CType(ddlreqid.SelectedItem.Text, String)
                        End If


                        mySqlCmd = New SqlCommand("sp_add_expense_detail", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@exid", SqlDbType.VarChar, 20)).Value = CType(txtexpid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@vlineno", SqlDbType.Int)).Value = intRow 'intRow
                        mySqlCmd.Parameters.Add(New SqlParameter("@exdate", SqlDbType.VarChar, 20)).Value = Format(CType(txtgriddate.Text, Date), "yyyy/MM/dd") 'ddlvehiclename.Value 
                        mySqlCmd.Parameters.Add(New SqlParameter("@expensecode ", SqlDbType.VarChar, 20)).Value = ddlcode.Items(ddlcode.SelectedIndex).Text


                        mySqlCmd.Parameters.Add(New SqlParameter("@transfermode", SqlDbType.VarChar, 100)).Value = ddlexptype.Value 'IIf(ddlexptype.Value = "Transfers", 0, IIf(ddlexptype.Value = "Safari", 1, 2))
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = strreqid

                        mySqlCmd.Parameters.Add(New SqlParameter("@units", SqlDbType.Decimal, 10)).Value = 1
                        mySqlCmd.Parameters.Add(New SqlParameter("@expcurrency", SqlDbType.Decimal, 10)).Value = txtamount.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@expvalue", SqlDbType.Decimal, 10)).Value = txtamount.Value

                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks ", SqlDbType.VarChar, 200)).Value = txtremarks.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@excid ", SqlDbType.VarChar, 20)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@vehiclecode ", SqlDbType.VarChar, 20)).Value = ddlvehcode.Items(ddlvehcode.SelectedIndex).Text
                        mySqlCmd.ExecuteNonQuery()

                    Next

                    'deletfrom detail table 
                    mySqlCmd = New SqlCommand("sp_del_expense_detail_request", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exid", SqlDbType.VarChar, 20)).Value = txtexpid.Text
                    mySqlCmd.ExecuteNonQuery()

                    'Save In expense_detail_request
                    For Each gvRow In grdRecord.Rows
                        introw1 = 1 + introw1



                        If Val(lblLineNo.Text) = 0 Then
                            lblLineNo.Text = intRow
                        End If

                        lblLineNo = gvRow.FindControl("lblLineNo")
                        txtgriddate = gvRow.FindControl("txtgriddate")
                        ddlcode = gvRow.FindControl("ddlcode")

                        ddlexpname = gvRow.FindControl("ddlexpname")
                        ddlexptype = gvRow.FindControl("ddlexptype")
                        ddlreqid = gvRow.FindControl("ddlreqid")
                        txtamount = gvRow.FindControl("txtamount")
                        txtremarks = gvRow.FindControl("txtremarks")
                        ddlvehcode = gvRow.FindControl("ddlvehcode")




                        mySqlCmd = New SqlCommand("sp_add_expense_detail_request", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@exid", SqlDbType.VarChar, 20)).Value = CType(txtexpid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(ddlreqid.SelectedValue, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@vlineno", SqlDbType.Int)).Value = intRow 'intRow
                              mySqlCmd.ExecuteNonQuery()

                    Next



                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_veh_expense", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exid", SqlDbType.VarChar, 20)).Value = CType(txtexpid.Text.Trim, String)
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

#Region " Private Sub ShowRecord1(ByVal RefCode As String)"
    Private Sub ShowRecord1(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othtypmast Where othtypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othtypcode")) = False Then
                        Me.txtexpid.Text = CType(mySqlReader("othtypcode"), String)
                    Else
                        Me.txtexpid.Text = ""
                    End If
                    If IsDBNull(mySqlReader("othtypname")) = False Then
                        Me.txtexpid.Text = CType(mySqlReader("othtypname"), String)
                    Else

                    End If

                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        Me.ddldname.Value = CType(mySqlReader("othgrpcode"), String)
                        Me.ddldcode.SelectedIndex = Me.ddldname.SelectedIndex     'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
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
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excsellmast ", "excsellcode", CType(txtexpid.Text.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Excursion Types code is already present.');", True)
                SetFocus(txtexpid)
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

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid()
        Dim myDS As New DataSet
        Dim mydataadapter As New SqlDataAdapter
        grdRecord.Visible = True
        Dim SqlConn As New SqlConnection

        If grdRecord.PageIndex < 0 Then
            grdRecord.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = "select * from expense_master "


            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            mydataadapter = New SqlDataAdapter(strSqlQry, SqlConn)
            mydataadapter.Fill(myDS)
            grdRecord.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdRecord.DataBind()
            Else
                grdRecord.PageIndex = 0
                grdRecord.DataBind()

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionsRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(mydataadapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Public Sub fillgridnew(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgridnew(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
        txtgridrows.Value = grd.Rows.Count

    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("tran_lineno", GetType(Integer)))

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
#Region "Public Sub show_record()"
    Public Sub show_record(ByVal RefCode As String)
        Dim myDS As New DataSet
        Dim mySqlReader As SqlDataReader
        mySqlConn = clsDBConnect.dbConnection           'connection open
        mySqlCmd = New SqlCommand("Select *,expense_master.remarks hremarks from expense_detail inner join expense_master on expense_master.exid =expense_detail.exid inner join vehicle_expense_master on expense_detail.expensecode =vehicle_expense_master.expensecode Where expense_master.exid='" & RefCode & "'", mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        Dim lblLineNo As New Label
        Dim txtgriddate As New TextBox
        Dim ddlcode As New HtmlSelect
        Dim ddlexpname As New HtmlSelect
        Dim ddlexptype As New HtmlSelect
        Dim ddlreqid As New DropDownList 'HtmlSelect
        Dim txtamount As New HtmlInputText
        'Dim txtremarks As New HtmlInputText
        Dim txtremarks As New HtmlTextArea
        Dim ddlvehcode As New HtmlSelect
        If mySqlReader.HasRows Then
            If mySqlReader.Read() = True Then


                txtexpid.Text = CType(mySqlReader("exid"), String)
                txtDate.Text = Format(CType(mySqlReader("exdate"), Date), "dd/MM/yyyy")


                If IsDBNull(mySqlReader("drivercode")) = False Then
                    ' Me.ddldcode.SelectedItem.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "drivermaster", "drivername", "drivercode", CType(mySqlReader("drivercode"), String))                        'Me.ddlcountry.SelectedValue = CType(mySqlReader("ctrycode"), String)
                    'Me.ddldname.SelectedItem.Value = Me.ddlcode

                    Me.ddldcode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "drivermaster", "drivername", "drivercode", CType(mySqlReader("drivercode"), String))                        'Me.ddlcountry.SelectedValue = CType(mySqlReader("ctrycode"), String)
                    Me.ddldname.Value = CType(mySqlReader("drivercode"), String)

                    '  ddldcode.SelectedItem.Text = mySqlReader("drivercode").ToString
                    'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddldcode, "drivercode", "select drivercode from  drivermaster where drivermaster.drivercode='" & mySqlReader("drivercode") & "'")
                    ' ddldname.SelectedItem.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "drivermaster", "drivername", "drivercode", CType(mySqlReader("drivercode"), String))                        'Me.ddlcountry.SelectedValue = CType(mySqlReader("ctrycode"), String)
                End If

                If IsDBNull(mySqlReader("hremarks")) = False Then
                    txtremks.Text = mySqlReader("hremarks")
                End If

                If IsDBNull(mySqlReader("expensevalue")) = False Then
                    txttot.Text = mySqlReader("expensevalue")
                End If

            End If
        End If
        mySqlReader.Close()
        mySqlCmd.Dispose()
        mySqlConn.Close()


        Dim d As Double
        'Open connection
        Dim lngCnt As Long
        lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "expense_detail", "count(exid)", "exid", RefCode)
        If lngCnt = 0 Then lngCnt = 1
        fillgridnew(grdRecord, False, lngCnt)
        mySqlConn = clsDBConnect.dbConnection           'co
        strSqlQry = "Select *,expense_master.remarks hremarks from expense_detail inner join expense_master on expense_master.exid =expense_detail.exid inner join vehicle_expense_master on expense_detail.expensecode =vehicle_expense_master.expensecode  Where expense_detail.exid='" & RefCode & "'"
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader()
        Dim totnew As Decimal
        Dim CheckCount As Integer = 0

        Dim MyCount As Integer = 0
        If mySqlReader.HasRows Then
            While mySqlReader.Read()
                CheckCount += 1
                MyCount = 0

                For Each gvRow In grdRecord.Rows

                    MyCount += 1
                    If CheckCount = MyCount Then
                    Else
                        GoTo NextLine
                    End If

                    lblLineNo = gvRow.FindControl("lblLineNo")
                    txtgriddate = gvRow.FindControl("txtgriddate")
                    ddlcode = gvRow.FindControl("ddlcode")

                    ddlexpname = gvRow.FindControl("ddlexpname")
                    ddlexptype = gvRow.FindControl("ddlexptype")
                    ddlreqid = gvRow.FindControl("ddlreqid")
                    txtamount = gvRow.FindControl("txtamount")
                    txtremarks = gvRow.FindControl("txtremarks")
                    ddlvehcode = gvRow.FindControl("ddlvehcode")




                    If IsDBNull(mySqlReader("vlineno")) = False Then
                        lblLineNo.Text = mySqlReader("vlineno")
                    End If

                    If IsDBNull(mySqlReader("exdate")) = False Then
                        txtgriddate.Text = mySqlReader("exdate")
                    End If

                    If IsDBNull(mySqlReader("exid")) = False Then
                        ddlcode.Value = mySqlReader("exid")
                    End If

                    If IsDBNull(mySqlReader("expensecode")) = False Then
                        ddlexpname.Value = mySqlReader("expensecode")
                    End If

                    If IsDBNull(mySqlReader("expensename")) = False Then
                        ddlcode.Value = mySqlReader("expensename")
                    End If

                    If IsDBNull(mySqlReader("transfermode")) = False Then
                        ddlexptype.SelectedIndex = mySqlReader("transfermode")
                    End If

                    If IsDBNull(mySqlReader("requestid")) = False Then
                        'ddlreqid.Value = mySqlReader("requestid")
                        'ddlreqid.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "vehiclemaster", "vehiclename", "vehiclecode", CType(mySqlReader("vehiclecode"), String))
                    End If


                    If IsDBNull(mySqlReader("expvalue")) = False Then
                        txtamount.Value = mySqlReader("expvalue")

                        If ViewState("excServiceSelltypeState") = "View" Or ViewState("excServiceSelltypeState") = "Delete" Then
                            txtamount.Disabled = True
                        Else
                            txtamount.Disabled = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtremarks.Value = mySqlReader("remarks")
                        If ViewState("excServiceSelltypeState") = "View" Or ViewState("excServiceSelltypeState") = "Delete" Then
                            txtremarks.Disabled = True
                        Else
                            txtremarks.Disabled = False
                        End If
                    End If

                    If IsDBNull(ddlexptype) = False Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehcode, "vehiclecode", "vehiclename", "select vehiclecode,vehiclename from  vehiclemaster where active=1 and isnull(usedfor,0)=" & ddlexptype.Value)

                    End If

                    If IsDBNull(mySqlReader("vehiclecode")) = False Then
                        ddlvehcode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "vehiclemaster", "vehiclename", "vehiclecode", CType(mySqlReader("vehiclecode"), String))                        'Me.ddlcountry.SelectedValue = CType(mySqlReader("ctrycode"), String)
                        ' Me.ddlvehiclename.Value = CType(mySqlReader("vehiclecode"), String)
                    End If




                    'totnew = totnew + txtfuelvalue.Value
NextLine:
                Next

                'txttotal.Value = totnew

            End While

        End If
        mySqlReader.Close()
        mySqlConn.Close()
    End Sub
#End Region
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcursionTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

 
    Protected Sub grdRecord_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRecord.PageIndexChanged


    End Sub

    Protected Sub grdRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRecord.SelectedIndexChanged
        grdRecord.Visible = True
    End Sub

#Region "Protected Sub grdRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRecord.RowDataBound"
    Protected Sub grdRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRecord.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
            Exit Sub
        End If

        Dim lblLineNo As New Label
        Dim txtgriddate As New TextBox
        Dim ddlcode As New HtmlSelect
        Dim ddlexpname As New HtmlSelect
        Dim ddlexptype As New HtmlSelect
        Dim ddlreqid As New DropDownList ' HtmlSelect
        Dim txtamount As New HtmlInputText
        'Dim txtremarks As New HtmlInputText
        Dim txtremarks As New HtmlTextArea
        Dim ddlvehcode As New HtmlSelect
        Dim gvrow1 As New GridView

        Dim strOpti As String = ""
        Dim strtable As String = ""
        Dim strfiled As String = ""
        Dim i As Integer = 0
        Dim totamount As Double
        gvRow = e.Row


        'txtDate = gvRow.FindControl("txtDate")
        'txtDueDate = gvRow.FindControl("txtDueDate")

       
        lblLineNo = gvRow.FindControl("lblLineNo")
        txtgriddate = gvRow.FindControl("txtgriddate")
        ddlcode = gvRow.FindControl("ddlcode")

        ddlexpname = gvRow.FindControl("ddlexpname")
        ddlexptype = gvRow.FindControl("ddlexptype")
        ddlreqid = gvRow.FindControl("ddlreqid")
        txtamount = gvRow.FindControl("txtamount")
        txtremarks = gvRow.FindControl("txtremarks")
        ddlvehcode = gvRow.FindControl("ddlvehcode")


        If ViewState("excServiceSelltypeState") = "New" Then

            If Session("TRANSFERPAGE") = "YES" Then
                ddlexptype.SelectedIndex = 0
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehcode, "vehiclecode", "vehiclename", "select vehiclecode,vehiclename from  vehiclemaster where active=1 and isnull(usedfor,0)=" & ddlexptype.SelectedIndex)
                ddlvehcode.Items.Add("[Select]")
                ddlvehcode.SelectedIndex = ddlvehcode.Items.Count - 1
            Else
                ddlexptype.SelectedIndex = 1
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehcode, "vehiclecode", "vehiclename", "select vehiclecode,vehiclename from  vehiclemaster where active=1 and isnull(usedfor,0)=" & ddlexptype.SelectedIndex)
                ddlvehcode.Items.Add("[Select]")
                ddlvehcode.SelectedIndex = ddlvehcode.Items.Count - 1
            End If

        End If

        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehcode, "vehiclecode", "vehiclename", "select vehiclecode,vehiclename from  vehiclemaster where active=1 ")
         objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcode, "expensecode", "expensename", "select expensecode,expensename from  vehicle_expense_master where active=1 ")
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexpname, "expensename", "expensecode", "select expensecode,expensename from  vehicle_expense_master  where active=1 ")

        If ddldcode.Value <> "[Select]" Then
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlreqid, "requestid", "requestid1", "select v.requestid,v.requestid as requestid1  from  transfers_booking_details v   left join expense_detail_request r on v.requestid =r.requestid where r.requestid is null and v.drivercode='" & ddldname.Value & "'")
            objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlreqid, "requestid", "select v.requestid,v.requestid as requestid1  from  transfers_booking_details v   left join expense_detail_request r on v.requestid =r.requestid where r.requestid is null and v.drivercode='" & ddldcode.Value & "'")


        End If


         
        Dim CodeId As String = ddlcode.ClientID
        Dim codename As String = ddlexpname.ClientID
        'Dim exptype As String = ddlexptype.ClientID

       

        ddlcode.Attributes.Add("onchange", "FillNamenew('" & CodeId & "','" & codename & "')")
        ddlexpname.Attributes.Add("onchange", "FillNamenew1('" & CodeId & "','" & codename & "')")

        ddlexptype.Attributes.Add("onchange", "fillvehbontype('" & ddlexptype.ClientID & "','" & ddlvehcode.ClientID & "')")

        txtamount.Attributes.Add("onchange", "javascript:grdTotalnew()")
        'For Each gvrow1 In grdRecord.Rows
        '    txtamount = gvrow1.FindControl("txtamount")
        '    If txtamount.Value <> 0 Then
        '        totamount = totamount + txtamount.Value
        '    End If
        'Next

        'txttot.Text = totamount
    End Sub
#End Region

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = grdRecord.Rows.Count + 1
        Dim lineno(count) As String
        Dim grddate(count) As String
        Dim expcode(count) As String
        Dim expname(count) As String
        Dim exptype(count) As String
        Dim reqid(count) As String
        Dim amount(count) As String
        Dim remarks(count) As String
        Dim vehcode(count) As String

        Dim lblLineNo As New Label
        Dim txtgriddate As New TextBox
        Dim ddlcode As New HtmlSelect
        Dim ddlexpname As New HtmlSelect
        Dim ddlexptype As New HtmlSelect
        Dim ddlreqid As New DropDownList 'HtmlSelect
        Dim txtamount As New HtmlInputText
        'Dim txtremarks As New HtmlInputText
        Dim txtremarks As New HtmlTextArea
        Dim ddlvehcode As New HtmlSelect



        For Each gvRow In grdRecord.Rows


            lblLineNo = gvRow.FindControl("lblLineNo")
            txtgriddate = gvRow.FindControl("txtgriddate")
            ddlcode = gvRow.FindControl("ddlcode")

            ddlexpname = gvRow.FindControl("ddlexpname")
            ddlexptype = gvRow.FindControl("ddlexptype")
            ddlreqid = gvRow.FindControl("ddlreqid")
            txtamount = gvRow.FindControl("txtamount")
            txtremarks = gvRow.FindControl("txtremarks")
            ddlvehcode = gvRow.FindControl("ddlvehcode")

            lineno(n) = lblLineNo.Text
            grddate(n) = txtgriddate.Text
            expcode(n) = ddlcode.Value
            expname(n) = ddlexpname.Value
            exptype(n) = ddlexptype.Value
            reqid(n) = ddlreqid.Text
            amount(n) = txtamount.Value
            remarks(n) = txtremarks.Value
            vehcode(n) = ddlvehcode.Value
            n = n + 1
        Next

        fillgridnew(grdRecord, False, grdRecord.Rows.Count + 1)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdRecord.Rows
            If n = i Then
                Exit For
            End If

            lblLineNo = gvRow.FindControl("lblLineNo")
            txtgriddate = gvRow.FindControl("txtgriddate")
            ddlcode = gvRow.FindControl("ddlcode")

            ddlexpname = gvRow.FindControl("ddlexpname")
            ddlexptype = gvRow.FindControl("ddlexptype")
            ddlreqid = gvRow.FindControl("ddlreqid")
            txtamount = gvRow.FindControl("txtamount")
            txtremarks = gvRow.FindControl("txtremarks")
            ddlvehcode = gvRow.FindControl("ddlvehcode")

            lblLineNo.Text = lineno(n)
            txtgriddate.Text = grddate(n)
            ddlcode.Value = expcode(n)
            ddlexpname.Value = expname(n)
            ddlexptype.Value = exptype(n)
            ddlreqid.Text = reqid(n)
            txtamount.Value = amount(n)
            txtremarks.Value = remarks(n)
            ddlvehcode.Value = vehcode(n)



            n = n + 1
        Next
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = grdRecord.Rows.Count
        Dim lineno(count) As String
        Dim grddate(count) As String
        Dim expcode(count) As String
        Dim expname(count) As String
        Dim exptype(count) As String
        Dim reqid(count) As String
        Dim amount(count) As String
        Dim remarks(count) As String
        Dim vehcode(count) As String
        Dim totnew As Decimal
        Dim chkdel As CheckBox
        Dim lblLineNo As New Label
        Dim txtgriddate As New TextBox
        Dim ddlcode As New HtmlSelect
        Dim ddlexpname As New HtmlSelect
        Dim ddlexptype As New HtmlSelect
        Dim ddlreqid As New DropDownList 'HtmlSelect
        Dim txtamount As New HtmlInputText
        'Dim txtremarks As New HtmlInputText
        Dim txtremarks As New HtmlTextArea
        Dim ddlvehcode As New HtmlSelect


        For Each gvRow In grdRecord.Rows

            chkDel = gvRow.FindControl("chkDel")
            If chkDel.Checked = False Then



                lblLineNo = gvRow.FindControl("lblLineNo")
                txtgriddate = gvRow.FindControl("txtgriddate")
                ddlcode = gvRow.FindControl("ddlcode")

                ddlexpname = gvRow.FindControl("ddlexpname")
                ddlexptype = gvRow.FindControl("ddlexptype")
                ddlreqid = gvRow.FindControl("ddlreqid")
                txtamount = gvRow.FindControl("txtamount")
                txtremarks = gvRow.FindControl("txtremarks")
                ddlvehcode = gvRow.FindControl("ddlvehcode")

                lineno(n) = lblLineNo.Text
                grddate(n) = txtgriddate.Text
                expcode(n) = ddlcode.Value
                expname(n) = ddlexpname.Value
                exptype(n) = ddlexptype.Value
                reqid(n) = ddlreqid.Text
                amount(n) = txtamount.Value
                remarks(n) = txtremarks.Value
                vehcode(n) = ddlvehcode.Value

                n = n + 1
            End If
        Next
        Dim ct As Integer
        ct = n
        If n = 0 Then
            ct = 0
        End If

        fillgridnew(grdRecord, False, ct)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdRecord.Rows
            If n = i Then
                Exit For
            End If

            lblLineNo = gvRow.FindControl("lblLineNo")
            txtgriddate = gvRow.FindControl("txtgriddate")
            ddlcode = gvRow.FindControl("ddlcode")

            ddlexpname = gvRow.FindControl("ddlexpname")
            ddlexptype = gvRow.FindControl("ddlexptype")
            ddlreqid = gvRow.FindControl("ddlreqid")
            txtamount = gvRow.FindControl("txtamount")
            txtremarks = gvRow.FindControl("txtremarks")
            ddlvehcode = gvRow.FindControl("ddlvehcode")

            lblLineNo.Text = lineno(n)
            txtgriddate.Text = grddate(n)
            ddlcode.Value = expcode(n)
            ddlexpname.Value = expname(n)
            ddlexptype.Value = exptype(n)
            ddlreqid.Text = reqid(n)
            txtamount.Value = amount(n)
            txtremarks.Value = remarks(n)
            ddlvehcode.Value = vehcode(n)


            totnew = totnew + txtamount.Value


            n = n + 1
        Next
        txttot.Text = totnew
    End Sub
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        'txt.Attributes.Add("onkeypress", "return checkNumber(event)")\

    End Sub
#End Region
    Protected Sub btnprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnprint.Click
        Dim strpop As String = ""
        Dim strreportfilter As String = ""
        Dim strreportoption As String = ""
        Dim strreqid As String = ""
        Dim strreportitle As String = ""

        If txtexpid.Text <> "" Then
            strreqid = txtexpid.Text
        Else
            MsgBox("Expense Id is Blank")
            Exit Sub
        End If

        strreportitle = "Vehicle Expense Report"

        strpop = "window.open('rptVehicleExpenseReport.aspx?Pageame=vehexpense&BackPageName=rptvehicleexpensereport.aspx " _
    & "&requestid=" & strreqid _
    & "&repfilter=" & strreportfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strreportitle & "','RepNewClient','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

    Protected Sub btnloadgrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnloadgrid.Click

        fillgridnew(grdRecord, True, 10)
    End Sub

    'Protected Sub ddldcode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddldcode.SelectedIndexChanged
    '    Try
    '        ddldname.SelectedIndex = ddldcode.SelectedIndex
    '        FillRequestId()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Protected Sub ddldname_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddldname.SelectedIndexChanged
    '    Try
    '        ddldcode.SelectedIndex = ddldname.SelectedIndex

    '        ddldcode.SelectedItem.Value = ddldname.SelectedItem.Text
    '        FillRequestId()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Sub FillRequestId()
        Try
            For Each dr As GridViewRow In grdRecord.Rows

                Dim ddlreqid As New DropDownList
                ddlreqid = dr.FindControl("ddlreqid")
                If ddldcode.Value <> "[Select]" Then
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlreqid, "requestid", "requestid1", "select v.requestid,v.requestid as requestid1  from  transfers_booking_details v   left join expense_detail_request r on v.requestid =r.requestid where r.requestid is null and v.drivercode='" & ddldname.Value & "'")
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlreqid, "requestid", "select v.requestid,v.requestid as requestid1  from  transfers_booking_details v   left join expense_detail_request r on v.requestid =r.requestid where r.requestid is null and v.drivercode='" & ddldcode.Value & "'")
                End If

            Next
        Catch ex As Exception

        End Try
    End Sub
End Class
