'------------================--------------=======================------------------================
'   Module Name    :    OtherServicesPriceListSerach.aspx.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :   31 July 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class OtherServicesPriceListSearch
    Inherits System.Web.UI.Page


#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection


#End Region

#Region "Enum GridCol"
    Enum GridCol
        PriceListCodeTCol = 0
        PriceListCode = 1
        GroupCode = 2
        FromDate = 3
        ToDate = 4
        MarketCode = 5
        SellType = 6
        SubSeason = 7
        Currency = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 13
        View = 14
        Delete = 15
        copy = 16
    End Enum
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then

            Try

                'TextLock(TxtPLCD)
                SetFocus(TxtPLCD)

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
                                             CType(strappname, String), "PriceListModule\OtherServicesPriceListSearch.aspx", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If


                checkIsPrivilege()

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSericeSellType, "othsellcode", "othsellname", "select distinct othsellcode,othsellname from othsellmast where active=1 order by othsellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSericeSellTypeN, "othsellname", "othsellcode", "select distinct othsellname,othsellcode from othsellmast where active=1 order by othsellname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select distinct plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select distinct plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSericeGCode, "othgrpcode", "othgrpname", "select distinct othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherSericeGName, "othgrpname", "othgrpcode", "select distinct othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonCD, "subseascode", "subseasname", "select distinct subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasonName, "subseasname", "subseascode", "select distinct subseasname,subseascode from subseasmast where active=1 order by subseasname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyCode, "currcode", "currname", "select distinct currcode,currname from currmast where active=1 order by currcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", "select distinct currname,currcode from currmast where active=1 order by currname", True)

                Session.Add("strsortExpression", "oplisth.oplistcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                charcters(TxtPLCD)
                'btnAddNew.Visible = True
                'btnExportToExcel.Visible = True
                'btnPrint.Visible = True
                Panel1.Visible = False
                ddlCurrencyCode.Visible = False
                ddlCurrencyName.Visible = False
                ddlSubSeasonCD.Visible = False
                ddlSubSeasonName.Visible = False
                ddlOtherSericeGCode.Visible = False
                ddlOtherSericeGName.Visible = False
                dpFromdate.txtDate.Visible = False
                dpToDate.txtDate.Visible = False
                ddlOtherSericeSellType.Visible = False
                ddlOtherSericeSellTypeN.Visible = False
                'FillGrid("oplistcode")
                FillGridWithOrderByValues()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                    ddlOtherSericeGCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherSericeGName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlOtherSericeSellType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherSericeGName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServicesPriceListSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OtherServicesPriceListWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region

#Region "Public Function checkIsPrivilege() As Boolean"
    Public Function checkIsPrivilege() As Boolean
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='8' and appid='1' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Statusapprove", "Yes")
            Else
                Session.Add("Statusapprove", "No")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try

    End Function
#End Region


#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            '  Session.Add("CurrencyCode", txtblocksale_header.blocksaleid.Text.Trim)
            '   Session.Add("CurrencyName", txtcityname.Text.Trim)
            '   Response.Redirect("rptCurrencies.aspx", False)
            If TxtPLCD.Text = "" Then

                Exit Sub
            End If
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Session("ColReportParams") = Nothing
            Session.Add("Pageame", "Other Services Price List")
            Session.Add("BackPageName", "OtherServicesPriceListSearch.aspx")

            If TxtPLCD.Text.Trim <> "" Then
                strReportTitle = "Block Sale ID : " & TxtPLCD.Text.Trim
                strSelectionFormula = "{blocksale_header.blocksaleid} LIKE '" & TxtPLCD.Text.Trim & "*'"
            End If


            If ddlOtherSericeSellType.Value.Trim <> "[Select]" Then
                If strSelectionFormula <> "" Then
                    strReportTitle = strReportTitle & " ; Other Sell Type Code : " & Trim(CType(ddlOtherSericeSellType.Items(ddlOtherSericeSellType.SelectedIndex).Text, String))
                    strSelectionFormula = strSelectionFormula & " and {othsellmast.othsellcode} = '" & Trim(CType(ddlOtherSericeSellType.Items(ddlOtherSericeSellType.SelectedIndex).Text, String)) & "'"
                Else
                    strReportTitle = "Other Sell Type Code : " & Trim(CType(ddlOtherSericeSellType.Items(ddlOtherSericeSellType.SelectedIndex).Text, String))
                    strSelectionFormula = "{othsellmast.othsellcode} = '" & Trim(CType(ddlOtherSericeSellType.Items(ddlOtherSericeSellType.SelectedIndex).Text, String)) & "'"
                End If
            End If


            If ddlMarketCode.Value.Trim <> "[Select]" Then
                If strSelectionFormula <> "" Then
                    strReportTitle = strReportTitle & " ; Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
                    strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
                Else
                    strReportTitle = "Market Code  : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
                    strSelectionFormula = "{plgrpmast.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
                End If
            End If

            If ddlOtherSericeGCode.Value.Trim <> "[Select]" Then
                If strSelectionFormula <> "" Then
                    strReportTitle = strReportTitle & " ;Other Service Group Code : " & Trim(CType(ddlOtherSericeGCode.Items(ddlOtherSericeGCode.SelectedIndex).Text, String))
                    strSelectionFormula = strSelectionFormula & " and {othgrpmast.othgrpcode} = '" & Trim(CType(ddlOtherSericeGCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
                Else
                    strReportTitle = "Other Service Group Code : " & Trim(CType(ddlOtherSericeGCode.Items(ddlOtherSericeGCode.SelectedIndex).Text, String))
                    strSelectionFormula = "{othgrpmast.othgrpcode} = '" & Trim(CType(ddlOtherSericeGCode.Items(ddlOtherSericeGCode.SelectedIndex).Text, String)) & "'"
                End If
            End If

            If ddlSubSeasonCD.Value.Trim <> "[Select]" Then
                If strSelectionFormula <> "" Then
                    strReportTitle = strReportTitle & " ; Season Code : " & Trim(CType(ddlSubSeasonCD.Items(ddlSubSeasonCD.SelectedIndex).Text, String))
                    strSelectionFormula = strSelectionFormula & " and {subseasmast.subseascode} = '" & Trim(CType(ddlSubSeasonCD.Items(ddlSubSeasonCD.SelectedIndex).Text, String)) & "'"
                Else
                    strReportTitle = "Season Code : " & Trim(CType(ddlSubSeasonCD.Items(ddlSubSeasonCD.SelectedIndex).Text, String))
                    strSelectionFormula = "{subseasmast.subseascode} = '" & Trim(CType(ddlSubSeasonCD.Items(ddlSubSeasonCD.SelectedIndex).Text, String)) & "'"
                End If
            End If

            If ddlCurrencyCode.Value.Trim <> "[Select]" Then
                If strSelectionFormula <> "" Then
                    strReportTitle = strReportTitle & " ; Currency Code : " & Trim(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String))
                    strSelectionFormula = strSelectionFormula & " and {currmast.currcode} = '" & Trim(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String)) & "'"
                Else
                    strReportTitle = "SCurrency Code : " & Trim(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String))
                    strSelectionFormula = "{currmast.currcode} = '" & Trim(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String)) & "'"
                End If
            End If


            If dpFromdate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
                strReportTitle = strReportTitle & "  From : " & dpFromdate.txtDate.Text.Trim & " To " & dpToDate.txtDate.Text.Trim
                If Trim(strSelectionFormula) = "" Then
                    strSelectionFormula = "( ({oplisth.frmdate} in  Date( '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "') " _
                    & "  to  Date('" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00 ") & "')) " _
                    & "  or ({oplisth.todate} in  Date( '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00 ") & "') " _
                    & "   to Date('" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "') )" _
                    & " or ( ({oplisth.frmdate} > Date( '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "')) " _
                    & "  and ({oplisth.todate} < Date('" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "') )  ) )"
                Else
                    strSelectionFormula = strSelectionFormula & "and ( ({oplisth.frmdate} in  Date( '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "') " _
                    & "  to  Date('" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00 ") & "')) " _
                    & "  or ({oplisth.todate} in  Date( '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00 ") & "') " _
                    & "   to Date('" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "') )" _
                    & " or ( ({oplisth.frmdate} > Date( '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "')) " _
                    & "  and ({oplisth.todate} < Date('" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd 00:00:00") & "') )  ) )"
                End If
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("exec sp_repoprice '" & TxtPLCD.Text & "'", SqlConn)
            myCommand.ExecuteReader()
            SqlConn.Close()

            Session.Add("SelectionFormula", strSelectionFormula)
            Session.Add("ReportTitle", strReportTitle)
            Response.Redirect("rptReport.aspx", False)
        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesPriceListSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Dim objDateTime As New clsDateTime
        strWhereCond = ""
        If TxtPLCD.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(oplisth.oplistcode) LIKE '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(oplisth.oplistcode) LIKE '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlOtherSericeGCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (oplisth.othgrpcode) = '" & Trim(CType(ddlOtherSericeGCode.Items(ddlOtherSericeGCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (oplisth.othgrpcode) = '" & Trim(CType(ddlOtherSericeGCode.Items(ddlOtherSericeGCode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If ddlOtherSericeGName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (othgrpmast.othgrpname) = '" & Trim(CType(ddlOtherSericeGName.Items(ddlOtherSericeGName.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (othgrpmast.othgrpname) = '" & Trim(CType(ddlOtherSericeGName.Items(ddlOtherSericeGName.SelectedIndex).Text, String)) & "'"
            End If
        End If



        If ddlOtherSericeSellType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " oplisth.othsellcode = '" & Trim(CType(ddlOtherSericeSellType.Items(ddlOtherSericeSellType.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND oplisth.othsellcode = '" & Trim(CType(ddlOtherSericeSellType.Items(ddlOtherSericeSellType.SelectedIndex).Text, String)) & "'"
            End If
        End If



        If ddlOtherSericeSellTypeN.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " othsellmast.othsellname= '" & Trim(CType(ddlOtherSericeSellTypeN.Items(ddlOtherSericeSellTypeN.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND othsellmast.othsellname = '" & Trim(CType(ddlOtherSericeSellTypeN.Items(ddlOtherSericeSellTypeN.SelectedIndex).Text, String)) & "'"
            End If
        End If




        If ddlCurrencyCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " oplisth.currcode = '" & Trim(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND oplisth.currcode = '" & Trim(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlCurrencyName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " currmast.currname = '" & Trim(CType(ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND currmast.currname = '" & Trim(CType(ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text, String)) & "'"
            End If
        End If



        If ddlMarketCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " oplisth.plgrpcode = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND oplisth.plgrpcode = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If ddlMarketName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " plgrpmast.plgrpname = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND plgrpmast.plgrpname = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSubSeasonCD.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " oplisth.subseascode = '" & Trim(CType(ddlSubSeasonCD.Items(ddlSubSeasonCD.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND oplisth.subseascode = '" & Trim(CType(ddlSubSeasonCD.Items(ddlSubSeasonCD.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSubSeasonName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " subseasmast.subseasname = '" & Trim(CType(ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND subseasmast.subseasname = '" & Trim(CType(ddlSubSeasonName.Items(ddlSubSeasonName.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If DDLstatus.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(oplisth.approve,0) = 0"
            Else
                strWhereCond = strWhereCond & " and isnull(oplisth.approve,0) = 0"
            End If
        ElseIf DDLstatus.SelectedIndex = 2 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(oplisth.approve,0) = 1"
            Else
                strWhereCond = strWhereCond & " and isnull(oplisth.approve,0) = 1"
            End If
        End If


        If DDLshowagent.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(oplisth.showagent,0) = 1"
            Else
                strWhereCond = strWhereCond & " and isnull(oplisth.showagent,0) = 1"
            End If
        End If

        If dpFromdate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),oplisth.frmdate,111) between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),oplisth.todate,111)  between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),oplisth.frmdate,111) > convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),oplisth.todate,111) < convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),oplisth.frmdate,111) between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),oplisth.todate,111)  between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),oplisth.frmdate,111) > convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),oplisth.todate,111) < convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If
        BuildCondition = strWhereCond
    End Function

