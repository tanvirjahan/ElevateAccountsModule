Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports System.IO


Partial Class AgentsOnline_FooterControl
    Inherits System.Web.UI.UserControl

    Dim objUtils As New clsUtils
    Dim objListOfValidLinks As New List(Of String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("GlobalAgentUserName") Is Nothing Then
                Response.Redirect("login.aspx")
            End If
            'CheckForValidTopLinks()
            'Dim bookedHotel As clsAgentsOnline.BookForHotel
            'bookedHotel = Session("BookForHotel")
            'objListOfValidLinks = ListOfValidLinks()

            'CheckForValidTopLinks()
        End If
    End Sub
    'Private Function ListOfValidLinks() As List(Of String)
    '    Dim validPages As New List(Of String)
    '    Dim strQuery As String = String.Empty
    '    strQuery = "select * from AgentMenuRights where userid='" & Session("GlobalAgentUserName") & "' and IsActive=1"
    '    Dim dsdata As DataSet
    '    dsdata = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), strQuery)
    '    If Not dsdata Is Nothing Then
    '        If dsdata.Tables.Count > 0 Then
    '            If dsdata.Tables(0).Rows.Count > 0 Then
    '                Dim strRights = dsdata.Tables(0).Rows(0)("privilege").ToString()
    '                Dim strMenuId As New List(Of String)(strRights.Split(";"))
    '                For i = 0 To strMenuId.Count - 1
    '                    strQuery = "select * from AgentMenu where Id='" & strMenuId(i) & "' and IsActive=1"
    '                    Dim dsdataPages As DataSet
    '                    dsdataPages = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), strQuery)
    '                    If Not dsdataPages Is Nothing Then
    '                        If dsdataPages.Tables(0).Rows.Count > 0 Then
    '                            validPages.Add(dsdataPages.Tables(0).Rows(0)("NavigationUrl").ToString())
    '                        End If
    '                    End If
    '                Next
    '            End If
    '        End If
    '    End If
    '    Return validPages
    'End Function
    'Private Sub CheckForValidTopLinks()
    '    If objListOfValidLinks.Contains("MyAccount.aspx") Then
    '        linkMyAccount.Visible = True
    '    End If
    '    If objListOfValidLinks.Contains("ContactUs.aspx") Then
    '        linkContactUs.Visible = True
    '    End If
    '    If objListOfValidLinks.Contains("RateDownload.aspx") Then
    '        linkRateDownload.Visible = True
    '    End If
    'End Sub
End Class
