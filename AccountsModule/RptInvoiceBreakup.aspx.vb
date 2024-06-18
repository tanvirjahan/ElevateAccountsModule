

'------------================--------------=======================------------------================
'   Module Name    :    RptCashBankBook.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    29 SEP 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization
Imports System
Imports System.IO
 
Imports System.Configuration

Imports System.Diagnostics

Imports ClosedXML.Excel
 
Imports System.Net
Imports System.Linq
Imports iTextSharp.text
Imports iTextSharp.text.pdf
#End Region


Partial Class AccountsModule_RptInvoiceBreakup
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim SqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim ObjDate As New clsDateTime
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    Shared divcode As String = ""
    Dim strappid As String = ""
    Dim strappname As String = ""
#End Region
    Dim max As Integer

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            btnPdfReport.Visible = False
            Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim appidnew As String = CType(Request.QueryString("appid"), String)
            strappid = appidnew
            If appidnew Is Nothing = False Then
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                strappname = Session("AppName")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            End If
            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(strappname, String), "AccountsModule\RptBalanceSheet.aspx?appid=" + strappid, btnadd, Button1, btnLoadReprt, gv_SearchResult)
            End If

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            divcode = ViewState("divcode")

            If Page.IsPostBack = False Then
                max = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=507"), String)

                Dim k As ListItem
                For i As Integer = 1 To max
                    k = New ListItem(i, i)
                    ' ddlselect.Items.Add(k)

                Next
                ddlselect.SelectedValue = max
                txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")


                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format(Request.QueryString("fromdate"), "dd/MM/yyyy")
                End If

                If Request.QueryString("level") <> "" Then
                    ddlselect.SelectedValue = Request.QueryString("level")
                End If

                Dim typ As Type
                typ = GetType(DropDownList)

                ddlselect.Visible = True
                Button1.Visible = False
            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepBalSheetWindowPostBack") Then
                btnLoadReprt_Click(sender, e)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("RptCashBankBook.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnLoadReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReprt.Click
        Try


            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strtodate, strlevel As String
            strtodate = Mid(Format(CType(txtFromDate.Text, Date), "dd/MM/YYYY"), 1, 10)
            strlevel = ddlselect.SelectedIndex + 1
            '            Response.Redirect("rptgltrialbalReport.aspx?pagetype=B&todate=" & strtodate & "&level=" & strlevel, False)



            If chknew.Checked = True Then

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand
                mySqlCmd.Connection = mySqlConn
                mySqlCmd.Transaction = sqlTrans

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandText = "sp_Balance"
                mySqlCmd.CommandTimeout = 0
                Dim parms As New List(Of SqlParameter)
                Dim parm(1) As SqlParameter

                parm(0) = New SqlParameter("@date", strtodate)
                parm(1) = New SqlParameter("@division", ViewState("divcode"))

                For i = 0 To 1
                    mySqlCmd.Parameters.Add(parm(i))
                Next
                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()

            End If



            Dim strpop As String = ""
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=B&todate=" & strtodate & "&newformat=" & IIf(chknew.Checked = True, 1, 0) & "&level=" & strlevel & "','RepBalSheet','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('rptgltrialbalReport.aspx?pagetype=B&todate=" & strtodate & "&newformat=" & IIf(chknew.Checked = True, 1, 0) & "&divid=" & divcode & "&level=" & strlevel & "','RepBalSheet');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBalancesheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptBalanceSheet','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim strfromcode, strtocode As Integer

        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        Try
            frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1103")
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

        End Try
    End Function


    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Try

        '    Dim parms As New List(Of SqlParameter)
        '    Dim i As Integer
        '    Dim parm(2) As SqlParameter

        '    Dim strtodate, strlevel As String
        '    strtodate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
        '    strlevel = ddlselect.SelectedValue

        '    parm(0) = New SqlParameter("@todate", strtodate)
        '    parm(1) = New SqlParameter("@div_code", divcode)
        '    parm(2) = New SqlParameter("@level", strlevel)

        '    For i = 0 To 2
        '        parms.Add(parm(i))
        '    Next

        '    Dim ds As New DataSet
        '    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_rep_balancesheet_xls", parms)

        '    If ds.Tables.Count > 0 Then
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            objUtils.ExportToExcel(ds, Response)
        '        End If
        '    Else
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
        '    End If

        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("RptBalancesheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        'End Try

    End Sub




    Protected Sub chknew_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles chknew.CheckedChanged
        If chknew.Checked = True Then
            ddlselect.Visible = True
            ddlselect.Enabled = True
        Else
            ddlselect.Visible = False
            ddlselect.Enabled = False
        End If
    End Sub
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strtodate, strlevel As String
            strtodate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strlevel = ddlselect.SelectedIndex + 1
            If chknew.Checked = True Then

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand
                mySqlCmd.Connection = mySqlConn
                mySqlCmd.Transaction = sqlTrans

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandText = "sp_Balance"
                mySqlCmd.CommandTimeout = 0
                Dim parms As New List(Of SqlParameter)
                Dim parm(1) As SqlParameter

                parm(0) = New SqlParameter("@date", strtodate)
                parm(1) = New SqlParameter("@division", ViewState("divcode"))

                For i = 0 To 1
                    mySqlCmd.Parameters.Add(parm(i))
                Next
                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()

            End If



            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=BalanceSheet&reportsType=pdf&pagetype=B&todate=" & strtodate & "&newformat=" & IIf(chknew.Checked = True, 1, 0) & "&divid=" & divcode & "&level=" & strlevel & "','RepBalSheet');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBalancesheet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub Journalslist()
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strtodate, strlevel As String
            strtodate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strlevel = ddlselect.SelectedIndex + 1
     
            Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), Integer)
            Dim decPlaces As String
            If decno = 3 Then
                decPlaces = "#,##0.000;[Red](#,##0.000)"
            Else
                decPlaces = "#,##0.00;[Red](#,##0.00)"
            End If
            Dim fromdate, todate, rpttype, customer, type, amt, user, rptcompanyname, rptname, custname, filter, rptfilter, filwidth, cols, decimalPoint, DecimalPoints, arrHeaders(), currcode, currDecno As String
            Dim rownum, datetype, reqtype, reporttype, bpax, fcfpax, fcnpax, cnpax, cfpax, fbpax As Integer
            Dim bamt, cfamt, cnamt, fbamt, fcfamt, fcnamt As Decimal
            Dim trow As Integer
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet

            'Dim wb As New XLWorkbook
            'Dim ws = wb.Worksheets.Add("AirportMAPriceListReport")
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            '  mySqlCmd = New SqlCommand("sp_rep_airportmapricelist", sqlConn)
            mySqlCmd = New SqlCommand("sp_get_vatdetails", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = strtodate
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = strtodate
            mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = ViewState("divcode")

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim journalsdt As New DataTable
            journalsdt = ds.Tables(0)
            cols = "P"

            Dim FileNameNew As String = "InvoiceBreakup_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim wb As New XLWorkbook
            Dim ws As IXLWorksheet = wb.Worksheets.Add("Journals")


            If journalsdt.Rows.Count >= 0 Then

                '  If journalsdt.Rows.Count >= 0 Then
                Dim colcount = journalsdt.Columns.Count
                Dim rowcount = journalsdt.Rows.Count
                trow = 5
             
 



                'Report Name Heading
                Dim company = ws.Range("A1:" & cols & "1").Merge()
                ws.Cell("A1").Value = CType(Session("CompanyName"), String)
                company.Style.Fill.SetBackgroundColor(XLColor.FromArgb(192, 192, 192))
                company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 15
                company.Style.Font.FontColor = XLColor.Black
                company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                'Report Name Heading
                Dim company1 = ws.Range("A2:" & cols & "2").Merge()
                ws.Cell("A2").Value = "Invoice Breakup"
                company1.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 14
                company1.Style.Font.FontColor = XLColor.Black
                company1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                company1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                'Report filter Heading
                ws.Cell("A3").Value = " As on Date " & strtodate
                Dim report = ws.Range("A3:" & cols & "3").Merge()
                report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 12
                report.Style.Alignment.SetWrapText().Font.FontColor = XLColor.Black
                trow = trow + 1
                For col As Integer = 0 To journalsdt.Columns.Count - 1
                    ws.Cell(trow, col + 1).Value = journalsdt.Columns(col).ColumnName
                Next

                Dim rowRange = ws.Range(trow, 1, trow, colcount)
                rowRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                rowRange.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                rowRange.Style.Font.SetBold()
                rowRange.Style.Alignment.WrapText = True
                trow += 1 ' Move to the next row for data
           
                ' Iterate over each row in the DataTable
                For Each journalsrows As DataRow In journalsdt.Rows
                    ' Iterate over each column in the DataTable
                    For col As Integer = 0 To journalsdt.Columns.Count - 1
                        ' Write the value from the DataTable to the corresponding cell in the worksheet
                        ws.Cell(trow, col + 1).Value = journalsrows(col)
                    Next

                    Dim rowRangerec = ws.Range(trow, 1, trow, colcount)
                    rowRangerec.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    rowRangerec.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                    rowRangerec.Style.Alignment.WrapText = True
                    trow += 1 ' Move to the next row in the worksheet for the next DataTable row
                Next
                trow += 1
                'Dim amount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(amount)", ""))
                'Dim taxableAmount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(taxableAmount)", ""))
                'Dim vatamount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(vatamount)", ""))
                'Dim grandamount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(grandamount)", ""))
                'Dim baseamount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(baseamount)", ""))
                'Dim basetaxableamount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(basetaxableamount)", ""))
                'Dim basevatamount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(basevatamount)", ""))
                'Dim basegrandamount As Decimal = Convert.ToDecimal(journalsdt.Compute("SUM(basegrandamount)", ""))

                'ws.Cell(trow, 8).Value = "Total"
                'ws.Cell(trow, 9).Value = amount
                'ws.Cell(trow, 11).Value = taxableAmount
                'ws.Cell(trow, 12).Value = vatamount
                'ws.Cell(trow, 13).Value = grandamount
                'ws.Cell(trow, 14).Value = baseamount

                'ws.Cell(trow, 15).Value = basetaxableamount

                'ws.Cell(trow, 16).Value = basevatamount

                'ws.Cell(trow, 17).Value = basegrandamount
                'Dim rowtotal = ws.Range(trow, 1, trow, colcount)
                'rowtotal.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                'rowtotal.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                'rowtotal.Style.Alignment.WrapText = True
                'End If


                'ws.Columns().Width = 15
                ws.Cell((rownum + 4), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
                ws.Range((rownum + 4), 1, (rownum + 4), 2).Merge()

             

                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    wb.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
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

    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try

            Journalslist()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptJournalsList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

End Class

