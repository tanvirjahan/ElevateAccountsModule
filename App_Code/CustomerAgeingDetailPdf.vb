Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Linq

Public Class CustomerAgeingDetailPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils


#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
    Dim Footerfont As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim gage1, gage2, gage3, gage4, gage5, gage6, gage7, gage9, gbal, fage1, fage2, fage3, fage4, fage5, fage6, fage7, fage9, fbal, tage1, tage2, tage3, tage4, tage5, tage6, tage7, tage9, tbal As Decimal
    Dim one, two, decimalPoint, headerarray(), three, currcode, currency, four, five, six, seven, addrLine1, addrLine2, addrLine3, addrLine4, addrLine5, pdc, common, supplier, customer As String
    Dim decno, currDecno, rownum As Integer
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


#Region "GenerateReport"
    Public Sub GenerateReport(ByVal todate As String, Type As String, reportfilter As String, currflg As Integer, reportname As String, reportsType As String, fromacct As String, toacct As String, fromcontrol As String, tocontrol As String,
                         fromcat As String, tocat As String, fromcity As String, tocity As String, fromctry As String, toctry As String,
                                                       agingtype As Integer, summdet As Integer, web As Integer, custtype As Integer, divcode As String, custgroup_sp_type As String, inclproforma As Integer, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, orderby As String, groupby As String, Optional ByVal fileName As String = "")

        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet



            decno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
            currcode = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            decimalPoint = "N" + decno.ToString()


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
            mySqlCmd.CommandTimeout = 0 'sharfudeen 12/09/20222
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            If groupby = "2" Then
                common = "acctname"
            ElseIf groupby = "3" Then
                common = "catname"
            ElseIf groupby = "4" Then
                common = "ctryname"
            ElseIf groupby = "5" Then
                common = "plgrpname"
            Else
                common = "acc_code"
            End If

            Dim dt = New DataTable()
            Dim acc = New DataColumn(common, GetType(String))
            dt.Columns.Add(acc)

            Dim acc_code = New DataTable()
            Dim acccode = New DataColumn("acc_code", GetType(String))
            acc_code.Columns.Add(acccode)

            addrLine1 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
            addrLine2 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
            addrLine3 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
            addrLine4 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
            addrLine5 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)

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
            If orderby = "1" Then
                custdetailsdt.DefaultView.Sort = IIf(groupby = "3", "catname ASC , acc_code ASC", IIf(groupby = "4", "ctryname DESC , acc_code ASC", IIf(groupby = "5", "plgrpname ASC , acc_code ASC", "acc_code ASC")))
            ElseIf orderby = "2" Then
                custdetailsdt.DefaultView.Sort = IIf(groupby = "3", "catname ASC , agentname ASC", IIf(groupby = "4", "ctryname DESC , agentname ASC", IIf(groupby = "5", "plgrpname ASC , agentname ASC", "agentname ASC")))
            End If
            custdetailsdt = custdetailsdt.DefaultView.ToTable
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
            If Type = "C" Then
                headerarray = {"DETAIL", "ARR.DATE", "CAN.DATE", "CREDITLIMIT", "CURRENT", one, two, three, four, five, six, seven, "Balance"}
            Else
                headerarray = {"DETAIL", "CREDITLIMIT", "CURRENT", one, two, three, four, five, six, seven, "Balance"}
            End If


            If String.Equals(reportsType, "excel") Then
                ExcelReport(custdetailsdt, dt, reportfilter, currflg, groupby, acc_code, currency, reportname, todate, Type, bytes)
            Else
                Dim document As New Document(PageSize.A4, 10.0F, 10.0F, 30.0F, 41.0F)
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                Dim documentWidth As Single = 780.0F


                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    Dim tableheader As PdfPTable = Nothing

                    'Header Table
                    tableheader = New PdfPTable(1)
                    tableheader.TotalWidth = documentWidth
                    tableheader.LockedWidth = True
                    tableheader.SetWidths(New Single() {1.0F}) '    
                    tableheader.Complete = False
                    tableheader.SplitRows = False
                    tableheader.SpacingBefore = 20.0F
                    tableheader.WidthPercentage = 100
                    If divcode = "02" Then
                        cell = ImageCell("~/Images/logo.jpg", 80.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/Images/logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    tableheader.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(reportname, headerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingLeft = 70.0F
                    cell.PaddingBottom = 4.0F
                    cell.PaddingTop = 1.0F
                    tableheader.AddCell(cell)

                    Dim commondata As PdfPTable = New PdfPTable(1)
                    commondata.TotalWidth = documentWidth
                    commondata.LockedWidth = True
                    commondata.SetWidths(New Single() {1.0F}) '    
                    commondata.Complete = False
                    commondata.SplitRows = False
                    commondata.SpacingBefore = 10.0F
                    commondata.WidthPercentage = 100
                    commondata.SpacingAfter = 5.0F
                    phrase = New Phrase()
                    phrase.Add(New Chunk("As on Date : " + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Space(15), normalfontbold))
                    phrase.Add(New Chunk(currency + Space(15), normalfontbold))
                    phrase.Add(New Chunk(supplier + Space(6) + customer + Environment.NewLine + vbLf, normalfontbold))
                    phrase.Add(New Chunk(reportfilter, normalfontbold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 4.0F
                    cell.PaddingTop = 1.0F
                    commondata.AddCell(cell)


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

                    Dim header As PdfPTable
                    If Type = "C" Then
                        header = New PdfPTable(13)
                        header.TotalWidth = documentWidth
                        header.LockedWidth = True
                        header.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F}) ' 
                        header.WidthPercentage = 100
                        header.Complete = False
                        header.SplitRows = False

                        For i = 0 To 12
                            phrase = New Phrase()
                            phrase.Add(New Chunk(headerarray(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            header.AddCell(cell)
                        Next
                    Else
                        header = New PdfPTable(11)
                        header.TotalWidth = documentWidth
                        header.LockedWidth = True
                        header.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                        header.WidthPercentage = 100
                        header.Complete = False
                        header.SplitRows = False

                        For i = 0 To 10
                            phrase = New Phrase()
                            phrase.Add(New Chunk(headerarray(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            header.AddCell(cell)
                        Next
                    End If


                    'add common header and footer part to every page
                    writer.PageEvent = New ClsHeaderFooter(tableheader, commondata, FooterTable, header)
                    document.Open()
                    If custdetailsdt.Rows.Count > 0 Then
                        For Each ageing In custdetailsdt.Rows
                            Dim grp_by As String = ageing(common).ToString()
                            Dim currDecno As Integer = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currcode & "'"), Integer)
                            Dim dr() As System.Data.DataRow
                            Dim dr1() As System.Data.DataRow
                            Dim dr2() As System.Data.DataRow
                            Dim dr3() As System.Data.DataRow
                            dr1 = dt.Select(common & "='" & grp_by & "'")
                            If dr1.Length = 0 Then
                                dt.Rows.Add(grp_by)
                                dr = custdetailsdt.Select(common & "='" & grp_by & "'")
                                If groupby <> "1" AndAlso groupby <> "0" Then
                                    Dim accode As PdfPTable = New PdfPTable(1)
                                    accode.TotalWidth = documentWidth
                                    accode.LockedWidth = True
                                    accode.SplitRows = False
                                    accode.SpacingBefore = 0.0F
                                    accode.WidthPercentage = 100
                                    accode.SpacingAfter = 0.0F
                                    accode.SetWidths(New Single() {1.0F}) '   
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(grp_by, caption))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.PaddingBottom = 4.0F
                                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                    cell.BorderWidthBottom = 0.0F
                                    accode.AddCell(cell)
                                    accode.Complete = True
                                    document.Add(accode)
                                    For i = 0 To dr.Length - 1
                                        Dim code As String = dr(i)("acc_code").ToString()
                                        dr3 = acc_code.Select("acc_code='" & code & "'")
                                        If dr3.Length = 0 Then
                                            acc_code.Rows.Add(code)
                                            dr2 = custdetailsdt.Select("acc_code='" & code & "'")
                                            Dim agent As PdfPTable = New PdfPTable(1)
                                            agent.TotalWidth = documentWidth
                                            agent.LockedWidth = True
                                            agent.SplitRows = False
                                            agent.SpacingBefore = 0.0F
                                            agent.WidthPercentage = 100
                                            agent.SpacingAfter = 0.0F
                                            agent.SetWidths(New Single() {1.0F}) '   
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(dr(i)("acc_code").ToString() + Space(10) + dr(i)("agentname").ToString(), caption))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.PaddingBottom = 4.0F
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                            agent.AddCell(cell)
                                            agent.Complete = True
                                            document.Add(agent)

                                            Dim data As PdfPTable
                                            If Type = "C" Then
                                                data = New PdfPTable(13)
                                                data.TotalWidth = documentWidth
                                                data.LockedWidth = True
                                                data.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F})
                                                data.WidthPercentage = 100
                                                data.Complete = False
                                                data.SplitRows = False
                                            Else
                                                data = New PdfPTable(11)
                                                data.TotalWidth = documentWidth
                                                data.LockedWidth = True
                                                data.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                                                data.WidthPercentage = 100
                                                data.Complete = False
                                                data.SplitRows = False
                                            End If

                                            For k = 0 To dr2.Length - 1
                                                tage1 = tage1 + Decimal.Parse(dr2(k)("age1"))
                                                tage2 = tage2 + Decimal.Parse(dr2(k)("age2"))
                                                tage3 = tage3 + Decimal.Parse(dr2(k)("age3"))
                                                tage4 = tage4 + Decimal.Parse(dr2(k)("age4"))
                                                tage5 = tage5 + Decimal.Parse(dr2(k)("age5"))
                                                tage6 = tage6 + Decimal.Parse(dr2(k)("age6"))
                                                tage7 = tage7 + Decimal.Parse(dr2(k)("age7"))
                                                tage9 = tage9 + Decimal.Parse(dr2(k)("age9"))
                                                tbal = tbal + Decimal.Parse(dr2(k)("balance"))

                                                Dim dataarray() As String
                                                If Type = "C" Then
                                                    dataarray = {dr2(k)("trantype").ToString() + " " + dr2(k)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr2(k)("trandate").ToString()), "dd/MM/yyyy"), Convert.ToString(dr2(k)("arrivaldate")), Convert.ToString(dr2(k)("canceldate")),
                                                    IIf(dr2(k)("pdc") = 0.0, "", dr2(k)("pdc")), IIf(dr2(k)("age9") = 0.0, "-", IIf(dr2(k)("age9") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age9"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age9")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr2(k)("age1") = 0.0, "-", IIf(dr2(k)("age1") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age1"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age1")).ToString("N2"))), IIf(dr2(k)("age2") = 0.0, "-", IIf(dr2(k)("age2") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age2"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age2")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr2(k)("age3") = 0.0, "-", IIf(dr2(k)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age3")).ToString("N2"))), IIf(dr2(k)("age4") = 0.0, "-", IIf(dr2(k)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age4"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age4")).ToString("N" + currDecno.ToString))),
                                                        IIf(dr2(k)("age5") = 0.0, "-", IIf(dr2(k)("age5") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age5"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age5")).ToString("N2"))), IIf(dr2(k)("age6") = 0.0, "-", IIf(dr2(k)("age6") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age6"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age6")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr2(k)("age7") = 0.0, "-", IIf(dr2(k)("age7") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age7"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age7")).ToString("N2"))), IIf(dr2(k)("balance") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("balance"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("balance")).ToString("N2"))}
                                                Else
                                                    dataarray = {dr2(k)("trantype").ToString() + " " + dr2(k)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr2(k)("trandate").ToString()), "dd/MM/yyyy"), IIf(dr2(k)("pdc") = 0.0, "", dr2(k)("pdc")), IIf(dr2(k)("age9") = 0.0, "-", IIf(dr2(k)("age9") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age9"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age9")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr2(k)("age1") = 0.0, "-", IIf(dr2(k)("age1") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age1"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age1")).ToString("N2"))), IIf(dr2(k)("age2") = 0.0, "-", IIf(dr2(k)("age2") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age2"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age2")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr2(k)("age3") = 0.0, "-", IIf(dr2(k)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age3")).ToString("N2"))), IIf(dr2(k)("age4") = 0.0, "-", IIf(dr2(k)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age4"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age4")).ToString("N" + currDecno.ToString))),
                                                        IIf(dr2(k)("age5") = 0.0, "-", IIf(dr2(k)("age5") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age5"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age5")).ToString("N2"))), IIf(dr2(k)("age6") = 0.0, "-", IIf(dr2(k)("age6") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age6"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age6")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr2(k)("age7") = 0.0, "-", IIf(dr2(k)("age7") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age7"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age7")).ToString("N2"))), IIf(dr2(k)("balance") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("balance"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("balance")).ToString("N2"))}
                                                End If


                                                'Dim dataarray() As String = {dr2(k)("trantype").ToString() + " " + dr2(k)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr2(k)("trandate").ToString()), "dd/MM/yyyy"), IIf(dr2(k)("pdc") = 0.0, "", dr2(k)("pdc")), IIf(dr2(k)("age9") = 0.0, "-", Decimal.Parse(dr2(k)("age9")).ToString(decimalPoint)),
                                                '  IIf(dr2(k)("age1") = 0.0, "-", Decimal.Parse(dr2(k)("age1")).ToString(decimalPoint)), IIf(dr2(k)("age2") = 0.0, "-", Decimal.Parse(dr2(k)("age2")).ToString(decimalPoint)), IIf(dr2(k)("age3") = 0.0, "-", Decimal.Parse(dr2(k)("age3")).ToString(decimalPoint)), IIf(dr2(k)("age4") = 0.0, "-", Decimal.Parse(dr2(k)("age4")).ToString(decimalPoint)),
                                                '      IIf(dr2(k)("age5") = 0.0, "-", Decimal.Parse(dr2(k)("age5")).ToString(decimalPoint)), IIf(dr2(k)("age6") = 0.0, "-", Decimal.Parse(dr2(k)("age6")).ToString(decimalPoint)),
                                                '   IIf(dr2(k)("age7") = 0.0, "-", Decimal.Parse(dr2(k)("age7")).ToString(decimalPoint)), IIf(dr2(k)("balance") = 0.0, "-", Decimal.Parse(dr2(k)("balance")).ToString(decimalPoint))}


                                                For j = 0 To dataarray.Length - 1
                                                    phrase = New Phrase()
                                                    phrase.Add(New Chunk(dataarray(j), normalfont))
                                                    If j = 0 Then
                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                                    Else
                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                                    End If

                                                    cell.PaddingBottom = 4.0F
                                                    cell.PaddingTop = 1.0F
                                                    data.AddCell(cell)
                                                Next
                                            Next
                                            document.Add(data)

                                            Dim data1 As PdfPTable
                                            Dim data1array() As String
                                            If Type = "C" Then
                                                data1 = New PdfPTable(13)
                                                data1.TotalWidth = documentWidth
                                                data1.LockedWidth = True
                                                data1.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F})
                                                data1.WidthPercentage = 100
                                                data1.Complete = False
                                                data1.SplitRows = False
                                                data1array = {"Total For " + dr(i)("agentname").ToString, "", "", IIf(dr(i)("creditlimit") = 0, "", IIf(dr(i)("creditlimit") < 0, "(" & Math.Abs(dr(i)("creditlimit")) & ")", dr(i)("creditlimit"))), IIf(tage9 = 0.0, "-", IIf(tage9 < 0.0, "(" & Math.Abs(tage9).ToString("N" + currDecno.ToString) & ")", tage9.ToString("N" + currDecno.ToString))),
                                                                               IIf(tage1 = 0.0, "-", IIf(tage1 < 0.0, "(" & Math.Abs(tage1).ToString("N" + currDecno.ToString) & ")", tage1.ToString("N" + currDecno.ToString))), IIf(tage2 = 0.0, "-", IIf(tage2 < 0.0, "(" & Math.Abs(tage2).ToString("N" + currDecno.ToString) & ")", tage2.ToString("N" + currDecno.ToString))), IIf(tage3 = 0.0, "-", IIf(tage3 < 0.0, "(" & Math.Abs(tage3).ToString("N" + currDecno.ToString) & ")", tage3.ToString("N" + currDecno.ToString))), IIf(tage4 = 0.0, "-", IIf(tage4 < 0.0, "(" & Math.Abs(tage4).ToString("N" + currDecno.ToString) & ")", tage4.ToString("N" + currDecno.ToString))), IIf(tage5 = 0.0, "-", IIf(tage5 < 0.0, "(" & Math.Abs(tage5).ToString("N" + currDecno.ToString) & ")", tage5.ToString("N" + currDecno.ToString))),
                                                           IIf(tage6 = 0.0, "-", IIf(tage6 < 0.0, "(" & Math.Abs(tage6).ToString("N" + currDecno.ToString) & ")", tage6.ToString("N" + currDecno.ToString))), IIf(tage7 = 0.0, "-", IIf(tage7 < 0.0, "(" & Math.Abs(tage7).ToString("N" + currDecno.ToString) & ")", tage7.ToString("N" + currDecno.ToString))), IIf(tbal < 0.0, "(" & Math.Abs(tbal).ToString("N" + currDecno.ToString) & ")", tbal.ToString("N" + currDecno.ToString))}
                                            Else
                                                data1 = New PdfPTable(11)
                                                data1.TotalWidth = documentWidth
                                                data1.LockedWidth = True
                                                data1.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                                                data1.WidthPercentage = 100
                                                data1.Complete = False
                                                data1.SplitRows = False
                                                data1array = {"Total For " + dr(i)("agentname").ToString, IIf(dr(i)("creditlimit") = 0, "", IIf(dr(i)("creditlimit") < 0, "(" & Math.Abs(dr(i)("creditlimit")) & ")", dr(i)("creditlimit"))), IIf(tage9 = 0.0, "-", IIf(tage9 < 0.0, "(" & Math.Abs(tage9).ToString("N" + currDecno.ToString) & ")", tage9.ToString("N" + currDecno.ToString))),
                                                                               IIf(tage1 = 0.0, "-", IIf(tage1 < 0.0, "(" & Math.Abs(tage1).ToString("N" + currDecno.ToString) & ")", tage1.ToString("N" + currDecno.ToString))), IIf(tage2 = 0.0, "-", IIf(tage2 < 0.0, "(" & Math.Abs(tage2).ToString("N" + currDecno.ToString) & ")", tage2.ToString("N" + currDecno.ToString))), IIf(tage3 = 0.0, "-", IIf(tage3 < 0.0, "(" & Math.Abs(tage3).ToString("N" + currDecno.ToString) & ")", tage3.ToString("N" + currDecno.ToString))), IIf(tage4 = 0.0, "-", IIf(tage4 < 0.0, "(" & Math.Abs(tage4).ToString("N" + currDecno.ToString) & ")", tage4.ToString("N" + currDecno.ToString))), IIf(tage5 = 0.0, "-", IIf(tage5 < 0.0, "(" & Math.Abs(tage5).ToString("N" + currDecno.ToString) & ")", tage5.ToString("N" + currDecno.ToString))),
                                                           IIf(tage6 = 0.0, "-", IIf(tage6 < 0.0, "(" & Math.Abs(tage6).ToString("N" + currDecno.ToString) & ")", tage6.ToString("N" + currDecno.ToString))), IIf(tage7 = 0.0, "-", IIf(tage7 < 0.0, "(" & Math.Abs(tage7).ToString("N" + currDecno.ToString) & ")", tage7.ToString("N" + currDecno.ToString))), IIf(tbal < 0.0, "(" & Math.Abs(tbal).ToString("N" + currDecno.ToString) & ")", tbal.ToString("N" + currDecno.ToString))}
                                            End If

                                            For j = 0 To data1array.Length - 1
                                                phrase = New Phrase()
                                                If j = 0 Then
                                                    phrase.Add(New Chunk(data1array(j), normalfontbold))
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                                Else
                                                    phrase.Add(New Chunk(data1array(j), normalfontbold))
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                                End If
                                                cell.PaddingBottom = 4.0F
                                                cell.PaddingTop = 1.0F
                                                data1.AddCell(cell)
                                            Next
                                            document.Add(data1)

                                            'total for particular grpby
                                            gage1 = gage1 + tage1
                                            gage2 = gage2 + tage2
                                            gage3 = gage3 + tage3
                                            gage4 = gage4 + tage4
                                            gage5 = gage5 + tage5
                                            gage6 = gage6 + tage6
                                            gage7 = gage7 + tage7
                                            gage9 = gage9 + tage9
                                            gbal = gbal + tbal
                                            tage1 = 0.0F
                                            tage2 = 0.0F
                                            tage3 = 0.0F
                                            tage4 = 0.0F
                                            tage5 = 0.0F
                                            tage6 = 0.0F
                                            tage7 = 0.0F
                                            tage9 = 0.0F
                                            tbal = 0.0F
                                        End If
                                    Next

                                    Dim grpby As PdfPTable
                                    Dim grpbyarray() As String
                                    If Type = "C" Then
                                        grpby = New PdfPTable(13)
                                        grpby.TotalWidth = documentWidth
                                        grpby.LockedWidth = True
                                        grpby.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F})
                                        grpby.WidthPercentage = 100
                                        grpby.Complete = False
                                        grpby.SplitRows = False
                                        grpbyarray = {"Total For " + grp_by, "", "", "", IIf(gage9 = 0.0, "-", IIf(gage9 < 0.0, "(" & Math.Abs(gage9).ToString("N" + currDecno.ToString) & ")", gage9.ToString("N" + currDecno.ToString))),
                                                                       IIf(gage1 = 0.0, "-", IIf(gage1 < 0.0, "(" & Math.Abs(gage1).ToString("N" + currDecno.ToString) & ")", gage1.ToString("N" + currDecno.ToString))), IIf(gage2 = 0.0, "-", IIf(gage2 < 0.0, "(" & Math.Abs(gage2).ToString("N" + currDecno.ToString) & ")", gage2.ToString("N" + currDecno.ToString))), IIf(gage3 = 0.0, "-", IIf(gage3 < 0.0, "(" & Math.Abs(gage3).ToString("N" + currDecno.ToString) & ")", gage3.ToString("N" + currDecno.ToString))), IIf(gage4 = 0.0, "-", IIf(gage4 < 0.0, "(" & Math.Abs(gage4).ToString("N" + currDecno.ToString) & ")", gage4.ToString("N" + currDecno.ToString))), IIf(gage5 = 0.0, "-", IIf(gage5 < 0.0, "(" & Math.Abs(gage5).ToString("N" + currDecno.ToString) & ")", gage5.ToString("N" + currDecno.ToString))),
                                                   IIf(gage6 = 0.0, "-", IIf(gage6 < 0.0, "(" & Math.Abs(gage6).ToString("N" + currDecno.ToString) & ")", gage6.ToString("N" + currDecno.ToString))), IIf(gage7 = 0.0, "-", IIf(gage7 < 0.0, "(" & Math.Abs(gage7).ToString("N" + currDecno.ToString) & ")", gage7.ToString("N" + currDecno.ToString))), IIf(gbal < 0.0, "(" & Math.Abs(gbal).ToString("N" + currDecno.ToString) & ")", gbal.ToString("N" + currDecno.ToString))}
                                    Else
                                        grpby = New PdfPTable(11)
                                        grpby.TotalWidth = documentWidth
                                        grpby.LockedWidth = True
                                        grpby.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                                        grpby.WidthPercentage = 100
                                        grpby.Complete = False
                                        grpby.SplitRows = False
                                        grpbyarray = {"Total For " + grp_by, "", IIf(gage9 = 0.0, "-", IIf(gage9 < 0.0, "(" & Math.Abs(gage9).ToString("N" + currDecno.ToString) & ")", gage9.ToString("N" + currDecno.ToString))),
                                                                       IIf(gage1 = 0.0, "-", IIf(gage1 < 0.0, "(" & Math.Abs(gage1).ToString("N" + currDecno.ToString) & ")", gage1.ToString("N" + currDecno.ToString))), IIf(gage2 = 0.0, "-", IIf(gage2 < 0.0, "(" & Math.Abs(gage2).ToString("N" + currDecno.ToString) & ")", gage2.ToString("N" + currDecno.ToString))), IIf(gage3 = 0.0, "-", IIf(gage3 < 0.0, "(" & Math.Abs(gage3).ToString("N" + currDecno.ToString) & ")", gage3.ToString("N" + currDecno.ToString))), IIf(gage4 = 0.0, "-", IIf(gage4 < 0.0, "(" & Math.Abs(gage4).ToString("N" + currDecno.ToString) & ")", gage4.ToString("N" + currDecno.ToString))), IIf(gage5 = 0.0, "-", IIf(gage5 < 0.0, "(" & Math.Abs(gage5).ToString("N" + currDecno.ToString) & ")", gage5.ToString("N" + currDecno.ToString))),
                                                   IIf(gage6 = 0.0, "-", IIf(gage6 < 0.0, "(" & Math.Abs(gage6).ToString("N" + currDecno.ToString) & ")", gage6.ToString("N" + currDecno.ToString))), IIf(gage7 = 0.0, "-", IIf(gage7 < 0.0, "(" & Math.Abs(gage7).ToString("N" + currDecno.ToString) & ")", gage7.ToString("N" + currDecno.ToString))), IIf(gbal < 0.0, "(" & Math.Abs(gbal).ToString("N" + currDecno.ToString) & ")", gbal.ToString("N" + currDecno.ToString))}
                                    End If

                                    For j = 0 To grpbyarray.Length - 1
                                        phrase = New Phrase()
                                        If j = 0 Then
                                            phrase.Add(New Chunk(grpbyarray(j), normalfontbold))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                        Else
                                            phrase.Add(New Chunk(grpbyarray(j), normalfontbold))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                        End If
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        grpby.AddCell(cell)
                                    Next
                                    document.Add(grpby)


                                Else
                                    Dim agent As PdfPTable = New PdfPTable(1)
                                    agent.TotalWidth = documentWidth
                                    agent.LockedWidth = True
                                    agent.SplitRows = False
                                    agent.SpacingBefore = 0.0F
                                    agent.WidthPercentage = 100
                                    agent.SpacingAfter = 0.0F
                                    agent.SetWidths(New Single() {1.0F}) '   
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(dr(0)("acc_code").ToString() + Space(10) + dr(0)("agentname").ToString(), caption))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.PaddingBottom = 4.0F
                                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                    agent.AddCell(cell)
                                    agent.Complete = True
                                    document.Add(agent)

                                    Dim data As PdfPTable
                                    If Type = "C" Then
                                        data = New PdfPTable(13)
                                        data.TotalWidth = documentWidth
                                        data.LockedWidth = True
                                        data.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F}) ' 
                                        data.WidthPercentage = 100
                                        data.Complete = False
                                        data.SplitRows = False
                                    Else
                                        data = New PdfPTable(11)
                                        data.TotalWidth = documentWidth
                                        data.LockedWidth = True
                                        data.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                                        data.WidthPercentage = 100
                                        data.Complete = False
                                        data.SplitRows = False
                                    End If

                                    For i = 0 To dr.Length - 1
                                        gage1 = gage1 + Decimal.Parse(dr(i)("age1"))
                                        gage2 = gage2 + Decimal.Parse(dr(i)("age2"))
                                        gage3 = gage3 + Decimal.Parse(dr(i)("age3"))
                                        gage4 = gage4 + Decimal.Parse(dr(i)("age4"))
                                        gage5 = gage5 + Decimal.Parse(dr(i)("age5"))
                                        gage6 = gage6 + Decimal.Parse(dr(i)("age6"))
                                        gage7 = gage7 + Decimal.Parse(dr(i)("age7"))
                                        gage9 = gage9 + Decimal.Parse(dr(i)("age9"))
                                        gbal = gbal + Decimal.Parse(dr(i)("balance"))

                                        Dim dataarray() As String
                                        If Type = "C" Then
                                            dataarray = {dr(i)("trantype").ToString() + " " + dr(i)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr(i)("trandate").ToString()), "dd/MM/yyyy"), Convert.ToString(dr(i)("arrivaldate")), Convert.ToString(dr(i)("canceldate")),
                                                    IIf(dr(i)("pdc") = 0.0, "", dr(i)("pdc")), IIf(dr(i)("age9") = 0.0, "-", IIf(dr(i)("age9") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age9"))).ToString("N") & ")", Decimal.Parse(dr(i)("age9")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr(i)("age1") = 0.0, "-", IIf(dr(i)("age1") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age1"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age1")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age2") = 0.0, "-", IIf(dr(i)("age2") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age2"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age2")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr(i)("age3") = 0.0, "-", IIf(dr(i)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age3")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age4") = 0.0, "-", IIf(dr(i)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age4"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age4")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr(i)("age5") = 0.0, "-", IIf(dr(i)("age5") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age5"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age5")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age6") = 0.0, "-", IIf(dr(i)("age6") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age6"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age6")).ToString("N" + currDecno.ToString))),
                                                    IIf(dr(i)("age7") = 0.0, "-", IIf(dr(i)("age7") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age7"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age7")).ToString("N" + currDecno.ToString))), IIf(dr(i)("balance") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("balance"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("balance")).ToString("N" + currDecno.ToString))}
                                        Else
                                            dataarray = {dr(i)("trantype").ToString() + " " + dr(i)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr(i)("trandate").ToString()), "dd/MM/yyyy"), IIf(dr(i)("pdc") = 0.0, "", dr(i)("pdc")), IIf(dr(i)("age9") = 0.0, "-", IIf(dr(i)("age9") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age9"))).ToString("N") & ")", Decimal.Parse(dr(i)("age9")).ToString("N" + currDecno.ToString))),
                                                IIf(dr(i)("age1") = 0.0, "-", IIf(dr(i)("age1") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age1"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age1")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age2") = 0.0, "-", IIf(dr(i)("age2") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age2"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age2")).ToString("N" + currDecno.ToString))),
                                                IIf(dr(i)("age3") = 0.0, "-", IIf(dr(i)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age3")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age4") = 0.0, "-", IIf(dr(i)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age4"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age4")).ToString("N" + currDecno.ToString))),
                                                IIf(dr(i)("age5") = 0.0, "-", IIf(dr(i)("age5") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age5"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age5")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age6") = 0.0, "-", IIf(dr(i)("age6") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age6"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age6")).ToString("N" + currDecno.ToString))),
                                                IIf(dr(i)("age7") = 0.0, "-", IIf(dr(i)("age7") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age7"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age7")).ToString("N" + currDecno.ToString))), IIf(dr(i)("balance") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("balance"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("balance")).ToString("N" + currDecno.ToString))}
                                        End If

                                        For j = 0 To dataarray.Length - 1
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(dataarray(j), normalfont))
                                            If j = 0 Then
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                            Else
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                            End If
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            data.AddCell(cell)
                                        Next
                                    Next
                                    document.Add(data)

                                    Dim data1 As PdfPTable
                                    Dim data1array() As String
                                    If Type = "C" Then
                                        data1 = New PdfPTable(13)
                                        data1.TotalWidth = documentWidth
                                        data1.LockedWidth = True
                                        data1.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F}) ' 
                                        data1.WidthPercentage = 100
                                        data1.Complete = False
                                        data1.SplitRows = False
                                        data1array = {"Total For " + dr(0)("agentname").ToString(), "", "", IIf(dr(0)("creditlimit") = 0, "", IIf(dr(0)("creditlimit") < 0, "(" & Math.Abs(dr(0)("creditlimit")) & ")", dr(0)("creditlimit"))), IIf(gage9 = 0.0, "-", IIf(gage9 < 0.0, "(" & Math.Abs(gage9).ToString("N" + currDecno.ToString) & ")", gage9.ToString("N" + currDecno.ToString))),
                                                     IIf(gage1 = 0.0, "-", IIf(gage1 < 0.0, "(" & Math.Abs(gage1).ToString("N" + currDecno.ToString) & ")", gage1.ToString("N" + currDecno.ToString))), IIf(gage2 = 0.0, "-", IIf(gage2 < 0.0, "(" & Math.Abs(gage2).ToString("N" + currDecno.ToString) & ")", gage2.ToString("N" + currDecno.ToString))), IIf(gage3 = 0.0, "-", IIf(gage3 < 0.0, "(" & Math.Abs(gage3).ToString("N" + currDecno.ToString) & ")", gage3.ToString("N" + currDecno.ToString))), IIf(gage4 = 0.0, "-", IIf(gage4 < 0.0, "(" & Math.Abs(gage4).ToString("N" + currDecno.ToString) & ")", gage4.ToString("N" + currDecno.ToString))), IIf(gage5 = 0.0, "-", IIf(gage5 < 0.0, "(" & Math.Abs(gage5).ToString("N" + currDecno.ToString) & ")", gage5.ToString("N" + currDecno.ToString))),
                                                     IIf(gage6 = 0.0, "-", IIf(gage6 < 0.0, "(" & Math.Abs(gage6).ToString("N" + currDecno.ToString) & ")", gage6.ToString("N" + currDecno.ToString))), IIf(gage7 = 0.0, "-", IIf(gage7 < 0.0, "(" & Math.Abs(gage7).ToString("N" + currDecno.ToString) & ")", gage7.ToString("N" + currDecno.ToString))), IIf(gbal < 0.0, "(" & Math.Abs(gbal).ToString("N" + currDecno.ToString) & ")", gbal.ToString("N" + currDecno.ToString))}
                                    Else
                                        data1 = New PdfPTable(11)
                                        data1.TotalWidth = documentWidth
                                        data1.LockedWidth = True
                                        data1.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                                        data1.WidthPercentage = 100
                                        data1.Complete = False
                                        data1.SplitRows = False
                                        data1array = {"Total For " + dr(0)("agentname").ToString(), IIf(dr(0)("creditlimit") = 0, "", IIf(dr(0)("creditlimit") < 0, "(" & Math.Abs(dr(0)("creditlimit")) & ")", dr(0)("creditlimit"))), IIf(gage9 = 0.0, "-", IIf(gage9 < 0.0, "(" & Math.Abs(gage9).ToString("N" + currDecno.ToString) & ")", gage9.ToString("N" + currDecno.ToString))),
                                                     IIf(gage1 = 0.0, "-", IIf(gage1 < 0.0, "(" & Math.Abs(gage1).ToString("N" + currDecno.ToString) & ")", gage1.ToString("N" + currDecno.ToString))), IIf(gage2 = 0.0, "-", IIf(gage2 < 0.0, "(" & Math.Abs(gage2).ToString("N" + currDecno.ToString) & ")", gage2.ToString("N" + currDecno.ToString))), IIf(gage3 = 0.0, "-", IIf(gage3 < 0.0, "(" & Math.Abs(gage3).ToString("N" + currDecno.ToString) & ")", gage3.ToString("N" + currDecno.ToString))), IIf(gage4 = 0.0, "-", IIf(gage4 < 0.0, "(" & Math.Abs(gage4).ToString("N" + currDecno.ToString) & ")", gage4.ToString("N" + currDecno.ToString))), IIf(gage5 = 0.0, "-", IIf(gage5 < 0.0, "(" & Math.Abs(gage5).ToString("N" + currDecno.ToString) & ")", gage5.ToString("N" + currDecno.ToString))),
                                                     IIf(gage6 = 0.0, "-", IIf(gage6 < 0.0, "(" & Math.Abs(gage6).ToString("N" + currDecno.ToString) & ")", gage6.ToString("N" + currDecno.ToString))), IIf(gage7 = 0.0, "-", IIf(gage7 < 0.0, "(" & Math.Abs(gage7).ToString("N" + currDecno.ToString) & ")", gage7.ToString("N" + currDecno.ToString))), IIf(gbal < 0.0, "(" & Math.Abs(gbal).ToString("N" + currDecno.ToString) & ")", gbal.ToString("N" + currDecno.ToString))}
                                    End If

                                    For j = 0 To data1array.Length - 1
                                        phrase = New Phrase()
                                        If j = 0 Then
                                            phrase.Add(New Chunk(data1array(j), normalfontbold))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                        Else
                                            phrase.Add(New Chunk(data1array(j), normalfontbold))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                        End If
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        data1.AddCell(cell)
                                    Next
                                    document.Add(data1)
                                End If
                            End If
                            fage1 = fage1 + gage1
                            fage2 = fage2 + gage2
                            fage3 = fage3 + gage3
                            fage4 = fage4 + gage4
                            fage5 = fage5 + gage5
                            fage6 = fage6 + gage6
                            fage7 = fage7 + gage7
                            fage9 = fage9 + gage9
                            fbal = fbal + gbal
                            gage1 = 0.0F
                            gage2 = 0.0F
                            gage3 = 0.0F
                            gage4 = 0.0F
                            gage5 = 0.0F
                            gage6 = 0.0F
                            gage7 = 0.0F
                            gage9 = 0.0F
                            gbal = 0.0F
                        Next

                        Dim data2 As PdfPTable
                        Dim data2array() As String
                        If Type = "C" Then
                            data2 = New PdfPTable(13)
                            data2.TotalWidth = documentWidth
                            data2.LockedWidth = True
                            data2.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F})
                            data2.WidthPercentage = 100
                            data2.Complete = False
                            data2.SplitRows = False
                            data2array = {"Total in " + currcode, "", "", "", IIf(fage9 = 0.0, "-", IIf(fage9 < 0.0, "(" & Math.Abs(fage9).ToString(decimalPoint) & ")", fage9.ToString("N2"))),
                                                           IIf(fage1 = 0.0, "-", IIf(fage1 < 0.0, "(" & Math.Abs(fage1).ToString(decimalPoint) & ")", fage1.ToString("N2"))), IIf(fage2 = 0.0, "-", IIf(fage2 < 0.0, "(" & Math.Abs(fage2).ToString(decimalPoint) & ")", fage2.ToString(decimalPoint))), IIf(fage3 = 0.0, "-", IIf(fage3 < 0.0, "(" & Math.Abs(fage3).ToString(decimalPoint) & ")", fage3.ToString(decimalPoint))), IIf(fage4 = 0.0, "-", IIf(fage4 < 0.0, "(" & Math.Abs(fage4).ToString(decimalPoint) & ")", fage4.ToString(decimalPoint))), IIf(fage5 = 0.0, "-", IIf(fage5 < 0.0, "(" & Math.Abs(fage5).ToString(decimalPoint) & ")", fage5.ToString(decimalPoint))),
                                       IIf(fage6 = 0.0, "-", IIf(fage6 < 0.0, "(" & Math.Abs(fage6).ToString(decimalPoint) & ")", fage6.ToString(decimalPoint))), IIf(fage7 = 0.0, "-", IIf(fage7 < 0.0, "(" & Math.Abs(fage7).ToString(decimalPoint) & ")", fage7.ToString(decimalPoint))), IIf(fbal < 0.0, "(" & Math.Abs(fbal).ToString(decimalPoint) & ")", fbal.ToString(decimalPoint))}
                        Else
                            data2 = New PdfPTable(11)
                            data2.TotalWidth = documentWidth
                            data2.LockedWidth = True
                            data2.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                            data2.WidthPercentage = 100
                            data2.Complete = False
                            data2.SplitRows = False
                            data2array = {"Total in " + currcode, "", IIf(fage9 = 0.0, "-", IIf(fage9 < 0.0, "(" & Math.Abs(fage9).ToString(decimalPoint) & ")", fage9.ToString("N2"))),
                                                           IIf(fage1 = 0.0, "-", IIf(fage1 < 0.0, "(" & Math.Abs(fage1).ToString(decimalPoint) & ")", fage1.ToString("N2"))), IIf(fage2 = 0.0, "-", IIf(fage2 < 0.0, "(" & Math.Abs(fage2).ToString(decimalPoint) & ")", fage2.ToString(decimalPoint))), IIf(fage3 = 0.0, "-", IIf(fage3 < 0.0, "(" & Math.Abs(fage3).ToString(decimalPoint) & ")", fage3.ToString(decimalPoint))), IIf(fage4 = 0.0, "-", IIf(fage4 < 0.0, "(" & Math.Abs(fage4).ToString(decimalPoint) & ")", fage4.ToString(decimalPoint))), IIf(fage5 = 0.0, "-", IIf(fage5 < 0.0, "(" & Math.Abs(fage5).ToString(decimalPoint) & ")", fage5.ToString(decimalPoint))),
                                       IIf(fage6 = 0.0, "-", IIf(fage6 < 0.0, "(" & Math.Abs(fage6).ToString(decimalPoint) & ")", fage6.ToString(decimalPoint))), IIf(fage7 = 0.0, "-", IIf(fage7 < 0.0, "(" & Math.Abs(fage7).ToString(decimalPoint) & ")", fage7.ToString(decimalPoint))), IIf(fbal < 0.0, "(" & Math.Abs(fbal).ToString(decimalPoint) & ")", fbal.ToString(decimalPoint))}
                        End If

                        For j = 0 To data2array.Length - 1
                            phrase = New Phrase()
                            If j = 0 Then
                                phrase.Add(New Chunk(data2array(j), normalfontbold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                cell.BackgroundColor = BaseColor.LIGHT_GRAY
                            Else
                                phrase.Add(New Chunk(data2array(j), normalfontbold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            End If
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            data2.AddCell(cell)
                        Next
                        document.Add(data2)

                        Dim data3 As PdfPTable
                        Dim data3array() As String
                        If Type = "C" Then
                            data3 = New PdfPTable(13)
                            data3.TotalWidth = documentWidth
                            data3.LockedWidth = True
                            data3.SetWidths(New Single() {0.14F, 0.06F, 0.06F, 0.08F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.08F, 0.08F, 0.08F})
                            data3.WidthPercentage = 100
                            data3.Complete = False
                            data3.SplitRows = False
                            data3array = {"Final Total", "", "", "", IIf(fage9 = 0.0, "-", IIf(fage9 < 0.0, "(" & Math.Abs(fage9).ToString(decimalPoint) & ")", fage9.ToString(decimalPoint))),
                                       IIf(fage1 = 0.0, "-", IIf(fage1 < 0.0, "(" & Math.Abs(fage1).ToString(decimalPoint) & ")", fage1.ToString(decimalPoint))), IIf(fage2 = 0.0, "-", IIf(fage2 < 0.0, "(" & Math.Abs(fage2).ToString(decimalPoint) & ")", fage2.ToString(decimalPoint))), IIf(fage3 = 0.0, "-", IIf(fage3 < 0.0, "(" & Math.Abs(fage3).ToString(decimalPoint) & ")", fage3.ToString(decimalPoint))), IIf(fage4 = 0.0, "-", IIf(fage4 < 0.0, "(" & Math.Abs(fage4).ToString(decimalPoint) & ")", fage4.ToString(decimalPoint))), IIf(fage5 = 0.0, "-", IIf(fage5 < 0.0, "(" & Math.Abs(fage5).ToString(decimalPoint) & ")", fage5.ToString(decimalPoint))),
                                       IIf(fage6 = 0.0, "-", IIf(fage6 < 0.0, "(" & Math.Abs(fage6).ToString(decimalPoint) & ")", fage6.ToString(decimalPoint))), IIf(fage7 = 0.0, "-", IIf(fage7 < 0.0, "(" & Math.Abs(fage7).ToString(decimalPoint) & ")", fage7.ToString(decimalPoint))), IIf(fbal < 0.0, "(" & Math.Abs(fbal).ToString(decimalPoint) & ")", fbal.ToString(decimalPoint))}
                        Else
                            data3 = New PdfPTable(11)
                            data3.TotalWidth = documentWidth
                            data3.LockedWidth = True
                            data3.SetWidths(New Single() {0.14F, 0.1F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.08F, 0.12F}) ' 
                            data3.WidthPercentage = 100
                            data3.Complete = False
                            data3.SplitRows = False
                            data3array = {"Final Total", "", IIf(fage9 = 0.0, "-", IIf(fage9 < 0.0, "(" & Math.Abs(fage9).ToString(decimalPoint) & ")", fage9.ToString(decimalPoint))),
                                       IIf(fage1 = 0.0, "-", IIf(fage1 < 0.0, "(" & Math.Abs(fage1).ToString(decimalPoint) & ")", fage1.ToString(decimalPoint))), IIf(fage2 = 0.0, "-", IIf(fage2 < 0.0, "(" & Math.Abs(fage2).ToString(decimalPoint) & ")", fage2.ToString(decimalPoint))), IIf(fage3 = 0.0, "-", IIf(fage3 < 0.0, "(" & Math.Abs(fage3).ToString(decimalPoint) & ")", fage3.ToString(decimalPoint))), IIf(fage4 = 0.0, "-", IIf(fage4 < 0.0, "(" & Math.Abs(fage4).ToString(decimalPoint) & ")", fage4.ToString(decimalPoint))), IIf(fage5 = 0.0, "-", IIf(fage5 < 0.0, "(" & Math.Abs(fage5).ToString(decimalPoint) & ")", fage5.ToString(decimalPoint))),
                                       IIf(fage6 = 0.0, "-", IIf(fage6 < 0.0, "(" & Math.Abs(fage6).ToString(decimalPoint) & ")", fage6.ToString(decimalPoint))), IIf(fage7 = 0.0, "-", IIf(fage7 < 0.0, "(" & Math.Abs(fage7).ToString(decimalPoint) & ")", fage7.ToString(decimalPoint))), IIf(fbal < 0.0, "(" & Math.Abs(fbal).ToString(decimalPoint) & ")", fbal.ToString(decimalPoint))}
                        End If

                        For j = 0 To data3array.Length - 1
                            phrase = New Phrase()
                            If j = 0 Then
                                phrase.Add(New Chunk(data3array(j), normalfontbold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                cell.BackgroundColor = BaseColor.LIGHT_GRAY
                            Else
                                phrase.Add(New Chunk(data3array(j), normalfontbold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            End If
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            data3.AddCell(cell)
                        Next
                        document.Add(data3)
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
                                    '     ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 775.0F, 10.0F, 0)
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

#Region "ExcelReport"
    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal reportfilter As String, ByVal currflg As Integer, ByVal groupby As String, ByVal acc_code As DataTable, ByVal currency As String, ByVal rptreportname As String, ByVal todate As String, ByVal Type As String, ByRef bytes() As Byte)
        Dim arrHeaders() As String
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add(rptreportname)
        ws.Columns.AdjustToContents()
        ws.Columns("A").Width = 23
        If Type = "C" Then
            ws.Columns("B:M").Width = 12
        Else
            ws.Columns("B:K").Width = 12
        End If
        rownum = 7
        Dim rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)

        ws.Cell("A1").Value = rptcompanyname
        Dim companyname
        If Type = "C" Then
            companyname = ws.Range("A1:M1").Merge()
        Else
            companyname = ws.Range("A1:K1").Merge()
        End If
        companyname.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        companyname.Style.Font.SetBold().Font.FontSize = 15
        companyname.Style.Font.FontColor = XLColor.Black
        companyname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        companyname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        'Report Name Heading
        ws.Cell("A2").Value = rptreportname
        Dim report
        If Type = "C" Then
            report = ws.Range("A2:M2").Merge()
        Else
            report = ws.Range("A2:K2").Merge()
        End If
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        report.Style.Font.SetBold().Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.Black
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        Dim rfilter
        If Type = "C" Then
            rfilter = ws.Range("A4:M4").Merge()
        Else
            rfilter = ws.Range("A4:K4").Merge()
        End If
        rfilter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        rfilter.Style.Font.SetBold().Font.FontSize = 12
        rfilter.Style.Font.FontColor = XLColor.Black
        rfilter.Cell(1, 1).Value = "As on Date : " + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Space(15) + currency + Space(15) + supplier + Space(6) + customer

        Dim filter
        If Type = "C" Then
            filter = ws.Range("A5:M5").Merge()
        Else
            filter = ws.Range("A5:K5").Merge()
        End If
        filter.Style.Font.SetBold().Font.FontSize = 12
        filter.Style.Font.FontColor = XLColor.Black
        filter.Cell(1, 1).Value = reportfilter
        Dim rowheight As Integer

        If reportfilter.Length > 100 Then
            rowheight = IIf(reportfilter.Length > 100 And reportfilter.Length < 200, 32, IIf(reportfilter.Length > 200 And reportfilter.Length < 300, 48, 64))
            ws.Row(5).Height = rowheight
        End If

        filter.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True


        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
        For i = 0 To headerarray.Length - 1
            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            ws.Cell(rownum, i + 1).Value = headerarray(i)
        Next

        Dim numformat, numformat1 As String
        Dim positiveFormat As String = ""
        Dim negativeFormat As String = ""
        Dim zeroFormat As String = ""
        Dim Fullnumformat As String = ""

        If custdetailsdt.Rows.Count > 0 Then
            For Each ageing In custdetailsdt.Rows
                Dim grp_by As String = ageing(common).ToString()
                currDecno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & ageing("currcode") & "'"), Integer)
                If currDecno = 2 Then
                    numformat = "#,##0.00"
                    numformat1 = "(#,##0.00)"

                    'sharfudeen 11/09/2022
                    positiveFormat = "#,##0.00_)"
                    negativeFormat = "(#,##0.00)"
                    zeroFormat = "-_)"
                    Fullnumformat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat + " ;@"


                ElseIf currDecno = 3 Then
                    numformat = "#,##0.000"
                    numformat1 = "(#,##0.000)"
                ElseIf currDecno = 4 Then
                    numformat = "#,##0.0000"
                    numformat1 = "(#,##0.0000)"
                Else
                    numformat = "#,##0.00"
                    numformat1 = "(#,##0.00)"
                End If
                Dim dr() As System.Data.DataRow
                Dim dr1() As System.Data.DataRow
                Dim dr2() As System.Data.DataRow
                Dim dr3() As System.Data.DataRow
                dr1 = dt.Select(common & "='" & grp_by & "'")
                If dr1.Length = 0 Then
                    dt.Rows.Add(grp_by)
                    dr = custdetailsdt.Select(common & "='" & grp_by & "'")
                    If groupby <> "1" AndAlso groupby <> "0" Then
                        rownum = rownum + 1
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Value = grp_by
                        ws.Range(rownum, 1, rownum, headerarray.Length).Merge()

                        For i = 0 To dr.Length - 1
                            Dim code As String = dr(i)("acc_code").ToString()
                            dr3 = acc_code.Select("acc_code='" & code & "'")
                            If dr3.Length = 0 Then
                                acc_code.Rows.Add(code)
                                dr2 = custdetailsdt.Select("acc_code='" & code & "'")
                                rownum = rownum + 1
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True

                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                ws.Range(rownum, 1, rownum, headerarray.Length).Value = dr(i)("acc_code").ToString() + Space(10) + dr(i)("agentname").ToString()
                                ws.Range(rownum, 1, rownum, headerarray.Length).Merge()

                                For k = 0 To dr2.Length - 1
                                    tage1 = tage1 + Decimal.Parse(dr2(k)("age1"))
                                    tage2 = tage2 + Decimal.Parse(dr2(k)("age2"))
                                    tage3 = tage3 + Decimal.Parse(dr2(k)("age3"))
                                    tage4 = tage4 + Decimal.Parse(dr2(k)("age4"))
                                    tage5 = tage5 + Decimal.Parse(dr2(k)("age5"))
                                    tage6 = tage6 + Decimal.Parse(dr2(k)("age6"))
                                    tage7 = tage7 + Decimal.Parse(dr2(k)("age7"))
                                    tage9 = tage9 + Decimal.Parse(dr2(k)("age9"))
                                    tbal = tbal + Decimal.Parse(dr2(k)("balance"))

                                    'Dim dataarray() As String = {dr2(k)("trantype").ToString() + " " + dr2(k)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr2(k)("trandate").ToString()), "dd/MM/yyyy"), IIf(dr2(k)("pdc") = 0.0, "", dr2(k)("pdc")), IIf(dr2(k)("age9") = 0.0, "-", IIf(dr2(k)("age9") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age9"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age9")).ToString("N" + currDecno.ToString))),
                                    '                           IIf(dr2(k)("age1") = 0.0, "-", IIf(dr2(k)("age1") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age1"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age1")).ToString("N2"))), IIf(dr2(k)("age2") = 0.0, "-", IIf(dr2(k)("age2") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age2"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age2")).ToString("N" + currDecno.ToString))),
                                    '                            IIf(dr2(k)("age3") = 0.0, "-", IIf(dr2(k)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age3")).ToString("N2"))), IIf(dr2(k)("age4") = 0.0, "-", IIf(dr2(k)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age4"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age4")).ToString("N" + currDecno.ToString))),
                                    '                             IIf(dr2(k)("age5") = 0.0, "-", IIf(dr2(k)("age5") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age5"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age5")).ToString("N2"))), IIf(dr2(k)("age6") = 0.0, "-", IIf(dr2(k)("age6") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age6"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("age6")).ToString("N" + currDecno.ToString))),
                                    '                           IIf(dr2(k)("age7") = 0.0, "-", IIf(dr2(k)("age7") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("age7"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr2(k)("age7")).ToString("N2"))), IIf(dr2(k)("balance") < 0.0, "(" & Math.Abs(Decimal.Parse(dr2(k)("balance"))).ToString("N2") & ")", Decimal.Parse(dr2(k)("balance")).ToString("N2"))}
                                    Dim dataarray() As String
                                    If Type = "C" Then
                                        dataarray = {dr2(k)("trantype").ToString() + " " + dr2(k)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr2(k)("trandate").ToString()), "dd/MM/yyyy"), Convert.ToString(dr2(k)("arrivaldate")), Convert.ToString(dr2(k)("canceldate")), IIf(dr2(k)("pdc") = 0.0, "", dr2(k)("pdc")), IIf(dr2(k)("age9") = 0.0, "-", Decimal.Parse(dr2(k)("age9")).ToString(decimalPoint)),
                                                   IIf(dr2(k)("age1") = 0.0, "-", Decimal.Parse(dr2(k)("age1")).ToString(decimalPoint)), IIf(dr2(k)("age2") = 0.0, "-", Decimal.Parse(dr2(k)("age2")).ToString(decimalPoint)), IIf(dr2(k)("age3") = 0.0, "-", Decimal.Parse(dr2(k)("age3")).ToString(decimalPoint)), IIf(dr2(k)("age4") = 0.0, "-", Decimal.Parse(dr2(k)("age4")).ToString(decimalPoint)),
                                                       IIf(dr2(k)("age5") = 0.0, "-", Decimal.Parse(dr2(k)("age5")).ToString(decimalPoint)), IIf(dr2(k)("age6") = 0.0, "-", Decimal.Parse(dr2(k)("age6")).ToString(decimalPoint)),
                                                    IIf(dr2(k)("age7") = 0.0, "-", Decimal.Parse(dr2(k)("age7")).ToString(decimalPoint)), IIf(dr2(k)("balance") = 0.0, "-", Decimal.Parse(dr2(k)("balance")).ToString(decimalPoint))}
                                    Else
                                        dataarray = {dr2(k)("trantype").ToString() + " " + dr2(k)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr2(k)("trandate").ToString()), "dd/MM/yyyy"), IIf(dr2(k)("pdc") = 0.0, "", dr2(k)("pdc")), IIf(dr2(k)("age9") = 0.0, "-", Decimal.Parse(dr2(k)("age9")).ToString(decimalPoint)),
                                                   IIf(dr2(k)("age1") = 0.0, "-", Decimal.Parse(dr2(k)("age1")).ToString(decimalPoint)), IIf(dr2(k)("age2") = 0.0, "-", Decimal.Parse(dr2(k)("age2")).ToString(decimalPoint)), IIf(dr2(k)("age3") = 0.0, "-", Decimal.Parse(dr2(k)("age3")).ToString(decimalPoint)), IIf(dr2(k)("age4") = 0.0, "-", Decimal.Parse(dr2(k)("age4")).ToString(decimalPoint)),
                                                       IIf(dr2(k)("age5") = 0.0, "-", Decimal.Parse(dr2(k)("age5")).ToString(decimalPoint)), IIf(dr2(k)("age6") = 0.0, "-", Decimal.Parse(dr2(k)("age6")).ToString(decimalPoint)),
                                                    IIf(dr2(k)("age7") = 0.0, "-", Decimal.Parse(dr2(k)("age7")).ToString(decimalPoint)), IIf(dr2(k)("balance") = 0.0, "-", Decimal.Parse(dr2(k)("balance")).ToString(decimalPoint))}
                                    End If


                                    rownum = rownum + 1
                                    ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.FontSize = 10
                                    ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                    ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
                                    For j = 0 To dataarray.GetUpperBound(0)
                                        If j = 0 Then
                                            ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                            ws.Cell(rownum, j + 1).Value = dataarray(j)
                                        Else
                                            ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                            If j > 1 AndAlso IsNumeric(dataarray(j)) Then  'AndAlso Not dataarray(j).Equals("-")
                                                If dataarray(j) < 0.0 Then
                                                    ws.Cell(rownum, j + 1).Value = Decimal.Parse(dataarray(j)) ' Math.Abs(Decimal.Parse(dataarray(j))) 'sharfudeen 12/09/2022
                                                    ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat  ' numformat1 'sharfudeen 12/09/2022
                                                Else
                                                    ws.Cell(rownum, j + 1).Value = Decimal.Parse(dataarray(j))
                                                    ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat 'sharfudeen 12/09/2022
                                                End If
                                            Else
                                                ws.Cell(rownum, j + 1).Value = dataarray(j)
                                            End If
                                        End If
                                    Next
                                Next
                                rownum = rownum + 1
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
                                'Dim data1array() As String = {"Total For " + dr(i)("agentname").ToString, IIf(dr(i)("creditlimit") = 0, "", IIf(dr(i)("creditlimit") < 0, "(" & Math.Abs(dr(i)("creditlimit")) & ")", dr(i)("creditlimit"))), IIf(tage9 = 0.0, "-", IIf(tage9 < 0.0, "(" & Math.Abs(tage9).ToString("N" + currDecno.ToString) & ")", tage9.ToString("N" + currDecno.ToString))),
                                '                               IIf(tage1 = 0.0, "-", IIf(tage1 < 0.0, "(" & Math.Abs(tage1).ToString("N" + currDecno.ToString) & ")", tage1.ToString("N" + currDecno.ToString))), IIf(tage2 = 0.0, "-", IIf(tage2 < 0.0, "(" & Math.Abs(tage2).ToString("N" + currDecno.ToString) & ")", tage2.ToString("N" + currDecno.ToString))), IIf(tage3 = 0.0, "-", IIf(tage3 < 0.0, "(" & Math.Abs(tage3).ToString("N" + currDecno.ToString) & ")", tage3.ToString("N" + currDecno.ToString))), IIf(tage4 = 0.0, "-", IIf(tage4 < 0.0, "(" & Math.Abs(tage4).ToString("N" + currDecno.ToString) & ")", tage4.ToString("N" + currDecno.ToString))), IIf(tage5 = 0.0, "-", IIf(tage5 < 0.0, "(" & Math.Abs(tage5).ToString("N" + currDecno.ToString) & ")", tage5.ToString("N" + currDecno.ToString))),
                                '           IIf(tage6 = 0.0, "-", IIf(tage6 < 0.0, "(" & Math.Abs(tage6).ToString("N" + currDecno.ToString) & ")", tage6.ToString("N" + currDecno.ToString))), IIf(tage7 = 0.0, "-", IIf(tage7 < 0.0, "(" & Math.Abs(tage7).ToString("N" + currDecno.ToString) & ")", tage7.ToString("N" + currDecno.ToString))), IIf(tbal < 0.0, "(" & Math.Abs(tbal).ToString("N" + currDecno.ToString) & ")", tbal.ToString("N" + currDecno.ToString))}

                                Dim data1array() As String
                                If Type = "C" Then
                                    data1array = {"Total For " + dr(i)("agentname").ToString(), "", "", IIf(dr(i)("creditlimit") = 0, "", dr(i)("creditlimit")), IIf(tage9 = 0.0, "-", tage9.ToString(decimalPoint)), IIf(tage1 = 0.0, "-", tage1.ToString(decimalPoint)), IIf(tage2 = 0.0, "-", tage2.ToString(decimalPoint)),
                                                    IIf(tage3 = 0.0, "-", tage3.ToString(decimalPoint)), IIf(tage4 = 0.0, "-", tage4.ToString(decimalPoint)), IIf(tage5 = 0.0, "-", tage5.ToString(decimalPoint)), IIf(tage6 = 0.0, "-", tage6.ToString(decimalPoint)), IIf(tage7 = 0.0, "-", tage7.ToString(decimalPoint)), IIf(tbal = 0.0, "-", tbal.ToString(decimalPoint))}
                                Else
                                    data1array = {"Total For " + dr(i)("agentname").ToString(), IIf(dr(i)("creditlimit") = 0, "", dr(i)("creditlimit")), IIf(tage9 = 0.0, "-", tage9.ToString(decimalPoint)), IIf(tage1 = 0.0, "-", tage1.ToString(decimalPoint)), IIf(tage2 = 0.0, "-", tage2.ToString(decimalPoint)),
                                                    IIf(tage3 = 0.0, "-", tage3.ToString(decimalPoint)), IIf(tage4 = 0.0, "-", tage4.ToString(decimalPoint)), IIf(tage5 = 0.0, "-", tage5.ToString(decimalPoint)), IIf(tage6 = 0.0, "-", tage6.ToString(decimalPoint)), IIf(tage7 = 0.0, "-", tage7.ToString(decimalPoint)), IIf(tbal = 0.0, "-", tbal.ToString(decimalPoint))}

                                End If


                                For j = 0 To data1array.GetUpperBound(0)
                                    If j = 0 Then
                                        ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                        ws.Cell(rownum, j + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                                        ws.Cell(rownum, j + 1).Value = data1array(j)
                                    Else
                                        ws.Cell(rownum, j + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                        If j > 1 AndAlso IsNumeric(data1array(j)) Then  'Not data1array(j).Equals("-") AndAlso
                                            If data1array(j) < 0.0 Then
                                                ws.Cell(rownum, j + 1).Value = Decimal.Parse(data1array(j)) ' Math.Abs(Decimal.Parse(data1array(j))) 'sharfudeen 12/09/2022
                                                ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat1 'sharfudeen 12/09/2022
                                            Else
                                                ws.Cell(rownum, j + 1).Value = Decimal.Parse(data1array(j))
                                                ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat 'sharfudeen 12/09/2022
                                            End If
                                        Else
                                            ws.Cell(rownum, j + 1).Value = data1array(j)
                                        End If
                                    End If
                                Next

                                'total for particular grpby
                                gage1 = gage1 + tage1
                                gage2 = gage2 + tage2
                                gage3 = gage3 + tage3
                                gage4 = gage4 + tage4
                                gage5 = gage5 + tage5
                                gage6 = gage6 + tage6
                                gage7 = gage7 + tage7
                                gage9 = gage9 + tage9
                                gbal = gbal + tbal
                                tage1 = 0.0F
                                tage2 = 0.0F
                                tage3 = 0.0F
                                tage4 = 0.0F
                                tage5 = 0.0F
                                tage6 = 0.0F
                                tage7 = 0.0F
                                tage9 = 0.0F
                                tbal = 0.0F
                            End If
                        Next


                        rownum = rownum + 1
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
                        'Dim grpbyarray() As String = {"Total For " + grp_by, "", IIf(gage9 = 0.0, "-", IIf(gage9 < 0.0, "(" & Math.Abs(gage9).ToString("N" + currDecno.ToString) & ")", gage9.ToString("N" + currDecno.ToString))),
                        '                               IIf(gage1 = 0.0, "-", IIf(gage1 < 0.0, "(" & Math.Abs(gage1).ToString("N" + currDecno.ToString) & ")", gage1.ToString("N" + currDecno.ToString))), IIf(gage2 = 0.0, "-", IIf(gage2 < 0.0, "(" & Math.Abs(gage2).ToString("N" + currDecno.ToString) & ")", gage2.ToString("N" + currDecno.ToString))), IIf(gage3 = 0.0, "-", IIf(gage3 < 0.0, "(" & Math.Abs(gage3).ToString("N" + currDecno.ToString) & ")", gage3.ToString("N" + currDecno.ToString))), IIf(gage4 = 0.0, "-", IIf(gage4 < 0.0, "(" & Math.Abs(gage4).ToString("N" + currDecno.ToString) & ")", gage4.ToString("N" + currDecno.ToString))), IIf(gage5 = 0.0, "-", IIf(gage5 < 0.0, "(" & Math.Abs(gage5).ToString("N" + currDecno.ToString) & ")", gage5.ToString("N" + currDecno.ToString))),
                        '           IIf(gage6 = 0.0, "-", IIf(gage6 < 0.0, "(" & Math.Abs(gage6).ToString("N" + currDecno.ToString) & ")", gage6.ToString("N" + currDecno.ToString))), IIf(gage7 = 0.0, "-", IIf(gage7 < 0.0, "(" & Math.Abs(gage7).ToString("N" + currDecno.ToString) & ")", gage7.ToString("N" + currDecno.ToString))), IIf(gbal < 0.0, "(" & Math.Abs(gbal).ToString("N" + currDecno.ToString) & ")", gbal.ToString("N" + currDecno.ToString))}

                        Dim grpbyarray() As String
                        If Type = "C" Then
                            grpbyarray = {"Total For " + grp_by, "", "", "", IIf(gage9 = 0.0, "-", gage9.ToString(decimalPoint)), IIf(gage1 = 0.0, "-", gage1.ToString(decimalPoint)), IIf(gage2 = 0.0, "-", gage2.ToString(decimalPoint)),
                                                     IIf(gage3 = 0.0, "-", gage3.ToString(decimalPoint)), IIf(gage4 = 0.0, "-", gage4.ToString(decimalPoint)), IIf(gage5 = 0.0, "-", gage5.ToString(decimalPoint)), IIf(gage6 = 0.0, "-", gage6.ToString(decimalPoint)), IIf(gage7 = 0.0, "-", gage7.ToString(decimalPoint)), IIf(gbal = 0.0, "-", gbal.ToString(decimalPoint))}
                        Else
                            grpbyarray = {"Total For " + grp_by, "", IIf(gage9 = 0.0, "-", gage9.ToString(decimalPoint)), IIf(gage1 = 0.0, "-", gage1.ToString(decimalPoint)), IIf(gage2 = 0.0, "-", gage2.ToString(decimalPoint)),
                                                     IIf(gage3 = 0.0, "-", gage3.ToString(decimalPoint)), IIf(gage4 = 0.0, "-", gage4.ToString(decimalPoint)), IIf(gage5 = 0.0, "-", gage5.ToString(decimalPoint)), IIf(gage6 = 0.0, "-", gage6.ToString(decimalPoint)), IIf(gage7 = 0.0, "-", gage7.ToString(decimalPoint)), IIf(gbal = 0.0, "-", gbal.ToString(decimalPoint))}
                        End If
                        For j = 0 To grpbyarray.GetUpperBound(0)
                            If j = 0 Then
                                ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                ws.Cell(rownum, j + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                                ws.Cell(rownum, j + 1).Value = grpbyarray(j)
                            Else
                                ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                If j > 1 AndAlso IsNumeric(grpbyarray(j)) Then  'Not grpbyarray(j).Equals("-") 
                                    If grpbyarray(j) < 0.0 Then
                                        ws.Cell(rownum, j + 1).Value = Decimal.Parse(grpbyarray(j)) ' Math.Abs(Decimal.Parse(grpbyarray(j))) 'sharfudeen 12/09/2022
                                        ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat1 'sharfudeen 12/09/2022
                                    Else
                                        ws.Cell(rownum, j + 1).Value = Decimal.Parse(grpbyarray(j))
                                        ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat 'sharfudeen 12/09/2022
                                    End If
                                Else
                                    ws.Cell(rownum, j + 1).Value = grpbyarray(j)
                                End If
                            End If
                        Next
                    Else
                        rownum = rownum + 1
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True

                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Value = dr(0)("acc_code").ToString() + Space(10) + dr(0)("agentname").ToString()
                        ws.Range(rownum, 1, rownum, headerarray.Length).Merge()
                        For i = 0 To dr.Length - 1
                            gage1 = gage1 + Decimal.Parse(dr(i)("age1"))
                            gage2 = gage2 + Decimal.Parse(dr(i)("age2"))
                            gage3 = gage3 + Decimal.Parse(dr(i)("age3"))
                            gage4 = gage4 + Decimal.Parse(dr(i)("age4"))
                            gage5 = gage5 + Decimal.Parse(dr(i)("age5"))
                            gage6 = gage6 + Decimal.Parse(dr(i)("age6"))
                            gage7 = gage7 + Decimal.Parse(dr(i)("age7"))
                            gage9 = gage9 + Decimal.Parse(dr(i)("age9"))
                            gbal = gbal + Decimal.Parse(dr(i)("balance"))
                            'Dim dataarray() As String = {dr(i)("trantype").ToString() + " " + dr(i)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr(i)("trandate").ToString()), "dd/MM/yyyy"), IIf(dr(i)("pdc") = 0.0, "", dr(i)("pdc")), IIf(dr(i)("age9") = 0.0, "-", IIf(dr(i)("age9") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age9"))).ToString("N") & ")", Decimal.Parse(dr(i)("age9")).ToString("N" + currDecno.ToString))),
                            '                           IIf(dr(i)("age1") = 0.0, "-", IIf(dr(i)("age1") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age1"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age1")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age2") = 0.0, "-", IIf(dr(i)("age2") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age2"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age2")).ToString("N" + currDecno.ToString))),
                            '                            IIf(dr(i)("age3") = 0.0, "-", IIf(dr(i)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age3")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age4") = 0.0, "-", IIf(dr(i)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age4"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age4")).ToString("N" + currDecno.ToString))),
                            '                            IIf(dr(i)("age3") = 0.0, "-", IIf(dr(i)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age3")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age4") = 0.0, "-", IIf(dr(i)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age4"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age4")).ToString("N" + currDecno.ToString))),
                            '                            IIf(dr(i)("age3") = 0.0, "-", IIf(dr(i)("age3") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age3"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age3")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age4") = 0.0, "-", IIf(dr(i)("age4") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age4"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age4")).ToString("N" + currDecno.ToString))),
                            '                             IIf(dr(i)("age5") = 0.0, "-", IIf(dr(i)("age5") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age5"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age5")).ToString("N" + currDecno.ToString))), IIf(dr(i)("age6") = 0.0, "-", IIf(dr(i)("age6") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age6"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age6")).ToString("N" + currDecno.ToString))),
                            '                           IIf(dr(i)("age7") = 0.0, "-", IIf(dr(i)("age7") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("age7"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("age7")).ToString("N" + currDecno.ToString))), IIf(dr(i)("balance") < 0.0, "(" & Math.Abs(Decimal.Parse(dr(i)("balance"))).ToString("N" + currDecno.ToString) & ")", Decimal.Parse(dr(i)("balance")).ToString("N" + currDecno.ToString))}

                            Dim dataarray() As String
                            If Type = "C" Then
                                dataarray = {dr(i)("trantype").ToString() + " " + dr(i)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr(i)("trandate").ToString()), "dd/MM/yyyy"), Convert.ToString(dr(i)("arrivaldate")), Convert.ToString(dr(i)("canceldate")), IIf(dr(i)("pdc") = 0.0, "", dr(i)("pdc")), IIf(dr(i)("age9") = 0.0, "-", Decimal.Parse(dr(i)("age9")).ToString(decimalPoint)),
                                                    IIf(dr(i)("age1") = 0.0, "-", Decimal.Parse(dr(i)("age1")).ToString(decimalPoint)), IIf(dr(i)("age2") = 0.0, "-", Decimal.Parse(dr(i)("age2")).ToString(decimalPoint)), IIf(dr(i)("age3") = 0.0, "-", Decimal.Parse(dr(i)("age3")).ToString(decimalPoint)), IIf(dr(i)("age4") = 0.0, "-", Decimal.Parse(dr(i)("age4")).ToString(decimalPoint)),
                                                        IIf(dr(i)("age5") = 0.0, "-", Decimal.Parse(dr(i)("age5")).ToString(decimalPoint)), IIf(dr(i)("age6") = 0.0, "-", Decimal.Parse(dr(i)("age6")).ToString(decimalPoint)),
                                                     IIf(dr(i)("age7") = 0.0, "-", Decimal.Parse(dr(i)("age7")).ToString(decimalPoint)), IIf(dr(i)("balance") = 0.0, "-", Decimal.Parse(dr(i)("balance")).ToString(decimalPoint))}
                            Else
                                dataarray = {dr(i)("trantype").ToString() + " " + dr(i)("tranid").ToString() + " " + Format(Convert.ToDateTime(dr(i)("trandate").ToString()), "dd/MM/yyyy"), IIf(dr(i)("pdc") = 0.0, "", dr(i)("pdc")), IIf(dr(i)("age9") = 0.0, "-", Decimal.Parse(dr(i)("age9")).ToString(decimalPoint)),
                                                    IIf(dr(i)("age1") = 0.0, "-", Decimal.Parse(dr(i)("age1")).ToString(decimalPoint)), IIf(dr(i)("age2") = 0.0, "-", Decimal.Parse(dr(i)("age2")).ToString(decimalPoint)), IIf(dr(i)("age3") = 0.0, "-", Decimal.Parse(dr(i)("age3")).ToString(decimalPoint)), IIf(dr(i)("age4") = 0.0, "-", Decimal.Parse(dr(i)("age4")).ToString(decimalPoint)),
                                                        IIf(dr(i)("age5") = 0.0, "-", Decimal.Parse(dr(i)("age5")).ToString(decimalPoint)), IIf(dr(i)("age6") = 0.0, "-", Decimal.Parse(dr(i)("age6")).ToString(decimalPoint)),
                                                     IIf(dr(i)("age7") = 0.0, "-", Decimal.Parse(dr(i)("age7")).ToString(decimalPoint)), IIf(dr(i)("balance") = 0.0, "-", Decimal.Parse(dr(i)("balance")).ToString(decimalPoint))}

                            End If

                            rownum = rownum + 1
                            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.FontSize = 10
                            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True
                            For j = 0 To dataarray.GetUpperBound(0)
                                If j = 0 Then
                                    ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                    ws.Cell(rownum, j + 1).Value = dataarray(j)
                                Else
                                    ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                    If j > 1 AndAlso IsNumeric(dataarray(j)) Then  'dataarray(j).Equals("-")
                                        If dataarray(j) < 0.0 Then
                                            ws.Cell(rownum, j + 1).Value = Decimal.Parse(dataarray(j)) ' Math.Abs(Decimal.Parse(dataarray(j))) 'sharfudeen 12/09/2022
                                            ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat1 'sharfudeen 12/09/2022
                                        Else
                                            ws.Cell(rownum, j + 1).Value = Decimal.Parse(dataarray(j))
                                            ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat 'sharfudeen 12/09/2022
                                        End If

                                    Else
                                        ws.Cell(rownum, j + 1).Value = dataarray(j)
                                    End If
                                End If
                            Next
                        Next

                        'Dim data1array() As String = {"Total For " + dr(0)("agentname").ToString(), IIf(dr(0)("creditlimit") = 0, "", IIf(dr(0)("creditlimit") < 0, "(" & Math.Abs(dr(0)("creditlimit")) & ")", dr(0)("creditlimit"))), IIf(gage9 = 0.0, "-", IIf(gage9 < 0.0, "(" & Math.Abs(gage9).ToString("N" + currDecno.ToString) & ")", gage9.ToString("N" + currDecno.ToString))),
                        '                               IIf(gage1 = 0.0, "-", IIf(gage1 < 0.0, "(" & Math.Abs(gage1).ToString("N" + currDecno.ToString) & ")", gage1.ToString("N" + currDecno.ToString))), IIf(gage2 = 0.0, "-", IIf(gage2 < 0.0, "(" & Math.Abs(gage2).ToString("N" + currDecno.ToString) & ")", gage2.ToString("N" + currDecno.ToString))), IIf(gage3 = 0.0, "-", IIf(gage3 < 0.0, "(" & Math.Abs(gage3).ToString("N" + currDecno.ToString) & ")", gage3.ToString("N" + currDecno.ToString))), IIf(gage4 = 0.0, "-", IIf(gage4 < 0.0, "(" & Math.Abs(gage4).ToString("N" + currDecno.ToString) & ")", gage4.ToString("N" + currDecno.ToString))), IIf(gage5 = 0.0, "-", IIf(gage5 < 0.0, "(" & Math.Abs(gage5).ToString("N" + currDecno.ToString) & ")", gage5.ToString("N" + currDecno.ToString))),
                        '           IIf(gage6 = 0.0, "-", IIf(gage6 < 0.0, "(" & Math.Abs(gage6).ToString("N" + currDecno.ToString) & ")", gage6.ToString("N" + currDecno.ToString))), IIf(gage7 = 0.0, "-", IIf(gage7 < 0.0, "(" & Math.Abs(gage7).ToString("N" + currDecno.ToString) & ")", gage7.ToString("N" + currDecno.ToString))), IIf(gbal < 0.0, "(" & Math.Abs(gbal).ToString("N" + currDecno.ToString) & ")", gbal.ToString("N" + currDecno.ToString))}

                        Dim data1array() As String
                        If Type = "C" Then
                            data1array = {"Total For " + dr(0)("agentname").ToString(), "", "", IIf(dr(0)("creditlimit") = 0, "", dr(0)("creditlimit")), IIf(gage9 = 0.0, "-", gage9.ToString(decimalPoint)), IIf(gage1 = 0.0, "-", gage1.ToString(decimalPoint)), IIf(gage2 = 0.0, "-", gage2.ToString(decimalPoint)),
                                                     IIf(gage3 = 0.0, "-", gage3.ToString(decimalPoint)), IIf(gage4 = 0.0, "-", gage4.ToString(decimalPoint)), IIf(gage5 = 0.0, "-", gage5.ToString(decimalPoint)), IIf(gage6 = 0.0, "-", gage6.ToString(decimalPoint)), IIf(gage7 = 0.0, "-", gage7.ToString(decimalPoint)), IIf(gbal = 0.0, "-", gbal.ToString(decimalPoint))}
                        Else
                            data1array = {"Total For " + dr(0)("agentname").ToString(), IIf(dr(0)("creditlimit") = 0, "", dr(0)("creditlimit")), IIf(gage9 = 0.0, "-", gage9.ToString(decimalPoint)), IIf(gage1 = 0.0, "-", gage1.ToString(decimalPoint)), IIf(gage2 = 0.0, "-", gage2.ToString(decimalPoint)),
                                                     IIf(gage3 = 0.0, "-", gage3.ToString(decimalPoint)), IIf(gage4 = 0.0, "-", gage4.ToString(decimalPoint)), IIf(gage5 = 0.0, "-", gage5.ToString(decimalPoint)), IIf(gage6 = 0.0, "-", gage6.ToString(decimalPoint)), IIf(gage7 = 0.0, "-", gage7.ToString(decimalPoint)), IIf(gbal = 0.0, "-", gbal.ToString(decimalPoint))}
                        End If

                        rownum = rownum + 1
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True

                        For j = 0 To data1array.GetUpperBound(0)
                            If j = 0 Then
                                ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                ws.Cell(rownum, j + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                                ws.Cell(rownum, j + 1).Value = data1array(j)
                            Else
                                ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                If j > 1 AndAlso IsNumeric(data1array(j)) Then  'Not data1array(j).Equals("-")
                                    If data1array(j) < 0.0 Then
                                        ws.Cell(rownum, j + 1).Value = Decimal.Parse(data1array(j)) ' Math.Abs(Decimal.Parse(data1array(j))) 'sharfudeen 12/09/2022
                                        ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat1 'sharfudeen 12/09/2022
                                    Else
                                        ws.Cell(rownum, j + 1).Value = Decimal.Parse(data1array(j))
                                        ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat 'sharfudeen 12/09/2022
                                    End If
                                Else
                                    ws.Cell(rownum, j + 1).Value = data1array(j)
                                End If
                            End If
                        Next
                    End If
                End If
                fage1 = fage1 + gage1
                fage2 = fage2 + gage2
                fage3 = fage3 + gage3
                fage4 = fage4 + gage4
                fage5 = fage5 + gage5
                fage6 = fage6 + gage6
                fage7 = fage7 + gage7
                fage9 = fage9 + gage9
                fbal = fbal + gbal
                gage1 = 0.0F
                gage2 = 0.0F
                gage3 = 0.0F
                gage4 = 0.0F
                gage5 = 0.0F
                gage6 = 0.0F
                gage7 = 0.0F
                gage9 = 0.0F
                gbal = 0.0F
            Next

            Dim data2array() As String
            If Type = "C" Then
                data2array = {"Total in " + currcode, "", "", "", IIf(fage9 = 0.0, "-", fage9.ToString(decimalPoint)), IIf(fage1 = 0.0, "-", fage1.ToString(decimalPoint)), IIf(fage2 = 0.0, "-", fage2.ToString(decimalPoint)), IIf(fage3 = 0.0, "-", fage3.ToString(decimalPoint)), IIf(fage4 = 0.0, "-", fage4.ToString(decimalPoint)),
                IIf(fage5 = 0.0, "-", fage5.ToString(decimalPoint)), IIf(fage6 = 0.0, "-", fage6.ToString(decimalPoint)), IIf(fage7 = 0.0, "-", fage7.ToString(decimalPoint)), IIf(fbal = 0.0, "-", fbal.ToString(decimalPoint))}
            Else
                data2array = {"Total in " + currcode, "", IIf(fage9 = 0.0, "-", fage9.ToString(decimalPoint)), IIf(fage1 = 0.0, "-", fage1.ToString(decimalPoint)), IIf(fage2 = 0.0, "-", fage2.ToString(decimalPoint)), IIf(fage3 = 0.0, "-", fage3.ToString(decimalPoint)), IIf(fage4 = 0.0, "-", fage4.ToString(decimalPoint)),
                IIf(fage5 = 0.0, "-", fage5.ToString(decimalPoint)), IIf(fage6 = 0.0, "-", fage6.ToString(decimalPoint)), IIf(fage7 = 0.0, "-", fage7.ToString(decimalPoint)), IIf(fbal = 0.0, "-", fbal.ToString(decimalPoint))}
            End If

            rownum = rownum + 1
            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True

            For j = 0 To data2array.GetUpperBound(0)
                If j = 0 Then
                    ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Cell(rownum, j + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                    ws.Cell(rownum, j + 1).Value = data2array(j)
                Else
                    ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    If j > 1 AndAlso IsNumeric(data2array(j)) Then   'Not data2array(j).Equals("-")
                        If data2array(j) < 0.0 Then
                            ws.Cell(rownum, j + 1).Value = Decimal.Parse(data2array(j))  ' Math.Abs(Decimal.Parse(data2array(j)))  'sharfudeen 12/09/2022
                            ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat1 'sharfudeen 12/09/2022
                        Else
                            ws.Cell(rownum, j + 1).Value = Decimal.Parse(data2array(j))
                            ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat  ' numformat 'sharfudeen 12/09/2022
                        End If
                    Else
                        ws.Cell(rownum, j + 1).Value = data2array(j)
                    End If
                End If
            Next

            Dim data3array() As String
            If Type = "C" Then
                data3array = {"Final Total", "", "", "", IIf(fage9 = 0.0, "-", fage9.ToString(decimalPoint)), IIf(fage1 = 0.0, "-", fage1.ToString(decimalPoint)), IIf(fage2 = 0.0, "-", fage2.ToString(decimalPoint)), IIf(fage3 = 0.0, "-", fage3.ToString(decimalPoint)), IIf(fage4 = 0.0, "-", fage4.ToString(decimalPoint)),
                IIf(fage5 = 0.0, "-", fage5.ToString(decimalPoint)), IIf(fage6 = 0.0, "-", fage6.ToString(decimalPoint)), IIf(fage7 = 0.0, "-", fage7.ToString(decimalPoint)), IIf(fbal = 0.0, "-", fbal.ToString(decimalPoint))}
            Else
                data3array = {"Final Total", "", IIf(fage9 = 0.0, "-", fage9.ToString(decimalPoint)), IIf(fage1 = 0.0, "-", fage1.ToString(decimalPoint)), IIf(fage2 = 0.0, "-", fage2.ToString(decimalPoint)), IIf(fage3 = 0.0, "-", fage3.ToString(decimalPoint)), IIf(fage4 = 0.0, "-", fage4.ToString(decimalPoint)),
                IIf(fage5 = 0.0, "-", fage5.ToString(decimalPoint)), IIf(fage6 = 0.0, "-", fage6.ToString(decimalPoint)), IIf(fage7 = 0.0, "-", fage7.ToString(decimalPoint)), IIf(fbal = 0.0, "-", fbal.ToString(decimalPoint))}
            End If

            rownum = rownum + 1
            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Font.SetBold().Font.FontSize = 9
            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, headerarray.Length).Style.Alignment.WrapText = True

            For j = 0 To data3array.GetUpperBound(0)
                If j = 0 Then
                    ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Cell(rownum, j + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                    ws.Cell(rownum, j + 1).Value = data3array(j)
                Else
                    ws.Cell(rownum, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    If j > 1 AndAlso IsNumeric(data3array(j)) Then   'Not data3array(j).Equals("-")
                        If data3array(j) < 0.0 Then
                            ws.Cell(rownum, j + 1).Value = Decimal.Parse(data3array(j)) ' Math.Abs(Decimal.Parse(data3array(j))) 'sharfudeen 12/09/2022
                            ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat1 'sharfudeen 12/09/2022
                        Else
                            ws.Cell(rownum, j + 1).Value = Decimal.Parse(data3array(j))
                            ws.Cell(rownum, j + 1).Style.NumberFormat.Format = Fullnumformat ' numformat 'sharfudeen 12/09/2022
                        End If
                    Else
                        ws.Cell(rownum, j + 1).Value = data3array(j)
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
End Class
