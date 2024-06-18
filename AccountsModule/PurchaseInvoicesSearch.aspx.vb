
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class AccountsModule_PurchaseInvoicesSearch
    Inherits System.Web.UI.Page

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
#End Region

#Region "Enum GridCol"
    Enum GridCol
        tran_id = 0
        status = 1
        tran_date = 2
        acc_type = 3
        acc_code = 4
        postaccount = 5
        supinvno = 6
        fromdate = 7
        todate = 8
        adddate = 9
        adduser = 10
        modate = 11
        moduser = 12
        Edit = 13
        view = 14
        Delete = 15
    End Enum
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim frmdate As String = ""
                Dim todate As String = ""

                txtconnection.Value = Session("dbconnectionName")

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
                                          CType(strappname, String), "AccountsModule\PurchaseInvoicesSearch.aspx", btnAddNew, btnExportToExcel, _
                                          btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.view)

                End If

                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)

                ''Record list will be according to the Changing the year  
                'If Not (Session("changeyear") Is Nothing) Then
                '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                '    If Session("changeyear") = Year(Now).ToString Then
                '        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                '    Else
                '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                '    End If

                '    txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                '    txtToDate.Text = Format(CType(todate, Date), "dd/MM/yyy")

                'Else
                '    txtFromDate.Text = ""
                '    txtToDate.Text = ""
                'End If



                'If txtFromDate.Text = "" Then
                '    txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select fdate from toursmaster"), Date), "dd/MM/yyy")
                'End If

                'If txtToDate.Text = "" Then
                '    txtToDate.Text = objectcl.GetSystemDateOnly(Session("dbconnectionName"))
                'End If

                FillDDL("IsPostBackFalse")
                FillGridWithOrderByValues()
                gv_SearchResult.Columns(0).Visible = True
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSpplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPostToCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPostToName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlStatus.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If


                ddlType.Attributes.Add("onchange", "javascript:FillSupplier('" + CType(ddlType.ClientID, String) + "','" + CType(lblCustCode.ClientID, String) + "','" + CType(lblCustName.ClientID, String) + "','" + CType(ddlSupplierCode.ClientID, String) + "','" + CType(ddlSpplierName.ClientID, String) + "','" + CType(ddlPostToCode.ClientID, String) + "','" + CType(ddlPostToName.ClientID, String) + "')")
                ddlSupplierCode.Attributes.Add("onchange", "javascript:suppliercodechange('" + CType(ddlType.ClientID, String) + "','" + CType(ddlSupplierCode.ClientID, String) + "','" + CType(ddlSpplierName.ClientID, String) + "','" + CType(ddlPostToCode.ClientID, String) + "','" + CType(ddlPostToName.ClientID, String) + "')")
                ddlSpplierName.Attributes.Add("onchange", "javascript:suppliernamechange('" + CType(ddlType.ClientID, String) + "','" + CType(ddlSupplierCode.ClientID, String) + "','" + CType(ddlSpplierName.ClientID, String) + "','" + CType(ddlPostToCode.ClientID, String) + "','" + CType(ddlPostToName.ClientID, String) + "')")
                ddlPostToCode.Attributes.Add("onchange", "javascript:postcodechange('" + CType(ddlPostToCode.ClientID, String) + "','" + CType(ddlPostToName.ClientID, String) + "','" + CType(txtPostCode.ClientID, String) + "','" + CType(txtPostName.ClientID, String) + "')")
                ddlPostToName.Attributes.Add("onchange", "javascript:postnamechange('" + CType(ddlPostToCode.ClientID, String) + "','" + CType(ddlPostToName.ClientID, String) + "','" + CType(txtPostCode.ClientID, String) + "','" + CType(txtPostName.ClientID, String) + "')")

            Else
                If rbtnadsearch.Checked = True Then
                    FillDDL("IsPostBackTrue")
                End If

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoicesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "PurchaseInvoiceWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If

    End Sub
    Private Sub FillDDL(ByVal strType As String)
        Try
            If strType = "IsPostBackFalse" Then
                If ddlType.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select Code,des from view_account where type ='" & ddlType.Value & "'", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select des,Code from view_account where type ='" & ddlType.Value & "'", True)

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select    Code as postaccount ,des as partyname from view_account where type ='" & ddlType.Value & "' order by Code ", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select    des as partyname,Code as postaccount  from view_account where type ='" & ddlType.Value & "' order by des ", True)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select  distinct Code,des from view_account where type in('S','A') order by code ", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select distinct Code,des from view_account where type in('S','A')order by des  ", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select  distinct  Code as postaccount ,des as partyname from view_account where type in('S','A') order by Code ", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select  distinct  des as partyname,Code as postaccount  from view_account  where type in('S','A') order by des ", True)
                End If



            ElseIf strType = "IsPostBackTrue" Then
                If ddlType.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select Code,des from view_account where type ='" & ddlType.Value & "'", True, ddlSupplierCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select Code,des from view_account where type ='" & ddlType.Value & "' ", True, ddlSpplierName.Value)
                    If ddlSupplierCode.Value <> "[Select]" Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select   Code as postaccount ,des as partyname from view_account where type ='" & ddlType.Value & "' and code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by Code ", True, txtPostCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select    des as partyname,Code as postaccount  from view_account where type ='" & ddlType.Value & "' and code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'   order by des ", True, txtPostName.Value)
                    Else
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select    Code as postaccount ,des as partyname from view_account where  type ='" & ddlType.Value & "' order by Code ", True, txtPostCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select    des as partyname,Code as postaccount  from view_account where  type ='" & ddlType.Value & "' order by des ", True, txtPostName.Value)
                    End If
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select Code,des from view_account  where type in('S','A') order by code", True, ddlSupplierCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select Code,des from view_account  where type in('S','A') order by des ", True, ddlSpplierName.Value)
                    If ddlSupplierCode.Value <> "[Select]" Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select   Code as postaccount ,des as partyname from view_account where   type in('S','A') and  code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by Code ", True, txtPostCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select    des as partyname,Code as postaccount  from view_account where type in('S','A') and  code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'   order by des ", True, txtPostName.Value)
                    Else
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select    Code as postaccount ,des as partyname from view_account where   type in('S','A') order by Code ", True, txtPostCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select    des as partyname,Code as postaccount  from view_account where   type in('S','A')  order by des ", True, txtPostName.Value)
                    End If

                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoicesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            'strSqlQry = " select *, [acctype]=case when acc_type='S' then 'Supplier' when acc_type='A' " & _
            '            "then 'Supplier Agent' end ," & _
            '            " from providerinv_header"


            strSqlQry = " select p.tran_id ,Convert(Varchar(10),p.tran_date,103) as tran_date,Convert(Varchar(10),p.fromdate,103) as fromdate, " & _
                      "Convert(Varchar(10),p.todate,103) as todate ," & _
                      "  case isnull(p.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                      "  acctype=case when p.acc_type='S' then 'Supplier' when p.acc_type='A' then 'Supplier Agent' end, " & _
                      "  p.acc_code ,v.des as accname,p.postaccount ,p.supinvno,p.adddate, p.adduser ,p.moddate,p.moduser" & _
                      "  from providerinv_header p,view_account v where p.acc_code=v.code and v.type='S' "

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoicesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Private Function BuildCondition() As String
        strWhereCond = ""
        Try

            If txtInvoiceNo.Value.Trim <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " and upper(p.tran_id) = '" & Trim(txtInvoiceNo.Value.Trim.ToUpper) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(p.tran_id) = '" & Trim(txtInvoiceNo.Value.Trim.ToUpper) & "'"
                End If
            End If

            If ddlType.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " and upper(p.acc_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(p.acc_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
                End If
            End If
            If ddlSupplierCode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " and upper(p.acc_code) = '" & Trim(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(p.acc_code) = '" & Trim(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)) & "'"
                End If
            End If
            If ddlPostToCode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " and upper(p.postaccount) = '" & Trim(CType(ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text, String)) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(p.postaccount) = '" & Trim(CType(ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text, String)) & "'"
                End If
            End If

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " and ( (convert(varchar(10),p.tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                    & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                    & " or (convert(varchar(10),p.tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                    & "  and convert(varchar(10),p.tran_date,111) > convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
                Else
                    strWhereCond = strWhereCond & " and ( (convert(varchar(10),p.tran_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                    & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                    & " or (convert(varchar(10),p.tran_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                    & "  and convert(varchar(10),p.tran_date,111) > convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

                End If
            End If
            If ddlStatus.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " and isnull(p.post_state,'')  = '" & ddlStatus.Value & "'"
                Else
                    strWhereCond = strWhereCond & " AND isnull(p.post_state,'')  = '" & ddlStatus.Value & "'"
                End If
            End If
        Catch ex As Exception

        End Try
        BuildCondition = strWhereCond
    End Function
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("PurchaseInvoices.aspx", False)
        Dim strpop As String = ""
        strpop = "window.open('PurchaseInvoices.aspx?State=New','PurchaseInvoice','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("tran_id", "DESC")
            Case 1
                FillGrid("tran_id", "ASC")
            Case 2
                FillGrid("tran_date", "ASC")
            Case 3
                FillGrid("acc_type", "ASC")
            Case 4
                FillGrid("acc_code", "ASC")
            Case 5
                FillGrid("supinvno", "ASC")
            Case 6
                FillGrid("fromdate", "ASC")
            Case 7
                FillGrid("todate", "ASC")
        End Select
    End Sub
    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "tran_id DESC"
            Case 1
                ExportWithOrderByValues = "tran_id ASC"
            Case 2
                ExportWithOrderByValues = "tran_date ASC"
            Case 3
                ExportWithOrderByValues = "acc_type ASC"
            Case 4
                ExportWithOrderByValues = "acc_code ASC"
            Case 5
                ExportWithOrderByValues = "supinvno ASC"
            Case 6
                ExportWithOrderByValues = "fromdate ASC"
            Case 7
                ExportWithOrderByValues = "todate ASC"
        End Select
    End Function

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridWithOrderByValues()
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblInvoiceNo")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("PurchaseInvoices.aspx", False)

                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If


                Dim strpop As String = ""
                strpop = "window.open('PurchaseInvoices.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','PurchaseInvoice','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("PurchaseInvoices.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('PurchaseInvoices.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','PurchaseInvoice','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("PurchaseInvoices.aspx", False)

                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                Dim strpop As String = ""
                strpop = "window.open('PurchaseInvoices.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','PurchaseInvoice','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoicesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  providerinv_header where tran_id='" + tranid + "' ")
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
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim frmdate As String = ""
        Dim todate As String = ""

        'If txtFromDate.Text = "" Or txtToDate.Text = "" Then
        '    'Record list will be according to the Changing the year  
        '    If Not (Session("changeyear") Is Nothing) Then
        '        frmdate = CDate(Session("changeyear") + "/01" + "/01")

        '        If Session("changeyear") = Year(Now).ToString Then
        '            todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
        '        Else
        '            todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
        '        End If

        '        txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
        '        txtToDate.Text = Format(CType(todate, Date), "dd/MM/yyy")

        '    End If
        'End If

        ''Record list will be according to the Changing the year  
        'If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        'If Session("changeyear") <> Year(CType(txtToDate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If


        FillGridWithOrderByValues()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtInvoiceNo.Value = ""
        ddlType.Value = "[Select]"
        ddlSupplierCode.Value = "[Select]"
        ddlSpplierName.Value = "[Select]"
        ddlPostToCode.Value = "[Select]"
        ddlPostToName.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        ddlStatus.Value = "[Select]"
        FillGridWithOrderByValues()
    End Sub

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            pnlSearch.Visible = False
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoicesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            pnlSearch.Visible = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoicesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
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


    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                strSqlQry = "select tran_id as [Purchase Invoice No] ," & _
                               "  case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status ," & _
                            "Convert(Varchar(10), tran_date,103) as [Purchase Invoice Date], " & _
                            "[Type]=case when acc_type='S' then 'Supplier' when acc_type='A' then 'Supplier Agent' end, " & _
                            "acc_code as [Supplier],postaccount as [Post To],supinvno as [Supplier Invoice No] , " & _
                            "(Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created], adduser as [User Created], " & _
                            "(Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified],moduser as [User Modified] " & _
                            "from providerinv_header "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "PurchaseInvoice")

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
            'Session.Add("CurrencyCode", txtgroupid.Text.Trim)
            'Session.Add("CurrencyName", txtmealname.Text.Trim)
            'Response.Redirect("rptCurrencies.aspx", False)
            Dim strfdate As String = ""
            Dim strldate As String = ""
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            Dim strHeader As String = ""
            Dim strdiv_code As String = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            Session("ColReportParams") = Nothing

            If ddlReportType.SelectedValue = "Brief" Then
                ' Session.Add("Pageame", "PurchaseInvoiceBrief")
                strHeader = "PurchaseInvoiceBrief"
            ElseIf ddlReportType.SelectedValue = "Detailed" Then
                'Session.Add("Pageame", "PurchaseInvoiceDetail")
                strHeader = "PurchaseInvoiceDetail"
            End If
            ' Session.Add("BackPageName", "PurchaseInvoicesSearch.aspx")


            'If txtInvoiceNo.Value <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ;Purchase Invoice No: " & txtInvoiceNo.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {providerinv_header.tran_id} = " & txtInvoiceNo.Value.Trim
            '    Else
            '        strReportTitle = "Purchase Invoice No: " & txtInvoiceNo.Value.Trim
            '        strSelectionFormula = "{providerinv_header.tran_id} = " & txtInvoiceNo.Value.Trim
            '    End If
            'End If
            'If ddlType.Value <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ;Type: " & ddlType.Items(ddlType.SelectedIndex).Text
            '        strSelectionFormula = strSelectionFormula & " and {providerinv_header.acc_type} = " & ddlType.Value
            '    Else
            '        strReportTitle = "Type: " & ddlType.Items(ddlType.SelectedIndex).Text
            '        strSelectionFormula = "{providerinv_header.acc_type} =" & ddlType.Value
            '    End If
            'End If
            'If ddlSupplierCode.Value <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ;Supplier Code: " & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
            '        strSelectionFormula = strSelectionFormula & " and {providerinv_header.acc_code} = " & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
            '    Else
            '        strReportTitle = "Supplier Code: " & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
            '        strSelectionFormula = "{providerinv_header.acc_code} = " & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
            '    End If
            'End If
            'If ddlPostToCode.Value <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ;Post Account Code: " & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text
            '        strSelectionFormula = strSelectionFormula & " and {providerinv_header.postaccount} = " & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text
            '    Else
            '        strReportTitle = "Post Account Code: " & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text
            '        strSelectionFormula = "{providerinv_header.postaccount} = " & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text
            '    End If
            'End If
            strfdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            strldate = Format(CType(txtToDate.Text, Date), "yyyy/MM/dd")

            'If txtFromDate.Text <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ;From Date: " & Format(CType(strfdate, Date), "dd/MM/yyyy")
            '        'strSelectionFormula = strSelectionFormula & " and {providerinv_header.postaccount} = '" & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text & "'"
            '    Else
            '        strReportTitle = "From Date: " & Format(CType(strfdate, Date), "dd/MM/yyyy")
            '        'strSelectionFormula = "{providerinv_header.postaccount} = '" & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text & "'"
            '    End If
            'End If
            'If txtFromDate.Text <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ;To Date: " & Format(CType(strldate, Date), "dd/MM/yyyy")
            '        'strSelectionFormula = strSelectionFormula & " and {providerinv_header.postaccount} = '" & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text & "'"
            '    Else
            '        strReportTitle = "To Date: " & Format(CType(strldate, Date), "dd/MM/yyyy")
            '        'strSelectionFormula = "{providerinv_header.postaccount} = '" & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)

            ' Session.Add("SelectionFormula", strSelectionFormula)
            '  Session.Add("ReportTitle", strReportTitle)

            'strorder = IIf(ddlOrderBy.SelectedIndex = 0, "I", "")
            'Response.Redirect("rptPuchaseInvoice.aspx?BackPageName=PurchaseInvoicesSearch.aspx&Pageame=" & strHeader & "&fdate=" & strfdate & "&ldate=" & strldate _
            '& "&div_code=" & strdiv_code & "&reporttitle=" & strReportTitle & "&SelectionFormula=" & strSelectionFormula & "&ReportTitle=" & strReportTitle, False)

            Dim strpop As String = ""
            strpop = "window.open('rptPuchaseInvoice.aspx?BackPageName=PurchaseInvoicesSearch.aspx&Pageame=" & strHeader & "&fdate=" & strfdate & "&ldate=" & strldate _
            & "&div_code=" & strdiv_code & "&InvoiceNo=" & txtInvoiceNo.Value & "&Type=" & ddlType.Value & "&SupplierCode=" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text _
            & "&PostToCode=" & ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text & "','RepPurchaseInvoice','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoicesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PurchaseInvoiceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class

'If ddlType.Value = "S" Then
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierCode, "Code", "des", "select Code,des from view_account where type ='S'", True, ddlSupplierCode.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSpplierName, "des", "Code", "select Code,des from view_account where type ='S'", True, ddlSpplierName.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToCode, "postaccount", "partyname", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True, ddlPostToCode.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToName, "partyname", "postaccount", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True, ddlPostToName.Value)
'ElseIf ddlType.Value = "A" Then
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierCode, "Code", "des", "select Code,des from view_account where type ='A'", True, ddlSupplierCode.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSpplierName, "des", "Code", "select Code,des from view_account where type ='A'", True, ddlSpplierName.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToCode, "postaccount", "supagentname", "select distinct view_account.postaccount,supplier_agents.supagentname from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'", True, ddlPostToCode.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToName, "supagentname", "postaccount", "select distinct supplier_agents.supagentname,view_account.postaccount from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'", True, ddlPostToName.Value)
'End If

'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToCode, "postaccount", "partyname", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToName, "partyname", "postaccount", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToCode, "postaccount", "supagentname", "select distinct view_account.postaccount,supplier_agents.supagentname from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToName, "supagentname", "postaccount", "select distinct supplier_agents.supagentname,view_account.postaccount from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'", True)