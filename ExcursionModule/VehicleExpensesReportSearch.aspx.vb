Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class NewclientsSearch
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
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction

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
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "ExcursionModule\VehicleExpensesReportSearch.aspx?appid=" + strappid, btnadd, Button1, BtnPrint, gv_SearchResult)
                End If
                txtconnection.Value = Session("dbconnectionName")

                If txtexpdtfrom.Text = "" Then
                    txtexpdtfrom.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If


                If txtexpdtto.Text = "" Then
                    txtexpdtto.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If



                 

                
                If Request.QueryString("fromdate") <> "" Then
                    txtexpdtfrom.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtexpdtto.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                

                'txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objutils.WritErrorLog("rptSupplierpoliciesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try



        End If

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            ddlvehfromcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlvehfromname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")




        End If

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepNewClientWindowPostBack") Then
            BtnPrint_Click(sender, e)
        End If


    End Sub

    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack = False Then
            ''sell type
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehfromcode, "vehiclecode", "vehiclename", "select vehiclecode,vehiclename from vehiclemaster where active=1   order by vehiclecode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehfromname, "vehiclename", "vehiclecode", "select vehiclecode,vehiclename from vehiclemaster where active=1 order by vehiclename", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehtocode, "vehiclecode", "vehiclename", "select vehiclecode,vehiclename from vehiclemaster where active=1   order by vehiclecode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlvehtoname, "vehiclename", "vehiclecode", "select vehiclecode,vehiclename from vehiclemaster where active=1   order by vehiclename", True)

            ''clients
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromclientcode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1   order by agentcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromclientname, "agentname", "agentcode", "select agentcode,agentname from agentmast where active=1   order by agentname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoclientcode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1   order by agentcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoclientname, "agentname", "agentcode", "select agentcode,agentname from agentmast where active=1   order by agentname", True)


            ''PARTY
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupfromcode, "partycode", "partyname", "select rtrim(ltrim(partycode)) partycode,rtrim(ltrim(partyname)) partyname from partymast where active=1 and partymast.sptypecode IN(select option_selected from reservation_parameters where param_id ='1001')   order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsupfromname, "partyname", "partycode", "select  rtrim(ltrim(partycode)) partycode,rtrim(ltrim(partyname)) partyname from partymast where active=1 and partymast.sptypecode  IN(select option_selected from reservation_parameters where param_id ='1001')  order by partyname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsuptocode, "partycode", "partyname", "select  rtrim(ltrim(partyname)) partyname,rtrim(ltrim(partycode)) partycode from partymast where active=1 and partymast.sptypecode  IN(select option_selected from reservation_parameters where param_id ='1001')  order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsuptoname, "partyname", "partycode", "select  rtrim(ltrim(partyname)) partyname,rtrim(ltrim(partycode)) partycode from partymast where active=1 and partymast.sptypecode  IN(select option_selected from reservation_parameters where param_id ='1001')  order by partyname", True)


            ''driver
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldriverfromcode, "drivercode", "drivername", "select drivercode,drivername from drivermaster where active=1   order by drivercode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldriverfromname, "drivername", "drivercode", "select drivercode,drivername from drivermaster where active=1   order by  drivername", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldrivertocode, "drivercode", "drivername", "select drivercode,drivername from drivermaster where active=1   order by drivercode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldrivertoname, "drivername", "drivercode", "select drivercode,drivername from drivermaster where active=1   order by drivername", True)

            ''cartype
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcarfromcode, "othcatcode", "othcatname", "select othcatcode,othcatname from othcatmast where othgrpcode in (select option_selected from reservation_parameters where param_id='1001')  order by othcatcode ", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcarfromname, "othcatname", "othcatcode", "select othcatcode,othcatname from othcatmast where othgrpcode in (select option_selected from reservation_parameters where param_id='1001')  order by othcatname ", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcartocode, "othcatcode", "othcatname", "select othcatcode,othcatname from othcatmast where othgrpcode in (select option_selected from reservation_parameters where param_id='1001')  order by othcatcode ", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcartoname, "othcatname", "othcatcode", "select othcatcode,othcatname from othcatmast where othgrpcode in (select option_selected from reservation_parameters where param_id='1001')  order by othcatcode ", True)

            ''exptype
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexpfromcode, "expensecode", "expensename", "select expensecode,expensename from vehicle_expense_master where vehicle_expense_master.active=1  order by expensecode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexpfromname, "expensename", "expensecode", "select expensecode,expensename from vehicle_expense_master where vehicle_expense_master.active=1  order by expensename", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexptocode, "expensecode", "expensename", "select expensecode,expensename from vehicle_expense_master where vehicle_expense_master.active=1  order by expensecode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexptoname, "expensename", "expensecode", "select expensecode,expensename from vehicle_expense_master where vehicle_expense_master.active=1  order by expensename", True)

            disableallcontrol()
            ddltrftype.SelectedIndex = 3
            ddlexptype.SelectedIndex = 3
            ''txtexpdtfrom.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
            ''txtexpdtto.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName")) 'DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
        Else
            If radvehall.Checked = True Then
                ddlvehfromcode.Value = "[Select]"
                ddlvehfromname.Value = "[Select]"
                ddlvehtocode.Value = "[Select]"
                ddlvehtoname.Value = "[Select]"
                ddlvehfromcode.Disabled = True
                ddlvehfromname.Disabled = True
                ddlvehtocode.Disabled = True
                ddlvehtoname.Disabled = True
            Else
                ddlvehfromcode.Disabled = False
                ddlvehfromname.Disabled = False
                ddlvehtocode.Disabled = False
                ddlvehtoname.Disabled = False

            End If

            If Radio1.Checked = True Then
                ddlfromclientcode.Value = "[Select]"
                ddlfromclientname.Value = "[Select]"
                ddltoclientcode.Value = "[Select]"
                ddltoclientname.Value = "[Select]"
                ddlfromclientcode.Disabled = True
                ddlfromclientname.Disabled = True
                ddltoclientcode.Disabled = True
                ddltoclientname.Disabled = True
            Else
                ddlfromclientcode.Disabled = False
                ddlfromclientname.Disabled = False
                ddltoclientcode.Disabled = False
                ddltoclientname.Disabled = False
            End If

            If radsupall.Checked = True Then
                ddlsupfromcode.Value = "[Select]"
                ddlsuptocode.Value = "[Select]"
                ddlsupfromname.Value = "[Select]"
                ddlsuptoname.Value = "[Select]"
                ddlsupfromcode.Disabled = True
                ddlsuptocode.Disabled = True
                ddlsupfromname.Disabled = True
                ddlsuptoname.Disabled = True
            Else
                ddlsupfromcode.Disabled = False
                ddlsuptocode.Disabled = False
                ddlsupfromname.Disabled = False
                ddlsuptoname.Disabled = False
            End If

            If raddriverall.Checked = True Then
                ddldriverfromcode.Value = "[Select]"
                ddldriverfromname.Value = "[Select]"
                ddldrivertocode.Value = "[Select]"
                ddldrivertoname.Value = "[Select]"
                ddldriverfromcode.Disabled = True
                ddldriverfromname.Disabled = True
                ddldrivertocode.Disabled = True
                ddldrivertoname.Disabled = True
            Else
                ddldriverfromcode.Disabled = False
                ddldriverfromname.Disabled = False
                ddldrivertocode.Disabled = False
                ddldrivertoname.Disabled = False
            End If
            If radcarall.Checked = True Then
                ddlcarfromcode.Value = "[Select]"
                ddlcarfromname.Value = "[Select]"
                ddlcartocode.Value = "[Select]"
                ddlcartoname.Value = "[Select]"
                ddlcarfromcode.Disabled = True
                ddlcarfromname.Disabled = True
                ddlcartocode.Disabled = True
                ddlcartoname.Disabled = True
            Else
                ddlcarfromcode.Disabled = False
                ddlcarfromname.Disabled = False
                ddlcartocode.Disabled = False
                ddlcartoname.Disabled = False
            End If
            If radexpall.Checked = True Then
                ddlexpfromcode.Value = "[Select]"
                ddlexpfromname.Value = "[Select]"
                ddlexptocode.Value = "[Select]"
                ddlexptoname.Value = "[Select]"
                ddlexpfromcode.Disabled = True
                ddlexpfromname.Disabled = True
                ddlexptocode.Disabled = True
                ddlexptoname.Disabled = True
            Else
                ddlexpfromcode.Disabled = False
                ddlexpfromname.Disabled = False
                ddlexptocode.Disabled = False
                ddlexptoname.Disabled = False
            End If

            If radreqall.Checked = True Then
                txtexpdtfrom.Enabled = False
                txtexpdtto.Enabled = False
            Else
                txtexpdtfrom.Enabled = True
                txtexpdtto.Enabled = True
            End If

        End If

        'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromflightcode, "flightcode", "partyname", "select flightcode,partyname from flightmast inner join partymast on flightmast.airlinecode =partymast.partycode  where flightmast.active=1  and partymast.sptypecode='AIR' order by partycode", True)



    End Sub

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click

        ddlvehfromcode.Value = "[Select]"
        ddlvehfromname.Value = "[Select]"
        ddlvehtocode.Value = "[Select]"
        ddlvehtoname.Value = "[Select]"
        ddlfromclientcode.Value = "[Select]"
        ddlfromclientname.Value = "[Select]"
        ddltoclientcode.Value = "[Select]"
        ddltoclientname.Value = "[Select]"
        ddlsupfromcode.Value = "[Select]"
        ddlsupfromname.Value = "[Select]"
        ddlsuptocode.Value = "[Select]"
        ddlsuptoname.Value = "[Select]"
        ddldriverfromcode.Value = "[Select]"
        ddldriverfromname.Value = "[Select]"
        ddldrivertocode.Value = "[Select]"
        ddldrivertoname.Value = "[Select]"
        ddlcarfromcode.Value = "[Select]"
        ddlcarfromname.Value = "[Select]"
        ddlcartocode.Value = "[Select]"
        ddlcartoname.Value = "[Select]"
        ddlexpfromcode.Value = "[Select]"
        ddlexpfromname.Value = "[Select]"
        ddlexptocode.Value = "[Select]"
        ddlexptoname.Value = "[Select]"

       
        disableallcontrol()
    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date

        Dim objDateTime As New clsDateTime
        Try

            If txtexpdtfrom.Text = "" Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)

                SetFocus(txtexpdtfrom)
                ValidatePage = False
                Exit Function
            End If






            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Private Function disableallcontrol()
        Radio1.Checked = True

        radsupall.Checked = True
        raddriverall.Checked = True
        radcarall.Checked = True
        radvehall.Checked = True
        ddlvehfromcode.Disabled = True
        ddlvehfromname.Disabled = True
        ddlvehtocode.Disabled = True
        ddlvehtoname.Disabled = True
        ddlfromclientcode.Disabled = True
        ddlfromclientname.Disabled = True
        ddltoclientcode.Disabled = True
        ddltoclientname.Disabled = True
        ddlsupfromcode.Disabled = True
        ddlsupfromname.Disabled = True
        ddlsuptocode.Disabled = True
        ddlsuptoname.Disabled = True
        ddldriverfromcode.Disabled = True
        ddldriverfromname.Disabled = True
        ddldrivertocode.Disabled = True
        ddldrivertoname.Disabled = True
        ddlcarfromcode.Disabled = True
        ddlcarfromname.Disabled = True
        ddlcartocode.Disabled = True
        ddlcartoname.Disabled = True
        ddlexpfromcode.Disabled = True
        ddlexpfromname.Disabled = True
        ddlexptocode.Disabled = True
        ddlexptoname.Disabled = True

        txtexpdtfrom.Enabled = False
        txtexpdtto.Enabled = False

        radcarrange.Checked = False
        radcarall.Checked = True
        raddriverrange.Checked = False
        raddriverall.Checked = True
        radexprange.Checked = False
        radexpall.Checked = True
        radreqrange.Checked = False
        radreqall.Checked = True
        radsuprange.Checked = False
        radsupall.Checked = True
        radvehrange.Checked = False
        radvehall.Checked = True

        ddltrftype.SelectedIndex = 3
        ddldet.SelectedIndex = 0
        ddlexptype.SelectedIndex = 3
    End Function

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If ValidatePage() = True Then
                'Session.Add("Pageame", "Newclient")
                'Session.Add("BackPageName", "NewclientSearch.aspx")
                'Session("ColReportParams") = Nothing

                Dim strReportTitle As String = ""
                Dim param1 As String = ""
                Dim param2 As String = ""
                Dim param3 As String = ""
                Dim param4 As String = ""
                Dim param5 As String = ""
                Dim param6 As String = ""

                Dim strfromdate As String = ""
                Dim strtodate As String = ""


                Dim strsellcodefrom As String = ""
                Dim strsellcodeto As String = ""
                Dim strclientfrom As String = ""
                Dim strclientto As String = ""
                Dim strpartyfrom As String = ""
                Dim strpartyto As String = ""
                Dim strairportfrom As String = ""
                Dim strairportto As String = ""
                Dim strairlinefrom As String = ""
                Dim strairlineto As String = ""
                Dim strflightfrom As String = ""
                Dim strflightto As String = ""
                Dim strgroupby As String = ""
                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""
                Dim strsectorcode As String = ""
                Dim dateall As String = ""

                Dim strarr(6) As String
                Dim k As Integer
                Dim P As Integer

                'selling code 
                strsellcodefrom = IIf(UCase(ddlvehfromcode.Items(ddlvehfromcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlvehfromcode.Items(ddlvehfromcode.SelectedIndex).Text, "")
                strsellcodeto = IIf(UCase(ddlvehtocode.Items(ddlvehtocode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlvehtocode.Items(ddlvehtocode.SelectedIndex).Text, "")

                'client 
                strclientfrom = IIf(UCase(ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text, "")
                strclientto = IIf(UCase(ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text, "")

                'party 
                strpartyfrom = IIf(UCase(ddlsupfromcode.Items(ddlsupfromcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsupfromcode.Items(ddlsupfromcode.SelectedIndex).Text, "")
                strpartyto = IIf(UCase(ddlsuptocode.Items(ddlsuptocode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuptocode.Items(ddlsuptocode.SelectedIndex).Text, "")

                'airport 
                strairportfrom = IIf(UCase(ddldriverfromcode.Items(ddldriverfromcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddldriverfromcode.Items(ddldriverfromcode.SelectedIndex).Text, "")
                strairportto = IIf(UCase(ddldrivertocode.Items(ddldrivertocode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddldrivertocode.Items(ddldrivertocode.SelectedIndex).Text, "")

                'airline
                strairlinefrom = IIf(UCase(ddlcarfromcode.Items(ddlcarfromcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcarfromcode.Items(ddlcarfromcode.SelectedIndex).Text, "")
                strairlineto = IIf(UCase(ddlcartocode.Items(ddlcartocode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcartocode.Items(ddlcartocode.SelectedIndex).Text, "")

                'flight
                strflightfrom = IIf(UCase(ddlexpfromcode.Items(ddlexpfromcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlexpfromcode.Items(ddlexpfromcode.SelectedIndex).Text, "")
                strflightto = IIf(UCase(ddlexptocode.Items(ddlexptocode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlexptocode.Items(ddlexptocode.SelectedIndex).Text, "")

                'strgroupby = mygroupresult()
                strfromdate = Mid(Format(CType(txtexpdtfrom.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtexpdtto.Text, Date), "u"), 1, 10)
                If radreqall.Checked = True Then
                    dateall = 0
                Else
                    dateall = 1
                End If


             
                    strReportTitle = " Vehicle Expense Report"
                    'find no user of this sp in rport
                    'percarexpense()
                    Dim strpop As String = ""
                '    strpop = "window.open('rptvehicleexpensereport1.aspx?Pageame=vehexpreport1&BackPageName=rptvehicleexpensereport1.aspx " _
                '& "&fromdate=" & strfromdate & "&todate=" & strtodate & "&dateall=" & dateall _
                '& "&sellcodefrom=" & strsellcodefrom & "&sellcodeto=" & strsellcodeto _
                '& "&strclientfrom=" & strclientfrom & "&strclientto=" & strclientto _
                '& "&strpartyfrom=" & strpartyfrom & "&strpartyto=" & strpartyto _
                '& "&strairportfrom=" & strairportfrom & "&strairportto=" & strairportto _
                '& "&strairlinefrom=" & strairlinefrom & "&strairlineto=" & strairlineto _
                '& "&strflightfrom=" & strflightfrom & "&strflightto=" & strflightto _
                '& "&strgroupby=" & strgroupby _
                '& "&exptype=" & ddlexptype.SelectedIndex _
                ' & "&rpttype=" & ddldet.SelectedIndex _
                '    & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                strpop = "window.open('rptvehicleexpensereport1.aspx?Pageame=vehexpreport1&BackPageName=rptvehicleexpensereport1.aspx " _
            & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&dateall=" & dateall _
            & "&sellcodefrom=" & strsellcodefrom & "&sellcodeto=" & strsellcodeto _
            & "&strclientfrom=" & strclientfrom & "&strclientto=" & strclientto _
            & "&strpartyfrom=" & strpartyfrom & "&strpartyto=" & strpartyto _
            & "&strairportfrom=" & strairportfrom & "&strairportto=" & strairportto _
            & "&strairlinefrom=" & strairlinefrom & "&strairlineto=" & strairlineto _
            & "&strflightfrom=" & strflightfrom & "&strflightto=" & strflightto _
            & "&strgroupby=" & strgroupby _
            & "&exptype=" & ddlexptype.SelectedIndex _
             & "&rpttype=" & ddldet.SelectedIndex _
                & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient' );"














                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptTransfercostPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid()
        Dim myDS As New DataSet
        Dim MyCommand As SqlCommand
        Dim SqlConn1 As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim ObjDate As New clsDateTime
        Dim strSqlQry As String

        lblMsg.Visible = False

        
        Try

           

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("NewclientsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    Protected Sub btndisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndisplay.Click

    End Sub

    Private Sub percarexpense()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start


            mySqlCmd = New SqlCommand("sp_rep_percar_Expense", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@alldate", SqlDbType.Int)).Value = IIf(radreqall.Checked = True, 0, 1)
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Format(txtexpdtfrom.Text, "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Format(txtexpdtto.Text, "yyyy/MM/dd")

            mySqlCmd.Parameters.Add(New SqlParameter("@driverfrom", SqlDbType.VarChar, 20)).Value = ddldriverfromcode.Items(ddldriverfromcode.SelectedIndex).Text
            mySqlCmd.Parameters.Add(New SqlParameter("@driverto", SqlDbType.VarChar, 20)).Value = ddldrivertocode.Items(ddldrivertocode.SelectedIndex).Text

            mySqlCmd.Parameters.Add(New SqlParameter("@vehiclefrom", SqlDbType.VarChar, 20)).Value = ddlvehfromcode.Items(ddlvehfromcode.SelectedIndex).Text
            mySqlCmd.Parameters.Add(New SqlParameter("@vehicleto", SqlDbType.VarChar, 20)).Value = ddlvehtocode.Items(ddlvehtocode.SelectedIndex).Text

            mySqlCmd.Parameters.Add(New SqlParameter("@exptype", SqlDbType.Int)).Value = ddlexptype.SelectedIndex

            mySqlCmd.Parameters.Add(New SqlParameter("@agentfrom", SqlDbType.VarChar, 20)).Value = ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text
            mySqlCmd.Parameters.Add(New SqlParameter("@agentto", SqlDbType.VarChar, 20)).Value = ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text

            mySqlCmd.Parameters.Add(New SqlParameter("@expfrom", SqlDbType.VarChar, 20)).Value = ddlexpfromcode.Items(ddlexpfromcode.SelectedIndex).Text
            mySqlCmd.Parameters.Add(New SqlParameter("@expto", SqlDbType.VarChar, 20)).Value = ddlexptocode.Items(ddlexptocode.SelectedIndex).Text

            mySqlCmd.Parameters.Add(New SqlParameter("@cartypefrom", SqlDbType.VarChar, 20)).Value = ddlcarfromcode.Items(ddlcarfromcode.SelectedIndex).Text
            mySqlCmd.Parameters.Add(New SqlParameter("@cartypeto", SqlDbType.VarChar, 20)).Value = ddlcartocode.Items(ddlcartocode.SelectedIndex).Text

            mySqlCmd.Parameters.Add(New SqlParameter("@trftype", SqlDbType.Int)).Value = ddltrftype.Items(ddltrftype.SelectedIndex).Text


            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            'Response.Redirect("MarketsSearch.aspx", False)
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('NationalityWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
        End Try

    End Sub

    Private Function mygroupresult()

        Try
            Select Case ddltrftype.Items(ddltrftype.SelectedIndex).Text
                Case "Flight No."
                    mygroupresult = "F"
                Case "Hotel "
                    mygroupresult = "P"
                Case "Clients "
                    mygroupresult = "C"
                Case "Arrival Time "
                    mygroupresult = "T"
                Case Else
                    mygroupresult = "F"
            End Select
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


    End Function
    



#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        'Dim DataTable As DataTable
     
    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=NewclientsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

   
   
    
End Class
