
'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceSellingTypes.aspx
'   Developer Name :    Amit Survase
'   Date           :    29 June 2008
'   
'
'-----------

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class Other_Services_Selling_Types_Search
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
        SellingCodeTCol = 0
        excsellcode = 1
        excsellname = 2
        Curency = 3
        Active = 4
        DateCreated = 5
        UserCreated = 6
        DateModified = 7
        UserModified = 8
        Edit = 9
        View = 10
        Delete = 11
    End Enum
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

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


                'SetFocus(txtSellingCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    
                    Lblselltypes.Text = "Excursion  Types List"
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                CType(strappname, String), "ExcursionModule\ExcursionTypesSearch.aspx?appid=11", btnAddNew, btnExportToExcel, _
                                                btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)



                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname  from othgrpmast where active=1 and othmaingrpcode in ('EXU','SAFARI') order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpname, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname  from othgrpmast where active=1 and othmaingrpcode in ('EXU','SAFARI') order by othgrpcode", True)

                Session.Add("strsortExpression", "othtypcode")
                Session.Add("strsortdirection", SortDirection.Ascending)

                'FillGrid("othsellcode")
                fillorderby()
                FillGrid("othtypmast.othtypcode")
                Dim typ As Type
                typ = GetType(DropDownList)


                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlgpcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlgpname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                'btnPrint.Visible = True

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OtherSerSelltypeWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub


#End Region


