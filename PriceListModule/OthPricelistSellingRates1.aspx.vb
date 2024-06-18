Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization

Partial Class PriceListModule_OthPricelistSellingRates1
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As SqlDataAdapter
    Dim Table As New DataTable()
    Dim ParameterArray As New ArrayList()
    Private dt As New DataTable
    Private cnt As Long
    Private arr(1) As String
    Private arrRName(1) As String
    Dim GvRow As String
    Dim gvRow1 As GridViewRow
    Dim dpFDate As New TextBox
    Dim dpTDate As New TextBox
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        'Dim ObjDate As New clsDateTime
        'Try
        '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        '    mySqlCmd = New SqlCommand("Select * from cplisthnew Where plistcode='" & RefCode & "'", mySqlConn)
        '    mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        '    If mySqlReader.HasRows Then
        '        If mySqlReader.Read() = True Then
        '            If IsDBNull(mySqlReader("plistcode")) = False Then
        '                txtBlockCode.Value = mySqlReader("plistcode")
        '                txtPLCode.Text = mySqlReader("plistcode")
        '            End If
        '            If IsDBNull(mySqlReader("sptypecode")) = False Then
        '                ddlSPName.Value = mySqlReader("sptypecode")
        '                ddlSPCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))
        '            End If
        '            If IsDBNull(mySqlReader("supagentcode")) = False Then
        '                ddlSupplierAName.Value = mySqlReader("supagentcode")
        '                ddlSupplierACode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentname", "supagentcode", mySqlReader("supagentcode"))
        '            End If
        '            If IsDBNull(mySqlReader("partycode")) = False Then
        '                ddlSupplierName.Value = mySqlReader("partycode")
        '                ddlSupplierCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("partycode"))
        '                GetWeekEndValues(mySqlReader("partycode"))
        '            End If
        '            If IsDBNull(mySqlReader("subseascode")) = False Then
        '                ddlSubSeasonName.Value = mySqlReader("subseascode")
        '                ddlSubSeasonCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
        '            End If
        '            If IsDBNull(mySqlReader("currcode")) = False Then
        '                ddlCurrencyName.Value = mySqlReader("currcode")
        '                ddlCurrencyCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
        '            End If
        '            If IsDBNull(mySqlReader("revisiondate")) = False Then
        '                dpRevDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("revisiondate")), String)
        '            End If
        '            If IsDBNull(mySqlReader("weekend2")) = False Then
        '                If mySqlReader("weekend2") = 1 Then
        '                    ChkWeek2.Checked = True
        '                ElseIf mySqlReader("weekend2") = 0 Then
        '                    ChkWeek2.Checked = False
        '                End If
        '            End If
        '            If IsDBNull(mySqlReader("weekend1")) = False Then
        '                If mySqlReader("weekend1") = 1 Then
        '                    ChkWeek1.Checked = True
        '                ElseIf mySqlReader("weekend1") = 0 Then
        '                    ChkWeek1.Checked = False
        '                End If
        '            End If
        '            If IsDBNull(mySqlReader("plist_mode")) = False Then
        '                If mySqlReader("plist_mode") = 1 Then
        '                    ChkBManual.Checked = True
        '                ElseIf mySqlReader("plist_mode") = 0 Then
        '                    ChkBManual.Checked = False
        '                End If
        '            End If
        '            If IsDBNull(mySqlReader("plisttype")) = False Then
        '                If mySqlReader("plisttype") = "0" Then
        '                    ddlPriceList.SelectedValue = "Normal Rates 1 Night"
        '                    ChkWeek1.Visible = False
        '                    ChkWeek2.Visible = False
        '                    lblWEO1.Visible = False
        '                    lblWEO2.Visible = False
        '                ElseIf mySqlReader("plisttype") = "1" Then
        '                    ddlPriceList.SelectedValue = "Weekly Rates 7 Nights"
        '                    ChkWeek1.Visible = False
        '                    ChkWeek2.Visible = False
        '                    lblWEO1.Visible = False
        '                    lblWEO2.Visible = False
        '                ElseIf mySqlReader("plisttype") = "2" Then
        '                    ddlPriceList.SelectedValue = "Weekend Rates 1 Night"
        '                    ChkWeek1.Visible = True
        '                    ChkWeek2.Visible = True
        '                    lblWEO1.Visible = False
        '                    lblWEO2.Visible = False
        '                ElseIf mySqlReader("plisttype") = "3" Then
        '                    ddlPriceList.SelectedValue = "Normal Rates > 1 Night"
        '                    ChkWeek1.Visible = False
        '                    ChkWeek2.Visible = False
        '                    lblWEO1.Visible = False
        '                    lblWEO2.Visible = False
        '                ElseIf mySqlReader("plisttype") = "4" Then
        '                    ddlPriceList.SelectedValue = "Weekend Rates > 1 Night"
        '                    ChkWeek1.Visible = True
        '                    ChkWeek2.Visible = True
        '                    lblWEO1.Visible = True
        '                    lblWEO2.Visible = True
        '                End If
        '            End If
        '        End If
        '    End If
        '    clsDBConnect.dbReaderClose(mySqlReader)
        '    clsDBConnect.dbCommandClose(mySqlCmd)
        '    clsDBConnect.dbConnectionClose(mySqlConn)
        'Catch ex As Exception
        '    objUtils.WritErrorLog("HederRsellingcode1new.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'Finally
        '    If mySqlConn.State = ConnectionState.Open Then
        '        clsDBConnect.dbConnectionClose(mySqlConn)
        '    End If
        'End Try
    End Sub
