Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
'Imports clsAgentsOnline.BookNow.GuestInfo

Public Class clsAgentsOnline

    'hotel details is stored in this main class


    ' booking details is stored in the main class

    Public Class BookForHotel
        Private _listOfHotels As New List(Of Hotel)
        Public Property Hotels As List(Of Hotel)
            Get
                Return _listOfHotels
            End Get
            Set(ByVal value As List(Of Hotel))
                _listOfHotels = value
            End Set

        End Property

    End Class

    Public Class SpecialEvents
        Private _partyCode As String = String.Empty
        Private _optionNo As String = String.Empty
        Private _roomTypeCode As String = String.Empty
        Private _mealCode As String = String.Empty
        Private _eventDate As String = String.Empty
        Private _eventName As String = String.Empty
        Private _isCoumpulsory As Boolean = False
        Private _eventCode As String = String.Empty
        Private _guestType As String = String.Empty
        Private _noOfpax As Integer = 0
        Private _rate As Decimal = 0.0
        Private _salePrice As Decimal = 0
        Private _remarks As String = String.Empty
        Private _roomNo As String = String.Empty

        Public Property PartyCode As String
            Get
                Return _partyCode
            End Get
            Set(ByVal value As String)
                _partyCode = value
            End Set
        End Property
        Public Property OptionNo As String
            Get
                Return _optionNo
            End Get
            Set(ByVal value As String)
                _optionNo = value
            End Set
        End Property
        Public Property RoomTypeCode As String
            Get
                Return _roomTypeCode
            End Get
            Set(ByVal value As String)
                _roomTypeCode = value
            End Set
        End Property

        Public Property MealCode As String
            Get
                Return _mealCode
            End Get
            Set(ByVal value As String)
                _mealCode = value
            End Set
        End Property
        Public Property EventDate As String
            Get
                Return _eventDate
            End Get
            Set(ByVal value As String)
                _eventDate = value
            End Set
        End Property
        Public Property EventName As String
            Get
                Return _eventName
            End Get
            Set(ByVal value As String)
                _eventName = value
            End Set
        End Property
        Public Property IsCompulsory As Boolean
            Get
                Return _isCoumpulsory
            End Get
            Set(ByVal value As Boolean)
                _isCoumpulsory = value
            End Set
        End Property
        Public Property EventCode As String
            Get
                Return _eventCode
            End Get
            Set(ByVal value As String)
                _eventCode = value
            End Set
        End Property
        Public Property GeustType As String
            Get
                Return _guestType
            End Get
            Set(ByVal value As String)
                _guestType = value
            End Set
        End Property
        Public Property NoOfPax As Integer
            Get
                Return _noOfpax
            End Get
            Set(ByVal value As Integer)
                _noOfpax = value
            End Set
        End Property
        Public Property Rate As Decimal
            Get
                Return _rate
            End Get
            Set(ByVal value As Decimal)
                _rate = value
            End Set
        End Property
        Public Property SalePrice As Decimal
            Get
                Return _salePrice
            End Get
            Set(ByVal value As Decimal)
                _salePrice = value
            End Set
        End Property
        Public Property Remarks As String
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property
        Public Property RoomNo As String
            Get
                Return _roomNo
            End Get
            Set(ByVal value As String)
                _roomNo = value
            End Set
        End Property


    End Class

        Public Class HandlingCharges
            Private _serviceType As String = String.Empty
            Private _serviceCode As String = String.Empty
            Private _LeadGuestName As String = String.Empty
            Private _date As DateTime
            Private _unit As Integer = 0
            Private _price As Decimal
            Private _value As Decimal
            Private _hotelLineNo As Integer

            Public Property ServiceType As String
                Get
                    Return _serviceType
                End Get
                Set(ByVal value As String)
                    _serviceType = value
                End Set
            End Property
            Public Property ServiceCode As String
                Get
                    Return _serviceCode
                End Get
                Set(ByVal value As String)
                    _serviceCode = value
                End Set
            End Property
            Public Property LeadGuestName As String
                Get
                    Return _LeadGuestName
                End Get
                Set(ByVal value As String)
                    LeadGuestName = value
                End Set
            End Property
            Public Property Datetime As DateTime
                Get
                    Return _date
                End Get
                Set(ByVal value As DateTime)
                    _date = value
                End Set
            End Property
            Public Property Unit As Integer
                Get
                    Return _unit
                End Get
                Set(ByVal value As Integer)
                    _unit = value
                End Set
            End Property
            Public Property Price As Decimal
                Get
                    Return _price
                End Get
                Set(ByVal value As Decimal)
                    _price = value
                End Set
            End Property
            Public Property Value As Decimal
                Get
                    Return _value
                End Get
                Set(ByVal value As Decimal)
                    _value = value
                End Set
            End Property
            Public Property HotelLineNo As Integer
                Get
                    Return _hotelLineNo
                End Get
                Set(ByVal value As Integer)
                    _hotelLineNo = value
                End Set
            End Property
    End Class
    Public Class VisaService
        Private _OLineNo As String
        Private _Price As Decimal
        Private _VisaTypeCode As String
        Private _VisaTypeName As String
        Private _VisaChargeCode As String
        Private _VisaChargeName As String
        Private _TotalPax As Integer
        Private _VisaHandOverDate As String

        Private _HotelLineNo As Integer 'changed by mohamed on 14/05/2016
        Private _TotalAdult As Integer 'changed by mohamed on 14/05/2016
        Private _TotalChild As Integer 'changed by mohamed on 14/05/2016
        Private _listOfGuestDetails As New List(Of GuestDetails) 'changed by mohamed on 14/05/2016

        Public Property ListOfGuestDetails() As List(Of GuestDetails) 'changed by mohamed on 14/05/2016
            Get
                Return _listOfGuestDetails
            End Get
            Set(ByVal value As List(Of GuestDetails))
                _listOfGuestDetails = value
            End Set
        End Property

        Public Property HotelLineNo() As Integer 'changed by mohamed on 14/05/2016
            Get
                Return _HotelLineNo
            End Get
            Set(ByVal value As Integer)
                _HotelLineNo = value
            End Set
        End Property


        Public Property TotalAdult() As Integer 'changed by mohamed on 14/05/2016
            Get
                Return _TotalAdult
            End Get
            Set(ByVal value As Integer)
                _TotalAdult = value
            End Set
        End Property
        Public Property TotalChild() As Integer 'changed by mohamed on 14/05/2016
            Get
                Return _TotalChild
            End Get
            Set(ByVal value As Integer)
                _TotalChild = value
            End Set
        End Property

        Public Property OLineNo() As String
            Get
                Return _OLineNo
            End Get
            Set(ByVal value As String)
                _OLineNo = value
            End Set
        End Property
        Public Property Price() As Decimal
            Get
                Return _Price
            End Get
            Set(ByVal value As Decimal)
                _Price = value
            End Set
        End Property
        Public Property VisaTypeCode() As String
            Get
                Return _VisaTypeCode
            End Get
            Set(ByVal value As String)
                _VisaTypeCode = value
            End Set
        End Property
        Public Property VisaTypeName() As String
            Get
                Return _VisaTypeName
            End Get
            Set(ByVal value As String)
                _VisaTypeName = value
            End Set
        End Property
        Public Property VisaChargeCode() As String
            Get
                Return _VisaChargeCode
            End Get
            Set(ByVal value As String)
                _VisaChargeCode = value
            End Set
        End Property
        Public Property VisaChargeName() As String
            Get
                Return _VisaChargeName
            End Get
            Set(ByVal value As String)
                _VisaChargeName = value
            End Set
        End Property
        Public Property TotalPax() As Integer
            Get
                Return _TotalPax
            End Get
            Set(ByVal value As Integer)
                _TotalPax = value
            End Set
        End Property
        Public Property VisaHandOverDate() As String
            Get
                Return _VisaHandOverDate
            End Get
            Set(ByVal value As String)
                _VisaHandOverDate = value
            End Set
        End Property


    End Class
    Public Class AirportService
        Private _ServiceType As String = String.Empty
        Private _ServiceTypeCode As String = String.Empty
        Private _TotalPrice As Decimal
        Private _TotalChildPrice As Decimal
        Private _TotalAdultPrice As Decimal
        Private _OLineNo As Integer
        Private _HotelLineNo As Integer
        Private _MeetingDate As Date
        Private _TotalAdult As Integer
        Private _TotalChild As Integer
        Private _ReceievedType As String

        Private _MeetingPlace As String 'changed by mohamed on 14/05/2016
        Private _listOfGuestDetails As New List(Of GuestDetails) 'changed by mohamed on 14/05/2016

        Public Property ListOfGuestDetails() As List(Of GuestDetails)
            Get
                Return _listOfGuestDetails
            End Get
            Set(ByVal value As List(Of GuestDetails))
                _listOfGuestDetails = value
            End Set
        End Property

        Public Property MeetingPlace() As String
            Get
                Return _MeetingPlace
            End Get
            Set(ByVal value As String)
                _MeetingPlace = value
            End Set
        End Property

        Public Property HotelLineNo() As Integer
            Get
                Return _HotelLineNo
            End Get
            Set(ByVal value As Integer)
                _HotelLineNo = value
            End Set
        End Property
        Public Property OLineNo() As Integer
            Get
                Return _OLineNo
            End Get
            Set(ByVal value As Integer)
                _OLineNo = value
            End Set
        End Property
        Public Property TotalAdultPrice() As Decimal
            Get
                Return _TotalAdultPrice
            End Get
            Set(ByVal value As Decimal)
                _TotalAdultPrice = value
            End Set
        End Property
        Public Property TotalAdult() As Integer
            Get
                Return _TotalAdult
            End Get
            Set(ByVal value As Integer)
                _TotalAdult = value
            End Set
        End Property
        Public Property TotalChild() As Integer
            Get
                Return _TotalChild
            End Get
            Set(ByVal value As Integer)
                _TotalChild = value
            End Set
        End Property
        Public Property TotalChildPrice() As Decimal
            Get
                Return _TotalChildPrice
            End Get
            Set(ByVal value As Decimal)
                _TotalChildPrice = value
            End Set
        End Property
        Public Property TotalPrice() As Decimal
            Get
                Return _TotalPrice
            End Get
            Set(ByVal value As Decimal)
                _TotalPrice = value
            End Set
        End Property
        Public Property MeetingDate() As Date
            Get
                Return _MeetingDate
            End Get
            Set(ByVal value As Date)
                _MeetingDate = value
            End Set
        End Property
        Public Property ServiceTypeCode() As String
            Get
                Return _ServiceTypeCode
            End Get
            Set(ByVal value As String)
                _ServiceTypeCode = value
            End Set
        End Property

        Public Property ServiceType() As String
            Get
                Return _ServiceType
            End Get
            Set(ByVal value As String)
                _ServiceType = value
            End Set
        End Property


        Public Property ReceivedType() As String
            Get
                Return _ReceievedType
            End Get
            Set(ByVal value As String)
                _ReceievedType = value
            End Set
        End Property



    End Class
    Public Class Excursion
        Private _AdultPrice As Decimal
        Private _ChildPrice As Decimal
        Private _SelectedExGroup As String = String.Empty
        Private _SelectedExType As String = String.Empty
        Private _TotalAdult As Integer = 0
        Private _TotalChild As Integer = 0
        Private _TotalPrice As Decimal
        Private _TourDate As Date
        Private _HotelLineNo As Integer = 0
        Private _OLineNo As Integer = 0

        Private _listOfGuestDetails As New List(Of GuestDetails) 'changed by mohamed on 14/05/2016

        Public Property ListOfGuestDetails() As List(Of GuestDetails) 'changed by mohamed on 14/05/2016
            Get
                Return _listOfGuestDetails
            End Get
            Set(ByVal value As List(Of GuestDetails))
                _listOfGuestDetails = value
            End Set
        End Property

        Public Property AdultPrice() As Decimal
            Get
                Return _AdultPrice
            End Get
            Set(ByVal value As Decimal)
                _AdultPrice = value
            End Set
        End Property

        Public Property ChildPrice() As Decimal
            Get
                Return _ChildPrice
            End Get
            Set(ByVal value As Decimal)
                _ChildPrice = value
            End Set
        End Property
        Public Property SelectedExGroup() As String
            Get
                Return _SelectedExGroup
            End Get
            Set(ByVal value As String)
                _SelectedExGroup = value
            End Set
        End Property
        Public Property SelectedExType() As String
            Get
                Return _SelectedExType
            End Get
            Set(ByVal value As String)
                _SelectedExType = value
            End Set
        End Property
        Public Property HotelLineNo() As Integer
            Get
                Return _HotelLineNo
            End Get
            Set(ByVal value As Integer)
                _HotelLineNo = value
            End Set
        End Property
        Public Property TotalAdult() As Integer
            Get
                Return _TotalAdult
            End Get
            Set(ByVal value As Integer)
                _TotalAdult = value
            End Set
        End Property
        Public Property TotalChild() As Integer
            Get
                Return _TotalChild
            End Get
            Set(ByVal value As Integer)
                _TotalChild = value
            End Set
        End Property
        Public Property TotalPrice() As Decimal
            Get
                Return _TotalPrice
            End Get
            Set(ByVal value As Decimal)
                _TotalPrice = value
            End Set
        End Property
        Public Property TourDate() As Date
            Get
                Return _TourDate
            End Get
            Set(ByVal value As Date)
                _TourDate = value
            End Set
        End Property

        Public Property OLineNo() As Integer
            Get
                Return _OLineNo
            End Get
            Set(ByVal value As Integer)
                _OLineNo = value
            End Set
        End Property


    End Class
    Public Class BookNow
        Private _hotellineno As Integer
        Private _PartyCode As String
        Private _RoomTypeCode As String
        Private _optionno As String
        Private _Rmcatcode As String
        Private _mealcode As String
        Private _dispmealcode As String 'changed by shahul on 29/03/2016

        Public Property hotellineno As Integer
            Get
                Return _hotellineno
            End Get
            Set(ByVal value As Integer)
                _hotellineno = value
            End Set

        End Property

        Public Property PartyCode() As String
            Get
                Return _PartyCode
            End Get
            Set(ByVal value As String)
                _PartyCode = value
            End Set
        End Property

        Public Property RoomTypeCode() As String
            Get
                Return _RoomTypeCode
            End Get
            Set(ByVal value As String)
                _RoomTypeCode = value
            End Set

        End Property
        Public Property optionno() As String
            Get
                Return _optionno
            End Get
            Set(ByVal value As String)
                _optionno = value
            End Set

        End Property
        Public Property Rmcatcode() As String
            Get
                Return _Rmcatcode
            End Get
            Set(ByVal value As String)
                _Rmcatcode = value
            End Set

        End Property
        Public Property mealcode() As String
            Get
                Return _mealcode
            End Get
            Set(ByVal value As String)
                _mealcode = value
            End Set

        End Property
        Public Property dispmealcode() As String 'changed by shahul on 29/03/2016
            Get
                Return _dispmealcode
            End Get
            Set(ByVal value As String)
                _dispmealcode = value
            End Set

        End Property

        ' price details is stored in the sub class
        Public Class PriceBreakup
            Private _hotellineno As Integer
            Private _PartyCode As String
            Private _RoomPrice As String
            Private _RoomPriceDisplay As String = String.Empty
            Private _fromDate As String = String.Empty
            Private _toDate As String = String.Empty
            Private _breakUpvalue As String = String.Empty
            Private _freeNights As String = String.Empty
            Private _nights As String = String.Empty
            Private _rmcatcode As String = String.Empty
            Private _NoOfRooms As Integer = 0

            Public Property hotellineno As Integer
                Get
                    Return _hotellineno
                End Get
                Set(ByVal value As Integer)
                    _hotellineno = value
                End Set

            End Property
            Public Property NoOfRooms As Integer
                Get
                    Return _NoOfRooms
                End Get
                Set(ByVal value As Integer)
                    _NoOfRooms = value
                End Set

            End Property

            Public Property rmcatcode() As String
                Get
                    Return _rmcatcode
                End Get
                Set(ByVal value As String)
                    _rmcatcode = value
                End Set
            End Property
            Public Property PartyCode() As String
                Get
                    Return _PartyCode
                End Get
                Set(ByVal value As String)
                    _PartyCode = value
                End Set
            End Property

            Public Property RoomPrice() As String
                Get
                    Return _RoomPrice
                End Get
                Set(ByVal value As String)
                    _RoomPrice = value
                End Set

            End Property

            Public Property RoomPriceDisplay() As String
                Get
                    Return _RoomPriceDisplay
                End Get
                Set(ByVal value As String)
                    _RoomPriceDisplay = value
                End Set
            End Property

            Public Property FromDate() As String
                Get
                    Return _fromDate
                End Get
                Set(ByVal value As String)
                    _fromDate = value
                End Set
            End Property
            Public Property ToDate() As String
                Get
                    Return _toDate
                End Get
                Set(ByVal value As String)
                    _toDate = value
                End Set
            End Property
            Public Property Value() As String
                Get
                    Return _breakUpvalue
                End Get
                Set(ByVal value As String)
                    _breakUpvalue = value
                End Set
            End Property

            Public Property FreeNights() As String
                Get
                    Return _freeNights
                End Get
                Set(ByVal value As String)
                    _freeNights = value
                End Set
            End Property

            Public Property Nights() As String
                Get
                    Return _nights
                End Get
                Set(ByVal value As String)
                    _nights = value
                End Set
            End Property
        End Class

        ' Travel info guest details sub class
        Public Class GuestInfo
            Private _hotellineno As Integer
            Private _RoomTypeCode As String
            Private _title As String
            Private _firstname As String
            Private _lastname As String
            Private _arrFlightNo As String
            Private _arrTime As String
            Private _arrAirport As String
            Private _depFlightNo As String
            Private _depTime As String
            Private _DepAirport As String
            Private _remarks As String
            Private _departureRemarks As String = String.Empty
            Private _rowkey As String
            Private _hotellineNostring As String




            Public Property hotellineno As Integer
                Get
                    Return _hotellineno
                End Get
                Set(ByVal value As Integer)
                    _hotellineno = value
                End Set

            End Property

            Public Property RoomTypeCode() As String
                Get
                    Return _RoomTypeCode
                End Get
                Set(ByVal value As String)
                    _RoomTypeCode = value
                End Set

            End Property

            Public Property Title() As String
                Get
                    Return _title
                End Get
                Set(ByVal value As String)
                    _title = value
                End Set

            End Property

            Public Property FirstName() As String
                Get
                    Return _firstname
                End Get
                Set(ByVal value As String)
                    _firstname = value
                End Set

            End Property

            Public Property LastName() As String
                Get
                    Return _lastname
                End Get
                Set(ByVal value As String)
                    _lastname = value
                End Set

            End Property

            Public Property ArrFlightNo() As String
                Get
                    Return _arrFlightNo
                End Get
                Set(ByVal value As String)
                    _arrFlightNo = value
                End Set

            End Property

            Public Property ArrTime() As String
                Get
                    Return _arrTime
                End Get
                Set(ByVal value As String)
                    _arrTime = value
                End Set

            End Property

            Public Property ArrAirport() As String
                Get
                    Return _arrAirport
                End Get
                Set(ByVal value As String)
                    _arrAirport = value
                End Set

            End Property

            Public Property DepFlightNo() As String
                Get
                    Return _depFlightNo
                End Get
                Set(ByVal value As String)
                    _depFlightNo = value
                End Set

            End Property

            Public Property DepTime() As String
                Get
                    Return _depTime
                End Get
                Set(ByVal value As String)
                    _depTime = value
                End Set

            End Property

            Public Property DepAirport() As String
                Get
                    Return _DepAirport
                End Get
                Set(ByVal value As String)
                    _DepAirport = value
                End Set

            End Property

            Public Property Remarks() As String
                Get
                    Return _remarks
                End Get
                Set(ByVal value As String)
                    _remarks = value
                End Set

            End Property

            Public Property DepartureRemarks() As String
                Get
                    Return _departureRemarks
                End Get
                Set(ByVal value As String)
                    _departureRemarks = value
                End Set
            End Property


            Public Property rowkey() As String
                Get
                    Return _rowkey
                End Get
                Set(ByVal value As String)
                    _rowkey = value
                End Set

            End Property

            Public Property HotelLineNoString() As String
                Get
                    Return _hotellineNostring
                End Get
                Set(ByVal value As String)
                    _hotellineNostring = value
                End Set
            End Property




            ' sub class for adults and child details
            Public Class AdultChildlts
                Private _hotellineno As Integer
                Private _title As String
                Private _firstname As String
                Private _lastname As String
                Private _age As Double
                Private _rowkey As String

                Public Property hotellineno As Integer
                    Get
                        Return _hotellineno
                    End Get
                    Set(ByVal value As Integer)
                        _hotellineno = value
                    End Set

                End Property

                Public Property Title() As String
                    Get
                        Return _title
                    End Get
                    Set(ByVal value As String)
                        _title = value
                    End Set

                End Property

                Public Property FirstName() As String
                    Get
                        Return _firstname
                    End Get
                    Set(ByVal value As String)
                        _firstname = value
                    End Set

                End Property

                Public Property LastName() As String
                    Get
                        Return _lastname
                    End Get
                    Set(ByVal value As String)
                        _lastname = value
                    End Set

                End Property

                Public Property Age() As Double
                    Get
                        Return _age
                    End Get
                    Set(ByVal value As Double)
                        _age = value
                    End Set

                End Property

                Public Property rowkey() As String
                    Get
                        Return _rowkey
                    End Get
                    Set(ByVal value As String)
                        _rowkey = value
                    End Set

                End Property
            End Class

        End Class

        Public Class AdultChild
            Private _hotellineno As Integer
            Private _rowIndex As Integer
            Private _PartyCode As String
            Private _RoomTypeCode As String
            Private _Roomcnt As Integer
            Private _Adults As Integer
            Private _Child As Integer
            Private _ChildAge1 As Double
            Private _ChildAge2 As Double
            Private _Title As String = String.Empty
            Private _firstName As String = String.Empty
            Private _lastName As String = String.Empty
            Private _Age As String = String.Empty
            Private _rowkey As String

            Public Property hotellineno As Integer
                Get
                    Return _hotellineno
                End Get
                Set(ByVal value As Integer)
                    _hotellineno = value
                End Set

            End Property

            Public Property RowIndex As Integer
                Get
                    Return _rowIndex
                End Get
                Set(ByVal value As Integer)
                    _rowIndex = value
                End Set

            End Property

            Public Property PartyCode() As String
                Get
                    Return _PartyCode
                End Get
                Set(ByVal value As String)
                    _PartyCode = value
                End Set
            End Property

            Public Property FirstName() As String
                Get
                    Return _firstName
                End Get
                Set(ByVal value As String)
                    _firstName = value
                End Set
            End Property

            Public Property LastName() As String
                Get
                    Return _lastName
                End Get
                Set(ByVal value As String)
                    _lastName = value
                End Set
            End Property

            Public Property Age() As String
                Get
                    Return _Age
                End Get
                Set(ByVal value As String)
                    _Age = value
                End Set
            End Property

            Public Property RoomTypeCode() As String
                Get
                    Return _RoomTypeCode
                End Get
                Set(ByVal value As String)
                    _RoomTypeCode = value
                End Set

            End Property

            Public Property NoRoom As Integer
                Get
                    Return _Roomcnt
                End Get
                Set(ByVal value As Integer)
                    _Roomcnt = value
                End Set

            End Property

            Public Property Adults As Integer
                Get
                    Return _Adults
                End Get
                Set(ByVal value As Integer)
                    _Adults = value
                End Set

            End Property

            Public Property Child As Integer
                Get
                    Return _Child
                End Get
                Set(ByVal value As Integer)
                    _Child = value
                End Set

            End Property

            Public Property ChildAge1 As Double
                Get
                    Return _ChildAge1
                End Get
                Set(ByVal value As Double)
                    _ChildAge1 = value
                End Set

            End Property

            Public Property ChildAge2 As Double
                Get
                    Return _ChildAge2
                End Get
                Set(ByVal value As Double)
                    _ChildAge2 = value
                End Set

            End Property

            Public Property rowkey() As String
                Get
                    Return _rowkey
                End Get
                Set(ByVal value As String)
                    _rowkey = value
                End Set

            End Property
        End Class

    End Class

    Public Class BookRoom
        Private _hotellineno As Integer
        Private _PartyCode As String
        Private _RoomTypeCode As String
        Private _RoomTypeDesc As String
        Private _optionno As String
        Private _Rmcatcode As String
        Private _mealcode As String
        Private _dispmealcode As String 'changed by shahul on 29/03/2016
        Private _hotellinenodet As Integer
        Private _optionstring As String
        Private _noofrooms As String
        Private _roomstring As String
        Private _listOfAdultAndChildDetails As New List(Of clsAgentsOnline.BookNow.AdultChild)
        Private _listOfRoomPrice As New clsAgentsOnline.RoomPrice
        Private _guestDetails As New clsAgentsOnline.BookNow.GuestInfo
        Private _promotionBreakUp As String
        Private _priceBreakUp As String
        Private _specialEventsApplied As String = String.Empty


        Private _SelectedPromotionStr As String 'Changed by mohamed on 10/05/2016
        Private _listOfGuestInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo) 'Changed by mohamed on 10/05/2016
        Private _RoomPrice As String 'Changed by mohamed on 10/05/2016

        Public Property Price() As String 'Changed by mohamed on 10/05/2016
            Get
                Return _RoomPrice
            End Get
            Set(ByVal value As String)
                _RoomPrice = value
            End Set
        End Property

        Public Property SelectedPromotionStr() As String 'Changed by mohamed on 10/05/2016
            Get
                Return _SelectedPromotionStr
            End Get
            Set(ByVal value As String)
                _SelectedPromotionStr = value
            End Set
        End Property

        Public Property ListOfGuestInfo() As List(Of clsAgentsOnline.BookNow.GuestInfo) 'Changed by mohamed on 10/05/2016
            Get
                Return _listOfGuestInfo
            End Get
            Set(ByVal value As List(Of clsAgentsOnline.BookNow.GuestInfo))
                _listOfGuestInfo = value
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

        Public Property PartyCode() As String
            Get
                Return _PartyCode
            End Get
            Set(ByVal value As String)
                _PartyCode = value
            End Set
        End Property

        Public Property RoomTypeCode() As String
            Get
                Return _RoomTypeCode
            End Get
            Set(ByVal value As String)
                _RoomTypeCode = value
            End Set

        End Property
        Public Property PromotionString() As String
            Get
                Return _promotionBreakUp
            End Get
            Set(ByVal value As String)
                _promotionBreakUp = value
            End Set
        End Property
        Public Property PriceBreakUp() As String
            Get
                Return _priceBreakUp
            End Get
            Set(ByVal value As String)
                _priceBreakUp = value
            End Set
        End Property

        Public Property SpecialEventsApplied() As String
            Get
                Return _specialEventsApplied
            End Get
            Set(ByVal value As String)
                _specialEventsApplied = value
            End Set
        End Property

        Public Property RoomTypeDesc() As String
            Get
                Return _RoomTypeDesc
            End Get
            Set(ByVal value As String)
                _RoomTypeDesc = value
            End Set

        End Property
        Public Property optionno() As String
            Get
                Return _optionno
            End Get
            Set(ByVal value As String)
                _optionno = value
            End Set

        End Property
        Public Property Rmcatcode() As String
            Get
                Return _Rmcatcode
            End Get
            Set(ByVal value As String)
                _Rmcatcode = value
            End Set

        End Property
        Public Property mealcode() As String
            Get
                Return _mealcode
            End Get
            Set(ByVal value As String)
                _mealcode = value
            End Set

        End Property
        Public Property dispmealcode() As String 'changed by shahul on 29/03/2016
            Get
                Return _dispmealcode
            End Get
            Set(ByVal value As String)
                _dispmealcode = value
            End Set

        End Property
        Public Property hotellinenodet As Integer
            Get
                Return _hotellinenodet
            End Get
            Set(ByVal value As Integer)
                _hotellinenodet = value
            End Set

        End Property
        Public Property optionstring() As String
            Get
                Return _optionstring
            End Get
            Set(ByVal value As String)
                _optionstring = value
            End Set

        End Property
        Public Property noofrooms As Integer
            Get
                Return _noofrooms
            End Get
            Set(ByVal value As Integer)
                _noofrooms = value
            End Set

        End Property
        Public Property roomstring() As String
            Get
                Return _roomstring
            End Get
            Set(ByVal value As String)
                _roomstring = value
            End Set
        End Property

        Public Property ListOfAdultAndChildDetails() As List(Of clsAgentsOnline.BookNow.AdultChild)
            Get
                Return _listOfAdultAndChildDetails
            End Get
            Set(ByVal value As List(Of clsAgentsOnline.BookNow.AdultChild))
                _listOfAdultAndChildDetails = value
            End Set
        End Property


        Public Property RoomPrice() As clsAgentsOnline.RoomPrice
            Get
                Return _listOfRoomPrice
            End Get
            Set(ByVal value As clsAgentsOnline.RoomPrice)
                _listOfRoomPrice = value
            End Set
        End Property


        'Public Property GuestDetails() As List(Of clsAgentsOnline.BookNow.GuestInfo)
        '    Get
        '        Return _guestDetails
        '    End Get
        '    Set(ByVal value As List(Of clsAgentsOnline.BookNow.GuestInfo))
        '        _guestDetails = value
        '    End Set
        'End Property

        Public Property GuestDetails() As clsAgentsOnline.BookNow.GuestInfo
            Get
                Return _guestDetails
            End Get
            Set(ByVal value As clsAgentsOnline.BookNow.GuestInfo)
                _guestDetails = value
            End Set
        End Property

    End Class
    Public Class RoomPrice
        Private _hotellineno As Integer
        Private _PartyCode As String
        Private _RoomTypeDesc As String
        Private _RoomPrice As String
        Private _NoRoom As Integer

        Public Property hotellineno As Integer
            Get
                Return _hotellineno
            End Get
            Set(ByVal value As Integer)
                _hotellineno = value
            End Set

        End Property

        Public Property PartyCode() As String
            Get
                Return _PartyCode
            End Get
            Set(ByVal value As String)
                _PartyCode = value
            End Set
        End Property

        Public Property RoomTypeDesc() As String
            Get
                Return _RoomTypeDesc
            End Get
            Set(ByVal value As String)
                _RoomTypeDesc = value
            End Set

        End Property
        Public Property Price() As String
            Get
                Return _RoomPrice
            End Get
            Set(ByVal value As String)
                _RoomPrice = value
            End Set

        End Property

        Public Property NoRoom() As Integer
            Get
                Return _NoRoom
            End Get
            Set(ByVal value As Integer)
                _NoRoom = value
            End Set

        End Property

    End Class

    Public Class GuestDetails
        Private _title As String
        Private _firstname As String
        Private _lastname As String
        Private _arrFlightNo As String
        Private _arrTime As String
        Private _arrAirport As String
        Private _depFlightNo As String
        Private _depTime As String
        Private _DepAirport As String
        Private _Age As String
        Private _guestLineNo As String
        Private _TransferType As Integer
        Private _adultAndChildInfo As List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)

        Public Property GuestLineNo() As String
            Get
                Return _guestLineNo
            End Get
            Set(ByVal value As String)
                _guestLineNo = value
            End Set

        End Property
        Public Property Age() As String
            Get
                Return _Age
            End Get
            Set(ByVal value As String)
                _Age = value
            End Set

        End Property


        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set

        End Property

        Public Property FirstName() As String
            Get
                Return _firstname
            End Get
            Set(ByVal value As String)
                _firstname = value
            End Set

        End Property

        Public Property LastName() As String
            Get
                Return _lastname
            End Get
            Set(ByVal value As String)
                _lastname = value
            End Set

        End Property

        Public Property ArrFlightNo() As String
            Get
                Return _arrFlightNo
            End Get
            Set(ByVal value As String)
                _arrFlightNo = value
            End Set

        End Property

        Public Property ArrTime() As String
            Get
                Return _arrTime
            End Get
            Set(ByVal value As String)
                _arrTime = value
            End Set

        End Property

        Public Property ArrAirport() As String
            Get
                Return _arrAirport
            End Get
            Set(ByVal value As String)
                _arrAirport = value
            End Set

        End Property

        Public Property DepFlightNo() As String
            Get
                Return _depFlightNo
            End Get
            Set(ByVal value As String)
                _depFlightNo = value
            End Set

        End Property

        Public Property DepTime() As String
            Get
                Return _depTime
            End Get
            Set(ByVal value As String)
                _depTime = value
            End Set

        End Property

        Public Property DepAirport() As String
            Get
                Return _DepAirport
            End Get
            Set(ByVal value As String)
                _DepAirport = value
            End Set

        End Property

        Public Property TransferType() As Integer
            Get
                Return _TransferType
            End Get
            Set(ByVal value As Integer)
                _TransferType = value
            End Set
        End Property

        Public Property ListChildDetails() As List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)
            Get
                Return _adultAndChildInfo
            End Get
            Set(ByVal value As List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts))
                _adultAndChildInfo = value
            End Set

        End Property



    End Class


    'Transfers details is stored in this main class
    Public Class TransfersMain
        Private _TransferLineNo As Integer
        Private _RowIndex As Integer
        Private _transferType As String
        Private _catCode As String
        Private _catName As String
        Private _Remarks As String
        Private _maxPax As Integer
        Private _price As Double
        Private _unit As Integer
        Private _tpListcode As String
        Private _saleCurrCd As String
        Private _wlprice As Double
        Private _pickupSec As String
        Private _dropupSec As String
        Private _arrivalDate As String = String.Empty
        Private _departureDate As String = String.Empty
        Private _listOfGuestDetails As New List(Of GuestDetails)


        Private _HotelLineNo As Integer 'changed by mohamed on 14/05/2016

        Public Property HotelLineNo() As Integer 'changed by mohamed on 14/05/2016
            Get
                Return _HotelLineNo
            End Get
            Set(ByVal value As Integer)
                _HotelLineNo = value
            End Set
        End Property

        Public Property ListOfGuestDetails() As List(Of GuestDetails)
            Get
                Return _listOfGuestDetails
            End Get
            Set(ByVal value As List(Of GuestDetails))
                _listOfGuestDetails = value
            End Set
        End Property

        Public Property TransferLineNo() As Integer
            Get
                Return _TransferLineNo
            End Get
            Set(ByVal value As Integer)
                _TransferLineNo = value
            End Set

        End Property

        Public Property RowIndex() As Integer
            Get
                Return _RowIndex
            End Get
            Set(ByVal value As Integer)
                _RowIndex = value
            End Set

        End Property

        Public Property TransferType() As String
            Get
                Return _transferType
            End Get
            Set(ByVal value As String)
                _transferType = value
            End Set

        End Property

        Public Property CatCode() As String
            Get
                Return _catCode
            End Get
            Set(ByVal value As String)
                _catCode = value
            End Set

        End Property

        Public Property TransferDate() As String
            Get
                Return _arrivalDate
            End Get
            Set(ByVal value As String)
                _arrivalDate = value
            End Set

        End Property
        Public Property CatName() As String
            Get
                Return _catName
            End Get
            Set(ByVal value As String)
                _catName = value
            End Set

        End Property

        Public Property Remarks() As String
            Get
                Return _Remarks
            End Get
            Set(ByVal value As String)
                _Remarks = value
            End Set

        End Property

        Public Property MaxPax() As Integer
            Get
                Return _maxPax
            End Get
            Set(ByVal value As Integer)
                _maxPax = value
            End Set

        End Property

        Public Property Price() As Double
            Get
                Return _price
            End Get
            Set(ByVal value As Double)
                _price = value
            End Set

        End Property

        Public Property Unit() As Integer
            Get
                Return _unit
            End Get
            Set(ByVal value As Integer)
                _unit = value
            End Set

        End Property


        Public Property TpListcode() As String
            Get
                Return _tpListcode
            End Get
            Set(ByVal value As String)
                _tpListcode = value
            End Set

        End Property

        Public Property SaleCurrCd() As String
            Get
                Return _saleCurrCd
            End Get
            Set(ByVal value As String)
                _saleCurrCd = value
            End Set

        End Property

        Public Property Wlprice() As Double
            Get
                Return _wlprice
            End Get
            Set(ByVal value As Double)
                _wlprice = value
            End Set

        End Property

        Public Property PickupSec() As String
            Get
                Return _pickupSec
            End Get
            Set(ByVal value As String)
                _pickupSec = value
            End Set

        End Property

        Public Property DropupSec() As String
            Get
                Return _dropupSec
            End Get
            Set(ByVal value As String)
                _dropupSec = value
            End Set

        End Property

    End Class

    'Other Service detail to show in guest page -changed by mohamed on 13/05/2016
    Public Class OtherServiceDetailToDisplay
        Private _Title As String
        Private _SubTitle As String
        Private _DateTime As String
        Private _FlightNo As String
        Private _Route As String
        Private _Adult As String
        Private _Child As String
        Private _VehicleType As String
        Private _EntryType As String
        Private _SubTotal As Double

        Public Property SubTotal() As Double
            Get
                Return _SubTotal
            End Get
            Set(ByVal value As Double)
                _SubTotal = value
            End Set
        End Property

        Public Property EntryType() As String
            Get
                Return _EntryType
            End Get
            Set(ByVal value As String)
                _EntryType = value
            End Set
        End Property

        Public Property VehicleType() As String
            Get
                Return _VehicleType
            End Get
            Set(ByVal value As String)
                _VehicleType = value
            End Set
        End Property

        Public Property Adult() As String
            Get
                Return _Adult
            End Get
            Set(ByVal value As String)
                _Adult = value
            End Set
        End Property

        Public Property Child() As String
            Get
                Return _Child
            End Get
            Set(ByVal value As String)
                _Child = value
            End Set
        End Property

        Public Property Route() As String
            Get
                Return _Route
            End Get
            Set(ByVal value As String)
                _Route = value
            End Set
        End Property

        Public Property FlightNo() As String
            Get
                Return _FlightNo
            End Get
            Set(ByVal value As String)
                _FlightNo = value
            End Set

        End Property

        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set

        End Property

        Public Property SubTitle() As String
            Get
                Return _SubTitle
            End Get
            Set(ByVal value As String)
                _SubTitle = value
            End Set

        End Property

        Public Property DateTime() As String
            Get
                Return _DateTime
            End Get
            Set(ByVal value As String)
                _DateTime = value
            End Set

        End Property
    End Class

End Class
