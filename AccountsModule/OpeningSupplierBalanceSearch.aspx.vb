'------------------------------------------------------------------------------------------------
'   Module Name    :    OpeningSupplierBalanceSearch 
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net

#End Region
Partial Class OpeningSupplierBalanceSearch
    Inherits System.Web.UI.Page
#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim NoDecRound As Decimal
    Dim strappid As String = ""
    Dim strappname As String = ""
    Dim document As New XLWorkbook
    Dim reportfilter As String
#End Region
#Region "Enum GridCol"
    Enum GridCol
        tranid = 2
        Edit = 20
        View = 21
        Delete = 22
    End Enum
#End Region
    '#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    '    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        pnlSearch.Visible = False
    '    End Sub
    '#End Region
    '#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    '    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        pnlSearch.Visible = True
    '    End Sub
    '#End Region
#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("OpeningSupplierBalance.aspx", False)
        Dim strpop As String = ""
        Dim actionstr As String = ""
        actionstr = "New"
        'strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType("", String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType("", String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        'strpop = "window.open('OpeningSupplierBalance.aspx?State=New','OpeningSupplierBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        NoDecRound = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)

        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)

        If appid Is Nothing = False Then
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select displayname from appmaster where appid='" & appid & "'")
        End If
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")

        ViewState.Add("divcode", divid)

        ''*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        'Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        ''*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        'ViewState.Add("divcode", divid)

    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        FilterGrid()

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try

                '                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Or CType(Session("AppName"), String) = Nothing Then
                '                    Response.Redirect("Login.aspx", False)
                '                    Exit Sub
                '                End If
                ViewState.Add("OpeningSupplierBalanceSearchOpenType", Request.QueryString("tran_type"))
                btnPrint_new.Attributes.Add("onclick", "return FormValidation('')")
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                Dim strappid As String = ""
                Dim strappname As String = ""

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                    strappname = Session("AppName")
                    '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                End If
                txtDivcode.Value = ViewState("divcode")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                Else
                    If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                           CType(strappname, String), "AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=S&appid=" + strappid, btnAddNew, btnPrint_new, _
                           btnPrint_new, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                    ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                           CType(strappname, String), "AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=A&appid=" + strappid, btnAddNew, btnPrint_new, _
                           btnPrint_new, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                    ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                           CType(strappname, String), "AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=C&appid=" + strappid, btnAddNew, btnPrint_new, _
                           btnPrint_new, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                    End If
                End If

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                Session.Add("OpenType", Request.QueryString("tran_type"))
                'pnlSearch.Visible = False
                'SetFocus(txtTranId)
                If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
                    Session.Add("tran_type", "OBS")
                    hdOPMode.Value = "S"
                    lblHeading.Text = "Opening Supplier Balance"
                    hdFillByGrid.Value = "S"
                    hdLinkButtonValue.Value = ""
                    'lblcode.Text = "Supplier Code"
                    'lblname.Text = "Supplier Name"
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Partycode", "Partyname", "select Partyname,Partycode from  partymast where active=1", True)
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppilerName, "Partyname", "Partycode", "select Partyname,Partycode from  partymast where active=1", True)
                ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
                    hdOPMode.Value = "C"
                    '    lblcode.Text = "Customer Code"
                    '    lblname.Text = "Customer Name"
                    lblHeading.Text = "Opening Customer Balance"
                    Session.Add("tran_type", "OBC")
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "agentcode", "agentname", "select agentname,agentcode from  agentmast where divcode='" & ViewState("divcode") & "' and active=1", True)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppilerName, "agentname", "agentcode", "select agentname,agentcode from  agentmast where divcode='" & ViewState("divcode") & "' and  active=1", True)
                ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
                    hdOPMode.Value = "A"
                    '    lblcode.Text = "Supplier Agent Code"
                    '    lblname.Text = "Supplier Agent Name"
                    lblHeading.Text = "Opening Supplier Agent Balance"
                    Session.Add("tran_type", "OBSA")
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "supagentcode", "supagentname", "select supagentname,supagentcode from  Supplier_agents where active=1", True)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppilerName, "supagentname", "supagentcode", "select supagentname,supagentcode from  Supplier_agents where active=1", True)
                End If
                'Dim dtDynamic = New DataTable()
                'Dim dcCode = New DataColumn("Code", GetType(String))
                'Dim dcEXCCLSCode = New DataColumn("EXCCLSCode", GetType(String))
                'dtDynamic.Columns.Add(dcCode)
                'dtDynamic.Columns.Add(dcEXCCLSCode)
                'Session("sDtDynamic") = dtDynamic
                ' --------end

                ' Create a Dynamic datatable ---- Start
                Session("sDtDynamicSearch") = Nothing
                Dim dtDynamicSearch = New DataTable()
                Dim dcSearchCode = New DataColumn("Code", GetType(String))
                Dim dcSearchValue = New DataColumn("Value", GetType(String))
                Dim dcSearchCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamicSearch.Columns.Add(dcSearchCode)
                dtDynamicSearch.Columns.Add(dcSearchValue)
                dtDynamicSearch.Columns.Add(dcSearchCodeAndValue)
                Session("sDtDynamicSearch") = dtDynamicSearch

                '' Create a Dynamic datatable ---- Start
                'Dim dtExcSuppClsDetails = New DataTable()
                ' ''   Dim dtHotelGroupDetails = New DataTable()
                'Dim dcExcSuppClsType = New DataColumn("Type", GetType(String))
                'Dim dcExcSuppClsTypeName = New DataColumn("TypeName", GetType(String))
                'Dim dcExcSuppClsCode = New DataColumn("Code", GetType(String))
                'Dim dcGroupDetailsEXCCLS = New DataColumn("EXCCLSCode", GetType(String))
                'dtExcSuppClsDetails.Columns.Add(dcExcSuppClsType)
                'dtExcSuppClsDetails.Columns.Add(dcExcSuppClsTypeName)
                'dtExcSuppClsDetails.Columns.Add(dcExcSuppClsCode)
                'dtExcSuppClsDetails.Columns.Add(dcGroupDetailsEXCCLS)
                'Session("sDtExcSuppClsDetails") = dtExcSuppClsDetails
                '--------end
                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)


                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrCode, "currcode", "currname", "select currname,currcode from  currmast where active=1", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrName, "currname", "currcode", "select currname,currcode from  currmast where active=1", True)

                'FillGrid("tran_id")
                'FillGridWithOrderByValues()

                'txtConvtRate.Attributes.Add("onkeypress", "return checkNumber(event)")

                ''Set Attribut For Supplier
                'ddlSupplierCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlSupplierCode.ClientID, String) + "','" + CType(ddlSuppilerName.ClientID, String) + "')")
                'ddlSuppilerName.Attributes.Add("onchange", "javascript:FillName('" + CType(ddlSupplierCode.ClientID, String) + "','" + CType(ddlSuppilerName.ClientID, String) + "')")
                ''Set Attribut For Supplier
                'ddlCurrCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlCurrCode.ClientID, String) + "','" + CType(ddlCurrName.ClientID, String) + "')")
                'ddlCurrName.Attributes.Add("onchange", "javascript:FillName('" + CType(ddlCurrCode.ClientID, String) + "','" + CType(ddlCurrName.ClientID, String) + "')")


                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlSuppilerName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlCurrCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlCurrName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OpeningSupplierBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            txtDivcode.Value = ViewState("divcode")
            If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
                hdOPMode.Value = "S"

            ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
                hdOPMode.Value = "C"

            ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
                hdOPMode.Value = "A"
            End If
            FillGridNew()
        End If
        ' ClientScript.GetPostBackEventReference(Me, String.Empty)
        'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OpeningSupplierBalanceWindowPostBack") Then
        '    btnSearch_Click(sender, e)
        'End If

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
            '    strSqlQry = " SELECT  *   FROM openparty_master where open_type='" & ViewState("OpeningSupplierBalanceSearchOpenType") & "'"

            strSqlQry = "select openparty_master.tran_id ,openparty_master.tran_type , view_account.des, " & _
                        " case isnull(openparty_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                        "convert(varchar(10),openparty_master.tran_date,103) as tran_date,openparty_master.open_type ,openparty_master.open_code , acctmast.acctname , " & _
                        "openparty_master.open_narration ,openparty_master.currcode  , convert(decimal(18,4) ,openparty_master.currrate) as currrate ,openparty_master.open_debit , " & _
                        "openparty_master.open_credit ,openparty_master.openbase_debit , openparty_master.openbase_credit ,openparty_master.adddate, " & _
                        "openparty_master.adduser ,openparty_master.moddate ,openparty_master.moduser   from openparty_master left join acctmast on   acctmast.div_code=openparty_master.div_id  and acctmast.acctcode=openparty_master.controlacctcode INNER JOIN  view_account " & _
                        "on view_account.type=openparty_master.open_type and view_account.code=openparty_master.open_code  and view_account.div_code=openparty_master.div_id where openparty_master.div_id='" & ViewState("divcode") & "' and  open_type='" & ViewState("OpeningSupplierBalanceSearchOpenType") & "'"


            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " and " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS
            Dim pagevaluecs = RowsPerPageCUS.SelectedValue
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
            objUtils.WritErrorLog("OpeningSupplierBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region
    '#Region "  Private Function BuildCondition() As String"
    '    Private Function BuildCondition() As String

    '        strWhereCond = ""
    '        If txtTranId.Text.Trim <> "" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " upper(openparty_master.tran_id) LIKE '" & Trim(txtTranId.Text.Trim.ToUpper) & "%'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(openparty_master.tran_id) LIKE '" & Trim(txtTranId.Text.Trim.ToUpper) & "%'"
    '            End If
    '        End If


    '        If ddlSuppilerName.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " openparty_master.open_code = '" & ddlSuppilerName.Value & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND openparty_master.open_code = '" & Trim(ddlSuppilerName.Value) & "'"
    '            End If
    '        End If

    '        If ddlCurrName.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " openparty_master.currcode = '" & ddlCurrName.Value & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND openparty_master.currcode = '" & Trim(ddlCurrName.Value) & "'"
    '            End If
    '        End If

    '        If txtConvtRate.Text.Trim <> "" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " openparty_master.currrate = " & Trim(txtConvtRate.Text.Trim) & ""
    '            Else
    '                strWhereCond = strWhereCond & " AND openparty_master.currrate = " & Trim(txtConvtRate.Text.Trim) & ""
    '            End If
    '        End If
    '        If ddlStatus.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " isnull(openparty_master.post_state,'')  = '" & ddlStatus.Value & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND isnull(openparty_master.post_state,'')  = '" & ddlStatus.Value & "'"
    '            End If
    '        End If

    '        BuildCondition = strWhereCond
    '    End Function

    '#End Region
    ''' <summary>
    ''' lbCountry_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdFillByGrid.Value = "Y"
        hdLinkButtonValue.Value = ""
        Dim strlbValue As String = ""

        Dim myButton As LinkButton = CType(sender, LinkButton)

        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
        Dim lb As Label = CType(dlItem.FindControl("lblType"), Label)

        If Not myButton Is Nothing Then
            strlbValue = myButton.Text
            If strlbValue = "EXCURSION" Then
                strlbValue = "%"
            End If

            hdLinkButtonValue.Value = strlbValue
            Try
                'FillGridByLinkButton()
                'FillCheckbox()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionClassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Finally


            End Try

        End If



    End Sub
    'Protected Sub lbCloseSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim myButton As Button = CType(sender, Button)
    '        Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
    '        Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
    '        Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)

    '        Dim dtDynamics As New DataTable
    '        dtDynamics = Session("sDtDynamicSearch")
    '        If dtDynamics.Rows.Count > 0 Then
    '            Dim j As Integer
    '            For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
    '                If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper Then
    '                    dtDynamics.Rows.Remove(dtDynamics.Rows(j))
    '                End If
    '            Next
    '        End If
    '        Session("sDtDynamicSearch") = dtDynamics
    '        dlListSearch.DataSource = dtDynamics
    '        dlListSearch.DataBind()
    '        FillGridNew()
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '    End Try

    'End Sub
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ":" & lsValue.Trim)
                Session("sDtDynamicSearch") = dtt
            End If
        End If
        Return True
    End Function
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamicSearch")
        Dim strCurrencyValue As String = ""
        Dim strdocumentvalue As String = ""
        Dim strsuppliervalue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strExcValue As String = ""

        Dim strExcClsValue As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Or dtt.Rows(i)("Code").ToString = "SUPPLIERAGENT" Or dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strsuppliervalue <> "" Then
                            strsuppliervalue = strsuppliervalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strsuppliervalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "OBSNo" Or dtt.Rows(i)("Code").ToString = "OBSANo" Or dtt.Rows(i)("Code").ToString = "OBCNo" Then
                        If strdocumentvalue <> "" Then
                            strdocumentvalue = strdocumentvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strdocumentvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CURRENCY" Then
                        If strCurrencyValue <> "" Then
                            strCurrencyValue = strCurrencyValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCurrencyValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Dim pagevaluecs = RowsPerPageCUS.SelectedValue
            strBindCondition = BuildConditionNew(strdocumentvalue, strsuppliervalue, strCurrencyValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "DESC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"
            strSqlQry = "select openparty_master.tran_id ,openparty_master.tran_type , view_account.des, " & _
                        " case isnull(openparty_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as post_state ," & _
                        "convert(varchar(10),openparty_master.tran_date,103) as tran_date,openparty_master.open_type ,openparty_master.open_code , acctmast.acctname , " & _
                        "openparty_master.open_narration ,openparty_master.currcode  , convert(decimal(18,4) ,openparty_master.currrate) as currrate ,openparty_master.open_debit , " & _
                        "openparty_master.open_credit ,openparty_master.openbase_debit , openparty_master.openbase_credit ,openparty_master.adddate, " & _
                        "openparty_master.adduser ,openparty_master.moddate ,openparty_master.moduser   from openparty_master  left join acctmast on   acctmast.div_code=openparty_master.div_id  and acctmast.acctcode=openparty_master.controlacctcode INNER JOIN  view_account " & _
                        "on view_account.type=openparty_master.open_type and view_account.code=openparty_master.open_code  and view_account.div_code=openparty_master.div_id left outer join currmast on " & _
                        " openparty_master.currcode= currmast.currcode  where openparty_master.div_id='" & ViewState("divcode") & "' and  open_type='" & ViewState("OpeningSupplierBalanceSearchOpenType") & "'"


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
            objUtils.WritErrorLog("BankTypeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Private Function BuildConditionNew(ByVal strDocumentvalue As String, strsuppliervalue As String, ByVal strCurrencyValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strDocumentvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(openparty_master. tran_id) IN (" & Trim(strDocumentvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(openparty_master. tran_id) IN (" & Trim(strDocumentvalue.Trim.ToUpper) & ")"
            End If
        End If

        If strsuppliervalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(view_account.des) IN (" & Trim(strsuppliervalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(view_account.des) IN (" & Trim(strsuppliervalue.Trim.ToUpper) & ")"
            End If
        End If
        If strCurrencyValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(currmast. currname) IN (" & Trim(strCurrencyValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(currmast. currname) IN (" & Trim(strCurrencyValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "upper(view_account.des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(openparty_master.tran_type) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(openparty_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(view_account.des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(openparty_master.tran_type) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(openparty_master.post_state) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  ) "
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

            'If ddlOrder.SelectedValue = "C" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " (CONVERT(datetime, convert(varchar(10),openparty_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            Else
                strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),openparty_master.tran_date,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            End If
            'ElseIf ddlOrder.SelectedValue = "M" Then
            '    If Trim(strWhereCond) = "" Then

            '        strWhereCond = " (CONVERT(datetime, convert(varchar(10),bank_master_type.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            '    Else
            '        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),bank_master_type.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            '    End If
            'End If
        End If


        BuildConditionNew = strWhereCond
    End Function
    'Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try

    '        Dim dtsExcSuppClsDetails As New DataTable
    '        dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")

    '        'Dim dtsCountryGroupDetailsNew As New DataTable
    '        'dtsCountryGroupDetailsNew = Session("sDtHotelGroupDetails")
    '        'dtsCountryGroupDetailsNew.Clear()

    '        Dim myButton As LinkButton = CType(sender, LinkButton)
    '        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
    '        Dim lb As LinkButton = CType(dlItem.FindControl("lbCountry"), LinkButton)

    '        If dtsExcSuppClsDetails.Rows.Count > 0 Then

    '            Dim i As Integer
    '            For i = dtsExcSuppClsDetails.Rows.Count - 1 To 0 Step i - 1
    '                If lb.Text.Trim = dtsExcSuppClsDetails.Rows(i)("TypeName").ToString.Trim Then
    '                    dtsExcSuppClsDetails.Rows.Remove(dtsExcSuppClsDetails.Rows(i))
    '                End If
    '                ' dtsCountryGroupDetails.Rows(i).Delete()
    '                dtsExcSuppClsDetails.AcceptChanges()
    '            Next
    '        End If
    '        Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails

    '        Dim dtDynamics As New DataTable
    '        dtDynamics = Session("sDtDynamic")

    '        If dtDynamics.Rows.Count > 0 Then
    '            Dim j As Integer
    '            For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
    '                ' For j As Integer = 0 To dtDynamics.Rows.Count - 1
    '                If lb.Text.Trim = dtDynamics.Rows(j)("EXCCLSCode").ToString.Trim Then
    '                    dtDynamics.Rows.Remove(dtDynamics.Rows(j))
    '                End If
    '            Next

    '        End If

    '        Session("sDtDynamic") = dtDynamics
    '        dlList.DataSource = dtDynamics
    '        dlList.DataBind()



    '        '' Create a Dynamic datatable ---- Start
    '        Dim ClearDataTable = New DataTable()
    '        Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
    '        Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
    '        Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
    '        Dim dcGroupDetailsEXCCLS = New DataColumn("EXCCLSCode", GetType(String))
    '        ClearDataTable.Columns.Add(dcGroupDetailsType)
    '        ClearDataTable.Columns.Add(dcGroupDetailsTypeName)
    '        ClearDataTable.Columns.Add(dcGroupDetailsCode)
    '        ClearDataTable.Columns.Add(dcGroupDetailsEXCCLS)
    '        gv_SearchResult.DataSource = ClearDataTable
    '        gv_SearchResult.DataBind()

    '    Catch ex As Exception

    '    End Try


    'End Sub
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessDocument As String = ""
        Dim lsProcessCurrency As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()

        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        'If hdOPMode.Value = "S" Then

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "SUPPLIER"
                    lsProcessCurrency = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIER", lsProcessCurrency, "S")
                Case "SUPPLIERAGENT"
                    lsProcessCurrency = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIERAGENT", lsProcessCurrency, "A")
                Case "CUSTOMER"
                    lsProcessCurrency = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessCurrency, "C")
                Case "OBSNo"
                    lsProcessCurrency = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("OBSNo", lsProcessCurrency, "D")

                Case "CURRENCY"
                    lsProcessCurrency = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CURRENCY", lsProcessCurrency, "C")
                Case "OBCNo"
                    lsProcessDocument = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("OBCNo", lsProcessDocument, "D")
                Case "OBSANo"
                    lsProcessDocument = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("OBSANo", lsProcessDocument, "D")

                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")

            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamicSearch")
        dlList.DataSource = dttDyn
        dlList.DataBind()
        FillGridNew()
        ' End If


        'Bind Gird based selection 

    End Sub
#Region "  Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            Dim strpop As String = ""
            Dim actionstr As String
            actionstr = ""

            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblTranID")
            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OpeningSupplierBalance.aspx", False)

                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If

                actionstr = "Edit"
                'strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OpeningSupplierBalance.aspx", False)
                actionstr = "View"
                'strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OpeningSupplierBalance.aspx", False)
                If Validateseal(lblId.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If


                actionstr = "Delete"
                'strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OpeningSupplierBalance.aspx?State=" + CType(actionstr, String) + "&divid=" & ViewState("divcode") & "&RefCode=" + CType(lblId.Text.Trim, String) + "&OpenType=" + CType(ViewState("OpeningSupplierBalanceSearchOpenType"), String) + "','OpeningSupplierBalance');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OpeningSupplierBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            dtDynamics = Session("sDtDynamicSearch")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicSearch") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  openparty_master where div_id='" & ViewState("divcode") & "' and tran_id='" + tranid + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("open_tran_state")) = False Then
                        If ds.Tables(0).Rows(0)("open_tran_state") = "S" Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
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
#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'FillGrid("tran_id")
        ' FillGridWithOrderByValues()
    End Sub
#End Region
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("tran_id", "DESC")
        'FillGridWithOrderByValues()
    End Sub
#End Region

    '#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    '    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
    '        Dim DS As New DataSet
    '        Dim DA As SqlDataAdapter
    '        Dim con As SqlConnection
    '        Dim objcon As New clsDBConnect
    '        Try
    '            If gv_SearchResult.Rows.Count <> 0 Then

    '                strSqlQry = "select openparty_master.tran_id as [Document No. ], " & _
    '                             " case isnull(openparty_master.post_state,'') when 'P' then 'Posted' when 'U' then 'UnPosted' end  as Status ," & _
    '                             " openparty_master.tran_type as [Transaction Type], " & _
    '                            "convert(varchar(10),openparty_master.tran_date,103) as [Transaction Date],openparty_master.open_type as [Type],view_account.des as [Name],acctmast.acctname as [Control A/c], " & _
    '                            "openparty_master.open_narration as [Narration],openparty_master.currcode as [Currency],convert(decimal(18,4) ,openparty_master.currrate) as [Conversion Rate],openparty_master.open_debit as [Debit], " & _
    '                            "openparty_master.open_credit as [Credit],openparty_master.openbase_debit as [Base Debit], openparty_master.openbase_credit as [Base Credit],openparty_master.adddate as [Date Created], " & _
    '                            "openparty_master.adduser as [User Created],openparty_master.moddate as [Date Modified],openparty_master.moduser as [User Modified]  from openparty_master left join acctmast on   acctmast.div_code=openparty_master.div_id  and acctmast.acctcode=openparty_master.controlacctcode INNER JOIN  view_account " & _
    '                            "on view_account.type=openparty_master.open_type and view_account.code=openparty_master.open_code and view_account.div_code=openparty_master.div_id where openparty_master.div_id='" & ViewState("divcode") & "' and open_type='" & ViewState("OpeningSupplierBalanceSearchOpenType") & "'"

    '                'If BuildCondition() <> "" Then
    '                '    strSqlQry = strSqlQry & " and " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
    '                'Else
    '                '    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
    '                'End If
    '                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '                DA = New SqlDataAdapter(strSqlQry, con)
    '                DA.Fill(DS, "cancellation")

    '                objUtils.ExportToExcel(DS, Response)
    '                con.Close()
    '            Else
    '                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
    '            End If
    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        End Try
    '    End Sub
    '#End Region

    'Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    txtTranId.Text = ""
    '    ddlCurrCode.Value = "[Select]"
    '    ddlCurrName.Value = "[Select]"
    '    ddlSuppilerName.Value = "[Select]"
    '    ddlSupplierCode.Value = "[Select]"
    '    txtConvtRate.Text = ""
    '    ddlStatus.Value = "[Select]"
    '    'FillGrid("tran_id")
    '    ddlOrderBy.SelectedIndex = 0
    '    FillGridWithOrderByValues()
    'End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Try
    '        Dim strReportTitle As String = ""
    '        Dim strSelectionFormula As String = ""
    '        Dim BackPage As String = ""
    '        Dim open_type As String = ""
    '        'Session.Add("Pageame", "SupplierOpeningTrailBalance")
    '        'Session("ColReportParams") = Nothing
    '        'If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
    '        '    session.Add("BackPageName", "~\AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=S")
    '        'ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
    '        '    session.Add("BackPageName", "~\AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=C")
    '        'ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
    '        '    session.Add("BackPageName", "~\AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=A")
    '        'End If
    '        If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
    '            ViewState.Add("BackPageName", "~\AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=S")
    '            BackPage = "OpeningSupplierBalanceSearch.aspx?tran_type=S"
    '            open_type = "S"
    '        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
    '            ViewState.Add("BackPageName", "~\AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=C")
    '            BackPage = "OpeningSupplierBalanceSearch.aspx?tran_type=C"
    '            open_type = "C"
    '        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
    '            ViewState.Add("BackPageName", "~\AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=A")
    '            BackPage = "OpeningSupplierBalanceSearch.aspx?tran_type=A"
    '            open_type = "A"
    '        End If
    '        If open_type = "S" Then
    '            Dim strpop As String = ""
    '            'strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & BackPage & "&opentype=" & open_type & "&TranID=" & txtTranId.Text.Trim & "&Supplier=" & Trim(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text) & "&Curr=" & Trim(ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text) & "&ConvRate=" & txtConvtRate.Text.Trim & " ','OpeningSupBal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '            strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & BackPage & "&divid=" & ViewState("divcode") & "&opentype=" & open_type & " ','OpeningSupBal');"
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    '        ElseIf open_type = "C" Then
    '            Dim strpop As String = ""
    '            'strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & BackPage & "&opentype=" & open_type & "&TranID=" & txtTranId.Text.Trim & "&Supplier=" & Trim(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text) & "&Curr=" & Trim(ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text) & "&ConvRate=" & txtConvtRate.Text.Trim & " ','OpeningCustBal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '            strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & BackPage & "&divid=" & ViewState("divcode") & "&opentype=" & open_type & "&TranID=" & "','OpeningCustBal');"
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    '        ElseIf open_type = "A" Then
    '            Dim strpop As String = ""
    '            'strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & BackPage & "&opentype=" & open_type & "&TranID=" & txtTranId.Text.Trim & "&Supplier=" & Trim(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text) & "&Curr=" & Trim(ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text) & "&ConvRate=" & txtConvtRate.Text.Trim & " ','OpeningSupAgentBal','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '            strpop = "window.open('rptReportNew.aspx?Pageame=SupplierOpeningTrailBalance&BackPageName=" & BackPage & "&divid=" & ViewState("divcode") & "&opentype=" & open_type & " ','OpeningSupAgentBal');"
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    '        End If
    '        'Session("OpenType") = ViewState("OpeningSupplierBalanceSearchOpenType") ' Time being this  session included


    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("OpeningSupplierBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try
    'End Sub
    ''Private Sub FillGridWithOrderByValues()
    '    Select Case ddlOrderBy.SelectedIndex
    '        Case 0
    '            FillGrid("view_account.des", "ASC")
    '        Case 1
    '            FillGrid("openparty_master.tran_id", "DESC")
    '        Case 2
    '            FillGrid("openparty_master.tran_id", "ASC")

    '    End Select
    'End Sub
    'Private Function ExportWithOrderByValues() As String
    '    ExportWithOrderByValues = ""
    '    Select Case ddlOrderBy.SelectedIndex
    '        Case 0
    '            ExportWithOrderByValues = "view_account.des ASC"
    '        Case 1
    '            ExportWithOrderByValues = "openparty_master.tran_id DESC"
    '        Case 2
    '            ExportWithOrderByValues = "openparty_master.tran_id ASC"

    '    End Select
    'End Function

    'Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
    '    FillGridWithOrderByValues()
    'End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OpeningSupplierBalanceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OpeningCustomerBalanceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OpeningAgentBalanceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
    End Sub
    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
       


       
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
            objUtils.WritErrorLog("OpeningSupplierBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound


        If e.Row.RowType = DataControlRowType.Header Then

            If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
                e.Row.Cells(GridCol.tranid).Text = "OBS No"
            ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
                e.Row.Cells(GridCol.tranid).Text = "OBC No"
            ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
                e.Row.Cells(GridCol.tranid).Text = "OBSA No"

            End If

            Exit Sub
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lbltransid As Label = e.Row.FindControl("lbltransid")
            Dim lbldes As Label = e.Row.FindControl("lbldes")
            'Dim lblcurrency As Label = e.Row.FindControl("lblcurrency")

            Dim lsCurrencyName As String = ""
            Dim lsSupplierName As String = ""
            Dim lsDocumentNo As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicSearch")
            If Session("sDtDynamicSearch") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsCurrencyName = ""

                        If "OBSNo" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsDocumentNo = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "OBSANo" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsDocumentNo = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "OBCNo" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsDocumentNo = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SUPPLIER" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSupplierName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SUPPLIERAGENT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSupplierName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CUSTOMER" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSupplierName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CURRENCY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCurrencyName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCurrencyName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsDocumentNo.Trim <> "" Then
                            lbltransid.Text = Regex.Replace(lbltransid.Text.Trim, lsDocumentNo.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSupplierName.Trim <> "" Then
                            lbldes.Text = Regex.Replace(lbldes.Text.Trim, lsSupplierName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        'If lsCurrencyName.Trim <> "" Then
                        '    lblcurrency.Text = Regex.Replace(lblcurrency.Text.Trim, lsCurrencyName.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '                    RegexOptions.IgnoreCase)
                        'End If
                    Next
                End If
            End If



        End If
    End Sub

    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamicSearch") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()
    End Sub

    Private Sub detailedReport()
        Dim FolderPath As String = "..\ExcelTemplates\"
        Dim FileName As String = "accountsTransactionDetailed_template.xlsx"
        Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        Dim RandomCls As New Random()
        Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
        Dim rptcompanyname, basecurrency As String

        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtdivcode.Value & "'"), String)
        basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

     Dim FileNameNew As String

        If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
            FileNameNew = "OpeningBalanceSuppliersDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
            FileNameNew = "OpeningBalanceSupplierAgentsDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
            FileNameNew = "OpeningBalanceCustomersDetailed_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
        End If
        document = New XLWorkbook(FilePath)
        Dim ws As IXLWorksheet = document.Worksheet("register")
        ws.Style.Font.FontName = "arial"

        'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
        'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
        'Dim PartyName As String = ""
        'Dim CatName As String = ""
        'Dim SectorCityName As String = ""

        Dim LastLine As Integer
        ws.Cell(1, 1).Value = rptcompanyname
        ws.Column(14).Delete()
        ws.Column(14).Delete()

        ws.Column(4).Width = 13.5
        ws.Column(6).Width = 30
        ws.Column(13).Width = 57.3

       If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
            ws.Cell(2, 1).Value = "Opening Balance Suppliers:Detailed Report"
            ws.Cell(6, 1).Value = "OBSNo"
            ws.Cell(6, 3).Value = "Supplier"
        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
            ws.Cell(2, 1).Value = "Opening Balance Supplier Agents:Detailed Report"
            ws.Cell(6, 1).Value = "OBSANo"
            ws.Cell(6, 3).Value = "Supplier Agents"
        ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
            ws.Cell(2, 1).Value = "Opening Balance Customers:Detailed Report"
            ws.Cell(6, 1).Value = "OBCNo"
            ws.Cell(6, 3).Value = "Customers"
        End If


        'create header

        ws.Cell(6, 2).Value = "Date"
        ws.Cell(6, 4).Value = "BillType"

        ws.Cell(6, 5).Value = "DueDate"
        ws.Cell(6, 6).Value = "DocumentNo"
        ws.Cell(6, 7).Value = "Currency"
        ws.Cell(6, 8).Value = "Rate"
        ws.Cell(6, 9).Value = "Debit"
        ws.Cell(6, 10).Value = "Credit"
        ws.Cell(6, 11).Value = "Debit"
        ws.Cell(6, 11).Value = ws.Cell(6, 11).Value & " (" & basecurrency & ")"
        ws.Cell(6, 12).Value = "Credit"
        ws.Cell(6, 12).Value = ws.Cell(6, 12).Value & " (" & basecurrency & ")"
     
        ws.Cell(6, 13).Value = "Narration"


        Dim sql As String

        sql = FillGridNew_report()


        Dim myDS As New DataSet

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(sql, SqlConn)
        myDataAdapter.Fill(myDS)



        Dim dt As New DataTable

        Dim dt_row As New DataTable

        dt = myDS.Tables(0)
        If dt.Rows.Count > 0 Then
            ws.Cell(4, 1).Value = "Report Filter: As On Date: " & dt.Rows(0).Item(1).ToString & "  " & reportfilter
        End If


        'dt_sum = ds.Tables(1)
        LastLine = 7
        Dim total_basedebit, total_basecredit As Double
        'total = Convert.ToDouble(dt.Compute("SUM(receipt_credit_base)", String.Empty))

        total_basedebit = 0
        total_basecredit = 0
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1


                dt_row = dt.Clone()
                dt_row.ImportRow(dt.Rows(i))




                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(LastLine, 1).InsertTable(dt_row.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()
                RateSheet.Style.Font.FontName = "arial"
                RateSheet.Columns("8:12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin
                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                RateSheet.Style.Font.Bold = True

                LastLine = LastLine + 1


                Dim dt_detail_ds As New DataSet
                Dim dt_detail As New DataTable
                Dim sqlst As String
                Dim id As String = dt.Rows(i).Item(0).ToString



                sqlst = "select '',convert(varchar(10),tran_date,103),'',against_bill_type,convert(varchar(10),open_due_date,103),against_tran_id,'','',open_debit,open_credit,open_base_debit,open_base_credit ,''from openparty_detail" & _
                        "  where div_id='" & txtDivcode.Value.Trim & "' and tran_type='" & Session("tran_type") & "' and tran_id='" & id & "'"
                dt_detail_ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlst)
                dt_detail = dt_detail_ds.Tables(0)

                Dim RateSheet_detail As IXLRange
                RateSheet_detail = ws.Cell(LastLine, 1).InsertTable(dt_detail.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(LastLine).Clear()
                ws.Rows(LastLine).Delete()

                RateSheet_detail.Style.Font.FontName = "arial"
                'RateSheet_detail.Columns("5").SetDataType(XLCellValues.Text)
                'RateSheet_detail.Column("2").SetDataType(XLCellValues.Text)

                RateSheet_detail.Columns("8:12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style3 As IXLBorder = RateSheet_detail.Cells.Style.Border
                style3.BottomBorder = XLBorderStyleValues.Thin
                style3.LeftBorder = XLBorderStyleValues.Thin
                RateSheet_detail.Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                total_basedebit = total_basedebit + Convert.ToDouble(dt_detail.Compute("SUM(open_base_debit)", String.Empty))
                total_basecredit = total_basecredit + Convert.ToDouble(dt_detail.Compute("SUM(open_base_credit)", String.Empty))

                LastLine = LastLine + dt_detail.Rows.Count
                LastLine = LastLine + 1

            Next



            'Dim RateSheet_summ As IXLRange
            'RateSheet_summ = ws.Range(LastLine, 1, LastLine, 15)

            ws.Cell(LastLine, 10).Value = "Total"
            ws.Cell(LastLine, 10).Style.Font.Bold = True
            ws.Cell(LastLine, 10).Style.Font.FontSize = 14
            ws.Cell(LastLine, 10).Style.Font.FontName = "arial"

            ws.Cell(LastLine, 11).Value = total_basedebit
            ws.Cell(LastLine, 11).Style.Font.Bold = True
            ws.Cell(LastLine, 11).Style.Font.FontSize = 14
            ws.Cell(LastLine, 11).Style.Font.FontName = "arial"

            ws.Cell(LastLine, 12).Value = total_basecredit
            ws.Cell(LastLine, 12).Style.Font.Bold = True
            ws.Cell(LastLine, 12).Style.Font.FontSize = 14
            ws.Cell(LastLine, 12).Style.Font.FontName = "arial"
            'ws.Cell(LastLine, 14).Value = total
            'ws.Cell(LastLine, 14).Style.Font.Bold = True
            'ws.Cell(LastLine, 14).Style.Font.FontSize = 14

            'RateSheet_summ.Style.Font.FontName = "arial"
            'RateSheet_summ.Columns("9:14").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
            'Dim style2 As IXLBorder = RateSheet_summ.Cells.Style.Border
            'style2.BottomBorder = XLBorderStyleValues.Thin
            'style2.LeftBorder = XLBorderStyleValues.Thin
            'RateSheet_summ.Style.Border.OutsideBorder = XLBorderStyleValues.Thin

            '    RateSheet_summ.Style.Font.Bold = True




        End If










        'ws.Protect(RandomNo)
        'ws.Protection.FormatColumns = True
        'ws.Protection.FormatRows = True

        Using MyMemoryStream As New MemoryStream()
            document.SaveAs(MyMemoryStream)
            document.Dispose()
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
            Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.BinaryWrite(MyMemoryStream.GetBuffer())
            MyMemoryStream.WriteTo(Response.OutputStream)
            Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End Using
    End Sub

    Protected Sub btnPrint_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint_new.Click

        Try
            If ddlrpt.SelectedValue = "Detailed" Then
                detailedReport()

            Else



                Dim FolderPath As String = "..\ExcelTemplates\"
                Dim FileName As String = "accountsTransaction_template.xlsx"
                Dim FilePath As String = Server.MapPath(FolderPath + FileName)
                Dim RandomCls As New Random()
                Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
                Dim rptcompanyname, basecurrency As String

                rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtDivcode.Value & "'"), String)
                basecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
                Dim FileNameNew As String

                If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
                    FileNameNew = "OpeningBalanceSuppliersRegisterBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
                    FileNameNew = "OpeningBalanceSupplierAgentsRegisterBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
                    FileNameNew = "OpeningBalancecustomersRegisterBrief_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                End If

                document = New XLWorkbook(FilePath)
                Dim ws As IXLWorksheet = document.Worksheet("register")
                ws.Style.Font.FontName = "arial"

                'Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
                'SheetTemplate.Style.Font.FontName = "Trebuchet MS"
                'Dim PartyName As String = ""
                'Dim CatName As String = ""
                'Dim SectorCityName As String = ""

                Dim LastLine As Integer
                ws.Cell(1, 1).Value = rptcompanyname

                If ViewState("OpeningSupplierBalanceSearchOpenType") = "S" Then
                    ws.Cell(2, 1).Value = "Opening Balance Suppliers:Brief Report"
                    ws.Cell(6, 1).Value = "OBSNo"
                    ws.Cell(6, 3).Value = "Supplier"
                ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "A" Then
                    ws.Cell(2, 1).Value = "Opening Balance Supplier Agents:Brief Report"
                    ws.Cell(6, 1).Value = "OBSANo"
                    ws.Cell(6, 3).Value = "Supplier Agents"
                ElseIf ViewState("OpeningSupplierBalanceSearchOpenType") = "C" Then
                    ws.Cell(2, 1).Value = "Opening Balance Customers:Brief Report"
                    ws.Cell(6, 1).Value = "OBCNo"
                    ws.Cell(6, 3).Value = "Customers"
                End If

               




                'create header

                ws.Cell(6, 2).Value = "Date"

                ws.Cell(6, 4).Value = "Currency"
                ws.Cell(6, 5).Value = "ConversionRate"
                ws.Cell(6, 6).Value = "Debit"
                ws.Cell(6, 7).Value = "Credit"
                ws.Cell(6, 8).Value = "Debit"
                ws.Cell(6, 8).Value = ws.Cell(6, 8).Value & " (" & basecurrency & ")"
                ws.Cell(6, 9).Value = "Credit"
                ws.Cell(6, 9).Value = ws.Cell(6, 9).Value & " (" & basecurrency & ")"
                ws.Cell(6, 10).Value = "Narration"


                Dim sql As String

                sql = FillGridNew_report()




                Dim myDS As New DataSet

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(sql, SqlConn)
                myDataAdapter.Fill(myDS)
                


                Dim dt As New DataTable



                dt = myDS.Tables(0)
                If dt.Rows.Count > 0 Then
                    ws.Cell(4, 1).Value = "Report Filter: As On Date: " & dt.Rows(0).Item(1).ToString & "  " & reportfilter
                End If

                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(7, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(7).Clear()
                ws.Rows(7).Delete()
                LastLine = 7 + dt.Rows.Count

                If dt.Rows.Count > 1 Then
                    ws.Range(LastLine, 1, LastLine, 10).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 10).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 10).Style.Font.Bold = True
                    ws.Cell(LastLine, 7).Value = "Total"
                    ws.Cell(LastLine, 7).Style.Font.Bold = True
                    RateSheet.Columns("5:9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                    RateSheet.Columns("5:9").SetDataType(XLCellValues.Number)
                    ws.Cell(LastLine, 8).SetFormulaR1C1("=SUM(h7:h" & LastLine - 1 & ")")
                    ws.Cell(LastLine, 8).Style.Font.FontName = "arial"
                    ws.Cell(LastLine, 9).SetFormulaR1C1("=SUM(i7:i" & LastLine - 1 & ")")
                    ws.Cell(LastLine, 9).Style.Font.FontName = "arial"

                End If



                RateSheet.Style.Font.FontName = "arial"

                'RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin

                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium










                'ws.Protect(RandomNo)
                'ws.Protection.FormatColumns = True
                'ws.Protection.FormatRows = True

                Using MyMemoryStream As New MemoryStream()
                    document.SaveAs(MyMemoryStream)
                    document.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    'Response.BinaryWrite(MyMemoryStream.GetBuffer())
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            End If
        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub



    Private Function FillGridNew_report() As String


        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamicSearch")
        Dim strCurrencyValue As String = ""
        Dim strdocumentvalue As String = ""
        Dim strsuppliervalue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strExcValue As String = ""

        Dim strExcClsValue As String = ""
        Dim reportfilterid, reportfilterText, reportfiltertype, reportfiltercurrency As String

        reportfilter = ""

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Or dtt.Rows(i)("Code").ToString = "SUPPLIERAGENT" Or dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strsuppliervalue <> "" Then
                            strsuppliervalue = strsuppliervalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strsuppliervalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfiltertype = dtt.Rows(i)("Code").ToString + ": " + strsuppliervalue
                        reportfilter = reportfilter & " " & reportfiltertype
                    End If
                    If dtt.Rows(i)("Code").ToString = "OBSNo" Or dtt.Rows(i)("Code").ToString = "OBSANo" Or dtt.Rows(i)("Code").ToString = "OBCNo" Then
                        If strdocumentvalue <> "" Then
                            strdocumentvalue = strdocumentvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strdocumentvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfilterid = dtt.Rows(i)("Code").ToString + " : " + strdocumentvalue
                        reportfilter = reportfilter & " " & reportfilterid
                    End If
                    If dtt.Rows(i)("Code").ToString = "CURRENCY" Then
                        If strCurrencyValue <> "" Then
                            strCurrencyValue = strCurrencyValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCurrencyValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                        reportfiltercurrency = dtt.Rows(i)("Code").ToString + " :" + strCurrencyValue
                        reportfilter = reportfilter & " " & reportfiltercurrency
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                        reportfilterText = dtt.Rows(i)("Code").ToString + "like " + strTextValue
                        reportfilter = reportfilter & " " & reportfilterText
                    End If
                Next
            End If
            strBindCondition = BuildConditionNew(strdocumentvalue, strsuppliervalue, strCurrencyValue, strTextValue)
            'customer_bank_master.other_bank_master_des, 
             strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "DESC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"
            If ddlrpt.SelectedValue = "Detailed" Then
                strSqlQry = "select openparty_master.tran_id ,convert(varchar(10),openparty_master.tran_date,103) as tran_date , view_account.des,'','','',openparty_master.currcode, convert(decimal(18,4) ,openparty_master.currrate) as currrate " & _
                       ",openparty_master.open_debit,openparty_master.open_credit,openparty_master.openbase_debit ,openparty_master.openbase_credit,openparty_master.open_narration  " & _
                          "  from openparty_master  left join acctmast on   acctmast.div_code=openparty_master.div_id  and acctmast.acctcode=openparty_master.controlacctcode INNER JOIN  view_account " & _
                      "on view_account.type=openparty_master.open_type and view_account.code=openparty_master.open_code  and view_account.div_code=openparty_master.div_id left outer join currmast on " & _
                      " openparty_master.currcode= currmast.currcode  where openparty_master.div_id='" & ViewState("divcode") & "' and  open_type='" & ViewState("OpeningSupplierBalanceSearchOpenType") & "'"


            Else
                strSqlQry = "select openparty_master.tran_id ,convert(varchar(10),openparty_master.tran_date,103) as tran_date , view_account.des,openparty_master.currcode, convert(decimal(18,4) ,openparty_master.currrate) as currrate " & _
                                       ",openparty_master.open_debit,openparty_master.open_credit,openparty_master.openbase_debit ,openparty_master.openbase_credit,openparty_master.open_narration  " & _
                                          "  from openparty_master  left join acctmast on   acctmast.div_code=openparty_master.div_id  and acctmast.acctcode=openparty_master.controlacctcode INNER JOIN  view_account " & _
                                      "on view_account.type=openparty_master.open_type and view_account.code=openparty_master.open_code  and view_account.div_code=openparty_master.div_id left outer join currmast on " & _
                                      " openparty_master.currcode= currmast.currcode  where openparty_master.div_id='" & ViewState("divcode") & "' and  open_type='" & ViewState("OpeningSupplierBalanceSearchOpenType") & "'"


            End If
          
            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If


           


            FillGridNew_report = strSqlQry

        Catch ex As Exception
            FillGridNew_report = ""
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesfreeformSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Function

End Class
