Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.IO
Imports Ionic.Zip

Partial Class SalesInvoiceSearchNew
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Enum GridCol"
    Enum GridCol
        invoiceNo = 0
        invoiceDate = 1
        status = 2
        bookingNumber = 3
        customerName = 4
        customerRef = 5
        currency = 6
        amount = 7
        salesAmount = 8
        costAmount = 9
        addDate = 10
        addUser = 11
        modDate = 12
        modUser = 13
        view = 14
        print = 15
        PrintPL = 16
        printJournal = 17
    End Enum
#End Region

#Region "Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim strappname As String = ""
        strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
    End Sub
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & AppId & "'")
                    If strappname = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accounts display name does not match with accounts module name in division master' );", True)
                        Exit Sub
                    End If
                End If

                txtDivcode.Value = ViewState("divcode")

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\SalesInvoiceSearchNew.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gvSearch:=gvSalesInvoice, ViewColumnNo:=GridCol.view, PrintColumnNo:=GridCol.print)
                'Tanvir 10012023
                'If gvSalesInvoice.Columns(15).Visible Then
                '    gvSalesInvoice.Columns(16).Visible = False
                '    gvSalesInvoice.Columns(17).Visible = True
                '    gvSalesInvoice.Columns(18).Visible = False
                'Else
                '    gvSalesInvoice.Columns(16).Visible = False
                '    gvSalesInvoice.Columns(17).Visible = False
                '    gvSalesInvoice.Columns(18).Visible = False
                'End If
                If gvSalesInvoice.Columns(16).Visible Then
                    gvSalesInvoice.Columns(17).Visible = False
                    gvSalesInvoice.Columns(18).Visible = True
                    gvSalesInvoice.Columns(19).Visible = False
                Else
                    gvSalesInvoice.Columns(17).Visible = False
                    gvSalesInvoice.Columns(18).Visible = False
                    gvSalesInvoice.Columns(19).Visible = False
                End If
                Session("DtSalesInvoiceDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("DtSalesInvoiceDynamic") = dtDynamic
                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                Session.Add("strsortExpression", "addDate")
                Session.Add("strsortdirection", SortDirection.Descending)

                Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                gvSalesInvoice.Columns(8).HeaderText = "Sales Amount (" + baseCurrency + ")"
                gvSalesInvoice.Columns(9).HeaderText = "Sales VatAmount (" + baseCurrency + ")" 'changed by Tanvir on 10012024
                gvSalesInvoice.Columns(10).HeaderText = "Cost Value (" + baseCurrency + ")" 'changed by mohamed on 24/06/2021
                Dim SaleinvoiceHideTaxable As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(option_selected,0) from reservation_parameters where param_id='5515'"))
                hdnSalesInvoiceFormat.Value = SaleinvoiceHideTaxable

                FillGridNew()

                btnDownloadAllFormat2.Visible = False
                If hdnSalesInvoiceFormat.Value = 1 Then
                    btnDownloadAllFormat2.Visible = True
                    btnDownloadAllFormat1.Text = "Download All Invoice - Format 1"
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SalesInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SalesInvoicePostBack") Then
            FillGridNew()
        End If
    End Sub
#End Region

#Region "Protected Sub btnvsprocess_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess.Click"
    Protected Sub btnvsprocess_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub FilterGrid()"
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessInvoice As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "INVOICE NO"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("INVOICE NO", lsProcessInvoice, "INVOICE NO")
                Case "STATUS"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("STATUS", lsProcessInvoice, "STATUS")
                Case "CUSTOMER"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessInvoice, "CUSTOMER")
                Case "BOOKING NO"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BOOKING NO", lsProcessInvoice, "BOOKING NO")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("DtSalesInvoiceDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 
    End Sub
#End Region

