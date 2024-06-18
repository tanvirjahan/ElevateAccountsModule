'------------================--------------=======================------------------================
'   Module Name    :    PrivilegeRightsforGroupSearch.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PrivilegeRightsforGroupSearch
    Inherits System.Web.UI.Page

#Region "Enum GridCol"
    Enum GridCol
        GroupID = 1
        GroupName = 2
        AppID = 3
        AppName = 4
        DateCreated = 5
        UserCreated = 6
        DateModified = 7
        UserModified = 8
        Edit = 9
        View = 10
        Delete = 11

    End Enum
#End Region

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

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

                RowsPerPageMS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                ' SetFocus(ddlGroupId)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "UserAdminModule\PrivilegeRightsforGroupSearch.aspx", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)


                End If
                Session("sDtDynamicuser") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamicuser") = dtDynamic
                Session.Add("grpstrsortExpression", "groupid")
                Session.Add("grpstrsortdirection", SortDirection.Ascending)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupId, "groupid", "groupname", "select groupid, groupname from groupmaster where active=1 order by groupid", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAppId, "appid", "appname", "select appid, appname from appmaster where appstatus=1 order by appid", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "groupname", "groupid", "select groupid, groupname from groupmaster where active=1 order by groupname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAppName, "appname", "appid", "select appid, appname from appmaster where appstatus=1 order by appname", True)

                FillGrid("groupid")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    '    ddlAppId.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '    ddlGroupId.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '    ddlAppName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '    ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PrivilegeRightsforGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        'btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "PrivRightsWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicuser")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamicuser") = dtt
            End If
        End If
        Return True
    End Function
    Private Function BuildConditionNew(ByVal strGroupValue As String, ByVal strappname As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strGroupValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(groupmaster.groupname) IN (" & Trim(strGroupValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(groupmaster.groupname) IN (" & Trim(strGroupValue.Trim.ToUpper) & ")"
            End If
        End If
        If strappname.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(appmaster.appname) IN (" & Trim(strappname.Trim.ToUpper) & ")"
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
                        strWhereCond1 = strWhereCond1 & " OR  upper(groupmaster.groupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(appmaster.appname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(groupmaster.groupid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),group_privilege.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),group_privilege.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),group_privilege.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),group_privilege.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If


        BuildConditionNew = strWhereCond
    End Function
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

                Case "GROUPNAME"
                    lsProcessgrp = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("GROUPNAME", lsProcessgrp, "G")

                Case "APPLICATIONNAME"
                    lsProcessApp = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("APPLICATIONNAME", lsProcessApp, "A")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamicuser")
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
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamicuser")
        Dim strGroupValue As String = ""
        Dim strappValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "GROUPNAME" Then
                        If strGroupValue <> "" Then
                            strGroupValue = strGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "APPLICATIONNAME" Then
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
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
          
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("grpstrsortExpression")
            Dim strsortorder As String = "ASC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"
            strSqlQry = " select group_privilege.*,groupmaster.groupname,appmaster.appname from group_privilege, appmaster, groupmaster " & _
                        " where group_privilege.groupid = groupmaster.groupid And group_privilege.appid = appmaster.appid"

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
#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""

        'If ddlAppId.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " group_privilege.appid = " & CType(Val(ddlAppId.Items(ddlAppId.SelectedIndex).Text.Trim), Long)
        '    Else
        '        strWhereCond = strWhereCond & " AND group_privilege.appid = " & CType(Val(ddlAppId.Items(ddlAppId.SelectedIndex).Text.Trim), Long)
        '    End If
        'End If

        'If ddlGroupId.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " group_privilege.groupid = " & CType(Val(ddlGroupId.Items(ddlGroupId.SelectedIndex).Text.Trim), Long)
        '    Else
        '        strWhereCond = strWhereCond & " AND group_privilege.groupid = " & CType(Trim(ddlGroupId.Items(ddlGroupId.SelectedIndex).Text.Trim), Long)
        '    End If
        'End If

        BuildCondition = strWhereCond
    End Function
#End Region
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicuser")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicuser") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
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
            strSqlQry = " select group_privilege.*,groupmaster.groupname,appmaster.appname from group_privilege, appmaster, groupmaster " & _
                        " where group_privilege.groupid = groupmaster.groupid And group_privilege.appid = appmaster.appid"

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
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
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsforGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("PrivilegeRightsforGroup.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('PrivilegeRightsforGroup.aspx?State=New','PrivRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('PrivilegeRightsforGroup.aspx?State=New','PrivRights');"

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        ' FillGrid("appid")

        FillGridNew()
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim strAppId As String
            Dim strGrpId As String
            '   strGrpId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).Cells(0).Text.ToString
            strAppId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).Cells(3).Text.ToString

            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")


            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('PrivilegeRightsforGroup.aspx?State=Edit&RefAppCode=" + CType(strAppId.Trim, String) + "&RefGrpCode=" + CType(lblId.Text.Trim, String) + "','PrivRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('PrivilegeRightsforGroup.aspx?State=Edit&RefAppCode=" + CType(strAppId.Trim, String) + "&RefGrpCode=" + CType(lblId.Text.Trim, String) + "','PrivRights');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefAppCode", CType(strAppId, String))
                'Session.Add("RefGrpCode", CType(strGrpId, String))
                'Response.Redirect("PrivilegeRightsforGroup.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('PrivilegeRightsforGroup.aspx?State=View&RefAppCode=" + CType(strAppId.Trim, String) + "&RefGrpCode=" + CType(lblId.Text.Trim, String) + "','PrivRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('PrivilegeRightsforGroup.aspx?State=View&RefAppCode=" + CType(strAppId.Trim, String) + "&RefGrpCode=" + CType(lblId.Text.Trim, String) + "','PrivRights');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('PrivilegeRightsforGroup.aspx?State=Delete&RefAppCode=" + CType(strAppId.Trim, String) + "&RefGrpCode=" + CType(lblId.Text.Trim, String) + "','PrivRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('PrivilegeRightsforGroup.aspx?State=Delete&RefAppCode=" + CType(strAppId.Trim, String) + "&RefGrpCode=" + CType(lblId.Text.Trim, String) + "','PrivRights');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsforGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid("appid")
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'ddlAppId.Value = "[Select]"
        'ddlAppName.Value = "[Select]"
        'ddlGroupId.Value = "[Select]"
        'ddlGroupName.Value = "[Select]"
        FillGrid("appid")
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblgroupname As Label = e.Row.FindControl("lblgroupname")
            Dim lblappname As Label = e.Row.FindControl("lblappname")
            Dim lsgroupName As String = ""

            Dim lsapplicationName As String = ""

            Dim lstextvalue As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicuser")
            If Session("sDtDynamicuser") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsgroupName = ""


                        If "GROUPNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsgroupName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "APPLICATIONNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
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
                        If lstextvalue.Trim <> "" Then
                            lblappname.Text = Regex.Replace(lblappname.Text.Trim, lsapplicationName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                            lblgroupname.Text = Regex.Replace(lblgroupname.Text.Trim, lsgroupName.Trim(), _
                             Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                         RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If
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

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                strSqlQry = "SELECT  group_privilege.groupid as [Group ID],groupmaster.groupname as [Group Name], " & _
                            "group_privilege.appid as [App ID],appmaster.appname as [App Name], " & _
                            "(Convert(Varchar, Datepart(DD,group_privilege.adddate))+ '/'+ Convert(Varchar, Datepart(MM,group_privilege.adddate))+ '/'+ Convert(Varchar, Datepart(YY,group_privilege.adddate)) + ' ' + Convert(Varchar, Datepart(hh,group_privilege.adddate))+ ':' + Convert(Varchar, Datepart(m,group_privilege.adddate))+ ':'+ Convert(Varchar, Datepart(ss,group_privilege.adddate))) as [Date Created], group_privilege.adduser as [User Created], " & _
                            "(Convert(Varchar, Datepart(DD,group_privilege.moddate))+ '/'+ Convert(Varchar, Datepart(MM,group_privilege.moddate))+ '/'+ Convert(Varchar, Datepart(YY,group_privilege.moddate)) + ' ' + Convert(Varchar, Datepart(hh,group_privilege.moddate))+ ':' + Convert(Varchar, Datepart(m,group_privilege.moddate))+ ':'+ Convert(Varchar, Datepart(ss,group_privilege.moddate))) as [Date Modified], group_privilege.moduser as [User Modified] " & _
                            " FROM group_privilege, appmaster, groupmaster where group_privilege.groupid = groupmaster.groupid And group_privilege.appid = appmaster.appid "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY  group_privilege.groupid,group_privilege.appid "
                Else
                    strSqlQry = strSqlQry & " ORDER BY group_privilege.groupid,group_privilege.appid"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "Privilege")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'Session.Add("CurrencyCode", txtgroupid.Text.Trim)
            'Session.Add("CurrencyName", txtmealname.Text.Trim)
            'Response.Redirect("rptCurrencies.aspx", False)

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""

            'Session.Add("Pageame", "Privilege Rights")
            'Session.Add("BackPageName", "PrivilegeRightsforGroupSearch.aspx")

            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=Privilege Rights&BackPageName=PrivilegeRightsforGroupSearch.aspx&GrpId=" & Trim(ddlGroupId.Items(ddlGroupId.SelectedIndex).Text) & "&AppId=" & Trim(ddlAppId.Items(ddlAppId.SelectedIndex).Text) & "','PrivRights','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Privilege Rights&BackPageName=PrivilegeRightsforGroupSearch.aspx','PrivRights');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PrivilegeRightsforGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PrivilegeRightsforGroupSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnClearDate_Click(sender As Object, e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamicuser")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If

        Session("sDtDynamicuser") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()
    End Sub

    Protected Sub RowsPerPageMS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageMS.SelectedIndexChanged
        FillGridNew()
    End Sub
End Class
