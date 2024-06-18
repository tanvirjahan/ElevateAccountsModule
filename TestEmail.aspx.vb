Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Net.Mail
Imports System.Web.HttpServerUtility

Partial Class Reservation_TestEmail
    Inherits System.Web.UI.Page
    Public Function SendEmailCC(ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String) As Boolean
        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient
        Dim strarr() As String
        Dim strarrCC() As String

        Dim i, j As Integer
        Try
            strarr = strTo.Split(",")
            strarrCC = strToCC.Split(",")


            'Set From Email id
            Mail_Message.From = FromAddress
            'Set To Email id
            j = 0
            For i = 1 To strarr.Length
                Mail_Message.To.Add(strarr(j))
                j = j + 1
            Next

            If strToCC <> "" Then
                j = 0
                For i = 1 To strarrCC.Length
                    Mail_Message.CC.Add(strarrCC(j))
                    j = j + 1
                Next
            End If

            'Set Subject
            'Dim attachFile As New Attachment(txtAttachmentPath.Text)
            'MyMessage.Attachments.Add(attachFile)

            Mail_Message.Subject = strSubject
            'Set Msg Body
            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True
            'msClient.Port = 25
            'msClient.Host = "127.0.0.1"

            msClient.Port = 465
            msClient.Host = "p3plcpnl0180.prod.phx3.secureserver.net"
            msClient.EnableSsl = True
            msClient.Credentials = New System.Net.NetworkCredential("", "")

            'comment by csn since no firewall need to uncomment on instalation
            msClient.Send(Mail_Message)
            SendEmailCC = True
        Catch ex As Exception
            SendEmailCC = False
        End Try
    End Function

   
End Class
