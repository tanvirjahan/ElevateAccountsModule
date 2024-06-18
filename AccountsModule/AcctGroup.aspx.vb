'------------------------------------------------------------------------------------------------
'   Module Name    :    AcctGroup.aspx
'   Developer Name :    Mangesh
'   Date           :    
'   
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class AcctGroup
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objdate As New clsDateTime
    Dim strQry As String

    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            ViewState.Add("AcctgrpState", Request.QueryString("State"))
            ViewState.Add("AcctgrpRefCode", Request.QueryString("RefCode"))
            ViewState.Add("AcctgrpParentId", Request.QueryString("ParentId"))
            ViewState.Add("AcctgrpChildId", Request.QueryString("ChildId"))
            ViewState.Add("AcctgrpAccLevel", Request.QueryString("AccLevel"))
            ViewState.Add("AcctgrpLevel", Request.QueryString("Level"))
            ViewState.Add("divcode", Request.QueryString("divid"))
            If Page.IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                'Numbers(txtAccCode)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMajor, "acctmajor", "acctorder", "select acctmajor,acctorder from acctmajor order by acctorder", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccMajobSub, "acctsubname", "acctsubcode", "select acctsubname,acctsubcode from acctmajorsub order by acctsubcode", True)

                If Request.QueryString("ord") <> "" Then
                    ddlMajor.Disabled = True
                    ddlMajor.Value = Request.QueryString("ord")

                Else
                    ddlMajor.Disabled = False
                End If
                If Request.QueryString("ord") = "5" Or Request.QueryString("ord") = "3" Or Request.QueryString("ord") = "6" Then
                    ddlAccMajobSub.Disabled = True
                End If
                If Request.QueryString("ChildId") = "1" Then
                    If Request.QueryString("Parentid") = "3" Then
                        ddlAccMajobSub.Disabled = True
                    End If
                End If
                If ddlMajor.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccMajobSub, "acctsubname", "acctsubcode", "select acctmajorsub.acctsubname as acctsubname,acctmajorsub.acctsubcode as acctsubcode from acctmajorsub inner join acctmajor on acctmajorsub.acctorder=acctmajor.acctorder and acctmajor.acctorder='" & ddlMajor.Value & "'  order by acctmajorsub.acctsubcode", True)
                Else
                    ddlAccMajobSub.Value = "[Select]"
                End If

                If ViewState("AcctgrpState") = "New" Then
                    SetFocus(txtAccCode)
                    Page.Title = "Add Account Group"
                    lblHeading.Text = "Add Account Group"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("AcctgrpState") = "Edit" Then
                    Page.Title = "Edit Account Group"
                    lblHeading.Text = "Edit Account Group"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("AcctgrpRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("AcctgrpState") = "Delete" Then
                    Page.Title = "Delete Account Group"
                    lblHeading.Text = "Delete Account Group"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("AcctgrpRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                End If
                DisableControls()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlMajor.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAccMajobSub.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            End If
            'If ddlMajor.Value <> "[Select]" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccMajobSub, "acctsubname", "acctsubcode", "select acctmajorsub.acctsubname as acctsubname,acctmajorsub.acctsubcode as acctsubcode from acctmajorsub inner join acctmajor on acctmajorsub.acctorder=acctmajor.acctorder and acctmajor.acctorder='" & ddlMajor.Value & "'  order by acctmajorsub.acctsubcode", True)
            'Else
            '    ddlAccMajobSub.Value = "[Select]"
            'End If
        Catch ex As Exception
            mySqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AcctGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load




    
    End Sub
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from acctgroup Where div_code='" & ViewState("divcode") & "' and acctcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.Read Then
                txtAccCode.Value = mySqlReader("acctcode")
                txtAccName.Value = mySqlReader("acctname")
                ddlMajor.Value = mySqlReader("acctorder")
                If IsDBNull(mySqlReader("acctbsorder")) = True Then
                    ddlAccMajobSub.Value = "[Select]"
                Else
                    ddlAccMajobSub.Value = CType(mySqlReader("acctbsorder"), String)
                End If

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccMajobSub, "acctsubname", "acctsubcode", "select acctmajorsub.acctsubname as acctsubname,acctmajorsub.acctsubcode as acctsubcode from acctmajorsub inner join acctmajor on acctmajorsub.acctorder=acctmajor.acctorder and acctmajor.acctorder='" & mySqlReader("acctorder") & "'  order by acctmajorsub.acctsubcode", True)
                'ddlAccMajobSub.Items(ddlAccMajobSub.SelectedIndex).Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctsubname from acctmajorsub where acctsubcode='" & CType(mySqlReader("acctbsorder"), Integer) & "' and acctorder='" & CType(mySqlReader("acctorder"), Integer) & "'"), String)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AcctGroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Public Function fnValidate() As Boolean"
    Public Function fnValidate() As Boolean
        If txtAccCode.Value.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter the account code.');", True)
            SetFocus(txtAccCode)
            fnValidate = False
            Exit Function
        End If
        If txtAccName.Value.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter the account name.');", True)
            SetFocus(txtAccName)
            fnValidate = False
            Exit Function
        End If
        If ddlMajor.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select major.');", True)
            SetFocus(ddlMajor)
            fnValidate = False
            Exit Function
        End If

        If ddlAccMajobSub.Disabled = False Then
            If ddlAccMajobSub.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Balance Sheet/ P & L Group.');", True)
                SetFocus(ddlAccMajobSub)
                fnValidate = False
                Exit Function
            End If
        End If


        If ViewState("AcctgrpState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "acctgroup", "acctcode", txtAccCode.Value.Trim) = 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account code is already present.');", True)
                SetFocus(txtAccCode)
                fnValidate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "acctgroup", "acctname", txtAccName.Value.Trim) = 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account name is already present.');", True)
                SetFocus(txtAccCode)
                fnValidate = False
                Exit Function
            End If
        ElseIf ViewState("AcctgrpState") = "Edit" Then
            'If objUtils.isDuplicateForModifynew(Session("dbconnectionName"),"acctgroup", "acctcode", "acctcode", txtAccCode.Value.Trim, CType(Session("RefCode"), String)) = 1 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account code is already present.');", True)
            '    SetFocus(txtAccCode)
            '    fnValidate = False
            'End If
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "acctgroup", "acctcode", "acctname", txtAccName.Value.Trim, CType(Session("RefCode"), String)) = 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account name is already present.');", True)
                SetFocus(txtAccCode)
                fnValidate = False
                Exit Function
            End If
        End If


        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")

        If txtAccCode.Value.Contains(strDelimiter) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account code  canot enter - " & strDelimiter & " ');", True)
            SetFocus(txtAccCode)
            fnValidate = False
            Exit Function
        End If

        If txtAccName.Value.Contains(strDelimiter) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account name  canot enter - " & strDelimiter & " ');", True)
            SetFocus(txtAccName)
            fnValidate = False
            Exit Function
        End If

        fnValidate = True

    End Function
