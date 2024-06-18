#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization
'Imports Ajax.AjaxJavaScriptHandler

#End Region
Partial Class ReceiptsAdjustBills
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
    Dim myDataAdapter1 As SqlDataAdapter
    Dim gvRow As GridViewRow
    Dim Flag As String = "0"
    Dim advbillcount As Integer
    Enum grd_col
        LienNo = 0
        chk = 1
        DocNo = 2
        DocType = 3
        RefNo = 4
        dDate = 5
        DueDate = 6
        ExchgRate = 7
        BalAmount = 8
        AmountToAdjust = 9
        baseAmount = 10
        Field2 = 11
        Field3 = 12
        Field4 = 13
        Field5 = 14
    End Enum
    Enum grd_col1
        LienNo = 0
        chk = 1
        DocNo = 2
        DocType = 3
        RefNo = 4
        dDate = 5
        DueDate = 6
        ExchgRate = 7
        BalAmount = 8
        AmountToAdjust = 9
        baseAmount = 10
        Field2 = 11
        Field3 = 12
        Field4 = 13
        Field5 = 14
    End Enum
    Enum grdAdv_col
        LienNo = 0
        DocNo = 1
        DDate = 2
        Doctype = 3
        RefNo = 4
        DueDate = 5
        Advpayamount = 6
        BaseAmount = 7
        Field2 = 8
        Field3 = 9
        Field4 = 10
        Field5 = 11
        chksel = 12
    End Enum
#End Region
#Region "NumbersHtml"
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        ' txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        ' txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
#Region "TextLockHtml"
    Public Sub TextLockHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region



    '<Ajax.AjaxMethod()>
    <System.Web.Services.WebMethod()>
    Public Shared Function DecRoundMethod(ByVal Ramt As Decimal, ByVal intmdecimal As Integer) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(intmdecimal, Integer), MidpointRounding.AwayFromZero)
        Return Rdamt
    End Function
    '<Ajax.AjaxMethod()>
    <System.Web.Services.WebMethod()>
    Public Shared Function DecRoundMethodtest(ByVal intmdecimal As String) As String
        Dim Rdamt As String = "test"
        ' Rdamt = Math.Round(Val(Ramt), CType(intmdecimal, Integer), MidpointRounding.AwayFromZero)
        Return Rdamt
    End Function

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState.Add("ReceiptsAdjustBillsState", Request.QueryString("State"))
        If Page.IsPostBack = False Then

            'Ajax.Utility.GenerateMethodScripts(Page)
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If
                txtconnection.Value = Session("dbconnectionName")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strdiv, strtrantype, strGridtype, glCode, strCDType, strtype, strAccCode, strLineNo, strCurrencyCode, strExchangeRate, strAmount, strBaseAmount, strOlineNo, strPersonCode, adjcolnostr, strrequestid, strtrandate As String
                '  strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)

                ViewState.Add("divcode", Request.QueryString("divid"))

                strdiv = ViewState("divcode")

                strtrantype = Request.QueryString("TranType")
                strGridtype = Request.QueryString("Gridtype")
                strtype = Request.QueryString("AccType")
                strAccCode = Request.QueryString("AccCode")
                strLineNo = Request.QueryString("lineNo")
                strOlineNo = Request.QueryString("OlineNo")
                strCurrencyCode = Request.QueryString("currcode")
                strExchangeRate = Request.QueryString("currrate") 'Math.Round(Val(Request.QueryString("currrate")), CType(txtdecimal.Value, Integer))
                strAmount = Request.QueryString("Amount")
                strBaseAmount = Request.QueryString("BaseAmount")
                glCode = Request.QueryString("ControlCode")
                txtGrdType.Value = Request.QueryString("Gridtype")
                strPersonCode = Request.QueryString("CNDNsperponcode")
                If Not Request.QueryString("Requestid") Is Nothing Then
                    strrequestid = Request.QueryString("Requestid")
                Else
                    strrequestid = ""
                End If
                strtrandate = Request.QueryString("trandate")


                ViewState.Add("ReceiptsAdjustBillsRefCode", Request.QueryString("RefCode"))

                If txtGrdType.Value = "Debit" Then
                    lblDrCrCaption.Text = "Dr"
                    strCDType = "D"
                ElseIf txtGrdType.Value = "Credit" Then
                    lblDrCrCaption.Text = "Cr"
                    strCDType = "C"
                End If
                Dim strOpti As String
                strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                lblBaseAmtCaption.Text = strOpti & " Amount"
                lblBaseAmountAdj.Text = strOpti & " Amount Adjusted"
                lblBaseAmtBalance.Text = strOpti & " Balance to Adjust"
                lblAmount.Text = strCurrencyCode & " Amount"
                lblAmountAdjusted.Text = strCurrencyCode & " Amount Adjusted"
                lblBalancetoAdjust.Text = strCurrencyCode & " Balance to Adjust"

                If strtype = "C" Then
                    txtAdAccountType.Value = "Customer"
                    'If Not Session("CNDNsperponcode") Is Nothing Then
                    '    txtSalesManCode.Value = CType(Session("CNDNsperponcode"), String)
                    'Else
                    '    txtSalesManCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"agentmast", "spersoncode", "agentcode", strAccCode)
                    'End If
                    'strPersonCode
                    If Not strPersonCode Is Nothing Then
                        txtSalesManCode.Value = CType(strPersonCode, String)
                    Else
                        txtSalesManCode.Value = objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "agentmast", "spersoncode", "agentcode", strAccCode, "divcode", ViewState("divcode"))
                    End If
                    txtSalesManName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "usermaster", "username", "usercode", txtSalesManCode.Value)
                ElseIf strtype = "S" Then
                    txtAdAccountType.Value = "Supplier"
                ElseIf strtype = "A" Then
                    txtAdAccountType.Value = "Supplier Agent"
                End If

                txtAccCode.Value = strAccCode
                ' txtAccName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "view_account", "des", "Code", strAccCode)
                txtAccName.Value = objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "view_account", "des", "type='" + strtype + "' and Code='" + strAccCode + "'", "div_code", ViewState("divcode"))
                txtCurrencyCode.Value = strCurrencyCode
                txtExchangeRate.Value = strExchangeRate
                txtADAmount.Value = DecRound(CType(Val(strAmount), Decimal))
                txtAmountAdjust.Value = 0
                txtBaseAmountAdjust.Value = 0
                txtBalAdu.Value = DecRound(CType(Val(strAmount), Decimal))

                txtBaseAmount.Value = DecRound(CType(Val(strBaseAmount), Decimal))


                Dim stragainst_tran_id, stragainst_tran_type As String
                If ViewState("ReceiptsAdjustBillsState") = "Edit" Or ViewState("ReceiptsAdjustBillsState") = "Delete" Or ViewState("ReceiptsAdjustBillsState") = "View" Then
                    stragainst_tran_id = ViewState("ReceiptsAdjustBillsRefCode")
                    stragainst_tran_type = strtrantype
                Else
                    stragainst_tran_id = "''"
                    stragainst_tran_type = ""
                End If
                Dim lineno As String
                If strOlineNo = "" Then
                    strOlineNo = strLineNo
                End If
                If CType(strLineNo, Decimal) = CType(strOlineNo, Decimal) Then
                    lineno = strLineNo
                Else
                    lineno = strOlineNo
                End If

                '' Added shahul 27/01/16
                advbillcount = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select count(*) from open_detail(nolock) where div_id='" & ViewState("divcode") & "' and against_tran_id='" & stragainst_tran_id & "' and against_tran_type='" & stragainst_tran_type & "' and open_mode <>'B'")

                FillAdjust(strdiv, strtype, stragainst_tran_id, lineno, stragainst_tran_type, glCode, strCDType, strrequestid)
                ' FillAdjust1(strdiv, strtype, stragainst_tran_id, lineno, stragainst_tran_type, glCode, strCDType, strrequestid)
                'Fillrequestlist()

                adjcolnostr = Request.QueryString("AdjColno")
                Dim intLineNo As Integer = 1
                Dim strLineKey As String
                strLineKey = "AgainstTranLineNo" & intLineNo & ":" & strLineNo
                fillDategrd(grdAdPay, True, 1)
                txtgrdAdpayRows.Value = grdAdPay.Rows.Count
                Dim collectionDate As Collection
                'If Not Session("Collection") Is Nothing Then
                '    If Session("Collection").ToString <> "" Then
                '        collectionDate = CType(Session("Collection"), Collection)
                '        If collectionDate.Count <> 0 Then
                '            If colexists(collectionDate, strLineKey) = True Then
                '                FillGrid()
                '            End If
                '        End If
                '    End If
                'End If
                collectionDate = GetCollectionFromSession()
                If collectionDate.Count <> 0 Then
                    If colexists(collectionDate, strLineKey) = True Then
                        FillGrid()
                    End If
                End If

                If collectionDate.Count = 0 Then
                    If strrequestid <> "" Then
                        hdnreqid.Value = strrequestid
                    End If
                End If


                disablegrid()

                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master where div_code='" & ViewState("divcode") & "'")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtSealDate.Value = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtSealDate.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If
                If Val(txtgrdAdjRows.Value) > 0 Then
                    'If strtrantype = "RV" Then
                    'grdAdjustBill.HeaderRow.Cells(11).Text = "Customer Ref.No"
                    'grdAdjustBill.HeaderRow.Cells(11).Text = objUtils.ExecuteQueryReturnStrinValuenew(Session("dbconnectionName"), "select field2 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='AB' and accttype='" & Request.QueryString("AccType") & "'")
                    'ElseIf strtrantype = "BPV" Or strtrantype = "CPV" Then
                    'grdAdjustBill.HeaderRow.Cells(11).Text = "Supplier Inv.No"
                    grdAdjustBill.HeaderRow.Cells(11).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field2 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='AB' and accttype='" & Request.QueryString("AccType") & "'")
                    'End If
                    grdAdjustBill.HeaderRow.Cells(4).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field1 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='AB' and accttype='" & Request.QueryString("AccType") & "'")
                    grdAdjustBill.HeaderRow.Cells(12).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field3 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='AB' and accttype='" & Request.QueryString("AccType") & "'")
                    grdAdjustBill.HeaderRow.Cells(13).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field4 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='AB' and accttype='" & Request.QueryString("AccType") & "'")
                    grdAdjustBill.HeaderRow.Cells(14).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field5 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='AB' and accttype='" & Request.QueryString("AccType") & "'")

                End If

                ''''''''''''Future Booking begin'''''''''''
                If Val(txtgrdAdjRows1.Value) > 0 Then
                    '   If strtrantype = "RV" Then
                    'grdFutureBooking.HeaderRow.Cells(11).Text = "Customer Ref.No"
                    'grdFutureBooking.HeaderRow.Cells(11).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field2 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='FB' and accttype='" & Request.QueryString("AccType") & "'")
                    'ElseIf strtrantype = "BPV" Or strtrantype = "CPV" Then
                    'grdFutureBooking.HeaderRow.Cells(11).Text = "Supplier Inv.No"
                    grdFutureBooking.HeaderRow.Cells(11).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field2 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='FB' and accttype='" & Request.QueryString("AccType") & "'")
                    'End If
                    grdFutureBooking.HeaderRow.Cells(4).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field1 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='FB' and accttype='" & Request.QueryString("AccType") & "'")
                    grdFutureBooking.HeaderRow.Cells(12).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field3 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='FB' and accttype='" & Request.QueryString("AccType") & "'")
                    grdFutureBooking.HeaderRow.Cells(13).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field4 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='FB' and accttype='" & Request.QueryString("AccType") & "'")
                    grdFutureBooking.HeaderRow.Cells(14).Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select field5 from adjust_bill_head(nolock) where div_id='" & ViewState("divcode") & "' and adjusttype='FB' and accttype='" & Request.QueryString("AccType") & "'")
                End If
                ''''''''''Future Booking end ''''''''''''''

                btnOk.Attributes.Add("onclick", "OnClickOk()")
                btnAExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReceiptsAdjustBills.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            btnOk.Attributes.Add("onclick", "OnClickOk()")
            btnAExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

        End If

    End Sub
    Private Function GetCollectionFromSession() As Collection
        Dim adjcolnostr As String
        adjcolnostr = Request.QueryString("AdjColno")
        Dim collectionDate1 As New Collection
        If Not Session("Collection" & ":" & adjcolnostr) Is Nothing Then
            If Session("Collection" & ":" & adjcolnostr).ToString <> "" Then
                collectionDate1 = CType(Session("Collection" & ":" & adjcolnostr), Collection)
            End If
        End If
        Return collectionDate1
    End Function
    Private Function GetAdvRowsCountFromSession() As Long
        Dim adjcolnostr As String
        adjcolnostr = Request.QueryString("AdjColno")
        Dim rcnt As Long
        rcnt = 0
        If Not Session("AdBillGridCount" & ":" & adjcolnostr) Is Nothing Then
            If Session("AdBillGridCount" & ":" & adjcolnostr).ToString <> "" Then
                rcnt = CType(Session("AdBillGridCount" & ":" & adjcolnostr), Long)
            End If
        End If
        Return rcnt
    End Function
    Private Sub FillAdjust(ByVal strdiv As String, ByVal strtype As String, ByVal stragainst_tran_id As String, ByVal strLineNo As Long, ByVal stragainst_tran_type As String, ByVal glCode As String, ByVal strCDType As String, ByVal strrequestid As String, Optional ByVal searchno As String = "", Optional ByVal searchType As Integer = 0)
        Dim myDS As New DataSet
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        If ViewState("ReceiptsAdjustBillsState") = "Edit" Or ViewState("ReceiptsAdjustBillsState") = "Delete" Or ViewState("ReceiptsAdjustBillsState") = "View" Then
            strSqlQry = "exec sp_getadjust_new '" & txtAccCode.Value & "','" & strtype & "','" & stragainst_tran_id & "'," & strLineNo & ",'" & stragainst_tran_type & "','" & glCode & "','" & strCDType & "','" & strdiv & "','" & strrequestid & "'"
        Else
            strSqlQry = "exec sp_getadjust_new'" & txtAccCode.Value & "','" & strtype & "','',null,null,'" & glCode & "','" & strCDType & "','" & strdiv & "','" & strrequestid & "'"
        End If
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.SelectCommand.CommandTimeout = 0
        myDataAdapter.Fill(myDS, "AdjustBill")
        'New Account Code Record Add In Last  
        myDS.Tables(0).DefaultView.RowFilter = "accmode='B'"
        'If searchno.Length > 0 Then
        '    If ddlABSearchNo.Value = 1 Then
        '        Dim dView As DataView = New DataView(myDS.Tables("AdjustBill"))
        '        dView.RowFilter = "DocNo like '%" & txtABSearchNo.Value & "%'"
        '        grdAdjustBill.DataSource = dView
        '    ElseIf ddlABSearchNo.Value = 2 Then
        '        Dim dView As DataView = New DataView(myDS.Tables("AdjustBill"))
        '        dView.RowFilter = "field1 like '%" & txtABSearchNo.Value & "%'"
        '        grdAdjustBill.DataSource = dView
        '    Else
        '        grdAdjustBill.DataSource = myDS.Tables("AdjustBill")
        '    End If
        'Else
        grdAdjustBill.DataSource = myDS.Tables("AdjustBill")
        'End If

        grdAdjustBill.DataBind()
        ''Tanvir 18092023 cr point6
        myDS.Tables(0).DefaultView.RowFilter = "accmode='F'"
        grdFutureBooking.DataSource = myDS.Tables("FutureBooking")
        grdFutureBooking.DataBind()
        ' ''Tanvir 18092023
        SqlConn.Close()
        txtgrdAdjRows1.Value = grdFutureBooking.Rows.Count         'Tanvir 18092023 
        txtgrdAdjRows.Value = grdAdjustBill.Rows.Count
        SqlConn.Close()
        txtgrdAdjRows.Value = grdAdjustBill.Rows.Count

    End Sub
    Private Sub FillAdjust1(ByVal strdiv As String, ByVal strtype As String, ByVal stragainst_tran_id As String, ByVal strLineNo As Long, ByVal stragainst_tran_type As String, ByVal glCode As String, ByVal strCDType As String, ByVal strrequestid As String, Optional ByVal searchno As String = "", Optional ByVal searchType As Integer = 0)
        Dim myDS As New DataSet
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        If ViewState("ReceiptsAdjustBillsState") = "Edit" Or ViewState("ReceiptsAdjustBillsState") = "Delete" Or ViewState("ReceiptsAdjustBillsState") = "View" Then
            strSqlQry = "exec sp_getadjust '" & txtAccCode.Value & "','" & strtype & "','" & stragainst_tran_id & "'," & strLineNo & ",'" & stragainst_tran_type & "','" & glCode & "','" & strCDType & "','" & strdiv & "','" & strrequestid & "'"
        Else
            strSqlQry = "exec sp_getadjust '" & txtAccCode.Value & "','" & strtype & "','',null,null,'" & glCode & "','" & strCDType & "','" & strdiv & "','" & strrequestid & "'"
        End If
        myDataAdapter1 = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter1.SelectCommand.CommandTimeout = 0
        myDataAdapter1.Fill(myDS, "FutureBooking")
        'New Account Code Record Add In Last  
        myDS.Tables(0).DefaultView.RowFilter = "accmode='F'"
        'If searchno.Length > 0 Then
        '    If ddlABSearchNo.Value = 1 Then
        '        Dim dView As DataView = New DataView(myDS.Tables("AdjustBill"))
        '        dView.RowFilter = "DocNo like '%" & txtABSearchNo.Value & "%'"
        '        grdAdjustBill.DataSource = dView
        '    ElseIf ddlABSearchNo.Value = 2 Then
        '        Dim dView As DataView = New DataView(myDS.Tables("AdjustBill"))
        '        dView.RowFilter = "field1 like '%" & txtABSearchNo.Value & "%'"
        '        grdAdjustBill.DataSource = dView
        '    Else
        '        grdAdjustBill.DataSource = myDS.Tables("AdjustBill")
        '    End If
        'Else
        grdFutureBooking.DataSource = myDS.Tables("FutureBooking")
        'End If

        grdFutureBooking.DataBind()
        SqlConn.Close()
        txtgrdAdjRows1.Value = grdFutureBooking.Rows.Count


    End Sub

    Private Sub Fillrequestlist()
        Dim ds As DataSet = New DataSet()
        Dim parms As New List(Of SqlParameter)
        Dim parm(1) As SqlParameter

        parm(0) = New SqlParameter("@agentcode", txtAccCode.Value)
        parms.Add(parm(0))

        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_request_list", parms)
        If ds.Tables(0).Rows.Count > 0 Then
            GVCollection.DataSource = ds.Tables(0)
            GVCollection.DataBind()
            GVCollection.Visible = True
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
        txtGrdType.Value = Request.QueryString("Gridtype")

    End Sub
