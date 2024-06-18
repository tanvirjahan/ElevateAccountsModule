Imports System.Data
Imports System.Data.SqlClient
Partial Class WebAdminModule_OnlineUsers
    Inherits System.Web.UI.Page
    Dim MyUtilities As New clsUtils
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                FillUsers()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Sub FillUsers()
        Try
            ChatUsers.InnerHtml = ""
            Dim MyDataSet As New DataSet
            Dim MyDataTable As New DataTable
            Dim Str As String = String.Empty
            Str = "Select C.*,MessageTime From Chat_Login C  Join (Select Top 1 MessageTime,UserFromId From Chat_Message Order By MessageTime Desc) M On M.UserFromId=C.LoginUserID Where IsActive=1 And LoginTime >='" & Format(CDate(Date.Today), "yyyy/MM/dd") & "' "
            MyDataSet = MyUtilities.GetDataFromDatasetnew(Session("dbconnectionName"), Str)
            If MyDataSet.Tables.Count > 0 Then
                MyDataTable = MyDataSet.Tables(0)
            End If
            If MyDataTable.Rows.Count > 0 Then
                ChatUsers.InnerHtml += "<p>List Of Online Users</p><table class='ChatTableAdd'><thead><tr><th></th><th>User Name</th><th>User Tye</th><th>Login Time</th><th>Login UserIP</th><th>Last Activity</th></tr></thead><tbody>"
                Dim i As Integer = 0
                Dim MyUserName As String = String.Empty
                For i = 0 To MyDataTable.Rows.Count - 1
                    If MyDataTable.Rows(i).Item("UserType").ToString().ToUpper() = "SUBAGENT" Or MyDataTable.Rows(i).Item("UserType").ToString().ToUpper() = "AGENT" Then
                        MyUserName = MyUtilities.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select webusername from agentmast where  agentcode='" & MyDataTable.Rows(i).Item("UserCode") & "'")
                    Else
                        MyUserName = MyDataTable.Rows(i).Item("UserCode")
                    End If
                    If MyDataTable.Rows(i).Item("UserType").ToString().ToUpper() = "SUBAGENT" Then
                        MyUserName = MyUserName & " \ " & MyDataTable.Rows(i).Item("SubUserCode")
                    End If
                    ChatUsers.InnerHtml += "<tr><td><a href='javascript:void(0)' class='LogOffUserClass' LoginUserId='" & MyDataTable.Rows(i).Item("LoginUserId") & "'>LogOff User</a></td><td>" & MyUserName & "</td><td>" & MyDataTable.Rows(i).Item("UserType") & "</td><td>" & Format(CDate(MyDataTable.Rows(i).Item("LoginTime")), "dd/MM/yyyy HH:MM") & "</td><td>" & MyDataTable.Rows(i).Item("LoginUserIp") & "</td><td>" & Format(CDate(MyDataTable.Rows(i).Item("MessageTime")), "dd/MM/yyyy HH:MM") & "</td></tr>"
                Next
                ChatUsers.InnerHtml += "</tbody></table>"
            Else
                ChatUsers.InnerHtml += "<p>No Users Online</p>"
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
