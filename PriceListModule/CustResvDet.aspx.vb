Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class CustResvDet
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
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            'changed   Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            If CType(Request.QueryString("appid"), String) = "1" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))


            ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomerGroupSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=1", String), "1")

            End If

            PanelReservstion.Visible = True
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            GetValuesForResvationDetails()

            If CType(Session("custState"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add New Customer - Reservation Details"
                Page.Title = Page.Title + " " + "New Customer - Reservation Details"
                BtnResSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer reservation details?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "Edit" Then

                BtnResSave.Text = "Update"

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Edit Customer - Reservation Details"
                Page.Title = Page.Title + " " + "Edit Customer - Reservation Details"
                BtnResSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer reservation details?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "View" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Customer - Reservation Details"
                Page.Title = Page.Title + " " + "View Customer - Reservation Details"
                BtnResSave.Visible = False
                BtnResCancel.Text = "Return to Search"
                BtnResCancel.Focus()

            ElseIf CType(Session("custState"), String) = "Delete" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Delete Customer - Reservation Details"
                Page.Title = Page.Title + " " + "Delete Customer - Reservation Details"
                BtnResSave.Text = "Delete"
                DisableControl()
                BtnResSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer reservation details?')==false)return false;")
            ElseIf CType(Session("CustState"), String) = "Addclient" Then

                Dim clientname As String
                Dim webuser As String
                SetFocus(txtCustomerCode)
                clientname = CType(Session("clientname"), String)
                webuser = CType(Session("webusername"), String)
                'If CType(Session("ExistClient"), String) = "1" Then
                '    DisableControl()
                '    BtnResSave.Visible = False
                'End If
                ShowRecord_registration(clientname, webuser)

                lblHeading.Text = "Add New Customer - Main Details"
                Page.Title = Page.Title + " " + "New Customer - Reservation Details"
                BtnResSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")


            End If
            BtnResCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If
        End If
        Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Response.Redirect("CustomersSearch.aspx")
        If Session("postback") <> "Addclient" Then
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Else

            Dim strscript As String = ""
            strscript = "window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


        End If
      
    End Sub
#End Region
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("custState") = "New" Or Session("CustState") = "Addclient" Then
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
        ElseIf Session("custState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", txtCustomerName.Value.Trim, CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
#Region "Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnResSave.Click"
    Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnResSave.Click
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim mode As String = ""
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        'to ensure  its add mode

        Try
            If Page.IsValid = True Then

                'If Session("custState") = "Edit" Then


                'End If
                If Session("custState") = "New" Or Session("CustState") = "Addclient" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custState") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("custState") = "New" Or Session("CustState") = "Addclient" Then
                        mySqlCmd = New SqlCommand("sp_updateres_agentmast", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("custState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updateres_agentmast", mySqlConn, sqlTrans)
                        frmmode = 2

                        'Dim mySqlCmdAmend As SqlCommand
                        'mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        'mySqlCmdAmend.CommandType = CommandType.Text
                        'mySqlCmdAmend.ExecuteNonQuery()

                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 500)).Value = CType(txtResAddress1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = CType(txtResAddress2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = CType(txtResAddress3.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(txtResPhone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = CType(txtResPhone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(txtResFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = CType(txtResContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = CType(txtResContact2.Value.Trim, String)
                    If CType(txtdesignation.Value, String) = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@designation", SqlDbType.VarChar, 100)).Value = CType(txtdesignation.Value.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@designation", SqlDbType.VarChar, 100)).Value = String.Empty
                    End If

                    If CType(txtiatano.Value, String) = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@iatano", SqlDbType.VarChar, 100)).Value = CType(txtiatano.Value.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@iatano", SqlDbType.VarChar, 100)).Value = String.Empty
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtResEmail.Value.Trim, String)

                    If txtweb.Value = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.VarChar, 100)).Value = CType(txtweb.Value.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.VarChar, 100)).Value = String.Empty
                    End If

                    If ddlCommunicateBy.SelectedValue = "Email" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int)).Value = 1
                    ElseIf ddlCommunicateBy.SelectedValue = "Fax" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int)).Value = 0

                    ElseIf ddlCommunicateBy.SelectedValue = "Both" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int)).Value = 2

                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@mobileno", SqlDbType.VarChar, 50)).Value = CType(txtresmob.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("custState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    'If CheckDeleteRelationShip() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction



                    'Dim mySqlCmdAmend As SqlCommand
                    'mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmdAmend.CommandType = CommandType.Text
                    'mySqlCmdAmend.ExecuteNonQuery()

                    'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""





                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("custState") = "New" Or Session("CustState") = "Addclient" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("custState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("custState") = "Delete" Then
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If

            End If

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

              
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustResvDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("CustResvDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Private Sub ShowRecord_registration(ByVal agentname As String,ByVal webuserid as string)"
    Private Sub ShowRecord_registration(ByVal agentname As String, ByVal webuserid As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from registration Where agentname='" & agentname & "' and webusername='" & webuserid & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("designation")) = False Then
                        Me.txtCustomerName.Value = mySqlReader("designation")
                    End If
                    If IsDBNull(mySqlReader("iatano")) = False Then
                        Me.txtCustomerName.Value = mySqlReader("iatano")
                    End If

                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        txtResAddress1.Disabled = True
        txtResAddress2.Disabled = True
        txtResAddress3.Disabled = True
        txtResPhone1.Disabled = True
        txtResPhone2.Disabled = True
        txtresmob.Disabled = True
        txtResFax.Disabled = True
        txtResContact1.Disabled = True
        txtResContact2.Disabled = True
        txtdesignation.Disabled = True
        txtiatano.Disabled = True
        txtResEmail.Disabled = True
        txtweb.Disabled = True
        ddlCommunicateBy.Enabled = False

        ddlCommunicateBy.Enabled = False
    End Sub
#End Region


#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromagent_detail", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a DetailsOfEarlyBirdPromotions, cannot delete this Customer');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If
        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_agent", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a CustomerPromotions, cannot delete this Customer');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If

        checkForDeletion = True
    End Function
#End Region

#Region "Private Sub GetValuesForResvationDetails()"
    Private Sub GetValuesForResvationDetails()
        Try
            If Session("custState") = "Edit" Or Session("custState") = "View" Or Session("custState") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '------------------------------------------------------
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
                            txtresmob.Value = mySqlReader("mobileno")
                        Else
                            txtresmob.Value = ""
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

                        If IsDBNull(mySqlReader("designation")) = False Then
                            txtdesignation.Value = mySqlReader("designation")
                        Else
                            txtdesignation.Value = ""
                        End If

                        If IsDBNull(mySqlReader("iatano")) = False Then
                            txtiatano.Value = mySqlReader("iatano")
                        Else
                            txtiatano.Value = ""
                        End If

                        If IsDBNull(mySqlReader("email")) = False Then
                            txtResEmail.Value = mySqlReader("email")
                        Else
                            txtResEmail.Value = ""
                        End If
                        If IsDBNull(mySqlReader("web")) = False Then
                            txtweb.Value = mySqlReader("web")
                        Else
                            txtweb.Value = ""
                        End If

                        If IsDBNull(mySqlReader("commmode")) = False Then
                            If mySqlReader("commmode") = "1" Then
                                ddlCommunicateBy.SelectedValue = "Email"
                            ElseIf mySqlReader("commmode") = "0" Then
                                ddlCommunicateBy.SelectedValue = "Fax"
                            ElseIf mySqlReader("commmode") = "2" Then
                                ddlCommunicateBy.SelectedValue = "Both"
                            End If
                        End If

                        '------------------------END-----------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustResvDet','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
