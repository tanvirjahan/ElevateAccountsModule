Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class MealMarkupFormula
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
    Dim OperatorACINew As New ArrayList
    Dim rValueNew As New ArrayList
    Dim ValueNew As New ArrayList
    Dim ACINew As New ArrayList
    Dim AdultNew As New ArrayList
    Dim ChildNew As New ArrayList
    Dim ExtraBedNew As New ArrayList
    Dim Rmcatnew As New ArrayList
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
                ViewState.Add("MarkupState", Request.QueryString("State"))
                ViewState.Add("MarkupFormulaID", Request.QueryString("FormulaID"))


                fillDategrd(grdCommFormula, False, 1)
                If ViewState("MarkupState") = "New" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Meal Markup Formula"
                    Page.Title = Page.Title + " " + "New MealMarkup Formula"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("MarkupState") = "Edit" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Edit Meal Markup Formula"
                    Page.Title = Page.Title + " " + "Edit MealMarkup Formula"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    DisableControl()
              
                ElseIf ViewState("MarkupState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Meal Markup Formula"
                    Page.Title = Page.Title + " " + "View MealMarkup Formula"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    DisableControl()
                ElseIf ViewState("MarkupState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Meal Markup Formula"
                    Page.Title = Page.Title + " " + "Delete MealMarkup Formula"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("MarkupState") = "Copy" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Meal Markup Formula"
                    Page.Title = Page.Title + " " + "New Meal Markup Formula"
                    btnSave.Text = "Save"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                End If
                If ddlFormulaType.Text = "Hotel" Then
                    grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
                Else
                    grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MealMarkupFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Page.Title = "MealMarkup Formula Entry"
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


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MarkupEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub btnAddRow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddGridRow()
    End Sub

    Protected Sub btnDeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        DeleteGridRow()
    End Sub
    Protected Sub btnrmcat_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(grdCommFormula.Rows(rowid).FindControl("txtrmcatcode"), TextBox).Text


        Dim strmeal As String = CType(strmealname, String)


        Dim MyDs As New DataTable
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

      
        strSqlQry = "select rmcatcode as rmtypcode,rmcatname rmtypname from  rmcatmast(nolock) where isnull(allotreqd,'')='No' and accom_extra in ('M','L')  order by isnull(rankorder,999)"

        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If
        gv_Showroomtypes.HeaderRow.Cells(3).Text = "Room Category"

        ''ChkExistingMeal(strmeal)
        ChkExistingRoomtypes(strmeal)


        ModalExtraPopup.Show()
    End Sub
    Private Sub ChkExistingRoomtypes(ByVal prm_strChkRoomtypes As String)
        Dim chkSelect As CheckBox
        Dim txtrmtypcode As Label


        Dim arrString As String()


        If prm_strChkRoomtypes <> "" Then
            arrString = prm_strChkRoomtypes.Split(",") 'spliting for ';' 1st

            For k = 0 To arrString.Length - 1
                'If arrString(k) <> "" Then

                '  Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                For Each grdRow In gv_Showroomtypes.Rows
                    chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                    txtrmtypcode = CType(grdRow.FindControl("txtrmtypcode"), Label)
                    If arrString(k) = txtrmtypcode.Text Then
                        chkSelect.Checked = True

                    End If

                Next
                'End If
            Next
            'Else
            '    'first case when not selected , chk all
            '    For Each grdRow In gvOccupancy.Rows
            '        chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
            '        chkSelect.Checked = True

            '    Next


        End If
    End Sub
    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click
        Try
            ' Dim txtbox As TextBox
            Dim roomtypes As String

            '   roomtypes = getroomtypes()

            Dim chkSelect As CheckBox
            Dim intAdult, intChild As Integer
            Dim strChkdStringVal As StringBuilder = New StringBuilder()
            Dim tickedornot As Boolean

            tickedornot = False
            For Each grdRow In gv_Showroomtypes.Rows
                chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                chkSelect = grdRow.FindControl("chkrmtype")
                If chkSelect.Checked = True Then
                    tickedornot = True
                    Exit For
                End If
            Next

            'If tickedornot = False Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Row');", True)
            '    ModalExtraPopup.Show()
            '    Exit Sub
            'End If



            Dim chk2 As CheckBox
            Dim txtrmtypcode1 As Label

            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtrmtypcode1 = gvRow.FindControl("txtrmtypcode")

                If chk2.Checked = True Then
                    roomtypes = roomtypes + txtrmtypcode1.Text + ","

                End If
            Next

            If roomtypes Is Nothing Then
                roomtypes = "All"

            Else
                roomtypes = roomtypes.Substring(0, roomtypes.Length - 1)
            End If

            If gv_Showroomtypes.HeaderRow.Cells(3).Text = "Room Category" Then
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtmealcode As TextBox = grdCommFormula.Rows(hdnMainGridRowid.Value).FindControl("txtrmcatcode")

                    txtmealcode.Text = roomtypes.ToString


                End If

            End If




            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnmealok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmealok.Click
        Try
            ' Dim txtbox As TextBox
            Dim mealname As String = ""

            '   roomtypes = getroomtypes()

            Dim chkSelect As CheckBox


            Dim strChkdStringVal As StringBuilder = New StringBuilder()
            Dim tickedornot As Boolean

            tickedornot = False
            For Each grdRow In gv_Showroomtypes.Rows
                chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                chkSelect = grdRow.FindControl("chkrmtype")
                If chkSelect.Checked = True Then
                    tickedornot = True
                    Exit For
                End If
            Next

            If tickedornot = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Meal Plan');", True)
                ModalExtraPopup.Show()
                Exit Sub
            End If



            Dim chk2 As CheckBox
            Dim txtmealcode1 As Label

            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtmealcode1 = gvRow.FindControl("txtrmtypcode")

                If chk2.Checked = True Then
                    mealname = mealname + txtmealcode1.Text + ","

                End If
            Next

            If mealname.Length > 0 Then
                mealname = mealname.Substring(0, mealname.Length - 1)
            End If

            If hdnMainGridRowid.Value <> "" Then
                Dim txtmealcode As TextBox = grdCommFormula.Rows(hdnMainGridRowid.Value).FindControl("txtrmcatcode")
                txtmealcode.Text = mealname.ToString
            End If
            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Private Sub AddGridRow()"
    Private Sub AddGridRow()

        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdCommFormula.Rows.Count + 1
        Dim FLineNo(count) As Integer
        Dim cFrom(count) As String
        Dim cTo(count) As String
        Dim Operator1(count) As String
        Dim rValue(count) As String
        Dim Value(count) As String
        Dim ACI(count) As String
        Dim Adult(count) As String
        Dim Child(count) As String
        Dim Infant(count) As String
        Dim OperatorACI(count) As String
        Dim rmcatcode(count) As String

        Dim n As Integer = 0
        Dim lblFLineNo As Label
        Dim txtFrom As TextBox
        Dim txtTo As TextBox
        Dim ddlOperator1 As DropDownList
        Dim rdoValue As RadioButton
        Dim rdoACI As RadioButton
        Dim txtValue As TextBox
        Dim txtAdult As TextBox
        Dim txtChild As TextBox
        Dim txtExtraBed As TextBox
        Dim txtrmcatcode As TextBox
        Dim ddlOperatorACI As DropDownList
        Try
            For Each GVRow In grdCommFormula.Rows
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                FLineNo(n) = CType(lblFLineNo.Text, Integer)
                txtFrom = GVRow.FindControl("txtFrom")
                cFrom(n) = CType(txtFrom.Text, String)
                txtTo = GVRow.FindControl("txtTo")
                cTo(n) = CType(txtTo.Text, String)
                ddlOperator1 = GVRow.FindControl("ddlOperator")
                Operator1(n) = CType(ddlOperator1.SelectedIndex, String)
                txtValue = GVRow.FindControl("txtValue")
                Value(n) = CType(txtValue.Text, String)
                txtAdult = GVRow.FindControl("txtAdults")
                Adult(n) = CType(txtAdult.Text, String)
                txtChild = GVRow.FindControl("txtChild")
                Child(n) = CType(txtChild.Text, String)
                txtExtraBed = GVRow.FindControl("txtExtraBed")
                Infant(n) = CType(txtExtraBed.Text, String)


                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                rmcatcode(n) = CType(txtrmcatcode.Text, String)
                ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                OperatorACI(n) = CType(ddlOperatorACI.SelectedIndex, String)
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
                txtFrom = GVRow.FindControl("txtFrom")
                txtFrom.Text = cFrom(n)
                txtTo = GVRow.FindControl("txtTo")
                txtTo.Text = cTo(n)
                If n = i - 1 Then
                    Dim txtFromLast As TextBox = grdCommFormula.Rows(grdCommFormula.Rows.Count - 1).FindControl("txtFrom")
                    If cTo(n) <> "" Then
                        txtFromLast.Text = cTo(n) + 1
                    End If
                End If
                ddlOperator1 = GVRow.FindControl("ddlOperator")
                ddlOperator1.SelectedIndex = Operator1(n)
                txtValue = GVRow.FindControl("txtValue")
                txtValue.Text = Value(n)
                txtAdult = GVRow.FindControl("txtAdults")
                txtAdult.Text = Adult(n)
                txtChild = GVRow.FindControl("txtChild")
                txtChild.Text = Child(n)
                txtExtraBed = GVRow.FindControl("txtExtraBed")
                txtExtraBed.Text = Infant(n)

                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtrmcatcode.Text = rmcatcode(n)
                ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                ddlOperatorACI.SelectedIndex = OperatorACI(n)

                n = n + 1
            Next
            Dim txtToLast As TextBox = grdCommFormula.Rows(grdCommFormula.Rows.Count - 1).FindControl("txtTo")
            txtToLast.Focus()
            Dim txtFromLast1 As TextBox = grdCommFormula.Rows(grdCommFormula.Rows.Count - 1).FindControl("txtFrom")
            If txtFromLast1.Text = "" Then
                txtFromLast1.Focus()
            End If

            Dim gridNewrow As GridViewRow
            gridNewrow = grdCommFormula.Rows(grdCommFormula.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdCommFormula.Rows.Count - 1, String) + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

            If ddlFormulaType.Text = "Hotel" Then
                grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
            Else
                grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
            End If
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
        Dim cFrom(count) As String
        Dim cTo(count) As String
        Dim Operator1(count) As String
        Dim Value(count) As String
        Dim Adult(count) As String
        Dim Child(count) As String
        Dim Infant(count) As String
        Dim OperatorACI(count) As String
        Dim rmcatcode(count) As String


        Dim n As Integer = 0
        Dim lblFLineNo As Label
        Dim txtFrom As TextBox
        Dim txtTo As TextBox
        Dim ddlOperator1 As DropDownList
        Dim txtValue As TextBox
        Dim txtAdult As TextBox
        Dim txtChild As TextBox
        Dim txtExtraBed As TextBox
        Dim txtrmcatcode As TextBox
        Dim chckDeletion As CheckBox
        Dim ddlOperatorACI As DropDownList

        Try
            For Each GVRow In grdCommFormula.Rows
                chckDeletion = GVRow.FindControl("chckDeletion")
                If chckDeletion.Checked = False Then
                    lblFLineNo = GVRow.FindControl("lblFLineNo")
                    FLineNo(n) = CType(lblFLineNo.Text, Integer)
                    txtFrom = GVRow.FindControl("txtFrom")
                    cFrom(n) = CType(txtFrom.Text, String)
                    txtTo = GVRow.FindControl("txtTo")
                    cTo(n) = CType(txtTo.Text, String)
                    ddlOperator1 = GVRow.FindControl("ddlOperator")
                    Operator1(n) = CType(ddlOperator1.SelectedIndex, String)
                    txtValue = GVRow.FindControl("txtValue")
                    Value(n) = CType(txtValue.Text, String)
                    txtAdult = GVRow.FindControl("txtAdults")
                    Adult(n) = CType(txtAdult.Text, String)
                    txtChild = GVRow.FindControl("txtChild")
                    Child(n) = CType(txtChild.Text, String)
                    txtExtraBed = GVRow.FindControl("txtExtraBed")
                    Infant(n) = CType(txtExtraBed.Text, String)

                    txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                    rmcatcode(n) = CType(txtrmcatcode.Text, String)

                    ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                    OperatorACI(n) = CType(ddlOperatorACI.SelectedIndex, String)
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
                lblFLineNo.Text = FLineNo(n)
                txtFrom = GVRow.FindControl("txtFrom")
                txtFrom.Text = cFrom(n)
                txtTo = GVRow.FindControl("txtTo")
                txtTo.Text = cTo(n)
                ddlOperator1 = GVRow.FindControl("ddlOperator")
                ddlOperator1.SelectedIndex = Operator1(n)
                txtValue = GVRow.FindControl("txtValue")
                txtValue.Text = Value(n)
                txtAdult = GVRow.FindControl("txtAdults")
                txtAdult.Text = Adult(n)
                txtChild = GVRow.FindControl("txtChild")
                txtChild.Text = Child(n)
                txtExtraBed = GVRow.FindControl("txtExtraBed")
                txtExtraBed.Text = Infant(n)

                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtrmcatcode.Text = rmcatcode(n)

                ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                ddlOperatorACI.SelectedIndex = OperatorACI(n)
                n = n + 1
            Next
            If ddlFormulaType.Text = "Hotel" Then
                grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
            Else
                grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MealMarkupformula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



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
                '  If ViewState("MarkupState") = "New" Or ViewState("MarkupState") = "Edit" Then
                If ViewState("MarkupState") = "New" Or ViewState("MarkupState") = "Edit" Or ViewState("MarkupState") = "Copy" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    If ValidateFormulaTerm() = False Then
                        Exit Sub
                    End If
                    If ValidateSlabRange() = False Then
                        Exit Sub
                    End If
                End If
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                sqlTrans = mySqlConn.BeginTransaction
                Dim strOpMode As String = ""
                If ViewState("MarkupState") = "New" Or ViewState("MarkupState") = "Copy" Then
                    Dim optionval As String
                    optionval = objUtils.GetAutoDocNo("MMARKFORM", mySqlConn, sqlTrans)
                    txtcode.Value = optionval.Trim
                End If
                mySqlCmd = New SqlCommand("SP_MealMarkupFormula", mySqlConn, sqlTrans)
                If ViewState("MarkupState") = "New" Or ViewState("MarkupState") = "Copy" Then
                    strOpMode = "1"
                ElseIf ViewState("MarkupState") = "Edit" Then
                    strOpMode = "2"
                ElseIf ViewState("MarkupState") = "Delete" Then
                    strOpMode = "3"
                End If


                '----------------------------------- Create XML for insert markup formula -------------------------------

                Dim lblFLineNo As Label
                Dim n As Integer = 0
                Dim txtFrom As TextBox
                Dim txtTo As TextBox
                Dim ddlOperator1 As DropDownList
                Dim ddlOperatorACI As DropDownList
                Dim txtValue As TextBox
                Dim txtAdult As TextBox
                Dim txtChild As TextBox
                Dim txtExtraBed As TextBox
                Dim txtrmcatcode As TextBox



                Dim strBuffer As New Text.StringBuilder
                Dim count = grdCommFormula.Rows.Count
                Dim iRpwCnt As Integer = 0
                strBuffer.Append("<FormulaDetails>")
                For Each GVRow In grdCommFormula.Rows
                    lblFLineNo = GVRow.FindControl("lblFLineNo")
                    txtFrom = GVRow.FindControl("txtFrom")
                    txtTo = GVRow.FindControl("txtTo")
                    ddlOperator1 = GVRow.FindControl("ddlOperator")
                    txtValue = GVRow.FindControl("txtValue")
                    txtAdult = GVRow.FindControl("txtAdults")
                    txtChild = GVRow.FindControl("txtChild")
                    txtExtraBed = GVRow.FindControl("txtExtraBed")
                    ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                    txtrmcatcode = GVRow.FindControl("txtrmcatcode")

                    strBuffer.Append("<FormulaDetail>")
                    strBuffer.Append(" <FLineNo>" & lblFLineNo.Text.Trim & "</FLineNo>")
                    strBuffer.Append(" <From>" & txtFrom.Text.Trim & " </From>")
                    Dim strTo As String = ""
                    If txtTo.Text.Trim = "" Then
                        strTo = "0"
                        strBuffer.Append(" <lastslab>1</lastslab>")
                    Else
                        strTo = txtTo.Text.Trim
                        strBuffer.Append(" <lastslab>0</lastslab>")
                    End If
                    strBuffer.Append(" <To>" & strTo & " </To>")
                    strBuffer.Append(" <Operator>" & ddlOperator1.SelectedItem.Text.Trim & " </Operator>")
                    If txtValue.Text.Trim <> "" Then
                        strBuffer.Append(" <Value>" & txtValue.Text.Trim & "</Value>")
                    Else
                        strBuffer.Append(" <Value>0</Value>")
                    End If
                    If txtAdult.Text.Trim <> "" Then
                        strBuffer.Append(" <Adults>" & txtAdult.Text.Trim & "</Adults>")
                    Else
                        strBuffer.Append(" <Adults>0</Adults>")
                    End If
                    If txtChild.Text.Trim <> "" Then
                        strBuffer.Append(" <Child>" & txtChild.Text.Trim & "</Child>")
                    Else
                        strBuffer.Append(" <Child>0</Child>")
                    End If
                    'If txtChild.Text.Trim <> "" Then
                    '    strBuffer.Append(" <ExtraBed>" & txtExtraBed.Text.Trim & "</ExtraBed>")
                    'Else
                    '    strBuffer.Append(" <ExtraBed>0</ExtraBed>")
                    'End If
                    strBuffer.Append(" <OperatorACI>" & ddlOperatorACI.SelectedItem.Text.Trim & " </OperatorACI>")
                    strBuffer.Append(" <Mealcategory>" & txtrmcatcode.Text.Trim & " </Mealcategory>")
                    strBuffer.Append("</FormulaDetail>")
                Next

                strBuffer.Append("</FormulaDetails>")

                '-----------------------------------------------------------



                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@FormulaId", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@FormulaName", SqlDbType.VarChar, 1000)).Value = CType(txtname.Value.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@FormulaDesc", SqlDbType.VarChar, 1000)).Value = CType(txtDescription.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@CurrCode", SqlDbType.VarChar, 20)).Value = CType(TextCurrencyCode.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@FormulaType", SqlDbType.VarChar, 50)).Value = CType(ddlFormulaType.SelectedValue.Trim, String)
                If chkActive.Checked = True Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@Active", SqlDbType.Int)).Value = 1
                ElseIf chkActive.Checked = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@Active", SqlDbType.Int)).Value = 0
                End If

                mySqlCmd.Parameters.Add(New SqlParameter("@AddUser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@ModUser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@MarkupXMLInput", SqlDbType.Xml)).Value = strBuffer.ToString
                mySqlCmd.Parameters.Add(New SqlParameter("@OpMode", SqlDbType.Int)).Value = strOpMode
                mySqlCmd.ExecuteNonQuery()


                'ElseIf ViewState("MarkupState") = "Delete" Then
                '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                '    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                '    mySqlCmd = New SqlCommand("sp_del_CommissionFormula", mySqlConn, sqlTrans)
                '    mySqlCmd.CommandType = CommandType.StoredProcedure
                '    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaID", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                '    mySqlCmd.ExecuteNonQuery()
                'End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Not Request.QueryString("Page") Is Nothing Then
                    If Request.QueryString("Page") = "applymarkup" Then
                        Dim strscript1 As String = ""
                        strscript1 = "window.opener.__doPostBack('ApplyMarkupWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                    Else
                        Dim strscript As String = ""
                        strscript = "window.opener.__doPostBack('MealMarkupWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    End If
                Else
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('MealMarkupWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MealMarkupFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If (ViewState("MarkupState") = "New") Or ViewState("MarkupState") = "Copy" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "New_MarkupSupplement_Header", "formulaName", txtname.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This markup formula name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf (ViewState("MarkupState") = "Edit") Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "New_MarkupSupplement_Header", "formulaid", "formulaName", txtname.Value.Trim, CType(txtcode.Value.Trim, String)) Then
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
            Dim lblFLineNo As Label
            Dim n As Integer = 0
            Dim txtFrom As TextBox
            Dim txtTo As TextBox
            Dim ddlOperator1 As DropDownList
            Dim txtValue As TextBox
            Dim txtAdult As TextBox
            Dim txtChild As TextBox
            Dim txtExtraBed As TextBox
            Dim txtrmcatcode As TextBox




            Dim count = grdCommFormula.Rows.Count
            Dim iRpwCnt As Integer = 0
            For Each GVRow In grdCommFormula.Rows

                lblFLineNo = GVRow.FindControl("lblFLineNo")
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                txtFrom = GVRow.FindControl("txtFrom")
                txtTo = GVRow.FindControl("txtTo")
                ddlOperator1 = GVRow.FindControl("ddlOperator")
                txtValue = GVRow.FindControl("txtValue")
                txtAdult = GVRow.FindControl("txtAdults")
                txtChild = GVRow.FindControl("txtChild")
                txtExtraBed = GVRow.FindControl("txtExtraBed")
                txtrmcatcode = GVRow.FindControl("txtrmcatcode")

                If txtFrom.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter lower cost');", True)
                    SetFocus(txtFrom)
                    ValidateFormulaTerm = False
                    Exit Function
                End If
                If txtTo.Text = "" And (iRpwCnt < count - 1) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter higher cost');", True)
                    SetFocus(txtTo)
                    ValidateFormulaTerm = False
                    Exit Function
                End If
                If ddlOperator1.SelectedIndex = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select any operator');", True)
                    SetFocus(ddlOperator1)
                    ValidateFormulaTerm = False
                    Exit Function
                End If
                If txtrmcatcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Category');", True)
                    SetFocus(txtrmcatcode)
                    ValidateFormulaTerm = False
                    Exit Function
                End If

                'If txtValue.Text = "" Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter the markup value');", True)
                '    SetFocus(txtValue)
                '    ValidateFormulaTerm = False
                '    Exit Function
                'End If


                'If txtAdult.Text = "" Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter the adult value');", True)
                '    SetFocus(txtValue)
                '    ValidateFormulaTerm = False
                '    Exit Function
                'End If
                'If txtChild.Text = "" Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter the child value');", True)
                '    SetFocus(txtChild)
                '    ValidateFormulaTerm = False
                '    Exit Function
                'End If



                n = n + 1
                iRpwCnt = iRpwCnt + 1
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
        If ViewState("MarkupState") = "View" Or ViewState("MarkupState") = "Delete" Then
            txtcode.Disabled = True
            txtname.Disabled = True
            txtDescription.Enabled = False
            TxtCurrencyName.Enabled = False
            chkActive.Disabled = True
            btnAddRow.Enabled = False
            btnDeleteRow.Enabled = False

            grdCommFormula.Enabled = False
        ElseIf ViewState("MarkupState") = "Edit" Then
            txtcode.Disabled = True
        End If
    End Sub
