'------------================--------------=======================------------------================
'   Module Name    :    SupplierAgents.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    8 July 2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupAgentResv
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
        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplieragentsSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            PanelReservation.Visible = True
            telepphone(txtResPhone1)
            telepphone(txtResPhone2)
            Numbers(txtResFax)
            charcters(txtSuppCode)
            charcters(txtSuppName)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTName, "sptypename", "sptypecode", "select * from sptypemast where active=1 order by sptypename", True)

            If CType(Session("SupagentsState"), String) = "New" Then
                SetFocus(txtSuppCode)
                lblHeading.Text = "Add New Supplier Agents" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Agents Master" + " - " + PanelReservation.GroupingText
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
            ElseIf CType(Session("SupagentsState"), String) = "Edit" Then
                BtnResSave.Text = "Update"
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                ddlType.Disabled = True
                ddlTName.Disabled = True
                SetFocus(txtResAddress1)
                lblHeading.Text = "Edit Supplier Agents" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Agents Master" + " - " + PanelReservation.GroupingText
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")

            ElseIf CType(Session("SupagentsState"), String) = "View" Then
                ddlType.Disabled = True
                ddlTName.Disabled = True
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier Agents" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Agents Master" + " - " + PanelReservation.GroupingText
                BtnResSave.Visible = False
                BtnResCancel.Text = "Return to Search"

            ElseIf CType(Session("SupagentsState"), String) = "Delete" Then
                ddlType.Disabled = True
                ddlTName.Disabled = True
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                DisableControl()
                lblHeading.Text = "Delete Supplier Agents" + " - " + PanelReservation.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Agents Master" + " - " + PanelReservation.GroupingText
                BtnResSave.Text = "Delete"
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
            End If
            BtnResCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

        End If

        Session.Add("submenuuser", "SupplierAgentsSearch.aspx")

    End Sub
#End Region


#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        Me.txtSuppCode.Disabled = True
        Me.txtSuppName.Disabled = True
        ddlType.Disabled = True
        ddlTName.Disabled = True
        '-------------- Reservation Details --------------------------
        txtResAddress1.Disabled = True
        txtResAddress2.Disabled = True
        txtResAddress3.Disabled = True
        txtResPhone1.Disabled = True
        txtResPhone2.Disabled = True
        txtResFax.Disabled = True
        txtResContact1.Disabled = True
        txtResContact2.Disabled = True
        txtResEmail.Disabled = True
        ddlComunicate.Enabled = False
        ddlSell.Enabled = False
        txtResWebSite.Disabled = True

        chkprintpricesinrequest.Disabled = True
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
                    If IsDBNull(mySqlReader("commmode")) = False Then
                        If mySqlReader("commmode") = "1" Then
                            ddlComunicate.SelectedValue = "Email"
                        ElseIf mySqlReader("commmode") = "0" Then
                            ddlComunicate.SelectedValue = "Fax"
                        ElseIf mySqlReader("commmode") = "2" Then
                            ddlComunicate.SelectedValue = "Both"
                        End If
                    End If
                    If IsDBNull(mySqlReader("selltype")) = False Then
                        If mySqlReader("selltype") = "1" Then
                            ddlSell.SelectedValue = "Beach"
                        ElseIf mySqlReader("selltype") = "0" Then
                            ddlSell.SelectedValue = "City"
                        ElseIf mySqlReader("selltype") = "2" Then
                            ddlSell.SelectedValue = "Desert"
                        End If
                    End If
                    If IsDBNull(mySqlReader("website")) = False Then
                        txtResWebSite.Value = mySqlReader("website")
                    Else
                        txtResWebSite.Value = ""
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
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        ' txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region

