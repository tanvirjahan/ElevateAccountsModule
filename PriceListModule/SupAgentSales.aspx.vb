'------------================--------------=======================------------------================
'   Module Name    :    SupplierAgents.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    9 July 2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class SupAgentSales
    Inherits System.Web.UI.Page
    Dim objuser As New clsUser

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
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplieragentsSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If


            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTName, "sptypename", "sptypecode", "select * from sptypemast where active=1 order by sptypename", True)

            telepphone(txtSaleTelephone1)
            telepphone(txtSaleTelephone2)
            Numbers(txtSaleFax)
            charcters(txtSuppCode)
            charcters(txtSuppName)
            If CType(Session("SupagentsState"), String) = "New" Then
                SetFocus(txtSuppCode)
                lblHeading.Text = "Add New Supplier Agents" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Agents Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
            ElseIf CType(Session("SupagentsState"), String) = "Edit" Then
                BtnSaleSave.Text = "Update"
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                ddlType.Disabled = True
                ddlTName.Disabled = True
                SetFocus(txtSaleTelephone1)
                lblHeading.Text = "Edit Supplier Agents" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Agents Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
            ElseIf CType(Session("SupagentsState"), String) = "View" Then
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                ddlType.Disabled = True
                ddlTName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier Agents" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Agents Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Visible = False
                BtnSaleCancel.Text = "Return to Search"
            ElseIf CType(Session("SupagentsState"), String) = "Delete" Then
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                ddlType.Disabled = True
                ddlTName.Disabled = True
                DisableControl()
                lblHeading.Text = "Delete Supplier Agents" + " - " + PanelSales.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Agents Master" + " - " + PanelSales.GroupingText
                BtnSaleSave.Text = "Delete"
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
            End If
            BtnSaleCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        End If

        Session.Add("submenuuser", "SupplierAgentsSearch.aspx")
    End Sub
#End Region


#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        Me.txtSuppCode.Disabled = True
        Me.txtSuppName.Disabled = True
        '------------------------------------------------------
        '---------  Sales Details ------------------------------------
        txtSaleTelephone1.Disabled = True
        txtSaleTelephone2.Disabled = True
        txtSaleMob.Disabled = True
        txtSaleFax.Disabled = True
        txtSaleContact1.Disabled = True
        txtSaleContact2.Disabled = True
        txtSaleEmail.Disabled = True
        '------------------------END-----------------------------------

    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("supagentcode")) = False Then
                        Me.txtSuppCode.Value = mySqlReader("supagentcode")
                    End If
                    If IsDBNull(mySqlReader("supagentname")) = False Then
                        Me.txtSuppName.Value = mySqlReader("supagentname")
                    End If
                    If IsDBNull(mySqlReader("sptypecode")) = False Then
                        ddlTName.Value = mySqlReader("sptypecode")
                        ddlType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))
                    End If
                    '------------------------END-----------------------------------
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
                        txtSaleMob.Value = mySqlReader("smobileno")
                    Else
                        txtSaleMob.Value = ""
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
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("SupagentsState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupagentsState") = "Edit" Then

                    If txtSaleEmail.Value.Trim <> "" Then
                        If EmailValidate(txtSaleEmail.Value.Trim, txtSaleEmail) = False Then
                            '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Fax field can not be blank.');", True)
                            Exit Sub
                        End If
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("SupagentsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("SupagentsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_supagents", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlType.Items(ddlType.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel1", SqlDbType.VarChar, 100)).Value = CType(txtSaleTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel2", SqlDbType.VarChar, 100)).Value = CType(txtSaleTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@smobileno", SqlDbType.VarChar, 100)).Value = CType(txtSaleMob.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sfax", SqlDbType.VarChar, 50)).Value = CType(txtSaleFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact1", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact2", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@semail", SqlDbType.VarChar, 100)).Value = CType(txtSaleEmail.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("SupagentsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from supagents_multiemail where supagentcode='" & txtSuppCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("SupagentsState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("SupagentsState") = "Edit" Then
                    Session("SupagentsState") = "Edit"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("SupagentsState") = "Delete" Then
                    'Response.Redirect("SupplierAgentsSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SupagentsWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

#Region "Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Session("State") = ""
        'Response.Redirect("SupplierAgentsSearch.aspx")
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SupagentsWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "blocksale_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a BlockFullOfSales, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cancel_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a CancellationPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "child_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a ChildPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compulsory_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a CompulsoryRemarks, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a PriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromotion_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a EarliBirdPromotion, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "minnights_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a MinimumNights, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costh", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a OtherServiceCostPriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a Promotions, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellsph", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a SellingFormulaForSupplier, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a GeneralPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a  SpecialEvents/Extras, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function




        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supagents_multiemail", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a Email Of SupplierAgent, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplisthnew", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a  TicketingpriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function


        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SuppAgentSales','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
