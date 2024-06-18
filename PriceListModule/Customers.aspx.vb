'------------================--------------=======================------------------================
'   Module Name    :    Customer.aspx
'   Developer Name :    Indulkar Sandeep
'   Date           :   
'   
'
'------------================--------------=======================------------------================
#Region " Name Space"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
#End Region

Partial Class Customers
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim ObjDate As New clsDateTime
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As New SqlDataAdapter
    Dim ddlPcode As DropDownList
#End Region

#Region "    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim GVRow As GridViewRow
       
        Dim RefCode As String
        If IsPostBack = False Then
            PanelMain.Visible = True
            PanelAccounts.Visible = False
            PanelGeneral.Visible = False
            PanelReservstion.Visible = False
            PanelSales.Visible = False
            PanelSurvey.Visible = False
            PanelUser.Visible = False
            PanelVisit.Visible = False
            PanelWebApproval.Visible = False

            txtconnection.Value = Session("dbconnectionName")

            FillDDL()
            fillgrd(gv_Email, True)
            fillgrd(grvSurvey, True)

            Numbers(TxtAccCreditDays)
            Numbers(txtAccCreditLimit)


            If CType(Session("State"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add New Customer "
                btnSave_Main.Text = "Save"
                BtnResSave.Text = "Save"
                BtnSaleSave.Text = "Save"
                BtnAccSave.Text = "Save"
                BtnUserSave.Text = "Save"
                BtnWebAppSave.Text = "Save"
                BtnVisitSave.Text = "Save"
                BtnSurveySave.Text = "Save"
                BtnGeneralSave.Text = "Save"
                btnSave_Main.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                BtnResSave.Attributes.Add("onclick", "return FormValidationReservation('New')")
                BtnSaleSave.Attributes.Add("onclick", "return ValidEmail('New')")


            ElseIf CType(Session("State"), String) = "Edit" Then

                btnSave_Main.Text = "Update"
                BtnResSave.Text = "Update"
                BtnSaleSave.Text = "Update"
                BtnAccSave.Text = "Update"
                BtnUserSave.Text = "Update"
                BtnWebAppSave.Text = "Update"
                BtnVisitSave.Text = "Update"
                BtnSurveySave.Text = "Update"
                BtnGeneralSave.Text = "Update"

                RefCode = CType(Session("RefCode"), String)

                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Edit Customer"
                btnSave_Main.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                BtnResSave.Attributes.Add("onclick", "return FormValidationReservation('Edit')")
                BtnSaleSave.Attributes.Add("onclick", "return ValidEmail('Edit')")

            ElseIf CType(Session("State"), String) = "View" Then

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Customer"
                btnSave_Main.Visible = False
                BtnResSave.Visible = False
                BtnSaleSave.Visible = False
                BtnAccSave.Visible = False
                BtnUserSave.Visible = False
                BtnWebAppSave.Visible = False
                BtnShowPassword.Visible = False
                BtnWebInviteCustomer.Visible = False
                BtnWebResendPasswprd.Visible = False
                BtnVisitSave.Visible = False
                BtnSurveySave.Visible = False
                BtnGeneralSave.Visible = False
                btnCancel_Main.Text = "Return to Search"
                BtnResCancel.Text = "Return to Search"
                BtnSaleCancel.Text = "Return to Search"
                BtnAccCancel.Text = "Return to Search"
                BtnUserCancel.Text = "Return to Search"
                BtnWebAppCancel.Text = "Return to Search"
                BtnSurveyCancel.Text = "Return to Search"
                BtnGeneralCancel.Text = "Return to Search"
                btnCancel_Main.Focus()

            ElseIf CType(Session("State"), String) = "Delete" Then

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "Delete Customer"

                BtnUserDetailAdd.Visible = False
                BtnShowPassword.Visible = False
                BtnWebInviteCustomer.Visible = False
                BtnWebResendPasswprd.Visible = False
                btnVisitFollo.Visible = False
                Btnaddsurvey.Visible = False
                btnViewForm.Visible = False
                btnSave_Main.Text = "Delete"
                BtnResSave.Text = "Delete"
                BtnSaleSave.Text = "Delete"
                BtnAccSave.Text = "Delete"
                BtnUserSave.Text = "Delete"
                BtnWebAppSave.Text = "Delete"
                BtnVisitSave.Text = "Delete"
                BtnSurveySave.Text = "Delete"
                BtnGeneralSave.Text = "Delete"
                btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want Delete?')==false)return false;")

            End If
             End If
        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSelling.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlOtherSell.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlOtherSellName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlTicketSelling.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlTicketSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCurrency.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSalesPerson.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSalesPersonName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlMarket.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSector.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSectorName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(e)")
    End Sub
#End Region

#Region "Private Sub FillDDL()"
    Private Sub FillDDL()
        strSqlQry = ""
        strSqlQry = "SELECT agentcatcode,agentcatname FROM agentcatmast WHERE active=1 order by agentcatcode "
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategory, "agentcatcode", "agentcatname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT agentcatname,agentcatcode FROM agentcatmast WHERE active=1 order by agentcatname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryName, "agentcatname", "agentcatcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select sellcode,sellname from sellmast where active=1 order by sellcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSelling, "sellcode", "sellname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select sellname,sellcode from sellmast where active=1 order by sellname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "sellname", "sellcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select othsellcode,othsellname from othsellmast where active=1  order by othsellcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSell, "othsellcode", "othsellname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select othsellname,othsellcode from othsellmast where active=1  order by othsellname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSellName, "othsellname", "othsellcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select tktsellcode,tktsellname from tktsellmast where active=1  order by tktsellcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTicketSelling, "tktsellcode", "tktsellname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select tktsellname,tktsellcode from tktsellmast where active=1  order by tktsellname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTicketSellingName, "tktsellname", "tktsellcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select currcode,currname from currmast where active=1 order by currcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select currname,currcode from currmast where active=1 order by currname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select currcode,currname from currmast where active=1 order by currcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select currname,currcode from currmast where active=1 order by currname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select ctryname,ctrycode from ctrymast where active=1 order by ctryname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select citycode,cityname from citymast where active=1  order by citycode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select cityname,citycode from citymast where active=1  order by cityname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select UserCode,UserName from UserMaster where active=1  order by UserCode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPerson, "UserCode", "UserName", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select UserCode,UserName from UserMaster where active=1  order by UserName"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPersonName, "UserName", "UserCode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1  order by plgrpcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpcode", "plgrpname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select plgrpname,plgrpcode from plgrpmast where active=1  order by plgrpname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select sectorcode,sectorname from agent_sectormaster where active=1  order by sectorcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSector, "sectorcode", "sectorname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select sectorname,sectorcode from agent_sectormaster where active=1  order by sectorname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", strSqlQry, True)

        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", "select acctcode,acctname from acctmast where upper(controlyn)='Y' order by acctcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", "select  acctname,acctcode from acctmast where upper(controlyn)='Y'  order by acctname", True)

        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostCode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1  order by agentcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostName, "agentname", "agentcode", "select  agentname,agentcode from agentmast where active=1  order by agentname", True)




    End Sub
#End Region

#Region "    Protected Sub BtnMainDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub BtnMainDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = True
        PanelReservstion.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelUser.Visible = False
        PanelSurvey.Visible = False
        PanelVisit.Visible = False
        PanelWebApproval.Visible = False
        SetFocus(txtshortname)
        '  GetValuesForMainDetails()
        ShowRecord(Session("RefCode"))
    End Sub
