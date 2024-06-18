Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Net

Partial Class UploadContract
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim uploadname As String
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ShowRecord()
                'ViewState.Add("UploadContractState", Request.QueryString("UploadContractState"))
                'ViewState.Add("UploadContractRefCode", Request.QueryString("RefCode"))
                'If ViewState("UploadContractState") = "New" Then
                '    SetFocus(txtAlternateText)
                '    lblHeading.Text = "Add Contract"
                '    btnSave.Text = "Save"
                '    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save upload Contract?')==false)return false;")
                'ElseIf ViewState("UploadContractState") = "Edit" Then
                '    SetFocus(txtAlternateText)
                '    lblHeading.Text = "Edit Contract"
                '    btnSave.Text = "Update"
                '    DisableControl()
                '    ShowRecord(CType(ViewState("UploadContractRefCode"), String))
                '    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update upload Contract?')==false)return false;")
                'ElseIf ViewState("UploadContractState") = "View" Then
                '    SetFocus(btnCancel)
                '    lblHeading.Text = "View Contract"
                '    btnSave.Visible = False
                '    btnCancel.Text = "Return to Search"
                '    DisableControl()
                '    btnSave.Visible = False
                '    ShowRecord(CType(ViewState("UploadContractRefCode"), String))

                'ElseIf ViewState("UploadContractState") = "Delete" Then
                '    SetFocus(btnSave)
                '    lblHeading.Text = "Delete Contract"
                '    btnSave.Text = "Delete"
                '    DisableControl()

                '    ShowRecord(CType(ViewState("UploadContractRefCode"), String))
                '    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete upload Contract?')==false)return false;")
                ' End If
                'btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                'objUtils.WritErrorLog("UploadBannerAdds.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("UploadContractState") = "New" Then
            fuImage.Visible = True
            lblImage.Visible = False
        ElseIf ViewState("UploadContractState") = "Edit" Then
            lblImage.Visible = True
            lblImage.Enabled = False
        Else
            fuImage.Visible = False
            lblImage.Visible = True
            lblImage.Enabled = False
            txtAlternateText.Enabled = False
            btnUpload.Visible = False

        End If

    End Sub
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord()

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agentcontract", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("AlternateText")) = False Then
                        Me.txtAlternateText.Text = CType(mySqlReader("AlternateText"), String)
                    Else
                        Me.txtAlternateText.Text = ""
                    End If

                    If IsDBNull(mySqlReader("imageurl")) = False Then
                        Me.lblImage.Text = CType(mySqlReader("imageurl"), String)
                    Else
                        Me.lblImage.Text = ""
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
        Response.Redirect("~/MainPage.aspx", False)
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            If Page.IsValid = True Then
                '   If ViewState("UploadContractState") = "New" Or ViewState("UploadContractState") = "Edit" Then

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

                'If ViewState("UploadContractState") = "New" Then
                mySqlCmd = New SqlCommand("sp_add_agentcontract", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@AlternateText", SqlDbType.VarChar, 100)).Value = CType(txtAlternateText.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@ImageUrl", SqlDbType.VarChar, 100)).Value = CType(lblImage.Text.Trim, String)
                'ElseIf ViewState("UploadContractState") = "Edit" Then
                '    mySqlCmd = New SqlCommand("sp_mod_Contract", mySqlConn, sqlTrans)
                '    mySqlCmd.CommandType = CommandType.StoredProcedure
                '    mySqlCmd.Parameters.Add(New SqlParameter("@AlternateText", SqlDbType.VarChar, 100)).Value = CType(txtAlternateText.Text.Trim, String)
                '    mySqlCmd.Parameters.Add(New SqlParameter("@ImageUrl", SqlDbType.VarChar, 100)).Value = CType(lblImage.Text.Trim, String)
                'End If
                mySqlCmd.ExecuteNonQuery()

                'ElseIf ViewState("UploadContractState") = "Delete" Then
                '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                '    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                '    mySqlCmd = New SqlCommand("sp_delete_Contract", mySqlConn, sqlTrans)
                '    mySqlCmd.CommandType = CommandType.StoredProcedure
                '    mySqlCmd.Parameters.Add(New SqlParameter("@AlternateText", SqlDbType.VarChar, 100)).Value = CType(txtAlternateText.Text.Trim, String)
                '    mySqlCmd.ExecuteNonQuery()

                '    If File.Exists(Server.MapPath(lblImage.Text.Trim)) = True Then
                '        File.Delete(Server.MapPath(lblImage.Text.Trim))
                '    End If
                ' End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("UploadBannerAddsSearch.aspx", False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
            End If

        Catch ex As Exception
            sqlTrans.Rollback()
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            If File.Exists(Server.MapPath(lblImage.Text.Trim)) = True Then
                File.Delete(Server.MapPath(lblImage.Text.Trim))
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("UploadBannerAdds.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function CheckImage() As Boolean"
    Private Function CheckImage() As Boolean
        If File.Exists(Server.MapPath(lblImage.Text.Trim)) = True Then
            Dim imageurl As String = Server.MapPath(lblImage.Text.Trim)
            Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromFile(imageurl)
            'Dim width As Integer = fullSizeImg.Width
            'Dim height As Integer = fullSizeImg.Height

            Dim width As Integer = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=517")  ' fullSizeImg.Width
            Dim height As Integer = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=518") 'fullSizeImg.Height

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
        Dim SrvFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvFilePath"), String)

        Dim FS As New FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read)
        Dim Img(CInt(FS.Length)) As Byte
        FS.Read(Img, 0, CInt(FS.Length))

        Dim nwkCred As NetworkCredential = New NetworkCredential(SrvUserName, SrvPassword, SrvDomain)
        Dim proxy As WebProxy = New WebProxy(SrvUri)
        proxy.Credentials = nwkCred
        Dim Service1 As New UploadService.UploadService
        Service1.Proxy = proxy

        Service1.UploadImage(Img, SrvFilePath, filename)
        Service1.Dispose()
    End Sub

