Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.IO

Partial Class SubMenuUserControl
    Inherits System.Web.UI.UserControl

    Private Connection As SqlConnection

    Dim objUser As New clsUser

    Private app As String
    Private menuid As String
    Private suppliertype As String
    Private contractid As String
    Private partycode As String
    Private Ratetype As String
    Private Calledfrom As String


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

    Public Property suptype() As String
        Get
            Return suppliertype
        End Get
        Set(ByVal value As String)
            suppliertype = value
        End Set
    End Property

    Public Property contractval() As String
        Get
            Return contractid
        End Get
        Set(ByVal value As String)
            contractid = value
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

    Public Property ratetypeval() As String
        Get
            Return Ratetype
        End Get
        Set(ByVal value As String)
            Ratetype = value
        End Set
    End Property
    Public Property Calledfromval() As String
        Get
            Return Calledfrom
        End Get
        Set(ByVal value As String)
            Calledfrom = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                Dim intCount As Integer = 1
                Dim intAppId As Integer
                Dim intGroupId As Integer
                Dim intMenuID As Long



                intAppId = app

                'nv 21/072016 to pass menuids from the pages instead of taking like below
                If menuid <> "" Then
                    intMenuID = menuid
                Else

                    'intMenuID = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplieragentsSearch.aspx?appid=" + app, String), intAppId)
                    'intMenuID = objUser.GetMenuId(CType("SupplierSearch.aspx", String))
                    ' added to handle multiple sub menus - Christo.A - 29.07.08
                    ' handled in Supplier master - christo. A 24/07/16
                    'If Session("submenuuser") = "SupplierSearch.aspx" Then
                    'intMenuID = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + app, String), intAppId)
                    'Else
                    'If Session("submenuuser") = "CustomersSearch.aspx" Then
                    'intMenuID = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + app, String), intAppId)
                    'Else
                    'intMenuID = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierAgentsSearch.aspx?appid=" + app, String), intAppId)
                    'End If
                End If
                Session("submenuuser") = ""
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = "" Then
                    Response.Redirect("~\Login.aspx", False)
                    Exit Sub
                Else
                    intGroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                End If

                CreateMainMenu(intAppId, intGroupId, intMenuID, suptype, Calledfrom)
                ' SubMenu(1, intCount, intAppId, intGroupId, intMenuID)
                ' MenuStyle()
                For Each item As MenuItem In Menu1.Items
                    Dim strMenuItems() As String
                    Dim strMenuItem As String = ""
                    If Path.GetFileName(item.NavigateUrl) <> "" Then
                        strMenuItems = Path.GetFileName(item.NavigateUrl).Split("?")
                        If strMenuItems(0) = Path.GetFileName(Request.PhysicalPath) Then
                            If Request.Url.ToString.Contains("rmcat=A") Or Request.Url.ToString.Contains("rmcat=S") Then
                                If Request.Url.ToString.Contains("rmcat=A") = True And strMenuItems(1) = "rmcat=A" Then
                                    item.Selected = True
                                    Exit For
                                ElseIf Request.Url.ToString.Contains("rmcat=S") = True And strMenuItems(1) = "rmcat=S" Then
                                    item.Selected = True
                                    Exit For
                                End If
                            Else
                                item.Selected = True
                                Exit For
                            End If


                            '  Exit For
                        End If
                    End If

                    'If Path.GetFileName(item.NavigateUrl).Equals(Path.GetFileName(Request.PhysicalPath), StringComparison.InvariantCultureIgnoreCase) Then

                    'End If
                Next
            End If

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
    Private Sub CreateMainMenu(ByVal intAppId As Integer, ByVal intGroupId As Integer, ByVal intMenuID As Integer, ByVal strsptype As String, ByVal Calledfrom As String)
        'Dim intAppId As Integer

        Dim intCount As Integer = 0

        Dim strMainMenu As String

        Dim daMainMenu As SqlDataAdapter

        Dim dsMainMenu As New DataSet()

        Dim tblMainMenu As DataTable

        Dim drSubMenu As DataRow
        Dim newsttype As String = ""

        Try
            'intAppId = objUser.GetAppId(Session("dbconnectionName"),CType(Session("AppName"), String))

            If (strsptype = "" Or strsptype = "[Select]") Then
                strMainMenu = "SELECT * FROM submenumaster where submenuid in (select submenuid from group_submenurights where appid='" & intAppId & "' and grpid= '" & intGroupId & "' and active=1 and menuid='" & intMenuID & "') and appid='" & intAppId & "' and menuid='" & intMenuID & "'"
            Else ' show the menus based on the service provider type - Christo. A - 25/07/16
                If strsptype = "HOT" Then
                    strMainMenu = "SELECT * FROM submenumaster where submenuid in (select submenuid from group_submenurights where appid='" & intAppId & "' and grpid= '" & intGroupId & "' and active=1 and menuid='" & intMenuID & "') and appid='" & intAppId & "' and menuid='" & intMenuID & "' and submenuid not in (90) "
                ElseIf strsptype <> "HOT" Then
                    strMainMenu = "SELECT * FROM submenumaster where submenuid in (select submenuid from group_submenurights where appid='" & intAppId & "' and grpid= '" & intGroupId & "' and active=1 and menuid='" & intMenuID & "') and appid='" & intAppId & "' and menuid='" & intMenuID & "' and submenuid not in (50,60,61,65,70,80,110,160) "
                End If
            End If
            strMainMenu &= " AND menu_status = '1' ORDER BY menuid"

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
                        If intMenuID = 1030 Then 'Contract menu
                            'childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?appid=" + app + "&partycode=" + partyval + "&contractid=" + contractval)
                            If Calledfrom <> "" Then
                                childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?Calledfrom=" + Calledfrom + "&appid=" + app)
                            Else
                                If strsptype <> "" Then
                                    If strsptype = "Other Serv" Then
                                        childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?type=OTH&appid=" + app)
                                    Else
                                        childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?type=" + strsptype + "&appid=" + app)
                                    End If
                                Else
                                    childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?appid=" + app)
                                End If
                            End If

                        Else
                            If Calledfrom <> "" Then
                                childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?Calledfrom=" + Calledfrom + "&appid=" + app)
                            Else
                                If strsptype <> "" Then
                                    If strsptype = "Other Serv" Then
                                        childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?type=OTH&appid=" + app)
                                    Else
                                        childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?type=" + strsptype + "&appid=" + app)
                                    End If


                                Else
                                    childmenu.NavigateUrl = CStr(drSubMenu("pagename") + "?appid=" + app)
                                End If

                            End If

                        End If

                            'iframeINF.window.navigate(drSubMenu("pagename"))
                        End If
                    Menu1.Items.Add(childmenu)
                    'If IsDBNull(drSubMenu("menuid")) = False Then
                    '    SubMenu(CInt(drSubMenu("menuid")), intCount, intAppId, intGroupId, intMenuID)
                    'End If
                    'intCount += 1
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
    'Private Sub SubMenu(ByVal ParentID As Integer, ByVal intCount As Integer, ByVal intAppId As Integer, ByVal intGroupId As Integer, ByVal intMenuID As Integer)

    '    Dim strSubMenu As String

    '    Dim ReturnSubString As String = ""

    '    Dim daSubMenu As SqlDataAdapter

    '    Dim dsSubMenu As New DataSet()

    '    Dim tblSubMenu As DataTable

    '    Dim drSubMenu As DataRow

    '    Try
    '        strSubMenu = "SELECT * FROM submenumaster where menuid in (select menuid from group_rights where appid='" & intAppId & "' and groupid= '" & intGroupId & "' and active=1) and appid='" & intAppId & "' and menuid ='" & intMenuID & "' "

    '        strSubMenu &= " AND menu_status = '1' ORDER BY menuid"

    '        Connection = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '        daSubMenu = New SqlDataAdapter(strSubMenu, Connection)

    '        daSubMenu.Fill(dsSubMenu, "Main")

    '        If dsSubMenu.Tables(0).Rows.Count > 0 Then

    '            tblSubMenu = dsSubMenu.Tables(0)

    '            For Each drSubMenu In tblSubMenu.Rows
    '                Dim childmenu As New System.Web.UI.WebControls.MenuItem
    '                If IsDBNull(drSubMenu("submenuname")) = False Then
    '                    childmenu.Text = CStr(drSubMenu("submenuname"))
    '                End If

    '                If IsDBNull(drSubMenu("pagename")) = False Then
    '                    childmenu.NavigateUrl = CStr(drSubMenu("pagename"))
    '                End If

    '                Menu1.Items(intCount).ChildItems.Add(childmenu)
    '                intCount += 1
    '            Next
    '            clsDBConnect.dbAdapterClose(daSubMenu)
    '            clsDBConnect.dbDataSetClose(dsSubMenu)
    '        End If
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '    Finally
    '        clsDBConnect.dbConnectionClose(Connection)
    '    End Try
    'End Sub
End Class
