'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceCategoriesSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    16 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class OtherServiceCategoriesSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim objUser As New clsUser
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim FindPage As String
#End Region

#Region "Enum GridCol"
    Enum GridCol
        CategoryCodeTCol = 0
        CategoryCode = 1
        CategoryName = 2
        GroupCode = 3
        GroupName = 4
        RankOrder = 5
        MinPax = 6
        MaxPax = 7
        UnitName = 8
        PrintRemarks = 9
        PaxCheckRequired = 10
        Active = 11
        CalculateByPaxUnits = 12
        DateCreated = 13
        UserCreated = 14
        DateModified = 15
        UserModified = 16
        Edit = 17
        View = 18
        Delete = 19

    End Enum
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Request.QueryString("VehiclePage") = "Yes" Then
            FindPage = "Yes"
            lblHead.Text = "Vehicle Type List"
        Else
            FindPage = "No"
            lblHead.Text = "Other Service Categories List"
        End If

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
                'If Session("SamePageDiffmenu") = "Yes" Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "I Am Vehicle " & "' );", True)
                'Else
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "I Am Other Service " & "' );", True)

                'End If

                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & CType(Session("VehicleType"), String) & "' );", True)
                '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGrpCode, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)
                ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGrpNm, "othgrpname", "select othgrpname from othgrpmast where active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)

                SetFocus(txtCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"),CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(strappname, String), "PriceListModule\OtherServiceCategoriesSearch.aspx", btnAddNew, btnExportToExcel, btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                Session.Add("strsortExpression", "othcatmast.othcatcode")
                Session.Add("strsortdirection", SortDirection.Ascending)

                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGrpCode, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)
                fillorderby()
                FillGrid("othcatmast.othcatname")
                charcters(txtCode)
                charcters(txtName)
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServiceTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OthcatWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtCode.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othcatmast.othcatcode) LIKE '" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othcatmast.othcatcode) LIKE '" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            End If
        End If

        If txtName.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othcatmast.othcatname) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othcatmast.othcatname) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            End If
        End If
        'If ddlOtherGrpCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " othgrpmast.othgrpcode= '" & Trim(ddlOtherGrpCode.Value) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND  othgrpmast.othgrpcode= '" & Trim(ddlOtherGrpCode.Value) & "'"
        '    End If
        'End If

        'If ddlOtherGrpName.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " othgrpmast.othgrpname= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND  othgrpmast.othgrpname= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text) & "'"
        '    End If
        'End If

        If ddlOtherGrpCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othgrpmast.othgrpcode)= '" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(othgrpmast.othgrpcode)= '" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text) & "'"
            End If
        End If

        If ddlOtherGrpName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othgrpmast.othgrpname)= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND  upper(othgrpmast.othgrpname)= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If
        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
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
            ' strSqlQry = "SELECT * FROM othtypmast"
            strSqlQry = " SELECT     dbo.othcatmast.othcatcode, dbo.othcatmast.othcatname,case isnull(dbo.othcatmast.adultchild,'k') when 'A' then 'adult' when 'C' then 'child' when 'K' then 'nil' end adultchild , dbo.othcatmast.othgrpcode, dbo.othgrpmast.othgrpname, dbo.othcatmast.grporder,  " & _
                      "dbo.othcatmast.minpax, dbo.othcatmast.maxpax, dbo.othcatmast.unitname,case when isnull(dbo.othcatmast.printremarks,0)=1 then 'Yes' else 'No' end as printremarks, case when isnull(dbo.othcatmast.paxcheckreqd,0)=1 then 'Yes' when isnull(dbo.othcatmast.paxcheckreqd,0)=0 then 'No' end as paxcheckreqd, " & _
                      "case when isnull(dbo.othcatmast.active,0)=1 then 'Active' when isnull(dbo.othcatmast.active,0)=0 then 'InActive' else 'InActive' end Active , " & _
                      "dbo.othcatmast.calcyn, dbo.othcatmast.adddate, dbo.othcatmast.adduser, dbo.othcatmast.moddate, " & _
                       "dbo.othcatmast.moduser FROM         dbo.othcatmast INNER JOIN   dbo.othgrpmast ON dbo.othcatmast.othgrpcode = dbo.othgrpmast.othgrpcode "
            If Trim(BuildCondition) <> "" Then
                ' strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                If FindPage = "Yes" Then
                    strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                Else
                    strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder

                End If
            Else
                '  strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode<>'TRFS' "
                If FindPage = "Yes" Then
                    strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                Else
                    strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If
              
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
            objUtils.WritErrorLog("OtherServiceCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("OtherServiceCategories.aspx", False)
        Dim strpop As String = ""
        If FindPage = "Yes" Then
            'strpop = "window.open('OtherServiceCategories.aspx?State=New&VehiclePage=Yes','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('OtherServiceCategories.aspx?State=New&VehiclePage=Yes','Othcat');"
        Else
            'strpop = "window.open('OtherServiceCategories.aspx?State=New','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('OtherServiceCategories.aspx?State=New','Othcat');"
        End If
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("othcatcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othcatmast.othcatname")
            Case 1
                FillGrid("othcatmast.othcatcode")
            Case 2
                FillGrid("othgrpmast.othgrpname")
            Case 3
                FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblothtypcode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceCategories.aspx", False)
                Dim strpop As String = ""
                If FindPage = "Yes" Then

                    'strpop = "window.open('OtherServiceCategories.aspx?State=Edit&VehiclePage=Yes&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('OtherServiceCategories.aspx?State=Edit&VehiclePage=Yes&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat');"
                Else
                    'strpop = "window.open('OtherServiceCategories.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('OtherServiceCategories.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat');"
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceCategories.aspx", False)
                Dim strpop As String = ""
                If FindPage = "Yes" Then

                    ' strpop = "window.open('OtherServiceCategories.aspx?State=View&VehiclePage=Yes&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('OtherServiceCategories.aspx?State=View&VehiclePage=Yes&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat');"
                Else
                    strpop = "window.open('OtherServiceCategories.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    'strpop = "window.open('OtherServiceCategories.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat');"
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                    'Session.Add("State", "Delete")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("OtherServiceCategories.aspx", False)
                Dim strpop As String = ""
                If FindPage = "Yes" Then
                    'strpop = "window.open('OtherServiceCategories.aspx?State=Delete&VehiclePage=Yes&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('OtherServiceCategories.aspx?State=Delete&VehiclePage=Yes&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat');"
                Else
                    'strpop = "window.open('OtherServiceCategories.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('OtherServiceCategories.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Othcat');"
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("othcatcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othcatmast.othcatname")
            Case 1
                FillGrid("othcatmast.othcatcode")
            Case 2
                FillGrid("othgrpmast.othgrpname")
            Case 3
                FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtCode.Value = ""
        txtName.Value = ""
        'ddlGrpCode.SelectedValue = "[Select]"
        'ddlGrpNm.SelectedValue = "[Select]"
        ddlOtherGrpCode.Value = "[Select]"
        ddlOtherGrpName.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        FillGrid("othcatmast.othcatname")
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        '  FillGrid(e.SortExpression, direction)
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

                strSqlQry = "SELECT othcatcode AS [Category Code],othcatname AS [Category Name],othcatmast.othgrpcode as [Group Code],othgrpmast.othgrpname as [Group Name],othcatmast.grporder as [Rank Order],othcatmast.minpax as [Min Pax],othcatmast.maxpax as [Max Pax],othcatmast.unitname as [Unit Name],othcatmast.printremarks as [Print Remark],othcatmast.paxcheckreqd as [Pax Check Required],[Active]=case when othcatmast.active=1 then 'Active' when othcatmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,othcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othcatmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othcatmast.adddate))+ ':' + Convert(Varchar, Datepart(m,othcatmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,othcatmast.adddate))) as [Date Created],othcatmast.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,othcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,othcatmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othcatmast.moddate))+ ':' + Convert(Varchar, Datepart(m,othcatmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,othcatmast.moddate))) as [Date Modified]FROM othcatmast INNER JOIN othgrpmast ON othcatmast.othgrpcode = othgrpmast.othgrpcode"

                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY othcatcode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY othcatcode"
                'End If

                If Trim(BuildCondition) <> "" Then
                    ' strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                    If FindPage = "Yes" Then
                        strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY  othcatcode"
                    Else
                        strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY othcatcode "

                    End If
                Else
                    '  strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode<>'TRFS' "
                    If FindPage = "Yes" Then
                        strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "

                        strSqlQry = strSqlQry & " ORDER BY othcatcode"
                    Else
                        strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "

                        strSqlQry = strSqlQry & " ORDER BY othcatcode"
                    End If

                End If

                'If Trim(BuildCondition) <> "" Then
                '    'strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                '    strSqlQry = strSqlQry & " WHERE dbo.othgrpmast.othcatcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY othcatcode"
                'Else
                '    strSqlQry = strSqlQry & " Where dbo.othgrpmast.othcatcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "
                '    strSqlQry = strSqlQry & " ORDER BY othcatcode "
                'End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "othcatmast")

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
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Other Service Category")
            'Session.Add("BackPageName", "OtherServiceCategoriesSearch.aspx")
            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=Other Service Category&BackPageName=OtherServiceCategoriesSearch.aspx&OthcatCode=" & txtCode.Value.Trim & "&OthcatName=" & txtName.Value.Trim & "&OthgrpCode=" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text) & "','RepOthCat','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Other Service Category&BackPageName=OtherServiceCategoriesSearch.aspx&OthcatCode=" & txtCode.Value.Trim & "&OthcatName=" & txtName.Value.Trim & "&OthgrpCode=" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text) & "','RepOthCat');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'If txtCode.Value.Trim <> "" Then
            '    strReportTitle = "Category Code : " & txtCode.Value.Trim
            '    strSelectionFormula = "{othcatmast.othcatcode} LIKE '" & txtCode.Value.Trim & "*'"
            'End If
            'If txtName.Value.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Category Name : " & txtName.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {othcatmast.othcatname} LIKE '" & txtName.Value.Trim & "*'"
            '    Else
            '        strReportTitle = "Category Name : " & txtName.Value.Trim
            '        strSelectionFormula = "{othcatmast.othcatname} LIKE '" & txtName.Value.Trim & "*'"
            '    End If
            'End If

            'If ddlOtherGrpCode.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Group  Code : " & CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.ToUpper, String)
            '        strSelectionFormula = strSelectionFormula & " and {othcatmast.othgrpcode} = '" & CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.ToUpper, String) & "'"
            '    Else
            '        strReportTitle = "Group  Code: " & CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.ToUpper, String)
            '        strSelectionFormula = "{othcatmast.othgrpcode} = '" & CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.ToUpper, String) & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region




    'Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    PnlOtherSerCat.Visible = False
    'End Sub

    'Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    PnlOtherSerCat.Visible = True
    'End Sub

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlOtherCat.Visible = False
        lblgrpcode.Visible = False
        ddlOtherGrpCode.Visible = False
        lblgrpname.Visible = False
        ddlOtherGrpName.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlOtherCat.Visible = True
        lblgrpcode.Visible = True
        ddlOtherGrpCode.Visible = True
        lblgrpname.Visible = True
        ddlOtherGrpName.Visible = True
    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Category Name")
        ddlOrderBy.Items.Add("Category Code")
        ddlOrderBy.Items.Add("Group Name")
        ddlOrderBy.Items.Add("Group Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othcatmast.othcatname")
            Case 1
                FillGrid("othcatmast.othcatcode")
            Case 2
                FillGrid("othgrpmast.othgrpname")
            Case 3
                FillGrid("othgrpmast.othgrpcode")
            
        End Select
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServiceCategoriesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
