#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Imports System.Diagnostics
#End Region

Partial Class AccountsModule_Bankdetails
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection

    Dim sqlTrans As SqlTransaction
    Dim objdate As New clsDateTime
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Try

            If Not HttpContext.Current.Session("divcode") Is Nothing Then
                divid = Convert.ToString(HttpContext.Current.Session("divcode").ToString())
            End If


            strSqlQry = "select acctcode,acctname from acctmast  where bankyn='Y'   and div_code='" & divid & "' and acctname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))

                Next
            End If
            Return acctnames
        Catch ex As Exception
            Return acctnames
        End Try

    End Function


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("BankdetailsState", Request.QueryString("State"))
                ViewState.Add("BankdetailsRefCode", Request.QueryString("RefCode"))
                 objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlacctcurr, "currcode", "currcode", "select currcode from currmast  order by currcode", True)

                'txtconnection.Value = Session("dbconnectionName")

                '''' Division

                ViewState.Add("divcode", Request.QueryString("divid"))
                Session("divcode") = ViewState("divcode")

                '''

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlBankcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlbankname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                If ViewState("BankdetailsState") = "New" Then
                    ' SetFocus(ddlBankcode)
                    lblHeading.Text = "Add Bank Details"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                ElseIf ViewState("BankdetailsState") = "Edit" Then

                    lblHeading.Text = "Edit Bank Details"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("BankdetailsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                ElseIf ViewState("BankdetailsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Bank Details"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("BankdetailsRefCode"), String))
                ElseIf ViewState("BankdetailsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Bank Details"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("BankdetailsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                End If
              
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CostCenterCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region


#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)

          
  
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from bankdetails_master Where bankcode='" & RefCode & "' and div_id='" & ViewState("divcode") & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("bankcode")) = False Then
                        ' ddlbankname.Value = mySqlReader("bankcode")
                        TxtBankCode.Text = mySqlReader("bankcode")
                        TxtBankName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctcode='" & mySqlReader("bankcode") & "'")
                        ' ddlBankcode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctcode='" & mySqlReader("bankcode") & "'")
                           End If

                    If IsDBNull(mySqlReader("dispbankname")) = False Then
                        Me.txtDispBankName.Value = CType(mySqlReader("dispbankname"), String)
                    Else
                        Me.txtDispBankName.Value = ""
                    End If

                    If IsDBNull(mySqlReader("accountcurrency")) = False Then
                        ddlacctcurr.Value = mySqlReader("accountcurrency")
                      
                    End If
                    If IsDBNull(mySqlReader("accountname")) = False Then
                        Me.txtAcctName.Value = CType(mySqlReader("accountname"), String)
                    Else
                        Me.txtAcctName.Value = ""
                    End If


                    If IsDBNull(mySqlReader("accountnumber")) = False Then
                        Me.txtAcctNumber.Value = CType(mySqlReader("accountnumber"), String)
                    Else
                        Me.txtAcctNumber.Value = ""
                    End If



                    If IsDBNull(mySqlReader("ibannumber")) = False Then
                        Me.txtIBANNumber.Value = CType(mySqlReader("ibannumber"), String)
                    Else
                        Me.txtIBANNumber.Value = ""
                    End If



                    If IsDBNull(mySqlReader("swiftcode")) = False Then
                        Me.txtswiftcode.Value = CType(mySqlReader("swiftcode"), String)
                    Else
                        Me.txtswiftcode.Value = ""
                    End If


                    If IsDBNull(mySqlReader("others")) = False Then
                        Me.txtothers.Text = CType(mySqlReader("others"), String)
                    Else
                        Me.txtothers.Text = ""
                    End If


                    If IsDBNull(mySqlReader("branchname")) = False Then
                        Me.txtbranch.Value = CType(mySqlReader("branchname"), String)
                    Else
                        Me.txtbranch.Value = ""
                    End If


                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("CostCenterCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region



#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("BankdetailsState") = "View" Or ViewState("BankdetailsState") = "Delete" Then

            TxtBankCode.Enabled = False
            TxtBankName.Enabled = False

            txtothers.Enabled = False
            txtAcctName.Disabled = True
            txtDispBankName.Disabled = True
            txtAcctNumber.Disabled = True
            txtIBANNumber.Disabled = True
            txtswiftcode.Disabled = True
            txtbranch.Disabled = True
        ElseIf ViewState("BankdetailsState") = "Edit" Then
            'ddlBankcode.Disabled = True
            'ddlbankname.Disabled = True
            TxtBankCode.Enabled = False
            TxtBankName.Enabled = False
        End If

    End Sub

#End Region




#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Try
            If ViewState("BankdetailsState") = "New" Then
                If objUtils.isDuplicatenewdiv(Session("dbconnectionName"), "bankdetails_master", "bankcode", TxtBankCode.Text.Trim, "div_id", ViewState("divcode"), "") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Details for this Bank already present.');", True)
                    'SetFocus(ddlBankcode)
                    SetFocus(TxtBankName.Text)
                    checkForDuplicate = False
                    Exit Function
                End If

            End If
            checkForDuplicate = True
        Catch ex As Exception
            objUtils.WritErrorLog("Bankdetails.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region




#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("BankdetailsState") = "New" Or ViewState("BankdetailsState") = "Edit" Then

                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("BankdetailsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_bankdetails_master", mySqlConn, sqlTrans)
                    ElseIf ViewState("BankdetailsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_bankdetails_master", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.Parameters.Add(New SqlParameter("@bankcode", SqlDbType.VarChar, 20)).Value = CType(ddlBankcode.Items(ddlBankcode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@bankcode", SqlDbType.VarChar, 20)).Value = CType(TxtBankCode.Text.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@bankname", SqlDbType.VarChar, 200)).Value = CType(ddlbankname.Items(ddlbankname.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@bankname", SqlDbType.VarChar, 200)).Value = CType(TxtBankName.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@dispbankname", SqlDbType.VarChar, 200)).Value = CType(txtDispBankName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accountname", SqlDbType.VarChar, 200)).Value = CType(txtAcctName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accountcurrency", SqlDbType.VarChar, 20)).Value = CType(ddlacctcurr.Items(ddlacctcurr.SelectedIndex).Text, String)
                


                mySqlCmd.Parameters.Add(New SqlParameter("@accountnumber", SqlDbType.VarChar, 100)).Value = CType(txtAcctNumber.Value.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@ibannumber", SqlDbType.VarChar, 200)).Value = CType(txtIBANNumber.Value.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@swiftcode", SqlDbType.VarChar, 100)).Value = CType(txtswiftcode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@others", SqlDbType.VarChar, 500)).Value = CType(txtothers.Text.Trim, String)
                    '01122014
                    mySqlCmd.Parameters.Add(New SqlParameter("@branchname", SqlDbType.VarChar, 100)).Value = CType(txtbranch.Value.Trim, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@divid", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)


                mySqlCmd.ExecuteNonQuery()
            ElseIf ViewState("BankdetailsState") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                mySqlCmd = New SqlCommand("sp_delete_bankdetails_master", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@bankcode", SqlDbType.VarChar, 20)).Value = CType(TxtBankCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@divid", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    mySqlCmd.ExecuteNonQuery()
            End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CostCenterCodeSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('bankdetailsWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("bankdetails.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub TxtBankName_TextChanged(sender As Object, e As System.EventArgs) Handles TxtBankName.TextChanged
        '  TxtBankName.Attributes.Add("onchange", "filldisplay();")
        If TxtBankName.Text <> "" Then
            txtDispBankName.Value = TxtBankName.Text.Trim
            txtAcctName.Value = TxtBankName.Text.Trim
            ddlacctcurr.Value = objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "acctmast", "currcode", "acctcode", TxtBankCode.Text.Trim, "div_code", ViewState("divcode"))
        Else
            txtDispBankName.Value = ""
            txtAcctName.Value = ""
            ddlacctcurr.Value = "[Select]"
        End If

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=BankDetail','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
