Imports Microsoft.VisualBasic

Public Class clsMaster
    Private strtext As String
    Private strvalue As String
    Property ListText() As String
        Get
            Return strtext
        End Get
        Set(ByVal Value As String)
            strtext = Value
        End Set
    End Property
    Property ListValue() As String
        Get
            Return strvalue
        End Get
        Set(ByVal Value As String)
            strvalue = Value
        End Set
    End Property

End Class
