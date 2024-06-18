Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization
Imports System.IO
Imports System.Collections.Generic

<System.Web.Services.WebServiceBindingAttribute(Name:="LoginServiceSoap", [Namespace]:="http://localhost/ColumbusRPTS/")> _
Partial Public Class ConnectWebService
    Inherits SoapHttpClientProtocol
    Public Sub New()
        Me.Url = "http://localhost/ColumbusRPTS/LoginWebService.asmx"

    End Sub
    ' // If Jordan Uncomment This

    '<System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/ColumbusRPTS/CheckAgentsLogin", RequestNamespace:="http://localhost/ColumbusRPTS/", ResponseNamespace:="http://localhost/ColumbusRPTS/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    'Public Function CheckAgentsLogin(ByVal MyLoginServiceClassRequest As MyLoginServiceClassClient) As String
    '    Dim results() As Object = Me.Invoke("CheckAgentsLogin", New Object() {MyLoginServiceClassRequest})
    '    Return CType(results(0), String)
    'End Function
    '<System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/ColumbusRPTS/HelloWorld", RequestNamespace:="http://localhost/ColumbusRPTS/", ResponseNamespace:="http://localhost/ColumbusRPTS/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    'Public Function HelloWorld() As String
    '    Dim results() As Object = Me.Invoke("HelloWorld", New Object(-1) {})
    '    Return CType(results(0), String)
    'End Function


    ' // If Uae UnCommentThis

    Public Function CheckAgentsLogin(ByVal MyLoginServiceClassRequest As MyLoginServiceClassClient) As String
        Dim MyLogin As New LoginWebService
        Dim MyLoginServiceClass As New MyLoginServiceClass
        MyLoginServiceClass.AgentCode = MyLoginServiceClassRequest.AgentCode
        MyLoginServiceClass.AgentSubCode = MyLoginServiceClassRequest.AgentSubCode
        MyLoginServiceClass.ConnectionString = MyLoginServiceClassRequest.ConnectionString
        MyLoginServiceClass.CountryLogin = MyLoginServiceClassRequest.CountryLogin
        MyLoginServiceClass.SaveOrCheck = MyLoginServiceClassRequest.SaveOrCheck
        Return MyLogin.CheckAgentsLogin(MyLoginServiceClass)
    End Function
    Public Function HelloWorld() As String
        Dim MyLogin As New LoginWebService
        Return MyLogin.HelloWorld()
    End Function

End Class


<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://localhost/ColumbusRPTS/")> _
Partial Public Class MyLoginServiceClassClient

    Private saveOrCheckField As Integer

    Private agentCodeField As String

    Private agentSubCodeField As String

    Private countryLoginField As String

    Private connectionStringField As String


    Public Property SaveOrCheck() As Integer
        Get
            Return Me.saveOrCheckField
        End Get
        Set(ByVal value As Integer)
            Me.saveOrCheckField = value
        End Set
    End Property


    Public Property AgentCode() As String
        Get
            Return Me.agentCodeField
        End Get
        Set(ByVal value As String)
            Me.agentCodeField = value
        End Set
    End Property


    Public Property AgentSubCode() As String
        Get
            Return Me.agentSubCodeField
        End Get
        Set(ByVal value As String)
            Me.agentSubCodeField = value
        End Set
    End Property


    Public Property CountryLogin() As String
        Get
            Return Me.countryLoginField
        End Get
        Set(ByVal value As String)
            Me.countryLoginField = value
        End Set
    End Property


    Public Property ConnectionString() As String
        Get
            Return Me.connectionStringField
        End Get
        Set(ByVal value As String)
            Me.connectionStringField = value
        End Set
    End Property
End Class

