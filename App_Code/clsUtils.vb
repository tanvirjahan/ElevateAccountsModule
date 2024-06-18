'##################################################################################################################
'Project Name:
'Module Name:
'Created By:
'Purpose:
'##################################################################################################################

Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Web.Configuration
Imports System.Reflection
Imports Microsoft.Office.Interop
Imports System.Data.OleDb
Imports System.Diagnostics
Imports ADODB

Public Class clsUtils
    Dim myDataReader As SqlDataReader
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Public Sub FillDropDownListnew(ByVal connstr As String, ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal FiledName As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False)
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            ddl.DataSource = myDataReader
            ddl.DataTextField = FiledName
            ddl.DataValueField = FiledName
            ddl.DataBind()
            If addNewFlag = True Then
                ddl.Items.Add("[Select]")
                ddl.SelectedValue = "[Select]"
            End If
        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Sub

    Public Sub Clear_All_contract_sessions()

        HttpContext.Current.Session("Maxid") = Nothing
        HttpContext.Current.Session("contractid") = Nothing
    End Sub
#Region "Public Sub FillDropDownListnew(ByVal connstr As String, ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal strDataTextField As String, ByVal strDataValueField As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False)"
    ' This method is used for fill dropdown
    Public Sub FillDropDownListWithValuenew(ByVal connstr As String, ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal strDataTextField As String, ByVal strDataValueField As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False)
        Dim myDataReader As SqlDataReader
        Dim SqlConn As New SqlConnection
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)
            Dim myCommand As New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader()
            ddl.DataSource = myDataReader
            ddl.DataTextField = strDataTextField
            ddl.DataValueField = strDataValueField
            ddl.DataBind()
            If addNewFlag = True Then
                ddl.Items.Add("[Select]")
                ddl.SelectedValue = "[Select]"
            End If
            myCommand.Dispose()
            myDataReader.Close()
            SqlConn.Close()
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                SqlConn.Close()
            End If
        End Try
    End Sub
#End Region

#Region "Public Sub FillDropDownListnewAll(ByVal connstr As String, ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal strDataTextField As String, ByVal strDataValueField As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False)"
    ' This method is used for fill dropdown
    Public Sub FillDropDownListWithValuenewAll(ByVal connstr As String, ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal strDataTextField As String, ByVal strDataValueField As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False)
        Dim myDataReader As SqlDataReader
        Dim SqlConn As New SqlConnection
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)
            Dim myCommand As New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader()
            ddl.DataSource = myDataReader
            ddl.DataTextField = strDataTextField
            ddl.DataValueField = strDataValueField
            ddl.DataBind()
            If addNewFlag = True Then
                ddl.Items.Add("[All]")
                ddl.SelectedValue = "[All]"
            End If
            myCommand.Dispose()
            myDataReader.Close()
            SqlConn.Close()
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                SqlConn.Close()
            End If
        End Try
    End Sub
