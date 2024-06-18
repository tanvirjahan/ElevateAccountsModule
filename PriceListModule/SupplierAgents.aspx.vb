'------------================--------------=======================------------------================
'   Module Name    :    SupplierAgents.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    19 June 2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class SupplierAgents
    Inherits System.Web.UI.Page

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
        Dim GVRow As GridViewRow
        Dim txt As HtmlInputText
        Dim RefCode As String
        If IsPostBack = False Then
            PanelMain.Visible = True
            PanelReservation.Visible = False
            PanelSales.Visible = False
            PanelAccounts.Visible = False
            PanelGeneral.Visible = False
            PanelEmail.Visible = False
            fillgrd(gv_Email, True)
            'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "sptypename", "select * from sptypemast where active=1 order by sptypecode", True)

            ''--------=======    For Main Details Fill DropDownList
            'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlCurrency, "currcode", "currname", "select * from currmast where active=1  order by currcode", True)
            'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", "select * from ctrymast where active=1  order by ctrycode", True)
            objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select * from sptypemast where active=1 order by sptypecode", True)

            '--------=======    For Main Details Fill DropDownList
            objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCurrency, "currcode", "select * from currmast where active=1  order by currcode", True)
            objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountry, "ctrycode", "select * from ctrymast where active=1  order by ctrycode", True)

            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                ' txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtContactNo")
                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next
            telepphone(txtResPhone1)
            telepphone(txtResPhone2)
            Numbers(txtResFax)
            telepphone(txtSaleTelephone1)
            telepphone(txtSaleTelephone2)
            Numbers(txtSaleFax)
            telepphone(txtAccTelephone1)
            telepphone(txtAccTelephone2)
            Numbers(txtAccFax)
            Numbers(TxtAccCreditDays)
            Numbers(txtAccCreditLimit)
            charcters(txtSuppCode)
            charcters(txtSuppName)
            If CType(Session("State"), String) = "New" Then
                SetFocus(txtSuppCode)
                lblHeading.Text = "Add New Supplier Agents"
                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")

            ElseIf CType(Session("State"), String) = "Edit" Then

                ' ----- Azia-----''
                btnSave.Text = "Update"
                BtnResSave.Text = "Update"
                BtnSaleSave.Text = "Update"
                BtnAccSave.Text = "Update"
                BtnGeneralSave.Text = "Update"
                BtnEmailSave.Text = "Update"
                '----- END -----''

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                SetFocus(txtSuppCode)
                lblHeading.Text = "Edit Supplier Agents"
                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")

            ElseIf CType(Session("State"), String) = "View" Then

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier Agents"
                btnSave.Visible = False
                BtnResSave.Visible = False
                BtnSaleSave.Visible = False
                BtnAccSave.Visible = False
                BtnGeneralSave.Visible = False
                BtnEmailSave.Visible = False
                btnCancel.Text = "Return to Search"
                BtnResCancel.Text = "Return to Search"
                BtnSaleCancel.Text = "Return to Search"
                BtnAccCancel.Text = "Return to Search"
                BtnGeneralCancel.Text = "Return to Search"
                BtnEmailCancel.Text = "Return to Search"
                btnCancel.Focus()

            ElseIf CType(Session("State"), String) = "Delete" Then

                RefCode = CType(Session("RefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                DisableControl()
                lblHeading.Text = "Delete Supplier Agents"
                btnSave.Text = "Delete"
                BtnResSave.Text = "Delete"
                BtnSaleSave.Text = "Delete"
                BtnAccSave.Text = "Delete"
                BtnGeneralSave.Text = "Delete"
                BtnEmailSave.Text = "Delete"
                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
                BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
                BtnSaleSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
                BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
                BtnGeneralSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
                BtnEmailSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
            End If


        Else
            Try
                If ddlTypeCode.SelectedValue <> "[Select]" Then


                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try

            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnResCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnSaleCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnAccCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnGeneralCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            BtnEmailCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        End If

    End Sub
#End Region

#Region "Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = True
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelEmail.Visible = False
        SetFocus(txtResAddress1)
        ' ShowRecord(Session("RefCode"))
        GetValuesForResvationDetails()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtResAddress1.ClientID + "');", True)
    End Sub
#End Region

#Region "Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = True
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelEmail.Visible = False
        SetFocus(ddlCategory)
        GetValuesForMainDetails()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCategory.ClientID + "');", True)

        'ShowRecord(Session("RefCode"))
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierAgentsSearch.aspx")
    End Sub
#End Region

