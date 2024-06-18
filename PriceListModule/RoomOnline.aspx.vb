
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports UploadService.UploadService
Imports System.IO
Imports System.Net
#End Region

Partial Class PriceListModule_RoomOnline
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objdatetime As New clsDateTime
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                'dpcontactDate.txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                Dim strtel As String = ""

                txtconnection.Value = Session("dbconnectionName")

                ViewState.Add("State", Request.QueryString("State"))
                ViewState.Add("partycode", Request.QueryString("partycode"))
                ViewState.Add("RefCode", Replace(Request.QueryString("RefCode"), "$", "+"))

                If Not Request.QueryString("partycode") = Nothing Then
                    txtCode.Value = Request.QueryString("partycode")
                Else
                    txtCode.Value = ""
                End If

                If Not Request.QueryString("partyname") = Nothing Then
                    txtName.Value = Request.QueryString("partyname")
                Else
                    txtName.Value = ""
                End If

                If ViewState("State") = "New" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlrmtypcode, "rmtypcode", "rmtypname", "select pm.rmtypcode,rm.rmtypname  from partyrmtyp  pm,rmtypmast rm,partymast p where pm.rmtypcode =rm.rmtypcode  and pm.partycode =p.partycode and pm.inactive =0  and pm.rmtypcode+pm.partycode not in (select rmtypcode+partycode from Party_Room_Details) and     pm.partycode='" & txtCode.Value & "' order by pm.rmtypcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlrmtypname, "rmtypname", "rmtypcode", "select rm.rmtypname,pm.rmtypcode  from partyrmtyp  pm,rmtypmast rm,partymast p where pm.rmtypcode =rm.rmtypcode  and pm.partycode =p.partycode and pm.inactive =0  and pm.rmtypcode+pm.partycode not in (select rmtypcode+partycode from Party_Room_Details)   and pm.partycode='" & txtCode.Value & "' order by rm.rmtypname", True)

                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlrmtypcode, "rmtypcode", "rmtypname", "select pm.rmtypcode,rm.rmtypname  from partyrmtyp  pm,rmtypmast rm,partymast p where pm.rmtypcode =rm.rmtypcode  and pm.partycode =p.partycode and pm.inactive =0 and pm.rmtypcode+pm.partycode in (select rmtypcode+partycode from Party_Room_Details) and    pm.partycode='" & txtCode.Value & "' order by pm.rmtypcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlrmtypname, "rmtypname", "rmtypcode", "select rm.rmtypname,pm.rmtypcode  from partyrmtyp  pm,rmtypmast rm,partymast p where pm.rmtypcode =rm.rmtypcode  and pm.partycode =p.partycode and pm.inactive =0    and pm.partycode='" & txtCode.Value & "' order by rm.rmtypname", True)

                End If




                If ViewState("State") = "New" Then

                    SetFocus(txtCode)
                    lblHeading.Text = "Add Rooms Online"
                    btnSave.Text = "Save"

                    DisableControl()
                    'ShowRecord(CType(ViewState("RefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("State") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Update Rooms Online"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("RefCode"), String))

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update currency?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("State") = "Delete" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "Delete Rooms Online"
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("RefCode"), String))

                ElseIf ViewState("State") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Rooms Online"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("RefCode"), String))
                End If


                btnViewImg1.Attributes.Add("onclick", "return PopUpImageView('" & lblimage1.Text & "')")
                btnViewImg2.Attributes.Add("onclick", "return PopUpImageView('" & lblimage2.Text & "')")
                btnViewImg3.Attributes.Add("onclick", "return PopUpImageView('" & lblimage3.Text & "')")
                btnViewImg4.Attributes.Add("onclick", "return PopUpImageView('" & lblimage4.Text & "')")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RoomOnline.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("State") = "View" Then

            txtdescription.ReadOnly = True
            'txtfacilit.ReadOnly = True
            ddlrmtypcode.Disabled = True
            ddlrmtypname.Disabled = True
            '  txtaction.ReadOnly = True
            btnSave.Visible = False
        ElseIf ViewState("State") = "Edit" Then
            ddlrmtypcode.Disabled = True
            ddlrmtypname.Disabled = True
        ElseIf ViewState("State") = "Delete" Then
            txtdescription.ReadOnly = True
            'txtfacilit.ReadOnly = True
            ddlrmtypcode.Disabled = True
            ddlrmtypname.Disabled = True
        End If

    End Sub

