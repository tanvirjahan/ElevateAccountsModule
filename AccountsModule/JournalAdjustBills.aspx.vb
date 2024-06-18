'----------------------------------------------------------------------------------------------------
'   Module Name    :    JournalAdjustBills
'   Developer Name :    Mangesh 
'   Date           :    
'   
'
'----------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class JournalAdjustBills
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
    Dim gvRow As GridViewRow

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
        Field2 = 10
        Field3 = 11
        Field4 = 12
        Field5 = 13
    End Enum
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                'pass="AccCode=" + ddlACode.value + "&AccType=" + ddltyp.options[ddltyp.selectedIndex].text  + "&TranId=" + txtTranId.value  + "&linNo=" + txtLienNo.value + "&glcode=" + ddlglCode.value + "&currrate=" + txtCurRate.value

                Dim strtype As String = Request.QueryString("AccType")
                Dim strAccCode As String = Request.QueryString("AccCode")
                Dim strLineNo As String = Request.QueryString("linNo")

                Dim strCurrencyCode As String = Request.QueryString("crcode")
                Dim strExchangeRate As String = Request.QueryString("currrate")
                Dim strAmount As String = Request.QueryString("Amount")


                Dim glCode As String
                glCode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(controlacctcode,0) from dbo.view_account where type='" & strtype & "'and code='" & strAccCode & "'")

                Dim strCDType As String
                strCDType = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acc_type_master", "acc_type_mode", "acc_type_name", strtype)


                If strtype = "C" Then
                    txtAdAccountType.Value = "Customer"
                ElseIf strtype = "S" Then
                    txtAdAccountType.Value = "Supplier"
                End If

                txtAccCode.Value = strAccCode
                txtAccName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "view_account", "des", "Code", strAccCode)


                txtCurrencyCode.Value = strCurrencyCode
                txtExchangeRate.Value = strExchangeRate
                txtADAmount.Value = strAmount

                txtAmountAdjust.Value = strAmount
                txtBalAdu.Value = strAmount
                '__________________________________________________________

                Dim myDS As New DataSet
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                If Session("State") = "Edit" Or Session("State") = "Delete" Then
                    'strSqlQry = "exec sp_getadjust '2033948','S','',null,null,'2033000','D','1',1"
                    'strSqlQry = "exec sp_getadjust '2033948','S','" & Session("RefCode") & "'," & strLineNo & ",'RV','2033000','D','01',1"
                    strSqlQry = "exec sp_getadjust '" & txtAccCode.Value & "','" & strtype & "','" & Session("RefCode") & "'," & strLineNo & ",'JV','" & glCode & "','" & strCDType & "','01'," & Val(txtExchangeRate.Value) & ""
                Else
                    strSqlQry = "exec sp_getadjust '" & txtAccCode.Value & "','" & strtype & "','',null,null,'" & glCode & "','" & strCDType & "','01'," & Val(txtExchangeRate.Value) & ""
                    'strSqlQry = "exec sp_getadjust '2033948','S','',null,null,'2033000','D','01',1"
                End If
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS, "AdjustBill")
                'New Account Code Record Add In Last  
                myDS.Tables(0).DefaultView.RowFilter = "accmode='B'"

                grdAdjustBill.DataSource = myDS.Tables("AdjustBill")

                grdAdjustBill.DataBind()
                SqlConn.Close()
                txtgrdAdjRows.Value = grdAdjustBill.Rows.Count
                '__________________________________________________________


                If Session("State") = "Edit" Or Session("State") = "Delete" Then
                    fillDategrd(grdAdPay, True, 5)
                    'strSqlQry = "exec sp_getadjust '2033948','S','',null,null,'2033000','D','1',1"
                    fillAdvanPay(Session("RefCode"))
                    txtgrdAdpayRows.Value = grdAdPay.Rows.Count
                Else
                    fillDategrd(grdAdPay, True, 5)
                    txtgrdAdpayRows.Value = grdAdPay.Rows.Count
                End If

                btnOk.Attributes.Add("onclick", "OnClickOk()")

                btnAExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false){return false}else{window.close()};")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("JournalAdjustBills.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            btnOk.Attributes.Add("onclick", "OnClickOk()")
            btnAExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

        End If
    End Sub