#Region " Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean"
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("DtSalesInvoiceDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("DtSalesInvoiceDynamic") = dtt
            End If
        End If
        Return True
    End Function
#End Region

#Region "Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click"
    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("DtSalesInvoiceDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("DtSalesInvoiceDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("DtSalesInvoiceDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("DtSalesInvoiceDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnResetSearch_Click(sender As Object, e As System.EventArgs) Handles btnResetSearch.Click"
    Protected Sub btnResetSearch_Click(sender As Object, e As System.EventArgs) Handles btnResetSearch.Click
        ddlOrder.SelectedIndex = 0
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
#End Region
   
#Region "Private Sub FillGridNew()"
    Private Sub FillGridNew()
        Try
            Dim strBindCondition As String = ""
            strBindCondition = BuildConditionNew()
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            Dim myDS As New DataSet
            lblMsg.Visible = False
            If gvSalesInvoice.PageIndex < 0 Then gvSalesInvoice.PageIndex = 0
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "Desc" 'changed by mohamed on 24/06/2021
            'changed by mohamed on 24/06/2021
            'strSqlQry = "select invoiceno,invoiceDate,'' as status, I.requestid as bookingNumber,I.agentname as customerName,I.agentref as customerRef,I.agentcurrcode as currency," &
            '"I.salecurrency amount,I.salevaluebase salesAmount ,I.addDate,I.addUser,I.modDate,I.modUser " &
            '"from invoiceheader I(nolock) where I.divcode='" + ViewState("divcode") + "'"
            ' Tanvir(10012024) salevatvaluebase
            strSqlQry = "select i.invoiceno,i.invoiceDate,'Posted' as status, I.requestid as bookingNumber,I.agentname as customerName,I.agentref as customerRef," & _
            " I.agentcurrcode as currency, I.salecurrency amount,I.salevaluebase salesAmount, isnull(pp.salevatvaluebase,0) salevatvaluebase,isnull(pp.prices_costvaluebase,0) CostValueBase , " & _
            " I.addDate,I.addUser,I.modDate,I.modUser from invoiceheader I(nolock) " & _
            " left join vw_booking_provisionposting_allservices_summary pp (nolock) on pp.invoiceno=i.invoiceno and" & _
            " pp.divcode=i.divcode and pp.requestid=i.requestid where I.divcode='" + ViewState("divcode") + "'"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                gvSalesInvoice.DataSource = myDS.Tables(0)
                gvSalesInvoice.PageSize = pagevaluecus
                gvSalesInvoice.DataBind()
            Else
                gvSalesInvoice.PageIndex = 0
                gvSalesInvoice.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function BuildConditionNew() As String"
    Private Function BuildConditionNew() As String
        Dim dtt As DataTable
        dtt = Session("DtSalesInvoiceDynamic")
        Dim strInvoiceNoValue As String = ""
        Dim strStatusValue As String = ""
        Dim strCustomerValue As String = ""
        Dim strBookingnoValue As String = ""
        Dim strTextValue As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "INVOICE NO" Then
                        If strInvoiceNoValue <> "" Then
                            strInvoiceNoValue = strInvoiceNoValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strInvoiceNoValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "STATUS" Then
                        If strStatusValue <> "" Then
                            strStatusValue = strStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strCustomerValue <> "" Then
                            strCustomerValue = strCustomerValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomerValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "BOOKING NO" Then
                        If strBookingnoValue <> "" Then
                            strBookingnoValue = strBookingnoValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strBookingnoValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            strWhereCond = ""
            If strInvoiceNoValue.Trim <> "" Then
                strWhereCond = "I.invoiceNo IN (" & Trim(strInvoiceNoValue.Trim.ToUpper) & ")"
            End If
            If strStatusValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "status in (" & Trim(strStatusValue.Trim.ToUpper) & "))"
                Else
                    strWhereCond = strWhereCond & "and status in (" & Trim(strStatusValue.Trim.ToUpper) & "))"
                End If
            End If
            If strCustomerValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "I.agentName in (" & Trim(strCustomerValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and I.agentName in (" & Trim(strCustomerValue.Trim.ToUpper) & ")"
                End If
            End If
            If strBookingnoValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "I.requestid in (" & Trim(strBookingnoValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and I.requestid in (" & Trim(strBookingnoValue.Trim.ToUpper) & ")"
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
                            strWhereCond1 = "I.InvoiceNo like '%" & Trim(strValue.Trim.ToUpper) & "%' " &
                            "or I.agentName like '%" & Trim(strValue.Trim.ToUpper) & "%' or I.requestid like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " or I.InvoiceNo like '%" & Trim(strValue.Trim.ToUpper) & "%' " &
                            "or I.agentName like '%" & Trim(strValue.Trim.ToUpper) & "%' or I.requestid like '%" & Trim(strValue.Trim.ToUpper) & "%'"
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
                If ddlOrder.SelectedValue = "I" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),I.InvoiceDate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),I.InvoiceDate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
                    End If
                ElseIf ddlOrder.SelectedValue = "C" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),I.adddate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
                    End If
                ElseIf ddlOrder.SelectedValue = "M" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10), I.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10), moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                    End If
                End If
            End If
            BuildConditionNew = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            BuildConditionNew = ""
        End Try
    End Function
#End Region

#Region "Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click"
    Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('SalesInvoiceNew.aspx?State=New&divid=" & ViewState("divcode") & "' ,'SalesInvoice');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
#End Region

