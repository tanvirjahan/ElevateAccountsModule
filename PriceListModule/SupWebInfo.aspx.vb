#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports UploadService.UploadService
Imports System.IO
Imports System.Net
Imports System.Web.UI

Imports System.Collections.Generic

#End Region

Partial Class SupWebInfo
    Inherits System.Web.UI.Page
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim MyDS As New DataSet
    Dim MyAdapter As SqlDataAdapter
    Dim GvRow As GridViewRow
    Dim strtypecode As String
#End Region


    '*** Danny 11/03/18>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    Protected Sub BtnSaveInfoWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim strpath_logo As String = ""

        Dim strpath As String = ""

        Dim flag As Boolean = False
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Dim lblHotelType As Label
        Dim lblameniti As Label

        Dim strpath_logo6 As String = ""
        Dim strpath6 As String = ""
        Dim strpath_logo7 As String = ""
        Dim strpath7 As String = ""
        Try



            If FileUpload.FileName <> "" Then

                strpath_logo = txtCode.Value & "_" & FileUpload.FileName(3)
                strpath = Server.MapPath("UploadedImages\" & strpath_logo)
                FileUpload.PostedFile.SaveAs(strpath)
                '  txtimg7.Text = strpath_logo7
                lblimage.Text = strpath_logo
                lblimage.Visible = True
                hdnFileName.Text = strpath_logo
            Else
                '   txtimg.Value = IIf(txtimg.Value = "", FileUpload.FileName, txtimg.Value)
                'SendImageToWebService(strpath1, strpath_logo7)
            End If




            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("SupState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_partywebinfo", mySqlConn, sqlTrans)
                    ElseIf Session("SupState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_partywebinfo", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 1000)).Value = CType(lblimage.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@hoteltext", SqlDbType.VarChar, 1000)).Value = CType(txtHOteltxt.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@latitude", SqlDbType.Decimal)).Value = Val(txtLatitude.Text)
                    mySqlCmd.Parameters.Add(New SqlParameter("@longitude", SqlDbType.Decimal)).Value = Val(txtLongitude.Text)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    'SQL  Trans start
                    If FileUpload2.FileName <> "" Then
                        UploadOtherImages(Request.Files)
                    End If
                ElseIf Session("SupState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_invtpe", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.VarChar, 10)).Value = ""
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("SupState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("SupState") = "Delete" Then
                    'Response.Redirect("SupplierSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("SupWebInfo.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try




    End Sub
    Private Sub UploadOtherImages(ByVal htf As HttpFileCollection)

        Dim AtcFileCnt As Integer = htf.Count - 1


        If AtcFileCnt > 5 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You have selected more than 5 Images.' );", True)
            Exit Sub
        End If


        If Not Directory.Exists(Server.MapPath("UploadedImages/HotelImage/" + CType(txtCode.Value.Trim, String) + "/")) Then
            Directory.CreateDirectory(Server.MapPath("UploadedImages/HotelImage/" + CType(txtCode.Value.Trim, String) + "/"))
        End If

        Dim fileEntries As String() = Directory.GetFiles(Server.MapPath("UploadedImages/HotelImage/" + CType(txtCode.Value.Trim, String) + "/"))
        If (5 - fileEntries.Length) < AtcFileCnt Then
            Dim s As String = CType(Val(5 - fileEntries.Length).ToString(), String)
            If s = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You already selected your maximum limit of images. Delete any one and upload the new image." + s + " Images.' );", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You can select only " + s + " Images.' );", True)
            End If

            Exit Sub
        End If

        For i As Integer = 1 To 5
            If AtcFileCnt > 0 Then
                Dim AttachedFile As HttpPostedFile = htf(AtcFileCnt)
