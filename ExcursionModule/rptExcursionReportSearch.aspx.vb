Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports System.IO

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
    Dim exctypecol As Collection
    Dim priidcost As Integer = 25
    Dim priidcomi As Integer = 50
    Dim showcostp As String = ""
    Dim showcommip As String = ""
    Dim excursionno As String = ""
    Dim objEmail As New clsEmail
    Dim objUser As New clsUser

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
                                                       CType(strappname, String), "ExcursionModule\rptExcursionReportSearch.aspx?appid=" + strappid, btnadd, Button1, BtnPrint, gv_SearchResult)
                End If
                txtconnection.Value = Session("dbconnectionName")



                If txtreqfrom.Text = "" Then
                    txtreqfrom.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If


                If txtreqto.Text = "" Then
                    txtreqto.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName")) 'DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If

                If txttourfrom.Text = "" Then
                    txttourfrom.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If

                If txttourto.Text = "" Then
                    txttourto.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                End If
              
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsellcode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode  not in (select option_selected from reservation_parameters where param_id=458)  order by partycode", True)
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsellname, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and sptypecode  not in (select option_selected from reservation_parameters where param_id=458) order by partyname", True)

                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlairportcode, "airportbordercode", "airportbordername", "select airportbordercode,airportbordername from airportbordersmaster where active=1  order by airportbordercode", True)
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlairportname, "airportbordername", "airportbordercode", "select airportbordername,airportbordercode from airportbordersmaster where active=1  order by airportbordername", True)

                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsectorcode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast where active=1  and othgrpcode in (select option_selected from reservation_parameters where param_id ='1001')order by othtypcode", True)
                'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlsectorname, "othtypname", "othtypcode", "select othtypcode,othtypname from othtypmast where active=1  and othgrpcode in (select option_selected from reservation_parameters where param_id ='1001')order by othtypcode", True)

                

              
                ' '''''''''''''''

                'If Request.QueryString("catcode") <> "" Then
                '    ddlsellname.Value = Request.QueryString("catcode")
                '    ddlsellcode.Value = ddlsellname.Items(ddlsellname.SelectedIndex).Text
                'End If
               
               
                
                'If Request.QueryString("fromdate") <> "" Then
                '    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                'End If
                'If Request.QueryString("todate") <> "" Then
                '    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                'End If

                'rbexctypeall.Attributes.Add("onclick", "javascript:loadgridexc();")
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


            'ddlFromsellcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            'ddlFromSellName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

         
          

        End If

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepNewClientWindowPostBack") Then
            BtnPrint_Click(sender, e)
        End If


    End Sub

    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack = False Then

            chktrfreq.Checked = False

            ''Main Group
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFrommaingpcode, "othmaingrpcode", "othmaingrpname", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1  order by othmaingrpcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFrommaingpName, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1  order by othmaingrpname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTomaingpcode, "othmaingrpcode", "othmaingrpname", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1  order by othmaingrpcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTomaingpname, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1  order by othmaingrpname", True)


            ''Excursion Group
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromexcgpcode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in('1104','1105'))  order by othgrpcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromexcgpname, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in('1104','1105'))  order by othgrpname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoexcgpcode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in('1104','1105'))  order by othgrpcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoexcgpname, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in('1104','1105'))  order by othgrpname", True)

            ''Market
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfrommarketcode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1   order by plgrpcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfrommarketname, "plgrpname", "plgrpcode", "select plgrpcode,plgrpname from plgrpmast where active=1   order by plgrpname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltomarketcode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1   order by plgrpcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltomarketname, "plgrpname", "plgrpcode", "select plgrpcode,plgrpname from plgrpmast where active=1   order by plgrpname", True)

            ''City
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromcitycode, "citycode", "cityname", "select citycode,cityname from citymast where active=1   order by citycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromcityname, "cityname", "citycode", "select citycode,cityname from citymast where active=1   order by cityname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltocitycode, "citycode", "cityname", "select citycode,cityname from citymast where active=1   order by citycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltocityname, "cityname", "citycode", "select citycode,cityname from citymast where active=1   order by cityname", True)

            ''Excursion Provider
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromexcprovider, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partymast.sptypecode in (select option_selected from reservation_parameters where param_id ='1033')   order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromexcprovidername, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode in (select option_selected from reservation_parameters where param_id ='1033')   order by partyname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoexcprovider, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partymast.sptypecode in (select option_selected from reservation_parameters where param_id ='1033')   order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoexcprovidername, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode in (select option_selected from reservation_parameters where param_id ='1033')   order by partyname", True)


            ''HOTEL
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromhotelcode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partymast.sptypecode='HOT'   order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromhotelname, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode='HOT'  order by partyname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltohotelcode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partymast.sptypecode='HOT'  order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltohotelname, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode='HOT'  order by partyname", True)

            'clients
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromclientcode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1   order by agentcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromclientname, "agentname", "agentcode", "select agentcode,agentname from agentmast where active=1   order by agentname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoclientcode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1   order by agentcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoclientname, "agentname", "agentcode", "select agentcode,agentname from agentmast where active=1   order by agentname", True)

            'Payment Terms
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfrompaytermscode, "paycode", "payname", "select paycode,payname from paymentmodemaster  order by paycode ", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfrompaytermsname, "payname", "paycode", "select paycode,payname from paymentmodemaster  order by payname ", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltopaytermscode, "paycode", "payname", "select paycode,payname from paymentmodemaster  order by paycode ", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltopaytermsname, "payname", "paycode", "select paycode,payname from paymentmodemaster  order by payname ", True)

            ''PARTY
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromsp, "partycode", "partyname", "select partycode,partyname from partymast where active=1   order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromspname, "partyname", "partycode", "select partycode,partyname from partymast where active=1   order by partyname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltosp, "partycode", "partyname", "select partycode,partyname from partymast where active=1   order by partycode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltospname, "partyname", "partycode", "select partycode,partyname from partymast where active=1   order by partyname", True)

            ''DRIVER
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromdcode, "drivercode", "drivername", "select drivercode,drivername from drivermaster where active=1   order by drivercode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromdname, "drivername", "drivercode", "select drivercode,drivername from drivermaster where active=1   order by drivername", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltodcode, "drivercode", "drivername", "select drivercode,drivername from drivermaster where active=1   order by drivercode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltodname, "drivername", "drivercode", "select drivercode,drivername from drivermaster where active=1   order by drivername", True)

            ''office sales
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromsalescode, "spersoncode", "spersonname", "select spersoncode,spersonname from spersonmast_office   order by spersoncode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromsalesname, "spersonname", "spersoncode", "select spersoncode,spersonname from spersonmast_office    order by spersonname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltosalescode, "spersoncode", "spersonname", "select spersoncode,spersonname from spersonmast_office    order by spersoncode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltosalesname, "spersonname", "spersoncode", "select spersoncode,spersonname from spersonmast_office    order by spersonname", True)


            ''SALES PERSON
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromsapcode, "spersoncode", "spersonname", "select spersoncode,spersonname from spersonmast order by spersoncode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromsapname, "spersonname", "spersoncode", "select spersoncode,spersonname from spersonmast    order by spersonname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltosapcode, "spersoncode", "spersonname", "select spersoncode,spersonname from spersonmast    order by spersoncode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltosapname, "spersonname", "spersoncode", "select spersoncode,spersonname from spersonmast   order by spersonname", True)


            ''COLLECTED BY 
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromcolcode, "spersoncode", "spersonname", "select spersoncode,spersonname from spersonmast order by spersoncode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromcolname, "spersonname", "spersoncode", "select spersoncode,spersonname from spersonmast    order by spersonname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltocolcode, "spersoncode", "spersonname", "select spersoncode,spersonname from spersonmast    order by spersoncode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltocolname, "spersonname", "spersoncode", "select spersoncode,spersonname from spersonmast   order by spersonname", True)

            ''OPERATOR 

            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromoperatorname, "agentname", "agentcode", "Select ltrim(rtrim(agentcode))agentcode,ltrim(rtrim(agentname))agentname from agentmast where active=1 and agentcode not in(select agentcode from agents_locked)  order by agentname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltooperatorname, "agentname", "agentcode", "Select ltrim(rtrim(agentcode))agentcode,ltrim(rtrim(agentname))agentname from agentmast where active=1 and agentcode not in(select agentcode from agents_locked)  order by agentname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromoperatorcode, "agentcode", "agentname", "Select ltrim(rtrim(agentcode))agentcode,ltrim(rtrim(agentname))agentname from agentmast where active=1 and agentcode not in(select agentcode from agents_locked)  order by agentcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltooperatorcode, "agentcode", "agentname", "Select ltrim(rtrim(agentcode))agentcode,ltrim(rtrim(agentname))agentname from agentmast where active=1 and agentcode not in(select agentcode from agents_locked)  order by agentcode", True)


            ''tourguide 

            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromtgname, "guidename", "guidecode", "Select ltrim(rtrim(guidecode))guidecode,ltrim(rtrim(guidename))guidename from guide_master where active=1  order by guidename", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltotgname, "guidename", "guidecode", "Select ltrim(rtrim(guidecode))guidecode,ltrim(rtrim(guidename))guidename from guide_master where active=1   order by guidename", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromtgcode, "guidecode", "guidename", "Select ltrim(rtrim(guidecode))guidecode,ltrim(rtrim(guidename))guidename from guide_master where active=1  order by guidecode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltotgcode, "guidecode", "guidename", "Select ltrim(rtrim(guidecode))guidecode,ltrim(rtrim(guidename))guidename from guide_master where active=1   order by guidecode", True)


            ''selling rate 
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromsellcode, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1 order by excsellcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromsellname, "excsellname", "excsellcode", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltosellcode, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1 order by excsellcode", True)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltosellname, "excsellname", "excsellcode", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)


            disableallcontrol()
            btnSave.Visible = False
            btnCancel.Visible = False
            chkcommission.Checked = False
            chkcost.Checked = False
            If rbexctypeall.Checked = True Then
                txtexctype.Style.Add("display", "none")
                btnloadgrid.Style.Add("display", "none")
            End If
            'loadgridexc

        Else
            If rbexctypeall.Checked = False Then
                txtexctype.Style.Add("display", "block")
                btnloadgrid.Style.Add("display", "block")
            End If

            If radcityall.Checked = True Then
                ddlfromcitycode.Value = "[Select]"
                ddlfromcityname.Value = "[Select]"
                ddltocitycode.Value = "[Select]"
                ddltocityname.Value = "[Select]"
                ddlfromcitycode.Disabled = True
                ddlfromcityname.Disabled = True
                ddltocitycode.Disabled = True
                ddltocityname.Disabled = True

            Else
                ddlfromcitycode.Disabled = False
                ddlfromcityname.Disabled = False
                ddltocitycode.Disabled = False
                ddltocityname.Disabled = False
            End If

            If rbmaingpall.Checked = True Then
                ddlFrommaingpcode.Value = "[Select]"
                ddlFrommaingpName.Value = "[Select]"
                ddlTomaingpcode.Value = "[Select]"
                ddlTomaingpname.Value = "[Select]"

                ddlFrommaingpcode.Disabled = True
                ddlFrommaingpName.Disabled = True
                ddlTomaingpcode.Disabled = True
                ddlTomaingpname.Disabled = True
            Else
                ddlFrommaingpcode.Disabled = False
                ddlFrommaingpName.Disabled = False
                ddlTomaingpcode.Disabled = False
                ddlTomaingpname.Disabled = False
            End If

            If rbexcgpall.Checked = True Then
                ddlfromexcgpcode.Value = "[Select]"
                ddlfromexcgpname.Value = "[Select]"
                ddltoexcgpcode.Value = "[Select]"
                ddltoexcgpname.Value = "[Select]"
                ddlfromexcgpcode.Disabled = True
                ddlfromexcgpname.Disabled = True
                ddltoexcgpcode.Disabled = True
                ddltoexcgpname.Disabled = True

            Else
                ddlfromexcgpcode.Disabled = False
                ddlfromexcgpname.Disabled = False
                ddltoexcgpcode.Disabled = False
                ddltoexcgpname.Disabled = False

            End If

            If radmarketall.Checked = True Then
                ddlfrommarketcode.Value = "[Select]"

                ddlfrommarketname.Value = "[Select]"
                ddltomarketcode.Value = "[Select]"
                ddltomarketname.Value = "[Select]"
                ddlfrommarketcode.Disabled = True

                ddlfrommarketname.Disabled = True
                ddltomarketcode.Disabled = True
                ddltomarketname.Disabled = True

            Else
                ddlfrommarketcode.Disabled = False

                ddlfrommarketname.Disabled = False
                ddltomarketcode.Disabled = False
                ddltomarketname.Disabled = False

            End If

            If radcityall.Checked = True Then
                ddlfromcitycode.Value = "[Select]"
                ddlfromcityname.Value = "[Select]"
                ddltocitycode.Value = "[Select]"
                ddltocityname.Value = "[Select]"
                ddlfromcitycode.Disabled = True
                ddlfromcityname.Disabled = True
                ddltocitycode.Disabled = True
                ddltocityname.Disabled = True
            Else

                ddlfromcitycode.Disabled = False
                ddlfromcityname.Disabled = False
                ddltocitycode.Disabled = False
                ddltocityname.Disabled = False
            End If

            If radexcproviderall.Checked = True Then
                ddlfromexcprovider.Value = "[Select]"
                ddlfromexcprovidername.Value = "[Select]"
                ddltoexcprovider.Value = "[Select]"
                ddltoexcprovidername.Value = "[Select]"
                ddlfromexcprovider.Disabled = True
                ddlfromexcprovidername.Disabled = True
                ddltoexcprovider.Disabled = True
                ddltoexcprovidername.Disabled = True
            Else
                ddlfromexcprovider.Disabled = False
                ddlfromexcprovidername.Disabled = False
                ddltoexcprovider.Disabled = False
                ddltoexcprovidername.Disabled = False
            End If

            If radhotelall.Checked = True Then
                ddlfromhotelcode.Value = "[Select]"
                ddlfromhotelname.Value = "[Select]"
                ddltohotelcode.Value = "[Select]"
                ddltohotelname.Value = "[Select]"
                ddlfromhotelcode.Disabled = True
                ddlfromhotelname.Disabled = True
                ddltohotelcode.Disabled = True
                ddltohotelname.Disabled = True

            Else
                ddlfromhotelcode.Disabled = False
                ddlfromhotelname.Disabled = False
                ddltohotelcode.Disabled = False
                ddltohotelname.Disabled = False

            End If


            If radclientall.Checked = True Then
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

            If radpaytermsall.Checked = True Then
                ddlfrompaytermscode.Value = "[Select]"
                ddlfrompaytermsname.Value = "[Select]"
                ddltopaytermscode.Value = "[Select]"
                ddltopaytermsname.Value = "[Select]"
                ddlfrompaytermscode.Disabled = True
                ddlfrompaytermsname.Disabled = True
                ddltopaytermscode.Disabled = True
                ddltopaytermsname.Disabled = True
            Else
                ddlfrompaytermscode.Disabled = False
                ddlfrompaytermsname.Disabled = False
                ddltopaytermscode.Disabled = False
                ddltopaytermsname.Disabled = False
            End If

            If radspall.Checked = True Then
                ddlfromsp.Value = "[Select]"
                ddlfromspname.Value = "[Select]"

                ddltosp.Value = "[Select]"
                ddltospname.Value = "[Select]"
                ddlfromsp.Disabled = True
                ddlfromspname.Disabled = True

                ddltosp.Disabled = True
                ddltospname.Disabled = True

            Else
                ddlfromsp.Disabled = False
                ddlfromspname.Disabled = False

                ddltosp.Disabled = False
                ddltospname.Disabled = False

            End If

            If raddall.Checked = True Then
                ddlfromdcode.Value = "[Select]"
                ddlfromdname.Value = "[Select]"
                ddltodcode.Value = "[Select]"
                ddltodname.Value = "[Select]"
                ddlfromdcode.Disabled = True
                ddlfromdname.Disabled = True
                ddltodcode.Disabled = True
                ddltodname.Disabled = True
            Else
                ddlfromdcode.Disabled = False
                ddlfromdname.Disabled = False
                ddltodcode.Disabled = False
                ddltodname.Disabled = False
            End If

            If radsalesall.Checked = True Then
                ddlfromsalescode.Value = "[Select]"
                ddlfromsalesname.Value = "[Select]"
                ddltosalescode.Value = "[Select]"
                ddltosalesname.Value = "[Select]"
                ddlfromsalescode.Disabled = True
                ddlfromsalesname.Disabled = True
                ddltosalescode.Disabled = True
                ddltosalesname.Disabled = True
            Else
                ddlfromsalescode.Disabled = False
                ddlfromsalesname.Disabled = False
                ddltosalescode.Disabled = False
                ddltosalesname.Disabled = False
            End If

            If radsapall.Checked = True Then
                ddlfromsapcode.Value = "[Select]"
                ddlfromsapname.Value = "[Select]"
                ddltosapcode.Value = "[Select]"
                ddltosapname.Value = "[Select]"
                ddlfromsapcode.Disabled = True
                ddlfromsapname.Disabled = True
                ddltosapcode.Disabled = True
                ddltosapname.Disabled = True

            Else
                ddlfromsapcode.Disabled = False
                ddlfromsapname.Disabled = False
                ddltosapcode.Disabled = False
                ddltosapname.Disabled = False


            End If

            If radcolall.Checked = True Then
                ddlfromcolcode.Value = "[Select]"
                ddlfromcolname.Value = "[Select]"
                ddltocolcode.Value = "[Select]"
                ddltocolname.Value = "[Select]"
                ddlfromcolcode.Disabled = True
                ddlfromcolname.Disabled = True
                ddltocolcode.Disabled = True
                ddltocolname.Disabled = True
            Else
                ddlfromcolcode.Disabled = False
                ddlfromcolname.Disabled = False
                ddltocolcode.Disabled = False
                ddltocolname.Disabled = False
            End If

            If radoperatorall.Checked = True Then
                ddlfromoperatorcode.Value = "[Select]"
                ddlfromoperatorname.Value = "[Select]"
                ddltooperatorcode.Value = "[Select]"
                ddltooperatorname.Value = "[Select]"
                ddlfromoperatorcode.Disabled = True
                ddlfromoperatorname.Disabled = True
                ddltooperatorcode.Disabled = True
                ddltooperatorname.Disabled = True

            Else
                ddlfromoperatorcode.Disabled = False
                ddlfromoperatorname.Disabled = False
                ddltooperatorcode.Disabled = False
                ddltooperatorname.Disabled = False


            End If

            If rbtgall.Checked = True Then
                ddlfromtgcode.Value = "[Select]"
                ddlfromtgname.Value = "[Select]"
                ddltotgcode.Value = "[Select]"
                ddltotgname.Value = "[Select]"
                ddlfromtgcode.Disabled = True
                ddlfromtgname.Disabled = True
                ddltotgcode.Disabled = True
                ddltotgname.Disabled = True

            Else
                ddlfromtgcode.Disabled = False
                ddlfromtgname.Disabled = False
                ddltotgcode.Disabled = False
                ddltotgname.Disabled = False


            End If

            If rbsellall.Checked = True Then
                ddlfromsellcode.Value = "[Select]"
                ddlfromsellname.Value = "[Select]"
                ddltosellcode.Value = "[Select]"
                ddltosellname.Value = "[Select]"
                ddlfromsellcode.Disabled = True
                ddlfromsellname.Disabled = True
                ddltosellcode.Disabled = True
                ddltosellname.Disabled = True

            Else
                ddlfromsellcode.Disabled = False
                ddlfromsellname.Disabled = False
                ddltosellcode.Disabled = False
                ddltosellname.Disabled = False


            End If

        End If

        'objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlfromflightcode, "flightcode", "partyname", "select flightcode,partyname from flightmast inner join partymast on flightmast.airlinecode =partymast.partycode  where flightmast.active=1  and partymast.sptypecode='AIR' order by partycode", True)



    End Sub

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlFrommaingpcode.Value = "[Select]"
        ddlFrommaingpName.Value = "[Select]"
        ddlTomaingpcode.Value = "[Select]"
        ddlTomaingpname.Value = "[Select]"

        ddlfromexcgpcode.Value = "[Select]"
        ddlfromexcgpname.Value = "[Select]"
        ddltoexcgpcode.Value = "[Select]"
        ddltoexcgpname.Value = "[Select]"

        ddlfrommarketcode.Value = "[Select]"

        ddlfrommarketname.Value = "[Select]"
        ddltomarketcode.Value = "[Select]"
        ddltomarketname.Value = "[Select]"

        ddlfromcitycode.Value = "[Select]"
        ddlfromcityname.Value = "[Select]"
        ddltocitycode.Value = "[Select]"
        ddltocityname.Value = "[Select]"

        ddlfromexcprovider.Value = "[Select]"
        ddlfromexcprovidername.Value = "[Select]"
        ddltoexcprovider.Value = "[Select]"
        ddltoexcprovidername.Value = "[Select]"

        ddlfromhotelcode.Value = "[Select]"
        ddlfromhotelname.Value = "[Select]"
        ddltohotelcode.Value = "[Select]"
        ddltohotelname.Value = "[Select]"

        ddlfromclientcode.Value = "[Select]"
        ddlfromclientname.Value = "[Select]"
        ddltoclientcode.Value = "[Select]"
        ddltoclientname.Value = "[Select]"

        ddlfrompaytermscode.Value = "[Select]"
        ddlfrompaytermsname.Value = "[Select]"
        ddltopaytermscode.Value = "[Select]"
        ddltopaytermsname.Value = "[Select]"

        ddlfromsp.Value = "[Select]"
        ddlfromspname.Value = "[Select]"

        ddltosp.Value = "[Select]"
        ddltospname.Value = "[Select]"

        ddlfromdcode.Value = "[Select]"
        ddlfromdname.Value = "[Select]"
        ddltodcode.Value = "[Select]"
        ddltodname.Value = "[Select]"

        ddlfromsalescode.Value = "[Select]"
        ddlfromsalesname.Value = "[Select]"
        ddltosalescode.Value = "[Select]"
        ddltosalesname.Value = "[Select]"

        ddlfromsapcode.Value = "[Select]"
        ddlfromsapname.Value = "[Select]"
        ddltosapcode.Value = "[Select]"
        ddltosapname.Value = "[Select]"


        ddlfromcolcode.Value = "[Select]"
        ddlfromcolname.Value = "[Select]"
        ddltocolcode.Value = "[Select]"
        ddltocolname.Value = "[Select]"

        ddlfromoperatorcode.Value = "[Select]"
        ddlfromoperatorname.Value = "[Select]"
        ddltooperatorcode.Value = "[Select]"
        ddltooperatorname.Value = "[Select]"

        ddlfromtgcode.Value = "[Select]"
        ddlfromtgname.Value = "[Select]"
        ddltotgcode.Value = "[Select]"
        ddltotgname.Value = "[Select]"

        ddlfromsellcode.Value = "[Select]"
        ddlfromsellname.Value = "[Select]"
        ddltosellcode.Value = "[Select]"
        ddltosellname.Value = "[Select]"

        txtexctype.Text = ""
        txtemail.Text = ""
        
        disableallcontrol()
    End Sub

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date

        Dim objDateTime As New clsDateTime
        Try

            'If txtFromDate.Text = "" Then

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)

            '    SetFocus(txtFromDate)
            '    ValidatePage = False
            '    Exit Function
            'End If






            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Private Function disableallcontrol()
        
        ''Main Group
        ddlFrommaingpcode.Disabled = True
        ddlFrommaingpName.Disabled = True
        ddlTomaingpcode.Disabled = True
        ddlTomaingpname.Disabled = True

        ddlfromexcgpcode.Disabled = True
        ddlfromexcgpname.Disabled = True
        ddltoexcgpcode.Disabled = True
        ddltoexcgpname.Disabled = True

        ddlfrommarketcode.Disabled = True

        ddlfrommarketname.Disabled = True
        ddltomarketcode.Disabled = True
        ddltomarketname.Disabled = True
        ''city
        ddlfromcitycode.Disabled = True
        ddlfromcityname.Disabled = True
        ddltocitycode.Disabled = True
        ddltocityname.Disabled = True

        ddlfromexcprovider.Disabled = True
        ddlfromexcprovidername.Disabled = True
        ddltoexcprovider.Disabled = True
        ddltoexcprovidername.Disabled = True
        ''hotel
        ddlfromhotelcode.Disabled = True
        ddlfromhotelname.Disabled = True
        ddltohotelcode.Disabled = True
        ddltohotelname.Disabled = True

        ddlfromclientcode.Disabled = True
        ddlfromclientname.Disabled = True
        ddltoclientcode.Disabled = True
        ddltoclientname.Disabled = True

        ddlfrompaytermscode.Disabled = True
        ddlfrompaytermsname.Disabled = True
        ddltopaytermscode.Disabled = True
        ddltopaytermsname.Disabled = True

        ddlfromsp.Disabled = True
        ddlfromspname.Disabled = True

        ddltosp.Disabled = True
        ddltospname.Disabled = True
       
        ddlfromdcode.Disabled = True
        ddlfromdname.Disabled = True
        ddltodcode.Disabled = True
        ddltodname.Disabled = True

        ddlfromsalescode.Disabled = True
        ddlfromsalesname.Disabled = True
        ddltosalescode.Disabled = True
        ddltosalesname.Disabled = True

        ddlfromsapcode.Disabled = True
        ddlfromsapname.Disabled =True 
        ddltosapcode.Disabled = True
        ddltosapname.Disabled = True

        ddlfromoperatorcode.Disabled = True
        ddltooperatorcode.Disabled = True
        ddlfromoperatorname.Disabled = True
        ddltooperatorname.Disabled = True

        ddlfromtgcode.Disabled = True
        ddltotgcode.Disabled = True
        ddlfromtgname.Disabled = True
        ddltotgname.Disabled = True

        ddlfromsellcode.Disabled = True
        ddltosellcode.Disabled = True
        ddlfromsellname.Disabled = True
        ddltosellname.Disabled = True


        ddlfromcolcode.Disabled = True
        ddlfromcolname.Disabled = True
        ddltocolcode.Disabled = True
        ddltocolname.Disabled = True

        txtreqfrom.Enabled = False
        txtreqto.Enabled = False
        txttourfrom.Enabled = False
        txttourto.Enabled = False


        radcityrange.Checked = False
        radclientrange.Checked = False
        radcolrange.Checked = False
        radexcproviderto.Checked = False
        radhotelrange.Checked = False
        radmarketrange.Checked = False
        radpaytermsrange.Checked = False
        radreqrange.Checked = False
        radsalesrange.Checked = False
        radsaprange.Checked = False
        radtourrange.Checked = False
        raddrange.Checked = False
        rbmaingprange.Checked = False
        rbexcgprange.Checked = False
        rbexctyperange.Checked = False
        radsprange.Checked = False

        rbmaingpall.Checked = True
        rbexcgpall.Checked = True
        rbexctypeall.Checked = True
        radcityall.Checked = True
        radhotelall.Checked = True
        radclientall.Checked = True
        radexcproviderall.Checked = True
        radcolall.Checked = True

        raddall.Checked = True
        radspall.Checked = True
        radmarketall.Checked = True
        radpaytermsall.Checked = True
        radreqall.Checked = True
        radsalesall.Checked = True
        radsapall.Checked = True
        radtourall.Checked = True

        radoperatorall.Checked = True
        radoperatorrange.Checked = False

        rbtgall.Checked = True
        rbtgrange.Checked = False

        rbsellall.Checked = True
        rbsellrange.Checked = False


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




                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""
                Dim strsectorcode As String = ""
                Dim strarr(6) As String
                Dim k As Integer
                Dim P As Integer
                Dim strmaingpcodefrom As String = ""
                Dim strmaingpcodeto As String = ""
                Dim strexcgpcodefrom As String = ""
                Dim strexcgpcodeto As String = ""
                Dim strmarketcodefrom As String = ""
                Dim strmarketcodeto As String = ""
                Dim strcitycodefrom As String = ""
                Dim strcitycodeto As String = ""
                Dim strexcproviderfrom As String = ""
                Dim strexcproviderto As String = ""
                Dim strhotelcodefrom As String = ""
                Dim strhotelcodeto As String = ""
                Dim strclientcodefrom As String = ""
                Dim strclientcodeto As String = ""
                Dim strpaytermsfrom As String = ""
                Dim strpaytermsto As String = ""
                Dim strspfrom As String = ""
                Dim strspto As String = ""
                Dim strdriverfrom As String = ""
                Dim strdriverto As String = ""
                Dim strsalesfrom As String = ""
                Dim strsalesto As String = ""
                Dim strsapcodefrom As String = ""
                Dim strsapcodeto As String = ""
                Dim strcollectfrom As String = ""
                Dim strcollectto As String = ""
                Dim strtourfrom As String = ""
                Dim strtourto As String = ""
                Dim strmulexctype As String = ""
                Dim stroperatorfrom As String = ""
                Dim stroperatorto As String = ""
                Dim strtourguidefrom As String = ""
                Dim strtourguideto As String = ""
                Dim strselltypecodefrom As String = ""
                Dim strselltypecodeto As String = ""


                Dim trfreq As String = ""
                'If txtexctype.Text = "" Then
                '    clearexcursiontable()
                'End If

                cusomerwisexcursion()
                If txtexctype.Text = "" Then
                    clearexcursiontable()
                Else
                    excursionmultipleselection()
                End If


                'maingroupcode
                strmaingpcodefrom = IIf(UCase(ddlFrommaingpcode.Items(ddlFrommaingpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFrommaingpcode.Items(ddlFrommaingpcode.SelectedIndex).Text, "")
                strmaingpcodeto = IIf(UCase(ddlTomaingpcode.Items(ddlTomaingpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlTomaingpcode.Items(ddlTomaingpcode.SelectedIndex).Text, "")
                If strmaingpcodefrom <> "" And strmaingpcodeto <> "" Then
                    strrepfilter = " Main Group from :" + strmaingpcodefrom + " To :" & strmaingpcodeto
                End If

                'excursiongroup code
                strexcgpcodefrom = IIf(UCase(ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text, "")
                strexcgpcodeto = IIf(UCase(ddltoexcgpcode.Items(ddltoexcgpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoexcgpcode.Items(ddltoexcgpcode.SelectedIndex).Text, "")

                If strexcgpcodefrom <> "" And strexcgpcodeto <> "" Then
                    strrepfilter = strrepfilter + " Excurison Group from :" + strexcgpcodefrom + " To :" & strexcgpcodeto
                End If



                'Market 
                strmarketcodefrom = IIf(UCase(ddlfrommarketcode.Items(ddlfrommarketcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfrommarketcode.Items(ddlfrommarketcode.SelectedIndex).Text, "")
                strmarketcodeto = IIf(UCase(ddltomarketcode.Items(ddltomarketcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltomarketcode.Items(ddltomarketcode.SelectedIndex).Text, "")


                If strmarketcodefrom <> "" And strmarketcodeto <> "" Then
                    strrepfilter = strrepfilter + " Market  from :" + strmarketcodefrom + " To :" & strmarketcodeto
                End If

                'City 
                strcitycodefrom = IIf(UCase(ddlfromcitycode.Items(ddlfromcitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromcitycode.Items(ddlfromcitycode.SelectedIndex).Text, "")
                strcitycodeto = IIf(UCase(ddltocitycode.Items(ddltocitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltocitycode.Items(ddltocitycode.SelectedIndex).Text, "")


                If strcitycodefrom <> "" And strcitycodeto <> "" Then
                    strrepfilter = strrepfilter + " City  from :" + strcitycodefrom + " To :" & strcitycodeto
                End If

                'exc provider 
                strexcproviderfrom = IIf(UCase(ddlfromexcprovider.Items(ddlfromexcprovider.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromexcprovider.Items(ddlfromexcprovider.SelectedIndex).Text, "")
                strexcproviderto = IIf(UCase(ddltoexcprovider.Items(ddltoexcprovider.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoexcprovider.Items(ddltoexcprovider.SelectedIndex).Text, "")


                If strexcproviderfrom <> "" And strexcproviderto <> "" Then
                    strrepfilter = strrepfilter + " Excursion Provider  from :" + strexcproviderfrom + " To :" & strexcproviderto
                End If

                'hotel
                strhotelcodefrom = IIf(UCase(ddlfromhotelcode.Items(ddlfromhotelcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromhotelcode.Items(ddlfromhotelcode.SelectedIndex).Text, "")
                strhotelcodeto = IIf(UCase(ddltohotelcode.Items(ddltohotelcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltohotelcode.Items(ddltohotelcode.SelectedIndex).Text, "")

                If strhotelcodefrom <> "" And strhotelcodeto <> "" Then
                    strrepfilter = strrepfilter + " Hotel  from :" + strhotelcodefrom + " To :" & strhotelcodeto
                End If

                'Client
                strclientcodefrom = IIf(UCase(ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text, "")
                strclientcodeto = IIf(UCase(ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text, "")

                If strclientcodefrom <> "" And strclientcodeto <> "" Then
                    strrepfilter = strrepfilter + " Client  from :" + strclientcodefrom + " To :" & strclientcodeto
                End If

                'Payment Terms
                strpaytermsfrom = IIf(UCase(ddlfrompaytermscode.Items(ddlfrompaytermscode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfrompaytermscode.Items(ddlfrompaytermscode.SelectedIndex).Text, "")
                strpaytermsto = IIf(UCase(ddltopaytermscode.Items(ddltopaytermscode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltopaytermscode.Items(ddltopaytermscode.SelectedIndex).Text, "")


                If strpaytermsfrom <> "" And strpaytermsto <> "" Then
                    strrepfilter = strrepfilter + " Payment Terms  from :" + strpaytermsfrom + " To :" & strpaytermsto
                End If

                'Service Provider
                strspfrom = IIf(UCase(ddlfromsp.Items(ddlfromsp.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsp.Items(ddlfromsp.SelectedIndex).Text, "")
                strspto = IIf(UCase(ddltosp.Items(ddltosp.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosp.Items(ddltosp.SelectedIndex).Text, "")


                If strspfrom <> "" And strspto <> "" Then
                    strrepfilter = strrepfilter + " Service Provider  from :" + strspfrom + " To :" & strspto
                End If

                'Driver
                strdriverfrom = IIf(UCase(ddlfromdcode.Items(ddlfromdcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromdcode.Items(ddlfromdcode.SelectedIndex).Text, "")
                strdriverto = IIf(UCase(ddltodcode.Items(ddltodcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltodcode.Items(ddltodcode.SelectedIndex).Text, "")


                If strdriverfrom <> "" And strdriverto <> "" Then
                    strrepfilter = strrepfilter + " Driver  from :" + strdriverfrom + " To :" & strdriverto
                End If



                'Sales office
                strsalesfrom = IIf(UCase(ddlfromsalescode.Items(ddlfromsalescode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsalescode.Items(ddlfromsalescode.SelectedIndex).Text, "")
                strsalesto = IIf(UCase(ddltosalescode.Items(ddltosalescode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosalescode.Items(ddltosalescode.SelectedIndex).Text, "")


                If strsalesfrom <> "" And strsalesto <> "" Then
                    strrepfilter = strrepfilter + " Sales Office  from :" + strsalesfrom + " To :" & strsalesto
                End If


                'Sales person
                strsapcodefrom = IIf(UCase(ddlfromsapcode.Items(ddlfromsapcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsapcode.Items(ddlfromsapcode.SelectedIndex).Text, "")
                strsapcodeto = IIf(UCase(ddltosapcode.Items(ddltosapcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosapcode.Items(ddltosapcode.SelectedIndex).Text, "")

                If strsapcodefrom <> "" And strsapcodeto <> "" Then
                    strrepfilter = strrepfilter + " Sales Person  from :" + strsapcodefrom + " To :" & strsapcodeto
                End If


                'OPERATOR
                stroperatorfrom = IIf(UCase(ddlfromoperatorcode.Items(ddlfromoperatorcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromoperatorcode.Items(ddlfromoperatorcode.SelectedIndex).Text, "")
                stroperatorto = IIf(UCase(ddltooperatorcode.Items(ddltooperatorcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltooperatorcode.Items(ddltooperatorcode.SelectedIndex).Text, "")

                ''11112014 tourguide

                strtourguidefrom = IIf(UCase(ddlfromtgcode.Items(ddlfromtgcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromtgcode.Items(ddlfromtgcode.SelectedIndex).Text, "")
                strtourguideto = IIf(UCase(ddltotgcode.Items(ddltotgcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltotgcode.Items(ddltotgcode.SelectedIndex).Text, "")

                strselltypecodefrom = IIf(UCase(ddlfromsellcode.Items(ddlfromsellcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsellcode.Items(ddlfromsellcode.SelectedIndex).Text, "")
                strselltypecodeto = IIf(UCase(ddltosellcode.Items(ddltosellcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosellcode.Items(ddltosellcode.SelectedIndex).Text, "")


                If stroperatorfrom <> "" And stroperatorto <> "" Then
                    strrepfilter = strrepfilter + " Opertator  from :" + stroperatorfrom + " To :" & stroperatorto
                End If

                If strtourguidefrom <> "" And strtourguideto <> "" Then
                    strrepfilter = strrepfilter + " Tour Guide  from :" + strtourguidefrom + " To :" & strtourguideto
                End If

                If strselltypecodefrom <> "" And strselltypecodeto <> "" Then
                    strrepfilter = strrepfilter + " Selling Type Code from :" + strselltypecodefrom + " To :" & strselltypecodeto
                End If


                'Collect By
                strcollectfrom = IIf(UCase(ddlfromcolcode.Items(ddlfromcolcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromcolcode.Items(ddlfromcolcode.SelectedIndex).Text, "")
                strcollectto = IIf(UCase(ddltocolcode.Items(ddltocolcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltocolcode.Items(ddltocolcode.SelectedIndex).Text, "")

                If strcollectfrom <> "" And strcollectto <> "" Then
                    strrepfilter = strrepfilter + " Collect  from :" + strcollectfrom + " To :" & strcollectto
                End If

                If radreqrange.Checked = True Then
                    strrepfilter = strrepfilter + " Request Date  from :" + txtreqfrom.Text + " To :" & txtreqto.Text
                End If

                If radtourrange.Checked = True Then
                    strrepfilter = strrepfilter + " Tour Date  from :" + txttourfrom.Text + " To :" & txttourto.Text
                End If


                strmulexctype = txtexctype.Text

                If txtexctype.Text = "" Then
                    excursionno = ""
                End If

                'excursionmultipleselection()
                'strgroupby = mygroupresult()
                strfromdate = Mid(Format(CType(txtreqfrom.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtreqto.Text, Date), "u"), 1, 10)
                strtourfrom = Mid(Format(CType(txttourfrom.Text, Date), "u"), 1, 10)
                strtourto = Mid(Format(CType(txttourto.Text, Date), "u"), 1, 10)

                If radreqrange.Checked = True Then
                    txtreqfrom.Enabled = True
                    txtreqto.Enabled = True
                End If

                If radtourrange.Checked = True Then
                    txttourfrom.Enabled = True
                    txttourto.Enabled = True
                End If


                strReportTitle = " Excursion  Report"


                Dim strpop As String = ""

                '    strpop = "window.open('rptExcursionReport.aspx?Pageame=TranferPricelist&BackPageName=rptTransferPricelistReport.aspx " _
                '& "&reqfrom=" & strfromdate & "&reqto=" & strtodate & "&Date=" & IIf(radreqrange.Checked = True, "Yes", "") _
                ' & "&tourfrom=" & strtourfrom & "&tourto=" & strtourto & "&dateyn=" & IIf(radtourrange.Checked = True, "Yes", "") _
                ' & "&maingpcodefrom=" & strmaingpcodefrom & "&maingpcodeto=" & strmaingpcodeto _
                '   & "&excgpcodefrom=" & strexcgpcodefrom & "&excgpcodeto=" & strexcgpcodeto _
                '   & "&marketcodefrom=" & strmarketcodefrom & "&marketcodeto=" & strmarketcodeto _
                '   & "&citycodefrom=" & strcitycodefrom & "&citycodeto=" & strcitycodeto _
                '   & "&excproviderfrom=" & strexcproviderfrom & "&excproviderto=" & strexcproviderto _
                '   & "&hotelcodefrom=" & strhotelcodefrom & "&hotelcodeto=" & strhotelcodeto _
                '   & "&clientcodefrom=" & strclientcodefrom & "&clientcodeto=" & strclientcodeto _
                '   & "&paytermsfrom=" & strpaytermsfrom & "&paytermsto=" & strpaytermsto _
                '   & "&spfrom=" & strspfrom & "&spto=" & strspto _
                '   & "&driverfrom=" & strdriverfrom & "&driverto=" & strdriverto _
                '   & "&salesfrom=" & strsalesfrom & "&salesto=" & strsalesto _
                '   & "&sapcodefrom=" & strsapcodefrom & "&sapcodeto=" & strsapcodeto _
                '     & "&operatorcodefrom=" & stroperatorfrom & "&operatorcodeto=" & stroperatorto _
                '      & "&tourguidefrom=" & strtourguidefrom & "&tourguideto=" & strtourguideto _
                '                    & "&selltypecodefrom=" & strselltypecodefrom & "&selltypecodeto=" & strselltypecodeto _
                '   & "&collectfrom=" & strcollectfrom & "&collectto=" & strcollectto _
                '   & "&opt=" & strmulexctype _
                '   & "&reporttype=" & ddlreptype.SelectedIndex _
                '   & "&strgroupby=" & strgroupby() _
                '    & "&language=" & ddllang.SelectedIndex _
                '     & "&dmc=" & ddldmc.SelectedIndex _
                '      & "&chkstatus=" & chkstatus() _
                '      & "&excursions=" & excursionno _
                '        & "&trfreq=" & IIf(chktrfreq.Checked = "true", 1, 0) _
                '& "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"


                strpop = "window.open('rptExcursionReport.aspx?Pageame=TranferPricelist&BackPageName=rptTransferPricelistReport.aspx " _
            & "&reqfrom=" & strfromdate & "&reqto=" & strtodate & "&Date=" & IIf(radreqrange.Checked = True, "Yes", "") _
             & "&tourfrom=" & strtourfrom & "&tourto=" & strtourto & "&dateyn=" & IIf(radtourrange.Checked = True, "Yes", "") _
             & "&maingpcodefrom=" & strmaingpcodefrom & "&maingpcodeto=" & strmaingpcodeto _
               & "&excgpcodefrom=" & strexcgpcodefrom & "&excgpcodeto=" & strexcgpcodeto _
               & "&marketcodefrom=" & strmarketcodefrom & "&marketcodeto=" & strmarketcodeto _
               & "&citycodefrom=" & strcitycodefrom & "&citycodeto=" & strcitycodeto _
               & "&excproviderfrom=" & strexcproviderfrom & "&excproviderto=" & strexcproviderto _
               & "&hotelcodefrom=" & strhotelcodefrom & "&hotelcodeto=" & strhotelcodeto _
               & "&clientcodefrom=" & strclientcodefrom & "&clientcodeto=" & strclientcodeto _
               & "&paytermsfrom=" & strpaytermsfrom & "&paytermsto=" & strpaytermsto _
               & "&spfrom=" & strspfrom & "&spto=" & strspto _
               & "&driverfrom=" & strdriverfrom & "&driverto=" & strdriverto _
               & "&salesfrom=" & strsalesfrom & "&salesto=" & strsalesto _
               & "&sapcodefrom=" & strsapcodefrom & "&sapcodeto=" & strsapcodeto _
                 & "&operatorcodefrom=" & stroperatorfrom & "&operatorcodeto=" & stroperatorto _
                  & "&tourguidefrom=" & strtourguidefrom & "&tourguideto=" & strtourguideto _
                                & "&selltypecodefrom=" & strselltypecodefrom & "&selltypecodeto=" & strselltypecodeto _
               & "&collectfrom=" & strcollectfrom & "&collectto=" & strcollectto _
               & "&opt=" & strmulexctype _
               & "&reporttype=" & ddlreptype.SelectedIndex _
               & "&strgroupby=" & strgroupby() _
                & "&language=" & ddllang.SelectedIndex _
                 & "&dmc=" & ddldmc.SelectedIndex _
                  & "&chkstatus=" & chkstatus() _
                  & "&excursions=" & excursionno _
                    & "&trfreq=" & IIf(chktrfreq.Checked = "true", 1, 0) _
            & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient' );"


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptTransfercostPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub





    Private Function chkstatus()
        Dim myint As Integer
        Try
            showcostp = IIf(showcost(priidcost) = 1, "Y", "N")
            showcommip = IIf(showcost(priidcomi) = 1, "Y", "N")
            If (chkcost.Checked = True And chkcommission.Checked = True And showcostp = "Y" And showcommip = "Y") Then
                myint = 4
            ElseIf (chkcost.Checked = True And showcostp = "Y") Then
                myint = 2
            ElseIf (chkcommission.Checked = True And showcommip = "Y") Then
                myint = 1
            Else
                myint = 0
            End If

            chkstatus = myint

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


    End Function

    Private Function strgroupby()
        Dim mystr As String = ""
        Select Case ddlgroupby.SelectedIndex
            Case 0
                mystr = "A"
            Case 1
                mystr = "B"
            Case 2
                mystr = "C"
            Case 3
                mystr = "D"
            Case 4
                mystr = "E"
            Case 5
                mystr = "F"
            Case 6
                mystr = "S"
            Case 7
                mystr = "H"
            Case 8
                mystr = "T"
            Case 9
                mystr = "P"
            Case 10
                mystr = "M"


        End Select
        strgroupby = mystr
    End Function

    Private Sub excursionmultipleselection()
        Dim mySqlCmd As SqlCommand
        Dim mySqlReader As SqlDataReader
        Dim mySqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim sqlTrans As SqlTransaction
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        Try
            Dim chksel As HtmlInputCheckBox
            Dim i As Integer = 0

            mySqlCmd = New SqlCommand("sp_docgen", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@optionname", SqlDbType.VarChar, 10)).Value = "PRINT"
            Dim param1 As SqlParameter
            Dim param2 As SqlParameter
            param1 = New SqlParameter
            param1.ParameterName = "@newno"
            param1.Direction = ParameterDirection.Output
            param1.DbType = DbType.String
            param1.Size = 200
            mySqlCmd.Parameters.Add(param1)

            param2 = New SqlParameter
            param2.ParameterName = "@docprefix"
            param2.Direction = ParameterDirection.Output
            param2.DbType = DbType.String
            param2.Size = 200
            mySqlCmd.Parameters.Add(param2)
            'myDataAdapter = New SqlDataAdapter(mySqlCmd)
            Dim str1 As String = ""
            Dim str2 As String = ""
            Dim newno As String = ""

            mySqlCmd.ExecuteNonQuery()
            str1 = param1.Value.ToString()
            str2 = param2.Value.ToString()
            str1 = "000000" + str1
            newno = str2 + "/" + Right(str1, 6)



            For Each GvRow In gv_SearchResult.Rows

                mySqlCmd = New SqlCommand("sp_multiple_excursions", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure


                chksel = GvRow.FindControl("chkcode")
                If chksel.Checked = True Then

                    mySqlCmd.Parameters.Add(New SqlParameter("@printid", SqlDbType.VarChar, 10)).Value = newno
                    mySqlCmd.Parameters.Add(New SqlParameter("@excursion", SqlDbType.VarChar, 20)).Value = gv_SearchResult.Rows(i).Cells(2).Text

                    mySqlCmd.ExecuteNonQuery()
                    clsDBConnect.dbCommandClose(mySqlCmd)
                End If
                i = i + 1
            Next

            Session.Add("mulselno", newno)
            'excursionno = newno

            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objutils.WritErrorLog("rptTransfercostPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub
    Private Function showcost(ByVal mypriid As Integer) As String
        Dim mySqlCmd As SqlCommand
        Dim mySqlReader As SqlDataReader
        Dim mySqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim sqlTrans As SqlTransaction
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        Try
            Dim chksel As HtmlInputCheckBox
            Dim i As Integer = 0

            mySqlCmd = New SqlCommand("sp_checkcostcommision", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@user", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@appid", SqlDbType.VarChar, 10)).Value = CType(Master.FindControl("hdnAppId"), HiddenField).Value
            mySqlCmd.Parameters.Add(New SqlParameter("@priid", SqlDbType.Int)).Value = mypriid
            Dim param1 As SqlParameter
            Dim param2 As SqlParameter
            param1 = New SqlParameter
            param1.ParameterName = "@showcost"
            param1.Direction = ParameterDirection.Output
            param1.DbType = DbType.String
            param1.Size = 200
            mySqlCmd.Parameters.Add(param1)


            'myDataAdapter = New SqlDataAdapter(mySqlCmd)
            Dim str1 As String = ""
            Dim str2 As String = ""
            Dim newno As String = ""

            mySqlCmd.ExecuteNonQuery()
            showcost = param1.Value.ToString()


            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objutils.WritErrorLog("rptTransfercostPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Function
    Private Sub cusomerwisexcursion()
        Dim mySqlCmd As SqlCommand
        Dim mySqlReader As SqlDataReader
        Dim mySqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim sqlTrans As SqlTransaction
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        Try
            Dim chksel As HtmlInputCheckBox
            Dim i As Integer = 0

            mySqlCmd = New SqlCommand("sp_rep_customerwiseexc", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure


            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtreqfrom.Text, Date), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txtreqto.Text, Date), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@agentfrm", SqlDbType.VarChar, 20)).Value = IIf(ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text = "[Select]", "", ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@agentto", SqlDbType.VarChar, 20)).Value = IIf(ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text = "[Select]", "", ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text)

            mySqlCmd.Parameters.Add(New SqlParameter("@exctypfrm", SqlDbType.VarChar, 20)).Value = txtexctype.Text  'IIf(ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text = "[Select]", "", ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@exctypto", SqlDbType.VarChar, 20)).Value = txtexctype.Text 'IIf(ddltoexcgpcode.Items(ddltoexcgpcode.SelectedIndex).Text = "[Select]", "", ddltoexcgpcode.Items(ddltoexcgpcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@hotfrm", SqlDbType.VarChar, 20)).Value = IIf(ddlfromhotelcode.Items(ddlfromhotelcode.SelectedIndex).Text = "[Select]", "", ddlfromhotelcode.Items(ddlfromhotelcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@hotto", SqlDbType.VarChar, 20)).Value = IIf(ddltohotelcode.Items(ddltohotelcode.SelectedIndex).Text = "[Select]", "", ddltohotelcode.Items(ddltohotelcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@mktfrm", SqlDbType.VarChar, 20)).Value = IIf(ddlfrommarketcode.Items(ddlfrommarketcode.SelectedIndex).Text = "[Select]", "", ddlfrommarketcode.Items(ddlfrommarketcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@mktto", SqlDbType.VarChar, 20)).Value = IIf(ddltomarketcode.Items(ddltomarketcode.SelectedIndex).Text = "[Select]", "", ddltomarketcode.Items(ddltomarketcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@alldate", SqlDbType.Int)).Value = IIf(radreqrange.Checked = True, 1, 0)
            mySqlCmd.Parameters.Add(New SqlParameter("@partyfrom", SqlDbType.VarChar, 20)).Value = IIf(ddlfromsp.Items(ddlfromsp.SelectedIndex).Text = "[Select]", "", ddlfromsp.Items(ddlfromsp.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@partyto", SqlDbType.VarChar, 20)).Value = IIf(ddltosp.Items(ddltosp.SelectedIndex).Text = "[Select]", "", ddltosp.Items(ddltosp.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@mainfrom", SqlDbType.VarChar, 20)).Value = IIf(ddlFrommaingpcode.Items(ddlFrommaingpcode.SelectedIndex).Text = "[Select]", "", ddltosp.Items(ddltosp.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@mainto", SqlDbType.VarChar, 20)).Value = IIf(ddlTomaingpcode.Items(ddlTomaingpcode.SelectedIndex).Text = "[Select]", "", ddlTomaingpcode.Items(ddlTomaingpcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@dmc", SqlDbType.Int)).Value = ddldmc.SelectedIndex
            mySqlCmd.Parameters.Add(New SqlParameter("@driverfrom", SqlDbType.VarChar, 20)).Value = IIf(ddlfromdcode.Items(ddlfromdcode.SelectedIndex).Text = "[Select]", "", ddlfromdcode.Items(ddlfromdcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@driverto", SqlDbType.VarChar, 20)).Value = IIf(ddltodcode.Items(ddltodcode.SelectedIndex).Text = "[Select]", "", ddltodcode.Items(ddltodcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@officefrom", SqlDbType.VarChar, 20)).Value = IIf(ddlfromsalescode.Items(ddlfromsalescode.SelectedIndex).Text = "[Select]", "", ddltodcode.Items(ddltodcode.SelectedIndex).Text)
            mySqlCmd.Parameters.Add(New SqlParameter("@officeto", SqlDbType.VarChar, 20)).Value = IIf(ddltosalescode.Items(ddltosalescode.SelectedIndex).Text = "[Select]", "", ddltosalescode.Items(ddltosalescode.SelectedIndex).Text)

            mySqlCmd.ExecuteNonQuery()


            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objutils.WritErrorLog("rptTransfercostPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim strsqlqry As String
        Dim sqlconn As New SqlConnection
        Dim mydataadapter As SqlDataAdapter

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strsqlqry = ""
        Try

            If ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text <> "[Select]" Then
                strsqlqry = "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode  = othtypmast.othgrpcode where othgrpmast.othgrpcode between '" & ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text & "' and '" & ddltoexcgpcode.Items(ddltoexcgpcode.SelectedIndex).Text & "'"
            Else
                strsqlqry = "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode  = othtypmast.othgrpcode "
            End If
           
            sqlconn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            mydataadapter = New SqlDataAdapter(strsqlqry, sqlconn)
            mydataadapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()

                btnSave.Visible = True
                btnCancel.Visible = True
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("OthPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(mydataadapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(sqlconn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()

    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=NewclientsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Btnloadgrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnloadgrid.Click
        If txtexctype.Text <> "" Then
            gv_SearchResult.Visible = True
            btnSave.Visible = True
            btnCancel.Visible = True
        Else
            FillGrid("othtypcode", "othtypcode")

        End If


    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        gv_SearchResult.Visible = False
        txtexctype.Text = ""
        btnSave.Visible = False
        btnCancel.Visible = False
        rbexctypeall.Checked = True
        rbexctyperange.Checked = False
        ''txtexctype.Visible = False
        If rbexctypeall.Checked = True Then
            txtexctype.Style.Add("display", "none")
            btnloadgrid.Style.Add("display", "none")
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strstr As String = ""
        Dim chksel As HtmlInputCheckBox
        Dim i As Integer = 0


        For Each GvRow In gv_SearchResult.Rows
            chksel = GvRow.FindControl("chkcode")
            If chksel.Checked = True Then
                strstr = strstr + gv_SearchResult.Rows(i).Cells(2).Text + ";"
            End If
            i = i + 1
        Next
        txtexctype.Text = strstr

        gv_SearchResult.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        'If txtexctype.Text = "" Then
        '    clearexcursiontable()
        'Else
        '    excursionmultipleselection()
        'End If
    End Sub

    Private Sub clearexcursiontable()
        Dim mySqlCmd As SqlCommand
        Dim mySqlReader As SqlDataReader
        Dim mySqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim sqlTrans As SqlTransaction
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        Try




            mySqlCmd = New SqlCommand("sp_del_multiple_excursions", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.ExecuteNonQuery()
            clsDBConnect.dbCommandClose(mySqlCmd)

            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objutils.WritErrorLog("rptTransfercostPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub
#Region "Public Function SendMailConfirm()"
    Public Function SendMailConfirmnew(ByVal emailid As String)
        Dim rnd As Random = New Random
        Dim strFilename As String = ""
        Dim strFullpath As String = ""
        Dim dsval As DataSet
        Dim reportoption As String = ""

        Try

            Dim strReportTitle As String = ""
            Dim param1 As String = ""
            Dim param2 As String = ""
            Dim param3 As String = ""
            Dim param4 As String = ""
            Dim param5 As String = ""
            Dim param6 As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""




            Dim strrepfilter As String = ""
            Dim strreportoption As String = ""
            Dim strsectorcode As String = ""
            Dim strarr(6) As String
            Dim k As Integer
            Dim P As Integer
            Dim strmaingpcodefrom As String = ""
            Dim strmaingpcodeto As String = ""
            Dim strexcgpcodefrom As String = ""
            Dim strexcgpcodeto As String = ""
            Dim strmarketcodefrom As String = ""
            Dim strmarketcodeto As String = ""
            Dim strcitycodefrom As String = ""
            Dim strcitycodeto As String = ""
            Dim strexcproviderfrom As String = ""
            Dim strexcproviderto As String = ""
            Dim strhotelcodefrom As String = ""
            Dim strhotelcodeto As String = ""
            Dim strclientcodefrom As String = ""
            Dim strclientcodeto As String = ""
            Dim strpaytermsfrom As String = ""
            Dim strpaytermsto As String = ""
            Dim strspfrom As String = ""
            Dim strspto As String = ""
            Dim strdriverfrom As String = ""
            Dim strdriverto As String = ""
            Dim strsalesfrom As String = ""
            Dim strsalesto As String = ""
            Dim strsapcodefrom As String = ""
            Dim strsapcodeto As String = ""
            Dim strcollectfrom As String = ""
            Dim strcollectto As String = ""
            Dim strtourfrom As String = ""
            Dim strtourto As String = ""
            Dim strmulexctype As String = ""
            Dim stroperatorfrom As String = ""
            Dim stroperatorto As String = ""
            Dim trfreq As String = ""
            Dim strtourguidefrom As String = ""
            Dim strtourguideto As String = ""
            Dim strselltypecodefrom As String = ""
            Dim strselltypecodeto As String = ""
            'If txtexctype.Text = "" Then
            '    clearexcursiontable()
            'End If

            cusomerwisexcursion()
            If txtexctype.Text = "" Then
                clearexcursiontable()
            Else
                excursionmultipleselection()
            End If


            'maingroupcode
            strmaingpcodefrom = IIf(UCase(ddlFrommaingpcode.Items(ddlFrommaingpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFrommaingpcode.Items(ddlFrommaingpcode.SelectedIndex).Text, "")
            strmaingpcodeto = IIf(UCase(ddlTomaingpcode.Items(ddlTomaingpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlTomaingpcode.Items(ddlTomaingpcode.SelectedIndex).Text, "")
            If strmaingpcodefrom <> "" And strmaingpcodeto <> "" Then
                strrepfilter = " Main Group from :" + strmaingpcodefrom + " To :" & strmaingpcodeto
            End If

            'excursiongroup code
            strexcgpcodefrom = IIf(UCase(ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromexcgpcode.Items(ddlfromexcgpcode.SelectedIndex).Text, "")
            strexcgpcodeto = IIf(UCase(ddltoexcgpcode.Items(ddltoexcgpcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoexcgpcode.Items(ddltoexcgpcode.SelectedIndex).Text, "")

            If strexcgpcodefrom <> "" And strexcgpcodeto <> "" Then
                strrepfilter = strrepfilter + " Excurison Group from :" + strexcgpcodefrom + " To :" & strexcgpcodeto
            End If



            'Market 
            strmarketcodefrom = IIf(UCase(ddlfrommarketcode.Items(ddlfrommarketcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfrommarketcode.Items(ddlfrommarketcode.SelectedIndex).Text, "")
            strmarketcodeto = IIf(UCase(ddltomarketcode.Items(ddltomarketcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltomarketcode.Items(ddltomarketcode.SelectedIndex).Text, "")


            If strmarketcodefrom <> "" And strmarketcodeto <> "" Then
                strrepfilter = strrepfilter + " Market  from :" + strmarketcodefrom + " To :" & strmarketcodeto
            End If

            'City 
            strcitycodefrom = IIf(UCase(ddlfromcitycode.Items(ddlfromcitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromcitycode.Items(ddlfromcitycode.SelectedIndex).Text, "")
            strcitycodeto = IIf(UCase(ddltocitycode.Items(ddltocitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltocitycode.Items(ddltocitycode.SelectedIndex).Text, "")


            If strcitycodefrom <> "" And strcitycodeto <> "" Then
                strrepfilter = strrepfilter + " City  from :" + strcitycodefrom + " To :" & strcitycodeto
            End If

            'exc provider 
            strexcproviderfrom = IIf(UCase(ddlfromexcprovider.Items(ddlfromexcprovider.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromexcprovider.Items(ddlfromexcprovider.SelectedIndex).Text, "")
            strexcproviderto = IIf(UCase(ddltoexcprovider.Items(ddltoexcprovider.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoexcprovider.Items(ddltoexcprovider.SelectedIndex).Text, "")


            If strexcproviderfrom <> "" And strexcproviderto <> "" Then
                strrepfilter = strrepfilter + " Excursion Provider  from :" + strexcproviderfrom + " To :" & strexcproviderto
            End If

            'hotel
            strhotelcodefrom = IIf(UCase(ddlfromhotelcode.Items(ddlfromhotelcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromhotelcode.Items(ddlfromhotelcode.SelectedIndex).Text, "")
            strhotelcodeto = IIf(UCase(ddltohotelcode.Items(ddltohotelcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltohotelcode.Items(ddltohotelcode.SelectedIndex).Text, "")

            If strhotelcodefrom <> "" And strhotelcodeto <> "" Then
                strrepfilter = strrepfilter + " Hotel  from :" + strhotelcodefrom + " To :" & strhotelcodeto
            End If

            'Client
            strclientcodefrom = IIf(UCase(ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromclientcode.Items(ddlfromclientcode.SelectedIndex).Text, "")
            strclientcodeto = IIf(UCase(ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoclientcode.Items(ddltoclientcode.SelectedIndex).Text, "")

            If strclientcodefrom <> "" And strclientcodeto <> "" Then
                strrepfilter = strrepfilter + " Client  from :" + strclientcodefrom + " To :" & strclientcodeto
            End If

            'Payment Terms
            strpaytermsfrom = IIf(UCase(ddlfrompaytermscode.Items(ddlfrompaytermscode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfrompaytermscode.Items(ddlfrompaytermscode.SelectedIndex).Text, "")
            strpaytermsto = IIf(UCase(ddltopaytermscode.Items(ddltopaytermscode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltopaytermscode.Items(ddltopaytermscode.SelectedIndex).Text, "")


            If strpaytermsfrom <> "" And strpaytermsto <> "" Then
                strrepfilter = strrepfilter + " Payment Terms  from :" + strpaytermsfrom + " To :" & strpaytermsto
            End If

            'Service Provider
            strspfrom = IIf(UCase(ddlfromsp.Items(ddlfromsp.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsp.Items(ddlfromsp.SelectedIndex).Text, "")
            strspto = IIf(UCase(ddltosp.Items(ddltosp.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosp.Items(ddltosp.SelectedIndex).Text, "")


            If strspfrom <> "" And strspto <> "" Then
                strrepfilter = strrepfilter + " Service Provider  from :" + strspfrom + " To :" & strspto
            End If

            'Driver
            strdriverfrom = IIf(UCase(ddlfromdcode.Items(ddlfromdcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromdcode.Items(ddlfromdcode.SelectedIndex).Text, "")
            strdriverto = IIf(UCase(ddltodcode.Items(ddltodcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltodcode.Items(ddltodcode.SelectedIndex).Text, "")


            If strdriverfrom <> "" And strdriverto <> "" Then
                strrepfilter = strrepfilter + " Driver  from :" + strdriverfrom + " To :" & strdriverto
            End If



            'Sales office
            strsalesfrom = IIf(UCase(ddlfromsalescode.Items(ddlfromsalescode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsalescode.Items(ddlfromsalescode.SelectedIndex).Text, "")
            strsalesto = IIf(UCase(ddltosalescode.Items(ddltosalescode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosalescode.Items(ddltosalescode.SelectedIndex).Text, "")


            If strsalesfrom <> "" And strsalesto <> "" Then
                strrepfilter = strrepfilter + " Sales Office  from :" + strsalesfrom + " To :" & strsalesto
            End If


            'Sales person
            strsapcodefrom = IIf(UCase(ddlfromsapcode.Items(ddlfromsapcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsapcode.Items(ddlfromsapcode.SelectedIndex).Text, "")
            strsapcodeto = IIf(UCase(ddltosapcode.Items(ddltosapcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosapcode.Items(ddltosapcode.SelectedIndex).Text, "")

            If strsapcodefrom <> "" And strsapcodeto <> "" Then
                strrepfilter = strrepfilter + " Sales Person  from :" + strsapcodefrom + " To :" & strsapcodeto
            End If


            'OPERATOR
            stroperatorfrom = IIf(UCase(ddlfromoperatorcode.Items(ddlfromoperatorcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromoperatorcode.Items(ddlfromoperatorcode.SelectedIndex).Text, "")
            stroperatorto = IIf(UCase(ddltooperatorcode.Items(ddltooperatorcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltooperatorcode.Items(ddltooperatorcode.SelectedIndex).Text, "")

            If stroperatorfrom <> "" And stroperatorto <> "" Then
                strrepfilter = strrepfilter + " Opertator  from :" + stroperatorfrom + " To :" & stroperatorto
            End If


            'Collect By
            strcollectfrom = IIf(UCase(ddlfromcolcode.Items(ddlfromcolcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromcolcode.Items(ddlfromcolcode.SelectedIndex).Text, "")
            strcollectto = IIf(UCase(ddltocolcode.Items(ddltocolcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltocolcode.Items(ddltocolcode.SelectedIndex).Text, "")

            If strcollectfrom <> "" And strcollectto <> "" Then
                strrepfilter = strrepfilter + " Collect  from :" + strcollectfrom + " To :" & strcollectto
            End If

            If radreqrange.Checked = True Then
                strrepfilter = strrepfilter + " Request Date  from :" + txtreqfrom.Text + " To :" & txtreqto.Text
            End If

            If radtourrange.Checked = True Then
                strrepfilter = strrepfilter + " Tour Date  from :" + txttourfrom.Text + " To :" & txttourto.Text
            End If


            strtourguidefrom = IIf(UCase(ddlfromtgcode.Items(ddlfromtgcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromtgcode.Items(ddlfromtgcode.SelectedIndex).Text, "")
            strtourguideto = IIf(UCase(ddltotgcode.Items(ddltotgcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltotgcode.Items(ddltotgcode.SelectedIndex).Text, "")

            strselltypecodefrom = IIf(UCase(ddlfromsellcode.Items(ddlfromsellcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlfromsellcode.Items(ddlfromsellcode.SelectedIndex).Text, "")
            strselltypecodeto = IIf(UCase(ddltosellcode.Items(ddltosellcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltosellcode.Items(ddltosellcode.SelectedIndex).Text, "")


            strmulexctype = txtexctype.Text

            If txtexctype.Text = "" Then
                excursionno = ""
            End If

            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)

            'excursionmultipleselection()
            'strgroupby = mygroupresult()
            strfromdate = Mid(Format(CType(txtreqfrom.Text, Date), "u"), 1, 10)
            strtodate = Mid(Format(CType(txtreqto.Text, Date), "u"), 1, 10)
            strtourfrom = Mid(Format(CType(txttourfrom.Text, Date), "u"), 1, 10)
            strtourto = Mid(Format(CType(txttourto.Text, Date), "u"), 1, 10)

            If radreqrange.Checked = True Then
                txtreqfrom.Enabled = True
                txtreqto.Enabled = True
            End If

            If radtourrange.Checked = True Then
                txttourfrom.Enabled = True
                txttourto.Enabled = True
            End If


            strReportTitle = " Excursion  Report"



            strFilename = rnd.Next.ToString()
            strFullpath = Server.MapPath(".")
            strFullpath += "\SavedReports\" + strFilename + ".pdf"

            Dim rep As New ReportDocument
            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue
            Dim ConnInfo As New ConnectionInfo
            Dim frommail As String

            With ConnInfo
                .ServerName = Session("dbServerName")
                .DatabaseName = Session("dbDatabaseName")
                .UserID = Session("dbUserName")
                .Password = Session("dbPassword")
            End With

            ' rep.Load(Server.MapPath("~\Report\rptConfirmationnew.rpt"))


            reportoption = ViewState("reportoption")

            Select Case ddlreptype.SelectedIndex 'Request.QueryString("reporttype")
                Case 0
                    Select Case chkstatus()  'Request.QueryString("chkstatus")
                        Case 0
                            If chktrfreq.Checked = 1 Then
                                rep.Load(Server.MapPath("~\Report\ExcReglst_det_trfreq.rpt"))
                            Else
                                rep.Load(Server.MapPath("~\Report\ExcReglst_detnew.rpt"))
                            End If

                        Case 1
                            rep.Load(Server.MapPath("~\Report\ExcReglst_detnew.rpt"))
                        Case 2
                            rep.Load(Server.MapPath("~\Report\excreglst_detcostnew.rpt"))
                        Case 4
                            rep.Load(Server.MapPath("~\Report\excreglst_detcommcostnew.rpt"))
                    End Select
                Case 1
                    rep.Load(Server.MapPath("~\Report\excreglst_detnormal_worate.rpt"))
                Case 2
                    rep.Load(Server.MapPath("~\Report\excreglst_detcommcost_unlink.rpt"))
                Case 3
                    rep.Load(Server.MapPath("~\Report\Excsafari_detnew.rpt"))
            End Select




            Dim RepTbls As Tables = rep.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            rep.SummaryInfo.ReportTitle = strReportTitle

            pnames = rep.DataDefinition.ParameterFields

            pname = pnames.Item("Conm")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("rephead")
            paramvalue.Value = strReportTitle
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("repfilter")
            paramvalue.Value = strrepfilter
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("pexcfrm")
            paramvalue.Value = strexcproviderfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pexcto")
            paramvalue.Value = strexcproviderto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("photfrm")
            paramvalue.Value = strhotelcodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("photto")
            paramvalue.Value = strhotelcodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("pddtfrm")
            paramvalue.Value = Format(CType(strfromdate, Date), "yyyy-MM-dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pddtto")
            paramvalue.Value = Format(CType(strtodate, Date), "yyyy-MM-dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("pspfrm")
            paramvalue.Value = strspfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pspto")
            paramvalue.Value = strspto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("dateyn")
            paramvalue.Value = IIf(radtourrange.Checked = True, "Yes", "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ppayfrn")
            paramvalue.Value = strpaytermsfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ppayto")
            paramvalue.Value = strpaytermsto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ptourdtfrm")
            paramvalue.Value = Format(CType(strtourfrom, Date), "yyyy-MM-dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ptourdtto")
            paramvalue.Value = Format(CType(strtourto, Date), "yyyy-MM-dd") '"date(" & Mid(DTPfrom1.value, 7, 4) & "," & Mid(DTPfrom1.value, 4, 2) & "," & Mid(DTPfrom1.value, 1, 2) & ")" 'Format(CType(Request.QueryString("tourto"), Date), "yyyy/MM/dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("date")
            paramvalue.Value = IIf(radreqrange.Checked = True, "Yes", "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("gmode")
            paramvalue.Value = strgroupby()
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("mbasecurr")
            paramvalue.Value = ""
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("pmktfrm")
            paramvalue.Value = strmarketcodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pmktto")
            paramvalue.Value = strmarketcodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("pcntfrm")
            paramvalue.Value = strclientcodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcntto")
            paramvalue.Value = strclientcodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("opt")
            paramvalue.Value = IIf(strmulexctype <> "", 1, 0)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excursion")
            paramvalue.Value = IIf(strmulexctype <> "", Trim(Session("mulselno")), excursionno)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("prepaid")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcltfrm")
            paramvalue.Value = strcollectfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcltto")
            paramvalue.Value = strcollectto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pexcpfrm")
            paramvalue.Value = strexcproviderfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("pexcpto")
            paramvalue.Value = strexcproviderto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("lang")
            paramvalue.Value = ddllang.SelectedIndex
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("pcityfrm")
            paramvalue.Value = strcitycodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcityto")
            paramvalue.Value = strcitycodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excgroupfrom")
            paramvalue.Value = strexcgpcodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excgroupto")
            paramvalue.Value = strexcgpcodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("partyfrom")
            paramvalue.Value = strspfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("partyto")
            paramvalue.Value = strspto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("mainfrom")
            paramvalue.Value = strmaingpcodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("mainto")
            paramvalue.Value = strmaingpcodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("dmc")
            paramvalue.Value = ddldmc.SelectedIndex
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("driverfrom")
            paramvalue.Value = strdriverfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("driverto")
            paramvalue.Value = strdriverto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("officefrom")
            paramvalue.Value = strsalesfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("officeto")
            paramvalue.Value = strsalesto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("spersonfrom")
            paramvalue.Value = strsapcodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("spersonto")
            paramvalue.Value = strsapcodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("operatorfrom")
            paramvalue.Value = stroperatorfrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("operatorto")
            paramvalue.Value = stroperatorto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("tourguidefrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = strtourguidefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("tourguideto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = strtourguideto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("othsellcodefrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = strselltypecodefrom
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("othsellcodeto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = strselltypecodeto
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)





            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            rep.ExportToDisk(ExportFormatType.PortableDocFormat, strFullpath)
            rep.Dispose()

            Dim agentref As String = ""
            Dim Strsubject As String = ""
            Dim strMessage As String = ""
            'agentref = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select agentref  from reservation_headernew where requestid='" + CType(Session("RequestId"), String) + "'")
            'If agentref <> "" Then
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "alert", "alert('Are You Sure to Send Mail to ');", True)
            '    strSubject = "Booking Ref.no:" + CType(Session("RequestId"), String) + " - Ref.No : " + agentref
            'Else
            '    strSubject = "Booking Ref.no:" + CType(Session("RequestId"), String)
            'End If
            Strsubject = " Excursion Report" + strrepfilter

            strMessage = "Dear Team " + "&nbsp;<br /><br />&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;"
            strMessage += "Please find attached Excursion Details " + "<br /><br />"
            strMessage += "<br />Regards<br />" + CType(Session("GlobalUserName"), String) + "<br />" + "<br />" + CType(Session("ComapnyName"), String)


            'frommail = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 563)
            frommail = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select usemail  from UserMaster where UserCode='" + CType(Session("GlobalUserName"), String) + "'")




            'If objEmail.SendEmail(frommail, ToAdd, strSubject, strMessage, strFullpath) Then
            If Trim(frommail) <> "" Then
                'strToCC = frommail

                If objEmail.SendCDOMessage(strFullpath, frommail, emailid, frommail, Strsubject, strMessage) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + emailid + "');", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + emailid + "');", True)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Email Id doesnt exists to " + CType(Session("GlobalUserName"), String) + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptrReservationRequest.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            Dim thefile As FileInfo = New FileInfo(strFullpath)
            'If thefile.Exists Then
            '    File.Delete(strFullpath)
            'End If
        End Try
        SendMailConfirmnew = ""
    End Function
#End Region

    Protected Sub btnemail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnemail.Click
        Try
            If ValidatePage() = True Then

                If txtemail.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email ID field can not be blank.');", True)


                    Exit Sub
                End If

                SendMailConfirmnew(txtemail.Text)






            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptTransfercostPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
End Class

