




#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region
Partial Class UploadPopularDeals
    Inherits System.Web.UI.Page



#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ValidImageSize As Boolean = True
#End Region
             
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      

        If Page.IsPostBack = False Then
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                btnViewimage.Visible = True
                Btnremove.Visible = True
                ViewState.Add("PopularDealState", Request.QueryString("State"))
                ViewState.Add("PopularDealRefCode", Request.QueryString("RefCode"))
                txtCode.Attributes.Add("readonly", "readonly")
                If ViewState("PopularDealState") = "New" Then
                    SetFocus(txtCode)
                    ChkInactive.Checked = False
                    Page.Title = " Add New Popular Deal"
                    lblHeading.Text = "Add New Popular Deal"
                    btnSave.Text = "Save"
                    btnViewimage.Visible = False
                    Btnremove.Visible = False
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    '  btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                ElseIf ViewState("PopularDealState") = "Edit" Then
                    '  SetFocus(txtName)
                    Page.Title = " Edit Popular Deal"
                    lblHeading.Text = "Edit Popular Deal"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("PopularDealRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("PopularDealState") = "View" Then
                    SetFocus(btnCancel)
                    Page.Title = " View Popular Deal"
                    lblHeading.Text = "View Popular Deal"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("PopularDealRefCode"), String))
                ElseIf ViewState("PopularDealState") = "Delete" Then
                    SetFocus(btnSave)
                    Page.Title = " Delete Popular Deal"
                    lblHeading.Text = "Delete Popular Deal"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("PopularDealRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                btnViewimage.Attributes.Add("onclick", "return PopUpImageView('" & txtimg.Value & "')")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("UploadPopularDeals.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub

#End Region


#Region "charcters1"
    Public Sub charcters1(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("PopularDealState") = "View" Or ViewState("PopularDealState") = "Delete" Then
            txtCode.Disabled = True
            '    hotelimage.Disabled = True
            '  txtOthGrpname.Enabled = False
            txtPrefSupName.Enabled = False

            txtRemark.ReadOnly = True
            hotelimage.Enabled = False
            btnViewimage.Enabled = True
            Btnremove.Enabled = False
            ChkInactive.Disabled = True

            txtRemark.Enabled = False
        ElseIf ViewState("PopularDealState") = "Edit" Then
            txtCode.Disabled = True

        End If

    End Sub

#End Region

#Region " Validate Page"

    Public Function checkforexisting() As Boolean
        If ViewState("PopularDealState") = "New" Then
            'Dim strfilter As String = " active=1 "
            If txtPrefSupCode.Text <> "" Or txtPrefSupName.Text <> "" Then
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "tblPopularDeal", "partycode", txtPrefSupCode.Text.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier  Already Exists.');", True)
                    SetFocus(txtPrefSupName)
                    checkforexisting = False
                    Exit Function
                End If
            End If

            'Dim checkstring As String = objUtils.CheckString(Session("dbconnectionName"), "select top 1 dealid from tblpopulardeal where active=1")
            'If checkstring <> "" Then
            '    If ChkInactive.Checked Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Active Deal Already Exists');", True)
            '        SetFocus(ChkInactive)
            '        checkforexisting = False
            '        Exit Function
            '    End If
            'ElseIf checkstring = "" Then
            '    If ChkInactive.Checked = False Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast One Deal Should be Active');", True)
            '        SetFocus(ChkInactive)
            '        checkforexisting = False
            '        Exit Function
            '    End If
            'End If
        End If


        If ViewState("PopularDealState") = "Edit" Then

            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "tblPopularDeal", "dealid", "partycode", txtPrefSupCode.Text.Trim, txtCode.Value) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier  Already Exists.');", True)
                SetFocus(txtPrefSupName)
                checkforexisting = False
                Exit Function
            End If
            If ChkInactive.Checked Then
                If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "tblPopularDeal", "dealid", "active", "1", txtCode.Value) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Active Deal Already Exists.');", True)
                    SetFocus(txtPrefSupName)
                    checkforexisting = False
                    Exit Function
                End If
          

            End If

        End If
        checkforexisting = True
    End Function
    Public Function ValidatePage() As Boolean
        Try

            If hotelimage.FileName <> "" Then
                If System.Drawing.Image.FromStream(hotelimage.PostedFile.InputStream).Height <> 500 Or System.Drawing.Image.FromStream(hotelimage.PostedFile.InputStream).Width <> 1920 Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select an Image With  (1920px X 500px) Dimensions');", True)
                    SetFocus(txtimg)
                    ValidImageSize = False
                    ValidatePage = False

                    Exit Function
                End If
            End If

        

                If hotelimage.FileName.Trim = "" And txtimg.Value.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Image.');", True)
                    SetFocus(txtimg)
                    ValidatePage = False
                    Exit Function
                End If
                If txtPrefSupCode.Text.Trim = "" Or txtPrefSupName.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Supplier.');", True)
                    SetFocus(txtPrefSupName)
                    ValidatePage = False
                    Exit Function
                End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
        ValidatePage = True
    End Function
