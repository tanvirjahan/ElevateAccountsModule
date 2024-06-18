Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient
Imports System.Collections.Generic
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://localhost/ColumbusRPTS/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class LoginWebService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function CheckAgentsLogin(ByVal MyLoginServiceClassRequest As MyLoginServiceClass) As String
        Try
            Dim SaveOrCheck As Integer = 0
            Dim AgentCode As String = String.Empty
            Dim AgentSubCode As String = String.Empty
            Dim CountryLogin As String = String.Empty
            Dim ConnectionString As String = String.Empty


            SaveOrCheck = MyLoginServiceClassRequest.SaveOrCheck
            AgentCode = MyLoginServiceClassRequest.AgentCode
            AgentSubCode = MyLoginServiceClassRequest.AgentSubCode
            CountryLogin = MyLoginServiceClassRequest.CountryLogin
            ConnectionString = MyLoginServiceClassRequest.ConnectionString
            Dim MyConnectionString As String = String.Empty

            MyConnectionString = ConfigurationManager.ConnectionStrings(ConnectionString).ConnectionString

            Dim Conn As New SqlConnection
            Dim LoginIp As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            Conn = New SqlConnection(MyConnectionString)
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

                '  EventLog.WriteEntry("Loginmsg1", MyConnectionString)

                Cmd = New SqlCommand(Str, Conn)
                Dr = Cmd.ExecuteReader
                If Dr.Read Then
                    If Not IsDBNull(Dr("AgentLoginDate")) Then
                        MyDate = Dr("AgentLoginDate")
                    End If
                    Dr.Close()
                    Cmd.Cancel()
                    '  EventLog.WriteEntry("Loginmsg2", MyDate)
                    If AgentSubCode = String.Empty Then
                        Str = "Update  Tbl_CheckAgents_Login Set AgentLoginDate='" & Format(CDate(Date.Now), "yyyy/MM/dd HH:mm:ss") & "',AgentLoginFlag=1,AgentLastLoginDate='" & Format(CDate(MyDate), "yyyy/MM/dd HH:mm:ss") & "',CountryLogin='" & CountryLogin & "',LoginIp ='" & LoginIp & "'  Where AgentCode='" & AgentCode & "'"
                    Else
                        Str = "Update  Tbl_CheckAgents_Login Set AgentLoginDate='" & Format(CDate(Date.Now), "yyyy/MM/dd HH:mm:ss") & "',AgentLoginFlag=1,AgentLastLoginDate='" & Format(CDate(MyDate), "yyyy/MM/dd HH:mm:ss") & "',CountryLogin='" & CountryLogin & "',LoginIp ='" & LoginIp & "'  Where AgentCode='" & AgentSubCode & "' And AgentSubCode='" & AgentCode & "'"
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
                        Str = "Insert Into Tbl_CheckAgents_Login(AgentCode,AgentSubCode,AgentLoginDate,AgentLoginFlag,AgentLastLoginDate,CountryLogin,LoginIp)Values('" & AgentCode & "','" & AgentSubCode & "','" & Format(CDate(Date.Now), "yyyy/MM/dd HH:mm:ss") & "',1,'" & Format(CDate(Date.Now), "yyyy/MM/dd HH:mm:ss") & "','" & CountryLogin & "','" & LoginIp & "')"
                    Else
                        Str = "Insert Into Tbl_CheckAgents_Login(AgentCode,AgentSubCode,AgentLoginDate,AgentLoginFlag,AgentLastLoginDate,CountryLogin,LoginIp)Values('" & AgentSubCode & "','" & AgentCode & "','" & Format(CDate(Date.Now), "yyyy/MM/dd HH:mm:ss") & "',1,'" & Format(CDate(Date.Now), "yyyy/MM/dd HH:mm:ss") & "','" & CountryLogin & "','" & LoginIp & "')"
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
                    ' HttpContext.Current.Session("LoginCorrect") = "LoginCorrect"
                    Cmd.Cancel()
                    Conn.Close()
                    Return ""
                Else
                    'If HttpContext.Current.Session("LoginCorrect") = "LoginCorrect" Then
                    '    Conn.Close()
                    '    Return ""
                    'End If
                    Return "You Have No Right To Access This Page Please Login In"
                End If
            End If
            Cmd.Cancel()
            Conn.Close()

            Return ""
        Catch ex As Exception
            'HttpContext.Current.Session("LoginCorrect") = "Error Login"
            Return ex.Message.ToString()
        End Try
    End Function


End Class

Public Class MyLoginServiceClass
    Dim _SaveOrCheck As Integer
    Dim _AgentCode As String
    Dim _AgentSubCode As String
    Dim _CountryLogin As String
    Dim _ConnectionString As String
    Property SaveOrCheck() As Integer
        Get
            Return _SaveOrCheck
        End Get
        Set(ByVal Value As Integer)
            _SaveOrCheck = Value
        End Set
    End Property
    Property AgentCode() As String
        Get
            Return _AgentCode
        End Get
        Set(ByVal Value As String)
            _AgentCode = Value
        End Set
    End Property
    Property AgentSubCode() As String
        Get
            Return _AgentSubCode
        End Get
        Set(ByVal Value As String)
            _AgentSubCode = Value
        End Set
    End Property
    Property CountryLogin() As String
        Get
            Return _CountryLogin
        End Get
        Set(ByVal Value As String)
            _CountryLogin = Value
        End Set
    End Property
    Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
        Set(ByVal Value As String)
            _ConnectionString = Value
        End Set
    End Property
End Class