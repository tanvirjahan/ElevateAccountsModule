'------------------------------------------------------------------------------------------------
'   Module Name    :    Journal 
'   Developer Name :    Mangesh
'   Date           :    
'   
'
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
#End Region
 
Partial Class Exchange
    Inherits System.Web.UI.Page
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
    Dim mbasecurrency As String = ""

    'For accounts posting
#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim sqlTrans As SqlTransaction
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim gvRow As GridViewRow

    Enum grd_col
        AccType = 0
        AccCode = 1
        AccName = 2
        ctrolcode = 3
        ctrolname = 4
        costcentercode = 5
        costcentername = 6
        Narration = 7
        Currency = 8
        CnvtRate = 9
        debit = 10
        Credit = 11
        BaseDebit = 12
        BaseCredit = 13
        AdBill = 14
        LienNo = 15
        t1 = 16
        t2 = 17
        t3 = 18
        oldlineno = 19
    End Enum
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "NumbersDecimalRound"
    Public Sub NumbersDecimalRound(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersDecimalRoundHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "NumbersInt"
    Public Sub NumbersIntHtml(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub NumbersInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub NumbersDateInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber2(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLockhtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState.Add("JournalState", Request.QueryString("State"))
        ViewState.Add("JournalTranType", "EX")
        If Page.IsPostBack = False Then
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                txtconnection.Value = Session("dbconnectionName")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                SetFocus(txtnarration)

                'Session.Add("Collection", "")
                ' Session.Add("TranType", "JV")

                Dim adjcolno As String
                adjcolno = objUtils.GetAutoDocNoWTnew(Session("dbconnectionName"), "ADJCOL")
                txtAdjcolno.Value = adjcolno
                Session.Add("Collection" & ":" & adjcolno, "")


                ViewState.Add("JournalRefCode", Request.QueryString("RefCode"))

                txtJDate.Text = Format(objDateTime.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy")
                txtTDate.Text = txtJDate.Text
                txtTranType.Text = CType(ViewState("JournalTranType"), String)

                'Base Cuurncy
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
                lblBaseTot.Text = txtbasecurr.Value & " Total"
                lblBaseDiff.Text = txtbasecurr.Value & " Diff."
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                strSqlQry = "select  narration,narration from narration where active=1"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNarration, "narration", "narration", strSqlQry, True)


                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master ")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If


                If ViewState("JournalState") = "New" Then
                    hdnRows.Value = 0
                    HideControls()
                    txtDocNo.Value = ""
                    lblHeading.Text = "Add " & lblHeading.Text
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("JournalState") = "Copy" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ""
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    lblHeading.Text = "Copy " & lblHeading.Text
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("JournalState") = "Edit" Then
                    txtDocNo.Value = ViewState("JournalRefCode")

                    hdnRows.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from exchange_detail Where tran_id='" & ViewState("JournalRefCode") & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "'")

                    HideControls()
                    lblHeading.Text = "Edit " & lblHeading.Text
                    btnSave.Text = "Update"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("JournalState") = "Delete" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    GrandToatal()

                    lblHeading.Text = "Delete " & lblHeading.Text
                    btnSave.Text = "Delete"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                ElseIf ViewState("JournalState") = "View" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    GrandToatal()
                    lblHeading.Text = "View " & lblHeading.Text
                ElseIf ViewState("JournalState") = "Cancel" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    lblHeading.Text = "Cancel " & lblHeading.Text
                    btnSave.Text = "Cancel"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Cancel')==false)return false;")
                ElseIf ViewState("JournalState") = "UndoCancel" Then
                    hdnRows.Value = 0
                    txtDocNo.Value = ViewState("JournalRefCode")
                    show_record(ViewState("JournalRefCode"))
                    ShowFillGrid(ViewState("JournalRefCode"))
                    lblHeading.Text = "Undo Cancel " & lblHeading.Text
                    btnSave.Text = "Undo"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Undocancel')==false)return false;")

                End If
                CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType("Accounts Module", String), "AccountsModule\JournalSearch.aspx")

                Disabled_Control()
                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                ddlNarration.Attributes.Add("onchange", "javascript:FillCombotoText('" + CType(ddlNarration.ClientID, String) + "','" + CType(txtnarration.ClientID, String) + "')")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlNarration.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Exchange.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = True Then

            Allowanyway()
            If Session("Allowanyway") = "Yes" Then
                chkadjust.Visible = False
            End If


            FillGrid()
            FillGridConvRate()

            txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            txtDivCode.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)

            Dim txtcr, txtbasecr, txtdr, txtbasedr, txtcrate As HtmlInputText
            Dim rowind As Integer

            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AdjBillWindowPostBack") Then
                rowind = Val(Session("LineNo")) - 1

                txtcr = grdJournal.Rows(rowind).FindControl("txtCredit")
                txtbasecr = grdJournal.Rows(rowind).FindControl("txtBaseCredit")
                txtdr = grdJournal.Rows(rowind).FindControl("txtDebit")
                txtbasedr = grdJournal.Rows(rowind).FindControl("txtBaseDebit")
                txtcrate = grdJournal.Rows(rowind).FindControl("txtConvRate")
                If Session("Gridtype") = "Debit" Then
                    txtdr.Value = Session("AmountAdjusted")
                    txtbasedr.Value = Session("BaseAmountAdjusted")
                    txtcrate.Value = CType(DecRound(txtbasedr.Value) / DecRound(txtdr.Value), Decimal)
                ElseIf Session("Gridtype") = "Credit" Then
                    txtcr.Value = Session("AmountAdjusted")
                    txtbasecr.Value = Session("BaseAmountAdjusted")
                    txtcrate.Value = CType(DecRound(txtbasecr.Value) / DecRound(txtcr.Value), Decimal)
                End If
                GrandToatal()
            End If
        Else
            If ViewState("JournalState") = "New" Then
                creategrid()
            ElseIf ViewState("JournalState") = "Edit" Then
                Dim norows As Integer = 1
                fillDategrd(grdJournal, False, norows)
                btnGenGrid.Enabled = False
                ShowControls()
                show_record(txtDocNo.Value.Trim())
                ShowFillGrid(txtDocNo.Value.Trim())
                GrandToatal()
            End If
        End If
    End Sub
#End Region

    Private Sub GrandToatal()
        Dim gvrow As GridViewRow
        Dim txtbasecr, txtbasedr As HtmlInputText
        Dim totalcr, totaldr As Decimal
        For Each gvrow In grdJournal.Rows
            txtbasecr = gvrow.FindControl("txtBaseCredit")
            totalcr = totalcr + CType(Val(txtbasecr.Value), Decimal)
            txtbasedr = gvrow.FindControl("txtBaseDebit")
            totaldr = totaldr + CType(Val(txtbasedr.Value), Decimal)

        Next
        txtTotBaseCredit.Value = DecRound(totalcr)
        txtTotBaseDebit.Value = DecRound(totaldr)

    End Sub
    Private Sub FillGridConvRate()
        Dim strQry As String = ""
        Dim gvrow As GridViewRow
        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim txtCurrRate As HtmlInputText

        For Each gvrow In grdJournal.Rows
            txtCurrRate = gvrow.FindControl("txtConvRate")
            ddlAccType = gvrow.FindControl("ddlType")
            ddlgAccCode = gvrow.FindControl("ddlgAccCode")

            'strQry = "select cur,convrate,controlacctcode from  view_account left outer join " & _
            '                   " currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "' and type='" & ddlAccType.Items(ddlAccType.SelectedIndex).Text & "' "
            If ddlAccType.Value <> "[Select]" And ddlgAccCode.Value <> "[Select]" Then
                If Val(txtCurrRate.Value) = 0 Then
                    strQry = "select convrate from  view_account left outer join " & _
                                    " currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "' and type='" & ddlAccType.Value & "' "
                    txtCurrRate.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)
                End If
            End If
        Next

    End Sub
