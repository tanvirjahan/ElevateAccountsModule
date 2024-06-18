'------------================--------------=======================------------------================
'   Module Name    :    PriseList.aspx
'   Developer Name :    Amit Survase
'   Date           :    17 July 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriseList
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strSqlQry1 As String
    Dim strSqlQry2 As String
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
        PriceListCode = 2
        SupplierCode = 3
        SupplierName = 4
        SupplierAgentName = 5
        Currency = 6
        Market = 7
        SubSeason = 8
        pricecode = 9
        promoid = 10
        RevisionDate = 11
        FromDate = 12
        ToDate = 13
        PriceListType = 14
        Mode = 15
        WeekEnd1 = 16
        WeekEnd2 = 17
        status = 18
        showweb = 19
        DateCreated = 20
        UserCreated = 21
        DateModified = 22
        UserModified = 23
        Edit = 24
        View = 25
        Delete = 26
        Copy = 27
        Print = 28
        promotion = 29
    End Enum
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        btnPrint.Visible = False
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnPrint.Visible = False
        If Page.IsPostBack = False Then
            Try

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim frmdate As String = ""
                Dim todate As String = ""
                ' btnPrint.Visible = False


                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                txtconnection.Value = Session("dbconnectionName")

                '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)

                SetFocus(TxtPLCD)

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                           CType(strappname, String), "PriceListModule\PriceList.aspx", btnAddNew, btnExportToExcel, btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, GridCol.Print, GridCol.Copy)
                End If
                btnPrint.Visible = False
                'If txtFromDate.Text = "" Then
                '    txtFromDate.Text = objDateTime.GetSystemDateOnly
                'End If
                'If txtToDate.Text = "" Then
                '    txtToDate.Text = objDateTime.GetSystemDateOnly
                'End If

                checkIsPrivilege()


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCD, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeNM, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketCD, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketNM, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgent, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierAgentNM, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode ='HOT' order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode ='HOT' order by partyname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyCD, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyNM, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeas, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasNM, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)


                Session.Add("strsortExpression", "cplisthnew.plistcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '  charcters(TxtPLCD)


                'Record list will be according to the Changing the year  
                'If Not (Session("changeyear") Is Nothing) Then
                '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                '    If Session("changeyear") = Year(Now).ToString Then
                '        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                '    Else
                '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                '    End If

                '    txtPfromdate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                '    txtPtodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

                'Else
                '    txtPfromdate.Text = ""
                '    txtPtodate.Text = ""
                'End If



                ' FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
              
                FillGrid("plistcode", "DESC")

                'ChkW1.Visible = False
                'ChkWeek2.Visible = False
                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode ='HOT' order by partyname", True, ddlSuppierNM.Value)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode ='HOT' order by partycode", True, ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text)

        End If
        Dim typ As Type
        typ = GetType(DropDownList)

        'If Request.QueryString("AutoNo") > 0 Then
        '    ViewState("AutoNo") = Request.QueryString("AutoNo")
        '    Session("AutoNo") = 0
        'Else
        '    ViewState("AutoNo") = 0
        'End If


        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlSPTypeCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSPTypeNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlmarketCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlMarketNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


            ddlSupplierAgent.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSuppierAgentNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlSuppierCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSuppierNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlCurrencyCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCurrencyNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlSubSeas.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSubSeasNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        End If

        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "pricelistWindowPostBack") Then
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
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
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


