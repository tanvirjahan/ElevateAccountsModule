'------------================--------------=======================------------------================
'   Module Name    :    CustomerCategories
'   Developer Name :    D'Silva Azia
'   Date           :   31 July 2008
'   
'---------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class OtherServicesPolicy
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                txtTransID.Disabled = True
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                ViewState.Add("OtherservpolicyState", Request.QueryString("State"))
                ViewState.Add("OtherservpolicyRefCode", Request.QueryString("RefCode"))

                If ViewState("OtherservpolicyState") = "New" Then
                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "Add New Other Services Policy"
                    Page.Title = Page.Title + " " + "New Other Services Policy"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("OtherservpolicyState") = "Edit" Then

                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "Edit Other Services Policy"
                    Page.Title = Page.Title + " " + "Edit Other Services Policy"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("OtherservpolicyRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")


                ElseIf ViewState("OtherservpolicyState") = "View" Then
                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "View Other Services Policy"
                    Page.Title = Page.Title + " " + "View Other Services Policy"
                    btnSave.Visible = False
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("OtherservpolicyRefCode"), String))

                ElseIf ViewState("OtherservpolicyState") = "Delete" Then
                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "Delete Other Services Policy"
                    Page.Title = Page.Title + " " + "Delete Other Services Policy"
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("OtherservpolicyRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Other Services Policy?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel')==false)return false;")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomerCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Try
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True, ddlGroupCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True, ddlGrpName.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True, ddlMarketCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True, ddlMarketName.Value)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomerCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

    Protected Sub DisableControl()
        ddlGroupCode.Disabled = True
        ddlGrpName.Disabled = True
        ddlMarketCode.Disabled = True
        ddlMarketName.Disabled = True
        txtCanActive.Disabled = True
        txtCanDeactive.Disabled = True
        txtRemarkAct.Disabled = True
        txtRemarkDeAct.Disabled = True
        txtChildActive.Disabled = True
        txtChildDeactive.Disabled = True
        chkActive.Disabled = True
    End Sub

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othserv_policy Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("tranid")) = False Then
                        Me.txtTransID.Value = CType(mySqlReader("tranid"), String)
                        'Else
                        '    Me.txtCode.Value = ""
                    End If

                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        Me.ddlGrpName.Value = CType(mySqlReader("othgrpcode"), String)
                        Me.ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", CType(mySqlReader("othgrpcode"), String))
                    Else
                        Me.ddlGroupCode.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        Me.ddlMarketName.Value = CType(mySqlReader("plgrpcode"), String)
                        Me.ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", CType(mySqlReader("plgrpcode"), String))
                    End If

                    If IsDBNull(mySqlReader("cancellation")) = False Then
                        Me.txtCanActive.Value = CType(mySqlReader("cancellation"), String)
                    Else
                        Me.txtCanActive.Value = ""
                    End If

                    If IsDBNull(mySqlReader("cancellationd")) = False Then
                        Me.txtCanDeactive.Value = CType(mySqlReader("cancellationd"), String)
                    Else
                        Me.txtCanDeactive.Value = ""
                    End If

                    If IsDBNull(mySqlReader("releaseperiod")) = False Then
                        Me.txtRemarkAct.Value = CType(mySqlReader("releaseperiod"), String)
                    Else
                        Me.txtRemarkAct.Value = ""
                    End If

                    If IsDBNull(mySqlReader("releaseperiodd")) = False Then
                        Me.txtRemarkDeAct.Value = CType(mySqlReader("releaseperiodd"), String)
                    Else
                        Me.txtRemarkDeAct.Value = ""
                    End If

                    If IsDBNull(mySqlReader("child")) = False Then
                        Me.txtChildActive.Value = CType(mySqlReader("child"), String)
                    Else
                        Me.txtChildActive.Value = ""
                    End If

                    If IsDBNull(mySqlReader("childd")) = False Then
                        Me.txtChildDeactive.Value = CType(mySqlReader("childd"), String)
                    Else
                        Me.txtChildDeactive.Value = ""
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
            '("SPOPOL")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Response.Redirect("OtherServicesPolicySearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then


                If ViewState("OtherservpolicyState") = "New" Or ViewState("OtherservpolicyState") = "Edit" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("OtherservpolicyState") = "New" Then
                        Dim optionval As String

                        optionval = objUtils.GetAutoDocNo("SPOPOL", mySqlConn, sqlTrans)
                        txtTransID.Value = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_othserv_policy ", mySqlConn, sqlTrans)

                    ElseIf ViewState("OtherservpolicyState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_othserv_policy", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value, String)

                    If ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String)
                    End If

                    If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@cancellation", SqlDbType.Text)).Value = CType(txtCanActive.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@releaseperiod", SqlDbType.Text)).Value = CType(txtRemarkAct.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@child", SqlDbType.Text)).Value = CType(txtChildActive.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@cancellationd", SqlDbType.Text)).Value = CType(txtCanDeactive.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@releaseperiodd", SqlDbType.Text)).Value = CType(txtRemarkDeAct.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@childd", SqlDbType.Text)).Value = CType(txtChildDeactive.Value.Trim, String)

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("OtherservpolicyState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_othserv_policy", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("OtherServicesPolicySearch.aspx", False)

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OtherServicesPolicyWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Dim OthpoResult As String
        OthpoResult = ""
        If ViewState("OtherservpolicyState") = "New" Then
            strSqlQry = "select 't' from othserv_policy where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "'" _
            & " and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "'"
            OthpoResult = Me.objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
            If OthpoResult = "t" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group and Market  is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("OtherservpolicyState") = "Edit" Then
            strSqlQry = "select 't' from othserv_policy where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "'" _
           & " and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "' and tranid <>'" & txtTransID.Value & "' "
            OthpoResult = Me.objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
            If OthpoResult = "t" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group and Market  is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesPolicy','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class

'Protected Sub btnCancel_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
'    Response.Redirect("OtherServicesPolicySearch.aspx", False)
'End Sub

'mySqlCmd = New SqlCommand("sp_del_othserv_policy", mySqlConn, sqlTrans)
'mySqlCmd.CommandType = CommandType.StoredProcedure
'mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value.Trim, String)
'mySqlCmd.ExecuteNonQuery()


'   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update city?')==false)return false;")