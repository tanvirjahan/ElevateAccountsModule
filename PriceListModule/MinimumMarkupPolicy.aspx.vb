Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class MinimumMarkupPolicy
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim ObjDate As New clsDateTime
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlTrans As SqlTransaction

    Dim blankrow As Integer = 0
    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
    Dim cFromNew As New ArrayList
    Dim cToNew As New ArrayList
    Dim Operator1New As New ArrayList
    Dim rValueNew As New ArrayList
    Dim ValueNew As New ArrayList
    Dim ACINew As New ArrayList
    Dim AdultNew As New ArrayList
    Dim ChildNew As New ArrayList
    Dim ExtraBedNew As New ArrayList
    Dim CopyRowlist As New ArrayList

    Dim iCurrecntIndex As Integer = 20
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("MinMarkupState", Request.QueryString("State"))
                ViewState.Add("MinFormulaID", Request.QueryString("FormulaID"))

                Session("MinMarkupState") = "New"
                ucMinMarkupPolicy.sbSetPageState("", "MinMarkupState", CType(Session("MinMarkupState"), String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                ucMinMarkupPolicy.sbShowCountry()
                fillDategrd(gvCommFormula, False, 1)
                fillDategrd(grdDates, False, 1)

                '    fillDategrd(grdCommFormula, False, 1)
                If ViewState("MinMarkupState") = "New" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Minimum Markup and Discount"
                    Page.Title = Page.Title + " " + "New Minimum Markup and Discount"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                    TxtCurrencyName.Text = "U S DOLLAR"
                    TextCurrencyCode.Text = "USD"



                ElseIf ViewState("MinMarkupState") = "Edit" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Edit Minimum Markup and Discount"
                    Page.Title = Page.Title + " " + "Edit Minimum Markup and Discount"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("MinFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MinFormulaID"), String))
                    ShowDates(CType(ViewState("MinFormulaID"), String))

                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")

                    ucMinMarkupPolicy.sbSetPageState(CType(ViewState("MinFormulaID"), String).Trim, "MIN_MARKUP", ViewState("MinMarkupState"))
                    Session.Add("RefCode", CType(CType(ViewState("MinFormulaID"), String).Trim, String))
                    '    ucMinMarkupPolicy.sbSetPageState(CType(ViewState("MinFormulaID"), String).Trim, Nothing, ViewState("State"))
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    ucMinMarkupPolicy.sbShowCountry()


                    DisableControl()
                ElseIf ViewState("MinMarkupState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Minimum Markup and Discount"
                    Page.Title = Page.Title + " " + "View Minimum Markup and Discount"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    ShowRecord(CType(ViewState("MinFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MinFormulaID"), String))
                    ShowDates(CType(ViewState("MinFormulaID"), String))
                    ucMinMarkupPolicy.sbSetPageState(CType(ViewState("MinFormulaID"), String).Trim, "MIN_MARKUP", ViewState("MinMarkupState"))
                    Session.Add("RefCode", CType(CType(ViewState("MinFormulaID"), String).Trim, String))
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    ucMinMarkupPolicy.sbShowCountry()

                    DisableControl()
                ElseIf ViewState("MinMarkupState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Minimum Markup and Discount"
                    Page.Title = Page.Title + " " + "Delete Minimum Markup and Discount"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("MinFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MinFormulaID"), String))
                    ShowDates(CType(ViewState("MinFormulaID"), String))
                    ucMinMarkupPolicy.sbSetPageState(CType(ViewState("MinFormulaID"), String).Trim, "MIN_MARKUP", ViewState("MinMarkupState"))
                    Session.Add("RefCode", CType(CType(ViewState("MinFormulaID"), String).Trim, String))
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    ucMinMarkupPolicy.sbShowCountry()
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("MinMarkupState") = "Copy" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Minimum Markup and Discount"
                    Page.Title = Page.Title + " " + "New Minimum Markup and Discount"
                    btnSave.Text = "Save"
                    ShowRecord(CType(ViewState("MinFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MinFormulaID"), String))
                    ShowDates(CType(ViewState("MinFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    ucMinMarkupPolicy.sbSetPageState(CType(ViewState("MinFormulaID"), String).Trim, "MIN_MARKUP", ViewState("MinMarkupState"))
                    Session.Add("RefCode", CType(CType(ViewState("MinFormulaID"), String).Trim, String))
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    ucMinMarkupPolicy.sbShowCountry()
                    'TxtCurrencyName.Text = "U S DOLLAR"
                    'TextCurrencyCode.Text = "USD"
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Page.Title = "Minimum Markup Policy Entry"
    End Sub
#End Region
    Private Sub ShowDates(ByVal RefCode As String)
        Try

            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            strSqlQry = "select count( distinct fromdate) from Minimum_Markup_Dates(nolock) where FormulaId='" & RefCode & "'" '" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            mySqlCmd = New SqlCommand("Select * from Minimum_Markup_Dates(nolock) Where FormulaId='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MinMarkupEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    

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
    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)



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
    Public Function FindDatePeriod() As Boolean



        Dim strMsg As String = ""

        FindDatePeriod = True
        Try

            '   CopyRow = 0

            Dim weekdaystr As String = ""

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = ucMinMarkupPolicy.checkcountrylist
            Session("AgentList") = ucMinMarkupPolicy.checkagentlist

            For Each GVRow In grdDates.Rows

                Dim txtfromdate As TextBox = GVRow.Findcontrol("txtfromdate")
                Dim txttodate As TextBox = GVRow.Findcontrol("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    Dim ds1 As DataSet
                    Dim parms3 As New List(Of SqlParameter)
                    Dim parm3(5) As SqlParameter

                    parm3(0) = New SqlParameter("@formulaid", CType(txtcode.Value.Trim, String))
                    parm3(1) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm3(2) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm3(3) = New SqlParameter("@country", CType(Session("CountryList"), String))
                    parm3(4) = New SqlParameter("@agent", CType(Session("AgentList"), String))
                    For i = 0 To 4
                        parms3.Add(parm3(i))
                    Next

                    ds1 = New DataSet()
                    ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkminimummarkup", parms3)


                    If ds1.Tables.Count > 0 Then
                        If ds1.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds1.Tables(0).Rows(0)("formulaid")) = False Then
                               
                                strMsg = "Minimum Markup already exists For this  Dates   " + " - Formula Id " + ds1.Tables(0).Rows(0)("formulaid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")


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
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                '  If ViewState("MinMarkupState") = "New" Or ViewState("MinMarkupState") = "Edit" Then
                If ViewState("MinMarkupState") = "New" Or ViewState("MinMarkupState") = "Edit" Or ViewState("MinMarkupState") = "Copy" Or ViewState("MinMarkupState") = "Delete" Then

                    If txtname.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter formula name.');", True)
                        Exit Sub
                    End If
                    If txtAdultWithVisa.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter adult with visa.');", True)
                        Exit Sub
                    End If

                    If txtAdultWithoutVisa.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter adult without visa.');", True)
                        Exit Sub
                    End If
                    If txtApplicableTo.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter applicable to.');", True)
                        Exit Sub
                    End If

                    ''' commented we are checking date period shahul 16/04/18
                    'If checkForDuplicate() = False Then
                    '    Exit Sub
                    'End If


                    If checkforexisting() = False Then
                        Exit Sub
                    End If


                    Dim strSelectedCountriesList As String = ""
                    strSelectedCountriesList = ucMinMarkupPolicy.checkcountrylist.ToString

                    Dim strSelectedAgentList As String = ""
                    strSelectedAgentList = ucMinMarkupPolicy.checkagentlist.ToString()
                    If strSelectedCountriesList = "" And strSelectedAgentList = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any country or agent.');", True)
                        Exit Sub
                    End If

                    If checkForMissingSlab() = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Slab is missing.');", True)
                        Exit Sub
                    End If

                    If ValidateSlabRange() = False Then
                        Exit Sub
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction
                    Dim strOpMode As String = ""
                    If ViewState("MinMarkupState") = "New" Or ViewState("MinMarkupState") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("MINMARK", mySqlConn, sqlTrans)
                        txtcode.Value = optionval.Trim
                    End If
                    mySqlCmd = New SqlCommand("sp_Add_Mod_MinimumMarkup", mySqlConn, sqlTrans)
                    If ViewState("MinMarkupState") = "New" Or ViewState("MinMarkupState") = "Copy" Then
                        strOpMode = "1"
                    ElseIf ViewState("MinMarkupState") = "Edit" Then
                        strOpMode = "2"
                    ElseIf ViewState("MinMarkupState") = "Delete" Then
                        strOpMode = "3"
                    End If


                    '----------------------------------- Create XML for insert markup formula -------------------------------

                    Dim lblFLineNo As Label
                    Dim n As Integer = 0
                    Dim txtFrom As TextBox
                    Dim txtTo As TextBox
                    Dim txtDiscount As TextBox



                    Dim strBuffer As New Text.StringBuilder
                    Dim count = gvCommFormula.Rows.Count
                    Dim strBufferFlag = ""
                    If count = 1 Then
                        For Each GVRow In gvCommFormula.Rows
                            txtTo = GVRow.FindControl("txtTo")
                            txtDiscount = GVRow.FindControl("txtDiscount")
                            If txtTo.Text = "" And txtDiscount.Text = "" Then
                                strBufferFlag = 1
                            End If
                        Next
                    End If

                    If strBufferFlag <> "1" Then


                        Dim iRpwCnt As Integer = 0
                        strBuffer.Append("<MinMarkup_Discounts>")
                        For Each GVRow In gvCommFormula.Rows
                            lblFLineNo = GVRow.FindControl("lblFLineNo")
                            txtFrom = GVRow.FindControl("txtFrom")
                            txtTo = GVRow.FindControl("txtTo")
                            txtDiscount = GVRow.FindControl("txtDiscount")
                            strBuffer.Append("<MinMarkup_Discount>")
                            strBuffer.Append(" <FLineNo>" & lblFLineNo.Text.Trim & "</FLineNo>")
                            strBuffer.Append(" <FromSlab>" & txtFrom.Text.Trim & " </FromSlab>")

                            Dim strTo As String = ""
                            If txtTo.Text.Trim = "" Then
                                strTo = "0"
                                strBuffer.Append(" <Lastslab>1</Lastslab>")
                            Else
                                strTo = txtTo.Text.Trim
                                strBuffer.Append(" <Lastslab>0</Lastslab>")
                            End If
                            strBuffer.Append(" <ToSlab>" & strTo & " </ToSlab>")


                            If txtDiscount.Text.Trim <> "" Then
                                strBuffer.Append(" <Discount>" & txtDiscount.Text.Trim & "</Discount>")
                            Else
                                strBuffer.Append(" <Discount>0</Discount>")
                            End If
                            strBuffer.Append("</MinMarkup_Discount>")
                        Next

                        strBuffer.Append("</MinMarkup_Discounts>")
                    Else
                        strBuffer = strBuffer.Append("<MinMarkup_Discounts>")
                        strBuffer.Append("<MinMarkup_Discount>")
                        strBuffer.Append("</MinMarkup_Discount>")
                        strBuffer.Append("</MinMarkup_Discounts>")
                    End If
                    '-----------------------------------------------------------


                    Dim strBufferdates As New Text.StringBuilder
                    Dim countdates = grdDates.Rows.Count
                    Dim strBufferdatesFlag = ""
                    Dim txtfromDate As TextBox
                    Dim txttodate As TextBox
                    If count = 1 Then
                        For Each GVRow In grdDates.Rows
                            txtfromDate = GVRow.FindControl("txtfromDate")
                            txttodate = GVRow.FindControl("txtToDate")
                            If txtfromDate.Text = "" And txttodate.Text = "" Then
                                strBufferdatesFlag = 1
                            End If
                        Next
                    End If

                    If strBufferdatesFlag <> "1" Then


                        Dim iRpwCnt As Integer = 1
                        strBufferdates.Append("<Minimum_Markup_Dates>")
                        For Each GVRow In grdDates.Rows
                            lblFLineNo = GVRow.FindControl("lblFLineNo")
                            txtfromDate = GVRow.FindControl("txtfromDate")
                            txttodate = GVRow.FindControl("txtToDate")

                            strBufferdates.Append("<Minimum_Markup_Date>")
                            strBufferdates.Append(" <DLineNo>" & iRpwCnt & "</DLineNo>")
                            strBufferdates.Append(" <FromDate>" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & " </FromDate>")
                            strBufferdates.Append(" <ToDate>" & Format(CType(txttodate.Text, Date), "yyyy/MM/dd") & " </ToDate>")
                            strBufferdates.Append("</Minimum_Markup_Date>")
                            iRpwCnt = iRpwCnt + 1
                        Next

                        strBufferdates.Append("</Minimum_Markup_Dates>")
                    Else
                        strBufferdates = strBufferdates.Append("<Minimum_Markup_Dates>")
                        strBufferdates.Append("<Minimum_Markup_Date>")
                        strBufferdates.Append("</Minimum_Markup_Date>")
                        strBufferdates.Append("</Minimum_Markup_Dates>")
                    End If



                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaId", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaName", SqlDbType.VarChar, 500)).Value = CType(txtname.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplicableTo", SqlDbType.VarChar, 1000)).Value = CType(txtApplicableTo.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@AdultWithVisa", SqlDbType.VarChar, 20)).Value = CType(txtAdultWithVisa.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@AdultWithoutVisa", SqlDbType.VarChar, 20)).Value = CType(txtAdultWithoutVisa.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildWithVisa", SqlDbType.VarChar, 20)).Value = CType(txtChildWithVisa.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildWithoutVisa", SqlDbType.VarChar, 20)).Value = CType(txtChildWithoutVisa.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@Currency", SqlDbType.VarChar, 20)).Value = CType(TextCurrencyCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@MinMarkup_Discount_XML", SqlDbType.Xml)).Value = strBuffer.ToString
                    mySqlCmd.Parameters.Add(New SqlParameter("@MinMarkup_Countries", SqlDbType.VarChar, 8000)).Value = strSelectedCountriesList
                    mySqlCmd.Parameters.Add(New SqlParameter("@MinMarkup_Agents", SqlDbType.VarChar, 8000)).Value = strSelectedAgentList
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@Active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@Active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@Userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@OpMode", SqlDbType.Int)).Value = strOpMode
                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildFreeupto", SqlDbType.Decimal)).Value = CType(txtchildfreeupto.Value.Trim, Decimal)
                    mySqlCmd.Parameters.Add(New SqlParameter("@Minimum_Markup_Dates_XML", SqlDbType.Xml)).Value = strBufferdates.ToString
                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildFreeWithVisa", SqlDbType.VarChar, 20)).Value = CType(txtChildfreeWithVisa.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildFreeWithoutVisa", SqlDbType.VarChar, 20)).Value = CType(txtChildfreeWithoutVisa.Value.Trim, String)

                    mySqlCmd.ExecuteNonQuery()

                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                    ucMinMarkupPolicy.clearsessions()   '' ADDED shahul 20/03/18


                    If Not Request.QueryString("Page") Is Nothing Then
                        If Request.QueryString("Page") = "MinMarkup" Then
                            Dim strscript1 As String = ""
                            strscript1 = "window.opener.__doPostBack('MinMarkupWindowPostBack', '');window.opener.focus();window.close();"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                            'Else
                            '    Dim strscript As String = ""
                            '    strscript = "window.opener.__doPostBack('MarkupWindowPostBack', '');window.opener.focus();window.close();"
                            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                        End If
                    Else
                        Dim strscript As String = ""
                        strscript = "window.opener.__doPostBack('MinMarkupWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    End If

                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Private Function ValidateSlabRange() As Boolean
        Dim n As Integer = 0
        Dim txtTo1 As TextBox
        Dim txtTo As TextBox
        Dim txtFrom As TextBox
        For i As Integer = 0 To gvCommFormula.Rows.Count - 1
            If i > 0 And gvCommFormula.Rows.Count > 1 Then
                txtFrom = gvCommFormula.Rows(i).FindControl("txtFrom")
                txtTo1 = gvCommFormula.Rows(i - 1).FindControl("txtTo")
                If txtFrom.Text <> "" And txtTo1.Text <> "" Then
                    If Not (txtTo1.Text + 1) = txtFrom.Text Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Slab range is not correct.');", True)
                        SetFocus(txtFrom)
                        ValidateSlabRange = False
                        Exit Function
                    End If
                End If
            End If

            txtFrom = gvCommFormula.Rows(i).FindControl("txtFrom")
            txtTo = gvCommFormula.Rows(i).FindControl("txtTo")
            If txtFrom.Text = "" And txtTo.Text = "" Then
                If Not (txtFrom.Text) > txtTo.Text Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Slab range is not correct.');", True)
                    SetFocus(txtFrom)
                    ValidateSlabRange = False
                    Exit Function
                End If
            End If

        Next
        ValidateSlabRange = True
        Return ValidateSlabRange
    End Function
    Private Function checkForMissingSlab() As Boolean
        If gvCommFormula.Rows.Count > 1 Then
            For Each row As GridViewRow In gvCommFormula.Rows
                Dim txtFrom As TextBox = CType(row.FindControl("txtFrom"), TextBox)
                Dim txtTo As TextBox = CType(row.FindControl("txtTo"), TextBox)
                If txtFrom.Text = "" And txtTo.Text = "" Then
                    txtFrom.Focus()
                    Return False
                End If
                If gvCommFormula.Rows.Count = 1 Then
                    If txtFrom.Text <> "" And txtTo.Text = "" Then
                        txtTo.Focus()
                        Return False
                    End If
                    If txtFrom.Text = "" And txtTo.Text <> "" Then
                        txtFrom.Focus()
                        Return False
                    End If
                End If

            Next
        End If

        Return True
    End Function

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If (ViewState("MinMarkupState") = "New") Or ViewState("MinMarkupState") = "Copy" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "Minimum_Markup_Header", "FormulaName", txtname.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This formula name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf (ViewState("MinMarkupState") = "Edit") Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "Minimum_Markup_Header", "FormulaId", "FormulaName", txtname.Value.Trim, CType(txtcode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This markup formula name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
#Region "Public Function ValidateFormulaTerm() As Boolean"
    Public Function ValidateFormulaTerm() As Boolean
        Try
            '--------------------------------------------- Validate Formula Grid ----------------------------------------------------

            ValidateFormulaTerm = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ValidateFormulaTerm = False
        End Try
    End Function
#End Region
#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        'Dim chckDeletion As CheckBox
        If ViewState("MinMarkupState") = "View" Or ViewState("MinMarkupState") = "Delete" Then
            txtcode.Disabled = True
            txtname.Disabled = True
            txtAdultWithVisa.Disabled = True
            txtAdultWithoutVisa.Disabled = True
            txtChildWithVisa.Disabled = True
            txtChildWithoutVisa.Disabled = True
            txtApplicableTo.Enabled = False
            chkActive.Disabled = True
        ElseIf ViewState("MinMarkupState") = "Edit" Then
            txtcode.Disabled = True
        End If
    End Sub
#End Region
#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("SELECT FormulaId,FormulaName,ApplicableTo,(SELECT CASE WHEN 1= 0 THEN NULL WHEN AdultWithVisa = FLOOR(AdultWithVisa) THEN CAST(CAST(AdultWithVisa AS INT)AS SQL_VARIANT) ELSE CAST(AdultWithVisa AS DECIMAL(38,5))END)AdultWithVisa, " _
                                      & " (SELECT CASE WHEN 1= 0 THEN NULL WHEN AdultWithoutVisa = FLOOR(AdultWithoutVisa) THEN CAST(CAST(AdultWithoutVisa AS INT)AS SQL_VARIANT) ELSE CAST(AdultWithoutVisa AS DECIMAL(38,5))END)AdultWithoutVisa, " _
                                      & " (SELECT CASE WHEN 1= 0 THEN NULL WHEN ChildWithVisa = FLOOR(ChildWithVisa) THEN CAST(CAST(ChildWithVisa AS INT)AS SQL_VARIANT) ELSE CAST(ChildWithVisa AS DECIMAL(38,5))END)ChildWithVisa, " _
                                      & " (SELECT CASE WHEN 1= 0 THEN NULL WHEN ChildWithoutVisa = FLOOR(ChildWithoutVisa) THEN CAST(CAST(ChildWithoutVisa AS INT)AS SQL_VARIANT) ELSE CAST(ChildWithoutVisa AS DECIMAL(38,5))END)ChildWithoutVisa,Active,Currency,CurrencyName,isnull(ChildFreeupto,0) ChildFreeupto,isnull(ChildFreeWithVisa,0) ChildFreeWithVisa,isnull(ChildFreeWithoutVisa,0) ChildFreeWithoutVisa from V_GET_MINIMUM_MARKUP_POLICY  Where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("FormulaID")) = False Then
                        If ViewState("MinMarkupState") <> "Copy" Then
                            Me.txtcode.Value = CType(mySqlReader("FormulaID"), String)
                        End If

                    Else
                        Me.txtcode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("FormulaName")) = False Then
                        Me.txtname.Value = CType(mySqlReader("FormulaName"), String)
                    Else
                        Me.txtname.Value = ""
                    End If
                    If IsDBNull(mySqlReader("ApplicableTo")) = False Then
                        Me.txtApplicableTo.Text = CType(mySqlReader("ApplicableTo"), String)
                    Else
                        Me.txtApplicableTo.Text = ""
                    End If
                    If IsDBNull(mySqlReader("AdultWithVisa")) = False Then
                        Me.txtAdultWithVisa.Value = CType(mySqlReader("AdultWithVisa"), String)
                    Else
                        Me.txtAdultWithVisa.Value = ""
                    End If
                    If IsDBNull(mySqlReader("AdultWithoutVisa")) = False Then
                        Me.txtAdultWithoutVisa.Value = CType(mySqlReader("AdultWithoutVisa"), String)
                    Else
                        Me.txtAdultWithoutVisa.Value = ""
                    End If
                    If mySqlReader("ChildWithVisa").ToString = "0" Then
                        Me.txtChildWithVisa.Value = ""

                    Else
                        Me.txtChildWithVisa.Value = CType(mySqlReader("ChildWithVisa"), String)
                    End If
                    If mySqlReader("ChildWithoutVisa").ToString = "0" Then
                        Me.txtChildWithoutVisa.Value = ""
                    Else
                        Me.txtChildWithoutVisa.Value = CType(mySqlReader("ChildWithoutVisa"), String)

                    End If
                    '' ADDEd shahul 25/03/18
                    If mySqlReader("ChildFreeupto").ToString = "0" Then
                        Me.txtchildfreeupto.Value = ""
                    Else
                        Me.txtchildfreeupto.Value = CType(mySqlReader("ChildFreeupto"), String)

                    End If

                    If mySqlReader("ChildFreeWithVisa").ToString = "0" Then
                        Me.txtChildfreeWithVisa.Value = ""

                    Else
                        Me.txtChildfreeWithVisa.Value = CType(mySqlReader("ChildFreeWithVisa"), String)
                    End If
                    If mySqlReader("ChildFreeWithoutVisa").ToString = "0" Then
                        Me.txtChildfreeWithoutVisa.Value = ""
                    Else
                        Me.txtChildfreeWithoutVisa.Value = CType(mySqlReader("ChildFreeWithoutVisa"), String)

                    End If

                    ''
                 
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("Currency")) = False Then
                        Me.TextCurrencyCode.Text = CType(mySqlReader("Currency"), String)
                    Else
                        Me.TextCurrencyCode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("CurrencyName")) = False Then
                        Me.TxtCurrencyName.Text = CType(mySqlReader("CurrencyName"), String)
                    Else
                        Me.TxtCurrencyName.Text = ""
                    End If

                End If
                End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region
#Region "Private Sub ShowCommFormula(ByVal RefCode As String)"

    Private Sub ShowCommFormula(ByVal RefCode As String)
        Try
            Dim lngCnt As Long
            lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "Minimum_Markup_Discount", "count(FormulaID)", "FormulaID", RefCode)
            If lngCnt = 0 Then lngCnt = 1
            fillDategrd(gvCommFormula, False, lngCnt)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("select FormulaId,[LineNo],FromSlab,ToSlab,Discount from Minimum_Markup_Discount where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            Dim n As Integer = 1
            If mySqlReader.HasRows Then
                While mySqlReader.Read()

                    '  Dim n As Integer = 0
                    Dim lblFLineNo As Label
                    Dim txtFrom As TextBox
                    Dim txtTo As TextBox
                    Dim txtDiscount As TextBox


                    If n <= gvCommFormula.Rows.Count Then
                        Dim gvRow As GridViewRow = gvCommFormula.Rows(n - 1)
                        lblFLineNo = gvRow.FindControl("lblFLineNo")
                        txtFrom = gvRow.FindControl("txtFrom")
                        txtTo = gvRow.FindControl("txtTo")
                        txtDiscount = gvRow.FindControl("txtDiscount")

                        If IsDBNull(mySqlReader("LineNo")) = False Then
                            lblFLineNo.Text = CType(mySqlReader("LineNo"), Integer)
                        End If
                        If IsDBNull(mySqlReader("FromSlab")) = False Then
                            txtFrom.Text = CType(mySqlReader("FromSlab"), String)
                        End If
                        If IsDBNull(mySqlReader("ToSlab")) = False Then
                            If CType(mySqlReader("ToSlab"), String) = "0" Then
                                txtTo.Text = ""
                            Else
                                txtTo.Text = CType(mySqlReader("ToSlab"), String)
                            End If
                        End If


                        If CType(mySqlReader("Discount"), String).Contains(".00") = True Then
                            txtDiscount.Text = CType(mySqlReader("Discount"), Integer)
                        Else
                            txtDiscount.Text = CType(mySqlReader("Discount"), String)
                        End If

                    End If

                    n = n + 1

                End While
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

    Protected Sub lnkCodeAndValue_Click(sender As Object, e As System.EventArgs)
        Try
            ucMinMarkupPolicy.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbCloseSearch_Click(sender As Object, e As System.EventArgs)
        Try
            ucMinMarkupPolicy.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentListSearch(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lsAgentNames As New List(Of String)
        Dim lsCountryList As String
        Try

            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList

            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                'If HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl") IsNot Nothing Then 'changed by mohamed on 03/10/2016 - instead of selected, used all
                'lsCountryList = HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    'strSqlQry += " and a.ctrycode in (" & lsCountryList & ")" 'changed by mohamed on 03/10/2016 -'commented this line to show all the agents.
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

    Protected Sub btnvsprocess_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess.Click
        Try
            ucMinMarkupPolicy.fnbtnVsProcess(txtvsprocesssplit, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function GetCurrencyName(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim CurrencyName As New List(Of String)
        Try
            strSqlQry = "select currname,currcode from currmast where currname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    CurrencyName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return CurrencyName
        Catch ex As Exception
            Return CurrencyName
        End Try

    End Function

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click

    End Sub

    Protected Sub btnGridAddRow_Click(sender As Object, e As System.EventArgs)
        AddGridRow()
    End Sub

    Private Sub AddGridRow()

        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gvCommFormula.Rows.Count + 1
        Dim FLineNo(count) As String
        Dim cFrom(count) As String
        Dim cTo(count) As String
        Dim cDiscount(count) As String


        Dim n As Integer = 0
        Dim lblFLineNo As Label
        Dim txtFrom As TextBox
        Dim txtTo As TextBox
        Dim txtDiscount As TextBox

        Try
            For Each GVRow In gvCommFormula.Rows
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                ' FLineNo(n) = CType(lblFLineNo.Text, String)
                FLineNo(n) = n + 1

                txtFrom = GVRow.FindControl("txtFrom")
                cFrom(n) = CType(txtFrom.Text, String)
                txtTo = GVRow.FindControl("txtTo")
                cTo(n) = CType(txtTo.Text, String)
                txtDiscount = GVRow.FindControl("txtDiscount")
                cDiscount(n) = CType(txtDiscount.Text, String)
                n = n + 1

            Next
            fillDategrd(gvCommFormula, False, gvCommFormula.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gvCommFormula.Rows
                If n = i Then
                    Exit For
                End If
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                lblFLineNo.Text = FLineNo(n)
                txtFrom = GVRow.FindControl("txtFrom")
                txtFrom.Text = cFrom(n)
                txtTo = GVRow.FindControl("txtTo")
                txtTo.Text = cTo(n)
                If n = i - 1 Then
                    Dim txtFromLast As TextBox = gvCommFormula.Rows(gvCommFormula.Rows.Count - 1).FindControl("txtFrom")
                    If cTo(n) <> "" Then
                        txtFromLast.Text = cTo(n) + 1
                    End If
                End If
                txtDiscount = GVRow.FindControl("txtDiscount")
                txtDiscount.Text = cDiscount(n)

                n = n + 1
            Next
            Dim txtToLast As TextBox = gvCommFormula.Rows(gvCommFormula.Rows.Count - 1).FindControl("txtTo")
            txtToLast.Focus()
            Dim txtFromLast1 As TextBox = gvCommFormula.Rows(gvCommFormula.Rows.Count - 1).FindControl("txtFrom")
            If txtFromLast1.Text = "" Then
                txtFromLast1.Focus()
            End If

            Dim gridNewrow As GridViewRow
            gridNewrow = gvCommFormula.Rows(gvCommFormula.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(gvCommFormula.Rows.Count - 1, String) + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

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
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("fLineNo", GetType(String)))
        '  dt.Columns.Add(New DataColumn("lowerslab", GetType(String)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            '  dr("lowerslab") = 0
            dt.Rows.Add(dr)
        Next
        CreateDataSource = New DataView(dt)
    End Function

    Protected Sub grdCommFormula_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCommFormula.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim gvRow As GridViewRow = CType(e.Row, GridViewRow)

            Dim txtFrom As TextBox = CType(gvRow.FindControl("txtFrom"), TextBox)
            Dim txtTo As TextBox = CType(gvRow.FindControl("txtTo"), TextBox)
            Dim txtDiscount As TextBox = CType(gvRow.FindControl("txtDiscount"), TextBox)
            Dim btnAddRow1 As Button = CType(gvRow.FindControl("btnGridAddRow"), Button)


            iCurrecntIndex = iCurrecntIndex + 1
            txtFrom.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtTo.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtDiscount.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            btnAddRow1.TabIndex = iCurrecntIndex

        End If
        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            e.Row.Attributes("onclick") = String.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex)
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event);"
            e.Row.Attributes("onselectstart") = "javascript:return false;"
        End If
        'If e.Row.RowType = DataControlRowType.Header Then


        '    e.Row.Cells(0).Visible = False
        '    e.Row.Cells(3).Visible = False
        '    e.Row.Cells(4).Visible = False
        '    e.Row.Cells(5).Visible = False
        'End If


    End Sub

    Protected Sub btnAddRow_Click(sender As Object, e As System.EventArgs)
        AddGridRow()
    End Sub

    Protected Sub btnDeleteRow_Click(sender As Object, e As System.EventArgs) Handles btnDeleteRow.Click
        DeleteGridRow()
    End Sub

    Private Sub DeleteGridRow()
        Dim count As Integer
        Dim GVRow As GridViewRow

        count = gvCommFormula.Rows.Count + 1
        Dim FLineNo(count) As String
        Dim cFrom(count) As String
        Dim cTo(count) As String
        Dim cDiscount(count) As String

        Dim n As Integer = 0
        Dim lblFLineNo As Label
        Dim txtFrom As TextBox
        Dim txtTo As TextBox
        Dim txtDiscount As TextBox
        Dim chckDeletion As CheckBox

        Try
            For Each GVRow In gvCommFormula.Rows
                chckDeletion = GVRow.FindControl("chkDelete")
                If chckDeletion.Checked = False Then
                    lblFLineNo = GVRow.FindControl("lblFLineNo")
                    'FLineNo(n) = CType(lblFLineNo.Text, Integer)
                    FLineNo(n) = n + 1
                    txtFrom = GVRow.FindControl("txtFrom")
                    cFrom(n) = CType(txtFrom.Text, String)
                    txtTo = GVRow.FindControl("txtTo")
                    cTo(n) = CType(txtTo.Text, String)
                    txtDiscount = GVRow.FindControl("txtDiscount")
                    cDiscount(n) = CType(txtDiscount.Text, String)
                    n = n + 1
                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(gvCommFormula, False, count)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gvCommFormula.Rows
                If n = i Then
                    Exit For
                End If
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                lblFLineNo.Text = FLineNo(n)
                txtFrom = GVRow.FindControl("txtFrom")
                txtFrom.Text = cFrom(n)
                txtTo = GVRow.FindControl("txtTo")
                txtTo.Text = cTo(n)
                txtDiscount = GVRow.FindControl("txtDiscount")
                txtDiscount.Text = cDiscount(n)
                n = n + 1
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MinimumMarkupPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


End Class
