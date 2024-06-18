

Partial Class PriceListModule_ImageViewWindow
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            Dim ImageName As String = ""
            Dim Pagename As String = ""

            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If Not (Request.QueryString("code") Is Nothing) = True Then
                ImageName = CType(Request.QueryString("code").Trim(), String)
            End If

            If Not (Request.QueryString("pagename") Is Nothing) = True Then
                Pagename = CType(Request.QueryString("pagename"), String)
            End If

            ShowImage(ImageName, Pagename)
        End If
    End Sub

    Private Sub ShowImage(ByVal ImageName As String, ByVal Pagename As String)
        Try
            If ImageName = "" Or ImageName = Nothing Then
                lblMessage.Visible = True
                img1.Visible = False
                Exit Sub
            End If

            If Pagename = "RoomOnline.aspx" Then
                img1.Src = "../PriceListModule/UploadedImages/Roomimages/" + ImageName
                img1.Visible = True
                lblMessage.Visible = False
            ElseIf Pagename = "UploadPopularDeals.aspx" Or Pagename = "UploadHottestOffers.aspx" Then
                img1.Src = "../WebAdminModule/UploadHomeImage/" + ImageName
                lblMessage.Visible = False
                img1.Visible = True
            ElseIf Pagename = "UserMaster.aspx" Then
                img1.Src = "../UserAdminModule/UploadImage/" + ImageName
                lblMessage.Visible = False
                img1.Visible = True
            Else

                img1.Src = "../PriceListModule/UploadedImages/" + ImageName
                lblMessage.Visible = False
                img1.Visible = True
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImageViewWindow.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

End Class
