Imports System.Net
Imports Microsoft.Office.Interop.Word
Imports System.IO


Partial Class PriceListModule_DownloadFiles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim objUtils As New clsUtils
        Try
            Dim strFile As String
            strFile = Request.QueryString("filename")
            Dim strFileLoc As String
            strFileLoc = Request.QueryString("fileLoc")
            Dim filePath As String = "~\\" & strFileLoc & "\\" + strFile
            Dim path As String = Server.MapPath(filePath)
            Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)

            Dim ext = IO.Path.GetExtension(filePath)
            Dim type As String = ""
            Dim forceDownload As Boolean = True

            If Not IsDBNull(ext) Then
                ext = LCase(ext)
            End If

            Select Case ext
                Case ".htm", ".html"
                    type = "text/HTML"
                Case ".txt"
                    type = "text/plain"
                Case ".doc", ".rtf", ".docx"
                    type = "Application/msword"
                Case ".csv", ".xls", ".xlsx "
                    type = "Application/x-msexcel"
                Case ".jpg", ".jpeg", ".gif", ".png"
                    type = "image/jpeg"
                Case ".gif"
                    type = "image/GIF"
                Case ".pdf"
                    type = "application/pdf"
                Case Else
                    type = "text/plain"
            End Select
            If file.Exists Then
                Response.Clear()
                Response.AppendHeader("Content-Length", file.Length.ToString())
                Response.AppendHeader("content-disposition", "inline; filename=" + file.Name)

                If type <> "" Then
                    Response.ContentType = type
                End If
                Response.WriteFile(file.FullName)
                Response.End()
            Else
                Response.Write("This file does not exist.")
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("DownloadFiles.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    'Private Sub OpenMSWordFileByBrowser()

    '    Dim htmlFilePath As String = "E:\\test.html"
    '    Convert("E:\\test.docx", htmlFilePath, WdSaveFormat.wdFormatHTML)

    '    Response.ClearContent()
    '    Response.ClearHeaders()
    '    Response.WriteFile(htmlFilePath)
    '    Response.Flush()
    '    Response.Close()
    'End Sub


    'Private Shared  Sub Convert(ByVal docFilePath As String, ByVal htmlFilePath As String, ByVal format As WdSaveFormat)

    '    Dim dirInfo As DirectoryInfo = New DirectoryInfo(docFilePath)
    '    Dim wordFile As FileInfo = New FileInfo(docFilePath)
    '    '
    '    Dim oMissing As Object = System.Reflection.Missing.Value
    '    Dim word As Microsoft.Office.Interop.Word.Application = New Microsoft.Office.Interop.Word.Application()
    '    Try
    '        word.Visible = False
    '        word.ScreenUpdating = False

    '        Dim filename As Object = CType(wordFile.FullName, Object)
    '        Document(doc = word.Documents.Open(filename, oMissing,
    '                                            oMissing, oMissing, oMissing, oMissing, oMissing,
    '                                            oMissing, oMissing, oMissing, oMissing, oMissing,
    '                                            oMissing, oMissing, oMissing, oMissing))
    '        Try
    '            doc.Activate()
    '            Dim outputFileName As Object = htmlFilePath
    '            Dim fileFormat As Object = format
    '            doc.SaveAs(outputFileName,
    '                        fileFormat, oMissing, oMissing,
    '                        oMissing, oMissing, oMissing, oMissing,
    '                        oMissing, oMissing, oMissing, oMissing,
    '                        oMissing, oMissing, oMissing, oMissing)

    '        Finally
    '            Dim saveChanges As Object = WdSaveOptions.wdDoNotSaveChanges
    '            (CType(doc, _Document)).Close( saveChanges,  oMissing,  oMissing)
    '            Do
    '            Loop While c = Nothing
    '        End Try
    '    Finally
    '        (CType(word, _Application)).Quit( oMissing,  oMissing,  oMissing)
    '        word = Nothing
    '    End Try
    'End Sub


End Class