#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = "SELECT  oplisth.oplistcode, oplisth.othgrpcode, convert(varchar(10),oplisth.frmdate,103) as frmdate , convert(varchar(10),oplisth.todate,103) as todate, oplisth.plgrpcode, oplisth.othsellcode, " & _
                     "oplisth.subseascode, oplisth.currcode, oplisth.adddate, oplisth.adduser, oplisth.moddate, oplisth.moduser, " & _
                      "plgrpmast.plgrpname, othgrpmast.othgrpname, othsellmast.othsellname, currmast.currname, subseasmast.subseasname,case isnull(oplisth.approve,0) when 0 then 'Unapproved' else 'Approved' end approve ," & _
                         " case isnull(oplisth.showagent,0) when 0 then 'Yes' else 'No' end showagent FROM oplisth INNER JOIN " & _
                   "plgrpmast ON oplisth.plgrpcode = plgrpmast.plgrpcode INNER JOIN " & _
                   "othsellmast ON oplisth.othsellcode = othsellmast.othsellcode INNER JOIN " & _
                   "othgrpmast ON oplisth.othgrpcode = othgrpmast.othgrpcode INNER JOIN " & _
                      "subseasmast ON oplisth.subseascode = subseasmast.subseascode INNER JOIN " & _
                     "currmast ON oplisth.currcode = currmast.currcode "


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
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

        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesPriceListSerach.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session("State") = "New"
        'Response.Redirect("OtherServicesPriceList1.aspx")
        Dim strpop As String = ""
        strpop = "window.open('OtherServicesPriceList1.aspx?State=New','OtherServicesPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        ' FillGrid("oplisth.oplistcode")
        FillGridWithOrderByValues()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbloplistcode")

            Dim approve As Label
            approve = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapprove")

            If e.CommandName = "Editrow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesPriceList1.aspx", False)

                If approve.Text = "Approved" Then
                    If Session("Statusapprove") = "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Edit the Approved Booking' );", True)
                        Return
                    End If
                End If

                Dim strpop As String = ""
                strpop = "window.open('OtherServicesPriceList1.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesPriceList1.aspx", False)

                Dim strpop As String = ""
                strpop = "window.open('OtherServicesPriceList1.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesPriceList2.aspx", False)

                Dim strpop As String = ""
                strpop = "window.open('OtherServicesPriceList2.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Deleterow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesPriceList2.aspx", False)

                If approve.Text = "Approved" Then
                    If Session("Statusapprove") = "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Delete the Approved Booking' );", True)
                        Return
                    End If
                End If


                Dim strpop As String = ""
                strpop = "window.open('OtherServicesPriceList2.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtPLCD.Text = ""
        ddlCurrencyCode.Value = "[Select]"
        ddlCurrencyName.Value = "[Select]"

        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"

        ddlOtherSericeGCode.Value = "[Select]"
        ddlOtherSericeGName.Value = "[Select]"

        ddlOtherSericeSellType.Value = "[Select]"
        ddlOtherSericeSellTypeN.Value = "[Select]"

        ddlCurrencyCode.Value = "[Select]"
        ddlSubSeasonCD.Value = "[Select]"

        ddlSubSeasonCD.Value = "[Select]"
        ddlSubSeasonName.Value = "[Select]"

        dpFromdate.txtDate.Text = ""
        dpToDate.txtDate.Text = ""

        'FillGrid("oplisth.oplistcode")
        ddlOrderBy.SelectedIndex = 0
        FillGridWithOrderByValues()
        SetFocus(TxtPLCD)
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing

    End Sub
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        '  FillGrid(e.SortExpression, direction)
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
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then


                strSqlQry = "SELECT dbo.oplisth.oplistcode AS [Price List Code], dbo.oplisth.othgrpcode AS [Group Code],othgrpmast.othgrpname as [Group Name], Convert(Varchar(10),oplisth.frmdate,103) as  [From Date], " & _
                          " Convert(Varchar(10),oplisth.todate,103) AS [To Date], dbo.oplisth.plgrpcode AS [Market Code], dbo.oplisth.othsellcode AS [Sell Type],  " & _
                          "dbo.oplisth.subseascode AS [Sub Season], dbo.oplisth.currcode AS Currency, (Convert(Varchar, Datepart(DD,oplisth.adddate))+ '/'+ Convert(Varchar, Datepart(MM,oplisth.adddate))+ '/'+ Convert(Varchar, Datepart(YY,oplisth.adddate)) + ' ' + Convert(Varchar, Datepart(hh,oplisth.adddate))+ ':' + Convert(Varchar, Datepart(m,oplisth.adddate))+ ':'+ Convert(Varchar, Datepart(ss,oplisth.adddate))) as [Date Created], " & _
                          "dbo.oplisth.adduser AS [User Created], (Convert(Varchar, Datepart(DD,oplisth.moddate))+ '/'+ Convert(Varchar, Datepart(MM,oplisth.moddate))+ '/'+ Convert(Varchar, Datepart(YY,oplisth.moddate)) + ' ' + Convert(Varchar, Datepart(hh,oplisth.moddate))+ ':' + Convert(Varchar, Datepart(m,oplisth.moddate))+ ':'+ Convert(Varchar, Datepart(ss,oplisth.moddate))) as [Date Modified],dbo.oplisth.moduser AS [User Modified] " & _
                           "FROM dbo.oplisth INNER JOIN " & _
                          "dbo.plgrpmast ON dbo.oplisth.plgrpcode = dbo.plgrpmast.plgrpcode INNER JOIN " & _
                          "dbo.othsellmast ON dbo.oplisth.othsellcode = dbo.othsellmast.othsellcode INNER JOIN " & _
                          "dbo.othgrpmast ON dbo.oplisth.othgrpcode = dbo.othgrpmast.othgrpcode INNER JOIN " & _
                    "dbo.subseasmast ON dbo.oplisth.subseascode = dbo.subseasmast.subseascode INNER JOIN " & _
                    "dbo.currmast ON dbo.oplisth.currcode = dbo.currmast.currcode"

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "oplisth")

                objUtils.ExportToExcel(DS, Response)
                con.Close()

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception
        End Try
    End Sub

