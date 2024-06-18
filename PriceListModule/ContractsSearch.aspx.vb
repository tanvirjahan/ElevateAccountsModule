'------------================--------------=======================------------------================
'   Module Name    :    ContractsSearch.aspx
'   Developer Name :    Amit Survase
'   Date           :    17 July 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ContractsSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strSqlQry1 As String
    Dim strSqlQry2 As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
#End Region


#Region "Enum GridCol"
    Enum GridCol
        contractid = 0
        partyname = 1
        supagentname = 2
        applicableto = 3
        FromDate = 4
        Todate = 5
        status = 6
        activestate = 7
        Edit = 10
        View = 11
        Delete = 12
        Copy = 13
        DateCreated = 14
        UserCreated = 15
        DateModified = 16
        UserModified = 17
        approveddate = 18
        approveduser = 19
    End Enum
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '  btnPrint.Visible = False
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' btnPrint.Visible = False
        If Page.IsPostBack = False Then
            Try

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim frmdate As String = ""
                Dim todate As String = ""
                ' btnPrint.Visible = False

                Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))
                ViewState("appid") = CType(Request.QueryString("appid"), String)

                If Session("partycode") Is Nothing Then
                    ViewState("partycode") = CType(Request.QueryString("partycode"), String)
                    Session("partycode") = CType(Request.QueryString("partycode"), String)
                    hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                Else
                    ViewState("partycode") = Session("partycode")
                    ' Session("partycode") = Session("Contractparty")
                    hdnpartycode.Value = Session("partycode")
                End If


                '  If Session("partycode") Is Nothing Then

                'End If


                Me.whotelatbcontrol.partyval = CType(Request.QueryString("partycode"), String)
                If AppId Is Nothing = False Then

                    ViewState("appid") = AppId.Value
                End If
                strappid = 1
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                Page.Title = "Contract Search "
                txtconnection.Value = Session("dbconnectionName")
                chkshowall.Style.Add("display", "block")
                '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)

                SetFocus(TxtContractId)

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                             CType(strappname, String), "PriceListModule\ContractsSearch.aspx?appid=1", btnAddNew, btnExportToExcel, btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If
                '  btnPrint.Visible = False
                'If txtFromDate.Text = "" Then
                '    txtFromDate.Text = objDateTime.GetSystemDateOnly
                'End If
                'If txtToDate.Text = "" Then
                '    txtToDate.Text = objDateTime.GetSystemDateOnly
                'End If

                checkIsPrivilege()

                ViewState("strsortExpression") = "contractid"
                Session.Add("strsortExpression", "contractid")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '  charcters(TxtPLCD)
                ddlOrder.SelectedIndex = 2



                FillGrid("contractid", "DESC")
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

                'ChkW1.Visible = False
                'ChkWeek2.Visible = False
                '  txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode ='HOT' order by partyname", True, ddlSuppierNM.Value)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode ='HOT' order by partycode", True, ddlSuppierNM.Items(ddlSuppierNM.SelectedIndex).Text)

            If Session("partycode") Is Nothing Then
                ViewState("partycode") = CType(Request.QueryString("partycode"), String)
                Session("partycode") = CType(Request.QueryString("partycode"), String)
                hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
            Else
                ViewState("partycode") = Session("partycode")
                ' Session("partycode") = Session("Contractparty")
                hdnpartycode.Value = Session("partycode")
            End If

            Me.whotelatbcontrol.partyval = hdnpartycode.Value

        End If
        Dim typ As Type
        typ = GetType(DropDownList)

        'If Request.QueryString("AutoNo") > 0 Then
        '    ViewState("AutoNo") = Request.QueryString("AutoNo")
        '    Session("AutoNo") = 0
        'Else
        '    ViewState("AutoNo") = 0
        'End If


        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlSPTypeCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSPTypeNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            'ddlmarketCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            'ddlMarketNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


            ddlSupplierAgent.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSuppierAgentNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            'ddlSuppierCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            'ddlSuppierNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlCurrencyCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCurrencyNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlSubSeas.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSubSeasNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        End If

        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ContractMainWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If


    End Sub

#End Region

#Region "Public Function checkIsPrivilege() As Boolean"
    Public Function checkIsPrivilege() As Boolean
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='8' and appid='1' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Statusapprove", "Yes")
            Else
                Session.Add("Statusapprove", "No")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try

    End Function
