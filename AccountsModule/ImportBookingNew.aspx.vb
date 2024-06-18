Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports ClosedXML.Excel
Imports DocumentFormat.OpenXml
Imports System.Web.Services
Imports System.Linq
Imports Microsoft.VisualBasic.FileIO

Partial Class ImportBookingNew
    Inherits System.Web.UI.Page


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim sqlConn As SqlConnection
    Dim gvRow As GridViewRow
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'The maximum number of form, query string, or posted file items has already been read from the request. To change the maximum allowed request collection count from its current value of 50000
        Try
            If Page.IsPostBack = False Then

                Session("dtImportBooking") = Nothing
                Session("dtAmendedBooking") = Nothing
                Session("dtImportedBooking") = Nothing
                Session("dtCancelBooking") = Nothing
                Session("MismatchPIDt") = Nothing

                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("MapBookingState", Request.QueryString("State"))
                ViewState.Add("MapBookingCode", Request.QueryString("MapCode"))

                If ViewState("MapBookingState") = "New" Then
                    lblHeading.Text = "Import Booking"
                End If

                Dim checkDateFlag As String = Convert.ToString(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='5511'"))
                If String.IsNullOrWhiteSpace(checkDateFlag) Then
                    hdnChkDtFlag.Value = "N"
                Else
                    hdnChkDtFlag.Value = checkDateFlag
                End If

                Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id=509"), Integer)
                txtDecno.Text = decno.ToString()

                btnExcelNewBooking.Visible = False
                btnReleaseInvoiceSealed.Visible = False
                btnSave.Visible = True
                lblNotemapping.Visible = False
                lblNotemissing.Visible = False

                btnRefresh.Visible = False

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                lblFileName.Text = "No File Chosen"



                ' Tab grid view binding
                Tab_GridBinding()


            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region
#Region "    Private Sub BindGVMismatchPI()"
    Private Sub BindGVMismatchPI()
        Dim pdt As New DataTable
        pdt.Columns.Add("bookingElementId", GetType(Integer))
        pdt.Columns.Add("invoiceno", GetType(String))
        pdt.Columns.Add("acc_line_no", GetType(Integer))
        pdt.Columns.Add("ProductGroup", GetType(String))
        pdt.Columns.Add("supplier", GetType(String))
        pdt.Columns.Add("partycode", GetType(String))
        pdt.Columns.Add("supplierid", GetType(Integer))
        pdt.Columns.Add("acc_ref2", GetType(String))
        pdt.Columns.Add("Purchaseinvoiceno", GetType(String))
        pdt.Columns.Add("PIlineno", GetType(Integer))
        pdt.Columns.Add("Maplineno", GetType(Integer))
        pdt.Columns.Add("Reasons", GetType(String))
        Session("MismatchPIDt") = pdt
    End Sub
#End Region
#Region "Protected Sub btnmappiSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmappiSave.Click"
    Protected Sub btnmappiSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmappiSave.Click
        Try
            Dim dt As New DataTable
            dt.Columns.Add("bookingElementId", GetType(Integer))
            dt.Columns.Add("invoiceno", GetType(String))
            dt.Columns.Add("acc_line_no", GetType(Integer))
            dt.Columns.Add("ProductGroup", GetType(String))
            dt.Columns.Add("supplier", GetType(String))
            dt.Columns.Add("partycode", GetType(String))
            dt.Columns.Add("supplierid", GetType(Integer))
            dt.Columns.Add("acc_ref2", GetType(String))
            dt.Columns.Add("Purchaseinvoiceno", GetType(String))
            dt.Columns.Add("PIlineno", GetType(Integer))
            dt.Columns.Add("Maplineno", GetType(Integer))
            dt.Columns.Add("Reasons", GetType(String))

            Dim lblelementid As Label
            Dim lblinvoiceno As Label
            Dim hdnacctranlineno As HiddenField
            Dim lblPrdgrp As Label
            Dim hdnPrdgrp As HiddenField
            Dim lblsupplier As Label
            Dim hdnpartycode As HiddenField
            Dim hdnsupplierid As HiddenField
            Dim lblaccref1 As Label
            Dim lblpurchaseinvoiceno As Label
            Dim txtMapacclineno As TextBox
            Dim lblPIlineno As Label
            Dim lblreasons As Label

            For Each gvr As GridViewRow In gvMapdetail2.Rows
                lblelementid = CType(gvr.FindControl("lblelementid"), Label)
                lblinvoiceno = CType(gvr.FindControl("lblinvoiceno"), Label)
                hdnacctranlineno = CType(gvr.FindControl("hdnacctranlineno"), HiddenField)
                lblPrdgrp = CType(gvr.FindControl("lblPrdgrp"), Label)
                hdnPrdgrp = CType(gvr.FindControl("hdnPrdgrp"), HiddenField)
                lblsupplier = CType(gvr.FindControl("lblsupplier"), Label)
                hdnpartycode = CType(gvr.FindControl("hdnpartycode"), HiddenField)
                hdnsupplierid = CType(gvr.FindControl("hdnsupplierid"), HiddenField)
                lblaccref1 = CType(gvr.FindControl("lblaccref1"), Label)
                lblpurchaseinvoiceno = CType(gvr.FindControl("lblpurchaseinvoiceno"), Label)
                lblPIlineno = CType(gvr.FindControl("lblPIlineno"), Label)
                txtMapacclineno = CType(gvr.FindControl("txtMapacclineno"), TextBox)
                lblreasons = CType(gvr.FindControl("lblreasons"), Label)

                Dim dr As DataRow = dt.NewRow
                dr("bookingElementId") = CType(lblelementid.Text, Integer)
                dr("invoiceno") = CType(lblinvoiceno.Text, String)
                dr("acc_line_no") = CType(hdnacctranlineno.Value, Integer)
                dr("ProductGroup") = CType(hdnPrdgrp.Value, String)
                dr("supplier") = CType(lblsupplier.Text, String)
                dr("partycode") = CType(hdnpartycode.Value, String)
                dr("supplierid") = CType(hdnsupplierid.Value, String)
                dr("acc_ref2") = CType(lblaccref1.Text, String)
                dr("Purchaseinvoiceno") = CType(lblpurchaseinvoiceno.Text, String)
                dr("PIlineno") = CType(lblPIlineno.Text, Integer)
                dr("Maplineno") = CType(txtMapacclineno.Text, String)
                dr("Reasons") = CType(lblreasons.Text, String)
                dt.Rows.Add(dr)
            Next

            If dt.Rows.Count > 0 Then
                Session("MismatchPIDt") = dt
            Else
                Session("MismatchPIDt") = Nothing
            End If
            ModalExtraPopup.Hide()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Protected Sub gvReconcileSummary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReconcileSummary.RowDataBound"
    Protected Sub gvReconcileSummary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReconcileSummary.RowDataBound
        Dim lblBookingType As Label
        gvRow = e.Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            lblBookingType = gvRow.FindControl("lblBookingType")
            If lblBookingType.Text = "Purchase Invoice Created" Then
                e.Row.Visible = False
            End If


        End If
    End Sub
#End Region

#Region "Protected Sub gvImportedBooking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvImportedBooking.RowDataBound"
    Protected Sub gvImportedBooking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvImportedBooking.RowDataBound
        'Dim ispurchaseinvoice As Integer
        'gvRow = e.Row


        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
        '    ispurchaseinvoice = rowView("ispurchaseinvoice")


        '    If ispurchaseinvoice = 1 Then
        '        e.Row.Visible = False
        '    End If


        'End If
    End Sub
#End Region

    Private Sub TabHotelInfo_ActiveTabChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabHotelInfo.ActiveTabChanged
       Tab_GridBinding()
    End Sub

    Private Sub Tab_GridBinding()
        Dim dtNewBooking As New DataTable
        Dim dtAmendBooking As New DataTable
        Dim dtCancelBooking As New DataTable
        Dim dtErrorBooking As New DataTable
        Dim dtPIBooking As New DataTable
        Dim dtAlreadyBooking As New DataTable
        Dim dtreconciliation As New DataTable
        Dim dtreconciliationSummary As New DataTable

      

        If TabHotelInfo.ActiveTabIndex = 0 Then  ' new booking

            If Not (Session("dtImportBooking") Is Nothing) Then
                dtNewBooking = CType(Session("dtImportBooking"), DataTable)
                gvBookingDetails.DataSource = dtNewBooking
                gvBookingDetails.DataBind()
            End If
            gvAmendBooking.DataSource = Nothing
            gvAmendBooking.DataBind()
            GvCancelBooking.DataSource = Nothing
            GvCancelBooking.DataBind()
            gvImportedBooking.DataSource = Nothing
            gvImportedBooking.DataBind()
            gvMismatchpI.DataSource = Nothing
            gvMismatchpI.DataBind()
            gvReconcileSummary.DataSource = Nothing
            gvReconcileSummary.DataBind()
            gvReconcile.DataSource = Nothing
            gvReconcile.DataBind()
            gvIgnoreList.DataSource = Nothing
            gvIgnoreList.DataBind()

        ElseIf TabHotelInfo.ActiveTabIndex = 1 Then ' amend booking
            If Not (Session("dtAmendedBooking") Is Nothing) Then
                dtAmendBooking = CType(Session("dtAmendedBooking"), DataTable)
                gvAmendBooking.DataSource = dtAmendBooking
                gvAmendBooking.DataBind()
            End If

            gvBookingDetails.DataSource = Nothing
            gvBookingDetails.DataBind()
            GvCancelBooking.DataSource = Nothing
            GvCancelBooking.DataBind()
            gvImportedBooking.DataSource = Nothing
            gvImportedBooking.DataBind()
            gvMismatchpI.DataSource = Nothing
            gvMismatchpI.DataBind()
            gvReconcileSummary.DataSource = Nothing
            gvReconcileSummary.DataBind()
            gvReconcile.DataSource = Nothing
            gvReconcile.DataBind()
            gvIgnoreList.DataSource = Nothing
            gvIgnoreList.DataBind()

        ElseIf TabHotelInfo.ActiveTabIndex = 2 Then 'cancel booking
            If Not (Session("dtCancelBooking") Is Nothing) Then
                dtCancelBooking = CType(Session("dtCancelBooking"), DataTable)
                GvCancelBooking.DataSource = dtCancelBooking
                GvCancelBooking.DataBind()
            End If
            gvBookingDetails.DataSource = Nothing
            gvBookingDetails.DataBind()
            gvAmendBooking.DataSource = Nothing
            gvAmendBooking.DataBind()
            gvImportedBooking.DataSource = Nothing
            gvImportedBooking.DataBind()
            gvMismatchpI.DataSource = Nothing
            gvMismatchpI.DataBind()
            gvReconcileSummary.DataSource = Nothing
            gvReconcileSummary.DataBind()
            gvReconcile.DataSource = Nothing
            gvReconcile.DataBind()
            gvIgnoreList.DataSource = Nothing
            gvIgnoreList.DataBind()

        ElseIf TabHotelInfo.ActiveTabIndex = 3 Then 'already imported
            If Not (Session("dtImportedBooking") Is Nothing) Then
                dtAlreadyBooking = CType(Session("dtImportedBooking"), DataTable)
                gvImportedBooking.DataSource = dtAlreadyBooking
                gvImportedBooking.DataBind()
            End If

            gvBookingDetails.DataSource = Nothing
            gvBookingDetails.DataBind()
            gvAmendBooking.DataSource = Nothing
            gvAmendBooking.DataBind()
            GvCancelBooking.DataSource = Nothing
            GvCancelBooking.DataBind()
            gvMismatchpI.DataSource = Nothing
            gvMismatchpI.DataBind()
            gvReconcileSummary.DataSource = Nothing
            gvReconcileSummary.DataBind()
            gvReconcile.DataSource = Nothing
            gvReconcile.DataBind()
            gvIgnoreList.DataSource = Nothing
            gvIgnoreList.DataBind()

        ElseIf TabHotelInfo.ActiveTabIndex = 4 Then 'Reconciliation

            If Not (Session("ReconcileList") Is Nothing) Then
                dtreconciliation = CType(Session("ReconcileList"), DataTable)
                gvReconcile.DataSource = dtreconciliation
                gvReconcile.DataBind()
            End If
            gvBookingDetails.DataSource = Nothing
            gvBookingDetails.DataBind()
            gvAmendBooking.DataSource = Nothing
            gvAmendBooking.DataBind()
            gvImportedBooking.DataSource = Nothing
            gvImportedBooking.DataBind()
            gvMismatchpI.DataSource = Nothing
            gvMismatchpI.DataBind()
            GvCancelBooking.DataSource = Nothing
            GvCancelBooking.DataBind()
            gvReconcileSummary.DataSource = Nothing
            gvReconcileSummary.DataBind()
            gvIgnoreList.DataSource = Nothing
            gvIgnoreList.DataBind()
            

        ElseIf TabHotelInfo.ActiveTabIndex = 5 Then  'Reconciliation Summary

            If Not (Session("dtreconSummary") Is Nothing) Then
                dtreconciliationSummary = CType(Session("dtreconSummary"), DataTable)
                gvReconcileSummary.DataSource = dtreconciliationSummary
                gvReconcileSummary.DataBind()
            End If
            gvBookingDetails.DataSource = Nothing
            gvBookingDetails.DataBind()
            gvAmendBooking.DataSource = Nothing
            gvAmendBooking.DataBind()
            gvImportedBooking.DataSource = Nothing
            gvImportedBooking.DataBind()
            gvMismatchpI.DataSource = Nothing
            gvMismatchpI.DataBind()
            GvCancelBooking.DataSource = Nothing
            GvCancelBooking.DataBind()
            gvReconcile.DataSource = Nothing
            gvReconcile.DataBind()
            gvIgnoreList.DataSource = Nothing
            gvIgnoreList.DataBind()

        ElseIf TabHotelInfo.ActiveTabIndex = 6 Then 'PI mismatch
            If Not (Session("dtPIMismatchBooking") Is Nothing) Then
                dtPIBooking = CType(Session("dtPIMismatchBooking"), DataTable)
                gvMismatchpI.DataSource = dtPIBooking
                gvMismatchpI.DataBind()
            End If
            gvBookingDetails.DataSource = Nothing
            gvBookingDetails.DataBind()
            gvAmendBooking.DataSource = Nothing
            gvAmendBooking.DataBind()
            GvCancelBooking.DataSource = Nothing
            GvCancelBooking.DataBind()
            gvImportedBooking.DataSource = Nothing
            gvImportedBooking.DataBind()
            gvReconcileSummary.DataSource = Nothing
            gvReconcileSummary.DataBind()
            gvReconcile.DataSource = Nothing
            gvReconcile.DataBind()
            gvIgnoreList.DataSource = Nothing
            gvIgnoreList.DataBind()

        Else  'error booking

            If Not (Session("dtIgnoreList") Is Nothing) Then
                dtErrorBooking = CType(Session("dtIgnoreList"), DataTable)
                gvIgnoreList.DataSource = dtErrorBooking
                gvIgnoreList.DataBind()
            End If
            gvBookingDetails.DataSource = Nothing
            gvBookingDetails.DataBind()
            gvAmendBooking.DataSource = Nothing
            gvAmendBooking.DataBind()
            GvCancelBooking.DataSource = Nothing
            GvCancelBooking.DataBind()
            gvImportedBooking.DataSource = Nothing
            gvImportedBooking.DataBind()
            gvReconcileSummary.DataSource = Nothing
            gvReconcileSummary.DataBind()
            gvReconcile.DataSource = Nothing
            gvReconcile.DataBind()

        End If
    End Sub

    Protected Sub btnImportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportExcel.Click
        Dim lBeginTranStarted As Boolean = False
        Try

            hdnValidBooking.Value = 0
            btnSave.Visible = False
            Dim reconcileList As New List(Of Reconcile)
            reconcileList = createReconcileList()

            Session("dtImportBooking") = Nothing
            Session("dtAmendedBooking") = Nothing
            Session("dtImportedBooking") = Nothing
            Session("dtCancelBooking") = Nothing
            Session("dtImportBookingExcel") = Nothing
            Session("dtPIMismatchBooking") = Nothing
            Session("dtreconSummary") = Nothing
            Session("ReconcileList") = Nothing
            Session("dtIgnoreList") = Nothing

            Dim lReconSummSaleValue As Decimal = 0, lReconSummCostValue As Decimal = 0
            Dim reconcileSummaryList As New List(Of ReconcileSummary)
            gvReconcileSummary.DataSource = reconcileSummaryList
            gvReconcileSummary.DataBind()

            Dim MapbookingSummaryList As New List(Of Mapingbookinglist)
            gvMismatchpI.DataSource = MapbookingSummaryList
            gvMismatchpI.DataBind()

            If Not IsDate(txtChkFromDt.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter valid Arrival Date');", True)
                txtChkFromDt.Focus()
                Exit Sub
            End If

            If Not IsDate(txtChkToDt.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter valid Arrival Date');", True)
                txtChkToDt.Focus()
                Exit Sub
            End If

            If FileUploadExcel.HasFile Then
                txtID.Text = ""
                txtCount.Text = ""
                gvBookingDetails.DataSource = Nothing
                gvBookingDetails.DataBind()
                lblNotemapping.Visible = False
                lblNotemissing.Visible = False
                Session("dtImportBooking") = Nothing
                Session("MapFlag") = 0
                Session("ValidateFlag") = 0
                Dim FileName As String = Path.GetFileName(FileUploadExcel.PostedFile.FileName)
                Dim FileExtension As String = Path.GetExtension(FileUploadExcel.PostedFile.FileName)
                If FileExtension.ToLower.Trim = ".xls" Or FileExtension.ToLower.Trim = ".xlsx" Then
                    Dim filePath As String = Server.MapPath("~/ImportBookingExcel/") + FileName
                    lblFilePath.Text = filePath
                    lblFileName.Text = FileName
                    FileUploadExcel.SaveAs(filePath)

                    ' get unique import id

                    Dim bulkId As String = ""
                    Dim sptype As String = "BULKIMPORT"
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    sqlTrans = mySqlConn.BeginTransaction
                    lBeginTranStarted = True
                    If bulkId = "" Then
                        bulkId = objUtils.GetAutoDocNo(sptype, mySqlConn, sqlTrans)
                    End If
                    sqlTrans.Commit()
                    lBeginTranStarted = False

                    ' Insert into sql table
                    bulkInsert(filePath, bulkId)

                    ' Call validate procedure
                    'exec(sp_importBooking_validate_ExcelTable) '','13092023.xlsx','D:\Elevate\Issue_BigFile','2023-06-01','2023-08-30'
                    Dim dtNewBooking As New DataTable
                    Dim dtAmendBooking As New DataTable
                    Dim dtCancelBooking As New DataTable
                    Dim dtErrorBooking As New DataTable
                    Dim dtPIBooking As New DataTable
                    Dim dtAlreadyBooking As New DataTable
                    Dim dtreconciliation As New DataTable
                    Dim dtreconciliationSummary As New DataTable
                    Dim dsResult As New DataSet
                    Dim cancelflag As Integer = 1
                    If chkIgnoreCancel.Checked = True Then
                        cancelflag = 0
                    Else
                        cancelflag = 1
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    mySqlCmd = New SqlCommand("sp_importBooking_validate_ExcelTable", mySqlConn)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.CommandTimeout = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@FileName", SqlDbType.VarChar, 200)).Value = FileName
                    mySqlCmd.Parameters.Add(New SqlParameter("@filePath", SqlDbType.VarChar, -1)).Value = filePath
                    mySqlCmd.Parameters.Add(New SqlParameter("@arrivalFromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd")
                    mySqlCmd.Parameters.Add(New SqlParameter("@arrivalToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd")
                    mySqlCmd.Parameters.Add(New SqlParameter("@CancelFlag", SqlDbType.Int)).Value = cancelflag
                    Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
                    myAdapter.SelectCommand = mySqlCmd
                    myAdapter.Fill(dsResult)
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)

                    If dsResult.Tables.Count > 0 Then
                        dtCancelBooking = dsResult.Tables(0)
                        dtErrorBooking = dsResult.Tables(1)
                        dtPIBooking = dsResult.Tables(2)
                        dtAlreadyBooking = dsResult.Tables(3)
                        dtAmendBooking = dsResult.Tables(4)
                        dtNewBooking = dsResult.Tables(5)
                        dtreconciliation = dsResult.Tables(6)
                        dtreconciliationSummary = dsResult.Tables(7)
                    End If

                    ' New Booking bind
                    Session("dtImportBooking") = dtNewBooking

                    gvReconcile.DataSource = dtreconciliation
                    gvReconcile.DataBind()
                    TabHotelInfo.ActiveTabIndex = 4

                    If dtNewBooking.Rows.Count > 0 Or dtAmendBooking.Rows.Count > 0 Or dtCancelBooking.Rows.Count > 0 Then
                        hdnValidBooking.Value = 1
                    End If

                    ' Amend booking bind
                    Session("dtAmendedBooking") = dtAmendBooking
                    ' Already imported booking bind
                    Session("dtImportedBooking") = dtAlreadyBooking
                    ' PI Created booking bind
                    Session("dtPIMismatchBooking") = dtPIBooking
                    ' Cancel booking bind
                    Session("dtCancelBooking") = dtCancelBooking

                    Session("dtreconSummary") = dtreconciliationSummary
                    Session("ReconcileList") = dtreconciliation
                    Session("dtIgnoreList") = dtErrorBooking

                    If Val(hdnValidBooking.Value) > 0 Then
                        btnSave.Visible = True
                    Else
                        btnSave.Visible = False
                    End If
                    btnExcelNewBooking.Visible = True
                    btnReleaseInvoiceSealed.Visible = True

                End If
            End If

        Catch ex As Exception
            ModalPopupLoading.Hide()

            If lBeginTranStarted = True Then
                sqlTrans.Rollback()
            End If
            ' objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message, Session("GlobalUserName"))
            If Not Session("ErrorRowNumber") Is Nothing Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error in the row " + Session("ErrorRowNumber").ToString() + "- " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End If
        End Try
    End Sub
   

    Public Sub bulkInsert(ByVal filename As String, ByVal bulkId As String)

        'Dim filename As String = "D:\Elevate\Issue_BigFile\june-july2023.csv"
        Dim tablename As String = "Import_Excel_Data"
        Dim columncount As Integer = 25
        Dim table As DataTable = New DataTable()

        Dim cnt As Integer = 0
        

        Try

            Using workBook As New XLWorkbook(filename, XLEventTracking.Disabled)
                'table = workBook.Worksheet(1).Table(0).AsNativeDataTable()
                Dim workSheet As IXLWorksheet = workBook.Worksheet(1)
                ' Dim firstrow As IXLRow = workBook.Worksheet(1)

                'Dim firstrow As IXLRow = workSheet.FirstRowUsed()
                'Dim firstPossibleAddress As IXLAddress = workSheet.Row(firstrow.RowNumber()).FirstCell().Address
                'Dim lastPossibleAddress As IXLAddress = workSheet.LastCellUsed().Address

                'Dim range As IXLRange = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange()
                'table = range.AsTable()

               


                Dim firstrow As Boolean = True
                For Each row As IXLRow In workSheet.Rows
                    If firstrow Then
                        Dim emptyCell As Boolean = False

                        table.Columns.Add(New DataColumn("UniqueId", GetType(Integer)))
                        For Each cell As IXLCell In row.Cells(usedCellsOnly:=False)
                            If cell.Value.ToString().Trim() <> "" Then
                                table.Columns.Add(cell.Value.ToString().Trim().Replace(" ", ""))
                            Else
                                If emptyCell = True Then Exit For
                                emptyCell = True
                            End If
                        Next
                        table.Columns.Add(New DataColumn("importId", GetType(String)))
                        firstrow = False
                    Else

                        Dim dtColumnCnt As Integer = table.Columns.Count() - 1

                        If dtColumnCnt >= 0 Then
                            Dim dr As DataRow = table.NewRow
                            cnt = cnt + 1
                            dr("UniqueId") = cnt

                            Dim i As Integer = 1
                            Dim emptyCell As Boolean = False
                            For Each cell As IXLCell In row.Cells("1:" + dtColumnCnt.ToString())
                                If cell.Value.ToString().Trim() <> "" Then
                                    ' dr(i) = cell.Value.ToString()
                                    If i = 4 Or i = 5 Or i = 6 Or i = 7 Then  '
                                        dr(i) = Convert.ToDateTime(cell.Value.ToString()).ToShortDateString()
                                        'cell.RichText.ToString()
                                    Else
                                        dr(i) = cell.RichText.ToString()
                                    End If
                                Else
                                    If emptyCell = True Then Exit For
                                    emptyCell = True
                                End If

                                i = i + 1
                            Next

                            If Not IsDBNull(dr("Bookingcode")) Then 'Tanvir 28/12/2023
                                dr("importId") = bulkId
                                table.Rows.Add(dr)  'Tanvir 28/12/2023
                            End If

                    End If
                    End If
                Next


            End Using

        Catch ex1 As Exception

            Dim excelrownumber As Integer = cnt
            Session("ErrorRowNumber") = cnt
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error in the row " + cnt + "- " + ex1.Message.Replace("'", " ") & "' );", True)
            ' objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex1.Message.ToString + "Error in the row " + cnt, Session("GlobalUserName"))
           
            Throw ex1 ''.Message.ToString() + "Error in the row  number " + cnt
        End Try

        Dim dbConnection As SqlConnection
        dbConnection = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        ' Using dbConnection As SqlConnection = New SqlConnection(Session("dbconnectionName").ToString())

        Try


            If dbConnection.State <> ConnectionState.Open Then
                dbConnection.Open()
            End If

            Dim sqlBulk As SqlBulkCopy = New SqlBulkCopy(dbConnection)
            sqlBulk.DestinationTableName = tablename
            sqlBulk.BulkCopyTimeout = 0
            sqlBulk.WriteToServer(table)
            dbConnection.Close()

            ' objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), "Bulk save successfull: " & tablename, Session("GlobalUserName"))
            dbConnection.Close()
        Catch ex2 As Exception
            'objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex2.Message, Session("GlobalUserName"))
            dbConnection.Close()
            Throw ex2
        End Try
        ' End Using



    End Sub


    Protected Sub gvBookingDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBookingDetails.PageIndexChanging
        gvBookingDetails.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub

    Protected Sub gvAmendBooking_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAmendBooking.PageIndexChanging
        gvAmendBooking.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub
    Protected Sub GvCancelBooking_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvCancelBooking.PageIndexChanging
        GvCancelBooking.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub

    Protected Sub gvImportedBooking_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvImportedBooking.PageIndexChanging
        gvImportedBooking.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub

    Protected Sub gvMismatchpI_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMismatchpI.PageIndexChanging
        gvMismatchpI.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub

    Protected Sub gvReconcile_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReconcile.PageIndexChanging
        gvReconcile.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub

    Protected Sub gvReconcileSummary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReconcileSummary.PageIndexChanging
        gvReconcileSummary.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub

    Protected Sub gvIgnoreList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvIgnoreList.PageIndexChanging
        gvIgnoreList.PageIndex = e.NewPageIndex
        Tab_GridBinding()
    End Sub



    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        Dim dt As DataTable = New DataTable()
        Dim dtEdit As DataTable = New DataTable()
        Dim lBeginTranStarted As Boolean = False
        Dim optionval As String = ""
        ' rosalin 23/03/2023
        Dim FailedBookings As String = ""
        Dim SuccessCount As Integer = 0
        Dim CancelCount As Integer = 0

        Dim EditFailedBookings As String = ""
        Dim EditSuccessCount As Integer = 0

        Try

            ' New booking insert
            If Not (Session("dtImportBooking") Is Nothing) Then
                dt = CType(Session("dtImportBooking"), DataTable)

                If dt.Rows.Count > 0 Then

                    'serviceType,slineNo,import_id,importlineno, noNights ,SalesTax , CostTax
                    'ispurchaseinvoice, isMultiCost, servicecount, isAmended, isAlreadyImport, isCancelled, checkHotelSupplier, checkIgnore
                    'dt.Columns.Remove("serviceType")  'dt.Columns.Remove("slineNo") 'dt.Columns.Remove("import_id")  'dt.Columns.Remove("importlineno")
                    'dt.Columns.Remove("noNights") 'dt.Columns.Remove("SalesTax") 'dt.Columns.Remove("CostTax") 'dt.Columns.Remove("ispurchaseinvoice")
                    'dt.Columns.Remove("isMultiCost") 'dt.Columns.Remove("servicecount") 'dt.Columns.Remove("isAmended") 'dt.Columns.Remove("isAlreadyImport")
                    'dt.Columns.Remove("isCancelled") 'dt.Columns.Remove("checkHotelSupplier") 'dt.Columns.Remove("checkIgnore")
                    Dim grpbooking = (From n In dt.AsEnumerable Group By Bookingcode = n.Field(Of String)("Bookingcode") Into grp = Group Order By Bookingcode Select New With {.Bookingcode = Bookingcode}).ToList()

                    Dim sptype As String = "MAPBOOKING"
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


                    For Each grpItem In grpbooking
                        Try
                            If Not grpItem Is Nothing Then
                                If Not grpItem.Bookingcode Is Nothing Then
                                    If grpItem.Bookingcode.ToString() <> "" Then
                                        sqlTrans = mySqlConn.BeginTransaction           'SQL Trans start
                                        lBeginTranStarted = True
                                        If optionval = "" Then
                                            optionval = objUtils.GetAutoDocNo(sptype, mySqlConn, sqlTrans)
                                        End If

                                        Dim bookingCode As String = grpItem.Bookingcode.ToString
                                        Dim bookingDt As DataTable = (From n In dt.AsEnumerable Where n.Field(Of String)("Bookingcode") = bookingCode Select n).CopyToDataTable()
                                        bookingDt.TableName = "BookingServices"
                                        Dim bookingXml As String = objUtils.GenerateXML(bookingDt)

                                        mySqlCmd = New SqlCommand("sp_add_importBooking_CR", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure
                                        mySqlCmd.Parameters.Add(New SqlParameter("@import_id", SqlDbType.VarChar, 20)).Value = optionval
                                        mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@filename", SqlDbType.VarChar, 200)).Value = lblFileName.Text.Trim
                                        mySqlCmd.Parameters.Add(New SqlParameter("@bookingCode", SqlDbType.VarChar, 20)).Value = bookingCode
                                        mySqlCmd.Parameters.Add(New SqlParameter("@bookingXml", SqlDbType.VarChar, -1)).Value = bookingXml
                                        mySqlCmd.CommandTimeout = 0
                                        mySqlCmd.ExecuteNonQuery()
                                        mySqlCmd.Dispose()
                                        sqlTrans.Commit()
                                        SuccessCount = SuccessCount + 1
                                        lBeginTranStarted = False
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            If lBeginTranStarted = True Then
                                sqlTrans.Rollback()
                            End If

                            If FailedBookings = "" Then
                                FailedBookings = FailedBookings + grpItem.Bookingcode.ToString
                            Else
                                FailedBookings = FailedBookings + "," + grpItem.Bookingcode.ToString
                            End If
                            ' insert into errorlog table
                            Dim mySqlCmd1 As SqlCommand
                            mySqlCmd1 = New SqlCommand("sp_add_importBooking_Error", mySqlConn)
                            mySqlCmd1.CommandType = CommandType.StoredProcedure
                            mySqlCmd1.Parameters.Add(New SqlParameter("@bookingCode", SqlDbType.VarChar, 20)).Value = grpItem.Bookingcode.ToString.Trim
                            mySqlCmd1.Parameters.Add(New SqlParameter("@errorMsg", SqlDbType.VarChar, -1)).Value = ex.Message
                            mySqlCmd1.Parameters.Add(New SqlParameter("@import_id", SqlDbType.VarChar, 20)).Value = optionval
                            mySqlCmd1.ExecuteNonQuery()
                            mySqlCmd1.Dispose()
                        End Try
                    Next
                End If
            End If

            ' Edit booking insert

            If Not (Session("dtAmendedBooking") Is Nothing) Then
                dtEdit = CType(Session("dtAmendedBooking"), DataTable)
                If dtEdit.Rows.Count > 0 Then
                    Dim grpbooking = (From n In dtEdit.AsEnumerable Group By Bookingcode = n.Field(Of String)("Bookingcode") Into grp = Group Order By Bookingcode Select New With {.Bookingcode = Bookingcode}).ToList()

                    Dim sptype As String = "MAPBOOKING"
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                    For Each grpItem In grpbooking
                        Try
                            If Not grpItem Is Nothing Then
                                If Not grpItem.Bookingcode Is Nothing Then
                                    If grpItem.Bookingcode.ToString() <> "" Then
                                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                                        lBeginTranStarted = True
                                        If optionval = "" Then
                                            optionval = objUtils.GetAutoDocNo(sptype, mySqlConn, sqlTrans)
                                        End If

                                        Dim bookingCode As String = grpItem.Bookingcode.ToString
                                        Dim bookingDt As DataTable = (From n In dtEdit.AsEnumerable Where n.Field(Of String)("Bookingcode") = bookingCode Select n).CopyToDataTable()
                                        bookingDt.TableName = "BookingServices"
                                        Dim bookingXml As String = objUtils.GenerateXML(bookingDt)

                                        mySqlCmd = New SqlCommand("sp_add_importBooking_CR", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure
                                        mySqlCmd.Parameters.Add(New SqlParameter("@import_id", SqlDbType.VarChar, 20)).Value = optionval
                                        mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@filename", SqlDbType.VarChar, 200)).Value = lblFileName.Text.Trim
                                        mySqlCmd.Parameters.Add(New SqlParameter("@bookingCode", SqlDbType.VarChar, 20)).Value = bookingCode
                                        mySqlCmd.Parameters.Add(New SqlParameter("@bookingXml", SqlDbType.VarChar, -1)).Value = bookingXml
                                        mySqlCmd.CommandTimeout = 0
                                        mySqlCmd.ExecuteNonQuery()
                                        mySqlCmd.Dispose()
                                        sqlTrans.Commit()
                                        EditSuccessCount = EditSuccessCount + 1
                                        lBeginTranStarted = False
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            If lBeginTranStarted = True Then
                                sqlTrans.Rollback()
                            End If

                            If EditFailedBookings = "" Then
                                EditFailedBookings = EditFailedBookings + grpItem.Bookingcode.ToString
                            Else
                                EditFailedBookings = EditFailedBookings + "," + grpItem.Bookingcode.ToString
                            End If
                            ' insert into errorlog table
                            Dim mySqlCmd1 As SqlCommand
                            mySqlCmd1 = New SqlCommand("sp_add_importBooking_Error", mySqlConn)
                            mySqlCmd1.CommandType = CommandType.StoredProcedure
                            mySqlCmd1.Parameters.Add(New SqlParameter("@bookingCode", SqlDbType.VarChar, 20)).Value = grpItem.Bookingcode.ToString.Trim
                            mySqlCmd1.Parameters.Add(New SqlParameter("@errorMsg", SqlDbType.VarChar, -1)).Value = ex.Message
                            mySqlCmd1.Parameters.Add(New SqlParameter("@import_id", SqlDbType.VarChar, 20)).Value = optionval
                            mySqlCmd1.ExecuteNonQuery()
                            mySqlCmd1.Dispose()
                        End Try
                    Next



                End If
            End If

            ' Cancel Booking
            Dim Canceldt As New DataTable()
            Dim cancelBookingXml As String = ""
            Canceldt.Columns.Add(New DataColumn("Bookingcode", GetType(String)))
            Dim cancelbookings As String = ""

         
                'For Each grv As GridViewRow In GvCancelBooking.Rows
                '    Dim drow As DataRow = Canceldt.NewRow
                '    lblBookingNo = CType(grv.FindControl("lblBookingNo"), Label)
                '    If Not (cancelbookings.Contains(lblBookingNo.Text)) Then
                '        cancelbookings = cancelbookings + "," + lblBookingNo.Text.Trim
                '        drow("Bookingcode") = lblBookingNo.Text.Trim
                '        Canceldt.Rows.Add(drow)
                '    End If
                'Next

                If Not (Session("dtCancelBooking") Is Nothing) Then


                Dim Canceldtmain As DataTable = CType(Session("dtCancelBooking"), DataTable)
                If Canceldtmain.Rows.Count > 0 Then
                    Dim grpbooking = (From n In Canceldtmain.AsEnumerable Group By Bookingcode = n.Field(Of String)("Bookingcode") Into grp = Group Order By Bookingcode Select New With {.Bookingcode = Bookingcode}).ToList()
                    For Each grpItem In grpbooking

                        Dim drow As DataRow = Canceldt.NewRow
                        drow("Bookingcode") = grpItem.Bookingcode.ToString.Trim()
                        Canceldt.Rows.Add(drow)

                    Next
                End If
                End If

                ' Dim sptype As String = "MAPBOOKING"
                ' mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


        Try
            If Canceldt.Rows.Count > 0 Then
                Canceldt.TableName = "cancelBookings"
                    cancelBookingXml = objUtils.GenerateXML(Canceldt)
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    sqlTrans = mySqlConn.BeginTransaction
                    lBeginTranStarted = True
                mySqlCmd = New SqlCommand("sp_update_importBooking", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@cancelBookingXml", SqlDbType.VarChar, -1)).Value = cancelBookingXml
                mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandTimeout = 0
                mySqlCmd.ExecuteNonQuery()
                mySqlCmd.Dispose()
                sqlTrans.Commit()
                    lBeginTranStarted = False
                    CancelCount = Canceldt.Rows.Count

                End If
            Catch ex As Exception
                If lBeginTranStarted = True Then
                    sqlTrans.Rollback()
                End If
            End Try



            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)



            ' Failed booking display
            Dim dtR As DataTable = New DataTable()
            Dim dr As DataRow = dtR.NewRow
            dtR.Columns.Add(New DataColumn("Details", GetType(String)))
            dtR.Columns.Add(New DataColumn("Counts", GetType(Integer)))
            dtR.Columns.Add(New DataColumn("Bookingcode", GetType(String)))
            dtR.Columns.Add(New DataColumn("Error", GetType(String)))

            If SuccessCount > 0 Then
                dr("Details") = "New Bookings"
                dr("Counts") = SuccessCount
                dr("Bookingcode") = ""
                dr("Error") = "Success"
                dtR.Rows.Add(dr)
            End If

            If EditSuccessCount > 0 Then
                Dim dr2 As DataRow = dtR.NewRow
                dr2("Details") = "Amend Bookings"
                dr2("Counts") = EditSuccessCount
                dr2("Bookingcode") = ""
                dr2("Error") = "Success"
                dtR.Rows.Add(dr2)
            End If

            If CancelCount > 0 And Canceldt.Rows.Count > 0 Then
                Dim dr1 As DataRow = dtR.NewRow
                dr1("Details") = "Cancel Bookings"
                dr1("Counts") = CancelCount
                dr1("Bookingcode") = ""
                dr1("Error") = "Success"
                dtR.Rows.Add(dr1)
            End If

            If FailedBookings <> "" Then
                Dim listFail() As String = FailedBookings.Split(",")

                For i As Integer = 0 To listFail.Count - 1
                    Dim dr2 As DataRow = dtR.NewRow
                    dr2("Details") = "New Failed Booking"
                    dr2("Counts") = 1
                    dr2("Bookingcode") = listFail(i).ToString
                    dr2("Error") = "Failed"
                    dtR.Rows.Add(dr2)
                Next
            End If

            If EditFailedBookings <> "" Then
                Dim listFail1() As String = EditFailedBookings.Split(",")

                For i As Integer = 0 To listFail1.Count - 1
                    Dim dr2 As DataRow = dtR.NewRow
                    dr2("Details") = "Amend Failed Booking"
                    dr2("Counts") = 1
                    dr2("Bookingcode") = listFail1(i).ToString
                    dr2("Error") = "Failed"
                    dtR.Rows.Add(dr2)
                Next
            End If



            gvImportResult.DataSource = dtR
            gvImportResult.DataBind()

            mpImportResult.Show()
            ModalPopupLoading.Hide()

            btnSave.Visible = False



        Catch ex As Exception
            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Protected Sub btnImportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportExcel.Click"
    Protected Sub btnImportExcel_Click_old(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles btnImportExcel.Click
        Try
            Session("dtImportBooking") = Nothing
            Session("dtAmendedBooking") = Nothing
            Session("dtImportedBooking") = Nothing
            Session("dtCancelBooking") = Nothing
            Session("dtImportBookingExcel") = Nothing ' sharfudeen 24/10/2022

            hdnValidBooking.Value = 0
            btnSave.Visible = False

            Dim reconcileList As New List(Of Reconcile)
            reconcileList = createReconcileList()

            Dim lReconSummSaleValue As Decimal = 0, lReconSummCostValue As Decimal = 0
            Dim reconcileSummaryList As New List(Of ReconcileSummary)
            gvReconcileSummary.DataSource = reconcileSummaryList
            gvReconcileSummary.DataBind()

            'Sharfudeen 24/10/2022
            Dim MapbookingSummaryList As New List(Of Mapingbookinglist)
            gvMismatchpI.DataSource = MapbookingSummaryList
            gvMismatchpI.DataBind()

            If Not IsDate(txtChkFromDt.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter valid Arrival Date');", True)
                txtChkFromDt.Focus()
                Exit Sub
            End If

            If Not IsDate(txtChkToDt.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter valid Arrival Date');", True)
                txtChkToDt.Focus()
                Exit Sub
            End If

            If FileUploadExcel.HasFile Then
                txtID.Text = ""
                txtCount.Text = ""
                gvBookingDetails.DataSource = Nothing
                gvBookingDetails.DataBind()
                lblNotemapping.Visible = False
                lblNotemissing.Visible = False
                Session("dtImportBooking") = Nothing
                Session("MapFlag") = 0
                Session("ValidateFlag") = 0
                Dim FileName As String = Path.GetFileName(FileUploadExcel.PostedFile.FileName)
                Dim FileExtension As String = Path.GetExtension(FileUploadExcel.PostedFile.FileName)
                If FileExtension.ToLower.Trim = ".xls" Or FileExtension.ToLower.Trim = ".xlsx" Then
                    Dim filePath As String = Server.MapPath("~/ImportBookingExcel/") + FileName
                    lblFilePath.Text = filePath
                    lblFileName.Text = FileName
                    FileUploadExcel.SaveAs(filePath)
                    Dim dt As New DataTable()
                    dt.Columns.Add(New DataColumn("UniqueId", GetType(Integer)))
                    dt.Columns.Add(New DataColumn("Bookingcode", GetType(String)))
                    dt.Columns.Add(New DataColumn("Mainpssgrname", GetType(String)))
                    dt.Columns.Add(New DataColumn("Date", GetType(String)))
                    dt.Columns.Add(New DataColumn("Linebookingdate", GetType(String)))
                    dt.Columns.Add(New DataColumn("Startdate", GetType(String)))
                    dt.Columns.Add(New DataColumn("Enddate", GetType(String)))
                    dt.Columns.Add(New DataColumn("Suppname", GetType(String)))
                    dt.Columns.Add(New DataColumn("SuppID", GetType(String)))
                    dt.Columns.Add(New DataColumn("ExchRate", GetType(String)))
                    dt.Columns.Add(New DataColumn("Description", GetType(String)))
                    dt.Columns.Add(New DataColumn("SalesPrice", GetType(String)))
                    dt.Columns.Add(New DataColumn("Salescurr", GetType(String)))
                    dt.Columns.Add(New DataColumn("Prodgrp", GetType(String)))
                    dt.Columns.Add(New DataColumn("Costprice", GetType(String)))
                    dt.Columns.Add(New DataColumn("Costcurr", GetType(String)))
                    dt.Columns.Add(New DataColumn("SupprefNum", GetType(String)))
                    dt.Columns.Add(New DataColumn("Nationality", GetType(String)))
                    dt.Columns.Add(New DataColumn("NoofRooms", GetType(String)))
                    dt.Columns.Add(New DataColumn("Agency", GetType(String)))
                    dt.Columns.Add(New DataColumn("Agencybookingref", GetType(String)))
                    dt.Columns.Add(New DataColumn("ClientID", GetType(String)))
                    dt.Columns.Add(New DataColumn("CtryAgency", GetType(String)))
                    dt.Columns.Add(New DataColumn("Mapagentcode", GetType(String)))
                    dt.Columns.Add(New DataColumn("Mappartycode", GetType(String)))
                    dt.Columns.Add(New DataColumn("Mapsalecurr", GetType(String)))
                    dt.Columns.Add(New DataColumn("Mapctryagent", GetType(String)))
                    dt.Columns.Add(New DataColumn("checkHotelSupplier", GetType(String)))   ' Supplier is belongs to Hotel or not
                    dt.Columns.Add(New DataColumn("checkIgnore", GetType(String)))
                    dt.Columns.Add(New DataColumn("mapService", GetType(String)))
                    dt.Columns.Add(New DataColumn("PaxNumber", GetType(String)))
                    dt.Columns.Add(New DataColumn("bookingElementId", GetType(String)))
                    dt.Columns.Add(New DataColumn("ispurchaseinvoice", GetType(Integer))) ' sharfudeen 17/10/2022

                    Dim cnt As Integer = 0
                    Dim excelDt As New DataTable()
                    Using workBook As New XLWorkbook(filePath)
                        Dim sheetcnt As Integer = 1
                        'While workBook.Worksheets.Count >= sheetcnt
                        Dim workSheet As IXLWorksheet = workBook.Worksheet(sheetcnt)
                        Dim firstRow As Boolean = True
                        For Each row As IXLRow In workSheet.Rows()
                            If firstRow Then
                                Dim emptyCell As Boolean = False
                                excelDt.Columns.Add(New DataColumn("UniqueId", GetType(Integer)))
                                For Each cell As IXLCell In row.Cells(usedCellsOnly:=False)
                                    If cell.Value.ToString().Trim() <> "" Then
                                        excelDt.Columns.Add(cell.Value.ToString().Trim().Replace(" ", ""))
                                    Else
                                        If emptyCell = True Then Exit For
                                        emptyCell = True
                                    End If
                                Next
                                firstRow = False
                            Else
                                Dim dtColumnCnt As Integer = excelDt.Columns.Count() - 1  'uniqueId
                                If dtColumnCnt >= 0 Then
                                    Dim dr As DataRow = excelDt.NewRow
                                    cnt = cnt + 1
                                    dr("UniqueId") = cnt

                                    Dim i As Integer = 1

                                    For Each cell As IXLCell In row.Cells("1:" + dtColumnCnt.ToString())
                                        dr(i) = cell.Value.ToString()
                                        i = i + 1
                                    Next

                                    If dr("Bookingcode") <> "" Then
                                        excelDt.Rows.Add(dr)
                                    End If
                                End If
                            End If
                        Next
                        'sheetcnt += 1
                        'End While
                    End Using

                    Dim decno As Integer = Convert.ToInt32(txtDecno.Text)

                    If excelDt.Rows.Count > 0 Then

                        For Each excelDr As DataRow In excelDt.Rows
                            Dim dr As DataRow = dt.NewRow
                            For Each excelCol As DataColumn In excelDt.Columns
                                Select Case (excelCol.ColumnName.Trim().ToLower())
                                    Case "uniqueid"
                                        dr("UniqueId") = Convert.ToInt32(excelDr(excelCol))

                                    Case "bookingcode"
                                        dr("Bookingcode") = Convert.ToString(excelDr(excelCol))

                                    Case "startdate"
                                        If IsDate(excelDr(excelCol)) Then
                                            dr("Startdate") = Convert.ToDateTime(excelDr(excelCol)).ToString("dd/MM/yyyy")
                                        Else
                                            dr("Startdate") = ""
                                        End If

                                    Case "enddate"
                                        If IsDate(excelDr(excelCol)) Then
                                            dr("Enddate") = Convert.ToDateTime(excelDr(excelCol)).ToString("dd/MM/yyyy")
                                        Else
                                            dr("Enddate") = ""
                                        End If
                                    Case "agency"
                                        dr("Agency") = Convert.ToString(excelDr(excelCol))

                                    Case "agencybookingreference"
                                        dr("Agencybookingref") = Convert.ToString(excelDr(excelCol))

                                    Case "mainpassengername"
                                        dr("Mainpssgrname") = Convert.ToString(excelDr(excelCol))

                                    Case "description"
                                        dr("Description") = Convert.ToString(excelDr(excelCol))

                                    Case "productgroup"
                                        dr("Prodgrp") = Convert.ToString(excelDr(excelCol))

                                    Case "salescurrency"
                                        dr("Salescurr") = Convert.ToString(excelDr(excelCol))

                                    Case "salesprice"
                                        dr("SalesPrice") = Convert.ToString(Math.Round(Val(excelDr(excelCol)), decno))

                                    Case "suppliername"
                                        dr("Suppname") = Convert.ToString(excelDr(excelCol))

                                    Case "costcurrency"
                                        dr("Costcurr") = Convert.ToString(excelDr(excelCol))

                                    Case "costprice"
                                        dr("Costprice") = Convert.ToString(Math.Round(Val(excelDr(excelCol)), decno))

                                    Case "date"
                                        If IsDate(excelDr(excelCol)) Then
                                            dr("Date") = Convert.ToString(excelDr(excelCol))
                                        Else
                                            dr("Date") = ""
                                        End If

                                    Case "linebookingdate"
                                        If IsDate(excelDr(excelCol)) Then
                                            dr("Linebookingdate") = Convert.ToString(excelDr(excelCol))
                                        Else
                                            dr("Linebookingdate") = ""
                                        End If
                                    Case "clientid"
                                        dr("ClientID") = Convert.ToString(excelDr(excelCol))

                                    Case "supplierid"
                                        dr("SuppID") = Convert.ToString(excelDr(excelCol))

                                    Case "paxnumber"
                                        dr("PaxNumber") = Convert.ToString(excelDr(excelCol))

                                    Case "bookingelementid"
                                        dr("bookingElementId") = Convert.ToString(excelDr(excelCol))

                                End Select
                            Next

                            If Convert.ToString(dr("Description")).Trim() = "" Then
                                dr("Description") = "Handling Fee"
                                dr("Prodgrp") = "Handling fees"
                            ElseIf Convert.ToString(dr("Description")).Trim().Contains("Handling fee") = True Then 'changed by mohamed on 07/06/2021
                                dr("Prodgrp") = "Handling fees"
                            End If
                            'Tanvir 09/01/2023
                            Dim servicetype As String = CType(objUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select isnull(servicetype,'') servicetype from int_productgroup where productgroup='" & dr("Prodgrp") & "'"), String)
                            If servicetype Is Nothing Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Product group " + dr("Prodgrp") + "  is not Defined. ');", True)
                                Exit Sub
                            End If



                            ''Dim AGentcurrency As String = CType(objUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select isnull(currcode,'') currcode from agentmast where agentname='" & dr("Agency") & "'"), String)
                            ''If Not AGentcurrency Is Nothing Then
                            ''    If AGentcurrency <> dr("Salescurr") Then
                            ''        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Agent Currency " + dr("Salescurr") + "  is  Different. ');", True)
                            ''        Exit Sub
                            ''    End If
                            ''End If
                            'Tanvir 09/01/2023
                            dt.Rows.Add(dr)

                        Next

                    End If

                    Dim finalDt As New DataTable
                    Dim ImportexcelDt As New DataTable
                    If dt.Rows.Count > 0 Then
                        Dim orderDt As DataTable = (From n In dt.AsEnumerable Order By n.Field(Of String)("bookingCode"), n.Field(Of String)("startdate"), n.Field(Of String)("enddate") Select n).CopyToDataTable()
                        finalDt = mappingBooking(orderDt)


                        'ImportexcelDt = finalDt

                        ''sharfudeen 24/10/2022

                        '  Dim ImportexcelDt As DataTable = CType(finalDt, DataTable).Clone
                        If finalDt.Rows.Count > 0 Then
                            ImportexcelDt = finalDt.Clone()
                            For i As Integer = 0 To finalDt.Rows.Count - 1
                                ImportexcelDt.ImportRow(finalDt.Rows(i))
                            Next
                        End If
                        If ImportexcelDt.Rows.Count > 0 Then
                            Session("dtImportBookingExcel") = ImportexcelDt
                        End If


                        Dim grpArrDate = (From n In finalDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode, .arrivalDate = grp.Min(Function(x) Convert.ToDateTime(x.Field(Of String)("startdate"))), .departureDate = grp.Max(Function(x) Convert.ToDateTime(x.Field(Of String)("enddate")))}).ToList()
                        Dim tmpfromdate As DateTime = grpArrDate.Min(Function(x) x.arrivalDate)
                        Dim tmptodate As DateTime = grpArrDate.Max(Function(x) x.arrivalDate)
                        'If IsDate(txtChkFromDt.Text) Then
                        '    Dim fromdate As DateTime = Convert.ToDateTime(txtChkFromDt.Text)
                        '    If tmpfromdate < fromdate Or fromdate > tmptodate Then
                        '        txtChkFromDt.Text = tmpfromdate.ToString("dd/MM/yyyy")
                        '    End If
                        'Else
                        '    txtChkFromDt.Text = tmpfromdate
                        'End If

                        'If IsDate(txtChkToDt.Text) Then
                        '    Dim todate As DateTime = Convert.ToDateTime(txtChkToDt.Text)
                        '    If tmptodate > todate Then
                        '        txtChkToDt.Text = tmptodate.ToString("dd/MM/yyyy")
                        '    End If
                        'Else
                        '    txtChkToDt.Text = tmpfromdate
                        'End If
                    End If

                    Dim bookingDt As New DataTable
                    bookingDt.Columns.Add("bookingCode")
                    Dim grpbooking = (From n In finalDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode}).ToList()
                    If grpbooking.Count > 0 Then
                        For i = 0 To grpbooking.Count - 1
                            Dim grpDr As DataRow = bookingDt.NewRow
                            grpDr("bookingCode") = grpbooking(i).bookingCode.Trim
                            bookingDt.Rows.Add(grpDr)
                        Next
                    End If

                    'check duplicate records 'changed by mohamed on 04/11/2021
                    Dim lsDuplicateBookings As String = ""
                    Dim grpDuplicatedt1 As DataTable = finalDt.DefaultView.ToTable(True, {"bookingCode", "Salescurr"})
                    Dim duplicates14 = grpDuplicatedt1.AsEnumerable().GroupBy(Function(i) i.Field(Of String)("bookingCode")).Where(Function(g) g.Count() > 1).Select(Function(g) g.Key).Distinct
                    For Each ed In duplicates14
                        lsDuplicateBookings = lsDuplicateBookings & IIf(lsDuplicateBookings = "", "", ", ") & ed.ToString()
                    Next
                    If lsDuplicateBookings.Trim <> "" Then
                        Dim lsDuplicateBookingsAlert As String = "alert('These bookings have two selling currencies " & lsDuplicateBookings & "');"
                        gvBookingDetails.DataSource = Nothing
                        gvBookingDetails.DataBind()
                        Session("dtImportBooking") = Nothing
                        btnSave.Visible = False
                        btnExcelNewBooking.Visible = False
                        btnReleaseInvoiceSealed.Visible = False
                        lblNotemapping.Visible = False
                        lblNotemissing.Visible = False
                        btnRefresh.Visible = False
                        Session("MapFlag") = 0
                        Session("ValidateFlag") = 0
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", lsDuplicateBookingsAlert, True)

                        Exit Sub
                    End If
                    'Tanvir 22062023
                    Dim lsDuplicateBookingelementid As String = ""
                    Dim grpDuplicatedt1ele As DataTable = finalDt.DefaultView.ToTable(False, {"bookingCode", "bookingelementid"})

                    Dim dtOutput As New DataTable

                    dtOutput.Columns.Add("bookingCode")
                    dtOutput.Columns.Add("bookingelementid")
                    dtOutput.Columns.Add("count")

                    For Each row As DataRow In grpDuplicatedt1ele.Rows
                        Dim query As String = String.Format("bookingCode='{0}' and bookingelementid='{1}'  ", row(0), row(1))
                        Dim count As Integer = grpDuplicatedt1ele.Select(query).Length
                        dtOutput.Rows.Add(row(0), row(1), count)
                    Next
                    If dtOutput.Rows.Count > 0 Then
                        For Each row As DataRow In dtOutput.Rows
                            If row("count") > 1 Then

                                Dim dupalert As String = "alert('Duplicate Bookingelement Id, Please Check Booking code : " & row("bookingcode") & " BookingElementID: " & row("bookingelementid") & "');"
                                ' Dim lsDuplicateBookingsAlert As String = "alert('These bookings have two selling currencies " & lsDuplicateBookings & "');"
                                gvBookingDetails.DataSource = Nothing
                                gvBookingDetails.DataBind()
                                Session("dtImportBooking") = Nothing
                                btnSave.Visible = False
                                btnExcelNewBooking.Visible = False
                                btnReleaseInvoiceSealed.Visible = False
                                lblNotemapping.Visible = False
                                lblNotemissing.Visible = False
                                btnRefresh.Visible = False
                                Session("MapFlag") = 0
                                Session("ValidateFlag") = 0
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", dupalert, True)

                                Exit Sub

                            End If
                        Next
                    End If

                    'Tanvir 22062023

                    If finalDt.Rows.Count > 0 Then
                        'Amend bookings
                        'sharfudeen 17/10/2022
                        Dim AmendDt As DataTable = fnAmendedBookingsNew(bookingDt, finalDt) '  fnAmendedBookings(bookingDt)

                        'Sharfudeen 17/10/2022
                        'Dim grpamended = (From n In AmendDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                        '                    Select New With {.bookingCode = bookingCode, .totalServ = grp.Count, .ispurchaseinvoice = grp.Min(Function(x) Convert.ToInt32(x.Field(Of Integer)("ispurchaseinvoice")))}).ToList()


                        'sharfudeen 19/09/2022
                        Dim grpamended = (From n In AmendDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode"), ispurchaseinvoice = n.Field(Of Integer)("ispurchaseinvoice") Into grp = Group Order By bookingCode _
                                            Select New With {.bookingCode = bookingCode, .totalServ = grp.Count, .ispurchaseinvoice = grp.Min(Function(x) Convert.ToInt32(x.Field(Of Integer)("ispurchaseinvoice")))}).ToList()

                        ' Dim grpamended = (From n In AmendDt.AsEnumerable Where n.Field(Of Integer)("ispurchaseinvoice") = 0 Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode, .totalServ = grp.Count}).ToList()


                        'Matching booking from Final datatale
                        'sharfudeen 04/10/2022
                        '  Dim tmpMatch = (From n In finalDt.AsEnumerable Join m In grpamended On n.Field(Of String)("bookingcode") Equals m.bookingCode Select n)
                        Dim tmpMatch = (From n In finalDt.AsEnumerable Join m In grpamended On n.Field(Of String)("bookingcode").Trim Equals m.bookingCode.Trim Select n)
                        Dim tmpMatchDt As New DataTable
                        If tmpMatch.Count > 0 Then
                            tmpMatchDt = tmpMatch.CopyToDataTable()
                        End If
                        Dim grpTmpMatch = (From n In tmpMatchDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode, .totalServ = grp.Count}).ToList()

                        'list of booking with Different no of services 
                        'sharfudeen 19/09/2022
                        '  Dim tmpServDiff = (From n In grpamended Join m In grpTmpMatch On n.bookingCode Equals m.bookingCode Where n.totalServ <> m.totalServ Select m)
                        Dim tmpServDiff = (From n In grpamended Join m In grpTmpMatch On n.bookingCode.Trim Equals m.bookingCode.Trim Where n.totalServ <> m.totalServ And n.ispurchaseinvoice = 0 Select m)

                        Dim finalAmendDt As New DataTable
                        If tmpServDiff.Count > 0 Then
                            Dim modBooking = (From n In finalDt.AsEnumerable Join m In tmpServDiff On n.Field(Of String)("bookingcode").Trim Equals m.bookingCode.Trim Select n)
                            If modBooking.Count > 0 Then
                                finalAmendDt = modBooking.CopyToDataTable()
                                For Each row In modBooking
                                    row.Delete()
                                Next
                                finalDt.AcceptChanges()
                            End If

                            Dim modBooking2 = (From n In AmendDt.AsEnumerable Join m In tmpServDiff On n.Field(Of String)("bookingcode").Trim Equals m.bookingCode.Trim Select n)
                            If modBooking2.Count > 0 Then
                                For Each row In modBooking2
                                    row.Delete()
                                Next
                                AmendDt.AcceptChanges()
                            End If
                        End If

                        'List of amended booking
                        'sharfudee 19/09/2022
                        'Dim amendVal = (From t In AmendDt.AsEnumerable Select t).Except((From n In finalDt.AsEnumerable Join m In AmendDt.AsEnumerable On n.Field(Of String)("bookingcode") Equals m.Field(Of String)("bookingcode") And Convert.ToDateTime(n.Field(Of String)("startdate")) Equals m.Field(Of DateTime)("startdate") _
                        '              And Convert.ToDateTime(n.Field(Of String)("enddate")) Equals m.Field(Of DateTime)("enddate") And n.Field(Of String)("ClientID") Equals m.Field(Of String)("agentCode") _
                        '              And n.Field(Of String)("Agency") Equals m.Field(Of String)("agent") And n.Field(Of String)("Suppname") Equals m.Field(Of String)("SupplierName") _
                        '              And n.Field(Of String)("SuppID") Equals m.Field(Of String)("SupplierID") And Convert.ToDecimal(n.Field(Of String)("SalesPrice")) Equals m.Field(Of Decimal)("SalesPrice") _
                        '              And Convert.ToDecimal(n.Field(Of String)("Costprice")) Equals m.Field(Of Decimal)("Costprice") And n.Field(Of String)("Description") Equals m.Field(Of String)("servDescription") _
                        '              And n.Field(Of String)("PaxNumber") Equals m.Field(Of String)("PaxNumber") _
                        '              And n.Field(Of String)("Prodgrp") Equals m.Field(Of String)("ProductGroup") _
                        '              And n.Field(Of String)("BookingElementId") Equals Convert.ToString(m.Field(Of Long)("BookingElementId")) _
                        '              Select m )

                        'sharfudeen 17/10/2022
                        'Dim amendVal = (From t In AmendDt.AsEnumerable Select t).Except((From n In finalDt.AsEnumerable Join m In AmendDt.AsEnumerable On n.Field(Of String)("bookingcode") Equals m.Field(Of String)("bookingcode") And Convert.ToDateTime(n.Field(Of String)("startdate")) Equals m.Field(Of DateTime)("startdate") _
                        'And Convert.ToDateTime(n.Field(Of String)("enddate")) Equals m.Field(Of DateTime)("enddate") And n.Field(Of String)("ClientID") Equals m.Field(Of String)("agentCode") _
                        'And n.Field(Of String)("Agency") Equals m.Field(Of String)("agent") _
                        'And n.Field(Of String)("Suppname") Equals m.Field(Of String)("SupplierName") _
                        'And n.Field(Of String)("SuppID") Equals m.Field(Of String)("SupplierID") _
                        'And Convert.ToDecimal(n.Field(Of String)("SalesPrice")) Equals m.Field(Of Decimal)("SalesPrice") _
                        'And Convert.ToDecimal(n.Field(Of String)("Costprice")) Equals m.Field(Of Decimal)("Costprice") And n.Field(Of String)("Description") Equals m.Field(Of String)("servDescription") _
                        'And n.Field(Of String)("PaxNumber") Equals m.Field(Of String)("PaxNumber") _
                        'And n.Field(Of String)("Prodgrp") Equals m.Field(Of String)("ProductGroup") _
                        'And n.Field(Of String)("BookingElementId") Equals Convert.ToString(m.Field(Of Long)("BookingElementId")) _
                        'Select m))

                        Dim amendVal = (From t In AmendDt.AsEnumerable Where t.Field(Of Integer)("ispurchaseinvoice") = 0 Select t).Except((From n In finalDt.AsEnumerable Join m In AmendDt.AsEnumerable On n.Field(Of String)("bookingcode") Equals m.Field(Of String)("bookingcode") And Convert.ToDateTime(n.Field(Of String)("startdate")) Equals m.Field(Of DateTime)("startdate") _
                And Convert.ToDateTime(n.Field(Of String)("enddate")) Equals m.Field(Of DateTime)("enddate") And n.Field(Of String)("ClientID") Equals m.Field(Of String)("agentCode") _
                And n.Field(Of String)("Agency") Equals m.Field(Of String)("agent") _
                And n.Field(Of String)("Suppname") Equals m.Field(Of String)("SupplierName") _
                And n.Field(Of String)("SuppID") Equals m.Field(Of String)("SupplierID") _
                And Convert.ToDecimal(n.Field(Of String)("SalesPrice")) Equals m.Field(Of Decimal)("SalesPrice") _
                And Convert.ToDecimal(n.Field(Of String)("Costprice")) Equals m.Field(Of Decimal)("Costprice") And n.Field(Of String)("Description") Equals m.Field(Of String)("servDescription") _
                And n.Field(Of String)("PaxNumber") Equals m.Field(Of String)("PaxNumber") _
                And n.Field(Of String)("Prodgrp") Equals m.Field(Of String)("ProductGroup") _
                And n.Field(Of String)("BookingElementId") Equals Convert.ToString(m.Field(Of Long)("BookingElementId")) _
                Select m))

                        If amendVal.Count > 0 Then
                            Dim amendValDt As DataTable = amendVal.CopyToDataTable()
                            Dim grpAmendVal = (From n In amendValDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode}).ToList()

                            'sharfudeen 17/10/2022
                            Dim modBooking = (From n In finalDt.AsEnumerable Where n.Field(Of Integer)("ispurchaseinvoice") = 0 Join m In grpAmendVal On n.Field(Of String)("bookingcode") Equals m.bookingCode Select n)
                            '   Dim modBooking = (From n In finalDt.AsEnumerable Join m In grpAmendVal On n.Field(Of String)("bookingcode") Equals m.bookingCode Select n)
                            If modBooking.Count > 0 Then
                                Dim finalValDt As DataTable = modBooking.CopyToDataTable()
                                If finalAmendDt.Rows.Count > 0 Then
                                    finalAmendDt.Merge(finalValDt)
                                Else
                                    finalAmendDt = finalValDt
                                End If
                                For Each row In modBooking
                                    row.Delete()
                                Next
                                finalDt.AcceptChanges()
                            End If

                            Dim modBooking2 = (From n In AmendDt.AsEnumerable Where n.Field(Of Integer)("ispurchaseinvoice") = 0 Join m In grpAmendVal On n.Field(Of String)("bookingcode") Equals m.bookingCode Select n)
                            '  Dim modBooking2 = (From n In AmendDt.AsEnumerable  Join m In grpAmendVal On n.Field(Of String)("bookingcode") Equals m.bookingCode Select n)
                            If modBooking2.Count > 0 Then
                                For Each row In modBooking2
                                    row.Delete()
                                Next
                                AmendDt.AcceptChanges()
                            End If

                        End If

                        '        'Purchase invoice created
                        '        'Sharfudeen 17/10/2022
                        '        Dim PurchaseinvoiceVal = (From n In finalDt.AsEnumerable Group By bookingcode = n.Field(Of String)("bookingcode"), bookingElementId = n.Field(Of String)("bookingElementId"), Prodgrp = n.Field(Of String)("Prodgrp"), SuppID = n.Field(Of String)("SuppID") Into grp = Group Order By bookingcode, bookingElementId Select New With {.bookingcode = bookingcode, .bookingElementId = bookingElementId, .Prodgrp = Prodgrp, .SuppID = SuppID}).ToList()

                        '        Dim PurchaseinvoiceVal = (From t In finalDt.AsEnumerable Where t.Field(Of Integer)("ispurchaseinvoice") = 1 Select t) _
                        'And n.Field(Of String)("BookingElementId") Equals Convert.ToString(m.Field(Of Long)("BookingElementId")) _
                        'Select m))
                        '        If PurchaseinvoiceVal.Count > 0 Then
                        '            Dim PurchaseinvoiceValDt As DataTable = PurchaseinvoiceVal.CopyToDataTable()
                        '        End If

                        'Already imported bookings

                        ' Dim grpImported = (From n In AmendDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode, .totalServ = grp.Count}).ToList()
                        'sharfudeen 17/10/2022
                        ' Dim grpImported = (From n In AmendDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode, .totalServ = grp.Count, .ispurchaseinvoice = grp.Min(Function(x) Convert.ToInt32(x.Field(Of Integer)("ispurchaseinvoice")))}).ToList()
                        Dim grpImported = (From n In AmendDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode"), ispurchaseinvoice = n.Field(Of Integer)("ispurchaseinvoice") Into grp = Group Order By bookingCode, ispurchaseinvoice Select New With {.bookingCode = bookingCode, .totalServ = grp.Count, .ispurchaseinvoice = grp.Min(Function(x) Convert.ToInt32(x.Field(Of Integer)("ispurchaseinvoice")))}).ToList()


                        'sharfudeen 19/09/2022
                        'Purchase invocie already created
                        Dim PurchaseinvoiceBooking = (From n In finalDt.AsEnumerable Join m In grpImported On n.Field(Of String)("bookingcode").Trim Equals m.bookingCode.Trim
                                                      Select New With {.bookingCode = n.Field(Of String)("bookingcode"), .salecurrency = n.Field(Of String)("Salescurr"), .SalesPrice = n.Field(Of String)("SalesPrice"), .costPrice = n.Field(Of String)("costPrice"), .arrivalDate = n.Field(Of String)("Startdate"), .departureDate = n.Field(Of String)("Enddate"), .ispurchaseinvoice = m.ispurchaseinvoice}).ToList()

                        Dim PurchaseinvoiceBookingDt As DataTable = New DataTable()
                        PurchaseinvoiceBookingDt.Columns.Add("bookingcode", GetType(String))
                        PurchaseinvoiceBookingDt.Columns.Add("salecurrency", GetType(String))
                        PurchaseinvoiceBookingDt.Columns.Add("SalesPrice", GetType(Decimal))
                        PurchaseinvoiceBookingDt.Columns.Add("costPrice", GetType(Decimal))
                        PurchaseinvoiceBookingDt.Columns.Add("arrivalDate", GetType(String))
                        PurchaseinvoiceBookingDt.Columns.Add("departureDate", GetType(String))
                        PurchaseinvoiceBookingDt.Columns.Add("ispurchaseinvoice", GetType(Integer))

                        If PurchaseinvoiceBooking.Count > 0 Then
                            For Each item In PurchaseinvoiceBooking
                                'Dim grpDr As DataRow = PurchaseinvoiceBookingDt.NewRow
                                PurchaseinvoiceBookingDt.Rows.Add(item.bookingCode, item.salecurrency, item.SalesPrice, item.costPrice, item.arrivalDate, item.departureDate, item.ispurchaseinvoice)
                            Next
                        End If


                        'sharfudeen 24/10/2022
                        'Purchase invocie already created
                        Dim PurchaseMapingbooking = (From n In finalDt.AsEnumerable Join m In grpImported On n.Field(Of String)("bookingcode").Trim Equals m.bookingCode.Trim
                                                      Select New With {.bookingCode = n.Field(Of String)("bookingcode"), .salecurrency = n.Field(Of String)("Salescurr"), .SalesPrice = n.Field(Of String)("SalesPrice"), .costPrice = n.Field(Of String)("costPrice"), .arrivalDate = n.Field(Of String)("Startdate"), .departureDate = n.Field(Of String)("Enddate"), .ispurchaseinvoice = m.ispurchaseinvoice}).ToList()

                        Dim PurchaseinvoiceMapingBookingDt As DataTable = New DataTable()
                        PurchaseinvoiceMapingBookingDt.Columns.Add("bookingcode", GetType(String))
                        PurchaseinvoiceMapingBookingDt.Columns.Add("salecurrency", GetType(String))
                        PurchaseinvoiceMapingBookingDt.Columns.Add("SalesPrice", GetType(Decimal))
                        PurchaseinvoiceMapingBookingDt.Columns.Add("costPrice", GetType(Decimal))
                        PurchaseinvoiceMapingBookingDt.Columns.Add("arrivalDate", GetType(String))
                        PurchaseinvoiceMapingBookingDt.Columns.Add("departureDate", GetType(String))
                        PurchaseinvoiceMapingBookingDt.Columns.Add("ispurchaseinvoice", GetType(Integer))

                        If PurchaseMapingbooking.Count > 0 Then
                            For Each item In PurchaseMapingbooking
                                'Dim grpDr As DataRow = PurchaseinvoiceBookingDt.NewRow
                                PurchaseinvoiceMapingBookingDt.Rows.Add(item.bookingCode, item.salecurrency, item.SalesPrice, item.costPrice, item.arrivalDate, item.departureDate, item.ispurchaseinvoice)
                            Next
                        End If



                        If grpImported.Count > 0 Then
                            'sharfudeen 19/09/2022
                            ' Dim ImpBooking = (From n In finalDt.AsEnumerable Join m In grpImported On n.Field(Of String)("bookingcode") Equals m.bookingCode Select n)
                            Dim ImpBooking = (From n In finalDt.AsEnumerable Join m In grpImported On n.Field(Of String)("bookingcode").Trim Equals m.bookingCode.Trim Select n)
                            Dim ImpBookingDt As New DataTable
                            If ImpBooking.Count > 0 Then
                                ImpBookingDt = ImpBooking.CopyToDataTable()
                                For Each row In ImpBooking
                                    row.Delete()
                                Next
                                finalDt.AcceptChanges()
                            End If

                            gvImportedBooking.DataSource = ImpBookingDt
                            gvImportedBooking.DataBind()
                            Session("dtImportedBooking") = ImpBookingDt




                            Dim impReconcile = (From n In ImpBookingDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                                                Select New With {.bookingCode = bookingCode, .total = 1, _
                                                .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                                                .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

                            Dim reconcileUpdate = reconcileList.Where(Function(x) x.Keys = "imported")

                            For Each item In reconcileUpdate
                                item.NoOfBookings = impReconcile.Count
                                item.TotalSaleValue = impReconcile.Sum(Function(x) x.totalsalevalue)
                                item.TotalCostValue = impReconcile.Sum(Function(x) x.totalcostvalue)
                            Next
                            hdnNoOfBookings.Value = impReconcile.Count
                            hdnSalevalue.Value = impReconcile.Sum(Function(x) x.totalsalevalue)
                            hdnCostvalue.Value = impReconcile.Sum(Function(x) x.totalcostvalue)
                        Else
                            gvImportedBooking.DataSource = Nothing
                            gvImportedBooking.DataBind()
                        End If

                        Session("dtAmendedBooking") = finalAmendDt
                        gvAmendBooking.DataSource = finalAmendDt
                        gvAmendBooking.DataBind()
                        Dim ValidBooking = (From n In finalAmendDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode}).ToList()
                        hdnValidBooking.Value = Val(hdnValidBooking.Value) + ValidBooking.Count

                        Dim impReconcileSummaryAmendValidate = (From n In finalAmendDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                        Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("Salescurr")), _
                        .salevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                        .costvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice"))), _
                            .arrivalDate = grp.Min(Function(x) Convert.ToDateTime(x.Field(Of String)("startdate"))), .departureDate = grp.Max(Function(x) Convert.ToDateTime(x.Field(Of String)("enddate")))
                            }).ToList()

                        'changed by mohamed on 04/12/2021
                        'reconcileSummaryList.AddRange(impReconcileSummaryAmendValidate)
                        For Each itm In impReconcileSummaryAmendValidate
                            Dim itm1 As New ReconcileSummary
                            itm1.BookingCode = itm.bookingCode
                            itm1.salecurrency = itm.salecurrency
                            itm1.SaleValue = itm.salevalue
                            itm1.CostValue = itm.costvalue
                            lReconSummSaleValue += itm1.SaleValue
                            lReconSummCostValue += itm1.CostValue
                            itm1.arrivalDate = itm.arrivalDate
                            itm1.departureDate = itm.departureDate
                            itm1.BookingType = "Amendment Validated"
                            reconcileSummaryList.Add(itm1)
                        Next

                        'sharfudeen 19/09/2022
                        If PurchaseinvoiceBookingDt.Rows.Count > 0 Then
                            Dim PurchaseinvoiceBookingValidate = (From n In PurchaseinvoiceBookingDt.AsEnumerable Where n.Field(Of Integer)("ispurchaseinvoice") = 1 Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                                Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("salecurrency")), _
                                .salevalue = grp.Sum(Function(x) (x.Field(Of Decimal)("SalesPrice"))), _
                                .costvalue = grp.Sum(Function(x) (x.Field(Of Decimal)("costPrice"))), _
                                    .arrivalDate = grp.Min(Function(x) Convert.ToDateTime(x.Field(Of String)("arrivalDate"))), .departureDate = grp.Max(Function(x) Convert.ToDateTime(x.Field(Of String)("departureDate")))
                                    }).ToList()

                            Dim lReconPurSummSaleValue As Decimal = 0, lReconPurSummCostValue As Decimal = 0
                            For Each item In PurchaseinvoiceBookingValidate
                                Dim itm1 As New ReconcileSummary


                                itm1.BookingCode = item.bookingCode
                                itm1.salecurrency = item.salecurrency
                                itm1.SaleValue = item.salevalue
                                itm1.CostValue = item.costvalue
                                lReconPurSummSaleValue += item.salevalue
                                lReconPurSummSaleValue += item.costvalue
                                itm1.arrivalDate = item.arrivalDate
                                itm1.departureDate = item.departureDate
                                itm1.BookingType = "Purchase Invoice Created"
                                reconcileSummaryList.Add(itm1)
                            Next
                        End If


                        'sharfudeen 24/10/2022
                        If PurchaseinvoiceMapingBookingDt.Rows.Count > 0 Then
                            Dim PurchaseinvoiceMapingBookingValidate = (From n In PurchaseinvoiceMapingBookingDt.AsEnumerable Where n.Field(Of Integer)("ispurchaseinvoice") = 1 Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                                Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("salecurrency")), _
                                .salevalue = grp.Sum(Function(x) (x.Field(Of Decimal)("SalesPrice"))), _
                                .costvalue = grp.Sum(Function(x) (x.Field(Of Decimal)("costPrice"))), _
                                    .arrivalDate = grp.Min(Function(x) Convert.ToDateTime(x.Field(Of String)("arrivalDate"))), .departureDate = grp.Max(Function(x) Convert.ToDateTime(x.Field(Of String)("departureDate")))
                                    }).ToList()

                            Dim lReconMapSummSaleValue As Decimal = 0
                            Dim lReconMapSummCostValue As Decimal = 0
                            For Each item In PurchaseinvoiceMapingBookingValidate
                                Dim itm1 As New Mapingbookinglist
                                itm1.BookingCode = item.bookingCode
                                itm1.salecurrency = item.salecurrency
                                itm1.SaleValue = item.salevalue
                                itm1.CostValue = item.costvalue
                                lReconMapSummSaleValue += item.salevalue
                                lReconMapSummCostValue += item.costvalue
                                itm1.arrivalDate = item.arrivalDate
                                itm1.departureDate = item.departureDate
                                itm1.BookingType = "Purchase Invoice Created"
                                MapbookingSummaryList.Add(itm1)
                            Next
                        End If

                        gvMismatchpI.DataSource = MapbookingSummaryList
                        gvMismatchpI.DataBind()


                        Dim AmendValid = (From n In finalAmendDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
                                               Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
                                               .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                                               .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

                        Dim AmendIgnore = (From n In finalAmendDt.AsEnumerable Where n.Field(Of String)("checkIgnore") = "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
                                               Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
                                               .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                                               .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

                        Dim amendedvalidated = reconcileList.Where(Function(x) x.Keys = "amendedvalidated")
                        Dim amendedignored = reconcileList.Where(Function(x) x.Keys = "amendedignored")

                        If AmendValid.Count > 0 Then
                            For Each item In amendedvalidated
                                item.NoOfBookings = AmendValid.Count
                                item.TotalSaleValue = AmendValid.Sum(Function(x) x.totalsalevalue)
                                item.TotalCostValue = AmendValid.Sum(Function(x) x.totalcostvalue)
                            Next
                        End If
                        If AmendIgnore.Count > 0 Then
                            For Each item In amendedignored
                                item.NoOfBookings = AmendIgnore.Count
                                item.TotalSaleValue = AmendIgnore.Sum(Function(x) x.totalsalevalue)
                                item.TotalCostValue = AmendIgnore.Sum(Function(x) x.totalcostvalue)
                            Next
                        End If
                    Else
                        gvAmendBooking.DataSource = Nothing
                        gvAmendBooking.DataBind()

                        gvImportedBooking.DataSource = Nothing
                        gvImportedBooking.DataBind()
                    End If

                    gvBookingDetails.ShowFooter = False
                    If finalDt.Rows.Count > 0 Then
                        'MapAndFilterTable(dt)
                        Session("dtImportBooking") = finalDt
                        gvBookingDetails.DataSource = finalDt
                        gvBookingDetails.DataBind()

                        Dim ValidBooking = (From n In finalDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode}).ToList()
                        hdnValidBooking.Value = Val(hdnValidBooking.Value) + ValidBooking.Count

                        Dim impReconcileSummaryNewValid = (From n In finalDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                            Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("Salescurr")), _
                            .salevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                            .costvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice"))), _
                            .arrivalDate = grp.Min(Function(x) Convert.ToDateTime(x.Field(Of String)("startdate"))), .departureDate = grp.Max(Function(x) Convert.ToDateTime(x.Field(Of String)("enddate")))}).ToList()
                        'reconcileSummaryList.AddRange(impReconcileSummaryNewValid.ToList())
                        For Each itm In impReconcileSummaryNewValid
                            Dim itm1 As New ReconcileSummary
                            itm1.BookingCode = itm.bookingCode
                            itm1.salecurrency = itm.salecurrency
                            itm1.SaleValue = itm.salevalue
                            itm1.CostValue = itm.costvalue
                            lReconSummSaleValue += itm1.SaleValue
                            lReconSummCostValue += itm1.CostValue
                            itm1.arrivalDate = itm.arrivalDate
                            itm1.departureDate = itm.departureDate
                            itm1.BookingType = "New Validated"
                            reconcileSummaryList.Add(itm1)
                        Next


                        Dim NewValid = (From n In finalDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
                                               Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
                                               .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                                               .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

                        Dim NewIgnore = (From n In finalDt.AsEnumerable Where n.Field(Of String)("checkIgnore") = "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
                                               Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
                                               .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                                               .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

                        Dim newvalidated = reconcileList.Where(Function(x) x.Keys = "newvalidated")
                        Dim newignored = reconcileList.Where(Function(x) x.Keys = "newignored")

                        If NewValid.Count > 0 Then
                            For Each item In newvalidated
                                item.NoOfBookings = NewValid.Count
                                item.TotalSaleValue = NewValid.Sum(Function(x) x.totalsalevalue)
                                item.TotalCostValue = NewValid.Sum(Function(x) x.totalcostvalue)
                            Next
                        End If
                        If NewIgnore.Count > 0 Then
                            For Each item In newignored
                                item.NoOfBookings = NewIgnore.Count
                                item.TotalSaleValue = NewIgnore.Sum(Function(x) x.totalsalevalue)
                                item.TotalCostValue = NewIgnore.Sum(Function(x) x.totalcostvalue)
                            Next
                        End If

                        btnRefresh.Visible = True
                        lblNotemapping.Visible = True
                        lblNotemissing.Visible = True

                        'btnSave.Visible = True
                        'If Session("MapFlag") = 1 Then
                        '    btnSave.Visible = False
                        'End If
                        'If Session("ValidateFlag") = 1 Then
                        '    btnSave.Visible = False
                        'End If

                    Else
                        gvBookingDetails.DataSource = Nothing
                        gvBookingDetails.DataBind()
                        Session("dtImportBooking") = Nothing
                        btnSave.Visible = False
                        btnExcelNewBooking.Visible = False
                        btnReleaseInvoiceSealed.Visible = False
                        lblNotemapping.Visible = False
                        lblNotemissing.Visible = False
                        btnRefresh.Visible = False
                        Session("MapFlag") = 0
                        Session("ValidateFlag") = 0
                    End If



                    If chkIgnoreCancel.Checked = True Then 'changed by mohamed on 19/07/2021
                        GvCancelBooking.DataSource = Nothing
                        GvCancelBooking.DataBind()
                    Else
                        'cancel booking
                        Dim cancelDt As DataTable = fnCancelledBookings(bookingDt)
                        GvCancelBooking.DataSource = cancelDt
                        GvCancelBooking.DataBind()
                        Session("dtCancelBooking") = cancelDt

                        'changed by mohamed on 20/06/2021
                        Dim NewCancelled1 = (From n In cancelDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                                                   Select New With {.bookingCode = bookingCode, .total = 1, _
                                                   .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of Decimal)("SalesPrice"))), _
                                                   .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of Decimal)("costPrice")))}).ToList()

                        Dim NewCancelled2 = reconcileList.Where(Function(x) x.Keys = "cancelled")

                        If NewCancelled2.Count > 0 Then
                            For Each item In NewCancelled2
                                item.NoOfBookings = NewCancelled1.Count
                                item.TotalSaleValue = NewCancelled1.Sum(Function(x) x.totalsalevalue)
                                item.TotalCostValue = NewCancelled1.Sum(Function(x) x.totalcostvalue)
                            Next
                        End If
                    End If




                    'Dim impReconcileSummaryCancelled = (From n In cancelDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
                    'Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("Salescurr")), _
                    '.salevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
                    '.costvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()
                    ''reconcileSummaryList.AddRange(impReconcileSummaryCancelled)
                    'For Each itm In impReconcileSummaryCancelled
                    '    Dim itm1 As New ReconcileSummary
                    '    itm1.BookingCode = itm.bookingCode
                    '    itm1.salecurrency = itm.salecurrency
                    '    itm1.SaleValue = itm.salevalue
                    '    itm1.CostValue = itm.costvalue
                    '    lReconSummSaleValue += itm1.SaleValue
                    '    lReconSummCostValue += itm1.CostValue
                    '    itm1.BookingType = "Cancelled"
                    '    reconcileSummaryList.Add(itm1)
                    'Next

                    'Reconciliation
                    Dim total = reconcileList.Where(Function(x) x.Keys = "total")
                    For Each item In total
                        item.NoOfBookings = reconcileList.Where(Function(y) y.Keys <> "total" And y.Keys <> "cancelled").Sum(Function(x) x.NoOfBookings)
                        item.TotalSaleValue = reconcileList.Where(Function(y) y.Keys <> "total" And y.Keys <> "cancelled").Sum(Function(x) x.TotalSaleValue)
                        item.TotalCostValue = reconcileList.Where(Function(y) y.Keys <> "total" And y.Keys <> "cancelled").Sum(Function(x) x.TotalCostValue)
                    Next

                    gvReconcile.DataSource = reconcileList
                    gvReconcile.DataBind()
                    Session("ReconcileList") = reconcileList

                    Dim itmTot As New ReconcileSummary
                    itmTot.BookingType = "Total"
                    itmTot.SaleValue = lReconSummSaleValue
                    itmTot.CostValue = lReconSummCostValue
                    itmTot.BookingType = "Total"
                    reconcileSummaryList.Add(itmTot)

                    gvReconcileSummary.DataSource = reconcileSummaryList
                    gvReconcileSummary.DataBind()


                    If Val(hdnValidBooking.Value) > 0 Then
                        btnSave.Visible = True
                    Else
                        btnSave.Visible = False
                    End If
                    btnExcelNewBooking.Visible = True
                    btnReleaseInvoiceSealed.Visible = True

                    TabHotelInfo.ActiveTabIndex = "0"
                    ModalPopupLoading.Hide()
                Else
                    ModalPopupLoading.Hide()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Choose import excel file' );", True)
                End If
            Else
                ModalPopupLoading.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Choose import excel file' );", True)
            End If
        Catch ex As Exception
            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function createReconcileList() As List(Of Reconcile)"
    Protected Function createReconcileList() As List(Of Reconcile)
        Dim reconcileList As New List(Of Reconcile)
        reconcileList.Add(New Reconcile() With {.Keys = "newvalidated", .Classification = "New booking validated", .NoOfBookings = 0, .TotalSaleValue = 0, .TotalCostValue = 0})
        reconcileList.Add(New Reconcile() With {.Keys = "newignored", .Classification = "New booking ignored", .NoOfBookings = 0, .TotalSaleValue = 0, .TotalCostValue = 0})
        reconcileList.Add(New Reconcile() With {.Keys = "amendedvalidated", .Classification = "Amended booking validated", .NoOfBookings = 0, .TotalSaleValue = 0, .TotalCostValue = 0})
        reconcileList.Add(New Reconcile() With {.Keys = "amendedignored", .Classification = "Amended booking ignored", .NoOfBookings = 0, .TotalSaleValue = 0, .TotalCostValue = 0})
        reconcileList.Add(New Reconcile() With {.Keys = "imported", .Classification = "Already imported booking", .NoOfBookings = 0, .TotalSaleValue = 0, .TotalCostValue = 0})
        reconcileList.Add(New Reconcile() With {.Keys = "total", .Classification = "Total", .NoOfBookings = 0, .TotalSaleValue = 0, .TotalCostValue = 0})
        reconcileList.Add(New Reconcile() With {.Keys = "cancelled", .Classification = "Cancelled Booking", .NoOfBookings = 0, .TotalSaleValue = 0, .TotalCostValue = 0})
        Return reconcileList
    End Function
