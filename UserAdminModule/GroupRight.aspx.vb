'------------------------------------------------------------------------------------------------
'   Module Name    :    GroupRight.aspx
'   Developer Name :    Mangesh
'   Date           :    10 Aug 2008
'   
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class GroupRight
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objdate As New clsDateTime
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction

    Dim clRight As New Collection
    Dim clSubMenu As New Collection
    Dim strGroupId As String
    Dim strAppId As String

    Dim gvrow As GridViewRow
    Dim chkSel As CheckBox

#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ViewState.Add("GrpRightsState", Request.QueryString("State"))
            ViewState.Add("GrpRightsAppId", Request.QueryString("AppId"))
            ViewState.Add("GrpRightsGroupid", Request.QueryString("Groupid"))
            If Page.IsPostBack = False Then
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If


                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpFunCode, "funname", "funid", "select funname,funid from functionalmaster    order by funid", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "groupname", "select * from groupmaster where active=1 order by groupid", True)
                If ViewState("GrpRightsState") = "New" Then
                    strSqlQry = "select distinct groupmaster.groupid ,groupname from groupmaster,appmaster where groupmaster.active=1 and appmaster.appstatus=1" & _
                                " and  convert(varchar(10),groupid)+convert(varchar(10),appid) not  in " & _
                                " (Select convert(varchar(10),groupid)+convert(varchar(10),appid) from group_rights) order by groupname"
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "groupname", strSqlQry, True)
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlApplication, "appname", "select appname from appmaster where appstatus=1 order by appid", True)

                Else
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "groupname", "select groupname from groupmaster where active=1 order by groupid", True)
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlApplication, "appname", "select appname from appmaster where appstatus=1 order by appid", True)
                End If

                'strGroupId = objUser.GetGroupId(Session("GlobalUserName").ToString, Session("Userpwd").ToString).ToString
                'strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"appmaster", "appid", "appname", ddlApplication.SelectedValue)

                'mySqlReader = objUser.GetAppName(Session("GlobalUserName").ToString, Session("Userpwd").ToString)
                'If mySqlReader.HasRows Then
                '    While mySqlReader.Read
                '        strAppId = objUser.GetAppId(Session("dbconnectionName"),mySqlReader(0)).ToString
                '    End While
                'End If
                'mySqlReader.Close()

                ' Fill_Tree_View(strGroupId, strAppId)
                Session.Add("clRight", clRight)
                Session.Add("clSubMenu", clSubMenu)
                btnSaveRights.Enabled = False
                If ViewState("GrpRightsState") = "New" Then
                    SetFocus(ddlGroup)
                    lblHeading.Text = "Add New Group Right"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("GrpRightsState") = "Edit" Then
                    lblHeading.Text = "Edit Group Right"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("GrpRightsAppId"), String), CType(ViewState("GrpRightsGroupid"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("GrpRightsState") = "View" Then
                    SetFocus(btnExit)
                    lblHeading.Text = "View Group Right"
                    btnSave.Visible = False
                    btnExit.Text = "Return to Search"
                    ShowRecord(CType(ViewState("GrpRightsAppId"), String), CType(ViewState("GrpRightsGroupid"), String))
                ElseIf ViewState("GrpRightsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Group Right"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("GrpRightsAppId"), String), CType(ViewState("GrpRightsGroupid"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                End If
                DisableControl()
                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlApplication.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGroup.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                tvMenu.Attributes.Add("oncontextmenu", "Onclick(event);")
                ' Me.BtnMenuClear.Attributes.Add("onclick", "javascript:ClearMenu()")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("GroupRight.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
#Region "Private Function FunctionalRightsUpdate() as String"
    Private Function FunctionalRightsUpdate(ByVal strResult As String, ByVal updatedRights As String) As String
        FunctionalRightsUpdate = ""
        Dim strTemp As String = ""
        Dim i As Integer
        Dim FlagExt As Boolean = False
        Dim strSplit As String()
        If strResult <> "" Then
            strTemp = strResult
            strSplit = strResult.Split(";")
            For i = 0 To strSplit.Length - 1
                If strSplit.GetValue(i) = updatedRights Then
                    FlagExt = True
                    Exit For
                End If
            Next
        End If
        If FlagExt = False Then
            'For i = 0 To strSplit.Length - 1
            '    strTemp = strTemp & strSplit.GetValue(i) & ";"
            'Next
            strTemp = strResult & updatedRights & ";"
        End If
        FunctionalRightsUpdate = strTemp
    End Function
#End Region
#Region "Private Function FunctionalRightsRemove() as String"
    Private Function FunctionalRightsRemove(ByVal strResult As String, ByVal updatedRights As String) As String
        FunctionalRightsRemove = ""
        Dim strTemp As String = ""
        Dim i As Integer
        Dim FlagExt As Boolean = False
        Dim strSplit As String()
        If strResult <> "" Then
            strSplit = strResult.Split(";")
            For i = 0 To strSplit.Length - 1
                If strSplit.GetValue(i) = updatedRights Then
                    FlagExt = True
                Else
                    If strSplit.GetValue(i) <> "" Then
                        strTemp = strTemp & strSplit.GetValue(i) & ";"
                    End If
                End If
            Next
        End If
        If FlagExt = False Then
            strTemp = strResult
        End If
        FunctionalRightsRemove = strTemp
    End Function
#End Region
#Region "Private Sub Gride Edit"
    Private Sub ShowGridEdit(ByVal grdName As String, ByVal menuid As Integer, ByVal appid As String, ByVal grpid As String)
        Dim strResult As String = ""
        Dim i As Integer


        Select Case grdName
            Case "grdFunctional"
                clRight = CType(Session("clRight"), Collection)
                Dim strfnRight As String()
                If IsNothing(clRight) = False Then
                    For i = 1 To clRight.Count
                        strfnRight = clRight(i).ToString.Split("/")
                        If appid = strfnRight.GetValue(1) And grpid = strfnRight.GetValue(0) And menuid = strfnRight.GetValue(2) Then
                            strResult = strfnRight.GetValue(3)
                        End If
                    Next

                    FillGrid("grdFunctional", menuid)
                    If strResult <> "" Then
                        Dim strSplit As String() = strResult.Split(";")
                        For i = 0 To strSplit.Length - 1
                            For Each gvrow In grdFunctional.Rows
                                chkSel = gvrow.FindControl("chkSelect")
                                If gvrow.Cells(2).Text = strSplit.GetValue(i) Then
                                    chkSel.Checked = True
                                    Exit For
                                End If
                            Next
                        Next
                    End If
                End If
            Case "grdSubMenu"
                clSubMenu = CType(Session("clSubMenu"), Collection)
                If IsNothing(clSubMenu) = False Then
                    FillGrid("grdSubMenu", menuid)
                    If grdSubMenu.Rows.Count <> 0 Then
                        btnSaveSubMenu.Visible = True
                        BtnSubMenuCancel.Visible = True
                    Else
                        btnSaveSubMenu.Visible = False
                        BtnSubMenuCancel.Visible = False
                        Exit Sub
                    End If
                    Dim strSubMenuRight As String()
                    For i = 1 To clSubMenu.Count
                        strSubMenuRight = clSubMenu(i).ToString.Split("/")
                        If appid = strSubMenuRight.GetValue(1) And grpid = strSubMenuRight.GetValue(0) And menuid = strSubMenuRight.GetValue(2) Then
                            strResult = strSubMenuRight.GetValue(3)
                            For Each gvrow In grdSubMenu.Rows
                                chkSel = gvrow.FindControl("chkSelect")
                                If gvrow.Cells(2).Text = strResult Then
                                    chkSel.Checked = strSubMenuRight.GetValue(4)
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                End If
        End Select
        DisableGrid()
    End Sub
#End Region
#Region "Private Sub Gride Edit View"
    Private Sub ShowGrid(ByVal grdName As String, ByVal menuid As Integer, ByVal appid As String, ByVal grpid As String)
        Dim strResult As String = ""
        Dim i As Integer
        Select Case grdName
            Case "grdFunctional"
                FillGrid("grdFunctional", menuid)
                strSqlQry = "select * from group_functionalrights where menuid=" & menuid & "  and appid=" & appid & " and grpid=" & grpid & ""
                mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strSqlQry)
                If mySqlReader.HasRows = True Then
                    If mySqlReader.Read = True Then
                        strResult = mySqlReader("functional_rights")
                    End If
                End If
                If strResult <> "" Then
                    Dim strSplit As String() = strResult.Split(";")
                    For i = 0 To strSplit.Length - 1
                        For Each gvrow In grdFunctional.Rows
                            chkSel = gvrow.FindControl("chkSelect")
                            If gvrow.Cells(2).Text = strSplit.GetValue(i) Then
                                chkSel.Checked = True
                                Exit For
                            End If
                        Next
                    Next
                End If
            Case "grdSubMenu"
                FillGrid("grdSubMenu", menuid)
                strSqlQry = "select  submenuid,active from group_submenurights where menuid=" & menuid & "  and appid=" & appid & " and grpid=" & grpid & ""
                mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strSqlQry)
                If mySqlReader.HasRows = True Then
                    While mySqlReader.Read
                        strResult = mySqlReader("submenuid")
                        For Each gvrow In grdSubMenu.Rows
                            chkSel = gvrow.FindControl("chkSelect")
                            If gvrow.Cells(2).Text = strResult Then
                                chkSel.Checked = mySqlReader("active")
                                Exit For
                            End If
                        Next
                    End While
                End If
        End Select
    End Sub
#End Region
#Region "Private Sub ShowRecord()"
    Private Sub ShowRecord(ByVal appid As String, ByVal grpid As String)
        Fill_Tree_View(grpid, appid)
        ddlGroup.SelectedItem.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupname", "groupid", grpid)
        ddlApplication.SelectedItem.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appname", "appid", appid)
        ddlGroup.Enabled = False
        ddlApplication.Enabled = False

        Dim strkey As String

        clRight = CType(Session("clRight"), Collection)
        strSqlQry = "select * from group_functionalrights where appid=" & appid & " and grpid=" & grpid & ""
        mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strSqlQry)
        If mySqlReader.HasRows = True Then
            While mySqlReader.Read
                'groupid/appid/menuid/funRights
                strkey = mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid") & "/" & mySqlReader("functional_rights")

                If colexists(clRight, mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid")) = False Then
                    clRight.Add(strkey, mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid"), Nothing, Nothing)
                Else
                    clRight.Remove(mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid"))
                    clRight.Add(strkey, mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid"), Nothing, Nothing)
                End If

            End While
        End If
        Session.Add("clRight", clRight)
        mySqlReader.Close()

        'Sub Menu
        clSubMenu = CType(Session("clSubMenu"), Collection)
        strSqlQry = "select appid,grpid,menuid,submenuid,case active when 1 then 'True' else 'False' end as [Active] from group_submenurights where appid=" & appid & " and grpid=" & grpid & ""
        mySqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strSqlQry)

        If mySqlReader.HasRows = True Then
            While mySqlReader.Read
                'groupid/appid/menuid/menuid
                strkey = mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid") & "/" & mySqlReader("submenuid") & "/" & mySqlReader("Active")
                If colexists(clSubMenu, mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid") & "/" & mySqlReader("submenuid")) = False Then
                    clSubMenu.Add(strkey, mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid") & "/" & mySqlReader("submenuid"), Nothing, Nothing)
                Else
                    clSubMenu.Remove(strkey)
                    clSubMenu.Add(strkey, mySqlReader("grpid") & "/" & mySqlReader("appid") & "/" & mySqlReader("menuid") & "/" & mySqlReader("submenuid"), Nothing, Nothing)
                End If
            End While
        End If
        Session.Add("clSubMenu", clSubMenu)
    End Sub
#End Region
#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("GrpRightsState") = "New" Then
            btnSave.Visible = False
            btnClearGroup.Visible = True
        ElseIf ViewState("GrpRightsState") = "Edit" Then
            btnEditRight.Visible = True
            btnSaveRights.Visible = True
            btnClearGroup.Visible = False
            btnSave.Visible = True

            lblFunRights.Visible = True
            ddlGrpFunCode.Visible = True
            btnSaveRightsAll.Visible = True
            btnRemoveRightsAll.Visible = True
            BtnMenuClear.Visible = True

        ElseIf ViewState("GrpRightsState") = "View" Or ViewState("GrpRightsState") = "Delete" Then
            ddlGroup.Enabled = False
            ddlApplication.Enabled = False

            btnEditRight.Visible = False
            btnCancelRight.Visible = False
            btnSaveRights.Visible = False

            btnSaveSubMenu.Visible = False
            BtnSubMenuCancel.Visible = False

            btnSave.Visible = False
            btnClearGroup.Visible = False

            lblFunRights.Visible = False
            ddlGrpFunCode.Visible = False
            btnSaveRightsAll.Visible = False
            btnRemoveRightsAll.Visible = False
            BtnMenuClear.Visible = False

            DisableGrid()
        End If
        If ViewState("GrpRightsState") = "Delete" Then
            btnSave.Visible = True
        End If


    End Sub
#End Region
#Region "Private Sub DisableGrid()"
    Private Sub DisableGrid()
        If ViewState("GrpRightsState") = "View" Or ViewState("GrpRightsState") = "Delete" Then
            For Each gvrow In grdFunctional.Rows
                chkSel = gvrow.FindControl("chkSelect")
                chkSel.Enabled = False
            Next
            For Each gvrow In grdSubMenu.Rows
                chkSel = gvrow.FindControl("chkSelect")
                chkSel.Enabled = False
            Next
            btnEditRight.Visible = False
            btnCancelRight.Visible = False
            btnSaveRights.Visible = False

            btnSaveSubMenu.Visible = False
            BtnSubMenuCancel.Visible = False
        End If
    End Sub
#End Region
    'Private Sub PopulateSubLevel(ByVal parentid As Integer, ByVal parentNode As TreeNode)
    '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


    '    strQry = " select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup WHERE div_code='" & txtDivcode.Value & "' and accttype=1 and  childid=sc.parentID) childnodecount FROM acctgroup sc where div_code='" & txtDivcode.Value & "' and  accttype=1 and childid=" & parentid
    '    Else
    '    strQry = " select parentID as [id],acctname,acctcode,(select count(*) FROM acctgroup WHERE div_code='" & txtDivcode.Value & "' and accttype=2 and  childid=sc.parentID) childnodecount FROM acctgroup sc where  div_code='" & txtDivcode.Value & "' and  childid=" & parentid
    '    End If

    '    Dim objCommand As New SqlCommand(strQry, mySqlConn)
    '    objCommand.Parameters.Add("@parentID", SqlDbType.Int).Value = parentid
    '    Dim da As New SqlDataAdapter(objCommand)
    '    Dim dt As New DataTable()
    '    da.Fill(dt)

    '    mySqlConn.Close()
    'End Sub


    Public Sub Fill_Tree_View(ByVal strGroup_Id As String, ByVal strAppid As String)

        tvMenu.Nodes.Clear()

        Dim myds As New DataSet
        Dim mycods As New DataSet
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open 



        strSqlQry = "select m.menuid,m.menudesc ,m.menu_status,m.parentid,m.menu_type from MenuMaster M left outer join Group_rights U " & _
                    " on M.appid=U.appid and M.menuid=U.menuid and U.groupid='" & strGroup_Id & "' and U.active=1 where M.menu_status=1  and m.parentid=0 " & _
                    " and M.appid='" & strAppid & "'  order by m.appid,parentid  "
        Dim daMainMenu As New SqlDataAdapter(strSqlQry, mySqlConn)

        strSqlQry = "select m.menuid,m.menudesc ,m.menu_status,m.parentid,m.menu_type from MenuMaster M left outer join group_Rights U  on M.appid=U.appid and M.menuid=U.menuid and U.groupid='" & strGroup_Id & "' and U.active=1 where M.menu_status=1   and M.appid='" & strAppid & "' and parentid<>0 union all select m.menuid,m.menudesc ,m.menu_status,m.parentid,m.menu_type from MenuMaster M left outer join group_Rights U  on M.appid=U.appid and M.menuid=U.menuid and U.groupid='" & strGroup_Id & "' and U.active=1 where M.menu_status=0 and m.linkedmenu=1 and M.appid='" & strAppid & "' and parentid<>0 and parentid<>1030 and parentid<>1200 and parentid<>1040 order by parentid,menuid"
        Dim daSubMenu As New SqlDataAdapter(strSqlQry, mySqlConn)
        daMainMenu.Fill(myds, "dtMainMenu")
        daSubMenu.Fill(myds, "dtSubMenu")
        myds.Relations.Add("MenuToSubMenu", myds.Tables("dtMainMenu").Columns("menuid"), myds.Tables("dtSubMenu").Columns("parentid"))

        If strAppid = 1 Then

            Dim Conmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\ContractsSearch.aspx?appid=" + strAppid, strAppid)
            Dim offmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\OfferSearch.aspx?appid=" + strAppid, strAppid)

            Dim AppyMarkuHotelId As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\ApplyMarkups.aspx?appid=" + strAppid, strAppid)

            strSqlQry = "select m.menuid,m.menudesc ,m.menu_status,m.parentid,m.menu_type from MenuMaster M left outer join group_Rights U  on M.appid=U.appid and M.menuid=U.menuid and U.groupid='" & strGroup_Id & "'  and U.active=1 where M.menu_status=0   and M.appid='" & strAppid & "' and m.menuid in ('" & Conmenuid & "','" & offmenuid & "','" & AppyMarkuHotelId & "') and linkedmenu=1 "
            Dim dasubcntrct As New SqlDataAdapter(strSqlQry, mySqlConn)

            strSqlQry = "select m.menuid,m.menudesc ,m.menu_status,m.parentid,m.menu_type from MenuMaster M left outer join group_Rights U  on M.appid=U.appid and M.menuid=U.menuid and U.groupid='" & strGroup_Id & "' and U.active=1 where M.menu_status=0   and M.appid='" & strAppid & "' and parentid in ('" & Conmenuid & "','" & offmenuid & "','" & AppyMarkuHotelId & "') and linkedmenu=1"
            Dim dacntrctMenu As New SqlDataAdapter(strSqlQry, mySqlConn)


            dasubcntrct.Fill(mycods, "dtCoMenu")
            dacntrctMenu.Fill(mycods, "dtCoSubMenu")
            'mySqlConn.Close()


            mycods.Relations.Add("SubMenuToCOMenu", mycods.Tables("dtCoMenu").Columns("menuid"), mycods.Tables("dtCoSubMenu").Columns("parentid"))
        End If
        Dim nodeMenu As TreeNode
        Dim nodesubMenu As TreeNode
        Dim node2subMenu As TreeNode
        Dim rowMenu, rowSub As DataRow
        Dim rowMenu1 As DataRow

        For Each rowMenu In myds.Tables("dtMainMenu").Rows
            nodeMenu = New TreeNode
            nodeMenu.Text = rowMenu("menudesc")
            nodeMenu.Value = rowMenu("menuid")
            nodeMenu.Checked = getmenuexists(strAppid, strGroup_Id, rowMenu("menuid"))

            For Each rowSub In rowMenu.GetChildRows("MenuToSubMenu")
                nodesubMenu = New TreeNode
                nodesubMenu.Text = rowSub("menudesc")
                nodesubMenu.Value = rowSub("menuid")
                nodesubMenu.Checked = getmenuexists(strAppid, strGroup_Id, rowSub("menuid"))
                nodeMenu.ChildNodes.Add(nodesubMenu)
                If strAppid = 1 Then
                    For Each rowMenu1 In mycods.Tables("dtCoSubMenu").Rows

                        node2subMenu = New TreeNode
                        If nodesubMenu.Value = rowMenu1("parentid") Then
                            node2subMenu.Text = rowMenu1("menudesc")

                            node2subMenu.Value = rowMenu1("menuid")
                            node2subMenu.Checked = getmenuexists(strAppid, strGroup_Id, rowMenu1("menuid"))

                            nodesubMenu.ChildNodes.Add(node2subMenu)
                        End If


                    Next
                End If
            Next


            nodeMenu.SelectAction = TreeNodeSelectAction.None

            tvMenu.Nodes.Add(nodeMenu)

        Next

        myds.Dispose()
        mycods.Dispose()

        daMainMenu.Dispose()
        daSubMenu.Dispose()
        mySqlConn.Close()
    End Sub
    Private Function getmenuexists(ByVal app As Integer, ByVal grp As Integer, ByVal menu As Integer) As Boolean
        Dim tempstr As String
        getmenuexists = False
        tempstr = "select g.groupid from group_rights U ,Groupmaster G  " _
               & "where G.groupid=u.groupid and U.active=1 and u.appid='" & app & "' and u.groupid='" & grp & "' and " _
               & "u.menuid = " & menu & ""

        If objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), tempstr) = 0 Then
            getmenuexists = False
        Else
            getmenuexists = True
        End If
        Return getmenuexists
    End Function
    Protected Sub tvMenu_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvMenu.SelectedNodeChanged
        Dim intMenuId As Integer = tvMenu.SelectedNode.Value
        FillGrid("grdFunctional", intMenuId)
        FillGrid("grdSubMenu", intMenuId)
        txtMenuId.Text = intMenuId
        strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
        strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)


        ShowGridEdit("grdFunctional", intMenuId, CType(strAppId, String), CType(strGroupId, String))
        ShowGridEdit("grdSubMenu", intMenuId, CType(strAppId, String), CType(strGroupId, String))

    End Sub
#Region "Private Sub FillGrid()"
    Private Sub FillGrid(ByVal strFillGridName As String, ByVal IntMenuId As Integer)
        Dim myDS As New DataSet
        Dim objUser2 As New clsUser
        Try
            strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
            strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

            Select Case strFillGridName

                Case "grdFunctional"
                    grdFunctional.Visible = True
                    If grdFunctional.PageIndex < 0 Then
                        grdFunctional.PageIndex = 0
                    End If
                    strSqlQry = ""
                    strSqlQry = "select isnull(functional_avail_rights,'') as functional_avail_rights  from menumaster where menuid='" & IntMenuId & "' and appid='" & strAppId & "'"

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS)
                    Dim strfrights As String()
                    Dim strfrights2 As String()
                    Dim i As Integer
                    Dim k As Integer
                    Dim strContractRights As String
                    Dim lngCount As Int16
                    If myDS.Tables(0).Rows.Count >= 1 Then
                        'For k = 0 To myDS.Tables(0).Rows.Count - 1


                        If myDS.Tables(0).Rows(k).Item("functional_avail_rights") <> "" Then
                            strfrights = Split(myDS.Tables(0).Rows(0).Item("functional_avail_rights").ToString, ";")
                            '  fillDategrd(grdSubMenu, False, strfrights.Length)

                            strContractRights = strfrights.GetValue(lngCount)
                            strfrights2 = Split(strContractRights, ";")

                            Dim dt As DataTable
                            Dim dr As DataRow
                            dt = New DataTable
                            dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
                            dt.Columns.Add(New DataColumn("fncode", GetType(String)))
                            dt.Columns.Add(New DataColumn("fnRight", GetType(String)))
                            For i = 0 To strfrights.Length - 1
                                If strfrights.GetValue(i) <> "" Then
                                    dr = dt.NewRow()
                                    dr(0) = i
                                    dr(1) = strfrights.GetValue(i)
                                    dr(2) = getfunctionalname(strfrights.GetValue(i))
                                    dt.Rows.Add(dr)
                                End If
                            Next
                            'return a DataView to the DataTable

                            grdFunctional.DataSource = New DataView(dt)
                            grdFunctional.DataBind()
                        Else
                            fn_Clear_Grid(1)
                        End If
                    Else
                        fn_Clear_Grid(1)
                        'Next

                    End If


                        Case "grdSubMenu"
                            grdSubMenu.Visible = True
                            If grdSubMenu.PageIndex < 0 Then
                                grdSubMenu.PageIndex = 0
                    End If
                    
                        strSqlQry = ""
                        strSqlQry = "select submenuid,submenuname  from submenumaster where appid='" & strAppId & "' and menu_status=1 and menuid='" & IntMenuId & "' "

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS)
                    grdSubMenu.DataSource = myDS
                    grdSubMenu.DataBind()
                    If myDS.Tables(0).Rows.Count = 0 Then
                        fn_Clear_Grid(2)
                    End If

            End Select
        Catch ex As EvaluateException
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("GroupRight.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)                   'Close connection
        End Try

    End Sub
#End Region
#Region "Private sub fn_Clear_Grid()"
    Private Sub fn_Clear_Grid(ByVal lngType As Long)
        Dim dt As DataTable
        Dim dr As DataRow
        If lngType = 1 Then
            dt = New DataTable
            dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
            dt.Columns.Add(New DataColumn("fncode", GetType(String)))
            dt.Columns.Add(New DataColumn("fnRight", GetType(String)))
            dr = dt.NewRow()
            dt.Rows.Add(dr)
            grdFunctional.DataSource = dt
            grdFunctional.DataBind()
        ElseIf lngType = 2 Then
            dt = New DataTable
            dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
            dt.Columns.Add(New DataColumn("submenuid", GetType(String)))
            dt.Columns.Add(New DataColumn("submenuname", GetType(String)))
            dr = dt.NewRow()
            dt.Rows.Add(dr)
            grdSubMenu.DataSource = dt
            grdSubMenu.DataBind()
        End If
        'End If
    End Sub
#End Region

    Private Function getfunctionalname(ByVal fcode As String) As String
        'getfunctionalname = Trim(comcls.getstring("select funname from functionalmaster where funid='" & Trim(fcode) & "'"))

        getfunctionalname = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "functionalmaster", "funname", "funid", fcode)
    End Function
    Protected Sub btnEditRight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditRight.Click
        tvMenu.Enabled = False
        btnSave.Enabled = True
        btnSaveRights.Enabled = True
        grdFunctional.Enabled = True
        btnCancelRight.Enabled = True
        btnEditRight.Enabled = False
    End Sub
    Protected Sub BtnSubMenuCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubMenuCancel.Click
        tvMenu.Enabled = True
        grdSubMenu.Enabled = True
        btnSave.Enabled = True


    End Sub


    Protected Sub btnCancelRight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelRight.Click
        tvMenu.Enabled = True
        grdFunctional.Enabled = False
        btnSave.Enabled = False
        btnCancelRight.Enabled = False
        btnEditRight.Enabled = True
    End Sub
    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
        Try
            Dim k As Integer
            colexists = False
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
                    If newcol(newkey).ToString <> "" Then
                        colexists = True
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            colexists = False
        End Try
    End Function
    Protected Sub btnSaveRights_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRights.Click
        Dim clTemp As New Collection

        If ddlGroup.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Group.');", True)
            Exit Sub
        End If
        If ddlApplication.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Application.');", True)
            Exit Sub
        End If
        If functionalrights() = "" Then


            ModalExtraPopup1.Show()


            Exit Sub

        Else

            strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
            strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

            Dim strRight As String = strGroupId & "/" & strAppId & "/" & txtMenuId.Text & "/" & functionalrights()

            clRight = CType(Session("clRight"), Collection)
            If colexists(clRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text) = False Then
                clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
            Else
                clRight.Remove(strGroupId & "/" & strAppId & "/" & txtMenuId.Text)
                clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
            End If

            Session.Add("clRight", clRight)

            tvMenu.Enabled = True
            grdFunctional.Enabled = False
            btnSaveRights.Enabled = False
            btnCancelRight.Enabled = False
            btnEditRight.Enabled = True

            grdFunctional.Enabled = False
            ddlApplication.Enabled = False
            ddlGroup.Enabled = False
            btnSave.Visible = True

        End If
    End Sub
    Public Function functionalrights() As String
        Dim strTemp As String = ""
        For Each gvrow In grdFunctional.Rows
            chkSel = gvrow.FindControl("chkSelect")
            If chkSel.Checked = True Then
                If gvrow.Cells(2).Text <> "" Then
                    strTemp = strTemp & gvrow.Cells(2).Text & ";"
                End If
            End If
        Next
        functionalrights = strTemp
    End Function
    Protected Sub ddlApplication_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlApplication.SelectedIndexChanged
        If ddlGroup.Enabled = True And ddlApplication.Enabled = True Then
            If ddlGroup.SelectedValue <> "[Select]" And ddlApplication.SelectedValue <> "[Select]" Then
                strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
                strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)
                Fill_Tree_View(strGroupId, strAppId)
                fn_Clear_Grid(1)
                fn_Clear_Grid(2)
            End If
        End If

    End Sub
    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        If ViewState("GrpRightsState") = "New" Then
            If ddlGroup.SelectedValue <> "[Select]" Then
                strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
                strSqlQry = "select * from appmaster where appstatus=1 and appid not in(select distinct  " & _
                " appid from group_rights where groupid='" & strGroupId & "') order by appid"
                objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlApplication, "appname", strSqlQry, True)
            Else
                objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlApplication, "appname", "select * from appmaster where appstatus=1 order by appid", True)
            End If
        End If
        If ddlGroup.Enabled = True And ddlApplication.Enabled = True Then
            If ddlGroup.SelectedValue <> "[Select]" And ddlApplication.SelectedValue <> "[Select]" Then
                strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
                strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)
                Fill_Tree_View(strGroupId, strAppId)
                fn_Clear_Grid(1)
                fn_Clear_Grid(2)
            End If
        End If
    End Sub
    Protected Sub btnSaveSubMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSubMenu.Click
        Dim clTemp As New Collection
        Dim flag As Boolean = False
        For Each gvrow In grdSubMenu.Rows
            chkSel = gvrow.FindControl("chkSelect")
            If chkSel.Checked = True Then
                If gvrow.Cells(2).Text <> "" Then
                    flag = True
                End If
            End If
        Next

        If ddlGroup.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Group.');", True)
            Exit Sub
        End If
        If ddlApplication.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Application.');", True)
            Exit Sub
        End If
        If flag = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Sub Menu Right.');", True)
            Exit Sub
        End If

        strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
        strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

        Dim strkey As String = strGroupId & "/" & strAppId & "/" & txtMenuId.Text

        clSubMenu = CType(Session("clSubMenu"), Collection)

        For Each gvrow In grdSubMenu.Rows
            chkSel = gvrow.FindControl("chkSelect")
            'If chkSel.Checked = True Then
            If gvrow.Cells(2).Text <> "" Then
                If colexists(clSubMenu, strkey & "/" & gvrow.Cells(2).Text) = False Then
                    clSubMenu.Add(strkey & "/" & gvrow.Cells(2).Text & "/" & chkSel.Checked, strkey & "/" & gvrow.Cells(2).Text, Nothing, Nothing)
                Else
                    clSubMenu.Remove(strkey & "/" & gvrow.Cells(2).Text)
                    clSubMenu.Add(strkey & "/" & gvrow.Cells(2).Text & "/" & chkSel.Checked, strkey & "/" & gvrow.Cells(2).Text, Nothing, Nothing)
                End If
            End If
            'End If
        Next
        Session.Add("clSubMenu", clSubMenu)
        grdSubMenu.Enabled = False
    End Sub
    Protected Sub btnClearGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearGroup.Click
        clRight = New Collection
        clSubMenu = New Collection
        fn_Clear_Grid(1)
        fn_Clear_Grid(2)
        ddlApplication.Enabled = True
        ddlGroup.Enabled = True
    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        clRight = CType(Session("clRight"), Collection)
        If ddlGroup.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Group.');", True)
            ValidatePage = False
            Exit Function
        End If
        If ddlApplication.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Application.');", True)
            ValidatePage = False
            Exit Function
        End If

        'we will not always have entry in group_functionalrights so to avoid the error message commented the below validation-shagun

        'If clRight.Count = 0 Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select At List One Menu Right.');", True)
        '    ValidatePage = False
        '    Exit Function
        'End If 


        ValidatePage = True
    End Function
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Dim strRes As String
        strRes = ""
        strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
        strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)
        If ViewState("GrpRightsState") = "New" Then
            strSqlQry = "select 't'   from group_rights where appid ='" & strAppId & "' and groupid='" & strGroupId & "'"
            strRes = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
            If strRes = "t" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group Id and Application Id is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim i, j As Integer
        Dim adddate As DateTime
        Dim adduser As String
        adduser = ""
        Try
            If Page.IsValid = True Then
                If ViewState("GrpRightsState") = "New" Or ViewState("GrpRightsState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    clRight = CType(Session("clRight"), Collection)
                    clSubMenu = CType(Session("clSubMenu"), Collection)

                    strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
                    strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

                    If ViewState("GrpRightsState") = "Edit" Then
                        adddate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  min(adddate)  from group_rights where appid='" & strAppId & "' and groupid='" & strGroupId & "'")
                        adduser = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  distinct adduser  from group_rights where appid='" & strAppId & "' and groupid='" & strGroupId & "'")
                        mySqlCmd = New SqlCommand("sp_del_grouprights", mySqlConn, sqlTrans)
                        mySqlCmd.CommandText = "sp_del_grouprights"
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@appid", strAppId))
                        mySqlCmd.Parameters.Add(New SqlParameter("@grpid ", strGroupId))
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String)))
                        mySqlCmd.ExecuteNonQuery()
                    End If
                    'Header Menus only
                    For i = 0 To tvMenu.Nodes.Count - 1
                        mySqlCmd = New SqlCommand("sp_add_group_rights", mySqlConn, sqlTrans)
                        mySqlCmd.CommandText = "sp_add_group_rights"
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Connection = mySqlConn
                        mySqlCmd.Transaction = sqlTrans
                        mySqlCmd.Parameters.Add(New SqlParameter("@appid", strAppId))
                        mySqlCmd.Parameters.Add(New SqlParameter("@menuid", tvMenu.Nodes.Item(i).Value))

                        mySqlCmd.Parameters.Add(New SqlParameter("@groupid ", strGroupId))
                        If tvMenu.Nodes.Item(i).Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active ", 1))
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@active ", 0))
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String)))
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", objdate.GetSystemDateTime(Session("dbconnectionName"))))
                        mySqlCmd.ExecuteNonQuery()
                    Next
                    'Sub Menus only
                    For i = 0 To tvMenu.Nodes.Count - 1
                        If tvMenu.Nodes.Item(i).Checked = True Then
                            For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
                                mySqlCmd = New SqlCommand("sp_add_group_rights", mySqlConn, sqlTrans)
                                mySqlCmd.CommandText = "sp_add_group_rights"
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Connection = mySqlConn
                                mySqlCmd.Transaction = sqlTrans
                                mySqlCmd.Parameters.Add(New SqlParameter("@appid", strAppId))
                                mySqlCmd.Parameters.Add(New SqlParameter("@menuid", tvMenu.Nodes.Item(i).ChildNodes.Item(j).Value))
                                mySqlCmd.Parameters.Add(New SqlParameter("@groupid ", strGroupId))
                                If tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked Then
                                    mySqlCmd.Parameters.Add(New SqlParameter("@active ", 1))
                                Else
                                    mySqlCmd.Parameters.Add(New SqlParameter("@active ", 0))
                                End If

                                mySqlCmd.Parameters.Add(New SqlParameter("@adddate", objdate.GetSystemDateTime(Session("dbconnectionName"))))
                                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String)))
                                mySqlCmd.ExecuteNonQuery()
                            Next
                        End If
                    Next

                    Dim strfnRight() As String
                    clRight = CType(Session("clRight"), Collection)
                    'For i = 1 To clRight.Count
                    '    strfnRight = clRight(i).ToString.Split("/")
                    '    If strAppId = strfnRight.GetValue(1) And strGroupId = strfnRight.GetValue(0) Then
                    '        mySqlCmd = New SqlCommand("sp_add_group_rights", mySqlConn, sqlTrans)
                    '        mySqlCmd.CommandText = "sp_add_group_rights"
                    '        mySqlCmd.CommandType = CommandType.StoredProcedure
                    '        mySqlCmd.Connection = mySqlConn
                    '        mySqlCmd.Transaction = sqlTrans
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@appid", strfnRight.GetValue(1)))
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@menuid", strfnRight.GetValue(2)))
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@groupid ", strfnRight.GetValue(0)))
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@active ", 1))
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String)))
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", objdate.GetSystemDateTime))
                    '        mySqlCmd.ExecuteNonQuery()
                    '    End If
                    'Next
                    i = 0
                    '//Save Functional Rights
                    For i = 1 To clRight.Count
                        strfnRight = clRight(i).ToString.Split("/")
                        If strAppId = strfnRight.GetValue(1) And strGroupId = strfnRight.GetValue(0) Then
                            mySqlCmd = New SqlCommand("sp_add_group_functionalrights", mySqlConn, sqlTrans)
                            mySqlCmd.CommandText = "sp_add_group_functionalrights"
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Connection = mySqlConn
                            mySqlCmd.Transaction = sqlTrans
                            mySqlCmd.Parameters.Add(New SqlParameter("@grpid ", strfnRight.GetValue(0)))
                            mySqlCmd.Parameters.Add(New SqlParameter("@appid", strfnRight.GetValue(1)))
                            mySqlCmd.Parameters.Add(New SqlParameter("@menuid", strfnRight.GetValue(2)))
                            mySqlCmd.Parameters.Add(New SqlParameter("@functional_rights ", strfnRight.GetValue(3)))
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    ' Save Sub Menu Right
                    Dim strSubMenuRight As String()
                    For i = 1 To clSubMenu.Count
                        strSubMenuRight = clSubMenu(i).ToString.Split("/")
                        If strAppId = strSubMenuRight.GetValue(1) And strGroupId = strSubMenuRight.GetValue(0) Then
                            mySqlCmd = New SqlCommand("sp_add_group_submenurights", mySqlConn, sqlTrans)
                            mySqlCmd.CommandText = "sp_add_group_submenurights"
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Connection = mySqlConn
                            mySqlCmd.Transaction = sqlTrans
                            mySqlCmd.Parameters.Add(New SqlParameter("@appid", strSubMenuRight.GetValue(1)))
                            mySqlCmd.Parameters.Add(New SqlParameter("@grpid ", strSubMenuRight.GetValue(0)))
                            mySqlCmd.Parameters.Add(New SqlParameter("@menuid", strSubMenuRight.GetValue(2)))
                            mySqlCmd.Parameters.Add(New SqlParameter("@Submenuid ", strSubMenuRight.GetValue(3)))
                            If strSubMenuRight.GetValue(4) = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@active ", 1))
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@active ", 0))
                            End If
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    If ViewState("GrpRightsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_update_group_rights", mySqlConn, sqlTrans)
                        mySqlCmd.CommandText = "sp_update_group_rights"
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@appid", strAppId))
                        mySqlCmd.Parameters.Add(New SqlParameter("@grpid ", strGroupId))
                        mySqlCmd.Parameters.Add(New SqlParameter("@adduser", adduser))
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", adddate))
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String)))
                        mySqlCmd.Parameters.Add(New SqlParameter("@moddate", objdate.GetSystemDateTime(Session("dbconnectionName"))))
                        mySqlCmd.ExecuteNonQuery()
                    End If
                ElseIf ViewState("GrpRightsState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
                    strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

                    mySqlCmd = New SqlCommand("sp_del_grouprights", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@appid", strAppId))
                    mySqlCmd.Parameters.Add(New SqlParameter("@grpid ", strGroupId))
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String)))
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            'Response.Redirect("GroupRightSearch.aspx", False)
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('GrpRightsWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("GroupRight.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub DeleteMenu(ByVal applicationid As Integer, ByVal groupid As Integer, ByVal menuid As Integer, ByVal savecon As SqlConnection, ByVal savesqltran As SqlTransaction, ByVal activebool As Boolean)
        Dim myCmd As New SqlCommand("sp_del_grouprights", savecon, savesqltran)
        myCmd.CommandText = "sp_del_grouprights"
        myCmd.Connection = savecon
        myCmd.Transaction = savesqltran
        myCmd.CommandType = CommandType.StoredProcedure
        myCmd.Parameters.Add(New SqlParameter("@appid", applicationid))
        myCmd.Parameters.Add(New SqlParameter("@menuid", menuid))
        myCmd.Parameters.Add(New SqlParameter("@grpid ", groupid))
        myCmd.ExecuteNonQuery()
    End Sub
    Private Sub SaveMenu(ByVal applicationid As Integer, ByVal groupid As Integer, ByVal menuid As Integer, ByVal savecon As SqlConnection, ByVal savesqltran As SqlTransaction, ByVal activebool As Boolean)

        If ViewState("GrpRightsState") = "Edit" Then
            Dim myCmd As New SqlCommand("sp_del_grouprights", savecon, savesqltran)
            myCmd.CommandText = "sp_del_grouprights"
            myCmd.Connection = savecon
            myCmd.Transaction = savesqltran
            myCmd.CommandType = CommandType.StoredProcedure
            myCmd.Parameters.Add(New SqlParameter("@appid", applicationid))
            myCmd.Parameters.Add(New SqlParameter("@menuid", menuid))
            myCmd.Parameters.Add(New SqlParameter("@grpid ", groupid))
            myCmd.ExecuteNonQuery()
        End If

        Dim mySqlCmd As New SqlCommand("sp_add_group_rights", savecon, savesqltran)
        mySqlCmd.CommandText = "sp_add_group_rights"
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Connection = savecon
        mySqlCmd.Transaction = savesqltran
        mySqlCmd.Parameters.Add(New SqlParameter("@appid", applicationid))
        mySqlCmd.Parameters.Add(New SqlParameter("@menuid", menuid))
        mySqlCmd.Parameters.Add(New SqlParameter("@groupid ", groupid))
        mySqlCmd.Parameters.Add(New SqlParameter("@active ", activebool))
        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String)))
        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", objdate.GetSystemDateTime(Session("dbconnectionName"))))
        mySqlCmd.ExecuteNonQuery()

    End Sub
    Private Sub SaveRights(ByVal applicationid As Integer, ByVal groupid As Integer, ByVal menuid As String, ByVal savecon As SqlConnection, ByVal savesqltran As SqlTransaction, ByVal grouprights As String)
        Dim mySqlCmd As New SqlCommand("sp_add_group_functionalrights", savecon, savesqltran)
        mySqlCmd.CommandText = "sp_add_group_functionalrights"
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Connection = savecon
        mySqlCmd.Transaction = savesqltran
        mySqlCmd.Parameters.Add(New SqlParameter("@grpid ", groupid))
        mySqlCmd.Parameters.Add(New SqlParameter("@appid", applicationid))
        mySqlCmd.Parameters.Add(New SqlParameter("@menuid", menuid))
        mySqlCmd.Parameters.Add(New SqlParameter("@functional_rights ", grouprights))
        mySqlCmd.ExecuteNonQuery()
    End Sub
    Private Sub SaveSubMenuRights(ByVal applicationid As Integer, ByVal groupid As Integer, ByVal menuid As String, ByVal savecon As SqlConnection, ByVal savesqltran As SqlTransaction, ByVal submenuid As Integer, ByVal activebool As Boolean)
        Dim mySqlCmd As New SqlCommand("sp_add_group_submenurights", savecon, savesqltran)
        mySqlCmd.CommandText = "sp_add_group_submenurights"
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Connection = savecon
        mySqlCmd.Transaction = savesqltran
        mySqlCmd.Parameters.Add(New SqlParameter("@appid", applicationid))
        mySqlCmd.Parameters.Add(New SqlParameter("@grpid ", groupid))
        mySqlCmd.Parameters.Add(New SqlParameter("@menuid", menuid))
        mySqlCmd.Parameters.Add(New SqlParameter("@Submenuid ", submenuid))
        mySqlCmd.Parameters.Add(New SqlParameter("@active ", activebool))
        mySqlCmd.ExecuteNonQuery()
    End Sub

    Protected Sub tvMenu_TreeNodeCheckChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles tvMenu.TreeNodeCheckChanged

    End Sub

    Protected Sub tvMenu_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles tvMenu.TreeNodePopulate

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("GroupRightSearch.aspx", False)
    End Sub

    Protected Sub btnSaveRightsAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clTemp As New Collection
        Dim i, j As Integer
        Dim FlagTv As Boolean = False
        If ddlGrpFunCode.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Functional Rights .');", True)
            Exit Sub
        End If
        If ddlGroup.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Group.');", True)
            Exit Sub
        End If
        If ddlApplication.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Application.');", True)
            Exit Sub
        End If
        For i = 0 To tvMenu.Nodes.Count - 1
            j = 1
            If tvMenu.Nodes.Item(i).Checked = True Then
                For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
                    txtMenuId.Text = tvMenu.Nodes.Item(i).ChildNodes.Item(j).Value
                    If tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked Then
                        FlagTv = True
                    End If
                Next
            End If
        Next

        If FlagTv = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Menu.');", True)
            Exit Sub
        End If

        strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
        strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

        Dim strRight As String
        Dim strRightTemp As String
        Dim strfnRight() As String
        For i = 0 To tvMenu.Nodes.Count - 1
            j = 1
            If tvMenu.Nodes.Item(i).Checked = True Then
                For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
                    txtMenuId.Text = tvMenu.Nodes.Item(i).ChildNodes.Item(j).Value
                    If tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked = True Then
                        clRight = CType(Session("clRight"), Collection)
                        If colexists(clRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text) = False Then
                            strRight = strGroupId & "/" & strAppId & "/" & txtMenuId.Text & "/" & FunctionalRightsUpdate("", ddlGrpFunCode.Items(ddlGrpFunCode.SelectedIndex).Value.Trim)
                            clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
                        Else
                            strRightTemp = clRight.Item(strGroupId & "/" & strAppId & "/" & txtMenuId.Text)
                            strfnRight = strRightTemp.ToString.Split("/")
                            strRight = strGroupId & "/" & strAppId & "/" & txtMenuId.Text & "/" & FunctionalRightsUpdate(strfnRight.GetValue(3), ddlGrpFunCode.Items(ddlGrpFunCode.SelectedIndex).Value.Trim)
                            clRight.Remove(strGroupId & "/" & strAppId & "/" & txtMenuId.Text)
                            clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
                        End If
                        Session.Add("clRight", clRight)
                    End If
                Next
            End If
        Next


        FillTreeEdit()
        ddlGrpFunCode.Value = "[Select]"
        fn_Clear_Grid(1)
        ddlApplication.Enabled = False
        ddlGroup.Enabled = False
        btnSave.Visible = True
    End Sub
    Protected Sub btnRemoveRightsAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clTemp As New Collection
        Dim i, j As Integer
        Dim FlagTv As Boolean = False
        If ddlGrpFunCode.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Functional Rights .');", True)
            Exit Sub
        End If
        If ddlGroup.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Group.');", True)
            Exit Sub
        End If
        If ddlApplication.SelectedValue = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Application.');", True)
            Exit Sub
        End If
        For i = 0 To tvMenu.Nodes.Count - 1
            j = 1
            If tvMenu.Nodes.Item(i).Checked = True Then
                For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
                    txtMenuId.Text = tvMenu.Nodes.Item(i).ChildNodes.Item(j).Value
                    If tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked Then
                        FlagTv = True
                    End If
                Next
            End If
        Next

        If FlagTv = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Menu.');", True)
            Exit Sub
        End If

        strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
        strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

        Dim strRight As String
        Dim strRightTemp As String
        Dim strfnRight() As String
        For i = 0 To tvMenu.Nodes.Count - 1
            j = 1
            If tvMenu.Nodes.Item(i).Checked = True Then
                For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
                    txtMenuId.Text = tvMenu.Nodes.Item(i).ChildNodes.Item(j).Value
                    If tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked = True Then
                        clRight = CType(Session("clRight"), Collection)
                        If colexists(clRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text) = False Then
                            'strRight = strGroupId & "/" & strAppId & "/" & txtMenuId.Text & "/" & FunctionalRightsUpdate("", ddlGrpFunCode.Items(ddlGrpFunCode.SelectedIndex).Value.Trim)
                            'clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
                        Else
                            strRightTemp = clRight.Item(strGroupId & "/" & strAppId & "/" & txtMenuId.Text)
                            strfnRight = strRightTemp.ToString.Split("/")
                            strRight = strGroupId & "/" & strAppId & "/" & txtMenuId.Text & "/" & FunctionalRightsRemove(strfnRight.GetValue(3), ddlGrpFunCode.Items(ddlGrpFunCode.SelectedIndex).Value.Trim)
                            clRight.Remove(strGroupId & "/" & strAppId & "/" & txtMenuId.Text)
                            clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
                        End If
                        Session.Add("clRight", clRight)
                    End If
                Next
            End If
        Next


        FillTreeEdit()
        ddlGrpFunCode.Value = "[Select]"
        fn_Clear_Grid(1)
        ddlApplication.Enabled = False
        ddlGroup.Enabled = False
        btnSave.Visible = True
    End Sub


    Private Sub FillTreeEdit()
        Dim i, j As Integer
        Dim FlagAll As Boolean = False
        clRight = CType(Session("clRight"), Collection)
        For i = 0 To tvMenu.Nodes.Count - 1
            j = 1
            For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
                txtMenuId.Text = tvMenu.Nodes.Item(i).ChildNodes.Item(j).Value
                If colexists(clRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text) = True Then
                    tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked = True
                    FlagAll = True
                Else
                    tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked = False
                    ' FlagAll = False
                End If
            Next
            If FlagAll = True Then
                tvMenu.Nodes.Item(i).Checked = True
                FlagAll = False
            Else
                tvMenu.Nodes.Item(i).Checked = False
            End If
        Next
    End Sub

    Protected Sub BtnMenuClear_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i, j As Integer
        For i = 0 To tvMenu.Nodes.Count - 1
            j = 1
            ' If tvMenu.Nodes.Item(i).Checked = True Then
            For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
                '   If tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked = True Then
                tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked = False
                'End If
            Next
            tvMenu.Nodes.Item(i).Checked = False
            'End If
        Next
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=GroupRight','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
        strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "appmaster", "appid", "appname", ddlApplication.SelectedValue)

        Dim strRight As String = strGroupId & "/" & strAppId & "/" & txtMenuId.Text & "/" & functionalrights()

        clRight = CType(Session("clRight"), Collection)
        If colexists(clRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text) = False Then
            clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
        Else
            clRight.Remove(strGroupId & "/" & strAppId & "/" & txtMenuId.Text)
            clRight.Add(strRight, strGroupId & "/" & strAppId & "/" & txtMenuId.Text, Nothing, Nothing)
        End If

        Session.Add("clRight", clRight)

        tvMenu.Enabled = True
        grdFunctional.Enabled = False
        btnSaveRights.Enabled = False
        btnCancelRight.Enabled = False
        btnEditRight.Enabled = True

        grdFunctional.Enabled = False
        ddlApplication.Enabled = False
        ddlGroup.Enabled = False
        btnSave.Visible = True

    End Sub

    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select functional rights.');", True)

    End Sub
