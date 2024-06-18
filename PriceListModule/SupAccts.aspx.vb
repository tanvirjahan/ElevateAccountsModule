#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupAccts
    Inherits System.Web.UI.Page
    Dim ctrycode As String
    Dim citycode As String
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
        PanelAccounts.Visible = True
        If IsPostBack = False Then

            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If



            If CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) <> "OTH" Then

                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)
                Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))


            ElseIf CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 39)
                Session("supmain_suptype") = "Other Serv"
            ElseIf CType(Request.QueryString("appid"), String) = "10" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
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
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)


            Session("partycode") = Nothing

            telepphone(txtAccTelephone1)
            telepphone(txtAccTelephone2)
            Numbers(txtAccFax)
            Numbers(TxtAccCreditDays)
            Numbers(txtAccCreditLimit)
            charcters(txtCode)
            charcters(txtName)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1  order by partycode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostName, "partyname", "partycode", "select  partyname,partycode from partymast where active=1  order by partyname", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", "select acctcode,acctname from acctmast where upper(controlyn)='Y' and cust_supp='S'order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", "select  acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='S'order by acctname", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccrualCode, "acctcode", "acctname", "select acctcode,acctname from acctmast where upper(controlyn)='Y' and cust_supp='S'order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccrualName, "acctname", "acctcode", "select  acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='S'order by acctname", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlInvPostCode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlInvPostName, "acctname", "acctcode", "select  acctname,acctcode from acctmast order by acctname", True)

            'Added ddlInvPostCode and ddlInvPostName by Archana on 25/06/2015





            Dim sptype As String
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

            Dim strsptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='458'")

            'If sptype = strsptype Then
            '    lblposttype.Visible = True
            '    ddlPosttype.Visible = True
            'Else
            '    lblposttype.Visible = False
            '    ddlPosttype.Visible = False
            'End If

            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                lblHeading.Text = "Add New Supplier" + " - " + PanelAccounts.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Master" + " - " + PanelAccounts.GroupingText
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
            ElseIf CType(Session("SupState"), String) = "Edit" Then
                BtnAccSave.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(txtCode)
                lblHeading.Text = "Edit Supplier" + " - " + PanelAccounts.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Master" + " - " + PanelAccounts.GroupingText
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")
                SetFocus(txtAccTelephone1)
            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier" + " - " + PanelAccounts.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Master" + " - " + PanelAccounts.GroupingText
                BtnAccSave.Visible = False
                BtnAccCancel.Text = "Return to Search"

            ElseIf CType(Session("SupState"), String) = "Delete" Then

                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                lblHeading.Text = "Delete Supplier" + " - " + PanelAccounts.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Master" + " - " + PanelAccounts.GroupingText
                BtnAccSave.Text = "Delete"
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
            End If
            BtnAccCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
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
        txtAccTelephone1.Disabled = True
        txtAccTelephone2.Disabled = True
        txtAccmobile.Disabled = True
        txtAccFax.Disabled = True
        txtAccContact1.Disabled = True
        txtAccContact2.Disabled = True
        txtAccEmail.Disabled = True
        TxtAccCreditDays.Disabled = True
        txtAccCreditLimit.Disabled = True
        ddlAccCode.Disabled = True
        ddlAccName.Disabled = True
        ddlAccrualCode.Disabled = True
        ddlAccrualName.Disabled = True
        ddlPostCode.Disabled = True
        ddlPostName.Disabled = True
        ChkCashSup.Disabled = True
        ddlSuppierCD.Disabled = True
        ddlSuppierNM.Disabled = True
        btnfilldetail.Enabled = False
        ddlInvPostCode.Disabled = True 'Added ddlInvPostCode by Archana on 25/06/2015
        ddlInvPostName.Disabled = True 'Added ddlInvPostName by Archana on 25/06/2015

        '------------------------END-----------------------------------

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

                    '---------  Account Details ------------------------------------

                    If IsDBNull(mySqlReader("atel1")) = False Then
                        txtAccTelephone1.Value = mySqlReader("atel1")
                    Else
                        txtAccTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("atel2")) = False Then
                        txtAccTelephone2.Value = mySqlReader("atel2")
                    Else
                        txtAccTelephone2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("amobileno")) = False Then
                        txtAccmobile.Value = mySqlReader("amobileno")
                    Else
                        txtAccmobile.Value = ""
                    End If

                    If IsDBNull(mySqlReader("afax")) = False Then
                        txtAccFax.Value = mySqlReader("afax")
                    Else
                        txtAccFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact1")) = False Then
                        txtAccContact1.Value = mySqlReader("acontact1")
                    Else
                        txtAccContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact2")) = False Then
                        txtAccContact2.Value = mySqlReader("acontact2")
                    Else
                        txtAccContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("aemail")) = False Then
                        txtAccEmail.Value = mySqlReader("aemail")
                    Else
                        txtAccEmail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crdays")) = False Then
                        TxtAccCreditDays.Value = mySqlReader("crdays")
                    Else
                        TxtAccCreditDays.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crlimit")) = False Then
                        txtAccCreditLimit.Value = mySqlReader("crlimit")
                    Else
                        txtAccCreditLimit.Value = ""
                    End If

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partycode<> '" & txtCode.Value & "' order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostName, "partyname", "partycode", "select  partyname,partycode from partymast where active=1 and partycode<> '" & txtCode.Value & "'   order by partyname", True)
                    If IsDBNull(mySqlReader("postaccount")) = False Then
                        ddlPostName.Value = mySqlReader("postaccount")
                        ddlPostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("postaccount"))
                    End If


                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        ddlAccName.Value = mySqlReader("controlacctcode")
                        ddlAccCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                    Else
                        ddlAccCode.Value = "[Select]"
                        ddlAccName.Value = "[Select]"
                    End If



                    If IsDBNull(mySqlReader("accrualacctcode")) = False Then
                        ddlAccrualName.Value = mySqlReader("accrualacctcode")
                        ddlAccrualCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("accrualacctcode"))
                    Else
                        ddlAccrualCode.Value = "[Select]"
                        ddlAccrualName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("cashsupp")) = False Then
                        If mySqlReader("cashsupp") = 1 Then
                            ChkCashSup.Checked = True
                        Else
                            ChkCashSup.Checked = False
                        End If
                    End If


                    'If IsDBNull(mySqlReader("postingtype")) = False Then
                    '    ddlPosttype.Value = mySqlReader("postingtype")
                    'Else
                    '    ddlPosttype.Value = "[Select]"
                    'End If

                    If IsDBNull(mySqlReader("invpost")) = False Then
                        If mySqlReader("invpost") = 1 Then
                            chkInvPost.Checked = True
                        Else
                            chkInvPost.Checked = False
                        End If
                    End If
                    'Added chkInvPost If Condition by Archana on 25/06/2015

                    If IsDBNull(mySqlReader("invaccount")) = False Then
                        ddlInvPostName.Value = mySqlReader("invaccount")
                        ddlInvPostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("invaccount"))
                    Else
                        ddlInvPostCode.Value = "[Select]"
                        ddlInvPostName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("TRNNo")) = False Then
                        TxtTRN.Value = mySqlReader("TRNNo")
                    Else
                        TxtTRN.Value = ""
                    End If

                    'Added ddlInvPostName and ddlInvPostCode by Archana on 26/05/2015

                    '------------------------END-----------------------------------

                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

