

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports UploadService.UploadService
Imports System.IO
Imports System.Net
#End Region

Partial Class SupResv
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
        If IsPostBack = False Then
            PanelReservation.Visible = True
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            If CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) <> "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)

                Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))



            ElseIf CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH" Then

                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 39)


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
         
         
            ' Session("partycode") = Nothing

            Dim sptype As String

            '    whotelatbcontrol.menuitemcolor()

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                Exit Sub
            End If
            telepphone(txtResPhone1)
            telepphone(txtResPhone2)
            Numbers(txtResFax)
            charcters(txtCode)
            charcters(txtName)




            If CType(Session("SupState"), String) = "New" Or CType(Session("SupState"), String) = "Edit" Then
                If Not Session("SupRefCode") = Nothing Then
                    sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" + CType(Session("SupRefCode"), String) + "'")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode='" + sptype + "' order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" + sptype + "' order by partyname", True)

                    'If sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=458") And CType(Request.QueryString("appid"), String) = "1" Then
                    '    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

                    '    Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                    '    Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

                    'ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=458") And CType(Request.QueryString("appid"), String) <> "1" Then
                    '    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))

                    '    Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                    '    Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

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
                    If CType(Request.QueryString("appid"), String) = "1" Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode in (select  option_selected from reservation_parameters  where param_id in(458,460)) order by partycode", True)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode in (select  option_selected from reservation_parameters  where param_id in(458,460)) order by partyname", True)
                    End If
                    If CType(Request.QueryString("appid"), String) = "11" Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierCD, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and sptypecode in (select  option_selected from reservation_parameters  where param_id =1033) order by partycode", True)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppierNM, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode in (select  option_selected from reservation_parameters  where param_id =1033) order by partyname", True)
                    End If
                End If
            End If
            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                lblHeading.Text = "Add New Supplier" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + PanelReservation.GroupingText
                'GetWeekEndValues()
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier ?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnResSave.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtResAddress1)
                lblHeading.Text = "Edit Supplier" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + PanelReservation.GroupingText
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier ?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + PanelReservation.GroupingText
                BtnResSave.Visible = False
                BtnResCancel.Text = "Return to Search"
            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                lblHeading.Text = "Delete Supplier" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + PanelReservation.GroupingText
                BtnResSave.Text = "Delete"
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier ?')==false)return false;")
            End If
            BtnResCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            'Else
            '    whotelatbcontrol.SelectMenuByValue()

            'For Each item As MenuItem In Menu1.Items
            '    If Path.GetFileName(item.NavigateUrl).Equals(Path.GetFileName(Request.PhysicalPath), StringComparison.InvariantCultureIgnoreCase) Then
            '        item.Selected = True
            '    End If
            'Next
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
        '-------------- Reservation Details --------------------------
        txtResAddress1.Disabled = True
        txtResAddress2.Disabled = True
        txtResAddress3.Disabled = True
        txtResPhone1.Disabled = True
        txtResPhone2.Disabled = True
        txtResMob.Disabled = True
        txtResFax.Disabled = True
        txtResContact1.Disabled = True
        txtResContact2.Disabled = True
        txtResWebSite.Disabled = True
        ddlWO2from.Enabled = False


        txtResEmail.Disabled = True


        ddlAutoEmail.Enabled = False
        chkprintpricesinrequest.Disabled = True

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

                    '-------------- Reservation Details --------------------------
                    If IsDBNull(mySqlReader("add1")) = False Then
                        txtResAddress1.Value = mySqlReader("add1")
                    Else
                        txtResAddress1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add2")) = False Then
                        txtResAddress2.Value = mySqlReader("add2")
                    Else
                        txtResAddress2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add3")) = False Then
                        txtResAddress3.Value = mySqlReader("add3")
                    Else
                        txtResAddress3.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel1")) = False Then
                        txtResPhone1.Value = mySqlReader("tel1")
                    Else
                        txtResPhone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel2")) = False Then
                        txtResPhone2.Value = mySqlReader("tel2")
                    Else
                        txtResPhone2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("mobileno")) = False Then
                        txtResMob.Value = mySqlReader("mobileno")
                    Else
                        txtResMob.Value = ""
                    End If

                    If IsDBNull(mySqlReader("fax")) = False Then
                        txtResFax.Value = mySqlReader("fax")
                    Else
                        txtResFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact1")) = False Then
                        txtResContact1.Value = mySqlReader("contact1")
                    Else
                        txtResContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact2")) = False Then
                        txtResContact2.Value = mySqlReader("contact2")
                    Else
                        txtResContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("email")) = False Then
                        txtResEmail.Value = mySqlReader("email")
                    Else
                        txtResEmail.Value = ""
                    End If



                    If IsDBNull(mySqlReader("website")) = False Then
                        txtResWebSite.Value = mySqlReader("website")
                    Else
                        txtResWebSite.Value = ""
                    End If




                    If IsDBNull(mySqlReader("cencelpolicy")) = False Then
                        ddlWO2from.SelectedValue = CType(mySqlReader("cencelpolicy"), String)
                    Else
                        ddlWO2from.SelectedValue = "[Select]"
                    End If


                    If IsDBNull(mySqlReader("automail")) = False Then
                        If CType(mySqlReader("automail"), String) = "1" Then
                            ddlAutoEmail.SelectedValue = "Yes"
                        ElseIf CType(mySqlReader("automail"), String) = "0" Then
                            ddlAutoEmail.SelectedValue = "No"
                        End If
                    End If

                    If IsDBNull(mySqlReader("checkprint")) = False Then
                        If CType(mySqlReader("checkprint"), String) = "1" Then
                            chkprintpricesinrequest.Checked = True
                        ElseIf CType(mySqlReader("checkprint"), String) = "0" Then
                            chkprintpricesinrequest.Checked = False
                        End If
                    End If


                    '------------------------END-----------------------------------

                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupResv.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        'txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        'txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region

