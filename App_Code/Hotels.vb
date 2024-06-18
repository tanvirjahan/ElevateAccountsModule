Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class Hotel
    Private _listOfRoom As New List(Of clsAgentsOnline.BookRoom)
    Private _searchDetails As New SearchCreteria
   
    Public Property SearchDetails() As SearchCreteria
        Get
            Return _searchDetails
        End Get
        Set(ByVal value As SearchCreteria)
            _searchDetails = value
        End Set
    End Property
    Public Property Room() As List(Of clsAgentsOnline.BookRoom)
        Get
            Return _listOfRoom
        End Get
        Set(ByVal value As List(Of clsAgentsOnline.BookRoom))
            _listOfRoom = value
        End Set
    End Property

End Class
