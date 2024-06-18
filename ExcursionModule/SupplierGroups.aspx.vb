
'------------================--------------=======================------------------================
'   Module Name    :    CountryGroup.aspx
'   Developer Name :    Abin Paul
'   Date           :    21 july 2016
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region


Partial Class SupplierGroups
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
    Shared formtype As String
    Dim objUser As New clsUser
    Dim strappname As String = ""
    Dim strappid As String = ""
#End Region

#Region "Enum GridCol"
    Enum GridCol

        partycode = 1
        partyName = 2
        category = 3
        cityname = 4
        sectorname = 5
        suppliergroup = 6
        Edit = 9
        View = 10
        Delete = 11
        copy = 12
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

                '   txtAutoComplete.Attributes.Add("onkeyup", "SetContextKey()")
                hdOPMode.Value = "S"
                hdFillByGrid.Value = "S"
                hdLinkButtonValue.Value = ""
                btnSave.Visible = False
                If Request.QueryString("type") = "OTH" Then
                    formtype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1501")
                Else
                    formtype = Request.QueryString("type")
                End If

                ' formtype = CType(Request.QueryString("type"), String)
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
                    lblHeading.Text = "Add Supplier Group"
                    Page.Title = Page.Title + " " + "New "
                    btnSave.Text = "Save"
                    'txtorder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from citymast") + 1
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save city?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf hdOPMode.Value = "E" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Supplier Group"
                    Page.Title = Page.Title + " " + "Edit  Supplier Group"
                    btnSave.Text = "Update"

                ElseIf hdOPMode.Value = "D" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Supplier Group"
                    Page.Title = Page.Title + " " + "Delete Supplier Group"
                    btnSave.Text = "Delete"

                ElseIf hdOPMode.Value = "S" Then
                    lblHeading.Text = "Supplier Groups"
                    Page.Title = Page.Title + " " + "Supplier Groups"
                    ' btnSave.Text = "Delete"
                End If

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                txtDivcode.Value = ViewState("divcode")


                Dim type As String = CType(Request.QueryString("type"), String)
                Dim strappid As String = ""

                Dim strappname As String = ""

                If Request.QueryString("appid") = "4" Then
                    strappname = AppName.Value
                ElseIf Request.QueryString("appid") = "14" Then
                    strappname = AppName.Value
                Else
                    strappname = AppName.Value
                End If




                strappid = CType(Request.QueryString("appid"), String)
                'If AppName Is Nothing = False Then

                '    strappname = AppName.Value

                'End If
                'If AppId.Value = "4" Then
                '    If txtDivcode.Value Is Nothing = False Then
                '        strappid = AppId.Value
                '    End If
                '    If AppName Is Nothing = False Then
                '        If txtDivcode.Value = "01" Then
                '            strappname = AppName.Value
                '        Else
                '            strappname = AppName.Value
                '        End If
                '    End If
                'End If
                ViewState("appid") = strappid
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                    If strappid = 1 Then
                        objUser.CheckPUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                    CType(strappname, String), "PriceListModule\SupplierSearch.aspx", 39, btnAddSupp, btnExport, _
                                                    btnSuppReport, gvSearchGrid, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.copy)


                    Else
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                           CType(strappname, String), "PriceListModule\SupplierSearch.aspx?appid=" + strappid + "&type=" + type, btnAddSupp, btnExport, _
                                                           btnSuppReport, gvSearchGrid, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.copy)

                    End If

                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                          CType(strappname, String), "ExcursionModule\SupplierGroups.aspx?appid=" + strappid + "&type=" + type, btnNew, btnEdit, _
                                          btnDelete, btnView, btnExport, btnSuppReport)




                End If


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    '' Create a Dynamic datatable ---- Start
                    Dim dtDynamic = New DataTable()
                    Dim dcCode = New DataColumn("Code", GetType(String))
                    Dim dcSuppliers = New DataColumn("Suppliers", GetType(String))
                    dtDynamic.Columns.Add(dcCode)
                    dtDynamic.Columns.Add(dcSuppliers)
                    Session("sDtDynamic") = dtDynamic
                    '--------end

                    '' Create a Dynamic datatable ---- Start
                    Session("sDtDynamicSearch") = Nothing
                    Dim dtDynamicSearch = New DataTable()
                    Dim dcSearchCode = New DataColumn("Code", GetType(String))
                    Dim dcSearchValue = New DataColumn("Value", GetType(String))
                    Dim dcSearchCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                    dtDynamicSearch.Columns.Add(dcSearchCode)
                    dtDynamicSearch.Columns.Add(dcSearchValue)
                    dtDynamicSearch.Columns.Add(dcSearchCodeAndValue)
                    Session("sDtDynamicSearch") = dtDynamicSearch
                    '--------end

                    '' Create a Dynamic datatable ---- Start
                    Dim dtSupplierGroupDetails = New DataTable()
                    Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                    Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
                    Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                    Dim dcGroupDetailsCountry = New DataColumn("Suppliers", GetType(String))
                    dtSupplierGroupDetails.Columns.Add(dcGroupDetailsType)
                    dtSupplierGroupDetails.Columns.Add(dcGroupDetailsTypeName)
                    dtSupplierGroupDetails.Columns.Add(dcGroupDetailsCode)
                    dtSupplierGroupDetails.Columns.Add(dcGroupDetailsCountry)
                    Session("sDtSupplierGroupDetails") = dtSupplierGroupDetails
                    '--------end
                    Session("strsortexpression") = "partymast.partyname"
                    FillGridNew()
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Session("strsortexpression") = "partymast.partyname"
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SuppliersWindowPostBack") Then
            FilterGrid()
        End If

        'Remove columns Category, City, Sector in search grid for Supplier
        'Added by Natraj on 04/04/2021
        gv_SearchResult.Columns(3).Visible = False
        gv_SearchResult.Columns(4).Visible = False
        gv_SearchResult.Columns(5).Visible = False

        gvSearchGrid.Columns(3).Visible = False
        gvSearchGrid.Columns(4).Visible = False
        gvSearchGrid.Columns(5).Visible = False
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
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master" 'Added by Archana on 05/06/2015 for VisaModule
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   'changed by mohamed on 27/08/2018
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

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub
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
        'Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        'Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Try

            If Page.IsValid = True Then
                If hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    If txtGroupTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter group name.');", True)
                        Exit Sub
                    End If

                    Dim dtsSupplierGroupDetails As DataTable
                    dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")
                    If dtsSupplierGroupDetails.Rows.Count <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Suppliers are not selected.');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If hdOPMode.Value = "N" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_supplierGroups", mySqlConn, sqlTrans)
                        frmmode = 1
                        lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='SUPPGROUP'")
                        optionval = objUtils.GetAutoDocNo("SUPPGROUP", mySqlConn, sqlTrans)
                        txtGroupCode.Text = optionval.Trim

                    ElseIf hdOPMode.Value = "E" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_supplierGroups", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If

                    Dim strBuffer As New Text.StringBuilder
                    If dtsSupplierGroupDetails.Rows.Count > 0 Then

                        strBuffer.Append("<SupplierGroups>")
                        For i = 0 To dtsSupplierGroupDetails.Rows.Count - 1
                            strBuffer.Append("<SupplierGroup>")
                            strBuffer.Append(" <SupplierGroupCode>" & txtGroupCode.Text.Trim & "</SupplierGroupCode>")
                            strBuffer.Append(" <PartyCode>" & dtsSupplierGroupDetails.Rows(i)("Code").ToString & " </PartyCode>")
                            strBuffer.Append("</SupplierGroup>")
                        Next
                        strBuffer.Append("</SupplierGroups>")
                    End If



                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupcode", SqlDbType.VarChar, 20)).Value = CType(txtGroupCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@grouptagname", SqlDbType.VarChar, 100)).Value = CType((((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "")).Replace("(T)", ""), String)
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
                    mySqlCmd = New SqlCommand("sp_Delete_SupplierGroups", mySqlConn, sqlTrans)
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
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Group Created Successfully..');", True)
                ElseIf hdOPMode.Value = "E" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Group Modified Successfully..');", True)
                ElseIf hdOPMode.Value = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Group Deleted Successfully..');", True)
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
                lblHeading.Text = "Supplier Group"

                FillGridNew()

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("SupplierGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region




#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CitiesSearch.aspx", False)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CntryGrpWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region


#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If hdOPMode.Value = "N" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "countrygroup", "countrygroupcode", CType(txtGroupCode.Text.Trim, String)) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group code is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "Suppliergroups", "suppliergrpname", ((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(SG)", "")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf hdOPMode.Value = "E" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "suppliergroups", "suppliergrpcode", "suppliergrpname", CType(((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(SG)", ""), String), txtGroupCode.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
    End Function
#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=supplier group','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
        Dim dtsSupplierGroupDetails As DataTable
        dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")

        For Each row In gv_SearchResult.Rows
            Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            Dim lblSupplierCode As Label = CType(row.FindControl("lblSupplierCode"), Label)
            Dim lblSupplierName As Label = CType(row.FindControl("lblSupplierName"), Label)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
                iFlag = 0
                If dtsSupplierGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSupplierGroupDetails.Rows.Count - 1
                        If dtsSupplierGroupDetails.Rows(i)("Code").ToString = lblSupplierCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsSupplierGroupDetails.NewRow()
                        If txtNameNew.Text.Contains("(ST)") Then
                            dtsSupplierGroupDetails.Rows.Add("ST", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTY)") Then
                            dtsSupplierGroupDetails.Rows.Add("C", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsSupplierGroupDetails.Rows.Add("CT", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTG)") Then
                            dtsSupplierGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(SG)") Then
                            dtsSupplierGroupDetails.Rows.Add("SG", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsSupplierGroupDetails.Rows.Add("T", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        End If
                        Session("sDtSupplierGroupDetails") = dtsSupplierGroupDetails

                    End If

                End If

            Else
                ChkBoxRows.Checked = False
                If dtsSupplierGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSupplierGroupDetails.Rows.Count - 1
                        If dtsSupplierGroupDetails.Rows(i)("Code").ToString = lblSupplierCode.Text Then
                            dtsSupplierGroupDetails.Rows.Remove(dtsSupplierGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtSupplierGroupDetails") = dtsSupplierGroupDetails
            End If

            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsSupplierGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Suppliers").ToString = "Suppliers" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("S", "Suppliers")
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
    Public Shared Function GetSupplierGroup(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim supplierNames As New List(Of String)
        Try

            ' strSqlQry = "select distinct rtrim(ltrim(sh.suppliergrpname))+'(SG)' name   from suppliergroups sh left join suppliergrps_detail sd  on  sh.suppliergrpcode=sd.suppliergrpcode left join partymast p on p.partycode=sd.partycode where p.sptypecode= '" & formtype & "' and  suppliergrpname like  " & "'" & prefixText & "%'  order by name"
            strSqlQry = "select distinct rtrim(ltrim(suppliergrpname))+'(SG)'suppliergrpname   from suppliergroups  h inner join suppliergrps_detail d on  h.suppliergrpcode=d.suppliergrpcode inner join partymast p on d.partycode=p.partycode where p.sptypecode= '" & formtype & "' and  suppliergrpname like  '" & prefixText & "%'  order by suppliergrpname"
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
                    supplierNames.Add(myDS.Tables(0).Rows(i)("suppliergrpname").ToString())
                Next

            End If

            Return supplierNames
        Catch ex As Exception
            Return supplierNames
        End Try

    End Function



    ''' <summary>
    ''' btnNew_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ' btnCancel.Attributes.Add("class", "btnExampleHold")
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
        lblHeading.Text = "Add Supplier Group"

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
        lblHeading.Text = "Edit Supplier Group"


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
        lblHeading.Text = "Delete Supplier Group"
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
        Dim dtsSupplierGroupDetails As DataTable
        dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")
        dtsSupplierGroupDetails.Rows.Clear()
        Session("sDtHotelGroupDetails") = dtsSupplierGroupDetails
        gv_SearchResult.DataSource = dtsSupplierGroupDetails
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
        lblHeading.Text = "Supplier Group"
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
            If strlbValue = "Suppliers" Then
                strlbValue = "%"
            End If

            hdLinkButtonValue.Value = strlbValue
            Try
                FillGridByLinkButton()
                FillCheckbox()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        Dim dtsSupplierGroupDetails As DataTable
        dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")

        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        If dtsSupplierGroupDetails.Rows.Count > 0 Then

            For Each row In gv_SearchResult.Rows

                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                Dim lblSupplierCode As Label = CType(row.FindControl("lblSupplierCode"), Label)
                Dim lblSupplierName As Label = CType(row.FindControl("lblSupplierName"), Label)


                For i As Integer = 0 To dtsSupplierGroupDetails.Rows.Count - 1
                    If dtsSupplierGroupDetails.Rows(i)("Code").ToString.Trim = lblSupplierCode.Text.Trim Then
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
            Dim lblSupplierCode As Label = CType(ChkBoxRows.FindControl("lblSupplierCode"), Label)
            Dim lblSupplierName As Label = CType(ChkBoxRows.FindControl("lblSupplierName"), Label)
            Dim row As GridViewRow
            Dim iFlag As Integer = 0
            Dim iFlagCheckedAll As Integer = 0
            Dim iFlagUnCheckedAll As Integer = 0
            Dim dtsSupplierGroupDetails As DataTable
            dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")

            If ChkBoxRows.Checked = True Then
                ChkBoxRows.Checked = True



                If dtsSupplierGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSupplierGroupDetails.Rows.Count - 1
                        If dtsSupplierGroupDetails.Rows(i)("Code").ToString = lblSupplierCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsSupplierGroupDetails.NewRow()
                        If txtNameNew.Text.Contains("(ST)") Then
                            dtsSupplierGroupDetails.Rows.Add("ST", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(G)") Then
                            dtsSupplierGroupDetails.Rows.Add("G", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTY)") Then
                            dtsSupplierGroupDetails.Rows.Add("CTY", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsSupplierGroupDetails.Rows.Add("CT", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTG)") Then
                            dtsSupplierGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(SG)") Then
                            dtsSupplierGroupDetails.Rows.Add("SG", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsSupplierGroupDetails.Rows.Add("T", txtNameNew.Text, lblSupplierCode.Text, lblSupplierName.Text)
                        End If

                        Session("sDtSupplierGroupDetails") = dtsSupplierGroupDetails

                    End If

                End If

            Else

                ChkBoxRows.Checked = False
                If dtsSupplierGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSupplierGroupDetails.Rows.Count - 1
                        If dtsSupplierGroupDetails.Rows(i)("Code").Trim.ToString = lblSupplierCode.Text.Trim Then
                            dtsSupplierGroupDetails.Rows.Remove(dtsSupplierGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtSupplierGroupDetails") = dtsSupplierGroupDetails
            End If

            ' Check all check box is checked  or not.
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

                If dtsSupplierGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Suppliers").ToString = "Suppliers" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("S", "Suppliers")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' FillGridByLinkButton
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillGridByLinkButton()
        Dim strorderby As String = "partymast.partyname"
        Dim strsortorder As String = "ASC"
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True

        Dim strlbValue As String = hdLinkButtonValue.Value
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""

        If Request.QueryString("type") = "OTH" Then
            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode=(select option_selected from reservation_parameters where param_id='1501')"
        Else
            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode='" & Request.QueryString("type") & "'"
        End If


        If strlbValue.Trim <> "" Then

            If strlbValue.Contains("(CTY)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"

                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(ST)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(sectormaster.sectorname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(CT)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(citymast.cityname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(citymast.cityname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(CTG)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(catmast.catname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(catmast.catname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(SG)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select cgd.partycode   from Suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname=" & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from Suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname=" & Trim(strlbValue) & ")"
                End If
            End If



            If strlbValue.Contains("(T)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")

                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strlbValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = " ( upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or    upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   (upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
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


        Dim dtsSupplierGroupDetails As DataTable
        Dim strValues As String = ""
        Dim strQuery As String = ""
        dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")

        If myDS.Tables(0).Rows.Count > 0 Then
            lblMsg.Visible = False
            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                strValues = ""
                strValues = myDS.Tables(0).Rows(i)("partycode").ToString
                If dtsSupplierGroupDetails.Rows.Count > 0 Then
                    For j As Integer = 0 To dtsSupplierGroupDetails.Rows.Count - 1
                        If dtsSupplierGroupDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
                            myDS.Tables(0).Rows(i)("sortorder") = 1
                            Exit For
                        End If
                    Next

                End If


            Next
        End If
        'End If


        Dim dataView As DataView = New DataView(myDS.Tables(0))
        dataView.Sort = "sortorder desc, partyname asc"
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

            Dim dtsSupplierGroupDetails As DataTable
            dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")
            'SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode='HOT' 
            Dim dt As New DataTable
            strSqlQry = "select cg.suppliergrpcode,cg.suppliergrpname,isnull(cg.active,0)active,cgd.partycode,(select c.partyname from partymast c where c.partycode=cgd.partycode and c.sptypecode='" & Request.QueryString("type") & "')Supplier  from suppliergroups cg , suppliergrps_detail cgd where cg.suppliergrpcode=cgd.suppliergrpcode and cg.suppliergrpname='" & (Trim(txtGroupTagName.Text.Trim)).Replace("(SG)", "") & "'  "
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    txtGroupCode.Text = dt.Rows(i)("suppliergrpcode").ToString

                    dtsSupplierGroupDetails.NewRow()
                    dtsSupplierGroupDetails.Rows.Add("SG", txtGroupTagName.Text, dt.Rows(i)("partycode").ToString, dt.Rows(i)("Supplier").ToString)


                    If dt.Rows(i)("active").ToString = "0" Then
                        chkActive.Checked = False
                    Else
                        chkActive.Checked = True
                    End If

                Next
                FillGridForEdit("partymast.partyname")

                If txtGroupTagName.Text.Contains("(SG)") Then
                    Dim dtt As DataTable
                    Dim iFlag As Integer = 0
                    dtt = Session("sDtDynamic")
                    If dtt.Rows.Count >= 0 Then
                        For i = 0 To dtt.Rows.Count - 1
                            If dtt.Rows(i)("Suppliers").ToString = txtGroupTagName.Text Then
                                iFlag = 1
                            End If
                        Next
                        If iFlag = 0 Then
                            dtt.NewRow()
                            dtt.Rows.Add("SG", txtGroupTagName.Text)
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
        '  lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        Dim sptype As String = ""

        If Request.QueryString("type") = "OTH" Then
            sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1501")
        Else
            sptype = Request.QueryString("type")
        End If

        strSqlQry = ""
        Try

            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode='" & sptype & "' "

            If txtGroupTagName.Text.Contains("(SG)") Then
                Dim strlbValue As String = ""
                strlbValue = (((((((Trim(txtGroupTagName.Text.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(H)", "")).Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select sgd.partycode   from suppliergroups sg,suppliergrps_detail sgd where sg.suppliergrpcode = sgd.suppliergrpcode and suppliergrpname=" & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select sgd.partycode   from suppliergroups sg,suppliergrps_detail sgd where  sg.suppliergrpcode =sgd.suppliergrpcode and suppliergrpname=" & Trim(strlbValue) & ")"
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
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
                lblMsg.Text = ""
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

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

        Dim dtsSupplierGroupDetails As DataTable
        dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")
        dtsSupplierGroupDetails.Rows.Clear()
        Session("sDtSupplierGroupDetails") = dtsSupplierGroupDetails
        gv_SearchResult.DataSource = dtsSupplierGroupDetails
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

            Dim dtsSupplierGroupDetails As New DataTable
            dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")

            'Dim dtsCountryGroupDetailsNew As New DataTable
            'dtsCountryGroupDetailsNew = Session("sDtHotelGroupDetails")
            'dtsCountryGroupDetailsNew.Clear()

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbCountry"), LinkButton)

            If dtsSupplierGroupDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsSupplierGroupDetails.Rows.Count - 1 To 0 Step i - 1
                    If lb.Text.Trim = dtsSupplierGroupDetails.Rows(i)("TypeName").ToString.Trim Then
                        dtsSupplierGroupDetails.Rows.Remove(dtsSupplierGroupDetails.Rows(i))
                    End If
                    ' dtsCountryGroupDetails.Rows(i).Delete()
                    dtsSupplierGroupDetails.AcceptChanges()
                Next
            End If
            Session("sDtSupplierGroupDetails") = dtsSupplierGroupDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                lblMsg.Visible = False
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    ' For j As Integer = 0 To dtDynamics.Rows.Count - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("Suppliers").ToString.Trim Then
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
            Dim dcGroupDetailsCountry = New DataColumn("Suppliers", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsTypeName)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsCountry)
            gv_SearchResult.DataSource = ClearDataTable
            gv_SearchResult.DataBind()

        Catch ex As Exception

        End Try


    End Sub
    ''' <summary>
    ''' btnView_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        lblHeading.Text = "View Supplier Group"
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
        Dim strSectorValue As String = ""
        Dim strCityValue As String = ""
        Dim strSuppGroupValue As String = ""
        Dim strCountryGroupValue As String = ""
        Dim strCategoryValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1

                If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                    If strSectorValue <> "" Then
                        strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                    If strCountryValue <> "" Then
                        strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CITY" Then
                    If strCityValue <> "" Then
                        strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CATEGORY" Then
                    If strCategoryValue <> "" Then
                        strCategoryValue = strCategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "SUPPLIERGROUP" Then
                    If strSuppGroupValue <> "" Then
                        strSuppGroupValue = strSuppGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strSuppGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            If CType(ViewState("appid"), String) = "10" Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS") Then
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,partymast.TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= (select option_selected from reservation_parameters where param_id='564') "
            ElseIf CType(ViewState("appid"), String) = "11" Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC") Then
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,partymast.TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= (select option_selected from reservation_parameters where param_id='1033') "
            ElseIf CType(ViewState("appid"), String) = "13" Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA") Then
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,partymast.TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= (select option_selected from reservation_parameters where param_id='1032') "
            ElseIf (CType(ViewState("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH") Then
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,partymast.TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= (select option_selected from reservation_parameters where param_id='1501') "


            End If



            If strCountryValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) IN (" & Trim(strCountryValue) & ")"

                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(strCountryValue) & ")"
                End If
            End If


            If strSectorValue <> "" Then

                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(sectormaster.sectorname) IN( " & Trim(strSectorValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue) & ")"
                End If
            End If

            If strCityValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(citymast.cityname) IN (" & Trim(strCityValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(strCityValue) & ")"
                End If
            End If

            If strCategoryValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(catmast.catname) IN (" & Trim(strCategoryValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(catmast.catname) IN (" & Trim(strCategoryValue) & ")"
                End If
            End If



            If strSuppGroupValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname IN (" & Trim(strSuppGroupValue) & "))"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname IN (" & Trim(strSuppGroupValue) & "))"
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
                            strWhereCond1 = " ( upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   (upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
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
            objUtils.WritErrorLog("SupplierGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally


        End Try

    End Sub





#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        '  FillGrid(Session("strsortexpression"), "")
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
        Dim strorderby As String = "partymast.partyname"
        Dim strsortorder As String = "ASC"
        Dim Type() As String
        Dim value() As String
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        hdFillByGrid.Value = "N"
        hdLinkButtonValue.Value = ""
        gv_SearchResult.Visible = True
        '  lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            lsProcessValue = lsProcessValue.ToUpper
            'strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode left outer JOIN hotelstatus on hotelstatus.hotelstatuscode=partymast.hotelstatuscode   where partymast.sptypecode='HOT' "
            If Request.QueryString("type") = "OTH" Then
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode='Other Serv' "
            Else
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode='" & Request.QueryString("type") & "' "
            End If


            Type = strType.Split(":")
            value = lsProcessValue.Split(":")
            If lsProcessValue.Trim <> "" Then
                lsProcessValue = (((((Trim(lsProcessValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(CTG)", "")
                For k = 0 To Type.GetUpperBound(0)
                    If Type(k) <> "T" Then
                        lsProcessValue = "'" & lsProcessValue & "'"
                    End If


                    If Type(k) = "CTY" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(ctrymast.ctryname) IN(" & Trim(value(k).Replace("(CTY)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(value(k).Replace("(CTY)", "")) & ")"
                        End If
                    End If

                    'If strType = "G" Then
                    '    If Trim(strWhereCond) = "" Then
                    '        strWhereCond = "  ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(lsProcessValue) & ")"
                    '    Else
                    '        strWhereCond = strWhereCond & " and ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(lsProcessValue) & ")"
                    '    End If
                    'End If

                    If Type(k) = "ST" Then

                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(sectormaster.sectorname) IN ( " & Trim(value(k).Replace("(ST)", "")) & ")"
                        Else

                            strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(value(k).Replace("(ST)", "")) & ")"
                        End If
                    End If
                    If Type(k) = "CT" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(citymast.cityname) IN (" & Trim(value(k).Replace("(CT)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(value(k).Replace("(CT)", "")) & ")"
                        End If
                    End If

                    If Type(k) = "CTG" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(catmast.catname) IN (" & Trim(value(k).Replace("(CTG)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(catmast.catname) IN (" & Trim(value(k).Replace("(CTG)", "")) & ")"
                        End If
                    End If

                    'If Type(k) = "H" Then
                    '    If Trim(strWhereCond) = "" Then
                    '        strWhereCond = " upper(partymast.partyname) IN (" & Trim(value(k).Replace("(H)", "")) & ")"
                    '    Else
                    '        strWhereCond = strWhereCond & " AND upper(partymast.partyname) IN (" & Trim(value(k).Replace("(H)", "")) & ")"
                    '    End If
                    'End If

                    If Type(k) = "SG" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname In (" & Trim(value(k).Replace("(SG)", "")) & "))"
                        Else
                            strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname IN (" & Trim(value(k).Replace("(SG)", "")) & "))"
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
                            'If strValue <> "" Then
                            '    If Trim(strWhereCond1) = "" Then
                            '        strWhereCond1 = " ( upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                            '    Else
                            '        strWhereCond1 = strWhereCond1 & " OR   (upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                            '    End If
                            'End If

                            If strValue <> "" Then
                                'If Trim(strWhereCond1) = "" Then
                                '    strWhereCond1 = " (upper(partymast.partyname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%' )) "
                                'Else
                                '    strWhereCond1 = strWhereCond1 & " AND (upper(partymast.partyname) 'LIKE %" & Trim(value(k).Replace("(T)", "")) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(value(k).Replace("(T)", "")) & "%' ))"
                                'End If
                                If Trim(strWhereCond1) = "" Then
                                    strWhereCond1 = " (upper(partymast.partyname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'   or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'    or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
                                Else
                                    strWhereCond1 = strWhereCond1 & " OR (upper(partymast.partyname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'   or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'    or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from suppliergroups cg,suppliergrps_detail cgd where cg.suppliergrpcode = cgd.suppliergrpcode and suppliergrpname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
                                End If

                            End If










                            'If strValue <> "" Then
                            '    If Trim(strWhereCond1) = "" Then
                            '        strWhereCond1 = " ( upper(partymast.partyname) LIKE %" & Trim(strValue.Trim.ToUpper) & "% or upper(ctrymast.ctryname) LIKE %" & Trim(strValue.Trim.ToUpper) & "% or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE %" & Trim(strValue.Trim.ToUpper) & "%)  or   upper(sectormaster.sectorname)  LIKE %" & Trim(strValue.Trim.ToUpper) & "%   or   upper(citymast.cityname) LIKE %" & Trim(strValue.Trim.ToUpper) & "%  or  upper(catmast.catname) LIKE %" & Trim(strValue.Trim.ToUpper) & "%  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE %" & Trim(strValue.Trim.ToUpper) & "% )) "
                            '    Else
                            '        strWhereCond1 = strWhereCond1 & " OR (upper(partymast.partyname) LIKE %" & Trim(strValue.Trim.ToUpper) & "% or upper(ctrymast.ctryname) LIKE %" & Trim(strValue.Trim.ToUpper) & "% or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE %" & Trim(strValue.Trim.ToUpper) & "%)  or   upper(sectormaster.sectorname)  LIKE %" & Trim(strValue.Trim.ToUpper) & "%   or   upper(citymast.cityname) LIKE %" & Trim(strValue.Trim.ToUpper) & "%  or  upper(catmast.catname) LIKE %" & Trim(strValue.Trim.ToUpper) & "%  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE %" & Trim(strValue.Trim.ToUpper) & "% ))"
                            '    End If
                            'End If
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
                FillCheckbox()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try
    End Sub








    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text '.Replace(": """, ":""")
        Dim lsProcessText As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessCity As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessCountryGroup As String = ""
        Dim lsProcessHotelGroup As String = ""
        Dim lsProcessCategory As String = ""
        Dim lsProcessHotels As String = ""
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
                    Case "COUNTRY"
                        lsProcessCity = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("COUNTRY", lsProcessCity, "CTY")
                    Case "CITY"
                        lsProcessCity = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CITY", lsProcessCity, "CT")
                    Case "SECTOR"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SECTOR", lsProcessCountry, "ST")
                    Case "CATEGORY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CATEGORY", lsProcessCountry, "CTG")
                    Case "SUPPLIERGROUP"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SUPPLIERGROUP", lsProcessCountry, "SG")

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
                    Case "CITY"
                        lsProcessCity = lsMainArr(i).Split(":")(1) & "(CT)"
                        txtNameNew.Text = lsProcessCity
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Suppliers").ToString = lsProcessCity Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CT", lsProcessCity)

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
                        ' FillGridByType("CT", lsProcessCity)
                    Case "SECTOR"
                        lsProcessSector = lsMainArr(i).Split(":")(1) & "(ST)"
                        txtNameNew.Text = lsProcessSector

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Suppliers").ToString = lsProcessSector Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("ST", lsProcessSector)
                            End If

                        End If
                        Session("sDtDynamic") = dtt

                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1) & "(T)"
                        txtNameNew.Text = lsProcessText

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Suppliers").ToString = lsProcessText Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("T", lsProcessText)
                            End If

                        End If
                        Session("sDtDynamic") = dtt
                        'FillGridByType("T", lsProcessText)

                        ' FillGridByType("S", lsProcessSector)
                    Case "CATEGORY"
                        lsProcessCategory = lsMainArr(i).Split(":")(1) & "(CTG)"
                        txtNameNew.Text = lsProcessCategory
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Suppliers").ToString = lsProcessCategory Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CTG", lsProcessCategory)
                            End If
                        End If
                        'If IsProcessType = "" Then
                        '    IsProcessType = "CTG"
                        'Else
                        '    IsProcessType = IsProcessType + "," + "CTG"
                        'End If
                        'If IsProcessValue = "" Then
                        '    IsProcessValue = lsProcessCategory
                        'Else
                        '    IsProcessValue = IsProcessValue + "," + lsProcessCategory
                        'End If
                        Session("sDtDynamic") = dtt
                        ' FillGridByType("CTG", lsProcessCategory)
                    Case "SUPPLIERGROUP"
                        lsProcessHotelGroup = lsMainArr(i).Split(":")(1) & "(SG)"
                        txtNameNew.Text = lsProcessHotelGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Suppliers").ToString = lsProcessHotelGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("SG", lsProcessHotelGroup)
                            End If

                        End If
                        Session("sDtDynamic") = dtt
                        'FillGridByType("HG", lsProcessHotelGroup)

                    Case "COUNTRY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1) & "(CTY)"
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Suppliers").ToString = lsProcessCountry Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CTY", lsProcessCountry)
                            End If

                        End If
                        Session("sDtDynamic") = dtt

                        '  FillGridByType("C", lsProcessCountry)

                End Select
            Next
            'Dim oldprocessvalue As String = ""
            'For i = 0 To dtt.Rows.Count - 1
            '    If IsProcessType <> "" Then
            '        If oldprocessvalue <> dtt.Rows(i).Item("Code") Then
            '            IsProcessType = IsProcessType + ":" + dtt.Rows(i).Item("Code")
            '            IsProcessValue = IsProcessValue + ":" + "'" + dtt.Rows(i).Item("Country") + "'"


            '        Else
            '            IsProcessType = IsProcessType
            '            IsProcessValue = IsProcessValue + "," + "'" + dtt.Rows(i).Item("Country") + "'"
            '        End If
            '    Else
            '        IsProcessType = dtt.Rows(i).Item("Code")
            '        IsProcessValue = "'" + dtt.Rows(i).Item("Country") + "'"
            '    End If
            '    oldprocessvalue = IsProcessType

            '    'If IsProcessValue <> "" Then

            '    'Else
            '    '    IsProcessValue = dtt.Rows(i).Item("Country")
            '    'End If
            'Next

            Dim arCode As New ArrayList
            For k = 0 To dtt.Rows.Count - 1
                If Not arCode.Contains(dtt.Rows(k).Item("Code")) Then
                    arCode.Add(dtt.Rows(k).Item("Code"))
                End If
            Next
            If arCode.Count > 0 Then
                For j = 0 To arCode.Count - 1
                    If IsProcessTypes <> "" Then
                        IsProcessTypes = IsProcessTypes & ":" & arCode(j).ToString
                    Else
                        IsProcessTypes = arCode(j).ToString
                    End If

                    For l = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(l)("Suppliers").ToString.Contains("(" & arCode(j).ToString.Trim & ")") Then
                            If IsProcessValue <> "" Then

                                IsProcessValue = IsProcessValue & ",'" & dtt.Rows(l)("Suppliers").ToString & "'"
                            Else
                                IsProcessValue = "'" & dtt.Rows(l)("Suppliers").ToString & "'"
                            End If

                        End If
                    Next
                    If IsProcessValues <> "" Then
                        IsProcessValues = IsProcessValues & ":" & IsProcessValue
                    Else
                        IsProcessValues = IsProcessValue
                    End If
                    IsProcessValue = ""
                Next
            End If

            FillGridByType(IsProcessTypes, IsProcessValues)
            If dtt.Rows.Count >= 0 Then
                Dim dtsSupplierGroupDetails As DataTable
                dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")
                If dtsSupplierGroupDetails.Rows.Count > 0 Then
                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Suppliers").ToString = "Suppliers" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("S", "Suppliers")
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
            Dim dtsSupplierGroupDetails As DataTable
            dtsSupplierGroupDetails = Session("sDtSupplierGroupDetails")
            dtsSupplierGroupDetails.Rows.Clear()
            Session("sDtSupplierGroupDetails") = dtsSupplierGroupDetails
            gv_SearchResult.DataSource = dtsSupplierGroupDetails
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
        Dim lblId As Label = CType(row.FindControl("lblSupplierCode"), Label)
        Session.Add("SupState", "Edit")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If (CType(ViewState("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"

        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"


        Else
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If

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
        Dim lblId As Label = CType(row.FindControl("lblSupplierCode"), Label)
        Session.Add("SupState", "View")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If (CType(ViewState("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"

        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"


        Else
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If

        '  strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"


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
        Dim lblId As Label = CType(row.FindControl("lblSupplierCode"), Label)
        Session.Add("SupState", "Delete")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If (CType(ViewState("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"

        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"


        Else
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If

        ' strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblSupplierCode"), Label)
        Session.Add("SupState", "Copy")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If (CType(ViewState("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"

        ElseIf (CType(ViewState("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC") Or (CType(ViewState("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC") Then
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&type=" + Request.QueryString("type") + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"


        Else
            strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If

        ' strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAddSupp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSupp.Click
        Session.Add("SupState", "New")
        Dim strpop As String = ""
        strpop = "window.open('../PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New&type=" + Request.QueryString("type") + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnHotelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSuppReport.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            strpop = "window.open('../PriceListModule/rptReportNew.aspx?suptype=" + CType(Request.QueryString("type"), String) + "&Pageame=Supplier&BackPageName=SupplierSearch.aspx','RepSupplier');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Suppliergroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvSearchGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchGrid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim chkCtry2 As CheckBox = e.Row.FindControl("chkCtry2")
            'Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            'chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)

            Dim lblSupplierName As Label = e.Row.FindControl("lblSupplierName")
            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblSectName")
            Dim lblSupplierGroup As Label = e.Row.FindControl("lblSupplierGroup")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextSupplierGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicSearch")
            If Session("sDtDynamicSearch") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""
                        If "COUNTRY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CATEGORY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CITY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SECTOR" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SUPPLIERGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSupplierGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCat
                            lsSearchTextSector = lsSearchTextCat
                            lsSearchTextSupplierGroup = lsSearchTextCat
                            lsSearchTextPartyName = lsSearchTextCat
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblCityName.Text = Regex.Replace(lblCityName.Text.Trim, lsSearchTextCtry.Trim(), _
                                      Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                                  RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextPartyName.Trim <> "" Then
                            lblSupplierName.Text = Regex.Replace(lblSupplierName.Text.Trim, lsSearchTextPartyName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblCityName.Text = Regex.Replace(lblCityName.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextSector.Trim <> "" Then
                            lblSectName.Text = Regex.Replace(lblSectName.Text.Trim, lsSearchTextSector.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCat.Trim <> "" Then
                            lblCatName.Text = Regex.Replace(lblCatName.Text.Trim, lsSearchTextCat.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextSupplierGroup.Trim <> "" Then
                            lblSupplierGroup.Text = Regex.Replace(lblSupplierGroup.Text.Trim, lsSearchTextSupplierGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If

    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim chkCtry2 As CheckBox = e.Row.FindControl("chkCtry2")
            'Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            'chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)

            Dim lblSupplierName As Label = e.Row.FindControl("lblSupplierName")
            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblSectName")
            Dim lblSupplierGroup As Label = e.Row.FindControl("lblSupplierGroup")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextSupplierGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""

                        If "CTG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("Suppliers").ToString.Trim.ToUpper
                            lsSearchTextCat = lsSearchTextCat.Replace("(CTG)", "")
                        End If
                        If "CTY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("Suppliers").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCity.Replace("(CTY)", "")
                        End If
                        If "ST" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("Suppliers").ToString.Trim.ToUpper
                            lsSearchTextSector = lsSearchTextSector.Replace("(ST)", "")
                        End If
                        If "SG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSupplierGroup = dtDynamics.Rows(j)("Suppliers").ToString.Trim.ToUpper
                            lsSearchTextSupplierGroup = lsSearchTextSupplierGroup.Replace("(SG)", "")
                        End If
                        If "T" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("Suppliers").ToString.Replace("(T)", "").Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCat
                            lsSearchTextSector = lsSearchTextCat
                            lsSearchTextSupplierGroup = lsSearchTextCat
                            lsSearchTextPartyName = lsSearchTextCat
                        End If

                        If lsSearchTextPartyName.Trim <> "" Then
                            lblSupplierName.Text = Regex.Replace(lblSupplierName.Text.Trim, lsSearchTextPartyName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblCityName.Text = Regex.Replace(lblCityName.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextSector.Trim <> "" Then
                            lblSectName.Text = Regex.Replace(lblSectName.Text.Trim, lsSearchTextSector.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCat.Trim <> "" Then
                            lblCatName.Text = Regex.Replace(lblCatName.Text.Trim, lsSearchTextCat.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextSupplierGroup.Trim <> "" Then
                            lblSupplierGroup.Text = Regex.Replace(lblSupplierGroup.Text.Trim, lsSearchTextSupplierGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If

        End If
    End Sub

   
    
    Protected Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        Dim strpop As String = ""
        strpop = "window.open('../AccountsModule/TransactionReports.aspx?printId=HotelGroups&suptype=" + CType(Request.QueryString("type"), String) + "&reportsType=excel','RepSupplier');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class