#Region "Private Function ValidateResrvation() As Boolean"
    Private Function ValidateResrvation() As Boolean
        Try
            If txtResAddress1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Address1 field can not be blank.');", True)
                'SetFocus(txtResAddress1)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResAddress1.ClientID + "');", True)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResPhone1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Telephone1 field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResPhone1.ClientID + "');", True)
                'SetFocus(txtResPhone1)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResEmail.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email field can not be blank.');", True)
                'SetFocus(txtResFax)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResEmail.ClientID + "');", True)
                ValidateResrvation = False
                Exit Function
            End If

            'If txtResEmail.Value.Trim <> "" Then
            '    If EmailValidate(txtResEmail.Value.Trim, txtResEmail) = False Then
            '        ValidateResrvation = False
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResEmail.ClientID + "');", True)
            '        'SetFocus(txtResEmail)
            '        Exit Function
            '    End If
            'End If



            'If Trim(txtResWebSite.Value) <> "" Then
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
            '    'SetFocus(txtResWebSite)
            '    'ValidateResrvation = False
            '    'Exit Function
            '    ' Else
            '    Dim getstr As String
            '    Dim dotpos1 As String
            '    Dim dotpos2 As String
            '    getstr = Trim(txtResWebSite.Value)
            '    dotpos1 = getstr.LastIndexOf(".")
            '    dotpos2 = getstr.LastIndexOf(".")
            '    If dotpos1 < 1 Or dotpos2 < 2 Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
            '        'SetFocus(txtResWebSite)
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
            '        ValidateResrvation = False
            '        Exit Function
            '    Else
            '        Dim laststr As String
            '        Dim atposstr As String()
            '        Dim getvaluestr As String
            '        Dim tempstr As String
            '        atposstr = getstr.Split(".")
            '        If atposstr.Length < 3 Then
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
            '            'SetFocus(txtResWebSite)
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
            '            ValidateResrvation = False
            '            Exit Function
            '        ElseIf atposstr.Length = 3 Then
            '            getvaluestr = atposstr.GetValue(atposstr.Length() - 3)
            '            tempstr = atposstr.GetValue(atposstr.Length() - 1)
            '            'Or IsNumeric(tempstr) = True Or tempstr.Length() < 3 Or tempstr.Length() > 3
            '            If getvaluestr <> "www" Then
            '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
            '                'SetFocus(txtResWebSite)
            '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
            '                ValidateResrvation = False
            '                Exit Function
            '            End If
            '        ElseIf atposstr.Length > 3 Then
            '            getvaluestr = atposstr.GetValue(atposstr.Length() - 4)
            '            tempstr = atposstr.GetValue(atposstr.Length() - 2)
            '            laststr = atposstr.GetValue(atposstr.Length() - 1)
            '            If laststr = "" Then
            '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
            '                ' SetFocus(txtResWebSite)
            '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
            '                ValidateResrvation = False
            '                Exit Function
            '            ElseIf getvaluestr <> "www" Or IsNumeric(tempstr) = True Or IsNumeric(laststr) = True Or tempstr.Length > 2 Or laststr.Length < 2 Or laststr.Length > 2 Then
            '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
            '                'SetFocus(txtResWebSite)
            '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
            '                ValidateResrvation = False
            '                Exit Function
            '            End If
            '        Else
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResWebSite.ClientID + "');", True)
            '            ValidateResrvation = False
            '            Exit Function
            '        End If
            '    End If
            'End If
            ValidateResrvation = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region

