Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptPriceExpirySearch
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
    Dim objUser As New clsUser
    Dim strappid As String = ""
    Dim strappname As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If dtpason.Text = "" Then
            dtpason.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        End If

        If Page.IsPostBack = False Then
            Try

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)

                strappid = AppId.Value
                If AppId Is Nothing = False Then

                    strappname = AppName.Value
                End If



                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\rptPriceExpirySearch.aspx", btnadd, Button1, BtnPrint, gv_SearchResult)
                End If








                txtconnection.Value = Session("dbconnectionName")

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlscatcode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where active=1 order by scatname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlscatName, "scatname", "scatcode", "select scatname, scatcode from sellcatmast where active=1 order by scatname", True)


                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)


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
                '''''''''''''''

                ' Party filter  '''''''''''
                strqry = "select ltrim(rtrim(partycode)) partycode,ltrim(rtrim(partyname)) partyname from partymast where active=1 "
                If default_country <> "" Then
                    strqry = strqry + " and ctrycode='" & default_country & "'"
                End If
                strqry = strqry + " order by partycode"
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", strqry, True)

                strqry = "select ltrim(rtrim(partyname)) partyname,ltrim(rtrim(partycode)) partycode from partymast where active=1 "
                If default_country <> "" Then
                    strqry = strqry + " and ctrycode='" & default_country & "'"
                End If
                strqry = strqry + " order by partyname"
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", strqry, True)


                If Request.QueryString("sptypecode") <> "" Then
                    ddlSpTypeName.Value = Request.QueryString("sptypecode")
                    ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
                End If
                If Request.QueryString("partycodef") <> "" Then
                    ddlPartyName.Value = Request.QueryString("partycodef")
                    ddlPartyCode.Value = ddlPartyName.Items(ddlPartyName.SelectedIndex).Text
                End If
                If Request.QueryString("asondate") <> "" Then
                    dtpason.Text = Format("U", CType(Request.QueryString("asondate"), Date))
                End If

                If Request.QueryString("citycodef") <> "" Then
                    ddlCityName.Value = Request.QueryString("citycodef")
                    ddlCityCode.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text
                End If

                If Request.QueryString("plgrpcodef") <> "" Then
                    ddlMarketName.Value = Request.QueryString("plgrpcodef")
                    ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
                End If

                If Request.QueryString("skip") <> "" Then
                    If Request.QueryString("skip") = "1" Then chkskip.Checked = True
                    If Request.QueryString("skip") = "0" Then chkskip.Checked = True
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptPriceExpirySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If



        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSpTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCatName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlPartyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlPartyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepPriceExpWindowPostBack") Then
            BtnPrint_Click(sender, e)
        End If
    End Sub

  

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlPartyCode.Value = "[Select]"
        ddlPartyName.Value = "[Select]"
        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"

        ddlSPTypeCode.Value = "[Select]"
        ddlSpTypeName.Value = "[Select]"
        ddlCityCode.Value = "[Select]"
        ddlCityName.Value = "[Select]"

        dtpason.Text = ""



    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            If ddlSPTypeCode.Value = "" Or UCase(Trim(ddlSPTypeCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Type can not be blank.');", True)
                SetFocus(ddlSPTypeCode)
                ValidatePage = False
                Exit Function
            End If

            If dtpason.Text = "" Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on Date field can not be blank.');", True)
                SetFocus(dtpason)
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
                'Session.Add("Pageame", "PriceExpiry")
                'Session.Add("BackPageName", "rptPriceExpirySearch.aspx")

                Dim strReportTitle As String = ""

                Dim strsptypecode As String = ""

                Dim strpartycodef As String = ""
                Dim strplgrpcodef As String = ""
                Dim strcitycodef As String = ""
                Dim strctrycode As String = ""
                Dim strcatcode As String = ""
                Dim strsellcatcode As String = ""

                Dim strexcept As String = ""
                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""
                Dim strasondate As String = ""
                Dim strapprove As String

                strsptypecode = IIf(UCase(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, "")
                strpartycodef = IIf(UCase(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, "")
                strplgrpcodef = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
                strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
                strcitycodef = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
                strcatcode = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "")
                strsellcatcode = IIf(UCase(ddlscatcode.Items(ddlscatcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlscatcode.Items(ddlscatcode.SelectedIndex).Text, "")

                strasondate = Mid(Format(CType(dtpason.Text, Date), "u"), 1, 10)
                strreportoption = Mid(dtpason.Text, 1, 10)

                If chkskip.Checked = True Then strexcept = "Y"
                If chkskip.Checked = False Then strexcept = "N"
                strapprove = CType(ddlapprovestatus.SelectedValue, String)

                strReportTitle = "Price Expiry"

                Dim strpop As String = ""
                'strpop = "window.open('rptPriceExpiryReport.aspx?Pageame=PriceExpiry&BackPageName=rptPriceExpirySearch.aspx&skip=" & strexcept & "&sptypecode=" & strsptypecode _
                '                & "&partycodef=" & strpartycodef & "&plgrpcodef=" & strplgrpcodef & "&approve=" & strapprove _
                '                & "&citycodef=" & strcitycodef & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode & "&sellcatcode=" & strsellcatcode & "&asondate=" & strasondate _
                '                & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepPriceExp','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

                strpop = "window.open('rptPriceExpiryReport.aspx?Pageame=PriceExpiry&BackPageName=rptPriceExpirySearch.aspx&skip=" & strexcept & "&sptypecode=" & strsptypecode _
                                & "&partycodef=" & strpartycodef & "&plgrpcodef=" & strplgrpcodef & "&approve=" & strapprove _
                                & "&citycodef=" & strcitycodef & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode & "&sellcatcode=" & strsellcatcode & "&asondate=" & strasondate _
                                & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepPriceExp');"



                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptPriceExpirySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub



 

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptPriceExpirySearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = True Then
            Dim servicetype As String = ""
            If hdnsptypecode.Value = "" Then

                servicetype = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))
                ddlSpTypeName.Value = CType(servicetype, String)
                ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
            End If
            If hdnctrycode.Value = "" Then
                default_country = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
                ddlcontName.Value = CType(default_country, String)
                ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text
            End If

            ddlSpTypeName.Value = hdnsptypecode.Value
            ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
            ddlcontName.Value = hdnctrycode.Value
            ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text
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
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlscatcode, "scatcode", "scatname", "select scatcode,scatname from sellcatmast where active=1" & strqry & " order by scatcode", True, ddlscatcode.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlscatName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1" & strqry & " order by scatname", True, ddlscatName.Value)
            ddlCatName.Value = hdncategory.Value
            ddlCCode.Value = ddlCatName.Items(ddlCatName.SelectedIndex).Text
            ddlscatName.Value = hdnsellcatcode.Value
            ddlscatcode.Value = ddlscatName.Items(ddlscatName.SelectedIndex).Text
            If ddlSPTypeCode.Value <> "[Select]" Then
                strqry = "sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "'"
            End If
            If ddlCCode.Value <> "[Select]" Then
                strqry = strqry & " and catcode='" & ddlCCode.Items(ddlCCode.SelectedIndex).Text & "'"
            End If
            If ddlscatcode.Value <> "[Select]" Then
                strqry = strqry & " and scatcode='" & ddlscatcode.Items(ddlscatcode.SelectedIndex).Text & "'"
            End If
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



            strqry = ""

            If ddlPartyCode.Value <> "[Select]" Then
                strqry = " and partycode='" & ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text & "'"
            End If

            If ddlSPTypeCode.Value <> "[Select]" Then
                strqry = strqry & " and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "'"
            End If
            ddlMarketName.Value = hdnmarketcode.Value
            ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text


            'If hdncategory.Value <> "" Then
            '    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1" & strqry & " order by partycode", True, )
            '    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 " & strqry & " order by partyname", True)

            'End If

        End If

    End Sub
End Class
