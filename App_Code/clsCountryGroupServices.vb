Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Text
Imports MyClasses
Imports System.Xml.Serialization
Imports System.IO
Namespace ColServices
    ' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    ' <System.Web.Script.Services.ScriptService()> _
    <ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class clsCountryGroupServices
        Inherits System.Web.Services.WebService

        Dim objUtils As New clsUtils
        Dim objDate As New clsDateTime
        Dim ClsGuestInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo)
        Dim ClsAdultChildInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)

        <WebMethod()> _
        Public Function HelloWorld() As String
            Return "Hello World"
        End Function
        ''' <summary>
        ''' GetListOfArrayVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayVisualSearch(ByVal arSqlStr) As String()
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' <summary>
        ''' GetListOfArrayCountryGroupVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCountryGroupVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(countrygroupname)) countrygroupname  from countrygroup where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayHolidayNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(holidayname)) holidayname  from holidaycalendar where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' <summary>
        ''' GetListOfArrayCountryVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCountryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(ctryname)) from ctrymast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' <summary>
        ''' GetListOfArrayRegionrVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayRegionrVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(plgrpname)) plgrpname  from plgrpmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
       
    End Class
End Namespace