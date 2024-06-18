'------------================--------------=======================------------------================
'   Page Name       :    SupplierCategoriesSearch.aspx
'   Developer Name  :    Pramod Desai
'   Date            :    14 June 2008
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Partial Class SupplierCategoriesSearch
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
        CategoryCodeTCol = 0
        CategoryCode = 1
        CategoryName = 2
        SProviderType = 3
        SProviderName = 4
        RankOrder = 5
        Active = 6
        DateCreated = 7
        UserCreated = 8
        DateModified = 9
        UserModified = 10
        Edit = 11
        View = 12
        Delete = 13

    End Enum
#End Region

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
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   'changed by mohamed on 27/08/2018
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblCategoryName As Label = e.Row.FindControl("lblCategoryName")

            Dim lsCategoryName As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsCategoryName = ""

                        If "CATEGORYNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCategoryName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCategoryName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsCategoryName.Trim <> "" Then
                            lblCategoryName.Text = Regex.Replace(lblCategoryName.Text.Trim, lsCategoryName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                       
                    Next
                End If
            End If



        End If




    End Sub



#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Page.Header.Title = "General"
            Try
                'Dim strappid As String = ""
                Dim strappname As String = ""

                Dim strappid As String = ""
                Dim AppId As String = CType(Request.QueryString("appid"), String)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)


                If AppId Is Nothing = False Then
                    strappid = AppId
                    ViewState("appid") = strappid
                End If
                If AppName Is Nothing = False Then

                    If AppId = "4" Then

                        strappname = AppName.Value
                    ElseIf AppId = "14" Then

                        strappname = AppName.Value
                    Else
                        strappname = AppName.Value
                    End If
                End If
                ViewState("appid") = strappid
                RowsPerPageSCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                SetFocus(txtSupplierCategoriesCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\SupplierCategoriesSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If
                '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryName, "ctryname", "select ctryname from ctrymast where active=1 order by ctryname", True)

                Session.Add("strsortExpression", "sptypecode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '' Create a Dynamic datatable ---- Start
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                '--------end
                FillDDL()
                fillorderby()
                ' FillGrid("catmast.catname")
                FillGridNew()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlSupplierType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSupplierTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SupcatsWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
        Page.Title = "SupplierCategories Search"
    End Sub
#End Region

#Region " Private Sub FillDDL()"
    Private Sub FillDDL()
        strSqlQry = ""
        'strSqlQry = "SELECT sptypecode FROM sptypemast WHERE active=1 ORDER BY sptypecode"
        'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSupplierType, "sptypecode", strSqlQry, True)

        'strSqlQry = ""
        'strSqlQry = "select sptypename from sptypemast where active=1 order by sptypename"
        'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSupplierName, "sptypename", strSqlQry, True)

        'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSupplierType, "sptypecode", "select sptypecode from sptypemast where active=1 order by sptypecode", True)
        strSqlQry = "SELECT sptypecode,sptypename FROM sptypemast WHERE active=1 ORDER BY sptypecode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierType, "sptypecode", "sptypename", strSqlQry, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierTypeName, "sptypename", "sptypecode", strSqlQry, True)
    End Sub
#End Region

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtSupplierCategoriesCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(catmast.catcode) LIKE '" & Trim(txtSupplierCategoriesCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(catmast.catcode) LIKE '" & Trim(txtSupplierCategoriesCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtSupplierCategoriesName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(catmast.catname) LIKE '" & Trim(txtSupplierCategoriesName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(catmast.catname) LIKE '" & Trim(txtSupplierCategoriesName.Text.Trim.ToUpper) & "%'"
            End If
        End If
        'If ddlSupplierType.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(sptypemast.sptypecode) = '" & Trim(ddlSupplierType.SelectedValue.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(sptypemast.sptypecode) = '" & Trim(ddlSupplierType.SelectedValue.Trim.ToUpper) & "'"
        '    End If
        'End If

        'If ddlSupplierName.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(sptypemast.sptypename) = '" & Trim(ddlSupplierName.SelectedValue.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(sptypemast.sptypename) = '" & Trim(ddlSupplierName.SelectedValue.Trim.ToUpper) & "'"
        '    End If
        'End If
        If ddlSupplierType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(sptypemast.sptypecode) = '" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(sptypemast.sptypecode) = '" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If

        If ddlSupplierTypeName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(sptypemast.sptypename) = '" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(sptypemast.sptypename) = '" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If

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



            If CType(ViewState("appid"), String) = "1" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where catmast.sptypecode in ( select  option_selected  from reservation_parameters where param_id in(458,460))"
            ElseIf CType(ViewState("appid"), String) = "11" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where  catmast.sptypecode = (select  option_selected  from reservation_parameters where param_id in(1033))"
            ElseIf CType(ViewState("appid"), String) = "10" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where  catmast.sptypecode = (select  option_selected  from reservation_parameters where param_id in(564))"
            ElseIf CType(ViewState("appid"), String) = "13" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where  catmast.sptypecode = (select  option_selected  from reservation_parameters where param_id in(1032))"
            ElseIf CType(ViewState("appid"), String) = "4" Or CType(ViewState("appid"), String) = "14" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode "

            End If
            
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
            objUtils.WritErrorLog("SupplierCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("SupplierCategories.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('SupplierCategories.aspx?State=New','Supcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('SupplierCategories.aspx?State=New','Supcats');"

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("catcode")
        ' FillGrid("catmast.catname")
        'Select Case ddlOrderBy.SelectedIndex
        '   Case 0
        'FillGrid("catmast.catname")
        '     Case 1
        'FillGrid("catmast.catcode")
        '   Case 2
        ' FillGrid("sptypemast.sptypename")
        '    Case 3
        '       FillGrid("sptypemast.sptypecode")
        FillGridNew()
        'End Select

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcatcode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SupplierCategories.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('SupplierCategories.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('SupplierCategories.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Supcats');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SupplierCategories.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('SupplierCategories.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Supcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('SupplierCategories.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Supcats');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SupplierCategories.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('SupplierCategories.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Supcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('SupplierCategories.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Supcats');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("catcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("catmast.catname")
            Case 1
                FillGrid("catmast.catcode")
            Case 2
                FillGrid("sptypemast.sptypename")
            Case 3
                FillGrid("sptypemast.sptypecode")

        End Select
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtSupplierCategoriesCode.Text = ""
        txtSupplierCategoriesName.Text = ""
        'ddlSupplierType.SelectedValue = "[Select]"
        'ddlSupplierName.SelectedValue = "[Select]"
        ddlSupplierType.Value = "[Select]"
        ddlSupplierTypeName.Value = "[Select]"
        Me.ddlOrderBy.SelectedIndex = 0
        FillGrid("catmast.catname")
        'FillGrid("catcode")
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region " Public Sub SortGridColoumn_click()"
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
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                'Response.ContentType = "application/vnd.ms-excel"
                'Response.Charset = ""
                'Me.EnableViewState = False

                'Dim tw As New System.IO.StringWriter()
                'Dim hw As New System.Web.UI.HtmlTextWriter(tw)
                'Dim frm As HtmlForm = New HtmlForm()
                'Me.Controls.Add(frm)
                'frm.Controls.Add(gv_SearchResult)
                'frm.RenderControl(hw)
                'Response.Write(tw.ToString())
                'Response.End()
                'Response.Clear()

                If CType(ViewState("appid"), String) = "1" Then
                    strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where catmast.sptypecode in ( select  option_selected  from reservation_parameters where param_id in(458,460))"
                ElseIf CType(ViewState("appid"), String) = "11" Then
                    strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where  catmast.sptypecode = (select  option_selected  from reservation_parameters where param_id in(1033))"
                ElseIf CType(ViewState("appid"), String) = "4" Or CType(ViewState("appid"), String) = "14" Then
                    strSqlQry = "SELECT c.catcode as [Category Code],c.catname as [Category Name],c.sptypecode as [SpType Code],sp.sptypename as [SpType Name],c.accrualacctcode,c.controlacctcode,c.rankorder,[IsActive]=case when c.active=1 then 'Active' when c.active=0 then 'InActive' end,c.adddate as [Date Created],c.adduser as[User Created],c.moddate as[Date Modified],c.moduser as [User Modified] FROM catmast  c INNER JOIN sptypemast sp ON c.sptypecode=sp.sptypecode"

                End If

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY catcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY catcode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "catmast")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
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
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=Supplier Category&BackPageName=SupplierCategoriesSearch.aspx&SupcatCode=" & txtSupplierCategoriesCode.Text.Trim & "&SupcatName=" & txtSupplierCategoriesName.Text.Trim & "&SuptypeCode=" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text) & "&SuptypeName=" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text) & "','RepSupCat','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Supplier Category&BackPageName=SupplierCategoriesSearch.aspx&SupcatCode=" & txtSupplierCategoriesCode.Text.Trim & "&SupcatName=" & txtSupplierCategoriesName.Text.Trim & "&SuptypeCode=" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text) & "&SuptypeName=" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text) & "&appid=" & CType(ViewState("appid"), String) & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Supplier Category")
            'Session.Add("BackPageName", "SupplierCategoriesSearch.aspx")

            'If txtSupplierCategoriesCode.Text.Trim <> "" Then
            '    strReportTitle = "Supplier Category Code : " & txtSupplierCategoriesCode.Text.Trim
            '    strSelectionFormula = "{catmast.catcode} LIKE '" & txtSupplierCategoriesCode.Text.Trim & "*'"
            'End If
            'If txtSupplierCategoriesName.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Supplier Category Name : " & txtSupplierCategoriesName.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {catmast.catname} LIKE '" & txtSupplierCategoriesName.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Supplier Category Name : " & txtSupplierCategoriesName.Text.Trim
            '        strSelectionFormula = "{catmast.catname} LIKE '" & txtSupplierCategoriesName.Text.Trim & "*'"
            '    End If
            'End If

            'If ddlSupplierType.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Supplier Type Code : " & ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {catmast.sptypecode} = '" & ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Supplier Type Code: " & ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{catmast.sptypecode} = '" & ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'If ddlSupplierTypeName.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Supplier Type Name : " & ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Supplier Type Name: " & ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{sptypemast.sptypename} = '" & ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If


            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlSupply.Visible = False
        lblSupTypeCode.Visible = False
        ddlSupplierType.Visible = False
        lblSupTypeName.Visible = False
        ddlSupplierTypeName.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlSupply.Visible = True
        lblSupTypeCode.Visible = True
        ddlSupplierType.Visible = True
        lblSupTypeName.Visible = True
        ddlSupplierTypeName.Visible = True
    End Sub


    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Category Name")
        ddlOrderBy.Items.Add("Category Code")
        ddlOrderBy.Items.Add("SupplierType Name")
        ddlOrderBy.Items.Add("SupplierType Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("catmast.catname")
            Case 1
                FillGrid("catmast.catcode")
            Case 2
                FillGrid("sptypemast.sptypename")
            Case 3
                FillGrid("sptypemast.sptypecode")

        End Select

    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        'Dim lsSearchTxt As String = ""
        'lsSearchTxt = txtvsprocesssplit.Text '.Replace(": """, ":""")
        'Dim lsProcessCity As String = ""
        'Dim lsProcessCountry As String = ""
        ''txtvsprocessCity.Text = ""
        ''txtvsprocessCountry.Text = ""
        'Dim lsMainArr As String()
        ''lsMainArr = lsSearchTxt.Split("|~,")
        'lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        'For i = 0 To lsMainArr.GetUpperBound(0)
        '    Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
        '        Case "CITY"
        '            'txtvsprocessCity.Text = Mid(lsMainArr(i).Split(":")(1), 2, lsMainArr(i).Split(":")(1).ToString.Length - 2)
        '            'txtvsprocessCity.Text = lsMainArr(i).Split(":")(1)
        '            lsProcessCity = lsMainArr(i).Split(":")(1)
        '        Case "COUNTRY"
        '            'txtvsprocessCountry.Text = Mid(lsMainArr(i).Split(":")(1), 2, lsMainArr(i).Split(":")(1).ToString.Length - 2)
        '            'txtvsprocessCountry.Text = lsMainArr(i).Split(":")(1)
        '            lsProcessCountry = lsMainArr(i).Split(":")(1)
        '    End Select
        'Next
        'txtcitycode.Text = ""
        'txtcityname.Text = ""
        ''ddlsccode.SelectedIndex = -1
        ''ddlscname.SelectedIndex = -1
        'objUtils.sbSetSelectedValueForHTMLSelect("[Select]", ddlsccode)
        'objUtils.sbSetSelectedValueForHTMLSelect("[Select]", ddlscname)
        'If lsProcessCity.Trim <> "" Then
        '    txtcityname.Text = lsProcessCity.Trim
        'End If

        'If lsProcessCountry.Trim <> "" Then
        '    objUtils.sbSetSelectedValueForHTMLSelect(lsProcessCountry, ddlscname)
        'End If

        'Select Case ddlOrderBy.SelectedIndex
        '    Case 0
        '        FillGrid("cityname")
        '    Case 1
        '        FillGrid("citycode")
        '    Case 2
        '        FillGrid("ctrycode")


        'End Select
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCategory As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "CATEGORYNAME"
                    lsProcessCategory = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CATEGORYNAME", lsProcessCategory, "CATEGORYNAME")

                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupplierCategoriesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function getRowpage() As String
        Dim rowpagescs As String
        If RowsPerPageSCS.SelectedValue = "20" Then
            rowpagescs = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagescs = RowsPerPageSCS.SelectedValue

        End If
        Return rowpagescs
    End Function
    Protected Sub RowsPerPageSCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageSCS.SelectedIndexChanged
        FillGridNew()
    End Sub
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()

    End Sub
    Private Function BuildConditionNew(ByVal strCategoryValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCategoryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(catmast.catname) IN (" & Trim(strCategoryValue.Trim.ToUpper) & ")"
                'Else
                '    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "(upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
                        'Else
                        'strWhereCond1 = strWhereCond1 & " OR  (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),catmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),catmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),catmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),catmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function
    Private Sub FillGridNew()
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strCategoryValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "CATEGORYNAME" Then
                        If strCategoryValue <> "" Then
                            strCategoryValue = strCategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Dim pagevaluescs = RowsPerPageSCS.SelectedValue
            strBindCondition = BuildConditionNew(strCategoryValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"
            If CType(ViewState("appid"), String) = "1" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where catmast.sptypecode in ( select  option_selected  from reservation_parameters where param_id in(458,460))"
            ElseIf CType(ViewState("appid"), String) = "11" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where  catmast.sptypecode = (select  option_selected  from reservation_parameters where param_id in(1033))"
            ElseIf CType(ViewState("appid"), String) = "10" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where  catmast.sptypecode = (select  option_selected  from reservation_parameters where param_id in(564))"
            ElseIf CType(ViewState("appid"), String) = "13" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode where  catmast.sptypecode = (select  option_selected  from reservation_parameters where param_id in(1032))"

            ElseIf CType(ViewState("appid"), String) = "4" Or CType(ViewState("appid"), String) = "14" Then
                strSqlQry = "SELECT catmast.*,sptypemast.sptypename,[IsActive]=case when catmast.active=1 then 'Active' when catmast.active=0 then 'InActive' end FROM catmast INNER JOIN sptypemast ON catmast.sptypecode=sptypemast.sptypecode "

            End If
            hdnappid.Value = CType(ViewState("appid"), String)
            ' MsgBox(hdnappid.Value)
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
                gv_SearchResult.PageSize = pagevaluescs
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
End Class
