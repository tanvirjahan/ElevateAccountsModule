Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Partial Class ExcessProvisionReversal
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Web Services Methods"
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetSuppliers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim bstrSqlQry As String = ""
        Dim myDS As New DataSet
        Dim supName As New List(Of String)
        Dim splitContext() As String = contextKey.Split("|")
        Dim divCode As String = ""
        Dim acctType As String = ""
        If splitContext.Count = 2 Then
            divCode = splitContext(0)
            If splitContext(1) = "Supplier" Then
                acctType = "S"
            Else
                acctType = "A"
            End If
        End If
        Try
            bstrSqlQry = "select code,des from view_account(nolock) where div_code='" & divCode & "' and [type]='" & acctType & "' and des like '" & Trim(prefixText) & "%' order by des"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            myDataAdapter = New SqlDataAdapter(bstrSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    supName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("des").ToString(), myDS.Tables(0).Rows(i)("code").ToString()))
                Next
            End If
            Return supName
        Catch ex As Exception
            Return supName
        End Try
    End Function
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                Dim checkDateFlag As String = Convert.ToString(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='5511'"))
                If checkDateFlag = "Y" Then
                    hdnChkDtFlag.Value = "Y"
                Else
                    hdnChkDtFlag.Value = "N"
                End If
                If hdnChkDtFlag.Value = "Y" Then
                    lblchkFromDt.InnerText = "From Check In Date"
                    lblChkToDt.InnerText = "To Check In Date"
                Else
                    lblchkFromDt.InnerText = "From Check Out Date"
                    lblChkToDt.InnerText = "To Check Out Date"
                End If

                txtTranType.Text = "JVR"
                hdnTranType.Value = "JVR"
                hdnDivcode.Value = Request.QueryString("divid")

                ViewState.Add("ExcessProvisionState", Request.QueryString("State"))
                ViewState.Add("RefCode", Request.QueryString("RefCode"))
                txtDocDate.Text = Date.Today.ToString("dd/MM/yyyy")

                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)
                hdnDecimalplaces.Value = decimalPlaces

                Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")
                hdnSealDate.Value = sealdate

                ddlType.Items.Add(New ListItem("Supplier", "Supplier"))
                ddlType.Items.Add(New ListItem("Supplier Agent", "Supplier Agent"))
                ddlType.SelectedIndex = 0

                If ViewState("ExcessProvisionState") = "New" Then
                    'BindGridProvisionReversal()
                    btnSave.Visible = False
                    lblHeadingGrid.Visible = False
                ElseIf ViewState("ExcessProvisionState") = "Edit" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "Edit Excess Provision Reversal"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("RefCode"), String))
                    ShowFillGrid(CType(ViewState("RefCode"), String), "Edit")
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                ElseIf ViewState("ExcessProvisionState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Excess Provision Reversal"
                    btnSave.Visible = False
                    btnPdfReport.Visible = True
                    ShowRecord(CType(ViewState("RefCode"), String))
                    ShowFillGrid(CType(ViewState("RefCode"), String), "View") 'View 31082022 Edit 19012024
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                ElseIf ViewState("ExcessProvisionState") = "Delete" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "Delete Excess Provision Reversal"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("RefCode"), String))
                    ShowFillGrid(CType(ViewState("RefCode"), String), "Delete") 'Delete 31082022 Edit 19012024 reverted not able to view the provision created to edit
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BindGridProvisionReversal()"
    Protected Sub BindGridProvisionReversal()
        Dim dt As New DataTable
        dt.Columns.Add("selection", GetType(Boolean))
        dt.Columns.Add("requestId", GetType(String))
        dt.Columns.Add("rlineno", GetType(Integer))
        dt.Columns.Add("roomno", GetType(Integer))
        dt.Columns.Add("checkin", GetType(String))
        dt.Columns.Add("checkout", GetType(String))
        dt.Columns.Add("servicetype", GetType(String))
        dt.Columns.Add("servicedetails", GetType(String))
        dt.Columns.Add("supplierInvoiceNo", GetType(String))
        dt.Columns.Add("partycode", GetType(String))
        dt.Columns.Add("currcode", GetType(String))
        dt.Columns.Add("convrate", GetType(Decimal))
        dt.Columns.Add("provisionamount", GetType(Decimal))
        dt.Columns.Add("provisionVatAmount", GetType(Decimal))
        dt.Columns.Add("actualamount", GetType(Decimal))
        dt.Columns.Add("actualVatAmount", GetType(Decimal))
        dt.Columns.Add("reversalAmount", GetType(Decimal))
        dt.Columns.Add("reversalVatAmount", GetType(Decimal))
        dt.Columns.Add("ProvCreditorsControlAcct", GetType(String))
        dt.Columns.Add("ProvVATInputCRAcct", GetType(String))
        dt.Columns.Add("CostSalesDiffAcct", GetType(String))
        gvExcessProvision.DataSource = dt
        gvExcessProvision.DataBind()
    End Sub
#End Region

