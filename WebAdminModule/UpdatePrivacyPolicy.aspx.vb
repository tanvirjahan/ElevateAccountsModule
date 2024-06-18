Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO

Partial Class UpdatePrivacyPolicy
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim mySqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then



            Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim strappid As String = ""
            Dim strappname As String = ""
            If AppId Is Nothing = False Then
                strappid = AppId.Value
            End If
            If AppName Is Nothing = False Then
                strappname = AppName.Value
            End If

            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            Try
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("select * from webdetails where webdetailid=1", mySqlConn, sqlTrans)
                mySqlReader = mySqlCmd.ExecuteReader()
                If mySqlReader.Read Then
                    Session.Add("State", "Edit")
                    If IsDBNull(mySqlReader("webdetailid")) = False Then
                        txtWebDetailId.Text = mySqlReader("webdetailid")
                    End If
                    If IsDBNull(mySqlReader("detail_text")) = False Then
                        Editor1.Content = mySqlReader("detail_text")
                    End If
                    btnSave.Text = "Update"
                Else
                    Session.Add("State", "Add")
                    txtWebDetailId.Text = "1"
                    btnSave.Text = "Save"
                End If
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("UpdatePrivacyPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            ''testing code for groups
            'Dim strfilenm As String = ""
            'File.Create("C:\wkhtmltopdf\sample.html").Dispose()

            'Dim strFile As String = "C:\wkhtmltopdf\sample.html"
            'Dim fileExists As Boolean = File.Exists(strFile)
            'Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))
            '    sw.WriteLine(IIf(fileExists, Editor1.Content, ""))
            'End Using

            'If fileExists Then
            '    strfilenm = GetWebImage()
            'End If
            ''******************

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
            If CType(Session("State"), String) = "Add" Then
                mySqlCmd = New SqlCommand("sp_add_webdetails", mySqlConn, sqlTrans)
            ElseIf CType(Session("State"), String) = "Edit" Then
                mySqlCmd = New SqlCommand("sp_mod_webdetails", mySqlConn, sqlTrans)
            End If
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@webdetailid", SqlDbType.Int, 9)).Value = txtWebDetailId.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@detail_text", SqlDbType.Text)).Value = Editor1.Content 'strfilenm txttest.Value 
            mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Save Sucessfully.');", True)
            Session.Add("State", "Edit")
            btnSave.Text = "Update"
        Catch ex As Exception
            objUtils.WritErrorLog("UpdatePrivacyPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


#End Region

    'Private Function GetWebImage() As String
    '    Dim strUrl As String = "C:\wkhtmltopdf\sample.html" 'Request.Url.AbsolutePath
    '    Dim strfile As String = ""
    '    strfile = WKHtmlToPdf(strUrl)
    '    'If (strfile IsNot Nothing) Then
    '    '    Response.ContentType = "image/png"
    '    '    Response.BinaryWrite(strfile)
    '    '    Response.End()
    '    'End If
    '    Return strfile
    'End Function


    'Private Function WKHtmlToPdf(ByVal url As String) As String  'Byte()
    '    'Dim strfileName As String = " - "
    '    Dim strwkhtmlDir As String = "C:\\wkhtmltopdf\\" 'C:\wkhtmltopdf
    '    Dim strwkhtml As String = "C:\\wkhtmltopdf\\wkhtmltoimage.exe"
    '    Dim p As New Process
    '    ' assemble destination PDF file name
    '    Dim strfileName As String = "D:\\althu\\dd.png" 'ConfigurationManager.AppSettings["ExportFilePath"] + "\\" + outputFilename + ".pdf";
    '    'Dim project As  Project= new Project(int.Parse(outputFilename));
    '    Dim strrealfileName As String = "D:\althu\dd.png"

    '    p.StartInfo.CreateNoWindow = True
    '    p.StartInfo.RedirectStandardOutput = True
    '    p.StartInfo.RedirectStandardError = True
    '    p.StartInfo.RedirectStandardInput = True
    '    p.StartInfo.UseShellExecute = False
    '    p.StartInfo.FileName = strwkhtml
    '    p.StartInfo.WorkingDirectory = strwkhtmlDir

    '    Dim switches As String = ""
    '    switches += "--crop-w 720 "
    '    'switches += "--print-media-type "
    '    'switches += "--margin-top 10mm --margin-bottom 10mm --margin-right 10mm --margin-left 10mm "
    '    'switches += "--page-size Letter "
    '    p.StartInfo.Arguments = switches + " " + url + " " + strfileName

    '    p.Start()

    '    'read output
    '    'Dim buffer(32768) As Byte
    '    'Dim file() As Byte
    '    'Using MS As New MemoryStream

    '    '    While (True)

    '    '        Dim read As Integer = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length)

    '    '        If (read <= 0) Then

    '    '            Exit While
    '    '        End If
    '    '        MS.Write(buffer, 0, read)
    '    '    End While
    '    '    file = MS.ToArray()
    '    'End Using

    '    ' wait or exit
    '    p.WaitForExit(60000)

    '    ' read the exit code, close process
    '    Dim returnCode As Integer = p.ExitCode
    '    p.Close()

    '    'Return IIf(returnCode = 0, File, 0)
    '    Return IIf(returnCode = 0, strrealfileName, "")


    'End Function


    






  

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/MainPage.aspx", False)
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UpdatePrivacyPolicy','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
