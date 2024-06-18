#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupSplEvents
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
             Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)
            Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
            Me.whotelatbcontrol.menuidval = ObjUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

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
            Session("partycode") = Nothing

            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If
            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
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
            objUtils.WritErrorLog("SupSplEvents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "party_invtype", "partycode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Supplier is already used for Inventory, cannot delete this Supplier');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region
    Private Sub FillSpecialPanel()

        Dim GvRow As GridViewRow
        Dim chk As HtmlInputCheckBox
        Dim spname As TextBox
        Dim spcode As TextBox
        Dim strsptype As String
        Try
            strSqlQry = "select * from party_splevents where partycode='" & txtCode.Value.Trim & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            strsptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", CType(Session("SupRefCode"), String))
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                'If ddlType.Value <> "[Select]" Then
                If strsptype <> "" Then
                    Dim MyDS_Special As New DataSet
                    'strSqlQry = "select * from spleventsmast where active=1 and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & "'"
                    'strSqlQry = "select * from spleventsmast where active=1 and sptypecode='" & strsptype & "'"
                    strSqlQry = "select * from party_splevents where partycode='" & txtCode.Value & "'  "
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    MyAdapter.Fill(MyDS_Special, "party_splevents")
                    GV_SpecialEvent.DataSource = MyDS_Special
                    GV_SpecialEvent.DataBind()
                    MyAdapter.Dispose()
                    mySqlConn.Close()
                End If

            Else
                ''mySqlConn.Close()

                ''               strSqlQry = "select  * from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "' and  spleventcode   in  ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "' ) union " & _
                ''                            "select *  from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "'  and  spleventcode not in ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "') "
                'strSqlQry = "select *,0 as orderby from spleventsmast where sptypecode ='" & strsptype & "' and  spleventcode   in  ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "' ) union " & _
                '            "select *,1 as orderby  from spleventsmast where sptypecode ='" & strsptype & "'  and  spleventcode not in ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "') order by orderby"

                strSqlQry = "select * from party_splevents where partycode='" & txtCode.Value & "'  "
                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "party_splevents")
                GV_SpecialEvent.DataSource = MyCatDS
                GV_SpecialEvent.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()
                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select spleventcode,inactive,spleventname from party_splevents where partycode='" & txtCode.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)

                i = 0
                While mySqlReader.Read = True
                    If CType(mySqlReader("inactive"), Integer) = 0 Then
                        arrCategory(i) = mySqlReader("spleventcode")
                        i = i + 1
                    End If
                End While



                MyAdapter.Dispose()
                mySqlConn.Close()


                For i = 0 To GV_SpecialEvent.Rows.Count
                    For Each GvRow In GV_SpecialEvent.Rows
                        chk = GvRow.FindControl("ChkSelect")
                        spcode = GvRow.FindControl("txtspcode")
                        spname = GvRow.FindControl("txtspname")
                        If (arrCategory(i) <> "" Or arrCategory(i) <> Nothing) Then
                            If arrCategory(i) = spcode.Text.Trim Then
                                chk.Checked = True
                                i = i + 1
                            End If
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
    Private Function spleventcatcode() As Boolean

        Try
            strSqlQry = "select * from view_splevents where partycode='" & txtCode.Value & "'"


            Dim MyGrpDS As New DataSet
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyGrpDS, "view_splevents")

            Dim GVRow1 As GridViewRow
            Dim CHK1 As HtmlInputCheckBox
            Dim txtspcode As TextBox

            If MyGrpDS.Tables(0).Rows.Count > 0 Then

               

                    For Each GVRow1 In GV_SpecialEvent.Rows
                    CHK1 = GVRow1.FindControl("ChkSelect")
                    txtspcode = GVRow1.FindControl("txtspcode")
                        If CHK1.Checked = False Then
                            For i As Integer = 0 To MyGrpDS.Tables(0).Rows.Count - 1

                            If txtspcode.Text = MyGrpDS.Tables(0).Rows(i).Item("rmcatcode") Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' You are not allowed to uncheck " + txtspcode.Text + " ,already used in " + MyGrpDS.Tables(0).Rows(i).Item("form") + " ')", True)

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
    Protected Sub BtnSaveSpecialEve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveSpecialEve.Click
        Try
            'sp_del_partyrmtyp 
            'sp_add_partyrmtyp

            If spleventcatcode() = False Then
                Exit Sub
            End If


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
                    'Dim GVRow As GridViewRow
                    'Dim CHK As HtmlInputCheckBox
                    For Each GVRow In GV_SpecialEvent.Rows
                        CHK = GVRow.FindControl("ChkSelect")
                        If CHK.Checked = True Then
                            flag = True
                            Exit For
                        End If
                        spcode = GVRow.FindControl("txtspcode")
                        spname = GVRow.FindControl("txtspname")
                    Next
                    If flag = False Then 'they wanted to remove 'if nothing checked , then will check
                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check atleast one special event type.');", True)
                        'Exit Sub

                        Dim str As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_contracts_specialevents_detail where partycode='" & txtCode.Value & "'")
                        If str <> "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Remove. Already used for Pricelist!!');", True)
                            Exit Sub
                        End If

                        Dim str1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 rs.eventcode  from reservation_online_hotels_detail rd inner join reservation_online_hotels_detail_splevents rs on rd.requestid =rs.requestid and rd.hotellineno = rs.hotellineno And rd.rmtyplineno = rs.rmtyplineno where partycode ='" & txtCode.Value & "'")
                        If str1 <> "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Remove. Already used in Reservation!!');", True)
                            Exit Sub
                        End If
                    End If
                    '-----------------------------------------------------------------------------------




                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()


                    Dim txtspcode As TextBox
                    Dim txtspname As TextBox

                    For Each GVRow In GV_SpecialEvent.Rows
                        CHK = GVRow.FindControl("ChkSelect")

                        txtspcode = GVRow.FindControl("txtspcode")
                        txtspname = GVRow.FindControl("txtspname")

                        'If CHK.Checked = True Then
                        mySqlCmd = New SqlCommand("sp_add_partysplevents", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(GVRow.Cells(2).Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(txtspcode.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(txtspname.Text, String)
                        If CHK.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@inactive ", SqlDbType.VarChar, 200)).Value = 0
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@inactive ", SqlDbType.VarChar, 200)).Value = 1
                        End If
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
                    mySqlCmd = New SqlCommand("sp_del_partymast_invtpe", mySqlConn, sqlTrans)
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupSplEvents','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
            strSqlQry = "select * from party_splevents where partycode='" & ddlSuppierNM.Value & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader
            strsptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", CType(Session("SupRefCode"), String))
            If mySqlReader.Read = False Then
                mySqlConn.Close()
                'If ddlType.Value <> "[Select]" Then
                If strsptype <> "" Then
                    Dim MyDS_Special As New DataSet
                    'strSqlQry = "select * from spleventsmast where active=1 and sptypecode='" & ddlType.Items(ddlType.SelectedIndex).Text & "'"
                    strSqlQry = "select * from party_splevents where partycode='" & ddlSuppierNM.Value & "'"
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    MyAdapter.Fill(MyDS_Special, "spleventsmast")
                    GV_SpecialEvent.DataSource = MyDS_Special
                    GV_SpecialEvent.DataBind()
                    MyAdapter.Dispose()
                    mySqlConn.Close()
                End If

            Else
                'mySqlConn.Close()

                '               strSqlQry = "select  * from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "' and  spleventcode   in  ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "' ) union " & _
                '                            "select *  from spleventsmast where sptypecode ='" & ddlType.Items(ddlType.SelectedIndex).Text & "'  and  spleventcode not in ( select spleventcode from party_splevents where partycode='" & txtCode.Value.Trim & "') "
                strSqlQry = "select * from party_splevents where partycode='" & ddlSuppierNM.Value & "'"

                Dim MyCatDS As New DataSet
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(MyCatDS, "spleventsmast")
                GV_SpecialEvent.DataSource = MyCatDS
                GV_SpecialEvent.DataBind()
                MyAdapter.Dispose()
                mySqlConn.Close()

                MyAdapter.Dispose()
                mySqlConn.Close()


                Dim arrCategory(MyCatDS.Tables(0).Rows.Count + 1) As String
                Dim i As Long = 0
                strSqlQry = "select spleventcode,inactive from party_splevents where partycode='" & ddlSuppierNM.Value & "' "
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                i = 0
                While mySqlReader.Read = True
                    If CType(mySqlReader("inactive"), Integer) = 0 Then
                        arrCategory(i) = mySqlReader("spleventcode")
                        i = i + 1
                    End If
                End While
                MyAdapter.Dispose()
                mySqlConn.Close()

                i = 0
                For i = 0 To GV_SpecialEvent.Rows.Count
                    For Each GvRow In GV_SpecialEvent.Rows
                        chk = GvRow.FindControl("ChkSelect")

                        spcode = GvRow.FindControl("txtspcode")
                        spname = GvRow.FindControl("txtspname")

                        If (arrCategory(i) <> "" Or arrCategory(i) <> Nothing) Then
                            If arrCategory(i) = spcode.Text.Trim Then
                                chk.Checked = True
                                i = i + 1
                            End If
                        End If
                    Next
                Next
            End If
            disablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try

    End Sub

    Protected Sub BtnAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddLine.Click
        AddLines()
    End Sub
    Private Sub AddLines()

        Dim chkSelect As HtmlInputCheckBox
        Dim txtspcode As TextBox
        Dim txtspname As TextBox
        Dim newnumber As Integer
        Try
            ' Create Grid for Data Table
            ''''''''''''''''''''''''''''
            dtgridsp = New DataTable
            dtgridsp.Columns.Add("chkSelect", GetType(Integer))
            dtgridsp.Columns.Add("spleventcode", GetType(String))
            dtgridsp.Columns.Add("spleventname", GetType(String))

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
                newnumber = newnumber + 1
                dtgridsp.Rows.Add(IIf(chkSelect.Checked = True, 1, 0), txtspcode.Text, txtspname.Text)
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
        Dim chkSelect As HtmlInputCheckBox
        Dim txtspcode As TextBox
        Dim txtspname As TextBox
        For Each GvRow In GV_SpecialEvent.Rows
            chkSelect = GvRow.FindControl("ChkSelect")
            txtspcode = GvRow.FindControl("txtspcode")
            txtspname = GvRow.FindControl("txtspname")
            If chkSelect.Checked = True Then
                Dim str As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_contracts_specialevents_detail d(nolock) ,party_splevents h(nolock) where d.spleventcode=h.spleventcode and   d.spleventcode='" & txtspcode.Text & "' and  h.partycode='" & txtCode.Value & "'")
                If str <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Remove. Already used for Pricelist!!');", True)
                    Exit Sub
                End If
            End If
        Next
        deleteLines()
    End Sub
    Private Sub deleteLines()

        Dim chkSelect As HtmlInputCheckBox
        Dim txtspcode As TextBox
        Dim txtspname As TextBox

        Try


            ' Create Grid for Data Table
            ''''''''''''''''''''''''''''
            dtgridsp = New DataTable
            dtgridsp.Columns.Add("chkSelect", GetType(Integer))
            dtgridsp.Columns.Add("spleventcode", GetType(String))
            dtgridsp.Columns.Add("spleventname", GetType(String))

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
                If chkSelect.Checked = False Then
                    dtgridsp.Rows.Add(IIf(chkSelect.Checked = True, 1, 0), txtspcode.Text, txtspname.Text)
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
