
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Partial Class PriceListModule_Default
    Inherits System.Web.UI.Page


#Region "Enum GridCol"
    Enum GridCol
        code = 0
        agentcode = 1
        agentName = 2
        ctryname = 3
        cityname = 4
        agentcatname = 5
        sectorname = 6
        countrygroups = 7
        customergroup = 8
        Edit = 10
        View = 11
        Delete = 12
    End Enum
#End Region
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Dim objUser As New clsUser
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
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
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

                btnNew.Enabled = True
                txtName.ReadOnly = True
                txtGroupCode.Enabled = False
                txtGroupTagName.ReadOnly = True
                txtGroupCode.ReadOnly = True
                btnEdit.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
                trNameAndCode.Visible = False


                If hdOPMode.Value = "N" Then
                    lblHeading.Text = "Add Customer Group"
                    Page.Title = Page.Title + " " + "New "
                    btnSave.Text = "Save"
                    ' txtorder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from citymast") + 1
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save city?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf hdOPMode.Value = "E" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Customer Group"
                    Page.Title = Page.Title + " " + "Edit Customer Group"
                    btnSave.Text = "Update"



                ElseIf hdOPMode.Value = "D" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Customer Group"
                    Page.Title = Page.Title + " " + "Delete Customer Group"
                    btnSave.Text = "Delete"


                    'ShowRecord(CType(ViewState("CitiesRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete city?')==false)return false;")
                ElseIf hdOPMode.Value = "S" Then
                    lblHeading.Text = "Customer Group"
                    Page.Title = Page.Title + " " + "Customer Group"
                    ' btnSave.Text = "Delete"
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")





                Dim CgAppname As String = ""
                Dim CgAppid As String = CType(Request.QueryString("appid"), String)

                CgAppname = objUser.GetAppName(Session("dbconnectionName"), CgAppid)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If CgAppid Is Nothing = False Then
                    strappid = CgAppid
                End If

                If CgAppname Is Nothing = False Then
                    strappname = CgAppname
                End If


                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else

                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                     CType(strappname, String), "PriceListModule\CustomerGroupSearch.aspx?appid=" + strappid, btnAddHotel, btnExportToExcel, btnHotelReport, gvSearchGrid, GridCol.Edit, GridCol.Delete, GridCol.View)

                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                               CType(strappname, String), "PriceListModule\CustomerGroupSearch.aspx?appid=" + strappid, btnNew, btnEdit, _
                                               btnDelete, btnView, btnExportToExcel, btnHotelReport)


                  

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
                    Dim dtCountryGroupDetails = New DataTable()
                    Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                    Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
                    Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                    Dim dcGroupDetailsCountry = New DataColumn("Country", GetType(String))
                    dtCountryGroupDetails.Columns.Add(dcGroupDetailsType)
                    dtCountryGroupDetails.Columns.Add(dcGroupDetailsTypeName)
                    dtCountryGroupDetails.Columns.Add(dcGroupDetailsCode)
                    dtCountryGroupDetails.Columns.Add(dcGroupDetailsCountry)
                    Session("sDtCountryGroupDetails") = dtCountryGroupDetails
                    '--------end
                    Session("strsortexpression") = "agentmast.agentname"
                    FillGridNew()
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomerGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        Page.Title = "Customer Group"
        ' btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CustomersWindowPostBack") Then
            btnResetSelection_Click(sender, e)
        End If
    End Sub
