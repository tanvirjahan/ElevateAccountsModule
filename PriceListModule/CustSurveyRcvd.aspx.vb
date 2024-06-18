Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class CustSurveyRcvd
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim ObjDate As New clsDateTime
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As New SqlDataAdapter
    Dim ddlPcode As DropDownList

#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            PanelSurvey.Visible = True
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            GetValuesForSurveys()
            If CType(Session("custstate"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add New Customer - Survey From Received"
                BtnSurveySave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer survey from received?')==false)return false;")
            ElseIf CType(Session("custstate"), String) = "Edit" Then

                BtnSurveySave.Text = "Update"

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Edit Customer - Survey From Received"
                BtnSurveySave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer survey from received?')==false)return false;")
            ElseIf CType(Session("custstate"), String) = "View" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Customer - Survey From Received"
                BtnSurveySave.Visible = False
                BtnSurveyCancel.Text = "Return to Search"
                BtnSurveyCancel.Focus()

            ElseIf CType(Session("custstate"), String) = "Delete" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Delete Customer - Survey From Received"
                BtnSurveySave.Text = "Delete"
                BtnSurveySave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer survey from received?')==false)return false;")
            End If
            BtnSurveyCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If
        End If
        Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region

#Region " Protected Sub BtnSurveyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSurveyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Response.Redirect("CustomersSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region


#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("custstate") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentcode", CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentname", txtCustomerName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("custstate") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", txtCustomerName.Value.Trim, CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

#Region "Protected Sub BtnSurveySave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSurveySave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim submit As HtmlInputText
        Dim remark As HtmlInputText
        Dim datesurv As New EclipseWebSolutions.DatePicker.DatePicker
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then
                If Session("custstate") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custstate") = "Edit" Or Session("custstate") = "View" Then

                    'If ValidateEmail() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("delete from agentmast_survey where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In grvSurvey.Rows
                        submit = GvRow.FindControl("txtSubmitedBy")
                        remark = GvRow.FindControl("txtRemarkSurvey")
                        datesurv = GvRow.FindControl("dpDateSurvey")
                        If CType(submit.Value, String) <> "" And CType(remark.Value, String) <> "" And CType(datesurv.txtDate.Text, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_agentsurvey", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@submittedby", SqlDbType.VarChar, 100)).Value = CType(submit.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(remark.Value.Trim.Trim, String)
                            If datesurv.txtDate.Text = "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@surveydate", SqlDbType.DateTime)).Value = DBNull.Value
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@surveydate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(datesurv.txtDate.Text)
                            End If
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                ElseIf Session("custstate") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_Del_agentsurvey", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("delete from agentmast_survey where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("custstate") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("custstate") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("custstate") = "Delete" Then
                    ' Response.Redirect("CustomersSearch.aspx", False)

                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustSurveyRcvd.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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
                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtCustomerName.Value = mySqlReader("agentname")
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustSurveyRcvd.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        grvSurvey.Enabled = False

    End Sub
#End Region


#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromagent_detail", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a DetailsOfEarlyBirdPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_agent", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a CustomerPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If

        checkForDeletion = True
    End Function
#End Region

#Region "Private Sub GetValuesForSurveys()"

    Private Sub GetValuesForSurveys()
        Try
            If Session("custstate") = "Edit" Or Session("custstate") = "View" Or Session("custstate") = "Delete" Then
                Dim count As Long
                Dim GVRow As GridViewRow
                Dim txt As HtmlInputText
                Dim datesurv As New EclipseWebSolutions.DatePicker.DatePicker
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select count(*) from agentmast_survey Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                count = mySqlCmd.ExecuteScalar
                mySqlCmd.Dispose()
                mySqlConn.Close()
                If count > 0 Then
                    fillgrd(grvSurvey, False, count)
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    mySqlCmd = New SqlCommand("Select * from agentmast_survey Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                    mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                    For Each GVRow In grvSurvey.Rows

                        If mySqlReader.Read = True Then
                            If IsDBNull(mySqlReader("submittedby")) = False Then
                                txt = GVRow.FindControl("txtSubmitedBy")
                                txt.Value = mySqlReader("submittedby")
                            End If
                            If IsDBNull(mySqlReader("remarks")) = False Then
                                txt = GVRow.FindControl("txtRemarkSurvey")
                                txt.Value = mySqlReader("remarks")
                            End If
                            If IsDBNull(mySqlReader("surveydate")) = False Then
                                datesurv = GVRow.FindControl("dpDateSurvey")
                                'datesurv.txtDate.Text = mySqlReader("surveydate")
                                datesurv.txtDate.Text = Format("U", CType((mySqlReader("surveydate")), Date))
                            End If
                        End If
                    Next
                    mySqlCmd.Dispose()
                    mySqlReader.Close()
                    mySqlConn.Close()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region

#Region "Protected Sub Btnaddsurvey_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub Btnaddsurvey_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddLinesSurvey()
    End Sub
#End Region

#Region "Private Sub AddLinesSurvey()"
    Private Sub AddLinesSurvey()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grvSurvey.Rows.Count + 1
        Dim txt As HtmlInputText
        Dim fDate(count) As String
        Dim datesurv As New EclipseWebSolutions.DatePicker.DatePicker
        Dim submit(count) As String
        Dim remark(count) As String
        ' Dim dateS(count) As Date
        'Dim chk(count) As Boolean
        Dim n As Integer = 0
        Try
            For Each GVRow In grvSurvey.Rows
                txt = GVRow.FindControl("txtSubmitedBy")
                submit(n) = CType(Trim(txt.Value), String)
                txt = Nothing
                txt = GVRow.FindControl("txtRemarkSurvey")
                remark(n) = CType(Trim(txt.Value), String)
                datesurv = GVRow.FindControl("dpDateSurvey")
                fDate(n) = CType(datesurv.txtDate.Text, String)
                n = n + 1
            Next
            fillgrd(grvSurvey, False, grvSurvey.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grvSurvey.Rows
                If n = i Then
                    Exit For
                End If
                txt = GVRow.FindControl("txtSubmitedBy")
                txt.Value = submit(n)
                txt = Nothing
                txt = GVRow.FindControl("txtRemarkSurvey")
                txt.Value = remark(n)
                datesurv = GVRow.FindControl("dpDateSurvey")
                datesurv.txtDate.Text = fDate(n)
                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
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

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustSurveyRcvd','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
