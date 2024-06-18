




Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic




Partial Class ExcSuppDetailsMaster
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
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txtpartycode As TextBox
            Dim txtpartyname As TextBox
            Dim chkpreferred As CheckBox
            Dim chkcash As CheckBox
            Dim txtcitycode As TextBox
            Dim txtpriority As TextBox
            Dim txtoldpartycode As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from excursiontypes_suppliers Where exctypcode='" & RefCode & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillDategrd(grdsuppdetails, False, count)
                'fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from excursiontypes_suppliers Where exctypcode='" & RefCode & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In grdsuppdetails.Rows
                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("exctypcode")) = False Then


                            txtpartycode = GVRow.FindControl("txtpartycode")
                            txtoldpartycode = GVRow.FindControl("txtoldpartycode")
                            txtpartyname = GVRow.FindControl("txtpartyname")
                            chkpreferred = GVRow.FindControl("chkpreferred")
                            chkcash = GVRow.FindControl("chkcash")
                            txtpriority = GVRow.FindControl("txtpriority")
                            txtpartycode.Text = CType(mySqlReader("partycode"), String)
                            txtoldpartycode.Text = CType(mySqlReader("partycode"), String)
                            txtpartyname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", CType(mySqlReader("partycode"), String))
                            If CType(mySqlReader("preferred"), Integer) = "1" Then
                                chkpreferred.Checked = True
                            ElseIf CType(mySqlReader("preferred"), Integer) = "0" Then
                                chkpreferred.Checked = False
                            End If
                            If CType(mySqlReader("cash"), Integer) = "1" Then
                                chkcash.Checked = True
                            ElseIf CType(mySqlReader("cash"), Integer) = "0" Then
                                chkcash.Checked = False
                            End If
                            If CType(mySqlReader("prioritize"), Integer) <> "0" Then
                                txtpriority.Text = CType(mySqlReader("prioritize"), Integer)
                            Else
                                txtpriority.Text = ""
                            End If
                        End If
                    End If


                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()

                'DisableSuppliers()
            End If
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
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If
            Session("excursionsupplier_deleterow") = Nothing
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("ExcursionModule\ExcursionSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            Panelctry.Visible = True
            fillDategrd(grdsuppdetails, True)
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            'GetValuesForResvationDetails()
            deletedrows()
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
                lblmainheading.Text = "Edit Excursion - Supplier Details"
                Page.Title = Page.Title + " " + "Edit Excursion - Supplier Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer reservation details?')==false)return false;")
            ElseIf CType(Session("ExcTypesState"), String) = "View" Then

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdsuppdetails.Enabled = False
                btnaddrow.Enabled = False
                btnHelp.Enabled = False
                btndelrow.Enabled = False
                'DisableControl()
                lblmainheading.Text = "View Excursion - Supplier Details"
                Page.Title = Page.Title + " " + "View Excursion - Supplier Details"
                BtnSave.Visible = False
                BtnCancel.Text = "Return to Search"
                BtnCancel.Focus()

            ElseIf CType(Session("ExcTypesState"), String) = "Delete" Then

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdsuppdetails.Enabled = False
                btndelrow.Enabled = False
                lblmainheading.Text = "Delete Excursion - Supplier Details"
                Page.Title = Page.Title + " " + "Delete Excursion - Supplier Details"
                BtnSave.Text = "Delete"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                'BtnResSave.Attributes.Add("onclick", "SELECT countries FROM excursiontypes_suppliers WHERE exctypcode='ET/000008' and partycode='E/000009'

            End If
            BtnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

        End If
        ' Session.Add("submenuuser", "CustomersSearch.aspx")
        Session.Add("submenuuser", "ExSuppDetailsMaster.aspx")
    End Sub
