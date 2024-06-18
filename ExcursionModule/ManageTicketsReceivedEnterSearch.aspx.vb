

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ExcursionModule_ManageTicketsReceivedEnterSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim ObjDate As New clsDateTime
#End Region

#Region "Enum GridCol"
    Enum GridCol
        ticketidHidden = 0
        ticketid = 1
        datereceived = 2
        othgrpname = 3
        othtypname = 4
        fromticketno = 5
        toticketno = 6
        ticketdate = 7
        DateCreated = 8
        UserCreated = 9
        DateModified = 10
        UserModified = 11
        Edit = 12
        Delete = 13
        View = 14
        Assign = 15
        Transfer = 16
    End Enum
#End Region


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.IsPostBack = False Then
            Try
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim otypecode1, otypecode2 As String

                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If


                SetFocus(txtAllotmentID)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    CType(strappname, String), "ExcursionModule\ManageTicketsReceivedEnterSearch.aspx?appid=11", btnAddNew, btnExportToExcel, _
                    btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, 0, 0, 0, 0, 0, GridCol.Assign, 0, GridCol.Transfer)
                End If


                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpCode, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpName, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)



                NumbersForTextbox(txtFromTicketNo)
                NumbersForTextbox(txtToTicketNo)

                Panel1.Visible = False


                FillGridWithOrderByValues()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlExGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlExGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlExTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlExTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ManageTicketsReceivedEnterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = True Then


            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ExcursionTicketsWindowPostBack") Then
                btnSearch_Click(sender, e)
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Dim objDateTime As New clsDateTime
        strWhereCond = ""
        If txtAllotmentID.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(excursion_tickets_received.ticketid) LIKE '" & Trim(txtAllotmentID.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(excursion_tickets_received.ticketid LIKE '" & Trim(txtAllotmentID.Value.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlExGrpCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (othgrpmast.othgrpcode) = '" & Trim(CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (othgrpmast.othgrpcode) = '" & Trim(CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If ddlExTypeCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (othtypmast.othtypcode) = '" & Trim(CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (othtypmast.othtypcode) = '" & Trim(CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If dpFromdate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),min(excursion_tickets_detail.ticketdate),111) between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),min(excursion_tickets_detail.ticketdate),111)  between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),min(excursion_tickets_detail.ticketdate),111) < convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),min(excursion_tickets_detail.ticketdate),111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),min(excursion_tickets_detail.ticketdate),111) between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),min(excursion_tickets_detail.ticketdate),111)  between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),min(excursion_tickets_detail.ticketdate),111) < convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),min(excursion_tickets_detail.ticketdate),111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If

        If txtFromTicketNo.Text <> "" And txtToTicketNo.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( min(excursion_tickets_detail.fromticketno) between " & CType(txtFromTicketNo.Text.Trim, Integer) & "" _
                & "  and  " & CType(txtToTicketNo.Text.Trim, Integer) & "" _
                & "  or max(excursion_tickets_detail.toticketno)  between " & txtFromTicketNo.Text.Trim & "" _
                & "  and  '" & CType(txtToTicketNo.Text.Trim, Integer) & "'" _
                & " or min(excursion_tickets_detail.fromticketno) < " & CType(txtFromTicketNo.Text.Trim, Integer) & "" _
                & "  and max(excursion_tickets_detail.toticketno) > " & CType(txtToTicketNo.Text.Trim, Integer) & ")"
            Else
                strWhereCond = strWhereCond & " and ( min(excursion_tickets_detail.fromticketno) between " & CType(txtFromTicketNo.Text.Trim, Integer) & "" _
               & "  and  " & CType(txtToTicketNo.Text.Trim, Integer) & "" _
               & "  or max(excursion_tickets_detail.toticketno)  between " & txtFromTicketNo.Text.Trim & "" _
               & "  and  '" & CType(txtToTicketNo.Text.Trim, Integer) & "'" _
               & " or min(excursion_tickets_detail.fromticketno) < " & CType(txtFromTicketNo.Text.Trim, Integer) & "" _
               & "  and max(excursion_tickets_detail.toticketno) > " & CType(txtToTicketNo.Text.Trim, Integer) & ")"

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

#Region "NumbersForTextbox"
    Public Sub NumbersForTextbox(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
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

            strSqlQry = "select excursion_tickets_received.ticketid," & _
                        "max(Convert(Varchar(10),excursion_tickets_received.datereceived,103)) as datereceived," & _
                        "max(othgrpmast.othgrpname)othgrpname,max(othtypmast.othtypname)othtypname," & _
                        "min(excursion_tickets_detail.fromticketno)fromticketno," & _
                        "max(excursion_tickets_detail.toticketno)toticketno," & _
                        "min(excursion_tickets_detail.ticketdate)ticketdate," & _
                         "max(excursion_tickets_received.adddate)adddate,max(excursion_tickets_received.adduser)adduser,max(excursion_tickets_received.moddate)moddate,max(excursion_tickets_received.moduser)moduser " & _
                        "from excursion_tickets_received " & _
                        "inner join othgrpmast on excursion_tickets_received.othgrpcode=othgrpmast.othgrpcode " & _
                        "inner join othtypmast on excursion_tickets_received.othtypcode=othtypmast.othtypcode " & _
                        "inner join excursion_tickets_detail on excursion_tickets_detail.ticketid=excursion_tickets_received.ticketid " & _
                         "group by excursion_tickets_received.ticketid,othgrpmast.othgrpcode,othtypmast.othtypcode "



            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " HAVING " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("ExcursionStopSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('ManageTicketsReceived.aspx?State=New','ManageTicketsReceived','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("stopsalemain_header.mstopid")
        FillGridWithOrderByValues()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"



    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then
                Exit Sub
            End If

            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblticketid")

            If e.CommandName = "EditRow" Then

                If CheckAgentAssigned(lblId.Text) = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All Tickets are Assigned to Agents');", True)
                    Exit Sub
                End If
                Dim strpop As String = ""
                strpop = "window.open('ManageTicketsReceived.aspx?State=EditRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ManageTicketsReceived','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then

                Dim strpop As String = ""
                strpop = "window.open('ManageTicketsReceived.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','ManageTicketsReceived','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            ElseIf e.CommandName = "DeleteRow" Then

                Dim strpop As String = ""
                strpop = "window.open('ManageTicketsReceived.aspx?State=DeleteRow&RefCode=" + CType(lblId.Text.Trim, String) + "','ManageTicketsReceived','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Assign" Then

                Dim strpop As String = ""
                strpop = "window.open('ManageTicketsReceivedAssign.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','ManageTicketsReceivedAssign','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            ElseIf e.CommandName = "Transfer" Then

                Dim strpop As String = ""
                strpop = "window.open('ManageTicketsReceivedTransfer.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','ManageTicketsReceivedTransfer','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If







        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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
                strSqlQry = "select excursion_tickets_received.ticketid as [Ticket ID]," & _
                      "max(Convert(Varchar(10),excursion_tickets_received.datereceived,103)) as [Date Received]," & _
                      "max(othgrpmast.othgrpname)as [Group Name],max(othtypmast.othtypname) as [Type Name]," & _
                      "min(excursion_tickets_detail.fromticketno) as [From Ticket No]," & _
                      "max(excursion_tickets_detail.toticketno) as [To Ticket No]," & _
                      "min(excursion_tickets_detail.ticketdate) as [Ticket Date]," & _
                       "max(excursion_tickets_received.adddate) as [Added Date],max(excursion_tickets_received.adduser) as [Added User],max(excursion_tickets_received.moddate) as [Modified Date],max(excursion_tickets_received.moduser) as [Modified User] " & _
                      "from excursion_tickets_received " & _
                      "inner join othgrpmast on excursion_tickets_received.othgrpcode=othgrpmast.othgrpcode " & _
                      "inner join othtypmast on excursion_tickets_received.othtypcode=othtypmast.othtypcode " & _
                      "inner join excursion_tickets_detail on excursion_tickets_detail.ticketid=excursion_tickets_received.ticketid " & _
                       "group by excursion_tickets_received.ticketid,othgrpmast.othgrpcode,othtypmast.othtypcode "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " HAVING " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "Manage_Recieved")

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


        dpFromdate.txtDate.Visible = True
        dpToDate.txtDate.Visible = True
        ddlExTypeCode.Visible = True
        ddlExTypeName.Visible = True
        dpFromdate.txtDate.Text = ""
        dpToDate.txtDate.Text = ""
        SetFocus(txtAllotmentID)
    End Sub

#End Region

#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Panel1.Visible = True


        dpFromdate.txtDate.Visible = True
        dpToDate.txtDate.Visible = True
        ddlExTypeCode.Visible = True
        ddlExTypeName.Visible = True
        SetFocus(txtAllotmentID)
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

        'FillGrid("stopsalemain_header.mstopid")
        FillGridWithOrderByValues()
        SetFocus(btnSearch)
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtAllotmentID.Value = ""
        ddlExTypeName.Value = "[Select]"
        ddlExTypeCode.Value = "[Select]"
        ddlExGrpName.Value = "[Select]"
        ddlExGrpCode.Value = "[Select]"

        dpFromdate.txtDate.Text = ""
        dpToDate.txtDate.Text = ""
        txtFromTicketNo.Text = ""
        txtToTicketNo.Text = ""
        ddlOrderBy.SelectedIndex = 0
        FillGridWithOrderByValues()
        ddlCustomer.Value = "[Select]"
        accSearch.Value = ""
        SetFocus(txtAllotmentID)
    End Sub
#End Region

    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("excursion_tickets_received.ticketid", "DESC")
            Case 1
                FillGrid("excursion_tickets_received.ticketid", "ASC")
            Case 2
                FillGrid("othgrpmast.othgrpcode", "ASC")
            Case 3
                FillGrid("othtypmast.othtypcode", "ASC")

        End Select
    End Sub

    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "excursion_tickets_received.ticketid DESC"
            Case 1
                ExportWithOrderByValues = "excursion_tickets_received.ticketid ASC"
            Case 2
                ExportWithOrderByValues = "othgrpmast.othgrpcode ASC"
            Case 3
                ExportWithOrderByValues = "othtypmast.othtypcode ASC"

        End Select
    End Function

    Protected Sub cmdhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MainStopSalesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim fromDate As String = ""
            Dim toDate As String = ""

            If dpFromdate.txtDate.Text = "" Then
                fromDate = ""
            Else
                fromDate = ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)
            End If

            If dpToDate.txtDate.Text = "" Then
                toDate = ""
            Else
                toDate = ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text)
            End If

            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=ManageTicketsReceived&BackPageName=ManageTicketsReceivedEnterSearch.aspx&TicketID=" & txtAllotmentID.Value.Trim & "&FromDate=" & fromDate & "&ToDate=" & toDate & "&FromTicketNo=" & txtFromTicketNo.Text.Trim & "&ToTicketNo=" & txtToTicketNo.Text.Trim & "&othgrpcode=" & Trim(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text) & "&othtypcode=" & Trim(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text) & "','RepCountry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception

        End Try
    End Sub

    Private Function CheckAgentAssigned(ByVal TicketID As String) As Boolean
        Try
            Dim MyDs As DataSet
            Dim strQuery As String = ""
            strQuery = "select isnull(assignedto,'')assignedto from excursion_tickets_subdetail where isnull(assignedto,'') =''  and ticketid='" & TicketID.Trim & "'"
            MyDs = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), strQuery)
            If MyDs.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

End Class