#End Region

#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If
        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("LineNo", GetType(Integer)))

        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region
#Region "Protected Sub grdAdjustBill_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAdjustBill.RowDataBound"
    Protected Sub grdAdjustBill_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAdjustBill.RowDataBound
        Dim txtAdujAmt, txtCurRate, txtBalAmount, txtBaseAmount As HtmlInputText
        Dim chk As HtmlInputCheckBox
        Dim strOpti As String
        gvRow = e.Row
        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.baseAmount).Text = strOpti & " Amount"
            Exit Sub
        End If

        'If e.Row.RowType = DataControlRowType.Header Then
        '    grdAdjustBill.HeaderRow.Cells(11).Text = "Customer Ref.no"
        'End If

        Dim mCurrCode As String = e.Row.DataItem("currcode")
        If Request.QueryString("currcode") = mCurrCode Then
            Flag = "1"
        Else
            Flag = "0"
        End If

        txtCurRate = gvRow.FindControl("txtCurRate")
        txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
        chk = gvRow.FindControl("chkBill")
        txtBalAmount = gvRow.FindControl("txtBalAmount")
        txtBaseAmount = gvRow.FindControl("txtBaseAmount")
        Dim hdnCurrcode As HiddenField = CType(gvRow.FindControl("hdnCurrCode"), HiddenField)

        hdnchkid.Value = "adjustBillTab_TabPanel1_grdAdjustBill_ctl02_chkBill"

        txtAdujAmt.Attributes.Add("onchange", "javascript:AdjestBillChange('" + CType(txtAdujAmt.ClientID, String) + "','" + CType(chk.ClientID, String) + "','" + CType(txtBalAmount.ClientID, String) + "','" + CType(txtCurRate.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "')")

        NumbersHtml(txtAdujAmt)

        'Commented by Ram 21-09-2023
        'Dim baseconvrate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  convrate from   currrates where tocurr=(select option_selected from reservation_parameters where param_id=457) and currcode='" + hdnCurrcode.Value + "'")
        Dim hdnbaseconrate As HiddenField = CType(gvRow.FindControl("hdnbaseconrate"), HiddenField) ' Added by Ram 21-09-2023



        'chk.Attributes.Add("OnClick", "javascript:OnchangeChk('" + CType(txtAdujAmt.ClientID, String) + "','" + CType(chk.ClientID, String) + "','" + CType(txtBalAmount.ClientID, String) + "','" + CType(txtCurRate.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "','" + hdnCurrcode.Value + "','" + baseconvrate + "' )")
        'Added by Ram 21-09-2023
        chk.Attributes.Add("OnClick", "javascript:OnchangeChk('" + CType(txtAdujAmt.ClientID, String) + "','" + CType(chk.ClientID, String) + "','" + CType(txtBalAmount.ClientID, String) + "','" + CType(txtCurRate.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "','" + hdnCurrcode.Value + "','" + hdnbaseconrate.Value + "' )")



        TextLockHtml(txtBalAmount)
        TextLockHtml(txtBaseAmount)
        TextLockHtml(txtCurRate)
        If Val(txtAdujAmt.Value) = 0 Then
            txtAdujAmt.Value = ""
            txtAdujAmt.Style.Add("readonly", "readonly")
        End If
    End Sub
#End Region
#Region "Protected Sub grdAdPay_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAdPay.RowDataBound"
    Protected Sub grdAdPay_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAdPay.RowDataBound
        Dim strOpti As String
        gvRow = e.Row
        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grdAdv_col.BaseAmount).Text = strOpti & " Amount"
            Exit Sub
        End If
        Dim strGridtype = Request.QueryString("Gridtype")
        Dim txtAdvPayAmount, txtBaseAmount As HtmlInputText
        Dim hdnADocNo As HiddenField
        Dim txtADocNo As HtmlInputText
        txtADocNo = gvRow.FindControl("txtADocNo")
        txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
        hdnADocNo = gvRow.FindControl("hdnADocNo")
        NumbersHtml(txtAdvPayAmount)
        txtBaseAmount = gvRow.FindControl("txtBaseAmount")
        TextLockHtml(txtBaseAmount)
        txtAdvPayAmount.Attributes.Add("onchange", "javascript:OnChangeAdPay('" + CType(txtAdvPayAmount.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(txtExchangeRate.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "')")
        txtADocNo.Attributes.Add("onchange", "javascript:OnChangeAdvanceno('" + CType(txtADocNo.ClientID, String) + "','" + CType(hdnADocNo.ClientID, String) + "')")
    End Sub
#End Region

#Region "Protected Sub grdFutureBooking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFutureBooking.RowDataBound"
    Protected Sub grdFutureBooking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFutureBooking.RowDataBound
        Dim txtAdujAmt1, txtCurRate1, txtBalAmount1, txtBaseAmount1 As HtmlInputText
        Dim chk As HtmlInputCheckBox
        Dim strOpti As String
        gvRow = e.Row
        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.baseAmount).Text = strOpti & " Amount"
            Exit Sub
        End If

        'If e.Row.RowType = DataControlRowType.Header Then
        '    grdAdjustBill.HeaderRow.Cells(11).Text = "Customer Ref.no"
        'End If

        Dim mCurrCode As String = e.Row.DataItem("currcode")
        If Request.QueryString("currcode") = mCurrCode Then
            Flag = "1"
        Else
            Flag = "0"
        End If

        txtCurRate1 = gvRow.FindControl("txtCurRate1")
        txtAdujAmt1 = gvRow.FindControl("txtAdujustAmt1")
        chk = gvRow.FindControl("chkBill1")
        txtBalAmount1 = gvRow.FindControl("txtBalAmount1")
        txtBaseAmount1 = gvRow.FindControl("txtBaseAmount1")
        Dim hdnCurrcode As HiddenField = CType(gvRow.FindControl("hdnCurrCode1"), HiddenField)

        hdnchkid.Value = "adjustBillTab_TabPanel1_grdFutureBooking_ctl02_chkBill"

        txtAdujAmt1.Attributes.Add("onchange", "javascript:AdjestBillChange('" + CType(txtAdujAmt1.ClientID, String) + "','" + CType(chk.ClientID, String) + "','" + CType(txtBalAmount1.ClientID, String) + "','" + CType(txtCurRate1.ClientID, String) + "','" + CType(txtBaseAmount1.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "')")

        NumbersHtml(txtAdujAmt1)

        ' Dim baseconvrate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  convrate from   currrates where tocurr=(select option_selected from reservation_parameters where param_id=457) and currcode='" + hdnCurrcode.Value + "'")
        Dim hdnbaseconrate As HiddenField = CType(gvRow.FindControl("hdnbaseconrate1"), HiddenField) ' Added by Ram 21-09-2023



        ' chk.Attributes.Add("OnClick", "javascript:OnchangeChk('" + CType(txtAdujAmt1.ClientID, String) + "','" + CType(chk.ClientID, String) + "','" + CType(txtBalAmount1.ClientID, String) + "','" + CType(txtCurRate1.ClientID, String) + "','" + CType(txtBaseAmount1.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "','" + hdnCurrcode.Value + "','" + baseconvrate + "' )")

        'Added by Ram 21-09-2023
        chk.Attributes.Add("OnClick", "javascript:OnchangeChk('" + CType(txtAdujAmt1.ClientID, String) + "','" + CType(chk.ClientID, String) + "','" + CType(txtBalAmount1.ClientID, String) + "','" + CType(txtCurRate1.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "','" + hdnCurrcode.Value + "','" + hdnbaseconrate.Value + "' )")



        TextLockHtml(txtBalAmount1)
        TextLockHtml(txtBaseAmount1)
        TextLockHtml(txtCurRate1)
        If Val(txtAdujAmt1.Value) = 0 Then
            txtAdujAmt1.Value = ""
            txtAdujAmt1.Style.Add("readonly", "readonly")
        End If
    End Sub
#End Region

#Region "Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean"
    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
        Try
            Dim k As Integer
            colexists = False
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
                    If newcol.Contains(newkey) = True Then
                        colexists = True
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            colexists = False
        End Try
    End Function
#End Region
#Region "Private Function coltranlineno(ByVal newcol As Collection, ByVal newkey As String) As string"
    Private Function coltranlineno(ByVal newcol As Collection, ByVal newkey As String) As String
        Try
            Dim k As Integer
            coltranlineno = ""
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
                    'If newcol.Contains(newkey) = True Then
                    If newcol.Item(k) = newkey Then
                        coltranlineno = "1"  'TranId
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            coltranlineno = ""
        End Try
    End Function
#End Region
#Region "Public Function validate_page() As Boolean"
    Public Function validate_page() As Boolean
        validate_page = True
        Dim txtADocNo, txtADocType, txtRefNo, txtAdvPayAmount, txtBaseAmount, txtField2, txtField3, txtField4, txtField5 As HtmlInputText
        Dim txtAdujAmt As HtmlInputText
        Dim chk As HtmlInputCheckBox
        Dim i As Integer
        'If Val(txtAmountAdjust.Value) <= 0 Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please adjust some amount.');", True)
        '    SetFocus(txtAmountAdjust)
        '    validate_page = False
        '    Exit Function
        'End If


        i = 0
        For Each gvRow In GVCollection.Rows
            Dim chksel As CheckBox = CType(gvRow.FindControl("chkSel"), CheckBox)
            If chksel.Checked = True Then
                i = i + 1
            End If
        Next

        If i > 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot select more than one request .');", True)
            validate_page = False
            Exit Function
        End If

        For Each gvRow In grdAdjustBill.Rows
            chk = gvRow.FindControl("chkBill")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
            If chk.Checked = True Then
                If Val(txtAdujAmt.Value) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter the adjust amount .');", True)
                    SetFocus(txtAdujAmt)
                    validate_page = False
                    Exit Function
                End If
            End If
        Next

        '' Future Booking begin
        For Each gvRow In grdFutureBooking.Rows
            chk = gvRow.FindControl("chkBill1")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt1")
            If chk.Checked = True Then
                If Val(txtAdujAmt.Value) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter the adjust amount .');", True)
                    SetFocus(txtAdujAmt)
                    validate_page = False
                    Exit Function
                End If
            End If
        Next


        '' Future Booking end

        Dim txtDate, txtDueDate As TextBox

        For Each gvRow In grdAdPay.Rows
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
            txtBaseAmount = gvRow.FindControl("txtBaseAmount")
            txtDate = gvRow.FindControl("txtDate")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")

            If txtADocNo.Value.Trim <> "" Or txtADocType.Value.Trim <> "" Or txtBaseAmount.Value.Trim <> "" Then
                'If txtADocNo.Value.Trim = "" Or txtADocType.Value.Trim = "" Or txtBaseAmount.Value.Trim = "" Then
                If txtADocNo.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter document no.');", True)
                    SetFocus(txtADocNo)
                    validate_page = False
                    Exit Function
                End If

                If txtADocType.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter document Type.');", True)
                    SetFocus(txtADocType)
                    validate_page = False
                    Exit Function
                End If

                If txtDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter date.');", True)
                    SetFocus(txtDate)
                    validate_page = False
                    Exit Function
                End If

                If txtDueDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter due date.');", True)
                    SetFocus(txtDueDate)
                    validate_page = False
                    Exit Function
                End If

                If Val(txtAdvPayAmount.Value) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Advance payment amount.');", True)
                    SetFocus(txtAdvPayAmount)
                    validate_page = False
                    Exit Function
                End If
                Dim dFromDate, dToDate As Date
                dFromDate = objDateTime.ConvertDateromTextBoxToDatabase(txtDate.Text)
                dToDate = objDateTime.ConvertDateromTextBoxToDatabase(txtDueDate.Text)

                If dFromDate <= dToDate Then

                Else
                    validate_page = False
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select due dates should be greater than date.');", True)
                    SetFocus(dToDate)
                    Exit Function
                End If


                Dim strType As String = Request.QueryString("AccType")
                Dim intReceiptLinno As Integer = CType(Request.QueryString("lineNo"), Integer)
                Dim strAccCode = Request.QueryString("AccCode")

                Dim Alflg As Integer
                Dim ErrMsg, strdiv As String
                If ViewState("divcode") Is Nothing Then
                    strdiv = Request.QueryString("divid")  ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                Else
                    strdiv = ViewState("divcode")
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand("sp_Check_Duplicate_refnos", SqlConn)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtADocNo.Value
                myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = txtADocType.Value
                myCommand.Parameters.Add(New SqlParameter("@tran_lineno ", SqlDbType.Int)).Value = intReceiptLinno
                myCommand.Parameters.Add(New SqlParameter("@acc_type ", SqlDbType.VarChar, 10)).Value = strType
                myCommand.Parameters.Add(New SqlParameter("@acc_code ", SqlDbType.VarChar, 10)).Value = strAccCode

                Dim nparam1 As SqlParameter
                Dim nparam2 As SqlParameter
                nparam1 = New SqlParameter
                nparam1.ParameterName = "@allowflg"
                nparam1.Direction = ParameterDirection.Output
                nparam1.DbType = DbType.Int16
                nparam1.Size = 9
                myCommand.Parameters.Add(nparam1)
                nparam2 = New SqlParameter
                nparam2.ParameterName = "@errmsg"
                nparam2.Direction = ParameterDirection.Output
                nparam2.DbType = DbType.String
                nparam2.Size = 200
                myCommand.Parameters.Add(nparam2)
                myDataAdapter = New SqlDataAdapter(myCommand)
                myCommand.ExecuteNonQuery()

                Alflg = nparam1.Value
                ErrMsg = nparam2.Value

                If Alflg = 1 And ErrMsg <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                    validate_page = False
                    Exit Function
                End If

            End If

        Next
    End Function
#End Region


    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            For Each row As GridViewRow In grdAdPay.Rows
                Dim txtDate As TextBox = DirectCast(row.FindControl("txtDate"), TextBox)
                If txtDate.Text <> "" And txtSealDate.Value <> "" Then
                    invdate = DateTime.Parse(txtDate.Text, MyCultureInfo, DateTimeStyles.None)
                    sealdate = DateTime.Parse(txtSealDate.Value, MyCultureInfo, DateTimeStyles.None)
                    If invdate <= sealdate Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                        Validateseal = False
                    End If
                End If
            Next


        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Protected Sub btnRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs)  "
    Protected Sub btnRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecalculate.Click
        Dim chk As HtmlInputCheckBox
        Dim txtAdujAmt, txtCurRate, btxtBaseAmount As HtmlInputText
        Dim lblDoNo, lblDocType, lblAccTranLineNo, lblrefno, lbltktno As Label
        Dim baseamount As Decimal = 0
        Dim totalbaseamount As Decimal = 0
        Dim Advbaseamount As Decimal = 0
        Dim txtADocNo, txtADocType, txtRefNo, txtAdvPayAmount, txtAdvBaseAmount As HtmlInputText
        For Each gvRow In grdAdjustBill.Rows
            chk = gvRow.FindControl("chkBill")
            lblAccTranLineNo = gvRow.FindControl("lblAccTranLineNo")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
            lblDoNo = gvRow.FindControl("lblDoNo")
            lblDocType = gvRow.FindControl("lblDocType")
            lblrefno = gvRow.FindControl("lblRefNo")
            lbltktno = gvRow.FindControl("lbltktno")
            txtCurRate = gvRow.FindControl("txtCurRate")
            btxtBaseAmount = gvRow.FindControl("txtBaseAmount")

            If chk.Checked = True Then
                baseamount = Val(txtCurRate.Value) * Val(txtAdujAmt.Value)
                btxtBaseAmount.Value = DecRound(baseamount)
                totalbaseamount = totalbaseamount + DecRound(baseamount)
            End If
        Next

        For Each gvRow In grdAdPay.Rows
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
            txtAdvBaseAmount = gvRow.FindControl("txtBaseAmount")

            If txtADocNo.Value.Trim <> "" And txtADocType.Value.Trim <> "" And txtAdvBaseAmount.Value.Trim <> "" Then
                Advbaseamount = Val(txtAdvBaseAmount.Value)
                totalbaseamount = totalbaseamount + Advbaseamount
            End If
        Next

        txtBaseAmountAdjust.Value = DecRound(totalbaseamount)

        '  txtBalanceAdjinbase.value = parseFloat(txtHeaderbaseAmt.value) - parseFloat(txtrbaseAmtAdj.value);
        txtBalAduinBase.Value = DecRound(Val(txtBaseAmount.Value) - Val(txtBaseAmountAdjust.Value))

    End Sub
#End Region
    Protected Sub btnadvance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadvance.Click
        '   validate_BillAgainst_previous(hdnadvancelastno.Value)
        'Sharfudeen 31/07/2023
        If validate_BillAgainst_previous(hdnadvancelastno.Value) = False Then
            Dim txtADocNo As HtmlInputText
            Dim hdnADocNo As HiddenField

            For Each gvRow In grdAdPay.Rows
                txtADocNo = gvRow.FindControl("txtADocNo")
                hdnADocNo = gvRow.FindControl("hdnADocNo")
                If hdnadvancelastno.Value = hdnADocNo.Value Then
                    txtADocNo.Value = hdnADocNo.Value
                    Exit For
                End If
            Next
            Exit Sub
        End If
    End Sub
#Region "Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs)  "
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim clAdBill As New Collection

        Dim strtrantype = Request.QueryString("TranType")
        Dim strType As String = Request.QueryString("AccType")
        Dim intReceiptLinno As Integer = CType(Request.QueryString("lineNo"), Integer)
        Dim strTranId As String = Request.QueryString("TranId")
        Dim strGridtype = Request.QueryString("Gridtype")
        Dim strAccCode = Request.QueryString("AccCode")
        Dim strglCode = Request.QueryString("ControlCode")
        Dim strBaseAmount As String = Request.QueryString("BaseAmount")
        Dim intLineNo As Integer = 1
        Dim strLineKey As String

        'If Val(txtBalAdu.Value) = 0 And Val(txtBalAduinBase.Value) <> 0 Then
        '    btnRecalculate_Click(sender, e)
        'End If

        btnRecalculate_Click(sender, e)

        If validate_page() = False Then
            Exit Sub
        End If


        'Sharfudeen 31/07/2023
        ' Dim ids As String
        'ids = hdnadvancepayment.Value
        'If ids.Length > 0 Then

        '    ids = Mid(ids, 2, ids.Length - 1)
        '    Dim strId As String()
        '    strId = ids.Split(",")
        '    For j As Integer = 0 To strId.Length - 1
        '        Dim advno As String = strId(j)
        '        If validate_BillAgainst_previous(advno) = False Then
        '            Exit Sub
        '        End If
        '    Next
        'End If

        'Sharfudeen 31/07/2023
        Dim txtADocNo1 As HtmlInputText
        Dim hdnADocNo As HiddenField
        For Each gvRow In grdAdPay.Rows
            txtADocNo1 = gvRow.FindControl("txtADocNo")
            hdnADocNo = gvRow.FindControl("hdnADocNo")
            If txtADocNo1.Value <> hdnADocNo.Value Then
                If validate_BillAgainst_previous(txtADocNo1.Value) = False Then
                    Exit Sub
                End If
                Exit For
            End If
        Next



        If Validateseal() = False Then
            Exit Sub
        End If
        'If Not Session("Collection") Is Nothing Then
        'If Session("Collection").ToString <> "" Then
        'clAdBill = CType(Session("Collection"), Collection)
        ' End If
        'End If
        clAdBill = GetCollectionFromSession()
        If Val(txtBalAdu.Value) <> 0 Then

            If txtflag.Value = 1 Then

                Dim optionval As String

                optionval = objUtils.GetAutoDocNoWTnew(Session("dbconnectionName"), "ADVANCE")

                strLineKey = intLineNo & ":" & intReceiptLinno
                AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, intReceiptLinno) 'intLineNo.ToString)
                AddCollection(clAdBill, "AccTranLineNo" & strLineKey, intLineNo.ToString)
                AddCollection(clAdBill, "TranId" & strLineKey, optionval)
                AddCollection(clAdBill, "TranDate" & strLineKey, Request.QueryString("trandate")) 'Format(CType(objDateTime.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy"))
                AddCollection(clAdBill, "TranType" & strLineKey, strtrantype) '"RV")
                AddCollection(clAdBill, "DueDate" & strLineKey, Format(CType(objDateTime.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy"))

                If Val(txtAmountAdjust.Value) > 0 Then
                    txtExchangeRate.Value = CType(DecRound(txtBaseAmountAdjust.Value) / DecRound(txtAmountAdjust.Value), Decimal)
                End If

                AddCollection(clAdBill, "CurrRate" & strLineKey, CType(Val(txtExchangeRate.Value), Decimal))
                Dim baseamt As Decimal
                baseamt = DecRound(DecRound(CType(Val(txtBalAdu.Value), Decimal)) * CType(Val(txtExchangeRate.Value), Decimal))
                '---------------------------- for adjusting conv rate for advance payment
                'txtExchangeRate.Value = CType(DecRound(txtBalAdu.Value) / DecRound(baseamt), Decimal)
                'baseamt = DecRound(DecRound(CType(Val(txtBalAdu.Value), Decimal)) * CType(Val(txtExchangeRate.Value), Decimal))
                '------------------------------end

                If strGridtype = "Credit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, DecRound(CType(Val(txtBalAdu.Value), Decimal)))
                    AddCollection(clAdBill, "Debit" & strLineKey, 0)
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, DecRound(CType(Val(baseamt), Decimal)))
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, 0)

                    txtAmountAdjust.Value = DecRound(txtAmountAdjust.Value) + DecRound(txtBalAdu.Value)
                    txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(baseamt)
                ElseIf strGridtype = "Debit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, 0)
                    AddCollection(clAdBill, "Debit" & strLineKey, DecRound(CType(Val(txtBalAdu.Value), Decimal)))
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, 0)
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, DecRound(CType(Val(baseamt), Decimal)))

                    txtAmountAdjust.Value = DecRound(txtAmountAdjust.Value) + DecRound(txtBalAdu.Value)
                    txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(baseamt)
                End If
                AddCollection(clAdBill, "RefNo" & strLineKey, "")
                AddCollection(clAdBill, "Field2" & strLineKey, "")
                AddCollection(clAdBill, "Field3" & strLineKey, "")
                AddCollection(clAdBill, "Field4" & strLineKey, "")
                AddCollection(clAdBill, "Field5" & strLineKey, "")
                AddCollection(clAdBill, "OpenMode" & strLineKey, "D")

                AddCollection(clAdBill, "AccType" & strLineKey, strType)
                AddCollection(clAdBill, "AccCode" & strLineKey, strAccCode)
                AddCollection(clAdBill, "AccGLCode" & strLineKey, strglCode)
                AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(CType(Val(strBaseAmount), Decimal)))


                intLineNo = 2
            End If
        End If


        Dim chk As HtmlInputCheckBox
        Dim txtAdujAmt, txtCurRate, btxtBaseAmount As HtmlInputText
        Dim lblDoNo, lblDocType, lblAccTranLineNo, lblrefno, lbltktno As Label
        Dim lblDoNo1, lblDocType1, lblAccTranLineNo1, lblrefno1, lbltktno1 As Label
        Dim i, ctcount As Long
        ctcount = clAdBill.Count / 21
        For i = 1 To ctcount
            strLineKey = i & ":" & intReceiptLinno
            If colexists(clAdBill, "AgainstTranLineNo" & strLineKey) = True Then
                If clAdBill("OpenMode" & strLineKey).ToString() = "B" Then
                    DeleteCollection(clAdBill, "AgainstTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "AccTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "TranId" & strLineKey)
                    DeleteCollection(clAdBill, "TranDate" & strLineKey)
                    DeleteCollection(clAdBill, "TranType" & strLineKey)
                    DeleteCollection(clAdBill, "DueDate" & strLineKey)
                    DeleteCollection(clAdBill, "CurrRate" & strLineKey)
                    DeleteCollection(clAdBill, "Credit" & strLineKey)
                    DeleteCollection(clAdBill, "Debit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseCredit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseDebit" & strLineKey)
                    DeleteCollection(clAdBill, "RefNo" & strLineKey)
                    DeleteCollection(clAdBill, "Field2" & strLineKey)
                    DeleteCollection(clAdBill, "Field3" & strLineKey)
                    DeleteCollection(clAdBill, "Field4" & strLineKey)
                    DeleteCollection(clAdBill, "Field5" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AccType" & strLineKey)
                    DeleteCollection(clAdBill, "AccCode" & strLineKey)
                    DeleteCollection(clAdBill, "AccGLCode" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AdjustBaseTotal" & strLineKey)
                End If
            End If
        Next

        Dim againstbillno As String = "" '' Added shahul 03/07/2018


        For Each gvRow In grdAdjustBill.Rows
            chk = gvRow.FindControl("chkBill")
            lblAccTranLineNo = gvRow.FindControl("lblAccTranLineNo")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
            lblDoNo = gvRow.FindControl("lblDoNo")
            lblDocType = gvRow.FindControl("lblDocType")
            lblrefno = gvRow.FindControl("lblRefNo")
            lbltktno = gvRow.FindControl("lbltktno")

            txtCurRate = gvRow.FindControl("txtCurRate")
            btxtBaseAmount = gvRow.FindControl("txtBaseAmount")

            If chk.Checked = True Then
                'btxtBaseAmount.Value = DecRound(CType(Val(txtAdujAmt.Value), Decimal) * CType(Val(txtCurRate.Value), Decimal))

                strLineKey = intLineNo & ":" & intReceiptLinno
                AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, intReceiptLinno) ' intLineNo.ToString)
                AddCollection(clAdBill, "AccTranLineNo" & strLineKey, lblAccTranLineNo.Text.Trim)
                AddCollection(clAdBill, "TranId" & strLineKey, lblDoNo.Text)
                againstbillno = againstbillno + "," + Trim(lblDoNo.Text)  '' Added shahul 03/07/2018
                AddCollection(clAdBill, "TranDate" & strLineKey, gvRow.Cells(grd_col.dDate).Text)
                AddCollection(clAdBill, "TranType" & strLineKey, lblDocType.Text)
                If gvRow.Cells(grd_col.DueDate).Text = "&nbsp;" Then
                    AddCollection(clAdBill, "DueDate" & strLineKey, gvRow.Cells(grd_col.dDate).Text)
                Else
                    AddCollection(clAdBill, "DueDate" & strLineKey, gvRow.Cells(grd_col.DueDate).Text)
                End If
                ' AddCollection(clAdBill, "CurrRate" & strLineKey, Val(gvRow.Cells(grd_col.ExchgRate).Text))
                AddCollection(clAdBill, "CurrRate" & strLineKey, txtCurRate.Value)
                If strGridtype = "Credit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, IIf(txtAdujAmt.Value > 0, DecRound(Math.Abs(CType(txtAdujAmt.Value, Decimal))), 0))
                    AddCollection(clAdBill, "Debit" & strLineKey, IIf(txtAdujAmt.Value < 0, DecRound(Math.Abs(CType(txtAdujAmt.Value, Decimal))), 0))
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(btxtBaseAmount.Value > 0, DecRound(Math.Abs(CType(btxtBaseAmount.Value, Decimal))), 0))
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(btxtBaseAmount.Value < 0, DecRound(Math.Abs(CType(btxtBaseAmount.Value, Decimal))), 0))
                ElseIf strGridtype = "Debit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, IIf(txtAdujAmt.Value < 0, Math.Abs(CType(txtAdujAmt.Value, Decimal)), 0))
                    AddCollection(clAdBill, "Debit" & strLineKey, IIf(txtAdujAmt.Value > 0, Math.Abs(CType(txtAdujAmt.Value, Decimal)), 0))
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(btxtBaseAmount.Value < 0, Math.Abs(CType(btxtBaseAmount.Value, Decimal)), 0))
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(btxtBaseAmount.Value > 0, Math.Abs(CType(btxtBaseAmount.Value, Decimal)), 0))
                End If
                AddCollection(clAdBill, "RefNo" & strLineKey, lblrefno.Text) 'gvRow.Cells(grd_col.RefNo).Text.Trim)
                AddCollection(clAdBill, "Field2" & strLineKey, lbltktno.Text) ' gvRow.Cells(grd_col.Field2).Text.Trim)
                AddCollection(clAdBill, "Field3" & strLineKey, gvRow.Cells(grd_col.Field3).Text.Trim)
                AddCollection(clAdBill, "Field4" & strLineKey, gvRow.Cells(grd_col.Field4).Text.Trim)
                AddCollection(clAdBill, "Field5" & strLineKey, gvRow.Cells(grd_col.Field5).Text.Trim)
                AddCollection(clAdBill, "OpenMode" & strLineKey, "B")
                AddCollection(clAdBill, "AccType" & strLineKey, strType)
                AddCollection(clAdBill, "AccCode" & strLineKey, strAccCode)
                AddCollection(clAdBill, "AccGLCode" & strLineKey, strglCode)
                AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(CType(Val(strBaseAmount), Decimal)))
                intLineNo = intLineNo + 1
            End If
        Next

        ' ''' Future Booking begin
        ctcount = clAdBill.Count / 21
        For i = 1 To ctcount
            strLineKey = i & ":" & intReceiptLinno
            If colexists(clAdBill, "AgainstTranLineNo" & strLineKey) = True Then
                If clAdBill("OpenMode" & strLineKey).ToString() = "F" Then
                    DeleteCollection(clAdBill, "AgainstTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "AccTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "TranId" & strLineKey)
                    DeleteCollection(clAdBill, "TranDate" & strLineKey)
                    DeleteCollection(clAdBill, "TranType" & strLineKey)
                    DeleteCollection(clAdBill, "DueDate" & strLineKey)
                    DeleteCollection(clAdBill, "CurrRate" & strLineKey)
                    DeleteCollection(clAdBill, "Credit" & strLineKey)
                    DeleteCollection(clAdBill, "Debit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseCredit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseDebit" & strLineKey)
                    DeleteCollection(clAdBill, "RefNo" & strLineKey)
                    DeleteCollection(clAdBill, "Field2" & strLineKey)
                    DeleteCollection(clAdBill, "Field3" & strLineKey)
                    DeleteCollection(clAdBill, "Field4" & strLineKey)
                    DeleteCollection(clAdBill, "Field5" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AccType" & strLineKey)
                    DeleteCollection(clAdBill, "AccCode" & strLineKey)
                    DeleteCollection(clAdBill, "AccGLCode" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AdjustBaseTotal" & strLineKey)
                End If
            End If
        Next


        For Each gvRow In grdFutureBooking.Rows
            chk = gvRow.FindControl("chkBill1")
            lblAccTranLineNo1 = gvRow.FindControl("lblAccTranLineNo1")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt1")
            lblDoNo1 = gvRow.FindControl("lblDoNo1")
            lblDocType1 = gvRow.FindControl("lblDocType1")
            lblrefno1 = gvRow.FindControl("lblRefNo1")
            lbltktno1 = gvRow.FindControl("lbltktno1")

            txtCurRate = gvRow.FindControl("txtCurRate1")
            btxtBaseAmount = gvRow.FindControl("txtBaseAmount1")

            If chk.Checked = True Then
                strLineKey = intLineNo & ":" & intReceiptLinno
                AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, intReceiptLinno) ' intLineNo.ToString)
                AddCollection(clAdBill, "AccTranLineNo" & strLineKey, lblAccTranLineNo1.Text.Trim)
                AddCollection(clAdBill, "TranId" & strLineKey, lblDoNo1.Text)
                againstbillno = againstbillno + "," + Trim(lblDoNo1.Text)  '' Added shahul 03/07/2018
                AddCollection(clAdBill, "TranDate" & strLineKey, gvRow.Cells(grd_col.dDate).Text)
                AddCollection(clAdBill, "TranType" & strLineKey, lblDocType1.Text)
                If gvRow.Cells(grd_col.DueDate).Text = "&nbsp;" Then
                    AddCollection(clAdBill, "DueDate" & strLineKey, gvRow.Cells(grd_col.dDate).Text)
                Else
                    AddCollection(clAdBill, "DueDate" & strLineKey, gvRow.Cells(grd_col.DueDate).Text)
                End If
                ' AddCollection(clAdBill, "CurrRate" & strLineKey, Val(gvRow.Cells(grd_col.ExchgRate).Text))
                AddCollection(clAdBill, "CurrRate" & strLineKey, txtCurRate.Value)
                If strGridtype = "Credit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, IIf(txtAdujAmt.Value > 0, DecRound(Math.Abs(CType(txtAdujAmt.Value, Decimal))), 0))
                    AddCollection(clAdBill, "Debit" & strLineKey, IIf(txtAdujAmt.Value < 0, DecRound(Math.Abs(CType(txtAdujAmt.Value, Decimal))), 0))
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(btxtBaseAmount.Value > 0, DecRound(Math.Abs(CType(btxtBaseAmount.Value, Decimal))), 0))
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(btxtBaseAmount.Value < 0, DecRound(Math.Abs(CType(btxtBaseAmount.Value, Decimal))), 0))
                ElseIf strGridtype = "Debit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, IIf(txtAdujAmt.Value < 0, Math.Abs(CType(txtAdujAmt.Value, Decimal)), 0))
                    AddCollection(clAdBill, "Debit" & strLineKey, IIf(txtAdujAmt.Value > 0, Math.Abs(CType(txtAdujAmt.Value, Decimal)), 0))
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(btxtBaseAmount.Value < 0, Math.Abs(CType(btxtBaseAmount.Value, Decimal)), 0))
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(btxtBaseAmount.Value > 0, Math.Abs(CType(btxtBaseAmount.Value, Decimal)), 0))
                End If
                AddCollection(clAdBill, "RefNo" & strLineKey, lblrefno1.Text) 'gvRow.Cells(grd_col.RefNo).Text.Trim)
                AddCollection(clAdBill, "Field2" & strLineKey, lbltktno1.Text) ' gvRow.Cells(grd_col.Field2).Text.Trim)
                AddCollection(clAdBill, "Field3" & strLineKey, gvRow.Cells(grd_col.Field3).Text.Trim)
                AddCollection(clAdBill, "Field4" & strLineKey, gvRow.Cells(grd_col.Field4).Text.Trim)
                AddCollection(clAdBill, "Field5" & strLineKey, gvRow.Cells(grd_col.Field5).Text.Trim)
                AddCollection(clAdBill, "OpenMode" & strLineKey, "F")
                AddCollection(clAdBill, "AccType" & strLineKey, strType)
                AddCollection(clAdBill, "AccCode" & strLineKey, strAccCode)
                AddCollection(clAdBill, "AccGLCode" & strLineKey, strglCode)
                AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(CType(Val(strBaseAmount), Decimal)))
                intLineNo = intLineNo + 1
            End If
        Next


        ' ''' Future Booking end

        'Advance Payment start
        ctcount = clAdBill.Count / 21
        For i = 1 To ctcount
            strLineKey = i & ":" & intReceiptLinno
            If colexists(clAdBill, "AgainstTranLineNo" & strLineKey) = True Then
                If clAdBill("OpenMode" & strLineKey).ToString() = "A" Or (clAdBill("OpenMode" & strLineKey).ToString() = "D" And DecRoundplace(Val(txtflag.Value), 0) = 0) Then
                    DeleteCollection(clAdBill, "AgainstTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "AccTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "TranId" & strLineKey)
                    DeleteCollection(clAdBill, "TranDate" & strLineKey)
                    DeleteCollection(clAdBill, "TranType" & strLineKey)
                    DeleteCollection(clAdBill, "DueDate" & strLineKey)
                    DeleteCollection(clAdBill, "CurrRate" & strLineKey)
                    DeleteCollection(clAdBill, "Credit" & strLineKey)
                    DeleteCollection(clAdBill, "Debit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseCredit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseDebit" & strLineKey)
                    DeleteCollection(clAdBill, "RefNo" & strLineKey)
                    DeleteCollection(clAdBill, "Field2" & strLineKey)
                    DeleteCollection(clAdBill, "Field3" & strLineKey)
                    DeleteCollection(clAdBill, "Field4" & strLineKey)
                    DeleteCollection(clAdBill, "Field5" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AccType" & strLineKey)
                    DeleteCollection(clAdBill, "AccCode" & strLineKey)
                    DeleteCollection(clAdBill, "AccGLCode" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AdjustBaseTotal" & strLineKey)
                End If
            End If
        Next

        Dim txtADocNo, txtADocType, txtRefNo, txtAdvPayAmount, txtBaseAmount, txtField2, txtField3, txtField4, txtField5 As HtmlInputText

        Dim txtDate, txtDueDate As TextBox
        For Each gvRow In grdAdPay.Rows
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
            txtBaseAmount = gvRow.FindControl("txtBaseAmount")
            txtDate = gvRow.FindControl("txtDate")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")

            If txtADocNo.Value.Trim <> "" And txtADocType.Value.Trim <> "" And txtBaseAmount.Value.Trim <> "" Then
                strLineKey = intLineNo & ":" & intReceiptLinno
                AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, intReceiptLinno) 'intLineNo.ToString)
                AddCollection(clAdBill, "AccTranLineNo" & strLineKey, intLineNo.ToString)
                AddCollection(clAdBill, "TranId" & strLineKey, txtADocNo.Value.Trim)
                AddCollection(clAdBill, "TranDate" & strLineKey, txtDate.Text.Trim)
                AddCollection(clAdBill, "TranType" & strLineKey, txtADocType.Value.Trim)
                AddCollection(clAdBill, "DueDate" & strLineKey, txtDueDate.Text.Trim)
                AddCollection(clAdBill, "CurrRate" & strLineKey, txtExchangeRate.Value)
                If strGridtype = "Credit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) > 0, DecRound(CType(txtAdvPayAmount.Value, Decimal)), 0))
                    AddCollection(clAdBill, "Debit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtAdvPayAmount.Value, Decimal))), 0))
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(CType(txtBaseAmount.Value, Decimal) > 0, DecRound(CType(txtBaseAmount.Value, Decimal)), 0))
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(CType(txtBaseAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtBaseAmount.Value, Decimal))), 0))
                ElseIf strGridtype = "Debit" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtAdvPayAmount.Value, Decimal))), 0))
                    AddCollection(clAdBill, "Debit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) > 0, DecRound(CType(txtAdvPayAmount.Value, Decimal)), 0))
                    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(CType(txtBaseAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtBaseAmount.Value, Decimal))), 0))
                    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(CType(txtBaseAmount.Value, Decimal) > 0, DecRound(CType(txtBaseAmount.Value, Decimal)), 0))
                End If
                AddCollection(clAdBill, "RefNo" & strLineKey, txtRefNo.Value.Trim)
                AddCollection(clAdBill, "Field2" & strLineKey, txtField2.Value.Trim)
                AddCollection(clAdBill, "Field3" & strLineKey, txtField3.Value.Trim)
                AddCollection(clAdBill, "Field4" & strLineKey, txtField4.Value.Trim)
                AddCollection(clAdBill, "Field5" & strLineKey, txtField5.Value.Trim)
                AddCollection(clAdBill, "OpenMode" & strLineKey, "A")
                AddCollection(clAdBill, "AccType" & strLineKey, strType)
                AddCollection(clAdBill, "AccCode" & strLineKey, strAccCode)
                AddCollection(clAdBill, "AccGLCode" & strLineKey, strglCode)
                AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(CType(Val(strBaseAmount), Decimal)))

                intLineNo = intLineNo + 1
            End If
        Next
        'Session.Add("Collection", clAdBill)
        'Session.Add("AdBillGridCount", grdAdPay.Rows.Count)

        Dim adjcolnostr As String
        adjcolnostr = Request.QueryString("AdjColno")
        Session.Add("Collection" & ":" & adjcolnostr, clAdBill)
        Session.Add("AdBillGridCount" & ":" & adjcolnostr, grdAdPay.Rows.Count)

        Session.Add("AmountAdjusted", txtAmountAdjust.Value)
        Session.Add("BaseAmountAdjusted", txtBaseAmountAdjust.Value)
        Session.Add("LineNo", Request.QueryString("lineNo"))
        'Request.QueryString("Gridtype")
        Session.Add("Gridtype", Request.QueryString("Gridtype"))
        If againstbillno <> "" Then
            Session.Add("Adjustedtranidbills", Right(againstbillno, Len(againstbillno) - 1))
        End If
        ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('AdjBillWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

    ' Rosalin update 07/10/2023
    Public Function DecRoundplace(ByVal Ramt As Decimal, ByVal intmdecimal As Integer) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(intmdecimal, Integer), MidpointRounding.AwayFromZero)
        Return Rdamt
    End Function

