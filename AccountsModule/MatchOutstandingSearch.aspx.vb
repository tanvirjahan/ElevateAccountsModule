'------------================--------------=======================------------------================
'   Module Name    :    MatchOutstandingSearch.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net
#End Region

Partial Class MatchOutstandingSearch
    Inherits System.Web.UI.Page

#Region "Enum GridCol"
    Enum GridCol
        DocNoCol = 0
        DocNo = 1
        DocType = 2
        status = 3
        FDate = 4
        ACCode = 5
        ACName = 6
        Amount = 7
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 13
        View = 14
        Delete = 15
        Copy = 16
    End Enum
#End Region

#Region "Global Declaration"
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
    Dim reportfilter As String
    Dim document As New XLWorkbook
#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '  Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim appidnew As String = CType(Request.QueryString("appid"), String)

        Dim appid As String = CType(Request.QueryString("appid"), String)
     


        '  Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        '   Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
        strappname = Session("AppName")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        If appid Is Nothing = False Then
            'If appid = "4" Then
            '    strappname = AppName.Value
            'Else
            '    strappname = AppName.Value
            'End If
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
        End If
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")


        ViewState.Add("divcode", divid)
        Page.ClientScript.RegisterHiddenField("vdivcode", ViewState("divcode"))
        If Page.IsPostBack = False Then
            Try

                SetFocus(txtDocNo)

                Dim frmdate As String = ""
                Dim todate As String = ""




                txtconnection.Value = Session("dbconnectionName")
                RowsPerPageCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\MatchOutstandingSearch.aspx?appid=" + appidnew, btnAddNew, btnPrint_new, _
                                                       btnPrint_new, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If

                ddlDocType.SelectedIndex = 1
                ddlDocType.Disabled = True

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



              

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCustomer.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCustomerName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlDocType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
                'btnPrint.Visible = False
                ddlType.Attributes.Add("onchange", "javascript:FillCustDDL('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "','" + CType(lblCustCode.ClientID, String) + "','" + CType(lblCustName.ClientID, String) + "')")
                ddlCustomer.Attributes.Add("onchange", "javascript:FillCodeName('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "')")
                ddlCustomerName.Attributes.Add("onchange", "javascript:FillCodeName('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "')")


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MatchOutstandingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then



                Session.Add("MExpression", "tran_id")
                Session.Add("Mdirection", SortDirection.Ascending)

                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                btnPrint_new.Attributes.Add("onclick", "return FormValidation('')")
                FillGridNew()

            Else

            End If
            FillGridNew()



            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "MatchOutStandingWindowPostBack") Then

                btnSearch_Click(sender, e)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstandingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Private Sub FillDDL()"
    Private Sub FillDDL()
        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomer, "code", "des", "select * from view_account where type = 'G' order by code", True)
        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomerName, "des", "code", "select * from view_account where type = 'G' order by des", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "code", "des", "select distinct  top 10 code, des from view_account where div_code='" & ViewState("divcode") & "' order by code", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "des", "code", "select distinct top 10 code, des from view_account  where div_code='" & ViewState("divcode") & "' order by des", True)
    End Sub
#End Region

#Region "Private Function BuildCondition1() As String"
    Private Function BuildCondition1() As String
        strWhereCond = ""
        If dpFromDate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),tran_date,111) between convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),tran_date,111) < convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),tran_date,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),tran_date,111) between convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),tran_date,111) < convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),tran_date,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If

        BuildCondition1 = strWhereCond
    End Function
#End Region


