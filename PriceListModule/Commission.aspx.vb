Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class Commission
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

#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("CommissionState", Request.QueryString("State"))
                ViewState.Add("CommissionFormulaID", Request.QueryString("FormulaID"))
                charcters(txtNewTermCode)
                FillTerms()
                fillDategrd(grdCommFormula, False, 5)
                If ViewState("CommissionState") = "New" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Commission"
                    Page.Title = Page.Title + " " + "New Commission"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("CommissionState") = "Edit" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Edit Commission"
                    Page.Title = Page.Title + " " + "Edit Commission"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("CommissionFormulaID"), String))
                    ShowCommFormula(CType(ViewState("CommissionFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    DisableControl()
                ElseIf ViewState("CommissionState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Commission"
                    Page.Title = Page.Title + " " + "View Commission"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    ShowRecord(CType(ViewState("CommissionFormulaID"), String))
                    ShowCommFormula(CType(ViewState("CommissionFormulaID"), String))
                    DisableControl()
                ElseIf ViewState("CommissionState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Commission"
                    Page.Title = Page.Title + " " + "Delete Commission"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("CommissionFormulaID"), String))
                    ShowCommFormula(CType(ViewState("CommissionFormulaID"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Commission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Page.Title = "Commission Entry"
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
        dt.Columns.Add(New DataColumn("fLineNo", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        CreateDataSource = New DataView(dt)
    End Function
#End Region
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CommissionEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Sub FillTerms()
        Try
            Dim strSqlQry As String
            Dim myDS As New DataSet
            strSqlQry = "select RankOrder,TermCode,TermName,SystemValue from commissionterms order by rankOrder"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdCommTerms.DataSource = myDS
            grdCommTerms.DataBind()
            clsDBConnect.dbAdapterClose(myDataAdapter)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CommissionSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try
    End Sub

    Protected Sub btnAddTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("CommissionTermMode") = "New"
        lblTermTitle.Text = "New Commission Term"
        ModalExtraPopup.Show()
    End Sub

    Protected Sub btnAddRow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddGridRow()
    End Sub

    Protected Sub btnDeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        DeleteGridRow()
    End Sub

#Region "Private Sub AddGridRow()"
    Private Sub AddGridRow()

        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdCommFormula.Rows.Count + 1
        Dim FLineNo(count) As Integer
        Dim Term1(count) As String
        Dim Term1Code(count) As String
        Dim Operator1(count) As String
        Dim Term2(count) As String
        Dim Term2Code(count) As String
        Dim ResultTerm(count) As String
        Dim ResultTermCode(count) As String

        Dim n As Integer = 0
        Dim lblFLineNo As Label
        Dim txtTerm1 As TextBox
        Dim txtTerm1Code As TextBox
        Dim ddlOperator1 As DropDownList
        Dim txtTerm2 As TextBox
        Dim txtTerm2Code As TextBox
        Dim txtResultTerm As TextBox
        Dim txtResultTermCode As TextBox

        Try
            For Each GVRow In grdCommFormula.Rows
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                FLineNo(n) = CType(lblFLineNo.Text, Integer)
                txtTerm1 = GVRow.FindControl("txtTerm1")
                Term1(n) = CType(txtTerm1.Text, String)
                txtTerm1Code = GVRow.FindControl("txtTerm1Code")
                Term1Code(n) = CType(txtTerm1Code.Text, String)
                ddlOperator1 = GVRow.FindControl("ddlOperator1")
                Operator1(n) = CType(ddlOperator1.SelectedIndex, String)
                txtTerm2 = GVRow.FindControl("txtTerm2")
                Term2(n) = CType(txtTerm2.Text, String)
                txtTerm2Code = GVRow.FindControl("txtTerm2Code")
                Term2Code(n) = CType(txtTerm2Code.Text, String)
                txtResultTerm = GVRow.FindControl("txtResultTerm")
                ResultTerm(n) = CType(txtResultTerm.Text, String)
                txtResultTermCode = GVRow.FindControl("txtResultTermCode")
                ResultTermCode(n) = CType(txtResultTermCode.Text, String)
                n = n + 1
            Next
            fillDategrd(grdCommFormula, False, grdCommFormula.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdCommFormula.Rows
                If n = i Then
                    Exit For
                End If
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                lblFLineNo.Text = FLineNo(n)
                txtTerm1 = GVRow.FindControl("txtTerm1")
                txtTerm1.Text = Term1(n)
                txtTerm1Code = GVRow.FindControl("txtTerm1Code")
                txtTerm1Code.Text = Term1Code(n)
                ddlOperator1 = GVRow.FindControl("ddlOperator1")
                ddlOperator1.SelectedIndex = Operator1(n)
                txtTerm2 = GVRow.FindControl("txtTerm2")
                txtTerm2.Text = Term2(n)
                txtTerm2Code = GVRow.FindControl("txtTerm2Code")
                txtTerm2Code.Text = Term2Code(n)
                txtResultTerm = GVRow.FindControl("txtResultTerm")
                txtResultTerm.Text = ResultTerm(n)
                txtResultTermCode = GVRow.FindControl("txtResultTermCode")
                txtResultTermCode.Text = ResultTermCode(n)
                n = n + 1
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "Private Sub DeleteGridRow()"
    Private Sub DeleteGridRow()
        Dim count As Integer
        Dim GVRow As GridViewRow

        count = grdCommFormula.Rows.Count + 1
        Dim FLineNo(count) As Integer
        Dim Term1(count) As String
        Dim Term1Code(count) As String
        Dim Operator1(count) As String
        Dim Term2(count) As String
        Dim Term2Code(count) As String
        Dim ResultTerm(count) As String
        Dim ResultTermCode(count) As String
        Dim n As Integer = 0

        Dim lblFLineNo As Label
        Dim txtTerm1 As TextBox
        Dim txtTerm1Code As TextBox
        Dim ddlOperator1 As DropDownList
        Dim txtTerm2 As TextBox
        Dim txtTerm2Code As TextBox
        Dim txtResultTerm As TextBox
        Dim txtResultTermCode As TextBox
        Dim chckDeletion As CheckBox

        Try
            For Each GVRow In grdCommFormula.Rows
                chckDeletion = GVRow.FindControl("chckDeletion")
                If chckDeletion.Checked = False Then
                    lblFLineNo = GVRow.FindControl("lblFLineNo")
                    FLineNo(n) = CType(lblFLineNo.Text, Integer)
                    txtTerm1 = GVRow.FindControl("txtTerm1")
                    Term1(n) = CType(txtTerm1.Text, String)
                    txtTerm1Code = GVRow.FindControl("txtTerm1Code")
                    Term1Code(n) = CType(txtTerm1Code.Text, String)
                    ddlOperator1 = GVRow.FindControl("ddlOperator1")
                    Operator1(n) = CType(ddlOperator1.SelectedIndex, String)
                    txtTerm2 = GVRow.FindControl("txtTerm2")
                    Term2(n) = CType(txtTerm2.Text, String)
                    txtTerm2Code = GVRow.FindControl("txtTerm2Code")
                    Term2Code(n) = CType(txtTerm2Code.Text, String)
                    txtResultTerm = GVRow.FindControl("txtResultTerm")
                    ResultTerm(n) = CType(txtResultTerm.Text, String)
                    txtResultTermCode = GVRow.FindControl("txtResultTermCode")
                    ResultTermCode(n) = CType(txtResultTermCode.Text, String)
                    n = n + 1
                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(grdCommFormula, False, count)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdCommFormula.Rows
                If n = i Then
                    Exit For
                End If
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                lblFLineNo.Text = n + 1
                txtTerm1 = GVRow.FindControl("txtTerm1")
                txtTerm1.Text = Term1(n)
                txtTerm1Code = GVRow.FindControl("txtTerm1Code")
                txtTerm1Code.Text = Term1Code(n)
                ddlOperator1 = GVRow.FindControl("ddlOperator1")
                ddlOperator1.SelectedIndex = Operator1(n)
                txtTerm2 = GVRow.FindControl("txtTerm2")
                txtTerm2.Text = Term2(n)
                txtTerm2Code = GVRow.FindControl("txtTerm2Code")
                txtTerm2Code.Text = Term2Code(n)
                txtResultTerm = GVRow.FindControl("txtResultTerm")
                txtResultTerm.Text = ResultTerm(n)
                txtResultTermCode = GVRow.FindControl("txtResultTermCode")
                txtResultTermCode.Text = ResultTermCode(n)
                n = n + 1
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Commission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetTermlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim TermName As New List(Of String)
        Try
            strSqlQry = "select TermName,TermCode from commissionterms where TermName like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    TermName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("TermName").ToString(), myDS.Tables(0).Rows(i)("TermCode").ToString()))
                Next
            End If
            Return TermName
        Catch ex As Exception
            Return TermName
        End Try

    End Function

    Protected Sub btnSaveTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTerm.Click
        Try
            If (ddlCalculateType.SelectedIndex <= 0) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Calculation Type');", True)
                ModalExtraPopup.Show()
                Exit Sub
            End If
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            If Session("CommissionTermMode") = "New" Then
                If checkForDuplicateTerm() = False Then
                    btnClearTerm_Click(sender, e)
                    Exit Sub
                End If
                mySqlCmd = New SqlCommand("sp_add_CommissionTerm", mySqlConn)
            ElseIf Session("CommissionTermMode") = "Edit" Then
                mySqlCmd = New SqlCommand("sp_mod_CommissionTerm", mySqlConn)
            End If
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@TermCode", SqlDbType.VarChar, 20)).Value = CType(txtNewTermCode.Value.Trim.ToUpper, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@TermName", SqlDbType.VarChar, 200)).Value = CType(txtNewTermName.Value.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@CalculateType", SqlDbType.VarChar, 10)).Value = CType(ddlCalculateType.SelectedValue, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@Active", SqlDbType.Int)).Value = IIf(chkTermActive.Checked = True, 1, 0)
            mySqlCmd.Parameters.Add(New SqlParameter("@UserLogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@paxcalc", SqlDbType.Int)).Value = IIf(chkcalcpax.Checked = True, 1, 0)
            mySqlCmd.ExecuteNonQuery()
            FillTerms()
            btnClearTerm_Click(sender, e)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Commission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Public Function checkForDuplicateTerm() As Boolean
        If objUtils.isDuplicatenew(Session("dbconnectionName"), "commissionTerms", "TermCode", CType(txtNewTermCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Commission Term code is already present.');", True)
            checkForDuplicateTerm = False
            Exit Function
        End If
        If objUtils.isDuplicatenew(Session("dbconnectionName"), "commissionTerms", "TermName", txtNewTermName.Value.Trim) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Commission Term name is already present.');", True)
            checkForDuplicateTerm = False
            Exit Function
        End If
        checkForDuplicateTerm = True
    End Function
    Protected Sub ChangePreference(ByVal sender As Object, ByVal e As EventArgs)
        Dim commandArgument As String = TryCast(sender, ImageButton).CommandArgument
        Dim rowIndex As Integer = TryCast(TryCast(sender, ImageButton).NamingContainer, GridViewRow).RowIndex
        Dim lbl As Label = CType(grdCommFormula.Rows(rowIndex).FindControl("lblFLineNo"), Label)
        Dim index As Integer = CType(lbl.Text, Integer)
        If commandArgument = "up" Then
            If (index = 1) Then
                Return
            Else
                MoveRow("up", index)
            End If
        ElseIf (commandArgument = "down") Then
            If (index = grdCommFormula.Rows.Count) Then
                Return
            Else
                MoveRow("down", index)
            End If
        End If

    End Sub

    Private Sub MoveRow(ByVal CommandArg As String, ByVal Index As String)

        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdCommFormula.Rows.Count
        Dim FLineNo(count) As Integer
        Dim Term1(count) As String
        Dim Term1Code(count) As String
        Dim Operator1(count) As String
        Dim Term2(count) As String
        Dim Term2Code(count) As String
        Dim ResultTerm(count) As String
        Dim ResultTermCode(count) As String

        Dim n As Integer = 0
        Dim lblFLineNo As Label
        Dim txtTerm1 As TextBox
        Dim txtTerm1Code As TextBox
        Dim ddlOperator1 As DropDownList
        Dim txtTerm2 As TextBox
        Dim txtTerm2Code As TextBox
        Dim txtResultTerm As TextBox
        Dim txtResultTermCode As TextBox
        Dim m As Integer
        Dim s As Integer
        Try
            For Each GVRow In grdCommFormula.Rows
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                FLineNo(n) = CType(lblFLineNo.Text, Integer)
                txtTerm1 = GVRow.FindControl("txtTerm1")
                Term1(n) = CType(txtTerm1.Text, String)
                txtTerm1Code = GVRow.FindControl("txtTerm1Code")
                Term1Code(n) = CType(txtTerm1Code.Text, String)
                ddlOperator1 = GVRow.FindControl("ddlOperator1")
                Operator1(n) = CType(ddlOperator1.SelectedIndex, String)
                txtTerm2 = GVRow.FindControl("txtTerm2")
                Term2(n) = CType(txtTerm2.Text, String)
                txtTerm2Code = GVRow.FindControl("txtTerm2Code")
                Term2Code(n) = CType(txtTerm2Code.Text, String)
                txtResultTerm = GVRow.FindControl("txtResultTerm")
                ResultTerm(n) = CType(txtResultTerm.Text, String)
                txtResultTermCode = GVRow.FindControl("txtResultTermCode")
                ResultTermCode(n) = CType(txtResultTermCode.Text, String)
                n = n + 1
            Next
            fillDategrd(grdCommFormula, False, grdCommFormula.Rows.Count)
            Dim i As Integer = n
            n = 0
            Dim tmpStr As String
            If (CommandArg = "up") Then
                m = Index - 2
                s = Index - 1
            Else
                m = Index - 1
                s = Index
            End If
            tmpStr = Term1(m)
            Term1(m) = Term1(s)
            Term1(s) = tmpStr
            tmpStr = Term1Code(m)
            Term1Code(m) = Term1Code(s)
            Term1Code(s) = tmpStr
            tmpStr = Operator1(m)
            Operator1(m) = Operator1(s)
            Operator1(s) = tmpStr
            tmpStr = Term2(m)
            Term2(m) = Term2(s)
            Term2(s) = tmpStr
            tmpStr = Term2Code(m)
            Term2Code(m) = Term2Code(s)
            Term2Code(s) = tmpStr
            tmpStr = ResultTerm(m)
            ResultTerm(m) = ResultTerm(s)
            ResultTerm(s) = tmpStr
            tmpStr = ResultTermCode(m)
            ResultTermCode(m) = ResultTermCode(s)
            ResultTermCode(s) = tmpStr

            For Each GVRow In grdCommFormula.Rows
                If n = i Then
                    Exit For
                End If
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                lblFLineNo.Text = FLineNo(n)
                txtTerm1 = GVRow.FindControl("txtTerm1")
                txtTerm1.Text = Term1(n)
                txtTerm1Code = GVRow.FindControl("txtTerm1Code")
                txtTerm1Code.Text = Term1Code(n)
                ddlOperator1 = GVRow.FindControl("ddlOperator1")
                ddlOperator1.SelectedIndex = Operator1(n)
                txtTerm2 = GVRow.FindControl("txtTerm2")
                txtTerm2.Text = Term2(n)
                txtTerm2Code = GVRow.FindControl("txtTerm2Code")
                txtTerm2Code.Text = Term2Code(n)
                txtResultTerm = GVRow.FindControl("txtResultTerm")
                txtResultTerm.Text = ResultTerm(n)
                txtResultTermCode = GVRow.FindControl("txtResultTermCode")
                txtResultTermCode.Text = ResultTermCode(n)
                n = n + 1
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("CommissionState") = "New" Or ViewState("CommissionState") = "Edit" Then
                    If ViewState("CommissionState") = "New" Or ViewState("CommissionState") = "Edit" Then
                        If checkForDuplicate() = False Then
                            Exit Sub
                        End If
                        If ValidateFormulaTerm() = False Then
                            Exit Sub
                        End If
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction
                    If ViewState("CommissionState") = "New" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("COMFORM", mySqlConn, sqlTrans)
                        txtcode.Value = optionval.Trim
                    End If
                    If ViewState("CommissionState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_CommFormulaHeader", mySqlConn, sqlTrans)
                    ElseIf ViewState("CommissionState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_CommFormulaHeader", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaID", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaName", SqlDbType.VarChar, 1000)).Value = CType(txtname.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaType", SqlDbType.VarChar, 100)).Value = "Flat"
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 1000)).Value = CType(txtremarks.Text.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    '----------------------------------- Deleting Data From Commission formula detail Table ---------------------------------------
                    If ViewState("CommissionState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_Del_CommFormulaDetail", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@FormulaID", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()
                    End If
                    '----------------------------------- Inserting Data To Commission formula detail Table -------------------------------
                    Dim lblFLineNo As Label
                    Dim txtTerm1Code As TextBox
                    Dim ddlOperator1 As DropDownList
                    Dim txtTerm2Code As TextBox
                    Dim txtResultTermCode As TextBox

                    For Each gvRow In grdCommFormula.Rows
                        lblFLineNo = gvRow.FindControl("lblFLineNo")
                        txtTerm1Code = gvRow.FindControl("txtTerm1Code")
                        ddlOperator1 = gvRow.FindControl("ddlOperator1")
                        txtTerm2Code = gvRow.FindControl("txtTerm2Code")
                        txtResultTermCode = gvRow.FindControl("txtResultTermCode")
                        If txtTerm1Code.Text <> "" And ddlOperator1.SelectedIndex <> 0 And txtTerm2Code.Text <> "" And txtResultTermCode.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_CommFormulaDetail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@FormulaID", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@FLineNo", SqlDbType.Int, 9)).Value = CType(lblFLineNo.Text, Long)
                            mySqlCmd.Parameters.Add(New SqlParameter("@Term1", SqlDbType.VarChar, 20)).Value = CType(txtTerm1Code.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@Operator1", SqlDbType.VarChar, 20)).Value = CType(ddlOperator1.SelectedItem.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@Term2", SqlDbType.VarChar, 20)).Value = CType(txtTerm2Code.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@ResultTerm", SqlDbType.VarChar, 20)).Value = CType(txtResultTermCode.Text.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    '-----------------------------------------------------------
                ElseIf ViewState("CommissionState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_CommissionFormula", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaID", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CommissionWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Commission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If (ViewState("CommissionState") = "New") Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "commissionformula_header", "formulaName", txtname.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This commission formula name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf (ViewState("CommissionState") = "Edit") Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "commissionformula_header", "formulaid", "formulaName", txtname.Value.Trim, CType(txtcode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This commission formula name is already present.');", True)
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
            Dim lblFLineNo As Label
            Dim txtTerm1 As TextBox
            Dim txtTerm1Code As TextBox
            Dim ddlOperator1 As DropDownList
            Dim txtTerm2 As TextBox
            Dim txtTerm2Code As TextBox
            Dim txtResultTerm As TextBox
            Dim txtResultTermCode As TextBox
            If grdCommFormula.Rows.Count > 1 Then
                For Each gvRow In grdCommFormula.Rows
                    lblFLineNo = gvRow.FindControl("lblFLineNo")
                    txtTerm1 = gvRow.FindControl("txtTerm1")
                    txtTerm1Code = gvRow.FindControl("txtTerm1Code")
                    ddlOperator1 = gvRow.FindControl("ddlOperator1")
                    txtTerm2 = gvRow.FindControl("txtTerm2")
                    txtTerm2Code = gvRow.FindControl("txtTerm2Code")
                    txtResultTerm = gvRow.FindControl("txtResultTerm")
                    txtResultTermCode = gvRow.FindControl("txtResultTermCode")
                    
                    If txtTerm1Code.Text = "" And txtTerm1.Text = "" And ddlOperator1.SelectedIndex = 0 And txtTerm2Code.Text = "" And txtTerm2.Text = "" And txtResultTermCode.Text = "" And txtResultTerm.Text = "" Then
                        CType(gvRow.FindControl("chckDeletion"), CheckBox).Checked = True
                    End If
                Next
                DeleteGridRow()
            End If
            Dim count = grdCommFormula.Rows.Count
            Dim n As Integer = 1
            For Each gvRow In grdCommFormula.Rows
                lblFLineNo = gvRow.FindControl("lblFLineNo")
                txtTerm1 = gvRow.FindControl("txtTerm1")
                txtTerm1Code = gvRow.FindControl("txtTerm1Code")
                ddlOperator1 = gvRow.FindControl("ddlOperator1")
                txtTerm2 = gvRow.FindControl("txtTerm2")
                txtTerm2Code = gvRow.FindControl("txtTerm2Code")
                txtResultTerm = gvRow.FindControl("txtResultTerm")
                txtResultTermCode = gvRow.FindControl("txtResultTermCode")
                If txtTerm1Code.Text <> "" And txtTerm1.Text <> "" And ddlOperator1.SelectedIndex <> 0 And txtTerm2Code.Text <> "" And txtTerm2.Text <> "" And txtResultTermCode.Text <> "" And txtResultTerm.Text <> "" Then
                    If txtTerm1Code.Text.Trim = txtTerm2Code.Text Or txtTerm1Code.Text.Trim = txtResultTermCode.Text Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Term1 should not be equal to Term2 or Result Term');", True)
                        SetFocus(txtTerm2)
                        ValidateFormulaTerm = False
                        Exit Function
                    End If
                    If txtTerm2Code.Text.Trim = txtTerm1Code.Text Or txtTerm2Code.Text.Trim = txtResultTermCode.Text Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Term2 should not be equal to Term1 or Result Term');", True)
                        SetFocus(txtTerm1)
                        ValidateFormulaTerm = False
                        Exit Function
                    End If
                    If n <> count Then
                        If txtTerm1Code.Text.Trim = "NPR" Or txtTerm2Code.Text = "NPR" Or txtResultTermCode.Text = "NPR" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('NPR should be in last row Result Term column');", True)
                            SetFocus(txtTerm1)
                            ValidateFormulaTerm = False
                            Exit Function
                        End If
                    ElseIf txtTerm1Code.Text.Trim = "NPR" Or txtTerm2Code.Text = "NPR" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('NPR should be in Result Term');", True)
                        SetFocus(txtTerm1)
                        ValidateFormulaTerm = False
                        Exit Function
                    ElseIf txtResultTermCode.Text.Trim <> "NPR" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Last Row Result Term should be NPR');", True)
                        SetFocus(txtResultTerm)
                        ValidateFormulaTerm = False
                        Exit Function
                    End If
                ElseIf txtTerm1Code.Text <> "" And (ddlOperator1.SelectedIndex = 0 Or txtTerm2Code.Text = "" Or txtResultTermCode.Text = "") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Commission Formula');", True)
                    SetFocus(txtTerm1)
                    ValidateFormulaTerm = False
                    Exit Function
                ElseIf txtTerm2Code.Text <> "" And (ddlOperator1.SelectedIndex = 0 Or txtTerm1Code.Text = "" Or txtResultTermCode.Text = "") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Commission Formula');", True)
                    SetFocus(txtTerm2)
                    ValidateFormulaTerm = False
                    Exit Function
                ElseIf txtResultTermCode.Text <> "" And (ddlOperator1.SelectedIndex = 0 Or txtTerm1Code.Text = "" Or txtTerm2Code.Text = "") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Commission Formula');", True)
                    SetFocus(txtResultTerm)
                    ValidateFormulaTerm = False
                    Exit Function
                ElseIf ddlOperator1.SelectedIndex <> 0 And (txtResultTermCode.Text = "" Or txtTerm1Code.Text = "" Or txtTerm2Code.Text = "") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check Commission Formula');", True)
                    SetFocus(ddlOperator1)
                    ValidateFormulaTerm = False
                    Exit Function
                End If
                n = n + 1
            Next
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
        If ViewState("CommissionState") = "View" Or ViewState("CommissionState") = "Delete" Then
            txtcode.Disabled = True
            txtname.Disabled = True
            txtremarks.Enabled = False
            chkActive.Disabled = True
            btnAddRow.Enabled = False
            btnDeleteRow.Enabled = False
            btnAddTerm.Enabled = False
            grdCommFormula.Enabled = False
        ElseIf ViewState("CommissionState") = "Edit" Then
            txtcode.Disabled = True
        End If
    End Sub
#End Region
#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("Select * from commissionformula_header Where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("FormulaID")) = False Then
                        Me.txtcode.Value = CType(mySqlReader("FormulaID"), String)

                    Else
                        Me.txtcode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("FormulaName")) = False Then
                        Me.txtname.Value = CType(mySqlReader("FormulaName"), String)
                    Else
                        Me.txtname.Value = ""
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        Me.txtremarks.Text = CType(mySqlReader("remarks"), String)
                    Else
                        Me.txtremarks.Text = ""
                    End If

                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Commission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "Commissionformula_detail", "count(FormulaID)", "FormulaID", RefCode)
            If lngCnt = 0 Then lngCnt = 1
            fillDategrd(grdCommFormula, False, lngCnt)
            Dim TermList As New List(Of ListItem)
            Dim mySqlCmd2 As SqlCommand
            Dim mySqlReader2 As SqlDataReader
            Dim n As Integer = 1
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd2 = New SqlCommand("select TermCode,TermName from commissionTerms", mySqlConn)
            mySqlReader2 = mySqlCmd2.ExecuteReader()
            If mySqlReader2.HasRows Then
                While mySqlReader2.Read()
                    TermList.Add(New ListItem(mySqlReader2("TermName"), mySqlReader2("TermCode")))
                End While
            End If
            mySqlReader2.Close()
            mySqlCmd = New SqlCommand("select * from commissionformula_detail where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    Dim lblFLineNo As Label
                    Dim txtTerm1 As TextBox
                    Dim txtTerm1Code As TextBox
                    Dim ddlOperator1 As DropDownList
                    Dim txtTerm2 As TextBox
                    Dim txtTerm2Code As TextBox
                    Dim txtResultTerm As TextBox
                    Dim txtResultTermCode As TextBox
                    If n <= grdCommFormula.Rows.Count Then
                        Dim gvRow As GridViewRow = grdCommFormula.Rows(n - 1)
                        lblFLineNo = gvRow.FindControl("lblFLineNo")
                        txtTerm1 = gvRow.FindControl("txtTerm1")
                        txtTerm1Code = gvRow.FindControl("txtTerm1Code")
                        ddlOperator1 = gvRow.FindControl("ddlOperator1")
                        txtTerm2 = gvRow.FindControl("txtTerm2")
                        txtTerm2Code = gvRow.FindControl("txtTerm2Code")
                        txtResultTerm = gvRow.FindControl("txtResultTerm")
                        txtResultTermCode = gvRow.FindControl("txtResultTermCode")
                        If IsDBNull(mySqlReader("FLineNo")) = False Then
                            lblFLineNo.Text = CType(mySqlReader("FLineNo"), Integer)
                        End If
                        If IsDBNull(mySqlReader("Term1")) = False Then
                            txtTerm1Code.Text = CType(mySqlReader("Term1"), String)
                        End If
                        If IsDBNull(mySqlReader("Term1")) = False Then
                            txtTerm1.Text = FindTerm(TermList, txtTerm1Code.Text.Trim)
                        End If
                        If IsDBNull(mySqlReader("Operator1")) = False Then
                            ddlOperator1.Text = CType(mySqlReader("Operator1"), String)
                        End If
                        If IsDBNull(mySqlReader("Term2")) = False Then
                            txtTerm2Code.Text = CType(mySqlReader("Term2"), String)
                        End If
                        If IsDBNull(mySqlReader("Term2")) = False Then
                            txtTerm2.Text = FindTerm(TermList, txtTerm2Code.Text.Trim)
                        End If
                        If IsDBNull(mySqlReader("ResultTerm")) = False Then
                            txtResultTermCode.Text = CType(mySqlReader("ResultTerm"), String)
                        End If
                        If IsDBNull(mySqlReader("ResultTerm")) = False Then
                            txtResultTerm.Text = FindTerm(TermList, txtResultTermCode.Text.Trim)
                        End If
                        n = n + 1
                    End If
                End While
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Commission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region
    Private Function FindTerm(ByVal TermList As List(Of ListItem), ByVal str As String) As String
        For Each tl In TermList
            If (tl.Value = str) Then
                Return tl.Text
            End If
        Next
        Return ""
    End Function

    Protected Sub btnClearTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearTerm.Click
        txtNewTermCode.Value = ""
        txtNewTermCode.Attributes.Remove("disabled")
        txtNewTermName.Value = ""
        ddlCalculateType.SelectedIndex = 0
        chkTermActive.Disabled = False

        chkTermActive.Checked = True
        lblTermTitle.Text = "New Commission Term"
    End Sub

    Protected Sub grdCommTerms_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdCommTerms.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSysVal As Label = TryCast(e.Row.FindControl("lblSystemValue"), Label)
            Dim linkBtn As LinkButton = TryCast(e.Row.FindControl("lbtnEdit"), LinkButton)
            If CType(lblSysVal.Text, Integer) = 1 Then
                linkBtn.Visible = False
            End If
        End If
    End Sub
    Protected Sub CommissionTermEdit(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim SqlConn As New SqlConnection
        Try
            Dim ctRow As GridViewRow = TryCast(TryCast(sender, LinkButton).NamingContainer, GridViewRow)
            txtNewTermCode.Value = CType(ctRow.FindControl("lblTermCode"), Label).Text
            strSqlQry = "select TermCode,TermName,perc_value,active,isnull(paxcalc,0) paxcalc from commissionterms where TermCode = '" + txtNewTermCode.Value.Trim + "'"
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand(strSqlQry, SqlConn)
            Dim myDataReader As SqlDataReader
            myDataReader = mySqlCmd.ExecuteReader()
            Dim tmpChkActive As Boolean = False
            If (myDataReader.HasRows) Then
                If (myDataReader.Read()) Then
                    If Not IsDBNull(myDataReader("TermName")) Then
                        txtNewTermName.Value = myDataReader("TermName")
                    Else
                        txtNewTermName.Value = ""
                    End If
                    If Not IsDBNull(myDataReader("perc_value")) Then
                        If myDataReader("perc_value") = "P" Then
                            ddlCalculateType.SelectedIndex = 1
                        ElseIf myDataReader("perc_value") = "V" Then
                            ddlCalculateType.SelectedIndex = 2
                        End If
                    Else
                        ddlCalculateType.SelectedIndex = 0
                    End If
                    If CType(myDataReader("active"), Integer) = 1 Then
                        chkTermActive.Checked = True
                        tmpChkActive = True
                    Else
                        chkTermActive.Checked = False
                    End If
                    If CType(myDataReader("paxcalc"), Integer) = 1 Then
                        chkcalcpax.Checked = True

                    Else
                        chkcalcpax.Checked = False
                    End If
                    Session("CommissionTermMode") = "Edit"
                    txtNewTermCode.Attributes("disabled") = True
                    lblTermTitle.Text = "Edit Commission Term"
                    ModalExtraPopup.Show()
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid Term Code' );", True)
            End If
            myDataReader.Close()
            If (tmpChkActive = True) Then
                strSqlQry = "select term from (select term1 as term from commissionformula_detail union select term2 as term from commissionformula_detail union select resultterm as term from commissionformula_detail) as t where term ='" + txtNewTermCode.Value.Trim + "'"
                mySqlCmd = New SqlCommand(strSqlQry, SqlConn)
                Dim myDataReader1 As SqlDataReader
                myDataReader1 = mySqlCmd.ExecuteReader()
                If (myDataReader1.HasRows) Then
                    chkTermActive.Disabled = True
                End If
                myDataReader1.Close()
            End If
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            If (mySqlConn.State = ConnectionState.Open) Then
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Commission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    
End Class