#End Region


#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If TxtContractId.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(contractid) = '" & Trim(TxtContractId.Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(contractid) = '" & Trim(TxtContractId.Text.Trim.ToUpper) & "'"
            End If
        End If

        If txtSupName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(partyname) = '" & Trim(txtSupName.Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(partyname) = '" & Trim(txtSupName.Text.Trim.ToUpper) & "'"
            End If
        End If

        'If txtCountryGroup.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(countrygroupname) = '" & Trim(txtCountryGroup.Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(countrygroupname) = '" & Trim(txtCountryGroup.Text.Trim.ToUpper) & "'"
        '    End If
        'End If
        'If txtCoutryName.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(ctryname) = '" & Trim(txtCoutryName.Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(ctryname) = '" & Trim(txtCoutryName.Text.Trim.ToUpper) & "'"
        '    End If
        'End If

        'If txtAgent.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(agentname) = '" & Trim(txtAgent.Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(agentname) = '" & Trim(txtAgent.Text.Trim.ToUpper) & "'"
        '    End If
        'End If
        If txtApproved.Text.Trim <> "" Then
            Dim strStatus As String = ""
            If txtApproved.Text.Trim = "Approved" Then
                strStatus = "Yes"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                End If
            ElseIf txtApproved.Text.Trim = "Pending" Then
                strStatus = "No"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                End If
            Else

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
        Dim myds1 As New DataSet
        Dim mydt1 As New DataTable
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        strSqlQry1 = ""
        Try


            If chkshowall.Checked = True Then
                strSqlQry = "select * from view_contracts_search(nolock) "
            Else
                strSqlQry = "select * from view_contracts_search(nolock)  where partycode='" & hdnpartycode.Value & "'"
            End If


            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & BuildCondition()

            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            Dim pliststr As String = ""
            'If ViewState("MyAutoNo") <> 1 Then
            '    pliststr = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "pricelist_links", "calledfromlist", "Autoid", Request.QueryString("AutoNo"))
            'End If

            If pliststr <> "" Then
                strSqlQry = strSqlQry & " WHERE contractid='" & pliststr & "'"

                ''strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            Else

                If Trim(BuildCondition) <> "" Then


                    strSqlQry = strSqlQry & "  " & BuildCondition()

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                Else

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If

            End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
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
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub


#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        ''Session("State") = "New"
        ''Response.Redirect("HeaderInfo.aspx")
        ''Response.Redirect("Currencies.aspx", False)
        'Dim strpop As String = ""
        'Session("MealPlans") = Nothing
        'strpop = "window.open('HeaderInfonew.aspx?State=New','HeaderInfo','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        '  Session.Add("State", "New")
        Session.Add("ContractState", "New")
        Session("ContractRefCode") = Nothing
        Session("contractid") = Nothing
        Session("State") = Nothing
        Session("Contractparty") = hdnpartycode.Value
        Session("Calledfrom") = "Contracts"
      
        Dim strpop As String = ""
        strpop = "window.open('ContractMain.aspx?Calledfrom=Contracts&appid=" + CType(ViewState("appid"), String) + "&partycode=" + hdnpartycode.Value + "','Contracts');"
        ' strpop = "window.open('ContractMain.aspx?appid=" + CType(ViewState("appid"), String) + "&partycode=" + hdnpartycode.Value + "&State=New','Contracts');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

#End Region

    Function SetVisibility(ByVal desc As Object, ByVal maxlen As Integer) As Boolean

        If desc.ToString = "" Then
            Return False
        Else
            If desc.ToString.Length > maxlen Then
                Return True
            Else
                Return False
            End If
        End If


    End Function
    Function Limit(ByVal desc As Object, ByVal maxlen As Integer) As String

        If desc.ToString = "" Then
            Return ""
        Else
            If desc.ToString.Length > maxlen Then
                desc = desc.Substring(0, maxlen)
            Else

                desc = desc
            End If
        End If

        Return desc


    End Function
    Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("cplisthnew.plistcode", "DESC")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("contractid", "DESC")
                '  FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("contractid", "ASC")
                'FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("partycode", "ASC")
                ' FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '   FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")
                ' FillGrid("supplier_agents.supagentname", "supplier_agents.supagentname", "ASC")
                'Case 5
                '    FillGrid("plgrpcode", "ASC")
                '    '  FillGrid("cplisthnew.plgrpcode", "edit_cplisthnew.plgrpcode", "ASC")
                'Case 6
                '    FillGrid("subseascode", "ASC")
                ' FillGrid("cplisthnew.subseascode", "edit_cplisthnew.plistcode", "ASC")

        End Select
    End Sub