#Region "Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)"
    Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)
        If colexists(dataCollection, strKey) = False Then
            If strVal = "&nbsp;" Then strVal = ""
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        Else
            dataCollection.Remove(strKey)
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        End If
    End Sub
    Public Sub DeleteCollection(ByVal dataCollection As Collection, ByVal strKey As String)
        If colexists(dataCollection, strKey) = True Then
            dataCollection.Remove(strKey)
        End If
    End Sub
    Private Sub FillGrid()
        Dim txtDate, txtDueDate As TextBox
        Dim gvrow, gvrow1 As GridViewRow
        Dim collectionDate As Collection
        Dim strLineKey As String
        Dim intReceiptLinno As Integer = CType(Request.QueryString("lineNo"), Integer)
        Dim chkBill As HtmlInputCheckBox
        Dim txtAdujustAmt, txtCurRate, BtxtBaseAmount As HtmlInputText

        Dim lblDoNo, lblDocType, lblAccTranLineNo As Label
        Dim lblDoNo1, lblDocType1, lblAccTranLineNo1 As Label


        Dim strtrantype = Request.QueryString("TranType")
        Dim strType As String = Request.QueryString("AccType")
        Dim strTranId As String = Request.QueryString("TranId")
        Dim strGridtype = Request.QueryString("Gridtype")
        Dim MainGrdCount As Integer = CType(Request.QueryString("MainGrdCount"), Integer)


        ''' Commented shahul 27/01/16 becoz always showing one advance bills
        '  If CType(GetAdvRowsCountFromSession(), Integer) > 1 Then
        If Val(advbillcount) > 1 Then
            'fillDategrd(grdAdPay, False, CType(GetAdvRowsCountFromSession(), Integer))
            fillDategrd(grdAdPay, False, advbillcount)
        Else
            fillDategrd(grdAdPay, True, 1)
        End If

        'If CType(GetAdvRowsCountFromSession(), Integer) > 1 Then
        '    fillDategrd(grdAdPay, False, CType(GetAdvRowsCountFromSession(), Integer))
        'Else
        '    fillDategrd(grdAdPay, True, 1)
        'End If

        txtgrdAdpayRows.Value = grdAdPay.Rows.Count
        ' If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            Dim intcount As Integer = collectionDate.Count / 21
            Dim intLinNo, MainRowidx As Integer
            MainRowidx = 1
            For MainRowidx = 1 To MainGrdCount
                If MainRowidx = intReceiptLinno Then
                    For intLinNo = 1 To intcount
                        strLineKey = intLinNo & ":" & intReceiptLinno
                        If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                            If collectionDate("OpenMode" & strLineKey).ToString() = "B" Then
                                For Each gvrow In grdAdjustBill.Rows
                                    lblAccTranLineNo = gvrow.FindControl("lblAccTranLineNo")
                                    lblDoNo = gvrow.FindControl("lblDoNo")
                                    lblDocType = gvrow.FindControl("lblDocType")
                                    chkBill = gvrow.FindControl("chkBill")
                                    txtAdujustAmt = gvrow.FindControl("txtAdujustAmt")
                                    txtCurRate = gvrow.FindControl("txtCurRate")
                                    BtxtBaseAmount = gvrow.FindControl("txtBaseAmount")
                                    Dim hdnCurrcode As HiddenField = CType(gvrow.FindControl("hdnCurrCode"), HiddenField)
                                    Dim hdnbaseconrate As HiddenField = CType(gvrow.FindControl("hdnbaseconrate"), HiddenField)

                                    'Dim baseconvrate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  convrate from   currrates where tocurr=(select option_selected from reservation_parameters where param_id=457) and currcode='" + hdnCurrcode.Value + "'")
                                    Dim baseconvrate As String = hdnbaseconrate.Value

                                    If collectionDate("AccTranLineNo" & strLineKey).ToString() = Trim(lblAccTranLineNo.Text) And collectionDate("TranId" & strLineKey).ToString() = Trim(lblDoNo.Text) And collectionDate("TranType" & strLineKey).ToString() = Trim(lblDocType.Text) Then
                                        chkBill.Checked = True
                                        If strGridtype = "Credit" Then
                                            If DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString()))
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Credit" & strLineKey).ToString())))
                                                Else
                                                    'txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(txtCurRate.Value) * Val(collectionDate("Credit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(baseconvrate) * Val(collectionDate("Credit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If
                                            End If
                                            If DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) * -1
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(collectionDate("Debit" & strLineKey).ToString())))
                                                Else
                                                    'txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(txtCurRate.Value) * Val(collectionDate("Debit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(baseconvrate) * Val(collectionDate("Debit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If

                                            End If

                                        ElseIf strGridtype = "Debit" Then
                                            If DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString()))
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Debit" & strLineKey).ToString())))
                                                Else
                                                    'txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(txtCurRate.Value) * Val(collectionDate("Debit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(baseconvrate) * Val(collectionDate("Debit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If

                                            End If
                                            If DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) * -1
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(collectionDate("Credit" & strLineKey).ToString())))
                                                Else
                                                    ' txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(txtCurRate.Value) * Val(collectionDate("Credit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(baseconvrate) * Val(collectionDate("Credit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If

                                            End If
                                        End If
                                        BtxtBaseAmount.Value = DecRound(DecRound(txtAdujustAmt.Value) * Val(txtCurRate.Value))
                                        ' txtCurRate.Value = DecRound(BtxtBaseAmount.Value) / DecRound(txtAdujustAmt.Value)

                                        txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(BtxtBaseAmount.Value)
                                        Exit For
                                    End If
                                Next
                                ''''''''''''''''''''' Future Booking Begin '''''''''''''''''''''''''
                            ElseIf collectionDate("OpenMode" & strLineKey).ToString() = "F" Then
                                For Each gvrow In grdFutureBooking.Rows
                                    lblAccTranLineNo1 = gvrow.FindControl("lblAccTranLineNo1")
                                    lblDoNo1 = gvrow.FindControl("lblDoNo1")
                                    lblDocType1 = gvrow.FindControl("lblDocType1")
                                    chkBill = gvrow.FindControl("chkBill1")
                                    txtAdujustAmt = gvrow.FindControl("txtAdujustAmt1")
                                    txtCurRate = gvrow.FindControl("txtCurRate1")
                                    BtxtBaseAmount = gvrow.FindControl("txtBaseAmount1")
                                    Dim hdnCurrcode As HiddenField = CType(gvrow.FindControl("hdnCurrCode1"), HiddenField)

                                    Dim baseconvrate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  convrate from   currrates where tocurr=(select option_selected from reservation_parameters where param_id=457) and currcode='" + hdnCurrcode.Value + "'")


                                    If collectionDate("AccTranLineNo" & strLineKey).ToString() = Trim(lblAccTranLineNo1.Text) And collectionDate("TranId" & strLineKey).ToString() = Trim(lblDoNo1.Text) And collectionDate("TranType" & strLineKey).ToString() = Trim(lblDocType1.Text) Then
                                        chkBill.Checked = True
                                        If strGridtype = "Credit" Then
                                            If DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString()))
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Credit" & strLineKey).ToString())))
                                                Else
                                                    'txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(txtCurRate.Value) * Val(collectionDate("Credit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(baseconvrate) * Val(collectionDate("Credit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If
                                            End If
                                            If DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) * -1
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(collectionDate("Debit" & strLineKey).ToString())))
                                                Else
                                                    'txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(txtCurRate.Value) * Val(collectionDate("Debit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(baseconvrate) * Val(collectionDate("Debit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If

                                            End If

                                        ElseIf strGridtype = "Debit" Then
                                            If DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString()))
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Debit" & strLineKey).ToString())))
                                                Else
                                                    'txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(txtCurRate.Value) * Val(collectionDate("Debit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(baseconvrate) * Val(collectionDate("Debit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If

                                            End If
                                            If DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) > 0 Then
                                                txtAdujustAmt.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) * -1
                                                If Request.QueryString("currcode") = hdnCurrcode.Value Then
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(collectionDate("Credit" & strLineKey).ToString())))
                                                Else
                                                    ' txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(txtCurRate.Value) * Val(collectionDate("Credit" & strLineKey).ToString())))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(baseconvrate) * Val(collectionDate("Credit" & strLineKey).ToString()) / Val(txtExchangeRate.Value)))
                                                End If

                                            End If
                                        End If
                                        BtxtBaseAmount.Value = DecRound(DecRound(txtAdujustAmt.Value) * Val(txtCurRate.Value))
                                        ' txtCurRate.Value = DecRound(BtxtBaseAmount.Value) / DecRound(txtAdujustAmt.Value)

                                        txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(BtxtBaseAmount.Value)
                                        Exit For
                                    End If
                                Next

                                '''''''''''''''''''''  Future Booking End ''''''''''''''''''''

                            ElseIf collectionDate("OpenMode" & strLineKey).ToString() = "A" Or collectionDate("OpenMode" & strLineKey).ToString() = "D" Then
                                Dim txtADocNo, txtDocType, txtRefNo, txtAdvPayAmount, txtBaseAmount, txtField2, txtField3, txtfield4, txtField5 As HtmlInputText
                                Dim hdnADocNo As HiddenField ' Sharfudeen 31/07/2023

                                hdnadvancepayment.Value = ""
                                For Each gvrow1 In grdAdPay.Rows
                                    'If gvrow1.RowIndex + 1 = intLinNo Then
                                    txtADocNo = gvrow1.FindControl("txtADocNo")
                                    hdnADocNo = gvrow1.FindControl("hdnADocNo") 'Sharfdueen 31/07/2023
                                    txtDocType = gvrow1.FindControl("txtDocType")
                                    txtDate = gvrow1.FindControl("txtDate")
                                    txtBaseAmount = gvrow1.FindControl("txtBaseAmount")
                                    If txtADocNo.Value.Trim = "" And txtDate.Text.Trim = "" And txtBaseAmount.Value.Trim = "" Then
                                        txtRefNo = gvrow1.FindControl("txtRefNo")
                                        txtDueDate = gvrow1.FindControl("txtDueDate")
                                        txtAdvPayAmount = gvrow1.FindControl("txtAdvPayAmount")
                                        txtField2 = gvrow1.FindControl("txtField2")
                                        txtField3 = gvrow1.FindControl("txtField3")
                                        txtfield4 = gvrow1.FindControl("txtfield4")
                                        txtField5 = gvrow1.FindControl("txtField5")
                                        If collectionDate("TranId" & strLineKey).ToString() <> "" And collectionDate("TranType" & strLineKey).ToString() <> "" And (collectionDate("Credit" & strLineKey).ToString() <> 0 Or collectionDate("Debit" & strLineKey).ToString() <> 0) Then
                                            txtADocNo.Value = collectionDate("TranId" & strLineKey).ToString()
                                            hdnADocNo.Value = collectionDate("TranId" & strLineKey).ToString() 'Sharfdueen 31/07/2023

                                            txtDate.Text = Format(CType(collectionDate("TranDate" & strLineKey).ToString(), Date), "dd/MM/yyyy")
                                            txtDocType.Value = collectionDate("TranType" & strLineKey).ToString()
                                            txtDueDate.Text = Format(CType(collectionDate("DueDate" & strLineKey).ToString(), Date), "dd/MM/yyyy")
                                            If strGridtype = "Credit" Then
                                                If DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) > 0 Then
                                                    txtAdvPayAmount.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString()))
                                                    txtBaseAmount.Value = DecRound(DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) * Val(txtExchangeRate.Value))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Credit" & strLineKey).ToString())))

                                                    txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(txtBaseAmount.Value)

                                                End If
                                                If DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) > 0 Then
                                                    txtAdvPayAmount.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) * -1
                                                    txtBaseAmount.Value = DecRound(DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) * Val(txtExchangeRate.Value)) * -1
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(collectionDate("Debit" & strLineKey).ToString())))

                                                    txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(txtBaseAmount.Value)
                                                End If
                                            ElseIf strGridtype = "Debit" Then
                                                If DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) > 0 Then
                                                    txtAdvPayAmount.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString()))
                                                    txtBaseAmount.Value = DecRound(DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) * Val(txtExchangeRate.Value))
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Debit" & strLineKey).ToString())))

                                                    txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(txtBaseAmount.Value)
                                                End If
                                                If DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) > 0 Then
                                                    txtAdvPayAmount.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) * -1
                                                    txtBaseAmount.Value = DecRound(DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) * Val(txtExchangeRate.Value)) * -1
                                                    txtBalAdu.Value = DecRound(DecRound(Val(txtBalAdu.Value)) + DecRound(Val(collectionDate("Credit" & strLineKey).ToString())))

                                                    txtBaseAmountAdjust.Value = DecRound(txtBaseAmountAdjust.Value) + DecRound(txtBaseAmount.Value)
                                                End If


                                            End If
                                            txtRefNo.Value = collectionDate("RefNo" & strLineKey).ToString()
                                            txtField2.Value = collectionDate("Field2" & strLineKey).ToString()
                                            txtField3.Value = collectionDate("Field3" & strLineKey).ToString()
                                            txtfield4.Value = collectionDate("Field4" & strLineKey).ToString()
                                            txtField5.Value = collectionDate("Field5" & strLineKey).ToString()

                                            'Sharfudeen 29/07/2023
                                            hdnadvancepayment.Value = hdnadvancepayment.Value + "," + txtADocNo.Value
                                            Exit For
                                        End If
                                    End If

                                Next
                                'ElseIf collectionDate("OpenMode" & strLineKey).ToString() = "D" Then
                                '    Dim txtADocNo, txtDocType, txtRefNo, txtADebit, txtACredit, txtBaseAmount, txtField2, txtField3, txtfield4, txtField5 As HtmlInputText
                                '    For Each gvrow2 In grdAdPay.Rows
                                '        If gvrow2.RowIndex + 1 = intLinNo Then
                                '            txtADocNo = gvrow2.FindControl("txtADocNo")
                                '            txtDocType = gvrow2.FindControl("txtDocType")
                                '            dtDate = gvrow2.FindControl("dtDate")
                                '            txtRefNo = gvrow2.FindControl("txtRefNo")
                                '            dtDueDate = gvrow2.FindControl("dtDueDate")
                                '            txtADebit = gvrow2.FindControl("txtADebit")
                                '            txtACredit = gvrow2.FindControl("txtACredit")
                                '            txtBaseAmount = gvrow2.FindControl("txtBaseAmount")
                                '            txtField2 = gvrow2.FindControl("txtField2")
                                '            txtField3 = gvrow2.FindControl("txtField3")
                                '            txtfield4 = gvrow2.FindControl("txtfield4")
                                '            txtField5 = gvrow2.FindControl("txtField5")
                                '            If collectionDate("TranId" & strLineKey).ToString() <> "" And collectionDate("TranType" & strLineKey).ToString() <> "" And (collectionDate("Credit" & strLineKey).ToString() <> 0 Or collectionDate("Debit" & strLineKey).ToString() <> 0) Then
                                '                txtADocNo.Value = collectionDate("TranId" & strLineKey).ToString()
                                '                dtDate.txtDate.Text = Format(CType(collectionDate("TranDate" & strLineKey).ToString(), Date), "dd/MM/yyyy")
                                '                txtDocType.Value = collectionDate("TranType" & strLineKey).ToString()
                                '                dtDueDate.txtDate.Text = Format(CType(collectionDate("DueDate" & strLineKey).ToString(), Date), "dd/MM/yyyy")
                                '                If strGridtype = "Credit" Then
                                '                    txtACredit.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString()))
                                '                    txtADebit.Value = 0
                                '                    txtBaseAmount.Value = DecRound(Val(collectionDate("Credit" & strLineKey).ToString())) * DecRound(Val(txtExchangeRate.Value))
                                '                    txtBalAdu.Value = DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Credit" & strLineKey).ToString()))
                                '                ElseIf strGridtype = "Debit" Then
                                '                    txtACredit.Value = 0
                                '                    txtADebit.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString()))
                                '                    txtBaseAmount.Value = DecRound(Val(collectionDate("Debit" & strLineKey).ToString())) * DecRound(Val(txtExchangeRate.Value))
                                '                    txtBalAdu.Value = DecRound(Val(txtBalAdu.Value)) - DecRound(Val(collectionDate("Debit" & strLineKey).ToString()))
                                '                End If
                                '                txtRefNo.Value = collectionDate("RefNo" & strLineKey).ToString()
                                '                txtField2.Value = collectionDate("Field2" & strLineKey).ToString()
                                '                txtField3.Value = collectionDate("Field3" & strLineKey).ToString()
                                '                txtfield4.Value = collectionDate("Field4" & strLineKey).ToString()
                                '                txtField5.Value = collectionDate("Field5" & strLineKey).ToString()
                                '                Exit For
                                '            End If
                                '        End If

                                '    Next

                            End If 'Close end if  collectionDate("OpenMode" & strLineKey).ToString() = "D" The
                        End If ' close colexists(collectionDate, "LinNo" & strLineKey) = True
                    Next  ' Close sub grid Loops
                End If  ' Close If MainRowidx = intReceiptLinno Then
            Next  ' Close Main Grid Loops
        End If 'Close  If collectionDate.Count <> 0
        '  End If 'Close  If Session("Collection").ToString <> "" Then
        txtAmountAdjust.Value = DecRound(DecRound(Val(txtADAmount.Value)) - DecRound(Val(txtBalAdu.Value)))
        txtBalAduinBase.Value = DecRound(txtBaseAmount.Value) - DecRound(txtBaseAmountAdjust.Value)
    End Sub
#End Region
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(Val(txtdecimal.Value), Integer), MidpointRounding.AwayFromZero)
        Return Rdamt
    End Function


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim n As Integer = 0
        Dim count As Integer
        count = grdAdPay.Rows.Count + 1

        Dim txtADocNo, txtADocType, txtRefNo, txtAdvPayAmount, txtBaseAmount, txtField2, txtField3, txtField4, txtField5 As HtmlInputText
        Dim hdnADocNo As HiddenField 'Sharfudeen 31/07/2023

        Dim txtDate, txtDueDate As TextBox

        Dim lineno(count) As String
        Dim DocNo(count) As String
        Dim DocType(count) As String
        Dim dtDates(count) As String
        Dim dtDueDates(count) As String
        Dim RefNo(count) As String
        Dim ADvPay(count) As String

        Dim BaseAmount(count) As String
        Dim Field2(count) As String
        Dim Field3(count) As String
        Dim Field4(count) As String
        Dim Field5(count) As String

        For Each gvRow In grdAdPay.Rows
            txtADocNo = gvRow.FindControl("txtADocNo")
            hdnADocNo = gvRow.FindControl("hdnADocNo") 'Sharfudeen 31/07/2023
            txtADocType = gvRow.FindControl("txtDocType")
            txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
            txtBaseAmount = gvRow.FindControl("txtBaseAmount")
            txtDate = gvRow.FindControl("txtDate")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")

            If txtADocNo.Value.Trim <> "" And txtADocType.Value.Trim <> "" And txtBaseAmount.Value.Trim <> "" Then
                DocNo(n) = txtADocNo.Value
                DocType(n) = txtADocType.Value
                dtDates(n) = Format(CType(txtDate.Text, Date), "dd/MM/yyyy")
                dtDueDates(n) = Format(CType(txtDueDate.Text, Date), "dd/MM/yyyy")
                RefNo(n) = txtRefNo.Value
                ADvPay(n) = txtAdvPayAmount.Value
                BaseAmount(n) = txtBaseAmount.Value
                Field2(n) = txtField2.Value
                Field3(n) = txtField3.Value
                Field4(n) = txtField4.Value
                Field5(n) = txtField5.Value

                n = n + 1
            End If
        Next
        fillDategrd(grdAdPay, False, grdAdPay.Rows.Count + 1)
        txtgrdAdpayRows.Value = grdAdPay.Rows.Count
        Dim i As Integer = n
        n = 0
        For Each gvRow In grdAdPay.Rows
            If n = i Then
                Exit For
            End If
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
            txtBaseAmount = gvRow.FindControl("txtBaseAmount")
            txtDate = gvRow.FindControl("txtDate")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")
            hdnADocNo = gvRow.FindControl("hdnADocNo") 'Sharfudeen 31/07/2023

            If DocNo(n) <> "" And DocType(n) <> "" And BaseAmount(n) <> "" Then
                txtADocNo.Value = DocNo(n)
                txtADocType.Value = DocType(n)
                txtDate.Text = Format(CType(dtDates(n), Date), "dd/MM/yyyy")
                txtDueDate.Text = Format(CType(dtDueDates(n), Date), "dd/MM/yyyy")
                txtRefNo.Value = RefNo(n)
                txtAdvPayAmount.Value = ADvPay(n)
                txtBaseAmount.Value = BaseAmount(n)
                txtField2.Value = Field2(n)
                txtField3.Value = Field3(n)
                txtField4.Value = Field4(n)
                txtField5.Value = Field5(n)
                hdnADocNo.Value = DocNo(n) 'Sharfudeen 31/07/2023
            End If
            n = n + 1
        Next
    End Sub
    Protected Sub btnAExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ScriptStr As String
        ScriptStr = "<script language=""javascript"">var win=window.close();</script>"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)
    End Sub

    Protected Sub btnDelLine_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clAdBill As New Collection
        Dim strtrantype = Request.QueryString("TranType")
        Dim strType As String = Request.QueryString("AccType")
        Dim intReceiptLinno As Integer = CType(Request.QueryString("lineNo"), Integer)
        Dim strTranId As String = Request.QueryString("TranId")
        Dim strGridtype = Request.QueryString("Gridtype")
        Dim strAccCode = Request.QueryString("AccCode")
        Dim strglCode = Request.QueryString("ControlCode")


        Dim intLineNo As Integer = 1
        Dim strLineKey As String
        Dim strtranKey As String

        If validate_BillAgainst() = False Then
            Exit Sub
        End If

        'If Session("Collection").ToString <> "" Then
        '    clAdBill = CType(Session("Collection"), Collection)
        'End If
        clAdBill = GetCollectionFromSession()
        Dim n As Integer = 0
        Dim count As Integer
        count = grdAdPay.Rows.Count
        Dim chk As HtmlInputCheckBox
        Dim txtAdujAmt, txtADocNo, txtADocType, txtRefNo, txtAdvPayAmount, txtgrdBaseAmount, txtField2, txtField3, txtField4, txtField5 As HtmlInputText
        Dim hdnADocNo As HiddenField ' Sharfudeen 31/07/2023

        Dim txtDate, txtDueDate As TextBox
        Dim chkSelect As CheckBox


        Dim TotBaseadvamt As Decimal

        Dim lineno(count) As String
        Dim DocNo(count) As String
        Dim DocType(count) As String
        Dim dtDates(count) As String
        Dim dtDueDates(count) As String
        Dim RefNo(count) As String
        Dim ADvPay(count) As String
        Dim BaseAmount(count) As String
        Dim Field2(count) As String
        Dim Field3(count) As String
        Dim Field4(count) As String
        Dim Field5(count) As String
        Dim Totadvamt As Decimal

        For Each gvRow In grdAdPay.Rows
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
            txtgrdBaseAmount = gvRow.FindControl("txtBaseAmount")
            txtDate = gvRow.FindControl("txtDate")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")
            chkSelect = gvRow.FindControl("chkSelect")

            ''sharfudeen(02/09/2022)
            strLineKey = intLineNo & ":" & intReceiptLinno
            'DeleteCollection(clAdBill, "AgainstTranLineNo" & strLineKey)
            'DeleteCollection(clAdBill, "AccTranLineNo" & strLineKey)
            'DeleteCollection(clAdBill, "TranId" & strLineKey)
            'DeleteCollection(clAdBill, "TranDate" & strLineKey)
            'DeleteCollection(clAdBill, "TranType" & strLineKey)
            'DeleteCollection(clAdBill, "DueDate" & strLineKey)
            'DeleteCollection(clAdBill, "CurrRate" & strLineKey)
            'DeleteCollection(clAdBill, "Credit" & strLineKey)
            'DeleteCollection(clAdBill, "Debit" & strLineKey)
            'DeleteCollection(clAdBill, "BaseCredit" & strLineKey)
            'DeleteCollection(clAdBill, "BaseDebit" & strLineKey)
            'DeleteCollection(clAdBill, "RefNo" & strLineKey)
            'DeleteCollection(clAdBill, "Field2" & strLineKey)
            'DeleteCollection(clAdBill, "Field3" & strLineKey)
            'DeleteCollection(clAdBill, "Field4" & strLineKey)
            'DeleteCollection(clAdBill, "Field5" & strLineKey)
            'DeleteCollection(clAdBill, "OpenMode" & strLineKey)
            'DeleteCollection(clAdBill, "AccType" & strLineKey)
            'DeleteCollection(clAdBill, "AccCode" & strLineKey)
            'DeleteCollection(clAdBill, "AccGLCode" & strLineKey)
            'DeleteCollection(clAdBill, "OpenMode" & strLineKey)


            intLineNo = intLineNo + 1

            If chkSelect.Checked = False Then
                If txtADocNo.Value.Trim <> "" And txtADocType.Value.Trim <> "" And txtgrdBaseAmount.Value.Trim <> "" Then
                    DocNo(n) = txtADocNo.Value
                    DocType(n) = txtADocType.Value
                    dtDates(n) = Format(CType(txtDate.Text, Date), "dd/MM/yyyy")
                    dtDueDates(n) = Format(CType(txtDueDate.Text, Date), "dd/MM/yyyy")
                    RefNo(n) = txtRefNo.Value
                    ADvPay(n) = txtAdvPayAmount.Value
                    BaseAmount(n) = txtgrdBaseAmount.Value
                    Field2(n) = txtField2.Value
                    Field3(n) = txtField3.Value
                    Field4(n) = txtField4.Value
                    Field5(n) = txtField5.Value
                    n = n + 1
                End If
            End If
        Next
        fillDategrd(grdAdPay, False, grdAdPay.Rows.Count)
        txtgrdAdpayRows.Value = grdAdPay.Rows.Count
        Dim i As Integer = n
        n = 0
        Totadvamt = 0
        TotBaseadvamt = 0

        'If Session("Collection").ToString <> "" Then
        '    clAdBill = CType(Session("Collection"), Collection)
        'End If

        intLineNo = 1
        For Each gvRow In grdAdPay.Rows
            If n = i Then
                Exit For
            End If
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
            txtgrdBaseAmount = gvRow.FindControl("txtBaseAmount")
            txtDate = gvRow.FindControl("txtDate")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")
            hdnADocNo = gvRow.FindControl("hdnADocNo") ' Sharfudeen 31/07/2023

            If DocNo(n) <> "" And DocType(n) <> "" And BaseAmount(n) <> "" Then
                txtADocNo.Value = DocNo(n)
                hdnADocNo.Value = DocNo(n) ' Sharfudeen 31/07/2023

                txtADocType.Value = DocType(n)
                txtDate.Text = Format(CType(dtDates(n), Date), "dd/MM/yyyy")
                txtDueDate.Text = Format(CType(dtDueDates(n), Date), "dd/MM/yyyy")
                txtRefNo.Value = RefNo(n)
                txtAdvPayAmount.Value = ADvPay(n)
                txtgrdBaseAmount.Value = BaseAmount(n)
                txtField2.Value = Field2(n)
                txtField3.Value = Field3(n)
                txtField4.Value = Field4(n)
                txtField5.Value = Field5(n)
                If strGridtype = "Credit" Then
                    Totadvamt = DecRound(DecRound(Val(Totadvamt)) + DecRound(Val(ADvPay(n))))
                    TotBaseadvamt = DecRound(DecRound(Val(TotBaseadvamt)) + DecRound(Val(BaseAmount(n))))
                ElseIf strGridtype = "Debit" Then
                    Totadvamt = DecRound(DecRound(Val(Totadvamt)) + DecRound(Val(ADvPay(n))))
                    TotBaseadvamt = DecRound(DecRound(Val(TotBaseadvamt)) + DecRound(Val(BaseAmount(n))))
                End If

                'sharfudeen 29/09/2022
                'strLineKey = intLineNo & ":" & intReceiptLinno
                'AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, intLineNo.ToString)
                'AddCollection(clAdBill, "AccTranLineNo" & strLineKey, intLineNo.ToString)
                'AddCollection(clAdBill, "TranId" & strLineKey, txtADocNo.Value.Trim)
                'AddCollection(clAdBill, "TranDate" & strLineKey, txtDate.Text.Trim)
                'AddCollection(clAdBill, "TranType" & strLineKey, txtADocType.Value.Trim)
                'AddCollection(clAdBill, "DueDate" & strLineKey, txtDueDate.Text)
                'AddCollection(clAdBill, "CurrRate" & strLineKey, txtExchangeRate.Value)
                'If strGridtype = "Credit" Then
                '    AddCollection(clAdBill, "Credit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) > 0, DecRound(CType(txtAdvPayAmount.Value, Decimal)), 0))
                '    AddCollection(clAdBill, "Debit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtAdvPayAmount.Value, Decimal))), 0))
                '    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(CType(txtgrdBaseAmount.Value, Decimal) > 0, DecRound(CType(txtgrdBaseAmount.Value, Decimal)), 0))
                '    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(CType(txtgrdBaseAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtgrdBaseAmount.Value, Decimal))), 0))
                'ElseIf strGridtype = "Debit" Then
                '    AddCollection(clAdBill, "Credit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtAdvPayAmount.Value, Decimal))), 0))
                '    AddCollection(clAdBill, "Debit" & strLineKey, IIf(CType(txtAdvPayAmount.Value, Decimal) > 0, DecRound(CType(txtAdvPayAmount.Value, Decimal)), 0))
                '    AddCollection(clAdBill, "BaseCredit" & strLineKey, IIf(CType(txtgrdBaseAmount.Value, Decimal) < 0, DecRound(Math.Abs(CType(txtgrdBaseAmount.Value, Decimal))), 0))
                '    AddCollection(clAdBill, "BaseDebit" & strLineKey, IIf(CType(txtgrdBaseAmount.Value, Decimal) > 0, DecRound(CType(txtgrdBaseAmount.Value, Decimal)), 0))
                'End If

                'AddCollection(clAdBill, "RefNo" & strLineKey, txtRefNo.Value.Trim)
                'AddCollection(clAdBill, "Field2" & strLineKey, txtField2.Value.Trim)
                'AddCollection(clAdBill, "Field3" & strLineKey, txtField3.Value.Trim)
                'AddCollection(clAdBill, "Field4" & strLineKey, txtField4.Value.Trim)
                'AddCollection(clAdBill, "Field5" & strLineKey, txtField5.Value.Trim)
                'AddCollection(clAdBill, "OpenMode" & strLineKey, "A")
                'AddCollection(clAdBill, "AccType" & strLineKey, strType)
                'AddCollection(clAdBill, "AccCode" & strLineKey, strAccCode)
                'AddCollection(clAdBill, "AccGLCode" & strLineKey, strglCode)

                intLineNo = intLineNo + 1
            End If
            n = n + 1
        Next

        '  Session.Add("Collection", clAdBill)

        For Each gvRow In grdAdjustBill.Rows
            chk = gvRow.FindControl("chkBill")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
            txtgrdBaseAmount = gvRow.FindControl("txtBaseAmount")
            If chk.Checked = True Then
                Totadvamt = DecRound(DecRound(Val(Totadvamt)) + DecRound(Val(txtAdujAmt.Value)))
                TotBaseadvamt = DecRound(DecRound(Val(TotBaseadvamt)) + DecRound(Val(txtgrdBaseAmount.Value)))
            End If
        Next

        '''''''''''Future Booking
        For Each gvRow In grdFutureBooking.Rows
            chk = gvRow.FindControl("chkBill1")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt1")
            txtgrdBaseAmount = gvRow.FindControl("txtBaseAmount1")
            If chk.Checked = True Then
                Totadvamt = DecRound(DecRound(Val(Totadvamt)) + DecRound(Val(txtAdujAmt.Value)))
                TotBaseadvamt = DecRound(DecRound(Val(TotBaseadvamt)) + DecRound(Val(txtgrdBaseAmount.Value)))
            End If
        Next
        '''''''''''''''''''''''''''''''''''


        txtBalAdu.Value = DecRound(DecRound(Val(txtADAmount.Value)) - DecRound(Val(Totadvamt)))
        txtAmountAdjust.Value = DecRound(Val(Totadvamt))

        txtBaseAmountAdjust.Value = DecRound(Val(TotBaseadvamt))
        txtBalAduinBase.Value = DecRound(txtBaseAmount.Value) - DecRound(txtBaseAmountAdjust.Value)
    End Sub

    Private Function validate_BillAgainst() As Boolean
        Try

            validate_BillAgainst = True
            Dim myDataAdapter As SqlDataAdapter
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String

            Dim acc_against_tran_id As String



            If ViewState("divcode") Is Nothing Then
                acc_against_tran_id = Request.QueryString("RefCode")
            Else
                acc_against_tran_id = ViewState("ReceiptsAdjustBillsRefCode")
            End If

            'Sharfudeen 29072023
            ' strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            If ViewState("divcode") Is Nothing Then
                strdiv = Request.QueryString("divid")  ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            Else
                strdiv = ViewState("divcode")
            End If

            Dim strtrantype = Request.QueryString("TranType")
            Dim chkSelect As CheckBox
            Dim txtADocNo As HtmlInputText
            For Each gvRow In grdAdPay.Rows
                chkSelect = gvRow.FindControl("chkSelect")
                txtADocNo = gvRow.FindControl("txtADocNo")
                If chkSelect.Checked = True Then
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    'Sharfudeen 31/07/2023
                    '  myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
                    myCommand = New SqlCommand("sp_Check_AgainstBills_settlement", SqlConn)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtADocNo.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = acc_against_tran_id ' Sharfudeen 31/07/2023
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = Trim(strtrantype)


                    Dim param1 As SqlParameter
                    Dim param2 As SqlParameter
                    param1 = New SqlParameter
                    param1.ParameterName = "@allowflg"
                    param1.Direction = ParameterDirection.Output
                    param1.DbType = DbType.Int16
                    param1.Size = 9
                    myCommand.Parameters.Add(param1)
                    param2 = New SqlParameter
                    param2.ParameterName = "@errmsg"
                    param2.Direction = ParameterDirection.Output
                    param2.DbType = DbType.String
                    param2.Size = 200
                    myCommand.Parameters.Add(param2)
                    myDataAdapter = New SqlDataAdapter(myCommand)
                    myCommand.ExecuteNonQuery()

                    Alflg = param1.Value
                    ErrMsg = param2.Value

                    If Alflg = 1 And ErrMsg <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                        validate_BillAgainst = False
                        Exit Function
                    End If
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function

    Private Function validate_BillAgainst_previous(ByVal ADocNo As String) As Boolean
        Try

            validate_BillAgainst_previous = True
            Dim myDataAdapter As SqlDataAdapter
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String

            'Sharfudeen 29072023
            ' strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            Dim acc_against_tran_id As String



            If ViewState("divcode") Is Nothing Then
                acc_against_tran_id = Request.QueryString("RefCode")
            Else
                acc_against_tran_id = ViewState("ReceiptsAdjustBillsRefCode")
            End If

            If ViewState("divcode") Is Nothing Then
                strdiv = Request.QueryString("divid")  ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            Else
                strdiv = ViewState("divcode")
            End If

            Dim strtrantype = Request.QueryString("TranType")

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            '  myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand = New SqlCommand("sp_Check_AgainstBills_settlement", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = ADocNo
            myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = acc_against_tran_id ' Sharfudeen 31/07/2023
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = Trim(strtrantype)

            Dim param1 As SqlParameter
            Dim param2 As SqlParameter
            param1 = New SqlParameter
            param1.ParameterName = "@allowflg"
            param1.Direction = ParameterDirection.Output
            param1.DbType = DbType.Int16
            param1.Size = 9
            myCommand.Parameters.Add(param1)
            param2 = New SqlParameter
            param2.ParameterName = "@errmsg"
            param2.Direction = ParameterDirection.Output
            param2.DbType = DbType.String
            param2.Size = 200
            myCommand.Parameters.Add(param2)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()

            Alflg = param1.Value
            ErrMsg = param2.Value

            If Alflg = 1 And ErrMsg <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                validate_BillAgainst_previous = False
                Exit Function
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function


    Private Sub disablegrid()
        If ViewState("ReceiptsAdjustBillsState") = "Delete" Or ViewState("ReceiptsAdjustBillsState") = "View" Then
            Dim chk As HtmlInputCheckBox
            Dim txtAdujAmt As HtmlInputText
            For Each gvRow In grdAdjustBill.Rows
                chk = gvRow.FindControl("chkBill")
                txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
                chk.Disabled = True
                'txtAdujAmt.Disabled = True
                TextLockHtml(txtAdujAmt)
            Next
            Dim txtADocNo, txtADocType, txtRefNo, txtAdvPayAmount, txtBaseAmount, txtField2, txtField3, txtField4, txtField5 As HtmlInputText

            Dim txtDate, txtDueDate As TextBox
            Dim ImgBtnDt, ImgBtnDueDate As ImageButton
            Dim chkSelect As CheckBox
            For Each gvRow In grdAdPay.Rows
                chkSelect = gvRow.FindControl("chkSelect")
                txtADocNo = gvRow.FindControl("txtADocNo")
                txtADocType = gvRow.FindControl("txtDocType")
                txtAdvPayAmount = gvRow.FindControl("txtAdvPayAmount")
                txtBaseAmount = gvRow.FindControl("txtBaseAmount")
                txtDate = gvRow.FindControl("txtDate")
                txtDueDate = gvRow.FindControl("txtDueDate")
                ImgBtnDt = gvRow.FindControl("ImgBtnDt")
                ImgBtnDueDate = gvRow.FindControl("ImgBtnDueDate")

                txtRefNo = gvRow.FindControl("txtRefNo")
                txtField2 = gvRow.FindControl("txtField2")
                txtField3 = gvRow.FindControl("txtField3")
                txtField4 = gvRow.FindControl("txtField4")
                txtField5 = gvRow.FindControl("txtField5")


                txtADocNo.Disabled = True
                txtADocType.Disabled = True
                'txtAdvPayAmount.Disabled = True
                'txtBaseAmount.Disabled = True
                txtDate.Enabled = False
                txtDueDate.Enabled = False
                ImgBtnDt.Enabled = False
                ImgBtnDueDate.Enabled = False
                txtRefNo.Disabled = True
                txtField2.Disabled = True
                txtField3.Disabled = True
                txtField4.Disabled = True
                txtField5.Disabled = True
                chkSelect.Enabled = False
                TextLockHtml(txtAdvPayAmount)
                TextLockHtml(txtBaseAmount)
            Next
            btnAdd.Visible = False
            btnDelLine.Visible = False
            btnOk.Visible = False
        End If
    End Sub
    '#Region "Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean"
    '    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
    '        Try
    '            Dim k As Integer
    '            colexists = False
    '            If newcol.Count > 0 Then
    '                For k = 1 To newcol.Count
    '                    If newcol(newkey).ToString <> "" Then
    '                        colexists = True
    '                        Exit Function
    '                    End If
    '                Next
    '            End If
    '        Catch ex As Exception
    '            colexists = False
    '        End Try
    '    End Function
    '#End Region



    Protected Sub GVCollection_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            Dim GVrow As GridViewRow
            GVrow = e.Row
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim chksel As CheckBox = CType(GVrow.FindControl("chkSel"), CheckBox)
                Dim lblrequestno As Label = CType(GVrow.FindControl("lblrequestno"), Label)
                Dim lblguestname As Label = CType(GVrow.FindControl("lblguestname"), Label)
                Dim lblamount As Label = CType(GVrow.FindControl("lblamount"), Label)

                chksel.Attributes.Add("onclick", "javascript:chkvalidate('" + CType(chksel.ClientID, String) + "','" + CType(lblrequestno.ClientID, String) + "','" + CType(lblguestname.ClientID, String) + "','" + CType(lblamount.ClientID, String) + "')")

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("receiptsadjustbills.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnBASearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBASearch.Click
        Try
            Dim strdiv, glCode, strLineNo, strOlineNo, strtrantype, strtype, strCDType, strrequestid As String
            'strCDType = ""
            'strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            'glCode = Request.QueryString("ControlCode")
            'strLineNo = Request.QueryString("lineNo")
            'strOlineNo = Request.QueryString("OlineNo")

            'Dim lineno As String
            'If strOlineNo = "" Then
            '    strOlineNo = strLineNo
            'End If
            'If CType(strLineNo, Decimal) = CType(strOlineNo, Decimal) Then
            '    lineno = strLineNo
            'Else
            '    lineno = strOlineNo
            'End If


            'strtrantype = Request.QueryString("TranType")
            'strtype = Request.QueryString("AccType")


            'Dim stragainst_tran_id, stragainst_tran_type As String
            'If ViewState("ReceiptsAdjustBillsState") = "Edit" Or ViewState("ReceiptsAdjustBillsState") = "Delete" Or ViewState("ReceiptsAdjustBillsState") = "View" Then
            '    stragainst_tran_id = ViewState("ReceiptsAdjustBillsRefCode")
            '    stragainst_tran_type = strtrantype
            'Else
            '    stragainst_tran_id = "''"
            '    stragainst_tran_type = ""
            'End If

            'If txtGrdType.Value = "Debit" Then
            '    lblDrCrCaption.Text = "Dr"
            '    strCDType = "D"
            'ElseIf txtGrdType.Value = "Credit" Then
            '    lblDrCrCaption.Text = "Cr"
            '    strCDType = "C"
            'End If


            'If Not Request.QueryString("Requestid") Is Nothing Then
            '    strrequestid = Request.QueryString("Requestid")
            'Else
            '    strrequestid = ""
            'End If



            'Dim collectionDate As Collection
            'collectionDate = GetCollectionFromSession()
            'If collectionDate.Count = 0 Then
            '    If strrequestid <> "" Then
            '        hdnreqid.Value = strrequestid
            '    End If
            'End If


            'FillAdjust(strdiv, strtype, stragainst_tran_id, lineno, stragainst_tran_type, glCode, strCDType, strrequestid, txtABSearchNo.Value, ddlABSearchNo.Value)

            For Each row As GridViewRow In grdAdjustBill.Rows
                Dim mDocNo As String = DirectCast(row.Cells(2).FindControl("lblDoNo"), Label).Text
                Dim mRefNo As String = DirectCast(row.Cells(2).FindControl("lblRefNo"), Label).Text
                Dim mtktno As String = DirectCast(row.Cells(3).FindControl("lbltktno"), Label).Text
                If ddlABSearchNo.Value = 1 Then
                    If Trim(UCase(mDocNo)) = Trim(UCase(txtABSearchNo.Value)) Then
                        row.BackColor = Drawing.Color.DodgerBlue
                        row.ForeColor = Drawing.Color.White
                        row.Focus()

                    End If
                ElseIf ddlABSearchNo.Value = 2 Then
                    If Trim(UCase(mRefNo)) = Trim(UCase(txtABSearchNo.Value)) Then
                        row.BackColor = Drawing.Color.DodgerBlue
                        row.ForeColor = Drawing.Color.White
                        row.Focus()
                    End If
                ElseIf ddlABSearchNo.Value = 3 Then
                    If Trim(UCase(mtktno)) = Trim(UCase(txtABSearchNo.Value)) Then
                        row.BackColor = Drawing.Color.DodgerBlue
                        row.ForeColor = Drawing.Color.White
                        row.Focus()
                    End If

                End If
            Next

            '''' Future Booking begin '''''''''''''
            For Each row As GridViewRow In grdFutureBooking.Rows
                Dim mDocNo As String = DirectCast(row.Cells(2).FindControl("lblDoNo1"), Label).Text
                Dim mRefNo As String = DirectCast(row.Cells(2).FindControl("lblRefNo1"), Label).Text
                Dim mtktno As String = DirectCast(row.Cells(3).FindControl("lbltktno1"), Label).Text
                If ddlABSearchNo.Value = 1 Then
                    If Trim(UCase(mDocNo)) = Trim(UCase(txtABSearchNo.Value)) Then
                        row.BackColor = Drawing.Color.DodgerBlue
                        row.ForeColor = Drawing.Color.White
                        row.Focus()

                    End If
                ElseIf ddlABSearchNo.Value = 2 Then
                    If Trim(UCase(mRefNo)) = Trim(UCase(txtABSearchNo.Value)) Then
                        row.BackColor = Drawing.Color.DodgerBlue
                        row.ForeColor = Drawing.Color.White
                        row.Focus()
                    End If
                ElseIf ddlABSearchNo.Value = 3 Then
                    If Trim(UCase(mtktno)) = Trim(UCase(txtABSearchNo.Value)) Then
                        row.BackColor = Drawing.Color.DodgerBlue
                        row.ForeColor = Drawing.Color.White
                        row.Focus()
                    End If

                End If
            Next

            '''''Future Booking End '''''''''''''''

        Catch ex As Exception

        End Try
    End Sub
