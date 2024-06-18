
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Net
#End Region


Partial Class WebAdminModule_UploadImagesForHomePg
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim chckDeletion As CheckBox
    Dim mySqlConn As SqlConnection
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("UploadImgHomePgState", Request.QueryString("State"))
                ViewState.Add("UploadImgHomePgRefCode", Request.QueryString("RefCode"))
                If ViewState("UploadImgHomePgState") = "New" Then
                    SetFocus(ddlImgPostion)
                    lblHeading.Text = "Add New Images for Home Page"
                    btnSave.Text = "Save"
                    chkActive.Checked = True
                    txtRankOrder.Text = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select ISNULL(max(rankorder),0 )+1  from    homepageimages ")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save/upload Image for home page?')==false)return false;")
                ElseIf ViewState("UploadImgHomePgState") = "Edit" Then
                    SetFocus(ddlImgPostion)
                    lblHeading.Text = "Edit Images for Home Page"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("UploadImgHomePgRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update upload Image for home page?')==false)return false;")
                ElseIf ViewState("UploadImgHomePgState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Image for home page"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    btnSave.Visible = False
                    ShowRecord(CType(ViewState("UploadImgHomePgRefCode"), String))

                ElseIf ViewState("UploadImgHomePgState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Image for home page"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("UploadImgHomePgRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete upload Image for home page?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                'objUtils.WritErrorLog("UploadBannerAdds.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("UploadImgHomePgState") = "New" Then
            fuImage.Visible = True
            txtImage.Visible = False
        ElseIf ViewState("UploadImgHomePgState") = "Edit" Then
            txtImage.Visible = True
            txtImage.Enabled = False
        Else
            fuImage.Visible = False
            txtImage.Visible = True
            txtImage.Enabled = False
            txtRankOrder.Enabled = False
            ddlImgPostion.Enabled = False
            'btnUpload.Visible = False
        End If

    End Sub
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select autoid imageid,imagename ,imageposition ,active ,rankorder from homepageimages Where autoid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("imageid")) = False Then
                        Me.txtImageId.Text = CType(mySqlReader("imageid"), String)
                    Else
                        Me.txtImageId.Text = ""
                    End If
                    If IsDBNull(mySqlReader("imageposition")) = False Then
                        Me.ddlImgPostion.SelectedValue = CType(mySqlReader("imageposition"), String)
                    Else
                        Me.ddlImgPostion.SelectedValue = ""
                    End If
                    If IsDBNull(mySqlReader("rankorder")) = False Then
                        Me.txtRankOrder.Text = CType(mySqlReader("rankorder"), String)
                    Else
                        Me.txtRankOrder.Text = ""
                    End If

                    If IsDBNull(mySqlReader("imagename")) = False Then
                        Me.txtImage.Text = CType(mySqlReader("imagename"), String)
                        ImgBanner.ImageUrl = "UploadHomeImage/" + txtImage.Text
                    Else
                        Me.txtImage.Text = ""
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        Me.chkActive.Checked = IIf(CType(mySqlReader("active"), Integer) = 1, True, False)
                        'ImgBanner.ImageUrl = txtImage.Text
                    Else
                        Me.chkActive.Checked = False
                    End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("UploadBannerAdds.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region


#Region "Private Function CheckImage() As Boolean"
    Private Function CheckImage() As Boolean
        If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
            Dim imageurl As String = Server.MapPath(txtImage.Text.Trim)
            Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)
            'Dim width As Integer = fullSizeImg.Width
            'Dim height As Integer = fullSizeImg.Height

            Dim width As Integer = 288 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=517")  ' fullSizeImg.Width
            Dim height As Integer = 209 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=518") 'fullSizeImg.Height

            If fullSizeImg.Height > height Then
                fullSizeImg.Dispose()
                CheckImage = False
                Exit Function
            Else
                CheckImage = True
            End If
            If fullSizeImg.Width > width Then
                fullSizeImg.Dispose()
                CheckImage = False
                Exit Function
            Else
                CheckImage = True
            End If
        End If
        CheckImage = True
    End Function
#End Region

    Public Function UploadImage() As Boolean
        Try
            Dim strFileName As String
            Dim ext As String
            If ViewState("UploadImgHomePgState") <> "Delete" Then
                If fuImage.HasFile Then
                    strFileName = fuImage.FileName
                    ext = strFileName.Substring(strFileName.LastIndexOf(".") + 1)
                    'ext == "gif" || ext == "GIF" || ext == "JPEG" || ext == "jpeg" || ext == "jpg" || ext == "JPG")
                    If ext.ToUpper = "GIF" Or ext.ToUpper = "JPEG" Or ext.ToUpper = "JPG" Then
                        'If File.Exists(Server.MapPath("UploadedImages/" + strFileName)) = True Then
                        If CheckImageExistToWebService(strFileName) = True And ViewState("UploadImgHomePgState") <> "Edit" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File already exist. Select other file name.');", True)
                            SetFocus(fuImage)
                            Return False
                        End If
                        If ViewState("UploadImgHomePgState") = "Edit" Then
                            'Delete from webserver code
                            'Dlete already uploaded image txtimage not fuimage
                            DeleteImageToWebService(txtImage.Text)

                        End If

                        fuImage.PostedFile.SaveAs(Server.MapPath("TempImage/" + strFileName))
                        txtImage.Text = "TempImage/" + strFileName

                        If CheckImage() = False Then
                            If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                                File.Delete(Server.MapPath(txtImage.Text.Trim))
                            End If

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                            Return False
                        Else

                            If SendImageToWebService(Server.MapPath("TempImage/"), strFileName) = False Then
                                Return False
                            End If
                            'If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                            '    File.Delete(Server.MapPath(txtImage.Text.Trim))
                            'End If
                            txtImage.Text = strFileName 'removing  "TempImage/" text, for saving to db 
                            'If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                            '    File.Delete(Server.MapPath(txtImage.Text.Trim))
                            'End If
                        End If
                        'ImgBanner.ImageUrl = txtImage.Text

                        txtImage.Text = strFileName 'removing  "TempImage/" text, for saving to db 
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Upload Gif or Jpg images only.' );", True)
                        Return False
                    End If
                Else
                    If txtImage.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select image file.' );", True)
                        Return False
                    End If
                End If
            Else
                'call for deleting file
                'CheckImage if exists
                DeleteImageToWebService(txtImage.Text)
            End If

        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
        UploadImage = True
    End Function

    Public Function SendImageToWebService(ByVal Path, ByVal filename) As Boolean
        'web server
        Dim SrvWEbUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvUserName"), String)
        Dim SrvWEbPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvPassword"), String)
        Dim SrvWEbDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvDomain"), String)
        Dim SrvWEbUri As String = CType(ConfigurationManager.AppSettings.Get("SrvUri"), String)
        Dim SrvWEbFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvImageWEBDBFilePath"), String)

        'DB SERvER
        Dim SrvDBUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUserName"), String)
        Dim SrvDBPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxPassword"), String)
        Dim SrvDBDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxDomain"), String)
        Dim SrvDBUri As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUri"), String)
        Dim SrvDBFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvImageWEBDBFilePath"), String)


        Dim FS As New FileStream(Path & filename, FileMode.OpenOrCreate, FileAccess.Read)
        Dim Img(CInt(FS.Length)) As Byte
        FS.Read(Img, 0, CInt(FS.Length))

        Dim nwkCred As NetworkCredential = New NetworkCredential(SrvWEbUserName, SrvWEbPassword, SrvWEbDomain)
        Dim proxy As WebProxy = New WebProxy(SrvWEbUri)
        proxy.Credentials = nwkCred
        Dim Service1 As New UploadService.UploadService
        Service1.Proxy = proxy
        Dim t As String = ""
        t = Service1.UploadImage(Img, SrvWEbFilePath, filename)

        Dim DBnwkCred As NetworkCredential = New NetworkCredential(SrvDBUserName, SrvDBPassword, SrvDBDomain)
        Dim DBproxy As WebProxy = New WebProxy(SrvDBUri)
        DBproxy.Credentials = DBnwkCred
        Dim Service2 As New FaxReference.UploadService
        Service2.Proxy = DBproxy
        Dim t1 As String = ""
        t1 = Service2.UploadImage(Img, SrvDBFilePath, filename)


        Service1.Dispose()
        Service2.Dispose()
        FS.Close()
        FS.Dispose()
        If t = "1" And t1 = "1" Then
            SendImageToWebService = False
        End If
        SendImageToWebService = True
    End Function

    Public Sub DeleteImageToWebService(ByVal filename)
        'web server
        Dim SrvWEbUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvUserName"), String)
        Dim SrvWEbPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvPassword"), String)
        Dim SrvWEbDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvDomain"), String)
        Dim SrvWEbUri As String = CType(ConfigurationManager.AppSettings.Get("SrvUri"), String)
        Dim SrvWEbFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvImageWEBDBFilePath"), String)

        'DB SERvER
        Dim SrvDBUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUserName"), String)
        Dim SrvDBPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxPassword"), String)
        Dim SrvDBDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxDomain"), String)
        Dim SrvDBUri As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUri"), String)
        Dim SrvDBFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvImageWEBDBFilePath"), String)


        'Dim FS As New FileStream(Path & filename, FileMode.OpenOrCreate, FileAccess.Read)
        'Dim Img(CInt(FS.Length)) As Byte
        'FS.Read(Img, 0, CInt(FS.Length))

        Dim nwkCred As NetworkCredential = New NetworkCredential(SrvWEbUserName, SrvWEbPassword, SrvWEbDomain)
        Dim proxy As WebProxy = New WebProxy(SrvWEbUri)
        proxy.Credentials = nwkCred
        Dim Service1 As New UploadService.UploadService
        Service1.Proxy = proxy
        Dim t As String = ""
        t = Service1.DeleteImage(SrvWEbFilePath, filename)

        Dim DBnwkCred As NetworkCredential = New NetworkCredential(SrvDBUserName, SrvDBPassword, SrvDBDomain)
        Dim DBproxy As WebProxy = New WebProxy(SrvDBUri)
        DBproxy.Credentials = DBnwkCred
        Dim Service2 As New FaxReference.UploadService
        Service2.Proxy = DBproxy
        Dim t1 As String = ""
        t1 = Service2.DeleteImage(SrvDBFilePath, filename)


        Service1.Dispose()
        Service2.Dispose()


    End Sub

    Public Function CheckImageExistToWebService(ByVal filename) As Boolean
        'web server
        Dim SrvWEbUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvUserName"), String)
        Dim SrvWEbPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvPassword"), String)
        Dim SrvWEbDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvDomain"), String)
        Dim SrvWEbUri As String = CType(ConfigurationManager.AppSettings.Get("SrvUri"), String)
        Dim SrvWEbFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvImageWEBDBFilePath"), String)

        'DB SERvER
        Dim SrvDBUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUserName"), String)
        Dim SrvDBPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxPassword"), String)
        Dim SrvDBDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxDomain"), String)
        Dim SrvDBUri As String = CType(ConfigurationManager.AppSettings.Get("SrvfaxUri"), String)
        Dim SrvDBFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvImageWEBDBFilePath"), String)


        Dim nwkCred As NetworkCredential = New NetworkCredential(SrvWEbUserName, SrvWEbPassword, SrvWEbDomain)
        Dim proxy As WebProxy = New WebProxy(SrvWEbUri)
        proxy.Credentials = nwkCred
        Dim Service1 As New UploadService.UploadService
        Service1.Proxy = proxy

        Dim DBExist, WebExist As Boolean
        If Service1.CheckImageExist(SrvWEbFilePath, filename) Then
            WebExist = True 'means file exist
        Else
            WebExist = False

        End If

        Dim DBnwkCred As NetworkCredential = New NetworkCredential(SrvDBUserName, SrvDBPassword, SrvDBDomain)
        Dim DBproxy As WebProxy = New WebProxy(SrvDBUri)
        DBproxy.Credentials = DBnwkCred
        Dim Service2 As New FaxReference.UploadService
        Service2.Proxy = DBproxy

        If Service2.CheckImageExist(SrvDBFilePath, filename) Then
            DBExist = True 'means file exist
        Else
            DBExist = False

        End If

        If DBExist = True Or WebExist = True Then
            Return True
        Else
            Return False
        End If

        Service1.Dispose()
        Service2.Dispose()
        Return False
    End Function

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            If Page.IsValid = True Then
                If ValidatePage() = True Then
                    If UploadImage() = True Then
                        If ViewState("UploadImgHomePgState") = "New" Or ViewState("UploadImgHomePgState") = "Edit" Then
                            If txtImage.Text.Trim <> "" Then
                                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                                If ViewState("UploadImgHomePgState") = "New" Then
                                    mySqlCmd = New SqlCommand("sp_add_homepageimages", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@imageposition", SqlDbType.VarChar, 20)).Value = CType(ddlImgPostion.SelectedValue, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(txtImage.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = IIf(chkActive.Checked = True, 1, 0)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int)).Value = CType(txtRankOrder.Text.Trim, Integer)
                                ElseIf ViewState("UploadImgHomePgState") = "Edit" Then
                                    mySqlCmd = New SqlCommand("sp_mod_homepageimages", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@autoid", SqlDbType.Int, 4)).Value = CType(txtImageId.Text.Trim, Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@imageposition", SqlDbType.VarChar, 20)).Value = CType(ddlImgPostion.SelectedValue, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(txtImage.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = IIf(chkActive.Checked = True, 1, 0)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int)).Value = CType(txtRankOrder.Text.Trim, Integer)
                                End If
                                mySqlCmd.ExecuteNonQuery()
                            End If

                        ElseIf ViewState("UploadImgHomePgState") = "Delete" Then
                            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                            mySqlCmd = New SqlCommand("sp_delete_homepageimages", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@autoid", SqlDbType.Int, 4)).Value = CType(txtImageId.Text.Trim, Integer)
                            mySqlCmd.ExecuteNonQuery()

                            'If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                            '    File.Delete(Server.MapPath(txtImage.Text.Trim))
                            'End If
                        End If
                        sqlTrans.Commit()    'SQl Tarn Commit
                        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                        clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                        'Response.Redirect("UploadBannerAddsSearch.aspx", False)
                        Dim strscript As String = ""
                        strscript = "window.opener.__doPostBack('UploadImageWebWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    End If
                Else
                    Exit Sub 'validate else 
                End If
            End If

        Catch ex As Exception
            sqlTrans.Rollback()
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            'If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
            '    File.Delete(Server.MapPath(txtImage.Text.Trim))
            'End If
            DeleteImageToWebService(txtImage.Text.Trim)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("UploadBannerAdds.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function ValidatePage() As Boolean"
    Private Function ValidatePage() As Boolean
        If fuImage.HasFile = False And txtImage.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select an image to upload .' );", True)
            ValidatePage = False
            Exit Function
        End If
        ValidatePage = True
    End Function
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("UploadImagesForHomePgSearch.aspx", False)
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UploadHomeImages','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
