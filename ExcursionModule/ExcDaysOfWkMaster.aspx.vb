






Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic



Partial Class ExcDaysOfWkMaster
    Inherits System.Web.UI.Page
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim n As Integer = 0
    Dim CopyRow As Integer = 0
    Dim blankrow As Integer = 0
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim CopyClick As Integer = 0
    Dim CopyRowlist As New ArrayList
    Dim txtstarttimenew As New ArrayList
    Dim txtendtimenew As New ArrayList
    Dim txtdurationnew As New ArrayList
    Dim txtminhoursnew As New ArrayList
    Dim txtmaxhoursnew As New ArrayList
    Dim ddldurationunitnew As New ArrayList
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

        grd.DataSource = filldaysgrid() ' CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region

    <System.Web.Script.Services.ScriptMethod()> _
         <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplierslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliernames As New List(Of String)
        Try
            strSqlQry = "select partycode,partyname from partymast where active=1 and sptypecode=(select option_selected from reservation_parameters where param_id=1033) and  partyname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliernames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))

                Next
            End If
            Return suppliernames
        Catch ex As Exception
            Return suppliernames
        End Try

    End Function
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
    'Protected Sub btnctrygrps_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim objbtn As Button = CType(sender, Button)
    '        Dim rowid As Integer = 0
    '        Dim row As GridViewRow
    '        row = CType(objbtn.NamingContainer, GridViewRow)
    '        rowid = row.RowIndex

    '        ViewState("rowid") = rowid
    '        CalluserControlBindMethod(rowid, "Bind")
    '    Catch ex As Exception
    '    End Try




    'End Sub


    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

    Private Sub fillweekdays(ByVal refcode As String)

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        Dim dt2 As DataSet
        Dim chk As CheckBox
        Dim txtstarttime As TextBox
        Dim txtendtime As TextBox
        Dim txtduration As TextBox
        Dim txtminhour As TextBox
        Dim txtmaxhour As TextBox
        Dim ddldurationunit As DropDownList

        Dim lblSrNo As Label
        dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select weekorder,startime,endtime,duration,durationunit,minhours,maxhours from excursiontypes_weekdays(nolock) where exctypcode='" & refcode & "'")

        Dim dv2 As DataView = New DataView(dt2.Tables(0))

        For Each gvRow As GridViewRow In grdweekdays.Rows
            lblSrNo = gvRow.FindControl("lblSrNo")
            chk = gvRow.FindControl("chkwkdays")
            chk.Checked = False

            If dt2.Tables.Count > 0 Then
                dv2.RowFilter = "weekorder=" & lblSrNo.Text
                If dv2.ToTable.Rows.Count > 0 Then
                    txtstarttime = CType(gvRow.FindControl("txtstarttime"), TextBox)
                    txtstarttime.Text = dv2.ToTable.Rows(0).Item(1).ToString
                    txtendtime = CType(gvRow.FindControl("txtendtime"), TextBox)
                    txtendtime.Text = dv2.ToTable.Rows(0).Item(2).ToString
                    txtduration = CType(gvRow.FindControl("txtduration"), TextBox)
                    txtduration.Text = dv2.ToTable.Rows(0).Item(3).ToString
                    txtminhour = CType(gvRow.FindControl("txtminhour"), TextBox)
                    txtminhour.Text = dv2.ToTable.Rows(0).Item("minhours").ToString
                    txtmaxhour = CType(gvRow.FindControl("txtmaxhour"), TextBox)
                    txtmaxhour.Text = dv2.ToTable.Rows(0).Item("maxhours").ToString
                    ddldurationunit = CType(gvRow.FindControl("ddldurationunit"), DropDownList)
                    ddldurationunit.SelectedValue = dv2.ToTable.Rows(0).Item(4).ToString
                    If dv2.ToTable.Rows(0).Item(4).ToString = "Days" Then

                        txtstarttime.Enabled = False
                        txtendtime.Enabled = False
                        txtduration.Enabled = False
                    ElseIf dv2.ToTable.Rows(0).Item(4).ToString = "Hours" Then
                        txtstarttime.Enabled = True
                        txtendtime.Enabled = True
                        txtduration.Enabled = True
                    End If
                    chk.Checked = True
                End If
                End If
        Next

     
        clsDBConnect.dbConnectionClose(mySqlConn)              'connection close 


    End Sub
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)


        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from excursiontypes Where exctypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("exctypcode")) = False Then
                        txtCustomerCode.Value = mySqlReader("exctypcode")
                        Me.txtCustomerName.Value = mySqlReader("exctypname")
                    End If
                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()
            mySqlCmd.Dispose()
            mySqlReader.Close()
            fillweekdays(RefCode)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcSectorMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then
     
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("ExcursionModule\ExcursionSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            Panelctry.Visible = True
            fillDategrd(grdweekdays, True, 6)
   


            If CType(Session("ExcTypesState"), String) = "New" Then
                '   SetFocus(txtCustomerCode)
                lblmainheading.Text = "Add New Excursion - Supplier Details"
                Page.Title = Page.Title + " " + " New Excursion - Supplier Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                BtnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer reservation details?')==false)return false;")
            ElseIf CType(Session("ExcTypesState"), String) = "Edit" Then

                BtnSave.Text = "Update"

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblmainheading.Text = "Edit Excursion- Days of Week Details"
                Page.Title = Page.Title + " " + "Edit Excursion - Days of Week  Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                filldaysgrid()
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer reservation details?')==false)return false;")
            ElseIf CType(Session("ExcTypesState"), String) = "View" Then

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdweekdays.Enabled = False
                'btnaddrow.Enabled = False
                btnHelp.Enabled = False
                ' btndelrow.Enabled = False
                'DisableControl()
                filldaysgrid()
                lblmainheading.Text = "View Excursion - Days of Week Details"
                Page.Title = Page.Title + " " + "View Excursion - Days of Week Details"
                BtnSave.Visible = False
                BtnCancel.Text = "Return to Search"
                BtnCancel.Focus()

            ElseIf CType(Session("ExcTypesState"), String) = "Delete" Then

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdweekdays.Enabled = False
                ' btndelrow.Enabled = False
                filldaysgrid()
                lblmainheading.Text = "Delete Excursion - Days of Week Details"
                Page.Title = Page.Title + " " + "Delete Excursion - Days of Week Details"
                BtnSave.Text = "Delete"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer reservation details?')==false)return false;")

            End If
            BtnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

        End If
        ' Session.Add("submenuuser", "CustomersSearch.aspx")
        Session.Add("submenuuser", "ExSuppDetailsMaster.aspx")
    End Sub