#Region " Private Function ValidateMainDetails() As Boolean"
    Private Function ValidateMainDetails() As Boolean
        ' Dim lbl As Label
        'Dim str As String
        Try
            If txtSuppCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
                'ScriptManagerProxy1.FindControl("txtSuppCode").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtSuppCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtSuppName.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                ' ScriptManagerProxy1.FindControl("txtSuppName").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtSuppName.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlTypeCode.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select type code.');", True)
                'ScriptManagerProxy1.FindControl("ddlTypeCode").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlTypeCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlCategory.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select category.');", True)
                '    ScriptManagerProxy1.FindControl("ddlCategory").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCategory.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlSelling.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select selling category.');", True)
                'SetFocus(ddlSelling)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSelling.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlCurrency.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select currency.');", True)
                'SetFocus(ddlCurrency)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCurrency.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlCountry.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select country.');", True)
                'SetFocus(ddlCountry)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCountry.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlCity.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select city.');", True)
                'SetFocus(ddlCity)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlCity.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If ddlSector.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select sector.');", True)
                'SetFocus(ddlSector)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSector.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            ValidateMainDetails = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Or Session("State") = "Edit" Then
                    If ValidateMainDetails() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_supagents", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentname", SqlDbType.VarChar, 100)).Value = CType(txtSuppName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType((ddlCategory.SelectedItem.Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scatcode", SqlDbType.VarChar, 20)).Value = CType((ddlSelling.SelectedItem.Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType((ddlCountry.SelectedItem.Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType((ddlCity.SelectedItem.Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType((ddlCurrency.SelectedItem.Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType((ddlSector.SelectedItem.Text), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
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
                '  Session.Add("SessionFirstCheck", "Edit")
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                    Session.Add("State", "Edit")
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    Session.Add("State", "Edit")
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierAgentsSearch.aspx", False)
                End If


                ' Response.Redirect("SupplierAgents.aspx", False)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

#Region "Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = True
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelEmail.Visible = False
        SetFocus(txtSaleTelephone1)
        '  ShowRecord(Session("RefCode"))
        GetValuesForSalesDetails()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtSaleTelephone1.ClientID + "');", True)

    End Sub
#End Region

#Region "Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = True
        PanelGeneral.Visible = False
        PanelEmail.Visible = False
        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAccPostTo, "supagentcode", "select * from supplier_agents where active=1 and  supagentcode<>'" & txtSuppCode.Value.Trim & "' and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by supagentcode", True)
        SetFocus(txtAccTelephone1)
        '  ShowRecord(Session("RefCode"))
        GetValuesForAccountDetails()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtAccTelephone1.ClientID + "');", True)

    End Sub
#End Region

#Region "Protected Sub BtnGeneral_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneral_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = True
        PanelEmail.Visible = False
        SetFocus(txtGeneral)
        ' ShowRecord(Session("RefCode"))
        GetValuesForGeneralDetails()
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtGeneral.ClientID + "');", True)

    End Sub
#End Region

#Region "Protected Sub BtnEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelMain.Visible = False
        PanelReservation.Visible = False
        PanelSales.Visible = False
        PanelAccounts.Visible = False
        PanelGeneral.Visible = False
        PanelEmail.Visible = True
        SetFocus(BtnAdd)
        GetValuesForEmailDetails()
    End Sub
#End Region

#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("no", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region

#Region "Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLines()
    End Sub

#End Region

#Region "Private Sub AddLines()"
    Private Sub AddLines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Email.Rows.Count + 1
        Dim txt As HtmlInputText
        Dim name(count) As String
        Dim email(count) As String
        Dim contact(count) As String
        'Dim chk(count) As Boolean
        Dim n As Integer = 0
        Try
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                name(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtEmail")
                email(n) = CType(Trim(txt.Value), String)
                txt = GVRow.FindControl("txtContactNo")
                contact(n) = CType(Trim(txt.Value), String)
                n = n + 1
            Next
            fillgrd(gv_Email, False, gv_Email.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_Email.Rows
                If n = i Then
                    Exit For
                End If
                'txtPerson txtEmail txtContactNo
                txt = GVRow.FindControl("txtPerson")
                txt.Value = name(n)
                txt = GVRow.FindControl("txtEmail")
                txt.Value = email(n)
                txt = GVRow.FindControl("txtContactNo")
                txt.Value = contact(n)
                n = n + 1
            Next
            For Each GVRow In gv_Email.Rows
                txt = GVRow.FindControl("txtPerson")
                txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtEmail")
                ' txt.Attributes.Add("onkeypress", "return checkCharacter(event)")
                txt = GVRow.FindControl("txtContactNo")
                txt.Attributes.Add("onkeypress", "return checkNumber(event)")
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#End Region

#Region "Protected Sub ddlTypeCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub ddlTypeCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", ddlTypeCode.SelectedValue)
        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCategory, "catcode", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedValue & "' order by catcode", True)
        txtCategoryName.Value = ""
        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSelling, "scatcode", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedValue & "' order by scatcode", True)
        txtSellingName.Value = ""
        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAccPostTo, "supagentcode", "select * from supplier_agents where active=1 and  supagentcode<>'" & txtSuppCode.Value.Trim & "' and sptypecode='" & ddlTypeCode.SelectedValue & "' order by supagentcode", True)
        txtAccPostTo2.Value = ""
    End Sub
#End Region

#Region "Protected Sub ddlCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub ddlCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", ddlCity.SelectedItem.Text)
        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select * from sectormaster where active=1  and citycode='" & ddlCity.SelectedValue & "' order by sectorcode", True)
        txtSectorName.Value = ""
    End Sub
