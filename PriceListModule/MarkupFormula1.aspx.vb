Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class MarkupFormula1
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

                hdNewForm.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='5'")
                hdAddinalFields.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='6'")

                fillDategrd(grdCommFormula, False, 1)
                If ViewState("MarkupState") = "New" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Markup Formula"
                    Page.Title = Page.Title + " " + "New Markup Formula"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("MarkupState") = "Edit" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Edit Markup Formula"
                    Page.Title = Page.Title + " " + "Edit Markup Formula"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    DisableControl()

                ElseIf ViewState("MarkupState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Markup Formula"
                    Page.Title = Page.Title + " " + "View Markup Formula"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    DisableControl()
                ElseIf ViewState("MarkupState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Markup Formula"
                    Page.Title = Page.Title + " " + "Delete Markup Formula"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("MarkupState") = "Copy" Then
                    SetFocus(txtname)
                    lblHeading.Text = "Add New Markup Formula"
                    Page.Title = Page.Title + " " + "New Markup Formula"
                    btnSave.Text = "Save"
                    ShowRecord(CType(ViewState("MarkupFormulaID"), String))
                    ShowCommFormula(CType(ViewState("MarkupFormulaID"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                End If
                'If ddlFormulaType.Text = "Hotel" Then
                '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
                'Else
                '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
                'End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MarkupFormula1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Page.Title = "Markup Formula Entry"
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

        Dim RoomComm(count) As String
        Dim RoomCommOp(count) As String
        Dim RoomNonComm(count) As String
        Dim RoomNonCommOp(count) As String
        Dim Tax(count) As String
        Dim TaxOp(count) As String
        Dim Breakfast(count) As String
        Dim BreakfastOp(count) As String
        Dim AdultEB(count) As String
        Dim AdultEBOp(count) As String
        Dim ChildSharing(count) As String
        Dim ChildSharingOp(count) As String

        Dim ChildEB(count) As String
        Dim ChildEBOp(count) As String
        Dim ExhSup(count) As String
        Dim ExhSupOp(count) As String
        Dim AdultMealsSupl(count) As String
        Dim AdultMealsSuplOp(count) As String
        Dim ChildMealsSupl(count) As String
        Dim ChildMealsSuplOp(count) As String

        Dim AdultSpclEvents(count) As String
        Dim AdultSpclEventsOp(count) As String
        Dim ChildSpclEvents(count) As String
        Dim ChildSpclEventsOp(count) As String


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
        Dim ddlOperatorACI As DropDownList

        Dim txtRoomComm As TextBox
        Dim txtRoomCommOp As TextBox
        Dim txtRoomNonComm As TextBox
        Dim txtRoomNonCommOp As TextBox
        Dim txtTax As TextBox
        Dim txtTaxOp As TextBox
        Dim txtBreakfast As TextBox
        Dim txtBreakfastOp As TextBox
        Dim txtAdultEB As TextBox
        Dim txtAdultEBOp As TextBox
        Dim txtChildSharing As TextBox
        Dim txtChildSharingOp As TextBox

        Dim txtChildEB As TextBox
        Dim txtChildEBOp As TextBox
        Dim txtExhSup As TextBox
        Dim txtExhSupOp As TextBox
        Dim txtAdultMealsSupl As TextBox
        Dim txtAdultMealsSuplOp As TextBox
        Dim txtChildMealsSupl As TextBox
        Dim txtChildMealsSuplOp As TextBox

        Dim txtAdultSpclEvents As TextBox
        Dim txtAdultSpclEventsOp As TextBox
        Dim txtChildSpclEvents As TextBox
        Dim txtChildSpclEventsOp As TextBox


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
                ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                OperatorACI(n) = CType(ddlOperatorACI.SelectedIndex, String)


                txtRoomComm = CType(GVRow.FindControl("txtRoomComm"), TextBox)
                RoomComm(n) = CType(txtRoomComm.Text, String)
                txtRoomCommOp = CType(GVRow.FindControl("txtRoomCommOp"), TextBox)
                RoomCommOp(n) = CType(txtRoomCommOp.Text, String)
                txtRoomNonComm = CType(GVRow.FindControl("txtRoomNonComm"), TextBox)
                RoomNonComm(n) = CType(txtRoomNonComm.Text, String)
                txtRoomNonCommOp = CType(GVRow.FindControl("txtRoomNonCommOp"), TextBox)
                RoomNonCommOp(n) = CType(txtRoomNonCommOp.Text, String)
                txtTax = CType(GVRow.FindControl("txtTax"), TextBox)
                Tax(n) = CType(txtTax.Text, String)
                txtTaxOp = CType(GVRow.FindControl("txtTaxOp"), TextBox)
                TaxOp(n) = CType(txtTaxOp.Text, String)
                txtBreakfast = CType(GVRow.FindControl("txtBreakfast"), TextBox)
                Breakfast(n) = CType(txtBreakfast.Text, String)
                txtBreakfastOp = CType(GVRow.FindControl("txtBreakfastOp"), TextBox)
                BreakfastOp(n) = CType(txtBreakfastOp.Text, String)
                txtAdultEB = CType(GVRow.FindControl("txtAdultEB"), TextBox)
                AdultEB(n) = CType(txtAdultEB.Text, String)
                txtAdultEBOp = CType(GVRow.FindControl("txtAdultEBOp"), TextBox)
                AdultEBOp(n) = CType(txtAdultEBOp.Text, String)
                txtChildSharing = CType(GVRow.FindControl("txtChildSharing"), TextBox)
                ChildSharing(n) = CType(txtChildSharing.Text, String)
                txtChildSharingOp = CType(GVRow.FindControl("txtChildSharingOp"), TextBox)
                ChildSharingOp(n) = CType(txtChildSharingOp.Text, String)

                txtChildEB = CType(GVRow.FindControl("txtChildEB"), TextBox)
                ChildEB(n) = CType(txtChildEB.Text, String)
                txtChildEBOp = CType(GVRow.FindControl("txtChildEBOp"), TextBox)
                ChildEBOp(n) = CType(txtChildEBOp.Text, String)
                txtExhSup = CType(GVRow.FindControl("txtExhSup"), TextBox)
                ExhSup(n) = CType(txtExhSup.Text, String)
                txtExhSupOp = CType(GVRow.FindControl("txtExhSupOp"), TextBox)
                ExhSupOp(n) = CType(txtExhSupOp.Text, String)
                txtAdultMealsSupl = CType(GVRow.FindControl("txtAdultMealsSupl"), TextBox)
                AdultMealsSupl(n) = CType(txtAdultMealsSupl.Text, String)
                txtAdultMealsSuplOp = CType(GVRow.FindControl("txtAdultMealsSuplOp"), TextBox)
                AdultMealsSuplOp(n) = CType(txtAdultMealsSuplOp.Text, String)
                txtChildMealsSupl = CType(GVRow.FindControl("txtChildMealsSupl"), TextBox)
                ChildMealsSupl(n) = CType(txtChildMealsSupl.Text, String)
                txtChildMealsSuplOp = CType(GVRow.FindControl("txtChildMealsSuplOp"), TextBox)
                ChildMealsSuplOp(n) = CType(txtChildMealsSuplOp.Text, String)

                txtAdultSpclEvents = CType(GVRow.FindControl("txtAdultSpclEvents"), TextBox)
                AdultSpclEvents(n) = CType(txtAdultSpclEvents.Text, String)
                txtAdultSpclEventsOp = CType(GVRow.FindControl("txtAdultSpclEventsOp"), TextBox)
                AdultSpclEventsOp(n) = CType(txtAdultSpclEventsOp.Text, String)
                txtChildSpclEvents = CType(GVRow.FindControl("txtChildSpclEvents"), TextBox)
                ChildSpclEvents(n) = CType(txtChildSpclEvents.Text, String)
                txtChildSpclEventsOp = CType(GVRow.FindControl("txtChildSpclEventsOp"), TextBox)
                ChildSpclEventsOp(n) = CType(txtChildSpclEventsOp.Text, String)





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
                ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                ddlOperatorACI.SelectedIndex = OperatorACI(n)


                txtRoomComm = CType(GVRow.FindControl("txtRoomComm"), TextBox)
                txtRoomComm.Text = RoomComm(n)
                txtRoomCommOp = CType(GVRow.FindControl("txtRoomCommOp"), TextBox)
                txtRoomCommOp.Text = RoomCommOp(n)
                txtRoomNonComm = CType(GVRow.FindControl("txtRoomNonComm"), TextBox)
                txtRoomNonComm.Text = RoomNonComm(n)
                txtRoomNonCommOp = CType(GVRow.FindControl("txtRoomNonCommOp"), TextBox)
                txtRoomNonCommOp.Text = RoomNonCommOp(n)
                txtTax = CType(GVRow.FindControl("txtTax"), TextBox)
                txtTax.Text = Tax(n)
                txtTaxOp = CType(GVRow.FindControl("txtTaxOp"), TextBox)
                txtTaxOp.Text = TaxOp(n)
                txtBreakfast = CType(GVRow.FindControl("txtBreakfast"), TextBox)
                txtBreakfast.Text = Breakfast(n)
                txtBreakfastOp = CType(GVRow.FindControl("txtBreakfastOp"), TextBox)
                txtBreakfastOp.Text = BreakfastOp(n)
                txtAdultEB = CType(GVRow.FindControl("txtAdultEB"), TextBox)
                txtAdultEB.Text = AdultEB(n)
                txtAdultEBOp = CType(GVRow.FindControl("txtAdultEBOp"), TextBox)
                txtAdultEBOp.Text = AdultEBOp(n)
                txtChildSharing = CType(GVRow.FindControl("txtChildSharing"), TextBox)
                txtChildSharing.Text = ChildSharing(n)
                txtChildSharingOp = CType(GVRow.FindControl("txtChildSharingOp"), TextBox)
                txtChildSharingOp.Text = ChildSharingOp(n)

                txtChildEB = CType(GVRow.FindControl("txtChildEB"), TextBox)
                txtChildEB.Text = ChildEB(n)
                txtChildEBOp = CType(GVRow.FindControl("txtChildEBOp"), TextBox)
                txtChildEBOp.Text = ChildEBOp(n)
                txtExhSup = CType(GVRow.FindControl("txtExhSup"), TextBox)
                txtExhSup.Text = ExhSup(n)
                txtExhSupOp = CType(GVRow.FindControl("txtExhSupOp"), TextBox)
                txtExhSupOp.Text = ExhSupOp(n)
                txtAdultMealsSupl = CType(GVRow.FindControl("txtAdultMealsSupl"), TextBox)
                txtAdultMealsSupl.Text = AdultMealsSupl(n)
                txtAdultMealsSuplOp = CType(GVRow.FindControl("txtAdultMealsSuplOp"), TextBox)
                txtAdultMealsSuplOp.Text = AdultMealsSuplOp(n)
                txtChildMealsSupl = CType(GVRow.FindControl("txtChildMealsSupl"), TextBox)
                txtChildMealsSupl.Text = ChildMealsSupl(n)
                txtChildMealsSuplOp = CType(GVRow.FindControl("txtChildMealsSuplOp"), TextBox)
                txtChildMealsSuplOp.Text = ChildMealsSuplOp(n)

                txtAdultSpclEvents = CType(GVRow.FindControl("txtAdultSpclEvents"), TextBox)
                txtAdultSpclEvents.Text = AdultSpclEvents(n)
                txtAdultSpclEventsOp = CType(GVRow.FindControl("txtAdultSpclEventsOp"), TextBox)
                txtAdultSpclEventsOp.Text = AdultSpclEventsOp(n)
                txtChildSpclEvents = CType(GVRow.FindControl("txtChildSpclEvents"), TextBox)
                txtChildSpclEvents.Text = ChildSpclEvents(n)
                txtChildSpclEventsOp = CType(GVRow.FindControl("txtChildSpclEventsOp"), TextBox)
                txtChildSpclEventsOp.Text = ChildSpclEventsOp(n)


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

            'If ddlFormulaType.Text = "Hotel" Then
            '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
            'Else
            '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
            'End If
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

        Dim RoomComm(count) As String
        Dim RoomCommOp(count) As String
        Dim RoomNonComm(count) As String
        Dim RoomNonCommOp(count) As String
        Dim Tax(count) As String
        Dim TaxOp(count) As String
        Dim Breakfast(count) As String
        Dim BreakfastOp(count) As String
        Dim AdultEB(count) As String
        Dim AdultEBOp(count) As String
        Dim ChildSharing(count) As String
        Dim ChildSharingOp(count) As String

        Dim ChildEB(count) As String
        Dim ChildEBOp(count) As String
        Dim ExhSup(count) As String
        Dim ExhSupOp(count) As String
        Dim AdultMealsSupl(count) As String
        Dim AdultMealsSuplOp(count) As String
        Dim ChildMealsSupl(count) As String
        Dim ChildMealsSuplOp(count) As String

        Dim AdultSpclEvents(count) As String
        Dim AdultSpclEventsOp(count) As String
        Dim ChildSpclEvents(count) As String
        Dim ChildSpclEventsOp(count) As String

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

        Dim txtRoomComm As TextBox
        Dim txtRoomCommOp As TextBox
        Dim txtRoomNonComm As TextBox
        Dim txtRoomNonCommOp As TextBox
        Dim txtTax As TextBox
        Dim txtTaxOp As TextBox
        Dim txtBreakfast As TextBox
        Dim txtBreakfastOp As TextBox
        Dim txtAdultEB As TextBox
        Dim txtAdultEBOp As TextBox
        Dim txtChildSharing As TextBox
        Dim txtChildSharingOp As TextBox

        Dim txtChildEB As TextBox
        Dim txtChildEBOp As TextBox
        Dim txtExhSup As TextBox
        Dim txtExhSupOp As TextBox
        Dim txtAdultMealsSupl As TextBox
        Dim txtAdultMealsSuplOp As TextBox
        Dim txtChildMealsSupl As TextBox
        Dim txtChildMealsSuplOp As TextBox

        Dim txtAdultSpclEvents As TextBox
        Dim txtAdultSpclEventsOp As TextBox
        Dim txtChildSpclEvents As TextBox
        Dim txtChildSpclEventsOp As TextBox

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
                    ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                    OperatorACI(n) = CType(ddlOperatorACI.SelectedIndex, String)

                    txtRoomComm = CType(GVRow.FindControl("txtRoomComm"), TextBox)
                    RoomComm(n) = CType(txtRoomComm.Text, String)
                    txtRoomCommOp = CType(GVRow.FindControl("txtRoomCommOp"), TextBox)
                    RoomCommOp(n) = CType(txtRoomCommOp.Text, String)
                    txtRoomNonComm = CType(GVRow.FindControl("txtRoomNonComm"), TextBox)
                    RoomNonComm(n) = CType(txtRoomNonComm.Text, String)
                    txtRoomNonCommOp = CType(GVRow.FindControl("txtRoomNonCommOp"), TextBox)
                    RoomNonCommOp(n) = CType(txtRoomNonCommOp.Text, String)
                    txtTax = CType(GVRow.FindControl("txtTax"), TextBox)
                    Tax(n) = CType(txtTax.Text, String)
                    txtTaxOp = CType(GVRow.FindControl("txtTaxOp"), TextBox)
                    TaxOp(n) = CType(txtTaxOp.Text, String)
                    txtBreakfast = CType(GVRow.FindControl("txtBreakfast"), TextBox)
                    Breakfast(n) = CType(txtBreakfast.Text, String)
                    txtBreakfastOp = CType(GVRow.FindControl("txtBreakfastOp"), TextBox)
                    BreakfastOp(n) = CType(txtBreakfastOp.Text, String)
                    txtAdultEB = CType(GVRow.FindControl("txtAdultEB"), TextBox)
                    AdultEB(n) = CType(txtAdultEB.Text, String)
                    txtAdultEBOp = CType(GVRow.FindControl("txtAdultEBOp"), TextBox)
                    AdultEBOp(n) = CType(txtAdultEBOp.Text, String)
                    txtChildSharing = CType(GVRow.FindControl("txtChildSharing"), TextBox)
                    ChildSharing(n) = CType(txtChildSharing.Text, String)
                    txtChildSharingOp = CType(GVRow.FindControl("txtChildSharingOp"), TextBox)
                    ChildSharingOp(n) = CType(txtChildSharingOp.Text, String)

                    txtChildEB = CType(GVRow.FindControl("txtChildEB"), TextBox)
                    ChildEB(n) = CType(txtChildEB.Text, String)
                    txtChildEBOp = CType(GVRow.FindControl("txtChildEBOp"), TextBox)
                    ChildEBOp(n) = CType(txtChildEBOp.Text, String)
                    txtExhSup = CType(GVRow.FindControl("txtExhSup"), TextBox)
                    ExhSup(n) = CType(txtExhSup.Text, String)
                    txtExhSupOp = CType(GVRow.FindControl("txtExhSupOp"), TextBox)
                    ExhSupOp(n) = CType(txtExhSupOp.Text, String)
                    txtAdultMealsSupl = CType(GVRow.FindControl("txtAdultMealsSupl"), TextBox)
                    AdultMealsSupl(n) = CType(txtAdultMealsSupl.Text, String)
                    txtAdultMealsSuplOp = CType(GVRow.FindControl("txtAdultMealsSuplOp"), TextBox)
                    AdultMealsSuplOp(n) = CType(txtAdultMealsSuplOp.Text, String)
                    txtChildMealsSupl = CType(GVRow.FindControl("txtChildMealsSupl"), TextBox)
                    ChildMealsSupl(n) = CType(txtChildMealsSupl.Text, String)
                    txtChildMealsSuplOp = CType(GVRow.FindControl("txtChildMealsSuplOp"), TextBox)
                    ChildMealsSuplOp(n) = CType(txtChildMealsSuplOp.Text, String)

                    txtAdultSpclEvents = CType(GVRow.FindControl("txtAdultSpclEvents"), TextBox)
                    AdultSpclEvents(n) = CType(txtAdultSpclEvents.Text, String)
                    txtAdultSpclEventsOp = CType(GVRow.FindControl("txtAdultSpclEventsOp"), TextBox)
                    AdultSpclEventsOp(n) = CType(txtAdultSpclEventsOp.Text, String)
                    txtChildSpclEvents = CType(GVRow.FindControl("txtChildSpclEvents"), TextBox)
                    ChildSpclEvents(n) = CType(txtChildSpclEvents.Text, String)
                    txtChildSpclEventsOp = CType(GVRow.FindControl("txtChildSpclEventsOp"), TextBox)
                    ChildSpclEventsOp(n) = CType(txtChildSpclEventsOp.Text, String)

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
                ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")
                ddlOperatorACI.SelectedIndex = OperatorACI(n)

                txtRoomComm = CType(GVRow.FindControl("txtRoomComm"), TextBox)
                txtRoomComm.Text = RoomComm(n)
                txtRoomCommOp = CType(GVRow.FindControl("txtRoomCommOp"), TextBox)
                txtRoomCommOp.Text = RoomCommOp(n)
                txtRoomNonComm = CType(GVRow.FindControl("txtRoomNonComm"), TextBox)
                txtRoomNonComm.Text = RoomNonComm(n)
                txtRoomNonCommOp = CType(GVRow.FindControl("txtRoomNonCommOp"), TextBox)
                txtRoomNonCommOp.Text = RoomNonCommOp(n)
                txtTax = CType(GVRow.FindControl("txtTax"), TextBox)
                txtTax.Text = Tax(n)
                txtTaxOp = CType(GVRow.FindControl("txtTaxOp"), TextBox)
                txtTaxOp.Text = TaxOp(n)
                txtBreakfast = CType(GVRow.FindControl("txtBreakfast"), TextBox)
                txtBreakfast.Text = Breakfast(n)
                txtBreakfastOp = CType(GVRow.FindControl("txtBreakfastOp"), TextBox)
                txtBreakfastOp.Text = BreakfastOp(n)
                txtAdultEB = CType(GVRow.FindControl("txtAdultEB"), TextBox)
                txtAdultEB.Text = AdultEB(n)
                txtAdultEBOp = CType(GVRow.FindControl("txtAdultEBOp"), TextBox)
                txtAdultEBOp.Text = AdultEBOp(n)
                txtChildSharing = CType(GVRow.FindControl("txtChildSharing"), TextBox)
                txtChildSharing.Text = ChildSharing(n)
                txtChildSharingOp = CType(GVRow.FindControl("txtChildSharingOp"), TextBox)
                txtChildSharingOp.Text = ChildSharingOp(n)

                txtChildEB = CType(GVRow.FindControl("txtChildEB"), TextBox)
                txtChildEB.Text = ChildEB(n)
                txtChildEBOp = CType(GVRow.FindControl("txtChildEBOp"), TextBox)
                txtChildEBOp.Text = ChildEBOp(n)
                txtExhSup = CType(GVRow.FindControl("txtExhSup"), TextBox)
                txtExhSup.Text = ExhSup(n)
                txtExhSupOp = CType(GVRow.FindControl("txtExhSupOp"), TextBox)
                txtExhSupOp.Text = ExhSupOp(n)
                txtAdultMealsSupl = CType(GVRow.FindControl("txtAdultMealsSupl"), TextBox)
                txtAdultMealsSupl.Text = AdultMealsSupl(n)
                txtAdultMealsSuplOp = CType(GVRow.FindControl("txtAdultMealsSuplOp"), TextBox)
                txtAdultMealsSuplOp.Text = AdultMealsSuplOp(n)
                txtChildMealsSupl = CType(GVRow.FindControl("txtChildMealsSupl"), TextBox)
                txtChildMealsSupl.Text = ChildMealsSupl(n)
                txtChildMealsSuplOp = CType(GVRow.FindControl("txtChildMealsSuplOp"), TextBox)
                txtChildMealsSuplOp.Text = ChildMealsSuplOp(n)

                txtAdultSpclEvents = CType(GVRow.FindControl("txtAdultSpclEvents"), TextBox)
                txtAdultSpclEvents.Text = AdultSpclEvents(n)
                txtAdultSpclEventsOp = CType(GVRow.FindControl("txtAdultSpclEventsOp"), TextBox)
                txtAdultSpclEventsOp.Text = AdultSpclEventsOp(n)
                txtChildSpclEvents = CType(GVRow.FindControl("txtChildSpclEvents"), TextBox)
                txtChildSpclEvents.Text = ChildSpclEvents(n)
                txtChildSpclEventsOp = CType(GVRow.FindControl("txtChildSpclEventsOp"), TextBox)
                txtChildSpclEventsOp.Text = ChildSpclEventsOp(n)


                n = n + 1
            Next
            'If ddlFormulaType.Text = "Hotel" Then
            '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
            'Else
            '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupFormula1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    optionval = objUtils.GetAutoDocNo("MARKFORM", mySqlConn, sqlTrans)
                    txtcode.Value = optionval.Trim
                End If
                mySqlCmd = New SqlCommand("SP_MarkupFormula", mySqlConn, sqlTrans)
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

                Dim txtRoomComm As TextBox
                Dim txtRoomCommOp As TextBox
                Dim txtRoomNonComm As TextBox
                Dim txtRoomNonCommOp As TextBox
                Dim txtTax As TextBox
                Dim txtTaxOp As TextBox
                Dim txtBreakfast As TextBox
                Dim txtBreakfastOp As TextBox
                Dim txtAdultEB As TextBox
                Dim txtAdultEBOp As TextBox
                Dim txtChildSharing As TextBox
                Dim txtChildSharingOp As TextBox

                Dim txtChildEB As TextBox
                Dim txtChildEBOp As TextBox
                Dim txtExhSup As TextBox
                Dim txtExhSupOp As TextBox
                Dim txtAdultMealsSupl As TextBox
                Dim txtAdultMealsSuplOp As TextBox
                Dim txtChildMealsSupl As TextBox
                Dim txtChildMealsSuplOp As TextBox

                Dim txtAdultSpclEvents As TextBox
                Dim txtAdultSpclEventsOp As TextBox
                Dim txtChildSpclEvents As TextBox
                Dim txtChildSpclEventsOp As TextBox



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



                    txtRoomComm = CType(GVRow.FindControl("txtRoomComm"), TextBox)
                    txtRoomCommOp = CType(GVRow.FindControl("txtRoomCommOp"), TextBox)
                    txtRoomNonComm = CType(GVRow.FindControl("txtRoomNonComm"), TextBox)
                    txtRoomNonCommOp = CType(GVRow.FindControl("txtRoomNonCommOp"), TextBox)
                    txtTax = CType(GVRow.FindControl("txtTax"), TextBox)
                    txtTaxOp = CType(GVRow.FindControl("txtTaxOp"), TextBox)
                    txtBreakfast = CType(GVRow.FindControl("txtBreakfast"), TextBox)
                    txtBreakfastOp = CType(GVRow.FindControl("txtBreakfastOp"), TextBox)
                    txtAdultEB = CType(GVRow.FindControl("txtAdultEB"), TextBox)
                    txtAdultEBOp = CType(GVRow.FindControl("txtAdultEBOp"), TextBox)
                    txtChildSharing = CType(GVRow.FindControl("txtChildSharing"), TextBox)
                    txtChildSharingOp = CType(GVRow.FindControl("txtChildSharingOp"), TextBox)

                    txtChildEB = CType(GVRow.FindControl("txtChildEB"), TextBox)
                    txtChildEBOp = CType(GVRow.FindControl("txtChildEBOp"), TextBox)
                    txtExhSup = CType(GVRow.FindControl("txtExhSup"), TextBox)
                    txtExhSupOp = CType(GVRow.FindControl("txtExhSupOp"), TextBox)
                    txtAdultMealsSupl = CType(GVRow.FindControl("txtAdultMealsSupl"), TextBox)
                    txtAdultMealsSuplOp = CType(GVRow.FindControl("txtAdultMealsSuplOp"), TextBox)
                    txtChildMealsSupl = CType(GVRow.FindControl("txtChildMealsSupl"), TextBox)
                    txtChildMealsSuplOp = CType(GVRow.FindControl("txtChildMealsSuplOp"), TextBox)

                    txtAdultSpclEvents = CType(GVRow.FindControl("txtAdultSpclEvents"), TextBox)
                    txtAdultSpclEventsOp = CType(GVRow.FindControl("txtAdultSpclEventsOp"), TextBox)
                    txtChildSpclEvents = CType(GVRow.FindControl("txtChildSpclEvents"), TextBox)
                    txtChildSpclEventsOp = CType(GVRow.FindControl("txtChildSpclEventsOp"), TextBox)

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
                    If txtChild.Text.Trim <> "" Then
                        strBuffer.Append(" <ExtraBed>" & txtExtraBed.Text.Trim & "</ExtraBed>")
                    Else
                        strBuffer.Append(" <ExtraBed>0</ExtraBed>")
                    End If
                    strBuffer.Append(" <OperatorACI>" & ddlOperatorACI.SelectedItem.Text.Trim & " </OperatorACI>")


                    If txtRoomComm.Text.Trim <> "" Then
                        strBuffer.Append(" <room_comm>" & txtRoomComm.Text.Trim & "</room_comm>")
                    Else
                        strBuffer.Append(" <room_comm>0</room_comm>")
                    End If
                    strBuffer.Append(" <room_comm_op>" & txtRoomCommOp.Text.Trim & " </room_comm_op>")

                    If txtRoomNonComm.Text.Trim <> "" And hdAddinalFields.Value.ToString.Trim.ToUpper = "SHOW".ToUpper Then 'added "show" condition by mohamed on 31/03/2018
                        strBuffer.Append(" <room_noncomm>" & txtRoomNonComm.Text.Trim & "</room_noncomm>")
                    Else
                        strBuffer.Append(" <room_noncomm>0</room_noncomm>")
                    End If
                    strBuffer.Append(" <room_noncomm_op>" & txtRoomNonCommOp.Text.Trim & " </room_noncomm_op>")

                    If txtTax.Text.Trim <> "" And hdAddinalFields.Value.ToString.Trim.ToUpper = "SHOW".ToUpper Then 'added "show" condition by mohamed on 31/03/2018
                        strBuffer.Append(" <tax>" & txtTax.Text.Trim & "</tax>")
                    Else
                        strBuffer.Append(" <tax>0</tax>")
                    End If
                    strBuffer.Append(" <tax_op>" & txtTaxOp.Text.Trim & " </tax_op>")

                    If txtBreakfast.Text.Trim <> "" And hdAddinalFields.Value.ToString.Trim.ToUpper = "SHOW".ToUpper Then 'added "show" condition by mohamed on 31/03/2018
                        strBuffer.Append(" <breakfast>" & txtBreakfast.Text.Trim & "</breakfast>")
                    Else
                        strBuffer.Append(" <breakfast>0</breakfast>")
                    End If
                    strBuffer.Append(" <breakfast_op>" & txtBreakfastOp.Text.Trim & " </breakfast_op>")


                    If txtAdultEB.Text.Trim <> "" Then
                        strBuffer.Append(" <adult_eb>" & txtAdultEB.Text.Trim & "</adult_eb>")
                    Else
                        strBuffer.Append(" <adult_eb>0</adult_eb>")
                    End If
                    strBuffer.Append(" <adult_eb_op>" & txtAdultEBOp.Text.Trim & " </adult_eb_op>")

                    If txtChildSharing.Text.Trim <> "" Then
                        strBuffer.Append(" <child_sharing>" & txtChildSharing.Text.Trim & "</child_sharing>")
                    Else
                        strBuffer.Append(" <child_sharing>0</child_sharing>")
                    End If
                    strBuffer.Append(" <child_sharing_op>" & txtChildSharingOp.Text.Trim & " </child_sharing_op>")

                    If txtChildEB.Text.Trim <> "" Then
                        strBuffer.Append(" <child_eb>" & txtChildEB.Text.Trim & "</child_eb>")
                    Else
                        strBuffer.Append(" <child_eb>0</child_eb>")
                    End If
                    strBuffer.Append(" <child_eb_op>" & txtChildEBOp.Text.Trim & " </child_eb_op>")

                    If txtExhSup.Text.Trim <> "" Then
                        strBuffer.Append(" <exhib_supp>" & txtExhSup.Text.Trim & "</exhib_supp>")
                    Else
                        strBuffer.Append(" <exhib_supp>0</exhib_supp>")
                    End If
                    strBuffer.Append(" <exhib_supp_op>" & txtExhSupOp.Text.Trim & " </exhib_supp_op>")

                    If txtAdultMealsSupl.Text.Trim <> "" Then
                        strBuffer.Append(" <adult_meal_supp>" & txtAdultMealsSupl.Text.Trim & "</adult_meal_supp>")
                    Else
                        strBuffer.Append(" <adult_meal_supp>0</adult_meal_supp>")
                    End If
                    strBuffer.Append(" <adult_meal_supp_op>" & txtAdultMealsSuplOp.Text.Trim & " </adult_meal_supp_op>")


                    If txtChildMealsSupl.Text.Trim <> "" Then
                        strBuffer.Append(" <child_meal_supp>" & txtChildMealsSupl.Text.Trim & "</child_meal_supp>")
                    Else
                        strBuffer.Append(" <child_meal_supp>0</child_meal_supp>")
                    End If
                    strBuffer.Append(" <child_meal_supp_op>" & txtChildMealsSuplOp.Text.Trim & " </child_meal_supp_op>")

                    If txtAdultSpclEvents.Text.Trim <> "" Then
                        strBuffer.Append(" <adult_spcl_events>" & txtAdultSpclEvents.Text.Trim & "</adult_spcl_events>")
                    Else
                        strBuffer.Append(" <adult_spcl_events>0</adult_spcl_events>")
                    End If
                    strBuffer.Append(" <adult_spcl_events_op>" & txtAdultSpclEventsOp.Text.Trim & " </adult_spcl_events_op>")


                    If txtChildSpclEvents.Text.Trim <> "" Then
                        strBuffer.Append(" <child_spcl_events>" & txtChildSpclEvents.Text.Trim & "</child_spcl_events>")
                    Else
                        strBuffer.Append(" <child_spcl_events>0</child_spcl_events>")
                    End If
                    strBuffer.Append(" <child_spcl_events_op>" & txtChildSpclEventsOp.Text.Trim & " </child_spcl_events_op>")


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
                        strscript = "window.opener.__doPostBack('MarkupWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    End If
                Else
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('MarkupWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupFormula1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If (ViewState("MarkupState") = "New") Or ViewState("MarkupState") = "Copy" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "markupformula_header", "formulaName", txtname.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This markup formula name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf (ViewState("MarkupState") = "Edit") Then
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
            Dim lblFLineNo As Label
            Dim n As Integer = 0
            Dim txtFrom As TextBox
            Dim txtTo As TextBox
            Dim ddlOperator1 As DropDownList
            Dim txtValue As TextBox
            Dim txtAdult As TextBox
            Dim txtChild As TextBox
            Dim txtExtraBed As TextBox




            Dim count = grdCommFormula.Rows.Count
            Dim iRpwCnt As Integer = 0
            For Each GVRow In grdCommFormula.Rows

                lblFLineNo = GVRow.FindControl("lblFLineNo")
                lblFLineNo = GVRow.FindControl("lblFLineNo")
                txtFrom = GVRow.FindControl("txtFrom")
                txtTo = GVRow.FindControl("txtTo")
                'ddlOperator1 = GVRow.FindControl("ddlOperator")
                'txtValue = GVRow.FindControl("txtValue")
                'txtAdult = GVRow.FindControl("txtAdults")
                'txtChild = GVRow.FindControl("txtChild")
                'txtExtraBed = GVRow.FindControl("txtExtraBed")

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
                'If ddlOperator1.SelectedIndex = 0 Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select any operator');", True)
                '    SetFocus(ddlOperator1)
                '    ValidateFormulaTerm = False
                '    Exit Function
                'End If

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
            mySqlCmd = New SqlCommand("select *,(select currname from currmast c where c.currcode=markupformula_header.currcode)currname from markupformula_header Where FormulaID='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("MarkupFormula1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "markupformula_detail", "count(FormulaID)", "FormulaID", RefCode)
            If lngCnt = 0 Then lngCnt = 1
            fillDategrd(grdCommFormula, False, lngCnt)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("select * from markupformula_detail where FormulaID='" & RefCode & "'", mySqlConn)
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

                    Dim txtRoomComm As TextBox
                    Dim txtRoomCommOp As TextBox
                    Dim txtRoomNonComm As TextBox
                    Dim txtRoomNonCommOp As TextBox
                    Dim txtTax As TextBox
                    Dim txtTaxOp As TextBox
                    Dim txtBreakfast As TextBox
                    Dim txtBreakfastOp As TextBox
                    Dim txtAdultEB As TextBox
                    Dim txtAdultEBOp As TextBox
                    Dim txtChildSharing As TextBox
                    Dim txtChildSharingOp As TextBox

                    Dim txtChildEB As TextBox
                    Dim txtChildEBOp As TextBox
                    Dim txtExhSup As TextBox
                    Dim txtExhSupOp As TextBox
                    Dim txtAdultMealsSupl As TextBox
                    Dim txtAdultMealsSuplOp As TextBox
                    Dim txtChildMealsSupl As TextBox
                    Dim txtChildMealsSuplOp As TextBox

                    Dim txtAdultSpclEvents As TextBox
                    Dim txtAdultSpclEventsOp As TextBox
                    Dim txtChildSpclEvents As TextBox
                    Dim txtChildSpclEventsOp As TextBox


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


                        txtRoomComm = CType(gvRow.FindControl("txtRoomComm"), TextBox)
                        txtRoomCommOp = CType(gvRow.FindControl("txtRoomCommOp"), TextBox)
                        txtRoomNonComm = CType(gvRow.FindControl("txtRoomNonComm"), TextBox)
                        txtRoomNonCommOp = CType(gvRow.FindControl("txtRoomNonCommOp"), TextBox)
                        txtTax = CType(gvRow.FindControl("txtTax"), TextBox)
                        txtTaxOp = CType(gvRow.FindControl("txtTaxOp"), TextBox)
                        txtBreakfast = CType(gvRow.FindControl("txtBreakfast"), TextBox)
                        txtBreakfastOp = CType(gvRow.FindControl("txtBreakfastOp"), TextBox)
                        txtAdultEB = CType(gvRow.FindControl("txtAdultEB"), TextBox)
                        txtAdultEBOp = CType(gvRow.FindControl("txtAdultEBOp"), TextBox)
                        txtChildSharing = CType(gvRow.FindControl("txtChildSharing"), TextBox)
                        txtChildSharingOp = CType(gvRow.FindControl("txtChildSharingOp"), TextBox)

                        txtChildEB = CType(gvRow.FindControl("txtChildEB"), TextBox)
                        txtChildEBOp = CType(gvRow.FindControl("txtChildEBOp"), TextBox)
                        txtExhSup = CType(gvRow.FindControl("txtExhSup"), TextBox)
                        txtExhSupOp = CType(gvRow.FindControl("txtExhSupOp"), TextBox)
                        txtAdultMealsSupl = CType(gvRow.FindControl("txtAdultMealsSupl"), TextBox)
                        txtAdultMealsSuplOp = CType(gvRow.FindControl("txtAdultMealsSuplOp"), TextBox)
                        txtChildMealsSupl = CType(gvRow.FindControl("txtChildMealsSupl"), TextBox)
                        txtChildMealsSuplOp = CType(gvRow.FindControl("txtChildMealsSuplOp"), TextBox)

                        txtAdultSpclEvents = CType(gvRow.FindControl("txtAdultSpclEvents"), TextBox)
                        txtAdultSpclEventsOp = CType(gvRow.FindControl("txtAdultSpclEventsOp"), TextBox)
                        txtChildSpclEvents = CType(gvRow.FindControl("txtChildSpclEvents"), TextBox)
                        txtChildSpclEventsOp = CType(gvRow.FindControl("txtChildSpclEventsOp"), TextBox)

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
                        If IsDBNull(mySqlReader("ExtraBed")) = False Then
                            txtExtraBed.Text = DecRound(mySqlReader("ExtraBed"))

                        End If

                        If IsDBNull(mySqlReader("room_comm")) = False Then
                            txtRoomComm.Text = DecRound(mySqlReader("room_comm"))
                        End If
                        If IsDBNull(mySqlReader("room_comm_op")) = False Then
                            txtRoomCommOp.Text = (mySqlReader("room_comm_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("room_noncomm")) = False Then
                            txtRoomNonComm.Text = DecRound(mySqlReader("room_noncomm"))
                        End If
                        If IsDBNull(mySqlReader("room_noncomm_op")) = False Then
                            txtRoomNonCommOp.Text = (mySqlReader("room_noncomm_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("tax")) = False Then
                            txtTax.Text = DecRound(mySqlReader("tax"))
                        End If
                        If IsDBNull(mySqlReader("tax_op")) = False Then
                            txtTaxOp.Text = (mySqlReader("tax_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("breakfast")) = False Then
                            txtBreakfast.Text = DecRound(mySqlReader("breakfast"))
                        End If
                        If IsDBNull(mySqlReader("breakfast_op")) = False Then
                            txtBreakfastOp.Text = (mySqlReader("breakfast_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("adult_eb")) = False Then
                            txtAdultEB.Text = DecRound(mySqlReader("adult_eb"))
                        End If
                        If IsDBNull(mySqlReader("adult_eb_op")) = False Then
                            txtAdultEBOp.Text = (mySqlReader("adult_eb_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("child_sharing")) = False Then
                            txtChildSharing.Text = DecRound(mySqlReader("child_sharing"))
                        End If
                        If IsDBNull(mySqlReader("child_sharing_op")) = False Then
                            txtChildSharingOp.Text = (mySqlReader("child_sharing_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("child_eb")) = False Then
                            txtChildEB.Text = DecRound(mySqlReader("child_eb"))
                        End If
                        If IsDBNull(mySqlReader("child_eb_op")) = False Then
                            txtChildEBOp.Text = (mySqlReader("child_eb_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("exhib_supp")) = False Then
                            txtExhSup.Text = DecRound(mySqlReader("exhib_supp"))
                        End If
                        If IsDBNull(mySqlReader("exhib_supp_op")) = False Then
                            txtExhSupOp.Text = (mySqlReader("exhib_supp_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("adult_meal_supp")) = False Then
                            txtAdultMealsSupl.Text = DecRound(mySqlReader("adult_meal_supp"))
                        End If
                        If IsDBNull(mySqlReader("adult_meal_supp_op")) = False Then
                            txtAdultMealsSuplOp.Text = (mySqlReader("adult_meal_supp_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("child_meal_supp")) = False Then
                            txtChildMealsSupl.Text = DecRound(mySqlReader("child_meal_supp"))
                        End If
                        If IsDBNull(mySqlReader("child_meal_supp_op")) = False Then
                            txtChildMealsSuplOp.Text = (mySqlReader("child_meal_supp_op")).ToString
                        End If
                        If IsDBNull(mySqlReader("adult_spcl_events")) = False Then
                            txtAdultSpclEvents.Text = DecRound(mySqlReader("adult_spcl_events"))
                        End If
                        If IsDBNull(mySqlReader("adult_spcl_events_op")) = False Then
                            txtAdultSpclEventsOp.Text = (mySqlReader("adult_spcl_events_op")).ToString
                        End If

                        If IsDBNull(mySqlReader("child_spcl_events")) = False Then
                            txtChildSpclEvents.Text = DecRound(mySqlReader("child_spcl_events"))
                        End If
                        If IsDBNull(mySqlReader("child_spcl_events_op")) = False Then
                            txtChildSpclEventsOp.Text = (mySqlReader("child_spcl_events_op")).ToString
                        End If


                    End If

                    n = n + 1

                End While
            End If

            enabletype()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkupFormula1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetOperators(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lstOperrator As New List(Of String)
        Try
            If prefixText = "" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("*", "*"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("/", "/"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("+", "+"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("-", "-"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("%", "%"))
            ElseIf prefixText = "*" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("*", "*"))
            ElseIf prefixText = "/" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("/", "/"))
            ElseIf prefixText = "+" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("+", "+"))
            ElseIf prefixText = "-" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("-", "-"))
            ElseIf prefixText = "%" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("%", "%"))
            Else
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("*", "*"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("/", "/"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("+", "+"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("-", "-"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("%", "%"))
            End If

            Return lstOperrator
        Catch ex As Exception
            Return lstOperrator
        End Try

    End Function

    Protected Sub grdCommFormula_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdCommFormula.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            If hdAddinalFields.Value.ToString.Trim.ToUpper <> "SHOW".ToUpper Then
                e.Row.Cells(4).Attributes.Add("style", "display:none")
                e.Row.Cells(5).Attributes.Add("style", "display:none")
                e.Row.Cells(6).Attributes.Add("style", "display:none")
                e.Row.Cells(3).Text = "Room"

            End If

        End If


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim gvRow As GridViewRow = CType(e.Row, GridViewRow)
            'Dim txtValue As TextBox = CType(gvRow.FindControl("txtValue"), TextBox)
            'Dim txtAdults As TextBox = CType(gvRow.FindControl("txtAdults"), TextBox)
            'Dim txtChild As TextBox = CType(gvRow.FindControl("txtChild"), TextBox)
            'Dim txtExtraBed As TextBox = CType(gvRow.FindControl("txtExtraBed"), TextBox)

            Dim txtFrom As TextBox = CType(gvRow.FindControl("txtFrom"), TextBox)
            Dim txtTo As TextBox = CType(gvRow.FindControl("txtTo"), TextBox)



            Dim txtRoomComm As TextBox = CType(gvRow.FindControl("txtRoomComm"), TextBox)
            Dim txtRoomCommOp As TextBox = CType(gvRow.FindControl("txtRoomCommOp"), TextBox)
            Dim txtRoomNonComm As TextBox = CType(gvRow.FindControl("txtRoomNonComm"), TextBox)
            Dim txtRoomNonCommOp As TextBox = CType(gvRow.FindControl("txtRoomNonCommOp"), TextBox)
            Dim txtTax As TextBox = CType(gvRow.FindControl("txtTax"), TextBox)
            Dim txtTaxOp As TextBox = CType(gvRow.FindControl("txtTaxOp"), TextBox)
            Dim txtBreakfast As TextBox = CType(gvRow.FindControl("txtBreakfast"), TextBox)
            Dim txtBreakfastOp As TextBox = CType(gvRow.FindControl("txtBreakfastOp"), TextBox)


            Dim txtAdultEB As TextBox = CType(gvRow.FindControl("txtAdultEB"), TextBox)
            Dim txtAdultEBOp As TextBox = CType(gvRow.FindControl("txtAdultEBOp"), TextBox)
            Dim txtChildSharing As TextBox = CType(gvRow.FindControl("txtChildSharing"), TextBox)
            Dim txtChildSharingOp As TextBox = CType(gvRow.FindControl("txtChildSharingOp"), TextBox)

            Dim txtChildEB As TextBox = CType(gvRow.FindControl("txtChildEB"), TextBox)
            Dim txtChildEBOp As TextBox = CType(gvRow.FindControl("txtChildEBOp"), TextBox)
            Dim txtExhSup As TextBox = CType(gvRow.FindControl("txtExhSup"), TextBox)
            Dim txtExhSupOp As TextBox = CType(gvRow.FindControl("txtExhSupOp"), TextBox)
            Dim txtAdultMealsSupl As TextBox = CType(gvRow.FindControl("txtAdultMealsSupl"), TextBox)
            Dim txtAdultMealsSuplOp As TextBox = CType(gvRow.FindControl("txtAdultMealsSuplOp"), TextBox)
            Dim txtChildMealsSupl As TextBox = CType(gvRow.FindControl("txtChildMealsSupl"), TextBox)
            Dim txtChildMealsSuplOp As TextBox = CType(gvRow.FindControl("txtChildMealsSuplOp"), TextBox)

            Dim txtAdultSpclEvents As TextBox = CType(gvRow.FindControl("txtAdultSpclEvents"), TextBox)
            Dim txtAdultSpclEventsOp As TextBox = CType(gvRow.FindControl("txtAdultSpclEventsOp"), TextBox)
            Dim txtChildSpclEvents As TextBox = CType(gvRow.FindControl("txtChildSpclEvents"), TextBox)
            Dim txtChildSpclEventsOp As TextBox = CType(gvRow.FindControl("txtChildSpclEventsOp"), TextBox)


            Dim btnAddRow1 As Button = CType(gvRow.FindControl("btnAddRow1"), Button)
            Dim ddlOperator As DropDownList = CType(gvRow.FindControl("ddlOperator"), DropDownList)
            Dim ddlOperatorACI As DropDownList = CType(gvRow.FindControl("ddlOperatorACI"), DropDownList)

            '   ddlOperatorACI.Attributes.Add("onchange", "enableACI('" & ddlOperatorACI.ClientID & "','" & txtAdults.ClientID & "','" & txtChild.ClientID & "','" & txtExtraBed.ClientID & "','" + CType(e.Row.RowIndex, String) + "');")


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

            'iCurrecntIndex = iCurrecntIndex + 1
            'ddlOperator.TabIndex = iCurrecntIndex
            If hdAddinalFields.Value.ToString.Trim.ToUpper <> "SHOW".ToUpper Then

                iCurrecntIndex = iCurrecntIndex + 1
                txtRoomCommOp.TabIndex = iCurrecntIndex

                iCurrecntIndex = iCurrecntIndex + 1
                txtRoomComm.TabIndex = iCurrecntIndex


                'grdCommFormula.HeaderRow.Cells(4).Visible = False
                'grdCommFormula.HeaderRow.Cells(5).Visible = False
                'grdCommFormula.HeaderRow.Cells(6).Visible = False


                e.Row.Cells(4).Attributes.Add("style", "display:none")
                e.Row.Cells(5).Attributes.Add("style", "display:none")
                e.Row.Cells(6).Attributes.Add("style", "display:none")

            Else



                iCurrecntIndex = iCurrecntIndex + 1
                txtRoomCommOp.TabIndex = iCurrecntIndex

                iCurrecntIndex = iCurrecntIndex + 1
                txtRoomComm.TabIndex = iCurrecntIndex


                iCurrecntIndex = iCurrecntIndex + 1
                txtRoomNonCommOp.TabIndex = iCurrecntIndex

                iCurrecntIndex = iCurrecntIndex + 1
                txtRoomNonComm.TabIndex = iCurrecntIndex


                iCurrecntIndex = iCurrecntIndex + 1
                txtTaxOp.TabIndex = iCurrecntIndex

                iCurrecntIndex = iCurrecntIndex + 1
                txtTax.TabIndex = iCurrecntIndex



                iCurrecntIndex = iCurrecntIndex + 1
                txtBreakfastOp.TabIndex = iCurrecntIndex

                iCurrecntIndex = iCurrecntIndex + 1
                txtBreakfast.TabIndex = iCurrecntIndex
            End If



            iCurrecntIndex = iCurrecntIndex + 1
            txtAdultEBOp.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtAdultEB.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtChildSharingOp.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtChildSharing.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtChildEBOp.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtChildEB.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtExhSupOp.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtExhSup.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtAdultMealsSuplOp.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtAdultMealsSupl.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtChildMealsSuplOp.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtChildMealsSupl.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtAdultSpclEventsOp.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtAdultSpclEvents.TabIndex = iCurrecntIndex


            iCurrecntIndex = iCurrecntIndex + 1
            txtChildSpclEventsOp.TabIndex = iCurrecntIndex

            iCurrecntIndex = iCurrecntIndex + 1
            txtChildSpclEvents.TabIndex = iCurrecntIndex


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

                txtFrom.Text = cFromNew.Item(k)
                txtTo.Text = cToNew.Item(k)
                ddlOperator1.SelectedIndex = Operator1New.Item(k)
                txtValue.Text = ValueNew.Item(k)
                txtAdult.Text = AdultNew.Item(k)
                txtChild.Text = ChildNew.Item(k)
                txtExtraBed.Text = ExtraBedNew.Item(k)
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
                    ExtrBed(n) = CType(txtExtraBed.Text, String)
                    ddlOperatorACI = GVRow.FindControl("ddlOperatorACI")

                    cFromNew.Add(CType(txtFrom.Text, String))
                    cToNew.Add(CType(txtTo.Text, String))
                    Operator1New.Add(CType(ddlOperator1.SelectedIndex, String))
                    ValueNew.Add(CType(txtValue.Text, String))
                    AdultNew.Add(CType(txtAdult.Text, String))
                    ChildNew.Add(CType(txtChild.Text, String))
                    ExtraBedNew.Add(CType(txtExtraBed.Text, String))
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
            If (txtFrom.Text = "" And txtTo1.Text = "") Or (txtFrom.Text = "" And txtTo1.Text <> "") Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter From or To cost.');", True)
                ValidateSlabRange = False
                Exit Function
            End If
            'If txtFrom.Text <> "" And txtTo1.Text <> "" Then
            '    If (Val(txtAdults.Text) <> 0 Or Val(txtChild.Text) <> 0 Or Val(txtExtraBed.Text) <> 0) And ddlOperatorACI.SelectedValue = "[Select]" Then

            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select ACI Operator.');", True)
            '        ValidateSlabRange = False
            '        Exit Function

            '    End If

            '    If (Val(txtAdults.Text) = 0 And Val(txtChild.Text) = 0 And Val(txtExtraBed.Text) = 0) And ddlOperatorACI.SelectedValue <> "[Select]" Then

            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('ACI Operator Selected Please enter Value in Adult or Child Or Extra Bed.');", True)
            '        ValidateSlabRange = False
            '        Exit Function

            '    End If
            'End If

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
        'If ddlFormulaType.Text = "Hotel" Then
        '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit"
        'Else
        '    grdCommFormula.HeaderRow.Cells(4).Text = "Unit Per Pax"
        'End If
    End Sub
End Class
