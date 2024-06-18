'------------================--------------=======================------------------================
'   Module Name    :    CustomerSectorSearch.aspx
'   Developer Name :    Amit Survase
'   Date           :    18 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class CustomerSectorSearch
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
        SectorCodeTCol = 0
        SectorCode = 1
        SectorName = 2
        'CountryCode = 3
        CountryName = 3
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
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   'changed by mohamed on 27/08/2018
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region


    Private Function BuildConditionNew(ByVal strSectorValue As String, ByVal strCountryValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strSectorValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then



                strWhereCond = "upper(agent_sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            End If
        End If


        If strCountryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "(agent_sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(agent_sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),agent_sectormaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),agent_sectormaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),agent_sectormaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),agent_sectormaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If


        BuildConditionNew = strWhereCond
    End Function
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

                Case "COUNTRY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCity, "COUNTRY")

                Case "SECTOR"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SECTOR", lsProcessCity, "SECTOR")

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
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CountriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = "Customer Sector"
        If Page.IsPostBack = False Then
            Try
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""

                strappid = CType(Request.QueryString("appid"), String)



                If CType(Request.QueryString("appid"), String) = "4" Then
                    strappname = AppName.Value
                ElseIf CType(Request.QueryString("appid"), String) = "14" Then
                    strappname = AppName.Value
                Else
                    strappname = AppName.Value
                End If



                txtconnection.Value = Session("dbconnectionName")
                RowsPerPageMS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                'SetFocus(TxtSectorCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\CustomerSectorSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountrycd, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active =1 order by ctrycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountrynm, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active =1 order by ctryname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketcd, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketnm, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
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
                fillorderby()
                Session.Add("strsortExpression", "sectorcode")
                Session.Add("strsortdirection", SortDirection.Ascending)

                FillGridNew()

            Catch ex As Exception
                objUtils.WritErrorLog("CustomerSectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try

            'Else
            '    Try
            '        If ddlmarketcd.Value <> "[Select]" Then

            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountrycd, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where plgrpcode='" & ddlmarketnm.Value & "' and active =1 order by ctrycode", True, ddlcountrycd.Value)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountrynm, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where plgrpcode='" & ddlmarketnm.Value & "' and active =1 order by ctryname", True, ddlcountrynm.Value)
            '        Else
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountrycd, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active =1 order by ctrycode", True)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcountrynm, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active =1 order by ctryname", True)

            '        End If
            '    Catch ex As Exception
            '        objUtils.WritErrorLog("CustomerSectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            '    End Try
        End If

        'Dim typ As Type
        'typ = GetType(DropDownList)

        'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
        '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

        '    ddlcountrycd.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        '    ddlcountrynm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        '    ddlmarketcd.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        '    ddlmarketnm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        'End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CustsectWindowPostBack") Then
            btnResetSelection_Click(sender, e)
        End If

    End Sub
