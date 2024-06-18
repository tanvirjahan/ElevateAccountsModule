'------------================--------------=======================------------------================
'   Module Name    :    SectorSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    11 June 2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

#End Region

Partial Class SectorSearchScrollLoad
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
        SectorCodeTCol = 0
        SectorCode = 1
        SectorName = 2
        CountryCode = 3
        CountryName = 4
        CityCode = 5
        CityName = 6
        Active = 7
        DateCreated = 8
        UserCreated = 9
        DateModified = 10
        UserModified = 11
        Edit = 12
        View = 13
        Delete = 14

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
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If

            Dim strScript As String = "javascript:fnRemoveHeader()"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)

        End If
    End Sub


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim default_group As String
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

                txtconnection.Value = Session("dbconnectionName")

                SetFocus(TxtSectorCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\SectorSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1001", String))
                hdDefault_Group.Value = default_group
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSelectGroupCode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast where active=1 and othgrpcode ='" & default_group & "' order by othtypcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSelectGroupName, "othtypname", "othtypcode", "select othtypname,othtypcode from othtypmast where active=1 and othgrpcode ='" & default_group & "' order by othtypname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where active=1 order by cityname", True)


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCountryCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
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


                Session.Add("strsortExpression", "sectorname")
                Session.Add("strsortdirection", SortDirection.Ascending)
                fillorderby()
                FillGrid("sectormaster.sectorname")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Try
                If ddlCountryCode.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where ctrycode='" & ddlCountryName.Value & "' and active=1 order by citycode", True, ddlCityCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where ctrycode='" & ddlCountryName.Value & "' and active=1 order by cityname", True, ddlCityName.Value)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where active=1 order by citycode", True, ddlCityCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where active=1 order by cityname", True, ddlCityName.Value)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        ' Added by Abin on 20160815
        Dim strScript As String = "javascript:fnRemoveHeader()"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        'End....
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SectSupWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""




        If TxtSectorCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(sectormaster.sectorcode) LIKE '" & Trim(TxtSectorCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(sectormaster.sectorcode) LIKE '" & Trim(TxtSectorCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If TxtSecorName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(sectormaster.sectorname) LIKE '" & Trim(TxtSecorName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) LIKE '" & Trim(TxtSecorName.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If Me.ddlSelectGroupCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othtypmast.othtypcode) = '" & Trim(ddlSelectGroupCode.Items(ddlSelectGroupCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypcode) = '" & Trim(ddlSelectGroupCode.Items(ddlSelectGroupCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If
        If Me.ddlSelectGroupName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othtypmast.othtypname) = '" & Trim(ddlSelectGroupName.Items(ddlSelectGroupName.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypname) = '" & Trim(ddlSelectGroupName.Items(ddlSelectGroupName.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If


        If Me.ddlCountryCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ctrymast.ctrycode) = '" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctrycode) = '" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If

        If Me.ddlCountryName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ctrymast.ctryname) = '" & Trim(ddlCountryName.Items(Me.ddlCountryName.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) = '" & Trim(ddlCountryName.Items(Me.ddlCountryName.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If

        If Me.ddlCityCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(citymast.citycode) = '" & Trim(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.citycode) = '" & Trim(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If

        If Me.ddlCityName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(citymast.cityname) = '" & Trim(Me.ddlCityName.Items(Me.ddlCityName.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.cityname) = '" & Trim(Me.ddlCityName.Items(Me.ddlCityName.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If

        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " CONVERT(datetime, convert(varchar(10),sectormaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103) "
                Else
                    strWhereCond = strWhereCond & "  and CONVERT(datetime, convert(varchar(10),sectormaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " CONVERT(datetime, convert(varchar(10),sectormaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103) "
                Else
                    strWhereCond = strWhereCond & "  and CONVERT(datetime, convert(varchar(10),sectormaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103) "
                End If
            End If



        End If


        BuildCondition = strWhereCond
    End Function


#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim strValue As String

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try


            ' strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end FROM sectormaster INNER JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON sectormaster.citycode = citymast.citycode INNER JOIN othtypmast ON sectormaster.sectorgroupcode  = othtypmast.othtypcode"
            strSqlQry = "SELECT  ROW_NUMBER() OVER ( " & " ORDER BY " & strorderby & " " & strsortorder & ")AS RowNumber,sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster INNER JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON sectormaster.citycode = citymast.citycode INNER JOIN othtypmast ON sectormaster.sectorgroupcode  = othtypmast.othtypcode"
            strValue = Trim(BuildCondition())
            'If strValue <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & strValue & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If
            If strValue <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strValue
            Else
                strSqlQry = strSqlQry
            End If

            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(myDS)

            Session("SSqlQuery") = strSqlQry
            myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)

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
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("Sector.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('Sector.aspx?State=New','Sectors','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('Sector.aspx?State=New','Sectors');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("sectormaster.sectorcode")
        'Select Case ddlOrderBy.SelectedIndex
        '    Case 0
        '        FillGrid("sectormaster.sectorname")
        '    Case 1
        '        FillGrid("sectormaster.sectorcode")
        '    Case 2
        '        FillGrid("citymast.cityname")
        '    Case 3
        '        FillGrid("citymast.citycode")
        '    Case 4
        '        FillGrid("ctrymast.ctryname")
        '    Case 5
        '        FillGrid("ctrymast.ctrycode")
        'End Select
        FillGridNew()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            If e.CommandName = "lnkedit1" Then
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim hdSecCode As TextBox = row.FindControl("hdSecCode")

                Dim strScript As String = "javascript:fnRemoveHeader()"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)


                Dim str As String = gv_SearchResult.Rows.Count
                Dim lblId As HiddenField
                '  lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblSectorCode")

                '    lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("hdSecCode")
                If e.CommandName = "lnkedit1" Then
                    'Session.Add("State", "Edit")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Sector.aspx", False)
                    Dim strpop As String = ""
                    'strpop = "window.open('Sector.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Sectors','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Sector.aspx?State=Edit&RefCode=" + CType(hdSecCode.Text.Trim, String) + "','Sectors');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "View" Then
                    'Session.Add("State", "View")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Sector.aspx", False)
                    Dim strpop As String = ""
                    'strpop = "window.open('Sector.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Sectors','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Sector.aspx?State=View&RefCode=" + CType(hdSecCode.Text.Trim, String) + "','Sectors');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "DeleteRow" Then
                    'Session.Add("State", "Delete")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Sector.aspx", False)
                    Dim strpop As String = ""
                    'strpop = "window.open('Sector.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Sectors','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Sector.aspx?State=Delete&RefCode=" + CType(hdSecCode.Text.Trim, String) + "','Sectors');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("sectormaster.sectorname")
            Case 1
                FillGrid("sectormaster.sectorcode")
            Case 2
                FillGrid("citymast.cityname")
            Case 3
                FillGrid("citymast.citycode")
            Case 4
                FillGrid("ctrymast.ctryname")
            Case 5
                FillGrid("ctrymast.ctrycode")
        End Select
    End Sub

