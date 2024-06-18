
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class DependentOffersSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strSqlQry1 As String
    Dim strSqlQry2 As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlTrans As SqlTransaction

    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection

    Dim objEmail As New clsEmail  ''' Added shahul

#End Region


#Region "Enum GridCol"
    Enum GridCol
        PromotionId = 0
        partyname = 1
        Promotionname = 2
        applicableto = 3
        MinFromDate = 4
        MinTodate = 5
        ApprovedStatus = 6
        status = 7
        Edit = 10
        View = 11
        Approve = 12
        Copy = 13
        DateCreated = 14
        UserCreated = 15
        DateModified = 16
        UserModified = 17
        approveddate = 18
        approveduser = 19

    End Enum
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '  btnPrint.Visible = False
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' btnPrint.Visible = False
        If Page.IsPostBack = False Then
            Try

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim frmdate As String = ""
                Dim todate As String = ""
                ' btnPrint.Visible = False

                'Me.whotelatbcontrol.appval = CType(Request.QueryString("appid"), String)
                'Me.whotelatbcontrol.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("HotelDetails.aspx", String), CType(Request.QueryString("appid"), Integer))
                ViewState("appid") = CType(Request.QueryString("appid"), String)

                ViewState("partycode") = CType(Request.QueryString("partycode"), String)
                '  If Session("partycode") Is Nothing Then
                Session("partycode") = CType(Request.QueryString("partycode"), String)
                'End If
                hdnpartycode.Value = CType(Request.QueryString("partycode"), String)

                '    Me.whotelatbcontrol.partyval = CType(Request.QueryString("partycode"), String)
                If AppId Is Nothing = False Then

                    ViewState("appid") = AppId.Value
                End If
                strappid = 1
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                txtconnection.Value = Session("dbconnectionName")

                chkshowall.Style.Add("display", "none")

                SetFocus(TxtContractId)

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                               CType(strappname, String), "PriceListModule\DependentOffersSearch.aspx?appid=1", btnAddNew, btnExportToExcel, btnPrint, gv_SearchResult, GridCol.Edit, ApproveColumnNo:=GridCol.Approve)
                End If



                checkIsPrivilege()


                Session.Add("strsortExpression", "promotionid")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '  charcters(TxtPLCD)
                ddlOrder.SelectedIndex = 2



                FillGrid("promotionid", "DESC")
                '' Create a Dynamic datatable ---- Start
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                '--------end

                ' btnchecking.Attributes.Add("OnClick", "ShowProgess()")
                'ChkW1.Visible = False
                'ChkWeek2.Visible = False
                '  txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            '  Me.whotelatbcontrol.partyval = hdnpartycode.Value

            If Session("partycode") Is Nothing Then
                ViewState("partycode") = CType(Request.QueryString("partycode"), String)
                Session("partycode") = CType(Request.QueryString("partycode"), String)
                hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
            Else
                ViewState("partycode") = Session("partycode")
                ' Session("partycode") = Session("Contractparty")
                hdnpartycode.Value = Session("partycode")
            End If

            '        Me.whotelatbcontrol.partyval = hdnpartycode.Value


        End If

        btnchecking.Attributes.Add("OnClick", "ShowProgess()")


        Dim typ As Type
        typ = GetType(DropDownList)




        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlSPTypeCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSPTypeNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")




            ddlSupplierAgent.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSuppierAgentNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")



            ddlCurrencyCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCurrencyNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlSubSeas.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSubSeasNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        End If

        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "DependentOfferMainWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If


    End Sub

#End Region

#Region "Public Function checkIsPrivilege() As Boolean"
    Public Function checkIsPrivilege() As Boolean
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
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

    End Function
#End Region


#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If TxtContractId.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(promotionid) = '" & Trim(TxtContractId.Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(promotionid) = '" & Trim(TxtContractId.Text.Trim.ToUpper) & "'"
            End If
        End If

        If txtSupName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(promotionname) = '" & Trim(txtSupName.Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(promotionname) = '" & Trim(txtSupName.Text.Trim.ToUpper) & "'"
            End If
        End If


        If txtApproved.Text.Trim <> "" Then
            Dim strStatus As String = ""
            If txtApproved.Text.Trim = "Approved" Then
                strStatus = "Yes"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                End If
            ElseIf txtApproved.Text.Trim = "Pending" Then
                strStatus = "No"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                Else
                    strWhereCond = strWhereCond & " AND upper(approved) = '" & Trim(strStatus.Trim.ToUpper) & "'"
                End If
            Else

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

    Private Sub FillnewGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim myds1 As New DataSet
        Dim mydt1 As New DataTable
        grddependent.Visible = True
        'lblMsg.Visible = False

        If grddependent.PageIndex < 0 Then
            grddependent.PageIndex = 0
        End If
        strSqlQry = ""
        strSqlQry1 = ""
        Try


            '   strSqlQry = " select '' promotionid,'' partycode ,''partyname,'' supagentcode,'' supagentname,'' status,'' promotionname,'' bookingcode, '' fromdate,'' todate,'' Applicableto,'' adddate,'' adduser, '' moddate,'' moduser,'' approveddate,'' approvedby "

            'If chkshowall.Checked = True Then
            '    strSqlQry = "select distinct o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o where a.promotionid=o.contpromid "
            'Else



            'strSqlQry = "select distinct o.contpromid  DependentId,a.partycode,a.partyname,a.promotionname,a.activestate,a.status,a.addate ,a.adduser,a.approveddate,a.approvedby   from view_Offer_search(nolock) a, offers_recalculate o where a.promotionid=o.contpromid and isnull(a.activestate,'With Drawn')='Active' union all  " _
            '      & "  select distinct o.contpromid  DependentId,a.partycode,a.partyname,a.applicableto promotionname,a.activestate,a.status,a.adddate addate ,a.adduser,a.approveddate,a.approvedby    from view_contracts_search(nolock) a, offers_recalculate o   where a.contractid=o.contpromid "

            strSqlQry = "select distinct o.contpromid  DependentId,a.partycode,a.partyname,a.promotionname,a.activestate,a.status,a.addate ,a.adduser,a.approveddate,a.approvedby   from view_Offer_search(nolock) a, offers_recalculate o where a.promotionid=o.contpromid  union all  " _
                  & "  select distinct o.contpromid  DependentId,a.partycode,a.partyname,a.applicableto promotionname,a.activestate,a.status,a.adddate addate ,a.adduser,a.approveddate,a.approvedby    from view_contracts_search(nolock) a, offers_recalculate o   where a.contractid=o.contpromid "

            'and isnull(a.activestate,'With Drawn')='Active' is added - changed by mohamed on 02/04/2018
            '' End If




            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & BuildCondition()

            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            Dim pliststr As String = ""



            If pliststr <> "" Then
                strSqlQry = strSqlQry & " WHERE a.contpromid='" & pliststr & "'"

                ''strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            Else

                If Trim(BuildCondition) <> "" Then


                    strSqlQry = strSqlQry & "  " & BuildCondition()

                    '  strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                    'Else

                    '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If

            End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            grddependent.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                grddependent.DataBind()
            Else
                grddependent.PageIndex = 0
                grddependent.DataBind()

            End If

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim myds1 As New DataSet
        Dim mydt1 As New DataTable
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        strSqlQry1 = ""
        Try


            '   strSqlQry = " select '' promotionid,'' partycode ,''partyname,'' supagentcode,'' supagentname,'' status,'' promotionname,'' bookingcode, '' fromdate,'' todate,'' Applicableto,'' adddate,'' adduser, '' moddate,'' moduser,'' approveddate,'' approvedby "

            If chkshowall.Checked = True Then
                'changed by mohamed on 02/04/2018
                'strSqlQry = "select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o where a.promotionid=o.promotionid  and a.promotionid <> o.contpromid "
                strSqlQry = "select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o, view_contracts_search moff "
                strSqlQry += " where a.promotionid=o.promotionid  and a.promotionid <> o.contpromid and moff.contractid=o.contpromid "
                strSqlQry += "   and isnull(a.activestate,'With Drawn')='Active' " 'and isnull(moff.activestate,'With Drawn')='Active'
                strSqlQry += "union select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o, view_Offer_search moff "
                strSqlQry += " where a.promotionid=o.promotionid  and a.promotionid <> o.contpromid and moff.promotionid=o.contpromid "
                strSqlQry += "  and isnull(a.activestate,'With Drawn')='Active' "  'and isnull(moff.activestate,'With Drawn')='Active' 
            Else
                'changed by mohamed on 02/04/2018
                'strSqlQry = "select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o where a.promotionid=o.promotionid and a.promotionid <> o.contpromid"
                strSqlQry = "select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o, view_contracts_search moff "
                strSqlQry += " where a.promotionid=o.promotionid  and a.promotionid <> o.contpromid and moff.contractid=o.contpromid "
                strSqlQry += "  and isnull(a.activestate,'With Drawn')='Active' "    ' and isnull(moff.activestate,'With Drawn')='Active' commented
                strSqlQry += "union select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o, view_Offer_search moff "
                strSqlQry += " where a.promotionid=o.promotionid  and a.promotionid <> o.contpromid and moff.promotionid=o.contpromid "
                strSqlQry += "   and isnull(a.activestate,'With Drawn')='Active' "   ' and isnull(moff.activestate,'With Drawn')='Active'
            End If




            Dim pliststr As String = ""



            If pliststr <> "" Then
                strSqlQry = strSqlQry & " WHERE a.promotionid='" & pliststr & "'"

                ''strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            Else

                If Trim(BuildCondition) <> "" Then


                    strSqlQry = strSqlQry & "  " & BuildCondition()

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                Else

                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If

            End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
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

            FillnewGrid("Promotionid", "DESC")

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub


#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click


        '  Session.Add("State", "New")
        'Session.Add("OfferState", "New")
        'Session("OfferRefCode") = Nothing
        'Session("Promotionid") = Nothing
        'Session("State") = Nothing
        'Session("Calledfrom") = "Offers"

        'Dim strpop As String = ""

        'strpop = "window.open('OfferMain.aspx?Calledfrom=Offers&appid=" + CType(ViewState("appid"), String) + "&partycode=" + hdnpartycode.Value + "&State=New','Offers');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

#End Region


#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("cplisthnew.plistcode", "DESC")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("Promotionid", "DESC")

            Case 1
                FillGrid("Promotionid", "ASC")

            Case 2
                FillGrid("promotionname", "ASC")
                ' FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '   FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")


        End Select
    End Sub

#End Region

    Private Sub GetMealPlanString(ByVal approve As Integer, ByVal plistcode As String)
        Try
            Dim mStrqry As String = ""
            If approve = 1 Then
                mStrqry = "SELECT mealcode FROM cplist_mealplan Where plistcode='" & plistcode & "'"
            Else
                mStrqry = "SELECT mealcode FROM edit_cplist_mealplan Where plistcode='" & plistcode & "'"
            End If

            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), mStrqry)
            Dim mealstr As String = ""
            If ds.Tables.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    If mealstr.Length = 0 Then
                        mealstr = row.Item("mealcode")
                    Else
                        mealstr += "," + row.Item("mealcode")
                    End If
                Next
            End If
            Session("MealPlans") = Nothing
            Session("MealPlans") = mealstr
        Catch ex As Exception

        End Try
    End Sub
    Private Function Validatepage(ByVal promotionid As String) As Boolean
        Dim chkcontract As String = ""

        chkcontract = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from edit_Offers_header(nolock) where promotionid='" & promotionid & "'")

        If chkcontract = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Offer  Already Approved Please check.');", True)
            Validatepage = False
            Exit Function

        End If





        Validatepage = True
    End Function
    Sub saverates(ByVal promotionid As String, ByVal partycode As String)

        Try
            Dim dt As New DataTable
            Dim strSqlQry As String = ""
            Dim sqlTrans As SqlTransaction


            strSqlQry = " execute sp_finalcalculated_rates_offers '" & CType(promotionid, String) & "'"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(dt)
            SqlConn.Close()

            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Offer !.');", True)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction


            mySqlCmd = New SqlCommand("sp_approve_promotion", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(promotionid, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(partycode, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
            mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
            mySqlCmd.ExecuteNonQuery()



            'mySqlCmd = New SqlCommand("insert into offers_recalculate_approved select contpromid,promotionid,adddate,adduser,getdate(), from  offers_recalculate where contpromid='" & promotionid & "'", mySqlConn, sqlTrans)
            'mySqlCmd.CommandType = CommandType.Text
            'mySqlCmd.ExecuteNonQuery()


            mySqlCmd = New SqlCommand("sp_save_recalculateoffers_approved", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = promotionid
            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.ExecuteNonQuery()
            mySqlCmd.Dispose() '


            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)




        Catch ex As Exception
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub
#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            Dim lblagent As Label
            Dim partycode As Label
            Dim supagentcode As String
            Dim supagentname As Label
            Dim lblCountryGroup As Label

            Dim approve As Label

            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            partycode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblparty")
            'lblCountryGroup = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblmarket")
            lblagent = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblagent")

            Dim status As Integer = 0
            '   If approve.Text = "Approved" Then status = 1
            If e.CommandName = "Page" Then Exit Sub
            Dim ds As New DataSet




            If e.CommandName = "Approve" Then
                'Dim scriptKey As String = "UniqueKeyForThisScript"
                'Dim javaScript As String = "<script type='text/javascript'>ShowProgess();</script>"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), scriptKey, javaScript, True)

                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "ShowProgess()", True)
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowProgess();", True)

                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")

                hdnpromoid.Value = lblId.Text
                If Validatepage(lblId.Text) = False Then
                    Exit Sub
                End If

                btnchecking_Click(sender, e)

                ' btnchecking.Attributes.Add("onClick", "ShowProgess();")


                'ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_offers  " & "'" & lblId.Text & "'")

                'If ds.Tables(0).Rows.Count > 0 Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Promotion Validation Errors Showing Please go to Validate For Approval see the Error list' );", True)
                '    Exit Sub
                '    'Else
                '    '    btnchecking.Attributes.Add("onClick", "ShowProgess()")
                '    'saverates(lblId.Text, partycode.Text)
                '    'btnSearch_Click(sender, e)
                'End If
                '
                'Session.Add("OfferState", "Edit")
                'Session.Add("OfferRefCode", CType(lblId.Text.Trim, String))
                'Session("Offerparty") = partycode.Text

                'Dim strpop As String = ""
                'strpop = "window.open('OfferMain.aspx?Calledfrom=Offers&appid=" + CType(Request.QueryString("appid"), String) + "&promotionid=" & lblId.Text & "&partycode=" + partycode.Text + "&State=Edit','OfferMain');"



                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
                Session.Add("OfferState", "View")
                Session.Add("OfferRefCode", CType(lblId.Text.Trim, String))
                Session("Offerparty") = partycode.Text
                Dim strpop As String = ""

                strpop = "window.open('OfferMain.aspx?Calledfrom=Offers&appid=" + CType(Request.QueryString("appid"), String) + "&promotionid=" & lblId.Text & "&partycode=" + partycode.Text + "&State=View','OfferMain');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")


                Dim approvestr As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT 't' FROM offers_header(nolock) Where  promotionid='" & lblId.Text & "'")
                If approvestr <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Promotion already approved so cannot be delete Please proceed to Withdraw' );", True)
                    Exit Sub
                End If


                Session.Add("OfferState", "Delete")
                Session.Add("OfferRefCode", CType(lblId.Text.Trim, String))
                Session("Offerparty") = partycode.Text
                Dim strpop As String = ""

                strpop = "window.open('OfferMain.aspx?Calledfrom=Offers&appid=" + CType(Request.QueryString("appid"), String) + "&promotionid=" & lblId.Text & "&partycode=" + partycode.Text + "&State=Delete','OfferMain');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Copy" Then
                lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
                Session.Add("OfferState", "Copy")
                Session.Add("OfferRefCode", CType(lblId.Text.Trim, String))
                Session("Offerparty") = partycode.Text
                Dim strpop As String = ""
                'strpop = "window.open('ContractMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "','ContractMain');"
                strpop = "window.open('OfferMain.aspx?Calledfrom=Offers&appid=" + CType(Request.QueryString("appid"), String) + "&promotionid=" & lblId.Text & "&partycode=" + partycode.Text + "&State=Copy','OfferMain');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtContractId.Text = ""
        'Txtprmname.Text = ""
        'Txtpromotionid.Text = ""
        txtSupName.Text = ""


        ddlSPTypeCD.Value = "[Select]"
        ddlSPTypeNM.Value = "[Select]"



        ddlSupplierAgent.Value = "[Select]"
        ddlSuppierAgentNM.Value = "[Select]"



        ddlCurrencyCD.Value = "[Select]"
        ddlCurrencyNM.Value = "[Select]"


        ddlSubSeas.Value = "[Select]"
        ddlSubSeasNM.Value = "[Select]"



        ViewState("MyAutoNo") = 1 'clear plist links 
        ddlOrderBy.SelectedIndex = 0

        FillGrid("promotionid", "DESC")
    End Sub
#End Region
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()


    End Sub
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function
    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
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
    Protected Sub DependApprove_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim approve As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)

            Dim lblpartycode As Label = CType(row.FindControl("lblparty"), Label)
            Dim lblDependentId As Label = CType(row.FindControl("lblDependentId"), Label)

            Dim lblstatus As Label = CType(row.FindControl("lblstatus"), Label)

            Dim strtemp As String = ""
            hdnpromoid.Value = lblDependentId.Text

            Dim strFromEmailID As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select fromemailid from email_text where emailtextfor=1")
            Dim strSubject1 As String = ""
            Dim toemail As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select option_selected from reservation_parameters where param_id=2011")


            Dim dt As New DataTable
            Dim strSqlQry As String = ""
            Dim applyflag As Boolean = False

            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_offers_header(nolock)  where promotionid='" & lblDependentId.Text & "'") <> "" Then


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand("sp_approve_promotion", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(lblpartycode.Text, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
                mySqlCmd.ExecuteNonQuery()



                mySqlCmd = New SqlCommand("sp_save_recalculateoffers_approved_depend", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)



                ''' Added shahul 15/05/18
                'Try
                '    strSubject1 = " Approval Error for this Promotion - " + lblDependentId.Text

                '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                '    mySqlCmd = New SqlCommand("sp_approve_markuprates_offers", SqlConn)
                '    mySqlCmd.CommandType = CommandType.StoredProcedure
                '    mySqlCmd.CommandTimeout = 0
                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                '    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)

                '    mySqlCmd.ExecuteNonQuery()
                '    SqlConn.Close()

                '    ModalPopupDays.Hide()

                'Catch ex As Exception
                '    ModalPopupDays.Hide()

                '    Dim strEmailText1 As String = ex.Message.ToString
                '    If objEmail.SendEmailCCust(strFromEmailID, toemail, toemail, strSubject1, strEmailText1, Server.MapPath("~/images/logo.png")) Then
                '    Else
                '        objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), "Sending Approval Fail Email " + strSubject1, Session("GlobalUserName"))
                '    End If
                'End Try

            Else

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand("sp_approve_contract", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(lblpartycode.Text, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
                mySqlCmd.ExecuteNonQuery()



                mySqlCmd = New SqlCommand("sp_save_recalculateoffers_approved_depend", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)


                ''' Added shahul 15/05/18
                'Try
                '    strSubject1 = " Approval Error for this Contract - " + lblDependentId.Text

                '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                '    mySqlCmd = New SqlCommand("sp_approve_markuprates_contracts", SqlConn)
                '    mySqlCmd.CommandType = CommandType.StoredProcedure
                '    mySqlCmd.CommandTimeout = 0
                '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                '    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)

                '    mySqlCmd.ExecuteNonQuery()
                '    SqlConn.Close()

                '    ModalPopupDays.Hide()

                'Catch ex As Exception

                '    ModalPopupDays.Hide()

                '    Dim strEmailText1 As String = ex.Message.ToString
                '    If objEmail.SendEmailCCust(strFromEmailID, toemail, toemail, strSubject1, strEmailText1, Server.MapPath("~/images/logo.png")) Then

                '        ''' Delete  from main table

                '        strSqlQry = " execute sp_delete_markuprates_contracts '" & CType(lblDependentId.Text, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

                '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                '        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                '        myDataAdapter.SelectCommand.CommandTimeout = 0
                '        myDataAdapter.Fill(dt)
                '        mySqlConn.Close()

                '    Else
                '        objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), "Sending Approval Fail Email " + strSubject1, Session("GlobalUserName"))
                '    End If
                'End Try


            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Dependent ID Approved Successfully ');", True)
            btnSearch_Click(sender, e)
            ModalPopupDays.Hide()

        Catch ex As Exception
            ModalPopupDays.Hide()
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub DepRate_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim approve As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)

            Dim lblpartycode As Label = CType(row.FindControl("lblparty"), Label)
            Dim lblDependentId As Label = CType(row.FindControl("lblDependentId"), Label)

            Dim lblstatus As Label = CType(row.FindControl("lblstatus"), Label)
            Dim lnkdepapprove As LinkButton = row.FindControl("lnkdepapprove")
            Dim DependApprove As LinkButton = row.FindControl("DependApprove")

            

            Dim strtemp As String = ""
            hdnpromoid.Value = lblDependentId.Text

            Dim strFromEmailID As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select fromemailid from email_text where emailtextfor=1")
            Dim strSubject1 As String = ""
            Dim toemail As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select option_selected from reservation_parameters where param_id=2011")


            Dim dt As New DataTable
            Dim strSqlQry As String = ""
            Dim applyflag As Boolean = False

            'Dim appapproved As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from offers_recalculate(nolock) where promotionid<>'" & lblDependentId.Text & "' and  contpromid='" & lblDependentId.Text & "'")
            Dim strMsg As String = "Some Offers  Approval Pending in the First Grid  for the Selected Dependent Please Approve Offers in first Grid " + "\n"

            Dim appapproved As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(stuff((select  ',' +convert(varchar(100),u.promotionid)    from offers_recalculate u(nolock), view_offer_search v  where  u.promotionid<>'" & lblDependentId.Text & "' and  u.contpromid='" & lblDependentId.Text & "' and u.promotionid =v.promotionid   and isnull(v.activestate,'')='Active' order by u.contpromid for xml path('')),1,1,''),'')")

            If appapproved <> "" Then
                strMsg = strMsg + "  " + appapproved + "\n"
                ModalPopupDays.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)
                '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Some Offers  Approval Pending in the First Grid  for the Selected Dependent Please Approve Offers in first Grid' );", True)
                Exit Sub
            End If

            Dim ds As New DataSet

            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from offers_recalculate(nolock) where contpromid='" & lblDependentId.Text & "'") <> "" Then

                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_offers_header(nolock)  where promotionid='" & lblDependentId.Text & "'") <> "" Then

                    ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_offers  " & "'" & lblDependentId.Text & "'")


                    If ds.Tables(0).Rows.Count > 0 Then


                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Promotion Validation Errors Showing Please go to Validate For Approval Correct the Error list' );", True)
                        Exit Sub

                    End If

                    ' Rosalin 2019-11-02
                    'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    'mySqlCmd = New SqlCommand("sp_finalcalculated_rates_offers", SqlConn)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.CommandTimeout = 0
                    'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                    'mySqlCmd.ExecuteNonQuery()
                    'SqlConn.Close()

                    ModalPopupDays.Hide()


                Else
                    ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_contract  " & "'" & lblDependentId.Text & "'")

                    If ds.Tables(0).Rows.Count > 0 Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contract Validation Errors Showing Please go to Validate For Approval Correct the Error list' );", True)
                        Exit Sub

                    End If


                    ' Rosalin 2019-11-02
                    'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    'mySqlCmd = New SqlCommand("sp_finalcalculated_rates", SqlConn)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.CommandTimeout = 0
                    'mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(lblDependentId.Text, String)
                    'mySqlCmd.ExecuteNonQuery()
                    'SqlConn.Close()

                    ModalPopupDays.Hide()



                End If



            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Dependent ID Rate Calculated Successfully You Can Approve');", True)
            ' btnSearch_Click(sender, e)
            ModalPopupDays.Hide()

            lnkdepapprove.Enabled = False
            DependApprove.Enabled = True

        Catch ex As Exception
            ModalPopupDays.Hide()
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub Approve_click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim approve As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbpromotid As Label = CType(row.FindControl("lblplistcode"), Label)
            Dim lblpartycode As Label = CType(row.FindControl("lblparty"), Label)
            Dim lblDependentId As Label = CType(row.FindControl("lblDependentId"), Label)

            Dim lblstatus As Label = CType(row.FindControl("lblstatus"), Label)

            Dim strSqlQry As String = ""
            Dim dt As New DataTable

            Dim strFromEmailID As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select fromemailid from email_text where emailtextfor=1")
            Dim strSubject1 As String = ""
            Dim toemail As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select option_selected from reservation_parameters where param_id=2011")


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction


            'Added by Rosalin 14th Oct 2019 for Rate Sheet and XML Pushing data into ColumbusRPTS main tables ( priceOffer adults etc...)

            'mySqlCmd = New SqlCommand("ColumbusRpts.dbo.New_approve_promotion", mySqlConn, sqlTrans)
            'mySqlCmd.CommandType = CommandType.StoredProcedure
            'mySqlCmd.CommandTimeout = 0
            'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromoid.Value, String)
            'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(lblpartycode.Text, String)
            'mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
            'mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
            'mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
            'mySqlCmd.ExecuteNonQuery()


            mySqlCmd = New SqlCommand("sp_approve_promotion", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromoid.Value, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(lblpartycode.Text, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
            mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
            mySqlCmd.ExecuteNonQuery()

            mySqlCmd = New SqlCommand("sp_save_recalculateoffers_approved", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = hdnpromoid.Value
            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.ExecuteNonQuery()
            ' mySqlCmd.Dispose() '





            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)


            ' ''' Added shahul 15/05/18
            'Try
            '    strSubject1 = " Approval Error for this Promotion - " + hdnpromoid.Value

            '    'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            '    'mySqlCmd = New SqlCommand("sp_approve_markuprates_offers", SqlConn)
            '    'mySqlCmd.CommandType = CommandType.StoredProcedure
            '    'mySqlCmd.CommandTimeout = 0
            '    'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromoid.Value, String)
            '    'mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
            '    'mySqlCmd.ExecuteNonQuery()
            '    'SqlConn.Close()

            '    'Added by Rosalin 9th Oct 2019 for Rate Sheet and XML Pushing data into ColumbusRPTS main tables ( priceOffer adults etc...)
            '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            '    'Open connection

            '    mySqlCmd = New SqlCommand("ColumbusRpts.dbo.New_PreFinalcalculateMain_offers", mySqlConn)
            '    mySqlCmd.CommandType = CommandType.StoredProcedure
            '    mySqlCmd.CommandTimeout = 0
            '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromoid.Value, String)
            '    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)

            '    mySqlCmd.ExecuteNonQuery()
            '    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            '    clsDBConnect.dbConnectionClose(mySqlConn)


            '    ModalPopupDays.Hide()



            'Catch ex As Exception
            '    ModalPopupDays.Hide()
            '    Dim strEmailText1 As String = ex.Message.ToString
            '    If objEmail.SendEmailCCust(strFromEmailID, toemail, toemail, strSubject1, strEmailText1, Server.MapPath("~/images/logo.png")) Then


            '        ''' Delete  from main table

            '        strSqlQry = " execute ColumbusRpts.dbo.sp_delete_markuprates_offers '" & CType(hdnpromoid.Value, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

            '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            '        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            '        myDataAdapter.SelectCommand.CommandTimeout = 0
            '        myDataAdapter.Fill(dt)
            '        mySqlConn.Close()

            '    Else
            '        objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), "Sending Approval Fail Email " + strSubject1, Session("GlobalUserName"))
            '    End If
            'End Try

            ModalPopupDays.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Offer Approved Successfully');", True)
            btnSearch_Click(sender, e)



        Catch ex As Exception
            ModalPopupDays.Hide()
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub
    Protected Sub Rate_Click(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim approve As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbpromotid As Label = CType(row.FindControl("lblplistcode"), Label)
            Dim lblpartycode As Label = CType(row.FindControl("lblparty"), Label)
            Dim lblDependentId As Label = CType(row.FindControl("lblDependentId"), Label)



            Dim lblstatus As Label = CType(row.FindControl("lblstatus"), Label)

            Dim lnkapprove As LinkButton = row.FindControl("lnkapprove")
            Dim lnkRateApprove As LinkButton = row.FindControl("RateApprove")

            lnkapprove.Enabled = True
            lnkRateApprove.Enabled = False

            Dim strtemp As String = ""
            hdnpromoid.Value = lbpromotid.Text
            Dim strSqlQry As String = ""
            Dim dt As New DataTable

            Dim strFromEmailID As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select fromemailid from email_text where emailtextfor=1")
            Dim strSubject1 As String = ""
            Dim toemail As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select option_selected from reservation_parameters where param_id=2011")

            Dim applyflag As Boolean = False

            If lblstatus.Text = "Yes" Then

                '''' Insert Main tables entry to Edit Table


                strSqlQry = " execute sp_insertrecords_edit_offers '" & CType(hdnpromoid.Value, String) & "'"

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.SelectCommand.CommandTimeout = 0
                myDataAdapter.Fill(dt)
                SqlConn.Close()



            End If


            Dim ds As New DataSet
            ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_offers  " & "'" & hdnpromoid.Value & "'")

           


            If ds.Tables(0).Rows.Count > 0 Then

                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                    If ds.Tables(0).Rows(i)("optionname") <> "Apply Offer To" Then
                        applyflag = True
                        Exit For
                    End If


                Next
                If applyflag = True Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Promotion Validation Errors Showing Please go to Validate For Approval Correct the Error list' );", True)
                    Exit Sub
                End If
            End If

            If lblstatus.Text = "Yes" Then


            End If


            ' Added by Rosalin on 2019-10-14 for XML and rate sheet- moving data to old DB

            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

            'mySqlCmd = New SqlCommand("New_insertrecords_edit_offers", SqlConn)
            'mySqlCmd.CommandType = CommandType.StoredProcedure
            'mySqlCmd.CommandTimeout = 0
            'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromoid.Value, String)

            'mySqlCmd.ExecuteNonQuery()



            'mySqlCmd = New SqlCommand("ColumbusRpts.dbo.sp_finalcalculated_rates_offers", SqlConn)
            'mySqlCmd.CommandType = CommandType.StoredProcedure
            'mySqlCmd.CommandTimeout = 0
            'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromoid.Value, String)

            'mySqlCmd.ExecuteNonQuery()
            'SqlConn.Close()

            ModalPopupDays.Hide()
            Dim strScript As String = "javascript: hidenoerror();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)

            lnkRateApprove.Enabled = True
            lnkapprove.Enabled = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Offer !.');", True)



        Catch ex As Exception
            ModalPopupDays.Hide()
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


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
                lbtext.Text = lbtext.Text.Substring(0, 25)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowCreated

    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then




            Dim lblplistcode As Label = e.Row.FindControl("lblplistcode")
            Dim lblparty As Label = e.Row.FindControl("lblparty")
            Dim lblagent As Label = e.Row.FindControl("lblagent")
            '   Dim lblsubagentname As Label = e.Row.FindControl("lblsubagentname")
            Dim lblpartyname As Label = e.Row.FindControl("lblpartyname")
            Dim lblstatus As Label = e.Row.FindControl("lblstatus")
            '  Dim lblDependentId As Label = e.Row.FindControl("lblDependentId")

            Dim lblactive As Label = e.Row.FindControl("lblactive")

            Dim lnkapprove As LinkButton = e.Row.FindControl("lnkapprove")
            Dim lnkRateApprove As LinkButton = e.Row.FindControl("RateApprove")

            lnkapprove.Enabled = True
            lnkRateApprove.Enabled = False

            'If lblstatus.Text = "Yes" Then
            '    lnkapprove.Visible = False
            'Else
            '    lnkapprove.Visible = True
            'End If

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextSectorGroup As String = ""

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCtry = ""

                        If "PROMOTIONID" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "PROMOTIONNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "HOTEL" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "APPROVEDSTATUS" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSectorGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCtry
                            lsSearchTextSector = lsSearchTextCtry
                            lsSearchTextSectorGroup = lsSearchTextCtry
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblplistcode.Text = Regex.Replace(lblplistcode.Text.Trim, lsSearchTextCtry.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblpartyname.Text = Regex.Replace(lblpartyname.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        'If lsSearchTextSector.Trim <> "" Then
                        '    lblsubagentname.Text = Regex.Replace(lblsubagentname.Text.Trim, lsSearchTextSector.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '                    RegexOptions.IgnoreCase)
                        'End If

                        If lsSearchTextSectorGroup.Trim <> "" Then
                            lblstatus.Text = Regex.Replace(lblstatus.Text.Trim, lsSearchTextSectorGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If

            If lblactive.Text <> "Active" Then
                e.Row.BackColor = Drawing.Color.Lavender
            End If

        End If
    End Sub
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        '  FillGrid(e.SortExpression, direction)
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
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = "select * from view_offer_search "
                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & ""
                Else
                    strSqlQry = strSqlQry & " ORDER BY promotionid"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "promotions")

                objUtils.ExportToExcel(DS, Response)
                con.Close()

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

#End Region

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        '    Try
        '        '  Session.Add("CurrencyCode", txtblocksale_header.blocksaleid.Text.Trim)
        '        '   Session.Add("CurrencyName", txtcityname.Text.Trim)
        '        '   Response.Redirect("rptCurrencies.aspx", False)

        '        Dim strReportTitle As String = ""
        '        Dim strSelectionFormula As String = ""

        '        Session.Add("Pageame", "Block Full Sales")
        '        Session.Add("BackPageName", "BlockFullSalesSearch.aspx")

        '        If txtTranId.Text.Trim <> "" Then
        '            strReportTitle = "Block Sale ID : " & txtTranId.Text.Trim
        '            strSelectionFormula = "{blocksale_header.blocksaleid} LIKE '" & txtTranId.Text.Trim & "*'"
        '        End If


        '        If ddlPartyCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlSupACode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlSupAName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        If ddlMarketCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlMarketName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlSPTypeCode.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '                strSelectionFormula = "{blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If
        '        If ddlSpTypeName.Value.Trim <> "[Select]" Then
        '            If strSelectionFormula <> "" Then
        '                strReportTitle = strReportTitle & " ; Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '                strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '            Else
        '                strReportTitle = "Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '                strSelectionFormula = "{supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '            End If
        '        End If

        '        Session.Add("SelectionFormula", strSelectionFormula)
        '        Session.Add("ReportTitle", strReportTitle)
        '        Response.Redirect("rptReport.aspx", False)
        '    Catch ex As Exception
        '        objUtils.WritErrorLog("BlockFullSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        '    End Try
    End Sub
#End Region



    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.CheckedChanged
        pnlHeader.Visible = False
        SetFocus(rbtnsearch)
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnadsearch.CheckedChanged
        pnlHeader.Visible = True
        SetFocus(rbtnadsearch)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim frmdate As String = ""
        Dim todate As String = ""





        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("promotionid", "DESC")

            Case 1
                FillGrid("promotionid", "ASC")

            Case 2
                FillGrid("partycode", "ASC")

            Case 3
                FillGrid("partyname", "ASC")

            Case 4
                FillGrid("supagentname", "ASC")

        End Select

        ModalPopupDays.Hide()
    End Sub
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PriceListSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("promotionid", "DESC")

            Case 1
                FillGrid("promotionid", "ASC")

            Case 2
                FillGrid("partycode", "ASC")

            Case 3
                FillGrid("partyname", "ASC")

            Case 4
                FillGrid("supagentname", "ASC")

        End Select
        SetFocus(ddlOrderBy)
    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub

    Protected Sub btnexit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click
        'Session("AutoNo")
        Session.Remove("AutoNo")
    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""
        Dim lsProcessPromid As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "PROMOTIONNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("PROMOTIONNAME", lsProcessCity, "PROMOTIONNAME")
                Case "PROMOTIONID"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("PROMOTIONID", lsProcessCountry, "PROMOTIONID")
                Case "DEPENDENTID"
                    lsProcessPromid = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("DEPENDENTID", lsProcessPromid, "DEPENDENTID")
                Case "HOTEL"
                    lsProcessGroup = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("HOTEL", lsProcessGroup, "HOTEL")
                Case "APPROVEDSTATUS"
                    lsProcessSector = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("APPROVEDSTATUS", lsProcessSector, "APPROVEDSTATUS")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
                    'If lsProcessAll.Trim = """" Then
                    '    lsProcessAll = ""
                    'End If
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew()

    End Sub
    Protected Sub RowsPerPageSS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageSS.SelectedIndexChanged
        FillGridNew()
    End Sub
    Public Function getRowpage() As String
        Dim rowpagess As String
        If RowsPerPageSS.SelectedValue = "[Select]" Then
            rowpagess = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagess = RowsPerPageSS.SelectedValue

        End If
        Return rowpagess
    End Function
    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strSectorValue As String = ""
        Dim strSectorGroupValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strPromidValue As String = ""
        Try

            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "PROMOTIONID" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "DEPENDENTID" Then
                        If strPromidValue <> "" Then
                            strPromidValue = strPromidValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strPromidValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "PROMOTIONNAME" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "HOTEL" Then
                        If strSectorValue <> "" Then
                            strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "APPROVEDSTATUS" Then
                        If strSectorGroupValue <> "" Then
                            strSectorGroupValue = strSectorGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                    End If
                Next
            Else
                chkshowall.Style.Add("display", "block")
                chkshowall.Checked = True
            End If
            Dim pagevaluess = getRowpage()
            strBindCondition = BuildConditionNew(strCountryValue, strCityValue, strSectorValue, strSectorGroupValue, strTextValue, strPromidValue)

            Dim myDS As New DataSet
            Dim strValue As String

            gv_SearchResult.Visible = True
            lblMsg.Visible = False

            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"




            'strSqlQry = "select * from view_contracts_search(nolock) where partycode='" & hdnpartycode.Value & "'"
            '  strSqlQry = " select '' promotionid,'' partycode ,''partyname,'' supagentcode,'' supagentname,'' status,'' promotionname,'' bookingcode, '' fromdate,'' todate,'' Applicableto,'' adddate,'' adduser, '' moddate,'' moduser,'' approveddate,'' approvedby "

            ' strSqlQry = "select * from view_Offer_search(nolock) " ' where partycode='" & hdnpartycode.Value & "'"

            'changed by mohamed on 02/04/2018
            'strSqlQry = "select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o where a.promotionid=o.promotionid  and a.promotionid <> o.contpromid "
            strSqlQry = "select o.contpromid  DependentId,a.* from view_Offer_search(nolock) a, offers_recalculate o, view_Offer_search moff "
            strSqlQry += " where a.promotionid=o.promotionid  and a.promotionid <> o.contpromid and moff.promotionid=o.contpromid "
            strSqlQry += " and isnull(moff.activestate,'With Drawn')='Active' and isnull(a.activestate,'With Drawn')='Active' "

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " AND " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            Dim ds1 As New DataSet

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)

            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = pagevaluess
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

            strSqlQry = "select  * from  (select distinct o.contpromid  DependentId,a.partycode,a.partyname,a.applicableto promotionname,a.activestate,a.status , " _
                       & " a.addate, a.adduser, a.approveddate, a.approvedby  from view_Offer_search(nolock) a, offers_recalculate o  where a.promotionid = o.contpromid " _
                       & " and isnull(a.activestate,'With Drawn')='Active' " _
                        & " union all   select distinct o.contpromid  DependentId,a.partycode,a.partyname,a.applicableto promotionname,a.activestate,a.status ," _
                       & " a.adddate addate, a.adduser, a.approveddate, a.approvedby   from view_contracts_search(nolock) a, offers_recalculate o  where a.contractid=o.contpromid )  rs"

            'and isnull(a.activestate,'With Drawn')='Active' is added - changed by mohamed on 02/04/2018

            If strPromidValue <> "" Then
                strPromidValue = " upper(DependentId) IN (" & Trim(strPromidValue.Trim.ToUpper) & ")"
                ' strPromidValue = strPromidValue.Replace("contpromid", "DependentId")
                strSqlQry = strSqlQry & " where  " & strPromidValue
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(ds1)


            grddependent.DataSource = ds1

            If ds1.Tables(0).Rows.Count > 0 Then
                grddependent.PageSize = pagevaluess
                grddependent.DataBind()
            Else
                grddependent.PageIndex = 0
                grddependent.DataBind()

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strSectorValue As String, ByVal strSectorGroupValue As String, ByVal strTextValue As String, ByVal strPromidValue As String) As String
        strWhereCond = ""

        If strSectorValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(partyname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(partyname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            End If
        End If

        If strSectorGroupValue <> "" Then
            If Trim(strWhereCond) = "" Then
                If Replace(strSectorGroupValue, "'", "") = "APPROVED" Then
                    strWhereCond = " status ='Yes'"
                ElseIf Replace(strSectorGroupValue, "'", "") = "UNAPPROVED" Then
                    strWhereCond = " status ='No'"
                Else
                    strWhereCond = " status IN (status)"
                End If
            Else
                If Replace(strSectorGroupValue, "'", "") = "APPROVED" Then
                    strWhereCond = strWhereCond & " AND   status ='Yes'"
                ElseIf Replace(strSectorGroupValue, "'", "") = "UNAPPROVED" Then
                    strWhereCond = strWhereCond & " AND  status ='No'"
                Else
                    strWhereCond = strWhereCond & " AND  status IN (status)"
                End If
                'strWhereCond = strWhereCond & " AND  upper(status) IN ( " & Trim(strSectorGroupValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCountryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(o.promotionid) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(o.promotionid) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If

        If strPromidValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(contpromid) IN (" & Trim(strPromidValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(contpromid) IN (" & Trim(strPromidValue.Trim.ToUpper) & ")"
            End If
        End If



        If strCityValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(promotionname) IN ( " & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(promotionname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            End If
        End If

        If strTextValue <> "" Then

            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then
                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = " (upper(o.promotionid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(contpromid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' OR upper(promotionname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(status) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(o.promotionid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(contpromid) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(promotionname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(status) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If

        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "P" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),convert(datetime,fromdate,103),111)  " _
                              & "  and convert(varchar(10),convert(datetime,todate,103),111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                              & "  between convert(varchar(10),convert(datetime,fromdate,103),111)  and  convert(varchar(10),convert(datetime,todate,103),111))   " _
                              & " or (convert(varchar(10),convert(datetime,fromdate,103),111) >= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                              & "  and convert(varchar(10),convert(datetime,todate,103),111) <= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ))"

                    '  strWhereCond = " (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & " and ((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),convert(datetime,fromdate,103),111)  " _
                       & "  and convert(varchar(10),convert(datetime,todate,103),111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                       & "  between convert(varchar(10),convert(datetime,fromdate,103),111)  and  convert(varchar(10),convert(datetime,todate,103),111))   " _
                       & " or (convert(varchar(10),convert(datetime,fromdate,103),111) >= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                       & "  and convert(varchar(10),convert(datetime,todate,103),111) <= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)))"

                    'strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function



    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        '  Session.Add("State", "New")



        'Dim strpop As String = ""
        'Dim contid As String = Nothing

        'Session("OfferRefCode") = Nothing
        'Session("promotionid") = Nothing
        'Session.Add("OfferState", "New")

        ''strpop = "window.open('ContractMainNew.aspx?appid=" + CType(ViewState("appid"), String) + "&partycode=" + hdnpartycode.Value + "&contractid=CON/000005','Contracts');"
        'strpop = "window.open('OfferMain.aspx?appid=" + CType(ViewState("appid"), String) + "&promotionid=" + contid + "&partycode=" + hdnpartycode.Value + "&State=New','Offers');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub chkshowall_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkshowall.CheckedChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("Promotionid", "DESC")

            Case 1
                FillGrid("Promotionid", "ASC")

            Case 2
                FillGrid("promotionname", "ASC")
                ' FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("partyname", "ASC")
                '   FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("supagentname", "ASC")


        End Select
    End Sub

    Protected Sub btnchecking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchecking.Click

        Dim ds As DataSet
        ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_offers  " & "'" & hdnpromoid.Value & "'")


        If ds.Tables(0).Rows.Count > 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Promotion Validation Errors Showing Please go to Validate For Approval see the Error list' );", True)
            Exit Sub
            'Else
            '    btnchecking.Attributes.Add("onClick", "ShowProgess()")
            'saverates(lblId.Text, partycode.Text)
            'btnSearch_Click(sender, e)
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        '    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "RefreshContent();", True)
    End Sub

    Protected Sub grddependent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grddependent.PageIndexChanging
        grddependent.PageIndex = e.NewPageIndex
        FillnewGrid("promotionid", "DESC")

    End Sub

    Protected Sub grddependent_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grddependent.RowCommand
      
    End Sub

    Protected Sub grddependent_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grddependent.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then


                Dim lnkdepapprove As LinkButton = e.Row.FindControl("lnkdepapprove")
                Dim DependApprove As LinkButton = e.Row.FindControl("DependApprove")

                lnkdepapprove.Enabled = True
                DependApprove.Enabled = False


            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DependentOffersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
      
    End Sub
End Class
