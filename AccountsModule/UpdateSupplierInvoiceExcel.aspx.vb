Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports ClosedXML.Excel
Imports DocumentFormat
Imports System.Security.AccessControl
Imports iTextSharp.text
Imports DocumentFormat.OpenXml.EMMA
Imports Org.BouncyCastle.Asn1.Ocsp

Partial Class UpdateSupplierInvoiceExcel
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
    <System.Web.Script.Services.ScriptMethod()>
    <System.Web.Services.WebMethod()>
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
#Region "Private Sub BindGVUpdateSupplier(ByVal rowcount As Integer)"
    Private Sub BindGVUpdateSupplier(ByVal rowcount As Integer)
        Dim dt As New DataTable
        dt.Columns.Add("selection", GetType(Boolean))
        dt.Columns.Add("requestId", GetType(String))
        dt.Columns.Add("rlineno", GetType(Integer))
        dt.Columns.Add("roomno", GetType(Integer))
        dt.Columns.Add("checkin", GetType(Date))
        dt.Columns.Add("checkout", GetType(Date))
        dt.Columns.Add("paxdetails", GetType(String))
        dt.Columns.Add("servicetype", GetType(String))
        dt.Columns.Add("GuestDetails", GetType(String))
        dt.Columns.Add("servicedetails", GetType(String))
        dt.Columns.Add("supconfno", GetType(String))
        dt.Columns.Add("provisionamount", GetType(Decimal))
        dt.Columns.Add("vatprovision", GetType(Decimal))
        dt.Columns.Add("actualamount", GetType(Decimal))
        dt.Columns.Add("prices_costnontaxablevaluebase", GetType(Decimal))
        dt.Columns.Add("prices_costtaxablevaluebase", GetType(Decimal))
        dt.Columns.Add("vatperc", GetType(Decimal))
        dt.Columns.Add("prices_costvatvaluebasecurrent", GetType(Decimal))
        dt.Columns.Add("prices_costvatvaluebase", GetType(Decimal))
        dt.Columns.Add("totalprice", GetType(Decimal))
        dt.Columns.Add("PaxType", GetType(String))
        dt.Columns.Add("Commission", GetType(Decimal))
        Session("SupplierInvoice") = dt
        If rowcount > 0 Then
            For i = 0 To rowcount - 1
                dt.Rows.Add("false", DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value)
            Next
        End If
        gvUpdateSupplier.DataSource = dt
        gvUpdateSupplier.DataBind()
    End Sub
#End Region
#Region "    Private Sub BindGVPricebreakup()"
    Private Sub BindGVPricebreakup()
        Dim pdt As New DataTable
        pdt.Columns.Add("divcode", GetType(String))
        pdt.Columns.Add("purchaseinvoiceno", GetType(String))
        pdt.Columns.Add("purchaseinvoicetype", GetType(String))
        pdt.Columns.Add("requestId", GetType(String))
        pdt.Columns.Add("rlineno", GetType(Integer))
        pdt.Columns.Add("roomno", GetType(Integer))
        pdt.Columns.Add("servicetype", GetType(String))
        pdt.Columns.Add("paxtype", GetType(String))
        pdt.Columns.Add("noofpax", GetType(Integer))
        pdt.Columns.Add("childages", GetType(String))
        pdt.Columns.Add("pricedate", GetType(Date))
        pdt.Columns.Add("bookingcode", GetType(String))
        pdt.Columns.Add("bookingname", GetType(String))
        pdt.Columns.Add("rate", GetType(Decimal))
        pdt.Columns.Add("currcode", GetType(String))
        pdt.Columns.Add("saleprice", GetType(Decimal))
        pdt.Columns.Add("salepricebase", GetType(Decimal))
        pdt.Columns.Add("costprice", GetType(Decimal))
        pdt.Columns.Add("costvalue", GetType(Decimal))
        pdt.Columns.Add("CostTaxableValue", GetType(Decimal))
        pdt.Columns.Add("CostNonTaxableValue", GetType(Decimal))
        pdt.Columns.Add("CostVatValue", GetType(Decimal))
        pdt.Columns.Add("Commission", GetType(Decimal))
        Session("priceListDt") = pdt
    End Sub
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

                Dim priceDateFlag As String = Convert.ToString(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='5513'"))
                If priceDateFlag = "Y" Then
                    hdnPricedateFlag.Value = "Y"
                Else
                    hdnPricedateFlag.Value = "N"
                End If

                txtDivcode.Text = Request.QueryString("divid")

                ViewState.Add("SupplierInvoiceState", Request.QueryString("State"))

                'ViewState.Add("ID", Request.QueryString("ID"))
                txtDocDate.Text = Date.Today.ToString("dd/MM/yyyy")
                txtChkFromDt.Text = Date.Today.ToString("dd/MM/yyyy")
                txtChkToDt.Text = Date.Today.ToString("dd/MM/yyyy")

                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)
                Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")
                hdnSealDate.Value = sealdate
                'ddlType.Items.Add(New ListItem("Supplier", "Supplier"))
                'ddlType.Items.Add(New ListItem("Supplier Agent", "Supplier Agent"))
                'ddlType.SelectedIndex = 0
                hdnCommissionFlag.Value = "False"
                Session("Commission") = "False"
                'Dim Commision As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='5314'")
                'If Commision = "Y" Then
                '    'ddlCommissionType.Items(0).Selected = True
                '    'hdnCommissionFlag.Value = "true"
                'Else
                '    'ddlCommissionType.Items(1).Selected = True
                '    'ddlCommissionType.Attributes.Add("disabled", "true")
                'End If

                ddlCommissionType.SelectedValue = "Select"

                Session("SuppInv") = Nothing
                Session("BookingNo") = Nothing
                Session("narration") = Nothing

                ExcelConfigData()

                If ViewState("SupplierInvoiceState") = "New" Then
                    BindGVUpdateSupplier(0)
                    BindGVPricebreakup()
                    ShowRecord(CType(ViewState("ID"), String))
                    ' btnSave.Attributes.Add("onclick", "return CompleteValidate('" & ViewState("SupplierInvoiceState") & "')")
                ElseIf ViewState("SupplierInvoiceState") = "Edit" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "Edit Update Supplier Invoice"
                    '' btnSave.Visible = False
                    BindGVUpdateSupplier(0)
                    BindGVPricebreakup()
                    ShowRecord(CType(ViewState("ID"), String))
                    ShowFillGrid(CType(ViewState("ID"), String), "Edit")
                    btnCancel.Text = "Return to Search"
                    DisableControl()


                ElseIf ViewState("SupplierInvoiceState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Update Supplier Invoice"
                    btnSave.Visible = False
                    BindGVPricebreakup()
                    ShowRecord(CType(ViewState("ID"), String))
                    ShowFillGrid(CType(ViewState("ID"), String), "View")
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                ElseIf ViewState("SupplierInvoiceState") = "Delete" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "Delete Update Supplier Invoice"
                    btnSave.Text = "Delete"
                    BindGVPricebreakup()
                    ShowRecord(CType(ViewState("ID"), String))
                    ShowFillGrid(CType(ViewState("ID"), String), "Delete")
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                End If
                'txtRequestId.Enabled = False
                'lblMsg.Visible = False
                'ModalPopupLoading.Hide()
                Dim dt As DataTable = CType(Session("SupplierInvoice"), DataTable)
                gvUpdateSupplier.DataSource = dt
                gvUpdateSupplier.DataBind()

                Errtr1.Visible = False
                Errtr2.Visible = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Excel Config Data"
    Private Sub ExcelConfigData()
        Try
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            mySqlCmd = New SqlCommand("sp_get_UpdateSupplierXLUploadConfig", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            myDataAdapter = New SqlDataAdapter(mySqlCmd)
            Dim dt As New DataTable
            myDataAdapter.Fill(dt)
            Session("sExcelConfigData") = dt
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region " Private Function FillGvUpdateEdit(ByVal RefCode As String) As DataTable"
    Private Function FillGvUpdateEdit(ByVal RefCode As String) As DataTable

        Try

            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "Select * from purchaseinvoicehoteldetail Where divcode='" & txtDivcode.Text & "' and  purchaseinvoiceno='" & RefCode & "' union select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,rlineno,roomno," _
         & "  spleventdate as checkin ,spleventdate as  checkout,paxdetails,servicedetails,supconfno,provisionamount,vatprovision,actualamount,prices_costnontaxablevaluebase,prices_costtaxablevaluebase,vatperc,prices_costvatvaluebase,totalprice,servicetype,GuestDetails,paxtype,Commission from purchaseinvoicespleventsdetail" _
         & " Where divcode='" & txtDivcode.Text & "' and  purchaseinvoiceno='" & RefCode & "' union " _
         & "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,rlineno,roomno,transferdate as checkin ,transferdate as  checkout,paxdetails,servicedetails, supconfno,provisionamount,vatprovision,actualamount, " _
        & "  prices_costnontaxablevaluebase,prices_costtaxablevaluebase,vatperc,prices_costvatvaluebase,totalprice,servicetype,GuestDetails,Paxtype,Commission   from PurchaseInvoiceTransfersdetail     Where divcode='" & txtDivcode.Text & "' and  purchaseinvoiceno='" & RefCode & "' union " _
             & " select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,elineno as rlineno,roomno,ExcDate as checkin ,ExcDate as  checkout,paxdetails,servicedetails, supconfno,provisionamount,vatprovision,actualamount, " _
        & "  prices_costnontaxablevaluebase,prices_costtaxablevaluebase,vatperc,prices_costvatvaluebase,totalprice,servicetype,GuestDetails,paxtype,Commission  from PurchaseInvoiceToursdetail     Where divcode='" & txtDivcode.Text & "' and  purchaseinvoiceno='" & RefCode & "' union  " _
        & " select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,alineno as rlineno, roomno,AirportMADate as checkin ,AirportMADate as  checkout,paxdetails,servicedetails, supconfno,provisionamount,vatprovision,actualamount, " _
        & "  prices_costnontaxablevaluebase,prices_costtaxablevaluebase,vatperc,prices_costvatvaluebase,totalprice,servicetype,GuestDetails,paxtype ,Commission  from PurchaseInvoice_Airportma_detail     Where divcode='" & txtDivcode.Text & "' and  purchaseinvoiceno='" & RefCode & "' union  " _
                  & " select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,olineno as rlineno, roomno,othDate as checkin ,Othdate as  checkout,paxdetails,servicedetails, supconfno,provisionamount,vatprovision,actualamount, " _
        & "  prices_costnontaxablevaluebase,prices_costtaxablevaluebase,vatperc,prices_costvatvaluebase,totalprice,servicetype,GuestDetails,paxtype ,Commission from PurchaseInvoice_Others_detail     Where divcode='" & txtDivcode.Text & "' and  purchaseinvoiceno='" & RefCode & "' union " _
                              & " select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,vlineno as rlineno,roomno,visaDate as checkin ,visadate as  checkout,paxdetails,servicedetails, supconfno,provisionamount,vatprovision,actualamount, " _
        & "  prices_costnontaxablevaluebase,prices_costtaxablevaluebase,vatperc,prices_costvatvaluebase,totalprice,servicetype,GuestDetails,paxtype ,Commission from PurchaseInvoice_visa_detail     Where divcode='" & txtDivcode.Text & "' and  purchaseinvoiceno='" & RefCode & "' order by purchaseinvoiceno"

            mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
            Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable
                    myDataAdapter.Fill(dt)
                    Dim dc As DataColumn = New DataColumn("Selection", GetType(Boolean))
                    dc.DefaultValue = True
                    dt.Columns.Add(dc)
                    Return dt
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            'clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(sqlConn)
        End Try
    End Function
#End Region

    'Tanvir 24102023
    Private Sub getprovision(ByVal requestids As String)

        mySqlCmd = New SqlCommand("sp_get_PIpostingrecords", sqlConn, sqlTrans)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.CommandTimeout = 0
        mySqlCmd.Parameters.Add(New SqlParameter("@div_code ", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
        mySqlCmd.Parameters.Add(New SqlParameter("@tran_id ", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
        mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"

        Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
            Using dt As New DataTable
                myDataAdapter.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Session("PIAdjustedRecords") = dt
                End If
            End Using
        End Using
    End Sub
    'Tanvir 24102023


    Private Sub ShowFillGrid(ByVal RefCode As String, ByVal Mode As String)
        Dim invRequestIds As String = ""
        Try
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand("sp_edit_supplier_purchaseInvoice", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@purchaseInvoiceNo", SqlDbType.VarChar, 20)).Value = RefCode
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = Mode
            Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable
                    myDataAdapter.Fill(dt)
                    'Tanvir 24102023
                    If dt.Rows.Count > 0 Then
                        Session("PIRecords") = CType(dt, DataTable)
                    End If


                    For Each row As DataRow In dt.Rows
                        If invRequestIds = "" Then
                            invRequestIds = "" + row("requestid") + ""
                        Else
                            invRequestIds = invRequestIds + "," + row("requestid") + ""
                        End If

                    Next
                    'Tanvir 24102023

                    Dim sum As Decimal = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(Commission)", String.Empty))), 0, Math.Round(Convert.ToDecimal(dt.Compute("sum(Commission)", String.Empty)), 2))
                    If sum > 0 Then
                        'ddlCommissionType.SelectedIndex = 1
                        gvUpdateSupplier.Columns(18).Visible = True
                        gvPriceList.Columns(13).Visible = True
                        ddlfillamount.Items.Add("Commission")
                        hdnCommissionFlag.Value = "true"
                        Session("Commission") = "true"
                    End If

                    Dim dc As DataColumn = New DataColumn("Selection", GetType(Boolean))
                    dc.DefaultValue = True
                    dt.Columns.Add(dc)
                    If dt.Rows.Count > 0 Then
                        gvUpdateSupplier.DataSource = dt
                        gvUpdateSupplier.DataBind()

                        lblMsg.Visible = False
                    Else
                        lblMsg.Visible = True
                    End If
                End Using
            End Using
            getprovision(invRequestIds)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
            If gvUpdateSupplier.Rows.Count > 0 Then
                calculateNetTotal()
                Dim lblTotalVatAmt As Label = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatAmt"), Label)
                hdCurrenttotalvatactual.Value = lblTotalVatAmt.Text
            End If
        Catch ex As SqlException
            btnDisplay.Visible = False
            btnClear.Visible = False
            btnSave.Visible = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            'clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(sqlConn)
        End Try
    End Sub
#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from purchaseinvoiceheader Where purchaseinvoiceno='" & RefCode & "' and divcode='" & txtDivcode.Text & "'", sqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("purchaseinvoiceno")) = False Then
                        txtDocNo.Text = mySqlReader("purchaseinvoiceno")
                    End If
                    If IsDBNull(mySqlReader("purchaseinvoicedate")) = False Then
                        txtDocDate.Text = mySqlReader("purchaseinvoicedate")
                    End If
                    If IsDBNull(mySqlReader("suppliertype")) = False Then
                        If mySqlReader("purchaseinvoicetype") = "S" Then
                            'Me.ddlType.SelectedValue = "Supplier"
                        ElseIf mySqlReader("purchaseinvoicetype") = "A" Then
                            ' Me.ddlType.SelectedValue = "Supplier Agent"
                        End If
                    End If
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        txtSupplierCode.Text = mySqlReader("partycode")
                        Me.txtSupplier.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & mySqlReader("partycode") & "'")
                        Me.txtTrnNo.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select TRNNo from partymast where partycode='" & mySqlReader("partycode") & "'")
                    Else
                        txtSupplierCode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("hotelinvoiceno")) = False Then
                        'Me.txtInvoiceNo.Text = CType(mySqlReader("hotelinvoiceno"), String)
                    Else
                        'Me.txtInvoiceNo.Text = ""
                    End If
                    If IsDBNull(mySqlReader("Bookingrefno")) = False Then
                        'Me.txtRequestId.Text = CType(mySqlReader("Bookingrefno"), String)
                    Else
                        'Me.txtRequestId.Text = ""
                    End If
                    If IsDBNull(mySqlReader("narration")) = False Then
                        'Me.txtNarration.Text = CType(mySqlReader("narration"), String)
                    Else
                        ' Me.txtNarration.Text = ""
                    End If

                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        Me.txtCtrlAcctCode.Text = CType(mySqlReader("controlacctcode"), String)
                        Me.txtCtrlAcct.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctcode='" & mySqlReader("controlacctcode") & "'")
                    Else
                        Me.txtCtrlAcctCode.Text = ""
                        Me.txtCtrlAcct.Text = ""
                    End If

                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.txtCurrencyCode.Text = CType(mySqlReader("currcode"), String)
                        Me.txtCurrency.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currname from currmast where currcode='" & mySqlReader("currcode") & "'")
                    Else
                        Me.txtCurrencyCode.Text = ""
                        Me.txtCurrency.Text = ""
                    End If

                    If IsDBNull(mySqlReader("convrate")) = False Then
                        Me.txtConvRate.Text = CType(mySqlReader("convrate"), String)

                    Else
                        Me.txtConvRate.Text = ""

                    End If

                    If IsDBNull(mySqlReader("frmchkout")) = False Then

                        'txtChkFromDt.Text = mySqlReader("frmchkout")

                    Else
                        'txtChkFromDt.Text = ""

                    End If
                    If IsDBNull(mySqlReader("tochkout")) = False Then

                        'txtChkToDt.Text = mySqlReader("tochkout")
                    Else
                        'txtChkToDt.Text = ""

                    End If
                    'If IsDBNull(mySqlReader("Commission")) = False Then

                    '    txtCommisionAmt.Text = mySqlReader("Commission")
                    'Else
                    '    txtCommisionAmt.Text = ""

                    'End If
                End If
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("CostCenterCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(sqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region
#Region "Private Sub DisableControl()"
    Private Sub DisableControl()

        'ddlType.Enabled = False
        txtSupplier.Enabled = False
        'txtChkFromDt.Enabled = False
        'txtChkToDt.Enabled = False
        ''txtInvoiceNo.Enabled = False  ' hide param  on 09/06/2021  
        If ViewState("SupplierInvoiceState") = "View" Then
            'txtInvoiceNo.Enabled = False
            txtDocDate.Enabled = False
            'txtRequestId.Enabled = False
            'txtNarration.Enabled = False
            btnDisplay.Enabled = False
            btnClear.Enabled = False
            If gvUpdateSupplier.Rows.Count > 0 Then
                For Each row In gvUpdateSupplier.Rows
                    Dim txtTotalAmt, txtCostVatAmt, txtVatPerc, txtTaxAmt, txtNonTaxAmt, txtCommissionAmt As TextBox
                    Dim lbtnActualAmt As LinkButton
                    txtNonTaxAmt = CType(row.FindControl("txtNonTaxAmt"), TextBox)
                    txtTaxAmt = CType(row.FindControl("txtTaxAmt"), TextBox)
                    txtVatPerc = CType(row.FindControl("txtVatPerc"), TextBox)
                    txtCostVatAmt = CType(row.FindControl("txtCostVatAmt"), TextBox)
                    txtTotalAmt = CType(row.FindControl("txtTotalAmt"), TextBox)
                    txtCommissionAmt = CType(row.FindControl("txtCommisionAmt"), TextBox)
                    lbtnActualAmt = CType(row.FindControl("lbtnActualAmt"), LinkButton)
                    txtNonTaxAmt.Enabled = False
                    txtTaxAmt.Enabled = False
                    txtVatPerc.Enabled = False
                    txtCostVatAmt.Enabled = False
                    txtTotalAmt.Enabled = False
                    txtCommissionAmt.Enabled = False
                Next
            End If
        Else

            'txtRequestId.Enabled = True
            'txtNarration.Enabled = True
        End If



    End Sub
#End Region
#Region "Protected Sub txtSupplier_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSupplier.TextChanged"
    Protected Sub txtSupplier_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSupplier.TextChanged
        Try
            If txtSupplier.Text.Trim <> "" And txtSupplierCode.Text.Trim <> "" Then
                Dim acctType As String
                'If ddlType.SelectedValue = "Supplier" Then
                'acctType = "S"
                'Else
                '   acctType = "A"
                'End If
                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                strSqlQry = "select v.controlacctcode, m.acctname,v.cur as currcode,u.currname,c.convrate, v.trnno from view_account v " &
                "inner join acctmast m on v.controlacctcode=m.acctcode and v.div_code=m.div_code " &
                "inner join currmast u on v.cur=u.currcode " &
                "left outer join currrates c on v.cur=c.currcode where c.tocurr =(select option_selected from reservation_parameters where param_id=457) " &
                "and v.div_code=@divcode and v.type=@acctType and v.code=@supplierCode"
                mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
                mySqlCmd.CommandType = CommandType.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@acctType", SqlDbType.VarChar, 20)).Value = acctType
                mySqlCmd.Parameters.Add(New SqlParameter("@supplierCode", SqlDbType.VarChar, 20)).Value = txtSupplierCode.Text.Trim
                Dim rs As SqlDataReader
                rs = mySqlCmd.ExecuteReader
                If rs.Read() Then
                    If Not IsDBNull(rs("controlacctcode")) Then
                        txtCtrlAcctCode.Text = Convert.ToString(rs("controlacctcode"))
                    Else
                        txtCtrlAcctCode.Text = ""
                    End If
                    If Not IsDBNull(rs("acctname")) Then
                        txtCtrlAcct.Text = Convert.ToString(rs("acctname"))
                    Else
                        txtCtrlAcct.Text = ""
                    End If
                    If Not IsDBNull(rs("currcode")) Then
                        txtCurrencyCode.Text = Convert.ToString(rs("currcode"))
                    Else
                        txtCurrencyCode.Text = ""
                    End If
                    If Not IsDBNull(rs("currname")) Then
                        txtCurrency.Text = Convert.ToString(rs("currname"))
                    Else
                        txtCurrency.Text = ""
                    End If
                    If Not IsDBNull(rs("convrate")) Then
                        txtConvRate.Text = Convert.ToString(rs("convrate"))
                    Else
                        txtConvRate.Text = ""
                    End If
                    If Not IsDBNull(rs("trnno")) Then
                        txtTrnNo.Text = Convert.ToString(rs("trnno"))
                    Else
                        txtTrnNo.Text = ""
                    End If
                End If
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
            End If
        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function ValidationDisplay() As Boolean"
    Protected Function ValidationDisplay() As Boolean

        If txtSupplierCode.Text.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier can not be empty' );", True)
            txtSupplier.Focus()
            ValidationDisplay = False
            Exit Function
        End If
        'If Not IsDate(txtChkFromDt.Text.Trim) Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From check out date can not be empty' );", True)
        '    txtChkFromDt.Focus()
        '    ValidationDisplay = False
        '    Exit Function
        'End If
        'If Not IsDate(txtChkToDt.Text.Trim) Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To check out date can not be empty' );", True)
        '    txtChkToDt.Focus()
        '    ValidationDisplay = False
        '    Exit Function
        'End If
        ValidationDisplay = True
    End Function
#End Region
#Region "  Protected Sub txtNonTaxAmountPop_TextChanged(ByVal sender As Object, ByVal e As EventArgs)"
    Protected Sub txtNonTaxAmountPop_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        CalculategvNonTaxAmount()
        Dim txtTaxAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtTaxAmt.NamingContainer
        CalculateTaxVatGv(gvr)
        CalculategvTaxAmountTotal()
        CalculategvVatAmount()
        ModalExtraPopup.Show()
    End Sub
#End Region
#Region "Protected Sub CalculateTaxMainGv(ByVal gvr As GridViewRow)"
    Protected Sub CalculateTaxVatMainGv(ByVal gvr As GridViewRow)
        'Dim txtCostPrice As TextBox = CType(gvr.FindControl("txtCostPrice"), TextBox)
        'Dim txtCostValue As TextBox = CType(gvr.FindControl("txtCostValue"), TextBox)
        Dim lbtnActualAmt As LinkButton = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
        Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)

        Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        Dim txtCostVatAmt As TextBox = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
        Dim txtvatpercpop As TextBox = CType(gvr.FindControl("txtvatperc"), TextBox)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtTaxAmt.Text) And IsNumeric(txtvatpercpop.Text) Then
            Dim taxableamount As Decimal
            If txtNonTaxAmt.Text = "" Then
                txtNonTaxAmt.Text = 0
                Dim txtNonTaxAmtBase As TextBox = CType(gvr.FindControl("txtNonTaxAmtBase"), TextBox) 'changed by mohamed on 25/12/2021
                txtNonTaxAmtBase.Text = 0 'changed by mohamed on 25/12/2021
            End If

            taxableamount = Math.Round(Convert.ToDecimal(lbtnActualAmt.Text) - (Convert.ToDecimal(txtNonTaxAmt.Text)), decimalPlaces)
            txtTaxAmt.Text = Math.Round(Convert.ToDecimal((taxableamount) / ((100 + txtvatpercpop.Text) / 100)), decimalPlaces)
            txtCostVatAmt.Text = Convert.ToDecimal(taxableamount) - Convert.ToDecimal(txtTaxAmt.Text)

            'changed by mohamed on 25/12/2021
            Dim txtTaxAmtBase As TextBox = CType(gvr.FindControl("txtTaxAmtBase"), TextBox)
            Dim txtCostVatAmtBase As TextBox = CType(gvr.FindControl("txtCostVatAmtBase"), TextBox)
            txtTaxAmtBase.Text = Math.Round(Val(txtTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
            txtCostVatAmtBase.Text = Math.Round(Val(txtCostVatAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
        End If

    End Sub
#End Region
#Region "Protected Sub CalculateTaxVatGv(ByVal gvr As GridViewRow)"
    Protected Sub CalculateTaxVatGv(ByVal gvr As GridViewRow)
        Dim txtCostPrice As TextBox = CType(gvr.FindControl("txtCostPrice"), TextBox)
        Dim txtCostValue As TextBox = CType(gvr.FindControl("txtCostValue"), TextBox)
        Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmount"), TextBox)
        Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmount"), TextBox)
        Dim txtCostVatAmt As TextBox = CType(gvr.FindControl("txtCostVatAmount"), TextBox)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtTaxAmt.Text) And IsNumeric(txtvatpercpop.Text) Then
            Dim taxableamount As Decimal
            If txtNonTaxAmt.Text = "" Then
                txtNonTaxAmt.Text = 0
                Dim txtNonTaxAmtBase As TextBox = CType(gvr.FindControl("txtNonTaxAmtBase"), TextBox) 'changed by mohamed on 25/12/2021
                txtNonTaxAmtBase.Text = 0 'changed by mohamed on 25/12/2021
            End If

            If hdServiceType.Value.ToUpper = "HOTEL" Then
                taxableamount = Math.Round(Convert.ToDecimal(txtCostPrice.Text) - (Convert.ToDecimal(txtNonTaxAmt.Text)), decimalPlaces)
            Else
                taxableamount = Math.Round(Convert.ToDecimal(txtCostValue.Text) - (Convert.ToDecimal(txtNonTaxAmt.Text)), decimalPlaces)
            End If
            txtTaxAmt.Text = Math.Round(Convert.ToDecimal((taxableamount) / ((100 + txtvatpercpop.Text) / 100)), decimalPlaces)
            txtCostVatAmt.Text = Convert.ToDecimal(taxableamount) - Convert.ToDecimal(txtTaxAmt.Text)

            ''changed by mohamed on 25/12/2021
            'Dim txtTaxAmtBase As TextBox = CType(gvr.FindControl("txtTaxAmtBase"), TextBox)
            'Dim txtCostVatAmtBase As TextBox = CType(gvr.FindControl("txtCostVatAmtBase"), TextBox)
            'txtTaxAmtBase.Text = Math.Round(Val(txtTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
            'txtCostVatAmtBase.Text = Math.Round(Val(txtCostVatAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
        End If

    End Sub
#End Region
#Region "   Private Sub CalculategvTaxAmountTotal()"
    Private Sub CalculategvTaxAmountTotal()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim totalTaxAmt As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtTaxAmount As TextBox = CType(row.FindControl("txtTaxAmount"), TextBox)
            If IsNumeric(txtTaxAmount.Text) Then
                totalTaxAmt = totalTaxAmt + Convert.ToDecimal(txtTaxAmount.Text)
            End If
        Next
        Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
        lblTaxAmountTotal.Text = Math.Round(Convert.ToDecimal(totalTaxAmt), decimalPlaces).ToString()
    End Sub
#End Region
#Region " Protected Sub txtvatpercpop_textchanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtvatpercpop_textchanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CalculateVatAmountGv()
        CalculategvVatAmount()
        CalculategvTaxAmountTotal()
        ModalExtraPopup.Show()
    End Sub
#End Region
    Private Sub CalculateVatAmountToAdjust()
        Dim decimalplaces = Session("decimalplaces")
        If gvPriceList.Rows.Count > 1 Then
            Dim vatamttoadjust As Decimal
            Dim VatDifference = Convert.ToDecimal(hdVatOldValue.Value) - Convert.ToDecimal(txttotalvatactual.Text)

            If Convert.ToDecimal(hdVatOldValue.Value) > Convert.ToDecimal(txttotalvatactual.Text) Then
                vatamttoadjust = Math.Abs(VatDifference / Convert.ToDecimal(hdVatOldValue.Value))
            Else
                vatamttoadjust = Math.Abs(VatDifference / Convert.ToDecimal(txttotalvatactual.Text))
            End If
            If Convert.ToDecimal(txttotalvatactual.Text) <> Convert.ToDecimal(hdVatOldValue.Value) Then

                For i = 0 To gvPriceList.Rows.Count - 1
                    Dim txtCostVatAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostVatAmount"), TextBox)
                    Dim txtTaxAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtTaxAmount"), TextBox)
                    Dim txtCostPrice As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostPrice"), TextBox)
                    Dim txtCostValue As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostValue"), TextBox)
                    Dim txtNonTaxAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtNonTaxAmount"), TextBox)
                    If VatDifference > -1 And VatDifference < 1 And i = 0 Then
                        txtCostVatAmount.Text = Math.Round(Convert.ToDecimal(txtCostVatAmount.Text) + VatDifference, decimalplaces)
                        Exit For
                    Else

                        If Convert.ToDecimal(hdVatOldValue.Value) > Convert.ToDecimal(txttotalvatactual.Text) Then
                            ' txtCostVatAmount.Text = Math.Round(txtCostVatAmount.Text * IIf(vatamttoadjust < 0, (1 - vatamttoadjust), (1 + vatamttoadjust)), decimalplaces)

                            txtCostVatAmount.Text = Math.Round(txtCostVatAmount.Text * (1 - vatamttoadjust), decimalplaces)
                            'datarow.Item("CostVatValue") = Math.Round(datarow.Item("CostVatValue") * IIf(vatamttoadjust < 0, (1 - vatamttoadjust), (1 + vatamttoadjust)), decimalplaces)
                        Else
                            txtCostVatAmount.Text = Math.Round(txtCostVatAmount.Text * (1 + vatamttoadjust), decimalplaces)
                            'DataRow.Item("CostVatValue") = Math.Round(DataRow.Item("CostVatValue") * (1 + vatamttoadjust), decimalplaces)
                        End If

                    End If
                Next
                CalculategvVatAmount()
                Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
                Dim Diffvatactual = Math.Abs(Math.Round((Convert.ToDecimal(txttotalvatactual.Text) - Convert.ToDecimal(lblCostVatAmountTotal.Text)), decimalplaces))

                If Diffvatactual > -1 And Diffvatactual < 1 Then
                    Dim txtCostVatAmount As TextBox = CType(gvPriceList.Rows(0).FindControl("txtCostVatAmount"), TextBox)

                    If Convert.ToDecimal(lblCostVatAmountTotal.Text) > Convert.ToDecimal(txttotalvatactual.Text) Then
                        txtCostVatAmount.Text = Math.Round(Convert.ToDecimal((txtCostVatAmount.Text) - Diffvatactual), decimalplaces)
                    Else
                        txtCostVatAmount.Text = Math.Round(Convert.ToDecimal((txtCostVatAmount.Text) + Diffvatactual), decimalplaces)
                    End If

                Else
                    Dim NewVatValueLeft As Decimal = Diffvatactual / gvPriceList.Rows.Count

                    For i = 0 To gvPriceList.Rows.Count - 1
                        If Convert.ToDecimal(lblCostVatAmountTotal.Text) <> Convert.ToDecimal(txttotalvatactual.Text) Then
                            Dim txtCostVatAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostVatAmount"), TextBox)
                            If Diffvatactual > 1 Then
                                ' DataRow.Item("CostVatValue") = Math.Round(DataRow.Item("CostVatValue") + Convert.ToDecimal(NewVatValueLeft), decimalplaces)
                                txtCostVatAmount.Text = Math.Round(Convert.ToDecimal(txtCostVatAmount.Text) + Convert.ToDecimal(NewVatValueLeft), decimalplaces)

                            End If

                        End If
                    Next

                    CalculategvVatAmount()
                    Dim NewVatValuTotal = Math.Round(Convert.ToDecimal(lblCostVatAmountTotal.Text), decimalplaces)
                    Dim AdjustFirstRowValue = Convert.ToDecimal(NewVatValuTotal - Math.Round(Convert.ToDecimal(txttotalvatactual.Text)))
                    If (NewVatValuTotal <> Math.Round(Convert.ToDecimal(txttotalvatactual.Text))) Then
                        Dim txtCostVatAmount As TextBox = CType(gvPriceList.Rows(0).FindControl("txtCostVatAmount"), TextBox)

                        If AdjustFirstRowValue < 0 Then

                            txtCostVatAmount.Text = Math.Round(Convert.ToDecimal((txtCostVatAmount.Text) - AdjustFirstRowValue), decimalplaces)
                        Else
                            txtCostVatAmount.Text = Math.Round(Convert.ToDecimal((txtCostVatAmount.Text) + AdjustFirstRowValue), decimalplaces)

                        End If
                    End If
                End If
                For i = 0 To gvPriceList.Rows.Count - 1
                    Dim txtCostVatAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostVatAmount"), TextBox)
                    Dim txtTaxAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtTaxAmount"), TextBox)
                    Dim txtCostPrice As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostPrice"), TextBox)
                    Dim txtCostValue As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostValue"), TextBox)
                    Dim txtNonTaxAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtNonTaxAmount"), TextBox)
                    'If Diffvatactual < 1 Then
                    'End If
                    If hdServiceType.Value.ToLower = "hotel" Then
                        txtTaxAmount.Text = Math.Round(Convert.ToDecimal(txtCostPrice.Text) - (Convert.ToDecimal(txtNonTaxAmount.Text) + Convert.ToDecimal(txtCostVatAmount.Text)), decimalplaces)
                    Else   'hdServiceType.Value.ToLower = "specialevent" Then
                        txtTaxAmount.Text = Math.Round(Convert.ToDecimal(txtCostValue.Text) - (Convert.ToDecimal(txtNonTaxAmount.Text) + Convert.ToDecimal(txtCostVatAmount.Text)), decimalplaces)
                    End If
                Next

            End If
        Else
            Dim txtCostVatAmount As TextBox = CType(gvPriceList.Rows(0).FindControl("txtCostVatAmount"), TextBox)
            Dim txtTaxAmount As TextBox = CType(gvPriceList.Rows(0).FindControl("txtTaxAmount"), TextBox)
            Dim txtCostPrice As TextBox = CType(gvPriceList.Rows(0).FindControl("txtCostPrice"), TextBox)
            Dim txtCostValue As TextBox = CType(gvPriceList.Rows(0).FindControl("txtCostValue"), TextBox)
            Dim txtNonTaxAmount As TextBox = CType(gvPriceList.Rows(0).FindControl("txtNonTaxAmount"), TextBox)
            txtCostVatAmount.Text = Convert.ToDecimal(txttotalvatactual.Text)
            If hdServiceType.Value.ToLower = "hotel" Then

                txtTaxAmount.Text = Math.Round(Convert.ToDecimal(txtCostPrice.Text) - (Convert.ToDecimal(txtNonTaxAmount.Text) + Convert.ToDecimal(txtCostVatAmount.Text)), decimalplaces)

            Else 'If hdServiceType.Value.ToLower = "specialevent" Then

                txtTaxAmount.Text = Math.Round(Convert.ToDecimal(txtCostValue.Text) - (Convert.ToDecimal(txtNonTaxAmount.Text) + Convert.ToDecimal(txtCostVatAmount.Text)), decimalplaces)
            End If

        End If
        CalculategvTaxAmountTotal()
        CalculategvVatAmount()
        hdVatOldValue.Value = txttotalvatactual.Text
    End Sub


#Region " Protected Sub txttotalvatactual_textchanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txttotalvatactual_textchanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' hdVatOldValue.Value = txttotalvatactual.Text
        CalculateVatAmountToAdjust()
        'AdjustVatAmount(hdServiceType.Value, filterPriceDt, decimalPlaces)
        'gvPriceList.DataSource = filterPriceDt
        'gvPriceList.DataBind()
        'CalculateVatAmountGv()
        'CalculategvVatAmount()
        ModalExtraPopup.Show()
    End Sub
#End Region


#Region "  Protected Sub CalculateVatAmountGv()"
    Protected Sub CalculateVatAmountGv()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If Not IsNumeric(txtvatpercpop.Text) Then
            txtvatpercpop.Text = 0
        End If
        For Each row In gvPriceList.Rows
            Dim txtTaxAmt As TextBox = CType(row.FindControl("txtTaxAmount"), TextBox)
            Dim txtCostVatAmount As TextBox = CType(row.FindControl("txtCostVatAmount"), TextBox)
            If Not IsNumeric(txtTaxAmt.Text) Then
                txtTaxAmt.Text = 0
            End If
            If IsNumeric(txtTaxAmt.Text) And IsNumeric(txtvatpercpop.Text) Then
                txtCostVatAmount.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text) * Convert.ToDecimal(txtvatpercpop.Text) / 100, decimalPlaces).ToString()
            End If
            CalculategvTaxAmount(row)
        Next

    End Sub
#End Region
    Private Sub CalculateVatAmount()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim totalCostVatAmt As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtCostVatAmount As TextBox = CType(row.FindControl("txtCostVatAmount"), TextBox)
            If IsNumeric(txtCostVatAmount.Text) Then
                totalCostVatAmt = totalCostVatAmt + Convert.ToDecimal(txtCostVatAmount.Text)
            End If
        Next
        Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
        lblCostVatAmountTotal.Text = Math.Round(Convert.ToDecimal(totalCostVatAmt), decimalPlaces).ToString()
    End Sub
#Region " Private Sub CalculategvCommissionAmount()"
    Private Sub CalculategvCommissionAmount()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim totalCommissionAmt As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtCommissionAmount As TextBox = CType(row.FindControl("txtCommissionAmount"), TextBox)
            If IsNumeric(txtCommissionAmount.Text) Then
                totalCommissionAmt = totalCommissionAmt + Convert.ToDecimal(txtCommissionAmount.Text)
            End If
        Next
        Dim lblCommissionAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCommissionAmountTotal"), Label)
        lblCommissionAmountTotal.Text = Math.Round(Convert.ToDecimal(totalCommissionAmt), decimalPlaces).ToString()
    End Sub
#End Region
#Region " Private Sub CalculategvVatAmount()"
    Private Sub CalculategvVatAmount()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim totalCostVatAmt As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtCostVatAmount As TextBox = CType(row.FindControl("txtCostVatAmount"), TextBox)
            If IsNumeric(txtCostVatAmount.Text) Then
                totalCostVatAmt = totalCostVatAmt + Convert.ToDecimal(txtCostVatAmount.Text)
            End If
        Next
        Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
        lblCostVatAmountTotal.Text = Math.Round(Convert.ToDecimal(totalCostVatAmt), decimalPlaces).ToString()
    End Sub
#End Region
#Region " Protected Sub txtTaxAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtTaxAmountPop_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtTaxAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtTaxAmt.NamingContainer
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtTaxAmt.Text) Then
            txtTaxAmt.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text), decimalPlaces).ToString()
        End If
        CalculateTaxGv(gvr)
        CalculategvTaxAmountTotal()
        CalculategvVatAmount()
        CalculategvNonTaxAmount()
        ModalExtraPopup.Show()
    End Sub
