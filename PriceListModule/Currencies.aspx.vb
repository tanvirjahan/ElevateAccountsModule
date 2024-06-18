'------------================--------------=======================------------------================
'   Page Name       :   Currencies.aspx
'   Developer Name  :    Pramod Desai
'   Date            :    7 June 2008
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient

Partial Class Currencies
    Inherits System.Web.UI.Page
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
        If Page.IsPostBack = False Then


           


            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                ViewState.Add("CurrenciesState", Request.QueryString("State"))
                ViewState.Add("CurrenciesRefCode", Request.QueryString("RefCode"))
                ViewState.Add("CurrValue", Request.QueryString("Value"))


                lblcurr.Text = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                lblcurrinv.Text = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")

                strSqlQry = ""
                strSqlQry = "select currcode,curname from WebServiceCurrencyMaster  order by currcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlwebcurrency, "currcode", "curname", strSqlQry, True)
                strSqlQry = ""
                strSqlQry = "select curname,currcode from WebServiceCurrencyMaster order by curname"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlwebCurrencyName, "curname", "currcode", strSqlQry, True)


                'If ViewState("CurrenciesState") = "New" Then
                If ViewState("CurrenciesState") = "New" Then

                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Currency"
                    Page.Title = Page.Title + " " + "New Currency Master"
                    btnSave.Text = "Save"

                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save currency?')==false)return false;")
                    ' btnSave.Attributes.Add("onclick", "return ValidationForExchate('New')")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("CurrenciesState") = "Edit" Then

                    SetFocus(txtName)
                    lblHeading.Text = "Edit Currency"
                    Page.Title = Page.Title + " " + "Edit Currency Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("CurrenciesRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update currency?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("CurrenciesState") = "View" Then

                    SetFocus(btnCancel)
                    lblHeading.Text = "View Currency"
                    Page.Title = Page.Title + " " + "View Currency Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("CurrenciesRefCode"), String))

                ElseIf ViewState("CurrenciesState") = "Delete" Then

                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Currency"
                    Page.Title = Page.Title + " " + "Delete Currency Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("CurrenciesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete currency?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                'txtCode.Attributes.Add("onblur", "changeText()")

                '                btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Currencies.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            strSqlQry = ""
            strSqlQry = "select curname,currcode from WebServiceCurrencyMaster order by curname"
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlwebCurrencyName, "curname", "currcode", strSqlQry, True, currhdn.Value)

            strSqlQry = ""
            strSqlQry = "select currcode,curname from WebServiceCurrencyMaster  order by currcode"
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlwebcurrency, "currcode", "curname", strSqlQry, True, ddlwebCurrencyName.Items(ddlwebCurrencyName.SelectedIndex).Text)

        End If
        Page.Title = "Currency Entry"
    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("CurrenciesState") = "View" Or ViewState("CurrenciesState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            txtCoin.Disabled = True
            txtConversion.Disabled = True
            chkActive.Disabled = True
        ElseIf ViewState("CurrenciesState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Try

            If Page.IsValid = True Then
                'If ViewState("CurrenciesState") = "New" Or ViewState("CurrenciesState") = "Edit" Then
                If ViewState("CurrenciesState") = "New" Or ViewState("CurrenciesState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("CurrenciesState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_curr", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("CurrenciesState") = "Edit" Then
                        frmmode = 2
                        mySqlCmd = New SqlCommand("sp_mod_curr", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcoin", SqlDbType.VarChar, 10)).Value = CType(txtCoin.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    If CType(txtConversion.Value.Trim, String) = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = System.DBNull.Value
                    Else
                        Dim param1 As New SqlParameter("@convrate", SqlDbType.Decimal)
                        param1.Precision = 18
                        param1.Scale = 12
                        param1.Value = CType(txtConversion.Value.Trim, Decimal)
                        mySqlCmd.Parameters.Add(param1)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(txtConversion.Value.Trim, Decimal)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(Format(CType(txtConversion.Value.Trim, Decimal), "#,####.000"), Decimal)
                    End If

                    If ddlwebcurrency.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@webcurrcode", SqlDbType.VarChar, 20)).Value = CType(ddlwebcurrency.Items(ddlwebcurrency.SelectedIndex).Text, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@webcurrcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                    End If
                    If CType(txtinvconversion.Value.Trim, String) = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@invcovrate", SqlDbType.Decimal, 18, 12)).Value = System.DBNull.Value
                    Else
                        Dim param As New SqlParameter("@invcovrate", SqlDbType.Decimal)
                        param.Precision = 18
                        param.Scale = 12
                        param.Value = CType(txtinvconversion.Value.Trim, Decimal)
                        mySqlCmd.Parameters.Add(param)
                        'mySqlCmd.Parameters.Add(New SqlParameter("@invcovrate", SqlDbType.Decimal)).Value = CType(txtinvconversion.Value.Trim, Decimal)


                        'mySqlCmd.Parameters.Add(New SqlParameter("@invcovrate", SqlDbType.Decimal, 18, 12)).Value = CType(Format(CType(txtinvconversion.Value.Trim, Decimal), "#,####.000"), Decimal)
                    End If

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("CurrenciesState") = "Delete" Then
                    frmmode = 3
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_curr", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    'added for log purpose
                    mySqlCmd.ExecuteNonQuery()
                End If
                strPassQry = ""
                '                result1 = strPassQry
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)
                'connection close
                'Response.Redirect("CurrenciesSearch.aspx", False)







                If ViewState("CurrenciesState") = "Edit" Or ViewState("CurrenciesState") = "View" Or ViewState("CurrenciesState") = "Delete" Then
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('CurrWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
                If ViewState("CurrenciesState") = "New" Then
                    If ViewState("CurrValue") <> "Addfrom" And ViewState("CurrValue") <> "AddCurrfrom" Then
                        Dim strpop As String = ""
                        'strpop = "window.open('Currencies.aspx?State=New','Currencies','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                        strpop = "window.open('CurrencyConversionRates.aspx?&FromCurr=" + CType(txtCode.Value.Trim, String) + "','Currencies');"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                    End If
                    If ViewState("CurrValue") = "Addfrom" Then
                        Session.Add("CurrCode", txtCode.Value)
                        Session.Add("CurrName", txtName.Value)
                        Dim strscript1 As String = ""


                        strscript1 = "window.opener.__doPostBack('SupcatsfromWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                    End If
                    If ViewState("CurrValue") = "AddCurrfrom" Then
                        Session.Add("CurrCode", txtCode.Value)
                        Session.Add("CurrName", txtName.Value)
                        Dim strscript1 As String = ""


                        strscript1 = "window.opener.__doPostBack('CurrencyWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                    End If
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If











            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Currencies.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try





    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("Select * from currmast Where currcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("currcode"), String)
                        lblText.Text = "1 " & CType(mySqlReader("currcode") & " = ", String)
                        lblTextinv.Text = "1 " & CType(mySqlReader("currcode") & " = ", String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("currname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("currname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("currcoin")) = False Then
                        Me.txtCoin.Value = CType(mySqlReader("currcoin"), String)
                    Else
                        Me.txtCoin.Value = ""
                    End If
                    If IsDBNull(mySqlReader("convrate")) = False Then
                        'Me.txtConversion.Value = CType(mySqlReader("convrate"), String)
                        'Me.txtConversion.Value = Format(CType(mySqlReader("convrate"), Decimal), "#,####.000")
                        Me.txtConversion.Value = Val(mySqlReader("convrate"))
                    Else
                        Me.txtConversion.Value = ""
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("Webservicecurrcode")) = False Then
                        ddlwebCurrencyName.Value = CType(mySqlReader("Webservicecurrcode"), String)
                        currhdn.Value = CType(mySqlReader("Webservicecurrcode"), String)
                    End If
                    If IsDBNull(mySqlReader("currname")) = False Then
                        ddlwebcurrency.Value = ddlwebCurrencyName.Items(ddlwebCurrencyName.SelectedIndex).Text
                    End If

                    If IsDBNull(mySqlReader("invconvrate")) = False Then

                        'Me.txtinvconversion.Value = Format(CType(mySqlReader("invconvrate"), Decimal), "#,####.000")
                        Me.txtinvconversion.Value = Val(mySqlReader("invconvrate"))
                    Else
                        Me.txtinvconversion.Value = ""
                    End If

                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Currencies.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CurrenciesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    '#Region "Validation for enter only number"
    '    Private Sub ValidateOnlyNumber()
    '        txtConversion.Attributes.Add("onkeypress", "return checkNumber(event)")
    '    End Sub 

    '#End Region
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a Customers, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "ctrymast", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a Country, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a PriceList, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew_convrates", "curcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a PriceList Conversionrates, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othsellmast", "curcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a OtherService SellingTypes, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a Suppliers, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellmast", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a Selling PriceTypes, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellspcath", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a SellingFormulaForCategories, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a SpecialEventsorExtras Pricelist, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supplier_agents", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a SupplierAgents, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktsellmast", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a TicketSellingTypes, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costh", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a OtherServiceCostPricelist, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplisthnew", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a TicketPriceList, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplisth", "currcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a OtherServicesPriceList, cannot delete this Currency');", True)
            checkForDeletion = False
            Exit Function


        End If

        checkForDeletion = True
    End Function
#End Region




#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If ViewState("CurrenciesState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "currmast", "currcode", txtCode.Value.Trim) Then
                'objUtils.MessageBox("This currency code is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "currmast", "currname", txtName.Value.Trim) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("CurrenciesState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "currmast", "currcode", "currname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ' Response.Write("<script language='javascript'> nw=window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=54,left=760,width=250,height=600'); </script>")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
