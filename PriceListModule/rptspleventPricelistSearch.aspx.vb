Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptspleventPricelistSearch
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
    Dim strppartycode As String = ""
    Dim strfromdate As String = ""
    Dim strtodate As String = ""
    Dim strplgrpcode As String = ""
    Dim strsellcode As String = ""

    Dim strcitycode As String = ""
    Dim strctrycode As String = ""
    Dim strcatcode As String = ""

    Dim strroomtype As String = ""
    Dim strmeal As String = ""
    Dim strselltype As String = ""
    Dim strexcept As String = ""

    Dim strpromtype As String = ""
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

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "sellcode", "sellname", "select sellcode, sellname from sellmast where active=1 order by sellcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "sellname", "sellcode", "select sellname, sellcode from sellmast where active=1 order by sellname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSeasoncode, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 order by seascode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSeasonName, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 order by seasname", True)



                servicetype = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))
                ddlSpTypeName.Value = CType(servicetype, String)
                ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

                default_country = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
                ddlcontName.Value = CType(default_country, String)
                ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text

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


                If Request.QueryString("selltype") <> "" Then
                    If Request.QueryString("selltype") = "0" Then rbbeach.Checked = True
                    If Request.QueryString("selltype") = "1" Then rbcity.Checked = True
                    If Request.QueryString("selltype") = "2" Then rball.Checked = True
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
                End If
                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepSplPLWindowPostBack") Then
                    BtnPrint_Click(sender, e)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptspecialeventPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load





        If Page.IsPostBack = True Then

            Try


                Dim servicetype As String = ""
                If hdnsptypecode.Value = "" Then

                    servicetype = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))
                    ddlSpTypeName.Value = CType(servicetype, String)
                    ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
                Else
                    ddlSpTypeName.Value = hdnsptypecode.Value
                    ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text


                End If
                If hdnctrycode.Value = "" Then
                    default_country = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
                    ddlcontName.Value = CType(default_country, String)
                    ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text

                Else

                    ddlcontName.Value = hdnctrycode.Value
                    ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text

                End If



                strqry = ""
                If ddlContCode.Value <> "[Select]" Then
                    strqry = " and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "'"
                End If
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1" & strqry & " order by citycode", True, ddlCCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1" & strqry & " order by cityname", True, ddlCatName.Value)

                ddlCityName.Value = hdncitycode.Value
                ddlCityCode.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text
                strqry = ""
                If ddlSPTypeCode.Value <> "[Select]" Then
                    strqry = " and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "'"
                End If

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1" & strqry & " order by catcode", True, ddlCityCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1" & strqry & " order by catname", True, ddlCityName.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode,scatname from sellcatmast where active=1" & strqry & " order by scatcode", True, ddlSellingCode.Value)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1" & strqry & " order by scatname", True, ddlSellingName.Value)
                ddlCatName.Value = hdncategory.Value
                ddlCCode.Value = ddlCatName.Items(ddlCatName.SelectedIndex).Text
                ddlSellingName.Value = hdnsellcatcode.Value
                ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text
                strqry = ""

                If ddlSPTypeCode.Value <> "[Select]" Then
                    strqry = " and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "'"
                End If



                If ddlCCode.Value <> "[Select]" Then
                    strqry = strqry & " and catcode='" & ddlCCode.Items(ddlCCode.SelectedIndex).Text & "'"
                End If
                'If ddlSellingCode.Value <> "[Select]" Then
                '    strqry = strqry & " and scatcode='" & ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text & "'"
                'End If
                If ddlContCode.Value <> "[Select]" Then
                    strqry = strqry & " and ctrycode='" & ddlContCode.Items(ddlContCode.SelectedIndex).Text & "'"
                End If
                If ddlCityCode.Value <> "[Select]" Then
                    strqry = strqry & " and citycode='" & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text & "'"
                End If


                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1" & strqry & " order by partycode", True, )
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 " & strqry & " order by partyname", True)

                ddlPartyName.Value = hdnsuppliercode.Value
                ddlPartyCode.Value = ddlPartyName.Items(ddlPartyName.SelectedIndex).Text






                If ddlPartyCode.Value <> "[Select]" Then
                    strqry = " and partycode='" & ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text & "'"
                End If

                If ddlSPTypeCode.Value <> "[Select]" Then
                    strqry = strqry & " and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "'"
                End If

























                If ddlMarketCode.Value <> "[Select]" Then
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "sellcode", "sellname", "select sellcode,sellname from sellmast where active=1 and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "' order by sellcode", True, ddlSellingCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "sellname", "sellcode", "select sellname,sellcode from sellmast where active=1 and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "' order by sellname", True, ddlSellingName.Value)
                Else
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "sellcode", "sellname", "select sellcode,sellname from sellmast where active=1 order by sellcode", True, ddlSellingCode.Value)
                    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "sellname", "sellcode", "select sellname,sellcode from sellmast where active=1 order by sellname", True, ddlSellingName.Value)
                End If
                ddlSellingName.Value = hdnsellingtypecode.Value
                ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text


            Catch ex As Exception

            End Try
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
        ddlSPTypeCode.Value = "[Select]"
        ddlSpTypeName.Value = "[Select]"
        ddlSellingCode.Value = "[Select]"
        ddlSellingName.Value = "[Select]"
        ddlCCode.Value = "[Select]"
        ddlCatName.Value = "[Select]"
        ddlContCode.Value = "[Select]"
        ddlcontName.Value = "[Select]"
        ddlCityCode.Value = "[Select]"
        ddlCityName.Value = "[Select]"
         ddlSeasoncode.Value = "[Select]"
        ddlSeasonName.Value = "[Select]"

        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
        txtUpdateAsOn.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))

        txtremarks.Text = ""

    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            'If ddlSPTypeCode.Value = "" Or UCase(Trim(ddlSPTypeCode.Value)) = UCase(Trim("[Select]")) Then
            '    MsgBox("Supplier Type can not be blank.", MsgBoxStyle.Information, "Price List")
            '    SetFocus(ddlSPTypeCode)
            '    ValidatePage = False
            '    Exit Function
            'End If

            'If ddlMarketCode.Value = "" Or UCase(Trim(ddlMarketCode.Value)) = UCase(Trim("[Select]")) Then
            '    MsgBox("Market can not be blank.", MsgBoxStyle.Information, "Price List")
            '    SetFocus(ddlMarketCode)
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

            If txtUpdateAsOn.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank.');", True)

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
                'Session.Add("Pageame", "SpecialEventsPricelist")
                'Session.Add("BackPageName", "rptspleventPricelistReport.aspx")

                strsptypecode = IIf(UCase(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, "")
                strppartycode = IIf(UCase(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, "")
                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)
                strasondate = txtUpdateAsOn.Text
                strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
                strsellcode = IIf(UCase(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text, "")
                strcitycode = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
                strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
                strcatcode = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "")
                Dim strapprove As Integer
                If rbbeach.Checked = True Then strselltype = "0"
                If rbcity.Checked = True Then strselltype = "1"
                If rball.Checked = True Then strselltype = "2"
                strapprove = CType(ddlapprovestatus.SelectedValue, String)
                strReportTitle = "Special Events Price List"

                strreportoption = ""

                'Response.Redirect("rptspleventPricelistReport.aspx?sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
                '& "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&sellcode=" & strsellcode _
                '& "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode _
                '& "&selltype=" & strselltype _
                '& "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)
                Dim strpop As String = ""
                'strpop = "window.open('rptspleventPricelistReport.aspx?&Pageame=SpecialEventsPricelist&BackPageName=rptspleventPricelistReport.aspx&sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
                '& "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&sellcode=" & strsellcode _
                '& "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&approve=" & strapprove & "&catcode=" & strcatcode _
                '& "&selltype=" & strselltype _
                '& "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepSplPL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                strpop = "window.open('rptspleventPricelistReport.aspx?&Pageame=SpecialEventsPricelist&BackPageName=rptspleventPricelistReport.aspx&sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
                & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&sellcode=" & strsellcode _
                & "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&approve=" & strapprove & "&catcode=" & strcatcode _
                & "&selltype=" & strselltype _
                & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepSplPL');"


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptspleventPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

   


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptSplEventsPL','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
