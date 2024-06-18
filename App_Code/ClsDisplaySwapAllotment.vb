Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic



Public Class ClsDisplaySwapAllotment
    Dim objUtils As New clsUtils
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader


    Dim mRequestID As String
    Dim mFromdate As String
    Dim mTodate As String
    Dim mRequestdate As String
    Dim mSupagentcode As String
    Dim mPartycode As String
    Dim mRmtypcode As String
    Dim mPlgrpcode As String
    Dim mAgentcode As String
    Dim mRlineno As String
    Dim mNorooms As String
    Dim mBasketid As String
    Dim dbconnectionName As String

    Public Property DBConnector As String
        Get
            Return dbconnectionName
        End Get
        Set(ByVal value As String)
            dbconnectionName = value
        End Set
    End Property

    Public Property RequestID As String
        Get
            Return mRequestID
        End Get
        Set(ByVal value As String)
            mRequestID = value
        End Set
    End Property

    Public Property Fromdate As String
        Get
            Return mFromdate
        End Get
        Set(ByVal value As String)
            mFromdate = value
        End Set
    End Property

    Public Property Todate As String
        Get
            Return mTodate
        End Get
        Set(ByVal value As String)
            mTodate = value
        End Set
    End Property

    Public Property Requestdate As String
        Get
            Return mRequestdate
        End Get
        Set(ByVal value As String)
            mRequestdate = value
        End Set
    End Property

    Public Property Supagentcode As String
        Get
            Return mSupagentcode
        End Get
        Set(ByVal value As String)
            mSupagentcode = value
        End Set
    End Property

    Public Property Partycode As String
        Get
            Return mPartycode
        End Get
        Set(ByVal value As String)
            mPartycode = value
        End Set
    End Property

    Public Property Rmtypcode As String
        Get
            Return mRmtypcode
        End Get
        Set(ByVal value As String)
            mRmtypcode = value
        End Set
    End Property

    Public Property Plgrpcode As String
        Get
            Return mPlgrpcode
        End Get
        Set(ByVal value As String)
            mPlgrpcode = value
        End Set
    End Property

    Public Property Agentcode As String
        Get
            Return mAgentcode
        End Get
        Set(ByVal value As String)
            mAgentcode = value
        End Set
    End Property

    Public Property Rlineno As Integer
        Get
            Return mRlineno
        End Get
        Set(ByVal value As Integer)
            mRlineno = value
        End Set
    End Property

    Public Property Norooms As String
        Get
            Return mNorooms
        End Get
        Set(ByVal value As String)
            mNorooms = value
        End Set
    End Property

    Public Property Basketid As Integer
        Get
            Return mBasketid
        End Get
        Set(ByVal value As Integer)
            mBasketid = value
        End Set
    End Property


    Public Sub New()
    End Sub

    Public Function fn_DisplaySwapAllotments() As DataSet
        Dim ds As New DataSet
        Dim i As Integer
        Dim parms As New List(Of SqlParameter)
        Dim parm(15) As SqlParameter
        Dim Check As New CheckBox
        Dim lblAlloted As New Label        

        Dim strQuery As String = ""


        parm(0) = New SqlParameter("@fromdate", Fromdate)
        parm(1) = New SqlParameter("@todate", Todate)
        'parm(2) = New SqlParameter("@requestdate", CType(Request.QueryString("todate"), String))
        parm(2) = New SqlParameter("@requestdate", Requestdate)
        parm(3) = New SqlParameter("@supagentcode", Supagentcode)
        parm(4) = New SqlParameter("@partycode", Partycode)
        parm(5) = New SqlParameter("@rmtypcode", Rmtypcode)
        parm(6) = New SqlParameter("@plgrpcode", Plgrpcode)
        parm(7) = New SqlParameter("@agentcode", Agentcode)
        parm(8) = New SqlParameter("@requestid", RequestID)
        parm(9) = New SqlParameter("@rlineno", Rlineno)
        parm(10) = New SqlParameter("@norooms", Norooms)
        parm(11) = New SqlParameter("@basketid", Basketid)
        parm(12) = New SqlParameter("@swaprooms", 1)
        parm(13) = New SqlParameter("@swapsub", 0)
        parm(14) = New SqlParameter("@type", IIf(Left(RequestID, 3) = "WRQ", 1, 0))
        parm(15) = New SqlParameter("@stopweb", 0)

        'End of parameter list

        For i = 0 To 15
            parms.Add(parm(i))
        Next
        ds = objUtils.ExecuteQuerynew(dbconnectionName, "sp_getallotments_swap", parms)
        Return ds
    End Function


End Class