filenamechanged:
                Dim SaveFile As String = Server.MapPath("UploadedImages/HotelImage/" + CType(txtCode.Value.Trim, String) + "/" + i.ToString + ".jpg")
                If File.Exists(SaveFile) Then
                    i = i + 1
                    GoTo filenamechanged
                End If

                AttachedFile.SaveAs(SaveFile)
                AtcFileCnt = AtcFileCnt - 1
            End If
        Next
        LoadOtherImages()
        'If (FileUpload2.HasFile) Then
        '    For Each uploadedFile As HttpPostedFile In Request.Files
        '        Dim s As String = uploadedFile.FileName
        '    Next
        '    'foreach (HttpPostedFile uploadedFile in UploadImages.PostedFiles)
        '    '{
        '    '    uploadedFile.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Images/"),
        '    '    uploadedFile.FileName)); listofuploadedfiles.Text += String.Format("{0}<br />", uploadedFile.FileName);
        '    '}

        '    'For Each uploadedFile As HttpPostedFile In FileUpload2.PostedFile

        '    'Next

        'End If



        '        If FileUpload2.PostedFiles.Count > 0 Then

        '            If FileUpload2.PostedFiles.Count > 5 Then
        '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You have selected more than 5 Images.' );", True)
        '                Exit Sub
        '            End If

        '            Dim fileEntries As String() = Directory.GetFiles(Server.MapPath("UploadedImages/HotelImage/" + CType(txtCode.Value.Trim, String) + "/"))
        '            If (5 - fileEntries.Rank) < FileUpload2.PostedFiles.Count Then
        '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('you can select only " + CType(Val(5 - fileEntries.Rank).ToString(), String) + ".' );", True)
        '                Exit Sub
        '            End If
        '            For i As Integer = 0 To FileUpload2.PostedFiles.Count - 1
        'filenamechanged:
        '                Dim SaveFile As String = Server.MapPath("UploadedImages/HotelImage/" + CType(txtCode.Value.Trim, String) + "/" + i.ToString + ".jpg")
        '                If File.Exists(SaveFile) Then
        '                    i = i + 1
        '                    GoTo filenamechanged
        '                End If
        '                Dim AttachedFile As HttpPostedFile = FileUpload2.PostedFiles(i)

        '                AttachedFile.SaveAs(SaveFile)


        '            Next



        'Else
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You have not selected any Other Images.' );", True)
        'End If

    End Sub
    Protected Sub RepeaterImages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RepeaterImages.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim aimgBRemoveOImage As ImageButton = e.Item.FindControl("imgBRemoveOImage")
            aimgBRemoveOImage.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Delete this image permenantly?')==false)return false;")

        End If

    End Sub
    Protected Sub imgBRemoveOImage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim ss As ImageButton = sender
            Dim a As String = Server.MapPath(ss.CommandArgument)


            If File.Exists(Server.MapPath(ss.CommandArgument)) Then
                File.Delete(Server.MapPath(ss.CommandArgument))
            End If
            LoadOtherImages()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupWebInfo.aspx=>imgBRemoveOImage_Click", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Private Sub LoadOtherImages()
        Try
            Dim fileEntries As String = "../PriceListModule/UploadedImages/HotelImage/" + CType(txtCode.Value.Trim, String) + "/"
            If Not Directory.Exists(Path.GetDirectoryName(Server.MapPath(fileEntries))) Then
                Directory.CreateDirectory(Path.GetDirectoryName(Server.MapPath(fileEntries)))
            End If
            Dim filesindirectory As String() = Directory.GetFiles(Server.MapPath(fileEntries))
            Dim images As List(Of String) = New List(Of String)(filesindirectory.Length)

            Dim i As Integer = 5
            For Each item As String In filesindirectory
                images.Add(String.Format(fileEntries + "{0}", System.IO.Path.GetFileName(item)))

                i = i - 1
                If i = 0 Then
                    Exit For
                End If
            Next
            RepeaterImages.DataSource = images
            RepeaterImages.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupWebInfo.aspx=>LoadOtherImages()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    '*** Danny 11/03/18<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        'PanelInfoForWEb.Visible = True
        NumbersForTextbox(txtLongitude)
        NumbersForTextbox(txtLatitude)
        Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
        Dim sptype As String
        sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" + CType(Session("SupRefCode"), String) + "'")
        If CType(Request.QueryString("appid"), String) = "1" Then
            Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)
        ElseIf CType(Request.QueryString("appid"), String) = "11" Then
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
        ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
        End If

        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
        Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

        If IsPostBack = False Then

            If CType(Session("SupState"), String) = "New" Or CType(Session("SupState"), String) = "Edit" Then
                If Not Session("SupRefCode") = Nothing Then

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode='" + sptype + "' order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" + sptype + "' order by partyname", True)

                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1  order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1  order by partyname", True)
                End If
            End If


            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                Exit Sub
            End If

            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                lblHeading.Text = "Add New Supplier"
                Page.Title = Page.Title + " " + "New Supplier Master"
                BtnSaveInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnSaveInfoWeb.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ' FillType_Amenities()
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtCode)
                lblHeading.Text = "Edit Supplier"
                Page.Title = Page.Title + " " + "Edit Supplier Master"
                BtnSaveInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")

            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                '  FillType_Amenities()
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                lblHeading.Text = "View Supplier"
                Page.Title = Page.Title + " " + "View Supplier Master"
                BtnSaveInfoWeb.Visible = False
                BtnSaveInfoWeb.Text = "Return to Search"

            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ' FillType_Amenities()
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True

                lblHeading.Text = "Delete Supplier"
                Page.Title = Page.Title + " " + "Delete Supplier Master"
                BtnSaveInfoWeb.Text = "Delete"
                BtnSaveInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
            End If
            BtnCancelInfoWeb.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")


            LoadOtherImages() '*** Danny 11/03/18

            ShowRecord(Session("SupRefCode"))
            FillInfoWebDisplay()
            DisplayInfoForWEB()
            'SetFocus(txtRooms)
            ''     ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtRooms.ClientID + "');", True)


            btnViewimage.Attributes.Add("onclick", "return PopUpImageView('" & lblimage.Text & "')")
            'Btnrmv7.Attributes.Add("onclick", "return PopUpImageView('" & lblimage.Text & "')")

        End If

        Dim typ As Type
        typ = GetType(DropDownList)
        Me.whotelatbcontrol.partyval = txtCode.Value
        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
            ddlSuppierCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSuppierNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If

        Session.Add("submenuuser", "SupplierSearch.aspx")
        Me.SubMenuUserControl1.suptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", txtCode.Value)

    End Sub

    Private Sub DisplayInfoForWEB()
        'Dim chk As HtmlInputCheckBox
        Try

            strSqlQry = "select * from partyinfo where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = True Then


                If IsDBNull(mySqlReader("hotelOverview")) = False Then
                    txtHOteltxt.Text = mySqlReader("hotelOverview")
                End If

                If IsDBNull(mySqlReader("latitude")) = False Then
                    txtLatitude.Text = mySqlReader("latitude")
                End If

                If IsDBNull(mySqlReader("longitude")) = False Then
                    txtLongitude.Text = mySqlReader("longitude")
                End If







                If IsDBNull(mySqlReader("thumbimage")) = False Then
                    ' txtimg7.Text = mySqlReader("thumbimage")
                    lblimage.Text = mySqlReader("thumbimage")
                    lblimage.Visible = True
                End If





            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

    End Sub