#End Region

#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Panel1.Visible = False
        ddlCurrencyCode.Visible = True
        ddlCurrencyName.Visible = False
        ddlSubSeasonCD.Visible = False
        ddlSubSeasonName.Visible = False
        ddlOtherSericeGCode.Visible = False
        ddlOtherSericeGName.Visible = False
        dpFromdate.txtDate.Visible = False
        dpToDate.txtDate.Visible = False
        ddlOtherSericeSellType.Visible = False
        ddlOtherSericeSellTypeN.Visible = False
        SetFocus(TxtPLCD)
    End Sub

#End Region

#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Panel1.Visible = True
        ddlCurrencyCode.Visible = True
        ddlCurrencyName.Visible = True
        ddlSubSeasonCD.Visible = True
        ddlSubSeasonName.Visible = True
        ddlOtherSericeGCode.Visible = True
        ddlOtherSericeGName.Visible = True
        dpFromdate.txtDate.Visible = True
        dpToDate.txtDate.Visible = True
        ddlOtherSericeSellType.Visible = True
        ddlOtherSericeSellTypeN.Visible = True
        SetFocus(TxtPLCD)
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If dpFromdate.txtDate.Text <> "" Then
            If dpToDate.txtDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Date field can not be left blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpToDate.txtDate.ClientID + "');", True)
                Exit Sub
            End If
        End If
        If dpToDate.txtDate.Text <> "" Then
            If dpFromdate.txtDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From Date field can not be left blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpFromdate.txtDate.ClientID + "');", True)
                Exit Sub
            End If
        End If

        'FillGrid("oplisth.oplistcode")
        FillGridWithOrderByValues()
        SetFocus(btnSearch)
    End Sub
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

    Protected Sub dpFromdate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("oplisth.oplistcode", "DESC")
            Case 1
                FillGrid("oplisth.oplistcode", "ASC")
            Case 2
                FillGrid("oplisth.othgrpcode", "ASC")
            Case 3
                FillGrid("othgrpmast.othgrpname", "ASC")
            Case 4
                FillGrid("oplisth.plgrpcode", "ASC")
            Case 5
                FillGrid("oplisth.othsellcode", "ASC")

        End Select
    End Sub
    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "oplisth.oplistcode DESC"
            Case 1
                ExportWithOrderByValues = "oplisth.oplistcode ASC"
            Case 2
                ExportWithOrderByValues = "oplisth.othgrpcode ASC"
            Case 3
                ExportWithOrderByValues = "othgrpmast.othgrpname ASC"
            Case 4
                ExportWithOrderByValues = "oplisth.plgrpcode ASC"
            Case 5
                ExportWithOrderByValues = "oplisth.othsellcode ASC"
        End Select
    End Function

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesPriceListSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub
End Class


