Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Partial Class PriceListMaster
    Inherits System.Web.UI.MasterPage

    Private Connection As SqlConnection
    Dim objUser As New clsUser

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                lblCurrentDate.Text = "Current Date : " & System.DateTime.Now.ToString("dd/MM/yyyy, hh:mm:ss")
                Dim intAppId As Integer
                Dim intGroupId As Integer

                intAppId = 1    'objUser.GetAppId(Session("dbconnectionName"),CType(Session("AppName"), String))
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = "" Then
                    Response.Redirect("/ColumbusRPTS/login.aspx", False)
                    Exit Sub
                Else
                    lblLoggedAs.Text = "Logged As : " & objUser.LoggedAs(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                    intGroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                End If
                CreateMainMenu(intAppId, intGroupId)
                ' MenuStyle()
            End If
            'Me.Page.Title = hdnAppName.Value + " - :: " + CType(Session("CompanyName"), String) + "::"

            title.Text = CType(Session("CompanyName"), String)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
    Private Sub MenuStyle()
        With Menu1
            .Orientation = Orientation.Horizontal
            .BackColor = Black
            .DynamicHorizontalOffset = "2"
            .ForeColor = White
            .StaticSubMenuIndent = "10"
            .Width = "528"
            .StaticSelectedStyle.BackColor = Black
            .StaticMenuItemStyle.HorizontalPadding = "5"
            .StaticMenuItemStyle.VerticalPadding = "2"
            .DynamicHoverStyle.BackColor = Black
            .DynamicHoverStyle.ForeColor = White
            .DynamicMenuStyle.BackColor = Gray
            .DynamicSelectedStyle.BackColor = Red
            .DynamicMenuItemStyle.HorizontalPadding = "5"
            .DynamicMenuItemStyle.VerticalPadding = "2"
            .StaticHoverStyle.BackColor = Black
            .StaticHoverStyle.ForeColor = White
        End With
    End Sub
    'Private Sub CreateMainMenu()
    '    Dim intCount As Integer = 0

    '    Dim strMainMenu As String

    '    Dim daMainMenu As SqlDataAdapter

    '    Dim dsMainMenu As New DataSet()

    '    Dim tblMainMenu As DataTable

    '    Dim drSubMenu As DataRow

    '    Try

    '        strMainMenu = "SELECT MENUID,MenuDescription,URLLINK FROM MenuSetting WHERE PARENTID=0"

    '        strMainMenu &= " AND ACTIVESTATUS = 'A' ORDER BY MENUID"

    '        Connection = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '        daMainMenu = New SqlDataAdapter(strMainMenu, Connection)

    '        daMainMenu.Fill(dsMainMenu, "Main")

    '        If dsMainMenu.Tables(0).Rows.Count > 0 Then

    '            tblMainMenu = dsMainMenu.Tables(0)

    '            For Each drSubMenu In tblMainMenu.Rows

    '                Dim childmenu As New System.Web.UI.WebControls.MenuItem
    '                If IsDBNull(drSubMenu("MenuDescription")) = False Then
    '                    childmenu.Text = CStr(drSubMenu("MenuDescription"))
    '                End If

    '                Menu1.Items.Add(childmenu)
    '                If IsDBNull(drSubMenu("MENUID")) = False Then
    '                    SubMenu(CInt(drSubMenu("MENUID")), intCount)
    '                End If
    '                intCount += 1
    '            Next
    '        End If
    '        clsDBConnect.dbAdapterClose(daMainMenu)
    '        clsDBConnect.dbDataSetClose(dsMainMenu)
    '    Catch ex As Exception

    '    Finally
    '        clsDBConnect.dbConnectionClose(Connection)


    '    End Try

    'End Sub
    'Private Sub SubMenu(ByVal ParentID As Integer, ByVal intCount As Integer)

    '    Dim strSubMenu As String

    '    Dim ReturnSubString As String = ""

    '    Dim daSubMenu As SqlDataAdapter

    '    Dim dsSubMenu As New DataSet()

    '    Dim tblSubMenu As DataTable

    '    Dim drSubMenu As DataRow

    '    Try
    '        strSubMenu = "SELECT MENUID,MenuDescription,PARENTID,URLLINK FROM MenuSetting WHERE PARENTID=" & ParentID

    '        strSubMenu &= " AND ACTIVESTATUS = 'A' ORDER BY MENUID"

    '        Connection = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '        daSubMenu = New SqlDataAdapter(strSubMenu, Connection)

    '        daSubMenu.Fill(dsSubMenu, "Main")

    '        If dsSubMenu.Tables(0).Rows.Count > 0 Then

    '            tblSubMenu = dsSubMenu.Tables(0)

    '            For Each drSubMenu In tblSubMenu.Rows
    '                Dim childmenu As New System.Web.UI.WebControls.MenuItem
    '                If IsDBNull(drSubMenu("MenuDescription")) = False Then
    '                    childmenu.Text = CStr(drSubMenu("MenuDescription"))
    '                End If

    '                If IsDBNull(drSubMenu("URLLINK")) = False Then
    '                    childmenu.NavigateUrl = CStr(drSubMenu("URLLINK"))
    '                End If

    '                Menu1.Items(intCount).ChildItems.Add(childmenu)

    '            Next
    '            clsDBConnect.dbAdapterClose(daSubMenu)
    '            clsDBConnect.dbDataSetClose(dsSubMenu)
    '        End If
    '    Catch ex As Exception

    '    Finally
    '        clsDBConnect.dbConnectionClose(Connection)
    '    End Try
    'End Sub

    Private Sub CreateMainMenu(ByVal intAppId As Integer, ByVal intGroupId As Integer)
        'Dim intAppId As Integer

        Dim intCount As Integer = 0

        Dim strMainMenu As String

        Dim daMainMenu As SqlDataAdapter

        Dim dsMainMenu As New DataSet()

        Dim tblMainMenu As DataTable

        Dim drSubMenu As DataRow

        Try
            'intAppId = objUser.GetAppId(Session("dbconnectionName"),CType(Session("AppName"), String))


            strMainMenu = "SELECT * FROM MenuMaster where menuid in (select menuid from group_rights where appid='" & intAppId & "' and groupid= '" & intGroupId & "' and active=1) and appid='" & intAppId & "' and parentid='0' "

            strMainMenu &= " AND menu_status = '1' ORDER BY menuid"

            Connection = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            daMainMenu = New SqlDataAdapter(strMainMenu, Connection)

            daMainMenu.Fill(dsMainMenu, "Main")

            If dsMainMenu.Tables(0).Rows.Count > 0 Then

                tblMainMenu = dsMainMenu.Tables(0)

                For Each drSubMenu In tblMainMenu.Rows

                    Dim childmenu As New System.Web.UI.WebControls.MenuItem
                    If IsDBNull(drSubMenu("menudesc")) = False Then
                        childmenu.Text = CStr(drSubMenu("menudesc"))
                    End If

                    Menu1.Items.Add(childmenu)
                    If IsDBNull(drSubMenu("menuid")) = False Then
                        SubMenu(CInt(drSubMenu("menuid")), intCount, intAppId, intGroupId)
                    End If
                    intCount += 1
                Next
            End If
            clsDBConnect.dbAdapterClose(daMainMenu)
            clsDBConnect.dbDataSetClose(dsMainMenu)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            clsDBConnect.dbConnectionClose(Connection)
        End Try

    End Sub
    Private Sub SubMenu(ByVal ParentID As Integer, ByVal intCount As Integer, ByVal intAppId As Integer, ByVal intGroupId As Integer)

        Dim strSubMenu As String

        Dim ReturnSubString As String = ""

        Dim daSubMenu As SqlDataAdapter

        Dim dsSubMenu As New DataSet()

        Dim tblSubMenu As DataTable

        Dim drSubMenu As DataRow

        Try
            strSubMenu = "SELECT * FROM MenuMaster where menuid in (select menuid from group_rights where appid='" & intAppId & "' and groupid= '" & intGroupId & "' and active=1) and appid='" & intAppId & "' and parentid=" & ParentID

            strSubMenu &= " AND menu_status = '1' ORDER BY menuid"

            Connection = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            daSubMenu = New SqlDataAdapter(strSubMenu, Connection)

            daSubMenu.Fill(dsSubMenu, "Main")

            If dsSubMenu.Tables(0).Rows.Count > 0 Then

                tblSubMenu = dsSubMenu.Tables(0)

                For Each drSubMenu In tblSubMenu.Rows
                    Dim childmenu As New System.Web.UI.WebControls.MenuItem
                    If IsDBNull(drSubMenu("menudesc")) = False Then
                        childmenu.Text = CStr(drSubMenu("menudesc"))
                    End If

                    If IsDBNull(drSubMenu("pagename")) = False Then
                        childmenu.NavigateUrl = CStr(drSubMenu("pagename"))
                        If drSubMenu("pagename").ToString.Contains("openwindow=new") Then
                            childmenu.Target = "_blank"
                        End If
                    End If

                    Menu1.Items(intCount).ChildItems.Add(childmenu)

                Next
                clsDBConnect.dbAdapterClose(daSubMenu)
                clsDBConnect.dbDataSetClose(dsSubMenu)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            clsDBConnect.dbConnectionClose(Connection)
        End Try
    End Sub
End Class