End Class


'objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select isnull(controlacctcode,0) from dbo.view_account where type='" & strtype & "'and code='" & strAccCode & "'")
' If TranType = "C" Then
'ElseIf TranType = "D" Then

'pass="AccCode=" + ddlACode.value + "&AccType=" + ddltyp.options[ddltyp.selectedIndex].text  + "&TranId=" + txtTranId.value  + "&linNo=" + txtLienNo.value + "&glcode=" + ddlglCode.value + "&currrate=" + txtCurRate.value
'Session.Add("CrDrBillAdjust", strGridtype)
'Session.Add("TranTypeBillAdjust", strGridtype)

'strSqlQry = "exec sp_getadjust '2033948','S','',null,null,'2033000','D','1',1"

'optionval = objUtils.GetAutoDocNo("ADVANCE", SqlConn, sqlTrans)
'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
' sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

'strSqlQry = "exec sp_getadjust '" & txtAccCode.Value & "','" & strtype & "','" & stragainst_tran_id & "'," & strLineNo & ",'" & stragainst_tran_type & "','" & glCode & "','" & strCDType & "','" & strdiv & "'," & 1 & ""
'strSqlQry = "exec sp_getadjust '" & txtAccCode.Value & "','" & strtype & "','',null,null,'" & glCode & "','" & strCDType & "','" & strdiv & "'," & 1 & ""


