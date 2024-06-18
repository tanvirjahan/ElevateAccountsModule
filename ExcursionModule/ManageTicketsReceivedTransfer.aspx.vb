

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ExcursionModule_ManageTicketsReceivedTransfer
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim SqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim ObjDate As New clsDateTime
    Dim sqlTrans As SqlTransaction
    Dim chkSel As CheckBox
    Dim otypecode1, otypecode2 As String

    Dim gvRow As GridViewRow

    Dim lblLineNo As Label

    Dim txtprefix As TextBox
    Dim txtsuffix As TextBox

    Dim txtfromticketno As TextBox
    Dim txttoticketno As TextBox
    Dim txtTicketDate As TextBox
    Dim txtRemarks As TextBox
    Dim chkDel As CheckBox
    Dim ImgBtnFromDate As ImageButton
    Dim ddlCustomer As HtmlSelect
    Dim ddlCustomerAssigned As HtmlSelect

    Dim txtAgentName As HtmlInputText
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Page.IsPostBack = False Then
                Session.Add("ManageTicketState", Request.QueryString("State"))
                Session.Add("ManageTicketRefCode", Request.QueryString("RefCode"))

                txtconnection.Value = Session("dbconnectionName")

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpCode, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExGrpName, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeCode, "othtypcode", "othtypname", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlExTypeName, "othtypname", "othtypcode", "select rtrim(ltrim(othtypcode))othtypcode,rtrim(ltrim(othtypname))othtypname from othtypmast a  inner join othgrpmast b on a.othgrpcode=b.othgrpcode and b.othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and a.active=1 order by a.othtypname", True)

            End If

            Dim typ As Type
            typ = GetType(DropDownList)
            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddlExGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlExGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlExTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlExTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then

                If CType(Session("ManageTicketState"), String) = "View" Then

                    Dim RefCode As String
                    RefCode = CType(Session("ManageTicketRefCode"), String)
                    ShowRecord(RefCode)
                    DisableAllControls()
                    btnSave.Visible = True
                    btnSave.Enabled = True
                End If
                NumbersForTextbox(txtFromNo)
                NumbersForTextbox(txtToNo)
                btnSave.Enabled = False

            End If
            btnFillRangeTickets.Attributes.Add("onclick", "return ValidateTicketRange()")

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")
    End Sub
#End Region

