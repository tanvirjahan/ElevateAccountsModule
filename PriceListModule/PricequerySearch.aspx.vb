Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PricequerySearch
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

    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


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

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpTypeName, "sptypename", "sptypecode", "select sptypename, sptypecode from sptypemast where active=1 order by sptypename", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMealCode, "mealcode", "mealname", "select mealcode,mealname from mealmast where active=1  order by mealcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMealName, "mealname", "mealcode", "select mealname,mealcode from mealmast where active=1  order by mealname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPartyName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingCode, "sellcode", "sellname", "select sellcode, sellname from sellmast where active=1 order by sellcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "sellname", "sellcode", "select sellname, sellcode from sellmast where active=1 order by sellname", True)

                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeCode, "rmtypcode", "rmtypname", "select rmtypcode,rmtypname from rmtypmast where active=1 order by rmtypcode", True)
                objutils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypename, "rmtypname", "rmtypcode", "select rmtypname,rmtypcode from rmtypmast where active=1 order by rmtypname", True)


                servicetype = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))
                ddlSpTypeName.Value = CType(servicetype, String)
                ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

                If Request.QueryString("sptypecode") <> "" Then
                    ddlSpTypeName.Value = Request.QueryString("sptypecode")
                    ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
                End If
                If Request.QueryString("partycode") <> "" Then
                    ddlPartyName.Value = Request.QueryString("partycode")
                    ddlPartyCode.Value = ddlPartyName.Items(ddlPartyName.SelectedIndex).Text
                End If
                If Request.QueryString("roomtype") <> "" Then
                    ddlRmtypename.Value = Request.QueryString("roomtype")
                    ddlRmtypeCode.Value = ddlRmtypename.Items(ddlRmtypename.SelectedIndex).Text
                End If
                If Request.QueryString("meal") <> "" Then
                    ddlMealName.Value = Request.QueryString("meal")
                    ddlMealCode.Value = ddlMealName.Items(ddlMealName.SelectedIndex).Text
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
                objutils.WritErrorLog("PricequerySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else

            Dim servicetype As String = ""
            If hdnsptypecode.Value = "" Then

                servicetype = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("458", String))
                ddlSpTypeName.Value = CType(servicetype, String)
                ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text
            End If


            ddlSpTypeName.Value = hdnsptypecode.Value
            ddlSPTypeCode.Value = ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text

            strqry = ""
            If ddlSPTypeCode.Value <> "[Select]" Then
                strqry = " and sptypecode='" & ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text & "'"
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

            ddlMealName.Value = hdnmealtype.Value
            ddlMealCode.Value = ddlMealName.Items(ddlMealName.SelectedIndex).Text

            ddlSellingName.Value = hdnselltype.Value

            ddlSellingCode.Value = ddlSellingName.Items(ddlSellingName.SelectedIndex).Text


        End If

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSpTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSellingCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlMealCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlMealName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlPartyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlPartyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlRmtypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRmtypename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepPriceExpWindowPostBack") Then
            BtnPrint_Click(sender, e)
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
        ddlMealCode.Value = "[Select]"
        ddlMealName.Value = "[Select]"
        ddlRmtypeCode.Value = "[Select]"
        ddlRmtypename.Value = "[Select]"

        txtFromDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
        txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))

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
            If ddlPartyCode.Value = "" Or UCase(Trim(ddlPartyCode.Value)) = UCase(Trim("[Select]")) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier can not be blank.');", True)


                SetFocus(ddlPartyCode)
                ValidatePage = False
                Exit Function
            End If
            If ddlRmtypeCode.Value = "" Or UCase(Trim(ddlRmtypeCode.Value)) = UCase(Trim("[Select]")) Then
                SetFocus(ddlRmtypeCode)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Room Type can not be blank.');", True)

                ValidatePage = False
                Exit Function
            End If
            If ddlMealCode.Value = "" Or UCase(Trim(ddlMealCode.Value)) = UCase(Trim("[Select]")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Meal Plan can not be blank.');", True)

                SetFocus(ddlMealCode)
                ValidatePage = False
                Exit Function
            End If

            If ddlMarketCode.Value = "" Or UCase(Trim(ddlMarketCode.Value)) = UCase(Trim("[Select]")) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Market can not be blank.');", True)


                SetFocus(ddlMarketCode)
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
                'Session.Add("Pageame", "Pricequery")
                'Session.Add("BackPageName", "PricequerySearch.aspx")

                Dim strReportTitle As String = ""

                Dim strsptypecode As String = ""

                Dim strpartycode As String = ""
                Dim strroomtype As String = ""
                Dim strmeal As String = ""
                Dim strfromdate As String = ""
                Dim strtodate As String = ""
                Dim strsellcode As String = ""
                Dim strplgrpcode As String = ""
                Dim strasondate As String = ""

                Dim strrepfilter As String = ""
                Dim strreportoption As String = ""



                strsptypecode = IIf(UCase(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, "")

                strpartycode = IIf(UCase(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, "")
                strroomtype = IIf(UCase(ddlRmtypeCode.Items(ddlRmtypeCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlRmtypeCode.Items(ddlRmtypeCode.SelectedIndex).Text, "")
                strmeal = IIf(UCase(ddlMealCode.Items(ddlMealCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMealCode.Items(ddlMealCode.SelectedIndex).Text, "")

                strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
                'Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10)
                strtodate = Format(CType(txtToDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
                'Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)

                strsellcode = IIf(UCase(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text, "")
                strplgrpcode = IIf(UCase(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, "")

                strasondate = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10)

                strReportTitle = "Price Query"

                strreportoption = ""

                'Session.Add("sptypecode", strsptypecode)

                'Session.Add("partycode", strpartycode)
                'Session.Add("roomtype", strroomtype)
                'Session.Add("meal", strmeal)
                'Session.Add("fromdate", strfromdate)
                'Session.Add("todate", strtodate)
                'Session.Add("sellcode", strsellcode)
                'Session.Add("plgrpcode", strplgrpcode)
                'Session.Add("asondate", strasondate)

                'Session.Add("repfilter", strrepfilter)
                'Session.Add("reportoption", strreportoption)

                'Session.Add("ReportTitle", strReportTitle)

                'Response.Redirect("PricequeryReport.aspx?sptypecode=" & strsptypecode & "&partycode=" & strpartycode _
                '& "&roomtype=" & strroomtype & "&meal=" & strmeal & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&sellcode=" & strsellcode & "&plgrpcode=" & strplgrpcode & "&asondate=" & strasondate & "&repfilter=" _
                '& strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle, False)
                Dim strpop As String = ""
                'strpop = "window.open('PricequeryReport.aspx?Pageame=Pricequery&BackPageName=PricequerySearch.aspx&sptypecode=" & strsptypecode & "&partycode=" & strpartycode _
                '& "&roomtype=" & strroomtype & "&meal=" & strmeal & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&sellcode=" & strsellcode & "&plgrpcode=" & strplgrpcode & "&asondate=" & strasondate & "&repfilter=" _
                '& strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepPriceQuery','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                strpop = "window.open('PricequeryReport.aspx?Pageame=Pricequery&BackPageName=PricequerySearch.aspx&sptypecode=" & strsptypecode & "&partycode=" & strpartycode _
                & "&roomtype=" & strroomtype & "&meal=" & strmeal & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&sellcode=" & strsellcode & "&plgrpcode=" & strplgrpcode & "&asondate=" & strasondate & "&repfilter=" _
                & strrepfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strReportTitle & "','RepPriceQuery');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("rptPricequerySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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

                MyCommand = New SqlCommand("sp_get_fprices_new", SqlConn1)
                MyCommand.CommandType = CommandType.StoredProcedure

                MyCommand.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(ddlRmtypeCode.Items(ddlRmtypeCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(ddlMealCode.Items(ddlMealCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = Mid(Format(CType(txtFromDate.Text, Date), "u"), 1, 10) 'ObjDate.ConvertDateromTextBoxToDatabase(dtpFromDate.txtDate.Text)
                MyCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10) 'ObjDate.ConvertDateromTextBoxToDatabase(dtptodate.txtDate.Text)
                MyCommand.Parameters.Add(New SqlParameter("@sellcode", SqlDbType.VarChar, 20)).Value = CType(ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                MyCommand.Parameters.Add(New SqlParameter("@requestdate1", SqlDbType.DateTime)).Value = Mid(Format(CType(txtToDate.Text, Date), "u"), 1, 10) 'ObjDate.ConvertDateromTextBoxToDatabase(dtptodate.txtDate.Text)

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
            objutils.WritErrorLog("PricequerySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    Protected Sub btndisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndisplay.Click
        FillGrid("ratetype")
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PricequerySearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
