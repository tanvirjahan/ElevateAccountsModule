'------------================--------------=======================------------------================
'   Module Name    :    Excursionpricelistsearch.aspx
'   Developer Name :    Amit Survase
'   Date           :    17 July 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class OtherServicesCostPricelistSearch
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

    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
#End Region


#Region "Enum GridCol"
    Enum GridCol
        PListCodeTCol = 0
        PListCode = 1
        groupname = 2
        currency = 3
        applicableto = 4
        FromDate = 5
        Todate = 6
        status = 7

        Edit = 8
        View = 9
        Delete = 10
        Copy = 11
        DateCreated = 12
        UserCreated = 13
        DateModified = 14
        UserModified = 15
        
    End Enum
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' btnPrint.Visible = False
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

                ViewState("appid") = CType(Request.QueryString("appid"), String)
                ViewState("costtype") = CType(Request.QueryString("costtype"), String)



                txtconnection.Value = Session("dbconnectionName")

                '  objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)

                SetFocus(TxtContractId)

                Session("OthPListFilter") = Request.Params("Type")
                hdDefault_Group.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='" & Request.Params("Type") & "'")

                txtconnection.Value = Session("dbconnectionName")



                If Request.QueryString("appid") Is Nothing = False Then
                    strappid = CType(Request.QueryString("appid"), String)
                Else
                    strappid = Session("newappid")
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    If strappid = 1 Then
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                 CType(strappname, String), "PriceListModule\OtherServicesCostPricelistSearch.aspx", btnAddNew, btnExportToExcel, _
                                                           btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                    Else
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                 CType(strappname, String), "PriceListModule\OtherServicesCostPricelistSearch.aspx?appid=" + strappid + "&Type=" + Session("OthPListFilter"), btnAddNew, btnExportToExcel, _
                                                           btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)

                    End If
                End If


                checkIsPrivilege()

                ViewState("strMACsortExpression") = "h.ocplistcode"

                Session.Add("strMACsortdirection", SortDirection.Ascending)
                '  charcters(TxtPLCD)
                ddlOrder.SelectedIndex = 2


                If hdDefault_Group.Value = "VISA" Then
                    Label1.Text = "Visa Price List Search"
                End If


                FillGrid("h.ocplistcode", "DESC")
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

                'ChkW1.Visible = False
                'ChkWeek2.Visible = False
                '  txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherservicescostPriceListsearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Dim typ As Type
        typ = GetType(DropDownList)


        btnPrint.Visible = False



        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AirportCostPriceListWindowPostBack") Then
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
            objUtils.WritErrorLog("OtherservicescostPriceListsearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                strWhereCond = " upper(oplistcode) = '" & Trim(TxtContractId.Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(oplistcode) = '" & Trim(TxtContractId.Text.Trim.ToUpper) & "'"
            End If
        End If

        If txtSupName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othgrpname) = '" & Trim(txtSupName.Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(othgrpname) = '" & Trim(txtSupName.Text.Trim.ToUpper) & "'"
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



            'strSqlQry = "select h.tplistcode,a.airportbordername,ah.mktcode airportbordercode,isnull(s.mktcode,'')  othtypcode,isnull(t.othtypname,'') othtypname, h.currcode,isnull(h.applicableto,'') applicableto,status=	case when isnull(approved,0)=1 then 'Yes' else 'No' End, " _
            '    & " h.frmdate,h.todate,h.adddate,h.adduser,h.moddate,h.moduser from trfplist_header h cross apply dbo.splitallotmkt(h.airportcode,',') ah left join airportbordersmaster  a on ah.mktcode= a.airportbordercode cross apply dbo.splitallotmkt(h.sectorcode,',') s   " _
            '    & " left join othtypmast t on  t.othtypcode=s.mktcode where shifting=0 "


            strSqlQry = "select  h.ocplistcode,  h.othgrpcode,o.othgrpname,h.currcode,p.partyname ,  min(d.frmdate) frmdate ,max(d.todate)todate ,h.adddate,h.adduser,h.moddate,h.moduser,  " _
                & " status=	case when isnull(h.approve,0)=1 then 'Yes' else 'No' End from oplist_costh h inner join othgrpmast o on h.othgrpcode=o.othgrpcode inner join partymast p on h.partycode=p.partycode  inner join othcostplisth_dates d  on h.ocplistcode= d.ocplistcode " _
                & " where h.othgrpcode='" & hdDefault_Group.Value & "'  group by  h.ocplistcode,  h.othgrpcode,o.othgrpname,p.partyname,h.currcode,h.approve,h.adddate,h.adduser,h.moddate,h.moduser "



            Dim pliststr As String = ""


            If pliststr <> "" Then
                strSqlQry = strSqlQry & " WHERE ocplistcode='" & pliststr & "'"

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

        Catch ex As Exception

            objUtils.WritErrorLog("OtherservicescostPriceListsearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub


#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        '  Session.Add("State", "New")
        Session("ClearPrice") = "No"
        Dim strpop As String = ""
        'strpop = "window.open('OthPriceList1.aspx?State=New','OthPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ' strpop = "window.open('OthPriceList1.aspx?State=New&Group='" + hdDefault_Group.Value + "','OthPriceList1');"

        strpop = "window.open('OthMACostpricelist1.aspx?State=New&Group=" + CType(hdDefault_Group.Value, String) + "','Othpricelist1');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