#Region "NumbersForTextbox"
    Public Sub NumbersForTextbox(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")
    End Sub
#End Region

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            strSqlQry = "select * from excursion_tickets_received where ticketid='" & RefCode & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = SqlCmd.ExecuteReader
            If mySqlReader.Read = True Then
                If IsDBNull(mySqlReader("ticketid")) = False Then
                    txtAllotmentID.Value = mySqlReader("ticketid")
                End If
                If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    ddlExGrpName.Value = mySqlReader("othgrpcode")


                    ddlExGrpCode.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode")))
                End If
                If IsDBNull(mySqlReader("othtypcode")) = False Then
                    ddlExTypeName.Value = mySqlReader("othtypcode")


                    ddlExTypeCode.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", mySqlReader("othtypcode")))
                End If

                If IsDBNull(mySqlReader("datereceived")) = False Then
                    dpDateRecieved.txtDate.Text = Format(CType(mySqlReader("datereceived"), Date), "dd/MM/yyyy")
                End If


                If IsDBNull(mySqlReader("remarks")) = False Then
                    txtRemark.Text = mySqlReader("remarks")
                End If

               

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub

#End Region



#Region " Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        Try
            ddlExGrpCode.Disabled = True
            ddlExGrpName.Disabled = True
            ddlExTypeCode.Disabled = True
            ddlExTypeName.Disabled = True
            txtRemark.Enabled = False

            btnSave.Enabled = False

            txtAllotmentID.Disabled = True

            gv_row.Enabled = True

            dpDateRecieved.Enabled = False

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
    End Sub
#End Region



#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
       
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExcursionStopSalesWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region


#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim GvRow As GridViewRow
        Dim ObjDate As New clsDateTime
        Dim chk As CheckBox

        Try
            If Page.IsValid = True Then

                If CType(Session("ManageTicketState"), String) = "View" Then


                    '-----------------------------------------------------------------------------------
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    '---------------------------------------------------------------------------------
                    '                 Inserting Into Main  Ticketing Prices


                    Dim lblLineNo As Label
                    Dim txtticketno As TextBox
                    Dim txtTicketDate As TextBox
                    Dim ddlCustomer As HtmlSelect

                    Dim optionval As String

                    For Each GvRow In gv_row.Rows


                        optionval = objUtils.GetAutoDocNo("EXTRANSFER", mySqlConn, sqlTrans)
                        txtAllotmentID.Value = optionval.Trim

                        lblLineNo = GvRow.FindControl("lblLineNo")
                        txtticketno = GvRow.FindControl("txtticketno")
                        txtTicketDate = GvRow.FindControl("txtTicketDate")
                        ddlCustomer = GvRow.FindControl("ddlCustomer")
                        ddlCustomerAssigned = GvRow.FindControl("ddlCustomerAssigned")

                        SqlCmd = New SqlCommand("sp_add_excursion_tickets_transferred", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@transferid", SqlDbType.VarChar, 20)).Value = CType(txtAllotmentID.Value.Trim, String)
                        SqlCmd.Parameters.Add(New SqlParameter("@ticketid", SqlDbType.VarChar, 20)).Value = CType(Session("ManageTicketRefCode"), String)


                        SqlCmd.Parameters.Add(New SqlParameter("@transferdate", SqlDbType.DateTime)).Value = DateTime.Now
                        SqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        SqlCmd.Parameters.Add(New SqlParameter("@assigned", SqlDbType.VarChar, 20)).Value = CType(ddlCustomerAssigned.Value.Trim, String)
                        SqlCmd.Parameters.Add(New SqlParameter("@ticketno", SqlDbType.VarChar, 100)).Value = CType(txtticketno.Text.Trim, String)
                        SqlCmd.Parameters.Add(New SqlParameter("@tlineno", SqlDbType.Int)).Value = CType(lblLineNo.Text.Trim, String)
                        SqlCmd.Parameters.Add(New SqlParameter("@transferredto", SqlDbType.VarChar, 20)).Value = CType(ddlCustomer.Value.Trim, String)

                        SqlCmd.ExecuteNonQuery()


                    Next


                    sqlTrans.Commit()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Transfered To Agent Sucessfully. Click on OK to Return Search Page');", True)

                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('ExcursionTicketsWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                End If


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
    End Sub
#End Region




    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MainAllotStopSales','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub gv_row_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_row.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                txtTicketDate = CType(e.Row.FindControl("txtTicketDate"), TextBox)
                txtAgentName = New HtmlInputText
                txtAgentName = e.Row.FindControl("accSearch")


                ddlCustomer = CType(e.Row.FindControl("ddlCustomer"), HtmlSelect)
                ddlCustomerAssigned = CType(e.Row.FindControl("ddlCustomerAssigned"), HtmlSelect)
                
                txtAgentName.Attributes.Add("CustomerId", ddlCustomer.ClientID)


                'NumbersForTextbox(txttoticketno)
                'txttoticketno.Attributes.Add("onchange", "javascript:CheckTicketNo('" + CType(txtfromticketno.ClientID, String) + "','" + CType(txttoticketno.ClientID, String) + "')")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerAssigned, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub FillGridSubDetail()
        Dim myDS As New DataSet
        strSqlQry = ""
        Try
            strSqlQry = "Select * from excursion_tickets_subdetail Where assignedto <> '[Select]' and  ticketid='" & CType(Session("ManageTicketRefCode"), String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_row.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_row.DataBind()
                btnSave.Enabled = True
                btnCopy.Visible = True
                btnRemove.Visible = True
                lblMsg.Visible = False

            Else
                btnSave.Enabled = False
                gv_row.DataSource = Nothing
                gv_row.DataBind()
                btnCopy.Visible = False
                btnRemove.Visible = False
                lblMsg.Visible = True
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            Dim ddlCustomer As HtmlSelect
            Dim txtAgentName As HtmlInputText
            Dim chkCopy As CheckBox

            Dim Customer As String = ""
            Dim AgentName As String = ""

            For Each gvRow In gv_row.Rows
                chkCopy = gvRow.FindControl("chkCopy")
                ddlCustomer = gvRow.FindControl("ddlCustomer")
                txtAgentName = gvRow.FindControl("accSearch")

                If chkCopy.Checked = True Then
                    Customer = CType(ddlCustomer.Value, String)
                    AgentName = CType(txtAgentName.Value, String)
                    Exit For
                End If
            Next

            For Each gvRow In gv_row.Rows

                ddlCustomer = gvRow.FindControl("ddlCustomer")
                txtAgentName = gvRow.FindControl("accSearch")

                ddlCustomer.Value = Customer
                txtAgentName.Value = AgentName

            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)

        End Try
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Try
            Dim ddlCustomer As HtmlSelect
            Dim txtAgentName As HtmlInputText

            For Each gvRow In gv_row.Rows

                ddlCustomer = gvRow.FindControl("ddlCustomer")
                txtAgentName = gvRow.FindControl("accSearch")

                ddlCustomer.Value = "[Select]"
                txtAgentName.Value = ""

            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
    End Sub


    Private Sub AssignAgent()
        Try

            Dim gvRow As GridViewRow
            Dim ddlCustomerAssigned, ddlCustomer As HtmlSelect
         
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            SqlCmd = New SqlCommand("Select ticketid,tlineno,assignedto from excursion_tickets_subdetail Where assignedto <> '' and  ticketid='" & CType(Session("ManageTicketRefCode"), String) & "'", SqlConn)
            mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            Dim ct As Integer
            If mySqlReader.HasRows Then
              
                While mySqlReader.Read()
                    For Each gvRow In gv_row.Rows
                        ddlCustomerAssigned = gvRow.FindControl("ddlCustomerAssigned")
                        ddlCustomer = gvRow.FindControl("ddlCustomer")
                        If ddlCustomerAssigned.Value = "[Select]" Then
                            If IsDBNull(mySqlReader("assignedto")) = False Then
                                'dpFDate.txtDate.Text = CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
                                ddlCustomerAssigned.Value = CType((mySqlReader("assignedto")), String)
                                ddlCustomer.Value = CType((mySqlReader("assignedto")), String)
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        Finally
            clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close           
        End Try
    End Sub

    Protected Sub btnFillAllTickets_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillAllTickets.Click

        FillGridSubDetail()
        AssignAgent()

    End Sub

    Protected Sub btnFillRangeTickets_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillRangeTickets.Click

        FillGridSubDetailWithRange(CType(txtFromNo.Text.Trim, Integer), CType(txtToNo.Text.Trim, Integer))
        AssignAgent()

    End Sub


    Private Sub FillGridSubDetailWithRange(ByVal FromTicketNo As Integer, ByVal ToTicketNo As Integer)
        Dim myDS As New DataSet
        strSqlQry = ""
        Try
            strSqlQry = "select S.*,  SUBSTRING(S.Ticketno,Len(D.prefix)+1,(LEN(S.Ticketno)- (Len(D.prefix)) )-Len(D.suffix)) As Number from excursion_tickets_subdetail S "
            strSqlQry = strSqlQry & "Inner Join excursion_tickets_detail D On D.ticketid=S.ticketid And D.tlineno=S.tlineno "
            strSqlQry = strSqlQry & "where S.ticketid='" & CType(Session("ManageTicketRefCode"), String) & "'"
            strSqlQry = strSqlQry & " and Convert(Int,SUBSTRING(S.Ticketno,Len(D.prefix)+1,(LEN(S.Ticketno) - (Len(D.prefix)) )-Len(D.suffix))) Between " & FromTicketNo & " And " & ToTicketNo

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_row.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_row.DataBind()
                btnCopy.Visible = True
                btnRemove.Visible = True
                lblMsg.Visible = False
                btnSave.Enabled = True
            Else
                gv_row.DataSource = Nothing
                gv_row.DataBind()
                btnCopy.Visible = False
                btnRemove.Visible = False
                lblMsg.Visible = True
                btnSave.Enabled = False
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ManageTicketsReceivedAssign.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub


    Private Sub ShowRecordAssigned(ByVal RefCode As String)
        Try
            Dim gvRow As GridViewRow
            Dim accSearch As HtmlInputText
            Dim ddlCustomer As HtmlSelect
            Dim lblTicketNo As Label

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            SqlCmd = New SqlCommand("select * from excursion_tickets_subdetail where ticketid='" & RefCode & "'", SqlConn)
            mySqlReader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            'Dim ct As Integer
            If mySqlReader.HasRows Then
                'ct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from stopsaleexc_date Where mstopid='" & RefCode & "'")
                'fillDategrd(grdDates, False, ct)
                While mySqlReader.Read()
                    For Each gvRow In gv_row.Rows
                        lblTicketNo = gvRow.FindControl("lblTicketNo")
                        accSearch = gvRow.FindControl("accSearch")
                        ddlCustomer = gvRow.FindControl("ddlCustomer")

                        If lblTicketNo.Text = CType(mySqlReader("ticketno"), String) Then
                            If IsDBNull(mySqlReader("assignedto")) = False Then
                                ddlCustomer.Value = CType(mySqlReader("assignedto"), String)
                            Else
                                ddlCustomer.Value = "[Select]"
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ManageTicketsReceivedAssign.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub
End Class