#Region "Protected Sub ShowRecord(ByVal RefCode As String)"

    Protected Sub ShowRecord(ByVal RefCode As String)
        Try
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from provisionReversal_master(nolock) Where tran_id='" & RefCode & "' and divcode='" & hdnDivcode.Value & "'", sqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("tran_id")) = False Then
                        txtDocNo.Text = mySqlReader("tran_id")
                    End If
                    If IsDBNull(mySqlReader("tran_date")) = False Then
                        txtDocDate.Text = mySqlReader("tran_date")
                    End If
                    If mySqlReader("suppliertype") = "A" Then
                        Me.ddlType.SelectedValue = "Supplier Agent"

                    Else
                        Me.ddlType.SelectedValue = "Supplier"
                    End If
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        txtSupplierCode.Text = mySqlReader("partycode")
                        Me.txtSupplier.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast(nolock) where partycode='" & mySqlReader("partycode") & "'")
                    Else
                        txtSupplierCode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("narration")) = False Then
                        Me.txtNarration.Text = CType(mySqlReader("narration"), String)
                    Else
                        Me.txtNarration.Text = ""
                    End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then

                        txtChkFromDt.Text = mySqlReader("fromdate")

                    Else
                        txtChkFromDt.Text = ""

                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then

                        txtChkToDt.Text = mySqlReader("todate")
                    Else
                        txtChkToDt.Text = ""

                    End If


                    txtBookingCode.Text = ""
                    'changed by mohamed on 11/01/2022
                    If IsDBNull(mySqlReader("requestid")) = False Then
                        txtBookingCode.Text = mySqlReader("requestid")
                    End If
                    'Tanvir 08042022
                    If IsDBNull(mySqlReader("Post_To_GLProvision")) = False Then
                        ChkExcGLProvision.Checked = mySqlReader("Post_To_GLProvision")
                    Else
                        ChkExcGLProvision.Checked = False
                    End If


                End If
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(sqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub ShowFillGrid(ByVal RefCode As String, ByVal Mode As String)"
    Protected Sub ShowFillGrid(ByVal RefCode As String, ByVal Mode As String)

        Try
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand("sp_edit_provisionReversal", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = RefCode
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = hdnDivcode.Value
            mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = Mode
            mySqlCmd.CommandTimeout = 0 'Tanvir 31082022
            Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable
                    myDataAdapter.Fill(dt)
                    Dim dc As DataColumn = New DataColumn("Selection", GetType(Boolean))
                    dc.DefaultValue = True
                    dt.Columns.Add(dc)
                    If dt.Rows.Count > 0 Then
                        gvExcessProvision.DataSource = dt
                        gvExcessProvision.DataBind()
                        lblMsg.Visible = False
                    Else
                        lblMsg.Visible = True
                    End If
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
            If gvExcessProvision.Rows.Count > 0 Then
                calculateNetTotal()
            End If
        Catch ex As SqlException
            btnDisplay.Visible = False
            btnClear.Visible = False
            btnSave.Visible = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        End Try
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        ddlType.Enabled = False
        txtSupplier.Enabled = False
        txtBookingCode.Enabled = False 'changed by mohamed 11/01/2022
        txtChkFromDt.Enabled = False
        txtChkToDt.Enabled = False
        chkValidate.Enabled = False
        If ViewState("ExcessProvisionState") = "View" Or ViewState("ExcessProvisionState") = "Delete" Then
            txtDocDate.Enabled = False
            txtNarration.Enabled = False
            btnDisplay.Enabled = False
            btnClear.Enabled = False
            If gvExcessProvision.Rows.Count > 0 Then
                For Each row In gvExcessProvision.Rows
                    Dim txtReversalAmt, txtReversalVatAmt As TextBox
                    txtReversalAmt = CType(row.FindControl("txtReversalAmt"), TextBox)
                    txtReversalVatAmt = CType(row.FindControl("txtReversalVatAmt"), TextBox)
                    txtReversalAmt.Enabled = False
                    txtReversalVatAmt.Enabled = False
                Next
            End If
        End If
    End Sub
#End Region

#Region "Protected Function ValidationDisplay() As Boolean"
    Protected Function ValidationDisplay() As Boolean
        If Not IsDate(txtChkFromDt.Text.Trim) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From check out date can not be empty' );", True)
            txtChkFromDt.Focus()
            ValidationDisplay = False
            Exit Function
        End If
        If Not IsDate(txtChkToDt.Text.Trim) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To check out date can not be empty' );", True)
            txtChkToDt.Focus()
            ValidationDisplay = False
            Exit Function
        End If
        ValidationDisplay = True
    End Function
#End Region

#Region "Protected Function getExcessProvision(ByVal result As String) As DataSet"
    Protected Function getExcessProvision(ByVal result As String) As DataSet
        Try
            If ValidationDisplay() = False Then
                Return Nothing
            End If
            Dim acctType As String
            If ddlType.SelectedValue = "Supplier" Then
                acctType = "S"
            Else
                acctType = "A"
            End If

            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("sp_get_excessProvision", sqlConn)
            mySqlCmd.CommandTimeout = 0 'changed by mohamed on 05/11/2022
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = hdnDivcode.Value.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@supplierType", SqlDbType.VarChar, 20)).Value = acctType
            mySqlCmd.Parameters.Add(New SqlParameter("@supplierCode", SqlDbType.VarChar, 20)).Value = txtSupplierCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = txtBookingCode.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@dateFlag", SqlDbType.VarChar, 1)).Value = hdnChkDtFlag.Value.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@validateEntries", SqlDbType.Bit)).Value = IIf(result <> "Excel", 1, chkValidate.Checked)
            mySqlCmd.Parameters.Add(New SqlParameter("@result", SqlDbType.VarChar, 20)).Value = result
            mySqlCmd.Parameters.Add(New SqlParameter("@todateiscutoffdate", SqlDbType.Int)).Value = IIf(result <> "Excel", 0, IIf(ChkToCheckAsCutOff.Checked = True, 1, 0))

            Dim ds As New DataSet
            Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                myDataAdapter.Fill(ds)
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
            Return ds
        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
            End If
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click"
    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
            Dim ds As DataSet = getExcessProvision("List")
            Dim dt As DataTable = ds.Tables(0)
            Dim dc As DataColumn = New DataColumn("Selection", GetType(Boolean))
            dc.DefaultValue = False
            dt.Columns.Add(dc)

            gvExcessProvision.DataSource = dt
            gvExcessProvision.DataBind()
            If dt.Rows.Count > 0 Then
                lblMsg.Visible = False
                btnSave.Visible = True
                txtBookingCode.Enabled = False 'changed by mohamed 11/01/2022
                txtSupplier.Enabled = False
            Else
                lblMsg.Visible = True
                btnSave.Visible = False
                If ViewState("ExcessProvisionState") <> "View" And ViewState("ExcessProvisionState") <> "Delete" And ViewState("ExcessProvisionState") <> "Edit" Then
                    txtBookingCode.Enabled = True 'changed by mohamed 11/01/2022
                    txtSupplier.Enabled = True
                End If
            End If
            lblHeadingGrid.Visible = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub calculateNetTotal()"
    Protected Sub calculateNetTotal()

        If gvExcessProvision.Rows.Count > 0 Then
            Dim chkSelection As CheckBox
            Dim hdnProvAmt, hdnProvVatAmt, hdnActualAmt, hdnActualVatAmt As HiddenField
            Dim txtReversalAmt, txtReversalVatAmt As TextBox

            Dim TotalProvisionAmt As Decimal = 0.0
            Dim TotalProvisionVatAmt As Decimal = 0.0
            Dim TotalActualAmt As Decimal = 0.0
            Dim TotalActualVatAmt As Decimal = 0.0
            Dim TotalReversalAmt As Decimal = 0.0
            Dim TotalReversalVatAmt As Decimal = 0.0

            Dim decimalPlacaes As Integer = Val(hdnDecimalplaces.Value)
            For Each gvr As GridViewRow In gvExcessProvision.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked Then
                    hdnProvAmt = CType(gvr.FindControl("hdnProvAmt"), HiddenField)
                    hdnProvVatAmt = CType(gvr.FindControl("hdnProvVatAmt"), HiddenField)
                    hdnActualAmt = CType(gvr.FindControl("hdnActualAmt"), HiddenField)
                    hdnActualVatAmt = CType(gvr.FindControl("hdnActualVatAmt"), HiddenField)
                    txtReversalAmt = CType(gvr.FindControl("txtReversalAmt"), TextBox)
                    txtReversalVatAmt = CType(gvr.FindControl("txtReversalVatAmt"), TextBox)

                    TotalProvisionAmt = TotalProvisionAmt + Convert.ToDecimal(Val(hdnProvAmt.Value))
                    TotalProvisionVatAmt = TotalProvisionVatAmt + Convert.ToDecimal(Val(hdnProvVatAmt.Value))
                    TotalActualAmt = TotalActualAmt + Convert.ToDecimal(Val(hdnActualAmt.Value))
                    TotalActualVatAmt = TotalActualVatAmt + Convert.ToDecimal(Val(hdnActualVatAmt.Value))
                    TotalReversalAmt = TotalReversalAmt + Convert.ToDecimal(Val(txtReversalAmt.Text))
                    TotalReversalVatAmt = TotalReversalVatAmt + Convert.ToDecimal(Val(txtReversalVatAmt.Text))

                End If
            Next
            gvExcessProvision.FooterRow.Cells(0).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvExcessProvision.FooterRow.Cells(1).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvExcessProvision.FooterRow.Cells(2).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvExcessProvision.FooterRow.Cells(3).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvExcessProvision.FooterRow.Cells(4).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")

            Dim lblTotalProvAmt As Label
            Dim lblTotalProvVatAmt As Label
            Dim lblTotalActualAmt As Label
            Dim lblTotalActualVatAmt As Label
            Dim lblTotalReversalAmt As Label
            Dim lblTotalReversalVatAmt As Label

            lblTotalProvAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalProvAmt"), Label)
            lblTotalProvVatAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalProvVatAmt"), Label)
            lblTotalActualAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalActualAmt"), Label)
            lblTotalActualVatAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalActualVatAmt"), Label)
            lblTotalReversalAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalReversalAmt"), Label)
            lblTotalReversalVatAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalReversalVatAmt"), Label)

            lblTotalProvAmt.Text = Math.Round(TotalProvisionAmt, decimalPlacaes).ToString()
            lblTotalProvVatAmt.Text = Math.Round(TotalProvisionVatAmt, decimalPlacaes)
            lblTotalActualAmt.Text = Math.Round(TotalActualAmt, decimalPlacaes)
            lblTotalActualVatAmt.Text = Math.Round(TotalActualVatAmt, decimalPlacaes)
            lblTotalReversalAmt.Text = Math.Round(TotalReversalAmt, decimalPlacaes)
            lblTotalReversalVatAmt.Text = Math.Round(TotalReversalVatAmt, decimalPlacaes)

        End If

    End Sub