#End Region




#Region "Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click


        Dim txtstarttime As TextBox
        Dim txtendtime As TextBox
        Dim txtduration As TextBox
        Dim txtMinHour As TextBox
        Dim txtMaxHour As TextBox
        Dim ddldurationunit As DropDownList
        Dim GvRow As GridViewRow
        Dim chkpreferred As CheckBox
        Dim chkwkdays As CheckBox
        Try
            If Page.IsValid = True Then
                If Session("ExcTypesState").ToString() = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("ExcTypesState").ToString() = "Edit" Then

                    '    'check  If ValidateEmail() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    'mySqlCmd = New SqlCommand("delete from agentmast_countries where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd = New SqlCommand("sp_add_addmast_exctypwkdayslog ", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    ' mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In grdweekdays.Rows
                        txtstarttime = GvRow.FindControl("txtstarttime")
                        txtendtime = GvRow.FindControl("txtendtime")
                        txtduration = GvRow.FindControl("txtduration")
                        txtMinHour = GvRow.FindControl("txtMinHour")
                        txtMaxHour = GvRow.FindControl("txtMaxHour")
                        ddldurationunit = GvRow.FindControl("ddldurationunit")
                        Dim lblorder As Label = GvRow.FindControl("lblSrNo")

                        chkwkdays = GvRow.FindControl("chkwkdays")

                        mySqlCmd = New SqlCommand("sp_add_exctypwkdays ", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        If chkwkdays.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@weekday ", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(2).Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@weekorder", SqlDbType.Int, 4)).Value = CType(lblorder.Text.Trim, String)

                            If CType(txtstarttime.Text, String) <> "" Then

                                mySqlCmd.Parameters.Add(New SqlParameter("@startime", SqlDbType.VarChar, 10)).Value = CType(txtstarttime.Text, String)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@startime", SqlDbType.VarChar, 10)).Value = "0"
                            End If

                            If CType(txtendtime.Text, String) <> "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@endtime ", SqlDbType.VarChar, 10)).Value = CType(txtendtime.Text, String)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@endtime ", SqlDbType.VarChar, 10)).Value = "0"
                            End If
                            If CType(Val(txtduration.Text), Integer) <> 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@duration ", SqlDbType.Int)).Value = CType(Val(txtduration.Text), Integer)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@duration ", SqlDbType.Int)).Value = DBNull.Value
                            End If
                            If CType(Val(txtMinHour.Text), Integer) <> 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@minhours ", SqlDbType.Int)).Value = CType(Val(txtMinHour.Text), Integer)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@minhours ", SqlDbType.Int)).Value = DBNull.Value
                            End If
                            If CType(Val(txtMaxHour.Text), Integer) <> 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@maxhours ", SqlDbType.Int)).Value = CType(Val(txtMaxHour.Text), Integer)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@maxhours ", SqlDbType.Int)).Value = DBNull.Value
                            End If

                            mySqlCmd.Parameters.Add(New SqlParameter("@durationunit", SqlDbType.VarChar, 10)).Value = ddldurationunit.SelectedValue
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If

                    Next
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    'Dim strscript As String = ""
                    'strscript = "window.opener.__doPostBack('ExcDaysOfWeekMaster', '');window.opener.focus();window.close();"


                ElseIf Session("ExcTypesState") = "Delete" Then
                    'check If checkForDeletion() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_exctypmaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()


                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("ExcTypesState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("ExcTypesState") = "Delete" Then

                    Dim strscript As String = ""

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Deleted Successfully.');", True)

                    strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
            ShowRecord(txtCustomerCode.Value)
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcDaysOfWkMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


    Private Function filldaysgrid() As DataTable


        Dim dt As New DataTable
        dt.Columns.Add("SrNo")
        dt.Columns.Add("wkdays")
        dt.Columns.Add("starttime")
        dt.Columns.Add("endtime")
        dt.Columns.Add("duration")
        dt.Columns.Add("durationunit")


        dt.Rows.Add("1", "SUNDAY", "", "", "", "")
        dt.Rows.Add("2", "MONDAY", "", "", "", "")
        dt.Rows.Add("3", "TUESDAY", "", "", "", "")
        dt.Rows.Add("4", "WEDNESDAY", "", "", "", "")
        dt.Rows.Add("5", "THURSDAY", "", "", "", "")
        dt.Rows.Add("6", "FRIDAY", "", "", "", "")
        dt.Rows.Add("7", "SATURDAY", "", "", "", "")
    


        Return dt
    End Function


    Private Sub copylines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdweekdays.Rows.Count + 1

        Dim n As Integer = 0


        Dim starttime(count) As String
        Dim endtime(count) As String
        Dim duration(count) As String
        Dim durationunit(count) As String
        Dim minhours(count) As String
        Dim maxhours(count) As String

        '   blankrow = 0

        '   CopyRow = 0


        Try

            For Each GVRow In grdweekdays.Rows


                Dim chkSelect As CheckBox = GVRow.FindControl("chkwkdays")

                Dim txtstarttime As TextBox = GVRow.FindControl("txtstarttime")
                Dim txtendtime As TextBox = GVRow.FindControl("txtendtime")
                Dim txtduration As TextBox = GVRow.FindControl("txtduration")
                Dim txtminhour As TextBox = GVRow.FindControl("txtminhour")
                Dim txtmaxhour As TextBox = GVRow.FindControl("txtmaxhour")
                Dim ddldurationunit As DropDownList = GVRow.FindControl("ddldurationunit")
        
     



                If chkSelect.Checked = True Then
                    CopyRowlist.Add(n)
                    CopyRow = n
                End If

                starttime(n) = CType(txtstarttime.Text, String)
                endtime(n) = CType(txtendtime.Text, String)
                duration(n) = CType(txtduration.Text, String)
                durationunit(n) = CType(ddldurationunit.Text, String)
                minhours(n) = CType(txtminhour.Text, String)
                maxhours(n) = CType(txtmaxhour.Text, String)

                txtstarttimenew.Add(CType(txtstarttime.Text, String))
                txtendtimenew.Add(CType(txtendtime.Text, String))
                txtdurationnew.Add(CType(txtduration.Text, String))
                txtminhoursnew.Add(CType(txtminhour.Text, String))
                txtmaxhoursnew.Add(CType(txtmaxhour.Text, String))
                ddldurationunitnew.Add(CType(ddldurationunit.SelectedValue, String))

                If chkSelect.Checked = False And (txtstarttime.Text = "") Then
                    blankrow = blankrow + 1
                End If

                n = n + 1
            Next




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

    End Sub


    Protected Sub btncopytonextrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopytonextrow.Click
        CopyClick = 2
        ' Addlines()
        copylines()
        n = 0
        ' setdynamicvalues()
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = grdweekdays.Rows.Count '+ 1


            Dim n As Integer = 0



            For Each GVRow In grdweekdays.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then




                Dim chkSelect As CheckBox = GVRow.FindControl("chkwkdays")

                Dim txtstarttime As TextBox = GVRow.FindControl("txtstarttime")
                Dim txtendtime As TextBox = GVRow.FindControl("txtendtime")
                Dim txtduration As TextBox = GVRow.FindControl("txtduration")
                Dim txtminhour As TextBox = GVRow.FindControl("txtminhour")
                Dim txtmaxhour As TextBox = GVRow.FindControl("txtmaxhour")

                Dim ddldurationunit As DropDownList = GVRow.FindControl("ddldurationunit")
                If ddldurationunit.SelectedValue = "Days" Then
                    txtstarttime.Enabled = False
                    txtendtime.Enabled = False
                    txtduration.Enabled = False
                ElseIf ddldurationunit.SelectedValue = "Hours" Then
                    txtstarttime.Enabled = True
                    txtendtime.Enabled = True
                    txtduration.Enabled = True
                End If




                If n > CopyRow And txtstarttime.Text = "" Then

                    txtstarttime.Text = txtstarttimenew.Item(CopyRow)
                    txtendtime.Text = txtendtimenew.Item(CopyRow)
                    txtduration.Text = txtdurationnew.Item(CopyRow)
                    ddldurationunit.SelectedValue = ddldurationunitnew.Item(CopyRow)
                    If ddldurationunit.SelectedValue = "Days" Then
                        txtstarttime.Enabled = False
                        txtendtime.Enabled = False
                        txtduration.Enabled = False
                    ElseIf ddldurationunit.SelectedValue = "Hours" Then
                        txtstarttime.Enabled = True
                        txtendtime.Enabled = True
                        txtduration.Enabled = True
                    End If
                    txtminhour.Text = txtminhoursnew.Item(CopyRow)
                    txtmaxhour.Text = txtmaxhoursnew.Item(CopyRow)

                    chkSelect.Checked = True
                    'txtnoofchidauto.ContextKey = txtrmtypcode.Text
                    Exit For

                End If
                n = n + 1
            Next
            CopyClick = 0
            ClearArray()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


    End Sub

    Sub ClearArray()
        txtstarttimenew.Clear()
        txtendtimenew.Clear()
        txtdurationnew.Clear()
        ddldurationunitnew.Clear()
        txtminhoursnew.Clear()
        txtmaxhoursnew.Clear()
    End Sub


    Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=excursiondays','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As System.EventArgs) Handles BtnCancel.Click

 

        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExcursionSuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        ' End If
    End Sub