#End Region
#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtSectorCode.Text = ""
        TxtSecorName.Text = ""
        'ddlcountrycd.SelectedValue = "[Select]"
        'ddlcountrynm.SelectedValue = "[Select]"
        'ddlmarketcd.SelectedValue = "[Select]"
        'ddlmarketnm.SelectedValue = "[Select]"
        Me.ddlCountryCode.Value = "[Select]"
        Me.ddlCountryName.Value = "[Select]"
        Me.ddlCityCode.Value = "[Select]"
        Me.ddlCityName.Value = "[Select]"
        Me.ddlOrderBy.SelectedIndex = 0
        Me.ddlSelectGroupCode.Value = "[Select]"
        Me.ddlSelectGroupName.Value = "[Select]"
        FillGrid("sectormaster.sectorname")
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
        '  FillGrid(Session("strsortexpression"), "")
        FillGridNew()
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
                strSqlQry = "SELECT sectorcode AS [Sector Code],sectorname as [Sector Name],SectorMaster.sectorgroupcode as [Sector Group Code],othtypmast.othtypname as [Sector Group Name],sectormaster.ctrycode as [Country Code],ctrymast.ctryname as [Country Name],sectormaster.citycode as [City Code],citymast.cityname as [City Name],[Active]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,sectormaster.adddate))+ '/'+ Convert(Varchar, Datepart(MM,sectormaster.adddate))+ '/'+ Convert(Varchar, Datepart(YY,sectormaster.adddate)) + ' ' + Convert(Varchar, Datepart(hh,sectormaster.adddate))+ ':' + Convert(Varchar, Datepart(m,sectormaster.adddate))+ ':'+ Convert(Varchar, Datepart(ss,sectormaster.adddate))) as [Date Created],sectormaster.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,sectormaster.moddate))+ '/'+ Convert(Varchar, Datepart(MM,sectormaster.moddate))+ '/'+ Convert(Varchar, Datepart(YY,sectormaster.moddate)) + ' ' + Convert(Varchar, Datepart(hh,sectormaster.moddate))+ ':' + Convert(Varchar, Datepart(m,sectormaster.moddate))+ ':'+ Convert(Varchar, Datepart(ss,sectormaster.moddate))) as [Date Modified]" & _