'strSqlQry = "SELECT dbo.oplisth.oplistcode, dbo.oplisth.othgrpcode, dbo.oplisth.frmdate, dbo.oplisth.todate, dbo.oplisth.plgrpcode, dbo.oplisth.othsellcode, " & _
'              "dbo.oplisth.subseascode, dbo.oplisth.currcode, dbo.oplisth.adddate, dbo.oplisth.adduser, dbo.oplisth.moddate, dbo.oplisth.moduser, " & _
'                "dbo.othsellmast.othsellname, dbo.plgrpmast.plgrpname, dbo.currmast.currname, dbo.subseasmast.subseasname, dbo.othgrpmast.othgrpname " & _
'            " FROM dbo.oplisth INNER JOIN " & _
'             " dbo.currmast ON dbo.oplisth.currcode = dbo.currmast.currcode INNER JOIN " & _
'              " dbo.plgrpmast ON dbo.oplisth.plgrpcode = dbo.plgrpmast.plgrpcode INNER JOIN " & _
'              " dbo.subseasmast ON dbo.oplisth.subseascode = dbo.subseasmast.subseascode INNER JOIN " & _
'              " dbo.othsellmast ON dbo.oplisth.othsellcode = dbo.othsellmast.othsellcode INNER JOIN " & _
'              " dbo.othgrpmast ON dbo.oplisth.othgrpcode = dbo.othgrpmast.othgrpcode"


