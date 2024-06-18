'##################################################################################################################
'Project Name:
'Module Name:
'Created By:
'Purpose:
'##################################################################################################################

Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Public Class clsDBConnect
    Private Name As String
    Private Shared nameweb As String



    Public Shared Property webdb() As String
        Get
            Return nameweb
        End Get
        Set(ByVal Value As String)
            nameweb = Value
        End Set
    End Property




    Public Shared ReadOnly Property ConnectionString() As String
        Get

            'Dim clsDB As New clsDBConnect




            Return ConfigurationManager.ConnectionStrings(clsDBConnect.webdb).ConnectionString

            'Return ConfigurationManager.ConnectionStrings().ConnectionString
        End Get
    End Property

    Public Shared Function dbConnection() As SqlConnection
        Dim SqlConn As New SqlConnection(ConnectionString())
        dbConnection = SqlConn
        If dbConnection.State = Data.ConnectionState.Open Then
            CType(dbConnection, IDisposable).Dispose()
        End If
        dbConnection.Open()
    End Function
    Public Shared Function dbConnectionnew(ByVal wbdb As String) As SqlConnection
        Dim SqlConn As New SqlConnection(ConnectionStringnew(wbdb))
        dbConnectionnew = SqlConn
        If dbConnectionnew.State = Data.ConnectionState.Open Then
            CType(dbConnectionnew, IDisposable).Dispose()
        End If
        dbConnectionnew.Open()
    End Function
    Public Shared ReadOnly Property ConnectionStringnew(ByVal dbcon As String) As String
        Get

            Return ConfigurationManager.ConnectionStrings(dbcon).ConnectionString

        End Get
    End Property

    Public Shared Sub dbConnectionClose(ByVal Sqlcon As SqlConnection)
        Try
            If Sqlcon.State = Data.ConnectionState.Open Then
                CType(Sqlcon, IDisposable).Dispose()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Shared Sub dbReaderClose(ByVal myReader As SqlDataReader)
        CType(myReader, IDisposable).Dispose()
    End Sub
    Public Shared Sub dbCommandClose(ByVal myCommand As SqlCommand)
        Try
            CType(myCommand, IDisposable).Dispose()
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub dbAdapterClose(ByVal myAdapter As SqlDataAdapter)
        CType(myAdapter, IDisposable).Dispose()
    End Sub
    Public Shared Sub dbDataSetClose(ByVal ds As DataSet)
        CType(ds, IDisposable).Dispose()
    End Sub
    Public Shared Sub dbSqlTransation(ByVal sqlTrans As SqlTransaction)
        CType(sqlTrans, IDisposable).Dispose()
    End Sub
End Class