#End Region

#Region "Protected Sub txtReversalAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtReversalAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If gvExcessProvision.Rows.Count > 0 Then
            Dim chkSelection As CheckBox
            Dim txtReversalAmt As TextBox
            Dim TotalReversalAmt As Decimal = 0.0

            For Each gvr As GridViewRow In gvExcessProvision.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked Then
                    txtReversalAmt = CType(gvr.FindControl("txtReversalAmt"), TextBox)
                    TotalReversalAmt = TotalReversalAmt + Convert.ToDecimal(Val(txtReversalAmt.Text))
                End If
            Next

            Dim lblTotalReversalAmt As Label
            lblTotalReversalAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalReversalAmt"), Label)
            lblTotalReversalAmt.Text = TotalReversalAmt
        End If
    End Sub
#End Region

#Region "Protected Sub txtReversalVatAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtReversalVatAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If gvExcessProvision.Rows.Count > 0 Then
            Dim chkSelection As CheckBox
            Dim txtReversalVatAmt As TextBox
            Dim TotalReversalVatAmt As Decimal = 0.0

            Dim decimalPlacaes As Integer = Val(hdnDecimalplaces.Value)
            For Each gvr As GridViewRow In gvExcessProvision.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked Then
                    txtReversalVatAmt = CType(gvr.FindControl("txtReversalVatAmt"), TextBox)
                    TotalReversalVatAmt = TotalReversalVatAmt + Convert.ToDecimal(Val(txtReversalVatAmt.Text))
                End If
            Next

            Dim lblTotalReversalVatAmt As Label
            lblTotalReversalVatAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalReversalVatAmt"), Label)
            lblTotalReversalVatAmt.Text = Math.Round(TotalReversalVatAmt, decimalPlacaes)
        End If
    End Sub