#End Region


#Region "Private Sub FillDDL()"
    Private Sub FillDDL()
        strSqlQry = ""
        strSqlQry = "SELECT currcode,currname FROM currmast WHERE active=1 ORDER BY currcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyCode, "currcode", "currname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT currcode,currname FROM currmast WHERE active=1 ORDER BY currname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", strSqlQry, True)

        strSqlQry = ""
        strSqlQry = "SELECT sptypecode,sptypename FROM sptypemast WHERE active=1 ORDER BY sptypecode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPCode, "sptypecode", "sptypename", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT sptypecode,sptypename FROM sptypemast WHERE active=1 ORDER BY sptypename"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPName, "sptypename", "sptypecode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT supagentcode,supagentname FROM supplier_agents WHERE active=1 ORDER BY supagentcode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierACode, "supagentcode", "supagentname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT supagentcode,supagentname FROM supplier_agents WHERE active=1 ORDER BY supagentname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAName, "supagentname", "supagentcode", strSqlQry, True)

        strSqlQry = ""
        strSqlQry = "SELECT partycode,partyname FROM partymast WHERE active=1 ORDER BY partycode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT partycode,partyname FROM partymast WHERE active=1 ORDER BY partyname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT subseascode,subseasname FROM subseasmast WHERE active=1 ORDER BY subseascode"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonCode, "subseascode", "subseasname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "SELECT subseascode,subseasname FROM subseasmast WHERE active=1 ORDER BY subseasname"
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonName, "subseasname", "subseascode", strSqlQry, True)

    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ViewState.Add("HedrsellingcodState", Request.QueryString("State"))
        'ViewState.Add("HedrsellingcodRefCode", Request.QueryString("RefCode"))
        ViewState.Add("OthPLsellingcodState", Request.QueryString("State"))
        ViewState.Add("OthPLsellingcodRefCode", Request.QueryString("RefCode"))

        ' Me.Title = "Price List - Selling Rates"
        Dim strOption As String = ""
        Dim strtitle As String = ""
        Dim strSPType As String = ""

        If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then

            strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthPListFilter"))
            Select Case strOption
                Case "CAR RENTAL"
                    strtitle = "Car Rental"
                    'strSPType = "1031"
                    'lblHeading.Text = "Car Rental Selling Formula"
                Case "VISA"
                    strtitle = "Visa "
                    'strSPType = "1032"
                    'lblHeading.Text = "Visa Selling Formula"
                Case "EXC"
                    strtitle = "Excursion  "
                    'strSPType = "1033"
                    'lblHeading.Text = "Excursion Selling Formula"
                Case "MEALS"
                    strtitle = "Restaurant  "
                    'strSPType = "1034"
                    'lblHeading.Text = "Restaurant Selling Formula"
                Case "GUIDES"
                    strtitle = "Guide  "
                    'strSPType = "1035"
                    'lblHeading.Text = "Guide Selling Formula"
                Case "ENTRANCE"
                    strtitle = "Entrance "
                    'strSPType = "1036"
                    'lblHeading.Text = "Guide Selling Formula"
                Case "JEEPWADI"
                    strtitle = "Jeepwadi "
                    'strSPType = "1037"
                    'lblHeading.Text = "Guide Selling Formula"
                Case "HFEES"
                    strtitle = "Handling Fee"

                Case "AIRPORTMA"
                    strtitle = "Airport Meet & Assist"

            End Select
            'strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
            '    " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

        ElseIf Session("OthPListFilter") = "OTH" Then
            strtitle = "Other Service "
            'strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
            '    " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025)) order by othgrpcode"
        End If

        Page.Title = Page.Title + " " + strtitle + " Price List - Selling Rates"
        lblheading.Text = strtitle + " Price List - Selling Rates"
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                End If
                txtconnection.Value = Session("dbconnectionName")
                FillDDL()
                'ShowRecord(CType(Session("SesionPlistCode"), String))
                ' DisableControl()
                'btnSave.Visible = False
                Session("PlistSaved") = False
                
                If ViewState("OthPLsellingcodRefCode") <> Nothing Then
                    'txtBlockCode.Value = Request.QueryString("PListCode")
                    txtPLCode.Text = ViewState("OthPLsellingcodRefCode") 'Request.QueryString("PListCode")
                End If

                If Request.QueryString("supplier") <> Nothing Then
                    ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text = Request.QueryString("supplier")
                    'GetWeekEndValues(Request.QueryString("supplier"))
                End If
                If Request.QueryString("suppliername") <> Nothing Then
                    ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text = Request.QueryString("suppliername")
                End If
                If Request.QueryString("SupplierType") <> Nothing Then
                    ddlSPCode.Items(ddlSPCode.SelectedIndex).Text = Request.QueryString("SupplierType")
                End If
                If Request.QueryString("SupplierTypeName") <> Nothing Then
                    ddlSPName.Items(ddlSPName.SelectedIndex).Text = Request.QueryString("SupplierTypeName")
                End If

                FillGridMarket("plgrpcode")

                Dim marketstr() As String
                Dim lblcode As Label
                Dim chksel As CheckBox
                If Request.QueryString("Market") <> Nothing Then
                    marketstr = Request.QueryString("Market").ToString.Split(";")
                    For i = 0 To marketstr.GetUpperBound(0)
                        For Each Me.gvRow1 In gv_Market.Rows
                            lblcode = gvRow1.FindControl("lblcode")
                            chksel = gvRow1.FindControl("chkSelect")
                            If marketstr(i).Trim = lblcode.Text.Trim Then
                                chksel.Checked = True
                            End If
                        Next
                    Next

                End If

                If Request.QueryString("SuppierAgent") <> Nothing Then
                    ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text = Request.QueryString("SuppierAgent")
                End If
                If Request.QueryString("SupplierAgentName") <> Nothing Then
                    ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text = Request.QueryString("SupplierAgentName")
                End If

                If Request.QueryString("CurrencyCode") <> Nothing Then
                    ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = Request.QueryString("CurrencyCode")
                End If
                If Request.QueryString("CurrencyName") <> Nothing Then
                    ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = Request.QueryString("CurrencyName")
                End If
                If Request.QueryString("SubSeasonCode") <> Nothing Then
                    ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text = Request.QueryString("SubSeasonCode")
                End If
                If Request.QueryString("SubSeasonName") <> Nothing Then
                    ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text = Request.QueryString("SubSeasonName")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OthPricelistSellingRates1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
        ShowSellingRadioBtns(ViewState("OthPLsellingcodRefCode"))
        fillDategrd(grdDates, True)
        ShowDates(ViewState("OthPLsellingcodRefCode"))
    End Sub