#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If TxtPLCD.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(plistcode) = '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(plistcode) = '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "'"
            End If
        End If

        If ddlSPTypeCD.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (sptypecode) = '" & Trim(CType(ddlSPTypeCD.Items(ddlSPTypeCD.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (sptypecode) = '" & Trim(CType(ddlSPTypeCD.Items(ddlSPTypeCD.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSPTypeNM.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (supagentname) = '" & Trim(CType(ddlSPTypeNM.Items(ddlSPTypeNM.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (supagentname) = '" & Trim(CType(ddlSPTypeNM.Items(ddlSPTypeCD.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSupplierAgent.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " supagentcode = '" & Trim(CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND supagentcode = '" & Trim(CType(ddlSupplierAgent.Items(ddlSupplierAgent.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSuppierAgentNM.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " sptypename= '" & Trim(CType(ddlSuppierAgentNM.Items(ddlSuppierAgentNM.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND sptypename = '" & Trim(CType(ddlSuppierAgentNM.Items(ddlSuppierAgentNM.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlCurrencyCD.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " currcode = '" & Trim(CType(ddlCurrencyCD.Items(ddlCurrencyCD.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND currcode = '" & Trim(CType(ddlCurrencyCD.Items(ddlCurrencyCD.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlCurrencyNM.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " currname = '" & Trim(CType(ddlCurrencyNM.Items(ddlCurrencyNM.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND currname = '" & Trim(CType(ddlCurrencyNM.Items(ddlCurrencyCD.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlmarketCD.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " plgrpcode like '%" & Trim(CType(ddlmarketCD.Items(ddlmarketCD.SelectedIndex).Text, String)) & "%'"
            Else
                strWhereCond = strWhereCond & " AND plgrpcode like '%" & Trim(CType(ddlmarketCD.Items(ddlmarketCD.SelectedIndex).Text, String)) & "%'"
            End If
        End If


        If ddlSuppierCD.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " partycode = '" & Trim(CType(ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND partycode = '" & Trim(CType(ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSuppierNM.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " partyname = '" & Trim(CType(ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND partyname = '" & Trim(CType(ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSubSeas.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " subseascode = '" & Trim(CType(ddlSubSeas.Items(ddlSubSeas.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND subseascode = '" & Trim(CType(ddlSubSeas.Items(ddlSubSeas.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlSubSeasNM.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " subseasname = '" & Trim(CType(ddlSubSeasNM.Items(ddlSubSeasNM.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND subseasname = '" & Trim(CType(ddlSubSeasNM.Items(ddlSubSeasNM.SelectedIndex).Text, String)) & "'"
            End If
        End If
        'ddlSupACode
        If ddlPriceList.SelectedValue <> "[Select]" Then
            If ddlPriceList.SelectedValue = "Normal Rates 1 Night" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " aplisttype = 0 "
                Else
                    strWhereCond = strWhereCond & " AND aplisttype = 0 "
                End If
            ElseIf ddlPriceList.SelectedValue = "Weekend Rates 1 Night" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " aplisttype = 2 "
                Else
                    strWhereCond = strWhereCond & " AND aplisttype = 2 "
                End If
            ElseIf ddlPriceList.SelectedValue = "Weekly Rates 7 Nights" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " aplisttype = 1 "
                Else
                    strWhereCond = strWhereCond & " AND aplisttype = 1 "
                End If
            ElseIf ddlPriceList.SelectedValue = "Normal Rates > 1 Night" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " aplisttype = 3 "
                Else
                    strWhereCond = strWhereCond & " AND aplisttype = 3 "
                End If
            ElseIf ddlPriceList.SelectedValue = "Weekend Rates > 1 Night" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " aplisttype = 4 "
                Else
                    strWhereCond = strWhereCond & " AND aplisttype = 4 "
                End If
            End If
        End If

        If DDLstatus.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(apprstatus,0) = 0"
            Else
                strWhereCond = strWhereCond & " and isnull(apprstatus,0) = 0"
            End If

        ElseIf DDLstatus.SelectedIndex = 2 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(apprstatus,0) = 1"
            Else
                strWhereCond = strWhereCond & " and isnull(apprstatus,0) = 1"
            End If
        End If

        If ddlshowweb.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(apprshowagent,0) = 1"
            Else
                strWhereCond = strWhereCond & " and isnull(apprshowagent ,0) = 1"
            End If

        ElseIf ddlshowweb.SelectedIndex = 2 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(apprshowagent,0) =0"
            Else
                strWhereCond = strWhereCond & " and isnull(apprshowagent ,0) = 0"
            End If
        End If

        If txtsupname.Value <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = strWhereCond & " partyname like '%" & txtsupname.Value & "%' "
            Else
                strWhereCond = strWhereCond & " and partyname like '%" & txtsupname.Value & "%' "
            End If
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partyname like '%" + txtsupname.Value + "%' order by partycode", True, ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and partyname like '%" + txtsupname.Value + "%'  order by partyname", True, ddlSuppierNM.Value)

        End If

        If Txtprmname.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = strWhereCond & " pricecode like '%" & Txtprmname.Text & "%' "
            Else
                strWhereCond = strWhereCond & " and pricecode like '%" & Txtprmname.Text & "%' "
            End If
        End If

        If Txtpromotionid.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = strWhereCond & " promotionid like '%" & Txtpromotionid.Text & "%' "
            Else
                strWhereCond = strWhereCond & " and promotionid like '%" & Txtpromotionid.Text & "%' "
            End If
        End If



        If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),revisiondate,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),revisiondate,111)  between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),revisiondate,111) > convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),revisiondate,111) < convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),revisiondate,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),revisiondate,111)  between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),revisiondate,111) > convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),revisiondate,111) < convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If

        If txtPfromdate.Text <> "" And txtPtodate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "((convert(varchar(10), '" & Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),frmdate,111)  " _
                & "  and convert(varchar(10),todate,111)) or   (convert(varchar(10), '" & Format(CType(txtPtodate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                & "  between convert(varchar(10),frmdate,111)  and  convert(varchar(10),todate,111))   " _
                & " or (convert(varchar(10),frmdate,111) >= convert(varchar(10), '" & Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),todate,111) <= convert(varchar(10), '" & Format(CType(txtPtodate.Text, Date), "yyyy/MM/dd") & "',111) ))"
            Else
                strWhereCond = strWhereCond & " and ((convert(varchar(10), '" & Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),frmdate,111)  " _
                & "  and convert(varchar(10),todate,111)) or   (convert(varchar(10), '" & Format(CType(txtPtodate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                & "  between convert(varchar(10),frmdate,111)  and  convert(varchar(10),todate,111))   " _
                & " or (convert(varchar(10),frmdate,111) >= convert(varchar(10), '" & Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),todate,111) <= convert(varchar(10), '" & Format(CType(txtPtodate.Text, Date), "yyyy/MM/dd") & "',111)))"

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
        Dim myds1 As New DataSet
        Dim mydt1 As New DataTable
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        strSqlQry1 = ""
        Try



            strSqlQry = "select * from view_plist_search(nolock)  "

           
            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & BuildCondition()

            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            Dim pliststr As String
            If ViewState("MyAutoNo") <> 1 Then
                pliststr = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "pricelist_links", "calledfromlist", "Autoid", Request.QueryString("AutoNo"))
            End If

            If pliststr <> "" Then
                strSqlQry = strSqlQry & " WHERE plistcode='" & pliststr & "'"

                ''strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            Else

                If Trim(BuildCondition) <> "" Then


                    strSqlQry = strSqlQry & " WHERE " & BuildCondition()

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                Else

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If

            End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
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
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PriseList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    
#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session("State") = "New"
        'Response.Redirect("HeaderInfo.aspx")
        'Response.Redirect("Currencies.aspx", False)
        Dim strpop As String = ""
        Session("MealPlans") = Nothing
        strpop = "window.open('HeaderInfonew.aspx?State=New','HeaderInfo','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

