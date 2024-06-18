'------------================--------------=======================------------------================
'   Module Name    :    CitiesSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    11 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class CitiesSearch
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
        scode = 0
        exid = 1
        drivername = 2
        vehiclecode = 3
        exdate = 4
        expensvalue = 5
        remarks = 6
        Edit = 7
        View = 8
        Delete = 9
    End Enum
#End Region
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
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)

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


                SetFocus(txtexid)


                'If Request.QueryString("Transfer") = "YES" Then
                '    Session("TRANSFERPAGE") = "YES"
                'Else
                '    Session("TRANSFERPAGE") = "NO"
                'End If



                If strappname = "Excursion Module" Then
                    Session("TRANSFERPAGE") = "NO"
                Else
                    Session("TRANSFERPAGE") = "YES"
                End If


                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "ExcursionModule\VehicleExpensesSearch.aspx?appid=" & strappid & "&Transfer=" & Request.QueryString("Transfer"), btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldrivercode, "drivercode", "drivername", "select drivercode,drivername from drivermaster where active=1 order by drivercode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldrivername, "drivername", "drivercode", "select  drivername,drivercode from drivermaster where active=1 order by drivername", True)


                Session.Add("strsortExpression", "exid")
                Session.Add("strsortdirection", SortDirection.Ascending)
                charcters(txtexid)
                'charcters(txtcityname)
                fillorderby()

                FillGrid("exid")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlcName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlcCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlsccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlscname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnPrint.Visible = False
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CityWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If txtexid.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(spersonmast_office.exid) LIKE '" & Trim(txtexid.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(expense_master.exid) LIKE '" & Trim(txtexid.Text.Trim.ToUpper) & "%'"
            End If
        End If



        If ddldrivercode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(expense_master.drivercode) = '" & Trim(ddldrivercode.Items(ddldrivercode.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(expense_master.drivercode)"
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
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
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

            strSqlQry = "SELECT exid,drivername,drivermaster.vehiclecode,exdate,expensevalue,remarks FROM expense_master inner join drivermaster on expense_master.drivercode =drivermaster.drivercode "


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY expense_master." & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Try

            'Session.Add("State", "New")
            'Response.Redirect("Cities.aspx", False)
            Dim strpop As String = ""
            'strpop = "window.open('VehicleExpenses.aspx?State=New','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('VehicleExpenses.aspx?State=New','Cities');"
            'strpop = "window.open('VehicleExpenses.aspx?State=New&mytrfpage=transfer&','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        '        FillGrid("citycode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("exid")
            Case 1
                FillGrid("drivername")
            Case 2
                FillGrid("vehiclecode")
        End Select
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
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Cities.aspx", False)

                Dim strpop As String = ""
                ' strpop = "window.open('VehicleExpenses.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('VehicleExpenses.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Cities.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('VehicleExpenses.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('VehicleExpenses.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Cities.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('VehicleExpenses.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('VehicleExpenses.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Cities');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "printrow" Then
                Dim strpop As String = ""
                Dim strreportfilter As String = ""
                Dim strreportoption As String = ""
                Dim strreqid As String = ""
                Dim strreportitle As String = ""

                'If txtexpid.Text <> "" Then
                '    strreqid = txtexpid.Text
                'Else
                '    MsgBox("Expense Id is Blank")
                '    Exit Sub
                'End If
                strreportitle = "Vehicle Expense Report"
                '    strpop = "window.open('rptVehicleExpenseReport.aspx?Pageame=vehexpense&BackPageName=rptvehicleexpensereport.aspx " _
                '& "&requestid=" & CType(lblId.Text.Trim, String) _
                '& "&repfilter=" & strreportfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strreportitle & "','RepNewClient','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

                strpop = "window.open('rptVehicleExpenseReport.aspx?Pageame=vehexpense&BackPageName=rptvehicleexpensereport.aspx " _
           & "&requestid=" & CType(lblId.Text.Trim, String) _
           & "&repfilter=" & strreportfilter & "&reportoption=" & strreportoption & "&reporttitle=" & strreportitle & "','RepNewClient');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VehicleExpenseSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("exid")
            Case 1
                FillGrid("drivername")
            Case 2
                FillGrid("vehiclecode")
        End Select
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtexid.Text = ""
        txtvehcode.Text = ""
        ddldrivercode.Value = "[Select]"
        ddldrivername.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        FillGrid("exid")
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

                strSqlQry = "SELECT exid,drivername,vehiclecode,exdate,expensevalue,remarks FROM expense_master inner join drivermaster on expense_master.drivercode =drivermaster.drivercode "
                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY exid "
                Else
                    strSqlQry = strSqlQry & " ORDER BY exid"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "exid")

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


            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=salesexperts&BackPageName=salesexpertsSearch.aspx&code=" & txtexid.Text.Trim & "&name=" & txtvehcode.Text.Trim & "&deptcode=" & Trim(ddldrivercode.Items(ddldrivercode.SelectedIndex).Text) & "&deptname=" & Trim(ddldrivername.Items(ddldrivername.SelectedIndex).Text) & "','RepCountry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '   pnlSearch.Visible = False
        lblctrycode.Visible = False
        ddldrivercode.Visible = False
        ddldrivername.Visible = False
        lblctryname.Visible = False
        lblvehcode.Visible = False
        txtvehcode.Visible = False


    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' pnlSearch.Visible = True
        lblctrycode.Visible = True
        ddldrivercode.Visible = True
        ddldrivername.Visible = True
        lblctryname.Visible = True
        lblvehcode.Visible = True
        txtvehcode.Visible = True

    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("exid")
        ddlOrderBy.Items.Add("drivername")
        ddlOrderBy.Items.Add("vehiclecode")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("exid")
            Case 1
                FillGrid("drivername")
            Case 2
                FillGrid("vehiclecode")
        End Select

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CitiesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class