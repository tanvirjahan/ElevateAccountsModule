Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class VisitfollSearch
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

                If txtFromDate.Text = "" Then
                    txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If


                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPerson, "UserCode", "UserName", "select UserCode,UserName from UserMaster where active=1  order by UserCode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPersonName, "UserName", "UserCode", "select UserCode,UserName from UserMaster where active=1  order by UserName", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomercode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomername, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)

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

                If Request.QueryString("citycode") <> "" Then
                    ddlCityName.Value = Request.QueryString("citycode")
                    ddlCityCode.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text
                End If
                If Request.QueryString("ctrycode") <> "" Then
                    ddlcontName.Value = Request.QueryString("ctrycode")
                    ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text
                End If
                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
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

            ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSalesPerson.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSalesPersonName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcustomercode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcustomername.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        End If

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepNewClientWindowPostBack") Then
            BtnPrint_Click(sender, e)
        End If


    End Sub

    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack = True Then

            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True, ddlContCode.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True, ddlcontName.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1 and ctrycode='" + ddlcontName.Value + "' order by citycode", True, txtCityName.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  and ctrycode='" + ddlcontName.Value + "' order by cityname", True, txtCityCode.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomercode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True, txtcustname.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomername, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True, txtcustcode.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPerson, "UserCode", "UserName", "select UserCode,UserName from UserMaster where active=1  order by UserCode", True, txtsmanname.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesPersonName, "UserName", "UserCode", "select UserCode,UserName from UserMaster where active=1  order by UserName", True, txtsmancode.Value)

        End If
    End Sub

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlContCode.Value = "[Select]"
        ddlcontName.Value = "[Select]"
        ddlCityCode.Value = "[Select]"
        ddlCityName.Value = "[Select]"
        ddlSalesPerson.Value = "[Select]"
        ddlSalesPersonName.Value = "[Select]"
        ddlcustomercode.Value = "[Select]"
        ddlcustomername.Value = "[Select]"
        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtFromDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))

    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try

            If txtFromDate.Text = "" Then

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

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try
            If ValidatePage() = True Then
                'Session.Add("Pageame", "Newclient")
                'Session.Add("BackPageName", "NewclientSearch.aspx")
                'Session("ColReportParams") = Nothing
                Dim strReportTitle As String = ""

                Dim strcustomercode As String = ""
                Dim strcustomername As String = ""
                Dim strcitycode As String = ""
                Dim strcityname As String = ""
                Dim strctrycode As String = ""
                Dim strctryname As String = ""
                Dim strsmancode As String = ""
                Dim strsmanname As String = ""
                Dim strfromdate As String = ""
                Dim strtodate As String = ""


                Dim strrepfilter As String = ""


                strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
                strctryname = IIf(UCase(ddlcontName.Items(ddlcontName.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcontName.Items(ddlcontName.SelectedIndex).Text, "")
                strcitycode = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
                strcityname = IIf(UCase(ddlCityName.Items(ddlCityName.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityName.Items(ddlCityName.SelectedIndex).Text, "")
                strcustomercode = IIf(UCase(ddlcustomercode.Items(ddlcustomercode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustomercode.Items(ddlcustomercode.SelectedIndex).Text, "")
                strcustomername = IIf(UCase(ddlcustomername.Items(ddlcustomername.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustomername.Items(ddlcustomername.SelectedIndex).Text, "")
                strsmancode = IIf(UCase(ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSalesPerson.Items(ddlSalesPerson.SelectedIndex).Text, "")
                strsmanname = IIf(UCase(ddlSalesPersonName.Items(ddlSalesPersonName.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSalesPersonName.Items(ddlSalesPersonName.SelectedIndex).Text, "")

                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)
                If strfromdate <> "" And strtodate <> "" Then
                    strrepfilter = strrepfilter + " From Date :" + Mid(Format(CType(txtFromDate.Text, Date), "dd/MM/yyyy"), 1, 10)
                    strrepfilter = strrepfilter + " - " + " To Date :" + Mid(Format(CType(txtToDate.Text, Date), "dd/MM/yyyy"), 1, 10)
                End If

                strReportTitle = "Visit Follow Up"

                Dim strpop As String = ""
                strpop = "window.open('VisitfollReport.aspx?Pageame=Visitfoll&BackPageName=VisitfollSearch.aspx&smancode=" & strsmancode & "&smanname=" & strsmanname & "&citycode=" & strcitycode & "&cityname=" & strcityname & _
                 "&ctrycode=" & strctrycode & "&ctryname=" & strctryname & "&customercode=" & strcustomercode & "&customername=" & strcustomername & "&fromdate=" & strfromdate & "&todate=" & strtodate & _
                 "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "','RepNewClient','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("NewclientsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=VisitfollSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
