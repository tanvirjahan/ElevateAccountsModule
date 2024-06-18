'------------================--------------=======================------------------================
'   Module Name    :    SupAgentGen.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    9 July 2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupAgentGen
    Inherits System.Web.UI.Page
    Dim objuser As New clsUser

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
        PanelGeneral.Visible = True
        ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select distinct sptypecode from sptypemast where active=1 order by sptypecode", True)

        'If Not Session("CompanyName") Is Nothing Then
        '    Me.Page.Title = CType(Session("CompanyName"), String)
        'End If

        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplieragentsSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTName, "sptypename", "sptypecode", "select * from sptypemast where active=1 order by sptypename", True)
            If CType(Session("SupagentsState"), String) = "New" Then
                SetFocus(txtGeneral)
                lblHeading.Text = "Add New Supplier Agents" + " - " + PanelGeneral.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Agents Master" + " - " + PanelGeneral.GroupingText
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
            ElseIf CType(Session("SupagentsState"), String) = "Edit" Then
                BtnGeneralSave.Text = "Update"
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                ddlType.Disabled = True
                ddlTName.Disabled = True
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                SetFocus(txtGeneral)
                lblHeading.Text = "Edit Supplier Agents" + " - " + PanelGeneral.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Agents Master" + " - " + PanelGeneral.GroupingText
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")

            ElseIf CType(Session("SupagentsState"), String) = "View" Then
                ddlType.Disabled = True
                ddlTName.Disabled = True
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                lblHeading.Text = "View Supplier Agents" + " - " + PanelGeneral.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Agents Master" + " - " + PanelGeneral.GroupingText
                BtnGeneralSave.Visible = False
                BtnGeneralCancel.Text = "Return to Search"

            ElseIf CType(Session("SupagentsState"), String) = "Delete" Then
                ddlType.Disabled = True
                ddlTName.Disabled = True
                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True

                lblHeading.Text = "Delete Supplier Agents" + " - " + PanelGeneral.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Agents Master" + " - " + PanelGeneral.GroupingText
                BtnGeneralSave.Text = "Delete"
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
            End If
            BtnGeneralCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        End If

        Session.Add("submenuuser", "SupplierAgentsSearch.aspx")
    End Sub
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("supagentcode")) = False Then
                        Me.txtSuppCode.Value = mySqlReader("supagentcode")
                    End If
                    If IsDBNull(mySqlReader("supagentname")) = False Then
                        Me.txtSuppName.Value = mySqlReader("supagentname")
                    End If
                    If IsDBNull(mySqlReader("sptypecode")) = False Then
                        ddlTName.Value = mySqlReader("sptypecode")
                        ddlType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))
                    End If

                    '---------  General Details ------------------------------------
                    If IsDBNull(mySqlReader("general")) = False Then
                        txtGeneral.Text = mySqlReader("general")
                    Else
                        txtGeneral.Text = ""
                    End If
                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region


#Region "Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("SupagentsState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("SupagentsState") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("SupagentsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updategen_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("SupagentsState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updategen_supagents", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlType.Items(ddlType.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@general", SqlDbType.Text)).Value = CType(txtGeneral.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("SupagentsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from supagents_multiemail where supagentcode='" & txtSuppCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If Session("SupagentsState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("SupagentsState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("SupagentsState") = "Delete" Then
                    'Response.Redirect("SupplierAgentsSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SupagentsWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("SupplierAgentsSearch.aspx")
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SupagentsWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "blocksale_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a BlockFullOfSales, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cancel_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a CancellationPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "child_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a ChildPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compulsory_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a CompulsoryRemarks, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a PriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromotion_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a EarliBirdPromotion, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "minnights_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a MinimumNights, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costh", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a OtherServiceCostPriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a Promotions, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellsph", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a SellingFormulaForSupplier, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a GeneralPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a  SpecialEvents/Extras, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function



        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplisthnew", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a  TicketingpriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function


        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SuppAgentGen','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
