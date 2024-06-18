
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupMultiMail
    Inherits System.Web.UI.Page
    Dim Objuser As New clsUser

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region
    Private Sub disablegrid()
        Dim txtperson As HtmlInputText
        Dim txtemail As HtmlInputText
        Dim txtcontactno As HtmlInputText
        Dim txtdesignation As HtmlInputText
        For Each gvrow In gv_Email.Rows
            txtperson = gvrow.FindControl("txtPerson")
            txtperson.Disabled = True
            txtemail = gvrow.FindControl("txtEmail")
            txtemail.Disabled = True
            txtcontactno = gvrow.FindControl("txtContactNo")
            txtcontactno.Disabled = True
            txtdesignation = gvrow.FindControl("txtdesignation")
            txtdesignation.Disabled = True
        Next

    End Sub
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim GVRow As GridViewRow
        Dim txt As HtmlInputText
        Dim RefCode As String
        PanelEmail.Visible = True
        If IsPostBack = False Then

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)


            If CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) <> "OTH" Then

                Me.SubMenuUserControl1.menuidval = Objuser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)

                Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                Me.whotelatbcontrol.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))


            ElseIf CType(Request.QueryString("appid"), String) = "1" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 39)

                Session("supmain_suptype") = "Other Serv"
            ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "10" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "13" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "HOT" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=HOT", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "HOT" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=HOT", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "OTH" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "VISA" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "VISA" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=VISA", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "TRFS" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "TRFS" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=TRFS", CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" And CType(Request.QueryString("type"), String) = "EXC" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "14" And CType(Request.QueryString("type"), String) = "EXC" Then
                Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=EXC", CType(Request.QueryString("appid"), Integer))


            End If


          
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            Session("partycode") = Nothing

            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If
            Dim sptype As String
            If CType(Session("SupState"), String) = "New" Or CType(Session("SupState"), String) = "Edit" Then
                sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" + CType(Session("SupRefCode"), String) + "'")

                If sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=458") Then

                    If CType(Request.QueryString("appid"), String) = 1 Then
                        Me.SubMenuUserControl1.menuidval = Objuser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 25)

                        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                        Me.whotelatbcontrol.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))
                    Else
                        Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                        Me.whotelatbcontrol.menuidval = ""
                    End If


                ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1032") Then
                    Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                    Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                    Me.whotelatbcontrol.menuidval = "" 'objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))
                ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=564") Then
                    Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                    Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                    Me.whotelatbcontrol.menuidval = ""
                ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1033") Then
                    Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=" + sptype, CType(Request.QueryString("appid"), Integer))
                    Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                    Me.whotelatbcontrol.menuidval = ""

                ElseIf sptype = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1501") Then

                    If CType(Request.QueryString("appid"), String) = 1 Then
                        Me.SubMenuUserControl1.menuidval = Objuser.GetPMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx", String), CType(Request.QueryString("appid"), Integer), 39)
                        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                        Me.whotelatbcontrol.menuidval = ""
                    Else
                        Me.SubMenuUserControl1.menuidval = Objuser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplierSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String) + "&type=OTH", CType(Request.QueryString("appid"), Integer))
                        Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                        Me.whotelatbcontrol.menuidval = "" 'objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))

                    End If


                End If

            End If

            fillgrd(gv_Email, True)
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                txt = GVRow.FindControl("txtContactNo")

                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next
            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                lblHeading.Text = "Add New Supplier" + " - " + PanelEmail.GroupingText
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")

            ElseIf CType(Session("SupState"), String) = "Edit" Then

                BtnEmailSave.Text = "Update"
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                SetFocus(gv_Email)
                lblHeading.Text = "Edit Supplier" + " - " + PanelEmail.GroupingText
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier?')==false)return false;")

            ElseIf CType(Session("SupState"), String) = "View" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                BtnAdd.Visible = False
                gv_Email.Enabled = False
                lblHeading.Text = "View Supplier" + " - " + PanelEmail.GroupingText
                BtnEmailSave.Visible = False
                BtnEmailCancel.Text = "Return to Search"
                disablegrid()
            ElseIf CType(Session("SupState"), String) = "Delete" Then
                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                BtnAdd.Visible = False
                txtCode.Disabled = True
                txtName.Disabled = True
                gv_Email.Enabled = False
                BtnAdd.Visible = False
                disablegrid()
                lblHeading.Text = "Delete Supplier" + " - " + PanelEmail.GroupingText
                BtnEmailSave.Text = "Delete"
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier?')==false)return false;")
            End If
            BtnEmailCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        End If
        Me.whotelatbcontrol.partyval = txtCode.Value
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
                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txt As HtmlInputText
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from partymast_mulltiemail Where partycode='" & RefCode & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from partymast_mulltiemail Where partycode='" & RefCode & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In gv_Email.Rows
                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("ContactPerson")) = False Then
                            txt = GVRow.FindControl("txtPerson")
                            txt.Value = mySqlReader("ContactPerson")
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txt = GVRow.FindControl("txtEmail")
                            txt.Value = mySqlReader("email")
                        End If
                        If IsDBNull(mySqlReader("contactno")) = False Then
                            txt = GVRow.FindControl("txtContactNo")
                            txt.Value = mySqlReader("contactno")
                        End If
                        If IsDBNull(mySqlReader("designation")) = False Then
                            txt = GVRow.FindControl("txtdesignation")
                            txt.Value = mySqlReader("designation")
                        End If
                    End If
                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupMultiMail.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