#End Region

 

   

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("ocplistcode", "DESC")

            Case 1
                FillGrid("partyname", "ASC")

            Case 2
                FillGrid("currcode", "ASC")

            Case 3
                FillGrid("othgrpname", "ASC")

            Case 4
                FillGrid("othgrpcode", "ASC")


        End Select
    End Sub

#End Region



#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")


            If e.CommandName = "Editrow" Then

                Dim strpop As String = ""
                'strpop = "window.open('OthPriceList1.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OthPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthMAcostpricelist1.aspx?State=Edit&Group=" + CType(hdDefault_Group.Value, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Othpricelist1');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesCostPriceList2.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('OthPriceList2.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','TrfPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthMAcostpricelist1.aspx?State=View&Group=" + CType(hdDefault_Group.Value, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Othpricelist1');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            ElseIf e.CommandName = "Deleterow" Then
                ''Session.Add("State", "Delete")
                ''Session.Add("RefCode", CType(lblId.Text.Trim, String))
                ''Response.Redirect("OtherServicesCostPriceList2.aspx", False)



                Dim strpop As String = ""
                'strpop = "window.open('OthPriceList2.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','TrfPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthMAcostpricelist1.aspx?State=Delete&Group=" + CType(hdDefault_Group.Value, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Othpricelist1');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesCostPriceList1.aspx", False)

                Dim strpop As String = ""
                'strpop = "window.open('OthPriceList1.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','OthPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OthMAcostpricelist1.aspx?State=Copy&Group=" + CType(hdDefault_Group.Value, String) + "&RefCode=" + CType(lblId.Text.Trim, String) + "','Othpricelist1');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherservicescostPriceListsearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtContractId.Text = ""

        txtSupName.Text = ""


        ddlSPTypeCD.Value = "[Select]"
        ddlSPTypeNM.Value = "[Select]"

        ddlSupplierAgent.Value = "[Select]"
        ddlSuppierAgentNM.Value = "[Select]"


        ddlCurrencyCD.Value = "[Select]"
        ddlCurrencyNM.Value = "[Select]"


        ddlSubSeas.Value = "[Select]"
        ddlSubSeasNM.Value = "[Select]"



        ViewState("MyAutoNo") = 1
        ddlOrderBy.SelectedIndex = 0
        ' FillGrid("cplisthnew.plistcode", "DESC")
        FillGrid("ocplistcode", "DESC")
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
#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            'Dim lblSectorName As Label = e.Row.FindControl("lblSectorName")
            'Dim lblSectorGroupName As Label = e.Row.FindControl("lblSectorGroupName")
            'Dim lblCountryName As Label = e.Row.FindControl("lblCountryName")
            'Dim lblCityName As Label = e.Row.FindControl("lblCityName")

            Dim lblplistcode As Label = e.Row.FindControl("lblplistcode")
            Dim lblexctypecode As Label = e.Row.FindControl("lblexctypecode")
            Dim lblothtypname As Label = e.Row.FindControl("lblothtypname")
            Dim lblcurrcode As Label = e.Row.FindControl("lblcurrcode")

            Dim lblothcatcode As Label = e.Row.FindControl("lblothcatcode")
            Dim lblothcatname As Label = e.Row.FindControl("lblothcatname")



            Dim lblapplicable As Label = e.Row.FindControl("lblapplicable")




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

                        If "PRICELIST" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "EXCURSIONS" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "VEHICLE" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
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
                        'If lsSearchTextCity.Trim <> "" Then
                        '    lblpartyname.Text = Regex.Replace(lblpartyname.Text.Trim, lsSearchTextCity.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '                    RegexOptions.IgnoreCase)
                        'End If
                        'If lsSearchTextSector.Trim <> "" Then
                        '    lblsubagentname.Text = Regex.Replace(lblsubagentname.Text.Trim, lsSearchTextSector.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '                    RegexOptions.IgnoreCase)
                        'End If

                        'If lsSearchTextSectorGroup.Trim <> "" Then
                        '    lblstatus.Text = Regex.Replace(lblstatus.Text.Trim, lsSearchTextSectorGroup.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '                    RegexOptions.IgnoreCase)
                        'End If


                    Next
                End If
            End If
            'If lblactive.Text <> "Active" Then
            '    e.Row.BackColor = Drawing.Color.Lavender
            'End If



        End If
    End Sub
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        '  FillGrid(e.SortExpression, direction)
        Session.Add("strMACsortExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region


#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(ViewState("strMACsortExpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strMACsortdirection", objUtils.SwapSortDirection(Session("strMACsortdirection")))
            dataView.Sort = ViewState("strMACsortExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strMACsortdirection"))
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

                'strSqlQry = "select h.eplistcode,h.exctypcode,c.exctypname exctypname,h.vehiclecode othcatcode,t.othcatname, h.currcode,h.applicableto,status=	case when isnull(approved,0)=1 then 'Yes' else 'No' End, " _
                ' & " h.frmdate,h.todate,h.adddate,h.adduser,h.moddate,h.moduser from excmulticplist_header h,excursiontypes c ,othcatmast t  where h.exctypcode=c.exctypcode and  h.vehiclecode=t.othcatcode  " _
                ' & " and isnull(c.multicost,'')='YES'  "

                strSqlQry = "select distinct h.tplistcode,  airportbordername=(select distinct stuff((select  ',' + u.airportbordername   from  trfplist_header h1 cross apply dbo.splitallotmkt(h1.airportcode,',') ah1  left join airportbordersmaster u on ah1.mktcode= u.airportbordercode  " _
               & "   where h1.tplistcode=h.tplistcode for xml path('')),1,1,'')), h.airportcode airportbordercode ,case when isnull(h.shifting,0)=1 then 'Yes' else 'No' end   shifting , isnull(h.sectorcode,'') othtypcode, othtypname=(select distinct stuff((select  ',' + u2.othtypname   from   " _
               & " trfplist_header h2 cross apply dbo.splitallotmkt(h2.sectorcode,',') ah2 left join othtypmast u2 on ah2.mktcode= u2.othtypcode    where h2.tplistcode=h.tplistcode for xml path('')),1,1,'')),h.currcode,isnull(h.applicableto,'') applicableto,status=	case when isnull(approved,0)=1 then 'Yes' else 'No' End, " _
           & " h.frmdate,h.todate,h.adddate,h.adduser,h.moddate,h.moduser from trfplist_header h cross apply dbo.splitallotmkt(h.airportcode,',') ah left join airportbordersmaster  a on ah.mktcode= a.airportbordercode cross apply dbo.splitallotmkt(h.sectorcode,',') s   " _
           & " left join othtypmast t on  t.othtypcode=s.mktcode where 1=1  "



                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & ""
                Else
                    strSqlQry = strSqlQry & " ORDER BY tplistcode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "transferprice")

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

        'Record list will be according to the Changing the year  
        'If Not (Session("changeyear") Is Nothing) Then
        '    frmdate = CDate(Session("changeyear") + "/01" + "/01")

        '    If Session("changeyear") = Year(Now).ToString Then
        '        If txtPtodate.Text = "" Then
        '            todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
        '        Else
        '            todate = CType(txtPtodate.Text, Date).ToString
        '        End If
        '    Else
        '        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
        '    End If

        '    txtPfromdate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
        '    txtPtodate.Text = Format(CType(todate, Date), "dd/MM/yyy")

        'Else
        '    txtPfromdate.Text = ""
        '    txtPtodate.Text = ""
        'End If



        'Record list will be according to the Changing the year  
        'If Session("changeyear") <> Year(CType(txtPfromdate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        'If Session("changeyear") <> Year(CType(txtPtodate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If



        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("ocplistcode", "DESC")
                '   FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("othgrpname", "ASC")
                '    FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("othgrpcode", "ASC")
                '   FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("ocplistcode", "DESC")
            Case 4
                FillGrid("ocplistcode", "DESC")

        End Select
    End Sub
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=A&MCostPriceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("tplistcode", "DESC")
                '   FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "DESC")
            Case 1
                FillGrid("exctypname", "ASC")
                '    FillGrid("cplisthnew.plistcode", "edit_cplisthnew.plistcode", "ASC")
            Case 2
                FillGrid("othcatcode", "ASC")
                '   FillGrid("cplisthnew.partycode", "edit_cplisthnew.partycode", "ASC")
            Case 3
                FillGrid("othcatname", "ASC")
                '  FillGrid("partymast.partyname", "partymast.partyname", "ASC")
            Case 4
                FillGrid("exctypcode", "ASC")
        End Select
        SetFocus(ddlOrderBy)
    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionMulticostSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ExcursionMulticostSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        Dim lsProcesssupplier As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "PRICELIST"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("PRICELIST", lsProcessCity, "PRICELIST")
                Case "SUPPLIER"
                    lsProcesssupplier = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SUPPLIER", lsProcesssupplier, "SUPPLIER")
                Case "SECTOR"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SECTOR", lsProcessCountry, "SECTOR")
                    'Case "CLASSIFICATION"
                    '    lsProcessGroup = lsMainArr(i).Split(":")(1)
                    '    sbAddToDataTable("CLASSIFICATION", lsProcessGroup, "CLASSIFICATION")
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
        Dim strSuppvalue As String = ""
        Try

            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "SUPPLIER" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                        If strSuppvalue <> "" Then
                            strSuppvalue = strSuppvalue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSuppvalue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "PRICELIST" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    'If dtt.Rows(i)("Code").ToString = "CLASSIFICATION" Then
                    '    If strSectorValue <> "" Then
                    '        strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    '    Else
                    '        strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    '    End If
                    'End If
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
            End If
            Dim pagevaluess = getRowpage()
            strBindCondition = BuildConditionNew(strCountryValue, strCityValue, strSectorValue, strSectorGroupValue, strTextValue, strSuppvalue)

            Dim myDS As New DataSet
            Dim strValue As String

            gv_SearchResult.Visible = True
            lblMsg.Visible = False

            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = ViewState("strMACsortExpression") 'Session("strMACsortExpression")
            Dim strsortorder As String = "ASC"





            strSqlQry = "select  h.ocplistcode,  h.othgrpcode,o.othgrpname,h.currcode,p.partyname ,  min(d.frmdate) frmdate ,max(d.todate)todate ,h.adddate,h.adduser,h.moddate,h.moduser,  " _
                & " status=	case when isnull(h.approve,0)=1 then 'Yes' else 'No' End from oplist_costh h inner join othgrpmast o on h.othgrpcode=o.othgrpcode inner join partymast p on h.partycode=p.partycode  inner join othcostplisth_dates d  on h.ocplistcode= d.ocplistcode " _
                & " where h.othgrpcode='" & hdDefault_Group.Value & "'" '  group by  h.ocplistcode,  h.othgrpcode,o.othgrpname,p.partyname,h.currcode,h.approve,h.adddate,h.adduser,h.moddate,h.moduser "




            'strSqlQry = "select  h.oplistcode,  h.othgrpcode,o.othgrpname,h.currcode,h.applicableto,  min(d.frmdate) frmdate ,max(d.todate)todate ,h.adddate,h.adduser,h.moddate,h.moduser,  " _
            '    & " status=	case when isnull(h.approve,0)=1 then 'Yes' else 'No' End from othplisth h inner join othgrpmast o on h.othgrpcode=o.othgrpcode  inner join othplisth_dates d  on h.oplistcode= d.oplistcode " _
            '    & " where h.othgrpcode='" & hdDefault_Group.Value & "'  "



            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & "  and " & strBindCondition & "  group by  h.ocplistcode,  h.othgrpcode,o.othgrpname,p.partyname,h.currcode,h.approve,h.adddate,h.adduser,h.moddate,h.moduser  ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & "   group by  h.ocplistcode,  h.othgrpcode,o.othgrpname,p.partyname,h.currcode,h.approve,h.adddate,h.adduser,h.moddate,h.moduser  ORDER BY " & strorderby & " " & strsortorder
            End If

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

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherservicescostPriceListsearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strSectorValue As String, ByVal strSectorGroupValue As String, ByVal strTextValue As String, ByVal strSuppvalue As String) As String
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
                    strWhereCond = " isnull(approve,0)=1"
                ElseIf Replace(strSectorGroupValue, "'", "") = "UNAPPROVED" Then
                    strWhereCond = " isnull(approve,0)=0"
                Else
                    strWhereCond = " isnull(approve,0) IN (0,1)"
                End If
            Else
                If Replace(strSectorGroupValue, "'", "") = "APPROVED" Then
                    strWhereCond = strWhereCond & " AND   isnull(approve,0)=1"
                ElseIf Replace(strSectorGroupValue, "'", "") = "UNAPPROVED" Then
                    strWhereCond = strWhereCond & " AND  isnull(approve,0)=0"
                Else
                    strWhereCond = strWhereCond & " AND  isnull(approve,0) IN (0,1)"
                End If
                'strWhereCond = strWhereCond & " AND  upper(status) IN ( " & Trim(strSectorGroupValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCountryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(partyname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(partyname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If

        If strSuppvalue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othgrpname) IN (" & Trim(strSuppvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND  upper(othgrpname) IN (" & Trim(strSuppvalue.Trim.ToUpper) & ")"
            End If
        End If



        If strCityValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(h.ocplistcode) IN ( " & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(h.ocplistcode) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = " (upper(h.ocplistcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othgrpname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(approve) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  (upper(h.ocplistcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othgrpname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(approve) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
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
                    strWhereCond = "((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),frmdate,111)  " _
                              & "  and convert(varchar(10),todate,111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                              & "  between convert(varchar(10),frmdate,111)  and  convert(varchar(10),todate,111))   " _
                              & " or (convert(varchar(10),frmdate,111) >= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                              & "  and convert(varchar(10),todate,111) <= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ))"

                    '  strWhereCond = " (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & " and ((convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) between  convert(varchar(10),frmdate,111)  " _
                       & "  and convert(varchar(10),todate,111)) or   (convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)  " _
                       & "  between convert(varchar(10),frmdate,111)  and  convert(varchar(10),todate,111))   " _
                       & " or (convert(varchar(10),frmdate,111) >= convert(varchar(10), '" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                       & "  and convert(varchar(10),todate,111) <= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)))"

                    'strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function




    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Session.Add("newappid", Request.QueryString("appid"))
            End If
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            ElseIf Session("newappid") Is Nothing = False Then
                Dim appid1 As String = CType(Session("newappid"), String)
                Select Case appid1
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
End Class