#Region "Public Sub Disabled_Control()"
    Public Sub Disabled_Control()
        txtDocNo.Disabled = True
        If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Copy" Then
            btnPrint.Visible = False
        ElseIf ViewState("JournalState") = "Edit" Then
            btnPrint.Visible = False
        ElseIf ViewState("JournalState") = "Delete" Or ViewState("JournalState") = "View" Or ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
            btnPrint.Visible = False
            txtnarration.Disabled = True
            txtReference.Disabled = True
            txtJDate.Enabled = False
            txtTDate.Enabled = False
            btnAdd.Visible = False
            btnDelLine.Visible = False

            btnSave.Visible = False
            chkPost.Visible = False
            DisableGrid()
        End If
        If ViewState("JournalState") = "View" Then
            btnPrint.Visible = True
        ElseIf ViewState("JournalState") = "Delete" Then
            btnSave.Visible = True
        ElseIf ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
            btnPrint.Visible = False
            btnSave.Visible = True
        End If
    End Sub
    Private Sub DisableGrid()
        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect
        Dim txtNarr As HtmlInputControl
        Dim txtCredit As HtmlInputControl
        Dim txtDebit As HtmlInputControl
        Dim txtCurrCode As HtmlInputControl
        Dim txtCurrRate As HtmlInputControl
        Dim txtBaseCredit As HtmlInputText
        Dim txtBaseDebit As HtmlInputText
        Dim btnBill As HtmlInputButton

        For Each gvRow In grdJournal.Rows
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")
            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")
            txtNarr = gvRow.FindControl("txtgnarration")
            btnBill = gvRow.FindControl("btnAd")

            btnBill.Disabled = False
            ddlAccType.Disabled = True
            ddlgAccCode.Disabled = True
            ddlgAccName.Disabled = True
            ddlConAccCode.Disabled = True
            ddlConAccName.Disabled = True
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            txtCurrCode.Disabled = True
            txtCurrRate.Disabled = True
            txtCredit.Disabled = True
            txtDebit.Disabled = True
            txtNarr.Disabled = True
        Next

    End Sub
#End Region

#Region "Public Function Allowanyway() As Boolean"
    Public Function Allowanyway()
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            strSql = "select appid from group_privilege_Detail where (privilegeid='2' and appid='4')  and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            myCommand = New SqlCommand(strSql, SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Allowanyway", "Yes")
            Else
                Session.Add("Allowanyway", "No")
            End If
            Allowanyway = ""
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close 
            Allowanyway = ""
        End Try
    End Function
#End Region

#Region "Public Sub show_record()"
    Public Sub show_record(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            chkPost.Checked = False
            Dim myDS As New DataSet
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("select * from exchange_master Where tran_id='" & RefCode & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("post_state")) = False And CType(ViewState("JournalState"), String) <> "Copy" Then
                        If CType(mySqlReader("post_state"), String) = "P" Then
                            lblPostmsg.Text = "Posted"
                            lblPostmsg.ForeColor = Drawing.Color.Red
                            chkPost.Checked = True
                        Else
                            lblPostmsg.Text = "UnPosted"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If

                    If IsDBNull(mySqlReader("cancel_state")) = False Then
                        If CType(mySqlReader("cancel_state"), String) = "Y" Then
                            lblPostmsg.Text = "Cancelled"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    End If


                    If IsDBNull(mySqlReader("tran_id")) = False Then
                        If ViewState("JournalState") = "Copy" Then
                            txtDocNo.Value = ""
                        Else
                            txtDocNo.Value = CType(mySqlReader("tran_id"), String)
                        End If
                    End If

                    If IsDBNull(mySqlReader("journal_date")) = False Then
                        txtJDate.Text = Format(mySqlReader("journal_date"), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("journal_tran_date")) = False Then
                        txtTDate.Text = Format(mySqlReader("journal_tran_date"), "dd/MM/yyyy")
                    End If
                    txtReference.Value = CType(mySqlReader("journal_mrv"), String)
                    txtnarration.Value = CType(mySqlReader("journal_narration"), String)
                    txtTranType.Text = CType(ViewState("JournalTranType"), String)

                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub
    Private Sub ShowFillGrid(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            Dim ddlAccType As HtmlSelect
            Dim ddlgAccCode As HtmlSelect
            Dim ddlgAccName As HtmlSelect
            Dim ddlConAccCode As HtmlSelect
            Dim ddlConAccName As HtmlSelect
            Dim ddlCCCode As HtmlSelect
            Dim ddlCCName As HtmlSelect
            Dim txtNarr, txtCredit, txtDebit, txtCurrCode, txtCurrRate, txtBaseCredit, txtBaseDebit, lblno, txtOldLineno As HtmlInputText
            Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname As HtmlInputText
            Dim sqlstr1, sqlstr2 As String
            Dim lngCnt As Long
            Dim credittot, debittot, basecredittot, basedebittot As Decimal
            If hdnRows.Value < txtNoofRows.Value Then
                lngCnt = CType(hdnRows.Value, Integer) + CType(txtNoofRows.Value, Integer)  'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"journal_detail", "count(tran_id)", "tran_id", RefCode)
            Else
                lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "exchange_detail", "count(tran_id)", "tran_id", RefCode)
            End If
            If lngCnt <= 5 Then lngCnt = 2
            fillDategrd(grdJournal, False, lngCnt)
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "Select * from exchange_detail Where tran_id='" & RefCode & "' and  tran_type='" & CType(ViewState("JournalTranType"), String) & "' order by tran_lineno"
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = myCommand.ExecuteReader()

            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdJournal.Rows
                        lblno = gvRow.FindControl("txtlineno")
                        If mySqlReader("tran_lineno") = CType(lblno.Value, Integer) Then
                            txtOldLineno = gvRow.FindControl("txtOldLineno")
                            ddlAccType = gvRow.FindControl("ddlType")
                            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
                            ddlgAccName = gvRow.FindControl("ddlgAccName")
                            ddlCCCode = gvRow.FindControl("ddlCostCode")
                            ddlCCName = gvRow.FindControl("ddlCostName")
                            txtCurrCode = gvRow.FindControl("txtCurrency")
                            txtCurrRate = gvRow.FindControl("txtConvRate")
                            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
                            ddlConAccName = gvRow.FindControl("ddlConAccName")
                            txtDebit = gvRow.FindControl("txtDebit")
                            txtCredit = gvRow.FindControl("txtCredit")

                            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

                            txtNarr = gvRow.FindControl("txtgnarration")

                            txtacctcode = gvRow.FindControl("txtacctcode")
                            txtacctname = gvRow.FindControl("txtacctname")
                            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
                            txtcontrolacname = gvRow.FindControl("txtcontrolacname")

                            txtOldLineno.Value = lblno.Value

                            ddlAccType.Value = mySqlReader("journal_acc_type").ToString.Trim

                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where type = '" & ddlAccType.Value & "'   order by code", True)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select Code,des from view_account where type = '" & ddlAccType.Value & "'  order by code", True)


                            ddlgAccName.Value = mySqlReader("journal_acc_code").ToString
                            ddlgAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select des from view_account where type = '" & ddlAccType.Value & "' and code ='" & mySqlReader("journal_acc_code").ToString & "' ")

                            txtacctname.Value = mySqlReader("journal_acc_code").ToString
                            txtacctcode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select des from view_account where type = '" & ddlAccType.Value & "' and code ='" & mySqlReader("journal_acc_code").ToString & "' ")

                            ddlConAccCode.Disabled = False
                            ddlConAccName.Disabled = False
                            ddlCCCode.Disabled = True
                            ddlCCName.Disabled = True
                            txtCurrRate.Disabled = False

                            If ddlAccType.Value = "G" Then
                                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                                ddlConAccCode.Disabled = True
                                ddlConAccName.Disabled = True
                                ddlCCCode.Disabled = False
                                ddlCCName.Disabled = False
                            ElseIf ddlAccType.Value = "C" Then
                                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  view_account.controlacctcode"
                                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  acctmast.acctname"
                            ElseIf ddlAccType.Value = "S" Then
                                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' union all  select distinct partymast.accrualacctcode controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' order by controlacctcode"
                                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' union all  select distinct acctmast.acctname ,partymast.accrualacctcode controlacctcode      from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "'  order by acctmast.acctname"
                            ElseIf ddlAccType.Value = "A" Then
                                sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' union all  select distinct supplier_agents.accrualacctcode controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by controlacctcode"
                                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' union all select distinct acctmast.acctname ,supplier_agents.accrualacctcode controlacctcode     from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by acctmast.acctname"
                            End If
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

                            If ddlAccType.Value <> "G" Then
                                ddlConAccName.Value = mySqlReader("journal_gl_code").ToString
                                ddlConAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctcode ='" & mySqlReader("journal_gl_code").ToString & "' ")
                                txtcontrolacname.Value = mySqlReader("journal_gl_code").ToString
                                txtctrolaccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctcode ='" & mySqlReader("journal_gl_code").ToString & "' ")
                            Else
                                ddlConAccName.Value = "[select]"
                                ddlConAccCode.Value = "[select]"
                                txtctrolaccode.Value = ""
                                txtcontrolacname.Value = ""
                            End If
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

                            ddlCCCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select costcenter_name from costcenter_master where costcenter_code ='" & mySqlReader("costcenter_code").ToString & "' ")
                            ddlCCName.Value = mySqlReader("costcenter_code").ToString




                            'Dim glCode As String
                            'glCode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select isnull(controlacctcode,0) from dbo.view_account where type='" & ddlAccType.Value & "'and code='" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "'")
                            'If ddlAccType.Value = "G" Then
                            '    ddlgControlAcc.Value = "Select"
                            'Else
                            '    ddlgControlAcc.Value = mySqlReader("journal_acc_code").ToString
                            'End If


                            txtCurrCode.Value = mySqlReader("journal_currency_id").ToString
                            txtCurrRate.Value = mySqlReader("journal_currency_rate").ToString
                            txtNarr.Value = mySqlReader("journal_narration").ToString

                            txtCredit.Value = DecRound(mySqlReader("journal_credit").ToString)
                            txtDebit.Value = DecRound(mySqlReader("journal_debit").ToString)

                            txtBaseCredit.Value = DecRound(mySqlReader("basecredit").ToString)
                            txtBaseDebit.Value = DecRound(mySqlReader("basedebit").ToString)


                            credittot = CType(Val(credittot), Decimal) + CType(Val(txtCredit.Value), Decimal)
                            debittot = CType(Val(debittot), Decimal) + CType(Val(txtDebit.Value), Decimal)
                            basecredittot = CType(Val(basecredittot), Decimal) + CType(Val(txtBaseCredit.Value), Decimal)
                            basedebittot = CType(Val(basedebittot), Decimal) + CType(Val(txtBaseDebit.Value), Decimal)

                            If ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Then
                                If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                                    txtCurrRate.Disabled = True
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If
            txtTotalCredit.Value = DecRound(credittot)
            txtTotalDebit.Value = DecRound(debittot)
            txtTotBaseCredit.Value = DecRound(credittot)
            txtTotBaseDebit.Value = DecRound(credittot)

            mySqlReader.Close()
            SqlConn.Close()

            adjust_bills()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Sub
