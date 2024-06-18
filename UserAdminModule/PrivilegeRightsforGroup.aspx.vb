'------------================--------------=======================------------------================
'   Module Name    :    PrivilegeRightsforGroups.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections.Generic
#End Region

Partial Class PrivilegeRightsforGroup
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim chkSel As CheckBox
    Shared PrivRightsState As String = ""
#End Region


    <System.Web.Script.Services.ScriptMethod()> _
         <System.Web.Services.WebMethod()> _
    Public Shared Function Getappslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim appnames As New List(Of String)
        Dim divid As String = ""
        Try



            strSqlQry = "select appname,appid  from appmaster where appstatus=1   and appname like  '" & Trim(prefixText) & "%'order by appid "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    appnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("appname").ToString(), myDS.Tables(0).Rows(i)("appid").ToString()))

                Next
            End If
            Return appnames
        Catch ex As Exception
            Return appnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getusergrpslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim usergrpnames As New List(Of String)
        Dim divid As String = ""
        Try
            If PrivRightsState = "New" Then
                strSqlQry = "select distinct groupmaster.groupid ,groupname from groupmaster,appmaster where groupmaster.active=1 and appmaster.appstatus=1" & _
                              " and  convert(varchar(10),groupid)+convert(varchar(10),appid) not  in " & _
                              " (Select convert(varchar(10),groupid)+convert(varchar(10),appid) from group_privilege) order by groupname"
            Else
                strSqlQry = "select groupid, groupname from groupmaster where active=1 and   groupname like  '" & Trim(prefixText) & "%' order by groupid "
            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    usergrpnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("groupname").ToString(), myDS.Tables(0).Rows(i)("groupid").ToString()))

                Next
            End If
            Return usergrpnames
        Catch ex As Exception
            Return usergrpnames
        End Try

    End Function
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("PrivRightsState", Request.QueryString("State"))
        ViewState.Add("PrivRightsRefAppCode", Request.QueryString("RefAppCode"))
        ViewState.Add("PrivRightsRefGrpCode", Request.QueryString("RefGrpCode"))
        If IsPostBack = False Then
            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            PrivRightsState = CType(ViewState("PrivRightsState"), String)
            txtconnection.Value = Session("dbconnectionName")

            Try
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlUserGroup, "groupname", "groupid", "select groupid, groupname from groupmaster where active=1 and groupid not in (select groupid from group_privilege) order by groupname", True)
                ''If ViewState("PrivRightsState") = "New" Then
                ''    strSqlQry = "select distinct groupmaster.groupid ,groupname from groupmaster,appmaster where groupmaster.active=1 and appmaster.appstatus=1" & _
                ''              " and  convert(varchar(10),groupid)+convert(varchar(10),appid) not  in " & _
                ''              " (Select convert(varchar(10),groupid)+convert(varchar(10),appid) from group_privilege) order by groupname"
                ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUserGroup, "groupname", "groupid", strSqlQry, True)
                ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlApplication, "appname", "appid", "select appname, appid from appmaster where appstatus=1  order by appname", True)
                ''Else
                ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUserGroup, "groupname", "groupid", "select groupid, groupname from groupmaster where active=1 order by groupname", True)
                ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlApplication, "appname", "appid", "select appname, appid from appmaster where appstatus=1  order by appname", True)
                ''End If



                If ViewState("PrivRightsState") = "New" Then
                    ' SetFocus(ddlUserGroup)
                    lblHeading.Text = "Add New Privilege Rights"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("PrivRightsState") = "Edit" Then
                    '  SetFocus(ddlUserGroup)
                    lblHeading.Text = "Edit Privilege Rights"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("PrivRightsRefAppCode"), String), CType(ViewState("PrivRightsRefGrpCode"), String))
                    FillGrid("PrivilegeId")
                    ShowPrivilegeGrids(CType(ViewState("PrivRightsRefAppCode"), String), CType(ViewState("PrivRightsRefGrpCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("PrivRightsState") = "View" Then
                    SetFocus(btnExit)
                    lblHeading.Text = "View Privilege Rights"
                    btnSave.Visible = False
                    ShowRecord(CType(ViewState("PrivRightsRefAppCode"), String), CType(ViewState("PrivRightsRefGrpCode"), String))
                    FillGrid("PrivilegeId")
                    ShowPrivilegeGrids(CType(ViewState("PrivRightsRefAppCode"), String), CType(ViewState("PrivRightsRefGrpCode"), String))
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("PrivRightsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Privilege Rights"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("PrivRightsRefAppCode"), String), CType(ViewState("PrivRightsRefGrpCode"), String))
                    FillGrid("PrivilegeId")
                    ShowPrivilegeGrids(CType(ViewState("PrivRightsRefAppCode"), String), CType(ViewState("PrivRightsRefGrpCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                DisableControl()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlUserGroup.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlApplication.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        
        Dim myDS As New DataSet

        grdPrivilege.Visible = True

        If grdPrivilege.PageIndex < 0 Then
            grdPrivilege.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            If TxtusergrpName.Text <> "" And txtappname.Text <> "" Then
                strSqlQry = "select * from privilege_master where privilegestatus=1 and Appid=" & txtappcode.Text

                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS)
                grdPrivilege.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then
                    grdPrivilege.DataBind()
                Else
                    grdPrivilege.DataBind()
                End If

            Else

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Usergroup/Application.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsforGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefAppCode As String, ByVal RefGrpCode As String)"
    Private Sub ShowRecord(ByVal RefAppCode As String, ByVal RefGrpCode As String)
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select * from group_privilege Where appid=" & Val(RefAppCode) & " and groupid=" & Val(RefGrpCode), SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("appid")) = False Then
                        'Me.ddlApplication.Value = CType(mySqlReader("appid"), String)
                        'tx()
                        txtappcode.Text = CType(mySqlReader("appid"), String)

                        txtappname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appname", "appid", CType(mySqlReader("appid"), String))
                    Else
                        'Me.ddlApplication.Value = "[Select]"
                        txtappname.Text = ""
                    End If

                    If IsDBNull(mySqlReader("groupid")) = False Then
                        TxtusergrpCode.Text = CType(mySqlReader("groupid"), String)

                        TxtusergrpName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupname", "groupid", CType(mySqlReader("groupid"), String))
                        'ddlUserGroup.Value = CType(mySqlReader("groupid"), String)
                    Else
                        'Me.ddlUserGroup.Value = "[Select]"
                        TxtusergrpName.Text = ""
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsforGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Private Sub ShowPrivilegeGrids(ByVal RefAppCode As String, ByVal RefGrpCode As String)"
    Private Sub ShowPrivilegeGrids(ByVal RefAppCode As String, ByVal RefGrpCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim chkSel As CheckBox

            '----------------------------------Show Records In Privilege Grid 
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
            myCommand = New SqlCommand("Select * from group_privilege_detail Where appid=" & Val(RefAppCode) & " and groupid=" & Val(RefGrpCode), SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    If IsDBNull(mySqlReader("privilegeid")) = False Then
                        For Each gvRow In grdPrivilege.Rows
                            If CType(mySqlReader("privilegeid"), String) = gvRow.Cells(0).Text.ToString Then
                                chkSel = gvRow.FindControl("chkSelect")
                                chkSel.Checked = True
                                Exit For
                            End If
                        Next
                    End If
                End While
            End If
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)              'connection close 

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsforGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            'clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
            'clsDBConnect.dbConnectionClose(SqlConn)              'connection close           
        End Try
    End Sub
#End Region

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("PrivilegeRightsforGroupSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            If Page.IsValid = True Then
                If ViewState("PrivRightsState") = "New" Or ViewState("PrivRightsState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("PrivRightsState") = "New" Then
                        myCommand = New SqlCommand("sp_add_group_privilege", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("PrivRightsState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_group_privilege", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    End If

                    ' myCommand.Parameters.Add(New SqlParameter("@appid", SqlDbType.Int, 9)).Value = CType(Val(ddlApplication.Items(ddlApplication.SelectedIndex).Value), Long)
                    myCommand.Parameters.Add(New SqlParameter("@appid", SqlDbType.Int, 9)).Value = txtappcode.Text.Trim
                    ' myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int, 9)).Value = CType(Val(ddlUserGroup.Items(ddlUserGroup.SelectedIndex).Value), Long)
                    myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int, 9)).Value = TxtusergrpCode.Text.Trim
                    myCommand.Parameters.Add(New SqlParameter("@active", SqlDbType.Int, 9)).Value = 1

                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    myCommand.ExecuteNonQuery()

                    '----------------------------------- Deleting Data From group_privilege_detail Table
                    myCommand = New SqlCommand("sp_del_group_privilege_detail", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@appid", SqlDbType.Int, 9)).Value = txtappcode.Text.Trim 'CType(Val(ddlApplication.Items(ddlApplication.SelectedIndex).Value), Long)
                    myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int, 9)).Value = TxtusergrpCode.Text.Trim 'CType(Val(ddlUserGroup.Items(ddlUserGroup.SelectedIndex).Value), Long)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    '----------------------------------- Inserting Data To group_privilege_detail Table
                    For Each gvRow In grdPrivilege.Rows
                        chkSel = gvRow.FindControl("chkSelect")
                        If chkSel.Checked = True Then
                            myCommand = New SqlCommand("sp_add_group_privilege_detail", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@appid", SqlDbType.Int, 9)).Value = txtappcode.Text.Trim ' CType(Val(ddlApplication.Items(ddlApplication.SelectedIndex).Value), Long)
                            myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int, 9)).Value = TxtusergrpCode.Text.Trim  'CType(Val(ddlUserGroup.Items(ddlUserGroup.SelectedIndex).Value), Long)
                            myCommand.Parameters.Add(New SqlParameter("@privilegeid", SqlDbType.Int, 9)).Value = CType(Val(gvRow.Cells(0).Text.Trim), Long)
                            myCommand.ExecuteNonQuery()
                        End If
                    Next
                    '-----------------------------------------------------------

                ElseIf ViewState("PrivRightsState") = "Delete" Then
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

                    myCommand = New SqlCommand("sp_del_group_privilege", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@appid", SqlDbType.Int, 9)).Value = txtappcode.Text.Trim  'CType(Val(ddlApplication.Items(ddlApplication.SelectedIndex).Value), Long)
                    myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int, 9)).Value = TxtusergrpCode.Text.Trim  ' CType(Val(ddlUserGroup.Items(ddlUserGroup.SelectedIndex).Value), Long)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                'Response.Redirect("PrivilegeRightsforGroupSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('PrivRightsWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsforGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Validate Page()"
    Public Function ValidatePage() As Boolean
        Try

            Dim flgCheck As Boolean = False

            For Each gvRow In grdPrivilege.Rows
                chkSel = gvRow.FindControl("chkSelect")
                If chkSel.Checked = True Then
                    flgCheck = True
                    Exit For
                End If
            Next
            If flgCheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one Privilege.');", True)
                SetFocus(grdPrivilege)
                ValidatePage = False
                Exit Function
            End If

            ValidatePage = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()

        If ViewState("PrivRightsState") = "New" Then
            btnSave.Visible = False
        ElseIf ViewState("PrivRightsState") = "Edit" Then
            btnFillGrid.Visible = False
            ' ddlUserGroup.Disabled = True
            TxtusergrpName.Enabled = False
            txtappname.Enabled = False
            ' ddlApplication.Disabled = True
        ElseIf ViewState("PrivRightsState") = "View" Or ViewState("PrivRightsState") = "Delete" Then
            btnFillGrid.Visible = False
            TxtusergrpName.Enabled = True
            txtappname.Enabled = True
            ' ddlUserGroup.Disabled = True
            'ddlApplication.Disabled = True
            btnSave.Visible = False
            For Each gvRow In grdPrivilege.Rows
                chkSel = gvRow.FindControl("chkSelect")
                chkSel.Enabled = False
            Next
        End If
        If ViewState("PrivRightsState") = "Delete" Then
            btnSave.Visible = True
        End If
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PrivilegeRightsforGroup','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillGrid.Click
        Try
            If ValidateGrid() = False Then
                Exit Sub
            End If
            FillGrid("PrivilegeId")
            btnSave.Visible = True
        Catch ex As Exception

        End Try

    End Sub
    Private Function ValidateGrid() As Boolean
        Dim strValue As String = ""

        If TxtusergrpCode.Text = " " Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select user group.');", True)
            SetFocus(TxtusergrpName)
            ValidateGrid = False
            Exit Function
        End If
        If txtappcode.Text = " " Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select application.');", True)
            SetFocus(txtappname)
            ValidateGrid = False
            Exit Function
        End If
        strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  top 1 't' FROM group_privilege_detail WHERE groupid='" & TxtusergrpCode.Text & "' and appid='" & txtappcode.Text & "'")
        If strValue <> "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('For selected group and application alreadey privilege assigned.');", True)
            ValidateGrid = False
            Exit Function
        End If

        ValidateGrid = True
    End Function
End Class


