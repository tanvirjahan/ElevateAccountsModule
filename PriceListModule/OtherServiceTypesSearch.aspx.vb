'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceTypesSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    16 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class OtherServiceTypesSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Enum GridCol"
    Enum GridCol
        TypeCodeTCol = 0
        TypeCode = 1
        TypeName = 2
        GroupCode = 3
        GroupName = 4
        DisplayOrder = 5
        MinPax = 6
        InActive = 7
        PrintInConfirmation = 8
        PaxCheckRequired = 9
        PrintRemarks = 10
        AutoCancellationReq = 11
        DateCreated = 12
        UserCreated = 13
        DateModified = 14
        UserModified = 15
        Edit = 16
        View = 17
        Delete = 18
    End Enum
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
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


                SetFocus(txtCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\OtherServiceTypesSearch.aspx", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                Session.Add("strsortExpression", "othtypmast.othtypcode")
                Session.Add("strsortdirection", SortDirection.Ascending)

                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGrpCode, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                fillorderby()
                FillGrid("othtypmast.othtypname")
                charcters(txtCode)
                charcters(txtName)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServiceTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OthtypeWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtCode.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othtypmast.othtypcode) LIKE '" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypcode) LIKE '" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            End If
        End If

        If txtName.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othtypmast.othtypname) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypname) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            End If
        End If
        'If ddlGrpCode.SelectedValue <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " othtypmast.othgrpcode= '" & Trim(ddlGrpCode.SelectedValue) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND  othtypmast.othgrpcode= '" & Trim(ddlGrpCode.SelectedValue) & "'"
        '    End If
        'End If

        If ddlOtherGrpCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othtypmast.othgrpcode)= '" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND  upper(othtypmast.othgrpcode)= '" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If
        If ddlOtherGrpName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othgrpmast.othgrpname)= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND  upper(othgrpmast.othgrpname)= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
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
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        ' txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
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
            ' strSqlQry = "SELECT * FROM othtypmast"

            'strSqlQry = "SELECT  dbo.othtypmast.othtypcode, dbo.othtypmast.othtypname, dbo.othtypmast.othgrpcode, dbo.othgrpmast.othgrpname, dbo.othtypmast.rankorder, " & _
            '  "        dbo.othtypmast.minpax, case dbo.othtypmast.active when 1 then 'Active'  else 'InActive' end Active, case dbo.othtypmast.printconf when 1 then 'Yes' else 'No' end as printconf, case dbo.othtypmast.paxcheckreq when 1 then 'Yes' else 'No' end as paxcheckreq, case dbo.othtypmast.printremarks when 1 then 'Yes' else 'No' end as printremarks, " & _
            ' "case dbo.othtypmast.autocancelreq when 1 then 'Yes' else 'No' end as autocancelreq, dbo.othtypmast.adddate, dbo.othtypmast.adduser, dbo.othtypmast.moddate, dbo.othtypmast.moduser " & _
            ' "FROM         dbo.othgrpmast INNER JOIN dbo.othtypmast ON dbo.othgrpmast.othgrpcode = dbo.othtypmast.othgrpcode"

            strSqlQry = "SELECT  dbo.othtypmast.othtypcode, dbo.othtypmast.othtypname, dbo.othtypmast.othgrpcode, dbo.othgrpmast.othgrpname, dbo.othtypmast.rankorder, " & _
        "        dbo.othtypmast.minpax, case dbo.othtypmast.active when 1 then 'Active'  else 'InActive' end Active, case dbo.othtypmast.printconf when 1 then 'Yes' else 'No' end as printconf, case dbo.othtypmast.paxcheckreq when 1 then 'Yes' else 'No' end as paxcheckreq, case dbo.othtypmast.printremarks when 1 then 'Yes' else 'No' end as printremarks, " & _
       "case dbo.othtypmast.autocancelreq when 1 then 'Yes' else 'No' end as autocancelreq, dbo.othtypmast.adddate, dbo.othtypmast.adduser, dbo.othtypmast.moddate, dbo.othtypmast.moduser " & _
       "FROM         dbo.othgrpmast INNER JOIN dbo.othtypmast ON dbo.othgrpmast.othgrpcode = dbo.othtypmast.othgrpcode"

            If Trim(BuildCondition) <> "" Then
                'strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                strSqlQry = strSqlQry & " WHERE dbo.othgrpmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " Where dbo.othgrpmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "
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
            objUtils.WritErrorLog("OtherServiceTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("OtherServiceTypes.aspx", False)
        Dim strpop As String = ""
        strpop = "window.open('OtherServiceTypes.aspx?State=New','Othtype','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("othtypcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othtypmast.othtypname")
            Case 1
                FillGrid("othtypmast.othtypcode")
            Case 2
                FillGrid("othgrpmast.othgrpname")
            Case 3
                FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblothtypcode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceTypes.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('OtherServiceTypes.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Othtype','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceTypes.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('OtherServiceTypes.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Othtype','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceTypes.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('OtherServiceTypes.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Othtype','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("othtypcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othtypmast.othtypname")
            Case 1
                FillGrid("othtypmast.othtypcode")
            Case 2
                FillGrid("othgrpmast.othgrpname")
            Case 3
                FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtCode.Value = ""
        txtName.Value = ""
        ' ddlGrpCode.SelectedValue = "[Select]"
        ddlOtherGrpCode.Value = "[Select]"
        ddlOtherGrpName.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        FillGrid("othtypmast.othtypname")
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
                'Response.ContentType = "application/vnd.ms-excel"
                'Response.Charset = ""
                'Me.EnableViewState = False

                'Dim tw As New System.IO.StringWriter()
                'Dim hw As New System.Web.UI.HtmlTextWriter(tw)
                'Dim frm As HtmlForm = New HtmlForm()
                'Me.Controls.Add(frm)
                'frm.Controls.Add(gv_SearchResult)
                'frm.RenderControl(hw)
                'Response.Write(tw.ToString())
                'Response.End()
                'Response.Clear()
                strSqlQry = "SELECT  othtypcode as [Type Code],othtypname as [Type Name], othgrpmast.othgrpcode as [Group Code], dbo.othgrpmast.othgrpname as [Group Name], minpax as [Min Pax], " & _
                            "rankorder as [Display Order],[Active]=case when othtypmast.active=1 then 'Active' when othtypmast.active=0 then 'InActive' end,printconf as [Print in Confirmation], paxcheckreq as [Pax Check Required],printremarks as [Print Remark], " & _
                            "autocancelreq as [Auto Cancellation Required],(Convert(Varchar, Datepart(DD,othtypmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othtypmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othtypmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othtypmast.adddate))+ ':' + Convert(Varchar, Datepart(m,dbo.othtypmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,dbo.othtypmast.adddate))) as [Date Created]," & _
                            "othtypmast.moduser as [User Modified],(Convert(Varchar, Datepart(DD,othtypmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othtypmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,dbo.othtypmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othtypmast.moddate))+ ':' + Convert(Varchar, Datepart(m,dbo.othtypmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,dbo.othtypmast.moddate))) as [Date Modified] " & _
                            "FROM othtypmast INNER JOIN  othgrpmast ON othtypmast.othgrpcode = othgrpmast.othgrpcode "


                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY othtypcode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY othtypcode"
                'End If
                If Trim(BuildCondition) <> "" Then
                    'strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                    strSqlQry = strSqlQry & " WHERE dbo.othgrpmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY othtypcode"
                Else
                    strSqlQry = strSqlQry & " Where dbo.othgrpmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "
                    strSqlQry = strSqlQry & " ORDER BY othtypcode "
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "othtypmast")

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
        
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Other service Type")
            'Session.Add("BackPageName", "OtherServiceTypesSearch.aspx")
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Other service Type&BackPageName=OtherServiceTypesSearch.aspx&OthtypeCode=" & txtCode.Value.Trim & "&OthtypeName=" & txtName.Value.Trim & "&OthgrpCode=" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text) & "','RepOthType','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'If txtCode.Value.Trim <> "" Then
            '    strReportTitle = "Other Service Type Code : " & txtCode.Value.Trim
            '    strSelectionFormula = "{othtypmast.othtypcode} LIKE '" & txtCode.Value.Trim & "*'"
            'End If
            'If txtName.Value.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Other Service Type Name : " & txtName.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {othtypmast.othtypname} LIKE '" & txtName.Value.Trim & "*'"
            '    Else
            '        strReportTitle = "Other Service Type Name : " & txtName.Value.Trim
            '        strSelectionFormula = "{othtypmast.othtypname} LIKE '" & txtName.Value.Trim & "*'"
            '    End If
            'End If
            'If ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Group Code : " & ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {othtypmast.othgrpcode} = '" & ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim & "'"
            '    Else
            '        strReportTitle = "Group Code: " & ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim
            '        strSelectionFormula = "{othtypmast.othgrpcode} = '" & ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblgrpcode.Visible = False
        ddlOtherGrpCode.Visible = False
        lblgrpname.Visible = False
        ddlOtherGrpName.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblgrpcode.Visible = True
        ddlOtherGrpCode.Visible = True
        lblgrpname.Visible = True
        ddlOtherGrpName.Visible = True
    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("ServiceType Name")
        ddlOrderBy.Items.Add("ServiceType Code")
        ddlOrderBy.Items.Add("Group Name")
        ddlOrderBy.Items.Add("Group Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub


    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othtypmast.othtypname")
            Case 1
                FillGrid("othtypmast.othtypcode")
            Case 2
                FillGrid("othgrpmast.othgrpname")
            Case 3
                FillGrid("othgrpmast.othgrpcode")
           
        End Select
    End Sub

    
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServiceTypesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
