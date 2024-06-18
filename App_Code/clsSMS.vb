Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Data.SqlClient
Public Class clsSMS

    Public Function sendSMS(ByVal tonumber As String, ByVal body As String) As Boolean
        Try

            Dim SqlConn As SqlConnection
            Dim myCommand As SqlCommand
            Dim result As Object = ""
            Dim strSqlQry As String = ""
            strSqlQry = "select option_selected from reservation_parameters where param_id='1012'"
            SqlConn = clsDBConnect.dbConnection               'Open connection
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            result = myCommand.ExecuteScalar()
            If result <> Nothing Then
                ''************
                ''uncomment while instaling , since no firewall so commented
                'If result = "Y" Then
                '    Dim req As HttpWebRequest = WebRequest.Create("http://205.210.190.32/smsonline/smppinterform.cfm?numbers=" & tonumber & ",&senderid=T3Group&accname=thuraya&accpass=thuraya123&msg=" & body)
                '    Dim resp As HttpWebResponse = req.GetResponse()
                '    Dim sr As New StreamReader(resp.GetResponseStream())
                '    Dim results As String = sr.ReadToEnd()
                '    sr.Close()
                '    sendSMS = True
                'End If
                ''************
            End If
        Catch ex As WebException
            sendSMS = False
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + ex.Message + "');", True)
        End Try
    End Function
End Class