#End Region
#Region "  Protected Sub CalculatemaingvTaxAmount(ByVal gvr As GridViewRow)"
    Protected Sub CalculatemaingvTaxAmount(ByVal gvr As GridViewRow)
        Dim txtCostVatAmount As TextBox = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
        Dim lbtnActualAmt As LinkButton = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
        Dim txtTaxAmount As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        Dim txtNonTaxAmount As TextBox = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
        If txtCostVatAmount.Text = "" Then txtCostVatAmount.Text = 0
        txtTaxAmount.Text = Convert.ToDecimal(lbtnActualAmt.Text) - (Convert.ToDecimal(txtCostVatAmount.Text) + Convert.ToDecimal(txtNonTaxAmount.Text))
    End Sub
#End Region
#Region "  Protected Sub CalculategvTaxAmount(ByVal gvr As GridViewRow)"
    Protected Sub CalculategvTaxAmount(ByVal gvr As GridViewRow)
        Dim txtCostVatAmount As TextBox = CType(gvr.FindControl("txtCostVatAmount"), TextBox)
        Dim txtCostPrice As TextBox = CType(gvr.FindControl("txtCostPrice"), TextBox)
        Dim txtCostValue As TextBox = CType(gvr.FindControl("txtCostValue"), TextBox)
        Dim txtTaxAmount As TextBox = CType(gvr.FindControl("txtTaxAmount"), TextBox)
        Dim txtNonTaxAmount As TextBox = CType(gvr.FindControl("txtNonTaxAmount"), TextBox)

        If Not IsNumeric(txtCostVatAmount.Text) Then
            txtCostVatAmount.Text = 0
        End If
        If Not IsNumeric(txtNonTaxAmount.Text) Then
            txtNonTaxAmount.Text = 0
        End If
        If Not IsNumeric(txtCostValue.Text) Then
            txtCostValue.Text = 0
        End If
        If hdServiceType.Value.ToUpper = "HOTEL" Then
            txtTaxAmount.Text = Convert.ToDecimal(txtCostPrice.Text) - (Convert.ToDecimal(txtCostVatAmount.Text) + Convert.ToDecimal(txtNonTaxAmount.Text))
        Else 'If hdServiceType.Value.ToUpper = "SPECIALEVENT" Or hdServiceType.Value.ToUpper = "TRANSFERS" Then
            txtTaxAmount.Text = Convert.ToDecimal(txtCostValue.Text) - (Convert.ToDecimal(txtCostVatAmount.Text) + Convert.ToDecimal(txtNonTaxAmount.Text))
        End If


    End Sub
#End Region
#Region " Protected Sub txtCostVatAmountPop_TextChanged(ByVal sender As Object, ByVal e As EventArgs)"
    Protected Sub txtCostVatAmountPop_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtTaxAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtTaxAmt.NamingContainer
        CalculategvVatAmount()
        CalculategvTaxAmount(gvr)
        CalculategvTaxAmountTotal()
        ModalExtraPopup.Show()
    End Sub
#End Region
#Region " Protected Sub txtCommissionAmountPop_TextChanged(ByVal sender As Object, ByVal e As EventArgs)"
    Protected Sub txtCommissionAmountPop_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtCommissionAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtCommissionAmt.NamingContainer
        CalculategvVatAmount()
        CalculategvTaxAmount(gvr)
        CalculategvTaxAmountTotal()
        ModalExtraPopup.Show()
    End Sub
#End Region
#Region "Protected Sub CalculateTaxMainGv(ByVal gvr As GridViewRow)"
    Protected Sub CalculateTaxMainGv(ByVal gvr As GridViewRow)
        Dim lbtnActualAmt As LinkButton = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
        Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
        Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        Dim txtCostVatAmt As TextBox = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
        Dim txtvatpercpop As TextBox = CType(gvr.FindControl("txtVatPerc"), TextBox)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtTaxAmt.Text) And IsNumeric(txtvatpercpop.Text) Then
            txtCostVatAmt.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text) * Convert.ToDecimal(txtvatpercpop.Text) / 100, decimalPlaces).ToString()

            txtNonTaxAmt.Text = Math.Round(Convert.ToDecimal(lbtnActualAmt.Text) - (Convert.ToDecimal(txtTaxAmt.Text) + Convert.ToDecimal(txtCostVatAmt.Text)), decimalPlaces)

            Dim txtNonTaxAmtBase As TextBox = CType(gvr.FindControl("txtNonTaxAmtBase"), TextBox) 'changed by mohamed on 25/12/2021
            Dim txtTaxAmtBase As TextBox = CType(gvr.FindControl("txtTaxAmtBase"), TextBox) 'changed by mohamed on 25/12/2021
            Dim txtCostVatAmtBase As TextBox = CType(gvr.FindControl("txtCostVatAmtBase"), TextBox) 'changed by mohamed on 25/12/2021
            txtNonTaxAmtBase.Text = Math.Round((Convert.ToDecimal(lbtnActualAmt.Text) * Convert.ToDecimal(txtConvRate.Text)) - (Convert.ToDecimal(txtTaxAmtBase.Text) + Convert.ToDecimal(txtCostVatAmtBase.Text)), decimalPlaces) 'changed by mohamed on 25/12/2021

        End If
    End Sub
#End Region
#Region "Protected Sub CalculateTaxGv(ByVal gvr As GridViewRow)"
    Protected Sub CalculateTaxGv(ByVal gvr As GridViewRow)
        Dim txtCostPrice As TextBox = CType(gvr.FindControl("txtCostPrice"), TextBox)
        Dim txtCostValue As TextBox = CType(gvr.FindControl("txtCostValue"), TextBox)
        Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmount"), TextBox)
        Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmount"), TextBox)
        Dim txtCostVatAmt As TextBox = CType(gvr.FindControl("txtCostVatAmount"), TextBox)
        Dim decimalPlaces As Integer = Session("decimalPlaces")

        If Not IsNumeric(txtTaxAmt.Text) Then
            txtTaxAmt.Text = 0
        End If
        If Not IsNumeric(txtCostVatAmt.Text) Then
            txtCostVatAmt.Text = 0
        End If
        If Not IsNumeric(txtCostPrice.Text) Then
            txtCostPrice.Text = 0
        End If

        If IsNumeric(txtTaxAmt.Text) And IsNumeric(txtvatpercpop.Text) Then
            txtCostVatAmt.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text) * Convert.ToDecimal(txtvatpercpop.Text) / 100, decimalPlaces).ToString()
            If hdServiceType.Value.ToUpper = "HOTEL" Then
                txtNonTaxAmt.Text = Math.Round(Convert.ToDecimal(txtCostPrice.Text) - (Convert.ToDecimal(txtTaxAmt.Text) + Convert.ToDecimal(txtCostVatAmt.Text)), decimalPlaces)
            Else
                txtNonTaxAmt.Text = Math.Round(Convert.ToDecimal(txtCostValue.Text) - (Convert.ToDecimal(txtTaxAmt.Text) + Convert.ToDecimal(txtCostVatAmt.Text)), decimalPlaces)
            End If
            ''changed by mohamed on 25/12/2021
            'Dim txtTaxAmtBase As TextBox = CType(gvr.FindControl("txtTaxAmtBase"), TextBox)
            'Dim txtNonTaxAmtBase As TextBox = CType(gvr.FindControl("txtNonTaxAmtBase"), TextBox)
            'Dim txtCostVatAmtBase As TextBox = CType(gvr.FindControl("txtCostVatAmtBase"), TextBox)
            'txtNonTaxAmtBase.Text = Math.Round(Val(txtNonTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
            'txtTaxAmtBase.Text = Math.Round(Val(txtTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
            'txtCostVatAmtBase.Text = Math.Round(Val(txtCostVatAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)

        End If
    End Sub
#End Region

#Region "Private Sub CalculategvNonTaxAmount()"
    Private Sub CalculategvNonTaxAmount()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim totalNonTaxAmt As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtNonTaxAmount As TextBox = CType(row.FindControl("txtNonTaxAmount"), TextBox)
            If IsNumeric(txtNonTaxAmount.Text) Then
                totalNonTaxAmt = totalNonTaxAmt + Convert.ToDecimal(txtNonTaxAmount.Text)
            End If
        Next
        Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
        lblNonTaxAmountTotal.Text = Math.Round(Convert.ToDecimal(totalNonTaxAmt), decimalPlaces).ToString()
    End Sub
#End Region

#Region "Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click"
    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
            'Dim pdt As DataTable = CType(Session("priceListDt"), DataTable)
            'pdt.Clear()

            'Session("priceListDt") = pdt

            Dim decimalplaces As Integer = DirectCast(Session("decimalplaces"), Integer)

            If ValidationDisplay() = False Then Exit Sub
            Dim acctType As String
            'If ddlType.SelectedValue = "Supplier" Then
            '    acctType = "S"
            'Else
            '    acctType = "A"
            'End If

            '''''''''''''''''
            If Me.txtCtrlAcct.Text = "" Then
                Me.txtCtrlAcctCode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select controlacctcode from partymast where partycode='" & txtSupplierCode.Text.Trim & "'")
                Me.txtCtrlAcct.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctcode='" & txtCtrlAcctCode.Text & "'")
            End If

            If Me.txtCurrencyCode.Text = "" Then
                Me.txtCurrencyCode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode from partymast where partycode='" & txtSupplierCode.Text.Trim & "'")
                Me.txtCurrency.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currname from currmast where currcode='" & txtCurrencyCode.Text & "'")
                Me.txtConvRate.Text = "1"
            End If

            '''''''''''''''''

            Dim dtExcel As New DataTable()
            dtExcel = CType(Session("sexceldt"), DataTable)

            Dim requestid As String = ""

            For Each row In dtExcel.Rows
                Dim referenceNum As String = ""
                If dtExcel.Columns.Contains("Bkg Reference Number") Then
                    referenceNum = row("Bkg Reference Number")
                Else
                    referenceNum = row("BkgRef")
                End If

                If referenceNum <> "" Then
                    requestid += referenceNum.Trim().ToString() + ","
                End If
                ''If String.IsNullOrEmpty(row("Invoice Date").ToString()) Then
                ''    rowsToRemove.Add(row)
                ''    dtExcel.

                ''End If
            Next


            ' Rename the columns
            If dtExcel.Columns.Contains("Inv No") Then
                dtExcel.Columns("Inv No").ColumnName = "Invno"
            End If
            If dtExcel.Columns.Contains("Invoice Date") Then
                dtExcel.Columns("Invoice Date").ColumnName = "InvDate"
            End If
            If dtExcel.Columns.Contains("Guest Name") Then
                dtExcel.Columns("Guest Name").ColumnName = "GuestName"
            End If
            If dtExcel.Columns.Contains("Bkg Reference Number") Then
                dtExcel.Columns("Bkg Reference Number").ColumnName = "BkgRef"
            End If
            If dtExcel.Columns.Contains("Amount Excluding VAT") Then
                dtExcel.Columns("Amount Excluding VAT").ColumnName = "ExculdeAmount"
            End If
            If dtExcel.Columns.Contains("VAT 5%") Then
                dtExcel.Columns("VAT 5%").ColumnName = "VAT"
            End If
            If dtExcel.Columns.Contains("Amount Inclusive of VAT") Then
                dtExcel.Columns("Amount Inclusive of VAT").ColumnName = "IncludeAmount"
            End If

            If dtExcel.Columns.Contains("Booking element ID") Then
                dtExcel.Columns("Booking element ID").ColumnName = "BookingElementID"
            End If

            If dtExcel.Columns.Contains("Total Inv.Amount (USD)") Then
                dtExcel.Columns("Total Inv.Amount (USD)").ColumnName = "IncludeAmount"
            End If

            dtExcel.TableName = "ExcelData"
            Dim ExcelData As String = objUtils.GenerateXML(dtExcel)



            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            ' mySqlCmd = New SqlCommand("sp_get_supplier_provision_excelUpload", sqlConn)
            mySqlCmd = New SqlCommand("sp_get_supplier_provision_excelUploadNew", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = IIf(txtDivcode.Text.Trim = "", "01", txtDivcode.Text.Trim()) 'txtDivcode.Text.Trim Tanvir 27122023
            mySqlCmd.Parameters.Add(New SqlParameter("@supplierCode", SqlDbType.VarChar, 20)).Value = txtSupplierCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 8000)).Value = requestid
            mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno", SqlDbType.VarChar, 20)).Value = ""
            mySqlCmd.Parameters.Add(New SqlParameter("@ExcelData", SqlDbType.Xml)).Value = ExcelData
            myDataAdapter = New SqlDataAdapter(mySqlCmd)
            myDataAdapter.SelectCommand.CommandTimeout = 0  'Rosalin 03/10/2023
            Dim dataSet As New DataSet
            myDataAdapter.Fill(dataSet)

            If dataSet IsNot Nothing Then
                If dataSet.Tables.Count > 0 Then
                    Dim dtData As DataTable = dataSet.Tables(0)
                    Dim dtErr As DataTable = dataSet.Tables(1)

                    Dim dtgvErr As DataTable = dataSet.Tables(1).Clone()

                    If dtErr IsNot Nothing Then
                        If dtErr.Rows.Count > 0 Then

                            gvError.DataSource = dtErr
                            gvError.DataBind()
                            Errtr1.Visible = True
                            Errtr2.Visible = True

                            'GoTo ErrGoto
                        End If
                    End If

                    ' Validation checking yes or no

                    Dim validationNeedOrNot As Integer = 0
                    validationNeedOrNot = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters  where param_id ='5755' ")




                    If dtData IsNot Nothing Then
                        If dtData.Rows.Count > 0 Then

                            Dim wSheetName As String = ViewState("WorkSheetName").ToString()

                            Dim dcInvNo As DataColumn = New DataColumn("InvNo", GetType(String))
                            dtData.Columns.Add(dcInvNo)

                            Dim dc2 As DataColumn = New DataColumn("Commission", GetType(Decimal))
                            dtData.Columns.Add(dc2)

                            Dim dc As DataColumn = New DataColumn("Selection", GetType(Boolean))
                            dc.DefaultValue = False
                            dtData.Columns.Add(dc)
                            'Dim dc1 As DataColumn = New DataColumn("totalprice", GetType(Decimal))
                            'dtData.Columns.Add(dc1)



                            Dim rowId As Integer = 1
                            For Each row In dtData.Rows
                                Dim rId As String = ""
                                Dim Actualamt As Decimal = 0, CostNonTaxable As Decimal = 0, CostTaxable As Decimal = 0, CostVAT As Decimal = 0
                                rId = row("requestid").ToString()
                                '  Actualamt = Math.Round(Convert.ToDecimal(row("actualamount")), decimalplaces)

                                If row("actualamount") IsNot DBNull.Value Then
                                    Actualamt = Math.Round(Convert.ToDecimal(row("actualamount")), decimalplaces)
                                Else

                                    Actualamt = 0
                                End If
                                '  CostNonTaxable = Math.Round(Convert.ToDecimal(row("prices_costnontaxablevaluebase")), decimalplaces)
                                If row("prices_costnontaxablevaluebase") IsNot DBNull.Value Then
                                    CostNonTaxable = Math.Round(Convert.ToDecimal(row("prices_costnontaxablevaluebase")), decimalplaces)
                                Else

                                    CostNonTaxable = 0
                                End If

                                If row("prices_costtaxablevaluebase") IsNot DBNull.Value Then
                                    CostTaxable = Math.Round(Convert.ToDecimal(row("prices_costtaxablevaluebase")), decimalplaces)
                                Else

                                    CostTaxable = 0
                                End If

                                If row("prices_costvatvaluebase") IsNot DBNull.Value Then
                                    CostVAT = Math.Round(Convert.ToDecimal(row("prices_costvatvaluebase")), decimalplaces)
                                Else

                                    CostTaxable = 0
                                End If

                                ' CostTaxable = IIf(Not IsDBNull(row("prices_costtaxablevaluebase")), Math.Round(Convert.ToDecimal(row("prices_costtaxablevaluebase")), decimalplaces), 0) ' Math.Round(Convert.ToDecimal(row("prices_costtaxablevaluebase")), decimalplaces)
                                ' CostVAT = IIf(Not IsDBNull(row("prices_costvatvaluebase")), Math.Round(Convert.ToDecimal(row("prices_costvatvaluebase")), decimalplaces), 0)  'Math.Round(Convert.ToDecimal(row("prices_costvatvaluebase")), decimalplaces)

                                'Actualamt = Math.Round(row("totalactualamount"), decimalplaces)
                                ' 'CostNonTaxable = Math.Round(row("prices_costnontaxablevaluebase"), decimalplaces)
                                'CostTaxable = Math.Round(row("totalprovisionamount"), decimalplaces)
                                'CostVAT = Math.Round(row("totalvatprovision"), decimalplaces)

                                For Each excelRow In dtExcel.Rows
                                    Dim rIdExcel As String = ""
                                    Dim ActualamtEx As Decimal = 0, CostNonTaxableEx As Decimal = 0, CostTaxableEx As Decimal = 0, CostVATEx As Decimal = 0
                                    Dim Commission As Decimal = 0
                                    rIdExcel = excelRow("BkgRef")   '"Bkg Reference Number"

                                    If wSheetName.ToString() = "Hotels without Comm." Or wSheetName.ToString() = "Transfer & Excursions" Then

                                        ActualamtEx = Math.Round(Convert.ToDecimal(excelRow("IncludeAmount")), decimalplaces)
                                        CostTaxableEx = Math.Round(Convert.ToDecimal(excelRow("ExculdeAmount")), decimalplaces)
                                        CostVATEx = Math.Round(Convert.ToDecimal(excelRow("VAT")), decimalplaces)

                                    ElseIf wSheetName.ToString() = "Hotels with Comm." Then

                                        ActualamtEx = Math.Round(Convert.ToDecimal(excelRow("IncludeAmount")), decimalplaces)
                                        CostTaxableEx = Math.Round(Convert.ToDecimal(excelRow("ExculdeAmount")), decimalplaces)
                                        CostVATEx = Math.Round(Convert.ToDecimal(excelRow("VAT")), decimalplaces)
                                        '  Commission = Math.Round(Convert.ToDecimal(excelRow("Commission")), decimalplaces)

                                    ElseIf wSheetName.ToString() = "Outbound Hotels" Then

                                        ' ActualamtEx = Math.Round(Convert.ToDecimal(excelRow("Total Inv.Amount (USD)")), decimalplaces)
                                        ActualamtEx = Math.Round(Convert.ToDecimal(excelRow("IncludeAmount")), decimalplaces)

                                    End If

                                    If rId = rIdExcel Then

                                        row("InvNo") = excelRow("InvNo")
                                        row("Commission") = Commission


                                        ''''
                                        'If validationNeedOrNot = 1 Then  'Validation  If
                                        '    If Actualamt <> ActualamtEx Then
                                        '        dtgvErr.Rows.Add(rowId, "Amount Inclusive of VAT (" & ActualamtEx & ") not match with this booking reference number", rIdExcel)
                                        '        rowId += 1
                                        '    End If

                                        '    'If Math.Round((CostTaxable + CostNonTaxable), decimalplaces) <> CostTaxableEx And wSheetName.ToString() <> "Outbound Hotels" Then
                                        '    '    dtgvErr.Rows.Add(rowId, "Amount Excluding VAT (" & CostTaxableEx & ") not match with this booking reference number", rIdExcel)
                                        '    '    rowId += 1
                                        '    'End If

                                        '    If CostTaxable <> CostTaxableEx And wSheetName.ToString() <> "Outbound Hotels" Then
                                        '        dtgvErr.Rows.Add(rowId, "Amount Excluding VAT (" & CostTaxableEx & ") not match with this booking reference number", rIdExcel)
                                        '        rowId += 1
                                        '    End If

                                        '    If CostVAT <> CostVATEx And wSheetName.ToString() <> "Outbound Hotels" Then
                                        '        dtgvErr.Rows.Add(rowId, "VAT 5% (" & CostVATEx & ") not match with this booking reference number", rIdExcel)
                                        '        rowId += 1
                                        '    End If

                                        'End If
                                        ''''


                                    End If
                                Next
                            Next



                            If dtgvErr.Rows.Count > 0 Then
                                gvError.DataSource = dtgvErr
                                gvError.DataBind()
                                Errtr1.Visible = True
                                Errtr2.Visible = True

                                ' GoTo ErrGoto
                            End If
                        End If ' Validation Required or not

                    End If


                    'Dim dc As DataColumn = New DataColumn("Selection", GetType(Boolean))
                    'dc.DefaultValue = False
                    'dtData.Columns.Add(dc)
                    'Dim dc1 As DataColumn = New DataColumn("totalprice", GetType(Decimal))
                    'dtData.Columns.Add(dc1)

                    'Dim dc2 As DataColumn = New DataColumn("Commission", GetType(Decimal))
                    'dtData.Columns.Add(dc2)
                    'If ViewState("SupplierInvoiceState") = "Edit" Then
                    '    Dim ExistingRecdt As DataTable = FillGvUpdateEdit(txtDocNo.Text)
                    '    dtData.Merge(ExistingRecdt)
                    'End If

                    If dtData.Rows.Count > 0 Then
                        Session("dtDisplayData") = dtData.Copy()
                        gvUpdateSupplier.DataSource = dtData
                        gvUpdateSupplier.DataBind()
                        Dim ShowGuestDetails As Integer
                        ShowGuestDetails = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters  where param_id ='5312' ")
                        If ShowGuestDetails = 1 Then
                            'gvUpdateSupplier.Columns(5).Visible = True
                            gvUpdateSupplier.Columns(6).Visible = True
                        Else
                            'gvUpdateSupplier.Columns(5).Visible = False
                            gvUpdateSupplier.Columns(6).Visible = False
                        End If
                        'Dim lblTotalVatAmt As Label = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatAmt"), Label)
                        'hdCurrenttotalvatactual.Value = lblTotalVatAmt.Text
                        lblMsg.Visible = False

                        txtSupplier.Enabled = False 'changed by mohamed on 08/11/2021
                        'ddlType.Enabled = False
                    Else
                        lblMsg.Visible = True
                    End If


                End If
            End If




