'------------------------------------------------------------------------------------------------
'   Module Name    :    ReceiptsSearch 
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class ReceiptsSearch_new
    Inherits System.Web.UI.Page

    Dim objDate As New clsDateTime
    Enum GridCol

        tran_id = 0
        tran_type = 1
        status = 2
        receipt_tran_date = 3
        receipt_date = 4
        receipt_credit = 5
        receipt_currency_id = 6
        receipt_received_from = 7
        receipt_cheque_number = 8
        receipt_cashbank_code = 9
        other_bank_master_des = 10
        adddate = 11
        adduser = 12
        moddate = 13
        moduser = 14
        Edit = 15
        View = 16
        Delete = 17
        Copy = 18
        chqprint = 20

    End Enum

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim strappid As String = ""
    Dim strappname As String = ""
#End Region
    '#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    '    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        pnlSearch.Visible = False
    '    End Sub
    '#End Region
    '#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    '    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        pnlSearch.Visible = True
    '        'txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
    '        'txtTodate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
    '    End Sub
    '#End Region
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("Receipts.aspx", False)
        Dim strpop As String = ""
        Dim actionstr As String = ""
        actionstr = "New"
        'strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType("", String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub



#End Region
#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        ''  gv_SearchResult.Visible = True
        'lblMsg.Visible = False

        ''If gv_SearchResult.PageIndex < 0 Then
        ''    gv_SearchResult.PageIndex = 0
        ''End If
        strSqlQry = ""
        Try
            strSqlQry = "SELECT receipt_master_new.tran_id, receipt_master_new.tran_type, " & _
                            " case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                            " receipt_master_new.tran_type AS Expr1,  " & _
                            " convert(varchar(10),receipt_master_new.receipt_date,103)  as receipt_date, " & _
                            "  convert(varchar(10),receipt_master_new.receipt_tran_date,103) as receipt_tran_date, " & _
                            " case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit " & _
                            " when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit else 0 end receipt_credit , " & _
                            " receipt_master_new.receipt_currency_id,   " & _
                            " receipt_master_new.receipt_received_from, receipt_master_new.receipt_cheque_number, receipt_master_new.receipt_cashbank_code, " & _
                            " customer_bank_master.other_bank_master_des, receipt_master_new.adddate, receipt_master_new.adduser, receipt_master_new.moddate, " & _
                            " receipt_master_new.moduser, receipt_master_new.tran_id + '|' + receipt_master_new.tran_type tranidcomarg " & _
                            " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                            " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                            " where receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"


            Dim strBuild As String = Trim(BuildCondition)
            If strBuild <> "" Then
                strSqlQry = strSqlQry & " and " & strBuild & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If


            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " And " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            ''   gv_SearchResult.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                ''   gv_SearchResult.DataBind()
                lblMsg.Visible = False
            Else
                '' gv_SearchResult.PageIndex = 0
                '' gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            '' gv_SearchResult.Columns(0).Visible = True
            UpdatePanel2.Update()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Private Function BuildCondition1() As String"
    Private Function BuildCondition1() As String
        strWhereCond = ""

        If txtFromDate.Text.Trim <> "" And txtTodate.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If


        BuildCondition1 = strWhereCond

    End Function
