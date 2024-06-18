Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class CustGen
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
            'changed  Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            If CType(Request.QueryString("appid"), String) = "1" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

                elseif CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomerGroupSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=1", String), "1")

            End If


                PanelGeneral.Visible = True

                charcters(txtCustomerCode)
                charcters(txtCustomerName)
                GetValuesForGeneralDetails()

                If CType(Session("custState"), String) = "New" Then
                    SetFocus(txtCustomerCode)
                    lblHeading.Text = "Add New Customer - General"
                    Page.Title = Page.Title + " " + "New Customer - General"
                    BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer general?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "Edit" Then

                    BtnGeneralSave.Text = "Update"

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    lblHeading.Text = "Edit Customer - General"
                    Page.Title = Page.Title + " " + "Edit Customer - General"
                    BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer general?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "View" Then

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    DisableControl()
                    lblHeading.Text = "View Customer - General"
                    Page.Title = Page.Title + " " + "View Customer - General"
                    BtnGeneralSave.Visible = False
                    BtnGeneralCancel.Text = "Return to Search"
                    BtnGeneralCancel.Focus()

                ElseIf CType(Session("custState"), String) = "Delete" Then

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    DisableControl()
                    lblHeading.Text = "Delete Customer - General"
                    Page.Title = Page.Title + " " + "Delete Customer - General"
                    BtnGeneralSave.Text = "Delete"
                    BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer general?')==false)return false;")
                End If
                BtnGeneralCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

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
#Region " Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGeneralSave.Click"
    Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGeneralSave.Click
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"

        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Try
            If Page.IsValid = True Then
                If Session("custState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custState") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("custState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatecom_agentmast", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("custState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updatecom_agentmast", mySqlConn, sqlTrans)
                        frmmode = 2



                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@general", SqlDbType.Text)).Value = CType(txtGeneral.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
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
        txtGeneral.ReadOnly = True

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
                            txtGeneral.Text = mySqlReader("general")
                        Else
                            txtGeneral.Text = ""
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

    '<WebMethod()> _
    'Public Function UpdateCustGen(ByVal constr As String, ByVal custcode As String, ByVal custGeneral As String,
    '                ByVal userlogged As String, ByVal frmmode As String) As String

    '    'Dim retlist As New List(Of clsMaster)

    '    If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
    '        UpdateCustGen = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
    '        Exit Function
    '    End If

    '    Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


    '    If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
    '        UpdateCustGen = "Permission Denied"
    '        Exit Function
    '    End If
    '    Dim result_temp As String

    '    result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

    '    Dim result As Integer
    '    Dim sqlstr As String

    '    Dim spname As String



    '    Dim p As Integer
    '    Dim parms As New List(Of SqlParameter)
    '    Dim parm(3) As SqlParameter
    '    parm(3) = New SqlParameter
    '    parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
    '    parm(1) = New SqlParameter("@general", CType(custGeneral, String))
    '    parm(2) = New SqlParameter("@userlogged", CType(userlogged, String))

    '    If frmmode = 1 Then
    '        For p = 0 To 2
    '            parms.Add(parm(p))
    '        Next
    '        spname = "sp_updatecom_agentmast"
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

    '    End If

    '    If frmmode = 2 Then

    '        For p = 0 To 2
    '            parms.Add(parm(p))
    '        Next
    '        spname = "sp_updatecom_agentmast"
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

    '    End If
    '    If frmmode = 3 Then
    '        parms.Add(parm(0))
    '        spname = "sp_del_agentmast"
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")

    '    End If



    '    result = objUtils.ExecuteNonQuerynew(constr, spname, parms)
    '    If result = Nothing Or IsDBNull(result) Then
    '        result = ""

    '    End If

    '    Return result
    'End Function


End Class