ErrGoto:
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
            calculateNetTotal()

        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    ' Set excel value in the grid.
    Private Sub setExcelValue(ByVal ActualamtEx As Decimal, ByVal CostNonTaxableEx As Decimal, ByVal CostTaxableEx As Decimal, ByVal CostVATEx As Decimal)

        Try
            Dim Actualamt As Decimal '= Math.Round(row("totalactualamount"), decimalplaces)
            Dim CostNonTaxable As Decimal '= Math.Round(row("prices_costnontaxablevaluebase"), decimalplaces)
            Dim CostTaxable As Decimal '= Math.Round(row("totalprovisionamount"), decimalplaces)
            Dim CostVAT As Decimal '= Math.Round(row("totalvatprovision"), decimalplaces)

            ' Dim ActualamtEx As Decimal = 0, CostNonTaxableEx As Decimal = 0, CostTaxableEx As Decimal = 0, CostVATEx As Decimal = 0
            '  Dim Commission As Decimal = 0

            If hdServiceType.Value.Trim().ToLower() = "hotel" Then
                Dim decimalPlaces As Integer = Session("decimalPlaces")

                If Val(ActualamtEx) > 0 Then
                    Dim dt As DataTable
                    dt = calculateVat(hdPriceRequestId.Value.Trim(), Val(hdPriceRlineNo.Value), txtSupplierCode.Text.Trim(), hdServiceType.Value.Trim(),
                                 Convert.ToDecimal(Val(txtvatpercpop.Text)), Convert.ToDecimal(Val(ActualamtEx)))
                    If dt.Rows.Count > 0 Then
                        CostNonTaxable = Math.Round(dt.Rows(0)("nonTaxablevalue"), decimalPlaces)
                        CostTaxable = Math.Round(dt.Rows(0)("taxableValue"), decimalPlaces)
                        CostVAT = Math.Round(dt.Rows(0)("vatValue"), decimalPlaces)
                        Actualamt = Math.Round((CostNonTaxableEx + CostTaxableEx + CostVATEx), decimalPlaces)

                    Else
                        CostNonTaxable = 0
                        CostTaxable = 0
                        CostVAT = 0
                        Actualamt = 0
                    End If
                Else
                    CostNonTaxable = 0
                    CostTaxable = 0
                    CostVAT = 0
                    Actualamt = 0
                End If
                ' calculategvPriceTotal_Ex()
                ' Else
                ' CalculategvPricecostprice()
            End If



            'If hdServiceType.Value.Trim().ToLower() = "hotel" Then
            '    Dim decimalPlaces As Integer = Session("decimalPlaces")
            '    Dim txtcostprice As TextBox = CType(sender, TextBox)
            '    Dim gvr As GridViewRow = txtcostprice.NamingContainer
            '    Dim txtNonTaxAmount As TextBox = CType(gvr.FindControl("txtNonTaxAmount"), TextBox)
            '    Dim txtTaxAmount As TextBox = CType(gvr.FindControl("txtTaxAmount"), TextBox)
            '    Dim txtCostVatAmount As TextBox = CType(gvr.FindControl("txtCostVatAmount"), TextBox)

            '    If Val(txtcostprice.Text) > 0 Then
            '        Dim dt As DataTable
            '        dt = calculateVat(hdPriceRequestId.Value.Trim(), Val(hdPriceRlineNo.Value), txtSupplierCode.Text.Trim(), hdServiceType.Value.Trim(),
            '                     Convert.ToDecimal(Val(txtvatpercpop.Text)), Convert.ToDecimal(Val(txtcostprice.Text)))
            '        If dt.Rows.Count > 0 Then
            '            txtNonTaxAmount.Text = Math.Round(dt.Rows(0)("nonTaxablevalue"), decimalPlaces)
            '            txtTaxAmount.Text = Math.Round(dt.Rows(0)("taxableValue"), decimalPlaces)
            '            txtCostVatAmount.Text = Math.Round(dt.Rows(0)("vatValue"), decimalPlaces)
            '        Else
            '            txtNonTaxAmount.Text = 0
            '            txtTaxAmount.Text = 0
            '            txtCostVatAmount.Text = 0
            '        End If
            '    Else
            '        txtNonTaxAmount.Text = 0
            '        txtTaxAmount.Text = 0
            '        txtCostVatAmount.Text = 0
            '    End If
            '    calculategvPriceTotal()
            'Else
            '    CalculategvPricecostprice()
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoiceExcel.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub calculategvPriceTotal_EX()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim total As Decimal = 0.0
        Dim totalCostValue As Decimal = 0.0
        Dim totalTaxAmt As Decimal = 0.0
        Dim totalNonTaxAmt As Decimal = 0.0
        Dim totalCostVatAmt As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtcostprice As TextBox = CType(row.FindControl("txtcostprice"), TextBox)
            If IsNumeric(txtcostprice.Text) Then
                total = total + Convert.ToDecimal(txtcostprice.Text)
            End If
            Dim txtCostValue As TextBox = CType(row.FindControl("txtCostValue"), TextBox)
            If IsNumeric(txtCostValue.Text) Then
                totalCostValue = totalCostValue + Convert.ToDecimal(txtCostValue.Text)
            End If
            Dim txtNonTaxAmount As TextBox = CType(row.FindControl("txtNonTaxAmount"), TextBox)
            If IsNumeric(txtNonTaxAmount.Text) Then
                totalNonTaxAmt = totalNonTaxAmt + Convert.ToDecimal(txtNonTaxAmount.Text)
            End If
            Dim txtTaxAmount As TextBox = CType(row.FindControl("txtTaxAmount"), TextBox)
            If IsNumeric(txtTaxAmount.Text) Then
                totalTaxAmt = totalTaxAmt + Convert.ToDecimal(txtTaxAmount.Text)
            End If
            Dim txtCostVatAmount As TextBox = CType(row.FindControl("txtCostVatAmount"), TextBox)
            If IsNumeric(txtCostVatAmount.Text) Then
                totalCostVatAmt = totalCostVatAmt + Convert.ToDecimal(txtCostVatAmount.Text)
            End If
        Next

        Dim lblCostPriceTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostPriceTotal"), Label)
        lblCostPriceTotal.Text = Math.Round(Convert.ToDecimal(total), decimalPlaces).ToString()

        Dim lblCostValueTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostValueTotal"), Label)
        lblCostValueTotal.Text = Math.Round(Convert.ToDecimal(totalCostValue), decimalPlaces).ToString()

        Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
        lblNonTaxAmountTotal.Text = Math.Round(Convert.ToDecimal(totalNonTaxAmt), decimalPlaces).ToString()

        Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
        lblTaxAmountTotal.Text = Math.Round(Convert.ToDecimal(totalTaxAmt), decimalPlaces).ToString()

        Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
        lblCostVatAmountTotal.Text = Math.Round(Convert.ToDecimal(totalCostVatAmt), decimalPlaces).ToString()

    End Sub


#Region "Protected Sub ddlCommissionType_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCommissionType.SelectedIndexChanged"
    Protected Sub ddlCommissionType_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCommissionType.SelectedIndexChanged 'Handles ddlCommissionType.SelectedIndexChanged

        If ddlCommissionType.SelectedValue = "Commissionable" Then
            'gvUpdateSupplier.Columns(18).Visible = True
            gvUpdateSupplier.Columns(19).Visible = True
            gvPriceList.Columns(13).Visible = True
            ddlfillamount.Items.Add("Commission")
            hdnCommissionFlag.Value = "true"
            Session("Commission") = "true"
        Else
            'gvUpdateSupplier.Columns(18).Visible = False
            gvUpdateSupplier.Columns(19).Visible = False
            gvPriceList.Columns(13).Visible = False
            ddlfillamount.Items.Remove("Commission")
            hdnCommissionFlag.Value = "false"
            Session("Commission") = "false"
        End If
    End Sub