#End Region

#Region "Protected Sub BtnReservation_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnReservation_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = True
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelUser.Visible = False
        PanelSurvey.Visible = False
        PanelVisit.Visible = False
        PanelWebApproval.Visible = False
        SetFocus(txtResAddress1)
        'ShowRecord(Session("RefCode"))
        GetValuesForResvationDetails()
    End Sub
#End Region

#Region " Protected Sub BtnGeneral_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneral_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = True
        PanelUser.Visible = False
        PanelSurvey.Visible = False
        PanelVisit.Visible = False
        PanelWebApproval.Visible = False
        SetFocus(txtGeneral)
        ' ShowRecord(Session("RefCode"))
        GetValuesForGeneralDetails()
    End Sub
#End Region

#Region "Protected Sub BtnWebAppr_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnWebAppr_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelUser.Visible = False
        PanelSurvey.Visible = False
        PanelVisit.Visible = False
        PanelWebApproval.Visible = True
        SetFocus(txtWebAppUsername)
        ' ShowRecord(Session("RefCode"))
        GetValuesForWebApprv()
    End Sub
#End Region

#Region "Protected Sub BtnAccountDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAccountDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = True
        PanelGeneral.Visible = False
        PanelUser.Visible = False
        PanelSurvey.Visible = False
        PanelVisit.Visible = False
        PanelWebApproval.Visible = False

        SetFocus(txtAccTelephone1)
        '  ShowRecord(Session("RefCode"))
        GetValuesForAccountDetails()
    End Sub
#End Region

#Region " Protected Sub BtnVisitFollow_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub BtnVisitFollow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelUser.Visible = False
        PanelSurvey.Visible = False
        PanelVisit.Visible = True
        PanelWebApproval.Visible = False
        SetFocus(txtGeneral)
        Dim GVRow As GridViewRow
        'Dim ddl As HtmlSelect
        Try
            If CType(Session("State"), String) = "New" Then
                fillgrd(gv_VisitFollow, True)
                For Each GVRow In gv_VisitFollow.Rows
                    ddlPcode = GVRow.FindControl("ddlSalesPersonCode")
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlPcode, "UserCode", "select UserCode,UserName from UserMaster where active=1 order by UserCode", True)
                Next
            Else
                GetValuesForVisit()
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
        ' GetValuesForVisit()
    End Sub
#End Region

#Region "Protected Sub BtnSurveyForm_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSurveyForm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelUser.Visible = False
        PanelSurvey.Visible = True
        PanelVisit.Visible = False
        PanelWebApproval.Visible = False
        SetFocus(txtGeneral)
        ' ShowRecord(Session("RefCode"))
        GetValuesForSurveys()
    End Sub
#End Region

#Region "Protected Sub btnCancel_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("State") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentcode", CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentname", txtCustomerName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("State") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", txtCustomerName.Value.Trim, CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

