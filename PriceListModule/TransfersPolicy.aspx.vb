#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_TransfersPolicy
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As SqlDataAdapter
    Dim gvRow1 As GridViewRow
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                txtTransID.Disabled = True
                ddlGroupCode.Disabled = True
                ddlGrpName.Disabled = True

                Dim strqry As String = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strqry, True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", strqry, True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                ddlGroupCode.SelectedIndex = 0
                ddlGrpName.SelectedIndex = 0

                FillGridMarket("plgrpcode")
                Session("TrfPolicySave") = False
                ViewState.Add("TrfpolicyState", Request.QueryString("State"))
                ViewState.Add("TrfpolicyRefCode", Request.QueryString("RefCode"))

                If ViewState("TrfpolicyState") = "New" Then
                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "Add New Transfers Policy"
                    Page.Title = Page.Title + " " + "New Transfers Policy"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("TrfpolicyState") = "Edit" Then

                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "Edit Transfers Policy"
                    Page.Title = Page.Title + " " + "Edit Transfers Policy"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("TrfpolicyRefCode"), String))
                    ShowMarkets(CType(ViewState("TrfpolicyRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")


                ElseIf ViewState("TrfpolicyState") = "View" Then
                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "View Transfers Policy"
                    Page.Title = Page.Title + " " + "View Transfers Policy"
                    btnSave.Visible = False
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("TrfpolicyRefCode"), String))
                    ShowMarkets(CType(ViewState("TrfpolicyRefCode"), String))
                ElseIf ViewState("TrfpolicyState") = "Delete" Then
                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "Delete Transfers Policy"
                    Page.Title = Page.Title + " " + "Delete Transfers Policy"
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("TrfpolicyRefCode"), String))
                    ShowMarkets(CType(ViewState("TrfpolicyRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Transfers Policy?')==false)return false;")
                ElseIf ViewState("TrfpolicyState") = "copy" Then

                    SetFocus(ddlGroupCode)
                    lblCustCatHead.Text = "Copy Transfers Policy"
                    Page.Title = Page.Title + " " + "Copy Transfers Policy"
                    btnSave.Text = "Save"
                    ShowRecord(CType(ViewState("TrfpolicyRefCode"), String))
                    ShowMarkets(CType(ViewState("TrfpolicyRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel')==false)return false;")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TransfersPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Try
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True, ddlGroupCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGrpName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True, ddlGrpName.Value)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True, ddlMarketCode.Value)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True, ddlMarketName.Value)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TransfersPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

    Protected Sub DisableControl()
        ddlGroupCode.Disabled = True
        ddlGrpName.Disabled = True
        'ddlMarketCode.Disabled = True
        'ddlMarketName.Disabled = True
        txtCanActive.Disabled = True
        txtCanDeactive.Disabled = True
        txtRemarkAct.Disabled = True
        txtRemarkDeAct.Disabled = True
        txtChildActive.Disabled = True
        txtChildDeactive.Disabled = True
        chkActive.Disabled = True
        gv_Market.Enabled = False
        btnSelectAll.Enabled = False
        btnUnselectAll.Enabled = False
    End Sub

#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        gv_Market.Visible = True
        If gv_Market.PageIndex < 0 Then
            gv_Market.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_Market.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Market.DataBind()
                txtrowcnt.Value = gv_Market.Rows.Count
            Else
                gv_Market.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TransfersPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
    End Sub
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othserv_policy Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("tranid")) = False And ViewState("TrfpolicyState") <> "copy" Then
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

                    'If IsDBNull(mySqlReader("plgrpcode")) = False Then
                    '    Me.ddlMarketName.Value = CType(mySqlReader("plgrpcode"), String)
                    '    Me.ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", CType(mySqlReader("plgrpcode"), String))
                    'End If

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
            objUtils.WritErrorLog("TransfersPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '' Response.Redirect("OtherServicesPolicySearch.aspx", False)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('TransfersPolicyWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
    Private Function ValidateGrid() As Boolean
        Dim ds As DataSet
        Dim chksel As CheckBox
        Dim lblcode As Label
        Dim grdFlag As Boolean
        Dim cnt As Integer = 0
        grdFlag = False

        Try
            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                lblcode = gvRow1.FindControl("lblcode")
                If chksel.Checked = True Then
                    grdFlag = True
                    cnt = cnt + 1
                End If
            Next

            If grdFlag = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
                ValidateGrid = False
                Exit Function
            End If

            ValidateGrid = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Session("TrfPolicySave") = False Then
                If Page.IsValid = True Then

                    If ViewState("TrfpolicyState") = "New" Or ViewState("TrfpolicyState") = "Edit" Or ViewState("TrfpolicyState") = "copy" Then
                        If ValidateGrid() = False Then
                            Exit Sub
                        End If

                        If checkForDuplicate() = False Then
                            Exit Sub
                        End If

                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                        If ViewState("TrfpolicyState") = "New" Or ViewState("TrfpolicyState") = "copy" Then
                            Dim optionval As String

                            optionval = objUtils.GetAutoDocNo("TRFPOL", mySqlConn, sqlTrans)
                            txtTransID.Value = optionval.Trim

                            mySqlCmd = New SqlCommand("sp_othserv_policy ", mySqlConn, sqlTrans)

                        ElseIf ViewState("TrfpolicyState") = "Edit" Then
                            mySqlCmd = New SqlCommand("sp_othserv_policy", mySqlConn, sqlTrans)
                        End If
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value, String)

                        If ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String)
                        End If

                        'If ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        'Else
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                        'End If

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
                        '---------------------------------------------------------------------------
                        mySqlCmd = New SqlCommand("sp_del_othserv_policy_market", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()

                        '----------------------------------- Inserting Data To market_detail Table
                        Dim chksel1 As CheckBox
                        Dim lblcode1 As Label

                        For Each Me.gvRow1 In gv_Market.Rows
                            chksel1 = gvRow1.FindControl("chkSelect")
                            lblcode1 = gvRow1.FindControl("lblcode")
                            If chksel1.Checked = True Then
                                mySqlCmd = New SqlCommand("sp_add_othserv_policy_markets", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(lblcode1.Text.Trim, String)
                                mySqlCmd.ExecuteNonQuery()
                            End If
                        Next


                    ElseIf ViewState("TrfpolicyState") = "Delete" Then

                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                        mySqlCmd = New SqlCommand("sp_del_othserv_policy_market", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()


                        mySqlCmd = New SqlCommand("sp_del_othserv_policy", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtTransID.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()
                    End If

                    sqlTrans.Commit()    'SQl Tarn Commit
                    Session("TrfPolicySave") = True
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                    'Response.Redirect("OtherServicesPolicySearch.aspx", False)

                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('TransfersPolicyWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TransfersPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region " Private Sub ShowMarkets(ByVal RefCode As String)"
    Private Sub ShowMarkets(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim chksel As CheckBox
            Dim lblcode As Label
            'Dim mktcode As String = ""
            'Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
            'Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othserv_policy_markets Where tranid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        For Each Me.gvRow1 In gv_Market.Rows
                            chksel = gvRow1.FindControl("chkSelect")
                            lblcode = gvRow1.FindControl("lblcode")
                            If lblcode.Text = mySqlReader("plgrpcode") Then
                                chksel.Checked = True
                                Exit For
                            End If

                        Next
                    End If
                End While
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Dim OthpoResult As String
        OthpoResult = ""
        Dim chksel As CheckBox
        Dim lblcode As Label
        If ViewState("TrfpolicyState") = "New" Or ViewState("TrfpolicyState") = "copy" Then
            'strSqlQry = "select 't' from othserv_policy where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "'" _
            '& " and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "'"
            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                lblcode = gvRow1.FindControl("lblcode")
                If chksel.Checked = True Then

                    'strSqlQry = "select plgrpcode from othserv_policy_markets where plgrpcode ='" & lblcode.Text & "'"
                    strSqlQry = " select othserv_policy_markets.plgrpcode from othserv_policy_markets inner join othserv_policy on othserv_policy .tranid =othserv_policy_markets .tranid " & _
                        "   where othserv_policy_markets.plgrpcode ='" & lblcode.Text & "' and othserv_policy.othgrpcode ='" & CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String) & "'"
                    If OthpoResult.Length <> 0 Then
                        OthpoResult = OthpoResult + "," + Me.objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
                    Else
                        OthpoResult = "" + Me.objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
                    End If

                End If
            Next
            If OthpoResult <> "" Then
                Dim strMsg As String = "alert('Policy already present for Market : " + OthpoResult + "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strMsg, True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("TrfpolicyState") = "Edit" Then

            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                lblcode = gvRow1.FindControl("lblcode")
                If chksel.Checked = True Then

                    'strSqlQry = "select plgrpcode from othserv_policy_markets where plgrpcode ='" & lblcode.Text & "'and tranid <>'" & txtTransID.Value & "' "

                    strSqlQry = " select othserv_policy_markets.plgrpcode from othserv_policy_markets inner join othserv_policy on othserv_policy .tranid =othserv_policy_markets .tranid " & _
                        "  where othserv_policy_markets.plgrpcode ='" & lblcode.Text & "' and othserv_policy_markets.tranid <>'" & txtTransID.Value & "' and othserv_policy.othgrpcode ='" & CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String) & "'"



                    If OthpoResult.Length > 0 Then
                        OthpoResult = OthpoResult + "," + Me.objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
                    Else
                        OthpoResult = "" + Me.objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
                    End If

                End If
            Next
            If OthpoResult <> "" Then
                Dim strMsg As String = "alert('Policy already present for Market : " + OthpoResult + "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strMsg, True)
                checkForDuplicate = False
                Exit Function
            End If

            ' strSqlQry = "select 't' from othserv_policy where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "'" _
            '& " and plgrpcode='" & ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text & "' and tranid <>'" & txtTransID.Value & "' "
            ' OthpoResult = Me.objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
            ' If OthpoResult = "t" Then
            '     ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group and Market  is already present.');", True)
            '     checkForDuplicate = False
            '     Exit Function
            ' End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=TransfersPolicy','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
        Dim chksel As CheckBox
        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub btnUnselectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectAll.Click
        Dim chksel As CheckBox
        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            chksel.Checked = False
        Next
    End Sub
End Class