#End Region

#Region "Protected Function mappingBooking(ByVal dt As DataTable) As DataTable"
    Protected Function mappingBooking(ByVal dt As DataTable) As DataTable
        Try

            Dim grpAgent = (From n In dt.AsEnumerable Group By ClientID = n.Field(Of String)("ClientId"), Agency = n.Field(Of String)("Agency"), CurrCode = n.Field(Of String)("Salescurr") Into grp = Group Order By ClientID Select New With {.ClientId = ClientID, .Agency = Agency, .CurrCode = CurrCode}).ToList()



            Dim grpAgentDt As New DataTable()
            grpAgentDt.Columns.Add("ClientId")
            grpAgentDt.Columns.Add("Agency")
            grpAgentDt.Columns.Add("CurrCode")
            If grpAgent.Count > 0 Then
                For i = 0 To grpAgent.Count - 1
                    Dim grpDr As DataRow = grpAgentDt.NewRow
                    grpDr("ClientId") = grpAgent(i).ClientId.Trim
                    grpDr("Agency") = grpAgent(i).Agency.Trim
                    grpDr("CurrCode") = grpAgent(i).CurrCode.Trim
                    grpAgentDt.Rows.Add(grpDr)
                Next
                grpAgentDt.TableName = "AgentList"
                Dim agents As String = objUtils.GenerateXML(grpAgentDt)
                Dim agentDt As DataTable = AgentDetails(agents)

                If agentDt.Rows.Count > 0 Then
                    For Each agentRow As DataRow In agentDt.Rows
                        Dim tmpClientId As String = agentRow("ClientId")
                        Dim tmpcurrcode As String = agentRow("currcode") 'changed by mohamed on 30/11/2021
                        Dim filterAgentDt = (From n In dt.AsEnumerable Where n.Field(Of String)("ClientId") = tmpClientId AndAlso n.Field(Of String)("Salescurr") = tmpcurrcode Select n)
                        If filterAgentDt.Count > 0 Then
                            For i = 0 To filterAgentDt.Count - 1
                                filterAgentDt(i)("Mapagentcode") = agentRow("agentCode")
                                filterAgentDt(i)("Mapsalecurr") = agentRow("currcode")
                                filterAgentDt(i)("Mapctryagent") = agentRow("ctrycode")
                                filterAgentDt(i)("CtryAgency") = agentRow("ctryName")
                            Next
                        End If
                    Next
                    dt.AcceptChanges()
                End If
            End If

            Dim grpSupplier = (From s In dt.AsEnumerable Group By SupplierID = s.Field(Of String)("SuppID"), SupplierName = s.Field(Of String)("Suppname") Into grp = Group Order By SupplierID Select New With {.SupplierID = SupplierID, .SupplierName = SupplierName}).ToList()

            Dim grpSupplierDt As New DataTable()
            grpSupplierDt.Columns.Add("SupplierID")
            grpSupplierDt.Columns.Add("SupplierName")
            If grpSupplier.Count > 0 Then
                For i = 0 To grpSupplier.Count - 1
                    Dim grpDr As DataRow = grpSupplierDt.NewRow
                    grpDr("SupplierID") = grpSupplier(i).SupplierID.Trim
                    grpSupplierDt.Rows.Add(grpDr)
                Next
                grpSupplierDt.TableName = "SupplierList"
                Dim Suppliers As String = objUtils.GenerateXML(grpSupplierDt)
                Dim supplierDt As DataTable = SupplierDetails(Suppliers)

                If supplierDt.Rows.Count > 0 Then
                    For Each suppRow As DataRow In supplierDt.Rows
                        Dim tmpSupplierID As String = suppRow("supplierId")
                        Dim filterSuppDt = (From n In dt.AsEnumerable Where n.Field(Of String)("SuppID") = tmpSupplierID Select n)
                        If filterSuppDt.Count > 0 Then
                            For i = 0 To filterSuppDt.Count - 1
                                filterSuppDt(i)("Mappartycode") = suppRow("partycode")
                                filterSuppDt(i)("Costcurr") = suppRow("currcode")
                                'supplier is not hotel
                                If suppRow("sptypecode") <> "HOT" And (filterSuppDt(i)("Prodgrp").ToString().ToLower() = "accommodation" Or _
                                                                       filterSuppDt(i)("Prodgrp").ToString().ToLower() = "holiday package" Or filterSuppDt(i)("Prodgrp").ToString().ToLower() = "hotels") Then
                                    filterSuppDt(i)("checkHotelSupplier") = "1"
                                End If
                            Next
                        End If
                    Next
                    dt.AcceptChanges()
                End If
            End If

            Dim grpVisaTypes = (From s In dt.AsEnumerable Where s.Field(Of String)("Prodgrp").ToString().ToLower() = "visa" Group By Description = s.Field(Of String)("Description") Into grp = Group Order By Description _
                                Select New With {.Description = Description}).ToList()
            If grpVisaTypes.Count > 0 Then
                Dim grpVisaTypesDt As New DataTable()
                grpVisaTypesDt.Columns.Add("Description")
                For i = 0 To grpVisaTypes.Count - 1
                    Dim grpDr As DataRow = grpVisaTypesDt.NewRow
                    grpDr("Description") = grpVisaTypes(i).Description.Trim
                    grpVisaTypesDt.Rows.Add(grpDr)
                Next
                grpVisaTypesDt.TableName = "visaTypes"
                Dim visaTypes As String = objUtils.GenerateXML(grpVisaTypesDt)
                Dim visaDt As DataTable = VisaDetails(visaTypes)

                If visaDt.Rows.Count > 0 Then
                    For Each visaRow As DataRow In visaDt.Rows
                        Dim tmpDescription As String = visaRow("Description")
                        Dim filterVisaDt = (From n In dt.AsEnumerable Where n.Field(Of String)("Prodgrp").ToString().ToLower() = "visa" And n.Field(Of String)("Description") = tmpDescription Select n)
                        If filterVisaDt.Count > 0 Then
                            For i = 0 To filterVisaDt.Count - 1
                                filterVisaDt(i)("mapService") = visaRow("othtypcode")
                            Next
                        End If
                    Next
                    dt.AcceptChanges()
                End If
            End If

            'Find Ignore bookings
            Dim IgnoreList = (From n In dt.AsEnumerable Where n.Field(Of String)("Bookingcode") = "" Or n.Field(Of String)("Mainpssgrname") = "" Or _
                              n.Field(Of String)("Date") = "" Or n.Field(Of String)("Linebookingdate") = "" Or _
                              n.Field(Of String)("ClientId") = "" Or n.Field(Of String)("Agency") = "" Or _
                              n.Field(Of String)("Startdate") = "" Or n.Field(Of String)("Enddate") = "" Or _
                              n.Field(Of String)("SuppID") = "" Or n.Field(Of String)("Suppname") = "" Or _
                              n.Field(Of String)("Description") = "" Or n.Field(Of String)("SalesPrice") = "" Or _
                              n.Field(Of String)("Salescurr") = "" Or n.Field(Of String)("Prodgrp") = "" Or _
                              n.Field(Of String)("Costprice") = "" Or n.Field(Of String)("Costcurr") = "" Or _
                              n.Field(Of String)("CtryAgency") = "" Or _
                              n.Field(Of String)("Mapagentcode") = "" Or n.Field(Of String)("Mappartycode") = "" Or _
                              n.Field(Of String)("Mapsalecurr") <> n.Field(Of String)("Salescurr") Or n.Field(Of String)("Mapctryagent") = "" Or _
                              (n.Field(Of String)("Prodgrp").ToString().ToLower() = "visa" And n.Field(Of String)("mapService") = "") Or _
                              (n.Field(Of String)("Prodgrp").ToString().ToLower() = "visa" And n.Field(Of String)("PaxNumber") = "") Or _
                              n.Field(Of String)("checkHotelSupplier") = "1" Or _
                              n.Field(Of String)("bookingElementId") = "" Select n)

            If IgnoreList.Count > 0 Then
                Dim IgnoreDt As DataTable = IgnoreList.CopyToDataTable()
                Dim IgnoreBookings = (From n In IgnoreDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode}).ToList()

                Dim updateIgnore = (From n In dt.AsEnumerable Join m In IgnoreBookings On n.Field(Of String)("bookingcode") Equals m.bookingCode Select n)

                For Each row As DataRow In updateIgnore
                    row("checkIgnore") = "1"
                Next
                dt.AcceptChanges()
            End If

            'Sharfudeen 17/10/2022
            Dim ValidatePurchaseinvoice = (From n In dt.AsEnumerable Group By bookingcode = n.Field(Of String)("bookingcode"), bookingElementId = n.Field(Of String)("bookingElementId"), Prodgrp = n.Field(Of String)("Prodgrp"), SuppID = n.Field(Of String)("SuppID") Into grp = Group Order By bookingcode, bookingElementId Select New With {.bookingcode = bookingcode, .bookingElementId = bookingElementId, .Prodgrp = Prodgrp, .SuppID = SuppID}).ToList()


            Dim ValudatepurchseinvoiceDt As New DataTable()
            ValudatepurchseinvoiceDt.Columns.Add("bookingcode")
            ValudatepurchseinvoiceDt.Columns.Add("bookingElementId")
            ValudatepurchseinvoiceDt.Columns.Add("Prodgrp")
            ValudatepurchseinvoiceDt.Columns.Add("SuppID")
            If ValidatePurchaseinvoice.Count > 0 Then
                For i = 0 To ValidatePurchaseinvoice.Count - 1
                    Dim grpDr As DataRow = ValudatepurchseinvoiceDt.NewRow
                    grpDr("bookingcode") = ValidatePurchaseinvoice(i).bookingcode.Trim
                    grpDr("bookingElementId") = ValidatePurchaseinvoice(i).bookingElementId.Trim
                    grpDr("Prodgrp") = ValidatePurchaseinvoice(i).Prodgrp.Trim
                    grpDr("SuppID") = ValidatePurchaseinvoice(i).SuppID.Trim
                    ValudatepurchseinvoiceDt.Rows.Add(grpDr)
                Next
                ValudatepurchseinvoiceDt.TableName = "PurchaseinvoiceList"
                Dim Purchaseinvoice2 As String = objUtils.GenerateXML(ValudatepurchseinvoiceDt)
                Dim Purchaseinvoice2Dt As DataTable = PurchaseDetails(Purchaseinvoice2)

                If Purchaseinvoice2Dt.Rows.Count > 0 Then
                    For Each PurchaseinvoiceRow As DataRow In Purchaseinvoice2Dt.Rows
                        Dim tmpbookingcode As String = PurchaseinvoiceRow("bookingcode")
                        Dim tmpbookingElementId As String = PurchaseinvoiceRow("bookingElementId")
                        Dim Dt2 = (From n In dt.AsEnumerable Where n.Field(Of String)("bookingcode") = tmpbookingcode AndAlso n.Field(Of String)("bookingElementId") = tmpbookingElementId Select n)

                        If Dt2.Count > 0 Then
                            For i = 0 To Dt2.Count - 1
                                Dt2(i)("ispurchaseinvoice") = PurchaseinvoiceRow("ispurchaseinvoice")
                            Next
                        End If
                    Next
                    dt.AcceptChanges()
                End If
            End If


            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region


