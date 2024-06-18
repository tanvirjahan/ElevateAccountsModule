Imports Microsoft.VisualBasic

Public Class ClassCityArea
    Private strvalue As String
    Private strtext As String
    Private strtype As String
    Property ListCity() As String
        Get
            Return strvalue
        End Get
        Set(ByVal Value As String)
            strvalue = Value
        End Set
    End Property
    Property ListText() As String
        Get
            Return strtext
        End Get
        Set(ByVal Value As String)
            strtext = Value
        End Set
    End Property
    Property ListType() As String
        Get
            Return strtype
        End Get
        Set(ByVal Value As String)
            strtype = Value
        End Set
    End Property
End Class
