'------------================--------------=======================------------------================
'   Module Name    :    OtherServicesPolicySearch.aspx
'   Developer Name :    D'Silva Azia
'   Date           :   30 july 2008
'------------------------------------------------------------------------------------------------------------

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class OtherServicesPolicySearch
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

        TransactionID = 0
        GroupCode = 1
        GroupName = 2
        MarketCode = 3
        MarketName = 4
        Active = 5
        DateCreated = 6
        UserCreated = 7
        DateModified = 8
        UserModified = 9
        Edit = 10
        View = 11
        Delete = 12
    End Enum
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                SetFocus(txtTransid)
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

                

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "PriceListModule\OtherServicesPolicySearch.aspx", btnAddNew, btnExcel, _
                                                      btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)


                Session.Add("strsortExpression", "tranid")
                Session.Add("strsortdirection", SortDirection.Ascending)
                'FillGrid("tranid")
                FillGridWithOrderByValues()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServicesPolicySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
            btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CurrWindowPostBack") Then
                btnSearch_Click(sender, e)
            End If


        Else
            Try
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True, ddlGroupCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True, ddlGrpName.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True, ddlMarketCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True, ddlMarketName.Value)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServicesPolicySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If


      
    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        '  lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            'strSqlQry = "SELECT othserv_policy.tranid ,othserv_policy.othgrpcode, " & _
            '        "othgrpmast.othgrpname,othserv_policy.plgrpcode , plgrpmast.plgrpname , " & _
            '        "[Active]= case when othserv_policy.active=1 then 'Yes' when othserv_policy.active=0 then 'No' end, " & _
            '        "(Convert(Varchar, Datepart(DD,othserv_policy.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othserv_policy.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othserv_policy.adddate)))adddate, " & _
            '        " othserv_policy.adduser, " & _
            '        "(Convert(Varchar, Datepart(DD,othserv_policy.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othserv_policy.moddate))+ '/'+ Convert(Varchar, Datepart(YY,othserv_policy.moddate)))moddate, " & _
            '        "othserv_policy.moduser  FROM  othserv_policy INNER JOIN  " & _
            '        "othgrpmast ON othserv_policy.othgrpcode = othgrpmast.othgrpcode INNER JOIN plgrpmast ON othserv_policy.plgrpcode = plgrpmast.plgrpcode "

            strSqlQry = "SELECT othserv_policy.tranid ,othserv_policy.othgrpcode, " & _
                    "othgrpmast.othgrpname,othserv_policy.plgrpcode , plgrpmast.plgrpname , " & _
                    "[Active]= case when othserv_policy.active=1 then 'Yes' when othserv_policy.active=0 then 'No' end, " & _
                    "othserv_policy.adddate,othserv_policy.adduser,othserv_policy.moddate,othserv_policy.moduser  FROM  othserv_policy INNER JOIN  " & _
                    " othgrpmast ON othserv_policy.othgrpcode = othgrpmast.othgrpcode INNER JOIN plgrpmast ON othserv_policy.plgrpcode = plgrpmast.plgrpcode "




            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
                lblMsg.Visible = False
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPolicySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""

        If txtTransid.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " othserv_policy.tranid = '" & Trim(txtTransid.Value.Trim) & "'"
            Else
                strWhereCond = strWhereCond & " AND othserv_policy.tranid = '" & Trim(txtTransid.Value.Trim) & "'"
            End If
        End If

        If ddlGroupCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " othserv_policy.othgrpcode = '" & Trim(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND othserv_policy.othgrpcode = '" & Trim(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text) & "'"
            End If
        End If

        If ddlMarketCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " othserv_policy.plgrpcode = '" & Trim(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND othserv_policy.plgrpcode = '" & Trim(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) & "'"
            End If
        End If

        BuildCondition = strWhereCond
    End Function

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("tranid")
        FillGridWithOrderByValues()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        If e.CommandName = "Page" Then Exit Sub
        If e.CommandName = "Sort" Then Exit Sub
        Dim lblID As Label
        Try
            lblID = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltranid")
            If e.CommandName = "Editrow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblID.Text.Trim, String))
                'Response.Redirect("OtherServicesPolicy.aspx", False)


                Dim strpop As String = ""
                strpop = "window.open('OtherServicesPolicy.aspx?State=Edit&RefCode=" + CType(lblID.Text.Trim, String) + "&Type=" + +Session("OthTypeFilter") + "','OtherServicesPolicy','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblID.Text.Trim, String))
                'Response.Redirect("OtherServicesPolicy.aspx", False)

                Dim strpop As String = ""
                strpop = "window.open('OtherServicesPolicy.aspx?State=View&RefCode=" + CType(lblID.Text.Trim, String) + "','OtherServicesPolicy','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblID.Text.Trim, String))
                'Response.Redirect("OtherServicesPolicy.aspx", False)


                Dim strpop As String = ""
                strpop = "window.open('OtherServicesPolicy.aspx?State=Delete&RefCode=" + CType(lblID.Text.Trim, String) + "','OtherServicesPolicy','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

        Catch ex As Exception
            'objUtils.WritErrorLog("OtherServicesPolicySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing

    End Sub

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

#Region " Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                strSqlQry = "SELECT othserv_policy.tranid AS [Transaction ID],othserv_policy.othgrpcode AS [Group Code],othgrpmast.othgrpname AS [Group Name],othserv_policy.plgrpcode AS [Market Code], plgrpmast.plgrpname AS [Market Name],case when othserv_policy.active=1 then 'Yes' when othserv_policy.active=0 then 'No' end as [Active] ,othserv_policy.adddate AS [Date Created],othserv_policy.adduser AS [User Created],othserv_policy.moddate as [Date Modified],othserv_policy.moduser as [User Modified] FROM  othserv_policy INNER JOIN othgrpmast ON othserv_policy.othgrpcode = othgrpmast.othgrpcode INNER JOIN plgrpmast ON othserv_policy.plgrpcode = plgrpmast.plgrpcode"
                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "otherservice_policy")
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Session("ColReportParams") = Nothing
            Session.Add("Pageame", "Other Services Policy")
            Session.Add("BackPageName", "OtherServicesPolicySearch.aspx")
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Other Services Policy&BackPageName=OtherServicesPolicySearch.aspx&TranID=" & txtTransid.Value.Trim & "&GrpCode=" & Trim(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text) & "&MktCode=" & Trim(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text) & "','RepOthSerPol','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'If txtTransid.Value <> "" Then
            '    strReportTitle = "Transaction Code: " & txtTransid.Value.Trim
            '    strSelectionFormula = " {othserv_policy.tranid} = '" & UCase(txtTransid.Value.Trim) & "'"
            'End If

            'If ddlGroupCode.Value <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & "  Group : " & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {othserv_policy.othgrpcode} = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "'"
            '    Else
            '        strReportTitle = "Group : " & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Value.Trim
            '        strSelectionFormula = " {othserv_policy.othgrpcode} = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "'"
            '    End If
            'End If

            'If ddlMarketCode.Value <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " Market  : " & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Value
            '        strSelectionFormula = strSelectionFormula & " and {othserv_policy.plgrpcode} = '" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "'"
            '    Else
            '        strReportTitle = "Market: " & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Value
            '        strSelectionFormula = " {othserv_policy.plgrpcode} = '" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "'"
            '    End If
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPolicySearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Protected Sub rbtnSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnSearch.CheckedChanged
        lblMarketCode.Visible = False
        lblMarketName.Visible = False
        ddlMarketCode.Visible = False
        ddlMarketName.Visible = False
    End Sub
#End Region
    Protected Sub rbtnAdvance_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnAdvance.CheckedChanged
        'FillGrid("tranid")
        FillGridWithOrderByValues()
        lblMarketCode.Visible = True
        lblMarketName.Visible = True
        ddlMarketCode.Visible = True
        ddlMarketName.Visible = True
    End Sub
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnAddNew_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("OtherServicesPolicy.aspx", False)


        Dim strpop As String = ""
        strpop = "window.open('OtherServicesPolicy.aspx?State=New','OtherServicesPolicy','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region
#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("tranid")
        FillGridWithOrderByValues()
    End Sub
#End Region
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtTransid.Value = ""
        ddlGroupCode.Value = "[Select]"
        ddlGrpName.Value = "[Select]"
        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"
        'FillGrid("tranid")
        ddlOrderBy.SelectedIndex = 0
        FillGridWithOrderByValues()
    End Sub

    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othserv_policy.tranid", "DESC")
            Case 1
                FillGrid("othserv_policy.tranid", "ASC")
            Case 2
                FillGrid("othserv_policy.othgrpcode", "ASC")
            Case 3
                FillGrid("othgrpmast.othgrpname", "ASC")
            Case 4
                FillGrid("othserv_policy.plgrpcode", "ASC")
            Case 5
                FillGrid("plgrpmast.plgrpname", "ASC")

        End Select
    End Sub

    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "othserv_policy.tranid DESC"
            Case 1
                ExportWithOrderByValues = "othserv_policy.tranid ASC"
            Case 2
                ExportWithOrderByValues = "othserv_policy.othgrpcode ASC"
            Case 3
                ExportWithOrderByValues = "othgrpmast.othgrpname ASC"
            Case 4
                ExportWithOrderByValues = "othserv_policy.plgrpcode ASC"
            Case 5
                ExportWithOrderByValues = "plgrpmast.plgrpname ASC"
        End Select
    End Function


    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesPolicysearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub
End Class

''objUtils.FillDropDownListnew(Session("dbconnectionName"), txtTransid, "tranid", "select tranid from othserv_policy where active=1 order by tranid", True)
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "othgrpcode", "select distinct othgrpcode from othserv_policy where active=1 order by othgrpcode", True)
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlMarket, "plgrpcode", "select plgrpcode from plgrpmast where active=1 order by plgrpcode", True)
'ddlGroup.SelectedValue = "[Select]"
'ddlMarket.SelectedValue = "[Select]"
''lblMarket.Visible = False
''ddlMarket.Visible = False
' ''lblMsg.Visible = False