#Region "    Protected Sub btnSave_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave_Main.Click"
    Protected Sub btnSave_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave_Main.Click
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Or Session("State") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_agentmast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_agentmast", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 100)).Value = CType(txtCustomerName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@shortname", SqlDbType.VarChar, 50)).Value = CType(txtshortname.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType((ddlCategory.Items(ddlCategory.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sellcode", SqlDbType.VarChar, 20)).Value = CType((ddlSelling.Items(ddlSelling.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 20)).Value = CType((ddlOtherSell.Items(ddlOtherSell.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tktsellcode", SqlDbType.VarChar, 20)).Value = CType((ddlTicketSelling.Items(ddlTicketSelling.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType((ddlCurrency.Items(ddlCurrency.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType((ddlCountry.Items(ddlCountry.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType((ddlCity.Items(ddlCity.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 20)).Value = CType((ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType((ddlMarket.Items(ddlMarket.SelectedIndex).Text), String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType((ddlSector.Items(ddlSector.SelectedIndex).Text), String)

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@commperc", SqlDbType.Decimal, 6, 2)).Value = CType(Val(txtCommission.Value), Decimal)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then

                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail  where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                '  Session.Add("SessionFirstCheck", "Edit")
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                    Session.Add("State", "Edit")
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    Session.Add("State", "Edit")
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If



            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()

        txtCustomerCode.Disabled = True
        txtCustomerName.Disabled = True
        txtshortname.Disabled = True
        ddlSalesPerson.Disabled = True
        ddlSalesPersonName.Disabled = True
        ddlCategory.Disabled = True
        ddlCategoryName.Disabled = True
        ddlSelling.Disabled = True
        ddlSellingName.Disabled = True
        ddlCurrency.Disabled = True
        ddlCurrencyName.Disabled = True
        ddlCountry.Disabled = True
        ddlCountryName.Disabled = True
        ddlCity.Disabled = True
        ddlCityName.Disabled = True
        ddlSector.Disabled = True
        ddlSectorName.Disabled = True
        ddlTicketSelling.Disabled = True
        ddlTicketSellingName.Disabled = True
        ddlMarket.Disabled = True
        ddlMarketName.Disabled = True
        ddlOtherSell.Disabled = True
        ddlOtherSellName.Disabled = True
        txtCommission.Disabled = True
        chkActive.Disabled = True
        '------------------------------------------------------
        '        '-------------- Reservation Details --------------------------
        txtResAddress1.Disabled = True
        txtResAddress2.Disabled = True
        txtResAddress3.Disabled = True
        txtResPhone1.Disabled = True
        txtResPhone2.Disabled = True
        txtResFax.Disabled = True
        txtResContact1.Disabled = True
        txtResContact2.Disabled = True
        txtResEmail.Disabled = True
        ddlCommunicateBy.Enabled = False

        '        '------------------------END-----------------------------------
        '        '---------  Sales Details ------------------------------------
        txtSaleRecommended.Disabled = True
        txtSaleTelephone1.Disabled = True
        txtSaleTelephone2.Disabled = True
        txtSaleFax.Disabled = True
        txtSaleContact1.Disabled = True
        txtSaleContact2.Disabled = True
        txtSaleEmail.Disabled = True
        '        '------------------------END-----------------------------------
        '        '---------  Account Details ------------------------------------

        txtAccTelephone1.Disabled = True
        txtAccTelephone2.Disabled = True
        txtAccFax.Disabled = True
        txtAccContact1.Disabled = True
        txtAccContact2.Disabled = True
        txtAccEmail.Disabled = True
        TxtAccCreditDays.Disabled = True
        txtAccCreditLimit.Disabled = True
        ddlAccCode.Disabled = True
        ddlAccName.Disabled = True
        ddlPostCode.Disabled = True
        ddlPostName.Disabled = True


        ChkAccBooking2.Disabled = True
        txtAccBooking.Disabled = True
        ChkCashSup.Disabled = True
        '        '------------------------END-----------------------------------
        '        '---------  General Details ------------------------------------
        txtGeneral.ReadOnly = True

        ' -------------------User Email ---------------------------------------------
        BtnUserDetailAdd.Enabled = False
        gv_Email.Enabled = False

        '-------------------------Survey
        grvSurvey.Enabled = False
        '--------------------------Visit-------------========-----------------------------
        btnVisitFollo.Enabled = False
        Btnaddsurvey.Enabled = False
        btnViewForm.Enabled = False
        gv_VisitFollow.Enabled = False


        '---------------------------Web Approval----------------------------------------

        txtWebAppUsername.Disabled = True
        txtWebAppPassword.Disabled = True
        txtWebAppContact.Disabled = True
        txtWebAppEmail.Disabled = True
        ChkWebApprove.Disabled = True

    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentcode")) = False Then
                        Me.txtCustomerCode.Value = mySqlReader("agentcode")
                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtCustomerName.Value = mySqlReader("agentname")
                    End If
                    If IsDBNull(mySqlReader("shortname")) = False Then
                        Me.txtshortname.Value = mySqlReader("shortname")
                    End If
                    '---------- Main Details    -------------------------


                    If IsDBNull(mySqlReader("catcode")) = False Then
                        Me.ddlCategoryName.Value = mySqlReader("catcode")
                        Me.ddlCategory.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentcatmast", "agentcatname", "agentcatcode", CType(mySqlReader("catcode"), String))
                    End If
                    If IsDBNull(mySqlReader("sellcode")) = False Then
                        ddlSellingName.Value = mySqlReader("sellcode")
                        ddlSelling.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellmast", "sellname", "sellcode", CType(mySqlReader("sellcode"), String))
                    End If

                    strSqlQry = ""
                    strSqlQry = "select currmast.currname,currmast.currcode from currmast inner join  sellmast on currmast.currcode=sellmast.currcode  where currmast.active=1 and sellmast.sellcode='" & mySqlReader("sellcode") & "' order by currmast.currname"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", strSqlQry, True)
                    strSqlQry = ""
                    strSqlQry = "select currmast.currcode,currmast.currname from currmast inner join  sellmast on currmast.currcode=sellmast.currcode  where currmast.active=1 and sellmast.sellcode='" & mySqlReader("sellcode") & "' order by  currmast.currcode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currname", strSqlQry, True)

                    If IsDBNull(mySqlReader("currcode")) = False Then
                        ddlCurrencyName.Value = mySqlReader("currcode")
                        ddlCurrency.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", CType(mySqlReader("currcode"), String))
                    End If
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        ddlCountryName.Value = mySqlReader("ctrycode")
                        ddlCountry.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                    End If

                    strSqlQry = ""
                    strSqlQry = "select citycode,cityname from citymast where active=1 and ctrycode='" & mySqlReader("ctrycode") & "'  order by citycode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", strSqlQry, True)
                    strSqlQry = ""
                    strSqlQry = "select cityname,citycode from citymast where active=1  and ctrycode='" & mySqlReader("ctrycode") & "'  order by cityname"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", strSqlQry, True)

                    strSqlQry = ""
                    strSqlQry = "select sectorcode,sectorname from agent_sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "'   order by sectorcode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSector, "sectorcode", "sectorname", strSqlQry, True)
                    strSqlQry = ""
                    strSqlQry = "select sectorname,sectorcode from agent_sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "'  order by sectorname"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", strSqlQry, True)


                    If IsDBNull(mySqlReader("citycode")) = False Then
                        ddlCityName.Value = mySqlReader("citycode")
                        ddlCity.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", CType(mySqlReader("citycode"), String))
                    End If
                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        ddlMarketName.Value = mySqlReader("plgrpcode")
                        ddlMarket.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", CType(mySqlReader("plgrpcode"), String))
                    End If
                    strSqlQry = ""
                    strSqlQry = "select sectorcode,sectorname from agent_sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "' and plgrpcode='" & mySqlReader("plgrpcode") & "'   order by sectorcode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSector, "sectorcode", "sectorname", strSqlQry, True)
                    strSqlQry = ""
                    strSqlQry = "select sectorname,sectorcode from agent_sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "' and plgrpcode='" & mySqlReader("plgrpcode") & "'  order by sectorname"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", strSqlQry, True)


                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        ddlSectorName.Value = mySqlReader("sectorcode")
                        ddlSector.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agent_sectormaster", "sectorname", "sectorcode", CType(mySqlReader("sectorcode"), String))
                    End If
                    If IsDBNull(mySqlReader("spersoncode")) = False Then
                        ddlSalesPersonName.Value = mySqlReader("spersoncode")
                        ddlSalesPerson.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "UserMaster", "UserName", "UserCode", CType(mySqlReader("spersoncode"), String))
                    End If
                    If IsDBNull(mySqlReader("tktsellcode")) = False Then
                        ddlTicketSellingName.Value = mySqlReader("tktsellcode")
                        ddlTicketSelling.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "tktsellmast", "tktsellname", "tktsellcode", CType(mySqlReader("tktsellcode"), String))
                    End If

                    If IsDBNull(mySqlReader("othsellcode")) = False Then
                        ddlOtherSellName.Value = mySqlReader("othsellcode")
                        ddlOtherSell.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "othsellmast.othsellname", "othsellcode", CType(mySqlReader("othsellcode"), String))
                    End If
                    If IsDBNull(mySqlReader("commperc")) = False Then
                        Me.txtCommission.Value = mySqlReader("commperc")
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                    If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agents_locked", "agentcode", CType(mySqlReader("agentcode"), String)) = True Then
                        lbllockstatus.Text = "Locked"
                    Else
                        lbllockstatus.Text = "UnLocked"
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaleSave.Click"
    Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaleSave.Click
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_agentmast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_agentmast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@recommby", SqlDbType.VarChar, 100)).Value = CType(txtSaleRecommended.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel1", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel2", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sfax", SqlDbType.VarChar, 50)).Value = CType(txtSaleFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact1", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact2", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@semail", SqlDbType.VarChar, 100)).Value = CType(txtSaleEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub GetValuesForSalesDetails()"
    Private Sub GetValuesForSalesDetails()
        Try
            If Session("State") = "Edit" Or Session("State") = "View" Or Session("State") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                'mySqlCmd = New SqlCommand("Select * from agentcatmast Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  Sales Details ------------------------------------
                        If IsDBNull(mySqlReader("recommby")) = False Then
                            txtSaleRecommended.Value = mySqlReader("recommby")
                        Else
                            txtSaleRecommended.Value = ""
                        End If
                        If IsDBNull(mySqlReader("stel1")) = False Then
                            txtSaleTelephone1.Value = mySqlReader("stel1")
                        Else
                            txtSaleTelephone1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("stel2")) = False Then
                            txtSaleTelephone2.Value = mySqlReader("stel2")
                        Else
                            txtSaleTelephone2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("sfax")) = False Then
                            txtSaleFax.Value = mySqlReader("sfax")
                        Else
                            txtSaleFax.Value = ""
                        End If
                        If IsDBNull(mySqlReader("scontact1")) = False Then
                            txtSaleContact1.Value = mySqlReader("scontact1")
                        Else
                            txtSaleContact1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("scontact2")) = False Then
                            txtSaleContact2.Value = mySqlReader("scontact2")
                        Else
                            txtSaleContact2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("semail")) = False Then
                            txtSaleEmail.Value = mySqlReader("semail")
                        Else
                            txtSaleEmail.Value = ""
                        End If
                        '------------------------END-----------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)


        End Try
    End Sub
#End Region

#Region "Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnSalesDetail_Click1(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSalesDetail_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = False
        PanelSales.Visible = True
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelUser.Visible = False
        PanelSurvey.Visible = False
        PanelVisit.Visible = False
        PanelWebApproval.Visible = False
        SetFocus(txtSaleRecommended)
        '  ShowRecord(Session("RefCode"))
        GetValuesForSalesDetails()
    End Sub
#End Region

#Region "Protected Sub BtnUserEmail_Click1(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnUserEmail_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservstion.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelUser.Visible = True
        PanelSurvey.Visible = False
        PanelVisit.Visible = False
        PanelWebApproval.Visible = False
        SetFocus(txtSaleTelephone1)
        '  ShowRecord(Session("RefCode"))
        GetValuesForEmailDetails()
    End Sub
#End Region

#Region "Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnResSave.Click"
    Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnResSave.Click
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateres_agentmast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updateres_agentmast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 100)).Value = CType(txtResAddress1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = CType(txtResAddress2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = CType(txtResAddress3.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(txtResPhone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = CType(txtResPhone2.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(txtResFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = CType(txtResContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = CType(txtResContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtResEmail.Value.Trim, String)



                    If ddlCommunicateBy.SelectedValue = "Email" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int)).Value = 1
                    ElseIf ddlCommunicateBy.SelectedValue = "Fax" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    'If CheckDeleteRelationShip() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Private Sub GetValuesForResvationDetails()"
    Private Sub GetValuesForResvationDetails()
        Try
            If Session("State") = "Edit" Or Session("State") = "View" Or Session("State") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '------------------------------------------------------
                        '-------------- Reservation Details --------------------------
                        If IsDBNull(mySqlReader("add1")) = False Then
                            txtResAddress1.Value = mySqlReader("add1")
                        Else
                            txtResAddress1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("add2")) = False Then
                            txtResAddress2.Value = mySqlReader("add2")
                        Else
                            txtResAddress2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("add3")) = False Then
                            txtResAddress3.Value = mySqlReader("add3")
                        Else
                            txtResAddress3.Value = ""
                        End If
                        If IsDBNull(mySqlReader("tel1")) = False Then
                            txtResPhone1.Value = mySqlReader("tel1")
                        Else
                            txtResPhone1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("tel2")) = False Then
                            txtResPhone2.Value = mySqlReader("tel2")
                        Else
                            txtResPhone2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("fax")) = False Then
                            txtResFax.Value = mySqlReader("fax")
                        Else
                            txtResFax.Value = ""
                        End If
                        If IsDBNull(mySqlReader("contact1")) = False Then
                            txtResContact1.Value = mySqlReader("contact1")
                        Else
                            txtResContact1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("contact2")) = False Then
                            txtResContact2.Value = mySqlReader("contact2")
                        Else
                            txtResContact2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txtResEmail.Value = mySqlReader("email")
                        Else
                            txtResEmail.Value = ""
                        End If
                        If IsDBNull(mySqlReader("commmode")) = False Then
                            If mySqlReader("commmode") = "1" Then
                                ddlCommunicateBy.SelectedValue = "Email"
                            ElseIf mySqlReader("commmode") = "0" Then
                                ddlCommunicateBy.SelectedValue = "Fax"
                            End If
                        End If

                        '------------------------END-----------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region " Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAccSave.Click"
    Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAccSave.Click
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    '-----------    Validate Page   ---------------
                    If ddlAccCode.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Control A/C Code field can not be blank.');", True)
                        SetFocus(ddlAccCode)
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_agentmast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updateacc_agentmast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel1", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel2", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@afax", SqlDbType.VarChar, 50)).Value = CType(txtAccFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact1", SqlDbType.VarChar, 100)).Value = CType(txtAccContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact2", SqlDbType.VarChar, 100)).Value = CType(txtAccContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@aemail", SqlDbType.VarChar, 100)).Value = CType(txtAccEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crdays", SqlDbType.Int, 4)).Value = CType(Val(TxtAccCreditDays.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crlimit", SqlDbType.Int, 4)).Value = CType(Val(txtAccCreditLimit.Value.Trim), Long)
                    If ChkCashSup.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashclient", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkCashSup.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashclient", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingcrlimit", SqlDbType.Money)).Value = CType(Val(txtAccBooking.Value.Trim), Decimal)
                    mySqlCmd.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = CType(ddlPostCode.Items(ddlPostCode.SelectedIndex).Text.Trim, String)
                    If ChkAccBooking2.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@bookcrlimitchk", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkAccBooking2.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@bookcrlimitchk", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub GetValuesForAccountDetails()"
    Private Sub GetValuesForAccountDetails()
        Try
            If Session("State") = "Edit" Or Session("State") = "View" Or Session("State") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then

                        '---------  Account Details ------------------------------------
                        If IsDBNull(mySqlReader("atel1")) = False Then
                            txtAccTelephone1.Value = mySqlReader("atel1")
                        Else
                            txtAccTelephone1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("atel1")) = False Then
                            txtAccTelephone1.Value = mySqlReader("atel1")
                        Else
                            txtAccTelephone1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("atel2")) = False Then
                            txtAccTelephone2.Value = mySqlReader("atel2")
                        Else
                            txtAccTelephone2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("afax")) = False Then
                            txtAccFax.Value = mySqlReader("afax")
                        Else
                            txtAccFax.Value = ""
                        End If
                        If IsDBNull(mySqlReader("acontact1")) = False Then
                            txtAccContact1.Value = mySqlReader("acontact1")
                        Else
                            txtAccContact1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("acontact2")) = False Then
                            txtAccContact2.Value = mySqlReader("acontact2")
                        Else
                            txtAccContact2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("aemail")) = False Then
                            txtAccEmail.Value = mySqlReader("aemail")
                        Else
                            txtAccEmail.Value = ""
                        End If
                        If IsDBNull(mySqlReader("crdays")) = False Then
                            TxtAccCreditDays.Value = mySqlReader("crdays")
                        Else
                            TxtAccCreditDays.Value = ""
                        End If
                        If IsDBNull(mySqlReader("crlimit")) = False Then
                            txtAccCreditLimit.Value = mySqlReader("crlimit")
                        Else
                            txtAccCreditLimit.Value = ""
                        End If

                        If IsDBNull(mySqlReader("cashclient")) = False Then
                            If mySqlReader("cashclient") = 1 Then
                                ChkCashSup.Checked = True
                            Else
                                ChkCashSup.Checked = False
                            End If
                        End If
                        If IsDBNull(mySqlReader("bookingcrlimit")) = False Then
                            txtAccBooking.Value = mySqlReader("bookingcrlimit")
                        Else
                            txtAccBooking.Value = ""
                        End If
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostCode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 and  agentcode<>'" & txtCustomerCode.Value.Trim & "'  order by agentcode", True)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostName, "agentname", "agentcode", "select  agentname,agentcode from agentmast where active=1 and  agentcode<>'" & txtCustomerCode.Value.Trim & "'  order by agentname", True)

                        If IsDBNull(mySqlReader("postaccount")) = False Then
                            ddlPostName.Value = mySqlReader("postaccount")
                            ddlPostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentmast", "agentname", "agentcode", mySqlReader("postaccount"))
                        Else
                            ddlPostName.Value = "[Select]"
                            ddlPostCode.Value = "[Select]"
                        End If

                        If IsDBNull(mySqlReader("controlacctcode")) = False Then
                            ddlAccName.Value = mySqlReader("controlacctcode")
                            ddlAccCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                        Else
                            ddlAccCode.Value = "[Select]"
                            ddlAccName.Value = "[Select]"
                        End If

                        If IsDBNull(mySqlReader("bookcrlimitchk")) = False Then
                            If mySqlReader("bookcrlimitchk") = 1 Then
                                ChkAccBooking2.Checked = True
                            Else
                                ChkAccBooking2.Checked = False
                            End If
                        End If


                        '------------------------END-----------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region " Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGeneralSave.Click"
    Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGeneralSave.Click
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatecom_agentmast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updatecom_agentmast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@general", SqlDbType.Text)).Value = CType(txtGeneral.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "   Private Sub GetValuesForGeneralDetails()"
    Private Sub GetValuesForGeneralDetails()
        Try
            'If Session("State") = "Edit" Then
            If Session("State") = "Edit" Or Session("State") = "View" Or Session("State") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  General Details ------------------------------------
                        If IsDBNull(mySqlReader("general")) = False Then
                            txtGeneral.Text = mySqlReader("general")
                        Else
                            txtGeneral.Text = ""
                        End If
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region "Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnUserDetailAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnUserDetailAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLines()
    End Sub
