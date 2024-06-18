
#Region "FillGridValues"
Public Class FillGridValues
    Dim RlineNo As Integer
    Dim Adult As Integer
    Dim Child As Integer
    Dim Unit As Integer
    Dim DateStr As String
    Dim OtherType As String
    Dim OtherCatType As String
    Dim Index As Integer
    Property RlineNoText() As Integer
        Get
            Return RlineNo
        End Get
        Set(ByVal Value As Integer)
            RlineNo = Value
        End Set
    End Property
    Property IndexText() As Integer
        Get
            Return Index
        End Get
        Set(ByVal Value As Integer)
            Index = Value
        End Set
    End Property
    Property AdultText() As Integer
        Get
            Return Adult
        End Get
        Set(ByVal Value As Integer)
            Adult = Value
        End Set
    End Property
    Property ChildText() As Integer
        Get
            Return Child
        End Get
        Set(ByVal Value As Integer)
            Child = Value
        End Set
    End Property
    Property UnitText() As Integer
        Get
            Return Unit
        End Get
        Set(ByVal Value As Integer)
            Unit = Value
        End Set
    End Property
    Property DateStrText() As String
        Get
            Return DateStr
        End Get
        Set(ByVal Value As String)
            DateStr = Value
        End Set
    End Property
    Property OtherTypeText() As String
        Get
            Return OtherType
        End Get
        Set(ByVal Value As String)
            OtherType = Value
        End Set
    End Property
    Property OtherCatTypeText() As String
        Get
            Return OtherCatType
        End Get
        Set(ByVal Value As String)
            OtherCatType = Value
        End Set
    End Property
End Class
#End Region