#End Region

#Region "Public Function ValidatePage() As Boolean"
    Public Function ValidatePage() As Boolean
        Try
            If ViewState("State") = "New" Then
                strSqlQry = "select * from Party_Room_Details where rmtypcode='" & CType(ddlrmtypcode.Items(ddlrmtypcode.SelectedIndex).Text, String) & "' and partycode = '" & txtCode.Value & "'"

                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Room Type Already Added');", True)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            End If
            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("RoomOnline.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

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
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            Dim strpath1 As String = ""
            Dim strpath2 As String = ""
            Dim strpath3 As String = ""
            Dim strpath4 As String = ""
            Dim strpath_logo1 As String = ""
            Dim strpath_logo2 As String = ""
            Dim strpath_logo3 As String = ""
            Dim strpath_logo4 As String = ""

            If Page.IsValid = True Then
                If ValidatePage() = False Then
                    Exit Sub
                End If

                If FileUpload1.FileName <> "" Then
                    strpath_logo1 = FileUpload1.FileName
                    strpath_logo1 = txtCode.Value & "_" & strpath_logo1
                    strpath1 = Server.MapPath("UploadedImages\Roomimages\" & strpath_logo1)
                    FileUpload1.PostedFile.SaveAs(strpath1)
                    txtimg1.Text = strpath_logo1
                    lblimage1.Text = strpath_logo1
                    lblimage1.Visible = True
                    hdnFileName.Text = strpath_logo1
                    ' SendImageToWebService(strpath1, strpath_logo1)
                End If
                If FileUpload2.FileName <> "" Then
                    strpath_logo2 = FileUpload2.FileName
                    strpath_logo2 = txtCode.Value & "_" & strpath_logo2
                    strpath2 = Server.MapPath("UploadedImages\Roomimages\" & strpath_logo2)
                    FileUpload2.PostedFile.SaveAs(strpath2)
                    txtimg2.Text = strpath_logo2
                    lblimage2.Text = strpath_logo2
                    lblimage2.Visible = True
                    hdnFileName.Text = strpath_logo2
                    '   SendImageToWebService(strpath2, strpath_logo2)
                End If
                If FileUpload3.FileName <> "" Then
                    strpath_logo3 = FileUpload3.FileName
                    strpath_logo3 = txtCode.Value & "_" & strpath_logo3
                    strpath3 = Server.MapPath("UploadedImages\Roomimages\" & strpath_logo3)
                    FileUpload3.PostedFile.SaveAs(strpath3)
                    txtimg3.Text = strpath_logo3
                    lblimage3.Text = strpath_logo3
                    lblimage3.Visible = True
                    hdnFileName.Text = strpath_logo3
                    'SendImageToWebService(strpath3, strpath_logo3)
                End If
                If FileUpload4.FileName <> "" Then
                    strpath_logo4 = FileUpload4.FileName
                    strpath_logo4 = txtCode.Value & "_" & strpath_logo4
                    strpath4 = Server.MapPath("UploadedImages\Roomimages\" & strpath_logo4)
                    FileUpload4.PostedFile.SaveAs(strpath4)
                    txtimg4.Text = strpath_logo4
                    lblimage4.Text = strpath_logo4
                    lblimage4.Visible = True
                    hdnFileName.Text = strpath_logo4
                    ' SendImageToWebService(strpath4, strpath_logo4)
                End If

                Dim regno As String = ""
                If ViewState("State") = "New" Or ViewState("State") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("State") = "New" Then
                        'regno = objUtils.GetAutoDocNo("VISITOR", mySqlConn, sqlTrans)

                        mySqlCmd = New SqlCommand("sp_add_partyrmtyp_online", mySqlConn, sqlTrans)
                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_partyrmtyp_online", mySqlConn, sqlTrans)
                        regno = txtCode.Value
                    End If
                    txtdescription.Text = Replace(txtdescription.Text, "'", "")
                    'txtfacilit.Text = Replace(txtfacilit.Text, "'", "")

                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(ddlrmtypcode.Items(ddlrmtypcode.SelectedIndex).Text, String)

                    If txtdescription.Text.Trim = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@description", SqlDbType.VarChar, 8000)).Value = CType(txtdescription.Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@description", SqlDbType.VarChar, 8000)).Value = DBNull.Value
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@facilities", SqlDbType.VarChar, 8000)).Value = ""

                    mySqlCmd.Parameters.Add(New SqlParameter("@img1", SqlDbType.VarChar, 100)).Value = CType(txtimg1.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@img2", SqlDbType.VarChar, 100)).Value = CType(txtimg2.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@img3", SqlDbType.VarChar, 100)).Value = CType(txtimg3.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@img4", SqlDbType.VarChar, 100)).Value = CType(txtimg4.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@roomstar", SqlDbType.Int)).Value = Val(ddlStarNo.SelectedValue)

                    mySqlCmd.ExecuteNonQuery()
                ElseIf ViewState("State") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp_online", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(ddlrmtypcode.Items(ddlrmtypcode.SelectedIndex).Text, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CurrenciesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('RoomOnlineWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomOnline.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            strSqlQry = "select partycode,roomdescription,roomfacilities,rmtypcode,roomimage1,roomimage2,roomimage3,roomimage4,roomstar from Party_Room_Details  where partycode= '" & txtCode.Value.Trim & "' and rmtypcode='" & CType(RefCode, String) & "'"


            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If ViewState("State") = "View" Or ViewState("State") = "Edit" Or ViewState("State") = "Delete" Then




                        If IsDBNull(mySqlReader("roomdescription")) = False Then
                            Me.txtdescription.Text = CType(mySqlReader("roomdescription"), String)
                        Else
                            Me.txtdescription.Text = ""
                        End If

                        If IsDBNull(mySqlReader("roomstar")) = False Then
                            ddlStarNo.SelectedValue = mySqlReader("roomstar")
                        End If

                        If IsDBNull(mySqlReader("rmtypcode")) = False Then
                            Me.ddlrmtypcode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "rmtypmast", "rmtypname", "rmtypcode", CType(mySqlReader("rmtypcode"), String))
                            Me.ddlrmtypname.Value = CType(mySqlReader("rmtypcode"), String)


                        End If

                        If IsDBNull(mySqlReader("roomimage1")) = False Then
                            txtimg1.Text = mySqlReader("roomimage1")
                            lblimage1.Text = mySqlReader("roomimage1")

                            lblimage1.Visible = True


                        End If
                        If IsDBNull(mySqlReader("roomimage2")) = False Then
                            txtimg2.Text = mySqlReader("roomimage2")
                            lblimage2.Text = mySqlReader("roomimage2")
                            lblimage2.Visible = True
                        End If

                        If IsDBNull(mySqlReader("roomimage3")) = False Then
                            txtimg3.Text = mySqlReader("roomimage3")
                            lblimage3.Text = mySqlReader("roomimage3")
                            lblimage3.Visible = True
                        End If

                        If IsDBNull(mySqlReader("roomimage4")) = False Then
                            txtimg4.Text = mySqlReader("roomimage4")
                            lblimage4.Text = mySqlReader("roomimage4")
                            lblimage4.Visible = True
                        End If


                    End If


                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomOnline.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CurrenciesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region


    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ' Response.Write("<script language='javascript'> nw=window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=54,left=760,width=250,height=600'); </script>")
        '' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Btnrmv1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblimage1.Text = ""
        txtimg1.Text = ""
    End Sub

    Protected Sub Btnrmv2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblimage2.Text = ""
        txtimg2.Text = ""

    End Sub

    Protected Sub btnrmv3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblimage3.Text = ""
        txtimg3.Text = ""
    End Sub

    Protected Sub btnrmv4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblimage4.Text = ""
        txtimg4.Text = ""
    End Sub
End Class