#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        BtnAdd.Visible = False
        Me.txtSuppCode.Disabled = True
        Me.txtSuppName.Disabled = True
        ddlTypeCode.Enabled = False
        txtTypeName.Disabled = True
        ddlCategory.Enabled = False
        txtCategoryName.Disabled = True
        ddlSelling.Enabled = False
        txtSellingName.Disabled = True
        ddlCurrency.Enabled = False
        txtCurrencyName.Disabled = True
        ddlCountry.Enabled = False
        txtCountryName.Disabled = True
        ddlCity.Enabled = False
        txtCityName.Disabled = True
        ddlSector.Enabled = False
        txtSectorName.Disabled = True
        chkActive.Disabled = True
        '------------------------------------------------------
        '-------------- Reservation Details --------------------------
        txtResAddress1.Disabled = True
        txtResAddress2.Disabled = True
        txtResAddress3.Disabled = True
        txtResPhone1.Disabled = True
        txtResPhone2.Disabled = True
        txtResFax.Disabled = True
        txtResContact1.Disabled = True
        txtResContact2.Disabled = True
        txtResEmail.Disabled = True
        ddlComunicate.Enabled = False
        ddlSell.Enabled = False
        txtResWebSite.Disabled = True
        '------------------------END-----------------------------------
        '---------  Sales Details ------------------------------------
        txtSaleTelephone1.Disabled = True
        txtSaleTelephone2.Disabled = True
        txtSaleFax.Disabled = True
        txtSaleContact1.Disabled = True
        txtSaleContact2.Disabled = True
        txtSaleEmail.Disabled = True
        '------------------------END-----------------------------------
        '---------  Account Details ------------------------------------

        txtAccTelephone1.Disabled = True
        txtAccTelephone2.Disabled = True
        txtAccFax.Disabled = True
        txtAccContact1.Disabled = True
        txtAccContact2.Disabled = True
        txtAccEmail.Disabled = True
        TxtAccCreditDays.Disabled = True
        txtAccCreditLimit.Disabled = True
        ddlAccPostTo.Enabled = True
        txtAccPostTo2.Disabled = True
        txtAccACCode.Disabled = True
        ChkCashSup.Disabled = True
        '------------------------END-----------------------------------
        '---------  General Details ------------------------------------
        txtGeneral.ReadOnly = True
        Dim GVRow As GridViewRow
        Dim txt As HtmlInputText
        For Each GVRow In gv_Email.Rows
            txt = GVRow.FindControl("txtPerson")
            txt.Disabled = True
            txt = GVRow.FindControl("txtEmail")
            txt.Disabled = True
            txt = GVRow.FindControl("txtContactNo")
            txt.Disabled = True
        Next
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
                        ddlTypeCode.SelectedItem.Text = mySqlReader("sptypecode")
                        txtTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", ddlTypeCode.SelectedItem.Text)


                    End If
                    'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlCategory, "catcode", "catname", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by catcode", True)
                    'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlSelling, "scatcode", "scatname", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by scatcode", True)
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCategory, "catcode", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by catcode", True)
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSelling, "scatcode", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by scatcode", True)

                    '---------- Main Details    -------------------------
                    If IsDBNull(mySqlReader("catcode")) = False Then

                        ddlCategory.SelectedItem.Text = mySqlReader("catcode")
                        txtCategoryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "catmast", "catname", "catcode", ddlCategory.SelectedItem.Text)
                    End If
                    If IsDBNull(mySqlReader("scatcode")) = False Then
                        ddlSelling.SelectedItem.Text = mySqlReader("scatcode")
                        txtSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellcatmast", "scatname", "scatcode", ddlSelling.SelectedItem.Text)
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        ddlCurrency.SelectedItem.Text = mySqlReader("currcode")
                        txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", ddlCurrency.SelectedValue)
                    End If
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        ddlCountry.SelectedItem.Text = mySqlReader("ctrycode")
                        txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", ddlCountry.SelectedValue)
                    End If
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCity, "citycode", "select * from citymast where active=1  and ctrycode='" & ddlCountry.SelectedItem.Text & "' order by citycode", True)
                    If IsDBNull(mySqlReader("citycode")) = False Then
                        ddlCity.SelectedItem.Text = mySqlReader("citycode")
                        txtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", ddlCity.SelectedItem.Text)
                    End If
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select * from sectormaster where active=1  and citycode='" & ddlCity.SelectedItem.Text & "' order by sectorcode", True)
                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        ddlSector.SelectedItem.Text = mySqlReader("sectorcode")
                        txtSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sectormaster", "sectorname", "sectorcode", ddlSector.SelectedItem.Text)
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If

                    '------------------------------------------------------
                    '-------------- Reservation Details --------------------------
                    If IsDBNull(mySqlReader("add1")) = False Then
                        txtResAddress1.Value = mySqlReader("add1")
                    Else
                        txtResAddress1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add2")) = False Then
                        txtResAddress2.Value = mySqlReader("add2")
                    Else
                        txtResAddress2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("add3")) = False Then
                        txtResAddress3.Value = mySqlReader("add3")
                    Else
                        txtResAddress3.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel1")) = False Then
                        txtResPhone1.Value = mySqlReader("tel1")
                    Else
                        txtResPhone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("tel2")) = False Then
                        txtResPhone2.Value = mySqlReader("tel2")
                    Else
                        txtResPhone2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("fax")) = False Then
                        txtResFax.Value = mySqlReader("fax")
                    Else
                        txtResFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact1")) = False Then
                        txtResContact1.Value = mySqlReader("contact1")
                    Else
                        txtResContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("contact2")) = False Then
                        txtResContact2.Value = mySqlReader("contact2")
                    Else
                        txtResContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("email")) = False Then
                        txtResEmail.Value = mySqlReader("email")
                    Else
                        txtResEmail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("commmode")) = False Then
                        If mySqlReader("commmode") = "1" Then
                            ddlComunicate.SelectedValue = "Email"
                        ElseIf mySqlReader("commmode") = "0" Then
                            ddlComunicate.SelectedValue = "Fax"
                        End If
                    End If
                    If IsDBNull(mySqlReader("selltype")) = False Then
                        If mySqlReader("selltype") = "1" Then
                            ddlSell.SelectedValue = "Beach"
                        ElseIf mySqlReader("selltype") = "0" Then
                            ddlSell.SelectedValue = "City"
                        End If
                    End If
                    If IsDBNull(mySqlReader("website")) = False Then
                        txtResWebSite.Value = mySqlReader("website")
                    Else
                        txtResWebSite.Value = ""
                    End If

                    '------------------------END-----------------------------------
                    '---------  Sales Details ------------------------------------

                    If IsDBNull(mySqlReader("stel1")) = False Then
                        txtSaleTelephone1.Value = mySqlReader("stel1")
                    Else
                        txtSaleTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("stel2")) = False Then
                        txtSaleTelephone2.Value = mySqlReader("stel2")
                    Else
                        txtSaleTelephone2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("sfax")) = False Then
                        txtSaleFax.Value = mySqlReader("sfax")
                    Else
                        txtSaleFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact1")) = False Then
                        txtSaleContact1.Value = mySqlReader("scontact1")
                    Else
                        txtSaleContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("scontact2")) = False Then
                        txtSaleContact2.Value = mySqlReader("scontact2")
                    Else
                        txtSaleContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("semail")) = False Then
                        txtSaleEmail.Value = mySqlReader("semail")
                    Else
                        txtSaleEmail.Value = ""
                    End If
                    '------------------------END-----------------------------------
                    '---------  Account Details ------------------------------------

                    If IsDBNull(mySqlReader("atel1")) = False Then
                        txtAccTelephone1.Value = mySqlReader("atel1")
                    Else
                        txtAccTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("atel2")) = False Then
                        txtAccTelephone2.Value = mySqlReader("atel2")
                    Else
                        txtAccTelephone2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("afax")) = False Then
                        txtAccFax.Value = mySqlReader("afax")
                    Else
                        txtAccFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact1")) = False Then
                        txtAccContact1.Value = mySqlReader("acontact1")
                    Else
                        txtAccContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact2")) = False Then
                        txtAccContact2.Value = mySqlReader("acontact2")
                    Else
                        txtAccContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("aemail")) = False Then
                        txtAccEmail.Value = mySqlReader("aemail")
                    Else
                        txtAccEmail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crdays")) = False Then
                        TxtAccCreditDays.Value = mySqlReader("crdays")
                    Else
                        TxtAccCreditDays.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crlimit")) = False Then
                        txtAccCreditLimit.Value = mySqlReader("crlimit")
                    Else
                        txtAccCreditLimit.Value = ""
                    End If

                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAccPostTo, "supagentcode", "select * from supplier_agents where active=1 and  supagentcode<>'" & txtSuppCode.Value.Trim & "' and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by supagentcode", True)
                    If IsDBNull(mySqlReader("agtcode")) = False Then
                        ddlAccPostTo.SelectedValue = mySqlReader("agtcode")
                        txtAccPostTo2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentname", "supagentcode", ddlAccPostTo.SelectedValue)
                    Else
                        ddlAccPostTo.SelectedItem.Text = "[Select]"
                        txtAccPostTo2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        txtAccACCode.Value = mySqlReader("controlacctcode")
                    Else
                        txtAccACCode.Value = ""
                    End If

                    If IsDBNull(mySqlReader("cashsupagent")) = False Then
                        If mySqlReader("cashsupagent") = 1 Then
                            ChkCashSup.Checked = True
                        Else
                            ChkCashSup.Checked = False
                        End If
                    End If
                    '------------------------END-----------------------------------
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
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txt As HtmlInputText
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from supagents_multiemail Where supagentcode='" & RefCode & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from supagents_multiemail Where supagentcode='" & RefCode & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In gv_Email.Rows
                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("contactperson")) = False Then
                            txt = GVRow.FindControl("txtPerson")
                            txt.Value = mySqlReader("contactperson")
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txt = GVRow.FindControl("txtEmail")
                            txt.Value = mySqlReader("email")
                        End If
                        If IsDBNull(mySqlReader("contactno")) = False Then
                            txt = GVRow.FindControl("txtContactNo")
                            txt.Value = mySqlReader("contactno")
                        End If
                    End If
                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region