' Dim sqlTrans As SqlTransaction

'AddCollection(clAdBill, "Credit" & strLineKey, 0)
'AddCollection(clAdBill, "Debit" & strLineKey, txtBalAdu.Value)
' sqlTrans.Commit()
' SqlConn.Close()


'txtAdujAmt.Attributes.Add("onchange", "javascript:AdjestBillChange('" + CType(txtAdujAmt.ClientID, String) + "','" + CType(chk.ClientID, String) + "','" + CType(txtBalAmount.ClientID, String) + "')")
'txtAdujAmt.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")


'Else
'  chk.Checked = True
'Later check Total of adjusted
'txtBalAdu.Value = DecRound(Val(txtBalAdu.Value)) - DecRound(Val(txtAdujAmt.Value))
' txtAdujAmt.Style.Add("readonly", "")
'If chkColFlag = False Then
'If ViewState("ReceiptsAdjustBillsState") = "Edit" Or ViewState("ReceiptsAdjustBillsState") = "Delete" Then
'    fillDategrd(grdAdPay, True, 5)
'   fillAdvanPay(ViewState("ReceiptsAdjustBillsRefCode"))
'  txtgrdAdpayRows.Value = grdAdPay.Rows.Count
'Else

'End If
'End If


'If ViewState("ReceiptsAdjustBillsState") = "Edit" Then
'    If Session("Collection").ToString <> "" Then
'        clAdBill = CType(Session("Collection"), Collection)

