
'------------================--------------=======================------------------================
'   Page Name       :   FrmMarkets.aspx


'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class FrmMarkets
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getdivisionslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Divisionsnames As New List(Of String)
        Try
            strSqlQry = "select division_master_code,division_master_des from division_master where  division_master_des like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Divisionsnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("division_master_des").ToString(), myDS.Tables(0).Rows(i)("division_master_code").ToString()))

                Next
            End If
            Return Divisionsnames
        Catch ex As Exception
            Return Divisionsnames
        End Try

    End Function
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                ViewState.Add("MarketsState", Request.QueryString("State"))
                ViewState.Add("MarketsRefCode", Request.QueryString("RefCode"))

                ViewState.Add("MarValue", Request.QueryString("Value"))

                ' objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlHOD, "UserName", "UserCode", "select UserCode ,UserName  from UserMaster where active =1", True)
                Dim divisionList As List(Of String) = Getdivisionslist("")
                Dim splittedDivision() As String = divisionList(0).Split(",")
                If (divisionList.Count > 0) Then
                    If (splittedDivision.Length > 0 And splittedDivision.Length > 1) Then
                        TxtDivisionName.Text = splittedDivision(0).Replace("{", "").Replace(ControlChars.Quote, "").Replace("First:", "")
                        TxtDivisionCode.Text = splittedDivision(1).Replace("}", "").Replace(ControlChars.Quote, "").Replace("Second:", "")
                    End If
                End If
                If ViewState("MarketsState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Market"
                    Page.Title = Page.Title + " " + "New Market Master"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("MarketsState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Market"
                    Page.Title = Page.Title + " " + "Edit Market Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("MarketsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("MarketsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Market"
                    Page.Title = Page.Title + " " + "View Market Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("MarketsRefCode"), String))

                ElseIf ViewState("MarketsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Market"
                    Page.Title = Page.Title + " " + "Delete Market Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("MarketsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")

                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("FrmMarkets.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        Page.Title = "Market Entry"
    End Sub

#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("MarketsState") = "View" Or ViewState("MarketsState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True
            'chkShowInweb.Disabled = True
            'ddlHOD.Enabled = False
        ElseIf ViewState("MarketsState") = "Edit" Then
            txtCode.Disabled = True
        End If
    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from marketmast Where marketcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("marketcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("marketcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("marketname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("marketname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                    'If IsDBNull(mySqlReader("showinweb")) = False Then
                    '    If CType(mySqlReader("showinweb"), String) = "1" Then
                    '        chkShowInweb.Checked = True
                    '    ElseIf CType(mySqlReader("showinweb"), String) = "0" Then
                    '        chkShowInweb.Checked = False
                    '    End If
                    'End If
                    'If IsDBNull(mySqlReader("HOD")) = False Then
                    '    ddlHOD.SelectedValue = mySqlReader("HOD")

                    'Else
                    '    ddlHOD.SelectedValue = "[Select]"
                    'End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Markets.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")

        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ClientIP & " ');", True)
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Try
            If Page.IsValid = True Then
                If ViewState("MarketsState") = "New" Or ViewState("MarketsState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    'SQL  Trans start
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    If ViewState("MarketsState") = "New" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("MARKET", mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim
                    End If

                    If ViewState("MarketsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_marketmast", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("MarketsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_marketmast", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@marketcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@marketname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@div_Code", SqlDbType.VarChar)).Value = CType(TxtDivisionCode.Text.Trim, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    'If chkShowInweb.Checked = True Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@showinweb", SqlDbType.Int)).Value = 1
                    'ElseIf chkShowInweb.Checked = False Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@showinweb", SqlDbType.Int)).Value = 0
                    'End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    '@hod
                    'CType(ddlHOD.SelectedValue, String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("MarketsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction
                    frmmode = 3 'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_marketmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@marketcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""



                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("MarketsSearch.aspx", False)


                If ViewState("MarValue") = "Addfrom" Then
                    Session.Add("RegionCode", txtCode.Value)
                    Session.Add("RegionName", txtName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('MarketWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If
                If ViewState("MarketsState") = "New" Or ViewState("MarketsState") = "View" Or ViewState("MarketsState") = "Edit" Or ViewState("MarketsState") = "Delete" Then
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('MktWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If





        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()


            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Markets.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("MarketsSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region


#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("MarketsState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "plgrpmast", "plgrpcode", txtCode.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "plgrpmast", "plgrpname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("MarketsState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "plgrpmast", "plgrpcode", "plgrpname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region



#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agent_sectormaster", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Customer Sectors, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Customers, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "blocksale_header", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a BlockFullSales, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compulsory_header", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Compulsory Remarks, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compare_ratesh", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a CompareRates, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplistdnew", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a DetailsOfPriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a PriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "ctrymast", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Country, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromotion_header", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This currency is already used for a EarlyBirdPromotion, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "minnights_header", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a MinimumNights, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costd", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Details OF OtherServiceCostPriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costh", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a  OtherServiceCostPriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplisth", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a  OtherServicePriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplistd", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a  Details of OtherServicePriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " othserv_policy", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a  OtherServicePolicy, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyallot", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Suppliers, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "package_header", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Package, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_header", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Promotions, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " recalculate_details", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Details Of RecalCulate, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " recalculate_header", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Recalcualte, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellmast", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a SellingPriceTypes, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellsph", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a SellingPriceFormulaforSuppliers, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a GeneralPolicy, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplistd", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a SpeicalEventsorExtras PriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a SpeicalEventsorExtras PriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellmast", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a SellingPriceTypes, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "subcanmarket_detail", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a CancellationPolicies, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplistdnew", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a Details Of TicketPriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplistdwknew", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a CancellationPolicies, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplisthnew", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a TicketPriceList, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktsellmast", "plgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This region is already used for a TicketSellingTypes, cannot delete this region');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Markets','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class