#End Region
#Region "Public Sub fillAdvanPay(ByVal tranid As String)"
    Public Sub fillAdvanPay(ByVal tranid As String)
        Dim clAdBill As New Collection
        Dim intLineNo As Long = 1
        Dim myDS As New DataSet
        Dim mySqlReader As SqlDataReader

        Dim txtADocNo As HtmlInputText
        Dim txtADocType As HtmlInputText
        Dim txtRefNo As HtmlInputText
        Dim txtADebit As HtmlInputText
        Dim txtACredit As HtmlInputText
        Dim txtBaseAmount As HtmlInputText
        Dim txtField2 As HtmlInputText
        Dim txtField3 As HtmlInputText
        Dim txtField4 As HtmlInputText
        Dim txtField5 As HtmlInputText

        Dim dtDate As EclipseWebSolutions.DatePicker.DatePicker
        Dim dtDueDate As EclipseWebSolutions.DatePicker.DatePicker

        Dim intReceiptLinno As Integer = CType(Request.QueryString("linNo"), Integer)

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        myCommand = New SqlCommand("select * from  open_detail Where against_tran_lineno=" & intReceiptLinno & " and against_tran_id='" & tranid & "' and open_mode='D'", SqlConn)
        mySqlReader = myCommand.ExecuteReader()
        If mySqlReader.HasRows Then
            While mySqlReader.Read()
                For Each gvRow In grdAdPay.Rows
                    txtADocNo = gvRow.FindControl("txtADocNo")
                    txtADocType = gvRow.FindControl("txtDocType")
                    txtADebit = gvRow.FindControl("txtADebit")
                    txtACredit = gvRow.FindControl("txtACredit")
                    txtBaseAmount = gvRow.FindControl("txtBaseAmount")
                    dtDate = gvRow.FindControl("dtDate")
                    dtDueDate = gvRow.FindControl("dtDueDate")
                    txtRefNo = gvRow.FindControl("txtRefNo")
                    txtField2 = gvRow.FindControl("txtField2")
                    txtField3 = gvRow.FindControl("txtField3")
                    txtField4 = gvRow.FindControl("txtField4")
                    txtField5 = gvRow.FindControl("txtField5")

                    If txtADocNo.Value.Trim = "" And txtADocType.Value.Trim = "" And txtBaseAmount.Value.Trim = "" Then

                        txtADocNo.Value = mySqlReader("tran_id")
                        dtDate.txtDate.Text = mySqlReader("tran_date")
                        txtADocType.Value = mySqlReader("tran_type")
                        dtDueDate.txtDate.Text = mySqlReader("open_due_date")

                        txtACredit.Value = mySqlReader("open_credit")
                        txtADebit.Value = mySqlReader("open_debit")
                        txtBaseAmount.Value = CType(Val(txtACredit.Value), Decimal) + CType(Val(txtADebit.Value), Decimal) * CType(Val(txtExchangeRate.Value), Decimal)
                        txtRefNo.Value = mySqlReader("open_field1")
                        txtField2.Value = mySqlReader("open_field2")
                        txtField3.Value = mySqlReader("open_field3")
                        txtField4.Value = mySqlReader("open_field4")
                        txtField5.Value = mySqlReader("open_field5")

                        If Val(txtBaseAmount.Value) <> 0 Then
                            txtBalAdu.Value = CType(txtBalAdu.Value, Decimal) - Val(txtBaseAmount.Value)
                        End If

                        Exit For
                    End If
                Next
            End While
        End If
        myCommand.Dispose()
        SqlConn.Close()
    End Sub
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 10
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
        Dim txtAdujAmt As HtmlInputText
        Dim chk As HtmlInputCheckBox
        If e.Row.RowIndex = -1 Then
            Exit Sub
        End If
        gvRow = e.Row
        txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
        chk = gvRow.FindControl("chkBill")

        txtAdujAmt.Attributes.Add("onchange", "javascript:AdjestBillChange('" + CType(txtAdujAmt.ClientID, String) + "','" + gvRow.Cells(8).Text + "')")
        txtAdujAmt.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")

        chk.Attributes.Add("OnClick", "javascript:OnchangeChk('" + CType(txtAdujAmt.ClientID, String) + "','" + CType(chk.ClientID, String) + "')")

        If Val(txtAdujAmt.Value) = 0 Then
            txtAdujAmt.Value = ""
            txtAdujAmt.Style.Add("readonly", "readonly")
        Else
            chk.Checked = True
            txtBalAdu.Value = CType(txtBalAdu.Value, Decimal) - Val(txtAdujAmt.Value)
        End If
    End Sub