#End Region
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "ACCOUNTNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("AccountName", lsProcessCity, "A")
                Case "DOCUMENTNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("DocumentNo", lsProcessCity, "D")
                Case "BANKNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BankName", lsProcessCity, "B")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        'If txtTranId.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(receipt_master_new.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(receipt_master_new.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
        '    End If
        'End If

        'If txtFromDate.Text.Trim <> "" And txtTodate.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = "( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        '        & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
        '        & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        '        & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
        '    Else
        '        strWhereCond = strWhereCond & " and ( (convert(varchar(10),receipt_master_new.receipt_tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        '        & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
        '        & " or (convert(varchar(10),receipt_master_new.receipt_tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
        '        & "  and convert(varchar(10),receipt_master_new.receipt_tran_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

        '    End If
        'End If


        'If rbtnadsearch.Checked = True Then
        '    If ddlType.Value <> "[Select]" Then
        '        If Trim(strWhereCond) = "" Then
        '            strWhereCond = " upper(receipt_master_new.receipt_cashbank_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
        '        Else
        '            strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_cashbank_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
        '        End If
        '    End If
        '    If ddlAccCode.Value <> "[Select]" Then
        '        If Trim(strWhereCond) = "" Then
        '            strWhereCond = " upper(receipt_master_new.receipt_cashbank_code) = '" & Trim(CType(ddlAccName.Value, String)) & "'"
        '        Else
        '            strWhereCond = strWhereCond & " AND upper(receipt_master_new.receipt_cashbank_code) = '" & Trim(CType(ddlAccName.Value, String)) & "'"
        '        End If
        '    End If

        '    If txtdesc.Text.Trim <> "" Then
        '        If Trim(strWhereCond) = "" Then
        '            strWhereCond = " receipt_master_new.receipt_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
        '        Else
        '            strWhereCond = strWhereCond & " AND  receipt_master_new.receipt_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
        '        End If
        '    End If


        '    If ddlBankCode.Value <> "[Select]" Then
        '        If Trim(strWhereCond) = "" Then

        '            strWhereCond = " upper(customer_bank_master.other_bank_master_code ) = '" & Trim(CType(ddlBankName.Value, String)) & "'"
        '        Else
        '            strWhereCond = strWhereCond & " AND upper(customer_bank_master.other_bank_master_code ) = '" & Trim(CType(ddlBankName.Value, String)) & "'"
        '        End If
        '    End If
        '    If txtFromRecvAmt.Text.Trim <> "" And txtToRecvAmt.Text.Trim <> "" Then
        '        If Trim(strWhereCond) = "" Then
        '            strWhereCond = " (receipt_master_new.receipt_credit between  " & txtFromRecvAmt.Text & "  and  " & txtToRecvAmt.Text & " ) "
        '        Else
        '            strWhereCond = strWhereCond & " AND (receipt_master_new.receipt_credit between  " & txtFromRecvAmt.Text & "  and  " & txtToRecvAmt.Text & " ) "
        '        End If
        '    End If
        '    If ddlStatus.Value <> "[Select]" Then
        '        If Trim(strWhereCond) = "" Then
        '            strWhereCond = " isnull(receipt_master_new.post_state,'')  = '" & ddlStatus.Value & "'"
        '        Else
        '            strWhereCond = strWhereCond & " AND isnull(receipt_master_new.post_state,'')  = '" & ddlStatus.Value & "'"
        '        End If
        '    End If
        'End If
        BuildCondition = strWhereCond

    End Function
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("ReceiptsSearchRVPVTranType", Request.QueryString("tran_type"))

        Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim appidnew As String = CType(Request.QueryString("appid"), String)

        Dim strappid As String = ""
        Dim strappname As String = ""

        If appidnew Is Nothing = False Then
            strappid = appidnew 'AppId.Value
        End If
        If AppName Is Nothing = False Then
            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
            strappname = Session("AppName")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        End If




        If Page.IsPostBack = False Then
            Try

                'Dim strtran_type As String
                'strtran_type = Request.QueryString("tran_type")
                ' Session.Add("RVPVTranType", strtran_type)



                Dim frmdate As String = ""
                Dim todate As String = ""

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")



                'txtconnection.Value = Session("dbconnectionName")


                '  SetFocus(txtTranId)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\ReceiptsSearch.aspx?tran_type=" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "&appid=" + appidnew, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If



                If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
                    lblheading.Text = "Receipt List"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
                    lblheading.Text = "Cash Payment List"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                    lblheading.Text = "Bank Payment List"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
                    lblheading.Text = "Deposit List"
                End If

                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                '  pnlSearch.Visible = False
                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)

                txtFromDate.Text = ""
                txtTodate.Text = ""
                ' txtFromRecvAmt.Text = ""

                '***************
                'Record list will be according to the Changing the year  
                'If Not (Session("changeyear") Is Nothing) Then
                '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                '    If Session("changeyear") = Year(Now).ToString Then
                '        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                '    Else
                '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                '    End If

                '    txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                '    txtTodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

                'Else
                '    txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                '    txtTodate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")           ' --
                'End If
                ''***************


                'txtToRecvAmt.Text = ""

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccCode, "acctcode", "acctname", "select acctcode,acctname from acctmast  where bankyn='Y' ", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlAccName, "acctname", "acctcode", "select acctcode,acctname from acctmast  where bankyn='Y' ", True)

                '        strSqlQry = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                '        " and  bankyn='Y' order by acctcode "
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", strSqlQry, True)

                '        strSqlQry = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                '        " and  bankyn='Y'  order by acctname"
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", strSqlQry, True)


                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlBankName, "other_bank_master_des", "other_bank_master_code", "select other_bank_master_des,other_bank_master_code from customer_bank_master where active=1 ", True)
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlBankCode, "other_bank_master_code", "other_bank_master_des", "select other_bank_master_des,other_bank_master_code from customer_bank_master where active=1 ", True)

                '        ddlType.Attributes.Add("onchange", "javascript:FillBankCashDet('" + CType(ddlType.ClientID, String) + "','" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")
                '        ddlAccCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")

                '        ddlAccCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")
                '        ddlAccName.Attributes.Add("onchange", "javascript:FillName('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")

                '        ddlBankCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlBankCode.ClientID, String) + "','" + CType(ddlBankName.ClientID, String) + "')")
                '        ddlBankName.Attributes.Add("onchange", "javascript:FillName('" + CType(ddlBankCode.ClientID, String) + "','" + CType(ddlBankName.ClientID, String) + "')")

                '        FillGrid("tran_id", "DESC")

                '        Dim typ As Type
                '        typ = GetType(DropDownList)

                '        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                '            ddlAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '            ddlAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '            ddlBankName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '            ddlBankCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '        End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        'txtDivcode.Value = ViewState("divcode")
        'btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        'ClientScript.GetPostBackEventReference(Me, String.Empty)


    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)

            txtdivcode.Value = ViewState("divcode")
            If Page.IsPostBack = False Then
                'If rbtnadsearch.Checked = True Then

                'FillCashBankDetails()
                'End If
                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '
                FillGridNew()

                'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ReceiptsWindowPostBack") Then
                '    btnSearch_Click(sender, e)
                'End If


            Else

                ' btnClear_Click(sender, e)
            End If
            btnvsprocess.Attributes.Add("onclick", "javascript:if(confirm('return true;")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region
    Private Sub FillCashBankDetails()
        Dim strSqlQry1, strSqlQry2, strSqlQry3 As String
        strSqlQry1 = ""
        strSqlQry2 = ""
        strSqlQry3 = ""
        'If ddlType.Items(ddlType.SelectedIndex).Text = "Bank" Then
        '    strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='B'  and  bankyn='Y' order by acctcode "
        '    strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctname"
        '    strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctcode"
        'ElseIf ddlType.Items(ddlType.SelectedIndex).Text = "Cash" Then
        '    strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='C'  and  bankyn='Y' order by acctcode "
        '    strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctname"
        '    strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctcode"
        'Else
        '    strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and  bankyn='Y' order by acctcode "
        '    strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and  bankyn='Y'  order by acctname"
        '    strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
        '    " and    bankyn='Y'  order by acctcode"
        'End If
        If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctcode"
        ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctcode"
        ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and    bankyn='Y'  order by acctcode"
        End If
        'If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Or CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Or CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
        '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", strSqlQry1, True, ddlAccCode.Value)
        '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", strSqlQry2, True, ddlAccName.Value)
        'End If
    End Sub
    Private Function BuildConditionNew(ByVal straccountvalue As String, ByVal strdocumentvalue As String, ByVal strbankvalue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If straccountvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(customer_bank_master.other_bank_master_des) IN (" & Trim(straccountvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(straccountvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strdocumentvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.tran_id) IN (" & Trim(strdocumentvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.tran_id) IN (" & Trim(strdocumentvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strbankvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(receipt_master_new.tran_id) IN (" & Trim(strbankvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(receipt_master_new.tran_id) IN (" & Trim(strbankvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strTextValue <> "" Then
            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then
                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = "(upper(customer_bank_master.other_bank_master_des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
                        'Else
                        'strWhereCond1 = strWhereCond1 & " OR  (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    End If
                End If
            Next



            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If

        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),customer_bank_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),customer_bank_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),customer_bank_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),customer_bank_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function
    Private Sub FillGridNew()
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strbankvalue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim straccountvalue As String = ""
        Dim strdocumentvalue As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "ACCOUNTNAME" Then
                        If straccountvalue <> "" Then
                            straccountvalue = straccountvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            straccountvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "DOCUMENTNO" Then
                        If strdocumentvalue <> "" Then
                            strdocumentvalue = strdocumentvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strdocumentvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "BANKNAME" Then
                        If strbankvalue <> "" Then
                            strbankvalue = strbankvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strbankvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                    End If
                Next
            End If
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            strBindCondition = BuildConditionNew(straccountvalue, strdocumentvalue, strbankvalue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            ''  gv_SearchResult.Visible = True
            lblMsg.Visible = False
            ''If gv_SearchResult.PageIndex < 0 Then
            ''    gv_SearchResult.PageIndex = 0
            ''End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"
            strSqlQry = "SELECT receipt_master_new.tran_id, receipt_master_new.tran_type, " & _
                             " case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                             " receipt_master_new.tran_type AS Expr1,  " & _
                             " convert(varchar(10),receipt_master_new.receipt_date,103)  as receipt_date, " & _
                             "  convert(varchar(10),receipt_master_new.receipt_tran_date,103) as receipt_tran_date, " & _
                             " case when isnull(tran_type,'')='RV' then receipt_master_new.receipt_credit when isnull(tran_type,'')='CPV' then  receipt_master_new.receipt_debit " & _
                             " when isnull(tran_type,'')='BPV' then receipt_master_new.receipt_debit when isnull(tran_type,'')='DEP' then receipt_master_new.receipt_debit else 0 end receipt_credit , " & _
                             " receipt_master_new.receipt_currency_id,   " & _
                             " receipt_master_new.receipt_received_from, receipt_master_new.receipt_cheque_number, receipt_master_new.receipt_cashbank_code, " & _
                             " customer_bank_master.other_bank_master_des, receipt_master_new.adddate, receipt_master_new.adduser, receipt_master_new.moddate, " & _
                             " receipt_master_new.moduser, receipt_master_new.tran_id + '|' + receipt_master_new.tran_type tranidcomarg " & _
                             " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                             " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                             " where receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"


            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            gv_SearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = pagevaluecus
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherBankMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    '#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    '    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '        Dim frmdate As String = ""
    '        Dim todate As String = ""

    '        'If txtFromDate.Text = "" Or txtTodate.Text = "" Then
    '        '    'Record list will be according to the Changing the year  
    '        '    If Not (Session("changeyear") Is Nothing) Then
    '        '        frmdate = CDate(Session("changeyear") + "/01" + "/01")

    '        '        If Session("changeyear") = Year(Now).ToString Then
    '        '            todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
    '        '        Else
    '        '            todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
    '        '        End If

    '        '        txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
    '        '        txtTodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

    '        '    Else
    '        '        txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
    '        '        txtTodate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")           ' --
    '        '    End If

    '        'End If

    '        ''Record list will be according to the Changing the year  
    '        'If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
    '        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
    '        '    Exit Sub
    '        'End If

    '        'If Session("changeyear") <> Year(CType(txtTodate.Text, Date)).ToString Then
    '        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
    '        '    Exit Sub
    '        'End If


    '        FillGrid("tran_id", "DESC")
    '    End Sub
    '#End Region
    '#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    '    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
    '        txtTranId.Text = ""
    '        ddlType.Value = "[Select]"
    '        ddlStatus.Value = "[Select]"
    '        ddlAccCode.Value = "[Select]"
    '        ddlAccName.Value = "[Select]"
    '        ddlBankCode.Value = "[Select]"
    '        ddlBankName.Value = "[Select]"
    '        '    txtFromDate.Text = ""
    '        '   txtTodate.Text = ""
    '        txtFromRecvAmt.Text = ""
    '        txtToRecvAmt.Text = ""
    '        txtdesc.Text = ""
    '        FillGrid("tran_id", "DESC")
    '    End Sub
    '#End Region
#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            Dim strpop As String = ""
            Dim actionstr As String
            actionstr = ""


            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As String
            Dim lbltrantype As String

            lblId = e.CommandArgument.ToString.Split("|")(0).ToString
            lbltrantype = e.CommandArgument.ToString.Split("|")(1).ToString

            Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),receipt_tran_date,111) from receipt_master_new where receipt_div_id='" & ViewState("divcode") & "' and tran_id='" + lblId + "'")

            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master where div_code='" & ViewState("divcode") & "'")

            If e.CommandName = "EditRow" Then


                If Validateseal(lblId) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If




                If mindate1 <> "" Then
                    If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                        Return
                    End If
                End If

                actionstr = "Edit"
                'strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Trim, String))
                'Response.Redirect("Receipts.aspx", False)

                If Validateseal(lblId) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                If mindate1 <> "" Then
                    If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                        Return
                    End If
                End If

                actionstr = "Delete"
                'strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Trim, String))
                'Response.Redirect("Receipts.aspx", False)
                actionstr = "View"
                'strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Trim, String))
                'Response.Redirect("Receipts.aspx", False)
                actionstr = "Copy"
                'strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('Receipts.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Trim, String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','Receipts');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "ViewLog" Then
                actionstr = "ViewLog"
                'strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Trim, String) + "','Journal','width=500,height=300 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Trim, String) + "','Journal');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "lnkChkPrint" And CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                actionstr = "print"
                'strpop = "window.open('chequeprint.aspx?type=" + CType(lbltrantype.Trim, String) + "&tranid=" + CType(lblId.Trim, String) + "','Journal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('chequeprint.aspx?type=" + CType(lbltrantype.Trim, String) + "&divid=" & ViewState("divcode") & "&tranid=" + CType(lblId.Trim, String) + "','Journal');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("tran_id", "DESC")
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
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

                'strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] , " & _
                '      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Receipt Date], " & _
                '     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Receipt Tran Date],  " & _
                '     " receipt_master_new.receipt_credit as [Receipt Credit] , receipt_master_new.receipt_currency_id as [Currency],   " & _
                '     " receipt_master_new.receipt_received_from as [Receipt Received From], receipt_master_new.receipt_cheque_number as [Receipt Cheque Number], " & _
                '     " receipt_master_new.receipt_cashbank_code as [Receipt Cash Bank Code], " & _
                '     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
                '     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
                '     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                '     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                '     " where receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"

                If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
                    strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] ,case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status,  " & _
                      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Receipt Date], " & _
                     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Tran Date],  " & _
                     " receipt_master_new.receipt_credit as [Amount Received] , receipt_master_new.receipt_currency_id as [Currency],   " & _
                     " receipt_master_new.receipt_received_from as [Received From], receipt_master_new.receipt_cheque_number as [Cheque Number], " & _
                     " receipt_master_new.receipt_cashbank_code as [Cash Bank Code], " & _
                     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                     " where receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
                    strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] ,case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status , " & _
                      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Cash Payment Date], " & _
                     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Tran Date],  " & _
                     " receipt_master_new.receipt_debit as [Amount Paid] , receipt_master_new.receipt_currency_id as [Currency],   " & _
                     " receipt_master_new.receipt_received_from as [Paid To], receipt_master_new.receipt_cheque_number as [Cheque Number], " & _
                     " receipt_master_new.receipt_cashbank_code as [Cash Bank Code], " & _
                     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                     " where receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                    strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] ,case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status , " & _
                      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Bank Payment Date], " & _
                     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Tran Date],  " & _
                     " receipt_master_new.receipt_debit as [Amount Paid] , receipt_master_new.receipt_currency_id as [Currency],   " & _
                     " receipt_master_new.receipt_received_from as [Paid To], receipt_master_new.receipt_cheque_number as [Cheque Number], " & _
                     " receipt_master_new.receipt_cashbank_code as [Cash Bank Code], " & _
                     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                     " where receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
                    strSqlQry = "SELECT  receipt_master_new.tran_id as [Tran ID], receipt_master_new.tran_type as [Tran Type] ,case isnull(receipt_master_new.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status , " & _
                      "  convert(varchar(10),receipt_master_new.receipt_date,103) as [Deposit Date], " & _
                     " convert(varchar(10),receipt_master_new.receipt_tran_date,103) as [Tran Date],  " & _
                     " receipt_master_new.receipt_debit as [Amount Deposit] , receipt_master_new.receipt_currency_id as [Currency],   " & _
                     " receipt_master_new.receipt_received_from as [Deposit To], receipt_master_new.receipt_cheque_number as [Cheque Number], " & _
                     " receipt_master_new.receipt_cashbank_code as [Cash Bank Code], " & _
                     " customer_bank_master.other_bank_master_des as [Other Bank Master Des ], receipt_master_new.adddate as [Date Created], receipt_master_new.adduser as[User Created] ," & _
                     "  receipt_master_new.moddate as [Date Modified], receipt_master_new.moduser  as [User Modified]" & _
                     " FROM  receipt_master_new LEFT OUTER JOIN  " & _
                     " customer_bank_master ON receipt_master_new.receipt_customer_bank = customer_bank_master.other_bank_master_code  " & _
                     " where receipt_master_new.receipt_div_id='" & ViewState("divcode") & "' and receipt_master_new.tran_type='" & CType(ViewState("ReceiptsSearchRVPVTranType"), String) & "'"
                End If


                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " and  " & BuildCondition() & " ORDER BY tran_id "
                Else
                    strSqlQry = strSqlQry & " ORDER BY tran_id "
                End If

                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "cancellation")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReceiptsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Function validaterpt() As Boolean
        If txtFromDate.Text = "" Or txtTodate.Text = "" Then

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Fill the From and To date');", True)

            Return False
        End If
        Return True


    End Function

    Public Function Validateseal(ByVal tranid) As Boolean
        Try


            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  receipt_master_new where receipt_div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("tran_state")) = False Then
                        If ds.Tables(0).Rows(0)("tran_state") = "S" Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim Pageame As String = ""
        If validaterpt() Then
            If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
                ViewState.Add("Pageame", "RecieptsReport")
                Pageame = "ReceiptsReport"
            ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "PV" Then
                ViewState.Add("Pageame", "PaymentsReport")
                Pageame = "PaymentsReport"
            End If

            ViewState.Add("Pageame", "GLtrialbalReport")
            ViewState.Add("BackPageName", "rptRV_PVreport.aspx")
            Try


                Dim strfromdate, strreporttype, strtodate, strtrantype, strtranid, strbanktype, strbankcode, strbank, poststate, strvoucher As String
                'strclosing = ddlclosing.SelectedIndex.ToString

                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
                strtodate = Mid(Format(CType(txtTodate.Text, Date), "yyyy/MM/dd"), 1, 10)
                ''strreporttype = ddlrpttype.SelectedIndex()
                ''strtrantype = CType(ViewState("ReceiptsSearchRVPVTranType"), String)
                ''strtranid = txtTranId.Text
                ''strbanktype = IIf(ddlType.Value = "[Select]", "", ddlType.Value)
                ''strbankcode = IIf(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text = "[Select]", "", ddlAccCode.Items(ddlAccCode.SelectedIndex).Text)
                ''strbank = IIf(ddlBankCode.Items(ddlBankCode.SelectedIndex).Text = "[Select]", "", ddlBankCode.Items(ddlBankCode.SelectedIndex).Text)
                ''poststate = IIf(ddlStatus.Value = "[Select]", "", ddlStatus.Value)
                strvoucher = "All"
                'Response.Redirect("rptRV_PVreport.aspx?frmdate=" & strfromdate & "&todate=" & strtodate & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&type=" & strreporttype, False)
                Dim strpop As String = ""
                'strpop = "window.open('rptRV_PVreport.aspx?BackPageName=rptRV_PVreport.aspx&Pageame=" & Pageame & "&frmdate=" & strfromdate & "&todate=" & strtodate & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&type=" & strreporttype & "&poststate=" & poststate & "&voucher=" & strvoucher & "','RepRVPV','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptRV_PVreport.aspx?BackPageName=rptRV_PVreport.aspx&Pageame=" & Pageame & "&divid=" & ViewState("divcode") & "&frmdate=" & strfromdate & "&todate=" & strtodate & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&type=" & strreporttype & "&poststate=" & poststate & "&voucher=" & strvoucher & "','RepRVPV');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReceiptsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try

        End If

    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowIndex = -1 Then
            If e.Row.RowType = DataControlRowType.Header Then
                If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
                    e.Row.Cells(GridCol.receipt_date).Text = "Receipt Date"
                    e.Row.Cells(GridCol.receipt_credit).Text = "Amount Received"
                    e.Row.Cells(GridCol.receipt_received_from).Text = "Received From"

                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "CPV" Then
                    e.Row.Cells(GridCol.receipt_date).Text = "Cash Payment Date"
                    e.Row.Cells(GridCol.receipt_credit).Text = "Amount Paid"
                    e.Row.Cells(GridCol.receipt_received_from).Text = "Paid To"

                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                    e.Row.Cells(GridCol.receipt_date).Text = "Bank Payment Date"
                    e.Row.Cells(GridCol.receipt_credit).Text = "Amount Paid"
                    e.Row.Cells(GridCol.receipt_received_from).Text = "Paid To"
                ElseIf CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "DEP" Then
                    e.Row.Cells(GridCol.receipt_date).Text = "Deposit Date"
                    e.Row.Cells(GridCol.receipt_credit).Text = "Amount Deposit"
                    e.Row.Cells(GridCol.receipt_received_from).Text = "Deposit To"

                End If
                Exit Sub
            End If
        End If

        If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "RV" Then
            e.Row.Cells(GridCol.chqprint).Width = "0"
        End If



    End Sub

    Protected Sub TemplateFieldBind(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Select Case sender.ID

                Case "lblRecievedFrom"
                    sender.Text = Eval("receipt_received_from")

                Case "lnkChkPrint"

                    If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                        sender.Visible = True
                    Else
                        sender.Visible = False
                    End If

                    'If CType(ViewState("ReceiptsSearchRVPVTranType"), String) = "BPV" Then
                    '    For Each row As GridViewRow In gv_SearchResult.Rows
                    '        row.Cells(20).Visible = True
                    '    Next
                    'Else
                    '    For Each row As GridViewRow In gv_SearchResult.Rows
                    '        row.Cells(20).Visible = False
                    '    Next
                    'End If

            End Select

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try


    End Sub

    Protected Sub btnChecking_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session.Add("State", "New")
        Response.Redirect("Receipts.aspx", False)
        Dim strpop As String = ""
        Dim actionstr As String = ""
        actionstr = "New"
        'strpop = "window.open('Receipts_old.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','ReceiptsNew','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('Receipts_old.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&divid=" & ViewState("divcode") & "&RVPVTranType=" + CType(ViewState("ReceiptsSearchRVPVTranType"), String) + "','ReceiptsNew');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "jsfunction('" & btnChecking.ClientID & "','true')", True)

    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        ViewState.Add("divcode", divid)


        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then


                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case 16 'changed by mohamed on 27/08/2018
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
End Class
