Imports System.Data
Imports System.Data.SqlClient
Public Class clssave
    Dim accounts As Collection


    Sub clraccounts()
        Accounts = New Collection
    End Sub
    Sub Addaccounts(ByVal cls As clsAccounts)
        Accounts.Add(cls)
    End Sub
    Function checkaccounts() As Boolean
        On Error GoTo accerr
        checkaccounts = True
        If accounts.Count = 0 Then
        End If
        Exit Function
accerr:
        If Err.Number = 91 Then
            checkaccounts = False
            Exit Function
        End If
    End Function

    Function saveaccounts(ByVal constr As String, ByRef scon As SqlConnection, ByRef stran As SqlTransaction, ByVal accpage As Object) As Integer
        On Error GoTo saveerr
        Dim i As Integer
        Dim objutils As New clsUtils
        Dim accresult As Integer
        saveaccounts = 0
        If checkaccounts() = True Then
            For i = 1 To accounts.Count
                mclsSaveno = i
                accresult = accounts(i).saveentry(constr, scon, stran, accpage)
                If accresult <> 0 Then
                    saveaccounts = accresult
                    '0-No errors,1-Exception Error,2-Validateamount failed,3-Validateaccountfailed
                    Exit Function
                Else
                    If i = 1 Then
                        mparent_tran_id = accounts(i).acc_tran_id
                    End If
                End If
            Next
        Else
            saveaccounts = 1
            objutils.MessageBox("No Accounts Posting Data found", accpage)
        End If
        Exit Function
saveerr:
        saveaccounts = 1
    End Function

End Class
