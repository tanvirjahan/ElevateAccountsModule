Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class ModuleMainPage
    Inherits System.Web.UI.Page
#Region " Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim myReader As SqlDataReader
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = ":: " + CType(Session("CompanyName"), String) + " :: "
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Page.IsPostBack = False Then
            lblCurrentDate.Text = "Current Date : " & System.DateTime.Now.ToString("dd/MM/yyyy, hh:mm:ss")
            If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = "" Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            Else
                lblLoggedAs.Text = "Logged As : " & objUser.LoggedAs(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))

                'myReader = objUser.GetAppName(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String)) '***changed by mohamed on 01/04/2018
                myReader = objUser.GetAppDisplayName(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                If myReader.HasRows Then
                    While myReader.Read
                        CreateButton(myReader(0))
                    End While
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ModuleMainPage.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

    Private Sub CreateButton(ByVal btnTeaxt As String)
        Dim tr As TableRow = New TableRow()
        Dim tc As TableCell = New TableCell()
        Dim btn As Button = New Button
        btn.Text = btnTeaxt
        Dim htmlColor As String = "#06788B"
        Dim myColor As Drawing.Color = System.Drawing.ColorTranslator.FromHtml(htmlColor)
        btn.BackColor = myColor
        btn.ForeColor = White
        btn.Width = 280
        tc.Controls.Add(btn)
        AddHandler btn.Click, AddressOf Button1_Click
        btn.OnClientClick = "SetTarget();"
        tr.Cells.Add(tc)
        Table1.Rows.Add(tr)
    End Sub

    Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As Button
        btn = sender

        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim lAppName As String
        lAppName = objUtils.GetString(Session("dbConnectionName"), "select appname from appmaster where DisplayName='" & btn.Text.Trim.ToString & "'")
        Session.Add("AppName", lAppName) 'Session.Add("AppName", btn.Text.Trim.ToString) '***changed by mohamed on 01/04/2018

        
        Session.Add("DAppName", btn.Text.Trim.ToString) 'Session.Add("AppName", btn.Text.Trim.ToString) '*** Danny 04/04/2018
        'Response.Redirect("MainPage.aspx", False)
        Dim divDes As String
        divDes = objUtils.GetString(Session("dbConnectionName"), "select division_master_des from division_master where accountsmodulename='" & Session("DAppName") & "'")
        Session.Add("divDes", divDes) 'Session.Add("AppName", btn.Text.Trim.ToString) '***changed by mohamed on 01/04/2018

        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        Dim intAppId As Integer
        intAppId = objUser.GetAppId(Session("dbconnectionName"), CType(Session("AppName"), String))
        Dim strpop As String = ""
        If intAppId <> 2 Then '--intAppId <> 15 --changed by shahul on 19/08/2018
            Session.Add("sAppId", intAppId.ToString)
            'strpop = "window.open('MainPage.aspx?appid=" & intAppId & "','" & Replace(Session("AppName"), " ", "") & "','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('MainPage.aspx?appid=" & intAppId & "','" & Replace(Session("AppName"), " ", "") & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        ElseIf intAppId = 2 Then
            Dim AppIdNew? As Integer
            Dim AppPageName As String = ""
            objUser.GetAppDetails(Session("dbconnectionName"), CType(Session("AppName"), String), AppIdNew, AppPageName)
            If Not IsNothing(AppIdNew) And AppPageName <> "" Then
                strpop = "window.open('" & AppPageName & "?appid=" & AppIdNew & "','" & Replace(Session("AppName"), " ", "") & "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        End If

    End Sub


End Class