#End Region


    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")
    End Sub

    Public Sub NumbersForTextbox(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")
    End Sub

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from partymast Where partycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        Me.txtCode.Value = mySqlReader("partycode")
                    End If
                    If IsDBNull(mySqlReader("partyname")) = False Then
                        Me.txtName.Value = mySqlReader("partyname")
                    End If
                    mySqlCmd.Dispose()
                    mySqlReader.Close()
                End If
            End If

            mySqlCmd.Dispose()
            mySqlReader.Close()
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from party_webinfo Where partycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("hoteltext")) = False Then
                        Me.txtHOteltxt.Text = CType(mySqlReader("hoteltext"), String)
                    Else
                        Me.txtHOteltxt.Text = ""
                    End If


                    If IsDBNull(mySqlReader("imagename")) = False Then
                        btnViewimage.Visible = True
                        Me.lblimage.Text = mySqlReader("imagename")
                    Else
                        Me.lblimage.Text = ""
                        btnViewimage.Visible = False
                    End If

                    If IsDBNull(mySqlReader("latitude")) = False Then
                        Me.txtLatitude.Text = CType(mySqlReader("latitude"), String)
                    Else
                        Me.txtLatitude.Text = ""
                    End If

                    If IsDBNull(mySqlReader("longitude")) = False Then
                        Me.txtLongitude.Text = CType(mySqlReader("longitude"), String)
                    Else
                        Me.txtLongitude.Text = ""
                    End If
                End If

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupWebInfo.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()

        End Try
    End Sub
