Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports MyClasses
Imports System.Xml.Serialization
Imports System.IO

Namespace ColServices
    ' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    <System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
     Public Class clsInventoryServices
        Inherits System.Web.Services.WebService
        Dim objUtils As New clsUtils
        <WebMethod()> _
        Public Function HelloWorld() As String
            Return "Hello World"
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayInventoryIDVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "Select LTRIM(RTRIM(InventoryID)) from inventory_header where InventoryType in ('B2B','Financial','General') order by InventoryID desc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayInventoryTypeVisualSearch(ByVal arSqlStr As String) As String()
            Dim retlist As New List(Of String)
            retlist.Add("B2B")
            retlist.Add("Financial")
            retlist.Add("General")
            Return retlist.ToArray()
        End Function

        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayApprovedStatusVisualSearch(ByVal arSqlStr As String) As String()
            Dim retlist As New List(Of String)
            retlist.Add("0")
            retlist.Add("1")
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayFreeSaleInventoryIDVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "Select LTRIM(RTRIM(InventoryID)) from inventory_header where InventoryType in ('Free Sale') order by InventoryID desc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayFreeSaleInventoryTypeVisualSearch(ByVal arSqlStr As String) As String()
            Dim retlist As New List(Of String)
            retlist.Add("Free Sale")
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayStopSaleInventoryIDVisualSearch(ByVal arSqlStr As String) As String()
            arSqlStr = "Select LTRIM(RTRIM(InventoryID)) from inventory_header where InventoryType in ('Stop Sale') order by InventoryID desc"
            Dim retlist As New List(Of String)
            Dim lsconStr As String = Session("dbconnectionName")
            retlist = objUtils.FillStringArray(lsconStr, arSqlStr)
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayStopSaleInventoryTypeVisualSearch(ByVal arSqlStr As String) As String()
            Dim retlist As New List(Of String)
            retlist.Add("Stop Sale")
            Return retlist.ToArray()
        End Function
        <WebMethod(EnableSession:=True)> _
        Public Function GetListOfArrayWithdrawTypeVisualSearch(ByVal arSqlStr As String) As String()
            Dim retlist As New List(Of String)
            retlist.Add("New")
            retlist.Add("Full")
            retlist.Add("Partial")
            Return retlist.ToArray()
        End Function
    End Class
End Namespace