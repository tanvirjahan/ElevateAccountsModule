Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class CustSubUser
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
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            If Session("custState") = "New" Then
                Response.Redirect("CustMainDet.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If
            PanelGeneral.Visible = True

            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            GetValuesForGeneralDetails()

            If CType(Session("custState"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add New Customer - Sub User"
                Page.Title = Page.Title + " " + "New Customer - Sub User"
            ElseIf CType(Session("custState"), String) = "Edit" Then
                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Edit Customer - Sub User"
                Page.Title = Page.Title + " " + "Edit Customer - Sub User"
                FillOtherGroups()
            ElseIf CType(Session("custState"), String) = "View" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True

                lblHeading.Text = "View Customer - Sub User"
                Page.Title = Page.Title + " " + "View Customer - Sub User"
                BtnGeneralCancel.Text = "Return to Search"
                BtnGeneralCancel.Focus()
                FillOtherGroups()

            ElseIf CType(Session("custState"), String) = "Delete" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Delete Customer - Sub User"
                Page.Title = Page.Title + " " + "Delete Customer - Sub User"
                FillOtherGroups()
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
        'Response.Redirect("CustomersSearch.aspx")

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
    '#Region " Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGeneralSave.Click"
    '    Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGeneralSave.Click
    '        Try
    '            If Page.IsValid = True Then
    '                If Session("custState") = "New" Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
    '                    Exit Sub
    '                ElseIf Session("custState") = "Edit" Then

    '                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

    '                    If Session("custState") = "New" Then
    '                        mySqlCmd = New SqlCommand("sp_updatecom_agentmast", mySqlConn, sqlTrans)
    '                    ElseIf Session("custState") = "Edit" Then

    '                        mySqlCmd = New SqlCommand("sp_updatecom_agentmast", mySqlConn, sqlTrans)
    '                    End If

    '                    mySqlCmd.CommandType = CommandType.StoredProcedure
    '                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
    '                    mySqlCmd.Parameters.Add(New SqlParameter("@general", SqlDbType.Text)).Value = CType(txtGeneral.Text.Trim, String)
    '                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
    '                    mySqlCmd.ExecuteNonQuery()
    '                ElseIf Session("custState") = "Delete" Then
    '                    If checkForDeletion() = False Then
    '                        Exit Sub
    '                    End If
    '                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

    '                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
    '                    mySqlCmd.CommandType = CommandType.StoredProcedure
    '                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
    '                    mySqlCmd.ExecuteNonQuery()
    '                    mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
    '                    mySqlCmd.CommandType = CommandType.Text
    '                    mySqlCmd.ExecuteNonQuery()
    '                End If
    '                sqlTrans.Commit()    'SQl Tarn Commit
    '                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
    '                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
    '                If Session("custState") = "New" Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
    '                ElseIf Session("custState") = "Edit" Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
    '                End If
    '                If Session("custState") = "Delete" Then
    '                    Response.Redirect("CustomersSearch.aspx", False)
    '                End If
    '            End If
    '        Catch ex As Exception
    '            If mySqlConn.State = ConnectionState.Open Then
    '                sqlTrans.Rollback()
    '            End If
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '            objUtils.WritErrorLog("CustGen.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        End Try
    '    End Sub
    '#End Region


    Private Sub FillOtherGroups()
        Dim GrupCode As String = ""
        Dim myadapter As SqlDataAdapter

        Try

            Dim MyGroupDS As New DataSet
            strSqlQry = "select a.agent_sub_code,a.sub_user_name,a.sub_user_email from agents_subusers a where a.agentcode='" & txtCustomerCode.Value & "'"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myadapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myadapter.Fill(MyGroupDS, "agents_subusers")
            Gv_subuser.DataSource = MyGroupDS
            Gv_subuser.DataBind()
            myadapter.Dispose()
            mySqlConn.Close()



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            mySqlConn.Close()
        End Try
    End Sub




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
            'If Session("custState") = "Edit" Then
            If Session("custState") = "Edit" Or Session("custState") = "View" Or Session("custState") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  General Details ------------------------------------

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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustsubUser','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