#Region "Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged"
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvSalesInvoice_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSalesInvoice.PageIndexChanging"
    Protected Sub gvSalesInvoice_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSalesInvoice.PageIndexChanging
        gvSalesInvoice.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvSalesInvoice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSalesInvoice.RowCommand"
    Protected Sub gvSalesInvoice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSalesInvoice.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblInvoiceNo As Label
            lblInvoiceNo = gvSalesInvoice.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblInvoiceNo")
            Dim strpop As String = ""
            If e.CommandName = "View" Then
                strpop = "window.open('AccountPosting.aspx?State=View&divid=" & ViewState("divcode") & "&ID=" & CType(lblInvoiceNo.Text.Trim, String) & "','AccountPosting');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            ElseIf e.CommandName = "Print" Then
                strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & CType(lblInvoiceNo.Text.Trim, String) & "','SalesInvoicePrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoice", strpop, True)
            ElseIf e.CommandName = "InvoicePrint2" Then
                strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & CType(lblInvoiceNo.Text.Trim, String) & "&FormatType=F2','SalesInvoicePrint2');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoice2", strpop, True)
            ElseIf e.CommandName = "PrintJournal" Then
                strpop = "window.open('PrintReport.aspx?printId=InvoiceVoucher&InvoiceNo=" & CType(lblInvoiceNo.Text.Trim, String) & "','InvoiceVoucherPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "InvoiceVoucher", strpop, True)
            ElseIf e.CommandName = "PrintProforma" Then
                Dim lblRequestId As Label
                lblRequestId = gvSalesInvoice.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblBookingNumber")
                strpop = "window.open('PrintReport.aspx?printId=bookingConfirmation&RequestId=" & CType(lblRequestId.Text.Trim, String) & "','ProformaPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvSalesInvoice_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSalesInvoice.RowDataBound"
    Protected Sub gvSalesInvoice_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSalesInvoice.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim lblAmount As Label = CType(e.Row.FindControl("lblAmount"), Label)
            If IsNumeric(lblAmount.Text) Then
                lblAmount.Text = Math.Round(Convert.ToDecimal(lblAmount.Text), decimalPlaces)
            End If
            Dim lblSalesAmount As Label = CType(e.Row.FindControl("lblSalesAmount"), Label)
            If IsNumeric(lblSalesAmount.Text) Then
                lblSalesAmount.Text = Math.Round(Convert.ToDecimal(lblSalesAmount.Text), decimalPlaces)
            End If
            'Tanvir 10012024
            Dim lblSalesVatAmount As Label = CType(e.Row.FindControl("lblSalesVatAmount"), Label)
            If IsNumeric(lblSalesAmount.Text) Then
                lblSalesVatAmount.Text = Math.Round(Convert.ToDecimal(lblSalesVatAmount.Text), decimalPlaces)
            End If

            Dim lblCostAmount As Label = CType(e.Row.FindControl("lblCostAmount"), Label) 'changed by mohamed on 24/06/2021
            If IsNumeric(lblCostAmount.Text) Then
                lblCostAmount.Text = Math.Round(Convert.ToDecimal(lblCostAmount.Text), decimalPlaces)
            End If

            If hdnSalesInvoiceFormat.Value = 1 Then
                Dim lbtnPrint2 As LinkButton = CType(e.Row.FindControl("lbtnPrint2"), LinkButton)
                lbtnPrint2.Visible = True
                Dim lbtnPrint As LinkButton = CType(e.Row.FindControl("lbtnPrint"), LinkButton)
                lbtnPrint.Text = "Format1"
            End If

            Dim lbtnPrintPL As LinkButton = CType(e.Row.FindControl("lbtnPrintPL"), LinkButton)
            lbtnPrintPL.Visible = False
        End If
    End Sub
#End Region

#Region "Protected Sub gvSalesInvoice_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSalesInvoice.Sorting"
    Protected Sub gvSalesInvoice_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSalesInvoice.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColumn()
    End Sub
#End Region

