'------------================--------------=======================------------------================
'   Module Name    :    RptBillStatusQuery.aspx
'   Developer Name :    Veera Raju
'   Date           :    04/12/2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
#End Region

Partial Class RptBillStatusQuery
    Inherits System.Web.UI.Page

#Region "Enum GridCol"
    Enum GridCol
        DocNoCol = 0
        DocNo = 1
        DocType = 2
        status = 3
        Type = 4
        FDate = 5
        Code = 6
        Name = 7
        Amount = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12
        Edit = 13
        View = 14
        Delete = 15
        Copy = 16
    End Enum
#End Region

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objectcl As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim SqlConn1 As SqlConnection
    Dim myDataAdapter As SqlDataAdapter
    Dim myDataAdapter1 As SqlDataAdapter
    Dim strappid As String = ""
    Dim strappname As String = ""
#End Region


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("RptBillStatusQuery_Type", Request.QueryString("type"))
        Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim appidnew As String = CType(Request.QueryString("appid"), String)
        If Request.QueryString("type") = "S" Then
            txtcustomercode.Visible = False
            txtcustomername.Visible = False
            Label5.Visible = False
        Else
            txtsuppliername.Visible = False
            txtsuppliercode.Visible = False
            lblsuptype.Visible = False
        End If

        If appidnew Is Nothing = False Then
            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
            strappname = Session("AppName")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        End If
        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        ViewState.Add("divcode", divid)
        Session.Add("suptype", "S")
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")
                imgicon.Style("visibility") = "hidden"
                SetFocus(txtFromDate)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    'objUser.CheckUserRight(Session("dbconnectionName"),CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                                   CType(Session("AppName"), String), "AccountsModule\RptBillStatusQuery.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String) & "", btnAddNew, btnExportToExcel, _
                    '                                   btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                If ViewState("RptBillStatusQuery_Type") = "C" Then
                    lblHeading.Text = "Customer Bill Status Query"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "code", "des", "select 'Customer' as code, 'C' des", True)
                    ddlType.Value = "C"
                ElseIf ViewState("RptBillStatusQuery_Type") = "S" Then
                    lblHeading.Text = "Supplier Bill Status Query"
                    strSqlQry = " select 'Supplier' as code, 'S' des union select 'Supplier Agent' as code, 'A' des "
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "code", "des", strSqlQry, True)
                    ddlType.Value = "S"
                End If
              
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomer, "code", "des", "select code,des from view_account where type='" & ddlType.Value & "' order by code", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCustomerName, "des", "code", "select des,code from view_accountwhere type='" & ddlType.Value & "'  order by des", True)
                'lblCustCode.Text = ddlType.Items(ddlType.SelectedIndex).Text + " Code"
                'lblCustName.Text = ddlType.Items(ddlType.SelectedIndex).Text + " Name"


                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)
                If txtFromDate.Text = "" Then
                    txtFromDate.Text = Format(objectcl.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                End If
                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If




                btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
                'ddlType.Attributes.Add("onchange", "javascript:FillCustDDL('" + CType(ddlType.ClientID, String) + "','" + CType(ddlCustomer.ClientID, String) + "','" + CType(ddlCustomerName.ClientID, String) + "','" + CType(lblCustCode.ClientID, String) + "','" + CType(lblCustName.ClientID, String) + "')")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        Dim typ As Type
        typ = GetType(DropDownList)
        btnDisplayBill.Attributes.Add("onclick", "return validatePage()")
        btnDisplaySettle.Attributes.Add("onclick", "return validatePage()")
        ' btnSelBillDislplaySettle.Attributes.Add("onclick", "return validatePage()")
        'btnSelSettleDisplayBill.Attributes.Add("onclick", "return validatePage()")

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

           
            ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
    End Sub


    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getcustomer(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customername As New List(Of String)
        Try

            strSqlQry = "select agentname,agentcode from agentmast where agentname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    customername.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return customername
        Catch ex As Exception
            Return customername
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplierlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliernames As New List(Of String)
        Dim divid As String = ""
        Try
            If Convert.ToString(HttpContext.Current.Session("suptype").ToString()) = "S" Then
                strSqlQry = "select partyname,partycode from partymast where partyname like  '" & Trim(prefixText) & "%' order by partycode "
            ElseIf Convert.ToString(HttpContext.Current.Session("suptype").ToString()) = "A" Then
                strSqlQry = "select supagentcode as partycode, supagentname as partyname from supplier_agents where active=1 and supagentname like  '" & Trim(prefixText) & "%' order by supagentcode "
            End If
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliernames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))

                Next
            End If
            Return suppliernames
        Catch ex As Exception
            Return suppliernames
        End Try

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RptBillStatusQueryWindowPostBack") Then

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        Try
            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                'SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If


            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                    'SetFocus(txtToDate)
                    ValidatePage = False
                    Exit Function
                End If
            End If
            If ddlType.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Type');", True)
                ValidatePage = False
                Exit Function
            End If
            If Request.QueryString("type") = "S" Then
                If txtsuppliercode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Supplier Name ');", True)
                    ValidatePage = False
                    Exit Function
                End If
            Else
                If txtcustomername.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Customer Name ');", True)
                    ValidatePage = False
                    Exit Function
                End If
            End If
    


            frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=508")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                        If frmdate < ds.Tables(0).Rows(0)("option_selected") Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date cannot enter below the " & ds.Tables(0).Rows(0)("option_selected") & " ' );", True)
                            ValidatePage = False
                            Exit Function
                        End If
                    End If
                End If
            End If

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGridBill(ByVal strorderby As String, Optional ByVal FromSettle As Integer = 1, Optional ByVal tranid As String = "", Optional ByVal trantype As String = "", Optional ByVal tranlineno As String = "", Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim divid As String
        divid = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

        gv_BillSearchResult.Visible = True
        'pnlBills.Visible = True
        'btnSelBillDislplaySettle.Visible = True
        lblMsg.Visible = False

        If gv_BillSearchResult.PageIndex < 0 Then
            gv_BillSearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If FromSettle = 1 Then
                    strSqlQry = " sp_display_bills  '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "' , " _
                    & " '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "','" & ddlType.Value & "','" & If(ddlType.Value = "C", txtcustomercode.Text, txtsuppliercode.Text) & "'," _
                    & "1,'" & divid & "'"
                Else
                    strSqlQry = " sp_display_settle  '" & trantype & "','" & tranid & "','" & tranlineno & "',2,'" & divid & "'"
                End If

            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_BillSearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                gv_BillSearchResult.DataBind()
                pnlBills.Visible = True
                ToFindTotal(gv_BillSearchResult)
                lblSett.Visible = True
                ' btnSelBillDislplaySettle.Visible = True
            Else
                gv_BillSearchResult.PageIndex = 0
                gv_BillSearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
                pnlBills.Visible = False
                lblSett.Visible = False
                '  btnSelBillDislplaySettle.Visible = False
            End If
            imgicon.Style("visibility") = "hidden"
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub FillGridSettle(ByVal strorderby As String, Optional ByVal FromBIll As Integer = 1, Optional ByVal tranid As String = "", Optional ByVal trantype As String = "", Optional ByVal tranlineno As String = "", Optional ByVal strsortorder As String = "ASC")
        Dim myDS1 As New DataSet
        Dim divid As String
        divid = ViewState("divcode") '  objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

        gv_SettleSearchResult.Visible = True
        lblMsg1.Visible = False
        'pnlSettlements.Visible = True
        'btnSelSettleDisplayBill.Visible = True

        If gv_BillSearchResult.PageIndex < 0 Then
            gv_BillSearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            If txtFromDate.Text <> "" And txtToDate.Text <> "" And (txtcustomercode.Text <> "[Select]" Or txtsuppliercode.Text <> "[Select]") Then
                If FromBIll = 1 Then
                    strSqlQry = " sp_display_bills  '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "' , " _
                    & " '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "','" & ddlType.Value & "','" & If(ddlType.Value = "C", txtcustomercode.Text, txtsuppliercode.Text) & "'," _
                    & "2,'" & divid & "'"
                Else
                    strSqlQry = " sp_display_settle  '" & trantype & "','" & tranid & "','" & tranlineno & "',1,'" & divid & "'"
                End If
            End If
            SqlConn1 = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter1 = New SqlDataAdapter(strSqlQry, SqlConn1)
            myDataAdapter1.Fill(myDS1)
            gv_SettleSearchResult.DataSource = myDS1

            If myDS1.Tables(0).Rows.Count > 0 Then
                gv_SettleSearchResult.DataBind()
                pnlSettlements.Visible = True
                ToFindTotal(gv_SettleSearchResult)
                lblAdj.Visible = True
                'btnSelSettleDisplayBill.Visible = True
            Else
                gv_SettleSearchResult.PageIndex = 0
                gv_SettleSearchResult.DataBind()
                lblMsg1.Visible = True
                lblMsg1.Text = "Records not found, Please redefine search criteria."
                pnlSettlements.Visible = False
                lblAdj.Visible = False
                ' btnSelSettleDisplayBill.Visible = False
            End If
            imgicon.Style("visibility") = "hidden"
            clsDBConnect.dbAdapterClose(myDataAdapter1)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn1)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
