#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_VisaTypesSearch
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
        othgrpCode = 3
        othgrpName = 4
        active = 5
        adddate = 6
        adduser = 7
        moddate = 8
        moduser = 9
        Edit = 10
        View = 11
        Delete = 12
    End Enum
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session.Add("strsortExpression", "othtypmast.othtypcode")
        Session.Add("strsortdirection", SortDirection.Ascending)
        If Page.IsPostBack = False Then
            Try
                'Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                'Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)

                Dim appid As String = CType(Request.QueryString("appid"), String)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)

                Dim strappid As String = ""
                Dim strappname As String = ""
                If appid Is Nothing = False Then
                    strappid = appid
                End If
                If AppName Is Nothing = False Then

                    strappname = AppName.Value

                End If

                RowsPerPageCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                Session("OthTypeFilter") = Request.Params("Type")
                SetFocus(txtCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\VisaTypesSearch.aspx?appid=" + strappid + "&Type=" + Session("OthTypeFilter"), btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
              

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

                'Dim strqry As String = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1003') order by othgrpcode"
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", strqry, True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", strqry, True)


                charcters(txtCode)
                charcters(txtName)
                TitleLoad(Session("OthTypeFilter"))
                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                '    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("VisaTypesSearch.aspx?Type=" + Session("OthTypeFilter"), Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        FillGridNew()
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CarRentalWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

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
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
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

    Private Sub TitleLoad(ByVal prm_strQueryString As String)
        Dim strOption As String = ""
        Dim strqry As String = ""

        If prm_strQueryString <> "OTH" Then
            strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", prm_strQueryString)
            'GrpCode n name column not need to be visible for all types other than 'OTH'
            gv_SearchResult.Columns(3).Visible = False
            gv_SearchResult.Columns(4).Visible = False

            strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='" + prm_strQueryString + "') order by othgrpcode"
        Else
            strOption = "OTH"
            strqry = "select othgrpcode,othgrpname from othgrpmast inner join othmaingrpmast on othgrpmast.othmaingrpcode =othmaingrpmast.othmaingrpcode and othmaingrpmast.othmaingrpcode not in(Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id in (1001,1002,1003,1021,1022,1027,1028,1105)) order by othgrpcode"
            rbtnadsearch.Visible = True
        End If
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpCode, "othgrpcode", "othgrpname", strqry, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", strqry, True)

        Select Case strOption
            Case "CAR RENTAL"
                Page.Title += "Car Rental Types"
                lblHeading.Text = "Car Rental Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
            Case "VISA"
                Page.Title += "Visa Types"
                lblHeading.Text = "Visa Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
            Case "EXC"
                Page.Title += "Excursion Types"
                lblHeading.Text = "Excursion Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
            Case "MEALS"
                Page.Title += "Restaurant Types"
                lblHeading.Text = "Restaurant Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
            Case "GUIDES"
                Page.Title += "Guide Types"
                lblHeading.Text = "Guide Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
            Case "OTH"
                Page.Title += "Other Service Types"
                lblHeading.Text = "Other Service Types"

            Case "ENTRANCE"
                Page.Title += "Entrance Types"
                lblHeading.Text = "Entrance Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
            Case "JEEPWADI"
                Page.Title += "Jeeb Ride Types"
                lblHeading.Text = "Jeeb Ride Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
            Case "HFEES"
                Page.Title += "Handling Fee Types"
                lblHeading.Text = "Handling Fee  Types"
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
        End Select
    End Sub


