Imports Microsoft.VisualBasic
Imports System.IO
Imports System
Imports System.Net

Public Class clsFax
    Inherits System.Web.UI.Page
    Public Function sendFax(ByVal bookingrefno As String, ByVal tonumber As String, ByVal toperson As String, ByVal filepath As String, ByVal constr As String) As Boolean
        'This function copies the fax file(pdf report) to fax outbox folder.
        Dim objUtils As New clsUtils
        Try


            Dim autofaxpath As String = ""
            Dim strSqlQry As String = ""
            Dim newfaxfilename As String = ""
            Dim targetpath As String = ""
            strSqlQry = "select option_selected from reservation_parameters where param_id='1009'"
            autofaxpath = objUtils.ExecuteQueryReturnStringValuenew(constr, strSqlQry)
            newfaxfilename = bookingrefno.Replace("/", "-") & "_" & toperson & "@" & tonumber.Replace(" ", "") & ".pdf"
            newfaxfilename = newfaxfilename.Replace("/", "-")
            ' targetpath = HttpContext.Current.Server.MapPath(autofaxpath) & "/" & newfaxfilename
            targetpath = autofaxpath & "\" & newfaxfilename

            Dim thefile As FileInfo = New FileInfo(filepath)
            Dim targetfile As FileInfo = New FileInfo(targetpath)

            If Not thefile.Exists Then
                Return False
            Else
                If Not targetfile.Exists Then
                    If objUtils.ExecuteQueryReturnSingleValuenew(constr, "select option_selected  from reservation_parameters where param_id =1050") = "DXB" Then
                        thefile.CopyTo(targetpath, False)
                        Return True
                    Else

                        SendFileToDBSErver(filepath, newfaxfilename, autofaxpath & "\")

                    End If
                End If
            End If
            Return True
        Catch ex As Exception

            objUtils.WritErrorLog("PriseList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Return False
        End Try
    End Function


    Private Sub SendFileToDBSErver(ByVal filepath, ByVal filename, ByVal faxfilepath)
        Dim SrvfaxUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUserName"), String)
        Dim SrvfaxPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxPassword"), String)
        Dim SrvfaxDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxDomain"), String)
        Dim SrvfaxUri As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUri"), String)
        '   Dim SrvfaxFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxFilePath"), String)

        Dim FS As New FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read)
        Dim faxfile(CInt(FS.Length)) As Byte
        FS.Read(faxfile, 0, CInt(FS.Length))

        Dim nwkCred As NetworkCredential = New NetworkCredential(SrvfaxUserName, SrvfaxPassword, SrvfaxDomain)
        Dim proxy As WebProxy = New WebProxy(SrvfaxUri)
        proxy.Credentials = nwkCred
        'Dim Service1 As New FaxReference.UploadService
        'Service1.Proxy = proxy
        'Dim t As String = ""
        't = Service1.Uploadfile(faxfile, faxfilepath, filename)


        'Service1.Dispose()
        FS.Dispose()

    End Sub

End Class
