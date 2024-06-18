Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports System.Linq
Imports ClosedXML.Excel

Public Class ClsCustomerAgeingPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim smallfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim normalfont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    Dim Footerfont As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim addrLine1, tran_typevalue, selectedgroup, headerarray(), currency, currcode, supplier, customer, addrLine2, addrLine3, addrLine4, addrLine5, pdc, common, one, two, three, four, five, six, seven, format1 As String
    Dim tage1, tage2, tage3, tage4, tage5, tage6, tage7, tage9, tbal, fage1, fage2, fage3, fage4, fage5, fage6, fage7, fage9, fbal As Decimal
    Dim currDecno, rownum, rowsc As Integer
    Dim Rowtitlebg As BaseColor = New BaseColor(192, 192, 192)
    Dim totalrow, total As DataRow
#End Region

#Region "Procedure parameter"
    Property tran_type() As String
        Get
            Return tran_typevalue
        End Get
        Set(ByVal value As String)
            tran_typevalue = value
        End Set
    End Property
#End Region

#Region "Private Shared Function PhraseCell(phrase As Phrase, align As Integer, Cols As Integer, celBorder As Boolean, Optional celBottomBorder As String = ""None"") As PdfPCell"
    Private Shared Function PhraseCell(ByVal phrase As Phrase, ByVal align As Integer, ByVal Cols As Integer, ByVal celBorder As Boolean, Optional ByVal celBottomBorder As String = "None") As PdfPCell
        Dim cell As New PdfPCell(phrase)
        If Cols > 1 Then cell.Colspan = Cols
        If celBorder Then
            If celBottomBorder <> "None" Then
                If celBottomBorder = "No" Then
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                    cell.BorderColor = BaseColor.BLACK
                Else
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.BorderColor = BaseColor.BLACK
                End If
            Else
                cell.BorderColor = BaseColor.BLACK
            End If
        Else
            cell.Border = Rectangle.NO_BORDER
        End If
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.5F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

    Private Sub PrintRow(ByVal row As DataRow, ByVal DataTable As PdfPTable, Optional ByVal count As Integer = 0)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        If rowsc = 0 Or rowsc = 1 Or count = 1 Then
            smallfont = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
        Else
            smallfont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
        End If
        currDecno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currcode & "'"), Integer)
        phrase = New Phrase()
        phrase.Add(New Chunk(row("acc_code").ToString(), smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        If rowsc = 0 Or rowsc = 1 Or count = 1 Then
            cell.BackgroundColor = Rowtitlebg
            cell.Colspan = 2
        End If
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        If rowsc <> 0 AndAlso rowsc <> 1 AndAlso count <> 1 Then
            phrase = New Phrase()
            phrase.Add(New Chunk(row("agentname").ToString(), smallfont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 8.0F
            'cell.BackgroundColor = Rowtitlebg
            cell.PaddingTop = 1.0F
            DataTable.AddCell(cell)
        End If

        phrase = New Phrase()
        Dim creditlimit As String = IIf(row("creditlimit") = 0, " ", row("creditlimit"))
        phrase.Add(New Chunk(creditlimit, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age9 As String = IIf(row("age9") = 0, "", Decimal.Parse(row("age9")).ToString("N" + currDecno.ToString))
        phrase.Add(New Chunk(age9, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age1 As String = IIf(row("age1") = 0, " ", FormatNumber(row("age1"), currDecno, UseParensForNegativeNumbers:=TriState.True))
        phrase.Add(New Chunk(age1, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age2 As String = IIf(row("age2") = 0, " ", FormatNumber(row("age2"), currDecno, UseParensForNegativeNumbers:=TriState.True))
        phrase.Add(New Chunk(age2, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age3 As String = IIf(row("age3") = 0, " ", FormatNumber(row("age3"), currDecno, UseParensForNegativeNumbers:=TriState.True))
        phrase.Add(New Chunk(age3, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age4 As String = IIf(row("age4") = 0, " ", FormatNumber(row("age4"), currDecno, UseParensForNegativeNumbers:=TriState.True))
        phrase.Add(New Chunk(age4, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age5 As String = IIf(row("age5") = 0, " ", FormatNumber(row("age5"), currDecno, UseParensForNegativeNumbers:=TriState.True))
        phrase.Add(New Chunk(age5, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age6 As String = IIf(row("age6") = 0, " ", FormatNumber(row("age6"), currDecno, UseParensForNegativeNumbers:=TriState.True))
        phrase.Add(New Chunk(age6, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()
        Dim age7 As String = IIf(row("age7") = 0, " ", FormatNumber(row("age7"), currDecno, UseParensForNegativeNumbers:=TriState.True))
        phrase.Add(New Chunk(age7, smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)

        phrase = New Phrase()

        phrase.Add(New Chunk(FormatNumber(row("balance"), currDecno, UseParensForNegativeNumbers:=TriState.True), smallfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingBottom = 8.0F
        cell.PaddingTop = 1.0F
        DataTable.AddCell(cell)
    End Sub

    Private Sub PrintExcelRow(ByVal row As DataRow, ByVal ws As IXLWorksheet, Optional ByVal count As Integer = 0)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        Dim format As String = Nothing

        'sharfudeen 11/09/2022
        Dim positiveFormat As String = ""
        Dim negativeFormat As String = ""
        Dim zeroFormat As String = ""
        Dim numberFormat As String = ""
        Dim fullNumberFormat As String = ""
        Dim roundpositiveFormat As String = ""
        Dim roundnegativeFormat As String = ""
        Dim roundnumberFormat As String = ""


        currDecno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & row("currcode") & "'"), Integer)

        If currDecno = 2 Then
            format = "####,##,##0.00"
            format1 = "(####,##,##0.00)"


            'sharfudeen 11/09/2022
            positiveFormat = "#,##0.00_)"
            negativeFormat = "(#,##0.00)"
            zeroFormat = "-_)"
            numberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat + " ;@"

            roundpositiveFormat = "#,##0_)"
            roundnegativeFormat = "(#,##0)"
            roundnumberFormat = roundpositiveFormat + ";" + roundnegativeFormat + ";" + zeroFormat + ";@"

        ElseIf currDecno = 3 Then
            format = "####,##,##0.000"
            format1 = "(####,##,##0.000)"
        ElseIf currDecno = 4 Then
            format = "####,##,##0.0000"
            format1 = "(####,##,##0.0000)"
        Else
            format = "####,##,##0.000"
            format1 = "(####,##,##0.000)"
        End If

        Dim decpt As String = "N" + currDecno.ToString()
        rownum = rownum + 1
        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.FontSize = 9
        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
        If rowsc = 0 Or rowsc = 1 Or count = 1 Then
            ws.Range(rownum, 1, rownum, 3).Style.Fill.SetBackgroundColor(XLColor.Gray)
            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold()
        End If
        ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        Dim acc As String = Space(1) & row("acc_code") & Space(1) & "."
        ws.Cell(rownum, 1).Value = acc
        ws.Cell(rownum, 1).SetDataType(XLCellValues.Text)

        ws.Cell(rownum, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        ws.Cell(rownum, 2).Value = row("agentname").ToString()

        ws.Cell(rownum, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

        If row("creditlimit") = 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 3).Value = ""
            ws.Cell(rownum, 3).Value = Decimal.Parse(row("creditlimit")).ToString(decpt)
            ws.Cell(rownum, 3).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022
        ElseIf row("creditlimit") < 0 Then
            'sharfudeen 11/09/2022
            '   ws.Cell(rownum, 3).Value = Decimal.Parse(Math.Abs(row("creditlimit"))).ToString(decpt)
            ws.Cell(rownum, 3).Value = Decimal.Parse(row("creditlimit")).ToString(decpt)
            ws.Cell(rownum, 3).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022
        Else
            ws.Cell(rownum, 3).Value = Decimal.Parse(Decimal.Parse(row("creditlimit")).ToString(decpt))
            ws.Cell(rownum, 3).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022
        End If
        'ws.Cell(rownum, 4).Value = IIf(row("creditlimit") = 0, " ", Decimal.Parse(row("creditlimit")).ToString(decpt))

        'If IIf(row("creditlimit") = 0, " ", Decimal.Parse(row("creditlimit")).ToString(decpt)) <> " " Then
        '    ws.Cell(rownum, 4).Style.NumberFormat.Format = format
        'End If
        ws.Cell(rownum, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

        If row("age9") = 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 4).Value = ""
            ws.Cell(rownum, 4).Value = Decimal.Parse(Decimal.Parse(row("age9")).ToString(decpt))
            ws.Cell(rownum, 4).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022
        ElseIf row("age9") < 0 Then
            'sharfudeen 11/09/2022
            '  ws.Cell(rownum, 4).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("age9"))).ToString(decpt))
            ws.Cell(rownum, 4).Value = Decimal.Parse(Decimal.Parse(row("age9")).ToString(decpt))
            ws.Cell(rownum, 4).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022
        Else
            ws.Cell(rownum, 4).Value = Decimal.Parse(Decimal.Parse(row("age9")).ToString(decpt))
            ws.Cell(rownum, 4).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If


        ws.Cell(rownum, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

        If row("age1") = 0 Then
            'sharfudeen 11/09/2022
            'ws.Cell(rownum, 5).Value = ""
            ws.Cell(rownum, 5).Value = Decimal.Parse(row("age1")).ToString(decpt)
            ws.Cell(rownum, 5).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("age1") < 0 Then
            'sharfudeen 11/09/2022
            '  Dim age1 = Decimal.Parse(Math.Abs(row("age1"))).ToString(decpt)
            Dim age1 = Decimal.Parse(row("age1")).ToString(decpt)
            ws.Cell(rownum, 5).Value = Decimal.Parse(age1)
            ws.Cell(rownum, 5).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 5).Value = Decimal.Parse(Decimal.Parse(row("age1")).ToString(decpt))
            ws.Cell(rownum, 5).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If

        ws.Cell(rownum, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        'ws.Cell(rownum, 7).Value = IIf(row("age2") = 0, " ", Decimal.Parse(row("age2")).ToString(decpt))
        If row("age2") = 0 Then
            'sharfudeen 11/09/2022
            'ws.Cell(rownum, 6).Value = ""
            ws.Cell(rownum, 6).Value = Decimal.Parse(Decimal.Parse(row("age2")).ToString(decpt))
            ws.Cell(rownum, 6).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("age2") < 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 6).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("age2"))).ToString(decpt))
            ws.Cell(rownum, 6).Value = Decimal.Parse(Decimal.Parse(row("age2")).ToString(decpt))
            ws.Cell(rownum, 6).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 6).Value = Decimal.Parse(Decimal.Parse(row("age2")).ToString(decpt))
            ws.Cell(rownum, 6).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If
        ws.Cell(rownum, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

        If row("age3") = 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 7).Value = ""
            ws.Cell(rownum, 7).Value = Decimal.Parse(Decimal.Parse(row("age3")).ToString(decpt))
            ws.Cell(rownum, 7).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("age3") < 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 7).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("age3"))).ToString(decpt))
            ws.Cell(rownum, 7).Value = Decimal.Parse(Decimal.Parse(row("age3")).ToString(decpt))
            ws.Cell(rownum, 7).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 7).Value = Decimal.Parse(Decimal.Parse(row("age3")).ToString(decpt))
            ws.Cell(rownum, 7).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If
        ws.Cell(rownum, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ' ws.Cell(rownum, 9).Value = IIf(row("age4") = 0, " ", Decimal.Parse(row("age4")).ToString(decpt))
        If row("age4") = 0 Then
            'sharfudeen 11/09/2022
            '   ws.Cell(rownum, 8).Value = ""
            ws.Cell(rownum, 8).Value = Decimal.Parse(Decimal.Parse(row("age4")).ToString(decpt))
            ws.Cell(rownum, 8).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("age4") < 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 8).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("age4"))).ToString(decpt))
            ws.Cell(rownum, 8).Value = Decimal.Parse(Decimal.Parse(row("age4")).ToString(decpt))
            ws.Cell(rownum, 8).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 8).Value = Decimal.Parse(Decimal.Parse(row("age4")).ToString(decpt))
            ws.Cell(rownum, 8).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If
        ws.Cell(rownum, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ' ws.Cell(rownum, 10).Value = IIf(row("age5") = 0, " ", Decimal.Parse(row("age5")).ToString(decpt))
        If row("age5") = 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 9).Value = ""
            ws.Cell(rownum, 9).Value = Decimal.Parse(Decimal.Parse(row("age5")).ToString(decpt))
            ws.Cell(rownum, 9).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("age5") < 0 Then
            'sharfudeen 11/09/2022
            'ws.Cell(rownum, 9).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("age5"))).ToString(decpt))
            ws.Cell(rownum, 9).Value = Decimal.Parse(Decimal.Parse(row("age5")).ToString(decpt))
            ws.Cell(rownum, 9).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 9).Value = Decimal.Parse(Decimal.Parse(row("age5")).ToString(decpt))
            ws.Cell(rownum, 9).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If
        ws.Cell(rownum, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ' ws.Cell(rownum, 11).Value = IIf(row("age6") = 0, " ", Decimal.Parse(row("age6")).ToString(decpt))
        If row("age6") = 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 10).Value = ""
            ws.Cell(rownum, 10).Value = Decimal.Parse(Decimal.Parse(row("age6")).ToString(decpt))
            ws.Cell(rownum, 10).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("age6") < 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 10).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("age6"))).ToString(decpt))
            ws.Cell(rownum, 10).Value = Decimal.Parse(Decimal.Parse(row("age6")).ToString(decpt))
            ws.Cell(rownum, 10).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 10).Value = Decimal.Parse(Decimal.Parse(row("age6")).ToString(decpt))
            ws.Cell(rownum, 10).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If
        ws.Cell(rownum, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ' ws.Cell(rownum, 12).Value = IIf(row("age7") = 0, " ", Decimal.Parse(row("age7")).ToString(decpt))
        If row("age7") = 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 11).Value = ""
            ws.Cell(rownum, 11).Value = Decimal.Parse(Decimal.Parse(row("age7")).ToString(decpt))
            ws.Cell(rownum, 11).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("age7") < 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 11).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("age7"))).ToString(decpt))
            ws.Cell(rownum, 11).Value = Decimal.Parse(Decimal.Parse(row("age7")).ToString(decpt))
            ws.Cell(rownum, 11).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 11).Value = Decimal.Parse(Decimal.Parse(row("age7")).ToString(decpt))
            ws.Cell(rownum, 11).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If
        ws.Cell(rownum, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ' ws.Cell(rownum, 12).Value = Decimal.Parse(row("balance")).ToString(decpt)

        If row("balance") = 0 Then
            'sharfudeen 11/09/2022
            ' ws.Cell(rownum, 12).Value = ""
            ws.Cell(rownum, 12).Value = Decimal.Parse(Decimal.Parse(row("balance")).ToString(decpt))
            ws.Cell(rownum, 12).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        ElseIf row("balance") < 0 Then
            'sharfudeen 11/09/2022
            '  ws.Cell(rownum, 12).Value = Decimal.Parse(Decimal.Parse(Math.Abs(row("balance"))).ToString(decpt))
            ws.Cell(rownum, 12).Value = Decimal.Parse(Decimal.Parse(row("balance")).ToString(decpt))
            ws.Cell(rownum, 12).Style.NumberFormat.Format = numberFormat ' format1 'sharfudeen 11/09/2022 
        Else
            ws.Cell(rownum, 12).Value = Decimal.Parse(Decimal.Parse(row("balance")).ToString(decpt))
            ws.Cell(rownum, 12).Style.NumberFormat.Format = numberFormat ' format 'sharfudeen 11/09/2022 
        End If
        '  ws.Cell(rownum, 13).Style.NumberFormat.Format = format
    End Sub

#Region "ExcelReport"
    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal reportfilter As String, ByVal currency As String, ByVal rptreportname As String, ByVal todate As String, ByRef bytes() As Byte)
        Dim arrHeaders() As String
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add(rptreportname)
        ws.Columns.AdjustToContents()
        ws.Columns("A").Width = 15
        ws.Columns("B").Width = 20
        ws.Columns("C:L").Width = 10
        rownum = 2
        Dim rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)

        ws.Cell("A1").Value = rptcompanyname
        Dim companyname = ws.Range("A1:L1").Merge()
        companyname.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        companyname.Style.Font.SetBold().Font.FontSize = 15
        companyname.Style.Font.FontColor = XLColor.Black
        companyname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        companyname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        'Report Name Heading
        ws.Cell("A2").Value = rptreportname
        Dim report = ws.Range("A2:L2").Merge()
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        report.Style.Font.SetBold().Font.FontSize = 13
        report.Style.Font.FontColor = XLColor.Black
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        Dim currList = (From n In custdetailsdt.AsEnumerable Group By currcode = n.Field(Of String)("currcode") Into grp = Group Order By currcode Select New With {.currcode = currcode}).ToList()

        If currList.Count > 0 Then
            For j = 0 To currList.Count - 1
                rownum = rownum + 2
                fbal = 0
                fage1 = 0
                fage2 = 0
                fage3 = 0
                fage4 = 0
                fage5 = 0
                fage6 = 0
                fage7 = 0
                fage9 = 0

                Dim tmpcurrcode As String = currList(j).currcode
                Dim currwiseDt As DataTable = (From n In custdetailsdt.AsEnumerable Where n.Field(Of String)("currcode") = tmpcurrcode Select n).CopyToDataTable()

                total = currwiseDt.NewRow()
                totalrow = currwiseDt.NewRow()
                totalrow("acc_code") = "Total in " + tmpcurrcode
                totalrow("agentname") = ""
                totalrow("creditlimit") = 0
                If currwiseDt.Rows.Count > 0 Then

                    totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                    totalrow("age9") = currwiseDt.Compute("Sum(age9)", String.Empty)
                    totalrow("age1") = currwiseDt.Compute("Sum(age1)", String.Empty)
                    totalrow("age2") = currwiseDt.Compute("Sum(age2)", String.Empty)
                    totalrow("age3") = currwiseDt.Compute("Sum(age3)", String.Empty)
                    totalrow("age4") = currwiseDt.Compute("Sum(age4)", String.Empty)
                    totalrow("age5") = currwiseDt.Compute("Sum(age5)", String.Empty)
                    totalrow("age6") = currwiseDt.Compute("Sum(age6)", String.Empty)
                    totalrow("age7") = currwiseDt.Compute("sum(age7)", String.Empty)
                    totalrow("balance") = currwiseDt.Compute("sum(balance)", String.Empty)
                Else
                    totalrow("age9") = 0
                    totalrow("age1") = 0
                    totalrow("age2") = 0
                    totalrow("age3") = 0
                    totalrow("age4") = 0
                    totalrow("age5") = 0
                    totalrow("age6") = 0
                    totalrow("age7") = 0
                    totalrow("balance") = 0
                End If
                total("acc_code") = "Final Total"
                total("agentname") = ""
                total("creditlimit") = 0
                If currwiseDt.Rows.Count > 0 Then

                    total("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                    total("age9") = currwiseDt.Compute("Sum(age9)", String.Empty)
                    total("age1") = currwiseDt.Compute("Sum(age1)", String.Empty)
                    total("age2") = currwiseDt.Compute("Sum(age2)", String.Empty)
                    total("age3") = currwiseDt.Compute("Sum(age3)", String.Empty)
                    total("age4") = currwiseDt.Compute("Sum(age4)", String.Empty)
                    total("age5") = currwiseDt.Compute("Sum(age5)", String.Empty)
                    total("age6") = currwiseDt.Compute("Sum(age6)", String.Empty)
                    total("age7") = currwiseDt.Compute("sum(age7)", String.Empty)
                    total("balance") = currwiseDt.Compute("sum(balance)", String.Empty)
                Else
                    total("age9") = 0
                    total("age1") = 0
                    total("age2") = 0
                    total("age3") = 0
                    total("age4") = 0
                    total("age5") = 0
                    total("age6") = 0
                    total("age7") = 0
                    total("balance") = 0
                End If
                currwiseDt.Rows.Add(totalrow)
                'currwiseDt.Rows.Add(total) 'final total

                Dim strdateline As String = "A" + rownum.ToString() + ":L" + rownum.ToString()
                Dim rfilter = ws.Range(strdateline).Merge()
                rfilter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
                rfilter.Style.Font.SetBold().Font.FontSize = 12
                rfilter.Style.Font.FontColor = XLColor.Black
                rfilter.Cell(1, 1).Value = "As on Date : " + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Space(15) + currency + " - " + tmpcurrcode + Space(15) + supplier + Space(6) + customer

                rownum = rownum + 1
                Dim strfilter As String = "A" + rownum.ToString() + ":L" + rownum.ToString()
                Dim filter = ws.Range(strfilter).Merge()
                filter.Style.Font.SetBold().Font.FontSize = 12
                filter.Style.Font.FontColor = XLColor.Black
                filter.Cell(1, 1).Value = reportfilter
                ' Dim le = reportfilter.Length
                ' ws.Row(4).AdjustToContents()
                Dim rowheight As Integer

                If reportfilter.Length > 100 Then
                    rowheight = IIf(reportfilter.Length > 100 And reportfilter.Length < 200, 32, IIf(reportfilter.Length > 200 And reportfilter.Length < 300, 48, 64))
                    ws.Row(rownum).Height = rowheight
                End If

                filter.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

                rownum = rownum + 1

                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
                For i = 0 To headerarray.Length - 1
                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Cell(rownum, i + 1).Value = headerarray(i)
                Next

                If currwiseDt.Rows.Count > 0 Then
                    If common <> "acc_code" Then

                        Dim customerdetails = From custdetails In currwiseDt.AsEnumerable() Group custdetails By GroupName = New With {Key .GroupName = custdetails.Field(Of String)(common)} Into Group

                        For Each Groupnameslist In customerdetails
                            If Groupnameslist.GroupName.GroupName <> "" Then '  Groupnameslist.Group.Count

                                rownum = rownum + 1
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True

                                ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                ws.Cell(rownum, 1).Value = selectedgroup.ToString() & " - " & Groupnameslist.GroupName.GroupName
                                ws.Range("A" & rownum & ":L" & rownum).Merge()
                                ws.Range("A" & rownum & ":L" & rownum).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                                rowsc = Groupnameslist.Group.Count + 2

                                For Each row In Groupnameslist.Group
                                    rowsc = rowsc - 1
                                    tage1 = tage1 + row("age1")
                                    tage2 = tage2 + row("age2")
                                    tage3 = tage3 + row("age3")
                                    tage4 = tage4 + row("age4")
                                    tage5 = tage5 + row("age5")
                                    tage6 = tage6 + row("age6")
                                    tage9 = tage9 + row("age9")
                                    tage7 = tage7 + row("age7")
                                    tbal = tbal + row("balance")
                                    PrintExcelRow(row, ws)
                                Next
                                fage1 = fage1 + tage1
                                fage2 = fage2 + tage2
                                fage3 = fage3 + tage3
                                fage4 = fage4 + tage4
                                fage5 = fage5 + tage5
                                fage6 = fage6 + tage6
                                fage9 = fage9 + tage9
                                fage7 = fage7 + tage7
                                fbal = fbal + tbal
                                totalrow("acc_code") = "Total For " + Groupnameslist.GroupName.GroupName
                                totalrow("agentname") = ""
                                totalrow("creditlimit") = 0
                                totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                                totalrow("age9") = tage9
                                totalrow("age1") = tage1
                                totalrow("age2") = tage2
                                totalrow("age3") = tage3
                                totalrow("age4") = tage4
                                totalrow("age5") = tage5
                                totalrow("age6") = tage6
                                totalrow("age7") = tage7
                                totalrow("balance") = tbal
                                PrintExcelRow(totalrow, ws, 1)
                                tage1 = 0
                                tage2 = 0
                                tage3 = 0
                                tage4 = 0
                                tage5 = 0
                                tage6 = 0
                                tage9 = 0
                                tage7 = 0
                                tbal = 0
                            End If
                        Next
                        totalrow("acc_code") = "Total in " + tmpcurrcode
                        totalrow("agentname") = ""
                        totalrow("creditlimit") = 0
                        totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                        totalrow("age9") = fage9
                        totalrow("age1") = fage1
                        totalrow("age2") = fage2
                        totalrow("age3") = fage3
                        totalrow("age4") = fage4
                        totalrow("age5") = fage5
                        totalrow("age6") = fage6
                        totalrow("age7") = fage7
                        totalrow("balance") = fbal
                        rowsc = rowsc - 1
                        PrintExcelRow(totalrow, ws)
                        'totalrow("acc_code") = "Final Total"
                        'totalrow("agentname") = ""
                        'totalrow("creditlimit") = 0
                        'totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                        'totalrow("age9") = fage9
                        'totalrow("age1") = fage1
                        'totalrow("age2") = fage2
                        'totalrow("age3") = fage3
                        'totalrow("age4") = fage4
                        'totalrow("age5") = fage5
                        'totalrow("age6") = fage6
                        'totalrow("age7") = fage7
                        'totalrow("balance") = fbal
                        'rowsc = rowsc - 1
                        'PrintExcelRow(totalrow, ws)
                    Else
                        rowsc = currwiseDt.Rows.Count + 1
                        For Each row In currwiseDt.Rows
                            rowsc = rowsc - 1
                            PrintExcelRow(row, ws)
                        Next
                    End If

                End If
            Next
        End If


        ws.Cell((rownum + 2), 1).Value = addrLine3 & addrLine5
        ws.Range((rownum + 2), 1, (rownum + 2), 5).Merge()
        ws.Cell((rownum + 3), 1).Value = addrLine1
        ws.Range((rownum + 3), 1, (rownum + 3), 5).Merge()
        ws.Cell((rownum + 4), 1).Value = addrLine2 & "  " & addrLine4
        ws.Range((rownum + 4), 1, (rownum + 4), 5).Merge()
        ws.Cell((rownum + 5), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
        ws.Range((rownum + 5), 1, (rownum + 5), 5).Merge()

        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using

    End Sub

#End Region

#Region "GenerateReport"

    Public Sub GenerateReport(ByVal todate As String, ByVal Type As String, ByVal reportfilter As String, ByVal currflg As Integer, ByVal reportname As String, ByVal reportsType As String, ByVal fromacct As String, ByVal toacct As String, ByVal fromcontrol As String, ByVal tocontrol As String,
                         ByVal fromcat As String, ByVal tocat As String, ByVal fromcity As String, ByVal tocity As String, ByVal fromctry As String, ByVal toctry As String,
     ByVal agingtype As Integer, ByVal summdet As Integer, ByVal web As Integer, ByVal custtype As Integer, ByVal divcode As String, ByVal custgroup_sp_type As String, ByVal inclproforma As Integer, ByVal currcodefilter As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, ByVal orderby As String, ByVal groupby As String, Optional ByVal fileName As String = "")

        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet

            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_statement_partyaging_summdet", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Char)).Value = Type
            mySqlCmd.Parameters.Add(New SqlParameter("@currflg", SqlDbType.Int)).Value = currflg
            mySqlCmd.Parameters.Add(New SqlParameter("@fromacct", SqlDbType.VarChar, 20)).Value = fromacct
            mySqlCmd.Parameters.Add(New SqlParameter("@toacct", SqlDbType.VarChar, 20)).Value = toacct
            mySqlCmd.Parameters.Add(New SqlParameter("@fromcontrol", SqlDbType.VarChar, 20)).Value = fromcontrol
            mySqlCmd.Parameters.Add(New SqlParameter("@tocontrol", SqlDbType.VarChar, 20)).Value = tocontrol
            mySqlCmd.Parameters.Add(New SqlParameter("@fromcat", SqlDbType.VarChar, 20)).Value = fromcat
            mySqlCmd.Parameters.Add(New SqlParameter("@tocat", SqlDbType.VarChar, 20)).Value = tocat
            mySqlCmd.Parameters.Add(New SqlParameter("@fromcity", SqlDbType.VarChar, 20)).Value = fromcity
            mySqlCmd.Parameters.Add(New SqlParameter("@tocity", SqlDbType.VarChar, 20)).Value = tocity
            mySqlCmd.Parameters.Add(New SqlParameter("@fromctry", SqlDbType.VarChar, 20)).Value = fromctry
            mySqlCmd.Parameters.Add(New SqlParameter("@toctry", SqlDbType.VarChar, 20)).Value = toctry
            mySqlCmd.Parameters.Add(New SqlParameter("@agingtype", SqlDbType.Int)).Value = agingtype
            mySqlCmd.Parameters.Add(New SqlParameter("@summdet", SqlDbType.Int)).Value = summdet
            mySqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.Int)).Value = web
            mySqlCmd.Parameters.Add(New SqlParameter("@custtype", SqlDbType.Int)).Value = custtype
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type
            mySqlCmd.Parameters.Add(New SqlParameter("@inclproforma", SqlDbType.Int)).Value = inclproforma
            mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = currcodefilter
            mySqlCmd.CommandTimeout = 0 'Tanvir 18042022
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            Dim Month As String = Format(Convert.ToDateTime(todate), "MM")
            If agingtype = 0 Then
                If Month = "01" Then
                    one = "JAN"
                    two = "DEC"
                    three = "NOV"
                    four = "OCT"
                    five = "SEP"
                    six = "AUG"
                    seven = "<=AUG"
                ElseIf Month = "02" Then
                    one = "FEB"
                    two = "JAN"
                    three = "DEC"
                    four = "NOV"
                    five = "OCT"
                    six = "SEP"
                    seven = "<=SEP"
                ElseIf Month = "03" Then
                    one = "MAR"
                    two = "FEB"
                    three = "JAN"
                    four = "DEC"
                    five = "NOV"
                    six = "OCT"
                    seven = "<=OCT"
                ElseIf Month = "04" Then
                    one = "APR"
                    two = "MAR"
                    three = "FEB"
                    four = "JAN"
                    five = "DEC"
                    six = "NOV"
                    seven = "<=NOV"
                ElseIf Month = "05" Then
                    one = "MAY"
                    two = "APR"
                    three = "MAR"
                    four = "FEB"
                    five = "JAN"
                    six = "DEC"
                    seven = "<=DEC"
                ElseIf Month = "06" Then
                    one = "JUN"
                    two = "MAY"
                    three = "APR"
                    four = "MAR"
                    five = "FEB"
                    six = "JAN"
                    seven = "<=JAN"
                ElseIf Month = "07" Then
                    one = "JUL"
                    two = "JUN"
                    three = "MAY"
                    four = "APR"
                    five = "MAR"
                    six = "FEB"
                    seven = "<=FEB"
                ElseIf Month = "08" Then
                    one = "AUG"
                    two = "JUL"
                    three = "JUN"
                    four = "MAY"
                    five = "APR"
                    six = "MAR"
                    seven = "<=MAR"
                ElseIf Month = "09" Then
                    one = "SEP"
                    two = "AUG"
                    three = "JUL"
                    four = "JUN"
                    five = "MAY"
                    six = "APR"
                    seven = "<=APR"
                ElseIf Month = "10" Then
                    one = "OCT"
                    two = "SEP"
                    three = "AUG"
                    four = "JUL"
                    five = "JUN"
                    six = "MAY"
                    seven = "<=MAY"
                ElseIf Month = "11" Then
                    one = "NOV"
                    two = "OCT"
                    three = "SEP"
                    four = "AUG"
                    five = "JUL"
                    six = "JUN"
                    seven = "<=JUN"
                ElseIf Month = "12" Then
                    one = "DEC"
                    two = "NOV"
                    three = "OCT"
                    four = "SEP"
                    five = "AUG"
                    six = "JUL"
                    seven = "<=JUL"
                End If
            Else
                one = "0-30"
                two = "30-60"
                three = "60-90"
                four = "90-120"
                five = "120-150"
                six = "150-365"
                seven = "Over 365"
            End If

            If currflg <> 1 Then
                currency = "(In A/C Currency)"
                currcode = custdetailsdt.AsEnumerable().Select(Function(s) s.Field(Of String)("currcode")).FirstOrDefault
            Else
                currency = "(In Base Currency)"
                currcode = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            End If
            If Type = "C" Then
                supplier = "Customer Type :"
            Else
                supplier = "Supplier Type :"
            End If
            If custtype = 0 Then
                customer = "All"
            ElseIf custtype = 1 Then
                customer = IIf(Type = "C", "Cash Customer", "Cash Supplier")
            ElseIf custtype = 2 Then
                customer = IIf(Type = "C", "Credit Customer", "Credit Supplier")
            End If
            If groupby = "2" Then
                selectedgroup = "Control A/c "
                common = "acctname"
            ElseIf groupby = "3" Then
                selectedgroup = "Category"
                common = "catname"
            ElseIf groupby = "4" Then
                selectedgroup = "Country"
                common = "ctryname"
            ElseIf groupby = "5" Then
                selectedgroup = "City"
                common = "plgrpname"
            Else
                common = "acc_code"
            End If

            If orderby = "1" Then
                custdetailsdt.DefaultView.Sort = IIf(groupby = "3", "catname ASC , acc_code ASC", IIf(groupby = "4", "ctryname DESC , acc_code ASC", IIf(groupby = "5", "plgrpname ASC , acc_code ASC", "acc_code ASC")))
            ElseIf orderby = "2" Then
                custdetailsdt.DefaultView.Sort = IIf(groupby = "3", "catname ASC , agentname ASC", IIf(groupby = "4", "ctryname DESC , agentname ASC", IIf(groupby = "5", "plgrpname ASC , agentname ASC", "agentname ASC")))
            End If
            custdetailsdt = custdetailsdt.DefaultView.ToTable

            addrLine1 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
            addrLine2 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
            addrLine3 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
            addrLine4 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
            addrLine5 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)
            headerarray = {"CODE", "NAME", "CR. LIMIT", "CURRENT", one, two, three, four, five, six, seven, "Balance"}



            If String.Equals(reportsType, "excel") Then
                ExcelReport(custdetailsdt, reportfilter, currency, reportname, todate, bytes)
            Else
                Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                Dim documentWidth As Single = 770.0F
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    Dim tableheader As PdfPTable = Nothing
                    Dim remainingPageSpace As Single
                    tableheader = New PdfPTable(1)
                    tableheader.TotalWidth = documentWidth
                    tableheader.LockedWidth = True
                    tableheader.SetWidths(New Single() {1.0F}) '    
                    tableheader.Complete = False
                    tableheader.SplitRows = False
                    tableheader.SpacingBefore = 15.0F
                    tableheader.WidthPercentage = 100
                    cell = ImageCell("~/Images/Logo.png", 80.0F, 80.0F)
                    tableheader.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(reportname, headerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingLeft = 70.0F
                    cell.PaddingBottom = 4.0F
                    cell.PaddingTop = 1.0F
                    tableheader.AddCell(cell)


                    'Dim commondata As PdfPTable = New PdfPTable(1)
                    'commondata.TotalWidth = documentWidth
                    'commondata.LockedWidth = True
                    'commondata.SetWidths(New Single() {1.0F}) '    
                    'commondata.Complete = False
                    'commondata.SplitRows = False
                    'commondata.SpacingBefore = 8.0F
                    'commondata.WidthPercentage = 100
                    'commondata.SpacingAfter = 4.0F
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("As on Date :" + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Space(15), normalfontbold))
                    'phrase.Add(New Chunk(currency + Space(15), normalfontbold))
                    'phrase.Add(New Chunk(supplier + Space(6) + customer + Environment.NewLine + vbLf, normalfontbold))
                    'phrase.Add(New Chunk(reportfilter, normalfontbold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    'cell.PaddingBottom = 4.0F
                    'cell.PaddingTop = 1.0F
                    'commondata.AddCell(cell)

                    Dim FooterTable = New PdfPTable(1)
                    FooterTable.TotalWidth = documentWidth
                    FooterTable.LockedWidth = True
                    FooterTable.SetWidths(New Single() {1.0F})
                    FooterTable.Complete = False
                    FooterTable.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine3 + addrLine5, Footerfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    ' cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine1, Footerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    ' cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine2 + "  " + addrLine4, Footerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    ' cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Printed Date:" & Date.Now.ToString("dd/MM/yyyy HH:mm:ss"), Footerfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    cell.PaddingBottom = 3.0F
                    FooterTable.AddCell(cell)
                    FooterTable.Complete = True


                    'Dim header As PdfPTable = New PdfPTable(12)
                    'header.TotalWidth = documentWidth
                    'header.LockedWidth = True
                    'header.SetWidths(New Single() {0.07F, 0.17F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.12F}) ' 

                    ''header.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                    'header.WidthPercentage = 100
                    'header.Complete = False
                    'header.SplitRows = False
                    '' Dim headerarray() As String = {"DETAIL", "CREDITLIMIT", "CURRENT", one, two, three, four, five, six, seven, "Balance"}


                    'For i = 0 To 11
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(headerarray(i), normalfontbold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingBottom = 4.0F
                    '    cell.PaddingTop = 1.0F
                    '    header.AddCell(cell)
                    'Next



                    'add common header and footer part to every page
                    'writer.PageEvent = New ClsHeaderFooter(tableheader, commondata, FooterTable, Header, "Ageing Summary")
                    writer.PageEvent = New ClsHeaderFooter(tableheader, Nothing, FooterTable, Nothing, "Ageing Summary")

                    document.Open()

                    Dim currList = (From n In custdetailsdt.AsEnumerable Group By currcode = n.Field(Of String)("currcode") Into grp = Group Order By currcode Select New With {.currcode = currcode}).ToList()

                    If currList.Count > 0 Then
                        For i = 0 To currList.Count - 1
                            fbal = 0
                            fage1 = 0
                            fage2 = 0
                            fage3 = 0
                            fage4 = 0
                            fage5 = 0
                            fage6 = 0
                            fage7 = 0
                            fage9 = 0

                            Dim tmpcurrcode As String = currList(i).currcode
                            Dim currwiseDt As DataTable = (From n In custdetailsdt.AsEnumerable Where n.Field(Of String)("currcode") = tmpcurrcode Select n).CopyToDataTable()

                            total = currwiseDt.NewRow()
                            totalrow = currwiseDt.NewRow()
                            totalrow("acc_code") = "Total in " + tmpcurrcode
                            totalrow("agentname") = ""
                            totalrow("creditlimit") = 0
                            If currwiseDt.Rows.Count > 0 Then

                                totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                                totalrow("age9") = currwiseDt.Compute("Sum(age9)", String.Empty)
                                totalrow("age1") = currwiseDt.Compute("Sum(age1)", String.Empty)
                                totalrow("age2") = currwiseDt.Compute("Sum(age2)", String.Empty)
                                totalrow("age3") = currwiseDt.Compute("Sum(age3)", String.Empty)
                                totalrow("age4") = currwiseDt.Compute("Sum(age4)", String.Empty)
                                totalrow("age5") = currwiseDt.Compute("Sum(age5)", String.Empty)
                                totalrow("age6") = currwiseDt.Compute("Sum(age6)", String.Empty)
                                totalrow("age7") = currwiseDt.Compute("sum(age7)", String.Empty)
                                totalrow("balance") = currwiseDt.Compute("sum(balance)", String.Empty)
                            Else
                                totalrow("age9") = 0
                                totalrow("age1") = 0
                                totalrow("age2") = 0
                                totalrow("age3") = 0
                                totalrow("age4") = 0
                                totalrow("age5") = 0
                                totalrow("age6") = 0
                                totalrow("age7") = 0
                                totalrow("balance") = 0
                            End If
                            total("acc_code") = "Final Total"
                            total("agentname") = ""
                            total("creditlimit") = 0
                            If currwiseDt.Rows.Count > 0 Then

                                total("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                                total("age9") = currwiseDt.Compute("Sum(age9)", String.Empty)
                                total("age1") = currwiseDt.Compute("Sum(age1)", String.Empty)
                                total("age2") = currwiseDt.Compute("Sum(age2)", String.Empty)
                                total("age3") = currwiseDt.Compute("Sum(age3)", String.Empty)
                                total("age4") = currwiseDt.Compute("Sum(age4)", String.Empty)
                                total("age5") = currwiseDt.Compute("Sum(age5)", String.Empty)
                                total("age6") = currwiseDt.Compute("Sum(age6)", String.Empty)
                                total("age7") = currwiseDt.Compute("sum(age7)", String.Empty)
                                total("balance") = currwiseDt.Compute("sum(balance)", String.Empty)
                            Else
                                total("age9") = 0
                                total("age1") = 0
                                total("age2") = 0
                                total("age3") = 0
                                total("age4") = 0
                                total("age5") = 0
                                total("age6") = 0
                                total("age7") = 0
                                total("balance") = 0
                            End If
                            currwiseDt.Rows.Add(totalrow)
                            'currwiseDt.Rows.Add(total) 'final total







                            Dim Datatable As PdfPTable = New PdfPTable(12)
                            Datatable.TotalWidth = documentWidth
                            Datatable.LockedWidth = True
                            Datatable.TotalWidth = documentWidth
                            Datatable.SetWidths(New Single() {0.07F, 0.17F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.12F}) ' 
                            Datatable.WidthPercentage = 100
                            Datatable.Complete = False
                            Datatable.SplitRows = False
                            Datatable.HeaderRows = 2


                            phrase = New Phrase()
                            phrase.Add(New Chunk("As on Date :" + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Space(15), normalfontbold))
                            phrase.Add(New Chunk(currency + " - " + tmpcurrcode + Space(15), normalfontbold))
                            phrase.Add(New Chunk(supplier + Space(6) + customer + Environment.NewLine + vbLf, normalfontbold))
                            phrase.Add(New Chunk(reportfilter, normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            cell.PaddingBottom = 1.0F
                            cell.PaddingTop = 7.0F
                            cell.Colspan = 12
                            Datatable.AddCell(cell)

                            For j = 0 To 11
                                phrase = New Phrase()
                                phrase.Add(New Chunk(headerarray(j), normalfontbold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 4.0F
                                Datatable.AddCell(cell)
                            Next

                            If currwiseDt.Rows.Count > 0 Then
                                If common <> "acc_code" Then

                                    Dim customerdetails = From custdetails In currwiseDt.AsEnumerable() Group custdetails By GroupName = New With {Key .GroupName = custdetails.Field(Of String)(common)} Into Group

                                    For Each Groupnameslist In customerdetails
                                        If Groupnameslist.GroupName.GroupName <> "" Then '  Groupnameslist.Group.Count
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(selectedgroup.ToString() & " - " & Groupnameslist.GroupName.GroupName, normalfontbold))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                            cell.PaddingBottom = 4.0F
                                            cell.Colspan = 12
                                            cell.PaddingTop = 1.0F
                                            Datatable.AddCell(cell)
                                            rowsc = Groupnameslist.Group.Count + 2
                                            For Each row In Groupnameslist.Group
                                                rowsc = rowsc - 1
                                                tage1 = tage1 + row("age1")
                                                tage2 = tage2 + row("age2")
                                                tage3 = tage3 + row("age3")
                                                tage4 = tage4 + row("age4")
                                                tage5 = tage5 + row("age5")
                                                tage6 = tage6 + row("age6")
                                                tage9 = tage9 + row("age9")
                                                tage7 = tage7 + row("age7")
                                                tbal = tbal + row("balance")
                                                PrintRow(row, Datatable)
                                            Next
                                            fage1 = fage1 + tage1
                                            fage2 = fage2 + tage2
                                            fage3 = fage3 + tage3
                                            fage4 = fage4 + tage4
                                            fage5 = fage5 + tage5
                                            fage6 = fage6 + tage6
                                            fage9 = fage9 + tage9
                                            fage7 = fage7 + tage7
                                            fbal = fbal + tbal
                                            totalrow("acc_code") = "Total For " + Groupnameslist.GroupName.GroupName
                                            totalrow("agentname") = ""
                                            totalrow("creditlimit") = 0
                                            totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                                            totalrow("age9") = tage9
                                            totalrow("age1") = tage1
                                            totalrow("age2") = tage2
                                            totalrow("age3") = tage3
                                            totalrow("age4") = tage4
                                            totalrow("age5") = tage5
                                            totalrow("age6") = tage6
                                            totalrow("age7") = tage7
                                            totalrow("balance") = tbal
                                            PrintRow(totalrow, Datatable, 1)
                                            tage1 = 0
                                            tage2 = 0
                                            tage3 = 0
                                            tage4 = 0
                                            tage5 = 0
                                            tage6 = 0
                                            tage9 = 0
                                            tage7 = 0
                                            tbal = 0
                                        End If
                                    Next
                                    totalrow("acc_code") = "Total in " + tmpcurrcode
                                    totalrow("agentname") = ""
                                    totalrow("creditlimit") = 0
                                    totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                                    totalrow("age9") = fage9
                                    totalrow("age1") = fage1
                                    totalrow("age2") = fage2
                                    totalrow("age3") = fage3
                                    totalrow("age4") = fage4
                                    totalrow("age5") = fage5
                                    totalrow("age6") = fage6
                                    totalrow("age7") = fage7
                                    totalrow("balance") = fbal
                                    rowsc = rowsc - 1
                                    PrintRow(totalrow, Datatable)
                                    'totalrow("acc_code") = "Final Total"
                                    'totalrow("agentname") = ""
                                    'totalrow("creditlimit") = 0
                                    'totalrow("currcode") = currwiseDt.Rows(0)("currcode").ToString()
                                    'totalrow("age9") = fage9
                                    'totalrow("age1") = fage1
                                    'totalrow("age2") = fage2
                                    'totalrow("age3") = fage3
                                    'totalrow("age4") = fage4
                                    'totalrow("age5") = fage5
                                    'totalrow("age6") = fage6
                                    'totalrow("age7") = fage7
                                    'totalrow("balance") = fbal
                                    'rowsc = rowsc - 1
                                    'PrintRow(totalrow, Datatable)
                                Else
                                    rowsc = currwiseDt.Rows.Count + 1
                                    For Each row In currwiseDt.Rows
                                        rowsc = rowsc - 1
                                        PrintRow(row, Datatable)
                                    Next
                                End If
                                document.Add(Datatable)
                            End If
                            If i < currList.Count - 1 Then
                                remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                                If remainingPageSpace < 72 Then document.NewPage()
                            End If
                        Next
                    End If

                    document.AddTitle(reportname)
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 775.0F, 10.0F, 0)
                                Next
                            End Using
                            bytes = mStream.ToArray()
                        End Using
                    End If
                End Using
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Private Shared Function ImageCell(path As String, scale As Single, align As Integer) As PdfPCell"
    Private Shared Function ImageCell(ByVal path As String, ByVal scale As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))
        image.ScalePercent(scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

End Class
