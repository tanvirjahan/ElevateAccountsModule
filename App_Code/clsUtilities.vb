Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.IO
Imports System.Net.Mail
Imports System.Web.HttpServerUtility
Imports System.Security.Cryptography
Imports System.Collections.Generic

Public Class clsUtilities

    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim Path As String = ConfigurationManager.AppSettings("AppLogs").ToString
    Dim sqlTrans As SqlTransaction
    ''' <summary>
    ''' GetDataFromDataset
    ''' </summary>
    ''' <param name="strSqlQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataFromDataset(ByVal strSqlQuery As String) As DataSet
        Dim ds As New DataSet
        Try
            SqlConn = clsDBConnect.dbConnection()                    'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQuery, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(ds)
            GetDataFromDataset = ds
        Catch ex As Exception
            GetDataFromDataset = Nothing
        Finally
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function
    Public Function GenerateXML(ByVal ds As DataSet) As String
        Dim obj As New StringWriter()
        Dim xmlstring As String
        ds.WriteXml(obj)
        xmlstring = obj.ToString().Replace("NewDataSet", "DocumentElement")
        Return xmlstring
    End Function

    Public Function GenerateXML_FromDataTable(ByVal dt As DataTable) As String
        Dim obj As New StringWriter()
        Dim xmlstring As String
        dt.WriteXml(obj)
        xmlstring = obj.ToString()
        Return xmlstring
    End Function

    Public Function GetDataFromDataTable(ByVal strSqlQuery As String) As DataTable
        Dim dt As New DataTable
        Try
            SqlConn = clsDBConnect.dbConnection()                    'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQuery, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(dt)
            GetDataFromDataTable = dt
        Catch ex As Exception
            GetDataFromDataTable = Nothing
        Finally
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function
    Public Shared Function GetSharedDataFromDataTable(ByVal strSqlQuery As String) As DataTable
        Dim dt As New DataTable
        Dim SqlConnShared As New SqlConnection
        Try

            Dim myDataAdapter As New SqlDataAdapter
            SqlConnShared = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQuery, SqlConnShared)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(dt)
            GetSharedDataFromDataTable = dt
        Catch ex As Exception
            GetSharedDataFromDataTable = Nothing
        Finally
            clsDBConnect.dbConnectionClose(SqlConnShared)                 'Close connection
        End Try
    End Function

    Public Shared Function GetSharedDataFromDataSet(ByVal strSqlQuery As String) As DataSet ' Added by abin on 20180621
        Dim ds As New DataSet
        Dim SqlConnShared As New SqlConnection
        Try

            Dim myDataAdapter As New SqlDataAdapter
            SqlConnShared = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQuery, SqlConnShared)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(ds)
            GetSharedDataFromDataSet = ds
        Catch ex As Exception
            GetSharedDataFromDataSet = Nothing
        Finally
            clsDBConnect.dbConnectionClose(SqlConnShared)                 'Close connection
        End Try
    End Function

    ''' <summary>
    ''' GetDataFromReader
    ''' </summary>
    ''' <param name="strSqlQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataFromReader(ByVal strSqlQuery As String) As SqlDataReader
        Try
            SqlConn = clsDBConnect.dbConnection()               'Open connection
            myCommand = New SqlCommand(strSqlQuery, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            GetDataFromReader = myDataReader
        Catch ex As Exception
            GetDataFromReader = Nothing
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function
    ''' <summary>
    ''' ExecuteQueryReturnSingleValue
    ''' </summary>
    ''' <param name="strQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecuteQueryReturnSingleValue(ByVal strQuery As String) As Object
        Dim objAgrValue As Object
        Try
            SqlConn = clsDBConnect.dbConnection()                     'Open connection
            myCommand = New SqlCommand(strQuery, SqlConn)
            objAgrValue = myCommand.ExecuteScalar
            If IsDBNull(objAgrValue) = False Then
                ExecuteQueryReturnSingleValue = objAgrValue
            Else
                ExecuteQueryReturnSingleValue = 0
            End If
        Catch ex As Exception
            ExecuteQueryReturnSingleValue = 0
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function
    ''' <summary>
    ''' ExecuteQueryReturnStringValue
    ''' </summary>
    ''' <param name="strQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecuteQueryReturnStringValue(ByVal strQuery As String) As String
        Dim objAgrValue As String
        Try
            SqlConn = clsDBConnect.dbConnection()                        'Open connection
            myCommand = New SqlCommand(strQuery, SqlConn)
            objAgrValue = myCommand.ExecuteScalar
            If IsDBNull(objAgrValue) = False Then
                ExecuteQueryReturnStringValue = objAgrValue
            Else
                ExecuteQueryReturnStringValue = ""
            End If
        Catch ex As Exception
            ExecuteQueryReturnStringValue = ""
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SharedExecuteQueryReturnStringValue(ByVal strQuery As String) As String
        Dim objAgrValue As String
        Dim SqlConn As New SqlConnection
        Dim myCommand As New SqlCommand
        Try

            SqlConn = clsDBConnect.dbConnection()                        'Open connection
            myCommand = New SqlCommand(strQuery, SqlConn)
            objAgrValue = myCommand.ExecuteScalar
            If IsDBNull(objAgrValue) = False Then
                SharedExecuteQueryReturnStringValue = objAgrValue
                If SharedExecuteQueryReturnStringValue.Contains("-") Then
                    SharedExecuteQueryReturnStringValue = ""
                End If
            Else
                SharedExecuteQueryReturnStringValue = ""
            End If
        Catch ex As Exception
            SharedExecuteQueryReturnStringValue = ""
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try


    End Function

    ''' <summary>
    ''' MessageBox
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    Public Sub MessageBox(ByVal msg As String, ByVal page As Object)

        ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "", "alert('" & msg & "'  );", True)
    End Sub
    ''' <summary>
    ''' GetDetailsPageWise
    ''' </summary>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="strQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDetailsPageWise(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal strQuery As String) As DataSet
        Dim strConnName As String = "strDBConnection"
        Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("[GetDetailsPageWise]")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex)
                cmd.Parameters.AddWithValue("@PageSize", pageSize)
                cmd.Parameters.AddWithValue("@SqlQuery", strQuery)
                cmd.Parameters.Add("@PageCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        sda.Fill(ds, "Customers")
                        Dim dt As New DataTable("PageCount")
                        dt.Columns.Add("PageCount")
                        dt.Rows.Add()
                        dt.Rows(0)(0) = cmd.Parameters("@PageCount").Value
                        ds.Tables.Add(dt)
                        Return ds
                    End Using
                End Using
            End Using
        End Using
    End Function

    Public Function checkForAgentDuplicate(ByVal strCompany As String, ByVal strTel As String, ByVal strFax As String, ByVal strEmail As String, ByVal strRegNo As String) As String()
        Dim strValue As String = ""
        Dim ErrMsg As String = ""
        Dim Email As String = ""
        Dim contact As String = ""
        Dim strValidate(2) As String

        SqlConn = clsDBConnect.dbConnection()
        myCommand = New SqlCommand("sp_validate_registrantionform", SqlConn)
        myCommand.CommandType = CommandType.StoredProcedure


        If (strCompany = "") = False Then
            myCommand.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 100)).Value = CType(strCompany.Trim, String)
        Else
            myCommand.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 100)).Value = String.Empty
        End If
        If (strTel = "") = False Then
            myCommand.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(strTel.Trim, String)
        Else
            myCommand.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = String.Empty
        End If
        If (strFax = "") = False Then
            myCommand.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(strFax.Trim, String)
        Else
            myCommand.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = String.Empty
        End If
        If (strEmail = "") = False Then
            myCommand.Parameters.Add(New SqlParameter("@agent_email", SqlDbType.VarChar, 100)).Value = CType(strEmail.Trim, String)
        Else
            myCommand.Parameters.Add(New SqlParameter("@agent_email", SqlDbType.VarChar, 100)).Value = String.Empty
        End If
        If (strRegNo = "") = False Then
            myCommand.Parameters.Add(New SqlParameter("@RegNo", SqlDbType.VarChar, 50)).Value = CType(strRegNo.Trim, String)
        Else
            myCommand.Parameters.Add(New SqlParameter("@RegNo", SqlDbType.VarChar, 50)).Value = String.Empty
        End If

        Dim paramErrMsg As New SqlParameter
        paramErrMsg.ParameterName = "@errmsg"
        paramErrMsg.Direction = ParameterDirection.Output
        paramErrMsg.DbType = DbType.String
        paramErrMsg.Size = 200
        myCommand.Parameters.Add(paramErrMsg)

        Dim paramEmail As New SqlParameter
        paramEmail.ParameterName = "@email"
        paramEmail.Direction = ParameterDirection.Output
        paramEmail.DbType = DbType.String
        paramEmail.Size = 100
        myCommand.Parameters.Add(paramEmail)

        Dim paramContact As New SqlParameter
        paramContact.ParameterName = "@contactperson"
        paramContact.Direction = ParameterDirection.Output
        paramContact.DbType = DbType.String
        paramContact.Size = 100
        myCommand.Parameters.Add(paramContact)

        myCommand.ExecuteNonQuery()

        If Not paramErrMsg Is Nothing Then
            ErrMsg = paramErrMsg.Value
        End If
        If Not paramEmail Is Nothing Then
            Email = paramEmail.Value
        End If
        If Not paramContact Is Nothing Then
            contact = paramContact.Value
        End If

        myCommand = Nothing

        '     clsDBConnect.dbConnectionClose(mySqlConn)           'connection close


        ' If ErrMsg <> "" Then
        strValidate(0) = ErrMsg
        strValidate(1) = Email
        strValidate(2) = contact
        'End If


        Return strValidate
    End Function

    ''' <summary>
    ''' Function to execute a Stored Procedure
    ''' </summary>
    ''' <param name="spName">SQL Command Text</param>
    ''' <returns>DataTable filled with the queried Results</returns>
    ''' <remarks></remarks>

    Public Function GetDataTable(ByVal spName As String, ByVal ParamArray sqlParams() As SqlParameter) As DataTable

        Dim _strConn As String = clsDBConnect.ConnectionString()
        Dim dataTable As DataTable = Nothing
        Dim filledRows As Integer = 0
        If spName.Trim.Length > 0 AndAlso _strConn.Trim.Length > 0 Then
            Using sqlCn As New SqlConnection(_strConn)
                Using sqlCmd As SqlCommand = sqlCn.CreateCommand
                    With sqlCmd
                        .CommandText = spName
                        .CommandType = CommandType.StoredProcedure
                        .Connection = sqlCn
                        .CommandTimeout = 0
                        If (sqlParams IsNot Nothing) AndAlso (sqlParams.Length > 0) Then .Parameters.AddRange(sqlParams)
                    End With
                    Using sqlDa As New SqlDataAdapter(sqlCmd)
                        Try
                            dataTable = New DataTable
                            filledRows = sqlDa.Fill(dataTable)
                        Catch sqlEx As SqlException
                            For Each sqlE As SqlError In sqlEx.Errors
                                WriteErrorLog(sqlE.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
                            Next
                            dataTable = Nothing
                        Catch ex As Exception
                            WriteErrorLog(ex.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
                            dataTable = Nothing

                        Finally
                            sqlCmd.Parameters.Clear()
                            spName = String.Empty
                            sqlParams = Nothing
                            sqlCn.Close()
                            sqlCn.Dispose()
                        End Try
                    End Using
                End Using
            End Using
        Else
            Return dataTable
        End If
        Return dataTable
    End Function

    Public Function GetDataSet(ByVal spName As String, ByVal ParamArray sqlParams() As SqlParameter) As DataSet

        Dim _strConn As String = clsDBConnect.ConnectionString()
        Dim dataSet As DataSet = Nothing
        Dim filledRows As Integer = 0
        If spName.Trim.Length > 0 AndAlso _strConn.Trim.Length > 0 Then
            Using sqlCn As New SqlConnection(_strConn)
                Using sqlCmd As SqlCommand = sqlCn.CreateCommand
                    With sqlCmd
                        .CommandText = spName
                        .CommandType = CommandType.StoredProcedure
                        .Connection = sqlCn
                        .CommandTimeout = 0
                        If (sqlParams IsNot Nothing) AndAlso (sqlParams.Length > 0) Then .Parameters.AddRange(sqlParams)
                    End With
                    Using sqlDa As New SqlDataAdapter(sqlCmd)
                        Try
                            dataSet = New DataSet
                            filledRows = sqlDa.Fill(dataSet)
                        Catch sqlEx As SqlException
                            For Each sqlE As SqlError In sqlEx.Errors
                                WriteErrorLog(sqlE.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
                            Next
                            dataSet = Nothing
                        Catch ex As Exception
                            WriteErrorLog(ex.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
                            dataSet = Nothing

                        Finally
                            sqlCmd.Parameters.Clear()
                            spName = String.Empty
                            sqlParams = Nothing
                            sqlCn.Close()
                            sqlCn.Dispose()
                        End Try
                    End Using
                End Using
            End Using
        Else
            Return dataSet
        End If
        Return dataSet
    End Function

    ''' <summary>
    ''' WriteErrorLog
    ''' </summary>
    ''' <param name="StrMsgText"></param>
    ''' <remarks></remarks>
    Public Sub WriteErrorLog(ByVal StrMsgText As String)
        Try
            Dim SW As StreamWriter
            Dim StrErrTime As String = ""
            Dim StrFileName As String = ""
            StrFileName = Path & Format(Today.Year, "0000") & Format(Today.Month, "00") & Format(Today.Day, "00") & ".SV"
            If Not Directory.Exists(Path) Then
                Directory.CreateDirectory(Path)
            End If
            StrErrTime = Format(DateTime.Now) ' & ":" & Format(Now.Hour, "00") & ":" & Format(Now.Minute, "00") & ":" & Format(Now.Second, "00")
            If Dir(StrFileName) = "" Then SW = File.CreateText(StrFileName) Else SW = File.AppendText(StrFileName)
            SW.Write(StrErrTime & ":" & StrMsgText)
            SW.Write(vbNewLine)
            SW.Close()
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' AddUpdateDeleteSQL
    ''' </summary>
    ''' <param name="strSql"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddUpdateDeleteSQL(ByVal strSql As String) As Integer
        Dim _strConn As String = clsDBConnect.ConnectionString()
        If strSql.Trim.Length > 0 AndAlso _strConn.Trim.Length > 0 Then
            Dim affectedRows As Integer = 0
            Using sqlCn As New SqlConnection(_strConn)
                Using sqlCmd As New SqlCommand
                    With sqlCmd
                        .CommandText = strSql
                        .CommandType = CommandType.Text
                        .Connection = sqlCn
                    End With
                    Try
                        If sqlCn.State = ConnectionState.Closed Then sqlCn.Open()
                        affectedRows = sqlCmd.ExecuteNonQuery()
                    Catch sqlEx As SqlException
                        For Each sqlE As SqlError In sqlEx.Errors
                            WriteErrorLog(sqlE.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
                        Next
                        Return -1
                    Catch ex As Exception
                        WriteErrorLog(ex.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
                        Return -1
                    Finally
                        If sqlCn.State = ConnectionState.Open Then
                            sqlCn.Close()
                            sqlCn.Dispose()
                        End If

                        strSql = String.Empty
                    End Try
                End Using
            End Using
            If affectedRows > 0 Then
                Return 0
            Else
                Return -1
            End If
        Else
            Return -1
        End If
    End Function

    Public Function ExecuteNonQuerynew(ByVal constr As String, ByVal storedProcedure As String, ByVal sqlParamList As List(Of SqlParameter), ByVal conn As SqlConnection, ByVal trans As SqlTransaction) As Integer
        'Open connection
        myCommand = New SqlCommand()
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = storedProcedure
        myCommand.Connection = conn
        myCommand.Transaction = trans
        Dim size As Integer
        Dim i As Integer
        size = sqlParamList.Count
        For i = 0 To size - 1
            myCommand.Parameters.Add(sqlParamList(i))
        Next
        Dim Norows As New Integer
        myCommand.CommandTimeout = 0
        Norows = myCommand.ExecuteNonQuery()

        Return Norows
    End Function

    ''' <summary>
    ''' ExecuteNonQuery_Param
    ''' </summary>
    ''' <param name="storedProcedure"></param>
    ''' <param name="sqlParamList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecuteNonQuery_Param(ByVal storedProcedure As String, ByVal sqlParamList As List(Of SqlParameter)) As Integer
        Dim Norows As New Integer
        Try
            'Open connection
            SqlConn = clsDBConnect.dbConnection()
            sqlTrans = SqlConn.BeginTransaction()
            '   myCommand = New SqlCommand()
            myCommand = New SqlCommand(storedProcedure, SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            'myCommand.CommandText = storedProcedure
            'myCommand.Connection = SqlConn
            'myCommand.Transaction = sqlTrans
            Dim size As Integer
            Dim i As Integer
            size = sqlParamList.Count
            For i = 0 To size - 1
                myCommand.Parameters.Add(sqlParamList(i))
            Next

            Norows = myCommand.ExecuteNonQuery()
            sqlTrans.Commit()
            Return Norows

        Catch ex As Exception
            sqlTrans.Rollback()
            WriteErrorLog(ex.StackTrace.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
            Return -1
        Finally
            sqlTrans.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' GetRandomPassword
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetRandomPassword() As String
        Dim strPassword As String = ""
        Try
            Dim myDataAdapter As SqlDataAdapter
            Dim SqlConn As SqlConnection = clsDBConnect.dbConnection()

            myCommand = New SqlCommand("GenerateRandomString", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add(New SqlParameter("@useNumbers", SqlDbType.Bit)).Value = 1
            myCommand.Parameters.Add(New SqlParameter("@useLowerCase", SqlDbType.Bit)).Value = 0
            myCommand.Parameters.Add(New SqlParameter("@useUpperCase", SqlDbType.Bit)).Value = 1
            myCommand.Parameters.Add(New SqlParameter("@charactersToUse", SqlDbType.VarChar, 100)).Value = System.DBNull.Value
            myCommand.Parameters.Add(New SqlParameter("@passwordLength", SqlDbType.SmallInt, 9)).Value = 7

            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@password"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            myCommand.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            Return param.Value

        Catch ex As Exception
            WriteErrorLog("ClsUtilities :: " & Reflection.MethodBase.GetCurrentMethod.Name & " :: " & ex.Message.ToString)
            Return strPassword
        End Try
    End Function
    '*** Danny 25/09/2018
    Function GetNumberofdaystoservicedate(ByVal Requestid As String) As String
        Dim strPassword As String = ""
        Try
            Dim myDataAdapter As SqlDataAdapter
            Dim SqlConn As SqlConnection = clsDBConnect.dbConnection()

            myCommand = New SqlCommand("SP_CheckNoofServiceDays", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar)).Value = Requestid

            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@RetunString"
            param.Direction = ParameterDirection.Output
            param.DbType = SqlDbType.VarChar
            param.Size = 1000
            myCommand.Parameters.Add(param)

            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            Return param.Value.ToString().Trim()

        Catch ex As Exception
            WriteErrorLog("ClsUtilities :: " & Reflection.MethodBase.GetCurrentMethod.Name & " :: " & ex.Message.ToString)
            Return strPassword
        End Try
    End Function

    Public Sub PwdSendmailLog_Entry(ByVal prm_agentcode As String, ByVal prm_stremails As String, ByVal prm_pagename As String, ByVal Username As String)
        Try
            Dim GlobalUserName As String = Nothing
            SqlConn = clsDBConnect.dbConnection()         'connection open
            Dim sqlTrans As SqlTransaction = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_add_PwdMailSend_Log", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(prm_agentcode, String)
            myCommand.Parameters.Add(New SqlParameter("@mailDateTime", SqlDbType.DateTime)).Value = CType(System.DateTime.Now, DateTime)
            myCommand.Parameters.Add(New SqlParameter("@mailSendPageName", SqlDbType.VarChar, 200)).Value = prm_pagename.ToString

            myCommand.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 20)).Value = IIf(GlobalUserName Is Nothing, Username, GlobalUserName)

            myCommand.Parameters.Add(New SqlParameter("@agent_email", SqlDbType.VarChar, 50)).Value = CType(prm_stremails.Trim, String)

            myCommand.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbSqlTransation(sqlTrans)
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            WriteErrorLog("ClsUtilities :: " & Reflection.MethodBase.GetCurrentMethod.Name & " :: " & ex.Message.ToString)
        End Try
    End Sub
    ''' <summary>
    ''' LoadTheme
    ''' </summary>
    ''' <param name="strCompany"></param>
    ''' <param name="htmlLink"></param>
    ''' <remarks></remarks>
    Sub LoadTheme(ByVal strCompany As String, ByVal htmlLink As HtmlLink)
        If Not strCompany Is Nothing Then

            If strCompany = "924065660726315" Then 'AgentsOnlineCommon
                htmlLink.Attributes("href") = "css/style-style1.css"
            ElseIf strCompany = "675558760549078" Then 'AgentsOnlineCommon1
                htmlLink.Attributes("href") = "css/style-style2.css"
            Else
                htmlLink.Attributes("href") = "css/style-common.css"
            End If
        Else
            htmlLink.Attributes("href") = "css/style-common.css"
        End If
    End Sub

    Public Sub FillDropDownList(ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal strQry As String, ByVal DataValueField As String, ByVal DataTextField As String, Optional ByVal DefaultFlag As Boolean = False, Optional ByVal DefaultFlagValue As String = "")
        Try
            SqlConn = clsDBConnect.dbConnection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If DefaultFlag = True Then
                ddl.AppendDataBoundItems = True
                ddl.Items.Add(New ListItem(DefaultFlagValue, DefaultFlagValue))
            End If
            With ddl
                .DataSource = myDataReader
                .DataTextField = DataTextField
                .DataValueField = DataValueField
                .DataBind()
            End With


        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Sub
    Public Sub FillDropDownListBasedOnNumber(ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal iNumber As Integer)
        If ddl.Items.Count > 0 Then
            For i As Integer = 0 To ddl.Items.Count - 1
                ddl.Items.Remove(i)
            Next

        End If
        ddl.Items.Add(New ListItem("--", "0"))
        For i As Integer = 1 To iNumber
            ddl.Items.Add(New ListItem(i.ToString, i.ToString))
        Next
    End Sub

    Public Sub FillCheckBoxList(ByVal chkl As System.Web.UI.WebControls.CheckBoxList, ByVal strQry As String, ByVal DataValueField As String, ByVal DataTextField As String, Optional ByVal DefaultFlag As Boolean = False, Optional ByVal DefaultFlagValue As String = "")
        Try
            SqlConn = clsDBConnect.dbConnection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If DefaultFlag = True Then
                chkl.AppendDataBoundItems = True
                chkl.Items.Add(New ListItem(DefaultFlagValue, DefaultFlagValue))
            End If
            With chkl
                .DataSource = myDataReader
                .DataTextField = DataTextField
                .DataValueField = DataValueField
                .DataBind()
            End With


        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Sub

    Sub FillDropDownListWithSpecifiedAges(ByVal dropDownList As DropDownList, ByVal childAges As String)

        If dropDownList.Items.Count > 0 Then
            For i As Integer = 0 To dropDownList.Items.Count - 1
                dropDownList.Items.Remove(i)
            Next

        End If
        '  dropDownList.Items.Clear()
        '  dropDownList.Items.Add(New ListItem("--", "0"))

        Dim childAgesArray As String() = childAges.Split(";")

        For i As Integer = 0 To childAgesArray.Length - 1
            dropDownList.Items.Add(New ListItem(childAgesArray(i).ToString, childAgesArray(i).ToString))
        Next
    End Sub

    Public Function Encrypt(ByVal clearText As String) As String
        Dim EncryptionKey As String = "ABIN@CRYPTOGRAPHY"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H4D, &H61, &H68, &H63, &H65, &H20, &H56, &H65, &H6E, &H6B, &H61, &H74, &H72, _
                                                                         &H61, &H6D, &H61, &H6E, &H20, &H44, &H75, &H62, &H61, &H69})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
                clearText = HttpUtility.UrlEncode(clearText)
            End Using
        End Using
        Return clearText
    End Function

    Public Function Decrypt(ByVal cipherText As String) As String
        Dim EncryptionKey As String = "ABIN@CRYPTOGRAPHY"
        cipherText = HttpUtility.UrlDecode(cipherText)
        cipherText = cipherText.Replace(" ", "+")
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H4D, &H61, &H68, &H63, &H65, &H20, &H56, &H65, &H6E, &H6B, &H61, &H74, &H72, _
                                                                         &H61, &H6D, &H61, &H6E, &H20, &H44, &H75, &H62, &H61, &H69})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
    Public Function SendEmailNotification(ByVal strEmailType As String, ByVal strFromEmail As String, ByVal strToEmail As String, ByVal strToCc As String, ByVal strToBcc As String, ByVal strEmailSubject As String, ByVal strEmailBody As String, ByVal strIsHtml As String, ByVal strIsAttachment As String, ByVal strAttachment As String, ByVal strSentStatus As String, ByVal strRequestId As String, ByVal strRemarks As String) As Boolean
        Try
            SqlConn = clsDBConnect.dbConnection()         'connection open
            Dim sqlTrans As SqlTransaction = SqlConn.BeginTransaction
            myCommand = New SqlCommand("Sp_AgentOnline_EmailNotification", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@EmailType", SqlDbType.VarChar, 200)).Value = CType(strEmailType, String)
            myCommand.Parameters.Add(New SqlParameter("@FromEmail", SqlDbType.VarChar, 200)).Value = CType(strFromEmail, String)
            myCommand.Parameters.Add(New SqlParameter("@ToEmail", SqlDbType.VarChar, 200)).Value = strToEmail.ToString
            myCommand.Parameters.Add(New SqlParameter("@ToCc", SqlDbType.VarChar, 5000)).Value = strToCc.ToString
            myCommand.Parameters.Add(New SqlParameter("@ToBcc", SqlDbType.VarChar, 5000)).Value = strToBcc.ToString
            myCommand.Parameters.Add(New SqlParameter("@EmailSubject", SqlDbType.VarChar, 8000)).Value = strEmailSubject.ToString

            myCommand.Parameters.Add(New SqlParameter("@EmailBody", SqlDbType.VarChar, -1)).Value = strEmailBody.ToString
            myCommand.Parameters.Add(New SqlParameter("@IsHtml", SqlDbType.VarChar, 20)).Value = strIsHtml.ToString
            myCommand.Parameters.Add(New SqlParameter("@IsAttachment", SqlDbType.VarChar, 20)).Value = strIsAttachment.ToString
            myCommand.Parameters.Add(New SqlParameter("@Attachment", SqlDbType.VarChar, 8000)).Value = strAttachment.ToString
            myCommand.Parameters.Add(New SqlParameter("@SentStatus", SqlDbType.VarChar, 200)).Value = strSentStatus.ToString
            myCommand.Parameters.Add(New SqlParameter("@RequestId", SqlDbType.VarChar, 200)).Value = strRequestId.ToString
            myCommand.Parameters.Add(New SqlParameter("@Remarks", SqlDbType.VarChar, 8000)).Value = strRemarks.ToString

            myCommand.ExecuteNonQuery()
            sqlTrans.Commit()
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)
            Return True
            'clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbSqlTransation(sqlTrans)
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            WriteErrorLog("ClsUtilities :: " & Reflection.MethodBase.GetCurrentMethod.Name & " :: " & ex.Message.ToString)
            Return False
        End Try
    End Function

    Sub SaveEmailLog(ByVal strRequestId As String, ByVal strSentTo As String, ByVal strPartyCode As String, ByVal strAmended As String, ByVal strCancelled As String, ByVal strSentToAgent As Object, ByVal strSentToHotel As Object, ByVal strLoggedUser As String)
        Dim strQuery As String = "insert into AgentOnline_EmailLog (RequestId,SentTo,PartyCode,Amended,Cancelled,SentToAgent,SentToHotel,LoggedUser,UpdatedDate) values ('" & strRequestId & "','" & strSentTo & "','" & strPartyCode & "','" & strAmended & "','" & strCancelled & "','" & strSentToAgent & "','" & strSentToHotel & "','" & strLoggedUser & "',getdate())"
        Dim iStatus As Integer = AddUpdateDeleteSQL(strQuery)
    End Sub

    Function CheckEmailConfirm(ByVal strCode As String) As String
        Dim objclsUtilities As New clsUtilities()
        Dim strDCode As String = objclsUtilities.Decrypt(strCode)
        Dim strQuery As String = "select isnull(EmailConfirm,0)EmailConfirm from agents_subusers where AgentSubCode='" & strDCode & "'"
        Dim strEmailConfirm As String = ExecuteQueryReturnStringValue(strQuery)
        Return strEmailConfirm
    End Function

    'changed by mohamed on 01/07/2018
    Function fn_NeedToSendHotelAgentEmailToReservationUser(Optional ByVal abFromQuotation As Boolean = False) As Boolean
        Dim objclsUtilities As New clsUtilities
        Dim lBReturnValue As Boolean = False
        Dim EmailSendOption As String = objclsUtilities.ExecuteQueryReturnStringValue("Select option_selected from reservation_parameters  where param_id=2015")
        If EmailSendOption.Trim.ToUpper = "Y" Then
            'Dim ResParam As New ReservationParameters
            'ResParam = Session("sobjResParam")
            'If ResParam IsNot Nothing Then
            '    If ResParam.LoginType = "RO" Then
            '        lBReturnValue = True
            '    End If
            'End If
            lBReturnValue = True
        End If

        'changed by mohamed on 25/09/2018 - REF DT25092018_A
        If abFromQuotation = True Then
            Dim EmailSendOptionQuote As String = objclsUtilities.ExecuteQueryReturnStringValue("Select option_selected from reservation_parameters  where param_id=2023")
            If EmailSendOptionQuote.Trim.ToUpper = "Y" Then
                lBReturnValue = True
            End If
        End If
        Return lBReturnValue
    End Function

    'changed by mohamed on 01/07/2018
    Function fn_NeedToSendHotelEmail() As Boolean
        Dim objclsUtilities As New clsUtilities
        Dim lBReturnValue As Boolean = False
        Dim EmailSendOption As String = objclsUtilities.ExecuteQueryReturnStringValue("Select option_selected from reservation_parameters  where param_id=2015")
        If EmailSendOption.Trim.ToUpper = "Y" Or EmailSendOption.Trim.ToUpper.Contains("H") = True Then
            lBReturnValue = True
        End If
        Return lBReturnValue
    End Function

    'changed by mohamed on 01/07/2018
    Function fn_NeedToSendAgentEmail() As Boolean
        Dim objclsUtilities As New clsUtilities
        Dim lBReturnValue As Boolean = False
        Dim EmailSendOption As String = objclsUtilities.ExecuteQueryReturnStringValue("Select option_selected from reservation_parameters  where param_id=2015")
        If EmailSendOption.Trim.ToUpper = "Y" Or EmailSendOption.Trim.ToUpper.Contains("A") = True Then
            lBReturnValue = True
        End If
        Return lBReturnValue
    End Function
End Class
