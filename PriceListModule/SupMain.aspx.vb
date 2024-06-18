Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Partial Class SupMain
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim objUser As New clsUser
    Shared sectorvalue As Integer
    Dim myDataAdapter As SqlDataAdapter
    Dim SqlConn As SqlConnection
    Dim ExtAppIDflag As String  'implementing for external app id eg;juniper id
    Dim ExtAppIDexist As Boolean
#End Region

#Region "Tax Details"
    Protected Sub ddlTax_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        DisplayTaxOptionsOnChange(ddlTax.SelectedValue)
    End Sub

    Protected Sub DisplayTaxOptionsOnChange(ByVal taxDetails As String)
        Dim state As String = CType(Request.QueryString("State"), String)

        If (taxDetails = "Taxable") Then
            If (CType(Request.QueryString("type"), String) = "HOT") Then
                lblservicecharges.Visible = True
                lblmncpltyfees.Visible = True
                lbltourismfees.Visible = True
                lblvtax.Visible = True

                txtServiceCharges.Visible = True
                TxtMunicipalityFees.Visible = True
                txtTourismFees.Visible = True
                txtVAT.Visible = True
                txtVAT.Disabled = False

                'If (state <> "Edit") Then
                txtVAT.Value = ""
                'End If
                lblperc1.Visible = True
                lblperc2.Visible = True
                lblperc3.Visible = True
                lblperc4.Visible = True

            Else
                lblservicecharges.Visible = False
                lblmncpltyfees.Visible = False
                lbltourismfees.Visible = False
                lblvtax.Visible = True

                txtServiceCharges.Visible = False
                TxtMunicipalityFees.Visible = False
                txtTourismFees.Visible = False
                txtVAT.Visible = True
                txtVAT.Disabled = False
                'If (state <> "Edit") Then
                txtVAT.Value = ""
                txtServiceCharges.Value = 0
                TxtMunicipalityFees.Value = 0
                txtTourismFees.Value = 0
                'End If


                lblperc1.Visible = False
                lblperc2.Visible = False
                lblperc3.Visible = False
                lblperc4.Visible = True

            End If


        ElseIf (taxDetails = "ZeroRated") Or (taxDetails = "NonRegistered") Or (taxDetails = "RC") Then 'Tanvir 30032022
            lblservicecharges.Visible = False
            lblmncpltyfees.Visible = False
            lbltourismfees.Visible = False
            lblvtax.Visible = True

            txtServiceCharges.Visible = False
            TxtMunicipalityFees.Visible = False
            txtTourismFees.Visible = False
            txtVAT.Visible = True
            'If (state <> "Edit") Then
            txtVAT.Value = "0"
            txtVAT.Disabled = True
            txtServiceCharges.Value = 0
            TxtMunicipalityFees.Value = 0
            txtTourismFees.Value = 0
            'End If


            lblperc1.Visible = False
            lblperc2.Visible = False
            lblperc3.Visible = False
            lblperc4.Visible = True

        Else
            lblservicecharges.Visible = False
            lblmncpltyfees.Visible = False
            lbltourismfees.Visible = False
            lblvtax.Visible = False

            txtServiceCharges.Visible = False
            TxtMunicipalityFees.Visible = False
            txtTourismFees.Visible = False
            txtVAT.Visible = False
            txtVAT.Disabled = False
            'If (state <> "Edit") Then
            txtVAT.Value = ""
            txtServiceCharges.Value = 0
            TxtMunicipalityFees.Value = 0
            txtTourismFees.Value = 0
            'End If


            lblperc1.Visible = False
            lblperc2.Visible = False
            lblperc3.Visible = False
            lblperc4.Visible = False

        End If

    End Sub

    Protected Sub DisplayTaxOptions(ByVal taxDetails As String)
        Dim state As String = CType(Request.QueryString("State"), String)

        If (taxDetails = "Taxable") Then
            If (CType(Request.QueryString("type"), String) = "HOT") Then
                lblservicecharges.Visible = True
                lblmncpltyfees.Visible = True
                lbltourismfees.Visible = True
                lblvtax.Visible = True

                txtServiceCharges.Visible = True
                TxtMunicipalityFees.Visible = True
                txtTourismFees.Visible = True
                txtVAT.Visible = True
                txtVAT.Disabled = False
                If (state = "New") Then
                    txtVAT.Value = ""
                End If
                lblperc1.Visible = True
                lblperc2.Visible = True
                lblperc3.Visible = True
                lblperc4.Visible = True

            Else
                lblservicecharges.Visible = False
                lblmncpltyfees.Visible = False
                lbltourismfees.Visible = False
                lblvtax.Visible = True

                txtServiceCharges.Visible = False
                TxtMunicipalityFees.Visible = False
                txtTourismFees.Visible = False
                txtVAT.Visible = True
                txtVAT.Disabled = False
                If (state = "New") Then
                    txtVAT.Value = ""
                    txtServiceCharges.Value = 0
                    TxtMunicipalityFees.Value = 0
                    txtTourismFees.Value = 0
                End If


                lblperc1.Visible = False
                lblperc2.Visible = False
                lblperc3.Visible = False
                lblperc4.Visible = True

                End If


        ElseIf (taxDetails = "ZeroRated") Then
                lblservicecharges.Visible = False
                lblmncpltyfees.Visible = False
                lbltourismfees.Visible = False
                lblvtax.Visible = True

                txtServiceCharges.Visible = False
                TxtMunicipalityFees.Visible = False
            txtTourismFees.Visible = False
            txtVAT.Disabled = True
            txtVAT.Visible = True
            If (state = "New") Then
                txtVAT.Value = "0"

                txtServiceCharges.Value = 0
                TxtMunicipalityFees.Value = 0
                txtTourismFees.Value = 0
            End If


            lblperc1.Visible = False
            lblperc2.Visible = False
            lblperc3.Visible = False
            lblperc4.Visible = True

            Else
                lblservicecharges.Visible = False
                lblmncpltyfees.Visible = False
                lbltourismfees.Visible = False
                lblvtax.Visible = False

                txtServiceCharges.Visible = False
                TxtMunicipalityFees.Visible = False
                txtTourismFees.Visible = False
            txtVAT.Visible = False
            txtVAT.Disabled = False
            If (state = "New") Then
                txtVAT.Value = ""
                txtServiceCharges.Value = 0
                TxtMunicipalityFees.Value = 0
                txtTourismFees.Value = 0
            End If


            lblperc1.Visible = False
            lblperc2.Visible = False
            lblperc3.Visible = False
            lblperc4.Visible = False

        End If

    End Sub

