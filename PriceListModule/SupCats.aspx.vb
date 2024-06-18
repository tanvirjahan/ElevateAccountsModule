#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupCats
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Dim accom_extra As String

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
        Dim appid As String

        PanelCategories.Visible = True
        ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select distinct sptypecode from sptypemast where active=1 order by sptypecode", True)

        If IsPostBack = False Then

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


            Session("partycode") = Nothing

            ' if additional query string passed with the page, then appid merging with the previous string - Christo.A - 24/07/16
            If CType(Request.QueryString("appid"), String) = "1" Then
                appid = CType(Request.QueryString("appid"), String) ' Mid(Request.QueryString("rmcat"), InStr(Request.QueryString("rmcat"), "appid", vbBinaryCompare) + 6, 1)
                If CType(Request.QueryString("appid"), String) = "4" Then 'accounts module
                    appid = "1"
                End If
                accom_extra = Mid(Request.QueryString("rmcat"), 1, 1)
                txtaccom_extra.Value = accom_extra
                Me.SubMenuUserControl1.appval = CType(appid, String)
                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)

                Me.whotelatbcontrol.appval = CType(appid, String)
                Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(appid, Integer))
            End If


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
                lblHeading.Text = "Add New Supplier" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                BtnSaveCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnSaveCategory.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtCode)
                lblHeading.Text = "Edit Supplier" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                BtnSaveCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")

            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                lblHeading.Text = "View Supplier" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                BtnSaveCategory.Visible = False
                BtnSaveCategory.Text = "Return to Search"

            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True

                lblHeading.Text = "Delete Supplier" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + IIf(txtaccom_extra.Value = "A", "Accomodation Categories", "Supplement Categories")
                BtnSaveCategory.Text = "Delete"
                BtnSaveCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
            End If
            BtnCancelCategory.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            FillCategorisPanel()
        Else
            accom_extra = Mid(Request.QueryString("rmcat"), 1, 1)
            txtaccom_extra.Value = accom_extra
            'FillCategorisPanel()
            'Session.Add("newrmcat", "new")
            If Session("newrmcat") = "new" Then
                ShowRecordFilldetail_again()
                Session.Remove("newrmcat")
            End If
        End If
            If txtaccom_extra.Value = "A" Then
                PanelCategories.GroupingText = "Accomodation Categories"
            Else
                PanelCategories.GroupingText = "Supplement Categories"
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
            objUtils.WritErrorLog("SupCats.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Private Function supcatcode() As Boolean

        Try
            strSqlQry = "select * from view_partyrmcatcode where partycode='" & txtCode.Value & "'"


            Dim MyGrpDS As New DataSet
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyGrpDS, "view_partyrmcatcode")

            Dim GVRow1 As GridViewRow
            Dim CHK1 As HtmlInputCheckBox

            If MyGrpDS.Tables(0).Rows.Count > 0 Then


                For Each GVRow1 In Gv_Categories.Rows
                    CHK1 = GVRow1.FindControl("ChkSelect")
                    If CHK1.Checked = False Then
                        For i As Integer = 0 To MyGrpDS.Tables(0).Rows.Count - 1

                            If GVRow1.Cells(2).Text = MyGrpDS.Tables(0).Rows(i).Item("rmcatcode") Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' You are not allowed to uncheck " + GVRow1.Cells(2).Text + " ,already used in " + MyGrpDS.Tables(0).Rows(i).Item("form") + " ')", True)

                                Return False
                                Exit Function


                            End If
                        Next

                    End If
                Next

            Else
                Return True
            End If


            Return True


        Catch ex As Exception

        End Try
    End Function

    Protected Sub BtnSaveCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            'If supcatcode() = False Then
            '    Exit Sub
            'End If
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp
            Dim flag As Boolean = False
            Dim GVRow As GridViewRow
            Dim CHK As HtmlInputCheckBox

            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then
                    For Each GVRow In Gv_Categories.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            flag = True
                            Exit For
                        End If
                    Next
                    If flag = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one category type.');", True)
                        Exit Sub
                    End If

                    '--------------------------------------------------------------------------------
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.VarChar, 10)).Value = CType(txtaccom_extra.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()


                    For Each GVRow In Gv_Categories.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyrmcat", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(2).Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@showmain", SqlDbType.Int, 4)).Value = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    'sp_update_crosstab_headernew
                    mySqlCmd = New SqlCommand("sp_update_crosstab_headernew", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                ElseIf Session("SupState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.VarChar, 10)).Value = CType(txtaccom_extra.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
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
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
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
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("SupState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
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
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("SupCats.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnCancelCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
    Protected Sub BtnDeSelectAllCategories_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_Categories.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
        Next
    End Sub

    Protected Sub BtnSelectAllCategories_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        For Each GvRow In Gv_Categories.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
        Next
    End Sub

    '    Protected Sub gv_Roomtype_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
    '        Gv_RoomType.PageIndex = e.NewPageIndex
    '        FillRommTypePanel()
    '    End Sub

    '#Region " Protected Sub gv_roomtype_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    '    Protected Sub gv_Roomtype_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
    '        'FillGrid(e.SortExpression, e.SortDirection)
    '        Session.Add("strsortexpression", e.SortExpression)
    '        FillRommTypePanel()
    '    End Sub
    '#End Region

    Private Sub FillCategorisPanel()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "'"
            If accom_extra = "A" Then
                strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra in ('A','C') ) "
            Else
                strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra not in ('A','C')) "
            End If
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyDS_Category As New DataSet
                If accom_extra = "A" Then
                    strSqlQry = "select * from rmcatmast where active=1 and sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "') " _
                        & " and accom_extra in ('A','C')"
                Else
                    strSqlQry = "select * from rmcatmast where active=1 and sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "') " _
                        & " and accom_extra not in ('A','C')"

                End If

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Category, "rmcatmast")
                Gv_Categories.DataSource = MyDS_Category
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
            Else
                mySqlConn.Close()
                If accom_extra = "A" Then
                    strSqlQry = "select  *,0 as orderby from rmcatmast where  rmcatcode " _
                        & " in  ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' ) " _
                        & " and accom_extra in ('A','C')  union  " _
                        & " select  *,1 as orderby from rmcatmast  where active=1 and rmcatmast.sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value & "') " _
                        & " and rmcatcode not in  ( select rmcatcode from partyrmcat " _
                        & " where partycode='" & txtCode.Value & "')  " _
                        & " and accom_extra in ('A','C') order by orderby, rankorder"
                Else
                    strSqlQry = "select  *,0 as orderby from rmcatmast where  rmcatcode " _
                        & " in  ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' ) " _
                        & " and accom_extra not in ('A','C')  union  " _
                        & " select  *,1 as orderby from rmcatmast  where active=1 and rmcatmast.sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value & "') " _
                        & " and rmcatcode not in  ( select rmcatcode from partyrmcat " _
                        & " where partycode='" & txtCode.Value & "')  " _
                        & " and rmcatmast.accom_extra not in ('A','C') order by orderby, rankorder"

                End If
                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "rmcatmast")
                Gv_Categories.DataSource = MyCatDS
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                If accom_extra = "A" Then
                    strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra in ('A','C')) "
                Else
                    strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra not in ('A','C')) "
                End If
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrCategory(i) = mySqlReader("rmcatcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To Gv_Categories.Rows.Count
                    For Each GvRow In Gv_Categories.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrCategory(i) = GvRow.Cells(2).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtaccom_extra.Value = "A" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupCats','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupSupplement','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
    End Sub

    Protected Sub btnfilldetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilldetail.Click
        If ddlSuppierCD.Value <> "[Select]" Or ddlSuppierCD.Value <> "" Then
            ShowRecordFilldetail()
        End If
    End Sub
    Private Sub ShowRecordFilldetail()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select rmcatcode from partyrmcat where partycode='" & ddlSuppierNM.Value & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyDS_Category As New DataSet
                strSqlQry = "select * from rmcatmast where active=1 and sptypecode in ( select  sptypecode from partymast where  partycode='" & ddlSuppierNM.Value & "')"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Category, "rmcatmast")
                Gv_Categories.DataSource = MyDS_Category
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
            Else
                mySqlConn.Close()

                strSqlQry = "select  *,0 as orderby from rmcatmast where  rmcatcode   in  ( select rmcatcode from partyrmcat where partycode='" & ddlSuppierNM.Value & "' ) union  " & _
                                          "select  *,1 as orderby from rmcatmast  where active=1 and rmcatmast.sptypecode in ( select  sptypecode from partymast where  partycode='" & ddlSuppierNM.Value & "') and rmcatcode not in  ( select rmcatcode from partyrmcat where partycode='" & ddlSuppierNM.Value & "') order by orderby"

                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "rmcatmast")
                Gv_Categories.DataSource = MyCatDS
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select rmcatcode from partyrmcat where partycode='" & ddlSuppierNM.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrCategory(i) = mySqlReader("rmcatcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To Gv_Categories.Rows.Count
                    For Each GvRow In Gv_Categories.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrCategory(i) = GvRow.Cells(2).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub
    Private Sub ShowRecordFilldetail_again()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "'"
            If accom_extra = "A" Then
                strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra in ('A','C') ) "
            Else
                strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra not in ('A','C')) "
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyDS_Category As New DataSet
                strSqlQry = "select * from rmcatmast where active=1 and sptypecode in ( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "')"
                If accom_extra = "A" Then
                    strSqlQry = "select * from rmcatmast where active=1 and sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "') " _
                        & " and accom_extra in ('A','C')"
                Else
                    strSqlQry = "select * from rmcatmast where active=1 and sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "') " _
                        & " and accom_extra not in ('A','C')"

                End If

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Category, "rmcatmast")
                Gv_Categories.DataSource = MyDS_Category
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
            Else
                mySqlConn.Close()

                strSqlQry = "select  *,0 as orderby from rmcatmast where  rmcatcode   in  ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "' ) union  " & _
                                          "select  *,1 as orderby from rmcatmast  where active=1 and rmcatmast.sptypecode in ( select  sptypecode from partymast where  partycode='" & txtCode.Value.Trim & "') and rmcatcode not in  ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "') order by orderby"

                If accom_extra = "A" Then
                    strSqlQry = "select  *,0 as orderby from rmcatmast where  rmcatcode " _
                        & " in  ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' ) " _
                        & " and accom_extra in ('A','C')  union  " _
                        & " select  *,1 as orderby from rmcatmast  where active=1 and rmcatmast.sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value & "') " _
                        & " and rmcatcode not in  ( select rmcatcode from partyrmcat " _
                        & " where partycode='" & txtCode.Value & "')  " _
                        & " and accom_extra in ('A','C') order by orderby, rankorder"
                Else
                    strSqlQry = "select  *,0 as orderby from rmcatmast where  rmcatcode " _
                        & " in  ( select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' ) " _
                        & " and accom_extra not in ('A','C')  union  " _
                        & " select  *,1 as orderby from rmcatmast  where active=1 and rmcatmast.sptypecode " _
                        & " in ( select  sptypecode from partymast where  partycode='" & txtCode.Value & "') " _
                        & " and rmcatcode not in  ( select rmcatcode from partyrmcat " _
                        & " where partycode='" & txtCode.Value & "')  " _
                        & " and rmcatmast.accom_extra not in ('A','C') order by orderby, rankorder"

                End If

                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "rmcatmast")
                Gv_Categories.DataSource = MyCatDS
                Gv_Categories.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value.Trim & "' "
                If accom_extra = "A" Then
                    strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra in ('A','C')) "
                Else
                    strSqlQry = "select rmcatcode from partyrmcat where partycode='" & txtCode.Value & "' and rmcatcode in (select rmcatcode from rmcatmast where accom_extra not in ('A','C')) "
                End If

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrCategory(i) = mySqlReader("rmcatcode")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To Gv_Categories.Rows.Count
                    For Each GvRow In Gv_Categories.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        If arrCategory(i) = GvRow.Cells(2).Text Then
                            chk.Checked = True
                        End If
                    Next
                Next
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub


    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("RoomCategories.aspx", False)
        Dim strpop As String = ""
        ' strpop = "window.open('RoomCategories.aspx?State=New','Rmcats','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('RoomCategories.aspx?State=New&Type=" + IIf(txtaccom_extra.Value.Trim = "A", "Acc", "Supp") + "','Rmcats');"
        Session.Add("newrmcat", "new")

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class