#End Region

#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("no", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region

#Region "Private Sub AddLines()"
    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Email.Rows.Count + 1
        Dim txt As HtmlInputText
        Dim name(count) As String
        Dim email(count) As String
        Dim contact(count) As String
        Dim n As Integer = 0
        Try
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                name(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtEmail")
                email(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtContactNo")
                contact(n) = CType(Trim(txt.Value), String)
                n = n + 1
            Next
            fillgrd(gv_Email, False, gv_Email.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_Email.Rows
                If n = i Then
                    Exit For
                End If

                txt = GVRow.FindControl("txtPerson")
                txt.Value = name(n)
                txt = GVRow.FindControl("txtEmail")
                txt.Value = email(n)
                txt = GVRow.FindControl("txtContactNo")
                txt.Value = contact(n)
                n = n + 1
            Next
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                ' txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtContactNo")
                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnUserSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnUserSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    If ValidateEmail() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In gv_Email.Rows
                        txtName = GvRow.FindControl("txtPerson")
                        txtEmail = GvRow.FindControl("txtEmail")
                        txtContact = GvRow.FindControl("txtContactNo")
                        If CType(txtName.Value, String) <> "" And CType(txtEmail.Value, String) <> "" And CType(txtContact.Value, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_agentmast_mulltiemail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactperson", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtEmail.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactno", SqlDbType.VarChar, 50)).Value = CType(txtContact.Value.Trim, String)
                            '  mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Private Sub GetValuesForEmailDetails()"
    Private Sub GetValuesForEmailDetails()
        Try
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txt As HtmlInputText
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from agentmast_mulltiemail Where agentcode='" & Session("RefCode") & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast_mulltiemail Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In gv_Email.Rows

                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("contactperson")) = False Then
                            txt = GVRow.FindControl("txtPerson")
                            txt.Value = mySqlReader("contactperson")
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txt = GVRow.FindControl("txtEmail")
                            txt.Value = mySqlReader("email")
                        End If
                        If IsDBNull(mySqlReader("contactno")) = False Then
                            txt = GVRow.FindControl("txtContactNo")
                            txt.Value = mySqlReader("contactno")
                        End If
                    End If
                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region " Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean"
    Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean
        Try
            Dim email1length As Integer
            email1length = Len(email.Trim)
            If email1length > 255 Then
                'objcommon.MessageBox("email1 length is too large..please enter valid email exampele(abc@abc.com)..", Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email length is too large..please enter valid email exampele(abc@abc.com).');", True)
                SetFocus(txt)
                Me.Page.SetFocus(txt)
                EmailValidate = False
                Exit Function
            Else
                Dim atpos As String
                Dim dotpos As String
                Dim s1 As String
                Dim s As String
                s1 = email
                atpos = s1.LastIndexOf("@")
                dotpos = s1.LastIndexOf(".")
                s = s1.LastIndexOf(".")
                If atpos < 1 Or dotpos < 2 Or s < 4 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                    SetFocus(txt)
                    EmailValidate = False
                    Exit Function
                Else
                    Dim sp As String()
                    Dim at As String()
                    Dim dot As String()
                    Dim chkcom As String
                    Dim chkyahoo As String
                    Dim test As String
                    Dim t As String
                    sp = s1.Split(".")
                    at = s1.Split("@")
                    chkcom = sp.GetValue(sp.Length() - 1)
                    chkyahoo = at.GetValue(at.Length() - 1)
                    dot = chkyahoo.Split(".")
                    If dot.Length() > 2 Then
                        t = dot.GetValue(dot.Length() - 3)
                        test = sp.GetValue(sp.Length() - 2)
                        If test <> "co" Or chkcom.Length() > 2 Or IsNumeric(t) = True Then
                            'objutil.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    Else
                        t = dot.GetValue(dot.Length() - 2)
                        test = sp.GetValue(sp.Length() - 1)
                        If test.Length < 2 Or IsNumeric(t) = True Or IsNumeric(test) = True Then
                            'objcommon.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    End If
                End If
            End If
            EmailValidate = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function

