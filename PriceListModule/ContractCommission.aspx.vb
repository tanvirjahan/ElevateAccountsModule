Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class PriceListModule_ContractCommission
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime
    Private cnt As Long
 

    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Private dt As New DataTable


    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
 
 
 

 




#End Region

#Region "Enum GridCol"
    Enum GridCol

        tranid = 1
        season = 2
        minfromdate = 3
        maxtodate = 4
        applicableto = 5
        Edit = 8
        View = 9
        Delete = 10
        Copy = 11

        DateCreated = 12
        UserCreated = 13
        DateModified = 14
        UserModified = 15

    End Enum
#End Region
    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Function SetVisibility(ByVal desc As Object, ByVal maxlen As Integer) As Boolean

        If desc.ToString = "" Then
            Return False
        Else
            If desc.ToString.Length > maxlen Then
                Return True
            Else
                Return False
            End If
        End If


    End Function
    Function Limit(ByVal desc As Object, ByVal maxlen As Integer) As String

        If desc.ToString = "" Then
            Return ""
        Else
            If desc.ToString.Length > maxlen Then
                desc = desc.Substring(0, maxlen)
            Else

                desc = desc
            End If
        End If

        Return desc


    End Function
    Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub ReadMoreLinkButtoncopycont_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If
            ModalViewrates.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub ReadMoreLinkButtonpromotion_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If
            ModalExtraPopup1.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "Numbers/lock text"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub LockText(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return chkTextLock(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        ' clearall()
        Panelsearch.Enabled = True
        Session("GV_HotelData") = Nothing
        PanelMain.Style("display") = "none"
        'Panelsearch.Style("display")="block")
        ViewState("CopyFrom") = ""
        If Session("Calledfrom") = "Offers" Then
            lblHeading.Text = "Promotion Commission  -" + ViewState("hotelname") + " - " + hdnpromotionid.Value
        Else
            lblHeading.Text = "Contract Commission  -" + ViewState("hotelname") + " - " + hdncontractid.Value
        End If

        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "ContractCommission", CType(Session("ContractState"), String))

        'txtpromotionid.Text = ""
        'txtpromoitonname.Text = ""


        '  Response.Redirect(Request.RawUrl)
        ddlorder.SelectedIndex = 0
        ddlorderby.SelectedIndex = 1
        If Session("Calledfrom") = "Offers" Then
            FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")
        Else
            FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")
        End If

    End Sub
    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        filldates()

    End Sub
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        dt.Columns.Add(New DataColumn("SeasonName", GetType(String)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        'Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("ADD", 0)
        '    row.FindControl("imgStayAdd").Visible = False
        '    Dim txtfromdate As TextBox
        '    txtfromdate = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromdate"), TextBox)
        '    txtfromdate.Focus()
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try


        Dim count As Integer
        Dim GVRow As GridViewRow

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

        count = gvchildage.Rows.Count + 1

        'count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Try
            For Each GVRow In grdDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                'lblseason = GVRow.FindControl("lblseason")
                'excl(n) = CType(lblseason.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                'lblseason = GVRow.FindControl("lblseason")
                'lblseason.Text = excl(n)

                n = n + 1
            Next

            'For Each GVRow In grdDates.Rows
            '    lblseason = GVRow.FindControl("lblseason")
            '    If lblseason.Text = "" Then
            '        lblseason.Text = txtseasonname.Text
            '    End If
            'Next

            Dim txtStayFromDt As TextBox
            txtStayFromDt = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
            txtStayFromDt.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Sub GenerateGridColumns(ByVal lsMode As String, ByVal liRowIndex As Integer)
        Dim dtStayPeriod As New DataTable
        Dim drStayPeriod As DataRow
        dtStayPeriod.Columns.Add(New DataColumn("FromDate", GetType(Date)))
        dtStayPeriod.Columns.Add(New DataColumn("ToDate", GetType(Date)))
        dtStayPeriod.Columns.Add(New DataColumn("SeasonName", GetType(String)))

        Dim txtfromDate As TextBox
        Dim txtToDate As TextBox
        Dim lblseason As Label
        If lsMode.Trim.ToUpper <> "BeLoad" Then
            For Each lRow As GridViewRow In grdDates.Rows
                txtfromDate = lRow.FindControl("txtfromDate")
                txtToDate = lRow.FindControl("txtToDate")
                lblseason = lRow.FindControl("lblseason")
                If IsDate(txtfromDate.Text) And IsDate(txtToDate.Text) Then
                    drStayPeriod = dtStayPeriod.NewRow()
                    drStayPeriod("FromDate") = txtfromDate.Text ' Format(CType(txtfromDate.Text, Date), "dd/MM/yyyy")
                    drStayPeriod("ToDate") = txtToDate.Text 'Format(CType(txtToDate.Text, Date), "dd/MM/yyyy")
                    drStayPeriod("SeasonName") = lblseason.Text
                    dtStayPeriod.Rows.Add(drStayPeriod)
                Else
                    drStayPeriod = dtStayPeriod.NewRow()
                    drStayPeriod("FromDate") = DBNull.Value
                    drStayPeriod("ToDate") = DBNull.Value
                    drStayPeriod("SeasonName") = ""
                    dtStayPeriod.Rows.Add(drStayPeriod)
                End If
            Next
        End If

        If lsMode.Trim.ToUpper = "DELETE" Then
            If (dtStayPeriod.Rows.Count > liRowIndex) Then
                dtStayPeriod.Rows(liRowIndex).Delete()
            End If
        End If
        If lsMode.Trim.ToUpper = "ADD" Or dtStayPeriod.Rows.Count = 0 Then
            drStayPeriod = dtStayPeriod.NewRow()
            drStayPeriod("FromDate") = DBNull.Value
            drStayPeriod("ToDate") = DBNull.Value
            drStayPeriod("SeasonName") = ""
            dtStayPeriod.Rows.Add(drStayPeriod)
        End If
        grdDates.DataSource = dtStayPeriod
        grdDates.DataBind()
    End Sub
    Protected Sub check_changed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox
        Dim lblselect As Label

        Dim objbtn As CheckBox = CType(sender, CheckBox)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        Dim txtseasoncodeCurr As Label = row.FindControl("txtseasoncode")

        For Each grdRow As GridViewRow In grdseason.Rows
            If grdRow.RowIndex <> rowid Then
                chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                lblselect = CType(grdRow.FindControl("lblselect"), Label)
                Dim txtseasoncode As Label = grdRow.FindControl("txtseasoncode")

                If txtseasoncodeCurr.Text.ToUpper.Trim = txtseasoncode.Text.ToUpper.Trim Then
                    chkSelect.Checked = objbtn.Checked
                End If
            End If
        Next
        objbtn.Focus()
    End Sub
    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        'Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("DELETE", row.RowIndex)
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try


        Dim count As Integer

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)
        Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        count = gvchildage.Rows.Count + 1

        Dim GVRow As GridViewRow
        '  count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdDates.Rows
                '  chkSelect = GVRow.FindControl("chkSelect")
                'If chkSelect.Checked = False Then
                If k <> row.RowIndex Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

                k = k + 1
            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdDates.Rows.Count > 1 Then
                fillDategrd(grdDates, False, grdDates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdDates, False, grdDates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If GVRow.RowIndex < count Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpFDate.Text = fDate(n)
                    dpTDate = GVRow.FindControl("txtToDate")
                    dpTDate.Text = tDate(n)
                    'lblseason = GVRow.FindControl("lblseason")
                    'lblseason.Text = excl(n)
                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try


    End Sub
    Private Sub filldates()

        Try

            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            strSqlQry = "select count( distinct fromdate) from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' and seasonname='" & txtseasonname.Text & "'" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            fillDategrd(grdDates, False, cnt)



            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            Dim lblseason As Label

            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' and seasonname='" & txtseasonname.Text & "' order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = myCommand.ExecuteReader



            For Each gvRow In grdDates.Rows

                dpFDate = gvRow.FindControl("txtfromdate")
                dpTDate = gvRow.FindControl("txttodate")
                lblseason = gvRow.FindControl("lblseason")


                If mySqlReader.Read = True Then

                    If IsDBNull(mySqlReader("todate")) = False Then
                        dpTDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                    End If

                    If IsDBNull(mySqlReader("fromdate")) = False Then

                        dpFDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                    End If
                    If IsDBNull(mySqlReader("SeasonName")) = False Then
                        lblseason.Text = mySqlReader("SeasonName")

                    End If


                End If
            Next




            For i As Integer = grdDates.Rows.Count - 1 To 1 Step -1
                Dim row As GridViewRow = grdDates.Rows(i)
                Dim previousRow As GridViewRow = grdDates.Rows(i - 1)
                Dim J As Integer = 1
                If row.Cells(J).Text = previousRow.Cells(J).Text Then
                    If previousRow.Cells(J).RowSpan = 0 Then
                        If row.Cells(J).RowSpan = 0 Then
                            previousRow.Cells(J).RowSpan += 2

                        Else
                            previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1

                        End If
                        row.Cells(J).Visible = False
                    End If
                End If
            Next

            mySqlReader.Close()
            mySqlConn.Close()


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            'clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            'clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try






        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'Finally

        '    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
        '    clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        'End Try
    End Sub





    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getseasonlist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim seasonlist As New List(Of String)
        Dim maxstate As String
        Try
            ' contextKey = Convert.ToString(HttpContext.Current.Viewstate("partycode").ToString())
            maxstate = Convert.ToString(HttpContext.Current.Session("ContractRefCode").ToString())


            strSqlQry = "select distinct SeasonName from view_contractseasons(nolock) where contractid='" & maxstate & "' and SeasonName like '" & Trim(prefixText) & "%' order by SeasonName "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    seasonlist.Add(myDS.Tables(0).Rows(i)("SeasonName").ToString())
                Next

            End If

            Return seasonlist
        Catch ex As Exception
            Return seasonlist
        End Try

    End Function



    Private Function ValidateSave() As Boolean





        If txtApplicableTo.Text = "" Or txtApplicableTo.Text = " " Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable To Should not be Blank');", True)
            ValidateSave = False
            SetFocus(txtApplicableTo)
            Exit Function
        End If




        Dim tickedornot As Boolean = False
        Dim chkSelect As CheckBox
        tickedornot = False
        For Each grdRow In grdseason.Rows
            chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)

            If chkSelect.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next


        For Each grdrow In grdDates.Rows
            Dim txtfromdate As TextBox = grdrow.findcontrol("txtfromdate")
            Dim txttodate As TextBox = grdrow.findcontrol("txttodate")

            If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                tickedornot = True
                Exit For
            End If

        Next

        If tickedornot = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Season or Enter Dates Manually');", True)
            ValidateSave = False
            Exit Function
        End If


        Dim seasonname As String = ""
        Dim chk2 As CheckBox
        Dim txtmealcode1 As Label
        Session("seasons") = Nothing
        Dim oldseason As String = ""

        For Each grdRow As GridViewRow In grdseason.Rows
            chk2 = grdRow.FindControl("chkseason")
            txtmealcode1 = grdRow.FindControl("txtseasoncode")

            If chk2.Checked = True And oldseason <> RTrim(LTrim(txtmealcode1.Text)) Then 'added rtrim & ltrim - changed by mohamed on 09/06/2018
                seasonname = seasonname + RTrim(LTrim(txtmealcode1.Text)) + ","
                oldseason = RTrim(LTrim(txtmealcode1.Text))
            End If
        Next

        If seasonname.Length > 0 Then
            seasonname = seasonname.Substring(0, seasonname.Length - 1)
        End If

        Session("seasons") = seasonname


        Dim rmcatchecked As Boolean = False
        For Each grdRow In grdrmcat.Rows
            chkSelect = CType(grdRow.FindControl("chkrmcat"), CheckBox)

            If chkSelect.Checked = True Then
                rmcatchecked = True
                Exit For
            End If
        Next

        If rmcatchecked = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Room category');", True)
            ValidateSave = False
            Exit Function
        End If

        rmcatchecked = False
        For Each grdRow In grdroomtype.Rows
            chkSelect = CType(grdRow.FindControl("chkrmtyp"), CheckBox)

            If chkSelect.Checked = True Then
                rmcatchecked = True
                Exit For
            End If
        Next
        If rmcatchecked = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Room Type');", True)
            ValidateSave = False
            Exit Function
        End If


        rmcatchecked = False
        For Each grdRow In grdmealplan.Rows
            chkSelect = CType(grdRow.FindControl("chkmeal"), CheckBox)

            If chkSelect.Checked = True Then
                rmcatchecked = True
                Exit For
            End If
        Next
        If rmcatchecked = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Meal Plan');", True)
            ValidateSave = False
            Exit Function
        End If


        Dim rmcatname As String = ""
        Dim rmtypname As String = ""
        Dim mealname As String = ""
        Dim chk3 As CheckBox
        Dim txtrmcatcode As Label
        Session("roomcategory") = Nothing
        Session("roomtypes") = Nothing
        Session("mealplans") = Nothing


        For Each grdRow As GridViewRow In grdrmcat.Rows
            chk3 = grdRow.FindControl("chkrmcat")
            txtrmcatcode = grdRow.FindControl("txtrmcatcode")

            If chk3.Checked = True Then
                rmcatname = rmcatname + txtrmcatcode.Text + ","

            End If
        Next

        If rmcatname.Length > 0 Then
            rmcatname = rmcatname.Substring(0, rmcatname.Length - 1)
        End If

        Session("roomcategory") = rmcatname


        For Each grdRow As GridViewRow In grdroomtype.Rows
            chk3 = grdRow.FindControl("chkrmtyp")
            txtrmcatcode = grdRow.FindControl("txtrmtypcode")

            If chk3.Checked = True Then
                rmtypname = rmtypname + txtrmcatcode.Text + ","

            End If
        Next

        If rmtypname.Length > 0 Then
            rmtypname = rmtypname.Substring(0, rmtypname.Length - 1)
        End If

        Session("roomtypes") = rmtypname


        For Each grdRow As GridViewRow In grdmealplan.Rows
            chk3 = grdRow.FindControl("chkmeal")
            txtrmcatcode = grdRow.FindControl("txtmealcode")

            If chk3.Checked = True Then
                mealname = mealname + txtrmcatcode.Text + ","

            End If
        Next

        If mealname.Length > 0 Then
            mealname = mealname.Substring(0, mealname.Length - 1)
        End If

        Session("mealplans") = mealname


        '------------Validate Commissionable 
        Dim formulacheck As Boolean = False
        Dim formula As String = ""
        Dim formulacount As Integer = 0


        For Each gvrow As GridViewRow In grdcommission.Rows

            Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
            Dim lblFormulaName As Label = gvrow.FindControl("lblFormulaName")
            Dim optcomm As RadioButton = gvrow.FindControl("optcomm")

            If optcomm.Checked = True Then
                formulacheck = True

                formulacount = formulacount + 1
            End If

        Next

        If formulacheck = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select the Commission Formula ');", True)
            ValidateSave = False
            Exit Function
        End If

        If formulacount > 1 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select the One Formula ');", True)
            ValidateSave = False
            Exit Function
        End If


        Dim str As String = ""
        For Each gvrow As GridViewRow In grdcommission.Rows

            Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
            Dim lblFormulaName As Label = gvrow.FindControl("lblFormula")
            Dim optcomm As RadioButton = gvrow.FindControl("optcomm")

            If optcomm.Checked = True Then
                str = lblFormulaName.Text

                str = str.Replace("+", " ")
                str = str.Replace("-", " ")
                str = str.Replace("*", " ")
                str = str.Replace("/", " ")
                str = str.Replace("=", " ")
                str = str.Replace("|", " ")

                For Each gvRow1 As GridViewRow In grdcommissiondetail.Rows

                    Dim txtperc As TextBox = gvRow1.FindControl("txtperc")
                    Dim lblterm1 As Label = gvRow1.FindControl("txtterm1")

                    If lblFormulaName.Text.Contains(" SR ") = True And lblterm1.Text.Contains("SR") = True And Val(txtperc.Text) = 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Formula Contains Service tax Please enter ServiceTax');", True)
                        ValidateSave = False
                        Exit Function
                    End If
                    If lblFormulaName.Text.Contains(" T ") = True And lblterm1.Text.Contains("T") = True And Val(txtperc.Text) = 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Formula Contains Service tax Please enter Tax');", True)
                        ValidateSave = False
                        Exit Function
                    End If

                    If lblFormulaName.Text.Contains(" C ") = True And lblterm1.Text.Contains("C") = True And Val(txtperc.Text) = 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Formula Contains Service tax Please enter Commission');", True)
                        ValidateSave = False
                        Exit Function
                    End If

                Next





            End If

        Next


        ''''''''''' Dates Overlapping

        Dim dtdatesnew As New DataTable
        Dim dsdates As New DataSet
        Dim dr As DataRow
        Dim xmldates As String = ""




        dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))


        For Each gvRow1 In grdDates.Rows
            Dim txtfromdate As TextBox = gvRow1.Findcontrol("txtfromdate")
            Dim txttodate As TextBox = gvRow1.Findcontrol("txttodate")

            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                dr = dtdatesnew.NewRow

                dr("fromdate") = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                dr("todate") = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                dtdatesnew.Rows.Add(dr)

            End If

        Next
        dsdates.Clear()
        If dtdatesnew IsNot Nothing Then
            If dtdatesnew.Rows.Count > 0 Then
                dsdates.Tables.Add(dtdatesnew)
                xmldates = objUtils.GenerateXML(dsdates)
            End If
        Else
            xmldates = "<NewDataSet />"
        End If

        Dim strMsg As String = ""
        Dim ds As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim parm(1) As SqlParameter

        parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))
        'parm(1) = New SqlParameter("@contractfromdate", DBNull.Value)
        'parm(2) = New SqlParameter("@contracttodate", DBNull.Value)

        parms.Add(parm(0))

        'For i = 0 To 2
        '    parms.Add(parm(i))
        'Next

        ds = New DataSet()
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates", parms)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
                    strMsg = "Dates Are Overlapping Please check " + "\n"
                    For i = 0 To ds.Tables(0).Rows.Count - 1

                        strMsg += "  Date -  " + ds.Tables(0).Rows(i)("fromdateC") + "\n"
                    Next

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                    ValidateSave = False
                    Exit Function
                End If
            End If
        End If


        '''''''''''''''''


        ''''''''''''''''''''''''''''''''''''''
        Dim ToDt As Date = Nothing
        Dim flgdt As Boolean = False

        For Each gvrow In grdDates.Rows
            Dim txtfromdate As TextBox = gvrow.Findcontrol("txtfromdate")
            Dim txttodate As TextBox = gvrow.Findcontrol("txttodate")



            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                If Left(Right(txtfromdate.Text, 4), 2) <> "20" Or Left(Right(txttodate.Text, 4), 2) <> "20" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  ');", True)
                    ValidateSave = False
                    SetFocus(txtfromdate)
                    Exit Function
                End If

                If seasonname <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Please Select Any One Season date or Manual dates ');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If Format(CType(txttodate.Text, Date), "yyyy/MM/dd") < Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date should be greater than From Date ');", True)
                    txttodate.Text = ""
                    SetFocus(txttodate)
                    ValidateSave = False
                    Exit Function
                End If

                If Session("Calledfrom") = "Offers" Then

                    'If Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value) Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & "  ');", True)
                    '    txtfromdate.Text = ""
                    '    SetFocus(txtfromdate)
                    '    ValidateSave = False
                    '    Exit Function
                    'End If
                    If Not (Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") >= Format(CType(hdnpromofrmdate.Value, Date), "yyyy/MM/dd") And Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") <= Format(CType(hdnpromotodate.Value, Date), "yyyy/MM/dd")) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                        '  txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        ValidateSave = False
                        Exit Function
                    End If

                    If Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") > Format(CType(hdnpromotodate.Value, Date), "yyyy/MM/dd") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                        txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        ValidateSave = False
                        Exit Function
                    End If

                    If (Format(CType(txttodate.Text, Date), "yyyy/MM/dd") > Format(CType(hdnpromotodate.Value, Date), "yyyy/MM/dd")) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                        txttodate.Text = ""
                        txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        ValidateSave = False
                        Exit Function
                    End If
                Else

                    If Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & "  ');", True)
                        txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        ValidateSave = False
                        Exit Function
                    End If
                    If Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") < Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnconfromdate.Value), Date), "yyyy/MM/dd") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & " ');", True)
                        txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        ValidateSave = False
                        Exit Function
                    End If

                    If (Format(CType(txttodate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value), Date), "yyyy/MM/dd")) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belong to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & " ');", True)
                        txttodate.Text = ""
                        txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        ValidateSave = False
                        Exit Function
                    End If

                End If
                'If ToDt <> Nothing Then
                '    'If ObjDate.ConvertDateromTextBoxToDatabase(txtSeasonfromDate.Text) <= ToDt Then
                '    If Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") <= ToDt Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Manual Dates Overlapping.');", True)
                '        SetFocus(txtfromdate)
                '        ValidateSave = False
                '        Exit Function
                '    End If
                'End If

                ToDt = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                flgdt = True

            Else
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date & To Date  Should not be Blank');", True)
                'SetFocus(txtfromdate)
                'ValidateSave = False
                'Exit Function
            End If


        Next




        ValidateSave = True
    End Function
    Private Sub ShowRecord(ByVal RefCode As String)

        Try




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_commission_header(nolock) Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("tranid")) = False Then
                        txtplistcode.Text = CType(mySqlReader("tranid"), String)
                    End If

                    If IsDBNull(mySqlReader("contractid")) = False And ViewState("CopyFrom") Is Nothing = True Then
                        hdncontractid.Value = CType(mySqlReader("contractid"), String)
                    End If
                    'If IsDBNull(mySqlReader("promotionid")) = False And ViewState("CopyFrom") Is Nothing = True Then
                    '    hdnpromotionid.Value = CType(mySqlReader("promotionid"), String)
                    'End If

                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",     ", ","), String)


                    End If

                    If IsDBNull(mySqlReader("computed")) = False Then
                        chkcomputed.Checked = IIf(CType(mySqlReader("computed"), String) = "1", True, False)
                    Else
                        chkcomputed.Checked = False
                    End If

                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_commission_header(nolock) where  tranid ='" & CType(RefCode, String) & "'") <> "" Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "UNAPPROVED"

                    Else
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "APPROVED"
                    End If

                End If
                Session("seasons") = ""
                If IsDBNull(mySqlReader("seasons")) = False Then
                    Session("seasons") = CType(RTrim(LTrim(mySqlReader("seasons"))), String)
                End If
                Session("roomcategory") = ""
                If IsDBNull(mySqlReader("roomcategory")) = False Then
                    Session("roomcategory") = CType(mySqlReader("roomcategory"), String)
                End If

                Session("roomtypes") = ""
                If IsDBNull(mySqlReader("roomtypes")) = False Then
                    Session("roomtypes") = CType(mySqlReader("roomtypes"), String)
                End If


                Session("mealplans") = ""
                If IsDBNull(mySqlReader("mealplans")) = False Then
                    Session("mealplans") = CType(mySqlReader("mealplans"), String)
                End If


                Dim strMealPlans As String = ""
                Dim strCondition As String = ""
                If Session("seasons") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                    strMealPlans = Session("seasons") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                    If strMealPlans.Length > 0 Then
                        Dim mString As String() = strMealPlans.Split(",")
                        For i As Integer = 0 To mString.Length - 1
                            If strCondition = "" Then
                                strCondition = "'" & mString(i) & "'"
                            Else
                                strCondition &= ",'" & mString(i) & "'"
                            End If
                        Next
                    End If
                End If


                Dim myDS As New DataSet
                grdseason.Visible = True
                strSqlQry = ""

                If Session("seasons") <> "" Then
                    strSqlQry = "select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected  from view_contractseasons(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname  Not IN (" & strCondition & ") " _
                     & " union all " _
                       & " select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,1 selected  from view_contractseasons  where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ")  order by  3 "



                Else
                    strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName " ' convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"
                End If

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS)
                grdseason.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then
                    grdseason.DataBind()


                Else
                    grdseason.DataBind()

                End If


                Dim chkSelect As CheckBox
                Dim lblselect As Label
                For Each grdRow In grdseason.Rows
                    chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                    lblselect = CType(grdRow.FindControl("lblselect"), Label)

                    If lblselect.Text = "1" Then
                        chkSelect.Checked = True
                        If ViewState("State") = "Copy" Then
                            chkSelect.Enabled = True
                            'Else
                            '    chkSelect.Enabled = False
                        End If

                    End If

                Next

                For i As Integer = grdseason.Rows.Count - 1 To 1 Step -1
                    Dim row As GridViewRow = grdseason.Rows(i)
                    Dim previousRow As GridViewRow = grdseason.Rows(i - 1)
                    Dim J As Integer = 2
                    Dim k As Integer = 1
                    If row.Cells(J).Text = previousRow.Cells(J).Text Then
                        If previousRow.Cells(J).RowSpan = 0 Then
                            If row.Cells(J).RowSpan = 0 Then
                                previousRow.Cells(J).RowSpan += 2
                                previousRow.Cells(k).RowSpan += 2

                            Else
                                previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1
                                previousRow.Cells(k).RowSpan = row.Cells(k).RowSpan + 1

                            End If
                            row.Cells(J).Visible = False
                            row.Cells(k).Visible = False
                        End If
                    End If
                Next



                Dim strrmcat As String = ""
                Dim strrmcatCondition As String = ""
                If Session("roomcategory") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                    strrmcat = Session("roomcategory") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                    If strrmcat.Length > 0 Then
                        Dim mString1 As String() = strrmcat.Split(",")
                        For i As Integer = 0 To mString1.Length - 1
                            If strrmcatCondition = "" Then
                                strrmcatCondition = "'" & mString1(i) & "'"
                            Else
                                strrmcatCondition &= ",'" & mString1(i) & "'"
                            End If
                        Next
                    End If
                End If


                Dim myDS1 As New DataSet
                grdrmcat.Visible = True
                strSqlQry = ""

                If Session("Calledfrom") = "Offers" Then

                    If Session("roomcategory") <> "" And ViewState("CopyFrom") <> "CopyFrom" Then
                        strSqlQry = "select * from  (select distinct prc.rmcatcode,1 selected , rc.rmcatname,isnull(rc.rankorder,999) rankorder  from partyrmcat prc,rmcatmast rc,view_contracts_commission_header h (nolock)  " _
                      & " cross apply dbo.splitallotmkt(h.roomcategory ,',') rm1  where prc.rmcatcode =rm1.mktcode and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A'  " _
                      & " and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode IN (" & strrmcatCondition & ")  and h.promotionid ='" & hdnpromotionid.Value & "') ts  order by rankorder"

                    ElseIf Session("roomcategory") <> "" And ViewState("CopyFrom") = "CopyFrom" Then

                        strSqlQry = " select * from  (select prc.rmcatcode,1 selected , rc.rmcatname,isnull(rc.rankorder,999) rankorder  from partyrmcat prc,rmcatmast rc,view_contracts_commission_header h (nolock)  " _
                    & " cross apply dbo.splitallotmkt(h.roomcategory ,',') rm1  where prc.rmcatcode =rm1.mktcode and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A'  " _
                    & " and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode IN (" & strrmcatCondition & ")  and h.promotionid ='" & hdncopypromotionid.Value & "') ts  order by rankorder"
                    Else

                        strSqlQry = "select prc.rmcatcode,0 selected , rc.rmcatname  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                       & hdnpartycode.Value & "'    order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

                    End If
                Else
                    If Session("roomcategory") <> "" And ViewState("CopyFrom") <> "CopyFrom" Then
                        strSqlQry = "select * from  (select distinct prc.rmcatcode,0 selected , rc.rmcatname,isnull(rc.rankorder,999) rankorder  from partyrmcat prc,rmcatmast rc ,view_contracts_commission_header h (nolock) where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.rmcatcode  Not IN (" & strrmcatCondition & ") and prc.partycode='" _
                      & hdnpartycode.Value & "' and h.contractid='" & hdncontractid.Value & "'  union all  select distinct prc.rmcatcode,1 selected , rc.rmcatname,isnull(rc.rankorder,999) rankorder  from partyrmcat prc,rmcatmast rc,view_contracts_commission_header h (nolock)  " _
                      & " cross apply dbo.splitallotmkt(h.roomcategory ,',') rm1  where prc.rmcatcode =rm1.mktcode and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A'  " _
                      & " and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode IN (" & strrmcatCondition & ")  and h.contractid ='" & hdncontractid.Value & "') ts  order by rankorder"

                    ElseIf Session("roomcategory") <> "" And ViewState("CopyFrom") = "CopyFrom" Then

                        strSqlQry = "select * from  (select distinct prc.rmcatcode,0 selected , rc.rmcatname,isnull(rc.rankorder,999) rankorder  from partyrmcat prc,rmcatmast rc ,view_contracts_commission_header h (nolock) where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.rmcatcode  Not IN (" & strrmcatCondition & ") and prc.partycode='" _
                    & hdnpartycode.Value & "' and h.contractid='" & hdncopycontractid.Value & "'  union all  select  distinct prc.rmcatcode,1 selected , rc.rmcatname,isnull(rc.rankorder,999) rankorder  from partyrmcat prc,rmcatmast rc,view_contracts_commission_header h (nolock)  " _
                    & " cross apply dbo.splitallotmkt(h.roomcategory ,',') rm1  where prc.rmcatcode =rm1.mktcode and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A'  " _
                    & " and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode IN (" & strrmcatCondition & ")  and h.contractid ='" & hdncopycontractid.Value & "') ts  order by rankorder"
                    Else

                        strSqlQry = "select prc.rmcatcode,0 selected , rc.rmcatname  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                       & hdnpartycode.Value & "'    order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

                    End If
                End If




                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS1)
                grdrmcat.DataSource = myDS1

                If myDS1.Tables(0).Rows.Count > 0 Then
                    grdrmcat.DataBind()
                Else
                    grdrmcat.DataBind()

                End If


                Dim chkSel As CheckBox
                Dim lblrmcat As Label
                For Each grdRow In grdrmcat.Rows
                    chkSel = CType(grdRow.FindControl("chkrmcat"), CheckBox)
                    lblrmcat = CType(grdRow.FindControl("lblselect"), Label)

                    If lblrmcat.Text = "1" Then
                        chkSel.Checked = True
                        If ViewState("State") = "Copy" Then
                            chkSel.Enabled = True

                        End If

                    End If

                Next


                Dim strrmtyp As String = ""
                Dim strrmtypcondition As String = ""
                If Session("roomtypes") Is Nothing = False Then
                    strrmtyp = Session("roomtypes")
                    If strrmtyp.Length > 0 Then
                        Dim mString2 As String() = strrmtyp.Split(",")
                        For i As Integer = 0 To mString2.Length - 1
                            If strrmtypcondition = "" Then
                                strrmtypcondition = "'" & mString2(i) & "'"
                            Else
                                strrmtypcondition &= ",'" & mString2(i) & "'"
                            End If
                        Next
                    End If
                End If


                Dim myDS2 As New DataSet
                grdroomtype.Visible = True
                strSqlQry = ""

                If Session("Calledfrom") = "Offers" Then
                    If Session("roomtypes") <> "" And ViewState("CopyFrom") <> "CopyFrom" Then

                        strSqlQry = "select * from  ( select  distinct prc.rmtypcode,1 selected , prc.rmtypname,isnull(prc.rankord,999) rankorder  from partyrmtyp prc,view_contracts_commission_header h (nolock)  " _
                      & " cross apply dbo.splitallotmkt(h.roomtypes ,',') rm1  where prc.rmtypcode =rm1.mktcode   " _
                      & " and prc.partycode='" & hdnpartycode.Value & "' and prc.inactive=0  and prc.rmtypcode IN (" & strrmtypcondition & ")  and h.promotionid ='" & hdnpromotionid.Value & "') ts  order by rankorder"

                    ElseIf Session("roomtypes") <> "" And ViewState("CopyFrom") = "CopyFrom" Then

                        strSqlQry = "select * from  (select distinct  prc.rmtypcode,1 selected , prc.rmtypname,isnull(prc.rankord,999) rankorder  from partyrmtyp prc(nolock),view_contracts_commission_header h (nolock)  " _
                    & " cross apply dbo.splitallotmkt(h.roomtypes ,',') rm1  where prc.rmtypcode =rm1.mktcode   " _
                    & " and prc.partycode='" & hdnpartycode.Value & "' and prc.inactive=0  and prc.rmtypcode IN (" & strrmtypcondition & ")  and h.promotionid ='" & hdncopypromotionid.Value & "') ts  order by rankorder"
                    Else
                        strSqlQry = "select rmtypcode,rmtypname,0 selected from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"

                        ' strSqlQry = "select prc.rmcatcode,0 selected , rc.rmcatname  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                        '& hdnpartycode.Value & "'    order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

                    End If

                Else
                    If Session("roomtypes") <> "" And ViewState("CopyFrom") <> "CopyFrom" Then

                        strSqlQry = "select * from  (select distinct prc.rmtypcode,0 selected , prc.rmtypname,isnull(prc.rankord,999) rankorder  from partyrmtyp prc(nolock),view_contracts_commission_header h (nolock) where prc.inactive=0  and   prc.rmtypcode  Not IN (" & strrmtypcondition & ") and prc.partycode='" _
                      & hdnpartycode.Value & "' and h.contractid='" & hdncontractid.Value & "'  union all  select  distinct prc.rmtypcode,1 selected , prc.rmtypname,isnull(prc.rankord,999) rankorder  from partyrmtyp prc,view_contracts_commission_header h (nolock)  " _
                      & " cross apply dbo.splitallotmkt(h.roomtypes ,',') rm1  where prc.rmtypcode =rm1.mktcode   " _
                      & " and prc.partycode='" & hdnpartycode.Value & "' and prc.inactive=0  and prc.rmtypcode IN (" & strrmtypcondition & ")  and h.contractid ='" & hdncontractid.Value & "') ts  order by rankorder"

                    ElseIf Session("roomtypes") <> "" And ViewState("CopyFrom") = "CopyFrom" Then

                        strSqlQry = "select * from  (select distinct prc.rmtypcode,0 selected , prc.rmtypname,isnull(prc.rankord,999) rankorder  from partyrmtyp prc(nolock),view_contracts_commission_header h (nolock) where prc.inactive=0  and prc.rmtypcode  Not IN (" & strrmtypcondition & ") and prc.partycode='" _
                    & hdnpartycode.Value & "' and h.contractid='" & hdncopycontractid.Value & "'  union all  select distinct  prc.rmtypcode,1 selected , prc.rmtypname,isnull(prc.rankord,999) rankorder  from partyrmtyp prc(nolock),view_contracts_commission_header h (nolock)  " _
                    & " cross apply dbo.splitallotmkt(h.roomtypes ,',') rm1  where prc.rmtypcode =rm1.mktcode   " _
                    & " and prc.partycode='" & hdnpartycode.Value & "' and prc.inactive=0  and prc.rmtypcode IN (" & strrmtypcondition & ")  and h.contractid ='" & hdncopycontractid.Value & "') ts  order by rankorder"
                    Else
                        strSqlQry = "select rmtypcode,rmtypname,0 selected from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"

                        ' strSqlQry = "select prc.rmcatcode,0 selected , rc.rmcatname  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                        '& hdnpartycode.Value & "'    order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

                    End If
                End If





                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS2)
                grdroomtype.DataSource = myDS2

                If myDS2.Tables(0).Rows.Count > 0 Then
                    grdroomtype.DataBind()


                Else
                    grdroomtype.DataBind()

                End If

                Dim chkSelrm As CheckBox
                Dim lblrmtmyp As Label
                For Each grdRow In grdroomtype.Rows
                    chkSelrm = CType(grdRow.FindControl("chkrmtyp"), CheckBox)
                    lblrmtmyp = CType(grdRow.FindControl("lblselect"), Label)

                    If lblrmtmyp.Text = "1" Then
                        chkSelrm.Checked = True
                        If ViewState("State") = "Copy" Then
                            chkSelrm.Enabled = True

                        End If

                    End If

                Next

                Dim strmeal As String = ""
                Dim strmealcondition As String = ""
                If Session("mealplans") Is Nothing = False Then
                    strmeal = Session("mealplans")
                    If strmeal.Length > 0 Then
                        Dim mString3 As String() = strmeal.Split(",")
                        For i As Integer = 0 To mString3.Length - 1
                            If strmealcondition = "" Then
                                strmealcondition = "'" & mString3(i) & "'"
                            Else
                                strmealcondition &= ",'" & mString3(i) & "'"
                            End If
                        Next
                    End If
                End If


                Dim myDS3 As New DataSet
                grdmealplan.Visible = True
                strSqlQry = ""

                If Session("Calledfrom") = "Offers" Then
                    If Session("mealplans") <> "" And ViewState("CopyFrom") <> "CopyFrom" Then
                        strSqlQry = "select * from  (select distinct  prc.mealcode,1 selected , rc.mealname,isnull(rc.rankorder,999) rankorder  from partymeal prc,mealmast rc,view_contracts_commission_header h (nolock)  " _
                      & " cross apply dbo.splitallotmkt(h.mealplans ,',') rm1  where prc.mealcode =rm1.mktcode and prc.mealcode=rc.mealcode   " _
                      & " and prc.partycode='" & hdnpartycode.Value & "' and prc.mealcode IN (" & strmealcondition & ")  and h.promotionid ='" & hdnpromotionid.Value & "') ts  order by rankorder"

                    ElseIf Session("mealplans") <> "" And ViewState("CopyFrom") = "CopyFrom" Then

                        strSqlQry = "select * from  (select distinct  prc.mealcode,1 selected , rc.mealname,isnull(rc.rankorder,999) rankorder  from partymeal prc,mealmast rc,view_contracts_commission_header h (nolock)  " _
                    & " cross apply dbo.splitallotmkt(h.mealplans ,',') rm1  where prc.mealcode =rm1.mktcode and prc.mealcode=rc.mealcode   " _
                    & " and prc.partycode='" & hdnpartycode.Value & "' and prc.mealcode IN (" & strmealcondition & ")  and h.promotionid ='" & hdncopypromotionid.Value & "') ts  order by rankorder"
                    Else

                        strSqlQry = "select p.mealcode as mealcode,m.mealname,0 selected  from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"

                    End If
                Else
                    If Session("mealplans") <> "" And ViewState("CopyFrom") <> "CopyFrom" Then
                        strSqlQry = "select * from  (select distinct prc.mealcode,0 selected , rc.mealname,isnull(rc.rankorder,999) rankorder  from partymeal prc,mealmast rc ,view_contracts_commission_header h (nolock) where prc.mealcode=rc.mealcode  and prc.mealcode  Not IN (" & strmealcondition & ") and prc.partycode='" _
                      & hdnpartycode.Value & "' and h.contractid='" & hdncontractid.Value & "'  union all   select distinct  prc.mealcode,1 selected , rc.mealname,isnull(rc.rankorder,999) rankorder  from partymeal prc,mealmast rc,view_contracts_commission_header h (nolock)  " _
                      & " cross apply dbo.splitallotmkt(h.mealplans ,',') rm1  where prc.mealcode =rm1.mktcode and prc.mealcode=rc.mealcode   " _
                      & " and prc.partycode='" & hdnpartycode.Value & "' and prc.mealcode IN (" & strmealcondition & ")  and h.contractid ='" & hdncontractid.Value & "') ts  order by rankorder"

                    ElseIf Session("mealplans") <> "" And ViewState("CopyFrom") = "CopyFrom" Then

                        strSqlQry = "select * from  (select distinct prc.mealcode,0 selected , rc.mealname,isnull(rc.rankorder,999) rankorder  from partymeal prc,mealmast rc ,view_contracts_commission_header h (nolock) where prc.mealcode=rc.mealcode  and prc.mealcode  Not IN (" & strmealcondition & ") and prc.partycode='" _
                    & hdnpartycode.Value & "' and h.contractid='" & hdncopycontractid.Value & "'  union all  select distinct  prc.mealcode,1 selected , rc.mealname,isnull(rc.rankorder,999) rankorder  from partymeal prc,mealmast rc,view_contracts_commission_header h (nolock)  " _
                    & " cross apply dbo.splitallotmkt(h.mealplans ,',') rm1  where prc.mealcode =rm1.mktcode and prc.mealcode=rc.mealcode   " _
                    & " and prc.partycode='" & hdnpartycode.Value & "' and prc.mealcode IN (" & strmealcondition & ")  and h.contractid ='" & hdncopycontractid.Value & "') ts  order by rankorder"
                    Else

                        strSqlQry = "select p.mealcode as mealcode,m.mealname,0 selected  from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"

                    End If
                End If



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS3)
                grdmealplan.DataSource = myDS3

                If myDS3.Tables(0).Rows.Count > 0 Then
                    grdmealplan.DataBind()


                Else
                    grdmealplan.DataBind()

                End If

                Dim chkSelmeal As CheckBox
                Dim lblmeal As Label
                For Each grdRow In grdmealplan.Rows
                    chkSelmeal = CType(grdRow.FindControl("chkmeal"), CheckBox)
                    lblmeal = CType(grdRow.FindControl("lblselect"), Label)

                    If lblmeal.Text = "1" Then
                        chkSelmeal.Checked = True
                        If ViewState("State") = "Copy" Then
                            chkSelmeal.Enabled = True

                        End If

                    End If

                Next





            End If


            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (hdnpartycode.Value.Trim <> "") Then
            Dim myDataAdapter As SqlDataAdapter
            grdpromotion.Visible = True


            Dim MyDs As New DataTable
            Dim countryList As String = ""
            Dim agentList As String = ""
            Dim filterCond As String = ""
            If wucCountrygroup.checkcountrylist.ToString().Trim <> "" Then
                countryList = wucCountrygroup.checkcountrylist.ToString().Trim.Replace(",", "','")
                filterCond = "h.promotionid  in (select promotionid from view_offers_countries where ctrycode in (' " + countryList + "'))"
            End If
            If wucCountrygroup.checkagentlist.ToString().Trim <> "" Then
                agentList = wucCountrygroup.checkagentlist.ToString().Trim.Replace(",", "','")
                If filterCond <> "" Then
                    filterCond = filterCond + " or h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                Else
                    filterCond = "h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                End If
            End If
            If filterCond <> "" Then
                filterCond = " and (" + filterCond + ")"
            End If
            filterCond = ""
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = " select c.tranid plistcode, h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_contracts_commission_header c(nolock)  where isnull(h.active,0)=0 and h.promotionid=c.promotionid and  isnull(h.commissiontype,'') ='Special commissionable Rates'  and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  h.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by h.promotionid,h.approved,h.promotionname,c.tranid order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

            'strSqlQry = " select h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,  " _
            '    & " case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status from view_offers_header h(nolock),view_offers_detail d(nolock) where isnull(h.commissiontype,'')='Special commissionable Rates' and  h.promotionid<>'" & hdnpromotionid.Value & "' and  h.promotionid =d.promotionid   " _
            '    & " and h.partycode='" + hdnpartycode.Value.Trim + "'  " + filterCond + " group by h.promotionid,h.approved  order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(MyDs)
            If MyDs.Rows.Count > 0 Then
                grdpromotion.DataSource = MyDs
                grdpromotion.DataBind()
                grdpromotion.Visible = True
            Else
                grdpromotion.Visible = False
            End If

            ModalExtraPopup1.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Hotel Name' );", True)
            Exit Sub
        End If
    End Sub
    Private Sub DisableControl()
        If ViewState("State") = "New" Or ViewState("State") = "Copy" Then




            txtApplicableTo.Enabled = True
            txtplistcode.Text = ""



            wucCountrygroup.Disable(True)
            txtseasonname.Enabled = True
            grdDates.Enabled = True
            grdcommissiondetail.Enabled = True
            grdcommission.Enabled = True
            grdseason.Enabled = True
            grdrmcat.Enabled = True
            grdroomtype.Enabled = True
            grdmealplan.Enabled = True

        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then



            wucCountrygroup.Disable(False)
            txtApplicableTo.Enabled = False

            txtpolicy.Enabled = False

            grdDates.Enabled = False
            grdcommissiondetail.Enabled = False
            grdcommission.Enabled = False
            txtseasonname.Enabled = False
            grdseason.Enabled = False
            grdrmcat.Enabled = False
            grdroomtype.Enabled = False
            grdmealplan.Enabled = False
        ElseIf ViewState("State") = "Edit" Then

            'dpFromDate.Enabled = True
            'dpToDate.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            txtpolicy.Enabled = True

            grdDates.Enabled = True
            grdcommissiondetail.Enabled = True
            grdcommission.Enabled = True
            txtseasonname.Enabled = False
            grdseason.Enabled = True
            grdrmcat.Enabled = True
            grdroomtype.Enabled = True
            grdmealplan.Enabled = True
        End If
    End Sub
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()
        ' FillGrid(hdncontractid.Value, hdnpartycode.Value, "Desc")


    End Sub
    Public Function checkforexisting() As Boolean

        checkforexisting = True
        Try
            If FindDatePeriod() = False Then
                checkforexisting = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

