Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Net.Mail
Imports System.Web.HttpServerUtility

Public Class clsEmail
    Dim strQry As String
    Dim strWhereCond As String
    Dim strInnerWhereCond As String
    Dim strMailMsg As String
    Dim strSubject As String
    Dim flgSendMail As Boolean

    Public Function SendEmail(ByVal strFrom As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMsg As String) As Boolean
        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient
        Try
            'Set From Email id
            Mail_Message.From = FromAddress
            'Set To Email id
            Mail_Message.To.Add(strTo)
            Mail_Message.Subject = strSubject

            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True

            msClient.Port = 25
            msClient.Host = "127.0.0.1"
            msClient.Credentials = New System.Net.NetworkCredential("", "") '





            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)
            SendEmail = True
        Catch ex As Exception
            SendEmail = False
        End Try
    End Function

    Public Function SendEmail(ByVal strFrom As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMsg As String, ByVal attchemnt As String) As Boolean
        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient
        Try
            'Set From Email id
            Mail_Message.From = FromAddress
            'Set To Email id
            Mail_Message.To.Add(strTo)
            'Set Subject
            'Dim attachFile As New Attachment(txtAttachmentPath.Text)
            'MyMessage.Attachments.Add(attachFile)

            Mail_Message.Subject = strSubject
            'Set Msg Body
            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True
            Mail_Message.Attachments.Add(New Attachment(attchemnt))
            msClient.Port = 25
            msClient.Host = "127.0.0.1"


            msClient.Credentials = New System.Net.NetworkCredential("", "") '
            'comment by csn since no firewall need to uncomment on instalation

            'msClient.Send(Mail_Message)
            Mail_Message.Dispose()
            SendEmail = True
        Catch ex As Exception
            Mail_Message.Dispose()
            SendEmail = False
        End Try
    End Function
    Public Function SendEmailCCust(ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String, ByVal attchemnt As String) As Boolean
        Try
            ' Create new mail message
            Dim MyMail As MailMessage = New MailMessage()
            ' Set recipient email, you can use Add() method to add 
            ' more than one recipient
            MyMail.To.Add(strTo)
            ' Set sender email address
            MyMail.From = New MailAddress(strFrom)
            ' Set subject of an email
            MyMail.To.Add(strToCC)


            MyMail.Subject = strSubject
            ' Create mail body string. Body includes an image tag
            MyMail.Body = strMsg

            ' Set mail body as HTML
            MyMail.IsBodyHtml = True

            ' Create file attachment
            If attchemnt <> "" Then
                Dim ImageAttachment As Attachment = New Attachment(attchemnt)
                'ImageAttachment.ContentId = "Logo.png"

                ' Add an image as file attachment
                MyMail.Attachments.Add(ImageAttachment)
                ' Set the ContentId of the attachment, used in body HTML
            Else

                Dim ImageAttachment As Attachment = New Attachment(attchemnt)

                ImageAttachment.ContentId = "Logo.png"

                ' Add an image as file attachment
                MyMail.Attachments.Add(ImageAttachment)
            End If


            ' Create instance of Smtp client
            Dim MySmtpClient As SmtpClient = New SmtpClient("email.hostcentric.com")
            ' If your ISP required it, set user name and password
            MySmtpClient.Credentials = New System.Net.NetworkCredential("updatejobs@mahce.com", "updatejobs")
            ' Finally, send created email
            MySmtpClient.Send(MyMail)


            SendEmailCCust = True
        Catch ex As Exception
            SendEmailCCust = False
        End Try
    End Function

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
            msClient.Port = 25



            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)

            'Local Testing Start
            msClient.Port = 25
            msClient.Host = "127.0.0.1"
            msClient.Credentials = New System.Net.NetworkCredential("", "") '
            msClient.Send(Mail_Message)
            'Local Testing End

            SendEmailCC = True
        Catch ex As Exception
            SendEmailCC = False
        End Try
    End Function
    Public Function SendEmailCC(ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String, ByVal attchemnt As String) As Boolean
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


            Mail_Message.Subject = strSubject
            'Set Msg Body
            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True
            Mail_Message.Attachments.Add(New Attachment(attchemnt))


            msClient.Port = 25
            msClient.Host = "127.0.0.1"
            msClient.Credentials = New System.Net.NetworkCredential("", "") '

            'msClient.Port = 25

            'msClient.Host = "127.0.0.1"

            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)
            SendEmailCC = True
        Catch ex As Exception
            SendEmailCC = False
        End Try
    End Function

    Public Function SendEmailCCMultiAttachmnt(ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String, ByVal attchemnt As String) As Boolean
        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient
        Dim strarr() As String
        Dim strarrCC() As String
        Dim strarrAttach() As String

        Dim i, j As Integer
        Try
            strarr = strTo.Split(",")
            strarrCC = strToCC.Split(",")

            strarrAttach = attchemnt.Split(";")
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


            Mail_Message.Subject = strSubject
            'Set Msg Body
            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True
            For i = 0 To strarrAttach.Length - 1
                Mail_Message.Attachments.Add(New Attachment(strarrAttach(i)))
            Next



            msClient.Port = 25
            msClient.Host = "127.0.0.1"
            msClient.Credentials = New System.Net.NetworkCredential("", "") '

            'msClient.Port = 25

            'msClient.Host = "127.0.0.1"

            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)
            SendEmailCCMultiAttachmnt = True
        Catch ex As Exception
            SendEmailCCMultiAttachmnt = False
        End Try
    End Function

    Public Function ReSendEmail(ByVal strFrom As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMsg As String, ByVal ctr As Integer) As Boolean
        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient
        Try


            'Set From Email id
            Mail_Message.From = FromAddress
            'Set To Email id
            Mail_Message.To.Add(strTo)



            Mail_Message.Subject = strSubject
            'Set Msg Body
            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True

            msClient.Port = 25
            msClient.Host = "127.0.0.1"
            msClient.Credentials = New System.Net.NetworkCredential("", "") '

            'msClient.Port = 25

            'msClient.Host = "127.0.0.1"

            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)
            ReSendEmail = True
            ctr = 1

        Catch ex As Exception
            ReSendEmail = False

            'If ctr <= 5 Then
            '    ctr = ctr + 1
            '    ReSendEmail(strFrom, strTo, strSubject, strMsg, ctr)
            'End If
        End Try
    End Function

    Public Function ReSendEmail(ByVal strFrom As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMsg As String, ByVal attchemnt As String, ByVal ctr As Integer) As Boolean
        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient
        Try
            'Set From Email id
            Mail_Message.From = FromAddress
            'Set To Email id
            Mail_Message.To.Add(strTo)

            'Set Subject
            'Dim attachFile As New Attachment(txtAttachmentPath.Text)
            'MyMessage.Attachments.Add(attachFile)

            Mail_Message.Subject = strSubject
            'Set Msg Body
            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True
            Mail_Message.Attachments.Add(New Attachment(attchemnt))

            msClient.Port = 25
            msClient.Host = "127.0.0.1"
            msClient.Credentials = New System.Net.NetworkCredential("", "") '

            'msClient.Port = 25

            'msClient.Host = "127.0.0.1"

            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)
            ReSendEmail = True
            ctr = 1
        Catch ex As Exception
            ReSendEmail = False

            'If ctr <= 5 Then
            '    ctr = ctr + 1
            '    ReSendEmail(strFrom, strTo, strSubject, strMsg, ctr)
            'End If
        End Try
    End Function

    Public Function SendEmailBCC(ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String, ByVal attchemnt As String) As Boolean
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
                    Mail_Message.Bcc.Add(strarrCC(j))
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
            If attchemnt <> "" Then
                Mail_Message.Attachments.Add(New Attachment(attchemnt))
            End If


            msClient.Port = 25

            ' msClient.Host = "127.0.0.1"

            msClient.Host = "127.0.0.1"
            msClient.Credentials = New System.Net.NetworkCredential("", "") '

            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)
            SendEmailBCC = True
        Catch ex As Exception
            SendEmailBCC = False
        End Try
    End Function
    Public Function SendCDOMessage(ByVal attach_filename As String, ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String) As Boolean

        Try
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

                Mail_Message.Priority = MailPriority.Normal
                Mail_Message.IsBodyHtml = True
                'Mail_Message.Attachments.Add(New Attachment(attach_filename))

                If attach_filename <> "" Then
                    Mail_Message.Attachments.Add(New Attachment(attach_filename))
                End If


                msClient.Port = 25
                msClient.Host = "127.0.0.1"
                msClient.Credentials = New System.Net.NetworkCredential("", "") '

                'msClient.Port = 25
                'msClient.Host = "127.0.0.1"
                'msClient.Credentials = New System.Net.NetworkCredential("", "") '



                msClient.Send(Mail_Message)
                SendCDOMessage = True
            Catch ex As Exception
                SendCDOMessage = False
            End Try
            SendCDOMessage = True
        Catch ex As Exception
            SendCDOMessage = False
        End Try


    End Function

End Class
