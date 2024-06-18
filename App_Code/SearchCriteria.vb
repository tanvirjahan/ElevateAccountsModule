Imports Microsoft.VisualBasic

Public Class SearchCreteria
    Private _Country As String
    Private _City As String
    Private _CheckInDate As String
    Private _CheckOutDate As String
    Private _CatCode As String
    Private _SectorCode As String
    Private _PartyCode As String
    Private _Status As Integer
    Private _RoomCnt As Integer
    Private _requestid As String
    Private _basketid As String
    Private _hotellineno As Integer
    Private _hotelsearch As String
    Private _roomsearch As String
    Private _Mode As String = String.Empty



    Public Property Country As String
        Get
            Return _Country
        End Get
        Set(ByVal value As String)
            _Country = value
        End Set

    End Property

    Public Property City As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set

    End Property

    Public Property CheckInDate As String
        Get
            Return _CheckInDate
        End Get
        Set(ByVal value As String)
            _CheckInDate = value
        End Set

    End Property

    Public Property CheckOutDate As String
        Get
            Return _CheckOutDate
        End Get
        Set(ByVal value As String)
            _CheckOutDate = value
        End Set

    End Property

    Public Property CatCode As String
        Get
            Return _CatCode
        End Get
        Set(ByVal value As String)
            _CatCode = value
        End Set

    End Property

    Public Property SectorCode As String
        Get
            Return _SectorCode
        End Get
        Set(ByVal value As String)
            _SectorCode = value
        End Set

    End Property

    Public Property PartyCode As String
        Get
            Return _PartyCode
        End Get
        Set(ByVal value As String)
            _PartyCode = value
        End Set

    End Property

    Public Property Status As Integer
        Get
            Return _Status
        End Get
        Set(ByVal value As Integer)
            _Status = value
        End Set

    End Property

    Public Property RoomCnt As Integer
        Get
            Return _RoomCnt
        End Get
        Set(ByVal value As Integer)
            _RoomCnt = value
        End Set

    End Property

    Public Property requestid As String
        Get
            Return _requestid
        End Get
        Set(ByVal value As String)
            _requestid = value
        End Set

    End Property

    Public Property basketid As String
        Get
            Return _basketid
        End Get
        Set(ByVal value As String)
            _basketid = value
        End Set

    End Property

    Public Property hotellineno As Integer
        Get
            Return _hotellineno
        End Get
        Set(ByVal value As Integer)
            _hotellineno = value
        End Set

    End Property

    Public Property HotelSearch As String
        Get
            Return _hotelsearch
        End Get
        Set(ByVal value As String)
            _hotelsearch = value
        End Set

    End Property

    Public Property RoomSearch As String
        Get
            Return _roomsearch
        End Get
        Set(ByVal value As String)
            _roomsearch = value
        End Set

    End Property

    Public Property Mode As String
        Get
            Return _Mode
        End Get
        Set(ByVal value As String)
            _Mode = value
        End Set

    End Property
End Class