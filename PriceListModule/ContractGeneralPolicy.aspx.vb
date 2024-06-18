Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class PriceListModule_ContractGenPolicy
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime
    Private cnt As Long
 

    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Private dt As New DataTable


    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
 
 
 

 




#End Region

#Region "Enum GridCol"
    Enum GridCol

        gneralpolicyid = 1
        Fromdate = 2
        Todate = 3
        applicableto = 4
        Edit = 5
        View = 6
        Delete = 7
        Copy = 8
        DateCreated = 9
        UserCreated = 10
        DateModified = 11
        UserModified = 12


    End Enum
#End Region
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "Numbers/lock text"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub LockText(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return chkTextLock(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        ' clearall()
        Panelsearch.Enabled = True
        Session("GV_HotelData") = Nothing
        PanelMain.Style("display") = "none"
        'Panelsearch.Style("display")="block")

        lblHeading.Text = "General Policy  -" + ViewState("hotelname") + " - " + hdncontractid.Value
        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "CONTRACTGENERALPOLICY", CType(Session("ContractState"), String))
        Response.Redirect(Request.RawUrl)
    End Sub




    Private Function ValidateSave() As Boolean
   
        If txtfromDate.Text <> "" And txtToDate.Text <> "" Then

            If Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") <= Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date should be greater than From Date ');", True)
                txtToDate.Text = ""
                SetFocus(txtToDate)
                ValidateSave = False
                Exit Function
            End If



            If Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & "  ');", True)
                txtfromDate.Text = ""
                SetFocus(txtfromDate)
                ValidateSave = False
                Exit Function
            End If
            If Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") < Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnconfromdate.Value), Date), "yyyy/MM/dd") Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & " ');", True)
                txtfromDate.Text = ""
                SetFocus(txtfromDate)
                ValidateSave = False
                Exit Function
            End If

            If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value), Date), "yyyy/MM/dd")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belong to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & " ');", True)
                txtToDate.Text = ""
                txtfromDate.Text = ""
                SetFocus(txtfromDate)
                ValidateSave = False
                Exit Function
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date & To Date  Should not be Blank');", True)
            SetFocus(txtfromDate)
            ValidateSave = False
            Exit Function
        End If



        If txtApplicableTo.Text = "" Or txtApplicableTo.Text = " " Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable To Should not be Blank');", True)
            ValidateSave = False
            SetFocus(txtApplicableTo)
            Exit Function
        End If

        If txtpolicy.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Policy Text Should not be Blank');", True)
            ValidateSave = False
            SetFocus(txtpolicy)
            Exit Function
        End If



        ValidateSave = True
    End Function
    Private Sub ShowRecord(ByVal RefCode As String)

        Try

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_genpolicy_header(nolock) Where genpolicyid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("genpolicyid")) = False Then
                        txtplistcode.Text = CType(mySqlReader("genpolicyid"), String)
                    End If

                    If IsDBNull(mySqlReader("contractid")) = False And ViewState("CopyFrom") Is Nothing = True Then
                        hdncontractid.Value = CType(mySqlReader("contractid"), String)
                    End If
                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",     ", ","), String)


                    End If
                    If IsDBNull(mySqlReader("policytext")) = False Then
                        txtpolicy.Text = CType(mySqlReader("policytext"), String)
                 
                    End If

                    If IsDBNull(mySqlReader("todate")) = False Then
                        txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                    End If

                    If IsDBNull(mySqlReader("fromdate")) = False Then

                        txtfromDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                    End If
                    'If IsDBNull(mySqlReader("fromdate")) = False Then
                    '    txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                    '    txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",     ", ","), String)
                    'End If
                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_genpolicy_header(nolock) where  genpolicyid ='" & CType(RefCode, String) & "'") <> "" Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "UNAPPROVED"

                    Else
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "APPROVED"
                    End If

                End If
            End If


            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub

    Private Sub DisableControl()
        If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

            txtpolicy.Enabled = True
            txtfromDate.Enabled = True
            txtToDate.Enabled = True

            txtApplicableTo.Enabled = True
            txtplistcode.Text = ""
            If ViewState("State") = "New" Then
                txtfromDate.Text = Now.ToString("dd/MM/yyyy")
                txtToDate.Text = Now.ToString("dd/MM/yyyy")
                txtpolicy.Text = ""
            End If
            wucCountrygroup.Disable(True)
        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then



            dpFromDate.Enabled = False
            dpToDate.Enabled = False
            wucCountrygroup.Disable(False)
            txtApplicableTo.Enabled = False

            txtpolicy.Enabled = False
            txtfromDate.Enabled = False
            txtToDate.Enabled = False


        ElseIf ViewState("State") = "Edit" Then

            dpFromDate.Enabled = True
            dpToDate.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            txtpolicy.Enabled = True
            txtfromDate.Enabled = True
            txtToDate.Enabled = True


        End If
    End Sub
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()
        '        FillGrid("genpolicyid", hdnpartycode.Value, "Desc")


    End Sub
    Public Function checkforexisting() As Boolean

        checkforexisting = True
        Try
            If FindDatePeriod() = False Then
                checkforexisting = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