#End Region

    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow
        Dim cnt As Integer = 0
        Dim txtpartycode As TextBox
        Dim chkpreferred As CheckBox
        Dim txtpartyname As TextBox
        Dim txtoldpartycode As TextBox

        For Each gvRow In grdsuppdetails.Rows
            txtpartycode = gvRow.FindControl("txtpartycode")
            chkpreferred = gvRow.FindControl("chkpreferred")
            txtpartyname = gvRow.FindControl("txtpartyname")

            If txtpartyname.Text <> "" And chkpreferred.Checked = True Then
                cnt = cnt + 1
            End If

        Next

        If cnt > 1 Or cnt = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Only One Preferred Supplier Need to select.');", True)
            ValidateSave = False
            Exit Function
        End If

        Dim MyAdapter As SqlDataAdapter
        Dim ds As New DataSet
        Dim strMsg As String = ""


        For Each gvRow In grdsuppdetails.Rows
            txtpartycode = gvRow.FindControl("txtpartycode")
            chkpreferred = gvRow.FindControl("chkpreferred")
            txtpartyname = gvRow.FindControl("txtpartyname")
            txtoldpartycode = gvRow.FindControl("txtoldpartycode")

            If txtoldpartycode.Text <> "" And txtoldpartycode.Text <> txtpartycode.Text And txtpartycode.Text <> "" Then

                strSqlQry = "select distinct h.eplistcode , 'SectorCost PriceList' Options from exccplist_header h(nolock), exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and h.partycode='" & txtoldpartycode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "' union all  " _
                            & " select distinct h.eplistcode,'MultiCost Price List' Options from excmulticplist_header h(nolock), excmulticplist_detail d(nolock)  where h.eplistcode=d.eplistcode and d.partycode='" & txtoldpartycode.Text & "' and h.exctypcode='" & txtCustomerCode.Value.Trim & "'"


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(ds, "showsectors")


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then
                            Dim partyname As String = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", CType(txtoldpartycode.Text, String))
                            strMsg = "For this " + partyname + "  Supplier Already Entered Cost Price list You Can not Change  Details below" + "\n"

                            For i = 0 To ds.Tables(0).Rows.Count - 1

                                strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                            Next

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            ValidateSave = False
                            Exit Function
                        End If
                    End If
                End If
            End If



        Next


        ValidateSave = True
    End Function
    Private Sub DisableSuppliers()

        Dim MyAdapter As SqlDataAdapter
        Dim ds As New DataSet
        Dim strMsg As String = ""
        Dim cnt As Integer = 0

        Try
            For Each GvRow In grdsuppdetails.Rows


                Dim txtpartycode As TextBox = GvRow.FindControl("txtpartycode")
                Dim txtpartyname As TextBox = GvRow.FindControl("txtpartyname")


                strSqlQry = "select sum(eplistcode) from (select  count(distinct h.eplistcode) eplistcode  from exccplist_header h(nolock), exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and h.partycode='" & txtpartycode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "' union all  " _
                            & " select count(distinct h.eplistcode) eplistcode from excmulticplist_header h(nolock), excmulticplist_detail d(nolock)  where h.eplistcode=d.eplistcode and d.partycode='" & txtpartycode.Text & "' and h.exctypcode='" & txtCustomerCode.Value.Trim & "') rs"



                cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)

                If cnt > 0 Then
                    txtpartyname.Enabled = False
                End If




            Next



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcsectorMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#Region "Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click


        Dim txtpartycode As TextBox
        Dim txtpartyname As TextBox
        Dim txtpriority As TextBox
        Dim GvRow As GridViewRow
        Dim chkpreferred As CheckBox
        Dim chkcash As CheckBox
        Try
            If Page.IsValid = True Then

                If ValidateSave() = False Then
                    Exit Sub
                End If

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
                    mySqlCmd = New SqlCommand("sp_add_addmast_exctypsupplog ", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    ' mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    'Delete from table which is available session 
                    Dim dt As DataTable = CType(Session("excursionsupplier_deleterow"), DataTable)
                    Dim row As DataRow
                    If dt.Rows.Count > 0 Or Session("excursionsupplier_deleterow") Is Nothing Then



                        For Each row In dt.Rows

                            mySqlCmd = New SqlCommand("sp_del_exctypsupp", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(row.Item(0), String)
                            mySqlCmd.ExecuteNonQuery()
                        Next

                    End If

                    For Each GvRow In grdsuppdetails.Rows
                        txtpartycode = GvRow.FindControl("txtpartycode")
                        txtpartyname = GvRow.FindControl("txtpartyname")
                        txtpriority = GvRow.FindControl("txtpriority")
                        chkpreferred = GvRow.FindControl("chkpreferred")
                        chkcash = GvRow.FindControl("chkcash")
                        If CType(txtpartyname.Text, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_exctypsupp", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode ", SqlDbType.VarChar, 20)).Value = CType(txtpartycode.Text.Trim, String)
                            If chkpreferred.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.VarChar, 100)).Value = "1"
                            ElseIf chkpreferred.Checked = False Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@preferred", SqlDbType.VarChar, 100)).Value = "0"
                            End If
                            If chkcash.Checked = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@cash", SqlDbType.VarChar, 100)).Value = "1"
                            ElseIf chkcash.Checked = False Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@cash", SqlDbType.VarChar, 100)).Value = "0"
                            End If
                            If CType(txtpriority.Text.Trim, String) = "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@prioritize ", SqlDbType.Int)).Value = "0"
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@prioritize ", SqlDbType.Int)).Value = CType(txtpriority.Text.Trim, Integer)
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@countries ", SqlDbType.Int)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@agents ", SqlDbType.Int)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If

                    Next
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
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
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcSuppDetailsMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



    Protected Sub btnaddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddrow.Click


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdsuppdetails.Rows.Count + 1
        Dim partycode(count) As String
        Dim partyname(count) As String
        Dim prefered(count) As String
        Dim cash(count) As String
        Dim priority(count) As String
        Dim oldpartycode(count) As String

        Dim n As Integer = 0
        Dim txtpartycode As TextBox
        Dim txtoldpartycode As TextBox
        Dim txtpartyname As TextBox
        Dim chkpreferred As CheckBox
        Dim chkcash As CheckBox
        Dim txtpriority As TextBox

        Try
            For Each GVRow In grdsuppdetails.Rows
                txtpartycode = GVRow.FindControl("txtpartycode")
                partycode(n) = CType(txtpartycode.Text, String)
                txtpartyname = GVRow.FindControl("txtpartyname")
                partyname(n) = CType(txtpartyname.Text, String)
                chkpreferred = GVRow.FindControl("chkpreferred")

                txtoldpartycode = GVRow.FindControl("txtoldpartycode")
                oldpartycode(n) = CType(txtoldpartycode.Text, String)

                If chkpreferred.Checked = True Then
                    prefered(n) = "1"
                ElseIf chkpreferred.Checked = False Then
                    prefered(n) = "0"
                End If
                chkcash = GVRow.FindControl("chkcash")
                If chkcash.Checked = True Then
                    cash(n) = "1"
                ElseIf chkcash.Checked = "0" Then
                    cash(n) = "0"
                End If
                txtpriority = GVRow.FindControl("txtpriority")
                priority(n) = CType(txtpriority.Text, String)

                n = n + 1
            Next
            fillDategrd(grdsuppdetails, False, grdsuppdetails.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdsuppdetails.Rows
                If n = i Then
                    Exit For
                End If
                txtpartycode = GVRow.FindControl("txtpartycode")
                txtpartycode.Text = partycode(n)
                txtoldpartycode = GVRow.FindControl("txtoldpartycode")
                txtoldpartycode.Text = oldpartycode(n)

                txtpartyname = GVRow.FindControl("txtpartyname")
                txtpartyname.Text = partyname(n)
                chkpreferred = GVRow.FindControl("chkpreferred")
                If prefered(n) = "1" Then
                    chkpreferred.Checked = True
                ElseIf prefered(n) = "0" Then
                    chkpreferred.Checked = False
                End If

                chkcash = GVRow.FindControl("chkcash")

                If cash(n) = "1" Then
                    chkcash.Checked = True
                ElseIf cash(n) = "0" Then
                    chkcash.Checked = False
                End If
                txtpriority = GVRow.FindControl("txtpriority")
                txtpriority.Text = priority(n)
                n = n + 1
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub



    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=excursionsupplier','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

        'Response.Redirect("EccSuppDetailsMaster.aspx", False)
        'Dim strscript As String = ""
        'strscript = "window.opener.__doPostBack('CntryGrpWindowPostBack', '');window.opener.focus();window.close();"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        ' End If
    End Sub

    Private Sub deletedrows()


        Dim dt As New DataTable

        dt.Columns.Add("partycode")
        dt.Columns.Add("partyname")
        dt.Columns.Add("preferred")
        dt.Columns.Add("cash")
        dt.Columns.Add("prioritize")
        dt.Columns.Add("oldpartycode")

        Session("excursionsupplier_deleterow") = dt



    End Sub
    Function CheckSuppliers() As Boolean
        CheckSuppliers = True
        Dim MyAdapter As SqlDataAdapter
        Dim ds As New DataSet
        Dim strMsg As String = ""
        Try
            For Each GvRow In grdsuppdetails.Rows
                Dim txtpartycode As TextBox = GvRow.FindControl("txtpartycode")
                Dim chkSelect As CheckBox = GvRow.FindControl("chksectgrps")
                Dim txtoldpartycode As TextBox = GvRow.FindControl("txtoldpartycode")

                If txtpartycode.Text <> "" And chkSelect.Checked = True Then

                    strSqlQry = "select distinct h.eplistcode , 'SectorCost PriceList' Options from exccplist_header h(nolock), exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and h.partycode='" & txtoldpartycode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "' union all  " _
                                & " select distinct h.eplistcode,' MultiCost Price List' Options from excmulticplist_header h(nolock), excmulticplist_detail d(nolock)  where h.eplistcode=d.eplistcode and d.partycode='" & txtoldpartycode.Text & "' and h.exctypcode='" & txtCustomerCode.Value.Trim & "'"




                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    MyAdapter.Fill(ds, "showsectors")


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then

                                Dim partyname As String = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", CType(txtoldpartycode.Text, String))

                                strMsg = "For this Supplier " + partyname + "  Excursion Already Entered Cost Price list " + "\n"

                                For i = 0 To ds.Tables(0).Rows.Count - 1

                                    strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                                Next

                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                CheckSuppliers = False
                                Exit Function
                            End If
                        End If
                    End If
                End If

            Next



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcsuppdetailsMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
    Protected Sub btndelrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelrow.Click


        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdsuppdetails.Rows.Count + 1
        Dim partycode(count) As String
        Dim partyname(count) As String
        Dim preferred(count) As String
        Dim priority(count) As String
        Dim oldpartycode(count) As String
        Dim cash(count) As String
        Dim n As Integer = 0
        Dim txtpartycode As TextBox
        Dim txtpartyname As TextBox
        Dim chkpreferred As CheckBox
        Dim txtpriority As TextBox
        Dim chkcash As CheckBox
        Dim chkSelect As CheckBox
        Dim txtoldpartycode As TextBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0



        Try

            If CheckSuppliers() = False Then
                Exit Sub
            End If

            Dim dt As DataTable
            dt = Session("excursionsupplier_deleterow")

            For Each GVRow In grdsuppdetails.Rows
                chkSelect = GVRow.FindControl("chksectgrps")
                txtpartycode = GVRow.FindControl("txtpartycode")
                txtpartyname = GVRow.FindControl("txtpartyname")
                chkpreferred = GVRow.FindControl("chkpreferred")
                chkcash = GVRow.FindControl("chkcash")
                txtpriority = GVRow.FindControl("txtpriority")
                txtoldpartycode = GVRow.FindControl("txtoldpartycode")
                If chkSelect.Checked = False Then

                    partycode(n) = CType(txtpartycode.Text, String)
                    oldpartycode(n) = CType(txtoldpartycode.Text, String)

                    partyname(n) = CType(txtpartyname.Text, String)

                    If chkpreferred.Checked = True Then
                        preferred(n) = "1"
                    ElseIf chkpreferred.Checked = False Then
                        preferred(n) = "0"
                    End If

                    If chkcash.Checked = True Then
                        cash(n) = "1"
                    ElseIf chkcash.Checked = "0" Then
                        cash(n) = "0"
                    End If

                    priority(n) = CType(txtpriority.Text, String)
                    n = n + 1
                Else

                    'If (objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "excursiontypes_suppliers", "countries", "exctypcode='" & txtCustomerCode.Value.Trim & "' and partycode='" & txtpartycode.Text & "'") <> "") Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Countrygroup already exists for this supplier.Do You Want to Continue');", True)
                    'End If

                    dt.Rows.Add(txtpartycode.Text, txtpartyname.Text, IIf(chkpreferred.Checked, "1", "0"), IIf(chkcash.Checked, "1", "0"), txtpriority.Text, txtoldpartycode.Text)
                    deletedrow = deletedrow + 1
                End If

            Next

            Session("excursionsupplier_deleterow") = dt


            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(grdsuppdetails, False, count)

            'If grdsuppdetails.Rows.Count > 1 Then
            '    fillDategrd(grdsuppdetails, False, grdsuppdetails.Rows.Count - deletedrow)
            'Else
            '    fillDategrd(grdsuppdetails, False, grdsuppdetails.Rows.Count)
            'End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdsuppdetails.Rows
                If GVRow.RowIndex < count Then
                    txtpartycode = GVRow.FindControl("txtpartycode")
                    txtpartycode.Text = partycode(n)

                    txtoldpartycode = GVRow.FindControl("txtoldpartycode")
                    txtoldpartycode.Text = oldpartycode(n)

                    txtpartyname = GVRow.FindControl("txtpartyname")
                    txtpartyname.Text = partyname(n)
                    chkpreferred = GVRow.FindControl("chkpreferred")
                    If preferred(n) = "1" Then
                        chkpreferred.Checked = True
                    ElseIf preferred(n) = "0" Then
                        chkpreferred.Checked = False
                    End If

                    chkcash = GVRow.FindControl("chkcash")

                    If cash(n) = "1" Then
                        chkcash.Checked = True
                    ElseIf cash(n) = "0" Then
                        chkcash.Checked = False
                    End If

                    txtpriority = GVRow.FindControl("txtpriority")
                    txtpriority.Text = priority(n)
                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub


#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region




    Protected Sub grdsuppdetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdsuppdetails.RowDataBound
        Dim lshidePreferredSupp As String, ChkPreferred As CheckBox, lblPreferred As Label
        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim txtpriority As TextBox = e.Row.FindControl("txtpriority")

            Numberssrvctrl(txtpriority)

            'changed by mohamed on 05/06/2018
            lshidePreferredSupp = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=5201")
            If lshidePreferredSupp.Trim = "1" Then 'And CType(Request.QueryString("appid"), String) = "11"
                ChkPreferred = e.Row.FindControl("ChkPreferred")
                If ChkPreferred IsNot Nothing Then
                    ChkPreferred.Style.Add("display", "none")
                End If
            End If
        ElseIf (e.Row.RowType = DataControlRowType.Header) Then 'changed by mohamed on 05/06/2018
            'changed by mohamed on 05/06/2018
            lblPreferred = e.Row.FindControl("lblPreferred")
            lblPreferred.Style.Add("display", "none")
            If lblPreferred IsNot Nothing Then
                lblPreferred.Style.Add("display", "none")
            End If
        End If

    End Sub
End Class