#End Region

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If ViewState("AcctgrpState") = "New" Or ViewState("AcctgrpState") = "Edit" Then
                If fnValidate() = False Then
                    Exit Sub
                End If
                '// Add & Edit Record
                Dim intParentId As Integer = ViewState("AcctgrpParentId")
                Dim intChildId As Integer = ViewState("AcctgrpChildId")

                Dim intAcctType As Integer = 1
                Dim intAccLevel As Integer = ViewState("AcctgrpAccLevel")
                Dim intAcctOrder As Integer = ddlMajor.Value

                Dim strLevel As String = ViewState("AcctgrpLevel")
                Dim strSplLevel As String() = strLevel.Split("-")

                Dim intLevel1 As Long = CType(strSplLevel.GetValue(0), Long)
                Dim intLevel2 As Long = CType(strSplLevel.GetValue(1), Long)
                Dim intLevel3 As Long = CType(strSplLevel.GetValue(2), Long)
                Dim intLevel4 As Long = CType(strSplLevel.GetValue(3), Long)
                Dim intLevel5 As Long = CType(strSplLevel.GetValue(4), Long)
                Dim intLevel6 As Long = CType(strSplLevel.GetValue(5), Long)
                Dim intLevel7 As Long = CType(strSplLevel.GetValue(6), Long)



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                '//Save acctgroup
                If ViewState("AcctgrpState") = "New" Then
                    mySqlCmd = New SqlCommand("sp_add_acctgroup", mySqlConn, sqlTrans)
                    mySqlCmd.CommandText = "sp_add_acctgroup"
                ElseIf ViewState("AcctgrpState") = "Edit" Then
                    ' intParentId = CType(Session("RefCode"), String)
                    mySqlCmd = New SqlCommand("sp_mod_acctgroup", mySqlConn, sqlTrans)
                    mySqlCmd.CommandText = "sp_mod_acctgroup"
                End If

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@acctcode", SqlDbType.VarChar, 20)).Value = txtAccCode.Value.ToString.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@acctname", SqlDbType.VarChar, 100)).Value = txtAccName.Value.ToString.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@parentid", SqlDbType.Int)).Value = intParentId
                mySqlCmd.Parameters.Add(New SqlParameter("@childid", SqlDbType.Int)).Value = intChildId
                mySqlCmd.Parameters.Add(New SqlParameter("@accttype", SqlDbType.Int)).Value = intAcctType
                mySqlCmd.Parameters.Add(New SqlParameter("@acctlevel", SqlDbType.Int)).Value = intAccLevel
                mySqlCmd.Parameters.Add(New SqlParameter("@acctorder", SqlDbType.Int)).Value = intAcctOrder
                mySqlCmd.Parameters.Add(New SqlParameter("@control", SqlDbType.VarChar, 1)).Value = DBNull.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@custorsupp", SqlDbType.VarChar, 1)).Value = DBNull.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@bankyn", SqlDbType.VarChar, 1)).Value = DBNull.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@bank_master_type_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@lvlno", SqlDbType.Int)).Value = 0
                If ddlAccMajobSub.Value <> "[Select]" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@acctbsorder", SqlDbType.Int)).Value = CType(ddlAccMajobSub.Value.Trim, Integer)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@acctbsorder", SqlDbType.Int)).Value = DBNull.Value
                End If
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                mySqlCmd.ExecuteNonQuery()

                If ViewState("AcctgrpState") = "New" Then
                    mySqlCmd = New SqlCommand("sp_add_accrep", mySqlConn, sqlTrans)
                    mySqlCmd.CommandText = "sp_add_accrep"
                ElseIf ViewState("AcctgrpState") = "Edit" Then
                    mySqlCmd = New SqlCommand("sp_mod_accrep", mySqlConn, sqlTrans)
                    mySqlCmd.CommandText = "sp_mod_accrep"
                End If

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@level1", SqlDbType.Int)).Value = intLevel1
                mySqlCmd.Parameters.Add(New SqlParameter("@level2", SqlDbType.Int)).Value = intLevel2
                mySqlCmd.Parameters.Add(New SqlParameter("@level3", SqlDbType.Int)).Value = intLevel3
                mySqlCmd.Parameters.Add(New SqlParameter("@level4", SqlDbType.Int)).Value = intLevel4
                mySqlCmd.Parameters.Add(New SqlParameter("@level5", SqlDbType.Int)).Value = intLevel5
                mySqlCmd.Parameters.Add(New SqlParameter("@level6", SqlDbType.Int)).Value = intLevel6
                mySqlCmd.Parameters.Add(New SqlParameter("@level7", SqlDbType.Int)).Value = intLevel7
                mySqlCmd.Parameters.Add(New SqlParameter("@acccode", SqlDbType.VarChar, 20)).Value = txtAccCode.Value.ToString.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@accname", SqlDbType.VarChar, 100)).Value = txtAccName.Value.ToString.Trim
                mySqlCmd.Parameters.Add(New SqlParameter("@acctlevel", SqlDbType.Int)).Value = intAccLevel
                If ViewState("AcctgrpState") = "Edit" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                End If
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                mySqlCmd.ExecuteNonQuery()



            ElseIf ViewState("AcctgrpState") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                mySqlCmd = New SqlCommand("sp_del_acctgroup", mySqlConn, sqlTrans)
                mySqlCmd.CommandText = "sp_del_acctgroup"
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@parentid", SqlDbType.Int)).Value = CType(ViewState("AcctgrpParentId"), Long)
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                mySqlCmd.ExecuteNonQuery()
            End If
            sqlTrans.Commit()
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            'Response.Redirect("AcctGroupsSearch.aspx", False)
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('AcctgrpWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Response.Redirect("AcctGroupsSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#Region "Private Sub DisableControls()"
    Private Sub DisableControls()
        Dim ChildExt As String
        ChildExt = ""
        If ViewState("AcctgrpState") = "New" Then
        ElseIf ViewState("AcctgrpState") = "Edit" Then
            txtAccCode.Disabled = True

            ddlAccMajobSub.Disabled = False

            'param on 31/12/2020
            If Request.QueryString("ord") = "5" Or Request.QueryString("ord") = "3" Or Request.QueryString("ord") = "6" Then
                ddlAccMajobSub.Disabled = True
            End If
            If Request.QueryString("ChildId") = "1" Then
                If Request.QueryString("Parentid") = "3" Then
                    ddlAccMajobSub.Disabled = True
                End If
            End If

            'ChildExt = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select 't' from acctgroup where accttype=1 and  childid in (select parentid from acctgroup where accttype=1 and  acctcode='" & CType(Session("RefCode"), String) & "' )")
            'If ChildExt = "t" Then
            '    ddlMajor.Disabled = True
            'Else
            '    ddlMajor.Disabled = False
            'End If
        ElseIf ViewState("AcctgrpState") = "Delete" Then
            txtAccCode.Disabled = True
            txtAccName.Disabled = True
            ddlMajor.Disabled = True
            ddlAccMajobSub.Disabled = True
        End If
    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=AcctGroup','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class


