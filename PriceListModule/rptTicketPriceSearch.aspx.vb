Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptTicketPriceSearch
    Inherits System.Web.UI.Page
    Dim objectcl As New clsDateTime
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim servicetype As String
    Dim default_country As String
    Dim strqry As String
    Dim seasonfrom As String
    Dim seasonto As String
    Dim strReportTitle As String = ""

    Dim strsptypecode As String = ""
    Dim strsupagentcode As String = ""
    Dim strpartycode As String = ""
    Dim strfromdate As String = ""
    Dim strtodate As String = ""
    Dim strplgrpcode As String = ""
    Dim strsellcode As String = ""

    Dim strcitycode As String = ""
    Dim strctrycode As String = ""
    Dim strcatcode As String = ""

    Dim strsubseasoncode As String = ""
    Dim strflightcode As String = ""
    Dim strflightclasscode As String = ""
    Dim strapprove As Integer

    Dim strrepfilter As String = ""
    Dim strreportoption As String = ""
    Dim strasondate As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Page.IsPostBack = False Then
            Try
                If txtFromDate.Text = "" Then
                    txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If

                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If
                If txtUpdateAsOn.Text = "" Then
                    txtUpdateAsOn.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If

                txtconnection.Value = Session("dbconnectionName")

                objutils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1code, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 order by seascode", True)
                objutils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1name, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 order by seasname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)

                servicetype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("500", String))
                ddlSpTypeName.Value = CType(servicetype, String)
                ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

                default_country = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
                ddlcontName.Value = CType(default_country, String)
                ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text


                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)

                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)

                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "tktsellcode", "tktsellname", "select tktsellcode,tktsellname from tktsellmast where active=1 order by tktsellcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "tktsellname", "tktsellcode", "select tktsellname, tktsellcode from tktsellmast where active=1 order by tktsellname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSeasoncode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSeasonName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)


                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightCode, "flightcode", "flightcode", "select flightcode,flightcode as flightcode1 from flightmast where active=1 and showinplist=1 order by flightcode", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightClassCode, "flightclscode", "flightclsname", "select flightclscode,flightclsname from flightclsmast where active=1 order by flightclscode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFlightClassname, "flightclsname", "flightclscode", "select flightclsname,flightclscode from flightclsmast where active=1 order by flightclsname", True)


                ' City Filter '''''''''''
                strqry = "select citycode,cityname from citymast where active=1 "
                If default_country <> "" Then
                    strqry = strqry + " and ctrycode='" & default_country & "'"
                End If
                strqry = strqry + " order by citycode"
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", strqry, True)

                strqry = "select cityname,citycode from citymast where active=1 "
                If default_country <> "" Then
                    strqry = strqry + " and ctrycode='" & default_country & "'"
                End If
                strqry = strqry + " order by cityname"
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", strqry, True)
                '''''''''''''''


                If ddlSPTypeCode.Value <> "[Select]" Then
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by catcode", True, ddlCCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by catname", True, ddlCatName.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by partycode", True, ddlPartyCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by partyname", True, ddlPartyName.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by supagentcode", True, ddlSupplierAgentCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by supagentname", True, ddlSupplierAgentName.Value)
                Else
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 and order by catcode", True, ddlCCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True, ddlCatName.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True, ddlPartyCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlPartyCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True, ddlSupplierAgentCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True, ddlSupplierAgentName.Value)
                End If
                If ddlContCode.Value <> "[Select]" Then
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "' order by citycode", True, ddlCityCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "' order by cityname", True, ddlCityName.Value)
                Else
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True, ddlCityCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True, ddlCityName.Value)
                End If

                If Request.QueryString("sptypecode") <> "" Then
                    ddlSpTypeName.Value = Request.QueryString("sptypecode")
                    ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
                End If
                If Request.QueryString("partycode") <> "" Then
                    ddlPartyName.Value = Request.QueryString("partycode")
                    ddlPartyCode.Value = ddlPartyName.Items(ddlPartyName.SelectedIndex).Text
                End If
                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                If Request.QueryString("asondate") <> "" Then
                    txtUpdateAsOn.Text = Format("U", CType(Request.QueryString("asondate"), Date))
                End If

                If Request.QueryString("citycode") <> "" Then
                    ddlCityName.Value = Request.QueryString("citycode")
                    ddlCityCode.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text
                End If
                If Request.QueryString("ctrycode") <> "" Then
                    ddlcontName.Value = Request.QueryString("ctrycode")
                    ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text
                End If

                If Request.QueryString("catcode") <> "" Then
                    ddlCatName.Value = Request.QueryString("catcode")
                    ddlCCode.Value = ddlCatName.Items(ddlCatName.SelectedIndex).Text
                End If
                If Request.QueryString("sellcode") <> "" Then
                    ddlSellingName.Value = Request.QueryString("sellcode")
                    ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text
                End If

                If Request.QueryString("plgrpcode") <> "" Then
                    ddlMarketName.Value = Request.QueryString("plgrpcode")
                    ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
                End If

                If Request.QueryString("supagentcode") <> "" Then
                    ddlSupplierAgentName.Value = Request.QueryString("supagentcode")
                    ddlSupplierAgentCode.Value = ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text
                End If
                If Request.QueryString("subseasoncode") <> "" Then
                    ddlSeasonName.Value = Request.QueryString("subseasoncode")
                    ddlSeasoncode.Value = ddlSeasonName.Items(ddlSeasonName.SelectedIndex).Text
                    'ddlseas1code
                    ddlseas1name.SelectedValue = Request.QueryString("subseasoncode")
                    ddlseas1code.SelectedValue = ddlSeasonName.Items(ddlSeasonName.SelectedIndex).Text
                End If
                If Request.QueryString("flightcode") <> "" Then
                    ddlFlightCode.Value = Request.QueryString("flightcode")
                    ddlFlightCode.Value = ddlFlightCode.Items(ddlFlightCode.SelectedIndex).Text
                End If
                If Request.QueryString("flightclasscode") <> "" Then
                    ddlFlightClassname.Value = Request.QueryString("flightclasscode")
                    ddlFlightClassCode.Value = ddlFlightClassname.Items(ddlFlightClassname.SelectedIndex).Text
                End If

                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCatName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSpTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellingCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlPartyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPartyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSeasoncode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSeasonName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlseas1code.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlseas1name.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSupplierAgentCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSupplierAgentName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlFlightCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlFlightClassCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFlightClassname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepTktPLWindowPostBack") Then
                    BtnPrint_Click(sender, e)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptTicketPriceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = True Then
            'servicetype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", CType("500", String))
            'ddlSpTypeName.Value = CType(servicetype, String)
            'ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

            'default_country = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", CType("459", String))
            'ddlcontName.Value = CType(default_country, String)
            'ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text

            If ddlSPTypeCode.Value <> "[Select]" Then
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by catcode", True, ddlCCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by catname", True, ddlCatName.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by partycode", True, ddlPartyCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by partyname", True, ddlPartyName.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by supagentcode", True, ddlSupplierAgentCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "' order by supagentname", True, ddlSupplierAgentName.Value)
            Else
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1  order by catcode", True, ddlCCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True, ddlCatName.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True, ddlPartyCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlPartyCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True, ddlSupplierAgentCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True, ddlSupplierAgentName.Value)
            End If
            If ddlContCode.Value <> "[Select]" Then
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "' order by citycode", True, ddlCityCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1 and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "' order by cityname", True, ddlCityName.Value)
            Else
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True, ddlCityCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True, ddlCityName.Value)
            End If

        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        '    rep.Close()
        '    rep.Dispose()
        'End If
    End Sub

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlPartyCode.Value = "[Select]"
        ddlPartyName.Value = "[Select]"
        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"
        'ddlSPTypeCode.Value = "[Select]"
        'ddlSpTypeName.Value = "[Select]"
        ddlSellingCode.Value = "[Select]"
        ddlSellingName.Value = "[Select]"
        ddlCCode.Value = "[Select]"
        ddlCatName.Value = "[Select]"
        'ddlContCode.Value = "[Select]"
        'ddlcontName.Value = "[Select]"
        ddlCityCode.Value = "[Select]"
        ddlCityName.Value = "[Select]"
        ddlSeasoncode.Value = "[Select]"
        ddlSeasonName.Value = "[Select]"
        ddlseas1code.SelectedValue = "[Select]"
        ddlseas1name.SelectedValue = "[Select]"
        ddlSupplierAgentCode.Value = "[Select]"
        ddlSupplierAgentName.Value = "[Select]"
        ddlFlightCode.Value = "[Select]"
        ddlFlightClassCode.Value = "[Select]"
        ddlFlightClassname.Value = "[Select]"

        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
        txtUpdateAsOn.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))

    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            If ddlSPTypeCode.Value = "" Or UCase(Trim(ddlSPTypeCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Type can not be blank..');", True)

                SetFocus(ddlSPTypeCode)
                ValidatePage = False
                Exit Function
            End If

            If ddlMarketCode.Value = "" Or UCase(Trim(ddlMarketCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Market can not be blank.');", True)

                SetFocus(ddlMarketCode)
                ValidatePage = False
                Exit Function
            End If

            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)

                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank..');", True)


                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If
            If CType(objectcl.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objectcl.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If txtUpdateAsOn.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on Date field can not be blank..');", True)

                SetFocus(txtUpdateAsOn)
                ValidatePage = False
                Exit Function
            End If

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try
            If ValidatePage() = True Then
                'Session.Add("Pageame", "TicketPricelist")
                'Session.Add("BackPageName", "rptTicketPriceSearch.aspx")

                strsptypecode = IIf(UCase(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, "")
                strsupagentcode = IIf(UCase(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, "")
                strpartycode = IIf(UCase(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, "")

                strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
                'Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Format(CType(txtToDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
                'Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)
                strasondate = txtUpdateAsOn.Text

                strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
                strsellcode = IIf(UCase(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text, "")
                strcitycode = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
                strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
                strcatcode = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "")

                strsubseasoncode = IIf(UCase(ddlSeasoncode.Items(ddlSeasoncode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSeasoncode.Items(ddlSeasoncode.SelectedIndex).Text, "")
                strflightcode = IIf(UCase(ddlFlightCode.Items(ddlFlightCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFlightCode.Items(ddlFlightCode.SelectedIndex).Text, "")
                strflightclasscode = IIf(UCase(ddlFlightClassCode.Items(ddlFlightClassCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFlightClassCode.Items(ddlFlightClassCode.SelectedIndex).Text, "")
                strapprove = CType(ddlapprovestatus.SelectedValue, String)

                strReportTitle = "Ticketing Price List"

                strreportoption = ""

                'Session.Add("sptypecode", strsptypecode)
                'Session.Add("supagentcode", strsupagentcode)
                'Session.Add("partycode", strpartycode)
                'Session.Add("fromdate", strfromdate)
                'Session.Add("todate", strtodate)
                'Session.Add("asondate", strasondate)
                'Session.Add("plgrpcode", strplgrpcode)
                'Session.Add("sellcode", strsellcode)

                'Session.Add("citycode", strcitycode)
                'Session.Add("ctrycode", strctrycode)
                'Session.Add("catcode", strcatcode)

                'Session.Add("subseasoncode", strsubseasoncode)
                'Session.Add("flightcode", strflightcode)
                'Session.Add("flightclasscode", strflightclasscode)

                'Session.Add("repfilter", strrepfilter)
                'Session.Add("reportoption", strreportoption)
                'Session.Add("ReportTitle", strReportTitle)

                'Response.Redirect("rptPricelistReport.aspx", False)
                'Response.Redirect("rptTicketPriceReport.aspx?sptypecode=" & strsptypecode & "&partycode=" & strpartycode _
                '& "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&sellcode=" & strsellcode _
                '& "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode & "&supagentcode=" & strsupagentcode _
                '& "&subseasoncode=" & strsubseasoncode & "&flightcode=" & strflightcode & "&flightclasscode=" & strflightclasscode _
                '& "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)

                Dim strpop As String = ""
                strpop = "window.open('rptTicketPriceReport.aspx?Pageame=TicketPricelist&BackPageName=rptTicketPriceSearch.aspx&sptypecode=" & strsptypecode & "&partycode=" & strpartycode _
                & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&sellcode=" & strsellcode _
                & "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode & "&supagentcode=" & strsupagentcode _
                & "&subseasoncode=" & strsubseasoncode & "&approve=" & strapprove & "&flightcode=" & strflightcode & "&flightclasscode=" & strflightclasscode _
                & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepTktPriceList','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptTicketPriceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub ddlseas1name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlseas1name.SelectedValue <> "[Select]" Then
            ddlseas1code.SelectedValue = ddlseas1name.SelectedItem.Text

        End If
        txtFromDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "seasmast", "frmdate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))
        txtToDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "seasmast", "todate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))

    End Sub

    Protected Sub ddlseas1code_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlseas1code.SelectedValue <> "[Select]" Then
            ddlseas1name.SelectedValue = ddlseas1code.SelectedItem.Text
        End If
        txtFromDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "seasmast", "frmdate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))
        txtToDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "seasmast", "todate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))
    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptTicketPriceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