End Class

'Dim strkey As String = strGroupId & "/" & strAppId & "/" & txtMenuId.Text
'For Each gvrow In grdSubMenu.Rows
'    chkSel = gvrow.FindControl("chkSelect")
'    'If chkSel.Checked = True Then
'    If gvrow.Cells(2).Text <> "" Then
'        If colexists(clSubMenu, strkey & "/" & gvrow.Cells(2).Text) = False Then
'            clSubMenu.Add(strkey & "/" & gvrow.Cells(2).Text & "/" & chkSel.Checked, strkey & "/" & gvrow.Cells(2).Text, Nothing, Nothing)
'        Else
'            clSubMenu.Remove(strkey & "/" & gvrow.Cells(2).Text)
'            clSubMenu.Add(strkey & "/" & gvrow.Cells(2).Text & "/" & chkSel.Checked, strkey & "/" & gvrow.Cells(2).Text, Nothing, Nothing)
'        End If
'    End If
'    'End If
'Next
'Session.Add("clSubMenu", clSubMenu)

'groupid/appid/menuid/menuid
'For i = 1 To clSubMenu.Count
'    strSubMenuRight = clSubMenu(i).ToString.Split("/")

'        strSubMenuRight.GetValue(4)
'    End If
'SaveSubMenuRights(strSubMenuRight.GetValue(1), strSubMenuRight.GetValue(0), strSubMenuRight.GetValue(2), mySqlConn, sqlTrans, strSubMenuRight.GetValue(3), strSubMenuRight.GetValue(4))
'SaveSubMenuRights(ByVal applicationid As Integer, ByVal groupid As Integer, ByVal menuid As String, ByVal savecon As SqlConnection, ByVal savesqltran As SqlTransaction, ByVal submenuid As Integer, ByVal activebool As Boolean)
'Next

