#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class PriceListModule_TrfSellingFormulas
    Inherits System.Web.UI.Page


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "TextLockHtml"
    Public Sub TextLockHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                txtconnection.Value = Session("dbconnectionName")


                objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlOperator, "operatorsymbol", "select operatorsymbol from operatormast", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellCode, "sellcode", "sellname", "select sellcode,sellname from sellmast where active=1 order by sellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCodeName, "sellname", "sellcode", "select sellname,sellcode from sellmast where active=1 order by sellname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCalculateFrm, "sellcode", "sellname", "select sellcode,sellname from sellmast where active=1 order by sellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCalculateName, "sellname", "sellcode", "select sellname,sellcode from sellmast where active=1 order by sellname", True)

                ddlCodeName.Disabled = False
                ddlFormulafrom.SelectedIndex = 2


                ViewState.Add("TrfSellingFormulaState", Request.QueryString("State"))
                ViewState.Add("TrfSellingFormulaeRefCode", Request.QueryString("RefCode"))


                If ViewState("TrfSellingFormulaState") = "New" Then
                    SetFocus(ddlSellCode)
                    lblHeading.Text = "Add Transfer Selling Formula"
                    Page.Title = Page.Title + " " + "New Transfer Selling Formula Master"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("TrfSellingFormulaState") = "Edit" Then

                    'SetFocus(txtCodeName)
                    lblHeading.Text = "Edit Transfer Selling Formula"
                    Page.Title = Page.Title + " " + "Edit Selling Price Formula Master"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("TrfSellingFormulaeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    '   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update Transfer Selling Formula?')==false)return false;")

                ElseIf ViewState("TrfSellingFormulaState") = "View" Then
                    ' SetFocus(txtCode)
                    lblHeading.Text = "View Transfer Selling Formula"
                    Page.Title = Page.Title + " " + "View Transfer Selling Formula Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("TrfSellingFormulaeRefCode"), String))

                ElseIf ViewState("TrfSellingFormulaState") = "Delete" Then
                    'SetFocus(txtCode)
                    lblHeading.Text = "Delete Transfer Selling Formula"
                    Page.Title = Page.Title + " " + "Delete Transfer Selling Formula Master"
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("TrfSellingFormulaeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Transfer Selling Formula?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel')==false)return false;")
                TextLockHtml(txtCurrencyCode)
                TextLockHtml(txtCurrencyName)
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSellCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCodeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlCalculateFrm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCalculateName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TrfSellingFormulas.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trf_sellformulas Where sellcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then

                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("sellcode")) = False Then
                        ddlCodeName.Value = CType(mySqlReader("sellcode"), String)
                        ddlSellCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellmast", "sellname", "sellcode", CType(mySqlReader("sellcode"), String))

                        txtCurrencyCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellmast", "currcode", "sellcode", CType(mySqlReader("sellcode"), String))
                        txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", CType(txtCurrencyCode.Value, String))

                    End If

                    If IsDBNull(mySqlReader("calcfrom")) = False Then
                        ddlCalculateName.Value = CType(mySqlReader("calcfrom"), String)
                        ddlCalculateFrm.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellmast", "sellname", "sellcode", CType(mySqlReader("calcfrom"), String))
                    End If


                    If IsDBNull(mySqlReader("fmlacurr")) = False Then
                        ddlFormulafrom.SelectedIndex = CType(mySqlReader("fmlacurr"), String)
                    End If
                    If IsDBNull(mySqlReader("operator")) = False Then
                        ddlOperator.SelectedValue = CType(mySqlReader("operator"), String)
                    End If
                    If IsDBNull(mySqlReader("sellstring")) = False Then
                        txtFormula.Value = CType(mySqlReader("sellstring"), String)
                    Else
                        txtFormula.Value = ""
                    End If
                    If IsDBNull(mySqlReader("value")) = False Then
                        txtFrmValue.Value = Format(CType(mySqlReader("value"), Decimal), "#,###.000")
                    Else
                        txtFrmValue.Value = ""
                    End If

                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfSellingFormulas.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region


