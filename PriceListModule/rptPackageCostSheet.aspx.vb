Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptPackageCostSheet
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

                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If

                txtconnection.Value = Session("dbconnectionName")

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                If Request.QueryString("packageid") <> "" Then
                    txtpackage.Text = Request.QueryString("packageid")
                End If

                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If

                If Request.QueryString("plgrpcode") <> "" Then
                    ddlMarketName.Value = Request.QueryString("plgrpcode")
                    ddlMarketCode.Value = ddlMarketName.Items(ddlMarketName.SelectedIndex).Text
                End If
                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepPkgCSWindowPostBack") Then
                    BtnPrint_Click(sender, e)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptPackageCostSheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptPackageCostSheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        '    rep.Close()
        '    rep.Dispose()
        'End If
    End Sub

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"

        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))

        txtpackage.Text = ""
    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try

            If ddlMarketCode.Value = "" Or UCase(Trim(ddlMarketCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Market can not be blank..');", True)

                SetFocus(ddlMarketCode)
                ValidatePage = False
                Exit Function
            End If

            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank');", True)

                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('As on  date field can not be blank');", True)
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
                'Session.Add("Pageame", "rptPackageCostSheet")
                'Session.Add("BackPageName", "rptPackageCostSheet.aspx")

                Dim strReportTitle As String = ""

                Dim strfromdate As String = ""
                Dim strtodate As String = ""
                Dim strplgrpcode As String = ""
                Dim strpackageid As String = ""
                Dim strreportoption As String = ""
                Dim strapprove As Integer
                Dim strrepfilter As String = ""

                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)

                strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")
                strpackageid = txtpackage.Text.Trim
                strreportoption = IIf(rbBrief.Checked = True, "0", "1")
                strapprove = CType(ddlapprovestatus.SelectedValue, String)

                strReportTitle = "Package Cost Sheet"

                'Session.Add("fromdate", strfromdate)
                'Session.Add("todate", strtodate)
                'Session.Add("plgrpcode", strplgrpcode)
                'Session.Add("packageid", strpackageid)
                'Session.Add("reportoption", strreportoption)

                'Session.Add("repfilter", strrepfilter)


                'Session.Add("ReportTitle", strReportTitle)

                'Response.Redirect("rptPackageCostSheetReport.aspx?fromdate=" & strfromdate _
                '& "&todate=" & strtodate & "&plgrpcode=" & strplgrpcode & "&packageid=" & strpackageid _
                '& "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)
                Dim strpop As String = ""
                strpop = "window.open('rptPackageCostSheetReport.aspx?Pageame=rptPackageCostSheet&BackPageName=rptPackageCostSheet.aspx&fromdate=" & strfromdate _
                & "&todate=" & strtodate & "&plgrpcode=" & strplgrpcode & "&approve=" & strapprove & "&packageid=" & strpackageid _
                & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepPkgCostSheet','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptPackageCostSheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=rptPackageCostSheet','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
