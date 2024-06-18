Imports System.Data
Imports System.Data.SqlClient

Partial Class AccountsModule_RelatedDocuments
    Inherits System.Web.UI.Page
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Dim Str As String = "Insert Into related_docs(docName,docPath,tranId,tranType,divId) Values ('" & docName & "','" & docPath & "','" & tranId & "','" & tranType & "','" & divId & "')"
        'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        'Dim cmd As SqlCommand
        'cmd = New SqlCommand(Str, SqlConn)
        'cmd.ExecuteNonQuery()
        'cmd.Cancel()
        'SqlConn.Close()

        FillGrid()
    End Sub

    Private Sub FillGrid()
        Dim myDS As New DataSet
        Dim strSqlQry As String = "SELECT docId,docName FROM related_docs"

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS)
        
        gv_Docs.DataSource = myDS
        gv_Docs.DataBind()
    End Sub

End Class