'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

'    Dim i, j As Integer
'    Dim passmenuid As String
'    Dim passmenustatus As Boolean
'    Try
'        If Page.IsValid = True Then

'            clRight = CType(Session("clRight"), Collection)
'            clSubMenu = CType(Session("clSubMenu"), Collection)

'            If clRight.Count = 0 Then
'                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select At List One Menu Right.');", True)
'                Exit Sub
'            End If


'            strGroupId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"groupmaster", "groupid", "groupname", ddlGroup.SelectedValue)
'            strAppId = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"appmaster", "appid", "appname", ddlApplication.SelectedValue)

'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

'            For i = 0 To tvMenu.Nodes.Count - 1
'                j = 1
'                passmenuid = tvMenu.Nodes.Item(i).Value
'                passmenustatus = tvMenu.Nodes.Item(i).Checked
'                If ViewState("GrpRightsState") <> "Delete" Then
'                    SaveMenu(strAppId, strGroupId, passmenuid, mySqlConn, sqlTrans, passmenustatus)
'                Else
'                    DeleteMenu(strAppId, strGroupId, passmenuid, mySqlConn, sqlTrans, passmenustatus)
'                End If
'                For j = 0 To tvMenu.Nodes.Item(i).ChildNodes.Count - 1
'                    passmenuid = tvMenu.Nodes.Item(i).ChildNodes.Item(j).Value
'                    passmenustatus = tvMenu.Nodes.Item(i).ChildNodes.Item(j).Checked
'                    If ViewState("GrpRightsState") <> "Delete" Then
'                        SaveMenu(strAppId, strGroupId, passmenuid, mySqlConn, sqlTrans, passmenustatus)
'                    Else
'                        DeleteMenu(strAppId, strGroupId, passmenuid, mySqlConn, sqlTrans, passmenustatus)
'                    End If
'                Next
'            Next
'            If ViewState("GrpRightsState") = "Delete" Then
'                Exit Sub
'            End If
'            i = 0