"FROM ctrymast INNER JOIN sectormaster ON ctrymast.ctrycode = sectormaster.ctrycode INNER JOIN " & _
  "citymast ON sectormaster.citycode = citymast.citycode inner join othtypmast on sectormaster.sectorgroupcode =othtypmast.othtypcode "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY sectorcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY sectorcode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "sectormaster")

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
            ' strpop = "window.open('rptReportNew.aspx?Pageame=Sector&BackPageName=SectorSearch.aspx&SectorCode=" & TxtSectorCode.Text.Trim & "&SectorName=" & TxtSecorName.Text.Trim & "&CtryCode=" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "&CtryName=" & Trim(ddlCountryName.Items(ddlCountryName.SelectedIndex).Text) & "&CityCode=" & Trim(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) & "&CityName=" & Trim(ddlCityName.Items(ddlCityName.SelectedIndex).Text) & "&SGroupCode=" & Trim(ddlSelectGroupCode.Items(ddlSelectGroupCode.SelectedIndex).Text) & "&SGroupName=" & Trim(ddlSelectGroupName.Items(ddlSelectGroupName.SelectedIndex).Text) & "','RepSector','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Sector&BackPageName=SectorSearch.aspx&SectorCode=" & TxtSectorCode.Text.Trim & "&SectorName=" & TxtSecorName.Text.Trim & "&CtryCode=" & Trim(ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text) & "&CtryName=" & Trim(ddlCountryName.Items(ddlCountryName.SelectedIndex).Text) & "&CityCode=" & Trim(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) & "&CityName=" & Trim(ddlCityName.Items(ddlCityName.SelectedIndex).Text) & "&SGroupCode=" & Trim(ddlSelectGroupCode.Items(ddlSelectGroupCode.SelectedIndex).Text) & "&SGroupName=" & Trim(ddlSelectGroupName.Items(ddlSelectGroupName.SelectedIndex).Text) & "');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Sector")
            'Session.Add("BackPageName", "SectorSearch.aspx")

            'If TxtSectorCode.Text.Trim <> "" Then
            '    strReportTitle = "Sector Code : " & TxtSectorCode.Text.Trim
            '    strSelectionFormula = "{sectormaster.sectorcode} LIKE '" & TxtSectorCode.Text.Trim & "*'"
            'End If
            'If TxtSecorName.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Sector Name : " & TxtSecorName.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorname} LIKE '" & TxtSecorName.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Country Name : " & TxtSecorName.Text.Trim
            '        strSelectionFormula = "{sectormaster.sectorname} LIKE '" & TxtSecorName.Text.Trim & "*'"
            '    End If
            'End If

            'If ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country Code : " & ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sectormaster.ctrycode} = '" & ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Country Code: " & ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{sectormaster.ctrycode} = '" & ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'If ddlCountryName.Items(ddlCountryName.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country Name : " & ddlCountryName.Items(ddlCountryName.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & ddlCountryName.Items(ddlCountryName.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Country Name: " & ddlCountryName.Items(ddlCountryName.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{ctrymast.ctryname} = '" & ddlCountryName.Items(ddlCountryName.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'If ddlCityCode.Items(ddlCityCode.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Market Code : " & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sectormaster.citycode} = '" & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "City Code: " & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{sectormaster.citycode} = '" & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'If ddlCityName.Items(ddlCityName.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Market Name : " & ddlCityName.Items(ddlCityName.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {citymast.cityname} = '" & ddlCityName.Items(ddlCityName.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "City Name: " & ddlCityName.Items(ddlCityName.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{citymast.cityname} = '" & ddlCityName.Items(ddlCityName.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.CheckedChanged
        'PnlSectors.Visible = False
        lblCtryCode.Visible = False
        lblCtryName.Visible = False
        lblCityCode.Visible = False
        lblCityName.Visible = False
        ddlCountryCode.Visible = False
        ddlCountryName.Visible = False
        ddlCityCode.Visible = False
        ddlCityName.Visible = False
    End Sub
    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnadsearch.CheckedChanged
        'PnlSectors.Visible = True
        lblCtryCode.Visible = True
        lblCtryName.Visible = True
        lblCityCode.Visible = True
        lblCityName.Visible = True
        ddlCountryCode.Visible = True
        ddlCountryName.Visible = True
        ddlCityCode.Visible = True
        ddlCityName.Visible = True
    End Sub

    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Sector Name")
        ddlOrderBy.Items.Add("Sector Code")
        ddlOrderBy.Items.Add("City Name")
        ddlOrderBy.Items.Add("City Code")
        ddlOrderBy.Items.Add("Country Name")
        ddlOrderBy.Items.Add("Country Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub


    Protected Sub ddlCityCode_ServerChange(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub




    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("sectormaster.sectorname")
            Case 1
                FillGrid("sectormaster.sectorcode")
            Case 2
                FillGrid("citymast.cityname")
            Case 3
                FillGrid("citymast.citycode")
            Case 4
                FillGrid("ctrymast.ctryname")
            Case 5
                FillGrid("ctrymast.ctrycode")
        End Select
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SectorSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFilter_Click(sender As Object, e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' FilterGrid
    ''' </summary>
    ''' <remarks></remarks>
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
                Case "CITY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CITY", lsProcessCity, "CITY")
                Case "COUNTRY"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCountry, "COUNTRY")
                Case "SECTORGROUP"
                    lsProcessGroup = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SECTORGROUP", lsProcessGroup, "CG")
                Case "SECTOR"
                    lsProcessSector = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SECTOR", lsProcessSector, "SECTOR")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
                    'If lsProcessAll.Trim = """" Then
                    '    lsProcessAll = ""
                    'End If
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew()

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="lsName"></param>
    ''' <param name="lsValue"></param>
    ''' <param name="lsShortCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' lbClose_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper Then
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
    ''' <summary>
    ''' FillGridNew
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strSectorValue As String = ""
        Dim strSectorGroupValue As String = ""
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
                    If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                        If strSectorValue <> "" Then
                            strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SECTORGROUP" Then
                        If strSectorGroupValue <> "" Then
                            strSectorGroupValue = strSectorGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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

            strBindCondition = BuildConditionNew(strCountryValue, strCityValue, strSectorValue, strSectorGroupValue, strTextValue)

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


            strSqlQry = "SELECT   ROW_NUMBER() OVER ( " & " ORDER BY " & strorderby & " " & strsortorder & ")AS RowNumber,sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster INNER JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON sectormaster.citycode = citymast.citycode INNER JOIN othtypmast ON sectormaster.sectorgroupcode  = othtypmast.othtypcode"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBindCondition
            Else
                strSqlQry = strSqlQry
            End If

            '  strValue = Trim(BuildCondition())
            'If strBindCondition <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & strBindCondition & " "
            'End If

            'If strBindCondition <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(myDS)

            Session("SSqlQuery") = strSqlQry
            myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)

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
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    ''' <summary>
    ''' BuildConditionNew
    ''' </summary>
    ''' <param name="strCountryValue"></param>
    ''' <param name="strCityValue"></param>
    ''' <param name="strSectorValue"></param>
    ''' <param name="strSectorGroupValue"></param>
    ''' <param name="strTextValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BuildConditionNew(ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strSectorValue As String, ByVal strSectorGroupValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""

        If strSectorValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            End If
        End If

        If strSectorGroupValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othtypmast.othtypname) IN ( " & Trim(strSectorGroupValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(othtypmast.othtypname) IN ( " & Trim(strSectorGroupValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCountryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If



        If strCityValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(citymast.cityname) IN ( " & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = " (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),sectormaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),sectormaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),sectormaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),sectormaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function

    Protected Sub btnClearDate_Click(sender As Object, e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
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


    <WebMethod()> _
    Public Shared Function GetSectorDetails(pageIndex As Integer) As String
        'Added to similate delay so that we see the loader working
        'Must be removed when moving to production
        System.Threading.Thread.Sleep(1000)
        Dim strSqlQry As String = HttpContext.Current.Session("SSqlQuery")
        Return clsUtils.GetDetailsPageWise(pageIndex, 10, strSqlQry).GetXml()

    End Function

    'Public Shared Function GetCustomersPageWise(pageIndex As Integer, pageSize As Integer) As DataSet
    '    Dim constring As String = ConfigurationManager.ConnectionStrings((HttpContext.Current.Session("dbconnectionName")).ToString).ConnectionString
    '    Using con As New SqlConnection(constring)
    '        Using cmd As New SqlCommand("[GetCustomersPageWise]")
    '            cmd.CommandType = CommandType.StoredProcedure
    '            cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
    '            cmd.Parameters.AddWithValue("@PageSize", pageSize)
    '            cmd.Parameters.Add("@PageCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output
    '            Using sda As New SqlDataAdapter()
    '                cmd.Connection = con
    '                sda.SelectCommand = cmd
    '                Using ds As New DataSet()
    '                    sda.Fill(ds, "Customers")
    '                    Dim dt As New DataTable("PageCount")
    '                    dt.Columns.Add("PageCount")
    '                    dt.Rows.Add()
    '                    dt.Rows(0)(0) = cmd.Parameters("@PageCount").Value
    '                    ds.Tables.Add(dt)
    '                    Return ds
    '                End Using
    '            End Using
    '        End Using
    '    End Using
    'End Function


    'Protected Sub lnkEdit_Click(sender As Object, e As System.EventArgs)
    '    Dim str As String = ""
    '    Dim str1 As String = ""
    '    Dim mylnkButton As LinkButton = CType(sender, LinkButton)
    '    Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
    '    Dim lnkcode As HiddenField = CType(row.FindControl("hdSecCode"), HiddenField)
    '    str = lnkcode.Value
    '    str1 = ""
    'End Sub

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblSectorName As Label = e.Row.FindControl("lblSectorName")
            Dim lblSectorGroupName As Label = e.Row.FindControl("lblSectorGroupName")
            Dim lblCountryName As Label = e.Row.FindControl("lblCountryName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")


            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextSectorGroup As String = ""

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
                        If "SECTOR" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SECTORGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSectorGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCtry
                            lsSearchTextSector = lsSearchTextCtry
                            lsSearchTextSectorGroup = lsSearchTextCtry
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
                        If lsSearchTextSector.Trim <> "" Then
                            lblSectorName.Text = Regex.Replace(lblSectorName.Text.Trim, lsSearchTextSector.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lsSearchTextSectorGroup.Trim <> "" Then
                            lblSectorGroupName.Text = Regex.Replace(lblSectorGroupName.Text.Trim, lsSearchTextSectorGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If
    End Sub
End Class
