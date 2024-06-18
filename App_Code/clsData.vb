Imports System.Data
Imports System.Data.SqlClient
Module Common


    Public mFieldcol As New Collection
    Public mdecimal As Integer
    Public mqtydecimal As Integer
    Public Acc_Mode As Integer
    Public mtran_mode As Integer '1-Add,2-Modify,3-Delete

    Public macc_tran_id As String
    Public macc_tran_lineno As Integer
    Public macc_tran_type As String
    Public macc_div_id As String
    Public macc_tran_date As Date

    Public mparent_tran_id As String
    Public mclsSaveno As Integer
    Public mcust_code As String

    Public mvalid_amount As Boolean
    Public mvalid_controlacct As Boolean
    Public mvalid_batch As Boolean

    Public Function getdataset(ByVal sqlstr As String, ByRef scon As SqlConnection, ByRef stran As SqlTransaction) As DataSet
        Dim ds As DataSet
        Dim cmd As SqlCommand
        Dim da As SqlDataAdapter
        ds = New DataSet
        Try
            If Not sqlstr = Nothing Then
                cmd = New SqlCommand(sqlstr, scon, stran)
                da = New SqlDataAdapter(cmd)
                da.Fill(ds)
                cmd = Nothing
            End If
        Catch ex As Exception
            '            MsgBox(ex.Message)
        End Try
        Return ds
    End Function
    Public Function getdatatable(ByVal sqlstr As String, ByRef scon As SqlConnection, ByRef stran As SqlTransaction) As DataTable
        Dim dt As DataTable
        dt = New DataTable
        Try
            If Not sqlstr = Nothing Then
                Dim ds As DataSet
                ds = New DataSet
                ds = getdataset(sqlstr, scon, stran)
                dt = ds.Tables(0)
                ds = Nothing
            End If
        Catch ex As Exception
            '           MsgBox(ex.Message)
        End Try
        Return dt
    End Function

End Module
