

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region
Partial Class AccountsModule_SalesInvoiceFreeForm
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim mbasecurrency As String = ""
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim gvRow As GridViewRow
    Dim chckDeletion As CheckBox
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim ScriptOpenModalDialog As String = "OpenModalDialog('{0}','{1}');"
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
#End Region


    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function Getacctlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)

        Try

            strSqlQry = "select acctcode, acctname from acctmast where controlyn='N' and bankyn='N'  and  acctname like  '%" & Trim(prefixText) & "%' and div_code in (" & HttpContext.Current.Session("div_code") & ") order by acctcode"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))

                Next
            End If
            Return acctnames
        Catch ex As Exception
            Return acctnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getsalesuserlist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)

        Try

            strSqlQry = "select usercode,username from usermaster where active=1 and username like '%" & Trim(prefixText) & "%' order by username"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("username").ToString(), myDS.Tables(0).Rows(i)("usercode").ToString()))

                Next
            End If
            Return acctnames
        Catch ex As Exception
            Return acctnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getctrylist(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)

        Try

            strSqlQry = "select a.ctrycode,c.ctryname from agentmast_countries a  left join ctrymast c on a.ctrycode=c.ctrycode  where  c.ctryname like  '%" & Trim(prefixText) & "%' and  a.agentcode='" & contextKey & "' union all  select distinct a.ctrycode,c.ctryname from partymast a  left join ctrymast c on a.ctrycode=c.ctrycode  where a.partycode='" & contextKey & "' and c.ctryname like  '%" & Trim(prefixText) & "%' order by ctryname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))

                Next
            End If
            Return acctnames
        Catch ex As Exception
            Return acctnames
        End Try

    End Function



    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function Getcostcenterlist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim costcenter As New List(Of String)
        Try

            strSqlQry = "select costcenter_code, costcenter_name from costcenter_master where active=1 and  costcenter_name like  '%" & Trim(prefixText) & "%'  order by costcenter_code"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    costcenter.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("costcenter_name").ToString(), myDS.Tables(0).Rows(i)("costcenter_code").ToString()))

                Next
            End If
            Return costcenter
        Catch ex As Exception
            Return costcenter
        End Try

    End Function



    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getcustlist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Custnames As New List(Of String)
        Try

            strSqlQry = "select agentcode,agentname from agentmast where active=1 and  agentname like  '%" & Trim(prefixText) & "%' and divcode in (" & HttpContext.Current.Session("div_code") & ") order by agentname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Custnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))

                Next
            End If
            Return Custnames
        Catch ex As Exception
            Return Custnames
        End Try

    End Function


    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppnames As New List(Of String)
        Try

            strSqlQry = "select partycode,partyname from partymast where active=1 and  partyname like  '%" & Trim(prefixText) & "%' order by partyname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))

                Next
            End If
            Return suppnames
        Catch ex As Exception
            Return suppnames
        End Try

    End Function



#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            Session("freeform_Detail") = Nothing
            Dim crdays As Double = 0
            chkPost.Checked = False
            Dim myDS As New DataSet 'Tanvir 08112023
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            'myCommand = New SqlCommand("Select * from freeforminvoice_master Where tran_id='" & RefCode & "' and tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "' and div_code='" & CType(txtdiv.Value, String) & "'", SqlConn)
            'Tanvir 08112023
            Dim strSqlQry As String
            strSqlQry = "Select * from freeforminvoice_master Where tran_id='" & RefCode & "' and tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "' and div_code='" & CType(txtdiv.Value, String) & "'"
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                Session("freeform_Detail") = CType(myDS.Tables(0), DataTable)
            End If
            'Tanvir 08112023
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If ViewState("SalesInvFreeFormState") <> "Copy" Then
                        If IsDBNull(mySqlReader("post_state")) = False Then
                            If CType(mySqlReader("post_state"), String) = "P" Then
                                lblPostmsg.Text = "Posted"
                                lblPostmsg.ForeColor = Drawing.Color.Red
                                chkPost.Checked = True
                            Else
                                lblPostmsg.Text = "UnPosted"
                                lblPostmsg.ForeColor = Drawing.Color.Green
                            End If
                        Else
                            lblPostmsg.Text = "UnPosted"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                        If IsDBNull(mySqlReader("tran_id")) = False Then
                            Me.txtDocNo.Text = CType(mySqlReader("tran_id"), String)
                        Else
                            Me.txtDocNo.Text = ""
                        End If
                    End If

                    If IsDBNull(mySqlReader("cancel_state")) = False Then
                        If CType(mySqlReader("cancel_state"), String) = "Y" Then
                            lblPostmsg.Text = "Cancelled"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    End If

                    If ViewState("SalesInvFreeFormState") <> "Copy" Then
                        If IsDBNull(mySqlReader("invoice_date")) = False Then
                            Me.txtJDate.Text = CType(Format(CType(mySqlReader("invoice_date"), Date), "dd/MM/yyyy"), String)
                        Else
                            Me.txtJDate.Text = ""
                        End If
                    Else
                        Me.txtJDate.Text = CType(Format(CType(Now, Date), "dd/MM/yyyy"), String)
                    End If

                    If IsDBNull(mySqlReader("supref")) = False Then
                        Me.txtrefno.Text = CType(mySqlReader("supref"), String)
                    Else
                        Me.txtrefno.Text = ""
                    End If

                    If IsDBNull(mySqlReader("invoicetype")) = False Then
                        Me.ddlinvoicetype.Text = CType(mySqlReader("invoicetype"), String)
                    Else
                        Me.ddlinvoicetype.Text = "Tax Invoice"
                    End If
                    If IsDBNull(mySqlReader("due_date")) = False Then
                        Me.txtTDate.Text = CType(Format(CType(mySqlReader("due_date"), Date), "dd/MM/yyyy"), String)
                    Else
                        Me.txtTDate.Text = ""
                    End If
                    If IsDBNull(mySqlReader("acc_type")) = False Then
                        Select Case CType(mySqlReader("acc_type"), String)
                            Case "C"
                                Me.ddltype.Text = "Customer"
                            Case "S"
                                Me.ddltype.Text = "Supplier"
                        End Select

                    Else
                        ddltype.Text = "Customer"
                    End If


                    If IsDBNull(mySqlReader("supcode")) = False Then
                        TxtCustCode.Text = CType(mySqlReader("supcode"), String)
                        Select Case CType(mySqlReader("acc_type"), String)
                            Case "C"
                                TxtCustName_AutoCompleteExtender.ServiceMethod = "Getcustlist"
                                lblcust.Text = "Customer"
                                PnlType.GroupingText = "Customer"

                                TxtcustName.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentname From agentmast where agentcode='" & TxtCustCode.Text & "'"), String)
                                txtcontrolac.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select controlacctcode From agentmast where agentcode='" & TxtCustCode.Text & "'"), String)
                                txtcontrolacname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctname From acctmast where acctcode='" & txtcontrolac.Text & "'"), String)
                                txttrnno.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNO From agentmast where agentcode='" & TxtCustCode.Text & "'"), String)
                                crdays = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(crdays,0) From agentmast where agentcode='" & TxtCustCode.Text & "'"), String)

                                If crdays = 0 Then
                                    txtTDate.Text = Now
                                Else
                                    If ViewState("SalesInvFreeFormState") = "Copy" Then
                                        txtTDate.Text = CType(Format(CType(DateAdd("d", crdays, txtJDate.Text), Date), "dd/MM/yyyy"), String)
                                    End If

                                End If


                            Case "S"

                                TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsupplist"
                                lblcust.Text = "Supplier"
                                PnlType.GroupingText = "Supplier"

                                If ViewState("SalesInvFreeFormState") = "New" Or ViewState("SalesInvFreeFormState") = "Copy" Then
                                    crdays = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(crdays,0) From partymast where partycode='" & TxtCustCode.Text & "'"), String)
                                    If crdays = 0 Then
                                        txtTDate.Text = Now
                                    Else
                                        If ViewState("SalesInvFreeFormState") = "Copy" Then
                                            txtTDate.Text = CType(Format(CType(DateAdd("d", crdays, txtJDate.Text), Date), "dd/MM/yyyy"), String)
                                        End If
                                    End If
                                End If



                                TxtcustName.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select partyname From partymast where partycode='" & TxtCustCode.Text & "'"), String)
                                txtAliasName.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select hotelalias From partymast where partycode='" & TxtCustCode.Text & "'"), String)
                                txtcontrolac.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select controlacctcode From partymast where partycode='" & TxtCustCode.Text & "'"), String)
                                txtcontrolacname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctname From acctmast where acctcode='" & txtcontrolac.Text & "'"), String)
                                txttrnno.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNO From partymast where partycode='" & TxtCustCode.Text & "'"), String)


                        End Select





                    Else


                        TxtCustName_AutoCompleteExtender.ServiceMethod = "Getcustlist"
                        lblcust.Text = "Customer"
                        PnlType.GroupingText = "Customer"

                    End If

                    'If ddlType.Value <> "[Select]" Then
                    '    lblCustCode.Text = ddlType.Items(ddlType.SelectedIndex).Text & " Code <font color='Red'> *</font>"
                    '    lblCustName.Text = ddlType.Items(ddlType.SelectedIndex).Text & " Name "
                    'Else
                    '    lblCustCode.Text = "Code <font color='Red'> *</font>"
                    '    lblCustName.Text = "Name "
                    'End If

                    If IsDBNull(mySqlReader("sourcecountry")) = False Then

                        txtsourcecode.Text = CType(mySqlReader("sourcecountry"), String)
                        txtsourcectry.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select ctryname From ctrymast where ctrycode='" & txtsourcecode.Text & "'"), String)
                    Else
                        txtsourcectry.Text = ""
                        txtsourcecode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.txtcurr.Text = CType(mySqlReader("currcode"), String)
                        txtcurrname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currname From currmast where currcode='" & txtcurr.Text & "'"), String)
                        lblsubtotal_curr.Text = "SubTotal(" & Me.txtcurr.Text & ")"
                        grdServiceInvoice.Columns("7").HeaderText = "Value(" & txtcurr.Text & ")"
                    Else
                        Me.txtcurr.Text = ""
                        txtcurrname.Text = ""
                    End If
                    If IsDBNull(mySqlReader("convrate")) = False Then
                        Me.txtconvrate.Text = CType(mySqlReader("convrate"), String)
                    Else
                        Me.txtconvrate.Text = ""
                    End If
                    If IsDBNull(mySqlReader("total")) = False Then
                        Me.txtsalevaltot.Text = CType(DecRound(CType(mySqlReader("total"), String)), String)           'Added Round param 12/11/2018
                    Else
                        Me.txtsalevaltot.Text = ""
                    End If
                    If IsDBNull(mySqlReader("Basetotal")) = False Then
                        Me.txtBasesalevaltot.Text = CType(DecRound(CType(mySqlReader("Basetotal"), String)), String)
                    Else
                        Me.txtBasesalevaltot.Text = ""
                    End If
                    If IsDBNull(mySqlReader("narration")) = False Then
                        Me.txtnarration.Text = CType(mySqlReader("narration"), String)
                    Else
                        Me.txtnarration.Text = ""
                    End If
                    If IsDBNull(mySqlReader("sperson")) = False Then

                        txtsalescode.Text = CType(mySqlReader("sperson"), String)
                        txtsales.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select username From usermaster where usercode='" & txtsalescode.Text & "'"), String)
                    Else
                        txtsalescode.Text = ""
                        txtsales.Text = ""

                    End If


                    If IsDBNull(mySqlReader("bookingno")) = False Then
                        Me.txtbookingno.Text = CType(mySqlReader("bookingno"), String)
                    Else
                        Me.txtbookingno.Text = ""
                    End If

                    'FillControlACCODENames()
                    'txtConversion.Disabled = False
                    'If Trim(txtbasecurr.Value) = Trim(txtCurrency.Value) Then
                    '    txtConversion.Disabled = True
                    'End If

                End If

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)                  'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)                 'sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)                 'connection close           
        End Try
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("SalesInvFreeFormState") = "New" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = False
        ElseIf ViewState("SalesInvFreeFormState") = "Copy" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = False
        ElseIf ViewState("SalesInvFreeFormState") = "Edit" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = False
        ElseIf ViewState("SalesInvFreeFormState") = "Delete" Or ViewState("SalesInvFreeFormState") = "View" Or ViewState("SalesInvFreeFormState") = "Cancel" Or ViewState("SalesInvFreeFormState") = "undoCancel" Then
            txtDocNo.Enabled = False
            txtJDate.Enabled = False
            txtTDate.Enabled = False
            txtrefno.Enabled = False
            txtnarration.Enabled = False
            'txtcurr = True
            'txtConversion.Disabled = True
            ddltype.Enabled = False
            txtsales.Enabled = False
            TxtcustName.Enabled = False
            ImgBtnFrmDt.Enabled = False
            ImageButton1.Enabled = False
            txtbookingno.Enabled = False
            DisableGrid()
            btnAddLine.Visible = False
            btnDelLine.Visible = False
            ' btnAdjustBill.Visible = True
            btnPrint.Visible = False
            btnPdfReport.Visible = False
            btnSave.Visible = False
            chkPost.Visible = False
        End If
        If ViewState("SalesInvFreeFormState") = "View" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = True
        ElseIf ViewState("SalesInvFreeFormState") = "Delete" Then
            btnSave.Visible = True
        ElseIf ViewState("SalesInvFreeFormState") = "Cancel" Or ViewState("SalesInvFreeFormState") = "undoCancel" Then
            btnPrint.Visible = False
            btnPdfReport.Visible = False
            btnSave.Visible = True
        End If
    End Sub