#End Region
#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("select *,(select currname from currmast c where c.currcode=New_MarkupSupplement_Header.currcode)currname from New_MarkupSupplement_Header Where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("FormulaID")) = False Then
                        If ViewState("MarkupState") <> "Copy" Then
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
                        Me.txtDescription.Text = CType(mySqlReader("formuladesc"), String)
                    Else
                        Me.txtDescription.Text = ""
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.TextCurrencyCode.Text = CType(mySqlReader("currcode"), String)
                    Else
                        Me.TextCurrencyCode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("FormulaType")) = False Then
                        Me.ddlFormulaType.SelectedValue = CType(mySqlReader("FormulaType"), String)
                    End If
                    If IsDBNull(mySqlReader("currname")) = False Then
                        Me.TxtCurrencyName.Text = CType(mySqlReader("currname"), String)
                    Else
                        Me.TxtCurrencyName.Text = ""
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
            objUtils.WritErrorLog("MealMarkupFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region
#Region "Private Sub ShowCommFormula(ByVal RefCode As String)"
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(4, Integer))
        Return Rdamt
    End Function
    Private Sub ShowCommFormula(ByVal RefCode As String)
        Try
            Dim lngCnt As Long
            lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "New_MarkupSupplement_Details", "count(FormulaID)", "FormulaID", RefCode)
            If lngCnt = 0 Then lngCnt = 1
            fillDategrd(grdCommFormula, False, lngCnt)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("select * from New_MarkupSupplement_Details  where FormulaID='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            Dim n As Integer = 1
            If mySqlReader.HasRows Then
                While mySqlReader.Read()

                    '  Dim n As Integer = 0
                    Dim lblFLineNo As Label
                    Dim txtFrom As TextBox
                    Dim txtTo As TextBox
                    Dim ddlOperator1 As DropDownList
                    Dim txtValue As TextBox
                    Dim txtAdult As TextBox
                    Dim txtChild As TextBox
                    Dim txtExtraBed As TextBox
                    Dim chckDeletion As CheckBox
                    Dim ddlOperatorACI As DropDownList
                    Dim txtrmcatcode As TextBox

                    If n <= grdCommFormula.Rows.Count Then
                        Dim gvRow As GridViewRow = grdCommFormula.Rows(n - 1)
                        lblFLineNo = gvRow.FindControl("lblFLineNo")
                        txtFrom = gvRow.FindControl("txtFrom")
                        txtTo = gvRow.FindControl("txtTo")
                        ddlOperator1 = gvRow.FindControl("ddlOperator")
                        ddlOperatorACI = gvRow.FindControl("ddlOperatorACI")
                        txtValue = gvRow.FindControl("txtValue")
                        txtAdult = gvRow.FindControl("txtAdults")
                        txtChild = gvRow.FindControl("txtChild")
                        txtExtraBed = gvRow.FindControl("txtExtraBed")
                        txtrmcatcode = gvRow.FindControl("txtrmcatcode")


                        If IsDBNull(mySqlReader("flineno")) = False Then
                            lblFLineNo.Text = CType(mySqlReader("flineno"), Integer)
                        End If
                        If IsDBNull(mySqlReader("lowerslab")) = False Then
                            txtFrom.Text = CType(mySqlReader("lowerslab"), String)
                        End If
                        If IsDBNull(mySqlReader("higherslab")) = False Then
                            If CType(mySqlReader("higherslab"), String) = "0" Then
                                txtTo.Text = ""
                            Else
                                txtTo.Text = CType(mySqlReader("higherslab"), String)
                            End If
                        End If
                        If IsDBNull(mySqlReader("operator")) = False Then
                            ddlOperator1.Text = CType(mySqlReader("operator"), String).Trim
                        End If
                        If IsDBNull(mySqlReader("operatorACI")) = False Then
                            ddlOperatorACI.Text = CType(mySqlReader("operatorACI"), String).Trim
                        End If
                        'If IsDBNull(mySqlReader("markuptype")) = False Then
                        '    If mySqlReader("markuptype").ToString = "Value" Then

                        'If CType(mySqlReader("value"), String).Contains(".00") = True Then
                        '    txtValue.Text = CType(mySqlReader("value"), Integer)
                        'Else
                        '    txtValue.Text = CType(mySqlReader("value"), String)
                        'End If

                        If IsDBNull(mySqlReader("value")) = False Then
                            txtValue.Text = DecRound(mySqlReader("value"))

                        End If

                        If IsDBNull(mySqlReader("adultvalue")) = False Then
                            txtAdult.Text = DecRound(mySqlReader("adultvalue"))

                        End If
                        If IsDBNull(mySqlReader("childvalue")) = False Then
                            txtChild.Text = DecRound(mySqlReader("childvalue"))

                        End If
                        If IsDBNull(mySqlReader("meal_category")) = False Then
                            txtrmcatcode.Text = CType(mySqlReader("meal_category"), String).Trim

                        End If

                       


                    End If

                    n = n + 1

                End While
            End If

            enabletype()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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



    Protected Sub grdCommFormula_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdCommFormula.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim gvRow As GridViewRow = CType(e.Row, GridViewRow)
            Dim txtValue As TextBox = CType(gvRow.FindControl("txtValue"), TextBox)
            Dim txtAdults As TextBox = CType(gvRow.FindControl("txtAdults"), TextBox)
            Dim txtChild As TextBox = CType(gvRow.FindControl("txtChild"), TextBox)
            Dim txtExtraBed As TextBox = CType(gvRow.FindControl("txtExtraBed"), TextBox)
            Dim txtrmcatcode As TextBox = CType(gvRow.FindControl("txtrmcatcode"), TextBox)
            Dim txtFrom As TextBox = CType(gvRow.FindControl("txtFrom"), TextBox)
            Dim txtTo As TextBox = CType(gvRow.FindControl("txtTo"), TextBox)
            Dim btnAddRow1 As Button = CType(gvRow.FindControl("btnAddRow1"), Button)
            Dim ddlOperator As DropDownList = CType(gvRow.FindControl("ddlOperator"), DropDownList)
            Dim ddlOperatorACI As DropDownList = CType(gvRow.FindControl("ddlOperatorACI"), DropDownList)

            ddlOperatorACI.Attributes.Add("onchange", "enableACI('" & ddlOperatorACI.ClientID & "','" & txtAdults.ClientID & "','" & txtChild.ClientID & "','" & txtExtraBed.ClientID & "','" + CType(e.Row.RowIndex, String) + "');")


            'e.Row.Cells(4).CssClass = "MarkupValue"
            'e.Row.Cells(5).CssClass = "MarkupValue"

            'e.Row.Cells(6).CssClass = "MarkupACI"
            'e.Row.Cells(7).CssClass = "MarkupACI"
            'e.Row.Cells(8).CssClass = "MarkupACI"
            'e.Row.Cells(9).CssClass = "MarkupACI"
            iCurrecntIndex = iCurrecntIndex + 1
            txtFrom.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtTo.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            ddlOperator.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtValue.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            ddlOperatorACI.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtExtraBed.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtAdults.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtChild.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtrmcatcode.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            btnAddRow1.TabIndex = iCurrecntIndex

          

        End If
        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            e.Row.Attributes("onclick") = String.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex)
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event);"
            e.Row.Attributes("onselectstart") = "javascript:return false;"
        End If
    End Sub

    Protected Sub btnCopy_Click(sender As Object, e As System.EventArgs) Handles btnCopy.Click
        CopyClick = 2
        ' Addlines()
        copylines()
        n = 0
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = grdCommFormula.Rows.Count '+ 1


            Dim n As Integer = 0
            Dim lblFLineNo As Label
            Dim txtFrom As TextBox
            Dim txtTo As TextBox
            Dim ddlOperator1 As DropDownList
            Dim txtValue As TextBox
            Dim txtAdult As TextBox
            Dim txtChild As TextBox
            Dim txtExtraBed As TextBox
            Dim chckDeletion As CheckBox
            Dim ddlOperatorACI As DropDownList
            Dim txtrmcatcode As TextBox



            Dim k As Integer = 0

            For Each GVRow In grdCommFormula.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then


                txtFrom = GVRow.FindControl("txtFrom")
                txtTo = GVRow.FindControl("txtTo")
                ddlOperator1 = GVRow.FindControl("ddlOperator")
                txtValue = GVRow.FindControl("txtValue")
                txtAdult = GVRow.FindControl("txtAdults")
                txtChild = GVRow.FindControl("txtChild")
                txtExtraBed = GVRow.FindControl("txtExtraBed")
                ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                txtrmcatcode = GVRow.FindControl("txtrmcatcode")

                txtFrom.Text = cFromNew.Item(k)
                txtTo.Text = cToNew.Item(k)
                ddlOperator1.SelectedIndex = Operator1New.Item(k)
                txtValue.Text = ValueNew.Item(k)
                txtAdult.Text = AdultNew.Item(k)
                txtChild.Text = ChildNew.Item(k)
                txtExtraBed.Text = ExtraBedNew.Item(k)
                txtrmcatcode.Text = Rmcatnew.item(k)
                ddlOperatorACI.SelectedIndex = OperatorACINew.Item(k)

                k = k + 1

                If k >= CopyRowlist.Count Then
                    Exit For
                End If
                n = n + 1
            Next
            CopyClick = 0
            ClearArray()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Private Sub copylines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdCommFormula.Rows.Count + 1
        Dim FLineNo(count) As Integer
        Dim cFrom(count) As String
        Dim cTo(count) As String
        Dim Operator1(count) As String
        Dim Value(count) As String
        Dim ACI(count) As String
        Dim Adult(count) As String
        Dim Child(count) As String
        Dim ExtrBed(count) As String
        Dim OperatorACI(count) As String
        Dim RmCatcode(count) As String

        Dim n As Integer = 0
        Dim lblFLineNo As Label
        Dim txtFrom As TextBox
        Dim txtTo As TextBox
        Dim ddlOperator1 As DropDownList
        Dim txtValue As TextBox
        Dim txtAdult As TextBox
        Dim txtChild As TextBox
        Dim txtExtraBed As TextBox
        Dim chckDeletion As CheckBox
        Dim txtrmcatcode As TextBox
        Dim ddlOperatorACI As DropDownList
        '   CopyRow = 0
        ClearArray()
        blankrow = 0
        Try

            For Each GVRow In grdCommFormula.Rows

                chckDeletion = GVRow.FindControl("chckDeletion")
                If chckDeletion.Checked = True Then
                    CopyRowlist.Add(n)
                    CopyRow = n

                    lblFLineNo = GVRow.FindControl("lblFLineNo")
                    FLineNo(n) = CType(lblFLineNo.Text, Integer)
                    txtFrom = GVRow.FindControl("txtFrom")
                    cFrom(n) = CType(txtFrom.Text, String)
                    txtTo = GVRow.FindControl("txtTo")
                    cTo(n) = CType(txtTo.Text, String)
                    ddlOperator1 = GVRow.FindControl("ddlOperator")
                    Operator1(n) = CType(ddlOperator1.SelectedIndex, String)
                    txtValue = GVRow.FindControl("txtValue")
                    Value(n) = CType(txtValue.Text, String)
                    txtAdult = GVRow.FindControl("txtAdults")
                    Adult(n) = CType(txtAdult.Text, String)
                    txtChild = GVRow.FindControl("txtChild")
                    Child(n) = CType(txtChild.Text, String)
                    txtExtraBed = GVRow.FindControl("txtExtraBed")
                    txtrmcatcode = GVRow.FindControl("txtrmcatcode")

                    ExtrBed(n) = CType(txtExtraBed.Text, String)
                    ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")

                    cFromNew.Add(CType(txtFrom.Text, String))
                    cToNew.Add(CType(txtTo.Text, String))
                    Operator1New.Add(CType(ddlOperator1.SelectedIndex, String))
                    ValueNew.Add(CType(txtValue.Text, String))
                    AdultNew.Add(CType(txtAdult.Text, String))
                    ChildNew.Add(CType(txtChild.Text, String))
                    ExtraBedNew.Add(CType(txtExtraBed.Text, String))
                    Rmcatnew.Add(CType(txtrmcatcode.Text, String))
                    OperatorACINew.Add(CType(ddlOperatorACI.SelectedIndex, String))
                End If
                If chckDeletion.Checked = False Then
                    blankrow = blankrow + 1
                End If
                n = n + 1
            Next
            Dim k As Integer
            k = blankrow + CopyRowlist.Count
            Do While blankrow < (CopyRowlist.Count)
                btnAddRow_Click(Nothing, Nothing)
                blankrow = blankrow + 1
            Loop

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub

    Sub ClearArray()
        cFromNew.Clear()
        cToNew.Clear()
        Operator1New.Clear()
        rValueNew.Clear()
        ValueNew.Clear()
        ACINew.Clear()
        AdultNew.Clear()
        ChildNew.Clear()
        ExtraBedNew.Clear()
        Rmcatnew.clear()
        OperatorACINew.Clear()

    End Sub

    Private Function ValidateSlabRange() As Boolean
        Dim n As Integer = 0
        Dim txtTo1 As TextBox
        Dim txtFrom As TextBox
        For i As Integer = 0 To grdCommFormula.Rows.Count - 1
            If i > 0 And grdCommFormula.Rows.Count > 1 Then
                txtFrom = grdCommFormula.Rows(i).FindControl("txtFrom")
                txtTo1 = grdCommFormula.Rows(i - 1).FindControl("txtTo")


                If txtFrom.Text <> "" And txtTo1.Text <> "" Then
                    If Not (txtTo1.Text + 1) = txtFrom.Text Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Slab range is not correct.');", True)
                        SetFocus(txtFrom)
                        ValidateSlabRange = False
                        Exit Function
                    End If



                End If
            End If
        Next


        For Each grdrow In grdCommFormula.Rows

            Dim txtAdults As TextBox = grdrow.FindControl("txtAdults")
            Dim txtExtraBed As TextBox = grdrow.FindControl("txtExtraBed")
            Dim txtChild As TextBox = grdrow.FindControl("txtChild")
            Dim ddlOperatorACI As DropDownList = grdrow.FindControl("ddlOperatorACI")
            txtFrom = grdrow.FindControl("txtFrom")
            txtTo1 = grdrow.FindControl("txtTo")
            '   If txtFrom.Text <> "" And txtTo1.Text <> "" Then
            If (Val(txtAdults.Text) <> 0 Or Val(txtChild.Text) <> 0) And ddlOperatorACI.SelectedValue = "[Select]" Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select ACI Operator.');", True)
                ValidateSlabRange = False
                Exit Function

            End If

            If (Val(txtAdults.Text) = 0 And Val(txtChild.Text) = 0) And ddlOperatorACI.SelectedValue <> "[Select]" Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('ACI Operator Selected Please enter Value in Adult or Child .');", True)
                ValidateSlabRange = False
                Exit Function

            End If
            '  End If

        Next
        enabletype()
        ValidateSlabRange = True
        Return ValidateSlabRange
    End Function
    Private Sub enabletype()
        For Each grdrow In grdCommFormula.Rows

            Dim txtAdults As TextBox = grdrow.FindControl("txtAdults")
            Dim txtExtraBed As TextBox = grdrow.FindControl("txtExtraBed")
            Dim txtChild As TextBox = grdrow.FindControl("txtChild")
            Dim ddlOperatorACI As DropDownList = grdrow.FindControl("ddlOperatorACI")

            If ddlOperatorACI.SelectedValue = "*" Or ddlOperatorACI.SelectedValue = "/" Then
                txtAdults.Enabled = True
                txtChild.Enabled = False
                txtExtraBed.Enabled = False
            ElseIf ddlOperatorACI.SelectedValue = "[Select]" Then
                txtAdults.Enabled = True
                txtChild.Enabled = True
                txtExtraBed.Enabled = True
            Else
                txtAdults.Enabled = True
                txtChild.Enabled = True
                txtExtraBed.Enabled = True
            End If

        Next
    End Sub
    Protected Sub btnAddRow_Click1(sender As Object, e As System.EventArgs)
        AddGridRow()
    End Sub

    Protected Sub ddlFormulaType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFormulaType.SelectedIndexChanged
        If ddlFormulaType.Text = "Hotel" Then
            grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
        Else
            grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
        End If
    End Sub
End Class
