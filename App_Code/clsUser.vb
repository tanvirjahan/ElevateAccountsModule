'##################################################################################################################
'Project Name:
'Module Name:
'Created By:
'Purpose:
'##################################################################################################################

Imports System.Data.SqlClient
Imports System.Data

Public Class clsUser

#Region "Golbal Declaration"
    Dim myDataReader As SqlDataReader
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim objUtils As New clsUtils
    Dim strSqlQry As String

#End Region


#Region "Public Function ValidateUser(ByVal constr As String,ByVal strUserCode As String, ByVal strPassword As String) As Boolean"
    Public Function ValidateUser(ByVal constr As String, ByVal strUserCode As String, ByVal strPassword As String) As Boolean
        ValidateUser = False
        Try
            '   Session("dbconnectionName")

            strSqlQry = "SELECT UserCode,dbo.pwddecript(userpwd) userpwd FROM usermaster WHERE UserCode='" & strUserCode & "' AND dbo.pwddecript(userpwd)='" & strPassword & "' AND active=1"
            SqlConn = clsDBConnect.dbConnectionnew(constr)                   'connection open
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataReader = myCommand.ExecuteReader
            If myDataReader.HasRows Then
                If myDataReader.Read Then
                    If StrComp(myDataReader("UserCode"), strUserCode, CompareMethod.Binary) = 0 And StrComp(myDataReader("userpwd"), strPassword, CompareMethod.Binary) = 0 Then
                        ValidateUser = True
                    Else
                        ValidateUser = False
                    End If
                End If
            Else
                ValidateUser = False
            End If
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Catch ex As Exception
            ValidateUser = False
        End Try
    End Function
#End Region

#Region "Public Function LoggedAs(ByVal constr As String,ByVal strUserCode As String, ByVal strPassword As String) As String"
    Public Function LoggedAs(ByVal constr As String, ByVal strUserCode As String, ByVal strPassword As String) As String
        LoggedAs = False
        Try
            strSqlQry = "SELECT UserName FROM UserMaster WHERE UserCode='" & strUserCode & "' AND dbo.pwddecript(userpwd)='" & strPassword & "' AND active=1"
            SqlConn = clsDBConnect.dbConnectionnew(constr)                  'connection open
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataReader = myCommand.ExecuteReader
            If myDataReader.HasRows Then
                If myDataReader.Read Then
                    LoggedAs = CType(myDataReader("UserName"), String)
                End If
            End If
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Catch ex As Exception
            LoggedAs = Nothing
        End Try
    End Function
#End Region

#Region "Public Function GetAppName(ByVal constr As String,ByVal strUserCode As String, ByVal strPassword As String) As SqlDataReader"
    Public Function GetAppName(ByVal constr As String, ByVal strUserCode As String, ByVal strPassword As String) As SqlDataReader
        strSqlQry = ""
        Try
            SqlConn = clsDBConnect.dbConnectionnew(constr)                     'Open connection
            strSqlQry = "SELECT appname FROM appmaster WHERE appid in " & _
                          "(SELECT appid FROM group_app_detail WHERE groupid in(SELECT groupid FROM UserMaster " & _
                          " WHERE UserCode='" & strUserCode & "' AND dbo.pwddecript(userpwd)='" & strPassword & "' AND active=1)) AND appstatus=1 order by appmaster.rankorder "
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            GetAppName = myDataReader
        Catch ex As Exception
            GetAppName = Nothing
        End Try

    End Function
#End Region

#Region "Public Function GetAppDisplayName(ByVal constr As String,ByVal strUserCode As String, ByVal strPassword As String) As SqlDataReader"
    Public Function GetAppDisplayName(ByVal constr As String, ByVal strUserCode As String, ByVal strPassword As String) As SqlDataReader
        strSqlQry = ""
        Try
            SqlConn = clsDBConnect.dbConnectionnew(constr)                     'Open connection
            strSqlQry = "SELECT DisplayName appname FROM appmaster WHERE appid in " & _
                          "(SELECT appid FROM group_app_detail WHERE groupid in(SELECT groupid FROM UserMaster " & _
                          " WHERE UserCode='" & strUserCode & "' AND dbo.pwddecript(userpwd)='" & strPassword & "' AND active=1)) AND appstatus=1 order by appmaster.rankorder "
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            GetAppDisplayName = myDataReader
        Catch ex As Exception
            GetAppDisplayName = Nothing
        End Try

    End Function
#End Region

#Region "Public Function GetAppId(ByVal constr As String,ByVal strAppName As String) As Integer"
    Public Function GetAppId(ByVal constr As String, ByVal strAppName As String) As Integer
        Try
            strSqlQry = ""
            strSqlQry = "SELECT appid FROM appmaster WHERE appname='" & strAppName & "'"
            GetAppId = objUtils.ExecuteQueryReturnSingleValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetAppId = Nothing
        End Try
    End Function
#End Region

#Region " Public Sub GetAppDetails(ByVal constr As String, ByVal strAppName As String, ByRef AppId As Integer, ByRef AppPageName As String)"
    Public Sub GetAppDetails(ByVal constr As String, ByVal strAppName As String, ByRef AppId? As Integer, ByRef AppPageName As String)
        Try
            SqlConn = clsDBConnect.dbConnectionnew(constr)                     'Open connection
            strSqlQry = "SELECT appid,pageName FROM appmaster WHERE appname='" & strAppName & "'"
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If (myDataReader.Read()) Then
                AppId = myDataReader("appid")
                AppPageName = myDataReader("pageName")
            Else
                AppId = Nothing
                AppPageName = ""
            End If
        Catch ex As Exception
            clsDBConnect.dbCommandClose(myCommand) 'Close command
            clsDBConnect.dbReaderClose(myDataReader) 'Close reader
            clsDBConnect.dbConnectionClose(SqlConn) 'Close  connection()
            AppId = Nothing
            AppPageName = ""
        End Try
    End Sub
#End Region


#Region "Public Function GetGroupId(ByVal strUserCode As String, ByVal strPassword As String) As Integer"
    Public Function GetGroupId(ByVal constr As String, ByVal strUserCode As String, ByVal strPassword As String) As Integer
        Try
            strSqlQry = ""
            strSqlQry = "SELECT groupid FROM UserMaster  WHERE UserCode='" & strUserCode & "' AND dbo.pwddecript(userpwd)='" & strPassword & "' AND active=1"
            GetGroupId = objUtils.ExecuteQueryReturnSingleValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetGroupId = Nothing
        End Try
    End Function