#End Region

    Private Sub DisableGrid()
        'Disable for DebitNote
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        Dim txtPartic As TextBox
        Dim txtCValue As TextBox
        Dim txtKWValue As TextBox

        Try
            For Each gvRow In grdServiceInvoice.Rows
                ddlAccCode = gvRow.FindControl("ddlAccountCode")
                ddlAccName = gvRow.FindControl("ddlAccountName")
                ddlCCCode = gvRow.FindControl("ddlCostCode")
                ddlCCName = gvRow.FindControl("ddlCostName")
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCValue = gvRow.FindControl("txtCurrValue")
                txtKWValue = gvRow.FindControl("txtKWDValue")
                chckDeletion = gvRow.FindControl("chckDeletion")

                'ddlAccCode.Disabled = True
                'ddlAccName.Disabled = True
                'ddlCCCode.Disabled = True
                'ddlCCName.Disabled = True
                'txtPartic.Enabled = False
                'txtCValue.Enabled = False
                ' txtKWValue.Enabled = False

                chckDeletion.Enabled = False
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#Region "Private Sub ShowGridDebitNote(ByVal RefCode As String)"
    Private Sub ShowGridDebitNote(ByVal RefCode As String)
        Try

            Dim lblLineNo As Label
            Dim txtacctcode, txtacct As TextBox
            Dim txtcostcentercode, txtcostcenter As TextBox
            Dim txtvatamt As TextBox
            Dim txtparticulars As TextBox
            Dim txtvatperc As TextBox
            Dim txtcurrvalue, txtbasevalue, txtnontaxableamt, txttaxableamt As TextBox
            Dim ddlvattype As DropDownList

            Dim taxableamt, nontaxable, vatamttot As Double
            taxableamt = 0
            nontaxable = 0

            vatamttot = 0

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select * from freeforminvoice_detail Where tran_id='" & RefCode & "' and tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "' and div_code='" & CType(txtdiv.Value, String) & "'", SqlConn)
            ''Tanvir 02/10/2023 point6
            'Dim myds As New DataSet
            'myDataAdapter = New SqlDataAdapter(myCommand)
            'myDataAdapter.Fill(myDS)
            'If myDS.Tables(0).Rows.Count > 0 Then
            '    Session("freeform_Detail") = CType(myds.Tables(0), DataTable)
            'End If
            'Tanvir 02/10/2023 point6
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdServiceInvoice.Rows
                        lblLineNo = gvRow.FindControl("lblLineID")
                        If lblLineNo.Text = CType(mySqlReader("Sno"), String) - 1 Then
                            txtacctcode = gvRow.FindControl("txtacctcode")
                            txtacct = gvRow.FindControl("txtacct")
                            txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                            txtcostcenter = gvRow.FindControl("txtcostcenter")
                            txtvatamt = gvRow.FindControl("txtvatamt")
                            txtparticulars = gvRow.FindControl("txtParticulars")
                            txtvatperc = gvRow.FindControl("txtvatperc")
                            txtcurrvalue = gvRow.FindControl("txtcurrvalue")
                            txtbasevalue = gvRow.FindControl("txtbasevalue")
                            txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                            txttaxableamt = gvRow.FindControl("txttaxableamt")
                            ddlvattype = gvRow.FindControl("ddlvattype")

                            If IsDBNull(mySqlReader("acc_code")) = False Then
                                txtacct.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("acc_code"), String))
                                txtacctcode.Text = CType(mySqlReader("acc_code"), String)
                            End If

                            If IsDBNull(mySqlReader("costcenter_code")) = False Then
                                txtcostcenter.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "costcenter_master", "costcenter_name", "costcenter_code", CType(mySqlReader("costcenter_code"), String))
                                txtcostcentercode.Text = CType(mySqlReader("costcenter_code"), String)
                            End If

                            If IsDBNull(mySqlReader("particulars")) = False Then
                                txtparticulars.Text = CType(mySqlReader("particulars"), String)
                            End If

                            If IsDBNull(mySqlReader("amount")) = False Then
                                txtcurrvalue.Text = CType(DecRound(CType(mySqlReader("amount"), String)), String)

                            End If

                            If IsDBNull(mySqlReader("baseamount")) = False Then
                                txtbasevalue.Text = DecRound(CType(mySqlReader("baseamount"), String))
                            End If


                            If IsDBNull(mySqlReader("vattype")) = False Then
                                ddlvattype.Text = (CType(mySqlReader("vattype"), String))
                                If ddlvattype.Text = "" Then
                                    txtvatperc.Enabled = False
                                    txtvatamt.Enabled = False
                                Else
                                    Select Case ddlvattype.Text
                                        Case "Taxable"
                                            txtvatperc.Enabled = True
                                            txtvatamt.Enabled = False
                                        Case "ZeroRated"
                                            txtvatperc.Enabled = False
                                            txtvatamt.Enabled = False
                                        Case "Exempt"
                                            txtvatperc.Enabled = False
                                            txtvatamt.Enabled = False
                                            'Tanvir 20062023
                                        Case "OutOfScope"
                                            txtvatperc.Enabled = False
                                            txtvatamt.Enabled = False
                                        Case "RCM"
                                            txtvatperc.Enabled = True
                                            txtvatamt.Enabled = False
                                            'Tanvir 20062023
                                    End Select


                                End If
                            End If
                            If IsDBNull(mySqlReader("nontaxamt")) = False Then
                                txtnontaxableamt.Text = DecRound(CType(mySqlReader("nontaxamt"), String))
                                nontaxable = nontaxable + DecRound(CType(mySqlReader("nontaxamt"), String))
                            End If

                            If IsDBNull(mySqlReader("taxamt")) = False Then
                                txttaxableamt.Text = DecRound(CType(mySqlReader("taxamt"), String))
                                taxableamt = taxableamt + DecRound(CType(mySqlReader("taxamt"), String))
                            End If
                            If IsDBNull(mySqlReader("vatperc")) = False Then
                                txtvatperc.Text = DecRound(CType(mySqlReader("vatperc"), String))
                            End If

                            If IsDBNull(mySqlReader("vatamt")) = False Then
                                txtvatamt.Text = DecRound(CType(mySqlReader("vatamt"), String))
                                vatamttot = vatamttot + DecRound(CType(mySqlReader("vatamt"), String))
                            End If
                            Exit For
                        End If
                    Next
                End While

                txtnontaxableamttot.Text = DecRound(nontaxable)
                txttaxableamttot.Text = DecRound(taxableamt)
                txtvatamttot.Text = DecRound(vatamttot)

                If txtconvrate.text = "" Then
                    txtconvrate.text = 0
                End If
                txtBasenontaxableamttot.Text = DecRound(nontaxable * Val(txtconvrate.Text))

                txtbasetaxableamttot.Text = DecRound(taxableamt * Val(txtconvrate.Text))
                txtBasevatamttot.Text = DecRound(vatamttot * Val(txtconvrate.Text))
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)              'connection close           
        End Try
    End Sub