#End Region

    Public Sub FillListBoxnew(ByVal connstr As String, ByVal lb As System.Web.UI.WebControls.ListBox, ByVal FiledName As String, ByVal strQry As String)
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            lb.DataSource = myDataReader
            lb.DataTextField = FiledName
            lb.DataValueField = FiledName
            lb.DataBind()
        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbReaderClose(myDataReader)                'Close reader
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Sub

    Public Function DDLFieldAvliable(ByVal ddl As DropDownList, ByVal strval As String) As Boolean
        Dim lngcnt As Long
        DDLFieldAvliable = False
        For lngcnt = 0 To ddl.Items.Count - 1
            If ddl.Items(lngcnt).Text = strval Then
                DDLFieldAvliable = True
                Exit For
            End If
        Next
    End Function

    Public Function GetDataFromDatasetnew(ByVal connstr As String, ByVal strSqlQuery As String) As DataSet
        Dim ds As New DataSet
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection

            myDataAdapter = New SqlDataAdapter(strSqlQuery, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(ds)
            GetDataFromDatasetnew = ds

        Catch ex As Exception
            GetDataFromDatasetnew = Nothing
        End Try
    End Function

    Public Function GetDataFromReadernew(ByVal connstr As String, ByVal strSqlQuery As String) As SqlDataReader
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection

            myCommand = New SqlCommand(strSqlQuery, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            GetDataFromReadernew = myDataReader

        Catch ex As Exception
            GetDataFromReadernew = Nothing
        End Try
    End Function
    Public Function ExecuteQueryReturnSingleValuenew(ByVal connstr As String, ByVal strQuery As String) As Object
        Dim objAgrValue As Object
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection

            myCommand = New SqlCommand(strQuery, SqlConn)
            myCommand.CommandTimeout = 0
            objAgrValue = myCommand.ExecuteScalar
            If IsDBNull(objAgrValue) = False Then
                ExecuteQueryReturnSingleValuenew = objAgrValue
            Else
                ExecuteQueryReturnSingleValuenew = 0
            End If
        Catch ex As Exception
            ExecuteQueryReturnSingleValuenew = 0
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function
    Public Function ExecuteQueryReturnStringValuenew(ByVal connstr As String, ByVal strQuery As String) As String
        Dim objAgrValue As String
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                    'Open connection
            'EventLog.WriteEntry("websrv", "qry:" & strQuery)
            myCommand = New SqlCommand(strQuery, SqlConn)
            objAgrValue = myCommand.ExecuteScalar
            If IsDBNull(objAgrValue) = False Then
                ExecuteQueryReturnStringValuenew = objAgrValue
            Else
                ExecuteQueryReturnStringValuenew = ""
            End If
        Catch ex As Exception
            ExecuteQueryReturnStringValuenew = ""
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function
    Public Function GetDBFieldFromStringnew(ByVal connstr As String, ByVal strTableName As String, ByVal strKeyName As String, _
            ByVal strCriteria As String, ByVal strValue As String) As Object
        Dim strQry As String
        Try
            strQry = "SELECT " & strKeyName & " FROM " & strTableName & _
                     " WHERE " & strCriteria & "='" & strValue & "'"
            GetDBFieldFromStringnew = ExecuteQueryReturnStringValuenew(connstr, strQry)
        Catch ex As Exception
            GetDBFieldFromStringnew = 0
        Finally
        End Try
    End Function
    Public Function GetDBFieldFromStringnewdiv(ByVal connstr As String, ByVal strTableName As String, ByVal strKeyName As String, _
            ByVal strCriteria As String, ByVal strValue As String, ByVal divid As String, ByVal divfield As String) As Object
        Dim strQry As String
        Try
            strQry = "SELECT " & strKeyName & " FROM " & strTableName & _
                     " WHERE " & strCriteria & "='" & strValue & "' and " & divid & "='" & divfield & "'"
            GetDBFieldFromStringnewdiv = ExecuteQueryReturnStringValuenew(connstr, strQry)
        Catch ex As Exception
            GetDBFieldFromStringnewdiv = 0
        Finally
        End Try
    End Function
    Public Function EntryExists(ByVal connstr As String, ByVal strTableName As String, ByVal strKeyName As String, _
               Optional ByVal strWhereCond As String = "") As Boolean
        Dim strQry As String
        Dim drResult As SqlDataReader
        Try
            strQry = "SELECT " & strKeyName & " FROM " & strTableName
            If strWhereCond <> "" Then
                strQry += " WHERE " & strWhereCond
            End If
            drResult = GetDataFromReadernew(connstr, strQry)

            If drResult Is Nothing = False Then
                If drResult.HasRows Then
                    EntryExists = True
                Else
                    EntryExists = False
                End If
            Else
                EntryExists = False
            End If

        Catch ex As Exception
            EntryExists = False
        Finally
        End Try

    End Function
    Public Function GetDBFieldFromLongnew(ByVal connstr As String, ByVal strTableName As String, ByVal strKeyName As String, _
    ByVal strCriteria As String, ByVal lngValue As Long) As Object
        Dim strQry As String
        Try
            strQry = "SELECT " & strKeyName & " FROM " & strTableName & _
            " WHERE " & strCriteria & "=" & lngValue
            GetDBFieldFromLongnew = ExecuteQueryReturnSingleValuenew(connstr, strQry)
        Catch ex As Exception
            GetDBFieldFromLongnew = 0
        Finally
        End Try
    End Function

    Public Function GetDBFieldFromMultipleCriterianew(ByVal connstr As String, ByVal strTableName As String, ByVal strKeyName As String, _
    ByVal strCriteria As String) As Object
        Dim strQry As String
        Try
            strQry = "SELECT " & strKeyName & " FROM " & strTableName & " WHERE " & strCriteria
            GetDBFieldFromMultipleCriterianew = ExecuteQueryReturnSingleValuenew(connstr, strQry)
        Catch ex As Exception
            GetDBFieldFromMultipleCriterianew = 0
        End Try
    End Function
    Public Function isDuplicatenew(ByVal connstr As String, ByVal strTableName As String, ByVal strFieldName As String, ByVal varDuplicateValue As Object, Optional ByVal strFilter As String = "") As Integer
        Dim strSelectQry As String
        strSelectQry = "SELECT " & strFieldName & " FROM " & strTableName & " WHERE " & strFieldName & " = '" & varDuplicateValue & "'"
        If strFilter <> "" Then
            strSelectQry = strSelectQry & " And " & strFilter
        End If
        Dim strQryValue
        strQryValue = ExecuteQueryReturnSingleValuenew(connstr, strSelectQry) 'Use Aggregate
        If Trim(strQryValue) = "" Then
            isDuplicatenew = 0
        Else
            isDuplicatenew = 1
        End If
    End Function

    Public Function isDuplicateForModifynew(ByVal connstr As String, ByVal strTable As String, ByVal strPrimaryKey As String, ByVal strFieldName As String, _
       ByVal varDupVal As Object, ByVal lngID As String, Optional ByVal strFilter As String = "") As Boolean
        Dim strSelectQry As String
        If strFilter <> "" Then
            strSelectQry = "SELECT " & strPrimaryKey & " FROM " & strTable & " WHERE " & strPrimaryKey & "<> '" & lngID & "' and " & strFieldName & "='" & varDupVal & "' and " & strFilter
        Else
            strSelectQry = "SELECT " & strPrimaryKey & " FROM " & strTable & " WHERE " & strPrimaryKey & "<> '" & lngID & "' and " & strFieldName & "='" & varDupVal & "'"
        End If

        Dim strQryValue
        strQryValue = ExecuteQueryReturnSingleValuenew(connstr, strSelectQry) 'Use Aggregate
        If strQryValue = "" Then
            isDuplicateForModifynew = 0
        Else
            isDuplicateForModifynew = 1
        End If
    End Function
    Public Sub WritErrorLog(ByVal PageName As String, ByVal strFileName As String, ByVal strErrorDescription As String, ByVal strUserName As String)
        'Open a file for writing
        'Get a StreamReader class that can be used to read the file
        Try
            Dim objStreamWriter As StreamWriter
            objStreamWriter = File.AppendText(strFileName)
            objStreamWriter.WriteLine(PageName & " || " & DateTime.Now.ToString() & " || " & strErrorDescription & " || " & strUserName)
            objStreamWriter.WriteLine("--------------------------------------------------------------------------------------------------------------")
            objStreamWriter.Close()
        Catch ex As Exception

        End Try

    End Sub
    Public Sub MessageBox(ByVal msg As String, ByVal page As Object)
        ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "", "alert('" & msg & "'  );", True)

        'Dim lbl As New Label
        'lbl.Text = "<script language='javascript'>" & Environment.NewLine & _
        '       "window.alert('" + msg + "')</script>"
        'page.Controls.Add(lbl)
    End Sub

    Public Sub ConfirmMessageBox(ByVal msg As String, ByVal page As Object)

        Dim lbl As New Label
        lbl.Text = "<script language='javascript'>" & Environment.NewLine & _
               "if(confirm('" + msg + "')==false)return false;</script>"
        page.Controls.Add(lbl)
    End Sub
    Public Sub ExportToExcel(ByVal ds As DataSet, ByVal response As HttpResponse)
        Try
            'first let's clean up the response.object
            response.Clear()
            response.Charset = ""
            'set the response mime type for excel
            response.ContentType = "application/vnd.ms-excel"
            response.AddHeader("content-disposition", " filename=Excel.xls")
            'create a string writer
            Dim stringWrite As New System.IO.StringWriter
            'create an htmltextwriter which uses the stringwriter
            Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)
            'instantiate a datagrid
            Dim dg As New DataGrid
            'set the datagrid datasource to the dataset passed in
            dg.DataSource = ds.Tables(0)
            'bind the datagrid
            dg.DataBind()
            'tell the datagrid to render itself to our htmltextwriter
            dg.RenderControl(htmlWrite)
            'all that's left is to output the html
            response.Write(stringWrite.ToString)
            response.End()
            response.Clear()
            response.Charset = ""

        Catch ex As Exception
            ' response.Write(ex.ToString)
        End Try

    End Sub

    Public Sub ExportToExcelnew(ByVal ds As DataSet, ByVal response As HttpResponse, ByVal myexcel As String)
        Try
            '
            'first let's clean up the response.object
            response.Clear()
            response.Charset = ""
            'set the response mime type for excel
            response.ContentType = "application/vnd.ms-excel"
            response.AddHeader("content-disposition", " filename=" & myexcel & ".xls")
            'create a string writer
            Dim stringWrite As New System.IO.StringWriter
            'create an htmltextwriter which uses the stringwriter
            Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)
            'instantiate a datagrid
            Dim dg As New DataGrid
            'set the datagrid datasource to the dataset passed in
            dg.DataSource = ds.Tables(0)
            'bind the datagrid
            dg.DataBind()
            'tell the datagrid to render itself to our htmltextwriter
            dg.RenderControl(htmlWrite)
            'all that's left is to output the html
            response.Write(stringWrite.ToString)
            response.End()
            response.Clear()
            response.Charset = ""

        Catch ex As Exception
            ' response.Write(ex.ToString)
        End Try

    End Sub
    Public Function GenerateStringWriterFormat(ByVal strSqlQry As String, ByVal strConn As String) As String
        Try

            Dim myDs As New DataSet
            SqlConn = clsDBConnect.dbConnectionnew(strConn)                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'create a string writer
            Dim stringWrite As New System.IO.StringWriter
            'create an htmltextwriter which uses the stringwriter
            Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)
            'instantiate a datagrid
            Dim gv As New GridView
            myDataAdapter.Fill(myDs)
            'set the datagrid datasource to the dataset passed in
            gv.DataSource = myDs.Tables(0)
            gv.DataBind()
            'tell the datagrid to render itself to our htmltextwriter
            gv.RenderControl(htmlWrite)
            'all that's left is to output the html
            Return stringWrite.ToString()

        Catch ex As Exception
            ' response.Write(ex.ToString)
        End Try

    End Function
    Public Function ConvertSortDirectionToSql(ByVal strsortDireciton As SortDirection)
        Dim newSortDirection As String = Nothing
        Select Case (strsortDireciton)
            Case SortDirection.Ascending
                newSortDirection = "ASC"
            Case SortDirection.Descending
                newSortDirection = "DESC"
        End Select
        Return newSortDirection
    End Function

    Public Function SwapSortDirection(ByVal strsortDireciton As SortDirection)
        Dim newsortDireciton As SortDirection
        Select Case (strsortDireciton)
            Case SortDirection.Ascending
                newsortDireciton = SortDirection.Descending
            Case SortDirection.Descending
                newsortDireciton = SortDirection.Ascending
        End Select
        Return newsortDireciton
    End Function
    Public Sub FillDropDownListHTMLNEW(ByVal connstr As String, ByVal ddl As System.Web.UI.HtmlControls.HtmlSelect, ByVal TextFiledName As String, ByVal ValueFiledName As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False, Optional ByVal selecteditem As String = "")

        Try

            SqlConn = clsDBConnect.dbConnectionnew(connstr)  'Open   connection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            ddl.DataSource = myDataReader
            ddl.DataTextField = TextFiledName
            ddl.DataValueField = ValueFiledName
            ddl.DataBind()
            If addNewFlag = True Then
                ddl.Items.Add("[Select]")
                ddl.Value = "[Select]"
            End If
            If selecteditem <> "" Then
                ddl.Value = selecteditem
            End If
        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand) 'Close command
            clsDBConnect.dbReaderClose(myDataReader) 'Close reader
            clsDBConnect.dbConnectionClose(SqlConn) 'Close  connection()
        End Try

    End Sub
    Public Sub FillDropDownListHTMLNEWForAll(ByVal connstr As String, ByVal ddl As System.Web.UI.HtmlControls.HtmlSelect, ByVal TextFiledName As String, ByVal ValueFiledName As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False, Optional ByVal selecteditem As String = "")

        Try

            SqlConn = clsDBConnect.dbConnectionnew(connstr)  'Open   connection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            ddl.DataSource = myDataReader
            ddl.DataTextField = TextFiledName
            ddl.DataValueField = ValueFiledName
            ddl.DataBind()
            If addNewFlag = True Then
                'ddl.Items.Add("[Select]")
                'ddl.Value = "[Select]"

                ddl.Items.Add("[All]")
                ddl.Value = "[All]"
            End If
            If selecteditem <> "" Then
                ddl.Value = selecteditem
            End If
        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand) 'Close command
            clsDBConnect.dbReaderClose(myDataReader) 'Close reader
            clsDBConnect.dbConnectionClose(SqlConn) 'Close  connection()
        End Try

    End Sub

    Public Function FillclsMasternew(ByVal connstr As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False) As List(Of clsMaster)

        Dim masterlist As New List(Of clsMaster)
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr) 'Open   connection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If myDataReader.HasRows Then
                Dim i As Integer = 0
                Do While myDataReader.Read
                    Dim cmaster As New clsMaster
                    If Not IsDBNull(myDataReader.GetValue(0)) Then
                        cmaster.ListText = myDataReader.GetValue(0)
                    Else
                        cmaster.ListText = ""
                    End If
                    If Not IsDBNull(myDataReader.GetValue(1)) Then
                        cmaster.ListValue = myDataReader.GetValue(1)
                    Else
                        'cmaster.ListValue = myDataReader.GetValue(1)
                        cmaster.ListValue = ""
                    End If

                    masterlist.Add(cmaster)
                    i = i + 1
                Loop
            End If
            If addNewFlag = True Then
                Dim cmaster As New clsMaster
                cmaster.ListText = "[Select]"
                cmaster.ListValue = "[Select]"
                masterlist.Add(cmaster)
            End If
        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand) 'Close command
            clsDBConnect.dbReaderClose(myDataReader) 'Close reader
            clsDBConnect.dbConnectionClose(SqlConn) 'Close  connection()
        End Try
        Return masterlist

    End Function


    Public Function FillclsMasterSP(ByVal connstr As String, ByVal strQry As String, Optional ByVal addNewFlag As Boolean = False) As List(Of clsMaster)

        Dim masterlist As New List(Of clsMaster)
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr) 'Open   connection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If myDataReader.HasRows Then
                Dim i As Integer = 0
                Do While myDataReader.Read
                    Dim cmaster As New clsMaster
                    If Not IsDBNull(myDataReader.GetValue(1)) Then
                        cmaster.ListText = myDataReader.GetValue(1)
                    Else
                        cmaster.ListText = ""
                    End If
                    If Not IsDBNull(myDataReader.GetValue(0)) Then
                        cmaster.ListValue = myDataReader.GetValue(0)
                    Else
                        cmaster.ListValue = ""
                    End If

                    masterlist.Add(cmaster)
                    i = i + 1
                Loop
            End If
            If addNewFlag = True Then
                Dim cmaster As New clsMaster
                cmaster.ListText = "[Select]"
                cmaster.ListValue = "[Select]"
                masterlist.Add(cmaster)
            End If
        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand) 'Close command
            clsDBConnect.dbReaderClose(myDataReader) 'Close reader
            clsDBConnect.dbConnectionClose(SqlConn) 'Close  connection()
        End Try
        Return masterlist

    End Function


    Public Function GetAutoDocNoyear(ByVal optionname As String, ByVal dcon As SqlConnection, ByVal dtran As SqlTransaction, ByVal docyear As String) As String
        Dim str As String
        str = ""
        GetAutoDocNoyear = ""
        If optionname <> "" Then
            Dim ds As DataSet
            ds = New DataSet
            'SqlConn = clsDBConnect.dbConnectionnew(connstr)
            'Dim comcls As Commoncls
            'comcls = New Commoncls
            myCommand = New SqlCommand("sp_getnumber", dcon, dtran)
            myCommand.CommandText = "sp_getnumber"
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = dcon
            myCommand.Parameters.Add(New SqlParameter("@optionname", optionname))
            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@newno"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            myCommand.Parameters.Add(param)
            myCommand.Parameters.Add(New SqlParameter("@cyear", docyear))
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            str = param.Value
            myCommand = Nothing
            Return str
        Else
            str = ""
            Return str
        End If
    End Function
    Public Function GetAutoDocNodiv(ByVal optionname As String, ByVal dcon As SqlConnection, ByVal dtran As SqlTransaction, ByVal divid As String) As String
        Dim str As String
        str = ""
        GetAutoDocNodiv = ""
        If optionname <> "" Then
            Dim ds As DataSet
            ds = New DataSet
            'SqlConn = clsDBConnect.dbConnectionnew(connstr)
            'Dim comcls As Commoncls
            'comcls = New Commoncls
            myCommand = New SqlCommand("sp_getnumber_div", dcon, dtran)
            myCommand.CommandText = "sp_getnumber_div"
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = dcon
            myCommand.Parameters.Add(New SqlParameter("@optionname", optionname))
            myCommand.Parameters.Add(New SqlParameter("@divid", divid))
            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@newno"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            myCommand.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            str = param.Value
            myCommand = Nothing
            Return str
        Else
            str = ""
            Return str
        End If
    End Function
    Public Function GetAutoDocNo(ByVal optionname As String, ByVal dcon As SqlConnection, ByVal dtran As SqlTransaction) As String
        Dim str As String
        str = ""
        GetAutoDocNo = ""
        If optionname <> "" Then
            Dim ds As DataSet
            ds = New DataSet
            'SqlConn = clsDBConnect.dbConnectionnew(connstr)
            'Dim comcls As Commoncls
            'comcls = New Commoncls
            myCommand = New SqlCommand("sp_getnumber", dcon, dtran)
            myCommand.CommandText = "sp_getnumber"
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = dcon
            myCommand.Parameters.Add(New SqlParameter("@optionname", optionname))
            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@newno"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            myCommand.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            str = param.Value
            myCommand = Nothing
            Return str
        Else
            str = ""
            Return str
        End If
    End Function

    Public Function Getdocgen(ByVal optionname As String, ByVal dcon As SqlConnection, ByVal dtran As SqlTransaction) As String
        Dim str As String
        Dim str1 As String
        str = ""
        str1 = ""

        Getdocgen = ""
        If optionname <> "" Then
            Dim ds As DataSet
            ds = New DataSet
            'SqlConn = clsDBConnect.dbConnectionnew(connstr)
            'Dim comcls As Commoncls
            'comcls = New Commoncls
            myCommand = New SqlCommand("sp_docgen", dcon, dtran)
            myCommand.CommandText = "sp_docgen"
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = dcon
            myCommand.Parameters.Add(New SqlParameter("@optionname", optionname))
            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@newno"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 10
            myCommand.Parameters.Add(param)
            Dim param1 As SqlParameter
            param1 = New SqlParameter
            param1.ParameterName = "@docprefix"
            param1.Direction = ParameterDirection.Output
            param1.DbType = DbType.String
            param1.Size = 5
            myCommand.Parameters.Add(param1)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            'str = Format(CType(param.Value, String), "0000")
            str = param.Value
            ' str = str1 + "/" + str
            myCommand = Nothing
            Return str
        Else
            str = ""
            Return str
        End If
    End Function
    Public Function GetAutoDocNoWTnew(ByVal constr As String, ByVal optionname As String) As String
        Dim str As String
        str = ""
        GetAutoDocNoWTnew = ""
        If optionname <> "" Then
            SqlConn = clsDBConnect.dbConnectionnew(constr)
            myCommand = New SqlCommand("sp_getnumber", SqlConn)
            myCommand.CommandText = "sp_getnumber"
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = SqlConn
            myCommand.Parameters.Add(New SqlParameter("@optionname", optionname))
            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@newno"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            myCommand.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            str = param.Value
            myCommand = Nothing
            Return str
        Else
            str = ""
            Return str
        End If
    End Function
    Public Function DeleteFile(ByVal filepath As String)
        If File.Exists(filepath) = True Then
            File.Delete(filepath)
        End If
        Return True
    End Function
    Public Function GetDBFieldValueExistnew(ByVal constr As String, ByVal strTableName As String, ByVal strKeyName As String, ByVal strKeyValue As String) As Boolean
        Dim strQry As String
        Dim strRes As String
        Try

            strQry = "SELECT  't' FROM " & strTableName & _
                     " WHERE " & strKeyName & "='" & strKeyValue & "'"
            strRes = ""
            strRes = ExecuteQueryReturnStringValuenew(constr, strQry)
            If Trim(strRes) <> "" Then
                GetDBFieldValueExistnew = True
            Else
                GetDBFieldValueExistnew = False
            End If
        Catch ex As Exception
            GetDBFieldValueExistnew = False
        Finally
        End Try
    End Function

    Public Function ExecuteQuerynew(ByVal connstr As String, ByVal storedProcedure As String, ByVal sqlParamList As List(Of SqlParameter)) As DataSet


        SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection

        myCommand = New SqlCommand()
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = storedProcedure
        myCommand.Connection = SqlConn
        Dim size As Integer
        Dim i As Integer
        size = sqlParamList.Count
        For i = 0 To size - 1
            myCommand.Parameters.Add(sqlParamList(i))
        Next
        myCommand.CommandTimeout = 0
        Dim adapter As New SqlDataAdapter(myCommand)
        Dim ds As New DataSet()
        Try
            adapter.Fill(ds)
        Catch ex As Exception
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Finally
            adapter.Dispose()
            ds.Dispose()
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
        Return ds
    End Function
    Public Function ExecuteQuerynew(ByVal connstr As String, ByVal storedProcedure As String) As DataSet


        SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection

        myCommand = New SqlCommand()
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = storedProcedure
        myCommand.Connection = SqlConn
        Dim adapter As New SqlDataAdapter(myCommand)
        Dim ds As New DataSet()
        Try
            adapter.Fill(ds)
        Catch ex As Exception
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Finally
            adapter.Dispose()
            ds.Dispose()
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
        Return ds
    End Function
    Public Function ExecuteQuerySqlnew(ByVal connstr As String, ByVal sql As String) As DataSet

        SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection

        myCommand = New SqlCommand(sql, SqlConn)
        Dim adapter As New SqlDataAdapter(myCommand)
        Dim ds As New DataSet()
        Try
            adapter.Fill(ds)
        Catch ex As Exception
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Finally
            adapter.Dispose()
            ds.Dispose()
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
        Return ds
    End Function
    Public Function ExecuteNonQuerynew(ByVal connstr As String, ByVal storedProcedure As String, ByVal sqlParamList As List(Of SqlParameter)) As Integer
        SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection
        myCommand = New SqlCommand()
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = storedProcedure
        myCommand.Connection = SqlConn
        Dim size As Integer
        Dim i As Integer
        size = sqlParamList.Count
        For i = 0 To size - 1
            myCommand.Parameters.Add(sqlParamList(i))
        Next
        Dim Norows As New Integer
        Try
            Norows = myCommand.ExecuteNonQuery()

        Catch ex As Exception
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
        Return Norows
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

        Norows = myCommand.ExecuteNonQuery()

        Return Norows
    End Function

    Public Function ExecuteNonQuerynew(ByVal constr As String, ByVal strSql As String, ByVal conn As SqlConnection, ByVal trans As SqlTransaction) As Boolean
        'Open connection
        myCommand = New SqlCommand()
        myCommand.CommandType = CommandType.Text
        myCommand.CommandText = strSql
        myCommand.Connection = conn
        myCommand.Transaction = trans

        Dim Norows As New Integer

        Norows = myCommand.ExecuteNonQuery()
        If (Norows < 0) = True Then
            ExecuteNonQuerynew = False
            Exit Function
        End If

        Return True
    End Function

    Public Function IsNumber(ByVal strDigit As String) As Boolean
        If strDigit.Trim().Length = 0 Then
            Return False
        End If

        Dim objRegex As Regex = New Regex("^[0-9]*$")
        Return objRegex.IsMatch(strDigit)
    End Function


    Public Function IsDecimal(ByVal strDigit As String) As Boolean
        If strDigit.Trim().Length = 0 Then
            Return False
        End If

        Dim objRegex As Regex = New Regex("^[0-9]*[.,]?[0-9]*$")
        Return objRegex.IsMatch(strDigit)
    End Function
    Public Function FillArraynew(ByVal connstr As String, ByVal strQry As String, ByVal cntArray As Long) As String()
        Dim ResultArray(cntArray) As String
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr) 'Open   connection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If myDataReader.HasRows Then
                Dim i As Integer = 0
                Do While myDataReader.Read
                    ' ResultArray(i) = CType(myDataReader.GetValue(i).ToString(), String)
                    For i = 0 To cntArray - 1
                        ResultArray(i) = CType(myDataReader.GetValue(i).ToString(), String)
                        '  i = i + 1
                    Next

                Loop
            End If

        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand) 'Close command
            clsDBConnect.dbReaderClose(myDataReader) 'Close reader
            clsDBConnect.dbConnectionClose(SqlConn) 'Close  connection()
        End Try
        Return ResultArray

    End Function

    Public Function ValidateEmail(ByVal strEmail As String) As Boolean
        If strEmail.Trim().Length = 0 Then
            Return False
        End If
        Dim objRegex As Regex = New Regex("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        Return objRegex.IsMatch(strEmail)
    End Function

    Public Function DeleteRowForBackToMainnew(ByVal connstr As String, ByVal requestid As String, ByVal rlineno As String, ByVal basketid As String, ByVal requesttype As String) As Boolean
        SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection
        myCommand = New SqlCommand()
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = "sp_request_backtomainpage"
        myCommand.Connection = SqlConn
        Dim size As Integer = 3
        Dim i As Integer
        Dim res As Boolean = True
        Dim parm(4) As SqlParameter
        parm(0) = New SqlParameter("@requestid", requestid)
        parm(1) = New SqlParameter("@rlineno", rlineno)
        parm(2) = New SqlParameter("@basketid", basketid)
        parm(3) = New SqlParameter("@requesttype", requesttype)
        For i = 0 To size
            myCommand.Parameters.Add(parm(i))
        Next
        Dim Norows As New Integer
        Try
            Norows = myCommand.ExecuteNonQuery()
            res = True

        Catch ex As Exception
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
            res = False
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
        Return res
    End Function

    Public Function RoundwithParameternew(ByVal connstr As String, ByVal numbertoround As Decimal) As Decimal
        Dim strsql As String = ""
        Dim roundednumber As Decimal = 0
        strsql = "select dbo.roundwithparameter(" & numbertoround & ")"
        roundednumber = CType(ExecuteQueryReturnSingleValuenew(connstr, strsql), Decimal)
        Return roundednumber
    End Function
    Public Sub ExecuteNonQuerynewProc(ByVal connstr As String, ByVal storedProcedure As String, ByVal sqlParamList As List(Of SqlParameter))
        SqlConn = clsDBConnect.dbConnectionnew(connstr)                     'Open connection
        myCommand = New SqlCommand()
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = storedProcedure
        myCommand.Connection = SqlConn
        Dim size As Integer
        Dim i As Integer
        size = sqlParamList.Count
        For i = 0 To size - 1
            myCommand.Parameters.Add(sqlParamList(i))
        Next
        Try
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Sub
    Public Function MyCodeReturn(ByVal MyPrefix As String, ByVal MyIdentity As Integer, ByVal MyLength As Integer) As String
        Try
            Dim MyString As String = String.Empty
            MyString = MyPrefix & Right("00000000" & MyIdentity, MyLength)
            Return MyString
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Function CodeReturn(ByVal MyIdentity As Integer, ByVal MyLength As Integer) As String
        Try
            Dim MyString As String = String.Empty
            MyString = Right("00000000" & MyIdentity, MyLength)
            Return MyString
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Function GetString(ByVal connstr As String, ByVal strQuery As String) As String
        Dim objAgrValue As String
        Dim da As SqlDataAdapter
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim dr As DataRow
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                    'Open connection
            'EventLog.WriteEntry("websrv", "qry:" & strQuery)
            myCommand = New SqlCommand(strQuery, SqlConn)
            'objAgrValue = myCommand.ExecuteNonQuery()
            da = New SqlDataAdapter(myCommand)
            da.Fill(ds)
            dt = ds.Tables(0)

            If IsDBNull(dt.Rows(0).Item(0)) = False Then
                GetString = dt.Rows(0).Item(0)
            Else
                GetString = ""
            End If
        Catch ex As Exception
            GetString = ""
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function

    Public Function CheckString(ByVal connstr As String, ByVal strQuery As String) As String
        Dim objAgrValue As String
        Dim da As SqlDataAdapter
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim dr As DataRow
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr)                    'Open connection
            'EventLog.WriteEntry("websrv", "qry:" & strQuery)
            myCommand = New SqlCommand(strQuery, SqlConn)
            'objAgrValue = myCommand.ExecuteNonQuery()
            da = New SqlDataAdapter(myCommand)
            da.Fill(ds)
            dt = ds.Tables(0)
            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0).Item(0)) = False Then
                    CheckString = dt.Rows(0).Item(0)
                Else
                    CheckString = ""
                End If
            Else
                CheckString = ""
            End If
        Catch ex As Exception
            CheckString = ""
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'Close command 
            clsDBConnect.dbConnectionClose(SqlConn)                 'Close connection
        End Try
    End Function

    Public Function GenerateXML(ByVal ds As DataSet) As String
        Dim obj As New StringWriter()
        Dim xmlstring As String
        ds.WriteXml(obj)
        xmlstring = obj.ToString()
        Return xmlstring
    End Function

    Public Function GenerateXML(ByVal ds As DataTable) As String
        Dim obj As New StringWriter()
        Dim xmlstring As String
        ds.WriteXml(obj)
        xmlstring = obj.ToString()
        Return xmlstring
    End Function

    'Changed by Mohamed on 25/07/2016
    Public Function FillStringArray(ByVal connstr As String, ByVal strQry As String) As List(Of String)
        Dim SqlConn As SqlConnection
        Dim myDataReader As SqlDataReader
        Dim myCommand As SqlCommand
        Dim masterlist As New List(Of String)
        Try
            SqlConn = clsDBConnect.dbConnectionnew(connstr) 'Open   connection()
            myCommand = New SqlCommand(strQry, SqlConn)
            myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If myDataReader.HasRows Then
                Dim i As Integer = 0
                Do While myDataReader.Read
                    masterlist.Add(myDataReader.GetValue(0))
                    i = i + 1
                Loop
            End If

        Catch ex As Exception

        Finally
            clsDBConnect.dbCommandClose(myCommand) 'Close command
            clsDBConnect.dbReaderClose(myDataReader) 'Close reader
            clsDBConnect.dbConnectionClose(SqlConn) 'Close  connection()
        End Try
        Return masterlist

    End Function

    'Changed by Mohamed on 25/07/2016
    Function splitWithWords(ByVal asSplitString As String, ByVal asSplitWord As String) As String()
        Try

            Dim aSplit As New List(Of String)
            Dim liStartPos As Integer = 1
            Dim liEndPos As Integer = 1
            Dim lsStr = asSplitString
            If asSplitString.Trim = "" Then
                GoTo retpos
            End If
            Do
                If liStartPos > lsStr.Length Then
                    GoTo retpos
                End If
                liEndPos = lsStr.IndexOf(asSplitWord, liStartPos)
                If liEndPos > 0 Then
                    aSplit.Add(Mid(asSplitString, liStartPos, liEndPos - liStartPos + 1))
                    liStartPos = liEndPos + asSplitWord.Length + 1
                Else
                    aSplit.Add(Mid(asSplitString, liStartPos))
                    Exit Do
                End If
            Loop