'If dpFromdate.txtDate.Text <> "" Then
'    If strSelectionFormula <> "" Then
'        strReportTitle = strReportTitle & " ; From Date : " & dpFromdate.txtDate.Text
'        strSelectionFormula = strSelectionFormula & " and {oplisth.frmdate} LIKE '" & dpFromdate.txtDate.Text & "'"
'    Else
'        strReportTitle = "From Date : " & dpFromdate.txtDate.Text
'        strSelectionFormula = "{oplisth.frmdate} LIKE '" & dpFromdate.txtDate.Text & "'"
'    End If
'End If


'If dpToDate.txtDate.Text <> "" Then
'    If strSelectionFormula <> "" Then
'        strReportTitle = strReportTitle & " ;To Date : " & dpToDate.txtDate.Text
'        strSelectionFormula = strSelectionFormula & " and {oplisth.todate} LIKE '" & dpToDate.txtDate.Text & "'"
'    Else
'        strReportTitle = "To Date: " & dpToDate.txtDate.Text
'        strSelectionFormula = "{oplisth.todate} LIKE '" & dpToDate.txtDate.Text & "'"
'    End If
'End If



'If dpFromdate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
'    If Trim(strWhereCond) = "" Then
'        strWhereCond = " ((oplisth.frmdate between '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)) & "' and '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text)) & "') " & _
'                        " or (oplisth.todate between '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)) & "' and '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text)) & "') " & _
'                        " or (oplisth.frmdate < '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)) & "' and oplisth.todate > '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text)) & "')) "
'    Else
'        strWhereCond = strWhereCond & " AND ((oplisth.frmdate between '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)) & "' and '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text)) & "') " & _
'                        " or (oplisth.todate between '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)) & "' and '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text)) & "') " & _
'                        " or (oplisth.frmdate < '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)) & "' and oplisth.todate > '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text)) & "')) "
'    End If
'End If
'        Response.ContentType = "application/vnd.ms-excel"
'        Response.Charset = ""
'        Me.EnableViewState = False