'    End If
'End If


'  AddCollection(clAdBill, "Debit" & strLineKey, txtADebit.Value)

' AddCollection(clAdBill, "Credit" & strLineKey, txtACredit.Value)

' Dim TranType As String = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"acc_type_master", "acc_type_mode", "acc_type_name", strType)
' Dim txtADebit As HtmlInputText
'Dim txtACredit As HtmlInputText

'If strGridtype = "Credit" Then
'    txtADebit.Disabled = True
'    txtACredit.Disabled = False
'Else
'    txtADebit.Disabled = False
'    txtACredit.Disabled = True
'End If

' txtADebit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
'txtACredit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")

' txtADebit.Attributes.Add("onchange", "javascript:OnChangeAdPay('" + CType(txtADebit.ClientID, String) + "','" + CType(txtACredit.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(txtExchangeRate.ClientID, String) + "','" + CType(e.Row.RowIndex, String) + "')")

'btnAExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false){return false}else{window.close()};")
'#Region "Public Sub fillAdvanPay(ByVal tranid As String)"
'    Public Sub fillAdvanPay(ByVal tranid As String)
'        Dim clAdBill As New Collection
'        Dim strLineKey As String
'        Dim intLineNo As Long = 1
'        Dim myDS As New DataSet
'        Dim mySqlReader As SqlDataReader

