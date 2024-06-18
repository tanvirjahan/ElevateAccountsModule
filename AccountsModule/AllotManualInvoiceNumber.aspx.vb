 

Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic


Partial Class AllotManualInvoiceNumber
    Inherits System.Web.UI.Page
 
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
#End Region
#Region "Web Service Methods"
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetRequestId(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim reqids As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select requestid from booking_header(nolock) where div_code='" & contextKey.Trim & "' and requestid like  '%" & prefixText & "%' order by requestid"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    reqids.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("requestid").ToString(), myDS.Tables(0).Rows(i)("requestid").ToString()))
                Next
            End If

            Return reqids
        Catch ex As Exception
            Return reqids
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agentnames As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select distinct ltrim(rtrim(agentname)) agentname from booking_header h(nolock) inner join agentmast a(nolock) on h.agentcode=a.agentcode where div_code='" & contextKey.Trim & "' and a.agentname like '%" & prefixText & "%' order by agentname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    agentnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentname").ToString()))
                Next
            End If

            Return agentnames
        Catch ex As Exception
            Return agentnames
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentRef(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agentref As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select distinct ltrim(rtrim(agentref)) as agentref from booking_header(nolock) where div_code='" & contextKey.Trim & "' and isnull(agentref,'')<>'' and agentref like '%" & prefixText & "%' order by agentref"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    agentref.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentref").ToString(), myDS.Tables(0).Rows(i)("agentref").ToString()))
                Next
            End If

            Return agentref
        Catch ex As Exception
            Return agentref
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetguestNames(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim gname As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select (case when isnull(g.title,'')<>'' then g.title else '' end + case when isnull(g.firstname,'')<>'' then case when isnull(g.title,'')<>'' then ' ' else '' end + " &
            "g.firstname else '' end + case when ISNULL(g.middlename,'')<>'' then ' ' + g.middlename else '' end + " &
            "case when isnull(g.lastname,'')<>'' then ' '+ g.lastname else '' end) as guestname  from booking_header h " &
            "inner join booking_guest g on h.requestid=g.requestid where div_code='" & contextKey.Trim & "' and g.guestlineno=1 and " &
            "(case when isnull(g.title,'')<>'' then g.title else '' end + case when isnull(g.firstname,'')<>'' then case when isnull(g.title,'')<>'' then ' ' else '' end + " &
            "g.firstname else '' end + case when ISNULL(g.middlename,'')<>'' then ' ' + g.middlename else '' end + " &
            "case when isnull(g.lastname,'')<>'' then ' '+ g.lastname else '' end) like '%" & prefixText & "%' order by guestname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    gname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("guestname").ToString(), myDS.Tables(0).Rows(i)("guestname").ToString()))
                Next
            End If

            Return gname
        Catch ex As Exception
            Return gname
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetSuppliers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliername As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select distinct partyname from view_common_booking_all where div_code='" & contextKey.Trim & "' and partyname like '%" & prefixText & "%' order by partyname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliername.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partyname").ToString()))
                Next
            End If

            Return suppliername
        Catch ex As Exception
            Return suppliername
        End Try
    End Function

#End Region


#Region "Enum GridCol"
    Enum GridCol
        requestid = 0
        requestDate = 1
        agentname = 2
        agentref = 3
        hotelref = 4
        hotelname = 5
        guestname = 6
        addDate = 7
        addUser = 8
        modDate = 9
        modUser = 10
        edit = 11
        print = 12
    End Enum
#End Region
#Region "Protected Function BuildCondition() As String"
    Protected Function BuildCondition() As String
        Try
            Dim strWhereCond As String = ""
            'If Convert.ToString(Session("SalesInvoiceState")) <> "Amend" Then
            '    strWhereCond = "isnull(invno,'')='' "
            'End If
            If txtBookingNo.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "RequestId like '%" & txtBookingNo.Text.Trim.ToUpper & "%'"
                Else
                    strWhereCond = strWhereCond & "and RequestId like '%" & txtBookingNo.Text.Trim.ToUpper & "%'"
                End If
            End If
            If txtCustName.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "agentName like '%" & txtCustName.Text.Trim & "%'"
                Else
                    strWhereCond = strWhereCond & "and agentName like '%" & txtCustName.Text.Trim & "%'"
                End If
            End If
            If txtSupName.Text.Trim() <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "requestid in (select distinct requestid from view_common_booking_all where partyName like '%" & txtSupName.Text.Trim & "%')"
                Else
                    strWhereCond = strWhereCond & "and requestid in (select distinct requestid from view_common_booking_all where partyName like '%" & txtSupName.Text.Trim & "%')"
                End If
            End If
            If txtGuestName.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "guestName like '%" & txtGuestName.Text.Trim & "%'"
                Else
                    strWhereCond = strWhereCond & "and guestName like '%" & txtGuestName.Text.Trim & "%'"
                End If
            End If
            If txtCustRef.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "agentref like '%" & txtCustRef.Text.Trim & "%'"
                Else
                    strWhereCond = strWhereCond & "and agentref like '%" & txtCustRef.Text.Trim & "%'"
                End If
            End If
            If ddlBookingType.SelectedValue.Trim <> "All" Then
                If strWhereCond = "" Then
                    strWhereCond = "bookingengineratetype ='" & ddlBookingType.SelectedValue & "'"
                Else
                    strWhereCond = strWhereCond & "and bookingengineratetype ='" & ddlBookingType.SelectedValue & "'"
                End If
            End If
            If IsDate(txtReqFromDt.Text) And IsDate(txtReqToDt.Text) Then
                If strWhereCond = "" Then
                    strWhereCond = "requestDate between CONVERT(datetime, '" + txtReqFromDt.Text + "',103) and CONVERT(datetime, '" + txtReqToDt.Text + "',103)"
                Else
                    strWhereCond = strWhereCond & "and requestDate between CONVERT(datetime, '" + txtReqFromDt.Text + "',103) and CONVERT(datetime, '" + txtReqToDt.Text + "',103)"
                End If
            End If
            BuildCondition = strWhereCond
        Catch ex As Exception
            BuildCondition = ""
            Throw ex
        End Try
    End Function
#End Region
#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim strappname As String = ""
        strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
    End Sub
#End Region
#Region "Protected Sub gvReadyInvoice_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReadyInvoice.Sorting"

    Protected Sub gvReadyInvoice_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReadyInvoice.RowDataBound
        e.Row.Cells(12).BorderWidth = "0"
        e.Row.Cells(13).BorderWidth = "0"
     

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' btn.Attributes.Add("onclick", "javascript:showtextbox('" + CType(txtinvno.ClientID, String) + "','" + CType(txtinvno.ClientID, String) + "')")
            '  Dim rowIndex As Integer = Convert.ToInt32(e.Row.RowIndex)
            '  hdReadyIndex.Value = rowIndex
            ' Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)
            Dim dvlblinvno As HtmlGenericControl = CType(e.Row.FindControl("dvlblinvno"), HtmlGenericControl)
            Dim dvtxtinvno As HtmlGenericControl = CType(e.Row.FindControl("dvtxtinvno"), HtmlGenericControl)
            Dim txtInvno As TextBox = CType(e.Row.FindControl("txtInvno"), TextBox)
            Dim ImageEdit As HtmlControl = CType(e.Row.FindControl("Imggvedit"), HtmlControl)
            ImageEdit.Attributes.Add("OnClick", "javascript:showtextbox('" + CType(dvtxtinvno.ClientID, String) + "','" + CType(dvlblinvno.ClientID, String) + "','" + CType(txtInvno.ClientID, String) + "')")
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)

            Dim lblAmount As Label = CType(e.Row.FindControl("lblAmount"), Label)
            If IsNumeric(lblAmount.Text) Then
                lblAmount.Text = Math.Round(Convert.ToDecimal(lblAmount.Text), decimalPlaces)
            End If
            Dim lblSalesAmount As Label = CType(e.Row.FindControl("lblSalesAmount"), Label)
            If IsNumeric(lblSalesAmount.Text) Then
                lblSalesAmount.Text = Math.Round(Convert.ToDecimal(lblSalesAmount.Text), decimalPlaces)
            End If
            Dim lblAmountReceived As Label = CType(e.Row.FindControl("lblAmountReceived"), Label)
            If IsNumeric(lblAmountReceived.Text) Then
                lblAmountReceived.Text = Math.Round(Convert.ToDecimal(lblAmountReceived.Text), decimalPlaces)
            End If
            Dim lblReceivedStatus As Label = CType(e.Row.FindControl("lblReceivedStatus"), Label)
    

        End If
    End Sub
    Protected Sub gvReadyInvoice_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReadyInvoice.Sorting
        Session.Add("strsortexpressionReady", e.SortExpression)
        SortReadyGridColoumn()
    End Sub
#End Region

#Region "Public Sub SortReadyGridColoumn()"
    Public Sub SortReadyGridColoumn()
        Dim DataTable As DataTable
        DisplayConfirmedRecords()
        DataTable = gvReadyInvoice.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirectionReady", objUtils.SwapSortDirection(Session("strsortdirectionReady")))
            dataView.Sort = Session("strsortexpressionReady") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirectionReady"))
            gvReadyInvoice.DataSource = dataView
            gvReadyInvoice.DataBind()
        End If
    End Sub
#End Region
#Region "Protected Sub gvReadyInvoice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReadyInvoice.RowCommand"
    Protected Sub gvReadyInvoice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReadyInvoice.RowCommand
        'Try
        '    Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
        '    hdReadyIndex.Value = rowIndex
        '    Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)
        '    Dim txtinvno As TextBox = CType(row.FindControl("txtInvno"), TextBox)
        '    Dim ImageEdit As ImageButton = CType(row.FindControl("Imggvedit"), ImageButton)
        '    'If e.CommandName = "ShowInvoiceNo" Then
        '    '    Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
        '    '    hdReadyIndex.Value = rowIndex
        '    '    Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)
        '    '    Dim txtinvno As TextBox = CType(row.FindControl("txtinvno"), TextBox)
        '    '    Dim lblinvno As Label = CType(row.FindControl("lblinvno"), Label)
        '    '    gvReadyInvoice.Columns(15).Visible = True
        '    '    lblinvno.Style.Add("display", "none")
        '    '    txtinvno.Style.Add("display", "block")
        '    '    'txtinvno.Attributes.Remove("readonly")
        '    '    txtinvno.Focus()
        '    '    '  btn.Attributes.Add("onclick", "javascript:showtextbox('" + CType(txtinvno.ClientID, String) + "','" + CType(txtinvno.ClientID, String) + "')")

        '    ImageEdit.Attributes.Add("onclick", "javascript:showtextbox()")
        Try
            If e.CommandName = "SaveInvoiceNo" Then
                Try

                    Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                    hdReadyIndex.Value = rowIndex
                    Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)
                    rowIndex = Convert.ToInt32(e.CommandArgument)
                    hdReadyIndex.Value = rowIndex
                    row = gvReadyInvoice.Rows(rowIndex)
                    Dim txtinvno As TextBox = CType(row.FindControl("txtInvno"), TextBox)
                    txtinvno = CType(row.FindControl("txtinvno"), TextBox)
                    Dim ImageSave As ImageButton = CType(row.FindControl("ImageSave"), ImageButton)

                    Dim lblRequestId As Label = CType(row.FindControl("lblRequestId"), Label)

                    Dim lblStatus As Label = CType(row.FindControl("lblStatus"), Label)
                    Dim lblinvno As Label = CType(row.FindControl("lblinvno"), Label)
                    'lblinvno.Style.Add("display", "block")
                    'txtinvno.Style.Add("display", "none")
                    'txtinvno.Attributes.Add("readonly", "readonly")
                    lblinvno.Text = txtinvno.Text
                    If lblinvno.Text = "" And txtinvno.Text = "" Then
                        ImageSave.Focus()
                        'ClientScript.RegisterStartupScript(Me.GetType(), " ", "alert('Please Enter InvoiceNo.');", True)
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter InvoiceNo.');", True)
                    Else
                        sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        sqlTrans = sqlConn.BeginTransaction   'connection open

                        mySqlCmd = New SqlCommand("sp_add_manualinvoice", sqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@manualinvoiceno", SqlDbType.VarChar)).Value = txtinvno.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                        sqlTrans.Commit()
                        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                        clsDBConnect.dbConnectionClose(sqlConn)
                        If (lblStatus.Text = "Pending") Then
                            lblStatus.Text = "Confirmed"
                        End If
                        lblinvno.Text = txtinvno.Text

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                        ImageSave.Focus()
                    End If

                Catch ex As Exception

                End Try
            ElseIf e.CommandName = "Proforma" Then
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                hdReadyIndex.Value = rowIndex
                Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)

                row = gvReadyInvoice.Rows(rowIndex)
                Dim lblRequestId As Label = CType(row.FindControl("lblRequestId"), Label)
                If lblRequestId.Text.Trim <> "" Then
                    Dim strpop As String = ""
                    strpop = "window.open('PrintReport.aspx?printId=bookingConfirmation&RequestId=" & lblRequestId.Text.Trim & "','ProformaPrint');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)
                End If
            ElseIf e.CommandName = "ProformaVat" Then
               
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                hdReadyIndex.Value = rowIndex
                Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)

                row = gvReadyInvoice.Rows(rowIndex)
                Dim lblinvno As Label = CType(row.FindControl("lblinvno"), Label)
                Dim lblRequestId As Label = CType(row.FindControl("lblRequestId"), Label)
                If lblRequestId.Text.Trim <> "" Then
                    Dim strpop As String = ""
                    strpop = "window.open('PrintReport.aspx?printId=ProformaVat&RequestId=" & lblRequestId.Text.Trim & "&ManualInvNo= " & lblinvno.Text & "','ProformaPrint');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)
                End If
            ElseIf e.CommandName = "View" Then
                Dim lbtnView As LinkButton
                lbtnView = CType(gvReadyInvoice.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbtnView"), LinkButton)
                Dim invoiceno As String = lbtnView.Text.Trim
                If invoiceno <> "" Then
                    Dim strpop As String = ""
                    strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & invoiceno & "','SalesInvoicePrint');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoice", strpop, True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AllotManualInvoiceNumber.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub DisplayPendingRecords()"
    Private Function DisplayPendingRecords() As DataTable
        Try
            ' lblPendingMsg.Visible = False
            Dim strBindCondition As String = BuildCondition()
            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionPending"))
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("bookingPending_salesInvoice_search", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
            mySqlCmd.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strBindCondition
            mySqlCmd.Parameters.Add(New SqlParameter("@sortBy", SqlDbType.VarChar, 100)).Value = "DepartureDate" ' strSortBy
            mySqlCmd.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@SalesInvoiceState", SqlDbType.VarChar, 20)).Value = "All"
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable()
                    myDataAdapter.Fill(dt)
                    Return dt
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Protected Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click"
    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try

            DisplayConfirmedRecords()
            ModalPopupLoading.Hide()

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + HttpUtility.JavaScriptStringEncode(ex.Message) & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Protected Sub DisplayConfirmedRecords()"
    Protected Sub DisplayConfirmedRecords()
        Try

            lblMsg.Visible = False
            Dim strBindCondition As String = BuildCondition()
            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionReady"))
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("booking_salesInvoice_search", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
            mySqlCmd.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strBindCondition
            mySqlCmd.Parameters.Add(New SqlParameter("@sortBy", SqlDbType.VarChar, 100)).Value = "DepartureDate" ' strSortBy
            mySqlCmd.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@SalesInvoiceState", SqlDbType.VarChar, 20)).Value = "All" 'Session("SalesInvoiceState")
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable()
                    myDataAdapter.Fill(dt)
                    Dim dc As New DataColumn("Selection", GetType(Boolean))
                    dc.DefaultValue = False
                    dt.Columns.Add(dc)
                    Dim dc1 As New DataColumn("InvoiceNo", GetType(String))
                    dc1.DefaultValue = ""
                    dt.Columns.Add(dc1)
                    Dim dtpending As New DataTable
                    dtpending = DisplayPendingRecords()
                    dt.Merge(dtpending)
                    gvReadyInvoice.DataSource = dt
                    gvReadyInvoice.DataBind()
                    If dt.Rows.Count = 0 Then
                        lblMsg.Visible = True
                    End If
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "Protected Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click"
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim firstday = New DateTime(Date.Now.Year, Date.Now.Month, 1)
        Dim lastday = firstday.AddMonths(1).AddDays(-1)
        txtChkFromDt.Text = firstday.ToString("dd/MM/yyyy")
        txtChkToDt.Text = lastday.ToString("dd/MM/yyyy")
        txtBookingNo.Text = ""
        txtCustName.Text = ""
        txtSupName.Text = ""
        txtGuestName.Text = ""
        txtCustRef.Text = ""
        txtReqFromDt.Text = ""
        txtReqToDt.Text = ""
        ddlBookingType.SelectedIndex = 0

        Dim dt As DataTable = CType(Session("bookingInvoice"), DataTable)
        gvReadyInvoice.DataSource = dt
        gvReadyInvoice.DataBind()

    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & AppId & "'")
                    If strappname = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accounts display name does not match with accounts module name in division master' );", True)
                        Exit Sub
                    End If
                End If

                txtDivcode.Value = ViewState("divcode")


                Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                gvReadyInvoice.Columns(11).HeaderText = "Sales Amount (" + baseCurrency + ")"


                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)

                Dim strBookingType As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2036'")
                Dim arrBookingType As String() = strBookingType.Split(";")
                If arrBookingType.Length > 0 Then
                    For i = 0 To arrBookingType.GetUpperBound(0)
                        ddlBookingType.Items.Add(New ListItem(arrBookingType(i).Trim, arrBookingType(i).Trim))
                    Next
                Else
                    ddlBookingType.Items.Add(New ListItem("All", "All"))
                End If
                Dim firstday = New DateTime(Date.Now.Year, Date.Now.Month, 1)
                Dim lastday = firstday.AddMonths(1).AddDays(-1)
                txtChkFromDt.Text = firstday.ToString("dd/MM/yyyy")
                txtChkToDt.Text = lastday.ToString("dd/MM/yyyy")

                'txtReqFromDt.Text = firstday.ToString("dd/MM/yyyy")
                'txtReqToDt.Text = lastday.ToString("dd/MM/yyyy")

                Dim dt As New DataTable
                dt.Columns.Add(New DataColumn("selection", GetType(Boolean)))
                dt.Columns.Add(New DataColumn("requestId", GetType(String)))
                dt.Columns.Add(New DataColumn("div_code", GetType(String)))
                dt.Columns.Add(New DataColumn("status", GetType(String)))
                dt.Columns.Add(New DataColumn("amended", GetType(String)))
                dt.Columns.Add(New DataColumn("arrivalDate", GetType(Date)))
                dt.Columns.Add(New DataColumn("departureDate", GetType(Date)))
                dt.Columns.Add(New DataColumn("agentName", GetType(String)))
                dt.Columns.Add(New DataColumn("agentref", GetType(String)))
                dt.Columns.Add(New DataColumn("guestName", GetType(String)))
                dt.Columns.Add(New DataColumn("Currency", GetType(String)))
                dt.Columns.Add(New DataColumn("amount", GetType(Decimal)))
                dt.Columns.Add(New DataColumn("salesAmount", GetType(Decimal)))
                dt.Columns.Add(New DataColumn("amountReceived", GetType(Decimal)))
                dt.Columns.Add(New DataColumn("receivedStatus", GetType(Integer)))
                '  dt.Columns.Add(New DataColumn("invoiceNo", GetType(String)))

                Session("bookingInvoice") = dt
                'Session("bookingSearchResult") = dt
                gvReadyInvoice.DataSource = dt
                gvReadyInvoice.DataBind()
                'Else
                '    Page.ClientScript.RegisterHiddenField("divCode", ViewState("divcode"))
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AllotManualinvoiceNumber.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
       
        ModalExtraPopup.Show()


    End Sub


    
 
End Class