#End Region

#Region "Public Function GetMenuId(ByVal constr As String,ByVal PageName As String) As Integer"
    Public Function GetMenuId(ByVal constr As String, ByVal PageName As String, ByVal appid As Integer) As Integer
        Try
            strSqlQry = ""
            strSqlQry = "SELECT menuid FROM MenuMaster WHERE upper(pagename)='" & PageName.ToUpper & "' and appid=" & appid
            GetMenuId = objUtils.ExecuteQueryReturnSingleValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetMenuId = Nothing
        End Try
    End Function
#End Region

    Public Function GetPMenuId(ByVal constr As String, ByVal PageName As String, ByVal appid As Integer, ByVal parentid As Integer) As Integer
        Try
            strSqlQry = ""
            strSqlQry = "SELECT menuid FROM MenuMaster WHERE upper(pagename)='" & PageName.ToUpper & "' and appid='" & appid & "' And parentid =' " & parentid & "'"
            GetPMenuId = objUtils.ExecuteQueryReturnSingleValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetPMenuId = Nothing
        End Try
    End Function



#Region "Public Function GetSubMenuId(ByVal constr As String,ByVal PageName As String,ByVal appid As String,ByVal CalledfromValue As String) As Integer"
    Public Function GetSubMenuId(ByVal constr As String, ByVal PageName As String, ByVal appid As Integer, ByVal CalledfromValue As String) As Integer
        Try
            strSqlQry = ""
            strSqlQry = "SELECT submenuid FROM SubMenuMaster WHERE upper(pagename)='" & PageName.ToUpper & "' and appid='" & appid & "'  And MenuId = '" & CalledfromValue & "'"
            GetSubMenuId = objUtils.ExecuteQueryReturnSingleValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetSubMenuId = Nothing
        End Try
    End Function
#End Region


#Region "Public Function GetCotractofferMenuId(ByVal constr As String,ByVal PageName As String,ByVal appid As String,ByVal CalledfromValue As String) As Integer"
    Public Function GetCotractofferMenuId(ByVal constr As String, ByVal PageName As String, ByVal appid As Integer, ByVal CalledfromValue As String) As Integer
        Try
            strSqlQry = ""
            strSqlQry = "SELECT menuid FROM MenuMaster WHERE upper(pagename)='" & PageName.ToUpper & "' and appid='" & appid & "'  And parentId = '" & CalledfromValue & "'"
            GetCotractofferMenuId = objUtils.ExecuteQueryReturnSingleValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetCotractofferMenuId = Nothing
        End Try
    End Function
#End Region




#Region "Public Function GetUserFunctionalRight(ByVal constr As String,ByVal GroupId As Integer, ByVal AppId As Integer, ByVal MenuId As Integer) As String"
    Public Function GetUserFunctionalRight(ByVal constr As String, ByVal GroupId As Integer, ByVal AppId As Integer, ByVal MenuId As Integer) As String
        Try
            strSqlQry = ""
            strSqlQry = "SELECT functional_rights FROM group_functionalrights WHERE grpid='" & GroupId & "' AND appid='" & AppId & "' AND menuid='" & MenuId & "'"
            GetUserFunctionalRight = objUtils.ExecuteQueryReturnSingleValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetUserFunctionalRight = Nothing
        End Try
    End Function
#End Region