#End Region

    Private Sub ClearNewMode()
        txtDocDate.Text = Date.Today.ToString("dd/MM/yyyy")
        txtDocNo.Text = ""
        'ddlType.SelectedIndex = 0
        txtSupplier.Text = ""
        txtSupplierCode.Text = ""
        txtCtrlAcct.Text = ""
        txtCtrlAcctCode.Text = ""
        txtCurrency.Text = ""
        txtCurrencyCode.Text = ""
        txtConvRate.Text = ""
        txtTrnNo.Text = ""
        'txtInvoiceNo.Text = ""
        'txtRequestId.Text = ""
        txtNarration.Text = ""
        txtChkFromDt.Text = ""
        txtChkToDt.Text = ""
        'ddlType.Focus()
        Dim dt As DataTable = CType(Session("SupplierInvoice"), DataTable)
        gvUpdateSupplier.DataSource = dt
        gvUpdateSupplier.DataBind()
        lblMsg.Visible = False
        Session("Commission") = "false"
        hdnCommissionFlag.Value = "False"
        ddlCommissionType.SelectedIndex = 0
        txtSupplier.Enabled = False 'changed by mohamed on 08/11/2021
        'ddlType.Enabled = True
        gvError.DataSource = Nothing
        gvError.DataBind()

    End Sub

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtDocDate.Text = Date.Today.ToString("dd/MM/yyyy")
        'ddlType.SelectedIndex = 0
        txtSupplier.Text = ""
        txtSupplierCode.Text = ""
        txtCtrlAcct.Text = ""
        txtCtrlAcctCode.Text = ""
        txtCurrency.Text = ""
        txtCurrencyCode.Text = ""
        txtConvRate.Text = ""
        txtTrnNo.Text = ""
        'txtInvoiceNo.Text = ""
        'txtRequestId.Text = ""
        'txtNarration.Text = ""
        'txtChkFromDt.Text = ""
        'txtChkToDt.Text = ""
        'ddlType.Focus()
        Dim dt As DataTable = CType(Session("SupplierInvoice"), DataTable)
        gvUpdateSupplier.DataSource = dt
        gvUpdateSupplier.DataBind()
        lblMsg.Visible = False
        txtSupplier.Enabled = False
        gvError.DataSource = Nothing
        gvError.DataBind()
        Errtr1.Visible = False
        Errtr2.Visible = False
        'ddlType.Enabled = True
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('UpdateSupplierInvoicePostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

    Protected Sub ShowGVPricelistPopup(ByVal gvr As GridViewRow)
        lblMsgPrice.Visible = False
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim lblPaxType As Label = CType(gvr.FindControl("lblPaxType"), Label)
        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)
        Dim txtRoomNo As TextBox = CType(gvr.FindControl("txtRoomNo"), TextBox)
        Dim txtCostVatAmt As TextBox = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
        Dim txtCommisionAmt As TextBox = CType(gvr.FindControl("txtCommisionAmt"), TextBox)
        Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
        Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        Dim lblVatInputProvAmt As Label = CType(gvr.FindControl("lblVatInputProvAmt"), Label)
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        Dim lblCheckIn As Label = CType(gvr.FindControl("lblCheckIn"), Label)
        Dim lblPaxDetails As Label = CType(gvr.FindControl("lblPax"), Label)
        Dim lblCurrentCostVatAmt As Label = CType(gvr.FindControl("lblCurrentCostVatAmt"), Label)

        Dim lblCommisionAmt As Label = CType(gvr.FindControl("lblCommisionAmt"), Label)
        Dim lblnontaxAmt As Label = CType(gvr.FindControl("lblnontaxAmt"), Label)
        Dim lbltaxAmt As Label = CType(gvr.FindControl("lbltaxAmt"), Label)
        hdCurrenttotalvatactual.Value = lblCurrentCostVatAmt.Text
        hdCurrenttotalCommissionactual.Value = IIf(lblCommisionAmt.Text = "", 0, lblCommisionAmt.Text)
        hdcurrenttaxactual.Value = lbltaxAmt.Text
        hdcurrentnontaxactual.Value = lblnontaxAmt.Text
        Dim ShowPriceList As HtmlGenericControl = CType(gvr.FindControl("ShowPriceList"), HtmlGenericControl)
        Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
        txttotalvatprovison.Text = lblVatInputProvAmt.Text
        txttotalvatactual.Text = txtCostVatAmt.Text
        currentcommissionamt.Value = IIf(txtCommisionAmt.Text = "", 0, txtCommisionAmt.Text)
        currentnontaxamt.Value = txtNonTaxAmt.Text
        currenttaxamt.Value = txtTaxAmt.Text
        hdPriceRequestId.Value = lblRequestId.Text
        hdPriceRlineNo.Value = txtRlineNo.Text
        hdPriceRoomNo.Value = txtRoomNo.Text
        HdnPaxType.Value = lblPaxType.Text
        hdServiceType.Value = lblServiceType.Text
        hdVatOldValue.Value = txtCostVatAmt.Text
        hdVatValueOldMainGrid.Value = lblCurrentCostVatAmt.Text
        If hdServiceType.Value.ToUpper = "HOTEL" Then
            If hdnPricedateFlag.Value = "Y" Then
                gvPriceList.Columns(0).HeaderText = "Check In Date"
            Else
                gvPriceList.Columns(0).HeaderText = "Price Date"
            End If
            gvPriceList.Columns(4).HeaderText = "Booking Code"
            gvPriceList.Columns(1).Visible = False
            gvPriceList.Columns(5).Visible = False
            gvPriceList.Columns(2).Visible = False
            gvPriceList.Columns(3).Visible = False
            gvPriceList.Columns(8).HeaderText = "Cost Price"
            gvPriceList.Columns(9).Visible = False
            gvPriceList.Columns(7).HeaderText = "Sale Price (" + baseCurrency + ")"
        Else 'If hdServiceType.Value.ToUpper = "SPECIALEVENT" Or hdServiceType.Value.ToUpper = "TRANSFERS" Or hdServiceType.Value.ToUpper = "TOURS" Or hdServiceType.Value.ToUpper = "AIRPORTMA" Or hdServiceType.Value.ToUpper = "AIRPORTMA" Then
            gvPriceList.Columns(1).Visible = True
            If hdServiceType.Value.ToUpper = "TRANSFERS" Then
                gvPriceList.Columns(0).HeaderText = "Transfer Date"
            ElseIf hdServiceType.Value.ToUpper = "TOURS" Then
                gvPriceList.Columns(0).HeaderText = "Tour Date"
            ElseIf hdServiceType.Value.ToUpper = "AIRPORTMA" Then
                gvPriceList.Columns(0).HeaderText = "AirportMA Date"
            ElseIf hdServiceType.Value.ToUpper = "OTHERS" Then
                gvPriceList.Columns(0).HeaderText = "OthService Date"
            ElseIf hdServiceType.Value.ToUpper = "VISA" Then
                gvPriceList.Columns(0).HeaderText = "Visa Date"
            Else
                gvPriceList.Columns(0).HeaderText = "SplEvent Date"
            End If
            gvPriceList.Columns(2).Visible = True
            gvPriceList.Columns(3).Visible = True
            If hdServiceType.Value.ToUpper = "TRANSFERS" Then
                gvPriceList.Columns(4).HeaderText = "Transfer Name"
            ElseIf hdServiceType.Value.ToUpper = "TOURS" Then
                gvPriceList.Columns(4).HeaderText = "Tour Name"
            ElseIf hdServiceType.Value.ToUpper = "AIRPORTMA" Then
                gvPriceList.Columns(4).HeaderText = "AirportMA Name"
            ElseIf hdServiceType.Value.ToUpper = "OTHERS" Then
                gvPriceList.Columns(4).HeaderText = "OthService Name"
            ElseIf hdServiceType.Value.ToUpper = "VISA" Then
                gvPriceList.Columns(4).HeaderText = "Visa Name"
            Else
                gvPriceList.Columns(4).HeaderText = "SplEvent Name"
            End If
            gvPriceList.Columns(5).Visible = True
            gvPriceList.Columns(7).HeaderText = "Sale Value (" + baseCurrency + ")"
            gvPriceList.Columns(4).ControlStyle.Width = "120"
            gvPriceList.Columns(8).HeaderText = "Cost Price"
            gvPriceList.Columns(9).Visible = True
            ';' ShowPriceList.Attributes.Add("style", "width:1000px")
            ' ShowPriceList.Style.Add("width", "1000px")


        End If
        txtvatpercpop.Text = CType(gvr.FindControl("txtVatPerc"), TextBox).Text
        hdnDocNo.Value = txtDocNo.Text
        'If ddlType.SelectedValue = "Supplier" Then
        '    hdnType.Value = "S"
        'Else
        '    hdnType.Value = "A"
        'End If

        Dim pdt As DataTable = CType(Session("priceListDt"), DataTable)

        Dim filterPdr = (From n In pdt.AsEnumerable Where n.Field(Of String)("requestId") = hdPriceRequestId.Value And
                              n.Field(Of Integer)("rlineno") = hdPriceRlineNo.Value And n.Field(Of Integer)("roomno") = hdPriceRoomNo.Value And n.Field(Of String)("servicetype") = hdServiceType.Value Select n)

        Dim filterPriceDt As New DataTable
        If filterPdr.Count > 0 Then
            filterPriceDt = filterPdr.CopyToDataTable()
        End If
        If filterPriceDt.Rows.Count > 0 Then
            Dim currcode As String = (From n In filterPriceDt.AsEnumerable Select n.Field(Of String)("currcode")).Distinct.SingleOrDefault
            gvPriceList.Columns(6).HeaderText = "Sale Price (" + currcode + ")"
            AdjustVatAmount(lblServiceType.Text, filterPriceDt, decimalPlaces)
            AdjustCommissionAmount(lblServiceType.Text, filterPriceDt, decimalPlaces)
            AdjustNonTaxAmount(lblServiceType.Text, filterPriceDt, decimalPlaces)
            AdjustTaxAmount(lblServiceType.Text, filterPriceDt, decimalPlaces)
            gvPriceList.DataSource = filterPriceDt
            gvPriceList.DataBind()
            CalculateGvPricelistTotals(filterPriceDt, decimalPlaces)


        Else
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            Dim count As Integer
            If lblServiceType.Text.ToLower = "hotel" Then
                count = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select count(*) from purchaseinvoicehotelpricedetail(nolock) where divcode='" & txtDivcode.Text & "' and purchaseinvoiceno='" & txtDocNo.Text & "' and purchaseinvoicetype='PI' and " &
                                                                      " requestid='" & lblRequestId.Text.Trim & "'and rlineno=" & txtRlineNo.Text & " and roomno=" & txtRoomNo.Text & "  ")
            ElseIf lblServiceType.Text.ToLower = "specialevent" Then
                count = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select count(*) from purchaseinvoicespleventspricedetail(nolock) where   divcode='" & txtDivcode.Text & "' and purchaseinvoiceno='" & txtDocNo.Text & "' and purchaseinvoicetype='PI' and " &
                                                                                              " requestid='" & lblRequestId.Text.Trim & "'and rlineno=" & txtRlineNo.Text & " and roomno=" & txtRoomNo.Text & "  ")
            ElseIf lblServiceType.Text.ToLower = "transfers" Then
                count = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select count(*) from purchaseinvoiceTransferspricedetail(nolock) where divcode='" & txtDivcode.Text & "' and purchaseinvoiceno='" & txtDocNo.Text & "' and purchaseinvoicetype='PI' and " &
                                                                                              " requestid='" & lblRequestId.Text.Trim & "'and rlineno=" & txtRlineNo.Text & " and roomno=" & txtRoomNo.Text & "  ")
            ElseIf lblServiceType.Text.ToLower = "tours" Then
                count = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select count(*) from purchaseinvoiceToursPriceDetail(nolock) where divcode='" & txtDivcode.Text & "' and purchaseinvoiceno='" & txtDocNo.Text & "' and purchaseinvoicetype='PI' and " &
                                                                                              " requestid='" & lblRequestId.Text.Trim & "'and elineno=" & txtRlineNo.Text & " and roomno=" & txtRoomNo.Text & "  ")
            ElseIf lblServiceType.Text.ToLower = "airportma" Then
                count = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select count(*) from purchaseinvoice_Airportma_pricedetail(nolock) where divcode='" & txtDivcode.Text & "' and purchaseinvoiceno='" & txtDocNo.Text & "' and purchaseinvoicetype='PI' and " &
                                                                                              " requestid='" & lblRequestId.Text.Trim & "'and alineno=" & txtRlineNo.Text & " and roomno=" & txtRoomNo.Text & "  ")
            ElseIf lblServiceType.Text.ToLower = "others" Then
                count = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select count(*) from purchaseinvoice_Others_pricedetail(nolock) where divcode='" & txtDivcode.Text & "' and purchaseinvoiceno='" & txtDocNo.Text & "' and purchaseinvoicetype='PI' and " &
                                                                                              " requestid='" & lblRequestId.Text.Trim & "'and olineno=" & txtRlineNo.Text & " and roomno=" & txtRoomNo.Text & "  ")
            ElseIf lblServiceType.Text.ToLower = "visa" Then
                count = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select count(*) from purchaseinvoice_Others_pricedetail(nolock) where divcode='" & txtDivcode.Text & "' and purchaseinvoiceno='" & txtDocNo.Text & "' and purchaseinvoicetype='PI' and " &
                                                                                              " requestid='" & lblRequestId.Text.Trim & "'and olineno=" & txtRlineNo.Text & " and roomno=" & txtRoomNo.Text & "  ")



            End If
            Dim priceDtFlag As String = hdnPricedateFlag.Value
            If count > 0 Then
                If lblServiceType.Text.ToLower = "hotel" Then
                    strSqlQry = "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,rlineno,pricedate,null  as paxtype, 0 as noofpax, null as childages,bookingcode," _
                & " null as bookingname,currcode,0 as rate, saleprice, salepricebase, costprice,0 as costvalue, costtaxablevalue, costnontaxablevalue, costvatvalue,Commission " _
                & " from purchaseinvoicehotelpricedetail p where p.divcode=@divcode and p.purchaseinvoiceno=@purchaseinvoiceno and p.purchaseinvoicetype=@purchaseinvoicetype and p.requestid=@requestId and p.rlineno=@rlineno and p.roomno=@roomno "

                ElseIf lblServiceType.Text.ToLower = "specialevent" Then
                    strSqlQry = "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,rlineno,roomno, spleventdate   as  pricedate,0 as bookingcode," _
     & " (select spleventname from invoicesplevent  where divcode=@divcode and purchaseinvoiceno=@purchaseinvoiceno and purchaseinvoicetype=@purchaseinvoicetype and requestid=@requestId and rlineno=@rlineno and  evlineno=@roomno) as bookingname ,paxtype,noofpax,childages,currcode,paxrate as rate,saleprice,salepricebase,costprice,costvalue,CostTaxableValue, costnontaxablevalue, costvatvalue,Commission    from purchaseinvoicespleventspricedetail  p " _
         & " where p.divcode=@divcode and p.purchaseinvoiceno=@purchaseinvoiceno and p.purchaseinvoicetype=@purchaseinvoicetype and p.requestid=@requestId and p.rlineno=@rlineno and p.roomno=@roomno"


                ElseIf lblServiceType.Text.ToLower = "transfers" Then
                    strSqlQry = "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,rlineno,roomno, transferdate   as  pricedate,0 as bookingcode," _
         & " (select serviceName from booking_services_cost  where  requestid=@requestId and elineno=@rlineno and  servicetype=@servicetype) as bookingname ,paxtype,noofpax , childages,currcode,paxrate as rate,saleprice,salepricebase,costprice,costvalue,CostTaxableValue, costnontaxablevalue, costvatvalue,Commission    from PurchaseInvoiceTransferspricedetail  p " _
             & " where p.divcode=@divcode and p.purchaseinvoiceno=@purchaseinvoiceno and p.purchaseinvoicetype=@purchaseinvoicetype and p.requestid=@requestId and p.rlineno=@rlineno and p.roomno=@roomno"

                ElseIf lblServiceType.Text.ToLower = "tours" Then
                    strSqlQry = "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,elineno,roomno, Excdate   as  pricedate,ExcTypCode as bookingcode," _
         & " (select distinct serviceName from booking_services_cost  where  requestid=@requestId and elineno=@rlineno and rownumber=@roomno and lower(servicetype)=@servicetype) as bookingname ,paxtype,noofpax,childages,currcode,paxrate as rate,saleprice,salepricebase,costprice,costvalue,CostTaxableValue, costnontaxablevalue, costvatvalue,Commission    from PurchaseInvoiceTourspricedetail  p " _
             & " where p.divcode=@divcode and p.purchaseinvoiceno=@purchaseinvoiceno and p.purchaseinvoicetype=@purchaseinvoicetype and p.requestid=@requestId and p.elineno=@rlineno and p.roomno=@roomno"

                ElseIf lblServiceType.Text.ToLower = "airportma" Then
                    strSqlQry = "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,alineno,roomno, airportmadate   as  pricedate,othtypcode as bookingcode," _
         & " (select serviceName from booking_services_cost  where  requestid=@requestId and elineno=@rlineno and  lower(servicetype) ='aiportma' and paxtype=@paxtype and rownumber=@roomno) as bookingname ,paxtype,noofpax,childages,currcode,paxrate as rate,saleprice,salepricebase,costprice,costvalue,CostTaxableValue, costnontaxablevalue, costvatvalue,Commission    from PurchaseInvoice_Airportma_pricedetail  p " _
             & " where p.divcode=@divcode and p.purchaseinvoiceno=@purchaseinvoiceno and p.purchaseinvoicetype=@purchaseinvoicetype and p.requestid=@requestId and p.alineno=@rlineno and p.roomno=@roomno"


                ElseIf lblServiceType.Text.ToLower = "others" Then
                    strSqlQry = "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,olineno,roomno, othdate   as  pricedate,OthTypCode as bookingcode," _
         & " (select serviceName from booking_services_cost  where  requestid=@requestId and elineno=@rlineno and  lower(servicetype) =@servicetype) as bookingname ,paxtype,noofpax,childages,currcode,paxrate as rate,saleprice,salepricebase,costprice,costvalue,CostTaxableValue, costnontaxablevalue, costvatvalue,Commission    from PurchaseInvoice_Others_pricedetail  p " _
             & " where p.divcode=@divcode and p.purchaseinvoiceno=@purchaseinvoiceno and p.purchaseinvoicetype=@purchaseinvoicetype and p.requestid=@requestId and p.olineno=@rlineno and p.roomno=@roomno"


                ElseIf lblServiceType.Text.ToLower = "visa" Then
                    strSqlQry = "select divcode,purchaseinvoiceno,purchaseinvoicetype,requestid,vlineno,roomno, visadate   as  pricedate,VisaTypeCode as bookingcode," _
         & " (select serviceName from booking_services_cost  where  requestid=@requestId and elineno=@rlineno and  lower(servicetype) =@servicetype) as bookingname ,paxtype,noofpax,childages,currcode,paxrate as rate,saleprice,salepricebase,costprice,costvalue,CostTaxableValue, costnontaxablevalue, costvatvalue,Commission    from PurchaseInvoice_Visa_pricedetail  p " _
             & " where p.divcode=@divcode and p.purchaseinvoiceno=@purchaseinvoiceno and p.purchaseinvoicetype=@purchaseinvoicetype and p.requestid=@requestId and p.rlineno=@rlineno and p.roomno=@roomno"

                End If
                mySqlCmd = New SqlCommand(strSqlQry, sqlConn)

                mySqlCmd.CommandType = CommandType.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno", SqlDbType.VarChar, 20)).Value = txtDocNo.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 10)).Value = "PI"
                mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                mySqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.Int)).Value = Convert.ToInt32(txtRoomNo.Text)
                mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = lblServiceType.Text.ToLower.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@paxtype", SqlDbType.VarChar, 20)).Value = lblPaxType.Text.ToLower.Trim
            Else
                If lblServiceType.Text.ToUpper = "HOTEL" Then

                    strSqlQry = "select p.pricedate,p.bookingcode,null as bookingname ,null as paxtype,0 as noofpax,null as childages,h.currcode,0 as rate,p.saleprice,p.SalePriceInAED salepricebase,p.totalprice costprice,0 as costvalue,p.CostTaxableValue,p.CostNonTaxableValue" &
                          " ,p.CostVatValue,0.00 As Commission from booking_hotel_detail_prices p(nolock) inner join booking_header h(nolock)  " &
                          " on p.requestid=h.requestid " &
                          "where p.requestid=@requestId and p.rlineno=@rlineno and p.roomno=@roomno " _
                          & " and ((@priceDateFlag='Y' and p.saleprice<>0 or p.totalprice<>0) or (@priceDateFlag='N' and p.saleprice=p.saleprice and p.totalprice=p.totalprice)) "
                    'mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                    'mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.Int)).Value = Convert.ToInt32(txtRoomNo.Text)

                ElseIf lblServiceType.Text.ToUpper = "SPECIALEVENT" Then
                    strSqlQry = " select    p.spleventdate as pricedate,s.spleventcode as bookingcode,s.spleventname as bookingname,p.paxtype as paxtype,p.noofpax as noofpax,p.childage as childages," _
                 & "  h.currcode ,p.paxrate as rate,p.spleventvalue as saleprice,s.spleventsalevaluebase as salepricebase,  p.paxcost costprice,p.spleventcostvalue as costvalue, s.spleventcosttaxablevalue as CostTaxableValue, s.spleventcostnontaxablevalue as CostNonTaxableValue, spleventcostvatvalue as CostVatValue,0.00 As Commission   from  booking_hotel_detail_specialevents p(nolock) inner join booking_header h(nolock)  " _
                 & "  on p.requestid=h.requestid  inner join  InvoiceSplevent s   on s.requestid=p.requestid and s.rlineno=p.rlineno and p.evlineno=s.evlineno and s.paxtype=p.paxtype where p.requestid=@requestId and p.rlineno=@rlineno and p.evlineno=@roomno and p.paxtype= @paxtype and p.spleventdate=@spleventdate"

                    '    mySqlCmd = New SqlCommand(strSqlQry, sqlConn)

                    '    mySqlCmd.CommandType = CommandType.Text
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.Int)).Value = Convert.ToInt32(txtRoomNo.Text)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@paxtype", SqlDbType.VarChar, 20)).Value = lblPaxDetails.Text.Split("-")(1).Trim
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@spleventdate", SqlDbType.VarChar, 20)).Value = Format(CType(lblCheckIn.Text, Date), "yyyy/MM/dd")
                ElseIf lblServiceType.Text.ToUpper = "TRANSFERS" Then
                    strSqlQry = " select    p.servicedate as pricedate,0 as bookingcode, servicename as bookingname,p.paxType as paxtype,p.noofPaxOrUnit as noofpax,case when lower(p.paxtype)='unit' then '' else s.childagestring end  as childages ," _
                 & "  h.currcode ,p.PaxOrUnitRate as rate,s.unitsalevalue as saleprice,(s.unitsalevalue * h.convrate)  as salepricebase, p.paxOrUnitRate as costprice,p.costValue as costvalue, round(p.costValue/(1+(convert(decimal(24,8),s.VATPer)/100)),@decimalPlaces) as CostTaxableValue, " _
                 & "0  as CostNonTaxableValue, p.costValue - round(p.costValue/(1+(convert(decimal(24,8),s.VATPer)/100)),@decimalPlaces) as CostVatValue,0.00 As Commission   from  booking_services_cost p(nolock) inner join booking_header h(nolock)  " _
                 & "  on p.requestid=h.requestid  inner   join  booking_transfers s   on s.requestid=p.requestid and s.tlineno=p.elineno    where p.requestid=@requestId and p.elineno=@rlineno  and p.servicetype=@servicetype   and p.paxtype=@PaxType" 'and CONVERT(date, p.servicedate)=@transferdate"
                    'mySqlCmd = New SqlCommand(strSqlQry, sqlConn)

                    'mySqlCmd.CommandType = CommandType.Text

                    'mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                    'mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = lblServiceType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@transferdate", SqlDbType.VarChar, 20)).Value = Format(CType(lblCheckIn.Text, Date), "yyyy/MM/dd")


                ElseIf lblServiceType.Text.ToUpper = "TOURS" Then
                    strSqlQry = " select    p.servicedate as pricedate,s.exctypcode as bookingcode, servicename as bookingname,p.paxType as paxtype,p.noofPaxOrUnit as noofpax, case when lower(p.paxtype)='unit' then '' else s.childages end  as childages ," _
                 & "  h.currcode ,p.PaxOrUnitRate as rate,s.unitsalevalue as saleprice,(s.unitsalevalue * h.convrate) as salepricebase, p.paxOrUnitRate as costprice,p.costValue as costvalue, round(p.costValue/(1+(convert(decimal(24,8),s.CostVATPerc)/100)),@decimalPlaces) as CostTaxableValue, " _
                 & "0  as CostNonTaxableValue, p.costValue - round(p.costValue/(1+(convert(decimal(24,8),s.CostVATPerc)/100)),@decimalPlaces) as CostVatValue ,0.00 As Commission  from  booking_services_cost p(nolock) inner join booking_header h(nolock)  " _
                 & "  on p.requestid=h.requestid  inner   join  booking_tours s   on s.requestid=p.requestid and s.elineno=p.elineno    where p.requestid=@requestId and p.elineno=@rlineno  and p.servicetype=@servicetype  and p.paxtype=@PaxType and p.rownumber=@rownumber " 'and CONVERT(date, p.servicedate)=@transferdate"
                    'mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                    'mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = lblServiceType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@transferdate", SqlDbType.VarChar, 20)).Value = Format(CType(lblCheckIn.Text, Date), "yyyy/MM/dd")


                ElseIf lblServiceType.Text.ToUpper = "AIRPORTMA" Then
                    strSqlQry = " select     p.servicedate as pricedate,s.othtypcode as bookingcode, servicename as bookingname,p.paxType as paxtype,p.noofPaxOrUnit as noofpax,case when lower(p.paxtype)='unit' then '' else s.childagestring end  as childages," _
                 & "  h.currcode ,p.PaxOrUnitRate as rate, case when paxtype='Unit' then s.unitsalevalue when paxtype='Adult' then s.adultsalevalue else s.childsalevalue end as saleprice,case when paxtype='Unit' then  (s.unitsalevalue * h.convrate) when paxtype='Adult' then (s.adultsalevalue * h.convrate) else   (s.childsalevalue * h.convrate)   end as salepricebase, p.paxOrUnitRate as costprice,p.costValue as costvalue, " _
                 & " round(p.costValue/(1+(convert(decimal(24,8),s.CostVATPerc)/100)),@decimalPlaces) as CostTaxableValue, 0  as CostNonTaxableValue, p.costValue - round(p.costValue/(1+(convert(decimal(24,8),s.CostVATPerc)/100)),@decimalPlaces) as CostVatValue,0.00 As Commission   from  booking_services_cost p(nolock) inner join booking_header h(nolock)  " _
                 & "  on p.requestid=h.requestid  inner   join  booking_airportma s   on s.requestid=p.requestid and s.alineno=p.elineno     where p.requestid=@requestId and p.elineno=@rlineno  and lower(p.servicetype)='AiportMA' and p.paxtype=@PaxType and p.rowNumber=@rowNumber " 'and CONVERT(date, p.servicedate)=@transferdate"
                    'mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
                    'mySqlCmd.CommandType = CommandType.Text
                    ''mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                    'mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = lblServiceType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@PaxType", SqlDbType.VarChar, 20)).Value = lblPaxType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@rowNumber", SqlDbType.Int)).Value = Convert.ToInt32(txtRoomNo.Text)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@transferdate", SqlDbType.VarChar, 20)).Value = Format(CType(lblCheckIn.Text, Date), "yyyy/MM/dd")

                ElseIf lblServiceType.Text.ToUpper = "OTHERS" Then
                    strSqlQry = " select     p.servicedate as pricedate,s.othtypcode as bookingcode, servicename as bookingname,p.paxType as paxtype,p.noofPaxOrUnit as noofpax,case when lower(p.paxtype)='unit' then '' else s.childages end  as childages," _
                 & "  h.currcode ,p.PaxOrUnitRate as rate,s.unitsalevalue as saleprice,(s.unitsalevalue * h.convrate) as salepricebase, p.paxOrUnitRate as costprice,p.costValue as costvalue, round(p.costValue/(1+(convert(decimal(24,8),s.CostVATPerc)/100)),@decimalPlaces) as CostTaxableValue, " _
                 & "0  as CostNonTaxableValue, p.costValue - round(p.costValue/(1+(convert(decimal(24,8),s.CostVATPerc)/100)),@decimalPlaces) as CostVatValue,0.00 As Commission   from  booking_services_cost p(nolock) inner join booking_header h(nolock)  " _
                 & "  on p.requestid=h.requestid  inner   join  booking_others s   on s.requestid=p.requestid and s.olineno=p.elineno    where p.requestid=@requestId and p.elineno=@rlineno  and lower(p.servicetype)=@servicetype  and p.paxtype=@PaxType " 'and CONVERT(date, p.servicedate)=@transferdate"
                    'mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                    'mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = lblServiceType.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@transferdate", SqlDbType.VarChar, 20)).Value = Format(CType(lblCheckIn.Text, Date), "yyyy/MM/dd")


                ElseIf lblServiceType.Text.ToUpper = "VISA" Then
                    strSqlQry = " select     p.servicedate as pricedate,s.visatypecode as bookingcode, servicename as bookingname,p.paxType as paxtype,p.noofPaxOrUnit as noofpax,case when lower(p.paxtype)='unit' then '' else s.childages end   as childages," _
                 & "  h.currcode ,p.PaxOrUnitRate as rate,s.visaprice as saleprice,(s.visaprice * h.convrate) as salepricebase, p.paxOrUnitRate as costprice,p.costValue as costvalue, round((p.costValue - isnull(s.CostNonTaxableValue,0))/(1+(convert(decimal(24,8),s.costvatperc)/100)),@decimalPlaces) as CostTaxableValue, isnull(s.CostNonTaxableValue,0)  as CostNonTaxableValue, " _
                 & "p.costValue - isnull(s.CostNonTaxableValue,0)-round((p.costValue - isnull(s.CostNonTaxableValue,0))/(1+(convert(decimal(24,8),s.costvatperc)/100)),@decimalPlaces) as CostVatValue,0.00 As Commission   from  booking_services_cost p(nolock) inner join booking_header h(nolock)  " _
                 & "  on p.requestid=h.requestid  inner   join  booking_visa s   on s.requestid=p.requestid and s.vlineno=p.elineno    where p.requestid=@requestId and p.elineno=@rlineno  and lower(p.servicetype)=@servicetype   and p.paxtype=@PaxType" 'and CONVERT(date, p.servicedate)=@transferdate"


                End If
                mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
                mySqlCmd.CommandType = CommandType.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = Convert.ToInt32(txtRlineNo.Text)
                mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = lblServiceType.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@transferdate", SqlDbType.VarChar, 20)).Value = Format(CType(lblCheckIn.Text, Date), "yyyy/MM/dd")
                If lblServiceType.Text.ToUpper = "SPECIALEVENT" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@paxtype", SqlDbType.VarChar, 20)).Value = lblPaxDetails.Text.Split("-")(1).Trim
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@PaxType", SqlDbType.VarChar, 20)).Value = lblPaxType.Text
                End If
                mySqlCmd.Parameters.Add(New SqlParameter("@rowNumber", SqlDbType.Int)).Value = Convert.ToInt32(txtRoomNo.Text)
                mySqlCmd.Parameters.Add(New SqlParameter("@spleventdate", SqlDbType.VarChar, 20)).Value = Format(CType(lblCheckIn.Text, Date), "yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.Int)).Value = Convert.ToInt32(txtRoomNo.Text)
                mySqlCmd.Parameters.Add(New SqlParameter("@priceDateFlag", SqlDbType.VarChar, 1)).Value = hdnPricedateFlag.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@decimalPlaces", SqlDbType.Int)).Value = decimalPlaces

            End If
            Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable
                    myDataAdapter.Fill(dt)
                    Dim currcode As String = (From n In dt.AsEnumerable Select n.Field(Of String)("currcode")).Distinct.SingleOrDefault
                    If lblServiceType.Text = "Hotel" Then
                        gvPriceList.Columns(6).HeaderText = "Sale Price (" + currcode + ")"
                    Else
                        gvPriceList.Columns(6).HeaderText = "Sale Value (" + currcode + ")"
                    End If
                    AdjustVatAmount(lblServiceType.Text, dt, decimalPlaces)
                    AdjustCommissionAmount(lblServiceType.Text, dt, decimalPlaces)
                    AdjustNonTaxAmount(lblServiceType.Text, dt, decimalPlaces)
                    AdjustTaxAmount(lblServiceType.Text, dt, decimalPlaces)
                    '  End If
                    If dt.Rows.Count > 0 Then
                        gvPriceList.DataSource = dt
                        gvPriceList.DataBind()
                        'For Each row As GridViewRow In gvPriceList.Rows
                        'Next
                        CalculateGvPricelistTotals(dt, decimalPlaces)
                    Else
                        lblMsgPrice.Visible = True
                    End If
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        End If

        If (ViewState("SupplierInvoiceState") = "View") Then
            DisableGvPricelist()
        End If

        calculateNetTotal()
    End Sub
#Region "Protected Sub gvUpdateSupplier_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUpdateSupplier.RowCommand"
    Protected Sub gvUpdateSupplier_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUpdateSupplier.RowCommand
        Try

            'Dim lblTotalVatInputProvAmt As Label = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatInputProvAmt"), Label)
            ' Dim lblTotalVatAmt As Label = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatAmt"), Label)
            'Dim lblTotalVatAmt As Label = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatAmt"), Label)

            '## Rosalin 03/10/2023
            'If e.CommandName = "ShowPrice" Then
            '    Dim gvr As GridViewRow = gvUpdateSupplier.Rows(e.CommandArgument)
            '    gvPriceList.DataSource = Nothing
            '    gvPriceList.DataBind()
            '    ShowGVPricelistPopup(gvr)
            '    ModalExtraPopup.Show()

            'End If
            ' ##
        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
    Private Sub AdjustCommissionAmount(ByVal servicetype As String, ByVal dt As DataTable, ByVal decimalplaces As Integer)
        If servicetype.ToLower = "hotel" Or servicetype.ToLower = "specialevent" Then
            Dim AdjustFirstRowValue As Decimal
            If dt.Rows.Count > 1 Then
                Dim dtsumcostcommissionvalue As Decimal

                Dim commissionDifference = Convert.ToDecimal(hdCurrenttotalCommissionactual.Value) - Convert.ToDecimal(currentcommissionamt.Value)
                If Convert.ToDecimal(currentcommissionamt.Value) <> Convert.ToDecimal(hdCurrenttotalCommissionactual.Value) Then
                    If commissionDifference > -1 And commissionDifference < 1 Then
                        dt.Rows(0).Item("Commission") = Math.Round(Convert.ToDecimal(dt.Rows(0).Item("Commission").ToString()) + commissionDifference, decimalplaces)
                    Else
                        Dim commissionamttoadjust As Decimal
                        If Convert.ToDecimal(hdCurrenttotalCommissionactual.Value) > Convert.ToDecimal(currentcommissionamt.Value) Then
                            commissionamttoadjust = Math.Abs(commissionDifference / Convert.ToDecimal(hdCurrenttotalCommissionactual.Value))
                        Else
                            commissionamttoadjust = Math.Abs(commissionDifference / Convert.ToDecimal(currentcommissionamt.Value))
                        End If
                        For Each datarow In dt.Rows
                            'If Convert.ToDecimal(hdCurrenttotalCommissionactual.Value) > Convert.ToDecimal(currentcommissionamt.Value) Then
                            '    datarow.Item("Commission") = Math.Round(datarow.Item("Commission") * (1 + commissionamttoadjust), decimalplaces)
                            'Else
                            datarow.Item("Commission") = Math.Round(datarow.Item("Commission") * (1 - commissionamttoadjust), decimalplaces)
                            'End If
                        Next
                    End If
                    dt.AcceptChanges()
                    dtsumcostcommissionvalue = Math.Round(dt.Compute("Sum(Commission)", String.Empty), decimalplaces)
                    Dim Diffcommissionactual = Math.Abs(Math.Round((Convert.ToDecimal(currentcommissionamt.Value) - dtsumcostcommissionvalue), decimalplaces))
                    If Diffcommissionactual > -1 And Diffcommissionactual < 1 Then
                        If Convert.ToDecimal(dtsumcostcommissionvalue) > Convert.ToDecimal(currentcommissionamt.Value) Then
                            dt.Rows(0).Item("Commission") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("Commission")) - Diffcommissionactual), decimalplaces)
                        Else
                            dt.Rows(0).Item("Commission") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("Commission")) + Diffcommissionactual), decimalplaces)
                        End If
                    Else
                        Dim NewcommissionValueLeft As Decimal = Diffcommissionactual / dt.Rows.Count
                        For Each datarow In dt.Rows
                            If Convert.ToDecimal(dtsumcostcommissionvalue) <> Convert.ToDecimal(currentcommissionamt.Value) Then
                                datarow.Item("Commission") = Math.Round(datarow.Item("Commission") + Convert.ToDecimal(NewcommissionValueLeft), decimalplaces)
                            End If
                        Next
                        dt.AcceptChanges()
                        Dim NewcommissionValuTotal = Math.Round(dt.Compute("Sum(Commission)", String.Empty), decimalplaces)
                        AdjustFirstRowValue = Math.Abs(Convert.ToDecimal(NewcommissionValuTotal - Convert.ToDecimal(currentcommissionamt.Value)))
                        If (NewcommissionValuTotal <> Math.Round(Convert.ToDecimal(currentcommissionamt.Value))) Then
                            If (NewcommissionValuTotal < Convert.ToDecimal(currentcommissionamt.Value)) Then
                                dt.Rows(0).Item("Commission") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("Commission")) + AdjustFirstRowValue), decimalplaces)
                            Else
                                dt.Rows(0).Item("Commission") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("Commission")) - AdjustFirstRowValue), decimalplaces)
                            End If
                        End If
                    End If
                    dt.AcceptChanges()
                    'For Each datarow In dt.Rows
                    '    If servicetype.ToLower = "hotel" Then
                    '        datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostPrice") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("Commission"))), decimalplaces)
                    '    ElseIf servicetype.ToLower = "specialevent" Then
                    '        datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostValue") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("Commission"))), decimalplaces)
                    '    End If
                    'Next
                ElseIf commissionDifference = 0 Then
                    Dim NewcommissionValueLeft As Decimal = currentcommissionamt.Value / dt.Rows.Count
                    For Each datarow In dt.Rows
                        ' If Convert.ToDecimal(dtsumcostcommissionvalue) <> Convert.ToDecimal(currentcommissionamt.Value) Then
                        datarow.Item("Commission") = Math.Round(Convert.ToDecimal(NewcommissionValueLeft), decimalplaces)
                        'End If
                    Next
                    dt.AcceptChanges()
                    Dim NewcommissionValuTotal = Math.Round(dt.Compute("Sum(Commission)", String.Empty), decimalplaces)
                    AdjustFirstRowValue = Math.Abs(Convert.ToDecimal(NewcommissionValuTotal - Math.Round(Convert.ToDecimal(currentcommissionamt.Value), decimalplaces)))
                    If (NewcommissionValuTotal <> Math.Round(Convert.ToDecimal(currentcommissionamt.Value), decimalplaces)) Then
                        If (NewcommissionValuTotal < Math.Round(Convert.ToDecimal(currentcommissionamt.Value), decimalplaces)) Then
                            dt.Rows(0).Item("Commission") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("Commission")) + AdjustFirstRowValue), decimalplaces)
                        Else
                            dt.Rows(0).Item("Commission") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("Commission")) - AdjustFirstRowValue), decimalplaces)
                        End If
                    End If

                End If
                dt.AcceptChanges()

            Else
                dt.Rows(0).Item("Commission") = Convert.ToDecimal(currentcommissionamt.Value)
                'If servicetype.ToLower = "hotel" Then

                '    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostPrice") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("Commission"))), decimalplaces)

                'ElseIf servicetype.ToLower = "specialevent" Then

                '    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostValue") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("Commission"))), decimalplaces)
                'End If
                dt.AcceptChanges()
            End If
        End If
    End Sub

    Private Sub AdjustNonTaxAmount(ByVal servicetype As String, ByVal dt As DataTable, ByVal decimalplaces As Integer)
        If servicetype.ToLower = "hotel" Or servicetype.ToLower = "specialevent" Then
            Dim AdjustFirstRowValue As Decimal
            If dt.Rows.Count > 1 Then
                Dim dtsumCostNonTaxableValue As Decimal

                Dim nontaxDifference = Convert.ToDecimal(hdcurrentnontaxactual.Value) - Convert.ToDecimal(currentnontaxamt.Value)
                If Convert.ToDecimal(currentnontaxamt.Value) <> Convert.ToDecimal(hdcurrentnontaxactual.Value) Then
                    If nontaxDifference > -1 And nontaxDifference < 1 Then
                        dt.Rows(0).Item("CostNonTaxableValue") = Math.Round(Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue").ToString()) + nontaxDifference, decimalplaces)
                    Else
                        Dim nontaxamttoadjust As Decimal
                        If Convert.ToDecimal(hdcurrentnontaxactual.Value) > Convert.ToDecimal(currentnontaxamt.Value) Then
                            nontaxamttoadjust = Math.Abs(nontaxDifference / Convert.ToDecimal(hdcurrentnontaxactual.Value))
                        Else
                            nontaxamttoadjust = Math.Abs(nontaxDifference / Convert.ToDecimal(currentnontaxamt.Value))
                        End If
                        For Each datarow In dt.Rows
                            If Convert.ToDecimal(hdcurrentnontaxactual.Value) > Convert.ToDecimal(currentnontaxamt.Value) Then
                                datarow.Item("CostNonTaxableValue") = Math.Round(datarow.Item("CostNonTaxableValue") * (1 - nontaxamttoadjust), decimalplaces)
                            Else
                                datarow.Item("CostNonTaxableValue") = Math.Round(datarow.Item("CostNonTaxableValue") * (1 + nontaxamttoadjust), decimalplaces)
                            End If
                        Next
                    End If
                    dt.AcceptChanges()
                    dtsumCostNonTaxableValue = Math.Round(dt.Compute("Sum(CostNonTaxableValue)", String.Empty), decimalplaces)
                    Dim Diffnontaxactual = Math.Abs(Math.Round((Convert.ToDecimal(currentnontaxamt.Value) - dtsumCostNonTaxableValue), decimalplaces))
                    If Diffnontaxactual > -1 And Diffnontaxactual < 1 Then
                        If Convert.ToDecimal(dtsumCostNonTaxableValue) > Convert.ToDecimal(currentnontaxamt.Value) Then
                            dt.Rows(0).Item("CostNonTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostNonTaxableValue")) - Diffnontaxactual), decimalplaces)
                        Else
                            dt.Rows(0).Item("CostNonTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostNonTaxableValue")) + Diffnontaxactual), decimalplaces)
                        End If
                    Else
                        Dim NewnontaxValueLeft As Decimal = Diffnontaxactual / dt.Rows.Count
                        For Each datarow In dt.Rows
                            If Convert.ToDecimal(dtsumCostNonTaxableValue) <> Convert.ToDecimal(currentnontaxamt.Value) Then
                                datarow.Item("CostNonTaxableValue") = Math.Round(datarow.Item("CostNonTaxableValue") + Convert.ToDecimal(NewnontaxValueLeft), decimalplaces)
                            End If
                        Next
                        dt.AcceptChanges()
                        Dim NewnontaxValuTotal = Math.Round(dt.Compute("Sum(CostNonTaxableValue)", String.Empty), decimalplaces)
                        AdjustFirstRowValue = Math.Abs(Convert.ToDecimal(NewnontaxValuTotal - Math.Round(Convert.ToDecimal(currentnontaxamt.Value))))
                        If (NewnontaxValuTotal <> Math.Round(Convert.ToDecimal(currentnontaxamt.Value))) Then
                            If (NewnontaxValuTotal < Convert.ToDecimal(currentnontaxamt.Value)) Then
                                dt.Rows(0).Item("CostNonTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostNonTaxableValue")) + AdjustFirstRowValue), decimalplaces)
                            Else
                                dt.Rows(0).Item("CostNonTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostNonTaxableValue")) - AdjustFirstRowValue), decimalplaces)
                            End If
                        End If
                    End If
                    dt.AcceptChanges()
                    'For Each datarow In dt.Rows
                    '    If servicetype.ToLower = "hotel" Then
                    '        datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostPrice") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("CostNonTaxableValue"))), decimalplaces)
                    '    ElseIf servicetype.ToLower = "specialevent" Then
                    '        datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostValue") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("CostNonTaxableValue"))), decimalplaces)
                    '    End If
                    'Next
                End If
                dt.AcceptChanges()

            Else
                dt.Rows(0).Item("CostNonTaxableValue") = Convert.ToDecimal(currentnontaxamt.Value)
                'If servicetype.ToLower = "hotel" Then

                '    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostPrice") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue"))), decimalplaces)

                'ElseIf servicetype.ToLower = "specialevent" Then

                '    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostValue") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue"))), decimalplaces)
                'End If
                dt.AcceptChanges()
            End If
        End If
    End Sub

    Private Sub AdjustTaxAmount(ByVal servicetype As String, ByVal dt As DataTable, ByVal decimalplaces As Integer)
        If servicetype.ToLower = "hotel" Or servicetype.ToLower = "specialevent" Then
            Dim AdjustFirstRowValue As Decimal
            If dt.Rows.Count > 1 Then
                Dim dtsumCostTaxableValue As Decimal

                Dim taxDifference = Convert.ToDecimal(hdcurrenttaxactual.Value) - Convert.ToDecimal(currenttaxamt.Value)
                If Convert.ToDecimal(currenttaxamt.Value) <> Convert.ToDecimal(hdcurrenttaxactual.Value) Then
                    If taxDifference > -1 And taxDifference < 1 Then
                        dt.Rows(0).Item("CostTaxableValue") = Math.Round(Convert.ToDecimal(dt.Rows(0).Item("CostTaxableValue").ToString()) + taxDifference, decimalplaces)
                    Else
                        Dim taxamttoadjust As Decimal
                        If Convert.ToDecimal(hdcurrenttaxactual.Value) > Convert.ToDecimal(currenttaxamt.Value) Then
                            taxamttoadjust = Math.Abs(taxDifference / Convert.ToDecimal(hdcurrenttaxactual.Value))
                        Else
                            taxamttoadjust = Math.Abs(taxDifference / Convert.ToDecimal(currenttaxamt.Value))
                        End If
                        For Each datarow In dt.Rows
                            If Convert.ToDecimal(hdcurrenttaxactual.Value) > Convert.ToDecimal(currenttaxamt.Value) Then
                                datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostTaxableValue") * (1 - taxamttoadjust), decimalplaces)
                            Else
                                datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostTaxableValue") * (1 + taxamttoadjust), decimalplaces)
                            End If
                        Next
                    End If
                    dt.AcceptChanges()
                    dtsumCostTaxableValue = Math.Round(dt.Compute("Sum(CostTaxableValue)", String.Empty), decimalplaces)
                    Dim Difftaxactual = Math.Abs(Math.Round((Convert.ToDecimal(currenttaxamt.Value) - dtsumCostTaxableValue), decimalplaces))
                    If Difftaxactual > -1 And Difftaxactual < 1 Then
                        If Convert.ToDecimal(dtsumCostTaxableValue) > Convert.ToDecimal(currenttaxamt.Value) Then
                            dt.Rows(0).Item("CostTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostTaxableValue")) - Difftaxactual), decimalplaces)
                        Else
                            dt.Rows(0).Item("CostTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostTaxableValue")) + Difftaxactual), decimalplaces)
                        End If
                    Else
                        Dim NewtaxValueLeft As Decimal = Difftaxactual / dt.Rows.Count
                        For Each datarow In dt.Rows
                            If Convert.ToDecimal(dtsumCostTaxableValue) <> Convert.ToDecimal(currenttaxamt.Value) Then
                                datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostTaxableValue") + Convert.ToDecimal(NewtaxValueLeft), decimalplaces)
                            End If
                        Next
                        dt.AcceptChanges()
                        Dim NewtaxValuTotal = Math.Round(dt.Compute("Sum(CostTaxableValue)", String.Empty), decimalplaces)
                        AdjustFirstRowValue = Math.Abs(Convert.ToDecimal(NewtaxValuTotal - Math.Round(Convert.ToDecimal(currenttaxamt.Value))))
                        If (NewtaxValuTotal <> Math.Round(Convert.ToDecimal(currenttaxamt.Value))) Then
                            If (NewtaxValuTotal < Convert.ToDecimal(currenttaxamt.Value)) Then
                                dt.Rows(0).Item("CostTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostTaxableValue")) + AdjustFirstRowValue), decimalplaces)
                            Else
                                dt.Rows(0).Item("CostTaxableValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostTaxableValue")) - AdjustFirstRowValue), decimalplaces)
                            End If
                        End If
                    End If
                    dt.AcceptChanges()
                    'For Each datarow In dt.Rows
                    '    If servicetype.ToLower = "hotel" Then
                    '        datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostPrice") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("CostTaxableValue"))), decimalplaces)
                    '    ElseIf servicetype.ToLower = "specialevent" Then
                    '        datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostValue") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("CostTaxableValue"))), decimalplaces)
                    '    End If
                    'Next
                End If
                dt.AcceptChanges()

            Else
                dt.Rows(0).Item("CostTaxableValue") = Convert.ToDecimal(currenttaxamt.Value)
                'If servicetype.ToLower = "hotel" Then

                '    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostPrice") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("CostTaxableValue"))), decimalplaces)

                'ElseIf servicetype.ToLower = "specialevent" Then

                '    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostValue") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("CostTaxableValue"))), decimalplaces)
                'End If
                dt.AcceptChanges()
            End If
        End If
    End Sub

    Private Sub AdjustVatAmount(ByVal servicetype As String, ByVal dt As DataTable, ByVal decimalplaces As Integer)
        If servicetype.ToLower = "hotel" Or servicetype.ToLower = "specialevent" Then
            Dim AdjustFirstRowValue As Decimal
            If dt.Rows.Count > 1 Then
                Dim dtsumcostvatvalue As Decimal

                Dim VatDifference = Convert.ToDecimal(hdCurrenttotalvatactual.Value) - Convert.ToDecimal(txttotalvatactual.Text)
                If Convert.ToDecimal(txttotalvatactual.Text) <> Convert.ToDecimal(hdCurrenttotalvatactual.Value) Then
                    If VatDifference > -1 And VatDifference < 1 Then
                        dt.Rows(0).Item("CostVatValue") = Math.Round(Convert.ToDecimal(dt.Rows(0).Item("CostVatValue").ToString()) + VatDifference, decimalplaces)
                    Else
                        Dim vatamttoadjust As Decimal
                        If Convert.ToDecimal(hdCurrenttotalvatactual.Value) > Convert.ToDecimal(txttotalvatactual.Text) Then
                            vatamttoadjust = Math.Abs(VatDifference / Convert.ToDecimal(hdCurrenttotalvatactual.Value))
                        Else
                            vatamttoadjust = Math.Abs(VatDifference / Convert.ToDecimal(txttotalvatactual.Text))
                        End If
                        For Each datarow In dt.Rows
                            If Convert.ToDecimal(hdCurrenttotalvatactual.Value) > Convert.ToDecimal(txttotalvatactual.Text) Then
                                datarow.Item("CostVatValue") = Math.Round(datarow.Item("CostVatValue") * (1 - vatamttoadjust), decimalplaces)
                            Else
                                datarow.Item("CostVatValue") = Math.Round(datarow.Item("CostVatValue") * (1 + vatamttoadjust), decimalplaces)
                            End If
                        Next
                    End If
                    dt.AcceptChanges()
                    dtsumcostvatvalue = Math.Round(dt.Compute("Sum(CostVatValue)", String.Empty), decimalplaces)
                    Dim Diffvatactual = Math.Abs(Math.Round((Convert.ToDecimal(txttotalvatactual.Text) - dtsumcostvatvalue), decimalplaces))
                    If Diffvatactual > -1 And Diffvatactual < 1 Then
                        If Convert.ToDecimal(dtsumcostvatvalue) > Convert.ToDecimal(txttotalvatactual.Text) Then
                            dt.Rows(0).Item("CostVatValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostVatValue")) - Diffvatactual), decimalplaces)
                        Else
                            dt.Rows(0).Item("CostVatValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostVatValue")) + Diffvatactual), decimalplaces)
                        End If
                    Else
                        Dim NewVatValueLeft As Decimal = Diffvatactual / dt.Rows.Count
                        For Each datarow In dt.Rows
                            If Convert.ToDecimal(dtsumcostvatvalue) <> Convert.ToDecimal(txttotalvatactual.Text) Then
                                datarow.Item("CostVatValue") = Math.Round(datarow.Item("CostVatValue") + Convert.ToDecimal(NewVatValueLeft), decimalplaces)
                            End If
                        Next
                        dt.AcceptChanges()
                        Dim NewVatValuTotal = Math.Round(dt.Compute("Sum(CostVatValue)", String.Empty), decimalplaces)
                        AdjustFirstRowValue = Math.Abs(Convert.ToDecimal(NewVatValuTotal - Math.Round(Convert.ToDecimal(txttotalvatactual.Text))))
                        If (NewVatValuTotal <> Math.Round(Convert.ToDecimal(txttotalvatactual.Text))) Then
                            If (NewVatValuTotal > Convert.ToDecimal(txttotalvatactual.Text)) Then
                                dt.Rows(0).Item("CostVatValue") = Math.Round(Convert.ToDecimal((dt.Rows(0).Item("CostVatValue")) - AdjustFirstRowValue), decimalplaces)
                            End If
                        End If
                    End If
                    dt.AcceptChanges()
                    For Each datarow In dt.Rows
                        If servicetype.ToLower = "hotel" Then
                            datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostPrice") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("CostVatValue"))), decimalplaces)
                        ElseIf servicetype.ToLower = "specialevent" Then
                            datarow.Item("CostTaxableValue") = Math.Round(datarow.Item("CostValue") - (Convert.ToDecimal(datarow.Item("CostNonTaxableValue")) + Convert.ToDecimal(datarow.Item("CostVatValue"))), decimalplaces)
                        End If
                    Next
                End If
                dt.AcceptChanges()

            Else
                dt.Rows(0).Item("CostVatValue") = Convert.ToDecimal(txttotalvatactual.Text)
                If servicetype.ToLower = "hotel" Then

                    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostPrice") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("CostVatValue"))), decimalplaces)

                ElseIf servicetype.ToLower = "specialevent" Then

                    dt.Rows(0).Item("CostTaxableValue") = Math.Round(dt.Rows(0).Item("CostValue") - (Convert.ToDecimal(dt.Rows(0).Item("CostNonTaxableValue")) + Convert.ToDecimal(dt.Rows(0).Item("CostVatValue"))), decimalplaces)
                End If
                dt.AcceptChanges()
            End If
        End If
    End Sub

