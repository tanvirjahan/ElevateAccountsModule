Imports System.Data
Imports System.Data.SqlClient

Partial Class MealMarkupSearch
    Inherits System.Web.UI.Page
#Region "Enum GridCol"
    Enum GridCol
        FormulaIDCol = 0
        FormulaID = 1
        FormulaName = 2
        CommissionFormula = 3
        Active = 4
        DateCreated = 5
        UserCreated = 6
        DateModified = 7
        UserModified = 8
        Edit = 9
        View = 10
        Delete = 11
        print = 12
        Copy = 13

    End Enum
#End Region
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Markup Formula','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
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
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try

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
                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                hdNewForm.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='5'")

                hdAddinalFields.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='6'")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\MarkupSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, GridCol.print, GridCol.Copy)

                End If
                '' Create a Dynamic datatable ---- Start
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                '--------end
                Session.Add("strsortExpression", "FormulaID")
                Session.Add("strsortdirection", SortDirection.Ascending)
                'fillorderby()
                ' FillTerms()



                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MarkupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnResetSelection.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "MealMarkupWindowPostBack") Then
            FillGridNew()
            ' FillTerms()

        End If
        Page.Title = "Markup Formula Search"
    End Sub
    Private Sub FillTerms()
        Try
            Dim strSqlQry As String
            Dim myDS As New DataSet
            strSqlQry = "select termcode +' = ' + termname as Terms from commissionterms order by rankOrder"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            dlTerms.DataSource = myDS
            dlTerms.DataBind()
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try
    End Sub

    Private Sub FillGridNew()
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strMarkupFormulaValue As String = ""
        Dim strFormulaType As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "MARKUP FORMULA NAME" Then
                        If strMarkupFormulaValue <> "" Then
                            strMarkupFormulaValue = strMarkupFormulaValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strMarkupFormulaValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "FORMULA TYPE" Then
                        If strFormulaType <> "" Then
                            strFormulaType = strFormulaType + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strFormulaType = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            strBindCondition = BuildConditionNew(strMarkupFormulaValue, strTextValue, strFormulaType)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If

            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"
            'If hdProjectName.Value = "ColumbusCommon" Or hdProjectName.Value = "ColumbusElite" Or hdProjectName.Value = "ColumbusVoyage" Then
            '    strSqlQry = "select *, [IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end,(select currmast.currname from currmast where currmast.currcode=New_MarkupSupplement_Header.currcode)currname   from New_MarkupSupplement_Header(noloack) "
            'Else
            '    strSqlQry = "SELECT * from V_GET_MARKUP_FORMULA "
            'End If

            strSqlQry = "select *, [IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end,(select currmast.currname from currmast where currmast.currcode=New_MarkupSupplement_Header.currcode)currname   from New_MarkupSupplement_Header(nolock) "

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("MarkupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strMarkupValue As String, ByVal strTextValue As String, ByVal strFormulaType As String) As String
        strWhereCond = ""
        If strMarkupValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(V_GET_MARKUP_FORMULA.FormulaName) IN (" & Trim(strMarkupValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(V_GET_MARKUP_FORMULA.FormulaName) IN (" & Trim(strMarkupValue.Trim.ToUpper) & ")"
            End If
        End If
        If strFormulaType.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(V_GET_MARKUP_FORMULA.FormulaType) IN (" & Trim(strFormulaType.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(V_GET_MARKUP_FORMULA.FormulaType) IN (" & Trim(strFormulaType.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "(upper(V_GET_MARKUP_FORMULA.FormulaName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or formulaid in (select formulaid from V_GET_MARKUP_FORMULA where upper(V_GET_MARKUP_FORMULA.MarkupFormula) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(IsActive) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'))"
                    Else
                        strWhereCond1 = strWhereCond1 & " or (upper(V_GET_MARKUP_FORMULA.FormulaName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or formulaid in (select formulaid from V_GET_MARKUP_FORMULA where upper(V_GET_MARKUP_FORMULA.MarkupFormula) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(IsActive) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'))"
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessMarkup As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "MARKUP FORMULA NAME"
                    lsProcessMarkup = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("MARKUP FORMULA NAME", lsProcessMarkup, "MARKUP FORMULA NAME")
                Case "FORMULA TYPE"
                    lsProcessMarkup = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("FORMULA TYPE", lsProcessMarkup, "FORMULA TYPE")
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
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")

            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                If hdNewForm.Value.ToString.Trim.ToUpper = "YES".ToUpper Then
                    strpop = "window.open('MarkupFormula1.aspx?State=Edit&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                Else
                    strpop = "window.open('MealMarkupFormula.aspx?State=Edit&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                If hdNewForm.Value.ToString.Trim.ToUpper = "YES".ToUpper Then
                    strpop = "window.open('MarkupFormula1.aspx?State=View&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                Else
                    strpop = "window.open('MealMarkupFormula.aspx?State=View&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                End If


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                If hdNewForm.Value.ToString.Trim.ToUpper = "YES".ToUpper Then
                    strpop = "window.open('MarkupFormula1.aspx?State=Delete&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                Else
                    strpop = "window.open('MealMarkupFormula.aspx?State=Delete&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                End If


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            ElseIf e.CommandName = "CopyRow" Then
                Dim strpop As String = ""
                If hdNewForm.Value.ToString.Trim.ToUpper = "YES".ToUpper Then
                    strpop = "window.open('MarkupFormula1.aspx?State=Copy&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                Else
                    strpop = "window.open('MealMarkupFormula.aspx?State=Copy&FormulaID=" + CType(lblId.Text.Trim, String) + "','MarkupFormula');"
                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblFormulaName As Label = e.Row.FindControl("lblFormulaName")
            Dim lblFormula As Label = e.Row.FindControl("lblFormula")

            Dim lsFormulaName As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsFormulaName = ""

                        If "MARKUP FORMULA NAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsFormulaName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsFormulaName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsFormulaName.Trim <> "" Then
                            lblFormulaName.Text = Regex.Replace(lblFormulaName.Text.Trim, lsFormulaName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), RegexOptions.IgnoreCase)
                            lblFormula.Text = Regex.Replace(lblFormula.Text.Trim, lsFormulaName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If
        End If
    End Sub
#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGridNew()

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

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
    Public Function getRowpage() As String
        Dim rowpagecus As String
        If RowsPerPageCUS.SelectedValue = "20" Then
            rowpagecus = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagecus = RowsPerPageCUS.SelectedValue

        End If
        Return rowpagecus
    End Function

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strCommissionValue As String = ""
        Dim strTextValue As String = ""
        Dim strFormulaTypeValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                If dtt.Rows.Count > 0 Then
                    For i As Integer = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(i)("Code").ToString = "MARKUP FORMULA NAME" Then
                            If strCommissionValue <> "" Then
                                strCommissionValue = strCommissionValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                            Else
                                strCommissionValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                            End If
                        End If
                        If dtt.Rows(i)("Code").ToString = "FORMULA TYPE" Then
                            If strFormulaTypeValue <> "" Then
                                strFormulaTypeValue = strFormulaTypeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                            Else
                                strFormulaTypeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
                strBindCondition = BuildConditionNew(strCommissionValue, strTextValue, strFormulaTypeValue)


                strSqlQry = "select formulaid,formulaname,formuladesc,MarkupFormulaString,FormulaType,IsActive,currname,adddate,adduser,moddate,moduser  from V_GET_MARKUP_FORMULA  "

                If Trim(strBindCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY FormulaID "
                Else
                    strSqlQry = strSqlQry & " ORDER BY FormulaID"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "commissionformula_header")

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

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        If hdNewForm.Value.ToString.Trim.ToUpper = "YES".ToUpper Then
            strpop = "window.open('MarkupFormula1.aspx?State=New','MarkupFormula');"
        Else
            strpop = "window.open('MealMarkupFormula.aspx?State=New','MarkupFormula');"
        End If

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
#End Region

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("ColReportParams") = Nothing
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=MarkupFormula&BackPageName=MarkupSearch.aspx','RepMarkup');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


End Class
