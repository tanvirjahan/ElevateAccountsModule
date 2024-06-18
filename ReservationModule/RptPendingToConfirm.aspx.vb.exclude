Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Linq

Partial Class RptPendingToConfirm
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
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim type As String = Convert.ToString(Request.QueryString("type"))
                If type <> "" Then
                    txtRptType.Text = type.Trim
                End If
                Dim ddlListItems() As String = {}
                If txtRptType.Text.Trim = "PendingToConfirm" Then
                    lblHeading.Text = "Pending Confirmation"
                    Me.Page.Title = "Pending Confirmation"
                    lblDdlCaption.Text = "Invoice Type"
                    ddlListItems = {"All", "New", "Amend"}
                    myReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "select division_master_code,division_master_des from division_master(nolock)")
                    If myReader.HasRows Then
                        While myReader.Read()
                            ddlDivision.Items.Add(New ListItem(myReader("division_master_des"), myReader("division_master_code")))
                        End While
                    End If
                    myReader.Close()
                ElseIf txtRptType.Text.Trim = "PendingUpdateSupplier" Then
                    lblHeading.Text = "Pending To Update Supplier And Cost"
                    Me.Page.Title = "Pending To Update Supplier And Cost"
                    lblDdlCaption.Text = "Report Type"
                    ddlListItems = {"Summary", "Details"}
                    trDivision.Visible = False
                End If
                For i = 0 To ddlListItems.GetUpperBound(0)
                    ddlInvoiceStateType.Items.Add(New ListItem(ddlListItems(i), ddlListItems(i)))
                Next

                If AppId.Value Is Nothing = False Then
                    strappid = AppId.Value
                End If
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                        CType(strappname, String), "ReservationModule\RptPendingToConfirm.aspx?type=" + txtRptType.Text.Trim, btnAddNew, btnExportToExcel, _
                        btnExportToExcel, gvSearch:=gvSearchResult)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptPendingToConfirm.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnLoadReport_Click(sender As Object, e As System.EventArgs) Handles btnLoadReport.Click"
    Protected Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReport.Click
        Try
            If txtRptType.Text.Trim = "PendingToConfirm" Then
                PendingConfirm()
            ElseIf txtRptType.Text = "PendingUpdateSupplier" Then
                UpdateSupplierAndCost()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptPendingToConfirm.aspx", Server.MapPath("ErrorLog.txt"), ex.StackTrace.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function Validation() As Boolean"
    Protected Function Validation() As Boolean
        Try
            If (Not IsDate(txtFromDt.Text) Or Not IsDate(txtToDt.Text)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Verify From Date and To Date' );", True)
                Validation = False
                Exit Function
            End If
            Validation = True
        Catch ex As Exception
            Validation = False
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Function BuildCondition() As String"
    Protected Function BuildCondition() As String
        Try
            Dim strWhereCond As String = ""
            If ddlInvoiceStateType.SelectedValue <> "Amend" Then
                strWhereCond = "isnull(invno,'')='' "
            End If
            BuildCondition = strWhereCond
        Catch ex As Exception
            BuildCondition = ""
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Sub PendingConfirm()"
    Protected Sub PendingConfirm()
        Try
            If Validation() = False Then Exit Sub
            Dim strBindCondition As String = BuildCondition()
            Dim strSortBy As String = "DepartureDate"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("bookingPending_salesInvoice_search", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = ddlDivision.SelectedValue
            myCommand.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strBindCondition
            myCommand.Parameters.Add(New SqlParameter("@sortBy", SqlDbType.VarChar, 100)).Value = strSortBy
            If IsDate(txtFromDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            If IsDate(txtToDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            myCommand.Parameters.Add(New SqlParameter("@SalesInvoiceState", SqlDbType.VarChar, 20)).Value = ddlInvoiceStateType.SelectedValue.Trim

            myDataAdapter = New SqlDataAdapter(myCommand)
            Dim myDs As New DataSet()
            myDataAdapter.Fill(myDs)
            Dim resultDt As DataTable = myDs.Tables(0)
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
            Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), Integer)
            Dim decPlaces As String
            If decno = 3 Then
                decPlaces = "#,##0.000;[Red](#,##0.000)"
            Else
                decPlaces = "#,##0.00;[Red](#,##0.00)"
            End If
            Dim baseCurrency As String = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

            If resultDt.Rows.Count > 0 Then
                Dim currCode As String = Convert.ToString(resultDt.Rows(0)("Currency"))
                Dim saleDecNo As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select nodigit from currmast where currcode='" + currCode + "'"), Integer)
                Dim SaleDecPlaces As String
                If saleDecNo = 3 Then
                    SaleDecPlaces = "#,##0.000;[Red](#,##0.000)"
                Else
                    SaleDecPlaces = "#,##0.00;[Red](#,##0.00)"
                End If
                Dim excelLastRow As Integer
                Dim runningRow As Integer

                Dim wbook As New XLWorkbook
                Dim wsPending As IXLWorksheet = wbook.Worksheets.Add("PendingConfirmation")
                wsPending.Style.Font.SetFontName("Trebuchet MS")
                Dim companyname As String = CType(Session("CompanyName"), String)
                excelLastRow = 2
                wsPending.Range("A" + excelLastRow.ToString + ":L" + excelLastRow.ToString).Merge()
                wsPending.Cell(excelLastRow, 1).Value = companyname
                wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(14)
                wsPending.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(197, 190, 151))

                excelLastRow = 4
                wsPending.Range("A" + excelLastRow.ToString + ":L" + excelLastRow.ToString).Merge()
                wsPending.Cell(excelLastRow, 1).Value = "Pending Confirmation"
                wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(12)
                wsPending.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(221, 217, 195))

                excelLastRow = 5
                wsPending.Range("A" + excelLastRow.ToString + ":L" + excelLastRow.ToString).Merge()
                wsPending.Cell(excelLastRow, 1).Value = "From Date :" + Convert.ToDateTime(txtFromDt.Text).ToString("dd/MM/yyyy") + " - To Date :" + Convert.ToDateTime(txtToDt.Text).ToString("dd/MM/yyyy") + "; Invoice Type : " + ddlInvoiceStateType.SelectedValue
                wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(10)

                excelLastRow = 7
                runningRow = excelLastRow
                Dim AmendTitle As String = ""
                If ddlInvoiceStateType.SelectedValue = "Amend" Then
                    AmendTitle = "Amended Date"
                Else
                    AmendTitle = "Amended"
                End If

                Dim tblTitle() As String = {"Request ID", "Request Date", "Status", AmendTitle, "Arrival Date", "Departure Date", "Customer Name", "Customer Ref.", "Guest Name", "Currency", "Amount", "Sales Amount (" + baseCurrency + ")"}
                For i = 0 To tblTitle.GetUpperBound(0)
                    wsPending.Cell(runningRow, i + 1).Value = tblTitle(i)
                Next
                Dim rngTitle As IXLRange
                rngTitle = wsPending.Range("A" + Convert.ToString(excelLastRow) + ":L" + Convert.ToString(runningRow))
                rngTitle.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
                rngTitle.Style.Font.SetBold(True)
                rngTitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)

                excelLastRow = excelLastRow + 1
                For Each dr As DataRow In resultDt.Rows
                    Dim ColNo As Integer = 0
                    runningRow = runningRow + 1

                    ColNo = 1
                    wsPending.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    wsPending.Cell(runningRow, ColNo).Value = dr("requestid")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = "dd/MM/yyyy"
                    wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Text
                    wsPending.Cell(runningRow, ColNo).Value = dr("requestdate")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Value = dr("status")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Text
                    wsPending.Cell(runningRow, ColNo).Value = dr("amended")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = "dd/MM/yyyy"
                    wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Text
                    wsPending.Cell(runningRow, ColNo).Value = dr("arrivalDate")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = "dd/MM/yyyy"
                    wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Text
                    wsPending.Cell(runningRow, ColNo).Value = dr("departuredate")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Value = dr("agentname")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Value = dr("agentref")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(True)
                    wsPending.Cell(runningRow, ColNo).Value = dr("guestname")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Value = dr("currency")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = SaleDecPlaces
                    wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Number
                    wsPending.Cell(runningRow, ColNo).Value = dr("amount")

                    ColNo = ColNo + 1
                    wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = decPlaces
                    wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Number
                    wsPending.Cell(runningRow, ColNo).Value = dr("salesamount")
                Next
                wsPending.Columns("1:12").AdjustToContents()
                wsPending.Range("A" + excelLastRow.ToString + ":J" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                wsPending.Range("K" + excelLastRow.ToString + ":L" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                Dim rngTblRange As IXLRange = wsPending.Range("A" + excelLastRow.ToString + ":L" + runningRow.ToString)
                rngTblRange.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
                rngTblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

                Dim FileNameNew As String = "pendingConfirmation_" & Now.Year & Now.Month.ToString("00") & Now.Day.ToString("00") & Now.Hour & Now.Minute & Now.Second & ".xlsx"
                Using MyMemoryStream As New MemoryStream()
                    wbook.SaveAs(MyMemoryStream)
                    wbook.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    'Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub UpdateSupplierAndCost()"
    Protected Sub UpdateSupplierAndCost()
        Try
            If Validation() = False Then Exit Sub
            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionRptPending"))
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("sp_rpt_updateSupplierAndCost", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            If IsDate(txtFromDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            If IsDate(txtToDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            myCommand.Parameters.Add(New SqlParameter("@rptType", SqlDbType.VarChar, 20)).Value = ddlInvoiceStateType.SelectedValue.Trim
            myDataAdapter = New SqlDataAdapter(myCommand)
            Dim myDs As New DataSet()
            myDataAdapter.Fill(myDs)
            Dim resultDt As DataTable = myDs.Tables(0)
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
            If resultDt.Rows.Count > 0 Then
                Dim wbook As New XLWorkbook
                Dim wsPending As IXLWorksheet = wbook.Worksheets.Add("PendingUpdateSupplier" + ddlInvoiceStateType.SelectedValue)
                wsPending.Style.Font.SetFontName("Trebuchet MS")

                If ddlInvoiceStateType.SelectedValue = "Summary" Then
                    SummaryTable(wsPending, resultDt)
                Else
                    DetailsTable(wsPending, resultDt)
                End If

                Dim FileNameNew As String = "PendingUpdateSupplier" + ddlInvoiceStateType.SelectedValue + "_" & Now.Year & Now.Month.ToString("00") & Now.Day.ToString("00") & Now.Hour & Now.Minute & Now.Second & ".xlsx"
                Using MyMemoryStream As New MemoryStream()
                    wbook.SaveAs(MyMemoryStream)
                    wbook.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    'Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub SummaryTable(ByRef wsPending As IXLWorksheet, ByRef resultDt As DataTable)"
    Protected Sub SummaryTable(ByRef wsPending As IXLWorksheet, ByRef resultDt As DataTable)
        Dim excelLastRow As Integer
        Dim runningRow As Integer
        Dim companyname As String = CType(Session("CompanyName"), String)
        excelLastRow = 2
        wsPending.Range("A" + excelLastRow.ToString + ":D" + excelLastRow.ToString).Merge()
        wsPending.Cell(excelLastRow, 1).Value = companyname
        wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(14)
        wsPending.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(197, 190, 151))

        excelLastRow = 4
        wsPending.Range("A" + excelLastRow.ToString + ":D" + excelLastRow.ToString).Merge()
        wsPending.Cell(excelLastRow, 1).Value = "Pending To Update Supplier And Cost"
        wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(12)
        wsPending.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(221, 217, 195))

        excelLastRow = 5
        wsPending.Range("A" + excelLastRow.ToString + ":D" + excelLastRow.ToString).Merge()
        wsPending.Cell(excelLastRow, 1).Value = "From Date :" + Convert.ToDateTime(txtFromDt.Text).ToString("dd/MM/yyyy") + " - To Date :" + Convert.ToDateTime(txtToDt.Text).ToString("dd/MM/yyyy") + " ; Report Type : " + ddlInvoiceStateType.SelectedValue.Trim
        wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(10)

        excelLastRow = 7
        runningRow = excelLastRow
        Dim tblTitle() As String = {"Request ID", "Agent Name", "Arrival Date", "Departure Date"}
        For i = 0 To tblTitle.GetUpperBound(0)
            wsPending.Cell(runningRow, i + 1).Value = tblTitle(i)
        Next
        Dim rngTitle As IXLRange
        rngTitle = wsPending.Range("A" + Convert.ToString(excelLastRow) + ":D" + Convert.ToString(runningRow))
        rngTitle.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
        rngTitle.Style.Font.SetBold(True)
        rngTitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)

        excelLastRow = excelLastRow + 1
        For Each dr As DataRow In resultDt.Rows
            Dim ColNo As Integer = 0
            runningRow = runningRow + 1

            ColNo = 1
            wsPending.Cell(runningRow, ColNo).Value = dr("requestid")

            ColNo = ColNo + 1
            wsPending.Cell(runningRow, ColNo).Value = dr("agentName")

            ColNo = ColNo + 1
            wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = "dd/MM/yyyy"
            wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Text
            wsPending.Cell(runningRow, ColNo).Value = dr("checkin")

            ColNo = ColNo + 1
            wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = "dd/MM/yyyy"
            wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Text
            wsPending.Cell(runningRow, ColNo).Value = dr("checkout")
        Next
        wsPending.Columns("1:4").AdjustToContents()
        Dim rngTblRange As IXLRange = wsPending.Range("A" + excelLastRow.ToString + ":D" + runningRow.ToString)
        rngTblRange.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(10).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        rngTblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    End Sub
#End Region

#Region "Protected Sub DetailsTable(ByRef wsPending As IXLWorksheet, ByRef resultDt As DataTable)"
    Protected Sub DetailsTable(ByRef wsPending As IXLWorksheet, ByRef resultDt As DataTable)
        Dim excelLastRow As Integer
        Dim runningRow As Integer
        Dim companyname As String = CType(Session("CompanyName"), String)
        excelLastRow = 2
        wsPending.Range("A" + excelLastRow.ToString + ":E" + excelLastRow.ToString).Merge()
        wsPending.Cell(excelLastRow, 1).Value = companyname
        wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(14)
        wsPending.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(197, 190, 151))

        excelLastRow = 4
        wsPending.Range("A" + excelLastRow.ToString + ":E" + excelLastRow.ToString).Merge()
        wsPending.Cell(excelLastRow, 1).Value = "Pending To Update Supplier And Cost "
        wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(12)
        wsPending.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(221, 217, 195))

        excelLastRow = 5
        wsPending.Range("A" + excelLastRow.ToString + ":E" + excelLastRow.ToString).Merge()
        wsPending.Cell(excelLastRow, 1).Value = "From Date :" + Convert.ToDateTime(txtFromDt.Text).ToString("dd/MM/yyyy") + " - To Date :" + Convert.ToDateTime(txtToDt.Text).ToString("dd/MM/yyyy") + " ; Report Type : " + ddlInvoiceStateType.SelectedValue.Trim
        wsPending.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        wsPending.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(10)

        excelLastRow = 7
        runningRow = excelLastRow
        Dim tblTitle() As String = {"Request ID", "Agent Name", "Service Date", "Service Type", "Service Details"}
        For i = 0 To tblTitle.GetUpperBound(0)
            wsPending.Cell(runningRow, i + 1).Value = tblTitle(i)
        Next
        Dim rngTitle As IXLRange
        rngTitle = wsPending.Range("A" + Convert.ToString(excelLastRow) + ":E" + Convert.ToString(runningRow))
        rngTitle.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
        rngTitle.Style.Font.SetBold(True)
        rngTitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)

        excelLastRow = excelLastRow + 1
        For Each dr As DataRow In resultDt.Rows
            Dim ColNo As Integer = 0
            runningRow = runningRow + 1

            ColNo = 1
            wsPending.Cell(runningRow, ColNo).Value = dr("requestid")

            ColNo = ColNo + 1
            wsPending.Cell(runningRow, ColNo).Value = dr("agentName")

            ColNo = ColNo + 1
            wsPending.Cell(runningRow, ColNo).Style.NumberFormat.Format = "dd/MM/yyyy"
            wsPending.Cell(runningRow, ColNo).DataType = XLCellValues.Text
            wsPending.Cell(runningRow, ColNo).Value = dr("serviceDate")

            ColNo = ColNo + 1
            wsPending.Cell(runningRow, ColNo).Value = dr("servicetype")

            ColNo = ColNo + 1
            wsPending.Cell(runningRow, ColNo).Style.Alignment.WrapText = True
            wsPending.Cell(runningRow, ColNo).Value = dr("servicedetails")
        Next
        wsPending.Columns("1:5").AdjustToContents()
        wsPending.Columns(5).Width = 65
        Dim rngTblRange As IXLRange = wsPending.Range("A" + excelLastRow.ToString + ":E" + runningRow.ToString)
        rngTblRange.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(10).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        rngTblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    End Sub
#End Region

#Region "Protected Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click"
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        txtFromDt.Text = ""
        txtToDt.Text = ""
        ddlInvoiceStateType.SelectedIndex = 0
        If txtRptType.Text.Trim = "PendingToConfirm" Then ddlDivision.SelectedIndex = 0
        txtFromDt.Focus()
    End Sub
#End Region

End Class
