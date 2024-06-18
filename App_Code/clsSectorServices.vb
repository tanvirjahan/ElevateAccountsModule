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
        Public Class clsSectorServices
        Inherits System.Web.Services.WebService


        Dim objUtils As New clsUtils
        Dim objDate As New clsDateTime
        Dim ClsGuestInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo)
        Dim ClsAdultChildInfo As New List(Of clsAgentsOnline.BookNow.GuestInfo.AdultChildlts)
        ''' <summary>
        ''' HelloWorld
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''' GetListOfArrayCityVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks> 
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayCityVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(cityname)) cityname  from citymast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' Tanvir 19072022
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayMarketVisualSearch(ByVal arSqlStr) As String()

            arSqlStr = "select ltrim(rtrim(Marketname)) Marketname  from MarketMast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayPropertyTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(propertytypename)) propertytypename  from hotel_propertytype where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDEALIDsVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(DealID)) DealID  from TblPopularDeal  "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfOfferIDsVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(OfferID)) DealID  from TblHottestOffers "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraygroupsTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select  ltrim(rtrim(groupname))  from groupmaster where active=1 order by groupid "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraydeptTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "  select  ltrim(rtrim(DeptName))  from DeptMaster  where active=1 order by Deptcode"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayApplicationsTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = " select ltrim(rtrim(appname))   from appmaster where appstatus=1 order by appid"
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
        ''' GetListOfArraySectorVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraySectorVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(sectorname)) from sectormaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' <summary>
        ''' GetListOfArraySectorGroupVisualSearch
        ''' </summary>
        ''' <param name="vDefault_Group"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArraySectorGroupVisualSearch(ByVal vDefault_Group) As String()
            Dim arSqlStr = "select distinct ltrim(rtrim(othtypname)) from othtypmast where active=1 and othgrpcode =" + vDefault_Group
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfCurrencyVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(currname)) from currmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfacctnamesVisualSearch(ByVal arSqlStr) As String()

            arSqlStr = "select other_bank_master_des from customer_bank_master where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfacctnameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(acctname))  from acctmast  where bankyn='N'  "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAgentCatNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(agentcatname)) from agentcatmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSectGrpNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(othtypname)) from othtypmast where active=1  and othgrpcode in (select option_selected from reservation_parameters where param_id =1001)"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfRegionVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(plgrpname)) from plgrpmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfBankdetailsVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(bank_master_type_des)) from bank_master_type "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfopBalTranidVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = " select distinct ltrim(rtrim(tran_id)) from openparty_master "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfOpeSupplierVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(partyname)) from partymast where active=1  "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfWebAdSupplierVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(partyname)) from partymast where active=1 and sptypecode=(select option_selected from reservation_parameters  where param_id=458) and showinweb=1   "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfBankNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(bankname)) from  BANKDETAILS_MASTER where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfCustBankVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(other_bank_master_des)) from customer_bank_master where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfNarrationVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(narration)) from narration where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfCostGroupVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(costcentergrp_name)) from costcentergroup_master where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfCostCenterVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(costcenter_name)) from costcenter_master where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSectorsVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(sectorname)) from agent_sectormaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfCountryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(ctryname)) from ctrymast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfCityVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(cityname)) from citymast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        '*** DANNY 27/02/2018
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAmenityNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(AmenityName)) from TB_HotelAmenitiesMaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        '*** DANNY 27/02/2018
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAmenityTypeNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(AmenityTypeCode)) from TB_AmenityTypeMaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSupplierVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(sptypename)) from sptypemast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfHotelTempNamesVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim( remarksdesc)) from hotelremarkstemplate  "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSupplierHotCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(catname)) from catmast where active=1  and sptypecode in (select option_selected from reservation_parameters where param_id in (460,458))"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfOthSupplierVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(partyname)) from partymast where active=1  and sptypecode = (select option_selected from reservation_parameters where param_id =1501)"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfWebSupplierCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(catname)) from catmast where active=1 "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSupplierCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(catname)) from catmast where active=1  and sptypecode in (select option_selected from reservation_parameters where param_id=1033)"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfVisaSupplierCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(catname)) from catmast where active=1  and sptypecode in (select option_selected from reservation_parameters where param_id=1032)"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfTrfSupplierCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(catname)) from catmast where active=1  and sptypecode in (select option_selected from reservation_parameters where param_id=564)"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSupplierAgentVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(supagentname)) from supplier_agents where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfRoomClassificationVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(roomclassname)) from room_classification where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()

        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfRoomCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "SELECT distinct case accom_extra  when 'A' then 'Adult Accommodation'when 'C' then 'Child Accommodation'  end   FROM rmcatmast WHERE ACCOM_EXTRA IN ('A','C')"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()

        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMealVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(mealname)) from mealmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfSupplementCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "SELECT distinct case accom_extra  when 'M' then 'Adult Meal Supplements'when 'L' then 'Child Meal Supplements' when 'E' then 'Extra' end   FROM rmcatmast WHERE ACCOM_EXTRA IN ('M','L','E')"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()

        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfRoomAccCategoryNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(rmcatname)) from rmcatmast where active=1 and accom_extra in('A','C')"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfRoomSupCategoryNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(rmcatname)) from rmcatmast where active=1 and accom_extra in('M','L','E')"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMealPlanVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(mealyn)) from rmcatmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfHotelChainNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(hotelchainname)) from hotelchainmaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfExhibitionVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(exhibitionname)) from exhibition_master where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfCommissionVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(formulaname)) from commissionformula_header where active=1 order by formulaname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfHotelStatusVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(hotelstatusname)) from hotelstatus where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAirportBorderNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(airportbordername)) from airportbordersmaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDriverNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(drivername)) from drivermaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfVehicleNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(othcatname)) from othcatmast where active=1 and othgrpcode='TRFS'"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAirportRepVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(repname)) from  Airportrep where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfTypeNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(othtypname)) from  othtypmast where othgrpcode=(select option_selected from reservation_parameters where param_id= 1028 ) "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfFlighTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct case when type=1 then 'ARRIVAL' else 'DEPARTURE' end status from  flightmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfFlightNumberVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(flightcode)) from  flightmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAirportVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct a.airportbordername from airportbordersmaster a join flightmast f on a.airportbordercode=f.airportbordercode"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAirportNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct a.airportbordername from airportbordersmaster a join othtypmast_airportborders f on a.airportbordercode=f.airportbordercode"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfVisaTypeNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(othtypname)) from  othtypmast where active=1 and othgrpcode='VISA'"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfOthgrpsNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(othgrpname))othgrpname from othgrpmast where active=1 and othgrpcode not in ( select * from view_system_othgrp) order by othgrpname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfOthgrpTypeNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(othtypname))othtypname from  othtypmast where active=1 and othgrpcode not in (select * from view_system_othgrp) order by othtypname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDocNoVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(tran_id)) from  matchos_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDocTypeNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(tran_type)) from  matchos_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfStatusVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(post_state)) from  matchos_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDocNAmeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(post_state)) from  matchos_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfJournalDocNoVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(tran_id)) from  journal_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfJDescriptionVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(journal_narration)) from  journal_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfusersVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(username)) from  usermaster"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfJStatusVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state from  journal_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMDocNoVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(tran_id)) from  matchos_master where div_code='01'"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMDocTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(tran_type)) from  matchos_master  "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMStatusVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state from  matchos_master"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDDocTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select distinct ltrim(rtrim(type)) from view_account"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDDocNoVisualSearch(ByVal arSqlStr) As String()
            Convert.ToString(HttpContext.Current.Session("CNDNOpen_type").ToString())

            arSqlStr = "select distinct ltrim(rtrim(tran_id)) from trdpurchase_master where div_id='01' and tran_type='" & Session("CNDNOpen_type") & "'"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDSupplierVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select DES from view_account WHERE TYPE='S' and div_code='01' order by des"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDCusVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = " select des FROM view_account WHERE TYPE='C' and div_code='01' order by des"

            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfDagentVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select DES from view_account WHERE TYPE='A' and div_code='01' order by des"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMSupplierVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select DES from view_account WHERE TYPE='S' and div_code='01' order by des"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMCusVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = " select des FROM view_account WHERE TYPE='C' and div_code='01' order by des"

            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMagentVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select DES from view_account WHERE TYPE='A' and div_code='01' order by des"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfTranDocVisualSearchStrNew(ByVal arSqlStr As String) As String()


            arSqlStr = "select distinct ltrim(rtrim(tran_id)) tranid,ltrim(rtrim(tran_id)) tranid1  from receipt_master_new (nolock)c where tran_type='" & Session("ReceiptsSearchRVPVTranType") & "'"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfTranDocVisualSearch(ByVal asCtryCode As String) As clsMaster()
            Dim retlist As New List(Of clsMaster)
            Dim sqlstr As String
            Convert.ToString(HttpContext.Current.Session("ReceiptsSearchRVPVTranType").ToString())
            sqlstr = "select distinct ltrim(rtrim(tran_id)) tranid,ltrim(rtrim(tran_id)) tranid1  from receipt_master_new (nolock)c where tran_type='" & Session("ReceiptsSearchRVPVTranType") & "'"
            retlist = objUtils.FillclsMasternew(Session("dbConnectionName"), sqlstr, True)
            Return retlist.ToArray()
        End Function
        '<WebMethod(EnableSession:=True)> _
        'Public Function GetListOfTranDocVisualSearch(ByVal arSqlStr) As String()
        '  arSqlStr ="select ltrim(rtrim(tran_id)) tranid  from receipt_master_new (nolock) where tran_type='" + trantype.value + "' and receipt_div_id='" + divcode.value + "' order by tranid   ";
        '    Dim retlist As New List(Of String)
        '    Dim lsconStr As String = Session("dbconnectionName")
        '    retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
        '    Return retlist.ToArray()
        'End Function
    End Class


End Namespace