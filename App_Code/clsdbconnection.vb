Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

Public Class clsdbconnection


    Public conT As SqlClient.SqlConnection
    Public Function OpenDBConnectionT() As Boolean
        Try
            conT = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("conStr"))
            conT.Open()
            OpenDBConnectionT = True
        Catch Ex As Exception
            OpenDBConnectionT = False
        End Try
    End Function

    Public Sub CloseDBConnectionT()
        Try
            conT.Close() : conT.Dispose() : conT = Nothing
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function dbConnection() As SqlConnection
        Dim SqlConn As New SqlConnection(ConfigurationManager.AppSettings("conStr"))
        dbConnection = SqlConn
        If dbConnection.State = Data.ConnectionState.Open Then
            CType(dbConnection, IDisposable).Dispose()
        End If
        dbConnection.Open()
    End Function
    Public Shared Sub dbConnectionClose(ByVal Sqlcon As SqlConnection)
        If Sqlcon.State = Data.ConnectionState.Open Then
            CType(Sqlcon, IDisposable).Dispose()
        End If
    End Sub

End Class
