
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports MyClasses
#End Region

Partial Class ExcursionModule_DeskExcursionRequestEntry
    Inherits System.Web.UI.Page
    Private IsView As String = String.Empty
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
    Dim otypecode1, otypecode2, english, office As String

    Dim objEmail As New clsEmail

    Dim gvRow As GridViewRow
    Dim lblLineNo As Label
    Dim txtTicketDate As TextBox
    Dim txtRemarks As TextBox
    Dim chkDel As CheckBox
    Dim ImgBtnFromDate As ImageButton
    Dim txtAgentName As HtmlInputText
    Dim sqlstr As String
    Dim lstExcursionHeader As New List(Of clsExcursionHedaer)
    Dim dtExcursionDetails As New DataTable
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            txtDate.Enabled = False 'Added by Archana on 08/3/2015 as date should be disabled
            ImgBtnDate.Enabled = False 'Added by Archana on 08/3/2015 as date should be disabled
            ddlSellingType.Disabled = True 'Added by Archana on 08/3/2015 as selling type should be disabled
            ' ddlCurrency.Disabled = True 'Added by Archana on 08/3/2015 as currency should be disabled
            ddlMarket.Disabled = True 'Added by Archana on 08/3/2015 as market should be disabled
            txtConvRate.Enabled = False 'Added by Archana on 08/3/2015 as conversion rate should be disabled
            ddlSalesExpert.Disabled = True 'Added by Archana on 08/3/2015 as sales expert should be disabled

            If Page.IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If



                Session.Add("ExcursionRequestState", Request.QueryString("State"))
                Session.Add("ExcursionRequestRefCode", Request.QueryString("RefCode"))


                Session("ExcursionRequestSubEntryState") = Request.QueryString("State")
                txtconnection.Value = Session("dbconnectionName")
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                hdntouroperator.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1111)

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                english = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1107")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentname", "agentcode", "Select ltrim(rtrim(agentcode))agentcode,ltrim(rtrim(agentname))agentname from agentmast where active=1 and agentcode not in(select agentcode from agents_locked) order by agentname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPayment, "payname", "paycode", "  select ltrim(rtrim(paycode))paycode, ltrim(rtrim(payname))payname from paymentmodemaster where paycode<> 'COMP' order by payname", True)
                'Added by Archana on 08/03/2015 -- payment mode drop down should fill without complimentary
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConcierge, "spersonname", "spersoncode", "select ltrim(rtrim(spersoncode))spersoncode,ltrim(rtrim(spersonname))spersonname from spersonmast where active=1 order by spersonname", True)
                ddlConcierge.Items(ddlConcierge.SelectedIndex).Text = CType(Session("GlobalUserName"), String)
                'Added by Archana on 08/03/2015--Conceige should fill whoever login that name
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSalesExpert, "spersonname", "spersoncode", "select ltrim(rtrim(spersoncode))spersoncode,ltrim(rtrim(spersonname))spersonname from spersonmast_office  order by spersonname", True)
                ddlSalesExpert.Items(ddlSalesExpert.SelectedIndex).Text = "Office"
                'Added by Archana on 08/03/2015-- as Sales expert to fill by default office and disabled
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlLanguage, "nationalityname", "nationalitycode", " select ltrim(rtrim(nationalitycode))nationalitycode,ltrim(rtrim(nationalityname))nationalityname from nationality_master where showinexcursions=1 and active=1 order by nationalityname", True, english)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpname", "plgrpcode", "select distinct ltrim(rtrim(plgrpmast.plgrpname))plgrpname,ltrim(rtrim(plgrpmast.plgrpcode))plgrpcode from plgrpmast inner join agentmast on agentmast.plgrpcode=plgrpmast.plgrpcode where plgrpmast.active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "excsellname", "excsellcode", "select distinct rtrim(ltrim(excsellmast.excsellname))excsellname,ltrim(rtrim(excsellmast.excsellcode))excsellcode from excsellmast inner join agentmast on agentmast.excsellcode=excsellmast.excsellcode where excsellmast.active=1 order by excsellname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", "select distinct agentmast.currcode,currrates.convrate FROM agentmast INNER JOIN currrates ON agentmast.currcode = currrates.currcode WHERE tocurr=(select option_selected from reservation_parameters where param_id=457)", True)
                '16082014
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), touroperator, "agentname", "agentcode", "Select ltrim(rtrim(agentcode))agentcode,ltrim(rtrim(agentname))agentname from agentmast where active=1 and agentcode not in(select agentcode from agents_locked)  order by agentname", True)
                NumbersForTextbox(txtCreditCardNo)

                CreateDataSourceSubEntry()
                'Session("ExcursionCostDetailTable") = Nothing

                If CType(Session("ExcursionRequestState"), String) = "New" Then

                    txtUser.Text = CType(Session("GlobalUserName"), String)
                    txtDate.Text = DateTime.Now
                    Dim RefCode As String
                    RefCode = CType(Session("ManageTicketRefCode"), String)
                    btnSave.Text = "Save"
                    FilGrid(gvExcursionRequest, False, True, 2)
                    lblHeading.Text = "Add New Excursion"
                    'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    'sqlTrans = mySqlConn.BeginTransaction                                           'SQL  Trans start
                    'Dim optionval As String
                    'optionval = objUtils.GetAutoDocNo("EX", mySqlConn, sqlTrans)
                    'hdnExcursionID.Value = optionval.Trim
                    'sqlTrans.Commit()

                    Dim strSql As String = ""
                    strSql = "select lastno+1 from docgen where optionname='EX'"
                    hdnExcursionID.Value = "EX/" + objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSql)


                    'Dim tempreqId As String
                    'tempreqId = CType((objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_new_exc_temp")).Tables(0).Rows(0)(0), String)
                    'Session.Add("excid", tempreqId)


                ElseIf CType(Session("ExcursionRequestState"), String) = "EditRow" Then
                    Dim RefCode As String
                    RefCode = CType(Session("ExcursionRequestRefCode"), String)
                    ShowRecord(RefCode)
                    ShowExcursionRequest_Details(RefCode)
                    btnSave.Text = "Final Update"
                    lblHeading.Text = "Edit Excursion"

                ElseIf CType(Session("ExcursionRequestState"), String) = "ViewRow" Then
                    Dim RefCode As String
                    RefCode = CType(Session("ExcursionRequestRefCode"), String)
                    ShowRecord(RefCode)
                    ShowExcursionRequest_Details(RefCode)
                    DisableControls()
                    btnSave.Visible = False
                    lblHeading.Text = "View Excursion"
                End If

                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    'ddlExGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            End If


            If Page.IsPostBack = True Then

                'sqlstr = "select distinct plgrpmast.plgrpname,plgrpmast.plgrpcode from plgrpmast inner join agentmast on agentmast.plgrpcode=plgrpmast.plgrpcode"
                'sqlstr = sqlstr + " where plgrpmast.active=1 and agentmast.agentcode='" & CType(ddlCustomer.Value, String) & "'"
                'sqlstr = sqlstr + " order by plgrpmast.plgrpname "
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpname", "plgrpcode", sqlstr, True)
                'If ddlMarket.Items.Count > 0 Then
                '    ddlMarket.SelectedIndex = 0
                'End If

                'sqlstr = ""
                'sqlstr = "select distinct excsellmast.excsellname,excsellmast.excsellcode from excsellmast inner join agentmast on agentmast.excsellcode=excsellmast.excsellcode"
                'sqlstr = sqlstr + " where excsellmast.active=1 and agentmast.agentcode='" & CType(ddlCustomer.Value, String) & "'"
                'sqlstr = sqlstr + " order by excsellmast.excsellname "

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "excsellname", "excsellcode", sqlstr, True)
                'If ddlSellingType.Items.Count > 0 Then
                '    ddlSellingType.SelectedIndex = 0
                'End If

                'sqlstr = ""
                'sqlstr = "SELECT agentmast.currcode,currrates.convrate FROM agentmast INNER JOIN currrates ON "
                'sqlstr = sqlstr + "agentmast.currcode = currrates.currcode WHERE tocurr=(select option_selected from reservation_parameters where param_id=457) "
                'sqlstr = sqlstr + " and agentcode='" & CType(ddlCustomer.Value, String) & "'"

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", sqlstr, True)
                'If ddlCurrency.Items.Count > 0 Then
                '    ddlCurrency.SelectedIndex = 0
                'End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpname", "plgrpcode", "select distinct ltrim(rtrim(plgrpmast.plgrpname))plgrpname,ltrim(rtrim(plgrpmast.plgrpcode))plgrpcode from plgrpmast inner join agentmast on agentmast.plgrpcode=plgrpmast.plgrpcode where plgrpmast.active=1 order by plgrpname", True, hdnMarketCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "excsellname", "excsellcode", "select distinct rtrim(ltrim(excsellmast.excsellname))excsellname,ltrim(rtrim(excsellmast.excsellcode))excsellcode from excsellmast inner join agentmast on agentmast.excsellcode=excsellmast.excsellcode where excsellmast.active=1 order by excsellname", True, hdnSellingTypeCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", "select distinct agentmast.currcode,currrates.convrate FROM agentmast INNER JOIN currrates ON agentmast.currcode = currrates.currcode WHERE tocurr=(select option_selected from reservation_parameters where param_id=457)", True, hdnCurrencyCode.Value)
                ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPayment, "payname", "paycode", "  select ltrim(rtrim(paycode))paycode, ltrim(rtrim(payname))payname from paymentmodemaster where paycode<> 'COMP' order by payname", True)

                ddlConcierge.Items(ddlConcierge.SelectedIndex).Text = CType(Session("GlobalUserName"), String)
                ddlSalesExpert.Items(ddlSalesExpert.SelectedIndex).Text = "Office"
                txtConvRate.Text = hdnAgentConvRate.Value

                If ddlPayment.Value = "CRE" Then
                    txtCreditCardNo.Style("display") = "block"
                    lblCreditCardNo.Style("display") = "block"
                Else
                    txtCreditCardNo.Style("display") = "none"
                    lblCreditCardNo.Style("display") = "none"
                End If

                hdntouroptemp.Value = touroperator.Value
                If ddlCustomer.Value = hdntouroperator.Value Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), touroperator, "agentname", "agentcode", "Select ltrim(rtrim(agentcode))agentcode,ltrim(rtrim(agentname))agentname from agentmast where active=1 and agentcode not in(select agentcode from agents_locked)  order by agentname", True)
                    touroperator.Value = hdntouroptemp.Value
                    touroperator.Style("display") = "block"
                    lbloperator.Style("display") = "block"
                Else
                    touroperator.Style("display") = "none"
                    lbloperator.Style("display") = "none"
                End If



            End If

            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ExcursionRequestEntryWindowPostBack") Then
                FillGridBySubEntry()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DeskExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return alphanumeric(event)")
    End Sub
