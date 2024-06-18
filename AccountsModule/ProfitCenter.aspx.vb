'------------================--------------=======================------------------================
'   Module Name    :    ProfitCenterSearch.aspx
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ProfitCenterMaster
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
                ViewState.Add("ProfitCenterState", Request.QueryString("State"))
                ViewState.Add("ProfitCenterRefCode", Request.QueryString("RefCode"))
                If ViewState("ProfitCenterState") = "New" Then
                    strSqlQry = "select ServiceCategory,SNo from servicecat where ServiceCategory not in(select ServiceCat from profitcentremast)  order by ServiceCategory"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlServicecode, "ServiceCategory", "ServiceCategory", strSqlQry, True)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlServicecode, "ServiceCategory", "ServiceCategory", "select ServiceCategory,SNo from servicecat order by ServiceCategory", True)
                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostcode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostname, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlIncomecode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlIncomename, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefCostcode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefCostname, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefIncomecode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefIncomename, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCompCode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCompName, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)


                If ViewState("ProfitCenterState") = "New" Then
                    SetFocus(ddlServicecode)
                    Page.Title = "Add New Profit Center"
                    lblHeading.Text = "Add New Profit Center"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("ProfitCenterState") = "Edit" Then
                    SetFocus(txtDisplayname)
                    Page.Title = "Edit Profit Center"
                    lblHeading.Text = "Edit Profit Center"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("ProfitCenterRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("ProfitCenterState") = "View" Then
                    SetFocus(btnCancel)
                    Page.Title = "View Profit Center"
                    lblHeading.Text = "View Profit Center"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("ProfitCenterRefCode"), String))
                ElseIf ViewState("ProfitCenterState") = "Delete" Then
                    SetFocus(btnSave)
                    Page.Title = "Delete Profit Center"
                    lblHeading.Text = "Delete Profit Center"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("ProfitCenterRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlServicecode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCostcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlIncomecode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlIncomename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlRefCostcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlRefCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlRefIncomecode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlRefIncomename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCompCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCompName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                charcters(txtDisplayname)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ProfitCenter.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("ProfitCenterState") = "View" Or ViewState("ProfitCenterState") = "Delete" Then
            txtDisplayname.Disabled = True
            chkActive.Disabled = True
            ddlCostcode.Disabled = True
            ddlCostname.Disabled = True
            ddlIncomecode.Disabled = True
            ddlIncomename.Disabled = True
            ddlServicecode.Disabled = True
            ddlRefCostcode.Disabled = True
            ddlRefCostname.Disabled = True
            ddlRefIncomecode.Disabled = True
            ddlRefIncomename.Disabled = True
            ddlCompCode.Disabled = True
            ddlCompName.Disabled = True
        ElseIf ViewState("ProfitCenterState") = "Edit" Then
            ddlServicecode.Disabled = True
        End If
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("ProfitCenterState") = "New" Or ViewState("ProfitCenterState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("ProfitCenterState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_profitcentremast", mySqlConn, sqlTrans)
                    ElseIf ViewState("ProfitCenterState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_profitcentremast", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@servicecat", SqlDbType.VarChar, 20)).Value = CType(ddlServicecode.Items(ddlServicecode.SelectedIndex).Text, String)
                    If txtDisplayname.Value.Trim = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@dispname", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@dispname", SqlDbType.VarChar, 100)).Value = CType(txtDisplayname.Value.Trim, String)
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = CType(ddlIncomecode.Items(ddlIncomecode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@costcode", SqlDbType.VarChar, 20)).Value = CType(ddlCostcode.Items(ddlCostcode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@refundincomecode", SqlDbType.VarChar, 20)).Value = CType(ddlRefIncomecode.Items(ddlRefIncomecode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@refundcostcode", SqlDbType.VarChar, 20)).Value = CType(ddlRefCostcode.Items(ddlRefCostcode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@complcode", SqlDbType.VarChar, 20)).Value = CType(ddlCompCode.Items(ddlCompCode.SelectedIndex).Text, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    If ViewState("ProfitCenterState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("ProfitCenterState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("ProfitCenterState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_profitcentremast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@servicecat", SqlDbType.VarChar, 20)).Value = CType(ddlServicecode.Value, String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("ProfitCenterSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ProfitCenterWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ProfitCenter.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from profitcentremast Where servicecat='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("servicecat")) = False Then
                        ddlServicecode.Value = RefCode
                    End If
                    If IsDBNull(mySqlReader("dispname")) = False Then
                        Me.txtDisplayname.Value = CType(mySqlReader("dispname"), String)
                    Else
                        Me.txtDisplayname.Value = ""
                    End If
                    If IsDBNull(mySqlReader("incomecode")) = False Then
                        ddlIncomename.Value = mySqlReader("incomecode")
                        ddlIncomecode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("incomecode"))
                    End If
                    If IsDBNull(mySqlReader("costcode")) = False Then
                        ddlCostname.Value = mySqlReader("costcode")
                        ddlCostcode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("costcode"))
                    End If
                    If IsDBNull(mySqlReader("refundincomecode")) = False Then
                        ddlRefIncomename.Value = mySqlReader("refundincomecode")
                        ddlRefIncomecode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("refundincomecode"))
                    End If
                    If IsDBNull(mySqlReader("refundcostcode")) = False Then
                        ddlRefCostname.Value = mySqlReader("refundcostcode")
                        ddlRefCostcode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("refundcostcode"))
                    End If
                    If IsDBNull(mySqlReader("complcode")) = False Then
                        ddlCompName.Value = mySqlReader("complcode")
                        ddlCompCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("complcode"))
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ProfitCenter.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("ProfitCenterSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Try
            If ViewState("ProfitCenterState") = "New" Then
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "profitcentremast", "servicecat", ddlServicecode.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Service Category  is already present.');", True)
                    SetFocus(ddlServicecode)
                    checkForDuplicate = False
                    Exit Function
                End If
            End If
            checkForDuplicate = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ProfitCenter.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ProfitCenter','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