#Region "Private Function ValidateResrvation() As Boolean"
    Private Function ValidateResrvation() As Boolean
        Try
            If txtResAddress1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Address1 field can not be blank.');", True)
                'SetFocus(txtResAddress1)

                ValidateResrvation = False
                Exit Function
            End If
            If txtResPhone1.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Telephone1 field can not be blank.');", True)
                SetFocus(txtResPhone1)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResFax.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Fax field can not be blank.');", True)
                SetFocus(txtResFax)
                ValidateResrvation = False
                Exit Function
            End If
            If txtResEmail.Value.Trim <> "" Then
                If EmailValidate(txtResEmail.Value.Trim, txtResEmail) = False Then
                    ValidateResrvation = False
                    SetFocus(txtResEmail)
                    Exit Function
                End If
            End If
            If Trim(txtResWebSite.Value) <> "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                'SetFocus(txtResWebSite)
                'ValidateResrvation = False
                'Exit Function
                ' Else
                Dim getstr As String
                Dim dotpos1 As String
                Dim dotpos2 As String
                getstr = Trim(txtResWebSite.Value)
                dotpos1 = getstr.LastIndexOf(".")
                dotpos2 = getstr.LastIndexOf(".")
                If dotpos1 < 1 Or dotpos2 < 2 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                    SetFocus(txtResWebSite)
                    ValidateResrvation = False
                    Exit Function
                Else
                    Dim laststr As String
                    Dim atposstr As String()
                    Dim getvaluestr As String
                    Dim tempstr As String
                    atposstr = getstr.Split(".")
                    If atposstr.Length < 3 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                        SetFocus(txtResWebSite)
                        ValidateResrvation = False
                        Exit Function
                    ElseIf atposstr.Length = 3 Then
                        getvaluestr = atposstr.GetValue(atposstr.Length() - 3)
                        tempstr = atposstr.GetValue(atposstr.Length() - 1)
                        If getvaluestr <> "www" Or IsNumeric(tempstr) = True Or tempstr.Length() < 3 Then 'Or tempstr.Length() > 2
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            SetFocus(txtResWebSite)
                            ValidateResrvation = False
                            Exit Function
                        End If
                    ElseIf atposstr.Length > 3 Then
                        getvaluestr = atposstr.GetValue(atposstr.Length() - 4)
                        tempstr = atposstr.GetValue(atposstr.Length() - 2)
                        laststr = atposstr.GetValue(atposstr.Length() - 1)
                        If laststr = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            SetFocus(txtResWebSite)
                            ValidateResrvation = False
                            Exit Function
                        ElseIf getvaluestr <> "www" Or IsNumeric(tempstr) = True Or IsNumeric(laststr) = True Or tempstr.Length > 2 Or laststr.Length < 2 Or laststr.Length > 2 Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                            SetFocus(txtResWebSite)
                            ValidateResrvation = False
                            Exit Function
                        End If
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid web site(www.xyz.com).');", True)
                        ValidateResrvation = False
                        Exit Function
                    End If
                End If
            End If
            ValidateResrvation = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region

