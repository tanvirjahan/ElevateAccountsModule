
Partial Class WebAdminModule_ChatDetails
    Inherits System.Web.UI.Page
    Dim objUtils As New clsUtils
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUserName, "username", "usercode", "select usercode,username  from UserMaster where active=1 order by username", True)
            End If

        Catch ex As Exception

        End Try

    End Sub
End Class
