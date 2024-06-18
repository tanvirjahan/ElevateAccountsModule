
'------------================--------------=======================------------------================
'   Module Name    :    UserMasterSearch.aspx
'   Developer Name :    sandeep Indulkar
'   Date           :    
'   
'
'------------------------------------------

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class UserMasterSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
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
        UserCodeTCol = 0
        UserCode = 1
        UserName = 2
        GroupID = 3
        UserDesignation = 4
        DepartmentCode = 5
        Email = 6
        Phone = 7
        Active = 8
        UserCreated = 9
        DateCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 13
        View = 14
        Delete = 15
    End Enum
#End Region

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try

            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicusermast")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicusermast") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessUser As String = ""
        Dim lsProcessDept As String = ""
        Dim lsProcessUserGrp As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim

                Case "USER"
                    lsProcessUser = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("USER", lsProcessUser, "U")
                Case "DEPARTMENT"
                    lsProcessDept = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("DEPARTMENT", lsProcessDept, "D")
                Case "USERGROUP"
                    lsProcessUserGrp = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("USERGROUP", lsProcessUserGrp, "UG")

                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamicusermast")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamicusermast")
        Dim strUserValue As String = ""
        Dim strDeptValue As String = ""
        Dim strUserGrpValue As String = ""

        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "USER" Then
                        If strUserValue <> "" Then
                            strUserValue = strUserValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strUserValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "DEPARTMENT" Then
                        If strDeptValue <> "" Then
                            strDeptValue = strDeptValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDeptValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "USERGROUP" Then
                        If strUserGrpValue <> "" Then
                            strUserGrpValue = strUserGrpValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strUserGrpValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            strBindCondition = BuildConditionNew(strUserValue, strDeptValue, strUserGrpValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("userstrsortExpression")
            Dim strsortorder As String = "ASC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"
            strSqlQry = "SELECT UserMaster.UserCode,UserMaster.UserName,groupmaster.groupname,UserMaster.userdesign,UserMaster.deptcode," & _
                        " UserMaster.usemail, UserMaster.ustel,[Active]=case UserMaster.active when 1 then 'Active' else 'In Active' end , " & _
                        " UserMaster.AddDate, UserMaster.AddUser, UserMaster.ModDate, UserMaster.ModUser, DeptMaster.DeptName,groupmaster.groupname" & _
                        " FROM UserMaster INNER JOIN  DeptMaster ON UserMaster.deptcode = DeptMaster.Deptcode INNER JOIN" & _
                        " groupmaster ON UserMaster.groupid = groupmaster.groupid "

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("UserMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strUserValue As String, ByVal strDeptValue As String, ByVal strUserGrpValue As String, ByVal strTextValue As String) As String

        strWhereCond = ""
        If strUserValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(usermaster. username) IN (" & Trim(strUserValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(usermaster.username) IN (" & Trim(strUserValue.Trim.ToUpper) & ")"
            End If
        End If
        If strDeptValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(deptmaster. deptname) IN (" & Trim(strDeptValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(deptmaster.deptname) IN (" & Trim(strDeptValue.Trim.ToUpper) & ")"
            End If
        End If
        If strUserGrpValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(groupmaster.groupname) IN (" & Trim(strUserGrpValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(groupmaster.groupname) IN (" & Trim(strUserGrpValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "upper(usermaster.username) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(deptmaster.deptname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(Groupmaster.groupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(usermaster.username) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(deptmaster.deptname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(Groupmaster.groupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   ) "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),usermaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),usermaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),usermaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),usermaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If


        BuildConditionNew = strWhereCond
    End Function
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicusermast")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamicusermast") = dtt
            End If
        End If
        Return True
    End Function
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack = False Then


            Try
                ''  SetFocus(TxtCode)



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


                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "UserAdminModule\UserMasterSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                Session("sDtDynamicusermast") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamicusermast") = dtDynamic
                Session.Add("userstrsortExpression", "usercode")
                Session.Add("userstrsortdirection", SortDirection.Ascending)

                ''FillGrid("usercode")
                FillGridNew()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("UserMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        ''BtnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "UserMastWindowPostBack") Then
            '  btnSearch_Click(sender, e)
            FillGridNew()
        End If
    End Sub


#End Region

#Region "  Private Function BuildCondition() As String"

    Private Function BuildCondition() As String
        strWhereCond = ""
        ''If TxtCode.Value <> "" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = " upper(usermaster.usercode) LIKE '" & Trim(TxtCode.Value.Trim.ToUpper) & "%'"
        ''    Else
        ''        strWhereCond = strWhereCond & " AND upper(usermaster.usercode) LIKE '" & Trim(TxtCode.Value.Trim.ToUpper) & "%'"
        ''    End If
        ''End If
        ''If TxtName.Value <> "" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = " upper(usermaster.username) LIKE '" & Trim(TxtName.Value.Trim.ToUpper) & "%'"
        ''    Else
        ''        strWhereCond = strWhereCond & " AND upper(usermaster.username) LIKE '" & Trim(TxtName.Value.Trim.ToUpper) & "%'"
        ''    End If
        ''End If

        ''If ddlDepartmentCode.Value <> "[Select]" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = " upper(DeptMaster.Deptcode) = '" & Trim(CType(ddlDepartmentCode.Items(ddlDepartmentCode.SelectedIndex).Text, String)) & "'"
        ''    Else
        ''        strWhereCond = strWhereCond & " AND upper(DeptMaster.Deptcode) = '" & Trim(CType(ddlDepartmentCode.Items(ddlDepartmentCode.SelectedIndex).Text, String)) & "'"
        ''    End If
        ''End If

        ''If ddlDepartmentName.Value <> "[Select]" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = " upper(DeptMaster.DeptName) = '" & Trim(CType(ddlDepartmentName.Items(ddlDepartmentName.SelectedIndex).Text, String)) & "'"
        ''    Else
        ''        strWhereCond = strWhereCond & " AND upper(DeptMaster.DeptName) = '" & Trim(CType(ddlDepartmentName.Items(ddlDepartmentName.SelectedIndex).Text, String)) & "'"
        ''    End If
        ''End If

        ''If ddlgroupcode.Value <> "[Select]" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = " upper(groupmaster.groupid) = '" & Trim(CType(ddlgroupcode.Items(ddlgroupcode.SelectedIndex).Text, String)) & "'"
        ''    Else
        ''        strWhereCond = strWhereCond & " AND upper(groupmaster.groupid) = '" & Trim(CType(ddlgroupcode.Items(ddlgroupcode.SelectedIndex).Text, String)) & "'"
        ''    End If
        ''End If

        ''If ddlgroupname.Value <> "[Select]" Then
        ''    If Trim(strWhereCond) = "" Then
        ''        strWhereCond = " upper(groupmaster.groupname) = '" & Trim(CType(ddlgroupname.Items(ddlgroupname.SelectedIndex).Text, String)) & "'"
        ''    Else
        ''        strWhereCond = strWhereCond & " AND upper(groupmaster.groupname) = '" & Trim(CType(ddlgroupname.Items(ddlgroupname.SelectedIndex).Text, String)) & "'"
        ''    End If
        ''End If

        BuildCondition = strWhereCond
    End Function
#End Region

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

            strSqlQry = "SELECT UserMaster.UserCode,UserMaster.UserName,UserMaster.groupid,UserMaster.userdesign,UserMaster.deptcode," & _
                        " UserMaster.usemail, UserMaster.ustel,[Active]=case UserMaster.active when 1 then 'Active' else 'In Active' end , " & _
                        " UserMaster.AddDate, UserMaster.AddUser, UserMaster.ModDate, UserMaster.ModUser, DeptMaster.DeptName,groupmaster.groupname" & _
                        " FROM UserMaster INNER JOIN  DeptMaster ON UserMaster.deptcode = DeptMaster.Deptcode INNER JOIN" & _
                        " groupmaster ON UserMaster.groupid = groupmaster.groupid "
            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("UserMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("UserMaster.aspx", False)
        Dim strpop As String = ""
        ' strpop = "window.open('UserMaster.aspx?State=New','UserMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('UserMaster.aspx?State=New','UserMast');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
        'FillGrid("usermaster.usercode")
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcode")

            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('UserMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','UserMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('UserMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','UserMast');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                'strpop = "window.open('UserMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','UserMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('UserMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','UserMast');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('UserMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','UserMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('UserMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','UserMast');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UserMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

    '#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"

    '    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '        FillGrid("usermaster.usercode")
    '    End Sub
    '#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lbluserdesignname As Label = e.Row.FindControl("lbluserdesignname")
            Dim lblusername As Label = e.Row.FindControl("lblusername")
            Dim lbldeptname As Label = e.Row.FindControl("lbldeptname")
            Dim lblusergroup As Label = e.Row.FindControl("lblusergroup")
            Dim lsusergroupName As String = ""

            Dim lsusername As String = ""

            Dim lsdeptname As String = ""
            Dim lstextvalue As String = ""

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicusermast")
            If Session("sDtDynamicusermast") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsusername = ""


                        If "USER" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsusername = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "DEPARTMENT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsdeptname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "USERGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsusergroupName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lstextvalue = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsusername.Trim <> "" Then
                            lblusername.Text = Regex.Replace(lblusername.Text.Trim, lsusername.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsdeptname.Trim <> "" Then
                            lbldeptname.Text = Regex.Replace(lbldeptname.Text.Trim, lsdeptname.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsusergroupName.Trim <> "" Then
                            lblusergroup.Text = Regex.Replace(lblusergroup.Text.Trim, lsusergroupName.Trim(), _
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
#End Region

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

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = "SELECT UserMaster.UserCode as [User Code],UserMaster.UserName as [User Name],UserMaster.groupid as [Group ID],UserMaster.userdesign as [User Designation],UserMaster.deptcode as [Department Code]," & _
                            " UserMaster.usemail as [Email ID], UserMaster.ustel as [Phone],[Active]=case UserMaster.active when 1 then 'Active' else 'In Active' end , " & _
                            " (Convert(Varchar, Datepart(DD,UserMaster.adddate))+ '/'+ Convert(Varchar, Datepart(MM,UserMaster.adddate))+ '/'+ Convert(Varchar, Datepart(YY,UserMaster.adddate)) + ' ' + Convert(Varchar, Datepart(hh,UserMaster.adddate))+ ':' + Convert(Varchar, Datepart(m,UserMaster.adddate))+ ':'+ Convert(Varchar, Datepart(ss,UserMaster.adddate))) as [Date Created]," & _
                            " UserMaster.AddUser as [User Created], " & _
                            " (Convert(Varchar, Datepart(DD,UserMaster.moddate))+ '/'+ Convert(Varchar, Datepart(MM,UserMaster.moddate))+ '/'+ Convert(Varchar, Datepart(YY,UserMaster.moddate)) + ' ' + Convert(Varchar, Datepart(hh,UserMaster.moddate))+ ':' + Convert(Varchar, Datepart(m,UserMaster.moddate))+ ':'+ Convert(Varchar, Datepart(ss,UserMaster.moddate))) as [Date Modified]," & _
                            " UserMaster.ModUser as [User Modified] " & _
                            " FROM UserMaster INNER JOIN  DeptMaster ON UserMaster.deptcode = DeptMaster.Deptcode INNER JOIN " & _
                            " groupmaster ON UserMaster.groupid = groupmaster.groupid"






                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY usercode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY usercode"
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(DS, "usermast")

                objUtils.ExportToExcel(DS, Response)
                clsDBConnect.dbConnectionClose(SqlConn)
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        '------ Azia --------------
        Try

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session.Add("Pageame", "User Master")
            'Session.Add("BackPageName", "UserMasterSearch.aspx")

            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=User Master&BackPageName=UserMasterSearch.aspx&UserCode=" & TxtCode.Value.Trim & "&UserName=" & TxtName.Value.Trim & "&DeptCode=" & Trim(ddlDepartmentCode.Items(ddlDepartmentCode.SelectedIndex).Text) & "&GrpCode=" & Trim(ddlgroupcode.Items(ddlgroupcode.SelectedIndex).Text) & "','UserMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=User Master&BackPageName=UserMasterSearch.aspx','UserMast');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UserMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        '-------------End ------------------------
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click"
    'Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
    '    TxtCode.Value = ""
    '    TxtName.Value = ""
    '    ddlDepartmentCode.Value = "[Select]"
    '    ddlDepartmentName.Value = "[Select]"
    '    ddlgroupcode.Value = "[Select]"
    '    ddlgroupname.Value = "[Select]"
    '    FillGrid("usermaster.usercode")
    'End Sub
#End Region

#Region " Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    'Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    lbldeptcode.Visible = True
    '    lbldeptname.Visible = True
    '    lblGroupcode.Visible = True
    '    lblGroupname.Visible = True
    '    ddlDepartmentCode.Visible = True
    '    ddlDepartmentName.Visible = True
    '    ddlgroupcode.Visible = True
    '    ddlgroupname.Visible = True
    'End Sub
#End Region
    
#Region "Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    'Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    lbldeptcode.Visible = False
    '    lbldeptname.Visible = False
    '    lblGroupcode.Visible = False
    '    lblGroupname.Visible = False
    '    ddlDepartmentCode.Visible = False
    '    ddlDepartmentName.Visible = False
    '    ddlgroupcode.Visible = False
    '    ddlgroupname.Visible = False
    'End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UserMasterSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub RowsPerPageMS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageMS.SelectedIndexChanged
        FillGridNew()
    End Sub

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamicusermast")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamicusermast") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()
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
End Class