#Region "Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnResSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    'objUtils.MessageBox("Please Save First Main Details.", Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    If ValidateResrvation() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_update_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_update_supagents", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 100)).Value = CType(txtResAddress1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = CType(txtResAddress2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = CType(txtResAddress3.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(txtResPhone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = CType(txtResPhone2.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(txtResFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = CType(txtResContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = CType(txtResContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtResEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@website", SqlDbType.VarChar, 200)).Value = CType(txtResWebSite.Value.Trim, String)


                    If ddlComunicate.SelectedValue = "Email" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlComunicate.SelectedValue = "Fax" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@commmode", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    If ddlSell.SelectedValue = "Beach" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = 1
                    ElseIf ddlSell.SelectedValue = "City" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = 0
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@selltype", SqlDbType.Int, 4)).Value = DBNull.Value
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
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
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierAgentsSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    If txtSaleEmail.Value.Trim <> "" Then
                        If EmailValidate(txtSaleEmail.Value.Trim, txtSaleEmail) = False Then
                            '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Fax field can not be blank.');", True)
                            Exit Sub
                        End If
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updatesales_supagents", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel1", SqlDbType.VarChar, 100)).Value = CType(txtSaleTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@stel2", SqlDbType.VarChar, 100)).Value = CType(txtSaleTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sfax", SqlDbType.VarChar, 50)).Value = CType(txtSaleFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact1", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scontact2", SqlDbType.VarChar, 100)).Value = CType(txtSaleContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@semail", SqlDbType.VarChar, 100)).Value = CType(txtSaleEmail.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
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
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierAgentsSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    '-----------    Validate Page   ---------------
                    If txtAccACCode.Value.Trim = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Control A/C Code field can not be blank.');", True)
                        SetFocus(txtAccACCode)
                        Exit Sub
                    End If
                    If txtAccEmail.Value.Trim <> "" Then
                        If EmailValidate(txtAccEmail.Value.Trim, txtAccEmail) = False Then
                            SetFocus(txtAccEmail)
                            Exit Sub
                        End If
                    End If
                    '---------------------------------------------------

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updateacc_supagents", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel1", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel2", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@afax", SqlDbType.VarChar, 50)).Value = CType(txtAccFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact1", SqlDbType.VarChar, 100)).Value = CType(txtAccContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact2", SqlDbType.VarChar, 100)).Value = CType(txtAccContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@aemail", SqlDbType.VarChar, 100)).Value = CType(txtAccEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crdays", SqlDbType.Int, 4)).Value = CType(Val(TxtAccCreditDays.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crlimit", SqlDbType.Int, 4)).Value = CType(Val(txtAccCreditLimit.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(txtAccACCode.Value.Trim, String)
                    If ChkCashSup.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashsupagent", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkCashSup.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashsupagent", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@agtcode", SqlDbType.VarChar, 20)).Value = CType(ddlAccPostTo.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("State") = "Delete" Then
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
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierAgentsSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneralSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updategen_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updategen_supagents", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlTypeCode.SelectedItem.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@general", SqlDbType.Text)).Value = CType(txtGeneral.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
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
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierAgentsSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Private Function ValidateEmail() As Boolean"
    Private Function ValidateEmail() As Boolean
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GVRow As GridViewRow
        Dim FLAG As Boolean = False
        Try
            For Each GVRow In gv_Email.Rows
                txtName = GVRow.FindControl("txtPerson")
                txtEmail = GVRow.FindControl("txtEmail")
                txtContact = GVRow.FindControl("txtContactNo")
                If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                    FLAG = True
                End If
            Next

            If FLAG = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter at least one email details.');", True)
                ValidateEmail = False
                Exit Function
            Else

                For Each GVRow In gv_Email.Rows
                    txtName = GVRow.FindControl("txtPerson")
                    txtEmail = GVRow.FindControl("txtEmail")
                    txtContact = GVRow.FindControl("txtContactNo")
                    If txtName.Value <> "" Or txtEmail.Value <> "" Or txtContact.Value <> "" Then
                        If txtName.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact Person field can not be blank.');", True)
                            SetFocus(txtName)
                            ValidateEmail = False
                            Exit Function
                        End If
                        If txtEmail.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email field can not be blank.');", True)
                            SetFocus(txtEmail)
                            ValidateEmail = False
                            Exit Function
                        Else
                            If EmailValidate(txtEmail.Value.Trim, txtEmail) = False Then
                                SetFocus(txtEmail)
                                ValidateEmail = False
                                Exit Function
                            End If
                        End If
                        If txtContact.Value = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact no field can not be blank.');", True)
                            SetFocus(txtContact)
                            ValidateEmail = False
                            Exit Function
                        End If

                    End If
                Next
            End If
            ValidateEmail = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

#Region " Protected Sub BtnEmailSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnEmailSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtName As HtmlInputText
        Dim txtEmail As HtmlInputText
        Dim txtContact As HtmlInputText
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State") = "Edit" Then

                    If ValidateEmail() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("delete from supagents_multiemail where supagentcode='" & txtSuppCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In gv_Email.Rows
                        txtName = GvRow.FindControl("txtPerson")
                        txtEmail = GvRow.FindControl("txtEmail")
                        txtContact = GvRow.FindControl("txtContactNo")
                        If CType(txtName.Value, String) <> "" And CType(txtEmail.Value, String) <> "" And CType(txtContact.Value, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_supagents_multiemail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactperson", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtEmail.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contactno", SqlDbType.VarChar, 50)).Value = CType(txtContact.Value.Trim, String)
                            '  mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("State") = "Delete" Then
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

                If Session("State") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("State") = "Delete" Then
                    Response.Redirect("SupplierAgentsSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnResCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierAgentsSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSaleCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierAgentsSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierAgentsSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGeneralCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierAgentsSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnEmailCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnEmailCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("SupplierAgentsSearch.aspx")
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("State") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "supplier_agents", "supagentcode", CType(txtSuppCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This supplier agent code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "supplier_agents", "supagentname", txtSuppName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This supplier agent name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("State") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "supplier_agents", "supagentcode", "supagentname", txtSuppName.Value.Trim, CType(txtSuppCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This supplier agent name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region


#Region " Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean"
    Private Function EmailValidate(ByVal email As String, ByVal txt As HtmlInputText) As Boolean
        Try
            Dim email1length As Integer
            email1length = Len(email.Trim)
            If email1length > 255 Then
                'objcommon.MessageBox("email1 length is too large..please enter valid email exampele(abc@abc.com)..", Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email length is too large..please enter valid email exampele(abc@abc.com).');", True)
                SetFocus(txt)
                Me.Page.SetFocus(txt)
                EmailValidate = False
                Exit Function
            Else
                Dim atpos As String
                Dim dotpos As String
                Dim s1 As String
                Dim s As String
                s1 = email
                atpos = s1.LastIndexOf("@")
                dotpos = s1.LastIndexOf(".")
                s = s1.LastIndexOf(".")
                If atpos < 1 Or dotpos < 2 Or s < 4 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                    SetFocus(txt)
                    EmailValidate = False
                    Exit Function
                Else
                    Dim sp As String()
                    Dim at As String()
                    Dim dot As String()
                    Dim chkcom As String
                    Dim chkyahoo As String
                    Dim test As String
                    Dim t As String
                    sp = s1.Split(".")
                    at = s1.Split("@")
                    chkcom = sp.GetValue(sp.Length() - 1)
                    chkyahoo = at.GetValue(at.Length() - 1)
                    dot = chkyahoo.Split(".")
                    If dot.Length() > 2 Then
                        t = dot.GetValue(dot.Length() - 3)
                        test = sp.GetValue(sp.Length() - 2)
                        If test <> "co" Or chkcom.Length() > 2 Or IsNumeric(t) = True Then
                            'objutil.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    Else
                        t = dot.GetValue(dot.Length() - 2)
                        test = sp.GetValue(sp.Length() - 1)
                        If test.Length < 2 Or IsNumeric(t) = True Or IsNumeric(test) = True Then
                            'objcommon.MessageBox("Please Enter Valid E-Mail Id [e.g abc@abc.com]", Page)
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    End If
                End If
            End If
            EmailValidate = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function

#End Region

#Region "Protected Sub ddlAccPostTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub ddlAccPostTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlAccPostTo.SelectedValue = "[Select]" Then
            txtAccPostTo2.Value = ""
        Else
            txtAccPostTo2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentname", "supagentcode", ddlAccPostTo.SelectedValue)
        End If

    End Sub
#End Region

    Protected Sub ddlSector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSector.SelectedIndexChanged
        txtSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sectormaster", "sectorname", "sectorcode", ddlSector.SelectedValue)
    End Sub

    Protected Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
        txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", ddlCountry.SelectedValue)
        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCity, "citycode", "select * from citymast where active=1  and ctrycode='" & ddlCountry.SelectedValue & "' order by citycode", True)
        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select * from sectormaster where active=1  and citycode='" & ddlCity.SelectedValue & "' order by sectorcode", True)
        txtCityName.Value = ""
        txtSectorName.Value = ""
    End Sub

    Protected Sub ddlCategory_SelectedIndexChanged2(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        If ddlCategory.SelectedValue = "[Select]" Then
            txtCategoryName.Value = ""
        Else
            txtCategoryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "catmast", "catname", "catcode", ddlCategory.SelectedValue)
        End If
    End Sub

    Protected Sub ddlSelling_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSelling.SelectedIndexChanged
        txtSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellcatmast", "scatname", "scatcode", ddlSelling.SelectedValue)
    End Sub


    Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
        txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", ddlCurrency.SelectedValue)
    End Sub
    Private Sub GetValuesForResvationDetails()
        Try
            If Session("State") = "Edit" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '------------------------------------------------------
                        '-------------- Reservation Details --------------------------
                        If IsDBNull(mySqlReader("add1")) = False Then
                            txtResAddress1.Value = mySqlReader("add1")
                        Else
                            txtResAddress1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("add2")) = False Then
                            txtResAddress2.Value = mySqlReader("add2")
                        Else
                            txtResAddress2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("add3")) = False Then
                            txtResAddress3.Value = mySqlReader("add3")
                        Else
                            txtResAddress3.Value = ""
                        End If
                        If IsDBNull(mySqlReader("tel1")) = False Then
                            txtResPhone1.Value = mySqlReader("tel1")
                        Else
                            txtResPhone1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("tel2")) = False Then
                            txtResPhone2.Value = mySqlReader("tel2")
                        Else
                            txtResPhone2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("fax")) = False Then
                            txtResFax.Value = mySqlReader("fax")
                        Else
                            txtResFax.Value = ""
                        End If
                        If IsDBNull(mySqlReader("contact1")) = False Then
                            txtResContact1.Value = mySqlReader("contact1")
                        Else
                            txtResContact1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("contact2")) = False Then
                            txtResContact2.Value = mySqlReader("contact2")
                        Else
                            txtResContact2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txtResEmail.Value = mySqlReader("email")
                        Else
                            txtResEmail.Value = ""
                        End If
                        If IsDBNull(mySqlReader("commmode")) = False Then
                            If mySqlReader("commmode") = "1" Then
                                ddlComunicate.SelectedValue = "Email"
                            ElseIf mySqlReader("commmode") = "0" Then
                                ddlComunicate.SelectedValue = "Fax"
                            End If
                        End If
                        If IsDBNull(mySqlReader("selltype")) = False Then
                            If mySqlReader("selltype") = "1" Then
                                ddlSell.SelectedValue = "Beach"
                            ElseIf mySqlReader("selltype") = "0" Then
                                ddlSell.SelectedValue = "City"
                            End If
                        End If
                        If IsDBNull(mySqlReader("website")) = False Then
                            txtResWebSite.Value = mySqlReader("website")
                        Else
                            txtResWebSite.Value = ""
                        End If

                        '------------------------END-----------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Private Sub GetValuesForGeneralDetails()
        Try
            If Session("State") = "Edit" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
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
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Private Sub GetValuesForSalesDetails()
        Try
            If Session("State") = "Edit" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  Sales Details ------------------------------------

                        If IsDBNull(mySqlReader("stel1")) = False Then
                            txtSaleTelephone1.Value = mySqlReader("stel1")
                        Else
                            txtSaleTelephone1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("stel2")) = False Then
                            txtSaleTelephone2.Value = mySqlReader("stel2")
                        Else
                            txtSaleTelephone2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("sfax")) = False Then
                            txtSaleFax.Value = mySqlReader("sfax")
                        Else
                            txtSaleFax.Value = ""
                        End If
                        If IsDBNull(mySqlReader("scontact1")) = False Then
                            txtSaleContact1.Value = mySqlReader("scontact1")
                        Else
                            txtSaleContact1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("scontact2")) = False Then
                            txtSaleContact2.Value = mySqlReader("scontact2")
                        Else
                            txtSaleContact2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("semail")) = False Then
                            txtSaleEmail.Value = mySqlReader("semail")
                        Else
                            txtSaleEmail.Value = ""
                        End If
                        '------------------------END-----------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Private Sub GetValuesForAccountDetails()
        Try
            If Session("State") = "Edit" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then

                        '---------  Account Details ------------------------------------

                        If IsDBNull(mySqlReader("atel1")) = False Then
                            txtAccTelephone1.Value = mySqlReader("atel1")
                        Else
                            txtAccTelephone1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("atel2")) = False Then
                            txtAccTelephone2.Value = mySqlReader("atel2")
                        Else
                            txtAccTelephone2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("afax")) = False Then
                            txtAccFax.Value = mySqlReader("afax")
                        Else
                            txtAccFax.Value = ""
                        End If
                        If IsDBNull(mySqlReader("acontact1")) = False Then
                            txtAccContact1.Value = mySqlReader("acontact1")
                        Else
                            txtAccContact1.Value = ""
                        End If
                        If IsDBNull(mySqlReader("acontact2")) = False Then
                            txtAccContact2.Value = mySqlReader("acontact2")
                        Else
                            txtAccContact2.Value = ""
                        End If
                        If IsDBNull(mySqlReader("aemail")) = False Then
                            txtAccEmail.Value = mySqlReader("aemail")
                        Else
                            txtAccEmail.Value = ""
                        End If
                        If IsDBNull(mySqlReader("crdays")) = False Then
                            TxtAccCreditDays.Value = mySqlReader("crdays")
                        Else
                            TxtAccCreditDays.Value = ""
                        End If
                        If IsDBNull(mySqlReader("crlimit")) = False Then
                            txtAccCreditLimit.Value = mySqlReader("crlimit")
                        Else
                            txtAccCreditLimit.Value = ""
                        End If

                        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAccPostTo, "supagentcode", "select * from supplier_agents where active=1 and  supagentcode<>'" & txtSuppCode.Value.Trim & "' and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by supagentcode", True)
                        If IsDBNull(mySqlReader("agtcode")) = False Then
                            ddlAccPostTo.SelectedValue = mySqlReader("agtcode")
                            txtAccPostTo2.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentname", "supagentcode", ddlAccPostTo.SelectedValue)
                        Else
                            ddlAccPostTo.SelectedItem.Text = "[Select]"
                            txtAccPostTo2.Value = ""
                        End If

                        If IsDBNull(mySqlReader("controlacctcode")) = False Then
                            txtAccACCode.Value = mySqlReader("controlacctcode")
                        Else
                            txtAccACCode.Value = ""
                        End If

                        If IsDBNull(mySqlReader("cashsupagent")) = False Then
                            If mySqlReader("cashsupagent") = 1 Then
                                ChkCashSup.Checked = True
                            Else
                                ChkCashSup.Checked = False
                            End If
                        End If
                        '------------------------END-----------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Private Sub GetValuesForEmailDetails()
        Try
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txt As HtmlInputText
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from supagents_multiemail Where supagentcode='" & Session("RefCode") & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from supagents_multiemail Where supagentcode='" & Session("RefCode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In gv_Email.Rows

                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("contactperson")) = False Then
                            txt = GVRow.FindControl("txtPerson")
                            txt.Value = mySqlReader("contactperson")
                        End If
                        If IsDBNull(mySqlReader("email")) = False Then
                            txt = GVRow.FindControl("txtEmail")
                            txt.Value = mySqlReader("email")
                        End If
                        If IsDBNull(mySqlReader("contactno")) = False Then
                            txt = GVRow.FindControl("txtContactNo")
                            txt.Value = mySqlReader("contactno")
                        End If
                    End If
                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
    Private Sub GetValuesForMainDetails()
        Try
            If Session("State") = "Edit" Then

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & Session("RefCode") & "'", mySqlConn)
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
                            ddlTypeCode.SelectedItem.Text = mySqlReader("sptypecode")
                            txtTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", ddlTypeCode.SelectedItem.Text)


                        End If
                        'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlCategory, "catcode", "catname", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by catcode", True)
                        'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlSelling, "scatcode", "scatname", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by scatcode", True)
                        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCategory, "catcode", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by catcode", True)
                        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSelling, "scatcode", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by scatcode", True)

                        '---------- Main Details    -------------------------
                        If IsDBNull(mySqlReader("catcode")) = False Then

                            ddlCategory.SelectedItem.Text = mySqlReader("catcode")
                            txtCategoryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "catmast", "catname", "catcode", ddlCategory.SelectedItem.Text)
                        End If
                        If IsDBNull(mySqlReader("scatcode")) = False Then
                            ddlSelling.SelectedItem.Text = mySqlReader("scatcode")
                            txtSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sellcatmast", "scatname", "scatcode", ddlSelling.SelectedItem.Text)
                        End If
                        If IsDBNull(mySqlReader("currcode")) = False Then
                            ddlCurrency.SelectedItem.Text = mySqlReader("currcode")
                            txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", ddlCurrency.SelectedValue)
                        End If
                        If IsDBNull(mySqlReader("ctrycode")) = False Then
                            ddlCountry.SelectedItem.Text = mySqlReader("ctrycode")
                            txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", ddlCountry.SelectedValue)
                        End If
                        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCity, "citycode", "select * from citymast where active=1  and ctrycode='" & ddlCountry.SelectedItem.Text & "' order by citycode", True)
                        If IsDBNull(mySqlReader("citycode")) = False Then
                            ddlCity.SelectedItem.Text = mySqlReader("citycode")
                            txtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", ddlCity.SelectedItem.Text)
                        End If
                        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select * from sectormaster where active=1  and citycode='" & ddlCity.SelectedItem.Text & "' order by sectorcode", True)
                        If IsDBNull(mySqlReader("sectorcode")) = False Then
                            ddlSector.SelectedItem.Text = mySqlReader("sectorcode")
                            txtSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sectormaster", "sectorname", "sectorcode", ddlSector.SelectedItem.Text)
                        End If
                        If IsDBNull(mySqlReader("active")) = False Then
                            If CType(mySqlReader("active"), String) = "1" Then
                                chkActive.Checked = True
                            ElseIf CType(mySqlReader("active"), String) = "0" Then
                                chkActive.Checked = False
                            End If
                        End If

                        '------------------------------------------------------
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub


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




        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supagents_multiemail", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a Email Of SupplierAgent, cannot delete this SupplierAgent');", True)
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
End Class