#Region "Private Function ValidateResrvation() As Boolean"
    Private Function ValidateResrvation() As Boolean
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & CType(Session("SupagentsRefCode"), String) & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                mySqlCmd.Dispose()
                mySqlConn.Close()
            Else
                mySqlCmd.Dispose()
                mySqlConn.Close()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please save first main details.');", True)
                ValidateResrvation = False
                Exit Function
            End If

            If txtResAddress1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Address1 field can not be blank.');", True)
                'SetFocus(txtResAddress1)

                ValidateResrvation = False
                Exit Function
            End If
            If txtResPhone1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Telephone1 field can not be blank.');", True)
                SetFocus(txtResPhone1)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResFax.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Fax field can not be blank.');", True)
                SetFocus(txtResFax)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResEmail.Value.Trim <> "" Then
                If EmailValidate(txtResEmail.Value.Trim, txtResEmail) = False Then
                    ValidateResrvation = False
                    SetFocus(txtResEmail)
                    Exit Function
                End If
            End If
            If Trim(txtResWebSite.Value) <> "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                'SetFocus(txtResWebSite)
                'ValidateResrvation = False
                'Exit Function
                ' Else
                Dim getstr As String
                Dim dotpos1 As String
                Dim dotpos2 As String
                getstr = Trim(txtResWebSite.Value)
                dotpos1 = getstr.LastIndexOf(".")
                dotpos2 = getstr.LastIndexOf(".")
                If dotpos1 < 1 Or dotpos2 < 2 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                    SetFocus(txtResWebSite)
                    ValidateResrvation = False
                    Exit Function
                Else
                    Dim laststr As String
                    Dim atposstr As String()
                    Dim getvaluestr As String
                    Dim tempstr As String
                    atposstr = getstr.Split(".")
                    If atposstr.Length < 3 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                        SetFocus(txtResWebSite)
                        ValidateResrvation = False
                        Exit Function
                    ElseIf atposstr.Length = 3 Then
                        getvaluestr = atposstr.GetValue(atposstr.Length() - 3)
                        tempstr = atposstr.GetValue(atposstr.Length() - 1)
                        If getvaluestr <> "www" Or IsNumeric(tempstr) = True Or tempstr.Length() < 3 Then 'Or tempstr.Length() > 2
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            SetFocus(txtResWebSite)
                            ValidateResrvation = False
                            Exit Function
                        End If
                    ElseIf atposstr.Length > 3 Then
                        getvaluestr = atposstr.GetValue(atposstr.Length() - 4)
                        tempstr = atposstr.GetValue(atposstr.Length() - 2)
                        laststr = atposstr.GetValue(atposstr.Length() - 1)
                        If laststr = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            SetFocus(txtResWebSite)
                            ValidateResrvation = False
                            Exit Function
                        ElseIf getvaluestr <> "www" Or IsNumeric(tempstr) = True Or IsNumeric(laststr) = True Or tempstr.Length > 2 Or laststr.Length < 2 Or laststr.Length > 2 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            SetFocus(txtResWebSite)
                            ValidateResrvation = False
                            Exit Function
                        End If
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                        ValidateResrvation = False
                        Exit Function
                    End If
                End If
            End If
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
                If Session("SupagentsState") = "New" Then
                    'objUtils.MessageBox("Please Save First Main Details.", Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupagentsState") = "Edit" Then

                    If ValidateResrvation() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("SupagentsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_update_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("SupagentsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_update_supagents", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlType.Items(ddlType.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 500)).Value = CType(txtResAddress1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = CType(txtResAddress2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = CType(txtResAddress3.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(txtResPhone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = CType(txtResPhone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@mobileno", SqlDbType.VarChar, 50)).Value = CType(txtresmob.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(txtResFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = CType(txtResContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = CType(txtResContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtResEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@website", SqlDbType.VarChar, 200)).Value = CType(txtResWebSite.Value.Trim, String)


                    If ddlComunicate.SelectedValue = "Email" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlComunicate.SelectedValue = "Fax" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int, 4)).Value = 0
                    ElseIf ddlComunicate.SelectedValue = "Both" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int, 4)).Value = 2
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    If ddlSell.SelectedValue = "Beach" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlSell.SelectedValue = "City" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = 0
                    ElseIf ddlSell.SelectedValue = "Desert" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = 2
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    If chkprintpricesinrequest.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkprint", SqlDbType.Char, 1)).Value = "1"
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkprint", SqlDbType.Char, 1)).Value = "0"
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
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

#Region "Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Session("State") = ""
        'Response.Redirect("SupplierAgentsSearch.aspx")
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SupagentsWindowPostBack', '');window.opener.focus();window.close();"
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



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplisthnew", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a  TicketingpriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function


        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SuppAgentResv','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
