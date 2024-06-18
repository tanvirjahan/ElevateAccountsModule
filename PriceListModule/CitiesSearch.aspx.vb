'------------================--------------=======================------------------================
'   Module Name    :    CitiesSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    11 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region
Partial Class CitiesSearch
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
        CityCodeTCol = 0
        CityCode = 1
        CityName = 2
        CountryCode = 3
        RankOrder = 4
        ShowinWeb = 5
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
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
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

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)

                Dim AppId As String = CType(Request.QueryString("appid"), String)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId
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

                SetFocus(txtcitycode)
                RowsPerPageCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "PriceListModule\CitiesSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryName, "ctryname", "select ctryname from ctrymast where active=1 order by ctryname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsccode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlsccode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlscname, "ctryname", "ctrycode", "select  ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)


                Session.Add("strsortExpression", "citycode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                charcters(txtcitycode)
                charcters(txtcityname)
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
                FillGridNew()
                'FillGrid("cityname")
               
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlcName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlcCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlsccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlscname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CityWindowPostBack") Then
            ' btnSearch_Click(sender, e)
            FillGridNew()
        End If
        Page.Title = "Cities Search"
    End Sub

#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If txtcitycode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(citymast.citycode) LIKE '" & Trim(txtcitycode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.citycode) LIKE '" & Trim(txtcitycode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtcityname.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(citymast.cityname) LIKE '" & Trim(txtcityname.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.cityname) LIKE '" & Trim(txtcityname.Text.Trim.ToUpper) & "%'"
            End If
        End If
        'If ddlCountryCode.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " citymast.ctrycode = '" & Trim(ddlCountryCode.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND citymast.ctrycode = '" & Trim(ddlCountryCode.SelectedValue) & "'"
        '    End If
        'End If
        'If ddlCountryName.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " ctrymast.ctryname = '" & Trim(ddlCountryName.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND ctrymast.ctryname = '" & Trim(ddlCountryName.SelectedValue) & "'"
        '    End If
        'End If

        If ddlsccode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(citymast.ctrycode) = '" & Trim(ddlsccode.Items(ddlsccode.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.ctrycode) = '" & Trim(ddlsccode.Items(ddlsccode.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If
        If ddlscname.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ctrymast.ctrycode) = '" & Trim(ddlscname.Items(ddlscname.SelectedIndex).Value.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctrycode) = '" & Trim(ddlscname.Items(ddlscname.SelectedIndex).Value.Trim.ToUpper) & "'"
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
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region


