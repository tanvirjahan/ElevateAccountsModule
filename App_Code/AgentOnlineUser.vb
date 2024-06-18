Imports Microsoft.VisualBasic

Public Class AgentOnlineUser
    Public _userName As String
    Public _password As String
    Public _agentCode As String
    Public _companyName As String
    Public _globalAgentUserName As String
    Public _type As String
    Public _currCode As String
    Public _ThumbnailImage As String
    Public _MissingHotelImage As String
    Public _ReportImage As String
    Public _OtherImage As String

    Public Property UserName As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Property AgentCode As String
        Get
            Return _agentCode
        End Get
        Set(ByVal value As String)
            _agentCode = value
        End Set
    End Property

    Public Property CompanyName As String
        Get
            Return _companyName
        End Get
        Set(ByVal value As String)
            _companyName = value
        End Set
    End Property

    Public Property GlobalAgentUserName As String
        Get
            Return _globalAgentUserName
        End Get
        Set(ByVal value As String)
            _globalAgentUserName = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Public Property ThumbnailImage As String
        Get
            Return _ThumbnailImage
        End Get
        Set(ByVal value As String)
            _ThumbnailImage = value
        End Set
    End Property

    Public Property CurrencyCode As String
        Get
            Return _currCode
        End Get
        Set(ByVal value As String)
            _currCode = value
        End Set
    End Property

    Public Property MissingHotelImage As String
        Get
            Return _MissingHotelImage
        End Get
        Set(ByVal value As String)
            _MissingHotelImage = value
        End Set
    End Property

    Public Property ReportImage As String
        Get
            Return _ReportImage
        End Get
        Set(ByVal value As String)
            _ReportImage = value
        End Set
    End Property

    Public Property OtherImage As String
        Get
            Return _OtherImage
        End Get
        Set(ByVal value As String)
            _OtherImage = value
        End Set
    End Property




End Class
