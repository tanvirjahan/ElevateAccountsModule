Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class CustSalesDet
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
            'changed Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            If CType(Request.QueryString("appid"), String) = "1" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomerGroupSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=1", String), "1")

            End If

            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            '16082014
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConcierge, "spersonname", "spersoncode", "select ltrim(rtrim(spersoncode))spersoncode,ltrim(rtrim(spersonname))spersonname from spersonmast where active=1 order by spersonname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesExpert, "spersonname", "spersoncode", "select ltrim(rtrim(spersoncode))spersoncode,ltrim(rtrim(spersonname))spersonname from spersonmast_office  order by spersonname", True)


            PanelSales.Visible = True
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            GetValuesForSalesDetails()



            If CType(Session("custstate"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add New Customer - Sale Details"
                Page.Title = Page.Title + " " + "New Customer - Sale Details"
                BtnSaleSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")

                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer sale details?')==false)return false;")
            ElseIf CType(Session("custstate"), String) = "Edit" Then

                BtnSaleSave.Text = "Update"

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Edit Customer - Sale Details"
                Page.Title = Page.Title + " " + "Edit Customer - Sale Details"
                BtnSaleSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")


                ' BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer sale details?')==false)return false;")
            ElseIf CType(Session("custstate"), String) = "View" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Customer - Sale Details"
                Page.Title = Page.Title + " " + "View Customer - Sale Details"
                BtnSaleSave.Visible = False
                BtnSaleCancel.Text = "Return to Search"
                BtnSaleCancel.Focus()

            ElseIf CType(Session("custstate"), String) = "Delete" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)

                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Delete Customer - Sale Details"
                Page.Title = Page.Title + " " + "Delete Customer - Sale Details"
                BtnSaleSave.Text = "Delete"
                DisableControl()
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer sale details?')==false)return false;")
                'ElseIf CType(Session("custstate"), String) = "Addclient" Then
                '    If CType(Session("ExistClient"), String) = "1" Then
                '        BtnSaleSave.Visible = False
                '        DisableControl()
                '    End If
            End If
            BtnSaleCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If
            End If
            Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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
        If Session("custstate") = "New" Then
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
        ElseIf Session("custstate") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", txtCustomerName.Value.Trim, CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