#End Region

#Region " Private Function ValidateEmail() As Boolean"
    Private Function ValidateEmail() As Boolean
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GVRow As GridViewRow
        Dim FLAG As Boolean = False
        Try
            For Each GVRow In gv_Email.Rows
                txtName = GVRow.FindControl("txtPerson")
                txtEmail = GVRow.FindControl("txtEmail")
                txtContact = GVRow.FindControl("txtContactNo")
                If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                    FLAG = True
                End If
            Next

            If FLAG = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter at least one email details.');", True)
                ValidateEmail = False
                Exit Function
            Else

                For Each GVRow In gv_Email.Rows
                    txtName = GVRow.FindControl("txtPerson")
                    txtEmail = GVRow.FindControl("txtEmail")
                    txtContact = GVRow.FindControl("txtContactNo")
                    If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                        If txtName.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact Person field can not be blank.');", True)
                            SetFocus(txtName)
                            ValidateEmail = False
                            Exit Function
                        End If
                        If txtEmail.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email field can not be blank.');", True)
                            SetFocus(txtEmail)
                            ValidateEmail = False
                            Exit Function
                        Else
                            If EmailValidate(txtEmail.Value.Trim, txtEmail) = False Then
                                SetFocus(txtEmail)
                                ValidateEmail = False
                                Exit Function
                            End If
                        End If
                        If txtContact.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact no field can not be blank.');", True)
                            SetFocus(txtContact)
                            ValidateEmail = False
                            Exit Function
                        End If

                    End If
                Next
            End If
            ValidateEmail = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region

#Region "Protected Sub btnVisitFollo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVisitFollow.Click"
    Protected Sub btnVisitFollo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVisitFollow.Click
        AddLinesVisit()
    End Sub
#End Region

#Region "Protected Sub Btnaddsurvey_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub Btnaddsurvey_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLinesSurvey()
    End Sub
#End Region

