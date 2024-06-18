#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Partial Class ExcursionClassification
    Inherits System.Web.UI.Page





#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction

    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Dim objUser As New clsUser
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("CitiesState", Request.QueryString("State"))
                ViewState.Add("CitiesRefCode", Request.QueryString("RefCode"))


                hdOPMode.Value = "S"
                hdFillByGrid.Value = "S"
                hdLinkButtonValue.Value = ""
                btnSave.Visible = False

                btnNew.Enabled = True
                txtName.ReadOnly = True
                txtExcClsCode.Enabled = False
                txtExcClsTagName.ReadOnly = True
                txtExcClsCode.ReadOnly = True
                btnEdit.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
                trNameAndCode.Visible = False
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                If hdOPMode.Value = "N" Then
                    lblHeading.Text = "Add Excursion Classification"
                    Page.Title = Page.Title + " " + "New "
                    btnSave.Text = "Save"


                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf hdOPMode.Value = "E" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Excursion Classification"
                    Page.Title = Page.Title + " " + "Edit Excursion Classification"
                    btnSave.Text = "Update"



                ElseIf hdOPMode.Value = "D" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Excursion Classification"
                    Page.Title = Page.Title + " " + "Delete Excursion Classification"
                    btnSave.Text = "Delete"




                ElseIf hdOPMode.Value = "S" Then
                    lblHeading.Text = "Excursion Classification"
                    Page.Title = Page.Title + " " + "Excursion Classification"
                    btnSave.Text = "Save"
                End If


                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                Else
                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                CType(strappname, String), "ExcursionModule\ExcursionClassification.aspx?appid=" + strappid, btnNew, btnEdit, _
                                                btnDelete, btnView, btnExcel, btnPrint)

                End If


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ' Create a Dynamic datatable ---- Start
                    Dim dtDynamic = New DataTable()
                    Dim dcCode = New DataColumn("Code", GetType(String))
                    Dim dcEXCCLSCode = New DataColumn("EXCCLSCode", GetType(String))
                    dtDynamic.Columns.Add(dcCode)
                    dtDynamic.Columns.Add(dcEXCCLSCode)
                    Session("sDtDynamic") = dtDynamic
                    ' --------end

                    ' Create a Dynamic datatable ---- Start
                    Session("sDtDynamicSearch") = Nothing
                    Dim dtDynamicSearch = New DataTable()
                    Dim dcSearchCode = New DataColumn("Code", GetType(String))
                    Dim dcSearchValue = New DataColumn("Value", GetType(String))
                    Dim dcSearchCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                    dtDynamicSearch.Columns.Add(dcSearchCode)
                    dtDynamicSearch.Columns.Add(dcSearchValue)
                    dtDynamicSearch.Columns.Add(dcSearchCodeAndValue)
                    Session("sDtDynamicSearch") = dtDynamicSearch

                    ' Create a Dynamic datatable ---- Start
                    Dim dtExcSuppClsDetails = New DataTable()
                    ''   Dim dtHotelGroupDetails = New DataTable()
                    Dim dcExcSuppClsType = New DataColumn("Type", GetType(String))
                    Dim dcExcSuppClsTypeName = New DataColumn("TypeName", GetType(String))
                    Dim dcExcSuppClsCode = New DataColumn("Code", GetType(String))
                    Dim dcGroupDetailsEXCCLS = New DataColumn("EXCCLSCode", GetType(String))
                    dtExcSuppClsDetails.Columns.Add(dcExcSuppClsType)
                    dtExcSuppClsDetails.Columns.Add(dcExcSuppClsTypeName)
                    dtExcSuppClsDetails.Columns.Add(dcExcSuppClsCode)
                    dtExcSuppClsDetails.Columns.Add(dcGroupDetailsEXCCLS)
                    Session("sDtExcSuppClsDetails") = dtExcSuppClsDetails

                    Session("strsortexpression") = "othtypmast.othtypname"

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Session("strsortexpression") = "othtypmast.othtypname"
        End If
        FillGridNew()
        'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SuppliersWindowPostBack") Then
        '    FilterGrid()
        'End If
    End Sub
#End Region
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"

                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region