#End Region


#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("cplisthnew.plistcode", "DESC")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("plistcode", "DESC")
                '  FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("plistcode", "ASC")
                'FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("partycode", "ASC")
                ' FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '   FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")
                ' FillGrid("supplier_agents.supagentname", "supplier_agents.supagentname", "ASC")
            Case 5
                FillGrid("plgrpcode", "ASC")
                '  FillGrid("cplisthnew.plgrpcode", "edit_cplisthnew.plgrpcode", "ASC")
            Case 6
                FillGrid("subseascode", "ASC")
                ' FillGrid("cplisthnew.subseascode", "edit_cplisthnew.plistcode", "ASC")

        End Select
    End Sub

#End Region

    Private Sub GetMealPlanString(ByVal approve As Integer, ByVal plistcode As String)
        Try
            Dim mStrqry As String = ""
            If approve = 1 Then
                mStrqry = "SELECT mealcode FROM cplist_mealplan Where plistcode='" & plistcode & "'"
            Else
                mStrqry = "SELECT mealcode FROM edit_cplist_mealplan Where plistcode='" & plistcode & "'"
            End If            

            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mStrqry)
            Dim mealstr As String = ""
            If ds.Tables.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    If mealstr.Length = 0 Then
                        mealstr = row.Item("mealcode")
                    Else
                        mealstr += "," + row.Item("mealcode")
                    End If
                Next
            End If
            Session("MealPlans") = Nothing
            Session("MealPlans") = mealstr
        Catch ex As Exception

        End Try
    End Sub


