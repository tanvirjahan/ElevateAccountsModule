Imports System.Data
Imports system.data.SqlClient
Partial Class Help
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils
    Dim mySqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                If Request.QueryString("hi") <> Nothing Then
                    GetHelpData(Request.QueryString("hi"))
                End If
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("Help.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Private Sub GetHelpData(ByVal strHelpId As String)
        Try
            mySqlConn = clsDBConnect.dbConnection               'connection open
            mySqlCmd = New SqlCommand("select help_text from help_text where help_id='" & strHelpId & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.Read Then
                If IsDBNull(mySqlReader("help_text")) = False Then
                    TD1.InnerHtml = mySqlReader("help_text")
                End If
            End If
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
        Catch ex As Exception
            objUtils.WritErrorLog("Help.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class