#End Region

    Public Function FindDatePeriod() As Boolean
      


        Dim strMsg As String = ""

        FindDatePeriod = True
        Try

            '   CopyRow = 0

            Dim weekdaystr As String = ""

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            'For Each GVRow In grdDates.Rows



                Dim ds As DataSet
                Dim parms2 As New List(Of SqlParameter)
            Dim parm2(8) As SqlParameter

            parm2(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
            parm2(2) = New SqlParameter("@fromdate", Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd"))
            parm2(3) = New SqlParameter("@todate", Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"))

                parm2(4) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
            parm2(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
            parm2(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                parm2(7) = New SqlParameter("@promotionid", "")
        

            For i = 0 To 7
                parms2.Add(parm2(i))
            Next



                ds = New DataSet()
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkgenpolicy", parms2)


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("genpolicyid")) = False Then
                        strMsg = "General Policy already exists For this Contract  " + CType(hdncontractid.Value, String) + " - Policy Id " + ds.Tables(0).Rows(0)("genpolicyid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        FindDatePeriod = False
                        Exit Function
                    End If
                    End If
                End If







        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If

            Dim lbltran As Label
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblgenpolicyid")
            If lbltran.Text.Trim = "" Then Exit Sub

            If e.CommandName <> "View" Then

                If Session("Calledfrom") = "Offers" Then
                    'Dim offerexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_offers_header(nolock) where promotionid='" & hdn.Value & "'")

                    'If offerexists <> "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Offer Main Details First');", True)
                    '    Exit Sub

                    'End If
                Else
                    Dim contexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_contracts(nolock) where contractid='" & hdncontractid.Value & "'")

                    If contexists Is Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Contract Main Details First');", True)
                        Exit Sub

                    End If
                End If
            End If


            If e.CommandName = "EditRow" Then
                ViewState("State") = "Edit"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ' Showdetailsgrid(CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()

                btnSave.Visible = True
                btnSave.Text = "Update"
                lblHeading.Text = "Edit General Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " General Policy  "
            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ' Showdetailsgrid(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = False
                lblHeading.Text = "View General Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " General Policy "
            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Visible = True
                ViewState("State") = "Delete"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                'Showdetailsgrid(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                lblHeading.Text = "Delete General Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " General Policy "
            ElseIf e.CommandName = "Copy" Then
                PanelMain.Visible = True
                ViewState("State") = "Copy"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ' Showdetailsgrid(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy General Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " General Policy "
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("ContractGenPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub grdviewrates_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdviewrates.RowCommand

        Try
        
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
        Dim lbltran As Label
        Dim lblcontract As Label
        lbltran = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
        lblcontract = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcontract")
        If lbltran.Text.Trim = "" Then Exit Sub
        If e.CommandName = "Select" Then
            hdncopycontractid.Value = CType(lblcontract.Text, String)
            PanelMain.Visible = True
            ViewState("CopyFrom") = "CopyFrom"
            ViewState("State") = "Copy"
            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
            PanelMain.Style("display") = "block"

            ' wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))


            ShowRecord(CType(lbltran.Text.Trim, String))

            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")

            wucCountrygroup.sbShowCountry()

            btnSave.Visible = True
            txtplistcode.Text = ""
            btnSave.Text = "Save"
            lblHeading.Text = "Copy General Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
            Page.Title = "General Policy "
            fillseason()


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractGenPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try


            strSqlQry = "select h.contractid as contractid , h.genpolicyid as plistcode, h.fromdate as fromdate,h.todate as todate, h.applicableto as applicableto  from view_contracts_genpolicy_header h(nolock) ,view_contracts_search s " _
                       & "  where isnull(s.withdraw,0)=0  and   h.contractid <>'" & hdncontractid.Value & "' and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "'"


            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myds)
            grdviewrates.DataSource = myds

            If myds.Tables(0).Rows.Count > 0 Then
                grdviewrates.DataBind()
            Else
                grdviewrates.PageIndex = 0
                grdviewrates.DataBind()

            End If


            ModalViewrates.Show()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Protected Sub ReadMoreLinkButtoncopycont_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If
            ModalViewrates.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            Dim strMsg As String = ""
            If Page.IsValid = True Then

                If ViewState("State") = "New" Or ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then

                    If ValidateSave() = False Then
                        Exit Sub
                    End If
                    If checkforexisting() = False Then
                        Exit Sub
                    End If

                    If chkctrygrp.Checked = True And Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If

                    If chkctrygrp.Checked = True And Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start


                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_General", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""



                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    '''''''''''''''''''''''


                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("GENPOL", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_genpolicy_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@genpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)

                        mySqlCmd.Parameters.Add(New SqlParameter("@policytext", SqlDbType.VarChar, -1)).Value = CType(Replace(txtpolicy.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                'command disposed

                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_contracts_genpolicy_header ", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@genpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""

                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)

                        mySqlCmd.Parameters.Add(New SqlParameter("@policytext", SqlDbType.VarChar, -1)).Value = CType(txtpolicy.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If



                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_genpolicy_agents  Where genpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_genpolicy_countries Where genpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then


                                mySqlCmd = New SqlCommand("sp_add_contracts_genpolicy_countries", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@genpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next

                    End If

                    If wucCountrygroup.checkagentlist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                        For i = 0 To arragents.Length - 1

                            If arragents(i) <> "" Then

                                mySqlCmd = New SqlCommand("sp_add_contracts_genpolicy_agents", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@genpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next
                    End If





                    ' ''mySqlCmd = New SqlCommand("sp_add_editpendforapprove", mySqlConn, sqlTrans)
                    ' ''mySqlCmd.CommandType = CommandType.StoredProcedure

                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 30)).Value = "edit_contracts_genpolicy_header"
                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@markets", SqlDbType.VarChar, 50)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 30)).Value = CType(txtplistcode.Text.Trim, String)

                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = txthotelname.Text
                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 100)).Value = hdnpartycode.Value
                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@moddate ", SqlDbType.DateTime)).Value = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "yyyy/MM/dd")
                    ' ''mySqlCmd.Parameters.Add(New SqlParameter("@pricecode", SqlDbType.VarChar, 100)).Value = ""
                    ' ''mySqlCmd.ExecuteNonQuery()



                    strMsg = "Saved Succesfully!!"
                ElseIf ViewState("State") = "Delete" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_General", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""



                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    '''''''''''''''''''''''


                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_contracts_genpolicy_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@genpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    strMsg = "Deleted  Succesfully!!"


                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed

                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close


                ViewState("State") = ""
                wucCountrygroup.clearsessions()
                btnReset_Click(sender, e)
                FillGrid("genpolicyid", hdnpartycode.Value, "DESC")

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region
    Private Sub fillseason()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select min(c.applicableto)applicableto,min(c.fromdate) fromdate,max(c.todate) todate from view_contracts_search c(nolock) Where c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    ' If ViewState("State") = "New" Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",    ", ","), String)

                    End If
                    'End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        hdnconfromdate.Value = Format(mySqlReader("fromdate"), "dd/MM/yyyy")


                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        hdncontodate.Value = Format(mySqlReader("todate"), "dd/MM/yyyy")


                    End If

                End If

            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()

            mySqlConn.Close()


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        ViewState("State") = "New"


        PanelMain.Visible = True
        PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False
        Session("contractid") = hdncontractid.Value
        wucCountrygroup.Visible = True
        wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
        wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
        fillseason()
        DisableControl()
        lblstatus.Visible = False
        lblstatustext.Visible = False


        txtfromDate.Text = hdnconfromdate.Value
        txtToDate.Text = hdncontodate.Value

        ' filldaysgrid()
        'FillMealplans()
        ' seasonsgridfill()
        wucCountrygroup.Visible = True

        'divcopy1.Style("display") = "none"



        'btncopyratesnextrow.Visible = True

        'lable12.Visible = True
        btnfillrate.Visible = False
        txtfillrate.Visible = False
        'fillroomgrid(grdRoomrates, True)

        'createdatacolumns()
        ' FillRoomdetails()

        btnSave.Visible = True

        btnSave.Text = "Save"
        lblHeading.Text = "New General Policy - " + ViewState("hotelname")
        Page.Title = Page.Title + " " + " General Policy -" + ViewState("hotelname")

        ' divuser.Style("display") = "none"
        Session("isAutoTick_wuccountrygroupusercontrol") = 1
        wucCountrygroup.sbShowCountry()



    End Sub
#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub
    'Protected Sub btnClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear1.Click
    '    ViewState("noshowclick") = Nothing
    '    ''  ModalExtraPopup.Hide()
    'End Sub
    Sub FillRoomdetails()
        ''  createdatatable()

        ''   grdRoomrates.Visible = True

        ''   lable12.Visible = True
        ''   btncopyratesnextrow.Visible = True
        ' grdWeekDays.Enabled = False


        btnfillrate.Visible = True
        txtfillrate.Visible = True

    End Sub


    Protected Sub btnClearPolicy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtViewPolicy.Value = ""

    End Sub

    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentListSearch(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lsAgentNames As New List(Of String)
        Dim lsCountryList As String
        Try

            'strSqlQry = "select agentname from agentmast where active=1 and agentname like  '" & prefixText & "%'"
            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList
            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
                End If

            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'lsAgentNames.Add(myDS.Tables(0).Rows(i)("agentname").ToString())
                    lsAgentNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next

            End If

            Return lsAgentNames
        Catch ex As Exception

            Return lsAgentNames
        End Try

    End Function
#End Region

    Private Sub FillGrid(ByVal StrOrderby As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try

            If StrOrderby = "fromdate" Or StrOrderby = "todate" Then
                strSqlQry = "select genpolicyid,fromdate,todate,applicableto,adddate,adduser,moddate,moduser  from view_contracts_genpolicy_header  " _
                  & " where contractid='" & hdncontractid.Value & "' order by  convert(datetime," & StrOrderby & " ,103) " & strsortorder & "  "

            Else

                strSqlQry = "select genpolicyid,fromdate,todate,applicableto,adddate,adduser,moddate,moduser  from view_contracts_genpolicy_header  " _
                            & " where contractid='" & hdncontractid.Value & "' order by " & StrOrderby & " " & strsortorder & "  "
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGeneralPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Function SetVisibility(ByVal desc As Object, ByVal maxlen As Integer) As Boolean

        If desc.ToString = "" Then
            Return False
        Else
            If desc.ToString.Length > maxlen Then
                Return True
            Else
                Return False
            End If
        End If


    End Function
    Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractGenPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Function Limit(ByVal desc As Object, ByVal maxlen As Integer) As String

        If desc.ToString = "" Then
            Return ""
        Else
            If desc.ToString.Length > maxlen Then
                desc = desc.Substring(0, maxlen)
            Else

                desc = desc
            End If
        End If

        Return desc


    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Dim CalledfromValue As String = ""

        Dim Gappid As String = ""
        Dim Gappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""

        If IsPostBack = False Then
            Gappid = 1
            Gappname = objUser.GetAppName(Session("dbconnectionName"), Gappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else


                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                CalledfromValue = Me.SubMenuUserControl1.menuidval
                objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(Gappname, String), "ContractGeneralPolicy.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
             btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)

            End If

            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractGeneralPolicy.aspx", Gappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, Gappid, intMenuID)
            If functionalrights <> "" Then

                strTempUserFunctionalRight = functionalrights.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strRights = strTempUserFunctionalRight.GetValue(lngCount)

                    If strRights = "07" Then
                        Count = 1
                    End If
                Next



                If Count = 1 Then
                    btncopycontract.Visible = True
                Else
                    btncopycontract.Visible = False
                End If

            Else

                btncopycontract.Visible = False

            End If

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))


            txtconnection.Value = Session("dbconnectionName")
            txtfromDate.Text = Now.ToString("dd/MM/yyyy")
            txtToDate.Text = Now.ToString("dd/MM/yyyy")


            hdnpartycode.Value = CType(Session("Contractparty"), String)
            hdncontractid.Value = CType(Session("contractid"), String)

            'Session("contractid") = hdncontractid.Value
            'Session("partycode") = hdnpartycode.Value

            SubMenuUserControl1.partyval = hdnpartycode.Value
            SubMenuUserControl1.contractval = CType(Session("contractid"), String)
            '  wucCountrygroup.Visible = False

            wucCountrygroup.sbSetPageState("", "CONTRACTGENERALPOLICY", CType(Session("ContractState"), String))


            '   hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
            txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
            ViewState("hotelname") = txthotelname.Text
            Session("partycode") = hdnpartycode.Value
            lblHeading.Text = lblHeading.Text + " - " + ViewState("hotelname") + " - " + hdncontractid.Value

            Page.Title = "General Policy"
            ddlorder.SelectedIndex = 0
            ddlorderby.SelectedIndex = 1
            'lblbookingvaltype.Visible = False
            'ddlBookingValidity.Visible = False





            'FillGrid(hdncontractid.Value, hdnpartycode.Value, "Desc")
            FillGrid("genpolicyid", hdnpartycode.Value, "DESC")
            '  PanelMain.Visible = False

            'btnCancel.Attributes.Add("onclick", "javascript :if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            End If
        Else
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
        End If
            txtfromDate.Attributes.Add("onchange", "setdate();")
            txtToDate.Attributes.Add("onchange", "checkdates('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "');")
            txtfromDate.Attributes.Add("onchange", "checkfromdates('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "');")

            chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")

            btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")

            Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub

    Protected Sub txtfromDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtfromDate.TextChanged



    End Sub
    Public Sub sortgvsearch()
        Select Case ddlorder.SelectedIndex
            Case 0
                FillGrid("genpolicyid", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 1
                FillGrid("fromdate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 2
                FillGrid("todate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 3
                FillGrid("applicableto", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 4
                FillGrid("adddate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 5
                FillGrid("adduser", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 6
                FillGrid("moddate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 7
                FillGrid("moduser", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
        End Select
    End Sub

    Protected Sub ddlorder_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub btnAddNew_Load(sender As Object, e As System.EventArgs) Handles btnAddNew.Load

    End Sub
End Class