#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Try

            If Page.IsValid = True Then
                If hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    If txtExcClsTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter group name.');", True)
                        Exit Sub
                    End If

                    Dim dtsExcSuppClsDetails As DataTable
                    dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")
                    'Commented / changed by mohamed on 05/06/2018
                    'If dtsExcSuppClsDetails.Rows.Count <= 0 Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Excursion Name are not selected.');", True)
                    '    Exit Sub
                    'End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If hdOPMode.Value = "N" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_ExcClassification", mySqlConn, sqlTrans)
                        frmmode = 1
                        lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='EXCCLS'")
                        optionval = objUtils.GetAutoDocNo("EXCCLS", mySqlConn, sqlTrans)
                        txtExcClsCode.Text = optionval.Trim

                    ElseIf hdOPMode.Value = "E" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_ExcClassification", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If

                    Dim strBuffer As New Text.StringBuilder
                    If dtsExcSuppClsDetails.Rows.Count > 0 Then

                        strBuffer.Append("<ExcursionClassifications>")
                        For i = 0 To dtsExcSuppClsDetails.Rows.Count - 1
                            strBuffer.Append("<ExcursionClassification>")
                            strBuffer.Append("<classificationcode>" & txtExcClsCode.Text.Trim & "</classificationcode>")
                            strBuffer.Append("<othtypcode>" & dtsExcSuppClsDetails.Rows(i)("Code").ToString & " </othtypcode>")
                            strBuffer.Append("</ExcursionClassification>")
                        Next
                        strBuffer.Append("</ExcursionClassifications>")
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@classificationcode", SqlDbType.VarChar, 20)).Value = CType(txtExcClsCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@classificationtagname", SqlDbType.VarChar, 100)).Value = CType((Trim(txtExcClsTagName.Text.Trim).Replace("(ET)", "")).Replace("(EC)", ""), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ValidXMLInput", SqlDbType.Xml)).Value = strBuffer.ToString
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@OPMode", SqlDbType.Int)).Value = frmmode

                    mySqlCmd.ExecuteNonQuery()


                ElseIf hdOPMode.Value = "D" Then
                    'If checkForDeletion() = False Then
                    '    Exit Sub
                    'End If
                    If txtExcClsTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Classification name.');", True)
                        txtExcClsTagName.Focus()
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_Delete_ExcClassification", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@classificationcode", SqlDbType.VarChar, 20)).Value = CType(txtExcClsCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If hdOPMode.Value = "N" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Excursion Classification Created Successfully..');", True)
                ElseIf hdOPMode.Value = "E" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Excursion Classification Modified Successfully..');", True)
                ElseIf hdOPMode.Value = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Excursion Classification Deleted Successfully..');", True)
                End If
                ClearFields()
                hdOPMode.Value = "S"

                Dim col As System.Drawing.Color
                col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
                btnCancel_new.BackColor = col
                btnCancel_new.ForeColor = Drawing.Color.White

                Dim col1 As System.Drawing.Color
                col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
                btnNew.BackColor = col1
                btnEdit.BackColor = col1
                btnDelete.BackColor = col1
                btnView.BackColor = col1
                btnNew.ForeColor = col
                btnEdit.ForeColor = col
                btnView.ForeColor = col
                btnDelete.ForeColor = col

                gvSearchGrid.Visible = True
                gv_SearchResult.Visible = False
                dlList.Visible = False
                dlListSearch.Visible = True
                trNameAndCode.Visible = False
                lblHeading.Text = "Excursion Classification"

                FillGridNew()

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("ExcursionClassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region




#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("CitiesSearch.aspx", False)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CntryGrpWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region


#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If hdOPMode.Value = "N" Then

            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excclassification_header", "classificationname", ((Trim(txtExcClsTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Classification name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf hdOPMode.Value = "E" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "excclassification_header", "classificationcode", "classificationname", CType(((Trim(txtExcClsTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(HG)", ""), String), txtExcClsCode.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Classification name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    '#Region "Public Function checkForDeletion() As Boolean"
    '    Public Function checkForDeletion() As Boolean
    '    End Function
    '#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcClassification','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    ''' <summary>
    ''' chkSelectAll_CheckedChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSelectAll"), CheckBox)
        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        Dim dtsExcSuppClsDetails As DataTable

        dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")

        For Each row In gv_SearchResult.Rows
            Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            Dim lblExcSuppCode As Label = CType(row.FindControl("lblExcSuppCode"), Label)
            Dim lblExcSuppName As Label = CType(row.FindControl("lblExcSuppName"), Label)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
                iFlag = 0
                If dtsExcSuppClsDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsExcSuppClsDetails.Rows.Count - 1
                        If dtsExcSuppClsDetails.Rows(i)("Code").ToString = lblExcSuppCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsExcSuppClsDetails.NewRow()

                        If txtNameNew.Text.Contains("(EC)") Then
                            dtsExcSuppClsDetails.Rows.Add("EC", txtNameNew.Text, lblExcSuppCode.Text, lblExcSuppName.Text)
                        ElseIf txtNameNew.Text.Contains("(ET)") Then
                            dtsExcSuppClsDetails.Rows.Add("ET", txtNameNew.Text, lblExcSuppCode.Text, lblExcSuppName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsExcSuppClsDetails.Rows.Add("T", txtNameNew.Text, lblExcSuppCode.Text, lblExcSuppName.Text)
                        End If

                        Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails

                    End If

                End If

            Else
                ChkBoxRows.Checked = False
                If dtsExcSuppClsDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsExcSuppClsDetails.Rows.Count - 1
                        If dtsExcSuppClsDetails.Rows(i)("Code").ToString = lblExcSuppCode.Text Then
                            dtsExcSuppClsDetails.Rows.Remove(dtsExcSuppClsDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails
            End If

            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsExcSuppClsDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("EXCCLSCode").ToString = "EXCURSION" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("E", "EXCURSION")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()


        Next
    End Sub

    ''' <summary>
    ''' GetHotelGroup 
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetExcursionCls(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim excursionclsName As New List(Of String)
        Try

            strSqlQry = "select rtrim(ltrim(classificationname)) classificationname from excclassification_header where  classificationname like  " & "'" & prefixText & "%'  order by classificationname"
            'and grouptype<>'C'
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    excursionclsName.Add(myDS.Tables(0).Rows(i)("classificationname").ToString())
                Next

            End If

            Return excursionclsName
        Catch ex As Exception
            Return excursionclsName
        End Try

    End Function




    ''' <summary>
    ''' btnNew_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        btnCancel.Attributes.Add("class", "btnExampleHold")
        hdOPMode.Value = "N"
        hdFillByGrid.Value = "N"
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        txtName.ReadOnly = False
        txtExcClsTagName.ReadOnly = False
        txtExcClsCode.ReadOnly = False
        txtExcClsTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtExcClsCode.Enabled = False
        txtExcClsTagName.Focus()
        lblHeading.Text = "Add Excursion Classification"
        btnSave.Text = "Save"
        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnNew.BackColor = col
        btnNew.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnEdit.BackColor = col1
        btnDelete.BackColor = col1
        btnCancel_new.BackColor = col1
        btnView.BackColor = col1
        btnEdit.ForeColor = col
        btnDelete.ForeColor = col
        btnView.ForeColor = col
        btnCancel_new.ForeColor = col


        gvSearchGrid.Visible = False
        dlListSearch.Visible = False

        gv_SearchResult.Visible = True
        dlList.Visible = True
        btnSave.Visible = True

        trNameAndCode.Visible = True

    End Sub
    ''' <summary>
    ''' btnEdit_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        txtName.ReadOnly = False
        txtExcClsTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        hdOPMode.Value = "E"
        txtExcClsTagName.ReadOnly = False
        txtExcClsTagName.Focus()
        btnSave.Text = "Update"
        lblHeading.Text = "Edit Excursion Classification"


        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnEdit.BackColor = col
        btnEdit.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnDelete.BackColor = col1
        btnCancel_new.BackColor = col1
        btnView.BackColor = col1
        btnNew.ForeColor = col
        btnDelete.ForeColor = col
        btnView.ForeColor = col
        btnCancel_new.ForeColor = col

        gvSearchGrid.Visible = False
        dlListSearch.Visible = False

        gv_SearchResult.Visible = True
        dlList.Visible = True
        btnSave.Visible = True
        trNameAndCode.Visible = True
    End Sub
    ''' <summary>
    ''' btnDelete_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        hdOPMode.Value = "D"
        txtExcClsTagName.ReadOnly = False
        btnSave.Text = "Delete"
        lblHeading.Text = "Delete Excursion Classification"
        txtExcClsTagName.Focus()


        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnDelete.BackColor = col
        btnDelete.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnEdit.BackColor = col1
        btnCancel_new.BackColor = col1
        btnView.BackColor = col1
        btnNew.ForeColor = col
        btnEdit.ForeColor = col
        btnView.ForeColor = col
        btnCancel_new.ForeColor = col

        gvSearchGrid.Visible = False
        dlListSearch.Visible = False

        gv_SearchResult.Visible = True
        dlList.Visible = True
        btnSave.Visible = True
        trNameAndCode.Visible = True
    End Sub
    ''' <summary>
    ''' btnCancel_new_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancel_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel_new.Click
        hdOPMode.Value = "S"
        txtName.ReadOnly = True
        txtExcClsTagName.ReadOnly = True
        txtExcClsCode.ReadOnly = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        txtExcClsTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtExcClsCode.Text = ""
        Dim dtsExcSuppClsDetails As DataTable
        dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")
        dtsExcSuppClsDetails.Rows.Clear()
        Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails
        gv_SearchResult.DataSource = dtsExcSuppClsDetails
        gv_SearchResult.DataBind()

        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtDynamic") = dtDynamic
        dlList.DataSource = dtDynamic
        dlList.DataBind()
        ClearFields()
        txtName.Enabled = True
        gv_SearchResult.Enabled = True
        lblHeading.Text = "Excursion Classification"
        btnSave.Visible = True

        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnCancel_new.BackColor = col
        btnCancel_new.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnEdit.BackColor = col1
        btnDelete.BackColor = col1
        btnView.BackColor = col1
        btnNew.ForeColor = col
        btnEdit.ForeColor = col
        btnView.ForeColor = col
        btnDelete.ForeColor = col

        gvSearchGrid.Visible = True
        dlListSearch.Visible = True

        gv_SearchResult.Visible = False
        dlList.Visible = False

        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")
        dtt.Rows.Clear()
        dlListSearch.DataSource = dtt
        dlListSearch.DataBind()
        Session("sDtDynamicSearch") = dtt
        FillGridNew()
        btnSave.Visible = False

        trNameAndCode.Visible = False
        txtExcClsTagName.ReadOnly = False
        txtExcClsTagName.Enabled = True

    End Sub
    ''' <summary>
    ''' lbCountry_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdFillByGrid.Value = "Y"
        hdLinkButtonValue.Value = ""
        Dim strlbValue As String = ""

        Dim myButton As LinkButton = CType(sender, LinkButton)

        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
        Dim lb As Label = CType(dlItem.FindControl("lblType"), Label)

        If Not myButton Is Nothing Then
            strlbValue = myButton.Text
            If strlbValue = "EXCURSION" Then
                strlbValue = "%"
            End If

            hdLinkButtonValue.Value = strlbValue
            Try
                FillGridByLinkButton()
                FillCheckbox()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionClassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Finally


            End Try

        End If



    End Sub
    ''' <summary>
    ''' gv_SearchResult_PageIndexChanging
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        If hdFillByGrid.Value = "Y" Then
            FillGridByLinkButton()
            FillCheckbox()
        Else
            FilterGrid()
            FillCheckbox()
        End If

    End Sub
    ''' <summary>
    ''' FillCheckbox
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillCheckbox()
        Dim dtsExcSuppClsDetails As DataTable
        dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")

        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        If dtsExcSuppClsDetails.Rows.Count > 0 Then

            For Each row In gv_SearchResult.Rows

                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                Dim lblExcSuppCode As Label = CType(row.FindControl("lblExcSuppCode"), Label)
                Dim lblExcSuppName As Label = CType(row.FindControl("lblExcSuppName"), Label)


                For i As Integer = 0 To dtsExcSuppClsDetails.Rows.Count - 1
                    If dtsExcSuppClsDetails.Rows(i)("Code").ToString.Trim = lblExcSuppCode.Text.Trim Then
                        ChkBoxRows.Checked = True
                        Exit For
                    Else
                        ChkBoxRows.Checked = False
                    End If

                Next
            Next

        End If
    End Sub
    ''' <summary>
    ''' chkSelect_CheckedChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Dim ChkBoxRows As CheckBox = CType(sender, CheckBox)
            Dim lblExcSuppCode As Label = CType(ChkBoxRows.FindControl("lblExcSuppCode"), Label)
            Dim lblExcSuppName As Label = CType(ChkBoxRows.FindControl("lblExcSuppName"), Label)
            Dim row As GridViewRow
            Dim iFlag As Integer = 0
            Dim iFlagCheckedAll As Integer = 0
            Dim iFlagUnCheckedAll As Integer = 0
            Dim dtsExcSuppClsDetails As DataTable
            dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")

            If ChkBoxRows.Checked = True Then
                ChkBoxRows.Checked = True



                If dtsExcSuppClsDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsExcSuppClsDetails.Rows.Count - 1
                        If dtsExcSuppClsDetails.Rows(i)("Code").ToString = lblExcSuppCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsExcSuppClsDetails.NewRow()
                        If txtNameNew.Text.Contains("(EC)") Then
                            dtsExcSuppClsDetails.Rows.Add("EC", txtNameNew.Text, lblExcSuppCode.Text, lblExcSuppName.Text)

                        ElseIf txtNameNew.Text.Contains("(ET)") Then
                            dtsExcSuppClsDetails.Rows.Add("ET", txtNameNew.Text, lblExcSuppCode.Text, lblExcSuppName.Text)

                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsExcSuppClsDetails.Rows.Add("T", txtNameNew.Text, lblExcSuppCode.Text, lblExcSuppName.Text)
                        End If

                        Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails

                    End If

                End If

            Else

                ChkBoxRows.Checked = False
                If dtsExcSuppClsDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsExcSuppClsDetails.Rows.Count - 1
                        If dtsExcSuppClsDetails.Rows(i)("Code").Trim.ToString = lblExcSuppCode.Text.Trim Then
                            dtsExcSuppClsDetails.Rows.Remove(dtsExcSuppClsDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails
            End If


            Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSelectAll"), CheckBox)
            Dim row1 As GridViewRow
            For Each row1 In gv_SearchResult.Rows
                Dim ChkBoxRows1 As CheckBox = CType(row1.FindControl("chkSelect"), CheckBox)
                If ChkBoxRows1.Checked = True Then
                    iFlagCheckedAll = 0
                Else
                    iFlagCheckedAll = 1
                    Exit For
                End If

            Next

            If iFlagCheckedAll = 0 And ChkBoxHeader.Checked = False Then
                ChkBoxHeader.Checked = True
            End If
            If iFlagCheckedAll = 1 And ChkBoxHeader.Checked = True Then
                ChkBoxHeader.Checked = False
            End If


            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsExcSuppClsDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("EXCCLSCode").ToString = "EXCURSION" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("E", "EXCURSION")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionClassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' FillGridByLinkButton
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillGridByLinkButton()
        Dim strorderby As String = "othtypname"
        Dim strsortorder As String = "ASC"
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True

        Dim strlbValue As String = hdLinkButtonValue.Value
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""


        If strlbValue.Trim <> "%" Then
            strSqlQry = "select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excclassification( othtypmast.othtypcode),'') ExcClassification ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser,0 sortorder from othtypmast inner join  othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "
        Else
            strSqlQry = "with ctee  as (select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excclassification( othtypmast.othtypcode),'') ExcClassification ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser,0 sortorder from othtypmast inner join " & _
                " othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') )select * from ctee othtypmast where ExcClassification ='' or ExcClassification ='" & txtExcClsTagName.Text & "'"
        End If
        If strlbValue.Trim <> "" Then

            If strlbValue.Contains("(ET)") Then
                strlbValue = ((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(EC)", ""))
                strlbValue = "'" & strlbValue & "'"

                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(othtypmast.othtypname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND  upper(othtypmast.othtypname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(EC)") Then
                strlbValue = ((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(EC)", ""))
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " othtypmast.othtypcode in (select egd.othtypcode   from excclassification_header ech eg,excclassification_detail egd where eg.classificationcode = egd.classificationcode and classificationname = " & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " othtypmast.othtypcode in (select egd.othtypcode   from excclassification_header eg,excclassification_detail egd where eg.classificationcode = egd.classificationcode and classificationname =" & Trim(strlbValue) & ")"
                End If
            End If



            If strlbValue.Contains("(T)") Then
                strlbValue = (((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(T)", "")).Replace("(EG)", ""))

                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strlbValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = " ( upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' OR  othtypmast.othtypcode in (select ECd.othtypcode   from excclassification_header EC,excclassification_detail ECd where EC.classificationcode = egd.classificationcode and classificationname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ))"
                            strWhereCond1 = strWhereCond1 & " OR   (upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or othtypmast.othtypcode in (select egd.othtypcode   from excclassification_header eg,excclassification_detail egd where eg.classificationcode = egd.classificationcode and classificationname   LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " or (" & strWhereCond1 & ")"
                End If

            End If
        End If


        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
        Else
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
        End If

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS)
        gv_SearchResult.DataBind()


        Dim dtsExcSuppClsDetails As DataTable
        Dim strValues As String = ""
        Dim strQuery As String = ""
        dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")

        If myDS.Tables(0).Rows.Count > 0 Then

            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                strValues = ""
                strValues = myDS.Tables(0).Rows(i)("othtypcode").ToString
                If dtsExcSuppClsDetails.Rows.Count > 0 Then
                    For j As Integer = 0 To dtsExcSuppClsDetails.Rows.Count - 1
                        If dtsExcSuppClsDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
                            myDS.Tables(0).Rows(i)("sortorder") = 1
                            Exit For
                        End If
                    Next

                End If


            Next
        End If



        Dim dataView As DataView = New DataView(myDS.Tables(0))
        dataView.Sort = "sortorder desc, othtypname asc"
        gv_SearchResult.DataSource = dataView
        If myDS.Tables(0).Rows.Count > 0 Then
            gv_SearchResult.DataBind()
        Else
            gv_SearchResult.PageIndex = 0
            gv_SearchResult.DataBind()
            lblMsg.Visible = True
            lblMsg.Text = "Records not found, Please redefine search criteria."
        End If
    End Sub
    ''' <summary>
    ''' txtGroupTagName_TextChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtExcClsTagName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExcClsTagName.TextChanged

        If hdOPMode.Value = "E" Or hdOPMode.Value = "D" Or hdOPMode.Value = "S" Then

            Dim dtsExcSuppClsDetails As DataTable
            dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")

            Dim dt As New DataTable
            strSqlQry = "select ech.classificationcode,ech.classificationname,isnull(ech.active,0)active,ecd.othtypcode,(select otm.othtypname from othtypmast otm where otm.othtypcode=ecd.othtypcode)othtypname from excclassification_header ech,excclassification_detail ecd where ech.classificationcode=ecd.classificationcode and ech.classificationname='" & (Trim(txtExcClsTagName.Text.Trim)).Replace("(EC)", "") & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    txtExcClsCode.Text = dt.Rows(i)("classificationcode").ToString

                    dtsExcSuppClsDetails.NewRow()
                    dtsExcSuppClsDetails.Rows.Add("EC", txtExcClsTagName.Text, dt.Rows(i)("othtypcode").ToString, dt.Rows(i)("othtypname").ToString)


                    If dt.Rows(i)("active").ToString = "0" Then
                        chkActive.Checked = False
                    Else
                        chkActive.Checked = True
                    End If

                Next
                FillGridForEdit("othtypmast.othtypname")

                If txtExcClsTagName.Text.Contains("(EC)") Then
                    Dim dtt As DataTable
                    Dim iFlag As Integer = 0
                    dtt = Session("sDtDynamic")
                    If dtt.Rows.Count >= 0 Then
                        For i = 0 To dtt.Rows.Count - 1
                            If dtt.Rows(i)("EXCCLSCode").ToString = txtExcClsTagName.Text Then
                                iFlag = 1
                            End If
                        Next
                        If iFlag = 0 Then
                            dtt.NewRow()
                            dtt.Rows.Add("EC", txtExcClsTagName.Text)
                            Session("sDtDynamic") = dtt
                            dlList.DataSource = dtt
                            dlList.DataBind()
                        End If

                    End If

                End If


            End If

            FillCheckbox()
            If hdOPMode.Value = "D" Then
                gv_SearchResult.Enabled = False
                txtName.Enabled = False
            End If
            If hdOPMode.Value = "S" Then
                gv_SearchResult.Enabled = False
                txtName.Enabled = False
            End If
        End If
    End Sub
    ''' <summary>
    ''' FillGridForEdit
    ''' </summary>
    ''' <param name="strorderby"></param>
    ''' <param name="strsortorder"></param>
    ''' <remarks></remarks>
    Private Sub FillGridForEdit(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        hdFillByGrid.Value = "N"
        hdLinkButtonValue.Value = ""
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = "select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excclassification( othtypmast.othtypcode),'') ExcClassification ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            " othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "


            Dim strlbValue As String = ""
            strlbValue = ((Trim(txtExcClsTagName.Text.Trim).Replace("(ET)", "")).Replace("(EC)", ""))
            strlbValue = "'" & strlbValue & "'"
            If Trim(strWhereCond) = "" Then
                strWhereCond = " othtypmast.othtypcode in (select ecd.othtypcode   from excclassification_header ech,excclassification_detail ecd where ech.classificationcode = ecd.classificationcode and classificationname=" & Trim(strlbValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND othtypmast.othtypcode in (select ecd.othtypcode   from excclassification_header ech,excclassification_detail ecd where ech.classificationcode = ecd.classificationcode and classificationname" & Trim(strlbValue) & ")"
            End If


            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gv_SearchResult.DataBind()
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionClassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try

    End Sub
    ''' <summary>
    ''' ClearFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearFields()
        hdOPMode.Value = "S"
        txtName.ReadOnly = True
        txtExcClsTagName.ReadOnly = True
        txtExcClsCode.ReadOnly = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        txtExcClsTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtExcClsCode.Text = ""
        btnSave.Text = "Save"

        Dim dtsExcSuppClsDetails As DataTable
        dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")
        dtsExcSuppClsDetails.Rows.Clear()
        Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails
        gv_SearchResult.DataSource = dtsExcSuppClsDetails
        gv_SearchResult.DataBind()


        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtDynamic") = dtDynamic
        dlList.DataSource = dtDynamic
        dlList.DataBind()

        txtName.Enabled = True
        gv_SearchResult.Enabled = True
    End Sub
    ''' <summary>
    ''' lbClose_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim dtsExcSuppClsDetails As New DataTable
            dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")

            'Dim dtsCountryGroupDetailsNew As New DataTable
            'dtsCountryGroupDetailsNew = Session("sDtHotelGroupDetails")
            'dtsCountryGroupDetailsNew.Clear()

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbCountry"), LinkButton)

            If dtsExcSuppClsDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsExcSuppClsDetails.Rows.Count - 1 To 0 Step i - 1
                    If lb.Text.Trim = dtsExcSuppClsDetails.Rows(i)("TypeName").ToString.Trim Then
                        dtsExcSuppClsDetails.Rows.Remove(dtsExcSuppClsDetails.Rows(i))
                    End If
                    ' dtsCountryGroupDetails.Rows(i).Delete()
                    dtsExcSuppClsDetails.AcceptChanges()
                Next
            End If
            Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    ' For j As Integer = 0 To dtDynamics.Rows.Count - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("EXCCLSCode").ToString.Trim Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()



            '' Create a Dynamic datatable ---- Start
            Dim ClearDataTable = New DataTable()
            Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
            Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            Dim dcGroupDetailsEXCCLS = New DataColumn("EXCCLSCode", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsTypeName)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsEXCCLS)
            gv_SearchResult.DataSource = ClearDataTable
            gv_SearchResult.DataBind()

        Catch ex As Exception

        End Try


    End Sub
    ' ("TypeName", GetType(String))

    ''' btnView_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        lblHeading.Text = "View Excursion Classification"
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        hdOPMode.Value = "S"
        txtExcClsTagName.ReadOnly = False
        txtName.ReadOnly = True
        txtExcClsTagName.Focus()
        btnSave.Visible = False

        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnView.BackColor = col
        btnView.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnEdit.BackColor = col1
        btnDelete.BackColor = col1
        btnCancel_new.BackColor = col1
        btnNew.ForeColor = col
        btnEdit.ForeColor = col
        btnCancel_new.ForeColor = col
        btnDelete.ForeColor = col

        gvSearchGrid.Visible = True
        dlListSearch.Visible = True

        gv_SearchResult.Visible = False
        dlList.Visible = False
        txtExcClsTagName.Enabled = False
        trNameAndCode.Visible = False
    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        FilterGrid()

    End Sub

    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ":" & lsValue.Trim)
                Session("sDtDynamicSearch") = dtt
            End If
        End If
        Return True
    End Function

    Private Sub FillGridNew()

        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")

        Dim strExcValue As String = ""

        Dim strExcClsValue As String = ""

        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strHotelchainValue As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "EXCURSIONTYPE" Then
                    If strExcValue <> "" Then
                        strExcValue = strExcValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strExcValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "EXCURSIONCLASSIFICATION" Then
                    If strExcClsValue <> "" Then
                        strExcClsValue = strExcClsValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strExcClsValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If



                If dtt.Rows(i)("Code").ToString = "TEXT" Then
                    If strTextValue <> "" Then
                        strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strTextValue = dtt.Rows(i)("Value").ToString
                    End If
                End If
            Next
        End If



        Dim myDS As New DataSet
        Dim strorderby As String = Session("strsortexpression")
        Dim strsortorder As String = "ASC"
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = " select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excclassification( othtypmast.othtypcode),'') ExcClassification,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            " othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "
            If strExcValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(othtypmast.othtypname) IN (" & Trim(strExcValue) & ")"

                Else
                    strWhereCond = strWhereCond & " AND  upper(othtypmast.othtypname) IN (" & Trim(strExcValue) & ")"
                End If
            End If



            If strExcClsValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "othtypmast.othtypcode in (select ecd.othtypcode   from excclassification_header ech,excclassification_detail ecd where ech.classificationcode = ecd.classificationcode and classificationname IN (" & Trim(strExcClsValue) & "))"
                Else

                    strWhereCond = strWhereCond & " AND othtypmast.othtypcode in (select ecd.othtypcode   from excclassification_header ech,excclassification_detail ecd where ech.classificationcode = ecd.classificationcode and classificationname IN  (" & Trim(strExcClsValue) & "))"
                End If
            End If

            If strTextValue <> "" Then

                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strTextValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = " ( upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  )     or othtypmast.othtypcode in (select ecd.othtypcode   from  excclassification_header ech, excclassification_detail ecd where ech. classificationcode = ecd. classificationcode and classificationname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   ( upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  )  or othtypmast.othtypcode in (select egd.othtypcode  from excclassification_header ech,excclassification_detail egd where ech. classificationcode = ecd. classificationcode and  classificationname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "

                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                End If

            End If



            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gvSearchGrid.DataBind()
            gvSearchGrid.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gvSearchGrid.DataBind()
            Else
                gvSearchGrid.PageIndex = 0
                gvSearchGrid.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally


        End Try

    End Sub






    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvSearchGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchGrid.PageIndexChanging
        gvSearchGrid.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub

    Private Sub FillGridByType(ByVal strType As String, ByVal lsProcessValue As String)
        Dim strorderby As String = "othtypmast.othtypname"
        Dim strsortorder As String = "ASC"
        Dim Type() As String
        Dim value() As String
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        hdFillByGrid.Value = "N"
        hdLinkButtonValue.Value = ""
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            lsProcessValue = lsProcessValue.ToUpper

            strSqlQry = "with ctee as(select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excclassification( othtypmast.othtypcode),'') ExcClassification ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            " othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI')) select * from ctee othtypmast where  othtypcode not in (select othtypcode from excclassification_detail) "

            Type = strType.Split(":")
            lsProcessValue = lsProcessValue.ToUpper
            value = lsProcessValue.Split(":")
            If lsProcessValue.Trim <> "" Then
                lsProcessValue = ((Trim(lsProcessValue.Trim).Replace("(ET)", "")).Replace("(EC)", ""))
                For k = 0 To Type.GetUpperBound(0)
                    If Type(k) <> "T" Then
                        lsProcessValue = "'" & lsProcessValue & "'"
                        ' If hdOPMode.Value = "E" Then
                        value(k) = "'" & value(k) & "'"
                        '  End If
                    End If



                    If Type(k) = "ET" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(othtypmast.othtypname) IN (" & Trim(value(k).Replace("(ET)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(othtypmast.othtypname) IN (" & Trim(value(k).Replace("(ET)", "")) & ")"
                        End If
                    End If

                    If Type(k) = "EC" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "othtypmast.othtypcode in (select egd.othtypcode   from excclassification_header  eg,excclassification_detail egd where eg.classificationcode = egd.classificationcode and classificationname In (" & Trim(value(k).Replace("(EC)", "")) & "))"
                        Else
                            strWhereCond = strWhereCond & " AND othtypmast.othtypcode in (select egd.othtypcode from excclassification_header eg,excclassification_detail egd where eg.classificationcode = egd.classificationcode and classificationname IN (" & Trim(value(k).Replace("(EC)", "")) & "))"
                        End If
                    End If


                    If Type(k) = "T" Then
                        Dim lsMainArr As String()
                        Dim limitArray As String()
                        Dim strValue As String = ""
                        Dim strWhereCond1 As String = ""
                        lsMainArr = lsProcessValue.Split(",")
                        limitArray = value(k).Split(",")
                        value(k) = value(k).Replace("'", "")

                        For i = 0 To limitArray.GetUpperBound(0)
                            strValue = ""
                            strValue = limitArray(i).Replace("'", "")


                            If strValue <> "" Then

                                If Trim(strWhereCond1) = "" Then
                                    strWhereCond1 = " (upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or othtypmast.othtypcode in (select egd.othtypcode   from excclassification_header eg,excclassification_detail egd where eg.classificationcode = egd.classificationcode and classificationname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  )) "
                                Else
                                    strWhereCond1 = strWhereCond1 & " OR (upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or othtypmast.othtypcode in (select egd.othtypcode   from excclassification_header eg,excclassification_detail egd where eg.classificationcode = egd.classificationcode and classificationname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
                                End If

                            End If





                        Next
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "(" & strWhereCond1 & ")"
                        Else
                            strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                        End If
                    End If
                Next
            End If
            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gv_SearchResult.DataBind()
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionClassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try
    End Sub








    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text '.Replace(": """, ":""")
        Dim lsProcessText As String = ""

        Dim lsProcessCity As String = ""

        Dim lsProcessCountryGroup As String = ""

        Dim lsMainArr As String()
        Dim IsProcessTypes As String = ""
        Dim IsProcessValue As String = ""
        Dim IsProcessValues As String = ""
        Dim lsProcessChain As String = ""
        Dim lsProcessStatus As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        If hdOPMode.Value = "S" Then

            For i = 0 To lsMainArr.GetUpperBound(0)
                Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                    Case "EXCURSIONTYPE"
                        lsProcessCity = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("EXCURSIONTYPE", lsProcessCity, "ET")
                    Case "EXCURSIONCLASSIFICATION"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("EXCURSIONCLASSIFICATION", lsProcessCountryGroup, "EC")
                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("TEXT", lsProcessText, "T")


                End Select
            Next

            Dim dttDyn As DataTable
            dttDyn = Session("sDtDynamicSearch")
            dlListSearch.DataSource = dttDyn
            dlListSearch.DataBind()
            FillGridNew()

        ElseIf hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then
            If txtExcClsTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter group name.');", True)
                txtExcClsTagName.Focus()
                Exit Sub
            End If
            hdFillByGrid.Value = "N"
            hdLinkButtonValue.Value = ""
            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")

            For i = 0 To lsMainArr.GetUpperBound(0)
                Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                    Case "EXCURSIONTYPE"
                        lsProcessCity = lsMainArr(i).Split(":")(1) & "(ET)"
                        txtNameNew.Text = lsProcessCity
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("EXCCLSCode").ToString = lsProcessCity Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("ET", lsProcessCity)

                            End If

                        End If
                        Session("sDtDynamic") = dtt
                        'If IsProcessType = "" Then
                        '    IsProcessType = "CT"
                        'Else
                        '    IsProcessType = IsProcessType + "," + "CT"
                        'End If

                        'If IsProcessValue = "" Then
                        '    IsProcessValue = lsProcessCity
                        'Else
                        '    IsProcessValue = IsProcessValue + "," + lsProcessCity
                        'End If
                        FillGridByType("ET", lsProcessCity)
                    Case "EXCURSIONCLASSIFICATION"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1) & "(EG)"
                        txtNameNew.Text = lsProcessCountryGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("EXCCLSCode").ToString = lsProcessCountryGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("EG", lsProcessCountryGroup)
                            End If

                        End If

                        Session("sDtDynamic") = dtt


                        FillGridByType("EG", lsProcessCountryGroup)
                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1) & "(T)"
                        txtNameNew.Text = lsProcessText

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("EXCCLSCode").ToString = lsProcessText Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("T", lsProcessText)
                            End If

                        End If

                        Session("sDtDynamic") = dtt
                        FillGridByType("T", lsProcessText)

                End Select
            Next

            'Dim arCode As New ArrayList
            'For k = 0 To dtt.Rows.Count - 1
            '    If Not arCode.Contains(dtt.Rows(k).Item("Code")) Then
            '        arCode.Add(dtt.Rows(k).Item("Code"))
            '    End If
            'Next
            'If arCode.Count > 0 Then
            '    For j = 0 To arCode.Count - 1
            '        If IsProcessTypes <> "" Then
            '            IsProcessTypes = IsProcessTypes & ":" & arCode(j).ToString
            '        Else
            '            IsProcessTypes = arCode(j).ToString
            '        End If

            '        For l = 0 To dtt.Rows.Count - 1
            '            If dtt.Rows(l)("Country").ToString.Contains("(" & arCode(j).ToString.Trim & ")") Then
            '                If IsProcessValue <> "" Then

            '                    IsProcessValue = IsProcessValue & ",'" & dtt.Rows(l)("Country").ToString & "'"
            '                Else
            '                    IsProcessValue = "'" & dtt.Rows(l)("Country").ToString & "'"
            '                End If

            '            End If
            '        Next
            '        If IsProcessValues <> "" Then
            '            IsProcessValues = IsProcessValues & ":" & IsProcessValue
            '        Else
            '            IsProcessValues = IsProcessValue
            '        End If
            '        IsProcessValue = ""
            '    Next
            'End If

            'FillGridByType(IsProcessTypes, IsProcessValues)
            If dtt.Rows.Count >= 0 Then
                Dim dtsExcSuppClsDetails As DataTable
                dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")
                If dtsExcSuppClsDetails.Rows.Count > 0 Then
                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("EXCCLSCode").ToString = "EXCURSION" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("E", "EXCURSION")
                        Session("sDtDynamic") = dtt
                    End If
                End If
            End If
            dlList.DataSource = dtt
            dlList.DataBind()

            txtName.Text = ""
        ElseIf hdOPMode.Value = "D" Then
            If txtExcClsTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group name.');", True)
                txtExcClsTagName.Focus()
                Exit Sub
            End If
        End If

    End Sub


    ''' <summary>
    ''' btnResetSelection_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click



        If hdOPMode.Value = "S" Then
            Dim dtt As DataTable
            dtt = Session("sDtDynamicSearch")
            dtt.Rows.Clear()
            dlListSearch.DataSource = dtt
            dlListSearch.DataBind()
            Session("sDtDynamicSearch") = dtt
            FillGridNew()
        Else
            Dim dtsExcSuppClsDetails As DataTable
            dtsExcSuppClsDetails = Session("sDtExcSuppClsDetails")
            dtsExcSuppClsDetails.Rows.Clear()
            Session("sDtExcSuppClsDetails") = dtsExcSuppClsDetails
            gv_SearchResult.DataSource = dtsExcSuppClsDetails
            gv_SearchResult.DataBind()


            Dim dtDynamic As DataTable
            dtDynamic = Session("sDtDynamic")
            dtDynamic.Rows.Clear()
            Session("sDtDynamic") = dtDynamic
            dlList.DataSource = dtDynamic
            dlList.DataBind()
        End If



    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbCloseSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicSearch")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicSearch") = dtDynamics
            dlListSearch.DataSource = dtDynamics
            dlListSearch.DataBind()
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub





    Protected Sub gvSearchGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchGrid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblPartyName As Label = e.Row.FindControl("lblothtypname")
            Dim lblexcursiongroup As Label = e.Row.FindControl("lblexcursiongroup")


            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextHotelGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicSearch")
            If Session("sDtDynamicSearch") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""

                        If "EXCURSIONTYPE" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "EXCURSIONCLASSIFICATION" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextPartyName

                        End If

                        If lsSearchTextPartyName.Trim <> "" Then
                            lblPartyName.Text = Regex.Replace(lblPartyName.Text.Trim, lsSearchTextPartyName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblexcursiongroup.Text = Regex.Replace(lblexcursiongroup.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                    Next
                End If
            End If



        End If

    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim chkCtry2 As CheckBox = e.Row.FindControl("chkCtry2")
            'Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            'chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)


            Dim lblPartyName As Label = e.Row.FindControl("lblothtyp")
            Dim lblexcursiongroup As Label = e.Row.FindControl("lblexcursiongroup")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextHotelGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""

                        If "ET" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("EXCCLSCode").ToString.Trim.ToUpper
                            lsSearchTextPartyName = lsSearchTextPartyName.Replace("(ET)", "")
                        End If
                        If "EG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("EXCCLSCode").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCity.Replace("(EG)", "")
                        End If


                        If "T" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("EXCCLSCode").ToString.Replace("(T)", "").Trim.ToUpper
                            lsSearchTextCity = lsSearchTextPartyName

                        End If
                        If lsSearchTextPartyName.Trim <> "" Then
                            lblPartyName.Text = Regex.Replace(lblPartyName.Text.Trim, lsSearchTextPartyName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblexcursiongroup.Text = Regex.Replace(lblexcursiongroup.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)


                        End If
                    Next
                End If
            End If

        End If
    End Sub


    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGridNew()

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region
End Class