#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            Dim plistcode As Label
            Dim partycode As Label
            Dim supagentcode As String
            Dim supagentname As Label
            Dim markets As Label

            Dim approve As Label

            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            approve = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapprove")
            Dim lblpromotionid As Label = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblpromotionid")
            Dim status As Integer = 0
            If approve.Text = "Approved" Then status = 1
            If e.CommandName = "Editrow" Then

                GetMealPlanString(status, lblId.Text)
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("HeaderInfo.aspx", False)

                If approve.Text = "Approved" Then
                    If Session("Statusapprove") = "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Edit the Approved Booking' );", True)
                        Return
                    End If
                End If

                If approve.Text = "Approved" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "javascript:confirmapprove('" & lblId.Text & "');", True)
                    Exit Sub
                Else
                    Dim strpop As String = ""
                    strpop = "window.open('HeaderInfonew.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&status=" & status & "','HeaderInfo','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If
            ElseIf e.CommandName = "View" Then                
                Dim strpop As String = ""
                GetMealPlanString(status, lblId.Text)
                strpop = "window.open('HeaderInfonew.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&status=" & status & "','HeaderInfo','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Deleterow" Then
                GetMealPlanString(status, lblId.Text)
                If approve.Text = "Approved" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Delete the Approved Booking' );", True)
                    Return
                End If



                Dim strpop As String = ""
                strpop = "window.open('HeaderInfonew.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&status=" & status & "','HeaderInfo','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Print" Then
                Dim strpop As String = ""
                GetMealPlanString(status, lblId.Text)
                strpop = "window.open('rptPLprint.aspx?Pageame=PriceList Print&BackPageName=pricelist.aspx&repfilter=PL&PLCode=" + CType(lblId.Text.Trim, String) + "','rptPLprint','width=' + screen.availWidth +',height='+ screen.availHeight +',left=0,top=0,status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Copy" Then
                GetMealPlanString(status, lblId.Text)
                Dim strpop As String = ""
                strpop = "window.open('HeaderInfonew.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "&status=" & status & "','HeaderInfo','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Promotion" Then


                If CType(lblpromotionid.Text, String) = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Not applicable for contracted rates');", True)
                    Exit Sub
                End If

                Dim strpop As String = ""
                strpop = "window.open('PromotionHeader.aspx?State=View&RefCode=" & CType(lblpromotionid.Text, String) & "','PromotionHeader','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "plinks" Then
                Dim plinkstr As String = ""
                Dim myds As New DataSet
                Dim mydt As New DataTable
                Dim autonum As Integer
                plistcode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
                partycode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblparty")
                supagentname = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblagent")
                supagentcode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentcode", "supagentname", supagentname.Text)
                markets = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblmarket")
                plinkstr = plinkstr + "supagentcode;" + supagentcode + ";partycode;" + partycode.Text + ";markets;" + markets.Text

                '  mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                'mySqlCmd = New SqlCommand("sp_add_pricelist_links", mySqlConn)
                'mySqlCmd.CommandType = CommandType.StoredProcedure
                'mySqlCmd.Parameters.Add(New SqlParameter("@inputparameterlist", SqlDbType.VarChar, 1000)).Value = plinkstr
                'mySqlCmd.Parameters.Add(New SqlParameter("@calledfromlist", SqlDbType.VarChar, 1000)).Value = plistcode.Text
                'mySqlCmd.ExecuteNonQuery()
                myds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), "Exec sp_add_pricelist_links '" & plinkstr & "','" & plistcode.Text & "' ")
                mydt = myds.Tables(0)
                autonum = mydt.Rows(0).Item(0)


                Session("AutoNo") = autonum

                'TxtTest.Text = "generalpolicysearch.aspx?AutoNo=" & autonum & ""
                ModalPopupDays.Show()


            End If
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtPLCD.Text = ""
        Txtprmname.Text = ""
        Txtpromotionid.Text = ""
        txtsupname.Value = ""


        ddlSPTypeCD.Value = "[Select]"
        ddlSPTypeNM.Value = "[Select]"

        ddlmarketCD.Value = "[Select]"
        ddlMarketNM.Value = "[Select]"

        ddlSupplierAgent.Value = "[Select]"
        ddlSuppierAgentNM.Value = "[Select]"

        ddlSuppierCD.Value = "[Select]"
        ddlSuppierNM.Value = "[Select]"

        ddlCurrencyCD.Value = "[Select]"
        ddlCurrencyNM.Value = "[Select]"


        ddlSubSeas.Value = "[Select]"
        ddlSubSeasNM.Value = "[Select]"

        ddlPriceList.SelectedValue = "[Select]"

        ViewState("MyAutoNo") = 1 'clear plist links 
        ddlOrderBy.SelectedIndex = 0
        ' FillGrid("cplisthnew.plistcode", "DESC")
        FillGrid("plistcode", "DESC")
    End Sub
