
'------------================--------------=======================------------------================
'   Page Name       :   RoomCategoriesSearch.aspx
'   Developer Name  :    Pramod Desai
'   Date            :    16 June 2008
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.ArrayList
Imports System.Array
Imports System.Collections.Generic

Partial Class RoomCategoriesSearch
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim roomcategorytype As String

#End Region



#Region "Enum GridCol"
    Enum GridCol

        RoomCategoryCode = 1
        RoomCategoryName = 2
        PrintName = 3
        accom_extra = 4
        rankorder = 4
        mealyn = 5
        allotreqd = 6
        Active = 7
        webname = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 14
        View = 15
        Delete = 16
    End Enum
#End Region

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblAccomName As Label = e.Row.FindControl("lblAccomName")


            Dim lsSearchTextName As String = ""
            Dim lsSearchTextAccom As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextName = ""

                        If "CATEGORYNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CATEGORYTYPE" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextAccom = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextAccom = lsSearchTextName

                        End If

                        If lsSearchTextName.Trim <> "" Then
                            lblCatName.Text = Regex.Replace(lblCatName.Text.Trim, lsSearchTextName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextAccom.Trim <> "" Then
                            lblAccomName.Text = Regex.Replace(lblAccomName.Text.Trim, lsSearchTextAccom.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If


                    Next
                End If
            End If



        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ViewState.Add("Type", Request.QueryString("type"))
        roomcategorytype = Request.QueryString("type")

        If Page.IsPostBack = False Then
            'Page.Header.Title = "General"
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
                RowsPerPageAS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                SetFocus(txtRoomCategoryCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\RoomCategoriesSearch.aspx?type=" & CType(ViewState("Type"), String) & "", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                If ViewState("Type") = "Acc" Then
                    lblheading.Text = "Accommodation Categories List"
                    '    lbl.Text = "Debit Note"
                    lblmealname.Visible = False
                    txtmealname.Visible = False
                ElseIf ViewState("Type") = "Supp" Then
                    lblheading.Text = "Supplement Categories List"
                    '    lblHeading.Text = "Credit Note"
                    lblmealname.Visible = True
                    txtmealname.Visible = True

                End If

                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlwebname, "webname", "select webname from rmcatmast where active=1 order by webname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSSPTypeCode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypename", True)
                Session.Add("strsortExpression", "rmcatmast.sptypecode")
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
                fillorderby()
                fillcategory()
                'FillGrid("rmcatname")
                FillGridNew()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RoomCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RmcatsWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
        If ViewState("Type") = "Acc" Then
            Page.Title = "Accommodation Category"
        End If
        If ViewState("Type") = "Supp" Then
            Page.Title = "Supplement Category"
        End If
    End Sub

    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtRoomCategoryCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(rmcatmast.rmcatcode) LIKE '" & Trim(txtRoomCategoryCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(rmcatmast.rmcatcode) LIKE '" & Trim(txtRoomCategoryCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtRoomCategoryName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(rmcatmast.rmcatname) LIKE '" & Trim(txtRoomCategoryName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(rmcatmast.rmcatname) LIKE '" & Trim(txtRoomCategoryName.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlcategorytype.SelectedValue <> "S" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(rmcatmast.accom_extra) = '" & ddlcategorytype.SelectedValue & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(rmcatmast.accom_extra) ='" & ddlcategorytype.SelectedValue & "'"
            End If
        End If

        If txtmealcode.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(rmcatmast.mealcode) = '" & txtmealcode.Text.Trim & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(rmcatmast.mealcode) ='" & txtmealcode.Text.Trim & "'"
            End If

        End If


        'If ddlwebname.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(rmcatmast.webname) LIKE '" & Trim(ddlwebname.Text.Trim.ToUpper) & "%'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(rmcatmast.webname) LIKE '" & Trim(ddlwebname.Text.Trim.ToUpper) & "%'"
        '    End If
        'End If
        'If ddlSSPTypeCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(sptypemast.sptypecode) = '" & Trim(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(sptypemast.sptypecode) = '" & Trim(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If

        'If ddlSSPTypeName.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(sptypemast.sptypename) = '" & Trim(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(sptypemast.sptypename) = '" & Trim(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If
        BuildCondition = strWhereCond
    End Function
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        gv_SearchResult.Visible = True
        lblMsg.Visible = False
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            If ViewState("Type") = "Acc" Then
                strSqlQry = "SELECT rmcatmast.rmcatcode,rmcatmast.rmcatname," _
                    & " case rmcatmast.accom_extra " _
                    & " when 'A' then 'Adult Accommodation' " _
                    & " when 'C' then 'Child Accommodation' " _
                    & " when 'M' then 'Adult Meal Supplements' " _
                    & " when 'L' then 'Child Meal Supplements' " _
                    & " when 'E' then 'Extra' end  as accom_extra," _
                    & " rmcatmast.prnname,rmcatmast.mealyn,rmcatmast.calcyn,rmcatmast.rankorder,rmcatmast.unitname, rmcatmast.allotreqd,rmcatmast.units,rmcatmast.webname,rmcatmast.adddate,rmcatmast.adduser,rmcatmast.moddate,rmcatmast.moduser, case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive'end as active, case when rmcatmast.autoconfirm=1 then 'Yes' when rmcatmast.autoconfirm=0 then 'No' end as autoconfirm FROM rmcatmast left join sptypemast ON rmcatmast.sptypecode=sptypemast.sptypecode where accom_extra in('A','C')"
            ElseIf ViewState("Type") = "Supp" Then
                strSqlQry = "SELECT rmcatmast.rmcatcode,rmcatmast.rmcatname," _
                    & " case rmcatmast.accom_extra " _
                    & " when 'A' then 'Adult Accommodation' " _
                    & " when 'C' then 'Child Accommodation' " _
                    & " when 'M' then 'Adult Meal Supplements' " _
                    & " when 'L' then 'Child Meal Supplements' " _
                    & " when 'E' then 'Extra' end  as accom_extra," _
                    & " rmcatmast.prnname,rmcatmast.mealyn,rmcatmast.calcyn,rmcatmast.rankorder,rmcatmast.unitname, rmcatmast.allotreqd,rmcatmast.units,rmcatmast.webname,rmcatmast.adddate,rmcatmast.adduser,rmcatmast.moddate,rmcatmast.moduser, case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive'end as active, case when rmcatmast.autoconfirm=1 then 'Yes' when rmcatmast.autoconfirm=0 then 'No' end as autoconfirm FROM rmcatmast left join sptypemast ON rmcatmast.sptypecode=sptypemast.sptypecode where accom_extra not in('A','C')"
            End If
            ''strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM rmcatmast"
            'strSqlQry = "SELECT rmcatmast.rmcatcode,rmcatmast.rmcatname,case rmcatmast.accom_extra when 'A' then 'Adult Accodmation'when 'C' then 'Child Accodmation' when 'E' then 'Extra' end  as accom_extra,rmcatmast.prnname,rmcatmast.mealyn,rmcatmast.calcyn,rmcatmast.rankorder,rmcatmast.unitname, rmcatmast.allotreqd,rmcatmast.units,rmcatmast.webname,rmcatmast.adddate,rmcatmast.adduser,rmcatmast.moddate,rmcatmast.moduser, case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive'end as active, case when rmcatmast.autoconfirm=1 then 'Yes' when rmcatmast.autoconfirm=0 then 'No' end as autoconfirm FROM rmcatmast left join sptypemast ON rmcatmast.sptypecode=sptypemast.sptypecode "
            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " and " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("RoomCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("RoomCategories.aspx", False)
        Dim strpop As String = ""
        ' strpop = "window.open('RoomCategories.aspx?State=New','Rmcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('RoomCategories.aspx?State=New&Type=" + ViewState("Type") + "','Rmcats');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("rmcatcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("rmcatmast.rmcatname")
            Case 1
                FillGrid("rmcatmast.rmcatcode")
                'Case 2
                '    FillGrid("sptypemast.sptypename")
                'Case 3
                '    FillGrid("sptypemast.sptypecode")

        End Select
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try

            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")

            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('RoomCategories.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Rmcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('RoomCategories.aspx?State=Edit&Type=" + ViewState("Type") + "&RefCode=" + Uri.EscapeDataString(CType(lblId.Text.Trim, String)) + "','Rmcats');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                'strpop = "window.open('RoomCategories.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Rmcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('RoomCategories.aspx?State=View&Type=" + ViewState("Type") + "&RefCode=" + Uri.EscapeDataString(CType(lblId.Text.Trim, String)) + "','Rmcats');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('RoomCategories.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Rmcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('RoomCategories.aspx?State=Delete&Type=" + ViewState("Type") + "&RefCode=" + Server.UrlEncode(CType(lblId.Text.Trim, String)) + "','Rmcats');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("rmcatcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("rmcatmast.rmcatname")
            Case 1
                FillGrid("rmcatmast.rmcatcode")
                'Case 2
                '    FillGrid("sptypemast.sptypename")
                'Case 3
                '    FillGrid("sptypemast.sptypecode")

        End Select
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtRoomCategoryCode.Text = ""
        txtRoomCategoryName.Text = ""
        txtmealname.Text = ""
        txtmealcode.Text = ""
        'ddlwebname.SelectedValue = "[Select]"
        'ddlSSPTypeCode.Value = "[Select]"
        'ddlSSPTypeName.Value = "[Select]"
        ddlcategorytype.SelectedValue = "S"
        ddlOrderBy.SelectedIndex = 0
        FillGrid("rmcatmast.rmcatname")
    End Sub

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
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

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                '    Response.ContentType = "application/vnd.ms-excel"
                '    Response.Charset = ""
                '    Me.EnableViewState = False

                '    Dim tw As New System.IO.StringWriter()
                '    Dim hw As New System.Web.UI.HtmlTextWriter(tw)
                '    Dim frm As HtmlForm = New HtmlForm()
                '    Me.Controls.Add(frm)
                '    frm.Controls.Add(gv_SearchResult)
                '    frm.RenderControl(hw)
                '    Response.Write(tw.ToString())
                '    Response.End()
                '    Response.Clear()etyvccv

                If ViewState("Type") = "Acc" Then
                    strSqlQry = "SELECT rmcatmast.rmcatcode AS [Room Category Code],rmcatmast.rmcatname AS [Room Category Name],rmcatmast.prnname as [Print Name]," _
                        & " case rmcatmast.accom_extra " _
                        & " when 'A' then 'Adult Accodmation' " _
                        & " when 'C' then 'Child Accodmation' " _
                        & " when 'M' then 'Adult Meal Supplements' " _
                        & " when 'L' then 'Child Meal Supplements' " _
                        & " when 'E' then 'Extra' end  as [Category Type]," _
                        & " rmcatmast.rankorder as [Display Order],rmcatmast.allotreqd as [Allotment Required],[Active]=case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,rmcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,rmcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,rmcatmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,rmcatmast.adddate))+ ':' + Convert(Varchar, Datepart(m,rmcatmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,rmcatmast.adddate))) as [Date Created],rmcatmast.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,rmcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,rmcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,rmcatmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,rmcatmast.moddate))+ ':' + Convert(Varchar, Datepart(m,rmcatmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,rmcatmast.moddate))) as [Date Modified] FROM rmcatmast left join  sptypemast on rmcatmast.sptypecode = sptypemast.sptypecode where allotreqd='Yes'"
                ElseIf ViewState("Type") = "Supp" Then
                    strSqlQry = "SELECT rmcatmast.rmcatcode AS [Room Category Code],rmcatmast.rmcatname AS [Room Category Name]," _
                        & " rmcatmast.prnname as [Print Name]," _
                        & " case rmcatmast.accom_extra " _
                        & " when 'A' then 'Adult Accodmation' " _
                        & " when 'C' then 'Child Accodmation' " _
                        & " when 'M' then 'Adult Meal Supplements' " _
                        & " when 'L' then 'Child Meal Supplements' " _
                        & " when 'E' then 'Extra' end  as [Category Type]," _
                        & " rmcatmast.rankorder as [Display Order],rmcatmast.mealyn as [Meal Plan],rmcatmast.allotreqd as [Allotment Required],[Active]=case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,rmcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,rmcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,rmcatmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,rmcatmast.adddate))+ ':' + Convert(Varchar, Datepart(m,rmcatmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,rmcatmast.adddate))) as [Date Created],rmcatmast.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,rmcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,rmcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,rmcatmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,rmcatmast.moddate))+ ':' + Convert(Varchar, Datepart(m,rmcatmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,rmcatmast.moddate))) as [Date Modified] FROM rmcatmast left join  sptypemast on rmcatmast.sptypecode = sptypemast.sptypecode where allotreqd='No'"
                End If


                ' strSqlQry = "SELECT rmcatmast.rmcatcode AS [Room Category Code],rmcatmast.rmcatname AS [Room Category Name],rmcatmast.prnname as [Print Name],rmcatmast.rankorder as [Display Order],rmcatmast.mealyn as [Meal Plan],rmcatmast.allotreqd as [Allotment Required],[Active]=case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,rmcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,rmcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,rmcatmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,rmcatmast.adddate))+ ':' + Convert(Varchar, Datepart(m,rmcatmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,rmcatmast.adddate))) as [Date Created],rmcatmast.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,rmcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,rmcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,rmcatmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,rmcatmast.moddate))+ ':' + Convert(Varchar, Datepart(m,rmcatmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,rmcatmast.moddate))) as [Date Modified] FROM rmcatmast left join  sptypemast on rmcatmast.sptypecode = sptypemast.sptypecode"

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " and " & BuildCondition() & " ORDER BY rmcatcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY rmcatcode" 'end
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                ' DA.Fill(DS, "othgrpmast")
                DA.Fill(DS, "rmcatmast")

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
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            ' strpop = "window.open('rptReportNew.aspx?Pageame=Room Category&BackPageName=RoomCategoriesSearch.aspx&RmcatCode=" & txtRoomCategoryCode.Text.Trim & "&RmcatName=" & txtRoomCategoryName.Text.Trim & "&SuptypeCode=" & Trim(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text) & "&SuptypeName=" & Trim(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text) & "','RepRoomCat','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Room Category&BackPageName=RoomCategoriesSearch.aspx&Typevalue=" & ViewState("Type") & "&RmcatCode=" & txtRoomCategoryCode.Text.Trim & "&RmcatName=" & txtRoomCategoryName.Text.Trim & "&CategoryType=" & ddlcategorytype.SelectedValue & "&CategoryTypeName=" & ddlcategorytype.SelectedItem.Text & "','RepRoomCat');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Room Category")
            'Session.Add("BackPageName", "RoomCategoriesSearch.aspx")

            'If txtRoomCategoryCode.Text.Trim <> "" Then
            '    strReportTitle = "Room Category Code : " & txtRoomCategoryCode.Text.Trim
            '    strSelectionFormula = "{rmcatmast.rmcatcode} LIKE '" & txtRoomCategoryCode.Text.Trim & "*'"
            'End If
            'If txtRoomCategoryName.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Room Category Name : " & txtRoomCategoryName.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {rmcatmast.rmcatname} LIKE '" & txtRoomCategoryName.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Country Name : " & txtRoomCategoryName.Text.Trim
            '        strSelectionFormula = "{rmcatmast.rmcatname} LIKE '" & txtRoomCategoryName.Text.Trim & "*'"
            '    End If
            'End If

            'If ddlSSPTypeCode.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; S.Type Code : " & CType(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text.ToUpper, String)
            '        strSelectionFormula = strSelectionFormula & " and {rmcatmast.sptypecode} = '" & CType(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text.ToUpper, String) & "'"
            '    Else
            '        strReportTitle = "S.Type Code: " & CType(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text.ToUpper, String)
            '        strSelectionFormula = "{rmcatmast.sptypecode} = '" & CType(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text.ToUpper, String) & "'"
            '    End If
            'End If

            'If ddlSSPTypeName.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; S.Type Name : " & CType(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text.ToUpper, String)
            '        strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text.ToUpper, String) & "'"
            '    Else
            '        strReportTitle = "S.Type Name: " & CType(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text.ToUpper, String)
            '        strSelectionFormula = "{sptypemast.sptypename} = '" & CType(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text.ToUpper, String) & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Room Category Name")
        ddlOrderBy.Items.Add("Room Category Code")
        'ddlOrderBy.Items.Add("Supplier Type Name")
        'ddlOrderBy.Items.Add("Supplier Type Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub
    Private Sub fillcategory()
        ddlcategorytype.Items.Clear()
        If ViewState("Type") = "Acc" Then
            ddlcategorytype.Items.Add("[Select]")
            ddlcategorytype.Items.Add("Adult Accommodation")
            ddlcategorytype.Items.Add("Child Accommodation")
            ddlcategorytype.Items.Add("Extra")
            ddlcategorytype.Items(0).Value = "S"
            ddlcategorytype.Items(1).Value = "A"
            ddlcategorytype.Items(2).Value = "C"
            ddlcategorytype.Items(3).Value = "E"

        Else
            ddlcategorytype.Items.Add("[Select]")
            ddlcategorytype.Items.Add("Adult Meal Supplements")
            ddlcategorytype.Items.Add("Child Meal Supplements")
            ddlcategorytype.Items.Add("Extra")

            ddlcategorytype.Items(0).Value = "S"
            ddlcategorytype.Items(1).Value = "M"
            ddlcategorytype.Items(2).Value = "L"
            ddlcategorytype.Items(3).Value = "E"

        End If
    End Sub





    'Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    PnlRoomCat.Visible = False
    'End Sub

    'Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    PnlRoomCat.Visible = True
    'End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("rmcatname")
            Case 1
                FillGrid("rmcatcode")
                'Case 2
                '    FillGrid("sptypemast.sptypename")
                'Case 3
                '    FillGrid("sptypemast.sptypecode")

        End Select
    End Sub
    'Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    'PnlRoom.Visible = False
    '    lblSupTypeCode.Visible = False
    '    ddlSSPTypeCode.Visible = False
    '    lblSupTypeName.Visible = False
    '    ddlSSPTypeName.Visible = False
    'End Sub

    'Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    '        PnlRoom.Visible = True
    '    lblSupTypeCode.Visible = True
    '    ddlSSPTypeCode.Visible = True
    '    lblSupTypeName.Visible = True
    '    ddlSSPTypeName.Visible = True


    'End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RoomCategoriesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetMeals(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim MealNames As New List(Of String)
        Try

            strSqlQry = "select mealname,mealcode from mealmast where mealname like  " & "'%" & prefixText & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'MealNames.Add(myDS.Tables(0).Rows(i)("mealname").ToString())
                    MealNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("mealname").ToString(), myDS.Tables(0).Rows(i)("mealcode").ToString()))
                Next

            End If

            Return MealNames
        Catch ex As Exception
            Return MealNames
        End Try

    End Function
    Protected Sub txtmealname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtmealname.TextChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("rmcatname")
            Case 1
                FillGrid("rmcatcode")

        End Select
    End Sub


    Protected Sub ddlcategorytype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlcategorytype.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("rmcatmast.rmcatname")
            Case 1
                FillGrid("rmcatmast.rmcatcode")

        End Select
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetRmcat(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet

        Dim RmcatNames As New List(Of String)

        Try

            'If Session("RoomCatetorytype") = "Acc" Then
            strSqlQry = "select rmcatname,rmcatcode from rmcatmast where  rmcatname like  " & "'%" & prefixText & "%'"
            'Else
            'strSqlQry = "select rmcatname,rmcatcode from rmcatmast where allotreqd='No' and rmcatname like  " & "'%" & prefixText & "%'"
            'End If
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'MealNames.Add(myDS.Tables(0).Rows(i)("mealname").ToString())
                    RmcatNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("rmcatname").ToString(), myDS.Tables(0).Rows(i)("rmcatcode").ToString()))
                Next

            End If

            Return RmcatNames
        Catch ex As Exception
            Return RmcatNames
        End Try

    End Function
    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strMealValue As String = ""
        Dim strCurrencyValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "CATEGORYNAME" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CATEGORYTYPE" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "MEALPLAN" Then
                        If strMealValue <> "" Then
                            strMealValue = strMealValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strMealValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Dim pagevaluecs = RowsPerPageAS.SelectedValue
            strBindCondition = BuildConditionNew(strCountryValue, strCityValue, strMealValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""

            If ViewState("Type") = "Acc" Then
                strSqlQry = "SELECT rmcatmast.rmcatcode,rmcatmast.rmcatname," _
                & " case rmcatmast.accom_extra " _
                & " when 'A' then 'Adult Accommodation' " _
                & " when 'C' then 'Child Accommodation' " _
                & " when 'M' then 'Adult Meal Supplements' " _
                & " when 'L' then 'Child Meal Supplements' " _
                & " when 'E' then 'Extra' end  as accom_extra," _
                & " rmcatmast.prnname,rmcatmast.mealyn,rmcatmast.calcyn,rmcatmast.rankorder,rmcatmast.unitname, rmcatmast.allotreqd,rmcatmast.units,rmcatmast.webname,rmcatmast.adddate,rmcatmast.adduser,rmcatmast.moddate,rmcatmast.moduser, case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive'end as active, case when rmcatmast.autoconfirm=1 then 'Yes' when rmcatmast.autoconfirm=0 then 'No' end as autoconfirm FROM rmcatmast left join sptypemast ON rmcatmast.sptypecode=sptypemast.sptypecode where accom_extra in('A','C')"
            ElseIf ViewState("Type") = "Supp" Then
                strSqlQry = "SELECT rmcatmast.rmcatcode,rmcatmast.rmcatname," _
                    & " case rmcatmast.accom_extra " _
                    & " when 'A' then 'Adult Accommodation' " _
                    & " when 'C' then 'Child Accommodation' " _
                    & " when 'M' then 'Adult Meal Supplements' " _
                    & " when 'L' then 'Child Meal Supplements' " _
                    & " when 'E' then 'Extra' end  as accom_extra," _
                    & " rmcatmast.prnname,rmcatmast.mealyn,rmcatmast.calcyn,rmcatmast.rankorder,rmcatmast.unitname, rmcatmast.allotreqd,rmcatmast.units,rmcatmast.webname,rmcatmast.adddate,rmcatmast.adduser,rmcatmast.moddate,rmcatmast.moduser, case when rmcatmast.active=1 then 'Active' when rmcatmast.active=0 then 'InActive'end as active, case when rmcatmast.autoconfirm=1 then 'Yes' when rmcatmast.autoconfirm=0 then 'No' end as autoconfirm FROM rmcatmast left join sptypemast ON rmcatmast.sptypecode=sptypemast.sptypecode where accom_extra not in('A','C')"
            End If
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"

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
                gv_SearchResult.PageSize = pagevaluecs

                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Private Function BuildConditionNew(ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strMealValue As String, ByVal strTextValue As String) As String
        Dim Categoryvalue As String = ""
        Dim Category As String()
        strWhereCond = ""
        If strCountryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(rmcatmast.rmcatname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(rmcatmast.rmcatname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If

        End If

        Category = strCityValue.Split(",")
        For i = 0 To Category.GetUpperBound(0)

            If Category(i) = "'ADULT ACCOMMODATION'" Then
                If Categoryvalue <> "" Then
                    Categoryvalue = Categoryvalue + "," + "'A'"
                Else
                    Categoryvalue = "'A'"
                End If
            ElseIf Category(i) = "'CHILD ACCOMMODATION'" Then
                If Categoryvalue <> "" Then
                    Categoryvalue = Categoryvalue + "," + "'C'"
                Else
                    Categoryvalue = "'C'"
                End If
            ElseIf Category(i) = "'ADULT MEAL SUPPLEMENTS'" Then
                If Categoryvalue <> "" Then
                    Categoryvalue = Categoryvalue + "," + "'M'"
                Else
                    Categoryvalue = "'M'"
                End If
            ElseIf Category(i) = "'CHILD MEAL SUPPLEMENTS'" Then
                If Categoryvalue <> "" Then
                    Categoryvalue = Categoryvalue + "," + "'L'"
                Else
                    Categoryvalue = "'L'"
                End If
            ElseIf Category(i) = "'EXTRA'" Then
                If Categoryvalue <> "" Then
                    Categoryvalue = Categoryvalue + "," + "'E'"
                Else
                    Categoryvalue = "'E'"
                End If



            End If






        Next
        If strCityValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(rmcatmast.accom_extra) IN (" & Trim(Categoryvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper( rmcatmast.accom_extra) IN (" & Trim(Categoryvalue.Trim.ToUpper) & ")"
            End If
        End If

        If strMealValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(rmcatmast.mealyn) IN (" & Trim(strMealValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(rmcatmast.mealyn) IN (" & Trim(strMealValue.Trim.ToUpper) & ")"
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

                        strWhereCond1 = " (upper(rmcatmast.rmcatname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(rmcatmast.accom_extra) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(rmcatmast.mealyn) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(rmcatmast.rmcatname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(rmcatmast.accom_extra) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(rmcatmast.mealyn) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),rmcatmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),rmcatmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),rmcatmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),rmcatmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If

            End If
        End If


        BuildConditionNew = strWhereCond
    End Function
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
            objUtils.WritErrorLog("RoomCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



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
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "CATEGORYNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CATEGORYNAME", lsProcessCity, "CATEGORYNAME")
                Case "CATEGORYTYPE"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CATEGORYTYPE", lsProcessCity, "CATEGORYTYPE")
                Case "MEALPLAN"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("MEALPLAN", lsProcessCity, "MEALPLAN")
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

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Public Function getRowpage() As String
        Dim rowpageas As String
        If RowsPerPageAS.SelectedValue = "20" Then
            rowpageas = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpageas = RowsPerPageAS.SelectedValue

        End If
        Return rowpageas
    End Function

    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageAS.SelectedIndexChanged
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
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub txtRoomCategoryName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRoomCategoryName.TextChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("rmcatmast.rmcatname")
            Case 1
                FillGrid("rmcatmast.rmcatcode")

        End Select
    End Sub
End Class