'        Dim txtADocNo As HtmlInputText
'        Dim txtADocType As HtmlInputText
'        Dim txtRefNo As HtmlInputText
'        Dim txtADebit As HtmlInputText
'        Dim txtACredit As HtmlInputText
'        Dim txtBaseAmount As HtmlInputText
'        Dim txtField2 As HtmlInputText
'        Dim txtField3 As HtmlInputText
'        Dim txtField4 As HtmlInputText
'        Dim txtField5 As HtmlInputText

'        Dim dtDate As EclipseWebSolutions.DatePicker.DatePicker
'        Dim dtDueDate As EclipseWebSolutions.DatePicker.DatePicker

'        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'        myCommand = New SqlCommand("select * from  open_detail Where against_tran_id='" & tranid & "' and open_mode='D'", SqlConn)
'        mySqlReader = myCommand.ExecuteReader()
'        If mySqlReader.HasRows Then
'            While mySqlReader.Read()
'                For Each gvRow In grdAdPay.Rows
'                    txtADocNo = gvRow.FindControl("txtADocNo")
'                    txtADocType = gvRow.FindControl("txtDocType")
'                    txtADebit = gvRow.FindControl("txtADebit")
'                    txtACredit = gvRow.FindControl("txtACredit")
'                    txtBaseAmount = gvRow.FindControl("txtBaseAmount")
'                    dtDate = gvRow.FindControl("dtDate")
'                    dtDueDate = gvRow.FindControl("dtDueDate")
'                    txtRefNo = gvRow.FindControl("txtRefNo")
'                    txtField2 = gvRow.FindControl("txtField2")
'                    txtField3 = gvRow.FindControl("txtField3")
'                    txtField4 = gvRow.FindControl("txtField4")
'                    txtField5 = gvRow.FindControl("txtField5")

