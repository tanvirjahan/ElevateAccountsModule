#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports MyClasses
Imports System.Web.Services

#End Region

Partial Class ExcursionModule_ExcursionRequestSubEntry
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim SqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim ObjDate As New clsDateTime
    Dim sqlTrans As SqlTransaction
    Dim chkSel As CheckBox
    Dim otypecode1, otypecode2, hotel As String
    Dim gvRow As GridViewRow
    Dim lblLineNo As Label
    Dim txtTicketDate As TextBox
    Dim chkDel As CheckBox
    Dim dtExcursionDetails As New DataTable
    Dim NewRLineNo As Integer = 0
    Private IsMultipleCost As Boolean = False

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                Try
                    Session.Add("ExcursionRequestSubEntryState", Request.QueryString("State"))
                    Session.Add("ExcursionRequestSubEntryRefCode", Request.QueryString("RefCode"))
                    Session.Add("ExcursionRequestSubEntryLineNo", Request.QueryString("LineNo"))

                    Session("TempMultiCostGrid") = Nothing
                    Session("OldTempMultiCostGrid") = Nothing 'Changed by Riswan on 12/04/2015

                    txtconnection.Value = Session("dbconnectionName")
                    txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)

                    otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                    otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                    hotel = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=458")

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionGroup, "othmaingrpname", "othmaingrpcode", "select rtrim(ltrim(othmaingrpcode))othmaingrpcode,rtrim(ltrim(othmaingrpname))othmaingrpname from othmaingrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othmaingrpname", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionSubGroup, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlHotel, "partyname", "partycode", "select ltrim(rtrim(partycode))partycode,ltrim(rtrim(partyname))partyname from partymast  where sptypecode = '" & hotel & "' and active=1 order by partyname ", True)

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTourGuide, "guidename", "guidecode", "   select ltrim(rtrim(guidecode))guidecode,ltrim(rtrim(guidename))guidename from guide_master where active=1 order by guidename", True)

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightNo, "flightcode", "flight_tranid", "select ltrim(rtrim(flightcode))flightcode,ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=0 order by flightcode", True)

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionType, "othtypname", "othtypcode", "select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname", True)
                    NumbersForTextbox(txtAdult)
                    NumbersForTextbox(txtChild)

                    NumbersForTextbox(txtAdultRate)
                    NumbersForTextbox(txtCostCurr)
                    NumbersForTextbox(txtAdultCostRate)
                    NumbersForTextbox(txtCostAmount)
                    NumbersForTextbox(txtChildRate)
                    NumbersForTextbox(txtCostConvRate)

                    NumbersForTextbox(txtChildCostRate)
                    NumbersForTextbox(txtCostConvRate)
                    NumbersForTextbox(txtCostConvRate)
                    NumbersForTextbox(txtCostConvRate)
                    NumbersForTextbox(txtCostConvRate)

                    If CType(Session("ExcursionRequestSubEntryState"), String) = "AddRow" Then
                        txtAdult.Attributes.Add("onchange", "return calculateAdultChildRate()")
                        txtChild.Attributes.Add("onchange", "return calculateAdultChildRate()")
                        txtAdultRate.Attributes.Add("onchange", "return calculateAdultChildRate()")
                        txtChildRate.Attributes.Add("onchange", "return calculateAdultChildRate()")

                        txtAdultCostRate.Attributes.Add("onchange", "return calculateAdultChildCostRate()")
                        txtChildCostRate.Attributes.Add("onchange", "return calculateAdultChildCostRate()")

                        txtSPersonComPer.Attributes.Add("onchange", "return calculateAdultChildRate()")

                        txtTourDate.Attributes.Add("onchange", "CallWebMethod('ExcursionType')")

                        Dim RefCode As String
                        RefCode = CType(Session("ExcursionRequestSubEntryRefCode"), String)
                        btnSave.Text = "Save"
                        txtTourDate.Text = Date.Now
                        txtArrivalDate.Text = Date.Now
                        txtDepartureDate.Text = Date.Now
                        txtreminder.Text = Date.Now
                        Dim LineNo As Integer
                        LineNo = CType(Session("ExcursionRequestSubEntryLineNo"), Integer)
                        ShowRecordFromDt(RefCode, LineNo)

                        lblHeading.Text = "Add Excursion Sub Entry"
                        Page.Title = Page.Title + " " + "Add Excursion Sub Entry"

                        txtwaiting.Text = "Our representative  will be waiting at the meeting location, starting the meeting time for 20 Minutes. "
                        txtcaneldeadline.Text = "4 Days "
                        txtcancelcharges.Text = "Full Charges "
                        txtnoshowcharges.Text = "Full Charges "


                    ElseIf CType(Session("ExcursionRequestSubEntryState"), String) = "EditRow" Then
                        txtAdult.Attributes.Add("onchange", "return calculateAdultChildRate()")
                        txtChild.Attributes.Add("onchange", "return calculateAdultChildRate()")
                        txtAdultRate.Attributes.Add("onchange", "return calculateAdultChildRate()")
                        txtChildRate.Attributes.Add("onchange", "return calculateAdultChildRate()")

                        txtAdultCostRate.Attributes.Add("onchange", "return calculateAdultChildCostRate()")
                        txtChildCostRate.Attributes.Add("onchange", "return calculateAdultChildCostRate()")

                        txtSPersonComPer.Attributes.Add("onchange", "return calculateAdultChildRate()")

                        txtTourDate.Attributes.Add("onchange", "CallWebMethod('ExcursionType')")

                        txtTourDate.Text = Date.Now
                        txtArrivalDate.Text = Date.Now
                        txtDepartureDate.Text = Date.Now
                        txtreminder.Text = Date.Now

                        If Session("RowState") = "ViewRow" Then

                            Dim RefCode As String
                            Dim LineNo As Integer
                            LineNo = CType(Session("ExcursionRequestSubEntryLineNo"), Integer)
                            RefCode = CType(Session("ExcursionRequestSubEntryRefCode"), String)
                            btnSave.Visible = False
                            btnMCSave.Visible = False
                            ShowRecordFromDt(RefCode, LineNo)
                            DisableControls()
                            lblHeading.Text = "View Excursion Sub Entry"
                            Page.Title = Page.Title + " " + "View Excursion Sub Entry"
                        Else
                            Dim RefCode As String
                            Dim LineNo As Integer
                            LineNo = CType(Session("ExcursionRequestSubEntryLineNo"), Integer)
                            RefCode = CType(Session("ExcursionRequestSubEntryRefCode"), String)
                            btnSave.Text = "Update"
                            'ShowRecord(RefCode, LineNo)
                            ShowRecordFromDt(RefCode, LineNo)
                            'btnResetAll.Enabled = False
                            lblHeading.Text = "Edit Excursion Sub Entry"
                            Page.Title = Page.Title + " " + "Edit Excursion Sub Entry"
                        End If

                    ElseIf CType(Session("ExcursionRequestSubEntryState"), String) = "ViewRow" Then

                    End If

                    'changed by mohamed on 01/05/2016
                    '------------------------------------------------------------------------------------------------------------
                    Dim lsBaseCurrCode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT upper(ltrim(RTrim(option_selected))) currency FROM reservation_parameters Where param_id=457")
                    Dim lsSuppCurrCode As String = lsBaseCurrCode
                    If Session("exCurrCodeMhd") IsNot Nothing Then
                        If Session("exCurrCodeMhd").ToString.Trim <> "" Then
                            lsSuppCurrCode = Session("exCurrCodeMhd").ToString.Trim.ToUpper
                        End If
                    End If
                    lblAmountAED.Text = "Amount(" & lsBaseCurrCode & ")"
                    lblTotalAmountAED.Text = "Total Amount(" & lsBaseCurrCode & ")"
                    lblCostAmountAED.Text = "Cost Amt(" & lsBaseCurrCode & ")"
                    '------------------------------------------------------------------------------------------------------------
                Catch ex As Exception

                End Try
            End If

            If IsPostBack = True Then

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionGroup, "othmaingrpname", "othmaingrpcode", "select rtrim(ltrim(othmaingrpcode))othmaingrpcode,rtrim(ltrim(othmaingrpname))othmaingrpname from othmaingrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othmaingrpname", True, hdnExcursionGroupCode.Value)

                Dim sqlstr As String = ""

                If ddlExcursionGroup.Value <> "[select]" Then
                    sqlstr = "select rtrim(ltrim(othgrpname))othgrpname,rtrim(ltrim(othgrpcode))othgrpcode from othgrpmast where active=1 and  othmaingrpcode='" & ddlExcursionGroup.Value & "' order by othgrpname"
                Else
                    sqlstr = "select rtrim(ltrim(othgrpname))othgrpname,rtrim(ltrim(othgrpcode))othgrpcode from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname"

                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionSubGroup, "othgrpname", "othgrpcode", sqlstr, True, hdnExcursionSubGroupCode.Value)
                sqlstr = ""

                If ddlExcursionGroup.Value <> "[Select]" Then
                    sqlstr = " select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & ddlExcursionGroup.Value & "' ) and a.active=1 order by a.othtypname"
                Else
                    sqlstr = " select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname"
                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionType, "othtypname", "othtypcode", sqlstr, True, hdnExcursionTypeCode.Value)

                ' sqlstr = "exec sp_get_exc_providers '2014/04/12','2014/04/12','BOUQUET'"
                Dim ddlExcursionType2 As String = "BOUQUET"
                sqlstr = "exec sp_get_exc_providers '" & ObjDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Text.Trim) & "','" & ObjDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Text.Trim) & "','" & ddlExcursionType.Value & "'"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionProvider, "partyname", "partycode", sqlstr, True, hdnExcursionProvider.Value)

                sqlstr = "select ltrim(rtrim(flightcode))flightcode,ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=" & hdnFlightType.Value & " order by flightcode"

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightNo, "flightcode", "flight_tranid", sqlstr, True, hdnFlightNo.Value)
                'txtAdultRate.Text = hdnAdultRate.Value
                'txtChildRate.Text = hdnChildRate.Value
                'txtSPersonCom.Text = hdnSPersonComPer.Value

                ddlFlightNo.Value = hdnFlightNo.Value
                txtCostCurr.Text = hdnCostCurr.Value
                txtCostConvRate.Text = hdnCostCurrConvRate.Value

                txtAmount.Text = hdnAmount.Value
                txtAmountAED.Text = hdnAmountAED.Value

                If Val(DirectCast(wucExcSumSubEntry.FindControl("txtTotal"), HtmlInputText).Value) = 0 Then
                    txtCostAmount.Text = hdnCostAmount.Value
                    txtCostAmountAED.Text = hdnCostAmountAED.Value
                Else
                    If wucExcSumSubEntry.IsGridUpdated Then
                        hdnCostAmount.Value = DirectCast(wucExcSumSubEntry.FindControl("txtTotal"), HtmlInputText).Value
                        hdnCostAmountAED.Value = DirectCast(wucExcSumSubEntry.FindControl("txtTotal"), HtmlInputText).Value
                    End If
                End If

                txtCostCurr.Text = hdnCostCurr.Value
                txtCostConvRate.Text = hdnCostCurrConvRate.Value

                txtTotalAmountAED.Text = hdnAmountAED.Value

            End If


            Dim MultiCostCount As Integer = 0



            If Session("TempMultiCostGrid") Is Nothing Then
                MultiCostCount = CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select Count(rlineno) rlinenocount from excursions_cost_detail_temp where excid='" & CType(Session("Excursion_ID"), String) & "' and rlineno='" & Session("ExcursionRequestSubEntryLineNo") & "'"), Integer)

                hdnIsMultipleCost.Value = MultiCostCount
            Else
                hdnIsMultipleCost.Value = 1
            End If

            
            'Changed by Riswan on 12/04/2015
            ddlExcursionProvider.Attributes.Remove("disabled")
            If hdnIsMultipleCost.Value > 0 Then
                ddlExcursionProvider.Attributes.Add("disabled", "disabled")
            End If
            txtAdultCostRate.Attributes.Remove("disabled")
            txtChildCostRate.Attributes.Remove("disabled")
            If ddlExcursionProvider.Value = "[Select]" Then
                txtAdultCostRate.Attributes.Add("disabled", "disabled")
                txtChildCostRate.Attributes.Add("disabled", "disabled")
            End If


            btnSave.Attributes.Add("onclick", "return FormValidation('New')")

            hdnSellingTypeForWS.Value = Request.QueryString("SellingType")
            hdnSpersonCodeForWS.Value = Request.QueryString("SpersonCode")
            'hdnPartyCodeForWS.Value = Request.QueryString("PartyCode")
            txtConfirmationNo.Text = Request.QueryString("TicketNo")
            'UpdatePanel1.Update()
            'UpdatePanel2.Update()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")
        'txttrfno.Attributes.Add("onkeypress", "return readonly(event)")
    End Sub