#End Region

    Private Sub GetMealPlanString(ByVal approve As Integer, ByVal plistcode As String)
        Try
            Dim mStrqry As String = ""
            If approve = 1 Then
                mStrqry = "SELECT mealcode FROM cplist_mealplan Where plistcode='" & plistcode & "'"
            Else
                mStrqry = "SELECT mealcode FROM edit_cplist_mealplan Where plistcode='" & plistcode & "'"
            End If

            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mStrqry)
            Dim mealstr As String = ""
            If ds.Tables.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    If mealstr.Length = 0 Then
                        mealstr = row.Item("mealcode")
                    Else
                        mealstr += "," + row.Item("mealcode")
                    End If
                Next
            End If
            Session("MealPlans") = Nothing
            Session("MealPlans") = mealstr
        Catch ex As Exception

        End Try
    End Sub


#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            Dim lblagent As Label
            Dim partycode As Label
            Dim supagentcode As String
            Dim supagentname As Label
            Dim lblCountryGroup As Label

            Dim approve As Label

            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            partycode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblparty")
            lblCountryGroup = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblmarket")
            lblagent = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblagent")
            Dim lblactive As Label = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblactive")

            Session("Calledfrom") = "Contracts"
            Dim status As Integer = 0
            '   If approve.Text = "Approved" Then status = 1
            If e.CommandName = "Page" Then Exit Sub

            Session("contractid") = Nothing
            If e.CommandName = "Editrow" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcodenew") '' changed shahul 11/06/2018
                Session.Add("ContractState", "Edit")
                Session.Add("ContractRefCode", CType(lblId.Text.Trim, String))
                Session("Contractparty") = partycode.Text
                Session.Add("contractid", CType(lblId.Text.Trim, String))


                Dim strpop As String = ""
                strpop = "window.open('ContractMain.aspx?Calledfrom=Contracts&appid=" + CType(Request.QueryString("appid"), String) + "&partycode=" + partycode.Text + "&State=Edit','ContractMain');"



                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
                Session.Add("ContractState", "View")
                Session.Add("ContractRefCode", CType(lblId.Text.Trim, String))
                Session("Contractparty") = partycode.Text
                Session.Add("contractid", CType(lblId.Text.Trim, String))

                Dim strpop As String = ""
                'strpop = "window.open('ContractMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','ContractMain');"
                strpop = "window.open('ContractMain.aspx?Calledfrom=Contracts&appid=" + CType(Request.QueryString("appid"), String) + "&partycode=" + partycode.Text + "&State=View','ContractMain');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")

                Dim approvestr As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT 't' FROM contracts(nolock) Where  contractid='" & lblId.Text & "'")
                If approvestr <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contract already approved so cannot be delete Please proceed to Withdraw' );", True)
                    Exit Sub
                End If


                Session.Add("ContractState", "Delete")
                Session.Add("ContractRefCode", CType(lblId.Text.Trim, String))
                Session("Contractparty") = partycode.Text
                Session.Add("contractid", CType(lblId.Text.Trim, String))

                Dim strpop As String = ""
                'strpop = "window.open('ContractMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','ContractMain');"
                strpop = "window.open('ContractMain.aspx?Calledfrom=Contracts&appid=" + CType(Request.QueryString("appid"), String) + "&partycode=" + partycode.Text + "&State=Delete','ContractMain');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Copy" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
                Session.Add("ContractState", "Copy")
                Session.Add("ContractRefCode", CType(lblId.Text.Trim, String))
                Session("Contractparty") = partycode.Text
                Session.Add("contractid", CType(lblId.Text.Trim, String))

                Dim strpop As String = ""
                'strpop = "window.open('ContractMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','ContractMain');"
                strpop = "window.open('ContractMain.aspx?Calledfrom=Contracts&appid=" + CType(Request.QueryString("appid"), String) + "&partycode=" + partycode.Text + "&State=Copy','ContractMain');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtContractId.Text = ""
        'Txtprmname.Text = ""
        'Txtpromotionid.Text = ""
        txtSupName.Text = ""


        ddlSPTypeCD.Value = "[Select]"
        ddlSPTypeNM.Value = "[Select]"

        'ddlmarketCD.Value = "[Select]"
        'ddlMarketNM.Value = "[Select]"

        ddlSupplierAgent.Value = "[Select]"
        ddlSuppierAgentNM.Value = "[Select]"

        'ddlSuppierCD.Value = "[Select]"
        'ddlSuppierNM.Value = "[Select]"

        ddlCurrencyCD.Value = "[Select]"
        ddlCurrencyNM.Value = "[Select]"


        ddlSubSeas.Value = "[Select]"
        ddlSubSeasNM.Value = "[Select]"

        'ddlPriceList.SelectedValue = "[Select]"

        ViewState("MyAutoNo") = 1 'clear plist links 
        ddlOrderBy.SelectedIndex = 0
        ' FillGrid("cplisthnew.plistcode", "DESC")
        FillGrid("contractid", "DESC")
    End Sub
