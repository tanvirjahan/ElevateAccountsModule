

Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Partial Class ExcSuppCountryGrps
    Inherits System.Web.UI.Page

    Private Connection As SqlConnection
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime


    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter


#End Region

#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub

    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList

            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                'If HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl") IsNot Nothing Then 'changed by mohamed on 03/10/2016 - instead of selected, used all
                'lsCountryList = HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    'strSqlQry += " and a.ctrycode in (" & lsCountryList & ")" 'changed by mohamed on 03/10/2016 -'commented this line to show all the agents.
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
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        ' clearall()
        BtnCancelsearch.Visible = True
        Panelsearch.Enabled = True
        ' Session("GV_HotelData") = Nothing
        PanelCtryGrps.Style("display") = "none"
        btnSave.Visible = False
        btnreset.Visible = False
        'Panelsearch.Style("display")="block")

        'lblHeading.Text = "General Policy  -" + ViewState("hotelname") + " - " + hdncontractid.Value
        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "ExcSuppCtryGroups", CType(Session("ExcTypesState"), String))
        Response.Redirect(Request.RawUrl)
    End Sub
#Region "Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click



        Try
            If Page.IsValid = True Then
                If Session("ExcTypesState").ToString() = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("State").ToString() = "Edit" Then

                    '    'check  If ValidateEmail() = False Then
                    '    Exit Sub
                    'End If

                    If wucCountrygroup.checkcountrylist.ToString <> "" Or wucCountrygroup.checkagentlist.ToString <> "" Then
                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                        mySqlCmd = New SqlCommand("sp_mod_exctypctrygrps ", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtpartycode.Text.Trim, String)


                        If wucCountrygroup.checkcountrylist.ToString.Trim <> "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@countries", SqlDbType.VarChar, 8000)).Value = wucCountrygroup.checkcountrylist.ToString.Trim
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@countries", SqlDbType.VarChar, 8000)).Value = ""
                        End If

                        If wucCountrygroup.checkagentlist.ToString.Trim <> "" Then

                            mySqlCmd.Parameters.Add(New SqlParameter("@agents", SqlDbType.VarChar, 8000)).Value = wucCountrygroup.checkagentlist.ToString.Trim
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@agents", SqlDbType.VarChar, 8000)).Value = ""
                        End If
                        ''Value in hdn variable , so splting to get string correctly
                        'Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                        'For i = 0 To arragents.Length - 1

                        '    If arragents(i) <> "" Then
                        '        mySqlCmd.Parameters.Add(New SqlParameter("@agents", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)
                        '    Else
                        '        mySqlCmd.Parameters.Add(New SqlParameter("@agents", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        '    End If
                        'Next


                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose() 'command disposed
                    End If


                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                ElseIf Session("State") = "Delete" Then
                    'check If checkForDeletion() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_exctypctrygrps", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtpartycode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()


                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If Session("ExcTypesState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("ExcTypesState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("ExcTypesState") = "Delete" Then

                    Dim strscript As String = ""

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Deleted Successfully.');", True)

                    strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If

                ViewState("State") = ""
                wucCountrygroup.clearsessions()
                btnReset_Click(sender, e)
                BtnCancelsearch.Visible = True
                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
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
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=excursionsupp','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub gv_SearchResult_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            BtnCancelsearch.Visible = False
            If e.CommandName = "More" Then Exit Sub
            Dim lbltran As Label
            Dim lblpartyname As Label
            Dim lblpartycode As Label

            lblpartyname = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpartyname")
            lblpartycode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpartycode")
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltranid")
            If lbltran.Text.Trim = "" Then Exit Sub
            txtpartycode.Text = lblpartycode.Text
            txtpartyname.Text = lblpartyname.Text
            lblSuppliercode.Visible = True
            lblSuppliername.Visible = True
            txtpartyname.Visible = True
            txtpartycode.Visible = True

            If e.CommandName = "View" Then
                Session("State") = "View"
                PanelCtryGrps.Style("display") = "block"
                Panelsearch.Enabled = False
                PanelCtryGrps.Enabled = False
                btnSave.Visible = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim & "/" & lblpartycode.Text.Trim, Nothing, ViewState("State"))
                ShowRecord(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnreset.Visible = True
                btnSave.Visible = False

                Page.Title = Page.Title + " " + " View Supplier Country Groups "
            ElseIf e.CommandName = "EditRow" Then
                PanelCtryGrps.Style("display") = "block"
                PanelCtryGrps.Enabled = True
                Session("State") = "Edit"
                btnSave.Visible = True
                btnreset.Visible = True
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim & "/" & lblpartycode.Text.Trim, Nothing, ViewState("State"))
                ' fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ' Showdetailsgrid(CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()


                btnSave.Visible = True
                btnSave.Text = "Update"
                'lblHeading.Text = "Edit General Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + "Edit Supplier Country Groups "
            ElseIf e.CommandName = "DeleteRow" Then
                PanelCtryGrps.Visible = True
                Session("State") = "Delete"
                PanelCtryGrps.Style("display") = "block"
                Panelsearch.Enabled = False
                PanelCtryGrps.Enabled = False

                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim & "/" & lblpartycode.Text.Trim, Nothing, ViewState("State"))

                ShowRecord(CType(lbltran.Text.Trim, String))

                btnreset.Visible = True
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()


                btnSave.Visible = True
                btnSave.Text = "Delete"
                '  lblHeading.Text = "Delete Supplier Country Groups - " + ViewState("hotelname") + " - " + txtpartyname.Text
                Page.Title = Page.Title + " " + " Delete Supplier Country Groups "

                'Else

                '    PanelCtryGrps.Style("display") = "none"

            End If

        Catch ex As Exception
            objUtils.WritErrorLog("ExcSuppCtryGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblcountries"), Label)
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
            objUtils.WritErrorLog("ContractsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub ReadMoreLinkButtonAgt_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblAgents"), Label)
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
            '  ModalExtraPopup1.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub FillGrid(ByVal strorderby As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")

        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If



        strSqlQry = ""
        Try

            ' If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then

            strSqlQry = "select e.exctypcode,e.partycode,p.partyname,e.countries ,e.agents,e.adddate,e.adduser,e.moddate,e.moduser from excursiontypes_suppliers  e  join  partymast p  on e.partycode=p.partycode and e.exctypcode='" & txtCustomerCode.Value & "'"





            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

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
            objUtils.WritErrorLog("ExcSuppCtryGrps.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    'Protected Sub gv_SearchResult_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
    '    Try
    '        If e.CommandName = "More" Then Exit Sub
    '        Dim lbltran As Label
    '        Dim lblpartyname As Label
    '        Dim lblpartycode As Label

    '        lblpartyname = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpartyname")
    '        lblpartycode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpartycode")
    '        lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltranid")
    '        If lbltran.Text.Trim = "" Then Exit Sub
    '        'txtpartycode.Text = lblpartycode.Text
    '        'txtpartyname.Text = lblpartyname.Text


    '        If e.CommandName = "View" Then
    '            Session("ExcTypesState") = "View"
    '            PanelCtryGrps.Visible = True
    '            PanelCtryGrps.Style("display") = "block"
    '            Panelsearch.Enabled = False
    '            PanelCtryGrps.Enabled = False
    '            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
    '            wucCountrygroup.sbSetPageState(lbltran.Text.Trim & "/" & lblpartycode.Text.Trim, Nothing, ViewState("State"))

    '            ShowRecord(CType(lbltran.Text.Trim, String))


    '            Session("isAutoTick_wuccountrygroupusercontrol") = 1
    '            wucCountrygroup.sbShowCountry()
    '            DisableControl()
    '            btnreset.Visible = True
    '            btnSave.Visible = False
    '            'btnSave.Text = "Update"
    '            ' lblHeading.Text = "Edit Commission - " + ViewState("hotelname") + " - " + hdncontractid.Value
    '            Page.Title = Page.Title + " " + " View Supplier Country Groups "
    '        ElseIf e.CommandName = "EditRow" Then
    '            Session("ExcTypesState") = "Edit"
    '            PanelCtryGrps.Visible = True

    '            Panelsearch.Enabled = True
    '            btnSave.Visible = True
    '            btnreset.Visible = True
    '            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
    '            wucCountrygroup.sbSetPageState(lbltran.Text.Trim & "/" & lblpartycode.Text.Trim, Nothing, ViewState("State"))
    '            ' fillseason()
    '            ShowRecord(CType(lbltran.Text.Trim, String))
    '            ' Showdetailsgrid(CType(lbltran.Text.Trim, String))

    '            Session("isAutoTick_wuccountrygroupusercontrol") = 1
    '            wucCountrygroup.sbShowCountry()
    '            DisableControl()

    '            btnSave.Visible = True
    '            btnSave.Text = "Update"
    '            'lblHeading.Text = "Edit General Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
    '            Page.Title = Page.Title + " " + "Edit Supplier Country Groups "
    '        ElseIf e.CommandName = "DeleteRow" Then
    '            PanelCtryGrps.Visible = True
    '            Session("ExcTypesState") = "Delete"
    '            PanelCtryGrps.Style("display") = "block"
    '            Panelsearch.Enabled = False
    '            PanelCtryGrps.Enabled = False

    '            Session.Add("RefCode", CType(lbltran.Text.Trim, String))
    '            wucCountrygroup.sbSetPageState(lbltran.Text.Trim & "/" & lblpartycode.Text.Trim, Nothing, ViewState("State"))

    '            ShowRecord(CType(lbltran.Text.Trim, String))

    '            btnreset.Visible = True
    '            Session("isAutoTick_wuccountrygroupusercontrol") = 1
    '            wucCountrygroup.sbShowCountry()

    '            DisableControl()
    '            btnSave.Visible = True
    '            btnSave.Text = "Delete"
    '            '  lblHeading.Text = "Delete Supplier Country Groups - " + ViewState("hotelname") + " - " + txtpartyname.Text
    '            Page.Title = Page.Title + " " + " Delete Supplier Country Groups "

    '        Else

    '            PanelCtryGrps.Style("display") = "none"

    '        End If

    '    Catch ex As Exception
    '        objUtils.WritErrorLog("ExcSuppCtryGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try
    'End Sub
    Private Sub DisableControl()


        If ViewState("State") = "View" Or ViewState("State") = "Delete" Then



            wucCountrygroup.Disable(True)

        ElseIf ViewState("State") = "EditRow" Then
            lblSuppliercode.Visible = True
            lblSuppliername.Visible = True
            txtpartycode.Visible = True
            txtpartyname.Visible = True

            wucCountrygroup.Disable(False)

        End If
    End Sub




#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("ExcursionModule\ExcursionSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            txtCustomerCode.Disabled = True
            txtCustomerName.Disabled = True
            'PanelCtryGrps.Visible = False
            btnSave.Visible = False
            btnreset.Visible = False
            lblSuppliercode.Visible = False
            lblSuppliername.Visible = False
            txtpartycode.Visible = False
            txtpartyname.Visible = False
            PanelCtryGrps.Style("display") = "none"
            wucCountrygroup.sbSetPageState("", "ExcSuppCtryGroups", Nothing)
            If CType(Session("ExcTypesState"), String) = "Edit" Then

                btnSave.Text = "Update"
               '
                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                FillGrid("e.exctypcode", txtCustomerCode.Value, "Desc")

                lblmainheading.Text = "Edit Excursion -  Supplier Country Details"
                Page.Title = Page.Title + " " + "Edit Excursion - Supplier  Country Details"
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer reservation details?')==false)return false;")
            ElseIf CType(Session("ExcTypesState"), String) = "View" Then

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                FillGrid("e.exctypcode", txtCustomerCode.Value, "Desc")
                PanelCtryGrps.Enabled = False
                lblmainheading.Text = "View Excursion - Supplier Details"
                Page.Title = Page.Title + " " + "View Excursion - Supplier Details"
                btnSave.Visible = False
                'btnreset.Visible = False
                'btnreset.Visible = True
                btnreset.Text = "Return to Search"
                btnreset.Focus()

            ElseIf CType(Session("ExcTypesState"), String) = "Delete" Then
                'btnSave.Visible = True
                'btnreset.Visible = True
                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                FillGrid("e.exctypcode", txtCustomerCode.Value, "Desc")
                PanelCtryGrps.Enabled = False
                lblmainheading.Text = "Delete Excursion - Supplier Details"
                Page.Title = Page.Title + " " + "Delete Excursion - Supplier Details"
                btnSave.Text = "Delete"
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer reservation details?')==false)return false;")

            End If
            btnreset.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

        End If
        ' Session.Add("submenuuser", "CustomersSearch.aspx")
        Session.Add("submenuuser", "ExcursionGroups.aspx")
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
                        Me.txtCustomerCode.Value = mySqlReader("exctypcode")
                        Me.txtCustomerName.Value = mySqlReader("exctypname")
                    End If
                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()
            mySqlCmd.Dispose()
            mySqlReader.Close()
          
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcSectorMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

    Protected Sub BtnCancel_Click(sender As Object, e As System.EventArgs) Handles BtnCancelsearch.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnresetsearch_Click(sender As Object, e As System.EventArgs) Handles BtnCancelsearch.Click
        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExcursionSuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub
End Class
