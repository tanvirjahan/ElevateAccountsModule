'------------================--------------=======================------------------================
'   Module Name    :   ApplicationRightsforgroup.aspx
'   Developer Name :    sandeep indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic



#End Region
Partial Class ApplicationRightsforgroup
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim chkSel As CheckBox

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            ViewState.Add("AppnRightsState", Request.QueryString("State"))
            ViewState.Add("AppnRightsRefCode", Request.QueryString("RefCode"))
            FillGridApp()
            If ViewState("AppnRightsState") = "New" Then



                lblHeading.Text = "Add New Application Rights for groups"
                btnSave.Text = "Save"
                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
            ElseIf ViewState("AppnRightsState") = "Edit" Then


                txtusername.Enabled = False
                txtusercode.Visible = False
                lblHeading.Text = "Edit Application Rights for groups"
                btnSave.Text = "Update"
                ShowRecord(CType(ViewState("AppnRightsRefCode"), String))

                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
            ElseIf ViewState("AppnRightsState") = "View" Then
                txtusername.Enabled = False
                txtusercode.Visible = False

                SetFocus(btnCancel)
                lblHeading.Text = "View Application Rights for groups"
                btnSave.Visible = False
                btnCancel.Text = "Return to Search"
                ShowRecord(CType(ViewState("AppnRightsRefCode"), String))
                DisableControl()
            ElseIf ViewState("AppnRightsState") = "Delete" Then

                txtusername.Enabled = False
                txtusercode.Visible = False
                SetFocus(btnSave)
                lblHeading.Text = "Delete Application Rights for groups"
                btnSave.Text = "Delete"
                ShowRecord(CType(ViewState("AppnRightsRefCode"), String))
                DisableControl()
                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
            End If
        End If
        btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

        End If
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim gvrow As GridViewRow
        Try
            If Page.IsValid = True Then
                If ViewState("AppnRightsState") = "New" Or ViewState("AppnRightsState") = "Edit" Then
                    If Validation() = False Then
                        Exit Sub
                    End If
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                    If ViewState("AppnRightsState") = "New" Then
                        myCommand = New SqlCommand("sp_add_group_app_master", SqlConn, sqlTrans)
                    Else
                        myCommand = New SqlCommand("sp_mod_group_app_master", SqlConn, sqlTrans)
                    End If
                    myCommand.CommandType = CommandType.StoredProcedure
                    '----------------------------------- Inserting Data Application rights
                    myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int)).Value = CType(txtusercode.Text, Long)

                    If ViewState("AppnRightsState") = "New" Then
                        myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("AppnRightsState") = "Edit" Then
                        myCommand.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    If ViewState("AppnRightsState") = "Edit" Then
                        '*** Delete 
                        myCommand = New SqlCommand("sp_del_group_app_detail", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int)).Value = CType(txtusercode.Text, Long)
                        myCommand.ExecuteNonQuery()
                    End If

                    For Each gvrow In grdApplicaion.Rows
                        chkSel = gvrow.FindControl("chkSelect")
                        If chkSel.Checked = True Then
                            myCommand = New SqlCommand("sp_add_group_app_detail", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int)).Value = CType(txtusercode.Text, Long)
                            myCommand.Parameters.Add(New SqlParameter("@appid", SqlDbType.Int)).Value = CType(gvrow.Cells(2).Text, Long)
                            myCommand.ExecuteNonQuery()
                        End If
                    Next
                    '----------------------------------- Deleting Data 

                Else 'Delete  
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                    myCommand = New SqlCommand("sp_del_group_app_master", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int)).Value = CType(txtusercode.Text, Long)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tran Commit
                clsDBConnect.dbSqlTransation(sqlTrans)               'sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)              'connection close
                'Response.Redirect("ApplicationRightsforgroupSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('AppnRightsWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)               'sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplicationRightsforgroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select groupmaster.groupname,group_app_detail.groupid,groupmaster.active,groupmaster.adddate,groupmaster.adduser,groupmaster.moddate,moduser,group_app_detail.appid from groupmaster,group_app_detail Where groupmaster.groupid=group_app_detail.groupid and group_app_detail.groupid = '" & RefCode & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Dim GVRow As GridViewRow
            Dim chkSelect As New CheckBox
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    If IsDBNull(mySqlReader("appid")) = False Then
                        For Each GVRow In grdApplicaion.Rows
                            If CType(mySqlReader("appid"), String) = GVRow.Cells(2).Text Then
                                chkSel = GVRow.FindControl("chkSelect")
                                chkSel.Checked = True
                            End If
                        Next
                    End If

                    If IsDBNull(mySqlReader("groupname")) = False Then
                        Me.txtusername.Text = CType(mySqlReader("groupname"), String)
                    Else
                        Me.txtusername.Text = ""
                    End If
                    If IsDBNull(mySqlReader("groupid")) = False Then
                        Me.txtusercode.Text = CType(mySqlReader("groupid"), String)
                    Else
                        Me.txtusercode.Text = ""
                    End If
 End While
            End If
            '*******************************End
        Catch ex As Exception
            objUtils.WritErrorLog("ApplicationRightsforgroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Private Sub FillGridApp()"
    Private Sub FillGridApp()
        Dim myDS As New DataSet
        grdApplicaion.Visible = True
        If grdApplicaion.PageIndex < 0 Then
            grdApplicaion.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            ' strSqlQry = "SELECT * FROM supplier_agents"
            strSqlQry = "select appid,appname from appmaster where appstatus=1"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            grdApplicaion.DataSource = myDS
            grdApplicaion.DataBind()
            clsDBConnect.dbConnectionClose(SqlConn)

        Catch ex As Exception
            objUtils.WritErrorLog("ApplicationRightsforgroup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        Dim GVRow As GridViewRow

        For Each GVRow In grdApplicaion.Rows
            chkSel = GVRow.FindControl("chkSelect")
            chkSel.Enabled = False
        Next

    End Sub

#End Region

#Region "Private Function Validation() As Boolean"
    Private Function Validation() As Boolean
        Try
            If txtusername.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select User Group.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtusername.ClientID + "');", True)
                Validation = False
                Exit Function
            End If
            If IsSelectApplication() = False Then
                Validation = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Select atleast one application.');", True)
                Exit Function
            End If
            Validation = True
        Catch ex As Exception
            Validation = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("UserMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Private Function IsSelectApplication() As Boolean"
    Private Function IsSelectApplication() As Boolean
        Dim GVRow As GridViewRow
        Dim chkSelect As New CheckBox
        For Each GVRow In grdApplicaion.Rows
            chkSelect = GVRow.FindControl("chkSelect")
            If chkSelect.Checked = True Then
                IsSelectApplication = True
                Exit Function
            End If
        Next
        IsSelectApplication = False
    End Function
#End Region

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("ApplicationRightsforgroupSearch.aspx")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ApplicationRightsforgroup','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getusergroup(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim usergroup As New List(Of String)
        Try


            strSqlQry = "select distinct groupname,groupid from groupmaster  where active=1 and  groupid not in (select groupid from group_app_master) and  groupname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    usergroup.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("groupname").ToString(), myDS.Tables(0).Rows(i)("groupid").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return usergroup
        Catch ex As Exception
            Return usergroup
        End Try

    End Function


End Class
