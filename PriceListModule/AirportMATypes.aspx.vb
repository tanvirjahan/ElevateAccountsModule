#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region


Partial Class PriceListModule_AirportMATypes
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region



#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return  checkNumberdecimal(event)")
    End Sub
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
                ViewState.Add("AirportMATypesState", Request.QueryString("State"))
                ViewState.Add("AirportMATypesRefCode", Request.QueryString("RefCode"))

                Dim strqry As String = ""
                Dim strtitle As String = ""
                Dim strOption As String = ""
                txtCode.Disabled = True

                ddlRateType.SelectedIndex = 0

                TxtCompAdult.Disabled = True
                TxtCompChild.Disabled = True
                ddlAddPaxChkReqd.Style("display") = "none"
                lbladdpaxreqd.Style("display") = "none"

                TxtPaxForUnit.Value = "0" ' Added by abin on 20180418
                TxtPaxForUnit.Style("display") = "none"
                lblPaxforUnit.Style("display") = "none" 'end

                If (Session("OthTypeFilter") <> Nothing And Session("OthTypeFilter") <> "OTH") Then

                    strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthTypeFilter"))
                    strtitle = "Airport Meet & Assist Types"

                    strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id='" & Session("OthTypeFilter") & "') order by othgrpcode"
                ElseIf Session("OthTypeFilter") = "OTH" Then
                    strtitle = "Other Service Types"


                    strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027,1028)) order by othgrpcode"

                End If


                '"select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1003') order by othgrpcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", strqry, True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", strqry, True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", "select acctcode,acctname from acctgroup where childid=38 order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", "select  acctname,acctcode from acctgroup where childid=38 order by acctname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccrualCode, "acctcode", "acctname", "select acctcode,acctname from acctgroup where childid=39 order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccrualName, "acctname", "acctcode", "select  acctname,acctcode from acctgroup where childid=39 order by acctname", True)



                If Session("OthTypeFilter") <> "OTH" Then
                    ddlOtherGrpCode.SelectedIndex = 0
                    ddlOtherGrpName.SelectedIndex = 0
                    ddlOtherGrpCode.Disabled = True
                    ddlOtherGrpName.Disabled = True
                End If
                'txtTrns.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =564")
                Dim strGrpCode As String = Request.Params("GrpCode")

                If ViewState("AirportMATypesState") = "New" Then
                    'SetFocus(txtCode)
                    lblHeading.Text = "Add " + strtitle
                    Page.Title = Page.Title + " " + "New " + strtitle
                    btnSave.Text = "Save"
                    fillairport()

                    ddlAccName.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =917")
                    ddlAccCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", ddlAccName.Value)

                    ddlAccrualName.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =918")
                    ddlAccrualCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", ddlAccrualName.Value)


                    Dim strOrdrQry As String = ""
                    If Session("OthTypeFilter") <> "OTH" Then
                        strOrdrQry = "select isnull (max(rankorder),0) from othtypmast where othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='" + Session("OthTypeFilter") + "')"
                    Else
                        strOrdrQry = "select isnull (max(rankorder),0) from othtypmast where othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027,1028))" 'order by othgrpcode
                    End If
                    ' txtorder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strOrdrQry) + 1
                    btnSave.Attributes.Add("onclick", "return Validate('New')")

                ElseIf ViewState("AirportMATypesState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit " + strtitle
                    Page.Title = Page.Title + " " + "Edit " + strtitle
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("AirportMATypesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return Validate('Edit')")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("AirportMATypesState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View " + strtitle
                    Page.Title = Page.Title + " " + "View " + strtitle
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Other Types"
                    DisableControl()

                    ShowRecord(CType(ViewState("AirportMATypesRefCode"), String))

                ElseIf ViewState("AirportMATypesState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete " + strtitle
                    Page.Title = Page.Title + " " + "Delete " + strtitle
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("AirportMATypesRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "return Validate('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                 Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
              
                Numbers(txtMinPax)
                Numbers(txtMaxPax)
                Numbers(TxtCompAdult)
                Numbers(TxtCompChild)
                Numberssrvctrl(txtChildFrmAge)
                Numberssrvctrl(txtChildToAge)
                'charcters1(txtRemark)
                btnViewimage.Attributes.Add("onclick", "return PopUpImageView('" & txtimg.Value & "')")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("AirportMATypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If


        'txtChildFrmAge.Attributes.Add("onchange", "validateage()")
        'txtChildToAge.Attributes.Add("onchange", "validateage()")
        If ddlRateType.SelectedIndex = 0 Then
            TxtCompAdult.Disabled = True
            TxtCompChild.Disabled = True
        Else
            TxtCompAdult.Disabled = False
            TxtCompChild.Disabled = False
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


    Protected Sub Btnselectall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnselectall.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In grdairports.Rows
            chksel = gvRow1.FindControl("chk")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub Btnunselectall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnunselectall.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In grdairports.Rows
            chksel = gvRow1.FindControl("chk")
            chksel.Checked = False
        Next
    End Sub




    Private Sub fillairport()
        Dim Dt As DataSet = New DataSet
        Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode ,airportbordername from airportbordersmaster order by airportbordername")
        grdairports.DataSource = Dt.Tables(0)
        grdairports.DataBind()

    End Sub

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("AirportMATypesState") = "View" Or ViewState("AirportMATypesState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            ''ddlGroup.Enabled = False
            ddlOtherGrpCode.Disabled = True
            ddlOtherGrpName.Disabled = True
            ''txtOtherGroup.Disabled = True
            fileVehicleImage.Enabled = False
            grdairports.Enabled = False
            txtimg.Disabled = True
            ddlOtherGrpCode.Disabled = True
            ddlOtherGrpName.Disabled = True
            txtRemark.ReadOnly = True
            ddltype.Disabled = True
            txtprefpartyname.Enabled = False
            txtMaxPax.Disabled = True
            txtMinPax.Disabled = True
            txtChildFrmAge.Disabled = True
            txtChildToAge.Disabled = True
            ChkInactive.Disabled = True
            ddlRateType.Disabled = True
            btnViewimage.Enabled = False
            Btnremove.Enabled = False
            Btnselectall.Enabled = False
            Btnunselectall.Enabled = False
            'ChkPrnConfirm.Disabled = True
            ' ChkPakReq.Disabled = True
            'ChkPrnRemark.Disabled = True
            ChkAutoCancel.Disabled = True
            'ddlPName.Disabled = True
            'ddlDName.Disabled = True
            'ddlType.Disabled = True
            grdairports.Enabled = False
            txtRemark.Enabled = False
        ElseIf ViewState("AirportMATypesState") = "Edit" Then
            txtCode.Disabled = True
            ddlOtherGrpCode.Disabled = True
            ddlOtherGrpName.Disabled = True
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
            If Val(txtChildFrmAge.Value.Trim) > Val(txtChildToAge.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Child To Age Should be greater than From Age.');", True)
                SetFocus(txtName)
                ValidatePage = False
                Exit Function
            End If

            If txtprefpartyname.Text.Trim = "" Or txtprefpartycode.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Preferred Supplier Cannot be Blank.');", True)
                SetFocus(txtprefpartyname)
                ValidatePage = False
                Exit Function
            End If
            If txtChildFrmAge.Value <> "" Then
                If txtChildToAge.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Child To Age.');", True)
                    SetFocus(txtChildToAge)
                    ValidatePage = False
                    Exit Function
                End If
            End If
            If txtChildToAge.Value <> "" Then
                If txtChildFrmAge.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Child From Age.');", True)
                    SetFocus(txtChildFrmAge)
                    ValidatePage = False
                    Exit Function
                End If
            End If


            If ddlOtherGrpCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group.');", True)
                SetFocus(ddlOtherGrpCode)
                ValidatePage = False
                Exit Function
            End If


            If ddltype.Value = 0 Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Airport Meet Type');", True)
                SetFocus(ddltype)
                ValidatePage = False
                Exit Function
            End If
            If ddlRateType.SelectedIndex = 1 Then
                If TxtCompAdult.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Compulsory Adult');", True)
                    SetFocus(TxtCompAdult)
                    ValidatePage = False
                    Exit Function
                End If
                If TxtCompChild.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Compulsory Child.');", True)
                    SetFocus(TxtCompChild)
                    ValidatePage = False
                    Exit Function
                End If
            End If

            'If txtOrder.Value.Trim = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Order field can not be blank.');", True)
            '    SetFocus(txtOrder)
            '    ValidatePage = False
            '    Exit Function

            'If txtOrder.Value.Trim = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Order field can not be blank.');", True)
            '    SetFocus(txtOrder)
            '    ValidatePage = False
            '    Exit Function
            'Else
            '    If Val(txtOrder.Value) <= 0 Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Order can not be less than or equal to zero.');", True)
            '        SetFocus(txtOrder)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            'End If
            If txtMinPax.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('MinPax field can not be blank.');", True)
                SetFocus(txtMinPax)
                ValidatePage = False
                Exit Function
            Else
                If Val(txtMinPax.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('MinPax can not be less than or equal to zero.');", True)
                    SetFocus(txtMinPax)
                    ValidatePage = False
                    Exit Function
                End If
            End If





            Dim chk As HtmlInputCheckBox
            Dim i As Integer

            Dim flag As Boolean = False

            For i = 0 To grdairports.Rows.Count - 1

                chk = CType(grdairports.Rows(i).FindControl("chk"), HtmlInputCheckBox)
                If chk.Checked Then
                    flag = True


                End If


            Next

            If flag = False Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select atleast one Airport Border!!!.');", True)

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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim strpath1 As String = ""
        Dim strpath2 As String = ""
        Dim strpath3 As String = ""
        Dim strpath4 As String = ""
        Dim strpath_logo1 As String = ""
        Dim strpath_logo2 As String = ""
        Dim strpath_logo3 As String = ""
        Dim strpath_logo4 As String = ""

        Try

            If Page.IsValid = True Then
                If ViewState("AirportMATypesState") = "New" Or ViewState("AirportMATypesState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    If fileVehicleImage.FileName <> "" Then

                        strpath_logo1 = txtCode.Value & "_" & fileVehicleImage.FileName ' IIf(txtimg.Value = "", fileVehicleImage.FileName, txtimg.Value)

                        strpath1 = Server.MapPath("UploadedImages/" & strpath_logo1)
                        fileVehicleImage.PostedFile.SaveAs(strpath1)
                        txtimg.Value = strpath_logo1
                        hdnFileName.Text = txtimg.Value
                        'Else
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid Image Size.');", True)
                        'End If
                    Else
                        txtimg.Value = IIf(txtimg.Value = "", fileVehicleImage.FileName, txtimg.Value)

                        ' SendImageToWebService(strpath1, strpath_logo1)
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("AirportMATypesState") = "New" Then

                        Dim optionval As String
                        Dim optionName As String

                        optionName = GetNoGenName(Session("OthTypeFilter"))
                        optionval = objUtils.GetAutoDocNo(optionName, mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_othtyp", mySqlConn, sqlTrans)
                    ElseIf ViewState("AirportMATypesState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_othtyp", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypname", SqlDbType.VarChar, 150)).Value = CType(txtName.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroup.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int, 4)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@minpax", SqlDbType.Int, 4)).Value = CType(Val(txtMinPax.Value), Long)
                    'If ChkPrnConfirm.Checked = True Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@printconf", SqlDbType.Int)).Value = 1
                    'ElseIf ChkPrnConfirm.Checked = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@printconf", SqlDbType.Int)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckreq", SqlDbType.Int)).Value = 0

                    mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 0
                    'End If

                    If ChkAutoCancel.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@autocancelreq", SqlDbType.Int)).Value = 1
                    ElseIf ChkAutoCancel.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@autocancelreq", SqlDbType.Int)).Value = 0
                    End If
                    If ChkInactive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf ChkInactive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.Int)).Value = ddlRateType.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@pickuppoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@dropoffpoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(txtimg.Value, String) ' CType(fileVehicleImage.FileName, String)

                    If ddlAccCode.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = CType(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    If ddlAccrualCode.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = CType(ddlAccrualCode.Items(ddlAccrualCode.SelectedIndex).Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    '  If ddltype.Value = "Arrival" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@AirportMeettype", SqlDbType.Int)).Value = ddltype.Value

                    mySqlCmd.Parameters.Add(New SqlParameter("@ratebasis", SqlDbType.VarChar, 20)).Value = CType(ddlRateType.Items(ddlRateType.Value).Text, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = CType(ddltype.Items(ddltype.Value).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@maxpax", SqlDbType.Int, 4)).Value = CType(Val(txtMaxPax.Value), Long)
                    If txtChildFrmAge.Value <> "" And txtChildToAge.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@childfrom", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtChildFrmAge.Value)
                        mySqlCmd.Parameters.Add(New SqlParameter("@childto", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtChildToAge.Value)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@childfrom", SqlDbType.Decimal)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@childto", SqlDbType.Decimal)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@additionalpaxyesno", SqlDbType.Int)).Value = ddlAddPaxChkReqd.SelectedIndex 'CType(ddlAddPaxChkReqd.Items(ddlAddPaxChkReqd.Value).Text, String)
                    If ddlRateType.SelectedIndex = 0 Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@compulsoryadult", SqlDbType.Int, 4)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@compulsorychild", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@compulsoryadult", SqlDbType.Int, 4)).Value = CType(TxtCompAdult.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@compulsorychild", SqlDbType.Int, 4)).Value = CType(TxtCompChild.Value, String)
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@preferredsupplier", SqlDbType.VarChar, 20)).Value = CType(txtprefpartycode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@flag", SqlDbType.Int, 4)).Value = 1
                    If ddlRateType.SelectedIndex = 0 Then 'added by Abin on 20180418
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxforunit", SqlDbType.Int, 4)).Value = CType("0", String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxforunit", SqlDbType.Int, 4)).Value = CType(TxtPaxForUnit.Value.Trim, String)
                    End If

                    mySqlCmd.ExecuteNonQuery()



                    If ViewState("AirportMATypesState") = "Edit" Then

                        Dim mySqlCmdgrid As SqlCommand

                        mySqlCmdgrid = New SqlCommand("sp_del_othtypmast_airportborders ", mySqlConn, sqlTrans)
                        mySqlCmdgrid.CommandType = CommandType.StoredProcedure
                        mySqlCmdgrid.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmdgrid.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)


                        mySqlCmdgrid.ExecuteNonQuery()

                    End If



                    Dim i As Integer = 0
                    Dim chk As HtmlInputCheckBox
                    For i = 0 To grdairports.Rows.Count - 1
                        chk = CType(grdairports.Rows(i).FindControl("chk"), HtmlInputCheckBox)
                        If chk.Checked Then

                            Dim mySqlCmdgrid As SqlCommand

                            mySqlCmdgrid = New SqlCommand("sp_add_othtypmast_airportborders", mySqlConn, sqlTrans)
                            mySqlCmdgrid.CommandType = CommandType.StoredProcedure
                            mySqlCmdgrid.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmdgrid.Parameters.Add(New SqlParameter("@airportbordercode", SqlDbType.VarChar, 20)).Value = CType(grdairports.Rows(i).Cells(1).Text, String)

                            mySqlCmdgrid.ExecuteNonQuery()

                        End If

                    Next






                ElseIf ViewState("AirportMATypesState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_othtyp", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@flag", SqlDbType.Int, 4)).Value = 1
                    mySqlCmd.ExecuteNonQuery()

                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("OtherServiceTypesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('AirportMATypesWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("AirportMATypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("select o.othtypcode,o.othtypname,o.othgrpcode,o.incomecode,o.expensecode,o.rankorder,o.printconf,o.printremarks,o.autocancelreq,o.active,o.remarks,o.transfertype,o.pickuppoint,o.dropoffpoint,o.imagename,o.incomecode,o.expensecode,o.AirportMeettype,a.ratebasis,a.servicetype ,a.minpax,a.maxpax,a.childfrom,a.childto,a.additionalpaxyesno,a.compulsoryadult,a.compulsorychild ,a.preferredsupplier,isnull(a.paxforunit,0)paxforunit  from othtypmast  o left outer join airportmatypes a on o.othtypcode =a.othtypcode Where o.othtypcode='" & RefCode & "'", mySqlConn)
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
                        ddlOtherGrpCode.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", CType(mySqlReader("othgrpcode"), String)), String)
                        ddlOtherGrpName.Value = CType(mySqlReader("othgrpcode"), String)
                    Else
                        ddlOtherGrpCode.Value = "[Select]"
                        ddlOtherGrpName.Value = "[Select]"
                        End If


                    If IsDBNull(mySqlReader("incomecode")) = False Then
                        ddlAccCode.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("incomecode"), String)), String)
                        ddlAccName.Value = CType(mySqlReader("incomecode"), String)
                    Else
                        ddlAccCode.Value = "[Select]"
                        ddlAccName.Value = "[Select]"
                        End If

                    If IsDBNull(mySqlReader("expensecode")) = False Then
                        ddlAccrualCode.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("expensecode"), String)), String)
                        ddlAccrualName.Value = CType(mySqlReader("expensecode"), String)
                    Else
                        ddlAccrualCode.Value = "[Select]"
                        ddlAccrualName.Value = "[Select]"
                        End If


                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Text = mySqlReader("remarks")
                    Else
                        Me.txtRemark.Text = ""
                        End If
                    If IsDBNull(mySqlReader("minpax")) = False Then
                        Me.txtMinPax.Value = mySqlReader("minpax")
                    Else
                        Me.txtMinPax.Value = ""
                        End If
                    If IsDBNull(mySqlReader("maxpax")) = False Then
                        Me.txtMaxPax.Value = mySqlReader("maxpax")
                    Else
                        Me.txtMaxPax.Value = ""
                        End If


                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            ChkInactive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            ChkInactive.Checked = False
                            End If
                        End If


                    If IsDBNull(mySqlReader("ratebasis")) = False Then
                        If mySqlReader("ratebasis") = "Adult/Child" Then ' modified by abin on 20180418
                            Me.ddlRateType.SelectedIndex = 0
                            TxtCompAdult.Disabled = True
                            TxtCompChild.Disabled = True
                            TxtCompAdult.Value = ""
                            TxtCompChild.Value = ""

                            ddlAddPaxChkReqd.Style("display") = "none"
                            lbladdpaxreqd.Style("display") = "none"
                            TxtPaxForUnit.Value = "0" ' Added by abin on 20180418
                            TxtPaxForUnit.Style("display") = "none"
                            lblPaxforUnit.Style("display") = "none" 'end

                        ElseIf mySqlReader("ratebasis") = "Unit" Then
                            Me.ddlRateType.SelectedIndex = 1
                            TxtCompAdult.Disabled = False
                            TxtCompChild.Disabled = False
                            ddlAddPaxChkReqd.Style("display") = "block"
                            lbladdpaxreqd.Style("display") = "block"
                            TxtPaxForUnit.Value = mySqlReader("paxforunit") ' Added by abin on 20180418
                            TxtPaxForUnit.Style("display") = "block"
                            lblPaxforUnit.Style("display") = "block" 'end

                            If IsDBNull(mySqlReader("compulsoryadult")) = False Then
                                Me.TxtCompAdult.Value = mySqlReader("compulsoryadult")
                            Else
                                Me.TxtCompAdult.Value = ""
                            End If
                            If IsDBNull(mySqlReader("compulsorychild")) = False Then
                                Me.TxtCompChild.Value = mySqlReader("compulsorychild")
                            Else
                                Me.TxtCompChild.Value = ""
                            End If

                        End If
                    Else
                        Me.ddlRateType.Value = "[Select]"
                    End If

                        'If mySqlReader("ratebasis") <> "Unit" Then
                        '    TxtCompAdult.Disabled = True
                        '    TxtCompChild.Disabled = True
                        'Else
                        '    TxtCompAdult.Disabled = False
                        '    TxtCompChild.Disabled = False

                        'End If
                     
                    If IsDBNull(mySqlReader("childfrom")) = False Then
                        If mySqlReader("childfrom") = "0" Then
                            Me.txtChildFrmAge.Value = ""
                        Else
                            Me.txtChildFrmAge.Value = mySqlReader("childfrom")

                        End If
                    End If

                    If IsDBNull(mySqlReader("childto")) = False Then
                   
                        If mySqlReader("childto") = "0" Then
                            Me.txtChildToAge.Value = ""
                        Else
                            Me.txtChildToAge.Value = mySqlReader("childto")

                        End If
                    End If
                    If IsDBNull(mySqlReader("autocancelreq")) = False Then
                        If CType(mySqlReader("autocancelreq"), String) = "1" Then
                            ChkAutoCancel.Checked = True
                        ElseIf CType(mySqlReader("autocancelreq"), String) = "0" Then
                            ChkAutoCancel.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("additionalpaxyesno")) = False Then
                        Me.ddlAddPaxChkReqd.Value = mySqlReader("additionalpaxyesno")

                    End If
                    'If IsDBNull(mySqlReader("transfertype")) = False Then

                    '    ddlRateType.Value = CType(mySqlReader("transfertype"), String)

                    'End If
                    If IsDBNull(mySqlReader("preferredsupplier")) = False Then
                        txtprefpartyname.Text = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", CType(mySqlReader("preferredsupplier"), String)), String)
                        txtprefpartycode.Text = CType(mySqlReader("preferredsupplier"), String)
                    Else
                        txtprefpartycode.Text = ""
                        txtprefpartyname.Text = ""
                    End If


                    If IsDBNull(mySqlReader("AirportMeettype")) = False Then

                        'If CType(mySqlReader("AirportMeettype"), Integer) = 1 Then
                        '    ddltype.SelectedIndex = 0

                        'ElseIf CType(mySqlReader("AirportMeettype"), Integer) = 2 Then
                        '    ddltype.SelectedIndex = 1
                        'ElseIf CType(mySqlReader("AirportMeettype"), Integer) = 3 Then
                        '    ddltype.SelectedIndex = 2
                        'Else
                        '    ddltype.SelectedIndex = 3
                        'End If

                        ddltype.Value = CType(mySqlReader("AirportMeettype"), Integer)

                    End If

                    Dim strpath As String
                    If IsDBNull(mySqlReader("imagename")) = False Then
                        Me.txtimg.Value = mySqlReader("imagename")
                        strpath = Server.MapPath("UploadedImages\" & Me.txtimg.Value)
                        hdnFileName.Text = mySqlReader("imagename")
                    Else
                        Me.txtimg.Value = ""
                    End If



                    ''''

                    fillairport()
                    Dim chk As HtmlInputCheckBox
                    Dim i As Integer
                    Dim k As Integer
                    Dim ds As DataSet = New DataSet

                    ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode from   othtypmast_airportborders where othtypcode='" & RefCode & "' ")
                    For k = 0 To ds.Tables(0).Rows.Count - 1
                        For i = 0 To grdairports.Rows.Count - 1
                            If CType(grdairports.Rows(i).Cells(1).Text, String).ToLower = ds.Tables(0).Rows(k).Item("airportbordercode").ToLower Then

                                chk = CType(grdairports.Rows(i).FindControl("chk"), HtmlInputCheckBox)
                                chk.Checked = True
                            End If

                        Next

                    Next


                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AirportMATypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function GetPrefPartylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""

        Dim myDS As New DataSet
        Dim partynames As New List(Of String)
        Try
            strSqlQry = "select partycode,partyname from partymast where sptypecode=(select option_Selected from reservation_parameters where param_id=564) and  active=1 and partyname like '" & Trim(prefixText) & "%'"
            ' strSqlQry = strSqlQry + " and sectorname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    partynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            Else



            End If
            Return partynames
        Catch ex As Exception
            Return partynames
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
            strFilter = "othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                 " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027,1028))"
        End If


        If ViewState("AirportMATypesState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypcode", CType(txtCode.Value.Trim, String), strFilter) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
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
         

        ElseIf ViewState("AirportMATypesState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "othtypname", txtName.Value.Trim, CType(txtCode.Value.Trim, String), strFilter) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
           


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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=AirMAType','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub txtprefpartyname_TextChanged(sender As Object, e As System.EventArgs) Handles txtprefpartyname.TextChanged

    End Sub

    Protected Sub ddlRateType_ServerChange(sender As Object, e As System.EventArgs) Handles ddlRateType.ServerChange
        'If ddlRateType.Value = "Unit" Then
        '    TxtCompAdult.Disabled = False
        '    TxtCompChild.Disabled = False
        'Else
        '    TxtCompAdult.Disabled = True
        '    TxtCompChild.Disabled = True
        'End If
    End Sub

    Protected Sub txtChildToAge_ServerChange(sender As Object, e As System.EventArgs) Handles txtChildToAge.ServerChange

    End Sub

    Protected Sub Btnremove_Click(sender As Object, e As System.EventArgs) Handles Btnremove.Click
        txtimg.Value = ""
    End Sub
End Class
