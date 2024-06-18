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
    Public Class clsHotelGroupServices
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
      

        ''' <summary>
        ''' GetListOfArraySectorVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraySectorVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(sectorname)) from sectormaster where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' <summary>
        ''' GetListOfSectGrpNameVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSectorGrpsNameVisualSearch(ByVal arSqlStr) As String()
            'If Convert.ToString(HttpContext.Current.Session("sectgrps_citycode")) = "" Then
            arSqlStr = "select distinct ltrim(rtrim(othtypname)) from othtypmast where active=1  and othgrpcode in (select option_selected from reservation_parameters where param_id =1001)"
            'Else
            '    arSqlStr = "select distinct ltrim(rtrim(othtypname)) from othtypmast where active=1  and othgrpcode in (select option_selected from reservation_parameters where param_id =1001) and citycode='" & Convert.ToString(HttpContext.Current.Session("sectgrps_citycode")) & "'"
            'End If

            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAlternativeSectorGrpsNameVisualSearch(ByVal arSqlStr) As String()
            'If Convert.ToString(HttpContext.Current.Session("sectgrps_citycode")) = "" Then
            arSqlStr = "select distinct ltrim(rtrim(sectorgroupname)) from AlternativeBookingSectorGroup(nolock)"
            'Else
            '    arSqlStr = "select distinct ltrim(rtrim(othtypname)) from othtypmast where active=1  and othgrpcode in (select option_selected from reservation_parameters where param_id =1001) and citycode='" & Convert.ToString(HttpContext.Current.Session("sectgrps_citycode")) & "'"
            'End If

            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function



        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(catname)) from catmast where active=1  and sptypecode not in (select option_selected from reservation_parameters where param_id =1033)"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraySuppCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(catname)) from catmast where active=1  and  sptypecode='EXC'"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayInventorytypesVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(item1)) from  dbo.SplitString1colsWithOrderField ('General, B2B, Financial, Free Sale, Dummy Booking',',')"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayHotelGroupVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(hotelgroupname)) from hotelgroup where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraysuppGroupVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(suppliergrpname)) from suppliergroups where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayASectorVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(sectorname)) from agent_sectormaster where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCountryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(ctryname)) from ctrymast where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayACategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(agentcatname)) from agentcatmast where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCustomerGroupVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(customergroupname)) from customergroup where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCountryGroupVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(countrygroupname)) from countrygroup where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCustomerVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(agentname)) from agentmast where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraySectorsVisualSearch(ByVal arSqlStr) As String()
            If Convert.ToString(HttpContext.Current.Session("hdOPMode")) = "S" Then
                arSqlStr = "select distinct ltrim(rtrim(sectorname)) from sectormaster where active=1 "
            Else
                arSqlStr = "select distinct ltrim(rtrim(sectorname)) from sectormaster where active=1 and citycode='" & Convert.ToString(HttpContext.Current.Session("sectgrps_citycode")) & "'"
                ' and isnull(sectorgroupcode,'')=''
            End If
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayAltSectorsVisualSearch(ByVal arSqlStr) As String()
            If Convert.ToString(HttpContext.Current.Session("hdOPMode")) = "S" Then
                arSqlStr = "select distinct ltrim(rtrim(sectorname)) from sectormaster where active=1 "
            Else
                arSqlStr = "select distinct ltrim(rtrim(sectorname)) from sectormaster where active=1 "
                ' and isnull(sectorgroupcode,'')=''
            End If
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
   
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCityVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(cityname)) from citymast where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayHotelsVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(partyname))partyname from partymast where partymast.sptypecode='HOT'"

            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayHotelChainVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(hotelchainname))  from hotelchainmaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function



        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayHotelNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(partyname))  from partymast where active=1 and partymast.sptypecode='HOT' and hotelchaincode is NULL"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayHotelStatusVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(hotelstatusname))  from hotelstatus where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayFormulaIDVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(formulaid)) from markup_hotels "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayRoomClassificationVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(roomclassname)) from room_classification where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
    End Class
End Namespace