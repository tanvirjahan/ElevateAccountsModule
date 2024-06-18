

'Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.CrystalReports
'Imports CrystalDecisions.Shared
'Imports CrystalDecisions.Web
'Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Net
#End Region

Partial Class rptPricelistSearch
    Inherits System.Web.UI.Page
    Dim objectcl As New clsDateTime
    ' Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim servicetype As String
    Dim default_country As String
    Dim default_market As String
    Dim default_selling As String
    Dim default_category As String
    Dim strqry As String
    Dim seasonfrom As String
    Dim seasonto As String
    Dim strReportTitle As String = ""

    Dim strsptypecode As String = ""
    Dim strppartycode As String = ""
    Dim strfromdate As String = ""
    Dim strtodate As String = ""
    Dim strplgrpcode As String = ""
    Dim stragentcode As String = ""
    Dim strsellcode As String = ""


    Dim strapprove As Integer
    Dim strrpttype As Integer
    Dim strshowweb As Integer

    Dim strcitycode As String = ""
    Dim strctrycode As String = ""
    Dim strcatcode As String = ""

    Dim strroomtype As String = ""
    Dim strmeal As String = ""
    Dim strselltype As String = ""
    Dim strexcept As String = ""
    Dim strpromocountry As String = ""

    Dim strpromtype As String = ""
    Dim strrepfilter As String = ""
    Dim strreportoption As String = ""
    Dim strasondate As String = ""

    Dim objUser As New clsUser
    Dim GroupId, AppId As Integer
    Dim PrivilegeId As String

    Dim strsellcatcode As String = ""
    Dim strpartyfilter As String = "0"
    Dim strcost As String = "0"