'        Dim tw As New System.IO.StringWriter()
'        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
'        Dim frm As HtmlForm = New HtmlForm()
'        Me.Controls.Add(frm)
'        frm.Controls.Add(gv_SearchResult)
'        frm.RenderControl(hw)
'        Response.Write(tw.ToString())
'        Response.End()
'        Response.Clear()
'    Else
'        objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
'strSqlQry = "SELECT  subseascode AS [Sub Season Code] , subseasname AS [Sub Season Name], dispname as [Display Name], disporder as [Display Order], active as [Active], (Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created] , adduser as [User Created], (Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified], moduser as [User Modified] FROM subseasmast"
'strSqlQry = "SELECT dbo.oplisth.oplistcode AS [Price List Code], dbo.oplisth.othgrpcode AS [Group Code], dbo.oplisth.frmdate AS [From Date], " & _
'             "dbo.oplisth.todate AS [To Date], dbo.oplisth.plgrpcode AS [Market Code], dbo.oplisth.othsellcode AS [Sell Type], " & _
'             "dbo.oplisth.subseascode AS [Sub Season], dbo.oplisth.currcode AS Currency, dbo.oplisth.adddate AS [Date Created], " & _
'             "dbo.oplisth.adduser AS [User Created], dbo.oplisth.moddate AS [Date Modified], dbo.oplisth.moduser AS [User Modified], dbo.othsellmast.othsellname, " & _
'              "dbo.plgrpmast.plgrpname, dbo.currmast.currname, dbo.subseasmast.subseasname, dbo.othgrpmast.othgrpname " & _
'           "FROM  dbo.oplisth INNER JOIN" & _
'      "dbo.currmast ON dbo.oplisth.currcode = dbo.currmast.currcode INNER JOIN " & _
'      "dbo.plgrpmast ON dbo.oplisth.plgrpcode = dbo.plgrpmast.plgrpcode INNER JOIN " & _
'      "dbo.subseasmast ON dbo.oplisth.subseascode = dbo.subseasmast.subseascode INNER JOIN " & _
'      "dbo.othsellmast ON dbo.oplisth.othsellcode = dbo.othsellmast.othsellcode INNER JOIN" & _
'      "dbo.othgrpmast ON dbo.oplisth.othgrpcode = dbo.othgrpmast.othgrpcode"