#End Region
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamic")
        Dim strSectorValue As String = ""
        Dim strCountryValue As String = ""

        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                        If strSectorValue <> "" Then
                            strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            strBindCondition = BuildConditionNew(strSectorValue, strCountryValue, strTextValue)
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
            strSqlQry = "select agent_sectormaster.sectorcode,agent_sectormaster.sectorname,ctrymast.ctryname,case when agent_sectormaster.active=1  " & _
                " then 'Active' else 'In active' end as [Active],agent_sectormaster.adddate,agent_sectormaster.adduser,agent_sectormaster.moddate  as moddate, " & _
                " isnull( agent_sectormaster.moduser,'') as moduser from agent_sectormaster  join ctrymast  on  agent_sectormaster.ctrycode= ctrymast.ctrycode   "
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
            objUtils.WritErrorLog("CustomerSectorsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If TxtSectorCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(agent_sectormaster.sectorcode) LIKE '" & Trim(TxtSectorCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorcode) LIKE '" & Trim(TxtSectorCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If TxtSecorName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(agent_sectormaster.sectorname) LIKE '" & Trim(TxtSecorName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) LIKE '" & Trim(TxtSecorName.Text.Trim.ToUpper) & "%'"
            End If
        End If

        'If ddlcountrycd.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(ctrymast.ctrycode) = '" & Trim(ddlcountrycd.Items(ddlcountrycd.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(ctrymast.ctrycode) = '" & Trim(ddlcountrycd.Items(ddlcountrycd.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If

        'If ddlcountrynm.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(ctrymast.ctryname) = '" & Trim(ddlcountrynm.Items(ddlcountrynm.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) = '" & Trim(ddlcountrynm.Items(ddlcountrynm.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If

        'If ddlmarketcd.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(plgrpmast.plgrpcode) = '" & Trim(ddlmarketcd.Items(ddlmarketcd.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(plgrpmast.plgrpcode) = '" & Trim(ddlmarketcd.Items(ddlmarketcd.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If

        'If ddlmarketnm.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(plgrpmast.plgrpname) = '" & Trim(ddlmarketnm.Items(ddlmarketnm.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(plgrpmast.plgrpname) = '" & Trim(ddlmarketnm.Items(ddlmarketnm.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If
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
            '            strSqlQry = "SELECT * FROM sectormaster"
            'strSqlQry = " SELECT     dbo.sectormaster.*, dbo.ctrymast.ctrycode AS Expr1, dbo.ctrymast.ctryname, dbo.citymast.citycode AS Expr2, dbo.citymast.cityname FROM         dbo.citymast INNER JOIN " & _
            ' "   dbo.ctrymast ON dbo.citymast.ctrycode = dbo.ctrymast.ctrycode INNER JOIN   dbo.sectormaster ON dbo.citymast.citycode = dbo.sectormaster.citycode AND dbo.ctrymast.ctrycode = dbo.sectormaster.ctrycode "

            ' strSqlQry = "SELECT     dbo.agent_sectormaster.sectorcode, dbo.agent_sectormaster.sectorname, dbo.agent_sectormaster.ctrycode, dbo.ctrymast.ctryname ," & _
            '  "           dbo.agent_sectormaster.plgrpcode, dbo.plgrpmast.plgrpname, dbo.agent_sectormaster.active, dbo.agent_sectormaster.adddate, " & _
            '  " dbo.agent_sectormaster.adduser, dbo.agent_sectormaster.moddate, dbo.agent_sectormaster.moduser " & _
            '" FROM         dbo.agent_sectormaster INNER JOIN  dbo.plgrpmast ON dbo.agent_sectormaster.plgrpcode = dbo.plgrpmast.plgrpcode INNER JOIN" & _
            ' "dbo.ctrymast ON dbo.agent_sectormaster.ctrycode = dbo.ctrymast.ctrycode AND dbo.plgrpmast.plgrpcode = dbo.ctrymast.plgrpcode"

            strSqlQry = "SELECT     dbo.agent_sectormaster.sectorcode, dbo.agent_sectormaster.sectorname," & _
                        " dbo.ctrymast.ctryname ," & _
       " case when agent_sectormaster.active=1 " & _
                " then 'Active' else 'In active' end as [Active], " & _
            "dbo.agent_sectormaster.adddate,  dbo.agent_sectormaster.adduser, dbo.agent_sectormaster.moddate,dbo.agent_sectormaster.moduser  " & _
            "FROM dbo.agent_sectormaster  INNER JOIN " & _
                      "dbo.ctrymast ON dbo.agent_sectormaster.ctrycode = dbo.ctrymast.ctrycode "
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
            objUtils.WritErrorLog("CustomerSectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("CustomerSector.aspx", False)


        Dim strpop As String = ""
        'strpop = "window.open('CustomerSector.aspx?State=New','CustomerSector','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('CustomerSector.aspx?State=New','CustomerSector');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
        'FillGrid("agent_sectormaster.sectorcode")
        'Select Case ddlOrderBy.SelectedIndex
        '    Case 0
        '        FillGrid(" agent_sectormaster.sectorname")
        '    Case 1
        '        FillGrid(" agent_sectormaster.sectorcode")
        '    Case 2
        '        FillGrid("ctrymast.ctryname")
        '    Case 3
        '        FillGrid("ctrymast.ctrycode")
        '    Case 4
        '        FillGrid("plgrpmast.plgrpname")
        '    Case 5
        '        FillGrid("plgrpmast.plgrpcode")
        ' End Select
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblSectorCode")

            If e.CommandName = "Editrow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("CustomerSector.aspx", False)


                Dim strpop As String = ""
                'strpop = "window.open('CustomerSector.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','CustomerSector','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CustomerSector.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','CustomerSector');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                ' Session.Add("State", "View")
                ' Session.Add("RefCode", CType(lblId.Text.Trim, String))
                ' Response.Redirect("CustomerSector.aspx", False)



                Dim strpop As String = ""
                'strpop = "window.open('CustomerSector.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','CustomerSector','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CustomerSector.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','CustomerSector');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("CustomerSector.aspx", False)





                Dim strpop As String = ""
                ' strpop = "window.open('CustomerSector.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','CustomerSector','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CustomerSector.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','CustomerSector');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerSectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("sectorcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid(" agent_sectormaster.sectorname")
            Case 1
                FillGrid(" agent_sectormaster.sectorcode")
            Case 2
                FillGrid("ctrymast.ctryname")
            Case 3
                FillGrid("ctrymast.ctrycode")
            Case 4
                FillGrid("plgrpmast.plgrpname")
            Case 5
                FillGrid("plgrpmast.plgrpcode")
        End Select
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtSectorCode.Text = ""
        TxtSecorName.Text = ""
        ddlcountrycd.Value = "[Select]"
        ddlcountrynm.Value = "[Select]"
        ddlmarketcd.Value = "[Select]"
        ddlmarketnm.Value = "[Select]"
        ' FillGrid("agent_sectormaster.sectorcode")
        ddlOrderBy.SelectedIndex = 0
        FillGrid(" agent_sectormaster.sectorname")
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then



            Dim lblCountryName As Label = e.Row.FindControl("lblCountryName")

            Dim lblSectorName As Label = e.Row.FindControl("lblSectorName")
            Dim lsSearchTextCtry As String = ""
            Dim lsSearchSector As String = ""
            Dim lsSearchCtry As String = ""
            Dim lsSearchTextSector As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1

                        '    lsSearchTextCtry = ""
                        If "SECTOR" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "COUNTRY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            ' lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If


                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            ' lsSearchTextCtry = lsSearchTextSector
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchCtry = lsSearchTextCtry
                            lsSearchSector = lsSearchTextSector

                        End If

                        If lsSearchCtry.Trim <> "" Then
                            lblCountryName.Text = Regex.Replace(lblCountryName.Text.Trim, lsSearchCtry.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchSector.Trim <> "" Then
                            lblSectorName.Text = Regex.Replace(lblSectorName.Text.Trim, lsSearchSector.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                    Next
                End If
            End If


        End If


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

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
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
                strSqlQry = "SELECT agent_sectormaster.sectorcode AS [SectorCode],agent_sectormaster.sectorname as [SectorName],ctrymast.ctryname as [Country]," & _
"[Active]=case when agent_sectormaster.active =1 then 'Active' when agent_sectormaster.active=0 then 'InActive' end,isnull(agent_sectormaster.adduser,'') as [User Created], (Convert(Varchar, Datepart(DD,agent_sectormaster.adddate))+ '/'+ Convert(Varchar, Datepart(MM,agent_sectormaster.adddate))+ '/'+ Convert(Varchar, Datepart(YY,agent_sectormaster.adddate)) + ' ' + Convert(Varchar, Datepart(hh,agent_sectormaster.adddate))+ ':' + Convert(Varchar, Datepart(m,agent_sectormaster.adddate))+ ':'+ Convert(Varchar, Datepart(ss,agent_sectormaster.adddate))) as [Date Created],isnull(agent_sectormaster.moduser,'') as [User Modified] ,isnull((Convert(Varchar, Datepart(DD,agent_sectormaster.moddate))+ '/'+ Convert(Varchar, Datepart(MM,agent_sectormaster.moddate))+ '/'+ Convert(Varchar, Datepart(YY,agent_sectormaster.moddate)) + ' ' + Convert(Varchar, Datepart(hh,agent_sectormaster.moddate))+ ':' + Convert(Varchar, Datepart(m,agent_sectormaster.moddate))+ ':'+ Convert(Varchar, Datepart(ss,agent_sectormaster.moddate))),'') as [Date Modified] " & _
" FROM  agent_sectormaster  join" & _
 " ctrymast ON agent_sectormaster.ctrycode = ctrymast.ctrycode and ctrymast.active=1 "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY sectorcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY sectorcode"
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(DS, "agent_sectormaster")

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
            Session("ColReportParams") = Nothing
            Session.Add("Pageame", "CustomerSector")
            Session.Add("BackPageName", "CustomerSectorSearch.aspx")
            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=CustomerSector&BackPageName=CustomerSectorSearch.aspx&SectCode=" & TxtSectorCode.Text.Trim & "&SectName=" & TxtSecorName.Text.Trim & "&MktCode=" & Trim(ddlmarketcd.Items(ddlmarketcd.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlcountrycd.Items(ddlcountrycd.SelectedIndex).Text) & "','RepCustSect','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=CustomerSector&BackPageName=CustomerSectorSearch.aspx&SectCode=" & TxtSectorCode.Text.Trim & "&SectName=" & TxtSecorName.Text.Trim & "','RepCustSect');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'If TxtSectorCode.Text.Trim <> "" Then
            '    strReportTitle = "Customer Sector Code : " & TxtSectorCode.Text.Trim
            '    strSelectionFormula = "{agent_sectormaster.sectorcode} LIKE '" & TxtSectorCode.Text.Trim & "*'"
            'End If
            'If TxtSecorName.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Customer Sector Name : " & TxtSecorName.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {agent_sectormaster.sectorname} LIKE '" & TxtSecorName.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Customer Sector Name : " & TxtSecorName.Text.Trim
            '        strSelectionFormula = "{agent_sectormaster.sectorname} LIKE '" & TxtSecorName.Text.Trim & "*'"
            '    End If
            'End If

            'If ddlcountrynm.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Country : " & ddlcountrynm.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {agent_sectormaster.ctrycode} = '" & ddlcountrynm.Value.Trim & "'"
            '    Else
            '        strReportTitle = "Country : " & ddlcountrynm.Value.Trim
            '        strSelectionFormula = "{agent_sectormaster.ctrycode} = '" & ddlcountrynm.Value.Trim & "'"
            '    End If
            'End If

            'If ddlmarketcd.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & " ; Market : " & ddlmarketcd.Value.Trim
            '        strSelectionFormula = strSelectionFormula & " and {agent_sectormaster.plgrpcode} = '" & ddlmarketcd.Value.Trim & "'"
            '    Else
            '        strReportTitle = "Market : " & ddlmarketcd.Value.Trim
            '        strSelectionFormula = "{agent_sectormaster.plgrpcode} = '" & ddlmarketcd.Value.Trim & "'"
            '    End If
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerSectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.CheckedChanged
        'PnlCustSect.Visible = False
        lblmarketcode.Visible = False
        lblmarketname.Visible = False
        lblctrycode.Visible = False
        lblCtryname.Visible = False
        ddlcountrycd.Visible = False
        ddlcountrynm.Visible = False
        ddlmarketcd.Visible = False
        ddlmarketnm.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlCustSect.Visible = True
        lblmarketcode.Visible = True
        lblmarketname.Visible = True
        lblctrycode.Visible = True
        lblCtryname.Visible = True
        ddlcountrycd.Visible = True
        ddlcountrynm.Visible = True
        ddlmarketcd.Visible = True
        ddlmarketnm.Visible = True
    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Sector Name")
        ddlOrderBy.Items.Add("Sector Code")
        ddlOrderBy.Items.Add("Country Name")
        ddlOrderBy.Items.Add("Country Code")
        ddlOrderBy.Items.Add("Market Name")
        ddlOrderBy.Items.Add("Market Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid(" agent_sectormaster.sectorname")
            Case 1
                FillGrid(" agent_sectormaster.sectorcode")
            Case 2
                FillGrid("ctrymast.ctryname")
            Case 3
                FillGrid("ctrymast.ctrycode")
            Case 4
                FillGrid("plgrpmast.plgrpname")
            Case 5
                FillGrid("plgrpmast.plgrpcode")
        End Select
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustomerSectorSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub RowsPerPageMS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageMS.SelectedIndexChanged
        FillGridNew()
    End Sub

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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

    Protected Sub gv_SearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_SearchResult.SelectedIndexChanged

    End Sub
End Class