#Region "Private Sub CheckUserRight()"
    Public Sub CheckUserRight(ByVal constr As String, ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, _
                               ByVal btnAdd As Button, ByVal btnExportToExcel As Button, ByVal btnPrint As Button, ByVal gvSearch As GridView, _
                               Optional ByVal EditColumnNo As Integer = 0, Optional ByVal DeleteColumnNo As Integer = 0, Optional ByVal ViewColumnNo As Integer = 0, _
                               Optional ByVal PrintColumnNo As Integer = 0, Optional ByVal CopyColumnNo As Integer = 0, _
                               Optional ByVal CancelColumnNo As Integer = 0, Optional ByVal UndoCancelColumnNo As Integer = 0,
                                Optional ByVal EmailColumnNo As Integer = 0, Optional ByVal ApproveColumnNo As Integer = 0, Optional ByVal AssignColumnNo As Integer = 0,
                                Optional ByVal RemoveAssignColumnNo As Integer = 0, Optional ByVal TransferColumnNo As Integer = 0, Optional ByVal WithdrawColumnNo As Integer = 0)
        Dim intGroupID As Integer
        Dim intAppID As Integer
        Dim intMenuID As Integer
        Dim strUserFunctionalRight As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim lngCount As Int16
        Dim strGetUserFunctionalRightValue As String
        Dim AddFlag As Boolean = False
        Dim EditFlag As Boolean = False
        Dim DeleteFlag As Boolean = False
        Dim ViewFlag As Boolean = False
        Dim ExportToExcelFlag As Boolean = False
        Dim ReportFlag As Boolean = False
        Dim PrintFlag As Boolean = False
        Dim CopyFlag As Boolean = False
        Dim CancelFlag As Boolean = False
        Dim UndoCancelFlag As Boolean = False
        Dim ApproveFlag As Boolean = False
        Dim EmailFlag As Boolean = False
        Dim AssignFlag As Boolean = False
        Dim RemoveAssignFlag As Boolean = False
        Dim TransferFlag As Boolean = False
        Dim WithdrawFlag As Boolean = False

        If CType(UserName, String) = "" Or CType(UserPwd, String) = "" Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intGroupID = GetGroupId(constr, CType(UserName, String), CType(UserPwd, String))
        End If
        If CType(AppName, String) = "" Or CType(AppName, String) = Nothing Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intAppID = GetAppId(constr, CType(AppName, String))
        End If
        intMenuID = GetMenuId(constr, PageName, intAppID)




        If Val(intGroupID) = 0 And Val(intAppID) = 0 And Val(intMenuID) = 0 Then
            ' Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            strUserFunctionalRight = GetUserFunctionalRight(constr, intGroupID, intAppID, intMenuID)
            If strUserFunctionalRight <> "" Then
                strTempUserFunctionalRight = strUserFunctionalRight.Split(";")

                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strGetUserFunctionalRightValue = strTempUserFunctionalRight.GetValue(lngCount)
                    'Add,Edit,Delete,View,Export,Print

                    If CType(strGetUserFunctionalRightValue, String) = "01" Or CType(strGetUserFunctionalRightValue, String) = "1" Then
                        AddFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "02" Or CType(strGetUserFunctionalRightValue, String) = "2" Then
                        EditFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "03" Or CType(strGetUserFunctionalRightValue, String) = "3" Then
                        DeleteFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "04" Or CType(strGetUserFunctionalRightValue, String) = "4" Then
                        ViewFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "05" Or CType(strGetUserFunctionalRightValue, String) = "5" Then
                        ExportToExcelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "06" Or CType(strGetUserFunctionalRightValue, String) = "6" Then
                        ReportFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "09" Or CType(strGetUserFunctionalRightValue, String) = "9" Then
                        PrintFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "07" Or CType(strGetUserFunctionalRightValue, String) = "7" Then
                        CopyFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "10" Then
                        CancelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "11" Then
                        UndoCancelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "12" Then
                        ApproveFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "13" Then
                        AssignFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "14" Then
                        RemoveAssignFlag = True
                    End If

                    If CType(strGetUserFunctionalRightValue, String) = "15" Then
                        TransferFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "16" Then
                        WithdrawFlag = True
                    End If
                    'Since no rights for email, setting as true.
                    EmailFlag = True

                Next
            Else
                AddFlag = False
                EditFlag = False
                DeleteFlag = False
                ViewFlag = False
                ExportToExcelFlag = False
                ReportFlag = False
                PrintFlag = False
                CopyFlag = False
                CancelFlag = False
                UndoCancelFlag = False
                EmailFlag = False
                ApproveFlag = False
                AssignFlag = False
                RemoveAssignFlag = False
                TransferFlag = False
                WithdrawFlag = False
            End If
        End If

        If AddFlag = True Then
            btnAdd.Visible = True
        Else
            btnAdd.Visible = False
        End If
        If EditColumnNo <> 0 Then
            If EditFlag = True Then
                gvSearch.Columns(EditColumnNo).Visible = True
            Else
                gvSearch.Columns(EditColumnNo).Visible = False
            End If
        End If
        If DeleteColumnNo <> 0 Then
            If DeleteFlag = True Then
                gvSearch.Columns(DeleteColumnNo).Visible = True
            Else
                gvSearch.Columns(DeleteColumnNo).Visible = False
            End If
        End If
        If ViewColumnNo <> 0 Then
            If ViewFlag = True Then
                gvSearch.Columns(ViewColumnNo).Visible = True
            Else
                gvSearch.Columns(ViewColumnNo).Visible = False
            End If
        End If


        If ExportToExcelFlag = True Then
            btnExportToExcel.Visible = True
        Else
            btnExportToExcel.Visible = False
        End If
        If ReportFlag = True Then
            btnPrint.Visible = True
        Else
            btnPrint.Visible = False
        End If
        If PrintColumnNo <> 0 Then
            If PrintFlag = True Then
                gvSearch.Columns(PrintColumnNo).Visible = True
            Else
                gvSearch.Columns(PrintColumnNo).Visible = False
            End If
        End If
        If CopyColumnNo <> 0 Then
            If CopyFlag = True Then
                gvSearch.Columns(CopyColumnNo).Visible = True
            Else
                gvSearch.Columns(CopyColumnNo).Visible = False
            End If
        End If

        If CancelColumnNo <> 0 Then
            If CancelFlag = True Then
                gvSearch.Columns(CancelColumnNo).Visible = True
            Else
                gvSearch.Columns(CancelColumnNo).Visible = False
            End If
        End If

        If UndoCancelColumnNo <> 0 Then
            If UndoCancelFlag = True Then
                gvSearch.Columns(UndoCancelColumnNo).Visible = True
            Else
                gvSearch.Columns(UndoCancelColumnNo).Visible = False
            End If
        End If
        If EmailColumnNo <> 0 Then
            If EmailFlag = True Then
                gvSearch.Columns(EmailColumnNo).Visible = True
            Else
                gvSearch.Columns(EmailColumnNo).Visible = False
            End If
        End If
        If ApproveColumnNo <> 0 Then
            If ApproveFlag = True Then
                gvSearch.Columns(ApproveColumnNo).Visible = True
            Else
                gvSearch.Columns(ApproveColumnNo).Visible = False
            End If
        End If

        If AssignColumnNo <> 0 Then
            If AssignFlag = True Then
                gvSearch.Columns(AssignColumnNo).Visible = True
            Else
                gvSearch.Columns(AssignColumnNo).Visible = False
            End If
        End If

        If RemoveAssignColumnNo <> 0 Then
            If RemoveAssignFlag = True Then
                gvSearch.Columns(RemoveAssignColumnNo).Visible = True
            Else
                gvSearch.Columns(RemoveAssignColumnNo).Visible = False
            End If
        End If

        If TransferColumnNo <> 0 Then
            If TransferFlag = True Then
                gvSearch.Columns(TransferColumnNo).Visible = True
            Else
                gvSearch.Columns(TransferColumnNo).Visible = False
            End If
        End If
        If WithdrawColumnNo <> 0 Then
            If WithdrawFlag = True Then
                gvSearch.Columns(WithdrawColumnNo).Visible = True
            Else
                gvSearch.Columns(WithdrawColumnNo).Visible = False
            End If
        End If

    End Sub