#Region "Protected Function PurchaseDetails(ByVal strpurchasedetails As String) As DataTable"
    Protected Function PurchaseDetails(ByVal strpurchasedetails As String) As DataTable
        Try
            Dim dt As New DataTable
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("sp_get_PurchaseinvoiceDetails", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
            mySqlCmd.Parameters.Add(New SqlParameter("@strPurchaseinvoice", SqlDbType.VarChar, -1)).Value = strpurchasedetails
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
            myAdapter.SelectCommand = mySqlCmd
            myAdapter.Fill(dt)
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Function AgentDetails(ByVal strAgents As String) As DataTable"
    Protected Function AgentDetails(ByVal strAgents As String) As DataTable
        Try
            Dim dt As New DataTable
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("sp_get_agentDetails", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
            mySqlCmd.Parameters.Add(New SqlParameter("@strAgents", SqlDbType.VarChar, -1)).Value = strAgents
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
            myAdapter.SelectCommand = mySqlCmd
            myAdapter.Fill(dt)
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Function SupplierDetails(ByVal strSuppliers As String) As DataTable"
    Protected Function SupplierDetails(ByVal strSuppliers As String) As DataTable
        Try
            Dim dt As New DataTable
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("sp_get_SupplierDetails", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
            mySqlCmd.Parameters.Add(New SqlParameter("@strSuppliers", SqlDbType.VarChar, -1)).Value = strSuppliers
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
            myAdapter.SelectCommand = mySqlCmd
            myAdapter.Fill(dt)
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Function VisaDetails(ByVal strVisatypes As String) As DataTable"
    Protected Function VisaDetails(ByVal strVisatypes As String) As DataTable
        Try
            Dim dt As New DataTable
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("sp_get_visaDetails", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
            mySqlCmd.Parameters.Add(New SqlParameter("@strVisaTypes", SqlDbType.VarChar, -1)).Value = strVisatypes
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
            myAdapter.SelectCommand = mySqlCmd
            myAdapter.Fill(dt)
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Function fnAmendedBookings(ByVal bookingDt As DataTable) As DataTable"
    Protected Function fnAmendedBookings(ByVal bookingDt As DataTable) As DataTable
        Try
            bookingDt.TableName = "bookings"
            Dim bookings As String = objUtils.GenerateXML(bookingDt)
            Dim dt As New DataTable
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("sp_get_amendBookings", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@arrivalFromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@arrivalToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@bookings", SqlDbType.VarChar, -1)).Value = bookings
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
            myAdapter.SelectCommand = mySqlCmd
            myAdapter.Fill(dt)
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Function fnAmendedBookingsNew(ByVal bookingDt As DataTable,ByVal Purchaseinvoicedt as DataTable) As DataTable"
    Protected Function fnAmendedBookingsNew(ByVal bookingDt As DataTable, ByVal Purchaseinvoicedt As DataTable) As DataTable
        Try
            bookingDt.TableName = "bookings"
            Dim bookings As String = objUtils.GenerateXML(bookingDt)
            Dim strPurchaseinvoice As String = ""
            Dim ValidatePurchaseinvoice = (From n In Purchaseinvoicedt.AsEnumerable Group By bookingcode = n.Field(Of String)("bookingcode"), bookingElementId = n.Field(Of String)("bookingElementId"), ispurchaseinvoice = n.Field(Of Integer)("ispurchaseinvoice"), SuppID = n.Field(Of String)("SuppID"), Prodgrp = n.Field(Of String)("Prodgrp") Into grp = Group Order By bookingcode, bookingElementId Select New With {.bookingcode = bookingcode, .bookingElementId = bookingElementId, .ispurchaseinvoice = ispurchaseinvoice, .Prodgrp = Prodgrp, .SuppID = SuppID}).ToList()

            Dim ValudatepurchseinvoiceDt As New DataTable()
            ValudatepurchseinvoiceDt.Columns.Add("bookingcode")
            ValudatepurchseinvoiceDt.Columns.Add("bookingElementId")
            ValudatepurchseinvoiceDt.Columns.Add("ispurchaseinvoice")
            ValudatepurchseinvoiceDt.Columns.Add("Prodgrp")
            ValudatepurchseinvoiceDt.Columns.Add("SuppID")

            If ValidatePurchaseinvoice.Count > 0 Then
                For i = 0 To ValidatePurchaseinvoice.Count - 1
                    Dim grpDr As DataRow = ValudatepurchseinvoiceDt.NewRow
                    grpDr("bookingcode") = ValidatePurchaseinvoice(i).bookingcode.Trim
                    grpDr("bookingElementId") = ValidatePurchaseinvoice(i).bookingElementId.Trim
                    grpDr("ispurchaseinvoice") = ValidatePurchaseinvoice(i).ispurchaseinvoice
                    grpDr("Prodgrp") = ValidatePurchaseinvoice(i).Prodgrp
                    grpDr("SuppID") = ValidatePurchaseinvoice(i).SuppID
                    ValudatepurchseinvoiceDt.Rows.Add(grpDr)
                Next
                ValudatepurchseinvoiceDt.TableName = "PurchaseinvoiceList"
                strPurchaseinvoice = objUtils.GenerateXML(ValudatepurchseinvoiceDt)
            End If



            Dim dt As New DataTable
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("sp_get_amendBookingsNew", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
            mySqlCmd.Parameters.Add(New SqlParameter("@arrivalFromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@arrivalToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@bookings", SqlDbType.VarChar, -1)).Value = bookings
            mySqlCmd.Parameters.Add(New SqlParameter("@strPurchaseinvoice", SqlDbType.VarChar, -1)).Value = strPurchaseinvoice
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
            myAdapter.SelectCommand = mySqlCmd
            myAdapter.Fill(dt)
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close


            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region


#Region "Protected Function fnCancelledBookings(ByVal bookingDt As DataTable) As DataTable"
    Protected Function fnCancelledBookings(ByVal bookingDt As DataTable) As DataTable
        Try
            bookingDt.TableName = "bookings"
            Dim bookings As String = objUtils.GenerateXML(bookingDt)
            Dim dt As New DataTable
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("sp_get_cancelBookings", mySqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
            mySqlCmd.Parameters.Add(New SqlParameter("@arrivalFromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@arrivalToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@bookings", SqlDbType.VarChar, -1)).Value = bookings
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter()
            myAdapter.SelectCommand = mySqlCmd
            myAdapter.Fill(dt)
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Return dt
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

    '#Region "Protected Sub btnRefresh_Click(sender As Object, e As System.EventArgs) Handles btnRefresh.Click"
    '    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
    '        Try
    '            hdnValidBooking.Value = 0
    '            Dim dt As DataTable = CType(Session("dtImportBooking"), DataTable)
    '            Dim reconcileList As List(Of Reconcile) = createReconcileList()

    '            Dim reconcileSummaryList As New List(Of ReconcileSummary)
    '            gvReconcileSummary.DataSource = reconcileSummaryList
    '            gvReconcileSummary.DataBind()

    '            If dt.Rows.Count > 0 Then
    '                Dim finalDt As DataTable = mappingBooking(dt)

    '                gvBookingDetails.ShowFooter = False
    '                If finalDt.Rows.Count > 0 Then
    '                    Session("dtImportBooking") = finalDt
    '                    gvBookingDetails.DataSource = finalDt
    '                    gvBookingDetails.DataBind()

    '                    Dim ValidBooking = (From n In finalDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode}).ToList()
    '                    hdnValidBooking.Value = ValidBooking.Count

    '                    Dim impReconcileSummaryNewValid = (From n In finalDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
    '                            Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("Salescurr")), _
    '                            .salevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
    '                            .costvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()
    '                    'reconcileSummaryList.AddRange(impReconcileSummaryNewValid.ToList())
    '                    For Each itm In impReconcileSummaryNewValid
    '                        Dim itm1 As New ReconcileSummary
    '                        itm1.BookingCode = itm.bookingCode
    '                        itm1.salecurrency = itm.salecurrency
    '                        itm1.SaleValue = itm.salevalue
    '                        itm1.CostValue = itm.costvalue
    '                        itm1.BookingType = "New Validated"
    '                        reconcileSummaryList.Add(itm1)
    '                    Next

    '                    Dim NewValid = (From n In finalDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
    '                                               Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
    '                                               .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
    '                                               .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

    '                    Dim NewIgnore = (From n In finalDt.AsEnumerable Where n.Field(Of String)("checkIgnore") = "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
    '                                           Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
    '                                           .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
    '                                           .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

    '                    Dim newvalidated = reconcileList.Where(Function(x) x.Keys = "newvalidated")
    '                    Dim newignored = reconcileList.Where(Function(x) x.Keys = "newignored")

    '                    If NewValid.Count > 0 Then
    '                        For Each item In newvalidated
    '                            item.NoOfBookings = NewValid.Count
    '                            item.TotalSaleValue = NewValid.Sum(Function(x) x.totalsalevalue)
    '                            item.TotalCostValue = NewValid.Sum(Function(x) x.totalcostvalue)
    '                        Next
    '                    End If
    '                    If NewIgnore.Count > 0 Then
    '                        For Each item In newignored
    '                            item.NoOfBookings = NewIgnore.Count
    '                            item.TotalSaleValue = NewIgnore.Sum(Function(x) x.totalsalevalue)
    '                            item.TotalCostValue = NewIgnore.Sum(Function(x) x.totalcostvalue)
    '                        Next
    '                    End If

    '                    'btnSave.Visible = True
    '                    'If Session("ValidateFlag") = 1 Then
    '                    '    btnSave.Visible = False
    '                    'End If
    '                    lblNotemapping.Visible = True
    '                    lblNotemissing.Visible = True
    '                    btnRefresh.Visible = True
    '                Else
    '                    gvBookingDetails.DataSource = Nothing
    '                    gvBookingDetails.DataBind()

    '                    btnSave.Visible = False
    '                    lblNotemapping.Visible = False
    '                    lblNotemissing.Visible = False
    '                    btnRefresh.Visible = False
    '                End If

    '                Dim AmendDt As DataTable = CType(Session("dtAmendedBooking"), DataTable)
    '                If AmendDt.Rows.Count > 0 Then
    '                    Dim finalAmendDt As DataTable = mappingBooking(AmendDt)
    '                    gvAmendBooking.DataSource = finalAmendDt
    '                    gvAmendBooking.DataBind()

    '                    Dim ValidBooking = (From n In finalAmendDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode Select New With {.bookingCode = bookingCode}).ToList()
    '                    hdnValidBooking.Value = Val(hdnValidBooking.Value) + ValidBooking.Count

    '                    Dim impReconcileSummaryAmendValidate = (From n In finalAmendDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
    '                        Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("Salescurr")), _
    '                        .salevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
    '                        .costvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()
    '                    'reconcileSummaryList.AddRange(impReconcileSummaryAmendValidate)
    '                    For Each itm In impReconcileSummaryAmendValidate
    '                        Dim itm1 As New ReconcileSummary
    '                        itm1.BookingCode = itm.bookingCode
    '                        itm1.salecurrency = itm.salecurrency
    '                        itm1.SaleValue = itm.salevalue
    '                        itm1.CostValue = itm.costvalue
    '                        itm1.BookingType = "Amendment Validated"
    '                        reconcileSummaryList.Add(itm1)
    '                    Next

    '                    Dim AmendValid = (From n In finalAmendDt.AsEnumerable Where n.Field(Of String)("checkIgnore") <> "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
    '                                              Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
    '                                              .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
    '                                              .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

    '                    Dim AmendIgnore = (From n In finalAmendDt.AsEnumerable Where n.Field(Of String)("checkIgnore") = "1" Group By bookingCode = n.Field(Of String)("bookingCode"), checkIgnore = n.Field(Of String)("checkIgnore") Into grp = Group Order By bookingCode _
    '                                           Select New With {.bookingCode = bookingCode, .checkIgnore = checkIgnore, .total = 1, _
    '                                           .totalsalevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
    '                                           .totalcostvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()

    '                    Dim amendedvalidated = reconcileList.Where(Function(x) x.Keys = "amendedvalidated")
    '                    Dim amendedignored = reconcileList.Where(Function(x) x.Keys = "amendedignored")

    '                    If AmendValid.Count > 0 Then
    '                        For Each item In amendedvalidated
    '                            item.NoOfBookings = AmendValid.Count
    '                            item.TotalSaleValue = AmendValid.Sum(Function(x) x.totalsalevalue)
    '                            item.TotalCostValue = AmendValid.Sum(Function(x) x.totalcostvalue)
    '                        Next
    '                    End If
    '                    If AmendIgnore.Count > 0 Then
    '                        For Each item In amendedignored
    '                            item.NoOfBookings = AmendIgnore.Count
    '                            item.TotalSaleValue = AmendIgnore.Sum(Function(x) x.totalsalevalue)
    '                            item.TotalCostValue = AmendIgnore.Sum(Function(x) x.totalcostvalue)
    '                        Next
    '                    End If

    '                Else
    '                    gvAmendBooking.DataSource = Nothing
    '                    gvAmendBooking.DataBind()
    '                End If

    '                If Session("dtCancelBooking") IsNot Nothing Then
    '                    Dim cancelDt As DataTable = CType(Session("dtCancelBooking"), DataTable)
    '                    If cancelDt.Rows.Count > 0 Then
    '                        Dim impReconcileSummaryCancelled = (From n In cancelDt.AsEnumerable Group By bookingCode = n.Field(Of String)("bookingCode") Into grp = Group Order By bookingCode _
    '                        Select New With {.bookingCode = bookingCode, .salecurrency = grp.Max(Function(x) x.Field(Of String)("Salescurr")), _
    '                        .salevalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("SalesPrice"))), _
    '                        .costvalue = grp.Sum(Function(x) Convert.ToDecimal(x.Field(Of String)("costPrice")))}).ToList()
    '                        'reconcileSummaryList.AddRange(impReconcileSummaryCancelled)
    '                        For Each itm In impReconcileSummaryCancelled
    '                            Dim itm1 As New ReconcileSummary
    '                            itm1.BookingCode = itm.bookingCode
    '                            itm1.salecurrency = itm.salecurrency
    '                            itm1.SaleValue = itm.salevalue
    '                            itm1.CostValue = itm.costvalue
    '                            itm1.BookingType = "Cancelled"
    '                            reconcileSummaryList.Add(itm1)
    '                        Next
    '                    End If
    '                End If


    '                Dim reconcileUpdate = reconcileList.Where(Function(x) x.Keys = "imported")

    '                For Each item In reconcileUpdate
    '                    item.NoOfBookings = Val(hdnNoOfBookings.Value)
    '                    item.TotalSaleValue = Val(hdnSalevalue.Value)
    '                    item.TotalCostValue = Val(hdnCostvalue.Value)
    '                Next

    '                'Reconciliation
    '                Dim total = reconcileList.Where(Function(x) x.Keys = "total")
    '                For Each item In total
    '                    item.NoOfBookings = reconcileList.Where(Function(y) y.Keys <> "total").Sum(Function(x) x.NoOfBookings)
    '                    item.TotalSaleValue = reconcileList.Where(Function(y) y.Keys <> "total").Sum(Function(x) x.TotalSaleValue)
    '                    item.TotalCostValue = reconcileList.Where(Function(y) y.Keys <> "total").Sum(Function(x) x.TotalCostValue)
    '                Next

    '                gvReconcile.DataSource = reconcileList
    '                gvReconcile.DataBind()

    '                gvReconcileSummary.DataSource = reconcileSummaryList
    '                gvReconcileSummary.DataBind()


    '                If Val(hdnValidBooking.Value) > 0 Then
    '                    btnSave.Visible = True
    '                Else
    '                    btnSave.Visible = False
    '                End If

    '                TabHotelInfo.ActiveTabIndex = "0"
    '                ModalPopupLoading.Hide()
    '            Else
    '                ModalPopupLoading.Hide()
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Choose excel file and import booking' );", True)
    '            End If
    '        Catch ex As Exception
    '            ModalPopupLoading.Hide()
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '            objUtils.WritErrorLog("ImportBooking.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        End Try
    '    End Sub
    '#End Region

#Region "Protected Sub gvBookingDetails_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBookingDetails.RowDataBound"
    Protected Sub gvBookingDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBookingDetails.RowDataBound

        'If e.Row.RowType = DataControlRowType.DataRow Then

        '    Dim txtMapAgent As TextBox
        '    Dim txtMappartycode As TextBox
        '    Dim txtMapsalecurr As TextBox
        '    Dim txtMapAgentCtry As TextBox
        '    Dim txtMapService As TextBox
        '    Dim txtPaxNumber As TextBox
        '    Dim txtCheckHotelSupplier As TextBox

        '    Dim lblSalescurr As Label
        '    Dim lblBookingcode As Label
        '    Dim lblMainpssg As Label
        '    Dim lblDt As Label
        '    Dim lblLinebookingdt As Label
        '    Dim lblStartdt As Label
        '    Dim lblEnddt As Label

        '    Dim lbldesc As Label
        '    Dim lblsalesprice As Label
        '    Dim lblProdgrp As Label
        '    Dim lblClientID As Label
        '    Dim lblCtryAgency As Label

        '    Dim lblSuppcode As Label
        '    Dim lblsuppname As Label
        '    Dim lblCostprice As Label
        '    Dim lblCostcurr As Label
        '    Dim txtIgnore As TextBox
        '    Dim chkSelect As CheckBox

        '    Dim lblBookElementId As Label

        '    chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
        '    txtIgnore = CType(e.Row.FindControl("txtIgnore"), TextBox)
        '    txtMapAgent = CType(e.Row.FindControl("txtMapAgent"), TextBox)
        '    txtMappartycode = CType(e.Row.FindControl("txtMapparty"), TextBox)
        '    txtMapsalecurr = CType(e.Row.FindControl("txtMapsalecurr"), TextBox)
        '    txtMapAgentCtry = CType(e.Row.FindControl("txtMapAgentCtry"), TextBox)
        '    txtMapService = CType(e.Row.FindControl("txtMapService"), TextBox)
        '    txtPaxNumber = CType(e.Row.FindControl("txtPaxNumber"), TextBox)
        '    txtCheckHotelSupplier = CType(e.Row.FindControl("txtCheckHotelSupplier"), TextBox)

        '    lblBookingcode = CType(e.Row.FindControl("lblBookingNo"), Label)
        '    lblMainpssg = CType(e.Row.FindControl("lblMainpssgrname"), Label)
        '    lblDt = CType(e.Row.FindControl("lblDate"), Label)
        '    lblLinebookingdt = CType(e.Row.FindControl("lbllinebookdate"), Label)
        '    lblStartdt = CType(e.Row.FindControl("lblstartdate"), Label)
        '    lblEnddt = CType(e.Row.FindControl("lblenddate"), Label)


        '    'lblexcgrate = CType(e.Row.FindControl("lblexchangerate"), Label)
        '    lbldesc = CType(e.Row.FindControl("lbldescription"), Label)
        '    lblProdgrp = CType(e.Row.FindControl("lblprodgroup"), Label)
        '    lblsalesprice = CType(e.Row.FindControl("lblsalesprice"), Label)
        '    lblSalescurr = CType(e.Row.FindControl("lblsalescurr"), Label)
        '    lblClientID = CType(e.Row.FindControl("lblClientId"), Label)
        '    lblCtryAgency = CType(e.Row.FindControl("lblAgencyCtry"), Label)

        '    lblSuppcode = CType(e.Row.FindControl("lblsuppid"), Label)
        '    lblsuppname = CType(e.Row.FindControl("lblsuppname"), Label)
        '    lblCostprice = CType(e.Row.FindControl("lblCostprice"), Label)
        '    lblCostcurr = CType(e.Row.FindControl("lblcostcurr"), Label)
        '    lblBookElementId = CType(e.Row.FindControl("lblBookElementId"), Label)

        '    'MApping cells

        '    If txtMapAgent.Text.Trim = "" Then
        '        e.Row.Cells(4).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If
        '    If txtMappartycode.Text.Trim = "" Then
        '        e.Row.Cells(11).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If

        '    If txtMapsalecurr.Text.Trim <> lblSalescurr.Text.Trim Then
        '        e.Row.Cells(9).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If
        '    If txtMapAgentCtry.Text.Trim = "" Then
        '        e.Row.Cells(18).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If

        '    If lblProdgrp.Text.ToLower = "visa" And txtMapService.Text = "" Then
        '        e.Row.Cells(7).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        lbldesc.ToolTip = "Visa type is not matching"
        '        Session("MapFlag") = 1
        '    End If

        '    If lblProdgrp.Text.ToLower = "visa" And txtPaxNumber.Text = "" Then
        '        e.Row.Cells(7).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        lbldesc.ToolTip = "Pax detail is missing for Visa"
        '        Session("MapFlag") = 1
        '    End If

        '    If txtCheckHotelSupplier.Text = "1" Then
        '        e.Row.Cells(11).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        lblsuppname.ToolTip = "Hotel and Supplier are different in the booking"
        '        Session("MapFlag") = 1
        '    End If

        '    'validate flag

        '    If lblBookingcode.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(1).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblMainpssg.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(6).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblDt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(14).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblLinebookingdt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(15).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblStartdt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(2).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblEnddt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(3).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblSuppcode.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(16).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    'If lblexcgrate.Text = "" Then
        '    '    Session("ValidateFlag") = 1
        '    '    e.Row.Cells(11).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    'End If
        '    If lbldesc.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(7).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If

        '    If lblsalesprice.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(10).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblSalescurr.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(9).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblProdgrp.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(8).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblCostprice.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(13).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblCostcurr.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(12).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblClientID.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(17).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblCtryAgency.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(18).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblBookElementId.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(23).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If

        '    If txtIgnore.Text = "1" Then
        '        chkSelect.Checked = False
        '        e.Row.BackColor = System.Drawing.Color.FromArgb(211, 211, 211)
        '    Else
        '        chkSelect.Checked = True
        '    End If

        'End If
    End Sub
#End Region

    Protected Sub ShowGVmapdetailPopup(ByVal gvr As GridViewRow, ByVal Purchaseinvoicedt As DataTable)
        lblMsgMapPi1.Visible = False

        Dim strPurchaseinvoice As String = ""
        '      Dim ValidatePurchaseinvoice = (From n In Purchaseinvoicedt.AsEnumerable Group By bookingcode = n.Field(Of String)("bookingcode"), bookingElementId = n.Field(Of String)("bookingElementId"), ispurchaseinvoice = n.Field(Of Integer)("ispurchaseinvoice"), SuppID = n.Field(Of String)("SuppID"), Prodgrp = n.Field(Of String)("Prodgrp") Into grp = Group Order By bookingcode, bookingElementId Select New With {.bookingcode = bookingcode, .bookingElementId = bookingElementId, .ispurchaseinvoice = ispurchaseinvoice, .Prodgrp = Prodgrp, .SuppID = SuppID}).ToList()
        Dim ValidatePurchaseinvoice = (From n In Purchaseinvoicedt.AsEnumerable Group By bookingcode = n.Field(Of String)("bookingcode"), bookingElementId = n.Field(Of String)("bookingElementId"), ispurchaseinvoice = n.Field(Of Integer)("ispurchaseinvoice"), SuppID = n.Field(Of String)("SuppID"), Prodgrp = n.Field(Of String)("Prodgrp"), SalesPrice = n.Field(Of String)("SalesPrice"), Costprice = n.Field(Of String)("Costprice") Into grp = Group Order By bookingcode, bookingElementId Select New With {.bookingcode = bookingcode, .bookingElementId = bookingElementId, .ispurchaseinvoice = ispurchaseinvoice, .Prodgrp = Prodgrp, .SuppID = SuppID, .SalesPrice = SalesPrice, .Costprice = Costprice}).ToList()

        Dim ValudatepurchseinvoiceDt As New DataTable()
        ValudatepurchseinvoiceDt.Columns.Add("bookingcode")
        ValudatepurchseinvoiceDt.Columns.Add("bookingElementId")
        ValudatepurchseinvoiceDt.Columns.Add("ispurchaseinvoice")
        ValudatepurchseinvoiceDt.Columns.Add("Prodgrp")
        ValudatepurchseinvoiceDt.Columns.Add("SuppID")
        ValudatepurchseinvoiceDt.Columns.Add("SalesPrice")
        ValudatepurchseinvoiceDt.Columns.Add("Costprice")

        If ValidatePurchaseinvoice.Count > 0 Then
            For i = 0 To ValidatePurchaseinvoice.Count - 1
                Dim grpDr As DataRow = ValudatepurchseinvoiceDt.NewRow
                grpDr("bookingcode") = ValidatePurchaseinvoice(i).bookingcode.Trim
                grpDr("bookingElementId") = ValidatePurchaseinvoice(i).bookingElementId.Trim
                grpDr("ispurchaseinvoice") = ValidatePurchaseinvoice(i).ispurchaseinvoice
                grpDr("Prodgrp") = ValidatePurchaseinvoice(i).Prodgrp
                grpDr("SuppID") = ValidatePurchaseinvoice(i).SuppID
                grpDr("SalesPrice") = ValidatePurchaseinvoice(i).SalesPrice
                grpDr("Costprice") = ValidatePurchaseinvoice(i).Costprice
                ValudatepurchseinvoiceDt.Rows.Add(grpDr)
            Next
            ValudatepurchseinvoiceDt.TableName = "PurchaseinvoiceList"
            strPurchaseinvoice = objUtils.GenerateXML(ValudatepurchseinvoiceDt)
        End If


        sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
        mySqlCmd = New SqlCommand("sp_mappingPI_booking", sqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
        mySqlCmd.Parameters.Add(New SqlParameter("@Bookingcode", SqlDbType.VarChar, 20)).Value = txtBookingNo.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@strPurchaseinvoice", SqlDbType.VarChar, -1)).Value = strPurchaseinvoice
        Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
            Using ds As New DataSet()
                myDataAdapter.Fill(ds)
                gvMapdetail1.DataSource = ds.Tables(0)
                gvMapdetail1.DataBind()

                gvMapdetail2.DataSource = ds.Tables(1)
                gvMapdetail2.DataBind()
            End Using
        End Using
        clsDBConnect.dbCommandClose(mySqlCmd)
        clsDBConnect.dbConnectionClose(sqlConn)

    End Sub
#Region "Protected Sub gvMismatchpI_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMismatchpI.RowCommand"
    Protected Sub gvMismatchpI_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMismatchpI.RowCommand
        Try

            If e.CommandName = "MappingPI" Then
                Dim purchseinvoiceDt As DataTable
                Dim gvr As GridViewRow = gvMismatchpI.Rows(e.CommandArgument)
                Dim lblBookingNo As Label = CType(gvr.FindControl("lblBookingNo"), Label)

                gvMapdetail1.DataSource = Nothing
                gvMapdetail1.DataBind()
                gvMapdetail2.DataSource = Nothing
                gvMapdetail2.DataBind()

                txtBookingNo.Text = lblBookingNo.Text
                purchseinvoiceDt = Session("dtImportBookingExcel")
                If purchseinvoiceDt.Rows.Count = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('New Booking is blank...' );", True)
                    Exit Sub
                End If
                ShowGVmapdetailPopup(gvr, purchseinvoiceDt)
                ModalExtraPopup.Show()

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


#Region "Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_old_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lBeginTranStarted As Boolean = False
        Try
            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")

            Dim dt As New DataTable()
            dt.Columns.Add(New DataColumn("UniqueId", GetType(Integer)))
            dt.Columns.Add(New DataColumn("Bookingcode", GetType(String)))
            dt.Columns.Add(New DataColumn("Mainpssgrname", GetType(String)))
            dt.Columns.Add(New DataColumn("Date", GetType(DateTime)))
            dt.Columns.Add(New DataColumn("Linebookingdate", GetType(DateTime)))
            dt.Columns.Add(New DataColumn("Startdate", GetType(String)))
            dt.Columns.Add(New DataColumn("Enddate", GetType(String)))
            dt.Columns.Add(New DataColumn("Suppname", GetType(String)))
            dt.Columns.Add(New DataColumn("SuppID", GetType(String)))
            dt.Columns.Add(New DataColumn("ExchRate", GetType(Decimal)))
            dt.Columns.Add(New DataColumn("Description", GetType(String)))
            dt.Columns.Add(New DataColumn("SalesPrice", GetType(Decimal)))
            dt.Columns.Add(New DataColumn("Salescurr", GetType(String)))
            dt.Columns.Add(New DataColumn("Prodgrp", GetType(String)))
            dt.Columns.Add(New DataColumn("Costprice", GetType(Decimal)))
            dt.Columns.Add(New DataColumn("Costcurr", GetType(String)))
            dt.Columns.Add(New DataColumn("SupprefNum", GetType(String)))
            dt.Columns.Add(New DataColumn("Nationality", GetType(String)))
            dt.Columns.Add(New DataColumn("NoofRooms", GetType(Integer)))
            dt.Columns.Add(New DataColumn("Agency", GetType(String)))
            dt.Columns.Add(New DataColumn("Agencybookingref", GetType(String)))
            dt.Columns.Add(New DataColumn("ClientID", GetType(String)))
            dt.Columns.Add(New DataColumn("CtryAgency", GetType(String)))
            dt.Columns.Add(New DataColumn("Mapagentcode", GetType(String)))
            dt.Columns.Add(New DataColumn("Mappartycode", GetType(String)))
            dt.Columns.Add(New DataColumn("Mapsalecurr", GetType(String)))
            dt.Columns.Add(New DataColumn("Mapctryagent", GetType(String)))
            dt.Columns.Add(New DataColumn("MapService", GetType(String)))
            dt.Columns.Add(New DataColumn("PaxNumber", GetType(String)))
            dt.Columns.Add(New DataColumn("BookingElementId", GetType(Long)))

            Dim lblBookingNo As Label
            Dim lblMainpssg As Label
            Dim lblDt As Label
            Dim lblLinebookingdt As Label
            Dim lblnightsNum As Label
            Dim lblstartdate As Label
            Dim lblenddate As Label
            Dim lblSupname As Label
            Dim lblSuppcode As Label
            Dim lblexcgrate As Label
            Dim lbldesc As Label
            Dim lblSalesPrc As Label
            Dim lblSalescurr As Label
            Dim lblProdgrp As Label
            Dim lblCostprice As Label
            Dim lblCostcurr As Label
            Dim lblSupprefNum As Label
            Dim lblNoofRooms As Label
            Dim lblAgency As Label
            Dim lblAgencybookingref As Label
            Dim lblClientID As Label
            Dim lblCtryAgency As Label
            Dim lblBookElementId As Label

            Dim txtUniqueId As TextBox
            Dim txtMapparty As TextBox
            Dim txtMapAgent As TextBox
            Dim txtMapsalecurr As TextBox
            Dim txtMapAgentCtry As TextBox
            Dim txtCheckHotelSupplier As TextBox
            Dim txtMapService As TextBox
            Dim txtPaxNumber As TextBox
            Dim txtIgnore As TextBox
            Dim chkSelect As CheckBox

            Dim tmpbookingNo As String = ""
            Dim tmpbook As String = ""

            For Each grv As GridViewRow In gvBookingDetails.Rows

                chkSelect = CType(grv.FindControl("chkSelect"), CheckBox)
                txtIgnore = CType(grv.FindControl("txtIgnore"), TextBox)

                If chkSelect.Checked = True And txtIgnore.Text <> "1" Then
                    lblBookingNo = CType(grv.FindControl("lblBookingNo"), Label)
                    txtUniqueId = CType(grv.FindControl("txtUniqueId"), TextBox)
                    lblstartdate = CType(grv.FindControl("lblstartdate"), Label)
                    lblenddate = CType(grv.FindControl("lblenddate"), Label)

                    txtCheckHotelSupplier = CType(grv.FindControl("txtCheckHotelSupplier"), TextBox)
                    If txtCheckHotelSupplier.Text = "1" Then
                        ModalPopupLoading.Hide()
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Hotel and Supplier are different in the booking : " + lblBookingNo.Text + "');", True)
                        Exit Sub
                    End If

                    'added param on 24/07/2021
                    If IsDate(sealdate) Then
                        Dim invoicedate As String = ""
                        tmpbook = lblBookingNo.Text.Trim
                        If tmpbookingNo <> tmpbook Then
                            tmpbookingNo = tmpbook
                            invoicedate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select invoicedate from InvoiceHeader(nolock) where requestid='" + tmpbookingNo + "'")
                        End If

                        If IsDate(invoicedate) Then
                            If (CType(invoicedate, Date) <= CType(sealdate, Date)) Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") invoice Date with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                Exit Sub
                            End If
                        Else
                            If hdnChkDtFlag.Value = "Y" Then
                                If (CType(lblstartdate.Text, Date) <= CType(sealdate, Date)) Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                    Exit Sub
                                End If
                            Else
                                If (CType(lblenddate.Text, Date) <= CType(sealdate, Date)) Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                    lblMainpssg = CType(grv.FindControl("lblMainpssgrname"), Label)
                    lblDt = CType(grv.FindControl("lblDate"), Label)
                    lblLinebookingdt = CType(grv.FindControl("lbllinebookdate"), Label)
                    lblnightsNum = CType(grv.FindControl("lblnoofnights"), Label)
                    lblSupname = CType(grv.FindControl("lblsuppname"), Label)
                    lblSuppcode = CType(grv.FindControl("lblsuppid"), Label)
                    lblexcgrate = CType(grv.FindControl("lblexchangerate"), Label)

                    lbldesc = CType(grv.FindControl("lbldescription"), Label)
                    lblSalesPrc = CType(grv.FindControl("lblsalesprice"), Label)
                    lblSalescurr = CType(grv.FindControl("lblsalescurr"), Label)

                    lblProdgrp = CType(grv.FindControl("lblprodgroup"), Label)
                    lblCostprice = CType(grv.FindControl("lblCostprice"), Label)
                    lblCostcurr = CType(grv.FindControl("lblcostcurr"), Label)
                    lblSupprefNum = CType(grv.FindControl("lblsupprefno"), Label)

                    lblNoofRooms = CType(grv.FindControl("lblNoofRooms"), Label)
                    lblAgency = CType(grv.FindControl("lblAgency"), Label)
                    lblAgencybookingref = CType(grv.FindControl("lblAgentBookingRef"), Label)
                    lblClientID = CType(grv.FindControl("lblClientId"), Label)
                    lblCtryAgency = CType(grv.FindControl("lblAgencyCtry"), Label)
                    lblBookElementId = CType(grv.FindControl("lblBookElementId"), Label)

                    txtMapparty = CType(grv.FindControl("txtMapparty"), TextBox)
                    txtMapAgent = CType(grv.FindControl("txtMapAgent"), TextBox)
                    txtMapsalecurr = CType(grv.FindControl("txtMapsalecurr"), TextBox)
                    txtMapAgentCtry = CType(grv.FindControl("txtMapAgentCtry"), TextBox)
                    txtMapService = CType(grv.FindControl("txtMapService"), TextBox)
                    txtPaxNumber = CType(grv.FindControl("txtPaxNumber"), TextBox)

                    Dim dr As DataRow = dt.NewRow
                    dr("UniqueId") = Convert.ToInt32(txtUniqueId.Text)
                    dr("Bookingcode") = Convert.ToString(lblBookingNo.Text)
                    dr("Mainpssgrname") = Convert.ToString(lblMainpssg.Text)
                    dr("Date") = Convert.ToDateTime(lblDt.Text).ToString()
                    dr("Linebookingdate") = Convert.ToDateTime(lblLinebookingdt.Text).ToString()
                    dr("Startdate") = Convert.ToDateTime(lblstartdate.Text).ToString("yyyy/MM/dd")
                    dr("Enddate") = Convert.ToDateTime(lblenddate.Text).ToString("yyyy/MM/dd")
                    dr("Suppname") = Convert.ToString(lblSupname.Text)
                    dr("SuppID") = Convert.ToString(lblSuppcode.Text)
                    'dr("ExchRate") = ""
                    dr("Description") = Convert.ToString(lbldesc.Text)
                    dr("SalesPrice") = Convert.ToDecimal(lblSalesPrc.Text)
                    dr("Salescurr") = Convert.ToString(lblSalescurr.Text)
                    dr("Prodgrp") = Convert.ToString(lblProdgrp.Text)
                    dr("Costprice") = Convert.ToDouble(lblCostprice.Text)
                    dr("Costcurr") = Convert.ToString(lblCostcurr.Text)
                    dr("SupprefNum") = Convert.ToString(lblSupprefNum.Text)
                    'dr("Nationality") = ""
                    'dr("NoofRooms") = ""
                    dr("Agency") = Convert.ToString(lblAgency.Text)
                    dr("Agencybookingref") = Convert.ToString(lblAgencybookingref.Text)
                    dr("ClientID") = Convert.ToString(lblClientID.Text)
                    dr("CtryAgency") = Convert.ToString(lblCtryAgency.Text)
                    dr("Mapagentcode") = Convert.ToString(txtMapAgent.Text)
                    dr("Mappartycode") = Convert.ToString(txtMapparty.Text)
                    dr("Mapsalecurr") = Convert.ToString(txtMapsalecurr.Text)
                    dr("Mapctryagent") = Convert.ToString(txtMapAgentCtry.Text)
                    dr("mapService") = Convert.ToString(txtMapService.Text)
                    dr("PaxNumber") = Convert.ToString(txtPaxNumber.Text)
                    dr("BookingElementId") = Convert.ToInt64(lblBookElementId.Text)

                    dt.Rows.Add(dr)
                End If
            Next

            For Each grv As GridViewRow In gvAmendBooking.Rows

                chkSelect = CType(grv.FindControl("chkSelect"), CheckBox)
                txtIgnore = CType(grv.FindControl("txtIgnore"), TextBox)

                If chkSelect.Checked = True And txtIgnore.Text <> "1" Then
                    lblBookingNo = CType(grv.FindControl("lblBookingNo"), Label)
                    txtUniqueId = CType(grv.FindControl("txtUniqueId"), TextBox)
                    lblstartdate = CType(grv.FindControl("lblstartdate"), Label)
                    lblenddate = CType(grv.FindControl("lblenddate"), Label)

                    txtCheckHotelSupplier = CType(grv.FindControl("txtCheckHotelSupplier"), TextBox)
                    If txtCheckHotelSupplier.Text = "1" Then
                        ModalPopupLoading.Hide()
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Hotel and Supplier are different in the booking : " + lblBookingNo.Text + "');", True)
                        Exit Sub
                    End If

                    'added param on 24/07/2021
                    If IsDate(sealdate) Then
                        Dim invoicedate As String = ""
                        tmpbook = lblBookingNo.Text.Trim
                        If tmpbookingNo <> tmpbook Then
                            tmpbookingNo = tmpbook
                            invoicedate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select invoicedate from InvoiceHeader(nolock) where requestid='" + tmpbookingNo + "'")
                        End If

                        If IsDate(invoicedate) Then
                            If (CType(invoicedate, Date) <= CType(sealdate, Date)) Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") invoice Date with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                Exit Sub
                            End If
                        Else
                            If hdnChkDtFlag.Value = "Y" Then
                                If (CType(lblstartdate.Text, Date) <= CType(sealdate, Date)) Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                    Exit Sub
                                End If
                            Else
                                If (CType(lblenddate.Text, Date) <= CType(sealdate, Date)) Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                    lblMainpssg = CType(grv.FindControl("lblMainpssgrname"), Label)
                    lblDt = CType(grv.FindControl("lblDate"), Label)
                    lblLinebookingdt = CType(grv.FindControl("lbllinebookdate"), Label)
                    lblnightsNum = CType(grv.FindControl("lblnoofnights"), Label)
                    lblSupname = CType(grv.FindControl("lblsuppname"), Label)
                    lblSuppcode = CType(grv.FindControl("lblsuppid"), Label)
                    lblexcgrate = CType(grv.FindControl("lblexchangerate"), Label)

                    lbldesc = CType(grv.FindControl("lbldescription"), Label)
                    lblSalesPrc = CType(grv.FindControl("lblsalesprice"), Label)
                    lblSalescurr = CType(grv.FindControl("lblsalescurr"), Label)

                    lblProdgrp = CType(grv.FindControl("lblprodgroup"), Label)
                    lblCostprice = CType(grv.FindControl("lblCostprice"), Label)
                    lblCostcurr = CType(grv.FindControl("lblcostcurr"), Label)
                    lblSupprefNum = CType(grv.FindControl("lblsupprefno"), Label)

                    lblNoofRooms = CType(grv.FindControl("lblNoofRooms"), Label)
                    lblAgency = CType(grv.FindControl("lblAgency"), Label)
                    lblAgencybookingref = CType(grv.FindControl("lblAgentBookingRef"), Label)
                    lblClientID = CType(grv.FindControl("lblClientId"), Label)
                    lblCtryAgency = CType(grv.FindControl("lblAgencyCtry"), Label)
                    lblBookElementId = CType(grv.FindControl("lblBookElementId"), Label)

                    txtMapparty = CType(grv.FindControl("txtMapparty"), TextBox)
                    txtMapAgent = CType(grv.FindControl("txtMapAgent"), TextBox)
                    txtMapsalecurr = CType(grv.FindControl("txtMapsalecurr"), TextBox)
                    txtMapAgentCtry = CType(grv.FindControl("txtMapAgentCtry"), TextBox)
                    txtMapService = CType(grv.FindControl("txtMapService"), TextBox)
                    txtPaxNumber = CType(grv.FindControl("txtPaxNumber"), TextBox)

                    Dim dr As DataRow = dt.NewRow
                    dr("UniqueId") = Convert.ToInt32(txtUniqueId.Text)
                    dr("Bookingcode") = Convert.ToString(lblBookingNo.Text)
                    dr("Mainpssgrname") = Convert.ToString(lblMainpssg.Text)
                    dr("Date") = Convert.ToDateTime(lblDt.Text).ToString()
                    dr("Linebookingdate") = Convert.ToDateTime(lblLinebookingdt.Text).ToString()
                    dr("Startdate") = Convert.ToDateTime(lblstartdate.Text).ToString("yyyy/MM/dd")
                    dr("Enddate") = Convert.ToDateTime(lblenddate.Text).ToString("yyyy/MM/dd")
                    dr("Suppname") = Convert.ToString(lblSupname.Text)
                    dr("SuppID") = Convert.ToString(lblSuppcode.Text)
                    'dr("ExchRate") = ""
                    dr("Description") = Convert.ToString(lbldesc.Text)
                    dr("SalesPrice") = Convert.ToDecimal(lblSalesPrc.Text)
                    dr("Salescurr") = Convert.ToString(lblSalescurr.Text)
                    dr("Prodgrp") = Convert.ToString(lblProdgrp.Text)
                    dr("Costprice") = Convert.ToDouble(lblCostprice.Text)
                    dr("Costcurr") = Convert.ToString(lblCostcurr.Text)
                    dr("SupprefNum") = Convert.ToString(lblSupprefNum.Text)
                    'dr("Nationality") = ""
                    'dr("NoofRooms") = ""
                    dr("Agency") = Convert.ToString(lblAgency.Text)
                    dr("Agencybookingref") = Convert.ToString(lblAgencybookingref.Text)
                    dr("ClientID") = Convert.ToString(lblClientID.Text)
                    dr("CtryAgency") = Convert.ToString(lblCtryAgency.Text)
                    dr("Mapagentcode") = Convert.ToString(txtMapAgent.Text)
                    dr("Mappartycode") = Convert.ToString(txtMapparty.Text)
                    dr("Mapsalecurr") = Convert.ToString(txtMapsalecurr.Text)
                    dr("Mapctryagent") = Convert.ToString(txtMapAgentCtry.Text)
                    dr("mapService") = Convert.ToString(txtMapService.Text)
                    dr("PaxNumber") = Convert.ToString(txtPaxNumber.Text)
                    dr("BookingElementId") = Convert.ToInt64(lblBookElementId.Text)

                    dt.Rows.Add(dr)
                End If
            Next

            Dim Canceldt As New DataTable()
            Dim cancelBookingXml As String = ""
            Canceldt.Columns.Add(New DataColumn("Bookingcode", GetType(String)))
            Dim cancelbookings As String = ""
            For Each grv As GridViewRow In GvCancelBooking.Rows
                Dim drow As DataRow = Canceldt.NewRow
                lblBookingNo = CType(grv.FindControl("lblBookingNo"), Label)
                If Not (cancelbookings.Contains(lblBookingNo.Text)) Then
                    cancelbookings = cancelbookings + "," + lblBookingNo.Text.Trim
                    drow("Bookingcode") = lblBookingNo.Text.Trim
                    Canceldt.Rows.Add(drow)
                End If
            Next
            If Canceldt.Rows.Count > 0 Then
                Canceldt.TableName = "cancelBookings"
                cancelBookingXml = objUtils.GenerateXML(Canceldt)
            End If

            If dt.Rows.Count > 0 Then
                Dim grpbooking = (From n In dt.AsEnumerable Group By Bookingcode = n.Field(Of String)("Bookingcode") Into grp = Group Order By Bookingcode Select New With {.Bookingcode = Bookingcode}).ToList()


                Dim optionval As String = ""
                Dim sptype As String = "MAPBOOKING"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

                For Each grpItem In grpbooking
                    Try
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                        lBeginTranStarted = True
                        If optionval = "" Then
                            optionval = objUtils.GetAutoDocNo(sptype, mySqlConn, sqlTrans)
                        End If
                        Dim bookingCode As String = grpItem.Bookingcode.ToString
                        Dim bookingDt As DataTable = (From n In dt.AsEnumerable Where n.Field(Of String)("Bookingcode") = bookingCode Select n).CopyToDataTable()
                        bookingDt.TableName = "BookingServices"
                        Dim bookingXml As String = objUtils.GenerateXML(bookingDt)

                        mySqlCmd = New SqlCommand("sp_add_importBooking", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@import_id", SqlDbType.VarChar, 20)).Value = optionval
                        mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@filename", SqlDbType.VarChar, 200)).Value = lblFileName.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@bookingCode", SqlDbType.VarChar, 20)).Value = bookingCode
                        mySqlCmd.Parameters.Add(New SqlParameter("@bookingXml", SqlDbType.VarChar, -1)).Value = bookingXml
                        mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                        sqlTrans.Commit()
                        lBeginTranStarted = False
                    Catch ex As Exception
                        If lBeginTranStarted = True Then
                            sqlTrans.Rollback()
                        End If
                    End Try
                Next

                If Canceldt.Rows.Count > 0 Then
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    lBeginTranStarted = True
                    mySqlCmd = New SqlCommand("sp_update_importBooking", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@cancelBookingXml", SqlDbType.VarChar, -1)).Value = cancelBookingXml
                    mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandTimeout = 0 'Tanvir 21022023
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd.Dispose()
                    sqlTrans.Commit()
                    lBeginTranStarted = False
                End If



                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                txtID.Text = optionval
                ModalPopupLoading.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File Imported and Saved Successfully.');", True)
                btnSave.Visible = False

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ImportBookingPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If lBeginTranStarted = True Then
                sqlTrans.Rollback()
            End If

            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbSqlTransation(sqlTrans)
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)
                End If
            End If

            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ImportBookingPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

#Region "Protected Sub gvAmendBooking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAmendBooking.RowDataBound"
    Protected Sub gvAmendBooking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAmendBooking.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then

        '    Dim txtMapAgent As TextBox
        '    Dim txtMappartycode As TextBox
        '    Dim txtMapsalecurr As TextBox
        '    Dim txtMapAgentCtry As TextBox
        '    Dim txtMapService As TextBox
        '    Dim txtPaxNumber As TextBox
        '    Dim txtCheckHotelSupplier As TextBox

        '    Dim lblSalescurr As Label
        '    Dim lblBookingcode As Label
        '    Dim lblMainpssg As Label
        '    Dim lblDt As Label
        '    Dim lblLinebookingdt As Label
        '    Dim lblStartdt As Label
        '    Dim lblEnddt As Label

        '    Dim lbldesc As Label
        '    Dim lblsalesprice As Label
        '    Dim lblProdgrp As Label
        '    Dim lblClientID As Label
        '    Dim lblCtryAgency As Label

        '    Dim lblSuppcode As Label
        '    Dim lblsuppname As Label
        '    Dim lblCostprice As Label
        '    Dim lblCostcurr As Label
        '    Dim txtIgnore As TextBox
        '    Dim chkSelect As CheckBox

        '    Dim lblBookElementId As Label

        '    chkSelect = CType(e.Row.FindControl("chkSelect"), CheckBox)
        '    txtIgnore = CType(e.Row.FindControl("txtIgnore"), TextBox)
        '    txtMapAgent = CType(e.Row.FindControl("txtMapAgent"), TextBox)
        '    txtMappartycode = CType(e.Row.FindControl("txtMapparty"), TextBox)
        '    txtMapsalecurr = CType(e.Row.FindControl("txtMapsalecurr"), TextBox)
        '    txtMapAgentCtry = CType(e.Row.FindControl("txtMapAgentCtry"), TextBox)
        '    txtMapService = CType(e.Row.FindControl("txtMapService"), TextBox)
        '    txtPaxNumber = CType(e.Row.FindControl("txtPaxNumber"), TextBox)
        '    txtCheckHotelSupplier = CType(e.Row.FindControl("txtCheckHotelSupplier"), TextBox)

        '    lblBookingcode = CType(e.Row.FindControl("lblBookingNo"), Label)
        '    lblMainpssg = CType(e.Row.FindControl("lblMainpssgrname"), Label)
        '    lblDt = CType(e.Row.FindControl("lblDate"), Label)
        '    lblLinebookingdt = CType(e.Row.FindControl("lbllinebookdate"), Label)
        '    lblStartdt = CType(e.Row.FindControl("lblstartdate"), Label)
        '    lblEnddt = CType(e.Row.FindControl("lblenddate"), Label)


        '    'lblexcgrate = CType(e.Row.FindControl("lblexchangerate"), Label)
        '    lbldesc = CType(e.Row.FindControl("lbldescription"), Label)
        '    lblProdgrp = CType(e.Row.FindControl("lblprodgroup"), Label)
        '    lblsalesprice = CType(e.Row.FindControl("lblsalesprice"), Label)
        '    lblSalescurr = CType(e.Row.FindControl("lblsalescurr"), Label)
        '    lblClientID = CType(e.Row.FindControl("lblClientId"), Label)
        '    lblCtryAgency = CType(e.Row.FindControl("lblAgencyCtry"), Label)

        '    lblSuppcode = CType(e.Row.FindControl("lblsuppid"), Label)
        '    lblsuppname = CType(e.Row.FindControl("lblsuppname"), Label)
        '    lblCostprice = CType(e.Row.FindControl("lblCostprice"), Label)
        '    lblCostcurr = CType(e.Row.FindControl("lblcostcurr"), Label)
        '    lblBookElementId = CType(e.Row.FindControl("lblBookElementId"), Label)

        '    'MApping cells

        '    If txtMapAgent.Text.Trim = "" Then
        '        e.Row.Cells(4).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If
        '    If txtMappartycode.Text.Trim = "" Then
        '        e.Row.Cells(11).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If

        '    If txtMapsalecurr.Text.Trim <> lblSalescurr.Text.Trim Then
        '        e.Row.Cells(9).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If
        '    If txtMapAgentCtry.Text.Trim = "" Then
        '        e.Row.Cells(18).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        Session("MapFlag") = 1
        '    End If

        '    If lblProdgrp.Text.ToLower = "visa" And txtMapService.Text = "" Then
        '        e.Row.Cells(7).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        lbldesc.ToolTip = "Visa type is not matching"
        '        Session("MapFlag") = 1
        '    End If

        '    If lblProdgrp.Text.ToLower = "visa" And txtPaxNumber.Text = "" Then
        '        e.Row.Cells(7).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        lbldesc.ToolTip = "Pax detail is missing for Visa"
        '        Session("MapFlag") = 1
        '    End If

        '    If txtCheckHotelSupplier.Text = "1" Then
        '        e.Row.Cells(11).BackColor = System.Drawing.Color.FromArgb(227, 225, 127)
        '        lblsuppname.ToolTip = "Hotel and Supplier are different in the booking"
        '        Session("MapFlag") = 1
        '    End If

        '    'validate flag

        '    If lblBookingcode.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(1).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblMainpssg.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(6).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblDt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(14).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblLinebookingdt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(15).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblStartdt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(2).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblEnddt.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(3).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblSuppcode.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(16).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    'If lblexcgrate.Text = "" Then
        '    '    Session("ValidateFlag") = 1
        '    '    e.Row.Cells(11).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    'End If
        '    If lbldesc.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(7).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If

        '    If lblsalesprice.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(10).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblSalescurr.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(9).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblProdgrp.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(8).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblCostprice.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(13).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblCostcurr.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(12).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblClientID.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(17).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblCtryAgency.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(18).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If
        '    If lblBookElementId.Text = "" Then
        '        Session("ValidateFlag") = 1
        '        e.Row.Cells(23).BackColor = System.Drawing.Color.FromArgb(0, 255, 255)
        '    End If

        '    If txtIgnore.Text = "1" Then
        '        chkSelect.Checked = False
        '        e.Row.BackColor = System.Drawing.Color.FromArgb(211, 211, 211)
        '    Else
        '        chkSelect.Checked = True
        '    End If

        'End If
    End Sub
#End Region

#Region "Public Class Reconcile"
    Public Class Reconcile
        Private _keys As String
        Private _Classification As String
        Private _NoOfBookings As Integer
        Private _totalSaleValue As Decimal
        Private _totalCostValue As Decimal

        Public Property Keys As String
            Get
                Return _keys
            End Get
            Set(ByVal value As String)
                _keys = value
            End Set
        End Property

        Public Property Classification As String
            Get
                Return _Classification
            End Get
            Set(ByVal value As String)
                _Classification = value
            End Set
        End Property

        Public Property NoOfBookings As Integer
            Get
                Return _NoOfBookings
            End Get
            Set(ByVal value As Integer)
                _NoOfBookings = value
            End Set
        End Property

        Public Property TotalSaleValue As Decimal
            Get
                Return _totalSaleValue
            End Get
            Set(ByVal value As Decimal)
                _totalSaleValue = value
            End Set
        End Property

        Public Property TotalCostValue As Decimal
            Get
                Return _totalCostValue
            End Get
            Set(ByVal value As Decimal)
                _totalCostValue = value
            End Set
        End Property

    End Class
#End Region

#Region "Public Class ReconcileSummary"
    Public Class ReconcileSummary
        Private _Requestid As String
        Private _salecurrency As String
        Private _BookingCode As String
        Private _SaleValue As Decimal
        Private _SaleNonTaxable As Decimal
        Private _SaleTaxable As Decimal
        Private _SaleVAT As Decimal
        Private _CostValue As Decimal
        Private _CostNonTaxable As Decimal
        Private _CostTaxable As Decimal
        Private _CostVAT As Decimal
        Private _BookingType As String
        Private _arrivalDate As Date
        Private _departureDate As Date

        Public Property arrivalDate As String
            Get
                Return _arrivalDate
            End Get
            Set(ByVal value As String)
                _arrivalDate = value
            End Set
        End Property
        Public Property departureDate As String
            Get
                Return _departureDate
            End Get
            Set(ByVal value As String)
                _departureDate = value
            End Set
        End Property

        Public Property BookingType As String
            Get
                Return _BookingType
            End Get
            Set(ByVal value As String)
                _BookingType = value
            End Set
        End Property
        Public Property Requestid As String
            Get
                Return _Requestid
            End Get
            Set(ByVal value As String)
                _Requestid = value
            End Set
        End Property
        Public Property salecurrency As String
            Get
                Return _salecurrency
            End Get
            Set(ByVal value As String)
                _salecurrency = value
            End Set
        End Property
        Public Property BookingCode As String
            Get
                Return _BookingCode
            End Get
            Set(ByVal value As String)
                _BookingCode = value
            End Set
        End Property
        Public Property SaleValue As Decimal
            Get
                Return _SaleValue
            End Get
            Set(ByVal value As Decimal)
                _SaleValue = value
            End Set
        End Property
        Public Property SaleNonTaxable As Decimal
            Get
                Return _SaleNonTaxable
            End Get
            Set(ByVal value As Decimal)
                _SaleNonTaxable = value
            End Set
        End Property
        Public Property SaleTaxable As Decimal
            Get
                Return _SaleTaxable
            End Get
            Set(ByVal value As Decimal)
                _SaleTaxable = value
            End Set
        End Property
        Public Property SaleVAT As Decimal
            Get
                Return _SaleVAT
            End Get
            Set(ByVal value As Decimal)
                _SaleVAT = value
            End Set
        End Property

        Public Property CostValue As Decimal
            Get
                Return _CostValue
            End Get
            Set(ByVal value As Decimal)
                _CostValue = value
            End Set
        End Property
        Public Property CostNonTaxable As Decimal
            Get
                Return _CostNonTaxable
            End Get
            Set(ByVal value As Decimal)
                _CostNonTaxable = value
            End Set
        End Property
        Public Property CostTaxable As Decimal
            Get
                Return _CostTaxable
            End Get
            Set(ByVal value As Decimal)
                _CostTaxable = value
            End Set
        End Property
        Public Property CostVAT As Decimal
            Get
                Return _CostVAT
            End Get
            Set(ByVal value As Decimal)
                _CostVAT = value
            End Set
        End Property
    End Class
#End Region

#Region "Public Class Mapingbookinglist"
    Public Class Mapingbookinglist
        Private _Requestid As String
        Private _salecurrency As String
        Private _BookingCode As String
        Private _SaleValue As Decimal
        Private _SaleNonTaxable As Decimal
        Private _SaleTaxable As Decimal
        Private _SaleVAT As Decimal
        Private _CostValue As Decimal
        Private _CostNonTaxable As Decimal
        Private _CostTaxable As Decimal
        Private _CostVAT As Decimal
        Private _BookingType As String
        Private _arrivalDate As Date
        Private _departureDate As Date

        Public Property arrivalDate As String
            Get
                Return _arrivalDate
            End Get
            Set(ByVal value As String)
                _arrivalDate = value
            End Set
        End Property
        Public Property departureDate As String
            Get
                Return _departureDate
            End Get
            Set(ByVal value As String)
                _departureDate = value
            End Set
        End Property

        Public Property BookingType As String
            Get
                Return _BookingType
            End Get
            Set(ByVal value As String)
                _BookingType = value
            End Set
        End Property
        Public Property Requestid As String
            Get
                Return _Requestid
            End Get
            Set(ByVal value As String)
                _Requestid = value
            End Set
        End Property
        Public Property salecurrency As String
            Get
                Return _salecurrency
            End Get
            Set(ByVal value As String)
                _salecurrency = value
            End Set
        End Property
        Public Property BookingCode As String
            Get
                Return _BookingCode
            End Get
            Set(ByVal value As String)
                _BookingCode = value
            End Set
        End Property
        Public Property SaleValue As Decimal
            Get
                Return _SaleValue
            End Get
            Set(ByVal value As Decimal)
                _SaleValue = value
            End Set
        End Property
        Public Property SaleNonTaxable As Decimal
            Get
                Return _SaleNonTaxable
            End Get
            Set(ByVal value As Decimal)
                _SaleNonTaxable = value
            End Set
        End Property
        Public Property SaleTaxable As Decimal
            Get
                Return _SaleTaxable
            End Get
            Set(ByVal value As Decimal)
                _SaleTaxable = value
            End Set
        End Property
        Public Property SaleVAT As Decimal
            Get
                Return _SaleVAT
            End Get
            Set(ByVal value As Decimal)
                _SaleVAT = value
            End Set
        End Property

        Public Property CostValue As Decimal
            Get
                Return _CostValue
            End Get
            Set(ByVal value As Decimal)
                _CostValue = value
            End Set
        End Property
        Public Property CostNonTaxable As Decimal
            Get
                Return _CostNonTaxable
            End Get
            Set(ByVal value As Decimal)
                _CostNonTaxable = value
            End Set
        End Property
        Public Property CostTaxable As Decimal
            Get
                Return _CostTaxable
            End Get
            Set(ByVal value As Decimal)
                _CostTaxable = value
            End Set
        End Property
        Public Property CostVAT As Decimal
            Get
                Return _CostVAT
            End Get
            Set(ByVal value As Decimal)
                _CostVAT = value
            End Set
        End Property
    End Class
#End Region

#Region "Protected Sub gvReconcile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReconcile.RowDataBound"
    Protected Sub gvReconcile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReconcile.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblClass As Label
            lblClass = CType(e.Row.FindControl("lblClass"), Label)
            Dim lblBookingCnt As Label = CType(e.Row.FindControl("lblBookingCnt"), Label)
            Dim lblSaleValue As Label = CType(e.Row.FindControl("lblSaleValue"), Label)
            Dim lblCostValue As Label = CType(e.Row.FindControl("lblCostValue"), Label)

            Dim decno As Integer = Convert.ToInt32(txtDecno.Text)
            Dim strDecno As String = ""
            If decno = 3 Then
                strDecno = "0.000"
            Else
                strDecno = "0.00"
            End If

            If lblClass.Text = "Total" Then
                lblClass.Style.Add("font-weight", "Bold")
                lblBookingCnt.Style.Add("font-weight", "Bold")
                lblSaleValue.Style.Add("font-weight", "Bold")
                lblCostValue.Style.Add("font-weight", "Bold")
            End If '
            lblSaleValue.Text = Decimal.Parse(Math.Round(Val(lblSaleValue.Text), Convert.ToInt32(txtDecno.Text))).ToString(strDecno)
            lblCostValue.Text = Decimal.Parse(Math.Round(Val(lblCostValue.Text), Convert.ToInt32(txtDecno.Text))).ToString(strDecno)
        End If
    End Sub
#End Region

    Protected Sub btnExcelNewBooking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelNewBooking.Click
        Try
            'ExportToExcel("NewBookingXls" + Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", ""), "newbooking")
            'ExportToExcel("AmendBookingXls" + Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", ""), "amendbooking")
            'ExportToExcel("CancelBookingXls" + Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", ""), "cancelbooking")\
            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('ImportBookingXlsExport.aspx','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


            Exit Sub
        Catch ex As Exception
            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingNew.aspx :: btnExcelNewBooking_Click :: ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ModalPopupLoading.Hide()
        End Try
    End Sub
    'Sub ExportToExcel(ByVal lfileName As String, ByVal lOptionToExport As String)
    '    Dim lDataTable As DataTable
    '    If lOptionToExport.ToLower = "newbooking" Then
    '        If Session("dtImportBooking") Is Nothing Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
    '            Exit Sub
    '        Else
    '            lDataTable = CType(Session("dtImportBooking"), DataTable).Copy
    '        End If
    '    End If

    '    If lOptionToExport.ToLower = "amendbooking" Then
    '        If Session("dtAmendedBooking") Is Nothing Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
    '            Exit Sub
    '        Else
    '            lDataTable = CType(Session("dtAmendedBooking"), DataTable).Copy
    '        End If
    '    End If

    '    If lOptionToExport.ToLower = "cancelbooking" Then
    '        If Session("dtCancelBooking") Is Nothing Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
    '            Exit Sub
    '        Else
    '            lDataTable = CType(Session("dtCancelBooking"), DataTable).Copy
    '        End If
    '    End If

    '    Dim lDataset As New DataSet
    '    lDataset.Tables.Add(lDataTable)
    '    'Dim clsUtilities1 As New clsUtils
    '    'clsUtilities1.ExportToExcel(lDataset, Response)
    '    ExcelExportService(lfileName, lDataset)
    'End Sub


    Protected Sub btnReleaseInvoiceSealed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReleaseInvoiceSealed.Click
        Try
            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")
            Dim tmpbook As String = ""
            Dim tmpbookingNo As String = ""
            Dim invoicedate As String

            For Each gvRow As GridViewRow In gvReconcileSummary.Rows
                Dim lblBookingNo As Label = gvRow.FindControl("lblBookingNo")
                Dim lblInvoiceSealed As Label = gvRow.FindControl("lblInvoiceSealed")
                Dim lblstartdate As Label = gvRow.FindControl("lblstartdate")
                Dim lblenddate As Label = gvRow.FindControl("lblenddate")
                lblInvoiceSealed.Text = ""
                'added param on 24/07/2021
                If IsDate(sealdate) And lblBookingNo.Text.Trim <> "" Then
                    invoicedate = ""
                    tmpbook = lblBookingNo.Text.Trim
                    If tmpbookingNo <> tmpbook Then
                        tmpbookingNo = tmpbook
                        invoicedate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select invoicedate from InvoiceHeader(nolock) where requestid='" + tmpbookingNo + "'")
                    End If

                    If IsDate(invoicedate) Then
                        If (CType(invoicedate, Date) <= CType(sealdate, Date)) Then
                            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") invoice Date with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                            lblInvoiceSealed.Text = "Invoice date is within sealed date"
                            'Exit Sub
                        End If
                    Else
                        If hdnChkDtFlag.Value = "Y" Then
                            If (CType(lblstartdate.Text, Date) <= CType(sealdate, Date)) Then
                                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                'Exit Sub
                                lblInvoiceSealed.Text = "Arrival date is within sealed date"
                            End If
                        Else
                            If (CType(lblenddate.Text, Date) <= CType(sealdate, Date)) Then
                                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblBookingNo.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                'Exit Sub
                                lblInvoiceSealed.Text = "Arrival date is within sealed date"
                            End If
                        End If
                    End If
                End If


            Next
        Catch ex As Exception
            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingNew.aspx :: btnExcelNewBooking_Click :: ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ModalPopupLoading.Hide()
        End Try
    End Sub

End Class
