Imports System.Data
Imports System.Data.SqlClient
Partial Class WebAdminModule_ListChatDetail
    Inherits System.Web.UI.Page
    Dim MyUtilities As New clsUtils
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            If Not IsPostBack Then
                FillChatDetail()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Sub FillChatDetail()
        Try
            Dim FromDate As String = Request.QueryString("FromDate")
            Dim ToDate As String = Request.QueryString("ToDate")
            Dim SalesPerson As String = Request.QueryString("SalesPerson")
            Dim AgentOrSub As String = Request.QueryString("AgentOrSub")
            Dim ChatUser As String = Request.QueryString("ChatUser")
            Dim Message As String = Server.UrlDecode(Request.QueryString("Message"))
            Dim MyResult As DataRow()
            ChatListDetail.InnerHtml = ""
            Dim MyDataSet As New DataSet
            Dim MyDataTable As New DataTable
            Dim MyDataTableSecond As New DataTable
            Dim Str As String = String.Empty
            Str = "Exec Sp_ViewChatDetail '" & Format(CDate(FromDate), "yyyy/MM/dd") & "','" & Format(CDate(ToDate), "yyyy/MM/dd") & "','" & SalesPerson & "','" & AgentOrSub & "','" & ChatUser & "','" & Message & "' "
            MyDataSet = MyUtilities.GetDataFromDatasetnew(Session("dbconnectionName"), Str)

            If MyDataSet.Tables.Count > 0 Then
                MyDataTable = MyDataSet.Tables(0)
            End If
            If MyDataSet.Tables.Count > 1 Then
                MyDataTableSecond = MyDataSet.Tables(1)
            End If
            If MyDataTable.Rows.Count > 0 Then
                Dim i As Integer = 0
                ChatListDetail.InnerHtml += "<table class='ChatTableAdd'><thead><tr><td>Sales Person</td><td>Connected Person</td><td>User Type</td><td>User</td><td>Start-End</td><td>Duration(Min)</td></tr></thead><tbody>"
                For i = 0 To MyDataTable.Rows.Count - 1
                    MyResult = MyDataTableSecond.[Select]("UserFromId = '" & Trim(MyDataTable.Rows(i).Item("LoginUserId")) & "' Or UserToId = '" & Trim(MyDataTable.Rows(i).Item("LoginUserId")) & "' ", "MessageTime")
                    If MyResult.Length > 0 Then
                        Dim MyUsername As String = String.Empty
                        If MyDataTable.Rows(i).Item("UserType").ToString().ToUpper() = "SUBAGENT" Then
                            MyUsername = MyDataTable.Rows(i).Item("UserCode") & " \ " & MyDataTable.Rows(i).Item("SubUserCode")
                        Else
                            MyUsername = MyDataTable.Rows(i).Item("UserCode")
                        End If
                        ChatListDetail.InnerHtml += "<tr><td>" & MyDataTable.Rows(i).Item("SpersonCode") & "</td><td>" & MyDataTable.Rows(i).Item("Connected") & "</td><td>" & MyDataTable.Rows(i).Item("UserType") & "</td><td>" & MyUsername & "</td><td>" & Format(CDate(MyDataTable.Rows(i).Item("LoginTime")), "dd/MM/yyyy HH:mm:ss") & " - " & Format(CDate(MyDataTable.Rows(i).Item("LogoutTime")), "dd/MM/yyyy HH:mm:ss") & "</td><td>" & MyDataTable.Rows(i).Item("Duration") & "</td></tr>"
                        ChatListDetail.InnerHtml += "<tr><td colspan='6'><table style='width:100%;border-collapse:collapse;border:2px solid #000;'><thead><tr><th>Date</th><th>Sender</th><th>Message</th></tr></thead><tbody>"
                        For Each MyRow As DataRow In MyResult
                            ChatListDetail.InnerHtml += "<tr><td style='border:2px solid #000;'>" & Format(CDate(MyRow("MessageTime")), "dd/MM/yyyy HH:mm:ss") & "</td><td style='border:2px solid #000;'>" & MyRow("UserCode") & "</td><td style='border:2px solid #000;'>" & MyRow("ChatMessage") & "</td></tr>"
                        Next
                        ChatListDetail.InnerHtml += "</tbody></table></td></tr>"
                    End If
                Next
                ChatListDetail.InnerHtml += "</tbody></table>"
            End If


        Catch ex As Exception

        End Try
    End Sub

End Class