#End Region

    Public Sub CheckPUserRight(ByVal constr As String, ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, ByVal parentid As Integer, _
                               ByVal btnAdd As Button, ByVal btnExportToExcel As Button, ByVal btnPrint As Button, ByVal gvSearch As GridView, _
                               Optional ByVal EditColumnNo As Integer = 0, Optional ByVal DeleteColumnNo As Integer = 0, Optional ByVal ViewColumnNo As Integer = 0, _
                               Optional ByVal PrintColumnNo As Integer = 0, Optional ByVal CopyColumnNo As Integer = 0, _
                               Optional ByVal CancelColumnNo As Integer = 0, Optional ByVal UndoCancelColumnNo As Integer = 0,
                                Optional ByVal EmailColumnNo As Integer = 0, Optional ByVal ApproveColumnNo As Integer = 0, Optional ByVal AssignColumnNo As Integer = 0, Optional ByVal RemoveAssignColumnNo As Integer = 0, Optional ByVal TransferColumnNo As Integer = 0)
        Dim intGroupID As Integer
        Dim intAppID As Integer
        Dim intMenuID As Integer
        Dim strUserFunctionalRight As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim lngCount As Int16
        Dim strGetUserFunctionalRightValue As String
        Dim AddFlag As Boolean = False
        Dim EditFlag As Boolean = False
        Dim DeleteFlag As Boolean = False
        Dim ViewFlag As Boolean = False
        Dim ExportToExcelFlag As Boolean = False
        Dim ReportFlag As Boolean = False
        Dim PrintFlag As Boolean = False
        Dim CopyFlag As Boolean = False
        Dim CancelFlag As Boolean = False
        Dim UndoCancelFlag As Boolean = False
        Dim ApproveFlag As Boolean = False
        Dim EmailFlag As Boolean = False
        Dim AssignFlag As Boolean = False
        Dim RemoveAssignFlag As Boolean = False
        Dim TransferFlag As Boolean = False

        If CType(UserName, String) = "" Or CType(UserPwd, String) = "" Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intGroupID = GetGroupId(constr, CType(UserName, String), CType(UserPwd, String))
        End If
        If CType(AppName, String) = "" Or CType(AppName, String) = Nothing Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intAppID = GetAppId(constr, CType(AppName, String))
        End If
        intMenuID = GetPMenuId(constr, PageName, intAppID, parentid)




        If Val(intGroupID) = 0 And Val(intAppID) = 0 And Val(intMenuID) = 0 Then
            ' Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            strUserFunctionalRight = GetUserFunctionalRight(constr, intGroupID, intAppID, intMenuID)
            If strUserFunctionalRight <> "" Then
                strTempUserFunctionalRight = strUserFunctionalRight.Split(";")

                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strGetUserFunctionalRightValue = strTempUserFunctionalRight.GetValue(lngCount)
                    'Add,Edit,Delete,View,Export,Print

                    If CType(strGetUserFunctionalRightValue, String) = "01" Or CType(strGetUserFunctionalRightValue, String) = "1" Then
                        AddFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "02" Or CType(strGetUserFunctionalRightValue, String) = "2" Then
                        EditFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "03" Or CType(strGetUserFunctionalRightValue, String) = "3" Then
                        DeleteFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "04" Or CType(strGetUserFunctionalRightValue, String) = "4" Then
                        ViewFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "05" Or CType(strGetUserFunctionalRightValue, String) = "5" Then
                        ExportToExcelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "06" Or CType(strGetUserFunctionalRightValue, String) = "6" Then
                        ReportFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "09" Or CType(strGetUserFunctionalRightValue, String) = "9" Then
                        PrintFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "07" Or CType(strGetUserFunctionalRightValue, String) = "7" Then
                        CopyFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "10" Then
                        CancelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "11" Then
                        UndoCancelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "12" Then
                        ApproveFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "13" Then
                        AssignFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "14" Then
                        RemoveAssignFlag = True
                    End If

                    If CType(strGetUserFunctionalRightValue, String) = "15" Then
                        TransferFlag = True
                    End If
                    'Since no rights for email, setting as true.
                    EmailFlag = True

                Next
            Else
                AddFlag = False
                EditFlag = False
                DeleteFlag = False
                ViewFlag = False
                ExportToExcelFlag = False
                ReportFlag = False
                PrintFlag = False
                CopyFlag = False
                CancelFlag = False
                UndoCancelFlag = False
                EmailFlag = False
                ApproveFlag = False
                AssignFlag = False
                RemoveAssignFlag = False
                TransferFlag = False
            End If
        End If

        If AddFlag = True Then
            btnAdd.Visible = True
        Else
            btnAdd.Visible = False
        End If
        If EditColumnNo <> 0 Then
            If EditFlag = True Then
                gvSearch.Columns(EditColumnNo).Visible = True
            Else
                gvSearch.Columns(EditColumnNo).Visible = False
            End If
        End If
        If DeleteColumnNo <> 0 Then
            If DeleteFlag = True Then
                gvSearch.Columns(DeleteColumnNo).Visible = True
            Else
                gvSearch.Columns(DeleteColumnNo).Visible = False
            End If
        End If
        If ViewColumnNo <> 0 Then
            If ViewFlag = True Then
                gvSearch.Columns(ViewColumnNo).Visible = True
            Else
                gvSearch.Columns(ViewColumnNo).Visible = False
            End If
        End If


        If ExportToExcelFlag = True Then
            btnExportToExcel.Visible = True
        Else
            btnExportToExcel.Visible = False
        End If
        If ReportFlag = True Then
            btnPrint.Visible = True
        Else
            btnPrint.Visible = False
        End If
        If PrintColumnNo <> 0 Then
            If PrintFlag = True Then
                gvSearch.Columns(PrintColumnNo).Visible = True
            Else
                gvSearch.Columns(PrintColumnNo).Visible = False
            End If
        End If
        If CopyColumnNo <> 0 Then
            If CopyFlag = True Then
                gvSearch.Columns(CopyColumnNo).Visible = True
            Else
                gvSearch.Columns(CopyColumnNo).Visible = False
            End If
        End If

        If CancelColumnNo <> 0 Then
            If CancelFlag = True Then
                gvSearch.Columns(CancelColumnNo).Visible = True
            Else
                gvSearch.Columns(CancelColumnNo).Visible = False
            End If
        End If

        If UndoCancelColumnNo <> 0 Then
            If UndoCancelFlag = True Then
                gvSearch.Columns(UndoCancelColumnNo).Visible = True
            Else
                gvSearch.Columns(UndoCancelColumnNo).Visible = False
            End If
        End If
        If EmailColumnNo <> 0 Then
            If EmailFlag = True Then
                gvSearch.Columns(EmailColumnNo).Visible = True
            Else
                gvSearch.Columns(EmailColumnNo).Visible = False
            End If
        End If
        If ApproveColumnNo <> 0 Then
            If ApproveFlag = True Then
                gvSearch.Columns(ApproveColumnNo).Visible = True
            Else
                gvSearch.Columns(ApproveColumnNo).Visible = False
            End If
        End If

        If AssignColumnNo <> 0 Then
            If AssignFlag = True Then
                gvSearch.Columns(AssignColumnNo).Visible = True
            Else
                gvSearch.Columns(AssignColumnNo).Visible = False
            End If
        End If

        If RemoveAssignColumnNo <> 0 Then
            If RemoveAssignFlag = True Then
                gvSearch.Columns(RemoveAssignColumnNo).Visible = True
            Else
                gvSearch.Columns(RemoveAssignColumnNo).Visible = False
            End If
        End If

        If TransferColumnNo <> 0 Then
            If TransferFlag = True Then
                gvSearch.Columns(TransferColumnNo).Visible = True
            Else
                gvSearch.Columns(TransferColumnNo).Visible = False
            End If
        End If

    End Sub