#End Region

#Region "Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        calculateNetTotal()
    End Sub
#End Region

#Region "Protected Sub chkSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub chkSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        calculateNetTotal()
    End Sub
#End Region

#Region "Protected Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportExcel.Click"
    Protected Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportExcel.Click
        Try
            Dim ds As DataSet = getExcessProvision("Excel")
            Dim filename As String = "ExcessProvision" + Date.Now.ToString("yyyyMMddHHss")
            objUtils.ExportToExcelnew(ds, Response, filename)
        Catch ex As Exception
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtDocDate.Text = Date.Today.ToString("dd/MM/yyyy")
        ddlType.SelectedIndex = 0
        txtSupplier.Text = ""
        txtSupplierCode.Text = ""
        txtNarration.Text = ""
        txtChkFromDt.Text = ""
        txtChkToDt.Text = ""
        chkValidate.Checked = True
        ddlType.Focus()
        gvExcessProvision.DataSource = Nothing
        gvExcessProvision.DataBind()
        lblMsg.Visible = False
        btnSave.Visible = False
        lblHeadingGrid.Visible = False
        If ViewState("ExcessProvisionState") <> "View" And ViewState("ExcessProvisionState") <> "Delete" And ViewState("ExcessProvisionState") <> "Edit" Then
            txtBookingCode.Enabled = True 'changed by mohamed 11/01/2022
            txtSupplier.Enabled = True
        End If

    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ProvisionReversalPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