#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region



    Protected Sub grdweekdays_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdweekdays.RowDataBound

        If (e.Row.RowType = DataControlRowType.DataRow) Then


            Dim txtstarttime As TextBox
            Dim txtendtime As TextBox
            Dim txtduration As TextBox
            Dim ddldurationunit As DropDownList

            Dim chkwkdays As CheckBox = e.Row.FindControl("chkwkdays")
            txtstarttime = e.Row.FindControl("txtstarttime")
            txtendtime = e.Row.FindControl("txtendtime")
            txtduration = e.Row.FindControl("txtduration")
            ddldurationunit = e.Row.FindControl("ddldurationunit")


            txtstarttime.Attributes.Add("onchange", "chkstarttime('" & txtstarttime.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")
            'txtendtime.Attributes.Add ("onchange","calcduration('"&txtstarttime.ClientID &"','"&txtendtime.ClientID &"'
            txtendtime.Attributes.Add("onchange", "chkendtime('" & txtstarttime.ClientID & "','" & txtendtime.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")

            ddldurationunit.Attributes.Add("onchange", "disabledbox('" & ddldurationunit.ClientID & "','" + txtstarttime.ClientID + "','" + txtendtime.ClientID + "','" + txtduration.ClientID + "','" + CType(e.Row.RowIndex, String) + "')")
        End If

    End Sub

    Protected Sub btncleartimings_Click(sender As Object, e As System.EventArgs) Handles btncleartimings.Click
     
        Dim row As GridViewRow
        For Each gvrow In grdweekdays.Rows
            Dim txtstarttime As TextBox = gvrow.FindControl("txtstarttime")
            Dim txtendtime As TextBox = gvrow.FindControl("txtendtime")
            Dim txtduration As TextBox = gvrow.FindControl("txtduration")
            Dim txtminhour As TextBox = gvrow.FindControl("txtminhour")
            Dim txtmaxhour As TextBox = gvrow.FindControl("txtmaxhour")
            txtstarttime.Text = " "
            
            txtendtime.Text = ""
            txtduration.Text = ""
            txtminhour.Text = ""
            txtmaxhour.Text = ""
        Next
    End Sub
End Class



