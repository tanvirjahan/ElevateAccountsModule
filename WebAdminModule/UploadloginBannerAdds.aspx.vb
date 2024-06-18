Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Net

Partial Class UploadLoginBannerAdds
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim filename As String = ""
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("UploadBannerState", Request.QueryString("State"))
                ViewState.Add("UploadBannerRefCode", Request.QueryString("RefCode"))
                If ViewState("UploadBannerState") = "New" Then
                    SetFocus(txtAlternateText)
                    lblHeading.Text = "Add New Login Banner Adds"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save upload Login banner adds?')==false)return false;")
                ElseIf ViewState("UploadBannerState") = "Edit" Then
                    SetFocus(txtAlternateText)
                    lblHeading.Text = "Edit Login Banner Adds"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("UploadBannerRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update upload Login banner adds?')==false)return false;")
                ElseIf ViewState("UploadBannerState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Login Banner Adds"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    btnSave.Visible = False
                    ShowRecord(CType(ViewState("UploadBannerRefCode"), String))

                ElseIf ViewState("UploadBannerState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Login Banner Adds"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("UploadBannerRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete upload Login banner adds?')==false)return false;")
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
        If ViewState("UploadBannerState") = "New" Then
            fuImage.Visible = True
            txtImage.Visible = False
        ElseIf ViewState("UploadBannerState") = "Edit" Then
            txtImage.Visible = True
            txtImage.Enabled = False
        Else
            fuImage.Visible = False
            txtImage.Visible = True
            txtImage.Enabled = False
            'txtAlternateText.Enabled = False
            'txtNavigateUrl.Enabled = False
            btnUpload.Visible = False
        End If

    End Sub
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agentlogin_banner Where bannerid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("bannerid")) = False Then
                        Me.txtBannerId.Text = CType(mySqlReader("bannerid"), String)
                    Else
                        Me.txtBannerId.Text = ""
                    End If
                    'If IsDBNull(mySqlReader("AlternateText")) = False Then
                    '    Me.txtAlternateText.Text = CType(mySqlReader("AlternateText"), String)
                    'Else
                    '    Me.txtAlternateText.Text = ""
                    'End If
                    'If IsDBNull(mySqlReader("NavigateUrl")) = False Then
                    '    Me.txtNavigateUrl.Text = CType(mySqlReader("NavigateUrl"), String)
                    'Else
                    '    Me.txtNavigateUrl.Text = ""
                    'End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            ChkInactive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            ChkInactive.Checked = False
                        End If
                    End If


                    If IsDBNull(mySqlReader("imagename")) = False Then
                        Me.txtImage.Text = CType(mySqlReader("imagename"), String)
                        ImgBanner.ImageUrl = txtImage.Text
                        ViewState("Filename") = CType(mySqlReader("imagename"), String)
                    Else
                        Me.txtImage.Text = ""
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

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("UploadLoginBannerAddsSearch.aspx", False)
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            If Page.IsValid = True Then
                If ViewState("UploadBannerState") = "New" Or ViewState("UploadBannerState") = "Edit" Then

                    'If fuImage.HasFile Then
                    '    Image1.ImageUrl = fuImage.FileName
                    '    Exit Sub
                    'End If

                    'If fuImage.HasFile Then
                    '    Dim strFileName As String
                    '    strFileName = "Banner_" + fuImage.FileName
                    '    If File.Exists(Server.MapPath("~\AgentModule\UploadedImages\" + strFileName)) = True Then
                    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File alredy exist. Select other file name.');", True)
                    '        SetFocus(fuImage)
                    '        Exit Sub
                    '    End If
                    '    fuImage.PostedFile.SaveAs(Server.MapPath("~\AgentModule\UploadedImages\" + strFileName))
                    '    txtImage.Text = "~\AgentModule\UploadedImages\" + strFileName
                    'End If

                    'If CheckImage() = False Then
                    '    If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                    '        File.Delete(Server.MapPath(txtImage.Text.Trim))
                    '    End If
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                    '    Exit Sub
                    'End If
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("UploadBannerState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_loginBannerAds", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@Imagename", SqlDbType.VarChar, 300)).Value = ViewState("Filename") 'CType(txtImage.Text.Trim, String)
                        If ChkInactive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                        ElseIf ChkInactive.Checked = False Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                        End If

                    ElseIf ViewState("UploadBannerState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_loginBannerAds", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@bannerid", SqlDbType.Int, 4)).Value = CType(txtBannerId.Text.Trim, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@Imagename", SqlDbType.VarChar, 300)).Value = ViewState("Filename") ';CType(txtImage.Text.Trim, String)
                        If ChkInactive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                        ElseIf ChkInactive.Checked = False Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                        End If

                    End If
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("UploadBannerState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_delete_loginBannerAds", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@bannerid", SqlDbType.Int, 4)).Value = CType(txtBannerId.Text.Trim, Integer)
                    mySqlCmd.ExecuteNonQuery()

                    If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                        File.Delete(Server.MapPath(txtImage.Text.Trim))
                    End If
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("UploadBannerAddsSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('UploadLoginBannerWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If

        Catch ex As Exception
            sqlTrans.Rollback()
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            'If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
            '    File.Delete(Server.MapPath(txtImage.Text.Trim))
            'End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("UploadBannerAdds.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            Dim width As Integer = 1350 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=517")  ' fullSizeImg.Width
            Dim height As Integer = 748 ' objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=518") 'fullSizeImg.Height

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

    Public Sub SendImageToWebService(ByVal Path, ByVal filename)
        Dim SrvUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvUserName"), String)
        Dim SrvPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvPassword"), String)
        Dim SrvDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvDomain"), String)
        Dim SrvUri As String = CType(ConfigurationManager.AppSettings.Get("SrvUri"), String)
        Dim SrvWebFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvWebFilePath"), String)

        Dim FS As New FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read)
        Dim Img(CInt(FS.Length)) As Byte
        FS.Read(Img, 0, CInt(FS.Length))

        Dim nwkCred As NetworkCredential = New NetworkCredential(SrvUserName, SrvPassword, SrvDomain)
        Dim proxy As WebProxy = New WebProxy(SrvUri)
        proxy.Credentials = nwkCred
        Dim Service1 As New UploadService.UploadService
        Service1.Proxy = proxy

        Service1.UploadImage(Img, SrvWebFilePath, filename)
        Service1.Dispose()
    End Sub

#Region "Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            Dim strpath_logo1 As String = ""
            Dim strpath1 As String = ""
            If fuImage.HasFile Then
                Dim strFileName As String
                Dim ext As String
                strFileName = "loginbanner_" + fuImage.FileName
                filename = "UploadHomeImage/loginbanner_" + fuImage.FileName

                ViewState("Filename") = filename
                ext = strFileName.Substring(strFileName.LastIndexOf(".") + 1)
                'ext == "gif" || ext == "GIF" || ext == "JPEG" || ext == "jpeg" || ext == "jpg" || ext == "JPG")
                If ext.ToUpper = "GIF" Or ext.ToUpper = "JPEG" Or ext.ToUpper = "JPG" Then
                    If File.Exists(Server.MapPath("~/agentsonline/UploadHomeImage/" + strFileName)) = True Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File alredy exist. Select other file name.');", True)
                        SetFocus(fuImage)
                        Exit Sub
                    End If
                    fuImage.PostedFile.SaveAs(Server.MapPath("~/agentsonline/UploadHomeImage/" + strFileName))
                    txtImage.Text = "~/agentsonline/UploadHomeImage/" + strFileName

                    If CheckImage() = False Then
                        If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                            File.Delete(Server.MapPath(txtImage.Text.Trim))
                        End If
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                        Exit Sub
                    Else
                        ' fuImage.PostedFile.SaveAs(Server.MapPath("~/WebAdminModule/UploadHomeImage/" + strFileName))

                        ' SendImageToWebService(Server.MapPath("~/WebAdminModule/UploadHomeImage/" + strFileName), strFileName)
                        'strpath_logo1 = fileVehicleImage.FileName ' IIf(txtimg.Value = "", fileVehicleImage.FileName, txtimg.Value)

                        'strpath1 = Server.MapPath("onlineimages/" & strpath_logo1)
                        'fileVehicleImage.PostedFile.SaveAs(strpath1)
                    End If
                    ImgBanner.ImageUrl = txtImage.Text
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select image file.' );", True)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Upload Gif or Jpg images only.' );", True)
                Exit Sub
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region



#Region "Protected Sub btnUpload1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload1.Click"
    Protected Sub btnUpload1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload1.Click
        Try
            If fuflash.HasFile Then
                Dim strFileName As String
                Dim ext As String
                strFileName = fuflash.FileName
                ext = strFileName.Substring(strFileName.LastIndexOf(".") + 1)

                If ext.ToUpper = "SWF" Then

                    'If File.Exists(Server.MapPath("~/AgentModule/" + strFileName)) = True Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File alredy exist. Select other file name.');", True)
                    '    SetFocus(fuImage)
                    '    Exit Sub
                    'End If

                    fuflash.PostedFile.SaveAs(Server.MapPath("~/AgentModule/" + strFileName))
                    txtflash.Text = "~/AgentModule/" + strFileName

                    If CheckImage() = False Then
                        If File.Exists(Server.MapPath(txtflash.Text.Trim)) = True Then
                            File.Delete(Server.MapPath(txtflash.Text.Trim))
                        End If
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                        Exit Sub
                    Else
                        SendImageToWebService(Server.MapPath("~/AgentModule/" + strFileName), strFileName)
                    End If
                    '' ImgBanner.ImageUrl = txtflash.Text
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Flash file.' );", True)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Upload Flash only.' );", True)
                Exit Sub
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region

#Region "Private Function ValidatePage() As Boolean"
    Private Function ValidatePage() As Boolean
        If txtImage.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please click on upload button to upload image.' );", True)
            ValidatePage = False
            Exit Function
        End If
        ValidatePage = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UploadBannerAdds','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class