#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CurrCount As Integer
        Dim ctryCount As Integer
        Dim catCount As Integer
        Dim htlCount As Integer
        Dim secCount As Integer
        Dim cityCount As Integer

        Dim CurrlngCount As Int16
        Dim CtrylngCount As Int16
        Dim seclngCount As Int16
        Dim catlngCount As Int16
        Dim citylngCount As Int16
        Dim htllngCount As Int16

        Dim strappname As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim strsecTempUserFunctionalRight As String()
        Dim strctryTempUserFunctionalRight As String()
        Dim strcityTempUserFunctionalRight As String()
        Dim strcatTempUserFunctionalRight As String()
        Dim strhtlTempUserFunctionalRight As String()


        Dim strcityRights As String
        Dim strctryRights As String
        Dim strsecRights As String
        Dim strcatRights As String
        Dim strhtlRights As String
        Dim strCurrRights As String

        ViewState.Add("appid", Request.QueryString("appid"))
        Dim strappid As String = ViewState("appid")


        Dim groupid As String = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))

        Dim menuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CurrenciesSearch.aspx?appid=" + strappid, strappid)
        Dim functionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, menuid)

        Dim ctrymenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CountriesSearch.aspx?appid=" + strappid, strappid)
        Dim ctryfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, ctrymenuid)


        Dim catmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\SuppliercategoriesSearch.aspx?appid=" + strappid, strappid)
        Dim catfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, catmenuid)

        Dim citymenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CitiesSearch.aspx?appid=" + strappid, strappid)
        Dim cityfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, citymenuid)


        Dim secmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\SectorSearch.aspx?appid=" + strappid, strappid)
        Dim secfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, secmenuid)

        Dim hotelmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\HotelChainMaster.aspx?appid=" + strappid, strappid)
        Dim hotelfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, hotelmenuid)




        If functionalrights <> "" Then
            strTempUserFunctionalRight = functionalrights.Split(";")
            For CurrlngCount = 0 To strTempUserFunctionalRight.Length - 1
                strCurrRights = strTempUserFunctionalRight.GetValue(CurrlngCount)

                If strCurrRights = "01" Then
                    CurrCount = 1
                End If
            Next
            If CurrCount = 1 Then
                btnCurrency.Visible = True
            Else
                btnCurrency.Visible = False
            End If
        End If
        If ctryfunctionalrights <> "" Then
            strctryTempUserFunctionalRight = ctryfunctionalrights.Split(";")
            For CtrylngCount = 0 To strctryTempUserFunctionalRight.Length - 1
                strctryRights = strctryTempUserFunctionalRight.GetValue(CtrylngCount)

                If strctryRights = "01" Then
                    ctryCount = 1
                End If
            Next
            If ctryCount = 1 Then
                btnCountry.Visible = True
            Else
                btnCountry.Visible = False
            End If
        End If
        If cityfunctionalrights <> "" Then
            strcityTempUserFunctionalRight = cityfunctionalrights.Split(";")
            For citylngCount = 0 To strcityTempUserFunctionalRight.Length - 1
                strcityRights = strcityTempUserFunctionalRight.GetValue(citylngCount)

                If strcityRights = "01" Then
                    cityCount = 1
                End If
            Next
            If cityCount = 1 Then
                btnCity.Visible = True
            Else
                btnCity.Visible = False
            End If
        End If

        If secfunctionalrights <> "" Then
            strsecTempUserFunctionalRight = secfunctionalrights.Split(";")
            For seclngCount = 0 To strsecTempUserFunctionalRight.Length - 1
                strsecRights = strsecTempUserFunctionalRight.GetValue(seclngCount)

                If strsecRights = "01" Then
                    secCount = 1
                End If
            Next
            If secCount = 1 Then
                btnSector.Visible = True
            Else
                btnSector.Visible = False
            End If
        End If
        If catfunctionalrights <> "" Then

            strcatTempUserFunctionalRight = catfunctionalrights.Split(";")
            For catlngCount = 0 To strcatTempUserFunctionalRight.Length - 1
                strcatRights = strcatTempUserFunctionalRight.GetValue(catlngCount)

                If strcatRights = "01" Then
                    catCount = 1
                End If
            Next
            If catCount = 1 Then
                btnCategory.Visible = True
            Else
                btnCategory.Visible = False
            End If

        End If


        If hotelfunctionalrights <> "" Then
            strhtlTempUserFunctionalRight = hotelfunctionalrights.Split(";")
            For htllngCount = 0 To strhtlTempUserFunctionalRight.Length - 1
                strhtlRights = strhtlTempUserFunctionalRight.GetValue(htllngCount)

                If strhtlRights = "01" Then
                    htlCount = 1
                End If
            Next
            If htlCount = 1 Then
                btnHotelChain.Visible = True
            Else
                btnHotelChain.Visible = False
            End If
        End If

        'Dim Supmenuidval As String = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\HotelGroups.aspx?appid=" + strappid, strappid)
        'Dim dt As New DataTable
        'strSqlQry = "select submenuid from group_submenurights where menuid='" + Supmenuidval + "' and active=0"
        'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        'myDataAdapter.Fill(dt)

        Dim RefCode As String
        'Session("SupRefCode") = txtCode.Value
        If CType(Request.QueryString("appid"), String) = "11" Or CType(Request.QueryString("appid"), String) = "10" Or CType(Request.QueryString("appid"), String) = "13" Then
            lblhotelchain.Visible = "false"
            txthotelchainname.Visible = "false"
            lblhotelstatus.Visible = "false"
            txthotelstatusname.Visible = "false"
            Label1.Visible = "false"
            txtpropertytypename.Visible = "false"
            btnHotelChain.Style.Add("display", "none")
            lblvat.Visible = False
            chkvat.Visible = False
        ElseIf (CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) <> "OTH") Then
            lblhotelchain.Visible = "true"
            txthotelchainname.Visible = "true"
            lblhotelstatus.Visible = "true"
            txthotelstatusname.Visible = "true"
            Label1.Visible = "true"
            txtpropertytypename.Visible = "true"
            btnHotelChain.Style.Add("display", "block")
            lblvat.Visible = True
            chkvat.Visible = True
        ElseIf (CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH") Then
            lblhotelchain.Visible = "false"
            txthotelchainname.Visible = "false"
            lblhotelstatus.Visible = "false"
            txthotelstatusname.Visible = "false"
            Label1.Visible = "false"
            txtpropertytypename.Visible = "false"
            btnHotelChain.Style.Add("display", "none")
            lblvat.Visible = False
            chkvat.Visible = False

        ElseIf ((CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH")) Then
            lblhotelchain.Visible = "false"
            txthotelchainname.Visible = "false"
            lblhotelstatus.Visible = "false"
            txthotelstatusname.Visible = "false"
            Label1.Visible = "false"
            txtpropertytypename.Visible = "false"
            btnHotelChain.Style.Add("display", "none")
            lblvat.Visible = False
            chkvat.Visible = False
            IndexChanged()

        ElseIf ((CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA") Or (CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA")) Then
            lblhotelchain.Visible = "false"
            txthotelchainname.Visible = "false"
            lblhotelstatus.Visible = "false"
            txthotelstatusname.Visible = "false"
            Label1.Visible = "false"
            txtpropertytypename.Visible = "false"
            btnHotelChain.Style.Add("display", "none")
            lblvat.Visible = False
            chkvat.Visible = False
            IndexChanged()
        ElseIf ((CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS") Or (CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS")) Then
            lblhotelchain.Visible = "false"
            txthotelchainname.Visible = "false"
            lblhotelstatus.Visible = "false"
            txthotelstatusname.Visible = "false"
            Label1.Visible = "false"
            txtpropertytypename.Visible = "false"
            btnHotelChain.Style.Add("display", "none")
            lblvat.Visible = False
            chkvat.Visible = False

            If Not Page.IsPostBack Then
                ddlTax.SelectedValue = "Exempted"
                txtVAT.Value = 0
            End If

            'ddlTax.Enabled = False
            lblservicecharges.Visible = "false"
            lblmncpltyfees.Visible = "false"
            lbltourismfees.Visible = "false"
            lblvtax.Visible = "false"

            txtServiceCharges.Visible = "false"
            TxtMunicipalityFees.Visible = "false"
            txtTourismFees.Visible = "false"
            txtVAT.Visible = "false"


            lblperc1.Visible = "false"
            lblperc2.Visible = "false"
            lblperc3.Visible = "false"
            lblperc4.Visible = "false"

            txtServiceCharges.Value = 0
            TxtMunicipalityFees.Value = 0
            txtTourismFees.Value = 0


        ElseIf ((CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC") Or (CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC")) Then
            lblhotelchain.Visible = "false"
            txthotelchainname.Visible = "false"
            lblhotelstatus.Visible = "false"
            txthotelstatusname.Visible = "false"
            Label1.Visible = "false"
            txtpropertytypename.Visible = "false"
            btnHotelChain.Style.Add("display", "none")
            lblvat.Visible = False
            chkvat.Visible = False

            IndexChanged()

            'ddlTax.Enabled = True
            'lblservicecharges.Visible = False
            'lblmncpltyfees.Visible = False
            'lbltourismfees.Visible = False
            'lblvtax.Visible = True

            'txtServiceCharges.Visible = False
            'TxtMunicipalityFees.Visible = False
            'txtTourismFees.Visible = False
            'txtVAT.Visible = True
            'txtVAT.Value = ""

            'lblperc1.Visible = False
            'lblperc2.Visible = False
            'lblperc3.Visible = False
            'lblperc4.Visible = True
        End If
        If IsPostBack = False Then
            '  Session("partycode") = Nothing

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)


            If CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) <> "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)
                Session("supmain_suptype") = "HOT"
                Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 39)
                Session("supmain_suptype") = "OtherServ"
            ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1033")
            ElseIf CType(Request.QueryString("appid"), String) = "10" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=564")
            ElseIf CType(Request.QueryString("appid"), String) = "13" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1032")
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "HOT" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=HOT", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "HOT" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=HOT", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH" Then
                Session("supmain_suptype") = "OtherServ"
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH" Then
                Session("supmain_suptype") = "OtherServ"
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1032")
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1032")
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=564")
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=564")
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1033")
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1033")
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))


            End If


            If CType(Request.QueryString("type"), String) = "EXC" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1033")
            ElseIf CType(Request.QueryString("type"), String) = "TRFS" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=564")
                ''changed to match filters from accounts 
                ' 'Session("supmain_suptype") = "HOT"
            ElseIf CType(Request.QueryString("type"), String) = "HOT" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=458")

            ElseIf CType(Request.QueryString("type"), String) = "VISA" Then
                Session("supmain_suptype") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1032")
            End If
            ExtAppIDflag = "N" 'implementing for external app id eg;juniper id
            ExtAppIDexist = False

            ExtAppIDflag = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=5503")

            If ExtAppIDflag.ToUpper = "Y" Then
                LblExtappid.Visible = True
                TxtExtappid.Visible = True
                Extappspan.Visible = True

                LblExtappid.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=5504")


            Else
                LblExtappid.Visible = False
                TxtExtappid.Visible = False
                LblExtappid.Text = ""
                Extappspan.Visible = False



            End If

            ' Session("partycode") = Nothing
            PanelMain.Visible = True
            charcters(txtCode)
            charcters(txtName)
            txtconnection.Value = Session("dbconnectionName")
            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            If Session("SupRefCode") Is Nothing Or CType(Session("SupRefCode"), String) = "" Then
                Session("SupRefCode") = CType(Session("partycode"), String)
            End If
            If CType(Session("SupState"), String) = "New" Then

                SetFocus(txtName)
                lblHeading.Text = "Add New Supplier" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + PanelMain.GroupingText
                txtCode.Disabled = True
                txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rnkorder),0) from partymast") + 1
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
            ElseIf CType(Session("SupState"), String) = "Copy" Then
                RefCode = CType(Session("SupRefCode"), String)
                SetFocus(txtName)
                lblHeading.Text = "Copy Supplier" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "Copy Supplier Master" + " - " + PanelMain.GroupingText
                txtCode.Disabled = True
                ' RefCode = CType(Session("SupRefCode"), String)
                If Session("partycode") Is Nothing Then
                    RefCode = CType(Session("SupRefCode"), String)
                Else
                    RefCode = CType(Session("partycode"), String)
                End If

                ShowRecord(RefCode)
                txtCode.Value = ""
                txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rnkorder),0) from partymast") + 1
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                RefCode = CType(Session("SupRefCode"), String)
                btnSave.Text = "Update"
                If Session("partycode") Is Nothing Then
                    RefCode = CType(Session("SupRefCode"), String)
                Else
                    RefCode = CType(Session("partycode"), String)
                End If

                ShowRecord(RefCode)
                txtCode.Disabled = True
                SetFocus(txthotelname)

                If ExtAppIDflag.ToUpper = "Y" Then


                    ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_partymast", "partycode", txtCode.Value)
                    If ExtAppIDexist Then
                        TxtExtappid.Disabled = True
                        TxtExtappid.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_partyCode  from int_partymast where partycode='" & txtCode.Value & "'")
                    Else
                        TxtExtappid.Disabled = False
                    End If
                End If
                lblHeading.Text = "Edit Supplier" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + PanelMain.GroupingText
                DisableControl()
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
            ElseIf CType(Session("SupState"), String) = "View" Then

                RefCode = CType(Session("SupRefCode"), String)

                If Session("partycode") Is Nothing Then
                    RefCode = CType(Session("SupRefCode"), String)
                Else
                    RefCode = CType(Session("partycode"), String)
                End If
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                If ExtAppIDflag.ToUpper = "Y" Then

                    TxtExtappid.Disabled = True
                    ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_partymast", "partycode", txtCode.Value)
                    If ExtAppIDexist Then

                        TxtExtappid.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_partyCode  from int_partymast where partycode='" & txtCode.Value & "'")

                    End If
                End If
                DisableControl()
                lblHeading.Text = "View Supplier" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + PanelMain.GroupingText
                btnSave.Visible = False
                btnCancel.Text = "Return to Search"
                btnCancel.Focus()
            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)

                If Session("partycode") Is Nothing Then
                    RefCode = CType(Session("SupRefCode"), String)
                Else
                    RefCode = CType(Session("partycode"), String)
                End If
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                If ExtAppIDflag.ToUpper = "Y" Then

                    TxtExtappid.Disabled = True
                    ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_partymast", "partycode", txtCode.Value)
                    If ExtAppIDexist Then

                        TxtExtappid.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_partyCode  from int_partymast where partycode='" & txtCode.Value & "'")


                    End If
                End If
                lblHeading.Text = "Delete Supplier" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + PanelMain.GroupingText
                btnSave.Text = "Delete"
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                DisableControl()
            End If
        Else
            'If Session("addcategory") = "new" Then
            '    Dim categoryname As String = Request.QueryString("categoryname")
            '    txtcategoryname.Text = categoryname
            '    Session.Remove("addcategory")
            'End If
            Try
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierAgentsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SupcatsfromWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("SupCategoryName") IsNot Nothing Then
                    If Session("SupCategoryCode") IsNot Nothing Then
                        Dim categoryname As String = Session("SupCategoryName")
                        txtcategorycode.Text = Session("SupCategoryCode")
                        txtcategoryname.Text = categoryname
                        Session.Remove("addcategory")
                        Session.Remove("SupCategoryName")
                        Session.Remove("SupCategoryCode")
                    End If
                End If
                If Session("CurrName") IsNot Nothing Then
                    If Session("CurrCode") IsNot Nothing Then
                        Dim currencyname As String = Session("CurrName")
                        txtcurrencycode.Text = Session("CurrCode")
                        txtcurrencyname.Text = currencyname
                        Session.Remove("addcategory")
                        Session.Remove("CurrName")
                        Session.Remove("CurrCode")
                    End If
                End If
                If Session("CitiesName") IsNot Nothing Then
                    If Session("CitiesCode") IsNot Nothing Then
                        Dim cityname As String = Session("CitiesName")
                        txtcitycode.Text = Session("CitiesCode")
                        txtcityname.Text = cityname
                        Session.Remove("addcategory")
                        Session.Remove("CitiesName")
                        Session.Remove("CitiesCode")
                    End If
                End If
                If Session("HotelName") IsNot Nothing Then
                    If Session("HotelCode") IsNot Nothing Then
                        Dim hotelname As String = Session("HotelName")
                        txthotelchainname.Text = hotelname
                        txthotelchaincode.Text = Session("HotelCode")
                        Session.Remove("addcategory")
                        Session.Remove("HotelName")
                        Session.Remove("HotelCode")
                    End If
                End If
                If Session("SectorName") IsNot Nothing Then
                    If Session("SectorCode") IsNot Nothing Then
                        Dim sectorname As String = Session("SectorName")
                        txtsectorname.Text = sectorname
                        txtsectorcode.Text = Session("SectorCode")
                        Session.Remove("addcategory")
                        Session.Remove("SectorName")
                        Session.Remove("SectorCode")
                    End If
                End If



            End If
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CtryWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("CountryName") IsNot Nothing Then
                    If Session("CountryCode") IsNot Nothing Then
                        Dim countryname As String = Session("CountryName")
                        txtcountrycode.Text = Session("CountryCode")
                        txtcountryname.Text = countryname
                        Session.Remove("addcategory")
                        Session.Remove("CountryName")
                        Session.Remove("CountryCode")
                    End If
                End If
            End If
        End If

        If Me.IsPostBack Then
            ExtAppIDflag = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=5503")

            If ExtAppIDflag.ToUpper = "Y" Then
                ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_partymast", "partycode", txtCode.Value)
                If ExtAppIDexist Then
                    TxtExtappid.Disabled = True
                End If
            End If









        End If




        Dim typ As Type
        typ = GetType(DropDownList)
        If Session("partycode") Is Nothing Then
            Me.whotelatbcontrol.partyval = txtCode.Value
        Else
            Me.whotelatbcontrol.partyval = Session("partycode")
        End If


        Me.SubMenuUserControl1.suptype = txthotelcode.Text.Trim
        Session.Add("submenuuser", "SupplierSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

    Protected Sub IndexChanged()

        ddlTax.Enabled = True
        lblservicecharges.Visible = False
        lblmncpltyfees.Visible = False
        lbltourismfees.Visible = False
        If (ddlTax.SelectedValue <> "Exempted") Then
            lblvtax.Visible = True
            txtVAT.Visible = True
            lblperc4.Visible = True
        Else
            lblvtax.Visible = False
            txtVAT.Visible = False
            lblperc4.Visible = False
        End If


        txtServiceCharges.Visible = False
        TxtMunicipalityFees.Visible = False
        txtTourismFees.Visible = False
       
        If Not Page.IsPostBack Then
            txtVAT.Value = ""
        End If

        txtServiceCharges.Value = 0
        TxtMunicipalityFees.Value = 0
        txtTourismFees.Value = 0

        lblperc1.Visible = False
        lblperc2.Visible = False
        lblperc3.Visible = False


    End Sub




#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("SupState") = "New" Or Session("SupState") = "Copy" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "view_account", "code", CType(txtCode.Value.Trim, String), "type='S'") Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "view_account", "des", txtName.Value.Trim, "type='S'") Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("SupState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "view_account", "code", "des", txtName.Value.Trim, CType(txtCode.Value.Trim, String), "type='S'") Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    Public Function CheckForTaxes() As Boolean
        If ddlTax.SelectedValue = "Taxable" Then
            If (CInt(txtVAT.Value) <= 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('VAT% should be greater than zero.');", True)
                CheckForTaxes = False
            End If

            If CType(txthotelcode.Text, String) = "HOT" Then
                If (CInt(txtServiceCharges.Value) < 0 Or TxtMunicipalityFees.Value < 0 Or txtTourismFees.Value < 0) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Tax% cannot be less than zero.');", True)
                    CheckForTaxes = False
                End If
            End If
        End If

        CheckForTaxes = True
    End Function

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Handles btnSave.Click
        Try
            If Page.IsValid = True Then

                If Session("SupState") = "New" Or Session("SupState") = "Edit" Or Session("SupState") = "Copy" Then

                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    If CheckForTaxes() = False Then
                        Exit Sub
                    End If

                    If Trim(txtpropertytypecode.Text) = "" And txthotelcode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=458") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Property Type cannot be blank ');", True)

                        Exit Sub
                    End If
                    Dim Suppcat As String = ""

                    Suppcat = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=5505")
                    If ExtAppIDflag.ToUpper = "Y" And ExtAppIDexist = False And Suppcat <> txtcategorycode.Text Then
                        If checkForMappingIDExisting() = False Then
                            Exit Sub
                        End If
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction
                    If Session("SupState") = "New" Or Session("SupState") = "Copy" Then
                        Dim optionval As String
                        Dim sptype As String

                        sptype = txthotelcode.Text

                        optionval = objUtils.GetAutoDocNo(sptype, mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim
                    End If

                    Session("partycode") = txtCode.Value
                    Session("SupRefCode") = CType(Session("partycode"), String)

                    If Session("SupState") = "New" Or Session("SupState") = "Copy" Then
                        mySqlCmd = New SqlCommand("sp_add_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("SupState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_partymast", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 200)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(txthotelcode.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType(txtcategorycode.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scatcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtcurrencycode.Text, String)

                    'changed by mohamed on 31/10/2021
                    'mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtsectorcode.Text, String)
                    If txthotelchaincode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtsectorcode.Text, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If txthotelchaincode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@hotelchaincode", SqlDbType.VarChar, 20)).Value = txthotelchaincode.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@hotelchaincode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    If txthotelstatuscode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@hotelstatuscode", SqlDbType.VarChar, 20)).Value = txthotelstatuscode.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@hotelstatuscode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    If txtpropertytypename.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@propertytypecode", SqlDbType.VarChar, 20)).Value = txtpropertytypecode.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@propertytypecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    If CType(txthotelcode.Text, String) = "HOT" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@areacode", SqlDbType.VarChar, 20)).Value = DBNull.Value 'CType(ddlAreaCode.Items(ddlAreaCode.SelectedIndex).Text, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@areacode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    If ChkPreferred.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int)).Value = 1
                    ElseIf ChkPreferred.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@rnkorder", SqlDbType.Int)).Value = CType(Val(txtOrder.Value.Trim), Long)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    If txtcontrolacccode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = txtcontrolacccode.Text

                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If txtaccrualcode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = txtaccrualcode.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If chkshow.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@showininweb", SqlDbType.Int)).Value = 1
                    ElseIf ChkPreferred.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@showininweb", SqlDbType.Int)).Value = 0
                    End If
                    If chkvat.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATexclude", SqlDbType.Int)).Value = 1
                    ElseIf ChkPreferred.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATexclude", SqlDbType.Int)).Value = 0
                    End If


                    mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Value)
                    mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Value)
                    mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Value)
                    mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Value)
                    mySqlCmd.Parameters.Add(New SqlParameter("@hotelalias", SqlDbType.VarChar, 400)).Value = CType(txtalaisname.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@taxdetails", SqlDbType.VarChar, 20)).Value = CType(ddlTax.SelectedValue.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    If ExtAppIDflag.ToUpper = "Y" And ExtAppIDexist = False And Suppcat <> txtcategorycode.Text Then
                        mySqlCmd = New SqlCommand("sp_add_int_partymast", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@int_partyCode", SqlDbType.VarChar, 20)).Value = CType(TxtExtappid.Value.Trim, String)

                        mySqlCmd.ExecuteNonQuery()
                    End If
                ElseIf Session("SupState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    Session("partycode") = txtCode.Value
                    Session("SupRefCode") = CType(Session("partycode"), String)
                    deleteuploadimage() 'Beforedeletion
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    If ExtAppIDflag.ToUpper = "Y" Then

                        'deleting mapped Table

                        mySqlCmd = New SqlCommand("sp_del_int_partymast", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                    End If


                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

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
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)

                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)

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
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                TxtExtappid.Disabled = True
                If Session("SupState") = "New" Or Session("SupState") = "Copy" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)

                    Session.Add("SupState", "Edit")
                    If Not Request.QueryString("Popup") Is Nothing Then
                        If Request.QueryString("Popup") <> "" Then
                            Dim strscript As String = ""
                            strscript = "window.opener.__doPostBack('ContractTrackWindowPostBack', '');window.opener.focus();window.close();"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                        End If
                    End If


                ElseIf Session("SupState") = "Edit" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    Session.Add("SupState", "Edit")
                End If
                If Session("SupState") = "Delete" Then

                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
                Session("type") = ""
                Session("supmain_ctrycode_for_filter") = ""
                Session("supmain_citycode_for_filter") = ""
            End If


            'Response.Redirect(Request.Url.AbsoluteUri)
            'commented by Elsitta on 24/08/2016 due to this code, all alert messages are not coming

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region
#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        txtCode.Disabled = True
        If CType(Session("SupState"), String) = "View" Or CType(Session("SupState"), String) = "Delete" Then

            Me.txtName.Disabled = True

            txtName.Disabled = True

            txthotelname.Enabled = False

            txtcountryname.Enabled = False
            txtcityname.Enabled = False
            txtsectorname.Enabled = False
            txtpropertytypename.Enabled = False
            chkActive.Disabled = True
            txthotelstatusname.Enabled = False
            txthotelchainname.Enabled = False
            txtcontrolaccname.Enabled = False
            txtcurrencyname.Enabled = False
            txthotelname.Enabled = False
            txtcontrolaccname.Enabled = False
            txtcategoryname.Enabled = False

            txtOrder.Disabled = True
            ChkPreferred.Disabled = True


            btnCategory.Enabled = False
            btnCity.Enabled = False
            btnCountry.Enabled = False
            btnCurrency.Enabled = False
            btnSector.Enabled = False
            btnHotelChain.Enabled = False
            txtServiceCharges.Disabled = True
            TxtMunicipalityFees.Disabled = True
            txtTourismFees.Disabled = True
            txtVAT.Disabled = True
            txtcontrolacccode.Enabled = False
            ddlTax.Enabled = False



            '------------------------------------------------------
        ElseIf CType(Session("SupState"), String) = "Edit" Then
            If checkForAccountExisting() = False Then

                txtcurrencyname.Enabled = False
            End If

        End If

    End Sub
#End Region
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

                    If IsDBNull(mySqlReader("hotelalias")) = False Then
                        Me.txtalaisname.Text = mySqlReader("hotelalias")
                    End If


                    If IsDBNull(mySqlReader("sptypecode")) = False Then

                        txthotelcode.Text = mySqlReader("sptypecode")

                        txthotelname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))
                    End If






                    If IsDBNull(mySqlReader("catcode")) = False Then
                        txtcategorycode.Text = mySqlReader("catcode")
                        txtcategoryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "catmast", "catname", "catcode", mySqlReader("catcode"))
                    End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtcurrencycode.Text = mySqlReader("currcode")
                        txtcurrencyname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If



                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        txtcountrycode.Text = mySqlReader("ctrycode")
                        txtcountryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", mySqlReader("ctrycode"))
                    End If






                    If IsDBNull(mySqlReader("citycode")) = False Then
                        txtcitycode.Text = mySqlReader("citycode")
                        txtcityname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", mySqlReader("citycode"))
                    End If




                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        txtsectorcode.Text = mySqlReader("sectorcode")
                        txtsectorname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sectormaster", "sectorname", "sectorcode", mySqlReader("sectorcode"))
                    End If


                    If IsDBNull(mySqlReader("areacode")) = False Then
                        txtareacode.Text = mySqlReader("areacode")
                        txtareaname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "areamaster", "areaname", "areacode", mySqlReader("areacode"))
                    End If
                    If IsDBNull(mySqlReader("hotelchaincode")) = False Then
                        txthotelchaincode.Text = mySqlReader("hotelchaincode")
                        txthotelchainname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "hotelchainmaster", "hotelchainname", "hotelchaincode", mySqlReader("hotelchaincode"))
                    End If
                    If IsDBNull(mySqlReader("hotelstatuscode")) = False Then
                        txthotelstatuscode.Text = mySqlReader("hotelstatuscode")
                        txthotelstatusname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "hotelstatus", "hotelstatusname", "hotelstatuscode", mySqlReader("hotelstatuscode"))
                    End If
                    If IsDBNull(mySqlReader("propertytype")) = False Then
                        txtpropertytypecode.Text = mySqlReader("propertytype")
                        txtpropertytypename.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "hotel_propertytype", "propertytypename", "propertytypecode", mySqlReader("propertytype"))
                    End If
                    'Added areacode by Archana on 19/05/2015
                    If IsDBNull(mySqlReader("Preferred")) = False Then
                        If CType(mySqlReader("Preferred"), String) = "1" Then
                            ChkPreferred.Checked = True
                        ElseIf CType(mySqlReader("Preferred"), String) = "0" Then
                            ChkPreferred.Checked = False
                        End If
                    End If

                    'Added Vat Exclude by shahul 30/08/17
                    If IsDBNull(mySqlReader("VATexclude")) = False Then
                        If CType(mySqlReader("VATexclude"), String) = "1" Then
                            chkvat.Checked = True
                        ElseIf CType(mySqlReader("VATexclude"), String) = "0" Then
                            chkvat.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("showinweb")) = False Then
                        If CType(mySqlReader("showinweb"), String) = "1" Then
                            chkshow.Checked = True
                        ElseIf CType(mySqlReader("showinweb"), String) = "0" Then
                            chkshow.Checked = False
                        End If
                    End If


                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                    'rnkorder
                    If IsDBNull(mySqlReader("rnkorder")) = False Then
                        Me.txtOrder.Value = mySqlReader("rnkorder")
                    End If


                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        txtcontrolacccode.Text = mySqlReader("controlacctcode")
                        txtcontrolaccname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                    Else
                        txtcontrolacccode.Text = ""
                        txtcontrolaccname.Text = ""
                    End If







                    If IsDBNull(mySqlReader("accrualacctcode")) = False Then
                        txtaccrualcode.Text = mySqlReader("accrualacctcode")
                        txtaccrualname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("accrualacctcode"))
                    Else
                        txtaccrualcode.Text = ""
                        txtaccrualname.Text = ""
                    End If

                    Me.txtServiceCharges.Value = 0
                    Me.TxtMunicipalityFees.Value = 0
                    Me.txtTourismFees.Value = 0
                    Me.txtVAT.Value = 0
                    If IsDBNull(mySqlReader("ServiceChargePerc")) = False Then
                        Me.txtServiceCharges.Value = Val(CType(mySqlReader("ServiceChargePerc"), String))
                    End If
                    If IsDBNull(mySqlReader("MunicipalityFeePerc")) = False Then
                        Me.TxtMunicipalityFees.Value = Val(CType(mySqlReader("MunicipalityFeePerc"), String))
                    End If
                    If IsDBNull(mySqlReader("TourismFeePerc")) = False Then
                        Me.txtTourismFees.Value = Val(CType(mySqlReader("TourismFeePerc"), String))
                    End If
                    If IsDBNull(mySqlReader("VATPerc")) = False Then
                        Me.txtVAT.Value = Val(CType(mySqlReader("VATPerc"), String))
                    End If
                    If IsDBNull(mySqlReader("taxdetails")) = False Then
                        Me.ddlTax.SelectedValue = CType(mySqlReader("taxdetails"), String)
                        DisplayTaxOptions(Me.ddlTax.SelectedValue)
                    Else
                        Me.ddlTax.SelectedValue = "Taxable"
                        DisplayTaxOptions(Me.ddlTax.SelectedValue)
                    End If
                End If



                Session.Add("type", txthotelcode.Text)
                Session("supmain_ctrycode_for_filter") = txtcountrycode.Text
                Session("supmain_citycode_for_filter") = txtcitycode.Text
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
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
#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region

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

            Return True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
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




        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymaxaccomodation", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MaximumAccomodation Of Supplier, cannot delete this Supplier');", True)
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




            ''check the accounts
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "receipt_detail", "receipt_acc_code", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for a accounts transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "journal_detail", "journal_acc_code", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for a accounts transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "openparty_master", "open_code", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for a accounts opening  transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " edit_contracts", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for contracts, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If


        checkForDeletion = True
    End Function
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getpropertytypelist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim propertytype As New List(Of String)
        Try

            strSqlQry = "select propertytypename,propertytypecode from hotel_propertytype where  propertytypename like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    propertytype.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("propertytypename").ToString(), myDS.Tables(0).Rows(i)("propertytypecode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return propertytype
        Catch ex As Exception
            Return propertytype
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function Gethoteltypelist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim cs As New SupMain

        Dim Hotelnames As New List(Of String)
        Try
            Dim strType As String = ""
            If CType(HttpContext.Current.Session("supmain_suptype"), String) <> "" Then

                If CType(HttpContext.Current.Session("supmain_suptype"), String) = "Other Serv" Then
                    strType = "OtherServ"
                Else
                    strType = CType(HttpContext.Current.Session("supmain_suptype"), String)
                End If
                '


                strSqlQry = "select sptypename,sptypecode from sptypemast where  sptypename like  '" & Trim(prefixText) & "%'  and sptypecode ='" & strType & "' "
            Else

                strSqlQry = "select sptypename,sptypecode from sptypemast where  sptypename like  '" & Trim(prefixText) & "%' "
            End If

            Dim SqlConn As New SqlConnection

            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sptypename").ToString(), myDS.Tables(0).Rows(i)("sptypecode").ToString()))
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
    Public Shared Function Getcategorylist(ByVal prefixText As String) As List(Of String)
        Dim sptyp As String = HttpContext.Current.Session("type")
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Categorynames As New List(Of String)
        Try

            If CType(HttpContext.Current.Session("supmain_suptype"), String) <> "" Then

                strSqlQry = "select catname,catcode from catmast where active=1 and sptypecode='" & CType(HttpContext.Current.Session("supmain_suptype"), String) & "'"

                strSqlQry = strSqlQry + " and catname like  '" & Trim(prefixText) & "%'"

            Else
                strSqlQry = "select catname,catcode from catmast where active=1 "
                If Trim(sptyp) <> "" Then
                    strSqlQry = strSqlQry + " and sptypecode='" & Trim(sptyp) & "'"
                End If
                strSqlQry = strSqlQry + " and catname like  '" & Trim(prefixText) & "%'"
            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Categorynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("catname").ToString(), myDS.Tables(0).Rows(i)("catcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Categorynames
        Catch ex As Exception
            Return Categorynames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function Getcurrencylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Currencynames As New List(Of String)
        Try
            strSqlQry = "select currname,currcode from currmast where  currname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Currencynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Currencynames
        Catch ex As Exception
            Return Currencynames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getcountrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Countrynames As New List(Of String)
        Try
            strSqlQry = "select ctryname,ctrycode from ctrymast  where  ctryname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Countrynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Countrynames
        Catch ex As Exception
            Return Countrynames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getcitylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim ctry As String = "" 'contextKey
        Dim myDS As New DataSet
        Dim Citynames As New List(Of String)
        Try

            If HttpContext.Current.Session("supmain_ctrycode_for_filter") IsNot Nothing Then
                ctry = HttpContext.Current.Session("supmain_ctrycode_for_filter")
            End If


            strSqlQry = "select cityname,citycode from citymast where active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            strSqlQry = strSqlQry + " and cityname like  '" & Trim(prefixText) & "%'"
            strSqlQry += " order by cityname"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Citynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("cityname").ToString(), myDS.Tables(0).Rows(i)("citycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Citynames
        Catch ex As Exception
            Return Citynames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getsectorlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim ctry, city As String
        Dim myDS As New DataSet
        Dim Sectornames As New List(Of String)
        Try

            If HttpContext.Current.Session("supmain_ctrycode_for_filter") IsNot Nothing Then
                ctry = HttpContext.Current.Session("supmain_ctrycode_for_filter")
            End If
            If HttpContext.Current.Session("supmain_ctrycode_for_filter") IsNot Nothing Then
                city = HttpContext.Current.Session("supmain_citycode_for_filter")
            End If
            strSqlQry = "select sectorname,sectorcode from sectormaster  where  active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            If Trim(city) <> "" Then
                strSqlQry = strSqlQry + " and citycode='" & Trim(city) & "'"
            End If
            strSqlQry = strSqlQry + " and sectorname like  '" & Trim(prefixText) & "%'"
            strSqlQry += " order by sectorname"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Sectornames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sectorname").ToString(), myDS.Tables(0).Rows(i)("sectorcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            Else
                'dummyCity_Click(sender, e)
                'SaveSectorCode()
                sectorvalue = 0

            End If
            Return Sectornames
        Catch ex As Exception
            Return Sectornames
        End Try
    End Function

    ' <System.Web.Script.Services.ScriptMethod()> _
    '<System.Web.Services.WebMethod()> _
    ' Public Shared Function Sectorlist(ByVal prefixText As String)
    '     Dim mySqlCmd As SqlCommand
    '     Dim mySqlReader As SqlDataReader
    '     Dim mySqlConn As SqlConnection
    '     Dim sqlTrans As SqlTransaction

    '     'saving city name and code for sectors , if for a city no sectors are present
    '     If txtcountrycode.Text <> "[Select]" And txtcitycode.Text <> "[Select]" Then
    '         If checkSectorForDuplicate() = False Then
    '             Exit Function
    '         End If

    '         Try
    '             mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '             sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
    '             mySqlCmd = New SqlCommand("sp_add_sectormaster", mySqlConn, sqlTrans)
    '             mySqlCmd.CommandType = CommandType.StoredProcedure
    '             mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
    '             mySqlCmd.Parameters.Add(New SqlParameter("@sectorname", SqlDbType.VarChar, 150)).Value = CType(txtcityname.Text.Trim, String)
    '             mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Text.Trim, String)
    '             mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
    '             mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
    '             mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
    '             mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
    '             mySqlCmd.ExecuteNonQuery()
    '             sqlTrans.Commit()    'SQl Tarn Commit
    '             clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
    '             clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '             clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

    '         Catch ex As Exception
    '             If mySqlConn.State = ConnectionState.Open Then
    '                 sqlTrans.Rollback()
    '             End If
    '             ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '             objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '         End Try
    '     End If
    ' End Function











    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getcontrolacclist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Controlaccnames As New List(Of String)
        Try
            strSqlQry = "select acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='S'  and  acctname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Controlaccnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Controlaccnames
        Catch ex As Exception
            Return Controlaccnames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getarealist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Areanames As New List(Of String)
        Try
            strSqlQry = "select areaname,areacode from areamaster where active=1  and  areaname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Areanames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("areaname").ToString(), myDS.Tables(0).Rows(i)("areacode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If
            Return Areanames
        Catch ex As Exception
            Return Areanames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getaccruallist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Accuralnames As New List(Of String)
        Try
            strSqlQry = "select  acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='S' where   acctname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Accuralnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("areaname").ToString(), myDS.Tables(0).Rows(i)("areacode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If
            Return Accuralnames
        Catch ex As Exception
            Return Accuralnames
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Gethotelchainlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelchainnames As New List(Of String)
        Try
            strSqlQry = "select hotelchainname,hotelchaincode from hotelchainmaster where  hotelchainname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelchainnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("hotelchainname").ToString(), myDS.Tables(0).Rows(i)("hotelchaincode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Hotelchainnames
        Catch ex As Exception
            Return Hotelchainnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Gethotelstatuslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotelstatus As New List(Of String)
        Try
            strSqlQry = "select hotelstatusname,hotelstatuscode from hotelstatus where  hotelstatusname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    hotelstatus.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("hotelstatusname").ToString(), myDS.Tables(0).Rows(i)("hotelstatuscode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return hotelstatus
        Catch ex As Exception
            Return hotelstatus
        End Try

    End Function
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        If (CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "HOT" Or CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "HOT" Or (CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) <> "OTH")) Then


            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=othersupmain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
    End Sub
    Private Function checkForAccountExisting() As Boolean
        'GetDBFieldValueExist
        Dim strValue As String = ""
        If Session("SupState") = "Edit" Then
            strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 't' from acc_tran where acc_type='S' and acc_code='" & Trim(txtCode.Value) & "'")
            If strValue <> "" Then
                checkForAccountExisting = False
                Exit Function
            End If
            strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 't' from view_unpost_acccode where acc_type='S' and acc_code='" & Trim(txtCode.Value) & "'")
            If strValue <> "" Then
                checkForAccountExisting = False
                Exit Function
            End If
        End If
        checkForAccountExisting = True
    End Function

    Public Function checkSectorForDuplicate() As Boolean
        'saving city name and code for sectors , if for a city no sectors are present
        If objUtils.isDuplicatenew(Session("dbconnectionName"), "sectormaster", "sectorcode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city code is already present as a sector.');", True)
            checkSectorForDuplicate = False
            Exit Function
        End If
        If objUtils.isDuplicatenew(Session("dbconnectionName"), "sectormaster", "sectorname", txtcityname.Text) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city name is already present as a sector.');", True)
            checkSectorForDuplicate = False
            Exit Function
        End If
        checkSectorForDuplicate = True
    End Function
    Public Function checkAreaForDuplicate() As Boolean
        'saving city name and code for sectors , if for a city no sectors are present
        If objUtils.isDuplicatenew(Session("dbconnectionName"), "areamaster", "areacode", CType(txtcitycode.Text.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city code is already present as a area.');", True)
            checkAreaForDuplicate = False
            Exit Function
        End If
        If objUtils.isDuplicatenew(Session("dbconnectionName"), "areamaster", "areaname", txtcitycode.Text.Trim) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city name is already present as a area.');", True)
            checkAreaForDuplicate = False
            Exit Function
        End If

        checkAreaForDuplicate = True
    End Function

    'Added checkAreaForDuplicate() function by Archana on 19/05/2015
    Private Function checkForMappingIDExisting() As Boolean
        'GetDBFieldValueExist

        If TxtExtappid.Value = "" Then
            checkForMappingIDExisting = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter" & LblExtappid.Text & " .');", True)
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_partymast", "int_partyCode", TxtExtappid.Value) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & LblExtappid.Text & " Already Present .');", True)
            checkForMappingIDExisting = False
            Exit Function
        End If

        'ExecuteQueryReturnStringValue

        checkForMappingIDExisting = True
    End Function
    Protected Sub dummyCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dummyCity.Click
        'saving city name and code for sectors , if for a city no sectors are present
        If txtcountrycode.Text <> "[Select]" And txtcitycode.Text <> "[Select]" Then
            If checkSectorForDuplicate() = False Then
                Exit Sub
            End If

            Try
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                mySqlCmd = New SqlCommand("sp_add_sectormaster", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@sectorname", SqlDbType.VarChar, 150)).Value = CType(txtcityname.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
                'mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Catch ex As Exception
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub

    Protected Sub dummyCityArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dummyCityArea.Click
        'saving city name and code for sectors , if for a city no sectors are present
        If txtcitycode.Text <> "[Select]" Then
            If checkAreaForDuplicate() = False Then
                Exit Sub
            End If

            Try
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                mySqlCmd = New SqlCommand("sp_add_areamaster", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@areacode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@areaname", SqlDbType.VarChar, 150)).Value = CType(txtcityname.Text.Trim, String)

                mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)

                mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1

                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                mySqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

            Catch ex As Exception
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Suppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try


        End If
    End Sub

    'Added dummyCityArea_Click by Archana on 19/05/2015

    Public Sub ToSendMailWhenNewSupplierCreated(ByVal SupCode As String, ByVal SupName As String)
        Dim objEmail As New clsEmail
        Dim fromMail As String = ""
        Dim toMail As String = ""
        Dim strMessage As String = ""
        Dim strSubject As String = ""
        strSubject = "New Supplier Created"
        strMessage = "Supplier Created with Supplier Code : " + CType(SupCode, String) + " and  Supplier Name : " + CType(SupName, String)
        Try
            Dim strfrommail = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1070'")
            Dim strTomail = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1010'")

            If objEmail.SendEmail(strfrommail, strTomail, strSubject, strMessage) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + strTomail + "');", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + strTomail + "');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try
    End Sub

    Protected Sub txthotelname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txthotelname.TextChanged
        Session.Add("type", txthotelcode.Text)
    End Sub

    Protected Sub txtcountryname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcountryname.TextChanged
        Session("supmain_ctrycode_for_filter") = txtcountrycode.Text
    End Sub

    Protected Sub txtcategoryname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcategoryname.TextChanged

    End Sub
    Protected Sub txtcityname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcityname.TextChanged
        Session("supmain_citycode_for_filter") = txtcitycode.Text

        Dim dsData As DataSet
        dsData = objUtils.GetDataFromDatasetnew(Session("dbConnectionName"), "select * from citymast where citycode='" & txtcitycode.Text & "' and ctrycode='" & txtcountrycode.Text & "'")
        If dsData.Tables.Count > 0 Then
            If dsData.Tables(0).Rows.Count > 0 Then
                txtServiceCharges.Value = dsData.Tables(0).Rows(0)("ServiceChargePerc")
                TxtMunicipalityFees.Value = dsData.Tables(0).Rows(0)("MunicipalityFeePerc")
                txtTourismFees.Value = dsData.Tables(0).Rows(0)("TourismFeePerc")
                txtVAT.Value = dsData.Tables(0).Rows(0)("VATPerc")
            End If
        End If
    End Sub
    Protected Sub txtsectorname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtsectorname.TextChanged
        'Dim strpop As String = ""
        'strpop = "window.open('Sector.aspx?State=New&Value=Addfrom','Supcats');"
        'Session.Add("addcategory", "new")
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('SupplierCategories.aspx?State=New&Value=Addfrom','Supcats');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('Countries.aspx?State=New&Value=Addfrom','Country');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('Currencies.aspx?State=New&Value=Addfrom','Currency');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('Cities.aspx?State=New&Value=Addfrom','City');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnSector_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('Sector.aspx?State=New&Value=Addfrom','Sector');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnHotelChain_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('HotelChain.aspx?State=New&Value=Addfrom','Hotelchain');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub txtcategorycode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcategorycode.TextChanged


    End Sub
End Class