#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False
        Dim pagevaluecs = getRowpage()
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            'strSqlQry = "SELECT *,case when isnull(citymast.active,0)=1 then 'Active' when isnull(citymast.active,0)=0 then 'InActive' else 'InActive' end IsActive FROM citymast INNER JOIN ctrymast ON citymast.ctrycode=ctrymast.ctrycode"

            strSqlQry = "SELECT citymast.citycode,citymast.cityname,citymast.ctrycode,citymast.rankorder,citymast.adddate,citymast.adduser,citymast.moddate,citymast.moduser,case when isnull(citymast.active,0)=1 then 'Active' when isnull(citymast.active,0)=0 then 'InActive' end as Active, case when isnull(citymast.showweb,0)=1 then 'Yes' when isnull(citymast.showweb,0)=0 then 'No' end  as showweb FROM citymast INNER JOIN ctrymast ON citymast.ctrycode=ctrymast.ctrycode"

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY citymast." & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
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
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim CAppId As String = CType(Request.QueryString("appid"), String)
        Dim Cstrappid As String = ""
        If CAppId Is Nothing = False Then
            Cstrappid = CAppId
        End If
        Dim strpop As String = ""
        'strpop = "window.open('Cities.aspx?State=New','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('Cities.aspx?State=New&AppId=" + Cstrappid + "','Cities');"

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'Select Case ddlOrderBy.SelectedIndex

        '    Case 0
        '        FillGrid("ctrymast.ctryname")
        '    Case 1
        '        FillGrid("ctrymast.ctrycode")
        '    Case 2
        '        FillGrid("plgrpmast.plgrpcode")
        'End Select
        FillGridNew()
        'FillGrid("ctrycode")
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCityCode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Cities.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('Cities.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Cities.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Cities.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('Cities.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Cities.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Cities.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('Cities.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Cities.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("cityname")
            Case 1
                FillGrid("citycode")
            Case 2
                FillGrid("ctrycode")
        End Select
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtcitycode.Text = ""
        txtcityname.Text = ""
        ddlsccode.Value = "[Select]"
        ddlscname.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        FillGrid("cityname")
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

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblCountryName As Label = e.Row.FindControl("lblCountryName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")


            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
           

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCtry = ""

                        If "COUNTRY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CITY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                       
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCtry
                           
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblCountryName.Text = Regex.Replace(lblCountryName.Text.Trim, lsSearchTextCtry.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblCityName.Text = Regex.Replace(lblCityName.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                     

                    Next
                End If
            End If



        End If
    End Sub

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                '        Response.ContentType = "application/vnd.ms-excel"
                '        Response.Charset = ""
                '        Me.EnableViewState = False

                '        Dim tw As New System.IO.StringWriter()
                '        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
                '        Dim frm As HtmlForm = New HtmlForm()
                '        Me.Controls.Add(frm)
                '        frm.Controls.Add(gv_SearchResult)
                '        frm.RenderControl(hw)
                '        Response.Write(tw.ToString())
                '        Response.End()
                '        Response.Clear()
                '    Else
                '        objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)

                'strSqlQry = "SELECT  citycode AS [City Code] , cityname AS [City Name], ctrycode as [Country Code], rankorder as [Rank Order], showweb as [Show In Web],active as [Active],(Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created] , adduser as [User Created], (Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified], moduser as [User Modified]  FROM citymast"

                strSqlQry = "SELECT  citymast.citycode AS [City Code] , citymast.cityname AS [City Name], citymast.ctrycode as [Country Code], citymast.rankorder as [Rank Order], citymast.showweb as [Show In Web],citymast.active as [Active],(Convert(Varchar, Datepart(DD,citymast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,citymast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,citymast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,citymast.adddate))+ ':' + Convert(Varchar, Datepart(m,citymast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,citymast.adddate))) as [Date Created] , citymast.adduser as [User Created], (Convert(Varchar, Datepart(DD,citymast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,citymast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,citymast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,citymast.moddate))+ ':' + Convert(Varchar, Datepart(m,citymast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,citymast.moddate))) as [Date Modified], citymast.moduser as [User Modified]  FROM citymast inner join ctrymast on citymast.ctrycode=ctrymast.ctrycode"


                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY citycode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY citycode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "citymast")

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
        'Try
        '    Session.Add("CurrencyCode", txtcitycode.Text.Trim)
        '    Session.Add("CurrencyName", txtcityname.Text.Trim)
        '    Response.Redirect("rptCities.aspx", False)
        'Catch ex As Exception
        '    objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try
        Try
            '  Session.Add("CurrencyCode", txtcitycode.Text.Trim)
            '   Session.Add("CurrencyName", txtcityname.Text.Trim)
            '   Response.Redirect("rptCurrencies.aspx", False)

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "City")
            'Session.Add("BackPageName", "CitiesSearch.aspx")

            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=City&BackPageName=CitiesSearch.aspx&CityCode=" & txtcitycode.Text.Trim & "&CityName=" & txtcityname.Text.Trim & "&CtryCode=" & Trim(ddlsccode.Items(ddlsccode.SelectedIndex).Text) & "&CtryName=" & Trim(ddlscname.Items(ddlscname.SelectedIndex).Text) & "','RepCountry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=City&BackPageName=CitiesSearch.aspx&CityCode=" & txtcitycode.Text.Trim & "&CityName=" & txtcityname.Text.Trim & "&CtryCode=" & Trim(ddlsccode.Items(ddlsccode.SelectedIndex).Text) & "&CtryName=" & Trim(ddlscname.Items(ddlscname.SelectedIndex).Text) & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)




            'If txtcitycode.Text.Trim <> "" Then
            '    strReportTitle = "City Code : " & txtcitycode.Text.Trim
            '    strSelectionFormula = "{citymast.citycode} LIKE '" & txtcitycode.Text.Trim & "*'"
            'End If
            'If txtcityname.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; City Name : " & txtcityname.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {citymast.cityname} LIKE '" & txtcityname.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "City Name : " & txtcityname.Text.Trim
            '        strSelectionFormula = "{citymast.cityname} LIKE '" & txtcityname.Text.Trim & "*'"
            '    End If
            'End If
            'If ddlsccode.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country Code : " & CType(ddlsccode.Items(ddlsccode.SelectedIndex).Text, String)
            '        strSelectionFormula = strSelectionFormula & " and {citymast.ctrycode} = '" & CType(ddlsccode.Items(ddlsccode.SelectedIndex).Text, String) & "'"
            '    Else
            '        strReportTitle = "Country Code: " & CType(ddlsccode.Items(ddlsccode.SelectedIndex).Text, String)
            '        strSelectionFormula = "{citymast.ctrycode} = '" & CType(ddlsccode.Items(ddlsccode.SelectedIndex).Text, String) & "'"
            '    End If
            'End If

            'If ddlscname.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country Name : " & CType(ddlscname.Items(ddlscname.SelectedIndex).Text, String)
            '        strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & CType(ddlscname.Items(ddlscname.SelectedIndex).Text, String) & "'"
            '    Else
            '        strReportTitle = "Country Name: " & CType(ddlscname.Items(ddlscname.SelectedIndex).Text, String)
            '        strSelectionFormula = "{ctrymast.ctryname} = '" & CType(ddlscname.Items(ddlscname.SelectedIndex).Text, String) & "'"
            '    End If
            'End If
            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '   pnlSearch.Visible = False
        lblctrycode.Visible = False
        ddlsccode.Visible = False
        ddlscname.Visible = False
        lblctryname.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' pnlSearch.Visible = True
        lblctrycode.Visible = True
        ddlsccode.Visible = True
        ddlscname.Visible = True
        lblctryname.Visible = True
    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("City Name")
        ddlOrderBy.Items.Add("City Code")
        ddlOrderBy.Items.Add("Country Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("cityname")
            Case 1
                FillGrid("citycode")
            Case 2
                FillGrid("ctrycode")
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
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strCurrencyValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CITY" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strCurrencyValue <> "" Then
                            strCurrencyValue = strCurrencyValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCurrencyValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    
                Next
            End If
            Dim pagevaluecs = RowsPerPageCS.SelectedValue
            strBindCondition = BuildConditionNew(strCountryValue, strCityValue, strCurrencyValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = "select citycode,cityname,ctryname,rankorder,showweb,citymast.active,citymast.adddate,citymast.adduser,citymast.moddate,citymast.moduser  ,case when isnull(dbo.ctrymast.active,0)=1 then 'Active'   when isnull(dbo.ctrymast.active,0)=0 then 'InActive' else 'InActive' end isactive from citymast inner join ctrymast on citymast.ctrycode=ctrymast.ctrycode"
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"

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
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CitiesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Function BuildConditionNew(ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strCurrencyValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCountryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If

        End If

        If strCityValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(citymast.cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(citymast.cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
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

                        strWhereCond1 = " (upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),ctrymast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),ctrymast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),ctrymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),ctrymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If

            End If
        End If


        BuildConditionNew = strWhereCond
    End Function

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



  

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Public Function getRowpage() As String
        Dim rowpagecs As String
        If RowsPerPageCS.SelectedValue = "20" Then
            rowpagecs = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagecs = RowsPerPageCS.SelectedValue

        End If
        Return rowpagecs
    End Function

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "COUNTRY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCity, "COUNTRY")
                Case "CITY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CITY", lsProcessCity, "CITY")

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

    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCS.SelectedIndexChanged
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

End Class