#Region "Private Sub AddLinesSurvey()"
    Private Sub AddLinesSurvey()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grvSurvey.Rows.Count + 1
        Dim txt As HtmlInputText
        Dim fDate(count) As String
        Dim datesurv As New EclipseWebSolutions.DatePicker.DatePicker
        Dim submit(count) As String
        Dim remark(count) As String
        ' Dim dateS(count) As Date
        'Dim chk(count) As Boolean
        Dim n As Integer = 0
        Try
            For Each GVRow In grvSurvey.Rows
                txt = GVRow.FindControl("txtSubmitedBy")
                submit(n) = CType(Trim(txt.Value), String)
                txt = Nothing
                txt = GVRow.FindControl("txtRemarkSurvey")
                remark(n) = CType(Trim(txt.Value), String)
                datesurv = GVRow.FindControl("dpDateSurvey")
                fDate(n) = CType(datesurv.txtDate.Text, String)
                n = n + 1
            Next
            fillgrd(grvSurvey, False, grvSurvey.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grvSurvey.Rows
                If n = i Then
                    Exit For
                End If
                txt = GVRow.FindControl("txtSubmitedBy")
                txt.Value = submit(n)
                txt = Nothing
                txt = GVRow.FindControl("txtRemarkSurvey")
                txt.Value = remark(n)
                datesurv = GVRow.FindControl("dpDateSurvey")
                datesurv.txtDate.Text = fDate(n)
                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnSurveySave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSurveySave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim submit As HtmlInputText
        Dim remark As HtmlInputText
        Dim datesurv As New EclipseWebSolutions.DatePicker.DatePicker
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Or Session("State") = "View" Then

                    'If ValidateEmail() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("delete from agentmast_survey where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In grvSurvey.Rows
                        submit = GvRow.FindControl("txtSubmitedBy")
                        remark = GvRow.FindControl("txtRemarkSurvey")
                        datesurv = GvRow.FindControl("dpDateSurvey")
                        If CType(submit.Value, String) <> "" And CType(remark.Value, String) <> "" And CType(datesurv.txtDate.Text, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_agentsurvey", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@submittedby", SqlDbType.VarChar, 100)).Value = CType(submit.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(remark.Value.Trim.Trim, String)
                            If datesurv.txtDate.Text = "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@surveydate", SqlDbType.DateTime)).Value = DBNull.Value
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@surveydate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(datesurv.txtDate.Text)
                            End If
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_Del_agentsurvey", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("delete from agentmast_survey where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub GetValuesForSurveys()"

    Private Sub GetValuesForSurveys()
        Try
            If Session("State") = "Edit" Or Session("State") = "View" Or Session("State") = "Delete" Then
                Dim count As Long
                Dim GVRow As GridViewRow
                Dim txt As HtmlInputText
                Dim datesurv As New EclipseWebSolutions.DatePicker.DatePicker
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select count(*) from agentmast_survey Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                count = mySqlCmd.ExecuteScalar
                mySqlCmd.Dispose()
                mySqlConn.Close()
                If count > 0 Then
                    fillgrd(grvSurvey, False, count)
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    mySqlCmd = New SqlCommand("Select * from agentmast_survey Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                    mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                    For Each GVRow In grvSurvey.Rows

                        If mySqlReader.Read = True Then
                            If IsDBNull(mySqlReader("submittedby")) = False Then
                                txt = GVRow.FindControl("txtSubmitedBy")
                                txt.Value = mySqlReader("submittedby")
                            End If
                            If IsDBNull(mySqlReader("remarks")) = False Then
                                txt = GVRow.FindControl("txtRemarkSurvey")
                                txt.Value = mySqlReader("remarks")
                            End If
                            If IsDBNull(mySqlReader("surveydate")) = False Then
                                datesurv = GVRow.FindControl("dpDateSurvey")
                                'datesurv.txtDate.Text = mySqlReader("surveydate")
                                datesurv.txtDate.Text = Format("U", CType((mySqlReader("surveydate")), Date))
                            End If
                        End If
                    Next
                    mySqlCmd.Dispose()
                    mySqlReader.Close()
                    mySqlConn.Close()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region " Protected Sub BtnVisitCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnVisitCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnUserCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnUserCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region " Protected Sub BtnSurveyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSurveyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnVisitSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnVisitSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim remark As HtmlInputText
        Dim datevisit As New EclipseWebSolutions.DatePicker.DatePicker
        ' Dim ddlSalesPerson As DropDownList
        Dim GvRow As GridViewRow
        Dim txtSalesPerson As HtmlInputText
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Or Session("State") = "View" Then

                    'If ValidateEmail() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("delete from agentmast_visit where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In gv_VisitFollow.Rows
                        txtSalesPerson = GvRow.FindControl("txtSalesPersonName")
                        remark = GvRow.FindControl("txtRemark")
                        datevisit = GvRow.FindControl("dpDateVisit")
                        ddlPcode = GvRow.FindControl("ddlSalesPersonCode")
                        If CType(txtSalesPerson.Value, String) <> "" And CType(remark.Value, String) <> "" And CType(datevisit.txtDate.Text, String) <> "" And CType(ddlPcode.SelectedValue, String) <> "[Select]" Then
                            mySqlCmd = New SqlCommand("sp_add_agentvisit", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 20)).Value = CType(ddlPcode.SelectedValue, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(remark.Value.Trim, String)
                            If datevisit.txtDate.Text = "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@visitdate", SqlDbType.DateTime)).Value = DBNull.Value
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@visitdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(datevisit.txtDate.Text)
                            End If
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_Del_agentsurvey", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("delete from agentmast_visit where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub GetValuesForVisit()"
    Private Sub GetValuesForVisit()
        Try
            If Session("State") = "Edit" Or Session("State") = "View" Or Session("State") = "Delete" Then
                Dim remark As HtmlInputText
                Dim datevisit As New EclipseWebSolutions.DatePicker.DatePicker
                ' Dim ddl As HtmlSelect
                Dim count As Long
                Dim GVRow As GridViewRow
                Dim txtSalesPersonName As HtmlInputText
                'Dim txtSalesPersonName As HtmlInputText
                'txtSalesPersonName = GVRow.FindControl("txtSalesPersonName")

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select count(*) from agentmast_visit Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                count = mySqlCmd.ExecuteScalar
                mySqlCmd.Dispose()
                mySqlConn.Close()
                If count > 0 Then
                    fillgrd(gv_VisitFollow, False, count)
                    'For Each GVRow In gv_VisitFollow.Rows
                    '    ddl = GVRow.FindControl("ddlSalesPersonCode")
                    '    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddl, "UserCode", "select UserCode from UserMaster where active=1 order by UserCode", True)
                    'Next
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    mySqlCmd = New SqlCommand("Select * from agentmast_visit Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                    mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                    For Each GVRow In gv_VisitFollow.Rows

                        If mySqlReader.Read = True Then
                            If IsDBNull(mySqlReader("spersoncode")) = False Then
                                ddlPcode = GVRow.FindControl("ddlSalesPersonCode")
                                objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlPcode, "UserCode", "select UserCode from UserMaster where active=1 order by UserCode", True)
                                ddlPcode.SelectedValue = mySqlReader("spersoncode").ToString.Trim
                                txtSalesPersonName = GVRow.FindControl("txtSalesPersonName")
                                txtSalesPersonName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "UserMaster", "UserName", "UserCode", ddlPcode.SelectedValue)
                            End If

                            If IsDBNull(mySqlReader("remarks")) = False Then
                                remark = GVRow.FindControl("txtRemark")
                                remark.Value = mySqlReader("remarks")
                            End If
                            If IsDBNull(mySqlReader("visitdate")) = False Then
                                datevisit = GVRow.FindControl("dpDateVisit")
                                'datevisit.txtDate.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("visitdate"))
                                datevisit.txtDate.Text = Format("U", CType((mySqlReader("visitdate")), Date))
                            End If
                        End If

                    Next
                    mySqlCmd.Dispose()
                    mySqlReader.Close()
                    mySqlConn.Close()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region "Private Sub AddLinesVisit()"
    Private Sub AddLinesVisit()
        Dim count As Integer
        Dim GVRow As GridViewRow

        count = gv_VisitFollow.Rows.Count + 1

        Dim txt As HtmlInputText
        'Dim ddlSalesPerson As DropDownList
        Dim fDate(count) As String
        Dim datevisit As New EclipseWebSolutions.DatePicker.DatePicker
        Dim salesperson(count) As String
        Dim remark(count) As String
        Dim ddl(count) As String
        'Dim chk(count) As Boolean
        Dim n As Integer = 0
        Try
            For Each GVRow In gv_VisitFollow.Rows
                txt = GVRow.FindControl("txtSalesPersonName")
                salesperson(n) = CType(Trim(txt.Value), String)
                txt = Nothing
                txt = GVRow.FindControl("txtRemark")
                remark(n) = CType(Trim(txt.Value), String)
                datevisit = GVRow.FindControl("dpDateVisit")
                fDate(n) = CType(datevisit.txtDate.Text, String)
                ddlPcode = GVRow.FindControl("ddlSalesPersonCode")
                ddl(n) = CType(ddlPcode.SelectedValue, String)
                n = n + 1
            Next
            fillgrd(gv_VisitFollow, False, gv_VisitFollow.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_VisitFollow.Rows
                If n > i Then
                    Exit For
                End If
                txt = GVRow.FindControl("txtSalesPersonName")
                txt.Value = salesperson(n)
                txt = Nothing
                txt = GVRow.FindControl("txtRemark")
                txt.Value = remark(n)
                datevisit = GVRow.FindControl("dpDateVisit")
                datevisit.txtDate.Text = fDate(n)
                ddlPcode = GVRow.FindControl("ddlSalesPersonCode")
                objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlPcode, "UserCode", "select UserCode from UserMaster where active=1  order by UserCode", True)
                If ddl(n) = "[Select]" Or ddl(n) = Nothing Then
                    ddlPcode.SelectedValue = "[Select]"
                Else
                    ddlPcode.SelectedValue = ddl(n)
                End If

                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#End Region