retpos:
            Return aSplit.ToArray()
        Catch ex As Exception
            Dim aSplit As New List(Of String)
            Return aSplit.ToArray()
        End Try
    End Function

    'Changed by Mohamed on 25/07/2016
    Function sbSetSelectedValueForHTMLSelect(ByVal asSelectString As String, ByVal aObject As HtmlSelect)
        For i As Integer = 0 To aObject.Items.Count - 1
            If Trim(UCase(aObject.Items(i).Text)) = Trim(UCase(asSelectString)) Then
                aObject.SelectedIndex = i
                Exit For
            End If
        Next
    End Function

    Function fnGridViewRowToDataRow(ByVal gvr As GridViewRow) As DataRow
        Dim di As Object = Nothing
        Dim drv As DataRowView = Nothing
        Dim dr As DataRow = Nothing

        If gvr IsNot Nothing Then
            di = TryCast(gvr.DataItem, System.Object)
            If di IsNot Nothing Then
                drv = TryCast(di, System.Data.DataRowView)
                If drv IsNot Nothing Then
                    dr = TryCast(drv.Row, System.Data.DataRow)
                End If
            End If
        End If

        Return dr
    End Function

    Public Function fnGridViewObjectToDataRow(ByVal list As Object) As DataRow

        Dim table As New DataTable()
        If list Is Nothing Then
            'don't know schema ....
            Return Nothing
        End If

        Dim fields() = DirectCast(list, System.Web.UI.WebControls.GridViewRow).DataItem.GetType.GetProperties
        For Each field In fields
            table.Columns.Add(field.Name, field.PropertyType)
        Next
        'For Each item In list
        Dim row As DataRow = table.NewRow()
        For Each field In fields
            Dim p = DirectCast(list, System.Web.UI.WebControls.GridViewRow).DataItem.GetType.GetProperty(field.Name)
            row(field.Name) = p.GetValue(DirectCast(list, System.Web.UI.WebControls.GridViewRow).DataItem, Nothing)
        Next
        table.Rows.Add(row)
        'Next
        'Return table
        Return row

        'Dim table As New DataTable()
        'Dim fields() As FieldInfo = list.GetType.GetFields
        'For Each field As FieldInfo In fields
        '    table.Columns.Add(field.Name, field.FieldType)
        'Next
        ''For Each item In list
        'Dim row As DataRow = table.NewRow()
        'For Each field As FieldInfo In fields
        '    row(field.Name) = field.GetValue(list)
        'Next
        'table.Rows.Add(row)
        ''Next
        'Return row
    End Function

    Public Function fnConvertToDataTable(Of t)(ByVal list As IList(Of t)) As DataTable
        Dim table As New DataTable()
        If list Is Nothing Then
            'don't know schema ....
            Return Nothing
        End If
        If list.Count <= 0 Then
            'don't know schema ....
            Return Nothing
        End If

        Dim fields() = list(0).GetType.GetProperties
        For Each field In fields
            table.Columns.Add(field.Name, field.PropertyType)
        Next
        For Each item In list
            Dim row As DataRow = table.NewRow()
            For Each field In fields
                Dim p = item.GetType.GetProperty(field.Name)
                row(field.Name) = p.GetValue(item, Nothing)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function GetDBFieldFromLongnewdiv(ByVal connstr As String, ByVal strTableName As String, ByVal strKeyName As String, _
   ByVal strCriteria As String, ByVal lngValue As Long, ByVal divfieldname As String, ByVal strdivvalue As String) As Object
        Dim strQry As String
        Try
            strQry = "SELECT " & strKeyName & " FROM " & strTableName & _
            " WHERE " & divfieldname & "='" & strdivvalue & "' and " & strCriteria & "=" & lngValue
            GetDBFieldFromLongnewdiv = ExecuteQueryReturnSingleValuenew(connstr, strQry)
        Catch ex As Exception
            GetDBFieldFromLongnewdiv = 0
        Finally
        End Try
    End Function


    Public Function GetDBFieldFromMultipleCriterianewdiv(ByVal connstr As String, ByVal strTableName As String, ByVal strKeyName As String, _
       ByVal strCriteria As String, ByVal divfield As String, ByVal divvalue As String) As Object
        Dim strQry As String
        Try
            strQry = "SELECT " & strKeyName & " FROM " & strTableName & " WHERE " & divfield & "=" & divvalue & " and " & strCriteria
            GetDBFieldFromMultipleCriterianewdiv = ExecuteQueryReturnSingleValuenew(connstr, strQry)
        Catch ex As Exception
            GetDBFieldFromMultipleCriterianewdiv = 0
        End Try
    End Function



    Public Function isDuplicatenewdiv(ByVal connstr As String, ByVal strTableName As String, ByVal strFieldName As String, ByVal varDuplicateValue As Object, ByVal divFieldName As String, ByVal divvalue As String, Optional ByVal strFilter As String = "") As Integer
        Dim strSelectQry As String
        strSelectQry = "SELECT " & strFieldName & " FROM " & strTableName & " WHERE " & strFieldName & " = '" & varDuplicateValue & "' and " & divFieldName & "='" & divvalue & "'"
        If strFilter <> "" Then
            strSelectQry = strSelectQry & " And " & strFilter
        End If
        Dim strQryValue
        strQryValue = ExecuteQueryReturnSingleValuenew(connstr, strSelectQry) 'Use Aggregate
        If Trim(strQryValue) = "" Then
            isDuplicatenewdiv = 0
        Else
            isDuplicatenewdiv = 1
        End If
    End Function


    Public Function isDuplicateForModifynewdiv(ByVal connstr As String, ByVal strTable As String, ByVal strPrimaryKey As String, ByVal strFieldName As String, _
          ByVal varDupVal As Object, ByVal lngID As String, ByVal divfield As String, ByVal strdivvalue As String, Optional ByVal strFilter As String = "") As Boolean
        Dim strSelectQry As String
        If strFilter <> "" Then
            strSelectQry = "SELECT " & strPrimaryKey & " FROM " & strTable & " WHERE  " & divfield & "='" & strdivvalue & "' and  " & strPrimaryKey & "<> '" & lngID & "' and " & strFieldName & "='" & varDupVal & "' and " & strFilter
        Else
            strSelectQry = "SELECT " & strPrimaryKey & " FROM " & strTable & " WHERE " & divfield & "='" & strdivvalue & "' and  " & strPrimaryKey & "<> '" & lngID & "' and " & strFieldName & "='" & varDupVal & "'"
        End If

        Dim strQryValue
        strQryValue = ExecuteQueryReturnSingleValuenew(connstr, strSelectQry) 'Use Aggregate
        If strQryValue = "" Then
            isDuplicateForModifynewdiv = 0
        Else
            isDuplicateForModifynewdiv = 1
        End If
    End Function



    Public Function GetDBFieldValueExistnewdiv(ByVal constr As String, ByVal strTableName As String, ByVal strKeyName As String, ByVal strKeyValue As String, ByVal divfield As String, ByVal strdivvalue As String) As Boolean
        Dim strQry As String
        Dim strRes As String
        Try

            strQry = "SELECT  't' FROM " & strTableName & _
                     " WHERE " & divfield & "='" & strdivvalue & "' and  " & strKeyName & "='" & strKeyValue & "'"
            strRes = ""
            strRes = ExecuteQueryReturnStringValuenew(constr, strQry)
            If Trim(strRes) <> "" Then
                GetDBFieldValueExistnewdiv = True
            Else
                GetDBFieldValueExistnewdiv = False
            End If
        Catch ex As Exception
            GetDBFieldValueExistnewdiv = False
        Finally
        End Try
    End Function

   


  
    Public Function TranslateType(ByRef type As Type) As ADODB.DataTypeEnum
        Try


            Select Case type.UnderlyingSystemType.ToString
                Case "System.Boolean"
                    Return ADODB.DataTypeEnum.adBoolean
                Case "System.Byte"
                    Return ADODB.DataTypeEnum.adUnsignedTinyInt
                Case "System.Char"
                    Return ADODB.DataTypeEnum.adChar
                Case "System.DateTime"
                    Return ADODB.DataTypeEnum.adDate
                Case "System.Decimal"
                    Return ADODB.DataTypeEnum.adCurrency

                Case "System.Double"


                    Return ADODB.DataTypeEnum.adDouble
                Case "System.Int16"
                    Return ADODB.DataTypeEnum.adSmallInt
                Case "System.Int32"
                    Return ADODB.DataTypeEnum.adInteger
                Case "System.Int64"
                    Return ADODB.DataTypeEnum.adBigInt
                Case "System.SByte"
                    Return ADODB.DataTypeEnum.adTinyInt
                Case "System.Single"
                    Return ADODB.DataTypeEnum.adSingle
                Case ("System.UInt16")
                    Return ADODB.DataTypeEnum.adUnsignedSmallInt
                Case "System.UInt32"
                    Return ADODB.DataTypeEnum.adUnsignedInt
                Case "System.UInt64"
                    Return ADODB.DataTypeEnum.adUnsignedBigInt
                Case "System.String"
                    '  Case default
                    Return ADODB.DataTypeEnum.adVarWChar
            End Select
        Catch ex As Exception

        End Try
    End Function
    Public Function convertToADODB(ByRef table As DataTable) As ADODB.Recordset


        Dim result As New ADODB.Recordset
        result.CursorLocation = CursorLocationEnum.adUseClient
        Dim resultFields As ADODB.Fields = result.Fields
        Dim col As DataColumn
        For Each col In table.Columns
            resultFields.Append(col.ColumnName, TranslateType(col.DataType),
                    col.MaxLength, col.AllowDBNull = ADODB.FieldAttributeEnum.adFldIsNullable)
        Next
        'result.Open(System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, 0)
        result.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, 0)

        For Each row As DataRow In table.Rows
            result.AddNew(System.Reflection.Missing.Value, System.Reflection.Missing.Value)

            For i As Integer = 0 To table.Columns.Count - 1
                resultFields(i).Value = IIf(IsDBNull(row(i)) = True, "", row(i))
            Next
        Next
        Return (result)
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
                                'WriteErrorLog(sqlE.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
                            Next
                            dataSet = Nothing
                        Catch ex As Exception
                            'WriteErrorLog(ex.Message.ToString & " :: " & Reflection.MethodBase.GetCurrentMethod.Name)
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

    Public Function GetAutoDocNodivYear(ByVal optionname As String, ByVal dcon As SqlConnection, ByVal dtran As SqlTransaction, ByVal divid As String, ByVal docyear As Integer) As String
        Dim str As String
        str = ""
        GetAutoDocNodivYear = ""
        If optionname <> "" Then
            Dim ds As DataSet
            ds = New DataSet
            'SqlConn = clsDBConnect.dbConnectionnew(connstr)
            'Dim comcls As Commoncls
            'comcls = New Commoncls
            myCommand = New SqlCommand("sp_getnumber_div", dcon, dtran)
            myCommand.CommandText = "sp_getnumber_div"
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = dcon
            myCommand.Parameters.Add(New SqlParameter("@optionname", optionname))
            myCommand.Parameters.Add(New SqlParameter("@divid", divid))
            myCommand.Parameters.Add(New SqlParameter("@cyear", docyear))
            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@newno"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            myCommand.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            str = param.Value
            myCommand = Nothing
            Return str
        Else
            str = ""
            Return str
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="strQuery"></param>
    ''' <param name="strConnName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDetailsPageWise(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal strQuery As String) As DataSet
        Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
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


    Public Function GetAutoDocNodivMonth(ByVal optionname As String, ByVal dcon As SqlConnection, ByVal dtran As SqlTransaction, ByVal divid As String, ByVal tranType As String, ByVal docmonth As String, ByVal docyear As String) As String
        Dim str As String
        str = ""
        If optionname <> "" Then
            myCommand = New SqlCommand("sp_get_monthyear_number", dcon, dtran)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = dcon
            myCommand.Parameters.Add(New SqlParameter("@docgen_div_optionName", optionname))
            myCommand.Parameters.Add(New SqlParameter("@div_id", divid))
            myCommand.Parameters.Add(New SqlParameter("@tran_type", tranType))
            myCommand.Parameters.Add(New SqlParameter("@docmonth", docmonth))
            myCommand.Parameters.Add(New SqlParameter("@docyear", docyear))
            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@code"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            myCommand.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            str = param.Value
            myCommand = Nothing
            Return str
        Else
            str = ""
            Return str
        End If
    End Function
End Class