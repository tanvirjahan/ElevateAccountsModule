Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class DiscountFormula
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
                ViewState.Add("DiscountFormulaState", Request.QueryString("State"))
                ViewState.Add("DiscountFormulaID", Request.QueryString("FormulaID"))

                Session("DiscountFormulaState") = "New"
                ucMinMarkupPolicy.sbSetPageState("", "DiscountFormulaState", CType(Session("DiscountFormulaState"), String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                ucMinMarkupPolicy.sbShowCountry()
                fillDategrd(gvCommFormula, False, 1)
                If ViewState("DiscountFormulaState") = "New" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Discount Formula"
                    Page.Title = Page.Title + " " + "New Discount Formula"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                    TxtCurrencyName.Text = "U S DOLLAR"
                    TextCurrencyCode.Text = "USD"

                ElseIf ViewState("DiscountFormulaState") = "Edit" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Edit Minimum Markup Formula"
                    Page.Title = Page.Title + " " + "Edit Discount Formula"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("DiscountFormulaID"), String))
                    ShowCommFormula(CType(ViewState("DiscountFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    DisableControl()
                ElseIf ViewState("DiscountFormulaState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Minimum Markup Formula"
                    Page.Title = Page.Title + " " + "View Discount Formula"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    ShowRecord(CType(ViewState("DiscountFormulaID"), String))
                    ShowCommFormula(CType(ViewState("DiscountFormulaID"), String))
                    DisableControl()
                ElseIf ViewState("DiscountFormulaState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Discount Formula"
                    Page.Title = Page.Title + " " + "Delete Discount Formula"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("DiscountFormulaID"), String))
                    ShowCommFormula(CType(ViewState("DiscountFormulaID"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("DiscountFormulaState") = "Copy" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Discount Formula"
                    Page.Title = Page.Title + " " + "New Discount Formula"
                    btnSave.Text = "Save"
                    ShowRecord(CType(ViewState("DiscountFormulaID"), String))
                    ShowCommFormula(CType(ViewState("DiscountFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                    TxtCurrencyName.Text = "U S DOLLAR"
                    TextCurrencyCode.Text = "USD"
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Page.Title = "Discount Formula Entry"
    End Sub
#End Region

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


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            'If Page.IsValid = True Then
            '    '  If ViewState("DiscountFormulaState") = "New" Or ViewState("DiscountFormulaState") = "Edit" Then
            '    If ViewState("DiscountFormulaState") = "New" Or ViewState("DiscountFormulaState") = "Edit" Or ViewState("DiscountFormulaState") = "Copy" Then
            '        If checkForDuplicate() = False Then
            '            Exit Sub
            '        End If
            '        If ValidateFormulaTerm() = False Then
            '            Exit Sub
            '        End If
            '        If ValidateSlabRange() = False Then
            '            Exit Sub
            '        End If
            '    End If
            '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            '    sqlTrans = mySqlConn.BeginTransaction
            '    Dim strOpMode As String = ""
            '    If ViewState("DiscountFormulaState") = "New" Or ViewState("DiscountFormulaState") = "Copy" Then
            '        Dim optionval As String
            '        optionval = objUtils.GetAutoDocNo("MARKFORM", mySqlConn, sqlTrans)
            '        txtcode.Value = optionval.Trim
            '    End If
            '    mySqlCmd = New SqlCommand("SP_MarkupFormula", mySqlConn, sqlTrans)
            '    If ViewState("DiscountFormulaState") = "New" Or ViewState("DiscountFormulaState") = "Copy" Then
            '        strOpMode = "1"
            '    ElseIf ViewState("DiscountFormulaState") = "Edit" Then
            '        strOpMode = "2"
            '    ElseIf ViewState("DiscountFormulaState") = "Delete" Then
            '        strOpMode = "3"
            '    End If


            '    '----------------------------------- Create XML for insert markup formula -------------------------------

            '    Dim lblFLineNo As Label
            '    Dim n As Integer = 0
            '    Dim txtFrom As TextBox
            '    Dim txtTo As TextBox
            '    Dim ddlOperator1 As DropDownList
            '    Dim txtValue As TextBox
            '    Dim txtAdult As TextBox
            '    Dim txtChild As TextBox
            '    Dim txtExtraBed As TextBox



            '    Dim strBuffer As New Text.StringBuilder
            '    Dim count = grdCommFormula.Rows.Count
            '    Dim iRpwCnt As Integer = 0
            '    strBuffer.Append("<FormulaDetails>")
            '    For Each GVRow In grdCommFormula.Rows
            '        lblFLineNo = GVRow.FindControl("lblFLineNo")
            '        txtFrom = GVRow.FindControl("txtFrom")
            '        txtTo = GVRow.FindControl("txtTo")
            '        ddlOperator1 = GVRow.FindControl("ddlOperator")
            '        txtValue = GVRow.FindControl("txtValue")
            '        txtAdult = GVRow.FindControl("txtAdults")
            '        txtChild = GVRow.FindControl("txtChild")
            '        txtExtraBed = GVRow.FindControl("txtExtraBed")

            '        strBuffer.Append("<FormulaDetail>")
            '        strBuffer.Append(" <FLineNo>" & lblFLineNo.Text.Trim & "</FLineNo>")
            '        strBuffer.Append(" <From>" & txtFrom.Text.Trim & " </From>")
            '        Dim strTo As String = ""
            '        If txtTo.Text.Trim = "" Then
            '            strTo = "0"
            '            strBuffer.Append(" <lastslab>1</lastslab>")
            '        Else
            '            strTo = txtTo.Text.Trim
            '            strBuffer.Append(" <lastslab>0</lastslab>")
            '        End If
            '        strBuffer.Append(" <To>" & strTo & " </To>")
            '        strBuffer.Append(" <Operator>" & ddlOperator1.SelectedItem.Text.Trim & " </Operator>")
            '        If txtValue.Text.Trim <> "" Then
            '            strBuffer.Append(" <Value>" & txtValue.Text.Trim & "</Value>")
            '        Else
            '            strBuffer.Append(" <Value>0</Value>")
            '        End If
            '        If txtAdult.Text.Trim <> "" Then
            '            strBuffer.Append(" <Adults>" & txtAdult.Text.Trim & "</Adults>")
            '        Else
            '            strBuffer.Append(" <Adults>0</Adults>")
            '        End If
            '        If txtChild.Text.Trim <> "" Then
            '            strBuffer.Append(" <Child>" & txtChild.Text.Trim & "</Child>")
            '        Else
            '            strBuffer.Append(" <Child>0</Child>")
            '        End If
            '        If txtChild.Text.Trim <> "" Then
            '            strBuffer.Append(" <ExtraBed>" & txtExtraBed.Text.Trim & "</ExtraBed>")
            '        Else
            '            strBuffer.Append(" <ExtraBed>0</ExtraBed>")
            '        End If

            '        strBuffer.Append("</FormulaDetail>")
            '    Next

            '    strBuffer.Append("</FormulaDetails>")

            '    '-----------------------------------------------------------



            '    mySqlCmd.CommandType = CommandType.StoredProcedure
            '    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaId", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
            '    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaName", SqlDbType.VarChar, 1000)).Value = CType(txtname.Value.Trim, String)
            '    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaDesc", SqlDbType.VarChar, 1000)).Value = CType(txtApplicableTo.Text.Trim, String)
            '    mySqlCmd.Parameters.Add(New SqlParameter("@CurrCode", SqlDbType.VarChar, 20)).Value = CType(TextCurrencyCode.Text.Trim, String)
            '    If chkActive.Checked = True Then
            '        mySqlCmd.Parameters.Add(New SqlParameter("@Active", SqlDbType.Int)).Value = 1
            '    ElseIf chkActive.Checked = False Then
            '        mySqlCmd.Parameters.Add(New SqlParameter("@Active", SqlDbType.Int)).Value = 0
            '    End If

            '    mySqlCmd.Parameters.Add(New SqlParameter("@AddUser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            '    mySqlCmd.Parameters.Add(New SqlParameter("@ModUser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            '    mySqlCmd.Parameters.Add(New SqlParameter("@MarkupXMLInput", SqlDbType.Xml)).Value = strBuffer.ToString
            '    mySqlCmd.Parameters.Add(New SqlParameter("@OpMode", SqlDbType.Int)).Value = strOpMode
            '    mySqlCmd.ExecuteNonQuery()


            '    'ElseIf ViewState("DiscountFormulaState") = "Delete" Then
            '    '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            '    '    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
            '    '    mySqlCmd = New SqlCommand("sp_del_CommissionFormula", mySqlConn, sqlTrans)
            '    '    mySqlCmd.CommandType = CommandType.StoredProcedure
            '    '    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaID", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
            '    '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            '    '    mySqlCmd.ExecuteNonQuery()
            '    'End If

            '    sqlTrans.Commit()    'SQl Tarn Commit
            '    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            '    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            '    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

            '    If Not Request.QueryString("Page") Is Nothing Then
            '        If Request.QueryString("Page") = "applymarkup" Then
            '            Dim strscript1 As String = ""
            '            strscript1 = "window.opener.__doPostBack('ApplyMarkupWindowPostBack', '');window.opener.focus();window.close();"
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
            '        Else
            '            Dim strscript As String = ""
            '            strscript = "window.opener.__doPostBack('MarkupWindowPostBack', '');window.opener.focus();window.close();"
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            '        End If
            '    Else
            '        Dim strscript As String = ""
            '        strscript = "window.opener.__doPostBack('MarkupWindowPostBack', '');window.opener.focus();window.close();"
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            '    End If


            'End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If (ViewState("DiscountFormulaState") = "New") Or ViewState("DiscountFormulaState") = "Copy" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "markupformula_header", "formulaName", txtname.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This markup formula name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf (ViewState("DiscountFormulaState") = "Edit") Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "markupformula_header", "formulaid", "formulaName", txtname.Value.Trim, CType(txtcode.Value.Trim, String)) Then
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
        If ViewState("DiscountFormulaState") = "View" Or ViewState("DiscountFormulaState") = "Delete" Then
            txtcode.Disabled = True
            txtname.Disabled = True
            txtApplicableTo.Enabled = False
            chkActive.Disabled = True
        ElseIf ViewState("DiscountFormulaState") = "Edit" Then
            txtcode.Disabled = True
        End If
    End Sub
#End Region
#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("select *,(select currname from currmast c where c.currcode=markupformula_header.currcode)currname from markupformula_header Where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("FormulaID")) = False Then
                        If ViewState("DiscountFormulaState") <> "Copy" Then
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
                    If IsDBNull(mySqlReader("formuladesc")) = False Then
                        Me.txtApplicableTo.Text = CType(mySqlReader("formuladesc"), String)
                    Else
                        Me.txtApplicableTo.Text = ""
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "markupformula_detail", "count(FormulaID)", "FormulaID", RefCode)
            If lngCnt = 0 Then lngCnt = 1

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("select * from markupformula_detail where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            Dim n As Integer = 1
            If mySqlReader.HasRows Then
                While mySqlReader.Read()

                    n = n + 1

                End While
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbCloseSearch_Click(sender As Object, e As System.EventArgs)
        Try
            ucMinMarkupPolicy.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnAddRow_Click1(sender As Object, e As System.EventArgs)
        AddGridRow()
    End Sub

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
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
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
#End Region

#Region "Private Sub AddGridRow()"
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
#End Region

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


    Protected Sub btnAddRow_Click(sender As Object, e As System.EventArgs) 'Handles btnAddRow0.Click
        AddGridRow()
    End Sub

    Protected Sub btnDeleteRow_Click(sender As Object, e As System.EventArgs) Handles btnDeleteRow.Click
        DeleteGridRow()
    End Sub

#Region "Private Sub DeleteGridRow()"
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
            objUtils.WritErrorLog("DiscountFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnGridAddRow_Click(sender As Object, e As System.EventArgs)
        AddGridRow()
    End Sub

    Protected Sub grdCommFormula_DataBound(sender As Object, e As System.EventArgs) Handles gvCommFormula.DataBound
     
    End Sub

    Protected Sub grdCommFormula_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCommFormula.RowCreated

    End Sub



    Private Sub gvCommFormula_HeaderRow_Merging()

        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

        Dim cell As New TableHeaderCell()
        cell.Text = "Serial No"
        cell.ColumnSpan = 1
        cell.RowSpan = 2
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.Text = "Per Person Profit Slabs"
        cell.ColumnSpan = 2
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 1
        cell.Text = "Discount Applicable on Differential M.U"
        cell.RowSpan = 2
        cell.Width = 150
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 1
        cell.Text = "Action"
        cell.RowSpan = 2
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 1
        cell.Text = "Delete"
        cell.RowSpan = 2
        row.Controls.Add(cell)

        '  row.BackColor = ColorTranslator.FromHtml("#3AC0F2")
        gvCommFormula.HeaderRow.Parent.Controls.AddAt(0, row)
        ' e.Row.Parent.Controls.AddAt(0, row)
    End Sub




    Protected Sub gvCommFormula_PreRender(sender As Object, e As System.EventArgs) Handles gvCommFormula.PreRender



        gvCommFormula_HeaderRow_Merging()

        If hdFlag.Value = "1" Then
            Dim gvheadRow As GridViewRow = gvCommFormula.HeaderRow
            gvheadRow.Cells(0).Visible = False
            gvheadRow.Cells(3).Visible = False
            gvheadRow.Cells(4).Visible = False
            gvheadRow.Cells(5).Visible = False
            If gvCommFormula.Rows.Count > 0 Then
                Dim Row As GridViewRow = gvCommFormula.Rows(0)
                Row.Cells(0).Visible = True
                Row.Cells(3).Visible = True
                Row.Cells(4).Visible = True
                Row.Cells(5).Visible = True
            End If

        Else
            Dim gvheadRow As GridViewRow = gvCommFormula.HeaderRow
            gvheadRow.Cells(0).Visible = False
            gvheadRow.Cells(3).Visible = False
            gvheadRow.Cells(4).Visible = False
            gvheadRow.Cells(5).Visible = False
        End If
        hdFlag.Value = "1"
    End Sub
End Class