#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtCode.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othtypmast.othtypcode) LIKE '%" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypcode) LIKE '%" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            End If
        End If

        If txtName.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othtypmast.othtypname) LIKE '%" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypname) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            End If
        End If
        If ddlGrpCode.Value <> "[Select]" And ddlGrpCode.Value <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " othtypmast.othgrpcode= '" & Trim(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text) & "'"
            Else
                strWhereCond = strWhereCond & " AND  othtypmast.othgrpcode= '" & Trim(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text) & "'"
            End If
        End If


        If ddlGrpName.Value <> "[Select]" And ddlGrpName.Value <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othgrpmast.othgrpname)= '" & Trim(ddlGrpName.Items(ddlGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND  upper(othgrpmast.othgrpname)= '" & Trim(ddlGrpName.Items(ddlGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
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
            strSqlQry = "SELECT  dbo.othtypmast.othtypcode, dbo.othtypmast.othtypname, dbo.othtypmast.othgrpcode, dbo.othgrpmast.othgrpname, dbo.othtypmast.rankorder, " & _
                    "  dbo.othtypmast.minpax, case dbo.othtypmast.active when 1 then 'Active'  else 'InActive' end Active, case dbo.othtypmast.printconf when 1 then 'Yes' else 'No' end as printconf, case dbo.othtypmast.paxcheckreq when 1 then 'Yes' else 'No' end as paxcheckreq, case dbo.othtypmast.printremarks when 1 then 'Yes' else 'No' end as printremarks, " & _
                    "case dbo.othtypmast.autocancelreq when 1 then 'Yes' else 'No' end as autocancelreq, dbo.othtypmast.adddate, dbo.othtypmast.adduser, dbo.othtypmast.moddate, dbo.othtypmast.moduser " & _
                    "FROM  dbo.othgrpmast INNER JOIN dbo.othtypmast ON dbo.othgrpmast.othgrpcode = dbo.othtypmast.othgrpcode INNER JOIN othmaingrpmast  on othgrpmast.othmaingrpcode =othmaingrpmast.othmaingrpcode"
            If (Session("OthTypeFilter") <> "OTH") Then
                strSqlQry = strSqlQry & " WHERE dbo.othgrpmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='" & Session("OthTypeFilter") & "')"
            Else
                strSqlQry = strSqlQry & " WHERE dbo.othmaingrpmast.othmaingrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                 " Where Param_Id in (1001,1002,1003,1021,1022,1027,1028,1105))"
            End If


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " And  " & BuildCondition()
            End If

            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder

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
            objUtils.WritErrorLog("VisaTypesSearch.aspx?Type=" + Session("OthTypeFilter"), Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        strpop = "window.open('VisaTypes.aspx?State=New','CarRental','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

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
            Dim strGrpCode As String = ""
            strGrpCode = gv_SearchResult.Rows(e.CommandArgument).Cells(3).Text
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblothtypcode")

            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""

                strpop = "window.open('VisaTypes.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&GrpCode=" + strGrpCode + "','Visa Types');"

                ' strpop = "window.open('VisaTypes.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&GrpCode=" + strGrpCode + "','CarRental','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                strpop = "window.open('VisaTypes.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&GrpCode=" + strGrpCode + "','Visa Types');"
                'strpop = "window.open('VisaTypes.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&GrpCode=" + strGrpCode + "','CarRental','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                strpop = "window.open('VisaTypes.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&GrpCode=" + strGrpCode + "','Visa Types');"
                'strpop = "window.open('VisaTypes.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&GrpCode=" + strGrpCode + "','CarRental','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VisaTypesSearch.aspx?Type=" + Session("OthTypeFilter"), Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        ddlGrpCode.SelectedIndex = 0
        ddlGrpName.SelectedIndex = 0

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
                            "rankorder as [Display Order],[Active]=case when othtypmast.active=1 then 'Active' when othtypmast.active=0 then 'InActive' end,case dbo.othtypmast.printconf when 1 then 'Yes' else 'No' end as Printconfirmation, case dbo.othtypmast.paxcheckreq when 1 then 'Yes' else 'No' end as Paxcheckrequired, case dbo.othtypmast.printremarks when 1 then 'Yes' else 'No' end as PrintRemarks, " & _
                            "autocancelreq as [Auto Cancellation Required],(Convert(Varchar, Datepart(DD,othtypmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othtypmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othtypmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othtypmast.adddate))+ ':' + Convert(Varchar, Datepart(m,dbo.othtypmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,dbo.othtypmast.adddate))) as [Date Created]," & _
                            "othtypmast.moduser as [User Modified],(Convert(Varchar, Datepart(DD,othtypmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othtypmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,dbo.othtypmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othtypmast.moddate))+ ':' + Convert(Varchar, Datepart(m,dbo.othtypmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,dbo.othtypmast.moddate))) as [Date Modified] " & _
                            "FROM othtypmast INNER JOIN  othgrpmast ON othtypmast.othgrpcode = othgrpmast.othgrpcode INNER JOIN othmaingrpmast  on othgrpmast.othmaingrpcode =othmaingrpmast.othmaingrpcode"


                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY othtypcode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY othtypcode"
                'End If
                If Trim(BuildCondition) <> "" Then
                    'strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                    strSqlQry = strSqlQry & " WHERE dbo.othgrpmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY othtypcode"
                Else
                    strSqlQry = strSqlQry & " WHERE dbo.othmaingrpmast.othmaingrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                 " Where Param_Id in (1001,1002,1003,1021,1022,1027,1028,1105))"
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
            Dim strVal As String = ""
            'Dim strqry As String = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='" & Session("OthTypeFilter") & "') order by othgrpcode"
            'strVal = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strqry)

            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Car Rental Type&BackPageName=VisaTypesSearch.aspx&OthtypeCode=" & txtCode.Value.Trim & "&OthtypeName=" & txtName.Value.Trim & "&OthgrpCode=" & ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text & "&GrpName=" & Trim(ddlGrpName.Items(ddlGrpName.SelectedIndex).Text) & "','RepCarRentalType','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'lblgrpcode.Visible = False
        'ddlOtherGrpCode.Visible = False
        'lblgrpname.Visible = False
        'ddlOtherGrpName.Visible = False
        lblgrpcode.Visible = False
        ddlGrpCode.Visible = False
        ddlGrpName.Visible = False
        lblgrpname.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'lblgrpcode.Visible = True
        'ddlOtherGrpCode.Visible = True
        'lblgrpname.Visible = True
        'ddlOtherGrpName.Visible = True
        If Session("OthTypeFilter") = "OTH" Then
            lblgrpcode.Visible = True
            ddlGrpCode.Visible = True
            ddlGrpName.Visible = True
            lblgrpname.Visible = True
        End If
    End Sub

    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("ServiceType Name")
        ddlOrderBy.Items.Add("ServiceType Code")
        If Session("OthTypeFilter") = "OTH" Then
            ddlOrderBy.Items.Add("Group Name")
            ddlOrderBy.Items.Add("Group Code")
        End If
        ddlOrderBy.SelectedIndex = 0
    End Sub


    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        FillGridNew()

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=VisaTypeSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
   

    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strCarValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "TYPENAME" Then
                        If strCarValue <> "" Then
                            strCarValue = strCarValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCarValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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

            strBindCondition = BuildConditionNew(strCarValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then

                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = "SELECT  dbo.othtypmast.othtypcode, dbo.othtypmast.othtypname,  dbo.othtypmast.othgrpcode, dbo.othgrpmast.othgrpname ,case when isnull(dbo.othtypmast.active,0)=1 then 'Active' when isnull(dbo.othtypmast.active,0)=0 then 'InActive' else 'InActive' end Active ,  dbo.othtypmast.adddate, dbo.othtypmast.adduser, dbo.othtypmast.moddate, dbo.othtypmast.moduser FROM  othtypmast        INNER JOIN   dbo.othgrpmast ON dbo.othtypmast.othgrpcode =dbo.othgrpmast.othgrpcode and othtypmast.othgrpcode='VISA'"
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"

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
            objUtils.WritErrorLog("VisaTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strCarValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCarValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(othtypmast.othtypname) IN (" & Trim(strCarValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(othtypmast.othtypname) IN (" & Trim(strCarValue.Trim.ToUpper) & ")"
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

                        strWhereCond1 = " (upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),othtypmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),othtypmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),othtypmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),othtypmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
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
            objUtils.WritErrorLog("VisaTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



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
            objUtils.WritErrorLog("CarTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "TYPENAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TYPENAME", lsProcessCity, "TYPENAME")


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
