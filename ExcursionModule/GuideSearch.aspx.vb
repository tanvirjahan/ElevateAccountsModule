﻿Imports System.Data
Imports System.Data.SqlClient

Partial Class ExcursionModule_GuideSearch
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
        GuideCodeTCol = 0
        GuideCode = 1
        GuideName = 2
        Tel = 3
        Mobile = 4
        Active = 5
        DateCreated = 6
        UserCreated = 7
        DateModified = 8
        UserModified = 9
        Edit = 10
        View = 11
        Delete = 12

    End Enum
#End Region


    'Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    '    If Page.IsPostBack = False Then
    '        If Request.QueryString("appid") Is Nothing = False Then
    '            Dim appid As String = CType(Request.QueryString("appid"), String)
    '            Select Case appid
    '                Case 1
    '                    Me.MasterPageFile = "~/PriceListMaster.master"
    '                Case 2
    '                    Me.MasterPageFile = "~/RoomBlock.master"
    '                Case 3
    '                    Me.MasterPageFile = "~/ReservationMaster.master"
    '                Case 4
    '                    Me.MasterPageFile = "~/AccountsMaster.master"
    '                Case 5
    '                    Me.MasterPageFile = "~/UserAdminMaster.master"
    '                Case 6
    '                    Me.MasterPageFile = "~/WebAdminMaster.master"
    '                Case 7
    '                    Me.MasterPageFile = "~/TransferHistoryMaster.master"
    '                Case Else
    '                    Me.MasterPageFile = "~/SubPageMaster.master"
    '            End Select
    '        Else
    '            Me.MasterPageFile = "~/SubPageMaster.master"
    '        End If
    '    End If
    'End Sub

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Page.Header.Title = "General"
            Try
                SetFocus(txtGuideCode)

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

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "ExcursionModule\GuideSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                Session.Add("strsortExpression", "guidecode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                fillorderby()
                FillGrid("guidecode")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("GuideSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "GuideWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub
#End Region

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtGuideCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(guide_master.guidecode) LIKE '" & Trim(txtGuideCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(guide_master.guidecode) LIKE '" & Trim(txtGuideCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtGuideName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(guide_master.guidename) LIKE '" & Trim(txtGuideName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(guide_master.guidename) LIKE '" & Trim(txtGuideName.Text.Trim.ToUpper) & "%'"
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
            strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive' end FROM guide_master"

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
            objUtils.WritErrorLog("GuideSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("Markets.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('Guides.aspx?State=New','Guides','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('Guides.aspx?State=New','Guides');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("plgrpcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("guidecode")
            Case 1
                FillGrid("guidename")

        End Select
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Markets.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('Guides.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Guides','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Guides.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Guides');"


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Markets.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('Guides.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Guides','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Guides.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Guides');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Markets.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('Guides.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Guides','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Guides.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Guides');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("GuideSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("plgrpcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("guidecode")
            Case 1
                FillGrid("guidename")

        End Select
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtGuideCode.Text = ""
        txtGuideName.Text = ""
        Me.ddlOrderBy.SelectedIndex = 0
        FillGrid("guidecode")
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
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

#Region " Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
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
                strSqlQry = "SELECT guidecode AS [Guide Code],guidename AS [Guide Name],guideaddress AS [Guide Address],tel AS [Telephone],mobile AS [Mobile],[Active]=case when active=1 then 'Active' when active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created],moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified] FROM guide_master"

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY guidecode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY guidecode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "guide_master")

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
            'strpop = "window.open('rptReportNew.aspx?Pageame=Guide&BackPageName=GuideSearch.aspx&GuideCode=" & txtGuideCode.Text.Trim & "&GuideName=" & txtGuideName.Text.Trim & "','RepGuide','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Guide&BackPageName=GuideSearch.aspx&GuideCode=" & txtGuideCode.Text.Trim & "&GuideName=" & txtGuideName.Text.Trim & "','RepGuide');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("GuideSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Guide Code")
        ddlOrderBy.Items.Add("Guide Name")

        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("guidecode")
            Case 1
                FillGrid("guidecode")

        End Select
    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=GuideSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class