#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("no", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        txtPLCode.Enabled = False
        ddlSPCode.Disabled = True
        ddlSPName.Disabled = True
        ddlCurrencyCode.Disabled = True
        ddlCurrencyName.Disabled = True
        ddlSupplierACode.Disabled = True
        ddlSupplierAName.Disabled = True
        ddlSubSeasonCode.Disabled = True
        ddlSubSeasonName.Disabled = True
        ddlSupplierCode.Disabled = True
        ddlSupplierName.Disabled = True
        'ddlPromotion.Disabled = True
        'ChkWeek1.Enabled = False
        'ChkWeek2.Enabled = False
        gv_Market.Enabled = False
        'lblWEO1.Visible = False
        'lblWEO2.Visible = False
        'If ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
        '    lblWEO1.Visible = True
        '    lblWEO2.Visible = True
        'Else
        '    lblWEO1.Visible = False
        '    lblWEO2.Visible = False
        'End If

        ' ChkBManual.Enabled = False
        'ddlPriceList.Visible = False
        'dpRevDate.Visible = False

    End Sub
#End Region

#Region "Private Sub DisableControl1()"
    Private Sub DisableControl1()
        Label1.Visible = False
    End Sub

#End Region

    Private Sub ShowSellingRadioBtns(ByVal RefCode As String)
        Dim mktlist As String = ""
        Dim strQry As String = ""
        'Dim chkSel As CheckBox
        'Dim lblcode As Label
        'For Each GvRow1 In gv_Market.Rows
        '    chkSel = gvRow1.FindControl("chkSelect")
        '    lblcode = gvRow1.FindControl("lblcode")
        '    If chkSel.Checked = True Then
        '        If mktlist = "" Then
        '            mktlist = GvRow1.Cells(1).Text
        '        Else
        '            mktlist = mktlist & "','" & lblcode.Text.Trim
        '        End If
        '    End If
        'Next

        mktlist = getmarket()
        If mktlist <> "" Then
            strQry = "select distinct a.selltype as sellcode,b.plgrpcode as plgrpcode  from othplist_selld  a inner join sellmast b on a.selltype=b.sellcode where  a.oplistcode='" & RefCode & "' and a.selltype<>'NET COST' and b.plgrpcode in (" & mktlist & ")  "
            strQry = strQry & " order by a.selltype"
            '" union all select distinct a.selltype as sellcode,b.plgrpcode as plgrpcode  from cplistdwknew  a inner join sellmast b on a.selltype=b.sellcode where  a.plistcode='" & RefCode & "' and a.selltype<>'NET COST' and b.plgrpcode in (" & mktlist & ") 
            Try

                Dim ds As New DataSet
                Dim i As Integer = 0

                Dim oldgrpcode As String = ""
                Dim tr As New TableRow()
                ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry)
                tbl.EnableViewState = True

                For i = 0 To ds.Tables(0).Rows.Count - 1
                    Dim rb As New RadioButton
                    rb.EnableViewState = True
                    If oldgrpcode <> ds.Tables(0).Rows(i)("plgrpcode").ToString Then
                        tr = New TableRow()
                        oldgrpcode = ds.Tables(0).Rows(i)("plgrpcode").ToString
                    End If
                    Dim td As New TableCell()
                    rb.GroupName = "sellgroup"
                    rb.Text = ds.Tables(0).Rows(i)("sellcode").ToString
                    td.Controls.Add(rb)
                    tr.Cells.Add(td)
                    tbl.Rows.Add(tr)
                Next


                'rdbtnlistSell.DataSource = mySqlReader
                'rdbtnlistSell.DataTextField = ("sellcode")
                'rdbtnlistSell.DataValueField = ("sellcode")
                'rdbtnlistSell.DataBind()
                'mySqlConn.Close()
            Catch ex As Exception
                mySqlConn.Close()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try
        End If
    End Sub

    Public Function ValidatePage() As Boolean
        ValidatePage = False
        Dim objDateTime As New clsDateTime
        Try
            Dim strQry As String = ""
            Dim ds As New DataSet
            Dim chksel As CheckBox
            Dim lblcode As Label
            Dim marketcode As String = ""
            Dim rb As RadioButton
            Dim i As Integer = 0
            Dim isselected As Boolean = False
            lblselling.Text = ""
            For i = 0 To tbl.Rows.Count - 1
                For j = 0 To tbl.Rows(i).Cells.Count - 1
                    rb = CType(tbl.Rows(i).Cells(j).Controls(0), RadioButton)
                    If rb.Checked = True Then
                        isselected = True
                        lblselling.Text = rb.Text
                        Exit For
                    End If
                Next
            Next

            If isselected = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any selling code.');", True)
                ValidatePage = False
                Exit Function
            End If

            'For Each Me.gvRow1 In gv_Market.Rows
            '    chksel = gvRow1.FindControl("chkSelect")
            '    lblcode = gvRow1.FindControl("lblcode")
            '    If chksel.Checked = True Then
            '        marketcode = lblcode.Text.Trim
            '    End If
            'Next

            'strQry = "select plgrpcode from sellmast where sellcode='" & lblselling.Text & "'"
            'ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry)

            'If ds.Tables(0).Rows.Count > 0 Then
            '    If ds.Tables(0).Rows(0)("plgrpcode") <> marketcode Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Market not related to the selected selling type');", True)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            'End If

            ValidatePage = True
        Catch ex As Exception

        End Try
    End Function
    Private Function ValidateGrid() As Boolean
        ValidateGrid = False
        Dim chksel As CheckBox
        Dim lblcode As Label
        Dim grdFlag As Boolean
        Dim cnt As Integer = 0
        grdFlag = False

        Try

            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                If chksel.Checked = True Then
                    grdFlag = True
                    cnt = cnt + 1
                End If
            Next

            If grdFlag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
                ValidateGrid = False
                Exit Function
            End If

            'If cnt > 1 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot select more than one market');", True)
            '    ValidateGrid = False
            '    Exit Function
            'End If


            ValidateGrid = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        'Dim mktstring
        gv_Market.Visible = True
        If gv_Market.PageIndex < 0 Then
            gv_Market.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            '  mktstring = getmarketstring()

            'strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
            'strSqlQry = strSqlQry & " and plgrpcode in (select plgrpcode from cplistdnew  where plistcode ='" + txtPLCode.Text.Trim + "' "
            'strSqlQry = strSqlQry & " union all select plgrpcode from cplistdwknew  where plistcode ='" + txtPLCode.Text.Trim + "') ORDER BY " & strorderby & " " & strsortorder

            strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
            strSqlQry = strSqlQry & " and plgrpcode in (select plgrpcode from othplist_selld  where oplistcode  ='" + txtPLCode.Text.Trim + "' )"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_Market.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Market.DataBind()

            Else
                gv_Market.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricelistSellingRates1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
    End Sub
