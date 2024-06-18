#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class PriceListModule_Default
    Inherits System.Web.UI.Page
    Dim ObjUser As New clsUser



   

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
    Dim txtspcode As TextBox
    Dim txtspname As TextBox
    Dim count As Integer

    Dim dtgridsp As DataTable
    Dim ChkSelect As HtmlInputCheckBox
    Dim SEventCode As New ArrayList
    Dim SEventsExtra As New ArrayList
    Dim n As Integer = 0
    Dim k As Integer = 0
    Dim delrows(count) As String
    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0


#End Region


        Private Sub disablegrid()
            Dim spname As TextBox
            Dim spcode As TextBox
            Dim chk As HtmlInputCheckBox
            Dim GvRow As GridViewRow
            Dim i As Integer

            i = 0
            For i = 0 To GV_SpecialEvent.Rows.Count
                For Each GvRow In GV_SpecialEvent.Rows
                    spcode = GvRow.FindControl("txtspcode")
                    spname = GvRow.FindControl("txtspname")
                    chk = GvRow.FindControl("ChkSelect")
                If CType(Session("SupState"), String) = "View" Then
                    spcode.ReadOnly = True
                    spname.ReadOnly = True
                    chk.Disabled = True
                ElseIf CType(Session("SupState"), String) = "Delete" Then
                    spcode.ReadOnly = True
                    spname.ReadOnly = True
                    chk.Disabled = True
                    ddlSuppierCD.Disabled = True
                    ddlSuppierNM.Disabled = True
                ElseIf CType(Session("SupState"), String) = "Edit" Then
                    spcode.ReadOnly = True
                    spname.ReadOnly = False
                    chk.Disabled = False
                Else
                    spcode.ReadOnly = False
                    spname.ReadOnly = False
                    chk.Disabled = False
                End If
                Next
            Next

        End Sub
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim RefCode As String
            PanelSpEvent.Visible = True
            ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select distinct sptypecode from sptypemast where active=1 order by sptypecode", True)

            If IsPostBack = False Then
     
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            If CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                Me.SubMenuUserControl1.menuidval = ObjUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\HotelGroups.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "1" Then
                Me.SubMenuUserControl1.menuidval = ObjUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)
            Else
                Me.SubMenuUserControl1.menuidval = ObjUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            End If
            Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)

            If CType(Request.QueryString("appid"), String) = 1 Then
                Me.whotelatbcontrol.menuidval = ObjUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))
            Else

                Me.whotelatbcontrol.menuidval = ""
            End If
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            Dim sptype As String

            If Not Session("SupRefCode") = Nothing Then
                sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" + CType(Session("SupRefCode"), String) + "'")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode='" + sptype + "' order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" + sptype + "' order by partyname", True)
            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1  order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1  order by partyname", True)
            End If

            Session("partycode") = Nothing

            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If
            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                txtspcode.ReadOnly = True
                lblHeading.Text = "Add New Supplier" + " - " + PanelSpEvent.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + PanelSpEvent.GroupingText
                BtnSaveSpecialEve.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnSaveSpecialEve.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)


                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtCode)

                lblHeading.Text = "Edit Supplier" + " - " + PanelSpEvent.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + PanelSpEvent.GroupingText
                BtnSaveSpecialEve.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")

            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)

                txtCode.Disabled = True
                txtName.Disabled = True

                lblHeading.Text = "View Supplier" + " - " + PanelSpEvent.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + PanelSpEvent.GroupingText
                BtnSaveSpecialEve.Visible = False
                BtnSaveSpecialEve.Text = "Return to Search"

            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True

                lblHeading.Text = "Delete Supplier" + " - " + PanelSpEvent.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + PanelSpEvent.GroupingText
                BtnSaveSpecialEve.Text = "Delete"
                BtnSaveSpecialEve.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
            End If
            BtnCancelSpecailEe.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            FillSpecialPanel()

            disablegrid()


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
            objUtils.WritErrorLog("SupInventory.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


    Private Sub FillSpecialPanel()

        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Try
            strSqlQry = "select * from party_invtype where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                Dim MyDS_Category As New DataSet
                Dim MyDS_Meal As New DataSet
                strSqlQry = "select * from inventory_types order by rankorder"

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Meal, "invtype")
                GV_SpecialEvent.DataSource = MyDS_Meal
                GV_SpecialEvent.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()

            Else
                mySqlConn.Close()

                'strSqlQry = "select  *,0 as orderby from mealmast where active=1 and  mealcode   in  ( select mealcode from partymeal where partycode='" & txtCode.Value & "' ) union  " & _
                '               "select  *,1 as orderby from mealmast where active=1 and mealmast.sptypecode in ( select  sptypecode from partymast where  partycode='" & txtCode.Value & "') and mealcode not in  ( select mealcode from partymeal where partycode='" & txtCode.Value & "') order by orderby"

                strSqlQry = "select invtype,rankorder, imagename from party_invtype  where partycode= '" & txtCode.Value & "'  union select invtype,rankorder ,imagename from inventory_types where invtype not in (select invtype from  party_invtype where partycode= '" & txtCode.Value & "' ) order by rankorder"

                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "invtype")
                GV_SpecialEvent.DataSource = MyCatDS
                GV_SpecialEvent.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrMeal(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select invtype from party_invtype where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrMeal(i) = mySqlReader("invtype")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To GV_SpecialEvent.Rows.Count
                    For Each GvRow In GV_SpecialEvent.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        Dim txtspcode As TextBox = GvRow.FindControl("txtspcode")
         


                        If arrMeal(i) = txtspcode.Text Then

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

    Protected Sub BtnSaveSpecialEve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveSpecialEve.Click
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp
            Dim flag As Boolean = False
            Dim GVRow As GridViewRow
            Dim CHK As HtmlInputCheckBox
            Dim spcode As TextBox
            Dim spname As TextBox

            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then


                    For Each GVRow In GV_SpecialEvent.Rows
                            
                        spcode = GVRow.FindControl("txtspcode")
                        spname = GVRow.FindControl("txtspname")
                    Next
                    'If flag = False Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one inventory type.');", True)
                    '    Exit Sub
                    'End If
                    ''-----------------------------------------------------------------------------------
                    'If checkForDuplicate() = False Then
                    '    Exit Sub
                    'End If



                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast_invtpe", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()


                    Dim txtspcode As TextBox
                    Dim txtspname As TextBox
                    Dim imagename As Image
                    For Each GVRow In GV_SpecialEvent.Rows
                        'CHK = GVRow.FindControl("ChkSelect")
                        imagename = GVRow.FindControl("imagename")
                        txtspcode = GVRow.FindControl("txtspcode")
                        txtspname = GVRow.FindControl("txtspname")

                        'If CHK.Checked = True Then
                        mySqlCmd = New SqlCommand("sp_add_partyinvtype", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(GVRow.Cells(2).Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@invtype", SqlDbType.VarChar, 20)).Value = CType(txtspcode.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.VarChar, 200)).Value = CType(txtspname.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 200)).Value = CType(imagename.ImageUrl, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                        'End If
                    Next


                ElseIf Session("SupState") = "Delete" Then
                        If checkForDeletion() = False Then
                            Exit Sub
                        End If
                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                        mySqlCmd = New SqlCommand("sp_del_partymast", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd = New SqlCommand("sp_del_partymast_invtpe", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
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
                    strscript = "window.opener.__doPostBack('SupInventoryWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupSplEvents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
        Protected Sub BtnCancelSpecailEe_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            'Response.Redirect("SupplierSearch.aspx")
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        End Sub

    'Public Function checkForDuplicate() As Boolean
    '    checkForDuplicate = True

    '    If objUtils.isDuplicatenew(Session("dbconnectionName"), "party_invtype", "rankorder", CType(txtspname.Text.Trim, String)) Then
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('prop.');", True)
    '        checkForDuplicate = False
    '        Exit Function
    '    End If

    '    checkForDuplicate = True
    'End Function

    Protected Sub BtnDeSelectAllSpEvent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        Dim spcode As TextBox
        Dim spname As TextBox

        For Each GvRow In GV_SpecialEvent.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = False
            spcode = GvRow.FindControl("txtspcode")
            spname = GvRow.FindControl("txtspname")
        Next
    End Sub

    Protected Sub BtnSelectAllSpEvent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As HtmlInputCheckBox
        Dim spcode As TextBox
        Dim spname As TextBox
        For Each GvRow In GV_SpecialEvent.Rows
            chk = GvRow.FindControl("ChkSelect")
            chk.Checked = True
            spcode = GvRow.FindControl("txtspcode")
            spname = GvRow.FindControl("txtspname")

        Next
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupInv','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnfilldetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilldetail.Click
        If ddlSuppierCD.Value <> "[Select]" Or ddlSuppierCD.Value <> "" Then
            ShowRecordFilldetail()
        End If
    End Sub
    Private Sub ShowRecordFilldetail()
        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Dim spcode As TextBox
        Dim spname As TextBox
        Dim strsptype As String
        Try
            strSqlQry = "select * from party_invtype where partycode='" & ddlSuppierNM.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            strsptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", CType(Session("SupRefCode"), String))
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                'If ddlType.Value <> "[Select]" Then

                Dim MyDS_Special As New DataSet
                'strSqlQry = "select * from spleventsmast where active=1 and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & "'"


                'strSqlQry = "select  *,0 as orderby from party_invtype where invtype   in  ( select party_invtype from party_invtype where partycode='" & ddlSuppierNM.Value & "' ) union  " & _
                '                   "select  *,1 as orderby from party_invtype and invtype.sptypecode in ( select  sptypecode from partymast where  partycode='" & ddlSuppierNM.Value & "') and invtype not in  ( select invtype from partyrmcat where partycode='" & ddlSuppierNM.Value & "') order by orderby"
                strSqlQry = "select * from inventory_types order by rankorder "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyDS_Special, "party_invtype")
                GV_SpecialEvent.DataSource = MyDS_Special
                GV_SpecialEvent.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()


            Else
                'mySqlConn.Close()

                '               strSqlQry = "select  * from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "' and  spleventcode   in  ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "' ) union " & _


                '                            "select *  from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "'  and  spleventcode not in ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "') "





                strSqlQry = "select invtype,rankorder,imagename from party_invtype  where partycode= '" & ddlSuppierNM.Value & "'  union select invtype,rankorder,imagename from inventory_types where invtype not in (select invtype from  party_invtype where partycode='" & ddlSuppierNM.Value & "') order by rankorder"
                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "party_invtype")
                GV_SpecialEvent.DataSource = MyCatDS
                GV_SpecialEvent.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select invtype from party_invtype where partycode='" & ddlSuppierNM.Value & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                While mySqlReader.Read = True
                    arrCategory(i) = mySqlReader("invtype")
                    i = i + 1
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                i = 0
                For i = 0 To GV_SpecialEvent.Rows.Count
                    For Each GvRow In GV_SpecialEvent.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        Dim txtspcode As TextBox = GvRow.FindControl("txtspcode")
                        If arrCategory(i) = txtspcode.Text Then
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
        disablegrid()

    End Sub

    Protected Sub BtnAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddLine.Click
        AddLines()
    End Sub
    Private Sub AddLines()

        Dim chkSelect As HtmlInputCheckBox
        Dim txtspcode As TextBox
        Dim txtspname As TextBox
        Dim newnumber As Integer
        Dim imagename As Image
        Try
            ' Create Grid for Data Table
            ''''''''''''''''''''''''''''
            dtgridsp = New DataTable
            dtgridsp.Columns.Add("chkSelect", GetType(Integer))
            dtgridsp.Columns.Add("invtype", GetType(String))
            dtgridsp.Columns.Add("rankorder", GetType(String))
            dtgridsp.Columns.Add("imagename", GetType(String))
            dtgridsp.AcceptChanges()
            'dtgridsp.Rows.Add(0, "", "")
            'Session("dtgridsp") = dtgridsp
            ''''''''''''''''''''''''''''

            '' Assign the control to the datatable
            'dtgridsp = Session("dtgridsp")
            newnumber = 0
            For Each GvRow In GV_SpecialEvent.Rows
                chkSelect = GvRow.FindControl("ChkSelect")
                txtspcode = GvRow.FindControl("txtspcode")
                txtspname = GvRow.FindControl("txtspname")
                imagename = GvRow.FindControl("imagename")

                newnumber = newnumber + 1
                dtgridsp.Rows.Add(IIf(chkSelect.Checked = True, 1, 0), txtspcode.Text, txtspname.Text, imagename.ImageUrl)
            Next
            newnumber = newnumber + 1
            dtgridsp.Rows.Add(0, newnumber, "")
            GV_SpecialEvent.DataSource = dtgridsp
            GV_SpecialEvent.DataBind()
            '''''''''''''''''''''''''''''''''




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub




    Protected Sub BtnDeleteLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDeleteLine.Click
        deleteLines()
    End Sub
    Private Sub deleteLines()

        Dim chkSelect As HtmlInputCheckBox
        Dim txtspcode As TextBox
        Dim txtspname As TextBox
        Dim imagename As Image
        Try
            ' Create Grid for Data Table
            ''''''''''''''''''''''''''''
            dtgridsp = New DataTable
            dtgridsp.Columns.Add("chkSelect", GetType(Integer))
            dtgridsp.Columns.Add("invtype", GetType(String))
            dtgridsp.Columns.Add("rankorder", GetType(String))
            dtgridsp.Columns.Add("imagename", GetType(String))
            dtgridsp.AcceptChanges()
            'dtgridsp.Rows.Add(0, "", "")
            'Session("dtgridsp") = dtgridsp
            ''''''''''''''''''''''''''''

            '' Assign the control to the datatable
            'dtgridsp = Session("dtgridsp")
            For Each GvRow In GV_SpecialEvent.Rows
                chkSelect = GvRow.FindControl("ChkSelect")
                txtspcode = GvRow.FindControl("txtspcode")
                txtspname = GvRow.FindControl("txtspname")
                imagename = GvRow.FindControl("imagename")
                If chkSelect.Checked = False Then
                    dtgridsp.Rows.Add(IIf(chkSelect.Checked = True, 1, 0), txtspcode.Text, txtspname.Text, imagename.ImageUrl)
                End If
            Next
            'dtgridsp.Rows.Add(0, "", "")
            GV_SpecialEvent.DataSource = dtgridsp
            GV_SpecialEvent.DataBind()
            '''''''''''''''''''''''''''''''''




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub





    
End Class
