
'------------================--------------=======================------------------================
'   Module Name    :    FlightMaster.aspx
'   Developer Name :    Amit Survase
'   Date           :    2 July 2008
'   
'
'-----------

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.ArrayList
Imports System.Collections.Generic
#End Region
Partial Class FlightsMasterSearch
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
        FlightNumberTColumn = 0
        tranid = 1
        FlightNumber = 2
        DepartureArrival = 3
        fromdate = 4
        todate = 5
        origin = 6
        destination = 7

        active = 9
        DateCreated = 10
        UserCreated = 11
        DateModified = 12
        UserModified = 13
        Edit = 14
        View = 15
        Delete = 16
        copy = 17
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
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
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

                RowsPerPageCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\FlightsMasterSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.copy)
                End If

                Pnldeparture.Visible = False
                PanelArrival.Visible = False

                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAirline, "airlinecode", "select distinct airlinecode from flightmast where active=1 order by airlinecode", True)
            


                Session.Add("strsortExpression", "flightcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                Session("sDtDynamic") = Nothing

                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic

                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("FlightMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlAirline.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlAirlineName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        End If
        FillGridNew()
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "FlightmastWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub


#End Region

#Region "  Private Function BuildCondition() As String"

    Private Function BuildCondition() As String
        strWhereCond = ""
        If TxtFlightno.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(f.flightcode) LIKE '" & Trim(TxtFlightno.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(f.flightcode) LIKE '" & Trim(TxtFlightno.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlAirlineName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(f.airlinecode) = '" & Trim(ddlAirlineName.Value.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(f.airlinecode) = '" & Trim(ddlAirlineName.Value.Trim.ToUpper) & "'"
            End If
        End If

        'Arrival
        If ddlFlightType.SelectedIndex <> 0 Then
            If ddlFlightType.SelectedIndex = 1 Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " f.type = 1"
                Else
                    strWhereCond = strWhereCond & " AND upper(f.type) = 1"
                End If


                If ddlarvlairports.Value <> "[Select]" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(f.airportbordercode) = '" + ddlarvlairports.Items(ddlarvlairports.SelectedIndex).Text + "'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(f.airportbordercode) = '" + ddlarvlairports.Items(ddlarvlairports.SelectedIndex).Text + "'"
                    End If
                End If

                If ddlCityArrival.Value <> "[Select]" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(f.city) = '" + ddlCityArrival.Value + "'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(f.city) = '" + ddlCityArrival.Value + "'"
                    End If
                End If

                If txtarrvl.Value <> "" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = "  f.airport like '%" + txtarrvl.Value + "%'"
                    Else
                        strWhereCond = strWhereCond & " AND f.airport like '%" + txtarrvl.Value + "%'"
                    End If
                End If


            End If
        End If

        'Departure
        If ddlFlightType.SelectedIndex <> 0 Then
            If ddlFlightType.SelectedIndex = 2 Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(f.type) = 0"
                Else
                    strWhereCond = strWhereCond & " AND upper(f.type) = 0"
                End If


                If ddlAirBorCode.Value <> "[Select]" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(f.airportbordercode) = '" + ddlAirBorCode.Items(ddlAirBorCode.SelectedIndex).Text + "'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(f.airportbordercode) = '" + ddlAirBorCode.Items(ddlAirBorCode.SelectedIndex).Text + "'"
                    End If
                End If

                If ddlCityDeparture.Value <> "[Select]" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(f.city) = '" + ddlCityDeparture.Value + "'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(f.city) = '" + ddlCityDeparture.Value + "'"
                    End If

                End If

                If txtdep.Value <> "" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " f.airport like '%" + txtdep.Value + "%'"
                    Else
                        strWhereCond = strWhereCond & " AND f.airport like '%" + txtdep.Value + "%'"
                    End If
                End If
            End If
        End If

        If DDLstatus.SelectedIndex = 0 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "f.active = f.active"
            End If
        ElseIf DDLstatus.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(f.active,0) = 0"
            End If
        ElseIf DDLstatus.SelectedIndex = 2 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(f.active,0) =1"
            End If
        End If


        If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),d.frmdate,111)  " _
                & "  and convert(varchar(10),d.todate,111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                & "  between convert(varchar(10),d.frmdate,111)  and  convert(varchar(10),d.todate,111))   " _
                & " or (convert(varchar(10),d.frmdate,111) <= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),d.todate,111) >= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ))"
            Else
                strWhereCond = strWhereCond & " and ((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),d.frmdate,111)  " _
                & "  and convert(varchar(10),d.todate,111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                & "  between convert(varchar(10),d.frmdate,111)  and  convert(varchar(10),d.todate,111))   " _
                & " or (convert(varchar(10),d.frmdate,111) >= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),d.todate,111) <= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)))"

            End If
        End If

        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'desc')"

    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "desc")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            'strSqlQry = "SELECT flightcode, Departtime1, arrivetime1, Fromcity, tocity, airlinecode,partymast.partyname as airlinename, " & _
            '            " terminal,case when isnull(type,0)=0 then 'Departure' when isnull(type,0)=1 then 'Arrival' end as  type, " & _
            '            " depflightno, arrivetime2, departtime2, case when isnull(flightmast.active,0)=1 then 'Active' " & _
            '            " when isnull(flightmast.active,0)=0 then 'InActive' end as active, case when isnull(flightmast.showinplist,0)=1 then 'Yes' " & _
            '            " when isnull(flightmast.showinplist,0)=0 then 'No' end as showinplist, flightmast. adddate, flightmast.adduser, " & _
            '            " flightmast.moddate,flightmast. moduser FROM dbo.flightmast left outer join partymast on flightmast.airlinecode=partymast.partycode "
            strSqlQry = " SELECT f.flight_tranid,f.flightcode, case when isnull(f.[type],0)=0 then 'Departure' when isnull(f.[type],0)=1 then 'Arrival' end as  [type], " & _
                        " convert(varchar(10),min(d.frmdate),103) frmdate,convert(varchar(10),MAX(d.todate),103) todate , case when isnull(f.[type],0)=0 then airportbordersmaster.airportbordername when isnull(f.[type],0)=1 then f.airport end as origin," & _
                        " case when isnull(f.[type],0)=0 then f.airport  when isnull(f.[type],0)=1 then airportbordersmaster.airportbordername end as destination,f.airlinecode," & _
                        " partymast.partyname as airlinename, case when isnull(f.active,0)=1 then 'Active' when isnull(f.active,0)=0 then 'InActive' end as active," & _
                        " f.adddate, f.adduser,f.moddate, f.moduser FROM flightmast f left outer join partymast on f.airlinecode=partymast.partycode left join airportbordersmaster on" & _
                        " f.airportbordercode = airportbordersmaster.airportbordercode left join flightmast_dates d  on  f.flight_tranid=d.flight_tranid  group by  f.flight_tranid,f.flightcode,f.type,d.frmdate,d.todate,airportbordersmaster.airportbordername,f.airport,f.airlinecode, partymast.partyname,f.active,f.adddate,f.adduser,f.moddate,f.moduser"

            Dim groupby As String = " group by f.flight_tranid,f.flightcode,f.type,airportbordersmaster.airportbordername,f.airport,f.airlinecode,partymast.partyname,f.active,f.adddate,f.adduser,f.moddate,f.moduser"

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & groupby & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & groupby & " ORDER BY " & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("FlightMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("FlightsMaster.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('FlightsMaster.aspx?State=New','Flightmast','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('FlightsMaster.aspx?State=New');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region


#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("flightmast.flightcode")
        FillGridNew()
    End Sub

#End Region


#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblflight_tranid")




            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("FlightsMaster.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('FlightsMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Flightmast','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('FlightsMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("FlightsMaster.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('FlightsMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Flightmast','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('FlightsMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "CopyRow" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("FlightsMaster.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('FlightsMaster.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Flightmast','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('FlightsMaster.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("FlightsMaster.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('FlightsMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Flightmast','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('FlightsMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FlightsMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region


#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("flightcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("flightcode", "asc")
            Case 1
                FillGrid("type", "asc")
            Case 2
                FillGrid("type", "desc")
            Case 3
                FillGrid("adddate", "desc")
            Case 4
                FillGrid("flight_tranid", "desc")
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


                'strSqlQry = " SELECT     flightcode AS [Flight Number], Departtime1 AS [Departure Time], arrivetime1 AS [Arrival Time], Fromcity AS [From City], tocity AS [To City], " & _
                '               " airlinecode AS Airlines, terminal AS Terminal, type AS [Arrival/Departure], depflightno AS [Dep.Flight No.], arrivetime2 AS [Arrival Time2],  " & _
                '                "departtime2 AS [Departure Time2], when isnull(flightmast.active,0)=0 then 'InActive' end   AS Active, adddate AS [Date Created], adduser AS [User Created], moddate AS [Date Modified], " & _
                '                    "moduser AS [User Modified] FROM dbo.flightmast"


                strSqlQry = "SELECT flightcode [Flight Number] , Departtime1 as  [Departure Time] , arrivetime1  AS [Arrival Time] ," & _
                            "Fromcity AS [From City], tocity AS [To City], airlinecode AS Airlines ,partymast.partyname as [Air Line Name], " & _
                            " terminal  AS Terminal , case when isnull(type,0)=0 then 'Departure' when isnull(type,0)=1 then 'Arrival' end  AS [Arrival/Departure], " & _
                            " depflightno AS [Dep.Flight No.] , arrivetime2 AS [Arrival Time2] , departtime2 AS [Departure Time2] , case when isnull(flightmast.active,0)=1 then 'Active' " & _
                            " when isnull(flightmast.active,0)=0 then 'InActive' end as Active, case when isnull(flightmast.showinplist,0)=1 then 'Yes' " & _
                            " when isnull(flightmast.showinplist,0)=0 then 'No' end as [ShowIn PriceList] , flightmast. adddate  AS [Date Created] " & _
                            " , flightmast.adduser AS [User Created] , " & _
                            " flightmast.moddate AS [Date Modified] ,flightmast. moduser AS [User Modified] FROM dbo.flightmast inner join partymast on flightmast.airlinecode=partymast.partycode "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY flightcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY flightcode"
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(DS, "flightmast")

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
            ' strpop = "window.open('rptReportNew.aspx?Pageame=Flights Master&BackPageName=FlightsMasterSearch.aspx&FlightNo=" & TxtFlightno.Text.Trim & "&AirCode=" & Trim(ddlAirline.Items(ddlAirline.SelectedIndex).Text) & "&detail=" & IIf(RBtnBrief.Checked, "0", "1") & "&active=" & DDLstatus.SelectedIndex & "&flighttype=" & ddlFlightType.SelectedIndex & "&airborcode=" & ddlarvlairportname.Value & "&CityArrival=" & ddlCityArrival.Value & _
            ' "&Orginarr=" & txtarrvl.Value & "&depairborcode=" & ddlAirBorName.Value & "&CityDeparture=" & ddlCityDeparture.Value & "&Orgindep=" & txtdep.Value & "&FromDate=" & txtFromDate.Text & "&ToDate=" & txtToDate.Text & "','FlightMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Flight&BackPageName=FlightsMasterSearch.aspx&FlightNo=" & TxtFlightno.Text.Trim & "&flighttype=" & ddlFlightType.SelectedIndex & "',' flightmast_new.rpt ');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'If TxtFlightno.Text.Trim <> "" Then
            '    strReportTitle = "Flight No. : " & TxtFlightno.Text.Trim
            '    strSelectionFormula = "{flightmast.flightcode} LIKE '" & TxtFlightno.Text.Trim & "*'"
            'End If
            'If TxtFromCity1.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; From City : " & TxtFromCity1.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {flightmast.fromcity} LIKE '" & TxtFromCity1.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "From City : " & TxtFromCity1.Text.Trim
            '        strSelectionFormula = "{flightmast.fromcity} LIKE '" & TxtFromCity1.Text.Trim & "*'"
            '    End If
            'End If


            'If TxtToCity1.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; To City : " & TxtToCity1.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {flightmast.tocity} LIKE '" & TxtToCity1.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "To City : " & TxtToCity1.Text.Trim
            '        strSelectionFormula = "{flightmast.tocity} LIKE '" & TxtToCity1.Text.Trim & "*'"
            '    End If
            'End If



            'If ddlAirlineName.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Airline : " & Trim(ddlAirlineName.Value.Trim)
            '        strSelectionFormula = strSelectionFormula & " and {flightmast.flightcode} = '" & Trim(ddlAirlineName.Value.Trim) & "'"
            '    Else
            '        strReportTitle = "Airline : " & ddlAirline.Value.Trim
            '        strSelectionFormula = "{flightmast.flightcode} = '" & ddlAirline.Value.Trim & "'"
            '    End If
            'End If

            'If TxtDeptTime.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Departure Time : " & TxtDeptTime.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {flightmast.departtime1} LIKE '" & TxtDeptTime.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Departure Time : " & TxtDeptTime.Text.Trim
            '        strSelectionFormula = "{flightmast.departtime1} LIKE '" & TxtDeptTime.Text.Trim & "*'"
            '    End If
            'End If

            'If TxtArrivalTime1.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Arrival Time : " & TxtArrivalTime1.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {flightmast.arrivetime1} LIKE '" & TxtArrivalTime1.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Arrival Time : " & TxtArrivalTime1.Text.Trim
            '        strSelectionFormula = "{flightmast.arrivetime1} LIKE '" & TxtArrivalTime1.Text.Trim & "*'"
            '    End If
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FlightsMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        TxtFlightno.Text = ""
        'TxtFromCity1.Text = ""
        'TxtToCity1.Text = ""
        ddlAirline.Value = "[Select]"
        ddlAirlineName.Value = "[Select]"
        'TxtArrivalTime1.Text = ""
        'TxtDeptTime.Text = ""
        txtFromDate.Text = ""
        txtToDate.Text = ""


        ddlOrderBy.SelectedIndex = 0
        FillGrid("flight_tranid")
    End Sub
#End Region




    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlFlightMaster.Visible = True
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "", True)
        'lblFromCity.Visible = False
        'lblToCity.Visible = False
        ' lblAirline.Visible = False
        'lblArrivalTime.Visible = False
        'lblDeparturetime.Visible = False
        'TxtFromCity1.Visible = False
        'TxtToCity1.Visible = False
        'TxtArrivalTime1.Visible = False
        'TxtDeptTime.Visible = False
        'ddlAirline.Visible = False
        'ddlAirlineName.Visible = False
        'lblAirName.Visible = False


    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SetFocus(TxtFlightno)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "", True)

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + TxtFlightno.ClientID + "');", True)
        'PnlFlightMaster.Visible = True
        ' ''lblFromCity.Visible = True
        ' ''lblToCity.Visible = True
        lblAirName.Visible = True
        ddlAirlineName.Visible = True
        lblAirName.Visible = True
        lblAirline.Visible = True
        'lblArrivalTime.Visible = True
        'lblDeparturetime.Visible = True
        'TxtFromCity1.Visible = True
        'TxtToCity1.Visible = True
        'TxtArrivalTime1.Visible = True
        'TxtDeptTime.Visible = True
        ddlAirline.Visible = True
    End Sub

    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Flight No")
        ddlOrderBy.Items.Add("Arrival Flight")
        ddlOrderBy.Items.Add("Departure Flight")
        ddlOrderBy.Items.Add("Date Created")
        ddlOrderBy.Items.Add("Tranid")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("flightcode", "asc")
            Case 1
                FillGrid("type", "asc")
            Case 2
                FillGrid("type", "desc")
            Case 3
                FillGrid("adddate", "desc")
            Case 4
                FillGrid("flight_tranid", "desc")
        End Select
    End Sub

    Protected Sub cmdhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=FlightsMasterSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlFlightType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFlightType.SelectedIndexChanged
        If ddlFlightType.SelectedIndex = 0 Then
            Pnldeparture.Visible = False
            PanelArrival.Visible = False
        End If
        If ddlFlightType.SelectedIndex = 1 Then
            Pnldeparture.Visible = False
            PanelArrival.Visible = True
        End If
        If ddlFlightType.SelectedIndex = 2 Then
            PanelArrival.Visible = False
            Pnldeparture.Visible = True

        End If

    End Sub


    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FlightMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strTypeValue As String = ""
        Dim strAirportValue As String = ""
        Dim strFlightNoValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1

                    

                    Dim dt31 As New DataTable

                    strSqlQry = "Select airportbordercode from airportbordersmaster where airportbordername='" & dtt.Rows(i)("value").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dt31)



                    If dtt.Rows(i)("Code").ToString = "AIRPORT" Then
                        If strAirportValue <> "" Then

                            strAirportValue = strAirportValue + ",'" + dt31.Rows(i)("airportbordercode").ToString + "'"
                        Else
                            strAirportValue = "'" + dt31.Rows(i)("airportbordercode").ToString + "'"
                        End If
                    End If



                    If dtt.Rows(i)("Code").ToString = "FLIGHTNO" Then
                        If strFlightNoValue <> "" Then
                            strFlightNoValue = strFlightNoValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strFlightNoValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "FLIGHTTYPE" Then

                        If dtt.Rows(i)("value").ToString = "ARRIVAL" Then

                            If strTypeValue <> "" Then

                                strTypeValue = strTypeValue + ",'" + 1 + "'"
                            Else

                                strTypeValue = 1
                            End If
                        Else


                            If strTypeValue <> "" Then

                                strTypeValue = strTypeValue + ",'" + 0 + "'"
                            Else


                                strTypeValue = 0
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

            strBindCondition = BuildConditionNew(strAirportValue, strTypeValue, strFlightNoValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = "SELECT f.flight_tranid,f.flightcode, case when isnull(f.[type],0)=0 then 'Departure' when isnull(f.[type],0)=1 then 'Arrival' end as  [type], " & _
                  " convert(varchar(10),min(d.frmdate),103) frmdate,convert(varchar(10),MAX(d.todate),103) todate , case when isnull(f.[type],0)=0 then airportbordersmaster.airportbordername when isnull(f.[type],0)=1 then f.airport end as origin," & _
                  " case when isnull(f.[type],0)=0 then f.airport  when isnull(f.[type],0)=1 then airportbordersmaster.airportbordername end as destination," & _
                  " partymast.partyname as airlinename, case when isnull(f.active,0)=1 then 'Active' when isnull(f.active,0)=0 then 'InActive' end as active," & _
                  " f.adddate, f.adduser,f.moddate, f.moduser FROM flightmast f left outer join partymast on f.airlinecode=partymast.partycode left join airportbordersmaster on" & _
                  " f.airportbordercode = airportbordersmaster.airportbordercode left join flightmast_dates d  on  f.flight_tranid=d.flight_tranid  group by  f.flight_tranid,f.flightcode,f.type,d.frmdate,d.todate,airportbordersmaster.airportbordername,f.airport,f.airlinecode, partymast.partyname,f.active,f.adddate,f.adduser,f.moddate,f.moduser"
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"

            If strBindCondition <> "" Then
                strSqlQry = "SELECT f.flight_tranid,f.flightcode, case when isnull(f.[type],0)=0 then 'Departure' when isnull(f.[type],0)=1 then 'Arrival' end as  [type], " & _
                 " convert(varchar(10),min(d.frmdate),103) frmdate,convert(varchar(10),MAX(d.todate),103) todate , case when isnull(f.[type],0)=0 then airportbordersmaster.airportbordername when isnull(f.[type],0)=1 then f.airport end as origin," & _
                 " case when isnull(f.[type],0)=0 then f.airport  when isnull(f.[type],0)=1 then airportbordersmaster.airportbordername end as destination," & _
                 " partymast.partyname as airlinename, case when isnull(f.active,0)=1 then 'Active' when isnull(f.active,0)=0 then 'InActive' end as active," & _
                 " f.adddate, f.adduser,f.moddate, f.moduser FROM flightmast f left outer join partymast on f.airlinecode=partymast.partycode left join airportbordersmaster on" & _
                 " f.airportbordercode = airportbordersmaster.airportbordercode left join flightmast_dates d  on  f.flight_tranid=d.flight_tranid where " & strBindCondition & " group by  f.flight_tranid,f.flightcode,f.type,d.frmdate,d.todate,airportbordersmaster.airportbordername,f.airport,f.airlinecode, partymast.partyname,f.active,f.adddate,f.adduser,f.moddate,f.moduser "

                'strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
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
            objUtils.WritErrorLog("BorderairportMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SectorSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Function BuildConditionNew(ByVal strAirportValue As String, ByVal strTypeValue As String, ByVal strFlightNoValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strAirportValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(f.airportbordercode) IN (" & Trim(strAirportValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(f.airportbordercode) IN (" & Trim(strAirportValue.Trim.ToUpper) & ")"
            End If

        End If

        If strTypeValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(f.type) IN (" & Trim(strTypeValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(f.type) IN (" & Trim(strTypeValue.Trim.ToUpper) & ")"
            End If

        End If
        If strFlightNoValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(f.flightcode) IN (" & Trim(strFlightNoValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(f.flightcode) IN (" & Trim(strFlightNoValue.Trim.ToUpper) & ")"
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

                        strWhereCond1 = "upper(f.airport) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(f.airportbordercode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(f.type) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(f.flightcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  upper(f.airport) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(f.airportbordercode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(f.type) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(f.flightcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"

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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),flightmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),flightmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),flightmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),flightmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
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
            objUtils.WritErrorLog("BorderAirportMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "FLIGHTTYPE"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("FLIGHTTYPE", lsProcessCity, "FLIGHTTYPE")
                Case "FLIGHTNO"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("FLIGHTNO", lsProcessCity, "FLIGHTNO")
                Case "AIRPORT"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("AIRPORT", lsProcessCity, "AIRPORT")

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
End Class

