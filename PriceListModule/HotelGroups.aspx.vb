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

Partial Class HotelGroups
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

        partycode = 1
        partyName = 2
        catname = 3
        cityname = 4
        sectorname = 5
        hotelgroup = 6
        Telephone = 7
        Contact = 8
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


             
                ViewState.Add("CitiesState", Request.QueryString("State"))
                ViewState.Add("CitiesRefCode", Request.QueryString("RefCode"))

                '   txtAutoComplete.Attributes.Add("onkeyup", "SetContextKey()")
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
                    lblHeading.Text = "Add Hotel Groups"
                    Page.Title = Page.Title + " " + "New "
                    btnSave.Text = "Save"
                    ' txtorder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from citymast") + 1
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save city?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf hdOPMode.Value = "E" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Hotel Group"
                    Page.Title = Page.Title + " " + "Edit Hotel Group"
                    btnSave.Text = "Update"



                ElseIf hdOPMode.Value = "D" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Hotel Group"
                    Page.Title = Page.Title + " " + "Delete Hotel Group"
                    btnSave.Text = "Delete"




                ElseIf hdOPMode.Value = "S" Then
                    lblHeading.Text = "Hotel Groups"
                    Page.Title = Page.Title + " " + "Hotel Groups"
                    ' btnSave.Text = "Delete"
                End If
                Dim HAppid As String = CType(Request.QueryString("appid"), String)
                Dim HAppname As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If HAppid Is Nothing = False Then
                    strappid = HAppid
                End If
                If HAppname Is Nothing = False Then

                    If HAppid = "4" Then

                        strappname = HAppname.Value
                    ElseIf HAppid = "14" Then

                        strappname = HAppname.Value
                    Else
                        strappname = HAppname.Value
                    End If
                End If
                Dim type As String = CType(Request.QueryString("type"), String)

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    If HAppid = "1" Then
                        objUser.CheckPUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                     CType(strappname, String), "PriceListModule\SupplierSearch.aspx", 25, btnAddHotel, Button1, _
                                                     btnHotelReport, gvSearchGrid, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.copy)

                        objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "PriceListModule\HotelGroups.aspx?appid=" + strappid, btnNew, btnEdit, _
                                             btnDelete, btnView, Button1, btnHotelReport)

                    Else
                       
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(strappname, String), "PriceListModule\SupplierSearch.aspx?appid=" + strappid + "&type=" + type, btnAddHotel, Button1, _
                                                   btnHotelReport, gvSearchGrid, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.copy)

                        objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                              CType(strappname, String), "PriceListModule\HotelGroups.aspx?appid=" + strappid + "&type=" + type, btnNew, btnEdit, _
                                              btnDelete, btnView, Button1, btnHotelReport)
                    End If

                End If


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    '' Create a Dynamic datatable ---- Start
                    Dim dtDynamic = New DataTable()
                    Dim dcCode = New DataColumn("Code", GetType(String))
                    Dim dcCountry = New DataColumn("Country", GetType(String))
                    dtDynamic.Columns.Add(dcCode)
                    dtDynamic.Columns.Add(dcCountry)
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
                    '--------end
                    Session("strsortexpression") = "partymast.partyname"
                    FillGridNew()
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("HotelChainMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
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

    Protected Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        Dim strpop As String = ""
        strpop = "window.open('../AccountsModule/TransactionReports.aspx?printId=HotelGroups&suptype=" + CType(Request.QueryString("type"), String) + "&reportsType=excel','RepSupplier');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
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

                    Dim dtsHotelGroupDetails As DataTable
                    dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
                    If dtsHotelGroupDetails.Rows.Count <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The hotels are not selected.');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If hdOPMode.Value = "N" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_HotelGroup", mySqlConn, sqlTrans)
                        frmmode = 1
                        lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='HOTELGROUP'")
                        optionval = objUtils.GetAutoDocNo("HOTELGROUP", mySqlConn, sqlTrans)
                        txtGroupCode.Text = optionval.Trim

                    ElseIf hdOPMode.Value = "E" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_HotelGroup", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If

                    Dim strBuffer As New Text.StringBuilder
                    If dtsHotelGroupDetails.Rows.Count > 0 Then

                        strBuffer.Append("<HotelGroups>")
                        For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                            strBuffer.Append("<HotelGroup>")
                            strBuffer.Append(" <HotelGroupCode>" & txtGroupCode.Text.Trim & "</HotelGroupCode>")
                            strBuffer.Append(" <PartyCode>" & dtsHotelGroupDetails.Rows(i)("Code").ToString & " </PartyCode>")
                            strBuffer.Append("</HotelGroup>")
                        Next
                        strBuffer.Append("</HotelGroups>")
                    End If



                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupcode", SqlDbType.VarChar, 20)).Value = CType(txtGroupCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@grouptagname", SqlDbType.VarChar, 100)).Value = CType((((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "")).Replace("(T)", "").Replace("(HG)", ""), String)
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
                    mySqlCmd = New SqlCommand("sp_Delete_HotelGroup", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupcode", SqlDbType.VarChar, 20)).Value = CType(txtGroupCode.Text.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If hdOPMode.Value = "N" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Hotel Group Created Successfully..');", True)
                ElseIf hdOPMode.Value = "E" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Hotel Group Modified Successfully..');", True)
                ElseIf hdOPMode.Value = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Hotel Group Deleted Successfully..');", True)
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
                lblHeading.Text = "Hotel Group"

                FillGridNew()

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("HotelGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "hotelgroup", "hotelgroupname", ((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf hdOPMode.Value = "E" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "hotelgroup", "hotelgroupcode", "hotelgroupname", CType(((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(R)", "")).Replace("(HG)", ""), String), txtGroupCode.Text.Trim) Then
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Hotels','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
                        If txtNameNew.Text.Contains("(S)") Then
                            dtsHotelGroupDetails.Rows.Add("S", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(G)") Then
                            dtsHotelGroupDetails.Rows.Add("G", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(C)") Then
                            dtsHotelGroupDetails.Rows.Add("C", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsHotelGroupDetails.Rows.Add("CT", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTG)") Then
                            dtsHotelGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(H)") Then
                            dtsHotelGroupDetails.Rows.Add("H", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HG)") Then
                            dtsHotelGroupDetails.Rows.Add("HG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HS)") Then
                            dtsHotelGroupDetails.Rows.Add("HS", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HC)") Then
                            dtsHotelGroupDetails.Rows.Add("HC", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
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
                        If dtt.Rows(j)("Country").ToString = "Hotels" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("H", "Hotels")
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
    Public Shared Function GetHotelGroup(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotelNames As New List(Of String)
        Try

            strSqlQry = "select rtrim(ltrim(hotelgroupname))+'(HG)' name from hotelgroup where  hotelgroupname like  " & "'" & prefixText & "%'  order by name"
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
                    hotelNames.Add(myDS.Tables(0).Rows(i)("name").ToString())
                Next

            End If

            Return hotelNames
        Catch ex As Exception
            Return hotelNames
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
        lblHeading.Text = "Add Hotel Group"

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
        lblHeading.Text = "Edit Hotel Group"


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
        lblHeading.Text = "Delete Hotel Group"
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
        lblHeading.Text = "Hotel Group"
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
            If strlbValue = "Hotels" Then
                strlbValue = "%"
            End If

            hdLinkButtonValue.Value = strlbValue
            Try
                FillGridByLinkButton()
                FillCheckbox()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("HotelGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                        If txtNameNew.Text.Contains("(S)") Then
                            dtsHotelGroupDetails.Rows.Add("S", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(G)") Then
                            dtsHotelGroupDetails.Rows.Add("G", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(C)") Then
                            dtsHotelGroupDetails.Rows.Add("C", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsHotelGroupDetails.Rows.Add("CT", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTG)") Then
                            dtsHotelGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(H)") Then
                            dtsHotelGroupDetails.Rows.Add("H", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HC)") Then
                            dtsHotelGroupDetails.Rows.Add("HC", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HS)") Then
                            dtsHotelGroupDetails.Rows.Add("HS", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)

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

                If dtsHotelGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "Hotels" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("H", "Hotels")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("HotelGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

        strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode  left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =partymast.hotelstatuscode  where partymast.sptypecode='HOT'"

        If strlbValue.Trim <> "" Then

            If strlbValue.Contains("(C)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"

                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                End If
            End If

            'If strlbValue.Contains("(G)") Then
            '    strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(H)", "")).Replace("(CTG)", "")
            '    strlbValue = "'" & strlbValue & "'"

            '    If Trim(strWhereCond) = "" Then
            '        strWhereCond = "  ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(strlbValue) & ")"
            '    Else
            '        strWhereCond = strWhereCond & " and ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(strlbValue) & ")"
            '    End If
            'End If

            If strlbValue.Contains("(S)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(sectormaster.sectorname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(CT)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(citymast.cityname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(citymast.cityname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(CTG)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(catmast.catname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(catmast.catname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(H)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(partymast.partyname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(partymast.partyname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(HG)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
                End If
            End If

            If strlbValue.Contains("(HC)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelchainmaster.hotelchainname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND upper(hotelchainmaster.hotelchainname) = " & Trim(strlbValue) & ""
                End If
            End If
            If strlbValue.Contains("(HS)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelstatus.hotelstatusname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(hotelstatus.hotelstatusname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(T)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")

                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strlbValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = " ( upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Trim.ToUpper) & "%' or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   (upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Trim.ToUpper) & "%'  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
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
                strValues = myDS.Tables(0).Rows(i)("partycode").ToString
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

            Dim dtsHotelGroupDetails As DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
            'SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode='HOT' 
            Dim dt As New DataTable
            strSqlQry = "select cg.hotelgroupcode,cg.hotelgroupname,isnull(cg.active,0)active,cgd.partycode,(select c.partyname from partymast c where c.partycode=cgd.partycode)Hotel  from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode=cgd.hotelgroupcode and cg.hotelgroupname='" & (Trim(txtGroupTagName.Text.Trim)).Replace("(HG)", "") & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    txtGroupCode.Text = dt.Rows(i)("hotelgroupcode").ToString

                    dtsHotelGroupDetails.NewRow()
                    dtsHotelGroupDetails.Rows.Add("HG", txtGroupTagName.Text, dt.Rows(i)("partycode").ToString, dt.Rows(i)("Hotel").ToString)


                    If dt.Rows(i)("active").ToString = "0" Then
                        chkActive.Checked = False
                    Else
                        chkActive.Checked = True
                    End If

                Next
                FillGridForEdit("partymast.partyname")

                If txtGroupTagName.Text.Contains("(HG)") Then
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
                            dtt.Rows.Add("HG", txtGroupTagName.Text)
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
        strSqlQry = ""
        Try

            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode='HOT' "

            If txtGroupTagName.Text.Contains("(HG)") Then
                Dim strlbValue As String = ""
                strlbValue = (((((((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(H)", "")).Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
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
    ''' <summary>
    ''' btnView_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        lblHeading.Text = "View Hotel Group"
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
        Dim strHotelGroupValue As String = ""
        Dim strCountryGroupValue As String = ""
        Dim strCategoryValue As String = ""
        Dim strHotelstatusValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strHotelchainValue As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELNAME" Then
                    If strCountryValue <> "" Then
                        strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "COUNTRYGROUP" Then
                    If strCountryGroupValue <> "" Then
                        strCountryGroupValue = strCountryGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                    If strSectorValue <> "" Then
                        strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
                If dtt.Rows(i)("Code").ToString = "HOTELGROUP" Then
                    If strHotelGroupValue <> "" Then
                        strHotelGroupValue = strHotelGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTELSTATUS" Then
                    If strHotelstatusValue <> "" Then
                        strHotelstatusValue = strHotelstatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelstatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"

                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTELCHAIN" Then
                    If strHotelchainValue <> "" Then
                        strHotelchainValue = strHotelchainValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelchainValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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

            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1, partymast.TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =partymast.hotelstatuscode  where partymast.sptypecode='HOT'"
            If strCountryValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) IN (" & Trim(strCountryValue) & ")"

                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(strCountryValue) & ")"
                End If
            End If

            'If strCountryGroupValue <> "" Then
            '    If Trim(strWhereCond) = "" Then
            '        strWhereCond = "  ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(strCountryGroupValue) & ")"
            '    Else
            '        strWhereCond = strWhereCond & " and ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(strCountryGroupValue) & ")"
            '    End If
            'End If

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

            If strHotelstatusValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelstatus.hotelstatusname) IN ( " & Trim(strHotelstatusValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(hotelstatus.hotelstatusname) IN (" & Trim(strHotelstatusValue) & ")"
                End If
            End If
            If strHotelchainValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelchainmaster.hotelchainname) IN ( " & Trim(strHotelchainValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(hotelchainmaster.hotelchainname) IN (" & Trim(strHotelchainValue) & ")"
                End If
            End If

            If strHotelGroupValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname IN (" & Trim(strHotelGroupValue) & "))"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname IN (" & Trim(strHotelGroupValue) & "))"
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
                            strWhereCond1 = " ( upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   (upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
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
            objUtils.WritErrorLog("HotelGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode left outer JOIN hotelstatus on hotelstatus.hotelstatuscode=partymast.hotelstatuscode   where partymast.sptypecode='HOT' "
            Type = strType.Split(":")
            value = lsProcessValue.Split(":")
            If lsProcessValue.Trim <> "" Then
                lsProcessValue = (((((((Trim(lsProcessValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                For k = 0 To Type.GetUpperBound(0)
                    If Type(k) <> "T" Then
                        lsProcessValue = "'" & lsProcessValue & "'"
                    End If


                    If Type(k) = "C" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(ctrymast.ctryname) IN(" & Trim(value(k).Replace("(C)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(value(k).Replace("(C)", "")) & ")"
                        End If
                    End If

                    'If strType = "G" Then
                    '    If Trim(strWhereCond) = "" Then
                    '        strWhereCond = "  ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(lsProcessValue) & ")"
                    '    Else
                    '        strWhereCond = strWhereCond & " and ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(lsProcessValue) & ")"
                    '    End If
                    'End If

                    If Type(k) = "S" Then

                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(sectormaster.sectorname) IN ( " & Trim(value(k).Replace("(S)", "")) & ")"
                        Else

                            strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(value(k).Replace("(S)", "")) & ")"
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

                    If Type(k) = "HG" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname In (" & Trim(value(k).Replace("(HG)", "")) & "))"
                        Else
                            strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname IN (" & Trim(value(k).Replace("(HG)", "")) & "))"
                        End If
                    End If
                    If Type(k) = "HS" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "upper(hotelstatus.hotelstatusname) IN (" & Trim(value(k).Replace("(HS)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(hotelstatus.hotelstatusname) IN (" & Trim(value(k).Replace("(HS)", "")) & ")"
                        End If
                    End If
                    If Type(k) = "HC" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "upper(hotelchainmaster.hotelchainname) IN (" & Trim(value(k).Replace("(HC)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(hotelchainmaster.hotelchainname) IN (" & Trim(value(k).Replace("(HC)", "")) & ")"
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
                                    strWhereCond1 = " (upper(partymast.partyname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'   or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Replace("(T)", "")) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
                                Else
                                    strWhereCond1 = strWhereCond1 & " OR (upper(partymast.partyname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'   or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Replace("(T)", "")) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
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
                            strWhereCond = strWhereCond & " OR (" & strWhereCond1 & ")"
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
            objUtils.WritErrorLog("HotelGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    Case "CITY"
                        lsProcessCity = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CITY", lsProcessCity, "CT")
                    Case "COUNTRYGROUP"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("COUNTRYGROUP", lsProcessCountryGroup, "G")
                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("TEXT", lsProcessText, "T")
                    Case "HOTELNAME"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("HOTELNAME", lsProcessCountry, "C")
                    Case "SECTOR"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SECTOR", lsProcessCountry, "S")
                    Case "CATEGORY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CATEGORY", lsProcessCountry, "CTG")
                    Case "HOTELGROUP"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("HOTELGROUP", lsProcessCountry, "HG")
                    Case "HOTELCHAIN"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("HOTELCHAIN", lsProcessCountry, "HC")
                    Case "HOTELSTATUS"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("HOTELSTATUS", lsProcessCountry, "HS")

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
                                If dtt.Rows(j)("Country").ToString = lsProcessCity Then
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
                    Case "COUNTRYGROUP"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1) & "(G)"
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
                                dtt.Rows.Add("G", lsProcessCountryGroup)
                            End If

                        End If

                        Session("sDtDynamic") = dtt
                    Case "HOTELSTATUS"
                        lsProcessStatus = lsMainArr(i).Split(":")(1) & "(HS)"
                        txtNameNew.Text = lsProcessStatus

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessStatus Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("HS", lsProcessStatus)
                            End If

                        End If
                        Session("sDtDynamic") = dtt
                    Case "HOTELCHAIN"
                        lsProcessChain = lsMainArr(i).Split(":")(1) & "(HC)"
                        txtNameNew.Text = lsProcessChain

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessChain Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("HC", lsProcessChain)
                            End If

                        End If

                        Session("sDtDynamic") = dtt
                        ' FillGridByType("CG", lsProcessCountryGroup)
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
                        'FillGridByType("T", lsProcessText)
                    Case "SECTOR"
                        lsProcessSector = lsMainArr(i).Split(":")(1) & "(S)"
                        txtNameNew.Text = lsProcessSector

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessSector Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("S", lsProcessSector)
                            End If

                        End If
                        Session("sDtDynamic") = dtt
                        ' FillGridByType("S", lsProcessSector)
                    Case "CATEGORY"
                        lsProcessCategory = lsMainArr(i).Split(":")(1) & "(CTG)"
                        txtNameNew.Text = lsProcessCategory
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCategory Then
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
                    Case "HOTELGROUP"
                        lsProcessHotelGroup = lsMainArr(i).Split(":")(1) & "(HG)"
                        txtNameNew.Text = lsProcessHotelGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessHotelGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("HG", lsProcessHotelGroup)
                            End If

                        End If
                        Session("sDtDynamic") = dtt
                        'FillGridByType("HG", lsProcessHotelGroup)

                    Case "COUNTRY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1) & "(C)"
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountry Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("C", lsProcessCountry)
                            End If

                        End If
                        Session("sDtDynamic") = dtt
                    Case "HOTELNAME"
                        lsProcessCountry = lsMainArr(i).Split(":")(1) & "(C)"
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountry Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("C", lsProcessCountry)
                            End If

                        End If
                        Session("sDtDynamic") = dtt

                        '  FillGridByType("C", lsProcessCountry)
                    Case "HOTELS"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = "Hotels" Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("H", "Hotels")
                            End If
                        End If
                        Session("sDtDynamic") = dtt
                        'FillGridByType("H", lsProcessCountry)
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
                        If dtt.Rows(l)("Country").ToString.Contains("(" & arCode(j).ToString.Trim & ")") Then
                            If IsProcessValue <> "" Then

                                IsProcessValue = IsProcessValue & ",'" & dtt.Rows(l)("Country").ToString & "'"
                            Else
                                IsProcessValue = "'" & dtt.Rows(l)("Country").ToString & "'"
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
            FillCheckbox()
            If dtt.Rows.Count >= 0 Then
                Dim dtsHotelGroupDetails As DataTable
                dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
                If dtsHotelGroupDetails.Rows.Count > 0 Then
                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "Hotels" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("H", "Hotels")
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
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        If CType(Request.QueryString("appid"), String) <> "1" Then
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + Request.QueryString("type") + "','Suppliers');"
        Else
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
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
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "View")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If CType(Request.QueryString("appid"), String) <> "1" Then
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + Request.QueryString("type") + "','Suppliers');"
        Else
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
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
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        '  strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        If CType(Request.QueryString("appid"), String) <> "1" Then
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + Request.QueryString("type") + "','Suppliers');"
        Else
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If
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
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "Copy")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Session.Add("partycode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        If CType(Request.QueryString("appid"), String) <> "1" Then
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "&type=" + Request.QueryString("type") + "','Suppliers');"
        Else
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If

        ' strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
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

        If CType(Request.QueryString("appid"), String) <> "1" Then
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New&type=" + Request.QueryString("type") + "','Suppliers');"

        Else
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New','Suppliers');"
        End If

        ' strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New','Suppliers');"
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
            strpop = "window.open('rptReportNew.aspx?Pageame=HotelGroup&BackPageName=HotelGroups.aspx&hotelgroupcode=" + txtGroupCode.Text.Trim + "&hotelgroupname=" + txtName.Text.Trim + "','rpthotelgroup');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Hotelgroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            Dim lblPartyName As Label = e.Row.FindControl("lblPartyName")
            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblSectName")
            Dim lblHotelGroup As Label = e.Row.FindControl("lblHotelGroup")

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

                        If "CATEGORY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CITY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SECTOR" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "HOTELGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextHotelGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCat
                            lsSearchTextSector = lsSearchTextCat
                            lsSearchTextHotelGroup = lsSearchTextCat
                            lsSearchTextPartyName = lsSearchTextCat
                        End If

                        If lsSearchTextPartyName.Trim <> "" Then
                            lblPartyName.Text = Regex.Replace(lblPartyName.Text.Trim, lsSearchTextPartyName.Trim(), _
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
                        If lsSearchTextHotelGroup.Trim <> "" Then
                            lblHotelGroup.Text = Regex.Replace(lblHotelGroup.Text.Trim, lsSearchTextHotelGroup.Trim(), _
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

            Dim lblPartyName As Label = e.Row.FindControl("lblPartyName")
            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblSectName")
            Dim lblHotelGroup As Label = e.Row.FindControl("lblHotelGroup")

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

                        If "CTG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lsSearchTextCat = lsSearchTextCat.Replace("(CTG)", "")
                        End If
                        If "CTY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCity.Replace("(CTY)", "")
                        End If
                        If "S" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lsSearchTextSector = lsSearchTextSector.Replace("(S)", "")
                        End If
                        If "HG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextHotelGroup = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                            lsSearchTextHotelGroup = lsSearchTextHotelGroup.Replace("(HG)", "")
                        End If
                        If "T" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("Country").ToString.Replace("(T)", "").Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCat
                            lsSearchTextSector = lsSearchTextCat
                            lsSearchTextHotelGroup = lsSearchTextCat
                            lsSearchTextPartyName = lsSearchTextCat
                        End If

                        If lsSearchTextPartyName.Trim <> "" Then
                            lblPartyName.Text = Regex.Replace(lblPartyName.Text.Trim, lsSearchTextPartyName.Trim(), _
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
                        If lsSearchTextHotelGroup.Trim <> "" Then
                            lblHotelGroup.Text = Regex.Replace(lblHotelGroup.Text.Trim, lsSearchTextHotelGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If

        End If
    End Sub
End Class
