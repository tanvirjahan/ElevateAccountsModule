Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Globalization
Imports System.Diagnostics

Imports ClosedXML.Excel


Imports System.Collections.Generic

Imports System.Net
Imports System.Linq
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class AccountsModule_ImportBookingXlsExport
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils
    'Dim reportname As String
    'Dim bytes As Byte()
    'Dim objUser As New clsUser
    'Dim strSqlQry As String
    'Dim strWhereCond As String
    'Dim SqlConn As SqlConnection
    'Dim myCommand As SqlCommand
    'Dim myReader As SqlDataReader
    'Dim myDataAdapter As SqlDataAdapter
    'Dim documentWidth As Single = 770.0F

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        If Not Session("GlobalUserName") Is Nothing Then
            Try
                'bytes = {}
                Dim lfileName As String = ""
                lfileName = "importbooking" '+ "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ""
                Dim lOptionToExport As String = Request.QueryString("printId")
                'Response.AddHeader("Content-Disposition", "inline; filename=" + lfileName + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".xlsx")
                'Response.AddHeader("Content-Length", bytes.Length.ToString())
                'Response.ContentType = "application/xls"

                'Dim ldt, dtcurrremarks As New DataTable
                'ldt = lds.Tables(0)
                'Dim cc = ldt.Columns.Count - 7
                'Dim arrHeaders(cc) As String

                'Dim wb As New XLWorkbook
                'Dim ws = wb.Worksheets.Add("Sheet1")
                'ws.Columns.AdjustToContents()
                'Dim colcount = ldt.Columns.Count
                ''ws.Column("A").Width = 50
                ''ws.Columns(2, cc).Width = 20
                'ws.Columns(1, cc).Width = 20
                'Dim rownum As Integer = 1
                'For i = 0 To ldt.Columns.Count - 1
                '    ws.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
                'Next
                'rownum += 1

                'For Each grpserviceRow As DataRow In ldt.Rows
                '    For i = 0 To ldt.Columns.Count - 1
                '        ws.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                '    Next
                'Next

                'Using wStream As New MemoryStream()
                '    wb.SaveAs(wStream)
                '    bytes = wStream.ToArray()
                'End Using

                'Response.Buffer = True
                'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                'Response.BinaryWrite(bytes)
                'Response.Flush()
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
                'Dim lDataTable As DataTable
                'If lOptionToExport.ToLower = "newbooking" Then
                '    If Session("dtImportBooking") Is Nothing Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
                '        Exit Sub
                '    Else
                '        lDataTable = CType(Session("dtImportBooking"), DataTable).Copy
                '    End If
                'End If

                'If lOptionToExport.ToLower = "amendbooking" Then
                '    If Session("dtAmendedBooking") Is Nothing Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
                '        Exit Sub
                '    Else
                '        lDataTable = CType(Session("dtAmendedBooking"), DataTable).Copy
                '    End If
                'End If

                'If lOptionToExport.ToLower = "cancelbooking" Then
                '    If Session("dtCancelBooking") Is Nothing Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
                '        Exit Sub
                '    Else
                '        lDataTable = CType(Session("dtCancelBooking"), DataTable).Copy
                '    End If
                'End If

                Dim lDataset As New DataSet
                'lDataset.Tables.Add(lDataTable)
                ''Dim clsUtilities1 As New clsUtils
                ''clsUtilities1.ExportToExcel(lDataset, Response)
                ExcelExportService(lfileName, lDataset)

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ImportBookingXlsExport.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Response.Redirect("Login.aspx", True)
        End If

    End Sub
    'Function lfnDataTable(ByVal lOptionToExport As String) As DataTable
    '    Dim lDataTable As DataTable
    '    If lOptionToExport.ToLower = "newbooking" Then
    '        If Session("dtImportBooking") Is Nothing Then
    '            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
    '            'Exit Function
    '            Return Nothing
    '        Else
    '            lDataTable = CType(Session("dtImportBooking"), DataTable).Copy
    '        End If
    '    End If

    '    If lOptionToExport.ToLower = "amendbooking" Then
    '        If Session("dtAmendedBooking") Is Nothing Then
    '            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
    '            'Exit Function
    '            Return Nothing
    '        Else
    '            lDataTable = CType(Session("dtAmendedBooking"), DataTable).Copy
    '        End If
    '    End If

    '    If lOptionToExport.ToLower = "cancelbooking" Then
    '        If Session("dtCancelBooking") Is Nothing Then
    '            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
    '            'Exit Function
    '            Return Nothing
    '        Else
    '            lDataTable = CType(Session("dtCancelBooking"), DataTable).Copy
    '        End If
    '    End If

    '    If lOptionToExport.ToLower = "importedbooking" Then
    '        If Session("dtImportedBooking") Is Nothing Then
    '            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
    '            'Exit Function
    '            Return Nothing
    '        Else
    '            lDataTable = CType(Session("dtImportedBooking"), DataTable).Copy
    '        End If
    '    End If

    '    Return lDataTable
    'End Function


    Function lfnDataTable(ByVal lOptionToExport As String) As DataTable
        Dim lDataTable As DataTable
        If lOptionToExport.ToLower = "newbooking" Then
            If Session("dtImportBooking") Is Nothing Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
                'Exit Function
                Return Nothing
            Else
                lDataTable = CType(Session("dtImportBooking"), DataTable).Copy
            End If
        End If

        If lOptionToExport.ToLower = "amendbooking" Then
            If Session("dtAmendedBooking") Is Nothing Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
                'Exit Function
                Return Nothing
            Else
                lDataTable = CType(Session("dtAmendedBooking"), DataTable).Copy
            End If
        End If

        If lOptionToExport.ToLower = "cancelbooking" Then
            If Session("dtCancelBooking") Is Nothing Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
                'Exit Function
                Return Nothing
            Else
                lDataTable = CType(Session("dtCancelBooking"), DataTable).Copy
            End If
        End If

        If lOptionToExport.ToLower = "importedbooking" Then
            If Session("dtImportedBooking") Is Nothing Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No data found to export to excel');", True)
                'Exit Function
                Return Nothing
            Else
                lDataTable = CType(Session("dtImportedBooking"), DataTable).Copy
            End If
        End If

        If lOptionToExport.ToLower = "mismatchpi" Then
            If Session("dtPIMismatchBooking") Is Nothing Then
                Return Nothing
            Else
                lDataTable = CType(Session("dtPIMismatchBooking"), DataTable).Copy
            End If
        End If

        If lOptionToExport.ToLower = "ignoredbookings" Then
            If Session("dtIgnoreList") Is Nothing Then
                Return Nothing
            Else
                lDataTable = CType(Session("dtIgnoreList"), DataTable).Copy
            End If
        End If

        If lOptionToExport.ToLower = "reconcilliationlist" Then
            If Session("ReconcileList") Is Nothing Then
                Return Nothing
            Else
                lDataTable = CType(Session("ReconcileList"), DataTable).Copy
            End If
        End If

        If lOptionToExport.ToLower = "reconcilliationsummary" Then
            If Session("dtreconSummary") Is Nothing Then
                Return Nothing
            Else
                lDataTable = CType(Session("dtreconSummary"), DataTable).Copy
            End If
        End If


        Return lDataTable
    End Function


    Sub ExcelExportService(ByVal lfileName As String, ByVal lds As DataSet)
        Dim bytes As Byte()
        bytes = {}



        Dim ldt As New DataTable
        ldt = lfnDataTable("newbooking") 'lds.Tables(0)
        Dim cc As Integer '= ldt.Columns.Count - 7
        'Dim arrHeaders(cc) As String

        Dim wb As New XLWorkbook

        Dim rownum As Integer = 1

        'New Booking
        Dim ws = wb.Worksheets.Add("newbooking")
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            ws.Columns.AdjustToContents()
            Dim colcount = ldt.Columns.Count
            'ws.Column("A").Width = 50
            'ws.Columns(2, cc).Width = 20
            ws.Columns(1, cc).Width = 20

            For i = 0 To ldt.Columns.Count - 1
                ws.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        ws.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        ws.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If
        

        'Amend Booking
        ldt = lfnDataTable("amendbooking") 'lds.Tables(0)
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            Dim wsAmend = wb.Worksheets.Add("amendbooking")
            wsAmend.Columns.AdjustToContents()
            wsAmend.Columns(1, cc).Width = 20
            rownum = 1
            For i = 0 To ldt.Columns.Count - 1
                wsAmend.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        wsAmend.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        wsAmend.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If

        'Cancel Booking
        ldt = lfnDataTable("cancelbooking") 'lds.Tables(0)
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            Dim wsCancel = wb.Worksheets.Add("cancelbooking")
            wsCancel.Columns.AdjustToContents()
            wsCancel.Columns(1, cc).Width = 20
            rownum = 1
            For i = 0 To ldt.Columns.Count - 1
                wsCancel.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        wsCancel.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        wsCancel.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If
       
        'Imported Booking
        ldt = lfnDataTable("importedbooking") 'lds.Tables(0)
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            Dim wsImported = wb.Worksheets.Add("importedbooking")
            wsImported.Columns.AdjustToContents()
            wsImported.Columns(1, cc).Width = 20
            rownum = 1
            For i = 0 To ldt.Columns.Count - 1
                wsImported.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        wsImported.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        wsImported.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If

        ' ## rosalin 09/10/2023

        'mismatch Booking
        ldt = lfnDataTable("mismatchpi") 'lds.Tables(0)
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            Dim wsmatch = wb.Worksheets.Add("mismatchpi")
            wsmatch.Columns.AdjustToContents()
            wsmatch.Columns(1, cc).Width = 20
            rownum = 1
            For i = 0 To ldt.Columns.Count - 1
                wsmatch.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        wsmatch.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        wsmatch.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If

        'ignored Booking
        ldt = lfnDataTable("ignoredbookings") 'lds.Tables(0)
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            Dim wsignored = wb.Worksheets.Add("ignoredbookings")
            wsignored.Columns.AdjustToContents()
            wsignored.Columns(1, cc).Width = 20
            rownum = 1
            For i = 0 To ldt.Columns.Count - 1
                wsignored.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        wsignored.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        wsignored.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If

        'Reconcilliation Booking
        ldt = lfnDataTable("reconcilliationlist") 'lds.Tables(0)
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            Dim wsrecon = wb.Worksheets.Add("reconcilliationlist")
            wsrecon.Columns.AdjustToContents()
            wsrecon.Columns(1, cc).Width = 20
            rownum = 1
            For i = 0 To ldt.Columns.Count - 1
                wsrecon.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        wsrecon.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        wsrecon.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If

        'Reconcilliation Summary Booking
        ldt = lfnDataTable("reconcilliationsummary") 'lds.Tables(0)
        If ldt IsNot Nothing Then
            cc = ldt.Columns.Count - 1
            Dim wsreconSum = wb.Worksheets.Add("reconcilliationsummary")
            wsreconSum.Columns.AdjustToContents()
            wsreconSum.Columns(1, cc).Width = 20
            rownum = 1
            For i = 0 To ldt.Columns.Count - 1
                wsreconSum.Cell(rownum, i + 1).Value = ldt.Columns(i).Caption
            Next
            rownum += 1

            For Each grpserviceRow As DataRow In ldt.Rows
                For i = 0 To ldt.Columns.Count - 1
                    If ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "salesprice" Or ldt.Columns(i).Caption.Trim.Replace(" ", "").ToLower() = "costprice" Then
                        wsreconSum.Cell(rownum, i + 1).Value = grpserviceRow(i).ToString()
                    Else
                        wsreconSum.Cell(rownum, i + 1).Value = IIf(ldt.Columns(i).DataType Is GetType(String), "'", "") & grpserviceRow(i).ToString()
                    End If
                Next
                rownum += 1
            Next
        End If

        ' ## End


        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using

        Response.AddHeader("Content-Disposition", "inline; filename=" + lfileName + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".xls")
        Response.AddHeader("Content-Length", bytes.Length.ToString())
        Response.ContentType = "application/xls"

        Response.Buffer = True
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.BinaryWrite(bytes)
        Response.Flush()
        HttpContext.Current.ApplicationInstance.CompleteRequest()

    End Sub

End Class
