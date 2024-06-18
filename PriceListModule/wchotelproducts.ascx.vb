Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Partial Class wchotelproducts
    Inherits System.Web.UI.UserControl


    Private Connection As SqlConnection

    Dim objUser As New clsUser

    Private app As String
    Private menuid As String
    Private menuselected As String
    Private partycode As String
    Public Property appval() As String
        Get
            Return app
        End Get
        Set(ByVal value As String)
            app = value
        End Set
    End Property
    Public Property menuidval() As String
        Get
            Return menuid
        End Get
        Set(ByVal value As String)
            menuid = value
        End Set
    End Property
    Public Property partyval() As String
        Get
            Return partycode
        End Get
        Set(ByVal value As String)
            partycode = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try


                'For i = 0 To 2
                '    Dim childmenu As New System.Web.UI.WebControls.MenuItem
                '    If i = 0 Then
                '        childmenu.Text = "Hotel"
                '        childmenu.NavigateUrl = ""
                '    ElseIf i = 1 Then
                '        childmenu.Text = "Contracts"
                '        childmenu.NavigateUrl = ""
                '    Else
                '        childmenu.Text = "Offers"
                '        childmenu.NavigateUrl = ""
                '    End If
                '    Menu1.Items.Add(childmenu)
                'Next

                Dim intCount As Integer = 1
                Dim intAppId As Integer
                Dim intGroupId As Integer
                Dim intMenuID As Long



                intAppId = app

                'nv 21/072016 to pass menuids from the pages instead of taking like below
                If menuid <> "" Then
                    intMenuID = menuid
                Else


                End If
                Session("submenuuser") = ""
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = "" Then
                    Response.Redirect("~\Login.aspx", False)
                    Exit Sub
                Else
                    intGroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                End If

                ' CreateMainMenu(1, 1, 1100)
                CreateMainMenu(intAppId, intGroupId, intMenuID)

                'Menu1.DynamicHoverStyle.BackColor = Black
                'Menu1.DynamicHoverStyle.ForeColor = White
                'Menu1.DynamicMenuStyle.BackColor = Gray
                'Menu1.DynamicSelectedStyle.BackColor = Red
                'Menu1.StaticHoverStyle.BackColor = Black
                'Menu1.StaticHoverStyle.ForeColor = White
                ' MenuStyle()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try
        End If
    End Sub
    Private Sub CreateMainMenu(ByVal intAppId As Integer, ByVal intGroupId As Integer, ByVal intMenuID As Integer)
        'Dim intAppId As Integer

        Dim intCount As Integer = 0
        ' Dim app As String = "1"

        Dim strMainMenu As String

        Dim daMainMenu As SqlDataAdapter

        Dim dsMainMenu As New DataSet()

        Dim tblMainMenu As DataTable

        Dim drSubMenu As DataRow

        Try
            'intAppId = objUser.GetAppId(Session("dbconnectionName"),CType(Session("AppName"), String))

            'strMainMenu = " select  h.menuid,h.submenuname,h.submenuid,h.pagename from submenumaster h,group_submenurights d where(h.submenuid = d.submenuid And h.menuid = d.menuid and h.active=1) and h.appid=d.appid  and h.menu_status=1  and d.active=1 and h.menuid='" & intMenuID & "' and h.appid='" & intAppId & "' order by h.menuid"
            If intAppId = 1 Then
                strMainMenu = "select s.submenuid,s.submenuname,s.menuid,s.pagename from submenumaster s where s.submenuid in (select g.submenuid from group_submenurights g where g.active=1 and g.menuid='" & intMenuID & "' and g.grpid='" & intGroupId & "' and g.appid='" & intAppId & "' ) and s.menuid='" & intMenuID & "' and s.appid='" & intAppId & "' "
            ElseIf intAppId = 2 Then
                strMainMenu = "select s.submenuid,s.submenuname,s.menuid,s.pagename from submenumaster s where s.submenuid in (select R.menuid from group_rights R inner join MenuMaster M on M.appid= R.appid and m.menuid=r.menuid and R.active=1 and R.groupid='" & intGroupId & "' and M.appid='" & intAppId & "' and parentid='" & intMenuID & "') and s.menuid='" & intMenuID & "' and s.appid='" & intAppId & "' "
                strMainMenu &= " AND menu_status = '1' ORDER BY menuid"
            Else
                strMainMenu = "SELECT * FROM submenumaster where  appid='" & intAppId & "'  and menuid='" & intMenuID & "' and appid='" & intAppId & "'"
                strMainMenu &= " AND menu_status = '1' ORDER BY menuid"

            End If

            'strMainMenu = "SELECT  h.menuid,h.submenuname,h.submenuid,h.pagename  FROM submenumaster h join group_submenurights g where  appid='" & intAppId & "'  and menuid='" & intMenuID & "' and grpid='" & intGroupId & "'"

            'strMainMenu &= " AND menu_status = '1' ORDER BY menuid"

            Connection = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            daMainMenu = New SqlDataAdapter(strMainMenu, Connection)

            daMainMenu.Fill(dsMainMenu, "Main")

            If dsMainMenu.Tables(0).Rows.Count > 0 Then

                tblMainMenu = dsMainMenu.Tables(0)

                For Each drSubMenu In tblMainMenu.Rows

                    Dim childmenu As New System.Web.UI.WebControls.MenuItem
                    If IsDBNull(drSubMenu("submenuname")) = False Then
                        childmenu.Text = CStr(drSubMenu("submenuname"))
                    End If
                    If IsDBNull(drSubMenu("pagename")) = False Then

                        '  childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?appid=" + app)
                        ' If intMenuID = 1030 Then 'Contract menu
                        childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?appid=" + app + "&partycode=" + partyval)
                        'Else
                        '    childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?appid=" + app)
                        'End If


                        'iframeINF.window.navigate(drSubMenu("pagename"))
                    End If
                    Menu1.Items.Add(childmenu)

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
    Private Sub MenuStyle()
        With Menu1
            .Orientation = Orientation.Horizontal
            .BackColor = White
            .DynamicHorizontalOffset = "2"
            .ForeColor = White
            .StaticSubMenuIndent = "10"
            '.Width = "250"



            '    .StaticSelectedStyle.BackColor = Black
            '.StaticMenuItemStyle.HorizontalPadding = "5"
            '.StaticMenuItemStyle.VerticalPadding = "2"
            '.DynamicHoverStyle.BackColor = Black
            '.DynamicHoverStyle.ForeColor = White
            '.DynamicMenuStyle.BackColor = Gray
            '.DynamicSelectedStyle.BackColor = Red
            '.DynamicMenuItemStyle.HorizontalPadding = "5"
            '.DynamicMenuItemStyle.VerticalPadding = "2"
            '.StaticHoverStyle.BackColor = Black
            '.StaticHoverStyle.ForeColor = White

        End With

        Menu1.StaticMenuItemStyle.CssClass = "btn"

    End Sub
    Public Sub menuitemcolor()
        '  Menu1.StaticSelectedStyle.CssClass = "btnnew"
        'Menu1.StaticMenuItemStyle.BackColor = Green
    End Sub


    Public Sub SelectMenuByValue()
        Dim iMenuCount As Integer = Menu1.Items.Count - 1
        For i As Integer = 0 To iMenuCount
            Dim menuItem As MenuItem = Menu1.Items(i)
            If menuItem.Value = 1 Then
                If menuItem.Enabled AndAlso menuItem.Selectable Then menuItem.Selected = True
                Exit For
            End If
            If CheckSelectSubMenu(menuItem, 1) Then Exit For
        Next
    End Sub
    Private Function CheckSelectSubMenu(ByVal menuItem As MenuItem, ByVal sValue As String) As Boolean
        CheckSelectSubMenu = False
        Dim iMenuCount As Integer = menuItem.ChildItems.Count - 1
        For i As Integer = 0 To iMenuCount
            Dim subMenuItem As MenuItem = menuItem.ChildItems(i)
            If subMenuItem.Value = sValue Then
                CheckSelectSubMenu = True
                If subMenuItem.Enabled AndAlso subMenuItem.Selectable Then subMenuItem.Selected = True
                Exit For
            End If
            If CheckSelectSubMenu(subMenuItem, sValue) Then
                CheckSelectSubMenu = True
                Exit For
            End If
        Next
    End Function
End Class
