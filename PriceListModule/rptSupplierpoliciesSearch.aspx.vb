Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptSupplierpoliciesSearch
    Inherits System.Web.UI.Page

    Dim objDateTime As New clsDateTime
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim servicetype As String
    Dim default_country As String
    Dim strqry As String
    Dim strReportTitle As String = ""

    Dim strsptypecode As String = ""
    Dim strppartycode As String = ""
    Dim strfromdate As String = ""
    Dim strtodate As String = ""
    Dim strasondate As String = ""
    Dim strplgrpcode As String = ""
    Dim strcitycode As String = ""
    Dim strctrycode As String = ""
    Dim strcatcode As String = ""
    Dim strscatcode As String = ""
    Dim strpromtype As String = ""
    Dim strrepfilter As String = ""
    Dim strreportoption As String = ""
    Dim strapprove As String = ""
    Dim strroomtype As String = ""
    Dim strpartyfilter As String = ""

    Dim objUser As New clsUser

    Dim strappid As String = ""
    Dim strappname As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.IsPostBack = False Then
            Try
                If txtFromDate.Text = "" Then
                    txtFromDate.Text = objDateTime.GetSystemDateOnly(Session("dbconnectionName"))
                End If
                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
                End If
                txtToDate.Text = txtToDate.Text.Substring(0, 5) + "/2020"

                If txtUpdateAsOn.Text = "" Then
                    txtUpdateAsOn.Text = objDateTime.GetSystemDateOnly(Session("dbconnectionName"))
                End If

                txtconnection.Value = Session("dbconnectionName")

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode, scatname from sellcatmast where active=1 order by scatcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1 order by scatname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeCode, "rmtypcode", "rmtypname", "select rmtypcode,rmtypname from rmtypmast where active=1 order by rmtypcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypename, "rmtypname", "rmtypcode", "select rmtypname,rmtypcode from rmtypmast where active=1 order by rmtypname", True)


                servicetype = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))
                ddlSpTypeName.Value = CType(servicetype, String)
                ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

                default_country = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
                ddlcontName.Value = CType(default_country, String)
                ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text

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
                If Request.QueryString("scatcode") <> "" Then
                    ddlSellingName.Value = Request.QueryString("scatcode")
                    ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text
                End If

                If Request.QueryString("plgrpcode") <> "" Then
                    ddlMarketName.Value = Request.QueryString("plgrpcode")
                    ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
                End If

                If Request.QueryString("promtype") <> "" Then
                    strpromtype = Request.QueryString("promtype")
                End If

                If Request.QueryString("reportoption") <> "" Then
                    strreportoption = Request.QueryString("reportoption")
                    If strreportoption = "1" Then
                        rball.Checked = True
                        rballclicked()
                    Else
                        rbparticular.Checked = True
                        rbparticularclicked()
                        If strreportoption = "2" Then rbcancellation.Checked = True
                        If strreportoption = "3" Then rbchild.Checked = True
                        If strreportoption = "4" Then rbpromotion.Checked = True
                        If strreportoption = "5" Then rbpromotionrmks.Checked = True
                        If strreportoption = "6" Then rbcompulsory.Checked = True
                        If strreportoption = "7" Then rbremarks.Checked = True
                        If strreportoption = "8" Then rbblocksales.Checked = True
                        If strreportoption = "9" Then rbminimumnights.Checked = True
                        If strreportoption = "10" Then rbmaxaccomadation.Checked = True
                        If strreportoption = "11" Then rbspecialevent.Checked = True
                    End If
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
            ddlRmtypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRmtypename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepSupPolWindowPostBack") Then
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

            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1" & strqry & " order by catcode", True, ddlCityCode.Value)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1" & strqry & " order by catname", True, ddlCityName.Value)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "scatcode", "scatname", "select scatcode,scatname from sellcatmast where active=1" & strqry & " order by scatcode", True, ddlSellingCode.Value)
            'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1" & strqry & " order by scatname", True, ddlSellingName.Value)
            'ddlCatName.Value = hdncategory.Value
            'ddlCCode.Value = ddlCatName.Items(ddlCatName.SelectedIndex).Text
            'ddlSellingName.Value = hdnsellcatcode.Value
            'ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text
            If ddlCCode.Value <> "[Select]" Then
                strqry = strqry & " and catcode='" & ddlCCode.Items(ddlCCode.SelectedIndex).Text & "'"
            End If
            If ddlSellingCode.Value <> "[Select]" Then
                strqry = strqry & " and scatcode='" & ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text & "'"
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

            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeCode, "rmtypcode", "rmtypname", "select rmtypmast.rmtypcode , rmtypmast.rmtypname from rmtypmast,partyrmtyp where  partyrmtyp.inactive=0  and rmtypmast.active=1 and rmtypmast.rmtypcode=partyrmtyp.rmtypcode" & strqry & "  order by rmtypcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypename, "rmtypname", "rmtypcode", "select rmtypmast.rmtypcode , rmtypmast.rmtypname from rmtypmast,partyrmtyp where  partyrmtyp.inactive=0  and rmtypmast.active=1 and rmtypmast.rmtypcode=partyrmtyp.rmtypcode" & strqry & "  order by rmtypname", True)



            ddlRmtypename.Value = hdnroomtypecode.Value
            ddlRmtypeCode.Value = ddlRmtypename.Items(ddlRmtypename.SelectedIndex).Text


            ddlMarketName.Value = hdnmarketcode.Value
            ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text


            'If hdncategory.Value <> "" Then
            '    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1" & strqry & " order by partycode", True, )
            '    objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 " & strqry & " order by partyname", True)

            'End If
        Else
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
                                                   CType(strappname, String), "PriceListModule\rptSupplierpoliciesSearch.aspx", btnadd, Button1, BtnPrint, gv_SearchResult)
            End If

        End If


    End Sub
    
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        '    rep.Close()
        '    rep.Dispose()
        'End If
    End Sub

    Protected Sub rbparticular_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbparticular.CheckedChanged
        rbparticularclicked()
    End Sub

    Protected Sub rball_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rball.CheckedChanged
        rballclicked()
    End Sub

    Protected Sub rballclicked()
        rbblocksales.Visible = False
        rbcancellation.Visible = False
        rbchild.Visible = False
        rbcompulsory.Visible = False
        rbearlybird.Visible = False
        rbminimumnights.Visible = False
        rbpromotion.Visible = False
        rbpromotionrmks.Visible = False
        rbmaxaccomadation.Visible = False
        ' rbpromotionandearly.Visible = False
        rbremarks.Visible = False
        rbspecialevent.Visible = False
        lblremarks.Visible = False
    End Sub
    Protected Sub rbparticularclicked()
        rbblocksales.Visible = True
        rbcancellation.Visible = True
        rbchild.Visible = True
        rbcompulsory.Visible = True
        'rbearlybird.Visible = True
        rbminimumnights.Visible = True
        rbpromotion.Visible = True
        rbpromotionrmks.Visible = True
        rbmaxaccomadation.Visible = True
        ' rbpromotionandearly.Visible = True
        rbremarks.Visible = True
        rbspecialevent.Visible = True
        lblremarks.Visible = True

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

        txtFromDate.Text = objDateTime.GetSystemDateOnly(Session("dbconnectionName"))
        txtToDate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
        txtUpdateAsOn.Text = objDateTime.GetSystemDateOnly(Session("dbconnectionName"))

      
    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            If ddlSPTypeCode.Value = "" Or UCase(Trim(ddlSPTypeCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Type can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSPTypeCode.ClientID + "');", True)
                'SetFocus(ddlSPTypeCode)
                ValidatePage = False
                Exit Function
            End If

            'If ddlMarketCode.Value = "" Or UCase(Trim(ddlMarketCode.Value)) = UCase(Trim("[Select]")) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Market can not be blank.');", True)
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlMarketCode.ClientID + "');", True)
            '    'SetFocus(ddlMarketCode)
            '    ValidatePage = False
            '    Exit Function
            'End If


            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                'SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If
            If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                ValidatePage = False
                Exit Function
            End If
            If txtUpdateAsOn.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on Date field can not be blank..');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtUpdateAsOn.ClientID + "');", True)
                'SetFocus(txtUpdateAsOn)
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
            If ValidatePage() = False Then
                Exit Sub
            End If
            'ViewState.Add("Pageame", "Supplierpolicies")
            'ViewState.Add("BackPageName", "rptSupplierpoliciesSearch.aspx")
            strsptypecode = IIf(UCase(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, "")
            strppartycode = IIf(UCase(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, "")
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)
            strasondate = Mid(Format(CType(txtUpdateAsOn.Text, Date), "u"), 1, 10)
            strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
            strcitycode = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
            strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
            strcatcode = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "")
            strscatcode = IIf(UCase(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text, "")
            strroomtype = IIf(UCase(ddlRmtypeCode.Items(ddlRmtypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlRmtypeCode.Items(ddlRmtypeCode.SelectedIndex).Text, "")
            If chkproviderfilter.Checked = True Then
                strpartyfilter = "1"
            Else
                strpartyfilter = "0"
            End If


            strpromtype = "0"

            strapprove = CType(ddlapprovestatus.SelectedValue, String)

            If rball.Checked = True Then
                strrepfilter = "All Policies"
                strreportoption = "1"
                strpromtype = "0"  ' 0 Both Earh & Promotion, 1 - Promotion, 2 Early Promotion
            Else
                If rbcancellation.Checked = True Then
                    strrepfilter = "Cancellation Policies"
                    strreportoption = "2"
                ElseIf rbchild.Checked = True Then
                    strrepfilter = "Child Policies"
                    strreportoption = "3"
                ElseIf rbpromotion.Checked = True Then
                    strrepfilter = "Promotion Policies"
                    strreportoption = "4"
                    strpromtype = "1"
                ElseIf rbpromotionrmks.Checked = True Then
                    strrepfilter = "Promotion Remarks"
                    strreportoption = "5"
                    strpromtype = "2"
                ElseIf rbcompulsory.Checked = True Then
                    strrepfilter = "Compulsory Remarks"
                    strreportoption = "6"
                ElseIf rbremarks.Checked = True Then
                    strrepfilter = "General Policy"
                    strreportoption = "7"
                ElseIf rbblocksales.Checked = True Then
                    strrepfilter = "Block Sales"
                    strreportoption = "8"
                ElseIf rbminimumnights.Checked = True Then
                    strrepfilter = "Minimum Nights"
                    strreportoption = "9"
                ElseIf rbmaxaccomadation.Checked = True Then
                    strrepfilter = "Maximum Accomodation"
                    strreportoption = "10"
                    strpromtype = "0"
                ElseIf rbspecialevent.Checked = True Then
                    strrepfilter = "Special Events"
                    strreportoption = "11"
                End If

            End If

            'Response.Redirect("rptSupplierpoliciesReport.aspx?sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
            '                  & "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&asondate=" & strasondate & "&fromdate=" & strfromdate & "&todate=" & strtodate _
            '                  & "&catcode=" & strcatcode & "&plgrpcode=" & strplgrpcode & "&promtype=" & strpromtype & "&scatcode=" & strscatcode & "&repfilter=" _
            '                  & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptSupplierpoliciesReport.aspx?Pageame=Supplierpolicies&BackPageName=rptSupplierpoliciesSearch.aspx&sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
            '                  & "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&asondate=" & strasondate & "&fromdate=" & strfromdate & "&todate=" & strtodate _
            '                  & "&catcode=" & strcatcode & "&plgrpcode=" & strplgrpcode & "&promtype=" & strpromtype & "&scatcode=" & strscatcode & "&roomtype=" & strroomtype & "&approve=" & strapprove & "&partyfilter=" & strpartyfilter & "&repfilter=" _
            '                  & strrepfilter & "&reportoption=" & strreportoption & "','RepTktSellType','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('rptSupplierpoliciesReport.aspx?Pageame=Supplierpolicies&BackPageName=rptSupplierpoliciesSearch.aspx&sptypecode=" & strsptypecode & "&partycode=" & strppartycode _
                              & "&citycode=" & strcitycode & "&ctrycode=" & strctrycode & "&asondate=" & strasondate & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                              & "&catcode=" & strcatcode & "&plgrpcode=" & strplgrpcode & "&promtype=" & strpromtype & "&scatcode=" & strscatcode & "&roomtype=" & strroomtype & "&approve=" & strapprove & "&partyfilter=" & strpartyfilter & "&repfilter=" _
                              & strrepfilter & "&reportoption=" & strreportoption & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptSupplierpoliciesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptSupplierpoliciesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
