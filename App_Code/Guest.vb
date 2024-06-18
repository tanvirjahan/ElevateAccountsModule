Imports Microsoft.VisualBasic

Public Class Guest
    Private _title As String
    Private _firstname As String
    Private _lastname As String
    Private _arrFlightNo As String
    Private _arrTime As String
    Private _arrAirport As String
    Private _depFlightNo As String
    Private _depTime As String
    Private _DepAirport As String
    Private _Age As String
    Private _guestLineNo As String

    Public Property GuestLineNo() As String
        Get
            Return _guestLineNo
        End Get
        Set(ByVal value As String)
            _guestLineNo = value
        End Set

    End Property
    Public Property Age() As String
        Get
            Return _Age
        End Get
        Set(ByVal value As String)
            _Age = value
        End Set

    End Property


    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set

    End Property

    Public Property FirstName() As String
        Get
            Return _firstname
        End Get
        Set(ByVal value As String)
            _firstname = value
        End Set

    End Property

    Public Property LastName() As String
        Get
            Return _lastname
        End Get
        Set(ByVal value As String)
            _lastname = value
        End Set

    End Property

    Public Property ArrFlightNo() As String
        Get
            Return _arrFlightNo
        End Get
        Set(ByVal value As String)
            _arrFlightNo = value
        End Set

    End Property

    Public Property ArrTime() As String
        Get
            Return _arrTime
        End Get
        Set(ByVal value As String)
            _arrTime = value
        End Set

    End Property

    Public Property ArrAirport() As String
        Get
            Return _arrAirport
        End Get
        Set(ByVal value As String)
            _arrAirport = value
        End Set

    End Property

    Public Property DepFlightNo() As String
        Get
            Return _depFlightNo
        End Get
        Set(ByVal value As String)
            _depFlightNo = value
        End Set

    End Property

    Public Property DepTime() As String
        Get
            Return _depTime
        End Get
        Set(ByVal value As String)
            _depTime = value
        End Set

    End Property

    Public Property DepAirport() As String
        Get
            Return _DepAirport
        End Get
        Set(ByVal value As String)
            _DepAirport = value
        End Set

    End Property

End Class
