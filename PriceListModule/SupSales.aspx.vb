
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class SupSales
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        PanelSales.Visible = True
        If IsPostBack = False Then
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If


            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            If CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) <> "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)

                Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))


            ElseIf CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 39)
                Session("supmain_suptype") = "Other Serv"
            ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "10" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "13" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "HOT" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=HOT", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "HOT" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=HOT", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))


            End If
           
         
            Dim sptype As String


            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If
            Session("partycode") = Nothing
            telepphone(txtSaleTelephone1)
            telepphone(txtSaleTelephone2)
            Numbers(txtSaleFax)
            charcters(txtCode)
            charcters(txtName)

            If CType(Session("SupState"), String) = "New" Or CType(Session("SupState"), String) = "Edit" Then
                If Not Session("SupRefCode") = Nothing Then
                    sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" + CType(Session("SupRefCode"), String) + "'")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode='" + sptype + "' order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" + sptype + "' order by partyname", True)

                    If sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=458") Then

                        If CType(Request.QueryString("appid"), String) = 1 Then
                            Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)
                            Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                            Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))
                        Else
                            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                            Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                            Me.whotelatbcontrol.menuidval = ""
                        End If


                    ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1032") Then
                        Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                        Me.whotelatbcontrol.menuidval = "" 'objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))
                    ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=564") Then
                        Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                        Me.whotelatbcontrol.menuidval = ""
                    ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1033") Then
                        Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                        Me.whotelatbcontrol.menuidval = ""

                    ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1501") Then

                        If CType(Request.QueryString("appid"), String) = 1 Then
                            Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 39)
                            Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                            Me.whotelatbcontrol.menuidval = ""
                        Else
                            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
                            Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                            Me.whotelatbcontrol.menuidval = "" 'objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

                        End If
                    End If

                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1  order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1  order by partyname", True)
                End If
                End If

            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                lblHeading.Text = "Add New Supplier" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier ?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnSaleSave.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtSaleTelephone1)
                lblHeading.Text = "Edit Supplier" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier ?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Visible = False
                BtnSaleCancel.Text = "Return to Search"
            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                lblHeading.Text = "Delete Supplier" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Text = "Delete"
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier ?')==false)return false;")
            End If
            BtnSaleCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
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


#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        Me.txtCode.Disabled = True
        Me.txtName.Disabled = True
        '------------------------------------------------------
        '---------  Sales Details ------------------------------------
        txtSaleTelephone1.Disabled = True
        txtSaleTelephone2.Disabled = True
        txtSaleMobile.Disabled = True
        txtSaleFax.Disabled = True
        txtSaleContact1.Disabled = True
        txtSaleContact2.Disabled = True
        txtSaleEmail.Disabled = True
        '------------------------END-----------------------------------
        If CType(Session("SupState"), String) = "View" Or CType(Session("SupState"), String) = "Delete" Then
            ddlSuppierCD.Disabled = True
            ddlSuppierNM.Disabled = True
            btnfilldetail.Enabled = False
        End If
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

                    '---------  Sales Details ------------------------------------

                    If IsDBNull(mySqlReader("stel1")) = False Then
                        txtSaleTelephone1.Value = mySqlReader("stel1")
                    Else
                        txtSaleTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("stel2")) = False Then
                        txtSaleTelephone2.Value = mySqlReader("stel2")
                    Else
                        txtSaleTelephone2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("smobileno")) = False Then
                        txtSaleMobile.Value = mySqlReader("smobileno")
                    Else
                        txtSaleMobile.Value = ""
                    End If

                    If IsDBNull(mySqlReader("sfax")) = False Then
                        txtSaleFax.Value = mySqlReader("sfax")
                    Else
                        txtSaleFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact1")) = False Then
                        txtSaleContact1.Value = mySqlReader("scontact1")
                    Else
                        txtSaleContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact2")) = False Then
                        txtSaleContact2.Value = mySqlReader("scontact2")
                    Else
                        txtSaleContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("semail")) = False Then
                        txtSaleEmail.Value = mySqlReader("semail")
                    Else
                        txtSaleEmail.Value = ""
                    End If
                    '------------------------END-----------------------------------

                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
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

#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region


