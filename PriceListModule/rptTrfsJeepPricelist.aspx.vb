Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_rptTrfsJeepPricelist
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

                ''lstOthCatLeft.DataSource = objutils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select othcatcode,othcatname from othcatmast where active <> 0  and othgrpcode in (select option_selected from reservation_parameters where param_id in (1001,1025))   order by  othcatname ") 'and othgrpcode='TRSPT'
                ''If objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select othcatcode,othcatname from othcatmast where active <> 0  and othgrpcode in (select option_selected from reservation_parameters where param_id in (1001,1025))   order by  othcatname ") Then 'and othgrpcode='TRSPT'
                'lstOthCatLeft.DataSource = objutils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select slabno,rtrim(ltrim(str(slabno)+ '-'+othcatcode)) as othcatname from othcat_slabs order by slabno")
                lstOthCatLeft.DataSource = objutils.ExecuteQuerynew(Session("dbconnectionName"), "sp_filltrfsJeepCatCode")
                lstOthCatLeft.DataValueField = "slabno"
                lstOthCatLeft.DataTextField = "othcatname"
                lstOthCatLeft.DataBind()

                If txtFromDate.Text = "" Then
                    txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If

                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If

                txtconnection.Value = Session("dbconnectionName")
                Dim strGrpQry As String = "select othgrpcode,othgrpname from othgrpmast where  active=1 and othgrpcode in (select " & _
                    " option_selected  from reservation_parameters where param_id in (1001,1025)) order by othgrpcode"
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGroupCode, "othgrpcode", "othgrpname", strGrpQry, True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGroupName, "othgrpname", "othgrpcode", strGrpQry, True)


                objutils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1code, "seascode", "seasname", "select seascode,seasname from seasmast where active=1 order by seascode", True)
                objutils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlseas1name, "seasname", "seascode", "select seasname,seascode from seasmast where active=1 order by seasname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "othsellcode", "othsellname", "select othsellcode, othsellname from othsellmast where active=1 order by othsellcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "othsellname", "othsellcode", "select othsellname, othsellcode from othsellmast where active=1 order by othsellname", True)

                If Request.QueryString("othergroupcode") <> "" Then
                    ddlOtherGroupName.Value = Request.QueryString("othergroupcode")
                    ddlOtherGroupCode.Value = ddlOtherGroupName.Items(ddlOtherGroupName.SelectedIndex).Text
                End If
                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                If Request.QueryString("sellcode") <> "" Then
                    ddlSellingName.Value = Request.QueryString("sellcode")
                    ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text
                End If

                If Request.QueryString("plgrpcode") <> "" Then
                    ddlMarketName.Value = Request.QueryString("plgrpcode")
                    ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
                End If

                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptTrfsJeepPricelist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                ddlSellingCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlseas1code.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlseas1name.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If

        End If

        'ClientScript.GetPostBackEventReference(Me, String.Empty)
        'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepOthPLWindowPostBack") Then
        '    BtnPrint_Click(sender, e)
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            Try

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

                'Dim str As String = "select slabno,rtrim(ltrim(str(slabno)+ '-'+othcatcode)) as othcatname from othcat_slabs "
                'If ddlOtherGroupName.Value <> "[Select]" Then
                '    str += " where  othgrpcode='" + ddlOtherGroupName.Value + "' "
                'End If
                'str += " order by  othcatname "
                Dim str As String = "sp_filltrfsJeepCatCode '" & IIf(ddlOtherGroupName.Value <> "[Select]", ddlOtherGroupName.Value, "") & "'"
                lstOthCatLeft.DataSource = objutils.ExecuteQuerySqlnew(Session("dbconnectionName"), str)
                lstOthCatLeft.DataValueField = "slabno"
                lstOthCatLeft.DataTextField = "othcatname"
                lstOthCatLeft.DataBind()

                'If hdnServiceCat.Value <> "" Then
                '    Dim strothcat() As String
                '    Dim i As Integer = 0
                '    strothcat = hdnServiceCat.Value.Split("$")
                '    For i = 0 To strothcat.Length
                '        lstOthCatRight.Items.Add()
                '    Next
                'End If
                'BtnPrint.Attributes.Add("onclick", "return ValidatePage();")
                'ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptTrfsJeepPricelist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        ddlOtherGroupCode.Value = "[Select]"
        ddlOtherGroupName.Value = "[Select]"
        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"
        ddlSellingCode.Value = "[Select]"
        ddlSellingName.Value = "[Select]"

        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))

    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            'If hdnServiceCat.Value = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select service category.');", True)

            '    SetFocus(lstOthCatLeft)
            '    ValidatePage = False
            '    Exit Function
            'End If
            'Dim str As String = hdnServiceCat.Value
            If ddlOtherGroupCode.Value = "" Or UCase(Trim(ddlOtherGroupCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group can not be blank.');", True)

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

            If ddlSellingCode.Value = "" Or UCase(Trim(ddlSellingCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selling Type can not be blank.');", True)
                SetFocus(ddlSellingCode)
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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank.');", True)

                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If
            If CType(objectcl.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objectcl.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
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
                'Session.Add("Pageame", "rptOtherServicePricelist")
                'Session.Add("BackPageName", "rptOtherServicePricelist.aspx")


                Dim strReportTitle As String = ""
                Dim strgrpcode As String = ""
                Dim strfromdate As String = ""
                Dim strtodate As String = ""
                Dim strplgrpcode As String = ""
                Dim strsellcode As String = ""
                Dim strpromtype As String = ""
                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""
                Dim strasondate As String = ""
                Dim strapprove As Integer
                Dim rtptype As String = ""

                strgrpcode = IIf(UCase(ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text, "")
                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)

                strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
                strsellcode = IIf(UCase(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text, "")
                strapprove = CType(ddlapprovestatus.SelectedValue, String)


                strreportoption = ""
                strReportTitle = "Transfer/Jeepwadi Price List"

                Dim othcat As String = ""
                Dim i As Integer = 0

                If hdnServiceCat.Value <> "" Then
                    othcat = hdnServiceCat.Value
                    othcat = othcat.Remove(0, 1)
                End If

                strreportoption = IIf(UCase(ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlOtherGroupCode.Items(ddlOtherGroupCode.SelectedIndex).Text, "") '"Sales"
                rtptype = "Sales"

             
                Dim strpop As String = ""
                strpop = "window.open('rptTrfJeepPriceListReport.aspx?pageame=rptTrfJeepPricelist&BackPageName=rptTrfsJeepPricelist.aspx&othergroupcode=" & strgrpcode & "&fromdate=" & strfromdate _
               & "&todate=" & strtodate & "&plgrpcode=" & strplgrpcode & "&approve=" & strapprove & "&sellcode=" & strsellcode _
               & "&rtptype=" & rtptype & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "&othcat=" & Server.UrlEncode(othcat) & "','RepOtherPL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptTrfsJeepPricelist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptTrfsJeepPricelist','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


End Class
