Imports Microsoft.VisualBasic

Public Class SearchGuest

    Private _Adult As Integer = 0
    Private _ChildAge As Integer = 0

    Public Property Adult As String
        Get
            Return _Adult
        End Get
        Set(ByVal value As String)
            _Adult = value
        End Set
    End Property


    Public Property ChildAge As String
        Get
            Return _ChildAge
        End Get
        Set(ByVal value As String)
            _ChildAge = value
        End Set
    End Property
End Class