#Region "Protected Sub BtnShowPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnShowPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            mySqlCmd = New SqlCommand("GenerateRandomString", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@useNumbers", SqlDbType.Bit)).Value = 1
            mySqlCmd.Parameters.Add(New SqlParameter("@useLowerCase", SqlDbType.Bit)).Value = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@useUpperCase", SqlDbType.Bit)).Value = 1
            mySqlCmd.Parameters.Add(New SqlParameter("@charactersToUse", SqlDbType.VarChar, 100)).Value = System.DBNull.Value
            mySqlCmd.Parameters.Add(New SqlParameter("@passwordLength", SqlDbType.SmallInt, 9)).Value = 7

            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@password"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            mySqlCmd.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(mySqlCmd)
            mySqlCmd.ExecuteNonQuery()
            'lblPwd.Text = param.Value
            txtWebAppPassword.Value = param.Value
            'End If
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close



        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnWebAppSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnWebAppSave.Click"

    Protected Sub BtnWebAppSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnWebAppSave.Click
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateweb_agentmast", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updateweb_agentmast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@webusername ", SqlDbType.VarChar, 20)).Value = CType(txtWebAppUsername.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@webpassword ", SqlDbType.VarChar, 10)).Value = CType(txtWebAppPassword.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@webcontact", SqlDbType.VarChar, 100)).Value = CType(txtWebAppContact.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@webemail", SqlDbType.VarChar, 100)).Value = CType(txtWebAppEmail.Value.Trim, String)
                    If ChkWebApprove.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkWebApprove.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub GetValuesForWebApprv()"
    Private Sub GetValuesForWebApprv()
        Try
            'If Session("State") = "Edit" Or Session("State") = "Edit" Or Session("State") = "Delete" Then
            If Session("State") = "Edit" Or Session("State") = "View" Or Session("State") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  Web Approval ------------------------------------
                        If IsDBNull(mySqlReader("webusername")) = False Then
                            txtWebAppUsername.Value = mySqlReader("webusername")
                        Else
                            txtWebAppUsername.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webpassword")) = False Then
                            txtWebAppPassword.Value = mySqlReader("webpassword")
                        Else
                            txtWebAppPassword.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webcontact")) = False Then
                            txtWebAppContact.Value = mySqlReader("webcontact")
                        Else
                            txtWebAppContact.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webemail")) = False Then
                            txtWebAppEmail.Value = mySqlReader("webemail")
                        Else
                            txtWebAppEmail.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webapprove")) = False Then
                            If CType(mySqlReader("webapprove"), String) = "1" Then
                                ChkWebApprove.Checked = True
                            ElseIf CType(mySqlReader("webapprove"), String) = "0" Then
                                ChkWebApprove.Checked = False
                            End If
                        End If

                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region "Protected Sub BtnWebAppCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnWebAppCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnWebInviteCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnWebInviteCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strEmailText As String = ""
        Dim strSubject As String = ""
        Dim to_email As String = ""

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))            'connection open
            'mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='563'", mySqlConn)
            mySqlCmd = New SqlCommand("Select * from email_text", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("emailtext")) = False Then
                        strEmailText = CType(mySqlReader("emailtext"), String)
                    End If
                    If IsDBNull(mySqlReader("subject")) = False Then
                        strSubject = CType(mySqlReader("subject"), String)
                    End If
                End If
            End If
            If strEmailText <> "" And strSubject <> "" Then
                Dim Mail_Message As New MailMessage()
                Dim msClient As New SmtpClient
                If txtWebAppEmail.Value.Trim <> "" Then
                    strEmailText = "<html>" & strEmailText & "<BR><BR> Login Account Info <BR> Username = " & txtWebAppUsername.Value.Trim & "<BR> Password = " & txtWebAppPassword.Value.Trim & "</html>"
                    to_email = txtWebAppEmail.Value.Trim
                    Dim from_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
                    Dim frmadd As New MailAddress(from_email)
                    msClient.Host = "localhost" '"127.0.0.1"
                    msClient.Port = 25
                    Mail_Message.From = frmadd
                    Mail_Message.To.Add(Trim(to_email))
                    Mail_Message.Subject = strSubject
                    Mail_Message.Body = strEmailText
                    Mail_Message.Priority = MailPriority.Normal
                    Mail_Message.IsBodyHtml = True
                    msClient.Send(Mail_Message)
                    Mail_Message = Nothing
                    to_email = ""

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('E-Mail sending successfully.');", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter e-mail id.');", True)
                    SetFocus(txtWebAppEmail)
                    Exit Sub
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customer.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnWebResendPasswprd_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnWebResendPasswprd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strEmailText As String = ""
        Dim strSubject As String = ""
        Dim to_email As String = ""

        Dim Mail_Message As New MailMessage()
        Dim msClient As New SmtpClient
        If txtWebAppEmail.Value.Trim <> "" Then
            strEmailText = "<html> Login Account Info <BR><BR> Username = " & txtWebAppUsername.Value.Trim & "<BR> Password = " & txtWebAppPassword.Value.Trim & "</html>"
            to_email = txtWebAppEmail.Value.Trim
            Dim from_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
            Dim frmadd As New MailAddress(from_email)
            msClient.Host = "localhost" '"127.0.0.1"
            msClient.Port = 25
            Mail_Message.From = frmadd
            Mail_Message.To.Add(Trim(to_email))
            Mail_Message.Subject = "Login Account Info"
            Mail_Message.Body = strEmailText
            Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True
            msClient.Send(Mail_Message)
            Mail_Message = Nothing
            to_email = ""

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('E-Mail sending successfully.');", True)
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter e-mail id.');", True)
            SetFocus(txtWebAppEmail)
            Exit Sub
        End If
    End Sub
