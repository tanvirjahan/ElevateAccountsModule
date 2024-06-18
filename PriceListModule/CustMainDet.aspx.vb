Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.IO
Imports System.Collections.Generic

Partial Class CustMainDet
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Private strImgName As String

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim existclientflag As Integer
    Shared sectorvalue As Integer
    Dim ExtAppIDflag As String  'implementing for external app id eg;juniper id
    Dim ExtAppIDexist As Boolean
#End Region

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetMarketlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Controlaccnames As New List(Of String)
        Try
            'Dim strDivCode As String = ""
            'If Not HttpContext.Current.Session("sCDivCode") Is Nothing Then
            '    strDivCode = HttpContext.Current.Session("sCDivCode")
            'End If

            'If strDivCode = "" Then
            strSqlQry = "select marketcode,marketname from marketmast where active=1  and  marketname like  '" & Trim(prefixText) & "%'  "
            'Else
            '    strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1  and  plgrpname like      '" & Trim(prefixText) & "%'  " 'and div_code='" & HttpContext.Current.Session("sCDivCode") & "'
            'End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Controlaccnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("marketname").ToString(), myDS.Tables(0).Rows(i)("marketcode").ToString()))
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
    Public Shared Function Getcontrolacclist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Controlaccnames As New List(Of String)
        Try
            Dim strDivCode As String = ""
            If Not HttpContext.Current.Session("sCDivCode") Is Nothing Then
                strDivCode = HttpContext.Current.Session("sCDivCode")
            End If

            If strDivCode = "" Then
                strSqlQry = "select acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='C'  and  acctname like  '" & Trim(prefixText) & "%'  "
            Else
                strSqlQry = "select acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='C'  and  acctname like  '" & Trim(prefixText) & "%' and div_code='" & HttpContext.Current.Session("sCDivCode") & "' "
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
    Public Shared Function Getctrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Ctrynames As New List(Of String)
        Try
            strSqlQry = "select ctrycode,ctryname from ctrymast where active=1 and  ctryname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Ctrynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))

                Next
            End If
            Return Ctrynames
        Catch ex As Exception
            Return Ctrynames
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

            If HttpContext.Current.Session("custmain_ctrycode_for_filter") IsNot Nothing Then
                ctry = HttpContext.Current.Session("custmain_ctrycode_for_filter")
            End If
            'If HttpContext.Current.Session("custmain_ctrycode_for_filter") IsNot Nothing Then
            '    city = HttpContext.Current.Session("custmain_citycode_for_filter")
            'End If
            strSqlQry = "select sectorname,sectorcode from agent_sectormaster  where  active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            'If Trim(city) <> "" Then
            '    strSqlQry = strSqlQry + " and citycode='" & Trim(city) & "'"
            'End If
            strSqlQry = strSqlQry + " and sectorname like  '" & Trim(prefixText) & "%'"

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
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getsalespersonlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""

        Dim myDS As New DataSet
        Dim SalesPersonnames As New List(Of String)
        Try


            strSqlQry = "select username,usercode from usermaster  where  active=1 and  username like  '" & Trim(prefixText) & "%'"


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    SalesPersonnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("username").ToString(), myDS.Tables(0).Rows(i)("usercode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            Else
                'dummyCity_Click(sender, e)
                'SaveSectorCode()
                sectorvalue = 0

            End If
            Return SalesPersonnames
        Catch ex As Exception
            Return SalesPersonnames
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

            If HttpContext.Current.Session("custmain_ctrycode_for_filter") IsNot Nothing Then
                ctry = HttpContext.Current.Session("custmain_ctrycode_for_filter")
            End If

            strSqlQry = "select cityname,citycode from citymast where active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            strSqlQry = strSqlQry + " and cityname like  '" & Trim(prefixText) & "%'"

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
    Public Shared Function Getdivisionslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Divisionsnames As New List(Of String)
        Try
            strSqlQry = "select division_master_code,division_master_des from division_master where  division_master_des like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Divisionsnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("division_master_des").ToString(), myDS.Tables(0).Rows(i)("division_master_code").ToString()))

                Next
            End If
            Return Divisionsnames
        Catch ex As Exception
            Return Divisionsnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentCatlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim AgentCatnames As New List(Of String)
        Try
            strSqlQry = "select agentcatcode,agentcatname from agentcatmast where active=1 and  agentcatname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    AgentCatnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentcatname").ToString(), myDS.Tables(0).Rows(i)("agentcatcode").ToString()))

                Next
            End If
            Return AgentCatnames
        Catch ex As Exception
            Return AgentCatnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function GetCurrencylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Currencynames As New List(Of String)
        Try
            strSqlQry = "select currcode,currname from currmast where active=1 and  currname like  '" & Trim(prefixText) & "%'"
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

                Next
            End If
            Return Currencynames
        Catch ex As Exception
            Return Currencynames
        End Try

    End Function
    Protected Sub TxtAgentCatName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtAgentCatName.TextChanged
        Session("custmain_agentcatcode_for_filter") = TxtAgentCatCode.Text()
    End Sub
    Protected Sub TxtCtryName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCtryName.TextChanged
        Session("custmain_ctrycode_for_filter") = TxtCtryCode.Text()
    End Sub
    Protected Sub TxtcurrName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCurrName.TextChanged
        Session("custmain_currcode_for_filter") = TxtCurrCode.Text()
    End Sub
    Protected Sub txtcityname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCityName.TextChanged
        Session("custmain_citycode_for_filter") = TxtCityCode.Text
    End Sub
    Protected Sub txtsectorname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtSectorName.TextChanged
        Session("custmain_sectorcode_for_filter") = TxtSectorCode.Text
    End Sub
    Protected Sub txtsalespersonresname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtSalesPersonResName.TextChanged
        Session("custmain_salespersoncode_for_filter") = TxtSalesPersonResCode.Text
    End Sub
    Protected Sub txtsalespersonconname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtSalesPersonConName.TextChanged
        Session("custmain_salespersonconcode_for_filter") = TxtSalesPersonConCode.Text
    End Sub
    Protected Sub TxtControlAccName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtControlAccName.TextChanged
        Session("custmain_controlaccode_for_filter") = TxtControlAccCode.Text
    End Sub
    Protected Sub TxtDivisionName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtDivisionName.TextChanged
        Session("custmain_divisioncode_for_filter") = TxtDivisionCode.Text
    End Sub

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim CurrCount As Integer
            Dim ctryCount As Integer
            Dim catCount As Integer
            Dim secCount As Integer
            Dim cityCount As Integer

            Dim CurrlngCount As Int16
            Dim CtrylngCount As Int16
            Dim seclngCount As Int16
            Dim catlngCount As Int16
            Dim citylngCount As Int16


            Dim strappname As String = ""
            Dim strTempUserFunctionalRight As String()
            Dim strsecTempUserFunctionalRight As String()
            Dim strctryTempUserFunctionalRight As String()
            Dim strcityTempUserFunctionalRight As String()
            Dim strcatTempUserFunctionalRight As String()

            Dim strcityRights As String
            Dim strctryRights As String
            Dim strsecRights As String
            Dim strcatRights As String
            Dim strCurrRights As String

            ViewState.Add("appid", Request.QueryString("appid"))
            Dim strappid As String = ViewState("appid")

            Dim groupid As String = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))

            Dim menuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CurrenciesSearch.aspx?appid=" + strappid, strappid)
            Dim functionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, menuid)

            Dim ctrymenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CountriesSearch.aspx?appid=" + strappid, strappid)
            Dim ctryfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, ctrymenuid)


            Dim catmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CustomercategoriesSearch.aspx?appid=" + strappid, strappid)
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
                    btnaddnewcurr.Visible = True
                Else
                    btnaddnewcurr.Visible = False
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
                    btnaddnewctry.Visible = True
                Else
                    btnaddnewctry.Visible = False
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
                    btnaddnewcity.Visible = True
                Else
                    btnaddnewcity.Visible = False
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
                    btnaddnewsector.Visible = True
                Else
                    btnaddnewsector.Visible = False
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
                    btnaddnewagentcat.Visible = True
                Else
                    btnaddnewagentcat.Visible = False
                End If

            End If




            existclientflag = 0
            Dim RefCode As String
            If IsPostBack = False Then

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                If CType(Request.QueryString("appid"), String) = "1" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

                ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
                ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=1", String), "1")
                End If

                ExtAppIDflag = "N" 'implementing for external app id eg;juniper id
                ExtAppIDexist = False
                PanelMain.Visible = True
                charcters(txtCustomerCode)
                charcters(txtCustomerName)
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

                txtconnection.Value = Session("dbconnectionName")


                CategoryPrivilege()

                Session("ExistClient") = existclientflag
                BindMarket()
                Dim divisionList As List(Of String) = Getdivisionslist("")
                Dim splittedDivision() As String = divisionList(0).Split(",")
                If (divisionList.Count > 0) Then
                    If (splittedDivision.Length > 0 And splittedDivision.Length > 1) Then
                        TxtDivisionName.Text = splittedDivision(0).Replace("{", "").Replace(ControlChars.Quote, "").Replace("First:", "")
                        TxtDivisionCode.Text = splittedDivision(1).Replace("}", "").Replace(ControlChars.Quote, "").Replace("Second:", "")
                    End If
                End If

                If CType(Session("CustState"), String) = "Addclient" Then
                    Session("postback") = "Addclient"


                Else
                    Session("postback") = ""
                End If


                If CType(Session("CustState"), String) = "New" Then
                    SetFocus(txtCustomerCode)
                    'hdncurncychngflag.Value = 0 'varbl to check if currency change required,so refilling occurs in client side
                    lblHeading.Text = "Add New Customer - Main Details"
                    Page.Title = Page.Title + " " + "New Customer - Main Details"
                    ' btnSave_Main.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                    'btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Customer?')==false)return false;")
                ElseIf CType(Session("CustState"), String) = "Edit" Then

                    btnSave_Main.Text = "Update"
                    'If CType(Session("ExistClient"), String) <> "0" Then
                    '    btnSave_Main.Visible = False
                    '    DisableControl()
                    'End If
                    RefCode = CType(Session("custRefCode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    If ExtAppIDflag.ToUpper = "Y" Then


                        ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_agentmast", "agentcode", txtCustomerCode.Value)
                        If ExtAppIDexist Then
                            TxtExtappid.Disabled = True
                            TxtExtappid.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_agentCode  from int_agentmast where agentcode='" & txtCustomerCode.Value & "'")
                        Else
                            TxtExtappid.Disabled = False
                        End If
                    End If

                    'txtCustomerName.Disabled = True
                    DisableControl()


                    lblHeading.Text = "Edit Customer - Main Details"
                    Page.Title = Page.Title + " " + "Edit Customer - Main Details"
                    'btnSave_Main.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                    btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Customer?')==false)return false;")
                ElseIf CType(Session("CustState"), String) = "Copy" Then

                    btnSave_Main.Text = "Save"
                    'If CType(Session("ExistClient"), String) <> "0" Then
                    '    btnSave_Main.Visible = False
                    '    DisableControl()
                    'End If
                    RefCode = CType(Session("custRefCode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Value = ""

                    ' txtCustomerName.Disabled = True
                    DisableControl()



                    lblHeading.Text = "New Customer - Main Details"
                    Page.Title = Page.Title + " " + "New Customer - Main Details"
                    'btnSave_Main.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                    'btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Customer?')==false)return false;")
                ElseIf CType(Session("CustState"), String) = "View" Then

                    RefCode = CType(Session("custRefCode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Enabled = False
                    TxtExtappid.Disabled = True
                    If ExtAppIDflag.ToUpper = "Y" Then


                        ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_agentmast", "agentcode", txtCustomerCode.Value)
                        If ExtAppIDexist Then

                            TxtExtappid.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_agentCode  from int_agentmast where agentcode='" & txtCustomerCode.Value & "'")

                        End If
                    End If

                    DisableControl()
                    lblHeading.Text = "View Customer - Main Details"
                    Page.Title = Page.Title + " " + "View Customer - Main Details"
                    btnSave_Main.Visible = False
                    btnCancel_Main.Text = "Return to Search"
                    btnCancel_Main.Focus()

                ElseIf CType(Session("CustState"), String) = "Delete" Then

                    RefCode = CType(Session("custRefCode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Enabled = False
                    TxtExtappid.Disabled = True
                    DisableControl()
                    lblHeading.Text = "Delete Customer - Main Details"
                    Page.Title = Page.Title + " " + "Delete Customer - Main Details"
                    btnSave_Main.Text = "Delete"
                    If ExtAppIDflag.ToUpper = "Y" Then


                        ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_agentmast", "agentcode", txtCustomerCode.Value)
                        If ExtAppIDexist Then

                            TxtExtappid.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_agentCode  from int_agentmast where agentcode='" & txtCustomerCode.Value & "'")

                        End If
                    End If

                    ' btnSave_Main.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                    DisableControl()

                    btnSave_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Customer?')==false)return false;")

                ElseIf CType(Session("CustState"), String) = "Addclient" Then
                    Dim regno As String
                    SetFocus(txtCustomerCode)


                    regno = CType(Session("regno"), String)

                    ShowRecord_registration(regno)
                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from agentmast where agentname like '" & txtCustomerName.Text & "' and active=1") = "t" Then
                        lblDisplay.Visible = True
                        lblDisplay.Text = "Customer Name Already Exists!!!"

                    End If
                    btnExistClient.Visible = True
                    'If ddlagentname.Visible = True Then
                    '    existclientflag = 1
                    '    Session("ExistClient") = existclientflag


                    'End If
                    'DisableControl()
                    lblHeading.Text = "Add New Customer - Main Details"
                    Page.Title = Page.Title + " " + "New Customer - Main Details"
                    btnSave_Main.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                End If
                btnCancel_Main.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                Dim typ As Type
                typ = GetType(DropDownList)

            End If
            Session.Add("submenuuser", "CustomersSearch.aspx")
            Dim tt As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 acc_code  from acc_tran(nolock) where acc_code='" & RefCode & "'")
            If tt <> "" Then
                'ddlCurrency.Disabled = True
                'ddlCurrencyName.Disabled = True
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "custcatfromWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("AgentCategoryName") IsNot Nothing Then
                    If Session("AgentCategoryCode") IsNot Nothing Then
                        Dim categoryname As String = Session("AgentCategoryName")
                        TxtAgentCatCode.Text = Session("AgentCategoryCode")
                        TxtAgentCatName.Text = categoryname
                        Session.Remove("addcategory")
                        Session.Remove("AgentCategoryName")
                        Session.Remove("AgentCategoryCode")
                    End If
                End If


            End If
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "sectorfromWindowPostBack") Then
            If Session("AgentSectorName") IsNot Nothing Then
                If Session("AgentSectorCode") IsNot Nothing Then
                    Dim sectorname As String = Session("AgentSectorName")
                    TxtSectorName.Text = sectorname
                    TxtSectorCode.Text = Session("AgentSectorCode")
                    Session.Remove("addcategory")
                    Session.Remove("AgentSectorName")
                    Session.Remove("AgentSectorCode")
                End If
            End If



        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CurrencyWindowPostBack") Then
            If Session("CurrName") IsNot Nothing Then
                If Session("CurrCode") IsNot Nothing Then
                    Dim currencyname As String = Session("CurrName")
                    TxtCurrCode.Text = Session("CurrCode")
                    TxtCurrName.Text = currencyname
                    Session.Remove("addcategory")
                    Session.Remove("CurrName")
                    Session.Remove("CurrCode")
                End If
            End If
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CityWindowPostBack") Then

            If Session("CitiesName") IsNot Nothing Then
                If Session("CitiesCode") IsNot Nothing Then
                    Dim cityname As String = Session("CitiesName")
                    TxtCityCode.Text = Session("CitiesCode")
                    TxtCityName.Text = cityname
                    Session.Remove("addcategory")
                    Session.Remove("CitiesName")
                    Session.Remove("CitiesCode")
                End If
            End If
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CtryWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("CountryName") IsNot Nothing Then
                    If Session("CountryCode") IsNot Nothing Then
                        Dim countryname As String = Session("CountryName")
                        TxtCtryCode.Text = Session("CountryCode")
                        TxtCtryName.Text = countryname
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
                ExtAppIDexist = objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_agentmast", "agentcode", txtCustomerCode.Value)
                If ExtAppIDexist Then
                    TxtExtappid.Disabled = True
                End If
            End If



        End If


    End Sub
#End Region





#Region "Protected Sub btnCancel_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel_Main.Click

        If Session("postback") <> "Addclient" Then
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Else

            Dim strscript As String = ""
            strscript = "window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


        End If

    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("CustState") = "New" Or Session("CustState") = "Addclient" Then
            Dim newstr As String = ""
            newstr = txtCustomerName.Text.Trim
            'If newstr.Contains("  ") Then

            'End If
            newstr = newstr.Replace("  ", " ")
            '' Added shahul 06/12/18
            If objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "view_account", "code", "type='C' and des", CType(newstr, String), "div_code", TxtDivisionCode.Text) <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name Already Present .');", True)
                checkForDuplicate = False
                Exit Function
            End If

            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "view_account", "des", newstr, "type='C'") Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name Already Present .');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
        ElseIf Session("CustState") = "Edit" Then
            Dim newstr As String = ""
            newstr = txtCustomerName.Text.Trim
            'If newstr.Contains("  ") Then

            'End If
            newstr = newstr.Replace("  ", " ")
            'If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "view_account", "code", "des", newstr, CType(txtCustomerCode.Value.Trim, String), "type='C'") Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name Already Present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            '' Added shahul 06/12/18
            If objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "view_account", "code", "type='C' and code<>'" & txtCustomerCode.Value & "' and des", CType(newstr, String), "div_code", TxtDivisionCode.Text) <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name Already Present .');", True)
                checkForDuplicate = False
                Exit Function
            End If


        
        End If
        'Tanvir 14072022
        If TxtMarketcode.Text = "" Or TxtMarketName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Market cannot be blank.');", True)
            checkForDuplicate = False
            Exit Function
        End If
        'Tanvir 14072022
        checkForDuplicate = True
    End Function
#End Region

    Function ValidatePhotoSize() As Boolean
        Try
            Dim MyLimit As Integer = 512000
            Dim FindSize As Integer = 0
            Dim NewSize As Double = 0





            FindSize = 0
            Dim MyAlert As String = "Upload Size Should Not Exceed 500 Kb "
            Dim MyAlert1 As String = "Only Jpg ,Bmp , Png  Files Allowed"
            If filelogo.HasFile = True Then

                If filelogo.PostedFile.ContentType = "image/jpeg" Or filelogo.PostedFile.ContentType = "image/jpg" Or filelogo.PostedFile.ContentType = "image/bmp" Or filelogo.PostedFile.ContentType = "image/x-png" Or filelogo.PostedFile.ContentType = "image/pjpeg" Then
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & MyAlert1 & "');", True)
                    Return False
                End If

                FindSize += filelogo.PostedFile.ContentLength

                If FindSize > MyLimit Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & MyAlert & "');", True)
                    Return False
                End If
            End If


            Return True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
            Return False
        End Try
    End Function

    Private Function checkForExisting() As Boolean
        'GetDBFieldValueExist
        If Session("CustState") = "Edit" Then
            If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "reservation_headernew", "agentcode", txtCustomerCode.Value) = True Then
                checkForExisting = False
                Exit Function
            ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "groupquote_header", "agentcode", txtCustomerCode.Value) = True Then
                checkForExisting = False
                Exit Function
            End If
        End If

        'ExecuteQueryReturnStringValue

        checkForExisting = True
    End Function


    Private Function checkForMappingIDExisting() As Boolean
        'GetDBFieldValueExist

        If TxtExtappid.Value = "" Then
            checkForMappingIDExisting = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter" & LblExtappid.Text & " .');", True)
            Exit Function
        Else
            Dim dt As DataTable
            dt = objUtils.GetDataFromDataTable("select * from agentmast a, int_agentmast i where a.agentcode=i.agentcode and i.int_agentCode+'/'+a.currcode='" & TxtExtappid.Value.Trim & "/" & TxtCurrCode.Text.Trim & "' and a.agentcode<>'" & txtCustomerCode.Value.Trim & "'")

            'changed by mohamed on 30/11/2021
            'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "int_agentmast", "int_agentCode", TxtExtappid.Value) = True Then
            If dt IsNot Nothing Then
                If dt.Rows.Count >= 1 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & LblExtappid.Text & " Already Present .');", True)
                    checkForMappingIDExisting = False
                    Exit Function
                End If
            End If
        End If
        'ExecuteQueryReturnStringValue

        checkForMappingIDExisting = True
    End Function


    Private Function checkForAccountExisting() As Boolean
        'GetDBFieldValueExist
        Dim strValue As String = ""
        If Session("CustState") = "Edit" Then
            strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 't' from acc_tran where acc_type='C' and acc_code='" & Trim(txtCustomerCode.Value) & "'")
            If strValue <> "" Then
                checkForAccountExisting = False
                Exit Function
            End If

            strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 't' from view_unpost_acccode where acc_type='C' and acc_code='" & Trim(txtCustomerCode.Value) & "'")
            If strValue <> "" Then
                checkForAccountExisting = False
                Exit Function
            End If

        End If

        checkForAccountExisting = True
    End Function

    Private Function checkForSellingTypes() As Boolean
        Dim sellcurrcode As String = ""
        Dim othsellcurrcode As String = ""
        Dim tktsellcurrcode As String = ""

        Dim othersellingcode As String = ""
        Dim visasellingcode As String = ""

        If txtCustomerName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field cannot be blank ');", True)
            checkForSellingTypes = False
            Exit Function
        End If

        If txtshortname.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Short Name field cannot be blank');", True)
            checkForSellingTypes = False
            Exit Function
        End If

        If TxtAgentCatCode.Text = "" Or TxtAgentCatName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Category    ');", True)
            checkForSellingTypes = False
            Exit Function
        End If
        If TxtDivisionCode.Text = "" Or TxtDivisionName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Division ');", True)
            checkForSellingTypes = False
            Exit Function
        End If

        If ddlbookengratetype.Text = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Booking Engine Rate Type');", True)
            checkForSellingTypes = False
            Exit Function
        End If

        If TxtCurrCode.Text = "" Or TxtCurrName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Currency Should be Selected');", True)
            checkForSellingTypes = False
            Exit Function
        End If

        If TxtCtryCode.Text = "" Or TxtCtryName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country Should be Selected');", True)
            checkForSellingTypes = False
            Exit Function
        End If

        If TxtCityCode.Text = "" Or TxtCityName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('City Should be Selected');", True)
            checkForSellingTypes = False
            Exit Function
        End If

        If TxtSalesPersonResCode.Text = "" Or TxtSalesPersonResName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Reservation Person Should be Selected');", True)
            checkForSellingTypes = False
            Exit Function
        End If



        If TxtSalesPersonConCode.Text = "" Or TxtSalesPersonConName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sales Person Should be Selected');", True)
            checkForSellingTypes = False
            Exit Function
        End If






        If TxtControlAccCode.Text = "" Or TxtControlAccName.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Main Account Code should be selected');", True)
            checkForSellingTypes = False
            Exit Function
        End If


        'If ddlMarket.Value = "[Select]" Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select any market');", True)
        '    checkForSellingTypes = False
        '    Exit Function
        'End If
        checkForSellingTypes = True
    End Function

