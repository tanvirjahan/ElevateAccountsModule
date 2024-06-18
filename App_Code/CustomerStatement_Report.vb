Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Web.HttpResponse
Imports System.Web.UI
Imports System.Data
Imports System.Linq
Imports System.Data.SqlClient
Imports ClosedXML.Excel
Imports System.IO
Public Class CustomerStatement_Report
    Inherits System.Web.UI.Page
#Region "Global Variable"
    Dim objUtils As New clsUtils 
    Dim NormalFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, basecolor.BLACK)
    Dim datafont As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, basecolor.BLACK)
    Dim datafontBold As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, basecolor.BLACK)
    Dim HeadingFont As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, basecolor.BLACK)
    Dim basecolor As BaseColor = New BaseColor(211, 211, 211)
    Dim BalFont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, basecolor.BLACK)
    Dim TitleFont As Font = FontFactory.GetFont("Times New Roman", 12, Font.BOLD, basecolor.BLACK)
    Dim NormalFontBold As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, basecolor.BLACK)
    Dim custSupType, contact1, DecimalPoint, DecimalPoint1 As String
    Dim acrlimit, cdays, finalcredit, finaldebit, totalbalances As Decimal
    Dim documentWidth As Single = 770.0F
    Dim currency As String
    Dim Month, reportfilter As String
    Dim agebalance, age9, age1, age2, age3, age4, age5, age6, age1bal As Decimal
    Dim currDecno As Integer
    Dim phrase As Phrase = Nothing
    Dim cell As PdfPCell = Nothing
    Dim rptcompanyname As String
