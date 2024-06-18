'------------================--------------=======================------------------================
'   Module Name    :    CountriesSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    11 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.ArrayList
Imports System.Collections.Generic

#End Region

Partial Class CountriesSearch
    Inherits System.Web.UI.Page
#Region "Enum GridCol"
    Enum GridCol
        CountryCodeTCol = 0
        CountryCode = 1
        CountryName = 2
        CurrencyCode = 3
        'WeekendFrom1 = 4
        'WeekendTo1 = 5
        'WeekendFrom2 = 6
        'WeekendTo2 = 7
        PriceListGroup = 4
        Nationality = 5
        IncludeinPromotions = 6
        IncludeinEarlyBirdPromotions = 7
        Active = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 13
        View = 14
        Delete = 15
    End Enum
#End Region

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
 

    ' Dim objclsConnectionName As clsConnectionName
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
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlcurrcode, "currcode", "select currcode from currmast where active=1 order by currcode", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlcurrnm, "currname", "select currname from currmast where active=1 order by currname", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlmarketcd, "plgrpcode", "select plgrpcode from plgrpmast where active=1 order by plgrpcode", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlmarketnm, "plgrpname", "select plgrpname from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSCurCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSCurName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                'objclsConnectionName.ConnectionName = Session("dbconnectionName").ToString
                ' objclsConnectionName.co = Session("dbconnectionName").ToString
                SetFocus(txtcountrycode)
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppId As String = CType(Request.QueryString("appid"), String)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
              
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
                RowsPerPageCCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\CountriesSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
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




                Session.Add("strsortExpression", "ctrycode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                fillorderby()
                'FillGrid("ctrymast.ctryname")
                FillGridNew()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSCurName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSCurCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSMktName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSMktCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CtryWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
        Page.Title = "Countries Search"
    End Sub

#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtcountrycode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(ctrymast.ctrycode) LIKE '" & Trim(txtcountrycode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctrycode) LIKE '" & Trim(txtcountrycode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtcountryname.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(ctrymast.ctryname) LIKE '" & Trim(txtcountryname.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) LIKE '" & Trim(txtcountryname.Text.Trim.ToUpper) & "%'"
            End If
        End If

        'If ddlcurrcode.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " currmast.currcode = '" & Trim(ddlcurrcode.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND currmast.currcode = '" & Trim(ddlcurrcode.SelectedValue) & "'"
        '    End If
        'End If

        'If ddlcurrnm.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " currmast.currname = '" & Trim(ddlcurrnm.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND currmast.currname = '" & Trim(ddlcurrnm.SelectedValue) & "'"
        '    End If
        'End If

        'If ddlmarketcd.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " plgrpmast.plgrpcode = '" & Trim(ddlmarketcd.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND plgrpmast.plgrpcode = '" & Trim(ddlmarketcd.SelectedValue) & "'"
        '    End If
        'End If

        'If ddlmarketnm.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " plgrpmast.plgrpname = '" & Trim(ddlmarketnm.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND plgrpmast.plgrpname = '" & Trim(ddlmarketnm.SelectedValue) & "'"
        '    End If
        'End If
        If ddlSCurCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " currmast.currcode = '" & Trim(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND currmast.currcode = '" & Trim(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text) & "'"
            End If
        End If

        If ddlSCurName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " currmast.currname = '" & Trim(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND currmast.currname = '" & Trim(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text) & "'"
            End If
        End If
        If ddlSMktCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " plgrpmast.plgrpcode = '" & Trim(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND plgrpmast.plgrpcode = '" & Trim(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text) & "'"
            End If
        End If

        If ddlSMktName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " plgrpmast.plgrpname = '" & Trim(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND plgrpmast.plgrpname = '" & Trim(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text) & "'"
            End If
        End If
        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        'Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        gv_SearchResult.Visible = True
        lblMsg.Visible = False
        Dim pagevalue = getRowpage()
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            'strSqlQry = "SELECT * FROM ctrymast"
            'strSqlQry = "SELECT * FROM ctrymast INNER JOIN currmast ON ctrymast.currcode=currmast.currcode"
            strSqlQry = "SELECT dbo.ctrymast.*,case when isnull(dbo.ctrymast.active,0)=1 then 'Active'  " & _
                       " when isnull(dbo.ctrymast.active,0)=0 then 'InActive' else 'InActive' end isactive, " & _
                       "  dbo.currmast.currname, dbo.plgrpmast.plgrpname " & _
                        "FROM dbo.ctrymast INNER JOIN " & _
                        "dbo.currmast ON dbo.ctrymast.currcode = dbo.currmast.currcode INNER JOIN " & _
                        "dbo.plgrpmast ON dbo.ctrymast.plgrpcode = dbo.plgrpmast.plgrpcode"
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
                gv_SearchResult.PageSize = pagevalue
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        'strpop = "window.open('Countries.aspx?State=New','Countries','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        'strpop = "window.open('Countries.aspx?State=New','Countries','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('Countries.aspx?State=New&AppId=" + Cstrappid + "','Countries');"
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
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcntryCode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Countries.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('Countries.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Countries','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

                strpop = "window.open('Countries.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Countries');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Countries.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('Countries.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Countries','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Countries.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Countries');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Countries.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('Countries.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Countries','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Countries.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Countries');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("ctrycode")
        Select Case ddlOrderBy.SelectedIndex

            Case 0
                FillGrid("ctrymast.ctryname")
            Case 1
                FillGrid("ctrymast.ctrycode")
            Case 2
                FillGrid("plgrpmast.plgrpcode")
        End Select
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtcountrycode.Text = ""
        txtcountryname.Text = ""
        ddlSCurCode.Value = "[Select]"
        ddlSCurName.Value = "[Select]"
        ddlSMktCode.Value = "[Select]"
        ddlSMktName.Value = "[Select]"
        Me.ddlOrderBy.SelectedIndex = 0
        FillGrid("ctrymast.ctryname")
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
                '    Response.Clear()
                'Else
                '    objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
                '    strSqlQry = "SELECT  subseascode AS [Sub Season Code] , subseasname AS [Sub Season Name], dispname as [Display Name], disporder as [Display Order], active as [Active], (Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created] , adduser as [User Created], (Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified], moduser as [User Modified] FROM subseasmast"
                strSqlQry = "SELECT  ctrycode AS [Country Code] , ctryname AS [Country Name], currcode as [Currency Code], plgrpcode as[Region],active as [Active], (Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created] , adduser as [User Created], (Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified], moduser as [User Modified] FROM ctrymast"

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY ctrycode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY ctrycode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "ctrymast")

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
        '    Try
        '        'Session.Add("CurrencyCode", txtcountrycode.Text.Trim)
        '        'Session.Add("CurrencyName", txtcountryname.Text.Trim)
        '        'Response.Redirect("rptCurrencies.aspx", False)
        '    Catch ex As Exception
        '        objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        '    End Try
        'End Sub
        Try
            'Session.Add("CurrencyCode", txtcountrycode.Text.Trim)
            'Session.Add("CurrencyName", txtcountryname.Text.Trim)
            'Response.Redirect("rptCurrencies.aspx", False)

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session("ColReportParams") = Nothing

            'Session.Add("Pageame", "Country")
            'Session.Add("BackPageName", "CountriesSearch.aspx")

            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=Country&BackPageName=CountriesSearch.aspx&CtryCode=" & txtcountrycode.Text.Trim & "&CtryName=" & txtcountryname.Text.Trim & "&CurrCode=" & Trim(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text) & "&CurrName=" & Trim(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text) & "&MktCode=" & Trim(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text) & "&MktName=" & Trim(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text) & "','RepCountry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Country&BackPageName=CountriesSearch.aspx&CtryCode=" & txtcountrycode.Text.Trim & "&CtryName=" & txtcountryname.Text.Trim & "&CurrCode=" & Trim(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text) & "&CurrName=" & Trim(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text) & "&MktCode=" & Trim(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text) & "&MktName=" & Trim(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text) & "','RepCountry');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'If txtcountrycode.Text.Trim <> "" Then
            '    strReportTitle = "Country Code : " & txtcountrycode.Text.Trim
            '    strSelectionFormula = "{ctrymast.ctrycode} LIKE '" & txtcountrycode.Text.Trim & "*'"
            'End If
            'If txtcountryname.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country Name : " & txtcountryname.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} LIKE '" & txtcountryname.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Country Name : " & txtcountryname.Text.Trim
            '        strSelectionFormula = "{ctrymast.ctryname} LIKE '" & txtcountryname.Text.Trim & "*'"
            '    End If
            'End If
            'If ddlSCurCode.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Currency Code : " & Trim(CType(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text, String))
            '        strSelectionFormula = strSelectionFormula & " and {ctrymast.currcode} = '" & Trim(CType(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text, String)) & "'"
            '    Else
            '        strReportTitle = "Currency Code: " & Trim(CType(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text, String))
            '        strSelectionFormula = "{ctrymast.currcode} = '" & Trim(CType(ddlSCurCode.Items(ddlSCurCode.SelectedIndex).Text, String)) & "'"
            '    End If
            'End If

            'If ddlSCurName.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Currency Name : " & Trim(CType(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text, String))
            '        strSelectionFormula = strSelectionFormula & " and {currmast.currname} = '" & Trim(CType(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text, String)) & "'"
            '    Else
            '        strReportTitle = "Currency Name: " & Trim(CType(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text, String))
            '        strSelectionFormula = "{currmast.currname} = '" & Trim(CType(ddlSCurName.Items(ddlSCurName.SelectedIndex).Text, String)) & "'"
            '    End If
            'End If
            'If ddlSMktCode.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Market Code : " & Trim(CType(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text, String))
            '        strSelectionFormula = strSelectionFormula & " and {ctrymast.plgrpcode} = '" & Trim(CType(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text, String)) & "'"
            '    Else
            '        strReportTitle = "Market Code: " & Trim(CType(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text, String))
            '        strSelectionFormula = "{ctrymast.plgrpcode} = '" & Trim(CType(ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text, String)) & "'"
            '    End If
            'End If

            'If ddlSMktName.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Market Name : " & Trim(CType(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text, String))
            '        strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} = '" & Trim(CType(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text, String)) & "'"
            '    Else
            '        strReportTitle = "Market Name: " & Trim(CType(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text, String))
            '        strSelectionFormula = "{plgrpmast.plgrpname} = '" & Trim(CType(ddlSMktName.Items(ddlSMktName.SelectedIndex).Text, String)) & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        PnlCurr.Visible = False
    End Sub

    Protected Sub rbnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbnadsearch.CheckedChanged
        PnlCurr.Visible = True
    End Sub

    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Country Name")
        ddlOrderBy.Items.Add("Country Code")
        ddlOrderBy.Items.Add("Region Code")

        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex

            Case 0
                FillGrid("ctrymast.ctryname")
            Case 1
                FillGrid("ctrymast.ctrycode")
            Case 2
                FillGrid("plgrpmast.plgrpcode")
        End Select
    End Sub

   

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountries(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim counTryNames As New List(Of String)
        Try

            strSqlQry = "select ctryname from ctrymast where ctryname like  " & "'%" & prefixText & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    counTryNames.Add(myDS.Tables(0).Rows(i)("ctryname").ToString())
                Next

            End If

            Return counTryNames
        Catch ex As Exception
            Return counTryNames
        End Try

    End Function

    '<System.Web.Script.Services.ScriptMethod()> _
    '<System.Web.Services.WebMethod()> _
    'Public Function GetCountriesForGrid(ByVal prefixText As String) As String

    '    Dim strSqlQry As String = ""
    '    Dim str As String = ""
    '    Dim myDS As New DataSet
    '    Dim counTryNames As New List(Of String)
    '    Try

    '        strSqlQry = "select ctryname from ctrymast where ctryname like  " & "'" & prefixText & "%'"
    '        Dim SqlConn As New SqlConnection
    '        Dim myDataAdapter As New SqlDataAdapter
    '        '  SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
    '        'Open connection
    '        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '        myDataAdapter.Fill(myDS)

    '        'If myDS.Tables(0).Rows.Count > 0 Then
    '        '    For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
    '        '        counTryNames.Add(myDS.Tables(0).Rows(i)("ctryname").ToString())
    '        '    Next

    '        'End If
    '        Return str

    '    Catch ex As Exception
    '        Return str
    '    End Try

    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtcountryname_TextChanged(sender As Object, e As System.EventArgs) Handles txtcountryname.TextChanged
        FillGrid("ctrymast.ctryname")
    End Sub


    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "REGION"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("REGION", lsProcessCity, "REGION")
                Case "COUNTRY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCity, "COUNTRY")

                Case "CURRENCY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CURRENCY", lsProcessCity, "CURRENCY")

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
   
    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then



            Dim lblCountryName As Label = e.Row.FindControl("lblCountryName")

            Dim lblCurrencyName As Label = e.Row.FindControl("lblCurrencyName")
            Dim lblRegionName As Label = e.Row.FindControl("lblRegionName")
            Dim lsSearchTextCtry As String = ""
            Dim lsSearchRegion As String = ""
            Dim lsSearchTextRegion As String = ""
            Dim lsSearchTextCurrency As String = ""
            Dim lsSearchTextSectorGroup As String = ""

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCtry = ""

                        If "REGION" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchRegion = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "COUNTRY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CURRENCY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCurrency = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextRegion = lsSearchTextCtry
                            lsSearchTextCurrency = lsSearchTextCtry
                            'lsSearchRegion = lsSearchRegion
                            'lsSearchTextSector = lsSearchTextCtry
                            ' lsSearchTextSectorGroup = lsSearchTextCtry
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblCountryName.Text = Regex.Replace(lblCountryName.Text.Trim, lsSearchTextCtry.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchRegion.Trim <> "" Then
                            lblRegionName.Text = Regex.Replace(lblRegionName.Text.Trim, lsSearchRegion.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If


                        
                        If lsSearchTextCurrency.Trim <> "" Then
                            lblCurrencyName.Text = Regex.Replace(lblCurrencyName.Text.Trim, lsSearchTextCurrency.Trim(), _
                                  Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                             RegexOptions.IgnoreCase)
                        End If
                        '  If lsSearchTextRegion.Trim <> "" Then
                        'lblRegionName.Text = Regex.Replace(lblRegionName.Text.Trim, lsSearchTextRegion.Trim(), _
                        ' Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        ' RegexOptions.IgnoreCase)
                        ' End If

                    Next
                End If
            End If



        End If
    End Sub

    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strRegionValue As String = ""
        Dim strCountryValue As String = ""
        Dim strCurrencyValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "REGION" Then
                        If strRegionValue <> "" Then
                            strRegionValue = strRegionValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strRegionValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "CURRENCY" Then
                        If strCurrencyValue <> "" Then
                            strCurrencyValue = strCurrencyValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCurrencyValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Dim pagevalueccs = RowsPerPageCCS.SelectedValue
            strBindCondition = BuildConditionNew(strRegionValue, strCountryValue, strCurrencyValue, strTextValue)
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
            strSqlQry = "SELECT dbo.ctrymast.*,case when isnull(dbo.ctrymast.active,0)=1 then 'Active'   when isnull(dbo.ctrymast.active,0)=0 then 'InActive' else 'InActive' end isactive,  dbo.currmast.currname, dbo.plgrpmast.plgrpname FROM dbo.ctrymast INNER JOIN dbo.currmast ON dbo.ctrymast.currcode = dbo.currmast.currcode INNER JOIN dbo.plgrpmast ON dbo.ctrymast.plgrpcode = dbo.plgrpmast.plgrpcode"

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
                gv_SearchResult.PageSize = pagevalueccs
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CountriesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Private Function BuildConditionNew(ByVal strRegionValue As String, ByVal strCountryValue As String, ByVal strCurrencyValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strRegionValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then



                strWhereCond = "upper(plgrpmast.plgrpname) IN (" & Trim(strRegionValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(plgrpmast.plgrpname) IN (" & Trim(strRegionValue.Trim.ToUpper) & ")"
            End If



        End If

        If strCountryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If


        If strCurrencyValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(currmast.currname) IN (" & Trim(strCurrencyValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(currmast.currname) IN (" & Trim(strCurrencyValue.Trim.ToUpper) & ")"
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

                        strWhereCond1 = " (upper(plgrpmast.plgrpname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(currmast.currname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(plgrpmast.plgrpname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(currmast.currname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') "
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



    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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
    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
    Public Function getRowpage() As String
        Dim rowpageccs As String
        If RowsPerPageCCS.SelectedValue = "20" Then
            rowpageccs = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpageccs = RowsPerPageCCS.SelectedValue

        End If
        Return rowpageccs
    End Function
    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCCS.SelectedIndexChanged
        FillGridNew()
    End Sub


End Class




