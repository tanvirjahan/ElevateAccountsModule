
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ExcursionModule_ManageTicketsReceived
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
    Dim chkSel As CheckBox
    Dim otypecode1, otypecode2 As String

    Dim gvRow As GridViewRow

    Dim lblLineNo As Label

    Dim txtprefix As TextBox
    Dim txtsuffix As TextBox

    Dim txtfromticketno As TextBox
    Dim txttoticketno As TextBox
    Dim txtTicketDate As TextBox
    Dim txtRemarks As TextBox
    Dim chkDel As CheckBox
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Page.IsPostBack = False Then
                Session.Add("ManageTicketState", Request.QueryString("State"))
                Session.Add("ManageTicketRefCode", Request.QueryString("RefCode"))

                txtconnection.Value = Session("dbconnectionName")


                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpCode, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpName, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname", True)

                ddlExTypeCode.Items.Clear()
                ddlExTypeCode.Items.Add("[Select]")
                ddlExTypeCode.Value = "[Select]"

                ddlExTypeName.Items.Clear()
                ddlExTypeName.Items.Add("[Select]")
                ddlExTypeName.Value = "[Select]"

            End If

            Dim typ As Type
            typ = GetType(DropDownList)
            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddlExGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlExGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlExTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlExTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                Try


                    If CType(Session("ManageTicketState"), String) = "New" Then
                        Dim RefCode As String
                        RefCode = CType(Session("ManageTicketRefCode"), String)
                        btnSave.Text = "Save"
                        btnDeleteRow.Visible = True
                        dpDateRecieved.txtDate.Text = Format(Date.Now, "dd/MM/yyyy")
                        FilGrid(gv_row, False, True, 5)
                        lblHeading.Text = "Add New Tickets"
                        Page.Title = Page.Title + " " + "Manage Tickets Recieved"
                        btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    ElseIf CType(Session("ManageTicketState"), String) = "EditRow" Then
                        btnSave.Text = "Save/Update"
                        Dim RefCode As String
                        RefCode = CType(Session("ManageTicketRefCode"), String)
                        ShowRecord(RefCode)
                        btnResetAll.Enabled = False
                        lblHeading.Text = "Edit Manage Tickets Recieved"
                        Page.Title = Page.Title + " " + "Edit Manage Tickets Recieved"
                        btnSave.Attributes.Add("onclick", "return FormValidation('EditRow')")
                    ElseIf CType(Session("ManageTicketState"), String) = "View" Then
                        Dim RefCode As String
                        RefCode = CType(Session("ManageTicketRefCode"), String)
                        ShowRecord(RefCode)
                        DisableAllControls()
                        btnSave.Visible = False
                        lblHeading.Text = "View Manage Tickets Recieved"
                        Page.Title = Page.Title + " " + "View Manage Tickets Recieved"

                    ElseIf CType(Session("ManageTicketState"), String) = "DeleteRow" Then
                        btnSave.Text = "Delete"
                        Dim RefCode As String
                        RefCode = CType(Session("ManageTicketRefCode"), String)
                        ShowRecord(RefCode)
                        DisableAllControls()
                        btnSave.Enabled = True

                        lblHeading.Text = "Delete Manage Tickets Recieved"
                        Page.Title = Page.Title + " " + "Delete Manage Tickets Recieved"
                        btnSave.Attributes.Add("onclick", "return FormValidation('DeleteRow')")
                    End If

                Catch ex As Exception

                End Try
            End If

            If IsPostBack = True Then

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 and a.othgrpcode='" & hdnExcursionGroupCode.Value & "' order by a.othtypcode", True, hdnExcursionTypeName.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 and a.othgrpcode='" & hdnExcursionGroupCode.Value & "' order by a.othtypname", True, hdnExcursionTypeCode.Value)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "NumbersForTextbox"
    Public Sub NumbersForTextbox(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            strSqlQry = "select * from excursion_tickets_received where ticketid='" & RefCode & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = SqlCmd.ExecuteReader
            If mySqlReader.Read = True Then
                If IsDBNull(mySqlReader("ticketid")) = False Then
                    txtAllotmentID.Value = mySqlReader("ticketid")
                End If
                If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    ddlExGrpName.Value = mySqlReader("othgrpcode")

                    ddlExGrpCode.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode")))

                    hdnExcursionGroupCode.Value = mySqlReader("othgrpcode")
                End If
                If IsDBNull(mySqlReader("othtypcode")) = False Then
                    'ddlExTypeName.Value = mySqlReader("othtypcode")
                    'ddlExTypeCode.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", mySqlReader("othtypcode")))

                    otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                    otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname", True)

                    ddlExTypeName.Value = mySqlReader("othtypcode")
                    ddlExTypeCode.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", mySqlReader("othtypcode")))

                    hdnExcursionTypeCode.Value = mySqlReader("othtypcode")
                    hdnExcursionTypeName.Value = ddlExTypeCode.Value

                End If

                If IsDBNull(mySqlReader("datereceived")) = False Then
                    dpDateRecieved.txtDate.Text = Format(CType(mySqlReader("datereceived"), Date), "dd/MM/yyyy")
                End If



                If IsDBNull(mySqlReader("remarks")) = False Then
                    txtRemark.Text = mySqlReader("remarks")
                End If


                If CType(Session("ManageTicketState"), String) = "EditRow" Or CType(Session("ManageTicketState"), String) = "View" Or CType(Session("ManageTicketState"), String) = "DeleteRow" Then

                    Dim lngCnt As Long
                    lngCnt = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excursion_tickets_detail", "count(ticketid)", "ticketid", RefCode), Long)

                    If CType(Session("ManageTicketState"), String) = "EditRow" Then
                        If lngCnt = 0 Then lngCnt = 1

                        FilGrid(gv_row, True, False, lngCnt)
                    Else
                        If lngCnt = 0 Then lngCnt = 0

                        FilGrid(gv_row, True, False, lngCnt)
                    End If


                    Dim lblLineNo As Label

                    Dim txtprefix As TextBox
                    Dim txtsuffix As TextBox

                    Dim txtfromticketno As TextBox
                    Dim txttoticketno As TextBox
                    Dim txtTicketDate As TextBox
                    Dim txtRemarks As TextBox

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    SqlCmd = New SqlCommand("Select * from excursion_tickets_detail Where ticketid='" & RefCode & "'", mySqlConn)
                    mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If mySqlReader.HasRows Then
                        While mySqlReader.Read()
                            For Each gvRow In gv_row.Rows


                                lblLineNo = gvRow.FindControl("lblLineNo")
                                txtfromticketno = gvRow.FindControl("txtfromticketno")
                                txttoticketno = gvRow.FindControl("txttoticketno")
                                txtTicketDate = gvRow.FindControl("txtTicketDate")
                                txtRemarks = gvRow.FindControl("txtRemarks")

                                txtprefix = gvRow.FindControl("txtprefix")
                                txtsuffix = gvRow.FindControl("txtsuffix")

                                If mySqlReader("tlineno") = CType(lblLineNo.Text, Integer) Then

                                    If IsDBNull(mySqlReader("fromticketno")) = False Then
                                        txtfromticketno.Text = CType(mySqlReader("fromticketno"), String)
                                    Else
                                        txtfromticketno.Text = ""
                                    End If

                                    If IsDBNull(mySqlReader("toticketno")) = False Then
                                        txttoticketno.Text = CType(mySqlReader("toticketno"), String)
                                    Else
                                        txttoticketno.Text = ""
                                    End If

                                    If IsDBNull(mySqlReader("ticketdate")) = False Then
                                        txtTicketDate.Text = CType(mySqlReader("ticketdate"), String)
                                    Else
                                        txtTicketDate.Text = ""
                                    End If

                                    If IsDBNull(mySqlReader("prefix")) = False Then
                                        txtprefix.Text = CType(mySqlReader("prefix"), String)
                                    Else
                                        txtprefix.Text = ""
                                    End If

                                    If IsDBNull(mySqlReader("suffix")) = False Then
                                        txtsuffix.Text = CType(mySqlReader("suffix"), String)
                                    Else
                                        txtsuffix.Text = ""
                                    End If

                                    If IsDBNull(mySqlReader("remarks")) = False Then
                                        txtRemarks.Text = CType(mySqlReader("remarks"), String)
                                    Else
                                        txtRemarks.Text = ""
                                    End If

                                End If
                            Next
                        End While
                    End If

                    SqlCmd.Dispose()
                    mySqlReader.Close()
                    mySqlConn.Close()
                End If

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub

#End Region








#Region " Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        Try
            ddlExGrpCode.Disabled = True
            ddlExGrpName.Disabled = True
            ddlExTypeCode.Disabled = True
            ddlExTypeName.Disabled = True


            btnSave.Enabled = False
            dpDateRecieved.Enabled = False
            btnResetAll.Enabled = False

            txtAllotmentID.Disabled = True
            txtRemark.Enabled = False

            gv_row.Enabled = False

            btnAddLines.Enabled = False
            btnDeleteRow.Enabled = False


            'Dim GvRow As GridViewRow
            'Dim txtRelease As TextBox
            'For Each GvRow In gv_allotment.Rows
            '    txtRelease = GvRow.FindControl("txtRelease")
            '    txtRelease.Enabled = False
            'Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            'lngcnt = 10
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region


#Region "Private Sub AddLines()"
    Private Sub AddLines()
        Try
            Dim n As Integer = 0
            Dim count As Integer
            count = gv_row.Rows.Count + 1
            Dim lineno(count) As String
            Dim fromticketno(count) As String
            Dim toticketno(count) As String
            Dim TicketDate(count) As String
            Dim Remarks(count) As String
            Dim prefix(count) As String
            Dim suffix(count) As String


            Dim gvRow As GridViewRow
            Dim lblLineNo As Label

            Dim txtprefix As TextBox
            Dim txtsuffix As TextBox

            Dim txtfromticketno As TextBox
            Dim txttoticketno As TextBox
            Dim txtTicketDate As TextBox
            Dim txtRemarks As TextBox
            Dim chkDel As CheckBox


            For Each gvRow In gv_row.Rows
                lblLineNo = gvRow.FindControl("lblLineNo")
                chkDel = gvRow.FindControl("chkDel")
                txtfromticketno = gvRow.FindControl("txtfromticketno")
                txttoticketno = gvRow.FindControl("txttoticketno")
                txtTicketDate = gvRow.FindControl("txtTicketDate")
                txtRemarks = gvRow.FindControl("txtRemarks")


                txtprefix = gvRow.FindControl("txtprefix")
                txtsuffix = gvRow.FindControl("txtsuffix")


                lineno(n) = lblLineNo.Text
                fromticketno(n) = txtfromticketno.Text
                toticketno(n) = txttoticketno.Text
                TicketDate(n) = txtTicketDate.Text
                Remarks(n) = txtRemarks.Text

                prefix(n) = txtprefix.Text
                suffix(n) = txtsuffix.Text

                n = n + 1
            Next
            FilGrid(gv_row, False, False, gv_row.Rows.Count + 1)

            Dim i As Integer = n
            n = 0

            For Each gvRow In gv_row.Rows
                If n = i Then
                    Exit For
                End If
                lblLineNo = gvRow.FindControl("lblLineNo")
                chkDel = gvRow.FindControl("chkDel")
                txtfromticketno = gvRow.FindControl("txtfromticketno")
                txttoticketno = gvRow.FindControl("txttoticketno")
                txtTicketDate = gvRow.FindControl("txtTicketDate")
                txtRemarks = gvRow.FindControl("txtRemarks")


                txtprefix = gvRow.FindControl("txtprefix")
                txtsuffix = gvRow.FindControl("txtsuffix")

                lineno(n) = lblLineNo.Text
                txtfromticketno.Text = fromticketno(n)
                txttoticketno.Text = toticketno(n)
                txtTicketDate.Text = TicketDate(n)
                txtRemarks.Text = Remarks(n)

                txtprefix.Text = prefix(n)
                txtsuffix.Text = suffix(n)
                n = n + 1
            Next
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Public Function ConvertDateFromDatabaseToTextBoxFormat(ByVal strdate As String) As String
        Dim strtemp As String = String.Empty
        Dim strday As String = String.Empty
        Dim strmonth As String = String.Empty
        Dim stryear As String = String.Empty
        Dim lnglist As Long
        Try
            For lnglist = 0 To Len(strdate)
                If Split(strdate, "/", , vbTextCompare)(lnglist) <> "" Then
                    If lnglist = 0 Then
                        stryear = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 1 Then
                        strmonth = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 2 Then
                        strday = Split(strdate, "/", , vbTextCompare)(lnglist)
                        Exit For
                    End If
                End If
            Next lnglist
            If strday.Length = 1 Then
                strday = "0" & strday
            End If
            If strmonth.Length = 1 Then
                strmonth = "0" & strmonth
            End If
            strtemp = strday & "/" & strmonth & "/" & Val(stryear)
            ConvertDateFromDatabaseToTextBoxFormat = strtemp
        Catch SQLexc As Exception
            ConvertDateFromDatabaseToTextBoxFormat = Nothing
        End Try
    End Function

#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExcursionTicketsWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

    Public Function ConvertDateromTextBoxToDatabaseFormat1(ByVal strdate As String) As String
        Dim strtemp As String = String.Empty
        Dim strday As String = String.Empty
        Dim strmonth As String = String.Empty
        Dim stryear As String = String.Empty
        Dim lnglist As Long
        Try
            For lnglist = 0 To Len(strdate)
                If Split(strdate, "/", , vbTextCompare)(lnglist) <> "" Then
                    If lnglist = 0 Then
                        strday = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 1 Then
                        strmonth = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 2 Then
                        stryear = Split(strdate, "/", , vbTextCompare)(lnglist)
                        Exit For
                    End If
                End If
            Next lnglist
            strtemp = (stryear) & "/" & (strmonth) & "/" & (strday)

            ConvertDateromTextBoxToDatabaseFormat1 = strtemp
        Catch SQLexc As Exception
            ConvertDateromTextBoxToDatabaseFormat1 = Nothing
        End Try
    End Function


    Protected Sub btnAddLines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLines.Click
        AddLines()
    End Sub

#Region "Public Function ConvertDateromTextBoxToDatabaseFormat(ByVal strdate As String) As String"
    Public Function ConvertDateromTextBoxToDatabaseFormat(ByVal strdate As String) As String
        Dim strtemp As String = String.Empty
        Dim strday As String = String.Empty
        Dim strmonth As String = String.Empty
        Dim stryear As String = String.Empty
        Dim lnglist As Long
        Try
            For lnglist = 0 To Len(strdate)
                If Split(strdate, "/", , vbTextCompare)(lnglist) <> "" Then
                    If lnglist = 0 Then
                        strday = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 1 Then
                        strmonth = Split(strdate, "/", , vbTextCompare)(lnglist)
                    ElseIf lnglist = 2 Then
                        stryear = Split(strdate, "/", , vbTextCompare)(lnglist)
                        Exit For
                    End If
                End If
            Next lnglist
            strtemp = Val(strmonth) & "/" & Val(strday) & "/" & Val(stryear)

            ConvertDateromTextBoxToDatabaseFormat = strtemp
        Catch SQLexc As Exception
            ConvertDateromTextBoxToDatabaseFormat = Nothing
        End Try
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim GvRow As GridViewRow
        Dim ObjDate As New clsDateTime
        Dim chk As CheckBox
        Dim frmdt As TextBox
        Dim todt As TextBox
        Dim RmCode As HtmlSelect
        Dim lblroomcodee As Label
        Dim lblstopsale As Label

        Try
            If Page.IsValid = True Then

                If CType(Session("ManageTicketState"), String) = "New" Then


                    If ValidateGridOneRow() = False Then
                        Exit Sub
                    End If

                    '-----------------------------------------------------------------------------------
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------------------------------------------------------------------------
                    '                 Inserting Into Main  Ticketing Prices
                    '---------------------------------------------------------------------------------
                    Dim optionval As String

                    optionval = objUtils.GetAutoDocNo("EXTICKET", mySqlConn, sqlTrans)
                    txtAllotmentID.Value = optionval.Trim
                    SqlCmd = New SqlCommand("sp_add_excursion_tickets_received", mySqlConn, sqlTrans)

                    SqlCmd.CommandType = CommandType.StoredProcedure

                    SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 30)).Value = CType(txtAllotmentID.Value.Trim, String)

                    If dpDateRecieved.txtDate.Text = "" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@datereceived", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@datereceived", SqlDbType.DateTime)).Value = Format(CType(dpDateRecieved.txtDate.Text, Date), "yyyy/MM/dd")
                    End If

                    If CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)
                    End If
                    If CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)
                    End If


                    SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 1000)).Value = CType(txtRemark.Text.Trim, String)

                    SqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = DateTime.Now


                    SqlCmd.ExecuteNonQuery()

                    '---------------------------------------------------------------------------------
                    '                 add for allotment new detail
                    '---------------------------------------------------------------------------------

                    Dim lblLineNo As Label
                    Dim txtfromticketno As TextBox
                    Dim txttoticketno As TextBox
                    Dim txtTicketDate As TextBox
                    Dim txtRemarks As TextBox

                    Dim txtprefix As TextBox
                    Dim txtsuffix As TextBox

                    For Each GvRow In gv_row.Rows
                        SqlCmd = New SqlCommand("sp_add_excursion_tickets_detail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                        lblLineNo = GvRow.FindControl("lblLineNo")
                        txtfromticketno = GvRow.FindControl("txtfromticketno")
                        txttoticketno = GvRow.FindControl("txttoticketno")
                        txtTicketDate = GvRow.FindControl("txtTicketDate")
                        txtRemarks = GvRow.FindControl("txtRemarks")
                        txtprefix = GvRow.FindControl("txtprefix")
                        txtsuffix = GvRow.FindControl("txtsuffix")

                        SqlCmd.Parameters.Add(New SqlParameter("@tlineno", SqlDbType.Int)).Value = Val(lblLineNo.Text)

                        If CType(txtfromticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@fromticketno", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@fromticketno", SqlDbType.VarChar, 100)).Value = CType(txtfromticketno.Text, String)
                        End If

                        If CType(txttoticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@toticketno", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@toticketno", SqlDbType.VarChar, 100)).Value = CType(txttoticketno.Text, String)
                        End If

                        If CType(txtTicketDate.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = Format(CType(txtTicketDate.Text, Date), "yyyy/MM/dd")
                        End If


                        If CType(txtprefix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = txtprefix.Text.Trim
                        End If

                        If CType(txtsuffix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = txtsuffix.Text.Trim
                        End If


                        SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = CType(txtRemarks.Text.Trim, String)

                        SqlCmd.ExecuteNonQuery()


                        'Inserting into subdetail

                        SqlCmd = New SqlCommand("sp_add_excursion_tickets_subdetail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)

                        SqlCmd.Parameters.Add(New SqlParameter("@tlineno", SqlDbType.Int)).Value = Val(lblLineNo.Text)

                        If CType(txtprefix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = txtprefix.Text.Trim
                        End If

                        If CType(txtsuffix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = txtsuffix.Text.Trim
                        End If

                        If CType(txtfromticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@fromTicketNo", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@fromTicketNo", SqlDbType.VarChar, 100)).Value = CType(txtfromticketno.Text, String)
                        End If

                        If CType(txttoticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@toTicketNo", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@toTicketNo", SqlDbType.VarChar, 100)).Value = CType(txttoticketno.Text, String)
                        End If

                        If CType(txtTicketDate.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = Format(CType(txtTicketDate.Text, Date), "yyyy/MM/dd")
                        End If
                        SqlCmd.ExecuteNonQuery()

                    Next


                    sqlTrans.Commit()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Sucessfully. Click on OK to Return Search Page');", True)

                    txtAllotmentID.Value = ""

                End If


                If CType(Session("ManageTicketState"), String) = "EditRow" Then

                    '-----------------------------------------------------------------------------------
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------------------------------------------------------------------------
                    '                 Inserting Into Main  Ticketing Prices
                    '---------------------------------------------------------------------------------
                    txtAllotmentID.Value = CType(Session("ManageTicketRefCode"), String)
                    SqlCmd = New SqlCommand("sp_mod_excursion_tickets_received", mySqlConn, sqlTrans)

                    SqlCmd.CommandType = CommandType.StoredProcedure

                    SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 30)).Value = CType(txtAllotmentID.Value.Trim, String)

                    If dpDateRecieved.txtDate.Text = "" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@datereceived", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@datereceived", SqlDbType.DateTime)).Value = Format(CType(dpDateRecieved.txtDate.Text, Date), "yyyy/MM/dd")
                    End If

                    If CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)
                    End If
                    If CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)
                    End If
                    SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 1000)).Value = CType(txtRemark.Text.Trim, String)
                    SqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = DateTime.Now


                    SqlCmd.ExecuteNonQuery()

                    '---------------------------------------------------------------------------------
                    '              delete  detaild
                    '---------------------------------------------------------------------------------
                    SqlCmd = New SqlCommand("sp_delete_excursion_tickets_detail", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 30)).Value = CType(txtAllotmentID.Value.Trim, String)
                    SqlCmd.ExecuteNonQuery()

                    '---------------------------------------------------------------------------------
                    '                 add  new detaild
                    '---------------------------------------------------------------------------------

                    Dim lblLineNo As Label
                    Dim txtfromticketno As TextBox
                    Dim txttoticketno As TextBox
                    Dim txtTicketDate As TextBox
                    Dim txtRemarks As TextBox

                    Dim txtprefix As TextBox
                    Dim txtsuffix As TextBox

                    For Each GvRow In gv_row.Rows
                        SqlCmd = New SqlCommand("sp_add_excursion_tickets_detail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                        lblLineNo = GvRow.FindControl("lblLineNo")
                        txtfromticketno = GvRow.FindControl("txtfromticketno")
                        txttoticketno = GvRow.FindControl("txttoticketno")
                        txtTicketDate = GvRow.FindControl("txtTicketDate")
                        txtRemarks = GvRow.FindControl("txtRemarks")
                        txtprefix = GvRow.FindControl("txtprefix")
                        txtsuffix = GvRow.FindControl("txtsuffix")

                        SqlCmd.Parameters.Add(New SqlParameter("@tlineno", SqlDbType.Int)).Value = Val(lblLineNo.Text)

                        If CType(txtfromticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@fromticketno", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@fromticketno", SqlDbType.VarChar, 100)).Value = CType(txtfromticketno.Text, String)
                        End If

                        If CType(txttoticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@toticketno", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@toticketno", SqlDbType.VarChar, 100)).Value = CType(txttoticketno.Text, String)
                        End If

                        If CType(txtTicketDate.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = Format(CType(txtTicketDate.Text, Date), "yyyy/MM/dd")
                        End If


                        If CType(txtprefix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = txtprefix.Text.Trim
                        End If

                        If CType(txtsuffix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = txtsuffix.Text.Trim
                        End If


                        SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = CType(txtRemarks.Text.Trim, String)

                        SqlCmd.ExecuteNonQuery()


                        'Inserting into subdetail

                        SqlCmd = New SqlCommand("sp_add_excursion_tickets_subdetail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)

                        SqlCmd.Parameters.Add(New SqlParameter("@tlineno", SqlDbType.Int)).Value = Val(lblLineNo.Text)

                        If CType(txtprefix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@prefix", SqlDbType.VarChar, 20)).Value = txtprefix.Text.Trim
                        End If

                        If CType(txtsuffix.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@suffix", SqlDbType.VarChar, 20)).Value = txtsuffix.Text.Trim
                        End If

                        If CType(txtfromticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@fromTicketNo", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@fromTicketNo", SqlDbType.VarChar, 100)).Value = CType(txtfromticketno.Text, String)
                        End If

                        If CType(txttoticketno.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@toTicketNo", SqlDbType.VarChar, 100)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@toTicketNo", SqlDbType.VarChar, 100)).Value = CType(txttoticketno.Text, String)
                        End If

                        If CType(txtTicketDate.Text, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@ticketdate", SqlDbType.DateTime)).Value = Format(CType(txtTicketDate.Text, Date), "yyyy/MM/dd")
                        End If
                        SqlCmd.ExecuteNonQuery()

                    Next


                    sqlTrans.Commit()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Sucessfully. Click on OK to Return Search Page');", True)
                    txtAllotmentID.Value = ""
                End If
                If CType(Session("ManageTicketState"), String) = "DeleteRow" Then

                    '-----------------------------------------------------------------------------------
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------------------------------------------------------------------------
                    '                 Inserting Into Main  Ticketing Prices
                    '---------------------------------------------------------------------------------
                    txtAllotmentID.Value = CType(Session("ManageTicketRefCode"), String)
                    SqlCmd = New SqlCommand("sp_delete_excursion_tickets", mySqlConn, sqlTrans)

                    SqlCmd.CommandType = CommandType.StoredProcedure

                    SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 30)).Value = CType(txtAllotmentID.Value.Trim, String)

                    SqlCmd.ExecuteNonQuery()

                    sqlTrans.Commit()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Deleted Sucessfully. Click on OK to Return Search Page');", True)

                    txtAllotmentID.Value = ""

                End If


                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ExcursionTicketsWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnResetAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetAll.Click
        Try

            ddlExGrpCode.Value = "[Select]"
            ddlExGrpName.Value = "[Select]"
            ddlExTypeCode.Value = "[Select]"
            ddlExTypeName.Value = "[Select]"
            txtAllotmentID.Disabled = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceived.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

   

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MainAllotStopSales','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    Public Sub FilGrid(ByVal grd As GridView, ByVal showrecord As Boolean, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Try

            Dim lngcnt As Long
            If blnload = True Then
                lngcnt = 1
            Else
                lngcnt = count
            End If
            If (showrecord) Then
                grd.DataSource = CreateDataSource()
                If CreateDataSource().Count = 0 Then
                    gv_row.DataSource = CreateDataSource(lngcnt)
                    gv_row.DataBind()
                    If lngcnt = 0 Then
                        ' lblMsg.Visible = True
                        btnSave.Enabled = False
                    End If
                    gv_row.Visible = True
                    Exit Sub
                End If
            Else
                gv_row.DataSource = CreateDataSource(lngcnt)

            End If
            'If CreateDataSource().Count > 0 Or CreateDataSource(lngcnt).Count > 0 Then
            '    'grd.DataBind()
            '    'grd.Visible = True
            '    'lblMsg.Visible = False
            'Else
            '    '  lblMsg.Visible = True
            'End If
            gv_row.DataBind()
            gv_row.Visible = True
            'txtgridrows.Value = grd.Rows.Count

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
       
    End Sub

    Private Function CreateDataSource() As DataView
        Try

            Dim sqlstr As String

            Dim dt As DataTable
            dt = New DataTable
            dt.Columns.Add(New DataColumn("tlineno", GetType(Integer)))

            sqlstr = "Select tlineno from excursion_tickets_detail Where ticketid='" & CType(Session("ManageTicketRefCode"), String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(sqlstr, SqlConn)
            myDataAdapter.Fill(dt)

            CreateDataSource = New DataView(dt)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            CreateDataSource = Nothing
        End Try

    End Function

    Private Function CreateDataSource(ByVal lngcount As Long) As DataView

        Try

            Dim dt As DataTable
            Dim dr As DataRow
            Dim i As Integer
            dt = New DataTable
            dt.Columns.Add(New DataColumn("tlineno", GetType(Integer)))


            For i = 1 To lngcount
                dr = dt.NewRow()
                dr(0) = i
                dt.Rows.Add(dr)
            Next
            'return a DataView to the DataTable
            CreateDataSource = New DataView(dt)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            CreateDataSource = Nothing
        End Try
       
    End Function

    Protected Sub btnDeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRow.Click
        Try

            Dim n As Integer = 0
            Dim count As Integer
            count = gv_row.Rows.Count + 1
            Dim lineno(count) As String
            Dim fromticketno(count) As String
            Dim toticketno(count) As String
            Dim TicketDate(count) As String
            Dim Remarks(count) As String
            Dim prefix(count) As String
            Dim suffix(count) As String

           

            For Each gvRow In gv_row.Rows
                chkDel = gvRow.FindControl("chkDel")
                If chkDel.Checked = False Then
                    lblLineNo = gvRow.FindControl("lblLineNo")
                    chkDel = gvRow.FindControl("chkDel")
                    txtfromticketno = gvRow.FindControl("txtfromticketno")
                    txttoticketno = gvRow.FindControl("txttoticketno")
                    txtTicketDate = gvRow.FindControl("txtTicketDate")
                    txtRemarks = gvRow.FindControl("txtRemarks")

                    txtprefix = gvRow.FindControl("txtprefix")
                    txtsuffix = gvRow.FindControl("txtsuffix")

                    lineno(n) = lblLineNo.Text
                    fromticketno(n) = txtfromticketno.Text
                    toticketno(n) = txttoticketno.Text
                    TicketDate(n) = txtTicketDate.Text
                    Remarks(n) = txtRemarks.Text

                    prefix(n) = txtprefix.Text
                    suffix(n) = txtsuffix.Text
                    n = n + 1
                End If
            Next

            Dim ct As Integer
            ct = n
            If n = 0 Then
                ct = 0
            End If
            If ct = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Minimum one row should be there to Save or update !');", True)
                Exit Sub
            Else
                FilGrid(gv_row, False, False, ct)
            End If

            Dim i As Integer = n
            n = 0

            For Each gvRow In gv_row.Rows
                If n = i Then
                    Exit For
                End If

               lblLineNo = gvRow.FindControl("lblLineNo")
                chkDel = gvRow.FindControl("chkDel")
                txtfromticketno = gvRow.FindControl("txtfromticketno")
                txttoticketno = gvRow.FindControl("txttoticketno")
                txtTicketDate = gvRow.FindControl("txtTicketDate")
                txtRemarks = gvRow.FindControl("txtRemarks")

                txtprefix = gvRow.FindControl("txtprefix")
                txtsuffix = gvRow.FindControl("txtsuffix")


                lineno(n) = lblLineNo.Text
                txtfromticketno.Text = fromticketno(n)
                txttoticketno.Text = toticketno(n)
                txtTicketDate.Text = TicketDate(n)
                txtRemarks.Text = Remarks(n)

                txtprefix.Text = prefix(n)
                txtsuffix.Text = suffix(n)
                n = n + 1
            Next
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gv_row_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_row.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                txtfromticketno = CType(e.Row.FindControl("txtfromticketno"), TextBox)
                txttoticketno = CType(e.Row.FindControl("txttoticketno"), TextBox)
                NumbersForTextbox(txtfromticketno)
                NumbersForTextbox(txttoticketno)
                txttoticketno.Attributes.Add("onchange", "javascript:CheckTicketNo('" + CType(txtfromticketno.ClientID, String) + "','" + CType(txttoticketno.ClientID, String) + "')")

            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Function ValidateGridOneRow() As Boolean

        Try

            If dpDateRecieved.txtDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Date Recieved');", True)
                dpDateRecieved.txtDate.Focus()
                Return False
            End If

            Dim GvRow As GridViewRow
            For Each GvRow In gv_row.Rows
                txtfromticketno = GvRow.FindControl("txtfromticketno")
                txttoticketno = GvRow.FindControl("txttoticketno")
                txtTicketDate = GvRow.FindControl("txtTicketDate")
                txtRemarks = GvRow.FindControl("txtRemarks")

                txtprefix = GvRow.FindControl("txtprefix")
                txtsuffix = GvRow.FindControl("txtsuffix")

                If txtfromticketno.Text.Trim = "" Or txttoticketno.Text.Trim = "" Or txtTicketDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Minimum one row should be entered !');", True)
                    txtfromticketno.Focus()
                    Return False
                End If

            Next
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

   
End Class