#Region "Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
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
        Dim designation(count) As String
        'Dim chk(count) As Boolean
        Dim n As Integer = 0
        Try
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                name(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtEmail")
                email(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtContactNo")
                contact(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtdesignation")
                designation(n) = CType(Trim(txt.Value), String)
                n = n + 1
            Next
            fillgrd(gv_Email, False, gv_Email.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_Email.Rows
                If n = i Then
                    Exit For
                End If
                'txtPerson txtEmail txtContactNo
                txt = GVRow.FindControl("txtPerson")
                txt.Value = name(n)
                txt = GVRow.FindControl("txtEmail")
                txt.Value = email(n)
                txt = GVRow.FindControl("txtContactNo")
                txt.Value = contact(n)
                txt = GVRow.FindControl("txtdesignation")
                txt.Value = designation(n)
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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email length is too large. Please enter valid email example(abc@abc.com).');", True)
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

#Region " Protected Sub BtnEmailSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnEmailSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim txtdesignation As HtmlInputText
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("SupState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupState") = "Edit" Then

                    If ValidateEmail() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    mySqlCmd = New SqlCommand("sp_del_partymast_mulltiemail", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                    'SQL  Trans start
                    'mySqlCmd = New SqlCommand("delete from partymast_mulltiemail where partycode='" & txtCode.Value & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In gv_Email.Rows
                        txtName = GvRow.FindControl("txtPerson")
                        txtEmail = GvRow.FindControl("txtEmail")
                        txtContact = GvRow.FindControl("txtContactNo")
                        txtdesignation = GvRow.FindControl("txtdesignation")
                        If CType(txtName.Value, String) <> "" And CType(txtEmail.Value, String) <> "" And CType(txtContact.Value, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_partymast_mulltiemail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactperson", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtEmail.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactno", SqlDbType.VarChar, 50)).Value = CType(txtContact.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@designation", SqlDbType.VarChar, 200)).Value = CType(txtdesignation.Value.Trim, String)
                            '  mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("SupState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

               

                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
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
                    'mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partysplevents", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("sp_del_partyinfo", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
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
                    'Response.Redirect("SupplierAgentsSearch.aspx", False)
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
            objUtils.WritErrorLog("SupMultiMail.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnEmailCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnEmailCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMultiMail','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
