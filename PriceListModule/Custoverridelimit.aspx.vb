Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class Custoverridelimit
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
    Dim myDataAdapter As New SqlDataAdapter
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            PanelGeneral.Visible = True

            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            Numbers(txtoverridelimit)
            GetValuesForGeneralDetails()


            If CType(Session("custState"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add Customer - Allow User Booking"
                Page.Title = Page.Title + " " + "New Customer - Allow User Booking"
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer general?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "Edit" Then

                BtnGeneralSave.Text = "Update"

                RefCode = CType(Session("custrefcode"), String)
                'If CType(Session("ExistClient"), String) = "1" Then
                '    BtnGeneralSave.Visible = False
                '    DisableControl()
                'End If
                ShowRecord(RefCode)
                fillgrid(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Edit Customer - Allow User Booking"
                Page.Title = Page.Title + " " + "Edit Customer - Allow User Booking"
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer general?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "View" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                fillgrid(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Customer - Allow User Booking"
                Page.Title = Page.Title + " " + "View Customer - Allow User Booking"
                BtnGeneralSave.Visible = False
                BtnGeneralCancel.Text = "Return to Search"
                BtnGeneralCancel.Focus()

            ElseIf CType(Session("custState"), String) = "Delete" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                fillgrid(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Delete Customer - Allow User Booking"
                Page.Title = Page.Title + " " + "Delete Customer - Allow User Booking"
                BtnGeneralSave.Text = "Delete"
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer general?')==false)return false;")
            End If
            BtnGeneralCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If
        End If
        Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '   Response.Redirect("CustomersSearch.aspx")
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
        If Session("custState") = "New" Then
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

#Region "Public Function Validateamendments() As Boolean"
    Public Function Validateamendments() As Boolean
        Validateamendments = True
        If Val(txtoverridelimit.Value) = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select authorized user.');", True)
            Validateamendments = False
        End If
    End Function
#End Region
#Region " Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) "
    Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Dim sno As Integer = 0
        Dim ds As New DataSet

        Try
            If Page.IsValid = True Then

                If Validateamendments() = False Then
                    Exit Sub
                End If

                If Session("custState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custState") = "Edit" Then
                    ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(max(sno),0)+1 sno from agentmast_override_log(nolock) where agentcode='" & txtCustomerCode.Value & "' ")
                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            sno = ds.Tables(0).Rows(0)("sno")
                        End If
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("custState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_update_agentmast_override_log", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("custState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_update_agentmast_override_log", mySqlConn, sqlTrans)
                        frmmode = 2

                        Dim mySqlCmdAmend As SqlCommand
                        mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmdAmend.CommandType = CommandType.Text
                        mySqlCmdAmend.ExecuteNonQuery()
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sno", SqlDbType.VarChar, 20)).Value = CType(sno, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@overridelimit", SqlDbType.Decimal, 6, 2)).Value = CType(txtoverridelimit.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtremarks.Text.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_update_agentmast_overridelimit", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@overridelimit", SqlDbType.Decimal, 6, 2)).Value = CType(txtoverridelimit.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sno", SqlDbType.Decimal, 6, 2)).Value = CType(sno, Integer)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("custState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    Dim mySqlCmdAmend As SqlCommand
                    mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmdAmend.CommandType = CommandType.Text
                    mySqlCmdAmend.ExecuteNonQuery()
                    'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast_allowuser_log where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd = New SqlCommand("delete from agentmast_override_log  where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text

                    mySqlCmd.ExecuteNonQuery()
                End If


                strPassQry = ""
               




                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("custState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("custState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("custState") = "Delete" Then
                    ' Response.Redirect("CustomersSearch.aspx", False)
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
            objUtils.WritErrorLog("CustGen.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("CustGen.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Private Sub fillgrid(ByVal RefCode As String)"
    Private Sub fillgrid(ByVal RefCode As String)
        Try

            Dim myDS As New DataSet

            gv_actionuserlog.Visible = True

            If gv_actionuserlog.PageIndex < 0 Then
                gv_actionuserlog.PageIndex = 0
            End If
            strSqlQry = ""
            strSqlQry = "select top 100 sno,requestid,overridelimit,odate,remarks from agentmast_override_log where agentcode='" & RefCode & "' order by sno DESC "


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_actionuserlog.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_actionuserlog.DataBind()
            Else
                gv_actionuserlog.PageIndex = 0
                gv_actionuserlog.DataBind()
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustGen.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        txtremarks.ReadOnly = True

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

#Region "   Private Sub GetValuesForGeneralDetails()"
    Private Sub GetValuesForGeneralDetails()
        Try
            'If session("custState") = "Edit" Then
            If Session("custState") = "Edit" Or Session("custState") = "View" Or Session("custState") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  General Details ------------------------------------
                        If IsDBNull(mySqlReader("general")) = False Then
                            txtremarks.Text = mySqlReader("general")
                        Else
                            txtremarks.Text = ""
                        End If
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustGen','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
