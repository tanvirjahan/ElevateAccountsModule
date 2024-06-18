'------------------------------------------------------------------------------------------------
'   Module Name    :    JournalSearch 
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class ExchSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region
#Region "Enum GridCol"
    Enum GridCol
        TransactionId = 0
        DocumentNo = 1
        DocType = 2
        Status = 3
        JournalDate = 4
        PostedDate = 5
        Narration = 6
        DateCreated = 7
        UserCreated = 8
        DateModified = 9
        UserModified = 10
        Edit = 11
        View = 12
        Delete = 13
        Copy = 14
        cancel = 15
        undocancel = 16

    End Enum
#End Region
#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlSearch.Visible = False
    End Sub
#End Region
#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlSearch.Visible = True
    End Sub
#End Region
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("Journal.aspx", False)
        Dim strpop As String = ""
        strpop = "window.open('Exchange.aspx?State=New','Exchange','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select journal_div_id,tran_id,tran_type,  " & _
                        "case isnull(cancel_state,'') when 'Y' then 'Cancelled' else (case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end) end  as post_state ," & _
                        " convert(varchar(10),journal_date ,103) as journal_date ," & _
                        " convert(varchar(10),journal_tran_date,103) as journal_tran_date,journal_mrv,journal_salesperson_code, " & _
                        " journal_narration, journal_tran_state, adddate, adduser, moddate, moduser, div_id, basedebit, basecredit " & _
                        " from exchange_master "
            Dim strBuild As String = Trim(BuildCondition)
            If strBuild <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBuild & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " WHERE " & Trim(BuildCondition1) & "  ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
                lblMessg.Visible = False
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMessg.Visible = True
                lblMessg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExchSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                strWhereCond = "( (convert(varchar(10),exchange_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),exchange_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),exchange_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),exchange_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),exchange_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),exchange_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If

        BuildCondition1 = strWhereCond
    End Function
#End Region


#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtTranId.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(exchange_master.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(exchange_master.tran_id) = '" & Trim(txtTranId.Text.Trim) & "'"
            End If
        End If

        If txtdesc.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " exchange_master.journal_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
            Else
                strWhereCond = strWhereCond & " AND  exchange_master.journal_narration like '%" & Trim(txtdesc.Text.Trim) & "%'"
            End If
        End If


        If txtFromDate.Text.Trim <> "" And txtTodate.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),exchange_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),exchange_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),exchange_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),exchange_master.journal_date,111) between convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),exchange_master.journal_date,111) < convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),exchange_master.journal_date,111) > convert(varchar(10), '" & Format(CType(txtTodate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If
        If ddlStatus.Value <> "[Select]" Then
            If ddlStatus.Value = "Y" Then ''Cancelled
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " isnull(cancel_state,'')  = '" & ddlStatus.Value & "' "
                Else
                    strWhereCond = strWhereCond & " AND isnull(cancel_state,'')  = '" & ddlStatus.Value & "' "
                End If

            Else
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " isnull(post_state,'')  = '" & ddlStatus.Value & "' and isnull(cancel_state,'')<>'Y'"
                Else
                    strWhereCond = strWhereCond & " AND isnull(post_state,'')  = '" & ddlStatus.Value & "' and isnull(cancel_state,'')<>'Y'"
                End If
            End If
        End If
        BuildCondition = strWhereCond
    End Function
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                SetFocus(txtTranId)
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim frmdate As String = ""
                Dim todate As String = ""


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
                                                       CType(strappname, String), "AccountsModule\ExchSearch.aspx", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy, GridCol.cancel, GridCol.undocancel)

                End If
                pnlSearch.Visible = False
                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)

                'txtFromDate.Text = ""
                'txtTodate.Text = ""

                'Record list will be according to the Changing the year  
                If Not (Session("changeyear") Is Nothing) Then
                    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                    If Session("changeyear") = Year(Now).ToString Then
                        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                    Else
                        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                    End If

                    txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                    txtTodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

                Else
                    txtFromDate.Text = ""
                    txtTodate.Text = ""
                End If



                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlType, "acc_type_name", "acc_type_name", "select acc_type_name,acc_type_mode   from  acc_type_master where acc_type_mode<>'G' order by acc_type_name", True)
                FillGrid("tran_id", "DESC")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExchSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ExchangeWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub
#End Region
#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim frmdate As String = ""
        Dim todate As String = ""

        'Record list will be according to the Changing the year  
        If Not (Session("changeyear") Is Nothing) Then
            frmdate = CDate(Session("changeyear") + "/01" + "/01")

            If Session("changeyear") = Year(Now).ToString Then
                todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
            Else
                todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
            End If

            txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
            txtTodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

        Else
            txtFromDate.Text = ""
            txtTodate.Text = ""
        End If



        'Record list will be according to the Changing the year  
        If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
            Exit Sub
        End If

        If Session("changeyear") <> Year(CType(txtTodate.Text, Date)).ToString Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
            Exit Sub
        End If


        FillGrid("tran_id", "DESC")
    End Sub