#Region "Protected Function Validation() As Boolean"
    Protected Function Validation() As Boolean
        Try
            Dim chkSelection As CheckBox
            Dim hdnProvAmt As HiddenField
            Dim hdnProvVatAmt As HiddenField
            Dim hdnActualAmt As HiddenField
            Dim hdnActualVatAmt As HiddenField
            Dim txtReversalAmt As TextBox
            Dim txtReversalVatAmt As TextBox

            Dim decimalplaces As Integer = CInt(Val(hdnDecimalplaces.Value))
            Dim rowselected As Boolean
            rowselected = False
            For Each gvr As GridViewRow In gvExcessProvision.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked Then
                    rowselected = True
                    hdnProvAmt = CType(gvr.FindControl("hdnProvAmt"), HiddenField)
                    hdnProvVatAmt = CType(gvr.FindControl("hdnProvVatAmt"), HiddenField)
                    hdnActualAmt = CType(gvr.FindControl("hdnActualAmt"), HiddenField)
                    hdnActualVatAmt = CType(gvr.FindControl("hdnActualVatAmt"), HiddenField)
                    txtReversalAmt = CType(gvr.FindControl("txtReversalAmt"), TextBox)
                    txtReversalVatAmt = CType(gvr.FindControl("txtReversalVatAmt"), TextBox)

                    'changed by mohamed on 05/11/2022 added this validation And Val(hdnProvAmt.Value) <> (Val(hdnActualAmt.Value) + Val(txtReversalAmt.Text))
                    'changed by mohamed on 18/12/2021 removed this validation 
                    If Val(txtReversalAmt.Text) < 0 And txtBookingCode.Text = "" And Math.Round(Val(hdnProvAmt.Value), decimalplaces) <> Math.Round((Val(hdnActualAmt.Value) + Val(txtReversalAmt.Text)), decimalplaces) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Reversal amount should be greater than or equal to zero');", True)
                        txtReversalAmt.Focus()
                        Validation = False
                        Exit Function
                    End If

                    'changed by mohamed on 05/11/2022 added this validation And Val(hdnProvAmt.Value) <> (Val(hdnActualAmt.Value) + Val(txtReversalAmt.Text))
                    'changed by mohamed on 18/12/2021 removed this validation
                    If Math.Round(Val(hdnProvAmt.Value), decimalplaces) < Math.Round(Val(hdnActualAmt.Value), decimalplaces) And txtBookingCode.Text = "" And Math.Round(Val(hdnProvAmt.Value), decimalplaces) <> Math.Round((Val(hdnActualAmt.Value) + Val(txtReversalAmt.Text)), decimalplaces) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Provision amount should be greater than actual amount');", True)
                        Validation = False
                        Exit Function
                    End If

                    If Math.Round(Val(hdnProvAmt.Value) - Val(hdnActualAmt.Value), decimalplaces) < Math.Round(Val(txtReversalAmt.Text), decimalplaces) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Reversal amount should be less than or equal to amount of difference between provision amount and actual amount');", True)
                        txtReversalAmt.Focus()
                        Validation = False
                        Exit Function
                    End If

                    'changed by mohamed on 05/11/2022 added this validation And Val(hdnProvVatAmt.Value) <> (Val(hdnActualVatAmt.Value) + Val(txtReversalVatAmt.Text))
                    'changed by mohamed on 18/12/2021 removed this validation
                    If Val(txtReversalVatAmt.Text) < 0 And txtBookingCode.Text = "" And Math.Round(Val(hdnProvVatAmt.Value), decimalplaces) <> Math.Round((Val(hdnActualVatAmt.Value) + Val(txtReversalVatAmt.Text)), decimalplaces) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Reversal VAT amount should be greater than or equal to zero');", True)
                        txtReversalVatAmt.Focus()
                        Validation = False
                        Exit Function
                    End If

                    'changed by mohamed on 05/11/2022 added this validation And Val(hdnProvVatAmt.Value) <> (Val(hdnActualVatAmt.Value) + Val(txtReversalVatAmt.Text))
                    'changed by mohamed on 18/12/2021 removed this validation
                    If Val(hdnProvVatAmt.Value) < Val(hdnActualVatAmt.Value) And txtBookingCode.Text = "" And Math.Round(Val(hdnProvVatAmt.Value), decimalplaces) <> Math.Round((Val(hdnActualVatAmt.Value) + Val(txtReversalVatAmt.Text)), decimalplaces) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Provision VAT amount should be greater than actual VAT amount');", True)
                        txtReversalVatAmt.Focus()
                        Validation = False
                        Exit Function
                    End If

                    If Math.Round(Val(hdnProvVatAmt.Value) - Val(hdnActualVatAmt.Value), decimalplaces) < Val(txtReversalVatAmt.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Reversal VAT amount should be less than or equal to amount of difference between provision VAT amount and actual VAT amount');", True)
                        txtReversalVatAmt.Focus()
                        Validation = False
                        Exit Function
                    End If

                End If
            Next

            'changed by mohamed on 06/11/2022 as all pendings for sealed date will appear which they used to do with booking id
            If txtSupplierCode.Text.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select the supplier to save');", True)
                Validation = False
                Exit Function
            End If

            If ViewState("ExcessProvisionState") = "New" And rowselected = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select rows in the list');", True)
                Validation = False
                Exit Function
            End If

            If txtTranType.Text = "" Or hdnTranType.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Transaction type Cannot be Blank');", True)
                txtTranType.Focus()
                Validation = False
                Exit Function
            End If

            If txtNarration.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Narration Cannot be Blank');", True)
                txtNarration.Focus()
                Validation = False
                Exit Function
            End If
            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from sealing_master")
            If IsDate(sealdate) Then
                If (CType(txtDocDate.Text, Date) <= CType(sealdate, Date)) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Journal Date Should be greater than seal date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                    txtDocDate.Focus()
                    Validation = False
                    Exit Function
                End If
            End If
            Validation = True
        Catch ex As Exception
            Validation = False
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Validation() = False Then Exit Sub

            If ViewState("ExcessProvisionState") = "New" Or ViewState("ExcessProvisionState") = "Edit" Then
                Dim dt As New DataTable
                dt.Columns.Add("tran_lineno", GetType(Integer))
                dt.Columns.Add("requestId", GetType(String))
                dt.Columns.Add("rlineno", GetType(Integer))
                dt.Columns.Add("roomno", GetType(Integer))
                dt.Columns.Add("checkin", GetType(String))
                dt.Columns.Add("checkout", GetType(String))
                dt.Columns.Add("servicetype", GetType(String))
                dt.Columns.Add("servicedetails", GetType(String))
                dt.Columns.Add("supplierInvoiceNo", GetType(String))
                dt.Columns.Add("partycode", GetType(String))
                dt.Columns.Add("currcode", GetType(String))
                dt.Columns.Add("convrate", GetType(Decimal))
                dt.Columns.Add("provisionamount", GetType(Decimal))
                dt.Columns.Add("provisionVatAmount", GetType(Decimal))
                dt.Columns.Add("actualamount", GetType(Decimal))
                dt.Columns.Add("actualVatAmount", GetType(Decimal))
                dt.Columns.Add("reversalAmount", GetType(Decimal))
                dt.Columns.Add("reversalVatAmount", GetType(Decimal))
                dt.Columns.Add("ProvCreditorsControlAcct", GetType(String))
                dt.Columns.Add("ProvVATInputCRAcct", GetType(String))
                dt.Columns.Add("CostSalesDiffAcct", GetType(String))

                Dim chkSelection As New CheckBox
                Dim hdnRequestId, hdnRlineNo, hdnRoomNo, hdnSupCode, hdnServiceType, hdnProvAmt, hdnProvVatAmt As HiddenField
                Dim hdnActualAmt, hdnActualVatAmt, hdnProvControlAcct, hdnProvVatAcct, hdnCostSalesDiffAcct As HiddenField
                Dim lblCheckIn, lblCheckOut, lblService, lblSupInvNo, lblCurrcode, lblConvrate As Label
                Dim txtReversalAmt, txtReversalVatAmt As TextBox

                Dim cnt As Integer = 0
                For Each gvr As GridViewRow In gvExcessProvision.Rows
                    chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                    If chkSelection.Checked = True Then
                        cnt = cnt + 1
                        hdnRequestId = CType(gvr.FindControl("hdnRequestId"), HiddenField)
                        hdnRlineNo = CType(gvr.FindControl("hdnRlineNo"), HiddenField)
                        hdnRoomNo = CType(gvr.FindControl("hdnRoomNo"), HiddenField)
                        lblCheckIn = CType(gvr.FindControl("lblCheckIn"), Label)
                        lblCheckOut = CType(gvr.FindControl("lblCheckOut"), Label)
                        lblService = CType(gvr.FindControl("lblService"), Label)
                        lblSupInvNo = CType(gvr.FindControl("lblSupInvNo"), Label)
                        lblCurrcode = CType(gvr.FindControl("lblCurrcode"), Label)
                        lblConvrate = CType(gvr.FindControl("lblConvrate"), Label)
                        hdnSupCode = CType(gvr.FindControl("hdnSupCode"), HiddenField)
                        hdnServiceType = CType(gvr.FindControl("hdnServiceType"), HiddenField)

                        hdnProvAmt = CType(gvr.FindControl("hdnProvAmt"), HiddenField)
                        hdnProvVatAmt = CType(gvr.FindControl("hdnProvVatAmt"), HiddenField)
                        hdnActualAmt = CType(gvr.FindControl("hdnActualAmt"), HiddenField)
                        hdnActualVatAmt = CType(gvr.FindControl("hdnActualVatAmt"), HiddenField)
                        txtReversalAmt = CType(gvr.FindControl("txtReversalAmt"), TextBox)
                        txtReversalVatAmt = CType(gvr.FindControl("txtReversalVatAmt"), TextBox)
                        hdnProvControlAcct = CType(gvr.FindControl("hdnProvControlAcct"), HiddenField)
                        hdnProvVatAcct = CType(gvr.FindControl("hdnProvVatAcct"), HiddenField)
                        hdnCostSalesDiffAcct = CType(gvr.FindControl("hdnCostSalesDiffAcct"), HiddenField)

                        Dim dr As DataRow = dt.NewRow
                        dr("tran_lineno") = cnt
                        dr("requestId") = hdnRequestId.Value
                        dr("rlineno") = hdnRlineNo.Value
                        dr("roomno") = hdnRoomNo.Value
                        dr("checkin") = Convert.ToDateTime(lblCheckIn.Text).ToString("yyyy/MM/dd")
                        dr("checkout") = Convert.ToDateTime(lblCheckOut.Text).ToString("yyyy/MM/dd")
                        dr("servicetype") = hdnServiceType.Value
                        dr("servicedetails") = lblService.Text.Trim
                        dr("supplierInvoiceNo") = lblSupInvNo.Text.Trim
                        dr("partycode") = hdnSupCode.Value.Trim
                        dr("currcode") = lblCurrcode.Text.Trim
                        dr("convrate") = Val(lblConvrate.Text)
                        dr("provisionamount") = Val(hdnProvAmt.Value)
                        dr("provisionVatAmount") = Val(hdnProvVatAmt.Value)
                        dr("actualamount") = Val(hdnActualAmt.Value)
                        dr("actualVatAmount") = Val(hdnActualVatAmt.Value)
                        dr("reversalAmount") = Val(txtReversalAmt.Text)
                        dr("reversalVatAmount") = Val(txtReversalVatAmt.Text)
                        dr("ProvCreditorsControlAcct") = hdnProvControlAcct.Value.Trim
                        dr("ProvVATInputCRAcct") = hdnProvVatAcct.Value.Trim
                        dr("CostSalesDiffAcct") = hdnCostSalesDiffAcct.Value.Trim
                        dt.Rows.Add(dr)
                    End If
                Next

                Dim provisionReversalXml As String = ""
                If dt.Rows.Count > 0 Then
                    dt.TableName = "provisionReversal"
                    provisionReversalXml = objUtils.GenerateXML(dt)
                End If

                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                sqlTrans = sqlConn.BeginTransaction
                Dim optionval As String
                Dim optionForId As String = Convert.ToString(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2039'"))
                Dim Month As String = Format(Convert.ToDateTime(txtDocDate.Text), "MM")
                Dim Year As String = Format(Convert.ToDateTime(txtDocDate.Text), "yyyy")

                If ViewState("ExcessProvisionState") = "New" Then
                    If optionForId = "Year" Then
                        optionval = objUtils.GetAutoDocNodiv(hdnTranType.Value.Trim, sqlConn, sqlTrans, hdnDivcode.Value)
                        txtDocNo.Text = optionval.Trim
                    ElseIf optionForId = "Month" Then
                        mySqlCmd = New SqlCommand("sp_get_monthyear_number", sqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@docgen_div_optionName ", SqlDbType.VarChar, 20)).Value = hdnTranType.Value.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = hdnDivcode.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = hdnTranType.Value.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@docmonth ", SqlDbType.VarChar, 10)).Value = Month
                        mySqlCmd.Parameters.Add(New SqlParameter("@docyear", SqlDbType.VarChar, 10)).Value = Year '2008
                        mySqlCmd.Parameters.Add(New SqlParameter("@Code", SqlDbType.VarChar, 20))
                        mySqlCmd.Parameters("@Code").Direction = ParameterDirection.Output
                        mySqlCmd.ExecuteNonQuery()
                        txtDocNo.Text = mySqlCmd.Parameters("@Code").Value.ToString()
                    End If
                End If
                If ViewState("ExcessProvisionState") = "New" Then
                    mySqlCmd = New SqlCommand("sp_add_provisionReversalHeader", sqlConn, sqlTrans)
                ElseIf ViewState("ExcessProvisionState") = "Edit" Then
                    mySqlCmd = New SqlCommand("sp_mod_provisionReversalHeader", sqlConn, sqlTrans)
                End If
                Dim d As String = Format(CType(txtDocDate.Text, Date), "yyyy/MM/dd")
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = hdnDivcode.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@trandate", SqlDbType.DateTime)).Value = Format(CType(txtDocDate.Text, Date), "yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = hdnTranType.Value.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = txtSupplierCode.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, -1)).Value = txtNarration.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@fromDate", SqlDbType.DateTime)).Value = Format(CType(txtChkFromDt.Text, Date), "yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@toDate", SqlDbType.DateTime)).Value = Format(CType(txtChkToDt.Text, Date), "yyyy/MM/dd")
                If ddlType.SelectedValue = "Supplier" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@suppliertype", SqlDbType.VarChar, 10)).Value = "S"
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@suppliertype", SqlDbType.VarChar, 10)).Value = "A"
                End If
                mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 100)).Value = txtBookingCode.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@validateEntries", SqlDbType.Bit)).Value = chkValidate.Checked
                mySqlCmd.Parameters.Add(New SqlParameter("@Post_To_GLProvision", SqlDbType.Bit)).Value = ChkExcGLProvision.Checked 'Tanvir 08042022
                If gvExcessProvision.Rows.Count > 0 Then
                    Dim lblTotalProvAmt As Label
                    Dim lblTotalProvVatAmt As Label
                    Dim lblTotalActualAmt As Label
                    Dim lblTotalActualVatAmt As Label
                    Dim lblTotalReversalAmt As Label
                    Dim lblTotalReversalVatAmt As Label

                    lblTotalProvAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalProvAmt"), Label)
                    lblTotalProvVatAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalProvVatAmt"), Label)
                    lblTotalActualAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalActualAmt"), Label)
                    lblTotalActualVatAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalActualVatAmt"), Label)
                    lblTotalReversalAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalReversalAmt"), Label)
                    lblTotalReversalVatAmt = CType(gvExcessProvision.FooterRow.FindControl("lblTotalReversalVatAmt"), Label)

                    mySqlCmd.Parameters.Add(New SqlParameter("@totalprovisionamount", SqlDbType.Decimal)).Value = Convert.ToDecimal(Val(lblTotalProvAmt.Text))
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalvatprovision", SqlDbType.Decimal)).Value = Convert.ToDecimal(Val(lblTotalProvVatAmt.Text))
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalActualamount", SqlDbType.Decimal)).Value = Convert.ToDecimal(Val(lblTotalActualAmt.Text))
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalActualVatamount", SqlDbType.Decimal)).Value = Convert.ToDecimal(Val(lblTotalActualVatAmt.Text))
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalReversalAmount", SqlDbType.Decimal)).Value = Convert.ToDecimal(Val(lblTotalReversalAmt.Text))
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalReversalVatAmount", SqlDbType.Decimal)).Value = Convert.ToDecimal(Val(lblTotalReversalVatAmt.Text))

                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalprovisionamount", SqlDbType.Decimal)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalvatprovision", SqlDbType.Decimal)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalActualamount", SqlDbType.Decimal)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalActualVatamount", SqlDbType.Decimal)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalReversalAmount", SqlDbType.Decimal)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@totalReversalVatAmount", SqlDbType.Decimal)).Value = 0
                End If
                mySqlCmd.CommandTimeout = 0 'Tanvir 01092022
                mySqlCmd.ExecuteNonQuery()

                mySqlCmd = New SqlCommand("sp_del_provisionReversaldetails", sqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode ", SqlDbType.VarChar, 10)).Value = hdnDivcode.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = hdnTranType.Value.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.CommandTimeout = 0 'Tanvir 01092022
                mySqlCmd.ExecuteNonQuery()

                mySqlCmd = New SqlCommand("sp_add_provisionReversaldetails", sqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = hdnDivcode.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = hdnTranType.Value.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@provisionXml", SqlDbType.VarChar, -1)).Value = provisionReversalXml
                mySqlCmd.CommandTimeout = 0 'Tanvir 01092022
                mySqlCmd.ExecuteNonQuery()

                mySqlCmd = New SqlCommand("sp_post_provisiondata", sqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode ", SqlDbType.VarChar, 10)).Value = hdnDivcode.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = hdnTranType.Value.Trim
                If ViewState("ExcessProvisionState") = "New" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = "new"
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = "Amend"
                End If
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.CommandTimeout = 0 'Tanvir 01092022
                mySqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbConnectionClose(sqlConn)

                sqlTrans = Nothing
                ModalPopupLoading.Hide()

            ElseIf ViewState("ExcessProvisionState") = "Delete" Then
                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                sqlTrans = sqlConn.BeginTransaction
                mySqlCmd = New SqlCommand("sp_del_provisionReversal", sqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode ", SqlDbType.VarChar, 10)).Value = hdnDivcode.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = hdnTranType.Value.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.CommandTimeout = 0 'Tanvir 01092022
                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbConnectionClose(sqlConn)

                sqlTrans = Nothing
                ModalPopupLoading.Hide()

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record deleted Successfully' ); ", True) ' 
            End If

            If ViewState("ExcessProvisionState") = "Delete" Then
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ProvisionReversalPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            ElseIf ViewState("ExcessProvisionState") = "New" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record added successfully');", True)
            ElseIf ViewState("ExcessProvisionState") = "Edit" Then
                Dim strURL As String = ""
                strURL = "window.open('Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Text & "&ttype=" & hdnTranType.Value.Trim & "&divid=" & hdnDivcode.Value.Trim & "&tdate=" & txtDocDate.Text.Trim + "','Log','width=100,height=100 left=20,top=20 status=1,toolbar=no,menubar=no,resizable=no,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strURL, True)
            End If
            btnSave.Visible = False
            btnPdfReport.Visible = True
            ViewState("ExcessProvisionState") = "View"
            DisableControl()
        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                sqlTrans.Rollback()
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
            End If
            Dim errMsg As String = Regex.Replace(ex.Message.ToString(), "[^0-9a-zA-Z\._ ]", " ")
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + errMsg & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click"
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('TransactionReports.aspx?printId=JournalDoc&Tranid=" & txtDocNo.Text.Trim & "&divid=" & hdnDivcode.Value & "&TranType=" & hdnTranType.Value.Trim & "&PrntSec=0','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcessProvisionReversal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvExcessProvision_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExcessProvision.RowDataBound"
    Protected Sub gvExcessProvision_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExcessProvision.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = Convert.ToInt32(hdnDecimalplaces.Value)
            Dim lblProvAmt As Label = CType(e.Row.FindControl("lblProvAmt"), Label)
            Dim lblProvVatAmt As Label = CType(e.Row.FindControl("lblProvVatAmt"), Label)
            Dim lblActualAmt As Label = CType(e.Row.FindControl("lblActualAmt"), Label)
            Dim lblActualVatAmt As Label = CType(e.Row.FindControl("lblActualVatAmt"), Label)
            Dim txtReversalAmt As TextBox = CType(e.Row.FindControl("txtReversalAmt"), TextBox)
            Dim txtReversalVatAmt As TextBox = CType(e.Row.FindControl("txtReversalVatAmt"), TextBox)
            If IsNumeric(lblProvAmt.Text) Then
                lblProvAmt.Text = Math.Round(Convert.ToDecimal(lblProvAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(lblProvVatAmt.Text) Then
                lblProvVatAmt.Text = Math.Round(Convert.ToDecimal(lblProvVatAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(lblActualAmt.Text) Then
                lblActualAmt.Text = Math.Round(Convert.ToDecimal(lblActualAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(lblActualVatAmt.Text) Then
                lblActualVatAmt.Text = Math.Round(Convert.ToDecimal(lblActualVatAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(txtReversalAmt.Text) Then
                txtReversalAmt.Text = Math.Round(Convert.ToDecimal(txtReversalAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(txtReversalVatAmt.Text) Then
                txtReversalVatAmt.Text = Math.Round(Convert.ToDecimal(txtReversalVatAmt.Text), decimalPlaces).ToString
            End If
        End If
    End Sub
#End Region



End Class
