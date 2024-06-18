#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class PriceListModule_HandlingFeesSellingFormulae
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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                txtconnection.Value = Session("dbconnectionName")

                Dim strqry As String = ""
                Dim strOption As String = "HFEES"
                Dim strtitle As String = "Handling Fee Selling Formula"

                    
                strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode='HFEES'"

                objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlOperator, "operatorsymbol", "select operatorsymbol from operatormast", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpCode, "othgrpcode", "othgrpname", strqry, True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", strqry, True)
                ddlGrpCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0
                ddlGrpCode.Disabled = True
                ddlGrpName.Disabled = True
              

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellCode, "othsellcode", "othsellname", "select othsellcode,othsellname from othsellmast where active=1 order by othsellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCodeName, "othsellname", "othsellcode", "select othsellname,othsellcode from othsellmast where active=1 order by othsellname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCalculateFrm, "othsellcode", "othsellname", "select othsellcode,othsellname from othsellmast where active=1 order by othsellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCalculateName, "othsellname", "othsellcode", "select othsellname,othsellcode from othsellmast where active=1 order by othsellname", True)

                '  objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCalculateFrm, "sellcode", "sellname", "select sellcode,sellname from sellmast where active=1 order by sellcode", True)
                '  objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCalculateName, "sellname", "sellcode", "select sellname,sellcode from sellmast where active=1 order by sellname", True)

                ddlCodeName.Disabled = False
                ddlFormulafrom.SelectedIndex = 2


                'ViewState.Add("TrfSellingFormulaState", Request.QueryString("State"))
                'ViewState.Add("TrfSellingFormulaeRefCode", Request.QueryString("RefCode"))
                ViewState.Add("OthSellingFormulaState", Request.QueryString("State"))
                ViewState.Add("OthSellingFormulaeRefCode", Request.QueryString("RefCode"))
                Dim strGrpCode As String = Request.Params("GrpCode")
                If ViewState("OthSellingFormulaState") = "New" Then
                    SetFocus(ddlSellCode)
                    lblHeading.Text = "Add " + strtitle
                    Page.Title = Page.Title + " " + "New " + strtitle
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("OthSellingFormulaState") = "Edit" Then

                    'SetFocus(txtCodeName)
                    lblHeading.Text = "Edit " + strtitle
                    Page.Title = Page.Title + " " + "Edit " + strtitle
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("OthSellingFormulaeRefCode"), String), strGrpCode)
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    '   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update  Selling Formula?')==false)return false;")

                ElseIf ViewState("OthSellingFormulaState") = "View" Then
                    ' SetFocus(txtCode)
                    lblHeading.Text = "View " + strtitle
                    Page.Title = Page.Title + " " + "View " + strtitle
                    btnSave.Visible = False
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("OthSellingFormulaeRefCode"), String), strGrpCode)

                ElseIf ViewState("OthSellingFormulaState") = "Delete" Then
                    'SetFocus(txtCode)
                    lblHeading.Text = "Delete " + strtitle
                    Page.Title = Page.Title + " " + "Delete " + strtitle
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("OthSellingFormulaeRefCode"), String), strGrpCode)
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete  Selling Formula?')==false)return false;")
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
                objUtils.WritErrorLog("handlingFeesSellingFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub




#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String, ByVal othGrpCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from oth_sellformulas Where sellcode='" & RefCode & "' and othgrpcode='HFEES'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then

                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("sellcode")) = False Then
                        ddlCodeName.Value = CType(mySqlReader("sellcode"), String)
                        ddlSellCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "othsellname", "othsellcode", CType(mySqlReader("sellcode"), String))

                        txtCurrencyCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "currcode", "othsellcode", CType(mySqlReader("sellcode"), String))
                        txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", CType(txtCurrencyCode.Value, String))

                    End If

                    If IsDBNull(mySqlReader("calcfrom")) = False Then
                        ddlCalculateName.Value = CType(mySqlReader("calcfrom"), String)
                        ddlCalculateFrm.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othsellmast", "othsellname", "othsellcode", CType(mySqlReader("calcfrom"), String))
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
                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        ddlGrpName.Value = CType(mySqlReader("othgrpcode"), String)
                        ddlGrpCode.Value = ddlGrpName.Items(ddlGrpName.SelectedIndex).Text

                        'Else
                        '    ddlGrpName.Value = 0
                    End If

                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthSellingFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region


    Private Sub TitleLoad(ByVal prm_strQueryString As String)
        Dim strOption As String = "HFEES"
        Page.Title += "Handling Fees Selling Formula"
        lblHeading.Text = "Handling Fees Selling Formula"



   

    End Sub

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("OthSellingFormulaState") = "View" Or ViewState("OthSellingFormulaState") = "Delete" Then
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
            ddlGrpCode.Disabled = True
            ddlGrpName.Disabled = True

        ElseIf ViewState("OthSellingFormulaState") = "Edit" Then
            ddlSellCode.Disabled = True
            ddlCodeName.Disabled = True
            ddlGrpCode.Disabled = True
            ddlGrpName.Disabled = True
        End If
    End Sub
#End Region

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            If Page.IsValid = True Then
                If ViewState("OthSellingFormulaState") = "New" Or ViewState("OthSellingFormulaState") = "Edit" Then

                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("OthSellingFormulaState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_oth_sellformulas", mySqlConn, sqlTrans)
                    ElseIf ViewState("OthSellingFormulaState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_oth_sellformulas", mySqlConn, sqlTrans)
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
                    If ddlGrpCode.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text.Trim, String)

                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = ""
                    End If

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("OthSellingFormulaState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_oth_sellformulas", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sellcode", SqlDbType.VarChar, 20)).Value = CType(ddlCodeName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text.Trim, String)

                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("SellingPriceFormulasSearch.aspx", False)

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('HandlingFeesSellingFormulaeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthSellingFormula.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("OthSellingFormulaState") = "New" Then
            Dim strFilter As String = " othgrpcode='" + ddlGrpCode.Items(ddlGrpCode.SelectedIndex).Text.Trim + "'"
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "oth_sellformulas", "sellcode", CType(ddlSellCode.Items(ddlSellCode.SelectedIndex).Text.Trim, String), strFilter) Then
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthSellingFormula','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