#Region "Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                Dim strpath_logo1 As String = ""
                Dim strpath1 As String = ""

                If Session("SupState") = "New" Then
                    'objUtils.MessageBox("Please Save First Main Details.", Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then
                    If ValidateResrvation() = False Then
                        Exit Sub
                    End If






                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("SupState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateres_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("SupState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updateres_partymast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 500)).Value = CType(txtResAddress1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = CType(txtResAddress2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = CType(txtResAddress3.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(txtResPhone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = CType(txtResPhone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@mobileno", SqlDbType.VarChar, 50)).Value = CType(txtResMob.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(txtResFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = CType(txtResContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = CType(txtResContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtResEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@website", SqlDbType.VarChar, 200)).Value = CType(txtResWebSite.Value.Trim, String)
                    If ddlWO2from.SelectedValue <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cencelpolicy", SqlDbType.VarChar, 100)).Value = CType(ddlWO2from.SelectedValue, String)

                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@cencelpolicy", SqlDbType.VarChar, 100)).Value = DBNull.Value

                    End If



                    If ddlAutoEmail.SelectedValue = "Yes" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@automail", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlAutoEmail.SelectedValue = "No" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@automail", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@automail", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    If chkprintpricesinrequest.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkprint", SqlDbType.Char, 1)).Value = "1"
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkprint", SqlDbType.Char, 1)).Value = "0"
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

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
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.VarChar, 10)).Value = ""
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partymeal", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)

                    'mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
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
            objUtils.WritErrorLog("SupResv.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Session("State") = ""
        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
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
                    If dot.Length() > 2 Then
                        t = dot.GetValue(dot.Length() - 3)
                        test = sp.GetValue(sp.Length() - 2)
                        'If test <> "co" Or chkcom.Length() > 2 Or IsNumeric(t) = True Then
                        '    'objutil.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                        '    SetFocus(txt)
                        '    EmailValidate = False
                        '    Exit Function
                        'End If
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupResv','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub




    Public Sub SendImageToWebService(ByVal Path, ByVal filename)
        Dim SrvUserName As String = CType(ConfigurationManager.AppSettings.Get("SrvUserName"), String)
        Dim SrvPassword As String = CType(ConfigurationManager.AppSettings.Get("SrvPassword"), String)
        Dim SrvDomain As String = CType(ConfigurationManager.AppSettings.Get("SrvDomain"), String)
        Dim SrvUri As String = CType(ConfigurationManager.AppSettings.Get("SrvUri"), String)
        Dim SrvFilePath As String = CType(ConfigurationManager.AppSettings.Get("SrvFilePath"), String)

        Dim FS As New FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read)
        Dim Img(CInt(FS.Length)) As Byte
        FS.Read(Img, 0, CInt(FS.Length))

        Dim nwkCred As NetworkCredential = New NetworkCredential(SrvUserName, SrvPassword, SrvDomain)
        Dim proxy As WebProxy = New WebProxy(SrvUri)
        proxy.Credentials = nwkCred
        Dim Service1 As New UploadService.UploadService
        Service1.Proxy = proxy

        Service1.UploadImage(Img, SrvFilePath, filename)
        Service1.Dispose()
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
                    '-------------- Reservation Details --------------------------
                    If IsDBNull(mySqlReader("add1")) = False Then
                        txtResAddress1.Value = mySqlReader("add1")
                    Else
                        txtResAddress1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add2")) = False Then
                        txtResAddress2.Value = mySqlReader("add2")
                    Else
                        txtResAddress2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add3")) = False Then
                        txtResAddress3.Value = mySqlReader("add3")
                    Else
                        txtResAddress3.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel1")) = False Then
                        txtResPhone1.Value = mySqlReader("tel1")
                    Else
                        txtResPhone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel2")) = False Then
                        txtResPhone2.Value = mySqlReader("tel2")
                    Else
                        txtResPhone2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("mobileno")) = False Then
                        txtResMob.Value = mySqlReader("mobileno")
                    Else
                        txtResMob.Value = ""
                    End If

                    If IsDBNull(mySqlReader("fax")) = False Then
                        txtResFax.Value = mySqlReader("fax")
                    Else
                        txtResFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact1")) = False Then
                        txtResContact1.Value = mySqlReader("contact1")
                    Else
                        txtResContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact2")) = False Then
                        txtResContact2.Value = mySqlReader("contact2")
                    Else
                        txtResContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("email")) = False Then
                        txtResEmail.Value = mySqlReader("email")
                    Else
                        txtResEmail.Value = ""
                    End If

                    If IsDBNull(mySqlReader("cencelpolicy")) = False Then
                        ddlWO2from.SelectedValue = CType(mySqlReader("cencelpolicy"), String)
                    Else
                        ddlWO2from.SelectedValue = "[Select]"
                    End If

                End If
            End If

            If IsDBNull(mySqlReader("website")) = False Then
                txtResWebSite.Value = mySqlReader("website")
            Else
                txtResWebSite.Value = ""
            End If

            'weekend1






            If IsDBNull(mySqlReader("automail")) = False Then
                If CType(mySqlReader("automail"), String) = "1" Then
                    ddlAutoEmail.SelectedValue = "Yes"
                ElseIf CType(mySqlReader("automail"), String) = "0" Then
                    ddlAutoEmail.SelectedValue = "No"
                End If
            End If

            If IsDBNull(mySqlReader("checkprint")) = False Then
                If CType(mySqlReader("checkprint"), String) = "1" Then
                    chkprintpricesinrequest.Checked = True
                ElseIf CType(mySqlReader("checkprint"), String) = "0" Then
                    chkprintpricesinrequest.Checked = False
                End If
            End If

            '------------------------END-----------------------------------



            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupResv.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region
End Class