'strSqlQry = "SELECT dbo.oplisth.oplistcode AS [Price List Code], dbo.oplisth.othgrpcode AS [Group Code],othgrpmast.othgrpname as [Group Name], (Convert(Varchar, Datepart(DD,oplisth.frmdate))+ '/'+ Convert(Varchar, Datepart(MM,oplisth.frmdate))+ '/'+ Convert(Varchar, Datepart(YY,oplisth.frmdate)) + ' ' + Convert(Varchar, Datepart(hh,oplisth.frmdate))+ ':' + Convert(Varchar, Datepart(m,oplisth.frmdate))+ ':'+ Convert(Varchar, Datepart(ss,oplisth.frmdate))) as  [From Date], " & _
'            "(Convert(Varchar, Datepart(DD,oplisth.todate))+ '/'+ Convert(Varchar, Datepart(MM,oplisth.todate))+ '/'+ Convert(Varchar, Datepart(YY,oplisth.todate)) + ' ' + Convert(Varchar, Datepart(hh,oplisth.todate))+ ':' + Convert(Varchar, Datepart(m,oplisth.todate))+ ':'+ Convert(Varchar, Datepart(ss,oplisth.todate)))  AS [To Date], dbo.oplisth.plgrpcode AS [Market Code], dbo.oplisth.othsellcode AS [Sell Type],  " & _
'            "dbo.oplisth.subseascode AS [Sub Season], dbo.oplisth.currcode AS Currency, (Convert(Varchar, Datepart(DD,oplisth.adddate))+ '/'+ Convert(Varchar, Datepart(MM,oplisth.adddate))+ '/'+ Convert(Varchar, Datepart(YY,oplisth.adddate)) + ' ' + Convert(Varchar, Datepart(hh,oplisth.adddate))+ ':' + Convert(Varchar, Datepart(m,oplisth.adddate))+ ':'+ Convert(Varchar, Datepart(ss,oplisth.adddate))) as [Date Created], " & _
'            "dbo.oplisth.adduser AS [User Created], (Convert(Varchar, Datepart(DD,oplisth.moddate))+ '/'+ Convert(Varchar, Datepart(MM,oplisth.moddate))+ '/'+ Convert(Varchar, Datepart(YY,oplisth.moddate)) + ' ' + Convert(Varchar, Datepart(hh,oplisth.moddate))+ ':' + Convert(Varchar, Datepart(m,oplisth.moddate))+ ':'+ Convert(Varchar, Datepart(ss,oplisth.moddate))) as [Date Modified],dbo.oplisth.moduser AS [User Modified] " & _
'             "FROM dbo.oplisth INNER JOIN " & _
'            "dbo.plgrpmast ON dbo.oplisth.plgrpcode = dbo.plgrpmast.plgrpcode INNER JOIN " & _
'            "dbo.othsellmast ON dbo.oplisth.othsellcode = dbo.othsellmast.othsellcode INNER JOIN " & _
'            "dbo.othgrpmast ON dbo.oplisth.othgrpcode = dbo.othgrpmast.othgrpcode INNER JOIN " & _
'      "dbo.subseasmast ON dbo.oplisth.subseascode = dbo.subseasmast.subseascode INNER JOIN " & _
'      "dbo.currmast ON dbo.oplisth.currcode = dbo.currmast.currcode"

'Response.Redirect("OtherServicesPriceList1.aspx", False)
'                Response.Redirect("OtherServicesPriceList1.aspx", False)