#End Region
#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtTranId.Text = ""
        ''   txtFromDate.Text = ""
        ''   txtTodate.Text = ""
        ddlStatus.Value = "[Select]"
        FillGrid("tran_id", "DESC")
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"


    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        If e.CommandName <> "Page" Then
            Try
                Dim strpop As String = ""
                Dim actionstr As String
                actionstr = ""
                Dim lblId As Label
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblTranID")
                If e.CommandName = "EditRow" Then
                    ' Session.Add("State", "Edit")
                    ' Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)

                    If Validatecancelled(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot edit...')", True)
                        Return
                    End If


                    If Validateseal(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                        Return
                    End If


                    actionstr = "Edit"
                    strpop = "window.open('Exchange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Exchange','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "DeleteRow" Then
                    'Session.Add("State", "Delete")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)

                    If Validatecancelled(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot delete...')", True)
                        Return
                    End If


                    If Validateseal(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                        Return
                    End If

                    actionstr = "Delete"
                    strpop = "window.open('Exchange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Exchange','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "View" Then
                    'Session.Add("State", "View")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)
                    actionstr = "View"
                    strpop = "window.open('Exchange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Exchange','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "Copy" Then
                    'Session.Add("State", "Copy")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)
                    If Validatecancelled(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cancelled Transaction cannot copy...')", True)
                        Return
                    End If


                    actionstr = "Copy"
                    strpop = "window.open('Exchange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Exchange','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "Cancelrow" Then
                    'Session.Add("State", "Copy")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)

                    If Validatecancelled(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Transaction Already Cancelled...')", True)
                        Return
                    End If
                    If Validateseal(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                        Return
                    End If

                    actionstr = "Cancel"
                    strpop = "window.open('Exchange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Exchange','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ElseIf e.CommandName = "UndoCancel" Then
                    'Session.Add("State", "Copy")
                    'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                    'Response.Redirect("Journal.aspx", False)

                    If Validatecancelled(lblId.Text) = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Transaction Not Cancelled...')", True)
                        Return
                    End If
                    If Validateseal(lblId.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                        Return
                    End If

                    actionstr = "UndoCancel"
                    strpop = "window.open('Exchange.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Exchange','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                End If


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExchSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

    Public Function Validatecancelled(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  exchange_master where tran_id='" + tranid + "' ")
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
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function


    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  exchange_master where tran_id='" + tranid + "' ")
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
                strSqlQry = "select  tran_id as	[Document No], tran_type as	[Doc Type]," & _
                             " case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as [Status] ," & _
                            "   convert(varchar(10),journal_date,103) as [Journal Date]," & _
                            " convert(varchar(10),journal_tran_date,103) as	[Posted Date], journal_narration as	[Narration],adddate   as	[Date Created], adduser as	[User Created]," & _
                            " moddate as	[Date Modified], moduser	as [User Modified]  from    exchange_master"
                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY tran_id "
                Else
                    strSqlQry = strSqlQry & " ORDER BY tran_id "
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "Journal")
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExchSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'Session.Add("CurrencyCode", txtgroupid.Text.Trim)
            'Session.Add("CurrencyName", txtmealname.Text.Trim)
            'Response.Redirect("rptCurrencies.aspx", False)
            Dim strfromdate, strtodate, poststate As String
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtTodate.Text, Date), "yyyy/MM/dd"), 1, 10)
            poststate = IIf(ddlStatus.Value = "[Select]", "", ddlStatus.Value)
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Journal")
            'Session.Add("BackPageName", "JournalSearch.aspx")
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Exchange&BackPageName=ExchSearch.aspx&JVTranId=" & txtTranId.Text.Trim & "&Fromdate=" & strfromdate & "&Todate=" & strtodate & "&poststate=" & poststate & "','RepJV','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception
            objUtils.WritErrorLog("ExchangeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
End Class

'If rbtnadsearch.Checked = True Then
'    'If ddlType.Value <> "[Select]" Then
'    '    If Trim(strWhereCond) = "" Then
'    '        strWhereCond = " upper(journal_master.tran_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
'    '    Else
'    '        strWhereCond = strWhereCond & " AND upper(journal_master.tran_type) = '" & Trim(CType(ddlType.Value, String)) & "'"
'    '    End If
'    'End If

'    If txtFromDate.Text <> "" And txtTodate.Text <> "" Then
'        If Trim(strWhereCond) = "" Then
'            strWhereCond = " (journal_master.journal_date between '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text)) & "' and '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(txtTodate.Text)) & "') "
'        Else
'            strWhereCond = strWhereCond & " AND (journal_master.journal_date between '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text)) & "' and '" & Trim(objDateTime.ConvertDateromTextBoxToDatabase(txtTodate.Text)) & "') "
'        End If
'    End If
'End If