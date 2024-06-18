Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports ClosedXML.Excel
Imports System.Collections.Generic
Imports System.Net

Partial Class RptPriceList
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUser As New clsUser
    Dim objUtils As New clsUtils
    Dim SqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim strSqlQry As String
    Dim document As New XLWorkbook
#End Region

#Region "Web Service Methods"
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetHotelsList(ByVal prefixText As String) As List(Of String)
        Dim bstrSqlQry As String = ""
        Dim myDS As New DataSet
        Dim PartyName As New List(Of String)
        Try
            bstrSqlQry = "select distinct M.partycode,M.partyname from partymast M inner join PriceAdults P on M.partycode=P.partycode and M.sptypecode='HOT' and M.active=1 and M.partyName like  '" & Trim(prefixText) & "%' order by M.partyname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            myDataAdapter = New SqlDataAdapter(bstrSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    PartyName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next
            End If
            Return PartyName
        Catch ex As Exception
            Return PartyName
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountriesList(ByVal prefixText As String) As List(Of String)
        Dim bstrSqlQry As String = ""
        Dim myDS As New DataSet
        Dim CtryName As New List(Of String)
        Try
            bstrSqlQry = "select distinct v.ctrycode,ct.ctryname from view_contractcountry v inner join ctrymast ct on v.ctrycode=ct.ctrycode and ct.ctryname like '" & Trim(prefixText) & "%' order by ct.ctryname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            myDataAdapter = New SqlDataAdapter(bstrSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    CtryName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next
            End If
            Return CtryName
        Catch ex As Exception
            Return CtryName
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentsList(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)
        Dim bstrSqlQry As String = ""
        Dim myDS As New DataSet
        Dim AgentName As New List(Of String)
        Try
            bstrSqlQry = "select agentcode,agentname from agentmast where active=1 and ctrycode='" & contextKey & "' and agentname like '" & Trim(prefixText) & "%' order by agentname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            myDataAdapter = New SqlDataAdapter(bstrSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    AgentName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
                Next
            End If
            Return AgentName
        Catch ex As Exception
            Return AgentName
        End Try
    End Function

