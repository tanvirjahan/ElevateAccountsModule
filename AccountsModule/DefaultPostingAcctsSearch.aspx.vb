Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.IO

Partial Class DefaultPostingAcctsSearch
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Enum GridCol"
    Enum GridCol
        postingId = 0
        ApplicableTo = 1
        countriesExceptAgents = 2
        agents = 3
        addDate = 4
        addUser = 5
        modDate = 6
        modUser = 7
        Edit = 8
        view = 9
    End Enum
#End Region

#Region "Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim strappname As String = ""
        If appid = "4" Then
            strappname = AppName.Value
        Else
            strappname = AppName.Value
        End If

        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
    End Sub
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As String = CType(Request.QueryString("appid"), String)


                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    If ViewState("divcode") = "01" Then
                        strappname = AppName.Value
                    Else
                        strappname = AppName.Value
                    End If
                End If

                txtDivcode.Value = ViewState("divcode")

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\DefaultPostingAcctsSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gvSearch:=gvDefaultPosting, EditColumnNo:=GridCol.Edit, ViewColumnNo:=GridCol.view)

                Page.Title = Page.Title + " " + "Default Posting Accounts"
                Session("DtDefaultPostDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("DtDefaultPostDynamic") = dtDynamic
                Session.Add("strsortExpression", "postingId")
                Session.Add("strsortdirection", SortDirection.Descending)

                HFshowctry_agent.Value = CType(objUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id=5507"), String)
                If HFshowctry_agent.Value = "N" Then
                    gvDefaultPosting.Columns.Item(GridCol.countriesExceptAgents).Visible = False
                    gvDefaultPosting.Columns.Item(GridCol.agents).Visible = False
                End If

                Dim postingId As String = CType(objUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select top 1 isnull(postingId,'') as postingId from InvoicePostingAccounts(nolock)"), String)
                If postingId <> "" Then
                    btnAddNew.Visible = False
                End If

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DefaultPostingAcctsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "DefaultPostingAcctPostBack") Then
            Dim postingId As String = CType(objUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select top 1 isnull(postingId,'') as postingId from InvoicePostingAccounts(nolock)"), String)
            If postingId <> "" Then
                btnAddNew.Visible = False
            End If
            FillGridNew()
        End If
    End Sub
#End Region

#Region "Protected Sub btnvsprocess_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess.Click"
    Protected Sub btnvsprocess_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAcctsSearch.aspx.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub FilterGrid()"
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessInventory As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "SERVICE ID"
                    lsProcessInventory = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SERVICE ID", lsProcessInventory, "SERVICE ID")
                Case "SERVICE NAME"
                    lsProcessInventory = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SERVICE NAME", lsProcessInventory, "SERVICE NAME")
                Case "COUNTRY"
                    lsProcessInventory = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessInventory, "COUNTRY")
                Case "AGENT"
                    lsProcessInventory = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("AGENT", lsProcessInventory, "AGENT")
                Case "CLASSIFICATION"
                    lsProcessInventory = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CLASSIFICATION", lsProcessInventory, "CLASSIFICATION")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("DtDefaultPostDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 
    End Sub
#End Region

#Region " Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean"
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("DtDefaultPostDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("DtDefaultPostDynamic") = dtt
            End If
        End If
        Return True
    End Function
#End Region

#Region "Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click"
    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("DtDefaultPostDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("DtDefaultPostDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("DtDefaultPostDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("DtDefaultPostDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAcctsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnResetSearch_Click(sender As Object, e As System.EventArgs) Handles btnResetSearch.Click"
    Protected Sub btnResetSearch_Click(sender As Object, e As System.EventArgs) Handles btnResetSearch.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        ddlOrder.SelectedIndex = 0
        FillGridNew()
    End Sub
#End Region

#Region "Private Sub FillGridNew()"
    Private Sub FillGridNew()
        Try
            Dim strBindCondition As String = ""
            strBindCondition = BuildConditionNew()
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            Dim myDS As New DataSet
            lblMsg.Visible = False
            If gvDefaultPosting.PageIndex < 0 Then gvDefaultPosting.PageIndex = 0
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "Desc"
            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " Where " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("DefaultPostingAcctSearch", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
            myCommand.Parameters.Add(New SqlParameter("@searchCond", SqlDbType.VarChar, -1)).Value = strSqlQry
            Using myDataAdapter As New SqlDataAdapter(myCommand)
                myDataAdapter.Fill(myDS)
            End Using
            If myDS.Tables(0).Rows.Count > 0 Then
                gvDefaultPosting.DataSource = myDS.Tables(0)
                gvDefaultPosting.PageSize = pagevaluecus
                gvDefaultPosting.DataBind()
            Else
                gvDefaultPosting.PageIndex = 0
                gvDefaultPosting.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAcctsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function BuildConditionNew() As String"
    Private Function BuildConditionNew() As String
        Dim dtt As DataTable
        dtt = Session("DtDefaultPostDynamic")
        Dim strServiceIdValue As String = ""
        Dim strServiceNameValue As String = ""
        Dim strCountryValue As String = ""
        Dim strAgentValue As String = ""
        Dim strClassificationValue As String = ""
        Dim strTextValue As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "SERVICE ID" Then
                        If strServiceIdValue <> "" Then
                            strServiceIdValue = strServiceIdValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strServiceIdValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SERVICE NAME" Then
                        If strServiceNameValue <> "" Then
                            strServiceNameValue = strServiceNameValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strServiceNameValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "AGENT" Then
                        If strAgentValue <> "" Then
                            strAgentValue = strAgentValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strAgentValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CLASSIFICATION" Then
                        If strClassificationValue <> "" Then
                            strClassificationValue = strClassificationValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strClassificationValue = dtt.Rows(i)("Value").ToString
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
            strWhereCond = ""
            If strServiceIdValue.Trim <> "" Then
                strWhereCond = "postingid IN (select postingId from InvoicePostingAccounts where serviceId in (" & Trim(strServiceIdValue.Trim.ToUpper) & ") and divcode='" & ViewState("divcode") & "')"
            End If
            If strServiceNameValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "postingId in (select postingId from InvoicePostingAccounts where ServiceName in (" & Trim(strServiceNameValue.Trim.ToUpper) & ") and divcode='" & ViewState("divcode") & "')"
                Else
                    strWhereCond = strWhereCond & "and postingId in (select postingId from InvoicePostingAccounts where ServiceName in (" & Trim(strServiceNameValue.Trim.ToUpper) & ") and divcode='" & ViewState("divcode") & "')"
                End If
            End If
            If strCountryValue.Trim <> "" Then
                Dim arrCountry() As String = strCountryValue.Split(",")
                Dim ctrysearch As String = ""
                For i = 0 To arrCountry.GetUpperBound(0)
                    If ctrysearch = "" Then
                        ctrysearch = "charindex(" & arrCountry(i) & ",countries)>0"
                    Else
                        ctrysearch = ctrysearch & " or charindex(" & arrCountry(i) & ",countries)>0"
                    End If
                Next
                If strWhereCond = "" Then
                    strWhereCond = "(" & ctrysearch & ")"
                Else
                    strWhereCond = strWhereCond & "and (" & ctrysearch & ")"
                End If
            End If
            If strAgentValue.Trim <> "" Then
                Dim arrAgent() As String = strAgentValue.Split(",")
                Dim AgentSearch As String = ""
                For i = 0 To arrAgent.GetUpperBound(0)
                    If AgentSearch = "" Then
                        AgentSearch = "charindex ((select agentcode from agentmast where agentname =" & arrAgent(i) & "), agentCodes)>0"
                    Else
                        AgentSearch = AgentSearch & " or charindex ((select agentcode from agentmast where agentname =" & arrAgent(i) & "), agentCodes)>0"
                    End If
                Next
                If strWhereCond = "" Then
                    strWhereCond = "(" & AgentSearch & ")"
                Else
                    strWhereCond = strWhereCond & "and (" & AgentSearch & ")"
                End If
            End If
            If strClassificationValue.Trim <> "" Then
                Dim arrClass() As String = strClassificationValue.Split(",")
                Dim classSearch As String = ""
                For i = 0 To arrClass.GetUpperBound(0)
                    If arrClass(i).Trim = "0" Then
                        If classSearch = "" Then
                            classSearch = "postingid not in(select postingId from InvoicePostingAcctClassification)"
                        Else
                            classSearch = classSearch & " or postingid not in(select postingId from InvoicePostingAcctClassification)"
                        End If
                    Else
                        If classSearch = "" Then
                            classSearch = "postingid IN (select distinct postingId from InvoicePostingAccounts where postingClassification=1)"
                        Else
                            classSearch = classSearch & " or postingid IN (select distinct postingId from InvoicePostingAccounts where postingClassification=1)"
                        End If
                    End If
                Next
                If strWhereCond = "" Then
                    strWhereCond = "(" & classSearch & ")"
                Else
                    strWhereCond = strWhereCond & "and (" & classSearch & ")"
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
                            strWhereCond1 = "postingId in (select distinct postingId from InvoicePostingAccounts where ServiceName like '%" & Trim(strValue.Trim.ToUpper) & "%' and divcode='" & ViewState("divcode") & "')  or charindex ((select agentcode from agentmast where agentname like '" & Trim(strValue.Trim.ToUpper) & "'), agentCodes)>0 or charindex ((select ctrycode from ctrymast where ctryname like '" & Trim(strValue.Trim.ToUpper) & "'), ctryCodes)>0  or " &
                            "PostingId like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " or postingId in (select distinct postingId from InvoicePostingAccounts where ServiceName like '%" & Trim(strValue.Trim.ToUpper) & "%' and divcode='" & ViewState("divcode") & "')  or charindex ((select agentcode from agentmast where agentname like '" & Trim(strValue.Trim.ToUpper) & "'), agentCodes)>0 or charindex ((select ctrycode from ctrymast where ctryname like '" & Trim(strValue.Trim.ToUpper) & "'), ctryCodes)>0  or " &
                            "PostingId like '%" & Trim(strValue.Trim.ToUpper) & "%'"
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

                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),adddate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
                    End If
                ElseIf ddlOrder.SelectedValue = "M" Then
                    If Trim(strWhereCond) = "" Then

                        strWhereCond = " (CONVERT(datetime, convert(varchar(10), moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10), moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                    End If
                End If
            End If
            BuildConditionNew = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAcctsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            BuildConditionNew = ""
        End Try
    End Function
#End Region

#Region "Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click"
    Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('DefaultPostingAccts.aspx?State=New&divid=" & ViewState("divcode") & "' ,'DefaultPosting');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
#End Region

#Region "Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged"
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvDefaultPosting_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDefaultPosting.PageIndexChanging"
    Protected Sub gvDefaultPosting_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDefaultPosting.PageIndexChanging
        gvDefaultPosting.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvDefaultPosting_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDefaultPosting.RowCommand"
    Protected Sub gvDefaultPosting_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDefaultPosting.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblPostingId As Label
            lblPostingId = gvDefaultPosting.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblPostingId")
            'lblPostingId.Text = lblPostingId.Text.Replace("<span style = 'background-color:#ffcc99'>", "")
            'lblPostingId.Text = lblPostingId.Text.Replace("</span>", "")
            Dim strpop As String = ""
            If e.CommandName = "View" Then
                strpop = "window.open('DefaultPostingAccts.aspx?&divid=" & ViewState("divcode") & "&State=View&ID=" + CType(lblPostingId.Text.Trim, String) + "','DefaultPosting');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            ElseIf e.CommandName = "EditPosting" Then
                strpop = "window.open('DefaultPostingAccts.aspx?&divid=" & ViewState("divcode") & "&State=Edit&ID=" + CType(lblPostingId.Text.Trim, String) + "','DefaultPosting');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            ElseIf e.CommandName = "Print" Then

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefaultPostingAcctsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvDefaultPosting_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDefaultPosting.Sorting"
    Protected Sub gvDefaultPosting_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDefaultPosting.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColumn()
    End Sub
#End Region

#Region "Public Sub SortGridColumn()"
    Public Sub SortGridColumn()
        Dim DataTable As DataTable
        FillGridNew()
        DataTable = gvDefaultPosting.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gvDefaultPosting.DataSource = dataView
            gvDefaultPosting.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim strBindCondition As String = ""
        Try
            If gvDefaultPosting.Rows.Count > 0 Then
                strBindCondition = BuildConditionNew()
                Dim strorderby As String = Session("strsortexpression")
                Dim strsortorder As String = IIf(Session("strsortdirection") = "0", "Asc", "Desc")
                Dim myDS As New DataSet
                If strBindCondition <> "" Then
                    strSqlQry = strSqlQry & " where " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myCommand = New SqlCommand("DefaultPostingAcctSearch", SqlConn)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
                myCommand.Parameters.Add(New SqlParameter("@searchCond", SqlDbType.VarChar, -1)).Value = strSqlQry
                Using myDataAdapter As New SqlDataAdapter(myCommand)
                    myDataAdapter.Fill(myDS, "DefaultPostingAccount")
                End Using
                Dim dt As DataTable = myDS.Tables(0)
                dt.Columns.Remove("ctryCodes")
                dt.Columns.Remove("agentCodes")
                dt.Columns.Remove("ctryExceptAgentCodes")
                dt.AcceptChanges()
                objUtils.ExportToExcel(myDS, Response)
                clsDBConnect.dbConnectionClose(SqlConn)
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

End Class
