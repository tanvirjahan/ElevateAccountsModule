'------------================--------------=======================------------------================
'   Module Name    :    DebitNoteSearch.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class DebitNoteSearch
    Inherits System.Web.UI.Page

#Region "Enum GridCol"
    Enum GridCol

        DocNo = 1
        DocType = 2
        status = 3
        Type = 4
        FDate = 5
        Code = 6
        Name = 7
        Amount = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 13
        View = 14
        Delete = 15
        Copy = 16
        Cancel = 17
        UndoCancel = 18
    End Enum
#End Region

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objectcl As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim strappid As String = ""
    Dim strappname As String = ""
    Dim dtt As DataTable

#End Region
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


    Private Function BuildConditionNew(ByVal strDocValue As String, ByVal strcusValue As String, ByVal strsupValue As String, ByVal stragentValue As String, ByVal strstatusValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strDocValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(trdpurchase_master.tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(trdpurchase_master.tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            End If

        End If

        If strcusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(view_account.des) IN (" & Trim(strcusValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(view_account.des) IN (" & Trim(strcusValue.Trim.ToUpper) & ")"
            End If
        End If

        If strsupValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(view_account.des) IN (" & Trim(strsupValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(view_account.des) IN (" & Trim(strsupValue.Trim.ToUpper) & ")"
            End If
        End If

        If stragentValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(view_account.des) IN (" & Trim(stragentValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(view_account.des) IN (" & Trim(stragentValue.Trim.ToUpper) & ")"
            End If
        End If


        If strstatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(trdpurchase_master.post_state) IN (" & Trim(strstatusValue.Trim.ToUpper) & ") and div_code='" & ViewState("divcode") & "' "
            Else
                strWhereCond = strWhereCond & " AND  upper(trdpurchase_master.post_state) IN (" & Trim(strstatusValue.Trim.ToUpper) & ") and div_code='" & ViewState("divcode") & "'"
            End If
        End If
        

        'ElseIf strTextValue = "UNPOSTED" Then
        '    strTextValue = "U"
        'End If
        If strTextValue <> "" Then
            Dim strValue2 As String = ""
            Dim strUNPOSTED = "UNPOSTED"
            Dim strPOSTED = "POSTED"
            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then

                    If strValue = "POSTED" Then

                        strValue2 = "P"
                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(trdpurchase_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or upper(trdpurchase_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_code='" & ViewState("divcode") & "'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(trdpurchase_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(trdpurchase_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_code='" & ViewState("divcode") & "'"
                        End If

                    ElseIf strValue = "UNPOSTED" Then
                        strValue2 = "U"

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(trdpurchase_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or upper(trdpurchase_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_code='" & ViewState("divcode") & "'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(trdpurchase_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(trdpurchase_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_code='" & ViewState("divcode") & "'"
                        End If
                    Else

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(trdpurchase_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(trdpurchase_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' and div_code='" & ViewState("divcode") & "'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(trdpurchase_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(trdpurchase_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' and div_code='" & ViewState("divcode") & "'"
                        End If
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),trdpurchase_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),trdpurchase_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),trdpurchase_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),trdpurchase_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "T" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),trdpurchase_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),trdpurchase_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If


        BuildConditionNew = strWhereCond
    End Function
    Private Sub FillGridNew()


        dtt = Session("sDtDynamic")
        Dim strDocValue As String = ""
        Dim strcusValue As String = ""
        Dim strsupValue As String = ""
        Dim stragentValue As String = ""
        Dim strstatusValue As String = ""
        Dim strTextValue As String = ""

        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "DOCUMENTNO" Then
                        If strDocValue <> "" Then
                            strDocValue = strDocValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDocValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strcusValue <> "" Then
                            strcusValue = strcusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strcusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Then
                        If strsupValue <> "" Then
                            strsupValue = strsupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strsupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIERAGENT" Then
                        If stragentValue <> "" Then
                            stragentValue = stragentValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            stragentValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "STATUS" Then
                        If dtt.Rows(i)("Value").ToString = "POSTED" Then
                            If strstatusValue <> "" Then
                                strstatusValue = strstatusValue + ",'" + "P" + "'"
                            Else
                                strstatusValue = "'" + "P".ToString + "'"
                            End If
                        Else
                            If strstatusValue <> "" Then
                                strstatusValue = strstatusValue + ",'" + "U" + "'"
                            Else
                                strstatusValue = "'" + "U".ToString + "'"
                            End If
                        End If
                    End If
                    
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString + ""
                        Else
                            strTextValue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If
                    End If


                Next
            End If
            Dim pagevaluecs = RowsPerPageCS.SelectedValue
            strBindCondition = BuildConditionNew(strDocValue, strcusValue, strsupValue, stragentValue, strstatusValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = "select distinct tran_id,tran_type, case isnull(trdpurchase_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state, " & _
                          " tran_date,supcode,view_account.des as supname, " & _
                          " [acctype]=case when acc_type='C' then 'Customer' when acc_type='S' then 'Supplier' when acc_type='A' then 'Supplier Agent' end, " & _
                          " total,adddate,adduser,moddate,moduser from trdpurchase_master, view_account " & _
                          " where trdpurchase_master.supcode = view_account.code And trdpurchase_master.acc_type = view_account.type and trdpurchase_master.div_id='" & ViewState("divcode") & "' and " _
                          & " trdpurchase_master.tran_type = '" & CType(ViewState("CNDNOpen_type"), String) & "'"


            Dim strorderby As String = Session("strsortDebitExpression")
            Dim strsortorder As String = "ASC"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " AND " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
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
                gv_SearchResult.PageSize = pagevaluecs
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
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
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("CNDNOpen_type", Request.QueryString("tran_type"))
        Session.Add("CNDNOpen_type", Request.QueryString("tran_type"))

        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        ViewState.Add("divcode", divid)
        hdndivcode.Value = ViewState("divcode")
        hdntrantype.value = Request.QueryString("tran_type")

        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
        strappname = Session("AppName")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        If appid Is Nothing = False Then
            strappid = appid
        End If
       


        If Page.IsPostBack = False Then
            Try

                '  'txtconnection.Value = Session("dbconnectionName")

                '  'SetFocus(txtDocNo)


                Dim frmdate As String = ""
                Dim todate As String = ""


                RowsPerPageCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\DebitNoteSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String) & "&appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy, GridCol.Cancel, GridCol.UndoCancel)

                End If
                If ViewState("CNDNOpen_type") = "DN" Then
                    lblHeading.Text = "Debit Note"
                ElseIf ViewState("CNDNOpen_type") = "CN" Then
                    lblHeading.Text = "Credit Note"
                End If
                '---check    ddlDocType.Value = CType(Session("CNDNOpen_type"), String)

                Session.Add("strsortDebitExpression", "tran_id")
                Session.Add("strsortDebitdirection", SortDirection.Ascending)
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic

                dtt = Session("sDtDynamic")
                FillGridNew()


                'If dpFromDate.txtDate.Text = "" Then
                '    dpFromDate.txtDate.Text = objectcl.GetSystemDateOnly
                'End If

                'If dpToDate.txtDate.Text = "" Then
                '    dpToDate.txtDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly)
                'End If

                ''Record list will be according to the Changing the year  
                'If Not (Session("changeyear") Is Nothing) Then
                '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                '    If Session("changeyear") = Year(Now).ToString Then
                '        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                '    Else
                '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                '    End If

                '    dpFromDate.txtDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                '    dpToDate.txtDate.Text = Format(CType(todate, Date), "dd/MM/yyy")

                'Else
                '    dpFromDate.txtDate.Text = ""
                '    dpToDate.txtDate.Text = ""
                'End If

                FillGridNew()
                FillDDL()
                'FillGrid("tran_id")
                ''' FillGridWithOrderByValues()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ' ''ddlCustomer.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlCustomerName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlDocType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                ' ''btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
                ' ''ddlType.Attributes.Add("onchange", "javascript:FillCustDDL('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "','" + CType(lblCustCode.ClientID, String) + "','" + CType(lblCustName.ClientID, String) + "')")
                ' ''ddlCustomer.Attributes.Add("onchange", "javascript:FillCodeName('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "','" + CType(txtcustcode.ClientID, String) + "','" + CType(txtcustname.ClientID, String) + "')")
                ' ''ddlCustomerName.Attributes.Add("onchange", "javascript:FillNameCode('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "','" + CType(txtcustcode.ClientID, String) + "','" + CType(txtcustname.ClientID, String) + "')")


                'If dpFromDate.txtDate.Text = "" Then
                '    'dpFromDate.txtDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                '    dpFromDate.txtDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select fdate from toursmaster"), Date), "dd/MM/yyy")
                'End If

                'If dpToDate.txtDate.Text = "" Then
                '    dpToDate.txtDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                '    'dpToDate.txtDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly)
                'End If



                ' ''lblType.Visible = False
                ' ''lblCustCode.Visible = False
                ' ''lblCustName.Visible = False
                ' ''lblFromAmount.Visible = False
                ' ''lblToAmount.Visible = False
                ' ''lblStatus.Visible = False
                ' ''ddlType.Visible = False
                ' ''ddlStatus.Visible = False
                ' ''ddlCustomer.Visible = False
                ' ''ddlCustomerName.Visible = False
                ' ''txtFromAmount.Visible = False
                ' ''txtToAmount.Visible = False
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = True Then
                ' ''If rbtnadsearch.Checked = True And ddlType.Value <> "[Select]" Then
                ' ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select code,des from view_account where div_code='" & ViewState("divcode") & "' and type='" & ddlType.Value & "' order by code", True, txtcustcode.Value)
                ' ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select des,code from view_account where div_code='" & ViewState("divcode") & "' and type='" & ddlType.Value & "'  order by des", True, txtcustname.Value)
                ' ''    lblCustCode.Text = ddlType.Items(ddlType.SelectedIndex).Text + " Code"
                ' ''    lblCustName.Text = ddlType.Items(ddlType.SelectedIndex).Text + " Name"
                ' ''Else
                ' ''    ddlType.Value = "[Select]"
                ' ''    ddlCustomer.Value = "[Select]"
                ' ''    ddlCustomerName.Value = "[Select]"
                ' ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select top 10 code,des from view_account where div_code='" & ViewState("divcode") & "'  order by code", True)
                ' ''    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select top 10 des,code from view_account  where div_code='" & ViewState("divcode") & "'  order by des", True)
                ' ''    lblCustCode.Text = "Code"
                ' ''    lblCustName.Text = "Name"
                ' ''    ' ddlStatus.Value = "[Select]"
                ' ''End If

            End If
            hdndivcode.Value = ViewState("divcode")
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "DebitNoteWindowPostBack") Then
                btnSearch_Click(sender, e)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Private Sub FillDDL()"
    Private Sub FillDDL()
        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select  top 10  code,des from view_account  where div_code='" & ViewState("divcode") & "' order by code", True)
        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select top 10  des,code  from view_account  where div_code='" & ViewState("divcode") & "' order by des", True)
    End Sub
#End Region

    '#Region "Private Function BuildCondition() As String"
    '    Private Function BuildCondition() As String
    '        strWhereCond = ""
    '        If txtDocNo.Value.Trim <> "" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " upper(tran_id) = '" & Trim(txtDocNo.Value.Trim.ToUpper) & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(tran_id) = '" & Trim(txtDocNo.Value.Trim.ToUpper) & "'"
    '            End If
    '        End If

    '        If ddlCustomer.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " upper(supcode) = '" & Trim(CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)) & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(supcode) = '" & Trim(CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)) & "'"
    '            End If
    '        End If
    '        If dpFromDate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = "( (convert(varchar(10),trdpurchase_master.tran_date,111) between convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
    '                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
    '                & " or (convert(varchar(10),trdpurchase_master.tran_date,111) < convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
    '                & "  and convert(varchar(10),trdpurchase_master.tran_date,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
    '            Else
    '                strWhereCond = strWhereCond & " and ( (convert(varchar(10),trdpurchase_master.tran_date,111) between convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
    '                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
    '                & " or (convert(varchar(10),trdpurchase_master.tran_date,111) < convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
    '                & "  and convert(varchar(10),trdpurchase_master.tran_date,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

    '            End If
    '        End If


    '        If txtFromAmount.Value <> "" And txtToAmount.Value <> "" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " (total between " & Val(txtFromAmount.Value) & " and " & Val(txtToAmount.Value) & ") "
    '            Else
    '                strWhereCond = strWhereCond & " AND (total between " & Val(txtFromAmount.Value) & " and " & Val(txtToAmount.Value) & ") "
    '            End If
    '        End If

    '        If ddlType.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " acc_type = '" & CType(ddlType.Value, String) & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND acc_type = '" & CType(ddlType.Value, String) & "'"
    '            End If
    '        End If

    '        If ddlStatus.Value <> "[Select]" Then
    '            If ddlStatus.Value = "Y" Then ''Cancelled
    '                If Trim(strWhereCond) = "" Then
    '                    strWhereCond = " isnull(cancel_state,'')  = '" & ddlStatus.Value & "'"
    '                Else
    '                    strWhereCond = strWhereCond & " AND isnull(cancel_state,'')  = '" & ddlStatus.Value & "'"
    '                End If

    '            Else
    '                If Trim(strWhereCond) = "" Then
    '                    strWhereCond = "  isnull(post_state,'')  = '" & ddlStatus.Value & "' and isnull(cancel_state,'')<>'Y'  "
    '                Else
    '                    strWhereCond = strWhereCond & " AND isnull(post_state,'')  = '" & ddlStatus.Value & "' and isnull(cancel_state,'')<>'Y' "
    '                End If

    '            End If
    '        End If

    '        BuildCondition = strWhereCond
    '    End Function
    '#End Region

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
            strSqlQry = " select tran_id,tran_type,case isnull(trdpurchase_master.cancel_state,'') when 'Y' then 'Cancelled' else (case isnull(trdpurchase_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end) end  as post_state, " & _
                        " tran_date,supcode,view_account.des as supname, " & _
                        " [acctype]=case when acc_type='C' then 'Customer' when acc_type='S' then 'Supplier' when acc_type='A' then 'Supplier Agent' end, " & _
                        " total,adddate,adduser,moddate,moduser from trdpurchase_master, view_account " & _
                        " where trdpurchase_master.supcode = view_account.code And trdpurchase_master.acc_type = view_account.type and trdpurchase_master.div_id='" & ViewState("divcode") & "' and " _
                        & " trdpurchase_master.tran_type = '" & CType(ViewState("CNDNOpen_type"), String) & "'"


            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("DebitNote.aspx", False)
        'ViewState("CNDNOpen_type")

        Dim strpop As String = ""
        'strpop = "window.open('DebitNote.aspx?State=New&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('DebitNote.aspx?State=New&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "&divid=" & ViewState("divcode") & "','DebitNote');"

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblCode")


            Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),tran_date,111) from trdpurchase_master  where div_id='" & ViewState("divcode") & "' and  tran_id='" + lblId.Text + "'")

            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot edit...')", True)
                    Return
                End If

                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If




                If mindate1 <> "" Then
                    If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                        Return
                    End If
                End If


                Dim strpop As String = ""
                'strpop = "window.open('DebitNote.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DebitNote.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('DebitNote.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DebitNote.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Delete...')", True)
                    Return
                End If



                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                If mindate1 <> "" Then
                    If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                        Return
                    End If
                End If

                Dim strpop As String = ""
                'strpop = "window.open('DebitNote.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DebitNote.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Copy...')", True)
                    Return
                End If
                Dim strpop As String = ""
                'strpop = "window.open('DebitNote.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DebitNote.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Cancelrow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DebitNote.aspx", False)

                If Validatecancelled(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Delete...')", True)
                    Return
                End If



                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                If mindate1 <> "" Then
                    If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                        Return
                    End If
                End If

                Dim strpop As String = ""
                'strpop = "window.open('DebitNote.aspx?State=Cancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DebitNote.aspx?State=Cancel&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "undoCancel" Then
                Dim strpop As String = ""
                strpop = "window.open('DebitNote.aspx?State=undoCancel&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote');"
           
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "ViewLog" Then
                Dim strpo As String
                Dim actionstr As String
                actionstr = "ViewLog"
                'strpo = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=500,height=300 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpo = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&trantype=" + CType(ViewState("CNDNOpen_type"), String) + "' " ','Journal');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpo, True)
                If Validatecancelled(lblId.Text) = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot Delete...')", True)
                    Return
                End If



                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                If mindate1 <> "" Then
                    If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                        Return
                    End If
                End If

                Dim strpop As String = ""
                'strpop = "window.open('DebitNote.aspx?State=undoCancel&RefCode=" + CType(lblId.Text.Trim, String) + "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DebitNote.aspx?State=undoCancel&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&CNDNOpen_type=" & ViewState("CNDNOpen_type") & "','DebitNote');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Public Function Validatecancelled(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  trdpurchase_master where div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("cancel_state")) = False Then
                        If ds.Tables(0).Rows(0)("cancel_state") = "Y" Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validatecancelled = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function


    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  trdpurchase_master  where div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
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
            objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim frmdate As String = ""
        Dim todate As String = ""

        ''Record list will be according to the Changing the year  
        'If Not (Session("changeyear") Is Nothing) And dpFromDate.txtDate.Text = "" And dpToDate.txtDate.Text = "" Then
        '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

        '    If Session("changeyear") = Year(Now).ToString Then
        '        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
        '    Else
        '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
        '    End If

        '    dpFromDate.txtDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
        '    dpToDate.txtDate.Text = Format(CType(todate, Date), "dd/MM/yyy")
        'End If



        ''  FillGrid("tran_id")
        ''Record list will be according to the Changing the year  
        'If Session("changeyear") <> Year(CType(dpToDate.txtDate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        'If Session("changeyear") <> Year(CType(dpToDate.txtDate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If


        '''FillGridWithOrderByValues()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ''txtDocNo.Value = ""
        ''txtFromAmount.Value = ""
        ''txtToAmount.Value = ""
        ''ddlCustomer.Value = "[Select]"
        ''ddlCustomerName.Value = "[Select]"
        ''ddlType.Value = "[Select]"
        ''ddlDocType.Value = "[Select]"
        ' ''dpFromDate.txtDate.Text = ""
        ' ''dpToDate.txtDate.Text = ""
        ''ddlStatus.Value = "[Select]"
        ' ''FillGrid("tran_id")
        ''ddlOrderBy.SelectedIndex = 0
        '''  FillGridWithOrderByValues()
    End Sub

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlSearch.Visible = False
        ' ''lblType.Visible = False
        ' ''lblCustCode.Visible = False
        ' ''lblCustName.Visible = False
        ' ''lblFromAmount.Visible = False
        ' ''lblToAmount.Visible = False
        ' ''lblStatus.Visible = False
        ' ''ddlType.Visible = False
        ' ''ddlStatus.Visible = False
        ' ''ddlCustomer.Visible = False
        ' ''ddlCustomerName.Visible = False
        ' ''txtFromAmount.Visible = False
        ' ''txtToAmount.Visible = False



    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlSearch.Visible = True
        ' ''lblType.Visible = True
        ' ''lblCustCode.Visible = True
        ' ''lblCustName.Visible = True
        ' ''lblFromAmount.Visible = True
        ' ''lblToAmount.Visible = True
        ' ''lblStatus.Visible = True  If Trim(BuildCondition) <> "" Then
        ' ''ddlType.Visible = True
        ' ''ddlStatus.Visible = True
        ' ''ddlCustomer.Visible = True
        ' ''ddlCustomerName.Visible = True
        ' ''txtFromAmount.Visible = True
        ' ''txtToAmount.Visible = True

    End Sub

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortDebitExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortDebitExpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortDebitdirection", objUtils.SwapSortDirection(Session("strsortDebitdirection")))
            dataView.Sort = Session("strsortDebitExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortDebitdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                strSqlQry = " SELECT  tran_id as [Doc No],tran_type as [Doc Type]," & _
                            " case isnull(trdpurchase_master.cancel_state,'') when 'Y' then 'Cancelled' else (case isnull(trdpurchase_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end) end  as Status," & _
                            " [Type]=case when acc_type='C' then 'Customer' when acc_type='S' then 'Supplier' when acc_type='A' then 'Supplier Agent' end, " & _
                            " (Convert(Varchar(10),tran_date,103)) as [Date], supcode as [Code],des as [Name],total as [Amount], " & _
                            " (Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created], adduser as [User Created], " & _
                            " (Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified],moduser as [User Modified] " & _
                            " from trdpurchase_master, view_account where trdpurchase_master.supcode = view_account.code And trdpurchase_master.acc_type = view_account.type " & _
                            " and trdpurchase_master.tran_type = '" & CType(ViewState("CNDNOpen_type"), String) & "' and  trdpurchase_master.div_id='" & ViewState("divcode") & "'"

                ' ''If Trim(BuildCondition) <> "" Then
                ' ''    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                ' ''Else
                ' ''    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                ' ''End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "DebitNote")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            Dim strfdate As String = ""
            Dim strldate As String = ""
            Dim strorder As String = "" ' I - ID else Date
            Dim strflag As Integer = 2 ' 2 Debit Note, 3 Credit Note 
            Dim strdiv_code As String = ViewState("divcode")
            ' Dim strrepfilter As String = ""
            Dim strHeader As String = ""
            Dim backPname As String = ""
            'If validaterpt() Then

            If ViewState("CNDNOpen_type") = "DN" Or ViewState("CNDNOpen_type") = "CN" Then
                strHeader = "DebitNoteBrief"
            
            End If

         
                backPname = "~\AccountsModule\DebitNoteSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String)

                ' strSelectionFormula = ""
                ' removed
                ''''''''''''
                'Session.Add("SelectionFormula", strSelectionFormula)
                'Session.Add("ReportTitle", strReportTitle)

                
                strorder = ""
                Dim strcust As String = ""
                Dim fromamount As String = ""
                Dim toamount As String = ""
                Dim docno As String = ""
                Dim poststate As String = ""
            Dim Type As String

            Type = ViewState("CNDNOpen_type")
                If lblHeading.Text = "Debit Note" Then
                    strflag = 2
                Else
                    strflag = 3
                End If
            'strdiv_code = "1"

            If dtt Is Nothing Then
                strcust = ""
                poststate = ""
                docno = ""




            Else
                For i As Integer = 0 To dtt.Rows.Count - 1

                    If dtt.Rows(i)("Code").ToString = "SUPPLIERAGENT" Or dtt.Rows(i)("Code").ToString = "CUSTOMER" Or dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strcust <> "" Then
                            strcust = strcust + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strcust = "'" + dtt.Rows(i)("value").ToString + "'"

                        End If
                    End If



                    If dtt.Rows(i)("Code").ToString = "STATUS" Then


                        If dtt.Rows(i)("Value").ToString = "POSTED" Then
                            If poststate <> "" Then
                                poststate = poststate + ",'" + "P" + "'"
                            Else
                                poststate = "'" + "P".ToString + "'"
                            End If
                        Else
                            If poststate <> "" Then
                                poststate = poststate + ",'" + "U" + "'"
                            Else
                                poststate = "'" + "U".ToString + "'"
                            End If

                        End If

                    End If

                    If dtt.Rows(i)("Code").ToString = "DOCUMENTNO" Then
                        If docno <> "" Then
                            docno = docno + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            docno = "'" + dtt.Rows(i)("value").ToString + "'"

                        End If
                    End If





                Next

            End If


            'Response.Redirect("~\PriceListModule\rptReport.aspx", False)
            'Response.Redirect("~\AccountsModule\rptDebitNotes.aspx", False)

            'Response.Redirect("rptDebitNotes.aspx?BackPageName=" & backPname & "&Pageame=" & strHeader & "&fdate=" & strfdate & "&ldate=" & strldate _
            '& "&order=" & strorder & "&flag=" & strflag & "&div_code=" & strdiv_code & "&DocNo=" & txtDocNo.Value _
            '& "&Customer=" & CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String) _
            '& "&FromAmount=" & txtToAmount.Value & "&ToAmount=" & txtToAmount.Value & "&Type=" & ddlType.Value & "&Status=" & ddlStatus.Value, False)

            If txtFromDate.Text = "" Then
                strfdate = "2017/01/01"
            Else
                strfdate = txtFromDate.Text
            End If
            If txtToDate.Text = "" Then
                strldate = "2020/12/31"
            Else
                strldate = txtToDate.Text
            End If



            Dim strpop As String = ""
            strpop = "window.open('rptDebitNotes.aspx?BackPageName=" & backPname & "&Pageame=" & strHeader & "&fdate=" & strfdate & "&ldate=" & strldate _
            & "&order=" & strorder & "&flag=" & strflag & "&div_code=" & strdiv_code & "&DocNo=" & docno _
            & "&Customer=" & strcust _
            & "&FromAmount=" & fromamount & "&ToAmount=" & toamount & "&Type=" & Type & "&Status=" & poststate & _
            "','RepDebitNote','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNoteSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
 
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DebitNoteSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Function validaterpt() As Boolean
        ' ''If dpFromDate.txtDate.Text = "" Or dpToDate.txtDate.Text = "" Then

        ' ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Fill the From and To date');", True)

        ' ''    Return False
        ' ''End If
        Return True


    End Function




    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit



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
                Case "DOCUMENTNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("DOCUMENTNO", lsProcessCity, "DOCUMENTNO")
                Case "CUSTOMER"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessCity, "C")
                Case "SUPPLIER"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIER", lsProcessCity, "S")
                Case "SUPPLIERAGENT"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIERAGENT", lsProcessCity, "A")
                Case "STATUS"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("STATUS", lsProcessCity, "STATUS")
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
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
       
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()

    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class