#Region "Global Declaration"
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim strSqlQry As String
#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Page.IsPostBack = False Then
            Try

                Dim AppId1 As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName1 As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId1 Is Nothing = False Then
                    strappid = AppId1.Value
                End If
                If AppName1 Is Nothing = False Then
                    strappname = AppName1.Value
                End If


                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                txtconnection.Value = Session("dbconnectionName")

                GroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                AppId = strappid
                PrivilegeId = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select privilegeid from group_privilege_detail where privilegeid=11 and appid=" & strappid & " and groupid=" & GroupId)

                ddlrpttype.SelectedIndex = 1
                ddlapprovestatus.SelectedIndex = 0

                If CType(Val(PrivilegeId), String) = "11" Then
                    lblmsg.Visible = True
                    divcost.Visible = True
                    Label11.Visible = True
                    rbcostnet.Visible = True
                    rbcosthotel.Visible = True
                Else
                    lblmsg.Visible = False
                    divcost.Visible = False
                    Label11.Visible = False
                    rbcostnet.Visible = False
                    rbcosthotel.Visible = False
                End If

                If txtFromDate.Text = "" Then
                    txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If
                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If
                txtToDate.Text = txtToDate.Text.Substring(0, 5) + "/2020"
                If txtUpdateAsOn.Text = "" Then
                    txtUpdateAsOn.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If


                default_category = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlseas1code, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 order by seascode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlseas1name, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 order by seasname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 and sptypecode='" & default_category & "' order by catcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 and sptypecode='" & default_category & "' order by catname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAgentCode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAgentName, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)

                ' Added ddlAgentCode and ddlAgentName by Archana on 21/05/2015

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "sellcode", "sellname", "select sellcode, sellname from sellmast where active=1 order by sellcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "sellname", "sellcode", "select sellname, sellcode from sellmast where active=1 order by sellname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlscatcode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where active=1 and sptypecode='" & default_category & "' order by scatname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlscatName, "scatname", "scatcode", "select scatname, scatcode from sellcatmast where active=1 and sptypecode='" & default_category & "' order by scatname", True)


                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSeasoncode, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 order by seascode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSeasonName, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 order by seasname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeCode, "rmtypcode", "rmtypname", "select rmtypcode,rmtypname from rmtypmast where active=1 order by rmtypcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypename, "rmtypname", "rmtypcode", "select rmtypname,rmtypcode from rmtypmast where active=1 order by rmtypname", True)


                servicetype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))
                ddlSpTypeName.Value = CType(servicetype, String)
                ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

                default_country = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
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


                ' Roomtype Filter '''''''''''
                'strqry = "select rmtypcode,rmtypname from rmtypmast where active=1 "
                'If servicetype <> "" Then
                '    strqry = strqry + " and sptypecode='" & servicetype & "'"
                'End If
                'strqry = strqry + " order by rmtypcode"
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlRmtypeCode, "rmtypcode", "rmtypname", strqry, True)

                'strqry = "select rmtypname,rmtypcode from rmtypmast where active=1 "
                'If servicetype <> "" Then
                '    strqry = strqry + " and sptypecode='" & servicetype & "'"
                'End If
                'strqry = strqry + " order by rmtypname"
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlRmtypename, "rmtypname", "rmtypcode", strqry, True)
                '''''''''''''''

                If Request.QueryString("sptypecode") <> "" Then
                    ddlSpTypeName.Value = Request.QueryString("sptypecode")
                    ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
                End If
                If Request.QueryString("partycode") <> "" Then
                    ddlPartyName.Value = Request.QueryString("partycode")
                    ddlPartyCode.Value = ddlPartyName.Items(ddlPartyName.SelectedIndex).Text

                    txtSupplierCode.Value = Request.QueryString("partycode")
                    txtSellingTypeName.Value = ddlPartyName.Items(ddlPartyName.SelectedIndex).Text
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

                    txtCityCode.Value = Request.QueryString("citycode")
                    txtCityName.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text
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
                    txtSellingTypeCode.Value = Request.QueryString("sellcode")
                    txtSellingTypeName.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text
                End If

                If Request.QueryString("plgrpcode") <> "" Then
                    ddlMarketName.Value = Request.QueryString("plgrpcode")
                    ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
                End If

                If Request.QueryString("agentcode") <> "" Then
                    ddlAgentName.Value = Request.QueryString("agentcode")
                    ddlAgentCode.Value = ddlAgentName.Items(ddlAgentName.SelectedIndex).Text
                End If

                'Added agentcode by Archana on 21/05/2015

                If Request.QueryString("meal") <> "" Then
                    If Request.QueryString("meal") = "1" Then rbmealyes.Checked = True
                    If Request.QueryString("meal") = "0" Then rbmealno.Checked = True
                End If

                If Request.QueryString("selltype") <> "" Then
                    If Request.QueryString("selltype") = "0" Then rbbeach.Checked = True
                    If Request.QueryString("selltype") = "1" Then rbcity.Checked = True
                    If Request.QueryString("selltype") = "2" Then rball.Checked = True
                End If

                If Request.QueryString("except") <> "" Then
                    If Request.QueryString("except") = "0" Then chkexcept.Checked = False
                    If Request.QueryString("except") = "1" Then chkexcept.Checked = True
                End If

                If Request.QueryString("roomtype") <> "" Then
                    ddlRmtypename.Value = Request.QueryString("roomtype")
                    ddlRmtypeCode.Value = ddlRmtypename.Items(ddlRmtypename.SelectedIndex).Text

                    txtRoomTypeCode.Value = Request.QueryString("roomtype")
                    txtRoomTypeName.Value = ddlRmtypename.Items(ddlRmtypename.SelectedIndex).Text
                End If

                'If Request.QueryString("promocountry") <> "" Then
                '    hdntxtbox.Value = Request.QueryString("promocountry")
                'End If

                chkDisplayRates.Text = chkDisplayRates.Text & " " & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                'lblConvRate.Text = lblConvRate.Text & " " & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                NumbersHtml(txtConversionRate)
                'ddlPartyCode.Attributes.Add("onblur", "javascript:FillCurrencyCode('Supplier');")
                'ddlSellingCode.Attributes.Add("onblur", "javascript:FillCurrencyCode('SellType');")

                txtFromDate.Attributes.Add("onblur", "javascript:ChangeDate();")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            ddlAgentCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);") 'Added by Archana on 21/05/2015
            ddlAgentName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);") 'Added by Archana on 21/05/2015

            ddlSeasoncode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSeasonName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlRmtypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRmtypename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlseas1code.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlseas1name.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepPLWindowPostBack") Then
            BtnPrint_Click(sender, e)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = True Then
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

            Dim agentcode As String
            If hdnagentcode.Value <> "" And hdnagentcode.Value <> "[Select]" Then
                agentcode = hdnagentcode.Value
                hdnmarketcode.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select plgrpcode from agentmast where agentcode='" + agentcode + "'")
                hdnsellingtype.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sellcode from agentmast where agentcode='" + agentcode + "'")

            End If

            'Added agentcode by Archana

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
           
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeCode, "rmtypcode", "rmtypname", "select rmtypmast.rmtypcode , rmtypmast.rmtypname from rmtypmast,partyrmtyp where  partyrmtyp.inactive=0  and rmtypmast.active=1 and rmtypmast.rmtypcode=partyrmtyp.rmtypcode" & strqry & "  order by rmtypcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypename, "rmtypname", "rmtypcode", "select rmtypmast.rmtypcode , rmtypmast.rmtypname from rmtypmast,partyrmtyp where  partyrmtyp.inactive=0  and rmtypmast.active=1 and rmtypmast.rmtypcode=partyrmtyp.rmtypcode" & strqry & "  order by rmtypname", True)

            ddlRmtypename.Value = hdnroomtypecode.Value
            ddlRmtypeCode.Value = ddlRmtypename.Items(ddlRmtypename.SelectedIndex).Text

            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

            ddlMarketName.Value = hdnmarketcode.Value
            ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
            strqry = ""
            If ddlMarketCode.Value <> "[Select]" Then
                strqry = " and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "'"
            End If


            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "sellcode", "sellname", "select sellcode, sellname from sellmast where active=1  " & strqry & "  order by sellcode", True, txtSellingTypeName.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "sellname", "sellcode", "select sellname, sellcode from sellmast where active=1 " & strqry & "   order by sellname", True, txtSellingTypeCode.Value)

            ddlSellingName.Value = hdnsellingtype.Value
            ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text


            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAgentCode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAgentName, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)

            ddlAgentName.Value = hdnagentcode.Value
            ddlAgentCode.Value = ddlAgentName.Items(ddlAgentName.SelectedIndex).Text
            strqry = ""
            'If ddlAgentCode.Value <> "[Select]" Then
            '    strqry = " and agentcode='" & ddlAgentCode.Items(ddlAgentCode.SelectedIndex).Text & "'"
            'End If
            'Added agentcode by Archana
           
            strqry = ""
            If ddlseas1code.Value <> "[Select]" Then
                strqry = " and seascode='" & ddlseas1code.Items(ddlseas1code.SelectedIndex).Text & "'"
            End If

            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlseas1code, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 " & strqry & " order by seascode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlseas1name, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 " & strqry & " order by seasname", True)
            ddlseas1name.Value = hdnseasoncode.Value
            ddlseas1code.Value = ddlseas1name.Items(ddlseas1name.SelectedIndex).Text

            'txtFromDate.Text = hdnfromdate.Value
            'txtToDate.Text = hdntodate.Value
            'If hdncategory.Value <> "" Then


        End If

    End Sub



    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlPartyCode.Value = "[Select]"
        ddlPartyName.Value = "[Select]"
        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"
        ddlAgentCode.Value = "[Select]" 'Added by Archana on 21/05/2015
        ddlAgentName.Value = "[Select]" 'Added by Archana on 21/05/2015
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
        ddlRmtypeCode.Value = "[Select]"
        ddlRmtypename.Value = "[Select]"
        ddlSeasoncode.Value = "[Select]"
        ddlSeasonName.Value = "[Select]"
        ddlscatcode.Value = "[Select]"
        ddlscatName.Value = "[Select]"

        txtCurrency.Value = ""
        txtConversionRate.Value = ""
        If txtFromDate.Text = "" Then
            txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        End If
        If txtToDate.Text = "" Then
            txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
        End If
        If txtUpdateAsOn.Text = "" Then
            txtUpdateAsOn.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        End If
        txtremarks.Text = ""

    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            If ddlSPTypeCode.Value = "" Or UCase(Trim(ddlSPTypeCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Type can not be blank.');", True)
                '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSPTypeCode.ClientID + "');", True)
                'SetFocus(ddlSPTypeCode)
                ValidatePage = False
                Exit Function
            End If

            If ddlMarketCode.Value = "" Or UCase(Trim(ddlMarketCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Market can not be blank.');", True)
                '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlMarketCode.ClientID + "');", True)
                'SetFocus(ddlMarketCode)
                ValidatePage = False
                Exit Function
            End If

            'If ddlAgentCode.Value = "" Or UCase(Trim(ddlAgentCode.Value)) = UCase(Trim("[Select]")) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Agent can not be blank.');", True)
            '    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlAgentCode.ClientID + "');", True)
            '    'SetFocus(ddlAgentCode)
            '    ValidatePage = False
            '    Exit Function
            'End If
            'Added agentcode by Archana

            If chkexcept.Checked = True And UCase(Trim(ddlCityCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('City Code can not be blank, when Check Expect is Selected');", True)
                '               ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCityCode.ClientID + "');", True)
                ' SetFocus(ddlCityCode)
                ValidatePage = False
                Exit Function

            End If

            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                '              ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                'SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                '             ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If
            If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If txtUpdateAsOn.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank.');", True)
                '           ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtUpdateAsOn.ClientID + "');", True)
                'SetFocus(txtUpdateAsOn)
                ValidatePage = False
                Exit Function
            End If

            'If CType(objDateTime.ConvertDateromTextBoxToDatabaseFormat(dtpFromDate.txtDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabaseFormat(dtpToDate.txtDate.Text), Date) Then
            '    MsgBox("To date field should be greater than From date Field.", MsgBoxStyle.Information, "Supplier Policies")
            '    SetFocus(dtpFromDate)
            '    ValidatePage = False
            '    Exit Function
            'End If

            If chkDisplayRates.Checked = True Then
                If txtCurrency.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select supplier or selling type.');", True)
                    ValidatePage = False
                    Exit Function
                End If
            End If

            If lblmsg.Visible = False Then
                If ddlSellingCode.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select selling type.');", True)
                    ValidatePage = False
                    Exit Function
                End If
            Else
                If ddlSellingCode.Value = "[Select]" Then
                    If rbcostnet.Checked = False And rbcosthotel.Checked = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Print cost.');", True)
                        ValidatePage = False
                        Exit Function
                    End If
                End If

                End If
                ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try

            If ValidatePage() = False Then
                Exit Sub
            End If
            'Session.Add("Pageame", "Pricelist")
            'Session.Add("BackPageName", "rptPricelistSearch.aspx")

            strsptypecode = IIf(UCase(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, "")
            strppartycode = IIf(UCase(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, "")
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
            'Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
            strtodate = Format(CType(txtToDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
            strasondate = txtUpdateAsOn.Text
            strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
            stragentcode = IIf(UCase(ddlAgentCode.Items(ddlAgentCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlAgentCode.Items(ddlAgentCode.SelectedIndex).Text, "")
            strsellcode = IIf(UCase(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text, "")
            strsellcatcode = IIf(UCase(ddlscatcode.Items(ddlscatcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlscatcode.Items(ddlscatcode.SelectedIndex).Text, "") '17082011
            strcitycode = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
            strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
            strcatcode = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "")
            strroomtype = IIf(UCase(ddlRmtypeCode.Items(ddlRmtypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlRmtypeCode.Items(ddlRmtypeCode.SelectedIndex).Text, "")
            strpromocountry = txtbox.Text
            'strpromocountry = hdntxtbox.Value

            If chkproviderfilter.Checked = True Then strpartyfilter = "1"

            If rbmealyes.Checked = True Then strmeal = "1"
            If rbmealno.Checked = True Then strmeal = "0"

            If rbbeach.Checked = True Then strselltype = "0"
            If rbcity.Checked = True Then strselltype = "1"
            If rball.Checked = True Then strselltype = "2"

            If chkexcept.Checked = True Then strexcept = "1"
            If chkexcept.Checked = False Then strexcept = "0"

            strapprove = CType(ddlapprovestatus.SelectedValue, String)
            strshowweb = CType(ddlshowweb.SelectedValue, String)
            strrpttype = CType(ddlrpttype.SelectedValue, String)

            '0 will take for the Net Payable and selling
            strcost = 0
            If strsellcode = "" Then
                If rbcostnet.Checked = True Then
                    strcost = "0"
                End If

                If rbcosthotel.Checked = True Then
                    strcost = "1"
                End If
            End If

            strReportTitle = "Price List"

            strreportoption = ""

            'Session.Add("sptypecode", strsptypecode)
            'Session.Add("ppartycode", strppartycode)
            'Session.Add("fromdate", strfromdate)
            'Session.Add("todate", strtodate)
            'Session.Add("asondate", strasondate)
            'Session.Add("plgrpcode", strplgrpcode)
            'Session.Add("sellcode", strsellcode)

            'Session.Add("citycode", strcitycode)
            'Session.Add("ctrycode", strctrycode)
            'Session.Add("catcode", strcatcode)

            'Session.Add("meal", strmeal)
            'Session.Add("selltype", strselltype)
            'Session.Add("except", strexcept)
            'Session.Add("roomtype", strroomtype)

            'Session.Add("repfilter", strrepfilter)
            'Session.Add("reportoption", strreportoption)
            'Session.Add("ReportTitle", strReportTitle)

            'Response.Redirect("rptPricelistReport.aspx", False)
            'Response.Redirect("rptPricelistReport.aspx?sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
            '& "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&sellcode=" & strsellcode _
            '& "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode & "&meal=" & strmeal _
            '& "&selltype=" & strselltype & "&except=" & strexcept & "&roomtype=" & strroomtype _
            '& "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)
            Dim strpop As String = ""
            Dim conbase As Integer
            If chkDisplayRates.Checked Then
                conbase = 1
            Else
                conbase = 0
            End If
            Dim subcode As String
            If ddlseas1code.Items(ddlseas1code.SelectedIndex).Text <> "[Select]" Then
                subcode = ddlseas1code.Items(ddlseas1code.SelectedIndex).Text
            Else
                subcode = ""
            End If



            'strpop = "window.open('rptPricelistReport.aspx?pageame=Pricelist&BackPageName=rptPricelistSearch.aspx&sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
            '           & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&agentcode=" & stragentcode & "&sellcode=" & strsellcode _
            '           & "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode & "&meal=" & strmeal _
            '           & "&selltype=" & strselltype & "&except=" & strexcept & "&promocountry=" & strpromocountry & "&roomtype=" & strroomtype & "&subscode=" & subcode _
            '           & "&repfilter=" & strrepfilter & "&convtbase=" & conbase & "&crate=" & txtConversionRate.Value & "&approve=" & strapprove & "&rpttype=" & strrpttype & "&showweb=" & strshowweb & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "&pfilter=" & strpartyfilter & "&sellcatcode=" & strsellcatcode & "&hotelcost=" & strcost & "','RepPriceList','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

            strpop = "window.open('rptPricelistReport.aspx?pageame=Pricelist&BackPageName=rptPricelistSearch.aspx&sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
                       & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&asondate=" & strasondate & "&plgrpcode=" & strplgrpcode & "&agentcode=" & stragentcode & "&sellcode=" & strsellcode _
                       & "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&catcode=" & strcatcode & "&meal=" & strmeal _
                       & "&selltype=" & strselltype & "&except=" & strexcept & "&promocountry=" & strpromocountry & "&roomtype=" & strroomtype & "&subscode=" & subcode _
                       & "&repfilter=" & strrepfilter & "&convtbase=" & conbase & "&crate=" & txtConversionRate.Value & "&approve=" & strapprove & "&rpttype=" & strrpttype & "&showweb=" & strshowweb & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "&pfilter=" & strpartyfilter & "&sellcatcode=" & strsellcatcode & "&hotelcost=" & strcost & "','RepPriceList');"




            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    'Protected Sub ddlseas1name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If ddlseas1name.SelectedValue <> "[Select]" Then
    '        ddlseas1code.SelectedItem.Text = ddlseas1name.SelectedItem.Value
    '    End If
    '    txtFromDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"seasmast", "frmdate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))
    '    txtToDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"seasmast", "todate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))

    'End Sub

    'Protected Sub ddlseas1code_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlseas1code.SelectedIndexChanged
    '    If ddlseas1code.SelectedValue <> "[Select]" Then
    '        ddlseas1name.SelectedValue = ddlseas1code.SelectedItem.Text
    '    End If
    '    txtFromDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"seasmast", "frmdate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))
    '    txtToDate.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"seasmast", "todate", "seascode", CType(Me.ddlseas1code.Items(ddlseas1code.SelectedIndex).Text, String))
    'End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptPricelistSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumberhtml(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub

    'Public Function ValidatePage() As Boolean
    '    Try
    '        If txtFromDate.Text = "" Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter from date');", True)
    '            SetFocus(txtFromDate)
    '            ValidatePage = False
    '            Exit Function
    '        End If
    '        If txtToDate.Text = "" Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter to date');", True)
    '            SetFocus(txtToDate)
    '            ValidatePage = False
    '            Exit Function
    '        End If
    '        ValidatePage = True
    '    Catch ex As Exception

    '    End Try
    'End Function

    Protected Sub btnSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelect.Click

        'Dim txtbox As TextBox
        Dim txtPriceList As String()
        Dim objbtn As Button = CType(sender, Button)
        Dim chksel As CheckBox
        Dim j As Integer

        Dim chkheader As CheckBox
        getShowCountries()

        If gv_ShowCountries.Visible = True Then
            ModalExtraPopup.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select market');", True)

            ModalExtraPopup.Hide()
            Exit Sub
        End If
        For j = 0 To gv_ShowCountries.Rows.Count - 1
            chksel = gv_ShowCountries.Rows(j).FindControl("chk2")
            chksel.Checked = False

        Next
        chkheader = gv_ShowCountries.HeaderRow.FindControl("chkAll")

        ' If hdntxtbox.Value <> "" Then
        If txtbox.Text <> "" Then
            ' txtPriceList = hdntxtbox.Value.Split(";")
            txtPriceList = txtbox.Text.Split(";")

            For lj As Integer = 0 To gv_ShowCountries.Rows.Count - 1
                For txtprice As Integer = txtPriceList.GetLowerBound(0) To txtPriceList.GetUpperBound(0) 'Plan Array Loop
                    If ((gv_ShowCountries.Rows(lj).Cells(2).Text) = (txtPriceList(txtprice))) Then
                        chksel = gv_ShowCountries.Rows(lj).FindControl("chk2")
                        chksel.Checked = True
                        Exit For
                    End If

                Next
            Next
           
            If (gv_ShowCountries.Rows.Count = txtPriceList.Length) Then
                chkheader.Checked = True
            Else
                chkheader.Checked = False
            End If

        End If

      
    End Sub

    Private Sub getShowCountries()

        Try

            Dim MyDs As New DataTable
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "select ctrycode,ctryname from  ctrymast where  active=1 and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "'"

            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(MyDs)

            If MyDs.Rows.Count > 0 Then
                gv_ShowCountries.DataSource = MyDs
                gv_ShowCountries.DataBind()
               
                gv_ShowCountries.Visible = True

            Else

                gv_ShowCountries.Visible = False
                
            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objutils.WritErrorLog("rptpricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click
        Try
            ' Dim txtbox As TextBox
            Dim countrystring As String

            countrystring = getcountry()

            'hdncountrycode = CType(gvRow.FindControl("hdntxtbox"), HiddenField)
            ' hdntxtbox.Value = countrystring
            txtbox.Text = countrystring

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objutils.WritErrorLog("rptpricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Function getcountry() As String

        Dim country As String = ""
        Try

            Dim chk2 As CheckBox
            Dim txtcountrycode As Label

            For Each gvRow As GridViewRow In gv_ShowCountries.Rows
                chk2 = gvRow.FindControl("chk2")
                txtcountrycode = gvRow.FindControl("txtcountrycode")

                If chk2.Checked = True Then
                    country = country + txtcountrycode.Text + ";"

                End If
            Next

            If country.Length > 0 Then
                country = country.Substring(0, country.Length - 1)
            End If
            Return country
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objutils.WritErrorLog("rptpricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Return "" 'mealplan
        End Try

    End Function

    Protected Sub btnClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear1.Click

        ' hdntxtbox.Value = ""
        txtbox.Text = ""

    End Sub

End Class
