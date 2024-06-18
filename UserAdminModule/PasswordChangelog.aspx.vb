

Imports System.Data
Imports System.Data.SqlClient

Partial Class UserAdminModule_PasswordChangelog
    Inherits System.Web.UI.Page


#Region "Enum GridCol"
    'Enum GridCol
    '    slno = 0
    '    agentcode = 1
    '    agentname = 2
    '    ImagePosition = 3
    '    Rankorder = 4
    '    Edit = 5
    '    View = 6
    '    Delete = 7
    'End Enum
#End Region

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim objdatetime As New clsDateTime
#End Region

#Region " Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                SetFocus(gv_SearchResult)


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
                    'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                       CType(strappname, String), "WebAdminModule\UploadImagesForHomePgSearch.aspx", btnAddNew, btnExportToExcel, _
                    '                        btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                Session.Add("strsortExpression", "UserCode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                FillGrid("UserCode")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PasswordChangelog.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        
    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""

        If txtUsercode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " p.usercode like '" & Trim(txtUsercode.Text.Trim) & "%'"
            Else
                strWhereCond = strWhereCond & " AND p.usercode like '" & Trim(txtUsercode.Text.Trim) & "%'"
            End If
        End If

        If txtUserName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " u.UserName LIKE  '" & Trim(txtUserName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND  u.UserName LIKE  '" & Trim(txtUserName.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtUserName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " u.UserName LIKE  '" & Trim(txtUserName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND  u.UserName LIKE  '" & Trim(txtUserName.Text.Trim.ToUpper) & "%'"
            End If
        End If
        'p.adddate(between) '' and ''

        If dpFromDate.txtDate.Text.Trim <> "" And dpToDate.txtDate.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " p.adddate between '" & objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpFromDate.txtDate.Text, String)) & "' and '" & objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpToDate.txtDate.Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND  p.adddate between '" & objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpFromDate.txtDate.Text, String)) & "' and '" & objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpToDate.txtDate.Text, String)) & "'"
            End If
        End If

        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        'lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select p.UserCode,u.UserName  ,p.adddate   from password_change_log p inner join UserMaster u on"

            strSqlQry += " p.UserCode = u.UserCode"


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
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        Catch ex As Exception
            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PasswordChangelog.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        ''Session.Add("State", "New")
        ''Response.Redirect("UploadBannerAdds.aspx", False)
        'Dim strpop As String = ""
        'strpop = "window.open('UploadImagesForHomePg.aspx?State=New','UploadImage','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("UserCode")
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        'Try
        '    If e.CommandName = "Page" Then Exit Sub
        '    If e.CommandName = "Sort" Then Exit Sub
        '    Dim lblId As Label
        '    lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")

        '    If e.CommandName = "EditRow" Then
        '        Dim strpop As String = ""
        '        strpop = "window.open('UploadImagesForHomePg.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','uploadImage','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        '    ElseIf e.CommandName = "View" Then
        '        Dim strpop As String = ""
        '        strpop = "window.open('UploadImagesForHomePg.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','uploadImage','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        '    ElseIf e.CommandName = "DeleteRow" Then
        '        Dim strpop As String = ""
        '        strpop = "window.open('UploadImagesForHomePg.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','uploadImage','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        '    End If
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("UploadImagesForHomePgSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillGrid("UserCode")
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        
        txtUsercode.Text = ""
        txtUserName.Text = ""
        'dpFromDate.DateValue = ""
        'dpToDate.DateValue = ""

        '.Text = ""
        'txtImageName.Text = ""
        FillGrid("SLNO")
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

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PwdChange_log','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


End Class
