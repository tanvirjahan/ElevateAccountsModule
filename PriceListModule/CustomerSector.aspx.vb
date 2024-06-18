'------------================--------------=======================------------------================
'   Module Name    :    CustomerSector .aspx
'   Developer Name :    Amit Survase
'   Date           :    18 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class CustomerSector
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
          <System.Web.Services.WebMethod()> _
    Public Shared Function Getctrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Ctrynames As New List(Of String)
        Try
            strSqlQry = "select ctrycode,ctryname from ctrymast where active=1 and  ctryname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Ctrynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))

                Next
            End If
            Return Ctrynames
        Catch ex As Exception
            Return Ctrynames
        End Try

    End Function
    Protected Sub TxtCtryName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCtryName.TextChanged
        Session("ctry_for_filter") = TxtCtryCode.Text()
    End Sub
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                txtconnection.Value = Session("dbconnectionName")

          

                ViewState.Add("CustsectState", Request.QueryString("State"))
                ViewState.Add("CustsectRefCode", Request.QueryString("RefCode"))
                ViewState.Add("CustValue", Request.QueryString("Value"))


                If ViewState("CustsectState") = "New" Then
                    SetFocus(txtSectorCode)
                    lblHeading.Text = "Add New Customer Sector"
                    Page.Title = Page.Title + " " + "New Customer Sector"
                    btnSave.Text = "Save"

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save sector?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("CustsectState") = "Edit" Then
                    SetFocus(txtSectorName)
                    lblHeading.Text = "Edit Customer Sector"
                    Page.Title = Page.Title + " " + "Edit Customer Sector"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("CustsectRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update sector?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("CustsectState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Customer Sector"
                    Page.Title = Page.Title + " " + "View Customer Sector"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CustsectRefCode"), String))

                ElseIf ViewState("CustsectState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Customer Sector"
                    Page.Title = Page.Title + " " + "Delete Customer Sector"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("CustsectRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete sector?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomerSector.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
      
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("CustsectState") = "View" Or ViewState("CustsectState") = "Delete" Then
            txtSectorCode.Disabled = True
            txtSectorName.Disabled = True
            TxtCtryName.Enabled = False
            chkActive.Disabled = True
        ElseIf ViewState("CustsectState") = "Edit" Then
            txtSectorCode.Disabled = True
        End If
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If txtSectorCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code cannot be blank.');", True)
                SetFocus(txtSectorCode)
                ValidatePage = False
                Exit Function
            End If
            If txtSectorName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name cannot be blank.');", True)
                SetFocus(txtSectorName)
                ValidatePage = False
                Exit Function
            End If

            If TxtCtryCode.Text = "" Or TxtCtryName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country cannot be blank.');", True)
                SetFocus(txtSectorName)
                ValidatePage = False
                Exit Function
            End If
            ''If ddlCountryCode.Value = "[Select]" Then
            ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select country code.');", True)
            ''    SetFocus(ddlCountryCode)
            ''    ValidatePage = False
            ''    Exit Function
            ''End If
         
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function

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
                If ViewState("CustsectState") = "New" Or ViewState("CustsectState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    If ViewState("CustsectState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_agent_secMast", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("CustsectState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_agent_secmast", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorname", SqlDbType.VarChar, 150)).Value = CType(txtSectorName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(TxtCtryCode.Text.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("CustsectState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_agent_secmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                strPassQry = ""
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CustomerSectorSearch.aspx", False)

                If ViewState("CustValue") = "Addsectorfrom" Then
                    Session.Add("AgentSectorCode", txtSectorCode.Value)
                    Session.Add("AgentSectorName", txtSectorName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('sectorfromWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CustsectWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerSectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agent_sectormaster Where sectorcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        Me.txtSectorCode.Value = CType(mySqlReader("sectorcode"), String)
                    Else
                        Me.txtSectorCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("sectorname")) = False Then
                        Me.txtSectorName.Value = CType(mySqlReader("sectorname"), String)
                    Else
                        Me.txtSectorName.Value = ""
                    End If

                  
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        Me.TxtCtryName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        Me.TxtCtryCode.Text = CType(mySqlReader("ctrycode"), String)
                    Else
                        Me.TxtCtryName.Text = ""
                        Me.TxtCtryName.Text = ""
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
            objUtils.WritErrorLog("CustomerSectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CustomerSectorSearch.aspx", False)

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("CustsectState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agent_sectormaster", "sectorcode", CType(txtSectorCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This customer sector code is already present.');", True)
                SetFocus(txtSectorCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agent_sectormaster", "sectorname", txtSectorName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This customer sector name is already present.');", True)
                SetFocus(txtSectorName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("CustsectState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agent_sectormaster", "sectorcode", "sectorname", txtSectorName.Value.Trim, CType(txtSectorCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This customer sector name is already present.');", True)
                SetFocus(txtSectorName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

#Region "Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            ' strSqlQry = ""
            ' TxtCountryName.Value = ddlCountryCode.Value  'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCountryCode.SelectedValue)
        Catch ex As Exception
            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region



#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "sectorcode", CType(txtSectorCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This CustomerSector is already used for a Customers, cannot delete this CustomerSector');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustomerSector','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