#Region " Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAccSave.Click
        Try
            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then
                    '-----------    Validate Page   ---------------
                    If ddlAccCode.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Control A/C Code field can not be blank.');", True)
                        Exit Sub
                    End If

                    'If ddlAccrualCode.Value = "[Select]" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accrual A/C Code field can not be blank.');", True)
                    '    Exit Sub
                    'End If
                    If txtAccEmail.Value.Trim <> "" Then
                        If EmailValidate(txtAccEmail.Value.Trim, txtAccEmail) = False Then
                            SetFocus(txtAccEmail)
                            Exit Sub
                        End If
                    End If
                    '---------------------------------------------------

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("SupState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_partymast", mySqlConn, sqlTrans)
                    ElseIf Session("SupState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_partymast", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel1", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel2", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@amobile", SqlDbType.VarChar, 50)).Value = CType(txtAccmobile.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@afax", SqlDbType.VarChar, 50)).Value = CType(txtAccFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact1", SqlDbType.VarChar, 100)).Value = CType(txtAccContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact2", SqlDbType.VarChar, 100)).Value = CType(txtAccContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@aemail", SqlDbType.VarChar, 100)).Value = CType(txtAccEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crdays", SqlDbType.Int, 4)).Value = CType(Val(TxtAccCreditDays.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crlimit", SqlDbType.Int, 4)).Value = CType(Val(txtAccCreditLimit.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlAccrualCode.Items(ddlAccrualCode.SelectedIndex).Text.Trim, String)


                    If ChkCashSup.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashsupp", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkCashSup.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashsupp", SqlDbType.Int, 4)).Value = 1
                    End If



                    If CType(ddlPostCode.Items(ddlPostCode.SelectedIndex).Text, String) <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = CType(ddlPostCode.Items(ddlPostCode.SelectedIndex).Text, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    'If CType(ddlPosttype.Value, String) <> "[Select]" Then
                    'mySqlCmd.Parameters.Add(New SqlParameter("@postingtype", SqlDbType.Int, 4)).Value = CType(ddlPosttype.Value, Integer)
                    'Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@postingtype", SqlDbType.Int, 4)).Value = System.DBNull.Value
                    'End If
                    If chkInvPost.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@invpost", SqlDbType.Int, 4)).Value = 0
                    ElseIf chkInvPost.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@invpost", SqlDbType.Int, 4)).Value = 1
                    End If
                    'Added chkInvPost by Archana on 25/06/2015
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoiceacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlInvPostCode.Items(ddlInvPostCode.SelectedIndex).Text.Trim, String)
                    'Added invoiceacctcode by Archana on 25/06/2015
                    mySqlCmd.Parameters.Add(New SqlParameter("@TRN", SqlDbType.VarChar, 20)).Value = CType(Val(TxtTRN.Value.Trim), Long)
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
                    'mySqlCmd = New SqlCommand("sp_del_partyallot", mySqlConn, sqlTrans)

                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
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
            objUtils.WritErrorLog("SupAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email length is too large..please enter valid email exampele(abc@abc.com).');", True)
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupAccts','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
                    '---------  Account Details ------------------------------------

                    If IsDBNull(mySqlReader("atel1")) = False Then
                        txtAccTelephone1.Value = mySqlReader("atel1")
                    Else
                        txtAccTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("atel2")) = False Then
                        txtAccTelephone2.Value = mySqlReader("atel2")
                    Else
                        txtAccTelephone2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("amobileno")) = False Then
                        txtAccmobile.Value = mySqlReader("amobileno")
                    Else
                        txtAccmobile.Value = ""
                    End If

                    If IsDBNull(mySqlReader("afax")) = False Then
                        txtAccFax.Value = mySqlReader("afax")
                    Else
                        txtAccFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact1")) = False Then
                        txtAccContact1.Value = mySqlReader("acontact1")
                    Else
                        txtAccContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact2")) = False Then
                        txtAccContact2.Value = mySqlReader("acontact2")
                    Else
                        txtAccContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("aemail")) = False Then
                        txtAccEmail.Value = mySqlReader("aemail")
                    Else
                        txtAccEmail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crdays")) = False Then
                        TxtAccCreditDays.Value = mySqlReader("crdays")
                    Else
                        TxtAccCreditDays.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crlimit")) = False Then
                        txtAccCreditLimit.Value = mySqlReader("crlimit")
                    Else
                        txtAccCreditLimit.Value = ""
                    End If

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostCode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 and partycode<> '" & txtCode.Value & "' order by partycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostName, "partyname", "partycode", "select  partyname,partycode from partymast where active=1 and partycode<> '" & txtCode.Value & "'   order by partyname", True)
                    If IsDBNull(mySqlReader("postaccount")) = False Then
                        ddlPostName.Value = mySqlReader("postaccount")
                        ddlPostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("postaccount"))
                    End If


                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        ddlAccName.Value = mySqlReader("controlacctcode")
                        ddlAccCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                    Else
                        ddlAccCode.Value = "[Select]"
                        ddlAccName.Value = "[Select]"
                    End If



                    If IsDBNull(mySqlReader("accrualacctcode")) = False Then
                        ddlAccrualName.Value = mySqlReader("accrualacctcode")
                        ddlAccrualCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("accrualacctcode"))
                    Else
                        ddlAccrualCode.Value = "[Select]"
                        ddlAccrualName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("cashsupp")) = False Then
                        If mySqlReader("cashsupp") = 1 Then
                            ChkCashSup.Checked = True
                        Else
                            ChkCashSup.Checked = False
                        End If
                    End If

                    'If IsDBNull(mySqlReader("postingtype")) = False Then
                    '    ddlPosttype.Value = mySqlReader("postingtype")
                    'Else
                    '    ddlPosttype.Value = "[Select]"
                    'End If

                    '------------------------END-----------------------------------

                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupAccts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region
End Class