#End Region
#Region "Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)"
    Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)
        If colexists(dataCollection, strKey) = False Then
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        Else
            dataCollection.Remove(strKey)
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        End If
    End Sub
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 10
        Else
            lngcnt = count
        End If
        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
        txtgridrows.Value = grd.Rows.Count
    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("LineNo", GetType(Integer)))

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
#Region "Protected Sub grdAcc_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAcc.RowDataBound"
    Protected Sub grdJournal_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdJournal.RowDataBound

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect
        Dim txtauto As HtmlInputText

        Dim txtCurrCode As HtmlInputText
        Dim txtCurrRate As HtmlInputText
        Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno As HtmlInputText
        Dim btnBill As HtmlInputButton
        Dim txtgnarration As HtmlInputText
        Dim txthid As HtmlInputText

        Dim lblno As HtmlInputText
        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname As HtmlInputText

        Dim strOpti, sqlstr1, sqlstr2 As String
        Dim i As Integer = 0
        gvRow = e.Row

        gvRow = e.Row
        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.BaseDebit).Text = strOpti & " Debit"
            gvRow.Cells(grd_col.BaseCredit).Text = strOpti & " Credit"
            Exit Sub
        End If
        txtauto = gvRow.FindControl("accSearch")
        txtacctcode = gvRow.FindControl("txtacctcode")
        txtacctname = gvRow.FindControl("txtacctname")
        txtctrolaccode = gvRow.FindControl("txtctrolaccode")
        txtcontrolacname = gvRow.FindControl("txtcontrolacname")


        ddlAccType = gvRow.FindControl("ddlType")
        ddlgAccCode = gvRow.FindControl("ddlgAccCode")
        ddlgAccName = gvRow.FindControl("ddlgAccName")
        txtCurrCode = gvRow.FindControl("txtCurrency")
        txtCurrRate = gvRow.FindControl("txtConvRate")

        ddlConAccCode = gvRow.FindControl("ddlConAccCode")
        ddlConAccName = gvRow.FindControl("ddlConAccName")
        ddlCCCode = gvRow.FindControl("ddlCostCode")
        ddlCCName = gvRow.FindControl("ddlCostName")

        txtDebit = gvRow.FindControl("txtDebit")
        txtCredit = gvRow.FindControl("txtCredit")
        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
        txtBaseCredit = gvRow.FindControl("txtBaseCredit")
        btnBill = gvRow.FindControl("btnAd")
        lblno = gvRow.FindControl("txtlineno")
        txtOldLineno = gvRow.FindControl("txtOldLineno")
        txtgnarration = gvRow.FindControl("txtgnarration")
        txthid = gvRow.FindControl("txthid")


        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccType, "acc_type_des", "acc_type_name", "select acc_type_des,acc_type_name from  acc_type_master where acc_type_mode<>'G' order by acc_type_name", True)

        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select top 10 Code,des from view_account   order by code", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select top 10 Code,des from view_account  order by des", True)

        sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
        sqlstr2 = " select  '' as acctname , '' as controlacctcode "

        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)


        txtauto.Attributes.Add("onfocus", "javascript:OnChangeType('" + CType(txtauto.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "')")


        ddlAccType.Attributes.Add("onchange", "javascript:fill_acountcode('" + CType(ddlAccType.ClientID, String) + "','" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(txtgnarration.ClientID, String) + "','" + CType(ddlCCCode.ClientID, String) + "','" + CType(ddlCCName.ClientID, String) + "','" + CType(txthid.ClientID, String) + "')")
        ddlgAccCode.Attributes.Add("onchange", "javascript:FillGACode('" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtacctcode.ClientID, String) + "','" + CType(txtacctname.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "')")
        ddlgAccName.Attributes.Add("onchange", "javascript:FillGAName('" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtacctcode.ClientID, String) + "','" + CType(txtacctname.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "')")

        ddlConAccCode.Attributes.Add("onchange", "javascript:FillCTCode('" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "')")
        ddlConAccName.Attributes.Add("onchange", "javascript:FillCTName('" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "')")



        txtBaseDebit.Attributes.Add("onchange", "javascript:baseconvertInRate()")
        txtBaseCredit.Attributes.Add("onchange", "javascript:baseconvertInRate()")


        ddlCCCode.Attributes.Add("onchange", "javascript:FillCodeName( '" + CType(ddlCCCode.ClientID, String) + "','" + CType(ddlCCName.ClientID, String) + "')")
        ddlCCName.Attributes.Add("onchange", "javascript:FillCodeName('" + CType(ddlCCName.ClientID, String) + "','" + CType(ddlCCCode.ClientID, String) + "')")

        'txtBaseDebit.Attributes.Add("onchange", "javascript:convertRateOnBaseCurrency('" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Debit", String) + "')")
        'txtBaseCredit.Attributes.Add("onchange", "javascript:convertRateOnBaseCurrency('" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Credit", String) + "')")


        'TextLockhtml(txtBaseDebit)
        'TextLockhtml(txtBaseCredit)
        NumbersHtml(txtBaseDebit)
        NumbersHtml(txtBaseCredit)
        NumbersHtml(txtCurrRate)
        NumbersHtml(txtCurrRate)
        NumbersDecimalRoundHtml(txtCredit)
        NumbersDecimalRoundHtml(txtDebit)

        Dim typ As Type
        typ = GetType(DropDownList)
        ' If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
        Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
        ddlAccType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlgAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlgAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlCCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlCCName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'End If

    End Sub