#End Region
    Protected Sub btnBillSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FindGrid(2)
    End Sub
    Protected Sub btnSettleSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FindGrid(1)
    End Sub
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'ddlCustomer.Value = "[Select]"
        'ddlCustomerName.Value = "[Select]"
        ddlType.Value = "[Select]"
        txtBillTranId.Value = ""
        txtSettleTranId.Value = ""
        txtFromDate.Text = ""
        txtToDate.Text = ""
        If txtFromDate.Text = "" Then
            txtFromDate.Text = Format(objectcl.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
        End If
        If txtToDate.Text = "" Then
            txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
        End If
        gv_BillSearchResult.Visible = False
        gv_SettleSearchResult.Visible = False
        pnlBills.Visible = False
        pnlSettlements.Visible = False
        ' btnSelBillDislplaySettle.Visible = False
        'btnSelSettleDisplayBill.Visible = False
        lblMsg.Visible = False
        lblMsg1.Visible = False
    End Sub
#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGridBill(Session("strsortexpression"), "")
        myDS = gv_BillSearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_BillSearchResult.DataSource = dataView
            gv_BillSearchResult.DataBind()
        End If
    End Sub
#End Region


    'Private Sub FillGridWithOrderByValues()
    '    Select Case ddlOrderBy.SelectedIndex
    '        Case 0
    '            FillGrid("tran_id", "DESC")
    '        Case 1
    '            FillGrid("tran_id", "ASC")
    '        Case 2
    '            FillGrid("acctype", "ASC")
    '        Case 3
    '            FillGrid("supcode", "ASC")
    '        Case 4
    '            FillGrid("supname", "ASC")
    '        Case 5
    '            FillGrid("total", "ASC")
    '    End Select
    'End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("RptBillStatusQuery_Type") = "C" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptBillStatusCust','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("RptBillStatusQuery_Type") = "S" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptBillStatusSupp','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
    End Sub

    Protected Sub gv_BillSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_BillSearchResult.PageIndexChanging
        'gv_BillSearchResult.PageIndex = e.NewPageIndex
        'FillGridBill("tran_id")
    End Sub

    Protected Sub gv_BillSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_BillSearchResult.RowCommand
        Try
            Dim gvRow As GridViewRow
            If e.CommandName = "Page" Then Exit Sub
            Dim lblBillDocType, lblBillDocNo, lblBilltranlineno As Label
            If e.CommandName = "View" Then
                gvRow = gv_BillSearchResult.Rows(CType(e.CommandArgument.ToString, Integer))
                lblBillDocType = gvRow.FindControl("lblBillDocType")
                lblBillDocNo = gvRow.FindControl("lblBillDocNo")
                lblBilltranlineno = gvRow.FindControl("lblBilltranlineno")
                FillGridSettle("Tranid", 2, lblBillDocNo.Text, lblBillDocType.Text, lblBilltranlineno.Text)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub
    Protected Sub gv_BillSearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_BillSearchResult.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim lblBillAmout, lblbillCrDr As Label
        '    lblBillAmout = e.Row.FindControl("lblBillAmout")
        '    lblbillCrDr = e.Row.FindControl("lblbillCrDr")
        '    If lblBillAmout.Text <> "" Then
        '        If Val(lblBillAmout.Text) > 0 Then
        '            lblbillCrDr.Text = "Dr"
        '        Else
        '            lblbillCrDr.Text = "Cr"
        '        End If
        '    Else
        '        lblbillCrDr.Text = ""
        '    End If
        'End If
    End Sub

    Protected Sub gv_SettleSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SettleSearchResult.RowCommand
        Try
            Dim gvRow As GridViewRow
            If e.CommandName = "Page" Then Exit Sub
            Dim lblSettleDocType, lblSettleDocNo, lblSettletranlineno As Label
            If e.CommandName = "View" Then
                gvRow = gv_SettleSearchResult.Rows(CType(e.CommandArgument.ToString, Integer))
                lblSettleDocType = gvRow.FindControl("lblSettleDocType")
                lblSettleDocNo = gvRow.FindControl("lblSettleDocNo")
                lblSettletranlineno = gvRow.FindControl("lblSettletranlineno")
                FillGridBill("Tranid", 2, lblSettleDocNo.Text, lblSettleDocType.Text, lblSettletranlineno.Text)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub


    Protected Sub gv_SettleSearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SettleSearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSettleDocType, lblSettleDocNo, lblChequeno As Label
            ' lblSettleAmount = e.Row.FindControl("lblSettleAmount")
            ' lblSettleCrDr = e.Row.FindControl("lblSettleCrDr")
            'If lblSettleAmount.Text <> "" Then
            '    If Val(lblSettleAmount.Text) > 0 Then
            '        lblSettleCrDr.Text = "Dr"
            '    Else
            '        lblSettleCrDr.Text = "Cr"
            '    End If
            'Else
            '    lblSettleCrDr.Text = ""
            'End If
            lblSettleDocType = e.Row.FindControl("lblSettleDocType")
            lblSettleDocNo = e.Row.FindControl("lblSettleDocNo")
            lblChequeno = e.Row.FindControl("lblChequeno")
            If lblSettleDocNo.Text <> "" And lblSettleDocType.Text <> "" Then
                Dim divid As String
                divid = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                lblChequeno.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select   unknowncommon1  from acccommon_master where  tran_type='" & lblSettleDocType.Text & "' and tran_id='" & lblSettleDocNo.Text & "' and div_id='" & divid & "' ")
            Else
                lblChequeno.Text = ""
            End If
        End If
    End Sub

    Protected Sub gv_SettleSearchResult_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gv_SettleSearchResult.SelectedIndexChanging
        gv_BillSearchResult.PageIndex = e.NewSelectedIndex
        FillGridSettle("tran_id")
    End Sub

    Protected Sub btnDisplayBill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplayBill.Click
        'If ValidatePage() = False Then
        '    Exit Sub
        'End If
        FillGridBill("Tranid")
    End Sub

    Protected Sub btnDisplaySettle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ValidatePage() = False Then
            Exit Sub
        End If
        FillGridSettle("Tranid")
    End Sub


    Private Sub FindGrid(ByVal displayflag As Integer)
        Try
            Dim gvRow As GridViewRow
            If displayflag = 2 Then
                Dim lblBillDocNo As Label
                For Each gvRow In gv_BillSearchResult.Rows
                    lblBillDocNo = gvRow.FindControl("lblBillDocNo")
                    If lblBillDocNo.Text = txtBillTranId.Value.Trim Then
                        gv_BillSearchResult.SelectedIndex = gvRow.DataItemIndex
                        Exit For
                    End If
                Next
            ElseIf displayflag = 1 Then
                Dim lblSettleDocNo As Label
                For Each gvRow In gv_SettleSearchResult.Rows
                    lblSettleDocNo = gvRow.FindControl("lblSettleDocNo")
                    If lblSettleDocNo.Text = txtSettleTranId.Value.Trim Then
                        gv_SettleSearchResult.SelectedIndex = gvRow.DataItemIndex
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    '
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub gv_BillSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_BillSearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub


    Sub ToFindTotal(ByVal gridView As GridView)
        Try
            Dim grdRow As GridViewRow
            Dim lblAmoutValue As Label

            Dim lblAmount As String = String.Empty
            If gridView.ID = "gv_BillSearchResult" Then
                lblAmount = "lblBillAmout"
            Else
                lblAmount = "lblSettleAmount"
            End If

            Dim total As Decimal = 0
            For Each grdRow In gridView.Rows
                lblAmoutValue = grdRow.FindControl(lblAmount)
                total = total + Val(lblAmoutValue.Text)
            Next

            If gridView.ID = "gv_BillSearchResult" Then
                lblSett.Text = ""
                lblSett.Text = "Total Amount : "
                lblSett.Text = lblSett.Text + total.ToString
            Else
                lblAdj.Text = ""
                lblAdj.Text = "Total Amount : "
                lblAdj.Text = lblAdj.Text + total.ToString
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   '' Added shahul MCP accounts
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
End Class

'Private Sub FillQueBIllHeader(ByVal displayflag As Integer)
'    Try


'        Dim gvRow As GridViewRow
'        Dim strdiv As String
'        strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511)
'        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'        myCommand = New SqlCommand("truncate table quebill_header", SqlConn)
'        myCommand.CommandType = CommandType.Text
'        myCommand.ExecuteNonQuery()
'        If displayflag = 2 Then
'            Dim lblBillDocType, lblBillDocNo, lblBilltranlineno As Label
'            Dim chkBillSelection As CheckBox
'            For Each gvRow In gv_BillSearchResult.Rows
'                lblBillDocType = gvRow.FindControl("lblBillDocType")
'                lblBillDocNo = gvRow.FindControl("lblBillDocNo")
'                lblBilltranlineno = gvRow.FindControl("lblBilltranlineno")
'                chkBillSelection = gvRow.FindControl("chkBillSelection")
'                If chkBillSelection.Checked = True Then
'                    myCommand = New SqlCommand("sp_add_quebill_header", SqlConn)
'                    myCommand.CommandType = CommandType.StoredProcedure
'                    myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv
'                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(lblBillDocNo.Text.Trim, String)
'                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(lblBillDocType.Text, String)
'                    myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int, 9)).Value = lblBilltranlineno.Text
'                    myCommand.Parameters.Add(New SqlParameter("@displayflag", SqlDbType.Int, 9)).Value = displayflag
'                    myCommand.ExecuteNonQuery()
'                End If
'            Next
'        ElseIf displayflag = 1 Then
'            Dim chkSettleSelection As CheckBox
'            Dim lblSettleDocType, lblSettleDocNo, lblSettletranlineno As Label
'            For Each gvRow In gv_SettleSearchResult.Rows
'                lblSettleDocType = gvRow.FindControl("lblSettleDocType")
'                lblSettleDocNo = gvRow.FindControl("lblSettleDocNo")
'                lblSettletranlineno = gvRow.FindControl("lblSettletranlineno")
'                chkSettleSelection = gvRow.FindControl("chkSettleSelection")
'                If chkSettleSelection.Checked = True Then
'                    myCommand = New SqlCommand("sp_add_quebill_header", SqlConn)
'                    myCommand.CommandType = CommandType.StoredProcedure
'                    myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv
'                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(lblSettleDocNo.Text.Trim, String)
'                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(lblSettleDocType.Text, String)
'                    myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int, 9)).Value = lblSettletranlineno.Text
'                    myCommand.Parameters.Add(New SqlParameter("@displayflag", SqlDbType.Int, 9)).Value = displayflag
'                    myCommand.ExecuteNonQuery()
'                End If
'            Next
'        End If
'    Catch ex As Exception
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
'        objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'    Finally
'        clsDBConnect.dbCommandClose(myCommand)                       'Close adapter
'        clsDBConnect.dbConnectionClose(SqlConn)
'    End Try
'End Sub
'Protected Sub btnSelSettleDisplayBill_Click(ByVal sender As Object, ByVal e As System.EventArgs)
'    'FillQueBIllHeader(1)
'    'FillGridBill("Tranid", 2)
'End Sub
'Protected Sub btnSelBillDislplaySettle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
'    'FillQueBIllHeader(2)
'    'FillGridSettle("Tranid", 2)
'End Sub