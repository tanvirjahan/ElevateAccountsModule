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
Partial Class CountryGroupSearch
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
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
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

                Session("SAppId") = strappid
                Session("SAppName") = strappname


                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                         CType(strappname, String), "PriceListModule\CountryGroupSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                    '                                   btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                            CType(strappname, String), "PriceListModule\CountryGroupSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                      btnPrint, gv_SearchResult)
                End If

     
                Session.Add("strsortExpression", "ct.ctryname")
                Session.Add("strsortdirection", SortDirection.Ascending)

                fillorderby()
                FillGrid("ct.ctryname")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CntryGrpWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
       

      
        'If ddlCountryCode.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " citymast.ctrycode = '" & Trim(ddlCountryCode.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND citymast.ctrycode = '" & Trim(ddlCountryCode.SelectedValue) & "'"
        '    End If
        'End If
        If txtcountryname.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(ct.ctryname) LIKE '" & Trim(txtcountryname.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(ct.ctryname) LIKE '" & Trim(txtcountryname.Text.Trim.ToUpper) & "%'"
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



#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        Dim strpop As String = ""
        
        strpop = "window.open('CountryGroup.aspx?State=New','Country');"

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        FillGrid("ct.ctryname")

    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCityCode")

            'If e.CommandName = "EditRow" Then
            '    'Session.Add("State", "Edit")
            '    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
            '    'Response.Redirect("Cities.aspx", False)
            '    Dim strpop As String = ""
            '    'strpop = "window.open('Cities.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    strpop = "window.open('Cities.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "');"

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'ElseIf e.CommandName = "View" Then
            '    'Session.Add("State", "View")
            '    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
            '    'Response.Redirect("Cities.aspx", False)
            '    Dim strpop As String = ""
            '    'strpop = "window.open('Cities.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    strpop = "window.open('Cities.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "');"
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'ElseIf e.CommandName = "DeleteRow" Then
            '    'Session.Add("State", "Delete")
            '    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
            '    'Response.Redirect("Cities.aspx", False)
            '    Dim strpop As String = ""
            '    'strpop = "window.open('Cities.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    strpop = "window.open('Cities.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities');"
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountryGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        FillGrid("ct.ctryname")

    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtCountryName.Text = ""
        FillGrid("ct.ctryname")
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
        Dim strsortorder As String = "ASC"
        Dim strorderby As String = "ct.ctryname"

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

                'strSqlQry = "SELECT  citymast.citycode AS [City Code] , citymast.cityname AS [City Name], citymast.ctrycode as [Country Code], citymast.rankorder as [Rank Order], citymast.showweb as [Show In Web],citymast.active as [Active],(Convert(Varchar, Datepart(DD,citymast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,citymast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,citymast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,citymast.adddate))+ ':' + Convert(Varchar, Datepart(m,citymast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,citymast.adddate))) as [Date Created] , citymast.adduser as [User Created], (Convert(Varchar, Datepart(DD,citymast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,citymast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,citymast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,citymast.moddate))+ ':' + Convert(Varchar, Datepart(m,citymast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,citymast.moddate))) as [Date Modified], citymast.moduser as [User Modified]  FROM citymast inner join ctrymast on citymast.ctrycode=ctrymast.ctrycode"


                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY citycode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY citycode"
                'End If

                strSqlQry = "select ct.ctrycode 'Country Code' , ct.ctryname Country,pl.plgrpname Region,isnull(dbo.fn_get_countrygroup(ct.ctrycode),'') 'Country Groups'" & _
                     " from ctrymast ct, plgrpmast pl  where(ct.plgrpcode = pl.plgrpcode) and ct.active=1 and pl.active=1"


                If txtCountryName.Text.Trim <> "" Then

                    If txtCountryName.Text.Contains("(R)") Then

                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(pl.plgrpname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                        Else

                            strWhereCond = strWhereCond & " AND upper(pl.plgrpname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                        End If

                    ElseIf txtCountryName.Text.Contains("(G)") Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "  ct.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname='" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "')"
                        Else
                            strWhereCond = strWhereCond & " and ct.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname='" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "')"
                        End If

                    Else
                        If Trim(strWhereCond) = "" Then

                            strWhereCond = " upper(ct.ctryname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                        Else
                            strWhereCond = strWhereCond & " AND upper(ct.ctryname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                        End If
                    End If
                End If

                If Trim(strWhereCond) <> "" Then
                    strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
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

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountryGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '   pnlSearch.Visible = False

    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub fillorderby()

    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CitiesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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

            strSqlQry = "select Name from view_countryregiongroup where name like  " & "'" & prefixText & "%' order by name"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    counTryNames.Add(myDS.Tables(0).Rows(i)("name").ToString())
                Next

            End If

            Return counTryNames
        Catch ex As Exception
            Return counTryNames
        End Try

    End Function


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

            strSqlQry = "select ct.ctrycode , ct.ctryname country,pl.plgrpname region,isnull(dbo.fn_get_countrygroup(ct.ctrycode),'') countrygroups" & _
                        " from ctrymast ct, plgrpmast pl  where(ct.plgrpcode = pl.plgrpcode) and ct.active=1 and pl.active=1"


            If txtCountryName.Text.Trim <> "" Then

                If txtCountryName.Text.Contains("(R)") Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(pl.plgrpname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND upper(pl.plgrpname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                    End If
               
                ElseIf txtCountryName.Text.Contains("(G)") Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = "  ct.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname='" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "')"
                    Else
                        strWhereCond = strWhereCond & " and ct.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname='" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "')"
                    End If
            
                Else
                    If Trim(strWhereCond) = "" Then

                        strWhereCond = " upper(ct.ctryname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(ct.ctryname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
                    End If
                End If
            End If

            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            'If txtcountryname.Text.Trim <> "" Then
            '    If Trim(strWhereCond) = "" Then

            '        strWhereCond = " upper(ct.ctryname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
            '    Else
            '        strWhereCond = strWhereCond & " AND upper(ct.ctryname) LIKE '" & Trim(txtcountryname.Text.Trim.ToUpper) & "%'"
            '    End If
            'End If

            'If Trim(strWhereCond) <> "" Then
            '    strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If




            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gv_SearchResult.DataBind()
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
            objUtils.WritErrorLog("CountryGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try

    End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelectAll_CheckedChanged(sender As Object, e As System.EventArgs)
        'Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSelectAll"), CheckBox)
        'Dim row As GridViewRow
        'For Each row In gv_SearchResult.Rows
        '    Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
        '    If ChkBoxHeader.Checked = True Then
        '        ChkBoxRows.Checked = True
        '    Else
        '        ChkBoxRows.Checked = False
        '    End If

        'Next
    End Sub




    Protected Sub txtCountryName_TextChanged(sender As Object, e As System.EventArgs) Handles txtCountryName.TextChanged
        FillGrid("ct.ctryname")
    End Sub
End Class