#Region "Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaleSave.Click
        Try
            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then
                    If txtSaleEmail.Value.Trim <> "" Then
                        If EmailValidate(txtSaleEmail.Value.Trim, txtSaleEmail) = False Then
                            '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Fax field can not be blank.');", True)
                            Exit Sub
                        End If
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("SupState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatesal_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("SupState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updatesal_partymast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel1", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel2", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@smobileno", SqlDbType.VarChar, 50)).Value = CType(txtSaleMobile.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sfax", SqlDbType.VarChar, 50)).Value = CType(txtSaleFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact1", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact2", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@semail", SqlDbType.VarChar, 100)).Value = CType(txtSaleEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("SupState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_partymast ", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
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

            objUtils.WritErrorLog("SupSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region " Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean"
    Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean
        Try
            Dim email1length As Integer
            email1length = Len(email.Trim)
            If email1length > 255 Then
                'objcommon.MessageBox("email1 length is too large..please enter valid email exampele(abc@abc.com)..", Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email length is too large..please enter valid email example(abc@abc.com).');", True)
                SetFocus(txt)
                Me.Page.SetFocus(txt)
                EmailValidate = False
                Exit Function
            Else
                Dim atpos As String
                Dim dotpos As String
                Dim s1 As String
                Dim s As String
                s1 = email
                atpos = s1.LastIndexOf("@")
                dotpos = s1.LastIndexOf(".")
                s = s1.LastIndexOf(".")
                If atpos < 1 Or dotpos < 2 Or s < 4 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                    SetFocus(txt)
                    EmailValidate = False
                    Exit Function
                Else
                    Dim sp As String()
                    Dim at As String()
                    Dim dot As String()
                    Dim chkcom As String
                    Dim chkyahoo As String
                    Dim test As String
                    Dim t As String
                    sp = s1.Split(".")
                    at = s1.Split("@")
                    chkcom = sp.GetValue(sp.Length() - 1)
                    chkyahoo = at.GetValue(at.Length() - 1)
                    dot = chkyahoo.Split(".")
                    If dot.Length() > 3 Then  '' Added 3 instead of 2 shahul 09/04/18 
                        t = dot.GetValue(dot.Length() - 3)
                        test = sp.GetValue(sp.Length() - 2)
                        If test <> "co" Or chkcom.Length() > 2 Or IsNumeric(t) = True Then
                            'objutil.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    Else
                        t = dot.GetValue(dot.Length() - 2)
                        test = sp.GetValue(sp.Length() - 1)
                        If test.Length < 2 Or IsNumeric(t) = True Or IsNumeric(test) = True Then
                            'objcommon.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    End If
                End If
            End If
            EmailValidate = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function

#End Region

#Region "Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Session("State") = ""
        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
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

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupSales','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnfilldetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilldetail.Click
        If ddlSuppierCD.Value <> "[Select]" Or ddlSuppierCD.Value <> "" Then
            ShowRecordFilldetail(ddlSuppierNM.Value)
        End If
    End Sub
#Region "Private Sub ShowRecordFilldetail(ByVal RefCode As String)"
    Private Sub ShowRecordFilldetail(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from partymast Where partycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    '---------  Sales Details ------------------------------------

                    If IsDBNull(mySqlReader("stel1")) = False Then
                        txtSaleTelephone1.Value = mySqlReader("stel1")
                    Else
                        txtSaleTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("stel2")) = False Then
                        txtSaleTelephone2.Value = mySqlReader("stel2")
                    Else
                        txtSaleTelephone2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("smobileno")) = False Then
                        txtSaleMobile.Value = mySqlReader("smobileno")
                    Else
                        txtSaleMobile.Value = ""
                    End If

                    If IsDBNull(mySqlReader("sfax")) = False Then
                        txtSaleFax.Value = mySqlReader("sfax")
                    Else
                        txtSaleFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact1")) = False Then
                        txtSaleContact1.Value = mySqlReader("scontact1")
                    Else
                        txtSaleContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact2")) = False Then
                        txtSaleContact2.Value = mySqlReader("scontact2")
                    Else
                        txtSaleContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("semail")) = False Then
                        txtSaleEmail.Value = mySqlReader("semail")
                    Else
                        txtSaleEmail.Value = ""
                    End If
                    '------------------------END-----------------------------------

                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region
End Class