#Region "Private Sub CheckNewFormatUserRight()"
    Public Sub CheckNewFormatUserRight(ByVal constr As String, ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, _
                               ByVal btnAdd As Button, ByVal btnEdit As Button, ByVal btnDelete As Button, ByVal btnView As Button, ByVal btnExportToExcel As Button, ByVal btnPrint As Button)
        Dim intGroupID As Integer
        Dim intAppID As Integer
        Dim intMenuID As Integer
        Dim strUserFunctionalRight As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim lngCount As Int16
        Dim strGetUserFunctionalRightValue As String
        Dim AddFlag As Boolean = False
        Dim EditFlag As Boolean = False
        Dim DeleteFlag As Boolean = False
        Dim ViewFlag As Boolean = False
        Dim ExportToExcelFlag As Boolean = False


        Dim ReportFlag As Boolean = False

        If CType(UserName, String) = "" Or CType(UserPwd, String) = "" Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intGroupID = GetGroupId(constr, CType(UserName, String), CType(UserPwd, String))
        End If
        If CType(AppName, String) = "" Or CType(AppName, String) = Nothing Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intAppID = GetAppId(constr, CType(AppName, String))
        End If
        intMenuID = GetMenuId(constr, PageName, intAppID)




        If Val(intGroupID) = 0 And Val(intAppID) = 0 And Val(intMenuID) = 0 Then
            ' Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            strUserFunctionalRight = GetUserFunctionalRight(constr, intGroupID, intAppID, intMenuID)
            If strUserFunctionalRight <> "" Then
                strTempUserFunctionalRight = strUserFunctionalRight.Split(";")

                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strGetUserFunctionalRightValue = strTempUserFunctionalRight.GetValue(lngCount)
                    'Add,Edit,Delete,View,Export,Print

                    If CType(strGetUserFunctionalRightValue, String) = "01" Or CType(strGetUserFunctionalRightValue, String) = "1" Then
                        AddFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "02" Or CType(strGetUserFunctionalRightValue, String) = "2" Then
                        EditFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "03" Or CType(strGetUserFunctionalRightValue, String) = "3" Then
                        DeleteFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "04" Or CType(strGetUserFunctionalRightValue, String) = "4" Then
                        ViewFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "05" Or CType(strGetUserFunctionalRightValue, String) = "5" Then
                        ExportToExcelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "06" Or CType(strGetUserFunctionalRightValue, String) = "6" Then
                        ReportFlag = True
                    End If


                Next
            Else
                AddFlag = False
                EditFlag = False
                DeleteFlag = False
                ViewFlag = False
                ExportToExcelFlag = False
                ReportFlag = False


            End If
        End If

        If AddFlag = True Then
            btnAdd.Visible = True
        Else
            btnAdd.Visible = False
        End If

        If EditFlag = True Then
            btnEdit.Visible = True
        Else
            btnEdit.Visible = False
        End If


        If DeleteFlag = True Then
            btnDelete.Visible = True
        Else
            btnDelete.Visible = False
        End If


        If ViewFlag = True Then
            btnView.Visible = True
        Else
            btnView.Visible = False
        End If



        If ExportToExcelFlag = True Then
            btnExportToExcel.Visible = True
        Else
            btnExportToExcel.Visible = False
        End If
        If ReportFlag = True Then
            btnPrint.Visible = True
        Else
            btnPrint.Visible = False
        End If


    End Sub
#End Region




