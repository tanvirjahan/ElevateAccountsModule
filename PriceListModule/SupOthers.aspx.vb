#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupOthers
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
        PanelOtherServices.Visible = True
        ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select distinct sptypecode from sptypemast where active=1 order by sptypecode", True)
        
        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If


            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If

            objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select * from othgrpmast where active=1 order by othgrpname", True)
            objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select * from othgrpmast where active=1 order by othgrpcode", True)


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
                lblHeading.Text = "Add New Supplier" + " - " + PanelOtherServices.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + PanelOtherServices.GroupingText
                BtnSaveOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnSaveOther.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtCode)
                lblHeading.Text = "Edit Supplier" + " - " + PanelOtherServices.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + PanelOtherServices.GroupingText
                BtnSaveOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")

            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                lblHeading.Text = "View Supplier" + " - " + PanelOtherServices.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + PanelOtherServices.GroupingText
                BtnSaveOther.Visible = False
                BtnSaveOther.Text = "Return to Search"

            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True

                lblHeading.Text = "Delete Supplier" + " - " + PanelOtherServices.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + PanelOtherServices.GroupingText
                BtnSaveOther.Text = "Delete"
                BtnSaveOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
            End If
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If
            BtnCancelOther.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            FillOtherGroups()
            '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlGroupCode.ClientID + "');", True)
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
            objUtils.WritErrorLog("SupOthers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Private Sub FillOtherGroups()
        Dim GrupCode As String = ""
        Try

            'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select * from othgrpmast where active=1  order by othgrpname", True)
            'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select * from othgrpmast where active=1  order by othgrpcode", True)

            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupCode, "othgrpcode", "othgrpname", "select * from othgrpmast where active=1  order by othgrpcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlGroupName, "othgrpname", "othgrpcode", "select * from othgrpmast where active=1  order by othgrpname", True)

            Dim MyGroupDS As New DataSet
            strSqlQry = "select 0 othselected,o.othgrpcode,o.othgrpname from othgrpmast o where o.active = 1 "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyGroupDS, "othgrpmast")
            GV_Group.DataSource = MyGroupDS
            GV_Group.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()


            strSqlQry = "select p.othgrpcode from partyothgrp p where p.partycode='" & txtCode.Value & "'"
            Dim gvrow As GridViewRow
            Dim chk As CheckBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While mySqlReader.Read = True
                For Each gvrow In GV_Group.Rows
                    If gvrow.Cells(1).Text = mySqlReader("othgrpcode") Then
                        chk = gvrow.FindControl("CheckBox1")
                        chk.Checked = True
                        Exit For
                    End If
                Next
            End While
            MyAdapter.Dispose()
            mySqlConn.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub

    Protected Sub BtnSaveOther_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp

            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then

                    If ValidateOtherType() = False Then
                        Exit Sub
                    End If
                    Dim GVRow As GridViewRow
                    Dim CHK As CheckBox

                    Dim cnt_grp As Integer = 0

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------            First Delete All Records From Details Table   -----------------------------
                    mySqlCmd = New SqlCommand("sp_del_partyothgrp_group", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothtyp_group", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothcat_group", mySqlConn, sqlTrans)

                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("sp_add_partyothgrp", mySqlConn, sqlTrans)

                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()


                    For Each GVRow In GvTypes.Rows
                        CHK = GVRow.FindControl("CheckBox2")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyothtyp", mySqlConn, sqlTrans)

                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    '---------------        Type        ---------------------------------------

                    'sp_add_partyothtyp
                    '---------------        Type        ---------------------------------------
                    For Each GVRow In Gv_Category.Rows
                        CHK = GVRow.FindControl("CheckBox3")
                        If CHK.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_partyothcat", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = HttpUtility.HtmlDecode(GVRow.Cells(1).Text)

                            'CType(GVRow.Cells(1).Text, String)

                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    '-----------------------------------------------------------------------------------------
                    sqlTrans.Commit()    'SQl Tarn Commit
                    mySqlConn.Close()

                    'sp_add_partyothcat
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
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmtyp", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyrmcat", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.VarChar, 10)).Value = ""
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
                    'mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
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

                    sqlTrans.Commit()    'SQl Tarn Commit
                End If
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
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                mySqlConn.Close()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupOthers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub BtnCancelOther_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
    
    Private Function ValidateOtherType() As Boolean
        Dim chk As CheckBox
        Dim flag As Boolean = False
        Dim strothtypcode As String = ""
        Dim strothcatcode As String = ""
        Try
            'GV_Group   ChkGroup
            'GvTypes
            'For Each GvRow In GV_Group.Rows
            '    ChkServer = GvRow.FindControl("CheckBox1")
            '    If ChkServer.Checked = True Then
            '        flag = True
            '        Exit For
            '    End If
            'Next

            'If flag = False Then
            If ddlGroupCode.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one group type.');", True)
                ValidateOtherType = False
                Exit Function
            End If

            '---------------------------------------------------------------
            flag = False
            For Each GvRow In GvTypes.Rows
                chk = GvRow.FindControl("CheckBox2")
                If chk.Checked = True Then
                    flag = True
                    Exit For
                End If
            Next
            If flag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one type.');", True)
                ValidateOtherType = False
                Exit Function
            End If
            'for other services -othtypecode
            For Each GvRow In GvTypes.Rows
                chk = GvRow.FindControl("CheckBox2")
                If chk.Checked = False Then
                    strothtypcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select d.othtypcode from othplist_costd d,othplisth h " _
                    & " where h.oplistcode=d.oplistcode and h.partycode='" + CType(Session("SupRefCode"), String) + "' and d.othtypcode='" + GvRow.Cells(1).Text _
                    + "' and h.othgrpcode='" + ddlGroupCode.SelectedItem.Text + "' and isnull(d.ocostprice,0)<>0")
                    If strothtypcode <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot uncheck " + strothtypcode + ", used in price list.');", True)
                        ValidateOtherType = False
                        Exit Function
                    End If
                End If
            Next
            'for tranfers-othtypecode
            For Each GvRow In GvTypes.Rows
                chk = GvRow.FindControl("CheckBox2")
                If chk.Checked = False Then
                    strothtypcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select d.othtypcode from trfplist_costd d,trfplisth h " _
                    & " where h.tplistcode=d.tplistcode and h.partycode='" + CType(Session("SupRefCode"), String) + "' and d.othtypcode='" + GvRow.Cells(1).Text _
                    + "' and h.othgrpcode='" + ddlGroupCode.SelectedItem.Text + "' and isnull(d.tcostprice,0)<>0")
                    If strothtypcode <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot uncheck " + strothtypcode + ", used in price list.');", True)
                        ValidateOtherType = False
                        Exit Function
                    End If
                End If
            Next

            '---------------------------------------------------------------

            flag = False
            For Each GvRow In Gv_Category.Rows
                chk = GvRow.FindControl("CheckBox3")
                If chk.Checked = True Then
                    flag = True
                    Exit For
                End If
            Next
            If flag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one category.');", True)
                ValidateOtherType = False
                Exit Function
            End If

            'for other services -catcode
            For Each GvRow In Gv_Category.Rows
                chk = GvRow.FindControl("CheckBox3")
                If chk.Checked = False Then
                    strothcatcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select d.othcatcode from othplist_costd d,othplisth h " _
                    & " where h.oplistcode=d.oplistcode and h.partycode='" + CType(Session("SupRefCode"), String) + "' and d.othcatcode='" + HttpUtility.HtmlDecode(GvRow.Cells(1).Text) _
                    + "' and h.othgrpcode='" + ddlGroupCode.SelectedItem.Text + "' and isnull(d.ocostprice,0)<>0")
                    If strothcatcode <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot uncheck " + strothcatcode + ", used in price list.');", True)
                        ValidateOtherType = False
                        Exit Function
                    End If
                End If
            Next
            'for transfers - catcode
            For Each GvRow In Gv_Category.Rows
                chk = GvRow.FindControl("CheckBox3")
                If chk.Checked = False Then
                    strothcatcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select d.othcatcode from trfplist_costd d,trfplisth h " _
                    & " where h.tplistcode=d.tplistcode and h.partycode='" + CType(Session("SupRefCode"), String) + "' and d.othcatcode='" + HttpUtility.HtmlDecode(GvRow.Cells(1).Text) _
                    + "' and h.othgrpcode='" + ddlGroupCode.SelectedItem.Text + "' and isnull(d.tcostprice,0)<>0")
                    If strothcatcode <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot uncheck " + strothcatcode + ", used in price list.');", True)
                        ValidateOtherType = False
                        Exit Function
                    End If
                End If
            Next

            ValidateOtherType = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
    Private Sub GridOthSelect(ByVal othgrpcode As String)
        Dim gvrow As GridViewRow
        Dim chk As CheckBox
        For Each gvrow In GV_Group.Rows
            If gvrow.Cells(1).Text = othgrpcode Then
                chk = gvrow.FindControl("CheckBox1")
                chk.Checked = True
                Exit For
            End If
        Next
    End Sub
    Private Sub FillOthTypCat(ByVal othgrpcode As String)
        Dim MyTypeDS As New DataSet
        Dim MyCatDS As New DataSet
        strSqlQry = "select   0 othtypselected,o.othtypcode,o.othtypname from othtypmast o  where o.active = 1 and o.othgrpcode='" & othgrpcode & "' group by o.othtypcode,o.othtypname,o.rankorder  order by o.rankorder"

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyTypeDS, "othtypmast")
        GvTypes.DataSource = MyTypeDS
        GvTypes.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()

        strSqlQry = "select p.othtypcode from partyothtyp p where p.partycode='" & txtCode.Value & "' and " _
        & " p.othgrpcode='" & othgrpcode & "'"
        Dim gvrow As GridViewRow
        Dim chk As CheckBox
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        While mySqlReader.Read = True
            For Each gvrow In GvTypes.Rows
                If gvrow.Cells(1).Text = mySqlReader("othtypcode") Then
                    chk = gvrow.FindControl("CheckBox2")
                    chk.Checked = True
                    Exit For
                End If
            Next
        End While
        mySqlReader.Dispose()
        mySqlConn.Close()

        strSqlQry = "select distinct  case when p.othcatcode is null then 0 else 1 end othcatselected,o.othcatcode,o.othcatname" _
        & " from othcatmast o left outer join partyothcat p on o.othcatcode=p.othcatcode " _
        & " where(o.active = 1 and o.othgrpcode='" & othgrpcode & "')"
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyCatDS, "othcatmast")
        Gv_Category.DataSource = MyCatDS
        Gv_Category.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()

        strSqlQry = "select p.othcatcode from partyothcat p where p.partycode='" & txtCode.Value & "' and " _
        & " p.othgrpcode='" & othgrpcode & "'"
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        While mySqlReader.Read = True
            For Each gvrow In Gv_Category.Rows
                If HttpUtility.HtmlDecode(gvrow.Cells(1).Text) = mySqlReader("othcatcode") Then
                    chk = gvrow.FindControl("CheckBox3")
                    chk.Checked = True
                    Exit For
                End If
            Next
        End While
        MyAdapter.Dispose()
        mySqlConn.Close()

    End Sub


    Protected Sub ddlGroupCode_SelectedIndexChanged(Optional ByVal sender As Object = Nothing, Optional ByVal e As System.EventArgs = Nothing)
        If ddlGroupCode.SelectedValue <> "[Select]" Then
            ddlGroupName.SelectedValue = ddlGroupCode.SelectedItem.Text
            GridOthSelect(ddlGroupCode.SelectedItem.Text)
            FillOthTypCat(ddlGroupCode.SelectedItem.Text)
        Else
            'called from delete group button, so refresh the groups grid
            ddlGroupName.SelectedValue = "[Select]"
            Dim gvrow As GridViewRow
            Dim chk As CheckBox
            For Each gvrow In GV_Group.Rows
                chk = gvrow.FindControl("CheckBox1")
                chk.Checked = False
            Next

            strSqlQry = "select p.othgrpcode from partyothgrp p where p.partycode='" & txtCode.Value & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While mySqlReader.Read = True
                For Each gvrow In GV_Group.Rows
                    If gvrow.Cells(1).Text = mySqlReader("othgrpcode") Then
                        chk = gvrow.FindControl("CheckBox1")
                        chk.Checked = True
                        Exit For
                    End If
                Next
            End While
            mySqlReader.Dispose()
            mySqlConn.Close()
            'reset the other types and categories to blank
            Dim MyTypeDS As New DataSet
            Dim MyCatDS As New DataSet
            strSqlQry = "select 0 othtypselected,o.othtypcode,o.othtypname from othtypmast o  where o.active = 1 and o.othgrpcode=''"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyTypeDS, "othtypmast")
            GvTypes.DataSource = MyTypeDS
            GvTypes.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()

            strSqlQry = "select 0 othcatselected,o.othcatcode,o.othcatname from othcatmast o  where o.active = 1 and o.othgrpcode=''"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyCatDS, "othcatmast")
            Gv_Category.DataSource = MyCatDS
            Gv_Category.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()

        End If

    End Sub

    Protected Sub ddlGroupName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroupName.SelectedIndexChanged
        If ddlGroupName.SelectedValue <> "[Select]" Then
            ddlGroupCode.SelectedValue = ddlGroupName.SelectedItem.Text
            FillOthTypCat(ddlGroupName.SelectedValue)
        End If

    End Sub
    Protected Sub Btn_DelOthGrp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then

                    If ddlGroupCode.SelectedValue = "[Select]" Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------            First Delete All Records From Details Table   -----------------------------
                    mySqlCmd = New SqlCommand("sp_del_partyothgrp_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothtyp_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_partyothcat_group", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()


                    '-----------------------------------------------------------------------------------------
                    sqlTrans.Commit()    'SQl Tarn Commit
                    mySqlConn.Close()
                    ddlGroupCode.SelectedValue = "[Select]"
                    ddlGroupCode_SelectedIndexChanged()
                    'sp_add_partyothcat
                End If
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
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                mySqlConn.Close()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupOthers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

   
    Protected Sub BtnSelectothtype_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        For Each GvRow In GvTypes.Rows
            chk = GvRow.FindControl("CheckBox2")
            chk.Checked = True
        Next

    End Sub

    Protected Sub BtnSelectothcat_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        For Each GvRow In Gv_Category.Rows
            chk = GvRow.FindControl("CheckBox3")
            chk.Checked = True
        Next
    End Sub

    Protected Sub BtnDeSelectothtype_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        For Each GvRow In GvTypes.Rows
            chk = GvRow.FindControl("CheckBox2")
            chk.Checked = False
        Next

    End Sub

    Protected Sub BtnDeSelectothcat_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        For Each GvRow In Gv_Category.Rows
            chk = GvRow.FindControl("CheckBox3")
            chk.Checked = False
        Next
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupOthers','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnfilldetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilldetail.Click
        If ddlGroupName.SelectedValue <> "[Select]" Then
            ddlGroupCode.SelectedValue = ddlGroupName.SelectedItem.Text
            ShowRecordFilldetail()
            ShowRecordFillOthTypCat(ddlGroupName.SelectedValue)
        End If
    End Sub
    Private Sub ShowRecordFilldetail()
        Dim GrupCode As String = ""
        Try

            Dim MyGroupDS As New DataSet
            strSqlQry = "select 0 othselected,o.othgrpcode,o.othgrpname from othgrpmast o where o.active = 1 "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyGroupDS, "othgrpmast")
            GV_Group.DataSource = MyGroupDS
            GV_Group.DataBind()
            MyAdapter.Dispose()
            mySqlConn.Close()


            strSqlQry = "select p.othgrpcode from partyothgrp p where p.partycode='" & ddlSuppierNM.Value & "'"
            Dim gvrow As GridViewRow
            Dim chk As CheckBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While mySqlReader.Read = True
                For Each gvrow In GV_Group.Rows
                    If gvrow.Cells(1).Text = mySqlReader("othgrpcode") Then
                        chk = gvrow.FindControl("CheckBox1")
                        chk.Checked = True
                        Exit For
                    End If
                Next
            End While
            MyAdapter.Dispose()
            mySqlConn.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub

    Private Sub ShowRecordFillOthTypCat(ByVal othgrpcode As String)
        Dim MyTypeDS As New DataSet
        Dim MyCatDS As New DataSet
        strSqlQry = "select  distinct 0 othtypselected,o.othtypcode,o.othtypname from othtypmast o  where o.active = 1 and o.othgrpcode='" & othgrpcode & "'"

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyTypeDS, "othtypmast")
        GvTypes.DataSource = MyTypeDS
        GvTypes.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()

        strSqlQry = "select p.othtypcode from partyothtyp p where p.partycode='" & ddlSuppierNM.Value & "' and " _
        & " p.othgrpcode='" & othgrpcode & "'"
        Dim gvrow As GridViewRow
        Dim chk As CheckBox
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        While mySqlReader.Read = True
            For Each gvrow In GvTypes.Rows
                If gvrow.Cells(1).Text = mySqlReader("othtypcode") Then
                    chk = gvrow.FindControl("CheckBox2")
                    chk.Checked = True
                    Exit For
                End If
            Next
        End While
        mySqlReader.Dispose()
        mySqlConn.Close()

        strSqlQry = "select distinct  case when p.othcatcode is null then 0 else 1 end othcatselected,o.othcatcode,o.othcatname" _
        & " from othcatmast o left outer join partyothcat p on o.othcatcode=p.othcatcode " _
        & " where(o.active = 1 and o.othgrpcode='" & othgrpcode & "')"
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        MyAdapter.Fill(MyCatDS, "othcatmast")
        Gv_Category.DataSource = MyCatDS
        Gv_Category.DataBind()
        MyAdapter.Dispose()
        mySqlConn.Close()

        strSqlQry = "select p.othcatcode from partyothcat p where p.partycode='" & ddlSuppierNM.Value & "' and " _
        & " p.othgrpcode='" & othgrpcode & "'"
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
        While mySqlReader.Read = True
            For Each gvrow In Gv_Category.Rows
                If gvrow.Cells(1).Text = mySqlReader("othcatcode") Then
                    chk = gvrow.FindControl("CheckBox3")
                    chk.Checked = True
                    Exit For
                End If
            Next
        End While
        MyAdapter.Dispose()
        mySqlConn.Close()

    End Sub
End Class