#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("TrfSellingFormulaState") = "View" Or ViewState("TrfSellingFormulaState") = "Delete" Then
            ddlSellCode.Disabled = True
            ddlCodeName.Disabled = True

            txtCurrencyCode.Disabled = True
            txtCurrencyName.Disabled = True

            ddlCalculateFrm.Disabled = True
            ddlCalculateName.Disabled = True

            txtFormula.Disabled = True
            txtFrmValue.Disabled = True

            ddlOperator.Enabled = False
            ddlFormulafrom.Disabled = True
            btnaddtostring1.Enabled = False
            btnAddtostring2.Enabled = False
            btnClear.Enabled = False
            btnClear.Visible = False

        ElseIf ViewState("TrfSellingFormulaState") = "Edit" Then
            ddlSellCode.Disabled = True
            ddlCodeName.Disabled = True
        End If
    End Sub
#End Region

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            If Page.IsValid = True Then
                If ViewState("TrfSellingFormulaState") = "New" Or ViewState("TrfSellingFormulaState") = "Edit" Then

                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("TrfSellingFormulaState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_TrfSellFormulas", mySqlConn, sqlTrans)
                    ElseIf ViewState("TrfSellingFormulaState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_TrfSellFormulas", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@sellcode", SqlDbType.VarChar, 10)).Value = CType(ddlSellCode.Items(ddlSellCode.SelectedIndex).Text.Trim, String)
                    If ddlCalculateFrm.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@calcfrom", SqlDbType.VarChar, 10)).Value = CType(ddlCalculateFrm.Items(ddlCalculateFrm.SelectedIndex).Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@calcfrom", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@sellstring", SqlDbType.VarChar, 100)).Value = CType(txtFormula.Value.Trim, String)

                    If ddlFormulafrom.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@fmlacurr", SqlDbType.Int, 1)).Value = ddlFormulafrom.SelectedIndex
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@fmlacurr", SqlDbType.Int, 1)).Value = 2
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@operator", SqlDbType.VarChar, 1)).Value = CType(ddlOperator.SelectedValue.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@value", SqlDbType.Decimal, 12, 4)).Value = CType(Format(CType(Val(txtFrmValue.Value.Trim), Decimal), "#,###.000"), Decimal)


                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("TrfSellingFormulaState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("[sp_delTrfSellFormulas]", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sellcode", SqlDbType.VarChar, 20)).Value = CType(ddlCodeName.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("SellingPriceFormulasSearch.aspx", False)

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('TrfSellingPriceFormulaeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfSellingFormulas.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '  ddlCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", ddlCurrency.Value.Trim.ToString)
    End Sub

    ' pending
    Protected Sub ddlCalculateFrm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'txtClculatefrm.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellmast", "currname", "sellcode", ddlCurrency.value.Trim.ToString)
    End Sub

    Protected Sub btnaddtostring1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtFormula.Value = ddlOperator.SelectedValue.Trim
    End Sub

    Protected Sub btnAddtostring2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtFormula.Value = txtFormula.Value.Trim & "" & Format(CType(txtFrmValue.Value.Trim, Decimal), "#,###.000")
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtFormula.Value = ""
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("SellingPriceFormulasSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("TrfSellingFormulaState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "trf_sellformulas", "sellcode", CType(ddlSellCode.Items(ddlSellCode.SelectedIndex).Text.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code  is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            checkForDuplicate = True
        End If
        checkForDuplicate = True
    End Function
#End Region

    Protected Sub ddlSellCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '  ddlCodeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellmast", "sellname", "sellcode", ddlSellCode.Value.Trim.ToString)
    End Sub

#Region "Public Function checkForDeletion() As String"
    Public Function checkForDeletion() As String
        Dim StrQurey As String = ""
        'StrQurey = "select distinct cplisthnew.plgrpcode from cplisthnew ,sellmast  where cplisthnew.plgrpcode=sellmast.plgrpcode and sellmast.sellcode='" & ddlSellCode.Value & "'"
        'checkForDeletion = True
        'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrQurey) <> "" Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SellingPriceFormula  is already used , cannot delete this Formula');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If
        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=TrfSellingFormulas','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
