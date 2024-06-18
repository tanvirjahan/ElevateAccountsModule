Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic

Partial Class PriceListModule_CustBookEngCtry
    Inherits System.Web.UI.Page
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
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
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
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
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentcode")) = False Then
                        Me.txtCustomerCode.Value = mySqlReader("agentcode")
                        Me.txtCustomerName.Value = mySqlReader("agentname")
                    End If
                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txtctrycode As TextBox
            Dim txtctryname As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from agentmast_countries Where agentcode='" & RefCode & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillDategrd(grdbookengctry, False, count)
                'fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast_countries Where agentcode='" & RefCode & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In grdbookengctry.Rows
                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("ctrycode")) = False Then
                            txtctrycode = GVRow.FindControl("txtctrycode")
                            txtctryname = GVRow.FindControl("txtctryname")
                            txtctrycode.Text = mySqlReader("ctrycode")
                            txtctryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        End If


                    End If
                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            'changed  Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            If CType(Request.QueryString("appid"), String) = "1" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

           ElseIf  CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomerGroupSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=1", String), "1")


            End If

            Panelctry.Visible = True
            fillDategrd(grdbookengctry, True)
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            'GetValuesForResvationDetails()

            If CType(Session("custState"), String) = "New" Then
                '   SetFocus(txtCustomerCode)
                lblmainheading.Text = "Add New Customer - Countries Details"
                Page.Title = Page.Title + " " + "New Customer - Countries Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                BtnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer reservation details?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "Edit" Then

                BtnSave.Text = "Update"

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblmainheading.Text = "Edit Customer - Countries Details"
                Page.Title = Page.Title + " " + "Edit Customer - Countries Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer reservation details?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "View" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdbookengctry.Enabled = False
                btnaddrow.Enabled = False
                btnHelp.Enabled = False
                btndelrow.Enabled = False
                'DisableControl()
                lblmainheading.Text = "View Customer - Countries Details"
                Page.Title = Page.Title + " " + "View Customer - Countries Details"
                BtnSave.Visible = False
                BtnCancel.Text = "Return to Search"
                BtnCancel.Focus()

            ElseIf CType(Session("custState"), String) = "Delete" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdbookengctry.Enabled = False
                btndelrow.Enabled = False
                lblmainheading.Text = "Delete Customer - Countries Details"
                Page.Title = Page.Title + " " + "Delete Customer - Countries Details"
                BtnSave.Text = "Delete"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer reservation details?')==false)return false;")
            ElseIf CType(Session("CustState"), String) = "Addclient" Then

                Dim clientname As String
                Dim webuser As String
                'SetFocus(txtCustomerCode)
                clientname = CType(Session("clientname"), String)
                webuser = CType(Session("webusername"), String)
                'If CType(Session("ExistClient"), String) = "1" Then
                '    DisableControl()
                '    BtnResSave.Visible = False
                'End If
                'ShowRecord_registration(clientname, webuser)

                lblmainheading.Text = "Add New Customer - Main Details"
                Page.Title = Page.Title + " " + "New Customer - Countries Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")


            End If
            BtnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            End If
            Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region



#Region "Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim txtctrycode As TextBox
        Dim txtctryname As TextBox

        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("custState").ToString() = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custState").ToString() = "Edit" Then

                    '    'check  If ValidateEmail() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    'mySqlCmd = New SqlCommand("delete from agentmast_countries where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd = New SqlCommand("sp_add_addmast_bookctrylog", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    ' mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In grdbookengctry.Rows
                        txtctrycode = GvRow.FindControl("txtctrycode")
                        txtctryname = GvRow.FindControl("txtctryname")

                        If CType(txtctryname.Text, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_addmast_bookctry", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode ", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = CType(txtctrycode.Text.Trim, String)
                            ' mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                ElseIf Session("custState") = "Delete" Then
                    'check If checkForDeletion() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("delete from agentmast_countries where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("custState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("custState") = "Delete" Then
                    Response.Redirect("CustomersSearch.aspx", False)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustBookEngCtry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



    Protected Sub btnaddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddrow.Click


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdbookengctry.Rows.Count + 1
        Dim countrycode(count) As String
        Dim countryname(count) As String

        Dim n As Integer = 0
        Dim txtctrycode As TextBox
        Dim txtctryname As TextBox


        Try
            For Each GVRow In grdbookengctry.Rows
                txtctrycode = GVRow.FindControl("txtctrycode")
                countrycode(n) = CType(txtctrycode.Text, String)
                txtctryname = GVRow.FindControl("txtctryname")
                countryname(n) = CType(txtctryname.Text, String)

                n = n + 1
            Next
            fillDategrd(grdbookengctry, False, grdbookengctry.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdbookengctry.Rows
                If n = i Then
                    Exit For
                End If
                txtctrycode = GVRow.FindControl("txtctrycode")
                txtctrycode.Text = countrycode(n)
                txtctryname = GVRow.FindControl("txtctryname")
                txtctryname.Text = countryname(n)

                n = n + 1
            Next
            'Dim gridNewrow As GridViewRow
            'gridNewrow = grdbookengctry.Rows(grdbookengctry.Rows.Count - 1)
            'Dim strRowId As String = gridNewrow.ClientID
            'Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdbookengctry.Rows.Count - 1, String) + "');")
            'ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustBooking','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


 
    Protected Sub BtnCancel_Click(sender As Object, e As System.EventArgs) Handles BtnCancel.Click
        'Response.Redirect("CustomersSearch.aspx")
        'If Session("postback") <> "Addclient" Then
        '    Dim strscript As String = ""
        '    strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Else

        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


        ' End If
    End Sub

    Protected Sub btndelrow_Click(sender As Object, e As System.EventArgs) Handles btndelrow.Click


        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdbookengctry.Rows.Count + 1
        Dim countrycode(count) As String
        Dim countryname(count) As String

        Dim n As Integer = 0
        Dim txtctrycode As TextBox
        Dim txtctryname As TextBox

        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdbookengctry.Rows
                chkSelect = GVRow.FindControl("chkbookengctry")
                If chkSelect.Checked = False Then
                    txtctrycode = GVRow.FindControl("txtctrycode")
                    countrycode(n) = CType(txtctrycode.Text, String)
                    txtctryname = GVRow.FindControl("txtctryname")
                    countryname(n) = CType(txtctryname.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdbookengctry.Rows.Count > 1 Then
                fillDategrd(grdbookengctry, False, grdbookengctry.Rows.Count - deletedrow)
            Else
                fillDategrd(grdbookengctry, False, grdbookengctry.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdbookengctry.Rows
                If GVRow.RowIndex < count Then
                    txtctrycode = GVRow.FindControl("txtctrycode")
                    txtctrycode.Text = countrycode(n)
                    txtctryname = GVRow.FindControl("txtctryname")
                    txtctryname.Text = countryname(n)

                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub grdbookengctry_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdbookengctry.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
        '    e.Row.Attributes("onclick") = String.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex)
        '    e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event);"
        '    e.Row.Attributes("onselectstart") = "javascript:return false;"
        'End If

    End Sub
End Class
