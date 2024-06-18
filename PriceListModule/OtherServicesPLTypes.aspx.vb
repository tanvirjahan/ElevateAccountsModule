

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class OtherServicesPLTypes
    Inherits System.Web.UI.Page



#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
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
                'txtconnection.Value = Session("dbconnectionName")
                ViewState.Add("CarRentalState", Request.QueryString("State"))
                ViewState.Add("CarRentalRefCode", Request.QueryString("RefCode"))
                txtCode.Disabled = True
                Dim strqry As String = ""
                Dim strtitle As String = ""
                Dim strOption As String = ""
                If (Session("OthTypeFilter") <> Nothing And Session("OthTypeFilter") <> "OTH") Then

                    strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthTypeFilter"))
                    Select Case strOption
                        Case "CAR RENTAL"
                            strtitle = "Car Rental Types"
                            'lblHeading.Text = "Car Rental Selling Formula"
                        Case "VISA"
                            strtitle = "Visa Types"
                            'lblHeading.Text = "Visa Selling Formula"
                        Case "EXC"
                            strtitle = "Excursion Types"
                            'lblHeading.Text = "Excursion Selling Formula"
                        Case "MEALS"
                            strtitle = "Restaurant Types"
                            'lblHeading.Text = "Restaurant Selling Formula"
                        Case "GUIDES"
                            strtitle = "Guide Types"
                            'lblHeading.Text = "Guide Selling Formula"
                        Case "ENTRANCE"
                            strtitle = "Entrance Types"
                            'lblHeading.Text = "Guide Selling Formula"
                        Case "JEEPWADI"
                            strtitle = "Jeeb Ride Types"
                            'lblHeading.Text = "Guide Selling Formula"
                        Case "HFEES"
                            strtitle = "Handling Fee Types"
                    End Select

                    strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id='" & Session("OthTypeFilter") & "') order by othgrpcode"
                ElseIf Session("OthTypeFilter") = "OTH" Then
                    strtitle = "Other Service Types"
                    lblExcTaxInv.Visible = True
                    chkExcTaxInv.Visible = True

                    strqry = "select othgrpcode,othgrpname from othgrpmast inner join othmaingrpmast on othgrpmast.othmaingrpcode =othmaingrpmast.othmaingrpcode and othmaingrpmast.othmaingrpcode not in(Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id in (1001,1002,1003,1021,1022,1027,1028,1105)) order by othgrpcode"

                End If


                '"select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1003') order by othgrpcode"
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", strqry, True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", strqry, True)
                'If Session("OthTypeFilter") <> "OTH" Then
                '    ddlOtherGrpCode.SelectedIndex = 0
                '    ddlOtherGrpName.SelectedIndex = 0
                '    ddlOtherGrpCode.Disabled = True
                '    ddlOtherGrpName.Disabled = True
                'End If
                'txtTrns.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =564")
                Dim strGrpCode As String = Request.Params("GrpCode")

                If ViewState("CarRentalState") = "New" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Add " + strtitle
                    Page.Title = Page.Title + " " + "New " + strtitle
                    btnSave.Text = "Save"

                    Dim strOrdrQry As String = ""
                    If Session("OthTypeFilter") <> "OTH" Then
                        strOrdrQry = "select isnull (max(rankorder),0) from othtypmast where othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='" + Session("OthTypeFilter") + "')"
                    Else
                        strOrdrQry = "select isnull (max(rankorder),0) from othtypmast where othgrpcode not in (Select  * From view_system_othgrp)"
                        '     " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027,1028))" 'order by othgrpcode
                    End If
                    '     txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strOrdrQry) + 1
                    btnSave.Attributes.Add("onclick", "return Validate('New')")

                ElseIf ViewState("CarRentalState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit " + strtitle
                    Page.Title = Page.Title + " " + "Edit " + strtitle
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("CarRentalRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return Validate('Edit')")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("CarRentalState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View " + strtitle
                    Page.Title = Page.Title + " " + "View " + strtitle
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Other Types"
                    DisableControl()

                    ShowRecord(CType(ViewState("CarRentalRefCode"), String))

                ElseIf ViewState("CarRentalState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete " + strtitle
                    Page.Title = Page.Title + " " + "Delete " + strtitle
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("CarRentalRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "return Validate('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                btnViewimage.Attributes.Add("onclick", "return PopUpImageView('" & txtimg.Value & "')")
                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                '    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'End If
                'charcters(txtCode)
                'charcters(txtName)
                'Numbers(txtOrder)
                'Numbers(txtMinPax)
                'charcters1(txtRemark)

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CarRentalTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("CarRentalState") = "View" Or ViewState("CarRentalState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            txtOthGrpname.Enabled = False
            txtPrefSupName.Enabled = False
            txtRemark.ReadOnly = True
            OthServiceImg.Enabled = False
            btnViewimage.Enabled = False
            Btnremove.Enabled = False
            ChkInactive.Disabled = True
            chkExcTaxInv.Disabled = True
         
            txtRemark.Enabled = False
        ElseIf ViewState("CarRentalState") = "Edit" Then
            txtCode.Disabled = True
      
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            'If txtCode.Value = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
            '    SetFocus(txtCode)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If txtName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                SetFocus(txtName)
                ValidatePage = False
                Exit Function
            End If
            If txtOthGrpCode.Text.Trim = "" Or txtOthGrpname.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('please Select Group.');", True)
                SetFocus(txtOthGrpname)
                ValidatePage = False
                Exit Function
            End If
            If txtPrefSupCode.Text.Trim = "" Or txtPrefSupName.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('please Select Preferred Supplier.');", True)
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

    Private Function GetNoGenName(ByVal prm_type As String) As String
        GetNoGenName = ""
        Dim strOption As String = ""

        If prm_type <> "OTH" Then
            strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", prm_type)
            Select Case strOption
                Case "CAR RENTAL"
                    GetNoGenName = "CTYPE"
                    Exit Function
                Case "VISA"
                    GetNoGenName = "VTYPE"
                    Exit Function
                Case "EXC"
                    GetNoGenName = "EXTYPE"
                    Exit Function
                Case "MEALS"
                    GetNoGenName = "MTYPE"
                    Exit Function
                Case "GUIDES"
                    GetNoGenName = "GTYPE"
                    Exit Function
                Case "ENTRANCE"
                    GetNoGenName = "ENTYPE"
                    Exit Function
                Case "JEEPWADI"
                    GetNoGenName = "JTYPE"
                    Exit Function
                Case "HFEES"
                    GetNoGenName = "HTYPE"
                    Exit Function
                Case "AIRPORTMA"
                    GetNoGenName = "ATYPE"
                    Exit Function
            End Select
        Else
            GetNoGenName = "OTYPE"

        End If
    End Function

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpath1, strpath_logo1 As String
        Try
            If Page.IsValid = True Then
                If ViewState("CarRentalState") = "New" Or ViewState("CarRentalState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    If OthServiceImg.FileName <> "" Then

                        strpath_logo1 = txtCode.Value & "_" & OthServiceImg.FileName ' IIf(txtimg.Value = "", fileVehicleImage.FileName, txtimg.Value)

                        strpath1 = Server.MapPath("UploadedImages/" & strpath_logo1)
                        OthServiceImg.PostedFile.SaveAs(strpath1)
                        txtimg.Value = strpath_logo1
                        hdnFileName.Text = txtimg.Value
                        'Else
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid Image Size.');", True)
                        'End If
                    Else
                        txtimg.Value = IIf(txtimg.Value = "", OthServiceImg.FileName, txtimg.Value)

                        ' SendImageToWebService(strpath1, strpath_logo1)
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("CarRentalState") = "New" Then
                        Dim optionval As String
                        Dim optionName As String

                        optionName = GetNoGenName(Session("OthTypeFilter"))
                        optionval = objUtils.GetAutoDocNo(optionName, mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim
                        mySqlCmd = New SqlCommand("sp_add_othtyp", mySqlConn, sqlTrans)
                    ElseIf ViewState("CarRentalState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_othtyp", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypname", SqlDbType.VarChar, 150)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtOthGrpCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int, 4)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@minpax", SqlDbType.Int, 4)).Value = DBNull.Value

                    mySqlCmd.Parameters.Add(New SqlParameter("@printconf", SqlDbType.Int)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.Int)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckReq", SqlDbType.Int)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@dropoffpoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@autocancelreq", SqlDbType.Int)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@pickuppoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(txtimg.Value, String)
                    'End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@preferredsupplier", SqlDbType.VarChar, 20)).Value = CType(txtPrefSupCode.Text.Trim, String)

                    If ChkInactive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf ChkInactive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@timelimitcheck", SqlDbType.Int)).Value = IIf(chktimelimit.Checked = True, 1, 0)

                    If Session("OthTypeFilter") = "OTH" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@excludeTaxInvoice", SqlDbType.Bit)).Value = IIf(chkExcTaxInv.Checked = True, 1, 0)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@excludeTaxInvoice", SqlDbType.Bit)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("CarRentalState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_othtyp", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
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
            End If
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
            mySqlCmd = New SqlCommand("Select * from othtypmast Where othtypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othtypcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("othtypcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("othtypname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("othtypname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    'If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    '    Me.ddlGroup.SelectedValue = CType(mySqlReader("othgrpcode"), String)
                    '    Me.txtOtherGroup.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othgrpmast", "othgrpname", "othgrpcode", ddlGroup.SelectedValue), String)
                    'End If
                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        txtOthGrpCode.Text = CType(mySqlReader("othgrpcode"), String)
                        txtOthGrpname.Text = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", CType(mySqlReader("othgrpcode"), String)), String)
                    Else
                        txtOthGrpCode.Text = " "
                        txtOthGrpname.Text = " "
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


                    If IsDBNull(mySqlReader("timelimitcheck")) = False Then
                        If CType(mySqlReader("timelimitcheck"), String) = "1" Then
                            chktimelimit.Checked = True
                        ElseIf CType(mySqlReader("timelimitcheck"), String) = "0" Then
                            chktimelimit.Checked = False
                        End If
                    End If


                    If IsDBNull(mySqlReader("imagename")) = False Then
                        txtimg.Value = mySqlReader("imagename")
                    Else
                        txtimg.Value = ""
                    End If

                    If Session("OthTypeFilter") = "OTH" Then
                        If IsDBNull(mySqlReader("ExcludeTaxInvoice")) = False Then
                            If CType(mySqlReader("ExcludeTaxInvoice"), Boolean) = True Then
                                chkExcTaxInv.Checked = True
                            Else
                                chkExcTaxInv.Checked = False
                            End If
                        Else
                            chkExcTaxInv.Checked = False
                        End If
                    Else
                        chkExcTaxInv.Checked = False
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
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select partyname,partycode from partymast where active=1 and sptypecode= ( select option_selected from reservation_parameters  where param_id=1501) and    partyname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetOthGrpslist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim othgrpnames As New List(Of String)
        Try

            strSqlQry = "select othgrpname,othgrpcode from othgrpmast where active=1 and othgrpcode not in (  select * from view_system_othgrp) and othgrpname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    othgrpnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othgrpname").ToString(), myDS.Tables(0).Rows(i)("othgrpcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return othgrpnames
        Catch ex As Exception
            Return othgrpnames
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


        If ViewState("CarRentalState") = "New" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypcode", CType(txtCode.Value.Trim, String), strFilter) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type code is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypname", txtName.Value.Trim, strFilter) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "rankorder", CType(txtOrder.Value.Trim, String)) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type Order is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
        ElseIf ViewState("CarRentalState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "othtypname", txtName.Value.Trim, CType(txtCode.Value.Trim, String), strFilter) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
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

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " othplist_selld", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServices PriceList, cannot delete this ServiceTypes');", True)
            checkForDeletion = False
            Exit Function
        End If
        'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " oplistd", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a DetailsofOtherServices CostPriceList, cannot delete this ServiceTypes');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If

        checkForDeletion = True
    End Function
#End Region


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServiceTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnViewimage_Click(sender As Object, e As System.EventArgs) Handles btnViewimage.Click

    End Sub

    Protected Sub Btnremove_Click(sender As Object, e As System.EventArgs) Handles Btnremove.Click
        txtimg.Value = ""
    End Sub
End Class