#Region "Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            If txtAlternateText.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Alter text Should not be blank.' );", True)
                Exit Sub
            End If

            If ViewState("UploadContractState") = "New" Then
                Dim dsval As DataSet
                dsval = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select 't' from agentcontract ")
                If dsval.Tables.Count > 0 Then
                    If dsval.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(dsval.Tables(0).Rows(0)(0)) = False Then
                            If dsval.Tables(0).Rows(0)(0).ToString() <> "" Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contract already exists cannot add again.');", True)
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If

            If ViewState("UploadContractState") = "Edit" Then
                Dim dsval As DataSet
                dsval = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select AlternateText from agentcontract ")
                If dsval.Tables.Count > 0 Then
                    If dsval.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(dsval.Tables(0).Rows(0)(0)) = False Then
                            If dsval.Tables(0).Rows(0)(0).ToString() = CType(txtAlternateText.Text.Trim, String) Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Change the alternate text name.');", True)
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If

            If fuImage.HasFile Then
                Dim strFileName As String
                strFileName = fuImage.FileName

                uploadname = txtAlternateText.Text.Trim + "_" + strFileName
                ViewState.Add("checkuploadname", "../PricelistModule/UploadedImages/" + uploadname)

                If File.Exists(Server.MapPath("../PricelistModule/UploadedImages/" + uploadname)) = True Then
                    File.Delete(Server.MapPath("../PricelistModule/UploadedImages/" + uploadname))
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File alredy exist. Select other file name.');", True)
                    ' SetFocus(fuImage)
                    'Exit Sub
                End If
                fuImage.PostedFile.SaveAs(Server.MapPath("~/PricelistModule/UploadedImages/" + uploadname))
                lblImage.Text = "../PricelistModule/UploadedImages/" + uploadname

                If Not File.Exists(lblImage.Text.Trim) = True Then
                    'SendImageToWebService(Server.MapPath("../PricelistModule/UploadedImages/" + uploadname), uploadname)

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Upload Successfully...' );", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Upload Fail...' );", True)

                End If

                '    If CheckImage() = False Then
                '        If File.Exists(Server.MapPath(txtImage.Text.Trim)) = True Then
                '            File.Delete(Server.MapPath(txtImage.Text.Trim))
                '        End If
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid image, image height is more.' );", True)
                '        Exit Sub
                '    Else
                '        SendImageToWebService(Server.MapPath("~/PricelistModule/UploadedImages/" + strFileName), strFileName)
                '    End If
                '    'ImgBanner.ImageUrl = txtImage.Text
                'Else
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select image file.' );", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Files are selected.' );", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region


#Region "Private Function ValidatePage() As Boolean"
    Private Function ValidatePage() As Boolean
        Dim strname As String = ""

        If txtAlternateText.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Alternate text Should not be blank.' );", True)
            ValidatePage = False
            Exit Function
        End If


        If lblImage.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please click on upload button to upload image.' );", True)
            ValidatePage = False
            Exit Function
        End If

        'If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentcontract", "alternatetext", CType(txtAlternateText.Text.Trim, String)) Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Alternate  already exists.');", True)
        '    ValidatePage = False
        '    Exit Function
        'End If



        'If ViewState("checkuploadname") <> lblImage.Text.Trim Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Upload alternatetext  has changed enter new name and upload first.');", True)
        '    ValidatePage = False
        '    Exit Function
        'End If
         
        'Dim dsval As DataSet
        'dsval = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select 't' from agentcontract ")
        'If dsval.Tables.Count > 0 Then
        '    If dsval.Tables(0).Rows.Count > 0 Then
        '        If IsDBNull(dsval.Tables(0).Rows(0)(0)) = False Then
        '            If dsval.Tables(0).Rows(0)(0).ToString() <> "" Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contract already exists Edit the existing contract.');", True)
        '                ValidatePage = False
        '                Exit Function
        '            End If
        '        End If
        '    End If
        'End If


        If ViewState("UploadContractState") = "Edit" Then
            Dim dsval As DataSet
            dsval = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select AlternateText from agentcontract ")
            If dsval.Tables.Count > 0 Then
                If dsval.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(dsval.Tables(0).Rows(0)(0)) = False Then
                        If dsval.Tables(0).Rows(0)(0).ToString() = CType(txtAlternateText.Text.Trim, String) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Change the alternate text name.');", True)
                            Exit Function
                        End If
                    End If
                End If
            End If
        End If

        ValidatePage = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UploadContractAdds','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Btnrmv_Click(sender As Object, e As System.EventArgs) Handles Btnrmv.Click
        lblImage.Text = ""
        txtAlternateText.Text = ""
    End Sub
End Class