#End Region


#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
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

                strSqlQry = "select * from view_plist_search "
                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & ""
                Else
                    strSqlQry = strSqlQry & " ORDER BY plistcode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "cplisthnew")

                objUtils.ExportToExcel(DS, Response)
                con.Close()

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

#End Region


#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        '    Try
        '        '  Session.Add("CurrencyCode", txtblocksale_header.blocksaleid.Text.Trim)
        '        '   Session.Add("CurrencyName", txtcityname.Text.Trim)
        '        '   Response.Redirect("rptCurrencies.aspx", False)

        '        Dim strReportTitle As String = ""
        '        Dim strSelectionFormula As String = ""

        '        Session.Add("Pageame", "Block Full Sales")
        '        Session.Add("BackPageName", "BlockFullSalesSearch.aspx")

        '        If txtTranId.Text.Trim <> "" Then
        '            strReportTitle = "Block Sale ID : " & txtTranId.Text.Trim
        '            strSelectionFormula = "{blocksale_header.blocksaleid} LIKE '" & txtTranId.Text.Trim & "*'"
        '        End If


        '        If ddlPartyCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlSupACode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlSupAName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlMarketCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlMarketName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlSPTypeCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlSpTypeName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        Session.Add("SelectionFormula", strSelectionFormula)
        '        Session.Add("ReportTitle", strReportTitle)
        '        Response.Redirect("rptReport.aspx", False)
        '    Catch ex As Exception
        '        objUtils.WritErrorLog("BlockFullSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        '    End Try
    End Sub
#End Region



    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.CheckedChanged
        pnlHeader.Visible = False
        SetFocus(rbtnsearch)
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnadsearch.CheckedChanged
        pnlHeader.Visible = True
        SetFocus(rbtnadsearch)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim frmdate As String = ""
        Dim todate As String = ""

        'Record list will be according to the Changing the year  
        'If Not (Session("changeyear") Is Nothing) Then
        '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

        '    If Session("changeyear") = Year(Now).ToString Then
        '        If txtPtodate.Text = "" Then
        '            todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
        '        Else
        '            todate = CType(txtPtodate.Text, Date).ToString
        '        End If
        '    Else
        '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
        '    End If

        '    txtPfromdate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
        '    txtPtodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

        'Else
        '    txtPfromdate.Text = ""
        '    txtPtodate.Text = ""
        'End If



        'Record list will be according to the Changing the year  
        'If Session("changeyear") <> Year(CType(txtPfromdate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        'If Session("changeyear") <> Year(CType(txtPtodate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If



        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("plistcode", "DESC")
                '   FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("plistcode", "ASC")
                '    FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("partycode", "ASC")
                '   FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '  FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")
                '   FillGrid("supplier_agents.supagentname", "supplier_agents.supagentname", "ASC")
            Case 5
                FillGrid("plgrpcode", "ASC")
                '      FillGrid("cplisthnew.plgrpcode", "edit_cplisthnew.plgrpcode", "ASC")
            Case 6
                FillGrid("subseascode", "ASC")
                '   FillGrid("cplisthnew.subseascode", "edit_cplisthnew.subseascode", "ASC")
        End Select
    End Sub
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PriceListSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("plistcode", "DESC")
                '  FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("plistcode", "ASC")
                '  FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("partycode", "ASC")
                '   FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '    FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")
                '  FillGrid("supplier_agents.supagentname", "supplier_agents.supagentname", "ASC")
            Case 5
                FillGrid("plgrpcode", "ASC")
                '    FillGrid("cplisthnew.plgrpcode", "edit_cplisthnew.plgrpcode", "ASC")
            Case 6
                FillGrid("subseascode", "ASC")
                '  FillGrid("cplisthnew.subseascode", "cplisthnew.subseascode", "ASC")
        End Select
        SetFocus(ddlOrderBy)
    End Sub

   
   
    Protected Sub btnexit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click
        'Session("AutoNo")
        Session.Remove("AutoNo")
    End Sub
End Class
