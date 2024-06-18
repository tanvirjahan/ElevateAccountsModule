Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class SearchDetails
    Private _Country As String = String.Empty
    Private _City As String = String.Empty
    Private _CheckInDate As String = String.Empty
    Private _CheckOutDate As String = String.Empty
    Private _CatCode As String = String.Empty
    Private _SectorCode As String = String.Empty
    Private _PartyCode As String = String.Empty
    Private _hotellineno As Integer = 0
    Private _TotalRoom As Integer = 0
    Private _guestDetails As New List(Of SearchGuest)

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

    Public Property hotellineno As Integer
        Get
            Return _hotellineno
        End Get
        Set(ByVal value As Integer)
            _hotellineno = value
        End Set

    End Property
    Public Property TotalRoom As Integer
        Get
            Return _TotalRoom
        End Get
        Set(ByVal value As Integer)
            _TotalRoom = value
        End Set

    End Property
    Public Property SearchGuest As List(Of SearchGuest)
        Get
            Return _guestDetails
        End Get
        Set(ByVal value As List(Of SearchGuest))
            _guestDetails = value
        End Set
    End Property

End Class
