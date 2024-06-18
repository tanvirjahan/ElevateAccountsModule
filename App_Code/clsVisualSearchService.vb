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
    Public Class clsVisualSearchService
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
        ''' 
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayTicketNoVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  from Email_Inbox"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayHotelStatusVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select hotelstatusname from hotelstatus where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayTrackingStatusVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select TrackStatusName from TrackingStatusMaster"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayFromEmailIdVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct EmailFrom from Email_Inbox"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayEmailDateVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct convert(varchar(10), EmailDate,103)EmailDate from Email_Inbox order by EmailDate desc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayEmailSubjectVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct EmailSubject from Email_Inbox"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayUpdateTypeVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select distinct typename from trackupdatetype where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayAssinedToVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select USERNAME from usermaster where deptcode='CON' and active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayProgressStageVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "select 'Assigned' ProgressStage union select 'Not Assigned' union select 'For Publishing' union select 'Clarification Pending' union select 'Reassigned' union select 'Pending Approval' union select 'Approval Assigned'  union select 'Pending for Approval Clarification' union select 'Reassigned for Approval' union select 'Approval Completed' "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMealMarkupFormulaVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(formulaname)) from New_MarkupSupplement_Header where active=1 order by formulaname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMarkupFormulaVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(formulaname)) from markupformula_header where active=1 order by formulaname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfFormulaTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select 'Hotel' FormulaType union select 'Country' FormulaType"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfMinimumMarkupFormulaVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(formulaname)) from Minimum_Markup_Header where active=1 order by formulaname"
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
        Public Function GetListOfArrayRegionVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(plgrpname)) plgrpname  from plgrpmast where active=1"
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
        Public Function GetListOfDiscountFormulaVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(formulaname)) from Discount_Formula_Header where active=1 order by formulaname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfExcClassificationVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(classificationname)) from excclassification_header where active=1 order by classificationname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfExcursionVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(excursiongroupname)) from excursiongroup where active=1 order by excursiongroupname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfExcRateBasisVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = " select distinct ltrim(rtrim((select case when ratebasis='ACS' then 'ADULT/CHILD/SENIOR' else ratebasis  end ))) ratebasis from excursiontypes"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfExcursionclassiVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select ltrim(rtrim(classificationname)) from excclassification_header where active=1 order by classificationname"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfExcursionTypeVisualSearch(ByVal arSqlStr) As String()
            '  arSqlStr = "select othtypname from othtypmast inner join othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "
            arSqlStr = "select exctypname from excursiontypes where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfExcFilterTypeVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select othtypname from othtypmast inner join othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') and  othtypcode not in (select ecd.othtypcode from excclassification_detail ecd) "
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function


        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfGroupNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select groupname from groupmaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfAppNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select groupname from groupmaster,group_app_master  where groupmaster.groupid=group_app_master.groupid and  active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfRdeptNameVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select DeptName from DeptMaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfRdeptheadVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select username from usermaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

    End Class

End Namespace