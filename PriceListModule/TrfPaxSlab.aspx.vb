
#Region "namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports ColServices

#End Region

Partial Class PriceListModule_TrfPaxSlab
    Inherits System.Web.UI.Page

    Dim mySqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim mySqlAdapter As SqlDataAdapter
    Dim sqlTrans As SqlTransaction
    Dim objUtils As New clsUtils
    Dim sqlReader As SqlDataReader

    Private dtPaxSlab As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                Session("PaxSlabFilter") = Request.Params("Type")

                Dim strentryQuery As String = "select 1  from othgrpmast where othgrpcode =(select option_selected  from reservation_parameters where param_id=" + Session("PaxSlabFilter") + ")" & _
                    " and paxcalcreqd=1"
                Dim strVal As String = ""
                sqlReader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), strentryQuery)
                If sqlReader.HasRows = True Then

                    ViewState.Add("TrfPaxSlabState", Request.QueryString("State"))
                    ViewState.Add("TrfPaxSlabRefCode", Request.QueryString("RefCode"))

                    If Session("PaxSlabFilter") = "1001" Then
                        Page.Title = Page.Title + " " + "Transfer Pax Slab"
                        lblHeading.Text = "Transfer Pax Slab"
                    Else
                        Page.Title = Page.Title + " " + "Jeeb Ride Pax Slab"
                        lblHeading.Text = "Jeeb Ride Pax Slab"
                    End If


                    Dim strGrpCode As String = ""
                    strGrpCode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("PaxSlabFilter"))
                    hdnGrpCode.Value = strGrpCode

                    dtPaxSlab.Columns.Add(New DataColumn("paxslab", GetType(Integer)))
                    dtPaxSlab.Columns.Add(New DataColumn("VehicleType", GetType(String)))
                    dtPaxSlab.Columns.Add(New DataColumn("Unit", GetType(Integer)))
                    dtPaxSlab.Columns.Add(New DataColumn("ExistsEntry", GetType(Integer)))
                    Session("GRD_paxSlab") = dtPaxSlab
                    FillGrid()

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save  Pax Slab?')==false)return false;")
                    btnReturn.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                Else

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Not Applicable');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                End If

            Catch ex As Exception

            End Try
        End If


    End Sub

    Private Sub FillGrid()
        Dim count As Integer
        Dim strQuery As StringBuilder
        Dim ddlVT As DropDownList
        Dim txtUnt As TextBox
        Dim grdRow As GridViewRow
        Dim dtRow() As DataRow
        Dim intpaxNo As Integer

        Dim tempDt As DataTable = New DataTable()
        dtPaxSlab = Session("GRD_paxSlab")


        strQuery = New StringBuilder
        'strQuery.Append("select COUNT(slabno) from othcat_slabs")
        strQuery.AppendFormat("select COUNT(slabno) from othcat_slabs where othgrpcode =(select option_selected  from reservation_parameters where param_id ='{0}')", Session("PaxSlabFilter"))

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd = New SqlCommand(strQuery.ToString, mySqlConn)
        count = mySqlCmd.ExecuteScalar
        mySqlConn.Close()

        If count > 0 Then
            strQuery.Remove(0, strQuery.Length)

            'strQuery.Append("select slabno as paxslab,othcatcode as VehicleType  from othcat_slabs")
            strQuery.AppendFormat("select slabno as paxslab,othcatcode as VehicleType ,Unit   from othcat_slabs where othgrpcode =(select option_selected  from reservation_parameters where param_id ='{0}')", Session("PaxSlabFilter"))
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            mySqlAdapter = New SqlDataAdapter(strQuery.ToString, mySqlConn)
            mySqlAdapter.Fill(tempDt)

            If tempDt.Rows.Count > 0 Then
                For Each dtRow1 In tempDt.Rows
                    intpaxNo = dtRow1(0)
                    'getting tble name according to the page filter 'trfs' or 'jeepwadi'
                    Dim strTblName As String = ""
                    If Session("PaxSlabFilter") = "1001" Then
                        strTblName = "trfplist_othcat_slabs"
                    Else
                        strTblName = "othplist_othcat_slabs"
                    End If

                    'if 'paxno' exists in selling pax table, disable or cannot delete
                    If (objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), strTblName, "slabno", intpaxNo.ToString) = True) Then
                        dtPaxSlab.Rows.Add(intpaxNo, dtRow1(1), dtRow1(2), 1)
                    Else
                        dtPaxSlab.Rows.Add(intpaxNo, dtRow1(1), dtRow1(2), 0)
                    End If
                    'select * from trfplist_othcat_slabs where slabno =1
                Next
            End If

            If dtPaxSlab.Rows.Count > 0 Then
                grdPaxSlab.DataSource = dtPaxSlab
                grdPaxSlab.DataBind()

                For Each grdRow In grdPaxSlab.Rows
                    intpaxNo = grdRow.Cells(0).Text
                    dtRow = dtPaxSlab.Select(String.Format("paxslab={0}", intpaxNo))
                    ddlVT = grdRow.FindControl("ddlVehicleType")
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlVT, "othcatcode", "select othcatcode  from othcatmast where active=1 and othgrpcode ='" & hdnGrpCode.Value & "'  order by othcatcode", True)
                    If (dtRow(0).Item("VehicleType") <> "") Then
                        ddlVT.SelectedValue = dtRow(0).Item("VehicleType")
                    End If
                    txtUnt = grdRow.FindControl("txtUnit")
                    If (dtRow(0).Item("Unit").ToString <> "") Then
                        txtUnt.Text = dtRow(0).Item("Unit")
                    End If
                    If (dtRow(0).Item("ExistsEntry") = 1) Then
                        grdRow.Enabled = False
                    End If
                Next


            End If

        ElseIf count <= 0 Then
            dtPaxSlab.Rows.Add(1, " ", 1, 0)
            'dtPaxSlab
            If dtPaxSlab.Rows.Count > 0 Then
                grdPaxSlab.DataSource = dtPaxSlab
                grdPaxSlab.DataBind()

                For Each grdRow In grdPaxSlab.Rows
                    ddlVT = grdRow.FindControl("ddlVehicleType")
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlVT, "othcatcode", "select othcatcode  from othcatmast where active=1 and othgrpcode ='" & hdnGrpCode.Value & "'  order by othcatcode", True)
                    txtUnt = grdRow.FindControl("txtUnit")

                    txtUnt.Text = "0"

                Next
            End If
        End If

        Session("GRD_paxSlab") = dtPaxSlab


    End Sub

    Protected Sub btnAddLines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLines.Click
        'dtPaxSlab = Session("GRD_paxSlab")
        Dim count As Integer
        Dim ddlVT As DropDownList
        Dim grdRow As GridViewRow
        Dim dtRow() As DataRow
        Dim intpaxNo As Integer
        Dim strVT As String
        Dim intentryexsts As Integer
        Dim txtUnt As TextBox
        Dim intUnit As Integer
        dtPaxSlab = Session("GRD_paxSlab")
        If (dtPaxSlab.Rows.Count > 0) Then
            dtPaxSlab.Clear()

        End If
        'copying values selected to the datattable
        If grdPaxSlab.Rows.Count > 0 Then
            For Each grdRow In grdPaxSlab.Rows
                intpaxNo = grdRow.Cells(0).Text
                ddlVT = grdRow.FindControl("ddlVehicleType")
                strVT = ddlVT.SelectedValue
                txtUnt = grdRow.FindControl("txtUnit")
                intUnit = txtUnt.Text

                'getting tble name according to the page filter 'trfs' or 'jeepwadi'
                Dim strTblName As String = ""
                If Session("PaxSlabFilter") = "1001" Then
                    strTblName = "trfplist_othcat_slabs"
                Else
                    strTblName = "othplist_othcat_slabs"
                End If

                If (objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), strTblName, "slabno", intpaxNo.ToString) = True) Then
                    intentryexsts = 1
                Else
                    intentryexsts = 0
                End If

                If dtPaxSlab Is Nothing = False Then
                    dtPaxSlab.Rows.Add(intpaxNo, strVT, intUnit, intentryexsts)

                End If
            Next
            'adding new row to datatable
            count = dtPaxSlab.Rows.Count
            dtPaxSlab.Rows.Add(count + 1, "", 1, 0)
            '------ final dt

            'binding this dt wt grid, and assigning back previous values and row with new value
            If dtPaxSlab.Rows.Count > 0 Then
                grdPaxSlab.DataSource = dtPaxSlab
                grdPaxSlab.DataBind()

                For Each grdRow In grdPaxSlab.Rows
                    intpaxNo = grdRow.Cells(0).Text
                    dtRow = dtPaxSlab.Select(String.Format("paxslab={0}", intpaxNo))
                    ddlVT = grdRow.FindControl("ddlVehicleType")
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlVT, "othcatcode", "select othcatcode  from othcatmast where active=1 and othgrpcode ='" & hdnGrpCode.Value & "'  order by othcatcode", True)
                    If (dtRow(0).Item("VehicleType") <> "") Then
                        ddlVT.SelectedValue = dtRow(0).Item("VehicleType")
                    End If
                    txtUnt = grdRow.FindControl("txtUnit")
                    If (dtRow(0).Item("Unit").ToString <> "") Then
                        txtUnt.Text = dtRow(0).Item("Unit")
                    End If
                    If (dtRow(0).Item("ExistsEntry") = 1) Then
                        grdRow.Enabled = False
                    End If
                Next
            End If
            'if grd containd=s no row (not necessary)
        ElseIf count <= 0 Then
            dtPaxSlab.Rows.Add(1, " ", 0, 0)
            'dtPaxSlab
            If dtPaxSlab.Rows.Count > 0 Then
                grdPaxSlab.DataSource = dtPaxSlab
                grdPaxSlab.DataBind()

                For Each grdRow In grdPaxSlab.Rows
                    ddlVT = grdRow.FindControl("ddlVehicleType")
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlVT, "othcatcode", "select othcatcode  from othcatmast where active=1 and othgrpcode ='" & hdnGrpCode.Value & "'  order by othcatcode", True)
                    txtUnt = grdRow.FindControl("txtUnit")

                    txtUnt.Text = "1"
                Next
            End If

        End If
        Session("GRD_paxSlab") = dtPaxSlab
    End Sub
    Private Function ValidatePage() As Boolean
        ValidatePage = True

        Dim ddlVT As DropDownList
        Dim grdRow As GridViewRow
        Dim txtUnt As TextBox
        'Dim intUnit As Integer
        For Each grdRow In grdPaxSlab.Rows
            ddlVT = grdRow.FindControl("ddlVehicleType")
            If CType(ddlVT.SelectedValue, String) = "[Select]" Then
                ValidatePage = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Vehicle Type needs to be selected');", True)
                Exit Function
            End If
            txtUnt = grdRow.FindControl("txtUnit")
            If CType(txtUnt.Text, String) = "" Then
                ValidatePage = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Unit cannot be Blank');", True)
                Exit Function
            End If
            txtUnt = grdRow.FindControl("txtUnit")
            If CType(txtUnt.Text, Integer) < 1 Then
                ValidatePage = False
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Unit cannot be less than 1');", True)
                Exit Function
            End If

        Next
    End Function


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ddlVT As DropDownList
        Dim grdRow As GridViewRow
        Dim txtUnt As TextBox
        Dim intpaxNo As Integer
        Dim strVT As String
        Dim intUnit As Integer

        Try

            If Page.IsValid = True Then
                If ValidatePage() = False Then
                    Exit Sub
                End If

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand("sp_del_othcat_slabs", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = hdnGrpCode.Value
                mySqlCmd.ExecuteNonQuery()

                If grdPaxSlab.Rows.Count > 0 Then
                    For Each grdRow In grdPaxSlab.Rows

                        intpaxNo = grdRow.Cells(0).Text
                        ddlVT = grdRow.FindControl("ddlVehicleType")
                        strVT = ddlVT.SelectedValue
                        txtUnt = grdRow.FindControl("txtUnit")
                        intUnit = txtUnt.Text

                        mySqlCmd = New SqlCommand("sp_addModOthCatSlabs", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@slabno", SqlDbType.VarChar, 20)).Value = intpaxNo
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = hdnGrpCode.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = strVT
                        mySqlCmd.Parameters.Add(New SqlParameter("@Unit", SqlDbType.Int)).Value = intUnit
                        mySqlCmd.ExecuteNonQuery()

                    Next
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)

                'Response.Redirect("~\MainPage.aspx")
                'TrfPaxSlabWindowPostBack
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('TrfPaxSlabWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("TrfPaxSlab.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub

    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        'Response.Redirect("~\MainPage.aspx")
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('TrfPaxSlabWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnDeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRow.Click
        Dim count As Integer
        Dim ddlVT As DropDownList
        Dim grdRow As GridViewRow
        Dim dtRow() As DataRow
        Dim intpaxNo As Integer
        Dim txtUnt As TextBox
        Dim strVT As String
        Dim intentryexsts As Integer
        Dim intUnit As Integer
        dtPaxSlab = Session("GRD_paxSlab")

        '****************************************************
        If (dtPaxSlab.Rows.Count > 0) Then
            dtPaxSlab.Clear()

        End If
        'copying values selected to the datattable
        If grdPaxSlab.Rows.Count > 0 Then
            For Each grdRow In grdPaxSlab.Rows
                intpaxNo = grdRow.Cells(0).Text
                ddlVT = grdRow.FindControl("ddlVehicleType")
                strVT = ddlVT.SelectedValue
                txtUnt = grdRow.FindControl("txtUnit")
                intUnit = txtUnt.Text

                'getting tble name according to the page filter 'trfs' or 'jeepwadi'
                Dim strTblName As String = ""
                If Session("PaxSlabFilter") = "1001" Then
                    strTblName = "trfplist_othcat_slabs"
                Else
                    strTblName = "othplist_othcat_slabs"
                End If

                If (objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), strTblName, "slabno", intpaxNo.ToString) = True) Then
                    intentryexsts = 1
                Else
                    intentryexsts = 0
                End If

                If dtPaxSlab Is Nothing = False Then
                    dtPaxSlab.Rows.Add(intpaxNo, strVT, intUnit, intentryexsts)

                End If
            Next
        End If

        '*****************************************************

        count = grdPaxSlab.Rows.Count
        If count > 0 Then
            If (dtPaxSlab.Rows.Count > 0) Then
                If dtPaxSlab.Rows(count - 1).Item("ExistsEntry") <> 1 Then
                    dtPaxSlab.Rows(count - 1).Delete()
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot delete the last row');", True)
                    Exit Sub
                End If
            End If

            If dtPaxSlab.Rows.Count > 0 Then
                grdPaxSlab.DataSource = dtPaxSlab
                grdPaxSlab.DataBind()

                For Each grdRow In grdPaxSlab.Rows
                    intpaxNo = grdRow.Cells(0).Text
                    dtRow = dtPaxSlab.Select(String.Format("paxslab={0}", intpaxNo))
                    ddlVT = grdRow.FindControl("ddlVehicleType")
                    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlVT, "othcatcode", "select othcatcode  from othcatmast where active=1 and othgrpcode ='" & hdnGrpCode.Value & "'  order by othcatcode", True)
                    If (dtRow(0).Item("VehicleType") <> "") Then
                        ddlVT.SelectedValue = dtRow(0).Item("VehicleType")
                    End If
                    txtUnt = grdRow.FindControl("txtUnit")
                    If (dtRow(0).Item("Unit").ToString <> "") Then
                        txtUnt.Text = dtRow(0).Item("Unit")
                    End If

                    If (dtRow(0).Item("ExistsEntry") = 1) Then
                        grdRow.Enabled = False
                    End If
                Next
            End If

        End If

    End Sub
End Class
