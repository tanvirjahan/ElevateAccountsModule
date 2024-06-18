'------------================--------------=======================------------------================
'   Module Name    :    SectorSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    11 June 2008
'   
'
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SectorSearch
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
        CategoryCodeTCol = 0
        CategoryCode = 1
        AgentName = 2
        'SellCode = 3
        'SellName = 4
        'CreditAction = 5
        'Commission = 6
        Active = 3
        DateCreated = 4
        UserCreated = 5
        DateModified = 6
        UserModified = 7
        Edit = 8
        View = 9
        Delete = 10
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
    ''' <summary>
    ''' lbClose_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
            dllistsearch.DataSource = dtDynamics
            dllistsearch.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbCloseSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicSearch")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicSearch") = dtDynamics
            dlListSearch.DataSource = dtDynamics
            dlListSearch.DataBind()
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dllistsearch.DataSource = dtt
        dllistsearch.DataBind()
        FillGridNew()

    End Sub

    Private Function BuildConditionNew(ByVal strGrpNameValue As String, ByVal strCtryNameValue As String, ByVal strCityNameValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strGrpNameValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(othtypmast.othtypname) IN (" & Trim(strGrpNameValue.Trim.ToUpper) & ")"
         
            End If
        End If
        If strCtryNameValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(ctrymast.ctryname) IN (" & Trim(strCtryNameValue.Trim.ToUpper) & ")"
    
            End If
        End If
        If strCityNameValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(citymast.cityname) IN (" & Trim(strCityNameValue.Trim.ToUpper) & ")"

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
                        strWhereCond1 = "(upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') or (upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') or (upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
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
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamic")
        Dim strGrpNameValue As String = ""
        Dim strCtryNameValue As String = ""
        Dim strCityNameValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "GROUPNAME" Then
                        If strGrpNameValue <> "" Then
                            strGrpNameValue = strGrpNameValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strGrpNameValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "COUNTRYNAME" Then
                        If strCtryNameValue <> "" Then
                            strCtryNameValue = strCtryNameValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCtryNameValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CITYNAME" Then
                        If strCityNameValue <> "" Then
                            strCityNameValue = strCityNameValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityNameValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Dim pagevaluems = RowsPerPageMS.SelectedValue
            strBindCondition = BuildConditionNew(strGrpNameValue, strCtryNameValue, strCityNameValue, strTextValue)
            Dim myDS As New DataSet
            '   Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"
            strSqlQry = "SELECT othtypmast.*,ctrymast.ctryname, citymast.cityname,[IsActive]=case when othtypmast.active=1 then 'Active' when othtypmast.active=0 then 'InActive' end FROM othtypmast " & _
                      "INNER JOIN ctrymast ON othtypmast.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON othtypmast.citycode = citymast.citycode where othgrpcode in (select option_selected from reservation_parameters where param_id =1001)"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
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
                gv_SearchResult.PageSize = pagevaluems
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
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
                Case "GROUPNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("GROUPNAME", lsProcessCity, "GRPNAME")
                Case "COUNTRYNAME"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRYNAME", lsProcessAll, "CTRYNAME")
                Case "CITYNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CITYNAME", lsProcessCity, "CITYNAME")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dllistsearch.DataSource = dtt
        dllistsearch.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Page.Title = "Sector Groups"
        If IsPostBack = False Then

            Try
                'SetFocus(txtCusCode)
                SetFocus(btnHelp)
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
                RowsPerPageMS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "PriceListModule\SectorGroupSearch.aspx?appid=" + strappid, btnAddNew, btnExport, _
                                                      btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

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
                Session.Add("strsortExpression", "othtypcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '  fillorderby()
                FillGridNew()


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SectSupWindowPostBack") Then
            btnResetSelection_Click(sender, e)
        End If
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
            strSqlQry = "SELECT othtypmast.*,ctrymast.ctryname, citymast.cityname,[IsActive]=case when othtypmast.active=1 then 'Active' when othtypmast.active=0 then 'InActive' end FROM othtypmast " & _
                    "INNER JOIN ctrymast ON othtypmast.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON othtypmast.citycode = citymast.citycode where othgrpcode in (select option_selected from reservation_parameters where param_id =1001)"

            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

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
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    '#Region "  Private Function BuildCondition() As String"
    '    Private Function BuildCondition() As String

    '        strWhereCond = ""
    '        If txtCusCode.Text.Trim <> "" Then
    '            If Trim(strWhereCond) = "" Then

    '                strWhereCond = " upper(agentcatmast.agentcatcode) LIKE '" & Trim(txtCusCode.Text.Trim.ToUpper) & "%'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatcode) LIKE '" & Trim(txtCusCode.Text.Trim.ToUpper) & "%'"
    '            End If
    '        End If

    '        If txtCusName.Text.Trim <> "" Then
    '            If Trim(strWhereCond) = "" Then

    '                strWhereCond = " upper(agentcatmast.agentcatname) LIKE '" & Trim(txtCusName.Text.Trim.ToUpper) & "%'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatname) LIKE '" & Trim(txtCusName.Text.Trim.ToUpper) & "%'"
    '            End If
    '        End If




    '        BuildCondition = strWhereCond
    '    End Function

    '#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("CustomerCategories.aspx", False)


        Dim strpop As String = ""
        'strpop = "window.open('CustomerCategories.aspx?State=New','CustomerCategories','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('SectorGroup.aspx?State=New','SectorGroup');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region

#Region "rotected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
        'Select Case ddlOrderBy.SelectedIndex
        '    Case 0
        '        FillGridNew()
        '        'FillGrid("agentcatmast.agentcatname")
        '    Case 1

        '        'FillGrid("agentcatmast.agentcatcode")
        '        'Case 2
        '        '    FillGrid("sellmast.sellname")
        '        'Case 3
        '        '    FillGrid("sellmast.sellcode")

        'End Select
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand





        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblID As Label
            lblID = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblSectorCode")
            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""

                strpop = "window.open('SectorGroup.aspx?State=Edit&RefCode=" + CType(lblID.Text.Trim, String) + "','SectorGroups');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then

                Dim strpop As String = ""

                strpop = "window.open('SectorGroup.aspx?State=View&RefCode=" + CType(lblID.Text.Trim, String) + "','SectorGroups');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""

                strpop = "window.open('SectorGroup.aspx?State=Delete&RefCode=" + CType(lblID.Text.Trim, String) + "','SectorGroups');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

    '#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    '    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '        FillGrid("agentcatcode")
    '    End Sub

    '#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'txtCusCode.Text = ""
        'txtCusName.Text = ""
        'ddlSellingType.Value = "[Select]"
        'ddlSellingName.Value = "[Select]"
        'FillGrid("agentcatcode")
        'ddlOrderBy.SelectedIndex = 0
        FillGrid("othtypmast.othtypcode")

    End Sub

#End Region


    Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing

    End Sub


#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

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

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                strSqlQry = "SELECT othtypmast.othtypcode as[ Group Code],othtypmast.othtypname as[ Group Name],othtypmast.incomecode,othtypmast.expensecode,ctrymast.ctryname as Country , citymast.cityname as City,[IsActive]=case when othtypmast.active=1 then 'Active' when othtypmast.active=0 then 'InActive' end ,(Convert(Varchar, Datepart(DD,othtypmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othtypmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othtypmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othtypmast.adddate))+ ':' + Convert(Varchar, Datepart(m,othtypmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,othtypmast.adddate))) as [Date Created],othtypmast.adduser as [User Created],(Convert(Varchar, Datepart(DD,othtypmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othtypmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,othtypmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othtypmast.moddate))+ ':' + Convert(Varchar, Datepart(m,othtypmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,othtypmast.moddate))) as [Date Modified],othtypmast.moduser as [User Modified]  FROM othtypmast INNER JOIN ctrymast ON othtypmast.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON othtypmast.citycode = citymast.citycode where othgrpcode in (select option_selected from reservation_parameters where param_id =1001)"
                'strSqlQry = "SELECT  agentcatcode AS [Customer Code] , agentcatname AS [Name],[Active]=case when active=1 then 'Active' when active=0 then 'In Active' end,(Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created],adduser as [User Created],(Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified],moduser as [User Modified]  FROM agentcatmast"
                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY agentcatcode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY agentcatcode"
                'End If
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

#Region " Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""



            ' strpop = "window.open('rptReportNew.aspx?Pageame=Sectorgroup&BackPageName=SectorGroupSearch.aspx&othtypcode=""&othtypname=""&CtryCode=""','RepSector','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

        
            strpop = "window.open('rptReportNew.aspx?Pageame=Sectorgroup&BackPageName=SectorGroupSearch.aspx','RepSector','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

#Region "Protected Sub rbtnSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub rbtnSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlAdvSearch.Visible = False
        'lblSellingTypeCode.Visible = False
        'lblSellingTypeName.Visible = False
        'ddlSellingType.Visible = False
        'ddlSellingName.Visible = False
    End Sub

#End Region




    'Private Sub fillorderby()
    '    ddlOrderBy.Items.Clear()
    '    ddlOrderBy.Items.Add("Category Name")
    '    ddlOrderBy.Items.Add("Category Code")
    '    'ddlOrderBy.Items.Add("Selling Name")
    '    'ddlOrderBy.Items.Add("Selling Code")
    '    ddlOrderBy.SelectedIndex = 0
    'End Sub

    'Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
    ' Select Case ddlOrderBy.SelectedIndex
    '    Case 0
    '        FillGrid("agentcatmast.agentcatname")
    '    Case 1
    '        FillGrid("agentcatmast.agentcatcode")


    'End Select
    'End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustomerCategoriesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



    Protected Sub dllistsearch_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dllistsearch.SelectedIndexChanged

    End Sub

    Protected Sub gv_SearchResult_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gv_SearchResult.SelectedIndexChanged

    End Sub

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblothtypname As Label = e.Row.FindControl("lblothtypname")
            Dim lblctryname As Label = e.Row.FindControl("lblctryname")
            Dim lblcityname As Label = e.Row.FindControl("lblcityname")
            Dim lsothtypname As String = ""
            Dim lsctryname As String = ""
            Dim lscityname As String = ""
            Dim lstext As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsothtypname = ""

                        If "GROUPNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsothtypname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "COUNTRYNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsctryname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If


                        If "CITYNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lscityname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsothtypname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsctryname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lscityname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsothtypname.Trim <> "" Then
                            lblothtypname.Text = Regex.Replace(lblothtypname.Text.Trim, lsothtypname.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lsctryname.Trim <> "" Then
                            lblctryname.Text = Regex.Replace(lblctryname.Text.Trim, lsctryname.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lscityname.Trim <> "" Then
                            lblcityname.Text = Regex.Replace(lblcityname.Text.Trim, lscityname.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lscityname.Trim <> "" Then
                            lblcityname.Text = Regex.Replace(lblcityname.Text.Trim, lscityname.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If






    End Sub

    Protected Sub btnClearDate_Click(sender As Object, e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub RowsPerPageMS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageMS.SelectedIndexChanged
        FillGridNew()
    End Sub
End Class
