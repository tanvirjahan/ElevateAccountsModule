
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ExcursionModule_ExcursionStopSales
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
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Page.IsPostBack = False Then
                ViewState.Add("ExcursionStopSalesState", Request.QueryString("State"))
                ViewState.Add("ExcursionStopSalesRefCode", Request.QueryString("RefCode"))

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

                pnlMarket.Visible = False
                Pnlmkt.Visible = False
                gv_Mkt.Visible = False
                btnFillmkt.Style("visibility") = "hidden"

                pnlRooms.Visible = False
                PnlRm.Visible = False
                gv_rm.Visible = False




                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")



                If ViewState("ExcursionStopSalesState") = "New" Then
                    'SetFocus(ddlSpTypeCD)
                    lblHeading.Text = "Add New Excursion Stop Sales"
                    fillDategrd(grdDates, True)
                    dpFromdate.Enabled = False
                    dpToDate.Enabled = False
                    FillMarket()
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Generate')")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("ExcursionStopSalesState") = "View" Then
                    lblHeading.Text = "View Excursion Stop Sales"
                    ShowRecord(ViewState("ExcursionStopSalesRefCode"))
                    ShowAllotmentnew_Details(ViewState("ExcursionStopSalesRefCode"))
                    ShowDates(ViewState("ExcursionStopSalesRefCode"))
                    FillGridMarket("plgrpcode")
                    ShowGridMarket(ViewState("ExcursionStopSalesRefCode"))
                    'RoomDetails()
                    'ShowGridRm(ViewState("ExcursionStopSalesRefCode"))

                    DisableAllControls()
                    SetFocus(btnExit)
                ElseIf ViewState("ExcursionStopSalesState") = "Edit" Then
                    lblHeading.Text = "Edit Excursion Stop Sales"
                    ShowRecord(ViewState("ExcursionStopSalesRefCode"))
                    ShowAllotmentnew_Details(ViewState("ExcursionStopSalesRefCode"))
                    ShowDates(ViewState("ExcursionStopSalesRefCode"))
                    FillGridMarket("plgrpcode")
                    ShowGridMarket(ViewState("ExcursionStopSalesRefCode"))
                    ddlExGrpCode.Disabled = True
                    ddlExGrpName.Disabled = True
                    ddlExTypeCode.Disabled = True
                    ddlExTypeName.Disabled = True
                    btnSave.Enabled = False
                    'RoomDetails()
                    'ShowGridRm(ViewState("ExcursionStopSalesRefCode"))

                    ''DisableAllControls()
                    ''SetFocus(btnExit)
                ElseIf ViewState("ExcursionStopSalesState") = "Delete" Then
                    lblHeading.Text = "View Excursion Stop Sales"
                    ShowRecord(ViewState("ExcursionStopSalesRefCode"))
                    ShowAllotmentnew_Details(ViewState("ExcursionStopSalesRefCode"))
                    ShowDates(ViewState("ExcursionStopSalesRefCode"))
                    FillGridMarket("plgrpcode")
                    ShowGridMarket(ViewState("ExcursionStopSalesRefCode"))
                    btnSave.Text = "Delete"
                   

                    'RoomDetails()
                    'ShowGridRm(ViewState("ExcursionStopSalesRefCode"))

                    DisableAllControls()
                    btnSave.Enabled = True
                    btnGenerate.Enabled = True
                    SetFocus(btnSave)

                End If

           



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
            btnSave.Attributes.Add("onclick", "return FormValidation('" + ViewState("ExcursionStopSalesState") + "')")

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = True Then

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 and a.othgrpcode='" & hdnExcursionGroupCode.Value & "' order by a.othtypcode", True, hdnExcursionTypeName.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 and a.othgrpcode='" & hdnExcursionGroupCode.Value & "' order by a.othtypname", True, hdnExcursionTypeCode.Value)


            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            strSqlQry = "select * from stopsaleexc_header where mstopid='" & RefCode & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = SqlCmd.ExecuteReader
            If mySqlReader.Read = True Then
                If IsDBNull(mySqlReader("mstopid")) = False Then
                    txtAllotmentID.Value = mySqlReader("mstopid")
                End If
                If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    ddlExGrpName.Value = mySqlReader("othgrpcode")
                    hdnsuppliercode.Value = mySqlReader("othgrpcode")

                    ddlExGrpCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
                    hdnExcursionGroupCode.Value = mySqlReader("othgrpcode")
                End If
                If IsDBNull(mySqlReader("othtypcode")) = False Then
                    otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                    otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname", True)

                   
                    ddlExTypeName.Value = mySqlReader("othtypcode")

                    ddlExTypeCode.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", mySqlReader("othtypcode")))
                    ''22102014
                    hdnExcursionTypeCode.Value = mySqlReader("othtypcode")
                    hdnExcursionTypeName.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", mySqlReader("othtypcode")))
                End If
                If IsDBNull(mySqlReader("frmdate")) = False Then
                    dpFromdate.txtDate.Text = Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy")
                End If
                If IsDBNull(mySqlReader("todate")) = False Then
                    dpToDate.txtDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                End If
             
                If IsDBNull(mySqlReader("remarks")) = False Then
                    txtRemark.Value = mySqlReader("remarks")
                End If
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub

#End Region