#Region "Private Sub CheckUserSubRight()"
    Public Sub CheckUserSubRight(ByVal constr As String, ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, ByVal CalledfromValue As String, _
                               ByVal btnAdd As Button, ByVal btnExportToExcel As Button, ByVal btnPrint As Button, ByVal gvSearch As GridView, _
                               Optional ByVal EditColumnNo As Integer = 0, Optional ByVal DeleteColumnNo As Integer = 0, Optional ByVal ViewColumnNo As Integer = 0, _
                               Optional ByVal PrintColumnNo As Integer = 0, Optional ByVal CopyColumnNo As Integer = 0, _
                               Optional ByVal CancelColumnNo As Integer = 0, Optional ByVal UndoCancelColumnNo As Integer = 0,
                                Optional ByVal EmailColumnNo As Integer = 0, Optional ByVal ApproveColumnNo As Integer = 0, Optional ByVal AssignColumnNo As Integer = 0, Optional ByVal RemoveAssignColumnNo As Integer = 0, Optional ByVal TransferColumnNo As Integer = 0)
        Dim intGroupID As Integer
        Dim intAppID As Integer
        Dim intMenuID As Integer
        Dim strUserFunctionalRight As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim lngCount As Int16
        Dim strGetUserFunctionalRightValue As String
        Dim AddFlag As Boolean = False
        Dim EditFlag As Boolean = False
        Dim DeleteFlag As Boolean = False
        Dim ViewFlag As Boolean = False
        Dim ExportToExcelFlag As Boolean = False
        Dim ReportFlag As Boolean = False
        Dim PrintFlag As Boolean = False
        Dim CopyFlag As Boolean = False
        Dim CancelFlag As Boolean = False
        Dim UndoCancelFlag As Boolean = False
        Dim ApproveFlag As Boolean = False
        Dim EmailFlag As Boolean = False
        Dim AssignFlag As Boolean = False
        Dim RemoveAssignFlag As Boolean = False
        Dim TransferFlag As Boolean = False

        If CType(UserName, String) = "" Or CType(UserPwd, String) = "" Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intGroupID = GetGroupId(constr, CType(UserName, String), CType(UserPwd, String))
        End If
        If CType(AppName, String) = "" Or CType(AppName, String) = Nothing Then
            'Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            intAppID = GetAppId(constr, CType(AppName, String))
        End If
        intMenuID = GetCotractofferMenuId(constr, PageName, intAppID, CalledfromValue)



        If Val(intGroupID) = 0 And Val(intAppID) = 0 And Val(intMenuID) = 0 Then
            ' Response.Redirect("Login.aspx", False)
            Exit Sub
        Else
            strUserFunctionalRight = GetUserFunctionalRight(constr, intGroupID, intAppID, intMenuID)
            If strUserFunctionalRight <> "" Then
                strTempUserFunctionalRight = strUserFunctionalRight.Split(";")

                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strGetUserFunctionalRightValue = strTempUserFunctionalRight.GetValue(lngCount)
                    'Add,Edit,Delete,View,Export,Print

                    If CType(strGetUserFunctionalRightValue, String) = "01" Or CType(strGetUserFunctionalRightValue, String) = "1" Then
                        AddFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "02" Or CType(strGetUserFunctionalRightValue, String) = "2" Then
                        EditFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "03" Or CType(strGetUserFunctionalRightValue, String) = "3" Then
                        DeleteFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "04" Or CType(strGetUserFunctionalRightValue, String) = "4" Then
                        ViewFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "05" Or CType(strGetUserFunctionalRightValue, String) = "5" Then
                        ExportToExcelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "06" Or CType(strGetUserFunctionalRightValue, String) = "6" Then
                        ReportFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "09" Or CType(strGetUserFunctionalRightValue, String) = "9" Then
                        PrintFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "07" Or CType(strGetUserFunctionalRightValue, String) = "7" Then
                        CopyFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "10" Then
                        CancelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "11" Then
                        UndoCancelFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "12" Then
                        ApproveFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "13" Then
                        AssignFlag = True
                    End If
                    If CType(strGetUserFunctionalRightValue, String) = "14" Then
                        RemoveAssignFlag = True
                    End If

                    If CType(strGetUserFunctionalRightValue, String) = "15" Then
                        TransferFlag = True
                    End If
                    'Since no rights for email, setting as true.
                    EmailFlag = True

                Next
            Else
                AddFlag = False
                EditFlag = False
                DeleteFlag = False
                ViewFlag = False
                ExportToExcelFlag = False
                ReportFlag = False
                PrintFlag = False
                CopyFlag = False
                CancelFlag = False
                UndoCancelFlag = False
                EmailFlag = False
                ApproveFlag = False
                AssignFlag = False
                RemoveAssignFlag = False
                TransferFlag = False
            End If
        End If

        If AddFlag = True Then
            btnAdd.Visible = True
        Else
            btnAdd.Visible = False
        End If
        If EditColumnNo <> 0 Then
            If EditFlag = True Then
                gvSearch.Columns(EditColumnNo).Visible = True
            Else
                gvSearch.Columns(EditColumnNo).Visible = False
            End If
        End If
        If DeleteColumnNo <> 0 Then
            If DeleteFlag = True Then
                gvSearch.Columns(DeleteColumnNo).Visible = True
            Else
                gvSearch.Columns(DeleteColumnNo).Visible = False
            End If
        End If
        If ViewColumnNo <> 0 Then
            If ViewFlag = True Then
                gvSearch.Columns(ViewColumnNo).Visible = True
            Else
                gvSearch.Columns(ViewColumnNo).Visible = False
            End If
        End If


        If ExportToExcelFlag = True Then
            btnExportToExcel.Visible = True
        Else
            btnExportToExcel.Visible = False
        End If
        If ReportFlag = True Then
            btnPrint.Visible = True
        Else
            btnPrint.Visible = False
        End If
        If PrintColumnNo <> 0 Then
            If PrintFlag = True Then
                gvSearch.Columns(PrintColumnNo).Visible = True
            Else
                gvSearch.Columns(PrintColumnNo).Visible = False
            End If
        End If
        If CopyColumnNo <> 0 Then
            If CopyFlag = True Then
                gvSearch.Columns(CopyColumnNo).Visible = True
            Else
                gvSearch.Columns(CopyColumnNo).Visible = False
            End If
        End If

        If CancelColumnNo <> 0 Then
            If CancelFlag = True Then
                gvSearch.Columns(CancelColumnNo).Visible = True
            Else
                gvSearch.Columns(CancelColumnNo).Visible = False
            End If
        End If

        If UndoCancelColumnNo <> 0 Then
            If UndoCancelFlag = True Then
                gvSearch.Columns(UndoCancelColumnNo).Visible = True
            Else
                gvSearch.Columns(UndoCancelColumnNo).Visible = False
            End If
        End If
        If EmailColumnNo <> 0 Then
            If EmailFlag = True Then
                gvSearch.Columns(EmailColumnNo).Visible = True
            Else
                gvSearch.Columns(EmailColumnNo).Visible = False
            End If
        End If
        If ApproveColumnNo <> 0 Then
            If ApproveFlag = True Then
                gvSearch.Columns(ApproveColumnNo).Visible = True
            Else
                gvSearch.Columns(ApproveColumnNo).Visible = False
            End If
        End If

        If AssignColumnNo <> 0 Then
            If AssignFlag = True Then
                gvSearch.Columns(AssignColumnNo).Visible = True
            Else
                gvSearch.Columns(AssignColumnNo).Visible = False
            End If
        End If

        If RemoveAssignColumnNo <> 0 Then
            If RemoveAssignFlag = True Then
                gvSearch.Columns(RemoveAssignColumnNo).Visible = True
            Else
                gvSearch.Columns(RemoveAssignColumnNo).Visible = False
            End If
        End If

        If TransferColumnNo <> 0 Then
            If TransferFlag = True Then
                gvSearch.Columns(TransferColumnNo).Visible = True
            Else
                gvSearch.Columns(TransferColumnNo).Visible = False
            End If
        End If

    End Sub
#End Region

















