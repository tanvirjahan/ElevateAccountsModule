Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptOtherServiceCost
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        If Page.IsPostBack = False Then
            Try
                If txtFromDate.Text = "" Then
                    txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                txtconnection.Value = Session("dbconnectionName")

                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If

         

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)


                objutils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1code, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 order by seascode", True)
                objutils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1name, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 order by seasname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)


                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)

                default_country = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))

                ddlcontName.Value = CType(default_country, String)
                ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1 and ctrycode='" & ddlcontName.Value & "' order by citycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1 and ctrycode='" & ddlcontName.Value & "' order by cityname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                'servicetype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", CType("458", String))
                'ddlSpTypeName.Value = CType(servicetype, String)
                'ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

                'default_country = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", CType("459", String))
                'ddlcontName.Value = CType(default_country, String)
                'ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text

                ' City Filter '''''''''''
                'strqry = "select citycode,cityname from citymast where active=1 "
                'If default_country <> "" Then
                '    strqry = strqry + " and ctrycode='" & default_country & "'"
                'End If
                'strqry = strqry + " order by citycode"
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityCode, "citycode", "cityname", strqry, True)

                'strqry = "select cityname,citycode from citymast where active=1 "
                'If default_country <> "" Then
                '    strqry = strqry + " and ctrycode='" & default_country & "'"
                'End If
                'strqry = strqry + " order by cityname"
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityName, "cityname", "citycode", strqry, True)
                '''''''''''''''
                If Request.QueryString("othergroupcode") <> "" Then
                    ddlOtherGroupName.Value = Request.QueryString("othergroupcode")
                    ddlOtherGroupCode.Value = ddlOtherGroupName.Items(ddlOtherGroupName.SelectedIndex).Text
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

                If Request.QueryString("plgrpcode") <> "" Then
                    ddlMarketName.Value = Request.QueryString("plgrpcode")
                    ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
                End If
                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptSupplierpoliciesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If



        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCatName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSpTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlPartyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlPartyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlseas1code.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlseas1name.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepOthPLWindowPostBack") Then
            BtnPrint_Click(sender, e)
        End If
    End Sub

    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        'If dtpFromDate.txtDate.Text = "" Then
        'dtpFromDate.txtDate.Text = objectcl.GetSystemDateOnly
        ' End If

        ' If dtptodate.txtDate.Text = "" Then
        'dtptodate.txtDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly)
        'End If

        'If Page.IsPostBack = False Then
        Try
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlOtherGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlOtherGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)


            'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1code, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 order by seascode", True)
            'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1name, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 order by seasname", True)

            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)

            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
            ''objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)

            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

            '                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingCode, "sellcode", "sellname", "select sellcode, sellname from sellmast where active=1 order by sellcode", True)
            '                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingName, "sellname", "sellcode", "select sellname, sellcode from sellmast where active=1 order by sellname", True)

            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingCode, "othsellcode", "othsellname", "select othsellcode, othsellname from othsellmast where active=1 order by othsellcode", True)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingName, "othsellname", "othsellcode", "select othsellname, othsellcode from othsellmast where active=1 order by othsellname", True)

            'servicetype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", CType("458", String))
            'ddlSpTypeName.Value = CType(servicetype, String)
            'ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

            'default_country = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", CType("459", String))
            'ddlcontName.Value = CType(default_country, String)
            'ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text

            ' City Filter '''''''''''
            'strqry = "select citycode,cityname from citymast where active=1 "
            'If default_country <> "" Then
            'strqry = strqry + " and ctrycode='" & default_country & "'"
            'End If
            ' strqry = strqry + " order by citycode"
            ' objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityCode, "citycode", "cityname", strqry, True)

            'strqry = "select cityname,citycode from citymast where active=1 "
            'If default_country <> "" Then
            'strqry = strqry + " and ctrycode='" & default_country & "'"
            'End If
            'strqry = strqry + " order by cityname"
            ' objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCityName, "cityname", "citycode", strqry, True)
            '''''''''''''''

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptSupplierpoliciesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ' End If



        'Dim typ As Type
        'typ = GetType(DropDownList)

        'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
        'Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

        'ddlCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlCatName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ' ddlSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlSpTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlSellingCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        'ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        'ddlPartyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlPartyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        'ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        'ddlseas1code.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'ddlseas1name.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


        'End If
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
        ddlSPTypeCode.Value = "[Select]"
        ddlSpTypeName.Value = "[Select]"
        ddlCCode.Value = "[Select]"
        ddlCatName.Value = "[Select]"
        ddlContCode.Value = "[Select]"
        ddlcontName.Value = "[Select]"
        ddlCityCode.Value = "[Select]"
        ddlCityName.Value = "[Select]"

        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))

    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime

        Try
            If ddlOtherGroupCode.Value = "" Or UCase(Trim(ddlOtherGroupCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Other Service Group can not be blank.');", True)

                SetFocus(ddlOtherGroupCode)
                ValidatePage = False
                Exit Function
            End If

            If ddlMarketCode.Value = "" Or UCase(Trim(ddlMarketCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Market can not be blank.');", True)

                SetFocus(ddlMarketCode)
                ValidatePage = False
                Exit Function
            End If

            'If ddlSPTypeCode.Value = "" Or UCase(Trim(ddlSPTypeCode.Value)) = UCase(Trim("[Select]")) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Type can not be blank.');", True)

            '    SetFocus(ddlSPTypeCode)
            '    ValidatePage = False
            '    Exit Function
            'End If



            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank.');", True)
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

            'servicetype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"partymast", "sptypecode", "partycode", ddlPartyName.Value)
            'If ddlSpTypeName.Value <> servicetype Then
            '    MsgBox("Please Select the Proper Service Type for the Supplier.", MsgBoxStyle.Information, "Other Service Cost Sheet")
            '    SetFocus(ddlSPTypeCode)
            'End If

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try
            If ValidatePage() = True Then
                'Session.Add("Pageame", "rptOtherServicePricelist")
                'Session.Add("BackPageName", "rptOtherServicePricelist.aspx")


                Dim strReportTitle As String = ""

                Dim strgrpcode As String = ""
                Dim strsptypecode As String = ""
                Dim strppartycode As String = ""
                Dim strfromdate As String = ""
                Dim strtodate As String = ""
                Dim strplgrpcode As String = ""
                Dim strsellcode As String = ""

                Dim strcitycode As String = ""
                Dim strctrycode As String = ""
                Dim strcatcode As String = ""


                Dim strpromtype As String = ""
                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""
                Dim strasondate As String = ""
                Dim strapprove As Integer
                Dim rtptype As String = ""

                strgrpcode = IIf(UCase(ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text, "")

                strsptypecode = IIf(UCase(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, "")
                strppartycode = IIf(UCase(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, "")
                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)

                strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
                strcitycode = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
                strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
                strcatcode = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "")
                strapprove = CType(ddlapprovestatus.SelectedValue, String)

                strreportoption = IIf(UCase(ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text, "") '"COST"
                strReportTitle = "Other Service Price List -  Cost Sheet"
                rtptype = "Cost"
                'Session.Add("grpcode", strgrpcode)
                'Session.Add("fromdate", strfromdate)
                'Session.Add("todate", strtodate)
                'Session.Add("plgrpcode", strplgrpcode)

                'Session.Add("sptypecode", strsptypecode)
                'Session.Add("ppartycode", strppartycode)

                'Session.Add("citycode", strcitycode)
                'Session.Add("ctrycode", strctrycode)
                'Session.Add("catcode", strcatcode)

                'Session.Add("repfilter", strrepfilter)
                'Session.Add("reportoption", strreportoption)

                'Session.Add("ReportTitle", strReportTitle)

                '                Response.Redirect("rptOtherServicePriceListReport.aspx", False)
                'Response.Redirect("rptOtherServicePriceListReport.aspx?othergroupcode=" & strgrpcode & "&fromdate=" & strfromdate _
                '& "&sptypecode=" & strsptypecode & "&todate=" & strtodate & "&plgrpcode=" & strplgrpcode & "&partycode=" & strppartycode _
                '& "&ctrycode=" & strctrycode & "&citycode=" & strcitycode & "&catcode=" & strcatcode _
                '& "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)
                Dim strpop As String = ""
                'strpop = "window.open('rptOtherServicePriceListReport.aspx?Pageame=rptOtherServicePricelist&BackPageName=rptOtherServicePricelist.aspx&othergroupcode=" & strgrpcode & "&fromdate=" & strfromdate _
                '                & "&sptypecode=" & strsptypecode & "&todate=" & strtodate & "&plgrpcode=" & strplgrpcode & "&approve=" & strapprove & "&partycode=" & strppartycode _
                '                & "&ctrycode=" & strctrycode & "&citycode=" & strcitycode & "&catcode=" & strcatcode _
                '                & "&rtptype=" & rtptype & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepOthCostSheet','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                strpop = "window.open('rptOtherServicePriceListReport.aspx?Pageame=rptOtherServicePricelist&BackPageName=rptOtherServicePricelist.aspx&othergroupcode=" & strgrpcode & "&fromdate=" & strfromdate _
                                & "&sptypecode=" & strsptypecode & "&todate=" & strtodate & "&plgrpcode=" & strplgrpcode & "&approve=" & strapprove & "&partycode=" & strppartycode _
                                & "&ctrycode=" & strctrycode & "&citycode=" & strcitycode & "&catcode=" & strcatcode _
                                & "&rtptype=" & rtptype & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepOthCostSheet');"


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptOtherServicePriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub ddlseas1name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlseas1name.SelectedValue <> "[Select]" Then
            ddlseas1code.SelectedItem.Text = ddlseas1name.SelectedItem.Value
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


    Protected Sub rbcostyes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblSupplierTypeCode.Visible = True
        lblsuppliertypename.Visible = True
        lblSupplierCode.Visible = True
        lblSupplierName.Visible = True
        lblCountryCode.Visible = True
        lblcountryname.Visible = True
        lblCityCode.Visible = True
        lblcityname.Visible = True
        lblCategorycode.Visible = True
        lblCategoryName.Visible = True
        lblmsg.Visible = True

        ddlPartyCode.Visible = True
        ddlPartyName.Visible = True
        ddlContCode.Visible = True
        ddlcontName.Visible = True
        ddlCityCode.Visible = True
        ddlCityName.Visible = True
        ddlCCode.Visible = True
        ddlCatName.Visible = True
        ddlSPTypeCode.Visible = True
        ddlSpTypeName.Visible = True
    End Sub

    Protected Sub rbcostno_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblSupplierTypeCode.Visible = False
        lblsuppliertypename.Visible = False
        lblSupplierCode.Visible = False
        lblSupplierName.Visible = False
        lblCountryCode.Visible = False
        lblcountryname.Visible = False
        lblCityCode.Visible = False
        lblcityname.Visible = False
        lblCategorycode.Visible = False
        lblCategoryName.Visible = False
        lblmsg.Visible = False

        ddlPartyCode.Visible = False
        ddlPartyName.Visible = False
        ddlContCode.Visible = False
        ddlcontName.Visible = False
        ddlCityCode.Visible = False
        ddlCityName.Visible = False
        ddlCCode.Visible = False
        ddlCatName.Visible = False
        ddlSPTypeCode.Visible = False
        ddlSpTypeName.Visible = False

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptOtherServiceCost','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
