
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ExcursionModule_ExcursionReleasePeriodSearch
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
        StopID = 0
        Supplier = 1
        SupplierAgent = 2
        Market = 3
        RoomTypeCode = 4
        RoomTypeName = 5
        AllotmentType = 6
        FromDate = 7
        ToDate = 8
        DateCreated = 9
        UserCreated = 10
        'DateModified = 11
        'UserModified = 12
        View = 11
    End Enum
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

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

                txtconnection.Value = Session("dbconnectionName")

                SetFocus(txtAllotmentID)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                         CType(strappname, String), "ExcursionModule\ExcursionReleasePeriodSearch.aspx?appid=11", btnAddNew, btnExportToExcel, _
                    '               btnPrint, gv_SearchResult, 0, 0, GridCol.View)
                End If


                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpCode, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpName, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCD, "plgrpcode", "plgrpname", "select distinct plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True, ddlMarketCD.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketNM, "plgrpname", "plgrpcode", "select distinct plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True, ddlMarketNM.Value)

                Session.Add("strsortExpression", "stopsalemain_header.mstopid")
                Session.Add("strsortdirection", SortDirection.Ascending)
                'btnAddNew.Visible = True
                'btnExportToExcel.Visible = True
                'btnPrint.Visible = False

                Panel1.Visible = False
                dpFromdate.txtDate.Visible = True
                dpToDate.txtDate.Visible = True

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
                objUtils.WritErrorLog("ExcursionReleasePeriodSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = True Then
                If rbtnadsearch.Checked = True Then
                    If ddlExTypeCode.Value <> "[Select]" Then
                        'strSqlQry = "select distinct partyallot.plgrpcode,plgrpmast.plgrpname from partyallot inner join plgrpmast on partyallot.plgrpcode = plgrpmast.plgrpcode where partyallot.partycode='" & Trim(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text) & "' order by partyallot.plgrpcode "
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCD, "plgrpcode", "plgrpname", strSqlQry, True, ddlMarketCD.Value)
                        'strSqlQry = "select distinct plgrpmast.plgrpname,partyallot.plgrpcode from partyallot inner join plgrpmast on partyallot.plgrpcode = plgrpmast.plgrpcode where partyallot.partycode='" & Trim(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text) & "' order by plgrpmast.plgrpname "
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketNM, "plgrpname", "plgrpcode", strSqlQry, True, ddlMarketNM.Value)

                        'strSqlQry = "  select  rmtypmast.rmtypcode ,rmtypmast.rmtypname from rmtypmast,partyrmtyp where partyrmtyp.inactive='0' and rmtypmast.active='1'and rmtypmast.rmtypcode=partyrmtyp.rmtypcode  and partycode='" & Trim(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text) & "' order by rmtypmast.rmtypcode"
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeCD, "rmtypcode", "rmtypname", strSqlQry, True, ddlRmtypeCD.Value)
                        'strSqlQry = "  select rmtypmast.rmtypname , rmtypmast.rmtypcode from rmtypmast,partyrmtyp where partyrmtyp.inactive='0' and rmtypmast.active='1'and rmtypmast.rmtypcode=partyrmtyp.rmtypcode  and partycode='" & Trim(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text) & "' order by rmtypmast.rmtypname"
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeNM, "rmtypname", "rmtypcode", strSqlQry, True, ddlRmtypeNM.Value)
                    Else
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCD, "plgrpcode", "plgrpname", "select distinct plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True, ddlMarketCD.Value)
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketNM, "plgrpname", "plgrpcode", "select distinct plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True, ddlMarketNM.Value)
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeCD, "rmtypcode", "rmtypname", "select distinct rmtypcode,rmtypname from rmtypmast where active=1 order by rmtypcode", True, ddlRmtypeCD.Value)
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRmtypeNM, "rmtypname", "rmtypcode", "select distinct rmtypname,rmtypcode from rmtypmast where active=1 order by rmtypname", True, ddlRmtypeNM.Value)
                    End If
                End If

            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ExcursionStopSalesWindowPostBack") Then
                btnSearch_Click(sender, e)
            End If





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionReleasePeriodSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region
#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Dim objDateTime As New clsDateTime
        strWhereCond = ""
        If txtAllotmentID.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(allotid) LIKE '" & Trim(txtAllotmentID.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(allotid) LIKE '" & Trim(txtAllotmentID.Value.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlExGrpCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (othgrpcode) = '" & Trim(CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (othgrpcode) = '" & Trim(CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If ddlExTypeCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (othtypcode) = '" & Trim(CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (othtypcode) = '" & Trim(CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlMarketCD.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (plgrpcode) LIKE '%" & Trim(CType(ddlMarketCD.Items(ddlMarketCD.SelectedIndex).Text, String)) & "%'"
            Else
                strWhereCond = strWhereCond & " AND (plgrpcode) LIKE '%" & Trim(CType(ddlMarketCD.Items(ddlMarketCD.SelectedIndex).Text, String)) & "%'"
            End If
        End If


        If dpFromdate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "((convert(varchar(10),frmdatec,111) between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),todatec,111)  between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),frmdatec,111) < convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),todatec,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),frmdatec,111) between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),todatec,111)  between convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),frmdatec,111) < convert(varchar(10), '" & Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),todatec,111) > convert(varchar(10), '" & Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If

        If DDLstatus.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " approve = 'No'"
            Else

                strWhereCond = strWhereCond & " and approve = 'No'"

            End If
        ElseIf DDLstatus.SelectedIndex = 2 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " approve = 'Yes'"

            Else
                strWhereCond = strWhereCond & " and approve = 'Yes'"

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

            'strSqlQry = "select a.allotid,b.othgrpname,c.othtypname,b.othgrpcode,c.othtypcode," & _
            '            "dbo.fn_get_excur_release_marketstring(a.allotid) plgrpcode," & _
            '            "Convert(Varchar(10), convert(datetime,a.frmdate),103) as frmdate," & _
            '            "Convert(Varchar(10), convert(datetime,a.todate),103) as todate," & _
            '            "a.adddate,a.adduser,case approve when 1 then 'Yes' else 'No' end approve from excallotmentnew_header a " & _
            '            "inner join othgrpmast b on a.othgrpcode=b.othgrpcode " & _
            '            "inner join othtypmast c on a.othtypcode=c.othtypcode"

            strSqlQry = "select * from view_releasePeriod"


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
            objUtils.WritErrorLog("ExcursionStopSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session("State") = "New"
        'Response.Redirect("MainAllotStopSales.aspx")
        Dim strpop As String = ""
        'strpop = "window.open('ExcursionReleasePeriod.aspx?State=New','MainAllotmentStopSales','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('ExcursionReleasePeriod.aspx?State=New','MainAllotmentStopSales');"
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
            If e.CommandName = "Page" Then Exit Sub

            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblmstopid")

            If e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("MainAllotStopSales.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('ExcursionReleasePeriod.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','MainAllotmentStopSales','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ExcursionReleasePeriod.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','MainAllotmentStopSales');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Editrow" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("MainAllotStopSales.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('ExcursionReleasePeriod.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','MainAllotmentStopSales','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ExcursionReleasePeriod.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','MainAllotmentStopSales');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("MainAllotStopSales.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('ExcursionReleasePeriod.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','MainAllotmentStopSales','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ExcursionReleasePeriod.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','MainAllotmentStopSales');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
           
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionReleasePeriodSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                'strSqlQry = "select a.allotid as [Allot ID],b.othgrpname as [Excursion Group Name],c.othtypname as [Excursion Type Name],b.othgrpcode as [Excursion Group Code],c.othtypcode as [Excursion Type Code]," & _
                '                      "dbo.fn_get_excur_release_marketstring(a.allotid) as [Market]," & _
                '                      "Convert(Varchar(10), convert(datetime,a.frmdate),103)  as [From Date]," & _
                '                      "Convert(Varchar(10), convert(datetime,a.todate),103) as [To Date]," & _
                '                      "a.adddate as [Added Date],a.adduser as [Added User], case approve when 1 then 'Yes' else 'No' end Approve from excallotmentnew_header a " & _
                '                      "inner join othgrpmast b on a.othgrpcode=b.othgrpcode " & _
                '                      "inner join othtypmast c on a.othtypcode=c.othtypcode"

                strSqlQry = "select allotid as [Allot ID],othgrpname as [Excursion Group Name],othtypname as [Excursion Type Name],othgrpcode as [Excursion Group Code],othtypcode as [Excursion Type Code]," & _
                                      "plgrpcode as [Market]," & _
                                      "frmdate  as [From Date]," & _
                                      "todate as [To Date]," & _
                                      "adddate as [Added Date],adduser as [Added User],approve as [Approve] from view_releasePeriod "
                                 

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "stopsalemain_header")

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
#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Panel1.Visible = False
        ddlMarketCD.Visible = True
        ddlMarketNM.Visible = True

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
        ddlMarketCD.Visible = True
        ddlMarketNM.Visible = True

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
#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtAllotmentID.Value = ""
        ddlExTypeName.Value = "[Select]"
        ddlExTypeCode.Value = "[Select]"
        ddlExGrpName.Value = "[Select]"
        ddlExGrpCode.Value = "[Select]"
        ddlMarketCD.Value = "[Select]"
        ddlMarketNM.Value = "[Select]"
        DDLstatus.SelectedIndex = 0
        dpFromdate.txtDate.Text = ""
        dpToDate.txtDate.Text = ""
        'FillGrid("stopsalemain_header.mstopid")
        ddlOrderBy.SelectedIndex = 0
        FillGridWithOrderByValues()

        SetFocus(txtAllotmentID)
    End Sub
#End Region

    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("allotid", "DESC")
            Case 1
                FillGrid("allotid", "ASC")
            Case 2
                FillGrid("othgrpcode", "ASC")
            Case 3
                FillGrid("othtypcode", "ASC")

        End Select
    End Sub
    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "allotid DESC"
            Case 1
                ExportWithOrderByValues = "allotid ASC"
            Case 2
                ExportWithOrderByValues = "othgrpcode ASC"
            Case 3
                ExportWithOrderByValues = "othtypcode ASC"

        End Select
    End Function

    Protected Sub cmdhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MainStopSalesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub
End Class