#End Region

    Private Function getmarket() As String
        Dim chksel As CheckBox
        Dim mktcode As String = ""
        Dim lblcode As Label
        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            lblcode = gvRow1.FindControl("lblcode")
            If chksel.Checked = True Then
                mktcode = mktcode + "'" + lblcode.Text.Trim + "'" + ","
            End If
        Next
        If mktcode.Length > 0 Then
            mktcode = mktcode.Substring(0, mktcode.Length - 1)
        End If

        Return mktcode
    End Function

    Private Function getmarketstring() As String
        Dim chksel As CheckBox
        Dim mktcode As String = ""
        Dim marketstr() As String
        Dim lblcode As Label
        If Request.QueryString("Market") <> Nothing Then
            marketstr = Request.QueryString("Market").ToString.Split(";")
            For i = 0 To marketstr.GetUpperBound(0)
                If marketstr(i).Trim <> "" Then
                    mktcode = mktcode + "'" + marketstr(i).Trim + "'" + ","
                End If
            Next
        End If
        If mktcode.Length > 0 Then
            mktcode = mktcode.Substring(0, mktcode.Length - 1)
        End If

        Return mktcode
    End Function

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim ObjDate As New clsDateTime
        Try
            'Dim frmdate As DateTime

            If ValidateGrid() = False Then
                Exit Sub
            End If
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim chksel As CheckBox
            Dim marketstr As String = ""
            Dim lblcode As Label

            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                lblcode = gvRow1.FindControl("lblcode")
                If chksel.Checked = True Then
                    marketstr = marketstr + ";" + lblcode.Text
                End If
            Next


            If marketstr.Length > 0 Then
                marketstr = marketstr.Substring(1, marketstr.Length - 1)
            End If

            'Response.Redirect("HederRsellingcode.aspx?supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "&sellcode=" & rdSellingList.SelectedValue & "&PListCoVde=" & txtBlockCode.Value, False)
            Response.Redirect("OthPricelistSellingRates2.aspx?State=" & CType(ViewState("OthPLsellingcodState"), String) & "&PListCode=" & txtPLCode.Text & "&sellcode=" & lblselling.Text &
                              "&RefCode=" & CType(ViewState("OthPLsellingcodRefCode"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
                              "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPCode.Items(ddlSPCode.SelectedIndex).Text &
                              "&SupplierTypeName=" & ddlSPName.Items(ddlSPName.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text &
                              "&SupplierAgentName=" & ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCode.Value &
                              "&CurrencyName=" & ddlCurrencyCode.Value & "&SubSeasonCode=" & ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text &
                              "&SubSeasonName=" & ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text, False)
            'End If

            'gv_SearchResult.Visible = True
            'DisableControl()
            'DisableControl1()

            'createdatatable()
            'createdatarows()
            ' gv_SearchResult.Enabled = False
        Catch ex As Exception

        End Try



    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Session("RefCode") = txtBlockCode.Value
        Session("BackPage") = "True"

        Dim chksel As CheckBox
        Dim marketstr As String = ""
        Dim lblcode As Label

        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            lblcode = gvRow1.FindControl("lblcode")
            If chksel.Checked = True Then
                marketstr = marketstr + ";" + lblcode.Text
            End If
        Next


        If marketstr.Length > 0 Then
            marketstr = marketstr.Substring(1, marketstr.Length - 1)
        End If

        Response.Redirect("OthPriceList2.aspx?State=Edit&RefCode=" & CType(ViewState("OthPLsellingcodRefCode"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
                          "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPCode.Items(ddlSPCode.SelectedIndex).Text &
                          "&SupplierTypeName=" & ddlSPName.Items(ddlSPName.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text &
                          "&SupplierAgentName=" & ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCode.Value &
                          "&CurrencyName=" & ddlCurrencyCode.Value & "&SubSeasonCode=" & ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text &
                          "&SubSeasonName=" & ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text, False)
    End Sub

    'Private Sub FillSellingCode()
    '    Dim StrQry As String = "select distinct sellcode from trfplisth_convrates  where tplistcode ='" & CType(Session("SesionPlistCode"), String) & "' order by sellcode"
    '    Try
    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        mySqlCmd = New SqlCommand(StrQry, mySqlConn)
    '        mySqlReader = mySqlCmd.ExecuteReader()
    '        rdSellingList.DataSource = mySqlReader
    '        rdSellingList.DataTextField = ("sellcode")
    '        rdSellingList.DataValueField = ("sellcode")
    '        rdSellingList.DataBind()
    '        mySqlConn.Close()
    '    Catch ex As Exception
    '        mySqlConn.Close()
    '    End Try
    'End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("BackPage") = ""
        'Response.Redirect("PriceList.aspx", False)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('OthPriceListWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthpriceListsellingcode','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

  

    Private Sub ShowDates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othplisth_dates Where oplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromDate")
                        dpTDate = gvRow.FindControl("txtToDate")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                dpFDate.Text = Format("U", mySqlReader("frmdate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = Format("U", mySqlReader("todate")) 'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelistSellingRates1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If ViewState("OthPLsellingcodRefCode") <> Nothing Then
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select count(*) from othplisth_dates where oplistcode='" + ViewState("OthPLsellingcodRefCode") + "'"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            lngcnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()
        End If

        If blnload = True Then
            lngcnt = lngcnt '10
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub

End Class
