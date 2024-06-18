Imports Microsoft.VisualBasic
Namespace MyClasses

    <Serializable()>
    Public Class CountryModel
        Public Sub CountryModel()
            _chkselect = 0
            _ordertype = 0
        End Sub



        Private _ctrycode As String
        Private _ctryname As String
        Private _plgrpname As String
        Private _plgrpcode As String
        Private _countrygroupname As String
        Private _countrygroupcode As String
        Private _chkselect As Integer
        Private _ordertype As Integer

        Public Property ordertype() As Integer
            Get
                Return _ordertype
            End Get
            Set(ByVal value As Integer)
                _ordertype = value
            End Set
        End Property

        Public Property chkselect() As Integer
            Get
                Return _chkselect
            End Get
            Set(ByVal value As Integer)
                _chkselect = value
            End Set
        End Property

        Public Property countrygroupcode() As String
            Get
                Return _countrygroupcode
            End Get
            Set(ByVal value As String)
                _countrygroupcode = value
            End Set
        End Property
        Public Property countrygroupname() As String
            Get
                Return _countrygroupname
            End Get
            Set(ByVal value As String)
                _countrygroupname = value
            End Set
        End Property
        Public Property plgrpcode() As String
            Get
                Return _plgrpcode
            End Get
            Set(ByVal value As String)
                _plgrpcode = value
            End Set
        End Property
        Public Property plgrpname() As String
            Get
                Return _plgrpname
            End Get
            Set(ByVal value As String)
                _plgrpname = value
            End Set
        End Property
        Public Property ctryname() As String
            Get
                Return _ctryname
            End Get
            Set(ByVal value As String)
                _ctryname = value
            End Set
        End Property
        Public Property ctrycode() As String
            Get
                Return _ctrycode
            End Get
            Set(ByVal value As String)
                _ctrycode = value
            End Set
        End Property

    End Class

    <Serializable()>
    Public Class CountryAgentModel
        Public Sub CountryAgentModel()
            _chkselect = 0
            _ordertype = 0
        End Sub


        Private _agentcode As String
        Private _agentname As String
        Private _ctrycodename As String
        Private _ctryname As String
        Private _ctrycode As String
        Private _chkselect As Integer
        Private _ordertype As Integer
        Private _customergroupname As String
        Private _customergroupcode As String

        Public Property ordertype() As Integer
            Get
                Return _ordertype
            End Get
            Set(ByVal value As Integer)
                _ordertype = value
            End Set
        End Property

        Public Property chkselect() As Integer
            Get
                Return _chkselect
            End Get
            Set(ByVal value As Integer)
                _chkselect = value
            End Set
        End Property


        Public Property agentcode() As String
            Get
                Return _agentcode
            End Get
            Set(ByVal value As String)
                _agentcode = value
            End Set
        End Property
        Public Property agentname() As String
            Get
                Return _agentname
            End Get
            Set(ByVal value As String)
                _agentname = value
            End Set
        End Property
        Public Property ctrycodename() As String
            Get
                Return _ctrycodename
            End Get
            Set(ByVal value As String)
                _ctrycodename = value
            End Set
        End Property
        Public Property ctryname() As String
            Get
                Return _ctryname
            End Get
            Set(ByVal value As String)
                _ctryname = value
            End Set
        End Property
        Public Property ctrycode() As String
            Get
                Return _ctrycode
            End Get
            Set(ByVal value As String)
                _ctrycode = value
            End Set
        End Property
        Public Property customergroupname() As String
            Get
                Return _customergroupname
            End Get
            Set(ByVal value As String)
                _customergroupname = value
            End Set
        End Property
        Public Property customergroupcode() As String
            Get
                Return _customergroupcode
            End Get
            Set(ByVal value As String)
                _customergroupcode = value
            End Set
        End Property
    End Class

    Public Class AutocompleteClass
        Dim _Id As String
        Dim _Name As String
        Dim _IsCode As String
        Property Id() As String
            Get
                Return _Id
            End Get
            Set(ByVal Value As String)
                _Id = Value
            End Set
        End Property

        Property IsCode() As String
            Get
                Return _IsCode
            End Get
            Set(ByVal Value As String)
                _IsCode = Value
            End Set
        End Property


        Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property
    End Class

    Public Class ITineraryclass
        Dim _day As String
        Dim _city As String
        Dim _route As String
        Dim _itinerary As String
        Property Day() As String
            Get
                Return _day
            End Get
            Set(ByVal Value As String)
                _day = Value
            End Set
        End Property
        Property City() As String
            Get
                Return _city
            End Get
            Set(ByVal Value As String)
                _city = Value
            End Set
        End Property
        Property Route() As String
            Get
                Return _route
            End Get
            Set(ByVal Value As String)
                _route = Value
            End Set
        End Property
        Property Itinerary() As String
            Get
                Return _itinerary
            End Get
            Set(ByVal Value As String)
                _itinerary = Value
            End Set
        End Property
    End Class

    Public Class QuotationAdd
        Dim _packageid As String
        Dim _market As String
        Dim _sellingtype As String
        Dim _agent As String
        Dim _from As DateTime
        Dim _to As DateTime
        Dim _roomcat As String
        Dim _divfactor As String
        Dim _choice1 As String
        Dim _choice2 As String
        Dim _choice3 As String
        Dim _roomcatstr As String
        Dim _partystr As String
        Dim _choice As Integer



        Property Packageid() As String
            Get
                Return _packageid
            End Get
            Set(ByVal Value As String)
                _packageid = Value
            End Set
        End Property

        Property Market() As String
            Get
                Return _market
            End Get
            Set(ByVal Value As String)
                _market = Value
            End Set
        End Property
        Property SellingType() As String
            Get
                Return _sellingtype
            End Get
            Set(ByVal Value As String)
                _sellingtype = Value
            End Set
        End Property
        Property Agent() As String
            Get
                Return _agent
            End Get
            Set(ByVal Value As String)
                _agent = Value
            End Set
        End Property

        Property FromDate() As String
            Get
                Return _from
            End Get
            Set(ByVal Value As String)
                _from = Value
            End Set

        End Property

        Property ToDate() As String
            Get
                Return _to
            End Get
            Set(ByVal Value As String)
                _to = Value
            End Set

        End Property

        Property RoomCat() As String
            Get
                Return _roomcat
            End Get
            Set(ByVal Value As String)
                _roomcat = Value
            End Set

        End Property

        Property Divfactor() As String
            Get
                Return _divfactor
            End Get
            Set(ByVal Value As String)
                _divfactor = Value
            End Set

        End Property

        Property Choice1() As String
            Get
                Return _choice1
            End Get
            Set(ByVal Value As String)
                _choice1 = Value
            End Set

        End Property
        Property Choice2() As String
            Get
                Return _choice2
            End Get
            Set(ByVal Value As String)
                _choice2 = Value
            End Set

        End Property

        Property Choice3() As String
            Get
                Return _choice3
            End Get
            Set(ByVal Value As String)
                _choice3 = Value
            End Set

        End Property
        Property RoomcatStr() As String
            Get
                Return _roomcatstr
            End Get
            Set(ByVal Value As String)
                _roomcatstr = Value
            End Set
        End Property
        Property ParyStr() As String
            Get
                Return _partystr
            End Get
            Set(ByVal Value As String)
                _partystr = Value
            End Set
        End Property
        Property Choice() As String
            Get
                Return _choice
            End Get
            Set(ByVal Value As String)
                _choice = Value
            End Set
        End Property
    End Class



    Public Class clsExcursionHedaer

        Private _excid, _spersoncode_office, _debitnoteno, _creditnoteno, _othsellcode, _plgrpcode, _currcode As String
        Private _spersoncode, _paycode, _collectby, _payref, _ticketno, _nationalitycode, _language, _prepaidid, _agentcode, _remarks As String
        Private _esettleid, _exc_provider, _costcurrcode, _invno, _incominginvno As String
        Private _dmc, _prepaidlineno As Integer
        Private _collectedamt, _convrate, _costconvrate As Decimal
        Private _requestdate, _coltdon As DateTime

        Property excid() As String
            Get
                Return _excid
            End Get
            Set(ByVal value As String)
                _excid = value
            End Set
        End Property

        Property requestdate() As DateTime
            Get
                Return _requestdate
            End Get
            Set(ByVal value As DateTime)
                _requestdate = value
            End Set
        End Property

        Property othsellcode() As String
            Get
                Return _othsellcode
            End Get
            Set(ByVal value As String)
                _othsellcode = value
            End Set
        End Property

        Property plgrpcode() As String
            Get
                Return _plgrpcode
            End Get
            Set(ByVal value As String)
                _plgrpcode = value
            End Set
        End Property

        Property currcode() As String
            Get
                Return _currcode
            End Get
            Set(ByVal value As String)
                _currcode = value
            End Set
        End Property

        Property convrate() As Decimal
            Get
                Return _convrate
            End Get
            Set(ByVal value As Decimal)
                _convrate = value
            End Set
        End Property

        Property spersoncode() As String
            Get
                Return _spersoncode
            End Get
            Set(ByVal value As String)
                _spersoncode = value
            End Set
        End Property

        Property paycode() As String
            Get
                Return _paycode
            End Get
            Set(ByVal value As String)
                _paycode = value
            End Set
        End Property

        Property collectby() As String
            Get
                Return _collectby
            End Get
            Set(ByVal value As String)
                _collectby = value
            End Set
        End Property

        Property payref() As String
            Get
                Return _payref
            End Get
            Set(ByVal value As String)
                _payref = value
            End Set
        End Property

        Property ticketno() As String
            Get
                Return _ticketno
            End Get
            Set(ByVal value As String)
                _ticketno = value
            End Set
        End Property

        Property nationalitycode() As String
            Get
                Return _nationalitycode
            End Get
            Set(ByVal value As String)
                _nationalitycode = value
            End Set
        End Property

        Property language() As String
            Get
                Return _language
            End Get
            Set(ByVal value As String)
                _language = value
            End Set
        End Property

        Property prepaidid() As String
            Get
                Return _prepaidid
            End Get
            Set(ByVal value As String)
                _prepaidid = value
            End Set
        End Property

        Property prepaidlineno() As Integer
            Get
                Return _prepaidlineno
            End Get
            Set(ByVal value As Integer)
                _prepaidlineno = value
            End Set
        End Property

        Property agentcode() As String
            Get
                Return _agentcode
            End Get
            Set(ByVal value As String)
                _agentcode = value
            End Set
        End Property

        Property remarks() As String
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Property esettleid() As String
            Get
                Return _esettleid
            End Get
            Set(ByVal value As String)
                _esettleid = value
            End Set
        End Property

        Property coltdon() As DateTime
            Get
                Return _coltdon
            End Get
            Set(ByVal value As DateTime)
                _coltdon = value
            End Set
        End Property

        Property exc_provider() As String
            Get
                Return _exc_provider
            End Get
            Set(ByVal value As String)
                _exc_provider = value
            End Set
        End Property

        Property costcurrcode() As String
            Get
                Return _costcurrcode
            End Get
            Set(ByVal value As String)
                _costcurrcode = value
            End Set
        End Property

        Property costconvrate() As Decimal
            Get
                Return _costconvrate
            End Get
            Set(ByVal value As Decimal)
                _costconvrate = value
            End Set
        End Property

        Property invno() As String
            Get
                Return _invno
            End Get
            Set(ByVal value As String)
                _invno = value
            End Set
        End Property

        Property incominginvno() As String
            Get
                Return _incominginvno
            End Get
            Set(ByVal value As String)
                _incominginvno = value
            End Set
        End Property

        Property creditnoteno() As String
            Get
                Return _creditnoteno
            End Get
            Set(ByVal value As String)
                _creditnoteno = value
            End Set
        End Property

        Property debitnoteno() As String
            Get
                Return _debitnoteno
            End Get
            Set(ByVal value As String)
                _debitnoteno = value
            End Set
        End Property

        Property collectedamt() As Decimal
            Get
                Return _collectedamt
            End Get
            Set(ByVal value As Decimal)
                _collectedamt = value
            End Set
        End Property

        Property dmc() As Integer
            Get
                Return _dmc
            End Get
            Set(ByVal value As Integer)
                _dmc = value
            End Set
        End Property

        Property spersoncode_office() As String
            Get
                Return _spersoncode_office
            End Get
            Set(ByVal value As String)
                _spersoncode_office = value
            End Set
        End Property


    End Class
End Namespace

