#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class TransportModule_TransferPriceList
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim objDateTime As New clsDateTime
    Dim myDataAdapter As SqlDataAdapter

    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
#End Region

#Region "Enum GridCol"
    Enum GridCol
        PListCodeTCol = 0
        PListCode = 1
        Suppplier = 2
        Market = 3
        SubSeason = 4
        Group = 5
        Currency = 6
        FromDate = 7
        ToDate = 8
        Active = 9
        DateCreated = 10
        UserCreated = 11
        DateModified = 12
        UserModified = 13
        Edit = 14
        View = 15
        Delete = 16
        Copy = 17
    End Enum
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""

                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If


                Session("OthPListFilter") = Request.Params("Type")
                SetFocus(TxtPLCD)
                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "ExcursionModule\ExcursionCostPriceListSearch.aspx?Type=" + Session("OthPListFilter"), btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                checkIsPrivilege()

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcd, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpnm, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcd, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id  in ('1021','1105')) order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpnm, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode in (select option_selected from reservation_parameters where param_id in ('1021','1105')) order by othgrpname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)

               
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellcd, "partycode", "partyname", "select partycode,partyname from partymast where   active=1 and partymast.sptypecode <>(select option_selected from reservation_parameters where param_id='458') order by partycode ", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "partyname", "partycode", "select partycode,partyname from partymast where active=1 order by partycode", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellcd, "partycode", "partyname", "select partycode,partyname from partymast where   active=1 and partymast.sptypecode not in (select option_selected from reservation_parameters where param_id IN('458','500')) order by partycode ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "partyname", "partycode", "select partycode,partyname from partymast where active=1 and partymast.sptypecode not in (select option_selected from reservation_parameters where param_id IN('458','500')) order by partycode", True)




                'TitleLoad(Session("OthPListFilter"))
                Session.Add("strsortExpression", "othplisth.oplistcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                charcters(TxtPLCD)


                'advance search option has handled in search button itself report not requires here 
                rbtnadsearch.Visible = False
                btnPrint.Visible = False



                FillGridWithOrderByValues()

                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


                ddlgpcd.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlgpnm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlSellcd.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlexccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlexcname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'Else
                '    If ddlgpcd.Value <> "[Select]" Then
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcd, "airportbordercode", "airportbordername", "select airportbordercode,airportbordername from airportbordersmaster where active=1 and airportbordercode='" & ddlgpcd.Items(ddlgpcd.SelectedIndex).Text & "' order by airportbordercode", True, ddlgpcd.Value)
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpname, "airportbordername", "airportbordercode", "select airportbordercode,airportbordername from airportbordersmaster where active=1 and airportbordername='" & ddlgpname.Items(ddlgpname.SelectedIndex).Text & "' order by airportbordername", True, ddlgpname.Value)
                '    Else
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcd, "airportbordercode", "airportbordername", "select airportbordercode,airportbordername from airportbordersmaster where active=1 order by airportbordercode", True, ddlgpcd.Value)
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpname, "airportbordername", "airportbordercode", "select airportbordercode,airportbordername from airportbordersmaster where active=1 and airportbordername='" & ddlgpname.Items(ddlgpname.SelectedIndex).Text & "' order by airportbordercode", True, ddlgpname.Value)

                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcd, "airportbordercode", "airportbordername", "select airportbordercode , airportbordername from airportbordersmaster where active=1 order by airportbordercode", True, ddlgpcd.Value)
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpname, "airportbordername", "airportbordercode", "select airportbordercode , airportbordername from airportbordersmaster where active=1 order by airportbordername", True, ddlgpname.Value)

                '    End If

                '    If ddlSellcd.Value <> "[Select]" Then
                '        strSqlQry = "select trfsellcode,trfsellname from trfsellmast where active=1 order by trfsellname"
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellcd, "trfsellcode", "trfsellname", strSqlQry, True, ddlSellcd.Value)

                '        strSqlQry = "select trfsellcode,trfsellname from trfsellmast where active=1 " & _
                '        "and trfsellcode='" & ddlSellcd.Items(ddlSellcd.SelectedIndex).Text & "'  order by trfsellname"
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "trfsellname", "trfsellcode", strSqlQry, True, ddlSellingName.Value)
                '    Else
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellcd, "trfsellcode", "trfsellname", "select trfsellcode,trfsellname as trfcodename from trfsellmast where active=1 order by trfcodename", True, ddlSellcd.Value)
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingName, "trfsellname", "trfsellcode", "select trfsellcode,trfsellname as trfcodename from trfsellmast where active=1 order by trfsellname", True, ddlSellingName.Value)
                '    End If
                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OthPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        'If ddlgrpcode.Value <> "[Select]" Then
        '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgrpcode, "othtypcode", "othtypname", "select othtypcode , othtypname  from othtypmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1001)", True, ddlgrpcode.Value)
        '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgrpnm, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlgrpcode.Items(ddlgrpcode.SelectedIndex).Text & "' order by partyname", True, ddlgrpnm.Value)
        'Else
        '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgrpcode, "othtypcode", "othtypname", "select othtypcode , othtypname  from othtypmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1001) order by othtypcode", True, ddlgrpcode.Value)
        '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgrpnm, "othtypname", "othtypcode", "select othtypcode , othtypname  from othtypmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1001) order by othtypname", True, ddlgrpnm.Value)

        'End If

        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OthPriceListWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region

    Private Sub TitleLoad(ByVal prm_strQueryString As String)
        Dim strOption As String = ""
        Dim strqry As String = ""
        Dim strSpType As String = ""

        If prm_strQueryString <> "OTH" Then
            strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", prm_strQueryString)
            ''GrpCode n name column not need to be visible for all types other than 'OTH'
            'gv_SearchResult.Columns(3).Visible = False
            'gv_SearchResult.Columns(4).Visible = False
            strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='" + prm_strQueryString + "') order by othgrpcode"
        Else
            strOption = "OTH"
            strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id in (1001,1002,1003,1021,1022,1023,1024,1025,1027,1028)) order by othgrpcode"
        End If
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcd, "othgrpcode", "othgrpname", strqry, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpnm, "othgrpname", "othgrpcode", strqry, True)

        Select Case strOption
            Case "CAR RENTAL"
                Page.Title += "Transfer Price List"
                lblheading.Text = "Transfer Price List"
                ddlgpcd.SelectedIndex = 0
                ddlSellcd.SelectedIndex = 0
                strSpType = "1031,1039"
        End Select

        'for populating sptype according to resrvtn params for suppliers
        If prm_strQueryString <> "OTH" Then

        Else
            Dim sptypeQry As String = ""
            sptypeQry = " select sptypecode,sptypename from sptypemast where active=1 and sptypecode not in (Select Option_Selected From Reservation_ParaMeters " & _
                "   Where Param_Id in (564,1031,1032,1033,1034,1035,1036,1037,1041,1028)) order by sptypecode "

        End If
    End Sub

