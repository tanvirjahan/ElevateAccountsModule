Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization



Partial Class PriceListModule_TrfPricelistSellingRates
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
        ViewState.Add("TrfPLsellingcodState", Request.QueryString("State"))
        ViewState.Add("TrfPLsellingcodRefCode", Request.QueryString("RefCode"))

        ' Me.Title = "Price List - Selling Rates"
        Page.Title = Page.Title + " " + "Transfer Price List - Selling Rates"

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
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPCode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)

                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierACode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)
                If ViewState("TrfPLsellingcodRefCode") <> Nothing Then
                    'txtBlockCode.Value = Request.QueryString("PListCode")
                    txtPLCode.Text = ViewState("TrfPLsellingcodRefCode") 'Request.QueryString("PListCode")
                End If

                If Request.QueryString("supplier") <> Nothing Then
                    ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text = Request.QueryString("supplier")
                    GetWeekEndValues(Request.QueryString("supplier"))
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
                ShowMarket(CType(txtPLCode.Text, String))

                'Dim marketstr() As String
                'Dim lblcode As Label
                'Dim chksel As CheckBox
                'If Request.QueryString("Market") <> Nothing Then
                '    marketstr = Request.QueryString("Market").ToString.Split(";")
                '    For i = 0 To marketstr.GetUpperBound(0)
                '        For Each Me.gvRow1 In gv_Market.Rows
                '            lblcode = gvRow1.FindControl("lblcode")
                '            chksel = gvRow1.FindControl("chkSelect")
                '            If marketstr(i).Trim = lblcode.Text.Trim Then
                '                chksel.Checked = True
                '            End If
                '        Next
                '    Next

                'End If

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

                ddlServerType.Items.Clear()
                ddlServerType.Items.Add("[Select]")
                ddlServerType.Items.Add("Arrival Borders")
                ddlServerType.Items.Add("Departure Borders")
                ddlServerType.Items.Add("Internal Transfer/Excursion")
                ddlServerType.Items.Add("Arrival/Departure Transfer Borders")
                If Request.QueryString("transfertype") <> Nothing Then
                    ddlServerType.SelectedIndex = CType(Request.QueryString("transfertype"), Integer)
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TrfPricelistSellingRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
        ShowSellingRadioBtns(ViewState("TrfPLsellingcodRefCode"))
        fillDategrd(grdDates, True)
        ShowDates(ViewState("TrfPLsellingcodRefCode"))
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
        ddlServerType.Enabled = False
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
            strQry = "select distinct a.selltype as sellcode,b.plgrpcode as plgrpcode  from trfplist_selld  a inner join sellmast b on a.selltype=b.sellcode where  a.tplistcode='" & RefCode & "' and a.selltype<>'NET COST' and b.plgrpcode in (" & mktlist & ")  "
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
            strSqlQry = strSqlQry & " and plgrpcode in (select plgrpcode from trfplist_selld  where tplistcode  ='" + txtPLCode.Text.Trim + "' )"
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
            objUtils.WritErrorLog("TrfPricelistSellingRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
    End Sub
#End Region

#Region "Private Sub ShowMarket(ByVal RefCode As String)"
    Private Sub ShowMarket(ByVal RefCode As String)
        Try
            Dim chkSel As CheckBox
            Dim lblcode As Label
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trfplisth_market  Where tplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        For Each Me.gvRow1 In gv_Market.Rows
                            chkSel = gvRow1.FindControl("chkSelect")
                            lblcode = gvRow1.FindControl("lblcode")
                            If CType(mySqlReader("plgrpcode"), String) = CType(lblcode.Text, String) Then
                                chkSel.Checked = True
                                Exit For
                            End If
                        Next
                    End If
                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
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
            Dim strQry As String

            If ValidateGrid() = False Then
                Exit Sub
            End If
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim chksel As CheckBox
            Dim marketstr As String = ""
            Dim lblcode As Label

            'For Each Me.gvRow1 In gv_Market.Rows
            '    chksel = gvRow1.FindControl("chkSelect")
            '    lblcode = gvRow1.FindControl("lblcode")
            '    If chksel.Checked = True Then
            '        marketstr = marketstr + ";" + lblcode.Text
            '    End If
            'Next


            'If marketstr.Length > 0 Then
            '    marketstr = marketstr.Substring(1, marketstr.Length - 1)
            'End If

            strQry = "select distinct a.plgrpcode   from trfplist_selld  a inner join sellmast b on a.selltype=b.sellcode where  a.tplistcode='" & txtPLCode.Text & "' and a.selltype<>'NET COST' and b.sellcode = '" + lblselling.Text + "' "
            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    marketstr = ds.Tables(0).Rows(0)("plgrpcode").ToString()
                End If
            End If


            'Response.Redirect("HederRsellingcode.aspx?supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "&sellcode=" & rdSellingList.SelectedValue & "&PListCoVde=" & txtBlockCode.Value, False)
            Response.Redirect("TrfPricelistSellingRates1.aspx?State=" & CType(ViewState("TrfPLsellingcodState"), String) & "&PListCode=" & txtPLCode.Text & "&sellcode=" & lblselling.Text &
                              "&RefCode=" & CType(ViewState("TrfPLsellingcodRefCode"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
                              "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPCode.Items(ddlSPCode.SelectedIndex).Text &
                              "&SupplierTypeName=" & ddlSPName.Items(ddlSPName.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text &
                              "&SupplierAgentName=" & ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyCode.Value &
                              "&CurrencyName=" & ddlCurrencyCode.Value & "&SubSeasonCode=" & ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text &
                              "&SubSeasonName=" & ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text &
                              "&transfertype=" & ddlServerType.SelectedIndex, False)
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
        Dim obj As New EncryptionDecryption
        Dim chksel As CheckBox
        Dim marketstr As String = ""
        Dim marketstrdis As String = ""
        Dim lblcode As Label

        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            lblcode = gvRow1.FindControl("lblcode")
            If chksel.Checked = True Then
                marketstr = marketstr + ";" + lblcode.Text
                marketstrdis = marketstrdis + "'" + lblcode.Text.Trim + "'" + ","
            End If
        Next


        If marketstr.Length > 0 Then
            marketstr = marketstr.Substring(1, marketstr.Length - 1)

        End If
        If marketstrdis.Length > 0 Then

            marketstrdis = marketstrdis.Substring(0, marketstrdis.Length - 1)
        End If


        'Response.Redirect("TrfPriceList2.aspx?State=Edit&RefCode=" & CType(ViewState("TrfPLsellingcodRefCode"), String) & "&supplier=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text &
        '                  "&suppliername=" & ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text & "&SupplierType=" & ddlSPCode.Items(ddlSPCode.SelectedIndex).Text &
        '                  "&SupplierTypeName=" & ddlSPName.Items(ddlSPName.SelectedIndex).Text & "&Market=" & marketstr & "&SuppierAgent=" & ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text &
        '                  "&SupplierAgentName=" & ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text & "&CurrencyCode=" & ddlCurrencyName.Value &
        '                  "&CurrencyName=" & ddlCurrencyCode.Value & "&SubSeasonCode=" & ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text &
        '                  "&SubSeasonName=" & ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text &
        '                   "&transfertype=" & ddlServerType.SelectedIndex, False)

        Dim SptypeCode As String = obj.Encrypt(CType(ddlSPCode.Items(ddlSPCode.SelectedIndex).Text, String), "&%#@?,:*")
        Dim SptypeName As String = obj.Encrypt(CType(ddlSPName.Items(ddlSPName.SelectedIndex).Text, String), "&%#@?,:*")
        Dim SupplierCode As String = obj.Encrypt(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String), "&%#@?,:*")
        Dim SupplierName As String = obj.Encrypt(CType(ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text, String), "&%#@?,:*")
        Dim PlcCode As String = obj.Encrypt(CType(txtPLCode.Text, String), "&%#@?,:*")
        Dim currcode As String = obj.Encrypt(CType(ddlCurrencyName.Value, String), "&%#@?,:*")
        Dim currname As String = obj.Encrypt(CType(ddlCurrencyCode.Value, String), "&%#@?,:*")
        Dim SubSeasCode As String = obj.Encrypt(CType(ddlSubSeasonCode.Items(ddlSubSeasonCode.SelectedIndex).Text, String), "&%#@?,:*")
        Dim SubSeasName As String = obj.Encrypt(CType(ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text, String), "&%#@?,:*")
        Dim SupplierAgentCode As String = obj.Encrypt(CType(ddlSupplierACode.Items(ddlSupplierACode.SelectedIndex).Text, String), "&%#@?,:*")
        Dim SupplierAgentName As String = obj.Encrypt(CType(ddlSupplierAName.Items(ddlSupplierAName.SelectedIndex).Text, String), "&%#@?,:*")
        Dim flag As String = "selling"


        Response.Redirect("TrfPriceList2.aspx?State=Edit&RefCode=" & CType(ViewState("TrfPLsellingcodRefCode"), String) & "&SupplierCode=" & SupplierCode &
                          "&SupplierName=" & SupplierName &
                          "&Market=" & marketstr & "&SupplierAgentCode=" & SupplierAgentCode &
                          "&SupplierAgentName=" & SupplierAgentName & "&currcode=" & currcode &
                          "&currname=" & currname & "&SubSeasCode=" & SubSeasCode &
                          "&SubSeasName=" & SubSeasName &
                           "&transfertype=" & ddlServerType.SelectedIndex & "&SptypeCode=" & SptypeCode & "&SptypeName=" & SptypeName & "&flag=" & flag & "&marketdis=" & marketstrdis, False)

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
        strscript = "window.opener.__doPostBack('TrfPriceListWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=HeaderRsellingcode1','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Private Sub GetWeekEndValues(ByVal PCode As String)

        '' Dim strCtryCode As String = ""
        'Dim strFromValue As String = ""
        'Dim strToValue As String = ""
        ''strCtryCode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"partymast", "ctrycode", "partycode", PCode)
        ''select wkfrmday1,wktoday1,wkfrmday2,wktoday2 from partymast where partycode=

        'If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday1", "partycode", PCode)) = False Then
        '    strFromValue = " From :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday1", "partycode", PCode)
        'End If
        'If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday1", "partycode", PCode)) = False Then
        '    strFromValue = strFromValue & " ,To :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday1", "partycode", PCode)
        'End If
        'If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday2", "ctrycode", PCode)) = False Then
        '    strToValue = "From :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wkfrmday2", "partycode", PCode)
        'End If
        'If IsDBNull(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday2", "partycode", PCode)) = False Then
        '    strToValue = strToValue & " , To :" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "wktoday2", "partycode", PCode)
        'End If
        'lblWEO1.Text = strFromValue
        'lblWEO2.Text = strToValue


        ''lblWeekEnd1.Text = strFromValue
        ''lblWeekEnd2.Text = strToValue

    End Sub

    Private Sub ShowDates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trfplisth_dates Where tplistcode='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("TrfPricelistSellingRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If ViewState("TrfPLsellingcodRefCode") <> Nothing Then
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select count(*) from trfplisth_dates where tplistcode='" + ViewState("TrfPLsellingcodRefCode") + "'"
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
