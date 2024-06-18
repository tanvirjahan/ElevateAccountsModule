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
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode, sectorname from sectormaster where active=1 order by sectorcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname, sectorcode from sectormaster where active=1 order by sectorname", True)

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

                If Request.QueryString("catcode") <> "" Then
                    ddlCatName.Value = Request.QueryString("catcode")
                    ddlCCode.Value = ddlCatName.Items(ddlCatName.SelectedIndex).Text
                End If
                If Request.QueryString("citycode") <> "" Then
                    ddlCityName.Value = Request.QueryString("citycode")
                    ddlCityCode.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text
                End If
                If Request.QueryString("ctrycode") <> "" Then
                    ddlcontName.Value = Request.QueryString("ctrycode")
                    ddlContCode.Value = ddlcontName.Items(ddlcontName.SelectedIndex).Text
                End If
                If Request.QueryString("sectorcode") <> "" Then
                    ddlSectorName.Value = Request.QueryString("sectorcode")
                    ddlSectorCode.Value = ddlSectorName.Items(ddlSectorName.SelectedIndex).Text
                End If
                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                If Request.QueryString("orderby") <> "" Then
                    If Request.QueryString("orderby") = "0" Then rbcode.Checked = True
                    If Request.QueryString("orderby") = "1" Then rbname.Checked = True
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

                    ddlContCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcontName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSectorCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSectorName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

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

            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode, sectorname from sectormaster where active=1 order by sectorcode", True, ddlSectorName.Value)
            objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname, sectorcode from sectormaster where active=1 order by sectorname", True, ddlSectorCode.Value)

            ddlCityName.Value = hdncitycode.Value
            ddlCityCode.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text

            ddlSectorName.Value = hdnsectorcode.Value
            ddlSectorCode.Value = ddlSectorName.Items(ddlSectorName.SelectedIndex).Text



        End If


    End Sub

    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        ddlCCode.Value = "[Select]"
        ddlCatName.Value = "[Select]"
        ddlContCode.Value = "[Select]"
        ddlcontName.Value = "[Select]"
        ddlCityCode.Value = "[Select]"
        ddlCityName.Value = "[Select]"
        ddlSectorCode.Value = "[Select]"
        ddlSectorName.Value = "[Select]"

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

                Dim strcatcode As String = ""
                Dim strcitycode As String = ""
                Dim strctrycode As String = ""
                Dim strsectorcode As String = ""
                Dim strfromdate As String = ""
                Dim strtodate As String = ""
                Dim strorderby As String = ""

                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""

                strcatcode = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "")
                strctrycode = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "")
                strcitycode = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "")
                strsectorcode = IIf(UCase(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text, "")
                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)

                strReportTitle = "New Clients"
                strrepfilter = "Order By : " + IIf(rbcode.Checked, "Client Code", "Client Name")
                strorderby = IIf(rbcode.Checked, 0, 1)

                'Session.Add("catcode", strcatcode)
                'Session.Add("citycode", strcitycode)
                'Session.Add("ctrycode", strctrycode)
                'Session.Add("sectorcode", strsectorcode)
                'Session.Add("fromdate", strfromdate)
                'Session.Add("todate", strtodate)
                'Session.Add("orderby", strorderby)

                'Session.Add("repfilter", strrepfilter)

                'Session.Add("ReportTitle", strReportTitle)

                'Response.Redirect("NewclientsReport.aspx", False)
                'Response.Redirect("NewclientsReport.aspx?catcode=" & strcatcode & "&citycode=" & strcitycode _
                '& "&ctrycode=" & strctrycode & "&sectorcode=" & strsectorcode & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&orderby=" & strorderby & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)
                Dim strpop As String = ""
                'strpop = "window.open('NewclientsReport.aspx?Pageame=Newclient&BackPageName=NewclientSearch.aspx&catcode=" & strcatcode & "&citycode=" & strcitycode _
                '& "&ctrycode=" & strctrycode & "&sectorcode=" & strsectorcode & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&orderby=" & strorderby & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                strpop = "window.open('NewclientsReport.aspx?Pageame=Newclient&BackPageName=NewclientSearch.aspx&catcode=" & strcatcode & "&citycode=" & strcitycode _
                & "&ctrycode=" & strctrycode & "&sectorcode=" & strsectorcode & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&orderby=" & strorderby & "&repfilter=" & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepNewClient');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("NewclientsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim MyCommand As SqlCommand
        Dim SqlConn1 As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim ObjDate As New clsDateTime

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try
            If ValidatePage() = True Then

                SqlConn1 = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

                MyCommand = New SqlCommand("sp_rep_newclient", SqlConn1)
                MyCommand.CommandType = CommandType.StoredProcedure

                MyCommand.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = IIf(UCase(ddlCCode.Items(ddlCCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCCode.Items(ddlCCode.SelectedIndex).Text, "") 'CType(ddlCCode.Items(ddlCCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = IIf(UCase(ddlContCode.Items(ddlContCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlContCode.Items(ddlContCode.SelectedIndex).Text, "") 'CType(ddlContCode.Items(ddlContCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = IIf(UCase(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, "") 'CType(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = IIf(UCase(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text, "") 'CType(ddlSectorCode.Items(ddlSectorCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10) 'ObjDate.ConvertDateromTextBoxToDatabase(dtpFromDate.txtDate.Text)
                MyCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10) 'ObjDate.ConvertDateromTextBoxToDatabase(dtptodate.txtDate.Text)

                myDataAdapter = New SqlDataAdapter(MyCommand)

                myDataAdapter.Fill(myDS)
                gv_SearchResult.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then
                    gv_SearchResult.DataBind()
                Else
                    gv_SearchResult.PageIndex = 0
                    gv_SearchResult.DataBind()
                    lblMsg.Visible = True
                    lblMsg.Text = "Records not found, Please redefine search criteria."
                End If
            End If
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
        FillGrid("custcode")
    End Sub


    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid(Session("strsortexpression"))
    End Sub

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        'FillGrid(e.SortExpression, e.SortDirection)
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objutils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objutils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=NewclientsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