#Region "Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaleSave.Click"
    Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaleSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim mode As String = ""
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Try
            If Page.IsValid = True Then

              

                If Session("custstate") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custstate") = "Edit" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("custstate") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_agentmast", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("custstate") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_agentmast", mySqlConn, sqlTrans)
                        frmmode = 2


                        'Dim mySqlCmdAmend As SqlCommand
                        'mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        'mySqlCmdAmend.CommandType = CommandType.Text
                        'mySqlCmdAmend.ExecuteNonQuery()

                    End If


                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@recommby", SqlDbType.VarChar, 100)).Value = CType(txtSaleRecommended.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel1", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel2", SqlDbType.VarChar, 50)).Value = CType(txtSaleTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@smobile", SqlDbType.VarChar, 50)).Value = CType(txtsalesmob.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sfax", SqlDbType.VarChar, 50)).Value = CType(txtSaleFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact1", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact2", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@semail", SqlDbType.VarChar, 100)).Value = CType(txtSaleEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If (CType(ddlConcierge.Value, String) <> "[Select]") Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@conspcode", SqlDbType.VarChar, 10)).Value = CType(ddlConcierge.Value, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@conspcode", SqlDbType.VarChar, 10)).Value = DBNull.Value

                    End If
                    If (CType(ddlSalesExpert.Value, String) <> "[Select]") Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@sespcode", SqlDbType.VarChar, 20)).Value = CType(ddlSalesExpert.Value, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@sespcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    mySqlCmd.ExecuteNonQuery()
                    ElseIf Session("custstate") = "Delete" Then
                        If checkForDeletion() = False Then
                            Exit Sub
                        End If
                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                        frmmode = 3


                        Dim mySqlCmdAmend As SqlCommand
                        mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmdAmend.CommandType = CommandType.Text
                        mySqlCmdAmend.ExecuteNonQuery()


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
                    If Session("custstate") = "New" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                    ElseIf Session("custstate") = "Edit" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    End If
                    If Session("custstate") = "Delete" Then
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
            objUtils.WritErrorLog("CustSalesDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("CustSalesDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        txtSaleRecommended.Disabled = True
        txtSaleTelephone1.Disabled = True
        txtSaleTelephone2.Disabled = True
        txtSaleFax.Disabled = True
        txtSaleContact1.Disabled = True
        txtSaleContact2.Disabled = True
        txtSaleEmail.Disabled = True
        txtsalesmob.Disabled = True
        ddlConcierge.Disabled = True
        ddlSalesExpert.Disabled = True

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

#Region "Private Sub GetValuesForSalesDetails()"
    Private Sub GetValuesForSalesDetails()
        Try
            If Session("custstate") = "Edit" Or Session("custstate") = "View" Or Session("custstate") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                'mySqlCmd = New SqlCommand("Select * from agentcatmast Where agentcode='" & session("custrefcode") & "'", mySqlConn)
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  Sales Details ------------------------------------
                        If IsDBNull(mySqlReader("recommby")) = False Then
                            txtSaleRecommended.Value = mySqlReader("recommby")
                        Else
                            txtSaleRecommended.Value = ""
                        End If
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
                            txtsalesmob.Value = mySqlReader("smobileno")
                        Else
                            txtsalesmob.Value = ""
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

                        If IsDBNull(mySqlReader("conspcode")) = False Then
                            ddlConcierge.Value = mySqlReader("conspcode")
                       
                        End If

                        If IsDBNull(mySqlReader("sespcode")) = False Then
                            ddlSalesExpert.Value = mySqlReader("sespcode")

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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustSalesDet','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



    '<WebMethod()> _
    'Public Function Reverse_UpdateCustSalesDet(ByVal constr As String, ByVal code As String,
    '                                            ByVal frmmode As String) As String

    '    If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
    '        Reverse_UpdateCustSalesDet = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
    '        Exit Function
    '    End If


    '    Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


    '    If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
    '        Reverse_UpdateCustSalesDet = "Permission Denied"
    '        Exit Function
    '    End If

    '    Dim result As Integer
    '    Dim spname As String


    '    Dim parms As New List(Of SqlParameter)
    '    Dim parm(10) As SqlParameter
    '    parm(10) = New SqlParameter
    '    parm(0) = New SqlParameter("@agentcode", CType(code, String))
    '    If frmmode > 1 Then
    '        Dim recomndby, stel1, stel2, smobile, sfax, scontact1, scontact2, semail, userlogged As String

    '        Dim active As Integer

    '        Dim ds As New DataSet
    '        ds = objUtils.GetDataFromDatasetnew(constr, "select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")

    '        If ds.Tables(0).Rows.Count > 0 Then
    '            recomndby = ds.Tables(0).Rows(0).Item("recommby")
    '            stel1 = ds.Tables(0).Rows(0).Item("stel1")
    '            stel2 = ds.Tables(0).Rows(0).Item("stel2")
    '            smobile = ds.Tables(0).Rows(0).Item("smobile")
    '            sfax = ds.Tables(0).Rows(0).Item("sfax")
    '            scontact1 = ds.Tables(0).Rows(0).Item("scontact1")
    '            scontact2 = ds.Tables(0).Rows(0).Item("scontact2")
    '            semail = ds.Tables(0).Rows(0).Item("semail")
    '            userlogged = ds.Tables(0).Rows(0).Item("moduser")
    '        End If
    '        parm(1) = New SqlParameter("@recommby", CType(recomndby, String))
    '        parm(2) = New SqlParameter("@stel1", CType(stel1, String))
    '        parm(3) = New SqlParameter("@stel2", CType(stel2, String))
    '        parm(4) = New SqlParameter("@smobile", CType(smobile, String))
    '        parm(5) = New SqlParameter("@sfax", CType(sfax, String))
    '        parm(6) = New SqlParameter("@scontact1", CType(scontact1, String))
    '        parm(7) = New SqlParameter("@scontact2", CType(scontact2, String))
    '        parm(8) = New SqlParameter("@semail", CType(semail, String))
    '        parm(9) = New SqlParameter("@userlogged", CType(userlogged, String))

    '    End If
    '    If frmmode = 1 Then
    '        parms.Add(parm(0))
    '        spname = "sp_del_agentmast"
    '        'Dim result_temp As String
    '        'result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "update registration  set approve=null,approvedate=null,approveuser=null  where regno='" & CType(code, String) & "'")

    '    End If
    '    If frmmode = 2 Then
    '        For p = 0 To 9
    '            parms.Add(parm(p))
    '        Next

    '        spname = "sp_updatesales_agentmast"

    '    End If

    '    If frmmode = 3 Then
    '        Dim result_temp As String
    '        result = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast select * from tempservicesave_agentmast where agentcode='" & CType(code, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into agentmast_mulltiemail  select * from tempservicesaveagentmast_mulltiemail where agentcode='" & CType(code, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_visit select * from tempservicesave_agentmast_visit where agentcode='" & CType(code, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  agentmast_survey  select * from tempservicesave_agentmast_survey   where agentcode='" & CType(code, String) & "'")

    '    End If

    '    If frmmode = 1 Or frmmode = 2 Then
    '        result = objUtils.ExecuteNonQuerynew(constr, spname, parms)

    '    End If
    '    result = objUtils.ExecuteNonQuerynew(constr, spname, parms)

    '    If result = Nothing Or IsDBNull(result) Then
    '        result = ""

    '    End If

    '    Return result
    'End Function


End Class