#End Region

    Public Function FindDatePeriod() As Boolean



        Dim strMsg As String = ""

        FindDatePeriod = True
        Try

            '   CopyRow = 0

            Dim weekdaystr As String = ""

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist



            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            For Each GVRow In grdseason.Rows

                'Dim txtfromdate As TextBox = GVRow.Findcontrol("txtfromdate")
                'Dim txttodate As TextBox = GVRow.Findcontrol("txttodate")
                'Dim lblseason As Label = GVRow.Findcontrol("lblseason")

                Dim txtmealcode1 As Label = GVRow.FindControl("txtseasoncode")
                Dim chkseason As CheckBox = GVRow.findcontrol("chkseason")

                If chkseason.Checked = True Then

                    Dim ds As DataSet
                    Dim parms2 As New List(Of SqlParameter)
                    Dim parm2(12) As SqlParameter

                    parm2(0) = New SqlParameter("@contractid", IIf(Session("Calledfrom") = "Offers", "", CType(hdncontractid.Value, String)))
                    parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm2(2) = New SqlParameter("@fromdate", Format(CType(GVRow.Cells(3).Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@todate", Format(CType(GVRow.Cells(4).Text, Date), "yyyy/MM/dd"))
                    parm2(4) = New SqlParameter("@season", CType(txtmealcode1.Text, String))
                    parm2(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm2(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(8) = New SqlParameter("@promotionid", IIf(Session("Calledfrom") = "Offers", CType(hdnpromotionid.Value, String), ""))
                    parm2(9) = New SqlParameter("@roomcategory", CType(Replace(Session("roomcategory"), ",  ", ","), String))
                    parm2(10) = New SqlParameter("@roomtypes", CType(Replace(Session("roomtypes"), ",  ", ","), String))
                    parm2(11) = New SqlParameter("@mealplans", CType(Replace(Session("mealplans"), ",  ", ","), String))

                    For i = 0 To 11
                        parms2.Add(parm2(i))
                    Next



                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcommission", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("tranid")) = False Then
                                If Session("Calledfrom") = "Offers" Then
                                    strMsg = "Commission already exists For this  Season   " + CType(hdnpromotionid.Value, String) + " - Policy Id " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                Else
                                    strMsg = "Commission already exists For this  Season   " + CType(hdncontractid.Value, String) + " - Policy Id " + ds.Tables(0).Rows(0)("tranid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                End If

                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                FindDatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If
                End If

            Next


            For Each GVRow In grdDates.Rows

                Dim txtfromdate As TextBox = GVRow.Findcontrol("txtfromdate")
                Dim txttodate As TextBox = GVRow.Findcontrol("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    Dim ds1 As DataSet
                    Dim parms3 As New List(Of SqlParameter)
                    Dim parm3(11) As SqlParameter

                    parm3(0) = New SqlParameter("@contractid", IIf(Session("Calledfrom") = "Offers", "", CType(hdncontractid.Value, String)))
                    parm3(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm3(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm3(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm3(4) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm3(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm3(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm3(7) = New SqlParameter("@promotionid", CType(hdnpromotionid.Value, String))
                    parm3(8) = New SqlParameter("@roomcategory", CType(Replace(Session("roomcategory"), ",  ", ","), String))
                    parm3(9) = New SqlParameter("@roomtypes", CType(Replace(Session("roomtypes"), ",  ", ","), String))
                    parm3(10) = New SqlParameter("@mealplans", CType(Replace(Session("mealplans"), ",  ", ","), String))


                    For i = 0 To 10
                        parms3.Add(parm3(i))
                    Next

                    ds1 = New DataSet()
                    ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcommission_manual", parms3)


                    If ds1.Tables.Count > 0 Then
                        If ds1.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds1.Tables(0).Rows(0)("tranid")) = False Then
                                If Session("Calledfrom") = "Offers" Then
                                    strMsg = "Commission already exists For this  Dates   " + CType(hdnpromotionid.Value, String) + " - Policy Id " + ds1.Tables(0).Rows(0)("tranid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")
                                Else
                                    strMsg = "Commission already exists For this  Dates   " + CType(hdncontractid.Value, String) + " - Policy Id " + ds1.Tables(0).Rows(0)("tranid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")
                                End If

                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                FindDatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If


                End If

            Next



        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Private Sub FillPromotionDates(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromdate")
                        dpTDate = gvRow.FindControl("txttodate")
                        '     Dim lblseason As Label = gvRow.FindControl("lblseason")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("fromdate")) = False Then
                                dpFDate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If
            '  txtseasonname.Enabled = False


            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(d.fromdate),103) fromdate, convert(varchar(10),max(d.todate),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & RefCode & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                hdnpromofrmdate.Value = ds.Tables(0).Rows(0).Item("fromdate")
                hdnpromotodate.Value = ds.Tables(0).Rows(0).Item("todate")
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Private Sub ShowDatesnew(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            Else
                strSqlQry = "select count( distinct fromdate) from view_contracts_commission_detail(nolock) where tranid='" & RefCode & "'" '" ' and subseasnname = '" & subseasonname & "'"
            End If




            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open  

            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)
            Else
                mySqlCmd = New SqlCommand("Select * from view_contracts_commission_detail(nolock) Where tranid='" & RefCode & "'", mySqlConn)

            End If

            ' mySqlCmd = New SqlCommand("Select * from view_contracts_commission_detail(nolock) Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromdate")
                        dpTDate = gvRow.FindControl("txttodate")
                        '     Dim lblseason As Label = gvRow.FindControl("lblseason")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("fromdate")) = False Then
                                dpFDate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                            End If
                            'If IsDBNull(mySqlReader("seasonname")) = False Then
                            '    lblseason.Text = CType(mySqlReader("seasonname"), String)
                            '    txtseasonname.Text = CType(mySqlReader("seasonname"), String)
                            'End If
                            Exit For
                        End If
                    Next
                End While
            End If
            '  txtseasonname.Enabled = False


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Private Sub ShowDates(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            strSqlQry = "select count( distinct fromdate) from view_contracts_commission_detail_manual(nolock) where tranid='" & RefCode & "'" '" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            mySqlCmd = New SqlCommand("Select * from view_contracts_commission_detail_manual(nolock) Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromdate")
                        dpTDate = gvRow.FindControl("txttodate")
                        '     Dim lblseason As Label = gvRow.FindControl("lblseason")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("fromdate")) = False Then
                                dpFDate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                            End If
                            'If IsDBNull(mySqlReader("seasonname")) = False Then
                            '    lblseason.Text = CType(mySqlReader("seasonname"), String)
                            '    txtseasonname.Text = CType(mySqlReader("seasonname"), String)
                            'End If
                            Exit For
                        End If
                    Next
                End While
            End If
            '  txtseasonname.Enabled = False


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If

            Dim lbltran As Label
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltranid")
            If lbltran.Text.Trim = "" Then Exit Sub

            If e.CommandName <> "View" Then

                If Session("Calledfrom") = "Offers" Then
                    Dim offerexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_offers_header(nolock) where promotionid='" & hdnpromotionid.Value & "'")

                    If offerexists Is Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Offer Main Details First');", True)
                        Exit Sub

                    End If
                Else
                    Dim contexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_contracts(nolock) where contractid='" & hdncontractid.Value & "'")

                    If contexists Is Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Contract Main Details First');", True)
                        Exit Sub

                    End If
                End If
            End If


            If e.CommandName = "EditRow" Then
                ViewState("State") = "Edit"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                FillTerms()
                FillFormula(lbltran.Text)

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()

                btnSave.Visible = True
                btnSave.Text = "Update"
                lblHeading.Text = "Edit Commission - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Commission  "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Edit Commission - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                    Page.Title = "Promotion Commission "
                End If
            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                FillTerms()
                FillFormula(lbltran.Text)

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = False
                lblHeading.Text = "View Commisiion - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Commission "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "View Commission - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                    Page.Title = "Promotion Commission "
                End If

            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Visible = True
                ViewState("State") = "Delete"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                FillTerms()
                FillFormula(lbltran.Text)

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                lblHeading.Text = "Delete Commission - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Commission "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Delete Commission - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                    Page.Title = "Promotion Commission "
                End If
            ElseIf e.CommandName = "Copy" Then
                PanelMain.Visible = True
                ViewState("State") = "Copy"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                FillTerms()
                FillFormula(lbltran.Text)
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy Commission - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Commission "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Copy Commission - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                    Page.Title = "Promotion Commission "
                End If
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub grdviewrates_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdviewrates.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lbltran As Label
            Dim lblcontract As Label, lblpromotionid As Label
            lbltran = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            lblcontract = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcontract")
            lblpromotionid = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionid")
            If lbltran.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then
                If Session("Calledfrom") = "Offers" Then
                    hdncopypromotionid.Value = CType(lblpromotionid.Text, String)
                Else
                    hdncopycontractid.Value = CType(lblcontract.Text, String)
                End If

                PanelMain.Visible = True
                ViewState("CopyFrom") = "CopyFrom"
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                PanelMain.Style("display") = "block"

                ' wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))


                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                FillTerms()
                FillFormula(CType(lbltran.Text.Trim, String))

                If Session("Calledfrom") = "Offers" Then
                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()
                    fillpromotiondetails(hdnpromotionid.Value)
                    FillPromotionDates(hdnpromotionid.Value)

                    btnSave.Visible = True
                    txtplistcode.Text = ""
                    btnSave.Text = "Save"
                    lblHeading.Text = "Copy Commission - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Commission "
                    fillseason()
                    DisableControl()

                Else
                    wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                    wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()

                    btnSave.Visible = True
                    txtplistcode.Text = ""
                    btnSave.Text = "Save"
                    lblHeading.Text = "Copy Commission - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Commission "
                    fillseason()
                    DisableControl()
                End If



            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try


            If Session("Calledfrom") = "Offers" Then


                grdviewrates.Columns(5).Visible = False
                grdviewrates.Columns(6).Visible = False

                '    strSqlQry = " select '' contractid,h.tranid plistcode,'' season,isnull(h.promotionid,'') promotionid, p.promotionname, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
                '& "   from view_contracts_commission_header h(nolock),view_offers_header p (nolock),view_offers_detail d(nolock)  where  p.commissiontype ='Special commissionable Rates'  and  p.promotionid =d.promotionid   and h.promotionid= d.promotionid and p.partycode='" & hdnpartycode.Value & "' and  h.promotionid<>'" & hdnpromotionid.Value & "'  group by h.promotionid,p.promotionname,h.tranid,h.applicableto  "


                strSqlQry = "select h.contractid as contractid,h.tranid plistcode,h.seasons season, '' promotionid,'' promotionname,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
            " h.applicableto    from view_contracts_commission_header h(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and    h.contractid<>'" & hdncontractid.Value & "' and h.seasons <>'' union all " _
            & " select h.contractid as contractid,h.tranid plistcode,'' season,'' promotionid,'' promotionname, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
            & "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and  h.tranid=d.tranid and     h.contractid<>'" & hdncontractid.Value & "' and h.seasons ='' group by h.contractid, h.tranid,h.applicableto  "





            Else

                'grdviewrates.Columns(2).Visible = True
                'grdviewrates.Columns(4).Visible = True
                'grdviewrates.Columns(5).Visible = False
                grdviewrates.Columns(5).Visible = False
                grdviewrates.Columns(6).Visible = False

                strSqlQry = "select h.contractid as contractid,h.tranid plistcode,h.seasons season, '' promotionid,'' promotionname,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
            " h.applicableto    from view_contracts_commission_header h(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and    h.contractid<>'" & hdncontractid.Value & "' and h.seasons <>'' union all " _
            & " select h.contractid as contractid,h.tranid plistcode,'' season,'' promotionid,'' promotionname, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
            & "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and  h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and  h.tranid=d.tranid and     h.contractid<>'" & hdncontractid.Value & "' and h.seasons ='' group by h.contractid, h.tranid,h.applicableto  "


            End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myds)
            grdviewrates.DataSource = myds

            If myds.Tables(0).Rows.Count > 0 Then
                grdviewrates.DataBind()
            Else
                grdviewrates.PageIndex = 0
                grdviewrates.DataBind()

            End If


            ModalViewrates.Show()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            Dim strMsg As String = ""
            If Page.IsValid = True Then

                If ViewState("State") = "New" Or ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then

                    If ValidateSave() = False Then
                        Exit Sub
                    End If
                    If checkforexisting() = False Then
                        Exit Sub
                    End If

                    If chkctrygrp.Checked = True And Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_commission", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'If Session("Calledfrom") = "Offers" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                    'Else
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                    'End If


                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    '''''''''''''''''''''''


                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("COMMISSION", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        'command disposed




                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_commission_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = IIf(txtApplicableTo.Text = "", "", CType(Replace(txtApplicableTo.Text, ",  ", ","), String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)

                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(Session("seasons") = "", "", CType(Replace(Session("seasons"), ",  ", ","), String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@roomcategory", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("roomcategory"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("roomtypes"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("mealplans"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@computed", SqlDbType.Int)).Value = IIf(chkcomputed.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                'command disposed

                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_contracts_commission_header ", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)

                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(Session("seasons") = "", "", CType(Replace(Session("seasons"), ",  ", ","), String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@roomcategory", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("roomcategory"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("roomtypes"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("mealplans"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@computed", SqlDbType.Int)).Value = IIf(chkcomputed.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If



                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_Commission  Where Commission_ID='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM new_edit_commissionroom Where Commission_ID='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    'If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                    '    For i = 0 To arrcountry.Length - 1

                    '        If arrcountry(i) <> "" Then


                    '            mySqlCmd = New SqlCommand("sp_add_contracts_commission_countries", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    'End If

                    'If wucCountrygroup.checkagentlist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For i = 0 To arragents.Length - 1

                    '        If arragents(i) <> "" Then

                    '            mySqlCmd = New SqlCommand("sp_add_contracts_commission_agents", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next
                    'End If

                    '''' save season  dates
                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_commission_detail Where tranid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()


                    Dim formulastring As String
                    Dim formulacode As String
                    formulastring = ""
                    For Each gvrow As GridViewRow In grdcommission.Rows


                        Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
                        Dim txtformulacode As Label = gvrow.FindControl("txtformulacode")
                        Dim optcomm As RadioButton = gvrow.FindControl("optcomm")


                        If optcomm.Checked = True Then
                            Dim i As Integer = 1
                            formulacode = txtformulacode.Text
                            For Each gridRow As GridViewRow In grdcommissiondetail.Rows

                                Dim txtperc As TextBox = gridRow.FindControl("txtperc")
                                Dim txtterm1 As Label = gridRow.FindControl("txtterm1")

                                If Val(txtperc.Text) <> 0 Then

                                    formulastring = formulastring + CType(txtterm1.Text.Trim, String) + "," + CType(Val(txtperc.Text.Trim), String) + ";"
                                End If
                            Next

                        End If

                    Next


                    Dim seasonname As String()

                    ' seasonname = Session("seasons") ' IIf(Session("seasons") = "", "", CType(Replace(Session("seasons"), ",  ", ","), String))

                    seasonname = Session("seasons").ToString.Trim.Split(",")

                    Dim chk2 As CheckBox
                    Dim txtmealcode1 As Label

                  

                    If Session("CountryList") <> "" Then

                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then

                                For Each grdRow As GridViewRow In grdseason.Rows
                                    chk2 = grdRow.FindControl("chkseason")
                                    txtmealcode1 = grdRow.FindControl("txtseasoncode")

                                    If chk2.Checked = True Then

                                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_commission_detail", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@formulastring", SqlDbType.VarChar, 1000)).Value = CType(formulastring.ToString.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@formulaid", SqlDbType.VarChar, 20)).Value = CType(formulacode.ToString.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = ""
                                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = ""
                                        mySqlCmd.Parameters.Add(New SqlParameter("@CCode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)

                                        mySqlCmd.Parameters.Add(New SqlParameter("@computed", SqlDbType.Int)).Value = IIf(chkcomputed.Checked = True, 1, 0)

                                        mySqlCmd.ExecuteNonQuery()
                                        mySqlCmd.Dispose() 'command disposed

                                    End If

                                Next
                            End If
                        Next
                    End If
                    If Session("AgentList") <> "" Then
                        Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                        For i = 0 To arragents.Length - 1

                            If arragents(i) <> "" Then

                                For Each grdRow As GridViewRow In grdseason.Rows
                                    chk2 = grdRow.FindControl("chkseason")
                                    txtmealcode1 = grdRow.FindControl("txtseasoncode")

                                    If chk2.Checked = True Then

                                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_commission_detail", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@formulastring", SqlDbType.VarChar, 1000)).Value = CType(formulastring.ToString.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@formulaid", SqlDbType.VarChar, 20)).Value = CType(formulacode.ToString.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(i), String) & "'")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@CCode", SqlDbType.VarChar, 20)).Value = ""


                                        mySqlCmd.Parameters.Add(New SqlParameter("@computed", SqlDbType.Int)).Value = IIf(chkcomputed.Checked = True, 1, 0)

                                        mySqlCmd.ExecuteNonQuery()
                                        mySqlCmd.Dispose() 'command disposed

                                    End If

                                Next




                            End If
                        Next

                    End If

                    If Session("seasons") = "" Then

                        Dim k As Integer = 1

                        If Session("CountryList") <> "" Then

                            Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                            For i = 0 To arrcountry.Length - 1

                                If arrcountry(i) <> "" Then

                                    For Each gvrow3 As GridViewRow In grdDates.Rows
                                        Dim txtfromdate As TextBox = gvrow3.FindControl("txtfromdate")
                                        Dim txttodate As TextBox = gvrow3.FindControl("txttodate")
                                        ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                                        If txtfromdate.Text <> "" And txttodate.Text <> "" Then


                                            mySqlCmd = New SqlCommand("sp_add_edit_contracts_commission_detail", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure

                                            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual1", String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                                            mySqlCmd.Parameters.Add(New SqlParameter("@formulastring", SqlDbType.VarChar, 1000)).Value = CType(formulastring.ToString.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@formulaid", SqlDbType.VarChar, 20)).Value = CType(formulacode.ToString.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = ""
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = ""
                                            mySqlCmd.Parameters.Add(New SqlParameter("@CCode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@computed", SqlDbType.Int)).Value = IIf(chkcomputed.Checked = True, 1, 0)

                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed

                                            k = k + 1
                                        End If
                                    Next
                                End If
                            Next
                        End If

                        If Session("AgentList") <> "" Then
                            Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                            For i = 0 To arragents.Length - 1

                                If arragents(i) <> "" Then


                                    For Each gvrow3 As GridViewRow In grdDates.Rows
                                        Dim txtfromdate As TextBox = gvrow3.FindControl("txtfromdate")
                                        Dim txttodate As TextBox = gvrow3.FindControl("txttodate")
                                        ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                                        If txtfromdate.Text <> "" And txttodate.Text <> "" Then


                                            mySqlCmd = New SqlCommand("sp_add_edit_contracts_commission_detail", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure

                                            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual1", String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                                            mySqlCmd.Parameters.Add(New SqlParameter("@formulastring", SqlDbType.VarChar, 1000)).Value = CType(formulastring.ToString.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@formulaid", SqlDbType.VarChar, 20)).Value = CType(formulacode.ToString.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(i), String) & "'")

                                            mySqlCmd.Parameters.Add(New SqlParameter("@CCode", SqlDbType.VarChar, 20)).Value = ""
                                            mySqlCmd.Parameters.Add(New SqlParameter("@computed", SqlDbType.Int)).Value = IIf(chkcomputed.Checked = True, 1, 0)

                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed

                                            k = k + 1
                                        End If
                                    Next


                                End If

                            Next
                        End If



                    End If


                    Dim gvrow5 As GridViewRow
                    Dim gvrow1 As GridViewRow
                    Dim chkselect As CheckBox
                    Dim chk3 As CheckBox
                    Dim chk4 As CheckBox


                    If Session("seasons") <> "" Then
                        For j = 0 To seasonname.Length - 1

                            If seasonname(j) <> "" Then

                                For Each grdRow In grdroomtype.Rows
                                    chkselect = CType(grdRow.FindControl("chkrmtyp"), CheckBox)
                                    Dim txtrmtypcode As Label = grdRow.FindControl("txtrmtypcode")
                                    If chkselect.Checked = True Then


                                        For Each gvrow5 In grdmealplan.Rows
                                            chk3 = CType(gvrow5.FindControl("chkmeal"), CheckBox)
                                            Dim txtmealcode As Label = gvrow5.FindControl("txtmealcode")

                                            If chk3.Checked = True Then

                                                For Each gvrow1 In grdrmcat.Rows
                                                    chk4 = CType(gvrow1.FindControl("chkrmcat"), CheckBox)
                                                    Dim txtrmcatcode As Label = gvrow1.FindControl("txtrmcatcode")

                                                    mySqlCmd = New SqlCommand("sp_add_new_commissionroom", mySqlConn, sqlTrans)
                                                    mySqlCmd.CommandType = CommandType.StoredProcedure


                                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(seasonname(j), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@roomtypename", SqlDbType.VarChar, 200)).Value = CType(txtrmtypcode.Text, String) ' CType(grdRow.Cells(2).Text, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(txtrmcatcode.Text, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(txtmealcode.Text, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                                    mySqlCmd.ExecuteNonQuery()
                                                    mySqlCmd.Dispose() 'command disposed
                                                Next


                                            End If


                                        Next

                                    End If
                                Next

                            End If
                        Next
                    Else

                        For Each grdRow In grdroomtype.Rows
                            chkselect = CType(grdRow.FindControl("chkrmtyp"), CheckBox)
                            Dim txtrmtypcode As Label = grdRow.FindControl("txtrmtypcode")
                            If chkselect.Checked = True Then


                                For Each gvrow In grdmealplan.Rows
                                    chk3 = CType(gvrow.FindControl("chkmeal"), CheckBox)
                                    Dim txtmealcode As Label = gvrow.FindControl("txtmealcode")

                                    If chk3.Checked = True Then

                                        For Each gvrow1 In grdrmcat.Rows
                                            chk4 = CType(gvrow1.FindControl("chkrmcat"), CheckBox)
                                            Dim txtrmcatcode As Label = gvrow1.FindControl("txtrmcatcode")

                                            mySqlCmd = New SqlCommand("sp_add_new_commissionroom", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure


                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual1", String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypename", SqlDbType.VarChar, 200)).Value = CType(txtrmtypcode.Text, String) 'CType(grdRow.Cells(2).Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(txtrmcatcode.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(txtmealcode.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text, String)
                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                        Next


                                    End If


                                Next

                            End If
                        Next


                    End If






                    ''''''''''


                    ''''''' to save formula

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_commissions Where tranid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()




                    '''''''''''




                    strMsg = "Saved Succesfully!!"
                ElseIf ViewState("State") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_commission", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'If Session("Calledfrom") = "Offers" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                    'Else
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                    'End If


                    'mySqlCmd.ExecuteNonQuery()
                    '   mySqlCmd.Dispose()
                    '''''''''''''''''''''''



                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_contracts_commission_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.CommandTimeout = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    strMsg = "Deleted  Succesfully!!"


                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed

                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close


                ViewState("State") = ""
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                wucCountrygroup.clearsessions()
                btnReset_Click(sender, e)





            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region
    Private Sub fillseason()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select min(c.applicableto)applicableto,min(c.fromdate) fromdate,max(c.todate) todate from view_contracts_search c(nolock) Where c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    ' If ViewState("State") = "New" Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",    ", ","), String)

                    End If
                    'End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        hdnconfromdate.Value = Format(mySqlReader("fromdate"), "dd/MM/yyyy")


                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        hdncontodate.Value = Format(mySqlReader("todate"), "dd/MM/yyyy")


                    End If

                End If

            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

            mySqlConn.Close()


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
    Private Sub FillFormula(Optional ByVal contractid As String = "")
        Dim myDS As New DataSet
        Dim strsql As String = ""
        Dim formulaid As String = ""


        grdcommission.Visible = True

        strsql = "SELECT formulaid,formulaname ,isnull(remarks,'') remarks,  (select left(terms,len(terms)-2) from (select (select term1 + ' ' + operator1 + ' ' + term2 + ' = ' + resultterm+ ' | ' from commissionformula_detail where formulaid=commissionformula_header.formulaid order by flineno For XML PATH ('')) as terms) as t) as [Formula]  FROM commissionformula_header where active=1 "


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
        myDataAdapter.Fill(myDS)
        grdcommission.DataSource = myDS
        grdcommission.DataBind()

        '  If chkcommission.Checked = True Then

        Dim formulacode As String = ""

        If Session("Calledfrom") = "Offers" Then
            If contractid <> "" Then
                formulacode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select formulaid from view_contracts_commissions(nolock) where tranid='" & contractid & "'")
            End If
        Else
            If txtplistcode.Text <> "" Then
                formulacode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select formulaid from view_contracts_commissions(nolock) where tranid='" & txtplistcode.Text & "'")
            End If
        End If


        For Each gvrow As GridViewRow In grdcommission.Rows

            Dim chkcomm As CheckBox = gvrow.FindControl("chkcomm")
            Dim txtformula As Label = gvrow.FindControl("txtformulacode")
            Dim optcomm As RadioButton = gvrow.FindControl("optcomm")


            If txtformula.Text = formulacode Then
                chkcomm.Checked = True
                optcomm.Checked = True

            End If

            If optcomm.Checked = True Then
                formulaid = txtformula.Text
            End If


        Next


        ' End If

        If CType(Session("ContractState"), String) = "New" Then

            grdcommissiondetail.Visible = True
            myDS.Clear()
            strsql = "SELECT  d.term1,t.termname,'' value, t.rankorder from commissionformula_detail d inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.formulaid='" & formulaid & "' union all  " _
                     & " select d.term2,t.termname,'' value ,t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & formulaid & "' order by 4"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        Else
            grdcommissiondetail.Visible = True
            myDS.Clear()
            If Session("Calledfrom") = "Offers" Then

                strsql = "SELECT distinct  d.term1,t.termname,d.value,t.rankorder   from view_contracts_commissions d  inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.tranid='" & contractid & "' union all " _
                   & " select d.term2,t.termname ,convert(decimal(18,3),0),t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & formulaid & "' and d.term2 not in (select term1 from view_contracts_commissions where tranid='" & contractid & "') order by 4"
            Else

                strsql = "SELECT distinct d.term1,t.termname,d.value,t.rankorder   from view_contracts_commissions d  inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.tranid='" & txtplistcode.Text & "' union all " _
                   & " select d.term2,t.termname ,convert(decimal(18,3),0),t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & formulaid & "' and d.term2 not in (select term1 from view_contracts_commissions where tranid='" & txtplistcode.Text & "') order by 4"
            End If


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        End If
        ' Enabletax()

    End Sub
    Protected Sub optcomm_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim optcomm As RadioButton = CType(sender, RadioButton)

        Dim row As GridViewRow = CType((CType(sender, RadioButton)).NamingContainer, GridViewRow)
        Dim txtformula As Label = row.FindControl("txtformulacode")


        Dim myDS As New DataSet
        Dim strsql As String = ""
        Dim formulaid As String = ""
        Dim contractid As String = ""

        If CType(ViewState("State"), String) = "New" Or ViewState("CopyFrom") = "CopyFrom" Or ViewState("CopyFrom") = "Copy" Then

            grdcommissiondetail.Visible = True
            myDS.Clear()
            strsql = "SELECT  d.term1,t.termname,'' value, t.rankorder from commissionformula_detail d inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.formulaid='" & txtformula.Text & "' union all  " _
                     & " select d.term2,t.termname,'' value ,t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & txtformula.Text & "' order by 4"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        Else
            grdcommissiondetail.Visible = True
            myDS.Clear()
            strsql = "SELECT  d.term1,t.termname,d.value,t.rankorder   from view_contracts_commissions d  inner join commissionterms t on d.term1=t.termcode where t.systemvalue=0 and d.tranid='" & txtplistcode.Text & "' union all " _
                     & " select d.term2,t.termname ,0,t.rankorder from commissionformula_detail d inner join commissionterms t on d.term2=t.termcode  where t.systemvalue=0 and d.formulaid='" & txtformula.Text & "' and d.term2 not in (select term1 from view_contracts_commissions where tranid='" & txtplistcode.Text & "') order by 4"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdcommissiondetail.DataSource = myDS
            grdcommissiondetail.DataBind()
        End If
        ' Enabletax()


        'sbAgentSelectAndSortAndAssign(sender, IIf(chk2_agent.Checked = True, 1, 0), "")
        '

    End Sub

    Private Sub FillTerms()
        Try
            Dim strSqlQry As String
            Dim myDS As New DataSet
            strSqlQry = "select RankOrder,TermCode,TermName from commissionterms order by rankOrder"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdCommTerms.DataSource = myDS
            grdCommTerms.DataBind()
            clsDBConnect.dbAdapterClose(myDataAdapter)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try
    End Sub
    Sub seasonsgridfill()
        Try
            Dim myDS As New DataSet
            grdseason.Visible = True
            strSqlQry = ""


            'strSqlQry = "select distinct seasonname subseasname,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' order by subseasname "
            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName " ' convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"


            ' strSqlQry = "select distinct seasonname subseasname,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' order by subseasname "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdseason.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdseason.DataBind()

            Else
                grdseason.DataBind()

            End If


            For i As Integer = grdseason.Rows.Count - 1 To 1 Step -1
                Dim row As GridViewRow = grdseason.Rows(i)
                Dim previousRow As GridViewRow = grdseason.Rows(i - 1)
                Dim J As Integer = 2
                Dim k As Integer = 1
                If row.Cells(J).Text = previousRow.Cells(J).Text Then
                    If previousRow.Cells(J).RowSpan = 0 Then
                        If row.Cells(J).RowSpan = 0 Then
                            previousRow.Cells(J).RowSpan += 2
                            previousRow.Cells(k).RowSpan += 2

                        Else
                            previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1
                            previousRow.Cells(k).RowSpan = row.Cells(k).RowSpan + 1

                        End If
                        row.Cells(J).Visible = False
                        row.Cells(k).Visible = False
                    End If
                End If
            Next

            strSqlQry = ""


            myDS.Clear()


            '''' To check Extra Pax available or not if is not there it includes party_rmcat table

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "sp_checkExtrapax'" & CType(hdnpartycode.Value, String) & "'"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlCmd.ExecuteNonQuery()

            'cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()



            strSqlQry = "select prc.rmcatcode,0 selected , rc.rmcatname  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                     & hdnpartycode.Value & "'    order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdrmcat.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdrmcat.DataBind()

            Else
                grdrmcat.DataBind()

            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    Private Sub FillRoomtypemealplan()
        Try
            Dim myDS As New DataSet
            grdroomtype.Visible = True
            strSqlQry = ""

            strSqlQry = "select rmtypcode,rmtypname,0 selected from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdroomtype.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdroomtype.DataBind()

            Else
                grdroomtype.DataBind()

            End If





            strSqlQry = ""
            Dim myDS1 As New DataSet

            strSqlQry = "select p.mealcode as mealcode,m.mealname,0 selected  from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS1)
            grdmealplan.DataSource = myDS1

            If myDS1.Tables(0).Rows.Count > 0 Then
                grdmealplan.DataBind()

            Else
                grdmealplan.DataBind()

            End If



            'If ViewState("State") <> "New" Then

            '    Dim dt As New DataTable
            '    Dim chksel As CheckBox
            '    Dim gvRow As GridViewRow
            '    Dim txtrmtypcode As Label
            '    Dim i As Integer
            '    dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select rmtypcode from view_contracts_checkinout_roomtypes(nolock) where checkinoutpolicyid='" & txtplistcode.Text & "'").Tables(0)
            '    For Each gvRow In grdroomtype.Rows
            '        chksel = gvRow.FindControl("chkrmtyp")
            '        txtrmtypcode = gvRow.FindControl("txtrmtypcode")
            '        For i = 0 To dt.Rows.Count - 1
            '            If dt.Rows(i)(0).ToString = txtrmtypcode.Text Then
            '                chksel.Checked = True
            '            End If
            '        Next
            '    Next


            '    Dim dtmeal As New DataTable
            '    Dim chkselmeal As CheckBox

            '    Dim txtmealcode As Label
            '    dtmeal = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select mealcode from view_contracts_checkinout_mealplans(nolock) where checkinoutpolicyid='" & txtplistcode.Text & "'").Tables(0)
            '    For Each gvRow In grdmealplan.Rows
            '        chkselmeal = gvRow.FindControl("chkmeal")
            '        txtmealcode = gvRow.FindControl("txtmealcode")
            '        For i = 0 To dtmeal.Rows.Count - 1
            '            If dtmeal.Rows(i)(0).ToString = txtmealcode.Text Then
            '                chkselmeal.Checked = True
            '            End If
            '        Next
            '    Next

            'End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        ViewState("State") = "New"

        txtseasonname.Text = ""
        PanelMain.Visible = True
        PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False
        Session("contractid") = hdncontractid.Value
        wucCountrygroup.Visible = True



        'If txtpromotionid.Text <> "" Then

        '    Dim commissiontype As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(commissiontype,'') from view_offers_header where promotionid='" & txtpromotionid.Text & "'")
        '    If commissiontype.ToUpper.ToUpper <> UCase("Special commissionable Rates") Then

        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Promotion Special commissionable Not Selected ');", True)
        '        PanelMain.Style("display") = "none"
        '        PanelMain.Style("display") = "block"
        '        Exit Sub

        '    End If
        'End If


        If Session("Calledfrom") = "Offers" Then

            'wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            'wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            divoffer.Style.Add("display", "block")
            lblHeading.Text = "New Commission  - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + " Commission -" + ViewState("hotelname") + "-" + hdnpromotionid.Value
            'txtpromotionid.Text = ""
            'txtpromoitonname.Text = ""


            txtpromotionid.Text = hdnpromotionid.Value
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(promotionname,'') promotionname , ApplicableTo,commissiontype  from view_offers_header (nolock) where  promotionid='" & txtpromotionid.Text & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                txtpromoitonname.Text = ds.Tables(0).Rows(0).Item("promotionname")
                txtApplicableTo.Text = ds.Tables(0).Rows(0).Item("ApplicableTo")
                hdncommtype.Value = ds.Tables(0).Rows(0).Item("commissiontype")
            End If

            Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
            If ds1.Tables(0).Rows.Count > 0 Then
                hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
            End If


            grdDates.Visible = True
            fillDategrd(grdDates, True)
            chkcomputed.Checked = False

            fillpromotiondetails(hdnpromotionid.Value)
            FillPromotionDates(hdnpromotionid.Value)

            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")

            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            DisableControl()
            lblstatus.Visible = False
            lblstatustext.Visible = False

            FillTerms()
            FillFormula()

            wucCountrygroup.Visible = True
            btnSave.Visible = True
            btnSave.Text = "Save"


        Else
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
            divoffer.Style.Add("display", "none")
            lblHeading.Text = "New Commission  - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + " Commission -" + ViewState("hotelname")

            fillseason()
            FillRoomtypemealplan()
            seasonsgridfill()
            DisableControl()
            lblstatus.Visible = False
            lblstatustext.Visible = False
            FillTerms()
            FillFormula()
            grdDates.Visible = True
            fillDategrd(grdDates, True)

            wucCountrygroup.Visible = True

            chkcomputed.Checked = False

            btnSave.Visible = True

            btnSave.Text = "Save"



            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()

        End If









    End Sub
#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub
    'Protected Sub btnClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear1.Click
    '    ViewState("noshowclick") = Nothing
    '    ''  ModalExtraPopup.Hide()
    'End Sub
    Sub FillRoomdetails()
        ''  createdatatable()

        ''   grdRoomrates.Visible = True

        ''   lable12.Visible = True
        ''   btncopyratesnextrow.Visible = True
        ' grdWeekDays.Enabled = False




    End Sub



    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentListSearch(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lsAgentNames As New List(Of String)
        Dim lsCountryList As String
        Try

            'strSqlQry = "select agentname from agentmast where active=1 and agentname like  '" & prefixText & "%'"
            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList
            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
                End If

            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'lsAgentNames.Add(myDS.Tables(0).Rows(i)("agentname").ToString())
                    lsAgentNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next

            End If

            Return lsAgentNames
        Catch ex As Exception

            Return lsAgentNames
        End Try

    End Function
#End Region

    Private Sub FillGrid(ByVal strsortby As String, ByVal contractid As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try
            If Session("Calledfrom") = "Offers" Then

                gv_SearchResult.Columns(2).Visible = True
                gv_SearchResult.Columns(3).Visible = True
                gv_SearchResult.Columns(4).Visible = False

                If strsortby = "fromdate" Or strsortby = "todate" Then



                    strSqlQry = "with ctee as(select h.tranid tranid,h.promotionid,p.promotionname,'' season, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                                & "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock),view_offers_header p (nolock)  where  h.promotionid=p.promotionid and h.tranid=d.tranid and     h.promotionid='" & contractid & "' and h.promotionid <>'' and h.seasons ='' group by h.tranid," _
                                & " h.promotionid,p.promotionname, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser ) select * from ctee " _
                                & " order by convert(datetime," & strsortby & ",103) " & strsortorder & ""


                    'strSqlQry = "with ctee as( select h.tranid tranid,h.promotionid,p.promotionname, h.seasons season,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') fromdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                    '             " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_commission_header h(nolock) ,view_offers_header p (nolock) where h.promotionid=p.promotionid and  h.promotionid='" & contractid & "' and h.promotionid <>'' and h.seasons <>'' union all " _
                    '             & " select h.tranid tranid,h.promotionid,p.promotionname,'' season, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                    '             & "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock),view_offers_header p (nolock)  where  h.promotionid=p.promotionid and h.tranid=d.tranid and     h.promotionid='" & contractid & "' and h.promotionid <>'' and h.seasons ='' group by h.tranid," _
                    '             & " h.promotionid,p.promotionname, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser ) select * from ctee " _
                    '             & " order by convert(datetime," & strsortby & ",103) " & strsortorder & ""

                Else



                    strSqlQry = "select h.tranid tranid,h.promotionid,p.promotionname,'' season, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                & "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock),view_offers_header p (nolock)  where  h.promotionid=p.promotionid and h.tranid=d.tranid and     h.promotionid='" & contractid & "' and " _
                & "  h.promotionid <>''  and  h.seasons ='' group by h.tranid,h.promotionid,p.promotionname,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser" _
                & " order by " & strsortby & " " & strsortorder & ""

                    '  strSqlQry = "select h.tranid tranid,h.promotionid,p.promotionname ,h.seasons season,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') fromdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                    '" h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_commission_header h(nolock),view_offers_header p (nolock)  where h.promotionid=p.promotionid and h.promotionid='" & contractid & "'  and h.promotionid <>'' and h.seasons <>'' union all " _
                    '& " select h.tranid tranid,h.promotionid,p.promotionname,'' season, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                    '& "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock),view_offers_header p (nolock)  where  h.promotionid=p.promotionid and h.tranid=d.tranid and     h.promotionid='" & contractid & "' and " _
                    '& "  h.promotionid <>''  and  h.seasons ='' group by h.tranid,h.promotionid,p.promotionname,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser" _
                    '& " order by " & strsortby & " " & strsortorder & ""

                End If

            Else

                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                gv_SearchResult.Columns(4).Visible = True
                If strsortby = "fromdate" Or strsortby = "todate" Then
                    strSqlQry = "with ctee as( select h.tranid tranid,h.promotionid,'' promotionname,h.seasons season,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') fromdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                                 " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_commission_header h(nolock)  where  h.contractid='" & contractid & "'and h.promotionid =''  and h.seasons <>'' union all " _
                                 & " select h.tranid tranid,h.promotionid,'' promotionname,'' season, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                                 & "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock)  where  h.tranid=d.tranid and     h.contractid='" & contractid & "' and h.promotionid ='' and h.seasons ='' group by h.tranid," _
                                 & " h.promotionid, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser ) select * from ctee " _
                                 & " order by convert(datetime," & strsortby & ",103) " & strsortorder & ""

                Else

                    strSqlQry = "select h.tranid tranid,h.promotionid,'' promotionname,h.seasons season,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') fromdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                  " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_commission_header h(nolock)  where  h.contractid='" & contractid & "' and h.promotionid ='' and h.seasons <>'' union all " _
                  & " select h.tranid tranid,h.promotionid,'' promotionname,'' season, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                  & "   from view_contracts_commission_header h(nolock),view_contracts_commission_detail d(nolock)  where  h.tranid=d.tranid and     h.contractid='" & contractid & "' and " _
                  & "  h.promotionid ='' and   h.seasons ='' group by h.tranid,h.promotionid,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser" _
                  & " order by " & strsortby & " " & strsortorder & ""

                End If
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)

        Dim CalledfromValue As String = ""

        Dim Conappid As String = ""
        Dim Conappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""


        If IsPostBack = False Then
            Conappid = 1
            Conappname = objUser.GetAppName(Session("dbconnectionName"), Conappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else
                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(Conappname, String), "ContractCommission.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(Conappname, String), "ContractCommission.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If

            End If

            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractCommission.aspx", Conappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, Conappid, intMenuID)

            If functionalrights <> "" Then
                strTempUserFunctionalRight = functionalrights.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strRights = strTempUserFunctionalRight.GetValue(lngCount)

                    If strRights = "07" Then
                        Count = 1
                    End If
                Next

                If CalledfromValue = 1030 Then
                    btnselect.Visible = False
                    If Count = 1 Then
                        btncopycontract.Visible = True
                    Else
                        btncopycontract.Visible = False
                    End If

                ElseIf CalledfromValue = 1200 Then
                    '  btncopycontract.Visible = False
                    If Count = 1 Then
                        btnselect.Visible = True
                        btncopycontract.Visible = True
                    Else
                        btnselect.Visible = False
                        btncopycontract.Visible = False
                    End If
                End If

            Else
                btnselect.Visible = False
                btncopycontract.Visible = False

            End If
            If Session("Calledfrom") = "Offers" Then

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                txtconnection.Value = Session("dbconnectionName")
                hdnpartycode.Value = CType(Session("Offerparty"), String)

                SubMenuUserControl1.partyval = hdnpartycode.Value
                '  SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                divoffer.Style.Add("display", "block")
                btnselect.Style.Add("display", "block")

                gv_SearchResult.Columns(2).Visible = True
                gv_SearchResult.Columns(3).Visible = True
                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                    txtpromotionid.Text = Session("OfferRefCode")
                    Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(promotionname,'') promotionname , ApplicableTo,commissiontype  from view_offers_header (nolock) where  promotionid='" & txtpromotionid.Text & "'")
                    If ds.Tables(0).Rows.Count > 0 Then
                        txtpromoitonname.Text = ds.Tables(0).Rows(0).Item("promotionname")
                        txtApplicableTo.Text = ds.Tables(0).Rows(0).Item("ApplicableTo")
                        hdncommtype.Value = ds.Tables(0).Rows(0).Item("commissiontype")
                    End If

                    Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
                    If ds1.Tables(0).Rows.Count > 0 Then
                        hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                        hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
                    End If


                End If
                wucCountrygroup.sbSetPageState("", "OFFERSCOMMISSION", CType(Session("OfferState"), String))


                hdnpartycode.Value = Session("partycode")
                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                lblHeading.Text = lblHeading.Text + " - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Page.Title = "Commission "

                '   btncopycontract.Style.Add("display", "none")

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1



                FillGrid("tranid", txtpromotionid.Text, hdnpartycode.Value, "Desc")

            Else

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                txtconnection.Value = Session("dbconnectionName")
                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)

                divoffer.Style.Add("display", "none")
                btnselect.Style.Add("display", "none")
                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                wucCountrygroup.sbSetPageState("", "CONTRACTCOMMISSION", CType(Session("ContractState"), String))

                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value
                lblHeading.Text = lblHeading.Text + " - " + ViewState("hotelname") + " - " + hdncontractid.Value

                Page.Title = "Commission "
                btncopycontract.Style.Add("display", "block")

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1



                FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")

                wucCountrygroup.sbSetPageState("", "CONTRACTCOMMISSION", CType(Session("ContractState"), String))


            End If






            '   hdnpartycode.Value = CType(Request.QueryString("partycode"), String)


            'lblbookingvaltype.Visible = False
            'ddlBookingValidity.Visible = False



            '  PanelMain.Visible = False

            'btnCancel.Attributes.Add("onclick", "javascript :if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            End If
        Else
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
        End If
            'txtfromDate.Attributes.Add("onchange", "setdate();")
            'txtToDate.Attributes.Add("onchange", "checkdates('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "');")
            'txtfromDate.Attributes.Add("onchange", "checkfromdates('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "');")

            chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")
            If Session("Calledfrom") = "Offers" Then
                btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdnpromotionid.Value & "')")
            '  btnAddNew.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
            '    btncopycontract.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
                btnselect.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
            Else
                btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
                btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            End If



            Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub


    Protected Sub grdDates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDates.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtFromDate As TextBox = CType(e.Row.FindControl("txtfromdate"), TextBox)
                Dim txtToDate As TextBox = CType(e.Row.FindControl("txttodate"), TextBox)
                'Dim btnImgRmv As ImageButton = CType(e.Row.FindControl("btnImgRmv"), ImageButton)

                If Session("Calledfrom") = "Offers" Then

                    txtFromDate.Attributes.Add("onchange", "setdate();")
                    txtToDate.Attributes.Add("onchange", "checkdatespromo('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
                    txtFromDate.Attributes.Add("onchange", "checkfromdatespromo('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")

                Else
                    txtFromDate.Attributes.Add("onchange", "setdate();")
                    txtToDate.Attributes.Add("onchange", "checkdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
                    txtFromDate.Attributes.Add("onchange", "checkfromdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
                End If


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub grdcommissiondetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdcommissiondetail.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtperc As TextBox = e.Row.FindControl("txtperc")

                Numberssrvctrl(txtperc)


            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub sortgvsearch()
        If Session("Calledfrom") = "Offers" Then
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 1
                    FillGrid("promotionid", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)

                Case 2
                    FillGrid("fromdate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("todate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 4
                    FillGrid("h.applicableto", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5
                    FillGrid("h.adddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 6
                    FillGrid("h.adduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 7
                    FillGrid("h.moddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 8
                    FillGrid("h.moduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            End Select
        Else
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 2
                    FillGrid("season", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("fromdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 4
                    FillGrid("todate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5
                    FillGrid("h.applicableto", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 6
                    FillGrid("h.adddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 7
                    FillGrid("h.adduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 8
                    FillGrid("h.moddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 98
                    FillGrid("h.moduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            End Select
        End If
    End Sub
    Protected Sub ddlorder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub
    Private Sub fillpromotiondetails(ByVal promotionid As String)

        Try


            Dim myDS2 As New DataSet
            strSqlQry = "select v.rmtypcode,p.rmtypname,1 selected from view_offers_rmtype v(nolock), partyrmtyp p (nolock) where p.rmtypcode=v.rmtypcode and p.partycode =v.partycode and  p.inactive=0 and v.partycode='" & hdnpartycode.Value & "' and v.promotionid='" & promotionid & "' order by isnull(p.rankord,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS2)
            grdroomtype.DataSource = myDS2

            If myDS2.Tables(0).Rows.Count > 0 Then
                grdroomtype.DataBind()
            Else
                grdroomtype.DataBind()
            End If

            myDS2.Clear()
            strSqlQry = "select v.mealcode,p.mealname,1 selected from view_offers_meal v(nolock), mealmast p (nolock) where p.mealcode=v.mealcode  and  p.active=1 and  v.promotionid='" & promotionid & "' order by isnull(p.rankorder,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS2)
            grdmealplan.DataSource = myDS2

            If myDS2.Tables(0).Rows.Count > 0 Then
                grdmealplan.DataBind()
            Else
                grdmealplan.DataBind()
            End If

            myDS2.Clear()
            strSqlQry = "select v.rmcatcode,p.rmcatname,1 selected from view_offers_accomodation v(nolock), rmcatmast p (nolock) where p.rmcatcode=v.rmcatcode  and  p.active=1  and v.promotionid='" & promotionid & "' order by isnull(p.rankorder,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS2)
            grdrmcat.DataSource = myDS2

            If myDS2.Tables(0).Rows.Count > 0 Then
                grdrmcat.DataBind()
            Else
                grdrmcat.DataBind()
            End If


            Dim chkSelrm As CheckBox
            Dim lblrmtmyp As Label
            For Each grdRow In grdroomtype.Rows
                chkSelrm = CType(grdRow.FindControl("chkrmtyp"), CheckBox)
                lblrmtmyp = CType(grdRow.FindControl("lblselect"), Label)

                If lblrmtmyp.Text = "1" Then
                    chkSelrm.Checked = True
                    If ViewState("State") = "Copy" Then
                        chkSelrm.Enabled = True

                    End If

                End If

            Next



            Dim chkSelmeal As CheckBox
            Dim lblmeal As Label
            For Each grdRow In grdmealplan.Rows
                chkSelmeal = CType(grdRow.FindControl("chkmeal"), CheckBox)
                lblmeal = CType(grdRow.FindControl("lblselect"), Label)

                If lblmeal.Text = "1" Then
                    chkSelmeal.Checked = True
                    If ViewState("State") = "Copy" Then
                        chkSelmeal.Enabled = True

                    End If

                End If

            Next




            Dim chkSel As CheckBox
            Dim lblrmcat As Label
            For Each grdRow In grdrmcat.Rows
                chkSel = CType(grdRow.FindControl("chkrmcat"), CheckBox)
                lblrmcat = CType(grdRow.FindControl("lblselect"), Label)

                If lblrmcat.Text = "1" Then
                    chkSel.Checked = True
                    If ViewState("State") = "Copy" Then
                        chkSel.Enabled = True

                    End If

                End If

            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub grdpromotion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdpromotion.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lblpromotionid As Label
            Dim lblpromotionname As Label, lblapplicable As Label, lblplistcode As Label

            lblpromotionid = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionid")
            lblpromotionname = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionname")
            lblapplicable = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapplicableto")
            lblplistcode = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            If lblpromotionid.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then
                txtpromotionid.Text = CType(lblpromotionid.Text, String)
                txtpromoitonname.Text = CType(lblpromotionname.Text, String)
                txtApplicableTo.Text = CType(lblapplicable.Text, String)

                FillFormula(lblplistcode.Text)
                FillTerms()

                PanelMain.Visible = True
                ViewState("State") = "Copy"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lblplistcode.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lblplistcode.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lblplistcode.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDatesnew(CType(hdnpromotionid.Value, String))

                FillTerms()
                FillFormula(lblplistcode.Text)
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"



                lblHeading.Text = "Copy Commission - " + ViewState("hotelname") + "- " + hdnpromotionid.Value
                Page.Title = "Promotion Commission "


                'fillpromotiondetails(lblpromotionid.Text)
                'FillPromotionDates(lblpromotionid.Text)

                'wucCountrygroup.sbSetPageState(lblpromotionid.Text.Trim, Nothing, "Edit")
                'Session("isAutoTick_wuccountrygroupusercontrol") = 1
                'wucCountrygroup.sbShowCountry()


                'wucCountrygroup.Visible = True
                'wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("ContractState"), String))
                'wucCountrygroup.sbSetPageState(lblpromotionid.Text, Nothing, "Edit")


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class
