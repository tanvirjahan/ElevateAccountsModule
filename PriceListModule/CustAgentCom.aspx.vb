Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class PriceListModule_CustAgentCom
    Inherits System.Web.UI.Page
    Dim objuser As New clsUser
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
    Dim strWhereCond As String

    Dim gvRow As GridViewRow
    Dim lblLineNo As Label
   
    Dim txtFromDate As TextBox
    Dim ImgBtnFromDate As ImageButton
    Dim txtToDate As TextBox
    Dim ImgBtnToDate As ImageButton
    Dim txtPercentage As TextBox
    Dim chkDel As CheckBox

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
                If CType(Session("custState"), String) = "New" Then
                    ' SetFocus(txtCustomerCode)
                    Dim RefCode As String
                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)

                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    btnSave.Text = "Save"
                    btnAddRow.Visible = True
                    btnDeleteRow.Visible = True
                    FilGrid(gv_row, False, True, 1)
                    lblHeading.Text = "Add New Customer - Percentage of Commission"
                    Page.Title = Page.Title + " " + "New Percentage of Commission"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save/update percentage of commission?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "Edit" Then

                    btnSave.Text = "Save/Update"
                    Dim RefCode As String
                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    btnAddRow.Visible = True
                    btnDeleteRow.Visible = True
                    lblHeading.Text = "Edit Customer - Percentage of Commission"
                    Page.Title = Page.Title + " " + "Edit Percentage of Commission"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save/update percentage of commission?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "View" Then
                    Dim RefCode As String
                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    btnSave.Visible = False
                    DisableControlForDelete()
                    lblHeading.Text = "View Customer - Percentage of Commission"
                    Page.Title = Page.Title + " " + "View Percentage of Commission"
                    btnCancel.Focus()
                    btnAddRow.Visible = False
                    btnDeleteRow.Visible = False


                ElseIf CType(Session("custState"), String) = "Delete" Then
                    btnSave.Text = "Delete"
                    Dim RefCode As String
                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    btnAddRow.Visible = False
                    btnDeleteRow.Visible = False
                    DisableControlForDelete()
                    btnCancel.Focus()
                    lblHeading.Text = "Delete Customer - Percentage of Commission"
                    Page.Title = Page.Title + " " + "Delete Customer - Percentage of Commission"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete percentage of commission?')==false)return false;")
                End If
                Session.Add("submenuuser", "CustomersSearch.aspx")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustAgentCom.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
       
    End Sub

    Private Function ValidatePage() As Boolean
        Dim dpFDate As New TextBox
        Dim dpTDate As New TextBox
        Dim ObjDate As New clsDateTime
        Dim flag As Integer
        Try

            flag = 0
            For Each gvRow In gv_row.Rows
                txtFromDate = gvRow.FindControl("txtFromDate")
                txtToDate = gvRow.FindControl("txtToDate")
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                    flag = 1
                    Exit For
                End If
            Next

            If flag = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date cannot be blank..')", True)
                ValidatePage = False
                Exit Function
            End If

            For Each gvRow In gv_row.Rows


                txtFromDate = gvRow.FindControl("txtFromDate")
                txtToDate = gvRow.FindControl("txtToDate")
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                    If ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        '    SetFocus(dpTDate.txtDate)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            Next

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function


    Public Function checkDateOverlapping() As Boolean

        If CType(Session("custState"), String) = "New" Or CType(Session("custState"), String) = "Edit" Then
            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim FromDateText As New TextBox
            Dim ToDateText As New TextBox
            Dim txtPercentage As New TextBox
            Dim ToDate As New Date
            Dim FromDate As New Date
            Dim ToDate1 As New Date
            Dim FromDate1 As New Date
            Dim DateOverLap As New List(Of DateOverLap)
            For i = 0 To gv_row.Rows.Count - 1
                Dim gvrow As GridViewRow = gv_row.Rows(i)
                FromDateText = New TextBox
                FromDateText = CType(gvrow.FindControl("txtFromDate"), TextBox)
                ToDateText = New TextBox
                ToDateText = CType(gvrow.FindControl("txtToDate"), TextBox)
                txtPercentage = New TextBox
                txtPercentage = CType(gvrow.FindControl("txtPercentage"), TextBox)

                If FromDateText.Text = String.Empty Then
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row " & i + 1 & "  FromDate Should Not Be Blank');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('FromDate Should Not Be Blank.');", True)
                    FromDateText.Focus()
                    Return False
                End If

                If ToDateText.Text = String.Empty Then
                    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row " & i + 1 & "  ToDate Should Not Be Blank');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('ToDate Should Not Be Blank');", True)
                    ToDateText.Focus()
                    Return False
                End If

                If txtPercentage.Text = String.Empty Then
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row " & i + 1 & " Percentage Should Not Be Blank');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Percentage Should Not Be Blank');", True)
                    txtPercentage.Focus()
                    Return False
                End If

                FromDate = CType(FromDateText.Text, Date)
                ToDate = CType(ToDateText.Text, Date)



                If ToDate < FromDate Then
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row " & i + 1 & "ToDate Should Not Less Than Or Equal To FromDate of  " & i + 1 & " Row');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                    Return False

                End If


                Dim DateOverLapAdd As New DateOverLap
                DateOverLapAdd.FromDate = FromDate
                DateOverLapAdd.ToDate = ToDate
                DateOverLapAdd.LineNo = i
                DateOverLap.Add(DateOverLapAdd)
                'For j = 0 To gv_row.Rows.Count - 1
                '    If j <> i And j > i Then
                '        Dim gvrow1 As GridViewRow = gv_row.Rows(j)
                '        FromDateText = New TextBox
                '        FromDateText = CType(gvrow1.FindControl("txtFromDate"), TextBox)
                '        ToDateText = New TextBox
                '        ToDateText = CType(gvrow1.FindControl("txtToDate"), TextBox)
                '        txtPercentage = New TextBox
                '        txtPercentage = CType(gvrow.FindControl("txtPercentage"), TextBox)
                '        If FromDateText.Text = String.Empty Then
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row  " & j + 1 & " FromDate Should Not Be Blank');", True)
                '            FromDateText.Focus()
                '            Return False
                '        End If

                '        If ToDateText.Text = String.Empty Then
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row  " & j + 1 & " ToDate Should Not Be Blank');", True)
                '            ToDateText.Focus()
                '            Return False
                '        End If

                '        If txtPercentage.Text = String.Empty Then
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row  " & j + 1 & " Percentage Should Not Be Blank');", True)
                '            txtPercentage.Focus()
                '            Return False
                '        End If



                '        FromDate1 = FromDateText.Text
                '        ToDate1 = ToDateText.Text
                '        If FromDate1 <= ToDate Then
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row " & j + 1 & "FromDate Should  Not be Less Than or equal To ToDate of  " & i + 1 & "  Row');", True)
                '            Return False
                '        End If
                '        If ToDate1 <= ToDate Then
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row  " & j + 1 & "ToDate Should Not be Less Than or equal to ToDate of  " & i + 1 & "  Row');", True)
                '            Return False
                '        End If
                '    End If
                'Next

            Next

            If DateOverLap.Count > 0 Then
                DateOverLap.Sort(AddressOf CompareByFromDate)
                For i = 0 To DateOverLap.Count - 1
                    For j = 0 To DateOverLap.Count - 1
                        If j <> i And j > i Then
                            If DateOverLap.Item(j).FromDate <= DateOverLap.Item(i).ToDate Then
                                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row " & j + 1 & "FromDate Should  Not be Less Than or equal To ToDate of  " & i + 1 & "  Row');", True)
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Dates Overlapping!');", True)
                                Return False
                            End If
                            If DateOverLap.Item(j).ToDate <= DateOverLap.Item(i).ToDate Then
                                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Row  " & j + 1 & "ToDate Should Not be Less Than or equal to ToDate of  " & i + 1 & "  Row');", True)
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Dates Overlapping!');", True)
                                Return False
                            End If


                        End If
                    Next
                Next
            End If

        End If
        checkDateOverlapping = True
    End Function
    Function CompareByFromDate(ByVal DateOverLapClass As DateOverLap, ByVal DateOverLapClass1 As DateOverLap) As Integer
        Return DateOverLapClass.FromDate.CompareTo(DateOverLapClass1.FromDate)
    End Function
    Private Sub DisableControl()
        For Each gvRow In gv_row.Rows
            lblLineNo = gvRow.FindControl("lblLineNo")
            txtFromDate = gvRow.FindControl("txtFromDate")
            ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
            txtToDate = gvRow.FindControl("txtToDate")
            ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
            txtPercentage = gvRow.FindControl("txtPercentage")
            chkDel = gvRow.FindControl("chkDel")

            txtFromDate.Enabled = False
            ImgBtnFromDate.Enabled = False
            txtToDate.Enabled = False
            ImgBtnToDate.Enabled = False
            'chkDel.Enabled = False
            txtPercentage.Enabled = False
        Next
       
    End Sub

    Private Sub DisableControlForDelete()
        For Each gvRow In gv_row.Rows
            lblLineNo = gvRow.FindControl("lblLineNo")
            txtFromDate = gvRow.FindControl("txtFromDate")
            ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
            txtToDate = gvRow.FindControl("txtToDate")
            ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
            txtPercentage = gvRow.FindControl("txtPercentage")
            chkDel = gvRow.FindControl("chkDel")

            txtFromDate.Enabled = False
            ImgBtnFromDate.Enabled = False
            txtToDate.Enabled = False
            ImgBtnToDate.Enabled = False
            chkDel.Enabled = False
            txtPercentage.Enabled = False
        Next

    End Sub
    Public Sub FilGrid(ByVal grd As GridView, ByVal showrecord As Boolean, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If
        If (showrecord) Then
            grd.DataSource = CreateDataSource()
            If CreateDataSource().Count = 0 Then
                grd.DataSource = CreateDataSource(lngcnt)
                grd.DataBind()
                If lngcnt = 0 Then
                    lblMsg.Visible = True
                    btnSave.Enabled = False
                End If
                grd.Visible = True
                Exit Sub
            End If
        Else
            grd.DataSource = CreateDataSource(lngcnt)

        End If
        If CreateDataSource().Count > 0 Or CreateDataSource(lngcnt).Count > 0 Then
            'grd.DataBind()
            'grd.Visible = True
            lblMsg.Visible = False
        Else
            lblMsg.Visible = True
        End If
        grd.DataBind()
        grd.Visible = True
        'txtgridrows.Value = grd.Rows.Count
    End Sub

    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("agentcommissioncode", GetType(Integer)))
       

        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function


    Private Function CreateDataSource() As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("agentcommissioncode", GetType(Integer)))
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        mySqlCmd = New SqlCommand("Select agentcommissioncode from agentcommission Where agentcode='" & CType(Session("custrefcode"), String) & "'", mySqlConn)
        myDataAdapter = New SqlDataAdapter(mySqlCmd)
        myDataAdapter.Fill(dt)

        CreateDataSource = New DataView(dt)

    End Function
    Protected Sub gv_row_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_row.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Set Attribut For Percentage
            gvRow = e.Row
            txtFromDate = gvRow.FindControl("txtFromDate")
            txtToDate = gvRow.FindControl("txtToDate")
            txtPercentage = gvRow.FindControl("txtPercentage")

            txtPercentage.Attributes.Add("onblur", "ToValidatePercentage('" + txtPercentage.ClientID + "','" + txtFromDate.ClientID + "','" + txtToDate.ClientID + "')")
            txtPercentage.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")

        End If
        

    End Sub

    Protected Sub gv_row_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_row.RowCommand
       


    End Sub
    

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

            If CType(Session("custState"), String) = "Edit" Or CType(Session("custState"), String) = "View" Or CType(Session("custState"), String) = "Delete" Then

                Dim lngCnt As Long
                lngCnt = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentcommission", "count(agentcode)", "agentcode", RefCode), Long)

                If CType(Session("custState"), String) = "Edit" Then
                    If lngCnt = 0 Then lngCnt = 1

                    FilGrid(gv_row, True, False, lngCnt)
                Else
                    If lngCnt = 0 Then lngCnt = 0

                    FilGrid(gv_row, True, False, lngCnt)
                End If
                

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                mySqlCmd = New SqlCommand("Select * from agentcommission Where agentcode='" & RefCode & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    While mySqlReader.Read()
                        For Each gvRow In gv_row.Rows
                            lblLineNo = gvRow.FindControl("lblLineNo")
                            txtFromDate = gvRow.FindControl("txtFromDate")
                            ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
                            txtToDate = gvRow.FindControl("txtToDate")
                            ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
                            txtPercentage = gvRow.FindControl("txtPercentage")
                            If mySqlReader("agentcommissioncode") = CType(lblLineNo.Text, Integer) Then

                                If IsDBNull(mySqlReader("fromdate")) = False Then
                                    txtFromDate.Text = CType(mySqlReader("fromdate"), String)
                                Else
                                    txtFromDate.Text = ""
                                End If

                                If IsDBNull(mySqlReader("todate")) = False Then
                                    txtToDate.Text = CType(mySqlReader("todate"), String)
                                Else
                                    txtToDate.Text = ""
                                End If

                                If IsDBNull(mySqlReader("percentage")) = False Then
                                    txtPercentage.Text = CType(mySqlReader("percentage"), Double).ToString("N")
                                Else
                                    txtPercentage.Text = ""
                                End If
                            End If
                        Next
                    End While
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustAgentCom.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub

    Protected Sub btnAddRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRow.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = gv_row.Rows.Count + 1
        Dim lineno(count) As String
        Dim fromdate(count) As String
        Dim imgfromdate(count) As String
        Dim todate(count) As String
        Dim imgtodate(count) As String
        Dim percentage(count) As String


        For Each gvRow In gv_row.Rows
            lblLineNo = gvRow.FindControl("lblLineNo")
            chkDel = gvRow.FindControl("chkDel")
            txtFromDate = gvRow.FindControl("txtFromDate")
            ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
            txtToDate = gvRow.FindControl("txtToDate")
            ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
            txtPercentage = gvRow.FindControl("txtPercentage")



            lineno(n) = lblLineNo.Text
            fromdate(n) = txtFromDate.Text
            todate(n) = txtToDate.Text
            percentage(n) = txtPercentage.Text
            n = n + 1
        Next
        FilGrid(gv_row, False, False, gv_row.Rows.Count + 1)

        Dim i As Integer = n
        n = 0

        For Each gvRow In gv_row.Rows
            If n = i Then
                Exit For
            End If
            lblLineNo = gvRow.FindControl("lblLineNo")
            chkDel = gvRow.FindControl("chkDel")
            txtFromDate = gvRow.FindControl("txtFromDate")
            ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
            txtToDate = gvRow.FindControl("txtToDate")
            ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
            txtPercentage = gvRow.FindControl("txtPercentage")
           
            lineno(n) = lblLineNo.Text
            txtFromDate.Text = fromdate(n)
            txtToDate.Text = todate(n)
            txtPercentage.Text = percentage(n)
            n = n + 1
        Next
    End Sub

    Protected Sub btnDeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRow.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = gv_row.Rows.Count + 1
        Dim lineno(count) As String
        Dim fromdate(count) As String
        Dim imgfromdate(count) As String
        Dim todate(count) As String
        Dim imgtodate(count) As String
        Dim percentage(count) As String

        For Each gvRow In gv_row.Rows
            chkDel = gvRow.FindControl("chkDel")
            If chkDel.Checked = False Then
                lblLineNo = gvRow.FindControl("lblLineNo")
                chkDel = gvRow.FindControl("chkDel")
                txtFromDate = gvRow.FindControl("txtFromDate")
                ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
                txtToDate = gvRow.FindControl("txtToDate")
                ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
                txtPercentage = gvRow.FindControl("txtPercentage")


                lineno(n) = lblLineNo.Text
                fromdate(n) = txtFromDate.Text
                todate(n) = txtToDate.Text
                percentage(n) = txtPercentage.Text
                n = n + 1
            End If
        Next

        Dim ct As Integer
        ct = n
        If n = 0 Then
            ct = 0
        End If
        If ct = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Minimum one row should be there to Save or update !');", True)
            Exit Sub
        Else
            FilGrid(gv_row, False, False, ct)
        End If

        Dim i As Integer = n
        n = 0

        For Each gvRow In gv_row.Rows
            If n = i Then
                Exit For
            End If

            lblLineNo = gvRow.FindControl("lblLineNo")
            chkDel = gvRow.FindControl("chkDel")
            txtFromDate = gvRow.FindControl("txtFromDate")
            ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
            txtToDate = gvRow.FindControl("txtToDate")
            ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
            txtPercentage = gvRow.FindControl("txtPercentage")


            lineno(n) = lblLineNo.Text
            txtFromDate.Text = fromdate(n)
            txtToDate.Text = todate(n)
            txtPercentage.Text = percentage(n)
            n = n + 1
        Next
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid = True Then
            Try
                If CType(Session("custState"), String) = "Edit" Or CType(Session("custState"), String) = "New" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkDateOverlapping() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnection           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentcommision", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(Session("custrefcode"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    Dim UniqueID As Integer = 0
                    UniqueID = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select max(agentcommissioncode)+1 as Code from agentcommission(nolock)")
                    For Each gvRow In gv_row.Rows
                        UniqueID = UniqueID + 1
                        lblLineNo = gvRow.FindControl("lblLineNo")

                        txtFromDate = gvRow.FindControl("txtFromDate")
                        ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
                        txtToDate = gvRow.FindControl("txtToDate")
                        ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
                        txtPercentage = gvRow.FindControl("txtPercentage")

                        mySqlCmd = New SqlCommand("sp_add_agentcommision", mySqlConn, sqlTrans)
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcommissioncode", SqlDbType.BigInt)).Value = UniqueID
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text.Trim), Date)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime, 100)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text.Trim), Date)
                        mySqlCmd.Parameters.Add(New SqlParameter("@percentage", SqlDbType.Decimal)).Value = CType(txtPercentage.Text.Trim, Decimal)
                        mySqlCmd.ExecuteNonQuery()
                    Next
                    sqlTrans.Commit()
                    sqlTrans.Dispose()
                    mySqlConn.Close()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully !');", True)


                ElseIf CType(Session("custState"), String) = "New" Then

                    mySqlConn = clsDBConnect.dbConnection           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim UniqueID As Integer = 0
                    UniqueID = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select max(agentcommissioncode)+1 as Code from agentcommission(nolock)")
                    Dim Id As Integer = 0
                    For Each gvRow In gv_row.Rows
                        UniqueID = UniqueID + 1
                        lblLineNo = gvRow.FindControl("lblLineNo")

                        txtFromDate = gvRow.FindControl("txtFromDate")
                        ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
                        txtToDate = gvRow.FindControl("txtToDate")
                        ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
                        txtPercentage = gvRow.FindControl("txtPercentage")

                        mySqlCmd = New SqlCommand("sp_add_agentcommision", mySqlConn, sqlTrans)
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcommissioncode", SqlDbType.BigInt)).Value = UniqueID
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text.Trim), Date)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime, 100)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text.Trim), Date)
                        mySqlCmd.Parameters.Add(New SqlParameter("@percentage", SqlDbType.Decimal)).Value = CType(txtPercentage.Text.Trim, Decimal)
                        mySqlCmd.ExecuteNonQuery()
                    Next
                    sqlTrans.Commit()
                    sqlTrans.Dispose()
                    mySqlConn.Close()

                    Dim strscript As String = ""
                    strscript = "window.opener.location.href=window.opener.location.href;window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                ElseIf CType(Session("custState"), String) = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnection           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_agentcommision", mySqlConn, sqlTrans)
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(Session("custrefcode"), String)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.ExecuteNonQuery()

                    'To insert into header detail
                    ''Dim UniqueID As Integer = 0
                    ''UniqueID = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select max(agentcommissioncode)+1 as Code from agentcommission")
                    ''For Each gvRow In gv_row.Rows

                    ''    lblLineNo = gvRow.FindControl("lblLineNo")

                    ''    txtFromDate = gvRow.FindControl("txtFromDate")
                    ''    ImgBtnFromDate = gvRow.FindControl("ImgBtnFromDate")
                    ''    txtToDate = gvRow.FindControl("txtToDate")
                    ''    ImgBtnToDate = gvRow.FindControl("ImgBtnToDate")
                    ''    txtPercentage = gvRow.FindControl("txtPercentage")

                    ''    mySqlCmd = New SqlCommand("sp_add_agentcommision", mySqlConn, sqlTrans)
                    ''    mySqlCmd.Parameters.Add(New SqlParameter("@agentcommissioncode", SqlDbType.BigInt)).Value = UniqueID
                    ''    mySqlCmd.CommandType = CommandType.StoredProcedure
                    ''    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    ''    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text.Trim), Date)
                    ''    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime, 100)).Value = CType(ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text.Trim), Date)
                    ''    mySqlCmd.Parameters.Add(New SqlParameter("@percentage", SqlDbType.Decimal)).Value = CType(txtPercentage.Text.Trim, Decimal)
                    ''    mySqlCmd.ExecuteNonQuery()
                    ''Next
                    sqlTrans.Commit()
                    sqlTrans.Dispose()
                    mySqlConn.Close()
                    Dim strscript As String = ""
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Deleted Successfully !');", True)
                End If

            Catch ex As Exception
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustAgentCom.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
       
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Response.Redirect("CustomersSearch.aspx")
        If Session("postback") <> "Addclient" Then
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Else

            Dim strscript As String = ""
            strscript = "window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


        End If
    End Sub
End Class

Public Class DateOverLap

    Dim _FromDate As Date
    Dim _ToDate As Date
    Dim _LineNo As Integer
    Public Property FromDate() As Date
        Get
            Return _FromDate
        End Get
        Set(ByVal value As Date)
            _FromDate = value
        End Set
    End Property
    Public Property ToDate() As Date
        Get
            Return _ToDate
        End Get
        Set(ByVal value As Date)
            _ToDate = value
        End Set
    End Property
    Public Property LineNo() As Integer
        Get
            Return _LineNo
        End Get
        Set(ByVal value As Integer)
            _LineNo = value
        End Set
    End Property


    

End Class