#Region "Private Sub ShowAllotmentnew_Details(ByVal RefCode As String)"
    Private Sub ShowAllotmentnew_Details(ByVal RefCode As String)
        Dim MyDs As New DataSet
        Try
            ', 0 as overallot added 
            'passed 0 defalut becz don't save this field so in view mode it give error (field not found)


            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = " SELECT 0 as rowid,a.alineno as [lineno], a.plgrpcode as market, a.othtypcode as othtype, b.othtypname as othtypname, convert(varchar(10),a.allotdate,103)as allotdate,convert(varchar(10),a.allotdatec,103) as allotdatec,a.stopsale " & _
                       "   FROM stopsaleexc_detail a inner join  othtypmast b on a.othtypcode= b.othtypcode and a.mstopid='" & RefCode & "'"


            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(MyDs)
            gv_allotment.DataSource = MyDs
            gv_allotment.DataBind()
            SqlConn.Close()
            pnlAllot.Visible = True
            gv_allotment.Visible = True

            'stopsale
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Private Sub ShowDates(ByVal RefCode As String)"

    Private Sub ShowDates(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            SqlCmd = New SqlCommand("Select * from stopsaleexc_date Where mstopid='" & RefCode & "'", SqlConn)
            mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            Dim ct As Integer
            If mySqlReader.HasRows Then
                ct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from stopsaleexc_date Where mstopid='" & RefCode & "'")
                fillDategrd(grdDates, False, ct)
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtPfromdate")
                        dpTDate = gvRow.FindControl("txtPtodate")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                'dpFDate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
                                dpFDate.Text = Format("U", CType((mySqlReader("frmdate")), Date))
                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                'dpTDate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
                                dpTDate.Text = Format("U", CType((mySqlReader("todate")), Date))
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           
        End Try
    End Sub
#End Region


    Private Sub ClearGrid()
        ', 0 as overallot added 
        'passed 0 defalut becz don't save this field so in view mode it give error (field not found)

        Dim MyDs As New DataSet
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "SELECT plgrpcode as [market], stopsalemain_detail.rmtypcode as [roomtype],rmtypmast.rmtypname,alineno AS [lineno],convert(varchar(10),allotdate,103) as allotdate, cancdays AS releaseperiod, alloted, availed, suballoted, suballotrev, stopped, available, minnights, 0 as overallot " & _
                       "FROM stopsalemain_detail,rmtypmast where stopsalemain_detail.rmtypcode=rmtypmast.rmtypcode and mstopid=''"
            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(MyDs)
            gv_allotment.DataSource = MyDs
            gv_allotment.DataBind()
            SqlConn.Close()
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region " Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        Try
            ddlExGrpCode.Disabled = True
            ddlExGrpName.Disabled = True
            ddlExTypeCode.Disabled = True
            ddlExTypeName.Disabled = True
           
            dpFromdate.Enabled = False
            dpToDate.Enabled = False
            txtRemark.Disabled = True
            btnSave.Enabled = False
            btnGenerate.Enabled = False
            btnResetAll.Enabled = False
            btnResetForHotel.Enabled = False
            btnStopAll.Enabled = False
            btnRemoveStopAll.Enabled = False
            txtAllotmentID.Disabled = True
            grdDates.Enabled = False
          
            btnAddLines.Enabled = False
            gv_Mkt.Enabled = False
            chkMarket.Enabled = False
            gv_rm.Enabled = False

            Dim GvRow As GridViewRow
            Dim chk As CheckBox
            For Each GvRow In gv_allotment.Rows
                chk = GvRow.FindControl("ChkStopSale")
                chk.Enabled = False
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Private Sub ShowGridMarket(ByVal RefCode As String)"

    Private Sub ShowGridMarket(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim chk As CheckBox

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            SqlCmd = New SqlCommand("Select * from stopsaleexc_market   Where mstopid='" & RefCode & "'", SqlConn)
            mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In gv_Mkt.Rows
                        chk = gvRow.FindControl("chkSelect")
                        If IsDBNull(mySqlReader("plgrpcode")) = False Then
                            If gvRow.Cells(1).Text.Trim = mySqlReader("plgrpcode") Then
                                chk.Checked = True
                            End If
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region " Private Sub ShowGridRm(ByVal RefCode As String)"

    Private Sub ShowGridRm(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim chk As CheckBox
            Dim lblroomcodee As Label

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            SqlCmd = New SqlCommand("Select * from stopsale_rooms   Where mstopid='" & RefCode & "'", SqlConn)
            mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In gv_rm.Rows
                        chk = gvRow.FindControl("chkSelect")
                        lblroomcodee = gvRow.FindControl("lblroomcode")
                        If IsDBNull(mySqlReader("rmtypcode")) = False Then
                            'If gvRow.Cells(1).Text.Trim = mySqlReader("rmtypcode") Then
                            '    chk.Checked = True
                            'End If
                            If lblroomcodee.Text.Trim = mySqlReader("rmtypcode") Then
                                chk.Checked = True
                            End If
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           
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

#Region " SetDate"
    Public Sub SetDate()
        Try
            Dim gvRow As GridViewRow
            Dim flgdt As Boolean = False
            Dim frmdt As TextBox
            Dim todt As TextBox

            For Each gvRow In grdDates.Rows
                frmdt = gvRow.FindControl("txtPfromdate")
                todt = gvRow.FindControl("txtPtodate")
                If todt.Text <> "" And todt.Text <> "" Then
                    If flgdt = False Then
                        dpFromdate.txtDate.Text = frmdt.Text
                    End If
                    dpToDate.txtDate.Text = todt.Text
                    flgdt = True
                End If
            Next

        Catch ex As Exception
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
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


#Region "Private Sub AddLines()"
    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim n As Integer = 0
        Dim fromdt As TextBox
        Dim todt As TextBox

        Try
            For Each GVRow In grdDates.Rows
                fromdt = GVRow.FindControl("txtPfromdate")
                fDate(n) = CType(fromdt.Text, String)
                todt = GVRow.FindControl("txtPtodate")
                tDate(n) = CType(todt.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If n = i Then
                    Exit For
                End If
                fromdt = GVRow.FindControl("txtPfromdate")
                fromdt.Text = fDate(n)
                todt = GVRow.FindControl("txtPtodate")
                todt.Text = tDate(n)
                n = n + 1
            Next
            'For Each GVRow In grdDates.Rows
            '    txt = GVRow.FindControl("txtPerson")
            '    txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
            '    txt = GVRow.FindControl("txtEmail")
            '    ' txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
            '    txt = GVRow.FindControl("txtContactNo")
            '    txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            'Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
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
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("MainStopSalesSearch.aspx")
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExcursionStopSalesWindowPostBack', '');window.opener.focus();window.close();"
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

#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click 'Handles btnGenerate.Click
        Dim othgrpcode As String
        Dim othtypcode As String
        Dim frmdate As TextBox
        Dim todate As TextBox
        Dim allotpye As String = ""
        Dim rmtypecode As String
        Dim market As String
        Dim includemarkets As String = ""
        Dim cnt As Long = 0
        Dim Flag As Boolean = False
        Dim chk As HtmlInputCheckBox
        Dim GvRow1 As GridViewRow
        Dim GvRow2 As GridViewRow
        Dim chkmkt As CheckBox
        Dim chkrm As CheckBox
        Dim lblroomcodee As Label
        Try

            

            If ValidatePage() = False Then
                Exit Sub
            End If

            If ddlExGrpCode.Value = "[Select]" Then
                othgrpcode = ""
            Else
                othgrpcode = ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text
                'othgrpcode = "SAFARIS"
            End If

            If ddlExTypeCode.Value = "[Select]" Then
                othtypcode = ""
            Else
                othtypcode = ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text
                'othtypcode = "ADVENFUJ"
            End If


           

            Dim Myds As New DataSet
            cnt = 0
            Dim dt As New DataTable
            For Each GvRow1 In gv_Mkt.Rows
                chkmkt = GvRow1.FindControl("chkSelect")
                If chkmkt.Checked = True Then
                    market = GvRow1.Cells(1).Text
                    ' For Each GvRow2 In gv_rm.Rows
                    ' chkrm = GvRow2.FindControl("chkSelect")
                    ' lblroomcodee = GvRow2.FindControl("lblroomcode")
                    'rmtypecode = GvRow2.Cells(1).Text
                    '  rmtypecode = lblroomcodee.Text.Trim()
                    '  If chkrm.Checked = True Then
                    For Each GvRow In grdDates.Rows
                        frmdate = GvRow.FindControl("txtPfromdate")
                        todate = GvRow.FindControl("txtPtodate")
                        If frmdate.Text <> "" And todate.Text <> "" Then
                            strSqlQry = "EXEC sp_gen_stopsalesexc '" & othgrpcode & "','" & othtypcode & "','" & CType(Format(CType(frmdate.Text, Date), "yyyy/MM/dd"), String) & "','" & CType(Format(CType(todate.Text, Date), "yyyy/MM/dd"), String) & "','" & market.Trim & "'"
                            If cnt = 0 Then
                                dt.Columns.Add(New DataColumn("lineno", GetType(String)))
                                dt.Columns.Add(New DataColumn("rowid", GetType(String)))
                                dt.Columns.Add(New DataColumn("market", GetType(String)))
                                dt.Columns.Add(New DataColumn("othtype", GetType(String)))
                                dt.Columns.Add(New DataColumn("othtypname", GetType(String)))
                                dt.Columns.Add(New DataColumn("allotdate", GetType(String)))
                                dt.Columns.Add(New DataColumn("releaseperiod", GetType(String)))
                                dt.Columns.Add(New DataColumn("stopsale", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("alloted", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("availed", GetType(String)))
                                'dt.Columns.Add(New DataColumn("suballoted", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("suballotrev", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("stopped", GetType(String)))
                                '  dt.Columns.Add(New DataColumn("available", GetType(String)))
                                '  dt.Columns.Add(New DataColumn("overallot", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("minnights", GetType(String)))

                                '    dt.Columns.Add(New DataColumn("stopsales", GetType(String)))


                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                SqlCmd = New SqlCommand(strSqlQry, SqlConn)
                                mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                                While mySqlReader.Read = True
                                    Dim dr As DataRow
                                    dr = dt.NewRow
                                    dr(0) = dt.Rows.Count + 1
                                    dr(1) = mySqlReader("rowid")
                                    dr(2) = mySqlReader("market")
                                    dr(3) = mySqlReader("othtype")
                                    dr(4) = mySqlReader("othtypname")
                                    dr(5) = Format(CType(mySqlReader("allotdate"), Date), "dd/MM/yyyy")
                                    dr(6) = mySqlReader("releaseperiod")
                                    dr(7) = mySqlReader("stopsale")
                                    dt.Rows.Add(dr)
                                End While

                                cnt = cnt + 1
                                mySqlReader.Close()
                                SqlConn.Close()
                            Else
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                SqlCmd = New SqlCommand(strSqlQry, SqlConn)
                                mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                                While mySqlReader.Read = True
                                    Dim dr As DataRow
                                    dr = dt.NewRow
                                    dr(0) = dt.Rows.Count + 1
                                    dr(1) = mySqlReader("rowid")
                                    dr(2) = mySqlReader("market")
                                    dr(3) = mySqlReader("othtype")
                                    dr(4) = mySqlReader("othtypname")
                                    dr(5) = Format(CType(mySqlReader("allotdate"), Date), "dd/MM/yyyy")
                                    dr(6) = mySqlReader("releaseperiod")
                                    dr(7) = mySqlReader("stopsale")
                                    dt.Rows.Add(dr)
                                End While

                            End If
                            cnt = cnt + 1
                            mySqlReader.Close()
                            SqlConn.Close()
                        End If
                    Next
                    ' End If
                    ' Next
                End If
            Next

            pnlAllot.Visible = True
            gv_allotment.Visible = True

            gv_allotment.DataSource = dt
            gv_allotment.DataBind()

            If gv_allotment.Rows.Count > 0 Then
                ''2102014
                btnSave.Enabled = True
            End If


            gv_Mkt.Enabled = False
            gv_rm.Enabled = False


            ddlExGrpCode.Disabled = True
            ddlExGrpName.Disabled = True
            ddlExTypeCode.Disabled = True
            ddlExTypeName.Disabled = True

            grdDates.Enabled = False

            txtRemark.Disabled = True
            btnGenerate.Enabled = False
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

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
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GvRow As GridViewRow
        Dim ObjDate As New clsDateTime
        Dim chk As CheckBox
        Dim frmdt As TextBox
        Dim todt As TextBox
        Dim RmCode As HtmlSelect
        Dim lblroomcodee As Label
        ' Dim flag As Boolean = False
        Try
            If Page.IsValid = True Then

                ''If ValidateGrid() = False Then
                ''    Exit Sub
                ''End If

                SetDate()
                Dim minfrmdate As Date
                Dim maxdate As Date


                minfrmdate = gv_allotment.Rows(0).Cells(5).Text
                maxdate = gv_allotment.Rows(gv_allotment.Rows.Count - 1).Cells(5).Text
                dpFromdate.txtDate.Text = minfrmdate
                dpToDate.txtDate.Text = maxdate

                '-----------------------------------------------------------------------------------
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                If Request.QueryString("State") = "New" Then


                    '---------------------------------------------------------------------------------
                    '                 Inserting Into Main  Ticketing Prices
                    '---------------------------------------------------------------------------------
                    Dim optionval As String

                    optionval = objUtils.GetAutoDocNo("EXCSTOP", mySqlConn, sqlTrans)
                    txtAllotmentID.Value = optionval.Trim
                    SqlCmd = New SqlCommand("sp_add_stopsaleexc_header", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure

                    SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 30)).Value = CType(txtAllotmentID.Value.Trim, String)
                    If CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 30)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 30)).Value = CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)
                    End If
                    If CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)
                    End If

                    If dpFromdate.txtDate.Text = "" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd")
                    End If
                    If dpToDate.txtDate.Text = "" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd")
                    End If


                    SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 200)).Value = CType(txtRemark.Value.Trim, String)

                    SqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.ExecuteNonQuery()

                    '---------------------------------------------------------------------------------
                    '                 add for allotment new detail
                    '---------------------------------------------------------------------------------

                    Dim lbl As Label
                    Dim rmtypcode As String
                    Dim othtypcode As String
                    Dim plgrpcode As String

                    For Each GvRow In gv_allotment.Rows
                        plgrpcode = GvRow.Cells(2).Text
                        othtypcode = GvRow.Cells(3).Text

                        SqlCmd = New SqlCommand("sp_add_stopsaleexc_detail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                        lbl = GvRow.FindControl("lblLineno")
                        SqlCmd.Parameters.Add(New SqlParameter("@alineno", SqlDbType.Int)).Value = Val(lbl.Text)
                        If CType(plgrpcode, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(plgrpcode, String)
                        End If
                        If CType(othtypcode, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = CType(othtypcode, String)
                        End If
                        If Trim(GvRow.Cells(5).Text) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdate", SqlDbType.DateTime)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")
                        End If

                        If Trim(GvRow.Cells(5).Text) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdatec", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdatec", SqlDbType.DateTime)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")
                        End If

                        chk = GvRow.FindControl("ChkStopSale")
                        If chk.Checked = False Then
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 0
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 1
                        End If

                        SqlCmd.ExecuteNonQuery()



                    Next

                    '---------------------------------------------------------------------------------
                    '                 add for allotment sp_add_allotments
                    '---------------------------------------------------------------------------------
                    For Each GvRow In gv_allotment.Rows

                        plgrpcode = GvRow.Cells(2).Text
                        rmtypcode = GvRow.Cells(3).Text

                        SqlCmd = New SqlCommand("sp_update_stopsales_exc", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        lbl = GvRow.FindControl("lblrowid")
                        SqlCmd.Parameters.Add(New SqlParameter("@rowid", SqlDbType.BigInt)).Value = Val(lbl.Text)
                        SqlCmd.Parameters.Add(New SqlParameter("@allotdate", SqlDbType.VarChar, 10)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")




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


                        If CType(plgrpcode, String) = "[Select]" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(plgrpcode, String)
                        End If
                        SqlCmd.Parameters.Add(New SqlParameter("@cancdays", SqlDbType.Int)).Value = 99
                        SqlCmd.Parameters.Add(New SqlParameter("@allotdatec", SqlDbType.VarChar, 10)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")
                        chk = GvRow.FindControl("ChkStopSale")
                        If chk.Checked = False Then
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 0
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 1
                        End If
                        SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 200)).Value = CType(txtRemark.Value.Trim, String)

                        SqlCmd.ExecuteNonQuery()
                    Next

                    '----------------------------------- Deleting Data From stopsalemain_Dates Table
                    SqlCmd = New SqlCommand("sp_del_stopsaleexc_date", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)

                    SqlCmd.ExecuteNonQuery()

                    '----------------------------------- Inserting Data To stopsalemain_Dates Table
                    For Each GvRow In grdDates.Rows
                        frmdt = GvRow.FindControl("txtPfromdate")
                        todt = GvRow.FindControl("txtPtodate")
                        If frmdt.Text <> "" And todt.Text <> "" Then
                            SqlCmd = New SqlCommand("sp_add_stopsaleexc_date", mySqlConn, sqlTrans)
                            SqlCmd.CommandType = CommandType.StoredProcedure
                            SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                            SqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(frmdt.Text)
                            SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(todt.Text)

                            SqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    '----------------------------------- Deleting Data From market_detail Table
                    SqlCmd = New SqlCommand("sp_del_stopsaleexc_market_detail", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@allotmentid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                    SqlCmd.ExecuteNonQuery()
                    '----------------------------------- Inserting Data To market_detail Table
                    For Each GvRow In gv_Mkt.Rows
                        chkSel = GvRow.FindControl("chkSelect")
                        If chkSel.Checked = True Then
                            SqlCmd = New SqlCommand("sp_add_stopsaleexc_market_detail", mySqlConn, sqlTrans)
                            SqlCmd.CommandType = CommandType.StoredProcedure
                            SqlCmd.Parameters.Add(New SqlParameter("@allotmentid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                            SqlCmd.Parameters.Add(New SqlParameter("@marketcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text.Trim, String)
                            SqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    btnSave.Enabled = False

                ElseIf Request.QueryString("State") = "Edit" Then '''' ******************************************

                    SqlCmd = New SqlCommand("sp_Edit_stopsaleexc_header", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure

                    SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 30)).Value = CType(txtAllotmentID.Value.Trim, String)
                    If CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 30)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 30)).Value = CType(ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text, String)
                    End If
                    If CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String) = "[Select]" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = CType(ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text, String)
                    End If

                    If dpFromdate.txtDate.Text = "" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = Format(CType(dpFromdate.txtDate.Text, Date), "yyyy/MM/dd")
                    End If
                    If dpToDate.txtDate.Text = "" Then
                        SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(dpToDate.txtDate.Text, Date), "yyyy/MM/dd")
                    End If


                    SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 200)).Value = CType(txtRemark.Value.Trim, String)

                    SqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.ExecuteNonQuery()

                    '---------------------------------------------------------------------------------
                    '                 add for allotment new detail
                    '---------------------------------------------------------------------------------

                    Dim lbl As Label
                    Dim rmtypcode As String
                    Dim othtypcode As String
                    Dim plgrpcode As String

                    SqlCmd = New SqlCommand("sp_Delete_stopsaleexc_detail", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                    SqlCmd.ExecuteNonQuery()



                    For Each GvRow In gv_allotment.Rows
                        plgrpcode = GvRow.Cells(2).Text
                        othtypcode = GvRow.Cells(3).Text

                        SqlCmd = New SqlCommand("sp_Edit_stopsaleexc_detail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                        lbl = GvRow.FindControl("lblLineno")
                        SqlCmd.Parameters.Add(New SqlParameter("@alineno", SqlDbType.Int)).Value = Val(lbl.Text)
                        If CType(plgrpcode, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(plgrpcode, String)
                        End If
                        If CType(othtypcode, String) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 30)).Value = CType(othtypcode, String)
                        End If
                        If Trim(GvRow.Cells(5).Text) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdate", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdate", SqlDbType.DateTime)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")
                        End If

                        If Trim(GvRow.Cells(5).Text) = "" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdatec", SqlDbType.DateTime)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@allotdatec", SqlDbType.DateTime)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")
                        End If

                        chk = GvRow.FindControl("ChkStopSale")
                        If chk.Checked = False Then
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 0
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 1
                        End If

                        SqlCmd.ExecuteNonQuery()



                    Next

                    '---------------------------------------------------------------------------------
                    '                 add for allotment sp_add_allotments
                    '---------------------------------------------------------------------------------
                    For Each GvRow In gv_allotment.Rows

                        plgrpcode = GvRow.Cells(2).Text
                        rmtypcode = GvRow.Cells(3).Text

                        SqlCmd = New SqlCommand("sp_update_stopsales_exc", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        lbl = GvRow.FindControl("lblrowid")
                        SqlCmd.Parameters.Add(New SqlParameter("@rowid", SqlDbType.BigInt)).Value = Val(lbl.Text)
                        SqlCmd.Parameters.Add(New SqlParameter("@allotdate", SqlDbType.VarChar, 10)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")




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


                        If CType(plgrpcode, String) = "[Select]" Then
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(plgrpcode, String)
                        End If
                        SqlCmd.Parameters.Add(New SqlParameter("@cancdays", SqlDbType.Int)).Value = 99
                        SqlCmd.Parameters.Add(New SqlParameter("@allotdatec", SqlDbType.VarChar, 10)).Value = Format(CType(GvRow.Cells(5).Text, Date), "yyyy/MM/dd")
                        chk = GvRow.FindControl("ChkStopSale")
                        If chk.Checked = False Then
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 0
                        Else
                            SqlCmd.Parameters.Add(New SqlParameter("@stopsale", SqlDbType.Int)).Value = 1
                        End If
                        SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 200)).Value = CType(txtRemark.Value.Trim, String)

                        SqlCmd.ExecuteNonQuery()
                    Next

                    '----------------------------------- Deleting Data From stopsalemain_Dates Table
                    SqlCmd = New SqlCommand("sp_del_stopsaleexc_date", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)

                    SqlCmd.ExecuteNonQuery()

                    '----------------------------------- Inserting Data To stopsalemain_Dates Table
                    For Each GvRow In grdDates.Rows
                        frmdt = GvRow.FindControl("txtPfromdate")
                        todt = GvRow.FindControl("txtPtodate")
                        If frmdt.Text <> "" And todt.Text <> "" Then
                            SqlCmd = New SqlCommand("sp_add_stopsaleexc_date", mySqlConn, sqlTrans)
                            SqlCmd.CommandType = CommandType.StoredProcedure
                            SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                            SqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(frmdt.Text)
                            SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(todt.Text)

                            SqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    '----------------------------------- Deleting Data From market_detail Table
                    SqlCmd = New SqlCommand("sp_del_stopsaleexc_market_detail", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@allotmentid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                    SqlCmd.ExecuteNonQuery()
                    '----------------------------------- Inserting Data To market_detail Table
                    For Each GvRow In gv_Mkt.Rows
                        chkSel = GvRow.FindControl("chkSelect")
                        If chkSel.Checked = True Then
                            SqlCmd = New SqlCommand("sp_add_stopsaleexc_market_detail", mySqlConn, sqlTrans)
                            SqlCmd.CommandType = CommandType.StoredProcedure
                            SqlCmd.Parameters.Add(New SqlParameter("@allotmentid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                            SqlCmd.Parameters.Add(New SqlParameter("@marketcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(1).Text.Trim, String)
                            SqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    btnSave.Enabled = False

                ElseIf Request.QueryString("State") = "Delete" Then           ''''  **********************************************

                    fnbtngenclick()

                    Dim lbl As New Label
                    SqlCmd = New SqlCommand("sp_del_stopsaleexc", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@mstopid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)

                    SqlCmd.ExecuteNonQuery()

                    For Each GvRow In gv_allotment.Rows
                        SqlCmd = New SqlCommand("sp_delete_allotmentsexc", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        lbl = GvRow.FindControl("lblrowid")
                        SqlCmd.Parameters.Add(New SqlParameter("@rowid", SqlDbType.BigInt)).Value = Val(lbl.Text)
                        SqlCmd.ExecuteNonQuery()
                    Next
                    btnSave.Enabled = False

                End If

                
                ''----------------------------------- Deleting Data From Room_detail Table
                'SqlCmd = New SqlCommand("sp_del_stopsalerm_detail", mySqlConn, sqlTrans)
                'SqlCmd.CommandType = CommandType.StoredProcedure
                'SqlCmd.Parameters.Add(New SqlParameter("@allotmentid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                'SqlCmd.ExecuteNonQuery()
                ''----------------------------------- Inserting Data To Room_detail Table
                'For Each GvRow In gv_rm.Rows
                '    chkSel = GvRow.FindControl("chkSelect")
                '    lblroomcodee = GvRow.FindControl("lblroomcode")
                '    If chkSel.Checked = True Then
                '        SqlCmd = New SqlCommand("sp_add_stopsalerooms_detail", mySqlConn, sqlTrans)
                '        SqlCmd.CommandType = CommandType.StoredProcedure
                '        SqlCmd.Parameters.Add(New SqlParameter("@allotmentid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                '        SqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(lblroomcodee.Text.Trim, String)
                '        SqlCmd.ExecuteNonQuery()
                '    End If
                'Next

                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Sucessfully');", True)


                'dpFromdate.Enabled = True
                'dpToDate.Enabled = True
                txtRemark.Disabled = False
                ClearGrid()
                txtAllotmentID.Value = ""
                pnlAllot.Visible = False
                gv_allotment.Visible = False
                btnGenerate.Enabled = True

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ExcursionStopSalesWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnResetAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetAll.Click
        Try

            ddlExTypeCode.Items.Clear()
            ddlExTypeCode.Items.Add("[Select]")
            ddlExTypeCode.Value = "[Select]"

            ddlExTypeName.Items.Clear()
            ddlExTypeName.Items.Add("[Select]")
            ddlExTypeName.Value = "[Select]"

            ddlExGrpCode.Value = "[Select]"
            ddlExGrpName.Value = "[Select]"
            ddlExTypeCode.Value = "[Select]"
            ddlExTypeName.Value = "[Select]"

            ddlExGrpCode.Disabled = False
            ddlExGrpName.Disabled = False
            ddlExTypeCode.Disabled = False
            ddlExTypeName.Disabled = False
            grdDates.Enabled = True

            ddlExGrpCode.Value = "[Select]"
            ddlExGrpName.Value = "[Select]"
            ddlExTypeCode.Value = "[Select]"
            ddlExTypeName.Value = "[Select]"

            txtAllotmentID.Disabled = True
            btnGenerate.Enabled = True


            fillDategrd(grdDates, True)

           
            btnFillmkt.Style("visibility") = "hidden"

            pnlRooms.Visible = False
            PnlRm.Visible = False
            gv_rm.Visible = False

            FillMarket()
            gv_Mkt.Enabled = True
            gv_allotment.DataSource = Nothing
            gv_allotment.DataBind()
            pnlAllot.Visible = False
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Function ValidateGrid() As Boolean
        ValidateGrid = True
        Dim GrdFlag As Boolean = False
        Dim GvRow As GridViewRow
        Dim chk As CheckBox

        For Each GvRow In gv_Mkt.Rows
            chk = GvRow.FindControl("chkSelect")
            If chk.Checked = True Then
                GrdFlag = True
            End If
        Next
        If GrdFlag = False Then
            ValidateGrid = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select atleast one Market .');", True)
            Exit Function
        End If
        GrdFlag = False

        For Each GvRow In gv_rm.Rows
            chk = GvRow.FindControl("chkSelect")
            If chk.Checked = True Then
                GrdFlag = True
            End If
        Next
        'If GrdFlag = False Then
        '    ValidateGrid = False
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select atleast one Room Type .');", True)
        '    Exit Function
        'End If
        GrdFlag = False

        For Each GvRow In gv_allotment.Rows
            GrdFlag = True
            Exit For
        Next
        If GrdFlag = False Then
            ValidateGrid = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Grid can not be left blank.');", True)
            Exit Function
        End If
        GrdFlag = False
        For Each GvRow In gv_allotment.Rows
            chk = GvRow.FindControl("ChkStopSale")
            If chk.Checked = True Then
                GrdFlag = True
            End If
        Next
    
    End Function

#Region "Public Function ValidatePage() As Boolean"
    Public Function ValidatePage() As Boolean
        Try
            Dim gvRow As GridViewRow
            Dim flgdt As Boolean = False
            Dim ToDt As TextBox
            Dim fromdt As TextBox
            Dim chk As CheckBox
            Dim flgmkt As Boolean = False
            Dim flgrm As Boolean = False
            Dim strmktcode As String = ""

            For Each gvRow In grdDates.Rows
                fromdt = gvRow.FindControl("txtPfromdate")
                ToDt = gvRow.FindControl("txtPtodate")
                If fromdt.Text <> "" And ToDt.Text <> "" Then

                    If ObjDate.ConvertDateromTextBoxToDatabase(ToDt.Text) < ObjDate.ConvertDateromTextBoxToDatabase(fromdt.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(dpTDate.txtDate)
                        ValidatePage = False
                        Exit Function
                    End If

                    flgdt = True
                End If
            Next

            For Each gvRow In gv_Mkt.Rows
                chk = gvRow.FindControl("chkSelect")
                If chk.Checked = True Then
                    flgmkt = True
                    Exit For
                End If
            Next
            If flgmkt = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select a Market.');", True)
                ValidatePage = False
                Exit Function
            End If

            For Each gvRow In gv_rm.Rows
                chk = gvRow.FindControl("chkSelect")
                If chk.Checked = True Then
                    flgrm = True
                    Exit For
                End If
            Next
            'If flgrm = False Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select a Room.');", True)
            '    ValidatePage = False
            '    Exit Function
            'End If


            If flgdt = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Stop Sales Dates grid should not be blank.');", True)
                SetFocus(grdDates)
                ValidatePage = False
                Exit Function
            End If

            ''validate_freesales begin
            'Dim lblroomcodee As Label
            'Dim dt As New DataTable
            ''Dim txtReleasePeriod As HtmlInputText
            'Dim Alflg As Long
            'For Each gvRow1 In gv_Mkt.Rows
            '    chk = gvRow1.FindControl("chkSelect")
            '    If chk.Checked = True Then

            '        strmktcode = gvRow1.Cells(1).Text.Trim

            '        For Each gvRow In grdDates.Rows

            '            fromdt = gvRow.FindControl("txtPfromdate")
            '            ToDt = gvRow.FindControl("txtPtodate")
            '            'ddlRmCode = gvRow.FindControl("ddlRmTypeCode")
            '            'txtReleasePeriod = gvRow.FindControl("txtReleasePeriod")
            '            If fromdt.Text <> "" And ToDt.Text <> "" Then

            '                For Each gvRow2 In gv_rm.Rows
            '                    chkSel = gvRow2.FindControl("chkSelect")
            '                    lblroomcodee = gvRow2.FindControl("lblroomcode")
            '                    If chkSel.Checked = True Then
            '                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            '                        'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            '                        SqlCmd = New SqlCommand("sp_validate_freesales", SqlConn)
            '                        SqlCmd.CommandType = CommandType.StoredProcedure

            '                        'If ddlsuppagentCode.Value = "[Select]" Then
            '                        '    SqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = ""
            '                        'Else
            '                        '    SqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(ddlsuppagentCode.Items(ddlsuppagentCode.SelectedIndex).Text, String)

            '                        'End If
            '                        'If ddlSuppierCD.Value = "[Select]" Then
            '                        '    SqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = ""
            '                        'Else
            '                        '    SqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text
            '                        'End If
            '                        If fromdt.Text = "" Then
            '                            SqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = ""
            '                        Else
            '                            SqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = CType(Format(ObjDate.ConvertDateromTextBoxToDatabase(fromdt.Text), "yyyy/MM/dd"), String)
            '                        End If
            '                        If ToDt.Text = "" Then
            '                            SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = ""
            '                        Else
            '                            SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = CType(Format(ObjDate.ConvertDateromTextBoxToDatabase(ToDt.Text), "yyyy/MM/dd"), String)
            '                        End If

            '                        If lblroomcodee.Text.Trim() = "" Then
            '                            SqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = ""
            '                        Else
            '                            SqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = lblroomcodee.Text.Trim()
            '                        End If
            '                        If strmktcode = "" Then
            '                            SqlCmd.Parameters.Add(New SqlParameter("@mktcode", SqlDbType.VarChar, 20)).Value = ""
            '                        Else
            '                            SqlCmd.Parameters.Add(New SqlParameter("@mktcode", SqlDbType.VarChar, 20)).Value = strmktcode
            '                        End If


            '                        Dim param1 As SqlParameter
            '                        param1 = New SqlParameter
            '                        param1.ParameterName = "@errflg"
            '                        param1.Direction = ParameterDirection.Output
            '                        param1.DbType = DbType.Int16
            '                        param1.Size = 9
            '                        SqlCmd.Parameters.Add(param1)
            '                        myDataAdapter = New SqlDataAdapter(SqlCmd)
            '                        SqlCmd.ExecuteNonQuery()
            '                        Alflg = param1.Value

            '                        If Alflg = 1 Then
            '                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Free Sale Entered for some dates!!');", True)
            '                            'chkallowstop.Visible = True
            '                            ValidatePage = True
            '                            Exit Function
            '                        End If
            '                    End If
            '                Next

            '            End If

            '        Next
            '    End If

            'Next
            ''validate_freesales end
            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

    Protected Sub btnResetForHotel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'dpFromdate.Enabled = True
            'dpToDate.Enabled = True
            txtRemark.Disabled = False
            txtAllotmentID.Value = ""
            pnlAllot.Visible = False
            gv_allotment.Visible = False
            btnGenerate.Enabled = True
            gv_Mkt.Enabled = True
            gv_rm.Enabled = True
            'fillDategrd(grdDates, True)
            'FillMarket()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnStopAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnStopAll.Click
        Dim chk As CheckBox
        Dim GvRow As GridViewRow
        Try
            For Each GvRow In gv_allotment.Rows
                If CType(GvRow.Cells(2).Text, String) <> "" Then
                    chk = GvRow.FindControl("ChkStopSale")
                    chk.Checked = True
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub btnRemoveStopAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        Dim GvRow As GridViewRow
        Try
            For Each GvRow In gv_allotment.Rows
                chk = GvRow.FindControl("ChkStopSale")
                chk.Checked = False
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub gv_allotment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_allotment.RowDataBound
        Dim lblStopSale As Label
        Dim ChkStopSale As CheckBox
        If e.Row.RowType = DataControlRowType.DataRow Then
            lblStopSale = e.Row.FindControl("lblStopSale")
            ChkStopSale = e.Row.FindControl("ChkStopSale")
            If Trim(lblStopSale.Text) = "1" Then
                ChkStopSale.Checked = True
            Else
                ChkStopSale.Checked = False
            End If
        End If
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MainAllotStopSales','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub grdDates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDates.RowDataBound
        Dim fromdt As TextBox
        Dim todt As TextBox

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            fromdt = CType(e.Row.FindControl("txtPfromdate"), TextBox)
            todt = CType(e.Row.FindControl("txtPtodate"), TextBox)

            fromdt.Attributes.Add("onchange", "javascript:ChangeDate('" + CType(fromdt.ClientID, String) + "','" + CType(todt.ClientID, String) + "')")

        End If
    End Sub

    Protected Sub btnFillmkt_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gv_Mkt.Enabled = True
        gv_rm.Enabled = True
        FillMarket()

    End Sub

#Region "Public Sub FillMarket()"
    Public Sub FillMarket()

        FillGridMarket("plgrpcode")
        '  RoomDetails()

    End Sub
#End Region

#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        gv_Mkt.Visible = True
        If gv_Mkt.PageIndex < 0 Then
            gv_Mkt.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            Dim sqlstr As String


            sqlstr = "select plgrpcode,plgrpname from  plgrpmast where active=1 order by plgrpcode "

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(sqlstr, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_Mkt.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Mkt.DataBind()
                Pnlmkt.Visible = True
                pnlMarket.Visible = True
                gv_Mkt.Visible = True
            Else
                gv_Mkt.DataBind()
                Pnlmkt.Visible = False
                pnlMarket.Visible = False
                gv_Mkt.Visible = False
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
    End Sub
#End Region

    Protected Sub chkrooms_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkRooms.Checked = True Then
            For Each gvRow In gv_rm.Rows
                chkSel = gvRow.FindControl("chkSelect")
                chkSel.Checked = True
            Next
        ElseIf chkRooms.Checked = False Then
            For Each gvRow In gv_rm.Rows
                chkSel = gvRow.FindControl("chkSelect")
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub chkMarket_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkMarket.Checked = True Then
            For Each gvRow In gv_Mkt.Rows
                chkSel = gvRow.FindControl("chkSelect")
                chkSel.Checked = True
            Next
        ElseIf chkMarket.Checked = False Then
            For Each gvRow In gv_Mkt.Rows
                chkSel = gvRow.FindControl("chkSelect")
                chkSel.Checked = False
            Next
        End If
    End Sub

#Region "Public RoomDetails()"
    Public Sub RoomDetails()
        FillGridRoomTypes()
    End Sub
#End Region

#Region "Private Sub FillGridRoomTypes()"
    Private Sub FillGridRoomTypes()

        Dim myDS As New DataSet
        gv_rm.Visible = True
        If gv_rm.PageIndex < 0 Then
            gv_rm.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            Dim sqlstr As String

            'If ddlSuppierCD.Value <> "[Select]" Then 'distinct
            '    sqlstr = "select  partyrmtyp.rmtypcode,rmtypmast.rmtypname from partyrmtyp inner join rmtypmast on partyrmtyp.rmtypcode = rmtypmast.rmtypcode   "
            '    sqlstr = sqlstr + " where partyrmtyp.inactive=0 and partyrmtyp.partycode='" & ddlSuppierCD.Items(ddlSuppierCD.SelectedIndex).Text & "'"
            '    sqlstr = sqlstr + "  order by partyrmtyp.rankord"

            '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            '    myDataAdapter = New SqlDataAdapter(sqlstr, SqlConn)
            '    myDataAdapter.Fill(myDS)
            '    gv_rm.DataSource = myDS


            '    If myDS.Tables(0).Rows.Count > 0 Then
            '        gv_rm.DataBind()
            '        PnlRm.Visible = True
            '        pnlRooms.Visible = True
            '        gv_rm.Visible = True
            '    Else
            '        gv_rm.DataBind()
            '        PnlRm.Visible = False
            '        pnlRooms.Visible = False
            '        gv_rm.Visible = False
            '    End If
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
    End Sub
#End Region

    Private Function fnbtngenclick()
        Dim othgrpcode As String
        Dim othtypcode As String
        Dim frmdate As TextBox
        Dim todate As TextBox
        Dim allotpye As String = ""
        Dim rmtypecode As String
        Dim market As String
        Dim includemarkets As String = ""
        Dim cnt As Long = 0
        Dim Flag As Boolean = False
        Dim chk As HtmlInputCheckBox
        Dim GvRow1 As GridViewRow
        Dim GvRow2 As GridViewRow
        Dim chkmkt As CheckBox
        Dim chkrm As CheckBox
        Dim lblroomcodee As Label
        Try



            If ValidatePage() = False Then
                Exit Function
            End If

            If ddlExGrpCode.Value = "[Select]" Then
                othgrpcode = ""
            Else
                othgrpcode = ddlExGrpCode.Items(ddlExGrpCode.SelectedIndex).Text
                'othgrpcode = "SAFARIS"
            End If

            If ddlExTypeCode.Value = "[Select]" Then
                othtypcode = ""
            Else
                othtypcode = ddlExTypeCode.Items(ddlExTypeCode.SelectedIndex).Text
                'othtypcode = "ADVENFUJ"
            End If




            Dim Myds As New DataSet
            cnt = 0
            Dim dt As New DataTable
            For Each GvRow1 In gv_Mkt.Rows
                chkmkt = GvRow1.FindControl("chkSelect")
                If chkmkt.Checked = True Then
                    market = GvRow1.Cells(1).Text
                    ' For Each GvRow2 In gv_rm.Rows
                    ' chkrm = GvRow2.FindControl("chkSelect")
                    ' lblroomcodee = GvRow2.FindControl("lblroomcode")
                    'rmtypecode = GvRow2.Cells(1).Text
                    '  rmtypecode = lblroomcodee.Text.Trim()
                    '  If chkrm.Checked = True Then
                    For Each GvRow In grdDates.Rows
                        frmdate = GvRow.FindControl("txtPfromdate")
                        todate = GvRow.FindControl("txtPtodate")
                        If frmdate.Text <> "" And todate.Text <> "" Then
                            strSqlQry = "EXEC sp_gen_stopsalesexc '" & othgrpcode & "','" & othtypcode & "','" & CType(Format(CType(frmdate.Text, Date), "yyyy/MM/dd"), String) & "','" & CType(Format(CType(todate.Text, Date), "yyyy/MM/dd"), String) & "','" & market.Trim & "'"
                            If cnt = 0 Then
                                dt.Columns.Add(New DataColumn("lineno", GetType(String)))
                                dt.Columns.Add(New DataColumn("rowid", GetType(String)))
                                dt.Columns.Add(New DataColumn("market", GetType(String)))
                                dt.Columns.Add(New DataColumn("othtype", GetType(String)))
                                dt.Columns.Add(New DataColumn("othtypname", GetType(String)))
                                dt.Columns.Add(New DataColumn("allotdate", GetType(String)))
                                dt.Columns.Add(New DataColumn("releaseperiod", GetType(String)))
                                dt.Columns.Add(New DataColumn("stopsale", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("alloted", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("availed", GetType(String)))
                                'dt.Columns.Add(New DataColumn("suballoted", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("suballotrev", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("stopped", GetType(String)))
                                '  dt.Columns.Add(New DataColumn("available", GetType(String)))
                                '  dt.Columns.Add(New DataColumn("overallot", GetType(String)))
                                ' dt.Columns.Add(New DataColumn("minnights", GetType(String)))

                                '    dt.Columns.Add(New DataColumn("stopsales", GetType(String)))


                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                SqlCmd = New SqlCommand(strSqlQry, SqlConn)
                                mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                                While mySqlReader.Read = True
                                    Dim dr As DataRow
                                    dr = dt.NewRow
                                    dr(0) = dt.Rows.Count + 1
                                    dr(1) = mySqlReader("rowid")
                                    dr(2) = mySqlReader("market")
                                    dr(3) = mySqlReader("othtype")
                                    dr(4) = mySqlReader("othtypname")
                                    dr(5) = Format(CType(mySqlReader("allotdate"), Date), "dd/MM/yyyy")
                                    dr(6) = mySqlReader("releaseperiod")
                                    dr(7) = mySqlReader("stopsale")
                                    dt.Rows.Add(dr)
                                End While

                                cnt = cnt + 1
                                mySqlReader.Close()
                                SqlConn.Close()
                            Else
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                SqlCmd = New SqlCommand(strSqlQry, SqlConn)
                                mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                                While mySqlReader.Read = True
                                    Dim dr As DataRow
                                    dr = dt.NewRow
                                    dr(0) = dt.Rows.Count + 1
                                    dr(1) = mySqlReader("rowid")
                                    dr(2) = mySqlReader("market")
                                    dr(3) = mySqlReader("othtype")
                                    dr(4) = mySqlReader("othtypname")
                                    dr(5) = Format(CType(mySqlReader("allotdate"), Date), "dd/MM/yyyy")
                                    dr(6) = mySqlReader("releaseperiod")
                                    dr(7) = mySqlReader("stopsale")
                                    dt.Rows.Add(dr)
                                End While

                            End If
                            cnt = cnt + 1
                            mySqlReader.Close()
                            SqlConn.Close()
                        End If
                    Next
                    ' End If
                    ' Next
                End If
            Next

            pnlAllot.Visible = True
            gv_allotment.Visible = True

            gv_allotment.DataSource = dt
            gv_allotment.DataBind()

            If gv_allotment.Rows.Count > 0 Then
                ''2102014
                btnSave.Enabled = True
            End If


            gv_Mkt.Enabled = False
            gv_rm.Enabled = False


            ddlExGrpCode.Disabled = True
            ddlExGrpName.Disabled = True
            ddlExTypeCode.Disabled = True
            ddlExTypeName.Disabled = True

            grdDates.Enabled = False

            txtRemark.Disabled = True
            btnGenerate.Enabled = False
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

End Class


