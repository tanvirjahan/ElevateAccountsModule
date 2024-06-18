'------------================--------------=======================------------------================
'   Page Name       :   FlightMaster.aspx
'   Developer Name  :   Amit Survase
'   Date            :   3 July 2008
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Partial Class FlightSectormaster
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
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("FlightsectorState", Request.QueryString("State"))
                ViewState.Add("FlightsectorRefCode", Request.QueryString("RefCode"))
                If ViewState("FlightsectorState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Flight Sector Master"
                    btnSave.Text = "Save"

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Flight Class Master?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("FlightsectorState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Flight Sector Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("FlightsectorRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Flight Class Master?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("FlightsectorState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Flight Sector Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("FlightsectorRefCode"), String))

                ElseIf ViewState("FlightsectorState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Flight Sector Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("FlightsectorRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Flight Class Master?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                '                btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("FlightSectormaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region


#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("FlightsectorState") = "View" Or ViewState("FlightsectorState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True
        ElseIf ViewState("FlightsectorState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region


#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            If Page.IsValid = True Then
                If ViewState("FlightsectorState") = "New" Or ViewState("FlightsectorState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("FlightsectorState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_flightsector", mySqlConn, sqlTrans)
                    ElseIf ViewState("FlightsectorState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_flightsector", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@flightsectorcode ", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@flightsectorname ", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("FlightsectorState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_flightsector", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@flightsectorcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("FlightClassMasterSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('FlightSectorWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("FlightSectormaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region


#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from flightsectormast Where flightsectorcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("flightsectorcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("flightsectorcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("flightsectorname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("flightsectorname"), String)
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
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FlightSectormaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region


#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("FlightClassMasterSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region


#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If ViewState("FlightsectorState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "flightsectormast", "flightsectorcode", txtCode.Value.Trim) Then
                'objUtils.MessageBox("This currency code is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Flight Sector Code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "flightsectormast", "flightsectorname", txtName.Value.Trim) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Flight Sector Name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("FlightsectorState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "flightsectormast", "flightsectorcode", "flightsectorname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Flight Sector name is already present.');", True)
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

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "pkgtkt_detail", "flightsectorcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  FlightSector is already used in a Package, cannot delete this FlightSector');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "grouptkt_detail", "flightsectorcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  FlightSector is already used in a GroupTicketing, cannot delete this FlightSector');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=FlightSectorMaster','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
