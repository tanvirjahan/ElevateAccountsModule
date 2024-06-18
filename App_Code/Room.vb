Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class Room


    Private _listOfPrice As New List(Of Price)
    Private _listOfPromotion As New List(Of Promotion)
    Private _listOfGuest As New List(Of Guest)

    Public Property Price() As List(Of Price)
        Get
            Return _listOfPrice
        End Get
        Set(ByVal value As List(Of Price))
            _listOfPrice = value
        End Set
    End Property

    Public Property Promotion() As List(Of Promotion)
        Get
            Return _listOfPromotion
        End Get
        Set(ByVal value As List(Of Promotion))
            _listOfPromotion = value
        End Set
    End Property

    Public Property Guest() As List(Of Guest)
        Get
            Return _listOfGuest
        End Get
        Set(ByVal value As List(Of Guest))
            _listOfGuest = value
        End Set
    End Property

End Class
