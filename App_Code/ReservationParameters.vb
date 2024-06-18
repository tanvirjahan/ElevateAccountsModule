Imports Microsoft.VisualBasic

Public Class ReservationParameters
    Private _Agent As String = ""
    Private _AgentCode As String = ""
    Private _LoginType As String = ""
    Private _GlobalUserName As String = ""
    Private _AgentCompany As String = ""
    Private _NoOfNightLimit As String = ""
    Private _ChildAgeLimit As String = ""
    Private _DivCode As String = ""
    Private _AbsoluteUrl As String = ""
    Private _LoginIp As String = ""
    Private _PhoneNo As String = ""
    Private _ShortName As String = ""
    Private _Logo As String = ""
    Private _OwnCompany As String = ""
    Private _Cumulative As String = ""
    Private _CssTheme As String = ""
    Private _WhiteLabel As String = ""
    Private _SubUserCode As String = ""
    Private _IsSubUser As String = ""
    Private _LoginIpLocationName As String = ""

    Public Property AgentName() As String
        Get
            Return _Agent
        End Get
        Set(ByVal Value As String)
            _Agent = Value
        End Set
    End Property
    Public Property AgentCode() As String
        Get
            Return _AgentCode
        End Get
        Set(ByVal Value As String)
            _AgentCode = Value
        End Set
    End Property
    Public Property LoginType() As String
        Get
            Return _LoginType
        End Get
        Set(ByVal Value As String)
            _LoginType = Value
        End Set
    End Property

    Public Property GlobalUserName() As String
        Get
            Return _GlobalUserName
        End Get
        Set(ByVal Value As String)
            _GlobalUserName = Value
        End Set
    End Property

    Public Property AgentCompany() As String
        Get
            Return _AgentCompany
        End Get
        Set(ByVal Value As String)
            _AgentCompany = Value
        End Set
    End Property

    Public Property NoOfNightLimit() As String
        Get
            Return _NoOfNightLimit
        End Get
        Set(ByVal Value As String)
            _NoOfNightLimit = Value
        End Set

    End Property

    Public Property ChildAgeLimit() As String
        Get
            Return _ChildAgeLimit
        End Get
        Set(ByVal Value As String)
            _ChildAgeLimit = Value
        End Set
    End Property
    Public Property DivCode() As String
        Get
            Return _DivCode
        End Get
        Set(ByVal Value As String)
            _DivCode = Value
        End Set
    End Property
    Public Property AbsoluteUrl() As String
        Get
            Return _AbsoluteUrl
        End Get
        Set(ByVal Value As String)
            _AbsoluteUrl = Value
        End Set
    End Property
    Public Property LoginIp() As String
        Get
            Return _LoginIp
        End Get
        Set(ByVal Value As String)
            _LoginIp = Value
        End Set
    End Property
    Public Property PhoneNo() As String
        Get
            Return _PhoneNo
        End Get
        Set(ByVal Value As String)
            _PhoneNo = Value
        End Set
    End Property
    Public Property ShortName() As String
        Get
            Return _ShortName
        End Get
        Set(ByVal Value As String)
            _ShortName = Value
        End Set
    End Property
    Public Property Logo() As String
        Get
            Return _Logo
        End Get
        Set(ByVal Value As String)
            _Logo = Value
        End Set
    End Property
    Public Property OwnCompany() As String
        Get
            Return _OwnCompany
        End Get
        Set(ByVal Value As String)
            _OwnCompany = Value
        End Set
    End Property
    Public Property WhiteLabel() As String
        Get
            Return _WhiteLabel
        End Get
        Set(ByVal Value As String)
            _WhiteLabel = Value
        End Set
    End Property
    Public Property CssTheme() As String
        Get
            Return _CssTheme
        End Get
        Set(ByVal Value As String)
            _CssTheme = Value
        End Set
    End Property
    Public Property Cumulative() As String
        Get
            Return _Cumulative
        End Get
        Set(ByVal Value As String)
            _Cumulative = Value
        End Set
    End Property
    Public Property SubUserCode() As String
        Get
            Return _SubUserCode
        End Get
        Set(ByVal Value As String)
            _SubUserCode = Value
        End Set
    End Property
    Public Property IsSubUser() As String
        Get
            Return _IsSubUser
        End Get
        Set(ByVal Value As String)
            _IsSubUser = Value
        End Set
    End Property
    Public Property LoginIpLocationName() As String
        Get
            Return _LoginIpLocationName
        End Get
        Set(ByVal Value As String)
            _LoginIpLocationName = Value
        End Set
    End Property

End Class