#Region "  Private Function BuildCondition() As String"

    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtSellingCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othtypmast.othtypcode) LIKE '" & Trim(txtSellingCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypcode) LIKE '" & Trim(txtSellingCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtSellingName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othtypmast.othtypname) LIKE '" & Trim(txtSellingName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypname) LIKE '" & Trim(txtSellingName.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlgpcode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othgrpmast.othgrpcode) = '" & Trim(CType(ddlgpcode.Items(ddlgpcode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(othgrpmast.othgrpcode) = '" & Trim(CType(ddlgpcode.Items(ddlgpcode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If ddlgpname.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othgrpmast.othgrpname) = '" & Trim(CType(ddlgpname.Items(ddlgpname.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(othgrpmast.othgrpname) = '" & Trim(CType(ddlgpname.Items(ddlgpname.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If ddltktreq.Items(ddltktreq.SelectedIndex).Text <> "All" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " othtypmast.ticketsreqd = '" & IIf(Trim(CType(ddltktreq.Items(ddltktreq.SelectedIndex).Text, String)) = "Yes", "1", "0") & "'"
            Else
                strWhereCond = strWhereCond & " AND othtypmast.ticketsreqd = '" & IIf(Trim(CType(ddltktreq.Items(ddltktreq.SelectedIndex).Text, String)) = "Yes", "1", "0") & "'"
            End If
        End If


        If ddluponreq.Items(ddluponreq.SelectedIndex).Text <> "All" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " othtypmast.uponrequest = '" & IIf(Trim(CType(ddluponreq.Items(ddluponreq.SelectedIndex).Text, String)) = "Yes", "1", "0") & "'"
            Else
                strWhereCond = strWhereCond & " AND othtypmast.uponrequest = '" & IIf(Trim(CType(ddluponreq.Items(ddluponreq.SelectedIndex).Text, String)) = "Yes", "1", "0") & "'"
            End If
        End If


        BuildCondition = strWhereCond
    End Function
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

            strSqlQry = ""


           
            strSqlQry = "select othtypcode,othtypname,othtypmast.othgrpcode,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            " othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "

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
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
       
        Dim strpop As String = ""
        'strpop = "window.open('ExcursionTypes.aspx?Type=trfsell&State=New','ExcursionSellingTypesSearch','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('ExcursionTypes.aspx?Type=trfsell&State=New','ExcursionSellingTypesSearch');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("othsellmast.othsellcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othtypmast.othtypname")
            Case 1
                FillGrid("othtypmast.othtypname")
            Case 2
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

            If e.CommandName = "Editrow" Then
                

                Dim strpop As String = ""
                'strpop = "window.open('ExcursionTypes.aspx?Type=excsell&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ExcursionTypes.aspx?Type=excsell&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                'strpop = "window.open('ExcursionTypes.aspx?Type=excsell&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ExcursionTypes.aspx?Type=excsell&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                Dim strpop As String = ""
                'strpop = "window.open('ExcursionTypes.aspx?Type=excsell&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ExcursionTypes.aspx?Type=excsell&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("othsellcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othtypmast.othtypname")
            Case 1
                FillGrid("othtypmast.othtypcode")
            Case 2
                FillGrid("othgrpmast.othgrpcode")

        End Select

    End Sub
#End Region


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
        Try
            If gv_SearchResult.Rows.Count <> 0 Then

               
                strSqlQry = "select othtypcode [Code],othtypname [Name],othtypmast.othgrpcode [Group code],(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as [Tickets Required],(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as [Upon Request],othtypmast.daysofweek [Days of Week],othtypmast.adddate as [Add Date],othtypmast.adduser as [Add User],othtypmast.moddate [Modified Date],othtypmast.moduser as [Modified User] from othtypmast inner join " & _
         " othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "



                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY othtypcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY othtypcode"
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(DS, "othtypmast")

                objUtils.ExportToExcel(DS, Response)

                clsDBConnect.dbConnectionClose(SqlConn)
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
            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=ExcursionTypesSearch&BackPageName=ExcursionTypesSearch.aspx&othtypcode=" & txtSellingCode.Text.Trim & "&othtypname=" & txtSellingName.Text.Trim & "&othgrpcode=" & Trim(ddlgpcode.Items(ddlgpcode.SelectedIndex).Text) & "&othgrpname=" & Trim(ddlgpname.Items(ddlgpname.SelectedIndex).Text) & "&ticketsreqd=" & Trim(ddltktreq.Items(ddltktreq.SelectedIndex).Text) & "&uponrequest=" & Trim(ddluponreq.Items(ddluponreq.SelectedIndex).Text) & "','OthSellType','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=ExcursionTypesSearch&BackPageName=ExcursionTypesSearch.aspx&othtypcode=" & txtSellingCode.Text.Trim & "&othtypname=" & txtSellingName.Text.Trim & "&othgrpcode=" & Trim(ddlgpcode.Items(ddlgpcode.SelectedIndex).Text) & "&othgrpname=" & Trim(ddlgpname.Items(ddlgpname.SelectedIndex).Text) & "&ticketsreqd=" & Trim(ddltktreq.Items(ddltktreq.SelectedIndex).Text) & "&uponrequest=" & Trim(ddluponreq.Items(ddluponreq.SelectedIndex).Text) & "','OthSellType');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
           ddlgpcode.Visible = False
        ddlgpname.Visible = False
        lblgpcode.Visible = False
        lblgpname.Visible = False
        ddltktreq.Visible = False
        ddluponreq.Visible = False
        lbltktreq.Visible = False
        lbluponreq.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    
        ddlgpcode.Visible = True
        ddlgpname.Visible = True
        lblgpcode.Visible = True
        lblgpname.Visible = True
        ddltktreq.Visible = True
        ddluponreq.Visible = True
        lbltktreq.Visible = True
        lbluponreq.Visible = True
    End Sub

    '#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    '    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    '    End Sub
    '#End Region
#Region "Protected Sub btnClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub btnClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtSellingCode.Text = ""
        txtSellingName.Text = ""
        ddlgpcode.Value = "[Select]"
        ddlgpname.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        ddluponreq.SelectedIndex = 0
        ddltktreq.selectedindex = 0
        'FillGrid("othsellcode")
        FillGrid("othtypmast.othtypname")
    End Sub
#End Region

    'Protected Sub ddlCurrName_ServerChange(ByVal sender As Object, ByVal e As System.EventArgs)

    'End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add(" Name")
        ddlOrderBy.Items.Add(" Code")
        ddlOrderBy.Items.Add("Group code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othtypmast.othtypname")
            Case 1
                FillGrid("othtypmast.othtypcode")
            Case 2
                FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesSellingTypesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub gv_SearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_SearchResult.SelectedIndexChanged

    End Sub


End Class