#Region "Public Function ValidateMainUser(ByVal constr As String,ByVal strUserName As String, ByVal strPassword As String) As Boolean"
    Public Function ValidateMainUser(ByVal constr As String, ByVal strUserName As String, ByVal strPassword As String) As Boolean
        ValidateMainUser = False
        Try
            strSqlQry = "SELECT webusername,dbo.pwddecript(webpassword) webpassword FROM agentmast WHERE webusername='" & strUserName & "' AND dbo.pwddecript(webpassword)='" & strPassword & "' AND active=1 and isnull(webapprove,0) =1"
            SqlConn = clsDBConnect.dbConnectionnew(constr)                 'connection open
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataReader = myCommand.ExecuteReader
            If myDataReader.HasRows Then
                If myDataReader.Read Then
                    If StrComp(myDataReader("webusername"), strUserName, CompareMethod.Binary) = 0 And _
                                       StrComp(myDataReader("webpassword"), strPassword, CompareMethod.Binary) = 0 Then
                        ValidateMainUser = True
                    Else
                        ValidateMainUser = False
                    End If
                End If
            Else
                ValidateMainUser = False
            End If
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Catch ex As Exception
            ValidateMainUser = False
        End Try
    End Function
#End Region


#Region "Public Function ValidateSubUser(ByVal constr As String,ByVal strUserCode As String, ByVal strUserName As String, ByVal strPassword As String) As Boolean"
    Public Function ValidateSubUser(ByVal constr As String, ByVal strUserCode As String, ByVal strUserName As String,
                                    ByVal strPassword As String) As Boolean
        ValidateSubUser = False
        Try
            'strSqlQry = "SELECT agentcode,agent_sub_code,dbo.pwddecript(pass_word) pass_word FROM agents_subusers WHERE active=1 and  agentcode='" & strUserCode & "' AND agent_sub_code='" & strUserName & "' AND dbo.pwddecript(pass_word)='" & strPassword & "'"
            strSqlQry = "SELECT webusername ,agent_sub_code,dbo.pwddecript(pass_word) pass_word FROM agents_subusers inner join agentmast on agents_subusers .AGENTCODE =agentmast .agentcode  WHERE agents_subusers.active=1 and  webusername='" & strUserCode & "' AND agent_sub_code='" & strUserName & "' AND dbo.pwddecript(pass_word)='" & strPassword & "'"
            SqlConn = clsDBConnect.dbConnectionnew(constr)                'connection open
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataReader = myCommand.ExecuteReader
            If myDataReader.HasRows Then
                If myDataReader.Read Then
                    If StrComp(myDataReader("webusername"), strUserCode, CompareMethod.Binary) = 0 And _
                       StrComp(myDataReader("agent_sub_code"), strUserName, CompareMethod.Binary) = 0 And _
                       StrComp(myDataReader("pass_word"), strPassword, CompareMethod.Binary) = 0 Then
                        ValidateSubUser = True
                    Else
                        ValidateSubUser = False
                    End If
                End If
            Else
                ValidateSubUser = False
            End If
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Catch ex As Exception
            ValidateSubUser = False
        End Try
    End Function
#End Region

    Public Function PostUnpostRight(ByVal constr As String, ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String) As Boolean
        Dim intGroupID As Integer
        Dim intAppID As Integer
        Dim intMenuID As Integer
        Dim strUserFunctionalRight As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim lngCount As Int16
        Dim strGetUserFunctionalRightValue As String
        ' Dim PostUnpostFlag As Boolean = False

        If CType(UserName, String) = "" Or CType(UserPwd, String) = "" Then
            Exit Function
        Else
            intGroupID = GetGroupId(constr, CType(UserName, String), CType(UserPwd, String))
        End If
        If CType(AppName, String) = "" Or CType(AppName, String) = Nothing Then
            Exit Function
        Else
            intAppID = GetAppId(constr, CType(AppName, String))
        End If
        intMenuID = GetMenuId(constr, PageName, intAppID)

        If Val(intGroupID) = 0 And Val(intAppID) = 0 And Val(intMenuID) = 0 Then
        Else
            strUserFunctionalRight = GetUserFunctionalRight(constr, intGroupID, intAppID, intMenuID)
            If strUserFunctionalRight <> "" Then
                strTempUserFunctionalRight = strUserFunctionalRight.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strGetUserFunctionalRightValue = strTempUserFunctionalRight.GetValue(lngCount)
                    If CType(strGetUserFunctionalRightValue, String) = "08" Or CType(strGetUserFunctionalRightValue, String) = "8" Then
                        PostUnpostRight = True
                    End If
                Next
            Else
                PostUnpostRight = False
            End If
        End If
    End Function
    Public Function PostUnpostRightnew(ByVal constr As String, ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, ByVal appid As String) As Boolean
        Dim intGroupID As Integer
        Dim intAppID As Integer
        Dim intMenuID As Integer
        Dim strUserFunctionalRight As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim lngCount As Int16
        Dim strGetUserFunctionalRightValue As String
        ' Dim PostUnpostFlag As Boolean = False

        If CType(UserName, String) = "" Or CType(UserPwd, String) = "" Then
            Exit Function
        Else
            intGroupID = GetGroupId(constr, CType(UserName, String), CType(UserPwd, String))
        End If
        If CType(AppName, String) = "" Or CType(AppName, String) = Nothing Then
            Exit Function
        Else
            '  intAppID = GetAppId(constr, CType(AppName, String))
            intAppID = appid
        End If
        intMenuID = GetMenuId(constr, PageName, intAppID)

        If Val(intGroupID) = 0 And Val(intAppID) = 0 And Val(intMenuID) = 0 Then
        Else
            strUserFunctionalRight = GetUserFunctionalRight(constr, intGroupID, intAppID, intMenuID)
            If strUserFunctionalRight <> "" Then
                strTempUserFunctionalRight = strUserFunctionalRight.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strGetUserFunctionalRightValue = strTempUserFunctionalRight.GetValue(lngCount)
                    If CType(strGetUserFunctionalRightValue, String) = "08" Or CType(strGetUserFunctionalRightValue, String) = "8" Then
                        PostUnpostRightnew = True
                    End If
                Next
            Else
                PostUnpostRightnew = False
            End If
        End If
    End Function
