
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit


#End Region
Partial Class SupHotelAmenities
    Inherits System.Web.UI.Page

    Dim objUser As New clsUser

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim MyDS As New DataSet
    Dim MyAdapter As SqlDataAdapter
    Dim GvRow As GridViewRow
    Dim strtypecode As String
#End Region

 

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        TabAmenities.Visible = True
        ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select distinct sptypecode from sptypemast where active=1 order by sptypecode", True)


        Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
        If CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\HotelGroups.aspx?appid=1", String), "1")
        ElseIf CType(Request.QueryString("appid"), String) = "1" Then
            Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)
        Else
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
        End If
        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
        Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

        'If Not Session("CompanyName") Is Nothing Then
        '    Me.Page.Title = CType(Session("CompanyName"), String)
        'End If

        If IsPostBack = False Then
            Session("partycode") = Nothing
            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If

            Dim sptype As String
            If CType(Session("SupState"), String) = "New" Or CType(Session("SupState"), String) = "Edit" Then
                If Not Session("SupRefCode") = Nothing Then
                    sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" + CType(Session("SupRefCode"), String) + "'")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode='" + sptype + "' order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" + sptype + "' order by partyname", True)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1  order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1  order by partyname", True)
                End If
            End If

            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                lblHeading.Text = "Add New Supplier" + " - " + TabAmenities.ToolTip
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + TabAmenities.ToolTip
                BtnSaveAmenity.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save your changes?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnSaveAmenity.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtCode)
                lblHeading.Text = "Edit Supplier" + " - " + TabAmenities.ToolTip
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + TabAmenities.ToolTip
                BtnSaveAmenity.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save your changes?')==false)return false;")

            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                lblHeading.Text = "View Supplier" + " - " + TabAmenities.ToolTip
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + TabAmenities.ToolTip
                BtnSaveAmenity.Visible = False
                BtnSaveAmenity.Text = "Return to Search"

            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                ddlSuppierCD.Visible = False
                ddlSuppierNM.Visible = False
                btnfilldetail.Visible = False
                PnlSuplierSel.Visible = False
                BtnSelectAllAmenities.Visible = False
                BtnDeSelectAllAmenities.Visible = False
                lblHeading.Text = "Delete Supplier" + " - " + TabAmenities.ToolTip
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + TabAmenities.ToolTip
                BtnSaveAmenity.Text = "Delete"
                BtnSaveAmenity.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Selected Amenities?')==false)return false;")
            End If

            

            BtnAmenityCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            FillAmenities(txtCode.Value.ToString())
        Else
            If Session("addmeal") = "new" Then
                FillAmenities(txtCode.Value.ToString())
                Session.Remove("addmeal")
            End If
        End If

        Dim typ As Type
        typ = GetType(DropDownList)

        Me.whotelatbcontrol.partyval = txtCode.Value

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
            ddlSuppierCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSuppierNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
        Me.SubMenuUserControl1.suptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", txtCode.Value.Trim)
        Session.Add("submenuuser", "SupplierSearch.aspx")

    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from partymast Where partycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        Me.txtCode.Value = mySqlReader("partycode")
                    End If
                    If IsDBNull(mySqlReader("partyname")) = False Then
                        Me.txtName.Value = mySqlReader("partyname")
                    End If
                    mySqlCmd.Dispose()
                    mySqlReader.Close()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupHotelAmenities.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "blocksale_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a BlockFullOfSales, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cancel_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a CancellationPolicy, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "child_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a ChildPolicy, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compulsory_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a CompulsoryRemarks, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplistdwknew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Promotions, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a PriceListConversion, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromotion_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a EarliBirdPromotion, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "flightmast", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a FlightMaster, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "hotels_construction", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a HotelConstruction, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "minnights_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MinimumNights, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "party_splevents", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SpecialEvents/Extras For Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyallot", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SupplierAllotment, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyinfo", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Supplier Information, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast_mulltiemail", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MultiEmail of Suppliers, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymaxaccomodation", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a MaximumAccomodation Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothcat", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherService Category OF Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothgrp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherServiceGroup Of Supplier , cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothtyp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a OtherServiceTypes of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyrmcat", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a RoomCategory Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyrmtyp", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a RoomType Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_header", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Promotions, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellsph", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SellingFormulaForSupplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SupplierPolicy, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplistd", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Details Of SpecialEvents/Extras, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a SpecialEvents/Extras, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymeal", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for a Meal Of Supplier, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region

    '*** May not need this in this form Now. this function checks weather any selected option is used in any child table   Danny  12/02/2018
    'Private Function mealcode() As Boolean

    '    Try
    '        strSqlQry = "select * from view_partymealcode where partycode='" & txtCode.Value & "'"


    '        Dim MyGrpDS As New DataSet
    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '        MyAdapter.Fill(MyGrpDS, "view_partymealcode")

    '        Dim GVRow1 As GridViewRow
    '        Dim CHK1 As HtmlInputCheckBox

    '        If MyGrpDS.Tables(0).Rows.Count > 0 Then


    '            For Each GVRow1 In Gv_MealPlan.Rows
    '                CHK1 = GVRow1.FindControl("ChkSelect")
    '                If CHK1.Checked = False Then
    '                    For i As Integer = 0 To MyGrpDS.Tables(0).Rows.Count - 1

    '                        If GVRow1.Cells(2).Text = MyGrpDS.Tables(0).Rows(i).Item("mealcode") Then
    '                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' You are not allowed to uncheck " + GVRow1.Cells(2).Text + " ,already used in " + MyGrpDS.Tables(0).Rows(i).Item("form") + " ')", True)

    '                            Return False
    '                            Exit Function


    '                        End If
    '                    Next

    '                End If
    '            Next

    '        Else
    '            Return True
    '        End If


    '        Return True


    '    Catch ex As Exception

    '    End Try
    'End Function
    <System.Web.Services.WebMethod()> _
    Public Function UpdateSelections() As Boolean
        Try
            '*** May not need this in this form Now Danny  12/02/2018
            'If mealcode() = False Then
            '    Exit Sub
            'End If

            Dim flag As Integer = 0
            'Dim GVRow As GridViewRow
            Dim strPrompt As String = "You have selected\n"


            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Function
                ElseIf Session("SupState") = "Edit" Then


                    ' '' ''*** Danny 8/3/18 Single tab saved>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    '' ''Select Case TabAmenities.ActiveTabIndex
                    '' ''    Case 0
                    '' ''        '*** 1 Tab==================================================
                    '' ''        flag = ValidateGrid(Gv_RoomAmenities)
                    '' ''        If flag = False Then
                    '' ''            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one Amenity from current Tab ROOM AMENITIES.');", True)
                    '' ''            Exit Sub
                    '' ''        End If
                    '' ''        UpdateGrdValues(Gv_RoomAmenities, "ROOM AMENITIES")
                    '' ''    Case 1
                    '' ''        '*** 1 Tab==================================================
                    '' ''        flag = ValidateGrid(Gv_HotelAmenities)
                    '' ''        If flag = False Then
                    '' ''            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one Amenity from current Tab HOTEL AMENITIES.');", True)
                    '' ''            Exit Sub
                    '' ''        End If
                    '' ''        UpdateGrdValues(Gv_HotelAmenities, "HOTEL AMENITIES")
                    '' ''    Case 3
                    '' ''        '*** 1 Tab==================================================
                    '' ''        flag = ValidateGrid(Gv_RecreationAmenities)
                    '' ''        If flag = False Then
                    '' ''            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one Amenity from current Tab RECREATION AMENITIES.');", True)
                    '' ''            Exit Sub
                    '' ''        End If
                    '' ''        UpdateGrdValues(Gv_RecreationAmenities, "RECREATION AMENITIES")
                    '' ''End Select
                    ''*** Danny 8/3/18 Single tab saved<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    ''*** Danny 8/3/18 Changed to all tab save

                    '*** 1 Tab==================================================
                    flag = ValidateGrid(Gv_RoomAmenities)
                    strPrompt = strPrompt + flag.ToString + " Amenities from ROOM AMENITIES \n"
                    '*** 1 Tab==================================================
                    flag = ValidateGrid(Gv_HotelAmenities)
                    strPrompt = strPrompt + flag.ToString + " Amenities from HOTEL AMENITIES \n"
                    '*** 1 Tab==================================================
                    flag = ValidateGrid(Gv_RecreationAmenities)
                    strPrompt = strPrompt + flag.ToString + " Amenities from RECREATION AMENITIES \n\n"
                    'If flag = False Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You have not selected any Record.');", True)

                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "", True)
                    '    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "Confirmselection();", True)
                    '    BtnSaveAmenity.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save your changes?')==false)return false;")
                    '    Exit Function
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    'sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    UpdateGrdValues(Gv_RoomAmenities, "ROOM AMENITIES")
                    UpdateGrdValues(Gv_HotelAmenities, "HOTEL AMENITIES")
                    UpdateGrdValues(Gv_RecreationAmenities, "RECREATION AMENITIES")

                    '*** Danny 8/3/18 Single tab saved<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                ElseIf Session("SupState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Function
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    'sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.VarChar, 10)).Value = ""
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_Del_partyothgrp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyothtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                End If

                'sqlTrans.Commit()    'SQl Tarn Commit
                'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 

                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("SupState") = "New" Then
                    strPrompt = strPrompt + "Record Saved Successfully."
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + strPrompt + "');", True)
                ElseIf Session("SupState") = "Edit" Then
                    strPrompt = strPrompt + "Record Updated Successfully."
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + strPrompt + "');", True)
                End If
                If Session("SupState") = "Delete" Then
                    'Response.Redirect("SupplierSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'If mySqlConn.State = ConnectionState.Open Then
            '    'sqlTrans.Rollback()
            'End If

            objUtils.WritErrorLog("SupHotelAmenities.aspx=>UpdateSelections()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Protected Sub BtnSaveAmenity_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateSelections()
    End Sub
    Private Function ValidateGrid(ByVal GrdV As GridView) As Integer
        Dim CHK As HtmlInputCheckBox
        ValidateGrid = 0
        For Each GvRow In GrdV.Rows
            CHK = GvRow.FindControl("ChkSelect")
            If CHK.Checked = True Then
                ValidateGrid = ValidateGrid + 1
                'Exit For
            End If
        Next

    End Function
    Private Sub UpdateGrdValues(ByVal GrdS As GridView, ByVal AmenityTypecode As String)



        Dim CHK As HtmlInputCheckBox
        'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        'sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

        Dim DtGrv As DataTable = New DataTable()
        DtGrv.Columns.Add("PartyCode")
        DtGrv.Columns.Add("AmenityCode")
        DtGrv.Columns.Add("userlogged")
        DtGrv.Columns.Add("AmenityTypecode")


        For Each GvRow In GrdS.Rows
            CHK = GvRow.FindControl("ChkSelect")
            If CHK.Checked = True Then
                DtGrv.Rows.Add(CType(txtCode.Value.Trim, String), CType(GvRow.Cells(2).Text, String), CType(Session("GlobalUserName"), String), CType(AmenityTypecode, String))
            End If
        Next
        Dim xmlStr As String
        Dim ds As New DataSet
        ds.Tables.Add(DtGrv)
        xmlStr = ds.GetXml

        'mySqlCmd = New SqlCommand("SP_Amenities_Select", mySqlConn, sqlTrans)
        mySqlCmd = New SqlCommand("SP_Amenities_Select", mySqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@AmenityTypecode", SqlDbType.VarChar, 20)).Value = CType(AmenityTypecode, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@XML", SqlDbType.Xml)).Value = CType(xmlStr, String)

        mySqlCmd.ExecuteNonQuery()

    End Sub
    Protected Sub BtnAmenityCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Private Sub SelectCheckBox(ByVal Grd As GridView)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Grd.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
        Next


    End Sub
    Private Sub DeSelectCheckBox(ByVal Grd As GridView)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Grd.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
        Next
    End Sub
    Protected Sub BtnDeSelectAllAmenities_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Select Case TabAmenities.ActiveTabIndex
        '    Case 0
        '*** 1 Tab============================================
        DeSelectCheckBox(Gv_RoomAmenities)
        'Case 1
        '*** 2 Tab============================================
        DeSelectCheckBox(Gv_HotelAmenities)
        'Case 2
        '*** 3 Tab============================================
        DeSelectCheckBox(Gv_RecreationAmenities)
        'End Select


    End Sub
    Protected Sub BtnSelectAllAmenities_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Select Case TabAmenities.ActiveTabIndex
        '    Case 0
        '*** 1 Tab============================================
        SelectCheckBox(Gv_RoomAmenities)
        'Case 1
        '*** 2 Tab============================================
        SelectCheckBox(Gv_HotelAmenities)
        'Case 2
        '*** 3 Tab============================================
        SelectCheckBox(Gv_RecreationAmenities)
        'End Select





    End Sub

   

    Private Sub GridFillAndCheck(ByVal strSqlQry As String, ByVal FillingGrid As GridView, ByVal SelectedParty As String, ByVal AmenityType As String)
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        mySqlConn.Close()

        'strSqlQry = "select  *,0 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='RECREATION AMENITIES' AND active=1 and  AmenityCode   in  ( select AmenityCode from TB_PartyAmenities where partycode='" & txtCode.Value & "' ) union  " & _
        '               "select  *,1 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='RECREATION AMENITIES' AND active=1 and TB_HotelAmenitiesMaster.sptypecode in ( select  sptypecode from partymast where  partycode='" & txtCode.Value & "') and AmenityCode not in  ( select AmenityCode from TB_PartyAmenities where partycode='" & txtCode.Value & "') order by orderby"

        Dim MyCatDS As New DataSet
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyCatDS, "AmenityCode")
        FillingGrid.DataSource = MyCatDS
        FillingGrid.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()
        Dim arrMeal(MyCatDS.Tables(0).Rows.Count + 1) As String
        Dim i As Long = 0
        strSqlQry = "select AmenityCode from TB_PartyAmenities where partycode='" & SelectedParty & "' AND AmenityTypecode='" & AmenityType & "'"
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        While mySqlReader.Read = True
            arrMeal(i) = mySqlReader("AmenityCode")
            i = i + 1
        End While
        MyAdapter.Dispose()
        mySqlConn.Close()

        i = 0
        For i = 0 To FillingGrid.Rows.Count
            For Each GvRow In FillingGrid.Rows
                chk = GvRow.FindControl("ChkSelect")
                If arrMeal(i) = GvRow.Cells(2).Text Then
                    chk.Checked = True
                End If
            Next
        Next
    End Sub
    Private Sub GridFill(ByVal strSqlQry As String, ByVal FillingGrid As GridView)
        mySqlConn.Close()
        Dim MyDS_Category As New DataSet
        Dim MyDS_Meal As New DataSet
        'strSqlQry = "select * from TB_HotelAmenitiesMaster where AmenityTypecode='ROOM AMENITIES' AND active=1 and sptypecode in " +
        '    "( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "')"

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyDS_Meal, "AmenityCode")
        FillingGrid.DataSource = MyDS_Meal
        FillingGrid.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()
    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=HotAme','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnfilldetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilldetail.Click
        If ddlSuppierNM.Value <> "[Select]" Or ddlSuppierNM.Value <> "" Then

            FillAmenities(ddlSuppierNM.Value.ToString())
        End If
    End Sub
    Private Sub FillAmenities(ByVal SelectedParty As String)
        '*** Dynamic Tab loading. Check box need to add=============================
        ' ''Try

        ' ''    Dim MyDS_Category As New DataSet
        ' ''    Dim MyDS_Meal As New DataSet
        ' ''    strSqlQry = "select * from TB_AmenityTypeMaster where Active ='1'"
        ' ''    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        ' ''    MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        ' ''    MyAdapter.Fill(MyDS_Meal, "AmenityCode")

        ' ''    'MyBase.OnInit(e)

        ' ''    For i As Integer = 0 To MyDS_Meal.Tables(0).Rows.Count - 1
        ' ''        Dim tabContent As Control = New Control()
        ' ''        Dim tab As TabPanel = New TabPanel()
        ' ''        tab.ID = MyDS_Meal.Tables(0).Rows(i).Item(0).ToString().Replace(" ", "_")
        ' ''        tab.HeaderText = MyDS_Meal.Tables(0).Rows(i).Item(0).ToString()
        ' ''        Dim Gv_Amenities1 As GridView = New GridView()

        ' ''        Dim ChkSelect As CheckBox = New CheckBox()
        ' ''        ChkSelect.ID = "ckk"
        ' ''        Dim GvColumn_Chk As TemplateField = New TemplateField()
        ' ''        GvColumn_Chk.HeaderText = "Amenity Name"
        ' ''        Gv_Amenities1.Columns.Add(GvColumn_Chk)

        ' ''        Dim GvColumn_AmenityName As BoundField = New BoundField()
        ' ''        GvColumn_AmenityName.DataField = "AmenityName"
        ' ''        GvColumn_AmenityName.HeaderText = "Amenity Name"
        ' ''        Gv_Amenities1.Columns.Add(GvColumn_AmenityName)

        ' ''        Dim GvColumn_AmenityCode As BoundField = New BoundField()
        ' ''        GvColumn_AmenityCode.DataField = "AmenityCode"
        ' ''        GvColumn_AmenityCode.HeaderText = "Amenity Code"
        ' ''        Gv_Amenities1.Columns.Add(GvColumn_AmenityCode)


        ' ''        mySqlConn.Close()
        ' ''        Dim MyDS_Category1 As New DataSet
        ' ''        Dim MyDS_Meal1 As New DataSet
        ' ''        strSqlQry = "select '',AmenityName,AmenityCode from TB_HotelAmenitiesMaster where AmenityTypecode ='" + tab.HeaderText + "' AND active=1 and sptypecode in ( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "')"

        ' ''        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        ' ''        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        ' ''        MyAdapter.Fill(MyDS_Meal1, "AmenityCode")
        ' ''        Gv_Amenities1.AutoGenerateColumns = False



        ' ''        Gv_Amenities1.DataSource = MyDS_Meal1
        ' ''        Gv_Amenities1.DataBind()
        ' ''        MyAdapter.Dispose()
        ' ''        mySqlConn.Close()

        ' ''        tabContent.Controls.Add(Gv_Amenities1)
        ' ''        tab.Controls.Add(tabContent)
        ' ''        Me.TabAmenities.Tabs.Add(tab)
        ' ''    Next

        ' ''    MyAdapter.Dispose()
        ' ''    mySqlConn.Close()

        ' ''Catch ex As Exception
        ' ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        ' ''Finally
        ' ''    mySqlConn.Close()
        ' ''End Try





        Try
            strSqlQry = "select * from TB_PartyAmenities where partycode='" & SelectedParty.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                '***1 Tab ===============================================================
                strSqlQry = "select * from TB_HotelAmenitiesMaster where AmenityTypecode='ROOM AMENITIES' AND active=1 and sptypecode in " +
                            "( select  sptypecode from partymast where  partycode='" & SelectedParty.Trim & "') " +
                            " ORDER BY AmenityName"
                GridFill(strSqlQry, Gv_RoomAmenities)

                '***2 Tab ===============================================================
                strSqlQry = "select * from TB_HotelAmenitiesMaster where AmenityTypecode='HOTEL AMENITIES' AND active=1 and sptypecode in " +
                    "( select  sptypecode from partymast where  partycode='" & SelectedParty.Trim & "') " +
                            " ORDER BY AmenityName"
                GridFill(strSqlQry, Gv_HotelAmenities)

                '***3 Tab ===============================================================
                strSqlQry = "select * from TB_HotelAmenitiesMaster where AmenityTypecode='RECREATION AMENITIES' AND active=1 and sptypecode in " +
                   "( select  sptypecode from partymast where  partycode='" & SelectedParty.Trim & "') " +
                            " ORDER BY AmenityName"
                GridFill(strSqlQry, Gv_RecreationAmenities)
            Else
                '***3 Tab ===============================================================
                strSqlQry = "select  *,0 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='ROOM AMENITIES' AND active=1 and  AmenityCode   in  ( select AmenityCode from TB_PartyAmenities where partycode='" & SelectedParty & "' ) union  " & _
                               "select  *,1 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='ROOM AMENITIES' AND active=1 and TB_HotelAmenitiesMaster.sptypecode in ( select  sptypecode from partymast where  partycode='" & SelectedParty & "') and AmenityCode not in  ( select AmenityCode from TB_PartyAmenities where partycode='" & SelectedParty & "') order by orderby"
                GridFillAndCheck(strSqlQry, Gv_RoomAmenities, SelectedParty, "ROOM AMENITIES")

                '***3 Tab ===============================================================
                strSqlQry = "select  *,0 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='HOTEL AMENITIES' AND active=1 and  AmenityCode   in  ( select AmenityCode from TB_PartyAmenities where partycode='" & SelectedParty & "' ) union  " & _
                               "select  *,1 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='HOTEL AMENITIES' AND active=1 and TB_HotelAmenitiesMaster.sptypecode in ( select  sptypecode from partymast where  partycode='" & SelectedParty & "') and AmenityCode not in  ( select AmenityCode from TB_PartyAmenities where partycode='" & SelectedParty & "') order by orderby"
                GridFillAndCheck(strSqlQry, Gv_HotelAmenities, SelectedParty, "HOTEL AMENITIES")

                '***3 Tab ===============================================================
                strSqlQry = "select  *,0 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='RECREATION AMENITIES' AND active=1 and  AmenityCode   in  ( select AmenityCode from TB_PartyAmenities where partycode='" & SelectedParty & "' ) union  " & _
                               "select  *,1 as orderby from TB_HotelAmenitiesMaster where AmenityTypecode='RECREATION AMENITIES' AND active=1 and TB_HotelAmenitiesMaster.sptypecode in ( select  sptypecode from partymast where  partycode='" & SelectedParty & "') and AmenityCode not in  ( select AmenityCode from TB_PartyAmenities where partycode='" & SelectedParty & "') order by orderby"
                GridFillAndCheck(strSqlQry, Gv_RecreationAmenities, SelectedParty, "RECREATION AMENITIES")

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            Response.Redirect("~/Login.aspx", False)
        Finally
            If mySqlConn IsNot Nothing Then
                mySqlConn.Close()
            End If
        End Try
    End Sub
   
    

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        Dim strpop As String = ""
        strpop = "window.open('SupHotelAmenitiesAdd.aspx?State=New','Amenities');"
        Session.Add("addmeal", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region
End Class