'                    If txtADocNo.Value.Trim = "" And txtADocType.Value.Trim = "" And txtBaseAmount.Value.Trim = "" Then

'                        txtADocNo.Value = mySqlReader("tran_id")
'                        dtDate.txtDate.Text = mySqlReader("tran_date")
'                        txtADocType.Value = mySqlReader("tran_type")
'                        dtDueDate.txtDate.Text = mySqlReader("open_due_date")

'                        txtACredit.Value = mySqlReader("open_credit")
'                        txtADebit.Value = mySqlReader("open_debit")
'                        txtBaseAmount.Value = CType(Val(txtACredit.Value), Decimal) + CType(Val(txtADebit.Value), Decimal) * CType(Val(txtExchangeRate.Value), Decimal)
'                        txtRefNo.Value = mySqlReader("open_field1")
'                        txtField2.Value = mySqlReader("open_field2")
'                        txtField3.Value = mySqlReader("open_field3")
'                        txtField4.Value = mySqlReader("open_field4")
'                        txtField5.Value = mySqlReader("open_field5")

'                        If Val(txtBaseAmount.Value) <> 0 Then
'                            txtBalAdu.Value = CType(txtBalAdu.Value, Decimal) - Val(txtBaseAmount.Value)
'                        End If

'                        Exit For
'                    End If
'                Next
'            End While
'        End If
'        myCommand.Dispose()
'        SqlConn.Close()
'    End Sub
'#End Region