#End Region


#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "blocksale_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a BlockFullOfSales, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cancel_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a CancellationPolicy, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "child_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a ChildPolicy, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compulsory_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a CompulsoryRemarks, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplistdwknew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Promotions, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a PriceListConversion, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromotion_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a EarliBirdPromotion, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "flightmast", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a FlightMaster, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "hotels_construction", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a HotelConstruction, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "minnights_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MinimumNights, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "party_splevents", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SpecialEvents/Extras For Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyallot", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SupplierAllotment, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyinfo", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Supplier Information, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast_mulltiemail", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MultiEmail of Suppliers, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymaxaccomodation", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MaximumAccomodation Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothcat", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherService Category OF Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothgrp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherServiceGroup Of Supplier , cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothtyp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherServiceTypes of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyrmcat", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a RoomCategory Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyrmtyp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a RoomType Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Promotions, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellsph", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SellingFormulaForSupplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SupplierPolicy, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplistd", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Details Of SpecialEvents/Extras, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SpecialEvents/Extras, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymeal", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Meal Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region

    Private Sub FillInfoWebDisplay()
        Try
            Dim dt As DataTable
            Dim dr As DataRow

            dt = New DataTable
            dt.Columns.Add(New DataColumn("Desc", GetType(String)))
            dr = dt.NewRow()
            dr(0) = "In House Doctor"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Health Club"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Swimming Pool"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Shuttle Service"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Ball Room"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Water Sports"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Squash"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Spa Pool"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Children Pool"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Business Center"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Ayurvedic"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Bar"
            dt.Rows.Add(dr)
            dr = dt.NewRow()
            dr(0) = "Pub"
            dt.Rows.Add(dr)
            'return a DataView to the DataTable
            'Gv_InfoForWeb.DataSource = dt
            'Gv_InfoForWeb.DataBind()
            'Gv_InfoForWeb.Visible = False
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
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
        FS.Close()
    End Sub

 
    Protected Sub BtnCancelInfoWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancelInfoWeb.Click
        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#Region "Function deleteuploadimage()"
    Private Function deleteuploadimage()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select mainimage ,subimage1,subimage2,subimage3,subimage4 from partyinfo Where partycode='" & txtCode.Value.Trim & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("mainimage")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("mainimage"))
                    End If
                    If IsDBNull(mySqlReader("subimage1")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\\" + mySqlReader("subimage1"))
                    End If
                    If IsDBNull(mySqlReader("subimage2")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("subimage2"))
                    End If
                    If IsDBNull(mySqlReader("subimage3")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("subimage3"))
                    End If
                    If IsDBNull(mySqlReader("subimage4")) = False Then
                        objUtils.DeleteFile(Server.MapPath(".") + "\UploadedImages\" + mySqlReader("subimage4"))
                    End If
                End If
            End If
            'FileUpload1.PostedFile.SaveAs(Server.MapPath(".") + "//UploadImage/" + strFileName)
            'txtImg.Text = FileUploa
            Return True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupWebInfo.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region
    Protected Sub BtnimgUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim strpath_logo As String = ""
            Dim strpath_logo1 As String = ""
            Dim strpath_logo2 As String = ""
            Dim strpath_logo3 As String = ""
            Dim strpath_logo4 As String = ""
            Dim strpath As String = ""
            Dim strpath1 As String = ""
            Dim strpath2 As String = ""
            Dim strpath3 As String = ""
            Dim strpath4 As String = ""

            'If FileUpload5.FileName <> "" Then
            '    strpath_logo = FileUpload5.FileName
            '    strpath_logo = txtCode.Value & "_" & strpath_logo
            '    strpath = Server.MapPath("UploadedImages\" & strpath_logo)
            '    FileUpload5.PostedFile.SaveAs(strpath)
            '    txtimg5.Text = strpath_logo
            'End If
            'If FileUpload1.FileName <> "" Then
            '    strpath_logo1 = FileUpload1.FileName
            '    strpath_logo1 = txtCode.Value & "_" & strpath_logo1
            '    strpath1 = Server.MapPath("UploadedImages\" & strpath_logo1)
            '    FileUpload1.PostedFile.SaveAs(strpath1)
            '    txtimg1.Text = strpath_logo1
            'End If
            'If FileUpload2.FileName <> "" Then
            '    strpath_logo2 = FileUpload2.FileName
            '    strpath_logo2 = txtCode.Value & "_" & strpath_logo2
            '    strpath2 = Server.MapPath("UploadedImages\" & strpath_logo2)
            '    FileUpload2.PostedFile.SaveAs(strpath2)
            '    txtimg2.Text = strpath_logo2
            'End If
            'If FileUpload3.FileName <> "" Then
            '    strpath_logo3 = FileUpload3.FileName
            '    strpath_logo3 = txtCode.Value & "_" & strpath_logo3
            '    strpath3 = Server.MapPath("UploadedImages\" & strpath_logo3)
            '    FileUpload3.PostedFile.SaveAs(strpath3)
            '    txtimg3.Text = strpath_logo3
            'End If
            'If FileUpload4.FileName <> "" Then
            '    strpath_logo4 = FileUpload4.FileName
            '    strpath_logo4 = txtCode.Value & "_" & strpath_logo4
            '    strpath4 = Server.MapPath("UploadedImages\" & strpath_logo4)
            '    FileUpload4.PostedFile.SaveAs(strpath4)
            '    txtimg4.Text = strpath_logo4
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


    End Sub





    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupWebInfo','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



    Protected Sub btnfilldetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilldetail.Click

        ShowRecordFilldetail()

    End Sub
    Private Sub ShowRecordFilldetail()
        Dim chk As HtmlInputCheckBox
        Dim strpath, strpath_logo As String
        Try
            If ddlSuppierCD.Value <> "[Select]" Then
                'strSqlQry = "select * from partyinfo where partycode='" & ddlSuppierNM.Value & "'"
                strSqlQry = "select * from party_webinfo where partycode='" & ddlSuppierNM.Value & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader
                If mySqlReader.Read = True Then
                    If IsDBNull(mySqlReader("imagename")) = False Then

                        strpath_logo = txtCode.Value & "_" & CType(mySqlReader("imagename"), String)
                        strpath = Server.MapPath("UploadedImages\" & strpath_logo)
                        ' FileUpload.PostedFile.SaveAs(strpath)

                        '  txtimg7.Text = strpath_logo7
                        lblimage.Text = strpath_logo
                        lblimage.Visible = True
                        hdnFileName.Text = strpath_logo
                    End If

                    If IsDBNull(mySqlReader("hoteltext")) = False Then
                        txtHOteltxt.Text = CType(mySqlReader("hoteltext"), String)
                    End If

                    If IsDBNull(mySqlReader("latitude")) = False Then
                        txtLatitude.Text = CType(mySqlReader("latitude"), String)
                    End If


                    If IsDBNull(mySqlReader("longitude")) = False Then
                        txtLongitude.Text = CType(mySqlReader("longitude"), String)
                    End If






                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

    End Sub



End Class
