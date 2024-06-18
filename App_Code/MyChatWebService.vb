Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports MyClasses
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class MyChatWebService
    Inherits System.Web.Services.WebService
    Dim MyUtilities As New clsUtils
    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function
    <WebMethod(EnableSession:=True)> _
    Public Function LogOffChat(ByVal ProfileId As String) As String
        Try
            Dim Str As String = String.Empty
            Dim MyConn As New SqlConnection
            MyConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            Dim Cmd As New SqlCommand
            Str = "Exec Sp_AddChatLogin '" & ProfileId & "','','',1,'" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR") & "','',1 "
            Cmd = New SqlCommand(Str, MyConn)
            Cmd.ExecuteNonQuery()
            Cmd.Cancel()
            MyConn.Close()
            Return "Success"
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    <WebMethod(EnableSession:=True)> _
    Public Function ShowCommunicationDetails() As List(Of MyFillClass)
        Try
            Dim MyCategory As New List(Of MyFillClass)
            Dim Str As String = String.Empty
            Dim MyDataSet As New DataSet
            Dim MyDataTable As New DataTable
            Str = "Exec Sp_Chat_Message_Admin "
            MyDataSet = MyUtilities.GetDataFromDatasetnew(Session("dbconnectionName"), Str)
            MyDataTable = New DataTable
            If MyDataSet.Tables.Count > 0 Then
                MyDataTable = MyDataSet.Tables(0)
            End If

            If MyDataTable.Rows.Count > 0 Then
                Dim i As Integer = 0

                For i = 0 To MyDataTable.Rows.Count - 1

                    Dim MyCategoryAdd As New MyFillClass
                    MyCategoryAdd.Id = MyDataTable.Rows(i).Item("UserFromId")
                    MyCategoryAdd.Name = IgnoreSpecials(MyDataTable.Rows(i).Item("ChatMessage"))
                    MyCategoryAdd.LoginId = String.Empty
                    MyCategoryAdd.Extra = MyDataTable.Rows(i).Item("InitalColumn")
                    MyCategory.Add(MyCategoryAdd)

                Next
            End If
            Return MyCategory
        Catch ex As Exception
            Dim MyCategory As New List(Of MyFillClass)
            Return MyCategory
        End Try
    End Function
    <WebMethod(EnableSession:=True)> _
    Public Function AgentAutoComplete(ByVal para1 As String) As List(Of AutocompleteClass)
        Try
            Dim CustomerAutoCompleteClass As New List(Of AutocompleteClass)
            Dim CustomerAutoCompleteClassAdd As New AutocompleteClass
            Dim strSql As String
            strSql = "Select  Agentcode,Agentname From AgentMast where active=1 and Agentname like '%" & para1 & "%'"

            Dim MYCommand As New SqlCommand(strSql, clsDBConnect.dbConnectionnew(Session("dbconnectionName").ToString()))

            Dim Dr As SqlDataReader
            Dr = MYCommand.ExecuteReader
            While Dr.Read
                If Not IsDBNull("Agentcode") Then
                    CustomerAutoCompleteClassAdd = New AutocompleteClass
                    CustomerAutoCompleteClassAdd.Id = Dr("AgentCode")
                    CustomerAutoCompleteClassAdd.Name = Dr("AgentName")
                    CustomerAutoCompleteClass.Add(CustomerAutoCompleteClassAdd)
                End If
            End While
            Dr.Close()
            MYCommand.Cancel()
            Return CustomerAutoCompleteClass
        Catch ex As Exception
        End Try
    End Function

    Public Function IgnoreSpecials(ByVal Str As String) As String
        Try
            Str = Str.Replace(Chr(34), "DOUBLEQUOTESSTRING")
            Str = Str.Replace(Chr(39), "SINGLEQUOTESSTRING")
            Str = Str.Replace(Chr(13), "LINEBREAK")
            Str = Str.Replace(Chr(47), "FORWARDSLASH")
            Str = Str.Replace(Chr(92), "BACKWARDSLASH")
            Str = Str.Replace(Chr(62), "GREATERTHAN")
            Str = Str.Replace(Chr(60), "LESSTHAN")
            Return Str
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
End Class
Public Class MyFillClass
    Dim _Id As String
    Dim _Name As String
    Dim _Extra As String
    Dim _LoginId As String

    Property Id() As String
        Get
            Return _Id
        End Get
        Set(ByVal Value As String)
            _Id = Value
        End Set
    End Property
    Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            _Name = Value
        End Set
    End Property
    Property Extra() As String
        Get
            Return _Extra
        End Get
        Set(ByVal Value As String)
            _Extra = Value
        End Set
    End Property
    Property LoginId() As String
        Get
            Return _LoginId
        End Get
        Set(ByVal Value As String)
            _LoginId = Value
        End Set
    End Property
End Class