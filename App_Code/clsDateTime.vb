'##################################################################################################################
'Project Name:
'Module Name:
'Created By:
'Purpose:
'##################################################################################################################

Public Class clsDateTime
    Dim objUtils As New clsUtils

    Dim strQry As String
    Public Function GetSystemDateTime(ByVal constr As String) As Date

        Try
            strQry = ""
            strQry = "select getdate()"
            GetSystemDateTime = CType(objUtils.ExecuteQueryReturnSingleValuenew(constr, strQry), Date)
        Catch ex As Exception
        End Try
    End Function

    Public Function GetSystemDateOnly(ByVal constr As String) As Date
        Try
            strQry = ""
            strQry = "select getdate()"
            GetSystemDateOnly = CType(objUtils.ExecuteQueryReturnSingleValuenew(constr, strQry), Date)
            GetSystemDateOnly = GetSystemDateOnly.ToShortDateString
        Catch ex As Exception
        End Try
    End Function
    Public Function GetSystemTimeOnly(ByVal constr As String) As Date
        Try
            strQry = ""
            strQry = "select getdate()"
            GetSystemTimeOnly = CType(objUtils.ExecuteQueryReturnSingleValuenew(constr, strQry), Date)
            GetSystemTimeOnly = GetSystemTimeOnly.ToShortTimeString
        Catch ex As Exception
        End Try
    End Function
    Public Function ConvertDateromTextBoxToDatabaseFormat(ByVal strdate As String) As String
        Dim strtemp As String = String.Empty
        Dim strday As String = String.Empty
        Dim strmonth As String = String.Empty
        Dim stryear As String = String.Empty
        Dim lnglist As Long
        Try
            For lnglist = 0 To Len(strdate)
                If Split(strdate, "/", , vbTextCompare)(lnglist) <> "" Then
                    If lnglist = 0 Then
                        strday = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 1 Then
                        strmonth = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 2 Then
                        stryear = Split(strdate, "/", , vbTextCompare)(lnglist)
                        Exit For
                    End If
                End If
            Next lnglist
            strtemp = "#" & Val(strmonth) & "/" & Val(strday) & "/" & Val(stryear) & "#"

            ConvertDateromTextBoxToDatabaseFormat = strtemp
        Catch SQLexc As Exception
            ConvertDateromTextBoxToDatabaseFormat = Nothing
        End Try
    End Function
    Public Function ConvertDateromTextBoxToDatabase(ByVal strdate As String) As Date
        Dim strtemp As String = String.Empty
        Dim strday As String = String.Empty
        Dim strmonth As String = String.Empty
        Dim stryear As String = String.Empty
        Dim lnglist As Long
        Try
            For lnglist = 0 To Len(strdate)
                If Split(strdate, "/", , vbTextCompare)(lnglist) <> "" Then
                    If lnglist = 0 Then
                        strday = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 1 Then
                        strmonth = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 2 Then
                        stryear = Split(strdate, "/", , vbTextCompare)(lnglist)
                        Exit For
                    End If
                End If
            Next lnglist

            ConvertDateromTextBoxToDatabase = New Date(Val(stryear), Val(strmonth), Val(strday))

        Catch SQLexc As Exception
            ConvertDateromTextBoxToDatabase = Nothing
        End Try
    End Function
    Public Function ConvertDateFromDatabaseToTextBoxFormat(ByVal strdate As String) As String
        Dim strtemp As String = String.Empty
        Dim strday As String = String.Empty
        Dim strmonth As String = String.Empty
        Dim stryear As String = String.Empty
        Dim lnglist As Long
        Try
            For lnglist = 0 To Len(strdate)
                If Split(strdate, "/", , vbTextCompare)(lnglist) <> "" Then
                    If lnglist = 0 Then
                        stryear = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 1 Then
                        strmonth = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 2 Then
                        strday = Split(strdate, "/", , vbTextCompare)(lnglist)
                        Exit For
                    End If
                End If
            Next lnglist
            If strday.Length = 1 Then
                strday = "0" & strday
            End If
            If strmonth.Length = 1 Then
                strmonth = "0" & strmonth
            End If
            strtemp = strday & "/" & strmonth & "/" & Val(stryear)
            ConvertDateFromDatabaseToTextBoxFormat = strtemp
        Catch SQLexc As Exception
            ConvertDateFromDatabaseToTextBoxFormat = Nothing
        End Try
    End Function
    Public Function ConvertDateromTextBoxToTextYearMonthDay(ByVal strdate As String) As String
        Dim strtemp As String = String.Empty
        Dim strday As String = String.Empty
        Dim strmonth As String = String.Empty
        Dim stryear As String = String.Empty
        Dim lnglist As Long
        Try
            For lnglist = 0 To Len(strdate)
                If Split(strdate, "/", , vbTextCompare)(lnglist) <> "" Then
                    If lnglist = 0 Then
                        strday = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 1 Then
                        strmonth = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 2 Then
                        stryear = Split(strdate, "/", , vbTextCompare)(lnglist)
                        Exit For
                    End If
                End If
            Next lnglist

            ConvertDateromTextBoxToTextYearMonthDay = stryear.Trim & "/" & strmonth & "/" & strday

        Catch SQLexc As Exception
            ConvertDateromTextBoxToTextYearMonthDay = Nothing
        End Try
    End Function
End Class
