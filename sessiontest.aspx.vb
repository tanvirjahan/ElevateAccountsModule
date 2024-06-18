
Partial Class sessiontest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "onLoad", "DisplaySessionTimeout()", True)

        End If

        

    End Sub
End Class