#Region "DisableGvPricelist()"
    Private Sub DisableGvPricelist()
        btnfillCostPricepopup.Enabled = False
        txtfillAmount.Enabled = False
        btnPriceSave.Visible = False
        txtvatpercpop.Enabled = False
        ddlfillamount.Enabled = False
        txttotalvatactual.Enabled = False

        For Each row In gvPriceList.Rows
            Dim txtCostPrice As TextBox = CType(row.FindControl("txtCostPrice"), TextBox)
            Dim txtNonTaxAmount As TextBox = CType(row.FindControl("txtNonTaxAmount"), TextBox)
            Dim txtTaxAmount As TextBox = CType(row.FindControl("txtTaxAmount"), TextBox)
            Dim txtCostVatAmount As TextBox = CType(row.FindControl("txtCostVatAmount"), TextBox)
            Dim txtCommissionAmount As TextBox = CType(row.FindControl("txtCommissionAmount"), TextBox)
            Dim txtCostValue As TextBox = CType(row.FindControl("txtCostValue"), TextBox)
            txtCostValue.Enabled = False
            txtCostPrice.Enabled = False
            txtNonTaxAmount.Enabled = False
            txtTaxAmount.Enabled = False
            txtCostVatAmount.Enabled = False
            txtCommissionAmount.Enabled = False
        Next
    End Sub
#End Region
#Region "  Private Sub CalculateGvPricelistTotals(dt As DataTable, decimalplaces As Integer)"
    Private Sub CalculateGvPricelistTotals(ByVal dt As DataTable, ByVal decimalplaces As Integer)

        Dim lblSalePriceTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblSalePriceTotal"), Label)
        lblSalePriceTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(salePrice)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(salePrice)", String.Empty)), decimalplaces).ToString)
        Dim lblRateTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblRateTotal"), Label)
        lblRateTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(rate)", String.Empty))) Or Not dt.Compute("sum(rate)", String.Empty) Is Nothing, String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(rate)", String.Empty)), decimalplaces).ToString)
        Dim lblSalePriceBaseTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblSalePriceBaseTotal"), Label)
        lblSalePriceBaseTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(salePricebase)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(salePricebase)", String.Empty)), decimalplaces).ToString)
        Dim lblCostPriceTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostPriceTotal"), Label)
        lblCostPriceTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(costprice)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(costprice)", String.Empty)), decimalplaces).ToString)
        Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
        lblNonTaxAmountTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(CostNonTaxableValue)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(CostNonTaxableValue)", String.Empty)), decimalplaces).ToString)
        Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
        lblCostVatAmountTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(CostVatValue)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(CostVatValue)", String.Empty)), decimalplaces).ToString)
        Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
        lblTaxAmountTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(CostTaxableValue)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(CostTaxableValue)", String.Empty)), decimalplaces).ToString)
        Dim lblCostValueTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostValueTotal"), Label)
        lblCostValueTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(CostValue)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(CostValue)", String.Empty)), decimalplaces).ToString)
        'instead of CostVatValue take commission
        Dim lblCommissionAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCommissionAmountTotal"), Label)
        lblCommissionAmountTotal.Text = IIf(IsDBNull(Convert.ToDecimal(dt.Compute("sum(Commission)", String.Empty))), String.Empty, Math.Round(Convert.ToDecimal(dt.Compute("sum(Commission)", String.Empty)), decimalplaces).ToString)

    End Sub
