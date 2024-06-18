Imports System.Data
Partial Class SubPageMaster
    Inherits System.Web.UI.MasterPage
    Dim objUser As New clsUser

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("CompanyName") Is Nothing Then
            'Me.Page.Title = ":: " + CType(Session("CompanyName"), String) + " :: "
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Page.IsPostBack = False Then
                lblCurrentDate.Text = "Current Date : " & System.DateTime.Now.ToString("dd/MM/yyyy, hh:mm:ss")
                lblLoggedAs.Text = "Logged As : " & objUser.LoggedAs(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Else
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
  
End Class