#End Region

#Region "NumbersForTextbox"
    Public Sub NumbersForTextbox(ByVal txtbox As TextBox)
        'txtbox.Attributes.Add("onkeypress", "return OnlyNumber(event)")
        txtbox.Attributes.Add("onkeypress", "return alphanumeric(event)")

    End Sub
#End Region


    Public Sub FilGrid(ByVal grd As GridView, ByVal showrecord As Boolean, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Try

            Dim lngcnt As Long
            If blnload = True Then
                lngcnt = 1
            Else
                lngcnt = count
            End If
            If (showrecord) Then
                grd.DataSource = CreateDataSource()
                If CreateDataSource().Count = 0 Then
                    gvExcursionRequest.DataSource = CreateDataSource(lngcnt)
                    gvExcursionRequest.DataBind()
                    If lngcnt = 0 Then
                        ' lblMsg.Visible = True
                        btnSave.Enabled = False
                    End If
                    gvExcursionRequest.Visible = True
                    Exit Sub
                End If
            Else
                gvExcursionRequest.DataSource = CreateDataSource(lngcnt)

            End If
            'If CreateDataSource().Count > 0 Or CreateDataSource(lngcnt).Count > 0 Then
            '    'grd.DataBind()
            '    'grd.Visible = True
            '    'lblMsg.Visible = False
            'Else
            '    '  lblMsg.Visible = True
            'End If
            gvExcursionRequest.DataBind()
            gvExcursionRequest.Visible = True
            'txtgridrows.Value = grd.Rows.Count

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try

    End Sub

    Private Function CreateDataSource(ByVal lngcount As Long) As DataView

        Try

            Dim dt As DataTable
            Dim dr As DataRow
            Dim i As Integer
            dt = New DataTable
            dt.Columns.Add(New DataColumn("rlineno", GetType(String)))
            dt.Columns.Add(New DataColumn("rowid", GetType(String)))
            dt.Columns.Add(New DataColumn("tourdate", GetType(String)))
            dt.Columns.Add(New DataColumn("othgrpcode", GetType(String)))
            dt.Columns.Add(New DataColumn("othgrpname", GetType(String)))

            dt.Columns.Add(New DataColumn("guestname", GetType(String)))
            dt.Columns.Add(New DataColumn("adults", GetType(String)))
            dt.Columns.Add(New DataColumn("child", GetType(String)))
            dt.Columns.Add(New DataColumn("rateadults", GetType(String)))
            dt.Columns.Add(New DataColumn("ratechild", GetType(String)))

            dt.Columns.Add(New DataColumn("amount", GetType(String)))
            dt.Columns.Add(New DataColumn("amountAED", GetType(String)))
            dt.Columns.Add(New DataColumn("amend", GetType(String)))
            dt.Columns.Add(New DataColumn("cancel", GetType(String)))
            dt.Columns.Add(New DataColumn("costRateAdult", GetType(String)))

            dt.Columns.Add(New DataColumn("costRateChild", GetType(String)))
            dt.Columns.Add(New DataColumn("costAmount", GetType(String)))
            dt.Columns.Add(New DataColumn("costAmountAED", GetType(String)))
            dt.Columns.Add(New DataColumn("cancelReason", GetType(String)))
            dt.Columns.Add(New DataColumn("complimentcust", GetType(String)))

            dt.Columns.Add(New DataColumn("complimentprov", GetType(String)))
            dt.Columns.Add(New DataColumn("comReduceAdultPer", GetType(String)))
            dt.Columns.Add(New DataColumn("comReduceChildPer", GetType(String)))
            dt.Columns.Add(New DataColumn("comReduceAmount", GetType(String)))
            dt.Columns.Add(New DataColumn("commpayperc", GetType(String)))

            dt.Columns.Add(New DataColumn("commpayamount", GetType(String)))
            dt.Columns.Add(New DataColumn("hotel", GetType(String)))
            dt.Columns.Add(New DataColumn("roomNo", GetType(String)))
            dt.Columns.Add(New DataColumn("providerCode", GetType(String)))
            dt.Columns.Add(New DataColumn("attn", GetType(String)))

            dt.Columns.Add(New DataColumn("conf", GetType(String)))
            dt.Columns.Add(New DataColumn("confno", GetType(String)))
            dt.Columns.Add(New DataColumn("flighttype", GetType(String)))
            dt.Columns.Add(New DataColumn("flightno", GetType(String)))
            dt.Columns.Add(New DataColumn("flighttime", GetType(String)))
            dt.Columns.Add(New DataColumn("airport", GetType(String)))

            dt.Columns.Add(New DataColumn("pickuptime", GetType(String)))
            dt.Columns.Add(New DataColumn("remarks", GetType(String)))
            dt.Columns.Add(New DataColumn("arrival", GetType(String)))
            dt.Columns.Add(New DataColumn("departure", GetType(String)))
            dt.Columns.Add(New DataColumn("locked", GetType(String)))


            dt.Columns.Add(New DataColumn("spersoncomm", GetType(String)))
            dt.Columns.Add(New DataColumn("partycode", GetType(String)))
            dt.Columns.Add(New DataColumn("guide", GetType(String)))
            dt.Columns.Add(New DataColumn("trf_required", GetType(String)))
            dt.Columns.Add(New DataColumn("trf_amount", GetType(String)))
            dt.Columns.Add(New DataColumn("trf_supplier", GetType(String)))

            dt.Columns.Add(New DataColumn("total_amount", GetType(String)))
            dt.Columns.Add(New DataColumn("incominginvno", GetType(String)))
            dt.Columns.Add(New DataColumn("debitnoteno", GetType(String)))
            dt.Columns.Add(New DataColumn("epartycode", GetType(String)))
            dt.Columns.Add(New DataColumn("othtypcode", GetType(String)))
            dt.Columns.Add(New DataColumn("othtypname", GetType(String)))
            dt.Columns.Add(New DataColumn("confirmed", GetType(String)))
            dt.Columns.Add(New DataColumn("supconf", GetType(String)))

            '13082014
            dt.Columns.Add(New DataColumn("exctime", GetType(String)))
            dt.Columns.Add(New DataColumn("trfno", GetType(String)))

            dt.Columns.Add(New DataColumn("supconfno", GetType(String)))
            dt.Columns.Add(New DataColumn("protktno", GetType(String)))
            dt.Columns.Add(New DataColumn("reminderdt", GetType(String)))
            'dt.Columns.Add(New DataColumn("trf_required", GetType(String)))

            dt.Rows.Add("1", "1", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")

            'For i = 1 To lngcount
            '    dr = dt.NewRow()
            '    dr(0) = i
            '    dt.Rows.Add(dr)
            'Next

            CreateDataSource = New DataView(dt)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            CreateDataSource = Nothing
        End Try

    End Function

    Private Function CreateDataSource() As DataView
        Try

            Dim sqlstr As String

            Dim dt As DataTable
            dt = New DataTable
            dt.Columns.Add(New DataColumn("tlineno", GetType(Integer)))

            sqlstr = "Select tlineno from excursion_tickets_detail Where ticketid='" & CType(Session("ManageTicketRefCode"), String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(sqlstr, SqlConn)
            myDataAdapter.Fill(dt)

            CreateDataSource = New DataView(dt)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            CreateDataSource = Nothing
        End Try

    End Function
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If CType(Session("ExcursionRequestState"), String) = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excursions_header", "ticketno", CType(txtTicketNo.Text.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Ticket No  already entered.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf CType(Session("ExcursionRequestState"), String) = "EditRow" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "excursions_header", "excid", "ticketno", txtTicketNo.Text.Trim, CType(txtExcursionID.Text.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Ticket No  already entered.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim GvRow As GridViewRow
        Dim ObjDate As New clsDateTime
        Dim chk As CheckBox
        Dim frmdt As TextBox
        Dim todt As TextBox
        Dim RmCode As HtmlSelect
        Dim lblroomcodee As Label
        Dim lblstopsale As Label



        Try
            If Page.IsValid = True Then
                If ValidateGridOneRow() = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter one record in grid');", True)
                    Exit Sub
                End If

                If checkForDuplicate() = False Then
                    Exit Sub
                End If

                If Val(txtConvRate.Text) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check Conversion Rate');", True)
                    Exit Sub
                End If

                If CType(Session("ExcursionRequestSubEntryState"), String) = "AddRow" Then


                    'Dim lstExcursionHeader As New List(Of clsExcursionHedaer)
                    'lstExcursionHeader = Session("lstExcursionHeader")


                    'Inserting into Header Table
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction                                           'SQL  Trans start

                    Dim optionval As String
                    Dim Optiontktno As String

                    '    If hdnExcursionID.Value = "" Then
                    optionval = objUtils.GetAutoDocNo("EX", mySqlConn, sqlTrans)
                    hdnExcursionID.Value = optionval.Trim
                    'End If

                    If txtTicketNo.Text.Trim = "" Then
                        Optiontktno = objUtils.GetAutoDocNo("TICKETNO", mySqlConn, sqlTrans)
                        txtTicketNo.Text = Optiontktno.Trim
                    End If


                    SqlCmd = New SqlCommand("sp_add_excursions_header", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = hdnExcursionID.Value.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@requestdate", SqlDbType.DateTime)).Value = CType(txtDate.Text.Trim, DateTime)

                    SqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 10)).Value = ddlSellingType.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = ddlMarket.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = ddlCurrency.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 2)).Value = Val(txtConvRate.Text)

                    SqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 20)).Value = ddlConcierge.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@paycode", SqlDbType.VarChar, 20)).Value = ddlPayment.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@collectby", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@payref", SqlDbType.VarChar, 20)).Value = ""

                    SqlCmd.Parameters.Add(New SqlParameter("@ticketno", SqlDbType.VarChar, 30)).Value = txtTicketNo.Text.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@nationalitycode", SqlDbType.VarChar, 20)).Value = ddlLanguage.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@language", SqlDbType.VarChar, 100)).Value = ddlLanguage.Items(ddlLanguage.SelectedIndex).Text
                    SqlCmd.Parameters.Add(New SqlParameter("@prepaidid", SqlDbType.VarChar, 20)).Value = DBNull.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@prepaidlineno", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = ddlCustomer.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@esettleid", SqlDbType.VarChar, 20)).Value = ""

                    SqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = DateTime.Now
                    'SqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    'SqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = DBNull.Value
                    'added dbnull in moddate and moduseer by Archana on 19/03/2015
                    SqlCmd.Parameters.Add(New SqlParameter("@coltdon", SqlDbType.DateTime)).Value = DBNull.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@exc_provider", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@costcurrcode", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@costconvrate", SqlDbType.Decimal, 18, 2)).Value = 0
                    SqlCmd.Parameters.Add(New SqlParameter("@invno", SqlDbType.VarChar, 20)).Value = txtInvoiceNo.Text.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@incominginvno", SqlDbType.VarChar, 20)).Value = txtCreditCardNo.Text.Trim

                    SqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = txtCreditNoteNo.Text.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@debitnoteno", SqlDbType.VarChar, 100)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@collectedamt", SqlDbType.Decimal, 18, 4)).Value = IIf(txtcollectamt.Text = "", 0, txtcollectamt.Text)
                    SqlCmd.Parameters.Add(New SqlParameter("@dmc", SqlDbType.Int)).Value = 0
                    SqlCmd.Parameters.Add(New SqlParameter("@spersoncode_office", SqlDbType.VarChar, 10)).Value = ddlSalesExpert.Value
                    '16082014
                    SqlCmd.Parameters.Add(New SqlParameter("@operatorcode", SqlDbType.VarChar, 20)).Value = IIf(touroperator.Value = "[Select]", "", touroperator.Value)


                    SqlCmd.ExecuteNonQuery()
                    '' update ticketsubdetail
                    'SqlCmd = New SqlCommand("sp_del_excticketdet", mySqlConn, sqlTrans)
                    'SqlCmd.CommandType = CommandType.StoredProcedure
                    'SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = hdnExcursionID.Value.Trim
                    'SqlCmd.ExecuteNonQuery()

                    'Inserting into Detail Table
                    Dim RLineNo As Integer = 1
                    dtExcursionDetails = Session("DtExcursionSubEntry")

                    For i = 0 To dtExcursionDetails.Rows.Count - 2

                        SqlCmd = New SqlCommand("sp_add_excursions_detail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = hdnExcursionID.Value.Trim
                        SqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = RLineNo
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("othtypcode").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@tourdate", SqlDbType.DateTime)).Value = dtExcursionDetails.Rows(i)("tourdate").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@adults", SqlDbType.Int)).Value = dtExcursionDetails.Rows(i)("adults").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@child", SqlDbType.Int)).Value = dtExcursionDetails.Rows(i)("child").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@salepricea", SqlDbType.Decimal, 18, 4)).Value = dtExcursionDetails.Rows(i)("rateadults").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@salepriceb", SqlDbType.Decimal, 18, 4)).Value = dtExcursionDetails.Rows(i)("ratechild").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@salecurrency", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("amount"))
                        SqlCmd.Parameters.Add(New SqlParameter("@salevalue", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("amountAED"))

                        SqlCmd.Parameters.Add(New SqlParameter("@provby", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@attn", SqlDbType.VarChar, 100)).Value = dtExcursionDetails.Rows(i)("attn").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@confno", SqlDbType.VarChar, 100)).Value = dtExcursionDetails.Rows(i)("confno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@conflineno", SqlDbType.Int)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@confirmedon", SqlDbType.DateTime)).Value = DBNull.Value

                        SqlCmd.Parameters.Add(New SqlParameter("@timelimit", SqlDbType.DateTime)).Value = DBNull.Value
                        SqlCmd.Parameters.Add(New SqlParameter("@econfid", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@pickuptime", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("pickuptime").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@hotel", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("hotel").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@guestname", SqlDbType.VarChar, 150)).Value = dtExcursionDetails.Rows(i)("guestname").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("roomno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@cancelled", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("cancel"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@confirmed", SqlDbType.Int)).Value = dtExcursionDetails.Rows(i)("confirmed").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@arrdate", SqlDbType.DateTime)).Value = dtExcursionDetails.Rows(i)("tourdate").ToString 'CType(dtExcursionDetails.Rows(i)("arrival"), DateTime)
                        SqlCmd.Parameters.Add(New SqlParameter("@depdate", SqlDbType.DateTime)).Value = dtExcursionDetails.Rows(i)("tourdate").ToString 'CType(dtExcursionDetails.Rows(i)("departure"), DateTime)

                        SqlCmd.Parameters.Add(New SqlParameter("@extype", SqlDbType.Int)).Value = DBNull.Value
                        SqlCmd.Parameters.Add(New SqlParameter("@amended", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("amend"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@costpricea", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costRateAdult"))
                        SqlCmd.Parameters.Add(New SqlParameter("@costpriceb", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costRateChild"))
                        SqlCmd.Parameters.Add(New SqlParameter("@costcurrency", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costAmount"))

                        SqlCmd.Parameters.Add(New SqlParameter("@costvalue", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costAmountAED"))
                        SqlCmd.Parameters.Add(New SqlParameter("@cancelreason", SqlDbType.VarChar, 500)).Value = dtExcursionDetails.Rows(i)("cancelReason").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@complimentcust", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("complimentcust"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@complimentprov", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("complimentprov"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@commrecperc", SqlDbType.Decimal, 10, 4)).Value = 0

                        SqlCmd.Parameters.Add(New SqlParameter("@commrecamount", SqlDbType.Decimal, 18, 4)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@commpayperc", SqlDbType.Decimal, 10, 4)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@commpayamount", SqlDbType.Decimal, 18, 4)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@flighttype", SqlDbType.Char)).Value = CType(dtExcursionDetails.Rows(i)("flighttype"), Char)
                        SqlCmd.Parameters.Add(New SqlParameter("@flightno", SqlDbType.VarChar, 10)).Value = dtExcursionDetails.Rows(i)("flightno").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@flighttime", SqlDbType.VarChar, 10)).Value = dtExcursionDetails.Rows(i)("flighttime").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@airport", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("airport").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@commrecpercb", SqlDbType.Decimal, 10, 4)).Value = 0
                        'SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("othgrpcode").ToString
                        'Commented by Archana on 09/03/2015 
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("othgrpcode").ToString
                        'Added by Archana on 09/03/2015--Passing othgrpcode as dbnull.value
                        SqlCmd.Parameters.Add(New SqlParameter("@drivercode", SqlDbType.VarChar, 20)).Value = ""

                        SqlCmd.Parameters.Add(New SqlParameter("@cartype", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@carno", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@guide", SqlDbType.VarChar, 20)).Value = IIf(dtExcursionDetails.Rows(i)("guide").ToString = "[Select]" Or dtExcursionDetails.Rows(i)("guide").ToString = "", DBNull.Value, dtExcursionDetails.Rows(i)("guide").ToString)
                        SqlCmd.Parameters.Add(New SqlParameter("@terminal", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@pickupdate", SqlDbType.DateTime)).Value = DBNull.Value

                        SqlCmd.Parameters.Add(New SqlParameter("@spersoncomm", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("spersoncomm"))
                        SqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("hotel").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@trf_required", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("trf_required"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@trf_amount", SqlDbType.Money)).Value = Val(dtExcursionDetails.Rows(i)("total_amount"))
                        SqlCmd.Parameters.Add(New SqlParameter("@trf_supplier", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("trf_supplier").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@total_amount", SqlDbType.Money)).Value = Val(dtExcursionDetails.Rows(i)("total_amount"))
                        SqlCmd.Parameters.Add(New SqlParameter("@incominginvno", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("incominginvno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@debitnoteno", SqlDbType.VarChar, 100)).Value = dtExcursionDetails.Rows(i)("debitnoteno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@epartycode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("epartycode").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@additionaldrivers", SqlDbType.VarChar)).Value = ""

                        SqlCmd.Parameters.Add(New SqlParameter("@costCurrencyName", SqlDbType.VarChar, 10)).Value = dtExcursionDetails.Rows(i)("costCurrencyName").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@costConvRate", SqlDbType.Decimal, 18, 12)).Value = Val(dtExcursionDetails.Rows(i)("costConvRate"))
                        SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar)).Value = dtExcursionDetails.Rows(i)("remarks").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@supconf", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("supconf"), Integer)

                        SqlCmd.Parameters.Add(New SqlParameter("@exctime", SqlDbType.VarChar, 20)).Value = CType(dtExcursionDetails.Rows(i)("exctime"), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@trfno", SqlDbType.VarChar, 20)).Value = CType(dtExcursionDetails.Rows(i)("trfno"), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@supconfno", SqlDbType.VarChar, 20)).Value = CType(dtExcursionDetails.Rows(i)("supconfno"), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@protktno", SqlDbType.VarChar, 500)).Value = CType(dtExcursionDetails.Rows(i)("protktno"), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@reminderdt", SqlDbType.DateTime)).Value = dtExcursionDetails.Rows(i)("reminderdt").ToString

                        'SqlCmd.Parameters.Add(New SqlParameter("@trf_required", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("trf_required"), Integer)


                        SqlCmd.ExecuteNonQuery()
                        RLineNo = RLineNo + 1

                    Next



                    sqlTrans.Commit()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close                    
                    RLineNo = 1
                    'For i = 0 To dtExcursionDetails.Rows.Count - 2
                    'SaveExcursionCostDetails(hdnExcursionID.Value.Trim)' Commented by Archana on 19/03/2015
                    'RLineNo = RLineNo + 1
                    'Next



                    '25082014
                    SendMailtotrfdept(Session("ExcursionRequestRefCode")) 'Uncommented by Archana on 19/03/2015
                    Session("ExcursionRequestRefCode") = ""
                    Session("ExcursionRequestSubEntryLineNo") = ""
                    Session("ExTicketNo") = ""
                    hdnExcursionID.Value = ""


                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Sucessfully. Click OK to Return Search Page');", True)
                    Session("ExcursionCostDetailTable") = Nothing

                End If


                If CType(Session("ExcursionRequestSubEntryState"), String) = "EditRow" Then

                    '-----------------------------------------------------------------------------------
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    Dim RLineNo As Integer = 1
                    'Inserting into log tables
                    SqlCmd = New SqlCommand("sp_excursions_log", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = txtExcursionID.Text.Trim
                    SqlCmd.ExecuteNonQuery()

                    '---------------------------------------------------------------------------------
                    '                 Inserting Into Header 
                    '---------------------------------------------------------------------------------
                    txtExcursionID.Text = CType(Session("ExcursionRequestRefCode"), String)
                    SqlCmd = New SqlCommand("sp_mod_excursions_header", mySqlConn, sqlTrans)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = txtExcursionID.Text.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@requestdate", SqlDbType.DateTime)).Value = CType(txtDate.Text.Trim, DateTime)

                    SqlCmd.Parameters.Add(New SqlParameter("@othsellcode", SqlDbType.VarChar, 10)).Value = ddlSellingType.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = ddlMarket.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = ddlCurrency.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 2)).Value = Val(txtConvRate.Text)

                    SqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 20)).Value = ddlConcierge.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@paycode", SqlDbType.VarChar, 20)).Value = ddlPayment.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@collectby", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@payref", SqlDbType.VarChar, 20)).Value = ""

                    SqlCmd.Parameters.Add(New SqlParameter("@ticketno", SqlDbType.VarChar, 30)).Value = txtTicketNo.Text.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@nationalitycode", SqlDbType.VarChar, 20)).Value = ddlLanguage.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@language", SqlDbType.VarChar, 100)).Value = ddlLanguage.Items(ddlLanguage.SelectedIndex).Text
                    SqlCmd.Parameters.Add(New SqlParameter("@prepaidid", SqlDbType.VarChar, 20)).Value = DBNull.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@prepaidlineno", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = ddlCustomer.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@esettleid", SqlDbType.VarChar, 20)).Value = ""

                    SqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = DateTime.Now
                    SqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = DateTime.Now

                    SqlCmd.Parameters.Add(New SqlParameter("@coltdon", SqlDbType.DateTime)).Value = DBNull.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@exc_provider", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    SqlCmd.Parameters.Add(New SqlParameter("@costcurrcode", SqlDbType.VarChar, 20)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@costconvrate", SqlDbType.Decimal, 18, 2)).Value = 0
                    SqlCmd.Parameters.Add(New SqlParameter("@invno", SqlDbType.VarChar, 20)).Value = txtInvoiceNo.Text.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@incominginvno", SqlDbType.VarChar, 20)).Value = txtCreditCardNo.Text.Trim

                    SqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = txtCreditNoteNo.Text.Trim
                    SqlCmd.Parameters.Add(New SqlParameter("@debitnoteno", SqlDbType.VarChar, 100)).Value = ""
                    SqlCmd.Parameters.Add(New SqlParameter("@collectedamt", SqlDbType.Decimal, 18, 4)).Value = txtcollectamt.Text
                    SqlCmd.Parameters.Add(New SqlParameter("@dmc", SqlDbType.Int)).Value = 0
                    SqlCmd.Parameters.Add(New SqlParameter("@spersoncode_office", SqlDbType.VarChar, 10)).Value = ddlSalesExpert.Value

                    SqlCmd.Parameters.Add(New SqlParameter("@operatorcode", SqlDbType.VarChar, 20)).Value = IIf(touroperator.Value = "[Select]", "", touroperator.Value)


                    SqlCmd.ExecuteNonQuery()

                    'Inserting into Detail Table

                    '' update ticketsubdetail
                    'SqlCmd = New SqlCommand("sp_del_excticketdet", mySqlConn, sqlTrans)
                    'SqlCmd.CommandType = CommandType.StoredProcedure
                    'SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = txtExcursionID.Text.Trim
                    'SqlCmd.ExecuteNonQuery()

                    ''Deleting first and then inserting
                    'SqlCmd = New SqlCommand("sp_del_excursions_detail", mySqlConn, sqlTrans)
                    'SqlCmd.CommandType = CommandType.StoredProcedure
                    'SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = txtExcursionID.Text.Trim
                    'SqlCmd.ExecuteNonQuery()


                    dtExcursionDetails = Session("DtExcursionSubEntry")

                    For i = 0 To dtExcursionDetails.Rows.Count - 2
                        SqlCmd = New SqlCommand("sp_add_excursions_detail", mySqlConn, sqlTrans)
                        SqlCmd.CommandType = CommandType.StoredProcedure
                        SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = txtExcursionID.Text.Trim
                        SqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("rlineno"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = IIf(dtExcursionDetails.Rows(i)("othtypcode").ToString = "[Select]", DBNull.Value, dtExcursionDetails.Rows(i)("othtypcode").ToString)
                        SqlCmd.Parameters.Add(New SqlParameter("@tourdate", SqlDbType.DateTime)).Value = dtExcursionDetails.Rows(i)("tourdate").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@adults", SqlDbType.Int)).Value = dtExcursionDetails.Rows(i)("adults").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@child", SqlDbType.Int)).Value = dtExcursionDetails.Rows(i)("child").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@salepricea", SqlDbType.Decimal, 18, 4)).Value = dtExcursionDetails.Rows(i)("rateadults").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@salepriceb", SqlDbType.Decimal, 18, 4)).Value = dtExcursionDetails.Rows(i)("ratechild").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@salecurrency", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("amount"))
                        SqlCmd.Parameters.Add(New SqlParameter("@salevalue", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("amountAED"))

                        SqlCmd.Parameters.Add(New SqlParameter("@provby", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@attn", SqlDbType.VarChar, 100)).Value = dtExcursionDetails.Rows(i)("attn").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@confno", SqlDbType.VarChar, 100)).Value = dtExcursionDetails.Rows(i)("confno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@conflineno", SqlDbType.Int)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@confirmedon", SqlDbType.DateTime)).Value = DBNull.Value

                        SqlCmd.Parameters.Add(New SqlParameter("@timelimit", SqlDbType.DateTime)).Value = DBNull.Value
                        SqlCmd.Parameters.Add(New SqlParameter("@econfid", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@pickuptime", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("pickuptime").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@hotel", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("hotel").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@guestname", SqlDbType.VarChar, 150)).Value = dtExcursionDetails.Rows(i)("guestname").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("roomno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@cancelled", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("cancel"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@confirmed", SqlDbType.Int)).Value = dtExcursionDetails.Rows(i)("confirmed").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@arrdate", SqlDbType.DateTime)).Value = dtExcursionDetails.Rows(i)("tourdate").ToString 'CType(dtExcursionDetails.Rows(i)("arrival"), DateTime)
                        SqlCmd.Parameters.Add(New SqlParameter("@depdate", SqlDbType.DateTime)).Value = dtExcursionDetails.Rows(i)("tourdate").ToString 'CType(dtExcursionDetails.Rows(i)("departure"), DateTime)

                        SqlCmd.Parameters.Add(New SqlParameter("@extype", SqlDbType.Int)).Value = DBNull.Value
                        SqlCmd.Parameters.Add(New SqlParameter("@amended", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("amend"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@costpricea", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costRateAdult"))
                        SqlCmd.Parameters.Add(New SqlParameter("@costpriceb", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costRateChild"))
                        SqlCmd.Parameters.Add(New SqlParameter("@costcurrency", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costAmount"))

                        SqlCmd.Parameters.Add(New SqlParameter("@costvalue", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("costAmountAED"))
                        SqlCmd.Parameters.Add(New SqlParameter("@cancelreason", SqlDbType.VarChar, 500)).Value = dtExcursionDetails.Rows(i)("cancelReason").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@complimentcust", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("complimentcust"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@complimentprov", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("complimentprov"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@commrecperc", SqlDbType.Decimal, 10, 4)).Value = 0

                        SqlCmd.Parameters.Add(New SqlParameter("@commrecamount", SqlDbType.Decimal, 18, 4)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@commpayperc", SqlDbType.Decimal, 10, 4)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@commpayamount", SqlDbType.Decimal, 18, 4)).Value = 0
                        SqlCmd.Parameters.Add(New SqlParameter("@flighttype", SqlDbType.Char)).Value = CType(dtExcursionDetails.Rows(i)("flighttype"), Char)
                        SqlCmd.Parameters.Add(New SqlParameter("@flightno", SqlDbType.VarChar, 10)).Value = dtExcursionDetails.Rows(i)("flightno").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@flighttime", SqlDbType.VarChar, 10)).Value = dtExcursionDetails.Rows(i)("flighttime").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@airport", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("airport").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@commrecpercb", SqlDbType.Decimal, 10, 4)).Value = 0
                        'SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("othgrpcode").ToString
                        'Commented by Archana on 09/03/2015 
                        SqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("othgrpcode").ToString
                        'Added othgrpcode by Archana on 09/03/2015 as we need to pass dbnull.value
                        SqlCmd.Parameters.Add(New SqlParameter("@drivercode", SqlDbType.VarChar, 20)).Value = ""

                        SqlCmd.Parameters.Add(New SqlParameter("@cartype", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@carno", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@guide", SqlDbType.VarChar, 20)).Value = IIf(dtExcursionDetails.Rows(i)("guide").ToString = "[Select]" Or dtExcursionDetails.Rows(i)("guide").ToString = "", DBNull.Value, dtExcursionDetails.Rows(i)("guide").ToString)
                        SqlCmd.Parameters.Add(New SqlParameter("@terminal", SqlDbType.VarChar, 20)).Value = ""
                        SqlCmd.Parameters.Add(New SqlParameter("@pickupdate", SqlDbType.DateTime)).Value = DBNull.Value

                        SqlCmd.Parameters.Add(New SqlParameter("@spersoncomm", SqlDbType.Decimal, 18, 4)).Value = Val(dtExcursionDetails.Rows(i)("spersoncomm"))
                        SqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("hotel").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@trf_required", SqlDbType.Int)).Value = CType(dtExcursionDetails.Rows(i)("trf_required"), Integer)
                        SqlCmd.Parameters.Add(New SqlParameter("@trf_amount", SqlDbType.Money)).Value = Val(dtExcursionDetails.Rows(i)("total_amount"))
                        SqlCmd.Parameters.Add(New SqlParameter("@trf_supplier", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("trf_supplier").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@total_amount", SqlDbType.Money)).Value = Val(dtExcursionDetails.Rows(i)("total_amount"))
                        SqlCmd.Parameters.Add(New SqlParameter("@incominginvno", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("incominginvno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@debitnoteno", SqlDbType.VarChar, 100)).Value = dtExcursionDetails.Rows(i)("debitnoteno").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@epartycode", SqlDbType.VarChar, 20)).Value = dtExcursionDetails.Rows(i)("epartycode").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@additionaldrivers", SqlDbType.VarChar)).Value = ""

                        SqlCmd.Parameters.Add(New SqlParameter("@costCurrencyName", SqlDbType.VarChar, 10)).Value = dtExcursionDetails.Rows(i)("costCurrencyName").ToString
                        SqlCmd.Parameters.Add(New SqlParameter("@costConvRate", SqlDbType.Decimal, 18, 12)).Value = Val(dtExcursionDetails.Rows(i)("costConvRate"))
                        SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar)).Value = dtExcursionDetails.Rows(i)("remarks").ToString

                        SqlCmd.Parameters.Add(New SqlParameter("@supconf", SqlDbType.Int)).Value = CType(IIf(IsDBNull(dtExcursionDetails.Rows(i)("supconf")) = True, 0, dtExcursionDetails.Rows(i)("supconf")), Integer)

                        SqlCmd.Parameters.Add(New SqlParameter("@exctime", SqlDbType.VarChar, 20)).Value = CType(IIf(IsDBNull(dtExcursionDetails.Rows(i)("exctime")) = True, "", dtExcursionDetails.Rows(i)("exctime")), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@trfno", SqlDbType.VarChar, 20)).Value = CType(IIf(IsDBNull(dtExcursionDetails.Rows(i)("trfno")) = True, "", dtExcursionDetails.Rows(i)("trfno")), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@supconfno", SqlDbType.VarChar, 20)).Value = CType(dtExcursionDetails.Rows(i)("supconfno"), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@protktno", SqlDbType.VarChar, 500)).Value = CType(dtExcursionDetails.Rows(i)("protktno"), String)

                        SqlCmd.Parameters.Add(New SqlParameter("@reminderdt", SqlDbType.DateTime)).Value = IIf(dtExcursionDetails.Rows(i)("reminderdt").ToString = "", Date.Now, dtExcursionDetails.Rows(i)("reminderdt").ToString)

                        'SqlCmd.Parameters.Add(New SqlParameter("@trf_required", SqlDbType.Int)).Value = CType(IIf(IsDBNull(dtExcursionDetails.Rows(i)("trf_required")) = True, 0, dtExcursionDetails.Rows(i)("trf_required")), Integer)

                        SqlCmd.ExecuteNonQuery()

                        'RLineNo = RLineNo + 1

                    Next

                    sqlTrans.Commit()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(SqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                    'For i = 0 To dtExcursionDetails.Rows.Count - 2
                    ' SaveExcursionCostDetails(txtExcursionID.Text.Trim)
                    'Next
                    '25082014
                    ' SendMailtotrfdept(Session("ExcursionRequestRefCode"))


                    Session("ExcursionRequestRefCode") = ""
                    Session("ExcursionRequestSubEntryLineNo") = ""
                    Session("ExTicketNo") = ""
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Sucessfully. Click OK to Return Search Page');", True)
                End If


                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ExcursionsRequestWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("DeskExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Private Function SaveExcursionCostDetails(ByVal excId As String, Optional ByVal rlineno As Integer = 0) As Boolean
        Try
            Dim mStrQry As String = " SELECT * FROM excursions_cost_detail_temp Where excid ='" & excId & "'"
            Dim mDt As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mStrQry)
            If mDt.Tables.Count > 0 Then Session("ExcursionCostDetailTable") = mDt.Tables(0)
            If Not Session("ExcursionCostDetailTable") Is Nothing Then
                Dim flag As Boolean = False
                Dim dtExcCostDetails As New DataTable
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                SqlCmd = New SqlCommand("DELETE FROM Excursions_CostDetail Where excid='" & CType(excId, String) & "'", SqlConn)
                SqlCmd.CommandType = CommandType.Text
                'SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = CType(excId, String)
                'SqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.VarChar, 20)).Value = CType(rlineno, String)
                SqlCmd.ExecuteNonQuery()

                dtExcCostDetails = DirectCast(Session("ExcursionCostDetailTable"), DataTable)

                For Each row As DataRow In dtExcCostDetails.Rows
                    'If CType(row.Item("rlineno"), Integer) = CType(rlineno, Integer) Then
                    SqlCmd = New SqlCommand("sp_InsertorUpdateExcursionCostDetails", SqlConn)
                    SqlCmd.CommandType = CommandType.StoredProcedure
                    SqlCmd.CommandTimeout = 0

                    SqlCmd.Parameters.Add(New SqlParameter("@RowId", SqlDbType.Int)).Value = 0
                    SqlCmd.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = CType(excId, String)
                    SqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = CType(row.Item("rlineno"), Integer)
                    SqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(row.Item("othtypcode"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@supplier", SqlDbType.VarChar, 20)).Value = CType(row.Item("supplier"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@AdultCostRate", SqlDbType.Decimal)).Value = CType(row.Item("AdultCostRate"), Decimal)
                    SqlCmd.Parameters.Add(New SqlParameter("@ChildCostRate", SqlDbType.Decimal)).Value = CType(row.Item("ChildCostRate"), Decimal)
                    SqlCmd.Parameters.Add(New SqlParameter("@costcurrency", SqlDbType.VarChar, 50)).Value = CType(row.Item("costcurrency"), String)
                    SqlCmd.Parameters.Add(New SqlParameter("@conversionrate", SqlDbType.Decimal)).Value = CType(row.Item("conversionrate"), Decimal)
                    SqlCmd.Parameters.Add(New SqlParameter("@costvalue", SqlDbType.Decimal)).Value = CType(row.Item("costvalue"), Decimal)
                    SqlCmd.Parameters.Add(New SqlParameter("@noofunits", SqlDbType.Int)).Value = CType(row.Item("noofunits"), Integer)
                    SqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 100)).Value = CType(row.Item("remarks"), String)
                    flag = SqlCmd.ExecuteNonQuery()
                    'End If
                Next

                If flag Then
                    Dim delQry As String = String.Empty
                    delQry = "DELETE FROM excursions_cost_detail_temp Where excid ='" & excId & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    SqlCmd = New SqlCommand(delQry, SqlConn)
                    SqlCmd.CommandType = CommandType.Text
                    SqlCmd.ExecuteNonQuery()
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DeskExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function


    Protected Sub gvExcursionRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvExcursionRequest.RowCommand
        Try
            If e.CommandName = "Page" Then
                Exit Sub
            End If

            If Not Request.QueryString("IsView") Is Nothing Then
                IsView = Request.QueryString("IsView")
            Else
                IsView = "0"
            End If

            Dim lblId As New Label
            Session("ExTicketNo") = txtTicketNo.Text

            Dim mCurLineNoID As Integer = 0
            For Each row As GridViewRow In gvExcursionRequest.Rows
                Dim hdnLineNo As HiddenField = CType(row.FindControl("hdnLineNo"), HiddenField)
                If hdnLineNo.Value <> "" Then mCurLineNoID = Val(hdnLineNo.Value) + 1
            Next
            Session("TempMultiCostGrid") = Nothing
            If e.CommandName = "AddRow" Then

                lblId.Text = e.CommandArgument.ToString
                Session.Add("ExConvRate", "")
                Session("ExConvRate") = txtConvRate.Text.Trim
                Dim strpop As String = ""

                If txtExcursionID.Text = "" Then
                    If hdnExcursionID.Value <> "" Then
                        Session("Excursion_ID") = hdnExcursionID.Value
                    Else
                        Session("Excursion_ID") = txtExcursionID.Text
                    End If

                    If lblId.Text = "" Then
                        lblId.Text = mCurLineNoID
                    End If

                    If checkTicketNo() = False Then
                        strpop = "window.open('DeskExcursionRequestSubEntry.aspx?State=AddRow&IsView=" + IsView + "&RefCode=" + CType(Session("ExcursionRequestRefCode"), String) + "&LineNo=" + CType(lblId.Text, String) + "&SellingType=" + CType(ddlSellingType.Value, String) + "&SpersonCode=" + CType(ddlConcierge.Value, String) + "&reqdate=" + Format(CType(txtDate.Text, Date), "yyyy/MM/dd") + "&plgrpcode=" + CType(ddlMarket.Value, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','DeskExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                        Exit Sub
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('Ticket Number Already Exists')", True)
                        Exit Sub
                    End If


                Else

                    Session("ExcursionCostDetailTable") = Nothing
                    Session("Excursion_ID") = txtExcursionID.Text

                    If lblId.Text = "" Then
                        lblId.Text = mCurLineNoID
                    End If

                    txtConvRate.Text = Session("ExConvRate")
                    Session("ExConvRate") = txtConvRate.Text.Trim
                    'strpop = "window.open('ExcursionRequestSubEntry.aspx?State=EditRow&RefCode=" + CType(Session("ExcursionRequestRefCode"), String) + "&LineNo=" + CType(lblId.Text, String) + "','ExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('DeskExcursionRequestSubEntry.aspx?State=EditRow&IsView=" + IsView + "&RefCode=" + CType(Session("ExcursionRequestRefCode"), String) + "&LineNo=" + CType(lblId.Text, String) + "&SellingType=" + CType(ddlSellingType.Value, String) + "&SpersonCode=" + CType(ddlConcierge.Value, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','DeskExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If


            ElseIf e.CommandName = "EditRow" Then
                Session("ExcursionCostDetailTable") = Nothing
                Session("Excursion_ID") = txtExcursionID.Text

                If lblId.Text = "" Then
                    lblId.Text = mCurLineNoID
                End If

                txtConvRate.Text = Session("ExConvRate")
                Session("ExConvRate") = txtConvRate.Text.Trim

                Dim strpop As String = ""
                strpop = "window.open('DeskExcursionRequestSubEntry.aspx?State=EditRow&IsView=" + IsView + "&RefCode=" + CType(Session("ManageTicketRefCode"), String) + "'&LineNo=" + CType(lblId.Text, Integer) + ",'DeskExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "ViewRow" Then
                Session("ExcursionCostDetailTable") = Nothing

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DeskExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvExcursionRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExcursionRequest.RowDataBound

        Dim btnSelect As Button
        If e.Row.RowType = DataControlRowType.DataRow Then

            If CType(Session("ExcursionRequestState"), String) = "New" Or CType(Session("ExcursionRequestState"), String) = "EditRow" Then
                btnSelect = e.Row.FindControl("btnSelect")
                btnSelect.Attributes.Add("onclick", "return FormValidation('New')")
            End If

            Dim lblAmend As Label
            Dim lblCancel As Label
            If e.Row.RowType = DataControlRowType.DataRow Then
                lblAmend = e.Row.FindControl("lblAmend")
                lblCancel = e.Row.FindControl("lblCancel")

                If lblAmend.Text = "1" Then
                    e.Row.BackColor = System.Drawing.Color.FromName("#FF934A")
                End If

                If lblCancel.Text = "1" Then
                    e.Row.BackColor = System.Drawing.Color.FromName("#ff3300")
                End If

            End If


        End If

    End Sub


    Private Sub CreateDataSourceSubEntry()
        Try
            dtExcursionDetails.Columns.Add(New DataColumn("rlineno", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("rowid", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("excid", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("tourdate", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("othgrpcode", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("othgrpname", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("guestname", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("adults", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("child", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("rateadults", GetType(Decimal)))

            dtExcursionDetails.Columns.Add(New DataColumn("ratechild", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("amount", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("amountAED", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("amend", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("cancel", GetType(Integer)))

            dtExcursionDetails.Columns.Add(New DataColumn("costRateAdult", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("costRateChild", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("costAmount", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("costAmountAED", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("cancelReason", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("complimentcust", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("complimentprov", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("comReduceAdultPer", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("comReduceChildPer", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("comReduceAmount", GetType(Decimal)))

            dtExcursionDetails.Columns.Add(New DataColumn("commpayperc", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("commpayamount", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("hotel", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("roomNo", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("providerCode", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("attn", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("conf", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("confno", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("flighttype", GetType(Char)))
            dtExcursionDetails.Columns.Add(New DataColumn("flightno", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("flighttime", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("airport", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("pickuptime", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("remarks", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("arrival", GetType(DateTime)))

            dtExcursionDetails.Columns.Add(New DataColumn("departure", GetType(DateTime)))
            dtExcursionDetails.Columns.Add(New DataColumn("locked", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("spersoncomm", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("partycode", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("guide", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("trf_required", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("trf_amount", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("trf_supplier", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("total_amount", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("incominginvno", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("debitnoteno", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("epartycode", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("othtypcode", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("confirmed", GetType(Integer)))
            dtExcursionDetails.Columns.Add(New DataColumn("othmaingrpcode", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("costCurrencyName", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("costConvRate", GetType(Decimal)))
            dtExcursionDetails.Columns.Add(New DataColumn("othtypname", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("supconf", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("exctime", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("trfno", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("supconfno", GetType(String)))
            dtExcursionDetails.Columns.Add(New DataColumn("protktno", GetType(String)))

            dtExcursionDetails.Columns.Add(New DataColumn("reminderdt", GetType(String)))
            'dtExcursionDetails.Columns.Add(New DataColumn("trf_required", GetType(String)))

            Session("DtExcursionSubEntry") = dtExcursionDetails

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
        End Try
    End Sub


    Private Sub FillGridBySubEntry()
        Try
            Dim DtExcursionSubEntry As New DataTable
            Dim DtExcursionSubEntryShow As New DataTable

            DtExcursionSubEntry = Session("DtExcursionSubEntry")

            DtExcursionSubEntryShow.Clear()
            DtExcursionSubEntryShow = Session("DtExcursionSubEntry")


            If DtExcursionSubEntry.Rows.Count > 0 Then
                'Dim FlightType As Char
                'FlightType = "A"
                'DtExcursionSubEntry.Rows.Add(1, 1, "", "", "",
                '"", "", 0, 0, 0,
                '0, 0, 0, 0, 0,
                '0, 0, 0, 0, "",
                '0, 0, 0, 0, 0,
                '0, 0, "", "", "",
                '"", "", "", FlightType, "",
                '"", "", "", "", DateTime.Now,
                'DateTime.Now, "", 0, "", "",
                '0, 0, "", 0, "", "", "")

                Dim drBlank As DataRow
                drBlank = DtExcursionSubEntryShow.NewRow()
                DtExcursionSubEntryShow.Rows.Add(drBlank)
                gvExcursionRequest.DataSource = DtExcursionSubEntryShow
                gvExcursionRequest.DataBind()
                gvExcursionRequest.Visible = True
                CalculateTotalInGrid(DtExcursionSubEntryShow)
            Else
                gvExcursionRequest.Visible = False
            End If

        Catch ex As Exception

        End Try


    End Sub

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            strSqlQry = "select * from excursions_header where excid='" & RefCode & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = SqlCmd.ExecuteReader
            If mySqlReader.Read = True Then
                If IsDBNull(mySqlReader("excid")) = False Then
                    txtExcursionID.Text = mySqlReader("excid")
                End If

                If IsDBNull(mySqlReader("requestdate")) = False Then
                    txtDate.Text = Format(CType(mySqlReader("requestdate"), Date), "dd/MM/yyyy")
                End If

                If IsDBNull(mySqlReader("ticketno")) = False Then
                    txtTicketNo.Text = mySqlReader("ticketno")
                End If

                If IsDBNull(mySqlReader("adduser")) = False Then
                    txtUser.Text = mySqlReader("adduser")
                End If

                If IsDBNull(mySqlReader("agentcode")) = False Then
                    ddlCustomer.Value = mySqlReader("agentcode")
                    txtAgent.Value = Trim(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentmast", "agentname", "agentcode", mySqlReader("agentcode")))

                    If ddlCustomer.Value = hdntouroperator.Value Then
                        touroperator.Style("display") = "block"
                        lbloperator.Style("display") = "block"

                        touroperator.Value = mySqlReader("operatorcode")

                    Else
                        touroperator.Style("display") = "none"
                        lbloperator.Style("display") = "none"
                    End If


                End If

                If IsDBNull(mySqlReader("plgrpcode")) = False Then
                    ddlMarket.Value = mySqlReader("plgrpcode")
                    hdnMarketCode.Value = mySqlReader("plgrpcode")

                End If

                If IsDBNull(mySqlReader("othsellcode")) = False Then
                    ddlSellingType.Value = mySqlReader("othsellcode")
                    hdnSellingTypeCode.Value = mySqlReader("othsellcode")
                End If

                If IsDBNull(mySqlReader("currcode")) = False Then
                    ddlCurrency.Value = mySqlReader("currcode")
                    hdnCurrencyCode.Value = mySqlReader("currcode")
                End If


                If IsDBNull(mySqlReader("convrate")) = False Then
                    txtConvRate.Text = mySqlReader("convrate")
                    hdnAgentConvRate.Value = mySqlReader("convrate")
                End If

                If IsDBNull(mySqlReader("paycode")) = False Then
                    ddlPayment.Value = mySqlReader("paycode")

                    If ddlPayment.Value = "CRE" Then
                        txtCreditCardNo.Style("display") = "block"
                        lblCreditCardNo.Style("display") = "block"
                    Else
                        txtCreditCardNo.Style("display") = "none"
                        lblCreditCardNo.Style("display") = "none"
                    End If

                    If ddlPayment.Value = "COL" Then
                        txtcollectamt.Style("display") = "block"
                        lblcollectamt.Style("display") = "block"
                    Else
                        txtcollectamt.Style("display") = "none"
                        lblcollectamt.Style("display") = "none"
                    End If


                End If

                If IsDBNull(mySqlReader("collectedamt")) = False Then
                    txtcollectamt.Text = mySqlReader("collectedamt")
                End If


                If IsDBNull(mySqlReader("incominginvno")) = False Then
                    txtCreditCardNo.Text = mySqlReader("incominginvno")
                End If

                If IsDBNull(mySqlReader("spersoncode")) = False Then
                    ddlConcierge.Value = mySqlReader("spersoncode")
                End If

                If IsDBNull(mySqlReader("spersoncode_office")) = False Then
                    ddlSalesExpert.Value = mySqlReader("spersoncode_office")
                End If

                If IsDBNull(mySqlReader("nationalitycode")) = False Then
                    ddlLanguage.Value = mySqlReader("nationalitycode")
                End If

                If IsDBNull(mySqlReader("creditnoteno")) = False Then
                    txtCreditNoteNo.Text = mySqlReader("creditnoteno")
                End If



                If IsDBNull(mySqlReader("invno")) = False Then
                    txtInvoiceNo.Text = mySqlReader("invno")
                End If

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ExcursionStopSales.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(SqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)

        End Try
    End Sub


    Private Sub ShowExcursionRequest_Details(ByVal RefCode As String)
        Dim MyDs As New DataTable
        Dim MyDsShow As New DataTable
        Try

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "select excursions_detail.rlineno,0 as rowid," & _
           "excursions_detail.excid,excursions_detail.tourdate,excursions_detail.othgrpcode," & _
           "othgrpmast.othgrpname,excursions_detail.guestname,excursions_detail.adults," & _
           "excursions_detail.child,excursions_detail.salepricea as rateadults,excursions_detail.salepriceb as ratechild," & _
           "excursions_detail.salecurrency as amount,excursions_detail.salevalue as amountAED,excursions_detail.amended as amend," & _
           "excursions_detail.cancelled as cancel,excursions_detail.costpricea as costRateAdult,excursions_detail.costpriceb as costRateChild," & _
           "excursions_detail.costcurrency as costAmount,excursions_detail.costvalue as costAmountAED,excursions_detail.cancelreason," & _
           "excursions_detail.complimentcust,excursions_detail.complimentprov,0 as comReduceAdultPer," & _
           "0 as comReduceChildPer,0 as comReduceAmount, 0 as commpayperc," & _
           "0 as commpayamount,excursions_detail.hotel,excursions_detail.roomno," & _
           "'' as providerCode,excursions_detail.attn,'' as conf,excursions_detail.confno," & _
           "excursions_detail.flighttype,excursions_detail.flightno,excursions_detail.flighttime," & _
           "excursions_detail.airport,excursions_detail.pickuptime,excursions_detail.remarks," & _
           "excursions_detail.arrdate as arrival,excursions_detail.depdate as departure,'' as locked," & _
           "excursions_detail.spersoncomm,excursions_detail.partycode,excursions_detail.guide," & _
           "excursions_detail.trf_required,excursions_detail.trf_amount,excursions_detail.trf_supplier," & _
           "excursions_detail.total_amount,excursions_detail.incominginvno,excursions_detail.debitnoteno,excursions_detail.epartycode," & _
           "excursions_detail.othtypcode,excursions_detail.confirmed,othgrpmast.othmaingrpcode as othmaingrpcode," & _
           "excursions_detail.costCurrencyName,excursions_detail.costConvRate,othtypmast.othtypname,excursions_detail.supconf,excursions_detail.exctime,excursions_detail.trfno,excursions_detail.supconfno,excursions_detail.protktno,excursions_detail.reminderdt " & _
           "from excursions_detail inner join othgrpmast on excursions_detail.othgrpcode=othgrpmast.othgrpcode " & _
           "inner join othtypmast on excursions_detail.othtypcode=othtypmast.othtypcode " & _
           "where excid='" & RefCode & "' Order By excursions_detail.rlineno"

            SqlCmd = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(MyDsShow)

            If MyDsShow.Rows.Count > 0 Then

                MyDs = MyDsShow
                'MyDsShow.Columns.Add("rlineno")
                If CType(Session("ExcursionRequestState"), String) = "ViewRow" Then
                Else
                    Dim drBlank As DataRow
                    drBlank = MyDs.NewRow()
                    'drBlank("rlineno") = MyDsShow.Rows(MyDsShow.Rows.Count - 1).Item("rlineno") + 1
                    MyDsShow.Rows.Add(drBlank)
                End If


                gvExcursionRequest.DataSource = MyDsShow
                gvExcursionRequest.DataBind()
                SqlConn.Close()
                gvExcursionRequest.Visible = True

                CalculateTotalInGrid(MyDs)
            Else
                gvExcursionRequest.Visible = False
                SqlConn.Close()

            End If

            Session("DtExcursionSubEntry") = MyDs

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("DeskExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Private Sub DisableControls()

        txtExcursionID.ReadOnly = True
        txtDate.ReadOnly = True
        ImgBtnDate.Enabled = False
        txtTicketNo.ReadOnly = True
        txtUser.ReadOnly = True
        txtAgent.Disabled = True
        ddlCustomer.Disabled = True
        ddlMarket.Disabled = True
        ddlSellingType.Disabled = True
        ' ddlCurrency.Disabled = True
        txtConvRate.Enabled = False
        ddlPayment.Disabled = True
        ddlConcierge.Disabled = True
        ddlSalesExpert.Disabled = True
        ddlLanguage.Disabled = True
        txtInvoiceNo.ReadOnly = True
        txtCreditNoteNo.Enabled = False
        txtCreditCardNo.Enabled = False
        touroperator.Disabled = True
    End Sub

    Private Function ValidateGridOneRow() As Boolean
        Try
            If gvExcursionRequest.Rows.Count = 1 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExcursionRequestEntryWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Private Function checkTicketNo() As Boolean
        Try
            Dim Result As String
            Result = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(ticketno,'')ticketno from excursions_header where ticketno='" & txtTicketNo.Text.Trim & "'")
            If Result <> "" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub CalculateTotalInGrid(ByVal Dt As DataTable)
        Dim MyDt As New DataTable
        Dim totalSaleAmt, totalSaleAmtAED, totalCostAmt, totalCostAmtAED As Decimal
        Dim i As Integer = 0
        Dim MyRows As Integer = 0
        totalSaleAmt = 0
        totalSaleAmtAED = 0
        totalCostAmt = 0
        totalCostAmtAED = 0
        MyDt = Dt

        If CType(Session("ExcursionRequestState"), String) = "ViewRow" Then
            MyRows = MyDt.Rows.Count - 1
        Else
            MyRows = MyDt.Rows.Count - 2
        End If
        Try


            If MyDt.Rows.Count > 0 Then
                For i = 0 To MyRows
                    'totalSaleAmt = totalSaleAmt + CType(MyDt.Rows(i).Item("amountAED"), Decimal)
                    'totalSaleAmtAED = totalSaleAmtAED + CType(MyDt.Rows(i).Item("amount"), Decimal)
                    'totalCostAmt = totalCostAmt + CType(MyDt.Rows(i).Item("costAmountAED"), Decimal)
                    'totalCostAmtAED = totalCostAmtAED + CType(MyDt.Rows(i).Item("costAmount"), Decimal)
                    totalSaleAmt = totalSaleAmt + CType(MyDt.Rows(i).Item("amount"), Decimal)
                    totalSaleAmtAED = totalSaleAmtAED + CType(MyDt.Rows(i).Item("amountAED"), Decimal)
                    totalCostAmt = totalCostAmt + CType(MyDt.Rows(i).Item("costAmount"), Decimal)
                    totalCostAmtAED = totalCostAmtAED + CType(MyDt.Rows(i).Item("costAmountAED"), Decimal)

                Next

                txtTotalSaleAmount.Text = Math.Round(totalSaleAmt, CType(txtdecimal.Value, Integer))
                txtTotalSaleAmountAED.Text = Math.Round(totalSaleAmtAED, CType(txtdecimal.Value, Integer))
                txtTotalCostAmount.Text = Math.Round(totalCostAmt, CType(txtdecimal.Value, Integer))
                txtTotalCostAmountAED.Text = Math.Round(totalCostAmtAED, CType(txtdecimal.Value, Integer))

            End If



        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("DeskExcursionRequestEntry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim temp As String = ""
        temp = txtAgent.Value
        txtAgent.Value = ""
        txtAgent.Value = temp
    End Sub


    Protected Sub btncopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopy.Click
        Try
            Dim dt As New DataTable
            Dim dr As DataRow

            Dim gvRow As GridViewRow


            Dim chkselect As New CheckBox
            Dim lbltransferdate As New Label
            If Session("DtExcursionSubEntry") IsNot Nothing Then
                dt = Session("DtExcursionSubEntry")


                dt.Rows(dt.Rows.Count - 1).Delete()


                For Each gvRow In gvExcursionRequest.Rows
                    '    gvTransfersRequest.DataKeys.(gvRow.RowIndex).Item("tlineno")
                    chkselect = gvRow.FindControl("chkselect")
                    If chkselect.Checked = True Then
                        dr = dt.NewRow
                        dr("rlineno") = gvExcursionRequest.Rows.Count
                        dr("rowid") = gvExcursionRequest.Rows.Count - 1  'IIf(Request.QueryString("LineNo") = "", CType(NewRLineNo, Integer), Request.QueryString("LineNo")) 'CType(NewRLineNo, Integer)
                        dr("excid") = IIf(CType(Session("ExcursionRequestRefCode"), String) = "", 1, CType(Session("ExcursionRequestRefCode"), String))
                        dr("tourdate") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("tourdate").ToString  'CType(ddlTransferDate.Text, DateTime)
                        dr("othtypcode") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("othtypcode").ToString  'CType(ddlTransferType.SelectedIndex, Integer)
                        dr("othtypname") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("othtypname").ToString  'CType(ddlStartingName.Value, String)
                        dr("guestname") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("guestname").ToString  'CType(ddlEndingName.Value, String)

                        dr("adults") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("adults").ToString 'CType(txtPickUp.Text, DateTime)

                        dr("child") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("child").ToString 'CType(txtPickUp.Text, DateTime)

                        dr("rateadults") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("rateadults").ToString  ' CType(txtPickUpTime.Text, String)

                        dr("ratechild") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("ratechild").ToString  ' CType(txtPickUpTime.Text, String)


                        dr("amount") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("amount").ToString  'CType(ddlCarTypeName.Value, String)
                        dr("amountAED") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("amountAED").ToString  'CType(txtVehicle.Text, String)


                        dr("amend") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("amend").ToString  ' IIf(Trim(txtSalesPrice.Text) = "", 0.0, CType(txtSalesPrice.Text, Decimal))
                        dr("cancel") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("cancel").ToString  ' IIf(Trim(txtSale.Text) = "", 0.0, CType(txtSale.Text, Decimal))

                        dr("costRateAdult") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("costRateAdult").ToString  ' IIf(Trim(txtSaleValue.Text) = "", 0.0, CType(txtSaleValue.Text, Decimal))


                        dr("costRateChild") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("costRateChild").ToString  ' CType(txtHotelValue.Value, String)
                        dr("costAmount") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("costAmount").ToString  ' ddlShiftHotelName.Value 'CType(txtShiftHotel.Value, String)
                        dr("costAmountAED") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("costAmountAED").ToString  ' CType(txtRoomNum.Text, String)
                        dr("cancelReason") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("cancelReason").ToString  ' CType(txtAdult.Text, Integer)
                        dr("complimentcust") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("complimentcust").ToString  'CType(txtChild.Text, Integer)
                        dr("complimentprov") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("complimentprov").ToString  ' CType(txtGuestName.Text, String)
                        dr("comReduceAdultPer") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("comReduceAdultPer").ToString
                        dr("comReduceChildPer") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("comReduceChildPer").ToString
                        dr("comReduceAmount") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("comReduceAmount").ToString  'CType(txtRemarks.Text, String)

                        dr("commpayperc") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("commpayperc").ToString  'IIf(Trim(txtHandling.Text) = "", 0.0, CType(txtHandling.Text, Decimal))
                        dr("commpayamount") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("commpayamount").ToString  ' IIf(Trim(txtHandlingValue.Text) = "", 0.0, CType(txtHandlingValue.Text, Decimal))
                        dr("hotel") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("hotel").ToString
                        dr("roomNo") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("roomNo").ToString

                        dr("providerCode") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("providerCode").ToString
                        dr("attn") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("attn").ToString
                        dr("conf") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("conf").ToString
                        dr("confno") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("confno").ToString  'CType(ddlSupplierCar.Value, String)
                        dr("flighttype") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("flighttype").ToString  'IIf(Trim(txtCostPrice.Text) = "", 0.0, CType(txtCostPrice.Text, Decimal))
                        dr("flightno") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("flightno").ToString  'IIf(CType(chp.Checked, Integer) = True, 1, 0)
                        dr("flighttime") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("flighttime").ToString  'CType(txtSupplierRef.Text, String)
                        dr("airport") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("airport").ToString  'IIf(CType(chkcompfrmsup.Checked, Integer) = True, 1, 0)

                        dr("pickuptime") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("pickuptime").ToString
                        dr("remarks") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("remarks").ToString
                        dr("arrival") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("arrival").ToString

                        dr("departure") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("departure").ToString  'CType(ddlSupplierCar.Value, String)
                        dr("locked") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("locked").ToString  'IIf(Trim(txtCostPrice.Text) = "", 0.0, CType(txtCostPrice.Text, Decimal))
                        dr("spersoncomm") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("spersoncomm").ToString  'IIf(CType(chp.Checked, Integer) = True, 1, 0)
                        dr("partycode") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("partycode").ToString  'CType(txtSupplierRef.Text, String)
                        dr("guide") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("guide").ToString  'IIf(CType(chkcompfrmsup.Checked, Integer) = True, 1, 0)

                        dr("trf_required") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("trf_required").ToString
                        dr("trf_amount") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("trf_amount").ToString
                        dr("trf_supplier") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("trf_supplier").ToString
                        dr("total_amount") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("total_amount").ToString  'CType(ddlSupplierCar.Value, String)
                        dr("incominginvno") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("incominginvno").ToString  'IIf(Trim(txtCostPrice.Text) = "", 0.0, CType(txtCostPrice.Text, Decimal))
                        dr("debitnoteno") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("debitnoteno").ToString  'IIf(CType(chp.Checked, Integer) = True, 1, 0)


                        dr("epartycode") = dt.Rows(gvRow.RowIndex).Item("epartycode").ToString
                        dr("othgrpcode") = dt.Rows(gvRow.RowIndex).Item("othgrpcode").ToString

                        dr("confirmed") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("confirmed").ToString
                        dr("othmaingrpcode") = dt.Rows(gvRow.RowIndex).Item("othmaingrpcode").ToString
                        dr("costCurrencyName") = dt.Rows(gvRow.RowIndex).Item("costCurrencyName").ToString
                        dr("costConvRate") = dt.Rows(gvRow.RowIndex).Item("costConvRate").ToString
                        dr("othtypname") = dt.Rows(gvRow.RowIndex).Item("othtypname").ToString
                        dr("supconf") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("supconf").ToString  'IIf(CType(chp.Checked, Integer) = True, 1, 0)
                        dr("exctime") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("exctime").ToString  'CType(txtSupplierRef.Text, String)
                        dr("trfno") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("trfno").ToString  'IIf(CType(chkcompfrmsup.Checked, Integer) = True, 1, 0)

                        dr("supconfno") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("supconfno").ToString
                        dr("protktno") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("protktno").ToString

                        dr("reminderdt") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("reminderdt").ToString

                        'dr("trf_required") = gvExcursionRequest.DataKeys(gvRow.RowIndex).Item("trf_required").ToString


                        dt.Rows.Add(dr)
                        dr = dt.NewRow
                        dt.Rows.Add(dr)
                        gvExcursionRequest.DataSource = dt
                        dtExcursionDetails = dt
                        Session("DtExcursionSubEntry") = dt
                        gvExcursionRequest.DataBind()
                        CalculateTotalInGrid(dt)
                        Exit Sub

                    End If


                Next

            End If


        Catch ex As Exception

        End Try
    End Sub

    Public Function SendMailtotrfdept(ByVal requestid As String) As Boolean
        Dim custcode As String = ""
        Dim subuser As String = ""
        Dim type As String = ""
        Dim strSql As String = ""
        Dim toCC As String = ""
        Dim fromMail As String = ""
        Dim toMail As String = ""
        Dim ds As DataSet = New DataSet()
        Dim strcust As String = ""
        Dim subject As String = ""
        Dim ReqId As String = requestid
        Dim ReqDate As String = ""
        Dim blnConfirmaion As Boolean = False
        Dim blnMailSend As Boolean = False
        Dim strAttachment As String = ""


        Dim rnd As Random = New Random
        Dim strFilename As String = ""
        Dim strFullpath As String = ""
        'Dim strToCC As String = ""
        Dim ToAdd As String = ""
        'Dim strSubject As String = ""

        Dim strMessage As String = ""
        Dim objEmail As New clsEmail
        Dim ContactPerson As String = ""

        'strSql = "select CONVERT(varchar,requestdate,103) from reservation_headernew where requestid='" + ReqId + "'"
        'ReqDate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSql)

        Try


            'Commented by Archana on 19/03/2015 

            Dim mLoggedOnUser As String = CType(Session("GlobalUserName"), String)

            fromMail = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select usemail  from UserMaster where UserCode='" + mLoggedOnUser + "'")
            strMessage = "Desk Excursion booking added by " + CType(Session("GlobalUserName"), String) + " Booking Ref No : " + hdnExcursionID.Value.Trim
            If Trim(fromMail) <> "" Then
                toCC = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =702")
                If objEmail.SendCDOMessage("", fromMail, toCC, fromMail, "Desk Excursion Booking No - " + hdnExcursionID.Value.Trim, strMessage) Then
                    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + ToAdd + "');", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + ToAdd + "');", True)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Email Id doesnt exists to " + ContactPerson + "');", True)
            End If

            SendMailtotrfdept = True

        Catch ex As Exception
            SendMailtotrfdept = False

        End Try


        'SendMailtotrfdept = True
    End Function
End Class