#End Region

#Region "Protected Sub ddlSalesPersonCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub ddlSalesPersonCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRow As GridViewRow
        Dim txt As HtmlInputText
        For Each GVRow In gv_VisitFollow.Rows
            ddlPcode = GVRow.FindControl("ddlSalesPersonCode")
            txt = GVRow.FindControl("txtSalesPersonName")
            'txt.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"UserMaster", "UserName", "UserCode", ddlSalesPerson.Value)
            txt.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "UserMaster", "UserName", "UserCode", ddlPcode.SelectedValue)
        Next
    End Sub
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"),"agentmast_mulltiemail", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a MultiEmail Of Customer Sectors, cannot delete this Customer');", True)
        '    checkForDeletion = False
        '    Exit Function

        'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"),"agentmast_survey", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a SurveyOfCustomers, cannot delete this Customer');", True)
        'checkForDeletion = False
        'Exit Function

        'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"),"agentmast_visit", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a VisitsOfCustomers, cannot delete this Customer');", True)
        'checkForDeletion = False
        'Exit Function

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromagent_detail", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a DetailsOfEarlyBirdPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_agent", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a CustomerPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If

        checkForDeletion = True
    End Function
#End Region
End Class

'Public Sub ddlSalesPersonCode_ServerChange(ByVal sender As Object, ByVal e As System.EventArgs)
'    Dim GVRow As GridViewRow
'    Dim txt As HtmlInputText
'    For Each GVRow In gv_VisitFollow.Rows
'        ddlSalesPerson = GVRow.FindControl("ddlSalesPersonCode")
'        txt = GVRow.FindControl("txtSalesPersonName")
'        txt.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"UserMaster", "UserName", "UserCode", ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text)
'    Next
'End Sub
'#Region "Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtCategoryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"agentcatmast", "agentcatname", "agentcatcode", CType(ddlCategory.SelectedValue.Trim, String))
'    '    SetFocus(ddlCategory)
'    'End Sub
'#End Region

'#Region "Protected Sub ddlSelling_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlSelling_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellmast", "sellmast.sellname", "sellmast.sellcode", CType(ddlSelling.SelectedValue.Trim, String))
'    '    SetFocus(ddlSelling)
'    'End Sub
'#End Region

'#Region "Protected Sub ddlOtherSell_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlOtherSell_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtOthersellname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othsellmast", "othsellmast.othsellname", "othsellcode", CType(ddlOtherSell.SelectedValue.Trim, String))
'    'End Sub
'#End Region

'#Region "Protected Sub ddlTicketSelling_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlTicketSelling_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtTicketname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"tktsellmast", "tktsellname", "tktsellcode", CType(ddlTicketSelling.SelectedValue.Trim, String))
'    'End Sub
'#End Region

'#Region "Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", CType(ddlCurrency.SelectedValue.Trim, String))
'    'End Sub
'#End Region

'#Region "Protected Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(ddlCountry.SelectedValue.Trim, String))
'    '    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCity, "citycode", "select citycode from citymast where active=1 and  ctrycode='" & ddlCountry.SelectedValue & "'", True)
'    '    'SetFocus(ddlCategory)
'    'End Sub
'#End Region

'#Region "Protected Sub ddlSector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlSector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"agent_sectormaster", "sectorname", "sectorcode", CType(ddlSector.SelectedValue.Trim, String))
'    'End Sub
'#End Region

'#Region "Protected Sub ddlMarket_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlMarket_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtMarketName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"plgrpmast", "plgrpname", "plgrpcode", CType(ddlMarket.SelectedValue.Trim, String))
'    'End Sub
'#End Region

'#Region "Protected Sub ddlSalesPerson_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlSalesPerson_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtSalesPersonName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"UserMaster", "UserName", "UserCode", CType(ddlSalesPerson.SelectedValue.Trim, String))
'    'End Sub
'#End Region

'#Region "Protected Sub ddlCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    'Protected Sub ddlCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    txtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"citymast", "cityname", "citycode", CType(ddlCity.SelectedValue.Trim, String))
'    'End Sub
'#End Region

'For Each GVRow In gv_VisitFollow.Rows
'    Dim ddl As HtmlSelect
'    ddl = GVRow.FindControl("ddlSalesPersonCode")
'    ddl.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'Next

'Dim ddl As DropDownList
'Dim txt As HtmlInputText


'btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want Update?')==false)return false;")
'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")
'BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")
'BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")
'BtnUserSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to  Update?')==false)return false;")
'BtnWebAppSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")
'BtnVisitSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")
'BtnSurveySave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")
'BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")
'BtnUserSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update?')==false)return false;")

'        BtnAdd.Visible = False

'If IsDBNull(mySqlReader("ctrycode")) = False Then
'    Me.ddlCCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
'    Me.ddlcname.Value = CType(mySqlReader("ctrycode"), String)

'End If
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAccPostTo, "agentcode", "select * from agentmast where active=1 and  agentcode<>'" & txtCustomerCode.Value.Trim & "' order by agentcode", True)
'ddlAccPostTo.SelectedValue = mySqlReader("postaccount")
'txtAccPostTo2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"supplier_agents", "supagentname", "supagentcode", ddlAccPostTo.SelectedValue)
'ddlAccPostTo.SelectedItem.Text = "[Select]"
'txtAccPostTo2.Value = ""
'If IsDBNull(mySqlReader("controlacctcode")) = False Then
'    txtAccACCode.Value = mySqlReader("controlacctcode")
'Else
'    txtAccACCode.Value = ""
'End If
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAccPostTo, "agentcode", "SELECT agentcode FROM agentmast WHERE active=1 and  agentcode<>'" & txtCustomerCode.Value.Trim & "' ORDER BY agentcode", True)

'#Region "Protected Sub ddlAccPostTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    Protected Sub ddlAccPostTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'        If ddlAccPostTo.SelectedValue = "[Select]" Then
'            txtAccPostTo2.Value = ""
'        Else
'            txtAccPostTo2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"agentmast", "agentname", "agentcode", ddlAccPostTo.SelectedValue)
'        End If
'    End Sub
'#End Region
'ddlAccPostTo.Visible = False
'txtAccPostTo2.Disabled = True
'txtAccACCode.Disabled = True
'If txtAccACCode.Value.Trim = "" Then

'If IsDBNull(mySqlReader("cashsupagent")) = False Then
'                            If mySqlReader("cashsupagent") = 1 Then
'                                ChkCashSup.Checked = True
'                            Else
'                                ChkCashSup.Checked = False
'                            End If
'                        End If

'If IsDBNull(mySqlReader("bookcrlimitchk")) = False Then
'                           If mySqlReader("bookcrlimitchk") = 1 Then
'                               ChkCashSup.Checked = True
'                           Else
'                               ChkCashSup.Checked = False
'                           End If
'                       End If