#End Region

    Private Sub SaveImage(strpath_logo1 As String)
        Dim strpath1 As String
        strpath1 = Server.MapPath("UploadHomeImage/" & strpath_logo1)
        hotelimage.PostedFile.SaveAs(strpath1)
        txtimg.Value = strpath_logo1
        hdnFileName.Text = txtimg.Value
    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strpath_logo1 As String
        Try
            If Page.IsValid = True Then
                If ViewState("PopularDealState") = "New" Or ViewState("PopularDealState") = "Edit" Then



                    If ValidatePage() = False Then
                        If hotelimage.FileName <> "" And ValidImageSize Then
                            If txtCode.Value <> "" Then
                                txtimg.Value = txtCode.Value & "_" & hotelimage.FileName
                            Else
                                txtimg.Value = hotelimage.FileName
                            End If
                        End If
                        Exit Sub
                    End If

                    If checkforexisting() = False Then
                        If hotelimage.FileName <> "" And ValidImageSize Then
                            If txtCode.Value <> "" Then
                                txtimg.Value = txtCode.Value & "_" & hotelimage.FileName
                            Else
                                txtimg.Value = hotelimage.FileName
                            End If
                        End If
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start


                    If ViewState("PopularDealState") = "New" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("POPDEAL", mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_tblPopularDeal", mySqlConn, sqlTrans)
                    ElseIf ViewState("PopularDealState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_tblPopularDeal", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure


                    If hotelimage.FileName <> "" Then
                        strpath_logo1 = txtCode.Value & "_" & hotelimage.FileName
                        SaveImage(strpath_logo1)
                    ElseIf ViewState("PopularDealState") = "New" And txtimg.Value <> "" Then
                        strpath_logo1 = txtCode.Value & "_" + txtimg.Value
                        SaveImage(strpath_logo1)
                    Else
                        txtimg.Value = IIf(txtimg.Value = "", hotelimage.FileName, txtimg.Value)
                    End If





                    mySqlCmd.Parameters.Add(New SqlParameter("@dealid", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtPrefSupCode.Text.Trim, String)

                    'If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@dealstartdate", SqlDbType.DateTime)).Value = txtfromdate.Text 'CType(Format(txtfromdate.Text, "yyyy/MM/dd"), DateTime)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@dealenddate", SqlDbType.DateTime)).Value = txttodate.Text 'CType(Format(txttodate.Text, "yyyy/MM/dd"), DateTime)
                    'Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@dealstartdate", SqlDbType.DateTime)).Value = DBNull.Value 'CType(Format(txtfromdate.Text, "yyyy/MM/dd"), DateTime)
                    mySqlCmd.Parameters.Add(New SqlParameter("@dealenddate", SqlDbType.DateTime)).Value = DBNull.Value 'CType(Format(txttodate.Text, "yyyy/MM/dd"), DateTime)
                    'End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@hotelimage", SqlDbType.VarChar, 100)).Value = CType(txtimg.Value, String)
                    If CType(txtRemark.Text, String) <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = ""
                    End If

                    If ChkInactive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf ChkInactive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)



                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("PopularDealState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_tblPopularDeal", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@dealid", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                Dim strscript As String = ""
                '  Response.Redirect("PopularDealsSearch.aspx")
                strscript = "window.opener.__doPostBack('PopularDealsWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("UploadPopularDeals.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from tblPopularDeal Where dealid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("dealid")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("dealid"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
   
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        txtPrefSupCode.Text = CType(mySqlReader("partycode"), String)
                        txtPrefSupName.Text = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", CType(mySqlReader("partycode"), String)), String)
                    Else
                        txtPrefSupCode.Text = " "
                        txtPrefSupName.Text = " "
                    End If
           
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Text = mySqlReader("remarks")
                    Else
                        Me.txtRemark.Text = ""
                    End If
             

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            ChkInactive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            ChkInactive.Checked = False
                        End If
                    End If



                    If IsDBNull(mySqlReader("hotelimage")) = False Then
                        hdnFileName.Text = mySqlReader("hotelimage")
                        txtimg.Value = mySqlReader("hotelimage")
                    Else
                        txtimg.Value = ""
                    End If




                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UploadPopularDeals.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetPreferSupplist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotelimages As New List(Of String)
        Try

            strSqlQry = "select partyname,partycode from partymast where active=1  and  sptypecode=(select option_selected  from reservation_parameters  where param_id=458 ) and showinweb=1 and    partyname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
                ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    hotelimages.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                        'hotelimages.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
                End If

            Return hotelimages
        Catch ex As Exception
            Return hotelimages
        End Try

    End Function

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("OtherServiceTypesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Private Function finddateperiod() As Boolean
        'Dim parms As New List(Of SqlParameter)
        'Dim parm(4) As SqlParameter
        'Dim ds As New DataSet

        'parm(0) = New SqlParameter("@dealid", CType(txtCode.Value, String))
        'parm(1) = New SqlParameter("@partycode", CType(txtPrefSupCode.Text, String))
        'If txtfromdate.Text <> "" And txttodate.Text <> "" Then
        '    parm(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
        '    parm(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
        'Else
        '    parm(2) = New SqlParameter("@fromdate", Format(CType(DateTime.Now, Date), "yyyy/MM/dd"))
        '    parm(3) = New SqlParameter("@todate", Format(CType(DateTime.Now.AddMonths(3), Date), "yyyy/MM/dd"))
        'End If

        'For i = 0 To 3
        '    parms.Add(parm(i))
        'Next



        'ds = New DataSet()
        'ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates_popdeal", parms)


        'If ds.Tables.Count > 0 Then
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        If IsDBNull(ds.Tables(0).Rows(0)("dealid")) = False Then
        '            'If Session("Calledfrom") = "Offers" Then
        '            '    strMsg = "Commission already exists For this  Season   " + CType(hdnpromotionid.Value, String) + " - Policy Id " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
        '            'Else
        '            '    strMsg = "Commission already exists For this  Season   " + CType(hdncontractid.Value, String) + " - Policy Id " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
        '            'End If

        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Deal Already exists within this Time Period ' );", True)
        '            finddateperiod = False
        '            Exit Function
        '        End If
        '    End If
        'End If
        finddateperiod = True
    End Function





    'Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Me.txtOtherGroup.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othgrpmast", "othgrpname", "othgrpcode", ddlGroup.SelectedValue), String)
    '    Catch ex As Exception

    '    End Try
    'End Sub



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PopularDealEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnViewimage_Click(sender As Object, e As System.EventArgs) Handles btnViewimage.Click

    End Sub

    Protected Sub Btnremove_Click(sender As Object, e As System.EventArgs) Handles Btnremove.Click
        txtimg.Value = ""
        ' hdnFileName.Text = ""
    End Sub
End Class