#End Region
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
   
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gvSearchGrid.Rows.Count <> 0 Then



                strSqlQry = hdSQL.Value
                strSqlQry = "Select '''' + agentmast.agentcode Code, agentmast.agentname Name,contact1 Reservation, isnull(dbo.fn_get_countrygroup(ctrymast.ctrycode),'') [Market], ctrymast.ctrycode [CountryCode], ctrymast.ctryname [CountryName],citymast.cityname [CityName],agentmast.tel1 [Telephone], mobileno Mobile,email [EmailId], case when agentmast.active=1 then 'Yes' else 'No' end Active, '''' + isnull(agentmast.TRNNo,'') TRNNo " + strSqlQry.Substring(strSqlQry.IndexOf("from agentmast"))


                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "catmast")
                objUtils.ExportToExcelnew(DS, Response, "CustomersList")
                con.Close()
                'Session("ExcelExportSqlQuery") = strSqlQry
                'Dim strpop As String = ""
                'strpop = "window.open('../Accounts/ExcelExportExc.aspx','Supcats');"
                'strpop = "window.open('../AccountsModule/TransactionReports.aspx?printId=CustGrps&reportsType=excel','RepCustomer');"

                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


                'con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                'DA = New SqlDataAdapter(strSqlQry, con)

                'DA.Fill(DS, "PreferredSupliers123")




                'If DS.Tables(0).Rows.Count > 0 Then

                '    objUtils.ExportToExcelnew(DS, Response, "Customers")
                '    con.Close()
                'Else
                '    objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
                'End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
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
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Try

            If Page.IsValid = True Then
                Dim dtsCountryGroupDetails As DataTable
                dtsCountryGroupDetails = Session("sDtCountryGroupDetails")
                Dim strBuffer As New Text.StringBuilder
                If dtsCountryGroupDetails.Rows.Count > 0 Then
                    strBuffer.Append("<CustomerGroups>")
                    For i = 0 To dtsCountryGroupDetails.Rows.Count - 1
                        strBuffer.Append("<CustomerGroup>")
                        strBuffer.Append(" <customergroupcode>" & txtGroupCode.Text.Trim & "</customergroupcode>")
                        strBuffer.Append(" <agentcode>" & dtsCountryGroupDetails.Rows(i)("Code").ToString.Replace("&", "_") & " </agentcode>")
                        strBuffer.Append("</CustomerGroup>")
                    Next
                    strBuffer.Append("</CustomerGroups>")
                End If
                If hdOPMode.Value = "N" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                End If
                If hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    'If checkForDate() = False Then
                    '    Exit Sub
                    'End If


                    If txtGroupTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Customer Group.');", True)
                        Exit Sub
                    End If


                    If dtsCountryGroupDetails.Rows.Count <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Customer are not selected.');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If hdOPMode.Value = "N" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_CustomerGroup", mySqlConn, sqlTrans)
                        frmmode = 1
                        lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='CUSTGROUP'")
                        optionval = objUtils.GetAutoDocNo("CUSTGROUP", mySqlConn, sqlTrans)
                        txtGroupCode.Text = optionval.Trim

                    ElseIf hdOPMode.Value = "E" Then
                        mySqlCmd = New SqlCommand("sp_Add_Mod_CustomerGroup", mySqlConn, sqlTrans)
                        frmmode = 2
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
                    mySqlCmd = New SqlCommand("sp_Del_customergroup", mySqlConn, sqlTrans)
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
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Customer  Group Created Successfully..');", True)
                ElseIf hdOPMode.Value = "E" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Customer  Group Updated Successfully..');", True)
                ElseIf hdOPMode.Value = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Customer  Group Deleted Successfully..');", True)
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
                lblHeading.Text = "Customer Group"

                FillGridNew()
                btnSave.Visible = False

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("CustomerGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region




#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CitiesSearch.aspx", False)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
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
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "customergroup", "customergroupname", ((Trim(txtGroupTagName.Text.Trim).Replace("(CU)", "")).Replace("(CT)", "")).Replace("(CG)", "").Replace("(CTG)", "").Replace("(S)", "").Replace("(CS)", "").Replace("(C)", "")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer Group is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf hdOPMode.Value = "E" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "customergroup", "customergroupname", "customergroupcode", txtGroupCode.Text.Trim, CType(((Trim(txtGroupTagName.Text.Trim).Replace("(CU)", "")).Replace("(CT)", "")).Replace("(CT)", "").Replace("(CTG)", "").Replace("(S)", "").Replace("(CS)", "").Replace("(C)", ""), String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer Group is already present.');", True)
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
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Customer Group','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSelectAll"), CheckBox)
        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        Dim dtsCountryGroupDetails As DataTable
        dtsCountryGroupDetails = Session("sDtCountryGroupDetails")

        For Each row In gv_SearchResult.Rows
            Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            Dim lblCountryCode As Label = CType(row.FindControl("lblcntryCode"), Label)
            Dim lblCountryName As Label = CType(row.FindControl("lblCountryName"), Label)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True

                If dtsCountryGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsCountryGroupDetails.Rows.Count - 1
                        If dtsCountryGroupDetails.Rows(i)("Code").ToString = lblCountryCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsCountryGroupDetails.NewRow()

                        If txtNameNew.Text.Contains("(CU)") Then
                            dtsCountryGroupDetails.Rows.Add("CU", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                        ElseIf txtNameNew.Text.Contains("(CS)") Then
                            dtsCountryGroupDetails.Rows.Add("CS", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsCountryGroupDetails.Rows.Add("CT", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                        ElseIf txtNameNew.Text.Contains("(S)") Then
                            dtsCountryGroupDetails.Rows.Add("S", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTG)") Then
                            dtsCountryGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                        ElseIf txtNameNew.Text.Contains("(CG)") Then
                            dtsCountryGroupDetails.Rows.Add("CG", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                        ElseIf txtNameNew.Text.Contains("(C)") Then
                            dtsCountryGroupDetails.Rows.Add("C", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)


                        End If
                        Session("sDtCountryGroupDetails") = dtsCountryGroupDetails

                    End If

                End If

            Else
                ChkBoxRows.Checked = False
                If dtsCountryGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsCountryGroupDetails.Rows.Count - 1
                        If dtsCountryGroupDetails.Rows(i)("Code").ToString = lblCountryCode.Text Then
                            dtsCountryGroupDetails.Rows.Remove(dtsCountryGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtCountryGroupDetails") = dtsCountryGroupDetails
            End If

            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsCountryGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "Customer" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("CU", "Customer")
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
    ''' GetCountryGroup
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getcustomergroup(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim holidayname As New List(Of String)
        Try

            strSqlQry = "select customergroupname from customergroup where customergroupname like  " & "'" & prefixText & "%'  order by customergroupname"
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
                    holidayname.Add(myDS.Tables(0).Rows(i)("customergroupname").ToString())
                Next

            End If

            Return holidayname
        Catch ex As Exception
            Return holidayname
        End Try

    End Function

    ''' <summary>
    ''' GetCountries
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountries(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim counTryNames As New List(Of String)
        Try

            strSqlQry = "select Name from view_countryregiongroup where name like  " & "'" & prefixText & "%'  order by name"
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
                    counTryNames.Add(myDS.Tables(0).Rows(i)("name").ToString())
                Next

            End If

            Return counTryNames
        Catch ex As Exception
            Return counTryNames
        End Try

    End Function


    ''' <summary>
    ''' txtName_TextChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        hdFillByGrid.Value = "N"
        hdLinkButtonValue.Value = ""
        txtNameNew.Text = txtName.Text
        FillGrid("agentmast.agentname")
        If txtName.Text.Contains("(CTG)") Then
            Dim iFlag As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")

            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Country").ToString = txtName.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("CTG", txtName.Text)
                    Session("sDtDynamic") = dtt
                    dlList.DataSource = dtt
                    dlList.DataBind()
                End If

            End If


        ElseIf txtName.Text.Contains("(CT)") Then
            Dim dtt As DataTable
            Dim iFlag As Integer = 0
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Country").ToString = txtName.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("CT", txtName.Text)
                    Session("sDtDynamic") = dtt
                    dlList.DataSource = dtt
                    dlList.DataBind()
                End If

            End If
        ElseIf txtName.Text.Contains("(S)") Then
            Dim dtt As DataTable
            Dim iFlag As Integer = 0
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Country").ToString = txtName.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("S", txtName.Text)
                    Session("sDtDynamic") = dtt
                    dlList.DataSource = dtt
                    dlList.DataBind()
                End If

            End If
        ElseIf txtName.Text.Contains("(CU)") Then
            Dim dtt As DataTable
            Dim iFlag As Integer = 0
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Country").ToString = txtName.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("CU", txtName.Text)
                    Session("sDtDynamic") = dtt
                    dlList.DataSource = dtt
                    dlList.DataBind()
                End If

            End If
        ElseIf txtName.Text.Contains("(CG)") Then
            Dim dtt As DataTable
            Dim iFlag As Integer = 0
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Country").ToString = txtName.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("CG", txtName.Text)
                    Session("sDtDynamic") = dtt
                    dlList.DataSource = dtt
                    dlList.DataBind()
                End If

            End If
        ElseIf txtName.Text.Contains("(CS)") Then
            Dim dtt As DataTable
            Dim iFlag As Integer = 0
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Country").ToString = txtName.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("CS", txtName.Text)
                    Session("sDtDynamic") = dtt
                    dlList.DataSource = dtt
                    dlList.DataBind()
                End If

            End If


        ElseIf txtName.Text.Contains("(C)") Then
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            Dim iFlag As Integer = 0
            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Country").ToString = txtName.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("C", txtName.Text)
                    Session("sDtDynamic") = dtt
                    dlList.DataSource = dtt
                    dlList.DataBind()
                End If

            End If

        End If

        txtName.Text = ""
    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    ''' <summary>
    ''' FillGrid
    ''' </summary>
    ''' <param name="strorderby"></param>
    ''' <param name="strsortorder"></param>
    ''' <remarks></remarks>
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
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

            strSqlQry = "select agentmast.agentcode , agentmast.agentname,agentcatmast.agentcatname,ctrymast.ctryname,citymast.cityname,agent_sectormaster.sectorname,isnull(dbo. fn_get_customergroup(agentmast.agentcodecode),'')CustomerGroup,isnull(dbo.fn_get_countrygroup(ctrymast.ctrycode),'') countrygroups" & _
                        " ,0 as sortorder from agentmast  inner join ctrymast where agentmast.ctrycode=ctrymast.ctrycode inner join agentcatmast where agentmast.catcode=agentcatmast.catcode left outer join agent_sectormaster where agentmast.sectorcode=agent_sectormaster.sectorcode inner join citymast where citymast.citycode=agentmast.citycode "


            If txtNameNew.Text.Trim <> "" Then

                If txtNameNew.Text.Contains("(C)") Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(ctrymast.ctryname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    End If
                ElseIf txtNameNew.Text.Contains("(CT)") Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(citymast.ctryname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND upper(citymast.ctryname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    End If
                ElseIf txtNameNew.Text.Contains("(S)") Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(agent_sectormaster.sectorname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    End If
                ElseIf txtNameNew.Text.Contains("(CTG)") Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(agentcatmast.agentcatname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    End If
                ElseIf txtNameNew.Text.Contains("(CS)") Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " upper(agentmast.agentname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CS)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    Else

                        strWhereCond = strWhereCond & " AND upper(agentmast.agentname) LIKE '" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "").Replace("(C)", "").Replace("(CT)", "").Replace("(CS)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"
                    End If
                ElseIf txtNameNew.Text.Contains("(CU)") Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " agentmast.agentcode in (select customergroup_detail.ctrycode   from customergroup,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname='" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(CTG)", "")).Replace("(CT)", "").Replace("(S)", "").Replace("(CU)", "").Replace("(CS)", "").Replace("(CG)", "") & "')"
                    Else
                        strWhereCond = strWhereCond & " and  agentmast.ctrycode in (select customergroup_detail.ctrycode   from customergroup,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname='" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CU)", "").Replace("(CTG)", "").Replace("(S)", "").Replace("(CS)", "").Replace("(CG)", "") & "')"
                    End If
                ElseIf txtNameNew.Text.Contains("(CG)") Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname='" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(CTG)", "")).Replace("(CT)", "").Replace("(S)", "").Replace("(CU)", "").Replace("(CS)", "").Replace("(CG)", "") & "')"
                    Else
                        strWhereCond = strWhereCond & " and ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname='" & ((Trim(txtNameNew.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(CTG)", "")).Replace("(CT)", "").Replace("(S)", "").Replace("(CU)", "").Replace("(CS)", "").Replace("(CG)", "") & "')"
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
            objUtils.WritErrorLog("CustomerGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try

    End Sub
#End Region
    ''' <summary>
    ''' btnNew_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ' btnCancel.Attributes.Add("class", "btnExampleHold")
        hdOPMode.Value = "N"
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
        lblHeading.Text = "Add Customer Group"

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
        lblHeading.Text = "Edit Customer Group"

        'txtFromDate.value = Select fromdate from holidaycalendar where holidayname=

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
        lblHeading.Text = "Delete Customer Group"
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
        Dim dtsCountryGroupDetails As DataTable
        dtsCountryGroupDetails = Session("sDtCountryGroupDetails")
        dtsCountryGroupDetails.Rows.Clear()
        Session("sDtCountryGroupDetails") = dtsCountryGroupDetails
        gv_SearchResult.DataSource = dtsCountryGroupDetails
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
        lblHeading.Text = "Customer Group"
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
            If strlbValue = "Customer" Then
                strlbValue = "%"
            End If

            hdLinkButtonValue.Value = strlbValue
            Try
                FillGridByLinkButton()
                FillCheckbox()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomerGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        Dim dtsCountryGroupDetails As DataTable
        dtsCountryGroupDetails = Session("sDtCountryGroupDetails")

        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        If dtsCountryGroupDetails.Rows.Count > 0 Then

            For Each row In gv_SearchResult.Rows

                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                Dim lblCountryCode As Label = CType(row.FindControl("lblcntryCode"), Label)
                Dim lblCountryName As Label = CType(row.FindControl("lblCountryName"), Label)


                For i As Integer = 0 To dtsCountryGroupDetails.Rows.Count - 1
                    If dtsCountryGroupDetails.Rows(i)("Code").ToString.Trim = lblCountryCode.Text.Trim Then
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


        Dim ChkBoxRows As CheckBox = CType(sender, CheckBox)
        Dim lblCountryCode As Label = CType(ChkBoxRows.FindControl("lblcntryCode"), Label)
        Dim lblCountryName As Label = CType(ChkBoxRows.FindControl("lblCountryName"), Label)
        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        Dim iFlagCheckedAll As Integer = 0
        Dim iFlagUnCheckedAll As Integer = 0
        Dim dtsCountryGroupDetails As DataTable
        dtsCountryGroupDetails = Session("sDtCountryGroupDetails")

        If ChkBoxRows.Checked = True Then
            ChkBoxRows.Checked = True



            If dtsCountryGroupDetails.Rows.Count >= 0 Then
                For i = 0 To dtsCountryGroupDetails.Rows.Count - 1
                    If dtsCountryGroupDetails.Rows(i)("Code").ToString = lblCountryCode.Text Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtsCountryGroupDetails.NewRow()

                    If txtNameNew.Text.Contains("(CTG)") Then
                        dtsCountryGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                    ElseIf txtNameNew.Text.Contains("(S)") Then
                        dtsCountryGroupDetails.Rows.Add("S", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                    ElseIf txtNameNew.Text.Contains("(CS)") Then
                        dtsCountryGroupDetails.Rows.Add("CS", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                    ElseIf txtNameNew.Text.Contains("(CU)") Then
                        dtsCountryGroupDetails.Rows.Add("CU", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                    ElseIf txtNameNew.Text.Contains("(CG)") Then
                        dtsCountryGroupDetails.Rows.Add("CG", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                    ElseIf txtNameNew.Text.Contains("(C)") Then
                        dtsCountryGroupDetails.Rows.Add("C", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                    ElseIf txtNameNew.Text.Contains("(CT)") Then
                        dtsCountryGroupDetails.Rows.Add("CT", txtNameNew.Text, lblCountryCode.Text, lblCountryName.Text)
                    End If

                    Session("sDtCountryGroupDetails") = dtsCountryGroupDetails
                End If

            End If

        Else

            ChkBoxRows.Checked = False
            If dtsCountryGroupDetails.Rows.Count >= 0 Then
                For i = 0 To dtsCountryGroupDetails.Rows.Count - 1
                    If dtsCountryGroupDetails.Rows(i)("Code").Trim.ToString = lblCountryCode.Text.Trim Then
                        dtsCountryGroupDetails.Rows.Remove(dtsCountryGroupDetails.Rows(i))
                        Exit For
                    End If
                Next

            End If
            Session("sDtCountryGroupDetails") = dtsCountryGroupDetails
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

            If dtsCountryGroupDetails.Rows.Count > 0 Then

                For j = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(j)("Country").ToString = "Customer" Then
                        iFlagS = 1
                    End If
                Next
                If iFlagS = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add("CU", "Customer")
                    Session("sDtDynamic") = dtt
                End If

            End If
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()


    End Sub
    ''' <summary>
    ''' FillGridByLinkButton
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillGridByLinkButton()
        Dim strorderby As String = "agentmast.agentname"
        Dim strsortorder As String = "ASC"
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True

        Dim strlbValue As String = hdLinkButtonValue.Value
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""


        strSqlQry = " select agentmast.agentcode , agentmast.agentname,agentcatmast.agentcatname,ctrymast.ctryname,citymast.cityname,agent_sectormaster.sectorname,isnull(dbo. fn_get_customergroup(agentmast.agentcode),'')Customergroup,isnull(dbo.fn_get_countrygroup(ctrymast.ctrycode),'')countrygroups,0 as sortorder from agentmast  inner  join ctrymast on agentmast.ctrycode=ctrymast.ctrycode inner join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode left outer join agent_sectormaster on agentmast.sectorcode=agent_sectormaster.sectorcode inner join citymast on citymast.citycode=agentmast.citycode"


        If strlbValue.Trim <> "" Then

        End If




        If strlbValue.Contains("(CTG)") Then




            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentcatmast.agentcatname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "%'"
            Else

                strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(S)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "%'"
            End If
        End If

        If strlbValue.Contains("(CS)") Then


            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.agentname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "%'"
            Else

                strWhereCond = strWhereCond & " AND upper(agentmast.agentname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(S)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "%'"
            End If
        End If

        If strlbValue.Contains("(CT)") Then




            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(citymast.cityname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CTG)", "").Replace("(CS)", "") & "%'"
            Else

                strWhereCond = strWhereCond & " AND upper(citymast.cityname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(S)", "")).Replace("(CT)", "")).Replace("(CTG)", "").Replace("(CS)", "") & "%'"
            End If
        End If
        If strlbValue.Contains("(C)") Then

            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ctrymast.ctryname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CTG)", "").Replace("(CTG)", "").Replace("(CS)", "") & "%'"

                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CTG)", "").Replace("(CS)", "") & "%'"
            End If
        End If

        If strlbValue.Contains("(S)") Then


            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(agent_sectormaster.sectorname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CTG)", "").Replace("(CS)", "") & "%'"
            Else

                strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("CG)", "")).Replace("(S)", "")).Replace("(CT)", "")).Replace("(CS)", "") & "%'"
            End If
        End If
        If strlbValue.Contains("(CG)") Then



            If Trim(strWhereCond) = "" Then
                strWhereCond = "  ctrymast.ctrycode in (select countrygroup_detail.ctrycode from countrygroup ,countrygroup_detail countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname ='" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "')"
            Else
                strWhereCond = strWhereCond & " and ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname ='" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "")).Replace("(S)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "')"
            End If
        End If
        If strlbValue.Contains("(CU)") Then


            If Trim(strWhereCond) = "" Then
                strWhereCond = "agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "')"
            Else

                strWhereCond = strWhereCond & "and  agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,coustomergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname like '" & (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "")).Replace("(C)", "")).Replace("(CT)", "")).Replace("(CS)", "").Replace("(CTG)", "") & "')"
            End If
        End If

        If strlbValue.Contains("(T)") Then
            strlbValue = (((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "")).Replace("(S)", "")).Replace("(CT)", "")).Replace("(CTG)", "").Replace("(CS)", "")

            Dim lsMainArr As String()
            Dim strValue As String
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(((((Trim(strlbValue.Trim.ToUpper).Replace("(CU)", "")).Replace("(S)", "")).Replace("(C)", "")).Replace("(CG)", "")).Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "").Replace("(T)", ""), ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then
                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = "(upper(agent_sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentcatmast.agentcatname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or ( agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') or   upper(ctrymast.ctryname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')) "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(agent_sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentcatmast.agentcatname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or ( agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   upper(ctrymast.ctryname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')) "
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

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))      'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS)
        gv_SearchResult.DataBind()
        ' If Not strlbValue.Contains("(R)") And Not strlbValue.Contains("(G)") Then
        Dim dtsCountryGroupDetails As DataTable
        Dim strValues As String = ""
        Dim strQuery As String = ""
        dtsCountryGroupDetails = Session("sDtCountryGroupDetails")

        If myDS.Tables(0).Rows.Count > 0 Then

            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                strValues = ""
                strValues = myDS.Tables(0).Rows(i)("agentcode").ToString
                If dtsCountryGroupDetails.Rows.Count > 0 Then
                    For j As Integer = 0 To dtsCountryGroupDetails.Rows.Count - 1
                        If dtsCountryGroupDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
                            myDS.Tables(0).Rows(i)("sortorder") = 1
                            Exit For
                        End If
                    Next

                End If


            Next
        End If
        'End If
        FillCheckbox()

        Dim dataView As DataView = New DataView(myDS.Tables(0))
        dataView.Sort = "sortorder desc, agentname asc"
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

            Dim dtsCountryGroupDetails As DataTable
            dtsCountryGroupDetails = Session("sDtCountryGroupDetails")

            Dim dt As New DataTable
            strSqlQry = "select customergroup.customergroupcode,customergroup.customergroupname,isnull(customergroup.active,0)active,customergroup_detail.agentcode,(select agentmast.agentname from agentmast  where agentmast.agentcode=customergroup_detail.agentcode)agentname  from customergroup,customergroup_detail where customergroup.customergroupcode=customergroup_detail.customergroupcode and customergroup.customergroupname ='" & ((Trim(txtGroupTagName.Text.Trim).Replace("(CU)", "")).Replace("(CT)", "")).Replace("(C)", "").Replace("(CTG)", "").Replace("(CS)", "").Replace("(S)", "") & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    txtGroupCode.Text = dt.Rows(i)("customergroupcode").ToString

                    dtsCountryGroupDetails.NewRow()
                    dtsCountryGroupDetails.Rows.Add("CU", txtGroupTagName.Text, dt.Rows(i)("agentcode").ToString, dt.Rows(i)("agentname").ToString)


                    If dt.Rows(i)("active").ToString = "0" Then
                        chkActive.Checked = False
                    Else
                        chkActive.Checked = True
                    End If

                Next
                FillGridForEdit("agentmast.agentname")
                Dim dt1 As New DataTable



                If txtGroupTagName.Text.Contains("(CU)") Then
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
                            dtt.Rows.Add("CU", txtGroupTagName.Text)
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

            strSqlQry = "select agentmast.agentcode , agentmast.agentname,agentcatmast.agentcatname,ctrymast.ctryname,citymast.cityname,agent_sectormaster.sectorname,isnull(dbo. fn_get_customergroup(agentmast.agentcode),'')Customergroup,isnull(dbo.fn_get_countrygroup(ctrymast.ctrycode),'')countrygroups ,0 as sortorder,agentmast.tel1, agentmast.contact1 from agentmast  inner join ctrymast on agentmast.ctrycode=ctrymast.ctrycode inner join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode left outer join agent_sectormaster on agentmast.sectorcode=agent_sectormaster.sectorcode inner join citymast on citymast.citycode=agentmast.citycode "



            If txtGroupTagName.Text.Trim <> "" Then

                'If txtGroupTagName.Text.Contains("(R)") Then

                '    '    If Trim(strWhereCond) = "" Then
                '        strWhereCond = " upper(plgrpmast .plgrpname) LIKE '" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "").Replace("(CG)", "") & "%'"
                '    Else

                '        strWhereCond = strWhereCond & " AND upper(plgrpmast .plgrpname) LIKE '" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "").Replace("(CG)", "") & "%'"
                '    End If

                'ElseIf txtGroupTagName.Text.Contains("(G)") Then
                '    If Trim(strWhereCond) = "" Then
                '        strWhereCond = " ctrymast.ctrycode in (select holidaycalendar_detail.ctrycode   from holidaycalendar,holidaycalendar_detail where holidaycalendar.holidaycode = holidaycalendar_detail.holidaycode and holidayname='" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "").Replace("(CG)", "") & "')"
                '    Else
                '    '        strWhereCond = strWhereCond & " ctrymast.ctrycode in (select holidaycalendar_detail.ctrycode   from holidaycalendar,holidaycalendar_detail where holidaycalendar.holidaycode = holidaycalendar_detail.holidaycode and holidayname='" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "").Replace("(CG)", "") & "')"
                'End If
                'If txtGroupTagName.Text.Contains("CU") Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "agentmast.agentcode in (select customergroup_detail.agentcode   from customergroup,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname='" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(CTG)", "")).Replace("(CT)", "").Replace("(S)", "").Replace("(CU)", "").Replace("(CS)", "").Replace("(CG)", "") & "')"
                Else
                    strWhereCond = strWhereCond & " AND agentmast.agentcode in (select customergroup_detail.agentcode   from customergroup,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname='" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(CTG)", "")).Replace("(CT)", "").Replace("(S)", "").Replace("(CU)", "").Replace("(CS)", "").Replace("(CG)", "") & "')"
                End If
                'Else
                '    If Trim(strWhereCond) = "" Then

                '        strWhereCond = "  ctrymast.ctrycode in (select holidaycalendar_detail.ctrycode   from holidaycalendar,holidaycalendar_detail where holidaycalendar.holidaycode = holidaycalendar_detail.holidaycode and holidayname='" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "").Replace("(CG)", "") & "')"
                '    Else
                '        strWhereCond = strWhereCond & " AND ctrymast.ctrycode in (select holidaycalendar_detail.ctrycode   from holidaycalendar,holidaycalendar_detail where holidaycalendar.holidaycode = holidaycalendar_detail.holidaycode and holidayname='" & ((Trim(txtGroupTagName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "").Replace("(CG)", "") & "')"
                '    End If
                'End If
            End If
            'End If
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

            FillCheckbox()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

        Dim dtsCountryGroupDetails As DataTable
        dtsCountryGroupDetails = Session("sDtCountryGroupDetails")
        dtsCountryGroupDetails.Rows.Clear()
        Session("sDtCountryGroupDetails") = dtsCountryGroupDetails
        gv_SearchResult.DataSource = dtsCountryGroupDetails
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

            Dim dtsCountryGroupDetails As New DataTable
            dtsCountryGroupDetails = Session("sDtCountryGroupDetails")

            'Dim dtsCountryGroupDetailsNew As New DataTable
            'dtsCountryGroupDetailsNew = Session("sDtCountryGroupDetails")
            'dtsCountryGroupDetailsNew.Clear()

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbCountry"), LinkButton)

            If dtsCountryGroupDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsCountryGroupDetails.Rows.Count - 1 To 0 Step i - 1
                    If lb.Text.Trim = dtsCountryGroupDetails.Rows(i)("TypeName").ToString.Trim Then
                        dtsCountryGroupDetails.Rows.Remove(dtsCountryGroupDetails.Rows(i))
                    End If
                    ' dtsCountryGroupDetails.Rows(i).Delete()
                    dtsCountryGroupDetails.AcceptChanges()
                Next
            End If
            Session("sDtCountryGroupDetails") = dtsCountryGroupDetails

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
        lblHeading.Text = "View Customer Group"
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
        Dim strCustomerGroupValue As String = ""
        Dim strCustomerValue As String = ""
        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strCountryGroupValue As String = ""
        Dim strGroupValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strcategoryValue As String = ""
        Dim strSectorValue As String = ""
        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1

                If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                    If strCountryValue <> "" Then
                        strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CUSTOMERGROUP" Then
                    If strCustomerGroupValue <> "" Then
                        strCustomerGroupValue = strCustomerGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCustomerGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                    If strCustomerValue <> "" Then
                        strCustomerValue = strCustomerValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCustomerValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
                If dtt.Rows(i)("Code").ToString = "CATEGORY" Then
                    If strcategoryValue <> "" Then
                        strcategoryValue = strcategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strcategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CITY" Then
                    If strCityValue <> "" Then
                        strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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

            strSqlQry = " select agentmast.agentcode , agentmast.agentname,agentcatmast.agentcatname,ctrymast.ctryname,citymast.cityname,agent_sectormaster.sectorname,isnull(dbo. fn_get_customergroup(agentmast.agentcode),'')Customergroup,isnull(dbo.fn_get_countrygroup(ctrymast.ctrycode),'')countrygroups ,0 as sortorder,agentmast.tel1, agentmast.contact1,agentmast.currcode from agentmast  inner join ctrymast on agentmast.ctrycode=ctrymast.ctrycode inner join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode left outer join agent_sectormaster on agentmast.sectorcode=agent_sectormaster.sectorcode   inner  join citymast on citymast.citycode=agentmast.citycode "


            If strCustomerGroupValue <> "" Then

                If Trim(strWhereCond) = "" Then
                    strWhereCond = " agentmast.agentcode in (select customergroup_detail.agentcode   from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
                Else

                    strWhereCond = strWhereCond & " AND agentmast.agentcode in (select customergroup_detail.agentcode  from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in  (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
                End If
            End If
            If strCountryGroupValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
                Else
                    strWhereCond = strWhereCond & "  AND ctrymast.ctrycode in (select countrygroup_detail.ctrycode    from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
                End If
            End If
            If strcategoryValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(agentcatmast.agentcatname) in (" & Trim(strcategoryValue) & ")"
                Else
                    strWhereCond = strWhereCond & " and  upper(agentcatmast.agentcatname) in (" & Trim(strcategoryValue) & ")"
                End If
            End If
            If strCustomerValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
                Else
                    strWhereCond = strWhereCond & " and  upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
                End If
            End If
            If strCityValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(citymast.cityname) IN (" & Trim(strCityValue) & ")"
                Else
                    strWhereCond = strWhereCond & " and upper(citymast.cityname) IN (" & Trim(strCityValue) & ")"
                End If
            End If
            If strSectorValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(agent_sectormaster.sectorname)  in (" & Trim(strSectorValue) & ")"
                Else
                    strWhereCond = strWhereCond & " AND  upper(agent_sectormaster.sectorname) in (" & Trim(strSectorValue) & ")"
                End If
            End If
            If strCountryValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) in (" & Trim(strCountryValue) & ")"
                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) in (" & Trim(strCountryValue) & ")"
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
                            strWhereCond1 = " (upper(agent_sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentcatmast.agentcatname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or ( agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') or   upper(ctrymast.ctryname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') )"

                        Else
                            strWhereCond1 = strWhereCond1 & " OR (upper(agent_sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentcatmast.agentcatname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or ( agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') or   upper(ctrymast.ctryname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%') )"
                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " or (" & strWhereCond1 & ")"
                End If

            End If



            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            'If txtcountryname.Text.Trim <> "" Then
            '    If Trim(strWhereCond) = "" Then

            '        strWhereCond = " upper(ct.ctryname) LIKE '" & ((Trim(txtCountryName.Text.Trim.ToUpper).Replace("(C)", "")).Replace("(R)", "")).Replace("(G)", "") & "%'"
            '    Else
            '        strWhereCond = strWhereCond & " AND upper(ct.ctryname) LIKE '" & Trim(txtcountryname.Text.Trim.ToUpper) & "%'"
            '    End If
            'End If

            'If Trim(strWhereCond) <> "" Then
            '    strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If




            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
                    hdSQL.Value = strSqlQry
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
            objUtils.WritErrorLog("customergroupsearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        Dim strorderby As String = "agentmast.agentname"
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

            strSqlQry = "select agentmast.agentcode , agentmast.agentname,agentcatmast.agentcatname,ctrymast.ctryname,citymast.cityname,agent_sectormaster.sectorname,isnull(dbo. fn_get_customergroup(agentmast.agentcode),'')Customergroup,isnull(dbo.fn_get_countrygroup(ctrymast.ctrycode),'')countrygroups ,0 as sortorder,agentmast.tel1, agentmast.contact1 from agentmast inner join ctrymast on agentmast.ctrycode=ctrymast.ctrycode inner join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode left outer join agent_sectormaster on agentmast.sectorcode=agent_sectormaster.sectorcode inner join citymast on citymast.citycode=agentmast.citycode"
            Type = strType.Split(":")
            value = lsProcessValue.Split(":")
            If lsProcessValue.Trim <> "" Then

                lsProcessValue = ((((Trim(lsProcessValue.Trim).Replace("(CU)", "")).Replace("(S)", "")).Replace("(CG)", "")).Replace("(C)", "")).Replace("(CT)", "").Replace("(CTG)", "").Replace("(CS)", "").Replace("(T)", "")
                'For k = 0 To Type.GetUpperBound(0)
                '    If Type(k) <> "T" Then
                '        lsProcessValue = "'" & lsProcessValue & "'"
                '    End If

                If strType = "CU" Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = "agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname='" & Trim(lsProcessValue.Trim.ToUpper) & "')"
                    Else

                        strWhereCond = strWhereCond & " AND  agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname='" & Trim(lsProcessValue.Trim.ToUpper) & "')"
                    End If
                End If
                If strType = "CT" Then

                    If Trim(strWhereCond) = "" Then
                        strWhereCond = "upper(citymast.cityname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond = strWhereCond & " and  upper(citymast.cityname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    End If
                End If
                If strType = "CG" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = "ctrymast.ctrycode IN (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail  where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname='" & Trim(lsProcessValue.Trim.ToUpper) & "')"
                    Else
                        strWhereCond = strWhereCond & " and ctrymast.ctrycode (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail  where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname='" & Trim(lsProcessValue.Trim.ToUpper) & "')"
                    End If
                End If
                If strType = "C" Then
                    If Trim(strWhereCond) = "" Then

                        strWhereCond = " upper(ctrymast.ctryname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"

                    Else
                        strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    End If
                End If
                If strType = "CTG" Then
                    If Trim(strWhereCond) = "" Then

                        strWhereCond = " upper(agentcatmast.agentcatname) LIKE'" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    End If
                End If
                If strType = "CS" Then
                    If Trim(strWhereCond) = "" Then

                        strWhereCond = " upper(agentmast.agentname) LIKE'" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(agentmast.agentname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    End If
                End If
                If strType = "S" Then
                    If Trim(strWhereCond) = "" Then

                        strWhereCond = " upper(agent_sectormaster.sectorname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) LIKE '" & Trim(lsProcessValue.Trim.ToUpper) & "%'"
                    End If
                End If
                If strType = "T" Then

                    Dim lsMainArr As String()
                    Dim strValue As String = ""
                    Dim strWhereCond1 As String = ""
                    lsMainArr = objUtils.splitWithWords(lsProcessValue, ",")
                    For i = 0 To lsMainArr.GetUpperBound(0)
                        strValue = ""
                        strValue = lsMainArr(i)
                        If strValue <> "" Then
                            If Trim(strWhereCond1) = "" Then
                                strWhereCond1 = " (upper(agent_sectormaster.sectorname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%' or upper(agentmast.agentname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%' or upper(agentcatmast.agentcatname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%' or upper(citymast.cityname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%'   or ( agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,customergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%')  or   ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%') or   upper(ctrymast.ctryname)  LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%') )"
                            Else
                                strWhereCond1 = strWhereCond1 & " OR(upper(agent_sectormaster.sectorname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%'or upper(agentmast.agentname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%' or upper(agentcatmast.agentcatname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%' or upper(citymast.cityname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%'   or ( agentmast.agentcode IN (select customergroup_detail.agentcode   from customergroup ,coustomergroup_detail  where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%')  or   ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%') or   upper(ctrymast.ctryname)  LIKE '%" & Trim(lsProcessValue.Trim.ToUpper) & "%')) "
                            End If
                        End If
                    Next
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = "(" & strWhereCond1 & ")"
                    Else
                        strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                    End If


                End If
                'Next 
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
            objUtils.WritErrorLog("CountryGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        Dim lsProcessgroup As String = ""
        Dim lsProcesscust As String = ""
        Dim lsProcesscustomer As String = ""

        Dim lsProcesssector As String = ""
        Dim lsProcesscity As String = ""
        Dim lsProcesscategory As String = ""
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        Dim lsProcessCountryGroup As String = ""
        Dim lsMainArr As String()

        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        If hdOPMode.Value = "S" Then

            For i = 0 To lsMainArr.GetUpperBound(0)
                Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                    Case "COUNTRYGROUP"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("COUNTRYGROUP", lsProcessCountryGroup, "CG")

                    Case "CUSTOMERGROUP"
                        lsProcesscustomer = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CUSTOMERGROUP", lsProcesscustomer, "CU")
                    Case "SECTOR"
                        lsProcesssector = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SECTOR", lsProcesssector, "S")
                    Case "CITY"
                        lsProcesscity = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CITY", lsProcesscity, "CT")
                    Case "CATEGORY"
                        lsProcesscategory = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CATEGORY", lsProcesscategory, "CTG")
                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("TEXT", lsProcessText, "T")
                    Case "COUNTRY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("COUNTRY", lsProcessCountry, "C")
                    Case "CUSTOMER"
                        lsProcesscust = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CUSTOMER", lsProcesscust, "CS")
                End Select
            Next

            Dim dttDyn As DataTable
            dttDyn = Session("sDtDynamicSearch")
            dlListSearch.DataSource = dttDyn
            dlListSearch.DataBind()
            FillGridNew()

        ElseIf hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then
            If txtGroupTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Customer Group.');", True)
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
                    Case "CUSTOMERGROUP"
                        lsProcesscustomer = lsMainArr(i).Split(":")(1) & "(CU)"
                        txtNameNew.Text = lsProcesscustomer
                        FillGridByType("CU", lsProcesscustomer)
                        'sbAddToDataTable("REGION", lsProcessRegion, "R")
                        Dim iFlag As Integer = 0


                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcesscustomer Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CU", lsProcesscustomer)

                            End If

                        End If

                    Case "SECTOR"
                        lsProcesssector = lsMainArr(i).Split(":")(1) & "(S)"
                        txtNameNew.Text = lsProcesssector
                        FillGridByType("S", lsProcesssector)
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcesssector Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("S", lsProcesssector)
                            End If

                        End If
                    Case "COUNTRYGROUP"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1) & "(CG)"
                        txtNameNew.Text = lsProcessCountryGroup
                        FillGridByType("CG", lsProcessCountryGroup)
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountryGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CG", lsProcessCountryGroup)
                            End If

                        End If

                    Case "CATEGORY"
                        lsProcesscategory = lsMainArr(i).Split(":")(1) & "(CTG)"
                        txtNameNew.Text = lsProcesscategory
                        FillGridByType("CTG", lsProcesscategory)
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcesscategory Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CTG", lsProcesscategory)
                            End If

                        End If
                    Case "CITY"
                        lsProcesscity = lsMainArr(i).Split(":")(1) & "(CT)"
                        txtNameNew.Text = lsProcesscity
                        FillGridByType("CT", lsProcesscity)
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcesscity Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CT", lsProcesscity)
                            End If

                        End If

                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1) & "(T)"
                        txtNameNew.Text = lsProcessText
                        FillGridByType("T", lsProcessText)
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


                    Case "COUNTRY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1) & "(C)"
                        txtNameNew.Text = lsProcessCountry
                        FillGridByType("C", lsProcessCountry)
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



                    Case "CUSTOMER"
                        lsProcesscust = lsMainArr(i).Split(":")(1) & "(CS)"
                        txtNameNew.Text = lsProcesscust
                        FillGridByType("CS", lsProcesscust)
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcesscust Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CS", lsProcesscust)
                            End If

                        End If
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
            'Next
            'FillGridByType(IsProcessType, IsProcessValue)
            If dtt.Rows.Count >= 0 Then

                Dim dtsCountryGroupDetails As DataTable
                dtsCountryGroupDetails = Session("sDtCountryGroupDetails")
                If dtsCountryGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "Customer" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("CU", "Customer")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()

            txtName.Text = ""
        ElseIf hdOPMode.Value = "D" Then
            If txtGroupTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Holiday Name.');", True)
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
            Dim dtsCountryGroupDetails As DataTable
            dtsCountryGroupDetails = Session("sDtCountryGroupDetails")
            dtsCountryGroupDetails.Rows.Clear()
            Session("sDtCountryGroupDetails") = dtsCountryGroupDetails
            gv_SearchResult.DataSource = dtsCountryGroupDetails
            gv_SearchResult.DataBind()


            Dim dtDynamic As DataTable
            dtDynamic = Session("sDtDynamic")
            dtDynamic.Rows.Clear()
            Session("sDtDynamic") = dtDynamic
            dlList.DataSource = dtDynamic
            dlList.DataBind()
        End If



    End Sub

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

    Protected Sub lnkEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblcntryCode"), Label)
        Session.Add("CustState", "Edit")
        Session.Add("CustRefCode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Customers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub lnkView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblcntryCode"), Label)
        Session.Add("CustState", "View")
        Session.Add("CustRefCode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If CType(Request.QueryString("appid"), String) = "2" Or CType(Request.QueryString("appid"), String) = "3" Or CType(Request.QueryString("appid"), String) = "9" Then
            strpop = "window.open('CustMainDet.aspx?appid=" + CType("1", String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Customers');"
        Else
            strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Customers');"
        End If

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnAddHotel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddHotel.Click
        Session.Add("CustState", "New")
        Dim strpop As String = ""
        strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New','Customers');"
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
        Dim lblId As Label = CType(row.FindControl("lblcntryCode"), Label)
        Session.Add("CustState", "Delete")
        Session.Add("CustRefCode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Customers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnHotelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHotelReport.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=CustomerGroup&BackPageName=CustomerGroupSearch.aspx&customergroupcode=" + txtGroupCode.Text.Trim + "&customergroupname=" + txtName.Text.Trim + "','rptCustomerGroup');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvSearchGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchGrid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim chkCtry2 As CheckBox = e.Row.FindControl("chkCtry2")
            'Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            'chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)

            Dim lblCUSName As Label = e.Row.FindControl("lblCuName")
            Dim lblCName As Label = e.Row.FindControl("lblCgName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblsectorName")
            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblCUName As Label = e.Row.FindControl("lblagentName")
            Dim lblctryName As Label = e.Row.FindControl("lblctryName")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextcountryGroup As String = ""
            Dim lsSearchTextcustomerGroup As String = ""
            Dim lsSearchTextcustomer As String = ""
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
                        If "COUNTRY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "COUNTRYGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextcountryGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CUSTOMERGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextcustomerGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CUSTOMER" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextcustomer = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCat
                            lsSearchTextSector = lsSearchTextCat
                            lsSearchTextcountryGroup = lsSearchTextCat
                            lsSearchTextcustomerGroup = lsSearchTextCat
                            lsSearchTextcustomer = lsSearchTextCat
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblctryName.Text = Regex.Replace(lblctryName.Text.Trim, lsSearchTextCtry.Trim(), _
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
                        If lsSearchTextcustomer.Trim <> "" Then
                            lblCUName.Text = Regex.Replace(lblCUName.Text.Trim, lsSearchTextcustomer.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextcustomerGroup.Trim <> "" Then
                            lblCUSName.Text = Regex.Replace(lblCUSName.Text.Trim, lsSearchTextcustomerGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextcountryGroup.Trim <> "" Then
                            lblCName.Text = Regex.Replace(lblCName.Text.Trim, lsSearchTextcountryGroup.Trim(), _
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

            Dim lblCUSName As Label = e.Row.FindControl("lblCuName")
            Dim lblCName As Label = e.Row.FindControl("lblCgName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblsectorName")
            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblCUName As Label = e.Row.FindControl("lblCountryName")
            Dim lblctryName As Label = e.Row.FindControl("lblctryName")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextcountryGroup As String = ""
            Dim lsSearchTextcustomerGroup As String = ""
            Dim lsSearchTextcustomer As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextcountryGroup = ""
                        If "CU" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextcountryGroup = dtDynamics.Rows(j)("COUNTRY").ToString.Trim.ToUpper
                            lsSearchTextcountryGroup = lsSearchTextcountryGroup.Replace("(CU)", "")
                        End If
                        If "CS" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextcustomer = dtDynamics.Rows(j)("COUNTRY").ToString.Trim.ToUpper
                            lsSearchTextcustomer = lsSearchTextcustomer.Replace("(CS)", "")
                        End If
                        If "CTG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("COUNTRY").ToString.Trim.ToUpper
                            lsSearchTextCat = lsSearchTextCat.Replace("(CTG)", "")
                        End If
                        If "CT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("COUNTRY").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCity.Replace("(CT)", "")
                        End If
                        If "S" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("COUNTRY").ToString.Trim.ToUpper
                            lsSearchTextSector = lsSearchTextSector.Replace("(S)", "")
                        End If
                        If "CG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextcountryGroup = dtDynamics.Rows(j)("COUNTRY").ToString.Trim.ToUpper
                            lsSearchTextcountryGroup = lsSearchTextcountryGroup.Replace("(CG)", "")
                        End If
                        If "C" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("COUNTRY").ToString.Trim.ToUpper
                            lsSearchTextCtry = lsSearchTextCtry.Replace("(C)", "")
                        End If
                        If "T" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("COUNTRY").ToString.Replace("(T)", "").Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCat
                            lsSearchTextSector = lsSearchTextCat
                            lsSearchTextcountryGroup = lsSearchTextCat
                            lsSearchTextcustomerGroup = lsSearchTextCat
                            lsSearchTextCtry = lsSearchTextCat
                            lsSearchTextcustomer = lsSearchTextCat
                        End If





                        If lsSearchTextcustomer.Trim <> "" Then
                            lblCUName.Text = Regex.Replace(lblCUName.Text.Trim, lsSearchTextcustomer.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextcustomerGroup.Trim <> "" Then
                            lblCUSName.Text = Regex.Replace(lblCUSName.Text.Trim, lsSearchTextcustomerGroup.Trim(), _
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
                        If lsSearchTextcountryGroup.Trim <> "" Then
                            lblCName.Text = Regex.Replace(lblCName.Text.Trim, lsSearchTextcountryGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCtry.Trim <> "" Then
                            lblctryName.Text = Regex.Replace(lblctryName.Text.Trim, lsSearchTextCtry.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If
    End Sub
End Class