#Region "Public Function checkIsPrivilege() As Boolean"
    Public Function checkIsPrivilege() As Boolean
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='8' and appid='1' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Statusapprove", "Yes")
            Else
                Session.Add("Statusapprove", "No")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try

        Return True

    End Function
#End Region


#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If TxtPLCD.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(eplistcode) LIKE '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(eplistcode) LIKE '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "%'"
            End If
        End If

       

        If ddlSellcd.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " partycode = '" & Trim(CType(ddlSellcd.Items(ddlSellcd.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND partycode = '" & Trim(CType(ddlSellcd.Items(ddlSellcd.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlgpcd.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " gpcode = '" & Trim(CType(ddlgpcd.Items(ddlgpcd.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND gpcode = '" & Trim(CType(ddlgpcd.Items(ddlgpcd.SelectedIndex).Text, String)) & "'"
            End If
        End If



        If ddlapprove.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " approved ='Yes' "
            Else
                strWhereCond = strWhereCond & " and approved ='Yes'"
            End If
        ElseIf ddlapprove.SelectedIndex = 2 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "approved = 'No'"
            Else
                strWhereCond = strWhereCond & " and approved = 'No'"
            End If

        End If

        If txtfromDate.Text <> "" And txtToDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = "( (convert(date,frmdate,103) between convert(date, '" & Format(CType(txtfromDate.Text, Date), "dd/MM/yyyy") & "',103) " _
                & "  and  convert(date, '" & Format(CType(txtToDate.Text, Date), "dd/MM/yyyy") & "',103) ) " _
                & "  or (convert(date,todate,103)  between convert(date, '" & Format(CType(txtfromDate.Text, Date), "dd/MM/yyyy") & "',103) " _
                & "  and  convert(date, '" & Format(CType(txtToDate.Text, Date), "dd/MM/yyyy") & "',103) )" _
                & " or (convert(date,frmdate,103) <= convert(date, '" & Format(CType(txtfromDate.Text, Date), "dd/MM/yyyy") & "',103) " _
                & "  and convert(date,todate,103) >= convert(date, '" & Format(CType(txtToDate.Text, Date), "dd/MM/yyyy") & "',103)) )"

            Else
                strWhereCond = strWhereCond & " and ( (convert(date,frmdate,103) between convert(date, '" & Format(CType(txtfromDate.Text, Date), "dd/MM/yyyy") & "',103) " _
                & "  and  convert(date, '" & Format(CType(txtToDate.Text, Date), "dd/MM/yyyy") & "',103) ) " _
                & "  or (convert(date,todate,103)  between convert(date, '" & Format(CType(txtfromDate.Text, Date), "dd/MM/yyyy") & "',103) " _
                & "  and  convert(date, '" & Format(CType(txtToDate.Text, Date), "dd/MM/yyyy") & "',103) )" _
                & " or (convert(date,frmdate,103) <= convert(date, '" & Format(CType(txtfromDate.Text, Date), "dd/MM/yyyy") & "',103) " _
                & "  and convert(date,todate,103) >= convert(date, '" & Format(CType(txtToDate.Text, Date), "dd/MM/yyyy") & "',103)) )"

            End If
        End If



        BuildCondition = strWhereCond
    End Function

#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select * from Excursion_plist_cost"


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition()
                'strSqlQry = strSqlQry & " group by  tplistcode, airportbordername, tsellcode , trfsellname,adduser," & _
                '" moddate,  moduser,adduser,shifting,frmdate,todate,adddate,adduser,moddate,moduser"
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder

            Else
                'strSqlQry = strSqlQry & " group by airportbordername, tsellcode , trfsellname,adduser,moddate,moduser,adduser"
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
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
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("ClearPrice") = "No"
        Dim strpop As String = ""
        'strpop = "window.open('OthPriceCostList1.aspx?State=New','OthPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('OthPriceCostList1.aspx?State=New','OthPriceList1');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("oplist_costh.ocplistcode")
        FillGridWithOrderByValues()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblocplistcode")

            Dim approve As Label
            approve = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapprove")

            If e.CommandName = "Editrow" Then
                
                'If Session("Statusapprove") = "No" Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Edit the Approved Booking' );", True)
                '    Return
                'End If

                Dim strpop As String = ""
                'strpop = "window.open('OthPriceCostList1.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OthPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthPriceCostList1.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OthPriceList1');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesCostPriceList2.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('OthPriceCostList2.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','TrfPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthPriceCostList2.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','TrfPriceList2');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                ''Session.Add("State", "Delete")
                ''Session.Add("RefCode", CType(lblId.Text.Trim, String))
                ''Response.Redirect("OtherServicesCostPriceList2.aspx", False)

               

                Dim strpop As String = ""
                'strpop = "window.open('OthPriceCostList2.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','TrfPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthPriceCostList2.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','TrfPriceList2');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesCostPriceList1.aspx", False)

                Dim strpop As String = ""
                ' strpop = "window.open('OthPriceCostList1.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','OthPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthPriceCostList1.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','OthPriceList1');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceCostList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtPLCD.Text = ""


        ddlgpcd.Value = "[Select]"
        ddlgpnm.Value = "[Select]"
        ddlgpcd.Value = "[Select]"
        ddlgpnm.Value = "[Select]"
        ddlSellcd.Value = "[Select]"
        ddlSellingName.Value = "[Select]"


        TxtPLCD.Text = ""

        txtfromDate.Text = ""
        txtToDate.Text = ""

        ' FillGrid("oplist_costh.ocplistcode")
        'TitleLoad(Session("OthPListFilter")) 'to load all default values in controls accoding to session value
        ddlOrderBy.SelectedIndex = 0
        FillGridWithOrderByValues()
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        'FillGrid(e.SortExpression, direction)
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region


#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = "select * from Excursion_plist_cost "


           

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition()
                       strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()

                Else
                     strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If



                       con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "oplisth")

                objUtils.ExportToExcel(DS, Response)
                'con.Close()

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            clsDBConnect.dbAdapterClose(DA)                       'Close adapter
            clsDBConnect.dbConnectionClose(con)
        End Try
    End Sub

#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

  
    End Sub
#End Region


    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        PnlOtherServiceCost.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        PnlOtherServiceCost.Visible = True
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        'FillGrid("ocplistcode")
        FillGridWithOrderByValues()
    End Sub

    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("eplistcode", "DESC")
            Case 1
                FillGrid("eplistcode", "ASC")
            Case 2
                FillGrid("gpname", "DESC")
            Case 3
                FillGrid("gpname", "ASC")
            Case 4
                FillGrid("partyname", "DESC")
            Case 5
                FillGrid("partyname", "ASC")
        End Select
    End Sub

    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "eplistcode DESC"
            Case 1
                ExportWithOrderByValues = "eplistcode ASC"
            Case 2
                ExportWithOrderByValues = "gpname DESC"
            Case 3
                ExportWithOrderByValues = "gpname ASC"
            Case 4
                ExportWithOrderByValues = "partyname DESC"
            Case 5
                ExportWithOrderByValues = "partyname ASC"
        End Select

    End Function

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthPricelistSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub

    Protected Sub gv_SearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_SearchResult.SelectedIndexChanged

    End Sub

    Protected Sub btnClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

    End Sub
End Class
