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
    Public Class clsAgentServices
        Inherits System.Web.Services.WebService
        Dim objUtils As New clsUtils

        <WebMethod()> _
        Public Function HelloWorld() As String
            Return "Hello World"
        End Function
        ''' <summary>
        ''' GetListOfArrayAgentSectorVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayAgentSectorVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select rtrim(ltrim(sectorName)) from agent_sectormaster where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' <summary>
        ''' GetListOfArrayAgentCategoryVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayAgentCategoryVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select rtrim(ltrim(agentCatName)) from agentCatMast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        ''' <summary>
        ''' GetListOfArrayAgentsVisualSearch
        ''' </summary>
        ''' <param name="arSqlStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayAgentsVisualSearch(ByVal arSqlStr) As String()
            arSqlStr = "select rtrim(ltrim(agentName)) from agentmast where active=1"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
    End Class

End Namespace