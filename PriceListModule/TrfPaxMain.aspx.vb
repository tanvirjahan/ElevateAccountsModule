#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_TrfPaxMain
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    'Dim objUser As New clsUser
    'Dim strSqlQry As String
    'Dim strWhereCond As String
    'Dim SqlConn As SqlConnection
    'Dim myCommand As SqlCommand
    'Dim myDataAdapter As SqlDataAdapter
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try

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

                End If

                Session("PaxSlabFilter") = Request.Params("Type")
                'Response.Redirect("TrfPaxSlab.aspx?Type=")

                Dim strpop As String = ""
                strpop = "window.open('TrfPaxSlab.aspx?Type=" + Session("PaxSlabFilter") + "','TransfersPaxSlab','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

              

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TrfPaxMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
           


        Else
            Try
              
                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "TrfPaxSlabWindowPostBack") Then

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TrfPaxMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If



    End Sub
End Class