#Region "Public Sub SortGridColumn()"
    Public Sub SortGridColumn()
        Dim DataTable As DataTable
        FillGridNew()
        DataTable = gvSalesInvoice.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gvSalesInvoice.DataSource = dataView
            gvSalesInvoice.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim strBindCondition As String = ""
        Try
            If gvSalesInvoice.Rows.Count > 0 Then
                strBindCondition = BuildConditionNew()
                Dim strorderby As String = Session("strsortexpression")
                Dim strsortorder As String = IIf(Session("strsortdirection") = "0", "Asc", "Desc")
                Dim myDS As New DataSet

                'changed by mohamed on 24/06/2021
                'strSqlQry = "select invoiceno,invoiceDate,'' as status, I.requestid as bookingNumber,I.agentname as customerName,I.agentref as customerRef,I.agentcurrcode as currency," &
                '"I.salecurrency amount,I.salevaluebase salesAmount ,I.addDate,I.addUser,I.modDate,I.modUser " &
                '"from invoiceheader I(nolock) where I.divcode='" + ViewState("divcode") + "'"

                strSqlQry = "select i.invoiceno,i.invoiceDate,'Posted' as status, I.requestid as bookingNumber,I.agentname as customerName,I.agentref as customerRef," & _
                " I.agentcurrcode as currency, I.salecurrency amount,I.salevaluebase salesAmount, isnull(pp.prices_costvaluebase,0) CostValueBase , " & _
                " I.addDate,I.addUser,I.modDate,I.modUser from invoiceheader I(nolock) " & _
                " left join vw_booking_provisionposting_allservices_summary pp (nolock) on pp.invoiceno=i.invoiceno and" & _
                " pp.divcode=i.divcode and pp.requestid=i.requestid where I.divcode='" + ViewState("divcode") + "'"

                If strBindCondition <> "" Then
                    strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS, "invoiceHeader")
                objUtils.ExportToExcel(myDS, Response)
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete"
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Dim lbltitle As Label = CType(Me.Master.FindControl("title"), Label)
        Dim strTitle As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code ='" & ViewState("divcode") & "'")
        lbltitle.Text = strTitle
        Me.Page.Title = strTitle
    End Sub
