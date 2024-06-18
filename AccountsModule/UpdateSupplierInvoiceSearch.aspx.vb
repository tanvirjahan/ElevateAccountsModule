Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.IO
Imports ClosedXML.Excel

Partial Class UpdateSupplierInvoiceSearch
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
    Dim document As New XLWorkbook
#End Region

#Region "Enum GridCol"
    'Enum GridCol
    '    invoiceNo = 0
    '    invoiceDate = 1
    '    status = 2
    '    bookingNumber = 3
    '    customerName = 4
    '    customerRef = 5
    '    currency = 6
    '    amount = 7
    '    salesAmount = 8
    '    addDate = 9
    '    addUser = 10
    '    modDate = 11
    '    modUser = 12
    '    view = 13
    '    print = 14
    '    PrintPL = 15
    '    printJournal = 16
    'End Enum
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

    Protected Sub ReadMoreLinkButtonNarration_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblNarration"), Label)
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
            '  ModalExtraPopup1.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim strappname As String = ""
        strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

                'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                '                                       CType(strappname, String), "AccountsModule\UpdateSupplierInvoiceSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                '                                       btnPrint, gvSearch:=gvSupplierInvoice, ViewColumnNo:=GridCol.view, PrintColumnNo:=GridCol.print)
                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                Session.Add("strsortExpression", "addDate")
                Session("DtSupplierInvoiceDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("DtSupplierInvoiceDynamic") = dtDynamic
                FillGridNew()
           
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("UpdateSupplierInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "UpdateSupplierInvoicePostBack") Then
            btnResetSelection_Click(sender, e)
        End If
    End Sub
#End Region

#Region "Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete'"
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Dim lbltitle As Label = CType(Me.Master.FindControl("title"), Label)
        Dim strTitle As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code ='" & ViewState("divcode") & "'")
        lbltitle.Text = strTitle
        Me.Page.Title = strTitle
    End Sub
#End Region

#Region "Protected Sub btnvsprocess_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess.Click"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "PURINVOICE NO"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("PURINVOICE NO", lsProcessInvoice, "PI NO")
                Case "SERVICE"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SERVICE", lsProcessInvoice, "SERV")
                Case "SUPPLIER"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIER", lsProcessInvoice, "S")
                Case "CONTROL ACCOUNT"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CONTROL ACCOUNT", lsProcessInvoice, "CA")
                Case "CURRENCY"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CURRENCY", lsProcessInvoice, "C")
                Case "SUPPLIER INVNO"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIER INVNO", lsProcessInvoice, "SINV NO")
                Case "NARRATION"
                    lsProcessInvoice = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BOOKING NO", lsProcessInvoice, "N")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("DtSupplierInvoiceDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 
    End Sub
#End Region

#Region " Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean"
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("DtSupplierInvoiceDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("DtSupplierInvoiceDynamic") = dtt
            End If
        End If
        Return True
    End Function
#End Region

#Region "Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click"
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("DtSupplierInvoiceDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("DtSupplierInvoiceDynamic") = dtt
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
            dtDynamics = Session("DtSupplierInvoiceDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("DtSupplierInvoiceDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnResetSearch_Click(sender As Object, e As System.EventArgs) Handles btnResetSearch.Click"
    Protected Sub btnResetSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSearch.Click
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
            If gvSupplierInvoice.PageIndex < 0 Then gvSupplierInvoice.PageIndex = 0
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "Desc"
            ' case when I.suppliertype ='S' then 'Supplier'  when I.suppliertype ='SA' then  'Supplier Agent' end ' m.acctname Tanvir 05012023
            strSqlQry = "select I.purchaseinvoiceno,I.purchaseinvoicedate, sp. sptypename  as suppliertype ,I.partycode,p.partyname," &
            "c.currname,I.hotelinvoiceno,I.Bookingrefno,I.total_actualamount,I.total_prices_costvatvaluebase,I.narration,I.addDate,I.addUser,I.modDate,I.modUser " &
            "from purchaseinvoiceheader I(nolock)   left join acctmast m on I.controlacctcode=m.acctcode and I.divcode=m.div_code left join  currmast c on c.currcode=I.currcode  left join partymast p on p.partycode=I.partycode  left join sptypemast sp on sp.sptypecode=p.sptypecode and p.partycode=I.partycode where I.divcode='" + ViewState("divcode") + "'  "
            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                gvSupplierInvoice.DataSource = myDS.Tables(0)
                gvSupplierInvoice.PageSize = pagevaluecus
                gvSupplierInvoice.DataBind()
            Else
                gvSupplierInvoice.PageIndex = 0
                'gvSupplierInvoice.DataSource = myDS.Tables(0)
                gvSupplierInvoice.DataBind()
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
            objUtils.WritErrorLog("UpdateSupplierInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function BuildConditionNew() As String"
    Private Function BuildConditionNew() As String
        Dim dtt As DataTable
        dtt = Session("DtSupplierInvoiceDynamic")
        Dim strInvoiceNoValue As String = ""
        Dim strInvoiceTypeValue As String = ""
        Dim strSupplierValue As String = ""
        Dim strControlAValue As String = ""
        Dim strTextValue As String = ""
        Dim strSupplierInvValue As String = ""
        Dim strCurrencyValue As String = ""

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "PURINVOICE NO" Then
                        If strInvoiceNoValue <> "" Then
                            strInvoiceNoValue = strInvoiceNoValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strInvoiceNoValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SERVICE" Then
                        If strInvoiceTypeValue <> "" Then
                            strInvoiceTypeValue = strInvoiceTypeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strInvoiceTypeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Then
                        If strSupplierValue <> "" Then
                            strSupplierValue = strSupplierValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSupplierValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CONTROL ACCOUNT" Then
                        If strControlAValue <> "" Then
                            strControlAValue = strControlAValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strControlAValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CURRENCY" Then
                        If strCurrencyValue <> "" Then
                            strCurrencyValue = strCurrencyValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCurrencyValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "SUPPLIER INVNO" Then
                        If strSupplierInvValue <> "" Then
                            strSupplierInvValue = strSupplierInvValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSupplierInvValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
                strWhereCond = "I.purchaseinvoiceno IN (" & Trim(strInvoiceNoValue.Trim.ToUpper) & ")"
            End If
            If strInvoiceTypeValue.Trim <> "" Then
                If strWhereCond = "" Then

                    strWhereCond = " sp.sptypename  in (" & Trim(strInvoiceTypeValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and   sp.sptypename  in (" & Trim(strInvoiceTypeValue.Trim.ToUpper) & ")"

                End If
                'Else
                '    strWhereCond = "I.suppliertype in ('SA')"

                '    'End If
                'Else
                '    If strInvoiceTypeValue = "Supplier" Then
                '        strWhereCond = strWhereCond & "and select sptypename from sptypemast sp inner join partymast p on sp.sptypecode=p.sptypecode and p.partycode=i.partycode and sp.sptypename  in (" & Trim(strInvoiceNoValue.Trim.ToUpper) & ")"
                '    Else
                '        strWhereCond = strWhereCond & "and I.suppliertype in ('SA')"

                '    End If
                '  strWhereCond = strWhereCond & "and I.suppliertype in (" & Trim(strInvoiceTypeValue.Trim.ToUpper) & ")"
            End If

            If strSupplierValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "p.partyname in (" & Trim(strSupplierValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and p.partyname in (" & Trim(strSupplierValue.Trim.ToUpper) & ")"
                End If
            End If
            If strControlAValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "m.acctname in (" & Trim(strControlAValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and m.acctname in (" & Trim(strControlAValue.Trim.ToUpper) & ")"
                End If
            End If
            If strCurrencyValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "c.currname in (" & Trim(strCurrencyValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and c.currname in (" & Trim(strCurrencyValue.Trim.ToUpper) & ")"
                End If
            End If
            If strSupplierInvValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "I.hotelinvoiceno in (" & Trim(strSupplierInvValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and I.hotelinvoiceno in (" & Trim(strSupplierInvValue.Trim.ToUpper) & ")"
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
                            strWhereCond1 = "I.purchaseinvoiceno like '%" & Trim(strValue.Trim.ToUpper) & "%' " &
                            "or I.bookingrefno like '%" & Trim(strValue.Trim.ToUpper) & "%' or I.narration like '%" & Trim(strValue.Trim.ToUpper) & "%'   " &
                            "or c.currname like  '%" & Trim(strValue.Trim.ToUpper) & "%'  or I.hotelinvoiceno like '%" & Trim(strValue.Trim.ToUpper) & "%'  or m.acctname like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " or I.InvoiceNo like '%" & Trim(strValue.Trim.ToUpper) & "%' " &
                                                       "or I.bookingrefno like '%" & Trim(strValue.Trim.ToUpper) & "%' or I.narration like '%" & Trim(strValue.Trim.ToUpper) & "%'   " &
                            "or c.currname like  '%" & Trim(strValue.Trim.ToUpper) & "%'  or I.hotelinvoiceno like '%" & Trim(strValue.Trim.ToUpper) & "%'  or m.acctname like '%" & Trim(strValue.Trim.ToUpper) & "%'"

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
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),I.purchaseInvoiceDate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),I.purchaseInvoiceDate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
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
            objUtils.WritErrorLog("UpdateSupplierInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            BuildConditionNew = ""
        End Try
    End Function
#End Region

#Region "Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click"
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('UpdateSupplierInvoice.aspx?State=New&divid=" & ViewState("divcode") & "' ,'SupplierInvoice');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
#End Region

    '#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    '    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
    '        Dim strBindCondition As String = ""
    '        Try



    '            If gvSupplierInvoice.Rows.Count > 0 Then
    '                strBindCondition = BuildConditionNew()
    '                Dim strorderby As String = Session("strsortexpression")
    '                Dim strsortorder As String = IIf(Session("strsortdirection") = "0", "Asc", "Desc")
    '                Dim myDS As New DataSet
    '                Dim dt As New DataTable
    '                Dim dtt As New DataTable
    '                Dim rptcompanyname, filter, rptfilter As String
    '                filter = String.Empty
    '                dtt = Session("DtSupplierInvoiceDynamic")

    '                If dtt.Rows.Count > 0 Then
    '                    For i As Integer = 0 To dtt.Rows.Count - 1
    '                        filter = filter & dtt.Rows(i)("Code").ToString() & " : " & dtt.Rows(i)("Value").ToString() & " "
    '                    Next
    '                End If

    '                rptfilter = "Report Filter : " & IIf(filter <> "", filter, "") & " " & IIf(txtFromDate.Text <> "", " From Date " & txtFromDate.Text, "") & IIf(txtToDate.Text <> "", " To Date " & txtToDate.Text, "")
    '                If filter.Equals("") AndAlso txtFromDate.Text.Equals("") AndAlso txtToDate.Text.Equals("") Then
    '                    rptfilter = ""
    '                End If
    '                'strSqlQry = "select I.purchaseinvoiceno[PurchaseInvoiceNo],I.purchaseinvoicedate[PurchaseInvoiceDate], case when I.suppliertype ='S' then 'Supplier'  when I.suppliertype ='SA' then  'Supplier Agent' end  suppliertype ,I.partycode,I.partyname," &
    '                '"m.acctname,c.currname,I.hotelinvoiceno,I.Bookingrefno,I.narration,I.addDate,I.addUser,I.modDate,I.modUser " &
    '                '"from purchaseinvoiceheader I(nolock) left join acctmast m on I.controlacctcode=m.acctcode and I.divcode=m.div_code left join  currmast c on c.currcode=I.currcode  where I.divcode='" + ViewState("divcode") + "'  "
    '                strSqlQry = "select I.purchaseinvoiceno,I.purchaseinvoicedate, sp. sptypename  as suppliertype ,p.partyname," &
    '        "m.acctname,c.currname,I.hotelinvoiceno,I.Bookingrefno,I.total_actualamount,I.narration,I.addDate,I.addUser,I.modDate,I.modUser " &
    '        "from purchaseinvoiceheader I(nolock)   left join acctmast m on I.controlacctcode=m.acctcode and I.divcode=m.div_code left join  currmast c on c.currcode=I.currcode  left join partymast p on p.partycode=I.partycode  left join sptypemast sp on sp.sptypecode=p.sptypecode and p.partycode=i.partycode where I.divcode='" + ViewState("divcode") + "'  "

    '                If strBindCondition <> "" Then
    '                    strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
    '                Else
    '                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
    '                End If
    '                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '                myDataAdapter.Fill(myDS, "PurChaseInvoice")


    '                dt = myDS.Tables(0)
    '                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
    '                clsDBConnect.dbConnectionClose(SqlConn)



    '                Dim RandomCls As New Random()
    '                Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString

    '                rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)

    '                     Dim document As New XLWorkbook

    '                Dim ws = document.Worksheets.Add("UpdateSuppInv")
    '                ws.Style.Font.FontName = "arial"
    '                ws.Columns("A:B").Width = 15
    '                ws.Column("C").Width = 12
    '                ws.Columns("D:E").Width = 20
    '                ws.Column("G").Width = 15
    '                ws.Column("F").Width = 10
    '                ws.Column("H").Width = 16
    '                ws.Columns("I:J").Width = 18
    '                ws.Columns("K:N").Width = 15
    '                ws.Columns("B").SetDataType(XLCellValues.DateTime)

    '                Dim LastLine As Integer
    '                ws.Range(1, 1, 1, 14).Value = rptcompanyname
    '                ws.Range(1, 1, 1, 14).Merge.Style.Font.SetBold().Font.FontSize = 14
    '                ws.Range(1, 1, 1, 14).Style.Fill.BackgroundColor = XLColor.LightGray
    '                ws.Range(1, 1, 1, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '                ws.Range("A1").Select()
    '                ws.Range(2, 1, 2, 14).Value = "Update Supplier Invoices"
    '                Dim rptname As String = "Update Supplier Invoices"
    '                ws.Range(2, 1, 2, 14).Merge.Style.Font.SetBold().Font.FontSize = 12
    '                ws.Range(2, 1, 2, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

    '                ws.Range(3, 1, 3, 16).Merge.Style.Alignment.WrapText = True
    '                If rptfilter <> "" Then
    '                    ws.Range(3, 1, 3, 14).Value = rptfilter
    '                    ws.Range(3, 1, 3, 16).Merge.Style.Font.FontSize = 9

    '                End If

    '                ws.Range(5, 1, 5, 14).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText().Font.FontSize = 10
    '                Dim tableaheader() As String = {"Purchase InvoiceNo", "	Purchase InvoiceDate", "	Service ", "Supplier Name	", "ControlAccount", "	Currency	", "Hotel InvoiceNo	", "BookingRefNo	", "Amount", "Narration", "	Date Created	", "User Created	", "DateModified	", "UserModified"}

    '                For i = 0 To tableaheader.Length - 1
    '                    ws.Cell(5, i + 1).Value = tableaheader(i)
    '                    ws.Cell(5, i + 1).Style.Fill.SetBackgroundColor(XLColor.Apricot)
    '                Next

    '                'Dim RateSheet As IXLRange
    '                'RateSheet = ws.Cell(6, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
    '                'ws.Rows(6).Clear()
    '                'ws.Rows(6).Delete()
    '                'LastLine = 6 + dt.Rows.Count
    '                'ws.Range(6, 1, LastLine - 1, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText().Font.FontSize = 10

    '                'ws.Range(6, 11, LastLine, 11).SetDataType(XLCellValues.DateTime)
    '                'ws.Range(6, 13, LastLine, 13).SetDataType(XLCellValues.DateTime)
    '                'ws.Range("A1").Select()


    '                'If dt.Rows.Count > 0 Then
    '                '    ws.Range(LastLine, 1, LastLine, 14).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
    '                '    ws.Range(LastLine, 1, LastLine, 14).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
    '                '    ws.Range(LastLine, 1, LastLine, 14).Style.Font.Bold = True
    '                '    ws.Range(LastLine, 1, LastLine, 14).Style.Fill.BackgroundColor = XLColor.LightGray
    '                '    ws.Range(LastLine, 1, LastLine, 14).Style.Alignment.WrapText = True
    '                '    ws.Cell(LastLine, 8).Value = "Total"
    '                '    ws.Cell(LastLine, 8).Style.Font.Bold = True
    '                '    RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
    '                '    RateSheet.Columns("J").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '                '    RateSheet.Columns("L").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '                '    ws.Cell(LastLine, 9).Value = dt.Compute("SUM(total_actualamount)", String.Empty)

    '                'End If



    '                Using MyMemoryStream As New MemoryStream()

    '                    document.SaveAs(MyMemoryStream)
    '                    document.Dispose()
    '                    Response.Clear()
    '                    Response.Buffer = True
    '                    Response.AddHeader("content-disposition", "attachment;filename=" + rptname & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx")
    '                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
    '                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    '                    MyMemoryStream.WriteTo(Response.OutputStream)
    '                    Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
    '                    Response.Flush()
    '                    HttpContext.Current.ApplicationInstance.CompleteRequest()
    '                End Using

    '            Else
    '                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
    '            End If
    '        Catch ex As Exception
    '            If Not SqlConn Is Nothing Then
    '                If SqlConn.State = ConnectionState.Open Then
    '                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
    '                    clsDBConnect.dbConnectionClose(SqlConn)
    '                End If
    '            End If
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        End Try
    '    End Sub
    '#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim strBindCondition As String = ""
        Try
            Dim RandomCls As New Random()
            Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString
            Dim rptcompanyname, filter, rptfilter As String
            filter = String.Empty
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)

            Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), Integer)
            Dim decPlaces As String
            If decno = 3 Then
                decPlaces = "#,##0.000;[Red](#,##0.000)"
            Else
                decPlaces = "#,##0.00;[Red](#,##0.00)"
            End If
            Dim FileNameNew As String
            FileNameNew = "UpdateSuppInv" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"

            ' document = New XLWorkbook()
            Dim ws As IXLWorksheet = document.Worksheets.Add("UpdateSuppInv")
            ws.Style.Font.FontName = "arial"
            ws.Columns("A:B").Width = 15
            ws.Column("C").Width = 12
            ws.Columns("D:E").Width = 20
            ws.Column("G").Width = 15
            ws.Column("F").Width = 10
            ws.Column("H").Width = 16
            ws.Columns("I:J").Width = 18
            '  ws.Columns("B").SetDataType(XLCellValues.DateTime)
            ws.Column("I").Style.NumberFormat.Format = decPlaces
            ws.Columns("K:N").Width = 15
            Dim LastLine As Integer
            ws.Range(1, 1, 1, 14).Value = rptcompanyname
            ws.Range(1, 1, 1, 14).Merge.Style.Font.SetBold().Font.FontSize = 14
            ws.Range(1, 1, 1, 14).Style.Fill.BackgroundColor = XLColor.LightGray
            ws.Range(1, 1, 1, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Range("A1").Select()
            ws.Range(2, 1, 2, 14).Value = "Update Supplier Invoices"
            ws.Range(2, 1, 2, 14).Merge.Style.Font.SetBold().Font.FontSize = 12
            ws.Range(2, 1, 2, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            Dim dt As New DataTable
            Dim dtt As New DataTable
            dtt = Session("DtSupplierInvoiceDynamic")

            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    filter = filter & dtt.Rows(i)("Code").ToString() & " : " & dtt.Rows(i)("Value").ToString() & " "
                Next
            End If
          
            rptfilter = "Report Filter : " & IIf(filter <> "", filter, "") & " " & IIf(txtFromDate.Text <> "", "" & ddlOrder.SelectedItem.Text & " From Date " & txtFromDate.Text, "") & IIf(txtToDate.Text <> "", " To Date " & txtToDate.Text, "")
            If filter.Equals("") AndAlso txtFromDate.Text.Equals("") AndAlso txtToDate.Text.Equals("") Then
                rptfilter = ""
            End If
            ws.Range(3, 1, 3, 16).Merge.Style.Alignment.WrapText = True
            If rptfilter <> "" Then
                ws.Range(3, 1, 3, 14).Value = rptfilter
                ws.Range(3, 1, 3, 16).Merge.Style.Font.FontSize = 11

            End If

            ws.Range(5, 1, 5, 14).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText().Font.FontSize = 10
            Dim tableaheader() As String = {"Purchase InvoiceNo", "	Purchase InvoiceDate", "	Services", "Supplier Name	", "ControlAccount", "	Currency	", "Hotel InvoiceNo	", "BookingRefNo	", "Amount", "Narration", "	Date Created	", "User Created	", "DateModified	", "UserModified"}

            For i = 0 To tableaheader.Length - 1
                ws.Cell(5, i + 1).Value = tableaheader(i)
                ws.Cell(5, i + 1).Style.Fill.SetBackgroundColor(XLColor.Apricot)
            Next



            If gvSupplierInvoice.Rows.Count > 0 Then
                strBindCondition = BuildConditionNew()
                Dim strorderby As String = Session("strsortexpression")
                Dim strsortorder As String = IIf(Session("strsortdirection") = "0", "Asc", "Desc")
                Dim myDS As New DataSet
                'strSqlQry = "select I.purchaseinvoiceno[PurchaseInvoiceNo],I.purchaseinvoicedate[PurchaseInvoiceDate], case when I.suppliertype ='S' then 'Supplier'  when I.suppliertype ='SA' then  'Supplier Agent' end  suppliertype ,I.partycode,I.partyname," &
                '"m.acctname,c.currname,I.hotelinvoiceno,I.Bookingrefno,I.narration,I.addDate,I.addUser,I.modDate,I.modUser " &
                '"from purchaseinvoiceheader I(nolock) left join acctmast m on I.controlacctcode=m.acctcode and I.divcode=m.div_code left join  currmast c on c.currcode=I.currcode  where I.divcode='" + ViewState("divcode") + "'  "
                strSqlQry = "select I.purchaseinvoiceno,I.purchaseinvoicedate, sp. sptypename  as suppliertype ,p.partyname," &
           "m.acctname,c.currname,I.hotelinvoiceno,I.Bookingrefno,I.total_actualamount,I.narration,I.addDate,I.addUser,I.modDate,I.modUser " &
           "from purchaseinvoiceheader I(nolock)   left join acctmast m on I.controlacctcode=m.acctcode and I.divcode=m.div_code left join  currmast c on c.currcode=I.currcode  left join partymast p on p.partycode=I.partycode  left join sptypemast sp on sp.sptypecode=p.sptypecode and p.partycode=I.partycode where I.divcode='" + ViewState("divcode") + "'  "
                '  strSqlQry = "select I.purchaseinvoiceno,I.purchaseinvoicedate, sp. sptypename  as suppliertype ,p.partyname," &
                '"m.acctname,c.currname,I.hotelinvoiceno,I.Bookingrefno,I.total_actualamount,I.narration,I.addDate,I.addUser,I.modDate,I.modUser " &
                '"from purchaseinvoiceheader I(nolock)   left join acctmast m on I.controlacctcode=m.acctcode and I.divcode=m.div_code left join  currmast c on c.currcode=I.currcode  left join partymast p on p.partycode=I.partycode  left join sptypemast sp on sp.sptypecode=p.sptypecode and p.partycode=i.partycode where I.divcode='" + ViewState("divcode") + "'  "

                If strBindCondition <> "" Then
                    strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS, "PurChaseInvoice")

                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)

                dt = myDS.Tables(0)

                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(6, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(6).Clear()
                ws.Rows(6).Delete()
                LastLine = 6 + dt.Rows.Count
                ws.Range(6, 1, LastLine - 1, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText().Font.FontSize = 10

                'ws.Range(6, 11, LastLine, 11).SetDataType(XLCellValues.DateTime)
                'ws.Range(6, 13, LastLine, 13).SetDataType(XLCellValues.DateTime)
                ws.Range("A1").Select()

                If dt.Rows.Count > 0 Then
                    ws.Range(LastLine, 1, LastLine, 14).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 14).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    ws.Range(LastLine, 1, LastLine, 14).Style.Font.Bold = True
                    ws.Range(LastLine, 1, LastLine, 14).Style.Alignment.WrapText = True
                    ws.Cell(LastLine, 8).Value = "Total"
                    ws.Cell(LastLine, 8).Style.Font.Bold = True
                    RateSheet.Columns("B").SetDataType(XLCellValues.DateTime)
                    RateSheet.Columns("K").SetDataType(XLCellValues.DateTime)
                    RateSheet.Columns("M").SetDataType(XLCellValues.DateTime)
                    RateSheet.Columns("9:13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    RateSheet.Columns("J").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                    RateSheet.Columns("B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                    RateSheet.Columns("L").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                    RateSheet.Column("I").Style.NumberFormat.Format = decPlaces
                    ws.Cell(LastLine, 9).Value = dt.Compute("SUM(total_actualamount)", String.Empty)

                End If



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

#Region "Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged"
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
#End Region
    Protected Sub gvSupplierInvoice_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSupplierInvoice.PageIndexChanging
        gvSupplierInvoice.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub
#Region "Public Sub SortGridColumn()"
    Public Sub SortGridColumn()
        Dim DataTable As DataTable
        FillGridNew()
        DataTable = gvSupplierInvoice.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gvSupplierInvoice.DataSource = dataView
            gvSupplierInvoice.DataBind()
        End If
    End Sub
#End Region

    Protected Sub gvSupplierInvoice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSupplierInvoice.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblPInvoiceNo As Label
            lblPInvoiceNo = gvSupplierInvoice.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblPInvoiceNo")
            Dim strpop As String = ""
            If e.CommandName = "View" Then
                strpop = "window.open('UpdateSupplierInvoice.aspx?State=View&divid=" & ViewState("divcode") & "&ID=" & CType(lblPInvoiceNo.Text.Trim, String) & "','UpdateSupplierInvoice');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "UpdateSupplierInvoice", strpop, True)
            ElseIf e.CommandName = "EditRow" Then
                strpop = "window.open('UpdateSupplierInvoice.aspx?State=Edit&divid=" & ViewState("divcode") & "&ID=" & CType(lblPInvoiceNo.Text.Trim, String) & "','UpdateSupplierInvoice');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "UpdateSupplierInvoice", strpop, True)
            ElseIf e.CommandName = "PrintJournal" Then
                strpop = "window.open('PrintReport.aspx?printId=PurchaseInvoiceVoucher&InvoiceNo=" & CType(lblPInvoiceNo.Text.Trim, String) & "&divcode=" & ViewState("divcode") & "','InvoiceVoucherPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "PurchaseInvoiceVoucher", strpop, True)

            ElseIf e.CommandName = "Print" Then
                strpop = "window.open('PrintReport.aspx?printId=PurchaseInvoice&InvoiceNo=" & CType(lblPInvoiceNo.Text.Trim, String) & "&divcode=" & ViewState("divcode") & "','PurchaseInvoicePrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "PurchaseInvoice", strpop, True)
                'ElseIf e.CommandName = "PrintJournal" Then
                '    strpop = "window.open('PrintReport.aspx?printId=InvoiceVoucher&InvoiceNo=" & CType(lblInvoiceNo.Text.Trim, String) & "','InvoiceVoucherPrint');"
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "InvoiceVoucher", strpop, True)
                'ElseIf e.CommandName = "PrintProforma" Then
                '    Dim lblRequestId As Label
                '    lblRequestId = gvSalesInvoice.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblBookingNumber")
                '    strpop = "window.open('PrintReport.aspx?printId=bookingConfirmation&RequestId=" & CType(lblRequestId.Text.Trim, String) & "','ProformaPrint');"
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)
                'TAnvir 28092022
            ElseIf e.CommandName = "DeleteRow" Then
                strpop = "window.open('UpdateSupplierInvoice.aspx?State=Delete&divid=" & ViewState("divcode") & "&ID=" & CType(lblPInvoiceNo.Text.Trim, String) & "','UpdateSupplierInvoice');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "UpdateSupplierInvoice", strpop, True)
                'TAnvir 28092022
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvSupplierInvoice_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSupplierInvoice.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColumn()
    End Sub
#Region "Protected Sub btnUploadExcel_Click(sender As Object, e As System.EventArgs) Handles btnUploadExcel.Click"
    Protected Sub btnUploadExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadExcel.Click
        Dim strpop As String = ""
        strpop = "window.open('UpdateSupplierInvoiceExcel.aspx?State=New&divid=" & ViewState("divcode") & "' ,'SupplierInvoiceExcel');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
#End Region

End Class