#End Region
#Region "Protected Sub gvUpdateSupplier_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUpdateSupplier.RowDataBound"
    Protected Sub gvUpdateSupplier_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUpdateSupplier.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = Session("decimalPlaces")
            Dim lblProvAmt As Label = CType(e.Row.FindControl("lblProvAmt"), Label)
            Dim lblVatInputProvAmt As Label = CType(e.Row.FindControl("lblVatInputProvAmt"), Label)
            Dim lbtnActualAmt As LinkButton = CType(e.Row.FindControl("lbtnActualAmt"), LinkButton)
            Dim txtNonTaxAmt As TextBox = CType(e.Row.FindControl("txtNonTaxAmt"), TextBox)
            Dim txtTaxAmt As TextBox = CType(e.Row.FindControl("txtTaxAmt"), TextBox)
            Dim txtCostVatAmt As TextBox = CType(e.Row.FindControl("txtCostVatAmt"), TextBox)
            Dim txtCommissionAmt As TextBox = CType(e.Row.FindControl("txtCommisionAmt"), TextBox)
            Dim txtVatPerc As TextBox = CType(e.Row.FindControl("txtVatPerc"), TextBox)


            If IsNumeric(lblProvAmt.Text) Then
                lblProvAmt.Text = Math.Round(Convert.ToDecimal(lblProvAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(lblVatInputProvAmt.Text) Then
                lblVatInputProvAmt.Text = Math.Round(Convert.ToDecimal(lblVatInputProvAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(lbtnActualAmt.Text) Then
                lbtnActualAmt.Text = Math.Round(Convert.ToDecimal(lbtnActualAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(txtNonTaxAmt.Text) Then
                txtNonTaxAmt.Text = Math.Round(Convert.ToDecimal(txtNonTaxAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(txtTaxAmt.Text) Then
                txtTaxAmt.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(txtCostVatAmt.Text) Then
                txtCostVatAmt.Text = Math.Round(Convert.ToDecimal(txtCostVatAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(txtCommissionAmt.Text) Then
                txtCommissionAmt.Text = Math.Round(Convert.ToDecimal(txtCommissionAmt.Text), decimalPlaces).ToString
            End If
            If IsNumeric(txtVatPerc.Text) Then
                txtVatPerc.Text = Math.Round(Convert.ToDecimal(txtVatPerc.Text), decimalPlaces).ToString
            End If

            'changed by mohamed on 25/12/2021
            Dim txtNonTaxAmtBase As TextBox = CType(e.Row.FindControl("txtNonTaxAmtBase"), TextBox)
            Dim txtTaxAmtBase As TextBox = CType(e.Row.FindControl("txtTaxAmtBase"), TextBox)
            Dim txtCostVatAmtBase As TextBox = CType(e.Row.FindControl("txtCostVatAmtBase"), TextBox)
            txtNonTaxAmtBase.Text = Math.Round(Val(txtNonTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
            txtTaxAmtBase.Text = Math.Round(Val(txtTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
            txtCostVatAmtBase.Text = Math.Round(Val(txtCostVatAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)

            'If hdnPricedateFlag.Value = "Y" Then 'changed by  mohamed on 25/12/2021
            txtNonTaxAmt.Enabled = False
            txtTaxAmt.Enabled = False
            txtCostVatAmt.Enabled = False
            txtVatPerc.Enabled = False
            'End If

            CalculateTotal(e.Row)
        End If
    End Sub
#End Region
#Region " Private Sub CalculategvPricecostprice()"
    Private Sub CalculategvPricecostprice()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        ' If IsNumeric(txtcostprice.Text) Then
        ' 'txtcostprice.Text = Math.Round(Convert.ToDecimal(txtcostprice.Text), decimalPlaces).ToString()
        ' End If
        Dim total As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtcostprice As TextBox = CType(row.FindControl("txtcostprice"), TextBox)
            If IsNumeric(txtcostprice.Text) Then
                total = total + Convert.ToDecimal(txtcostprice.Text)
            End If


        Next
        Dim lblCostPriceTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostPriceTotal"), Label)
        lblCostPriceTotal.Text = Math.Round(Convert.ToDecimal(total), decimalPlaces).ToString()
        lblCostPriceTotal.Text = Math.Round(Convert.ToDecimal(lblCostPriceTotal.Text), decimalPlaces).ToString()

    End Sub
#End Region

#Region " Protected Sub txtcostprice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtcostprice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If hdServiceType.Value.Trim().ToLower() = "hotel" Then
            Dim decimalPlaces As Integer = Session("decimalPlaces")
            Dim txtcostprice As TextBox = CType(sender, TextBox)
            Dim gvr As GridViewRow = txtcostprice.NamingContainer
            Dim txtNonTaxAmount As TextBox = CType(gvr.FindControl("txtNonTaxAmount"), TextBox)
            Dim txtTaxAmount As TextBox = CType(gvr.FindControl("txtTaxAmount"), TextBox)
            Dim txtCostVatAmount As TextBox = CType(gvr.FindControl("txtCostVatAmount"), TextBox)

            If Val(txtcostprice.Text) > 0 Then
                Dim dt As DataTable
                dt = calculateVat(hdPriceRequestId.Value.Trim(), Val(hdPriceRlineNo.Value), txtSupplierCode.Text.Trim(), hdServiceType.Value.Trim(),
                             Convert.ToDecimal(Val(txtvatpercpop.Text)), Convert.ToDecimal(Val(txtcostprice.Text)))
                If dt.Rows.Count > 0 Then
                    txtNonTaxAmount.Text = Math.Round(dt.Rows(0)("nonTaxablevalue"), decimalPlaces)
                    txtTaxAmount.Text = Math.Round(dt.Rows(0)("taxableValue"), decimalPlaces)
                    txtCostVatAmount.Text = Math.Round(dt.Rows(0)("vatValue"), decimalPlaces)
                Else
                    txtNonTaxAmount.Text = 0
                    txtTaxAmount.Text = 0
                    txtCostVatAmount.Text = 0
                End If
            Else
                txtNonTaxAmount.Text = 0
                txtTaxAmount.Text = 0
                txtCostVatAmount.Text = 0
            End If
            calculategvPriceTotal()
        Else
            CalculategvPricecostprice()
        End If
        ModalExtraPopup.Show()
    End Sub
#End Region

#Region "Protected Sub calculategvPriceTotal()"
    Protected Sub calculategvPriceTotal()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim total As Decimal = 0.0
        Dim totalCostValue As Decimal = 0.0
        Dim totalTaxAmt As Decimal = 0.0
        Dim totalNonTaxAmt As Decimal = 0.0
        Dim totalCostVatAmt As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtcostprice As TextBox = CType(row.FindControl("txtcostprice"), TextBox)
            If IsNumeric(txtcostprice.Text) Then
                total = total + Convert.ToDecimal(txtcostprice.Text)
            End If
            Dim txtCostValue As TextBox = CType(row.FindControl("txtCostValue"), TextBox)
            If IsNumeric(txtCostValue.Text) Then
                totalCostValue = totalCostValue + Convert.ToDecimal(txtCostValue.Text)
            End If
            Dim txtNonTaxAmount As TextBox = CType(row.FindControl("txtNonTaxAmount"), TextBox)
            If IsNumeric(txtNonTaxAmount.Text) Then
                totalNonTaxAmt = totalNonTaxAmt + Convert.ToDecimal(txtNonTaxAmount.Text)
            End If
            Dim txtTaxAmount As TextBox = CType(row.FindControl("txtTaxAmount"), TextBox)
            If IsNumeric(txtTaxAmount.Text) Then
                totalTaxAmt = totalTaxAmt + Convert.ToDecimal(txtTaxAmount.Text)
            End If
            Dim txtCostVatAmount As TextBox = CType(row.FindControl("txtCostVatAmount"), TextBox)
            If IsNumeric(txtCostVatAmount.Text) Then
                totalCostVatAmt = totalCostVatAmt + Convert.ToDecimal(txtCostVatAmount.Text)
            End If
        Next
        Dim lblCostPriceTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostPriceTotal"), Label)
        lblCostPriceTotal.Text = Math.Round(Convert.ToDecimal(total), decimalPlaces).ToString()

        Dim lblCostValueTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostValueTotal"), Label)
        lblCostValueTotal.Text = Math.Round(Convert.ToDecimal(totalCostValue), decimalPlaces).ToString()

        Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
        lblNonTaxAmountTotal.Text = Math.Round(Convert.ToDecimal(totalNonTaxAmt), decimalPlaces).ToString()

        Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
        lblTaxAmountTotal.Text = Math.Round(Convert.ToDecimal(totalTaxAmt), decimalPlaces).ToString()

        Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
        lblCostVatAmountTotal.Text = Math.Round(Convert.ToDecimal(totalCostVatAmt), decimalPlaces).ToString()

    End Sub
#End Region

#Region "Protected Function calculateVat(ByVal requestid As String, ByVal lineno As Integer, ByVal partycode As String, ByVal serviceType As String, ByVal vatPerc As Decimal, ByVal costValue As Decimal) As DataTable"
    Protected Function calculateVat(ByVal requestid As String, ByVal lineno As Integer, ByVal partycode As String, ByVal serviceType As String, ByVal vatPerc As Decimal, ByVal costValue As Decimal) As DataTable
        Try
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand("sp_purchaseInvoice_calculate_vat", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = requestid
            mySqlCmd.Parameters.Add(New SqlParameter("@lineno", SqlDbType.Int)).Value = lineno
            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = partycode
            mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = serviceType
            mySqlCmd.Parameters.Add(New SqlParameter("@vatPerc", SqlDbType.Decimal)).Value = vatPerc
            mySqlCmd.Parameters.Add(New SqlParameter("@costvalue", SqlDbType.Decimal)).Value = costValue
            Dim dt As New DataTable
            Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                myDataAdapter.Fill(dt)
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
            Return dt
        Catch ex As Exception
            Return Nothing
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            ModalExtraPopup.Show()
        End Try
    End Function
#End Region

#Region "Protected Sub txtCostValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtCostValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim txtCostValue As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtCostValue.NamingContainer
        Dim txtNonTaxAmount As TextBox = CType(gvr.FindControl("txtNonTaxAmount"), TextBox)
        Dim txtTaxAmount As TextBox = CType(gvr.FindControl("txtTaxAmount"), TextBox)
        Dim txtCostVatAmount As TextBox = CType(gvr.FindControl("txtCostVatAmount"), TextBox)
        If Val(txtCostValue.Text) > 0 Then
            Dim dt As DataTable
            dt = calculateVat(hdPriceRequestId.Value.Trim(), Val(hdPriceRlineNo.Value), txtSupplierCode.Text.Trim(), hdServiceType.Value.Trim(),
                         Convert.ToDecimal(Val(txtvatpercpop.Text)), Convert.ToDecimal(Val(txtCostValue.Text)))
            If dt.Rows.Count > 0 Then
                txtNonTaxAmount.Text = Math.Round(dt.Rows(0)("nonTaxablevalue"), decimalPlaces)
                txtTaxAmount.Text = Math.Round(dt.Rows(0)("taxableValue"), decimalPlaces)
                txtCostVatAmount.Text = Math.Round(dt.Rows(0)("vatValue"), decimalPlaces)
            Else
                txtNonTaxAmount.Text = 0
                txtTaxAmount.Text = 0
                txtCostVatAmount.Text = 0
            End If
        Else
            txtNonTaxAmount.Text = 0
            txtTaxAmount.Text = 0
            txtCostVatAmount.Text = 0
        End If
        calculategvPriceTotal()
        ModalExtraPopup.Show()
    End Sub
#End Region

#Region "Protected Sub txtNonTaxAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtNonTaxAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtNonTaxAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtNonTaxAmt.NamingContainer
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtNonTaxAmt.Text) Then
            txtNonTaxAmt.Text = Math.Round(Convert.ToDecimal(txtNonTaxAmt.Text), decimalPlaces).ToString()
            'changed by mohamed on 25/12/2021
            Dim txtNonTaxAmtBase As TextBox = CType(gvr.FindControl("txtNonTaxAmtBase"), TextBox)
            txtNonTaxAmtBase.Text = Math.Round(Val(txtNonTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)

        End If
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)

        If lblServiceType.Text.ToLower = "tours" Or lblServiceType.Text.ToLower = "transfers" Or lblServiceType.Text.ToLower = "airportma" Or lblServiceType.Text.ToLower = "visa" Or lblServiceType.Text.ToLower = "others" Then
            CalculateTaxVatMainGv(gvr)
        End If
        CalculateTotal(gvr)
        calculateNetTotal()
    End Sub
#End Region

#Region "Protected Sub txtTaxAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtTaxAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtTaxAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtTaxAmt.NamingContainer
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtTaxAmt.Text) Then
            txtTaxAmt.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text), decimalPlaces).ToString()
            'changed by mohamed on 25/12/2021
            Dim txtTaxAmtBase As TextBox = CType(gvr.FindControl("txtTaxAmtBase"), TextBox)
            txtTaxAmtBase.Text = Math.Round(Val(txtTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)

        End If
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        If lblServiceType.Text.ToLower = "tours" Or lblServiceType.Text.ToLower = "transfers" Or lblServiceType.Text.ToLower = "airportma" Or lblServiceType.Text.ToLower = "visa" Or lblServiceType.Text.ToLower = "others" Then
            CalculateTaxMainGv(gvr)
        Else
            CalculateTax(gvr)
        End If
        CalculateTotal(gvr)
        calculateNetTotal()
    End Sub
#End Region

#Region "Protected Sub txtVatPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtVatPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtVatPerc As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtVatPerc.NamingContainer
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        If lblServiceType.Text.ToLower = "tours" Or lblServiceType.Text.ToLower = "transfers" Or lblServiceType.Text.ToLower = "airportma" Or lblServiceType.Text.ToLower = "visa" Or lblServiceType.Text.ToLower = "others" Then
            CalculateTaxMainGv(gvr)
        Else
            CalculateTax(gvr)
        End If
        CalculateTotal(gvr)
        calculateNetTotal()
    End Sub
#End Region

#Region "Protected Sub txtVatAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtVatAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtVatAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtVatAmt.NamingContainer
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)
        Dim txtRoomNo As TextBox = CType(gvr.FindControl("txtRoomNo"), TextBox)
        Dim lblPaxDetails As Label = CType(gvr.FindControl("lblPaxDetails"), Label)
        Dim lblCheckIn As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtVatAmt.Text) Then
            txtVatAmt.Text = Math.Round(Convert.ToDecimal(txtVatAmt.Text), decimalPlaces).ToString()
        End If
        If lblServiceType.Text.ToLower = "tours" Or lblServiceType.Text.ToLower = "transfers" Or lblServiceType.Text.ToLower = "airportma" Or lblServiceType.Text.ToLower = "visa" Then
            CalculatemaingvTaxAmount(gvr)
        End If
        'If gvPriceList.Rows.Count <> 0 Then
        '    ShowGVPricelistPopup(gvr)
        '    Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
        '    Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
        '    Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
        '    Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
        '    Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        '    txtNonTaxAmt.Text = lblNonTaxAmountTotal.Text
        '    txtTaxAmt.Text = lblTaxAmountTotal.Text
        'End If
        CalculateTotal(gvr)
        calculateNetTotal()
        hdCurrenttotalvatactual.Value = Convert.ToDecimal(txtVatAmt.Text)
    End Sub
#End Region

#Region "Protected Sub txtCommissionAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtCommissionAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtCommissionAmt As TextBox = CType(sender, TextBox)
        Dim gvr As GridViewRow = txtCommissionAmt.NamingContainer
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)
        Dim txtRoomNo As TextBox = CType(gvr.FindControl("txtRoomNo"), TextBox)
        Dim lblPaxDetails As Label = CType(gvr.FindControl("lblPaxDetails"), Label)
        Dim lblCheckIn As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtCommissionAmt.Text) Then
            txtCommissionAmt.Text = Math.Round(Convert.ToDecimal(txtCommissionAmt.Text), decimalPlaces).ToString()
        End If
        If lblServiceType.Text.ToLower = "tours" Or lblServiceType.Text.ToLower = "transfers" Or lblServiceType.Text.ToLower = "airportma" Or lblServiceType.Text.ToLower = "visa" Then
            CalculatemaingvTaxAmount(gvr)
        End If
        'If gvPriceList.Rows.Count <> 0 Then
        '    ShowGVPricelistPopup(gvr)
        '    Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
        '    Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
        '    Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
        '    Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
        '    Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        '    txtNonTaxAmt.Text = lblNonTaxAmountTotal.Text
        '    txtTaxAmt.Text = lblTaxAmountTotal.Text
        'End If
        CalculateTotal(gvr)
        calculateNetTotal()
        hdCurrenttotalCommissionactual.Value = Convert.ToDecimal(txtCommissionAmt.Text)
    End Sub
#End Region

#Region "Protected Sub CalculateTotal(ByVal gvr As GridViewRow)"
    Protected Sub CalculateTotal(ByVal gvr As GridViewRow)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim txtNonTaxAmt As TextBox = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
        Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        Dim txtCostVatAmt As TextBox = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
        Dim txtCommisionAmt As TextBox = CType(gvr.FindControl("txtCommisionAmt"), TextBox)
        Dim txtTotalAmt As TextBox = CType(gvr.FindControl("txtTotalAmt"), TextBox)
        Dim lbtnActualAmt As LinkButton = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
        Dim total As Decimal = 0
        If IsNumeric(txtNonTaxAmt.Text) Then
            total = total + Convert.ToDecimal(txtNonTaxAmt.Text)
        End If
        If IsNumeric(txtTaxAmt.Text) Then
            total = total + Convert.ToDecimal(txtTaxAmt.Text)
        End If
        If IsNumeric(txtCostVatAmt.Text) Then
            total = total + Convert.ToDecimal(txtCostVatAmt.Text)
        End If
        'If IsNumeric(txtCommisionAmt.Text) Then
        '    total = total + Convert.ToDecimal(txtCommisionAmt.Text)
        'End If
        txtTotalAmt.Text = Math.Round(Convert.ToDecimal(lbtnActualAmt.Text), decimalPlaces).ToString() 'rosalin 30/11/2023
        'Math.Round(total, decimalPlaces).ToString()

        'changed by mohamed on 25/12/2021
        Dim totalBase As Decimal = 0
        Dim txtTotalAmtBase As TextBox = CType(gvr.FindControl("txtTotalAmtBase"), TextBox)
        Dim txtNonTaxAmtBase As TextBox = CType(gvr.FindControl("txtNonTaxAmtBase"), TextBox)
        Dim txtTaxAmtBase As TextBox = CType(gvr.FindControl("txtTaxAmtBase"), TextBox)
        Dim txtCostVatAmtBase As TextBox = CType(gvr.FindControl("txtCostVatAmtBase"), TextBox)

        If IsNumeric(txtNonTaxAmtBase.Text) Then
            totalBase = totalBase + Convert.ToDecimal(txtNonTaxAmtBase.Text)
        End If
        If IsNumeric(txtTaxAmtBase.Text) Then
            totalBase = totalBase + Convert.ToDecimal(txtTaxAmtBase.Text)
        End If
        If IsNumeric(txtCostVatAmtBase.Text) Then
            totalBase = totalBase + Convert.ToDecimal(txtCostVatAmtBase.Text)
        End If
        txtTotalAmtBase.Text = Math.Round(Convert.ToDecimal(lbtnActualAmt.Text), decimalPlaces).ToString() 'rosalin 30/11/2023
        'Math.Round(totalBase, decimalPlaces).ToString()
    End Sub
#End Region

#Region "Protected Sub CalculateTax(ByVal gvr As GridViewRow)"
    Protected Sub CalculateTax(ByVal gvr As GridViewRow)
        Dim txtTaxAmt As TextBox = CType(gvr.FindControl("txtTaxAmt"), TextBox)
        Dim txtVatPerc As TextBox = CType(gvr.FindControl("txtVatPerc"), TextBox)
        Dim txtCostVatAmt As TextBox = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)

        Dim lbtnActualAmt As LinkButton = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If IsNumeric(txtTaxAmt.Text) And IsNumeric(txtVatPerc.Text) Then 'Commented 11/12/2018
            If lblServiceType.Text.ToLower = "tours" Or lblServiceType.Text.ToLower = "transfers" Or lblServiceType.Text.ToLower = "airportma" Or lblServiceType.Text.ToLower = "visa" Then
                txtTaxAmt.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text) / (1 + (Convert.ToDecimal(txtVatPerc.Text) / 100)), decimalPlaces).ToString()
                txtCostVatAmt.Text = Convert.ToDecimal(lbtnActualAmt.Text) - Convert.ToDecimal(txtTaxAmt.Text)

            Else
                txtCostVatAmt.Text = Math.Round(Convert.ToDecimal(txtTaxAmt.Text) * Convert.ToDecimal(txtVatPerc.Text) / 100, decimalPlaces).ToString()

            End If
            'changed by mohamed on 25/12/2021
            Dim txtTaxAmtBase As TextBox = CType(gvr.FindControl("txtTaxAmtBase"), TextBox)
            Dim txtCostVatAmtBase As TextBox = CType(gvr.FindControl("txtCostVatAmtBase"), TextBox)
            txtTaxAmtBase.Text = Math.Round(Val(txtTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)
            txtCostVatAmtBase.Text = Math.Round(Val(txtCostVatAmt.Text) * Convert.ToDecimal(txtConvRate.Text), decimalPlaces)

        End If
    End Sub
#End Region

#Region "Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click"
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        calculateNetTotal()
        ModalExtraPopup.Hide()
    End Sub
#End Region

#Region " Private Sub CalculategvCostpriceTotal()"
    Private Sub CalculategvCostpriceTotal()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        Dim totalcp As Decimal = 0.0
        For Each row As GridViewRow In gvPriceList.Rows
            Dim txtcostprice As TextBox = CType(row.FindControl("txtcostprice"), TextBox)
            If IsNumeric(txtcostprice.Text) Then
                totalcp = totalcp + Convert.ToDecimal(txtcostprice.Text)
            End If
        Next
        Dim lblCostPriceTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostPriceTotal"), Label)
        lblCostPriceTotal.Text = Math.Round(Convert.ToDecimal(totalcp), decimalPlaces).ToString()
    End Sub
#End Region

#Region "Protected Sub btnfillCostPricepopup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillCostPricepopup.Click"
    Protected Sub btnfillCostPricepopup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillCostPricepopup.Click

        ModalExtraPopup.Show()
        Dim decimalPlaces As Integer = Session("decimalPlaces")
        If ddlfillamount.SelectedValue = "CostPrice" Then
            For Each row As GridViewRow In gvPriceList.Rows
                Dim txtCostPrice As TextBox
                txtCostPrice = CType(row.FindControl("txtCostPrice"), TextBox)
                txtCostPrice.Text = Math.Round(Convert.ToDecimal(Val(txtfillAmount.Text)), decimalPlaces)
            Next
            CalculategvCostpriceTotal()
        ElseIf ddlfillamount.SelectedValue = "NonTaxableAmount" Then
            For Each row As GridViewRow In gvPriceList.Rows
                Dim txtNonTaxAmount As TextBox
                txtNonTaxAmount = CType(row.FindControl("txtNonTaxAmount"), TextBox)
                txtNonTaxAmount.Text = Math.Round(Convert.ToDecimal(Val(txtfillAmount.Text)), decimalPlaces)
            Next
            CalculategvNonTaxAmount()
        ElseIf ddlfillamount.SelectedValue = "TaxableAmount" Then
            Dim txtTaxAmount As TextBox
            For Each row In gvPriceList.Rows
                txtTaxAmount = CType(row.FindControl("txtTaxAmount"), TextBox)
                txtTaxAmount.Text = Math.Round(Convert.ToDecimal(Val(txtfillAmount.Text)), decimalPlaces)
            Next
            CalculategvTaxAmountTotal()
        ElseIf ddlfillamount.SelectedValue = "VatAmount" Then
            Dim txtCostVatAmount As TextBox
            For Each row In gvPriceList.Rows
                txtCostVatAmount = CType(row.FindControl("txtCostVatAmount"), TextBox)
                txtCostVatAmount.Text = Math.Round(Convert.ToDecimal(Val(txtfillAmount.Text)), decimalPlaces)
            Next
            CalculategvVatAmount()
        ElseIf ddlfillamount.SelectedValue = "Commission" Then

            Dim txtCommissionAmount As TextBox
            For Each row In gvPriceList.Rows
                txtCommissionAmount = CType(row.FindControl("txtCommissionAmount"), TextBox)
                txtCommissionAmount.Text = Math.Round(Convert.ToDecimal(Val(txtfillAmount.Text)), decimalPlaces)
            Next
            CalculategvCommissionAmount()
        End If
        ' CalculategvPricecostprice()
    End Sub
#End Region
#Region "    Protected Function ValidationPriceBreakup() As Boolean"
    Protected Function ValidationPriceBreakup(ByVal ServiceType As String) As Boolean

        Dim decimalPlaces As Integer = Session("decimalPlaces")
        For i = 0 To gvPriceList.Rows.Count - 1
            Dim txtCostValue As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostValue"), TextBox)
            Dim txtCostPrice As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostPrice"), TextBox)
            Dim txtTaxAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtTaxAmount"), TextBox)
            Dim txtNonTaxAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtNonTaxAmount"), TextBox)
            Dim txtCostVatAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCostVatAmount"), TextBox)
            Dim txtCommissionAmount As TextBox = CType(gvPriceList.Rows(i).FindControl("txtCommissionAmount"), TextBox)
            ValidationPriceBreakup = False
            If ServiceType.ToLower() = "hotel" Then
                If Convert.ToDecimal(txtNonTaxAmount.Text) > Convert.ToDecimal(txtCostPrice.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('NonTaxable Amount is more than Cost Price please check Row- " & i + 1 & "');", True)
                    ValidationPriceBreakup = True
                    Exit Function
                End If
                If Convert.ToDecimal(txtTaxAmount.Text) > Convert.ToDecimal(txtCostPrice.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Taxable Amount is more than Cost Price please check Row- " & i + 1 & "');", True)
                    ValidationPriceBreakup = True
                    Exit Function
                End If
                If Convert.ToDecimal(txtCostPrice.Text) <> Convert.ToDecimal(txtNonTaxAmount.Text) + Convert.ToDecimal(txtTaxAmount.Text) + Convert.ToDecimal(txtCostVatAmount.Text) Then
                    'ModalExtraPopup.Show()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('CostPrice does not match with Total Amount Please check Row- " & i + 1 & "');", True)
                    ValidationPriceBreakup = True
                    Exit Function
                End If
            ElseIf ServiceType.ToLower() = "special event" Then
                If Convert.ToDecimal(txtNonTaxAmount.Text) > Convert.ToDecimal(txtCostValue.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('NonTaxable Amount is more than Cost Price please check Row- " & i + 1 & "');", True)
                    ValidationPriceBreakup = True
                    Exit Function
                End If
                If Convert.ToDecimal(txtTaxAmount.Text) > Convert.ToDecimal(txtCostValue.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Taxable Amount is more than Cost Price please check Row- " & i + 1 & "');", True)
                    ValidationPriceBreakup = True
                    Exit Function
                End If
                If Convert.ToDecimal(txtCostValue.Text) <> Convert.ToDecimal(txtNonTaxAmount.Text) + Convert.ToDecimal(txtTaxAmount.Text) + Convert.ToDecimal(txtCostVatAmount.Text) Then
                    'ModalExtraPopup.Show()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('txtCostValue does not match with Total Amount Please check Row- " & i + 1 & "');", True)
                    ValidationPriceBreakup = True
                    Exit Function
                End If
            End If
        Next

        Dim lblCostPriceTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostPriceTotal"), Label)
        Dim lblCostValueTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostValueTotal"), Label)
        Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
        Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
        Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)

        If ServiceType.ToLower() = "hotel" Then
            If Convert.ToDecimal(Val(lblCostPriceTotal.Text)) <> Convert.ToDecimal((Val(lblNonTaxAmountTotal.Text) + Val(lblTaxAmountTotal.Text) + Val(lblCostVatAmountTotal.Text))) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total cost price does not match with sum of total taxable amount + total non taxable amount + vat amount');", True)
                ValidationPriceBreakup = True
                Exit Function
            End If
        Else
            If Convert.ToDecimal(Val(lblCostValueTotal.Text)) <> Convert.ToDecimal((Val(lblNonTaxAmountTotal.Text) + Val(lblTaxAmountTotal.Text) + Val(lblCostVatAmountTotal.Text))) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total cost value does not match with sum of total taxable amount + total non taxable amount + vat amount');", True)
                ValidationPriceBreakup = True
                Exit Function
            End If

        End If

        'If Convert.ToDecimal(lblCostPriceTotal.Text) <> Convert.ToDecimal(lblNonTaxAmountTotal.Text) + Convert.ToDecimal(lblTaxAmountTotal.Text) + Convert.ToDecimal(lblCostVatAmountTotal.Text) Then
        '    'ModalExtraPopup.Show()
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('CostPrice Total and Total Amount are Not Equal Please Check ');", True)
        '    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('CostPrice doesn't match total amount please check' );", True)
        '    ValidationPriceBreakup = True
        '    Exit Function
        'End If
        ValidationPriceBreakup = False
    End Function
#End Region
#Region "Protected Sub btnPriceSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriceSave.Click"
    Protected Sub btnPriceSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriceSave.Click
        Try
            If ValidationPriceBreakup(hdServiceType.Value) = True Then

                ModalExtraPopup.Show()
                Exit Sub
            End If

            Dim dt As New DataTable
            dt.Columns.Add("divcode", GetType(String))
            dt.Columns.Add("purchaseinvoiceno", GetType(String))
            dt.Columns.Add("purchaseinvoicetype", GetType(String))
            dt.Columns.Add("paxtype", GetType(String))
            dt.Columns.Add("noofpax", GetType(Integer))
            dt.Columns.Add("childages", GetType(String))
            dt.Columns.Add("requestId", GetType(String))
            dt.Columns.Add("rlineno", GetType(Integer))
            dt.Columns.Add("roomno", GetType(Integer))
            dt.Columns.Add("servicetype", GetType(String))
            dt.Columns.Add("pricedate", GetType(Date))
            dt.Columns.Add("bookingcode", GetType(String))
            dt.Columns.Add("bookingname", GetType(String))
            dt.Columns.Add("rate", GetType(Decimal))
            dt.Columns.Add("currcode", GetType(String))
            dt.Columns.Add("saleprice", GetType(Decimal))
            dt.Columns.Add("salepricebase", GetType(Decimal))
            dt.Columns.Add("costprice", GetType(Decimal))
            dt.Columns.Add("costvalue", GetType(Decimal))
            dt.Columns.Add("CostTaxableValue", GetType(Decimal))
            dt.Columns.Add("CostNonTaxableValue", GetType(Decimal))
            dt.Columns.Add("CostVatValue", GetType(Decimal))
            dt.Columns.Add("Commission", GetType(Decimal))
            Dim lblstayDate As Label
            Dim lblRate As Label
            Dim lblBookingCode As Label
            Dim lblBookingName As Label
            Dim txtcurrcode As TextBox
            Dim lblSalePrice As Label
            Dim lblSalePriceBase As Label
            Dim lblPaxType As Label
            Dim lblNoOfPax As Label
            Dim lblChildAges As Label
            Dim txtCostPrice As TextBox
            Dim txtTaxAmount As TextBox
            Dim txtNonTaxAmount As TextBox
            Dim txtCostVatAmount As TextBox
            Dim txtCommissionAmount As TextBox
            Dim txtCostValue As TextBox

            For Each gvr As GridViewRow In gvPriceList.Rows
                lblstayDate = CType(gvr.FindControl("lblstayDate"), Label)
                lblBookingCode = CType(gvr.FindControl("lblBookingCode"), Label)
                lblBookingName = CType(gvr.FindControl("lblBookingName"), Label)
                txtcurrcode = CType(gvr.FindControl("txtcurrcode"), TextBox)
                lblSalePrice = CType(gvr.FindControl("lblSalePrice"), Label)
                lblSalePriceBase = CType(gvr.FindControl("lblSalePriceBase"), Label)
                lblPaxType = CType(gvr.FindControl("lblPaxType"), Label)
                lblRate = CType(gvr.FindControl("lblRate"), Label)
                lblNoOfPax = CType(gvr.FindControl("lblNoOfPax"), Label)
                lblChildAges = CType(gvr.FindControl("lblChildAges"), Label)
                txtCostPrice = CType(gvr.FindControl("txtCostPrice"), TextBox)
                txtCostValue = CType(gvr.FindControl("txtCostValue"), TextBox)
                txtTaxAmount = CType(gvr.FindControl("txtTaxAmount"), TextBox)
                txtNonTaxAmount = CType(gvr.FindControl("txtNonTaxAmount"), TextBox)
                txtCostVatAmount = CType(gvr.FindControl("txtCostVatAmount"), TextBox)
                txtCommissionAmount = CType(gvr.FindControl("txtCommissionAmount"), TextBox)
                txtcurrcode = CType(gvr.FindControl("txtcurrcode"), TextBox)

                Dim dr As DataRow = dt.NewRow

                'dr("div_code") = txtDivcode.Text
                'dr("purchaseinvoicetype") = "S"
                dr("requestid") = hdPriceRequestId.Value
                dr("rlineno") = hdPriceRlineNo.Value
                dr("roomno") = hdPriceRoomNo.Value
                dr("servicetype") = hdServiceType.Value
                dr("pricedate") = Convert.ToDateTime(lblstayDate.Text)
                dr("bookingcode") = lblBookingCode.Text
                If hdServiceType.Value.ToLower = "Hotel" Then
                    dr("bookingname") = lblBookingName.Text
                Else

                    dr("bookingname") = lblBookingName.Text
                End If
                dr("paxtype") = lblPaxType.Text
                dr("noofpax") = lblNoOfPax.Text
                dr("childages") = lblChildAges.Text
                dr("currcode") = txtcurrcode.Text
                dr("rate") = lblRate.Text
                If IsNumeric(lblSalePrice.Text) Then
                    dr("saleprice") = Convert.ToDecimal(lblSalePrice.Text)
                End If
                If IsNumeric(lblSalePriceBase.Text) Then
                    dr("salepricebase") = Convert.ToDecimal(lblSalePriceBase.Text)
                End If
                If IsNumeric(txtCostPrice.Text) Then
                    dr("costprice") = Convert.ToDecimal(txtCostPrice.Text)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost price should be numeric');", True)
                    Exit Sub
                End If
                If IsNumeric(txtCostValue.Text) Then
                    dr("costvalue") = Convert.ToDecimal(txtCostValue.Text)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost Value should be numeric');", True)
                    Exit Sub
                End If
                If IsNumeric(txtTaxAmount.Text) Then
                    dr("CostTaxableValue") = Convert.ToDecimal(txtTaxAmount.Text)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Taxable Amount should be numeric');", True)
                    Exit Sub
                End If
                If IsNumeric(txtNonTaxAmount.Text) Then
                    dr("CostNonTaxableValue") = Convert.ToDecimal(txtNonTaxAmount.Text)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('NonTaxable Amount should be numeric');", True)
                    Exit Sub
                End If
                If IsNumeric(txtCostVatAmount.Text) Then
                    dr("CostVatValue") = Convert.ToDecimal(txtCostVatAmount.Text)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Vat Amount should be numeric');", True)
                    Exit Sub
                End If
                If IsNumeric(txtCommissionAmount.Text) Then
                    dr("Commission") = Convert.ToDecimal(txtCommissionAmount.Text)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Commission Value should be numeric');", True)
                    Exit Sub
                End If
                dt.Rows.Add(dr)
            Next
            Dim decimalPlaces As Integer = Session("decimalPlaces")
            If dt.Rows.Count > 0 Then
                'If hdServiceType.Value.ToLower = "hotel" Then
                Dim actualAmount As Decimal
                If hdServiceType.Value.ToLower = "hotel" Then
                    actualAmount = Math.Round(dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("costprice")), decimalPlaces)
                Else
                    actualAmount = Math.Round(dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("costvalue")), decimalPlaces)
                End If
                Dim svtax As Decimal
                Dim mptax As Decimal
                Dim tourismtax As Decimal
                Dim vattax As Decimal
                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                strSqlQry = "select Partycode,partyname, isnull(serviceChargePerc,0)as serviceChargePerc,isnull(MunicipalityFeePerc,0) as MunicipalityFeePerc,isnull(TourismFeePerc,0) as TourismFeePerc,isnull(VatPerc,0)as VatPerc from partymast " &
                "where partycode=@partyCode"
                mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
                mySqlCmd.CommandType = CommandType.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@partyCode", SqlDbType.VarChar, 20)).Value = txtSupplierCode.Text.Trim
                Dim mySqlReader As SqlDataReader
                mySqlReader = mySqlCmd.ExecuteReader()
                If mySqlReader.Read() Then
                    svtax = Convert.ToDecimal(mySqlReader("serviceChargePerc"))
                    mptax = Convert.ToDecimal(mySqlReader("MunicipalityFeePerc"))
                    tourismtax = Convert.ToDecimal(mySqlReader("TourismFeePerc"))
                    vattax = Convert.ToDecimal(mySqlReader("VatPerc"))
                End If
                mySqlReader.Close()

                Dim dtTax As New DataTable
                mySqlCmd = New SqlCommand("select * from dbo.FN_Calculate_Taxvalue_rsv(@svtax,@mptax,@vattax,@tourismtax,@cprice,@price,@agentcode)", sqlConn)
                mySqlCmd.CommandType = CommandType.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@svtax", SqlDbType.Decimal)).Value = svtax
                mySqlCmd.Parameters.Add(New SqlParameter("@mptax", SqlDbType.Decimal)).Value = mptax
                mySqlCmd.Parameters.Add(New SqlParameter("@vattax", SqlDbType.Decimal)).Value = vattax
                mySqlCmd.Parameters.Add(New SqlParameter("@tourismtax", SqlDbType.Decimal)).Value = tourismtax
                mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal)).Value = actualAmount
                mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal)).Value = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtSupplierCode.Text.Trim
                Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                    myDataAdapter.Fill(dtTax)
                End Using
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
                Dim pdt As DataTable = CType(Session("priceListDt"), DataTable)
                Dim filterPdr = (From n In pdt.AsEnumerable Where n.Field(Of String)("requestId") = hdPriceRequestId.Value And
                                    n.Field(Of Integer)("rlineno") = hdPriceRlineNo.Value And n.Field(Of Integer)("roomno") = hdPriceRoomNo.Value And n.Field(Of String)("servicetype") = hdServiceType.Value Select n)
                If filterPdr.Count > 0 Then
                    For i = filterPdr.Count - 1 To 0 Step -1
                        filterPdr(i).Delete()
                    Next
                    pdt.AcceptChanges()
                End If
                pdt.Merge(dt)

                'savePricebreakup(pdt)
                'pdt.Columns.Add("userlogged", GetType(String))

                Session("priceListDt") = pdt
                Dim lblTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblTaxAmountTotal"), Label)
                Dim lblNonTaxAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblNonTaxAmountTotal"), Label)
                Dim lblCostVatAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCostVatAmountTotal"), Label)
                Dim lblCommissionAmountTotal As Label = CType(gvPriceList.FooterRow.FindControl("lblCommissionAmountTotal"), Label)
                Dim costDt As DataTable = MakeDataTable()
                If Not costDt Is Nothing Then
                    Dim filterdr = (From n In costDt.AsEnumerable Where n.Field(Of String)("requestId") = hdPriceRequestId.Value And
                                    n.Field(Of Integer)("rlineno") = hdPriceRlineNo.Value And n.Field(Of Integer)("roomno") = hdPriceRoomNo.Value And n.Field(Of String)("servicetype") = hdServiceType.Value And n.Field(Of String)("PaxType") = HdnPaxType.Value Select n)
                    If filterdr.Count > 1 Then
                        Dim tmpdt As DataTable = CType(Session("SupplierInvoice"), DataTable).Clone
                        gvUpdateSupplier.DataSource = tmpdt
                        gvUpdateSupplier.DataBind()
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Duplicate rows exist');", True)
                        Exit Sub
                    ElseIf filterdr.Count = 1 Then
                        If dtTax.Rows.Count > 0 Then
                            filterdr(0)("selection") = True
                            filterdr(0)("actualamount") = actualAmount
                            filterdr(0)("prices_costnontaxablevaluebase") = Convert.ToDecimal(lblNonTaxAmountTotal.Text)  'pdt.Compute("sum(CostNonTaxableValue)", "roomno = " & hdPriceRoomNo.Value & " and ") 'dtTax(0)("nontaxablevalue")
                            filterdr(0)("prices_costtaxablevaluebase") = Convert.ToDecimal(lblTaxAmountTotal.Text)  'pdt.Compute("sum(CostTaxableValue)", String.Empty) ' dtTax(0)("taxablevalue")
                            filterdr(0)("prices_costvatvaluebase") = Convert.ToDecimal(lblCostVatAmountTotal.Text) 'pdt.Compute("sum(CostVatValue)", String.Empty) 'dtTax(0)("vatvalue")
                            filterdr(0)("Commission") = Convert.ToDecimal(lblCommissionAmountTotal.Text)
                            filterdr(0)("totalprice") = Convert.ToDecimal(lblNonTaxAmountTotal.Text) + Convert.ToDecimal(lblTaxAmountTotal.Text) + Convert.ToDecimal(lblCostVatAmountTotal.Text) ' Math.Round(pdt.Compute("sum(CostNonTaxableValue)", String.Empty) + pdt.Compute("sum(CostTaxableValue)", String.Empty) + pdt.Compute("sum(CostVatValue)", String.Empty), decimalPlaces)
                            filterdr(0)("vatperc") = txtvatpercpop.Text
                            filterdr(0).AcceptChanges()
                        End If
                    End If
                    gvUpdateSupplier.DataSource = costDt
                    gvUpdateSupplier.DataBind()
                    calculateNetTotal()
                End If
            End If
            ModalExtraPopup.Hide()
        Catch ex As Exception

            Dim dt As DataTable = CType(Session("SupplierInvoice"), DataTable).Clone
            gvUpdateSupplier.DataSource = dt
            gvUpdateSupplier.DataBind()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
#End Region

#Region "Protected Function MakeDataTable() As DataTable"
    Protected Function MakeDataTable() As DataTable
        Try
            Dim dt As DataTable = CType(Session("SupplierInvoice"), DataTable).Clone
            Dim chkSelection As CheckBox
            Dim lblRequestId As Label
            Dim txtRlineNo As TextBox
            Dim txtRoomNo As TextBox
            Dim lblCheckIn As Label
            Dim lblCheckOut As Label
            Dim lblPaxType As Label
            Dim lblPax As Label
            Dim lblService As Label
            Dim lblSupConfNo As Label
            Dim lblServiceType As Label
            Dim lblProvAmt As Label
            Dim lblVatInputProvAmt As Label
            Dim lbtnActualAmt As LinkButton
            Dim txtNonTaxAmt As TextBox
            Dim txtTaxAmt As TextBox
            Dim txtVatPerc As TextBox
            Dim txtVatAmt As TextBox
            Dim txtTotalAmt As TextBox
            Dim txtCommisionAmt As TextBox
            For Each gvr As GridViewRow In gvUpdateSupplier.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                lblRequestId = CType(gvr.FindControl("lblRequestId"), Label)
                lblPaxType = CType(gvr.FindControl("lblPaxType"), Label)
                txtRlineNo = CType(gvr.FindControl("txtRlineNo"), TextBox)
                txtRoomNo = CType(gvr.FindControl("txtRoomNo"), TextBox)
                lblCheckIn = CType(gvr.FindControl("lblCheckIn"), Label)
                lblCheckOut = CType(gvr.FindControl("lblCheckOut"), Label)
                lblPax = CType(gvr.FindControl("lblPax"), Label)
                lblService = CType(gvr.FindControl("lblService"), Label)
                lblServiceType = CType(gvr.FindControl("lblServiceType"), Label)
                lblSupConfNo = CType(gvr.FindControl("lblSupConfNo"), Label)
                lblProvAmt = CType(gvr.FindControl("lblProvAmt"), Label)
                lblVatInputProvAmt = CType(gvr.FindControl("lblVatInputProvAmt"), Label)
                lbtnActualAmt = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
                txtNonTaxAmt = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
                txtTaxAmt = CType(gvr.FindControl("txtTaxAmt"), TextBox)
                txtVatPerc = CType(gvr.FindControl("txtVatPerc"), TextBox)
                txtVatAmt = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
                txtTotalAmt = CType(gvr.FindControl("txtTotalAmt"), TextBox)
                txtCommisionAmt = CType(gvr.FindControl("txtCommisionAmt"), TextBox)
                Dim dr As DataRow = dt.NewRow
                dr("selection") = chkSelection.Checked
                dr("requestId") = lblRequestId.Text.Trim
                If IsNumeric(txtRlineNo.Text) Then
                    dr("rlineno") = Convert.ToInt32(txtRlineNo.Text)
                End If
                If IsNumeric(txtRoomNo.Text) Then
                    dr("roomno") = Convert.ToInt32(txtRoomNo.Text)
                End If
                If IsDate(lblCheckIn.Text) Then
                    dr("checkin") = Convert.ToDateTime(lblCheckIn.Text)
                End If
                If IsDate(lblCheckOut.Text) Then
                    dr("checkout") = Convert.ToDateTime(lblCheckOut.Text)
                End If
                dr("paxdetails") = lblPax.Text.Trim
                dr("PaxType") = lblPaxType.Text.Trim
                dr("servicedetails") = lblService.Text.Trim
                dr("supconfno") = lblSupConfNo.Text.Trim
                dr("servicetype") = lblServiceType.Text.Trim
                If IsNumeric(lblProvAmt.Text) Then
                    dr("provisionamount") = Convert.ToDecimal(lblProvAmt.Text)
                End If
                If IsNumeric(lblVatInputProvAmt.Text) Then
                    dr("vatprovision") = Convert.ToDecimal(lblVatInputProvAmt.Text)
                End If
                If IsNumeric(lbtnActualAmt.Text) Then
                    dr("actualamount") = Convert.ToDecimal(lbtnActualAmt.Text)
                End If
                If IsNumeric(txtNonTaxAmt.Text) Then
                    dr("prices_costnontaxablevaluebase") = Convert.ToDecimal(txtNonTaxAmt.Text)
                End If
                If IsNumeric(txtTaxAmt.Text) Then
                    dr("prices_costtaxablevaluebase") = Convert.ToDecimal(txtTaxAmt.Text)
                End If
                If IsNumeric(txtVatPerc.Text) Then
                    dr("vatperc") = Convert.ToDecimal(txtVatPerc.Text)
                End If
                If IsNumeric(txtVatAmt.Text) Then
                    dr("prices_costvatvaluebase") = Convert.ToDecimal(txtVatAmt.Text)
                End If
                If IsNumeric(txtTotalAmt.Text) Then
                    dr("totalprice") = Convert.ToDecimal(txtTotalAmt.Text)
                End If
                If IsNumeric(txtCommisionAmt.Text) Then
                    dr("Commission") = Convert.ToDecimal(txtCommisionAmt.Text)
                End If
                dt.Rows.Add(dr)
            Next
            MakeDataTable = dt
        Catch ex As Exception
            MakeDataTable = Nothing
            Throw ex
        End Try
    End Function
#End Region


#Region "Protected Sub gvPriceList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPriceList.RowDataBound"
    Protected Sub gvPriceList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPriceList.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = Session("decimalPlaces")

            Dim lblRate As Label = CType(e.Row.FindControl("lblRate"), Label)
            Dim lblSalePrice As Label = CType(e.Row.FindControl("lblSalePrice"), Label)
            Dim lblbookingname As Label = CType(e.Row.FindControl("lblbookingname"), Label)
            Dim lblbookingcode As Label = CType(e.Row.FindControl("lblbookingcode"), Label)
            Dim lblSalePriceBase As Label = CType(e.Row.FindControl("lblSalePriceBase"), Label)
            Dim txtCostPrice As TextBox = CType(e.Row.FindControl("txtCostPrice"), TextBox)
            Dim txtCostValue As TextBox = CType(e.Row.FindControl("txtCostValue"), TextBox)
            Dim txtNonTaxAmount As TextBox = CType(e.Row.FindControl("txtNonTaxAmount"), TextBox)
            Dim txtTaxAmount As TextBox = CType(e.Row.FindControl("txtTaxAmount"), TextBox)
            Dim txtCostVatAmount As TextBox = CType(e.Row.FindControl("txtCostVatAmount"), TextBox)
            Dim txtCommissionAmount As TextBox = CType(e.Row.FindControl("txtCommissionAmount"), TextBox)
            If hdServiceType.Value.ToLower = "hotel" Then
                lblbookingcode.Style.Add("display", "block")
                lblbookingname.Style.Add("display", "none")
            Else
                lblbookingcode.Style.Add("display", "none")
                lblbookingname.Style.Add("display", "block")
            End If
            If IsNumeric(lblRate.Text) Then
                lblRate.Text = Math.Round(Convert.ToDecimal(lblRate.Text), decimalPlaces)
            End If
            If IsNumeric(txtCostValue.Text) Then
                txtCostValue.Text = Math.Round(Convert.ToDecimal(txtCostValue.Text), decimalPlaces)
            End If
            If IsNumeric(lblSalePrice.Text) Then
                lblSalePrice.Text = Math.Round(Convert.ToDecimal(lblSalePrice.Text), decimalPlaces)
            End If
            If IsNumeric(lblSalePriceBase.Text) Then
                lblSalePriceBase.Text = Math.Round(Convert.ToDecimal(lblSalePriceBase.Text), decimalPlaces)
            End If
            If IsNumeric(txtCostPrice.Text) Then
                txtCostPrice.Text = Math.Round(Convert.ToDecimal(txtCostPrice.Text), decimalPlaces)
            End If
            If IsNumeric(txtNonTaxAmount.Text) Then
                txtNonTaxAmount.Text = Math.Round(Convert.ToDecimal(txtNonTaxAmount.Text), decimalPlaces)
            End If
            If IsNumeric(txtTaxAmount.Text) Then
                txtTaxAmount.Text = Math.Round(Convert.ToDecimal(txtTaxAmount.Text), decimalPlaces)
            End If
            If IsNumeric(txtCostVatAmount.Text) Then
                txtCostVatAmount.Text = Math.Round(Convert.ToDecimal(txtCostVatAmount.Text), decimalPlaces)
            End If
            If IsNumeric(txtCommissionAmount.Text) Then
                txtCommissionAmount.Text = Math.Round(Convert.ToDecimal(txtCommissionAmount.Text), decimalPlaces)
            End If
        End If
    End Sub
#End Region

#Region "Protected Sub calculateNetTotal()"
    Protected Sub calculateNetTotal()

        If gvUpdateSupplier.Rows.Count > 0 Then
            Dim chkSelection As CheckBox
            Dim lblProvAmt As Label
            Dim lblVatInputProvAmt As Label
            Dim lbtnActualAmt As LinkButton
            Dim txtNonTaxAmt As TextBox
            Dim txtTaxAmt As TextBox
            Dim txtVatAmt As TextBox
            Dim txtTotalAmt As TextBox
            Dim txtCommisionAmt As TextBox
            Dim lblServiceType As Label 'changed by mohamed on 25/12/2021

            Dim TotalProvAmt As Decimal = 0.0
            Dim TotalVatInputProvAmt As Decimal = 0.0
            Dim TotalActualAmt As Decimal = 0.0
            Dim TotalNonTaxAmt As Decimal = 0.0
            Dim TotalTaxAmt As Decimal = 0.0
            Dim TotalVatAmt As Decimal = 0.0
            Dim NetTotalAmt As Decimal = 0.0
            Dim NetCommissionAmt As Decimal = 0.0
            For Each gvr As GridViewRow In gvUpdateSupplier.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked Then
                    lblProvAmt = CType(gvr.FindControl("lblProvAmt"), Label)
                    lblVatInputProvAmt = CType(gvr.FindControl("lblVatInputProvAmt"), Label)
                    lbtnActualAmt = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
                    txtNonTaxAmt = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
                    txtTaxAmt = CType(gvr.FindControl("txtTaxAmt"), TextBox)
                    txtVatAmt = CType(gvr.FindControl("txtCostVatAmt"), TextBox)
                    txtTotalAmt = CType(gvr.FindControl("txtTotalAmt"), TextBox)
                    txtCommisionAmt = CType(gvr.FindControl("txtCommisionAmt"), TextBox)
                    lblServiceType = CType(gvr.FindControl("lblServiceType"), Label) 'changed by mohamed on 25/12/2021

                    If IsNumeric(lblProvAmt.Text) Then
                        TotalProvAmt = TotalProvAmt + Convert.ToDecimal(lblProvAmt.Text)
                    End If
                    If IsNumeric(lblVatInputProvAmt.Text) Then
                        TotalVatInputProvAmt = TotalVatInputProvAmt + Convert.ToDecimal(lblVatInputProvAmt.Text)
                    End If
                    If IsNumeric(lbtnActualAmt.Text) Then
                        TotalActualAmt = TotalActualAmt + Convert.ToDecimal(lbtnActualAmt.Text)
                    End If

                    Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")

                    ''changed by mohamed on 25/12/2021
                    If CType(txtCurrencyCode.Text, String) <> baseCurrency And (CType(lblServiceType.Text.ToLower, String) = "others" Or lblServiceType.Text.ToLower = "tours") Then 'changed by mohamed on 16/06/2022 included tours with others
                        'If CType(txtCurrencyCode.Text, String) <> baseCurrency And (lblServiceType.Text.ToLower = "tours" Or lblServiceType.Text.ToLower = "transfers" Or lblServiceType.Text.ToLower = "airportma" Or lblServiceType.Text.ToLower = "visa" Or lblServiceType.Text.ToLower = "others") Then 'changed by mohamed on 16/06/2022 to calculate for all except hotel not used
                        If IsNumeric(txtNonTaxAmt.Text) Then
                            TotalNonTaxAmt = TotalNonTaxAmt + (Convert.ToDecimal(txtNonTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text))
                        End If
                        If IsNumeric(txtTaxAmt.Text) Then
                            TotalTaxAmt = TotalTaxAmt + (Convert.ToDecimal(txtTaxAmt.Text) * Convert.ToDecimal(txtConvRate.Text))
                        End If
                        If IsNumeric(txtVatAmt.Text) Then
                            TotalVatAmt = TotalVatAmt + (Convert.ToDecimal(txtVatAmt.Text) * Convert.ToDecimal(txtConvRate.Text))
                        End If
                        If IsNumeric(txtTotalAmt.Text) Then
                            NetTotalAmt = NetTotalAmt + Convert.ToDecimal(lbtnActualAmt.Text) ' rosalin 29/11/2023
                            '(Convert.ToDecimal(txtTotalAmt.Text) * Convert.ToDecimal(txtConvRate.Text))
                        End If
                        If IsNumeric(txtCommisionAmt.Text) Then
                            NetCommissionAmt = NetCommissionAmt + (Convert.ToDecimal(txtCommisionAmt.Text) * Convert.ToDecimal(txtConvRate.Text))
                        End If
                    Else
                        If IsNumeric(txtNonTaxAmt.Text) Then
                            TotalNonTaxAmt = TotalNonTaxAmt + Convert.ToDecimal(txtNonTaxAmt.Text)
                        End If
                        If IsNumeric(txtTaxAmt.Text) Then
                            TotalTaxAmt = TotalTaxAmt + Convert.ToDecimal(txtTaxAmt.Text)
                        End If
                        If IsNumeric(txtVatAmt.Text) Then
                            TotalVatAmt = TotalVatAmt + Convert.ToDecimal(txtVatAmt.Text)
                        End If
                        If IsNumeric(txtTotalAmt.Text) Then
                            NetTotalAmt = NetTotalAmt + Convert.ToDecimal(lbtnActualAmt.Text) 'Convert.ToDecimal(txtTotalAmt.Text)
                        End If
                        If IsNumeric(txtCommisionAmt.Text) Then
                            NetCommissionAmt = NetCommissionAmt + Convert.ToDecimal(txtCommisionAmt.Text)
                        End If
                    End If
                End If
            Next
            gvUpdateSupplier.FooterRow.Cells(0).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvUpdateSupplier.FooterRow.Cells(1).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvUpdateSupplier.FooterRow.Cells(2).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvUpdateSupplier.FooterRow.Cells(3).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            gvUpdateSupplier.FooterRow.Cells(4).BorderColor = Drawing.ColorTranslator.FromHtml("#DDD9CF")
            Dim lblTotalProvAmt As Label
            Dim lblTotalVatInputProvAmt As Label
            Dim lblTotalActualAmt As Label
            Dim lblTotalNonTaxAmt As Label
            Dim lblTotalTaxAmt As Label
            Dim lblTotalVatAmt As Label
            Dim lblNetTotalAmt As Label
            Dim lblNetCommissionAmt As Label
            lblTotalProvAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalProvAmt"), Label)
            lblTotalProvAmt.Text = TotalProvAmt
            lblTotalVatInputProvAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatInputProvAmt"), Label)
            lblTotalVatInputProvAmt.Text = TotalVatInputProvAmt
            lblTotalActualAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalActualAmt"), Label)
            lblTotalActualAmt.Text = TotalActualAmt
            lblTotalNonTaxAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalNonTaxAmt"), Label)
            lblTotalNonTaxAmt.Text = TotalNonTaxAmt
            lblTotalTaxAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalTaxAmt"), Label)
            lblTotalTaxAmt.Text = TotalTaxAmt
            lblTotalVatAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatAmt"), Label)
            lblTotalVatAmt.Text = TotalVatAmt
            lblNetTotalAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblNetTotalAmt"), Label)
            lblNetTotalAmt.Text = NetTotalAmt
            lblNetCommissionAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblNetCommisionAmt"), Label)
            lblNetCommissionAmt.Text = NetCommissionAmt
        End If

    End Sub
#End Region

#Region "Protected Sub chkSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub chkSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        Dim lblGuestDetails As Label
        'If txtNarration.Text = "" Or Session("narration") <> "" Then
        '    Dim guest As String = ""
        '    For Each row As GridViewRow In gvUpdateSupplier.Rows
        '        chk = CType(row.FindControl("chkSelection"), CheckBox)
        '        lblGuestDetails = CType(row.FindControl("lblGuestDetails"), Label)
        '        If chk.Checked = True Then
        '            If guest <> "" Then
        '                guest = guest + "," + lblGuestDetails.Text
        '            Else
        '                guest = lblGuestDetails.Text
        '            End If
        '        End If
        '    Next
        '    txtNarration.Text = txtSupplier.Text.Trim() + "; " + guest + IIf(txtInvoiceNo.Text.Trim() <> "", "; ", "") + txtInvoiceNo.Text
        'End If


        'If txtNarration.Text = "" Then
        '    'txtNarration.Text = "Purchase Invoices From " + txtChkFromDt.Text + " to " + txtChkToDt.Text
        'End If
        'If Session("narration") <> "" Then
        '    'txtNarration.Text = "Purchase Invoices From " + txtChkFromDt.Text + " to " + txtChkToDt.Text
        'End If

        'Session("narration") = txtNarration.Text
        Dim lblRequestId As Label
        Dim suppno As String
        Dim supp As String
        'If txtInvoiceNo.Text = "" Then
        '    For Each row As GridViewRow In gvUpdateSupplier.Rows
        '        chk = CType(row.FindControl("chkSelection"), CheckBox)
        '        lblRequestId = CType(row.FindControl("lblRequestId"), Label)
        '        If chk.Checked = True Then
        '            If txtInvoiceNo.Text <> "" Then
        '                txtInvoiceNo.Text = txtInvoiceNo.Text + "," + lblRequestId.Text
        '            Else
        '                txtInvoiceNo.Text = lblRequestId.Text
        '            End If
        '        End If
        '    Next
        'End If
        If Session("SuppInv") <> "" Then
            supp = CType(Session("SuppInv"), String)
            For Each row As GridViewRow In gvUpdateSupplier.Rows
                chk = row.FindControl("chkSelection")
                lblRequestId = row.FindControl("lblRequestId")
                If chk.Checked = True Then
                    suppno += lblRequestId.Text + ","
                Else
                    'If supp.Contains(",") Then
                    '    If suppno.Contains(lblRequestId.Text + ",") Then
                    '        suppno = supp.Replace(lblRequestId.Text + ",", "")
                    '    ElseIf suppno.Contains(lblRequestId.Text) Then
                    '        suppno = supp.Replace(lblRequestId.Text, "")
                    '    End If
                    'Else
                    '    suppno = supp.Replace(lblRequestId.Text, "")
                    'End If
                End If
            Next
            'If suppno <> "" Then
            '    txtInvoiceNo.Text = suppno.Substring(0, suppno.Length - 1)
            'Else
            '    txtInvoiceNo.Text = suppno
            'End If
            'Session("SuppInv") = txtInvoiceNo.Text
        End If
        'Session("SuppInv") = txtInvoiceNo.Text
        suppno = ""
        'If txtRequestId.Text = "" Then
        '    For Each row As GridViewRow In gvUpdateSupplier.Rows
        '        chk = CType(row.FindControl("chkSelection"), CheckBox)
        '        lblRequestId = CType(row.FindControl("lblRequestId"), Label)
        '        If chk.Checked = True Then
        '            If txtRequestId.Text <> "" Then
        '                txtRequestId.Text = txtRequestId.Text + "," + lblRequestId.Text
        '            Else
        '                txtRequestId.Text = lblRequestId.Text
        '            End If
        '        End If
        '    Next
        'End If
        If Session("BookingNo") <> "" Then
            For Each row As GridViewRow In gvUpdateSupplier.Rows
                chk = row.FindControl("chkSelection")
                lblRequestId = row.FindControl("lblRequestId")
                If chk.Checked = True Then
                    suppno += lblRequestId.Text + ","
                End If
            Next
            'If suppno <> "" Then
            '    txtRequestId.Text = suppno.Substring(0, suppno.Length - 1)
            'Else
            '    txtRequestId.Text = suppno
            'End If
            'Session("BookingNo") = txtRequestId.Text
        End If
        'Session("BookingNo") = txtRequestId.Text

        'Added param on 28/06/2021
        Dim lblChkOut As Label
        Dim maxCheckoutDate As Date
        For Each gvr As GridViewRow In gvUpdateSupplier.Rows
            chk = CType(gvr.FindControl("chkSelection"), CheckBox)
            If chk.Checked Then
                lblChkOut = CType(gvr.FindControl("lblCheckOut"), Label)
                If Not IsDate(maxCheckoutDate) Then
                    maxCheckoutDate = CType(lblChkOut.Text, Date)
                Else
                    If maxCheckoutDate < CType(lblChkOut.Text, Date) Then
                        maxCheckoutDate = CType(lblChkOut.Text, Date)
                    End If
                End If
            End If
        Next
        If IsDate(maxCheckoutDate) And IsDate(hdnSealDate.Value) Then
            If maxCheckoutDate <= CType(hdnSealDate.Value, Date) Then
                maxCheckoutDate = Convert.ToDateTime(hdnSealDate.Value).AddDays(1)
            End If
            txtDocDate.Text = maxCheckoutDate.ToString("dd/MM/yyyy")
        ElseIf IsDate(maxCheckoutDate) Then
            txtDocDate.Text = maxCheckoutDate.ToString("dd/MM/yyyy")
        End If

        calculateNetTotal()
    End Sub
#End Region

#Region "Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        calculateNetTotal()
    End Sub
#End Region

#Region "Protected Function Validation() As Boolean"
    Protected Function Validation() As Boolean
        Try
            Dim chkSelection As CheckBox
            Dim lbtnActualAmt As LinkButton
            Dim txtNonTaxAmt As TextBox
            Dim lblChkOut As Label
            Dim lblCheckIn As Label
            Dim lblRequestId As Label

            Dim txtTotalAmt As TextBox
            'If ViewState("SupplierInvoiceState") = "New" Then
            '    For Each gvr As GridViewRow In gvUpdateSupplier.Rows
            '        chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
            '        If gvUpdateSupplier.Rows.Count = 0 Then
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select rows in the list');", True)
            '            ' txtInvoiceNo.Focus()
            '            Validation = False
            '            Exit Function
            '        End If



            'Tanvir 30092022

            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand("sp_get_account_against_bill", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@div_code ", SqlDbType.VarChar, 20)).Value = txtDivcode.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@tran_id ", SqlDbType.VarChar, 10)).Value = txtDocNo.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"

            Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable
                    myDataAdapter.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Entry is adjusted in accounts hence this action cannot be performed');", True)
                        txtDocDate.Focus()
                        Validation = False
                        Exit Function
                    End If
                End Using
            End Using

            'Tanvir 30092022

            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
            Dim rowselected As Boolean
            rowselected = False
            For Each gvr As GridViewRow In gvUpdateSupplier.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked Then
                    rowselected = True
                    lbtnActualAmt = CType(gvr.FindControl("lbtnActualAmt"), LinkButton)
                    txtNonTaxAmt = CType(gvr.FindControl("txtNonTaxAmt"), TextBox)
                    txtTotalAmt = CType(gvr.FindControl("txtTotalAmt"), TextBox)
                    lblChkOut = CType(gvr.FindControl("lblCheckOut"), Label)
                    lblCheckIn = CType(gvr.FindControl("lblCheckIn"), Label)
                    lblRequestId = CType(gvr.FindControl("lblRequestId"), Label)

                    'If hdnChkDtFlag.Value = "Y" Then
                    '    If (CType(lblCheckIn.Text, Date) > CType(txtDocDate.Text, Date)) Then
                    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Purchase Bill Date Should be greater than Check In Date. Please Check Row-" & gvr.RowIndex + 1 & "');", True)
                    '        txtDocDate.Focus()
                    '        Validation = False
                    '        Exit Function
                    '    End If
                    'Else
                    '    If (CType(lblChkOut.Text, Date) > CType(txtDocDate.Text, Date)) Then
                    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Purchase Bill Date Should be Greater Than Check Out Date. Please Check Row-" & gvr.RowIndex + 1 & "');", True)
                    '        txtDocDate.Focus()
                    '        Validation = False
                    '        Exit Function
                    '    End If
                    'End If

                    If IsNumeric(lbtnActualAmt.Text) And IsNumeric(txtTotalAmt.Text) Then
                        Dim actualAmt As Decimal = Convert.ToDecimal(lbtnActualAmt.Text)
                        Dim totalAmt As Decimal = Convert.ToDecimal(txtTotalAmt.Text)
                        Dim diff As Decimal = Math.Abs(actualAmt - totalAmt)
                        Dim diffConfig As Decimal = System.Configuration.ConfigurationManager.AppSettings("PIDiff")
                        If actualAmt <> totalAmt And diff > 0.5 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Actual Amount is not equal to Total Amount for " & lblRequestId.Text & "');", True)
                            txtNonTaxAmt.Focus()
                            Validation = False
                            Exit Function
                        End If
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Actual Amount or Total Amount is not numeric for " & lblRequestId.Text & "');", True)
                        txtNonTaxAmt.Focus()
                        Validation = False
                        Exit Function
                    End If
                End If
            Next
            If ViewState("SupplierInvoiceState") = "New" And rowselected = False Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select rows in the list');", True)
                ' txtInvoiceNo.Focus()
                Validation = False
                Exit Function
            End If
            'If txtInvoiceNo.Text = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Invoice No Cannot be Blank');", True)
            '    txtInvoiceNo.Focus()
            '    Validation = False
            '    Exit Function
            'End If
            'If txtNarration.Text = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Narration Cannot be Blank');", True)
            '    txtNarration.Focus()
            '    Validation = False
            '    Exit Function
            'End If
            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from sealing_master")
            If IsDate(sealdate) Then
                If (CType(txtDocDate.Text, Date) <= CType(sealdate, Date)) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Purchase Bill Date Should be greater than seal date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
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
        Dim recordsUpdatedSuccessfully As Boolean = False 'Tanvir 20012024

        Try
            If Validation() = False Then Exit Sub
            Dim strdiv As String
            ' strdiv = Convert.ToString(ViewState("divcode"))
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open

            Dim optionval As String
            hdnCommissionFlag.Value = Session("Commission")
            Dim optionForId As String = Convert.ToString(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2039'"))
            '            Dim Month As String = Format(Convert.ToDateTime(Now.Date), "MM")
            '           Dim Year As String = Format(Convert.ToDateTime(Now.Date), "yyyy")
            Dim Month As String = Format(Convert.ToDateTime(txtDocDate.Text), "MM")
            Dim Year As String = Format(Convert.ToDateTime(txtDocDate.Text), "yyyy")

            Dim dtData As DataTable = DirectCast(Session("dtDisplayData"), DataTable)

            'Dim groupedDataTable As New DataTable()
            'groupedDataTable.Columns.Add("InvNo", GetType(String))


            'Dim query = dtData.AsEnumerable().Select(Function(row) row.Field(Of String)("InvNo")).Distinct()

            'If query.Any() Then
            '    For Each result As String In query
            '        groupedDataTable.Rows.Add(result.ToString())
            '    Next
            'End If

            ' For Each rowInvNo In groupedDataTable.Rows
            If gvUpdateSupplier.Rows.Count > 0 Then

                Dim chkSelection1 As New CheckBox
                Dim lblInvoiceNo1, lblVatInputProvAmt1, lblProvAmt1, lblRequestId1, lblRequestIddesc, lblCheckout1 As New Label
                Dim txtTotalAmt1, txtCommisionAmt1, txtCostVatAmt1, txtTaxAmt1, txtNonTaxAmt1 As New TextBox
                Dim lbtnActualAmt1 As New LinkButton

                Dim dcTotalAmt, dcCommisionAmt, dcCostVatAmt, dcTaxAmt, dcNonTaxAmt, dcActualAmt, dcVatInputProvAmt, dcProvAmt As New Decimal
                For Each dr1 In gvUpdateSupplier.Rows
                    chkSelection1 = CType(dr1.FindControl("chkSelection"), CheckBox)
                    lblInvoiceNo1 = CType(dr1.FindControl("lblInvoiceNo"), Label) ' Added by Ram 28-09-2023
                    lblRequestIddesc = CType(dr1.FindControl("lblRequestId"), Label) ' Added by Tanvir 07062024
                    lblCheckout1 = CType(dr1.FindControl("lblCheckOut"), Label)  ' Added by Tanvir 07062024
                    If chkSelection1.Checked Then
                        Dim bookingRefNo As String = ""
                        sqlTrans = sqlConn.BeginTransaction
                        ' Dim InvoiceNumber As String = rowInvNo("InvNo").ToString().Trim()
                        If (ViewState("SupplierInvoiceState") = "New" Or ViewState("SupplierInvoiceState") = "Edit") Then 'Tanvir 28092022
                            If ViewState("SupplierInvoiceState") = "New" Then
                                If optionForId = "Year" Then
                                    optionval = objUtils.GetAutoDocNodiv("PINVYEAR", sqlConn, sqlTrans, txtDivcode.Text)
                                    txtDocNo.Text = optionval.Trim
                                ElseIf optionForId = "Month" Then
                                    mySqlCmd = New SqlCommand("sp_get_monthyear_number", sqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@docgen_div_optionName ", SqlDbType.VarChar, 20)).Value = "PINVMonth"
                                    mySqlCmd.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                                    mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"
                                    mySqlCmd.Parameters.Add(New SqlParameter("@docmonth ", SqlDbType.VarChar, 10)).Value = Month
                                    mySqlCmd.Parameters.Add(New SqlParameter("@docyear", SqlDbType.VarChar, 10)).Value = Year '2008
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Code", SqlDbType.VarChar, 20))
                                    mySqlCmd.Parameters("@Code").Direction = ParameterDirection.Output
                                    mySqlCmd.ExecuteNonQuery()
                                    txtDocNo.Text = mySqlCmd.Parameters("@Code").Value.ToString()
                                End If
                            End If
                            If ViewState("SupplierInvoiceState") = "New" Then
                                mySqlCmd = New SqlCommand("sp_add_purchaseinvoiceheader", sqlConn, sqlTrans)
                            ElseIf ViewState("SupplierInvoiceState") = "Edit" Then
                                mySqlCmd = New SqlCommand("sp_mod_purchaseinvoiceheader", sqlConn, sqlTrans)
                            End If
                            Dim d As String = Format(CType(txtDocDate.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@divcode ", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                            mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno ", SqlDbType.VarChar, 20)).Value = txtDocNo.Text

                            'tanvir 07062024
                            Dim docdate As DateTime

                            docdate = Format(CType(lblCheckout1.Text, Date), "yyyy/MM/dd")
                            If IsDate(docdate) And IsDate(hdnSealDate.Value) Then
                                If docdate <= CType(hdnSealDate.Value, Date) Then
                                    docdate = Convert.ToDateTime(hdnSealDate.Value).AddDays(1)
                                End If
                                txtDocDate.Text = docdate.ToString("dd/MM/yyyy")
                            ElseIf IsDate(docdate) Then
                                txtDocDate.Text = docdate.ToString("dd/MM/yyyy")
                            End If
                            'tanvir 07062024
                            mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicedate", SqlDbType.DateTime)).Value = Format(CType(txtDocDate.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 20)).Value = "PI"
                            If ViewState("SupplierInvoiceState") = "New" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = txtSupplierCode.Text
                                'mySqlCmd.Parameters.Add(New SqlParameter("@hotelinvoiceno", SqlDbType.VarChar, 20)).Value = txtInvoiceNo.Text   'hide param on 09/06/2021
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@hotelinvoiceno", SqlDbType.VarChar, 20)).Value = lblInvoiceNo1.Text ' InvoiceNumber 'txtInvoiceNo.Text  'add param on 09/06/2021
                            mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, -1)).Value = IIf(txtNarration.Text = "", txtSupplier.Text + " " + lblRequestIddesc.Text, txtNarration.Text) 'Tanvir 07062024
                            'mySqlCmd.Parameters.Add(New SqlParameter("@Bookingrefno", SqlDbType.VarChar, 200)).Value = "" 'txtRequestId.Text
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                            If ViewState("SupplierInvoiceState") = "New" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = txtCtrlAcctCode.Text
                                mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 200)).Value = CType(txtCurrencyCode.Text, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtConvRate.Text)
                            End If



                            '   If chkSelection1.Checked And lblInvoiceNo1.Text.Trim() = InvoiceNumber Then
                            lblRequestId1 = CType(dr1.FindControl("lblRequestId"), Label) ' Added by Ram 28-09-2023
                            bookingRefNo += lblRequestId1.Text.Trim().ToString() + ","

                            txtTotalAmt1 = CType(dr1.FindControl("txtTotalAmt"), TextBox)
                            txtCommisionAmt1 = CType(dr1.FindControl("txtCommisionAmt"), TextBox)
                            txtCostVatAmt1 = CType(dr1.FindControl("txtCostVatAmt"), TextBox)
                            txtTaxAmt1 = CType(dr1.FindControl("txtTaxAmt"), TextBox)
                            txtNonTaxAmt1 = CType(dr1.FindControl("txtNonTaxAmt"), TextBox)
                            lbtnActualAmt1 = CType(dr1.FindControl("lbtnActualAmt"), LinkButton)
                            lblVatInputProvAmt1 = CType(dr1.FindControl("lblVatInputProvAmt"), Label)
                            lblProvAmt1 = CType(dr1.FindControl("lblProvAmt"), Label)

                            ' dcTotalAmt += Convert.ToDecimal(IIf(txtTotalAmt1.Text = "", 0, txtTotalAmt1.Text))
                            dcTotalAmt += Convert.ToDecimal(IIf(txtTotalAmt1.Text = "", 0, lbtnActualAmt1.Text))
                            dcCommisionAmt += Convert.ToDecimal(IIf(txtCommisionAmt1.Text = "", 0, txtCommisionAmt1.Text))
                            dcCostVatAmt += Convert.ToDecimal(IIf(txtCostVatAmt1.Text = "", 0, txtCostVatAmt1.Text))
                            dcTaxAmt += Convert.ToDecimal(IIf(txtTaxAmt1.Text = "", 0, txtTaxAmt1.Text))
                            dcNonTaxAmt += Convert.ToDecimal(IIf(txtNonTaxAmt1.Text = "", 0, txtNonTaxAmt1.Text))
                            dcActualAmt += Convert.ToDecimal(IIf(lbtnActualAmt1.Text = "", 0, lbtnActualAmt1.Text))
                            dcVatInputProvAmt += Convert.ToDecimal(IIf(lblVatInputProvAmt1.Text = "", 0, lblVatInputProvAmt1.Text))
                            dcProvAmt += Convert.ToDecimal(IIf(lblProvAmt1.Text = "", 0, lblProvAmt1.Text))
                            'End If


                            Dim lblNetTotalAmt As Label
                            Dim lblNetCommissionAmt As Label
                            Dim lblTotalVatAmt As Label
                            Dim lblTotalTaxAmt As Label
                            Dim lblTotalNonTaxAmt As Label
                            Dim lblTotalActualAmt As Label
                            Dim lblTotalVatInputProvAmt As Label
                            Dim lblTotalProvAmt As Label

                            lblTotalProvAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalProvAmt"), Label)
                            lblTotalVatInputProvAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatInputProvAmt"), Label)
                            lblTotalActualAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalActualAmt"), Label)
                            lblTotalNonTaxAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalNonTaxAmt"), Label)
                            lblTotalTaxAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalTaxAmt"), Label)
                            lblTotalVatAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblTotalVatAmt"), Label)
                            lblNetTotalAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblNetTotalAmt"), Label)
                            lblNetCommissionAmt = CType(gvUpdateSupplier.FooterRow.FindControl("lblNetCommisionAmt"), Label)

                            Dim tottxtNonTaxAmt, tottxtTaxAmt, tottxtVatPerc, tottxtCostVatAmt, tottxtCommisionAmt, tottxtTotalAmt As TextBox
                            Dim totlbtnActualAmt As LinkButton
                            Dim totllblVatInputProvAmount, totlblProvAmt As Label

                            totlblProvAmt = CType(dr1.FindControl("lblProvAmt"), Label)
                            tottxtNonTaxAmt = CType(dr1.FindControl("txtNonTaxAmtBase"), TextBox)
                            tottxtTaxAmt = CType(dr1.FindControl("txtTaxAmtBase"), TextBox)
                            tottxtVatPerc = CType(dr1.FindControl("txtVatPerc"), TextBox)
                            tottxtCostVatAmt = CType(dr1.FindControl("txtCostVatAmtBase"), TextBox)
                            tottxtTotalAmt = CType(dr1.FindControl("txtTotalAmtBase"), TextBox)
                            tottxtCommisionAmt = CType(dr1.FindControl("txtCommisionAmt"), TextBox)
                            totlbtnActualAmt = CType(dr1.FindControl("lbtnActualAmt"), LinkButton)
                            totllblVatInputProvAmount = CType(dr1.FindControl("lblVatInputProvAmt"), Label)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@totalprovisionamount", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblTotalProvAmt.Text)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_vatprovision", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblTotalVatInputProvAmt.Text)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_actualamount ", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblTotalActualAmt.Text)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costnontaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblTotalNonTaxAmt.Text)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costtaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblTotalTaxAmt.Text)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costvatvaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblTotalVatAmt.Text)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_totalprice", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblNetTotalAmt.Text)
                            'If hdnCommissionFlag.Value = "true" Then
                            '    mySqlCmd.Parameters.Add(New SqlParameter("@Commission", SqlDbType.Decimal)).Value = Convert.ToDecimal(lblNetCommissionAmt.Text)
                            'End If

                            'mySqlCmd.Parameters.Add(New SqlParameter("@totalprovisionamount", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcProvAmt)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_vatprovision", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcVatInputProvAmt)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_actualamount ", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcProvAmt) + Convert.ToDecimal(dcVatInputProvAmt)  'Convert.ToDecimal(dcActualAmt)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costnontaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcNonTaxAmt)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costtaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcTaxAmt)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costvatvaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcCostVatAmt)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@total_totalprice", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcTotalAmt)
                            'Tanvir 26122023

                            mySqlCmd.Parameters.Add(New SqlParameter("@totalprovisionamount", SqlDbType.Decimal)).Value = Convert.ToDecimal(totlblProvAmt.Text) 'Convert.ToDecimal(dcProvAmt)
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_vatprovision", SqlDbType.Decimal)).Value = Convert.ToDecimal(totllblVatInputProvAmount.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_actualamount", SqlDbType.Decimal)).Value = Convert.ToDecimal(totlbtnActualAmt.Text) ' Convert.ToDecimal(lblProvAmt1.Text)+'Convert.ToDecimal(dcProvAmt) + Convert.ToDecimal(dcVatInputProvAmt)
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costnontaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(tottxtNonTaxAmt.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costtaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(tottxtTaxAmt.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costvatvaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(tottxtCostVatAmt.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_totalprice", SqlDbType.Decimal)).Value = Convert.ToDecimal(tottxtTotalAmt.Text)
                            'Tanvir 26122023
                            If hdnCommissionFlag.Value = "true" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@Commission", SqlDbType.Decimal)).Value = Convert.ToDecimal(dcCommisionAmt)
                            End If
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@totalprovisionamount", SqlDbType.Decimal)).Value = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_vatprovision", SqlDbType.Decimal)).Value = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_actualamount ", SqlDbType.Decimal)).Value = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costnontaxablevaluebase", SqlDbType.Decimal)).Value = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costtaxablevaluebase", SqlDbType.Decimal)).Value = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_prices_costvatvaluebase", SqlDbType.Decimal)).Value = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@total_totalprice", SqlDbType.Decimal)).Value = 0
                            If hdnCommissionFlag.Value = "true" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@Commission", SqlDbType.Decimal)).Value = 0
                            End If
                        End If

                        If bookingRefNo <> "" Then
                            bookingRefNo = bookingRefNo.Remove(bookingRefNo.Length - 1, 1)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@Bookingrefno", SqlDbType.VarChar, 200)).Value = bookingRefNo 'txtRequestId.Text

                        If ViewState("SupplierInvoiceState") = "New" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmchkout", SqlDbType.DateTime)).Value = Format(CType(txtChkFromDt.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@tochkout", SqlDbType.DateTime)).Value = Format(CType(txtChkToDt.Text, Date), "yyyy/MM/dd")

                            'If ddlType.SelectedValue = "Supplier" Then
                            '    mySqlCmd.Parameters.Add(New SqlParameter("@suppliertype", SqlDbType.VarChar, 10)).Value = "S"
                            'Else

                            '    mySqlCmd.Parameters.Add(New SqlParameter("@suppliertype", SqlDbType.VarChar, 10)).Value = "A"
                            'End If

                            mySqlCmd.Parameters.Add(New SqlParameter("@suppliertype", SqlDbType.VarChar, 10)).Value = "S"
                        End If
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.ExecuteNonQuery()

                        Dim invRequestIds As String = ""
                        mySqlCmd = New SqlCommand("sp_del_purchaseinvoicehoteldetails", sqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@divcode ", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno ", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 20)).Value = "PI"
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.ExecuteNonQuery()

                        Dim chkSelection As New CheckBox
                        Dim lblInvoiceNo, lblRequestId, lblCheckIn, lblCheckOut, lblPax, lblPaxType, lblService, lblServiceType, lblSupConfNo, lblProvAmt, lblVatInputProvAmount, lblGuestDetails As Label
                        Dim txtRlineNo, txtRoomNo, txtNonTaxAmt, txtTaxAmt, txtVatPerc, txtCostVatAmt, txtTotalAmt, txtCommisionAmt As TextBox

                        Dim lbtnActualAmt As LinkButton
                        'For Each dr As GridViewRow In gvUpdateSupplier.Rows
                        '    chkSelection = CType(dr.FindControl("chkSelection"), CheckBox)
                        '    lblInvoiceNo = CType(dr.FindControl("lblInvoiceNo"), Label) ' Added by Ram 28-09-2023

                        '    If chkSelection.Checked = True And lblInvoiceNo.Text = InvoiceNumber Then ' And condition Added by Ram 28-09-2023

                        lblRequestId = CType(dr1.FindControl("lblRequestId"), Label)
                        txtRlineNo = CType(dr1.FindControl("txtRlineNo"), TextBox)
                        txtRoomNo = CType(dr1.FindControl("txtRoomNo"), TextBox)
                        lblCheckIn = CType(dr1.FindControl("lblCheckIn"), Label)
                        lblCheckOut = CType(dr1.FindControl("lblCheckOut"), Label)
                        lblPax = CType(dr1.FindControl("lblPax"), Label)
                        lblService = CType(dr1.FindControl("lblService"), Label)
                        lblServiceType = CType(dr1.FindControl("lblServiceType"), Label)
                        lblSupConfNo = CType(dr1.FindControl("lblSupConfNo"), Label)
                        lblProvAmt = CType(dr1.FindControl("lblProvAmt"), Label)
                        lblVatInputProvAmount = CType(dr1.FindControl("lblVatInputProvAmt"), Label)
                        txtNonTaxAmt = CType(dr1.FindControl("txtNonTaxAmt"), TextBox)
                        txtTaxAmt = CType(dr1.FindControl("txtTaxAmt"), TextBox)
                        txtVatPerc = CType(dr1.FindControl("txtVatPerc"), TextBox)
                        txtCostVatAmt = CType(dr1.FindControl("txtCostVatAmt"), TextBox)
                        txtTotalAmt = CType(dr1.FindControl("txtTotalAmt"), TextBox)
                        txtCommisionAmt = CType(dr1.FindControl("txtCommisionAmt"), TextBox)
                        lbtnActualAmt = CType(dr1.FindControl("lbtnActualAmt"), LinkButton)
                        lblGuestDetails = CType(dr1.FindControl("lblGuestDetails"), Label)
                        lblPaxType = CType(dr1.FindControl("lblPaxType"), Label)
                        mySqlCmd = New SqlCommand("sp_add_purchaseinvoicehoteldetail", sqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno", SqlDbType.VarChar, 20)).Value = txtDocNo.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 20)).Value = "PI"
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = txtRlineNo.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.Int)).Value = txtRoomNo.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkin", SqlDbType.DateTime)).Value = lblCheckIn.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkout", SqlDbType.DateTime)).Value = lblCheckOut.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxdetails", SqlDbType.VarChar, -1)).Value = lblPax.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@servicedetails", SqlDbType.VarChar, 200)).Value = lblService.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@hotelconfno", SqlDbType.VarChar, 20)).Value = lblSupConfNo.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@provisionamount", SqlDbType.Decimal)).Value = lblProvAmt.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@vatprovision", SqlDbType.Decimal)).Value = lblVatInputProvAmount.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@actualamount", SqlDbType.Decimal)).Value = lbtnActualAmt.Text.Trim

                        Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                        'changed by mohamed on 25/12/2021
                        If CType(txtCurrencyCode.Text, String) <> baseCurrency And (CType(lblServiceType.Text.ToLower, String) = "others" Or CType(lblServiceType.Text.ToLower, String) = "tours" Or CType(lblServiceType.Text.ToLower, String) = "transfers") Then 'changed by mohamed on 16/06/2022  included tours with others added transfers 26122022
                            mySqlCmd.Parameters.Add(New SqlParameter("@prices_costnontaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtNonTaxAmt.Text.Trim) * Convert.ToDecimal(txtConvRate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@prices_costtaxablevaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtTaxAmt.Text.Trim) * Convert.ToDecimal(txtConvRate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@prices_costvatvaluebase", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtCostVatAmt.Text.Trim) * Convert.ToDecimal(txtConvRate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@totalprice", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtTotalAmt.Text.Trim) * Convert.ToDecimal(txtConvRate.Text)
                            'lbtnActualAmt.Text.Trim
                            '
                            If hdnCommissionFlag.Value = "true" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@Commission", SqlDbType.Decimal)).Value = Convert.ToDecimal(txtCommisionAmt.Text.Trim) * Convert.ToDecimal(txtConvRate.Text)
                            End If
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@prices_costnontaxablevaluebase", SqlDbType.Decimal)).Value = txtNonTaxAmt.Text.Trim
                            mySqlCmd.Parameters.Add(New SqlParameter("@prices_costtaxablevaluebase", SqlDbType.Decimal)).Value = txtTaxAmt.Text.Trim
                            mySqlCmd.Parameters.Add(New SqlParameter("@prices_costvatvaluebase", SqlDbType.Decimal)).Value = txtCostVatAmt.Text.Trim
                            mySqlCmd.Parameters.Add(New SqlParameter("@totalprice", SqlDbType.Decimal)).Value = txtTotalAmt.Text.Trim 'lbtnActualAmt.Text.Trim '
                            If hdnCommissionFlag.Value = "true" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@Commission", SqlDbType.Decimal)).Value = txtCommisionAmt.Text.Trim
                            End If
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@vatperc", SqlDbType.Decimal)).Value = txtVatPerc.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = CType(lblServiceType.Text.ToLower, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@GuestDetails", SqlDbType.VarChar, 20)).Value = CType(lblGuestDetails.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@PaxType", SqlDbType.VarChar, 20)).Value = CType(lblPaxType.Text, String)
                        mySqlCmd.ExecuteNonQuery()
                        '  If CType(lblServiceType.Text.ToLower, String) = "hotel" Then
                        If (Not Session("priceListDt") Is Nothing) Then
                            Dim pdtb As DataTable = CType(Session("priceListDt"), DataTable)
                            ' If lblServiceType.Text.ToLower = "hotel" Then
                            mySqlCmd = New SqlCommand("sp_add_purchaseinvoicehotelpricedetailtable", sqlConn, sqlTrans)
                            'ElseIf lblServiceType.Text.ToLower = "specialevent" Then
                            '    mySqlCmd = New SqlCommand("sp_add_purchaseinvoicespleventspricedetailtable", sqlConn, sqlTrans)

                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text
                            mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno", SqlDbType.VarChar, 20)).Value = txtDocNo.Text.Trim
                            mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 10)).Value = "PI"
                            mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = CType(lblServiceType.Text.ToLower, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                            Dim View As DataView = New DataView(pdtb)
                            View.RowFilter = "Requestid='" & lblRequestId.Text.Trim & "' and roomno =" & txtRoomNo.Text & " and rlineno=" & txtRlineNo.Text & " and paxtype= '" & lblPaxType.Text.ToLower & "' "
                            Dim dtfilteredtable As DataTable = View.ToTable

                            If dtfilteredtable.Columns.Contains("servicetype") Then
                                dtfilteredtable.Columns.Remove("servicetype")
                                dtfilteredtable.Columns.Remove("bookingname")
                            End If
                            mySqlCmd.Parameters.AddWithValue("@purchaseinvoicehotelpricedetail", dtfilteredtable)
                            mySqlCmd.CommandTimeout = 0
                            mySqlCmd.ExecuteNonQuery()

                        End If

                        ' End If
                        '  End If
                        'End If



                        'commented to add specail events
                        mySqlCmd = New SqlCommand("sp_post_purchaseinvoicedata", sqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@divcode ", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno ", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 10)).Value = "PI"
                        If ViewState("SupplierInvoiceState") = "New" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = "new"
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = "Amend"
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.ExecuteNonQuery()

                        If ViewState("SupplierInvoiceState") = "Edit" Then
                            'Tanvir 04102023 point6
                            If Not Session("PIAdjustedRecords") Is Nothing Then
                                Dim dt As DataTable = DirectCast(Session("PIAdjustedRecords"), DataTable)
                                'Dim filterExpression As String = "  div_code = " & strdiv & " " ' and against_tran_lineno=1 ' against_tran_id =" & txtDocNo.Value & "  and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And 
                                For Each row As DataRow In dt.Rows
                                    mySqlCmd = New SqlCommand("sp_reverse_pending_invoices", sqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                                    mySqlCmd.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = row("doclineno")
                                    mySqlCmd.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("docno"), String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("doctype"), String)


                                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("acc_code"), String) ' - -ddlgAccCode.Text
                                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(row("acc_type"), String) ' collectionDate("AccType" & strLineKey).ToString
                                    If CType(row("doctype"), String) = "PR" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = Convert.ToDecimal(CType(row("debit"), String))
                                        mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = Convert.ToDecimal(CType(row("basedebit"), String))
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                        mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = Convert.ToDecimal(CType(row("credit"), String))
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = Convert.ToDecimal(row("basecredit"))


                                    End If
                                    mySqlCmd.ExecuteNonQuery()
                                Next
                            End If
                        End If

                        '  If Not Session("opendetail_records") Is Nothing Then

                        '     Dim prrecordsadjusted As DataTable = DirectCast(Session("opendetail_records"), DataTable)
                        mySqlCmd = New SqlCommand("sp_get_PIpostingrecords", sqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@div_code ", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@tran_id ", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"
                        'mySqlCmd.Parameters.Add(New SqlParameter("@requestids", SqlDbType.VarChar, 10)).Value = Me.txtRequestId.Text

                        Using myDataAdapter = New SqlDataAdapter(mySqlCmd)
                            Using dt1 As New DataTable
                                myDataAdapter.Fill(dt1)
                                For Each row As DataRow In dt1.Rows
                                    mySqlCmd = New SqlCommand("sp_update_Pending_Invoices", sqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                                    mySqlCmd.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = row("doclineno")
                                    mySqlCmd.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("docno"), String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("doctype"), String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = Format(CType(row("docdate"), Date), "yyyy/MM/dd")
                                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("acc_code"), String)
                                    'If ddlType.SelectedValue = "Supplier" Then
                                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(row("acc_type"), String)


                                    mySqlCmd.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = IIf(IsDBNull(row("against_tran_id")), DBNull.Value, row("against_tran_id"))
                                    mySqlCmd.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = IIf(IsDBNull(row("against_tran_type")), DBNull.Value, row("against_tran_type"))
                                    mySqlCmd.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = IIf(IsDBNull((row("against_tran_lineno"))), DBNull.Value, row("against_tran_type"))
                                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = CType(row("acc_gl_code"), String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@duedate ", SqlDbType.DateTime)).Value = Format(CType(row("duedate"), Date), "yyyy/MM/dd")
                                    mySqlCmd.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = CType(row("currrate"), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@field1", SqlDbType.VarChar, 500)).Value = "" 'Trim(txtbookingno.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@field2", SqlDbType.VarChar, 500)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@field3 ", SqlDbType.VarChar, 500)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@field4", SqlDbType.VarChar, 500)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@field5", SqlDbType.VarChar, 500)).Value = ""
                                    mySqlCmd.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = "B"
                                    mySqlCmd.Parameters.Add(New SqlParameter("@open_field3 ", SqlDbType.VarChar, 500)).Value = txtNarration.Text
                                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = CType(txtCurrencyCode.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_State", SqlDbType.VarChar, 10)).Value = "P"
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = CType(row("debit"), Decimal)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = CType(row("credit"), Decimal)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(row("basedebit"), Decimal)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(row("basecredit"), Decimal)
                                    If CType(row("doctype"), String) = "PI" Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                        mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = Convert.ToDecimal(CType(row("credit"), String))
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = Convert.ToDecimal(CType(row("basecredit"), String))
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = Convert.ToDecimal(CType(row("debit"), String))
                                        mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = Convert.ToDecimal(CType(row("basedebit"), String))
                                        mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0


                                    End If
                                    mySqlCmd.ExecuteNonQuery()
                                Next

                            End Using
                        End Using


                        'ElseIf ViewState("SupplierInvoiceState") = "Delete" Then


                        '    mySqlCmd = New SqlCommand("sp_delvoucher", sqlConn, sqlTrans)
                        '    mySqlCmd.CommandType = CommandType.StoredProcedure
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "purchaseinvoice"
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = "PI"
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        '    mySqlCmd.ExecuteNonQuery()

                        '    'Dim invRequestIds As String = ""
                        '    mySqlCmd = New SqlCommand("sp_del_purchaseinvoicehoteldetails", sqlConn, sqlTrans)
                        '    mySqlCmd.CommandType = CommandType.StoredProcedure
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@divcode ", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno ", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 20)).Value = "PI"
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                        '    mySqlCmd.CommandTimeout = 0
                        '    mySqlCmd.ExecuteNonQuery()


                        '    mySqlCmd = New SqlCommand("sp_del_purchaseinvoiceheader", sqlConn, sqlTrans)
                        '    mySqlCmd.CommandType = CommandType.StoredProcedure
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoiceno", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@purchaseinvoicetype", SqlDbType.VarChar, 10)).Value = "PI"
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = txtDivcode.Text
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        '    mySqlCmd.ExecuteNonQuery()
                        ' End If
                        sqlTrans.Commit()
                        recordsUpdatedSuccessfully = True
                        clsDBConnect.dbSqlTransation(sqlTrans)

                        sqlTrans = Nothing
                    End If 'checked if end 
                Next

            End If ' Grid count if end


            clsDBConnect.dbConnectionClose(sqlConn)





            ModalPopupLoading.Hide()


            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully' ); ", True) ' 
            If ViewState("SupplierInvoiceState") = "New" Then
                ClearNewMode()

            Else

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('UpdateSupplierInvoicePostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
            'Tanvir 20012024
            'Catch ex As Exception

            '    If Not sqlConn Is Nothing Then
            '        sqlTrans.Rollback()
            '        clsDBConnect.dbCommandClose(mySqlCmd)
            '        clsDBConnect.dbConnectionClose(sqlConn)
            '    End If
            '    Dim errMsg As String = Regex.Replace(ex.Message.ToString(), "[^0-9a-zA-Z\._ ]", " ")
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + errMsg & "' );", True)

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully' ); ", True) ' 
            '    btnDisplay_Click(sender, e)

            '    objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            'End Try
         

            Catch ex As Exception

                If Not sqlConn Is Nothing Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbCommandClose(mySqlCmd)
                    clsDBConnect.dbConnectionClose(sqlConn)
                End If

                Dim errMsg As String = Regex.Replace(ex.Message.ToString(), "[^0-9a-zA-Z\._ ]", " ")


                If recordsUpdatedSuccessfully Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully' ); ", True)
                    btnDisplay_Click(sender, e)
                End If


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description: " + errMsg & "' );", True)
                objUtils.WritErrorLog("UpdateSupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        'Tanvir 20012024
    End Sub
#End Region


    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click

        Dim dtError As New DataTable()
        dtError.Columns.Add("id", GetType(Integer))
        dtError.Columns.Add("requestid", GetType(String))
        dtError.Columns.Add("errordesc", GetType(String))

        gvError.DataSource = dtError
        gvError.DataBind()
        Errtr1.Visible = False
        Errtr2.Visible = False

        BindGVUpdateSupplier(0)

        Try
            If fileUploadExcel.HasFile Then
                Dim FileName As String = Path.GetFileName(fileUploadExcel.PostedFile.FileName)
                Dim FileExtension As String = Path.GetExtension(fileUploadExcel.PostedFile.FileName)

                If (FileExtension = ".xls" Or FileExtension = ".xlsx") Then

                    Dim folder As String = Server.MapPath("~/PIExcelUpload/")
                    If Not Directory.Exists(folder) Then
                        Directory.CreateDirectory(folder)
                    End If

                    Dim filePath As String = folder + FileName
                    fileUploadExcel.SaveAs(filePath)

                    ' Dim filePath As String = Server.MapPath("~/ImportBookingExcel/") + FileName
                    'fileUploadExcel.SaveAs(filePath)

                    If Session("sExcelConfigData") Is Nothing Then
                        ExcelConfigData()
                    End If

                    Dim dtExcelConfigData As DataTable = CType(Session("sExcelConfigData"), DataTable)

                    Dim cnt As Integer = 0
                    Dim excelDt As New DataTable()
                    excelDt.Columns.Add("SupplierName", GetType(String))
                    excelDt.Columns.Add("TRNNo", GetType(String))
                    excelDt.AcceptChanges()

                    Using workBook As New XLWorkbook(filePath)
                        Dim sheetcnt As Integer = 1
                        'While workBook.Worksheets.Count >= sheetcnt
                        Dim workSheet As IXLWorksheet = workBook.Worksheet(sheetcnt)
                        Dim firstRow As Boolean = True

                        Dim wSheetName As String = workSheet.Name().ToString()

                        'If wSheetName.ToString().Trim() <> "Hotels without Comm." Or wSheetName.ToString().Trim() <> "Hotels without Comm" Or
                        '   wSheetName.ToString().Trim() <> "Hotels with Comm." Or wSheetName.ToString().Trim() <> "Hotels with Comm" Or
                        '   wSheetName.ToString().Trim() <> "Outbound Hotels" Or
                        '   wSheetName.ToString().Trim() <> "Transfer & Excursions" Then

                        '    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Work sheet name is not correct.' );", True)
                        '    ' Exit Sub

                        'End If

                        Dim dtFilteredXLConfigData As DataTable = dtExcelConfigData.Clone()

                        Dim filteredRows = From row In dtExcelConfigData.AsEnumerable()
                                           Where row.Field(Of String)("SheetName") = wSheetName
                                           Select row

                        For Each row As DataRow In filteredRows
                            dtFilteredXLConfigData.ImportRow(row)
                        Next

                        Dim strSupplierCell As String = ""
                        Dim strHotelTRNCell As String = "A1"
                        Dim startCellAddress As String = ""
                        Dim endCellAddress As String = ""

                        If dtFilteredXLConfigData IsNot Nothing Then

                            If dtFilteredXLConfigData.Rows.Count > 0 Then

                                Dim supplierCelldt As DataTable = (From row In dtFilteredXLConfigData.AsEnumerable()
                                                                   Where row.Field(Of String)("ConfigDescription") = "SupplierXlCell"
                                                                   Select row).CopyToDataTable()
                                If supplierCelldt IsNot Nothing Then
                                    If supplierCelldt.Rows.Count = 1 Then
                                        strSupplierCell = supplierCelldt.Rows(0)("ConfigValue").ToString()
                                    Else

                                    End If
                                End If

                                If wSheetName.ToString().Trim() = "Hotels without Comm." Or wSheetName.ToString().Trim() = "Hotels with Comm." Then

                                    Dim HotelTRNCelldt As DataTable = (From row In dtFilteredXLConfigData.AsEnumerable()
                                                                       Where row.Field(Of String)("ConfigDescription") = "HotelTRN"
                                                                       Select row).CopyToDataTable()
                                    If HotelTRNCelldt IsNot Nothing Then
                                        If HotelTRNCelldt.Rows.Count = 1 Then
                                            strHotelTRNCell = HotelTRNCelldt.Rows(0)("ConfigValue").ToString()
                                        Else

                                        End If
                                    End If
                                End If

                                Dim StartCelldt As DataTable = (From row In dtFilteredXLConfigData.AsEnumerable()
                                                                Where row.Field(Of String)("ConfigDescription") = "TableStart"
                                                                Select row).CopyToDataTable()
                                If StartCelldt IsNot Nothing Then
                                    If StartCelldt.Rows.Count = 1 Then
                                        startCellAddress = StartCelldt.Rows(0)("ConfigValue").ToString()
                                    Else

                                    End If
                                End If

                                Dim EndCelldt As DataTable = (From row In dtFilteredXLConfigData.AsEnumerable()
                                                              Where row.Field(Of String)("ConfigDescription") = "TableEnd"
                                                              Select row).CopyToDataTable()

                                If EndCelldt IsNot Nothing Then
                                    If EndCelldt.Rows.Count = 1 Then
                                        endCellAddress = EndCelldt.Rows(0)("ConfigValue").ToString()
                                    Else

                                    End If
                                End If
                            Else
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Work sheet name is not correct. It should be any one of these Hotels without Comm. OR Hotels with Comm. OR Outbound Hotels OR Transfer & Excursions' );", True)
                                Exit Sub

                            End If


                        End If

                        Dim cellSupplier As IXLCell = workSheet.Cell(strSupplierCell)
                        Dim cellValue As String = cellSupplier.Value.ToString()

                        Dim cellHotelTRN As IXLCell = workSheet.Cell(strHotelTRNCell)
                        Dim cellHotelTRNValue As String = cellHotelTRN.Value.ToString()

                        If wSheetName.ToString().Trim() = "Hotels without Comm." Or wSheetName.ToString().Trim() = "Hotels without Comm" Or
                            wSheetName.ToString().Trim() = "Hotels with Comm." Or wSheetName.ToString().Trim() = "Hotels with Comm" Or
                            wSheetName.ToString().Trim() = "Outbound Hotels" Or
                            wSheetName.ToString().Trim() = "Transfer & Excursions" Then
                            'Or wSheetName.ToString().Trim() = "Transfers, Tours & Excursions" Then


                            If wSheetName.ToString().Trim() = "Hotels without Comm." Or wSheetName.ToString().Trim() = "Hotels without Comm" Then
                                ddlCommissionType.SelectedValue = "NonCommissionable"
                                hdnCommissionFlag.Value = "False"

                            ElseIf wSheetName.ToString().Trim() = "Hotels with Comm." Or wSheetName.ToString().Trim() = "Hotels with Comm" Then
                                ddlCommissionType.SelectedValue = "Commissionable"
                                hdnCommissionFlag.Value = "true"

                            ElseIf wSheetName.ToString().Trim() = "Outbound Hotels" Then
                                ddlCommissionType.SelectedValue = "Select"
                                hdnCommissionFlag.Value = "False"

                            ElseIf wSheetName.ToString().Trim() = "Transfer & Excursions" Or wSheetName.ToString().Trim() = "Transfers, Tours & Excursions" Then
                                ddlCommissionType.SelectedValue = "Select"
                                hdnCommissionFlag.Value = "False"

                            Else
                                dtError.Rows.Add(1, "", wSheetName & " this worksheet doesn't match with our existing excel config details")
                                If dtError IsNot Nothing Then
                                    If dtError.Rows.Count > 0 Then
                                        gvError.DataSource = dtError
                                        gvError.DataBind()
                                        Errtr1.Visible = True
                                        Errtr2.Visible = True
                                    End If
                                End If
                                Exit Sub
                            End If
                        Else
                            dtError.Rows.Add(1, "", "work Sheet name is not correct. It should be any one of Hotels without Comm OR Hotels with Comm OR Outbound Hotels OR Transfer & Excursions")
                            Exit Sub
                        End If

                        ddlCommissionType_OnSelectedIndexChanged(sender, e)

                        ViewState.Add("WorkSheetName", wSheetName.ToString())

                        Dim numRows As Integer = workSheet.Rows.Count
                        Dim numCols As Integer = workSheet.Columns.Count

                        ' Get the cell range
                        Dim cellRange As IXLRange = workSheet.Range(startCellAddress, endCellAddress)

                        Dim cellVal As String = ""
                        ' Iterate through the cells in the range
                        For Each cell As IXLCell In cellRange.CellsUsed()
                            excelDt.Columns.Add(cell.Value.ToString().Trim(), GetType(String))
                        Next
                        excelDt.AcceptChanges()

                        Dim condition As Boolean = True
                        Dim i As Integer = 1
                        Dim k As Integer = 0
                        While condition
                            If k = 1 Then
                                condition = False
                            End If

                            Dim sCellAddress As String = "A" + Convert.ToString(17 + i)
                            Dim eCellAddress As String = "G" + Convert.ToString(17 + i)

                            Dim pattern As String = "([A-Za-z]+)(\d+)"

                            ' Create a regex object and match the input string
                            Dim regex As New Regex(pattern)
                            Dim smatches As MatchCollection = regex.Matches(startCellAddress)

                            ' Process and display the matched parts
                            For Each match As Match In smatches
                                Dim characterPart As String = match.Groups(1).Value
                                Dim numericPart As Integer = Integer.Parse(match.Groups(2).Value)

                                sCellAddress = characterPart & Convert.ToString(numericPart + i)
                            Next

                            Dim ematches As MatchCollection = regex.Matches(endCellAddress)

                            ' Process and display the matched parts
                            For Each match As Match In ematches
                                Dim characterPart As String = match.Groups(1).Value
                                Dim numericPart As Integer = Integer.Parse(match.Groups(2).Value)

                                eCellAddress = characterPart & Convert.ToString(numericPart + i)
                            Next

                            Dim cCount As Integer = 0
                            cCount = excelDt.Columns.Count()

                            Dim newRow As DataRow = excelDt.NewRow()
                            newRow(0) = cellValue.ToString()
                            newRow(1) = cellHotelTRNValue.ToString()

                            Dim cRange As IXLRange = workSheet.Range(sCellAddress, eCellAddress)
                            Dim j As Integer = 2

                            For Each cell As IXLCell In cRange.Cells()
                                If j = 2 Then
                                    If cell.Value.ToString().Trim() = "" Then
                                        'condition = False
                                        k = 1
                                    End If
                                End If

                                newRow(j) = cell.Value.ToString()
                                j += 1
                            Next
                            excelDt.Rows.Add(newRow)
                            excelDt.AcceptChanges()
                            i += 1

                        End While

                        'excelDt.Rows.RemoveAt(excelDt.Rows.Count - 2)

                        'Dim dtexcel As DataTable = excelDt.Clone()

                        'For Each row As DataRow In excelDt.Rows
                        '    If row("Inv No") <> "" And row("Bkg Reference Number") <> "" Then
                        '        Dim newRow As DataRow = row.Table.Clone().NewRow()
                        '        newRow.ItemArray = row.ItemArray
                        '        dtexcel.Rows.Add(newRow)
                        '    End If
                        'Next

                        Dim rowsToRemove As New List(Of DataRow)

                        For Each row As DataRow In excelDt.Rows
                            If String.IsNullOrEmpty(row("Inv No").ToString()) Or String.IsNullOrEmpty(row("Bkg Reference Number").ToString()) Then
                                rowsToRemove.Add(row)
                            End If
                        Next

                        ' Remove the empty rows from the DataTable
                        For Each rowToRemove As DataRow In rowsToRemove
                            excelDt.Rows.Remove(rowToRemove)
                        Next

                        Dim dtexcel As DataTable = excelDt.Copy()
                        Session("sexceldt") = dtexcel.Copy()

                        If dtexcel IsNot Nothing Then
                            If dtexcel.Rows.Count > 0 Then
                                If dtexcel.Rows(0)("SupplierName").ToString().Trim() = "" Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier name should be in A2 cell in the excel' );", True)
                                    Exit Sub
                                End If

                                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                                mySqlCmd = New SqlCommand("sp_get_supplier_ac_details", sqlConn)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = IIf(txtDivcode.Text.Trim = "", "01", txtDivcode.Text.Trim())
                                mySqlCmd.Parameters.Add(New SqlParameter("@acctType", SqlDbType.VarChar, 20)).Value = "S"
                                mySqlCmd.Parameters.Add(New SqlParameter("@suppliername", SqlDbType.VarChar, 500)).Value = dtexcel.Rows(0)("SupplierName").ToString().Trim()
                                myDataAdapter = New SqlDataAdapter(mySqlCmd)
                                Dim dt As New DataTable
                                myDataAdapter.Fill(dt)
                                If dt IsNot Nothing Then
                                    If dt.Rows.Count = 1 Then
                                        txtSupplier.Text = Convert.ToString(dt.Rows(0)("des"))
                                        txtSupplierCode.Text = Convert.ToString(dt.Rows(0)("code"))
                                        txtCtrlAcctCode.Text = Convert.ToString(dt.Rows(0)("controlacctcode"))
                                        txtCtrlAcct.Text = Convert.ToString(dt.Rows(0)("acctname"))
                                        txtCurrencyCode.Text = Convert.ToString(dt.Rows(0)("currcode"))
                                        txtCurrency.Text = Convert.ToString(dt.Rows(0)("currname"))
                                        txtConvRate.Text = Convert.ToString(dt.Rows(0)("convrate"))
                                        txtTrnNo.Text = Convert.ToString(dt.Rows(0)("trnno"))
                                    ElseIf dt.Rows.Count = 0 Then
                                        dtError.Rows.Add(1, "", "There is no supplier name in the excel")
                                    Else
                                        dtError.Rows.Add(1, "", "Multiple Supplier Occured in the excel")
                                    End If
                                Else
                                    dtError.Rows.Add(1, "", "There is no supplier name in the excel")
                                End If
                                clsDBConnect.dbCommandClose(mySqlCmd)
                                clsDBConnect.dbConnectionClose(sqlConn)
                            Else
                                dtError.Rows.Add(1, "", "There is no required data or format in the excel")
                            End If
                        Else
                            dtError.Rows.Add(1, "", "There is no required data or format in the excel")
                        End If

                        If dtError IsNot Nothing Then
                            If dtError.Rows.Count > 0 Then
                                gvError.DataSource = dtError
                                gvError.DataBind()
                                Errtr1.Visible = True
                                Errtr2.Visible = True
                            End If
                        End If

                    End Using
                Else

                End If
            End If
        Catch ex As Exception
            dtError.Rows.Add(1, "", ex.Message.ToString())
            If dtError IsNot Nothing Then
                If dtError.Rows.Count > 0 Then
                    gvError.DataSource = dtError
                    gvError.DataBind()
                    Errtr1.Visible = True
                    Errtr2.Visible = True
                End If
            End If
        End Try
    End Sub
End Class