#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try

                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)

                strappid = AppId.Value
                If AppId Is Nothing = False Then

                    strappname = AppName.Value
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\rptPricelist.aspx", btnadd, Button1, btnLoadReport, gv_SearchResult)
                End If

                '' Added shahul 12/06/2018
                Dim showgroup As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters(nolock) where param_id=3001")

                If showgroup = "N" Then
                    chkcolinExcel.Visible = False
                    lblgroup.Visible = False
                Else
                    chkcolinExcel.Visible = True
                    lblgroup.Visible = True
                End If
                '''''''''''


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Protected Sub btnLoadReport_Click(sender As Object, e As System.EventArgs) Handles btnLoadReport.Click"
    Protected Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReport.Click
        Try
            If chkcolinExcel.Checked Then
                ColumnFormat()
                Exit Sub
            End If




            Dim FolderPath As String = "..\ExcelTemplates\"
            Dim FileName As String = "RateSheetFormat.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)
            Dim RandomCls As New Random()
            Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString

            Dim FileNameNew As String = "RateSheetPrint_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            document = New XLWorkbook(FilePath)
            Dim ws As IXLWorksheet = document.Worksheet("CONTRACTED RATES")
            ws.Style.Font.FontName = "Times New Roman"

            Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
            SheetTemplate.Style.Font.FontName = "Times New Roman"
            Dim PartyName As String = ""
            Dim CatName As String = ""
            Dim SectorCityName As String = ""

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            Dim sqlqry = "select * from view_partymast where partycode='" + txtHotelCode.Text.Trim + "'"
            mySqlCmd = New SqlCommand(sqlqry, SqlConn)
            mySqlCmd.CommandType = CommandType.Text
            Dim mySqlReader As SqlDataReader
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.Read() Then
                ws.Cell(1, 1).Value = mySqlReader("partyname").ToString.Trim.ToUpper
                ws.Cell(2, 3).Value = mySqlReader("catname").ToString.Trim
                ws.Cell(3, 3).Value = mySqlReader("sectorname").ToString.Trim + ", " + mySqlReader("cityname").ToString.Trim
                PartyName = mySqlReader("partyname").ToString.Trim.ToUpper
                CatName = mySqlReader("catname").ToString.Trim
                SectorCityName = mySqlReader("sectorname").ToString.Trim + ", " + mySqlReader("cityname").ToString.Trim
            End If
            mySqlReader.Close()
            mySqlCmd.Dispose()
            Dim LastLine As Integer
            ws.Cell(5, 3).Value = txtCtryName.Text.Trim.ToUpper
            If txtAgentCode.Text.Trim <> "" Then
                ws.Cell(6, 3).Value = txtAgentName.Text.Trim.ToUpper
                LastLine = 7
            Else
                ws.Row(6).Delete()
                LastLine = 6
            End If

            mySqlCmd = New SqlCommand("sp_rep_contractrates", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@SourceCtryCode", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@RateType", SqlDbType.VarChar, 100)).Value = IIf(rdoNetPayable.Checked, "Net Payable", "Selling Rates")
            mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.CommandTimeout = 0
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dtContract As New DataTable
            Dim dtOffer As New DataTable
            Dim dtPromotion As New DataTable
            myDataAdapter.Fill(ds)
            dt = ds.Tables(0)
            dtContract = ds.Tables(1)
            dtOffer = ds.Tables(2)
            dtPromotion = ds.Tables(3)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()

            If dt.Rows.Count < 1 And dtPromotion.Rows.Count < 1 Then
                ModalPopupLoading.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There is no price list in this hotel' );", True)
                txtHotelName.Focus()
                Exit Sub
            End If
            If dt.Rows.Count > 0 Then
                Dim contractids As String = ""
                If dtContract.Rows.Count > 0 Then
                    contractids = dtContract.Rows(0).Item("contractids")
                    ws.Cell(4, 3).Value = contractids
                End If

                Dim offerRange As IXLRange
                Dim ContractLink As Integer = LastLine + 3
                Dim SpecialLink As Integer = LastLine + 4
                Dim CancelLink As Integer = LastLine + 5
                Dim CheckInOutLink As Integer = LastLine + 6
                Dim GeneralLink As Integer = LastLine + 7
                Dim LinkLastLine As Integer = LastLine + 7
                Dim OfferLastLine As Integer
                Dim hideList As New List(Of Integer)
                LastLine = LastLine + 3
                Dim cnt As Integer = LastLine
                Dim offerFirstRow As Integer
                If dtOffer.Rows.Count > 0 Then
                    If dtOffer.Rows.Count > 4 Then
                        LastLine = LastLine + 6
                        ws.Row(LastLine).InsertRowsAbove(dtOffer.Rows.Count - 4)
                        ws.Range(LastLine, 1, LastLine + dtOffer.Rows.Count - 4, 14).Style.Fill.SetBackgroundColor(XLColor.White)
                        LastLine = LastLine + dtOffer.Rows.Count - 4
                    Else
                        LastLine = LastLine + 6
                    End If
                    ws.Cell(cnt, 4).Value = "Promotion ID"
                    ws.Cell(cnt, 5).Value = "Contract ID"
                    ws.Cell(cnt, 6).Value = "Promotion Name"
                    ws.Range(cnt, 6, cnt, 13).Merge()
                    offerFirstRow = cnt + 1
                    ws.Range(cnt, 4, cnt, 13).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 4, cnt, 13).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(cnt, 4, cnt, 13).FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                    For Each rs As DataRow In dtOffer.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 4).Value = rs(0)
                        ws.Cell(cnt, 5).Value = rs(1)
                        ws.Cell(cnt, 6).Value = rs(2)
                        ws.Range(cnt, 6, cnt, 13).Merge()
                    Next
                    offerRange = ws.Range(offerFirstRow, 4, cnt, 13)
                    offerRange.Style.Alignment.SetWrapText(True)
                    offerRange.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    offerRange.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    offerRange.FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                    offerRange.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    offerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                Else
                    ws.Row(LastLine - 1).Cell(4).Clear()
                    ws.Range(LastLine - 1, 4, LastLine - 1, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = LastLine + 6
                End If
                OfferLastLine = LastLine - 2
                '--------------- Tax inclusive Note  ----------------------
                ws.Row(LastLine).InsertRowsBelow(2)
                TaxNoteTable(ws, LastLine, 13)
                '---------------------- End --------------------
                LastLine = LastLine + 4
                cnt = LastLine
                If rdoNetPayable.Checked Then
                    ws.Cell(cnt - 2, 6).Value = "Net Payable"
                    If ws.Column(6).Width < ws.Cell(cnt - 2, 6).Value.ToString.Length Then
                        ws.Column(6).Width = ws.Cell(cnt - 2, 6).Value.ToString.Length + 2
                    End If
                End If
                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(cnt, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(cnt).Clear()
                ws.Rows(cnt).Delete()
                RateSheet.Style.Font.FontName = "Times New Roman"
                RateSheet.Columns(6, 12).SetDataType(XLCellValues.Number)
                RateSheet.Columns(1, 5).SetDataType(XLCellValues.Text)
                RateSheet.Columns(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin
                RateSheet.Columns("1:3,5,11").Style.Border.RightBorder = XLBorderStyleValues.Medium
                For i = 1 To dt.Rows.Count
                    If RateSheet.Cell(i, 1).Value <> RateSheet.Cell(i + 1, 1).Value Or RateSheet.Cell(i, 2).Value <> RateSheet.Cell(i + 1, 2).Value Or RateSheet.Cell(i, 3).Value <> RateSheet.Cell(i + 1, 3).Value Then
                        RateSheet.Rows(i).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                    End If
                Next
                For Each rng As IXLCell In RateSheet.Columns(6, 11).Cells
                    If rng.Value = 0 Then
                        rng.Value = ""
                    End If
                Next
                ws.Columns(6, 11).Width = 12
                Dim showcol As Integer = 6
                Dim col As Integer = 5
                For Each kol As IXLRangeColumn In RateSheet.Columns(6, 11)
                    col = col + 1
                    If kol.IsEmpty() Then
                        ws.Column(col).Hide()
                        hideList.Add(col)
                        showcol = showcol - 1
                    End If
                Next
                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium
                ws.Columns("1,3:5,13").AdjustToContents()
                ws.Row(ContractLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine - 3).ToString + ":M" + RateSheet.LastRow.WorksheetRow.RowNumber().ToString
                ws.Row(ContractLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                Dim mergeCalwidth As Integer = showcol * 12
                mergeCalwidth = mergeCalwidth + ws.Column(12).Width + ws.Column(13).Width
                If dtOffer.Rows.Count > 0 Then
                    cnt = offerFirstRow
                    For Each rs As DataRow In dtOffer.Rows
                        Dim rowUyaram As Integer = Math.Ceiling(rs(2).ToString.Length / mergeCalwidth) * 15.2
                        ws.Row(cnt).Height = rowUyaram + 15
                        cnt = cnt + 1
                    Next
                End If
                LastLine = RateSheet.LastRow.WorksheetRow.RowNumber()
                ws.Range(LastLine + 1, 1, LastLine + 2, 14).Style.Fill.SetBackgroundColor(XLColor.White)

                mySqlCmd = New SqlCommand("sp_rep_specialevents", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@RateType", SqlDbType.VarChar, 100)).Value = IIf(rdoNetPayable.Checked, "Net Payable", "Selling Rates")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtSpecialEvt As New DataTable
                myDataAdapter.Fill(dtSpecialEvt)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                If dtSpecialEvt.Rows.Count > 0 Then
                    'LastLine = LastLine + 3
                    '--------------- Tax inclusive Note  ----------------------
                    LastLine = LastLine + 2
                    TaxNoteTable(ws, LastLine, 13)
                    LastLine = LastLine + 1
                    '---------------------- End --------------------
                    Dim specialFirstRow As Integer = LastLine
                    ws.Cell(LastLine, 1).Value = "SPECIAL EVENTS LIST"
                    ws.Range(LastLine, 1, LastLine, 13).Merge()
                    TitleStyle(ws.Cell(LastLine, 1))
                    Dim distinctSeDt As DataTable = dtSpecialEvt.DefaultView.ToTable(True, "CompulsoryType")
                    Dim colList As New List(Of Integer)
                    For i = 1 To 5
                        colList.Add(i)
                    Next
                    Dim ColNo As Integer = 6
                    Do While (colList.Count < 9)
                        If hideList.IndexOf(ColNo) < 0 Then
                            colList.Add(ColNo)
                        End If
                        ColNo = ColNo + 1
                    Loop
                    Dim seEvtTitle As New List(Of String)
                    seEvtTitle.Add("Special Events Name")
                    seEvtTitle.Add("Special Events Date")
                    seEvtTitle.Add("Room Types")
                    seEvtTitle.Add("Meal Plans")
                    seEvtTitle.Add("Room Occupancy")
                    seEvtTitle.Add("Adult Rate")
                    seEvtTitle.Add("Age From")
                    seEvtTitle.Add("Age To")
                    seEvtTitle.Add("Rate")
                    For Each dr As DataRow In distinctSeDt.Rows
                        Dim filterDr As DataRow()
                        Dim filterDt As DataTable = Nothing
                        filterDt = dtSpecialEvt.Clone()
                        filterDr = dtSpecialEvt.Select("CompulsoryType='" + dr(0).ToString() + "'")
                        For Each row As DataRow In filterDr
                            filterDt.ImportRow(row)
                        Next
                        filterDt.Columns.Remove("spleventid")
                        filterDt.Columns.Remove("contpromid")
                        filterDt.Columns.Remove("CompulsoryType")
                        LastLine = LastLine + 2
                        ws.Range(LastLine - 1, 1, LastLine, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                        ws.Cell(LastLine, 1).Value = "Compulsory Options"
                        ws.Cell(LastLine, 2).Value = dr("CompulsoryType")
                        ws.Cell(LastLine, 1).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.SetBold(True).Fill.SetBackgroundColor(XLColor.BlueGray).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Range(LastLine, 2, LastLine, 3).Merge().Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.SetBold(True).Fill.SetBackgroundColor(XLColor.GreenYellow).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        LastLine = LastLine + 3
                        ws.Range(LastLine - 2, 1, LastLine - 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                        Dim j As Integer = 0
                        For Each i As Integer In colList
                            If j <= 5 Then
                                ws.Range(LastLine - 1, colList(j), LastLine, colList(j)).Merge().SetValue(seEvtTitle(j)).Style.Fill.SetBackgroundColor(XLColor.BlueGray)
                            ElseIf j = 6 Then
                                ws.Range(LastLine - 1, colList(j), LastLine - 1, colList(j + 2)).Merge().SetValue("Child Rate").Style.Fill.SetBackgroundColor(XLColor.GreenYellow)
                                ws.Cell(LastLine, colList(j)).SetValue(seEvtTitle(j)).Style.Fill.SetBackgroundColor(XLColor.BlueGray)
                            Else
                                ws.Cell(LastLine, colList(j)).SetValue(seEvtTitle(j)).Style.Fill.SetBackgroundColor(XLColor.BlueGray)
                            End If
                            j = j + 1
                        Next
                        ws.Range(LastLine - 1, 1, LastLine, colList(j - 1)).Style.Alignment.SetWrapText(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11)
                        ws.Row(LastLine).AdjustToContents()
                        ws.Range(LastLine - 1, 1, LastLine, colList(j - 1)).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Font.SetBold(True)
                        LastLine = LastLine + 1
                        cnt = LastLine

                        For Each ftDr As DataRow In filterDt.Rows
                            Dim colID As Integer = 0
                            For Each i As Integer In colList
                                If i = 2 Then
                                    ws.Cell(cnt, i).Value = ftDr(colID).ToString()
                                    ws.Cell(cnt, i).Style.NumberFormat.Format = "dd/MM/yyyy"
                                Else
                                    ws.Cell(cnt, i).Value = ftDr(colID).ToString()
                                End If
                                colID = colID + 1
                            Next
                            cnt = cnt + 1
                        Next
                        cnt = cnt - 1
                        Dim rngSE As IXLRange
                        rngSE = ws.Range(LastLine, 1, cnt, colList(8))
                        rngSE.Style.Alignment.SetWrapText(True)
                        Dim mFCnt As Integer = LastLine
                        Dim mLCnt As Integer = LastLine
                        For i = LastLine To cnt - 1
                            If ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value And ws.Cell(i, 2).Value = ws.Cell(i + 1, 2).Value And ws.Cell(i, 3).Value = ws.Cell(i + 1, 3).Value _
                                And ws.Cell(i, 4).Value = ws.Cell(i + 1, 4).Value And ws.Cell(i, 5).Value = ws.Cell(i + 1, 5).Value And ws.Cell(i, colList(5)).Value = ws.Cell(i + 1, colList(5)).Value Then
                                mLCnt = i + 1
                            Else
                                If mFCnt <> mLCnt Then
                                    For m As Integer = 0 To 5
                                        ws.Range(mFCnt, colList(m), mLCnt, colList(m)).Merge()
                                    Next
                                    Dim tmpH As Integer = Math.Ceiling(Math.Ceiling(ws.Cell(mFCnt, 3).Value.ToString.Length / ws.Column(3).Width) * 15 / (mLCnt - mFCnt))
                                    If tmpH < 18 Then tmpH = 18
                                    ws.Rows(mFCnt, mLCnt).Height = tmpH
                                Else
                                    ws.Row(i).Style.Alignment.SetWrapText(True)
                                End If
                                mFCnt = i + 1
                                mLCnt = i + 1
                            End If
                        Next
                        If mFCnt <> mLCnt Then
                            For m As Integer = 0 To 5
                                ws.Range(mFCnt, colList(m), mLCnt, colList(m)).Merge()
                            Next
                            Dim tmpH As Integer = Math.Ceiling(Math.Ceiling(ws.Cell(mFCnt, 3).Value.ToString.Length / ws.Column(3).Width) * 15 / (mLCnt - mFCnt))
                            If tmpH < 18 Then tmpH = 18
                            ws.Rows(mFCnt, mLCnt).Height = tmpH
                        Else
                            ws.Row(mFCnt).Style.Alignment.SetWrapText(True)
                        End If
                        rngSE.Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Font.SetFontName("Times New Roman").Font.SetFontSize(11)
                        rngSE.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        rngSE.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        rngSE.LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                        LastLine = cnt
                    Next
                    ws.Row(SpecialLink).Cell(1).Hyperlink.InternalAddress = "A" + (specialFirstRow).ToString + ":M" + LastLine.ToString
                    ws.Row(SpecialLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                    ws.Range(LastLine + 1, 1, LastLine + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                Else
                    For i = SpecialLink To GeneralLink - 1
                        ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value
                    Next
                    CancelLink = CancelLink - 1
                    CheckInOutLink = CheckInOutLink - 1
                    GeneralLink = GeneralLink - 1
                    ws.Range(GeneralLink, 1, GeneralLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    If OfferLastLine > LinkLastLine Then
                        ws.Cell(GeneralLink + 1, 1).Value = ""
                        ws.Range(GeneralLink + 1, 1, GeneralLink + 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                    Else
                        ws.Row(GeneralLink + 1).Delete()
                        LastLine = LastLine - 1
                        LinkLastLine = LinkLastLine - 1
                    End If
                End If

                mySqlCmd = New SqlCommand("sp_rep_cancelpolicy", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtPolicy As New DataTable
                myDataAdapter.Fill(dtPolicy)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                If dtPolicy.Rows.Count > 0 Then
                    Dim canPolicy As IXLRange
                    LastLine = LastLine + 3
                    cnt = LastLine
                    ws.Range(cnt, 1, cnt, 13).Merge()
                    ws.Cell(cnt, 1).Value = "CANCELLATION / NO SHOW / EARLY CHECKOUT - POLICY"
                    TitleStyle(ws.Cell(cnt, 1))
                    ws.Range(cnt, 1, cnt, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Season"
                    ws.Cell(cnt, 2).Value = "Period"
                    ws.Cell(cnt, 3).Value = "Applicable Room Types"
                    ws.Cell(cnt, 4).Value = "Meal Plan"
                    ws.Range(cnt, 4, cnt, 5).Merge()
                    ws.Cell(cnt, 6).Value = "Cancellation / No Show / Early Checkout - Policy"
                    ws.Range(cnt, 6, cnt, 13).Merge()
                    ws.Cell(cnt, 6).Style.Alignment.SetWrapText(True)
                    If ws.Cell(cnt, 6).Value.ToString.Trim.Length > mergeCalwidth Then
                        ws.Row(cnt).Height = Math.Ceiling(ws.Cell(cnt, 6).Value.ToString.Length / mergeCalwidth) * 15.2
                    End If
                    ws.Range(cnt, 1, cnt, 13).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(cnt, 1, cnt, 13).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Columns(2).Width = 12
                    Dim pstartRow = cnt + 1
                    For Each rs As DataRow In dtPolicy.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Cell(cnt, 2).Value = rs(1)
                        If ws.Cell(cnt, 2).Value.ToString.Substring(0, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(0, 1)
                        If ws.Cell(cnt, 2).Value.ToString.Substring(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1)
                        ws.Cell(cnt, 3).Value = rs(2)
                        ws.Cell(cnt, 4).Value = rs(3)
                        ws.Range(cnt, 4, cnt, 5).Merge()
                        ws.Cell(cnt, 6).Value = rs(4)
                        ws.Range(cnt, 6, cnt, 13).Merge()
                        'If ws.Cell(cnt, 6).Value.ToString.Substring(0, 1) = """" Then ws.Cell(cnt, 6).Value = ws.Cell(cnt, 6).Value.ToString.Remove(0, 1)
                        'If ws.Cell(cnt, 6).Value.ToString.Substring(ws.Cell(cnt, 6).Value.ToString.Length - 1, 1) = """" Then ws.Cell(cnt, 6).Value = ws.Cell(cnt, 6).Value.ToString.Remove(ws.Cell(cnt, 6).Value.ToString.Length - 1, 1)
                        FormatSpecificString(ws.Cell(cnt, 6))
                        Dim rowH2 = Math.Ceiling(rs(1).ToString.Length / ws.Column(2).Width) * 15.2
                        Dim rowH3 = Math.Ceiling(rs(2).ToString.Length / ws.Column(3).Width) * 15.2
                        Dim rowH6 = Math.Ceiling(rs(4).ToString.Length / mergeCalwidth) * 15.2
                        Dim rowUyaram As Integer
                        If rowH2 > rowH3 Then rowUyaram = rowH2 Else rowUyaram = rowH3
                        If rowUyaram < rowH6 Then
                            rowUyaram = rowH6
                            ws.Row(cnt).Height = rowUyaram + 15
                        End If
                    Next
                    canPolicy = ws.Range(pstartRow, 1, cnt, 13)
                    canPolicy.Style.Alignment.SetWrapText(True)
                    canPolicy.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    canPolicy.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    canPolicy.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    canPolicy.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Row(CancelLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine).ToString + ":M" + canPolicy.LastRow.WorksheetRow.RowNumber().ToString
                    ws.Row(CancelLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                    Dim patternRow As Integer = canPolicy.LastRow.RowNumber
                    'ws.Range(1, 14, patternRow, 14).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(patternRow + 1, 1, patternRow + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = patternRow
                Else
                    For i = CancelLink To GeneralLink - 1
                        ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value
                    Next
                    CheckInOutLink = CheckInOutLink - 1
                    GeneralLink = GeneralLink - 1
                    ws.Range(GeneralLink, 1, GeneralLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    If OfferLastLine > LinkLastLine Then
                        ws.Cell(GeneralLink + 1, 1).Value = ""
                        ws.Range(GeneralLink + 1, 1, GeneralLink + 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                    Else
                        ws.Row(GeneralLink + 1).Delete()
                        LastLine = LastLine - 1
                        LinkLastLine = LinkLastLine - 1
                    End If
                End If

                CheckInCheckOutPolicy(ws, contractids, LastLine, hideList, CheckInOutLink, GeneralLink, OfferLastLine, LinkLastLine)

                mySqlCmd = New SqlCommand("sp_rep_generalpolicy", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtGenPolicy As New DataTable
                myDataAdapter.Fill(dtGenPolicy)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                If dtGenPolicy.Rows.Count > 0 Then
                    LastLine = LastLine + 3
                    ws.Cell(LastLine, 1).Value = "GENERAL POLICY"
                    ws.Range(LastLine, 1, LastLine, 13).Merge()
                    TitleStyle(ws.Cell(LastLine, 1))
                    LastLine = LastLine + 1
                    ws.Cell(LastLine, 1).Value = "From Date"
                    ws.Cell(LastLine, 2).Value = "To Date"
                    ws.Cell(LastLine, 3).Value = "Policy"
                    ws.Range(LastLine, 3, LastLine, 13).Merge()
                    ws.Range(LastLine, 1, LastLine, 13).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    ws.Range(LastLine, 1, LastLine, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    LastLine = LastLine + 1
                    cnt = LastLine

                    Dim genCols As New List(Of String)(New String() {"fromdate", "todate", "policytext"})
                    Dim findCol As Integer = 0
                    While findCol < dtGenPolicy.Columns.Count
                        Dim strColName As String = dtGenPolicy.Columns(findCol).ColumnName.ToLower()
                        If Not genCols.Contains(strColName) Then
                            dtGenPolicy.Columns.Remove(strColName)
                        Else
                            findCol += 1
                        End If
                    End While

                    Dim rngGen As IXLRange
                    rngGen = ws.Cell(cnt, 1).InsertData(dtGenPolicy.AsEnumerable).AsRange()
                    rngGen.Style.Alignment.SetWrapText(True)
                    Dim cntRow As Integer = rngGen.LastRow.RowNumber
                    mergeCalwidth = mergeCalwidth + ws.Column(3).Width + ws.Column(4).Width + ws.Column(5).Width
                    Dim i As Integer = cnt
                    While i <= cntRow
                        If (Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) > 350 Then
                            Dim Nrows As Integer = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / 350)
                            ws.Row(i).InsertRowsBelow(Nrows)
                            ws.Range(i, 1, i + Nrows, 1).Merge()
                            ws.Range(i, 2, i + Nrows, 2).Merge()
                            ws.Range(i, 3, i + Nrows, 13).Merge()
                            ws.Rows(i, i + Nrows).Height = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / (Nrows + 1))
                            i = i + Nrows
                            cntRow = cntRow + Nrows
                        Else
                            ws.Range(i, 3, i, 13).Merge()
                            ws.Row(i).Height = Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150
                        End If
                        i = i + 1
                    End While
                    ws.Range(cnt, 1, cntRow, 13).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(cnt, 1, cntRow, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 1, cntRow, 13).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(cnt, 1, cntRow, 13).LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    ws.Row(GeneralLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine - 1).ToString + ":M" + cntRow.ToString
                    ws.Row(GeneralLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                    LastLine = cntRow
                    ws.Range(LastLine + 1, 1, LastLine + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                Else
                    ws.Range(GeneralLink - 1, 1, GeneralLink - 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    If OfferLastLine > LinkLastLine Then
                        ws.Cell(GeneralLink, 1).Value = ""
                        ws.Range(GeneralLink, 1, GeneralLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                    Else
                        ws.Row(GeneralLink).Delete()
                        LastLine = LastLine - 1
                        LinkLastLine = LinkLastLine - 1
                    End If
                End If

                mySqlCmd = New SqlCommand("sp_rep_hotelconstruction", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtConstruct As New DataTable
                myDataAdapter.Fill(dtConstruct)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                SqlConn.Close()
                If dtConstruct.Rows.Count > 0 Then
                    LastLine = LastLine + 3
                    ws.Cell(LastLine, 1).Value = "HOTEL CONSTRUCTION"
                    ws.Range(LastLine, 1, LastLine, 13).Merge()
                    TitleStyle(ws.Cell(LastLine, 1))
                    LastLine = LastLine + 1
                    ws.Cell(LastLine, 1).Value = "From Date"
                    ws.Cell(LastLine, 2).Value = "To Date"
                    ws.Cell(LastLine, 3).Value = "Construction"
                    ws.Range(LastLine, 3, LastLine, 13).Merge()
                    ws.Range(LastLine, 1, LastLine, 13).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    ws.Range(LastLine, 1, LastLine, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    LastLine = LastLine + 1
                    cnt = LastLine

                    Dim constCols As New List(Of String)(New String() {"fromdate", "todate", "constructiontext"})
                    Dim findCol As Integer = 0
                    While findCol < dtConstruct.Columns.Count
                        Dim strColName As String = dtConstruct.Columns(findCol).ColumnName.ToLower()
                        If Not constCols.Contains(strColName) Then
                            dtConstruct.Columns.Remove(strColName)
                        Else
                            findCol += 1
                        End If
                    End While

                    Dim rngCons As IXLRange
                    rngCons = ws.Cell(cnt, 1).InsertData(dtConstruct.AsEnumerable).AsRange()
                    rngCons.Style.Alignment.SetWrapText(True)
                    Dim cntRow As Integer = rngCons.LastRow.RowNumber
                    Dim i As Integer = cnt
                    While i <= cntRow
                        If (Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) > 350 Then
                            Dim Nrows As Integer = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / 350)
                            ws.Row(i).InsertRowsBelow(Nrows)
                            ws.Range(i, 1, i + Nrows, 1).Merge()
                            ws.Range(i, 2, i + Nrows, 2).Merge()
                            ws.Range(i, 3, i + Nrows, 13).Merge()
                            ws.Rows(i, i + Nrows).Height = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / (Nrows + 1))
                            i = i + Nrows
                            cntRow = cntRow + Nrows
                        Else
                            ws.Range(i, 3, i, 13).Merge()
                            ws.Row(i).Height = Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150
                        End If
                        i = i + 1
                    End While
                    ws.Range(cnt, 1, cntRow, 13).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(cnt, 1, cntRow, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 1, cntRow, 13).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(cnt, 1, cntRow, 13).LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    LastLine = cntRow
                    ws.Range(LastLine + 1, 1, LastLine + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                End If

                ws.Protect(RandomNo)
                ws.Protection.FormatColumns = True
                ws.Protection.FormatRows = True
            Else
                ws.Dispose()
                document.Worksheet("CONTRACTED RATES").Delete()
            End If
            Dim sheetPos As Integer = 2
            For Each pDr As DataRow In dtPromotion.Rows
                Dim sheetName As String = CType(sheetPos - 1, String) & " " & pDr("sheetName")
                sheetName = Regex.Replace(sheetName, "[-=\]\[;./~!@#$%^*()_+{}|:?\\\n]", "_")
                If (sheetName.Length > 31) Then sheetName = sheetName.Substring(0, 31)
                Dim sheet As IXLWorksheet = SheetTemplate.CopyTo(document, sheetName, sheetPos)
                FillOffer(sheet, PartyName, CatName, SectorCityName, pDr)
                sheet.Protect(RandomNo)
                sheet.Protection.FormatColumns = True
                sheet.Protection.FormatRows = True
                sheetPos = sheetPos + 1
            Next
            SheetTemplate.Dispose()
            document.Worksheet("Offer Template").Delete()
            Using MyMemoryStream As New MemoryStream()
                document.SaveAs(MyMemoryStream)
                document.Dispose()
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                'Response.BinaryWrite(MyMemoryStream.GetBuffer())
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                Response.Flush()
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End Using
        Catch ex As Exception
            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Protected Sub ColumnFormat()"
    Protected Sub ColumnFormat()



        Try
            Dim FolderPath As String = "..\ExcelTemplates\"
            Dim FileName As String = "RateSheetdateFormat.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)
            Dim RandomCls As New Random()
            Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString

            Dim FileNameNew As String = "RateSheetPrint_DateFormat_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            document = New XLWorkbook(FilePath)
            Dim ws As IXLWorksheet = document.Worksheet("CONTRACTED RATES")
            ws.Style.Font.FontName = "Trebuchet MS"

            Dim SheetTemplate As IXLWorksheet = New XLWorkbook(FilePath).Worksheet("Offer Template")
            SheetTemplate.Style.Font.FontName = "Trebuchet MS"
            Dim PartyName As String = ""
            Dim CatName As String = ""
            Dim SectorCityName As String = ""

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            Dim sqlqry = "select * from view_partymast where partycode='" + txtHotelCode.Text.Trim + "'"
            mySqlCmd = New SqlCommand(sqlqry, SqlConn)
            mySqlCmd.CommandType = CommandType.Text
            Dim mySqlReader As SqlDataReader
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.Read() Then
                ws.Cell(1, 1).Value = mySqlReader("partyname").ToString.Trim.ToUpper
                ws.Cell(2, 3).Value = mySqlReader("catname").ToString.Trim
                ws.Cell(3, 3).Value = mySqlReader("sectorname").ToString.Trim + ", " + mySqlReader("cityname").ToString.Trim
                PartyName = mySqlReader("partyname").ToString.Trim.ToUpper
                CatName = mySqlReader("catname").ToString.Trim
                SectorCityName = mySqlReader("sectorname").ToString.Trim + ", " + mySqlReader("cityname").ToString.Trim
            End If
            mySqlReader.Close()
            mySqlCmd.Dispose()
            Dim LastLine As Integer
            ws.Cell(5, 3).Value = txtCtryName.Text.Trim.ToUpper
            If txtAgentCode.Text.Trim <> "" Then
                ws.Cell(6, 3).Value = txtAgentName.Text.Trim.ToUpper
                LastLine = 7
            Else
                ws.Row(6).Delete()
                LastLine = 6
            End If

            mySqlCmd = New SqlCommand("sp_rep_contractrates_groupdates", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@SourceCtryCode", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@RateType", SqlDbType.VarChar, 100)).Value = IIf(rdoNetPayable.Checked, "Net Payable", "Selling Rates")
            mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.CommandTimeout = 0
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dtContract As New DataTable
            Dim dtOffer As New DataTable
            Dim dtPromotion As New DataTable
            Dim dtcolumns As New DataTable
            myDataAdapter.Fill(ds)

            'dt = ds.Tables(5)
            'dtContract = ds.Tables(2)
            'dtOffer = ds.Tables(3)
            'dtPromotion = ds.Tables(4)
            'dtcolumns = ds.Tables(6)



            dt = ds.Tables(3)
            dtContract = ds.Tables(0)
            dtOffer = ds.Tables(1)
            dtPromotion = ds.Tables(2)

            'If dt.Rows.Count >= 1 And dtPromotion.Rows.Count >= 1 Then
            dtcolumns = ds.Tables(4)
            ' End If

            mySqlCmd.Dispose()
            myDataAdapter.Dispose()

            'dt = ds.Tables(0)
            'dtContract = ds.Tables(1)
            'dtOffer = ds.Tables(2)
            'dtPromotion = ds.Tables(3)
            'mySqlCmd.Dispose()
            'myDataAdapter.Dispose()

            If dt.Rows.Count < 1 And dtPromotion.Rows.Count < 1 Then
                ModalPopupLoading.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There is no price list in this hotel' );", True)
                txtHotelName.Focus()
                Exit Sub
            End If
            If dt.Rows.Count > 0 Then
                Dim contractids As String = ""
                If dtContract.Rows.Count > 0 Then
                    contractids = dtContract.Rows(0).Item("contractids")
                    ws.Cell(4, 3).Value = contractids
                End If

                Dim offerRange As IXLRange
                Dim ContractLink As Integer = LastLine + 3
                Dim SpecialLink As Integer = LastLine + 4
                Dim CancelLink As Integer = LastLine + 5
                Dim CheckInOutLink As Integer = LastLine + 6
                Dim GeneralLink As Integer = LastLine + 7
                Dim LinkLastLine As Integer = LastLine + 7
                Dim OfferLastLine As Integer
                Dim hideList As New List(Of Integer)
                LastLine = LastLine + 3
                Dim cnt As Integer = LastLine
                Dim offerFirstRow As Integer
                If dtOffer.Rows.Count > 0 Then
                    If dtOffer.Rows.Count > 4 Then
                        LastLine = LastLine + 6
                        ws.Row(LastLine).InsertRowsAbove(dtOffer.Rows.Count - 4)
                        ' ws.Range(LastLine, 1, LastLine + dtOffer.Rows.Count - 4, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                        LastLine = LastLine + dtOffer.Rows.Count - 4
                    Else
                        LastLine = LastLine + 6
                    End If

                    Dim promocol As String
                    If dt.Columns.Count < 8 Then
                        promocol = dt.Columns.Count
                        ws.Range(cnt - 1, 4, cnt - 1, dt.Columns.Count).Merge()
                    Else
                        promocol = 8
                        ws.Range(cnt - 1, 4, cnt - 1, 8).Merge()
                    End If
                    offerRange = ws.Range(cnt - 1, 4, cnt - 1, promocol)
                    offerRange.FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                    offerRange.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)

                    ws.Cell(cnt, 4).Value = "Promotion ID"
                    ws.Cell(cnt, 5).Value = "Contract ID"
                    ws.Cell(cnt, 6).Value = "Promotion Name"
                    ws.Range(cnt, 6, cnt, promocol).Merge()
                    offerFirstRow = cnt + 1
                    ws.Range(cnt, 4, cnt, promocol).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 4, cnt, promocol).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(cnt, 4, cnt, promocol).FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                    For Each rs As DataRow In dtOffer.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 4).Value = rs(0)
                        ws.Cell(cnt, 5).Value = rs(1)
                        ws.Cell(cnt, 6).Value = rs(2)
                        ws.Range(cnt, 6, cnt, promocol).Merge()

                    Next



                    offerRange = ws.Range(offerFirstRow, 4, cnt, promocol)
                    offerRange.Style.Alignment.SetWrapText(True)
                    offerRange.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    offerRange.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    offerRange.FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                    offerRange.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    offerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                Else
                    ws.Row(LastLine - 1).Cell(4).Clear()
                    ws.Range(LastLine - 1, 3, LastLine - 1, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(LastLine, 1, LastLine + dtOffer.Rows.Count - 2, dt.Columns.Count - 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.None)
                    LastLine = LastLine + 6
                End If
                OfferLastLine = LastLine - 2
                '--------------- Tax inclusive Note  ----------------------
                ws.Row(LastLine).InsertRowsBelow(2)
                TaxNoteTable(ws, LastLine, dt.Columns.Count)
                '---------------------- End --------------------
                LastLine = LastLine + 4
                cnt = LastLine

                '---- creating dynamic columns---'
                Dim colname As Integer
                colname = 4

                ws.Cell(cnt + 1, 1).Value = "Room Type"


                ws.Cell(cnt + 1, 2).Value = "Room Category"


                ws.Cell(cnt + 1, 3).Value = "Age Combination"




                For Each rs As DataRow In dtcolumns.Rows
                    ws.Cell(cnt + 1, colname).Value = rs(1).ToString.Replace(";", "").Trim
                    ws.Cell(cnt + 1, colname).Value = ws.Cell(cnt + 1, colname).Value + vbNewLine + rs(8).ToString.Replace(";", vbNewLine)
                    'ws.Cell(cnt + 1, colname).Value = rs(4) & vbNewLine & Regex.Replace(ws.Cell(cnt + 1, colname).Value, "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]", "") & vbNewLine & "Minstay & MinstayType:" & rs(2).ToString & " &" & rs(3).ToString
                    ws.Cell(cnt + 1, colname).Value = Regex.Replace(ws.Cell(cnt + 1, colname).Value, "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]", "") & vbNewLine & "Minstay & MinstayType:" & rs(2).ToString

                    If rs(3).ToString.Length > 0 Then
                        ws.Cell(cnt + 1, colname).Value = ws.Cell(cnt + 1, colname).Value & " &" & rs(3).ToString
                    End If
                    ws.Cell(cnt + 1, colname).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                    ws.Cell(cnt + 1, colname).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                    ws.Cell(cnt + 1, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    ws.Cell(cnt + 1, colname).Style.Alignment.WrapText = True
                    ws.Row(cnt + 1).AdjustToContents()
                    ws.Cell(cnt + 1, colname).Style.Font.Bold = True
                    ws.Row(cnt + 1).Height = 85


                    ws.Cell(cnt + 3, colname).Value = rs(4)
                    ws.Cell(cnt + 3, colname).Style.Font.Bold = True
                    ws.Cell(cnt + 3, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    ws.Column(colname).Width = 25.3


                    ws.Rows(cnt).Style.Border.BottomBorder = XLBorderStyleValues.Medium

                    ws.Cell(cnt + 2, colname).Style.Font.Bold = True
                    ws.Cell(cnt + 2, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

                    If rdoNetPayable.Checked Then
                        ws.Cell(cnt + 2, colname).Value = "Net Payable"
                    Else

                        ws.Cell(cnt + 2, colname).Value = "Sale Price"
                    End If
                    colname = colname + 1


                Next



                ws.Row(cnt + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                ws.Range(cnt, 1, cnt + 3, dt.Columns.Count).Style.Font.SetFontColor(XLColor.White)



                ws.Range(cnt - 1, 1, cnt - 1, dt.Columns.Count).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                ws.Range(cnt - 1, 1, cnt + 3, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.MediumElectricBlue)

                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(cnt + 4, 1).InsertTable(dt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(cnt).Clear()
                ws.Rows(cnt).Delete()

                RateSheet.Style.Font.FontName = "Trebuchet MS"
                RateSheet.Columns(4, dt.Columns.Count).SetDataType(XLCellValues.Number)
                RateSheet.Columns(1, 3).SetDataType(XLCellValues.Text)
                RateSheet.Columns(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin
                ' RateSheet.Columns("1:3,5,11").Style.Border.RightBorder = XLBorderStyleValues.Medium
                For i = 1 To dt.Rows.Count
                    If RateSheet.Cell(i, 1).Value = RateSheet.Cell(i + 1, 1).Value And RateSheet.Cell(i, 2).Value <> RateSheet.Cell(i + 1, 2).Value Then
                        RateSheet.Rows(i).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                    End If
                    If RateSheet.Cell(i, 1).Value <> RateSheet.Cell(i + 1, 1).Value Then
                        RateSheet.Rows(i).Style.Border.BottomBorder = XLBorderStyleValues.Thick
                    End If
                Next
                For Each rng As IXLCell In RateSheet.Columns(4, dt.Columns.Count).Cells
                    If rng.Value = 0 Then
                        rng.Value = ""
                    End If
                Next
                'ws.Columns(6, 11).Width = 12
                Dim showcol As Integer = 6
                Dim col As Integer = 3
                For Each kol As IXLRangeColumn In RateSheet.Columns(4, dt.Columns.Count)
                    col = col + 1
                    If kol.IsEmpty() Then
                        ws.Column(col).Hide()
                        hideList.Add(col)
                        showcol = showcol - 1
                    End If
                Next
                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium
                ws.Columns("1,3:5,13").AdjustToContents()
                ws.Row(ContractLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine - 3).ToString + ":M" + RateSheet.LastRow.WorksheetRow.RowNumber().ToString
                ws.Row(ContractLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                Dim mergeCalwidth As Integer = showcol * 12
                mergeCalwidth = mergeCalwidth + ws.Column(12).Width + ws.Column(13).Width
                If dtOffer.Rows.Count > 0 Then
                    cnt = offerFirstRow
                    For Each rs As DataRow In dtOffer.Rows
                        Dim rowUyaram As Integer = Math.Ceiling(rs(2).ToString.Length / mergeCalwidth) * 15.2
                        ws.Row(cnt).Height = rowUyaram + 15
                        cnt = cnt + 1
                    Next
                End If
                LastLine = RateSheet.LastRow.WorksheetRow.RowNumber()
                ws.Range(LastLine + 1, 1, LastLine + 2, 14).Style.Fill.SetBackgroundColor(XLColor.White)

                mySqlCmd = New SqlCommand("sp_rep_specialevents", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@RateType", SqlDbType.VarChar, 100)).Value = IIf(rdoNetPayable.Checked, "Net Payable", "Selling Rates")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtSpecialEvt As New DataTable
                myDataAdapter.Fill(dtSpecialEvt)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                If dtSpecialEvt.Rows.Count > 0 Then
                    'LastLine = LastLine + 3
                    '--------------- Tax inclusive Note  ----------------------
                    LastLine = LastLine + 2
                    TaxNoteTable(ws, LastLine, 13)
                    LastLine = LastLine + 1
                    '---------------------- End --------------------
                    Dim specialFirstRow As Integer = LastLine
                    ws.Cell(LastLine, 1).Value = "SPECIAL EVENTS LIST"
                    ws.Range(LastLine, 1, LastLine, 13).Merge()
                    TitleStyle(ws.Cell(LastLine, 1))
                    Dim distinctSeDt As DataTable = dtSpecialEvt.DefaultView.ToTable(True, "CompulsoryType")
                    Dim colList As New List(Of Integer)
                    For i = 1 To 5
                        colList.Add(i)
                    Next
                    Dim ColNo As Integer = 6
                    Do While (colList.Count < 9)
                        If hideList.IndexOf(ColNo) < 0 Then
                            colList.Add(ColNo)
                        End If
                        ColNo = ColNo + 1
                    Loop
                    Dim seEvtTitle As New List(Of String)
                    seEvtTitle.Add("Special Events Name")
                    seEvtTitle.Add("Special Events Date")
                    seEvtTitle.Add("Room Types")
                    seEvtTitle.Add("Meal Plans")
                    seEvtTitle.Add("Room Occupancy")
                    seEvtTitle.Add("Adult Rate")
                    seEvtTitle.Add("Age From")
                    seEvtTitle.Add("Age To")
                    seEvtTitle.Add("Rate")
                    For Each dr As DataRow In distinctSeDt.Rows
                        Dim filterDr As DataRow()
                        Dim filterDt As DataTable = Nothing
                        filterDt = dtSpecialEvt.Clone()
                        filterDr = dtSpecialEvt.Select("CompulsoryType='" + dr(0).ToString() + "'")
                        For Each row As DataRow In filterDr
                            filterDt.ImportRow(row)
                        Next
                        filterDt.Columns.Remove("spleventid")
                        filterDt.Columns.Remove("contpromid")
                        filterDt.Columns.Remove("CompulsoryType")
                        LastLine = LastLine + 2
                        ws.Range(LastLine - 1, 1, LastLine, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                        ws.Cell(LastLine, 1).Value = "Compulsory Options"
                        ws.Cell(LastLine, 2).Value = dr("CompulsoryType")
                        ws.Cell(LastLine, 1).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.SetBold(True).Fill.SetBackgroundColor(XLColor.BlueGray).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Range(LastLine, 2, LastLine, 3).Merge().Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.SetBold(True).Fill.SetBackgroundColor(XLColor.GreenYellow).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        LastLine = LastLine + 3
                        ws.Range(LastLine - 2, 1, LastLine - 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                        Dim j As Integer = 0
                        For Each i As Integer In colList
                            If j <= 5 Then
                                ws.Range(LastLine - 1, colList(j), LastLine, colList(j)).Merge().SetValue(seEvtTitle(j)).Style.Fill.SetBackgroundColor(XLColor.BlueGray)
                            ElseIf j = 6 Then
                                ws.Range(LastLine - 1, colList(j), LastLine - 1, colList(j + 2)).Merge().SetValue("Child Rate").Style.Fill.SetBackgroundColor(XLColor.GreenYellow)
                                ws.Cell(LastLine, colList(j)).SetValue(seEvtTitle(j)).Style.Fill.SetBackgroundColor(XLColor.BlueGray)
                            Else
                                ws.Cell(LastLine, colList(j)).SetValue(seEvtTitle(j)).Style.Fill.SetBackgroundColor(XLColor.BlueGray)
                            End If
                            j = j + 1
                        Next
                        ws.Range(LastLine - 1, 1, LastLine, colList(j - 1)).Style.Alignment.SetWrapText(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
                        ws.Row(LastLine).AdjustToContents()
                        ws.Range(LastLine - 1, 1, LastLine, colList(j - 1)).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Font.SetBold(True)
                        LastLine = LastLine + 1
                        cnt = LastLine

                        For Each ftDr As DataRow In filterDt.Rows
                            Dim colID As Integer = 0
                            For Each i As Integer In colList
                                If i = 2 Then
                                    ws.Cell(cnt, i).Value = ftDr(colID).ToString()
                                    ws.Cell(cnt, i).Style.NumberFormat.Format = "dd/MM/yyyy"
                                Else
                                    ws.Cell(cnt, i).Value = ftDr(colID).ToString()
                                End If
                                colID = colID + 1
                            Next
                            cnt = cnt + 1
                        Next
                        cnt = cnt - 1
                        Dim rngSE As IXLRange
                        rngSE = ws.Range(LastLine, 1, cnt, colList(8))
                        rngSE.Style.Alignment.SetWrapText(True)
                        Dim mFCnt As Integer = LastLine
                        Dim mLCnt As Integer = LastLine
                        For i = LastLine To cnt - 1
                            If ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value And ws.Cell(i, 2).Value = ws.Cell(i + 1, 2).Value And ws.Cell(i, 3).Value = ws.Cell(i + 1, 3).Value _
                                And ws.Cell(i, 4).Value = ws.Cell(i + 1, 4).Value And ws.Cell(i, 5).Value = ws.Cell(i + 1, 5).Value And ws.Cell(i, colList(5)).Value = ws.Cell(i + 1, colList(5)).Value Then
                                mLCnt = i + 1
                            Else
                                If mFCnt <> mLCnt Then
                                    For m As Integer = 0 To 5
                                        ws.Range(mFCnt, colList(m), mLCnt, colList(m)).Merge()
                                    Next
                                    Dim tmpH As Integer = Math.Ceiling(Math.Ceiling(ws.Cell(mFCnt, 3).Value.ToString.Length / ws.Column(3).Width) * 15 / (mLCnt - mFCnt))
                                    If tmpH < 18 Then tmpH = 18
                                    ws.Rows(mFCnt, mLCnt).Height = tmpH
                                Else
                                    ws.Row(i).Style.Alignment.SetWrapText(True)
                                End If
                                mFCnt = i + 1
                                mLCnt = i + 1
                            End If
                        Next
                        If mFCnt <> mLCnt Then
                            For m As Integer = 0 To 5
                                ws.Range(mFCnt, colList(m), mLCnt, colList(m)).Merge()
                            Next
                            Dim tmpH As Integer = Math.Ceiling(Math.Ceiling(ws.Cell(mFCnt, 3).Value.ToString.Length / ws.Column(3).Width) * 15 / (mLCnt - mFCnt))
                            If tmpH < 18 Then tmpH = 18
                            ws.Rows(mFCnt, mLCnt).Height = tmpH
                        Else
                            ws.Row(mFCnt).Style.Alignment.SetWrapText(True)
                        End If
                        rngSE.Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
                        rngSE.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        rngSE.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        rngSE.LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                        LastLine = cnt
                    Next
                    ws.Row(SpecialLink).Cell(1).Hyperlink.InternalAddress = "A" + (specialFirstRow).ToString + ":M" + LastLine.ToString
                    ws.Row(SpecialLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                    ws.Range(LastLine + 1, 1, LastLine + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                Else
                    For i = SpecialLink To GeneralLink - 1
                        ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value
                    Next
                    CancelLink = CancelLink - 1
                    CheckInOutLink = CheckInOutLink - 1
                    GeneralLink = GeneralLink - 1
                    ws.Range(GeneralLink, 1, GeneralLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    If OfferLastLine > LinkLastLine Then
                        ws.Cell(GeneralLink + 1, 1).Value = ""
                        ws.Range(GeneralLink + 1, 1, GeneralLink + 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                    Else
                        ws.Row(GeneralLink + 1).Delete()
                        LastLine = LastLine - 1
                        LinkLastLine = LinkLastLine - 1
                    End If
                End If

                mySqlCmd = New SqlCommand("sp_rep_cancelpolicy", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtPolicy As New DataTable
                myDataAdapter.Fill(dtPolicy)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                If dtPolicy.Rows.Count > 0 Then
                    Dim canPolicy As IXLRange
                    LastLine = LastLine + 3
                    cnt = LastLine
                    ws.Range(cnt, 1, cnt, 13).Merge()
                    ws.Cell(cnt, 1).Value = "CANCELLATION / NO SHOW / EARLY CHECKOUT - POLICY"
                    TitleStyle(ws.Cell(cnt, 1))
                    ws.Range(cnt, 1, cnt, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Season"
                    ws.Cell(cnt, 2).Value = "Period"
                    ws.Cell(cnt, 3).Value = "Applicable Room Types"
                    ws.Cell(cnt, 4).Value = "Meal Plan"
                    ws.Range(cnt, 4, cnt, 5).Merge()
                    ws.Cell(cnt, 6).Value = "Cancellation / No Show / Early Checkout - Policy"
                    ws.Range(cnt, 6, cnt, 13).Merge()
                    ws.Cell(cnt, 6).Style.Alignment.SetWrapText(True)
                    If ws.Cell(cnt, 6).Value.ToString.Trim.Length > mergeCalwidth Then
                        ws.Row(cnt).Height = Math.Ceiling(ws.Cell(cnt, 6).Value.ToString.Length / mergeCalwidth) * 15.2
                    End If
                    ws.Range(cnt, 1, cnt, 13).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(cnt, 1, cnt, 13).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Columns(2).Width = 12
                    Dim pstartRow = cnt + 1
                    For Each rs As DataRow In dtPolicy.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Cell(cnt, 2).Value = rs(1)
                        If ws.Cell(cnt, 2).Value.ToString.Substring(0, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(0, 1)
                        If ws.Cell(cnt, 2).Value.ToString.Substring(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1)
                        ws.Cell(cnt, 3).Value = rs(2)
                        ws.Cell(cnt, 4).Value = rs(3)
                        ws.Range(cnt, 4, cnt, 5).Merge()
                        ws.Cell(cnt, 6).Value = rs(4)
                        ws.Range(cnt, 6, cnt, 13).Merge()
                        'If ws.Cell(cnt, 6).Value.ToString.Substring(0, 1) = """" Then ws.Cell(cnt, 6).Value = ws.Cell(cnt, 6).Value.ToString.Remove(0, 1)
                        'If ws.Cell(cnt, 6).Value.ToString.Substring(ws.Cell(cnt, 6).Value.ToString.Length - 1, 1) = """" Then ws.Cell(cnt, 6).Value = ws.Cell(cnt, 6).Value.ToString.Remove(ws.Cell(cnt, 6).Value.ToString.Length - 1, 1)
                        FormatSpecificString(ws.Cell(cnt, 6))
                        Dim rowH2 = Math.Ceiling(rs(1).ToString.Length / ws.Column(2).Width) * 15.2
                        Dim rowH3 = Math.Ceiling(rs(2).ToString.Length / ws.Column(3).Width) * 15.2
                        Dim rowH6 = Math.Ceiling(rs(4).ToString.Length / mergeCalwidth) * 15.2
                        Dim rowUyaram As Integer
                        If rowH2 > rowH3 Then rowUyaram = rowH2 Else rowUyaram = rowH3
                        If rowUyaram < rowH6 Then
                            rowUyaram = rowH6
                            ws.Row(cnt).Height = rowUyaram + 15
                        End If
                    Next
                    canPolicy = ws.Range(pstartRow, 1, cnt, 13)
                    canPolicy.Style.Alignment.SetWrapText(True)
                    canPolicy.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    canPolicy.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    canPolicy.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    canPolicy.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Row(CancelLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine).ToString + ":M" + canPolicy.LastRow.WorksheetRow.RowNumber().ToString
                    ws.Row(CancelLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                    Dim patternRow As Integer = canPolicy.LastRow.RowNumber
                    'ws.Range(1, 14, patternRow, 14).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(patternRow + 1, 1, patternRow + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = patternRow
                Else
                    For i = CancelLink To GeneralLink - 1
                        ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value
                    Next
                    CheckInOutLink = CheckInOutLink - 1
                    GeneralLink = GeneralLink - 1
                    ws.Range(GeneralLink, 1, GeneralLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    If OfferLastLine > LinkLastLine Then
                        ws.Cell(GeneralLink + 1, 1).Value = ""
                        ws.Range(GeneralLink + 1, 1, GeneralLink + 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                    Else
                        ws.Row(GeneralLink + 1).Delete()
                        LastLine = LastLine - 1
                        LinkLastLine = LinkLastLine - 1
                    End If
                End If

                CheckInCheckOutPolicy(ws, contractids, LastLine, hideList, CheckInOutLink, GeneralLink, OfferLastLine, LinkLastLine)

                mySqlCmd = New SqlCommand("sp_rep_generalpolicy", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtGenPolicy As New DataTable
                myDataAdapter.Fill(dtGenPolicy)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                If dtGenPolicy.Rows.Count > 0 Then
                    LastLine = LastLine + 3
                    ws.Cell(LastLine, 1).Value = "GENERAL POLICY"
                    ws.Range(LastLine, 1, LastLine, 13).Merge()
                    TitleStyle(ws.Cell(LastLine, 1))
                    LastLine = LastLine + 1
                    ws.Cell(LastLine, 1).Value = "From Date"
                    ws.Cell(LastLine, 2).Value = "To Date"
                    ws.Cell(LastLine, 3).Value = "Policy"
                    ws.Range(LastLine, 3, LastLine, 13).Merge()
                    ws.Range(LastLine, 1, LastLine, 13).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    ws.Range(LastLine, 1, LastLine, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    LastLine = LastLine + 1
                    cnt = LastLine

                    Dim genCols As New List(Of String)(New String() {"fromdate", "todate", "policytext"})
                    Dim findCol As Integer = 0
                    While findCol < dtGenPolicy.Columns.Count
                        Dim strColName As String = dtGenPolicy.Columns(findCol).ColumnName.ToLower()
                        If Not genCols.Contains(strColName) Then
                            dtGenPolicy.Columns.Remove(strColName)
                        Else
                            findCol += 1
                        End If
                    End While

                    Dim rngGen As IXLRange
                    rngGen = ws.Cell(cnt, 1).InsertData(dtGenPolicy.AsEnumerable).AsRange()
                    rngGen.Style.Alignment.SetWrapText(True)
                    Dim cntRow As Integer = rngGen.LastRow.RowNumber
                    mergeCalwidth = mergeCalwidth + ws.Column(3).Width + ws.Column(4).Width + ws.Column(5).Width
                    Dim i As Integer = cnt
                    While i <= cntRow
                        If (Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) > 350 Then
                            Dim Nrows As Integer = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / 350)
                            ws.Row(i).InsertRowsBelow(Nrows)
                            ws.Range(i, 1, i + Nrows, 1).Merge()
                            ws.Range(i, 2, i + Nrows, 2).Merge()
                            ws.Range(i, 3, i + Nrows, 13).Merge()
                            ws.Rows(i, i + Nrows).Height = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / (Nrows + 1))
                            i = i + Nrows
                            cntRow = cntRow + Nrows
                        Else
                            ws.Range(i, 3, i, 13).Merge()
                            ws.Row(i).Height = Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150
                        End If
                        i = i + 1
                    End While
                    ws.Range(cnt, 1, cntRow, 13).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(cnt, 1, cntRow, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 1, cntRow, 13).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(cnt, 1, cntRow, 13).LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    ws.Row(GeneralLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine - 1).ToString + ":M" + cntRow.ToString
                    ws.Row(GeneralLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                    LastLine = cntRow
                    ws.Range(LastLine + 1, 1, LastLine + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                Else
                    ws.Range(GeneralLink - 1, 1, GeneralLink - 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    If OfferLastLine > LinkLastLine Then
                        ws.Cell(GeneralLink, 1).Value = ""
                        ws.Range(GeneralLink, 1, GeneralLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                    Else
                        ws.Row(GeneralLink).Delete()
                        LastLine = LastLine - 1
                        LinkLastLine = LinkLastLine - 1
                    End If
                End If

                mySqlCmd = New SqlCommand("sp_rep_hotelconstruction", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
                mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
                myDataAdapter = New SqlDataAdapter
                myDataAdapter.SelectCommand = mySqlCmd
                Dim dtConstruct As New DataTable
                myDataAdapter.Fill(dtConstruct)
                mySqlCmd.Dispose()
                myDataAdapter.Dispose()
                SqlConn.Close()
                If dtConstruct.Rows.Count > 0 Then
                    LastLine = LastLine + 3
                    ws.Cell(LastLine, 1).Value = "HOTEL CONSTRUCTION"
                    ws.Range(LastLine, 1, LastLine, 13).Merge()
                    TitleStyle(ws.Cell(LastLine, 1))
                    LastLine = LastLine + 1
                    ws.Cell(LastLine, 1).Value = "From Date"
                    ws.Cell(LastLine, 2).Value = "To Date"
                    ws.Cell(LastLine, 3).Value = "Construction"
                    ws.Range(LastLine, 3, LastLine, 13).Merge()
                    ws.Range(LastLine, 1, LastLine, 13).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    ws.Range(LastLine, 1, LastLine, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    LastLine = LastLine + 1
                    cnt = LastLine

                    Dim constCols As New List(Of String)(New String() {"fromdate", "todate", "constructiontext"})
                    Dim findCol As Integer = 0
                    While findCol < dtConstruct.Columns.Count
                        Dim strColName As String = dtConstruct.Columns(findCol).ColumnName.ToLower()
                        If Not constCols.Contains(strColName) Then
                            dtConstruct.Columns.Remove(strColName)
                        Else
                            findCol += 1
                        End If
                    End While

                    Dim rngCons As IXLRange
                    rngCons = ws.Cell(cnt, 1).InsertData(dtConstruct.AsEnumerable).AsRange()
                    rngCons.Style.Alignment.SetWrapText(True)
                    Dim cntRow As Integer = rngCons.LastRow.RowNumber
                    Dim i As Integer = cnt
                    While i <= cntRow
                        If (Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) > 350 Then
                            Dim Nrows As Integer = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / 350)
                            ws.Row(i).InsertRowsBelow(Nrows)
                            ws.Range(i, 1, i + Nrows, 1).Merge()
                            ws.Range(i, 2, i + Nrows, 2).Merge()
                            ws.Range(i, 3, i + Nrows, 13).Merge()
                            ws.Rows(i, i + Nrows).Height = Math.Ceiling((Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150) / (Nrows + 1))
                            i = i + Nrows
                            cntRow = cntRow + Nrows
                        Else
                            ws.Range(i, 3, i, 13).Merge()
                            ws.Row(i).Height = Math.Ceiling(ws.Cell(i, 3).Value.ToString.Length / mergeCalwidth) * 21 + 150
                        End If
                        i = i + 1
                    End While
                    ws.Range(cnt, 1, cntRow, 13).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(cnt, 1, cntRow, 13).Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 1, cntRow, 13).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(cnt, 1, cntRow, 13).LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    LastLine = cntRow
                    ws.Range(LastLine + 1, 1, LastLine + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
                End If

                'ws.Protect(RandomNo)
                'ws.Protection.FormatColumns = True
                'ws.Protection.FormatRows = True
            Else
                ws.Dispose()
                document.Worksheet("CONTRACTED RATES").Delete()
            End If
            Dim sheetPos As Integer = 2
            For Each pDr As DataRow In dtPromotion.Rows
                Dim sheetName As String = CType(sheetPos - 1, String) & " " & pDr("sheetName")
                sheetName = Regex.Replace(sheetName, "[-=\]\[;./~!@#$%^*()_+{}|:?\\\n]", "_")
                If (sheetName.Length > 31) Then sheetName = sheetName.Substring(0, 31)
                Dim sheet As IXLWorksheet = SheetTemplate.CopyTo(document, sheetName, sheetPos)
                FillOffer_columnformat(sheet, PartyName, CatName, SectorCityName, pDr)
                sheet.Protect(RandomNo)
                sheet.Protection.FormatColumns = True
                sheet.Protection.FormatRows = True
                sheetPos = sheetPos + 1
            Next
            SheetTemplate.Dispose()
            document.Worksheet("Offer Template").Delete()
            Using MyMemoryStream As New MemoryStream()
                document.SaveAs(MyMemoryStream)
                document.Dispose()
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                'Response.BinaryWrite(MyMemoryStream.GetBuffer())
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                Response.Flush()
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End Using
        Catch ex As Exception
            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptPriceList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region




#Region "Protected Sub FillOffer(ByVal ws As IXLWorksheet, ByVal PartyName As String, CatName As String, SectorCityName As String, promotionDr As DataRow)"
    Protected Sub FillOffer(ByVal ws As IXLWorksheet, ByVal PartyName As String, ByVal CatName As String, ByVal SectorCityName As String, ByVal promotionDr As DataRow)
        Try
            ws.Cell(1, 1).Value = PartyName
            ws.Cell(2, 3).Value = CatName
            ws.Cell(3, 3).Value = SectorCityName
            ws.Cell(4, 3).Value = promotionDr("PromotionID")
            ws.Cell(5, 3).Value = txtCtryName.Text.Trim.ToUpper
            Dim LastLine As Integer
            If txtAgentCode.Text.Trim <> "" Then
                ws.Cell(6, 3).Value = txtAgentName.Text.Trim.ToUpper
                LastLine = 7
            Else
                ws.Row(6).Delete()
                LastLine = 6
            End If
            LastLine = LastLine + 3

            'Combinable Offers
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand("sp_rep_offerrates", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@SourceCtryCode", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@RateType", SqlDbType.VarChar, 100)).Value = IIf(rdoNetPayable.Checked, "Net Payable", "Selling Rates")
            mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@Promotionid", SqlDbType.VarChar, 20)).Value = promotionDr("PromotionID")
            mySqlCmd.CommandTimeout = 0
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dtContract As New DataTable
            Dim dtOffer As New DataTable
            myDataAdapter.Fill(ds)
            dt = ds.Tables(0)
            dtContract = ds.Tables(1)
            dtOffer = ds.Tables(2)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()

            Dim offerRange As IXLRange
            Dim ContractLink As Integer = LastLine
            Dim CancelLink As Integer = LastLine + 1
            Dim TermLink As Integer = LastLine + 2
            Dim LinkLastLine As Integer = LastLine + 2
            Dim OfferLastLine As Integer
            Dim cnt As Integer = LastLine
            Dim offerFirstRow As Integer
            If dtOffer.Rows.Count > 0 Then
                If dtOffer.Rows.Count > 2 Then
                    LastLine = LastLine + 4
                    ws.Row(LastLine).InsertRowsAbove(dtOffer.Rows.Count - 2)
                    ws.Range(LastLine, 1, LastLine + dtOffer.Rows.Count - 2, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = LastLine + dtOffer.Rows.Count - 2
                Else
                    LastLine = LastLine + 4
                End If
                ws.Cell(cnt, 4).Value = "Promotion ID"
                ws.Cell(cnt, 5).Value = "Contract ID"
                ws.Cell(cnt, 6).Value = "Promotion Name"
                ws.Range(cnt, 6, cnt, 21).Merge()
                offerFirstRow = cnt + 1
                ws.Range(cnt, 4, cnt, 21).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(cnt, 4, cnt, 21).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                ws.Range(cnt, 4, cnt, 21).FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                For Each rs As DataRow In dtOffer.Rows
                    cnt = cnt + 1
                    ws.Cell(cnt, 4).Value = rs(0)
                    ws.Cell(cnt, 5).Value = rs(1)
                    ws.Cell(cnt, 6).Value = rs(2)
                    ws.Range(cnt, 6, cnt, 21).Merge()
                Next
                offerRange = ws.Range(offerFirstRow, 4, cnt, 21)
                offerRange.Style.Alignment.SetWrapText(True)
                offerRange.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                offerRange.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                offerRange.FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                offerRange.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                offerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                OfferLastLine = cnt
            Else
                ws.Row(LastLine - 1).Cell(4).Clear()
                ws.Row(LastLine - 1).Cell(4).Style.Fill.SetBackgroundColor(XLColor.White)
                OfferLastLine = LastLine
                LastLine = LastLine + 4
            End If

            'Offer Rates
            LastLine = LastLine + 1
            Dim linkContractFirstRow = LastLine + 2
            Dim HeaderRng As IXLRange
            HeaderRng = ws.Range(LastLine, 1, LastLine + 4, 23).AsRange()
            Dim LoopCnt As Integer = 1
            Dim rangeStr As New List(Of Integer)
            Dim rangeStrAvail As New List(Of Integer)
            Dim mergeCalwidth As Integer
            Dim distinctDt As DataTable = dt.DefaultView.ToTable(True, "ratesbasedon")
            If distinctDt.Rows.Count <= 0 Then
                LastLine = LastLine + 6
                ws.Range(LastLine - 1, 1, LastLine - 1, 23).Style.Fill.SetBackgroundColor(XLColor.White)
            End If
            For Each dr As DataRow In distinctDt.Rows
                Dim filterDr As DataRow()
                Dim filterDt As DataTable = Nothing
                filterDt = dt.Clone()
                filterDr = dt.Select("ratesbasedon='" + dr(0).ToString() + "'")
                For Each row As DataRow In filterDr
                    filterDt.ImportRow(row)
                Next
                filterDt.Columns.Remove("ratesbasedon")
                ws.Cell(LastLine, 2).Value = dr(0).ToString()
                '--------------- Tax inclusive Note  ----------------------
                LastLine = LastLine + 1
                ws.Row(LastLine).InsertRowsBelow(2)
                TaxNoteTable(ws, LastLine, 23)
                LastLine = LastLine + 1
                '---------------------- End --------------------
                'LastLine = LastLine + 2
                ws.Cell(LastLine, 1).Value = promotionDr("PromotionName")

                LastLine = LastLine + 3
                cnt = LastLine

                If rdoNetPayable.Checked Then
                    ws.Cell(cnt - 2, 16).Value = "Net Payable"
                    If ws.Column(16).Width < ws.Cell(cnt - 2, 16).Value.ToString.Length Then
                        ws.Column(16).Width = ws.Cell(cnt - 2, 16).Value.ToString.Length + 2
                    End If
                End If

                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(cnt, 1).InsertTable(filterDt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                ws.Rows(cnt).Clear()
                ws.Rows(cnt).Delete()
                RateSheet.Style.Font.FontName = "Times New Roman"
                RateSheet.Columns(16, 22).SetDataType(XLCellValues.Number)
                RateSheet.Columns(1, 8).SetDataType(XLCellValues.Text)
                RateSheet.Columns(1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                RateSheet.Columns("9").SetDataType(XLCellValues.Number)
                RateSheet.Columns(10, 15).SetDataType(XLCellValues.Text)
                RateSheet.Columns(10, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin
                RateSheet.Columns("1:3,5,9,15,21").Style.Border.RightBorder = XLBorderStyleValues.Medium
                For i = 1 To dt.Rows.Count
                    If RateSheet.Cell(i, 1).Value <> RateSheet.Cell(i + 1, 1).Value Or RateSheet.Cell(i, 2).Value <> RateSheet.Cell(i + 1, 2).Value Or RateSheet.Cell(i, 3).Value <> RateSheet.Cell(i + 1, 3).Value Then
                        RateSheet.Rows(i).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                    End If
                Next

                For Each rng As IXLCell In RateSheet.Columns(16, 21).Cells
                    If rng.Value = "0" Then
                        rng.Value = ""
                    End If
                Next
                ws.Columns(16, 21).Width = 12
                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium
                ws.Columns("1,3:15,23").AdjustToContents()

                LastLine = RateSheet.LastRow.WorksheetRow.RowNumber()
                ws.Range(LastLine + 1, 1, LastLine + 2, 23).Style.Fill.SetBackgroundColor(XLColor.White)

                Dim col As Integer = 9
                For Each kol As IXLRangeColumn In RateSheet.Columns(10, 21)
                    col = col + 1
                    If kol.IsEmpty() Then
                        If rangeStrAvail.IndexOf(col) < 0 Then 'changed by mohamed / parameswaran on 08/08/2018
                            If rangeStr.IndexOf(col) < 0 Then
                                rangeStr.Add(col)
                            End If
                        End If
                    Else
                        If rangeStr.IndexOf(col) >= 0 Then
                            rangeStr.Remove(col)
                        End If
                        If rangeStrAvail.IndexOf(col) < 0 Then 'changed by mohamed / parameswaran on 08/08/2018
                            rangeStrAvail.Add(col)
                        End If
                    End If
                Next

                LoopCnt = LoopCnt + 1
                LastLine = LastLine + 3
                If LoopCnt <= distinctDt.Rows.Count Then
                    ws.Cell(LastLine, 1).Value = HeaderRng
                Else
                    ws.Row(ContractLink).Cell(1).Hyperlink.InternalAddress = "A" + linkContractFirstRow.ToString + ":W" + RateSheet.LastRow.WorksheetRow.RowNumber().ToString
                    ws.Row(ContractLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)

                    Dim showcol As Integer = 12
                    For i = 10 To 21
                        If rangeStr.IndexOf(i) < 0 Then
                            mergeCalwidth = mergeCalwidth + ws.Column(i).Width
                            'Else 'else part is moved to out of ratebasedon for loop 'changed by mohamed / parameswaran on 08/08/2018
                            '    'ws.Column(i).Hide()
                        End If
                    Next

                    If dtOffer.Rows.Count > 0 Then
                        cnt = offerFirstRow
                        For Each rs As DataRow In dtOffer.Rows
                            Dim rowUyaram As Integer = Math.Ceiling(rs(2).ToString.Length / mergeCalwidth) * 15.2
                            ws.Row(cnt).Height = rowUyaram + 15
                            cnt = cnt + 1
                        Next
                    End If
                End If
            Next

            'changed by mohamed / parameswaran on 08/08/2018
            For i = 10 To 21
                If rangeStr.IndexOf(i) >= 0 Then
                    ws.Column(i).Hide()
                End If
            Next

            'Offer Cancellation Policy
            mySqlCmd = New SqlCommand("sp_rep_cancelpolicy", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = promotionDr("PromotionID")
            mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim dtPolicy As New DataTable
            myDataAdapter.Fill(dtPolicy)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()
            If dtPolicy.Rows.Count > 0 Then
                Dim canPolicy As IXLRange
                cnt = LastLine
                ws.Range(cnt, 1, cnt, 23).Merge()
                ws.Cell(cnt, 1).Value = "CANCELLATION / NO SHOW / EARLY CHECKOUT - POLICY"
                TitleStyle(ws.Cell(cnt, 1))
                ws.Range(cnt, 1, cnt, 23).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium)
                cnt = cnt + 1
                ws.Cell(cnt, 1).Value = "Season"
                ws.Cell(cnt, 2).Value = "Period"
                ws.Range(cnt, 2, cnt, 3).Merge()
                ws.Cell(cnt, 4).Value = "Applicable Room Types"
                ws.Range(cnt, 4, cnt, 6).Merge()
                ws.Cell(cnt, 7).Value = "Meal Plan"
                ws.Range(cnt, 7, cnt, 9).Merge()
                ws.Cell(cnt, 10).Value = "Cancellation / No Show / Early Checkout - Policy"
                ws.Range(cnt, 10, cnt, 23).Merge()
                ws.Cell(cnt, 10).Style.Alignment.SetWrapText(True)
                mergeCalwidth = mergeCalwidth + ws.Column(22).Width + ws.Column(23).Width
                If ws.Cell(cnt, 10).Value.ToString.Trim.Length > mergeCalwidth Then
                    ws.Row(cnt).Height = Math.Ceiling(ws.Cell(cnt, 10).Value.ToString.Length / mergeCalwidth) * 15.2
                End If
                ws.Range(cnt, 1, cnt, 23).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                ws.Range(cnt, 1, cnt, 23).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                Dim pstartRow = cnt + 1
                For Each rs As DataRow In dtPolicy.Rows
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = rs(0)
                    ws.Cell(cnt, 2).Value = rs(1)
                    ws.Range(cnt, 2, cnt, 3).Merge()
                    If ws.Cell(cnt, 2).Value.ToString.Substring(0, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(0, 1)
                    If ws.Cell(cnt, 2).Value.ToString.Substring(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1)
                    ws.Cell(cnt, 4).Value = rs(2)
                    ws.Range(cnt, 4, cnt, 6).Merge()
                    ws.Cell(cnt, 7).Value = rs(3)
                    ws.Range(cnt, 7, cnt, 9).Merge()
                    ws.Cell(cnt, 10).Value = rs(4)
                    ws.Range(cnt, 10, cnt, 23).Merge()
                    FormatSpecificString(ws.Cell(cnt, 10))
                    Dim rowH2 = Math.Ceiling(rs(1).ToString.Length / (ws.Column(2).Width + ws.Column(3).Width)) * 15.2
                    Dim rowH3 = Math.Ceiling(rs(2).ToString.Length / (ws.Column(4).Width + ws.Column(5).Width + ws.Column(6).Width)) * 15.2
                    Dim rowH6 = Math.Ceiling(rs(4).ToString.Length / mergeCalwidth) * 15.2
                    Dim rowUyaram As Integer
                    If rowH2 > rowH3 Then rowUyaram = rowH2 Else rowUyaram = rowH3
                    If rowUyaram < rowH6 Then rowUyaram = rowH6
                    ws.Row(cnt).Height = rowUyaram + 15
                Next
                canPolicy = ws.Range(pstartRow, 1, cnt, 23)
                canPolicy.Style.Alignment.SetWrapText(True)
                canPolicy.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                canPolicy.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                canPolicy.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                canPolicy.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                ws.Row(CancelLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine).ToString + ":W" + canPolicy.LastRow.WorksheetRow.RowNumber().ToString
                ws.Row(CancelLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                Dim patternRow As Integer = canPolicy.LastRow.RowNumber
                ws.Range(patternRow + 1, 1, patternRow + 2, 24).Style.Fill.SetBackgroundColor(XLColor.White)
                LastLine = patternRow + 3
            Else
                For i = CancelLink To TermLink - 1
                    ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value
                Next
                TermLink = TermLink - 1
                ws.Range(TermLink, 1, TermLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                If OfferLastLine > LinkLastLine Then
                    ws.Cell(TermLink + 1, 1).Value = ""
                    ws.Range(TermLink + 1, 1, TermLink + 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                Else
                    ws.Row(TermLink + 1).Delete()
                    LastLine = LastLine - 1
                    LinkLastLine = LinkLastLine - 1
                End If
            End If
            mySqlCmd = New SqlCommand("sp_rep_offer_terms", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = promotionDr("PromotionID")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim DsTerms As New DataSet
            Dim dtRemark As New DataTable
            Dim dtRoomUpgrade As New DataTable
            Dim dtMealUpgrade As New DataTable
            Dim dtFreeNights As New DataTable
            Dim dtSpecial As New DataTable
            Dim dtInterHotel As New DataTable
            Dim dtFlight As New DataTable
            Dim dtCompliment As New DataTable
            myDataAdapter.Fill(DsTerms)
            dtRemark = DsTerms.Tables(0)
            dtRoomUpgrade = DsTerms.Tables(1)
            dtMealUpgrade = DsTerms.Tables(2)
            dtFreeNights = DsTerms.Tables(3)
            dtSpecial = DsTerms.Tables(4)
            dtInterHotel = DsTerms.Tables(5)
            dtFlight = DsTerms.Tables(6)
            dtCompliment = DsTerms.Tables(7)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()
            SqlConn.Close()

            Dim EnableTerms As Boolean = False
            For Each tDt As DataTable In DsTerms.Tables
                If tDt.Rows.Count > 0 Then
                    EnableTerms = True
                    Exit For
                End If
            Next
            If EnableTerms = True Then
                'Terms and Conditions - Remarks
                Dim termMergeWidth As Integer = 0
                Dim TermFirstRow As Integer = LastLine
                cnt = LastLine
                ws.Range(cnt, 1, cnt, 9).Merge()
                ws.Cell(cnt, 1).Value = "TERMS AND CONDITIONS"
                TitleStyle(ws.Cell(cnt, 1))
                ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium)
                cnt = cnt + 1
                LastLine = cnt
                For i = 1 To 9
                    termMergeWidth = termMergeWidth + ws.Column(i).Width
                Next
                If dtRemark.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Remarks"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    Dim rngRemarks As IXLRange
                    Dim Remark1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtRemark.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs("Remarks")
                        ws.Range(cnt, 1, cnt, 9).Merge()
                        ws.Row(cnt).Height = (Math.Ceiling(rs(0).ToString.Length / termMergeWidth * 21)) + 15
                    Next
                    rngRemarks = ws.Range(Remark1Row, 1, cnt, 9)
                    rngRemarks.Style.Alignment.SetWrapText(True)
                    rngRemarks.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngRemarks.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    rngRemarks.LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    rngRemarks.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Room Upgrade
                cnt = LastLine
                If dtRoomUpgrade.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Room Upgrade"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Upgrade From"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Upgrade To"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngRoomUpgrade As IXLRange
                    Dim RoomUpgrd1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtRoomUpgrade.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs("UpgradeFrom")
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs("UpgradeTo")
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngRoomUpgrade = ws.Range(RoomUpgrd1Row, 1, cnt, 4)
                    rngRoomUpgrade.Style.Alignment.SetWrapText(True)
                    rngRoomUpgrade.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngRoomUpgrade.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(RoomUpgrd1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(RoomUpgrd1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Meal Upgrade
                cnt = LastLine
                If dtMealUpgrade.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Meal Upgrade"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Upgrade From"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Upgrade To"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngMealUpgrade As IXLRange
                    Dim MealUpgrd1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtMealUpgrade.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs("UpgradeFrom")
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs("UpgradeTo")
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngMealUpgrade = ws.Range(MealUpgrd1Row, 1, cnt, 4)
                    rngMealUpgrade.Style.Alignment.SetWrapText(True)
                    rngMealUpgrade.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngMealUpgrade.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(MealUpgrd1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(MealUpgrd1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Free Nights
                cnt = LastLine
                If dtFreeNights.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Free Nights"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "From Date"
                    ws.Cell(cnt, 2).Value = "To Date"
                    ws.Cell(cnt, 3).Value = "Booking Code"
                    ws.Cell(cnt, 4).Value = "Apply Free Nights To"
                    ws.Cell(cnt, 5).Value = "Stay For"
                    ws.Cell(cnt, 6).Value = "Pay For"
                    ws.Cell(cnt, 7).Value = "Allow Multiples"
                    ws.Cell(cnt, 8).Value = "Max Multiples Allowed"
                    ws.Cell(cnt, 9).Value = "Max Free Nights"
                    If ws.Column(7).Width < 15 Then ws.Column(7).Width = 15
                    If ws.Column(8).Width < 15 Then ws.Column(8).Width = 15
                    ws.Range(cnt, 1, cnt, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Row(cnt).Cells(3, 9).Style.Alignment.SetWrapText(True)
                    Dim rngFreeNights As IXLRange
                    cnt = cnt + 1
                    rngFreeNights = ws.Cell(cnt, 1).InsertTable(dtFreeNights).SetShowHeaderRow(False).AsRange()
                    ws.Rows(cnt).Clear()
                    ws.Rows(cnt).Delete()
                    rngFreeNights.Columns("1:2").SetDataType(XLCellValues.Text)
                    rngFreeNights.Columns("5:6,8:9").SetDataType(XLCellValues.Number)
                    rngFreeNights.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngFreeNights.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    rngFreeNights.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    cnt = rngFreeNights.LastRow.RowNumber
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Special Occasion
                cnt = LastLine
                If dtSpecial.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Special Occasion"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    Dim rngSpecial As IXLRange
                    Dim Special1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtSpecial.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 9).Merge()
                        ws.Row(cnt).Height = (Math.Ceiling(rs(0).ToString.Length / termMergeWidth * 15.2)) + 15
                    Next
                    rngSpecial = ws.Range(Special1Row, 1, cnt, 9)
                    rngSpecial.Style.Alignment.SetWrapText(True)
                    rngSpecial.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngSpecial.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    rngSpecial.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Inter Hotels
                cnt = LastLine
                If dtInterHotel.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Inter Hotels"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Inter Hotel Stay Required"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Minimum Nights"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngInterHotel As IXLRange
                    Dim InterHotel1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtInterHotel.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs(1)
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngInterHotel = ws.Range(InterHotel1Row, 1, cnt, 4)
                    rngInterHotel.Style.Alignment.SetWrapText(True)
                    rngInterHotel.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngInterHotel.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(InterHotel1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(InterHotel1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Selected Flights Only
                cnt = LastLine
                If dtFlight.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Selected Flights Only (This offer is applicable only to arrivals in the below flights)"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    Dim rngFlight As IXLRange
                    Dim Flight1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtFlight.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 9).Merge()
                        ws.Row(cnt).Height = (Math.Ceiling(rs(0).ToString.Length / termMergeWidth * 15.2)) + 15
                    Next
                    rngFlight = ws.Range(Flight1Row, 1, cnt, 9)
                    rngFlight.Style.Alignment.SetWrapText(True)
                    rngFlight.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngFlight.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    rngFlight.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Complimentary Transfers
                cnt = LastLine
                If dtCompliment.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Complimentary Transfers"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Transfer Type"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Applicable Airport"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Times New Roman").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngCompliment As IXLRange
                    Dim Compliment1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtCompliment.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs(1)
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngCompliment = ws.Range(Compliment1Row, 1, cnt, 4)
                    rngCompliment.Style.Alignment.SetWrapText(True)
                    rngCompliment.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngCompliment.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(Compliment1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(Compliment1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, 23).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If
                ws.Range(LastLine - 1, 1, LastLine - 1, 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                ws.Range(1, 24, LastLine + 1, 24).Style.Fill.SetBackgroundColor(XLColor.White)
                ws.Range(LastLine, 1, LastLine + 1, 24).Style.Fill.SetBackgroundColor(XLColor.White)
                ws.Row(TermLink).Cell(1).Hyperlink.InternalAddress = "A" + (TermFirstRow).ToString + ":I" + LastLine.ToString()
                ws.Row(TermLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
            Else
                ws.Range(TermLink - 1, 1, TermLink - 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                If OfferLastLine > LinkLastLine Then
                    ws.Cell(TermLink, 1).Value = ""
                    ws.Range(TermLink, 1, TermLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                Else
                    ws.Row(TermLink).Delete()
                    LastLine = LastLine - 1
                    LinkLastLine = LinkLastLine - 1
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region






#Region "Protected Sub FillOffer_columnformat(ByVal ws As IXLWorksheet, ByVal PartyName As String, CatName As String, SectorCityName As String, promotionDr As DataRow)"
    Protected Sub FillOffer_columnformat(ByVal ws As IXLWorksheet, ByVal PartyName As String, ByVal CatName As String, ByVal SectorCityName As String, ByVal promotionDr As DataRow)
        Try
            ws.Cell(1, 1).Value = PartyName
            ws.Cell(2, 3).Value = CatName
            ws.Cell(3, 3).Value = SectorCityName
            ws.Cell(4, 3).Value = promotionDr("PromotionID")
            ws.Cell(5, 3).Value = txtCtryName.Text.Trim.ToUpper
            Dim LastLine As Integer
            If txtAgentCode.Text.Trim <> "" Then
                ws.Cell(6, 3).Value = txtAgentName.Text.Trim.ToUpper
                LastLine = 7
            Else
                ws.Row(6).Delete()
                LastLine = 6
            End If
            LastLine = LastLine + 3

            'Combinable Offers
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand("sp_rep_offerrates_groupdates", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@SourceCtryCode", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@RateType", SqlDbType.VarChar, 100)).Value = IIf(rdoNetPayable.Checked, "Net Payable", "Selling Rates")
            mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@Promotionid", SqlDbType.VarChar, 20)).Value = promotionDr("PromotionID")
            mySqlCmd.CommandTimeout = 0
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dtContract As New DataTable
            Dim dtOffer As New DataTable
            Dim dtcolumn As New DataTable
            myDataAdapter.Fill(ds)
            dt = ds.Tables(2)
            dtContract = ds.Tables(0)
            dtOffer = ds.Tables(1)
            dtcolumn = ds.Tables(3)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()

            Dim offerRange As IXLRange
            Dim ContractLink As Integer = LastLine
            Dim CancelLink As Integer = LastLine + 1
            Dim TermLink As Integer = LastLine + 2
            Dim LinkLastLine As Integer = LastLine + 2
            Dim OfferLastLine As Integer
            Dim cnt As Integer = LastLine
            Dim offerFirstRow As Integer
            If dtOffer.Rows.Count > 0 Then
                If dtOffer.Rows.Count > 2 Then
                    LastLine = LastLine + 4
                    ws.Row(LastLine).InsertRowsAbove(dtOffer.Rows.Count - 2)
                    ws.Range(LastLine, 1, LastLine + dtOffer.Rows.Count - 2, dt.Columns.Count - 1).Style.Fill.SetBackgroundColor(XLColor.White)

                    LastLine = LastLine + dtOffer.Rows.Count - 2
                Else
                    LastLine = LastLine + 4
                End If

                Dim promocol As String
                If dt.Columns.Count < 8 Then
                    promocol = dt.Columns.Count
                    ws.Range(cnt - 1, 4, cnt - 1, dt.Columns.Count).Merge()
                Else
                    promocol = 8
                    ws.Range(cnt - 1, 4, cnt - 1, 8).Merge()
                End If




                offerRange = ws.Range(cnt - 1, 4, cnt - 1, promocol)
                offerRange.FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                offerRange.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)


                ws.Cell(cnt, 4).Value = "Promotion ID"
                ws.Cell(cnt, 5).Value = "Contract ID"
                ws.Cell(cnt, 6).Value = "Promotion Name"
                ws.Range(cnt, 6, cnt, promocol).Merge()
                offerFirstRow = cnt + 1
                ws.Range(cnt, 4, cnt, promocol).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(cnt, 4, cnt, promocol).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                ws.Range(cnt, 4, cnt, promocol).FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                For Each rs As DataRow In dtOffer.Rows
                    cnt = cnt + 1
                    ws.Cell(cnt, 4).Value = rs(0)
                    ws.Cell(cnt, 5).Value = rs(1)
                    ws.Cell(cnt, 6).Value = rs(2)
                    ws.Range(cnt, 6, cnt, promocol).Merge()
                Next
                offerRange = ws.Range(offerFirstRow, 4, cnt, promocol)
                offerRange.Style.Alignment.SetWrapText(True)
                offerRange.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                offerRange.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                offerRange.FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
                offerRange.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                offerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                OfferLastLine = cnt
            Else
                ws.Row(LastLine - 1).Cell(4).Clear()
                ws.Row(LastLine - 1).Cell(4).Style.Fill.SetBackgroundColor(XLColor.White)
                OfferLastLine = LastLine
                LastLine = LastLine + 4
            End If

            'Offer Rates
            LastLine = LastLine + 1
            Dim linkContractFirstRow = LastLine + 2
            Dim HeaderRng As IXLRange
            HeaderRng = ws.Range(LastLine, 1, LastLine + 4, 23).AsRange()
            Dim LoopCnt As Integer = 1
            Dim rangeStr As New List(Of Integer)
            Dim mergeCalwidth As Integer
            Dim distinctDt As DataTable = dt.DefaultView.ToTable(True, "ratesbasedon")
            If distinctDt.Rows.Count <= 0 Then
                LastLine = LastLine + 6
                ws.Range(LastLine - 1, 1, LastLine - 1, 23).Style.Fill.SetBackgroundColor(XLColor.White)
            End If
            For Each dr As DataRow In distinctDt.Rows
                Dim filterDr As DataRow()
                Dim filterDt As DataTable = Nothing
                filterDt = dt.Clone()
                filterDr = dt.Select("ratesbasedon='" + dr(0).ToString() + "'")
                For Each row As DataRow In filterDr
                    filterDt.ImportRow(row)
                Next
                filterDt.Columns.Remove("ratesbasedon")
                ws.Cell(LastLine, 2).Value = dr(0).ToString()
                '--------------- Tax inclusive Note  ----------------------
                LastLine = LastLine + 1
                ws.Row(LastLine).InsertRowsBelow(2)
                TaxNoteTable(ws, LastLine, filterDt.Columns.Count)
                LastLine = LastLine + 1
                '---------------------- End --------------------
                'LastLine = LastLine + 2
                ws.Cell(LastLine, 1).Value = promotionDr("PromotionName")

                LastLine = LastLine + 1
                cnt = LastLine

                Dim colname As Integer
                colname = 4
                ws.Cell(cnt + 5, 1).Value = "Room Type"


                ws.Cell(cnt + 5, 2).Value = "Room Category"


                ws.Cell(cnt + 5, 3).Value = "Age Combination"
                'ws.Row(cnt + 1).Height = 65


                'ws.Range(cnt + 4, colname, cnt + 5, colname + 3).Merge()

                For Each rs As DataRow In dtcolumn.Rows
                    ' 

                    ws.Cell(cnt, colname).Value = rs(1).ToString.Replace(";", "").Trim
                    ws.Cell(cnt, colname).Value = ws.Cell(cnt, colname).Value + vbNewLine + rs(13).ToString.Replace(";", vbNewLine)
                    ws.Cell(cnt, colname).Value = Regex.Replace(ws.Cell(cnt, colname).Value, "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]", "")

                    ws.Cell(cnt, colname).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)


                    ws.Column(colname).Width = 25.3


                    ws.Cell(cnt, colname).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                    ws.Cell(cnt, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

                    ws.Cell(cnt, colname).Style.Font.Bold = True
                    ws.Row(cnt).AdjustToContents()
                    ws.Row(cnt).Height = 85
                    ws.Cell(cnt, colname).Style.Alignment.WrapText = True


                    ' ws.Rows(cnt).Style.Border.BottomBorder = XLBorderStyleValues.Medium


                    ws.Cell(cnt + 1, colname).Value = rs(6).ToString
                    ws.Cell(cnt + 1, colname).Style.Alignment.WrapText = True

                    ws.Cell(cnt + 1, colname).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                    ws.Cell(cnt + 1, colname).Style.Font.Bold = True
                    ws.Cell(cnt + 1, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    ws.Row(cnt + 1).AdjustToContents()
                    ws.Row(cnt + 1).Height = 40

                    ws.Cell(cnt + 2, colname).Value = rs(5).ToString
                    ws.Cell(cnt + 2, colname).Style.Alignment.WrapText = True
                    ws.Cell(cnt + 2, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    ws.Cell(cnt + 2, colname).Style.Font.Bold = True

                    ws.Cell(cnt + 2, colname).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                    ' ws.Cell(cnt + 2, colname).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)

                    ws.Row(cnt + 2).AdjustToContents()

                    ' Dim rowUyaram As Integer = Math.Ceiling(rs(5).ToString.Length / 25.3) * 18
                    ws.Row(cnt + 2).Height = 50
                    'ws.Row(cnt + 2).Height = 30


                    ws.Cell(cnt + 3, colname).Value = "Minstay & MinstayType:" & rs(2).ToString
                    If rs(3).ToString.Length > 0 Then
                        ws.Cell(cnt + 3, colname).Value = ws.Cell(cnt + 3, colname).Value & " &" & rs(3).ToString

                        'ws.Row(cnt + 3).Height = 30
                    End If
                    ws.Cell(cnt + 3, colname).Style.Font.SetBold(True)
                    ws.Cell(cnt + 3, colname).Style.Alignment.WrapText = True

                    ws.Cell(cnt + 3, colname).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                    ws.Cell(cnt + 3, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    'ws.Row(cnt + 2).Merge = xl
                    ws.Row(cnt + 3).AdjustToContents()
                    'rowUyaram = Math.Ceiling(ws.Cell(cnt + 3, colname).Value.ToString.Length / 25.3) * 15.2
                    ' ws.Row(cnt + 2).Height = rowUyaram + 15
                    'ws.Cell(cnt, 6).Value = rs(2)
                    ws.Row(cnt + 3).Height = 45
                    ws.Range(cnt + 4, colname, cnt + 5, colname).Merge()

                    ws.Range(cnt + 4, colname, cnt + 5, colname).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                    ws.Cell(cnt + 4, colname).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                    ws.Cell(cnt + 4, colname).Style.Font.SetBold(True)
                    ws.Cell(cnt + 4, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    ws.Cell(cnt + 4, colname).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                    If rdoNetPayable.Checked Then
                        ws.Cell(cnt + 4, colname).Value = "Net Payable"
                    Else

                        ws.Cell(cnt + 4, colname).Value = "Sale Price"
                    End If
                    ws.Cell(cnt + 6, colname).Value = rs(4).ToString
                    ws.Cell(cnt + 6, colname).Style.Font.SetBold(True)
                    ws.Cell(cnt + 6, colname).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    ws.Cell(cnt + 6, colname).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                    colname = colname + 1
                Next




                'If rdoNetPayable.Checked Then
                '    ws.Cell(cnt - 2, 16).Value = "Net Payable"
                '    If ws.Column(16).Width < ws.Cell(cnt - 2, 16).Value.ToString.Length Then
                '        ws.Column(16).Width = ws.Cell(cnt - 2, 16).Value.ToString.Length + 2
                '    End If
                'End I

                'ws.Range(cnt - 1, 1, cnt - 1, filterDt.Columns.Count).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                ws.Range(cnt, 1, cnt + 6, filterDt.Columns.Count).Style.Font.SetFontColor(XLColor.White)



                ws.Range(cnt - 1, 1, cnt + 2, filterDt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.MediumElectricBlue)
                ws.Range(cnt, 1, cnt + 6, filterDt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.MediumElectricBlue)
                ws.Range(cnt, 1, cnt + 6, filterDt.Columns.Count).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                ws.Range(cnt + 5, 1, cnt + 7, filterDt.Columns.Count).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                ws.Range(cnt + 4, 2, cnt + 6, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                ws.Range(cnt + 4, 3, cnt + 6, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                Dim RateSheet As IXLRange
                RateSheet = ws.Cell(cnt + 8, 1).InsertTable(filterDt.AsEnumerable).SetShowHeaderRow(False).AsRange()
                'ws.Rows(cnt).Clear()
                'ws.Rows(cnt).Delete()


                ws.Rows(cnt + 8).Clear()
                ws.Rows(cnt + 8).Delete()


                RateSheet.Style.Font.FontName = "Trebuchet MS"
                'RateSheet.Columns(16, 22).SetDataType(XLCellValues.Number)
                'RateSheet.Columns(1, 8).SetDataType(XLCellValues.Text)
                RateSheet.Columns(4, filterDt.Columns.Count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                'RateSheet.Columns("9").SetDataType(XLCellValues.Number)
                'RateSheet.Columns(10, 15).SetDataType(XLCellValues.Text)
                'RateSheet.Columns(10, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                Dim style1 As IXLBorder = RateSheet.Cells.Style.Border
                style1.BottomBorder = XLBorderStyleValues.Thin
                style1.LeftBorder = XLBorderStyleValues.Thin
                'RateSheet.Columns("1:3,5,9,15,21").Style.Border.RightBorder = XLBorderStyleValues.Medium
                'For i = 1 To dt.Rows.Count
                '    If RateSheet.Cell(i, 1).Value <> RateSheet.Cell(i + 1, 1).Value Or RateSheet.Cell(i, 2).Value <> RateSheet.Cell(i + 1, 2).Value Or RateSheet.Cell(i, 3).Value <> RateSheet.Cell(i + 1, 3).Value Then
                '        RateSheet.Rows(i).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                '    End If
                'Next



                For i = 1 To dt.Rows.Count
                    If RateSheet.Cell(i, 1).Value = RateSheet.Cell(i + 1, 1).Value And RateSheet.Cell(i, 2).Value <> RateSheet.Cell(i + 1, 2).Value Then
                        RateSheet.Rows(i).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                    End If
                    If RateSheet.Cell(i, 1).Value <> RateSheet.Cell(i + 1, 1).Value Then
                        RateSheet.Rows(i).Style.Border.BottomBorder = XLBorderStyleValues.Thick
                    End If
                Next



                'For Each rng As IXLCell In RateSheet.Columns(3, totalcolcount).Cells
                '    If rng.Value = 0 Then
                '        rng.Value = ""
                '    End If
                'Next

                'Dim showcol As Integer = 6
                'Dim col As Integer = 3
                'For Each kol As IXLRangeColumn In RateSheet.Columns(3, dt.Columns.Count)
                '    col = col + 1
                '    If kol.IsEmpty() Then
                '        ws.Column(col).Hide()

                '        showcol = showcol - 1
                '    End If
                'Next

                'ws.Columns(16, 21).Width = 12
                RateSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Medium
                'ws.Columns("1,3:15,23").Style.Alignment


                LastLine = RateSheet.LastRow.WorksheetRow.RowNumber()
                ws.Range(LastLine + 1, 1, LastLine + 2, 23).Style.Fill.SetBackgroundColor(XLColor.White)

                Dim col As Integer = 9
                For Each kol As IXLRangeColumn In RateSheet.Columns(10, 21)
                    col = col + 1
                    If kol.IsEmpty() Then
                        If rangeStr.IndexOf(col) < 0 Then
                            rangeStr.Add(col)
                        End If
                    Else
                        If rangeStr.IndexOf(col) >= 0 Then
                            rangeStr.Remove(col)
                        End If
                    End If
                Next

                LoopCnt = LoopCnt + 1
                LastLine = LastLine + 3
                If LoopCnt <= distinctDt.Rows.Count Then
                    ws.Cell(LastLine, 1).Value = HeaderRng
                Else
                    ws.Row(ContractLink).Cell(1).Hyperlink.InternalAddress = "A" + linkContractFirstRow.ToString + ":W" + RateSheet.LastRow.WorksheetRow.RowNumber().ToString
                    ws.Row(ContractLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)

                    'Dim showcol As Integer = 12
                    'For i = 10 To 21
                    '    If rangeStr.IndexOf(i) < 0 Then
                    '        mergeCalwidth = mergeCalwidth + ws.Column(i).Width
                    '    Else
                    '        ws.Column(i).Hide()
                    '    End If
                    'Next

                    'If dtOffer.Rows.Count > 0 Then
                    '    cnt = offerFirstRow
                    '    For Each rs As DataRow In dtOffer.Rows
                    '        Dim rowUyaram As Integer = Math.Ceiling(rs(2).ToString.Length / mergeCalwidth) * 15.2
                    '        ws.Row(cnt).Height = rowUyaram + 15
                    '        cnt = cnt + 1
                    '    Next
                    'End If
                End If
            Next

            'Offer Cancellation Policy
            mySqlCmd = New SqlCommand("sp_rep_cancelpolicy", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = promotionDr("PromotionID")
            mySqlCmd.Parameters.Add(New SqlParameter("@AgentCode", SqlDbType.VarChar, 20)).Value = txtAgentCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@sourcecountry", SqlDbType.VarChar, 20)).Value = txtCtryCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@FromDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@ToDate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim dtPolicy As New DataTable
            myDataAdapter.Fill(dtPolicy)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()
            If dtPolicy.Rows.Count > 0 Then
                Dim canPolicy As IXLRange
                cnt = LastLine
                ws.Range(cnt, 1, cnt, dt.Columns.Count - 1).Merge()
                ws.Cell(cnt, 1).Value = "CANCELLATION / NO SHOW / EARLY CHECKOUT - POLICY"
                TitleStyle(ws.Cell(cnt, 1))
                ws.Range(cnt, 1, cnt, dt.Columns.Count - 1).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium)
                cnt = cnt + 1
                ws.Cell(cnt, 1).Value = "Season"
                ws.Cell(cnt, 2).Value = "Period"
                ws.Range(cnt, 2, cnt, 3).Merge()
                ws.Cell(cnt, 4).Value = "Applicable Room Types"
                ws.Range(cnt, 4, cnt, 6).Merge()
                ws.Cell(cnt, 7).Value = "Meal Plan"
                ws.Range(cnt, 7, cnt, 9).Merge()
                ws.Cell(cnt, 10).Value = "Cancellation / No Show / Early Checkout - Policy"
                ws.Range(cnt, 10, cnt, dt.Columns.Count - 1).Merge()
                ws.Cell(cnt, 10).Style.Alignment.SetWrapText(True)
                mergeCalwidth = mergeCalwidth + ws.Column(dt.Columns.Count - 2).Width + ws.Column(dt.Columns.Count - 1).Width
                If ws.Cell(cnt, 10).Value.ToString.Trim.Length > mergeCalwidth Then
                    ws.Row(cnt).Height = Math.Ceiling(ws.Cell(cnt, 10).Value.ToString.Length / mergeCalwidth) * 15.2
                End If
                ws.Range(cnt, 1, cnt, dt.Columns.Count - 1).Style.Font.SetBold(True).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                ws.Range(cnt, 1, cnt, dt.Columns.Count - 1).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                Dim pstartRow = cnt + 1
                For Each rs As DataRow In dtPolicy.Rows
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = rs(0)
                    ws.Cell(cnt, 2).Value = rs(1)
                    ws.Range(cnt, 2, cnt, 3).Merge()
                    If ws.Cell(cnt, 2).Value.ToString.Substring(0, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(0, 1)
                    If ws.Cell(cnt, 2).Value.ToString.Substring(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1) = """" Then ws.Cell(cnt, 2).Value = ws.Cell(cnt, 2).Value.ToString.Remove(ws.Cell(cnt, 2).Value.ToString.Length - 1, 1)
                    ws.Cell(cnt, 4).Value = rs(2)
                    ws.Range(cnt, 4, cnt, 6).Merge()
                    ws.Cell(cnt, 7).Value = rs(3)
                    ws.Range(cnt, 7, cnt, 9).Merge()
                    ws.Cell(cnt, 10).Value = rs(4)
                    ws.Range(cnt, 10, cnt, dt.Columns.Count).Merge()
                    FormatSpecificString(ws.Cell(cnt, 10))
                    Dim rowH2 = Math.Ceiling(rs(1).ToString.Length / (ws.Column(2).Width + ws.Column(3).Width)) * 15.2
                    Dim rowH3 = Math.Ceiling(rs(2).ToString.Length / (ws.Column(4).Width + ws.Column(5).Width + ws.Column(6).Width)) * 15.2
                    Dim rowH6 = Math.Ceiling(rs(4).ToString.Length / mergeCalwidth) * 15.2
                    Dim rowUyaram As Integer
                    If rowH2 > rowH3 Then rowUyaram = rowH2 Else rowUyaram = rowH3
                    If rowUyaram < rowH6 Then rowUyaram = rowH6
                    ws.Row(cnt).Height = rowUyaram + 15
                Next
                canPolicy = ws.Range(pstartRow, 1, cnt, dt.Columns.Count - 1)
                canPolicy.Style.Alignment.SetWrapText(True)
                canPolicy.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                canPolicy.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                canPolicy.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                canPolicy.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                ws.Row(CancelLink).Cell(1).Hyperlink.InternalAddress = "A" + (LastLine).ToString + ":W" + canPolicy.LastRow.WorksheetRow.RowNumber().ToString
                ws.Row(CancelLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
                Dim patternRow As Integer = canPolicy.LastRow.RowNumber
                ws.Range(patternRow + 1, 1, patternRow + 2, dt.Columns.Count - 1).Style.Fill.SetBackgroundColor(XLColor.White)
                LastLine = patternRow + 3
            Else
                For i = CancelLink To TermLink - 1
                    ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value
                Next
                TermLink = TermLink - 1
                ws.Range(TermLink, 1, TermLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                If OfferLastLine > LinkLastLine Then
                    ws.Cell(TermLink + 1, 1).Value = ""
                    ws.Range(TermLink + 1, 1, TermLink + 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                Else
                    ws.Row(TermLink + 1).Delete()
                    LastLine = LastLine - 1
                    LinkLastLine = LinkLastLine - 1
                End If
            End If
            mySqlCmd = New SqlCommand("sp_rep_offer_terms", SqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = promotionDr("PromotionID")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim DsTerms As New DataSet
            Dim dtRemark As New DataTable
            Dim dtRoomUpgrade As New DataTable
            Dim dtMealUpgrade As New DataTable
            Dim dtFreeNights As New DataTable
            Dim dtSpecial As New DataTable
            Dim dtInterHotel As New DataTable
            Dim dtFlight As New DataTable
            Dim dtCompliment As New DataTable
            myDataAdapter.Fill(DsTerms)
            dtRemark = DsTerms.Tables(0)
            dtRoomUpgrade = DsTerms.Tables(1)
            dtMealUpgrade = DsTerms.Tables(2)
            dtFreeNights = DsTerms.Tables(3)
            dtSpecial = DsTerms.Tables(4)
            dtInterHotel = DsTerms.Tables(5)
            dtFlight = DsTerms.Tables(6)
            dtCompliment = DsTerms.Tables(7)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()
            SqlConn.Close()

            Dim EnableTerms As Boolean = False
            For Each tDt As DataTable In DsTerms.Tables
                If tDt.Rows.Count > 0 Then
                    EnableTerms = True
                    Exit For
                End If
            Next
            If EnableTerms = True Then
                'Terms and Conditions - Remarks
                Dim termMergeWidth As Integer = 0
                Dim TermFirstRow As Integer = LastLine
                cnt = LastLine
                ws.Range(cnt, 1, cnt, 9).Merge()
                ws.Cell(cnt, 1).Value = "TERMS AND CONDITIONS"
                TitleStyle(ws.Cell(cnt, 1))
                ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium)
                cnt = cnt + 1
                LastLine = cnt
                For i = 1 To 9
                    termMergeWidth = termMergeWidth + ws.Column(i).Width
                Next
                If dtRemark.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Remarks"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    Dim rngRemarks As IXLRange
                    Dim Remark1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtRemark.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs("Remarks")
                        ws.Range(cnt, 1, cnt, 9).Merge()
                        ws.Row(cnt).Height = (Math.Ceiling(rs(0).ToString.Length / termMergeWidth * 21)) + 15
                    Next
                    rngRemarks = ws.Range(Remark1Row, 1, cnt, 9)
                    rngRemarks.Style.Alignment.SetWrapText(True)
                    rngRemarks.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngRemarks.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    rngRemarks.LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    rngRemarks.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Room Upgrade
                cnt = LastLine
                If dtRoomUpgrade.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Room Upgrade"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Upgrade From"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Upgrade To"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngRoomUpgrade As IXLRange
                    Dim RoomUpgrd1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtRoomUpgrade.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs("UpgradeFrom")
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs("UpgradeTo")
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngRoomUpgrade = ws.Range(RoomUpgrd1Row, 1, cnt, 4)
                    rngRoomUpgrade.Style.Alignment.SetWrapText(True)
                    rngRoomUpgrade.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngRoomUpgrade.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(RoomUpgrd1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(RoomUpgrd1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Meal Upgrade
                cnt = LastLine
                If dtMealUpgrade.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Meal Upgrade"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Upgrade From"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Upgrade To"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngMealUpgrade As IXLRange
                    Dim MealUpgrd1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtMealUpgrade.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs("UpgradeFrom")
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs("UpgradeTo")
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngMealUpgrade = ws.Range(MealUpgrd1Row, 1, cnt, 4)
                    rngMealUpgrade.Style.Alignment.SetWrapText(True)
                    rngMealUpgrade.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngMealUpgrade.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(MealUpgrd1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(MealUpgrd1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Free Nights
                cnt = LastLine
                If dtFreeNights.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Free Nights"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "From Date"
                    ws.Cell(cnt, 2).Value = "To Date"
                    ws.Cell(cnt, 3).Value = "Booking Code"
                    ws.Cell(cnt, 4).Value = "Apply Free Nights To"
                    ws.Cell(cnt, 5).Value = "Stay For"
                    ws.Cell(cnt, 6).Value = "Pay For"
                    ws.Cell(cnt, 7).Value = "Allow Multiples"
                    ws.Cell(cnt, 8).Value = "Max Multiples Allowed"
                    ws.Cell(cnt, 9).Value = "Max Free Nights"
                    If ws.Column(7).Width < 15 Then ws.Column(7).Width = 15
                    If ws.Column(8).Width < 15 Then ws.Column(8).Width = 15
                    ws.Range(cnt, 1, cnt, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Row(cnt).Cells(3, 9).Style.Alignment.SetWrapText(True)
                    Dim rngFreeNights As IXLRange
                    cnt = cnt + 1
                    rngFreeNights = ws.Cell(cnt, 1).InsertTable(dtFreeNights).SetShowHeaderRow(False).AsRange()
                    ws.Rows(cnt).Clear()
                    ws.Rows(cnt).Delete()
                    rngFreeNights.Columns("1:2").SetDataType(XLCellValues.Text)
                    rngFreeNights.Columns("5:6,8:9").SetDataType(XLCellValues.Number)
                    rngFreeNights.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngFreeNights.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    rngFreeNights.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    cnt = rngFreeNights.LastRow.RowNumber
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Special Occasion
                cnt = LastLine
                If dtSpecial.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Special Occasion"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    Dim rngSpecial As IXLRange
                    Dim Special1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtSpecial.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 9).Merge()
                        ws.Row(cnt).Height = (Math.Ceiling(rs(0).ToString.Length / termMergeWidth * 15.2)) + 15
                    Next
                    rngSpecial = ws.Range(Special1Row, 1, cnt, 9)
                    rngSpecial.Style.Alignment.SetWrapText(True)
                    rngSpecial.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngSpecial.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    rngSpecial.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Inter Hotels
                cnt = LastLine
                If dtInterHotel.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Inter Hotels"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Inter Hotel Stay Required"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Minimum Nights"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngInterHotel As IXLRange
                    Dim InterHotel1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtInterHotel.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs(1)
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngInterHotel = ws.Range(InterHotel1Row, 1, cnt, 4)
                    rngInterHotel.Style.Alignment.SetWrapText(True)
                    rngInterHotel.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngInterHotel.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(InterHotel1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(InterHotel1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Selected Flights Only
                cnt = LastLine
                If dtFlight.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Selected Flights Only (This offer is applicable only to arrivals in the below flights)"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    Dim rngFlight As IXLRange
                    Dim Flight1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtFlight.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 9).Merge()
                        ws.Row(cnt).Height = (Math.Ceiling(rs(0).ToString.Length / termMergeWidth * 15.2)) + 15
                    Next
                    rngFlight = ws.Range(Flight1Row, 1, cnt, 9)
                    rngFlight.Style.Alignment.SetWrapText(True)
                    rngFlight.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngFlight.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    rngFlight.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If

                'Terms and Conditions - Complimentary Transfers
                cnt = LastLine
                If dtCompliment.Rows.Count > 0 Then
                    ws.Range(cnt, 1, cnt, 9).Merge()
                    ws.Cell(cnt, 1).Value = "Complimentary Transfers"
                    ws.Range(cnt, 1, cnt, 9).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Cell(cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Cell(cnt, 1).Style.Fill.SetBackgroundColor(XLColor.OrangeRyb)
                    cnt = cnt + 1
                    ws.Cell(cnt, 1).Value = "Transfer Type"
                    ws.Range(cnt, 1, cnt, 2).Merge()
                    ws.Cell(cnt, 3).Value = "Applicable Airport"
                    ws.Range(cnt, 3, cnt, 4).Merge()
                    ws.Range(cnt, 1, cnt, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Font.Bold = True
                    ws.Range(cnt, 1, cnt, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    Dim rngCompliment As IXLRange
                    Dim Compliment1Row As Integer = cnt + 1
                    For Each rs As DataRow In dtCompliment.Rows
                        cnt = cnt + 1
                        ws.Cell(cnt, 1).Value = rs(0)
                        ws.Range(cnt, 1, cnt, 2).Merge()
                        ws.Cell(cnt, 3).Value = rs(1)
                        ws.Range(cnt, 3, cnt, 4).Merge()
                        Dim rowH1 As Integer = Math.Ceiling(rs(0).ToString.Length / (ws.Column(1).Width + ws.Column(2).Width)) * 15.2
                        Dim rowH3 As Integer = Math.Ceiling(rs(1).ToString.Length / (ws.Column(3).Width + ws.Column(4).Width)) * 15.2
                        If rowH1 > rowH3 Then
                            ws.Row(cnt).Height = rowH1 + 15
                        Else
                            ws.Row(cnt).Height = rowH3 + 15
                        End If
                    Next
                    rngCompliment = ws.Range(Compliment1Row, 1, cnt, 4)
                    rngCompliment.Style.Alignment.SetWrapText(True)
                    rngCompliment.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    rngCompliment.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    ws.Range(Compliment1Row - 1, 5, cnt, 9).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(Compliment1Row - 1, 5, cnt, 9).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
                    ws.Range(LastLine, 10, cnt, dt.Columns.Count).Style.Fill.SetBackgroundColor(XLColor.White)
                    LastLine = cnt + 1
                End If
                ws.Range(LastLine - 1, 1, LastLine - 1, 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                ' ws.Range(1, 24, LastLine + 1, 24).Style.Fill.SetBackgroundColor(XLColor.White)
                ws.Range(LastLine, 1, LastLine + 1, 24).Style.Fill.SetBackgroundColor(XLColor.White)
                ws.Row(TermLink).Cell(1).Hyperlink.InternalAddress = "A" + (TermFirstRow).ToString + ":I" + LastLine.ToString()
                ws.Row(TermLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
            Else
                ws.Range(TermLink - 1, 1, TermLink - 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
                If OfferLastLine > LinkLastLine Then
                    ws.Cell(TermLink, 1).Value = ""
                    ws.Range(TermLink, 1, TermLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
                Else
                    ws.Row(TermLink).Delete()
                    LastLine = LastLine - 1
                    LinkLastLine = LinkLastLine - 1
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region




#Region "Protected Sub TitleStyle(ByVal cellRng As IXLCell)"
    Protected Sub TitleStyle(ByVal cellRng As IXLCell)
        Dim script As IXLStyle = cellRng.Style
        script.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

        If chkcolinExcel.Checked Then
            script.Font.FontName = "Trebuchet MS"
        Else
            script.Font.FontName = "Times New Roman"
        End If


        script.Font.FontSize = 11
        script.Fill.BackgroundColor = XLColor.MediumElectricBlue
        script.Font.FontColor = XLColor.White
        script.Font.Bold = True
    End Sub
#End Region

#Region "Protected Sub FormatSpecificString(ByVal cel As IXLCell)"
    Protected Sub FormatSpecificString(ByVal cel As IXLCell)
        cel.DataType = XLCellValues.Text
        Dim SearchStr As New List(Of String)
        SearchStr.Add("Cancellation Charges")
        SearchStr.Add("No Show Charges")
        SearchStr.Add("Early Checkout Charges")
        Dim intex As Integer
        For Each Str As String In SearchStr
            Dim cnt As Integer = Str.Length
            Dim pos As Integer = 0
            Dim status As Boolean = True
            Do While (status)
                intex = cel.Value.ToString.IndexOf(Str, pos)
                If intex > 0 Then
                    cel.RichText.Substring(intex, cnt).SetBold(True).SetFontColor(XLColor.Red)
                    pos = intex + cnt
                    status = True
                Else
                    status = False
                    Exit Do
                End If
            Loop
        Next
    End Sub
#End Region

#Region "Protected Sub BtnClear_Click(sender As Object, e As System.EventArgs) Handles BtnClear.Click"
    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        txtHotelName.Text = ""
        txtHotelCode.Text = ""
        txtCtryCode.Text = ""
        txtCtryName.Text = ""
        txtAgentCode.Text = ""
        txtAgentName.Text = ""
        rdoNetPayable.Checked = True
        txtFromDate.Text = ""
        txtToDate.Text = ""
        txtHotelName.Focus()
    End Sub
#End Region

#Region "Protected Sub CheckInCheckOutPolicy(ByRef ws As IXLWorksheet, ByVal contractids As String, ByRef LastLine As Integer, ByVal HideList As List(Of Integer), ByVal CheckInOutLink As Integer, ByRef GeneralLink As Integer, ByVal OfferLastLine As Integer, ByRef LinkLastLine As Integer)"
    Protected Sub CheckInCheckOutPolicy(ByRef ws As IXLWorksheet, ByVal contractids As String, ByRef LastLine As Integer, ByVal HideList As List(Of Integer), ByVal CheckInOutLink As Integer, ByRef GeneralLink As Integer, ByVal OfferLastLine As Integer, ByRef LinkLastLine As Integer)
        mySqlCmd = New SqlCommand("sp_rep_CheckinOutContracts", SqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@rateplanid", SqlDbType.VarChar, 1000)).Value = contractids
        myDataAdapter = New SqlDataAdapter
        myDataAdapter.SelectCommand = mySqlCmd
        Dim ds As New DataSet
        Dim dt As New DataTable
        myDataAdapter.Fill(ds)
        dt = ds.Tables(0)
        mySqlCmd.Dispose()
        myDataAdapter.Dispose()
        Dim sqlqry As String = ""
        If dt.Rows.Count < 1 Then
            For i = CheckInOutLink To GeneralLink - 1
                ws.Cell(i, 1).Value = ws.Cell(i + 1, 1).Value
            Next
            GeneralLink = GeneralLink - 1
            ws.Range(GeneralLink, 1, GeneralLink, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
            If OfferLastLine > LinkLastLine Then
                ws.Cell(GeneralLink + 1, 1).Value = ""
                ws.Range(GeneralLink + 1, 1, GeneralLink + 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None).Fill.SetBackgroundColor(XLColor.White)
            Else
                ws.Row(GeneralLink + 1).Delete()
                LastLine = LastLine - 1
                LinkLastLine = LinkLastLine - 1
            End If
            Exit Sub
        End If
        Dim colList As New List(Of Integer)
        For i = 1 To 5
            colList.Add(i)
        Next
        Dim ColNo As Integer = 6
        Do While (colList.Count < 10)
            If HideList.IndexOf(ColNo) < 0 Then
                colList.Add(ColNo)
            End If
            ColNo = ColNo + 1
        Loop
        'Dim contractids As String = ""
        Dim cnt As Integer
        Dim CheckinOutFirstRow = LastLine + 2
        For Each dr In dt.Rows
            ws.Range(LastLine + 1, 1, LastLine + 4, 13).Style.Fill.SetBackgroundColor(XLColor.White)
            LastLine = LastLine + 2
            ws.Range(LastLine, 1, LastLine, 2).Merge.Style.Font.SetBold(True)
            ws.Cell(LastLine, 1).Value = "Check In Check Out ID"
            ws.Range(LastLine, 3, LastLine, 6).Merge.Style.Font.SetBold(True)
            ws.Cell(LastLine, 3).Value = dr("checkinoutpolicyid")
            If chkcolinExcel.Checked Then

                ws.Range(LastLine, 1, LastLine, 6).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
            Else
                ws.Range(LastLine, 1, LastLine, 6).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11)
            End If

            ws.Range(LastLine, 1, LastLine, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(63, 162, 238))
            LastLine = LastLine + 2
            cnt = LastLine
            ws.Range(cnt, 1, cnt, colList(9)).Merge()
            ws.Cell(cnt, 1).Value = "POLICY - CHECK-IN / CHECK-OUT"
            TitleStyle(ws.Cell(cnt, 1))
            ws.Range(cnt, 1, cnt, colList(9)).Style.Border.SetRightBorder(XLBorderStyleValues.Medium).Border.SetLeftBorder(XLBorderStyleValues.Medium).Border.SetTopBorder(XLBorderStyleValues.Medium)
            cnt = cnt + 1
            ws.Range(cnt, 1, cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
            cnt = cnt + 1
            ws.Range(cnt, 1, cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Cell(cnt, 1).Value = "CHECK IN TIME"
            ws.Cell(cnt, 2).Value = dr("checkintime") + "HRS"
            ws.Range(cnt, 4, cnt, 5).Merge.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Cell(cnt, 4).Value = "CHECK OUT TIME"
            ws.Range(cnt, colList(5), cnt, colList(6)).Merge.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Cell(cnt, colList(5)).Value = dr("checkouttime") + "HRS"
            cnt = cnt + 1
            ws.Range(cnt, 1, cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
            cnt = cnt + 1
            Dim rLine As Integer = cnt
            ws.Range(cnt, 1, cnt, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Cell(cnt, 1).Value = "PERIOD Applicable"
            ws.Cell(cnt, 2).Value = "From"
            ws.Cell(cnt, 3).Value = "Till"
            cnt = cnt + 1
            Dim strSeason As String = dr("seasons")
            Dim strCond As String = ""
            If strSeason Is Nothing = False Then
                If strSeason.Length > 0 Then
                    Dim ssString As String() = strSeason.Split(",")
                    For i As Integer = 0 To ssString.Length - 1
                        If strCond = "" Then
                            strCond = "'" & ssString(i) & "'"
                        Else
                            strCond &= ",'" & ssString(i) & "'"
                        End If
                    Next
                End If
            End If
            sqlqry = " select  distinct * from view_contractseasons(nolock) where contractid=@contractID and seasonname IN (" & strCond & ") order by fromdate"
            mySqlCmd = New SqlCommand(sqlqry, SqlConn)
            mySqlCmd.CommandType = CommandType.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@contractID", SqlDbType.VarChar, 20)).Value = dr("Contractid")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim dtDt As New DataTable
            myDataAdapter.Fill(dtDt)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()
            For Each drSeason In dtDt.Rows
                ws.Cell(cnt, 2).Value = drSeason("fromDate")
                ws.Cell(cnt, 2).Style.NumberFormat.Format = "dd/MM/yyyy"
                ws.Cell(cnt, 3).Value = drSeason("toDate")
                ws.Cell(cnt, 3).Style.NumberFormat.Format = "dd/MM/yyyy"
                cnt = cnt + 1
            Next
            Dim checkDtCnt As Integer = cnt
            cnt = rLine
            ws.Range(cnt, 4, cnt, 5).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Cell(cnt, 4).Value = "ROOM TYPE Applicable"
            sqlqry = "select R.checkinoutpolicyid,R.rmtypcode,P.rmtypname from contracts_checkinout_roomtypes R inner join partyrmtyp P on R.rmtypcode=P.rmtypcode and R.checkinoutpolicyid=@checkinoutpolicyid and P.partycode=@PartyCode order by rankord"
            mySqlCmd = New SqlCommand(sqlqry, SqlConn)
            mySqlCmd.CommandType = CommandType.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@PartyCode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = dr("checkinoutpolicyid")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim dtRoom As New DataTable
            myDataAdapter.Fill(dtRoom)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()
            For Each drRoom In dtRoom.Rows
                ws.Range(cnt, colList(5), cnt, colList(9)).Merge()
                ws.Cell(cnt, colList(5)).Value = drRoom("RmTypName")
                cnt = cnt + 1
            Next
            If checkDtCnt > cnt Then
                ws.Range(cnt, 5, checkDtCnt, colList(9)).Merge()
                cnt = checkDtCnt
            Else
                ws.Range(checkDtCnt, 1, cnt, 4).Merge()
            End If
            ws.Range(LastLine + 1, 1, cnt, colList(9)).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(LastLine + 2, 1, cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
            ws.Range(LastLine + 2, 2, LastLine + 2, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(60, 179, 113)).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            ws.Range(LastLine + 2, colList(5), LastLine + 2, colList(6)).Style.Fill.SetBackgroundColor(XLColor.FromArgb(205, 92, 92)).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            ws.Range(LastLine + 2, 1, cnt - 1, 1).Style.Font.SetBold(True)
            ws.Range(LastLine + 2, 4, cnt - 1, 5).Style.Font.SetBold(True)
            ws.Range(LastLine + 4, 2, LastLine + 4, 3).Style.Font.SetBold(True)
            ws.Range(LastLine + 4, 2, cnt - 1, 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            ws.Range(LastLine + 4, colList(5), cnt - 1, colList(9)).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

            If chkcolinExcel.Checked Then
                ws.Range(LastLine + 2, 1, cnt - 1, colList(9)).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
            Else
                ws.Range(LastLine + 2, 1, cnt - 1, colList(9)).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11)
            End If

            'ws.Range(cnt, 1, cnt, colList(9)).LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)

            LastLine = cnt

            sqlqry = "select * from contracts_checkinout_restricted where checkinoutpolicyid = @checkinoutpolicyid order by datetype,rlineno"
            mySqlCmd = New SqlCommand(sqlqry, SqlConn)
            mySqlCmd.CommandType = CommandType.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = dr("checkinoutpolicyid")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim dtRestrict As New DataTable
            myDataAdapter.Fill(dtRestrict)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()

            If dtRestrict.Rows.Count > 0 Then
                cnt = cnt + 1
                ws.Range(cnt, 1, cnt, 2).Merge().Value = "RESTRICTED CHECK-IN DATES"
                ws.Range(cnt, 3, cnt, colList(5)).Merge().Value = "RESTRICTED CHECK-OUT DATES"
                Dim script As IXLStyle = ws.Range(cnt, 1, cnt, colList(5)).Style
                script.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                If chkcolinExcel.Checked Then
                    script.Font.FontName = "Trebuchet MS"
                Else
                    script.Font.FontName = "Times New Roman"
                End If

                script.Font.FontSize = 11
                script.Fill.BackgroundColor = XLColor.FromArgb(255, 228, 181)
                script.Font.FontColor = XLColor.Black
                script.Font.Bold = True
                Dim drCheckIn() As DataRow = dtRestrict.Select("dateType='Checkin'")
                Dim drCheckOut() As DataRow = dtRestrict.Select("dateType='Checkout'")
                Dim tmpLine As Integer = cnt
                If drCheckIn.Length > 0 Then
                    For i = 0 To drCheckIn.Length - 1
                        cnt = cnt + 1
                        ws.Range(cnt, 1, cnt, 2).Merge.Value = drCheckIn(i).Item("restrictDate")
                        ws.Cell(cnt, 1).Style.NumberFormat.Format = "dd/MM/yyyy"
                    Next
                End If
                Dim CheckInCnt As Integer = cnt
                cnt = tmpLine
                If drCheckOut.Length > 0 Then
                    For i = 0 To drCheckOut.Length - 1
                        cnt = cnt + 1
                        ws.Range(cnt, 3, cnt, colList(5)).Merge.Value = drCheckOut(i).Item("restrictDate")
                        ws.Cell(cnt, 3).Style.NumberFormat.Format = "dd/MM/yyyy"
                    Next
                End If
                If CheckInCnt > cnt Then
                    For i = cnt To CheckInCnt
                        ws.Range(i, 3, i, colList(5)).Merge()
                    Next
                    cnt = CheckInCnt
                Else
                    For i = CheckInCnt To cnt
                        ws.Range(i, 1, i, 2).Merge()
                    Next
                End If
                ws.Range(LastLine + 1, colList(6), cnt, colList(9)).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(LastLine + 1, colList(6), cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
                ws.Range(LastLine + 2, 1, cnt + 1, colList(9)).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(LastLine + 2, 1, cnt + 1, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
                ws.Range(LastLine + 2, 1, cnt + 1, colList(9)).LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                ws.Range(LastLine + 1, 1, cnt, colList(5)).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)


                If chkcolinExcel.Checked Then
                    ws.Range(LastLine + 2, 1, cnt, colList(5)).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
                Else
                    ws.Range(LastLine + 2, 1, cnt, colList(5)).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11)
                End If

                LastLine = cnt + 1
            Else
                ws.Range(LastLine, 1, LastLine, colList(9)).LastRow.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            End If

            sqlqry = "select * from contracts_checkinout_detail where checkinoutpolicyid = @checkinoutpolicyid order by checkinouttype,clineno"
            mySqlCmd = New SqlCommand(sqlqry, SqlConn)
            mySqlCmd.CommandType = CommandType.Text
            mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = dr("checkinoutpolicyid")
            myDataAdapter = New SqlDataAdapter
            myDataAdapter.SelectCommand = mySqlCmd
            Dim dtEinLout As New DataTable
            myDataAdapter.Fill(dtEinLout)
            mySqlCmd.Dispose()
            myDataAdapter.Dispose()

            If dtEinLout.Rows.Count > 0 Then
                ws.Range(LastLine + 1, 1, LastLine + 2, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
                LastLine = LastLine + 2
                cnt = LastLine
                ws.Range(cnt, 1, cnt, colList(9)).Merge().Value = "POLICY - EARLY CHECK-IN / LATE CHECK-OUT"
                Dim script As IXLStyle = ws.Range(cnt, 1, cnt, colList(9)).Style
                script.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                If chkcolinExcel.Checked Then
                    script.Font.FontName = "Trebuchet MS"
                Else
                    script.Font.FontName = "Times New Roman"
                End If

                script.Font.FontSize = 11
                script.Fill.BackgroundColor = XLColor.FromArgb(60, 179, 113)
                script.Font.FontColor = XLColor.Black
                script.Font.Bold = True
                Dim tmpLine As Integer = cnt
                cnt = cnt + 1
                ws.Range(cnt, 2, cnt, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(180, 182, 233)).Font.SetBold(True)
                ws.Cell(cnt, 2).Value = "From"
                ws.Range(cnt, 3, cnt, 3).Style.Fill.SetBackgroundColor(XLColor.FromArgb(180, 182, 233)).Font.SetBold(True)
                ws.Cell(cnt, 3).Value = "Till"
                For Each drSeason In dtDt.Rows
                    cnt = cnt + 1
                    ws.Cell(cnt, 2).Value = drSeason("fromDate")
                    ws.Cell(cnt, 2).Style.NumberFormat.Format = "dd/MM/yyyy"
                    ws.Cell(cnt, 3).Value = drSeason("toDate")
                    ws.Cell(cnt, 3).Style.NumberFormat.Format = "dd/MM/yyyy"
                Next
                Dim dtCnt As Integer = cnt
                cnt = tmpLine
                For Each drRoom In dtRoom.Rows
                    cnt = cnt + 1
                    ws.Range(cnt, colList(5), cnt, colList(9)).Merge()
                    ws.Cell(cnt, colList(5)).Value = drRoom("RmTypName")
                Next
                ws.Range(tmpLine + 1, colList(5), cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.FromArgb(240, 230, 198))
                If dtCnt > cnt Then
                    ws.Range(tmpLine + 1, 1, dtCnt, 1).Merge.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)
                    ws.Range(tmpLine + 1, 4, dtCnt, 5).Merge.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)
                    ws.Range(cnt + 1, colList(5), dtCnt, colList(9)).Merge()
                    cnt = dtCnt
                Else
                    ws.Range(tmpLine + 1, 1, cnt, 1).Merge.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)
                    ws.Range(dtCnt + 1, 2, cnt, 3).Merge()
                    ws.Range(tmpLine + 1, 4, cnt, 5).Merge.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)
                End If
                ws.Cell(tmpLine + 1, 1).Value = "Applicable Period"
                ws.Cell(tmpLine + 1, 4).Value = "Applicable Room Types"
                ws.Range(tmpLine, 1, cnt, colList(9)).Style.Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
                ws.Range(tmpLine, 1, cnt, colList(9)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)



                If chkcolinExcel.Checked Then
                    ws.Range(tmpLine + 1, 1, cnt, colList(9)).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
                Else
                    ws.Range(tmpLine + 1, 1, cnt, colList(9)).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11)
                End If

                cnt = cnt + 1
                ws.Range(cnt, 1, cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
                ws.Range(cnt, 1, cnt, colList(9)).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                Dim DrEIn() As DataRow = dtEinLout.Select("checkinouttype='Early CheckIn'")
                Dim DrLOut() As DataRow = dtEinLout.Select("checkinouttype='Late CheckOut'")
                If DrEIn.Length > 0 Then
                    tmpLine = cnt + 1
                    cnt = cnt + 1
                    ws.Range(cnt, 1, cnt, 1).Style.Font.SetBold(True)
                    ws.Cell(cnt, 1).Value = "Early Check-In Time (From)"
                    ws.Column(1).AdjustToContents()
                    ws.Range(cnt, 2, cnt, 3).Merge().Style.Font.SetBold(True)
                    ws.Cell(cnt, 2).Value = "Early check-in time (Till)"
                    ws.Range(cnt, 4, cnt, colList(5)).Merge().Style.Font.SetBold(True)
                    ws.Cell(cnt, 4).Value = "Charge applicable"
                    ws.Range(cnt, colList(6), cnt, colList(9)).Merge().Style.Font.SetBold(True)
                    ws.Cell(cnt, colList(6)).Value = "Meal plan"
                    For i = 0 To DrEIn.Length - 1
                        If (DrEIn(i).Item("percentage") <> 100) Then
                            cnt = cnt + 1
                            If Not String.IsNullOrEmpty(DrEIn(i).Item("fromHours")) Then
                                ws.Cell(cnt, 1).Value = DrEIn(i).Item("fromHours") + " hrs"
                            End If
                            If Not String.IsNullOrEmpty(DrEIn(i).Item("toHours")) Then
                                ws.Range(cnt, 2, cnt, 3).Merge().Value = DrEIn(i).Item("toHours") + " hrs"
                            Else
                                ws.Range(cnt, 2, cnt, 3).Merge()
                            End If
                            ws.Range(cnt, 4, cnt, colList(5)).Merge().Value = Convert.ToString(DrEIn(i).Item("percentage")) + "% of 1 night charge"
                            ws.Range(cnt, colList(6), cnt, colList(9)).Merge().Value = "Basic / Booked meal plan"
                        Else
                            cnt = cnt + 1
                            If Not String.IsNullOrEmpty(DrEIn(i).Item("fromHours")) Then
                                ws.Cell(cnt, 1).Value = DrEIn(i).Item("fromHours") + " hrs"
                            End If
                            If Not String.IsNullOrEmpty(DrEIn(i).Item("toHours")) Then
                                ws.Range(cnt, 2, cnt, 3).Merge().Value = DrEIn(i).Item("toHours") + " hrs"
                            Else
                                ws.Range(cnt, 2, cnt, 3).Merge()
                            End If
                            ws.Range(cnt, 4, cnt, colList(5)).Merge().Value = Convert.ToString(DrEIn(i).Item("percentage")) + "% of 1 night charge"
                            ws.Range(cnt, colList(6), cnt, colList(9)).Merge().Value = "Basic / Booked meal plan"
                            cnt = cnt + 1
                            ws.Range(cnt, 1, cnt, colList(9)).Merge().Value = "100% charge applies for early check-In before : " + DrEIn(i).Item("fromHours") + " hrs"
                            ws.Range(cnt, 1, cnt, colList(9)).Style.Font.SetFontColor(XLColor.FromArgb(168, 33, 18)).Font.SetBold(True)
                            'ws.Range(cnt, 4, cnt, colList(9)).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            'ws.Cell(cnt, 4).Value = DrEIn(i).Item("fromHours") + " hrs"
                        End If
                    Next
                    ws.Range(tmpLine, 1, cnt, colList(9)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Range(tmpLine, 1, cnt, colList(9)).Style.Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)

                    If chkcolinExcel.Checked Then
                        ws.Range(tmpLine, 1, cnt, colList(9)).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
                    Else
                        ws.Range(tmpLine, 1, cnt, colList(9)).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11)
                    End If

                    cnt = cnt + 1
                    ws.Range(cnt, 1, cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White)
                    ws.Range(cnt, 1, cnt, colList(9)).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                End If
                If DrLOut.Length > 0 Then
                    tmpLine = cnt + 1
                    cnt = cnt + 1
                    ws.Range(cnt, 1, cnt, 1).Style.Font.SetBold(True)
                    ws.Cell(cnt, 1).Value = "Late Check-Out Time (From)"
                    ws.Column(1).AdjustToContents()
                    ws.Range(cnt, 2, cnt, 3).Merge().Style.Font.SetBold(True)
                    ws.Cell(cnt, 2).Value = "Late Check-Out time (Till)"
                    ws.Range(cnt, 4, cnt, colList(5)).Merge().Style.Font.SetBold(True)
                    ws.Cell(cnt, 4).Value = "Charge applicable"
                    ws.Range(cnt, colList(6), cnt, colList(9)).Merge().Style.Font.SetBold(True)
                    ws.Cell(cnt, colList(6)).Value = "Meal plan"
                    For i = 0 To DrLOut.Length - 1
                        If (DrLOut(i).Item("percentage") <> 100) Then
                            cnt = cnt + 1
                            If Not String.IsNullOrEmpty(DrLOut(i).Item("fromHours")) Then
                                ws.Cell(cnt, 1).Value = DrLOut(i).Item("fromHours") + " hrs"
                            End If
                            If Not String.IsNullOrEmpty(DrLOut(i).Item("toHours")) Then
                                ws.Range(cnt, 2, cnt, 3).Merge().Value = DrLOut(i).Item("toHours") + " hrs"
                            Else
                                ws.Range(cnt, 2, cnt, 3).Merge()
                            End If
                            ws.Range(cnt, 4, cnt, colList(5)).Merge().Value = Convert.ToString(DrLOut(i).Item("percentage")) + "% of 1 night charge"
                            ws.Range(cnt, colList(6), cnt, colList(9)).Merge().Value = "Basic / Booked meal plan"
                        Else
                            cnt = cnt + 1
                            If Not String.IsNullOrEmpty(DrLOut(i).Item("fromHours")) Then
                                ws.Cell(cnt, 1).Value = DrLOut(i).Item("fromHours") + " hrs"
                            End If
                            If Not String.IsNullOrEmpty(DrLOut(i).Item("toHours")) Then
                                ws.Range(cnt, 2, cnt, 3).Merge().Value = DrLOut(i).Item("toHours") + " hrs"
                            Else
                                ws.Range(cnt, 2, cnt, 3).Merge()
                            End If
                            ws.Range(cnt, 4, cnt, colList(5)).Merge().Value = Convert.ToString(DrLOut(i).Item("percentage")) + "% of 1 night charge"
                            ws.Range(cnt, colList(6), cnt, colList(9)).Merge().Value = "Basic / Booked meal plan"
                            cnt = cnt + 1
                            ws.Range(cnt, 1, cnt, colList(9)).Merge().Value = "100% charge applies for Late Check-Out after : " + DrLOut(i).Item("fromHours") + " hrs"
                            ws.Range(cnt, 1, cnt, colList(9)).Style.Font.SetFontColor(XLColor.FromArgb(168, 33, 18)).Font.SetBold(True)
                            'ws.Range(cnt, 4, cnt, colList(9)).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            'ws.Cell(cnt, 4).Value = DrLOut(i).Item("fromHours") + " hrs"
                        End If
                    Next
                    ws.Range(tmpLine, 1, cnt, colList(9)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Range(tmpLine, 1, cnt, colList(9)).Style.Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)


                    If chkcolinExcel.Checked Then
                        ws.Range(tmpLine, 1, cnt, colList(9)).Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(11)
                    Else
                        ws.Range(tmpLine, 1, cnt, colList(9)).Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11)
                    End If

                    cnt = cnt + 1
                    ws.Range(cnt, 1, cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 1, cnt, colList(9)).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                Else
                    ws.Range(cnt, 1, cnt, colList(9)).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    ws.Range(cnt, 1, cnt, colList(9)).LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                End If
                LastLine = cnt
            End If
        Next
        ws.Row(CheckInOutLink).Cell(1).Hyperlink.InternalAddress = "A" + (CheckinOutFirstRow).ToString + ":M" + LastLine.ToString()
        ws.Row(CheckInOutLink).Cell(1).Style.Fill.SetPatternType(XLFillPatternValues.None).Font.SetUnderline(XLFontUnderlineValues.None)
        ws.Range(LastLine + 1, 1, LastLine + 2, 13).Style.Fill.SetBackgroundColor(XLColor.White)
    End Sub
#End Region

#Region "Protected Sub TaxNoteTable(ByRef ws As IXLWorksheet, ByRef LastLine As Integer, ByVal colCnt As Integer)"
    Protected Sub TaxNoteTable(ByRef ws As IXLWorksheet, ByRef LastLine As Integer, ByVal colCnt As Integer)
        Dim taxRange As IXLRange
        'LastLine = LastLine + 1
        'ws.Range(LastLine, 1, LastLine, colCnt).Merge().Clear()
        LastLine = LastLine + 1
        ws.Range(LastLine, 1, LastLine, colCnt).Merge()
        ws.Cell(LastLine, 1).Value = "BELOW RATES ARE INCLUSIVE OF ALL TAXES INCLUDING VAT"
        LastLine = LastLine + 1
        ws.Cell(LastLine, 1).Value = "BELOW RATES DOES NOT INCLUDE TOURISM DIRHAM FEE WHICH IS TO BE PAID BY THE GUEST DIRECTLY AT THE HOTEL"
        ws.Range(LastLine, 1, LastLine, colCnt).Merge()
        taxRange = ws.Range(LastLine - 1, 1, LastLine, colCnt)
        'LastLine = LastLine + 1
        'ws.Range(LastLine, 1, LastLine, colCnt).Merge().Clear()
        taxRange.Style.Alignment.SetWrapText(True)
        If chkcolinExcel.Checked Then
            taxRange.Style.Font.SetFontName("Trebuchet MS").Font.SetFontSize(10).Font.SetBold(True).Fill.SetBackgroundColor(XLColor.Yellow)
        Else
            taxRange.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(10).Font.SetBold(True).Fill.SetBackgroundColor(XLColor.Yellow)
        End If

        taxRange.LastColumn.Style.Border.SetRightBorder(XLBorderStyleValues.Medium)
        taxRange.FirstColumn.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium)
        taxRange.LastRowUsed.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium)
        taxRange.FirstRowUsed.Style.Border.SetTopBorder(XLBorderStyleValues.Medium)
        taxRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
    End Sub
#End Region

End Class
