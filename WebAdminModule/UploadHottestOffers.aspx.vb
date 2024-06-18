



 

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class UploadHottestOffers
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
                ViewState.Add("HottestOfferState", Request.QueryString("State"))
                ViewState.Add("HottestOfferRefCode", Request.QueryString("RefCode"))
                btnViewimage.Visible = True
                Btnremove.Visible = True
                If ViewState("HottestOfferState") = "New" Then
                    SetFocus(txtCode)
                    'txtCode.Attributes.Add("readonly", "readonly")
                    ' lblcode.Visible = False
                    ChkInactive.Checked = False
                    txtCode.Attributes.Add("readonly", "readonly")
                    Page.Title = " Add New Hottest Offers"
                    lblHeading.Text = "Add New Hottest Offers"
                    btnSave.Text = "Save"
                    btnViewimage.Visible = False
                    Btnremove.Visible = False
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("HottestOfferState") = "Edit" Then
                    '  SetFocus(txtName)
                    Page.Title = " Edit Hottest Offers"
                    lblHeading.Text = "Edit Hottest Offers"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("HottestOfferRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("HottestOfferState") = "View" Then
                    SetFocus(btnCancel)
                    Page.Title = " View Hottest Offers"
                    lblHeading.Text = "View Hottest Offers"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("HottestOfferRefCode"), String))
                ElseIf ViewState("HottestOfferState") = "Delete" Then
                    SetFocus(btnSave)
                    Page.Title = " Delete Hottest Offers"
                    lblHeading.Text = "Delete Hottest Offers"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("HottestOfferRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                ' Me.btnSave.Attributes("onclick") = "document.getElementById('" & Me.hdnFileName.ClientID & "').value=" & "document.getElementById('" & Me.hotelimage.ClientID & "').value;"

                btnViewimage.Attributes.Add("onclick", "return PopUpImageView('" & txtimg.Value & "')")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("UploadHottestOffers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
            ' txtimg.Value = hdnFileName.Text
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
        If ViewState("HottestOfferState") = "View" Or ViewState("HottestOfferState") = "Delete" Then
            txtCode.Disabled = True
            '    hotelimage.Disabled = True
            '  txtOthGrpname.Enabled = False
            txtPrefSupName.Enabled = False
            txtRemark.ReadOnly = True
            hotelimage.Enabled = False
            btnViewimage.Enabled = True
            Btnremove.Enabled = False
            ChkInactive.Disabled = True
            txtfromdate.Enabled = False
            txttodate.Enabled = False
            ImgBtnFrmDt.Enabled = False
            ImgBtnToDt.Enabled = False
            txtRemark.Enabled = False
        ElseIf ViewState("HottestOfferState") = "Edit" Then
            txtCode.Disabled = True

        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try

            'If txtfromdate.Text <> "" And txttodate.Text = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter OfferEndDate.');", True)
            '    SetFocus(txttodate)
            '    ValidatePage = False
            '    Exit Function
            'End If
            'If txttodate.Text <> "" And txtfromdate.Text = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter OfferStartDate.');", True)
            '    SetFocus(txtfromdate)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If hotelimage.FileName.Trim = "" And txtimg.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Image.');", True)
                SetFocus(txtimg)
                ValidatePage = False
                Exit Function
            End If
            If hotelimage.FileName <> "" Then
                If System.Drawing.Image.FromStream(hotelimage.PostedFile.InputStream).Height <> 464 Or System.Drawing.Image.FromStream(hotelimage.PostedFile.InputStream).Width <> 626 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select an Image With (626px X464px) Dimension');", True)
                    SetFocus(txtimg)
                    ValidImageSize = False
                    ValidatePage = False
                    Exit Function
                End If
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

    Public Function checkforexisting() As Boolean
        Try
            If ViewState("HottestOfferState") = "New" Then
                '    Dim strfilter As String = " active=1 "
                If txtPrefSupCode.Text <> "" Or txtPrefSupName.Text <> "" Then
                    If objUtils.isDuplicatenew(Session("dbconnectionName"), "tblHottestOffers", "partycode", txtPrefSupCode.Text.Trim) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier  Already Exists.');", True)
                        SetFocus(txtPrefSupName)
                        checkforexisting = False
                        Exit Function
                    End If
                End If
            End If
          

            If ChkInactive.Checked = False Then
                Dim checkstring As String = objUtils.CheckString(Session("dbconnectionName"), "select top 1 offerid from tblhottestoffers where active=1")
                If checkstring = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast One Offer Should be Active');", True)
                    SetFocus(ChkInactive)
                    checkforexisting = False
                    Exit Function
                End If
            End If


            If ViewState("HottestOfferState") = "Edit" Then


                If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "tblHottestOffers", "Offerid", "partycode", txtPrefSupCode.Text.Trim, txtCode.Value) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier  Already Exists.');", True)
                    SetFocus(txtPrefSupName)
                    checkforexisting = False
                    Exit Function
                End If
            End If

            checkforexisting = True

            'If FindDatePeriod() = False Then
            '    checkforexisting = False
            '    Exit Function
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Private Function finddateperiod() As Boolean
        Dim parms As New List(Of SqlParameter)
        Dim parm(4) As SqlParameter
        Dim ds As New DataSet

        parm(0) = New SqlParameter("@offerid", CType(txtCode.Value, String))
        parm(1) = New SqlParameter("@partycode", CType(txtPrefSupCode.Text, String))
        If txtfromdate.Text <> "" And txttodate.Text <> "" Then
            parm(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
            parm(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
        Else
            parm(2) = New SqlParameter("@fromdate", Format(CType(DateTime.Now, Date), "yyyy/MM/dd"))
            parm(3) = New SqlParameter("@todate", Format(CType(DateTime.Now.AddMonths(3), Date), "yyyy/MM/dd"))
        End If

        For i = 0 To 3
            parms.Add(parm(i))
        Next



        ds = New DataSet()
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates_hotoffer", parms)


        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("offerid")) = False Then
                    'If Session("Calledfrom") = "Offers" Then
                    '    strMsg = "Commission already exists For this  Season   " + CType(hdnpromotionid.Value, String) + " - Policy Id " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                    'Else
                    '    strMsg = "Commission already exists For this  Season   " + CType(hdncontractid.Value, String) + " - Policy Id " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                    'End If

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Offer Already exists within this Time Period ' );", True)
                    finddateperiod = False
                    Exit Function
                End If
            End If
        End If
        finddateperiod = True
    End Function
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
            '  If Page.IsValid = True Then
            If ViewState("HottestOfferState") = "New" Or ViewState("HottestOfferState") = "Edit" Then
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

                If ViewState("HottestOfferState") = "New" Then
                    Dim optionval As String

                    optionval = objUtils.GetAutoDocNo("HOTOFFER", mySqlConn, sqlTrans)
                    txtCode.Value = optionval.Trim
                    If txtimg.Value <> "" Then
                        txtimg.Value = txtCode.Value & "_" + txtimg.Value
                    End If
                    mySqlCmd = New SqlCommand("sp_add_tblHottestOffers", mySqlConn, sqlTrans)
                ElseIf ViewState("HottestOfferState") = "Edit" Then
                    mySqlCmd = New SqlCommand("sp_mod_tblHottestOffers", mySqlConn, sqlTrans)

                End If

                If hotelimage.FileName <> "" Then
                    strpath_logo1 = txtCode.Value & "_" & hotelimage.FileName
                    SaveImage(strpath_logo1)
                ElseIf ViewState("PopularDealState") = "New" And txtimg.Value <> "" Then
                    strpath_logo1 = txtCode.Value & "_" + txtimg.Value
                    SaveImage(strpath_logo1)
                Else
                    txtimg.Value = IIf(txtimg.Value = "", hotelimage.FileName, txtimg.Value)
                End If




                mySqlCmd.CommandType = CommandType.StoredProcedure

                mySqlCmd.Parameters.Add(New SqlParameter("@OfferId ", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)

                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtPrefSupCode.Text.Trim, String)

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@offerstartdate", SqlDbType.DateTime)).Value = txtfromdate.Text 'CType(Format(txtfromdate.Text, "yyyy/MM/dd"), DateTime)
                    mySqlCmd.Parameters.Add(New SqlParameter("@offerenddate", SqlDbType.DateTime)).Value = txttodate.Text 'CType(Format(txttodate.Text, "yyyy/MM/dd"), DateTime)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@offerstartdate", SqlDbType.DateTime)).Value = DBNull.Value  'CType(Format(txtfromdate.Text, "yyyy/MM/dd"), DateTime)
                    mySqlCmd.Parameters.Add(New SqlParameter("@offerenddate", SqlDbType.DateTime)).Value = DBNull.Value 'CType(Format(txttodate.Text, "yyyy/MM/dd"), DateTime)
                End If


                mySqlCmd.Parameters.Add(New SqlParameter("@hotelimage", SqlDbType.VarChar, 100)).Value = CType(txtimg.Value, String)
                'End If

                mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)

                If ChkInactive.Checked = True Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                ElseIf ChkInactive.Checked = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                End If

                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                ' mySqlCmd.Parameters.Add(New SqlParameter("@timelimitcheck", SqlDbType.Int)).Value = IIf(chktimelimit.Checked = True, 1, 0)
                'End If


                mySqlCmd.ExecuteNonQuery()

            ElseIf ViewState("HottestOfferState") = "Delete" Then

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                mySqlCmd = New SqlCommand("sp_del_tblHottestOffers", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@OfferId ", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                mySqlCmd.ExecuteNonQuery()
            End If

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            'Response.Redirect("OtherServiceTypesSearch.aspx", False)
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('OtherServicesPLTypesWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("VisaTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from TblHottestOffers Where Offerid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("Offerid")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("Offerid"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("OfferStartDate")) = False Then
                        Me.txtfromdate.Text = CType(mySqlReader("OfferStartDate"), String)
                    Else
                        Me.txtfromdate.Text = ""
                    End If
                    If IsDBNull(mySqlReader("OfferEndDate")) = False Then
                        Me.txttodate.Text = CType(mySqlReader("OfferEndDate"), String)
                    Else
                        Me.txttodate.Text = ""
                    End If


                    If IsDBNull(mySqlReader("partycode")) = False Then
                        txtPrefSupCode.Text = CType(mySqlReader("partycode"), String)
                        txtPrefSupName.Text = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", CType(mySqlReader("partycode"), String)), String)
                    Else
                        txtPrefSupCode.Text = " "
                        txtPrefSupName.Text = " "
                    End If
                    'If IsDBNull(mySqlReader("rankorder")) = False Then
                    '    txtOrder.Value = mySqlReader("rankorder")
                    'Else
                    '    Me.txtOrder.Value = ""
                    'End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Text = mySqlReader("remarks")
                    Else
                        Me.txtRemark.Text = ""
                    End If
                    'If IsDBNull(mySqlReader("minpax")) = False Then
                    '    Me.txtMinPax.Value = mySqlReader("minpax")
                    'Else
                    '    Me.txtMinPax.Value = ""
                    'End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            ChkInactive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            ChkInactive.Checked = False
                        End If
                    End If


                    'If IsDBNull(mySqlReader("timelimitcheck")) = False Then
                    '    If CType(mySqlReader("timelimitcheck"), String) = "1" Then
                    '        chktimelimit.Checked = True
                    '    ElseIf CType(mySqlReader("timelimitcheck"), String) = "0" Then
                    '        chktimelimit.Checked = False
                    '    End If
                    'End If


                    If IsDBNull(mySqlReader("hotelimage")) = False Then
                        txtimg.Value = mySqlReader("hotelimage")
                    Else
                        txtimg.Value = ""
                    End If


                    'If IsDBNull(mySqlReader("paxcheckReq")) = False Then
                    '    If CType(mySqlReader("paxcheckReq"), String) = "1" Then
                    '        ChkPakReq.Checked = True
                    '    ElseIf CType(mySqlReader("paxcheckReq"), String) = "0" Then
                    '        ChkPakReq.Checked = False
                    '    End If
                    'End If

                    'If IsDBNull(mySqlReader("printRemarks")) = False Then
                    '    If CType(mySqlReader("printRemarks"), String) = "1" Then
                    '        ChkPrnRemark.Checked = True
                    '    ElseIf CType(mySqlReader("printRemarks"), String) = "0" Then
                    '        ChkPrnRemark.Checked = False
                    '    End If
                    'End If

                    'If IsDBNull(mySqlReader("autocancelreq")) = False Then
                    '    If CType(mySqlReader("autocancelreq"), String) = "1" Then
                    '        ChkAutoCancel.Checked = True
                    '    ElseIf CType(mySqlReader("autocancelreq"), String) = "0" Then
                    '        ChkAutoCancel.Checked = False
                    '    End If
                    'End If


                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPLTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            strSqlQry = "select partyname,partycode from partymast where active=1  and sptypecode=(select option_selected  from reservation_parameters  where param_id=458 ) and showinweb=1  and  partyname like  '" & Trim(prefixText) & "%'"

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

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Dim strFilter As String
        If (Session("OthTypeFilter") <> "OTH") Then
            strFilter = "othgrpcode= (select option_selected  from reservation_parameters where param_id =" + Session("OthTypeFilter") + ")"
        Else
            strFilter = "othgrpcode not in (select * from view_system_othgrp)"
            ' " '(Select Option_Selected From Reservation_ParaMeters" & _Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027))"
        End If


        If ViewState("HottestOfferState") = "New" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypcode", CType(txtCode.Value.Trim, String), strFilter) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type code is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypname", hotelimage.Value.Trim, strFilter) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type name is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "rankorder", CType(txtOrder.Value.Trim, String)) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type Order is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            'ElseIf ViewState("HottestOfferState") = "Edit" Then
            '    If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "othtypname", hotelimage.Value.Trim, CType(txtCode.Value.Trim, String), strFilter) Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type name is already present.');", True)
            '        checkForDuplicate = False
            '        Exit Function
            '    End If
            'If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "rankorder", CType(txtOrder.Value.Trim, String), CType(txtCode.Value.Trim, String), strFilter) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type Order is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
        End If

        checkForDuplicate = True
    End Function
#End Region



    'Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Me.txtOtherGroup.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othgrpmast", "othgrpname", "othgrpcode", ddlGroup.SelectedValue), String)
    '    Catch ex As Exception

    '    End Try
    'End Sub

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothtyp", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServicesTypes of suppliers, cannot delete this ServiceTypes');", True)
        '    checkForDeletion = False
        '    Exit Function

        'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " oplistdnew", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServicesPriceList, cannot delete this ServiceTypes');", True)
        '    checkForDeletion = False
        '    Exit Function

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " othplist_selld", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServices PriceList, cannot delete this ServiceTypes');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If
        'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " oplistd", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a DetailsofOtherServices CostPriceList, cannot delete this ServiceTypes');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If

        checkForDeletion = True
    End Function
#End Region


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=HottestDealEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnViewimage_Click(sender As Object, e As System.EventArgs) Handles btnViewimage.Click

    End Sub

    Protected Sub Btnremove_Click(sender As Object, e As System.EventArgs) Handles Btnremove.Click
        txtimg.Value = ""

    End Sub
End Class


