Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient


Namespace ColServices
    ' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    <System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class ClsVisaServices
        Inherits System.Web.Services.WebService
        Dim objUtils As New clsUtils

        <WebMethod()> _
        Public Function HelloWorld() As String
            Return "Hello World"
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayRequestIDVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(RequestId)) as RequestId from (select distinct RequestId from Visa_guest(nolock) union all " &
            "select distinct b.RequestId from booking_guest(nolock) b left outer join Visa_guest(nolock) v on b.requestid=v.RequestId and b.guestlineno=v.GuestLineNo " &
            "where v.RequestId is null and v.GuestLineNo is null union all select RequestId from visabooking_header(nolock)) as t order by RequestId asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayFamilyNameVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(LastName)) as LastName from (select distinct LastName from Visa_guest union all " &
            "select distinct b.LastName from booking_guest b left outer join Visa_guest v on b.requestid=v.RequestId and b.guestlineno=v.GuestLineNo " &
            "where v.RequestId is null and v.GuestLineNo is null union all select distinct LastName from visabooking_guest) as t where LastName is not null and LastName <>'' order by LastName asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayFirstNameVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(FirstName)) as FirstName from (select distinct FirstName from Visa_guest union all " &
            "select distinct b.FirstName from booking_guest b left outer join Visa_guest v on b.requestid=v.RequestId and b.guestlineno=v.GuestLineNo " &
            "where v.RequestId is null and v.GuestLineNo is null union all select distinct FirstName from visabooking_guest) as t where FirstName is not null and FirstName <>'' order by FirstName asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayPassportNoVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct LTRIM(rtrim(passportNo)) as passportNo from (select PassportNo from Visa_guest union select PassportNo from visabooking_guest " &
            "union select PassportNo from booking_guest) as t where PassportNo <>'' order by PassportNo"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayAgentVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(agentname)) as AgentName from(select distinct a.agentname from booking_header h inner join agentmast a on h.agentcode= a.agentcode " &
            "union all select distinct a.agentname from visabooking_header h inner join agentmast a on h.agentcode= a.agentcode) as t order by AgentName asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraySponsorVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select ltrim(rtrim(PartyName)) as SponsorName from partymast where sptypecode='VISA' order by SponsorName asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayVisaOnlyRequestIDVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(RequestId)) as RequestId from visaBooking_header order by RequestId asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayVisaOnlyFamilyNameVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(LastName)) as LastName from visaBooking_guest order by LastName asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayVisaOnlyFirstNameVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(FirstName)) as FirstName from visaBooking_guest order by FirstName asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayVisaOnlyAgentVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(a.agentname)) as AgentName from Visabooking_header h inner join agentmast a on h.agentcode= a.agentcode order by AgentName asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayVisaOnlyAgentRefVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct ltrim(rtrim(AgentRef)) as AgentRef from visaBooking_header order by agentRef asc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
    End Class
End Namespace