#End Region

    Protected Sub btnDownloadAllFormat1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadAllFormat1.Click
        Try
            sbDownloadAllInvoice(False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub btnDownloadAllFormat2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadAllFormat2.Click
        Try
            sbDownloadAllInvoice(True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Sub sbDownloadAllInvoice(ByVal lbFormat2 As Boolean)
        Dim strpop As String, i As Integer = 0
        For Each gvRow As GridViewRow In gvSalesInvoice.Rows
            i += 1
            Dim chkDownload As CheckBox = gvRow.FindControl("chkDownload")
            Dim lblInvoiceNo As Label = gvRow.FindControl("lblInvoiceNo")
            If chkDownload.Checked = True Then
                If lbFormat2 = True Then
                    strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & CType(lblInvoiceNo.Text.Trim, String) & "&FormatType=F2','SalesInvoicePrint2" + i.ToString() + "');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoicepopup2" + i.ToString(), strpop, True)
                Else
                    strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & CType(lblInvoiceNo.Text.Trim, String) & "','SalesInvoicePrint" + i.ToString() + "');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoicepopup" + i.ToString(), strpop, True)
                End If

                'chkDownload.Checked = False
            End If
        Next
    End Sub

    Protected Sub chkDownloadAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            For Each gvRow As GridViewRow In gvSalesInvoice.Rows
                Dim chkDownloadAll As CheckBox = CType(sender, CheckBox)
                Dim chkDownload As CheckBox = gvRow.FindControl("chkDownload")
                chkDownload.Checked = chkDownloadAll.Checked
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    ' roslain 06/09/2023 CR point - 4 download all invoices into folder
    'Protected Sub btnDownloadFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadFolder.Click
    '    Try
    '        Dim rootpath As String = System.Configuration.ConfigurationManager.AppSettings("InvoiceDownloadPath")
    '        '"D:\Elevate\CRPoints\DownloadInvoice"
    '        For Each gvRow As GridViewRow In gvSalesInvoice.Rows
    '            Dim chkDownload As CheckBox = gvRow.FindControl("chkDownload")
    '            Dim lblInvoiceNo As Label = gvRow.FindControl("lblInvoiceNo")
    '            Dim lblCustomerName As Label = gvRow.FindControl("lblCustomerName")
    '            Dim lblInvoiceDate As Label = gvRow.FindControl("lblInvoiceDate")

    '            Dim rootfolder As String = rootpath & "\" & lblCustomerName.Text.Replace("'", "")
    '            ' Dim CurrentDate As String = DateTime.Now.ToString("yyyy-MM-dd")
    '            'Dim subfolder As String = rootfolder & "\" & CurrentDate
    '            Dim InvoiceDate As String = lblInvoiceDate.Text.Replace("/", "-")
    '            Dim subfolder As String = rootfolder & "\" & InvoiceDate

    '            'Directory.Delete(folderPath, true);

    '            If chkDownload.Checked = True Then

    '                If Directory.Exists(rootfolder) Then
    '                    Directory.Delete(rootfolder, True)
    '                End If

    '                Directory.CreateDirectory(rootfolder)


    '                If Directory.Exists(subfolder) Then
    '                    Directory.Delete(subfolder, True)
    '                End If

    '                Directory.CreateDirectory(subfolder)

    '                'If Not Directory.Exists(rootfolder) Then
    '                '    Directory.CreateDirectory(rootfolder)
    '                'End If

    '                'If Not Directory.Exists(subfolder) Then
    '                '    Directory.CreateDirectory(subfolder)
    '                'End If

    '                Dim fileName As String = subfolder & "\Invoice@" + lblInvoiceNo.Text + ".pdf"

    '                InvoiceSaveToFolder(fileName, lblInvoiceNo.Text)
    '            End If
    '        Next


    '        Dim Zippath As String = rootpath & "\"

    '        Using zip As New ZipFile()
    '            'zip.AlternateEncodingUsage = ZipOption.AsNecessary
    '            ' zip.AddDirectory(Server.MapPath("~/Upload/"))
    '            zip.AddDirectory(Zippath)
    '            Response.Clear()
    '            Response.BufferOutput = False
    '            Dim zipName As String = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
    '            Response.ContentType = "application/zip"
    '            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
    '            zip.Save(Response.OutputStream)
    '            Response.End()
    '        End Using




    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '    End Try
    'End Sub


    Protected Sub btnDownloadFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadFolder.Click
        Try
            Dim rootpath As String = System.Configuration.ConfigurationManager.AppSettings("InvoiceDownloadPath")
            '"D:\Elevate\CRPoints\DownloadInvoice"



            If Directory.Exists(rootpath) Then
                Directory.Delete(rootpath, True)
            End If

            Dim selectFlag As Integer = 0

            For Each gvRow As GridViewRow In gvSalesInvoice.Rows
                Dim chkDownload As CheckBox = gvRow.FindControl("chkDownload")
                Dim lblInvoiceNo As Label = gvRow.FindControl("lblInvoiceNo")
                Dim lblCustomerName As Label = gvRow.FindControl("lblCustomerName")
                Dim lblInvoiceDate As Label = gvRow.FindControl("lblInvoiceDate")



                Dim rootfolder As String = rootpath & "\" & lblCustomerName.Text.Replace("'", "")
                ' Dim CurrentDate As String = DateTime.Now.ToString("yyyy-MM-dd")
                'Dim subfolder As String = rootfolder & "\" & CurrentDate
                Dim InvoiceDate As String = lblInvoiceDate.Text.Replace("/", "-")
                ' Dim subfolder As String = rootfolder & "\" & InvoiceDate



                'Directory.Delete(folderPath, true);



                If chkDownload.Checked = True Then

                    
                    'Directory.CreateDirectory(subfolder)
                    selectFlag = 1
                    If Not Directory.Exists(rootfolder) Then
                        Directory.CreateDirectory(rootfolder)
                    End If
                    'If Not Directory.Exists(subfolder) Then
                    '    Directory.CreateDirectory(subfolder)
                    'End If

                    ' Dim fileName As String = subfolder & "\Invoice@" + lblInvoiceNo.Text + ".pdf"

                    Dim fileName As String = rootfolder & "\Invoice@" + lblInvoiceNo.Text + ".pdf"



                    InvoiceSaveToFolder(fileName, lblInvoiceNo.Text)
                End If
            Next

            If selectFlag = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select the invoice to download' );", True)
                Return
            End If



            Dim Zippath As String = rootpath & "\"



            Using zip As New ZipFile()
                'zip.AlternateEncodingUsage = ZipOption.AsNecessary
                ' zip.AddDirectory(Server.MapPath("~/Upload/"))
                zip.AddDirectory(Zippath)
                Response.Clear()
                Response.BufferOutput = False
                Dim zipName As String = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
                Response.ContentType = "application/zip"
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
                zip.Save(Response.OutputStream)
                Response.End()
            End Using







        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub



    Public Sub InvoiceSaveToFolder(ByVal fileName As String, ByVal invoiceNo As String)
        Try

            ' Dim fileName As String = ""
            Dim bytes As Byte()

            Dim SI As clsInvoicePdf = New clsInvoicePdf()
            'Dim invoiceNo As String = Request.QueryString("InvoiceNo")
            ' Dim formatType As String = Convert.ToString(Request.QueryString("FormatType"))
            ' If formatType = Nothing Then formatType = ""
            Dim ds As New DataSet
            bytes = {}
            SI.InvoicePrint_Download(invoiceNo, bytes, ds, "Save", fileName:=fileName)  ' formatType:=formatType
            
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

   

End Class
