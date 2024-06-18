Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class SearchForHotels

    Private _listOfHotel As New List(Of Hotel)
    
    Public Property Hotel() As List(Of Hotel)
        Get
            Return _listOfHotel
        End Get
        Set(ByVal value As List(Of Hotel))
            _listOfHotel = value
        End Set
    End Property



End Class