#End Region
#Region "Protected Sub grdAdPay_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAdPay.RowDataBound"
    Protected Sub grdAdPay_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAdPay.RowDataBound
        If e.Row.RowIndex = -1 Then
            Exit Sub
        End If
        gvRow = e.Row
        Dim txtADebit As HtmlInputText
        Dim txtACredit As HtmlInputText
        Dim txtBaseAmount As HtmlInputText

        txtADebit = gvRow.FindControl("txtADebit")
        txtACredit = gvRow.FindControl("txtACredit")
        txtBaseAmount = gvRow.FindControl("txtBaseAmount")

        txtADebit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        txtACredit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")

        txtACredit.Attributes.Add("onchange", "javascript:OnChangeAdPay('" + CType(txtACredit.ClientID, String) + "','" + CType(txtADebit.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(txtExchangeRate.ClientID, String) + "')")
        txtADebit.Attributes.Add("onchange", "javascript:OnChangeAdPay('" + CType(txtADebit.ClientID, String) + "','" + CType(txtACredit.ClientID, String) + "','" + CType(txtBaseAmount.ClientID, String) + "','" + CType(txtExchangeRate.ClientID, String) + "')")


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
#Region "Public Function validate_page() As Boolean"
    Public Function validate_page() As Boolean
        validate_page = True
        Dim txtADocNo As HtmlInputText
        Dim txtADocType As HtmlInputText
        Dim txtRefNo As HtmlInputText
        Dim txtADebit As HtmlInputText
        Dim txtACredit As HtmlInputText
        Dim txtBaseAmount As HtmlInputText
        Dim txtField2 As HtmlInputText
        Dim txtField3 As HtmlInputText
        Dim txtField4 As HtmlInputText
        Dim txtField5 As HtmlInputText

        Dim txtAdujAmt As HtmlInputText
        Dim chk As HtmlInputCheckBox

        For Each gvRow In grdAdjustBill.Rows
            chk = gvRow.FindControl("chkBill")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
            If chk.Checked = True Then
                If Val(txtAdujAmt.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter the adujust amount .');", True)
                    SetFocus(txtAdujAmt)
                    validate_page = False
                    Exit Function
                End If
            End If
        Next


        Dim dtDate As EclipseWebSolutions.DatePicker.DatePicker
        Dim dtDueDate As EclipseWebSolutions.DatePicker.DatePicker

        For Each gvRow In grdAdPay.Rows
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtADebit = gvRow.FindControl("txtADebit")
            txtACredit = gvRow.FindControl("txtACredit")
            txtBaseAmount = gvRow.FindControl("txtBaseAmount")
            dtDate = gvRow.FindControl("dtDate")
            dtDueDate = gvRow.FindControl("dtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")

            If txtADocNo.Value.Trim <> "" Or txtADocType.Value.Trim <> "" Or txtBaseAmount.Value.Trim <> "" Then
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

                If dtDate.txtDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter date.');", True)
                    SetFocus(dtDate)
                    validate_page = False
                    Exit Function
                End If

                If dtDueDate.txtDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter due date.');", True)
                    SetFocus(dtDueDate)
                    validate_page = False
                    Exit Function
                End If


                If Val(txtADebit.Value) <= 0 And Val(txtACredit.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter debit or credit amount.');", True)
                    SetFocus(txtADebit)
                    validate_page = False
                    Exit Function
                End If
                Dim dFromDate, dToDate As Date
                dFromDate = objDateTime.ConvertDateromTextBoxToDatabase(dtDate.txtDate.Text)
                dToDate = objDateTime.ConvertDateromTextBoxToDatabase(dtDueDate.txtDate.Text)

                If dFromDate <= dToDate Then

                Else
                    validate_page = False
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select due dates should be greater than date.');", True)
                    SetFocus(dToDate)
                    Exit Function
                End If
            End If

        Next
    End Function
#End Region
#Region "Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click"
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim clAdBill As New Collection
        Dim chk As HtmlInputCheckBox
        Dim txtAdujAmt As HtmlInputText
        Dim strType As String = Request.QueryString("AccType")
        Dim intReceiptLinno As Integer = CType(Request.QueryString("linNo"), Integer)
        Dim strTranId As String = Request.QueryString("TranId")

        Dim sqlTrans As SqlTransaction
        Dim intLineNo As Integer = 1
        Dim strLineKey As String


        If validate_page() = False Then
            Exit Sub
        End If

        If Session("State") = "Edit" Then
            If Session("Collection").ToString <> "" Then
                clAdBill = CType(Session("Collection"), Collection)

            End If
        End If


        If Val(txtBalAdu.Value) <> 0 Then

            If txtflag.Value = 1 Then
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

                Dim optionval As String
                optionval = objUtils.GetAutoDocNo("ADVANCE", SqlConn, sqlTrans)

                strLineKey = intLineNo & ":" & intReceiptLinno
                AddCollection(clAdBill, "LinNo" & strLineKey, intLineNo.ToString)
                AddCollection(clAdBill, "TranId" & strLineKey, optionval.Trim)
                AddCollection(clAdBill, "TranDate" & strLineKey, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
                AddCollection(clAdBill, "TranType" & strLineKey, "JV")
                AddCollection(clAdBill, "DueDate" & strLineKey, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))

                AddCollection(clAdBill, "Credit" & strLineKey, 0)
                AddCollection(clAdBill, "Debit" & strLineKey, txtBalAdu.Value)

                AddCollection(clAdBill, "RefNo" & strLineKey, "")
                AddCollection(clAdBill, "Field2" & strLineKey, "")
                AddCollection(clAdBill, "Field3" & strLineKey, "")
                AddCollection(clAdBill, "Field4" & strLineKey, "")
                AddCollection(clAdBill, "Field5" & strLineKey, "")
                AddCollection(clAdBill, "OpenMode" & strLineKey, "D")

                sqlTrans.Commit()
                SqlConn.Close()
                intLineNo = 2
            End If
        End If



        Dim TranType As String = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acc_type_master", "acc_type_mode", "acc_type_name", strType)

        For Each gvRow In grdAdjustBill.Rows
            chk = gvRow.FindControl("chkBill")
            txtAdujAmt = gvRow.FindControl("txtAdujustAmt")
            If chk.Checked = True Then
                strLineKey = intLineNo & ":" & intReceiptLinno
                AddCollection(clAdBill, "LinNo" & strLineKey, intLineNo.ToString)
                AddCollection(clAdBill, "TranId" & strLineKey, gvRow.Cells(grd_col.DocNo).Text)
                AddCollection(clAdBill, "TranDate" & strLineKey, gvRow.Cells(grd_col.dDate).Text)
                AddCollection(clAdBill, "TranType" & strLineKey, gvRow.Cells(grd_col.DocType).Text)
                AddCollection(clAdBill, "DueDate" & strLineKey, gvRow.Cells(grd_col.DueDate).Text)

                If TranType = "C" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, txtAdujAmt.Value)
                    AddCollection(clAdBill, "Debit" & strLineKey, 0)
                ElseIf TranType = "D" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, 0)
                    AddCollection(clAdBill, "Debit" & strLineKey, txtAdujAmt.Value)
                End If

                AddCollection(clAdBill, "RefNo" & strLineKey, gvRow.Cells(grd_col.RefNo).Text.Trim)
                AddCollection(clAdBill, "Field2" & strLineKey, gvRow.Cells(grd_col.Field2).Text.Trim)
                AddCollection(clAdBill, "Field3" & strLineKey, gvRow.Cells(grd_col.Field3).Text.Trim)
                AddCollection(clAdBill, "Field4" & strLineKey, gvRow.Cells(grd_col.Field4).Text.Trim)
                AddCollection(clAdBill, "Field5" & strLineKey, gvRow.Cells(grd_col.Field5).Text.Trim)
                AddCollection(clAdBill, "OpenMode" & strLineKey, "B")
                intLineNo = intLineNo + 1
            End If
        Next


        Dim txtADocNo As HtmlInputText
        Dim txtADocType As HtmlInputText
        Dim txtRefNo As HtmlInputText
        Dim txtADebit As HtmlInputText
        Dim txtACredit As HtmlInputText
        Dim txtBaseAmount As HtmlInputText
        Dim txtField2 As HtmlInputText
        Dim txtField3 As HtmlInputText
        Dim txtField4 As HtmlInputText
        Dim txtField5 As HtmlInputText

        Dim dtDate As EclipseWebSolutions.DatePicker.DatePicker
        Dim dtDueDate As EclipseWebSolutions.DatePicker.DatePicker

        For Each gvRow In grdAdPay.Rows
            txtADocNo = gvRow.FindControl("txtADocNo")
            txtADocType = gvRow.FindControl("txtDocType")
            txtADebit = gvRow.FindControl("txtADebit")
            txtACredit = gvRow.FindControl("txtACredit")
            txtBaseAmount = gvRow.FindControl("txtBaseAmount")
            dtDate = gvRow.FindControl("dtDate")
            dtDueDate = gvRow.FindControl("dtDueDate")
            txtRefNo = gvRow.FindControl("txtRefNo")
            txtField2 = gvRow.FindControl("txtField2")
            txtField3 = gvRow.FindControl("txtField3")
            txtField4 = gvRow.FindControl("txtField4")
            txtField5 = gvRow.FindControl("txtField5")

            If txtADocNo.Value.Trim <> "" And txtADocType.Value.Trim <> "" And txtBaseAmount.Value.Trim <> "" Then
                strLineKey = intLineNo & ":" & intReceiptLinno
                AddCollection(clAdBill, "LinNo" & strLineKey, intLineNo.ToString)
                AddCollection(clAdBill, "TranId" & strLineKey, txtADocNo.Value.Trim)
                AddCollection(clAdBill, "TranDate" & strLineKey, dtDate.txtDate.Text.Trim)
                AddCollection(clAdBill, "TranType" & strLineKey, txtADocType.Value.Trim)
                AddCollection(clAdBill, "DueDate" & strLineKey, dtDueDate.txtDate.Text)

                If TranType = "C" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, txtACredit.Value)
                    AddCollection(clAdBill, "Debit" & strLineKey, 0)
                ElseIf TranType = "D" Then
                    AddCollection(clAdBill, "Credit" & strLineKey, 0)
                    AddCollection(clAdBill, "Debit" & strLineKey, txtADebit.Value)
                End If

                AddCollection(clAdBill, "RefNo" & strLineKey, txtRefNo.Value.Trim)
                AddCollection(clAdBill, "Field2" & strLineKey, txtField2.Value.Trim)
                AddCollection(clAdBill, "Field3" & strLineKey, txtField3.Value.Trim)
                AddCollection(clAdBill, "Field4" & strLineKey, txtField4.Value.Trim)
                AddCollection(clAdBill, "Field5" & strLineKey, txtField5.Value.Trim)
                AddCollection(clAdBill, "OpenMode" & strLineKey, "D")

                intLineNo = intLineNo + 1
            End If
        Next

        Session.Add("Collection", clAdBill)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.close();", True)
    End Sub
#End Region
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
#End Region

    Protected Sub btnAExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAExit.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.close();", True)
    End Sub
End Class
