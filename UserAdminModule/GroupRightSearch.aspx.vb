'------------------------------------------------------------------------------------------------- 
'   Page Name       : GroupRightSearch.aspx
'   Developer Name  : Mangesh 
'   Date            : 11 August 2008
'   
'-------------------------------------------------------------------------------------------------

Imports System.Data
Imports System.Data.SqlClient

Partial Class GroupRightSearch
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region
#Region "Enum GridCol"
    Enum GridCol
        AppID = 0
        AppName = 1
        GroupID = 2
        GroupName = 3
        DateCreated = 6
        UserCreated = 7
        DateModified = 8
        UserModified = 9
        Edit = 10
        View = 11
        Delete = 12
    End Enum
#End Region
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessApp As String = ""
        Dim lsProcessgrp As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim

                Case "GROUP"
                    lsProcessgrp = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("GROUP", lsProcessgrp, "G")

                Case "APPLICATION"
                    lsProcessApp = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("APPLICATION", lsProcessApp, "A")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamicgrprgt")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Private Function BuildConditionNew(ByVal strGroupValue As String, ByVal strappname As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strGroupValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(groupmaster. groupname) IN (" & Trim(strGroupValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(groupmaster.groupname) IN (" & Trim(strGroupValue.Trim.ToUpper) & ")"
            End If
        End If
        If strappname.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(appmaster. appname) IN (" & Trim(strappname.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(appmaster.appname) IN (" & Trim(strappname.Trim.ToUpper) & ")"
            End If
        End If
        If strTextValue <> "" Then
            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then
                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = "upper(groupmaster.groupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(appmaster.appname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(groupmaster.groupid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(groupmaster.groupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(appmaster.appname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(groupmaster.groupid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If
        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),groupmaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),groupmaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),groupmaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),groupmaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If


        BuildConditionNew = strWhereCond
    End Function
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamicgrprgt")
        Dim strGroupValue As String = ""
        Dim strappValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "GROUP" Then
                        If strGroupValue <> "" Then
                            strGroupValue = strGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "APPLICATION" Then
                        If strappValue <> "" Then
                            strappValue = strappValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strappValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                    End If
                Next
            End If
            Dim pagevaluems = RowsPerPageMS.SelectedValue
            strBindCondition = BuildConditionNew(strGroupValue, strappValue, strTextValue)
            Dim myDS As New DataSet

            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0

            End If
            strSqlQry = ""
            Dim strorderby As String = Session("grprtstrsortExpression")
            Dim strsortorder As String = "ASC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"
            strSqlQry = "select distinct groupmaster .groupid,appmaster.appid,appmaster.appname appname ," & _
           " groupmaster.groupname, groupmaster.adddate, groupmaster.adduser, " & _
                      "  groupmaster.moddate moddate,groupmaster.moduser moduser " & _
                         "from group_rights inner join   groupmaster on " & _
           " group_rights.groupid = groupmaster.groupid And groupmaster.active = 1" & _
                       " inner join appmaster on group_rights.appid = appmaster.appid and appmaster.appstatus=1 "

            '"select distinct group_rights.groupid,group_rights.appid,appmaster.appname appname ," & _
            '            " groupmaster.groupname , group_rights.adddate, group_rights.adduser , " & _
            '            "group_rights.moddate moddate,group_rights.moduser moduser " & _
            '            " from group_rights inner join   groupmaster on  " & _
            '            " group_rights.groupid=groupmaster.groupid  and groupmaster.active=1" & _
            '            " inner join appmaster on group_rights.appid = appmaster.appid and appmaster.appstatus=1 "
                      If strBindCondition <> "" Then

                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            gv_SearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = pagevaluems
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicgrprgt")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicgrprgt") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicgrprgt")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamicgrprgt") = dtt
            End If
        End If
        Return True
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try


                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                '  SetFocus(ddlGrpCode)
               
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "UserAdminModule\GroupRightSearch.aspx", BtnAddNew, BtnExportToExcel, _
                                                       BtnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                RowsPerPageMS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpCode, "groupid", "groupname", "select groupid,groupname from groupmaster  where active=1 order by groupid", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "groupname", "groupid", "select groupname,groupid from groupmaster  where active=1 order by groupname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAppCode, "appid", "appname", "select appid,appname from appmaster  where appstatus=1 order by appid", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAppName, "appname", "appid", "select appname,appid from appmaster  where appstatus=1 order by appname", True)
                Session("sDtDynamicgrprgt") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamicgrprgt") = dtDynamic
                Session.Add("grprtstrsortExpression", "groupid")
                Session.Add("grprtstrsortdirection", SortDirection.Ascending)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    '    ddlGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '    ddlGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '    ddlAppCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '    ddlAppName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("GroupRightSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        FillGridNew()

        '  BtnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "GrpRightsWindowPostBack") Then
            '   btnSearch_Click(sender, e)
        End If

    End Sub
#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
        
            strSqlQry = "select distinct group_rights.groupid,group_rights.appid,max(appmaster.appname) appname ," & _
                        " max(groupmaster.groupname) groupname,max(group_rights.adddate) adddate,max(group_rights.adduser) adduser, " & _
                        " max(group_rights.moddate) moddate,max(group_rights.moduser) moduser " & _
                        " from group_rights inner join   groupmaster on  " & _
                        " group_rights.groupid=groupmaster.groupid  and groupmaster.active=1" & _
                        " inner join appmaster on group_rights.appid = appmaster.appid and appmaster.appstatus=1  group by group_rights.adddate,group_rights.adduser,group_rights.moddate,group_rights.moduser,group_rights.groupid,group_rights.appid"

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & "  where  " & BuildCondition() & " group by group_rights.appid, group_rights.groupid ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " group by group_rights.appid, group_rights.groupid ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found. Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("GroupRightSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        'If ddlGrpCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(groupmaster.groupid) = '" & Trim(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(groupmaster.groupid) = '" & Trim(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If
        'If ddlAppCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " upper(appmaster.appid ) = '" & Trim(ddlAppCode.Items(ddlAppCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(appmaster.appid) = '" & Trim(ddlAppCode.Items(ddlAppCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If

        BuildCondition = strWhereCond
    End Function
#End Region
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("GroupRight.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('GroupRight.aspx?State=New','GrpRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('GroupRight.aspx?State=New','GrpRights');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region
#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As New SqlConnection
        Dim objcon As New clsDBConnect


        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = "select groupmaster.groupid as [Group ID],groupmaster.groupname as [Group Name],group_app_detail.appid as [App ID], " & _
                            "appmaster.appname as [App Name] ,group_app_master.adddate as [Date Created] ,group_app_master.adduser as [User Created] " & _
                            " ,group_app_master.moddate [Date Modified],group_app_master.moduser as [User Modified]  " & _
                          "  from group_app_master,groupmaster,group_app_detail,appmaster  where group_app_master.groupid=groupmaster.groupid and " & _
                          " group_app_master.groupid=group_app_detail.groupid and " & _
                          " group_app_detail.appid = appmaster.appid "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " and  " & BuildCondition() & " ORDER BY groupmaster.groupid "
                Else
                    strSqlQry = strSqlQry & " ORDER BY groupmaster.groupid"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "groupmaster")

                objUtils.ExportToExcel(DS, Response)

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            clsDBConnect.dbConnectionClose(con)

        End Try
    End Sub
#End Region

    Protected Sub gv_SearchResult_PageIndexChanged(sender As Object, e As System.EventArgs) Handles gv_SearchResult.PageIndexChanged

    End Sub

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        ' FillGrid("appid")

        FillGridNew()
    End Sub
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region
#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    ''Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
    ''    'ddlGrpCode.Value = "[Select]"
    ''    'ddlGrpName.Value = "[Select]"
    ''    'ddlAppCode.Value = "[Select]"
    ''    'ddlAppName.Value = "[Select]"
    ''    FillGrid("groupid")
    ''End Sub
#End Region
    '#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    '    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
    '        FillGrid("groupid")
    '    End Sub

    '#End Region
#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblaId, lblgId As Label
            lblaId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblAppid")
            lblgId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblgroupid")

            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('GroupRight.aspx?State=Edit&AppId=" + CType(lblaId.Text.Trim, String) + "&Groupid=" + CType(lblgId.Text.Trim, String) + "','GrpRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('GroupRight.aspx?State=Edit&AppId=" + CType(lblaId.Text.Trim, String) + "&Groupid=" + CType(lblgId.Text.Trim, String) + "','GrpRights');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                'strpop = "window.open('GroupRight.aspx?State=View&AppId=" + CType(lblaId.Text.Trim, String) + "&Groupid=" + CType(lblgId.Text.Trim, String) + "','GrpRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('GroupRight.aspx?State=View&AppId=" + CType(lblaId.Text.Trim, String) + "&Groupid=" + CType(lblgId.Text.Trim, String) + "','GrpRights');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('GroupRight.aspx?State=Delete&AppId=" + CType(lblaId.Text.Trim, String) + "&Groupid=" + CType(lblgId.Text.Trim, String) + "','GrpRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('GroupRight.aspx?State=Delete&AppId=" + CType(lblaId.Text.Trim, String) + "&Groupid=" + CType(lblgId.Text.Trim, String) + "','GrpRights');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("GroupRightSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session.Add("Pageame", "Group Rights")
            'Session.Add("BackPageName", "GroupRightSearch.aspx")
            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=Group Rights&BackPageName=GroupRightSearch.aspx&GrpCode=" & Trim(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text) & "&AppCode=" & Trim(ddlAppCode.Items(ddlAppCode.SelectedIndex).Text) & "','GrpRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ' strpop = "window.open('rptReportNew.aspx?Pageame=Group Rights&BackPageName=GroupRightSearch.aspx&GrpCode="m(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text) & "&AppCode=" & Trim(ddlAppCode.Items(ddlAppCode.SelectedIndex).Text) & "','GrpRights');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Group Rights&BackPageName=GroupRightSearch.aspx ','GrpRights');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=GroupRightSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub RowsPerPageMS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageMS.SelectedIndexChanged
        FillGridNew()
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblgroupname As Label = e.Row.FindControl("lblgroupname")
            Dim lblappname As Label = e.Row.FindControl("lblappname")
            Dim lsgroupName As String = ""

            Dim lsapplicationName As String = ""

            Dim lstextvalue As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicgrprgt")
            If Session("sDtDynamicgrprgt") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsgroupName = ""


                        If "GROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsgroupName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "APPLICATION" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsapplicationName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lstextvalue = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsgroupName.Trim <> "" Then
                            lblgroupname.Text = Regex.Replace(lblgroupname.Text.Trim, lsgroupName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lsapplicationName.Trim <> "" Then
                            lblappname.Text = Regex.Replace(lblappname.Text.Trim, lsapplicationName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UserMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnClearDate_Click(sender As Object, e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamicgrprgt")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamicgrprgt") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()
    End Sub
End Class