#End Region


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        If IsPostBack = False Then
            Try

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SalesinvoiceFreeForm.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ViewState.Add("SalesInvFreeFormState", Request.QueryString("State"))
            ViewState.Add("SalesInvFreeFormRefCode", Request.QueryString("RefCode"))
            ViewState.Add("CNDNOpen_type", Request.QueryString("CNDNOpen_type"))
            ViewState.Add("div_code", Request.QueryString("div_code"))
            txtdiv.Value = ViewState("div_code")
            Session("div_code") = ViewState("div_code")
            'Session("freeform_Detail") = Nothing 'Tanvir 20102023
            Dim opt As String = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 5503)
            If (opt = "Y") Then
                lblbookingno.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 5506)
            End If

            mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            If IsPostBack = False Then
                txtconnection.Value = Session("dbconnectionName")
                txtdiv.Value = ViewState("div_code")
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                If check_Privilege() = 1 Then

                    chkPost.Enabled = True
                    chkPost.Checked = True
                Else

                    chkPost.Enabled = False
                    chkPost.Checked = False ' true
                End If

                'fillgrd(grdServiceInvoice, True)

                'FillGridDebitNoteDefault()
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
                txtbase.Value = mbasecurrency
                txtvatpercentage.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=2013"), String)



                Select Case ViewState("CNDNOpen_type")
                    Case "IN"
                        lblHeading.Text = "Sales Invoice Free Form "
                        ddltype.SelectedValue = "Customer"
                        'ddltype.Enabled = False

                    Case "PI"
                        lblHeading.Text = "Purchase Invoice Free Form "
                        lblInvNo.Text = "PI No"
                        ddltype.SelectedValue = "Supplier"
                        LblInvoiceDate.Text = "PI Date"
                        lblInvtype.Text = "PI Type"
                        lblInvtype.Style("TEXT-ALIGN") = "right"

                        'ddltype.Enabled = False
                    Case ("PE")
                        lblHeading.Text = "Purchase Expense Search"
                        lblInvNo.Text = "PE No"
                        ddltype.SelectedValue = "Supplier"
                        LblInvoiceDate.Text = "PE Date"
                        lblInvtype.Text = "PE Type"
                        lblInvtype.Style("TEXT-ALIGN") = "right"
                        'ddltype.Enabled = False
                    Case "DN"
                        lblHeading.Text = "Other Debit Note"
                        lblInvNo.Text = "DN No"
                        LblInvoiceDate.Text = "DN Date"
                        lblInvtype.Text = "DN Type"
                        lblInvtype.Style("TEXT-ALIGN") = "right"
                    Case "CN"
                        lblHeading.Text = "Other Credit Note "
                        lblInvNo.Text = "CN No"
                        LblInvoiceDate.Text = "CN Date"
                        lblInvtype.Text = "CN Type"
                        lblInvtype.Style("TEXT-ALIGN") = "right"
                End Select

                ddltype_SelectedIndexChanged(ddltype, e)
                'txtconnection.Value = Session("dbconnectionName")
                'ViewState("SalesInvFreeFormState") = "New"

                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master ")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If
                Dim adjcolno As String
                adjcolno = objUtils.GetAutoDocNoWTnew(Session("dbconnectionName"), "ADJCOL")
                txtAdjcolno.Value = adjcolno
                Session.Add("Collection" & ":" & adjcolno, "")


                If ViewState("SalesInvFreeFormState") = "New" Then
                    SetFocus(txtJDate)
                    txtJDate.Text = Format("dd/MM/yyyy", CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date))


                    Select Case ViewState("CNDNOpen_type")
                        Case "IN"
                            lblHeading.Text = "Add New Sales Invoice Free Form "
                            'Added by Natraj on 03/03/2021
                            Dim acctCode As String = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select v.code from view_account v inner join reservation_parameters r on v.code=r.option_selected where r.param_id='4502' and v.type='G'"), String)
                            If String.IsNullOrEmpty(acctCode) Then objUtils.MessageBox("VAT account code is missing.", Page)

                        Case "PI"
                            lblHeading.Text = "Add New Purchase Invoice Free Form "
                            'Added by Natraj on 03/03/2021
                            Dim acctCode As String = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select v.code from view_account v inner join reservation_parameters r on v.code=r.option_selected where r.param_id='4502' and v.type='G'"), String)
                            If String.IsNullOrEmpty(acctCode) Then objUtils.MessageBox("VAT account code is missing.", Page)
                        Case "PE"
                            lblHeading.Text = "Add New Purchase Expense "
                        Case "DN"
                            lblHeading.Text = "Add New Debit Note "
                        Case "CN"
                            lblHeading.Text = "Add New Credit Note "
                    End Select



                    btnSave.Text = "Save"
                    FillGrids()
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    txtTDate.Text = Now
                    txtJDate.Text = Now

                ElseIf ViewState("SalesInvFreeFormState") = "Copy" Then
                    SetFocus(txtJDate)


                    Select Case ViewState("CNDNOpen_type")
                        Case "IN"
                            lblHeading.Text = "Copy New Sales Invoice Free Form "

                        Case "PI"
                            lblHeading.Text = "Copy New Purchase Invoice Free Form "
                        Case "PE"
                            lblHeading.Text = "Copy New Purchase Expense "
                        Case "DN"
                            lblHeading.Text = "Copy New Debit Note "
                        Case "CN"
                            lblHeading.Text = "Copy New Credit Note "
                    End Select


                    btnSave.Text = "Save"
                    FillGrids()
                    ShowRecord(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("SalesInvFreeFormState") = "Edit" Then
                    SetFocus(txtJDate)



                    Select Case ViewState("CNDNOpen_type")
                        Case "IN"
                            lblHeading.Text = "Edit Sales Invoice Free Form "

                        Case "PI"
                            lblHeading.Text = "Edit Purchase Invoice Free Form "
                        Case "PE"
                            lblHeading.Text = "Edit Purchase Expense "
                        Case "DN"
                            lblHeading.Text = "Edit Debit Note "
                        Case "CN"
                            lblHeading.Text = "Edit Credit Note "
                    End Select

                    btnSave.Text = "Update"
                    FillGrids()
                    ShowRecord(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    'fillcollection(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")

                ElseIf ViewState("SalesInvFreeFormState") = "View" Then
                    SetFocus(btnCancel)
                    Select Case ViewState("CNDNOpen_type")
                        Case "IN"
                            lblHeading.Text = "View Sales Invoice Free Form "

                        Case "PI"
                            lblHeading.Text = "View Purchase Invoice Free Form "
                        Case "PE"
                            lblHeading.Text = "View Purchase Expense "
                        Case "DN"
                            lblHeading.Text = "View Debit Note "
                        Case "CN"
                            lblHeading.Text = "View Credit Note "
                    End Select



                    btnSave.Visible = False
                    FillGrids()
                    ShowRecord(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    ' fillcollection(CType(ViewState("SalesInvFreeFormRefCode"), String))

                ElseIf ViewState("SalesInvFreeFormState") = "Delete" Then
                    SetFocus(btnSave)
                    Select Case ViewState("CNDNOpen_type")
                        Case "IN"
                            lblHeading.Text = "Delete Sales Invoice Free Form "

                        Case "PI"
                            lblHeading.Text = "Delete Purchase Invoice Free Form "
                        Case "PE"
                            lblHeading.Text = "Delete Purchase Expense "
                        Case "DN"
                            lblHeading.Text = "Delete Debit Note "
                        Case "CN"
                            lblHeading.Text = "Delete Credit Note "
                    End Select
                    btnSave.Text = "Delete"
                    FillGrids()
                    ShowRecord(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    'fillcollection(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("SalesInvFreeFormState") = "Cancel" Then
                    SetFocus(btnSave)
                    Select Case ViewState("CNDNOpen_type")
                        Case "IN"
                            lblHeading.Text = "Cancel Sales Invoice Free Form "

                        Case "PI"
                            lblHeading.Text = "Cancel Purchase Invoice Free Form "
                        Case "PE"
                            lblHeading.Text = "Cancel Purchase Expense "
                        Case "DN"
                            lblHeading.Text = "Cancel Debit Note "
                        Case "CN"
                            lblHeading.Text = "Cancel Credit Note "
                    End Select
                    btnSave.Text = "Cancel"
                    FillGrids()
                    ShowRecord(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    'fillcollection(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Cancel')")
                ElseIf ViewState("SalesInvFreeFormState") = "undoCancel" Then
                    SetFocus(btnSave)
                    Select Case ViewState("CNDNOpen_type")
                        Case "IN"
                            lblHeading.Text = "UndoCancel Sales Invoice Free Form "

                        Case "PI"
                            lblHeading.Text = "UndoCancel Purchase Invoice Free Form "
                        Case "PE"
                            lblHeading.Text = "UndoCancel Purchase Expense "
                        Case "DN"
                            lblHeading.Text = "UndoCancel Debit Note "
                        Case "CN"
                            lblHeading.Text = "UndoCancel Credit Note "
                    End Select
                    btnSave.Text = "Undo"
                    FillGrids()
                    ShowRecord(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    ShowGridDebitNote(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    'fillcollection(CType(ViewState("SalesInvFreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('UndoCancel')")


                End If
                DisableControl()

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                grdServiceInvoice.Columns("7").HeaderText = "Value(" & txtcurr.Text & ")"

                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")

                lblsubtotal_base.Text = "SubTotal(" & mbasecurrency & ")"

                btnAddLine_Click(sender, e)

            Else
                lblsubtotal_curr.Text = "SubTotal(" & txtcurr.Text & ")"
                grdServiceInvoice.Columns("8").HeaderText = "Value(" & mbasecurrency & ")"
                grdServiceInvoice.Columns("7").HeaderText = "Value(" & txtcurr.Text & ")"
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Public Function check_Privilege() As Integer
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='3' and appid='4' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            'mySqlCmd = New SqlCommand(strSql, mySqlConn)
            'mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSql)
            If ds1.Tables.Count > 0 Then
                If ds1.Tables(0).Rows.Count > 0 Then
                    Return 1
                Else
                    Return 0
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            'clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            'clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try
    End Function
#Region "Public Sub FillGrids()"
    Public Sub FillGrids()
        If ViewState("SalesInvFreeFormState") = "New" Then
            fillgrdserviceinvoice(grdServiceInvoice, True)
            'FillGridHotel()
            fillgrdserviceinvoiceDefault()
        ElseIf ViewState("SalesInvFreeFormState") <> "New" Then
            Dim lngCnt As Long
            lngCnt = objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "freeforminvoice_detail", "count(tran_id)", "tran_id", CType(ViewState("SalesInvFreeFormRefCode"), String), "div_code", txtdiv.Value)
            If lngCnt > 1 Then
                grdServiceInvoice.Columns("7").HeaderText = "Value(" & txtcurr.Text & ")"
                fillgrdserviceinvoice(grdServiceInvoice, False, lngCnt)
                fillgrdserviceinvoiceDefault()
            Else : grdServiceInvoice.Columns("7").HeaderText = "Value(" & txtcurr.Text & ")"

                fillgrdserviceinvoice(grdServiceInvoice, True)
                fillgrdserviceinvoiceDefault()
            End If
        End If

    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
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

#Region "Public Sub fillgrdserviceinvoice(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrdserviceinvoice(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If
        grd.Columns("8").HeaderText = "Value(" & mbasecurrency & ")"
        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region






#Region "Private Sub fillgrdserviceinvoiceDefault()"
    Private Sub fillgrdserviceinvoiceDefault()
        Try
            For Each gvRow In grdServiceInvoice.Rows

                'Dim count As Integer


                Dim txttaxableamt As TextBox
                Dim txtnontaxableamt As TextBox
                Dim txtvatperc As TextBox
                Dim txtvatamount As TextBox
                Dim txtvalue As TextBox
                Dim txtbasevalue As TextBox
                Dim ddlvatType As DropDownList
                Dim txtcostcenter As TextBox
                Dim txtcostcentercode As TextBox
                txtcostcenter = gvRow.FindControl("txtcostcenter")
                txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                txtbasevalue = gvRow.FindControl("txtbasevalue")
                txtvalue = gvRow.FindControl("txtcurrvalue")
                txtvatamount = gvRow.FindControl("txtvatamt")
                ddlvatType = gvRow.FindControl("ddlvattype")
                txttaxableamt = gvRow.FindControl("txttaxableamt")
                txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                txtvatperc = gvRow.FindControl("txtvatperc")
                txtvatamount.Enabled = False
                txtcostcentercode.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select  costcenter_code from  costcenter_master where costcenter_code='GEN'"), String)
                txtcostcenter.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select  costcenter_name from  costcenter_master where costcenter_code='GEN'"), String)
                ddlvatType.Attributes.Add("onchange", "javascript:Calculate_Gridvalues('" + CType(ddlvatType.ClientID, String) + "','" + CType(txtvatperc.ClientID, String) + "','" + CType(txtnontaxableamt.ClientID, String) + "','" + CType(txttaxableamt.ClientID, String) + "','" + CType(txtbasevalue.ClientID, String) + "','" + CType(txtvalue.ClientID, String) + "','" + CType(txtvatamount.ClientID, String) + "');grdTotal();")
                txtnontaxableamt.Attributes.Add("onchange", "javascript:Calculate_Gridvalues('" + CType(ddlvatType.ClientID, String) + "','" + CType(txtvatperc.ClientID, String) + "','" + CType(txtnontaxableamt.ClientID, String) + "','" + CType(txttaxableamt.ClientID, String) + "','" + CType(txtbasevalue.ClientID, String) + "','" + CType(txtvalue.ClientID, String) + "','" + CType(txtvatamount.ClientID, String) + "');grdTotal();")

                txttaxableamt.Attributes.Add("onchange", "javascript:Calculate_Gridvalues('" + CType(ddlvatType.ClientID, String) + "','" + CType(txtvatperc.ClientID, String) + "','" + CType(txtnontaxableamt.ClientID, String) + "','" + CType(txttaxableamt.ClientID, String) + "','" + CType(txtbasevalue.ClientID, String) + "','" + CType(txtvalue.ClientID, String) + "','" + CType(txtvatamount.ClientID, String) + "');grdTotal();")
                txtvatperc.Attributes.Add("onchange", "javascript:Calculate_Gridvalues('" + CType(ddlvatType.ClientID, String) + "','" + CType(txtvatperc.ClientID, String) + "','" + CType(txtnontaxableamt.ClientID, String) + "','" + CType(txttaxableamt.ClientID, String) + "','" + CType(txtbasevalue.ClientID, String) + "','" + CType(txtvalue.ClientID, String) + "','" + CType(txtvatamount.ClientID, String) + "');grdTotal();")
                txtvatperc.Text = txtvatpercentage.Text



                Numbers(txttaxableamt)
                Numbers(txtnontaxableamt)
                Numbers(txtvatperc)

            Next
            txtgridrows.Value = grdServiceInvoice.Rows.Count
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DebitNote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region


#Region "Private Sub AddLinesTosalesinvoice()"
    Private Sub AddLinesTosalesinvoice()
        Dim count As Integer
        Dim txtacctcode As TextBox
        Dim txtacctname As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtparticulars As TextBox
        Dim txtnontaxableamt As TextBox
        Dim txttaxableamt As TextBox
        Dim txtvatperc As TextBox
        Dim txtvatamt As TextBox
        Dim txtsalevalue As TextBox
        Dim txtbasevalue As TextBox
        Dim ddlvattype As DropDownList
        count = grdServiceInvoice.Rows.Count + 1

        Dim AccCode(count) As String
        Dim AccName(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim Partic(count) As String
        Dim NTamt(count) As String
        Dim Tamt(count) As String
        Dim vatperc(count) As String
        Dim vatamt(count) As String
        Dim salevalue(count) As String
        Dim basevalue(count) As String
        Dim vattype(count) As String

        Dim n As Integer = 0
        Dim i As Integer



        Try
            'If txtDate.Text <> "" Then
            '    txtdt.Text = CType(Format(CType(txtDate.Text, Date), "yyyy/MM/dd"), String)
            'End If
            For Each gvRow As GridViewRow In grdServiceInvoice.Rows





                txtacctcode = gvRow.FindControl("txtacctcode")
                txtacctname = gvRow.FindControl("txtacct")
                txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                txtcostcentername = gvRow.FindControl("txtcostcenter")
                txtparticulars = gvRow.FindControl("txtparticulars")
                txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                txttaxableamt = gvRow.FindControl("txttaxableamt")
                txtvatperc = gvRow.FindControl("txtvatperc")
                txtvatamt = gvRow.FindControl("txtvatamt")
                txtsalevalue = gvRow.FindControl("txtcurrvalue")
                txtbasevalue = gvRow.FindControl("txtbasevalue")
                ddlvattype = gvRow.FindControl("ddlvattype")





                AccCode(n) = CType(txtacctcode.Text, String)
                AccName(n) = CType(txtacctname.Text, String)
                CCCode(n) = CType(txtcostcentercode.Text, String)
                CCName(n) = CType(txtcostcentername.Text, String)
                Partic(n) = CType(txtparticulars.Text, String)
                NTamt(n) = CType(txtnontaxableamt.Text, String)
                Tamt(n) = CType(txttaxableamt.Text, String)
                vatperc(n) = CType(txtvatperc.Text, String)
                vatamt(n) = CType(txtvatamt.Text, String)
                salevalue(n) = CType(txtsalevalue.Text, String)
                basevalue(n) = CType(txtbasevalue.Text, String)
                vattype(n) = CType(ddlvattype.Text, String)




                n = n + 1
            Next
            fillgrdserviceinvoice(grdServiceInvoice, False, grdServiceInvoice.Rows.Count + 1)
            fillgrdserviceinvoiceDefault()
            i = n
            n = 0
            For Each gvRow As GridViewRow In grdServiceInvoice.Rows
                If n = i Then
                    Exit For
                End If
                txtacctcode = gvRow.FindControl("txtacctcode")
                txtacctname = gvRow.FindControl("txtacct")
                txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                txtcostcentername = gvRow.FindControl("txtcostcenter")
                txtparticulars = gvRow.FindControl("txtparticulars")
                txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                txttaxableamt = gvRow.FindControl("txttaxableamt")
                txtvatperc = gvRow.FindControl("txtvatperc")
                txtvatamt = gvRow.FindControl("txtvatamt")
                txtsalevalue = gvRow.FindControl("txtcurrvalue")
                txtbasevalue = gvRow.FindControl("txtbasevalue")
                ddlvattype = gvRow.FindControl("ddlvattype")


                txtacctcode.Text = AccCode(n)
                txtacctname.Text = AccName(n)
                txtcostcentercode.Text = CCCode(n)
                txtcostcentername.Text = CCName(n)
                txtparticulars.Text = Partic(n)
                txtnontaxableamt.Text = NTamt(n)
                txttaxableamt.Text = Tamt(n)
                txtvatperc.Text = vatperc(n)
                txtvatamt.Text = vatamt(n)
                txtsalevalue.Text = salevalue(n)
                txtbasevalue.Text = basevalue(n)
                ddlvattype.Text = vattype(n)


                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region
#Region "Private Sub DelLinessalesinvoice()"
    Private Sub DelLinessalesinvoice()
        Dim count As Integer
        Dim txtacctcode As TextBox
        Dim txtacctname As TextBox
        Dim txtcostcentercode As TextBox
        Dim txtcostcentername As TextBox
        Dim txtparticulars As TextBox
        Dim txtnontaxableamt As TextBox
        Dim txttaxableamt As TextBox
        Dim txtvatperc As TextBox
        Dim txtvatamt As TextBox
        Dim txtsalevalue As TextBox
        Dim txtbasevalue As TextBox
        Dim ddlvattype As DropDownList

        count = grdServiceInvoice.Rows.Count + 1

        Dim AccCode(count) As String
        Dim AccName(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim Partic(count) As String
        Dim NTamt(count) As String
        Dim Tamt(count) As String
        Dim vatperc(count) As String
        Dim vatamt(count) As String
        Dim salevalue(count) As String
        Dim basevalue(count) As String
        Dim vattype(count) As String
        Dim taxtot, Nontax, vatamttot, salvaltot As Decimal
        taxtot = 0
        Nontax = 0
        vatamttot = 0
        salvaltot = 0
        Dim n As Integer = 0
        Dim i As Integer
        Try
            'If txtDate.Text <> "" Then
            '    txtdt.Text = CType(Format(CType(txtDate.Text, Date), "yyyy/MM/dd"), String)
            'End If
            For Each gvRow In grdServiceInvoice.Rows
                chckDeletion = gvRow.FindControl("chckDeletion")
                If chckDeletion.Checked = False Then
                    txtacctcode = gvRow.FindControl("txtacctcode")
                    txtacctname = gvRow.FindControl("txtacct")
                    txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                    txtcostcentername = gvRow.FindControl("txtcostcenter")
                    txtparticulars = gvRow.FindControl("txtparticulars")
                    txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                    txttaxableamt = gvRow.FindControl("txttaxableamt")
                    txtvatperc = gvRow.FindControl("txtvatperc")
                    txtvatamt = gvRow.FindControl("txtvatamt")
                    txtsalevalue = gvRow.FindControl("txtcurrvalue")
                    txtbasevalue = gvRow.FindControl("txtbasevalue")
                    ddlvattype = gvRow.FindControl("ddlvattype")


                    AccCode(n) = CType(txtacctcode.Text, String)
                    AccName(n) = CType(txtacctname.Text, String)
                    CCCode(n) = CType(txtcostcentercode.Text, String)
                    CCName(n) = CType(txtcostcentername.Text, String)
                    Partic(n) = CType(txtparticulars.Text, String)
                    NTamt(n) = CType(txtnontaxableamt.Text, String)
                    Tamt(n) = CType(txttaxableamt.Text, String)
                    vatperc(n) = CType(txtvatperc.Text, String)
                    vatamt(n) = CType(txtvatamt.Text, String)
                    salevalue(n) = CType(txtsalevalue.Text, String)
                    basevalue(n) = CType(txtbasevalue.Text, String)
                    vattype(n) = CType(ddlvattype.Text, String)

                    n = n + 1
                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillgrdserviceinvoice(grdServiceInvoice, False, count)
            fillgrdserviceinvoiceDefault()
            i = n
            n = 0
            For Each gvRow In grdServiceInvoice.Rows
                If n = i Then
                    Exit For
                End If
                txtacctcode = gvRow.FindControl("txtacctcode")
                txtacctname = gvRow.FindControl("txtacct")
                txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                txtcostcentername = gvRow.FindControl("txtcostcenter")
                txtparticulars = gvRow.FindControl("txtparticulars")
                txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                txttaxableamt = gvRow.FindControl("txttaxableamt")
                txtvatperc = gvRow.FindControl("txtvatperc")
                txtvatamt = gvRow.FindControl("txtvatamt")
                txtsalevalue = gvRow.FindControl("txtcurrvalue")
                txtbasevalue = gvRow.FindControl("txtbasevalue")
                ddlvattype = gvRow.FindControl("ddlvattype")

                txtacctcode.Text = AccCode(n)
                txtacctname.Text = AccName(n)
                txtcostcentercode.Text = CCCode(n)
                txtcostcentername.Text = CCName(n)
                txtparticulars.Text = Partic(n)
                txtnontaxableamt.Text = NTamt(n)
                txttaxableamt.Text = Tamt(n)
                txtvatperc.Text = vatperc(n)
                txtvatamt.Text = vatamt(n)
                txtsalevalue.Text = salevalue(n)
                txtbasevalue.Text = basevalue(n)
                ddlvattype.Text = vattype(n)


                taxtot = taxtot + Val(txttaxableamt.Text)
                Nontax = Nontax + Val(txtnontaxableamt.Text)
                vatamttot = vatamttot + Val(txtvatamt.Text)
                salvaltot = salvaltot + Val(txtsalevalue.Text)



                n = n + 1
            Next
            txttaxableamttot.Text = Math.Round(taxtot, CType(txtdecimal.Value, Integer))
            txtnontaxableamttot.Text = Math.Round(Nontax, CType(txtdecimal.Value, Integer))
            txtvatamttot.Text = Math.Round(vatamttot, CType(txtdecimal.Value, Integer))
            txtsalevaltot.Text = Math.Round(salvaltot, CType(txtdecimal.Value, Integer))


            If txtconvrate.Text = "" Then
                txtconvrate.Text = 0
            End If
            txtbasetaxableamttot.Text = Math.Round(taxtot * txtconvrate.Text, CType(txtdecimal.Value, Integer))
            txtBasenontaxableamttot.Text = Math.Round(Nontax * txtconvrate.Text, CType(txtdecimal.Value, Integer))
            txtBasevatamttot.Text = Math.Round(vatamttot * txtconvrate.Text, CType(txtdecimal.Value, Integer))
            txtBasesalevaltot.Text = Math.Round(salvaltot * txtconvrate.Text, CType(txtdecimal.Value, Integer))



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region




#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region


#Region "NumbersDecimalRound"
    Public Sub NumbersDecimalRound(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersDecimalRoundHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region





    'Protected Sub TxtcustName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtcustName.TextChanged
    '    If ddltype.Text = "Customer" Then



    '        If objUtils.EntryExists(Session("dbconnectionName"), "agentmast", "agentname", " agentname='" & TxtcustName.Text & "' ") Then


    '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '            myCommand = New SqlCommand("Select currcode,controlacctcode,spersoncode,trnno,ctrycode,isnull(crdays,0) crdays  from agentmast Where agentname='" & TxtcustName.Text & "' ", SqlConn)
    '            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
    '            If mySqlReader.HasRows Then
    '                If mySqlReader.Read() = True Then




    '                    txtcurr.Text = CType(mySqlReader("currcode"), String)
    '                    txtcurrname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currname from currmast where currcode='" & txtcurr.Text & "'"), String)
    '                    txtcontrolac.Text = CType(mySqlReader("controlacctcode"), String)
    '                    txtcontrolacname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctname From acctmast where acctcode='" & txtcontrolac.Text & "'"), String)

    '                    txtsalescode.Text = CType(mySqlReader("spersoncode"), String)
    '                    txtsales.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select username From usermaster where usercode='" & txtsalescode.Text & "'"), String)
    '                    txtconvrate.Text = DecRound(CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select convrate from currrates where currcode='" + txtcurr.Text + "' and tocurr = (select option_selected from reservation_parameters where param_id=457)"), String))
    '                    txttrnno.Text = CType(mySqlReader("trnno"), String)

    '                    'txtsourcecode.Text = CType(mySqlReader("ctrycode"), String)
    '                    'txtsourcectry.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select ctryname from ctrymast where ctrycode='" & txtsourcecode.Text & "'"), String)


    '                    txtTDate.Text = CType(txtJDate.Text, Date).AddDays(CType(mySqlReader("crdays"), Double))

    '                    If CType(mySqlReader("crdays"), Double) = 0 Then
    '                        txtTDate.Text = Now

    '                    End If




    '                    lblsubtotal_curr.Text = "SubTotal(" & txtcurr.Text & ")"
    '                    grdServiceInvoice.Columns("7").HeaderText = "value(" & txtcurr.Text & " )"
    '                End If
    '            End If








    '        Else
    '            clear_typefield()


    '        End If

    '    Else



    '        If objUtils.EntryExists(Session("dbconnectionName"), "partymast", "partyname", " partyname='" & TxtcustName.Text & "' ") Then


    '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '            myCommand = New SqlCommand("Select currcode,controlacctcode,ctrycode,isnull(crdays,0) crdays from partymast Where partycode='" & TxtCustCode.Text & "'", SqlConn)
    '            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
    '            If mySqlReader.HasRows Then
    '                If mySqlReader.Read() = True Then


    '                    txtcurr.Text = CType(mySqlReader("currcode"), String)
    '                    txtcurrname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currname from currmast where currcode='" & txtcurr.Text & "'"), String)
    '                    txtcontrolac.Text = CType(mySqlReader("controlacctcode"), String)
    '                    txtcontrolacname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctname From acctmast where acctcode='" & txtcontrolac.Text & "'"), String)
    '                    txtsales.Text = ""
    '                    txtsalescode.Text = ""
    '                    txtconvrate.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select convrate from currrates where currcode='" + txtcurr.Text + "' and tocurr = (select option_selected from reservation_parameters where param_id=457)"), String)
    '                    txttrnno.Text = ""
    '                    'txtsourcecode.Text = CType(mySqlReader("ctrycode"), String)
    '                    'txtsourcectry.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select ctryname from ctrymast where ctrycode='" & txtsourcecode.Text & "'"), String)

    '                    If CType(mySqlReader("crdays"), Double) = 0 Then
    '                        txtTDate.Text = Now
    '                    Else
    '                        'txtTDate.Text = FormatDateTime((txtJDate.Text)) + CType(mySqlReader("crdays"), Double)

    '                        txtTDate.Text = DateAdd(DateInterval.Weekday, CType(mySqlReader("crdays"), Double), CDate(txtJDate.Text)).ToShortDateString()
    '                    End If
    '                    lblsubtotal_curr.Text = "SubTotal(" & txtcurr.Text & ")"
    '                    grdServiceInvoice.Columns("7").HeaderText = "value(" & txtcurr.Text & " )"
    '                End If
    '            End If



    '        Else
    '            clear_typefield()


    '        End If



    '    End If
    '    Session("cust_for_filter") = TxtCustCode.Text()
    'End Sub


    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
        '  Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer))
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer), MidpointRounding.AwayFromZero)
        'Rdamt = Math.Round(Val(Ramt) * Val(txtdecimal.Value)) / Val(txtdecimal.Value)
        Return Rdamt
    End Function
    Private Sub clear_typefield()
        TxtcustName.Text = ""
        TxtCustCode.Text = ""
        txtcurr.Text = ""
        txtcurrname.Text = ""
        txtcontrolac.Text = ""
        txtcontrolacname.Text = ""
        txtsales.Text = ""
        txtsalescode.Text = ""
        txtconvrate.Text = ""
        txttrnno.Text = ""
        txtsourcectry.Text = ""
        txtsourcecode.Text = ""
        lblsubtotal_curr.Text = "SubTotal()"
        grdServiceInvoice.Columns("7").HeaderText = "value( )"
        txtTDate.Text = Now
        txtAliasName.Text = ""


    End Sub

    Protected Sub ddltype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddltype.SelectedIndexChanged

        clear_typefield()



        If ddltype.Text = "Customer" Then
            TxtCustName_AutoCompleteExtender.ServiceMethod = "Getcustlist"
            lblcust.Text = "Customer"
            PnlType.GroupingText = "Customer"
            lblreferno.Text = "Customer Reference No"
            lblAliasName.Text = "Short Name"
        Else

            TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsupplist"
            lblcust.Text = "Supplier"
            PnlType.GroupingText = "Supplier"
            lblreferno.Text = "Supplier Invoice No"
            lblAliasName.Text = "Legal Name of Entity"
        End If
    End Sub



    Protected Sub btnAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLine.Click
        AddLinesTosalesinvoice()
    End Sub

    Protected Sub btnDelLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelLine.Click
        DelLinessalesinvoice()
    End Sub
    'Private Function validate_BillAgainst() As Boolean
    '    Try
    '        validate_BillAgainst = True
    '        Dim Alflg As Integer
    '        Dim ErrMsg, strdiv As String
    '        strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

    '        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
    '        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
    '        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)

    '        Dim param1 As SqlParameter
    '        Dim param2 As SqlParameter
    '        param1 = New SqlParameter
    '        param1.ParameterName = "@allowflg"
    '        param1.Direction = ParameterDirection.Output
    '        param1.DbType = DbType.Int16
    '        param1.Size = 9
    '        myCommand.Parameters.Add(param1)
    '        param2 = New SqlParameter
    '        param2.ParameterName = "@errmsg"
    '        param2.Direction = ParameterDirection.Output
    '        param2.DbType = DbType.String
    '        param2.Size = 200
    '        myCommand.Parameters.Add(param2)
    '        myDataAdapter = New SqlDataAdapter(myCommand)
    '        myCommand.ExecuteNonQuery()

    '        Alflg = param1.Value
    '        ErrMsg = param2.Value

    '        If Alflg = 1 And ErrMsg <> "" Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
    '            validate_BillAgainst = False
    '            Exit Function
    '        End If
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("salesinvoicefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    Finally
    '        clsDBConnect.dbCommandClose(myCommand)
    '        clsDBConnect.dbConnectionClose(SqlConn)
    '    End Try
    'End Function

    Private Function validate_BillAdjust() As Boolean
        Try
            validate_BillAdjust = True

            Dim strdiv As String
            '   strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
 


            'Tanvir 30092022

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_get_account_against_bill", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_code ", SqlDbType.VarChar, 10)).Value = CType(txtdiv.Value, String)
            myCommand.Parameters.Add(New SqlParameter("@tran_id ", SqlDbType.VarChar, 30)).Value = txtDocNo.Text
            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()
            '  Using myDataAdapter = New SqlDataAdapter(myCommand)
            Using dt As New DataTable
                myDataAdapter.Fill(dt)
                If dt.Rows.Count > 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Entry is adjusted in accounts hence this action cannot be performed');", True)
                    txtJDate.Focus()
                    validate_BillAdjust = False
                    Exit Function
                End If
            End Using
            'End Using

            'Tanvir 30092022


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function
    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtJDate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

          
        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function


#Region "ValidateGrids()"
    Public Function ValidateGrids() As Boolean
        Try
            Dim flgCheck As Boolean = False



            Dim i As Integer = 1
            Dim txtacctcode As TextBox
            Dim txtacct As TextBox
            Dim txtcostcentercode As TextBox
            Dim txtvatamt As TextBox
            Dim txtparticulars As TextBox
            Dim txtvatperc As TextBox
            Dim txtcurrvalue, txtbasevalue, txtnontaxableamt, txttaxableamt As TextBox
            Dim ddlvattype As DropDownList

            For Each gvRow In grdServiceInvoice.Rows
                txtacctcode = gvRow.FindControl("txtacctcode")
                txtacct = gvRow.FindControl("txtacct")
                txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                txtvatamt = gvRow.FindControl("txtvatamt")
                txtparticulars = gvRow.FindControl("txtParticulars")
                txtvatperc = gvRow.FindControl("txtvatperc")
                txtcurrvalue = gvRow.FindControl("txtcurrvalue")
                txtbasevalue = gvRow.FindControl("txtbasevalue")
                txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                txttaxableamt = gvRow.FindControl("txttaxableamt")
                ddlvattype = gvRow.FindControl("ddlvattype")
                If txtbasevalue.Text = "0" Then
                    txtbasevalue.Text = ""
                End If

                If txtacctcode.Text <> "" Or txtvatamt.Text <> "" Or txtcurrvalue.Text <> "" Or txtbasevalue.Text <> "" Or txtnontaxableamt.Text <> "" Or txttaxableamt.Text <> "" Then
                    If txtacctcode.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Account Code for sales invoice.');", True)
                        SetFocus(grdServiceInvoice)
                        ValidateGrids = False
                        Exit Function
                    End If

                    If txtacctcode.Text <> "" Then
                        If objUtils.EntryExists(Session("dbconnectionName"), "acctmast", "acctname", " acctname='" & txtacct.Text & "' ") = False Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' AccountName  not valid on line no  " & i & "');", True)
                            SetFocus(grdServiceInvoice)
                            ValidateGrids = False
                            Exit Function
                        End If
                    End If

                    If txtparticulars.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Particulars cannot be blank.');", True)
                        SetFocus(grdServiceInvoice)
                        ValidateGrids = False
                        Exit Function
                    End If
                    'If ddlCCCode.Value = "[Select]" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Cost Center Code for debit note.');", True)
                    '    SetFocus(grdDebitNote)
                    '    ValidateGrids = False
                    '    Exit Function
                    'End If

                    If ddlvattype.Text = "Taxable" And (txtvatperc.Text = "" Or txtvatperc.Text = "0") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('VAT% cannot be zero.');", True)
                        SetFocus(grdServiceInvoice)
                        ValidateGrids = False
                        Exit Function
                    End If

                    If ddlvattype.Text = "Taxable" And (txttaxableamt.Text = "" Or txttaxableamt.Text = "0") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter taxable Amt for sales invoice.');", True)
                        SetFocus(grdServiceInvoice)
                        ValidateGrids = False
                        Exit Function
                    End If
                    If txtvatperc.Text <> "" Then



                        If ddlvattype.Text <> "Taxable" And (CType(txtvatperc.Text, Double) > 0) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('VAT % should be zero,If vat type is either exempt or zero rated');", True)
                            SetFocus(grdServiceInvoice)
                            ValidateGrids = False
                            Exit Function
                        End If
                    End If
                    If txtcurrvalue.Text = "" Or txtcurrvalue.Text = "0" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('sale value should not be blank.');", True)
                        SetFocus(grdServiceInvoice)
                        ValidateGrids = False
                        Exit Function
                    End If

                    If txtcurrvalue.Text > 0 And (txtbasevalue.Text = "0" Or txtbasevalue.Text = "") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('  Base value should not be blank, check the exchange rate.');", True)
                        SetFocus(grdServiceInvoice)
                        ValidateGrids = False
                        Exit Function
                    End If
                    flgCheck = True
                End If
                i = i + 1
            Next
            If flgCheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sales Invoice grid should not be blank.');", True)
                SetFocus(grdServiceInvoice)
                ValidateGrids = False
                Exit Function
            End If


            'If CheckAdjustBill() = True Then
            '    'If ddlType.Value <> "[Select]" Then
            '    If validate_AdjustBill(1, TxtCustCode.Text, IIf(ddltype.Text = "Customer", "C", "S"), txtcontrolac.Text, CType(Val(txtBasesalevaltot.Text), Decimal)) = False Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid Bill Adjust amount.');", True)
            '        ValidateGrids = False
            '        Exit Function
            '        'End If
            '    End If
            'End If
            ValidateGrids = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region


#Region "Public Function validate_AdjustBill(ByVal intReceiptLinNo As String,  ByVal strGlCode As String) as Boolean"
    'Public Function validate_AdjustBill(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal Adjustamt As Decimal) As Boolean
    '    validate_AdjustBill = True
    '    Dim collectionDate As Collection
    '    Dim strLineKey As String
    '    Dim MainGrdCount As Integer = 1
    '    Dim base_debit, base_credit As Decimal
    '    'If Session("Collection").ToString <> "" Then
    '    'collectionDate = CType(Session("Collection"), Collection)
    '    collectionDate = GetCollectionFromSession()
    '    If collectionDate.Count <> 0 Then
    '        Dim intcount As Integer = collectionDate.Count / 21
    '        Dim intLinNo, MainRowidx As Integer
    '        MainRowidx = 1
    '        For MainRowidx = 1 To MainGrdCount
    '            If MainRowidx = intReceiptLinNo Then
    '                base_debit = 0
    '                base_credit = 0
    '                For intLinNo = 1 To intcount
    '                    strLineKey = intLinNo & ":" & intReceiptLinNo
    '                    If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
    '                        If collectionDate("OpenMode" & strLineKey).ToString = "B" Then
    '                            If collectionDate("AccCode" & strLineKey).ToString = strAccCode And collectionDate("AccType" & strLineKey).ToString = strAccType And collectionDate("AccGLCode" & strLineKey).ToString = strGlCode Then
    '                                base_debit = DecRound(DecRound(base_debit) + DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
    '                                base_credit = DecRound(DecRound(base_credit) + DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
    '                            Else
    '                                validate_AdjustBill = False
    '                                Exit Function

    '                            End If
    '                        Else
    '                            base_debit = DecRound(DecRound(base_debit) + DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
    '                            base_credit = DecRound(DecRound(base_credit) + DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
    '                        End If
    '                    End If
    '                Next
    '            End If
    '        Next
    '    End If
    '    ' End If
    '    If DecRound(Adjustamt) = DecRound(base_debit) Or DecRound(Adjustamt) = DecRound(base_credit) Then
    '    Else
    '        validate_AdjustBill = False
    '        Exit Function
    '    End If
    'End Function
#End Region

    Private Function GetCollectionFromSession() As Collection
        Dim adjcolnostr As String
        adjcolnostr = txtAdjcolno.Value
        Dim collectionDate1 As New Collection
        If Not Session("Collection" & ":" & adjcolnostr) Is Nothing Then
            If Session("Collection" & ":" & adjcolnostr).ToString <> "" Then
                collectionDate1 = CType(Session("Collection" & ":" & adjcolnostr), Collection)
            End If
        End If
        Return collectionDate1
    End Function
#Region "Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)"
    Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Dim collectionDate As Collection
        Dim spersoncode As String
        Dim strdiv As String
        Dim strLineKey As String
        Dim MainGrdCount As Integer = 1

        'If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            strdiv = ViewState("div_code")
            Dim intcount As Integer = collectionDate.Count / 21
            Dim intLinNo, MainRowidx As Integer
            MainRowidx = 1
            For MainRowidx = 1 To MainGrdCount
                If MainRowidx = intReceiptLinNo Then
                    For intLinNo = 1 To intcount
                        strLineKey = intLinNo & ":" & intReceiptLinNo
                        If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                            If collectionDate("AccCode" & strLineKey).ToString = strAccCode And collectionDate("AccType" & strLineKey).ToString = strAccType And collectionDate("AccGLCode" & strLineKey).ToString = strGlCode Then
                                myCommand = New SqlCommand("sp_add_open_detail_new", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = collectionDate("TranId" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = collectionDate("TranType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("TranDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = collectionDate("AccTranLineNo" & strLineKey)

                                myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = intReceiptLinNo 'collectionDate("AgainstTranLineNo" & strLineKey) 'intReceiptLinNo
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")
                                If collectionDate("DueDate" & strLineKey).ToString = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = DBNull.Value
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                End If

                                If strAccType = "C" Then
                                    If txtsales.Text <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = txtsales.Text
                                    Else
                                        'spersoncode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"agentmast", "spersoncode", "agentcode", strAccCode)
                                        myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = DBNull.Value
                                    End If
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = DBNull.Value
                                End If
                                myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("Debit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("Credit" & strLineKey), Decimal))

                                myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = collectionDate("RefNo" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = collectionDate("Field2" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = collectionDate("Field3" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = collectionDate("Field4" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = collectionDate("Field5" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = collectionDate("OpenMode" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = DBNull.Value
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(collectionDate("CurrRate" & strLineKey), Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = collectionDate("AccType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccCode" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccGLCode" & strLineKey).ToString

                                myCommand.ExecuteNonQuery()
                            End If
                        End If
                    Next
                End If
            Next
        End If
        ' End If
    End Sub
#End Region

    Private Function CheckAdjustBill() As Boolean
        CheckAdjustBill = False
        Dim intLineNo As Integer = 1
        Dim strLineKey As String

        strLineKey = "AgainstTranLineNo" & intLineNo & ":" & 1
        Dim collectionDate As Collection
        ' If Not Session("Collection") Is Nothing Then
        'If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            If colexists(collectionDate, strLineKey) = True Then
                CheckAdjustBill = True
                Exit Function
            End If
        End If
        'End If
        'End If
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim strdiv, strcostcentercode As String

            ' Supplier chain hotels - invoice will be posted in single hotel code - Christo - 05/02/19
            Dim postingto As String
            postingto = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select postaccount from partymast where partycode='" & TxtCustCode.Text & "'"), String)
            If Trim(postingto) = "0" Then
                postingto = ""
            End If

            If Page.IsValid = True Then
                'If ViewState("SalesInvFreeFormState") = "Edit" Or ViewState("SalesInvFreeFormState") = "Delete" Or ViewState("SalesInvFreeFormState") = "Cancel" Or ViewState("SalesInvFreeFormState") = "undoCancel" Then
                '    If validate_BillAgainst() = False Then
                '        Exit Sub
                '    End If
                'End If
                'Tanvir 20102023 
                If ViewState("SalesInvFreeFormState") = "Edit" Or ViewState("SalesInvFreeFormState") = "Delete" Or ViewState("SalesInvFreeFormState") = "Cancel" Or ViewState("SalesInvFreeFormState") = "undoCancel" Then
                    If validate_BillAdjust() = False Then
                        Exit Sub
                    End If
                End If
                'Tanvir 20102023

                If ViewState("SalesInvFreeFormState") = "New" Or ViewState("SalesInvFreeFormState") = "Edit" Or ViewState("SalesInvFreeFormState") = "Copy" Or ViewState("SalesInvFreeFormState") = "undoCancel" Then
                    If ValidateGrids() = False Then
                        Exit Sub
                    End If
                End If

                If ViewState("SalesInvFreeFormState") = "New" Or ViewState("SalesInvFreeFormState") = "Edit" Or ViewState("SalesInvFreeFormState") = "Delete" Or ViewState("SalesInvFreeFormState") = "Cancel" Or ViewState("SalesInvFreeFormState") = "undoCancel" Then
                    If Validateseal() = False Then
                        Exit Sub
                    End If
                End If

                strdiv = ViewState("div_code")
                strcostcentercode = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 510)
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If

                If ViewState("SalesInvFreeFormState") = "New" Or ViewState("SalesInvFreeFormState") = "Edit" Or ViewState("SalesInvFreeFormState") = "Copy" Then
                    If ViewState("SalesInvFreeFormState") = "New" Or ViewState("SalesInvFreeFormState") = "Copy" Then

                        Dim optionval As String

                        If (ViewState("SalesInvFreeFormState") = "New" And txtDocNo.Text <> "") Then 'Tanvir(10112022)
                            ' to avoid multiple time save in new mode - christo.A - 27/12/18-- Tanvir 10112022
                        Else 'Tanvir 10112022
                            txtDocNo.Text = ""
                            If (CType(ViewState("CNDNOpen_type"), String) = "PE") Then
                                optionval = objUtils.GetAutoDocNodiv(CType(ViewState("CNDNOpen_type"), String), SqlConn, sqlTrans, strdiv)
                            Else
                                ' Yearwise number Sequence - Christo.A - 20/12/18
                                Dim optionType As String = objUtils.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=2037")
                                If LCase(optionType) = "year" Then
                                    optionval = objUtils.GetAutoDocNodivYear(CType(ViewState("CNDNOpen_type"), String), SqlConn, sqlTrans, strdiv, Year(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")))
                                ElseIf LCase(optionType) = "month" Then
                                    Dim sqlqry As String = "select prefix from docgen_div where optionname='" + ViewState("CNDNOpen_type") + "' and div_id='" + strdiv + "' and docyear=2008"
                                    Dim prefix As String = objUtils.ExecuteQueryReturnStringValue(sqlqry)
                                    ' optionval = objUtils.GetAutoDocNodivMonth(CType(ViewState("CNDNOpen_type"), String), SqlConn, sqlTrans, strdiv, prefix, Month(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")).ToString("00"), CType(Year(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")), String))
                                    optionval = objUtils.GetAutoDocNodivMonth(CType(ViewState("CNDNOpen_type"), String), SqlConn, sqlTrans, strdiv, UCase(CType(ViewState("CNDNOpen_type"), String)), Month(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")).ToString("00"), CType(Year(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")), String))

                                    'insert into monthdocgen(divcode,tranType,docMonth,docYear,docNo) values(@div_id,@tran_type,@docmonth,@docyear,0)                  

                                    '    optionval = objUtils.GetAutoDocNodivYear(CType(ViewState("ReceiptsRVPVTranType"), String), SqlConn, sqlTrans, strdiv, Year(Format(CType(txtDate.Text, Date), "yyyy/MM/dd")))
                                    'ElseIf LCase(optionType) = "month" Then
                                    ' optionval = objUtils.GetAutoDocNodivMonth(CType(ViewState("ReceiptsRVPVTranType"), String), SqlConn, sqlTrans, strdiv, UCase(CType(ViewState("ReceiptsRVPVTranType"), String)), Month(Format(CType(txtDate.Text, Date), "yyyy/MM/dd")).ToString("00"), CType(Year(Format(CType(txtDate.Text, Date), "yyyy/MM/dd")), String))
                                Else
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('ID generate option type is wrong');", True)
                                    sqlTrans.Rollback()
                                    Exit Sub
                                End If
                            End If
                            txtDocNo.Text = optionval.Trim
                        End If
                        myCommand = New SqlCommand("sp_add_freeforminvoice_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("SalesInvFreeFormState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_freeforminvoice_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If




                    myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    'If txtDate.Text = "" Then
                    '    myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = DBNull.Value
                    'Else

                    myCommand.Parameters.Add(New SqlParameter("@invoicetype", SqlDbType.VarChar, 20)).Value = CType(ddlinvoicetype.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@invoice_date", SqlDbType.DateTime)).Value = CType(Format(CType(txtJDate.Text, Date), "yyyy/MM/dd"), String)
                    'End If
                    myCommand.Parameters.Add(New SqlParameter("@supref", SqlDbType.VarChar, 20)).Value = CType(txtrefno.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@othref", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    If txtTDate.Text = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@due_date", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@due_date", SqlDbType.DateTime)).Value = CType(Format(CType(txtTDate.Text, Date), "yyyy/MM/dd"), String)
                    End If
                    'If ddltype.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = IIf(ddltype.Text = "Customer", "C", "S")
                    'Else
                    '    myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = DBNull.Value
                    'End If
                    'If ddlCustomer.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@supcode", SqlDbType.VarChar, 20)).Value = CType(TxtCustCode.Text, String)
                    'Else
                    '    myCommand.Parameters.Add(New SqlParameter("@supcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    'End If
                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtcurr.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType((txtconvrate.Text), Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@total", SqlDbType.Money)).Value = CType(Val(txtsalevaltot.Text), Double)
                    myCommand.Parameters.Add(New SqlParameter("@basetotal", SqlDbType.Money)).Value = CType(Val(txtBasesalevaltot.Text), Double)
                    'myCommand.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = ""

                    myCommand.Parameters.Add(New SqlParameter("@bookingno ", SqlDbType.VarChar, 500)).Value = CType((txtbookingno.Text), String)

                    myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = CType((txtnarration.Text), String)


                    myCommand.Parameters.Add(New SqlParameter("@referenceno", SqlDbType.VarChar, 500)).Value = CType((txtrefno.Text), String)

                    myCommand.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 500)).Value = CType((txtsourcecode.Text), String)
                    myCommand.Parameters.Add(New SqlParameter("@trans_state", SqlDbType.VarChar, 1)).Value = "U"

                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    myCommand.Parameters.Add(New SqlParameter("@sperson", SqlDbType.VarChar, 10)).Value = CType(txtsalescode.Text, String)


                    If chkPost.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If


                    myCommand.ExecuteNonQuery()

                    If ViewState("SalesInvFreeFormState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "freeforminvoice_master"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                            myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                            myCommand.ExecuteNonQuery()
                        End If


                        myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                        myCommand.ExecuteNonQuery()

                        myCommand = New SqlCommand("sp_del_freeforminvoice_detail", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                        myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv

                        myCommand.ExecuteNonQuery()

                        ' Tanvir 02/10/2023 Point 6
                        Dim dataTable As DataTable = DirectCast(Session("freeform_Detail"), DataTable)
                        If Not Session("freeform_Detail") Is Nothing Then

                            Dim filterExpression As String = "  div_code = " & strdiv & " " ' and against_tran_lineno=1 ' against_tran_id =" & txtDocNo.Value & "  and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And 
                            Dim filteredRows() As DataRow = dataTable.Select(filterExpression)
                            Dim advpayment As Integer = 0
                            If ViewState("SalesInvFreeFormState") = "Edit" Then
                                If dataTable.Rows.Count > 0 Then


                                    For Each row As DataRow In filteredRows
                                        myCommand = New SqlCommand("sp_reverse_pending_invoices", SqlConn, sqlTrans)
                                        myCommand.CommandType = CommandType.StoredProcedure
                                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                        myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = 1
                                        myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                                        myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                                        '  If Trim(postingto) <> "" Then
                                        '    myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = Trim(postingto)
                                        'Else

                                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = row("supcode") ' Trim(TxtCustCode.Text)
                                        ' End If

                                        myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = row("acc_type")  'IIf(ddltype.Text = "Customer", "C", "S") ' "G"      '"G"

                                        If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                                            myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("total"), Decimal)) ' IIf(DecRound(CType(row("amount"), Decimal)) < DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(row("amount"), Decimal))) ' DecRound(CType(txtsalevaltot.Text, Decimal))
                                            myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                            myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("basetotal"), Decimal)) ' IIf(DecRound(CType(row("baseamount"), Decimal)) < DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(row("baseamount"), Decimal))) ' DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                            myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                                        Else
                                            If CType(ViewState("CNDNOpen_type"), String) = "DN" Then

                                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("total"), Decimal)) 'IIf(DecRound(CType(row("amount"), Decimal)) < DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(row("amount"), Decimal)))
                                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("basetotal"), Decimal)) ' IIf(DecRound(CType(row("baseamount"), Decimal)) < DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(row("baseamount"), Decimal)))
                                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0

                                            Else
                                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("total"), Decimal)) ' DecRound(CType(txtsalevaltot.Text, Decimal))
                                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("basetotal"), Decimal)) ' DecRound(CType(row("baseamount"), Decimal)) ' DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                            End If
                                        End If


                                        myCommand.ExecuteNonQuery()
                                    Next
                                End If
                            End If
                        End If

                    End If

                    'If CheckAdjustBill() = True Then
                    '    Save_Open_detail(1, TxtCustCode.Text, IIf(ddltype.Text = "Customer", "C", "S"), txtcontrolac.Text, SqlConn, sqlTrans)
                    'End If
                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        caccounts.clraccounts()
                        cacc.acc_tran_id = txtDocNo.Text
                        cacc.acc_tran_type = CType(ViewState("CNDNOpen_type"), String)
                        cacc.acc_tran_date = Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")
                        cacc.acc_div_id = strdiv

                        'Posting for the Header Level
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id

                        If Trim(postingto) <> "" Then
                            ctran.acc_code = Trim(postingto)
                        Else
                            ctran.acc_code = TxtCustCode.Text
                        End If


                        ctran.acc_type = IIf(ddltype.Text = "Customer", "C", "S") ' "G"
                        ctran.acc_currency_id = txtcurr.Text
                        ctran.acc_currency_rate = CType(Val(txtconvrate.Text), Decimal)
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = txtnarration.Text
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = 1 ' 0
                        ctran.acc_tran_type = cacc.acc_tran_type

                        ctran.pacc_gl_code = txtcontrolac.Text

                        ctran.acc_ref1 = Trim(txtbookingno.Text)
                        ctran.acc_ref2 = txtrefno.Text
                        ctran.acc_ref3 = ""
                        ctran.acc_ref4 = ""
                        cacc.addtran(ctran)
                        'If CheckAdjustBill() = False Then
                        '    'without adjustbill only posting
                        csubtran = New clsSubTran
                        csubtran.acc_against_tran_id = cacc.acc_tran_id
                        csubtran.acc_against_tran_lineno = 1
                        csubtran.acc_against_tran_type = cacc.acc_tran_type

                        If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                            csubtran.acc_debit = DecRound(CType(txtsalevaltot.Text, Decimal))
                            csubtran.acc_credit = 0
                            csubtran.acc_base_debit = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                            csubtran.acc_base_credit = 0
                        Else
                            If CType(ViewState("CNDNOpen_type"), String) = "DN" Then

                                csubtran.acc_credit = 0
                                csubtran.acc_debit = DecRound(CType(txtsalevaltot.Text, Decimal))
                                csubtran.acc_base_credit = 0
                                csubtran.acc_base_debit = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                            Else
                                csubtran.acc_credit = DecRound(CType(txtsalevaltot.Text, Decimal))
                                csubtran.acc_debit = 0
                                csubtran.acc_base_credit = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                csubtran.acc_base_debit = 0
                            End If

                        End If
                        csubtran.acc_tran_date = cacc.acc_tran_date
                        csubtran.acc_due_date = cacc.acc_tran_date
                        csubtran.acc_field1 = Trim(txtbookingno.Text)
                        csubtran.acc_field2 = txtrefno.Text
                        csubtran.acc_field3 = ""
                        csubtran.acc_field4 = ""
                        csubtran.acc_field5 = ""
                        csubtran.acc_tran_id = cacc.acc_tran_id
                        csubtran.acc_tran_lineno = 1
                        csubtran.acc_tran_type = cacc.acc_tran_type
                        csubtran.acc_narration = txtnarration.Text
                        csubtran.acc_type = IIf(ddltype.Text = "Customer", "C", "S") ' "G"      '"G"
                        csubtran.currate = CType(txtconvrate.Text, Decimal)
                        If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                            csubtran.acc_base_debit = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                            csubtran.acc_base_credit = 0
                        Else
                            If CType(ViewState("CNDNOpen_type"), String) = "DN" Then
                                csubtran.acc_base_credit = 0
                                csubtran.acc_base_debit = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                            Else
                                csubtran.acc_base_credit = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                csubtran.acc_base_debit = 0
                            End If

                        End If
                        csubtran.costcentercode = ""
                        cacc.addsubtran(csubtran)
                        'End If
                    End If
                    '----------------------------------- Inserting Data To DebitNote Details Table
                    Dim i As Integer = 1
                    Dim txtacctcode As TextBox
                    Dim txtcostcentercode As TextBox
                    Dim txtvatamt As TextBox
                    Dim txtparticulars As TextBox
                    Dim txtvatperc As TextBox
                    Dim txtcurrvalue, txtbasevalue, txtnontaxableamt, txttaxableamt As TextBox
                    Dim ddlvattype As DropDownList
                    Dim vatbaseamount As Double = 0.0

                    For Each gvRow In grdServiceInvoice.Rows


                        txtacctcode = gvRow.FindControl("txtacctcode")
                        txtcostcentercode = gvRow.FindControl("txtcostcentercode")
                        txtvatamt = gvRow.FindControl("txtvatamt")
                        txtparticulars = gvRow.FindControl("txtParticulars")
                        txtvatperc = gvRow.FindControl("txtvatperc")
                        txtcurrvalue = gvRow.FindControl("txtcurrvalue")
                        txtbasevalue = gvRow.FindControl("txtbasevalue")
                        txtnontaxableamt = gvRow.FindControl("txtnontaxableamt")
                        txttaxableamt = gvRow.FindControl("txttaxableamt")
                        ddlvattype = gvRow.FindControl("ddlvattype")

                        ' vatbaseamount = DecRound(CType(Val(txtvatamt.Text), Decimal) * Val(txtconvrate.Text))
                        vatbaseamount = CType(Val(txtvatamt.Text), Decimal) * Val(txtconvrate.Text)


                        If txtacctcode.Text <> "" Then
                            If objUtils.EntryExists(Session("dbconnectionName"), "acctmast", "acctcode", " acctcode='" & txtacctcode.Text & "' ") Then


                                i = i + 1
                                myCommand = New SqlCommand("sp_add_freeforminvoice_detail", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                                myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                                myCommand.Parameters.Add(New SqlParameter("@sno", SqlDbType.Int, 9)).Value = i

                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(txtacctcode.Text.Trim, String)
                                If txtcostcentercode.Text <> "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = CType(txtcostcentercode.Text.Trim, String)
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = DBNull.Value 'strcostcentercode
                                End If
                                myCommand.Parameters.Add(New SqlParameter("@particulars", SqlDbType.VarChar, 1000)).Value = CType(txtparticulars.Text, String)
                                myCommand.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money)).Value = CType(Val(txtcurrvalue.Text), Double)
                                myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = CType(Val(txtbasevalue.Text), Double)
                                myCommand.Parameters.Add(New SqlParameter("@vattype", SqlDbType.VarChar, 100)).Value = CType((ddlvattype.Text), String)
                                myCommand.Parameters.Add(New SqlParameter("@nontaxamt", SqlDbType.Money)).Value = CType(Val(txtnontaxableamt.Text), Double)
                                myCommand.Parameters.Add(New SqlParameter("@taxamt", SqlDbType.Money)).Value = CType(Val(txttaxableamt.Text), Double)
                                myCommand.Parameters.Add(New SqlParameter("@vatperc", SqlDbType.Money)).Value = CType(Val(txtvatperc.Text), Double)
                                myCommand.Parameters.Add(New SqlParameter("@vatamt", SqlDbType.Money)).Value = CType(Val(txtvatamt.Text), Double)
                                myCommand.ExecuteNonQuery()

                                If chkPost.Checked = True Then
                                    'Detail Level Against Posting
                                    ctran = New clstran
                                    ctran.acc_tran_id = cacc.acc_tran_id
                                    ctran.acc_code = CType(txtacctcode.Text, String)
                                    ctran.acc_type = "G"
                                    ctran.acc_currency_id = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                                    ctran.acc_currency_rate = 1
                                    ctran.acc_div_id = strdiv
                                    ctran.acc_narration = txtnarration.Text
                                    ctran.acc_tran_date = cacc.acc_tran_date
                                    ctran.acc_tran_lineno = i
                                    ctran.acc_tran_type = cacc.acc_tran_type
                                    ctran.pacc_gl_code = ""
                                    ctran.acc_ref1 = ""
                                    ctran.acc_ref2 = ""
                                    ctran.acc_ref3 = ""
                                    ctran.acc_ref4 = ""
                                    cacc.addtran(ctran)

                                    csubtran = New clsSubTran
                                    csubtran.acc_against_tran_id = cacc.acc_tran_id
                                    csubtran.acc_against_tran_lineno = i
                                    csubtran.acc_against_tran_type = cacc.acc_tran_type
                                    If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                                        csubtran.acc_credit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) > 0, (CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal)), 0))
                                        csubtran.acc_debit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) < 0, Math.Abs((CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal))), 0))
                                        csubtran.acc_base_credit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) > 0, (CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal)), 0))
                                        csubtran.acc_base_debit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) < 0, Math.Abs((CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal))), 0))
                                    Else

                                        If CType(ViewState("CNDNOpen_type"), String) = "DN" Then

                                            csubtran.acc_credit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) > 0, (CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal)), 0))
                                            csubtran.acc_debit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) < 0, Math.Abs((CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal))), 0))
                                            csubtran.acc_base_credit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) > 0, (CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal)), 0))
                                            csubtran.acc_base_debit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) < 0, Math.Abs((CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal))), 0))

                                        Else
                                            csubtran.acc_credit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) < 0, Math.Abs((CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal))), 0))
                                            csubtran.acc_debit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) > 0, (CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal)), 0))
                                            csubtran.acc_base_credit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) < 0, Math.Abs((CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal))), 0))
                                            csubtran.acc_base_debit = DecRound(IIf(CType(txtbasevalue.Text, Decimal) > 0, (CType(txtbasevalue.Text, Decimal) - CType(vatbaseamount, Decimal)), 0))
                                        End If


                                    End If
                                    csubtran.acc_tran_date = cacc.acc_tran_date
                                    csubtran.acc_due_date = cacc.acc_tran_date
                                    csubtran.acc_field1 = ""
                                    csubtran.acc_field2 = ""
                                    csubtran.acc_field3 = ""
                                    csubtran.acc_field4 = ""
                                    csubtran.acc_field5 = ""
                                    csubtran.acc_tran_id = cacc.acc_tran_id
                                    csubtran.acc_tran_lineno = i
                                    csubtran.acc_tran_type = cacc.acc_tran_type
                                    csubtran.acc_narration = txtparticulars.Text
                                    csubtran.acc_type = "G"
                                    csubtran.currate = 1
                                    'if it is blank then post to default 510
                                    If txtcostcentercode.Text <> "" Then
                                        csubtran.costcentercode = txtcostcentercode.Text
                                    Else
                                        csubtran.costcentercode = strcostcentercode
                                    End If
                                    cacc.addsubtran(csubtran)
                                End If

                            End If
                        End If
                    Next

                    ''' VAT Posting
                    If chkPost.Checked = True And Val(txtvatamttot.Text) <> 0 Then

                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        If (ddltype.Text = "Customer") Then
                            ctran.acc_code = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", IIf(strdiv = "01", 4501, 4503))
                        Else
                            ctran.acc_code = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", IIf(strdiv = "01", 4502, 4504))
                        End If
                        ctran.acc_type = "G"
                        ctran.acc_currency_id = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                        ctran.acc_currency_rate = 1
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = "VAT Posting  - " + Left(txtnarration.Text, 200)
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = 1021
                        ctran.acc_tran_type = cacc.acc_tran_type
                        ctran.pacc_gl_code = ""
                        ctran.acc_ref1 = ""
                        ctran.acc_ref2 = ""
                        ctran.acc_ref3 = ""
                        ctran.acc_ref4 = ""
                        cacc.addtran(ctran)


                        csubtran = New clsSubTran
                        csubtran.acc_against_tran_id = cacc.acc_tran_id
                        csubtran.acc_against_tran_lineno = 1021
                        csubtran.acc_against_tran_type = cacc.acc_tran_type
                        If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                            csubtran.acc_credit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) > 0, CType(txtBasevatamttot.Text, Decimal), 0))
                            csubtran.acc_debit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) < 0, Math.Abs(CType(txtBasevatamttot.Text, Decimal)), 0))
                            csubtran.acc_base_credit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) > 0, CType(txtBasevatamttot.Text, Decimal), 0))
                            csubtran.acc_base_debit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) < 0, Math.Abs(CType(txtBasevatamttot.Text, Decimal)), 0))
                        Else
                            If CType(ViewState("CNDNOpen_type"), String) = "DN" Then
                                csubtran.acc_credit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) > 0, CType(txtBasevatamttot.Text, Decimal), 0))
                                csubtran.acc_debit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) < 0, Math.Abs(CType(txtBasevatamttot.Text, Decimal)), 0))
                                csubtran.acc_base_credit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) > 0, CType(txtBasevatamttot.Text, Decimal), 0))
                                csubtran.acc_base_debit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) < 0, Math.Abs(CType(txtBasevatamttot.Text, Decimal)), 0))
                            Else
                                csubtran.acc_credit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) < 0, Math.Abs(CType(txtBasevatamttot.Text, Decimal)), 0))
                                csubtran.acc_debit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) > 0, CType(txtBasevatamttot.Text, Decimal), 0))
                                csubtran.acc_base_credit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) < 0, Math.Abs(CType(txtBasevatamttot.Text, Decimal)), 0))
                                csubtran.acc_base_debit = DecRound(IIf(CType(txtBasevatamttot.Text, Decimal) > 0, CType(txtBasevatamttot.Text, Decimal), 0))
                            End If

                        End If
                        csubtran.acc_tran_date = cacc.acc_tran_date
                        csubtran.acc_due_date = cacc.acc_tran_date
                        csubtran.acc_field1 = ""
                        csubtran.acc_field2 = ""
                        csubtran.acc_field3 = ""
                        csubtran.acc_field4 = ""
                        csubtran.acc_field5 = ""
                        csubtran.acc_tran_id = cacc.acc_tran_id
                        csubtran.acc_tran_lineno = 1021
                        csubtran.acc_tran_type = cacc.acc_tran_type
                        csubtran.acc_narration = "VAT Posting  - " + Left(txtnarration.Text, 200)
                        csubtran.acc_type = "G"
                        csubtran.currate = 1
                        ''if it is blank then post to default 510
                        'If txtcostcentercode.Text <> "" Then
                        ' csubtran.costcentercode = txtcostcentercode.Text
                        'Else
                        csubtran.costcentercode = strcostcentercode
                        'End If
                        cacc.addsubtran(csubtran)


                    End If

                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        cacc.table_name = ""
                        caccounts.Addaccounts(cacc)
                        If caccounts.saveaccounts(Session("dbconnectionName"), SqlConn, sqlTrans, Me.Page) <> 0 Then
                            Err.Raise(vbObjectError + 100)
                        End If
                        If ViewState("SalesInvFreeFormState") <> "Delete" Then
                            myCommand = New SqlCommand("sp_update_Pending_Invoices", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = 1
                            myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                            myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                            myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")
                            If Trim(postingto) <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = Trim(postingto)
                            Else

                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = Trim(TxtCustCode.Text)
                            End If

                            myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = IIf(ddltype.Text = "Customer", "C", "S") ' "G"      '"G"
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = 1
                            myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = txtcontrolac.Text
                            myCommand.Parameters.Add(New SqlParameter("@duedate ", SqlDbType.DateTime)).Value = Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")
                            myCommand.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = CType(txtconvrate.Text, Decimal)
                            myCommand.Parameters.Add(New SqlParameter("@field1", SqlDbType.VarChar, 500)).Value = Trim(txtbookingno.Text)
                            myCommand.Parameters.Add(New SqlParameter("@field2", SqlDbType.VarChar, 500)).Value = txtrefno.Text
                            myCommand.Parameters.Add(New SqlParameter("@field3 ", SqlDbType.VarChar, 500)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@field4", SqlDbType.VarChar, 500)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@field5", SqlDbType.VarChar, 500)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = "B"
                            myCommand.Parameters.Add(New SqlParameter("@open_field3 ", SqlDbType.VarChar, 500)).Value = txtnarration.Text
                            myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = txtcurr.Text
                            myCommand.Parameters.Add(New SqlParameter("@acc_State", SqlDbType.VarChar, 10)).Value = "P"

                            If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(txtsalevaltot.Text, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                            Else
                                If CType(ViewState("CNDNOpen_type"), String) = "DN" Then

                                    myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(txtsalevaltot.Text, Decimal))
                                    myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                    myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                    myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0

                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                    myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(txtsalevaltot.Text, Decimal))
                                    myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                    myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                End If
                            End If

                            '  myCommand.Parameters.Add(New SqlParameter("@basecurrency", SqlDbType.Decimal, 18, 12)).Value = 0
                            myCommand.ExecuteNonQuery()

                        End If


                        'For Accounts Posting
                        lblPostmsg.Text = "Posted"
                        lblPostmsg.ForeColor = Drawing.Color.Red
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If
                ElseIf ViewState("SalesInvFreeFormState") = "Delete" Then

                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "freeforminvoice_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                    'Tanvir 02/10/2023 point6
                    Dim dataTable As DataTable = DirectCast(Session("freeform_Detail"), DataTable)
                    If Not Session("freeform_Detail") Is Nothing Then

                        Dim filterExpression As String = "div_code = " & strdiv & " " 'against_tran_id =" & txtDocNo.Text & "  and against_tran_lineno= 1 and   against_tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "' And
                        Dim filteredRows() As DataRow = dataTable.Select(filterExpression)


                        '   If ViewState("SalesInvFreeFormState") = "Edit" Or ViewState("SalesInvFreeFormState") = "Delete" Then
                        For Each row As DataRow In filteredRows
                            'myCommand = New SqlCommand("sp_reverse_pending_invoices", SqlConn, sqlTrans)
                            'myCommand.CommandType = CommandType.StoredProcedure
                            'myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            'myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = 1
                            'myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                            'myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                            'If Trim(postingto) <> "" Then
                            '    myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = Trim(postingto)
                            'Else

                            '    myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = Trim(TxtCustCode.Text)
                            'End If

                            'myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = IIf(ddltype.Text = "Customer", "C", "S") ' "G"      '"G"

                            'If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                            '    myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("amount"), Decimal)) ' IIf(DecRound(CType(row("amount"), Decimal)) < DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(row("amount"), Decimal))) ' DecRound(CType(txtsalevaltot.Text, Decimal))
                            '    myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                            '    myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("baseamount"), Decimal)) ' IIf(DecRound(CType(row("baseamount"), Decimal)) < DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(row("baseamount"), Decimal))) ' DecRound(CType(txtBasesalevaltot.Text, Decimal))
                            '    myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                            'Else
                            '    If CType(ViewState("CNDNOpen_type"), String) = "DN" Then

                            '        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("amount"), Decimal)) 'IIf(DecRound(CType(row("amount"), Decimal)) < DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(row("amount"), Decimal)))
                            '        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                            '        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("baseamount"), Decimal)) ' IIf(DecRound(CType(row("baseamount"), Decimal)) < DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(row("baseamount"), Decimal)))
                            '        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0

                            '    Else
                            '        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                            '        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("amount"), Decimal)) ' DecRound(CType(txtsalevaltot.Text, Decimal))
                            '        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                            '        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("baseamount"), Decimal)) ' DecRound(CType(row("baseamount"), Decimal)) ' DecRound(CType(txtBasesalevaltot.Text, Decimal))
                            '    End If
                            'End If
                            myCommand = New SqlCommand("sp_reverse_pending_invoices", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = 1
                            myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                            myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                            '  If Trim(postingto) <> "" Then
                            '    myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = Trim(postingto)
                            'Else

                            myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = row("supcode") ' Trim(TxtCustCode.Text)
                            ' End If

                            myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = row("acc_type")  'IIf(ddltype.Text = "Customer", "C", "S") ' "G"      '"G"

                            If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("total"), Decimal)) ' IIf(DecRound(CType(row("amount"), Decimal)) < DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(row("amount"), Decimal))) ' DecRound(CType(txtsalevaltot.Text, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("basetotal"), Decimal)) ' IIf(DecRound(CType(row("baseamount"), Decimal)) < DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(row("baseamount"), Decimal))) ' DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                            Else
                                If CType(ViewState("CNDNOpen_type"), String) = "DN" Then

                                    myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("total"), Decimal)) 'IIf(DecRound(CType(row("amount"), Decimal)) < DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(txtsalevaltot.Text, Decimal)), DecRound(CType(row("amount"), Decimal)))
                                    myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                    myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("basetotal"), Decimal)) ' IIf(DecRound(CType(row("baseamount"), Decimal)) < DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(txtBasesalevaltot.Text, Decimal)), DecRound(CType(row("baseamount"), Decimal)))
                                    myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0

                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                    myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("total"), Decimal)) ' DecRound(CType(txtsalevaltot.Text, Decimal))
                                    myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                    myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("basetotal"), Decimal)) ' DecRound(CType(row("baseamount"), Decimal)) ' DecRound(CType(txtBasesalevaltot.Text, Decimal))
                                End If
                            End If
                            myCommand.ExecuteNonQuery()
                        Next
                        'End If
                    End If

                    myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.ExecuteNonQuery()


                    myCommand = New SqlCommand("sp_del_freeforminvoice", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                ElseIf ViewState("SalesInvFreeFormState") = "Cancel" Then

                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "freeforminvoice_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_cancel_trdpurchase_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.ExecuteNonQuery()


                    myCommand = New SqlCommand("sp_cancel_freeforminvoice__master", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                ElseIf ViewState("SalesInvFreeFormState") = "undoCancel" Then
                    myCommand = New SqlCommand("sp_undocancel_freeforminvoice_master", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("CNDNOpen_type"), String)
                    myCommand.ExecuteNonQuery()


                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close

                If ViewState("SalesInvFreeFormState") = "Delete" Or ViewState("SalesInvFreeFormState") = "Cancel" Or ViewState("SalesInvFreeFormState") = "undoCancel" Then
                    'Response.Redirect("DebitNoteSearch.aspx?tran_type=" & CType(Session("CNDNOpen_type"), String) & "", False)
                    'Response.Redirect("~\AccountsModule\DebitNoteSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String) & "", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('DebitNoteWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                Else
                    If ViewState("SalesInvFreeFormState") = "New" Or ViewState("SalesInvFreeFormState") = "Copy" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully');", True)
                        'Dim strscript As String = ""
                        'strscript = "window.opener.__doPostBack('DebitNoteWindowPostBack', '');window.opener.focus();window.close();"
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    ElseIf ViewState("SalesInvFreeFormState") = "Edit" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record update successfully');", True)
                        Dim strURL As String = ""

                        strURL = "Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Text.Trim & "&ttype=" & ViewState("CNDNOpen_type").ToString & "&tdate=" & txtJDate.Text.Trim
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", String.Format(ScriptOpenModalDialog, strURL, 300), True)
                    End If
                    'btnPrint.Visible = True
                    btnPdfReport.Visible = True
                    ViewState("SalesInvFreeFormState") = "View"
                    DisableControl()
                    'btnPrint_Click(sender, e)
                    'btnAdjustBill.Visible = False
                End If

                Session.Remove("Collection" & ":" & txtAdjcolno.Value)
            End If


        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoicefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Public Sub fillcollection(ByVal tranid As String, ByVal lineno As Integer)"
    'Public Sub fillcollection(ByVal tranid As String)

    '    Dim clAdBill As New Collection
    '    Dim strLineKey As String
    '    Dim intLineNo As Long = 1
    '    Dim MainRowct As Long
    '    Dim MainRowindex As Long
    '    Dim myDS As New DataSet
    '    Dim mySqlReader As SqlDataReader
    '    ' Dim rowbasetotal As Decimal
    '    MainRowct = 1
    '    'objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select count(*) from receipt_detail Where   receipt_acc_type<>'G' and tran_id='" & tranid & "' and tran_type='" & CType(Session("RVPVTranType"), String) & "'")
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myCommand = New SqlCommand("select * from  open_detail Where against_tran_id='" & tranid & "' and against_tran_type='" & CType(ViewState("CNDNOpen_type"), String) & "' order by against_tran_lineno,tran_lineno ", SqlConn)
    '    mySqlReader = myCommand.ExecuteReader()
    '    If mySqlReader.HasRows Then
    '        While mySqlReader.Read()
    '            If MainRowindex <> mySqlReader("against_tran_lineno") Then
    '                intLineNo = 1
    '            End If
    '            strLineKey = intLineNo & ":" & mySqlReader("against_tran_lineno")
    '            AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, mySqlReader("against_tran_lineno")) 'intLineNo.ToString)
    '            AddCollection(clAdBill, "AccTranLineNo" & strLineKey, mySqlReader("tran_lineno"))
    '            AddCollection(clAdBill, "TranId" & strLineKey, mySqlReader("tran_id"))
    '            AddCollection(clAdBill, "TranDate" & strLineKey, Format(CType(mySqlReader("tran_date"), Date), "dd/MM/yyyy"))
    '            AddCollection(clAdBill, "TranType" & strLineKey, mySqlReader("tran_type"))
    '            AddCollection(clAdBill, "DueDate" & strLineKey, Format(CType(mySqlReader("open_due_date"), Date), "dd/MM/yyyy"))
    '            AddCollection(clAdBill, "CurrRate" & strLineKey, mySqlReader("currency_rate"))
    '            AddCollection(clAdBill, "Credit" & strLineKey, DecRound(mySqlReader("open_credit")))
    '            AddCollection(clAdBill, "Debit" & strLineKey, DecRound(mySqlReader("open_debit")))
    '            AddCollection(clAdBill, "BaseCredit" & strLineKey, DecRound(mySqlReader("Base_Credit")))
    '            AddCollection(clAdBill, "BaseDebit" & strLineKey, DecRound(mySqlReader("Base_Debit")))
    '            AddCollection(clAdBill, "RefNo" & strLineKey, mySqlReader("open_field1"))
    '            AddCollection(clAdBill, "Field2" & strLineKey, mySqlReader("open_field2"))
    '            AddCollection(clAdBill, "Field3" & strLineKey, mySqlReader("open_field3"))
    '            AddCollection(clAdBill, "Field4" & strLineKey, mySqlReader("open_field4"))
    '            AddCollection(clAdBill, "Field5" & strLineKey, mySqlReader("open_field5"))
    '            AddCollection(clAdBill, "OpenMode" & strLineKey, mySqlReader("open_mode"))
    '            AddCollection(clAdBill, "AccType" & strLineKey, mySqlReader("Acc_Type"))
    '            AddCollection(clAdBill, "AccCode" & strLineKey, mySqlReader("Acc_Code"))
    '            AddCollection(clAdBill, "AccGLCode" & strLineKey, mySqlReader("Acc_GL_Code"))
    '            'rowbasetotal = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select case isnull(tran_type,'') when 'RV' then basecredit  else basedebit end basetotal   from receipt_detail Where   receipt_acc_type<>'G' and tran_id='" & tranid & "' and tran_type='" & CType(Session("RVPVTranType"), String) & "' and tran_lineno='" & mySqlReader("against_tran_lineno") & "'")
    '            AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(txtBasesalevaltot.Text))

    '            MainRowindex = mySqlReader("against_tran_lineno")
    '            intLineNo = intLineNo + 1
    '        End While
    '    End If
    '    myCommand.Dispose()
    '    SqlConn.Close()

    '    'Session.Add("Collection", clAdBill)
    '    Session.Add("Collection" & ":" & txtAdjcolno.Value, clAdBill)

    'End Sub
#End Region
#Region "Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean"
    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
        Try
            Dim k As Integer
            colexists = False
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
                    ' If newcol(newkey).ToString <> "" Then
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
#Region "Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)"
    Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)
        If colexists(dataCollection, strKey) = False Then
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        Else
            dataCollection.Remove(strKey)
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        End If
    End Sub
#End Region
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("SalesInvFreeFormState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim backPname As String
        Try

            backPname = "~\AccountsModule\SaleInvoicefreeformSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String)

            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDocNew.aspx?Pageame=salesinvoicefreeform&BackPageName=~\AccountsModule\salesinvoivefreeform.aspx&TranId=" & txtDocNo.Text & "&TranType=" & CType(ViewState("CNDNOpen_type"), String) & "&divid=" & ViewState("div_code") & "&Curr=" & CType(txtcurr.Text, String) & "','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoivefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Dim backPname As String
        Try

            backPname = "~\AccountsModule\SaleInvoicefreeformSearch.aspx?tran_type=" & CType(ViewState("CNDNOpen_type"), String)

            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('../AccountsModule/TransactionReports.aspx?Pageame=salesinvoicefreeform&BackPageName=~\AccountsModule\salesinvoivefreeform.aspx&printId=salesinvoivefreeform&TranId=" & txtDocNo.Text & "&TranType=" & CType(ViewState("CNDNOpen_type"), String) & "&divid=" & ViewState("div_code") & "&Curr=" & CType(txtcurr.Text, String) & "&TRNNo=" & CType(txttrnno.Text, String) & "','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("salesinvoivefreeform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub grdServiceInvoice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdServiceInvoice.RowDataBound
        Try


            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtacctcode As TextBox = CType(e.Row.FindControl("txtacct"), TextBox)
                Dim txtparticulars As TextBox = CType(e.Row.FindControl("txtParticulars"), TextBox)
                txtacctcode.Attributes.Add("onfocus", "javascript:displaynarration_default(" + txtparticulars.ClientID + ");")
                txtacctcode.Attributes.Add("onchange", "javascript:displaynarration_default(" + txtparticulars.ClientID + ");")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session.Remove("Collection" & ":" & txtAdjcolno.Value)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('salesinvoicefreeformWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
End Class