#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtDocNo.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(tran_id) = '" & Trim(txtDocNo.Value.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(tran_id) = '" & Trim(txtDocNo.Value.Trim.ToUpper) & "'"
            End If
        End If
        If ddlDocType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " tran_type = '" & ddlDocType.Value & "'"
            Else
                strWhereCond = strWhereCond & " AND tran_type = '" & ddlDocType.Value & "'"
            End If
        End If
        If ddlStatus.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(post_state,'')  = '" & ddlStatus.Value & "'"
            Else
                strWhereCond = strWhereCond & " AND isnull(post_state,'')  = '" & ddlStatus.Value & "'"
            End If
        End If


        If ddlCustomer.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(acc_code) = '" & Trim(CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(acc_code) = '" & Trim(CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)) & "'"
            End If
        End If
        If ddlCustomerName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(acc_code) = '" & Trim(CType(ddlCustomerName.Value, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(acc_code) = '" & Trim(CType(ddlCustomerName.Value, String)) & "'"
            End If
        End If

        If dpFromDate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),tran_date,111) between convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),tran_date,111) < convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),tran_date,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),tran_date,111) between convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & " or (convert(varchar(10),tran_date,111) < convert(varchar(10), '" & Format(CType(dpFromDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),tran_date,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If
        If txtFromAmount.Value <> "" And txtToAmount.Value <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (amount between " & Val(txtFromAmount.Value) & " and " & Val(txtToAmount.Value) & ") "
            Else
                strWhereCond = strWhereCond & " AND (amount between " & Val(txtFromAmount.Value) & " and " & Val(txtToAmount.Value) & ") "
            End If
        End If

        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "DESC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = " SELECT  tran_id,narration ,tran_type , " & _
                         " case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                          " Convert(Varchar(10),tran_date,103) as tran_date, acc_code,  des , amount , " & _
                          " adddate, adduser,  moddate,moduser  " & _
                          " from matchos_master, view_account where matchos_master.div_id=view_account.div_code and matchos_master.div_id='" & ViewState("divcode") & "' and  matchos_master.acc_code = view_account.code and matchos_master.acc_type = view_account.type "

            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder

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
            objUtils.WritErrorLog("MatchOutstandingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("MatchOutstanding.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('MatchOutstanding.aspx?State=New','MatchOutstanding','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('MatchOutstanding.aspx?State=New&divid=" & ViewState("divcode") & "','MatchOutstanding');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("tran_id", "DESC")
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        If e.CommandName = "Page" Then Exit Sub
        If e.CommandName = "Sort" Then Exit Sub
        Try
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblCode")
            Dim lblTranType As Label = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblTranType")

            Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),matchos_date,111) from matchos_master  where div_id='" & ViewState("divcode") & "' and tran_id='" + lblId.Text + "'")

            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master where div_code='" & ViewState("divcode") & "'")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("MatchOutstanding.aspx", False)

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
                'strpop = "window.open('MatchOutstanding.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','MatchOutstanding','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('MatchOutstanding.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "','MatchOutstanding');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("MatchOutstanding.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('MatchOutstanding.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','MatchOutstanding','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('MatchOutstanding.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "','MatchOutstanding');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("MatchOutstanding.aspx", False)

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
                'strpop = "window.open('MatchOutstanding.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','MatchOutstanding','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('MatchOutstanding.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "','MatchOutstanding');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("MatchOutstanding.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('MatchOutstanding.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','MatchOutstanding','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('MatchOutstanding.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "','MatchOutstanding');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "ViewLog" Then
                Dim actionstr As String
                Dim strpop As String = ""
                actionstr = "ViewLog"
                'strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Journal','width=500,height=300 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ViewLog.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&divid=" & ViewState("divcode") & "&trantype=" + CType(lblTranType.Text.Trim, String) + "','MatchOutstanding');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)




            End If
        Catch ex As Exception
            objUtils.WritErrorLog("MatchOutstandingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  matchos_master where div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
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


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Dim frmdate As String = ""
        Dim todate As String = ""


        FillGrid("tran_id", "DESC")
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDocNo.Value = ""
        txtFromAmount.Value = ""
        txtToAmount.Value = ""
        ddlCustomer.Value = "[Select]"
        ddlCustomerName.Value = "[Select]"
        ddlDocType.Value = "[Select]"
        '  dpFromDate.txtDate.Text = ""
        '  dpToDate.txtDate.Text = ""
        ddlStatus.Value = "[Select]"
        FillGrid("tran_id", "DESC")
    End Sub

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlSearch.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnadsearch.CheckedChanged
        pnlSearch.Visible = True
    End Sub

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("MExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("MExpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("Mdirection", objUtils.SwapSortDirection(Session("Mdirection")))
            dataView.Sort = Session("MExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("Mdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

    'Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

    '    Dim DS As New DataSet
    '    Dim DA As SqlDataAdapter
    '    Dim con As SqlConnection
    '    Dim objcon As New clsDBConnect

    '    Try
    '        If gv_SearchResult.Rows.Count <> 0 Then
    '            strSqlQry = " SELECT  tran_id as [Document No],tran_type as [Doc Type], " & _
    '                        " case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status ," & _
    '                        " Convert(Varchar(10),tran_date,103) as [Date], " & _
    '                        " acc_code as [A/C Code], des as [A/C Name], amount as [Amount],narration [NARRATION] " & _
    '                        " (Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created], adduser as [User Created], " & _
    '                        " (Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified],moduser as [User Modified] " & _
    '                        " from matchos_master, view_account where matchos_master.div_id=view_account.div_code and matchos_master.div_id='" & ViewState("divcode") & "' and   matchos_master.acc_code = view_account.code and matchos_master.acc_type = view_account.type "

    '            If Trim(BuildCondition) <> "" Then
    '                strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY tran_id "
    '            Else
    '                strSqlQry = strSqlQry & " ORDER BY tran_id"
    '            End If
    '            con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '            DA = New SqlDataAdapter(strSqlQry, con)
    '            DA.Fill(DS, "MatchOutstanding")

    '            objUtils.ExportToExcel(DS, Response)
    '            con.Close()
    '        Else
    '            objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MatchOutstandingSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit




    End Sub
    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strDocValue As String = ""
        Dim strcusValue As String = ""
        Dim strsupValue As String = ""
        Dim stragentValue As String = ""
        Dim strstatusValue As String = ""
        Dim strnarrationValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""

        Try

            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "MOSNO" Then
                        If strDocValue <> "" Then
                            strDocValue = strDocValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDocValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If


                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then
                        If strnarrationValue <> "" Then
                            strnarrationValue = strnarrationValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strnarrationValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            strBindCondition = BuildConditionNew(strDocValue, strcusValue, strsupValue, stragentValue, strstatusValue, strTextValue, strnarrationValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = " SELECT  tran_id,narration ,tran_type , " & _
                         " case isnull(post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                          " Convert(Varchar(10),tran_date,103) as tran_date, acc_code,  des , amount , " & _
                          " adddate, adduser,  moddate,moduser  " & _
                          " from matchos_master, view_account where matchos_master.div_id=view_account.div_code and matchos_master.div_id='" & ViewState("divcode") & "' and  matchos_master.acc_code = view_account.code and matchos_master.acc_type = view_account.type "

            Dim strorderby As String = Session("MExpression")
            Dim strsortorder As String = "DESC"

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
            objUtils.WritErrorLog("MatchOutstandingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub


    Private Function BuildConditionNew(ByVal strDocValue As String, ByVal strcusValue As String, ByVal strsupValue As String, ByVal stragentValue As String, ByVal strstatusValue As String, ByVal strTextValue As String, ByVal strnarrationValue As String) As String
        strWhereCond = ""
        If strDocValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(matchos_master.tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(matchos_master.tran_id) IN (" & Trim(strDocValue.Trim.ToUpper) & ")"
            End If

        End If

        If strnarrationValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(matchos_master.narration) IN (" & Trim(strnarrationValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(matchos_master.narration) IN (" & Trim(strnarrationValue.Trim.ToUpper) & ")"
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
                strWhereCond = " upper(matchos_master.post_state) IN (" & Trim(strstatusValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(matchos_master.post_state) IN (" & Trim(strstatusValue.Trim.ToUpper) & ")"
            End If
        End If
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

                            strWhereCond1 = "upper(matchos_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(matchos_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_id='" & ViewState("divcode") & "'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(matchos_master.tran_id) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des)) LIKE '%" & Trim(strPOSTED.Trim.ToUpper) & "%'  or upper(matchos_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_id='" & ViewState("divcode") & "'"
                        End If

                    ElseIf strValue = "UNPOSTED" Then
                        strValue2 = "U"

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(matchos_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(matchos_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_id='" & ViewState("divcode") & "'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(matchos_master.tran_id) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strUNPOSTED.Trim.ToUpper) & "%'  or upper(matchos_master.post_state) LIKE '%" & Trim(strValue2.Trim.ToUpper) & "%' and div_id='" & ViewState("divcode") & "'"
                        End If
                    Else

                        If Trim(strWhereCond1) = "" Then

                            strWhereCond1 = "upper(matchos_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(matchos_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' and div_id='" & ViewState("divcode") & "' or upper(matchos_master.narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  upper(matchos_master.tran_id) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(view_account.des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(matchos_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' and  div_id='" & ViewState("divcode") & "' or upper(matchos_master.narration) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),matchos_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),matchos_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),matchos_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),matchos_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "T" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),matchos_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),matchos_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If


        BuildConditionNew = strWhereCond
    End Function

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





    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Public Function getRowpage() As String
        Dim rowpagecs As String
        If RowsPerPageCS.SelectedValue = "20" Then
            rowpagecs = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagecs = RowsPerPageCS.SelectedValue

        End If
        Return rowpagecs
    End Function

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstandingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "NARRATION"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("NARRATION", lsProcessCity, "NARRATION")
                Case "MOSNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("MOSNO", lsProcessCity, "MOSNO")
                Case "CUSTOMER"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessCity, "CUSTOMER")
                Case "SUPPLIER"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIER", lsProcessCity, "SUPPLIER")
                Case "SUPPLIERAGENT"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIERAGENT", lsProcessCity, "SUPPLIERAGENT")
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

    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCS.SelectedIndexChanged
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



    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click



        'End Select
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstandingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

    Private Sub detailedReport()
        Dim FolderPath As String = "..\ExcelTemplates\"
        Dim FileName As String = "accountsTransactionDetailed_template.xlsx"
        Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        Dim RandomCls As New Random()
        Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
        Dim rptcompanyname, basecurrency As String

        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)
        basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

        Dim FileNameNew As String
        FileNameNew = "MOSDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

        document = New XLWorkbook(FilePath)
        Dim ws As IXLWorksheet = document.Worksheet("register")
        ws.Style.Font.FontName = "arial"

        'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
        'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
        'Dim PartyName As String = ""
        'Dim CatName As String = ""
        'Dim SectorCityName As String = ""

        Dim LastLine As Integer
        ws.Cell(1, 1).Value = rptcompanyname






        'create header
        ws.Cell(2, 1).Value = "Match Outstanding Register:Detailed Report"
        ws.Cell(6, 1).Value = "MOSNo"
        ws.Cell(6, 3).Value = "Type"
        ws.Cell(6, 4).Value = "Account"
        ws.Cell(6, 2).Value = "Date"


        ws.Cell(6, 5).Value = "DocType"
        ws.Cell(6, 6).Value = "Doc No"
        ws.Cell(6, 7).Value = "Doc Date"

        ws.Cell(6, 8).Value = "Due Date"

        ws.Cell(6, 9).Value = "Currency"
        ws.Cell(6, 10).Value = "Rate"
        ws.Cell(6, 11).Value = "Amount"
        ws.Cell(6, 12).Value = "Adjusted Amount"
        ws.Cell(6, 13).Value = "Adjusted Debit"
        ws.Cell(6, 14).Value = " Adjusted Credit"
        ws.Cell(6, 15).Value = "Adjusted Debit"
        ws.Cell(6, 15).Value = ws.Cell(6, 15).Value & " (" & basecurrency & ")"
        ws.Cell(6, 16).Value = "Adjusted Credit"
        ws.Cell(6, 16).Value = ws.Cell(6, 16).Value & " (" & basecurrency & ")"
     
        ws.Cell(6, 17).Value = "Narration/ document"

        ws.Column(15).Width = 16.29

        ws.Column(16).Width = 16.29
        ws.Column(17).Width = 57.29
        ws.Cell(6, 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
        ws.Cell(6, 17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

        'ws.Column(16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
        'ws.Column(17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
        Dim sql As String

        sql = FillGridNew_report()
        ws.Cell(4, 1).Value = reportfilter

        Dim myDS As New DataSet

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(sql, SqlConn)
        myDataAdapter.Fill(myDS)



        Dim dt As New DataTable

        Dim dt_row As New DataTable

        dt = myDS.Tables(0)



        'dt_sum = ds.Tables(1)
        LastLine = 7
        Dim total_basedebit, total_basecredit, total As Double

        total_basedebit = 0
        total_basecredit = 0
        'total = Convert.ToDouble(dt.Compute("SUM(amount_base)", String.Empty))

        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1


                dt_row = dt.Clone()
                dt_row.ImportRow(dt.Rows(i))




                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(LastLine, 1).InsertTable(dt_row.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet.Style.Font.FontName = "arial"
                RateSheet.Columns("10:16").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin
                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                RateSheet.Style.Font.Bold = True

                LastLine = LastLine + 1


                Dim dt_detail_ds, dt_master_ds As New DataSet
                Dim dt_detail, dt_master As New DataTable
                Dim sqlst As String
                Dim id As String = dt.Rows(i).Item(0).ToString



                sqlst = "  select acc_code,acc_type,gl_code,post_state,fromdate,todate" & _
                            " from matchos_master where  div_id='" & ViewState("divcode") & "' and tran_id='" & dt.Rows(i).Item(0).ToString & "' and tran_type='MOS' "
                dt_master_ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlst)
                dt_master = dt_master_ds.Tables(0)

                Dim custcode, type, docno, trantype, controlacc, posted, fromdate, todate As String
                Dim strSqlQry As String
                strSqlQry = "exec  sp_getadjust_MatchOutstanding_report"
                custcode = dt_master.Rows(0).Item(0).ToString
                type = dt_master.Rows(0).Item(1).ToString
                docno = id
                trantype = "MOS"
                controlacc = dt_master.Rows(0).Item(2).ToString
                posted = dt_master.Rows(0).Item(3).ToString
                fromdate = dt_master.Rows(0).Item(4).ToString
                todate = dt_master.Rows(0).Item(5).ToString





                If custcode <> "" Then
                    strSqlQry = strSqlQry & " '" & custcode & "',"
                Else
                    strSqlQry = strSqlQry & " '',"
                End If

                strSqlQry = strSqlQry & " '" & type & "', '" & id & "', 1,'MOS',"

                If controlacc <> "" Then
                    strSqlQry = strSqlQry & " '" & controlacc & "',"
                Else
                    strSqlQry = strSqlQry & " '',"
                End If
                If type = "C" Then
                    strSqlQry = strSqlQry & " 'C',"
                Else
                    strSqlQry = strSqlQry & " 'D',"
                End If



                strSqlQry = strSqlQry & "'" & ViewState("divcode") & "'" + ","

                If posted = "P" Then
                    strSqlQry = strSqlQry & " 'P'" + ","
                Else
                    strSqlQry = strSqlQry & " 'U'" + ","
                End If

                If fromdate <> "" Then
                    strSqlQry = strSqlQry & "'" + Format(CType(fromdate, Date), "yyyy/MM/dd") + "'" + ","
                End If

                If todate <> "" Then
                    strSqlQry = strSqlQry & "'" + Format(CType(todate, Date), "yyyy/MM/dd") + "'"
                End If
                dt_detail_ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), strSqlQry)
                dt_detail = dt_detail_ds.Tables(0)


                Dim RateSheet_detail As IXLRange

                RateSheet_detail = ws.Cell(LastLine, 1).InsertTable(dt_detail.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet_detail.Style.Font.FontName = "arial"
                RateSheet_detail.Columns("10:16").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style3 As IXLBorder = RateSheet_detail.Cells.Style.Border
                style3.BottomBorder = XLBorderStyleValues.Thin
                style3.LeftBorder = XLBorderStyleValues.Thin
                RateSheet_detail.Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                total_basedebit = total_basedebit + Convert.ToDouble(dt_detail.Compute("SUM(basedebit)", String.Empty))
                total_basecredit = total_basecredit + Convert.ToDouble(dt_detail.Compute("SUM(basecredit)", String.Empty))

                LastLine = LastLine + dt_detail.Rows.Count
                LastLine = LastLine + 1

            Next




            ws.Cell(LastLine, 14).Value = "Total"
            ws.Cell(LastLine, 14).Style.Font.Bold = True
            ws.Cell(LastLine, 14).Style.Font.FontSize = 14


            ws.Cell(LastLine, 15).Value = total_basedebit
            ws.Cell(LastLine, 15).Style.Font.Bold = True
            ws.Cell(LastLine, 15).Style.Font.FontSize = 14


            ws.Cell(LastLine, 16).Value = total_basecredit
            ws.Cell(LastLine, 16).Style.Font.Bold = True
            ws.Cell(LastLine, 16).Style.Font.FontSize = 14

            'ws.Cell(LastLine, 18).Value = total
            'ws.Cell(LastLine, 18).Style.Font.Bold = True
            'ws.Cell(LastLine, 18).Style.Font.FontSize = 14

          



        End If










        'ws.Protect(RandomNo)
        'ws.Protection.FormatColumns = True
        'ws.Protection.FormatRows = True

        Using MyMemoryStream As New MemoryStream()
            document.SaveAs(MyMemoryStream)
            document.Dispose()
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
            Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.BinaryWrite(MyMemoryStream.GetBuffer())
            MyMemoryStream.WriteTo(Response.OutputStream)
            Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End Using
    End Sub
    Protected Sub btnPrint_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint_new.Click
        Try
            If ddlrpt.SelectedValue = "Detailed" Then
                detailedReport()

            Else



                Dim FolderPath As String = "..\ExcelTemplates\"
                Dim FileName As String = "accountsTransaction_template.xlsx"
                Dim FilePath As String = Server.MapPath(FolderPath + FileName)
                Dim RandomCls As New Random()
                Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
                Dim rptcompanyname, basecurrency As String

                rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)
                basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
                Dim FileNameNew As String

                FileNameNew = "MOSBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"





                document = New XLWorkbook(FilePath)
                Dim ws As IXLWorksheet = document.Worksheet("register")
                ws.Style.Font.FontName = "arial"

                'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
                'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
                'Dim PartyName As String = ""
                'Dim CatName As String = ""
                'Dim SectorCityName As String = ""

                Dim LastLine As Integer
                ws.Cell(1, 1).Value = rptcompanyname


                ws.Column("10").Delete()


                ws.Column("9").Width = 57


                ws.Column("3").Width = 24
                ws.Column("4").Width = 39.5



                ws.Cell(2, 1).Value = "Match Outstanding:Brief Report"


                'create header

                ws.Cell(6, 2).Value = "Date"
                ws.Cell(6, 1).Value = "MOS No"
                ws.Cell(6, 3).Value = "Type"
                ws.Cell(6, 4).Value = "Account"
                ws.Cell(6, 5).Value = "Currency"
                ws.Cell(6, 6).Value = "Rate"

                ws.Cell(6, 7).Value = "Amount"

                ws.Cell(6, 8).Value = "Amount"
                ws.Cell(6, 8).Value = ws.Cell(6, 8).Value & " (" & basecurrency & ")"
                ws.Cell(6, 9).Value = "Narration"


                Dim sql As String

                sql = FillGridNew_report()
                ws.Cell(4, 1).Value = reportfilter

                Dim myDS As New DataSet

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS)



                Dim dt As New DataTable



                dt = myDS.Tables(0)

                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(7, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(7).Clear()
                ws.Rows(7).Delete()
                LastLine = 7 + dt.Rows.Count

                If dt.Rows.Count > 1 Then
                    ws.Range(LastLine, 1, LastLine, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 6).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 6).Style.Font.Bold = True
                    ws.Cell(LastLine, 7).Value = "Total"
                    ws.Cell(LastLine, 7).Style.Font.Bold = True
                    RateSheet.Columns("6:8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                    RateSheet.Columns("6:8").SetDataType(XLCellValues.Number)
                    ws.Cell(LastLine, 8).SetFormulaR1C1("=SUM(h7:h" & LastLine - 1 & ")")
                    ws.Cell(LastLine, 8).Style.Font.FontName = "arial"

                End If



                RateSheet.Style.Font.FontName = "arial"

                'RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin

                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium










                'ws.Protect(RandomNo)
                'ws.Protection.FormatColumns = True
                'ws.Protection.FormatRows = True

                Using MyMemoryStream As New MemoryStream()
                    document.SaveAs(MyMemoryStream)
                    document.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    'Response.BinaryWrite(MyMemoryStream.GetBuffer())
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            End If
        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub



    Private Function FillGridNew_report() As String


        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strDocValue As String = ""
        Dim strcusValue As String = ""
        Dim strsupValue As String = ""
        Dim stragentValue As String = ""
        Dim strstatusValue As String = ""
        Dim strnarrationValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""

        Dim reportfilterMosno, reportfilterText, reportfilternarration, reportfilterrcustomer, reportfilterrsupplier, reportfilterrsupplieragent, reportfilterstatus As String
        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then
            reportfilter = "Transaction From  " & txtFromDate.Text.Trim & " To " & txtToDate.Text
        End If

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "MOSNO" Then
                        If strDocValue <> "" Then
                            strDocValue = strDocValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDocValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfilterMosno = dtt.Rows(i)("Code").ToString + ":" + strDocValue
                        reportfilter = reportfilter & " " & reportfilterMosno
                    End If


                    If dtt.Rows(i)("Code").ToString = "NARRATION" Then
                        If strnarrationValue <> "" Then
                            strnarrationValue = strnarrationValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strnarrationValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfilternarration = dtt.Rows(i)("Code").ToString + ":" + strnarrationValue
                        reportfilter = reportfilter & " " & reportfilternarration
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strcusValue <> "" Then
                            strcusValue = strcusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strcusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfilterrcustomer = dtt.Rows(i)("Code").ToString + ":" + strcusValue
                        reportfilter = reportfilter & " " & reportfilterrcustomer
                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Then
                        If strsupValue <> "" Then
                            strsupValue = strsupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strsupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfilterrsupplier = dtt.Rows(i)("Code").ToString + ":" + strsupValue
                        reportfilter = reportfilter & " " & reportfilterrsupplier
                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIERAGENT" Then
                        If stragentValue <> "" Then
                            stragentValue = stragentValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            stragentValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfilterrsupplieragent = dtt.Rows(i)("Code").ToString + ":" + stragentValue
                        reportfilter = reportfilter & " " & reportfilterrsupplieragent
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

                        reportfilterstatus = dtt.Rows(i)("Code").ToString + ":" + strstatusValue
                        reportfilter = reportfilter & " " & reportfilterstatus
                    End If

                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString + ""
                        Else
                            strTextValue = "" + dtt.Rows(i)("Value").ToString + ""
                        End If

                        reportfilterText = dtt.Rows(i)("Code").ToString + ":" + strTextValue
                        reportfilter = reportfilter & " " & reportfilterText
                    End If


                Next
            End If
            strBindCondition = BuildConditionNew(strDocValue, strcusValue, strsupValue, stragentValue, strstatusValue, strTextValue, strnarrationValue)
            If ddlrpt.SelectedValue = "Detailed" Then
                strSqlQry = " SELECT  tran_id, Convert(Varchar(10),tran_date,103) as tran_date,case acc_type when 'C' then 'Customer' when 'S' then 'Supplier' when 'G' then 'General' when 'A' then 'Supplier Agent' end,des,'','','','',currcode,currency_rate,'','','','','','',narration   " & _
  "  from matchos_master, view_account where matchos_master.div_id=view_account.div_code and matchos_master.div_id='" & ViewState("divcode") & "' and  matchos_master.acc_code = view_account.code and matchos_master.acc_type = view_account.type "
            Else

                strSqlQry = " SELECT  tran_id, Convert(Varchar(10),tran_date,103) as tran_date,case acc_type when 'C' then 'Customer' when 'S' then 'Supplier' when 'G' then 'General' when 'A' then 'Supplier Agent' end,des,currcode,currency_rate,amount,amount*currency_rate,narration   " & _
                        "  from matchos_master, view_account where matchos_master.div_id=view_account.div_code and matchos_master.div_id='" & ViewState("divcode") & "' and  matchos_master.acc_code = view_account.code and matchos_master.acc_type = view_account.type "
            End If
            Dim strorderby As String = Session("MExpression")
            Dim strsortorder As String = "DESC"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            FillGridNew_report = strSqlQry

        Catch ex As Exception
            FillGridNew_report = ""
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesfreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Function
End Class