#End Region
    Public Sub CreatePdf(ByVal reportsType As String, ByVal rptfilter As String, ByVal datetype As Integer, ByVal fromdate As String, ByVal todate As String, ByVal Type As String, ByVal currflg As Integer, ByVal fromacct As String, ByVal toacct As String, ByVal fromcontrol As String, ByVal tocontrol As String,
                         ByVal fromcat As String, ByVal tocat As String, ByVal fromcity As String, ByVal tocity As String, ByVal fromctry As String, ByVal toctry As String,
                                                       ByVal agingtype As Integer, ByVal pdcyesno As Integer, ByVal includezero As Integer, ByVal summdet As Integer, ByVal web As Integer, ByVal divcode As String, ByVal custgroup_sp_type As String, ByVal inclproforma As Integer, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal rpttype As String, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try

            If divcode <> "" Then
                rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            Month = Format(Convert.ToDateTime(todate), "MM")
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            custSupType = IIf(Type = "C", "CUSTOMER STATEMENT", IIf(Type = "S", "SUPPLIER STATEMENT", "SUPPLIER AGENT STATEMENT"))

            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_statement_party", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.Int)).Value = datetype
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
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
            mySqlCmd.Parameters.Add(New SqlParameter("@pdcyesno", SqlDbType.Int)).Value = pdcyesno
            mySqlCmd.Parameters.Add(New SqlParameter("@includezero", SqlDbType.Int)).Value = includezero
            mySqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.Int)).Value = web
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type
            mySqlCmd.Parameters.Add(New SqlParameter("@inclproforma", SqlDbType.Int)).Value = inclproforma
            mySqlCmd.CommandTimeout = 0
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            Dim conn As New SqlConnection
            Dim SqlCmd As New SqlCommand
            Dim DataAdapter As New SqlDataAdapter
            Dim ds2 As New DataSet
            conn = clsDBConnect.dbConnectionnew("strDBConnection")
            SqlCmd = New SqlCommand("sp_statement_partyaging", conn)
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            SqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Char)).Value = Type
            SqlCmd.Parameters.Add(New SqlParameter("@currflg", SqlDbType.Int)).Value = currflg
            SqlCmd.Parameters.Add(New SqlParameter("@fromacct", SqlDbType.VarChar, 20)).Value = fromacct
            SqlCmd.Parameters.Add(New SqlParameter("@toacct", SqlDbType.VarChar, 20)).Value = toacct
            SqlCmd.Parameters.Add(New SqlParameter("@fromcontrol", SqlDbType.VarChar, 20)).Value = fromcontrol
            SqlCmd.Parameters.Add(New SqlParameter("@tocontrol", SqlDbType.VarChar, 20)).Value = tocontrol
            SqlCmd.Parameters.Add(New SqlParameter("@fromcat", SqlDbType.VarChar, 20)).Value = fromcat
            SqlCmd.Parameters.Add(New SqlParameter("@tocat", SqlDbType.VarChar, 20)).Value = tocat
            SqlCmd.Parameters.Add(New SqlParameter("@fromcity", SqlDbType.VarChar, 20)).Value = fromcity
            SqlCmd.Parameters.Add(New SqlParameter("@tocity", SqlDbType.VarChar, 20)).Value = tocity
            SqlCmd.Parameters.Add(New SqlParameter("@fromctry", SqlDbType.VarChar, 20)).Value = fromctry
            SqlCmd.Parameters.Add(New SqlParameter("@toctry", SqlDbType.VarChar, 20)).Value = toctry
            SqlCmd.Parameters.Add(New SqlParameter("@agingtype", SqlDbType.Int)).Value = agingtype
            SqlCmd.Parameters.Add(New SqlParameter("@summdet", SqlDbType.Int)).Value = summdet
            SqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.Int)).Value = web
            SqlCmd.Parameters.Add(New SqlParameter("@agasondate", SqlDbType.DateTime)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            SqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            SqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type
            SqlCmd.Parameters.Add(New SqlParameter("@inclproforma", SqlDbType.Int)).Value = inclproforma
            SqlCmd.CommandTimeout = 0
            DataAdapter.SelectCommand = SqlCmd
            DataAdapter.Fill(ds2)
            Dim detailsdt As New DataTable
            detailsdt = ds2.Tables(0)

            Dim dt = New DataTable()
            Dim acc = New DataColumn("acc_code", GetType(String))
            dt.Columns.Add(acc)


            Dim agents As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim strSql As String = "SELECT * FROM dbo.agentmast"
            Using dad As New SqlDataAdapter(strSql, conn1)
                dad.Fill(agents)
            End Using



            'If currflg = 0 Then
            '    currency = "USD"
            'Else
            '    Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
            '    currency = c
            'End If
            'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)

            reportfilter = rptfilter
            If reportsType = "excel" Then

                ExcelReport(custdetailsdt, dt, agents, detailsdt, fromdate, todate, datetype, agingtype, Type, currflg, bytes)

            Else
                FontFactory.RegisterDirectories()
                Dim pdfdoc As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
                pdfdoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)

                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(pdfdoc, memoryStream)
                    pdfdoc.Open()
                    If custdetailsdt.Rows.Count > 0 Then
                        If Type = "S" Then
                            SupplierStament(custdetailsdt, dt, agents, detailsdt, fromdate, todate, datetype, agingtype, Type, pdfdoc, currflg)

                        Else
                            custdetailsdt.DefaultView.Sort = "acc_code ASC"
                            custdetailsdt = custdetailsdt.DefaultView.ToTable

                            For Each Customer_Statement In custdetailsdt.Rows
                                Dim decCurreccy As String
                                '07/01/2019

                                currency = Customer_Statement("currcode").ToString()
                                If currflg = 0 Then
                                    decCurreccy = currency
                                Else
                                    decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                                End If
                                currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)



                                ''added param 20/11/2018
                                'If currflg = 0 Then
                                '    currency = Customer_Statement("currcode").ToString()
                                '    currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
                                'End If
                                Dim acccode As String = Customer_Statement("acc_code").ToString()
                                Dim accname As String = Customer_Statement("accname").ToString()
                                Dim crlimit As String = Customer_Statement("crlimit").ToString()
                                Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
                                Dim k As Integer
                                Dim dr() As System.Data.DataRow
                                Dim dr1() As System.Data.DataRow
                                Dim agentdet() As System.Data.DataRow
                                dr1 = dt.Select("acc_code='" & acccode & "'")
                                If dr1.Length = 0 Then
                                    dt.Rows.Add(acccode)
                                    dr = custdetailsdt.Select("acc_code='" & acccode & "'")
                                    agentdet = agents.Select("agentcode='" & acccode & "'")
                                    Dim dr3() As System.Data.DataRow
                                    dr3 = detailsdt.Select("acc_code='" & acccode & "'")
                                    'If dr3.Length = 0 Then
                                    '    Dim x As Integer = 1
                                    'End If
                                    Dim logo As PdfPTable = Nothing
                                    logo = New PdfPTable(1)
                                    logo.TotalWidth = documentWidth
                                    logo.LockedWidth = True
                                    logo.SetWidths(New Single() {1.0F})
                                    logo.Complete = False
                                    logo.SplitRows = False
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                                    logo.AddCell(cell)
                                    'company name
                                    If divcode = "02" Then
                                        cell = ImageCell("~/Images/logo.jpg", 80.0F, PdfPCell.ALIGN_LEFT)
                                    Else
                                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                                    End If
                                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                                    cell.Colspan = 2
                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 4
                                    logo.AddCell(cell)
                                    pdfdoc.Add(logo)

                                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                                    tblTitle.SetWidths(New Single() {1.0F})
                                    tblTitle.TotalWidth = documentWidth
                                    tblTitle.LockedWidth = True
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(custSupType & Environment.NewLine & vbLf & reportfilter, TitleFont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.PaddingBottom = 3.0F
                                    tblTitle.AddCell(cell)
                                    tblTitle.SpacingBefore = 7
                                    pdfdoc.Add(tblTitle)
                                    Dim overdue As Decimal
                                    Dim l = agentdet.Length

                                    If agentdet.Length > 0 Then
                                        If IsDBNull(agentdet(0)("crlimit")) Then agentdet(0)("crlimit") = 0
                                        If agentdet(0)("crlimit") Then
                                            Dim overbal As Decimal
                                            If dr3.Length > 0 Then
                                                overbal = Decimal.Parse(dr3(0)("balance"))
                                            Else
                                                overbal = 0
                                            End If
                                            Dim overcr As Decimal = Decimal.Parse(agentdet(0)("crlimit"))
                                            overdue = Decimal.Subtract(overbal, overcr)
                                            contact1 = agentdet(0)("contact1").ToString()
                                            acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
                                            cdays = Decimal.Parse(agentdet(0)("crdays"))
                                        Else
                                            If dr3.Length > 0 Then
                                                overdue = Decimal.Parse(dr3(0)("balance"))
                                            Else
                                                overdue = 0
                                            End If
                                        End If
                                    Else
                                        Dim overbal As Decimal = Decimal.Parse(dr3(0)("balance"))
                                        Dim overcr As Decimal = 0
                                        overdue = Decimal.Subtract(overbal, overcr)
                                        contact1 = String.Empty
                                        crlimit = 0
                                        cdays = 0
                                    End If


                                    Dim tblcommon As PdfPTable = New PdfPTable(2)
                                    tblcommon.SetWidths(New Single() {0.5F, 0.5F})
                                    tblcommon.TotalWidth = documentWidth
                                    tblcommon.LockedWidth = True
                                    Dim tbl As PdfPTable = New PdfPTable(1)
                                    phrase = New Phrase()
                                    Dim juniperId As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_agentCode from int_agentmast where agentcode='" + Customer_Statement("acc_code").ToString() + "'")
                                    phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() & "                  ", NormalFont))
                                    phrase.Add(New Chunk("JUNIPER REF NO" & Space(3) & ":" & Space(5) & juniperId & Environment.NewLine & vbLf, NormalFont))
                                    phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Convert.ToDateTime(todate).ToString("dd/MM/yyyy") + Environment.NewLine & vbLf, NormalFont))
                                    phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
                                    phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
                                    phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
                                    phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.PaddingBottom = 3.0F
                                    tbl.AddCell(cell)
                                    tblcommon.AddCell(tbl)
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("Credit Limit" & "      " & ":" & Space(5) & acrlimit.ToString() + Environment.NewLine & vbLf, NormalFont))
                                    phrase.Add(New Chunk("Credit Days" & "      " & ":" & Space(5) & cdays.ToString() + Environment.NewLine & vbLf, NormalFont))
                                    Dim overdueAmt As String = IIf(overdue <= 0, Decimal.Parse((Math.Abs(overdue))).ToString("N" + currDecno.ToString) + IIf(overdue = 0, "", " Cr"), overdue.ToString("N" + currDecno.ToString) + " Dr")
                                    phrase.Add(New Chunk("OverDue" & "          " & ":" & Space(5) & overdueAmt + Environment.NewLine & vbLf, NormalFont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingLeft = 235.0F
                                    tblcommon.AddCell(cell)
                                    tblcommon.SpacingBefore = 7
                                    pdfdoc.Add(tblcommon)


                                    Dim desc As PdfPTable = New PdfPTable(2)
                                    desc.SetWidths(New Single() {0.82F, 0.18F})
                                    desc.TotalWidth = documentWidth
                                    desc.LockedWidth = True
                                    phrase = New Phrase()
                                    If datetype <> 0 Then
                                        phrase.Add(New Chunk("Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
                                    Else
                                        phrase.Add(New Chunk("Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
                                    End If
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                    cell.PaddingBottom = 3.0F
                                    desc.AddCell(cell)
                                    phrase = New Phrase()
                                    Dim curr As PdfPTable = New PdfPTable(1)
                                    phrase.Add(New Chunk("Currency" + "         " + ":" + Space(7) + decCurreccy, NormalFont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                                    cell.PaddingBottom = 3.0F
                                    cell.PaddingRight = 15.0F
                                    curr.AddCell(cell)
                                    desc.AddCell(curr)
                                    desc.SpacingBefore = 7
                                    pdfdoc.Add(desc)


                                    Dim balance As PdfPTable = New PdfPTable(1)
                                    balance.SetWidths(New Single() {1.0F})
                                    balance.TotalWidth = documentWidth
                                    balance.LockedWidth = True
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("We would appreciate if you could settle the balance due at the earliest", BalFont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                    cell.PaddingBottom = 3.0F
                                    balance.AddCell(cell)
                                    balance.SpacingBefore = 7
                                    pdfdoc.Add(balance)

                                    Dim maintbl As PdfPTable = New PdfPTable(10)
                                    Dim arrow2() As String

                                    maintbl.SetWidths(New Single() {0.075F, 0.1F, 0.12F, 0.045F, 0.14F, 0.2F, 0.08F, 0.08F, 0.08F, 0.08F})
                                    maintbl.TotalWidth = documentWidth
                                    maintbl.LockedWidth = True
                                    arrow2 = {"TRAN DATE", "INV.NO", "JUNIPER REF NO", "TYPE", "AGENT REF.", "GUEST/SERVICE DETAILS",
                                                               "DEBIT", "CREDIT", "BALANCE", "CUMULATIVE BALANCE"}

                                    For i = 0 To arrow2.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrow2(i), HeadingFont))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        maintbl.AddCell(cell)
                                    Next
                                    maintbl.SpacingBefore = 7
                                    pdfdoc.Add(maintbl)
                                    Dim gpbyMonth As DataTable
                                    Dim arrow3() As String = Nothing
                                    Dim tdate As String = Nothing
                                    Dim arr3index As Integer
                                    Dim cumBalance As Decimal
                                    Dim totaldebit, totalcredit, debits, credits As Decimal
                                    finalcredit = 0
                                    finaldebit = 0

                                    If dr.Length > 0 Then
                                        gpbyMonth = dr.CopyToDataTable()
                                    End If

                                    'changed by mohamed on 15/03/2022
                                    'Dim group As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)
                                    Dim group As IEnumerable(Of IGrouping(Of String, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").ToString("yyyy/MM"))

                                    cumBalance = 0
                                    For Each gpdata In group
                                        Dim perticulars As String

                                        totalcredit = 0
                                        totaldebit = 0
                                        totalbalances = 0
                                        Dim trantypes As String

                                        'changed by mohamed on 15/03/2022
                                        Dim ldt As DataTable = gpdata.CopyToDataTable
                                        Dim ldtv As DataView = ldt.DefaultView
                                        ldtv.Sort = "trandate,tranid,acc_gl_code,acc_type,acc_code,mode"

                                        For Each row As DataRow In ldtv.ToTable.Rows

                                            Dim data As PdfPTable = New PdfPTable(10)
                                            data.SetWidths(New Single() {0.075F, 0.1F, 0.12F, 0.045F, 0.14F, 0.2F, 0.08F, 0.08F, 0.08F, 0.08F})

                                            data.TotalWidth = documentWidth
                                            data.LockedWidth = True
                                            trantypes = row("trantype").ToString()

                                            finalcredit = Decimal.Parse(row("credit")) + finalcredit
                                            finaldebit = Decimal.Parse(row("debit")) + finaldebit
                                            debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
                                            credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
                                            totalcredit = totalcredit + credits
                                            totaldebit = totaldebit + debits
                                            totalbalances = totalbalances + (debits - credits)
                                            cumBalance = cumBalance + (debits - credits)
                                            Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
                                            tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
                                            'row("arrdate").ToString(), row("depdate").ToString(),
                                            arrow3 = {tdate, row("tranid").ToString(), row("extId").ToString(), row("trantype").ToString(), row("reconfno").ToString(), row("particulars").ToString(),
                                            IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)), (debits - credits).ToString("N" + currDecno.ToString),
                                                      IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) + IIf(cumBalance = 0, "", " Cr"), cumBalance.ToString("N" + currDecno.ToString) + " Dr")}
                                            For k = 0 To arrow3.Length - 1

                                                phrase = New Phrase()
                                                phrase.Add(New Chunk(arrow3(k), datafont))
                                                If k = 0 Or k < 6 Then
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                Else
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                                End If


                                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                                cell.SetLeading(12, 0)
                                                cell.PaddingBottom = 4.0F
                                                cell.PaddingTop = 1.0F
                                                data.AddCell(cell)
                                            Next
                                            data.SpacingBefore = 0
                                            pdfdoc.Add(data)
                                            ' End If
                                        Next

                                        Dim mtotal As PdfPTable = New PdfPTable(6)
                                        Dim monthtotal() As String
                                        mtotal.SetWidths(New Single() {0.48F, 0.2F, 0.08F, 0.08F, 0.08F, 0.08F})
                                        mtotal.TotalWidth = documentWidth
                                        mtotal.LockedWidth = True
                                        monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString), (totaldebit - totalcredit).ToString("N" + currDecno.ToString), ""}
                                        'IIf(totalbalances <= 0, Decimal.Parse((Math.Abs(totalbalances))).ToString("N" + currDecno.ToString) + IIf(totalbalances = 0, "", " Cr"), totalbalances.ToString("N" + currDecno.ToString) + " Dr")
                                        For k = 0 To monthtotal.Length - 1
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(monthtotal(k), datafont))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                            cell.SetLeading(12, 0)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                            If k <> 0 Then
                                                cell.BackgroundColor = basecolor
                                            End If
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            mtotal.AddCell(cell)
                                        Next
                                        mtotal.SpacingBefore = 0
                                        pdfdoc.Add(mtotal)

                                    Next

                                    Dim Final As PdfPTable = New PdfPTable(6)
                                    Final.SetWidths(New Single() {0.48F, 0.2F, 0.08F, 0.08F, 0.08F, 0.08F})
                                    Final.TotalWidth = documentWidth
                                    Final.LockedWidth = True
                                    Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString), (finaldebit - finalcredit).ToString("N" + currDecno.ToString), ""}
                                    For k = 0 To finalTotal.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(finalTotal(k), datafontBold))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        Final.AddCell(cell)
                                    Next
                                    Final.SpacingBefore = 7
                                    pdfdoc.Add(Final)

                                    Dim one As String, two As String, three As String, four As String, five As String, six As String
                                    If dr3.Length > 0 Then
                                        If agingtype = 0 Then
                                            If Month = "01" Then
                                                one = "JAN"
                                                two = "DEC"
                                                three = "NOV"
                                                four = "OCT"
                                                five = "SEP"
                                                six = "AUG"
                                            ElseIf Month = "02" Then
                                                one = "FEB"
                                                two = "JAN"
                                                three = "DEC"
                                                four = "NOV"
                                                five = "OCT"
                                                six = "SEP"
                                            ElseIf Month = "03" Then
                                                one = "MAR"
                                                two = "FEB"
                                                three = "JAN"
                                                four = "DEC"
                                                five = "NOV"
                                                six = "OCT"
                                            ElseIf Month = "04" Then
                                                one = "APR"
                                                two = "MAR"
                                                three = "FEB"
                                                four = "JAN"
                                                five = "DEC"
                                                six = "NOV"
                                            ElseIf Month = "05" Then
                                                one = "MAY"
                                                two = "APR"
                                                three = "MAR"
                                                four = "FEB"
                                                five = "JAN"
                                                six = "DEC"
                                            ElseIf Month = "06" Then
                                                one = "JUN"
                                                two = "MAY"
                                                three = "APR"
                                                four = "MAR"
                                                five = "FEB"
                                                six = "JAN"
                                            ElseIf Month = "07" Then
                                                one = "JUL"
                                                two = "JUN"
                                                three = "MAY"
                                                four = "APR"
                                                five = "MAR"
                                                six = "FEB"
                                            ElseIf Month = "08" Then
                                                one = "AUG"
                                                two = "JUL"
                                                three = "JUN"
                                                four = "MAY"
                                                five = "APR"
                                                six = "MAR"
                                            ElseIf Month = "09" Then
                                                one = "SEP"
                                                two = "AUG"
                                                three = "JUL"
                                                four = "JUN"
                                                five = "MAY"
                                                six = "APR"
                                            ElseIf Month = "10" Then
                                                one = "OCT"
                                                two = "SEP"
                                                three = "AUG"
                                                four = "JUL"
                                                five = "JUN"
                                                six = "MAY"
                                            ElseIf Month = "11" Then
                                                one = "NOV"
                                                two = "OCT"
                                                three = "SEP"
                                                four = "AUG"
                                                five = "JUL"
                                                six = "JUN"
                                            ElseIf Month = "12" Then
                                                one = "DEC"
                                                two = "NOV"
                                                three = "OCT"
                                                four = "SEP"
                                                five = "AUG"
                                                six = "JUL"
                                            End If
                                        Else
                                            one = "<=0-30"
                                            two = "<=31-60"
                                            three = "<=61-90"
                                            four = "<=91-120"
                                            five = "<=121-150"
                                            six = "Over 150"
                                        End If
                                        Dim tbldetail As PdfPTable = New PdfPTable(1)
                                        tbldetail.SetWidths(New Single() {1.0F})
                                        tbldetail.TotalWidth = documentWidth
                                        tbldetail.LockedWidth = True
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk("Aging Analysis Of Balance", NormalFontBold))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                        cell.PaddingBottom = 3.0F
                                        tbldetail.AddCell(cell)
                                        tbldetail.SpacingBefore = 30
                                        pdfdoc.Add(tbldetail)

                                        Dim detailtbl As PdfPTable = New PdfPTable(8)
                                        detailtbl.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
                                        detailtbl.TotalWidth = documentWidth
                                        detailtbl.LockedWidth = True
                                        Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
                                        For i = 0 To 7
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(arrow4(i), HeadingFont))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                            cell.SetLeading(12, 0)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            detailtbl.AddCell(cell)
                                        Next
                                        detailtbl.SpacingBefore = 0
                                        pdfdoc.Add(detailtbl)



                                        Dim arr7() As Decimal
                                        arr7 = {dr3(0)("balance"), dr3(0)("age9"), dr3(0)("age1"), dr3(0)("age2"), dr3(0)("age3"), dr3(0)("age4"),
                                                  dr3(0)("age5"), dr3(0)("age6")}

                                        Dim tbldata As PdfPTable = New PdfPTable(8)
                                        tbldata.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
                                        tbldata.TotalWidth = documentWidth
                                        tbldata.LockedWidth = True
                                        For i = 0 To 7
                                            phrase = New Phrase()
                                            If i = 0 Then
                                                Dim bal As String = IIf(arr7(i) <= 0, Decimal.Parse((Math.Abs(arr7(i)))).ToString("N" + currDecno.ToString) + IIf(arr7(i) = 0, "", " Cr"), arr7(i).ToString("N" + currDecno.ToString) + " Dr")
                                                phrase.Add(New Chunk(bal, datafont))
                                            Else
                                                phrase.Add(New Chunk(arr7(i).ToString("N" + currDecno.ToString), datafont))
                                            End If
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                            cell.SetLeading(12, 0)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            tbldata.AddCell(cell)
                                        Next
                                        tbldata.SpacingBefore = 0
                                        pdfdoc.Add(tbldata)
                                    End If
                                    fdebit = 0.0
                                    fcredit = 0.0
                                    fbalance = 0
                                    mdebit = 0
                                    mcredit = 0
                                    mbalance = 0
                                    cumbal = 0
                                    pdfdoc.NewPage()
                                End If
                            Next
                        End If
                    Else
                        Dim logo As PdfPTable = Nothing
                        Dim Phrase = New Phrase()
                        Dim cell As PdfPCell = Nothing
                        logo = New PdfPTable(1)
                        logo.TotalWidth = documentWidth
                        logo.LockedWidth = True
                        logo.SetWidths(New Single() {1.0F})
                        logo.Complete = False
                        logo.SplitRows = False
                        Phrase = New Phrase()
                        Phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
                        cell = PhraseCell(Phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        logo.AddCell(cell)
                        'company name
                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.Colspan = 2
                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        logo.AddCell(cell)
                        pdfdoc.Add(logo)

                        Dim tblTitle As PdfPTable = New PdfPTable(1)
                        tblTitle.SetWidths(New Single() {1.0F})
                        tblTitle.TotalWidth = documentWidth
                        tblTitle.LockedWidth = True
                        Phrase = New Phrase()
                        Phrase.Add(New Chunk(custSupType & Environment.NewLine & vbLf & reportfilter, TitleFont))
                        cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 3.0F
                        tblTitle.AddCell(cell)
                        tblTitle.SpacingBefore = 7
                        pdfdoc.Add(tblTitle)


                        Dim tblcommon As PdfPTable = New PdfPTable(2)
                        tblcommon.SetWidths(New Single() {0.5F, 0.5F})
                        tblcommon.TotalWidth = documentWidth
                        tblcommon.LockedWidth = True
                        Dim tbl As PdfPTable = New PdfPTable(1)
                        Phrase = New Phrase()
                        Phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + " " + Environment.NewLine & vbLf, NormalFont))
                        Phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + todate + Environment.NewLine & vbLf, NormalFont))
                        Phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
                        Phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & " ", NormalFont))
                        Phrase.Add(New Chunk("FAX" & Space(11) & ":" & "" & Environment.NewLine, NormalFont))
                        Phrase.Add(New Chunk("          " & Space(16) & "                  ", NormalFont))
                        cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 3.0F
                        tbl.AddCell(cell)
                        tblcommon.AddCell(tbl)
                        Phrase = New Phrase()
                        Phrase.Add(New Chunk("Credit Limit" & "      " & ":" & Space(5) & "" + Environment.NewLine & vbLf, NormalFont))
                        Phrase.Add(New Chunk("Credit Days" & "      " & ":" & Space(5) & "" + Environment.NewLine & vbLf, NormalFont))
                        Phrase.Add(New Chunk("OverDue" & "          " & ":" & Space(5) & "" + Environment.NewLine & vbLf, NormalFont))
                        cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.SetLeading(12, 0)
                        cell.PaddingLeft = 250.0F
                        tblcommon.AddCell(cell)
                        tblcommon.SpacingBefore = 7
                        pdfdoc.Add(tblcommon)

                    End If
                    pdfdoc.AddTitle(custSupType)
                    pdfdoc.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, basecolor.BLACK)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New System.IO.MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 805.0F, 10.0F, 0)
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
#Region "Private Shared Function PhraseCell(phrase As Phrase, align As Integer, Cols As Integer, celBorder As Boolean, Optional celBottomBorder As String = ""None"") As PdfPCell"
    Private Shared Function PhraseCell(ByVal phrase As Phrase, ByVal align As Integer, ByVal Cols As Integer, ByVal celBorder As Boolean, Optional ByVal celBottomBorder As String = "None") As PdfPCell
        Dim cell As New PdfPCell(phrase)
        If Cols > 1 Then cell.Colspan = Cols
        If celBorder Then
            If celBottomBorder <> "None" Then
                If celBottomBorder = "No" Then
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                    cell.BorderColor = basecolor.BLACK
                Else
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.BorderColor = basecolor.BLACK
                End If
            Else
                cell.BorderColor = basecolor.BLACK
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

    'Public Sub SupplierStament(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal agents As DataTable, ByVal detailsdt As DataTable, ByVal fromdate As String, ByVal todate As String, ByVal datetype As String, ByVal agingtype As String, ByVal Type As String, ByRef pdfdoc As Document, ByVal currflg As Integer)

    '    Dim phrase As Phrase = Nothing
    '    Dim cell As PdfPCell = Nothing
    '    Dim currDecno As Integer

    '    custdetailsdt.DefaultView.Sort = "acc_code ASC"
    '    custdetailsdt = custdetailsdt.DefaultView.ToTable
    '    'If detailsdt.Rows.Count > 0 Then
    '    '    age1 = Convert.ToDecimal(detailsdt.Compute("Sum(age1)", ""))
    '    '    age2 = Convert.ToDecimal(detailsdt.Compute("Sum(age2)", ""))
    '    '    age3 = Convert.ToDecimal(detailsdt.Compute("Sum(age3)", ""))
    '    '    age4 = Convert.ToDecimal(detailsdt.Compute("Sum(age4)", ""))
    '    '    age5 = Convert.ToDecimal(detailsdt.Compute("Sum(age5)", ""))
    '    '    age6 = Convert.ToDecimal(detailsdt.Compute("Sum(age6)", ""))
    '    '    age1bal = Convert.ToDecimal(detailsdt.Compute("Sum(balance)", ""))
    '    'End If
    '    For Each Customer_Statement In custdetailsdt.Rows

    '        Dim decCurreccy As String
    '        '07/01/2019

    '        currency = Customer_Statement("currcode").ToString()
    '        If currflg = 0 Then
    '            decCurreccy = currency
    '        Else
    '            decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '        End If
    '        currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)

    '        'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)


    '        'Commented by Tanvir currency was showing usd 06/1/2019
    '        'If currflg = 0 Then
    '        '    currency = "USD"
    '        'Else
    '        '    Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '        '    currency = c

    '        'End If
    '        'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)

    '        'Dim row() As DataRow = custdetailsdt.Select("trantype<>'PR'")

    '        Dim trantypep = Customer_Statement("trantype").ToString()

    '        ' If Not (String.Equals(trantypep, "PR")) Then
    '        Dim acccode As String = Customer_Statement("acc_code").ToString()
    '        Dim accname As String = Customer_Statement("accname").ToString()
    '        Dim crlimit As String = Customer_Statement("crlimit").ToString()
    '        Dim totaldebit, totalcredit, debits, credits As Decimal
    '        'Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
    '        Dim k, r As Integer
    '        Dim dr() As System.Data.DataRow
    '        Dim dr1() As System.Data.DataRow
    '        Dim agentdet() As System.Data.DataRow
    '        dr1 = dt.Select("acc_code='" & acccode & "'")

    '        If dr1.Length = 0 Then
    '            dt.Rows.Add(acccode)
    '            dr = custdetailsdt.Select("acc_code='" & acccode & "'")
    '            agentdet = agents.Select("agentcode='" & acccode & "'")
    '            Dim dr3() As System.Data.DataRow
    '            '  dr3 = detailsdt.Select("acc_code='" & acccode & "'")

    '            Dim overdue, overbal, overcr As Decimal

    '            Dim gpbyMonth As DataTable
    '            Dim gpby As DataTable
    '            Dim arrow3() As String = Nothing
    '            Dim tdate As String = Nothing
    '            Dim arr3index As Integer
    '            Dim cumBalance As Decimal
    '            Dim mon As String = Format(Convert.ToDateTime(dr(0)("trandate").ToString()), "MM")

    '            If dr.Length > 0 Then
    '                '  Dim groups1 = From custledger In dr.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .gl_code = custledger.Field(Of String)("acc_gl_code"), Key .accname = custledger.Field(Of String)("accname")} Into Group Order By g.acccode
    '                Dim groups1 = From gpbyrow In dr.AsEnumerable() Group gpbyrow By g = New With {Key .acccode = gpbyrow.Field(Of String)("acc_code"), Key .gl_code = gpbyrow.Field(Of String)("acc_gl_code"), Key .accname = gpbyrow.Field(Of String)("accname")} Into Group Order By g.acccode

    '                For Each gpdata1 In groups1
    '                    gpbyMonth = gpdata1.Group.CopyToDataTable()

    '                    Dim dr4 = detailsdt.AsEnumerable().Where(Function(s) s.Field(Of String)("acc_code") = gpdata1.g.acccode And s.Field(Of String)("acc_gl_code") = gpdata1.g.gl_code)
    '                    Dim debit_t As Object = gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'")
    '                    Dim sumdebit As Decimal
    '                    If Not debit_t.Equals(DBNull.Value) Then
    '                        sumdebit = Convert.ToDecimal(gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'"))
    '                    Else
    '                        sumdebit = 0
    '                    End If
    '                    Dim credit_t As Object = gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'")
    '                    Dim sumCredit As Decimal
    '                    If Not credit_t.Equals(DBNull.Value) Then
    '                        sumCredit = Convert.ToDecimal(gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'"))
    '                    Else
    '                        sumCredit = 0
    '                    End If
    '                    acccode = gpbyMonth(0)("acc_code")
    '                    agebalance = sumCredit - sumdebit

    '                    agentdet = agents.Select("agentcode='" & acccode & "'")

    '                    If agentdet.Length > 0 Then
    '                        'If agentdet(0)("crlimit") Then
    '                        '    overbal = agebalance
    '                        '    overcr = Decimal.Parse(agentdet(0)("crlimit"))
    '                        '    overdue = Decimal.Subtract(overcr, overbal)
    '                        '    contact1 = agentdet(0)("contact1").ToString()
    '                        '    acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
    '                        '    cdays = Decimal.Parse(agentdet(0)("crdays"))
    '                        '    'Else
    '                        '    '    overdue = Decimal.Parse(dr3(0)("balance"))
    '                        'End If
    '                    Else
    '                        overbal = agebalance
    '                        overcr = 0
    '                        overdue = Decimal.Subtract(overcr, overbal)
    '                        contact1 = String.Empty
    '                        crlimit = 0
    '                        cdays = 0
    '                    End If
    '                    totalcredit = 0
    '                    totaldebit = 0
    '                    totalbalances = 0
    '                    Dim trantypes As String
    '                    Dim logo As PdfPTable = Nothing
    '                    logo = New PdfPTable(1)
    '                    logo.TotalWidth = documentWidth
    '                    logo.LockedWidth = True
    '                    logo.SetWidths(New Single() {1.0F})
    '                    logo.Complete = False
    '                    logo.SplitRows = False
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                    logo.AddCell(cell)
    '                    'company name
    '                    cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                    cell.Colspan = 2
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingBottom = 4
    '                    logo.AddCell(cell)
    '                    pdfdoc.Add(logo)

    '                    Dim tblTitle As PdfPTable = New PdfPTable(1)
    '                    tblTitle.SetWidths(New Single() {1.0F})
    '                    tblTitle.TotalWidth = documentWidth
    '                    tblTitle.LockedWidth = True
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(custSupType & Environment.NewLine & vbLf & reportfilter, TitleFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tblTitle.AddCell(cell)
    '                    tblTitle.SpacingBefore = 7
    '                    pdfdoc.Add(tblTitle)
    '                    Dim tblcommon As PdfPTable = New PdfPTable(2)
    '                    tblcommon.SetWidths(New Single() {0.5F, 0.5F})
    '                    tblcommon.TotalWidth = documentWidth
    '                    tblcommon.LockedWidth = True
    '                    Dim tbl As PdfPTable = New PdfPTable(1)
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() + Environment.NewLine & vbLf, NormalFont))
    '                    phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Environment.NewLine & vbLf, NormalFont))
    '                    phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
    '                    phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
    '                    phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tbl.AddCell(cell)
    '                    tblcommon.AddCell(tbl)
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Credit Limit" & "      " & ":" & Space(5) & acrlimit.ToString() + Environment.NewLine & vbLf, NormalFont))
    '                    phrase.Add(New Chunk("Credit Days" & "      " & ":" & Space(5) & cdays.ToString() + Environment.NewLine & vbLf, NormalFont))
    '                    phrase.Add(New Chunk("OverDue" & "          " & ":" & Space(5) & overdue.ToString("N" + currDecno.ToString) + Environment.NewLine & vbLf, NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingLeft = 250.0F
    '                    tblcommon.AddCell(cell)
    '                    tblcommon.SpacingBefore = 7
    '                    pdfdoc.Add(tblcommon)


    '                    Dim desc As PdfPTable = New PdfPTable(2)
    '                    desc.SetWidths(New Single() {0.82F, 0.18F})
    '                    desc.TotalWidth = documentWidth
    '                    desc.LockedWidth = True
    '                    phrase = New Phrase()
    '                    If datetype <> 0 Then
    '                        phrase.Add(New Chunk("Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
    '                    Else
    '                        phrase.Add(New Chunk("Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
    '                    End If
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                    cell.PaddingBottom = 3.0F
    '                    desc.AddCell(cell)
    '                    phrase = New Phrase()
    '                    Dim curr As PdfPTable = New PdfPTable(1)
    '                    phrase.Add(New Chunk("Currency" + "         " + ":" + Space(7) + currency, NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                    cell.PaddingBottom = 3.0F
    '                    curr.AddCell(cell)
    '                    desc.AddCell(curr)
    '                    desc.SpacingBefore = 7
    '                    pdfdoc.Add(desc)


    '                    Dim balance As PdfPTable = New PdfPTable(1)
    '                    balance.SetWidths(New Single() {1.0F})
    '                    balance.TotalWidth = documentWidth
    '                    balance.LockedWidth = True
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("We would appreciate if you could settle the balance due at the earliest", BalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                    cell.PaddingBottom = 3.0F
    '                    balance.AddCell(cell)
    '                    balance.SpacingBefore = 7
    '                    pdfdoc.Add(balance)

    '                    Dim maintbl As PdfPTable = New PdfPTable(10)
    '                    Dim arrow2() As String

    '                    ' maintbl = New PdfPTable(10)
    '                    maintbl.SetWidths(New Single() {0.075F, 0.045F, 0.09F, 0.07F, 0.08F, 0.34F, 0.1F, 0.1F, 0.1F, 0.1F})
    '                    maintbl.TotalWidth = documentWidth
    '                    maintbl.LockedWidth = True
    '                    arrow2 = {"TRAN DATE", "TYPE", "Doc No", "Booking ID.", "SUP INV NO/TICKET NO",
    '                                              "Description", "DEBIT", "CREDIT", "BALANCE", "CUMBAL"}

    '                    For i = 0 To arrow2.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrow2(i), HeadingFont))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        maintbl.AddCell(cell)
    '                    Next

    '                    maintbl.SpacingBefore = 7
    '                    pdfdoc.Add(maintbl)

    '                    Dim group1 As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)

    '                    finalcredit = 0
    '                    finaldebit = 0
    '                    For Each gpdata In group1
    '                        Dim perticulars As String

    '                        'gpdata.Select 
    '                        gpby = gpdata.CopyToDataTable()


    '                        For Each row As DataRow In gpdata
    '                            Dim data As PdfPTable = New PdfPTable(10)
    '                            data.SetWidths(New Single() {0.075F, 0.045F, 0.09F, 0.07F, 0.08F, 0.34F, 0.1F, 0.1F, 0.1F, 0.1F})
    '                            data.TotalWidth = documentWidth
    '                            data.LockedWidth = True
    '                            trantypes = row("trantype").ToString()
    '                            If Not String.IsNullOrEmpty(trantypes) Then
    '                                finalcredit = Decimal.Parse(row("credit")) + finalcredit
    '                                finaldebit = Decimal.Parse(row("debit")) + finaldebit
    '                                debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
    '                                credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
    '                                totalcredit = totalcredit + credits
    '                                totaldebit = totaldebit + debits
    '                                totalbalances = totalbalances + (credits - debits)
    '                                cumBalance = cumBalance + (credits - debits)

    '                                Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
    '                                tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
    '                                If row("particulars").ToString().Contains(row("incinvdesc").ToString()) Then
    '                                    perticulars = row("particulars").ToString()
    '                                Else
    '                                    perticulars = row("particulars").ToString() & Environment.NewLine & vbLf & row("incinvdesc").ToString()
    '                                End If
    '                                arrow3 = {tdate, row("trantype").ToString(), docno, row("fileno").ToString(), row("reconfno").ToString(), perticulars, IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)), (credits - debits).ToString("N" + currDecno.ToString()), IIf(cumBalance <= 0, "(" & Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) & ")", cumBalance.ToString("N" + currDecno.ToString))}
    '                                For k = 0 To arrow3.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(arrow3(k), datafont))

    '                                    If k = 0 Or k < 5 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                    ElseIf k = 5 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                    Else
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                    End If
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                                    cell.SetLeading(12, 0)
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    data.AddCell(cell)
    '                                Next
    '                                data.SpacingBefore = 0
    '                                pdfdoc.Add(data)
    '                            End If
    '                        Next

    '                        Dim mtotal As PdfPTable = New PdfPTable(6)
    '                        Dim monthtotal() As String
    '                        mtotal.SetWidths(New Single() {0.55F, 0.15F, 0.1F, 0.1F, 0.1F, 0.1F})
    '                        mtotal.TotalWidth = documentWidth
    '                        mtotal.LockedWidth = True
    '                        monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString), totalbalances.ToString("N" + currDecno.ToString), ""}
    '                        For k = 0 To monthtotal.Length - 1
    '                            phrase = New Phrase()
    '                            phrase.Add(New Chunk(monthtotal(k), datafont))
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                            cell.SetLeading(12, 0)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                            If k <> 0 Then
    '                                cell.BackgroundColor = basecolor
    '                            End If
    '                            cell.PaddingBottom = 4.0F
    '                            cell.PaddingTop = 1.0F
    '                            mtotal.AddCell(cell)
    '                        Next
    '                        mtotal.SpacingBefore = 0
    '                        pdfdoc.Add(mtotal)

    '                    Next

    '                    Dim Final As PdfPTable = New PdfPTable(5)
    '                    Final.SetWidths(New Single() {0.55F, 0.15F, 0.1F, 0.1F, 0.2F})
    '                    Final.TotalWidth = documentWidth
    '                    Final.LockedWidth = True
    '                    Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString), ""}
    '                    For k = 0 To finalTotal.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(finalTotal(k), datafontBold))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        Final.AddCell(cell)
    '                    Next
    '                    Final.SpacingBefore = 7
    '                    pdfdoc.Add(Final)

    '                    Dim one As String, two As String, three As String, four As String, five As String, six As String
    '                    If agingtype = 0 Then
    '                        If Month = "01" Then
    '                            one = "JAN"
    '                            two = "DEC"
    '                            three = "NOV"
    '                            four = "OCT"
    '                            five = "SEP"
    '                            six = "<=AUG"
    '                        ElseIf Month = "02" Then
    '                            one = "FEB"
    '                            two = "JAN"
    '                            three = "DEC"
    '                            four = "NOV"
    '                            five = "OCT"
    '                            six = "<=SEP"
    '                        ElseIf Month = "03" Then
    '                            one = "MAR"
    '                            two = "FEB"
    '                            three = "JAN"
    '                            four = "DEC"
    '                            five = "NOV"
    '                            six = "<=OCT"
    '                        ElseIf Month = "04" Then
    '                            one = "APR"
    '                            two = "MAR"
    '                            three = "FEB"
    '                            four = "JAN"
    '                            five = "DEC"
    '                            six = "<=NOV"
    '                        ElseIf Month = "05" Then
    '                            one = "MAY"
    '                            two = "APR"
    '                            three = "MAR"
    '                            four = "FEB"
    '                            five = "JAN"
    '                            six = "<=DEC"
    '                        ElseIf Month = "06" Then
    '                            one = "JUN"
    '                            two = "MAY"
    '                            three = "APR"
    '                            four = "MAR"
    '                            five = "FEB"
    '                            six = "<=JAN"
    '                        ElseIf Month = "07" Then
    '                            one = "JUL"
    '                            two = "JUN"
    '                            three = "MAY"
    '                            four = "APR"
    '                            five = "MAR"
    '                            six = "<=FEB"
    '                        ElseIf Month = "08" Then
    '                            one = "AUG"
    '                            two = "JUL"
    '                            three = "JUN"
    '                            four = "MAY"
    '                            five = "APR"
    '                            six = "<=MAR"
    '                        ElseIf Month = "09" Then
    '                            one = "SEP"
    '                            two = "AUG"
    '                            three = "JUL"
    '                            four = "JUN"
    '                            five = "MAY"
    '                            six = "<=APR"
    '                        ElseIf Month = "10" Then
    '                            one = "OCT"
    '                            two = "SEP"
    '                            three = "AUG"
    '                            four = "JUL"
    '                            five = "JUN"
    '                            six = "<=MAY"
    '                        ElseIf Month = "11" Then
    '                            one = "NOV"
    '                            two = "OCT"
    '                            three = "SEP"
    '                            four = "AUG"
    '                            five = "JUL"
    '                            six = "<=JUN"
    '                        ElseIf Month = "12" Then
    '                            one = "DEC"
    '                            two = "NOV"
    '                            three = "OCT"
    '                            four = "SEP"
    '                            five = "AUG"
    '                            six = "<=JUL"
    '                        End If
    '                    Else
    '                        one = "<=0-30"
    '                        two = "<=31-60"
    '                        three = "<=61-90"
    '                        four = "<=91-120"
    '                        five = "<=121-150"
    '                        six = "Over 150"
    '                    End If
    '                    Dim tbldetail As PdfPTable = New PdfPTable(1)
    '                    tbldetail.SetWidths(New Single() {1.0F})
    '                    tbldetail.TotalWidth = documentWidth
    '                    tbldetail.LockedWidth = True
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Ageing Analysis Of Balance", NormalFontBold))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tbldetail.AddCell(cell)
    '                    tbldetail.SpacingBefore = 30
    '                    pdfdoc.Add(tbldetail)

    '                    Dim detailtbl As PdfPTable = New PdfPTable(8)
    '                    detailtbl.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
    '                    detailtbl.TotalWidth = documentWidth
    '                    detailtbl.LockedWidth = True
    '                    Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
    '                    For i = 0 To 7
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrow4(i), HeadingFont))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        detailtbl.AddCell(cell)
    '                    Next
    '                    detailtbl.SpacingBefore = 0
    '                    pdfdoc.Add(detailtbl)

    '                    Dim arr7() As Decimal
    '                    ' arr7 = {age1bal, age9, age1, age2, age3, age4, age5, age6}
    '                    arr7 = {dr4(0)("balance"), dr4(0)("age9"), dr4(0)("age1"), dr4(0)("age2"), dr4(0)("age3"), dr4(0)("age4"),
    '                                     dr4(0)("age5"), dr4(0)("age6")}

    '                    Dim tbldata As PdfPTable = New PdfPTable(8)
    '                    tbldata.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
    '                    tbldata.TotalWidth = documentWidth
    '                    tbldata.LockedWidth = True
    '                    For i = 0 To 7
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arr7(i).ToString("N" + currDecno.ToString), datafont))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        tbldata.AddCell(cell)
    '                    Next
    '                    tbldata.SpacingBefore = 0
    '                    pdfdoc.Add(tbldata)
    '                    finalcredit = 0
    '                    finaldebit = 0
    '                    cumBalance = 0
    '                    agebalance = 0
    '                    ' pdfdoc.NewPage()
    '                    'age9 = 0
    '                    'age1 = 0
    '                    'age2 = 0
    '                    'age3 = 0
    '                    'age4 = 0
    '                    'age5 = 0
    '                    'age6 = 0 
    '                    pdfdoc.NewPage()
    '                Next

    '            End If
    '        End If
    '        ' End If

    '    Next

    'End Sub

    Public Sub SupplierStament(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal agents As DataTable, ByVal detailsdt As DataTable, ByVal fromdate As String, ByVal todate As String, ByVal datetype As String, ByVal agingtype As String, ByVal Type As String, ByRef pdfdoc As Document, ByVal currflg As Integer)

        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        Dim currDecno As Integer
        Dim provisionAcctCode As String = ""
        Dim provisionAcctName As String = ""

        custdetailsdt.DefaultView.Sort = "acc_code ASC"
        custdetailsdt = custdetailsdt.DefaultView.ToTable
        'If detailsdt.Rows.Count > 0 Then
        '    age1 = Convert.ToDecimal(detailsdt.Compute("Sum(age1)", ""))
        '    age2 = Convert.ToDecimal(detailsdt.Compute("Sum(age2)", ""))
        '    age3 = Convert.ToDecimal(detailsdt.Compute("Sum(age3)", ""))
        '    age4 = Convert.ToDecimal(detailsdt.Compute("Sum(age4)", ""))
        '    age5 = Convert.ToDecimal(detailsdt.Compute("Sum(age5)", ""))
        '    age6 = Convert.ToDecimal(detailsdt.Compute("Sum(age6)", ""))
        '    age1bal = Convert.ToDecimal(detailsdt.Compute("Sum(balance)", ""))
        'End If
        For Each Customer_Statement In custdetailsdt.Rows

            Dim decCurreccy As String
            '07/01/2019

            currency = Customer_Statement("currcode").ToString()
            If currflg = 0 Then
                decCurreccy = currency
            Else
                decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
            End If
            currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)

            'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)


            'Commented by Tanvir currency was showing usd 06/1/2019
            'If currflg = 0 Then
            '    currency = "USD"
            'Else
            '    Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
            '    currency = c

            'End If
            'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)

            'Dim row() As DataRow = custdetailsdt.Select("trantype<>'PR'")

            Dim trantypep = Customer_Statement("trantype").ToString()

            ' If Not (String.Equals(trantypep, "PR")) Then
            Dim acccode As String = Customer_Statement("acc_code").ToString()
            Dim accname As String = Customer_Statement("accname").ToString()
            Dim crlimit As String = Customer_Statement("crlimit").ToString()
            Dim totaldebit, totalcredit, debits, credits As Decimal
            'Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
            Dim k, r As Integer
            Dim dr() As System.Data.DataRow
            Dim dr1() As System.Data.DataRow
            Dim agentdet() As System.Data.DataRow
            dr1 = dt.Select("acc_code='" & acccode & "'")

            Dim conn As New SqlConnection
            Dim SqlCmd As New SqlCommand
            Dim DataAdapter As New SqlDataAdapter
            Dim ds2 As New DataSet
            conn = clsDBConnect.dbConnectionnew("strDBConnection")
            SqlCmd = New SqlCommand("select option_selected, option_value from reservation_parameters where param_id='5516'", conn)
            SqlCmd.CommandType = CommandType.Text
            Dim mysqlAdapter As New SqlDataAdapter()
            Dim provisionDt As New DataTable()
            mysqlAdapter.SelectCommand = SqlCmd
            mysqlAdapter.Fill(provisionDt)
            SqlCmd.Dispose()
            If provisionDt.Rows.Count > 0 Then
                If Not IsDBNull(provisionDt.Rows(0)("option_selected")) Then
                    provisionAcctCode = Convert.ToString(provisionDt.Rows(0)("option_selected"))
                Else
                    provisionAcctCode = ""
                End If
                If Not IsDBNull(provisionDt.Rows(0)("option_value")) Then
                    provisionAcctName = Convert.ToString(provisionDt.Rows(0)("option_value"))
                Else
                    provisionAcctName = ""
                End If
            End If

            SqlCmd = New SqlCommand("select Hotel_Account_Name, Hotel_Account_Number, Hotel_Account_Banck_Name, " +
                 " Hotel_Account_Branch_Name,Hotel_Account_SWIFT, Hotel_Account_IBAN from partymast where partycode = '" & acccode & "'", conn)
            SqlCmd.CommandType = CommandType.Text

            SqlCmd.CommandTimeout = 0
            DataAdapter.SelectCommand = SqlCmd
            DataAdapter.Fill(ds2)
            Dim bankdetails As New DataTable
            bankdetails = ds2.Tables(0)

            Dim supplierAccount As New SupplierAccount
            Dim bankDetail As DataRow = bankdetails.Rows(0)

            If Not IsDBNull(bankDetail("Hotel_Account_Name")) Then
                supplierAccount.AccName = bankDetail("Hotel_Account_Name").ToString()
            End If

            If Not IsDBNull(bankDetail("Hotel_Account_Number")) Then
                supplierAccount.AccNo = bankDetail("Hotel_Account_Number").ToString()
            End If

            If Not IsDBNull(bankDetail("Hotel_Account_Banck_Name")) Then
                supplierAccount.BankName = bankDetail("Hotel_Account_Banck_Name").ToString()
            End If

            If Not IsDBNull(bankDetail("Hotel_Account_Branch_Name")) Then
                supplierAccount.BranchName = bankDetail("Hotel_Account_Branch_Name").ToString()
            End If

            If Not IsDBNull(bankDetail("Hotel_Account_IBAN")) Then
                supplierAccount.Iban = bankDetail("Hotel_Account_IBAN").ToString()
            End If

            If Not IsDBNull(bankDetail("Hotel_Account_SWIFT")) Then
                supplierAccount.SwiftCode = bankDetail("Hotel_Account_SWIFT").ToString()
            End If



            If dr1.Length = 0 Then
                dt.Rows.Add(acccode)
                dr = custdetailsdt.Select("acc_code='" & acccode & "'")
                agentdet = agents.Select("agentcode='" & acccode & "'")
                Dim dr3() As System.Data.DataRow
                '  dr3 = detailsdt.Select("acc_code='" & acccode & "'")

                Dim overdue, overbal, overcr As Decimal

                Dim gpbyMonth As DataTable
                Dim gpby As DataTable
                Dim arrow3() As String = Nothing
                Dim tdate As String = Nothing
                Dim arr3index As Integer
                Dim cumBalance As Decimal
                Dim mon As String = Format(Convert.ToDateTime(dr(0)("trandate").ToString()), "MM")

                If dr.Length > 0 Then
                    '  Dim groups1 = From custledger In dr.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .gl_code = custledger.Field(Of String)("acc_gl_code"), Key .accname = custledger.Field(Of String)("accname")} Into Group Order By g.acccode
                    Dim groups1 = From gpbyrow In dr.AsEnumerable() Group gpbyrow By g = New With {Key .acccode = gpbyrow.Field(Of String)("acc_code"), Key .gl_code = gpbyrow.Field(Of String)("acc_gl_code"), Key .accname = gpbyrow.Field(Of String)("accname")} Into Group Order By g.acccode

                    For Each gpdata1 In groups1
                        gpbyMonth = gpdata1.Group.CopyToDataTable()

                        Dim dr4 = detailsdt.AsEnumerable().Where(Function(s) s.Field(Of String)("acc_code") = gpdata1.g.acccode And s.Field(Of String)("acc_gl_code") = gpdata1.g.gl_code)
                        Dim debit_t As Object = gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'")
                        Dim sumdebit As Decimal
                        If Not debit_t.Equals(DBNull.Value) Then
                            sumdebit = Convert.ToDecimal(gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'"))
                        Else
                            sumdebit = 0
                        End If
                        Dim credit_t As Object = gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'")
                        Dim sumCredit As Decimal
                        If Not credit_t.Equals(DBNull.Value) Then
                            sumCredit = Convert.ToDecimal(gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'"))
                        Else
                            sumCredit = 0
                        End If
                        acccode = gpbyMonth(0)("acc_code")
                        agebalance = sumCredit - sumdebit

                        agentdet = agents.Select("agentcode='" & acccode & "'")

                        If agentdet.Length > 0 Then
                            'If agentdet(0)("crlimit") Then
                            '    overbal = agebalance
                            '    overcr = Decimal.Parse(agentdet(0)("crlimit"))
                            '    overdue = Decimal.Subtract(overcr, overbal)
                            '    contact1 = agentdet(0)("contact1").ToString()
                            '    acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
                            '    cdays = Decimal.Parse(agentdet(0)("crdays"))
                            '    'Else
                            '    '    overdue = Decimal.Parse(dr3(0)("balance"))
                            'End If
                        Else
                            overbal = agebalance
                            overcr = 0
                            overdue = Decimal.Subtract(overcr, overbal)
                            contact1 = String.Empty
                            crlimit = 0
                            cdays = 0
                        End If
                        totalcredit = 0
                        totaldebit = 0
                        totalbalances = 0
                        Dim trantypes As String
                        Dim logo As PdfPTable = Nothing
                        logo = New PdfPTable(1)
                        logo.TotalWidth = documentWidth
                        logo.LockedWidth = True
                        logo.SetWidths(New Single() {1.0F})
                        logo.Complete = False
                        logo.SplitRows = False
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        logo.AddCell(cell)
                        'company name
                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.Colspan = 2
                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        logo.AddCell(cell)
                        pdfdoc.Add(logo)

                        Dim tblTitle As PdfPTable = New PdfPTable(1)
                        tblTitle.SetWidths(New Single() {1.0F})
                        tblTitle.TotalWidth = documentWidth
                        tblTitle.LockedWidth = True

                        Dim ProName As String = ""
                        If provisionAcctCode = gpdata1.g.gl_code Then
                            ProName = provisionAcctName & vbCrLf & vbCrLf
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(custSupType & Environment.NewLine & vbLf & ProName & reportfilter, TitleFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 3.0F
                        tblTitle.AddCell(cell)
                        tblTitle.SpacingBefore = 7
                        pdfdoc.Add(tblTitle)
                        Dim tblcommon As PdfPTable = New PdfPTable(2)
                        tblcommon.SetWidths(New Single() {0.5F, 0.5F})
                        tblcommon.TotalWidth = documentWidth
                        tblcommon.LockedWidth = True
                        Dim tbl As PdfPTable = New PdfPTable(1)
                        phrase = New Phrase()
                        Dim juniperId As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select int_partyCode from int_partymast where partycode='" + Customer_Statement("acc_code").ToString() + "'")
                        phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() & "                  ", NormalFont))
                        phrase.Add(New Chunk("JUNIPER REF NO" & Space(3) & ":" & Space(5) & juniperId & Environment.NewLine & vbLf, NormalFont))
                        phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Environment.NewLine, NormalFont))
                        phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine, NormalFont))
                        phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
                        phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
                        phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & Environment.NewLine, NormalFont))


                        phrase.Add(New Chunk("ACCOUNT NAME" + "   " + ":" + Space(6) + supplierAccount.AccName + Environment.NewLine, NormalFont))
                        phrase.Add(New Chunk("ACCOUNT NO" + "        " + ":" + Space(6) + supplierAccount.AccNo & Space(12), NormalFont))
                        phrase.Add(New Chunk("BANK NAME" + "   " + ":" + Space(6) + supplierAccount.BankName + ", " + supplierAccount.BranchName + Environment.NewLine, NormalFont))
                        phrase.Add(New Chunk("SWIFT CODE" & "          " & ":" & Space(6) & supplierAccount.SwiftCode & Space(12), NormalFont))
                        phrase.Add(New Chunk("IBAN" & "   " & ":" & Space(12) & supplierAccount.Iban & Environment.NewLine, NormalFont))
                        phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))

                        'phrase.Add(New Chunk("ACCOUNT NAME" + "           " + ":" + Space(12) + supplierAccount.AccName + Environment.NewLine & vbLf, NormalFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 3.0F
                        tbl.AddCell(cell)
                        tblcommon.AddCell(tbl)



                        phrase = New Phrase()
                        phrase.Add(New Chunk("Credit Limit" & "      " & ":" & Space(5) & acrlimit.ToString() + Environment.NewLine & vbLf, NormalFont))
                        phrase.Add(New Chunk("Credit Days" & "      " & ":" & Space(5) & cdays.ToString() + Environment.NewLine & vbLf, NormalFont))
                        phrase.Add(New Chunk("OverDue" & "          " & ":" & Space(5) & overdue.ToString("N" + currDecno.ToString) + Environment.NewLine & vbLf, NormalFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.SetLeading(12, 0)
                        cell.PaddingLeft = 250.0F
                        tblcommon.AddCell(cell)
                        tblcommon.SpacingBefore = 7
                        pdfdoc.Add(tblcommon)


                        Dim desc As PdfPTable = New PdfPTable(2)
                        desc.SetWidths(New Single() {0.82F, 0.18F})
                        desc.TotalWidth = documentWidth
                        desc.LockedWidth = True
                        phrase = New Phrase()
                        If datetype <> 0 Then
                            phrase.Add(New Chunk("Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
                        Else
                            phrase.Add(New Chunk("Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
                        End If
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        desc.AddCell(cell)
                        phrase = New Phrase()
                        Dim curr As PdfPTable = New PdfPTable(1)
                        phrase.Add(New Chunk("Currency" + "         " + ":" + Space(7) + currency, NormalFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.PaddingBottom = 3.0F
                        curr.AddCell(cell)
                        desc.AddCell(curr)
                        desc.SpacingBefore = 7
                        pdfdoc.Add(desc)


                        Dim balance As PdfPTable = New PdfPTable(1)
                        balance.SetWidths(New Single() {1.0F})
                        balance.TotalWidth = documentWidth
                        balance.LockedWidth = True
                        phrase = New Phrase()
                        phrase.Add(New Chunk("We would appreciate if you could settle the balance due at the earliest", BalFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        balance.AddCell(cell)
                        balance.SpacingBefore = 7
                        pdfdoc.Add(balance)

                        Dim maintbl As PdfPTable = New PdfPTable(10)
                        Dim arrow2() As String

                        ' maintbl = New PdfPTable(10)
                        maintbl.SetWidths(New Single() {0.075F, 0.09F, 0.045F, 0.07F, 0.08F, 0.34F, 0.1F, 0.1F, 0.1F, 0.1F})
                        maintbl.TotalWidth = documentWidth
                        maintbl.LockedWidth = True
                        arrow2 = {"TRAN DATE", "Doc No", "TYPE", "JUNIPER REF NO", "SUP INV NO",
                                                  "Description", "DEBIT", "CREDIT", "BALANCE", "CUMBAL"}

                        For i = 0 To arrow2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrow2(i), HeadingFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            maintbl.AddCell(cell)
                        Next

                        maintbl.SpacingBefore = 7
                        pdfdoc.Add(maintbl)

                        Dim group1 As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)

                        finalcredit = 0
                        finaldebit = 0


                        For Each gpdata In group1
                            Dim perticulars As String

                            totalcredit = 0 'changed by mohamed on 19/06/2021 as cumulative is coming
                            totaldebit = 0
                            totalbalances = 0

                            'gpdata.Select 
                            gpby = gpdata.CopyToDataTable()


                            For Each row As DataRow In gpdata
                                Dim data As PdfPTable = New PdfPTable(10)
                                data.SetWidths(New Single() {0.075F, 0.09F, 0.045F, 0.07F, 0.08F, 0.34F, 0.1F, 0.1F, 0.1F, 0.1F})
                                data.TotalWidth = documentWidth
                                data.LockedWidth = True
                                trantypes = row("trantype").ToString()
                                If Not String.IsNullOrEmpty(trantypes) Then
                                    finalcredit = Decimal.Parse(row("credit")) + finalcredit
                                    finaldebit = Decimal.Parse(row("debit")) + finaldebit
                                    debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
                                    credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
                                    totalcredit = totalcredit + credits
                                    totaldebit = totaldebit + debits
                                    totalbalances = totalbalances + (credits - debits)
                                    cumBalance = cumBalance + (credits - debits)

                                    Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
                                    tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
                                    If row("particulars").ToString().Contains(row("incinvdesc").ToString()) Then
                                        perticulars = row("particulars").ToString()
                                    Else
                                        perticulars = row("particulars").ToString() & Environment.NewLine & vbLf & row("incinvdesc").ToString()
                                    End If
                                    arrow3 = {tdate, docno, row("trantype").ToString(), row("extId").ToString(), row("reconfno").ToString(), perticulars, IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)), (credits - debits).ToString("N" + currDecno.ToString()), IIf(cumBalance <= 0, "(" & Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) & ")", cumBalance.ToString("N" + currDecno.ToString))}
                                    For k = 0 To arrow3.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrow3(k), datafont))

                                        If k = 0 Or k < 5 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        ElseIf k = 5 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        End If
                                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                        cell.SetLeading(12, 0)
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        data.AddCell(cell)
                                    Next
                                    data.SpacingBefore = 0
                                    pdfdoc.Add(data)
                                End If
                            Next

                            Dim mtotal As PdfPTable = New PdfPTable(6)
                            Dim monthtotal() As String
                            mtotal.SetWidths(New Single() {0.55F, 0.15F, 0.1F, 0.1F, 0.1F, 0.1F})
                            mtotal.TotalWidth = documentWidth
                            mtotal.LockedWidth = True
                            monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString), totalbalances.ToString("N" + currDecno.ToString), ""}
                            For k = 0 To monthtotal.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(monthtotal(k), datafont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.SetLeading(12, 0)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                If k <> 0 Then
                                    cell.BackgroundColor = basecolor
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                mtotal.AddCell(cell)
                            Next
                            mtotal.SpacingBefore = 0
                            pdfdoc.Add(mtotal)

                        Next

                        Dim Final As PdfPTable = New PdfPTable(5)
                        Final.SetWidths(New Single() {0.55F, 0.15F, 0.1F, 0.1F, 0.2F})
                        Final.TotalWidth = documentWidth
                        Final.LockedWidth = True
                        Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString), ""}
                        For k = 0 To finalTotal.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(finalTotal(k), datafontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            Final.AddCell(cell)
                        Next
                        Final.SpacingBefore = 7
                        pdfdoc.Add(Final)

                        Dim one As String, two As String, three As String, four As String, five As String, six As String
                        If agingtype = 0 Then
                            If Month = "01" Then
                                one = "JAN"
                                two = "DEC"
                                three = "NOV"
                                four = "OCT"
                                five = "SEP"
                                six = "<=AUG"
                            ElseIf Month = "02" Then
                                one = "FEB"
                                two = "JAN"
                                three = "DEC"
                                four = "NOV"
                                five = "OCT"
                                six = "<=SEP"
                            ElseIf Month = "03" Then
                                one = "MAR"
                                two = "FEB"
                                three = "JAN"
                                four = "DEC"
                                five = "NOV"
                                six = "<=OCT"
                            ElseIf Month = "04" Then
                                one = "APR"
                                two = "MAR"
                                three = "FEB"
                                four = "JAN"
                                five = "DEC"
                                six = "<=NOV"
                            ElseIf Month = "05" Then
                                one = "MAY"
                                two = "APR"
                                three = "MAR"
                                four = "FEB"
                                five = "JAN"
                                six = "<=DEC"
                            ElseIf Month = "06" Then
                                one = "JUN"
                                two = "MAY"
                                three = "APR"
                                four = "MAR"
                                five = "FEB"
                                six = "<=JAN"
                            ElseIf Month = "07" Then
                                one = "JUL"
                                two = "JUN"
                                three = "MAY"
                                four = "APR"
                                five = "MAR"
                                six = "<=FEB"
                            ElseIf Month = "08" Then
                                one = "AUG"
                                two = "JUL"
                                three = "JUN"
                                four = "MAY"
                                five = "APR"
                                six = "<=MAR"
                            ElseIf Month = "09" Then
                                one = "SEP"
                                two = "AUG"
                                three = "JUL"
                                four = "JUN"
                                five = "MAY"
                                six = "<=APR"
                            ElseIf Month = "10" Then
                                one = "OCT"
                                two = "SEP"
                                three = "AUG"
                                four = "JUL"
                                five = "JUN"
                                six = "<=MAY"
                            ElseIf Month = "11" Then
                                one = "NOV"
                                two = "OCT"
                                three = "SEP"
                                four = "AUG"
                                five = "JUL"
                                six = "<=JUN"
                            ElseIf Month = "12" Then
                                one = "DEC"
                                two = "NOV"
                                three = "OCT"
                                four = "SEP"
                                five = "AUG"
                                six = "<=JUL"
                            End If
                        Else
                            one = "<=0-30"
                            two = "<=31-60"
                            three = "<=61-90"
                            four = "<=91-120"
                            five = "<=121-150"
                            six = "Over 150"
                        End If
                        Dim tbldetail As PdfPTable = New PdfPTable(1)
                        tbldetail.SetWidths(New Single() {1.0F})
                        tbldetail.TotalWidth = documentWidth
                        tbldetail.LockedWidth = True
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Ageing Analysis Of Balance", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 3.0F
                        tbldetail.AddCell(cell)
                        tbldetail.SpacingBefore = 30
                        pdfdoc.Add(tbldetail)

                        Dim detailtbl As PdfPTable = New PdfPTable(8)
                        detailtbl.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
                        detailtbl.TotalWidth = documentWidth
                        detailtbl.LockedWidth = True
                        Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
                        For i = 0 To 7
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrow4(i), HeadingFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            detailtbl.AddCell(cell)
                        Next
                        detailtbl.SpacingBefore = 0
                        pdfdoc.Add(detailtbl)

                        Dim arr7() As Decimal
                        ' arr7 = {age1bal, age9, age1, age2, age3, age4, age5, age6}
                        arr7 = {dr4(0)("balance"), dr4(0)("age9"), dr4(0)("age1"), dr4(0)("age2"), dr4(0)("age3"), dr4(0)("age4"),
                                         dr4(0)("age5"), dr4(0)("age6")}

                        Dim tbldata As PdfPTable = New PdfPTable(8)
                        tbldata.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
                        tbldata.TotalWidth = documentWidth
                        tbldata.LockedWidth = True
                        For i = 0 To 7
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arr7(i).ToString("N" + currDecno.ToString), datafont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tbldata.AddCell(cell)
                        Next
                        tbldata.SpacingBefore = 0
                        pdfdoc.Add(tbldata)
                        finalcredit = 0
                        finaldebit = 0
                        cumBalance = 0
                        agebalance = 0
                        ' pdfdoc.NewPage()
                        'age9 = 0
                        'age1 = 0
                        'age2 = 0
                        'age3 = 0
                        'age4 = 0
                        'age5 = 0
                        'age6 = 0 
                        pdfdoc.NewPage()
                    Next

                End If
            End If
            ' End If

        Next

    End Sub

#Region "Private Shared Function ImageCell(path As String, scale As Single, align As Integer) As PdfPCell"
    Private Shared Function ImageCell(ByVal path As String, ByVal scale As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))
        image.ScalePercent(scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = basecolor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

    Private Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal agents As DataTable, ByVal detailsdt As DataTable, ByVal fromdate As String, ByVal todate As String, ByVal datetype As String, ByVal agingtype As String, ByVal Type As String, ByVal currflg As Integer, ByRef bytes() As Byte)
        Try

            Dim pdfdoc As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
            Dim wb As New XLWorkbook
            Dim ws = wb.Worksheets.Add("Statement")
            Dim rowcount As Integer = 5

            If custdetailsdt.Rows.Count > 0 Then

                custdetailsdt.DefaultView.Sort = "acc_code ASC"
                custdetailsdt = custdetailsdt.DefaultView.ToTable

                If Type = "S" Then
                    SupplierStamentExcel(custdetailsdt, dt, agents, detailsdt, fromdate, todate, datetype, agingtype, Type, ws, currflg, rowcount)
                Else
                    'Sharfudeen 21/12/2023

                    'ws.Column("A:D").Width = 15
                    'ws.Column("E").Width = 28
                    'ws.Columns("L:I").Width = 15

                    ws.Column("A:E").Width = 15
                    ws.Column("F").Width = 28
                    ws.Columns("L:I").Width = 15

                    ws.Cell("A" & rowcount).Value = custSupType

                    Dim company = ws.Range("A" & rowcount & ":E" & rowcount).Merge()
                    company.Style.Font.SetBold().Font.FontSize = 15

                    rowcount = rowcount + 1
                    ws.Cell("A" & rowcount).Value = reportfilter

                    Dim filter = ws.Range("A" & rowcount & ":I" & rowcount).Merge()
                    filter.Style.Font.SetBold().Font.FontSize = 13

                    Dim rowheight As Integer

                    If reportfilter.Length > 140 Then
                        rowheight = IIf(reportfilter.Length > 140 And reportfilter.Length < 240, 32, IIf(reportfilter.Length > 240 And reportfilter.Length < 340, 48, 64))
                        ws.Row(rowcount).Height = rowheight
                    End If

                    For Each Customer_Statement In custdetailsdt.Rows
                        'added param 20/11/2018
                        'If currflg = 0 Then
                        '    currency = "USD"

                        'Else
                        '    Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                        '    currency = c

                        'End If
                        'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
                        'If currflg = 0 Then
                        '    currency = Customer_Statement("currcode").ToString()
                        '    currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
                        'End If
                        Dim decCurreccy As String
                        '07/01/2019

                        currency = Customer_Statement("currcode").ToString()
                        If currflg = 0 Then
                            decCurreccy = currency
                        Else
                            decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                        End If
                        currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)



                        If currDecno = 1 Then
                            DecimalPoint = "###,##,##,##0.0"
                            DecimalPoint1 = "(###,##,##,##0.0)"
                        ElseIf currDecno = 2 Then
                            DecimalPoint = "###,##,##,##0.00"
                            DecimalPoint1 = "(###,##,##,##0.00)"
                        ElseIf currDecno = 3 Then
                            DecimalPoint = "###,##,##,##0.000"
                            DecimalPoint1 = "(###,##,##,##0.000)"
                        ElseIf currDecno = 4 Then
                            DecimalPoint = "###,##,##,##0.0000"
                            DecimalPoint1 = "(###,##,##,##0.00000)"
                        Else
                            DecimalPoint = "###,##,##,##0.00"
                            DecimalPoint1 = "(###,##,##,##0.00)"
                        End If

                        Dim acccode As String = Customer_Statement("acc_code").ToString()
                        Dim accname As String = Customer_Statement("accname").ToString()
                        Dim crlimit As String = Customer_Statement("crlimit").ToString()
                        Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
                        Dim k As Integer
                        Dim dr() As System.Data.DataRow
                        Dim dr1() As System.Data.DataRow
                        Dim agentdet() As System.Data.DataRow
                        dr1 = dt.Select("acc_code='" & acccode & "'")


                        If dr1.Length = 0 Then
                            dt.Rows.Add(acccode)
                            dr = custdetailsdt.Select("acc_code='" & acccode & "'")
                            agentdet = agents.Select("agentcode='" & acccode & "'")
                            Dim dr3() As System.Data.DataRow
                            dr3 = detailsdt.Select("acc_code='" & acccode & "'")
                            Dim logo As PdfPTable = Nothing
                            logo = New PdfPTable(1)
                            logo.TotalWidth = documentWidth
                            logo.LockedWidth = True
                            logo.SetWidths(New Single() {1.0F})
                            logo.Complete = False
                            logo.SplitRows = False
                            'Phrase = New Phrase()
                            'Phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
                            'cell = PhraseCell(Phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            'cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                            'logo.AddCell(cell)
                            ''company name
                            'cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                            'cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            'cell.Colspan = 2
                            'cell.SetLeading(12, 0)
                            'cell.PaddingBottom = 4
                            'logo.AddCell(cell)
                            'pdfdoc.Add(logo)

                            'Comapny Name Heading

                            Dim overdue As Decimal
                            Dim l = agentdet.Length
                            If agentdet.Length > 0 Then
                                If IsDBNull(agentdet(0)("crlimit")) Then agentdet(0)("crlimit") = 0
                                If agentdet(0)("crlimit") Then
                                    Dim overbal As Decimal = Decimal.Parse(dr3(0)("balance"))
                                    Dim overcr As Decimal = Decimal.Parse(agentdet(0)("crlimit"))
                                    overdue = Decimal.Subtract(overbal, overcr)
                                    contact1 = agentdet(0)("contact1").ToString()
                                    acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
                                    cdays = Decimal.Parse(agentdet(0)("crdays"))
                                Else
                                    overdue = Decimal.Parse(dr3(0)("balance"))
                                End If
                            Else
                                Dim overbal As Decimal = Decimal.Parse(dr3(0)("balance"))
                                Dim overcr As Decimal = 0
                                overdue = Decimal.Subtract(overbal, overcr)
                                contact1 = String.Empty
                                crlimit = 0
                                cdays = 0
                            End If

                            'phrase = New Phrase()
                            'phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() + Environment.NewLine & vbLf, NormalFont))
                            'phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Convert.ToDateTime(todate).ToString("dd/MM/yyyy") + Environment.NewLine & vbLf, NormalFont))
                            'phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
                            'phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
                            'phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
                            'phrase.Add(New Chunk("          " & Space(16) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))

                            rowcount = rowcount + 1
                            ws.Range(rowcount + 4, 1, rowcount + 4, 6).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Font.FontSize = 11

                            ws.Range(rowcount, 1, rowcount, 2).Value = "TO"
                            ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Font.FontSize = 11
                            ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString()
                            ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Font.FontSize = 11

                            ws.Cell(rowcount, 8).Value = "Credit Limit"
                            ws.Cell(rowcount, 9).Value = ":" & Space(3) & acrlimit.ToString()

                            rowcount = rowcount + 1
                            ws.Range(rowcount, 1, rowcount, 1).Value = "DATE"
                            ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
                            ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
                            ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

                            ws.Cell(rowcount, 8).Value = "Credit Days"
                            ws.Cell(rowcount, 9).Value = ":" & Space(3) & cdays.ToString()


                            rowcount = rowcount + 1
                            ws.Range(rowcount, 1, rowcount, 1).Value = "ATTN."
                            ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
                            ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & contact1
                            ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

                            Dim overDueAmt As String = IIf(overdue <= 0, Decimal.Parse((Math.Abs(overdue))).ToString("N" + currDecno.ToString) + IIf(overdue = 0, "", " Cr"), overdue.ToString("N" + currDecno.ToString) + " Dr")
                            ws.Cell(rowcount, 8).Value = "OverDue"
                            ws.Cell(rowcount, 9).Value = ":" & Space(3) & overDueAmt

                            rowcount = rowcount + 1
                            ws.Range(rowcount, 1, rowcount, 1).Value = "TEL"
                            ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
                            ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & Customer_Statement("tel1").ToString() & Space(15) & "FAX:" & Space(3) & Customer_Statement("fax").ToString()
                            ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)


                            rowcount = rowcount + 1
                            ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)

                            ws.Range(rowcount, 2, rowcount, 2).Value = Space(3) & Customer_Statement("tel2").ToString()

                            ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

                            rowcount = rowcount + 1
                            If datetype <> 0 Then
                                ws.Cell(rowcount, 1).Value = "Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

                            Else
                                ws.Cell(rowcount, 1).Value = "Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

                            End If
                            ws.Range(rowcount, 1, rowcount, 7).Merge().Style.Alignment.WrapText = True

                            ws.Cell(rowcount, 8).Value = "Currency" & Space(3) & ":" & Space(4) + currency
                            'ws.Cell(rowCount, 11)
                            ws.Range(rowcount, 8, rowcount, 9).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)

                            rowcount = rowcount + 1
                            ws.Cell(rowcount, 1).Value = "We would appreciate if you could settle the balance due at the earliest"
                            ws.Range(rowcount, 1, rowcount, 7).Merge() ' ws.Range(rowcount, 1, rowcount, 6).Merge() sharfudeen



                            Dim arrow2() As String



                            'arrow2 = {"TRAN DATE", "TYPE", "INV.NO", "AGENT REF.", "GUEST/SERVICE DETAILS",
                            '                          "SPERSON", "DEBIT", "CREDIT", "NET BALANCE"}

                            'Sharfudeen 21/12/2023
                            'arrow2 = {"TRAN DATE", "TYPE", "INV.NO", "AGENT REF.", "GUEST/SERVICE DETAILS",
                            '                          "SPERSON", "DEBIT", "CREDIT", "NET BALANCE", "BOOKING ID"}

                            arrow2 = {"TRAN DATE", "TYPE", "INV.NO", "BOOKING ID", "AGENT REF.", "GUEST/SERVICE DETAILS",
                          "SPERSON", "DEBIT", "CREDIT", "NET BALANCE"}

                            rowcount = rowcount + 2
                            ws.Range(rowcount, 1, rowcount, arrow2.Length).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                            ws.Range(rowcount, 1, rowcount, arrow2.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

                            For i = 0 To arrow2.Length - 1
                                ws.Cell(rowcount, i + 1).Value = arrow2(i)
                            Next

                            Dim gpbyMonth As DataTable
                            Dim arrow3() As String = Nothing
                            Dim tdate As String = Nothing
                            Dim arr3index As Integer
                            Dim cumBalance As Decimal
                            Dim totaldebit, totalcredit, debits, credits As Decimal
                            finalcredit = 0
                            finaldebit = 0

                            'Dim groups = From gpbymonth In dr.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .gl_code = custledger.Field(Of String)("acc_gl_code"), Key .accname = custledger.Field(Of String)("accname")} Into Group Order By g.acccode
                            If dr.Length > 0 Then
                                gpbyMonth = dr.CopyToDataTable()
                            End If

                            'Sharfudeen 20/12/2023
                            '   Dim group As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)
                            Dim group As IEnumerable(Of IGrouping(Of String, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").ToString("yyyy/MM"))



                            ' Dim groups As IEnumerable(Of IGrouping(Of String, DataRow)) = dscurrecncy.AsEnumerable.GroupBy(Function(g) g.Field(Of String)(groupby)).OrderBy(Function(o) o.Key)
                            cumBalance = 0
                            For Each gpdata In group
                                Dim perticulars As String

                                totalcredit = 0
                                totaldebit = 0
                                totalbalances = 0
                                Dim trantypes As String

                                For Each row As DataRow In gpdata


                                    trantypes = row("trantype").ToString()
                                    '  If Not String.Equals(trantypes, "PR") Then
                                    finalcredit = Decimal.Parse(row("credit")) + finalcredit
                                    finaldebit = Decimal.Parse(row("debit")) + finaldebit
                                    debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
                                    credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
                                    totalcredit = totalcredit + credits
                                    totaldebit = totaldebit + debits
                                    totalbalances = totalbalances + (debits - credits)
                                    cumBalance = cumBalance + (debits - credits)
                                    Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
                                    tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")

                                    'Sharfudeen 20/12/2023
                                    'arrow3 = {tdate, row("trantype").ToString(), row("tranid").ToString(), row("reconfno").ToString(), row("particulars").ToString(),
                                    'row("sperson").ToString(), IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)),
                                    '          IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) + IIf(cumBalance = 0, "", " Cr"), cumBalance.ToString("N" + currDecno.ToString) + " Dr"), row("extId").ToString()}
                                    arrow3 = {tdate, row("trantype").ToString(), row("tranid").ToString(), row("extId").ToString(), row("reconfno").ToString(), row("particulars").ToString(),
                                    row("sperson").ToString(), IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)),
                                              IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) + IIf(cumBalance = 0, "", " Cr"), cumBalance.ToString("N" + currDecno.ToString) + " Dr")}


                                    '  arrow3 = {tdate, row("trantype").ToString(), row("tranid").ToString(), row("reconfno").ToString(), row("particulars").ToString(),
                                    'row("sperson").ToString(), IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)),
                                    '          IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) + IIf(cumBalance = 0, "", " Cr"), cumBalance.ToString("N" + currDecno.ToString) + " Dr")}
                                    rowcount = rowcount + 1
                                    ws.Range(rowcount, 1, rowcount, arrow3.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                                    ws.Range(rowcount, 1, rowcount, arrow3.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

                                    For k = 0 To arrow3.Length - 1

                                        'Sharfudeen 21/12/2023
                                        'If k = 0 Or k < 6 Or k = 9 Then
                                        If k = 0 Or k < 7 Then ' Or k = 9
                                            ws.Cell(rowcount, k + 1).Value = arrow3(k)
                                            ws.Cell(rowcount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                        Else
                                            ws.Cell(rowcount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                            If k = 7 Then '  k = 6 'Sharfudeen 21/120/2023
                                                If debits = 0 Then
                                                    ws.Cell(rowcount, k + 1).Value = "-"
                                                Else
                                                    ws.Cell(rowcount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                    ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint
                                                End If

                                            ElseIf k = 8 Then ' k = 7 'Sharfudeen 21/120/2023
                                                If credits = 0 Then
                                                    ws.Cell(rowcount, k + 1).Value = "-"
                                                Else
                                                    ws.Cell(rowcount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                    ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint
                                                End If


                                            ElseIf k = 9 Then ' k = 8 'Sharfudeen 21/120/2023
                                                If cumBalance = 0 Then
                                                    ws.Cell(rowcount, k + 1).Value = "-"
                                                ElseIf cumBalance < 0 Then
                                                    ws.Cell(rowcount, k + 1).Value = arrow3(k)  'Decimal.Parse(arrow3(k))
                                                    'ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint1
                                                Else
                                                    ws.Cell(rowcount, k + 1).Value = arrow3(k) 'Decimal.Parse(arrow3(k))
                                                    'ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint
                                                End If
                                            Else
                                                ws.Cell(rowcount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint

                                            End If
                                        End If
                                    Next

                                Next


                                Dim monthtotal() As String

                                monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString),
                                               IIf(totalbalances <= 0, Decimal.Parse((Math.Abs(totalbalances))).ToString("N" + currDecno.ToString) + IIf(totalbalances = 0, "", " Cr"), totalbalances.ToString("N" + currDecno.ToString) + " Dr")}
                                rowcount = rowcount + 1
                                ws.Range(rowcount, 1, rowcount, 10).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                                ws.Range(rowcount, 1, rowcount, 10).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                                ws.Range(rowcount, 7, rowcount, 10).Style.Fill.SetBackgroundColor(XLColor.LightGray)

                                For k = 0 To monthtotal.Length - 1
                                    If k = 0 Then
                                        ws.Cell(rowcount, k + 1).Value = monthtotal(k)
                                        'Sharfudeen 21/12/2023
                                        ' ws.Range(rowcount, 1, rowcount, 5).Merge()
                                        ws.Range(rowcount, 1, rowcount, 6).Merge()
                                    Else
                                        If k = 1 Then
                                            ws.Cell(rowcount, k + 6).Value = monthtotal(k) ' k + 5 sharfudeen
                                            'ws.Range(rowcount, k + 5, rowcount, k + 7).Merge()                                        
                                        ElseIf k = monthtotal.Length - 1 Then
                                            ws.Cell(rowcount, k + 6).Value = monthtotal(k) 'k + 5 sharfudeen
                                            ws.Cell(rowcount, k + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                        ElseIf monthtotal(k) = 0 And k <> monthtotal.Length - 1 Then
                                            ws.Cell(rowcount, k + 6).Value = "-" 'k + 5 sharfudeen
                                            ws.Cell(rowcount, k + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                            'ElseIf k = monthtotal.Length - 1 Then
                                            '    ws.Cell(rowcount, 14).Value = " "
                                        Else
                                            ws.Cell(rowcount, k + 6).Value = Decimal.Parse(monthtotal(k)) ' k + 5 sharfudeen
                                            ws.Cell(rowcount, k + 6).Style.NumberFormat.Format = DecimalPoint ' k + 5 sharfudeen
                                        End If

                                    End If
                                Next


                            Next

                            'Dim Final As PdfPTable = New PdfPTable(5)
                            'Final.SetWidths(New Single() {0.58F, 0.14F, 0.07F, 0.07F, 0.14F})
                            'Final.TotalWidth = documentWidth
                            'Final.LockedWidth = True
                            Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString)}
                            rowcount = rowcount + 1
                            ws.Range(rowcount, 1, rowcount, 10).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                            ' ws.Range(rowcount, 2, rowcount, 14).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                            ' ws.Range(rowcount, 9, rowcount, 14).Style.Fill.SetBackgroundColor(XLColor.LightGray)

                            For k = 0 To finalTotal.Length - 1
                                If k = 0 Then
                                    ws.Cell(rowcount, k + 1).Value = finalTotal(k)
                                    ws.Range(rowcount, 1, rowcount, 6).Merge() ' , 5) Sharfudeen
                                Else
                                    If k = 1 Then
                                        ws.Cell(rowcount, k + 6).Value = finalTotal(k) 'k + 5 Sharfudeen
                                        'ws.Range(rowcount, k + 6, rowcount, k + 7).Merge()

                                    ElseIf String.IsNullOrEmpty(finalTotal(k)) Then
                                        ws.Cell(rowcount, k + 6).Value = "" ' k + 5 Sharfudeen
                                    Else
                                        ws.Cell(rowcount, k + 6).Value = Decimal.Parse(finalTotal(k)) 'k + 5 Sharfudeen
                                        ws.Cell(rowcount, k + 6).Style.NumberFormat.Format = DecimalPoint 'k + 5 Sharfudeen
                                    End If

                                End If
                            Next
                            'Final.SpacingBefore = 7
                            'pdfdoc.Add(Final)

                            Dim one As String, two As String, three As String, four As String, five As String, six As String
                            If agingtype = 0 Then
                                If Month = "01" Then
                                    one = "JAN"
                                    two = "DEC"
                                    three = "NOV"
                                    four = "OCT"
                                    five = "SEP"
                                    six = "AUG"
                                ElseIf Month = "02" Then
                                    one = "FEB"
                                    two = "JAN"
                                    three = "DEC"
                                    four = "NOV"
                                    five = "OCT"
                                    six = "SEP"
                                ElseIf Month = "03" Then
                                    one = "MAR"
                                    two = "FEB"
                                    three = "JAN"
                                    four = "DEC"
                                    five = "NOV"
                                    six = "OCT"
                                ElseIf Month = "04" Then
                                    one = "APR"
                                    two = "MAR"
                                    three = "FEB"
                                    four = "JAN"
                                    five = "DEC"
                                    six = "NOV"
                                ElseIf Month = "05" Then
                                    one = "MAY"
                                    two = "APR"
                                    three = "MAR"
                                    four = "FEB"
                                    five = "JAN"
                                    six = "DEC"
                                ElseIf Month = "06" Then
                                    one = "JUN"
                                    two = "MAY"
                                    three = "APR"
                                    four = "MAR"
                                    five = "FEB"
                                    six = "JAN"
                                ElseIf Month = "07" Then
                                    one = "JUL"
                                    two = "JUN"
                                    three = "MAY"
                                    four = "APR"
                                    five = "MAR"
                                    six = "FEB"
                                ElseIf Month = "08" Then
                                    one = "AUG"
                                    two = "JUL"
                                    three = "JUN"
                                    four = "MAY"
                                    five = "APR"
                                    six = "MAR"
                                ElseIf Month = "09" Then
                                    one = "SEP"
                                    two = "AUG"
                                    three = "JUL"
                                    four = "JUN"
                                    five = "MAY"
                                    six = "APR"
                                ElseIf Month = "10" Then
                                    one = "OCT"
                                    two = "SEP"
                                    three = "AUG"
                                    four = "JUL"
                                    five = "JUN"
                                    six = "MAY"
                                ElseIf Month = "11" Then
                                    one = "NOV"
                                    two = "OCT"
                                    three = "SEP"
                                    four = "AUG"
                                    five = "JUL"
                                    six = "JUN"
                                ElseIf Month = "12" Then
                                    one = "DEC"
                                    two = "NOV"
                                    three = "OCT"
                                    four = "SEP"
                                    five = "AUG"
                                    six = "JUL"
                                End If
                            Else
                                one = "<=0-30"
                                two = "<=31-60"
                                three = "<=61-90"
                                four = "<=91-120"
                                five = "<=121-150"
                                six = "Over 150"
                            End If

                            rowcount = rowcount + 2

                            ws.Cell(rowcount, 1).Value = "Ageing Analysis Of Balance"
                            ws.Range(rowcount, 1, rowcount, 6).Merge().Style.Font.SetBold().Font.FontSize = 10
                            'phrase.Add(New Chunk("Ageing Analysis Of Balance", NormalFontBold))

                            rowcount = rowcount + 1
                            Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
                            ws.Range(rowcount, 1, rowcount, 8).Style.Font.SetBold.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 9
                            ws.Range(rowcount, 1, rowcount, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                            For i = 0 To 7
                                ws.Cell(rowcount, i + 1).Value = arrow4(i)
                            Next

                            Dim arr7() As Decimal
                            arr7 = {dr3(0)("balance"), dr3(0)("age9"), dr3(0)("age1"), dr3(0)("age2"), dr3(0)("age3"), dr3(0)("age4"),
                                      dr3(0)("age5"), dr3(0)("age6")}
                            rowcount = rowcount + 1
                            ws.Range(rowcount, 1, rowcount, arr7.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 9
                            ws.Range(rowcount, 1, rowcount, arr7.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

                            For i = 0 To 7
                                If i = 0 Then
                                    Dim bal As String = IIf(arr7(i) <= 0, Decimal.Parse((Math.Abs(arr7(i)))).ToString("N" + currDecno.ToString) + IIf(arr7(i) = 0, "", " Cr"), arr7(i).ToString("N" + currDecno.ToString) + " Dr")
                                    ws.Cell(rowcount, i + 1).Value = bal
                                    ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                Else
                                    ws.Cell(rowcount, i + 1).Value = arr7(i)
                                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoint
                                End If
                            Next
                            rowcount = rowcount + 3
                            fdebit = 0.0
                            fcredit = 0.0
                            fbalance = 0
                            mdebit = 0
                            mcredit = 0
                            mbalance = 0
                            cumbal = 0

                        End If
                    Next
                End If

            Else
                ws.Cell("A" & rowcount).Value = custSupType

                Dim company = ws.Range("A" & rowcount & ":D" & rowcount).Merge()
                company.Style.Font.SetBold().Font.FontSize = 15

                rowcount = rowcount + 1
                ws.Cell("A" & rowcount).Value = reportfilter

                Dim filter = ws.Range("A" & rowcount & ":M" & rowcount).Merge()
                filter.Style.Font.SetBold().Alignment.SetWrapText().Font.FontSize = 13

                Dim rowheight As Integer
                If reportfilter.Length > 140 Then
                    rowheight = IIf(reportfilter.Length > 140 And reportfilter.Length < 240, 32, IIf(reportfilter.Length > 240 And reportfilter.Length < 340, 48, 64))
                    ws.Row(rowcount).Height = rowheight
                End If


            End If

            ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
            ws.Range((rowcount + 2), 1, (rowcount + 2), 4).Merge()
            Using wStream As New MemoryStream()
                wb.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub




    Public Sub SupplierStamentExcel(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal agents As DataTable, ByVal detailsdt As DataTable, ByVal fromdate As String, ByVal todate As String, ByVal datetype As String, ByVal agingtype As String, ByVal Type As String, ByRef ws As IXLWorksheet, ByVal currflg As Integer, ByRef rowCount As Integer)
        Try
            Dim phrase As Phrase = Nothing
            Dim cell As PdfPCell = Nothing

            'If detailsdt.Rows.Count > 0 Then
            '    age1 = Convert.ToDecimal(detailsdt.Compute("Sum(age1)", ""))
            '    age2 = Convert.ToDecimal(detailsdt.Compute("Sum(age2)", ""))
            '    age3 = Convert.ToDecimal(detailsdt.Compute("Sum(age3)", ""))
            '    age4 = Convert.ToDecimal(detailsdt.Compute("Sum(age4)", ""))
            '    age5 = Convert.ToDecimal(detailsdt.Compute("Sum(age5)", ""))
            '    age6 = Convert.ToDecimal(detailsdt.Compute("Sum(age6)", ""))
            '    age1bal = Convert.ToDecimal(detailsdt.Compute("Sum(balance)", ""))
            'End If
            rowCount = 1

            '  ws.Cell("A2").Value = rptcompanyname
            Dim companyname = ws.Range("A" & rowCount & ":J" & rowCount).Merge()
            companyname.Style.Font.SetBold().Font.FontSize = 15
            companyname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            companyname.Merge()
            companyname.Style.Fill.BackgroundColor = XLColor.LightGray
            ws.Cell("A" & rowCount).Value = rptcompanyname

            rowCount = rowCount + 1
            ws.Cell("A" & rowCount).Value = custSupType

            Dim company = ws.Range("A" & rowCount & ":D" & rowCount).Merge()
            company.Style.Font.SetBold().Font.FontSize = 15

            rowCount = rowCount + 1
            ws.Cell("A" & rowCount).Value = reportfilter

            Dim filter = ws.Range("A" & rowCount & ":J" & rowCount).Merge()
            filter.Style.Alignment.SetWrapText().Font.SetBold().Font.FontSize = 13


            rowCount = rowCount + 2
            For Each Customer_Statement In custdetailsdt.Rows
                Dim decCurreccy As String
                '07/01/2019

                currency = Customer_Statement("currcode").ToString()
                If currflg = 0 Then
                    decCurreccy = currency
                Else
                    decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                End If
                currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)

                If currDecno = 1 Then
                    DecimalPoint = "###,##,##,##0.0"
                    DecimalPoint1 = "(###,##,##,##0.0)"
                ElseIf currDecno = 2 Then
                    DecimalPoint = "###,##,##,##0.00"
                    DecimalPoint1 = "(###,##,##,##0.00)"
                ElseIf currDecno = 3 Then
                    DecimalPoint = "###,##,##,##0.000"
                    DecimalPoint1 = "(###,##,##,##0.000)"
                ElseIf currDecno = 4 Then
                    DecimalPoint = "###,##,##,##0.0000"
                    DecimalPoint1 = "(###,##,##,##0.00000)"
                Else
                    DecimalPoint = "###,##,##,##0.00"
                    DecimalPoint1 = "(###,##,##,##0.00)"
                End If


                'If currflg = 0 Then
                '    currency = Customer_Statement("currcode").ToString()
                '    currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
                'End If
                ' Commented by Tanvir 6/01/2019  currency was showing usd
                '    If currflg = 0 Then
                '        currency = "USD"
                '    Else
                '        Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                '        currency = c

                '    End If
                ' currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
                'Dim row() As DataRow = custdetailsdt.Select("trantype<>'PR'")
                Dim trantypep = Customer_Statement("trantype").ToString()
                If Not (String.Equals(trantypep, "PR")) Then
                    Dim acccode As String = Customer_Statement("acc_code").ToString()
                    Dim accname As String = Customer_Statement("accname").ToString()
                    Dim crlimit As String = Customer_Statement("crlimit").ToString()
                    Dim totaldebit, totalcredit, debits, credits As Decimal
                    'Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
                    Dim k, r As Integer
                    Dim dr() As System.Data.DataRow
                    Dim dr1() As System.Data.DataRow
                    Dim agentdet() As System.Data.DataRow
                    dr1 = dt.Select("acc_code='" & acccode & "'")
                    If dr1.Length = 0 Then
                        dt.Rows.Add(acccode)
                        dr = custdetailsdt.Select("acc_code='" & acccode & "'")
                        agentdet = agents.Select("agentcode='" & acccode & "'")



                        Dim overdue, overbal, overcr As Decimal

                        Dim gpby As DataTable
                        ws.Column("A").Width = 12
                        ws.Column("B").Width = 10
                        ws.Column("C").Width = 13
                        ws.Column("E").Width = 10
                        ws.Column("D").Width = 12
                        ws.Column("F").Width = 42

                        ws.Columns("G:K").Width = 12

                        Dim gpbyMonth As DataTable
                        Dim arrow3() As String = Nothing
                        Dim tdate As String = Nothing
                        Dim arr3index As Integer
                        Dim cumBalance As Decimal
                        Dim mon As String = Format(Convert.ToDateTime(dr(0)("trandate").ToString()), "MM")
                        If dr.Length > 0 Then

                            Dim groups1 = From gpbyrow In dr.AsEnumerable() Group gpbyrow By g = New With {Key .acccode = gpbyrow.Field(Of String)("acc_code"), Key .gl_code = gpbyrow.Field(Of String)("acc_gl_code"), Key .accname = gpbyrow.Field(Of String)("accname")} Into Group Order By g.acccode

                            For Each gpdata1 In groups1

                                Dim dr4 = detailsdt.AsEnumerable().Where(Function(s) s.Field(Of String)("acc_code") = gpdata1.g.acccode And s.Field(Of String)("acc_gl_code") = gpdata1.g.gl_code)
                                gpbyMonth = gpdata1.Group.CopyToDataTable()

                                Dim sumdebit = Convert.ToDecimal(gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'"))
                                Dim sumCredit = Convert.ToDecimal(gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'"))
                                acccode = gpbyMonth(0)("acc_code")
                                agebalance = sumCredit - sumdebit

                                agentdet = agents.Select("agentcode='" & acccode & "'")

                                'If agentdet.Length > 0 Then
                                '    If Not (TypeOf agentdet(0)("crlimit") Is DBNull) Then
                                '        overbal = agebalance
                                '        overcr = Decimal.Parse(agentdet(0)("crlimit"))
                                '        overdue = Decimal.Subtract(overcr, overbal)
                                '        contact1 = agentdet(0)("contact1").ToString()
                                '        acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
                                '        cdays = Decimal.Parse(agentdet(0)("crdays"))

                                '    End If
                                'Else
                                '    overbal = agebalance
                                '    overcr = 0
                                '    overdue = Decimal.Subtract(overcr, overbal)
                                '    contact1 = String.Empty
                                '    crlimit = 0
                                '    cdays = 0
                                'End If

                                totalcredit = 0
                                totaldebit = 0
                                totalbalances = 0
                                Dim trantypes As String



                                Dim rowheight As Integer
                                If reportfilter.Length > 140 Then
                                    rowheight = IIf(reportfilter.Length > 140 And reportfilter.Length < 240, 32, IIf(reportfilter.Length > 240 And reportfilter.Length < 340, 48, 64))
                                    ws.Row(rowCount).Height = rowheight
                                End If

                                rowCount = rowCount + 1
                                ws.Range(rowCount + 4, 1, rowCount + 4, 6).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)

                                'ws.Range(rowCount, 1, rowCount, 1).Value = "TO"
                                'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin)
                                'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString()
                                'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin)

                                'ws.Cell(rowCount, 9).Value = "Credit Limit"
                                'ws.Cell(rowCount, 10).Value = ":" & Space(3) & acrlimit.ToString()

                                'rowCount = rowCount + 1
                                'ws.Range(rowCount, 1, rowCount, 1).Value = "DATE"
                                'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
                                'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
                                'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

                                'ws.Cell(rowCount, 9).Value = "Credit Days"
                                'ws.Cell(rowCount, 10).Value = ":" & Space(3) & cdays.ToString()


                                'rowCount = rowCount + 1
                                'ws.Range(rowCount, 1, rowCount, 1).Value = "ATTN."
                                'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
                                'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & contact1
                                'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                                'ws.Cell(rowCount, 9).Value = "OverDue"
                                'ws.Cell(rowCount, 10).Value = ":" & Space(3) & overdue.ToString("N" + currDecno.ToString)

                                'rowCount = rowCount + 1
                                'ws.Range(rowCount, 1, rowCount, 1).Value = "TEL"
                                'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
                                'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & Customer_Statement("tel1").ToString() & Space(10) & "FAX:" & Space(3) & Customer_Statement("fax").ToString()
                                'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)


                                rowCount = rowCount + 1
                                ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)

                                ws.Range(rowCount, 2, rowCount, 2).Value = Space(3) & Customer_Statement("tel2").ToString()
                                ws.Range(rowCount, 2, rowCount, 6).Merge()

                                rowCount = rowCount + 1
                                If datetype <> 0 Then
                                    ws.Cell(rowCount, 1).Value = "Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

                                Else
                                    ws.Cell(rowCount, 1).Value = "Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

                                End If
                                ws.Range(rowCount, 1, rowCount, 6).Merge()

                                ws.Cell(rowCount, 9).Value = "Currency" & Space(3) & ":" & Space(4) + currency
                                'ws.Cell(rowCount, 11)
                                ws.Range(rowCount, 9, rowCount, 10).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)

                                rowCount = rowCount + 1
                                ws.Cell(rowCount, 1).Value = "We would appreciate if you could settle the balance due at the earliest"
                                ws.Range(rowCount, 1, rowCount, 6).Merge()

                                Dim arrow2() As String
                                arrow2 = {"TRAN DATE", "TYPE", "Doc No", "JUNIPER REF NO.", "SUP INV NO/TICKET NO",
                                                          "Description", "DEBIT", "CREDIT", "BALANCE", "CUMBAL"}

                                rowCount = rowCount + 2
                                ws.Range(rowCount, 1, rowCount, 10).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                                ws.Range(rowCount, 1, rowCount, 10).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                                For i = 0 To arrow2.Length - 1
                                    ws.Cell(rowCount, i + 1).Value = arrow2(i)

                                Next


                                Dim group1 As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)
                                finalcredit = 0
                                finaldebit = 0



                                For Each gpdata In group1
                                    Dim perticulars As String

                                    totalcredit = 0 'changed by mohamed on 19/06/2021 as cumulative is coming
                                    totaldebit = 0
                                    totalbalances = 0

                                    gpby = gpdata.CopyToDataTable()


                                    For Each row As DataRow In gpdata
                                        Dim data As PdfPTable = New PdfPTable(10)
                                        data.SetWidths(New Single() {0.075F, 0.045F, 0.07F, 0.07F, 0.08F, 0.36F, 0.1F, 0.1F, 0.1F, 0.1F})
                                        data.TotalWidth = documentWidth
                                        data.LockedWidth = True
                                        trantypes = row("trantype").ToString()
                                        If Not String.IsNullOrEmpty(trantypes) Then
                                            finalcredit = Decimal.Parse(row("credit")) + finalcredit
                                            finaldebit = Decimal.Parse(row("debit")) + finaldebit
                                            debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
                                            credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
                                            totalcredit = totalcredit + credits
                                            totaldebit = totaldebit + debits
                                            totalbalances = totalbalances + (credits - debits)
                                            cumBalance = cumBalance + (credits - debits)
                                            Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
                                            tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
                                            If row("particulars").ToString().Contains(row("incinvdesc").ToString()) Then
                                                perticulars = row("particulars").ToString()
                                            Else
                                                perticulars = row("particulars").ToString() & Environment.NewLine & vbLf & row("incinvdesc").ToString()
                                            End If
                                            arrow3 = {tdate, row("trantype").ToString(), docno, row("fileno").ToString(), row("reconfno").ToString(), perticulars, IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", Math.Round(credits, currDecno)), (credits - debits).ToString("N" + currDecno.ToString), IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString()), cumBalance.ToString("N" + currDecno.ToString))}
                                            rowCount = rowCount + 1
                                            ws.Range(rowCount, 1, rowCount, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                                            ws.Range(rowCount, 1, rowCount, 10).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

                                            '  ws.Row(rowCount).AdjustToContents()
                                            For k = 0 To arrow3.Length - 1
                                                If k = 5 Then
                                                    ws.Cell(rowCount, k + 1).Value = arrow3(k)
                                                    ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText()
                                                    ' ws.Range(rowCount, k + 1, rowCount, k + 2).Merge()
                                                    ws.Cell(rowCount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                ElseIf k > 5 Then
                                                    ws.Cell(rowCount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    If k = 6 Then
                                                        If debits = 0 Then
                                                            ws.Cell(rowCount, k + 1).Value = "-"
                                                        Else
                                                            ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                            ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
                                                        End If
                                                    ElseIf k = 7 Then
                                                        If credits = 0 Then
                                                            ws.Cell(rowCount, k + 1).Value = "-"
                                                        Else
                                                            ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                            ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
                                                        End If
                                                    ElseIf k = arrow3.Length - 1 Then

                                                        If cumBalance < 0 Then
                                                            ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                            ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint1
                                                        ElseIf cumBalance = 0 Then
                                                            ws.Cell(rowCount, k + 1).Value = "-"
                                                        Else
                                                            ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                            ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
                                                        End If

                                                    Else

                                                        ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
                                                        ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
                                                    End If

                                                Else
                                                    ws.Cell(rowCount, k + 1).Value = arrow3(k)
                                                    ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                End If
                                            Next

                                        End If
                                    Next


                                    Dim monthtotal() As String

                                    monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString), totalbalances.ToString("N" + currDecno.ToString), ""}
                                    rowCount = rowCount + 1
                                    ws.Range(rowCount, 1, rowCount, 10).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 7
                                    ws.Range(rowCount, 1, rowCount, 10).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                                    ws.Range(rowCount, 6, rowCount, 10).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                                    For k = 0 To monthtotal.Length - 1
                                        If k = 0 Then
                                            ws.Range(rowCount, 1, rowCount, 5).Merge()
                                            ws.Range(rowCount, 1, rowCount, 5).Style.Border.SetBottomBorder(XLBorderStyleValues.None)
                                            ' ws.Range(rowCount, 2, rowCount, 7).Style.Border.SetTopBorder(XLBorderStyleValues.None)
                                        ElseIf k = 1 Or String.IsNullOrEmpty(monthtotal(k)) Then
                                            ws.Cell(rowCount, k + 5).Value = monthtotal(k)
                                            ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                        Else
                                            ws.Cell(rowCount, k + 5).Value = Decimal.Parse(monthtotal(k))
                                            ws.Cell(rowCount, k + 5).Style.NumberFormat.Format = DecimalPoint
                                            ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                        End If
                                    Next
                                Next


                                Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString), ""}

                                rowCount = rowCount + 1
                                ws.Range(rowCount, 1, rowCount, 10).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 7
                                ws.Range(rowCount, 1, rowCount, 10).Style.Alignment.WrapText = True
                                'ws.Range(rowCount, 2, rowCount, 12).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

                                For k = 0 To finalTotal.Length - 1
                                    If k = 0 Then
                                        ws.Range(rowCount, 1, rowCount, 5).Merge()
                                        ws.Range(rowCount, 1, rowCount, 5).Style.Border.SetBottomBorder(XLBorderStyleValues.None)

                                    ElseIf k = 1 Or String.IsNullOrEmpty(finalTotal(k)) Then
                                        ws.Cell(rowCount, k + 5).Value = finalTotal(k)
                                        ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                    Else
                                        ws.Cell(rowCount, k + 5).Value = Decimal.Parse(finalTotal(k))
                                        ws.Cell(rowCount, k + 5).Style.NumberFormat.Format = DecimalPoint
                                        ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                    End If
                                Next


                                Dim one As String, two As String, three As String, four As String, five As String, six As String
                                If agingtype = 0 Then
                                    If Month = "01" Then
                                        one = "JAN"
                                        two = "DEC"
                                        three = "NOV"
                                        four = "OCT"
                                        five = "SEP"
                                        six = "<=AUG"
                                    ElseIf Month = "02" Then
                                        one = "FEB"
                                        two = "JAN"
                                        three = "DEC"
                                        four = "NOV"
                                        five = "OCT"
                                        six = "<=SEP"
                                    ElseIf Month = "03" Then
                                        one = "MAR"
                                        two = "FEB"
                                        three = "JAN"
                                        four = "DEC"
                                        five = "NOV"
                                        six = "<=OCT"
                                    ElseIf Month = "04" Then
                                        one = "APR"
                                        two = "MAR"
                                        three = "FEB"
                                        four = "JAN"
                                        five = "DEC"
                                        six = "<=NOV"
                                    ElseIf Month = "05" Then
                                        one = "MAY"
                                        two = "APR"
                                        three = "MAR"
                                        four = "FEB"
                                        five = "JAN"
                                        six = "<=DEC"
                                    ElseIf Month = "06" Then
                                        one = "JUN"
                                        two = "MAY"
                                        three = "APR"
                                        four = "MAR"
                                        five = "FEB"
                                        six = "<=JAN"
                                    ElseIf Month = "07" Then
                                        one = "JUL"
                                        two = "JUN"
                                        three = "MAY"
                                        four = "APR"
                                        five = "MAR"
                                        six = "<=FEB"
                                    ElseIf Month = "08" Then
                                        one = "AUG"
                                        two = "JUL"
                                        three = "JUN"
                                        four = "MAY"
                                        five = "APR"
                                        six = "<=MAR"
                                    ElseIf Month = "09" Then
                                        one = "SEP"
                                        two = "AUG"
                                        three = "JUL"
                                        four = "JUN"
                                        five = "MAY"
                                        six = "<=APR"
                                    ElseIf Month = "10" Then
                                        one = "OCT"
                                        two = "SEP"
                                        three = "AUG"
                                        four = "JUL"
                                        five = "JUN"
                                        six = "<=MAY"
                                    ElseIf Month = "11" Then
                                        one = "NOV"
                                        two = "OCT"
                                        three = "SEP"
                                        four = "AUG"
                                        five = "JUL"
                                        six = "<=JUN"
                                    ElseIf Month = "12" Then
                                        one = "DEC"
                                        two = "NOV"
                                        three = "OCT"
                                        four = "SEP"
                                        five = "AUG"
                                        six = "<=JUL"
                                    End If
                                Else
                                    one = "<=0-30"
                                    two = "<=31-60"
                                    three = "<=61-90"
                                    four = "<=91-120"
                                    five = "<=121-150"
                                    six = "Over 150"
                                End If

                                rowCount = rowCount + 2

                                ws.Cell(rowCount, 1).Value = "Ageing Analysis Of Balance"
                                ws.Range(rowCount, 1, rowCount, 6).Merge().Style.Font.SetBold().Font.FontSize = 10
                                'phrase.Add(New Chunk("Ageing Analysis Of Balance", NormalFontBold))



                                rowCount = rowCount + 1

                                Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
                                ws.Range(rowCount, 1, rowCount, 8).Style.Font.SetBold.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 8
                                ws.Range(rowCount, 1, rowCount, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                For i = 0 To 7
                                    ws.Cell(rowCount, i + 1).Value = arrow4(i)
                                Next

                                rowCount = rowCount + 1
                                Dim arr7() As Decimal
                                '  arr7 = {age1bal, age9, age1, age2, age3, age4, age5, age6}
                                arr7 = {dr4(0)("balance"), dr4(0)("age9"), dr4(0)("age1"), dr4(0)("age2"), dr4(0)("age3"), dr4(0)("age4"),
                                                 dr4(0)("age5"), dr4(0)("age6")}

                                ws.Range(rowCount, 1, rowCount, arr7.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 8
                                ws.Range(rowCount, 1, rowCount, arr7.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

                                For i = 0 To 7
                                    ws.Cell(rowCount, i + 1).Value = arr7(i)
                                    ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint
                                Next
                                ' ws.Columns("A:M").AdjustToContents()
                                'ws.Rows().AdjustToContents()
                                rowCount = rowCount + 2
                                finalcredit = 0
                                finaldebit = 0
                                cumBalance = 0
                                agebalance = 0
                            Next
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


End Class
#Region "Class SupplierAccount"
Public Class SupplierAccount
    Private _acccode As String
    Private _accname As String
    Private _accno As String
    Private _bankname As String
    Private _branchname As String
    Private _swiftcode As String
    Private _iban As String
    Public Property AccCode() As String
        Get
            Return _acccode
        End Get
        Set(ByVal value As String)
            _acccode = value
        End Set
    End Property
    Public Property AccName() As String
        Get
            Return _accname
        End Get
        Set(ByVal value As String)
            _accname = value
        End Set
    End Property

    Public Property AccNo() As String
        Get
            Return _accno
        End Get
        Set(ByVal value As String)
            _accno = value
        End Set
    End Property


    Public Property BankName() As String
        Get
            Return _bankname
        End Get
        Set(ByVal value As String)
            _bankname = value
        End Set
    End Property

    Public Property BranchName() As String
        Get
            Return _branchname
        End Get
        Set(ByVal value As String)
            _branchname = value
        End Set
    End Property

    Public Property SwiftCode() As String
        Get
            Return _swiftcode
        End Get
        Set(ByVal value As String)
            _swiftcode = value
        End Set
    End Property

    Public Property Iban() As String
        Get
            Return _iban
        End Get
        Set(ByVal value As String)
            _iban = value
        End Set
    End Property
End Class
#End Region