'            '//Save Functional Rights
'            Dim strfnRight As String()
'            'groupid/appid/menuid/funRights
'            For i = 1 To clRight.Count
'                strfnRight = clRight(i).ToString.Split("/")
'                SaveRights(strfnRight.GetValue(1), strfnRight.GetValue(0), strfnRight.GetValue(2), mySqlConn, sqlTrans, strfnRight.GetValue(3))
'            Next

'            '// Save Sub Menu Right
'            Dim strSubMenuRight As String()
'            'groupid/appid/menuid/menuid
'            For i = 1 To clSubMenu.Count
'                strSubMenuRight = clSubMenu(i).ToString.Split("/")
'                SaveSubMenuRights(strSubMenuRight.GetValue(1), strSubMenuRight.GetValue(0), strSubMenuRight.GetValue(2), mySqlConn, sqlTrans, strSubMenuRight.GetValue(3), strSubMenuRight.GetValue(4))
'            Next

'            sqlTrans.Commit()
'            mySqlConn.Close()
'        End If

'        Response.Redirect("GroupRightSearch.aspx", False)
'    Catch ex As Exception
'        mySqlConn.Close()
'        objUtils.WritErrorLog("GroupRight.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'    End Try
'End Sub

'If ViewState("GrpRightsState") = "Edit" Then
'    ShowGrid("grdFunctional", intMenuId, CType(Session("AppId"), String), CType(Session("Groupid"), String))
'    ShowGrid("grdSubMenu", intMenuId, CType(Session("AppId"), String), CType(Session("Groupid"), String))
'    grdFunctional.Enabled = False
'End If
