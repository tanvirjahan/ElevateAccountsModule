#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Partial Class ExcursionGroups
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
#Region "Enum GridCol"
    Enum GridCol

        exctypcode = 1
        exctypname = 2
        excursiongroup = 3
        classificationname = 4
        ratebasis = 5
        suppliergroup = 6
        tktbasedontime = 7
        autoconfirm = 8
        sicpvt = 9
        adddate = 10
        adduser = 11
        Edit = 14
        View = 15
        Delete = 16
        Copy = 17
    End Enum
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
                txtGroupCode.Enabled = False
                txtGroupTagName.ReadOnly = True
                txtGroupCode.ReadOnly = True
                btnEdit.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
                trNameAndCode.Visible = False
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                If hdOPMode.Value = "N" Then
                    lblHeading.Text = "Add Excursion Groups"
                    Page.Title = Page.Title + " " + "New "
                    btnSave.Text = "Save"


                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf hdOPMode.Value = "E" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Excursion Group"
                    Page.Title = Page.Title + " " + "Edit Excursion Group"
                    btnSave.Text = "Update"



                ElseIf hdOPMode.Value = "D" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Excursion Group"
                    Page.Title = Page.Title + " " + "Delete Excursion Group"
                    btnSave.Text = "Delete"




                ElseIf hdOPMode.Value = "S" Then
                    lblHeading.Text = "Excursion Groups"
                    Page.Title = Page.Title + " " + "ExcursionGroups"
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
                    Exit Sub
                Else



                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                        CType(strappname, String), "ExcursionModule\ExcursionGroups.aspx?appid=" + strappid, btnAddHotel, btnExportToExcel, _
                                                                      btnHotelReport, gvSearchGrid, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)

                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                CType(strappname, String), "ExcursionModule\ExcursionGroups.aspx?appid=" + strappid, btnNew, btnEdit, _
                                                btnDelete, btnView, btnExportToExcel, btnHotelReport)


                End If
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ' Create a Dynamic datatable ---- Start
                    Dim dtDynamic = New DataTable()
                    Dim dcCode = New DataColumn("Code", GetType(String))
                    Dim dcCountry = New DataColumn("Country", GetType(String))
                    dtDynamic.Columns.Add(dcCode)
                    dtDynamic.Columns.Add(dcCountry)
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
                    Dim dtHotelGroupDetails = New DataTable()
                    Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                    Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
                    Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                    Dim dcGroupDetailsCountry = New DataColumn("Country", GetType(String))
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsType)
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsTypeName)
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsCode)
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsCountry)
                    Session("sDtHotelGroupDetails") = dtHotelGroupDetails

                    Session("strsortexpression") = "excursiontypes.exctypname"

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Session("strsortexpression") = "excursiontypes.exctypname"
        End If
        FillGridNew()
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ExcursionSuppliersWindowPostBack") Then
            FilterGrid()
        End If
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

                    If txtGroupTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter group name.');", True)
                        Exit Sub
                    End If

                    Dim dtsHotelGroupDetails As DataTable
                    dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
                    If dtsHotelGroupDetails.Rows.Count <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Excursion Name are not selected.');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If hdOPMode.Value = "N" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_ExcursionGroup", mySqlConn, sqlTrans)
                        frmmode = 1
                        lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='EXCRGRP'")
                        optionval = objUtils.GetAutoDocNo("EXCRGRP", mySqlConn, sqlTrans)
                        txtGroupCode.Text = optionval.Trim

                    ElseIf hdOPMode.Value = "E" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_ExcursionGroup", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If

                    Dim strBuffer As New Text.StringBuilder
                    If dtsHotelGroupDetails.Rows.Count > 0 Then

                        strBuffer.Append("<ExcursionGroups>")
                        For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                            strBuffer.Append("<ExcursionGroup>")
                            strBuffer.Append("<ExcursionGroupCode>" & txtGroupCode.Text.Trim & "</ExcursionGroupCode>")
                            strBuffer.Append("<othtypcode>" & dtsHotelGroupDetails.Rows(i)("Code").ToString & " </othtypcode>")
                            strBuffer.Append("</ExcursionGroup>")
                        Next
                        strBuffer.Append("</ExcursionGroups>")
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupcode", SqlDbType.VarChar, 20)).Value = CType(txtGroupCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@grouptagname", SqlDbType.VarChar, 100)).Value = CType((Trim(txtGroupTagName.Text.Trim).Replace("(ET)", "")).Replace("(EG)", ""), String)
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
                    If txtGroupTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group name.');", True)
                        txtGroupTagName.Focus()
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_Delete_ExcursionGroup", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupcode", SqlDbType.VarChar, 20)).Value = CType(txtGroupCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If hdOPMode.Value = "N" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Excursion Group Created Successfully..');", True)
                ElseIf hdOPMode.Value = "E" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Excursion Group Modified Successfully..');", True)
                ElseIf hdOPMode.Value = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Excursion Group Deleted Successfully..');", True)
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
                lblHeading.Text = "Excursion Group"

                FillGridNew()

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excursiongroup", "excursiongroupname", ((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf hdOPMode.Value = "E" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "excursiongroup", "excursiongroupcode", "excursiongroupname", CType(((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(HG)", ""), String), txtGroupCode.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=excursions','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

        For Each row In gv_SearchResult.Rows
            Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
            Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
                iFlag = 0
                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsHotelGroupDetails.NewRow()

                        If txtNameNew.Text.Contains("(EG)") Then
                            dtsHotelGroupDetails.Rows.Add("EG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(ET)") Then
                            dtsHotelGroupDetails.Rows.Add("ET", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsHotelGroupDetails.Rows.Add("T", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        End If

                        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails

                    End If

                End If

            Else
                ChkBoxRows.Checked = False
                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                            dtsHotelGroupDetails.Rows.Remove(dtsHotelGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
            End If

            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsHotelGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "EXCURSION" Then
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
    Public Shared Function GetExcursionGroup(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim excursionName As New List(Of String)
        Try

            strSqlQry = "select rtrim(ltrim(excursiongroupname)) excursiongroupname from excursiongroup where  excursiongroupname like  " & "'" & prefixText & "%'  order by excursiongroupname"
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
                    excursionName.Add(myDS.Tables(0).Rows(i)("excursiongroupname").ToString())
                Next

            End If

            Return excursionName
        Catch ex As Exception
            Return excursionName
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
        txtGroupTagName.ReadOnly = False
        txtGroupCode.ReadOnly = False
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtGroupCode.Enabled = False
        txtGroupTagName.Focus()
        lblHeading.Text = "Add Excursion Group"
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
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        hdOPMode.Value = "E"
        txtGroupTagName.ReadOnly = False
        txtGroupTagName.Focus()
        btnSave.Text = "Update"
        lblHeading.Text = "Edit Excursion Group"


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
        txtGroupTagName.ReadOnly = False
        btnSave.Text = "Delete"
        lblHeading.Text = "Delete Excursion Group"
        txtGroupTagName.Focus()


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
        txtGroupTagName.ReadOnly = True
        txtGroupCode.ReadOnly = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtGroupCode.Text = ""
        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
        dtsHotelGroupDetails.Rows.Clear()
        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
        gv_SearchResult.DataSource = dtsHotelGroupDetails
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
        lblHeading.Text = "Excursion Group"
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
        txtGroupTagName.ReadOnly = False
        txtGroupTagName.Enabled = True

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
                objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        If dtsHotelGroupDetails.Rows.Count > 0 Then

            For Each row In gv_SearchResult.Rows

                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
                Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)


                For i As Integer = 0 To dtsHotelGroupDetails.Rows.Count - 1
                    If dtsHotelGroupDetails.Rows(i)("Code").ToString.Trim = lblHotelCode.Text.Trim Then
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
            Dim lblHotelCode As Label = CType(ChkBoxRows.FindControl("lblHotelCode"), Label)
            Dim lblHotelName As Label = CType(ChkBoxRows.FindControl("lblHotelName"), Label)
            Dim row As GridViewRow
            Dim iFlag As Integer = 0
            Dim iFlagCheckedAll As Integer = 0
            Dim iFlagUnCheckedAll As Integer = 0
            Dim dtsHotelGroupDetails As DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

            If ChkBoxRows.Checked = True Then
                ChkBoxRows.Checked = True



                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsHotelGroupDetails.NewRow()
                        If txtNameNew.Text.Contains("(EG)") Then
                            dtsHotelGroupDetails.Rows.Add("EG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)

                        ElseIf txtNameNew.Text.Contains("(ET)") Then
                            dtsHotelGroupDetails.Rows.Add("ET", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)

                        ElseIf txtNameNew.Text.Contains("(C)") Then
                            dtsHotelGroupDetails.Rows.Add("C", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)

                        ElseIf txtNameNew.Text.Contains("(RB)") Then
                            dtsHotelGroupDetails.Rows.Add("RB", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)

                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsHotelGroupDetails.Rows.Add("T", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        End If

                        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails

                    End If

                End If

            Else

                ChkBoxRows.Checked = False
                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").Trim.ToString = lblHotelCode.Text.Trim Then
                            dtsHotelGroupDetails.Rows.Remove(dtsHotelGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
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

                If dtsHotelGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "EXCURSION" Then
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
            objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' FillGridByLinkButton
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillGridByLinkButton()
        Dim strorderby As String = "excursiontypes.exctypname"
        Dim strsortorder As String = "ASC"
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True

        Dim strlbValue As String = hdLinkButtonValue.Value
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""

        'strSqlQry = "select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excursiongroup( othtypmast.othtypcode),'') Excursiongroup ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser,0 sortorder from othtypmast inner join " & _
        '    " othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI')"
        strSqlQry = "select excursiontypes.exctypcode,excursiontypes.exctypname,excursiongroup.excursiongroupcode,isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'') excursiongroup,excclassification_header.classificationname,(select case when excursiontypes.ratebasis='ACS' then'Adult/Child/Senior' else excursiontypes.ratebasis end) ratebasis  ,entrytktreqd,tktbasedontime,autoconfirm,transferincl,sicpvt,(select case when excursiontypes.active='1' then 'Yes' else 'No'end) as Active,excursiontypes.adddate,excursiontypes.adduser,excursiontypes.moddate,excursiontypes.moduser,0 sortorder from excursiontypes left join excursiongroup on  excursiongroup.excursiongroupname= isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'')  inner join excclassification_header on excursiontypes.classificationcode=excclassification_header.classificationcode "

        If strlbValue.Trim <> "" Then

            If strlbValue.Contains("(ET)") Then
                strlbValue = ((((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(EG)", "")).Replace("(C)", "")).Replace("(RB)", ""))
                strlbValue = "'" & strlbValue & "'"

                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.exctypname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND  upper(excursiontypes.exctypname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(EG)") Then
                strlbValue = ((((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(EG)", "")).Replace("(C)", "")).Replace("(RB)", ""))
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname = " & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname =" & Trim(strlbValue) & ")"
                End If
            End If


            If strlbValue.Contains("(C)") Then
                strlbValue = ((((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(EG)", "")).Replace("(C)", "")).Replace("(RB)", ""))
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " excclassification_header.classificationcode in (select egd.classificationcode   from excclassification_header egd excclassificationname= " & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname =" & Trim(strlbValue) & ")"
                End If
            End If
            If strlbValue.Contains("(RB)") Then
                strlbValue = ((((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(EG)", "")).Replace("(C)", "")).Replace("(RB)", ""))
                strlbValue = "'" & strlbValue & "'"

                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.ratebasis) = (" & IIf(Trim(strlbValue) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(strlbValue)) & ")"
                Else
                    strWhereCond = strWhereCond & " AND  upper(excursiontypes.ratebasis)= (" & IIf(Trim(strlbValue) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(strlbValue)) & ")"
                End If
            End If


            If strlbValue.Contains("(T)") Then
                strlbValue = (((((Trim(strlbValue.Trim).Replace("(ET)", "")).Replace("(T)", "")).Replace("(EG)", "")).Replace("(C)", "")).Replace("(RB)", ""))

                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strlbValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = " ( upper(excursiontypes.exctypcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' OR  excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ))"
                            strWhereCond1 = strWhereCond1 & " OR   (upper(excursiontypes.exctypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname   LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
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


        Dim dtsHotelGroupDetails As DataTable
        Dim strValues As String = ""
        Dim strQuery As String = ""
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

        If myDS.Tables(0).Rows.Count > 0 Then

            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                strValues = ""
                strValues = myDS.Tables(0).Rows(i)("exctypcode").ToString
                If dtsHotelGroupDetails.Rows.Count > 0 Then
                    For j As Integer = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
                            myDS.Tables(0).Rows(i)("sortorder") = 1
                            Exit For
                        End If
                    Next

                End If


            Next
        End If



        Dim dataView As DataView = New DataView(myDS.Tables(0))
        dataView.Sort = "sortorder desc, exctypname asc"
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
    Protected Sub txtGroupTagName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGroupTagName.TextChanged

        If hdOPMode.Value = "E" Or hdOPMode.Value = "D" Or hdOPMode.Value = "S" Then

            Dim dtsHotelGroupDetails As DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

            Dim dt As New DataTable
            strSqlQry = "select eg.excursiongroupcode,eg.excursiongroupname,isnull(eg.active,0)active,egd.othtypcode,(select eg.othtypname from othtypmast eg where eg.othtypcode=egd.othtypcode)othtypname from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode=egd.excursiongroupcode and eg.excursiongroupname='" & (Trim(txtGroupTagName.Text.Trim)).Replace("(EG)", "") & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    txtGroupCode.Text = dt.Rows(i)("excursiongroupcode").ToString

                    dtsHotelGroupDetails.NewRow()
                    dtsHotelGroupDetails.Rows.Add("EG", txtGroupTagName.Text, dt.Rows(i)("othtypcode").ToString, dt.Rows(i)("othtypname").ToString)


                    If dt.Rows(i)("active").ToString = "0" Then
                        chkActive.Checked = False
                    Else
                        chkActive.Checked = True
                    End If

                Next
                FillGridForEdit("excursiontypes.exctypname")

                If txtGroupTagName.Text.Contains("(EG)") Then
                    Dim dtt As DataTable
                    Dim iFlag As Integer = 0
                    dtt = Session("sDtDynamic")
                    If dtt.Rows.Count >= 0 Then
                        For i = 0 To dtt.Rows.Count - 1
                            If dtt.Rows(i)("Country").ToString = txtGroupTagName.Text Then
                                iFlag = 1
                            End If
                        Next
                        If iFlag = 0 Then
                            dtt.NewRow()
                            dtt.Rows.Add("EG", txtGroupTagName.Text)
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

            'strSqlQry = "select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excursiongroup( othtypmast.othtypcode),'') Excursiongroup ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            '" othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "
            strSqlQry = "select excursiontypes.exctypcode,excursiontypes.exctypname,excursiongroup.excursiongroupcode,isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'') excursiongroup,excclassification_header.classificationname,excursiontypes.ratebasis,entrytktreqd,tktbasedontime,autoconfirm,transferincl,sicpvt,(select case when excursiontypes.active='1' then 'Yes' else 'No'end) as Active,excursiontypes.adddate,excursiontypes.adduser,excursiontypes.moddate,excursiontypes.moduser from excursiontypes left join excursiongroup on  excursiongroup.excursiongroupname= isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'')  inner join excclassification_header on excursiontypes.classificationcode=excclassification_header.classificationcode "

            Dim strlbValue As String = ""
            strlbValue = ((((Trim(txtGroupTagName.Text.Trim).Replace("(ET)", "")).Replace("(EG)", "")).Replace("(C)", "")).Replace("(RB)", ""))
            strlbValue = "'" & strlbValue & "'"
            If Trim(strWhereCond) = "" Then
                strWhereCond = " excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname=" & Trim(strlbValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname" & Trim(strlbValue) & ")"
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
            objUtils.WritErrorLog("HotelGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        txtGroupTagName.ReadOnly = True
        txtGroupCode.ReadOnly = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtGroupCode.Text = ""
        btnSave.Text = "Save"

        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
        dtsHotelGroupDetails.Rows.Clear()
        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
        gv_SearchResult.DataSource = dtsHotelGroupDetails
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

            Dim dtsHotelGroupDetails As New DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

            'Dim dtsCountryGroupDetailsNew As New DataTable
            'dtsCountryGroupDetailsNew = Session("sDtHotelGroupDetails")
            'dtsCountryGroupDetailsNew.Clear()

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbCountry"), LinkButton)

            If dtsHotelGroupDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsHotelGroupDetails.Rows.Count - 1 To 0 Step i - 1
                    If lb.Text.Trim = dtsHotelGroupDetails.Rows(i)("TypeName").ToString.Trim Then
                        dtsHotelGroupDetails.Rows.Remove(dtsHotelGroupDetails.Rows(i))
                    End If
                    ' dtsCountryGroupDetails.Rows(i).Delete()
                    dtsHotelGroupDetails.AcceptChanges()
                Next
            End If
            Session("sDtHotelGroupDetails") = dtsHotelGroupDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    ' For j As Integer = 0 To dtDynamics.Rows.Count - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("Country").ToString.Trim Then
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
            Dim dcGroupDetailsCountry = New DataColumn("Country", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsTypeName)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsCountry)
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
        lblHeading.Text = "View Excursion Group"
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        hdOPMode.Value = "S"
        txtGroupTagName.ReadOnly = False
        txtName.ReadOnly = True
        txtGroupTagName.Focus()
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
        txtGroupTagName.Enabled = False
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

        Dim strCountryValue As String = ""

        Dim strclsvalue As String = ""
        Dim strCountryGroupValue As String = ""

        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strHotelchainValue As String = ""
        Dim strtextratebasis As String = ""
        Dim strsicpvt As String = ""
        Dim strcombo As String = ""
        Dim strmulticost As String = ""
        Dim strsectorwiserates As String = ""

        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "EXCURSIONTYPE" Then
                    If strCountryValue <> "" Then
                        strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "CLASSIFICATION" Then
                    If strclsvalue <> "" Then
                        strclsvalue = strclsvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strclsvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "RATEBASIS" Then
                    If strtextratebasis <> "" Then
                        strtextratebasis = strtextratebasis + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strtextratebasis = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "SIC/PRIVATE" Then
                    If strsicpvt <> "" Then
                        strsicpvt = strsicpvt + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strsicpvt = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "MULTIPLECOST" Then
                    If strmulticost <> "" Then
                        strmulticost = strmulticost + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strmulticost = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "SECTORWISERATES" Then
                    If strsectorwiserates <> "" Then
                        strsectorwiserates = strsectorwiserates + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strsectorwiserates = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "COMBO" Then
                    If strcombo <> "" Then
                        strcombo = strcombo + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strcombo = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EXCURSIONGROUP" Then
                    If strCountryGroupValue <> "" Then
                        strCountryGroupValue = strCountryGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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

            'strSqlQry = " select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excursiongroup( othtypmast.othtypcode),'') Excursiongroup ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            '" othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "

            strSqlQry = "select excursiontypes.exctypcode,excursiontypes.exctypname,excursiongroup.excursiongroupcode,isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'') excursiongroup,excclassification_header.classificationname,(select case when excursiontypes.ratebasis='ACS' then'Adult/Child/Senior' else excursiontypes.ratebasis end) ratebasis  ,entrytktreqd,tktbasedontime,autoconfirm,transferincl,sicpvt,sectorwiserates,multicost,combo,(select case when excursiontypes.active='1' then 'Yes' else 'No'end) as Active,excursiontypes.adddate,excursiontypes.adduser,excursiontypes.moddate,excursiontypes.moduser from excursiontypes left join excursiongroup on  excursiongroup.excursiongroupname= isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'')  inner join excclassification_header on excursiontypes.classificationcode=excclassification_header.classificationcode "

            If strCountryValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.exctypname) IN (" & Trim(strCountryValue) & ")"

                Else
                    strWhereCond = strWhereCond & " AND  upper(excursiontypes.exctypname) IN (" & Trim(strCountryValue) & ")"
                End If
            End If



            If strCountryGroupValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "  excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname IN (" & Trim(strCountryGroupValue) & "))"
                Else

                    strWhereCond = strWhereCond & " AND excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname IN  (" & Trim(strCountryGroupValue) & "))"
                End If
            End If


            If strclsvalue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excclassification_header.classificationname) IN (" & Trim(strclsvalue) & ")"

                Else
                    strWhereCond = strWhereCond & " AND  upper(excclassification_header.classificationname) IN (" & Trim(strclsvalue) & ")"
                End If
            End If



            If strtextratebasis <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.ratebasis) IN (" & IIf(Trim(strtextratebasis) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(strtextratebasis)) & ")"

                Else
                    strWhereCond = strWhereCond & " AND  upper(excursiontypes.ratebasis) IN (" & IIf(Trim(strtextratebasis) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(strtextratebasis)) & ")"
                End If
            End If

            If strsicpvt <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.sicpvt) IN (" & Trim(strsicpvt) & ")"

                Else
                    strWhereCond = strWhereCond & "  AND  upper(excursiontypes.sicpvt) IN (" & Trim(strsicpvt) & ")"
                End If
            End If
            If strcombo <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.combo) IN (" & Trim(strcombo) & ")"

                Else
                    strWhereCond = strWhereCond & "  AND  upper(excursiontypes.combo) IN (" & Trim(strcombo) & ")"
                End If
            End If
            If strmulticost <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.multicost) IN (" & Trim(strmulticost) & ")"

                Else
                    strWhereCond = strWhereCond & "  AND  upper(excursiontypes.multicost) IN (" & Trim(strmulticost) & ")"
                End If
            End If
            If strsectorwiserates <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(excursiontypes.sectorwiserates) IN (" & Trim(strsectorwiserates) & ")"

                Else
                    strWhereCond = strWhereCond & "  AND  upper(excursiontypes.sectorwiserates) IN (" & Trim(strsectorwiserates) & ")"
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
                            strWhereCond1 = " ( upper(excursiontypes.exctypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  )     or excursiontypes.exctypcode in (select egd.othtypcode   from  excursiongroup eg, excursiongroup_detail egd where eg. excursiongroupcode = egd. excursiongroupcode and excursiongroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   ( upper(excursiontypes.exctypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  )  or excursiontypes.exctypcode in (select egd.othtypcode  from excursiongroup eg,excursiongroup_detail egd where eg. excursiongroupcode = egd. excursiongroupcode and  excursiongroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "

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
            gvSearchGrid.DataSource = dataView
            gvSearchGrid.DataBind()
        End If
    End Sub
#End Region
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
        Dim strorderby As String = "excursiontypes.exctypname"
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
            'strSqlQry = "select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excursiongroup( othtypmast.othtypcode),'') Excursiongroup ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            '" othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "
            strSqlQry = "select excursiontypes.exctypcode,excursiontypes.exctypname,excursiongroup.excursiongroupcode,isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'') excursiongroup,excclassification_header.classificationname,(select case when excursiontypes.ratebasis='ACS' then'Adult/Child/Senior' else excursiontypes.ratebasis end) ratebasis  ,entrytktreqd,tktbasedontime,autoconfirm,transferincl,sicpvt,(select case when excursiontypes.active='1' then 'Yes' else 'No'end) as Active,excursiontypes.adddate,excursiontypes.adduser,excursiontypes.moddate,excursiontypes.moduser from excursiontypes left join excursiongroup on  excursiongroup.excursiongroupname= isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'')  inner join excclassification_header on excursiontypes.classificationcode=excclassification_header.classificationcode "

            Type = strType.Split(":")
            lsProcessValue = lsProcessValue.ToUpper
            value = lsProcessValue.Split(":")
            If lsProcessValue.Trim <> "" Then
                lsProcessValue = ((((Trim(lsProcessValue.Trim).Replace("(ET)", "")).Replace("(EG)", "")).Replace("(C)", "")).Replace("(RB)", ""))
                For k = 0 To Type.GetUpperBound(0)
                    If Type(k) <> "T" Then
                        lsProcessValue = "'" & lsProcessValue & "'"
                        ' If hdOPMode.Value = "E" Then
                        value(k) = "'" & value(k) & "'"
                        '  End If
                    End If



                    If Type(k) = "ET" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(excursiontypes.exctypname) IN (" & Trim(value(k).Replace("(ET)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(excursiontypes.exctypname) IN (" & Trim(value(k).Replace("(ET)", "")) & ")"
                        End If
                    End If

                    If Type(k) = "EG" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname In (" & Trim(value(k).Replace("(EG)", "")) & "))"
                        Else
                            strWhereCond = strWhereCond & " AND excursiontypes.exctypcode in (select egd.othtypcode from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname IN (" & Trim(value(k).Replace("(EG)", "")) & "))"
                        End If
                    End If


                    If Type(k) = "C" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(excclassification_header.classificationname) IN (" & Trim(value(k).Replace("(C)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(excclassification_header.classificationname) IN (" & Trim(value(k).Replace("(C)", "")) & ")"
                        End If
                    End If


                    If Type(k) = "RB" Then
                        If Trim(strWhereCond) = "" Then

                            strWhereCond = " upper(excursiontypes.ratebasis) IN (" & IIf(Trim(value(k).Replace("(RB)", "")) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(value(k).Replace("(RB)", ""))) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(excursiontypes.ratebasis) IN (" & IIf(Trim(value(k).Replace("(RB)", "")) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(value(k).Replace("(RB)", ""))) & ")"
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
                                    strWhereCond1 = " (upper(excursiontypes.exctypname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
                                Else
                                    strWhereCond1 = strWhereCond1 & " OR (upper(excursiontypes.exctypname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or excursiontypes.exctypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
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
            objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    Case "EXCURSIONGROUP"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("EXCURSIONGROUP", lsProcessCountryGroup, "EG")
                    Case "CLASSIFICATION"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CLASSIFICATION", lsProcessCountryGroup, "C")
                    Case "SIC/PRIVATE"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SIC/PRIVATE", lsProcessCountryGroup, "SP")
                    Case "SECTORWISERATES"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SECTORWISERATES", lsProcessCountryGroup, "SW")
                    Case "COMBO"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("COMBO", lsProcessCountryGroup, "CM")
                    Case "MULTIPLECOST"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("MULTIPLECOST", lsProcessCountryGroup, "MC")

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
            If txtGroupTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter group name.');", True)
                txtGroupTagName.Focus()
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
                                If dtt.Rows(j)("Country").ToString = lsProcessCity Then
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
                    Case "EXCURSIONGROUP"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1) & "(EG)"
                        txtNameNew.Text = lsProcessCountryGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountryGroup Then
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
                    Case "CLASSIFICATION"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1) & "(C)"
                        txtNameNew.Text = lsProcessCountryGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountryGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("C", lsProcessCountryGroup)
                            End If

                        End If

                        Session("sDtDynamic") = dtt


                        FillGridByType("C", lsProcessCountryGroup)
                    Case "RATEBASIS"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1) & "(RB)"
                        txtNameNew.Text = lsProcessCountryGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountryGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("RB", lsProcessCountryGroup)
                            End If

                        End If

                        Session("sDtDynamic") = dtt


                        FillGridByType("RB", lsProcessCountryGroup)
                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1) & "(T)"
                        txtNameNew.Text = lsProcessText

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessText Then
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


            If dtt.Rows.Count >= 0 Then
                Dim dtsHotelGroupDetails As DataTable
                dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
                If dtsHotelGroupDetails.Rows.Count > 0 Then
                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "EXCURSION" Then
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
            If txtGroupTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group name.');", True)
                txtGroupTagName.Focus()
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
            Dim dtsHotelGroupDetails As DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
            dtsHotelGroupDetails.Rows.Clear()
            Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
            gv_SearchResult.DataSource = dtsHotelGroupDetails
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "Edit")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("othtypcode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('ExcursionTypesMaster.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionTypes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub lnkCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "Copy")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("othtypcode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('ExcursionTypesMaster.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','ExcursionTypes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "View")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If CType(Request.QueryString("appid"), String) = "2" Or CType(Request.QueryString("appid"), String) = "3" Or CType(Request.QueryString("appid"), String) = "9" Then
            strpop = "window.open('ExcursionTypesMaster.aspx?appid=" + CType("1", String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        Else
            strpop = "window.open('ExcursionTypesMaster.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "Delete")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("othypcode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('ExcursionTypesMaster.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAddHotel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddHotel.Click
        Session.Add("SupState", "New")
        Dim strpop As String = ""
        strpop = "window.open('ExcursionTypesMaster.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnHotelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHotelReport.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Guide&BackPageName=ExcursionGroups.aspx&excursiongroupcode=" & txtGroupCode.Text.Trim & "&excursiongroupname=" & txtName.Text.Trim & "','rptexcursiongroup');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvSearchGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchGrid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblPartyName As Label = e.Row.FindControl("lblothtypname")
            Dim lblexcursiongroup As Label = e.Row.FindControl("lblexcursiongroup")
            Dim lblclassificationname As Label = e.Row.FindControl("lblclassificationname")
            Dim lblsectorwiserates As Label = e.Row.FindControl("lblsectorwiserates")
            Dim lblcombo As Label = e.Row.FindControl("lblcombo")
            Dim lblmulticost As Label = e.Row.FindControl("lblmulticost")
            Dim lblratebasis As Label = e.Row.FindControl("lblratebasis")
            Dim lblsicpvt As Label = e.Row.FindControl("lblsicpvt")
            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchsicpvt As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextHotelGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim lssearchtextcls As String = ""
            Dim lssearchtextratebasis As String = ""
            Dim lssearchcombo As String = ""
            Dim lssearchmulticost As String = ""
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
                        If "EXCURSIONGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "CLASSIFICATION" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lssearchtextcls = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "COMBO" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lssearchcombo = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "MULTIPLECOST" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lssearchmulticost = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SIC/PRIVATE" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchsicpvt = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SECTORWISERATES" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "RATEBASIS" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lssearchtextratebasis = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
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

                        If lssearchtextcls.Trim <> "" Then
                            lblclassificationname.Text = Regex.Replace(lblclassificationname.Text.Trim, lssearchtextcls.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lsSearchsicpvt.Trim <> "" Then
                            lblsicpvt.Text = Regex.Replace(lblsicpvt.Text.Trim, lsSearchsicpvt.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)


                        End If
                        If lssearchmulticost.Trim <> "" Then
                            lblmulticost.Text = Regex.Replace(lblmulticost.Text.Trim, lssearchmulticost.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)


                        End If

                        If lssearchcombo.Trim <> "" Then
                            lblcombo.Text = Regex.Replace(lblcombo.Text.Trim, lssearchcombo.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)


                        End If
                        If lsSearchTextSector.Trim <> "" Then
                            lblsectorwiserates.Text = Regex.Replace(lblsectorwiserates.Text.Trim, lsSearchTextSector.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)


                        End If

                        If lssearchtextratebasis.Trim <> "" Then
                            lblratebasis.Text = Regex.Replace(lblratebasis.Text.Trim, lssearchtextratebasis.Trim(), _
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

            Dim lblclassificationname As Label = e.Row.FindControl("lblclassificationname")
            Dim lblratebasis As Label = e.Row.FindControl("lblratebasis")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lssearchtextcls As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextHotelGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim lssearchtextratebasis As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""

                        If "ET" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lsSearchTextPartyName = lsSearchTextPartyName.Replace("(ET)", "")
                        End If
                        If "EG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCity.Replace("(EG)", "")
                        End If

                        If "C" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lssearchtextcls = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lssearchtextcls = lssearchtextcls.Replace("(C)", "")
                        End If
                        If "RB" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lssearchtextratebasis = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lssearchtextratebasis = lssearchtextratebasis.Replace("(RB)", "")
                        End If

                        If "T" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("Country").ToString.Replace("(T)", "").Trim.ToUpper
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

                        If lssearchtextcls.Trim <> "" Then
                            lblclassificationname.Text = Regex.Replace(lblclassificationname.Text.Trim, lssearchtextcls.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)


                        End If

                        If lssearchtextratebasis.Trim <> "" Then
                            lblratebasis.Text = Regex.Replace(lblratebasis.Text.Trim, lssearchtextratebasis.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If

        End If
    End Sub


End Class