#End Region

#Region "NumbersForTextbox"
    Public Sub NumbersForTextbox(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")

    End Sub
#End Region

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim GvRow As GridViewRow
        Dim ObjDate As New clsDateTime
        Dim chk As CheckBox
        Dim frmdt As TextBox
        Dim todt As TextBox
        Dim RmCode As HtmlSelect
        Dim lblroomcodee As Label
        Dim lblstopsale As Label
        Try

            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master(nolock)")

            If txtTourDate.Text <> "" Then
                If Convert.ToDateTime(txtTourDate.Text) <= Convert.ToDateTime(sealdate) Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                    Return
                End If
            End If



            If Page.IsValid = True Then
                txtCostAmount.Text = hdnCostAmount.Value
                txtCostAmountAED.Text = hdnCostAmountAED.Value

                If ValidateCostValue() = False Then
                    txtCostAmount.Text = hdnCostAmount.Value
                    If ddlExcursionProvider.Value = "[Select]" Then
                        If CInt(Val(txtCostAmount.Text)) > 0 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select excursion provider');", True)
                            Exit Sub
                        End If
                    End If
                Else
                    If hdnValidate.Value = 1 Then
                        If ddlExcursionProvider.Value <> "[Select]" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value has entered from multiple cost. you cant change the excursion provider and the value of adult cost rate and child cost rate should be zero.');", True)
                            Exit Sub
                        End If
                        If CDec(Val(hdnCostAmount.Value)) <> CDec(Val(txtCostAmount.Text)) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check the cost amount. Please open multiple cost and click save.');", True)
                            Exit Sub
                        End If
                        If CInt(Val(txtAdultCostRate.Text)) <> 0 OrElse CInt(Val(txtChildCostRate.Text)) <> 0 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value has entered from multiple cost. you cant change the excursion provider and the value of adult cost rate and child cost rate should be zero.');", True)
                            Exit Sub
                        End If
                        ''' Added shahul 13/10/2015
                        If Val(hdnmulticostcheck.Value) = 1 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Adults and child modified. Please open multiple cost and click save.');", True)
                            Exit Sub
                        End If
                        '''''''''''
                    End If
                End If

                If ddlExcursionProvider.Value <> "[Select]" And (CDec(Val(txtAdultCostRate.Text)) = 0 And CDec(Val(txtChildCostRate.Text)) = 0) And CDec(Val(txtCostAmount.Text)) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter cost Amount.');", True)
                    Exit Sub
                End If
                'If ddlExcursionProvider.Value <> "[Select]" And (Val(txtAdultCostRate.Text) <> 0 Or Val(txtChildCostRate.Text) <> 0) And Val(txtCostAmount.Text) = 0 Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check Cost Amount and Cost Amount AED.');", True)
                '    Exit Sub
                'End If

                If CDec(Val(txtCostAmount.Text)) > 0 And CDec(Val(txtCostAmountAED.Text)) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check Cost Amount and Cost Amount AED.');", True)
                    Exit Sub
                End If

                If CDec(Val(txtCostAmountAED.Text)) > 0 And CDec(Val(txtCostAmount.Text)) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check Cost Amount and Cost Amount AED.');", True)
                    Exit Sub
                End If

                If CInt(Val(txtCostConvRate.Text)) = 1 Then
                    If CDec(Val(txtCostAmountAED.Text)) <> CDec(Val(txtCostAmount.Text)) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check Cost Amount and Cost Amount AED.');", True)
                        Exit Sub
                    End If
                End If

                If CType(Session("ExcursionRequestSubEntryState"), String) = "AddRow" Then

                    If ValidateDate() = False Then
                        Exit Sub
                    End If

                    If CheckForTicketNo(CType(Session("ExTicketNo"), String)) = False Then
                        Exit Sub
                    End If

                    Dim RlineNo As Integer = 0
                    If Session("ExcursionRequestSubEntryLineNo") = "" Then
                        RlineNo = 0
                    Else
                        RlineNo = CType(Session("ExcursionRequestSubEntryLineNo"), Integer)
                    End If

                    AddRows(RlineNo)

                    'Dim lstExcursionHeader As New List(Of clsExcursionHedaer)
                    'lstExcursionHeader = Session("lstExcursionHeader")

                End If


                If CType(Session("ExcursionRequestSubEntryState"), String) = "EditRow" Then

                    If ValidateDate() = False Then
                        Exit Sub
                    End If

                    'If CheckForTicketNo(CType(Session("ExTicketNo"), String)) = False Then
                    '    Exit Sub
                    'End If

                    If CType(Session("ExcursionRequestSubEntryLineNo"), String) = "" Then

                        AddRowsEdit()
                    Else
                        EditRows(CType(Session("ExcursionRequestSubEntryLineNo"), Integer))

                    End If

                End If
                'If CInt(Val(txtCostAmount.Text)) > 0 Then
                If SaveMultiCostTemp() = True Then  'Changed by Riswan on 12/04/2015
                    Session("TempMultiCostGrid") = Nothing
                    Session("OldTempMultiCostGrid") = Nothing 'Changed by Riswan on 12/04/2015
                End If
                'End If
            End If
        Catch ex As Exception
            'If mySqlConn.State = ConnectionState.Open Then
            'sqlTrans.Rollback()
            'End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Private Function ValidateCostValue() As Boolean
        Try
            If Session("TempMultiCostGrid") Is Nothing Then
                Dim rlinenotemp As String = String.Empty
                Dim ds As New DataSet
                If Request.QueryString("LineNo") Is Nothing Then rlinenotemp = "0" Else rlinenotemp = CInt(Val(Request.QueryString("LineNo")))
                Dim mValidQry As String = "Select * from excursions_cost_detail_temp where excid='" & CType(Session("Excursion_ID"), String) & "' and rlineno=" & rlinenotemp
                ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mValidQry)
                If ds.Tables(0).Rows.Count > 0 Then
                    hdnValidate.Value = 1
                    Return True
                Else
                    hdnValidate.Value = 0
                    Return False
                End If
                hdnValidate.Value = 0
            Else
                If hdnTotalMC.Value > 0 Then
                    hdnValidate.Value = 1
                    Return True
                Else
                    hdnValidate.Value = 0
                    Return False
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Function SaveMultiCostTemp() As Boolean  'Changed by Riswan on 12/04/2015
        Dim GrdhdnRowId As New HiddenField
        Dim GrdddlSupplier As New DropDownList
        Dim GrdddlExcType As New DropDownList
        Dim GrdtxtAdultCostRate As New TextBox
        Dim GrdtxtChildCostRate As New TextBox
        Dim GrdtxtCostCurrency As New TextBox
        Dim GrdtxtConversionRate As New TextBox
        Dim GrdtxtCostValue As New TextBox
        Dim Grdtxtnoofunits As New TextBox
        Dim GrdtxtRemarks As New TextBox

        Dim grdMultipleCost As New GridView
        If Session("TempMultiCostGrid") Is Nothing Then
            If Session("OldTempMultiCostGrid") Is Nothing Then
                grdMultipleCost = DirectCast(wucExcSumSubEntry.FindControl("grdMultipleCost"), GridView)
            Else
                grdMultipleCost = DirectCast(Session("OldTempMultiCostGrid"), GridView)
            End If
        Else
            grdMultipleCost = DirectCast(Session("TempMultiCostGrid"), GridView)
        End If

        If GrdtxtAdultCostRate.Text = "" Then GrdtxtAdultCostRate.Text = 0
        If GrdtxtChildCostRate.Text = "" Then GrdtxtChildCostRate.Text = 0
        If GrdtxtConversionRate.Text = "" Then GrdtxtConversionRate.Text = 0
        If GrdtxtCostValue.Text = "" Then GrdtxtCostValue.Text = 0
        If Grdtxtnoofunits.Text = "" Then Grdtxtnoofunits.Text = 0


        Try
            Dim rlinenotemp As Integer = 0
            Dim mRlineNo As Integer = 0
            Dim CurState As String = Session("ExcursionRequestSubEntryState")
            Dim mRowId As Integer = 0
            If GrdhdnRowId.Value <> "" Then mRowId = CInt(GrdhdnRowId.Value)

            If grdMultipleCost.Rows.Count > 0 Then

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = SqlConn.BeginTransaction 'Changed by Riswan on 12/04/2015



                '    If Request.QueryString("LineNo") Is Nothing Then rlinenotemp = "0" Else rlinenotemp = CInt(Val(Request.QueryString("LineNo")))
                '    Dim mDelQry As String = "DELETE from excursions_cost_detail_temp where excid='" & CType(Session("Excursion_ID"), String) & "' and rlineno=" & rlinenotemp
                '    SqlCmd = New SqlCommand(mDelQry, SqlConn, sqlTrans) 'Changed by Riswan on 12/04/2015
                '    SqlCmd.CommandType = CommandType.Text
                '    SqlCmd.ExecuteNonQuery()
                'Else

                If Request.QueryString("LineNo") Is Nothing Then rlinenotemp = "0" Else rlinenotemp = CInt(Val(Request.QueryString("LineNo")))
                Dim mDelQry As String = "DELETE from excursions_cost_detail_temp where excid='" & CType(Session("Excursion_ID"), String) & "' and rlineno=" & rlinenotemp
                SqlCmd = New SqlCommand(mDelQry, SqlConn, sqlTrans) 'Changed by Riswan on 12/04/2015
                SqlCmd.CommandType = CommandType.Text
                SqlCmd.ExecuteNonQuery()

                'SqlCmd = New SqlCommand("sp_InsertExcursionCostDetailsTemp", SqlConn, sqlTrans) 'Changed by Riswan on 12/04/2015
                For i = 0 To grdMultipleCost.Rows.Count - 1
                    mRowId = 0
                    GrdhdnRowId = CType(grdMultipleCost.Rows(i).FindControl("hdnRowID"), HiddenField)
                    GrdddlSupplier = CType(grdMultipleCost.Rows(i).FindControl("ddlSupplier"), DropDownList)
                    GrdddlExcType = CType(grdMultipleCost.Rows(i).FindControl("ddlExcType"), DropDownList)
                    GrdtxtAdultCostRate = CType(grdMultipleCost.Rows(i).FindControl("txtAdultCostRate"), TextBox)
                    GrdtxtChildCostRate = CType(grdMultipleCost.Rows(i).FindControl("txtChildCostRate"), TextBox)
                    GrdtxtCostCurrency = CType(grdMultipleCost.Rows(i).FindControl("txtCostCurrency"), TextBox)
                    GrdtxtConversionRate = CType(grdMultipleCost.Rows(i).FindControl("txtConversionRate"), TextBox)
                    GrdtxtCostValue = CType(grdMultipleCost.Rows(i).FindControl("txtCostValue"), TextBox)
                    Grdtxtnoofunits = CType(grdMultipleCost.Rows(i).FindControl("txtnoofunits"), TextBox)
                    GrdtxtRemarks = CType(grdMultipleCost.Rows(i).FindControl("txtRemarks"), TextBox)
                    If GrdhdnRowId.Value <> "" Then mRowId = CInt(GrdhdnRowId.Value)

                    If GrdtxtAdultCostRate.Text = "" Then GrdtxtAdultCostRate.Text = 0
                    If GrdtxtChildCostRate.Text = "" Then GrdtxtChildCostRate.Text = 0
                    If GrdtxtConversionRate.Text = "" Then GrdtxtConversionRate.Text = 0
                    If GrdtxtCostValue.Text = "" Then GrdtxtCostValue.Text = 0
                    If Grdtxtnoofunits.Text = "" Then Grdtxtnoofunits.Text = 0

                    If GrdddlSupplier.SelectedValue <> "[Select]" Then
                        SqlCmd = New SqlCommand("sp_InsertExcursionCostDetailsTemp", SqlConn, sqlTrans) 'Changed by Riswan on 12/04/2015
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@RowId", SqlDbType.Int)).Value = mRowId
                        SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = CType(Session("Excursion_ID"), String)
                        SqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = rlinenotemp
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GrdddlExcType.SelectedValue, String)
                        SqlCmd.Parameters.Add(New SqlParameter("@supplier", SqlDbType.VarChar, 20)).Value = CType(GrdddlSupplier.SelectedValue, String)
                        SqlCmd.Parameters.Add(New SqlParameter("@AdultCostRate", SqlDbType.Decimal)).Value = CType(GrdtxtAdultCostRate.Text, Decimal)
                        SqlCmd.Parameters.Add(New SqlParameter("@ChildCostRate", SqlDbType.Decimal)).Value = CType(GrdtxtChildCostRate.Text, Decimal)
                        SqlCmd.Parameters.Add(New SqlParameter("@costcurrency", SqlDbType.VarChar, 50)).Value = CType(GrdtxtCostCurrency.Text, String)
                        SqlCmd.Parameters.Add(New SqlParameter("@conversionrate", SqlDbType.Decimal)).Value = CType(GrdtxtConversionRate.Text, Decimal)
                        SqlCmd.Parameters.Add(New SqlParameter("@costvalue", SqlDbType.Decimal)).Value = CType(GrdtxtCostValue.Text, Decimal)
                        SqlCmd.Parameters.Add(New SqlParameter("@noofunits", SqlDbType.Int)).Value = CType(Grdtxtnoofunits.Text, Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 200)).Value = CType(GrdtxtRemarks.Text, String)
                        SqlCmd.ExecuteNonQuery()
                    End If
                Next



                sqlTrans.Commit()
            End If
            SaveMultiCostTemp = True 'Changed by Riswan on 12/04/2015
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            SaveMultiCostTemp = False 'Changed by Riswan on 12/04/2015
        Finally 'Changed by Riswan on 12/04/2015
            clsDBConnect.dbCommandClose(SqlCmd)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try


    End Function

    Private Sub AddRows(ByVal RLineNo As Integer)
        Try

            dtExcursionDetails = Session("DtExcursionSubEntry")
            If dtExcursionDetails.Rows.Count > 0 Then
                dtExcursionDetails.Rows(dtExcursionDetails.Rows.Count - 1).Delete()
            End If

            'NewRLineNo = Session("NewRLineNo") + 1
            'Session("NewRLineNo") = NewRLineNo
            If dtExcursionDetails.Rows.Count > 0 Then

                Dim i As Integer = 0
                Dim RowCount As Integer = 0
                Dim flg As Boolean = False

                For i = 0 To dtExcursionDetails.Rows.Count - 1
                    If dtExcursionDetails.Rows(i).Item("RlineNo") = RLineNo Then
                        RowCount = i
                        flg = True
                        Exit For
                    End If
                Next
                If flg Then
                    dtExcursionDetails.Rows.RemoveAt(RowCount)
                End If

            End If
            dtExcursionDetails.Rows.Add(RLineNo, 1, "", txtTourDate.Text, ddlExcursionSubGroup.Items(ddlExcursionSubGroup.SelectedIndex).Value,
           ddlExcursionSubGroup.Items(ddlExcursionSubGroup.SelectedIndex).Text, txtGuestName.Text, Val(txtAdult.Text), Val(txtChild.Text), Val(txtAdultRate.Text),
           Val(txtChildRate.Text), Val(txtAmount.Text), Val(txtAmountAED.Text), IIf(chkAmendment.Checked = True, 1, 0), IIf(chkCancel.Checked = True, 1, 0),
           Val(txtAdultCostRate.Text), Val(txtChildCostRate.Text), Val(txtCostAmountAED.Text), Val(txtCostAmount.Text), txtCancelReason.Text,
           IIf(chkFreeToCustomer.Checked = True, 1, 0), IIf(chkFreeFromSupplier.Checked = True, 1, 0), 0, 0, 0,
           0, 0, ddlHotel.Items(ddlHotel.SelectedIndex).Value, txtRoomNo.Text, ddlExcursionProvider.Items(ddlExcursionProvider.SelectedIndex).Value,
           txtAttention.Text, "", txtConfirmationNo.Text, IIf(ddlFlightType.Items(ddlFlightType.SelectedIndex).Value = 0, "D", "A"), ddlFlightNo.Items(ddlFlightNo.SelectedIndex).Text,
           txtFlightTime.Text, txtAirport.Text, txtPickTime.Text, txtRemarks.Text, CType(txtArrivalDate.Text, DateTime),
           CType(txtDepartureDate.Text, DateTime), "", Val(txtSPersonCom.Text), ddlHotel.Items(ddlHotel.SelectedIndex).Value, ddlTourGuide.Items(ddlTourGuide.SelectedIndex).Value,
           IIf(chktrfreq.Checked = True, 1, 0), 0, "", Val(txtTotalAmountAED.Text), txtIncomeInvoiceNo.Text,
           txtDebitNoteNo.Text, ddlExcursionProvider.Value, ddlExcursionType.Value, IIf(ddlConfirmed.Value = "1", 1, 0), ddlExcursionGroup.Value,
           txtCostCurr.Text.Trim, IIf(Val(txtCostConvRate.Text) = 0, 1, Val(txtCostConvRate.Text)), ddlExcursionType.Items(ddlExcursionType.SelectedIndex).Text, IIf(chksupconf.Checked = True, 1, 0), txtexctime.Text, txttrfno.Text, txtsupconfno.Text, txtprotktno.Text, txtreminder.Text, txtcancelcharges.Text, txtnoshowcharges.Text, txtcaneldeadline.Text, txtwaiting.Text)

            Session("DtExcursionSubEntry") = dtExcursionDetails

            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('ExcursionRequestEntryWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
    End Sub


    Private Sub AddRowsEdit()
        Try
            Dim rLineNo As Integer = 0
            rLineNo = CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select max(rlineno)+1 rlineno from excursions_detail where excid='" & CType(Session("ExcursionRequestRefCode"), String) & "'"), Integer)

            dtExcursionDetails = Session("DtExcursionSubEntry")
            If dtExcursionDetails.Rows.Count > 0 Then
                dtExcursionDetails.Rows(dtExcursionDetails.Rows.Count - 1).Delete()
            End If

            dtExcursionDetails.Rows.Add(rLineNo, 1, CType(Session("ExcursionRequestRefCode"), String), txtTourDate.Text, ddlExcursionSubGroup.Items(ddlExcursionSubGroup.SelectedIndex).Value,
           ddlExcursionSubGroup.Items(ddlExcursionSubGroup.SelectedIndex).Text, txtGuestName.Text, Val(txtAdult.Text), Val(txtChild.Text), Val(txtAdultRate.Text),
           Val(txtChildRate.Text), Val(txtAmount.Text), Val(txtAmountAED.Text), IIf(chkAmendment.Checked = True, 1, 0), IIf(chkCancel.Checked = True, 1, 0),
           Val(txtAdultCostRate.Text), Val(txtChildCostRate.Text), Val(txtCostAmountAED.Text), Val(txtCostAmount.Text), txtCancelReason.Text,
           IIf(chkFreeToCustomer.Checked = True, 1, 0), IIf(chkFreeFromSupplier.Checked = True, 1, 0), 0, 0, 0,
           0, 0, ddlHotel.Items(ddlHotel.SelectedIndex).Value, txtRoomNo.Text, ddlExcursionProvider.Items(ddlExcursionProvider.SelectedIndex).Value,
           txtAttention.Text, "", txtConfirmationNo.Text, IIf(ddlFlightType.Items(ddlFlightType.SelectedIndex).Value = 0, "D", "A"), ddlFlightNo.Items(ddlFlightNo.SelectedIndex).Text,
           txtFlightTime.Text, txtAirport.Text, txtPickTime.Text, txtRemarks.Text, CType(txtArrivalDate.Text, DateTime),
           CType(txtDepartureDate.Text, DateTime), "", Val(txtSPersonCom.Text), ddlHotel.Items(ddlHotel.SelectedIndex).Value, ddlTourGuide.Items(ddlTourGuide.SelectedIndex).Value,
           IIf(chktrfreq.Checked = True, 1, 0), 0, "", Val(txtTotalAmountAED.Text), txtIncomeInvoiceNo.Text,
           txtDebitNoteNo.Text, ddlExcursionProvider.Value, ddlExcursionType.Value, IIf(ddlConfirmed.Value = "1", 1, 0), ddlExcursionGroup.Value,
           txtCostCurr.Text.Trim, Val(txtCostConvRate.Text), ddlExcursionType.Items(ddlExcursionType.SelectedIndex).Text, IIf(chksupconf.Checked = True, 1, 0), txtexctime.Text, txttrfno.Text, txtsupconfno.Text, txtprotktno.Text, txtreminder.Text, txtcancelcharges.Text, txtnoshowcharges.Text, txtcaneldeadline.Text, txtwaiting.Text)

            Session("DtExcursionSubEntry") = dtExcursionDetails

            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('ExcursionRequestEntryWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub EditRows(ByVal LineNo As Integer)
        Try

            dtExcursionDetails = Session("DtExcursionSubEntry")
            If dtExcursionDetails.Rows.Count > 0 Then
                dtExcursionDetails.Rows(dtExcursionDetails.Rows.Count - 1).Delete()
            End If

            Dim i As Integer = 0
            Dim RowCount As Integer = 0
            Dim flg As Boolean = False

            For i = 0 To dtExcursionDetails.Rows.Count - 1
                If dtExcursionDetails.Rows(i).Item("RlineNo") = LineNo Then
                    RowCount = i
                    flg = True
                    Exit For
                End If
            Next
            If flg Then
                dtExcursionDetails.Rows.RemoveAt(RowCount)
            End If

            dtExcursionDetails.Rows.Add(LineNo, 1, CType(Session("ExcursionRequestSubEntryRefCode"), String), txtTourDate.Text, ddlExcursionSubGroup.Items(ddlExcursionSubGroup.SelectedIndex).Value,
           ddlExcursionSubGroup.Items(ddlExcursionSubGroup.SelectedIndex).Text, txtGuestName.Text, Val(txtAdult.Text), Val(txtChild.Text), Val(txtAdultRate.Text),
           Val(txtChildRate.Text), Val(txtAmount.Text), Val(txtAmountAED.Text), IIf(chkAmendment.Checked = True, 1, 0), IIf(chkCancel.Checked = True, 1, 0),
           Val(txtAdultCostRate.Text), Val(txtChildCostRate.Text), Val(txtCostAmountAED.Text), Val(txtCostAmount.Text), txtCancelReason.Text,
           IIf(chkFreeToCustomer.Checked = True, 1, 0), IIf(chkFreeFromSupplier.Checked = True, 1, 0), 0, 0, 0,
           0, 0, ddlHotel.Items(ddlHotel.SelectedIndex).Value, txtRoomNo.Text, ddlExcursionProvider.Items(ddlExcursionProvider.SelectedIndex).Value,
           txtAttention.Text, "", txtConfirmationNo.Text, IIf(ddlFlightType.Items(ddlFlightType.SelectedIndex).Value = 0, "D", "A"), ddlFlightNo.Items(ddlFlightNo.SelectedIndex).Text,
           txtFlightTime.Text, txtAirport.Text, txtPickTime.Text, txtRemarks.Text, CType(txtArrivalDate.Text, DateTime),
           CType(txtDepartureDate.Text, DateTime), "", Val(txtSPersonCom.Text), ddlHotel.Items(ddlHotel.SelectedIndex).Value, ddlTourGuide.Items(ddlTourGuide.SelectedIndex).Value,
           IIf(chktrfreq.Checked = True, 1, 0), 0, "", Val(txtTotalAmountAED.Text), txtIncomeInvoiceNo.Text,
           txtDebitNoteNo.Text, ddlExcursionProvider.Value, ddlExcursionType.Value, IIf(ddlConfirmed.Value = "1", 1, 0), ddlExcursionGroup.Value,
           txtCostCurr.Text.Trim, IIf(Val(txtCostConvRate.Text) = 0, 1, Val(txtCostConvRate.Text)), ddlExcursionType.Items(ddlExcursionType.SelectedIndex).Text, IIf(chksupconf.Checked = True, 1, 0), txtexctime.Text, txttrfno.Text, txtsupconfno.Text, txtprotktno.Text, txtreminder.Text, txtcancelcharges.Text, txtnoshowcharges.Text, txtcaneldeadline.Text, txtwaiting.Text)

            Session("DtExcursionSubEntry") = dtExcursionDetails

            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('ExcursionRequestEntryWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
    End Sub


    Private Sub ShowRecord(ByVal RefCode As String, ByVal LineNo As Integer)
        Try
            strSqlQry = "select *,othgrpmast.othmaingrpcode from excursions_detail inner join othgrpmast on excursions_detail.othgrpcode = othgrpmast.othgrpcode where excursions_detail.excid='" & RefCode & "' and rlineno=" & LineNo
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = SqlCmd.ExecuteReader
            Dim sqlstr As String = ""
            If mySqlReader.Read = True Then

                If IsDBNull(mySqlReader("othmaingrpcode")) = False Then
                    ddlExcursionGroup.Value = mySqlReader("othmaingrpcode")
                    hdnExcursionGroupCode.Value = mySqlReader("othmaingrpcode")
                End If


                If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    sqlstr = "select rtrim(ltrim(othgrpname))othgrpname,rtrim(ltrim(othgrpcode))othgrpcode from othgrpmast where active=1 and  othmaingrpcode='" & ddlExcursionGroup.Value & "' order by othgrpcode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionSubGroup, "othgrpname", "othgrpcode", sqlstr, True, hdnExcursionSubGroupCode.Value)
                    ddlExcursionSubGroup.Value = mySqlReader("othgrpcode")
                    hdnExcursionSubGroupCode.Value = mySqlReader("othgrpcode")
                End If

                If IsDBNull(mySqlReader("othtypcode")) = False Then
                    sqlstr = " select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & ddlExcursionGroup.Value & "' ) and a.active=1 order by a.othtypcode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionType, "othtypname", "othtypcode", sqlstr, True, hdnExcursionTypeCode.Value)
                    ddlExcursionType.Value = mySqlReader("othtypcode")
                    hdnExcursionTypeCode.Value = mySqlReader("othtypcode")
                End If

                If IsDBNull(mySqlReader("epartycode")) = False Then
                    hdnExcursionProvider.Value = mySqlReader("epartycode")
                    sqlstr = "exec sp_get_exc_providers '" & ObjDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Text.Trim) & "','" & ObjDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Text.Trim) & "','" & ddlExcursionType.Value & "'"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionProvider, "partyname", "partyname", sqlstr, True, hdnExcursionProvider.Value)
                    ddlExcursionProvider.Value = mySqlReader("epartycode")
                End If


                If IsDBNull(mySqlReader("hotel")) = False Then
                    ddlHotel.Value = mySqlReader("hotel")
                End If


                If IsDBNull(mySqlReader("confirmed")) = False Then
                    ddlConfirmed.Value = mySqlReader("confirmed")
                End If


                If IsDBNull(mySqlReader("guide")) = False Then
                    ddlTourGuide.Value = mySqlReader("guide")
                End If


                If IsDBNull(mySqlReader("flighttype")) = False Then
                    If mySqlReader("flighttype") = "A" Then
                        ddlFlightType.Value = "0"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightNo, "flightcode", "flight_tranid", "select ltrim(rtrim(flightcode))flightcode,ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=0 order by flightcode", True)
                    Else
                        ddlFlightType.Value = "1"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightNo, "flightcode", "flight_tranid", "select ltrim(rtrim(flightcode))flightcode,ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=1 order by flightcode", True)
                    End If

                End If

                If IsDBNull(mySqlReader("flightno")) = False Then
                    ddlFlightNo.Items(ddlFlightNo.SelectedIndex).Text = mySqlReader("flightno")
                End If




                If IsDBNull(mySqlReader("tourdate")) = False Then
                    txtTourDate.Text = Format(CType(mySqlReader("tourdate"), Date), "dd/MM/yyyy")
                End If


                If IsDBNull(mySqlReader("amended")) = False Then
                    If mySqlReader("amended") = 1 Then
                        chkAmendment.Checked = True
                    ElseIf mySqlReader("amended") = 0 Then
                        chkAmendment.Checked = False
                    End If
                End If

                If IsDBNull(mySqlReader("cancelled")) = False Then
                    If mySqlReader("cancelled") = 1 Then
                        chkCancel.Checked = True
                    ElseIf mySqlReader("cancelled") = 0 Then
                        chkCancel.Checked = False
                    End If
                End If

                If IsDBNull(mySqlReader("cancelreason")) = False Then
                    txtCancelReason.Text = mySqlReader("cancelreason")
                End If



                If IsDBNull(mySqlReader("complimentcust")) = False Then
                    If mySqlReader("complimentcust") = 1 Then
                        chkFreeToCustomer.Checked = True
                    ElseIf mySqlReader("complimentcust") = 0 Then
                        chkFreeToCustomer.Checked = False
                    End If
                End If

                If IsDBNull(mySqlReader("complimentprov")) = False Then
                    If mySqlReader("complimentprov") = 1 Then
                        chkFreeFromSupplier.Checked = True
                    ElseIf mySqlReader("complimentprov") = 0 Then
                        chkFreeFromSupplier.Checked = False
                    End If
                End If


                If IsDBNull(mySqlReader("spersoncomm")) = False Then
                    txtSPersonCom.Text = mySqlReader("spersoncomm")
                End If

                If IsDBNull(mySqlReader("spersoncomm")) = False Then
                    txtSPersonComPer.Text = mySqlReader("spersoncomm")
                End If




                If IsDBNull(mySqlReader("guestname")) = False Then
                    txtGuestName.Text = mySqlReader("guestname")
                End If

                If IsDBNull(mySqlReader("roomno")) = False Then
                    txtRoomNo.Text = mySqlReader("roomno")

                End If

                If IsDBNull(mySqlReader("adults")) = False Then
                    txtAdult.Text = mySqlReader("adults")
                End If

                If IsDBNull(mySqlReader("child")) = False Then
                    txtChild.Text = mySqlReader("child")
                End If

                If IsDBNull(mySqlReader("attn")) = False Then
                    txtAttention.Text = mySqlReader("attn")
                End If

                If IsDBNull(mySqlReader("salepricea")) = False Then
                    txtAdultRate.Text = mySqlReader("salepricea")
                End If

                If IsDBNull(mySqlReader("salepriceb")) = False Then
                    txtChildRate.Text = mySqlReader("salepriceb")
                End If

                If IsDBNull(mySqlReader("salepriceb")) = False Then
                    txtAmount.Text = mySqlReader("salepriceb")
                End If

                If IsDBNull(mySqlReader("salepriceb")) = False Then
                    txtAmountAED.Text = mySqlReader("salepriceb")
                End If


                'If IsDBNull(mySqlReader("spersoncode")) = False Then
                '    ddlExcursionProvider.Value = mySqlReader("spersoncode")
                'End If

                If IsDBNull(mySqlReader("costpricea")) = False Then
                    txtAdultCostRate.Text = mySqlReader("costpricea")
                End If

                If IsDBNull(mySqlReader("costpriceb")) = False Then
                    txtChildCostRate.Text = mySqlReader("costpriceb")
                End If

                If IsDBNull(mySqlReader("costvalue")) = False Then

                    txtCostAmount.Text = mySqlReader("costvalue")
                End If

                If IsDBNull(mySqlReader("costvalue")) = False Then
                    txtAmountAED.Text = mySqlReader("costvalue")
                End If



                If IsDBNull(mySqlReader("confno")) = False Then
                    txtConfirmationNo.Text = mySqlReader("confno")
                End If

                If IsDBNull(mySqlReader("incominginvno")) = False Then
                    txtIncomeInvoiceNo.Text = mySqlReader("incominginvno")
                End If

                If IsDBNull(mySqlReader("debitnoteno")) = False Then
                    txtDebitNoteNo.Text = mySqlReader("debitnoteno")
                End If



                If IsDBNull(mySqlReader("airport")) = False Then
                    txtAirport.Text = mySqlReader("airport")
                End If

                If IsDBNull(mySqlReader("arrdate")) = False Then
                    txtArrivalDate.Text = Format(CType(mySqlReader("arrdate"), Date), "dd/MM/yyyy")
                End If

                If IsDBNull(mySqlReader("depdate")) = False Then
                    txtDepartureDate.Text = Format(CType(mySqlReader("depdate"), Date), "dd/MM/yyyy")
                End If

                If IsDBNull(mySqlReader("pickuptime")) = False Then
                    txtPickTime.Text = mySqlReader("pickuptime")
                End If

                If IsDBNull(mySqlReader("flighttime")) = False Then
                    txtFlightTime.Text = mySqlReader("flighttime")
                End If

                If IsDBNull(mySqlReader("flighttime")) = False Then
                    txtRemarks.Text = ""
                End If

                If IsDBNull(mySqlReader("total_amount")) = False Then
                    txtTotalAmountAED.Text = mySqlReader("total_amount")
                End If

                If IsDBNull(mySqlReader("reminderdt")) = False Then
                    txtreminder.Text = Format(CType(mySqlReader("reminderdt"), Date), "dd/MM/yyyy")
                End If


                If IsDBNull(mySqlReader("cancelcharges")) = False Then
                    txtcancelcharges.Text = mySqlReader("cancelcharges")
                End If
                If IsDBNull(mySqlReader("noshowcharges")) = False Then
                    txtnoshowcharges.Text = mySqlReader("noshowcharges")
                End If
                If IsDBNull(mySqlReader("canceldeadline")) = False Then
                    txtcaneldeadline.Text = mySqlReader("canceldeadline")
                End If
                If IsDBNull(mySqlReader("waitingpolicy")) = False Then
                    txtwaiting.Text = mySqlReader("waitingpolicy")
                End If


            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub


    Private Function ValidateDate() As Boolean
        Try
            If ddlFlightNo.Value <> "[Select]" And ddlFlightNo.Value <> "" Then
                If CType(txtDepartureDate.Text, Date) < CType(txtArrivalDate.Text, Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Departure Date should be greater than Arrival Date');", True)
                    Return False
                End If
            End If


            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function

    Private Function CheckForTicketNo(ByVal ticketNo As String) As Boolean
        Try
            Dim strValue As String = ""
            strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ticketno from excursions_header where ticketno='" & ticketNo & "'")
            If strValue = "" Then
                Return True
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Entered Ticket No already exists');", True)
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try


    End Function

    Private Sub DisableControls()
        txtTourDate.ReadOnly = True
        ImgBtnTourDate.Enabled = False
        ImgBtnArrivalDate.Enabled = False
        ImgBtnDepartureDate.Enabled = False

        chkAmendment.Enabled = False
        chkCancel.Enabled = False
        txtCancelReason.ReadOnly = True
        ddlExcursionGroup.Disabled = True
        chkFreeFromSupplier.Enabled = False
        chkFreeToCustomer.Enabled = False
        ddlExcursionSubGroup.Disabled = True
        txtSPersonCom.ReadOnly = True
        txtSPersonComPer.ReadOnly = True
        ddlExcursionType.Disabled = True
        txtGuestName.ReadOnly = True
        txtRoomNo.ReadOnly = True
        txtAdult.ReadOnly = True
        txtChild.ReadOnly = True
        txtAttention.ReadOnly = True
        txtAdultRate.ReadOnly = True
        txtChildRate.ReadOnly = True
        txtAmount.ReadOnly = True
        txtAmountAED.ReadOnly = True
        ddlExcursionProvider.Disabled = True
        txtCostCurr.ReadOnly = True
        txtCostConvRate.ReadOnly = True
        txtAdultCostRate.ReadOnly = True
        txtChildCostRate.ReadOnly = True
        txtAdultCostRate.ReadOnly = True
        txtAirport.ReadOnly = True
        txtCostAmount.ReadOnly = True
        txtCostAmountAED.ReadOnly = True
        txtArrivalDate.ReadOnly = True
        ddlHotel.Disabled = True
        txtDepartureDate.ReadOnly = True
        ddlFlightType.Disabled = True
        ddlConfirmed.Disabled = True
        txtConfirmationNo.ReadOnly = True
        ddlFlightNo.Disabled = True
        txtPickTime.ReadOnly = True
        txtFlightTime.ReadOnly = True
        txtIncomeInvoiceNo.ReadOnly = True
        txtRemarks.ReadOnly = True
        txtDebitNoteNo.ReadOnly = True
        ddlTourGuide.Disabled = True
        txtTotalAmountAED.ReadOnly = True
        chksupconf.Enabled = False
        txtexctime.Enabled = False
        txttrfno.Enabled = False
        txtsupconfno.Enabled = False
        txtprotktno.Enabled = False
        txtreminder.Enabled = False
        chktrfreq.Enabled = False
    End Sub



    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Session("TempMultiCostGrid") = Nothing
        Session("OldTempMultiCostGrid") = Nothing 'Changed by Riswan on 12/04/2015

        Dim strscript As String = ""
        strscript = "window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Session("RowState") = ""
    End Sub



    Private Sub ShowRecordFromDt(ByVal RefCode As String, ByVal LineNo As Integer)
        Try

            Dim showDt As New DataTable
            Dim showDataRow() As DataRow
            Dim sqlstr As String = ""

            showDt = Session("DtExcursionSubEntry")

            If showDt.Rows.Count > 0 Then


                If RefCode = "" Then
                    showDataRow = showDt.Select("rlineno=" & LineNo)
                Else
                    showDataRow = showDt.Select("excid='" & RefCode & "' and rlineno=" & LineNo)
                End If


                If showDataRow.Length = 1 Then
                    'showDataRow(0)("excid").ToString()

                    If IsDBNull(showDataRow(0)("tourdate")) = False Then
                        txtTourDate.Text = Format(CType(showDataRow(0)("tourdate"), Date), "dd/MM/yyyy")
                    End If

                    If IsDBNull(showDataRow(0)("othmaingrpcode")) = False Then
                        ddlExcursionGroup.Value = showDataRow(0)("othmaingrpcode")
                        hdnExcursionGroupCode.Value = showDataRow(0)("othmaingrpcode")
                    End If


                    If IsDBNull(showDataRow(0)("othgrpcode")) = False Then
                        sqlstr = "select rtrim(ltrim(othgrpname))othgrpname,rtrim(ltrim(othgrpcode))othgrpcode from othgrpmast where active=1 and  othmaingrpcode='" & ddlExcursionGroup.Value & "' order by othgrpname"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionSubGroup, "othgrpname", "othgrpcode", sqlstr, True, hdnExcursionSubGroupCode.Value)
                        ddlExcursionSubGroup.Value = showDataRow(0)("othgrpcode")
                        hdnExcursionSubGroupCode.Value = showDataRow(0)("othgrpcode")
                    End If

                    If IsDBNull(showDataRow(0)("othtypcode")) = False Then
                        sqlstr = " select rtrim(ltrim(othtypname))othtypname,rtrim(ltrim(othtypcode))othtypcode from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & ddlExcursionGroup.Value & "' ) and a.active=1 order by a.othtypname"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionType, "othtypname", "othtypcode", sqlstr, True, hdnExcursionTypeCode.Value)
                        ddlExcursionType.Value = showDataRow(0)("othtypcode")
                        hdnExcursionTypeCode.Value = showDataRow(0)("othtypcode")
                    End If

                    'If IsDBNull(showDataRow(0)("epartycode")) = False Then

                    If ddlExcursionType.Value <> "[Select]" Then
                        sqlstr = "exec sp_get_exc_providers '" & ObjDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Text.Trim) & "','" & ObjDate.ConvertDateromTextBoxToTextYearMonthDay(txtTourDate.Text.Trim) & "','" & ddlExcursionType.Value & "'"

                    Else
                        sqlstr = " select P.partycode,p.partyname  from partymast p where P.sptypecode in (select option_selected from reservation_parameters where param_id ='1033')"

                    End If
                  
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExcursionProvider, "partyname", "partycode", sqlstr, True, hdnExcursionProvider.Value)

                    If ddlExcursionType.Value <> "[Select]" And IsDBNull(showDataRow(0)("epartycode")) = False Then
                        ddlExcursionProvider.Value = showDataRow(0)("epartycode")
                        hdnExcursionProvider.Value = showDataRow(0)("epartycode")
                    End If

                    'End If


                    If IsDBNull(showDataRow(0)("hotel")) = False Then
                        ddlHotel.Value = showDataRow(0)("hotel")
                        txthotel.Value = ddlHotel.Items(ddlHotel.SelectedIndex).Text
                    End If


                    If IsDBNull(showDataRow(0)("confirmed")) = False Then
                        ddlConfirmed.Value = showDataRow(0)("confirmed")
                    End If


                    If IsDBNull(showDataRow(0)("guide")) = False Then
                        ddlTourGuide.Value = showDataRow(0)("guide")
                    End If


                    If IsDBNull(showDataRow(0)("flighttype")) = False Then
                        If showDataRow(0)("flighttype") = "A" Then
                            ddlFlightType.Value = "1"
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightNo, "flightcode", "flight_tranid", "select ltrim(rtrim(flightcode))flightcode,ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=1 order by flightcode", True)
                        Else
                            ddlFlightType.Value = "0"
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightNo, "flightcode", "flight_tranid", "select ltrim(rtrim(flightcode))flightcode,ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=0 order by flightcode", True)
                        End If

                    End If

                    If IsDBNull(showDataRow(0)("flightno")) = False Then
                        'ddlFlightNo.Items(ddlFlightNo.SelectedIndex).Text = showDataRow(0)("flightno")
                        'hdnFlightNo.Value = ddlFlightNo.Items(ddlFlightNo.SelectedIndex).Value
                        If showDataRow(0)("flighttype") = "A" Then
                            ddlFlightNo.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=1 and flightcode='" & showDataRow(0)("flightno") & "'")
                            hdnFlightNo.Value = ddlFlightNo.Value
                        Else
                            ddlFlightNo.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ltrim(rtrim(flight_tranid))flight_tranid from flightmast where  active=1 and type=0 and flightcode='" & showDataRow(0)("flightno") & "'")
                            hdnFlightNo.Value = ddlFlightNo.Value
                        End If

                    End If







                    If IsDBNull(showDataRow(0)("amend")) = False Then
                        If showDataRow(0)("amend") = 1 Then
                            chkAmendment.Checked = True
                        ElseIf showDataRow(0)("amend") = 0 Then
                            chkAmendment.Checked = False
                        End If
                    End If

                    If IsDBNull(showDataRow(0)("cancel")) = False Then
                        If showDataRow(0)("cancel") = 1 Then
                            chkCancel.Checked = True
                        ElseIf showDataRow(0)("cancel") = 0 Then
                            chkCancel.Checked = False
                        End If
                    End If

                    If IsDBNull(showDataRow(0)("cancelreason")) = False Then
                        txtCancelReason.Text = showDataRow(0)("cancelreason")
                    End If



                    If IsDBNull(showDataRow(0)("complimentcust")) = False Then
                        If showDataRow(0)("complimentcust") = 1 Then
                            chkFreeToCustomer.Checked = True
                        ElseIf showDataRow(0)("complimentcust") = 0 Then
                            chkFreeToCustomer.Checked = False
                        End If
                    End If

                    If IsDBNull(showDataRow(0)("complimentprov")) = False Then
                        If showDataRow(0)("complimentprov") = 1 Then
                            chkFreeFromSupplier.Checked = True
                        ElseIf showDataRow(0)("complimentprov") = 0 Then
                            chkFreeFromSupplier.Checked = False
                        End If
                    End If


                    If IsDBNull(showDataRow(0)("spersoncomm")) = False Then
                        txtSPersonCom.Text = Math.Round(showDataRow(0)("spersoncomm"), CType(txtdecimal.Value, Integer))
                    End If





                    If IsDBNull(showDataRow(0)("guestname")) = False Then
                        txtGuestName.Text = showDataRow(0)("guestname")
                    End If

                    If IsDBNull(showDataRow(0)("roomno")) = False Then
                        txtRoomNo.Text = showDataRow(0)("roomno")

                    End If

                    If IsDBNull(showDataRow(0)("adults")) = False Then
                        txtAdult.Text = Math.Round(showDataRow(0)("adults"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("child")) = False Then
                        txtChild.Text = Math.Round(showDataRow(0)("child"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("attn")) = False Then
                        txtAttention.Text = showDataRow(0)("attn")
                    End If

                    If IsDBNull(showDataRow(0)("rateadults")) = False Then
                        txtAdultRate.Text = Math.Round(showDataRow(0)("rateadults"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("ratechild")) = False Then
                        txtChildRate.Text = Math.Round(showDataRow(0)("ratechild"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("amountAED")) = False Then
                        txtAmountAED.Text = Math.Round(showDataRow(0)("amountAED"), CType(txtdecimal.Value, Integer))
                        hdnAmountAED.Value = Math.Round(showDataRow(0)("amountAED"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("amount")) = False Then
                        txtAmount.Text = Math.Round(showDataRow(0)("amount"), CType(txtdecimal.Value, Integer))
                        hdnAmount.Value = Math.Round(showDataRow(0)("amount"), CType(txtdecimal.Value, Integer))
                    End If


                    If IsDBNull(showDataRow(0)("spersoncomm")) = False And showDataRow(0)("spersoncomm") <> 0 Then
                        txtSPersonComPer.Text = Math.Round((showDataRow(0)("spersoncomm") * 100) / showDataRow(0)("amountAED"), CType(txtdecimal.Value, Integer))
                    End If


                    'If IsDBNull(mySqlReader("spersoncode")) = False Then
                    '    ddlExcursionProvider.Value = mySqlReader("spersoncode")
                    'End If

                    If IsDBNull(showDataRow(0)("costRateAdult")) = False Then
                        txtAdultCostRate.Text = Math.Round(showDataRow(0)("costRateAdult"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("costRateChild")) = False Then
                        txtChildCostRate.Text = Math.Round(showDataRow(0)("costRateChild"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("costAmount")) = False Then
                        txtCostAmountAED.Text = Math.Round(showDataRow(0)("costAmount"), CType(txtdecimal.Value, Integer))
                        hdnCostAmountAED.Value = Math.Round(showDataRow(0)("costAmount"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("costAmountAED")) = False Then
                        txtCostAmount.Text = Math.Round(showDataRow(0)("costAmountAED"), CType(txtdecimal.Value, Integer))
                        hdnCostAmount.Value = Math.Round(showDataRow(0)("costAmountAED"), CType(txtdecimal.Value, Integer))
                    End If



                    If IsDBNull(showDataRow(0)("confno")) = False Then
                        txtConfirmationNo.Text = showDataRow(0)("confno")
                    End If

                    If IsDBNull(showDataRow(0)("incominginvno")) = False Then
                        txtIncomeInvoiceNo.Text = showDataRow(0)("incominginvno")
                    End If

                    If IsDBNull(showDataRow(0)("debitnoteno")) = False Then
                        txtDebitNoteNo.Text = showDataRow(0)("debitnoteno")
                    End If



                    If IsDBNull(showDataRow(0)("airport")) = False Then
                        txtAirport.Text = showDataRow(0)("airport")
                    End If

                    If IsDBNull(showDataRow(0)("arrival")) = False Then
                        txtArrivalDate.Text = Format(CType(showDataRow(0)("arrival"), Date), "dd/MM/yyyy")
                    End If

                    If IsDBNull(showDataRow(0)("departure")) = False Then
                        txtDepartureDate.Text = Format(CType(showDataRow(0)("departure"), Date), "dd/MM/yyyy")
                    End If

                    If IsDBNull(showDataRow(0)("pickuptime")) = False Then
                        txtPickTime.Text = showDataRow(0)("pickuptime")
                    End If

                    If IsDBNull(showDataRow(0)("flighttime")) = False Then
                        txtFlightTime.Text = showDataRow(0)("flighttime")
                    End If


                    If IsDBNull(showDataRow(0)("total_amount")) = False Then
                        txtTotalAmountAED.Text = Math.Round(showDataRow(0)("total_amount"), CType(txtdecimal.Value, Integer))
                    End If

                    If IsDBNull(showDataRow(0)("costCurrencyName")) = False Then
                        txtCostCurr.Text = showDataRow(0)("costCurrencyName")
                        hdnCostCurr.Value = showDataRow(0)("costCurrencyName")
                    End If

                    If IsDBNull(showDataRow(0)("costConvRate")) = False Then
                        txtCostConvRate.Text = showDataRow(0)("costConvRate")
                        hdnCostCurrConvRate.Value = showDataRow(0)("costConvRate")
                    End If

                    If IsDBNull(showDataRow(0)("remarks")) = False Then
                        txtRemarks.Text = showDataRow(0)("remarks")
                    End If


                    If IsDBNull(showDataRow(0)("supconf")) = False Then
                        If showDataRow(0)("supconf") = 1 Then
                            chksupconf.Checked = True
                            txtsupconfno.Style("display") = "block"
                            lblsupconf.Style("display") = "block"
                            txtsupconfno.Text = showDataRow(0)("supconfno")
                        ElseIf showDataRow(0)("supconf") = 0 Then
                            chksupconf.Checked = False
                            txtsupconfno.Style("display") = "none"
                            lblsupconf.Style("display") = "none"
                        End If
                    End If

                    If IsDBNull(showDataRow(0)("trf_required")) = False Then
                        If showDataRow(0)("trf_required") = 1 Then
                            chktrfreq.Checked = True
                        Else
                            chktrfreq.Checked = False
                        End If

                    End If


                    If IsDBNull(showDataRow(0)("cancelcharges")) = False Then
                        txtcancelcharges.Text = showDataRow(0)("cancelcharges")
                    End If
                    If IsDBNull(showDataRow(0)("noshowcharges")) = False Then
                        txtnoshowcharges.Text = showDataRow(0)("noshowcharges")
                    End If
                    If IsDBNull(showDataRow(0)("canceldeadline")) = False Then
                        txtcaneldeadline.Text = showDataRow(0)("canceldeadline")
                    End If
                    If IsDBNull(showDataRow(0)("waitingpolicy")) = False Then
                        txtwaiting.Text = showDataRow(0)("waitingpolicy")
                    End If


                    txtprotktno.Text = showDataRow(0)("protktno")


                    If IsDBNull(showDataRow(0)("reminderdt")) = False Then
                        txtreminder.Text = Format(CType(showDataRow(0)("reminderdt"), Date), "dd/MM/yyyy")
                    End If


                    If IsDBNull(showDataRow(0)("exctime")) = False Then
                        txtexctime.Text = showDataRow(0)("exctime")
                    End If

                    If IsDBNull(showDataRow(0)("trfno")) = False Then
                        If showDataRow(0)("trfno") <> "" Then
                            txttrfno.Style("display") = "block"
                            lbltransferid.Style("display") = "block"
                            txttrfno.Text = showDataRow(0)("trfno")

                            '27082014
                            txtsupplier.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast p inner join transfer_booking_subdetail sub on p.partycode=sub.suppliercode where   sub.requestid='" & showDataRow(0)("trfno") & "'")
                            txtdriver.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select drivername from drivermaster d inner join transfer_booking_subdetail sub on d.drivercode=sub.drivercode where   sub.requestid='" & showDataRow(0)("trfno") & "'")
                            txtmob.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select mobileno from drivermaster d inner join transfer_booking_subdetail sub on d.drivercode=sub.drivercode where   sub.requestid='" & showDataRow(0)("trfno") & "'")

                            If txtsupplier.Text <> "" Then

                                txtsupplier.Style("display") = "block"
                                lblsupplier.Style("display") = "block"
                            Else
                                txtsupplier.Style("display") = "none"
                                lblsupplier.Style("display") = "none"
                            End If

                            If txtdriver.Text <> "" Then
                                txtdriver.Style("display") = "block"
                                txtmob.Style("display") = "block"
                                lbldriver.Style("display") = "block"
                                lblmob.Style("display") = "block"
                            Else
                                txtdriver.Style("display") = "none"
                                txtmob.Style("display") = "none"
                                lbldriver.Style("display") = "none"
                                lblmob.Style("display") = "none"
                            End If

                        End If

                    End If

                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))


        End Try
    End Sub

    Protected Sub gv_Flight_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Flight.RowCommand
        Try
            If e.CommandName = "Page" Then
                Exit Sub
            End If

            Dim lblFlightNo, lblFlightTranID As Label
            lblFlightNo = gv_Flight.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblFlightNo")
            lblFlightTranID = gv_Flight.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblFlightTranID")

            If e.CommandName = "SelectRow" Then
                Session("RowState") = ""
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "FillFlightDetails('" & lblFlightTranID.Text.Trim & "');", True)
            End If

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("ExcursionsRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub FillFlight_Details()
        Try


            Dim MyDs As New DataTable
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            '1-Arrival 0-Departure
            If ddlFlightType.Value = "1" Then
                'strSqlQry = "select ltrim(rtrim(flightcode))flightcode,flight_tranid,city,arrivetime1,airport from flightmast where  active=1 and type=1 "
                strSqlQry = " select ltrim(rtrim(flightcode))flightcode,min(flight_tranid)flight_tranid,min(city)city,min(arrivetime1)arrivetime1,min(airport)airport from flightmast where active=1 and type=1 group by flightcode "
            Else
                'strSqlQry = "select ltrim(rtrim(flightcode))flightcode,flight_tranid,city,Departtime1,airport from flightmast where  active=1 and type=0 "
                strSqlQry = " select ltrim(rtrim(flightcode))flightcode,min(flight_tranid)flight_tranid,min(city)city,min(Departtime1)Departtime1,min(airport)airport from flightmast where  active=1 and type=0 group by flightcode "
            End If


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & BuildCondition() & " order by flightcode "
            Else
                strSqlQry = strSqlQry & " order by flightcode "
            End If

            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(MyDs)

            If MyDs.Rows.Count > 0 Then
                gv_Flight.DataSource = MyDs
                gv_Flight.DataBind()
                gv_Flight.Visible = True
                lblMsg.Visible = False
            Else
                gv_Flight.Visible = False
                lblMsg.Visible = True
            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub Fillticket_Details()
        Try
            Dim lblticketno As Label
            Dim chkselect As HtmlInputCheckBox
            Dim gvrow As GridViewRow
            Dim str As String = ""
            str = txtprotktno.Text
            For Each gvrow In gvticketdet.Rows
                lblticketno = gvrow.FindControl("lblticketno")
                chkselect = gvrow.FindControl("chkselect")

                If chkselect.Checked = True Then
                    str = str + lblticketno.Text + ","
                End If
            Next

            Dim MyDs As New DataTable
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            '1-Arrival 0-Departure

            'If txtSearch1.Text <> "" Then
            '    strSqlQry = " select  ltrim(rtrim(ticketno))ticketno from excursion_tickets_subdetail where isnull(assignedto,'''')='''' and ticketno like '%" & txtSearch1.Text & "%'   order by ticketno"

            'Else
            '    strSqlQry = " select  ltrim(rtrim(ticketno))ticketno from excursion_tickets_subdetail where isnull(assignedto,'''')=''''  order by ticketno"

            'End If

            strSqlQry = "sp_show_selec_ticket '" & str & "','" & txtSearch1.Text & "','" & ddlExcursionSubGroup.Items(ddlExcursionSubGroup.SelectedIndex).Value & "','" & ddlExcursionType.Items(ddlExcursionType.SelectedIndex).Value & "'"


            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(MyDs)

            If MyDs.Rows.Count > 0 Then
                gvticketdet.DataSource = MyDs
                gvticketdet.DataBind()
                gvticketdet.Visible = True
                'lblMsg.Visible = False
            Else
                gvticketdet.Visible = False
                'lblMsg.Visible = True
            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Function BuildCondition() As String

        strWhereCond = ""

        If ddlSearchType.Value = "1" Then

            If txtSearch.Text.Trim <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " having min(city) LIKE '" & Trim(txtSearch.Text.Trim.ToUpper) & "%'"
                End If

            End If
        End If

        If ddlSearchType.Value = "0" Then

            If txtSearch.Text.Trim <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " having min(airport) LIKE '" & Trim(txtSearch.Text.Trim.ToUpper) & "%'"
                End If

            End If
        End If


        If ddlSearchType.Value = "2" Then

            If txtSearch.Text.Trim <> "" Then
                If Trim(strWhereCond) = "" Then
                    If ddlFlightType.Value = "1" Then
                        strWhereCond = " having min(arrivetime1) LIKE '" & Trim(txtSearch.Text.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond = " having min(Departtime1) LIKE '" & Trim(txtSearch.Text.Trim.ToUpper) & "%'"
                    End If

                End If

            End If
        End If

        If ddlSearchType.Value = "3" Then

            If txtSearch.Text.Trim <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " having flightcode LIKE '" & Trim(txtSearch.Text.Trim.ToUpper) & "%'"
                End If

            End If
        End If


        BuildCondition = strWhereCond
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try

            FillFlight_Details()
            ModalPopupDays.Show()

            'ShowFlightHelp.Style("display") = "block"
            'ShowFlightHelp.Style("position") = "absolute"
            'ShowFlightHelp.Style("top") = "100px"
            'ShowFlightHelp.Style("left") = "400px"
            'ShowFlightHelp.Style("z-index") = "100px"
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub btnGetGlight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetGlight.Click
        FillFlight_Details()
        ModalPopupDays.Show()
    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

    End Sub

    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselect.Click
        Fillticket_Details()
        Modalpopuptkts.Show()
        'ModalPopupDays.Hide()
    End Sub

    Protected Sub btnsearch1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch1.Click
        Fillticket_Details()
        Modalpopuptkts.Show()
        'ModalPopupDays.Hide()
    End Sub

    Protected Sub btnselect1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselect1.Click
        Dim lblticketno As Label
        Dim chkselect As HtmlInputCheckBox
        Dim gvrow As GridViewRow
        Dim str As String = ""
        For Each gvrow In gvticketdet.Rows
            lblticketno = gvrow.FindControl("lblticketno")
            chkselect = gvrow.FindControl("chkselect")

            If chkselect.Checked = True Then
                str = str + lblticketno.Text + ","
            End If
        Next

        txtprotktno.Text = str

    End Sub

    Protected Sub btnMultipleCost_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mExcID As String = Session("Excursion_ID")
        Dim mRlineNo As String = Session("ExcursionRequestSubEntryLineNo") 'Session("Rlineno")

        ddlExcursionProvider.Value = hdnExcursionProvider.Value
        If ddlExcursionProvider.Value <> "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value sholud be zero, Please choose [Select] from the excursion provider.');", True)
        Else
            wucExcSumSubEntry.ExcursionType = ddlExcursionType.Value
            wucExcSumSubEntry.Adult = Val(txtAdult.Text)
            wucExcSumSubEntry.Child = Val(txtChild.Text)
            wucExcSumSubEntry.TourDate = txtTourDate.Text
            Dim mState As String = CType(Session("ExcursionRequestSubEntryState"), String)
            wucExcSumSubEntry.BindGrid(mExcID, mRlineNo, mState)
            ModalPopupMultipleCost.Show()
        End If
    End Sub


    Protected Sub btnMCAddRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMCAddRow.Click
        ModalPopupMultipleCost.Show()
        Dim mExcID As String = Session("Excursion_ID")
        Dim mRlineNo As String = Session("ExcursionRequestSubEntryLineNo") ' Session("Rlineno")
        wucExcSumSubEntry.ExcursionType = ddlExcursionType.Value
        wucExcSumSubEntry.Adult = Val(txtAdult.Text)
        wucExcSumSubEntry.Child = Val(txtChild.Text)
        wucExcSumSubEntry.TourDate = txtTourDate.Text

        wucExcSumSubEntry.AddRow()
    End Sub

    Protected Sub btnMCDelRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMCDelRow.Click
        ModalPopupMultipleCost.Show()
        Dim mExcID As String = Session("Excursion_ID")
        Dim mRlineNo As String = Session("ExcursionRequestSubEntryLineNo") 'Session("Rlineno")
        wucExcSumSubEntry.ExcursionType = ddlExcursionType.Value
        wucExcSumSubEntry.Adult = Val(txtAdult.Text)
        wucExcSumSubEntry.Child = Val(txtChild.Text)
        wucExcSumSubEntry.TourDate = txtTourDate.Text
        wucExcSumSubEntry.DeleteRow()
    End Sub

    Protected Sub btnMCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        wucExcSumSubEntry.ExcursionType = ddlExcursionType.Value
        wucExcSumSubEntry.IsGridUpdated = True
        Dim mExcID As String = Session("Excursion_ID")
        Dim mRlineNo As String = Session("ExcursionRequestSubEntryLineNo")
        If wucExcSumSubEntry.ValidateExcursionCostDetails Then
            If wucExcSumSubEntry.SaveMultipleCost() Then
                ModalPopupMultipleCost.Hide()
                ''' Added shahul 13/10/2015
                hdnmulticostcheck.Value = 0
                ''''''''''''
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total value of the multi cost should be greater than zero.');", True)
                ModalPopupMultipleCost.Show()
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please follow this validations and Save.\n1. Each and every row should have supplier if the cost value is greater than zero.\n2. Adult,Child cost rate should not be zero if the adult and cost count is greater than zero.\n3.Adult cost rate should not be equal to zero, if the no of units value is greater than one.');", True)
            ModalPopupMultipleCost.Show()
        End If
    End Sub

    Protected Sub wucExcSumSubEntry_PassTotalValue(ByVal Totalval As String) Handles wucExcSumSubEntry.PassTotalValue
        txtCostAmount.Text = Totalval

        If Val(Totalval) > 0 Then
            hdnIsMultipleCost.Value = 1
            ddlExcursionProvider.Attributes.Add("disabled", "disabled") '//Changed by Riswan on 12/04/2015
        Else
            hdnIsMultipleCost.Value = 0
            ddlExcursionProvider.Attributes.Remove("disabled") '//Changed by Riswan on 12/04/2015
        End If
        hdnTotalMC.Value = Totalval
        hdnCostAmount.Value = Totalval
        hdnCostAmountAED.Value = Totalval
        txtCostAmountAED.Text = Totalval
        txtCostConvRate.Text = "1.0000000"
        hdnCostCurrConvRate.Value = "1.0000000"
        txtAdultCostRate.Text = 0
        txtChildCostRate.Text = 0
        ddlExcursionProvider.Value = "[Select]"
    End Sub

    Protected Sub btnSubEntryClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubEntryClose.Click
        'Dim grdMultipleCost As GridView
        'grdMultipleCost = DirectCast(wucExcSumSubEntry.FindControl(""), GridView)
        wucExcSumSubEntry.IsGridUpdated = False
        'wucExcSumSubEntry.SaveMultipleCost(True)
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function GetExcTypeCode(ByVal constr As String, ByVal supplier As String) As String
        Dim query As String = "SELECT sptypecode FROM partymast Where partycode ='" & supplier & "'"
        Dim ds As New DataSet
        Dim mObjUtilities As New clsUtils
        ds = mObjUtilities.ExecuteQuerySqlnew(constr, query)
        If IsDBNull(ds.Tables(0).Rows(0).Item("sptypecode")) Then
            Return String.Empty
        Else
            Return ds.Tables(0).Rows(0).Item("sptypecode").ToString
        End If
    End Function

End Class


