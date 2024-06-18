Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class CustUsermail
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
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If


            PanelUser.Visible = True
            fillgrd(gv_Email, True)
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            GetValuesForEmailDetails()
            If CType(Session("State"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add New Customer - User Email"
                BtnUserSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Customer?')==false)return false;")
            ElseIf CType(Session("State"), String) = "Edit" Then

                BtnUserSave.Text = "Update"

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Edit Customer - User Email"
                BtnUserSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Customer?')==false)return false;")
            ElseIf CType(Session("State"), String) = "View" Then

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Customer - User Email"
                BtnUserSave.Visible = False
                BtnUserCancel.Text = "Return to Search"
                BtnUserCancel.Focus()

            ElseIf CType(Session("State"), String) = "Delete" Then

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Delete Customer - User Email"
                BtnUserSave.Text = "Delete"
                BtnUserSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Customer?')==false)return false;")
            End If
            BtnUserCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If
        End If
        Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnUserCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnUserCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("CustomersSearch.aspx")
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("State") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentcode", CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentname", txtCustomerName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("State") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", txtCustomerName.Value.Trim, CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

#Region "Protected Sub BtnUserSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnUserSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    If ValidateEmail() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In gv_Email.Rows
                        txtName = GvRow.FindControl("txtPerson")
                        txtEmail = GvRow.FindControl("txtEmail")
                        txtContact = GvRow.FindControl("txtContactNo")
                        If CType(txtName.Value, String) <> "" And CType(txtEmail.Value, String) <> "" And CType(txtContact.Value, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_agentmast_mulltiemail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactperson", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtEmail.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactno", SqlDbType.VarChar, 50)).Value = CType(txtContact.Value.Trim, String)
                            '  mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("State") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustUsermail.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnUserDetailAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnUserDetailAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLines()
    End Sub
#End Region

#Region "Private Sub AddLines()"
    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Email.Rows.Count + 1
        Dim txt As HtmlInputText
        Dim name(count) As String
        Dim email(count) As String
        Dim contact(count) As String
        Dim n As Integer = 0
        Try
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                name(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtEmail")
                email(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtContactNo")
                contact(n) = CType(Trim(txt.Value), String)
                n = n + 1
            Next
            fillgrd(gv_Email, False, gv_Email.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_Email.Rows
                If n = i Then
                    Exit For
                End If

                txt = GVRow.FindControl("txtPerson")
                txt.Value = name(n)
                txt = GVRow.FindControl("txtEmail")
                txt.Value = email(n)
                txt = GVRow.FindControl("txtContactNo")
                txt.Value = contact(n)
                n = n + 1
            Next
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                ' txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtContactNo")
                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentcode")) = False Then
                        Me.txtCustomerCode.Value = mySqlReader("agentcode")
                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtCustomerName.Value = mySqlReader("agentname")
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustUsermail.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        BtnUserDetailAdd.Enabled = False
        gv_Email.Enabled = False

    End Sub
#End Region


#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromagent_detail", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a DetailsOfEarlyBirdPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_agent", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a CustomerPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If

        checkForDeletion = True
    End Function
#End Region

#Region " Private Sub GetValuesForEmailDetails()"
    Private Sub GetValuesForEmailDetails()
        Try
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txt As HtmlInputText
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from agentmast_mulltiemail Where agentcode='" & Session("RefCode") & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast_mulltiemail Where agentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In gv_Email.Rows

                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("contactperson")) = False Then
                            txt = GVRow.FindControl("txtPerson")
                            txt.Value = mySqlReader("contactperson")
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txt = GVRow.FindControl("txtEmail")
                            txt.Value = mySqlReader("email")
                        End If
                        If IsDBNull(mySqlReader("contactno")) = False Then
                            txt = GVRow.FindControl("txtContactNo")
                            txt.Value = mySqlReader("contactno")
                        End If
                    End If
                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("no", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region


#Region " Private Function ValidateEmail() As Boolean"
    Private Function ValidateEmail() As Boolean
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GVRow As GridViewRow
        Dim FLAG As Boolean = False
        Try
            For Each GVRow In gv_Email.Rows
                txtName = GVRow.FindControl("txtPerson")
                txtEmail = GVRow.FindControl("txtEmail")
                txtContact = GVRow.FindControl("txtContactNo")
                If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                    FLAG = True
                End If
            Next

            If FLAG = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter at least one email details.');", True)
                ValidateEmail = False
                Exit Function
            Else

                For Each GVRow In gv_Email.Rows
                    txtName = GVRow.FindControl("txtPerson")
                    txtEmail = GVRow.FindControl("txtEmail")
                    txtContact = GVRow.FindControl("txtContactNo")
                    If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                        If txtName.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact Person field can not be blank.');", True)
                            SetFocus(txtName)
                            ValidateEmail = False
                            Exit Function
                        End If
                        If txtEmail.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email field can not be blank.');", True)
                            SetFocus(txtEmail)
                            ValidateEmail = False
                            Exit Function
                        Else
                            If EmailValidate(txtEmail.Value.Trim, txtEmail) = False Then
                                SetFocus(txtEmail)
                                ValidateEmail = False
                                Exit Function
                            End If
                        End If
                        If txtContact.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact no field can not be blank.');", True)
                            SetFocus(txtContact)
                            ValidateEmail = False
                            Exit Function
                        End If

                    End If
                Next
            End If
            ValidateEmail = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
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

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustUsermail','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