#End Region
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
    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            'Dim lblSectorName As Label = e.Row.FindControl("lblSectorName")
            'Dim lblSectorGroupName As Label = e.Row.FindControl("lblSectorGroupName")
            'Dim lblCountryName As Label = e.Row.FindControl("lblCountryName")
            'Dim lblCityName As Label = e.Row.FindControl("lblCityName")

            Dim lblplistcode As Label = e.Row.FindControl("lblplistcode")
            Dim lblparty As Label = e.Row.FindControl("lblparty")
            Dim lblagent As Label = e.Row.FindControl("lblagent")
            Dim lblsubagentname As Label = e.Row.FindControl("lblsubagentname")
            Dim lblpartyname As Label = e.Row.FindControl("lblpartyname")
            Dim lblstatus As Label = e.Row.FindControl("lblstatus")
            Dim lblapplicable As Label = e.Row.FindControl("lblapplicable")

            Dim lblactive As Label = e.Row.FindControl("lblactive")


            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextSectorGroup As String = ""

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCtry = ""

                        If "CONTRACTID" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "HOTEL" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SUPPLIERAGENT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "APPROVEDSTATUS" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSectorGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCtry
                            lsSearchTextSector = lsSearchTextCtry
                            lsSearchTextSectorGroup = lsSearchTextCtry
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblplistcode.Text = Regex.Replace(lblplistcode.Text.Trim, lsSearchTextCtry.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblpartyname.Text = Regex.Replace(lblpartyname.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextSector.Trim <> "" Then
                            lblsubagentname.Text = Regex.Replace(lblsubagentname.Text.Trim, lsSearchTextSector.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lsSearchTextSectorGroup.Trim <> "" Then
                            lblstatus.Text = Regex.Replace(lblstatus.Text.Trim, lsSearchTextSectorGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                       
                    Next
                End If
            End If
            If lblactive.Text <> "Active" Then
                e.Row.BackColor = Drawing.Color.Lavender
            End If



        End If
    End Sub
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
        FillGrid(ViewState("strsortExpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = ViewState("strsortExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
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

                strSqlQry = "select * from view_contracts_search "
                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & ""
                Else
                    strSqlQry = strSqlQry & " ORDER BY contractid"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "contracts")

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
#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        '    Try
        '        '  Session.Add("CurrencyCode", txtblocksale_header.blocksaleid.Text.Trim)
        '        '   Session.Add("CurrencyName", txtcityname.Text.Trim)
        '        '   Response.Redirect("rptCurrencies.aspx", False)

        '        Dim strReportTitle As String = ""
        '        Dim strSelectionFormula As String = ""

        '        Session.Add("Pageame", "Block Full Sales")
        '        Session.Add("BackPageName", "BlockFullSalesSearch.aspx")

        '        If txtTranId.Text.Trim <> "" Then
        '            strReportTitle = "Block Sale ID : " & txtTranId.Text.Trim
        '            strSelectionFormula = "{blocksale_header.blocksaleid} LIKE '" & txtTranId.Text.Trim & "*'"
        '        End If


        '        If ddlPartyCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlSupACode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlSupAName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlMarketCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlMarketName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlSPTypeCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlSpTypeName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        Session.Add("SelectionFormula", strSelectionFormula)
        '        Session.Add("ReportTitle", strReportTitle)
        '        Response.Redirect("rptReport.aspx", False)
        '    Catch ex As Exception
        '        objUtils.WritErrorLog("BlockFullSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        '    End Try
    End Sub
#End Region



    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.CheckedChanged
        pnlHeader.Visible = False
        SetFocus(rbtnsearch)
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnadsearch.CheckedChanged
        pnlHeader.Visible = True
        SetFocus(rbtnadsearch)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim frmdate As String = ""
        Dim todate As String = ""

        'Record list will be according to the Changing the year  
        'If Not (Session("changeyear") Is Nothing) Then
        '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

        '    If Session("changeyear") = Year(Now).ToString Then
        '        If txtPtodate.Text = "" Then
        '            todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
        '        Else
        '            todate = CType(txtPtodate.Text, Date).ToString
        '        End If
        '    Else
        '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
        '    End If

        '    txtPfromdate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
        '    txtPtodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

        'Else
        '    txtPfromdate.Text = ""
        '    txtPtodate.Text = ""
        'End If



        'Record list will be according to the Changing the year  
        'If Session("changeyear") <> Year(CType(txtPfromdate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        'If Session("changeyear") <> Year(CType(txtPtodate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If



        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("contractid", "DESC")
                '   FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("contractid", "ASC")
                '    FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("partycode", "ASC")
                '   FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '  FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")
                '   FillGrid("supplier_agents.supagentname", "supplier_agents.supagentname", "ASC")
                'Case 5
                '    FillGrid("plgrpcode", "ASC")
                '    '      FillGrid("cplisthnew.plgrpcode", "edit_cplisthnew.plgrpcode", "ASC")
                'Case 6
                '    FillGrid("subseascode", "ASC")
                '   FillGrid("cplisthnew.subseascode", "edit_cplisthnew.subseascode", "ASC")
        End Select
    End Sub
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PriceListSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("contractid", "DESC")
                '  FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("contractid", "ASC")
                '  FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("partycode", "ASC")
                '   FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '    FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")
                '  FillGrid("supplier_agents.supagentname", "supplier_agents.supagentname", "ASC")
                'Case 5
                '    FillGrid("plgrpcode", "ASC")
                '    '    FillGrid("cplisthnew.plgrpcode", "edit_cplisthnew.plgrpcode", "ASC")
                'Case 6
                '    FillGrid("subseascode", "ASC")
                '  FillGrid("cplisthnew.subseascode", "cplisthnew.subseascode", "ASC")
        End Select
        SetFocus(ddlOrderBy)
    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub

    Protected Sub btnexit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click
        'Session("AutoNo")
        Session.Remove("AutoNo")
    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "HOTEL"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("HOTEL", lsProcessCity, "HOTEL")
                Case "CONTRACTID"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CONTRACTID", lsProcessCountry, "CONTRACTID")
                Case "SUPPLIERAGENT"
                    lsProcessGroup = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIERAGENT", lsProcessGroup, "SUPPLIERAGENT")
                Case "APPROVEDSTATUS"
                    lsProcessSector = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("APPROVEDSTATUS", lsProcessSector, "APPROVEDSTATUS")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
                    'If lsProcessAll.Trim = """" Then
                    '    lsProcessAll = ""
                    'End If
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew()

    End Sub
    Protected Sub RowsPerPageSS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageSS.SelectedIndexChanged
        FillGridNew()
    End Sub
    Public Function getRowpage() As String
        Dim rowpagess As String
        If RowsPerPageSS.SelectedValue = "[Select]" Then
            rowpagess = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagess = RowsPerPageSS.SelectedValue

        End If
        Return rowpagess
    End Function
    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strSectorValue As String = ""
        Dim strSectorGroupValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try

            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "CONTRACTID" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "HOTEL" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIERAGENT" Then
                        If strSectorValue <> "" Then
                            strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "APPROVEDSTATUS" Then
                        If strSectorGroupValue <> "" Then
                            strSectorGroupValue = strSectorGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Else
                chkshowall.Style.Add("display", "block")
                chkshowall.Checked = True
            End If
            Dim pagevaluess = getRowpage()
            strBindCondition = BuildConditionNew(strCountryValue, strCityValue, strSectorValue, strSectorGroupValue, strTextValue)

            Dim myDS As New DataSet
            Dim strValue As String

            gv_SearchResult.Visible = True
            lblMsg.Visible = False

            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = ViewState("strsortExpression") 'Session("strsortexpression")
            Dim strsortorder As String = "ASC"


            ' strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster INNER JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON sectormaster.citycode = citymast.citycode INNER JOIN othtypmast ON sectormaster.sectorgroupcode  = othtypmast.othtypcode"

            strSqlQry = "select * from view_contracts_search(nolock) " 'where partycode='" & hdnpartycode.Value & "'"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " where  " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
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
                gv_SearchResult.PageSize = pagevaluess
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strSectorValue As String, ByVal strSectorGroupValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""

        If strSectorValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(supagentname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(supagentname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            End If
        End If

        If strSectorGroupValue <> "" Then
            If Trim(strWhereCond) = "" Then
                If Replace(strSectorGroupValue, "'", "") = "APPROVED" Then
                    strWhereCond = " status ='Yes'"
                ElseIf Replace(strSectorGroupValue, "'", "") = "UNAPPROVED" Then
                    strWhereCond = " status ='No'"
                Else
                    strWhereCond = " status IN (status)"
                End If
            Else
                If Replace(strSectorGroupValue, "'", "") = "APPROVED" Then
                    strWhereCond = strWhereCond & " AND   status ='Yes'"
                ElseIf Replace(strSectorGroupValue, "'", "") = "UNAPPROVED" Then
                    strWhereCond = strWhereCond & " AND  status ='No'"
                Else
                    strWhereCond = strWhereCond & " AND  status IN (status)"
                End If
                'strWhereCond = strWhereCond & " AND  upper(status) IN ( " & Trim(strSectorGroupValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCountryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(contractid) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(contractid) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If



        If strCityValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(partyname) IN ( " & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(partyname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = " (upper(contractid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(supagentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(status) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(contractid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(supagentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(status) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
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
            ElseIf ddlOrder.SelectedValue = "P" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),fromdate,111)  " _
                              & "  and convert(varchar(10),todate,111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                              & "  between convert(varchar(10),fromdate,111)  and  convert(varchar(10),todate,111))   " _
                              & " or (convert(varchar(10),fromdate,111) >= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                              & "  and convert(varchar(10),todate,111) <= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ))"

                    '  strWhereCond = " (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & " and ((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),fromdate,111)  " _
                       & "  and convert(varchar(10),todate,111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                       & "  between convert(varchar(10),fromdate,111)  and  convert(varchar(10),todate,111))   " _
                       & " or (convert(varchar(10),fromdate,111) >= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                       & "  and convert(varchar(10),todate,111) <= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)))"

                    'strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function


  
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        '  Session.Add("State", "New")

        'Session.Add("ContractState", "New")

        'Dim strpop As String = ""
        'Dim contid As String = Nothing

        'Session("ContractRefCode") = Nothing
        'Session("contractid") = Nothing
        'Session.Add("ContractState", "New")

        ''strpop = "window.open('ContractMainNew.aspx?appid=" + CType(ViewState("appid"), String) + "&partycode=" + hdnpartycode.Value + "&contractid=CON/000005','Contracts');"
        'strpop = "window.open('ContractMainNew.aspx?appid=" + CType(ViewState("appid"), String) + "&contractid=" + contid + "&partycode=" + hdnpartycode.Value + "&State=New','Contracts');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub chkshowall_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkshowall.CheckedChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("contractid", "DESC")
                '  FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("contractid", "ASC")
                'FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("partycode", "ASC")
                ' FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '   FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")
                ' FillGrid("supplier_agents.supagentname", "supplier_agents.supagentname", "ASC")
                'Case 5
                '    FillGrid("plgrpcode", "ASC")
                '    '  FillGrid("cplisthnew.plgrpcode", "edit_cplisthnew.plgrpcode", "ASC")
                'Case 6
                '    FillGrid("subseascode", "ASC")
                ' FillGrid("cplisthnew.subseascode", "edit_cplisthnew.plistcode", "ASC")

        End Select
    End Sub
End Class