#Region "Public Function CategoryPrivilege() As Boolean"
    Public Function CategoryPrivilege()
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            strSql = "select appid from group_privilege_Detail where privilegeid='10' and appid='1' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Allowcat", "Yes")
            Else
                Session.Add("Allowcat", "No")
            End If
            CategoryPrivilege = ""
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("custmaindet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
            CategoryPrivilege = ""
        End Try
    End Function
#End Region

    Protected Sub btnExistClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExistClient.Click
        lblAgentName.Visible = True
        ddlagentname.Visible = True

        ' ddlagentname.Attributes.Add("onchange", "FillAgentDetails('" & txtCustomerName.ClientID & "','" & txtCustomerCode.ClientID & "','" & ddlagentname.ClientID & "','" & txtshortname.ClientID & "','" & ddlCategoryName.ClientID & "','" & ddlCategory.ClientID & "','" & ddlCurrencyName.ClientID & "','" & ddlCurrency.ClientID & "','" & ddlSellingName.ClientID & "','" & ddlSelling.ClientID & "','" & ddlCountryName.ClientID & "','" & ddlCountry.ClientID & "','" & ddlCityName.ClientID & "','" & ddlCity.ClientID & "','" & ddlSalesPersonName.ClientID & "','" & ddlSalesPerson.ClientID & "','" & ddlMarketName.ClientID & "','" & ddlMarket.ClientID & "','" & ddlSectorName.ClientID & "','" & ddlSector.ClientID & "','" & txtCommission.ClientID & "','" & txtVoucheraddress.ClientID & "','" & ddlMainAccName.ClientID & "');")
        '  DisableControlExistClient()
        '  ddlCategory.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentcatmast", "agentcatname", "agentcatcode", ddlCategoryName.Value)
    End Sub


#Region "Protected Sub btnSave_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave_Main.Click"
    Protected Sub btnSave_Main_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave_Main.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim lastno As String
        Dim logo As Byte()

        Dim strPassQry As String = "false"
        Dim loginregion As String
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Try
            If Page.IsValid = True Then
                If Session("CustState") = "New" Or Session("CustState") = "Edit" Or Session("CustState") = "Addclient" Or Session("CustState") = "Copy" Then

                    If CType(Session("ExistClient"), String) = 0 Then


                        If checkForDuplicate() = False Then ' To be Checked
                            Exit Sub
                        End If
                        If checkForSellingTypes() = False Then
                            Exit Sub
                        End If


                        If ValidatePhotoSize() = False Then
                            Exit Sub
                        End If
                    End If
                    'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    'sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start


                    If ExtAppIDflag.ToUpper = "Y" Then 'And ExtAppIDexist = False Then 'changed by mohamed on 05/12/2021
                        If checkForMappingIDExisting() = False Then
                            Exit Sub
                        End If
                    End If
                    If CType(Session("ExistClient"), String) = 0 Then
                        If Session("CustState") = "New" Or Session("CustState") = "Addclient" Or Session("CustState") = "Copy" Then




                            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                            sqlTrans = mySqlConn.BeginTransaction

                            Dim optionval As String
                            lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='CUSTCODE'")
                            optionval = objUtils.GetAutoDocNo("CUSTCODE", mySqlConn, sqlTrans)
                            txtCustomerCode.Value = optionval.Trim

                            mySqlCmd = New SqlCommand("sp_add_agentmast", mySqlConn, sqlTrans)
                            frmmode = 1



                        ElseIf Session("CustState") = "Edit" Then

                            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                            sqlTrans = mySqlConn.BeginTransaction

                            mySqlCmd = New SqlCommand("sp_mod_agentmast", mySqlConn, sqlTrans)
                            frmmode = 2

                        End If
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 200)).Value = CType(txtCustomerName.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@shortname", SqlDbType.VarChar, 50)).Value = CType(txtshortname.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType(TxtAgentCatCode.Text.Trim, String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(TxtCurrCode.Text.Trim, String)


                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(TxtCtryCode.Text.Trim, String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(TxtCityCode.Text.Trim, String)

                        If TxtSectorCode.Text.Trim <> "" Or TxtSectorName.Text.Trim <> "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(TxtSectorCode.Text.Trim, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 20)).Value = CType(TxtSalesPersonResCode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@salespersoncode", SqlDbType.VarChar, 10)).Value = CType(TxtSalesPersonConCode.Text.Trim, String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = CType(TxtDivisionCode.Text.Trim, String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@bookingengineratetype", SqlDbType.VarChar, 20)).Value = CType(ddlbookengratetype.SelectedValue, String)

                        loginregion = ""
                        Dim i As Integer = 0
                        For i = 0 To 2
                            loginregion = IIf(ChkBoxloginbox.Items(i).Selected, loginregion & ChkBoxloginbox.Items(i).Value & ":", loginregion)

                        Next
                        If loginregion.Length > 0 And loginregion <> "" Then
                            loginregion = loginregion.Substring(0, loginregion.Length - 1) 'to avoid ending ":"
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@loginregions", SqlDbType.VarChar, 200)).Value = loginregion

                        If chkActive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                        ElseIf chkActive.Checked = False Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(TxtControlAccCode.Text.Trim, String)

                        If ddlwebCurrencyName.Value = "[Select]" Then

                            mySqlCmd.Parameters.Add(New SqlParameter("@webcurrcode", SqlDbType.VarChar, 20)).Value = CType(ctryhidd1.Value, String)

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@webcurrcode", SqlDbType.VarChar, 20)).Value = CType(ddlwebCurrencyName.Value, String)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@voucheradd", SqlDbType.Text)).Value = CType(txtVoucheraddress.Text, String)
                        If (filelogo.HasFile = False And imgvoucher.ImageUrl = "") Or chkVoucher.Checked Then

                            mySqlCmd.Parameters.Add(New SqlParameter("@voucherlogo", SqlDbType.Image)).Value = DBNull.Value
                            logo = Nothing
                        Else
                            If filelogo.HasFile Then
                                strImgName = filelogo.FileName

                                mySqlCmd.Parameters.Add(New SqlParameter("@voucherlogo", SqlDbType.Image)).Value = IIf(strImgName = "", DBNull.Value, filelogo.FileBytes)
                                logo = filelogo.FileBytes
                            Else
                                strImgName = imgvoucher.ImageUrl
                                Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                                strImgName = strImgName.Substring(1)
                                strImgName = t & strImgName

                                mySqlCmd.Parameters.Add(New SqlParameter("@voucherlogo", SqlDbType.Image)).Value = IIf(strImgName = "", DBNull.Value, File.ReadAllBytes(strImgName))
                                logo = File.ReadAllBytes(strImgName)
                            End If


                        End If




                        mySqlCmd.Parameters.Add(New SqlParameter("@handlingyesno", SqlDbType.Int)).Value = 0



                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@TRNNo", SqlDbType.VarChar, 100)).Value = txtTRNNo.Text.Trim 'changed by mohamed on 23/04/2018

                        mySqlCmd.Parameters.Add(New SqlParameter("@MarketCode", SqlDbType.VarChar, 100)).Value = Trim(TxtMarketcode.Text) 'Tanvir 14072022 ddlMarket.Value.Trim 'added by abin on 20200104
                        mySqlCmd.Parameters.Add(New SqlParameter("@AllowBooking", SqlDbType.Int)).Value = IIf(chkAllowBooking.Checked, 1, 0) 'Added by abin on 20200726


                        mySqlCmd.ExecuteNonQuery()

                        If ExtAppIDflag.ToUpper = "Y" And ExtAppIDexist = False Then
                            mySqlCmd = New SqlCommand("sp_add_int_agentmast", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@int_agentCode", SqlDbType.VarChar, 20)).Value = CType(TxtExtappid.Value.Trim, String)

                            mySqlCmd.ExecuteNonQuery()
                        End If


                        If Session("CustState") = "Addclient" Then
                            If CType(Session("ExistClient"), String) = 0 Then
                                frmmode = 4
                                mySqlCmd = New SqlCommand("sp_update_registration", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@regno", SqlDbType.VarChar, 20)).Value = CType(Session("regno"), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                mySqlCmd.ExecuteNonQuery()
                            End If
                        End If
                    End If

                ElseIf Session("CustState") = "Delete" Then

                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    frmmode = 3
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction


                    If ExtAppIDflag.ToUpper = "Y" Then

                        'deleting mapped Table

                        mySqlCmd = New SqlCommand("sp_del_int_agentmast", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                    End If




                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()





                End If

                If Session("ExistClient") <> 0 And Session("CustState") = "Addclient" Then
                    frmmode = 5


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    mySqlCmd = New SqlCommand("sp_edit_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@regno", SqlDbType.VarChar, 20)).Value = CType(Session("regno"), String)
                    mySqlCmd.ExecuteNonQuery()

                    Dim mySqlCmdAmend As SqlCommand
                    mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmdAmend.CommandType = CommandType.Text
                    mySqlCmdAmend.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("sp_update_registration1", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@regno", SqlDbType.VarChar, 20)).Value = CType(Session("regno"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    'Else
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('helloo')", True)

                End If

                strPassQry = ""





                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                '  Session.Add("SessionFirstCheck", "Edit")


                TxtExtappid.Disabled = True

                Session.Add("CustRefCode", txtCustomerCode.Value)

                If Session("CustState") = "New" Or Session("CustState") = "Addclient" Or Session("CustState") = "Copy" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)


                    Session.Add("CustState", "Edit")



                    txtCode.Value = txtCustomerCode.Value 'added by sribish
                    'If Session("ExistClient") = "0" Then
                    If filelogo.HasFile Then
                        Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                        File.WriteAllBytes(t & "\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp", filelogo.FileBytes)

                        imgvoucher.ImageUrl = "~\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp"
                        imgvoucher.Visible = True
                        chkVoucher.Visible = True
                        chkVoucher.Checked = False




                    End If






                ElseIf Session("CustState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    Session.Add("CustState", "Edit")
                    txtCode.Value = txtCustomerCode.Value

                    If filelogo.HasFile Then
                        Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                        File.WriteAllBytes(t & "\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp", filelogo.FileBytes)

                        imgvoucher.ImageUrl = "~\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp"
                        imgvoucher.Visible = True



                    End If


                End If

                If Session("CustState") = "Delete" Then


                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If




            End If

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()



            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        If CType(Session("CustState"), String) = "View" Or CType(Session("CustState"), String) = "Delete" Then

            txtCustomerCode.Disabled = True
            txtCustomerName.Enabled = False
            TxtAgentCatName.Enabled = False
            txtshortname.Enabled = False
            TxtCurrName.Enabled = False
            TxtCtryName.Enabled = False
            TxtSectorName.Enabled = False
            TxtDivisionName.Enabled = False
            TxtCityName.Enabled = False
            TxtSalesPersonConName.Enabled = False
            TxtSalesPersonResName.Enabled = False
            TxtControlAccName.Enabled = False

            chkActive.Disabled = True
            ddlbookengratetype.Enabled = False
            btnaddnewctry.Enabled = False
            btnaddnewcity.Enabled = False
            btnaddnewcurr.Enabled = False
            btnaddnewagentcat.Enabled = False
            btnaddnewsector.Enabled = False
            ddlwebcurrency.Disabled = True
            ddlwebCurrencyName.Disabled = True
            chkActive.Disabled = True

            ChkBoxloginbox.Enabled = False
            txtVoucheraddress.Enabled = False
            txtTRNNo.Enabled = False 'changed by mohamed on 23/04/2018
        ElseIf CType(Session("CustState"), String) = "Edit" Then
            If checkForExisting() = False Then

            ElseIf checkForAccountExisting() = False Then

            End If


        ElseIf CType(Session("CustState"), String) = "Addclient" Then
            If CType(Session("ExistClient"), String) <> 0 Then
                txtCustomerCode.Disabled = True
                txtCustomerName.Enabled = False
                txtshortname.Enabled = False

                ddlwebcurrency.Disabled = True
                ddlwebCurrencyName.Disabled = True


                chkActive.Disabled = True

                txtVoucheraddress.Enabled = False
                chkVoucher.Enabled = False
                chkActive.Disabled = True

                ChkBoxloginbox.Enabled = False
                filelogo.Enabled = False
                txtwebcity.Disabled = True
                txtwebctry.Disabled = True

                txtTRNNo.Enabled = False 'changed by mohamed on 23/04/2018
            End If
        End If


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
                        Me.txtCode.Value = Me.txtCustomerCode.Value

                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtCustomerName.Text = mySqlReader("agentname")

                    End If


                    If IsDBNull(mySqlReader("voucheradd")) = False Then
                        Me.txtVoucheraddress.Text = mySqlReader("voucheradd")
                    Else
                        Me.txtVoucheraddress.Text = ""
                    End If


                    Dim loginregion As String()
                    ChkBoxloginbox.Items(0).Selected = False
                    ChkBoxloginbox.Items(1).Selected = False
                    ChkBoxloginbox.Items(2).Selected = False
                    If IsDBNull(mySqlReader("loginregions")) = False Then
                        loginregion = mySqlReader("loginregions").ToString.Split(":")
                        Dim i As Integer
                        For i = 0 To loginregion.Length - 1

                            Select Case UCase(loginregion(i))
                                Case "UAE"

                                    ChkBoxloginbox.Items(0).Selected = True
                                Case "OMN"
                                    ChkBoxloginbox.Items(1).Selected = True
                                Case "ROTW"
                                    ChkBoxloginbox.Items(2).Selected = True

                            End Select
                        Next
                    End If



                    If IsDBNull(mySqlReader("shortname")) = False Then
                        Me.txtshortname.Text = mySqlReader("shortname")
                    End If

                    If IsDBNull(mySqlReader("catcode")) = False Then
                        Me.TxtAgentCatName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentcatmast", "agentcatname", "agentcatcode", CType(mySqlReader("catcode"), String))
                        Me.TxtAgentCatCode.Text = CType(mySqlReader("catcode"), String)
                    Else
                        Me.TxtAgentCatName.Text = ""
                        Me.TxtAgentCatCode.Text = ""
                    End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.TxtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", CType(mySqlReader("currcode"), String))
                        Me.TxtCurrCode.Text = CType(mySqlReader("currcode"), String)
                    Else
                        Me.TxtCurrName.Text = ""
                        Me.TxtCurrCode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        Me.TxtCtryName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        Me.TxtCtryCode.Text = CType(mySqlReader("ctrycode"), String)
                    Else
                        Me.TxtCtryName.Text = ""
                        Me.TxtCtryCode.Text = ""
                    End If


                    strSqlQry = ""
                    strSqlQry = "select currmast.currname,currmast.currcode from currmast  where currmast.active=1  order by currmast.currname"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlwebCurrencyName, "currname", "currcode", strSqlQry, True)
                    strSqlQry = ""
                    strSqlQry = "select currmast.currcode,currmast.currname from currmast   where currmast.active=1  order by  currmast.currcode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlwebcurrency, "currcode", "currname", strSqlQry, True)

                    If IsDBNull(mySqlReader("webcurrcode")) = False Then
                        ctryhidd1.Value = mySqlReader("webcurrcode")
                        ddlwebCurrencyName.Value = mySqlReader("webcurrcode")
                        ddlwebcurrency.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", CType(mySqlReader("webcurrcode"), String))
                    End If


                    If IsDBNull(mySqlReader("citycode")) = False Then
                        Me.TxtCityName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", CType(mySqlReader("citycode"), String))
                        Me.TxtCityCode.Text = CType(mySqlReader("citycode"), String)
                    Else
                        Me.TxtCityName.Text = ""
                        Me.TxtCityCode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        Me.TxtSectorName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agent_sectormaster", "sectorname", "sectorcode", CType(mySqlReader("sectorcode"), String))
                        Me.TxtSectorCode.Text = CType(mySqlReader("sectorcode"), String)
                    Else
                        Me.TxtSectorName.Text = ""
                        Me.TxtSectorCode.Text = ""
                    End If


                    If IsDBNull(mySqlReader("spersoncode")) = False Then
                        Me.TxtSalesPersonResName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "UserMaster", "UserName", "UserCode", CType(mySqlReader("spersoncode"), String))
                        Me.TxtSalesPersonResCode.Text = CType(mySqlReader("spersoncode"), String)
                    Else
                        Me.TxtSalesPersonResName.Text = ""
                        Me.TxtSalesPersonResCode.Text = ""

                    End If

                    If IsDBNull(mySqlReader("salescontact")) = False Then
                        Me.TxtSalesPersonConName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "UserMaster", "UserName", "UserCode", CType(mySqlReader("salescontact"), String))
                        Me.TxtSalesPersonConCode.Text = CType(mySqlReader("salescontact"), String)
                    Else
                        Me.TxtSalesPersonConName.Text = ""
                        Me.TxtSalesPersonConCode.Text = ""



                    End If

                    Dim default_group As String
                    default_group = ""
                    default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1108", String))

                    strSqlQry = ""

                    strSqlQry = ""

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If

                    'changed by mohamed on 23/04/2018
                    If IsDBNull(mySqlReader("TRNNO")) = False Then
                        txtTRNNo.Text = mySqlReader("TRNNO")
                    Else
                        txtTRNNo.Text = ""
                    End If


                    If IsDBNull(mySqlReader("bookingengineratetype")) = False Then
                        If (CType(mySqlReader("bookingengineratetype"), String).ToLower) = "individual" Then
                            ddlbookengratetype.SelectedIndex = 1
                        ElseIf (CType(mySqlReader("bookingengineratetype"), String).ToLower) = "cumulative" Then
                            ddlbookengratetype.SelectedIndex = 2
                        End If

                    End If

                    If IsDBNull(mySqlReader("marketcode")) = False Then
                        'Tanvir 14/07/2022 ddlMarket.Value = mySqlReader("marketcode")
                        TxtMarketcode.Text = mySqlReader("marketcode")
                        '  Dim strControlAccName As String
                        TxtMarketName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select marketname from marketmast(nolock) where marketcode='" + mySqlReader("marketcode") + "' ") 'and div_code='" + mySqlReader("divcode") + "' Tanvir 14072022

                    End If

                    If IsDBNull(mySqlReader("divcode")) = False Then
                        TxtDivisionCode.Text = mySqlReader("divcode")
                        Session("sCDivCode") = TxtDivisionCode.Text
                        TxtDivisionName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "division_master", "division_master_des", "division_master_code", mySqlReader("divcode"))
                    Else
                        TxtDivisionCode.Text = ""
                        TxtDivisionName.Text = ""
                    End If
                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        TxtControlAccCode.Text = mySqlReader("controlacctcode")
                        'modofied by abin on 20181219
                        Dim strControlAccName As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast(nolock) where acctcode='" + mySqlReader("controlacctcode") + "' and div_code='" + mySqlReader("divcode") + "'")
                        TxtControlAccName.Text = strControlAccName 'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                    Else
                        TxtControlAccCode.Text = ""
                        TxtControlAccName.Text = ""
                    End If

                    If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agents_locked", "agentcode", CType(mySqlReader("agentcode"), String)) = True Then
                        lbllockstatus.Text = "Locked"
                    Else
                        lbllockstatus.Text = "UnLocked"
                    End If
                    'Added by abin on 20200726
                    If IsDBNull(mySqlReader("AllowBooking")) = False Then
                        If CType(mySqlReader("AllowBooking"), String) = "1" Then
                            chkAllowBooking.Checked = True
                        ElseIf CType(mySqlReader("AllowBooking"), String) = "0" Then
                            chkAllowBooking.Checked = False
                        End If
                    Else
                        chkAllowBooking.Checked = False
                    End If


                    If IsDBNull(mySqlReader("voucherlogo")) = False Then
                        Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                        If File.Exists((t & "\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp")) Then
                            File.Delete((t & "\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp"))
                        End If
                        File.WriteAllBytes(t & "\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp", mySqlReader("voucherlogo"))

                        imgvoucher.ImageUrl = "~\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp"
                        imgvoucher.Visible = True
                        chkVoucher.Visible = True

                        'File.Delete("~\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp")
                        '  File.Delete("~\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp")
                    Else
                        imgvoucher.ImageUrl = ""
                        imgvoucher.Visible = False
                        chkVoucher.Visible = False
                    End If

                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Private Sub ShowRecord_registration(ByVal regno As String)"
    Private Sub ShowRecord_registration(ByVal regno As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from registration Where regno='" & regno & "' ", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtCustomerName.Text = mySqlReader("agentname")
                    End If

                    If IsDBNull(mySqlReader("agentshortname")) = False Then
                        Me.txtshortname.Text = mySqlReader("agentshortname")
                    End If


                   

                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        txtwebctry.Visible = True
                        txtwebctry.Value = mySqlReader("ctrycode")
                        webctry.Visible = True

                      
                    End If
              
                    If IsDBNull(mySqlReader("citycode")) = False Then
                   
                        txtwebcity.Visible = True
                        txtwebcity.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "webcities", "city", "id", CType(mySqlReader("citycode"), String))
                        webcity.Visible = True
                    End If
        
                    If IsDBNull(mySqlReader("add3")) = False Then
                        txtVoucheraddress.Text = mySqlReader("add3")
                    End If

                End If



                If IsDBNull(mySqlReader("voucherlogo")) = False Then
                    Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                    If File.Exists((t & "\Pricelistmodule\UploadedLogo\ " & regno.Replace("/", "_") & "voucherlogo.bmp")) Then
                        File.Delete((t & "\Pricelistmodule\UploadedLogo\ " & regno.Replace("/", "_") & "voucherlogo.bmp"))
                    End If
                    File.WriteAllBytes(t & "\Pricelistmodule\UploadedLogo\ " & regno.Replace("/", "_") & "voucherlogo.bmp", mySqlReader("voucherlogo"))

                    imgvoucher.ImageUrl = "~\Pricelistmodule\UploadedLogo\ " & regno.Replace("/", "_") & "voucherlogo.bmp"
                    imgvoucher.Visible = True
                    chkVoucher.Visible = True

                    'File.Delete("~\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp")
                    '  File.Delete("~\Pricelistmodule\UploadedLogo\ " & Me.txtCustomerCode.Value & "voucherlogo.bmp")
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromagent_detail", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a DetailsOfEarlyBirdPromotions, cannot delete this Customer');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If
        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_agent", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a CustomerPromotions, cannot delete this Customer');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If
        '' Added shahul 06/12/18
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "booking_header", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already made the booking, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If
        '' Added shahul 06/12/18
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "quote_booking_header", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already made the Quotation, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If
     

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "receipt_detail", "receipt_acc_type='C' and  receipt_acc_code", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a accounts transaction, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "journal_detail", "journal_acc_type='C' and journal_acc_code", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a accounts transaction, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "openparty_master", "open_type='C' and open_code", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a accounts openingbalance transaction, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If


        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustMainDet','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub chkVoucher_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVoucher.CheckedChanged
        If chkVoucher.Checked Then

            imgvoucher.ImageUrl = ""
            imgvoucher.Visible = False

        End If
    End Sub


 

    Protected Sub ddlagentname_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlagentname.SelectedIndexChanged
        Dim agentcode As String = ""
        agentcode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select agentcode from agentmast where agentname like '" & CType(ddlagentname.Items(ddlagentname.SelectedIndex).Text, String) & "%'")

        ShowRecord(agentcode)
        If ddlagentname.SelectedValue <> "[Select]" Then
            existclientflag = existclientflag + 1
        End If
        Session("ExistClient") = existclientflag
        DisableControl()

    End Sub

    Protected Sub btnaddnewagentcat_Click(sender As Object, e As System.EventArgs) Handles btnaddnewagentcat.Click
        Dim strpop As String = ""
        strpop = "window.open('CustomerCategories.aspx?State=New&Value=Addfrom','CustCat');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnaddnewcurr_Click(sender As Object, e As System.EventArgs) Handles btnaddnewcurr.Click
        Dim strpop As String = ""
        strpop = "window.open('Currencies.aspx?State=New&Value=AddCurrfrom','Currency');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnaddnewcity_Click(sender As Object, e As System.EventArgs) Handles btnaddnewcity.Click
        Dim strpop As String = ""
        strpop = "window.open('Cities.aspx?State=New&Value=AddCityfrom','City');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnaddnewctry_Click(sender As Object, e As System.EventArgs) Handles btnaddnewctry.Click
        Dim strpop As String = ""
        strpop = "window.open('Countries.aspx?State=New&Value=Addfrom','Country');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnaddnewsector_Click(sender As Object, e As System.EventArgs) Handles btnaddnewsector.Click
        Dim strpop As String = ""
        strpop = "window.open('CustomerSector.aspx?State=New&Value=Addsectorfrom','Sector');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Private Sub BindMarket()
        strSqlQry = ""
        strSqlQry = "select marketid,marketname from tblbusinessunits order by marketid"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "marketname", "marketid", strSqlQry, True)
    End Sub


    Protected Sub btnaddmarket_Click(sender As Object, e As System.EventArgs) Handles btnaddmarket.Click
        Dim strpop As String = ""
        strpop = "window.open('../AccountsModule/FrmMarkets.aspx?State=New&Value=Addfrom','Market');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