#Region "Public Function GetAppName(ByVal constr As String,ByVal strAppName As String) As Integer"
    Public Function GetAppName(ByVal constr As String, ByVal strAppId As String) As String
        Try
            strSqlQry = ""
            strSqlQry = "SELECT appname FROM appmaster WHERE appid='" & strAppId & "'"
            GetAppName = objUtils.ExecuteQueryReturnStringValuenew(constr, strSqlQry)
        Catch ex As Exception
            GetAppName = ""
        End Try
    End Function
#End Region

    Public Function CheckAgentsLogin(ByVal SaveOrCheck As Integer, ByVal AgentCode As String, ByVal AgentSubCode As String, ByVal CountryLogin As String, ByVal ConnectionString As String) As String
        Try
            'Dim ConnectionString As String = String.Empty
            Dim Conn As New SqlConnection
            'ConnectionString = "Data Source=188.244.109.2;Initial Catalog=Master;Persist Security Info=true;User Id=sa;Pwd=ADM@hce"
            'ConnectionString = "Data Source=NVENKAT-VAIO\SQLSERVERR2;Initial Catalog=Woninfinstall;Persist Security Info=true;User Id=sa;Pwd=admin"
            Dim LoginIp As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            Conn = New SqlConnection(ConnectionString)
            Conn.Open()
            Dim Cmd As New SqlCommand
            Dim Str As String = String.Empty
            Dim Dr As SqlDataReader
            Dim MyDate As Date
            If SaveOrCheck = 1 Then
                If AgentSubCode = String.Empty Then
                    Str = "Select AgentCode,AgentLoginDate From Tbl_CheckAgents_Login Where AgentCode='" & AgentCode & "'"
                Else
                    Str = "Select AgentCode,AgentLoginDate From Tbl_CheckAgents_Login Where AgentCode='" & AgentSubCode & "' And AgentSubCode='" & AgentCode & "'"
                End If

                Cmd = New SqlCommand(Str, Conn)
                Dr = Cmd.ExecuteReader
                If Dr.Read Then
                    If Not IsDBNull(Dr("AgentLoginDate")) Then
                        MyDate = Dr("AgentLoginDate")
                    End If
                    Dr.Close()
                    Cmd.Cancel()
                    If AgentSubCode = String.Empty Then
                        Str = "Update  Tbl_CheckAgents_Login Set AgentLoginDate='" & Format(CDate(Date.Today), "yyyy/MM/dd") & "',AgentLoginFlag=1,AgentLastLoginDate='" & Format(CDate(MyDate), "yyyy/MM/dd") & "',CountryLogin='" & CountryLogin & "',LoginIp ='" & LoginIp & "'  Where AgentCode='" & AgentCode & "'"
                    Else
                        Str = "Update  Tbl_CheckAgents_Login Set AgentLoginDate='" & Format(CDate(Date.Today), "yyyy/MM/dd") & "',AgentLoginFlag=1,AgentLastLoginDate='" & Format(CDate(MyDate), "yyyy/MM/dd") & "',CountryLogin='" & CountryLogin & "',LoginIp ='" & LoginIp & "'  Where AgentCode='" & AgentSubCode & "' And AgentSubCode='" & AgentCode & "'"
                    End If
                    Cmd = New SqlCommand(Str, Conn)
                    Cmd.ExecuteNonQuery()
                    Cmd.Cancel()
                    Conn.Close()
                    Return ""
                Else
                    Dr.Close()
                    Cmd.Cancel()
                    If AgentSubCode = String.Empty Then
                        Str = "Insert Into Tbl_CheckAgents_Login(AgentCode,AgentSubCode,AgentLoginDate,AgentLoginFlag,AgentLastLoginDate,CountryLogin,LoginIp)Values('" & AgentCode & "','" & AgentSubCode & "','" & Format(CDate(Date.Today), "yyyy/MM/dd") & "',1,'" & Format(CDate(Date.Today), "yyyy/MM/dd") & "','" & CountryLogin & "','" & LoginIp & "')"
                    Else
                        Str = "Insert Into Tbl_CheckAgents_Login(AgentCode,AgentSubCode,AgentLoginDate,AgentLoginFlag,AgentLastLoginDate,CountryLogin,LoginIp)Values('" & AgentSubCode & "','" & AgentCode & "','" & Format(CDate(Date.Today), "yyyy/MM/dd") & "',1,'" & Format(CDate(Date.Today), "yyyy/MM/dd") & "','" & CountryLogin & "','" & LoginIp & "')"
                    End If

                    Cmd = New SqlCommand(Str, Conn)
                    Cmd.ExecuteNonQuery()
                    Cmd.Cancel()
                    Conn.Close()
                    Return ""
                End If
            Else
                If AgentSubCode = String.Empty Then
                    Str = "Select AgentCode,AgentLoginDate From Tbl_CheckAgents_Login Where AgentCode='" & AgentCode & "' And AgentLoginFlag=1"
                Else
                    Str = "Select AgentCode,AgentLoginDate From Tbl_CheckAgents_Login Where AgentCode='" & AgentSubCode & "' And AgentSubCode='" & AgentCode & "' And AgentLoginFlag=1"
                End If
                Cmd = New SqlCommand(Str, Conn)
                Dr = Cmd.ExecuteReader
                If Dr.Read Then
                    If AgentSubCode = String.Empty Then
                        Str = "Update  Tbl_CheckAgents_Login Set AgentLoginFlag=0  Where AgentCode='" & AgentCode & "'"
                    Else
                        Str = "Update  Tbl_CheckAgents_Login Set AgentLoginFlag=0  Where AgentCode='" & AgentSubCode & "' And AgentSubCode='" & AgentCode & "'"
                    End If
                    Dr.Close()
                    Cmd.Cancel()
                    Cmd = New SqlCommand(Str, Conn)
                    Cmd.ExecuteNonQuery()
                    HttpContext.Current.Session("LoginCorrect") = "LoginCorrect"
                    Cmd.Cancel()
                    Conn.Close()
                    Return ""
                Else
                    If HttpContext.Current.Session("LoginCorrect") = "LoginCorrect" Then
                        Conn.Close()
                        Return ""
                    End If
                    Return "You Have No Right To Access This Page Please Login In"
                End If
            End If
            Cmd.Cancel()
            Conn.Close()
            Return ""
        Catch ex As Exception
            HttpContext.Current.Session("LoginCorrect") = ""
            Return "Error Login"
        End Try
    End Function
End Class