#End Region
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("JournalState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim intRow As Integer
        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim gvRow1 As GridViewRow
        Dim chksel As CheckBox
        Dim lbltranid As Label
        Dim lbltranlineno As Label
        Dim lbltrantype As Label

        Dim ddlCCCode As HtmlSelect
        Dim txtNarr, txtCredit, txtDebit, txtCurrCode, txtCurrRate, txtBaseCredit, txtBaseDebit, lblno As HtmlInputText
        Dim sqlTrans As SqlTransaction
        Dim strdiv, strcostcentercode As String


        strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
        Try
            If Page.IsValid = True Then
                If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Or ViewState("JournalState") = "UndoCancel" Then
                    If validate_page() = False Then
                        Exit Sub
                    End If
                End If

                If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Delete" Or ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
                    If Validateseal() = False Then
                        Exit Sub
                    End If
                End If


                strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                strcostcentercode = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 510)
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = SqlConn.BeginTransaction
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If

                If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Edit" Or ViewState("JournalState") = "Copy" Then
                    If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Copy" Then
                        Dim optionval As String
                        'optionval = objUtils.GetAutoDocNo(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans)
                        'txtDocNo.Value = optionval.Trim

                        optionval = objUtils.GetAutoDocNo(CType(ViewState("JournalTranType"), String), SqlConn, sqlTrans)

                        txtDocNo.Value = optionval.Trim
                        myCommand = New SqlCommand("sp_journal_master_exdiff", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("JournalState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_journal_master_exdiff", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If


                    myCommand.Parameters.Add(New SqlParameter("@journal_div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@journal_date ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtJDate.Text)
                    myCommand.Parameters.Add(New SqlParameter("@journal_tran_date ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtTDate.Text)
                    myCommand.Parameters.Add(New SqlParameter("@journal_mrv", SqlDbType.VarChar, 10)).Value = txtReference.Value.Trim
                    myCommand.Parameters.Add(New SqlParameter("@journal_salesperson_code", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@journal_narration", SqlDbType.VarChar, 200)).Value = txtnarration.Value
                    myCommand.Parameters.Add(New SqlParameter("@journal_tran_state", SqlDbType.VarChar, 1)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objDateTime.GetSystemDateTime(Session("dbconnectionName"))

                    If txtTotBaseDebit.Value <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(txtTotBaseDebit.Value, Double)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = Decimal.Zero
                    End If

                    If txtTotBaseCredit.Value <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(txtTotBaseCredit.Value, Double)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = Decimal.Zero
                    End If

                    If chkPost.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If


                    myCommand.ExecuteNonQuery()

                    If ViewState("JournalState") = "Edit" Then
                        myCommand = New SqlCommand("sp_del_transaction", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                        myCommand.ExecuteNonQuery()
                    End If


                    'Save In Detail Table
                    'Save In Detail Table
                    For Each gvRow In grdJournal.Rows

                        lblno = gvRow.FindControl("txtlineno")
                        If Val(lblno.Value) = 0 Then
                            lblno.Value = intRow
                        End If

                        ddlAccType = gvRow.FindControl("ddlType")
                        ddlgAccCode = gvRow.FindControl("ddlgAccCode")
                        ddlgAccName = gvRow.FindControl("ddlgAccName")
                        ddlConAccCode = gvRow.FindControl("ddlConAccCode")
                        ddlCCCode = gvRow.FindControl("ddlCostCode")
                        txtCurrCode = gvRow.FindControl("txtCurrency")
                        txtCurrRate = gvRow.FindControl("txtConvRate")
                        txtDebit = gvRow.FindControl("txtDebit")
                        txtCredit = gvRow.FindControl("txtCredit")
                        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                        txtBaseCredit = gvRow.FindControl("txtBaseCredit")
                        txtNarr = gvRow.FindControl("txtgnarration")

                        If ddlAccType.Value <> "[Select]" And ddlgAccCode.Value <> "[Select]" Then
                            intRow = 1 + intRow
                            myCommand = New SqlCommand("sp_journal_detail_exdiff", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure

                            myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = intRow
                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)

                            myCommand.Parameters.Add(New SqlParameter("@journal_acc_type", SqlDbType.VarChar, 1)).Value = ddlAccType.Value
                            myCommand.Parameters.Add(New SqlParameter("@journal_acc_code", SqlDbType.VarChar, 20)).Value = ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text

                            If ddlAccType.Value <> "G" Then
                                If ddlConAccCode.Value <> "[Select]" Then
                                    myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                End If
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@journal_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@journal_group", SqlDbType.VarChar, 1)).Value = DBNull.Value
                            myCommand.Parameters.Add(New SqlParameter("@journal_narration", SqlDbType.VarChar, 200)).Value = txtNarr.Value.Trim
                            myCommand.Parameters.Add(New SqlParameter("@journal_currency_id", SqlDbType.VarChar, 20)).Value = txtCurrCode.Value.Trim
                            myCommand.Parameters.Add(New SqlParameter("@journal_currency_rate", SqlDbType.Decimal, 18.12)).Value = CType(txtCurrRate.Value, Decimal)

                            myCommand.Parameters.Add(New SqlParameter("@journal_debit", SqlDbType.Money)).Value = CType(Val(txtDebit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@journal_credit", SqlDbType.Money)).Value = CType(Val(txtCredit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(Val(txtBaseDebit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(Val(txtBaseCredit.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv

                            If ddlAccType.Value = "G" Then
                                If ddlCCCode.Value <> "[Select]" Then
                                    myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = ddlCCCode.Items(ddlCCCode.SelectedIndex).Text
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = strcostcentercode
                                End If
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If

                            myCommand.ExecuteNonQuery()



                            'For Accounts Posting
                            If chkPost.Checked = True Then
                                'inserting into acc_tran
                                myCommand = New SqlCommand("sp_acc_tran_exdiff", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                myCommand.Parameters.Add(New SqlParameter("@acc_tran_lineno", SqlDbType.Int)).Value = intRow
                                myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                                myCommand.Parameters.Add(New SqlParameter("@acc_tran_date ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtJDate.Text)
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = ddlAccType.Value
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text
                                If ddlAccType.Value <> "G" Then
                                    If ddlConAccCode.Value <> "[Select]" Then
                                        myCommand.Parameters.Add(New SqlParameter("@gl_code", SqlDbType.VarChar, 20)).Value = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@gl_code", SqlDbType.VarChar, 20)).Value = String.Empty
                                    End If
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@gl_code", SqlDbType.VarChar, 20)).Value = String.Empty
                                End If
                                myCommand.Parameters.Add(New SqlParameter("@acc_narration", SqlDbType.VarChar, 200)).Value = txtNarr.Value.Trim
                                myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = txtCurrCode.Value.Trim
                                myCommand.Parameters.Add(New SqlParameter("@acc_currency_rate", SqlDbType.Decimal, 18.12)).Value = CType(txtCurrRate.Value, Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@acc_group", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                myCommand.Parameters.Add(New SqlParameter("@acc_tran_state", SqlDbType.VarChar, 1)).Value = ddlAccType.Value
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.ExecuteNonQuery()

                                'Insert records into acc_subtran
                                myCommand = New SqlCommand("sp_acc_sub_tran_exdiff", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                If ddlAccType.Value = "G" Then
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_lineno", SqlDbType.Int)).Value = intRow
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                                Else
                                    For Each gvRow1 In GVCollection.Rows
                                        chksel = gvRow1.FindControl("chkSel")
                                        If chksel.Checked = True Then
                                            lbltranid = gvRow1.FindControl("lbltranid")
                                            lbltranlineno = gvRow1.FindControl("lbltranlineno")
                                            lbltrantype = gvRow1.FindControl("lbltrantype")
                                            myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = CType(lbltranid.Text, String)
                                            myCommand.Parameters.Add(New SqlParameter("@acc_tran_lineno", SqlDbType.Int)).Value = CType(lbltranlineno.Text, Integer)
                                            myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 10)).Value = CType(lbltrantype.Text, String)
                                        End If
                                    Next
                                End If

                                myCommand.Parameters.Add(New SqlParameter("@acc_against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                myCommand.Parameters.Add(New SqlParameter("@acc_against_tran_lineno", SqlDbType.Int)).Value = intRow
                                myCommand.Parameters.Add(New SqlParameter("@acc_against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                                myCommand.Parameters.Add(New SqlParameter("@acc_div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@acc_base_debit", SqlDbType.Money)).Value = CType(Val(txtBaseDebit.Value), Double)
                                myCommand.Parameters.Add(New SqlParameter("@acc_base_credit", SqlDbType.Money)).Value = CType(Val(txtBaseCredit.Value), Double)

                                If ddlAccType.Value = "G" Then
                                    If ddlCCCode.Value <> "[Select]" Then
                                        myCommand.Parameters.Add(New SqlParameter("@costcentercode", SqlDbType.VarChar, 20)).Value = ddlCCCode.Items(ddlCCCode.SelectedIndex).Text
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@costcentercode", SqlDbType.VarChar, 20)).Value = strcostcentercode
                                    End If
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@costcentercode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                End If

                                myCommand.Parameters.Add(New SqlParameter("@acc_currency_rate", SqlDbType.Decimal, 18.12)).Value = CType(txtCurrRate.Value, Decimal)
                                myCommand.ExecuteNonQuery()
                            End If
                        End If
                    Next

                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        lblPostmsg.Text = "Posted"
                        lblPostmsg.ForeColor = Drawing.Color.Red
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If


                ElseIf ViewState("JournalState") = "Delete" Then
                    'Delete Record
                    myCommand = New SqlCommand("sp_del_transaction", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.ExecuteNonQuery()


                    myCommand = New SqlCommand("sp_del_exchange", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.ExecuteNonQuery()

                ElseIf ViewState("JournalState") = "Cancel" Then

                    'Cancel Record
                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "exchange_detail"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_cancel_journal_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_cancel_journal", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged ", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                ElseIf ViewState("JournalState") = "UndoCancel" Then
                    myCommand = New SqlCommand("sp_undocancel_journal", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("JournalTranType"), String)
                    myCommand.ExecuteNonQuery()


                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                If ViewState("JournalState") = "Delete" Or ViewState("JournalState") = "Cancel" Or ViewState("JournalState") = "UndoCancel" Then
                    '  Response.Redirect("JournalSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('ExchangeWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                Else
                    If ViewState("JournalState") = "New" Or ViewState("JournalState") = "Copy" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully');", True)
                    ElseIf ViewState("JournalState") = "Edit" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record update successfully');", True)
                    End If

                    btnPrint.Visible = True
                    ViewState("JournalState") = "View"
                    Disabled_Control()
                    'btnPrint_Click(sender, e)  

                End If
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Public Function Validateseal() As Boolean
        Try
            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtJDate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Exchange.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean"
    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
        Try
            Dim k As Integer
            colexists = False
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
                    If newcol.Contains(newkey) = True Then
                        colexists = True
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            colexists = False
        End Try
    End Function
#End Region
#Region "Public Function validate_page() As Boolean"
    Public Function validate_page() As Boolean
        validate_page = True

        If txtTDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter transaction date.');", True)
            SetFocus(txtTDate)
            validate_page = False
            Exit Function
        End If
        If txtJDate.Text = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter journal date .');", True)
            SetFocus(txtJDate)
            validate_page = False
            Exit Function
        End If


        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim txtCredit, txtDebit, txtCurrCode, txtCurrRate, lblno, txtBaseCredit, txtBaseDebit As HtmlInputText
        Dim baseamt As Decimal
        Dim dfalg As Boolean = True
        Dim gvrow1 As GridViewRow
        Dim chkSel As CheckBox
        Dim i As Integer
        For Each gvRow In grdJournal.Rows

            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            lblno = gvRow.FindControl("txtlineno")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            If ddlAccType.Value.Trim <> "[Select]" Or txtCurrRate.Value.Trim <> "" Or ddlgAccName.Value.Trim <> "[Select]" Then
                dfalg = False
                If ddlAccType.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account type.');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If
                If ddlgAccName.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account code.');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If

                If txtCurrCode.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select currancy code.');", True)
                    SetFocus(txtCurrCode)
                    validate_page = False
                    Exit Function
                End If
                If Val(txtCurrRate.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid exachange rate.');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If
                'If Val(txtDebit.Value) <= 0 And Val(txtCredit.Value) <= 0 Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter debit or credit amount.');", True)
                '    SetFocus(txtDebit)
                '    validate_page = False
                '    Exit Function
                'End If
                If Val(txtBaseDebit.Value) = 0 And Val(txtBaseCredit.Value) = 0 Then
                    Dim strMsg As String = ""
                    Dim basecurr As String = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                    strMsg = "Both " & basecurr & " Debit amount and " & basecurr & " Credit amount can not be zero."
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)
                    SetFocus(txtBaseDebit)
                    validate_page = False
                    Exit Function
                End If

                'If Val(txtDebit.Value) > 0 Then
                '    If Not txtBaseDebit.Value = CType(txtDebit.Value, Decimal) * CType(txtCurrRate.Value, Decimal) Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter correct base debit amount.');", True)
                '        SetFocus(txtBaseDebit)
                '        validate_page = False
                '        Exit Function
                '    End If
                'End If

                'If Val(txtCredit.Value) > 0 Then
                '    If Not txtBaseCredit.Value = CType(txtCredit.Value, Decimal) * CType(txtCurrRate.Value, Decimal) Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter correct base credit amount.');", True)
                '        SetFocus(txtBaseCredit)
                '        validate_page = False
                '        Exit Function
                '    End If
                'End If
                i = 1
                If ddlAccType.Value <> "G" Then
                    For Each gvrow1 In GVCollection.Rows
                        chkSel = gvrow1.FindControl("chkSel")
                        If chkSel.Checked = True Then
                            i = 0
                            Exit For
                        End If
                    Next
                Else
                    i = 0
                End If

                If i > 0 And ViewState("JournalState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Adjust bill for Client and supplier account .');", True)
                    validate_page = False
                    Exit Function
                End If

                If ddlAccType.Value <> "G" Then
                    If ddlConAccCode.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Control account code.');", True)
                        SetFocus(ddlAccType)
                        validate_page = False
                        Exit Function
                    End If
                    If Val(txtBaseDebit.Value) <> 0 Then
                        baseamt = DecRound(CType(txtBaseDebit.Value, Decimal))
                    Else
                        baseamt = DecRound(CType(txtBaseCredit.Value, Decimal))
                    End If
                End If
            End If

        Next

        If chkPost.Checked = False Then
            validate_page = True
        Else
            If dfalg = True Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter more than one journal voucher in grid.');", True)
                validate_page = False
                Exit Function
            End If
        End If

        'Total of debit and credit in base amount should be equal

        If Val(txtTotBaseCredit.Value) <> Val(txtTotBaseDebit.Value) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total of debit and credit in base amount should be equal.');", True)
            validate_page = False
            Exit Function
        End If

    End Function
#End Region
#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Response.Redirect("JournalSearch.aspx", False)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Session.Remove("Collection" & ":" & txtAdjcolno.Value)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExchangeWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub
#End Region
#Region "Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click"
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = grdJournal.Rows.Count + 1
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String
        Dim ckDeletion(count) As String

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect
        Dim txtCredit, txtCurrCode, txtCurrRate, txtNarr, txtBaseCredit, txtOldLineno, txtDebit As HtmlInputText
        Dim lblno As HtmlInputText
        Dim chckDeletion As CheckBox

        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname, txtBaseDebit As HtmlInputText
        Dim sqlstr1, sqlstr2 As String

        For Each gvRow In grdJournal.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            'ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            'ddlgAccName = gvRow.FindControl("ddlgAccName")

            'ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            'ddlConAccName = gvRow.FindControl("ddlConAccName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")
            If txtacctcode.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
                If chckDeletion.Checked = True Then
                    ckDeletion(n) = 1
                Else
                    ckDeletion(n) = 0
                End If
                Olineno(n) = txtOldLineno.Value
                acctype(n) = ddlAccType.Value
                acccode(n) = txtacctcode.Value
                accname(n) = txtacctname.Value 'ddlgAccName.Value
                controlcode(n) = txtctrolaccode.Value
                controlname(n) = txtcontrolacname.Value
                CCCode(n) = CType(ddlCCCode.Value, String)
                CCName(n) = CType(ddlCCName.Value, String)

                currcode(n) = txtCurrCode.Value
                crate(n) = txtCurrRate.Value

                credit(n) = txtCredit.Value
                debit(n) = txtDebit.Value
                bcredit(n) = txtBaseCredit.Value
                bdebit(n) = txtBaseDebit.Value
                narration(n) = txtNarr.Value

                n = n + 1
            End If
        Next

        fillDategrd(grdJournal, False, grdJournal.Rows.Count + 1)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdJournal.Rows
            If n = i Then
                Exit For
            End If
            chckDeletion = gvRow.FindControl("chckDeletion")

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")


            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")


            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")

            If ckDeletion(n) = 1 Then
                chckDeletion.Checked = True
            Else
                chckDeletion.Checked = False
            End If
            txtOldLineno.Value = Olineno(n)
            txtacctcode.Value = acccode(n)
            txtacctname.Value = accname(n)
            txtctrolaccode.Value = controlcode(n)
            txtcontrolacname.Value = controlname(n)


            ddlAccType.Value = acctype(n)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where type = '" & ddlAccType.Value & "'   order by code", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select des,Code from view_account where type = '" & ddlAccType.Value & "'  order by des", True)


            ddlgAccName.Value = accname(n)
            ddlgAccCode.Value = acccode(n)

            ddlConAccCode.Disabled = False
            ddlConAccName.Disabled = False
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            txtCurrRate.Disabled = False

            If acctype(n) = "G" Then
                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                ddlConAccCode.Disabled = True
                ddlConAccName.Disabled = True
                ddlCCCode.Disabled = False
                ddlCCName.Disabled = False
            ElseIf acctype(n) = "C" Then
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            ElseIf acctype(n) = "S" Then
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' union all  select distinct partymast.accrualacctcode controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' union all  select distinct acctmast.acctname ,partymast.accrualacctcode controlacctcode      from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname"
            ElseIf acctype(n) = "A" Then
                sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' union all  select distinct supplier_agents.accrualacctcode controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' union all  select distinct acctmast.acctname ,supplier_agents.accrualacctcode controlacctcode     from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by acctmast.acctname "
            End If
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

            ddlConAccCode.Value = controlcode(n)
            ddlConAccName.Value = controlname(n)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

            ddlCCCode.Value = CCCode(n)
            ddlCCName.Value = CCName(n)
            txtCurrCode.Value = currcode(n)
            txtCurrRate.Value = crate(n)

            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)
            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)

            txtNarr.Value = narration(n)
            If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                txtCurrRate.Disabled = True
            End If

            n = n + 1
        Next

    End Sub
#End Region
    Private Sub FillGrid()
        Dim n As Integer = 0
        Dim count As Integer
        count = grdJournal.Rows.Count
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String
        Dim ckDeletion(count) As String

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect
        Dim txtCredit, txtCurrCode, txtCurrRate, txtNarr, txtBaseCredit, txtOldLineno, txtDebit, txtBaseDebit As HtmlInputText
        Dim lblno As HtmlInputText
        Dim chckDeletion As CheckBox

        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname As HtmlInputText
        Dim sqlstr1, sqlstr2 As String

        For Each gvRow In grdJournal.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            'ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            'ddlgAccName = gvRow.FindControl("ddlgAccName")

            'ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            'ddlConAccName = gvRow.FindControl("ddlConAccName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")
            If txtacctcode.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
                If chckDeletion.Checked = True Then
                    ckDeletion(n) = 1
                Else
                    ckDeletion(n) = 0
                End If
                Olineno(n) = txtOldLineno.Value
                acctype(n) = ddlAccType.Value
                acccode(n) = txtacctcode.Value
                accname(n) = txtacctname.Value 'ddlgAccName.Value
                controlcode(n) = txtctrolaccode.Value
                controlname(n) = txtcontrolacname.Value
                CCCode(n) = CType(ddlCCCode.Value, String)
                CCName(n) = CType(ddlCCName.Value, String)

                currcode(n) = txtCurrCode.Value
                crate(n) = txtCurrRate.Value

                credit(n) = txtCredit.Value
                debit(n) = txtDebit.Value
                bcredit(n) = txtBaseCredit.Value
                bdebit(n) = txtBaseDebit.Value
                narration(n) = txtNarr.Value

                n = n + 1
            End If
        Next

        fillDategrd(grdJournal, False, grdJournal.Rows.Count)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdJournal.Rows
            If n = i Then
                Exit For
            End If
            chckDeletion = gvRow.FindControl("chckDeletion")

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")


            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")


            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")

            If ckDeletion(n) = 1 Then
                chckDeletion.Checked = True
            Else
                chckDeletion.Checked = False
            End If
            txtOldLineno.Value = Olineno(n)
            txtacctcode.Value = acccode(n)
            txtacctname.Value = accname(n)
            txtctrolaccode.Value = controlcode(n)
            txtcontrolacname.Value = controlname(n)


            ddlAccType.Value = acctype(n)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where type = '" & ddlAccType.Value & "'   order by code", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select des,Code from view_account where type = '" & ddlAccType.Value & "'  order by des", True)


            ddlgAccName.Value = accname(n)
            ddlgAccCode.Value = acccode(n)

            ddlConAccCode.Disabled = False
            ddlConAccName.Disabled = False
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            txtCurrRate.Disabled = False
            If acctype(n) = "G" Then
                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                ddlConAccCode.Disabled = True
                ddlConAccName.Disabled = True
                ddlCCCode.Disabled = False
                ddlCCName.Disabled = False
            ElseIf acctype(n) = "C" Then
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            ElseIf acctype(n) = "S" Then
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' union all  select distinct partymast.accrualacctcode controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' union all  select distinct acctmast.acctname ,partymast.accrualacctcode controlacctcode      from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname"
            ElseIf acctype(n) = "A" Then
                sqlstr1 = " select distinct supplier_agents.controlacctcode ,acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' union all  select distinct supplier_agents.accrualacctcode controlacctcode ,acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "'  order by controlacctcode"
                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' union all  select distinct acctmast.acctname ,supplier_agents.accrualacctcode controlacctcode     from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "'  order by acctmast.acctname "

            End If
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

            ddlConAccCode.Value = controlcode(n)
            ddlConAccName.Value = controlname(n)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

            ddlCCCode.Value = CCCode(n)
            ddlCCName.Value = CCName(n)
            txtCurrCode.Value = currcode(n)
            txtCurrRate.Value = crate(n)

            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)
            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)

            txtNarr.Value = narration(n)

            If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                txtCurrRate.Disabled = True
            End If

            n = n + 1
        Next
    End Sub
    Protected Sub btnDelLine_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clAdBill As New Collection
        Dim clAdBillnew As New Collection

        Dim intLineNo As Integer = 1
        Dim strLineKey As String

        Dim MainGrdCount As Integer
        Dim credittot, debittot, basecredittot, basedebittot As Decimal
        Dim n As Integer = 0
        Dim count As Integer
        count = grdJournal.Rows.Count + 1
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String

        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String


        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        Dim txtCredit, txtCurrCode, txtCurrRate, txtNarr, txtBaseCredit, txtOldLineno, txtDebit, txtBaseDebit As HtmlInputText
        Dim lblno As HtmlInputText
        Dim chckDeletion As CheckBox

        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname As HtmlInputText
        Dim sqlstr1, sqlstr2 As String
        Dim cntcont, j As Long
        'If Session("Collection").ToString <> "" Then
        '    clAdBill = CType(Session("Collection"), Collection)
        'End If

        For Each gvRow In grdJournal.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            'ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            'ddlgAccName = gvRow.FindControl("ddlgAccName")

            'ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            'ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")


            txtNarr = gvRow.FindControl("txtgnarration")


            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")


            If chckDeletion.Checked = True Then

                cntcont = clAdBill.Count / 21
                For j = 1 To cntcont
                    strLineKey = j & ":" & lblno.Value
                    DeleteCollection(clAdBill, "AgainstTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "AccTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "TranId" & strLineKey)
                    DeleteCollection(clAdBill, "TranDate" & strLineKey)
                    DeleteCollection(clAdBill, "TranType" & strLineKey)
                    DeleteCollection(clAdBill, "DueDate" & strLineKey)
                    DeleteCollection(clAdBill, "CurrRate" & strLineKey)
                    DeleteCollection(clAdBill, "Credit" & strLineKey)
                    DeleteCollection(clAdBill, "Debit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseCredit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseDebit" & strLineKey)
                    DeleteCollection(clAdBill, "RefNo" & strLineKey)
                    DeleteCollection(clAdBill, "Field2" & strLineKey)
                    DeleteCollection(clAdBill, "Field3" & strLineKey)
                    DeleteCollection(clAdBill, "Field4" & strLineKey)
                    DeleteCollection(clAdBill, "Field5" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AccType" & strLineKey)
                    DeleteCollection(clAdBill, "AccCode" & strLineKey)
                    DeleteCollection(clAdBill, "AccGLCode" & strLineKey)
                    DeleteCollection(clAdBill, "AdjustBaseTotal" & strLineKey)
                Next
            Else
                If txtacctcode.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
                    lineno(n) = lblno.Value
                    Olineno(n) = txtOldLineno.Value
                    acctype(n) = ddlAccType.Value
                    acccode(n) = txtacctcode.Value
                    accname(n) = txtacctname.Value 'ddlgAccName.Value
                    controlcode(n) = txtctrolaccode.Value
                    controlname(n) = txtcontrolacname.Value
                    CCCode(n) = CType(ddlCCCode.Value, String)
                    CCName(n) = CType(ddlCCName.Value, String)

                    currcode(n) = txtCurrCode.Value
                    crate(n) = txtCurrRate.Value

                    credit(n) = txtCredit.Value
                    debit(n) = txtDebit.Value
                    bcredit(n) = txtBaseCredit.Value
                    bdebit(n) = txtBaseDebit.Value

                    narration(n) = txtNarr.Value
                    n = n + 1
                End If
            End If
        Next
        'Session.Add("Collection", clAdBill)
        Dim collectionDate As Collection
        Dim strLineKeynew As String
        Dim sno As Integer
        'If Session("Collection").ToString <> "" Then
        '    collectionDate = CType(Session("Collection"), Collection)
        'End If

        collectionDate = clAdBill

        Dim grdct As Long
        grdct = n
        If grdct = 0 Then
            grdct = 1
        End If
        fillDategrd(grdJournal, False, grdct)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdJournal.Rows
            If n = i Then
                Exit For
            End If

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")

            txtOldLineno.Value = Olineno(n)
            txtacctcode.Value = acccode(n)
            txtacctname.Value = accname(n)
            txtctrolaccode.Value = controlcode(n)
            txtcontrolacname.Value = controlname(n)

            ddlAccType.Value = acctype(n)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where type = '" & ddlAccType.Value & "'   order by code", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select Code,des from view_account where type = '" & ddlAccType.Value & "'  order by des", True)


            ddlgAccName.Value = accname(n)
            ddlgAccCode.Value = acccode(n)

            ddlConAccCode.Disabled = False
            ddlConAccName.Disabled = False
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            If acctype(n) = "G" Then
                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                ddlConAccCode.Disabled = True
                ddlConAccName.Disabled = True
                ddlCCCode.Disabled = False
                ddlCCName.Disabled = False
            ElseIf acctype(n) = "C" Then
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            ElseIf acctype(n) = "S" Then
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' union all  select distinct partymast.accrualacctcode controlacctcode  , acctmast.acctname  from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' union all  select distinct acctmast.acctname ,partymast.accrualacctcode controlacctcode from acctmast ,partymast where   partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname"
            ElseIf acctype(n) = "A" Then
                sqlstr1 = " select distinct supplier_agents.controlacctcode, acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' union all  select distinct supplier_agents.accrualacctcode controlacctcode, acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' union all  select distinct acctmast.acctname ,supplier_agents.accrualacctcode controlacctcode     from acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by acctmast.acctname"

            End If
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)


            ddlConAccCode.Value = controlcode(n)
            ddlConAccName.Value = controlname(n)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

            ddlCCCode.Value = CCCode(n)
            ddlCCName.Value = CCName(n)

            txtCurrCode.Value = currcode(n)
            txtCurrRate.Value = crate(n)

            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)
            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)

            txtNarr.Value = narration(n)


            credittot = DecRound(DecRound(credittot) + DecRound(CType(txtCredit.Value, Decimal)))
            basecredittot = DecRound(DecRound(basecredittot) + DecRound(CType(txtBaseCredit.Value, Decimal)))
            debittot = DecRound(DecRound(debittot) + DecRound(CType(txtDebit.Value, Decimal)))
            basedebittot = DecRound(DecRound(basedebittot) + DecRound(CType(txtBaseDebit.Value, Decimal)))

            If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                txtCurrRate.Disabled = True
            End If
            '   lineno(count)


            sno = 1
            If acctype(n) <> "G" Then
                MainGrdCount = grdJournal.Rows.Count  ' lineno.Length
                Dim MainRowidx As Integer
                MainRowidx = 1
                For MainRowidx = 0 To MainGrdCount - 1
                    cntcont = collectionDate.Count / 21
                    sno = 1
                    For j = 1 To cntcont
                        strLineKey = j & ":" & lineno(MainRowidx)
                        strLineKeynew = sno & ":" & lblno.Value
                        If colexists(clAdBillnew, "AgainstTranLineNo" & strLineKeynew) = False Then
                            If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                                If collectionDate("AccCode" & strLineKey).ToString = accname(n) And collectionDate("AccType" & strLineKey).ToString = acctype(n) And collectionDate("AccGLCode" & strLineKey).ToString = controlname(n) And (DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)) = bcredit(n) Or DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)) = bdebit(n)) And CType(collectionDate("CurrRate" & strLineKey), Decimal) = Val(CType(crate(n), Decimal)) And collectionDate("AgainstTranLineNo" & strLineKey) = lineno(MainRowidx) Then
                                    AddCollection(clAdBillnew, "AgainstTranLineNo" & strLineKeynew, lblno.Value) 'collectionDate("AgainstTranLineNo" & strLineKey))
                                    AddCollection(clAdBillnew, "AccTranLineNo" & strLineKeynew, collectionDate("AccTranLineNo" & strLineKey))
                                    AddCollection(clAdBillnew, "TranId" & strLineKeynew, collectionDate("TranId" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "TranDate" & strLineKeynew, Format(CType(collectionDate("TranDate" & strLineKey).ToString, Date), "dd/MM/yyyy"))
                                    AddCollection(clAdBillnew, "TranType" & strLineKeynew, collectionDate("TranType" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "DueDate" & strLineKeynew, Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "dd/MM/yyyy"))
                                    AddCollection(clAdBillnew, "CurrRate" & strLineKeynew, CType(collectionDate("CurrRate" & strLineKey), Decimal))
                                    AddCollection(clAdBillnew, "Credit" & strLineKeynew, DecRound(CType(collectionDate("Credit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "Debit" & strLineKeynew, DecRound(CType(collectionDate("Debit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "BaseCredit" & strLineKeynew, DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "BaseDebit" & strLineKeynew, DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "RefNo" & strLineKeynew, collectionDate("RefNo" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field2" & strLineKeynew, collectionDate("Field2" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field3" & strLineKeynew, collectionDate("Field3" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field4" & strLineKeynew, collectionDate("Field4" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field5" & strLineKeynew, collectionDate("Field5" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "OpenMode" & strLineKeynew, collectionDate("OpenMode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccType" & strLineKeynew, collectionDate("AccType" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccCode" & strLineKeynew, collectionDate("AccCode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccGLCode" & strLineKeynew, collectionDate("AccGLCode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AdjustBaseTotal" & strLineKeynew, DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)))


                                    DeleteCollection(collectionDate, "AgainstTranLineNo" & strLineKey)
                                    DeleteCollection(collectionDate, "AccTranLineNo" & strLineKey)
                                    DeleteCollection(collectionDate, "TranId" & strLineKey)
                                    DeleteCollection(collectionDate, "TranDate" & strLineKey)
                                    DeleteCollection(collectionDate, "TranType" & strLineKey)
                                    DeleteCollection(collectionDate, "DueDate" & strLineKey)
                                    DeleteCollection(collectionDate, "CurrRate" & strLineKey)
                                    DeleteCollection(collectionDate, "Credit" & strLineKey)
                                    DeleteCollection(collectionDate, "Debit" & strLineKey)
                                    DeleteCollection(collectionDate, "BaseCredit" & strLineKey)
                                    DeleteCollection(collectionDate, "BaseDebit" & strLineKey)
                                    DeleteCollection(collectionDate, "RefNo" & strLineKey)
                                    DeleteCollection(collectionDate, "Field2" & strLineKey)
                                    DeleteCollection(collectionDate, "Field3" & strLineKey)
                                    DeleteCollection(collectionDate, "Field4" & strLineKey)
                                    DeleteCollection(collectionDate, "Field5" & strLineKey)
                                    DeleteCollection(collectionDate, "OpenMode" & strLineKey)
                                    DeleteCollection(collectionDate, "AccType" & strLineKey)
                                    DeleteCollection(collectionDate, "AccCode" & strLineKey)
                                    DeleteCollection(collectionDate, "AccGLCode" & strLineKey)
                                    sno = sno + 1
                                End If
                            End If
                            'Else
                            '    Exit For
                        End If
                    Next
                Next
            End If


            n = n + 1
        Next

        'Session.Add("Collection", clAdBillnew)
        Session.Add("Collection" & ":" & txtAdjcolno.Value, clAdBillnew)

        txtTotalCredit.Value = DecRound(Val(credittot))
        txtTotalDebit.Value = DecRound(Val(debittot))
        txtTotBaseCredit.Value = DecRound(Val(basecredittot))
        txtTotBaseDebit.Value = DecRound(Val(basedebittot))
    End Sub
    Public Sub DeleteCollection(ByVal dataCollection As Collection, ByVal strKey As String)
        If colexists(dataCollection, strKey) = True Then
            dataCollection.Remove(strKey)
        End If
    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer))
        Return Rdamt
    End Function
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Journal','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            '    Exit Sub
            'End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Do you want to print' );", True)
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            'Session.Add("RefCode", CType(txtDocNo.Value.Trim, String))
            'Session.Add("Pageame", "JournalDoc")
            'Session.Add("BackPageName", "~\AccountsModule\Journal.aspx?tran_type=" & CType(Session("RVPVTranType"), String) & "")

            'strSelectionFormula = ""
            'If txtDocNo.Value.Trim <> "" Then
            '    If Trim(strSelectionFormula) = "" Then
            '        'strReportTitle = "Doc No : " & txtDocNo.Value.Trim
            '        strSelectionFormula = " {journal_master.tran_id}='" & txtDocNo.Value.Trim & "'"
            '    Else
            '        'strReportTitle = strReportTitle & "Vocher No : " & txtDocNo.Value.Trim & "'"
            '        strSelectionFormula = strSelectionFormula & " {journal_master.tran_id}='" & txtDocNo.Value.Trim & "'"
            '    End If
            'Else
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Tran ID' );", True)
            '    Exit Sub
            'End If

            'If Trim(strSelectionFormula) = "" Then
            '    strSelectionFormula = " {journal_master.tran_type} = '" & txtTranType.Text & "' " & _
            '    " and  {journal_master.journal_div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'Else
            '    strSelectionFormula = strSelectionFormula & " AND {journal_master.tran_type} = '" & txtTranType.Text & "'" & _
            '    " and  {journal_master.journal_div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'End If
            ''Dim lblstr As String
            ''lblstr = ""
            ''If Session("RVPVTranType") = "RV" Then
            ''    lblstr = "Receipt Voucher"
            ''ElseIf Session("RVPVTranType") = "PV" Then
            ''    lblstr = "Payment Voucher"
            ''End If
            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Session.Add("PrinDocTitle", "Journal Voucher")

            'Dim ScriptStr As String
            'ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDoc.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"

            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDocNew.aspx?Pageame=ExchangediffDoc&BackPageName=~\AccountsModule\ExchSearch.aspx&Tranid=" & txtDocNo.Value & "&TranType=" & txtTranType.Text & "','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Journal.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String)
        Dim PostUnpostFlag As Boolean = False
        PostUnpostFlag = objUser.PostUnpostRight(Session("dbconnectionName"), UserName, UserPwd, AppName, PageName)
        If PostUnpostFlag = True Then
            chkPost.Visible = True
            lblPostmsg.Visible = True
        Else
            chkPost.Visible = False
            lblPostmsg.Visible = False
            If ViewState("JournalState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("JournalState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub

    Public Sub creategrid()
        If ViewState("JournalState") = "New" Then
            fillDategrd(grdJournal, False, 2)
            btnGenGrid.Enabled = False
            ShowControls()
        End If

    End Sub

    Protected Sub btnGenGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("JournalState") = "New" Then
            If txtNoofRows.Value = "0" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Can not create 0 no of rows.' );", True)
                Return
            End If
            fillDategrd(grdJournal, False, txtNoofRows.Value)
            btnGenGrid.Enabled = False
            ShowControls()
        End If
        If ViewState("JournalState") = "Edit" Then
            If txtNoofRows.Value > hdnRows.Value Then
                Dim norows As Integer = CType(hdnRows.Value, Integer) + CType(txtNoofRows.Value, Integer)
                fillDategrd(grdJournal, False, norows)
            End If
            btnGenGrid.Enabled = False
            ShowControls()
            show_record(txtDocNo.Value.Trim())
            ShowFillGrid(txtDocNo.Value.Trim())
        End If

    End Sub
    Public Sub HideControls()
        btnSave.Style("visibility") = "hidden"
        grdJournal.Style("visibility") = "hidden"
        btnAdd.Style("visibility") = "hidden"
        btnDelLine.Style("visibility") = "hidden"
        Label7.Style("visibility") = "hidden"
        txtTotalDebit.Style("visibility") = "hidden"
        txtTotalCredit.Style("visibility") = "hidden"
        lblBaseTot.Style("visibility") = "hidden"
        txtTotBaseDebit.Style("visibility") = "hidden"
        txtTotBaseCredit.Style("visibility") = "hidden"
        lblBaseDiff.Style("visibility") = "hidden"
        txtTotBaseDiff.Style("visibility") = "hidden"
        chkPost.Style("visibility") = "hidden"
    End Sub
    Public Sub ShowControls()
        btnSave.Style("visibility") = "visible"
        grdJournal.Style("visibility") = "visible"
        btnAdd.Style("visibility") = "visible"
        btnDelLine.Style("visibility") = "visible"
        Label7.Style("visibility") = "visible"
        txtTotalDebit.Style("visibility") = "visible"
        txtTotalCredit.Style("visibility") = "visible"
        lblBaseTot.Style("visibility") = "visible"
        txtTotBaseDebit.Style("visibility") = "visible"
        txtTotBaseCredit.Style("visibility") = "visible"
        lblBaseDiff.Style("visibility") = "visible"
        txtTotBaseDiff.Style("visibility") = "visible"
        chkPost.Style("visibility") = "visible"
    End Sub

    Protected Sub adjust_bills()
        Dim ds As DataSet = New DataSet()
        Dim parms As New List(Of SqlParameter)
        Dim parm(4) As SqlParameter
        Dim todate As String
        todate = Format(CType(txtJDate.Text, Date), "yyyy/MM/dd")  'Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim stracccode As String = ""
        Dim strAccType As String = ""

        For Each gvRow In grdJournal.Rows
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")

            If ddlAccType.Value.Trim <> "G" And ddlgAccCode.Value.Trim <> "[Select]" Then
                stracccode = CType(ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text, String)
                strAccType = CType(ddlAccType.Items(ddlAccType.SelectedIndex).Value, String)
            End If
        Next

        parm(0) = New SqlParameter("@fromacct", CType(stracccode, String))
        parm(1) = New SqlParameter("@fromcontrol", String.Empty)
        parm(2) = New SqlParameter("@todate", CType(todate, String))
        parm(3) = New SqlParameter("@type", CType(strAccType, String))
        If txtDocNo.Value <> "" Then
            parm(4) = New SqlParameter("@tranid", CType(txtDocNo.Value, String))
        Else
            parm(4) = New SqlParameter("@tranid", String.Empty)
        End If

        parms.Add(parm(0))
        parms.Add(parm(1))
        parms.Add(parm(2))
        parms.Add(parm(3))
        parms.Add(parm(4))


        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_exchange_difference", parms)
        If ds.Tables(0).Rows.Count > 0 Then
            GVCollection.DataSource = ds.Tables(0)
            GVCollection.DataBind()
            GVCollection.Visible = True
            txtRowCount.Value = ds.Tables(0).Rows.Count
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No bills to be adjusted');", True)
            Return
        End If

    End Sub
    Protected Sub btngenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        adjust_bills()
    End Sub

   
    Protected Sub GVCollection_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then



            Dim chksel As CheckBox = CType(e.Row.FindControl("chksel"), CheckBox)
            Dim lbltranid As Label = CType(e.Row.FindControl("lbltranid"), Label)

            chksel.Attributes.Add("onclick", "javascript:chkmultiple('" + CType(chksel.ClientID, String) + "')")


        End If
    End Sub
End Class



